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
        /// Spot REST API base url
        /// </summary>
        public const string SpotRestApiBaseUrl = "https://api.binance.com";
        
        /// <summary>
        /// Futures data websocket API url
        /// </summary>
        public static readonly Uri FuturesApiWebsocketUrl = new Uri("wss://fstream.binance.com");
        
        /// <summary>
        /// Futures REST API base url
        /// </summary>
        public const string FuturesRestApiBaseUrl = "https://fapi.binance.com";

        /// <summary>
        /// User data websocket API rul
        /// </summary>
        /// <param name="listenKey">Create user's listen key via separate API</param>
        public static Uri UserWebsocketUrl(string listenKey) =>
            new Uri($"wss://stream.binance.com:9443/ws/{listenKey}");
        
        /// <summary>
        /// Futures user data websocket API url
        /// </summary>
        /// <param name="listenKey">Create user's listen key via separate API</param>
        public static Uri UserFuturesWebsocketUrl(string listenKey) =>
            new Uri($"wss://fstream.binance.com/ws/{listenKey}");
    }
}
