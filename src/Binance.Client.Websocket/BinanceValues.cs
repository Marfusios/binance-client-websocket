using System;

namespace Binance.Client.Websocket
{
    /// <summary>
    /// Binance static urls
    /// </summary>
    public static class BinanceValues
    {
        /// <summary>
        /// Market data websocket API url
        /// </summary>
        public static readonly Uri ApiWebsocketUrl = new Uri("wss://stream.binance.com:9443");
        
        /// <summary>
        /// Futures data websocket API url
        /// </summary>
        public static readonly Uri FuturesApiWebsocketUrl = new Uri("wss://fstream.binance.com");

        /// <summary>
        /// User data websocket API rul
        /// </summary>
        /// <param name="listenKey">Create user's listen key via separate API</param>
        public static Uri UserWebsocketUrl(string listenKey) =>
            new Uri($"wss://stream.binance.com:9443/ws/{listenKey}");
    }
}
