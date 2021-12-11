namespace Binance.Client.Websocket.Subscriptions;

/// <summary>
/// The best bid or ask's price or quantity for a specified symbol (ethbtc, bnbbtc, etc)
/// </summary>
public class BookTickerSubscription : SimpleSubscriptionBase
{
    /// <summary>
    /// The best bid or ask's price or quantity for a specified symbol (ethbtc, bnbbtc, etc)
    /// </summary>
    public BookTickerSubscription(string symbol) : base(symbol)
    {
    }

    public override string Channel => "bookTicker";
}