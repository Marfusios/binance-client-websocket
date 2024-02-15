using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.MiniTicker
{
    public class MiniTickerResponse : ResponseBase<MiniTicker>
    {
        internal static bool TryHandle(JObject response, ISubject<MiniTickerResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            if (stream == null)
                return false;

            if (!stream.EndsWith("miniTicker"))
            {
                return false;
            }

            var parsed = response!.ToObject<MiniTickerResponse>(BinanceJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}