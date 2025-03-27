using System;
using System.Collections;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public static GameLevelData GetLevelData(int level)
        {
            var gameName = UomaController.Instance.GameName;
            var gameData = UomaDataManager.GetState().Games[gameName];
            return gameData.Levels[level - 1];
        }
        
        public IEnumerator GetPlayerCompleteLevel(Action<int> callback)
        {
            // 从配置中查找gameId
            var gameConfig = UomaDataManager.GetState();

            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;

            if (gameId == null)
            {
                callback?.Invoke(0);
                yield break;
            }

            // 更新游戏信息
            yield return _gameWebApi.GetGameInfo((response) =>
            {
                if (response?.Data == null)
                {
                    callback?.Invoke(0);
                    return;
                }

                // 查找最高通关关卡
                var gameData = UomaDataManager.GetState().Games[gameName];
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

                callback?.Invoke(highestCompletedLevel + 1); // 返回当前关（0为第1关）
            });
        }

        public IEnumerator CompleteLevel(int gameLevel, int star, Action<(int successCode, int currLevel, int currencyNum)> callback)
        {
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;
            
            // 从配置中查找gameId和levelId
            string levelId = null;

            if (gameId == null)
            {
                callback?.Invoke((1, 0, 0));
                yield break;
            }

            // 从配置中查找levelId
            var levelConfig = UomaDataManager.GetLevelConfig();
            if (levelConfig?.Levels != null && levelConfig.Levels.ContainsKey(gameName))
            {   
                levelId = levelConfig.Levels[gameName][gameLevel-1].id;
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

                StartCoroutine(UpdateLevelInfo(callback));
            });
        }

        private IEnumerator UpdateLevelInfo(Action<(int successCode, int currLevel, int currencyNum)> callback)
        {
            var gameName = UomaController.Instance.GameName;
            var gameId = UomaController.Instance.GameId;
            
            bool userInfoUpdated = false;
            bool gameInfoUpdated = false;
            int nextLevel = 1;
            int virtualCurrency = 0;

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
                    if (gameState.Games.ContainsKey(gameName) && gameState.Games[gameName].Levels != null)
                    {
                        int highestCompletedLevel = -1;
                        for (int i = 0; i < gameState.Games[gameName].Levels.Count; i++)
                        {
                            if (gameState.Games[gameName].Levels[i].IsComplete == 1)
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