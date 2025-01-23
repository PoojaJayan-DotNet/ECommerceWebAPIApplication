using ECommerceApplication.Domain.DTOs;
using ECommerceApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Infrastructure.Persistence.Repository
{
    public interface ICustomerRepository : IRepository<Customer>
    {
            Task<OrderResponseDto> GetRecentOrder(OrderRequestDto orderRequest);

        Task<bool> CheckIfValidCustomer(OrderRequestDto orderRequestDto);
        Task<DbConnectionTestResponse> CheckDbConnection();
    }
}
