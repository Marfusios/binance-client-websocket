namespace Binance.Client.Websocket.Subscriptions;

/// <summary>
/// Base class for every simple subscription (just symbol + channel)
/// </summary>
public abstract class SimpleSubscriptionBase : SubscriptionBase
{
    /// <summary>
    /// Create simple subscription for provided symbol
    /// </summary>
    /// <param name="symbol"></param>
    protected SimpleSubscriptionBase(string symbol)
    {
        Symbol = (symbol ?? string.Empty).ToLower();
    }

    /// <summary>
    /// Target symbol (bnbbtc, ethbtc, etc)
    /// </summary>
    public string Symbol { get; }

    /// <summary>
    /// Target channel (trade, aggTrade, etc)
    /// </summary>
    public abstract string Channel { get; }

    /// <inheritdoc />
    public override string StreamName => $"{Symbol}@{Channel}";
}