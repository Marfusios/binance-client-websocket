namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Mini-ticker all symbol statistics for the previous 24hrs
    /// </summary>
    public class AllMarketMiniTickerSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// Mini-ticker all symbol statistics for the previous 24hrs
        /// </summary>

        public AllMarketMiniTickerSubscription()
        {
        }
        /// <inheritdoc />
        public override string StreamName => "!miniTicker@arr";
        /// <inheritdoc />
        public override string Channel => string.Empty;

    }
}