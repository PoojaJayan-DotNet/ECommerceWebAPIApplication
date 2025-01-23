using Azure;
using ECommerceApplication.Domain.DTOs;
using ECommerceApplication.Domain.Models;
using ECommerceApplication.Infrastructure.Persistence.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace ECommerceWebAPIApplication.Service
{
    public class CustomerService
    {
        private readonly ICustomerRepository _customerRepository;
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ICustomerRepository customerRepository,ILogger<CustomerService> logger) 
        { 
            _customerRepository = customerRepository;
            _logger = logger;
        }
        public async Task<bool> CheckIfValidCustomer(OrderRequestDto orderRequestDto)
        {
            try
            {
                return await _customerRepository.CheckIfValidCustomer(orderRequestDto);
            }
            catch (Exception ex) {
                _logger.LogError(ex.Message);
            }
            return false;
        }

        public async Task<OrderResponseDto> GetMostRecentOrderByCustomer(OrderRequestDto orderRequestDto)
        {
            try
            {
                return await _customerRepository.GetRecentOrder(orderRequestDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;

        }
        public async Task<DbConnectionTestResponse> CheckDbConnection()
        {
            try
            {
                return await _customerRepository.CheckDbConnection();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
            return null;
        }
    }
}
