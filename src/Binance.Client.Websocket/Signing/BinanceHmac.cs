using System;
using System.Security.Cryptography;
using System.Text;

namespace Binance.Client.Websocket.Signing
{
    /// <summary>
    /// Binance HMAC signature signing.
    /// </summary>
    public class BinanceHmac : IBinanceSignatureService
    {
        private readonly byte[]? _secret;

        public BinanceHmac(string? secret)
        {
            _secret = secret != null ? Encoding.UTF8.GetBytes(secret) : 
                null;
        }

        public string Sign(string payload)
        {
            using var hmacSha256 = new HMACSHA256(_secret ?? throw new InvalidOperationException("Secret cannot be null"));
            var payloadBytes = Encoding.UTF8.GetBytes(payload);

            var hash = hmacSha256.ComputeHash(payloadBytes);

            return BitConverter.ToString(hash).Replace("-", string.Empty).ToLower();
        }
    }
}