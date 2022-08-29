using Microsoft.AspNetCore.Components;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class ProductsByCategory
    {
        [Parameter]
        public int CategoryId { get; set; }

        [Inject]
        public IProductService ProductService { get; set; }

        [Inject]
        public IManageProductsLocalStorageService ManageProductsLocalStorageService { get; set; }

        public IEnumerable<ProductDto> ProductList { get; set; }
        public string CategoryName { get; set; }
        public string ErrorMessage { get; set; }

        protected override async Task OnParametersSetAsync()
        {
            try
            {
                ProductList = await GetProductCollectionByCategoryId(CategoryId);

                if (ProductList != null && ProductList.Any())
                {
                    var productDto = ProductList.FirstOrDefault(p => p.CategoryId == CategoryId);

                    if (productDto != null)
                    {
                        CategoryName = productDto.CategoryName;
                    }

                }
            }
            catch (Exception ex)
            {
                ErrorMessage = ex.Message;
            }
        }

        private async Task<IEnumerable<ProductDto>> GetProductCollectionByCategoryId(int categoryId)
        {
            var productCollection = await ManageProductsLocalStorageService.GetCollection();

            if (productCollection != null)
            {
                return productCollection.Where(p => p.CategoryId == categoryId);
            }
            else
            {
                return await ProductService.GetItemsByCategory(categoryId);
            }

        }

    }
}
