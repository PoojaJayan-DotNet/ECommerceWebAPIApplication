using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Domain.Models
{
    public class Product
    {
        public int Quantity { get; set; }
        public string ProductName { get; set; }
        public decimal PriceEach { get; set; }
    }
}
