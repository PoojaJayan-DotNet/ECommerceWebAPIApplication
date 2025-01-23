using ECommerceApplication.Domain.DTOs;
using ECommerceApplication.Domain.Models;
using ECommerceApplication.Infrastructure.Persistence.DataContext;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using System.Collections;
using System.Text.Json.Serialization;
using Microsoft.Extensions.Options;
using System.Text.Json;
using ECommerceWebAPIApplication.NewFolder;

namespace ECommerceApplication.Infrastructure.Persistence.Repository
{
    public class CustomerRepository :  ICustomerRepository
    {
        private readonly DbName _dbName;
        private readonly DapperDataContext _dapperContext;
        public CustomerRepository(DbName dbName,DapperDataContext dapperDataContext) 
        {
            _dbName = dbName;
            _dapperContext = dapperDataContext;
        }

        public async Task<DbConnectionTestResponse> CheckDbConnection()
        {
            var response = new DbConnectionTestResponse();
            try
            {               
                var testQuery = $"SELECT COUNT(PRODUCTID) FROM [{_dbName.ECommerceDB}].[dbo].PRODUCTS";


                using (var connection = _dapperContext.DefaultConnection)
                {
                    if (await connection.ExecuteAsync(testQuery, commandType: CommandType.Text) != null)
                        response.EcommerceDbConnectionStatus = "Connected";
                }
                
            }
            catch(Exception ex)
            {

            }
            return response;
        }

        public async Task<bool> CheckIfValidCustomer(OrderRequestDto orderRequestDto)
        {
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", orderRequestDto.CutomerId, DbType.Int32, ParameterDirection.Input, size: 50);
            parameters.Add("@Email", orderRequestDto.Email, DbType.String, ParameterDirection.Input, size: 50);


            var dbName = _dbName.ECommerceDB;
            var testQuery = $@" SELECT COUNT(CUSTOMERID) 
                                FROM [{dbName}].[dbo].[CUSTOMERS] 
                                WHERE CUSTOMERID = @CustomerId AND EMAIL = @Email";
         
            using (var connection = _dapperContext.DefaultConnection)
            {
                if (await connection.ExecuteScalarAsync<int>(testQuery,parameters, commandType: CommandType.Text) > 0)
                    return true;
            }
            return false;
        }

        public async Task<OrderResponseDto> GetRecentOrder(OrderRequestDto orderRequest)
        {
            var response = new OrderResponseDto();
            var parameters = new DynamicParameters();
            parameters.Add("@CustomerId", orderRequest.CutomerId, DbType.Int32, ParameterDirection.Input, size: 50);
            parameters.Add("@Email", orderRequest.Email, DbType.String, ParameterDirection.Input, size: 50);


            var quey = @"WITH LatestOrder AS (
                        SELECT TOP 1 
                            o.OrderId,
                            o.CustomerId,
                            o.OrderDate,
                            o.DeliveryExpected,
                            c.Firstname,
                            c.Lastname,
                            CONCAT(c.HouseNo, ' ', c.Street, ', ', c.Town, ', ', c.PostCode) AS DeliveryAddress,
                            o.ContainsGift
                        FROM Customers c
                        LEFT JOIN Orders o ON o.CustomerId = c.CustomerId
                        WHERE c.CustomerId = @CustomerId AND c.Email = @Email
                        ORDER BY o.OrderDate DESC
                    ),
                    OrderDetails AS (
                        SELECT 
                            oi.OrderId,
                            CASE 
                                WHEN lo.ContainsGift = 1 THEN 'Gift' 
                                ELSE p.ProductName 
                            END AS Product,
                            oi.Quantity,
                            oi.Price AS PriceEach
                        FROM OrderItems oi
                        INNER JOIN Products p ON oi.ProductId = p.ProductId
                        INNER JOIN LatestOrder lo ON lo.OrderId = oi.OrderId
                    )
                    SELECT 
                        JSON_QUERY(
                            (
                                SELECT 
                                    (
                                        SELECT 
                                            c.Firstname AS [firstName],
                                            c.Lastname AS [lastName]
                                        FROM Customers c
                                        WHERE c.CustomerId = @CustomerId AND c.Email = @Email
                                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                                    ) AS Customer,
                                    (
                                        SELECT 
                                            lo.OrderId AS [orderNumber],
                                            FORMAT(lo.OrderDate, 'dd-MMM-yyyy') AS [orderDate],
                                            lo.DeliveryAddress AS [deliveryAddress],
                                            (
                                                SELECT 
                                                    od.Product AS [product],
                                                    od.Quantity AS [quantity],
                                                    od.PriceEach AS [priceEach]
                                                FROM OrderDetails od
                                                FOR JSON PATH
                                            ) AS [orderItems],
                                            FORMAT(lo.DeliveryExpected, 'dd-MMM-yyyy') AS [deliveryExpected]
                                        FROM LatestOrder lo
                                        WHERE lo.OrderId IS NOT NULL -- Include only if an order exists
                                        FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                                    ) AS [Order]
                                FOR JSON PATH, WITHOUT_ARRAY_WRAPPER
                            )
                        ) AS Result;";
            var dbName = _dbName.ECommerceDB;

            using (var connection = _dapperContext.DefaultConnection)
            {
                var data = await connection.QuerySingleAsync<string>(quey, parameters, commandType: CommandType.Text);
                return Helpers.MapJsonToDto(data);
            }

        }
    }
}
