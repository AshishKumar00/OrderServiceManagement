using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Application.DTOs
{
    public class CreateOrderRequest
    {
        public Guid ProductId { get; set; }

        public int Quantity { get; set; }
    }

    
}

