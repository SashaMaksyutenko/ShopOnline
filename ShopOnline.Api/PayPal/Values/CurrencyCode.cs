using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace ShopOnline.Api.PayPal.Values
{
    public enum CurrencyCode
    {

        /// <summary>
        /// Great British Pounds
        /// </summary>
        [EnumMember(Value = "GBP")]
        GBP = 0,

        /// <summary>
        /// US Dollars
        /// </summary>
        [EnumMember(Value = "USD")]
        USD = 1,

        /// <summary>
        /// Euros
        /// </summary>
        [EnumMember(Value = "EUR")]
        EUR = 2,
    }
}
