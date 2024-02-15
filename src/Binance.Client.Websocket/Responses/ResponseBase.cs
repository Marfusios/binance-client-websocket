using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses
{
    /// <summary>
    /// Base message for every response
    /// </summary>
    public class ResponseBase<TPayload> where TPayload : class
    {
        /// <summary>
        /// Unique stream name.
        /// Could be "bnbbtc@trade", "bnbbtc@depth", etc.
        /// </summary>
        [JsonProperty("stream")]
        public string Stream { get; set; } = string.Empty;

        /// <summary>
        /// Returned data
        /// </summary>
        [JsonProperty("data")]
        public TPayload? Data { get; set; }
    }
}
