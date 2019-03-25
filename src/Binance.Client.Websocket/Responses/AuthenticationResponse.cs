using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses
{
    public class AuthenticationResponse : MessageBase
    {
        public override MessageType Op => MessageType.AuthKey;

        public bool Success { get; set; }

        internal static bool TryHandle(JObject response, ISubject<AuthenticationResponse> subject)
        {
            if (response?["request"]?["op"]?.Value<string>() == "authKey")
            {
                var parsed = response.ToObject<AuthenticationResponse>(BinanceJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }
            return false;
        }
    }
}
