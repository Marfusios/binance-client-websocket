using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Files;
using Binance.Client.Websocket.Responses.Trades;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Binance.Client.Websocket.Tests.Integration;

public class BinanceFileClientTests
{
    // ----------------------------------------------------------------
    // Don't forget to decompress gzip files before starting the tests
    // ----------------------------------------------------------------

    [SkippableFact]
    public async Task OnStart_ShouldStreamMessagesFromFile()
    {
        var files = new[]
        {
            "data/binance_raw_xbtusd_2018-11-13.txt"
        };
        foreach (var file in files)
        {
            var exist = File.Exists(file);
            Skip.If(!exist, $"The file '{file}' doesn't exist. Don't forget to decompress gzip file!");
        }

        var trades = new List<Trade>();

        var communicator = new BinanceFileClient
        {
            FileNames = files,
            Delimiter = ";;"
        };

        var client = new BinanceWebsocketClient(NullLogger.Instance, communicator);
        client.Streams.TradesStream.Subscribe(response =>
        {
            trades.Add(response.Data);
        });

        await communicator.Start();

        Assert.Equal(44259, trades.Count);
    }
}