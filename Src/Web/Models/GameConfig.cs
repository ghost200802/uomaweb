using System.Collections.Generic;

namespace UomaWeb.Models
{
    public class GameConfig
    {
        public Dictionary<string, GameAction> Games { get; set; }
    }

    public class GameAction
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}