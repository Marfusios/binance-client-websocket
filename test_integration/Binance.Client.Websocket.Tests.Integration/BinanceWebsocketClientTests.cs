using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Requests;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Websockets;
using Xunit;

namespace Binance.Client.Websocket.Tests.Integration
{
    public class BinanceWebsocketClientTests
    {
        private static readonly string API_KEY = "your_api_key";
        private static readonly string API_SECRET = "";

        [Fact]
        public async Task PingPong()
        {
            var url = BinanceValues.ApiWebsocketUrl;
            using (var communicator = new BinanceWebsocketCommunicator(url))
            {
                PongResponse received = null;
                var receivedEvent = new ManualResetEvent(false);

                using (var client = new BinanceWebsocketClient(communicator))
                {

                    client.Streams.PongStream.Subscribe(pong =>
                    {
                        received = pong;
                        receivedEvent.Set();
                    });

                    await communicator.Start();

                    await client.Send(new PingRequest());

                    receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                    Assert.NotNull(received);
                }
            }
        }

        [SkippableFact]
        public async Task Authentication()
        {
            Skip.If(string.IsNullOrWhiteSpace(API_SECRET));

            var url = BinanceValues.ApiWebsocketUrl;
            using (var communicator = new BinanceWebsocketCommunicator(url))
            {
                AuthenticationResponse received = null;
                var receivedEvent = new ManualResetEvent(false);

                using (var client = new BinanceWebsocketClient(communicator))
                {

                    client.Streams.AuthenticationStream.Subscribe(auth =>
                    {
                        received = auth;
                        receivedEvent.Set();
                    });

                    await communicator.Start();

                    await client.Authenticate(API_KEY, API_SECRET);

                    receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

                    Assert.NotNull(received);
                    Assert.True(received.Success);
                }
            }
        }

    }
}
