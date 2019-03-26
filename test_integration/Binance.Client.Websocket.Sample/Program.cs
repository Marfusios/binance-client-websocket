using System;
using System.IO;
using System.Reflection;
using System.Runtime.Loader;
using System.Threading;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Serilog;
using Serilog.Events;

namespace Binance.Client.Websocket.Sample
{
    class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        static void Main(string[] args)
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
            using (var communicator = new BinanceWebsocketCommunicator(url))
            {
                communicator.Name = "Binance-1";
                communicator.ReconnectTimeoutMs = (int)TimeSpan.FromMinutes(10).TotalMilliseconds;
                communicator.ReconnectionHappened.Subscribe(type =>
                    Log.Information($"Reconnection happened, type: {type}"));

                using (var client = new BinanceWebsocketClient(communicator))
                {
                    SubscribeToStreams(client);

                    client.SetSubscriptions(
                        new TradeSubscription("btcusdt"),
                        new TradeSubscription("ethbtc"),
                        new TradeSubscription("bnbbtc"),
                        new AggregateTradeSubscription("bnbbtc")
                        );
                    communicator.Start().Wait();

                    ExitEvent.WaitOne();
                }
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static void SubscribeToStreams(BinanceWebsocketClient client)
        {
            client.Streams.PongStream.Subscribe(x =>
                Log.Information($"Pong received ({x.Message})"));

            client.Streams.AggregateTradesStream.Subscribe(response =>
            {
                var trade = response.Data;
                Log.Information($"Trade aggreg [{trade.Symbol}] [{(trade.IsBuyerMaker ? "SELL" : "BUY")}] " +
                                $"price: {trade.Price} size: {trade.Quantity}");
            });

            client.Streams.TradesStream.Subscribe(response =>
            {
                var trade = response.Data;
                Log.Information($"Trade normal [{trade.Symbol}] [{(trade.IsBuyerMaker ? "SELL" : "BUY")}] " +
                                $"price: {trade.Price} size: {trade.Quantity}");
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
