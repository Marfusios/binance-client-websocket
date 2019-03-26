using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Binance.Client.Websocket.Json
{
    /// <summary>
    /// Binance preconfigured JSON serializer
    /// </summary>
    public static class BinanceJsonSerializer
    {
        /// <summary>
        /// JSON settings
        /// </summary>
        public static JsonSerializerSettings Settings => new JsonSerializerSettings
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>() { new BinanceStringEnumConverter { CamelCaseText = true} }
        };

        /// <summary>
        /// Serializer instance
        /// </summary>
        public static JsonSerializer Serializer => JsonSerializer.Create(Settings);

        /// <summary>
        /// Deserialize string into object
        /// </summary>
        public static T Deserialize<T>(string data)
        {
            return JsonConvert.DeserializeObject<T>(data, Settings);
        }

        /// <summary>
        /// Serialize object into JSON string
        /// </summary>
        public static string Serialize(object data)
        {
            return JsonConvert.SerializeObject(data, Settings);
        }
    }
}
