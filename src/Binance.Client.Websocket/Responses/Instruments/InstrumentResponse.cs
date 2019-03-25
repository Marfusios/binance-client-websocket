using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Instruments
{
    public class InstrumentResponse : ResponseBase
    {
        /// <inheritdoc />
        public override MessageType Op => MessageType.Instrument;

        public Instrument[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<InstrumentResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "instrument")
                return false;

            var parsed = response.ToObject<InstrumentResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
