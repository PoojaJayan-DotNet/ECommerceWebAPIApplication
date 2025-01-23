using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerceApplication.Domain.Models
{
    public class Order
    {
        public int OrderNumber { get; set; }
        public string OrderDate { get; set; }
        public string DeliveryAddress { get; set; }
        public IEnumerable<OrderItem> OrderItems { get; set; }

        public string DeliveryExpected { get; set; }
    }
}
