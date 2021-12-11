using System;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Responses.Trades;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.AggregateTrades;

/// <summary>
/// Aggregated info about executed trades
/// </summary>
public class AggregateTrade : MessageBase
{
    /// <summary>
    /// The symbol the trade was for
    /// </summary>
    [JsonProperty("s")]
    public string Symbol { get; set; }

    /// <summary>
    /// The id of this aggregated trade
    /// </summary>
    [JsonProperty("a")]
    public long AggregatedTradeId { get; set; }

    /// <summary>
    /// The price of the trades
    /// </summary>
    [JsonProperty("p")]
    public double Price { get; set; }

    /// <summary>
    /// The combined quantity of the trades
    /// </summary>
    [JsonProperty("q")]
    public double Quantity { get; set; }

    /// <summary>
    /// The first trade id in this aggregation
    /// </summary>
    [JsonProperty("f")]
    public long FirstTradeId { get; set; }

    /// <summary>
    /// The last trade id in this aggregation
    /// </summary>
    [JsonProperty("l")]
    public long LastTradeId { get; set; }

    /// <summary>
    /// The time of the trades
    /// </summary>
    [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
    public DateTime TradeTime { get; set; }

    /// <summary>
    /// Whether the buyer was the maker
    /// </summary>
    [JsonProperty("m")]
    public bool IsBuyerMaker { get; set; }
        
    /// <summary>
    /// Was the trade the best price match?
    /// </summary>
    [JsonProperty("M")]
    public bool IsMatch { get; set; }

    /// <summary>
    /// Side of the trade
    /// </summary>
    [JsonIgnore]
    public TradeSide Side => IsBuyerMaker ? TradeSide.Sell : TradeSide.Buy;
}