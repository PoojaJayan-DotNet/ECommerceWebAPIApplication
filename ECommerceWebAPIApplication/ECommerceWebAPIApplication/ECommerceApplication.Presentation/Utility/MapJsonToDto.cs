using ECommerceApplication.Domain.DTOs;
using ECommerceApplication.Domain.Models;
using Newtonsoft.Json;

namespace ECommerceWebAPIApplication.NewFolder
{
    static class Helpers
    {
        public static OrderResponseDto MapJsonToDto(string jsonResponse)
        {
            var outerObject = JsonConvert.DeserializeObject<Dictionary<string, string>>(jsonResponse);

            
            var customer = JsonConvert.DeserializeObject<Customer>(outerObject["Customer"]);

            // Check if the "Order" key exists and is not null or empty
            Order order = null;
            if (outerObject.ContainsKey("Order") && !string.IsNullOrEmpty(outerObject["Order"]))
            {
                order = JsonConvert.DeserializeObject<Order>(outerObject["Order"]);
            }


            return new OrderResponseDto
            {
                Customer = customer,
                Order = order
            };
        }
    }

}
