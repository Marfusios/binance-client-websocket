using Binance.Client.Websocket.Validations;

namespace Binance.Client.Websocket.Requests
{
    /// <summary>
    /// Subscribe to trades stream
    /// </summary>
    public class TradesSubscribeRequest : SubscribeRequestBase
    {
        /// <summary>
        /// Subscribe to all trades
        /// </summary>
        public TradesSubscribeRequest()
        {
            Symbol = string.Empty;
        }

        /// <summary>
        /// Subscribe to trades for selected pair ('XBTUSD', etc)
        /// </summary>
        public TradesSubscribeRequest(string pair)
        {
            BnbValidations.ValidateInput(pair, nameof(pair));

            Symbol = pair;
        }

        /// <summary>
        /// Trade topic
        /// </summary>
        public override string Topic => "trade";

        /// <inheritdoc />
        public override string Symbol { get; }
    }
}
