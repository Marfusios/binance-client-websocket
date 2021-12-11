namespace Binance.Client.Websocket.Subscriptions;

/// <summary>
/// Aggregate trade subscription, provide symbol (ethbtc, bnbbtc, etc)
/// </summary>
public class AggregateTradeSubscription : SimpleSubscriptionBase
{
    /// <summary>
    /// Aggregate trade subscription, provide symbol (ethbtc, bnbbtc, etc)
    /// </summary>
    public AggregateTradeSubscription(string symbol) : base(symbol)
    {
    }

    /// <inheritdoc />
    public override string Channel => "aggTrade";
}