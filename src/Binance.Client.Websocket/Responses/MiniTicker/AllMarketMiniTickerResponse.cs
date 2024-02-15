using System.Collections.Generic;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.MiniTicker
{
    public class AllMarketMiniTickerResponse : ResponseBase<List<MiniTicker>>
    {
        internal static bool TryHandle(JObject response, ISubject<AllMarketMiniTickerResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
                return false;

            if (!stream.EndsWith("miniTicker@arr"))
            {
                return false;
            }

            var parsed = response!.ToObject<AllMarketMiniTickerResponse>(BinanceJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}