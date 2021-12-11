using System;
using System.Linq;
using Binance.Client.Websocket.Exceptions;
using Binance.Client.Websocket.Json;
using Binance.Client.Websocket.Responses;
using Binance.Client.Websocket.Responses.AggregateTrades;
using Binance.Client.Websocket.Responses.Books;
using Binance.Client.Websocket.Responses.BookTickers;
using Binance.Client.Websocket.Responses.Kline;
using Binance.Client.Websocket.Responses.MarkPrice;
using Binance.Client.Websocket.Responses.MiniTicker;
using Binance.Client.Websocket.Responses.Trades;
using Binance.Client.Websocket.Subscriptions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Websocket.Client;

namespace Binance.Client.Websocket.Client;

/// <summary>
/// Binance websocket client.
/// Use method `SetSubscriptions(...)` to subscribe to channels.
/// And `Streams` to handle messages.
/// </summary>
public class BinanceWebsocketClient : IBinanceWebsocketClient
{
    readonly ILogger _logger;

    readonly IWebsocketClient _client;
    readonly IDisposable _messageReceivedSubscription;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    /// <param name="logger">The logger to use for logging any warnings or errors.</param>
    /// <param name="client">The client to use for the websocket.</param>
    /// <param name="subscriptions">The required subscriptions.</param>
    public BinanceWebsocketClient(ILogger logger, IWebsocketClient client, params SubscriptionBase[] subscriptions)
    {
        if (subscriptions == null) throw new ArgumentNullException(nameof(subscriptions));

        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _client = client ?? throw new ArgumentNullException(nameof(client));
        _messageReceivedSubscription = _client.MessageReceived.Subscribe(HandleMessage);

        SetSubscriptions(subscriptions);
        BinanceJsonSerializer.Initialize(_logger);
    }

    /// <inheritdoc />
    public BinanceClientStreams Streams { get; } = new();

    /// <summary>
    /// Cleanup everything
    /// </summary>
    public void Dispose()
    {
        _messageReceivedSubscription?.Dispose();
    }

    static Uri PrepareSubscriptions(Uri baseUrl, params SubscriptionBase[] subscriptions)
    {
        if (subscriptions == null || !subscriptions.Any())
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

    void SetSubscriptions(params SubscriptionBase[] subscriptions)
    {
        var newUrl = PrepareSubscriptions(_client.Url, subscriptions);
        _client.Url = newUrl;
    }

    /// <inheritdoc />
    public void Send<T>(T request)
    {
        if (request == null) throw new ArgumentNullException(nameof(request));
        try
        {
            var serialized = BinanceJsonSerializer.Serialize(request);
            _client.Send(serialized);
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage($"Exception while sending message '{request}'. Error: {e.Message}"));
            throw;
        }
    }

    static string LogMessage(string message) => $"[BNB WEBSOCKET CLIENT] {message}";

    void HandleMessage(ResponseMessage message)
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

            _logger.LogWarning(LogMessage($"Unhandled response:  '{messageSafe}'"));
        }
        catch (Exception e)
        {
            _logger.LogError(e, LogMessage("Exception while receiving message"));
        }
    }

    bool HandleRawMessage(string msg) => PongResponse.TryHandle(msg, Streams.PongStream);

    bool HandleObjectMessage(string msg)
    {
        var response = BinanceJsonSerializer.Deserialize<JObject>(msg);

        return
            TradeResponse.TryHandle(response, Streams.TradesStream) ||
            AggregatedTradeResponse.TryHandle(response, Streams.AggregateTradesStream) ||
            OrderBookPartialResponse.TryHandle(response, Streams.OrderBookPartialStream) ||
            OrderBookDiffResponse.TryHandle(response, Streams.OrderBookDiffStream) ||
            FundingResponse.TryHandle(response, Streams.FundingStream) ||
            BookTickerResponse.TryHandle(response, Streams.BookTickerStream) ||
            KlineResponse.TryHandle(response, Streams.KlineStream) ||
            MiniTickerResponse.TryHandle(response, Streams.MiniTickerStream);
    }
}