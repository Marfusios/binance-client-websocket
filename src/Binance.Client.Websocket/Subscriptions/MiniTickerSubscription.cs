namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Mini-ticker specified symbol statistics for the previous 24hrs
    /// </summary>
    public class MiniTickerSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// Mini-ticker specified symbol statistics for the previous 24hrs
        /// </summary>
        /// <param name="symbol"></param>
        public MiniTickerSubscription(string symbol) : base(symbol)
        {
        }

        /// <inheritdoc />
        public override string Channel => "miniTicker";
    }
}