using System;
using System.Collections.Generic;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Messages;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses
{
    public class InfoResponse : MessageBase
    {
        public override MessageType Op => MessageType.Info;

        public string Info { get; set; }
        public DateTime Version { get; set; }
        public DateTime Timestamp { get; set; }
        public string Docs { get; set; }
        public Dictionary<string, object> Limit { get; set; }

        internal static bool TryHandle(JObject response, ISubject<InfoResponse> subject)
        {
            if (response?["info"] != null && response?["version"] != null)
            {
                var parsed = response.ToObject<InfoResponse>(BinanceJsonSerializer.Serializer);
                subject.OnNext(parsed);
                return true;
            }
            return false;
        }
    }
}
