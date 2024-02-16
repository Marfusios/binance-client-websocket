using System;
using System.Net.WebSockets;
using Binance.Client.Websocket.Communicator;
using Microsoft.Extensions.Logging;
using Websocket.Client;

namespace Binance.Client.Websocket.Websockets
{
    /// <inheritdoc cref="WebsocketClient" />
    public class BinanceWebsocketCommunicator : WebsocketClient, IBinanceCommunicator
    {
        /// <inheritdoc />
        public BinanceWebsocketCommunicator(Uri url, Func<ClientWebSocket>? clientFactory = null)
            : base(url, clientFactory)
        {
        }

        /// <inheritdoc />
        public BinanceWebsocketCommunicator(Uri url, ILogger<BinanceWebsocketCommunicator> logger, Func<ClientWebSocket>? clientFactory = null)
            : base(url, logger, clientFactory)
        {
        }
    }
}
