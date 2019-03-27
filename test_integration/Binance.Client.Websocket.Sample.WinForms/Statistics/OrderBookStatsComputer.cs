using System.Linq;
using Binance.Client.Websocket.Responses.Books;

namespace Binance.Client.Websocket.Sample.WinForms.Statistics
{
    class OrderBookStatsComputer
    {
        private OrderBookLevel[] _bids = new OrderBookLevel[0];
        private OrderBookLevel[] _asks = new OrderBookLevel[0];


        public void HandleOrderBook(OrderBookPartialResponse response)
        {
            var ob = response.Data;

            _bids = ob.Bids;
            _asks = ob.Asks;
        }

        public OrderBookStats GetStats()
        {
            var bids = _bids.OrderByDescending(x => x.Price).ToArray();
            var asks = _asks.OrderBy(x => x.Price).ToArray();

            if(!bids.Any() || !asks.Any())
                return OrderBookStats.NULL;

            var bidAmounts = bids.Take(20).Sum(x => x.Quantity * x.Price);
            var askAmounts = asks.Take(20).Sum(x => x.Quantity * x.Price);

            var total = bidAmounts + askAmounts + 0.0;

            var bidsPerc = bidAmounts / total * 100;
            var asksPerc = askAmounts / total * 100;

            return new OrderBookStats(
                bids[0].Price,
                asks[0].Price,
                bidsPerc,
                asksPerc,
                bidAmounts,
                askAmounts
                );
        }
    }

    class OrderBookStats
    {
        public static readonly OrderBookStats NULL = new OrderBookStats(0, 0, 0, 0, 0, 0);

        public OrderBookStats(double bid, double ask, double bidAmountPerc, double askAmountPerc, 
            double bidAmount, double askAmount)
        {
            Bid = bid;
            Ask = ask;
            BidAmountPerc = bidAmountPerc;
            AskAmountPerc = askAmountPerc;
            BidAmount = bidAmount;
            AskAmount = askAmount;
        }

        public double Bid { get; }
        public double Ask { get; }

        public double BidAmountPerc { get; }
        public double AskAmountPerc { get; }

        public double BidAmount { get; }
        public double AskAmount { get; }
    }
}
