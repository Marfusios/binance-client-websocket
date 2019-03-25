using System.Collections.Generic;
using System.Linq;
using Binance.Client.Websocket.Exceptions;

namespace Binance.Client.Websocket.Validations
{
    internal static class BnbValidations
    {
         /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if value is null or empty/white spaces
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <param name="name">Input parameter name</param>
        public static void ValidateInput(string value, string name)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                throw new BinanceBadInputException($"Input string parameter '{name}' is null or empty. Please correct it.");
            }
        }

        /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if value is null
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <param name="name">Input parameter name</param>
        public static void ValidateInput<T>(T value, string name)
        {
            if (Equals(value, default(T)))
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is null. Please correct it.");
            }
        }

        /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if collection is null or collection is empty
        /// </summary>
        /// <param name="collection">The collection to be validated</param>
        /// <param name="name">Input parameter name</param>
        public static void ValidateInputCollection<T>(IEnumerable<T> collection, string name)
        {
            // ReSharper disable once PossibleMultipleEnumeration
            ValidateInput(collection, name);

            // ReSharper disable once PossibleMultipleEnumeration
            if (!collection.Any())
            {
                throw new BinanceBadInputException($"Input collection '{name}' is empty. Please correct it.");
            }
        }

        /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if value is not in specified range
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <param name="name">Input parameter name</param>
        /// <param name="minValue">Minimal value of input</param>
        /// <param name="maxValue">Maximum value of input</param>
        public static void ValidateInput(int value, string name, int minValue = int.MinValue, int maxValue = int.MaxValue)
        {
            if (value < minValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is lower than {minValue}. Please correct it.");
            }
            if (value > maxValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is higher than {maxValue}. Please correct it.");
            }
        }

        /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if value is not in specified range
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <param name="name">Input parameter name</param>
        /// <param name="minValue">Minimal value of input</param>
        /// <param name="maxValue">Maximum value of input</param>
        public static void ValidateInput(long value, string name, long minValue = long.MinValue, long maxValue = long.MaxValue)
        {
            if (value < minValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is lower than {minValue}. Please correct it.");
            }
            if (value > maxValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is higher than {maxValue}. Please correct it.");
            }
        }

        /// <summary>
        /// It throws <exception cref="BinanceBadInputException"></exception> if value is not in specified range
        /// </summary>
        /// <param name="value">The value to be validated</param>
        /// <param name="name">Input parameter name</param>
        /// <param name="minValue">Minimal value of input</param>
        /// <param name="maxValue">Maximum value of input</param>
        public static void ValidateInput(double value, string name, double minValue = double.MinValue, double maxValue = double.MaxValue)
        {
            if (value < minValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is lower than {minValue}. Please correct it.");
            }
            if (value > maxValue)
            {
                throw new BinanceBadInputException($"Input parameter '{name}' is higher than {maxValue}. Please correct it.");
            }
        }
    }
}
