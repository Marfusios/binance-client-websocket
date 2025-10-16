namespace Binance.Client.Websocket.Websockets
{
    /// <summary>
    /// Supported Binance user data stream types.
    /// </summary>
    public enum BinanceUserStreamType
    {
        /// <summary>
        /// Spot account user data stream.
        /// </summary>
        Spot,
        
        /// <summary>
        /// Futures account user data stream.
        /// </summary>
        Futures
    }
}
