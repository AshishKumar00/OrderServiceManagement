using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Domain.Models;


namespace OrderServiceManagement.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task CreateOrderAsync(Order order);

        Task SaveChangesAsync();
        Task AddAsync(Order order);
        Task AddAsync(OrderItem orderItem);

        Task<Order> GetById(Guid orderId);

        Task<List<OrderResponseDto>> GetOrdersByUserId(Guid userId, int pageNumber, int pageSize);

    }
}
