using System;
using Newtonsoft.Json;

namespace UomaWeb.Models
{
    public class ConsumeUserGameItemRequest
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("gameItemId")]
        public string GameItemId { get; set; }
    }

    public class ConsumeUserGameItemReply
    {
        [JsonProperty("code")]
        public uint Code { get; set; }

        [JsonProperty("data")]
        public ConsumeUserGameItemReplyData Data { get; set; }
    }

    public class ConsumeUserGameItemReplyData
    {
    }

    public class PurchaseUserGameItemRequest
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("gameItemId")]
        public string GameItemId { get; set; }

        [JsonProperty("gameItemItemNum")]
        public string GameItemItemNum { get; set; }
    }

    public class PurchaseUserGameItemReply
    {
        [JsonProperty("code")]
        public uint Code { get; set; }

        [JsonProperty("data")]
        public PurchaseUserGameItemReplyData Data { get; set; }
    }

    public class PurchaseUserGameItemReplyData
    {
        [JsonProperty("shortUrl")]
        public string ShortUrl { get; set; }
    }

    public class CreateUserGameLevelRequest
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("gameLevelId")]
        public string GameLevelId { get; set; }

        [JsonProperty("gameLevelStar")]
        public string GameLevelStar { get; set; }
    }

    public class CreateUserGameLevelReply
    {
        [JsonProperty("code")]
        public uint Code { get; set; }

        [JsonProperty("data")]
        public CreateUserGameLevelReplyData Data { get; set; }
    }

    public class CreateUserGameLevelReplyData
    {
    }
}