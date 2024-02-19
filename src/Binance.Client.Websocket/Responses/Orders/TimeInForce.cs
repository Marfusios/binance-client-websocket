using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// The time the order will be active for
    /// </summary>
    public enum TimeInForce
    {
        /// <summary>
        /// GoodTillCanceled orders will stay active until they are filled or canceled
        /// </summary>
        [JsonProperty("GTC")]
        GoodTillCanceled,
        
        /// <summary>
        /// ImmediateOrCancel orders have to be at least partially filled upon placing or will be automatically canceled
        /// </summary>
        [JsonProperty("IOC")]
        ImmediateOrCancel,
        
        /// <summary>
        /// FillOrKill orders have to be entirely filled upon placing or will be automatically canceled
        /// </summary>
        [JsonProperty("FOK")]
        FillOrKill,
        
        /// <summary>
        /// GoodTillCrossing orders will post only
        /// </summary>
        [JsonProperty("GTX")]
        GoodTillCrossing,
        
        /// <summary>
        /// Good til the order expires or is canceled
        /// </summary>
        [JsonProperty("GTE_GTC")]
        GoodTillExpiredOrCanceled,
        
        /// <summary>
        /// Good til date
        /// </summary>
        [JsonProperty("GTD")]
        GoodTillDate
    }
}