using System.Linq;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Binance.Client.Websocket.Responses.Books;

/// <summary>
/// Partial order book response
/// </summary>
public class OrderBookPartialResponse : ResponseBase<OrderBookPartial>
{

    internal static bool TryHandle(JObject response, ISubject<OrderBookPartialResponse> subject)
    {
        var stream = response?["stream"]?.Value<string>();
        if (stream == null)
            return false;

        if (!stream.Contains("depth"))
            return false;

        if (stream.EndsWith("depth"))
        {
            // ignore, not partial, but diff response
            return false;
        }

        var parsed = response.ToObject<OrderBookPartialResponse>(BinanceJsonSerializer.Serializer);
        parsed.Data.Symbol = stream.Split('@').FirstOrDefault();
        subject.OnNext(parsed);

        return true;
    }

    /// <summary>
    /// Stream snapshot manually via communicator
    /// </summary>
    public static void StreamFakeSnapshot(OrderBookPartial snapshot, IWebsocketClient client)
    {
        var symbolSafe = (snapshot?.Symbol ?? string.Empty).ToLower();
        var countSafe = snapshot?.Bids?.Length ?? 0;
        var response = new OrderBookPartialResponse
        {
            Data = snapshot,
            Stream = $"{symbolSafe}@depth{countSafe}"
        };

        var serialized = JsonConvert.SerializeObject(response, BinanceJsonSerializer.Settings);
        client.StreamFakeMessage(ResponseMessage.TextMessage(serialized));
    }
}