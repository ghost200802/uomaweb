using System;
using System.Collections;
using System.Collections.Generic;
using UomaWeb.Models;
using UnityEngine;

namespace UomaWeb
{
    public class GameHelperTest : MonoBehaviour
    {
        private readonly GameHelper _gameHelper;

        public GameHelperTest()
        {
            _gameHelper = new GameHelper();
        }

        public IEnumerator RunTest()
        {
            while (true)
            {
                Console.WriteLine("\n请选择GameHelper测试操作：");
                Console.WriteLine("1. 查询虚拟币数量");
                Console.WriteLine("2. 查询玩家当前关卡");
                Console.WriteLine("3. 使用道具");
                Console.WriteLine("4. 购买道具");
                Console.WriteLine("5. 查询道具数量");
                Console.WriteLine("6. 通关关卡");
                Console.WriteLine("7. 返回主菜单");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        yield return GetVirtualCurrency();
                        break;

                    case "2":
                        yield return GetPlayerCurrLevel();
                        break;

                    case "3":
                        yield return UseGameItem();
                        break;

                    case "4":
                        yield return BuyGameItem();
                        break;

                    case "5":
                        yield return GetGameItemNum();
                        break;

                    case "6":
                        yield return CompleteLevel();
                        break;

                    case "7":
                        yield break;

                    default:
                        Console.WriteLine("无效的选择");
                        break;
                }
            }
        }

        private IEnumerator GetVirtualCurrency()
        {
            yield return _gameHelper.GetVirtualCurrency((virtualCurrency) =>
            {
                Console.WriteLine($"当前虚拟币数量：{virtualCurrency}");
            });
        }

        private IEnumerator GetPlayerCurrLevel()
        {
            Console.WriteLine("请选择游戏：");
            var gameConfig = GameDataManager.GetGameConfig();
            var gameList = new List<string>();
            var index = 1;
            foreach (var game in gameConfig.Games)
            {
                Console.WriteLine($"{index}. {game.Value.name}");
                gameList.Add(game.Key);
                index++;
            }
            if (!int.TryParse(Console.ReadLine(), out int gameChoice) || gameChoice < 1 || gameChoice > gameList.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var gameName = gameList[gameChoice - 1];
            yield return _gameHelper.GetPlayerCurrLevel(gameName, (level) =>
            {
                Console.WriteLine($"当前关卡：{level}");
            });
        }

        private IEnumerator UseGameItem()
        {
            Console.WriteLine("请选择游戏：");
            var gameConfigForUse = GameDataManager.GetGameConfig();
            var itemConfigForUse = GameDataManager.GetItemConfig();
            var gameListForUse = new List<string>();
            var indexForUse = 1;
            foreach (var game in gameConfigForUse.Games)
            {
                Console.WriteLine($"{indexForUse}. {game.Value.name}");
                gameListForUse.Add(game.Key);
                indexForUse++;
            }
            if (!int.TryParse(Console.ReadLine(), out int gameChoiceForUse) || gameChoiceForUse < 1 || gameChoiceForUse > gameListForUse.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var gameNameForUse = gameListForUse[gameChoiceForUse - 1];

            Console.WriteLine("请选择道具：");
            var itemList = new List<string>();
            var itemIndex = 1;
            foreach (var item in itemConfigForUse.Items[gameNameForUse])
            {
                Console.WriteLine($"{itemIndex}. {item.Value.name}");
                itemList.Add(item.Key);
                itemIndex++;
            }
            if (!int.TryParse(Console.ReadLine(), out int itemChoice) || itemChoice < 1 || itemChoice > itemList.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var itemNameForUse = itemList[itemChoice - 1];

            yield return _gameHelper.UseGameItem(gameNameForUse, itemNameForUse, (result) =>
            {
                if (result.successCode == 0)
                {
                    Console.WriteLine($"使用成功！当前虚拟币：{result.currencyNum}，剩余道具数量：{result.itemNum}");
                }
                else
                {
                    Console.WriteLine("使用失败");
                }
            });
        }

        private IEnumerator BuyGameItem()
        {
            Console.WriteLine("请选择游戏：");
            var gameConfigForBuy = GameDataManager.GetGameConfig();
            var itemConfigForBuy = GameDataManager.GetItemConfig();
            var gameListForBuy = new List<string>();
            var indexForBuy = 1;
            foreach (var game in gameConfigForBuy.Games)
            {
                Console.WriteLine($"{indexForBuy}. {game.Value.name}");
                gameListForBuy.Add(game.Key);
                indexForBuy++;
            }
            if (!int.TryParse(Console.ReadLine(), out int gameChoiceForBuy) || gameChoiceForBuy < 1 || gameChoiceForBuy > gameListForBuy.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var gameNameForBuy = gameListForBuy[gameChoiceForBuy - 1];

            Console.WriteLine("请选择道具：");
            var itemListForBuy = new List<string>();
            var itemIndexForBuy = 1;
            foreach (var item in itemConfigForBuy.Items[gameNameForBuy])
            {
                Console.WriteLine($"{itemIndexForBuy}. {item.Value.name}");
                itemListForBuy.Add(item.Key);
                itemIndexForBuy++;
            }
            if (!int.TryParse(Console.ReadLine(), out int itemChoiceForBuy) || itemChoiceForBuy < 1 || itemChoiceForBuy > itemListForBuy.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var itemNameForBuy = itemListForBuy[itemChoiceForBuy - 1];
            Console.WriteLine("请输入购买数量：");
            if (!int.TryParse(Console.ReadLine(), out int buyNum) || buyNum <= 0)
            {
                Console.WriteLine("无效的购买数量");
                yield break;
            }

            yield return _gameHelper.BuyGameItem(gameNameForBuy, itemNameForBuy, buyNum, (result) =>
            {
                if (result.successCode == 0)
                {
                    Console.WriteLine($"购买成功！当前虚拟币：{result.currencyNum}，道具数量：{result.itemNum}");
                }
                else
                {
                    Console.WriteLine("购买失败");
                }
            });
        }

        private IEnumerator GetGameItemNum()
        {
            Console.WriteLine("请选择游戏：");
            var gameConfigForQuery = GameDataManager.GetGameConfig();
            var gameListForQuery = new List<string>();
            var indexForQuery = 1;
            foreach (var game in gameConfigForQuery.Games)
            {
                Console.WriteLine($"{indexForQuery}. {game.Value.name}");
                gameListForQuery.Add(game.Key);
                indexForQuery++;
            }
            if (!int.TryParse(Console.ReadLine(), out int gameChoiceForQuery) || gameChoiceForQuery < 1 || gameChoiceForQuery > gameListForQuery.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var gameNameForQuery = gameListForQuery[gameChoiceForQuery - 1];
            yield return _gameHelper.GetGameItemNum(gameNameForQuery, (itemNums) =>
            {
                Console.WriteLine("\n道具数量列表：");
                foreach (var item in itemNums)
                {
                    Console.WriteLine($"{item.Key}: {item.Value}");
                }
            });
        }

        private IEnumerator CompleteLevel()
        {
            Console.WriteLine("请选择游戏：");
            var gameConfigForComplete = GameDataManager.GetGameConfig();
            var levelConfig = GameDataManager.GetLevelConfig();
            var gameListForComplete = new List<string>();
            var indexForComplete = 1;
            foreach (var game in gameConfigForComplete.Games)
            {
                Console.WriteLine($"{indexForComplete}. {game.Value.name}");
                gameListForComplete.Add(game.Key);
                indexForComplete++;
            }
            if (!int.TryParse(Console.ReadLine(), out int gameChoiceForComplete) || gameChoiceForComplete < 1 || gameChoiceForComplete > gameListForComplete.Count)
            {
                Console.WriteLine("无效的选择");
                yield break;
            }
            var gameNameForComplete = gameListForComplete[gameChoiceForComplete - 1];
            var gameLevels = levelConfig.Levels[gameNameForComplete];

            Console.WriteLine("\n可选关卡列表：");
            for (int i = 0; i < gameLevels.Count; i++)
            {
                Console.WriteLine($"{i + 1}. {gameLevels[i].name}");
            }

            Console.WriteLine("\n请输入关卡序号：");
            if (!int.TryParse(Console.ReadLine(), out int levelIndex) || levelIndex < 1 || levelIndex > gameLevels.Count)
            {
                Console.WriteLine("无效的关卡序号");
                yield break;
            }

            Console.WriteLine("\n请输入星级评分（1-3）：");
            if (!int.TryParse(Console.ReadLine(), out int star) || star < 1 || star > 3)
            {
                Console.WriteLine("无效的星级评分，请输入1-3之间的数字");
                yield break;
            }

            yield return _gameHelper.CompleteLevel(gameNameForComplete, levelIndex, star, (result) =>
            {
                if (result.successCode == 0)
                {
                    Console.WriteLine("关卡完成提交成功！");
                }
                else
                {
                    Console.WriteLine("关卡完成提交失败");
                }
            });
        }
    }
}