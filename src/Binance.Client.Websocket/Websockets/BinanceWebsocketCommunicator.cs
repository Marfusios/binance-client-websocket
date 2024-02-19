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

        public async Task Authenticate(string apiKey, IBinanceSignatureService signature)
        {
            // TODO: use IHttpClientFactory
            var http = new HttpClient();
            _userApi = new BinanceUserRestApi(apiKey: apiKey, signatureService: signature, httpClient: http);

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
                    _ = _userApi.PingSpotListenKey(_listenKey);
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
            var response = await _userApi.CreateSpotListenKey();
            _listenKey = response.ListenKey;
            if (string.IsNullOrWhiteSpace(_listenKey))
                throw new BinanceException("Listen key is empty, cannot authenticate websocket connection");
            
            var newUrl = BinanceValues.UserWebsocketUrl(_listenKey);
            Url = newUrl;
            _logger.LogInformation("The new listen key was obtained, changing websocket url to {url}", Url);
        }
    }
}
