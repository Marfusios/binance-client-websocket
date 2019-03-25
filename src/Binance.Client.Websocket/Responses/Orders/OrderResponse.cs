using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Orders
{
    public class OrderResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Order;

        public Order[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<OrderResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "order")
                return false;

            var parsed = response.ToObject<OrderResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
