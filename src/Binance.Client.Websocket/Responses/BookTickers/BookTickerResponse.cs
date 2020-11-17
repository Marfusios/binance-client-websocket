using System;
using System.Diagnostics;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.BookTickers
{
    public class BookTickerResponse : ResponseBase<BookTickers.BookTicker>
    {
        internal static bool TryHandle(JObject response, ISubject<BookTickerResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
            {
                return false;
            }

            if (!stream.EndsWith("bookTicker"))
            {
                return false;
            }

            var parsed = response.ToObject<BookTickerResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}