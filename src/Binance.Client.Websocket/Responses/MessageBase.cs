using System;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses
{
    /// <summary>
    /// Base class for every message/data
    /// </summary>
    public class MessageBase
    {
        /// <summary>
        /// The type of the event
        /// </summary>
        [JsonProperty("e")]
        public string Event { get; set; }
        /// <summary>
        /// The time the event happened
        /// </summary>
        [JsonProperty("E"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime EventTime { get; set; }
    }
}
