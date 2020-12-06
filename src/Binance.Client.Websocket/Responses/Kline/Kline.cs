using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Kline
{
    /// <summary>
    /// The current klines/candlestick
    /// </summary>
    public class Kline : MessageBase
    {
        /// <summary>
        /// Kline start time
        /// </summary>
        [JsonProperty("t")]
        public double StartTime { get; set; }

        /// <summary>
        /// Kline close time
        /// </summary>
        [JsonProperty("T")]
        public double CloseTime { get; set; }

        /// <summary>
        /// Symbol
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// Interval
        /// </summary>
        [JsonProperty("i")]
        public string Interval { get; set; }

        /// <summary>
        /// First trade ID
        /// </summary>
        [JsonProperty("f")]
        public double FirstTradeId { get; set; }

        /// <summary>
        /// Last trade ID
        /// </summary>
        [JsonProperty("L")]
        public double LastTradeId { get; set; }

        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("o")]
        public double OpenPrice { get; set; }

        /// <summary>
        /// Close price
        /// </summary>
        [JsonProperty("c")]
        public double ClosePrice { get; set; }

        /// <summary>
        /// High price
        /// </summary>
        [JsonProperty("h")]
        public double HighPrice { get; set; }

        /// <summary>
        /// Low price
        /// </summary>
        [JsonProperty("l")]
        public double LowPrice { get; set; }

        /// <summary>
        /// Base asset volume
        /// </summary>
        [JsonProperty("v")]
        public double BaseAssetVolume { get; set; }

        /// <summary>
        /// Number of trades
        /// </summary>
        [JsonProperty("n")]
        public double NumberTrades { get; set; }

        /// <summary>
        /// Is this kline closed?
        /// </summary>
        [JsonProperty("x")]
        public bool IsClose { get; set; }

        /// <summary>
        /// Quote asset volume
        /// </summary>
        [JsonProperty("q")]
        public double QuoteAssetVolume { get; set; }

        /// <summary>
        /// Taker buy base asset volume
        /// </summary>
        [JsonProperty("V")]
        public double TakerBuyBaseAssetVolume { get; set; }

        /// <summary>
        /// Taker buy quote asset volume
        /// </summary>
        [JsonProperty("Q")]
        public double TakerBuyQuoteAssetVolume { get; set; }

        /// <summary>
        /// Ignore
        /// </summary>
        [JsonProperty("B")]
        public double Ignore { get; set; }
    }
}