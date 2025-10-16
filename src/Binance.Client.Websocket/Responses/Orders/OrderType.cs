using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// Order type for an order
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// Limit orders will be placed at a specific price. If the price isn't available in the order book for that asset the order will be added in the order book for someone to fill.
        /// </summary>
        [JsonProperty("LIMIT")]
        Limit,
        
        /// <summary>
        /// Market order will be placed without a price. The order will be executed at the best price available at that time in the order book.
        /// </summary>
        [JsonProperty("MARKET")]
        Market,
        
        /// <summary>
        /// Stop loss order. Will execute a market order when the price drops below a price to sell and therefor limit the loss
        /// </summary>
        [JsonProperty("STOP_LOSS")]
        StopLoss,
        
        /// <summary>
        /// Stop loss order. Will execute a limit order when the price drops below a price to sell and therefor limit the loss
        /// </summary>
        [JsonProperty("STOP_LOSS_LIMIT")]
        StopLossLimit,
        
        /// <summary>
        /// Take profit order. Will execute a market order when the price rises above a price to sell and therefor take a profit
        /// </summary>
        [JsonProperty("TAKE_PROFIT")]
        TakeProfit,
        
        /// <summary>
        /// Take profit limit order. Will execute a limit order when the price rises above a price to sell and therefor take a profit
        /// </summary>
        [JsonProperty("TAKE_PROFIT_LIMIT")]
        TakeProfitLimit,
        
        /// <summary>
        /// Same as a limit order, however it will fail if the order would immediately match, therefor preventing taker orders
        /// </summary>
        [JsonProperty("LIMIT_MAKER")]
        LimitMaker,
        
        /// <summary>
        /// Stop order
        /// </summary>
        [JsonProperty("STOP")]
        Stop,
        
        /// <summary>
        /// Stop market order
        /// </summary>
        [JsonProperty("STOP_MARKET")]
        StopMarket,
        
        /// <summary>
        /// Take profit market order
        /// </summary>
        [JsonProperty("TAKE_PROFIT_MARKET")]
        TakeProfitMarket,
        
        /// <summary>
        /// Trailing stop market order
        /// </summary>
        [JsonProperty("TRAILING_STOP_MARKET")]
        TrailingStopMarket,
        
        /// <summary>
        /// Liquidation order
        /// </summary>
        [JsonProperty("LIQUIDATION")]
        Liquidation
    }
}
