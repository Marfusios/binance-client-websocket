using System;

namespace Binance.Client.Websocket.Utils
{
    /// <summary>
    /// Utils for time
    /// </summary>
    public static class BinanceTime
    {
        /// <summary>
        /// Base Unix time (1.1.1970)
        /// </summary>
        public static readonly DateTime UnixBase = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

        /// <summary>
        /// Return current total Unix milliseconds
        /// </summary>
        /// <returns></returns>
        public static long NowMs()
        {
            var subtracted = DateTime.UtcNow.Subtract(UnixBase);
            return (long)subtracted.TotalMilliseconds;
        }

        /// <summary>
        /// Convert Unix milliseconds into DateTime
        /// </summary>
        public static DateTime ConvertToTime(long timeInMs)
        {
            return UnixBase.AddMilliseconds(timeInMs);
        }
    }
}
