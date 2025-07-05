using BlazorApp1.Modals;
using Business.Repository;
using Business.Repository.IRepository;
using DataAcess;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;


namespace ShinySparkle_Apis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(Roles = "Admin")]
    public class ProductsController : ControllerBase
    {
        private readonly IProductListRepository db;

        public ProductsController(IProductListRepository db)
        {
            this.db = db;
        }

        // ✅ Get all products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductVM>>> GetProducts()
        {
            var products = await db.GetAllProduct();
            return Ok(products);
        }

        // ✅ Get product by ID
        [HttpGet("{id:int}")]
        public async Task<ActionResult<ProductVM>> GetProduct(int id)
        {
            var product = await db.GetProductByID(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        }
        [HttpGet("GetProductsByCategory/{category}")]
        public async Task<ActionResult<IEnumerable<ProductVM>>> GetProductsByCategory(string category)
        {
            var products = await db.GetProductByCategory(category);

            if (products == null)
            {
                return NotFound("No products found for this category.");
            }

            return Ok(products);
        }


    }
}
