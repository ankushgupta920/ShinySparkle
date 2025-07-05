using BlazorApp1.Modals;

namespace BlazorApp1.Pages
{
    public partial class Products
    {
        public List<ProductVM> products;

        protected override async Task OnInitializedAsync()
        {
            products = (await ProductService.GetProductsAsync())?.ToList();
            foreach (var p in products)
            {
                p.Quantity = CartService.GetQuantityInCart(p.Id); // ? set quantity from cart
                
            }

        }
        private void NavigateToProduct(int productId)
        {
            NavigationManager.NavigateTo($"/product/{productId}");
        }
      
    }
}