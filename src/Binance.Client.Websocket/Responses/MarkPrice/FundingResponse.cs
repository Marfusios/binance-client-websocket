using System.Linq;
using System;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.MarkPrice
{
    public class FundingResponse : ResponseBase<Funding>
    {
        internal static bool TryHandle(JObject response, ISubject<FundingResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();

            if (stream == null)
                return false;

            if (stream.IndexOf("markPrice", StringComparison.Ordinal) < 0)
                return false;

            var parsed = response!.ToObject<FundingResponse>(BinanceJsonSerializer.Serializer);
            if (parsed != null)
            {
                parsed.Data.Symbol = GetSymbol(stream);
                subject.OnNext(parsed);
            }

            return true;
        }

        private static string GetSymbol(string stream)
        {
            var separator = stream.IndexOf('@');
            return separator >= 0 ? stream.Substring(0, separator) : stream;
        }
    }
}
