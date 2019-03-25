using System;

namespace Binance.Client.Websocket.Exceptions
{
    public class BinanceBadInputException : BinanceException
    {
        public BinanceBadInputException()
        {
        }

        public BinanceBadInputException(string message) : base(message)
        {
        }

        public BinanceBadInputException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
