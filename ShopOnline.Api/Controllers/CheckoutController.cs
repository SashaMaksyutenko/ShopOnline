using Microsoft.AspNetCore.Mvc;
using PayPalCheckoutSdk.Orders;
using ShopOnline.Api.Repositories.Contracts;
using ShopOnline.Api.Extensions;

namespace ShopOnline.Api.Controllers
{
    public class CheckoutController : Controller
    {
        private readonly IShoppingCartRepository shoppingCartRepository;
        private readonly IProductRepository productRepository;

        public CheckoutController(IShoppingCartRepository shoppingCartRepository, IProductRepository productRepository)
        {
            this.shoppingCartRepository = shoppingCartRepository;
            this.productRepository = productRepository;
        }

        /// <summary>
        /// This action is called when the user clicks on the PayPal button.
        /// </summary>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/create/{clientId}/{clientSecret}/{currencyCode}/{userId}")]
        public async Task<PayPal.SmartButtonHttpResponse> Create(string clientId, string clientSecret, string currencyCode, int userId)
        {
            try
            {
                var cartItems = await this.shoppingCartRepository.GetItems(userId);
                var products = await this.productRepository.GetItems();
                var cartItemsDto = cartItems.ConvertToDto(products);
                var request = new OrdersCreateRequest();

                PayPal.PayPalClient.ClientId = clientId;
                PayPal.PayPalClient.ClientSecret = clientSecret;

                request.Prefer("return=representation");
                request.RequestBody(PayPal.OrderBuilder.Build(cartItemsDto, currencyCode));

                // Call PayPal to set up a transaction
                var response = await PayPal.PayPalClient.Client().Execute(request);

                // Create a response, with an order id.
                var result = response.Result<Order>();
                var payPalHttpResponse = new PayPal.SmartButtonHttpResponse(response)
                {
                    orderID = result.Id
                };

                return payPalHttpResponse;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        /// <summary>
        /// This action is called once the PayPal transaction is approved
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/approved/{orderId}")]
        public IActionResult Approved(string orderId)
        {
            return Ok();
        }

        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/complete/{orderId}")]
        public IActionResult Complete(string orderId)
        {
            // 1. Update the database.
            // 2. Complete the order process. Create and send invoices etc.
            // 3. Complete the shipping process.
            return Ok();
        }

        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/cancel/{orderId}")]
        public IActionResult Cancel(string orderId)
        {
            // 1. Remove the orderId from the database.
            return Ok();
        }

        /// <summary>
        /// This action is called once the PayPal transaction is complete
        /// </summary>
        /// <param name="orderId"></param>
        /// <returns></returns>
        [Route("api/paypal/checkout/order/error/{orderId}/{error}")]
        public IActionResult Error(string orderId,
                                   string error)
        {
            // Log the error.
            // Notify the user.
            return NoContent();
        }
    }
}