using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UomaWeb.Models;

namespace UomaWeb
{
    public class GameDataManager
    {
        private static ItemConfig _itemConfig;
        private static GameConfig _gameConfig;
        private static UomaGameData _uomaGame;

        private static void InitializeUomaGameData()
        {
            _uomaGame = new UomaGameData
            {
                Games = new Dictionary<string, GameData>(),
                PlayerData = null,//new PlayerData()
            };
        }

        static GameDataManager()
        {
            // 先初始化数据结构，再加载配置
            InitializeUomaGameData();
            GameDataManager.LoadGameConfig();
            GameDataManager.LoadItemConfig();
        }

        public static UomaGameData GetGameState()
        {
            return _uomaGame;
        }

        public static void LoadGameConfig()
        {
            try
            {
                var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json");
                var json = File.ReadAllText(configPath);
                _gameConfig = JsonConvert.DeserializeObject<GameConfig>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载游戏配置失败: {ex.Message}");
            }
        }

        public static void LoadItemConfig()
        {
            try
            {
                var configPath = Path.Combine(Directory.GetCurrentDirectory(), "config", "items.json");
                var json = File.ReadAllText(configPath);
                _itemConfig = JsonConvert.DeserializeObject<ItemConfig>(json);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载物品配置失败: {ex.Message}");
            }
        }

        public static void SetToken(string token)
        {
            if(_uomaGame.Token != null && _uomaGame.Token != token)
            {
                Console.WriteLine("Token已更改");
                InitializeUomaGameData();
            }
            _uomaGame.Token = token;
        }

        public static void UpdateGameData(string gameId, GameInfo gameInfo)
        {
            if (gameInfo == null) return;

            // 从配置中查找gameId对应的游戏key
            string gameKey = null;
            foreach (var game in _gameConfig.Games)
            {
                if (game.Value.id == gameId)
                {
                    gameKey = game.Key;
                    break;
                }
            }

            // 如果找到对应的游戏key，使用它来更新数据
            if (gameKey != null)
            {
                if (!_uomaGame.Games.ContainsKey(gameKey))
                {
                    _uomaGame.Games[gameKey] = new GameData
                    {
                        Items = new Dictionary<string, GameItemData>()
                    };
                }

                // 更新道具数据
                if (gameInfo.GameItem != null)
                {
                    foreach (var item in gameInfo.GameItem)
                    {
                        if (item?.GameItemId == null) continue;

                        // 从配置中查找道具ID对应的key
                        string itemKey = null;
                        if (_itemConfig?.Items != null && _itemConfig.Items.ContainsKey(gameKey))
                        {
                            foreach (var configItem in _itemConfig.Items[gameKey])
                            {
                                if (configItem.Value.id == item.GameItemId)
                                {
                                    itemKey = configItem.Key.ToLower();
                                    break;
                                }
                            }
                        }

                        // 如果找到对应的道具key，使用它来更新数据
                        if (itemKey != null)
                        {
                            _uomaGame.Games[gameKey].Items[itemKey] = new GameItemData
                            {
                                IsFree = item.IsFree,
                                VirtualCurrencyPrice = item.VirtualCurrencyPrice,
                                Stock = item.Stock
                            };
                        }
                    }
                }
            }
        }

        public static void UpdateUserData(UserInfo userInfo)
        {
            if (userInfo == null) return;

            _uomaGame.PlayerData = new PlayerData
            {
                InviteCode = userInfo.InviteCode,
                LoginCountryId = userInfo.LoginCountryId,
                AvailableBalanceCny = userInfo.AvailableBalanceCny,
                AvailableBalanceUsd = userInfo.AvailableBalanceUsd,
                VirtualCurrency = userInfo.VirtualCurrency
            };
        }
        
    }
}