using System.Reactive.Subjects;

using System;

namespace Binance.Client.Websocket.Responses
{
    /// <summary>
    /// Pong response
    /// </summary>
    public class PongResponse : MessageBase
    {
        /// <summary>
        /// Received pong message
        /// </summary>
        public string Message { get; set; } = string.Empty;

        internal static bool TryHandle(string? response, ISubject<PongResponse> subject)
        {
            if (response == null)
                return false;

            if (response.IndexOf("pong", StringComparison.OrdinalIgnoreCase) < 0)
                return false;

            var parsed = new PongResponse { Message = response };
            subject.OnNext(parsed);
            return true;
        }
    }
}
