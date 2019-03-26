namespace Binance.Client.Websocket.Responses
{
    /// <summary>
    /// Base message for every response
    /// </summary>
    public class ResponseBase<TPayload>
    {
        /// <summary>
        /// Unique stream name.
        /// Could be "bnbbtc@trade", "bnbbtc@depth", etc.
        /// </summary>
        public string Stream { get; set; }

        /// <summary>
        /// Returned data
        /// </summary>
        public TPayload Data { get; set; }
    }
}
