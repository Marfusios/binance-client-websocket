using System;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.MarkPrice
{
    /// <summary>
    /// Mark price and funding rate for a single symbol pushed every 3 seconds or every second.
    /// </summary>
    public class Funding : MessageBase
    {
        /// <summary>
        /// The symbol the trade was for
        /// </summary>
        [JsonProperty("s")]
        public string? Symbol { get; set; }

        /// <summary>
        /// The id of this aggregated trade
        /// </summary>
        [JsonProperty("p")]
        public double MarkPrice { get; set; }

        /// <summary>
        /// The price of the trades
        /// </summary>
        [JsonProperty("i")]
        public double IndexPrice { get; set; }

        /// <summary>
        /// The combined quantity of the trades
        /// </summary>
        [JsonProperty("r")]
        public double FundingRate { get; set; }

        /// <summary>
        /// The time of the trades
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime NextFundingTime { get; set; }
    }
}