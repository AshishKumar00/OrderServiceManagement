using Microsoft.EntityFrameworkCore;
using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using OrderServiceManagement.Domain.Models;
using OrderServiceManagement.Infrastructure.Data;

namespace OrderServiceManagement.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly OrderDbContext _context;

        public OrderRepository(OrderDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }

        public async Task AddAsync(OrderItem orderItem)
        {
            await _context.OrderItems.AddAsync(orderItem);
        }

        public async Task CreateOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);
        }


        public async Task<Order> GetById(Guid orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(x => x.OrderId == orderId);
        }

        public async Task<List<OrderResponseDto>> GetOrdersByUserId(Guid userId, int pageNumber, int pageSize)
        {
            var orderresList = new List<OrderResponseDto>();
            var  res = await _context.Orders
                .Where(x => x.UserId == userId)
                .Include(x => x.OrderItems)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            foreach (var order in res)
            {
                var orderDto = new OrderResponseDto();
                orderDto.OrderItems = new List<OrderItemResponseDto>();
                orderDto.OrderId = order.OrderId;
                orderDto.UserId = order.UserId;
                orderDto.OrderStatus = order.OrderStatus;
                orderDto.CreatedAt = order.CreatedAt;
                foreach (var item in order.OrderItems)
                {
                    var orderItemDto = new OrderItemResponseDto();
                    orderItemDto.OrderItemId = item.OrderItemId;
                    orderItemDto.ProductId = item.ProductId;
                    orderItemDto.Quantity = item.Quantity;
                    orderDto.OrderItems.Add(orderItemDto);
                }
                orderresList.Add(orderDto);
            }

            return orderresList;
        }
       
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
