using System.Runtime.Serialization;

namespace ShopOnline.Api.PayPal.Values
{
    /// <summary>
    /// Configures a Continue or Pay Now checkout flow.
    /// Source: https://developer.paypal.com/docs/api/orders/v2/
    /// </summary>
    public enum UserAction
    {
        [EnumMember(Value = "CONTINUE")]
        CONTINUE = 0,

        /// <summary>
        /// After you redirect the customer to the PayPal payment page,
        /// a Pay Now button appears.
        /// Use this option when the final amount is known when the checkout is initiated
        /// and you want to process the payment immediately when the customer clicks Pay Now.
        /// </summary>
        [EnumMember(Value = "PAY_NOW")]
        PAY_NOW = 1,
    }
}