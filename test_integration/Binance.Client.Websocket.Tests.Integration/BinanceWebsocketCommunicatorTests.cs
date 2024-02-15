using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Websockets;
using Xunit;

namespace Binance.Client.Websocket.Tests.Integration
{
    public class BinanceWebsocketCommunicatorTests
    {
        [Fact]
        public async Task OnStarting_ShouldGetInfoResponse()
        {
            var url = BinanceValues.ApiWebsocketUrl;
            using var communicator = new BinanceWebsocketCommunicator(url);
            var receivedEvent = new ManualResetEvent(false);

            communicator.MessageReceived.Subscribe(msg =>
            {
                receivedEvent.Set();
            });

            communicator.Url = new Uri(url + "stream?streams=btcusdt@trade");

            await communicator.Start();

            receivedEvent.WaitOne(TimeSpan.FromSeconds(30));
        }
    }
}
