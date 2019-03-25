using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Positions
{
    public class PositionResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Position;

        public Position[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<PositionResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "position")
                return false;

            var parsed = response.ToObject<PositionResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
