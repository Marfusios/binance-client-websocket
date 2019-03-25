using Binance.Client.Websocket.Messages;
using Binance.Client.Websocket.Utils;
using Binance.Client.Websocket.Validations;

namespace Binance.Client.Websocket.Requests
{
    public class AuthenticationRequest : RequestBase
    {
        private readonly string _apiKey;
        private readonly string _authSig;
        private readonly long _authNonce;
        private readonly string _authPayload;

        public AuthenticationRequest(string apiKey, string apiSecret)
        {
            BnbValidations.ValidateInput(apiKey, nameof(apiKey));
            BnbValidations.ValidateInput(apiSecret, nameof(apiSecret));

            _apiKey = apiKey;

            _authNonce = BinanceAuthentication.CreateAuthNonce();
            _authPayload = BinanceAuthentication.CreateAuthPayload(_authNonce);

            _authSig = BinanceAuthentication.CreateSignature(apiSecret, _authPayload);
        }

        public override MessageType Operation => MessageType.AuthKey;

        public object[] Args => new object[]
        {
            _apiKey,
            _authNonce,
            _authSig
        };
    }
}
