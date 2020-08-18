namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Mark price and funding subscription, provide symbol (ethbtc, bnbbtc, etc)
    /// </summary>
    public class FundingSubscription: SimpleSubscriptionBase
    {
        /// <summary>
        /// Trade subscription, provide symbol (ethbtc, bnbbtc, etc)
        /// </summary>
        public FundingSubscription(string symbol) : base(symbol)
        {
        }

        /// <inheritdoc />
        public override string Channel => "markPrice";
        
        public override string StreamName => $"{Symbol}@{Channel}";
    }
}