using System.Linq;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Books;

/// <summary>
/// Order book difference response
/// </summary>
public class OrderBookDiffResponse : ResponseBase<OrderBookDiff>
{

    internal static bool TryHandle(JObject response, ISubject<OrderBookDiffResponse> subject)
    {
        var stream = response?["stream"]?.Value<string>();
        if (stream == null)
            return false;

        if (!stream.EndsWith("depth"))
        {
            // ignore, not order book diff
            return false;
        }

        var parsed = response.ToObject<OrderBookDiffResponse>(BinanceJsonSerializer.Serializer);
        subject.OnNext(parsed);

        return true;
    }
}