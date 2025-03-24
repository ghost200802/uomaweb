using System;
using System.Collections;
using System.IO;
using System.Linq;
using UomaWeb.Models;
using Newtonsoft.Json;
using UnityEngine;

namespace UomaWeb
{
    public class WebApiTest : MonoBehaviour
    {
        private readonly GameWebApi _apiClient;

        public WebApiTest()
        {
            _apiClient = new GameWebApi();
        }

        public IEnumerator RunTest()
        {
            while (true)
            {
                Console.WriteLine("\n请选择WebAPI测试操作：");
                Console.WriteLine("1. 获取玩家信息");
                Console.WriteLine("2. 选择游戏并获取游戏信息");
                Console.WriteLine("3. 查看内存数据");
                Console.WriteLine("4. 购买游戏道具");
                Console.WriteLine("5. 消耗游戏道具");
                Console.WriteLine("6. 完成游戏关卡");
                Console.WriteLine("7. 返回主菜单");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        yield return GetUserInfo();
                        break;

                    case "2":
                        yield return GetGameInfo();
                        break;

                    case "3":
                        ShowGameState();
                        break;

                    case "4":
                        yield return PurchaseGameItem();
                        break;

                    case "5":
                        yield return ConsumeGameItem();
                        break;

                    case "6":
                        yield return CompleteLevelTest();
                        break;

                    case "7":
                        yield break;

                    default:
                        Console.WriteLine("无效的选择");
                        break;
                }
            }
        }

        private IEnumerator GetUserInfo()
        {
            yield return _apiClient.GetUserInfo((response) =>
            {
                if (response?.Data != null)
                {
                    GameDataManager.UpdateUserData(response.Data);
                }
            });
        }

        private IEnumerator GetGameInfo()
        {
            var gameConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
            var games = JsonConvert.DeserializeObject<GameConfig>(gameConfig);

            Console.WriteLine("\n可选游戏列表：");
            var gameList = games.Games.ToList();
            for (int i = 0; i < gameList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gameList[i].Key}: {gameList[i].Value.name} - {gameList[i].Value.description}");
            }

            Console.WriteLine("\n请输入游戏序号：");
            if (int.TryParse(Console.ReadLine(), out int index) && index > 0 && index <= gameList.Count)
            {
                var selectedGame = gameList[index - 1];
                yield return _apiClient.GetGameInfo(selectedGame.Value.id, null);
            }
            else
            {
                Console.WriteLine("无效的游戏序号");
            }
        }

        private void ShowGameState()
        {
            var gameState = GameDataManager.GetGameState();
            Console.WriteLine("\n当前内存数据：");
            Console.WriteLine(JsonConvert.SerializeObject(gameState, Formatting.Indented));
        }

        private IEnumerator PurchaseGameItem()
        {
            var gameConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
            var itemConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "items.json"));
            var games = JsonConvert.DeserializeObject<GameConfig>(gameConfig);
            var items = JsonConvert.DeserializeObject<ItemConfig>(itemConfig);

            Console.WriteLine("\n可选游戏列表：");
            var gameList = games.Games.ToList();
            for (int i = 0; i < gameList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gameList[i].Key}: {gameList[i].Value.name}");
            }

            Console.WriteLine("\n请输入游戏序号：");
            if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex > 0 && gameIndex <= gameList.Count)
            {
                var selectedGame = gameList[gameIndex - 1];
                var gameItems = items.Items[selectedGame.Key];

                Console.WriteLine("\n可选道具列表：");
                var itemList = gameItems.ToList();
                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {itemList[i].Value.name}");
                }

                Console.WriteLine("\n请输入道具序号：");
                if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= itemList.Count)
                {
                    var selectedItem = itemList[itemIndex - 1];
                    Console.WriteLine("\n请输入购买数量：");
                    if (!int.TryParse(Console.ReadLine(), out int itemNum) || itemNum <= 0)
                    {
                        Console.WriteLine("无效的购买数量，请输入正整数");
                        yield break;
                    }

                    yield return _apiClient.PurchaseUserGameItem(
                        selectedGame.Value.id,
                        selectedItem.Value.id,
                        itemNum.ToString(),
                        (response) =>
                        {
                            if (response == null)
                            {
                                Console.WriteLine("购买失败：服务器响应无效");
                            }
                            else if (response.Code == 200)
                            {
                                Console.WriteLine("购买成功！");
                            }
                            else
                            {
                                Console.WriteLine("购买失败");
                            }
                        });
                }
                else
                {
                    Console.WriteLine("无效的道具序号");
                }
            }
            else
            {
                Console.WriteLine("无效的游戏序号");
            }
        }

        private IEnumerator ConsumeGameItem()
        {
            var gameConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
            var itemConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "items.json"));
            var games = JsonConvert.DeserializeObject<GameConfig>(gameConfig);
            var items = JsonConvert.DeserializeObject<ItemConfig>(itemConfig);

            Console.WriteLine("\n可选游戏列表：");
            var gameList = games.Games.ToList();
            for (int i = 0; i < gameList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gameList[i].Key}: {gameList[i].Value.name}");
            }

            Console.WriteLine("\n请输入游戏序号：");
            if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex > 0 && gameIndex <= gameList.Count)
            {
                var selectedGame = gameList[gameIndex - 1];
                var gameItems = items.Items[selectedGame.Key];

                Console.WriteLine("\n可选道具列表：");
                var itemList = gameItems.ToList();
                for (int i = 0; i < itemList.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {itemList[i].Value.name}");
                }

                Console.WriteLine("\n请输入道具序号：");
                if (int.TryParse(Console.ReadLine(), out int itemIndex) && itemIndex > 0 && itemIndex <= itemList.Count)
                {
                    var selectedItem = itemList[itemIndex - 1];

                    yield return _apiClient.ConsumeUserGameItem(
                        selectedGame.Value.id,
                        selectedItem.Value.id,
                        (response) =>
                        {
                            if (response == null)
                            {
                                Console.WriteLine("消耗失败：服务器响应无效");
                            }
                            else if (response.Code == 200)
                            {
                                Console.WriteLine("消耗成功！");
                            }
                            else
                            {
                                Console.WriteLine("消耗失败");
                            }
                        });
                }
                else
                {
                    Console.WriteLine("无效的道具序号");
                }
            }
            else
            {
                Console.WriteLine("无效的游戏序号");
            }
        }

        private IEnumerator CompleteLevelTest()
        {
            var gameConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
            var levelConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "levels.json"));
            var games = JsonConvert.DeserializeObject<GameConfig>(gameConfig);
            var levels = JsonConvert.DeserializeObject<LevelConfig>(levelConfig);

            Console.WriteLine("\n可选游戏列表：");
            var gameList = games.Games.ToList();
            for (int i = 0; i < gameList.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gameList[i].Key}: {gameList[i].Value.name}");
            }

            Console.WriteLine("\n请输入游戏序号：");
            if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex > 0 && gameIndex <= gameList.Count)
            {
                var selectedGame = gameList[gameIndex - 1];
                var gameLevels = levels.Levels[selectedGame.Key];

                Console.WriteLine("\n可选关卡列表：");
                for (int i = 0; i < gameLevels.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {gameLevels[i].name}");
                }

                Console.WriteLine("\n请输入关卡序号：");
                if (int.TryParse(Console.ReadLine(), out int levelIndex) && levelIndex > 0 && levelIndex <= gameLevels.Count)
                {
                    var selectedLevel = gameLevels[levelIndex - 1];

                    Console.WriteLine("\n请输入星级评分（1-3）：");
                    if (int.TryParse(Console.ReadLine(), out int star) && star >= 1 && star <= 3)
                    {
                        yield return _apiClient.LevelComplete(
                            selectedGame.Value.id,
                            selectedLevel.id,
                            star.ToString(),
                            (response) =>
                            {
                                if (response == null)
                                {
                                    Console.WriteLine("关卡完成提交失败：服务器响应无效");
                                }
                                else if (response.Code == 200)
                                {
                                    Console.WriteLine("关卡完成提交成功！");
                                }
                                else
                                {
                                    Console.WriteLine("关卡完成提交失败");
                                }
                            });
                    }
                    else
                    {
                        Console.WriteLine("无效的星级评分，请输入1-3之间的数字");
                    }
                }
                else
                {
                    Console.WriteLine("无效的关卡序号");
                }
            }
            else
            {
                Console.WriteLine("无效的游戏序号");
            }
        }
    }
}