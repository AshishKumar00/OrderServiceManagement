using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Application.Interfaces
{
    public interface IProductService
    {
        // Task<bool> ReduceStock(Guid productId, int quantity);
        Task<Dictionary<bool, String>> ReduceStock(Guid productId, int quantity);
        Task<bool> RollbackStock(Guid productId, int quantity);

    }
}
