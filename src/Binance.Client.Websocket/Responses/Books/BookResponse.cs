using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Books
{
    public class BookResponse : ResponseBase<BookLevel>
    {

        internal static bool TryHandle(JObject response, ISubject<BookResponse> subject)
        {
            if (response?["table"]?.Value<string>() != "orderBookL2")
                return false;

            var parsed = response.ToObject<BookResponse>(BinanceJsonSerializer.Serializer);
            subject.OnNext(parsed);

            return true;
        }
    }
}
