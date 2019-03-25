using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Quotes
{
    public class QuoteResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Quote;

        public Quote[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<QuoteResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "quote")
                return false;

            var parsed = response.ToObject<QuoteResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
