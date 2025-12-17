using System.Collections.Generic;
using Newtonsoft.Json;

namespace UomaWeb.Models
{
    public class ApiResponse<T>
    {
        [JsonProperty("code")]
        public int Code { get; set; }
        [JsonProperty("data")]
        public T Data { get; set; }
        [JsonProperty("msg")]
        public string Message { get; set; }
    }

    public class GameInfo
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }
        [JsonProperty("gameName")]
        public string GameName { get; set; }
        [JsonProperty("gameImageUrls")]
        public List<string> GameImg { get; set; }
        [JsonProperty("gameDetail")]
        public string GameDetail { get; set; }
        [JsonProperty("status")]
        public int Status { get; set; }
        [JsonProperty("gameLevels")]
        public List<GameLevelInfo> GameLevel { get; set; }
        [JsonProperty("gameItems")]
        public List<GameItemInfo> GameItem { get; set; }
    }

    public class GameLevelInfo
    {
        [JsonProperty("gameLevelId")]
        public string GameLevelId { get; set; }
        [JsonProperty("gameLevelName")]
        public string GameLevelName { get; set; }
        [JsonProperty("isComplete")]
        public int IsComplete { get; set; }
        [JsonProperty("gameLevelStar")]
        public int GameLevelStar { get; set; }
    }

    public class GameItemInfo
    {
        [JsonProperty("gameItemId")]
        public string GameItemId { get; set; }
        [JsonProperty("gameItemName")]
        public string GameItemName { get; set; }
        [JsonProperty("gameItemImageUrls")]
        public List<string> GameItemImg { get; set; }
        [JsonProperty("gameItemDetail")]
        public string GameItemDesc { get; set; }
        [JsonProperty("isFree")]
        public int IsFree { get; set; }
        [JsonProperty("virtualCurrencyPrice")]
        public string VirtualCurrencyPrice { get; set; }
        [JsonProperty("gameItemQuantity")]
        public string GameItemItemNum { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}