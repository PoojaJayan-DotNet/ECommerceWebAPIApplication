
using Azure;
using ECommerceApplication.Domain.DTOs;
using ECommerceWebAPIApplication.Service;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System.Net;

namespace ECommerceWebAPIApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerService _customerService;
        private readonly ILogger<CustomerController> _logger;
        public CustomerController(CustomerService customerService,ILogger<CustomerController> logger)
        { 
            _customerService = customerService;
            _logger = logger;
        }
        [HttpGet]
        [Route("/testapi")]
        public IActionResult TestAPI()
        {
            try
            {
                return Ok("API is working");
            }
            catch (Exception ex)
            {
                Log.Error($"Error - {ex.Message.ToString()} \n {ex.StackTrace.ToString()}");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Something went wrong.Please try again!",
                    Detail = ex.Message,
                    Status = 500
                });
            }

        }
        [HttpGet]
        [Route("/dbconnectintest")]
        public async Task<IActionResult> TestDbConnection()
        {
            try
            {
                return Ok(await _customerService.CheckDbConnection());
            }
            catch (Exception ex)
            {
                Log.Error($"Error - {ex.Message.ToString()} \n {ex.StackTrace.ToString()}");

                return StatusCode(500, new ProblemDetails
                {
                    Title = "Something went wrong.Please try again!",
                    Detail = ex.Message,
                    Status = 500
                });
            }
        }

        [HttpPost]
        [Route("/getrecentorder")]
        public async Task<IActionResult> GetRecentOrder(OrderRequestDto requestDto)
        {
            try
            {
                
                if (await _customerService.CheckIfValidCustomer(requestDto))
                {
                    var orderDetails = await _customerService.GetMostRecentOrderByCustomer(requestDto);
                    return Ok(orderDetails);
                }

                return StatusCode(404, new ProblemDetails
                {
                    Title = "No customer with the given data is found",
                    Detail = "Not Found",
                    Status = 404
                });
            }
            catch (Exception ex)
            {
                Log.Error($"Error - {ex.Message.ToString()} \n {ex.StackTrace.ToString()}");
                return StatusCode(500, new ProblemDetails
                {
                    Title = "Something went wrong.Please try again!",
                    Detail = ex.Message,
                    Status = 500
                });
            }
        }
    }
}
