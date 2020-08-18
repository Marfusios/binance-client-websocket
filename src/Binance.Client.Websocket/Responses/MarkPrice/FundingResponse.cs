using System.Linq;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.MarkPrice
{
    public class FundingResponse: ResponseBase<Funding>
    {
        internal static bool TryHandle(JObject response, ISubject<FundingResponse> subject)
        {
            var stream = response?["stream"]?.Value<string>();
            
            if (stream == null)
                return false;
            
            if (!stream.Contains("markPrice"))
                return false;

            var parsed = response.ToObject<FundingResponse>(BinanceJsonSerializer.Serializer);
            parsed.Data.Symbol = stream.Split('@').FirstOrDefault();
            subject.OnNext(parsed);

            return true;
        }
    }
}