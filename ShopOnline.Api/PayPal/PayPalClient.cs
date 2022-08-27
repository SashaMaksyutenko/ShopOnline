using PayPalCheckoutSdk.Core;
using System.Text;
using System.Runtime.Serialization.Json;

namespace ShopOnline.Api.PayPal
{
    public class PayPalClient
    {
        // Place these static properties into a settings area.
        public static string ClientId { get; set; }
        public static string ClientSecret { get; set; }

        ///<summary>
        /// Set up PayPal environment with sandbox credentials.
        /// In production, use LiveEnvironment.
        ///</summary>
        public static PayPalEnvironment Environment()
        {
            return new SandboxEnvironment(ClientId,
                                          ClientSecret);
        }

        ///<summary>
        /// Returns PayPalHttpClient instance to invoke PayPal APIs.
        ///</summary>
        public static PayPalHttpClient Client()
        {
            return new PayPalHttpClient(Environment());
        }

        public static PayPalHttpClient Client(string refreshToken)
        {
            return new PayPalHttpClient(Environment(), refreshToken);
        }


        ///<summary>
        /// Use this method to serialize Object to a JSON string.
        ///</summary>
        public static String ObjectToJSONString(Object serializableObject)
        {
            MemoryStream memoryStream = new();
            var writer = JsonReaderWriterFactory.CreateJsonWriter(memoryStream,
                                                                  Encoding.UTF8,
                                                                  true,
                                                                  true,
                                                                  "  ");

            var ser = new DataContractJsonSerializer(serializableObject.GetType(),
                                                     new DataContractJsonSerializerSettings
                                                     {
                                                         UseSimpleDictionaryFormat = true
                                                     });

            ser.WriteObject(writer,
                            serializableObject);

            memoryStream.Position = 0;
            StreamReader sr = new(memoryStream);

            return sr.ReadToEnd();
        }
    }
}