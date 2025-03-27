using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaGameConfig
    {
        public static GameConfig GetConfig()
        {
            return new GameConfig
            {
                Games = new Dictionary<string, Game>
                {
                    { "CandySweet", new Game
                        {
                            id = "1904083505756282880",
                            name = "消消乐",
                            description = "一款经典的三消类游戏"
                        }
                    },
                    { "ScrewRemoval", new Game
                        {
                            id = "1904079869357629440",
                            name = "钉钉子",
                            description = "考验技巧的益智游戏"
                        }
                    }
                }
            };
        }
    }
}