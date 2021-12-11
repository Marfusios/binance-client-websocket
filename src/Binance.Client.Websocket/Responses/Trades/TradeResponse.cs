using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Trades;

/// <summary>
/// Trades response
/// </summary>
public class TradeResponse : ResponseBase<Trade>
{
    internal static bool TryHandle(JObject response, ISubject<TradeResponse> subject)
    {
        var stream = response?["stream"]?.Value<string>();
        if (stream == null || !stream.ToLower().EndsWith("@trade"))
            return false;

        var parsed = response.ToObject<TradeResponse>(BinanceJsonSerializer.Serializer);
        subject.OnNext(parsed);

        return true;
    }
}