using System;
using System.Linq;
using System.Threading.Tasks;
using Binance.Client.Websocket.Communicator;
using Binance.Client.Websocket.Exceptions;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Logging;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Responses.AggregateTrades;
using Binance.Client.Websocket.Responses.Books;
using Binance.Client.Websocket.Responses.Trades;
using Binance.Client.Websocket.Subscriptions;
using Binance.Client.Websocket.Validations;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Binance.Client.Websocket.Client
{
    /// <summary>
    /// Binance websocket client.
    /// Use method `Connect()` to start client and subscribe to channels.
    /// And `Streams` to subscribe. 
    /// </summary>
    public class BinanceWebsocketClient : IDisposable
    {
        private static readonly ILog Log = LogProvider.GetCurrentClassLogger();

        private readonly IBinanceCommunicator _communicator;
        private readonly IDisposable _messageReceivedSubscription;

        /// <inheritdoc />
        public BinanceWebsocketClient(IBinanceCommunicator communicator)
        {
            BnbValidations.ValidateInput(communicator, nameof(communicator));

            _communicator = communicator;
            _messageReceivedSubscription = _communicator.MessageReceived.Subscribe(HandleMessage);
        }

        /// <summary>
        /// Provided message streams
        /// </summary>
        public BinanceClientStreams Streams { get; } = new BinanceClientStreams();

        /// <summary>
        /// Cleanup everything
        /// </summary>
        public void Dispose()
        {
            _messageReceivedSubscription?.Dispose();
        }

        /// <summary>
        /// Combine url with subscribed streams
        /// </summary>
        public Uri PrepareSubscriptions(Uri baseUrl, params SubscriptionBase[] subscriptions)
        {
            BnbValidations.ValidateInput(baseUrl, nameof(baseUrl));

            if(subscriptions == null || !subscriptions.Any())
                throw new BinanceBadInputException("Please provide at least one subscription");

            var streams = subscriptions.Select(x => x.StreamName).ToArray();
            var urlPart = string.Join("/", streams);
            var urlPartFull = $"/stream?streams={urlPart}";

            var currentUrl = baseUrl.ToString().Trim();

            if (currentUrl.Contains("stream?"))
            {
                // do nothing, already configured
                return baseUrl;
            }

            var newUrl = new Uri($"{currentUrl.TrimEnd('/')}{urlPartFull}");
            return newUrl;
        }

        /// <summary>
        /// Combine url with subscribed streams and set it into communicator.
        /// Then you need to call communicator.Start() or communicator.Reconnect()
        /// </summary>
        public void SetSubscriptions(params SubscriptionBase[] subscriptions)
        {
            var newUrl = PrepareSubscriptions(_communicator.Url, subscriptions);
            _communicator.Url = newUrl;
        }

        /// <summary>
        /// Serializes request and sends message via websocket communicator. 
        /// It logs and re-throws every exception. 
        /// </summary>
        /// <param name="request">Request/message to be sent</param>
        public async Task Send<T>(T request)
        {
            try
            {
                BnbValidations.ValidateInput(request, nameof(request));

                var serialized = BinanceJsonSerializer.Serialize(request);
                await _communicator.Send(serialized).ConfigureAwait(false);
            }
            catch (Exception e)
            {
                Log.Error(e, L($"Exception while sending message '{request}'. Error: {e.Message}"));
                throw;
            }
        }

        private string L(string msg)
        {
            return $"[BNB WEBSOCKET CLIENT] {msg}";
        }

        private void HandleMessage(ResponseMessage message)
        {
            try
            {
                bool handled;
                var messageSafe = (message.Text ?? string.Empty).Trim();

                if (messageSafe.StartsWith("{"))
                {
                    handled = HandleObjectMessage(messageSafe);
                    if (handled)
                        return;
                }

                handled = HandleRawMessage(messageSafe);
                if (handled)
                    return;

                Log.Warn(L($"Unhandled response:  '{messageSafe}'"));
            }
            catch (Exception e)
            {
                Log.Error(e, L("Exception while receiving message"));
            }
        }

        private bool HandleRawMessage(string msg)
        {
            // ********************
            // ADD RAW HANDLERS BELOW
            // ********************

            return
                PongResponse.TryHandle(msg, Streams.PongSubject);
        }

        private bool HandleObjectMessage(string msg)
        {
            var response = BinanceJsonSerializer.Deserialize<JObject>(msg);

            // ********************
            // ADD OBJECT HANDLERS BELOW
            // ********************

            return

                TradeResponse.TryHandle(response, Streams.TradesSubject) ||
                AggregatedTradeResponse.TryHandle(response, Streams.TradeBinSubject) ||
                OrderBookPartialResponse.TryHandle(response, Streams.OrderBookPartialSubject) || 
                OrderBookDiffResponse.TryHandle(response, Streams.OrderBookDiffSubject)
                ;
        }
    }
}
