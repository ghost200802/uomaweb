using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public class GameHelperTest
    {
        private readonly GameHelper _gameHelper;

        public GameHelperTest()
        {
            _gameHelper = new GameHelper();
        }

        public async Task RunTest()
        {
            while (true)
            {
                Console.WriteLine("\n请选择GameHelper测试操作：");
                Console.WriteLine("1. 查询虚拟币数量");
                Console.WriteLine("2. 查询玩家当前关卡");
                Console.WriteLine("3. 使用道具");
                Console.WriteLine("4. 购买道具");
                Console.WriteLine("5. 查询道具数量");
                Console.WriteLine("6. 返回主菜单");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        var virtualCurrency = await _gameHelper.GetVirtualCurrencyAsync();
                        Console.WriteLine($"当前虚拟币数量：{virtualCurrency}");
                        break;

                    case "2":
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
                            break;
                        }
                        var gameName = gameList[gameChoice - 1];
                        var level = await _gameHelper.GetPlayerCurrLevelAsync(gameName);
                        Console.WriteLine($"当前关卡：{level}");
                        break;

                    case "3":
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
                            break;
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
                            break;
                        }
                        var itemNameForUse = itemList[itemChoice - 1];

                        var useResult = await _gameHelper.UseGameItemAsync(gameNameForUse, itemNameForUse);
                        if (useResult.successCode == 0)
                        {
                            Console.WriteLine($"使用成功！当前虚拟币：{useResult.currencyNum}，剩余道具数量：{useResult.itemNum}");
                        }
                        else
                        {
                            Console.WriteLine("使用失败");
                        }
                        break;

                    case "4":
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
                            break;
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
                            break;
                        }
                        var itemNameForBuy = itemListForBuy[itemChoiceForBuy - 1];
                        Console.WriteLine("请输入购买数量：");
                        if (!int.TryParse(Console.ReadLine(), out int buyNum) || buyNum <= 0)
                        {
                            Console.WriteLine("无效的购买数量");
                            break;
                        }

                        var buyResult = await _gameHelper.BuyGameItemAsync(gameNameForBuy, itemNameForBuy, buyNum);
                        if (buyResult.successCode == 0)
                        {
                            Console.WriteLine($"购买成功！当前虚拟币：{buyResult.currencyNum}，道具数量：{buyResult.itemNum}");
                        }
                        else
                        {
                            Console.WriteLine("购买失败");
                        }
                        break;

                    case "5":
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
                            break;
                        }
                        var gameNameForQuery = gameListForQuery[gameChoiceForQuery - 1];
                        var itemNums = await _gameHelper.GetGameItemNumAsync(gameNameForQuery);
                        
                        Console.WriteLine("\n道具数量列表：");
                        foreach (var item in itemNums)
                        {
                            Console.WriteLine($"{item.Key}: {item.Value}");
                        }
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("无效的选择");
                        break;
                }
            }
        }
    }
}