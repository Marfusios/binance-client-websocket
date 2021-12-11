using System;

namespace Binance.Client.Websocket.Exceptions;

public class BinanceException : Exception
{
    public BinanceException()
    {
    }

    public BinanceException(string message)
        : base(message)
    {
    }

    public BinanceException(string message, Exception innerException) : base(message, innerException)
    {
    }
}