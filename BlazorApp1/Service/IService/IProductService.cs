using BlazorApp1.Modals;

namespace BlazorApp1.Service.IService
{
    public interface IProductService
    {
       public Task<IEnumerable<ProductVM>> GetProductsAsync();
       public Task<ProductVM> GetProductByIdAsync(int id);
       public Task<IEnumerable<ProductVM>> GetProductsByCategoryAsync(string category);

    }
}
