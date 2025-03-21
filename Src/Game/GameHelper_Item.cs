using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public async Task<(int successCode, int currencyNum, int itemNum)> UseGameItemAsync(string gameName, string itemName)
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

            if (gameId == null) return (1, 0, 0);

            foreach (var item in GameDataManager.GetItemConfig().Items[gameKey])
            {
                if (item.Key.ToLower() == itemName.ToLower())
                {
                    itemId = item.Value.id;
                    itemKey = item.Key.ToLower();
                    break;
                }
            }

            if (itemId == null) return (1, 0, 0);

            // 使用道具
            var response = await _gameWebApi.ConsumeUserGameItemAsync(gameId, itemId);
            if (response?.Code != 200) return (response?.Code ?? 1, 0, 0);

            // 更新用户信息和游戏信息
            var userResponse = await _gameWebApi.GetUserInfoAsync();
            var gameResponse = await _gameWebApi.GetGameInfoAsync(gameId);

            if (userResponse?.Data != null)
            {
                GameDataManager.UpdateUserData(userResponse.Data);
            }

            int itemNum = 0;
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

            return (0, int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out int vc) ? vc : 0, itemNum);
        }

        public async Task<(int successCode, int currencyNum, int itemNum)> BuyGameItemAsync(string gameName, string itemName, int num)
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

            if (gameId == null) return (1, 0, 0);

            foreach (var item in GameDataManager.GetItemConfig().Items[gameKey])
            {
                if (item.Key.ToLower() == itemName.ToLower())
                {
                    itemId = item.Value.id;
                    itemKey = item.Key.ToLower();
                    break;
                }
            }

            if (itemId == null) return (1, 0, 0);

            // 购买道具
            var response = await _gameWebApi.PurchaseUserGameItemAsync(gameId, itemId, num.ToString());
            if (response?.Code != 200) return (response?.Code ?? 1, 0, 0);

            // 更新用户信息和游戏信息
            var userResponse = await _gameWebApi.GetUserInfoAsync();
            var gameResponse = await _gameWebApi.GetGameInfoAsync(gameId);

            if (userResponse?.Data != null)
            {
                GameDataManager.UpdateUserData(userResponse.Data);
            }

            int itemNum = 0;
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

            return (0, int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out int vc) ? vc : 0, itemNum);
        }

        public async Task<Dictionary<string, int>> GetGameItemNumAsync(string gameName)
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

            if (gameId == null) return new Dictionary<string, int>();

            // 更新游戏信息
            var response = await _gameWebApi.GetGameInfoAsync(gameId);
            if (response?.Data == null) return new Dictionary<string, int>();

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

            return result;
        }
    }
}