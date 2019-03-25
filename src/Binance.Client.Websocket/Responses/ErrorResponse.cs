using System.Collections.Generic;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses
{
    public class ErrorResponse : MessageBase
    {
        public override MessageType Op => MessageType.Error;

        public double? Status { get; set; }
        public string Error { get; set; }
        public Dictionary<string, object> Meta { get; set; }
        public Dictionary<string, object> Request { get; set; }

        internal static bool TryHandle(JObject response, ISubject<ErrorResponse> subject)
        {
            if (response?["error"] != null)
            {
                var parsed = response.ToObject<ErrorResponse>(BinanceJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }
            return false;
        }
    }
}
