using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Infrastructure.Services;
using System.Security.Claims;

namespace OrderServiceManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var userId = User.Claims
                .FirstOrDefault(x => x.Type == "UserId")
                ?.Value;
            var result = await _orderService.CreateOrderAsync(request, userId);
            return Ok(result);
        }


        [Authorize(Roles = "ADMIN")]
        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders(int pageNumber = 1, int pageSize = 10)
        {
            var userId = Guid.Parse(User.Claims.First(x => x.Type == "UserId").Value);

            return Ok(await _orderService.GetMyOrders(userId, pageNumber, pageSize));
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            return Ok(await _orderService.GetOrderById(id));
        }

        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            await _orderService.CancelOrder(id);

            return Ok("Order cancelled successfully");
        }
    }
}
