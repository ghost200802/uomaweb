using System.Collections.Generic;
using Newtonsoft.Json;

namespace UomaWeb.Models
{
    public class GameItemOrderItem
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("gameItemId")]
        public string GameItemId { get; set; }

        [JsonProperty("gameItemQuantity")]
        public string GameItemQuantity { get; set; }
    }

    public class OrderItem
    {
        [JsonProperty("gameItemOrderItem")]
        public GameItemOrderItem GameItemOrderItem { get; set; }
    }

    public class GenerateOrderPaymentAmountRequest
    {
        [JsonProperty("orderType")]
        public int OrderType { get; set; }

        [JsonProperty("orderAmountCurrency")]
        public string OrderAmountCurrency { get; set; }

        [JsonProperty("orderItem")]
        public OrderItem OrderItem { get; set; }
    }

    public class GenerateOrderPaymentAmountReply
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public GenerateOrderPaymentAmountReplyData Data { get; set; }
    }

    public class GenerateOrderPaymentAmountReplyData
    {
        [JsonProperty("paymentAmount")]
        public string PaymentAmount { get; set; }
    }

    public class CreateOrderRequest
    {
        [JsonProperty("orderType")]
        public int OrderType { get; set; }

        [JsonProperty("paymentAmount")]
        public string PaymentAmount { get; set; }

        [JsonProperty("orderAmountCurrency")]
        public string OrderAmountCurrency { get; set; }

        [JsonProperty("paymentChannel")]
        public int PaymentChannel { get; set; }

        [JsonProperty("clientPlatform")]
        public string ClientPlatform { get; set; }

        [JsonProperty("orderItem")]
        public OrderItem OrderItem { get; set; }
    }

    public class CreateOrderReply
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("data")]
        public CreateOrderReplyData Data { get; set; }
    }

    public class CreateOrderReplyData
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }
    }

    public class PayOrderRequest
    {
        [JsonProperty("orderId")]
        public string OrderId { get; set; }

        [JsonProperty("orderType")]
        public int OrderType { get; set; }

        [JsonProperty("clientPlatform")]
        public string ClientPlatform { get; set; }
    }

    public class PayOrderReply
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        
        [JsonProperty("data")]
        public PayOrderReplyData Data { get; set; }
    }

    public class PayOrderReplyData
    {
        [JsonProperty("status")]
        public int Status { get; set; }
        
        [JsonProperty("paymentStatus")]
        public int PaymentStatus { get; set; }
    }
}
