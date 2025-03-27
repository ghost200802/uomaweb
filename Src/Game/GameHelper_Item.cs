using System;
using System.Collections;
using System.Collections.Generic;
using UomaWeb.Models;
using Unity;
using UnityEngine;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public static int GetItemPrice(string itemName)
        {
            try
            {
                var gameName = UomaController.Instance.GameName;
                var gameData = UomaDataManager.GetState()?.Games[gameName];
                var gameDataItems = gameData?.Items;
                return gameDataItems?[itemName.ToLower()]?.VirtualCurrencyPrice ?? -1;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return -1;
        }
        
        public IEnumerator UseGameItem(string itemName, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            // 从配置中查找gameId和itemId
            var gameConfig = UomaDataManager.GetState();

            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;
            
            string itemId = null;
            string itemKey = null;



            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            foreach (var item in UomaDataManager.GetItemConfig().Items[gameName])
            {
                if (item.Key.ToLower() == itemName.ToLower())
                {
                    itemId = item.Value.id;
                    itemKey = item.Key.ToLower();
                    break;
                }
            }

            if (itemId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            Debug.Log($"Before Use Item {gameName}-{itemName}-{itemId}");
            
            // 使用道具
            yield return _gameWebApi.ConsumeUserGameItem(gameId, itemId, (response) =>
            {
                if (response?.Code != 200)
                {
                    callback?.Invoke((response?.Code ?? 1, 0, 0));
                    return;
                }
                
                this.StartCoroutine(UpdateItemInfo(itemKey, callback));
            });
        }

        public IEnumerator BuyGameItem(string itemName, int num, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            // 从配置中查找gameId和itemId
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;
            
            string itemId = null;
            string itemKey = null;

            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            foreach (var item in UomaDataManager.GetItemConfig().Items[gameName])
            {
                if (item.Key.ToLower() == itemName.ToLower())
                {
                    itemId = item.Value.id;
                    itemKey = item.Key.ToLower();
                    break;
                }
            }

            if (itemId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            // 购买道具
            yield return _gameWebApi.PurchaseUserGameItem(gameId, itemId, num.ToString(), (response) =>
            {
                if (response?.Code != 200)
                {
                    callback?.Invoke((response?.Code ?? 1, 0, 0));
                    return;
                }

                this.StartCoroutine(UpdateItemInfo(itemKey, callback));
            });
        }

        private IEnumerator UpdateItemInfo(string itemKey, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;
            
            bool userInfoUpdated = false;
            bool gameInfoUpdated = false;
            int virtualCurrency = 0;
            int itemNum = 0;

            // 更新用户信息
            yield return _gameWebApi.GetUserInfo((userResponse) =>
            {
                if (userResponse?.Data != null)
                {
                    UomaDataManager.UpdateUserData(userResponse.Data);
                    int.TryParse(UomaDataManager.GetState().PlayerData?.VirtualCurrency, out virtualCurrency);
                }
                userInfoUpdated = true;
            });

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo((gameResponse) =>
            {
                if (gameResponse?.Data != null)
                {
                    UomaDataManager.UpdateGameData(gameResponse.Data);
                    var gameState = UomaDataManager.GetState();
                    if (gameState.Games.ContainsKey(gameName) && 
                        gameState.Games[gameName].Items.ContainsKey(itemKey))
                    {
                        itemNum = gameState.Games[gameName].Items[itemKey].GameItemItemNum;
                    }
                }
                gameInfoUpdated = true;
                
            });

            // 等待所有信息更新完成
            while (!userInfoUpdated || !gameInfoUpdated)
            {
                yield return null;
            }

            Debug.Log($"Item {gameName}-{itemKey}-{itemNum}");
            callback?.Invoke((0, virtualCurrency, itemNum));
        }

        public IEnumerator GetGameItemNum(Action<Dictionary<string, int>> callback)
        {
            // 从配置中查找gameId
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;

            if (gameId == null)
            {
                callback?.Invoke(new Dictionary<string, int>());
                yield break;
            }

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo((response) =>
            {
                if (response?.Data == null)
                {
                    callback?.Invoke(new Dictionary<string, int>());
                    return;
                }

                var result = new Dictionary<string, int>();
                var gameState = UomaDataManager.GetState();
                if (gameState.Games.ContainsKey(gameName))
                {
                    foreach (var item in gameState.Games[gameName].Items)
                    {
                        result[item.Key] = item.Value.GameItemItemNum;
                    }
                }

                callback?.Invoke(result);
            });
        }
    }
}