using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Margins
{
    public class MarginResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Margin;

        public Margin[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<MarginResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "margin")
                return false;

            var parsed = response.ToObject<MarginResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
