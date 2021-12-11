using System.Reactive.Subjects;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Responses.AggregateTrades;
using Binance.Client.Websocket.Responses.Books;
using Binance.Client.Websocket.Responses.BookTickers;
using Binance.Client.Websocket.Responses.Kline;
using Binance.Client.Websocket.Responses.MarkPrice;
using Binance.Client.Websocket.Responses.MiniTicker;
using Binance.Client.Websocket.Responses.Trades;

namespace Binance.Client.Websocket.Client;

/// <summary>
/// All provided streams.
/// You need to set subscriptions in advance (via method `SetSubscriptions()` on BinanceWebsocketClient)
/// </summary>
public class BinanceClientStreams
{
    /// <summary>
    /// Response stream to every ping request
    /// </summary>
    public readonly Subject<PongResponse> PongStream = new();

    /// <summary>
    /// Trades stream - emits every executed trade on Binance
    /// </summary>
    public readonly Subject<TradeResponse> TradesStream = new();

    /// <summary>
    /// Chunk of trades - emits grouped trades together
    /// </summary>
    public readonly Subject<AggregatedTradeResponse> AggregateTradesStream = new();

    /// <summary>
    /// Partial order book stream - emits small snapshot of the order book
    /// </summary>
    public readonly Subject<OrderBookPartialResponse> OrderBookPartialStream = new();

    /// <summary>
    /// Order book difference stream - emits small snapshot of the order book
    /// </summary>
    public readonly Subject<OrderBookDiffResponse> OrderBookDiffStream = new();

    /// <summary>
    /// Mark price and funding rate stream - emits mark price and funding rate for a single symbol pushed every 3 seconds or every second
    /// </summary>
    public readonly Subject<FundingResponse> FundingStream = new();

    /// <summary>
    ///  The best bid or ask's price or quantity in real-time for a specified symbol
    /// </summary>
    public readonly Subject<BookTickerResponse> BookTickerStream = new();

    /// <summary>
    /// The Kline/Candlestick subscription, provide symbol and chart intervals
    /// </summary>
    public readonly Subject<KlineResponse> KlineStream = new();

    /// <summary>
    /// Mini-ticker specified symbol statistics for the previous 24hrs
    /// </summary>
    public readonly Subject<MiniTickerResponse> MiniTickerStream = new();
}