using System;
using System.IO;
using System.Linq;
using System.Runtime.Loader;
using System.Threading;
using System.Threading.Tasks;
using Binance.Client.Websocket.Client;
using Binance.Client.Websocket.Communicator;
using Binance.Client.Websocket.Signing;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Websockets;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Extensions.Logging;

namespace Binance.Client.Websocket.Sample
{
    class Program
    {
        private static readonly ManualResetEvent ExitEvent = new(false);

        private const string ApiKey = "";
        private const string ApiSecret = "";
        
        static async Task Main(string[] args)
        {
            var logger = InitLogging();

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
            using (var communicator = new BinanceWebsocketCommunicator(url, logger.CreateLogger<BinanceWebsocketCommunicator>()))
            using (var fCommunicator = new BinanceWebsocketCommunicator(fUrl, logger.CreateLogger<BinanceWebsocketCommunicator>()))
            {
                communicator.Name = "Binance-1";
                communicator.ReconnectTimeout = TimeSpan.FromMinutes(10);
                communicator.ReconnectionHappened.Subscribe(info =>
                    Log.Information("Reconnection happened, type: {type}", info.Type));
                communicator.DisconnectionHappened.Subscribe(info =>
                    Log.Information("Disconnection happened, type: {type}", info.Type));

                fCommunicator.Name = "Binance-f";
                fCommunicator.ReconnectTimeout = TimeSpan.FromMinutes(10);
                fCommunicator.ReconnectionHappened.Subscribe(info =>
                    Log.Information("Reconnection happened, type: {type}", info.Type));
                fCommunicator.DisconnectionHappened.Subscribe(info =>
                    Log.Information("Disconnection happened, type: {type}", info.Type));
                
                using (var client = new BinanceWebsocketClient(communicator, logger.CreateLogger<BinanceWebsocketClient>()))
                using (var fClient = new BinanceWebsocketClient(fCommunicator, logger.CreateLogger<BinanceWebsocketClient>()))
                {
                    SubscribeToStreams(client, communicator);
                    //SubscribeToStreams(fClient, communicator);

                    client.SetSubscriptions(
                    new TradeSubscription("btcusdt"),
                    //new TradeSubscription("ethbtc"),
                    //new TradeSubscription("bnbusdt"),
                    //new AggregateTradeSubscription("bnbusdt"),
                    new OrderBookPartialSubscription("btcusdt", 5)
                    //new OrderBookPartialSubscription("bnbusdt", 10),
                    //new OrderBookDiffSubscription("btcusdt"),
                    //new BookTickerSubscription("btcusdt"),
                    //new KlineSubscription("btcusdt", "1m"),
                    //new MiniTickerSubscription("btcusdt")
                    //new AllMarketMiniTickerSubscription()
                    );

                    //fClient.SetSubscriptions(
                    //    new AllMarketMiniTickerSubscription());
                    
                    if (!string.IsNullOrWhiteSpace(ApiSecret))
                    {
                        await communicator.Authenticate(ApiKey, new BinanceHmac(ApiSecret));
                    }
                    
                    await communicator.Start();
                    //fCommunicator.Start().Wait();

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
                Log.Information("Pong received ({message})", x.Message));

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
                Log.Information("Trade normal [{symbol}] [{side}] " +
                                "price: {price} size: {size}", trade.Symbol, trade.Side, trade.Price, trade.Quantity);
            });

            client.Streams.OrderBookPartialStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information("Order book snapshot [{symbol}] " +
                                "bid: {bid} " +
                                "ask: {ask}",
                    ob?.Symbol,
                    ob?.Bids?.FirstOrDefault()?.Price.ToString("F"),
                    ob?.Asks?.FirstOrDefault()?.Price.ToString("F"));
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
                Log.Information($"Book ticker [{ob.Symbol}] " +
                                $"Best ask price: {ob.BestAskPrice} " +
                                $"Best ask qty: {ob.BestAskQty} " +
                                $"Best bid price: {ob.BestBidPrice} " +
                                $"Best bid qty: {ob.BestBidQty}");
            });

            client.Streams.KlineStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information($"Kline [{ob.Symbol}] " +
                                $"Kline start time: {ob.StartTime} " +
                                $"Kline close time: {ob.CloseTime} " +
                                $"Interval: {ob.Interval} " +
                                $"First trade ID: {ob.FirstTradeId} " +
                                $"Last trade ID: {ob.LastTradeId} " +
                                $"Open price: {ob.OpenPrice} " +
                                $"Close price: {ob.ClosePrice} " +
                                $"High price: {ob.HighPrice} " +
                                $"Low price: {ob.LowPrice} " +
                                $"Base asset volume: {ob.BaseAssetVolume} " +
                                $"Number of trades: {ob.NumberTrades} " +
                                $"Is this kline closed?: {ob.IsClose} " +
                                $"Quote asset volume: {ob.QuoteAssetVolume} " +
                                $"Taker buy base: {ob.TakerBuyBaseAssetVolume} " +
                                $"Taker buy quote: {ob.TakerBuyQuoteAssetVolume} " +
                                $"Ignore: {ob.Ignore} ");
            });

            client.Streams.MiniTickerStream.Subscribe(response =>
            {
                var ob = response.Data;
                Log.Information($"Mini-ticker [{ob.Symbol}] " +
                                $"Open price: {ob.OpenPrice} " +
                                $"Close price: {ob.ClosePrice} " +
                                $"High price: {ob.HighPrice} " +
                                $"Low price: {ob.LowPrice} " +
                                $"Base asset volume: {ob.BaseAssetVolume} " +
                                $"Quote asset volume: {ob.QuoteAssetVolume}");
            });
            client.Streams.AllMarketMiniTickerStream.Subscribe(response =>
            {
                response.Data.ForEach(ob =>
                {

                    Log.Information($"All market mini-ticker [{ob.Symbol}] " +
                                    $"Open price: {ob.OpenPrice} " +
                                    $"Close price: {ob.ClosePrice} " +
                                    $"High price: {ob.HighPrice} " +
                                    $"Low price: {ob.LowPrice} " +
                                    $"Base asset volume: {ob.BaseAssetVolume} " +
                                    $"Quote asset volume: {ob.QuoteAssetVolume}");
                });
            });

            client.Streams.OrderUpdateStream.Subscribe(order =>
            {
                Log.Information("Order {type} {side} updated, amount: {quantity}, price: {price}. Filled: {filled}. " +
                                "Status: {status}, execution: {executionType}", 
                    order.Type, order.Side, order.Quantity, order.Price, order.QuantityFilled, order.Status, order.ExecutionType);
            });
        }

        private static SerilogLoggerFactory InitLogging()
        {
            var executingDir = AppContext.BaseDirectory;
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
