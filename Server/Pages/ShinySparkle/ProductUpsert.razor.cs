using BlazorApp1.Modals;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components;
using Blazored.TextEditor;
using Microsoft.EntityFrameworkCore;

namespace Server.Pages.ShinySparkle
{
    public partial class ProductUpsert : ComponentBase
    {
        [Parameter]
        public int? Id { get; set; }

        private ProductVM productVM { get; set; } = new ProductVM();
        private string Title { get; set; } = "Create";
        private List<string> DeletedImagesNames { get; set; } = new List<string>();
        private bool isImageUploadProcessStarted { get; set; } = false;
        public BlazoredTextEditor QuillHtml { get; set; } = new BlazoredTextEditor();

        protected override async Task OnInitializedAsync()
        {
            if (Id != null)
            {
                // Update mode
                Title = "Update";
                productVM = await ProductListRepository.GetProductByID(Id.Value);
                if (productVM?.AdditionalImages != null)
                {
                    productVM.AdditionalImages = productVM.AdditionalImages.ToList();
                }
            }
            else
            {
                // Create mode
                productVM = new ProductVM();
            }
        }

        protected async override void OnAfterRender(bool firstRender)
        {
            if (!firstRender)
                return;

            bool loading = true;
            while (loading)
            {
                try
                {
                    if (!string.IsNullOrEmpty(productVM.Description))
                    {
                        await QuillHtml.LoadHTMLContent(productVM.Description);
                    }
                    loading = false;
                }
                catch
                {
                    await Task.Delay(10);
                    loading = true;
                }
            }

            base.OnAfterRender(firstRender);
        }

        private async Task HandleProductUpsert()
        {
            try
            {
                productVM.Description = await QuillHtml.GetHTML();

                if (Id != null && Id > 0)
                {
                    await ProductListRepository.UpdateProduct(Id.Value, productVM);
                }
                else
                {
                    await ProductListRepository.CreateProduct(productVM);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
            }

            NavigationManager.NavigateTo("/ProductList");
        }

        private async Task HandleImageUpload(InputFileChangeEventArgs e)
        {
            isImageUploadProcessStarted = true;
            try
            {
                var images = new List<string>();
                foreach (var file in e.GetMultipleFiles())
                {
                    var uploadedImage = await FileUpload.UploadFile(file);
                    images.Add(uploadedImage);
                }

                if (images.Any())
                {
                    if (string.IsNullOrEmpty(productVM.MainImageUrl))
                    {
                        // Set the first uploaded image as MainImageUrl if it's not set
                        productVM.MainImageUrl = images.First();
                        images.RemoveAt(0); // Remove the first image from the list
                    }

                    // Add remaining images to AdditionalImages
                    productVM.AdditionalImages.AddRange(images);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error uploading image: {ex.Message}");
            }
            finally
            {
                isImageUploadProcessStarted = false;
            }
        }

        private async Task DeletePhoto(string imageUrl, bool isMainImage)
        {
            try
            {
                if (string.IsNullOrEmpty(imageUrl))
                    return;

                var imageName = imageUrl.Replace("ProductImages/", ""); // Extract file name
                FileUpload.DeleteFile(imageName); // ? Delete from folder

                if (isMainImage)
                {
                    // ? Remove Main Image from ViewModel
                    productVM.MainImageUrl = null;

                    // ? If editing an existing product, update the database
                    if (productVM.Id != 0)
                    {
                        var product = await _dbContext.Products.FindAsync(productVM.Id);
                        if (product != null)
                        {
                            product.MainImageUrl = null;
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }
                else
                {
                    // ? Remove from AdditionalImages in ViewModel
                    if (productVM.AdditionalImages != null && productVM.AdditionalImages.Contains(imageUrl))
                    {
                        productVM.AdditionalImages.Remove(imageUrl);
                    }

                    // ? If editing an existing product, update the database
                    if (productVM.Id != 0)
                    {
                        var product = await _dbContext.Products.FindAsync(productVM.Id);
                        if (product != null && product.AdditionalImages != null)
                        {
                            product.AdditionalImages.Remove(imageUrl);
                            await _dbContext.SaveChangesAsync();
                        }
                    }
                }

                StateHasChanged(); // ? Refresh UI
            }
            catch (Exception ex)
            {
                Console.WriteLine($"? Error deleting image: {ex.Message}");
            }
        }

    }
}