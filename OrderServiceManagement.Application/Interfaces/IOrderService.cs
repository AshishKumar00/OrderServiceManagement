using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Application.Interfaces
{
    public interface IOrderService
    {
       // Task<string> CreateOrderAsync(string userName, CreateOrderDto dto);
        Task<Dictionary<bool, string>> CreateOrderAsync(CreateOrderRequest request, string userId);

        Task<List<OrderResponseDto>> GetMyOrders(Guid userId, int pageNumber, int pageSize);

        Task<Order> GetOrderById(Guid orderId);

       Task CancelOrder(Guid orderId);


    }
}
