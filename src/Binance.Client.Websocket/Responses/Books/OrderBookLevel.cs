using Binance.Client.Websocket.Json;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Responses.Books;

/// <summary>
/// One level of the order book
/// </summary>
[JsonConverter(typeof(ArrayConverter))]
public class OrderBookLevel
{
    /// <summary>
    /// The price of this order book level
    /// </summary>
    [ArrayProperty(0)]
    public double Price { get; set; }
    /// <summary>
    /// The quantity of this price in the order book
    /// </summary>
    [ArrayProperty(1)]
    public double Quantity { get; set; }
}