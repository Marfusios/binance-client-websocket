using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.BookTickers
{
    /// <summary>
    ///  Info the best bid or ask's price or quantity
    /// </summary>
    public class BookTicker
    {
        /// <summary>
        /// order book updateId
        /// </summary>
        [JsonProperty("u")]
        public double OrderBookUpdateId { get; set; }

        /// <summary>
        /// The symbol 
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The best bid price
        /// </summary>
        [JsonProperty("b")]
        public double BestBidPrice { get; set; }

        /// <summary>
        /// The best bid quantity
        /// </summary>
        [JsonProperty("B")]
        public double BestBidQty { get; set; }

        /// <summary>
        /// Thr best ask price
        /// </summary>
        [JsonProperty("a")]
        public double BestAskPrice { get; set; }

        /// <summary>
        /// The best ask quantity
        /// </summary>
        [JsonProperty("A")]
        public double BestAskQty { get; set; }
    }
}