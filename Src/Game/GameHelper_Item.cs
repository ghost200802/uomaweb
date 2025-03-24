using System;
using System.Collections;
using System.Collections.Generic;
using UomaWeb.Models;
using Unity;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public IEnumerator UseGameItem(string gameName, string itemName, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            // 从配置中查找gameId和itemId
            var gameConfig = GameDataManager.GetGameState();
            string gameId = null;
            string itemId = null;
            string gameKey = null;
            string itemKey = null;

            foreach (var game in GameDataManager.GetGameConfig().Games)
            {
                if (game.Key == gameName)
                {
                    gameId = game.Value.id;
                    gameKey = game.Key;
                    break;
                }
            }

            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            foreach (var item in GameDataManager.GetItemConfig().Items[gameKey])
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

            // 使用道具
            yield return _gameWebApi.ConsumeUserGameItem(gameId, itemId, (response) =>
            {
                if (response?.Code != 200)
                {
                    callback?.Invoke((response?.Code ?? 1, 0, 0));
                    return;
                }

                this.StartCoroutine(UpdateItemInfo(gameId, gameKey, itemKey, callback));
            });
        }

        public IEnumerator BuyGameItem(string gameName, string itemName, int num, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            // 从配置中查找gameId和itemId
            var gameConfig = GameDataManager.GetGameState();
            string gameId = null;
            string itemId = null;
            string gameKey = null;
            string itemKey = null;

            foreach (var game in GameDataManager.GetGameConfig().Games)
            {
                if (game.Key == gameName)
                {
                    gameId = game.Value.id;
                    gameKey = game.Key;
                    break;
                }
            }

            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            foreach (var item in GameDataManager.GetItemConfig().Items[gameKey])
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

                this.StartCoroutine(UpdateItemInfo(gameId, gameKey, itemKey, callback));
            });
        }

        private IEnumerator UpdateItemInfo(string gameId, string gameKey, string itemKey, Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            bool userInfoUpdated = false;
            bool gameInfoUpdated = false;
            int virtualCurrency = 0;
            int itemNum = 0;

            // 更新用户信息
            yield return _gameWebApi.GetUserInfo((userResponse) =>
            {
                if (userResponse?.Data != null)
                {
                    GameDataManager.UpdateUserData(userResponse.Data);
                    int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out virtualCurrency);
                }
                userInfoUpdated = true;
            });

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo(gameId, (gameResponse) =>
            {
                if (gameResponse?.Data != null)
                {
                    GameDataManager.UpdateGameData(gameId, gameResponse.Data);
                    var gameState = GameDataManager.GetGameState();
                    if (gameState.Games.ContainsKey(gameKey) && 
                        gameState.Games[gameKey].Items.ContainsKey(itemKey))
                    {
                        int.TryParse(gameState.Games[gameKey].Items[itemKey].GameItemItemNum, out itemNum);
                    }
                }
                gameInfoUpdated = true;
            });

            // 等待所有信息更新完成
            while (!userInfoUpdated || !gameInfoUpdated)
            {
                yield return null;
            }

            callback?.Invoke((0, virtualCurrency, itemNum));
        }

        public IEnumerator GetGameItemNum(string gameName, Action<Dictionary<string, int>> callback)
        {
            // 从配置中查找gameId
            var gameConfig = GameDataManager.GetGameState();
            string gameId = null;
            string gameKey = null;

            foreach (var game in GameDataManager.GetGameConfig().Games)
            {
                if (game.Key == gameName)
                {
                    gameId = game.Value.id;
                    gameKey = game.Key;
                    break;
                }
            }

            if (gameId == null)
            {
                callback?.Invoke(new Dictionary<string, int>());
                yield break;
            }

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo(gameId, (response) =>
            {
                if (response?.Data == null)
                {
                    callback?.Invoke(new Dictionary<string, int>());
                    return;
                }

                var result = new Dictionary<string, int>();
                var gameState = GameDataManager.GetGameState();
                if (gameState.Games.ContainsKey(gameKey))
                {
                    foreach (var item in gameState.Games[gameKey].Items)
                    {
                        int.TryParse(item.Value.GameItemItemNum, out int num);
                        result[item.Key] = num;
                    }
                }

                callback?.Invoke(result);
            });
        }
    }
}