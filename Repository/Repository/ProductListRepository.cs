using AutoMapper;
using BlazorApp1.Modals;
using BlazorApp1.Pages;
using Business.Repository.IRepository;
using DataAcess;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Business.Repository
{
    public class ProductListRepository : IProductListRepository
    {
        private readonly ApplicationDbContext db;
        private readonly IMapper mapper;
        public ProductListRepository(ApplicationDbContext db, IMapper mapper)
        {
            this.db = db;
            this.mapper = mapper;
        }

        public async Task<ProductVM> CreateProduct(ProductVM productVM)
        {
            Product product = mapper.Map<ProductVM, Product>(productVM);
            var addProduct = await db.AddAsync(product);
            await db.SaveChangesAsync();
            return mapper.Map<Product, ProductVM>(addProduct.Entity);
        }

        public async Task<int> DeleteProduct(int Id)
        {
            var product = await db.Products.FindAsync(Id);

            if (product != null)
            {
                // Delete main product image
                if (!string.IsNullOrEmpty(product.MainImageUrl))
                {
                    string mainImagePath = Path.Combine("wwwroot", product.MainImageUrl);
                    if (File.Exists(mainImagePath))
                    {
                        File.Delete(mainImagePath);
                    }
                }

                // Delete additional images
                if (product.AdditionalImages != null && product.AdditionalImages.Count > 0)
                {
                    foreach (var imageUrl in product.AdditionalImages)
                    {
                        string imagePath = Path.Combine("wwwroot", imageUrl);
                        if (File.Exists(imagePath))
                        {
                            File.Delete(imagePath);
                        }
                    }
                }

                // Remove product from database
                db.Products.Remove(product);
                return await db.SaveChangesAsync();
            }

            return 0;
        }

        public async Task<IEnumerable<ProductVM>> GetAllProduct()
        {
            try
            {
                IEnumerable<ProductVM> ProductVms = mapper.Map
                    <IEnumerable<Product>, IEnumerable<ProductVM>>(await db.Products.ToListAsync());
                return ProductVms;
            }
            catch (Exception ex)
            {
                return null;
            }
            
        }

        public async Task<IEnumerable<ProductVM>> GetProductByCategory(string Category)
        {
            try
            {
                var products = await db.Products
                               .Where(x => x.Category == Category)
                               .ToListAsync();

                return mapper.Map<List<Product>, List<ProductVM>>(products);
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public async Task<ProductVM> GetProductByID(int Id)
        {
            try
            {
                ProductVM ProductVM = mapper.Map<Product, ProductVM>(await db.Products.FirstOrDefaultAsync(x => x.Id == Id));
                return ProductVM;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public Task<ProductVM> IsProductUnique(string name, int Id = 0)
        {
            throw new NotImplementedException();
        }

        public async Task<ProductVM> UpdateProduct(int Id, ProductVM productVM)
        {
            try
            {
                if (Id == productVM.Id)
                {
                    Product ProductDetails = await db.Products.FindAsync(Id);
                    if (ProductDetails == null)
                    {
                        return null; // ✅ Return null if product not found
                    }
                    
                    // ✅ Keep existing images if not updated
                    if (string.IsNullOrEmpty(productVM.MainImageUrl))
                    {
                        productVM.MainImageUrl = ProductDetails.MainImageUrl;
                    }
                    if (productVM.AdditionalImages == null || !productVM.AdditionalImages.Any())
                    {
                        productVM.AdditionalImages = ProductDetails.AdditionalImages;
                    }

                    Product Product = mapper.Map<ProductVM, Product>(productVM, ProductDetails);

                    var updatedProduct = db.Products.Update(Product);
                    await db.SaveChangesAsync();
                    return mapper.Map<Product, ProductVM>(updatedProduct.Entity);
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
