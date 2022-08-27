using PayPalCheckoutSdk.Orders;
using ShopOnline.Api.PayPal.Values;
using ShopOnline.Models.Dtos;
using System.Globalization;

namespace ShopOnline.Api.PayPal
{
    public static class OrderBuilder
    {
        /// <summary>
        /// Use classes from the PayPalCheckoutSdk to build an OrderRequest
        /// </summary>
        /// <returns></returns>
        public static OrderRequest Build(IEnumerable<CartItemDto> cartItems, string currencyCode)
        {
            try
            {
                var basket = cartItems.FirstOrDefault();

                //https://developer.paypal.com/docs/api/reference/locale-codes/#
                OrderRequest orderRequest = new()
                {
                    CheckoutPaymentIntent = CheckoutPaymentIntent.CAPTURE.ToString(),
                    ApplicationContext = new ApplicationContext
                    {
                        BrandName = "COMPANY NAME HERE",
                        LandingPage = nameof(LandingPage.LOGIN),
                        UserAction = nameof(UserAction.PAY_NOW),
                        ShippingPreference = nameof(ShippingPreference.NO_SHIPPING),
                        Locale = "en-GB"
                    },
                    PurchaseUnits = new List<PurchaseUnitRequest>
                    {
                        new PurchaseUnitRequest
                        {
                            Description = basket.ProductDescription,
                            SoftDescriptor = basket.ProductName.Substring(0, 22),
                            AmountWithBreakdown = new AmountWithBreakdown
                            {
                                CurrencyCode = Enum.Parse<CurrencyCode>(currencyCode, true).ToString(),
                                Value = basket.Price.ToString(CultureInfo.InvariantCulture),
                                AmountBreakdown = new AmountBreakdown
                                {
                                    ItemTotal = new Money
                                    {
                                        CurrencyCode = Enum.Parse<CurrencyCode>(currencyCode, true).ToString(),
                                        Value = cartItems.Sum(p => p.TotalPrice).ToString(CultureInfo.InvariantCulture)
                                    },
                                    // Discount = new Money
                                    // {
                                    //     CurrencyCode = Values.CurrencyCode.USD,
                                    //     Value = basket.Price.ToString()
                                    // }
                                }
                            },
                            Items = new List<Item>()
                        }
                    }
                };

                foreach (var product in cartItems)
                {
                    orderRequest.PurchaseUnits[0]
                                .Items
                                .Add(new Item
                                {
                                    Name = product.ProductName,
                                    Description = product.ProductDescription,
                                    UnitAmount = new Money
                                    {
                                        CurrencyCode = Enum.Parse<CurrencyCode>(currencyCode, true).ToString(),
                                        Value = product.Price.ToString(CultureInfo.InvariantCulture)
                                    },
                                    Quantity = product.Qty.ToString(CultureInfo.InvariantCulture),
                                    Category = nameof(Values.Item.Category.DIGITAL_GOODS)
                                });
                }

                return orderRequest;
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}