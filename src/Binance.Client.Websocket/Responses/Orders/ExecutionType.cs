using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// The type of execution
    /// </summary>
    public enum ExecutionType
    {
        /// <summary>
        /// New
        /// </summary>
        New,
        
        /// <summary>
        /// Canceled
        /// </summary>
        Canceled,
        
        /// <summary>
        /// Replaced
        /// </summary>
        Replaced,
        
        /// <summary>
        /// Rejected
        /// </summary>
        Rejected,
        
        /// <summary>
        /// Trade
        /// </summary>
        Trade,
        
        /// <summary>
        /// Calculated liquidation execution
        /// </summary>
        [JsonProperty("CALCULATED")]
        Calculated,
        
        /// <summary>
        /// Expired
        /// </summary>
        Expired,
        
        /// <summary>
        /// Amendment
        /// </summary>
        Amendment,
        
        /// <summary>
        /// Self trade prevented
        /// </summary>
        TradePrevention
    }
}
