namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Order book difference subscription.
    /// It will return only difference, you need to load snapshot in advance. 
    /// </summary>
    public class OrderBookDiffSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// Diff order book subscription, provide symbol (ethbtc, bnbbtc, etc)
        /// </summary>
        public OrderBookDiffSubscription(string symbol) : base(symbol)
        {
        }

        /// <inheritdoc />
        public override string Channel => "depth";
    }
}
