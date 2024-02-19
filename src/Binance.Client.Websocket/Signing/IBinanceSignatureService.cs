namespace Binance.Client.Websocket.Signing
{
    /// <summary>
    /// Interface for signing payloads.
    /// </summary>
    public interface IBinanceSignatureService
    {
        string Sign(string payload);
    }
}