namespace Binance.Client.Websocket.Subscriptions;

/// <summary>
/// Base class for every subscription
/// </summary>
public abstract class SubscriptionBase
{
    /// <summary>
    /// Unique stream name to subscribe
    /// </summary>
    public abstract string StreamName { get; }
}