using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// The reason the order was rejected
    /// </summary>
    public enum OrderRejectReason
    {
        /// <summary>
        /// Not rejected
        /// </summary>
        None,
        
        /// <summary>
        /// Unknown instrument
        /// </summary>
        [JsonProperty("UNKNOWN_INSTRUMENT")]
        UnknownInstrument,
        
        /// <summary>
        /// Closed market
        /// </summary>
        [JsonProperty("MARKET_CLOSED")]
        MarketClosed,
        
        /// <summary>
        /// Quantity out of bounds
        /// </summary>
        [JsonProperty("PRICE_QTY_EXCEED_HARD_LIMITS")]
        PriceQuantityExceedsHardLimits,
        
        /// <summary>
        /// Unknown order
        /// </summary>
        [JsonProperty("UNKNOWN_ORDER")]
        UnknownOrder,
        
        /// <summary>
        /// Duplicate
        /// </summary>
        [JsonProperty("DUPLICATE_ORDER")]
        DuplicateOrder,
        
        /// <summary>
        /// Unkown account
        /// </summary>
        [JsonProperty("UNKNOWN_ACCOUNT")]
        UnknownAccount,
        
        /// <summary>
        /// Not enough balance
        /// </summary>
        [JsonProperty("INSUFFICIENT_BALANCE")]
        InsufficientBalance,
        
        /// <summary>
        /// Account not active
        /// </summary>
        [JsonProperty("ACCOUNT_INACTIVE")]
        AccountInactive,
        
        /// <summary>
        /// Cannot settle
        /// </summary>
        [JsonProperty("ACCOUNT_CANNOT_SETTLE")]
        AccountCannotSettle,
        
        /// <summary>
        /// Stop price would trigger immediately
        /// </summary>
        [JsonProperty("STOP_PRICE_WOULD_TRIGGER_IMMEDIATELY")]
        StopPriceWouldTrigger
    }
}