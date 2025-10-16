using System;
using System.Reactive.Subjects;
using Binance.Client.Websocket.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Binance.Client.Websocket.Responses.Orders
{
    /// <summary>
    /// Futures user data stream order update.
    /// </summary>
    public class FuturesOrderUpdate
    {
        internal static bool TryHandle(JObject? response, ISubject<FuturesOrderUpdate> subject)
        {
            var stream = response?["e"]?.Value<string>();
            if (stream != "ORDER_TRADE_UPDATE")
                return false;

            var parsed = response!.ToObject<FuturesOrderUpdate>(BinanceJsonSerializer.Serializer);
            if (parsed != null)
            {
                var listenKey = response["listenKey"]?.Value<string>();
                if (!string.IsNullOrWhiteSpace(listenKey))
                {
                    parsed.ListenKey = listenKey!;
                    if (parsed.Order != null && string.IsNullOrWhiteSpace(parsed.Order.ListenKey))
                    {
                        parsed.Order.ListenKey = listenKey!;
                    }
                }

                subject.OnNext(parsed);
            }

            return true;
        }
        
        /// <summary>
        /// The type of the event.
        /// </summary>
        [JsonProperty("e")]
        public string Event { get; set; } = string.Empty;
        
        /// <summary>
        /// The time the event happened.
        /// </summary>
        [JsonProperty("E"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? EventTime { get; set; }
        
        /// <summary>
        /// The transaction time.
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? TransactionTime { get; set; }
        
        /// <summary>
        /// Order payload.
        /// </summary>
        [JsonProperty("o")]
        public FuturesOrderData Order { get; set; } = new FuturesOrderData();
        
        /// <summary>
        /// Optional listen key that generated this event.
        /// </summary>
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }

    /// <summary>
    /// Futures order data payload.
    /// </summary>
    public class FuturesOrderData
    {
        /// <summary>
        /// Symbol.
        /// </summary>
        [JsonProperty("s")]
        public string Symbol { get; set; } = string.Empty;
        
        /// <summary>
        /// Client order id.
        /// </summary>
        [JsonProperty("c")]
        public string ClientOrderId { get; set; } = string.Empty;
        
        /// <summary>
        /// Side.
        /// </summary>
        [JsonProperty("S")]
        public OrderSide Side { get; set; }
        
        /// <summary>
        /// Order type.
        /// </summary>
        [JsonProperty("o")]
        public OrderType Type { get; set; }
        
        /// <summary>
        /// Time in force.
        /// </summary>
        [JsonProperty("f")]
        public TimeInForce TimeInForce { get; set; }
        
        /// <summary>
        /// Original quantity.
        /// </summary>
        [JsonProperty("q")]
        public double Quantity { get; set; }
        
        /// <summary>
        /// Original price.
        /// </summary>
        [JsonProperty("p")]
        public double Price { get; set; }
        
        /// <summary>
        /// Average price.
        /// </summary>
        [JsonProperty("ap")]
        public double AveragePrice { get; set; }
        
        /// <summary>
        /// Stop price.
        /// </summary>
        [JsonProperty("sp")]
        public double StopPrice { get; set; }
        
        /// <summary>
        /// Execution type.
        /// </summary>
        [JsonProperty("x")]
        public ExecutionType ExecutionType { get; set; }
        
        /// <summary>
        /// Order status.
        /// </summary>
        [JsonProperty("X")]
        public OrderStatus Status { get; set; }
        
        /// <summary>
        /// Order id.
        /// </summary>
        [JsonProperty("i")]
        public long OrderId { get; set; }
        
        /// <summary>
        /// Last filled quantity.
        /// </summary>
        [JsonProperty("l")]
        public double LastFilledQuantity { get; set; }
        
        /// <summary>
        /// Accumulated filled quantity.
        /// </summary>
        [JsonProperty("z")]
        public double AccumulatedFilledQuantity { get; set; }
        
        /// <summary>
        /// Last filled price.
        /// </summary>
        [JsonProperty("L")]
        public double LastFilledPrice { get; set; }
        
        /// <summary>
        /// Commission asset.
        /// </summary>
        [JsonProperty("N")]
        public string CommissionAsset { get; set; } = string.Empty;
        
        /// <summary>
        /// Commission.
        /// </summary>
        [JsonProperty("n")]
        public double Commission { get; set; }
        
        /// <summary>
        /// Trade time.
        /// </summary>
        [JsonProperty("T"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? TradeTime { get; set; }
        
        /// <summary>
        /// Trade id.
        /// </summary>
        [JsonProperty("t")]
        public long TradeId { get; set; }
        
        /// <summary>
        /// Bids notional.
        /// </summary>
        [JsonProperty("b")]
        public double BidsNotional { get; set; }
        
        /// <summary>
        /// Asks notional.
        /// </summary>
        [JsonProperty("a")]
        public double AsksNotional { get; set; }
        
        /// <summary>
        /// Indicates maker side.
        /// </summary>
        [JsonProperty("m")]
        public bool IsMakerSide { get; set; }
        
        /// <summary>
        /// Reduce only flag.
        /// </summary>
        [JsonProperty("R")]
        public bool ReduceOnly { get; set; }
        
        /// <summary>
        /// Working type.
        /// </summary>
        [JsonProperty("wt")]
        public WorkingType WorkingType { get; set; }
        
        /// <summary>
        /// Original order type.
        /// </summary>
        [JsonProperty("ot")]
        public OrderType OriginalOrderType { get; set; }
        
        /// <summary>
        /// Position side.
        /// </summary>
        [JsonProperty("ps")]
        public PositionSide PositionSide { get; set; }
        
        /// <summary>
        /// Close all flag.
        /// </summary>
        [JsonProperty("cp")]
        public bool CloseAll { get; set; }
        
        /// <summary>
        /// Activation price.
        /// </summary>
        [JsonProperty("AP")]
        public double ActivationPrice { get; set; }
        
        /// <summary>
        /// Callback rate.
        /// </summary>
        [JsonProperty("cr")]
        public double CallbackRate { get; set; }
        
        /// <summary>
        /// Price protection flag.
        /// </summary>
        [JsonProperty("pP")]
        public bool PriceProtect { get; set; }
        
        /// <summary>
        /// Ignore field si.
        /// </summary>
        [JsonProperty("si")]
        public long PreventedQuantity { get; set; }
        
        /// <summary>
        /// Ignore field ss.
        /// </summary>
        [JsonProperty("ss")]
        public long PreventedSecondaryQuantity { get; set; }
        
        /// <summary>
        /// Realized profit.
        /// </summary>
        [JsonProperty("rp")]
        public double RealizedProfit { get; set; }
        
        /// <summary>
        /// Self trade prevention mode.
        /// </summary>
        [JsonProperty("V")]
        public SelfTradePreventionMode SelfTradePreventionMode { get; set; }
        
        /// <summary>
        /// Price match mode.
        /// </summary>
        [JsonProperty("pm")]
        public string PriceMatchMode { get; set; } = string.Empty;
        
        /// <summary>
        /// Good till date time.
        /// </summary>
        [JsonProperty("gtd"), JsonConverter(typeof(UnixDateTimeConverter))]
        public DateTime? GoodTillDate { get; set; }
        
        /// <summary>
        /// Listen key.
        /// </summary>
        [JsonProperty("listenKey")]
        public string ListenKey { get; set; } = string.Empty;
    }
}
