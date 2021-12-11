using System;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace Binance.Client.Websocket.Json;

/// <summary>
/// Enum converter - convert enum as string
/// </summary>
public class BinanceStringEnumConverter : StringEnumConverter
{
    readonly ILogger _logger;

    /// <summary>
    /// Creates a new instance.
    /// </summary>
    public BinanceStringEnumConverter(ILogger logger)
    {
        _logger = logger;
        NamingStrategy = new CamelCaseNamingStrategy();
    }

    /// <summary>
    /// Read JSON string and convert to enum
    /// </summary>
    public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
    {
        try
        {
            var val = reader.Value;
            if (val is string valS && string.IsNullOrWhiteSpace(valS))
            {
                // received empty string, can't parse to enum, use default enum value (first)
                return existingValue;
            }
            return base.ReadJson(reader, objectType, existingValue, serializer);
        }
        catch
        {
            _logger.LogWarning($"Can't parse enum, value: {reader.Value}, target type: {objectType}, using default '{existingValue}'");
            return existingValue;
        }
    }
}