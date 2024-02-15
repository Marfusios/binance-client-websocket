using System;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Trades
{
    /// <summary>
    /// Info about executed trade
    /// </summary>
    public class Trade : MessageBase
    {
        /// <summary>
        /// The symbol the trade was for
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The id of this aggregated trade
        /// </summary>
        [JsonProperty("t")]
        public long TradeId { get; set; }

        /// <summary>
        /// The price of the trades
        /// </summary>
        [JsonProperty("p")]
        public double Price { get; set; }

        /// <summary>
        /// The combined quantity of the trades
        /// </summary>
        [JsonProperty("q")]
        public double Quantity { get; set; }

        /// <summary>
        /// The first trade id in this aggregation
        /// </summary>
        [JsonProperty("b")]
        public long BuyerOrderId { get; set; }

        /// <summary>
        /// The last trade id in this aggregation
        /// </summary>
        [JsonProperty("a")]
        public long SellerOrderId { get; set; }

        /// <summary>
        /// The time of the trades
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime TradeTime { get; set; }

        /// <summary>
        /// Whether the buyer was the maker
        /// </summary>
        [JsonProperty("m")]
        public bool IsBuyerMaker { get; set; }

        /// <summary>
        /// Was the trade the best price match?
        /// </summary>
        [JsonProperty("M")]
        public bool IsMatch { get; set; }

        /// <summary>
        /// Side of the trade
        /// </summary>
        [JsonIgnore]
        public TradeSide Side => IsBuyerMaker ? TradeSide.Sell : TradeSide.Buy;
    }
}
