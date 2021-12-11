using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Binance.Client.Websocket.Json;

/// <summary>
/// Binance preconfigured JSON serializer
/// </summary>
public static class BinanceJsonSerializer
{
    /// <summary>
    /// Inject a logger to use in deserialization.
    /// </summary>
    /// <param name="logger">The logger to use.</param>
    public static void Initialize(ILogger logger)
    {
        Settings = new()
        {
            ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            Formatting = Formatting.None,
            Converters = new List<JsonConverter>
            {
                new BinanceStringEnumConverter(logger)
            }
        };
        Serializer = JsonSerializer.Create(Settings);
    }

    /// <summary>
    /// JSON settings
    /// </summary>
    public static JsonSerializerSettings Settings { get; private set; }

    /// <summary>
    /// Serializer instance
    /// </summary>
    public static JsonSerializer Serializer { get; private set; }

    /// <summary>
    /// Deserialize string into object
    /// </summary>
    public static T Deserialize<T>(string data)
    {
        return JsonConvert.DeserializeObject<T>(data, Settings);
    }

    /// <summary>
    /// Serialize object into JSON string
    /// </summary>
    public static string Serialize(object data)
    {
        return JsonConvert.SerializeObject(data, Settings);
    }
}