using System.Runtime.Serialization;

namespace ShopOnline.Api.PayPal.Values
{
    /// <summary>
    /// The type of landing page to show on the PayPal site for customer checkout.
    /// Default: NO_PREFERENCE.
    /// Source: https://developer.paypal.com/docs/api/orders/v2/
    /// </summary>
    public enum LandingPage
    {
        /// <summary>
        /// When the customer clicks PayPal Checkout, the customer is redirected to a page to log in to PayPal and approve the payment.
        /// </summary>
        [EnumMember(Value = "LOGIN")]
        LOGIN = 0,

        /// <summary>
        /// When the customer clicks PayPal Checkout, 
        /// the customer is redirected to a page to enter credit or 
        /// debit card and other relevant billing information required to complete the purchase.
        /// </summary>
        [EnumMember(Value = "BILLING")]
        BILLING = 1,

        /// <summary>
        /// When the customer clicks PayPal Checkout,
        /// the customer is redirected to either a page to log in to PayPal and
        /// approve the payment or to a page to enter credit or
        /// debit card and other relevant billing information
        /// required to complete the purchase, depending on their previous interaction with PayPal.
        /// </summary
        [EnumMember(Value = "NO_PREFERENCE")]
        NO_PREFERENCE = 2,
    }
}