using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaItemConfig
    {
        public static ItemConfig GetConfig()
        {
            return new ItemConfig
            {
                ItemNames = new Dictionary<string, List<string>>
                {
                    {
                        "CandySweet", new List<string> { "ExtraMoves", "Stripes", "Bomb", "MulticolorCandy", "FreeMove", "ExplodeArea", "Marmalade" }
                    },
                    {
                        "ScrewRemoval", new List<string> { "unlock", "skip", "guide" }
                    },
                    {
                        "WaterSort", new List<string> { "skip", "undo" }
                    },
                    {
                        "KnifeHit", new List<string> { "skip", "undo" }
                    },
                    {
                        "TilesMatching", new List<string> { "suggest", "shuffle", "undo", "continue" }
                    }
                }
            };
        }
    }
}