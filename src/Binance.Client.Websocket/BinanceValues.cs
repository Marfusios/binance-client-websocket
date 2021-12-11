using System;

namespace Binance.Client.Websocket;

/// <summary>
/// Binance static urls
/// </summary>
public static class BinanceValues
{
    /// <summary>
    /// Main Binance url to websocket API
    /// </summary>
    public static readonly Uri ApiWebsocketUrl = new("wss://stream.binance.com:9443");
        
    public static readonly Uri FuturesApiWebsocketUrl = new("wss://fstream.binance.com");
}