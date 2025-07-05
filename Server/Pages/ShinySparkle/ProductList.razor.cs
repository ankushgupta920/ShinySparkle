using BlazorApp1.Modals;
using Business.Repository;
using Microsoft.JSInterop;

namespace Server.Pages.ShinySparkle
{
    public partial class ProductList
    {
        private IEnumerable<ProductVM> products { get; set; } = new List<ProductVM>();  //Model Binding.....


        protected override async Task OnInitializedAsync()
        {
            products = await ProductListRepository.GetAllProduct();
        }
        private async Task HandleDelete(int productId)
        {
            var product = await ProductListRepository.GetProductByID(productId);

            if (product != null)
            {
                // Delete Main Image
                if (!string.IsNullOrEmpty(product.MainImageUrl))
                {
                    FileUpload.DeleteFile(product.MainImageUrl.Replace("ProductImages/", ""));
                }

                // Delete Additional Images
                foreach (var image in product.AdditionalImages)
                {
                    FileUpload.DeleteFile(image.Replace("ProductImages/", ""));
                }

                await ProductListRepository.DeleteProduct(productId);

                // Refresh product list after deletion
                products = await ProductListRepository.GetAllProduct();
            }

        }
        

    }


}
