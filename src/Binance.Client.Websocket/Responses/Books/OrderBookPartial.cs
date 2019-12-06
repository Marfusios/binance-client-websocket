using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Books
{
    /// <summary>
    /// Partial order book
    /// </summary>
    public class OrderBookPartial
    {
        /// <summary>
        /// The symbol the update is for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// The ID of the last update
        /// </summary>
        [JsonProperty("lastUpdateId")]
        public long LastUpdateId { get; set; }

        /// <summary>
        /// Bid levels
        /// </summary>
        [JsonProperty("bids")]
        public OrderBookLevel[] Bids { get; set; }

        /// <summary>
        /// Asks levels
        /// </summary>
        [JsonProperty("asks")]
        public OrderBookLevel[] Asks { get; set; }
    }
}
