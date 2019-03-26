namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Trade subscription, provide symbol (ethbtc, bnbbtc, etc)
    /// </summary>
    public class TradeSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// Trade subscription, provide symbol (ethbtc, bnbbtc, etc)
        /// </summary>
        public TradeSubscription(string symbol) : base(symbol)
        {
        }

        /// <inheritdoc />
        public override string Channel => "trade";
    }
}
