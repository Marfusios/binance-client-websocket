using System;
using System.Globalization;
using Binance.Client.Websocket.Utils;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Binance.Client.Websocket.Json
{
    public class UnixDateTimeConverter : DateTimeConverterBase
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var subtracted = ((DateTime)value).Subtract(BitmexTime.UnixBase);
            writer.WriteRawValue(subtracted.TotalMilliseconds.ToString(CultureInfo.InvariantCulture));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            if (reader.Value == null) { return null; }
            return BitmexTime.ConvertToTime((long)reader.Value);
        }
    }
}
