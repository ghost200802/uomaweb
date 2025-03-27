using Newtonsoft.Json;

namespace UomaWeb.Models
{
    public class LevelCompleteRequest
    {
        [JsonProperty("gameId")]
        public string GameId { get; set; }

        [JsonProperty("gameLevelId")]
        public string GameLevelId { get; set; }

        [JsonProperty("gameLevelStar")]
        public string GameLevelStar { get; set; }
    }

    public class LevelCompleteReply
    {
        public int Code = 0;
        public string Message = "";
    }
}