using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UomaWeb.Models;
using Newtonsoft.Json;
using UomaWeb;

class Program
{
    static async Task Main(string[] args)
    {
        var apiClient = new GameWebApi();

        while (true)
        {
            Console.WriteLine("\n请选择操作：");
            Console.WriteLine("1. 获取玩家信息");
            Console.WriteLine("2. 选择游戏并获取游戏信息");
            Console.WriteLine("3. 查看内存数据");
            Console.WriteLine("4. 购买游戏道具");
            Console.WriteLine("5. 消耗游戏道具");
            Console.WriteLine("6. 退出");

            var choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    var userResponse = await apiClient.GetUserInfoAsync();
                    if (userResponse?.Data != null)
                    {
                        GameDataManager.UpdateUserData(userResponse.Data);
                    }
                    break;

                case "2":
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
                        await apiClient.GetGameInfoAsync(selectedGame.Value.id);
                    }
                    else
                    {
                        Console.WriteLine("无效的游戏序号");
                    }
                    break;

                case "3":
                    var gameState = GameDataManager.GetGameState();
                    Console.WriteLine("\n当前内存数据：");
                    Console.WriteLine(JsonConvert.SerializeObject(gameState, Formatting.Indented));
                    break;

                case "4":
                    var gameConfigForItems = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
                    var itemConfig = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "items.json"));
                    var gamesForItems = JsonConvert.DeserializeObject<GameConfig>(gameConfigForItems);
                    var items = JsonConvert.DeserializeObject<ItemConfig>(itemConfig);

                    Console.WriteLine("\n可选游戏列表：");
                    var gameListForItems = gamesForItems.Games.ToList();
                    for (int i = 0; i < gameListForItems.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {gameListForItems[i].Key}: {gameListForItems[i].Value.name}");
                    }

                    Console.WriteLine("\n请输入游戏序号：");
                    if (int.TryParse(Console.ReadLine(), out int gameIndex) && gameIndex > 0 && gameIndex <= gameListForItems.Count)
                    {
                        var selectedGame = gameListForItems[gameIndex - 1];
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
                                break;
                            }

                            var purchaseResponse = await apiClient.PurchaseUserGameItemAsync(
                                selectedGame.Value.id,
                                selectedItem.Value.id,
                                itemNum.ToString()
                            );

                            if (purchaseResponse == null)
                            {
                                Console.WriteLine("购买失败：服务器响应无效");
                            }
                            else if (purchaseResponse.Code == 200)
                            {
                                Console.WriteLine("购买成功！");
                            }
                            else
                            {
                                Console.WriteLine("购买失败");
                            }
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
                    break;

                case "5":
                    var gameConfigForConsume = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "games.json"));
                    var itemConfigForConsume = File.ReadAllText(Path.Combine(Directory.GetCurrentDirectory(), "config", "items.json"));
                    var gamesForConsume = JsonConvert.DeserializeObject<GameConfig>(gameConfigForConsume);
                    var itemsForConsume = JsonConvert.DeserializeObject<ItemConfig>(itemConfigForConsume);

                    Console.WriteLine("\n可选游戏列表：");
                    var gameListForConsume = gamesForConsume.Games.ToList();
                    for (int i = 0; i < gameListForConsume.Count; i++)
                    {
                        Console.WriteLine($"{i + 1}. {gameListForConsume[i].Key}: {gameListForConsume[i].Value.name}");
                    }

                    Console.WriteLine("\n请输入游戏序号：");
                    if (int.TryParse(Console.ReadLine(), out int gameIndexForConsume) && gameIndexForConsume > 0 && gameIndexForConsume <= gameListForConsume.Count)
                    {
                        var selectedGameForConsume = gameListForConsume[gameIndexForConsume - 1];
                        var gameItemsForConsume = itemsForConsume.Items[selectedGameForConsume.Key];

                        Console.WriteLine("\n可选道具列表：");
                        var itemListForConsume = gameItemsForConsume.ToList();
                        for (int i = 0; i < itemListForConsume.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {itemListForConsume[i].Value.name}");
                        }

                        Console.WriteLine("\n请输入道具序号：");
                        if (int.TryParse(Console.ReadLine(), out int itemIndexForConsume) && itemIndexForConsume > 0 && itemIndexForConsume <= itemListForConsume.Count)
                        {
                            var selectedItemForConsume = itemListForConsume[itemIndexForConsume - 1];

                            var consumeResponse = await apiClient.ConsumeUserGameItemAsync(
                                selectedGameForConsume.Value.id,
                                selectedItemForConsume.Value.id
                            );

                            if (consumeResponse == null)
                            {
                                Console.WriteLine("消耗失败：服务器响应无效");
                            }
                            else if (consumeResponse.Code == 200)
                            {
                                Console.WriteLine("消耗成功！");
                            }
                            else
                            {
                                Console.WriteLine("消耗失败");
                            }
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