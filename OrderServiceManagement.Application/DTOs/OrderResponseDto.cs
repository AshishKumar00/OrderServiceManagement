using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Application.DTOs
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }

        public string OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public List<OrderItemResponseDto> OrderItems { get; set; }
    }

    public class OrderItemResponseDto
    {
        public Guid OrderItemId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
