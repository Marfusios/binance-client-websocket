using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Globalization;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Json
{
    /// <summary>
    /// Convert JSON array into object
    /// </summary>
    public class ArrayConverter : JsonConverter
    {
        private static readonly ConcurrentDictionary<Type, ArrayPropertyInfo[]> CachedProperties = new ConcurrentDictionary<Type, ArrayPropertyInfo[]>();

        public override bool CanConvert(Type objectType)
        {
            return true;
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = Activator.CreateInstance(objectType);
            var arr = JArray.Load(reader);
            return ParseObject(arr, result, objectType);
        }

        private static object ParseObject(JArray arr, object result, Type objectType)
        {
            foreach (var propertyInfo in GetArrayProperties(objectType))
            {
                if (propertyInfo.Index >= arr.Count)
                    continue;

                var property = propertyInfo.Property;
                if (propertyInfo.IsArray)
                {
                    var objType = propertyInfo.ElementType ?? throw new JsonSerializationException($"Array property '{property.Name}' has no element type.");
                    var innerArray = (JArray)arr[propertyInfo.Index];
                    var count = 0;
                    if (innerArray.Count == 0)
                    {
                        var arrayResult = (IList)Activator.CreateInstance(propertyInfo.PropertyType, new [] { 0 });
                        property.SetValue(result, arrayResult);
                    }
                    else if (innerArray[0].Type == JTokenType.Array)
                    {
                        var arrayResult = (IList)Activator.CreateInstance(propertyInfo.PropertyType, new [] { innerArray.Count });
                        foreach (var obj in innerArray)
                        {
                            var innerObj = Activator.CreateInstance(objType);
                            arrayResult[count] = ParseObject((JArray)obj, innerObj, objType);
                            count++;
                        }
                        property.SetValue(result, arrayResult);
                    }
                    else
                    {
                        var arrayResult = (IList)Activator.CreateInstance(propertyInfo.PropertyType, new [] { 1 });
                        var innerObj = Activator.CreateInstance(objType);
                        arrayResult[0] = ParseObject(innerArray, innerObj, objType);
                        property.SetValue(result, arrayResult);
                    }
                    continue;
                }

                var value = propertyInfo.ConverterSerializer != null
                    ? arr[propertyInfo.Index].ToObject(propertyInfo.PropertyType, propertyInfo.ConverterSerializer)
                    : arr[propertyInfo.Index];

                if (value != null && propertyInfo.PropertyType.IsInstanceOfType(value))
                    property.SetValue(result, value);
                else
                {
                    if (value is JToken token)
                        if (token.Type == JTokenType.Null)
                            value = null;

                    var stringValue = value?.ToString();
                    if ((propertyInfo.PropertyType == typeof(decimal)
                     || propertyInfo.PropertyType == typeof(decimal?))
                     && stringValue != null
                     && (stringValue.IndexOf('e') >= 0 || stringValue.IndexOf('E') >= 0))
                    {
                        if (decimal.TryParse(stringValue, NumberStyles.Float, CultureInfo.InvariantCulture, out var dec))
                            property.SetValue(result, dec);
                    }
                    else
                    {
                        property.SetValue(result, value == null ? null : Convert.ChangeType(value, propertyInfo.PropertyType));
                    }
                }
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteStartArray();

            var last = -1;
            foreach (var propertyInfo in GetArrayProperties(value.GetType()))
            {
                if (propertyInfo.Index == last)
                    continue;

                while (propertyInfo.Index != last + 1)
                {
                    writer.WriteValue((string)null);
                    last += 1;
                }

                last = propertyInfo.Index;
                if (propertyInfo.Converter != null)
                    writer.WriteRawValue(JsonConvert.SerializeObject(propertyInfo.Property.GetValue(value), propertyInfo.Converter));
                else if (!propertyInfo.IsSimple)
                    serializer.Serialize(writer, propertyInfo.Property.GetValue(value));
                else
                    writer.WriteValue(propertyInfo.Property.GetValue(value));
            }
            writer.WriteEndArray();
        }

        private static ArrayPropertyInfo[] GetArrayProperties(Type type)
        {
            return CachedProperties.GetOrAdd(type, CreateArrayProperties);
        }

        private static ArrayPropertyInfo[] CreateArrayProperties(Type type)
        {
            var properties = type.GetProperties();
            var result = new ArrayList(properties.Length);

            foreach (var property in properties)
            {
                var attribute = (ArrayPropertyAttribute)property.GetCustomAttribute(typeof(ArrayPropertyAttribute));
                if (attribute == null)
                    continue;

                result.Add(new ArrayPropertyInfo(property, attribute.Index));
            }

            var items = (ArrayPropertyInfo[])result.ToArray(typeof(ArrayPropertyInfo));
            Array.Sort(items, (left, right) => left.Index.CompareTo(right.Index));
            return items;
        }

        private static JsonConverter CreateConverter(JsonConverterAttribute attribute)
        {
            return (JsonConverter)Activator.CreateInstance(attribute.ConverterType, attribute.ConverterParameters);
        }

        private static bool IsSimple(Type type)
        {
            if (type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>))
            {
                // nullable type, check if the nested type is simple.
                return IsSimple(type.GetGenericArguments()[0]);
            }
            return type.IsPrimitive
              || type.IsEnum
              || type == typeof(string)
              || type == typeof(decimal);
        }

        private sealed class ArrayPropertyInfo
        {
            public ArrayPropertyInfo(PropertyInfo property, int index)
            {
                Property = property;
                Index = index;
                PropertyType = property.PropertyType;
                ElementType = PropertyType.GetElementType();
                IsArray = PropertyType.BaseType == typeof(Array);
                IsSimple = IsSimple(PropertyType);

                var converterAttribute = (JsonConverterAttribute)property.GetCustomAttribute(typeof(JsonConverterAttribute))
                    ?? (JsonConverterAttribute)PropertyType.GetCustomAttribute(typeof(JsonConverterAttribute));
                if (converterAttribute != null)
                {
                    Converter = CreateConverter(converterAttribute);
                    ConverterSerializer = new JsonSerializer();
                    ConverterSerializer.Converters.Add(Converter);
                }
            }

            public PropertyInfo Property { get; }

            public int Index { get; }

            public Type PropertyType { get; }

            public Type? ElementType { get; }

            public bool IsArray { get; }

            public bool IsSimple { get; }

            public JsonConverter? Converter { get; }

            public JsonSerializer? ConverterSerializer { get; }
        }
    }

    /// <summary>
    /// JSON array property
    /// </summary>
    public class ArrayPropertyAttribute: Attribute
    {
        /// <summary>
        /// Index of the item
        /// </summary>
        public int Index { get; }

        /// <summary>
        /// Create array property attribute with specified index
        /// </summary>
        public ArrayPropertyAttribute(int index)
        {
            Index = index;
        }
    }
}
