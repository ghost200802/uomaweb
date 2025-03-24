using System;
using System.Collections;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public IEnumerator GetPlayerCurrLevel(string gameName, Action<int> callback)
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
                callback?.Invoke(0);
                yield break;
            }

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo(gameId, (response) =>
            {
                if (response?.Data == null)
                {
                    callback?.Invoke(0);
                    return;
                }

                // 查找最高通关关卡
                var gameData = GameDataManager.GetGameState().Games[gameKey];
                if (gameData?.Levels == null)
                {
                    callback?.Invoke(1);
                    return;
                }

                int highestCompletedLevel = -1;
                for (int i = 0; i < gameData.Levels.Count; i++)
                {
                    if (gameData.Levels[i].IsComplete == 1)
                    {
                        highestCompletedLevel = i;
                    }
                }

                callback?.Invoke(highestCompletedLevel + 2); // 返回下一关
            });
        }

        public IEnumerator CompleteLevel(string gameName, int gameLevel, int star, Action<(int successCode, int currLevel, int currencyNum)> callback)
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

            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            // 从配置中查找levelId
            var levelConfig = GameDataManager.GetLevelConfig();
            if (levelConfig?.Levels != null && levelConfig.Levels.ContainsKey(gameKey))
            {   
                levelId = levelConfig.Levels[gameKey][gameLevel].id;
                Console.WriteLine($"Level ID: {levelId}");
            }

            if (levelId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            // 提交通关信息
            yield return _gameWebApi.LevelComplete(gameId, levelId, star.ToString(), (response) =>
            {
                if (response?.Code != 200)
                {
                    callback?.Invoke((response?.Code ?? 1, 0, 0));
                    return;
                }

                StartCoroutine(UpdateLevelInfo(gameId, gameKey, callback));
            });
        }

        private IEnumerator UpdateLevelInfo(string gameId, string gameKey, Action<(int successCode, int currLevel, int currencyNum)> callback)
        {
            bool userInfoUpdated = false;
            bool gameInfoUpdated = false;
            int nextLevel = 1;
            int virtualCurrency = 0;

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
                gameInfoUpdated = true;
            });

            // 等待所有信息更新完成
            while (!userInfoUpdated || !gameInfoUpdated)
            {
                yield return null;
            }

            callback?.Invoke((0, nextLevel, virtualCurrency));
        }
    }
}