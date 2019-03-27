using System;
using System.Collections.Generic;
using System.Linq;
using Binance.Client.Websocket.Responses.Trades;

namespace Binance.Client.Websocket.Sample.WinForms.Statistics
{
    class TradeStatsComputer
    {
        private readonly List<Trade> _lastTrades = new List<Trade>();

        public void HandleTrade(Trade newTrade)
        {
            _lastTrades.Add(newTrade);
        }

        public TradeStats GetStatsFor(int minutes)
        {
            var timeLimit = DateTime.UtcNow.Subtract(TimeSpan.FromMinutes(minutes));
            var trades = _lastTrades.Where(x => x.TradeTime >= timeLimit).ToArray();

            var buys = trades.Where(x => x.Side == TradeSide.Buy).Sum(x => x.Quantity);
            var sells = trades.Where(x => x.Side == TradeSide.Sell).Sum(x => x.Quantity);

            if(buys <= 0 && sells <= 0)
                return TradeStats.NULL;

            //var relative = (buys - sells - 0.0) / (buys + sells + 0.0);
            //var relativePerc = relative * 100;

            //var buysPerc = relative >= 0 ? relativePerc : 100 + relativePerc;
            //var sellsPerc = relative <= 0 ? Math.Abs(relativePerc) : 100 - relativePerc;

            var total = buys + sells + 0.0;

            var buysPerc = buys / total * 100;
            var sellsPerc = sells / total * 100;

            var count = trades.Length;

            return new TradeStats(buysPerc, sellsPerc, count);
        }
    }


    class TradeStats
    {
        public static readonly TradeStats NULL = new TradeStats(0, 0, 0);

        public TradeStats(double buysPerc, double sellsPerc, int totalCount)
        {
            BuysPerc = buysPerc;
            SellsPerc = sellsPerc;
            TotalCount = totalCount;
        }

        public double BuysPerc { get; }
        public double SellsPerc { get; }

        public int TotalCount { get; }
    }

}
