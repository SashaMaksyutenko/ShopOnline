using System.Runtime.Serialization;

namespace ShopOnline.Api.PayPal.Values
{
    /// <summary>
    /// The shipping preference:
    /// Displays the shipping address to the customer.
    /// Enables the customer to choose an address on the PayPal site.
    /// Restricts the customer from changing the address during the payment-approval process.
    /// Default: GET_FROM_FILE.
    /// Source: https://developer.paypal.com/docs/api/orders/v2/
    /// </summary>
    public enum ShippingPreference
    {
        /// <summary>
        /// Use the customer-provided shipping address on the PayPal site.
        /// </summary>
        [EnumMember(Value = "GET_FROM_FILE")]
        GET_FROM_FILE = 0,

        /// <summary>
        /// Redact the shipping address from the PayPal site. Recommended for digital goods.
        /// </summary>
        [EnumMember(Value = "NO_SHIPPING")]
        NO_SHIPPING = 1,

        /// <summary>
        /// Use the merchant-provided address. The customer cannot change this address on the PayPal site.
        /// </summary>
        [EnumMember(Value = "SET_PROVIDED_ADDRESS")]
        SET_PROVIDED_ADDRESS = 2,
    }
}