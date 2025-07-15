using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using Doozy.Engine.Utils.ColorModels;
using Newtonsoft.Json;
using UomaWeb.Models;
using Unity;
using UnityEngine;

namespace UomaWeb
{
    public static class UomaDataManager
    {
        private static ItemConfig _itemConfig;
        private static UomaWeb.Models.GameConfig _gameConfig;
        private static UomaData _uomaState = new();
        public static int CompleteLevel { get; set; }
        
        private static readonly Dictionary<string, int> _itemNums = new();

        public static int CurrLevel => CompleteLevel + 1;

        private static void InitializeUomaGameData()
        {
            _uomaState = new UomaData
            {
                Games = new Dictionary<string, GameData>(),
                PlayerData = null,//new PlayerData()
            };
        }

        public static void Init()
        {
            UomaDataManager.LoadGameConfig();
            UomaDataManager.LoadItemConfig();
        }

        public static int GetVirtualCurrency()
        {
            return int.Parse(_uomaState?.PlayerData?.VirtualCurrency ?? "0");
        }
        public static bool CheckGameData()
        {
            return _uomaState?.Games.ContainsKey(UomaController.Instance.GameName) == true;
        }
        
        public static UomaData GetState()
        {
            return _uomaState;
        }

        public static UomaWeb.Models.GameConfig GetGameConfig()
        {
            return _gameConfig;
        }

        public static ItemConfig GetItemConfig()
        {
            return _itemConfig;
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
                _itemConfig = UomaUtils.IsTestPlatform ? UomaItemConfig.GetTestConfig() : UomaItemConfig.GetConfig();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"加载物品配置失败: {ex.Message}");
            }
        }

        public static void SetToken(string token)
        {
            if(_uomaState.Token != null && _uomaState.Token != token)
            {
                Console.WriteLine("Token已更改");
                InitializeUomaGameData();
            }
            _uomaState.Token = token;
        }

        public static void UpdateItemNum(string itemKey, int num)
        {
            if(!_itemNums.TryAdd(itemKey.ToLower(), num))
            {
                _itemNums[itemKey] = num;
            }
        }

        public static int GetItemNum(string itemKey)
        {
            return _itemNums.GetValueOrDefault(itemKey.ToLower());
        }

        public static void UpdateGameData(GameInfo gameInfo)
        {
            if (gameInfo == null) return;
            
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;

            // 如果找到对应的游戏key，使用它来更新数据
            if (gameName != null)
            {
                if (!_uomaState.Games.ContainsKey(gameName))
                {
                    _uomaState.Games[gameName] = new GameData
                    {
                        Items = new Dictionary<string, GameItemData>(),
                        Levels = new List<GameLevelData>()
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
                            _uomaState.Games[gameName].Items[itemKey] = new GameItemData
                            {
                                IsFree = item.IsFree,
                                VirtualCurrencyPrice = int.Parse(item.VirtualCurrencyPrice),
                                GameItemItemNum = int.Parse(item.GameItemItemNum)
                            };
                            PlayerPrefs.SetInt($"{UomaController.Instance.GameName}-{itemKey.ToLower()}", int.Parse(item.GameItemItemNum));
                            PlayerPrefs.Save();
                            
                            UpdateItemNum(itemKey, int.Parse(item.GameItemItemNum));
                        }
                    }
                }

                // 更新关卡数据
                if(gameInfo.GameLevel != null)
                {
                    foreach (var level in gameInfo.GameLevel)
                    {
                        if (level?.GameLevelId == null) continue;

                        // 直接使用GameLevelId和GameLevelName
                        var levelData = new GameLevelData
                        {
                            Id = level.GameLevelId,
                            Name = level.GameLevelName,
                            IsComplete = level.IsComplete,
                            GameLevelStar = level.GameLevelStar
                        };

                        // 查找是否已存在该关卡
                        var existingLevelIndex = _uomaState.Games[gameName].Levels.FindIndex(l => l.Id == level.GameLevelId);
                        if (existingLevelIndex >= 0)
                        {
                            _uomaState.Games[gameName].Levels[existingLevelIndex] = levelData;
                        }
                        else
                        {
                            _uomaState.Games[gameName].Levels.Add(levelData);
                        }
                    }
                }
            }
        }

        public static void UpdateUserData(UserInfo userInfo)
        {
            if (userInfo == null) return;

            _uomaState.PlayerData = new PlayerData
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