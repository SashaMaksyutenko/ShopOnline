
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using ShopOnline.Models.Dtos;
using ShopOnline.Web.Services.Contracts;
using System.Text.Json;

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
                if (ShoppingCartItems == null || !ShoppingCartItems.Any())
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
                    var currencyCode = "USD";
                    ShoppingCartItems = await ShoppingCartService.GetItems(HardCoded.UserId);
                    if (ShoppingCartItems != null && ShoppingCartItems.Any())
                    {
                        PaymentAmount = ShoppingCartItems.Sum(p => p.TotalPrice);
                        TotalQty = ShoppingCartItems.Sum(p => p.Qty);
                        PaymentDescription = $"Payment_{HardCoded.UserId}_{Guid.NewGuid()}";

                        var purchase_units = new[] {
                            new
                            {
                                amount = new
                                {
                                    value = ShoppingCartItems.Sum(p => p.TotalPrice).ToString(),
                                    breakdown = ShoppingCartItems.Select(p => new
                                    {
                                        item_total = new
                                        {
                                            currency_code = "USD",
                                            value = ShoppingCartItems.Sum(p => p.TotalPrice).ToString()
                                        }
                                    }).First()
                                },
                                items = ShoppingCartItems.Select(p => new
                                {
                                    description = p.ProductDescription,
                                    name = p.ProductName,
                                    quantity = p.Qty,
                                    unit_amount = new
                                    {
                                        currency_code = "USD",
                                        value = p.Price.ToString()
                                    }
                                }).ToArray()
                            }
                        }.ToArray();
                        var data = JsonSerializer.Serialize(purchase_units);
                        await Js.InvokeVoidAsync("initPayPalButton", data);
                    }
                    else
                    {
                        DisplayButtons = "none";
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}