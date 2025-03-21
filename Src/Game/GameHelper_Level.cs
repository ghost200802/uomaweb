using System;
using System.Threading.Tasks;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public async Task<int> GetPlayerCurrLevelAsync(string gameName)
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

            if (gameId == null) return 0;

            // 更新游戏信息
            var response = await _gameWebApi.GetGameInfoAsync(gameId);
            if (response?.Data == null) return 0;

            // 查找最高通关关卡
            var gameData = GameDataManager.GetGameState().Games[gameKey];
            if (gameData?.Levels == null) return 1;

            int highestCompletedLevel = -1;
            for (int i = 0; i < gameData.Levels.Count; i++)
            {
                if (gameData.Levels[i].IsComplete == 1)
                {
                    highestCompletedLevel = i;
                }
            }

            return highestCompletedLevel + 2; // 返回下一关
        }

        public async Task<(int successCode, int currLevel, int currencyNum)> CompleteLevelAsync(string gameName, int gameLevel, int star)
        {
            // 从配置中查找gameId和levelId
            var gameConfig = GameDataManager.GetGameState();
            string gameId = null;
            string gameKey = null;
            string levelId = null;

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

            // 从配置中查找levelId
            var levelConfig = GameDataManager.GetLevelConfig();
            if (levelConfig?.Levels != null && levelConfig.Levels.ContainsKey(gameKey))
            {   
                levelId = levelConfig.Levels[gameKey][gameLevel].id;
                Console.WriteLine($"Level ID: {levelId}");
            }

            if (levelId == null) return (1, 0, 0);

            // 提交通关信息
            var response = await _gameWebApi.LevelCompleteAsync(gameId, levelId, star.ToString());
            if (response?.Code != 200) return (response?.Code ?? 1, 0, 0);

            // 更新用户信息和游戏信息
            var userResponse = await _gameWebApi.GetUserInfoAsync();
            var gameResponse = await _gameWebApi.GetGameInfoAsync(gameId);

            if (userResponse?.Data != null)
            {
                GameDataManager.UpdateUserData(userResponse.Data);
            }

            int nextLevel = 1;
            if (gameResponse?.Data != null)
            {
                GameDataManager.UpdateGameData(gameId, gameResponse.Data);
                var gameState = GameDataManager.GetGameState();
                if (gameState.Games.ContainsKey(gameKey) && gameState.Games[gameKey].Levels != null)
                {
                    int highestCompletedLevel = -1;
                    for (int i = 0; i < gameState.Games[gameKey].Levels.Count; i++)
                    {
                        if (gameState.Games[gameKey].Levels[i].IsComplete == 1)
                        {
                            highestCompletedLevel = i;
                        }
                    }
                    nextLevel = highestCompletedLevel + 2;
                }
            }

            return (0, nextLevel, int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out int vc) ? vc : 0);
        }
    }
}