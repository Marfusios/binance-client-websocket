using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Responses.AggregateTrades;
using Binance.Client.Websocket.Responses.Books;
using Binance.Client.Websocket.Responses.MarkPrice;
using Binance.Client.Websocket.Responses.Trades;

namespace Binance.Client.Websocket.Client
{
    /// <summary>
    /// All provided streams.
    /// You need to set subscriptions in advance (via method `SetSubscriptions()` on BinanceWebsocketClient)
    /// </summary>
    public class BinanceClientStreams
    {
        internal readonly Subject<PongResponse> PongSubject = new Subject<PongResponse>();

        internal readonly Subject<TradeResponse> TradesSubject = new Subject<TradeResponse>();
        internal readonly Subject<AggregatedTradeResponse> TradeBinSubject = new Subject<AggregatedTradeResponse>();

        internal readonly Subject<OrderBookPartialResponse> OrderBookPartialSubject =
            new Subject<OrderBookPartialResponse>();

        internal readonly Subject<OrderBookDiffResponse> OrderBookDiffSubject = new Subject<OrderBookDiffResponse>();
        internal readonly Subject<FundingResponse> FundingSubject = new Subject<FundingResponse>();


        // PUBLIC

        /// <summary>
        /// Response stream to every ping request
        /// </summary>
        public IObservable<PongResponse> PongStream => PongSubject.AsObservable();

        /// <summary>
        /// Trades stream - emits every executed trade on Binance
        /// </summary>
        public IObservable<TradeResponse> TradesStream => TradesSubject.AsObservable();

        /// <summary>
        /// Chunk of trades - emits grouped trades together
        /// </summary>
        public IObservable<AggregatedTradeResponse> AggregateTradesStream => TradeBinSubject.AsObservable();

        /// <summary>
        /// Partial order book stream - emits small snapshot of the order book
        /// </summary>
        public IObservable<OrderBookPartialResponse> OrderBookPartialStream => OrderBookPartialSubject.AsObservable();

        /// <summary>
        /// Order book difference stream - emits small snapshot of the order book
        /// </summary>
        public IObservable<OrderBookDiffResponse> OrderBookDiffStream => OrderBookDiffSubject.AsObservable();

        /// <summary>
        /// Mark price and funding rate stream - emits mark price and funding rate for a single symbol pushed every 3 seconds or every second
        /// </summary>
        public IObservable<FundingResponse> FundingStream => FundingSubject.AsObservable();
    }
}