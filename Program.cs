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
        var apiClient = new GameApiClient();

        while (true)
        {
            Console.WriteLine("\n请选择操作：");
            Console.WriteLine("1. 获取玩家信息");
            Console.WriteLine("2. 选择游戏并获取游戏信息");
            Console.WriteLine("3. 查看内存数据");
            Console.WriteLine("4. 退出");

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
                    return;

                default:
                    Console.WriteLine("无效的选择");
                    break;
            }
        }
    }
}