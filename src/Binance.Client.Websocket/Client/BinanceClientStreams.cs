using System;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Responses.Books;
using Binance.Client.Websocket.Responses.TradeBins;
using Binance.Client.Websocket.Responses.Trades;

namespace Binance.Client.Websocket.Client
{
    /// <summary>
    /// All provided streams.
    /// You need to send subscription request in advance (via method `Send()` on BitmexWebsocketClient)
    /// </summary>
    public class BinanceClientStreams
    {
        internal readonly Subject<PongResponse> PongSubject = new Subject<PongResponse>();

        internal readonly Subject<TradeResponse> TradesSubject = new Subject<TradeResponse>();
        internal readonly Subject<AggregatedTradeResponse> TradeBinSubject = new Subject<AggregatedTradeResponse>();
        internal readonly Subject<BookResponse> BookSubject = new Subject<BookResponse>();


        // PUBLIC

        /// <summary>
        /// Response stream to every ping request
        /// </summary>
        public IObservable<PongResponse> PongStream => PongSubject.AsObservable();

        /// <summary>
        /// Trades stream - emits every executed trade on Bitmex
        /// </summary>
        public IObservable<TradeResponse> TradesStream => TradesSubject.AsObservable();

        /// <summary>
        /// Chunk of trades - emits grouped trades together
        /// </summary>
        public IObservable<AggregatedTradeResponse> AggregateTradesStream => TradeBinSubject.AsObservable();

        /// <summary>
        /// Order book stream - emits every update in the order book
        /// </summary>
        public IObservable<BookResponse> BookStream => BookSubject.AsObservable();
    }
}
