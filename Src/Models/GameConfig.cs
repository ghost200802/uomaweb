using System.Collections.Generic;

namespace UomaWeb.Models
{
    public class GameConfig
    {
        public Dictionary<string, Game> Games { get; set; }
    }

    public class Game
    {
        public string id { get; set; }
        public string name { get; set; }
        public string description { get; set; }
    }
}