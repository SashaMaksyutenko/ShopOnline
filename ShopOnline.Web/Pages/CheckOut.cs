using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;

namespace ShopOnline.Web.Pages
{
    public partial class CheckOut
    {
        [Inject]
        public IJSRuntime Js { get; set; }
        protected IEnumerable<CartItemDto> ShoppingCartItems { get; set; }
        protected int TotalQty { get; set; }
        protected string PaymentDescription { get; set; }
        protected decimal PaymentAmount { get; set; }
        [Inject]
        public IShoppingCartService ShoppingCartService { get; set; }

        protected string DisplayButtons { get; set; } = "block";

        protected override async Task<Task> OnInitializedAsync()
        {
            try
            {
                ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                if (ShoppingCartItems != null && ShoppingCartItems.Any())
                {
                    Guid orderGuid = Guid.NewGuid();
                    PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                    TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                    PaymentDescription = $"Payment_ {HardCoded.UserId} _{orderGuid}";
                }
                else
                {
                    DisplayButtons = "none";
                }
            }
            catch (Exception)
            {
                throw;
            }
            return base.OnInitializedAsync();
        }
        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            try
            {
                if (firstRender)
                {
                    var clientId = "ARyYhF8NJA8sbFk5-kV9FyffjvTPVJZp_30ePzohFbnW1gWawkO77r2QhCJRb6MGXEc-44XAgKo6xobK";
                    var clientSecret = "EARIqCRHe-UDs3fKudEbJCL1Lkb-ndYW1gUZYdB11hbTfDu26hXjCKHKONEhEAsvAecaEZs74iGsK8Wn";
                    var CurrencySign = "$";   // Get from a data store
                    var currencyCode = "USD";
                    await Js.InvokeVoidAsync("initPayPalButton", clientId, clientSecret, currencyCode, HardCoded.UserId);
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}

