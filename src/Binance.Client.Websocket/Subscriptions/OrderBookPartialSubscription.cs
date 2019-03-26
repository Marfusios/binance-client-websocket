namespace Binance.Client.Websocket.Subscriptions
{
    /// <summary>
    /// Partial order book subscription, provide symbol (ethbtc, bnbbtc, etc) and levels
    /// </summary>
    public class OrderBookPartialSubscription : SimpleSubscriptionBase
    {
        /// <summary>
        /// Partial order book subscription, provide symbol (ethbtc, bnbbtc, etc) and levels
        /// </summary>
        /// <param name="symbol">ethbtc, bnbbtc, etc</param>
        /// <param name="levels">Target levels, valid are 5, 10, or 20</param>
        public OrderBookPartialSubscription(string symbol, int levels) : base(symbol)
        {
            Levels = levels;
        }

        /// <summary>
        /// Target levels, valid are 5, 10, or 20.
        /// </summary>
        public int Levels { get; }

        /// <inheritdoc />
        public override string Channel => "depth";

        /// <inheritdoc />
        public override string StreamName => $"{Symbol}@{Channel}{Levels}";
    }
}
