using BlazorApp1.Modals;
using DataAcess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository.IRepository
{
    public interface IProductListRepository
    {
        //Add
        public Task<ProductVM> CreateProduct(ProductVM productVM);
        //Update
        public Task<ProductVM> UpdateProduct(int Id, ProductVM productVM);
        //GetByID
        public Task<ProductVM> GetProductByID(int Id);
        //GetBycategory
        public Task<IEnumerable<ProductVM>> GetProductByCategory(string Category);

        //Delete
        public Task<int> DeleteProduct(int Id);
        //GetAll
        public Task<IEnumerable<ProductVM>> GetAllProduct();
        // Check If Product Name is Unique
        public Task<ProductVM> IsProductUnique(string name, int Id = 0);
    }
}
