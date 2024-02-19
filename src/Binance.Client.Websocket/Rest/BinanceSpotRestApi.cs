using System.Net.Http;
using Binance.Client.Websocket.Signing;

namespace Binance.Client.Websocket.Rest
{
    public abstract class BinanceSpotRestApi : BinanceRestApi
    {
        protected const string DEFAULT_SPOT_BASE_URL = "https://api.binance.com";

        public BinanceSpotRestApi(HttpClient httpClient, string? apiKey, string? apiSecret, string baseUrl = DEFAULT_SPOT_BASE_URL)
        : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, apiSecret: apiSecret)
        {
        }

        public BinanceSpotRestApi(HttpClient httpClient, string? apiKey, IBinanceSignatureService signatureService, string baseUrl = DEFAULT_SPOT_BASE_URL)
        : base(httpClient, baseUrl: baseUrl, apiKey: apiKey, signatureService: signatureService)
        {
        }
    }
}