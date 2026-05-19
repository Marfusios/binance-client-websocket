using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Communicator;
using Binance.Client.Websocket.Signing;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Binance.Client.Websocket.Sample.NetFramework
{
    internal class Program
    {
        private static readonly ManualResetEvent ExitEvent = new ManualResetEvent(false);

        private const string ApiKey = "";
        private const string ApiSecret = "";

        private static void Main(string[] args)
        {
            var logger = InitLogging();

            AppDomain.CurrentDomain.ProcessExit += CurrentDomainOnProcessExit;
            Console.CancelKeyPress += ConsoleOnCancelKeyPress;

            Console.WriteLine("|=======================|");
            Console.WriteLine("|     BINANCE CLIENT    |");
            Console.WriteLine("|=======================|");
            Console.WriteLine();

            Log.Debug("====================================");
            Log.Debug("   STARTING (full .NET Framework)   ");
            Log.Debug("====================================");

            var url = BinanceValues.ApiWebsocketUrl;
            using (var communicator = new BinanceWebsocketCommunicator(url, logger.CreateLogger<BinanceWebsocketCommunicator>()))
            using (var client = new BinanceWebsocketClient(communicator, logger.CreateLogger<BinanceWebsocketClient>()))
            {
                communicator.Name = "Binance-1";
                communicator.ReconnectTimeout = TimeSpan.FromMinutes(10);
                communicator.ReconnectionHappened.Subscribe(info =>
                    Log.Information("Reconnection happened, type: {type}", info.Type));
                communicator.DisconnectionHappened.Subscribe(info =>
                    Log.Information("Disconnection happened, type: {type}", info.Type));

                SubscribeToStreams(client);

                client.SetSubscriptions(
                    new TradeSubscription("btcusdt"),
                    new OrderBookPartialSubscription("btcusdt", 5));

                if (!string.IsNullOrWhiteSpace(ApiSecret))
                {
                    communicator.Authenticate(ApiKey, new BinanceHmac(ApiSecret)).Wait();
                }

                communicator.Start().Wait();

                ExitEvent.WaitOne();
            }

            Log.Debug("====================================");
            Log.Debug("              STOPPING              ");
            Log.Debug("====================================");
            Log.CloseAndFlush();
        }

        private static void SubscribeToStreams(BinanceWebsocketClient client)
        {
            client.Streams.PongStream.Subscribe(x =>
                Log.Information("Pong received ({message})", x.Message));

            client.Streams.TradesStream.Subscribe(response =>
            {
                var trade = response.Data;
                Log.Information(
                    "Trade normal [{symbol}] [{side}] price: {price} size: {size}",
                    trade.Symbol,
                    trade.Side,
                    trade.Price,
                    trade.Quantity);
            });

            client.Streams.OrderBookPartialStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information(
                    "Order book snapshot [{symbol}] bid: {bid} ask: {ask}",
                    ob?.Symbol,
                    ob?.Bids?.FirstOrDefault()?.Price.ToString("F"),
                    ob?.Asks?.FirstOrDefault()?.Price.ToString("F"));
            });
        }

        private static SerilogLoggerFactory InitLogging()
        {
            var executingDir = Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
            var logPath = Path.Combine(executingDir, "logs", "verbose.log");
            var logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.File(logPath, rollingInterval: RollingInterval.Day)
                .WriteTo.Console(LogEventLevel.Debug)
                .CreateLogger();

            Log.Logger = logger;
            return new SerilogLoggerFactory(logger);
        }

        private static void CurrentDomainOnProcessExit(object sender, EventArgs eventArgs)
        {
            Log.Warning("Exiting process");
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
