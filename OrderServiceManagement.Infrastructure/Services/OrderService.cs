using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Domain.Models;
using OrderServiceManagement.Infrastructure.Data;

namespace OrderServiceManagement.Infrastructure.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _repository;

        private readonly IProductService _inventoryService;

        private readonly OrderDbContext _context;
        private readonly AuthDbContext _authContext;

        public OrderService(
            IOrderRepository repository,
            IProductService inventoryService,
            OrderDbContext context,
            AuthDbContext authContext
            )
        {
            _repository = repository;
            _inventoryService = inventoryService;
            _context = context;
            _authContext = authContext;
        }

        public async Task<Dictionary<bool, string>> CreateOrderAsync(CreateOrderRequest request, string userId)
        {

            var stockReduced = await _inventoryService.ReduceStock(request.ProductId, request.Quantity);

            try
            {
                var orderId = Guid.NewGuid();

                var order = new Order
                {
                    OrderId = orderId,
                    UserId = Guid.Parse(userId),
                    CreatedAt = DateTime.UtcNow,
                };
                var orderItem = new OrderItem
                {
                    OrderId = orderId,
                    OrderItemId = Guid.NewGuid(),
                    ProductId = request.ProductId,
                    Quantity = request.Quantity
                };

                if (stockReduced.ContainsKey(false))
                {
                    order.OrderStatus = "CANCELLED";
                    await _repository.AddAsync(order);

                    return new Dictionary<bool, string> {
                { false, stockReduced.GetValueOrDefault(false) }
                };
                }
                //result.Add(false, $"Insufficient Product Quantity. You can purchase Maximum {product.StockQty} products");

                else
                {
                    order.OrderStatus = "CREATED";
                    await _repository.AddAsync(order);
                    await _repository.AddAsync(orderItem);
                }
                await _repository.SaveChangesAsync();
               //bool rollbackResult = await _inventoryService.RollbackStock(request.ProductId, request.Quantity);

                return new Dictionary<bool, string> {
                { true, "Order created successfully" }
                };
            }
            catch (Exception ex)
            {
                // rollback
                bool rollbackResult = await _inventoryService.RollbackStock(request.ProductId, request.Quantity);
                if(rollbackResult)
                {
                    return new Dictionary<bool, string> {{ false, "Order creation failed, stock rollback successful" }};
                }
                else
                {
                    return new Dictionary<bool, string> {{ false, "Order creation failed, stock rollback failed" }};
                }
            } 
           
        }

        public async Task<List<OrderResponseDto>> GetMyOrders(Guid userId, int pageNumber, int pageSize)
        {
            return await _repository.GetOrdersByUserId(userId,pageNumber,pageSize);
        }

        public async Task<Order> GetOrderById(Guid orderId)
        {
        
                var order = await _repository.GetById(orderId);

                if (order == null)
                    throw new Exception("Order not found");

                return order;
            

           
        }

        public async Task CancelOrder(Guid orderId)
        {
            var order =await _repository.GetById(orderId);

            if (order == null)
                throw new Exception("Order not found");

            if (order.OrderStatus == "CANCELLED")
                throw new Exception(
                    "Order already cancelled");

            // RESTORE STOCK

            foreach (var item in order.OrderItems)
            {
                await _inventoryService.RollbackStock(
                    item.ProductId,
                    item.Quantity);
            }

            order.OrderStatus = "CANCELLED";

            await _repository.SaveChangesAsync();
        }

      
    }
}

