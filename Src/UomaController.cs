using System.Collections.Generic;
using System.Threading.Tasks;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaController
    {
        private static readonly GameHelper _gameHelper = new GameHelper();

        public static string GameName {get; set;}

        public static async Task<int> GetCurrentLevelAsync()
        {
            return await _gameHelper.GetPlayerCurrLevelAsync(GameName);
        }

        public static async Task<(int successCode, int currLevel, int currencyNum)> CompleteLevelAsync(int gameLevel, int star)
        {
            return await _gameHelper.CompleteLevelAsync(GameName, gameLevel, star);
        }

        public static async Task<int> GetVirtualCurrencyAsync()
        {
            return await _gameHelper.GetVirtualCurrencyAsync();
        }

        public static async Task<(int successCode, int currencyNum, int itemNum)> UseGameItemAsync(string itemName)
        {
            return await _gameHelper.UseGameItemAsync(GameName, itemName);
        }

        public static async Task<(int successCode, int currencyNum, int itemNum)> BuyGameItemAsync(string itemName, int num)
        {
            return await _gameHelper.BuyGameItemAsync(GameName, itemName, num);
        }

        public static async Task<Dictionary<string, int>> GetGameItemNumAsync()
        {
            return await _gameHelper.GetGameItemNumAsync(GameName);
        }
    }
}