using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// Futures position side.
    /// </summary>
    public enum PositionSide
    {
        /// <summary>
        /// Both long and short (one-way mode).
        /// </summary>
        [JsonProperty("BOTH")]
        Both,
        
        /// <summary>
        /// Long hedge mode position.
        /// </summary>
        [JsonProperty("LONG")]
        Long,
        
        /// <summary>
        /// Short hedge mode position.
        /// </summary>
        [JsonProperty("SHORT")]
        Short
    }
}
