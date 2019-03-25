using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Wallets
{
    public class WalletResponse : ResponseBase
    {
        public override MessageType Op => MessageType.Wallet;

        public Wallet[] Data { get; set; }

        internal static bool TryHandle(JObject response, ISubject<WalletResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "wallet")
                return false;

            var parsed = response.ToObject<WalletResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
