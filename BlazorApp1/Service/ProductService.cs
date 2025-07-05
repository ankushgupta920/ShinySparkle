using BlazorApp1.Pages;
using BlazorApp1.Service.IService;
using Newtonsoft.Json;
using System.Net.Http;

namespace BlazorApp1.Modals
{
    public class ProductService : IProductService
    {
        private readonly HttpClient client;

        public ProductService(HttpClient client)
        {
            this.client = client;
        }

        public async Task<ProductVM> GetProductByIdAsync(int id)
        {
            var response = await client.GetAsync($"api/Products/{id}");
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ProductVM>(content);
            return data;
        }

        public async Task<IEnumerable<ProductVM>> GetProductsAsync()
        {
            var response = await client.GetAsync("api/Products");
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(content);
            return data;
        }

        public async Task<IEnumerable<ProductVM>> GetProductsByCategoryAsync(string category)
        {
            var response = await client.GetAsync($"api/Products/GetProductsByCategory/{category}");
            var content = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<IEnumerable<ProductVM>>(content);
            return data;
        }
    }
}