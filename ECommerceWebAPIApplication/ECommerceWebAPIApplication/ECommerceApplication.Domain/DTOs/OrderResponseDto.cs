using ECommerceApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Domain.DTOs
{
    public class OrderResponseDto
    {
        public Customer Customer { get; set; }
        public Order Order { get; set; }
    }
}
