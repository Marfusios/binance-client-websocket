using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Liquidation
{
    public class LiquidationResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Position;

        public Liquidation[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<LiquidationResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "liquidation")
                return false;

            var parsed = response.ToObject<LiquidationResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
