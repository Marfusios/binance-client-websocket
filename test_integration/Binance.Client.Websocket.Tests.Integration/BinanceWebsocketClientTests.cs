using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Responses.Trades;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Xunit;

namespace Binance.Client.Websocket.Tests.Integration
{
    public class BinanceWebsocketClientTests
    {
        [Fact]
        public async Task Connect_ShouldWorkAndReceiveResponse()
        {
            var url = BinanceValues.ApiWebsocketUrl;
            using var communicator = new BinanceWebsocketCommunicator(url);
            TradeResponse received = null;
            var receivedEvent = new ManualResetEvent(false);

            using var client = new BinanceWebsocketClient(communicator);
            client.Streams.TradesStream.Subscribe(response =>
            {
                received = response;
                receivedEvent.Set();
            });

            client.SetSubscriptions(
                new TradeSubscription("btcusdt")
            );

            await communicator.Start();

            receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

            Assert.NotNull(received);
        }

    }
}
