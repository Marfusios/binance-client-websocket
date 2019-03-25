namespace Binance.Client.Websocket.Requests
{
    public class MarginSubscribeRequest : SubscribeRequestBase
    {
        public override string Topic => "margin";
    }
}
