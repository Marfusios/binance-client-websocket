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
            using (var communicator = new BinanceWebsocketCommunicator(url))
            {
                string received = null;
                var receivedEvent = new ManualResetEvent(false);

                communicator.MessageReceived.Subscribe(msg =>
                {
                    received = msg.Text;
                    receivedEvent.Set();
                });

                await communicator.Start();

                receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                Assert.NotNull(received);
            }
        }
    }
}
