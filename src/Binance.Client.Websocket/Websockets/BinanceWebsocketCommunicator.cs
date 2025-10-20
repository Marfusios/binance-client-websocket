using System;
using System.Net.Http;
using System.Net.WebSockets;
using System.Reactive.Linq;
using System.Threading.Tasks;
using Binance.Client.Websocket.Communicator;
using Binance.Client.Websocket.Exceptions;
using Binance.Client.Websocket.Rest;
using Binance.Client.Websocket.Signing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Websocket.Client;

namespace Binance.Client.Websocket.Websockets
{
    /// <inheritdoc cref="WebsocketClient" />
    public class BinanceWebsocketCommunicator : WebsocketClient, IBinanceCommunicator
    {
        private readonly ILogger<BinanceWebsocketCommunicator> _logger;
        private BinanceUserRestApi? _userApi;
        private IDisposable? _disconnectionStream;
        private IDisposable? _timerStream;
        private string? _listenKey;

        /// <inheritdoc />
        public BinanceWebsocketCommunicator(Uri url, Func<ClientWebSocket>? clientFactory = null)
            : base(url, clientFactory)
        {
            _logger = NullLogger<BinanceWebsocketCommunicator>.Instance;
        }

        /// <inheritdoc />
        public BinanceWebsocketCommunicator(Uri url, ILogger<BinanceWebsocketCommunicator> logger, Func<ClientWebSocket>? clientFactory = null)
            : base(url, logger, clientFactory)
        {
            _logger = logger;
        }

        public BinanceUserStreamType StreamType => Url.Host.Contains("fstream") ? BinanceUserStreamType.Futures : BinanceUserStreamType.Spot;

        public async Task Authenticate(string apiKey, IBinanceSignatureService signature)
        {
            // TODO: use IHttpClientFactory
            var http = new HttpClient();
            var baseUrl = StreamType switch
            {
                BinanceUserStreamType.Futures => BinanceValues.FuturesRestApiBaseUrl,
                _ => BinanceValues.SpotRestApiBaseUrl
            };
            _userApi = new BinanceUserRestApi(httpClient: http, signatureService: signature, baseUrl: baseUrl, apiKey: apiKey);

            await AuthenticateUrl();

            _disconnectionStream = DisconnectionHappened
                .Subscribe(x =>
            {
                if (x.Type == DisconnectionType.ByUser || x.Type == DisconnectionType.Exit)
                    return;
                AuthenticateUrl().Wait();
            });

            _timerStream = Observable
                .Timer(TimeSpan.FromMinutes(10), TimeSpan.FromMinutes(10))
                .Subscribe(x =>
                {
                    if (_userApi == null || string.IsNullOrWhiteSpace(_listenKey))
                        return;
                    _logger.LogInformation("Refreshing listen key to keep it alive");
                    _ = PingListenKey();
                });
        }

        public new void Dispose()
        {
            _userApi = null;
            _disconnectionStream?.Dispose();
            _timerStream?.Dispose();
            base.Dispose();
        }

        private async Task AuthenticateUrl()
        {
            if (_userApi == null)
                return;

            _logger.LogInformation("Getting a new listen key for authenticated websocket connection");
            switch (StreamType)
            {
                case BinanceUserStreamType.Futures:
                    {
                        var response = await _userApi.CreateFuturesListenKey();
                        _listenKey = response.ListenKey;
                        if (string.IsNullOrWhiteSpace(_listenKey))
                            throw new BinanceException("Listen key is empty, cannot authenticate websocket connection");

                        var newUrl = BinanceValues.UserFuturesWebsocketUrl(_listenKey);
                        Url = newUrl;
                        break;
                    }
                default:
                    {
                        var response = await _userApi.CreateSpotListenKey();
                        _listenKey = response.ListenKey;
                        if (string.IsNullOrWhiteSpace(_listenKey))
                            throw new BinanceException("Listen key is empty, cannot authenticate websocket connection");

                        var newUrl = BinanceValues.UserWebsocketUrl(_listenKey);
                        Url = newUrl;
                        break;
                    }
            }

            _logger.LogInformation("The new listen key was obtained, changing websocket url to {url}", Url);
        }

        private async Task PingListenKey()
        {
            if (_userApi == null || string.IsNullOrWhiteSpace(_listenKey))
                return;

            try
            {
                switch (StreamType)
                {
                    case BinanceUserStreamType.Futures:
                        await _userApi.PingFuturesListenKey(_listenKey);
                        break;
                    default:
                        await _userApi.PingSpotListenKey(_listenKey);
                        break;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to refresh listen key");
            }
        }
    }
}
