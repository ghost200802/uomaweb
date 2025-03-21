using System;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using UomaWeb.Models;
using Newtonsoft.Json;
using UomaWeb;

namespace UomaWeb
{
    public class WebApiTest
    {
        private readonly GameWebApi _apiClient;

        public WebApiTest()
        {
            _apiClient = new GameWebApi();
        }

        public async Task RunTest()
        {
            while (true)
            {
                Console.WriteLine("\n请选择WebAPI测试操作：");
                Console.WriteLine("1. 获取玩家信息");
                Console.WriteLine("2. 选择游戏并获取游戏信息");
                Console.WriteLine("3. 查看内存数据");
                Console.WriteLine("4. 购买游戏道具");
                Console.WriteLine("5. 消耗游戏道具");
                Console.WriteLine("6. 返回主菜单");

                var choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        await GetUserInfo();
                        break;

                    case "2":
                        await GetGameInfo();
                        break;

                    case "3":
                        ShowGameState();
                        break;

                    case "4":
                        await PurchaseGameItem();
                        break;

                    case "5":
                        await ConsumeGameItem();
                        break;

                    case "6":
                        return;

                    default:
                        Console.WriteLine("无效的选择");
                        break;
                }
            }
        }

        private async Task GetUserInfo()
        {
            var userResponse = await _apiClient.GetUserInfoAsync();
            if (userResponse?.Data != null)
            {
                GameDataManager.UpdateUserData(userResponse.Data);
            }
        }

        private async Task GetGameInfo()
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
                await _apiClient.GetGameInfoAsync(selectedGame.Value.id);
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

        private async Task PurchaseGameItem()
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
                        return;
                    }

                    var purchaseResponse = await _apiClient.PurchaseUserGameItemAsync(
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
        }

        private async Task ConsumeGameItem()
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

                    var consumeResponse = await _apiClient.ConsumeUserGameItemAsync(
                        selectedGame.Value.id,
                        selectedItem.Value.id
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
        }
    }
}