using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Infrastructure.Services;
using System.Security.Claims;

namespace OrderServiceManagement.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;
        private readonly ILogger<OrderController> _logger;

        public OrderController(IOrderService orderService, ILogger<OrderController> logger)
        {
            _orderService = orderService;
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            try
            {
                _logger.LogInformation("CreateOrder called for ProductId={ProductId}, Quantity={Quantity}", request.ProductId, request.Quantity);

                var userId = User.Claims
                    .FirstOrDefault(x => x.Type == "UserId")
                    ?.Value;

                if (string.IsNullOrWhiteSpace(userId))
                {
                    _logger.LogWarning("CreateOrder failed: missing UserId claim");
                    return Unauthorized("User is not authenticated");
                }

                var result = await _orderService.CreateOrderAsync(request, userId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating order for ProductId={ProductId}", request?.ProductId);
                return Problem(detail: ex.Message, title: "Failed to create order", statusCode: StatusCodes.Status500InternalServerError);
            }
        }


        [HttpGet("my-orders")]
        public async Task<IActionResult> MyOrders(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var claim = User.Claims.FirstOrDefault(x => x.Type == "UserId")?.Value;
                if (!Guid.TryParse(claim, out var userId))
                {
                    _logger.LogWarning("MyOrders failed: invalid or missing UserId claim");
                    return Unauthorized("Invalid user");
                }

                _logger.LogInformation("Fetching orders for UserId={UserId} Page={PageNumber} Size={PageSize}", userId, pageNumber, pageSize);
                var orders = await _orderService.GetMyOrders(userId, pageNumber, pageSize);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching orders for user");
                return Problem(detail: ex.Message, title: "Failed to fetch orders", statusCode: StatusCodes.Status500InternalServerError);
            }
        }

        [HttpGet("GetById")]
        public async Task<IActionResult> GetById(Guid id)
        {
            //return Ok(await _orderService.GetOrderById(id));
            try
            {
                _logger.LogInformation("GetById called for OrderId={OrderId}", id);
                var order = await _orderService.GetOrderById(id);
                return Ok(order);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching order with id={OrderId}", id);
                return Problem(detail: ex.Message, title: "Failed to fetch order", statusCode: StatusCodes.Status500InternalServerError);
            }
        }



        [HttpPatch("{id}/cancel")]
        public async Task<IActionResult> Cancel(Guid id)
        {
            try
            {
                _logger.LogInformation("Cancel called for OrderId={OrderId}", id);

                await _orderService.CancelOrder(id);

                return Ok("Order cancelled successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error cancelling order with id {OrderId}", id);
                return Problem(detail: ex.Message, title: "Failed to cancel order", statusCode: StatusCodes.Status500InternalServerError);

            }
        }
    }
}
