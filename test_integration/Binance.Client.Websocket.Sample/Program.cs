using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Communicator;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Serilog;
using Serilog.Events;

namespace Binance.Client.Websocket.Sample
{
    class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        static async Task Main(string[] args)
        {
            InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            AssemblyLoadContext.Default.Unloading += DefaultOnUnloading;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|     BINANCE CLIENT    |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("              STARTING              ");
            Log.Debug("====================================");


            var url = BinanceValues.ApiWebsocketUrl;
            var fUrl = BinanceValues.FuturesApiWebsocketUrl;
            using (var communicator = new BinanceWebsocketCommunicator(url))
            using (var fCommunicator = new BinanceWebsocketCommunicator(fUrl))
            {
                communicator.Name = "Binance-1";
                communicator.ReconnectTimeout = TimeSpan.FromMinutes(10);
                communicator.ReconnectionHappened.Subscribe(type =>
                    Log.Information($"Reconnection happened, type: {type}"));

                fCommunicator.Name = "Binance-f";
                fCommunicator.ReconnectTimeout = TimeSpan.FromMinutes(10);
                fCommunicator.ReconnectionHappened.Subscribe(type =>
                    Log.Information($"Reconnection happened, type: {type}"));

                using (var client = new BinanceWebsocketClient(communicator))
                using (var fClient = new BinanceWebsocketClient(fCommunicator))
                {
                    SubscribeToStreams(client, communicator);
                    SubscribeToStreams(fClient, communicator);

                    client.SetSubscriptions(
                        //new TradeSubscription("btcusdt"),
                        //new TradeSubscription("ethbtc"),
                        //new TradeSubscription("bnbusdt"),
                        //new AggregateTradeSubscription("bnbusdt"),
                        //new OrderBookPartialSubscription("btcusdt", 5),
                        //new OrderBookPartialSubscription("bnbusdt", 10),
                        //new OrderBookDiffSubscription("btcusdt")
                        new BookTickerSubscription("btcusdt")
                    );

                    fClient.SetSubscriptions(
                        new FundingSubscription("btcusdt"));
                    communicator.Start().Wait();
                    fCommunicator.Start().Wait();

                    ExitEvent.WaitOne();
                }
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static void SubscribeToStreams(BinanceWebsocketClient client, IBinanceCommunicator comm)
        {
            client.Streams.PongStream.Subscribe(x =>
                Log.Information($"Pong received ({x.Message})"));

            client.Streams.FundingStream.Subscribe(response =>
            {
                var funding = response.Data;
                Log.Information($"Funding: [{funding.Symbol}] rate:[{funding.FundingRate}] " +
                                $"mark price: {funding.MarkPrice} next funding: {funding.NextFundingTime} " +
                                $"index price {funding.IndexPrice}");
            });

            client.Streams.AggregateTradesStream.Subscribe(response =>
            {
                var trade = response.Data;
                Log.Information($"Trade aggreg [{trade.Symbol}] [{trade.Side}] " +
                                $"price: {trade.Price} size: {trade.Quantity}");
            });

            client.Streams.TradesStream.Subscribe(response =>
            {
                var trade = response.Data;
                Log.Information($"Trade normal [{trade.Symbol}] [{trade.Side}] " +
                                $"price: {trade.Price} size: {trade.Quantity}");
            });

            client.Streams.OrderBookPartialStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information($"Order book snapshot [{ob.Symbol}] " +
                                $"bid: {ob.Bids.FirstOrDefault()?.Price:F} " +
                                $"ask: {ob.Asks.FirstOrDefault()?.Price:F}");
                Task.Delay(500).Wait();
                //OrderBookPartialResponse.StreamFakeSnapshot(response.Data, comm);
            });

            client.Streams.OrderBookDiffStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information($"Order book diff [{ob.Symbol}] " +
                                $"bid: {ob.Bids.FirstOrDefault()?.Price:F} " +
                                $"ask: {ob.Asks.FirstOrDefault()?.Price:F}");
            });

            client.Streams.BookTickerStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information($"Book ticker [{ob.Symbol}]" +
                                $"Best ask price: {ob.BestAskPrice} " +
                                $"Best ask qty: {ob.BestAskQty} " +
                                $"Best bid price: {ob.BestBidPrice} " +
                                $"Best bid qty: {ob.BestBidQty}");
            });
        }

        private static void InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.ColoredConsole(LogEventLevel.Debug)
                .CreateLogger();
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
            ExitEvent.Set();
        }

        private static void DefaultOnUnloading(AssemblyLoadContext assemblyLoadContext)
        {
            Log.Warning("Unloading process");
            ExitEvent.Set();
        }

        private static void ConsoleOnCancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Log.Warning("Canceling process");
            e.Cancel = true;
            ExitEvent.Set();
        }
    }
}