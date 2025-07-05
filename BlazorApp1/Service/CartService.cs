using BlazorApp1.Modals;
using Blazored.LocalStorage;

namespace BlazorApp1.Service
{
    public class CartService
    {
        private readonly ILocalStorageService _localStorage;
        public event Action OnCartUpdated;
        private List<ProductVM> _cart = new();

        public CartService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task InitializeCart()
        {
            _cart = await _localStorage.GetItemAsync<List<ProductVM>>("cart") ?? new List<ProductVM>();
            NotifyCartChanged();
        }

        public async Task AddToCart(ProductVM product)
        {
            var existingProduct = _cart.FirstOrDefault(p => p.Id == product.Id);
            if (existingProduct != null)
            {
                existingProduct.Quantity++; // ✅ If product exists, increase quantity

            }
            else
            {
                product.Quantity = 1; // ✅ Otherwise, set quantity to 1
                _cart.Add(product);
            }
            await SaveCart();   // ✅ Save to LocalStorage
        }

        public async Task UpdateQuantity(int productId, int newQuantity)
        {
            var product = _cart.FirstOrDefault(p => p.Id == productId);
            if (product != null && newQuantity > 0)
            {
                product.Quantity = newQuantity;
                await SaveCart();
            }
        }

        public async Task RemoveFromCart(int productId)
        {
            _cart.RemoveAll(p => p.Id == productId); // ✅ Removes product by ID
            await SaveCart(); // ✅ Saves updated cart
        }

        public async Task ClearCart()
        {
            _cart.Clear();
            await SaveCart();
        }

        public List<ProductVM> GetCartItems() => _cart;

        public async Task<int> GetCartCount()
        {
            return _cart.Sum(p => p.Quantity);
        }
        public int GetQuantityInCart(int productId)
        {
            var product = _cart.FirstOrDefault(p => p.Id == productId);
            return product?.Quantity ?? 0;
        }

        private async Task SaveCart()
        {
            await _localStorage.SetItemAsync("cart", _cart);
            NotifyCartChanged();
        }

        private void NotifyCartChanged() => OnCartUpdated?.Invoke();
    }
}
