using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Domain.Models
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid UserId { get; set; }
        
        public string OrderStatus { get; set; }

        public DateTime CreatedAt { get; set; }

        public ICollection<OrderItem> OrderItems { get; set; }= new List<OrderItem>();
    }
}
