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
                            id = "1998684889318727680",
                            name = "消消乐",
                            description = "一款经典的三消类游戏"
                        }
                    },
                    { "ScrewRemoval", new GameAction
                        {
                            id = "1998680062454964224",
                            name = "钉钉子",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "WaterSort", new GameAction
                        {
                            id = "1998683248985153536",
                            name = "倒水",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "KnifeHit", new GameAction
                        {
                            id = "1998684030824390656",
                            name = "飞刀",
                            description = "考验技巧的益智游戏"
                        }
                    },
                    { "TilesMatching", new GameAction
                        {
                            id = "1998685531936432128",
                            name = "方块消除",
                            description = "方块消除的游戏"
                        }
                    }
                }
            };
        }
    }
}