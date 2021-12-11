using System;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Responses.Trades;
using Binance.Client.Websocket.Subscriptions;
using Microsoft.Extensions.Logging.Abstractions;
using Websocket.Client;
using Xunit;

namespace Binance.Client.Websocket.Tests.Integration;

public class BinanceWebsocketClientTests
{
    [Fact]
    public async Task Connect_ShouldWorkAndReceiveResponse()
    {
        var url = BinanceValues.ApiWebsocketUrl;
        using var apiClient = new WebsocketClient(url);

        TradeResponse received = null;
        var receivedEvent = new ManualResetEvent(false);

        using var client = new BinanceWebsocketClient(NullLogger.Instance, apiClient, new TradeSubscription("btcusdt"));

        client.Streams.TradesStream.Subscribe(response =>
        {
            received = response;
            receivedEvent.Set();
        });

        await apiClient.Start();

        receivedEvent.WaitOne(TimeSpan.FromSeconds(30));

        Assert.NotNull(received);
    }

}