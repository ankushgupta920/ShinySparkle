using System.ComponentModel.DataAnnotations;

namespace BlazorApp1.Modals
{
    public class ProductVM
    {
        public int Id { get; set; }

        //[Required(ErrorMessage = "Product Name is required")]
        public string Name { get; set; }
        public string ImageUrl { get; set; }
        //[Required(ErrorMessage = "Price is required")]
        //[Range(1, double.MaxValue, ErrorMessage = "Price must be greater than zero")]
        public decimal Price { get; set; }
        //[Range(0, double.MaxValue, ErrorMessage = "Old Price cannot be negative")]
        public decimal OldPrice { get; set; }
        public int Discount => (int)((OldPrice - Price) / OldPrice * 100);
        public string MainImageUrl { get; set; } = "";
        public List<string> AdditionalImages { get; set; } = new List<string>();
        //[Required(ErrorMessage = "Description is required")]
        public string Description { get; set; } = "";
        public string SkuCode { get; set; } = "";
        //[Required(ErrorMessage = "Category is required")]
        public string Category { get; set; }
        public int Quantity { get; set; } = 1; // Default Quantity is 1
    }
}






