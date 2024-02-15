using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Binance.Client.Websocket.Json
{
    /// <summary>
    /// Enum converter - convert enum as string
    /// </summary>
    public class BinanceStringEnumConverter : StringEnumConverter
    {
        /// <summary>
        /// Read JSON string and convert to enum
        /// </summary>
        public override object? ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
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
                return existingValue;
            }
        }
    }
}
