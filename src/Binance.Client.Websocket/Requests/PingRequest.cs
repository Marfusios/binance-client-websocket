using Binance.Client.Websocket.Messages;

namespace Binance.Client.Websocket.Requests
{
    /// <summary>
    /// Raw ping request to get pong response
    /// </summary>
    public class PingRequest : RequestBase
    {
        /// <inheritdoc />
        public override MessageType Operation => MessageType.Ping;

        /// <inheritdoc />
        public override bool IsRaw => true;
    }
}
