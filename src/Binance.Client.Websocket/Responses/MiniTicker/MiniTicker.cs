﻿﻿using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.MiniTicker
{
    /// <summary>
    /// /// Mini-ticker specified symbol statistics for the previous 24hrs
    /// </summary>
    public class MiniTicker : MessageBase
    {
        /// <summary>
        /// The symbol the trade was for
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; }

        /// <summary>
        /// Close price
        /// </summary>
        [JsonProperty("c")]
        public double ClosePrice { get; set; }

        /// <summary>
        /// Open price
        /// </summary>
        [JsonProperty("o")]
        public double OpenPrice { get; set; }

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
        /// Total traded base asset volume
        /// </summary>
        [JsonProperty("v")]
        public double BaseAssetVolume { get; set; }

        /// <summary>
        /// Total traded quote asset volume
        /// </summary>
        [JsonProperty("q")]
        public double QuoteAssetVolume { get; set; }
    }
}