using System.Collections.Generic;

namespace UomaWeb.Models
{
    public class ItemConfig
    {
        public Dictionary<string, Dictionary<string, ItemAction>> Items { get; set; }
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
        public string id { get; set; }
        public string name { get; set; }
    }
}