using System;
using System.Collections.Generic;
using System.IO;
using Newtonsoft.Json;
using UomaWeb.Models;
using Unity;
using UnityEngine;

namespace UomaWeb
{
    public static class GameDataManager
    {
        private static ItemConfig _itemConfig;
        private static LevelConfig _levelConfig;
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

        public static void Init()
        {
            GameDataManager.InitializeUomaGameData();
            GameDataManager.LoadGameConfig();
            GameDataManager.LoadItemConfig();
            GameDataManager.LoadLevelConfig();
        }

        public static int GetVirtualCurrency()
        {
            return int.Parse(_uomaGame?.PlayerData?.VirtualCurrency ?? "0");
        }

        public static UomaGameData GetGameState()
        {
            return _uomaGame;
        }

        public static GameConfig GetGameConfig()
        {
            return _gameConfig;
        }

        public static ItemConfig GetItemConfig()
        {
            return _itemConfig;
        }

        public static LevelConfig GetLevelConfig()
        {
            return _levelConfig;
        }

        public static void LoadGameConfig()
        {
            try
            {
                _gameConfig = UomaGameConfig.GetConfig();
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
                _itemConfig = UomaItemConfig.GetConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载物品配置失败: {ex.Message}");
            }
        }

        public static void LoadLevelConfig()
        {
            try
            {
                _levelConfig = UomaLevelConfig.GetConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载关卡配置失败: {ex.Message}");
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

        public static void UpdateGameData(GameInfo gameInfo)
        {
            if (gameInfo == null) return;
            
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;

            // 如果找到对应的游戏key，使用它来更新数据
            if (gameName != null)
            {
                if (!_uomaGame.Games.ContainsKey(gameName))
                {
                    _uomaGame.Games[gameName] = new GameData
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
                        if (_itemConfig?.Items != null && _itemConfig.Items.ContainsKey(gameName))
                        {
                            foreach (var configItem in _itemConfig.Items[gameName])
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
                            _uomaGame.Games[gameName].Items[itemKey] = new GameItemData
                            {
                                IsFree = item.IsFree,
                                VirtualCurrencyPrice = item.VirtualCurrencyPrice,
                                GameItemItemNum = item.GameItemItemNum
                            };
                        }
                    }
                }

                if(gameInfo.GameLevel!= null)
                {
                    foreach (var level in gameInfo.GameLevel)
                    {
                        if (level?.GameLevelId == null) continue;

                        // 从配置中查找LevelID对应的num
                        int levelNum = -1;
                        string levelId = null;
                        if (_levelConfig?.Levels != null && _levelConfig.Levels.ContainsKey(gameName))
                        {   
                            for (int i = 0; i < _levelConfig.Levels[gameName].Count; i++)
                            {
                                if (_levelConfig.Levels[gameName][i].id == level.GameLevelId)
                                {
                                    levelNum = i;
                                    levelId = _levelConfig.Levels[gameName][i].id;
                                    break;
                                }
                            }
                        }

                        // 确保Levels列表已初始化
                        if (_uomaGame.Games[gameName].Levels == null)
                        {
                            _uomaGame.Games[gameName].Levels = new List<GameLevelData>();
                        }

                        // 如果找到对应的关卡，更新数据；否则添加新关卡
                        if (levelId != null)
                        {
                            if (levelNum < _uomaGame.Games[gameName].Levels.Count)
                            {
                                _uomaGame.Games[gameName].Levels[levelNum] = new GameLevelData
                                {
                                    Id = levelId,
                                    Name = _levelConfig.Levels[gameName][levelNum].name,
                                    IsComplete = level.IsComplete
                                };
                            }
                            else
                            {
                                _uomaGame.Games[gameName].Levels.Add(new GameLevelData
                                {
                                    Id = levelId,
                                    Name = _levelConfig.Levels[gameName][levelNum].name,
                                    IsComplete = level.IsComplete
                                });
                            }
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