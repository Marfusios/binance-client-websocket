using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// The status of an orderн
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// Order is new
        /// </summary>
        New,
        
        /// <summary>
        /// Order is partly filled, still has quantity left to fill
        /// </summary>
        [JsonProperty("PARTIALLY_FILLED")]
        PartiallyFilled,
        
        /// <summary>
        /// The order has been filled and completed
        /// </summary>
        Filled,
        
        /// <summary>
        /// The order has been canceled
        /// </summary>
        Canceled,
        
        /// <summary>
        /// The order is in the process of being canceled  (currently unused)
        /// </summary>
        [JsonProperty("PENDING_CANCEL")]
        PendingCancel,
        
        /// <summary>
        /// The order has been rejected
        /// </summary>
        Rejected,
        
        /// <summary>
        /// The order has expired
        /// </summary>
        Expired,
        
        /// <summary>
        /// Liquidation with Insurance Fund
        /// </summary>
        [JsonProperty("NEW_INSURANCE")]
        Insurance,
        
        /// <summary>
        /// Counterparty Liquidation
        /// </summary>
        [JsonProperty("NEW_ADL")]
        Adl,
        
        /// <summary>
        /// Expired because of trigger SelfTradePrevention
        /// </summary>
        [JsonProperty("EXPIRED_IN_MATCH")]
        ExpiredInMatch
    }
}