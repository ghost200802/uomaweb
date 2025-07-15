using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaGameConfig
    {
        public static UomaWeb.Models.GameConfig GetConfig()
        {
            return new UomaWeb.Models.GameConfig
            {
                Games = new Dictionary<string, GameAction>
                {
                    { "CandySweet", new GameAction
                        {
                            id = "1904083505756282880",
                            name = "消消乐",
                            description = "一款经典的三消类游戏"
                        }
                    },
                    { "ScrewRemoval", new GameAction
                        {
                            id = "1904079869357629440",
                            name = "钉钉子",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "WaterSort", new GameAction
                        {
                            id = "1904084067851739136",
                            name = "倒水",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "KnifeHit", new GameAction
                        {
                            id = "1904086016353411072",
                            name = "飞刀",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "TilesMatching", new GameAction
                        {
                            id = "1944714241605410816",
                            name = "方块消除",
                            description = "方块消除的游戏"
                        }
                    }
                }
            };
        }
    }
}