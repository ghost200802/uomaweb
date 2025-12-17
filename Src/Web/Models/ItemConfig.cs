using System.Collections.Generic;
using Newtonsoft.Json;

namespace UomaWeb.Models
{
    public class ItemConfig
    {
        public Dictionary<string, List<string>> ItemNames { get; set; }

        public Dictionary<string, Dictionary<string,ItemAction>> Items { get; set; }        
    }

    public class ItemAction
    {
        public string id { get; set; }
        public string name { get; set; }
    }

    public class LevelConfig
    {
        public Dictionary<string, List<LevelAction>> Levels { get; set; }
    }

    public class LevelAction
    {
        [JsonProperty("gameLevelId")]
        public string id { get; set; }
        [JsonProperty("gameLevelName")]
        public string name { get; set; }
        [JsonProperty("isComplete")]
        public int isComplete { get; set; }
        [JsonProperty("gameLevelStar")]
        public string gameLevelStar { get; set; }
    }
}