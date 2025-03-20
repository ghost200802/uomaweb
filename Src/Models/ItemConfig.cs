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
}