using System;

namespace Binance.Client.Websocket.Client;

/// <summary>
/// Binance websocket client.
/// </summary>
public interface IBinanceWebsocketClient : IDisposable
{
    /// <summary>
    /// Serializes request and sends message via websocket communicator. 
    /// It logs and re-throws every exception. 
    /// </summary>
    /// <param name="request">Request/message to be sent</param>
    void Send<T>(T request);

    /// <summary>
    /// Provided message streams.
    /// </summary>
    BinanceClientStreams Streams { get; }
}