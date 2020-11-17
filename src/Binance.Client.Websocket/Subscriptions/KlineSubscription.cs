namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// The Kline/Candlestick subscription, provide symbol and chart intervals
    /// </summary>
    public class KlineSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// The Kline/Candlestick subscription, provide symbol and chart intervals
        /// </summary>
        /// <param name="symbol"></param>
        /// <param name="interval">Target interval, valid are
        /// 1m, 3m, 5m, 15m, 30m, 1h, 2h, 4h, 6h, 8h, 12h, 1d, 3d, 1w, 1M</param>
        public KlineSubscription(string symbol, string interval) : base(symbol)
        {
            Interval = interval;
        }

        private string Interval { get; }

        public override string Channel => "kline";

        public override string StreamName => $"{Symbol}@{Channel}_{Interval}";
        
    }
}