using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// Futures order stop price working type.
    /// </summary>
    public enum WorkingType
    {
        /// <summary>
        /// Contract price.
        /// </summary>
        [JsonProperty("CONTRACT_PRICE")]
        ContractPrice,
        
        /// <summary>
        /// Mark price.
        /// </summary>
        [JsonProperty("MARK_PRICE")]
        MarkPrice
    }
}
