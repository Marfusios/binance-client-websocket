using System;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses
{
    /// <summary>
    /// Listen key expired response.
    /// This event will be received when the listenKey used for the user data stream expires.
    /// No more user data events will be updated after this event is received until a new valid listenKey is used.
    /// </summary>
    public class ListenKeyExpiredResponse : MessageBase
    {
        /// <summary>
        /// The expired listen key
        /// </summary>
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; } = string.Empty;

        internal static bool TryHandle(JObject? response, ISubject<ListenKeyExpiredResponse> subject)
        {
            var eventType = response?["e"]?.Value<string>();
            if (eventType != "listenKeyExpired")
                return false;

            var parsed = response!.ToObject<ListenKeyExpiredResponse>(BinanceJsonSerializer.Serializer);
            if (parsed != null)
                subject.OnNext(parsed);

            return true;
        }
    }
}
