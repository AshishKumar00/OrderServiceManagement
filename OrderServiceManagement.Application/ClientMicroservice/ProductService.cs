using OrderServiceManagement.Application.DTOs;
using OrderServiceManagement.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace OrderServiceManagement.Application.ClientMicroservice
{
    public class ProductService : IProductService
    {
        private readonly HttpClient _httpClient;

        public ProductService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }


        public async Task<Dictionary<bool, String>> ReduceStock(Guid productId, int quantity)
        {
            var request = new ReduceStockDto
            {
                ProductId = productId,
                Quantity = quantity
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7273/api/Product/reduce_stock", request);

            var result = await response.Content.ReadFromJsonAsync<Dictionary<bool, string>>();
            return result;
        }

        public async Task<bool> RollbackStock(Guid productId, int quantity)
        {
            var request = new ReduceStockDto
            {
                ProductId = productId,
                Quantity = quantity
            };

            var response = await _httpClient.PostAsJsonAsync("https://localhost:7273/api/Product/restore_stock", request);

            var result = await response.Content.ReadFromJsonAsync<bool>();
            return result;


        }


     
       


    }
}
