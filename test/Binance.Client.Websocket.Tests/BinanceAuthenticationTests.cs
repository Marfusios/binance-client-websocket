using Binance.Client.Websocket.Utils;
using Xunit;

namespace Binance.Client.Websocket.Tests
{
    public class BinanceAuthenticationTests
    {
        [Fact]
        public void CreateSignature_ShouldReturnCorrectString()
        {
            var nonce = BinanceAuthentication.CreateAuthNonce(123456);
            var payload = BinanceAuthentication.CreateAuthPayload(nonce);
            var signature = BinanceAuthentication.CreateSignature(payload, "api_secret");

            Assert.Equal("7657aa8b00b54ee7d58ed0ed42b6cad6d8b1e008bee4617b70d11cd87dbbc1e6", signature);
        }
    }
}
