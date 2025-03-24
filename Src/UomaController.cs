using System.Collections.Generic;
using System.Threading.Tasks;
using UomaWeb.Models;

namespace UomaWeb
{
    public class UomaController
    {
        private readonly GameHelper _gameHelper;
        protected string _gameName;

        public string GameName => _gameName;

        public UomaController()
        {
            _gameHelper = new GameHelper();
        }

        public async Task<int> GetCurrentLevelAsync()
        {
            return await _gameHelper.GetPlayerCurrLevelAsync(GameName);
        }

        public async Task<(int successCode, int currLevel, int currencyNum)> CompleteLevelAsync(int gameLevel, int star)
        {
            return await _gameHelper.CompleteLevelAsync(GameName, gameLevel, star);
        }

        public async Task<int> GetVirtualCurrencyAsync()
        {
            return await _gameHelper.GetVirtualCurrencyAsync();
        }

        public async Task<(int successCode, int currencyNum, int itemNum)> UseGameItemAsync(string itemName)
        {
            return await _gameHelper.UseGameItemAsync(GameName, itemName);
        }

        public async Task<(int successCode, int currencyNum, int itemNum)> BuyGameItemAsync(string itemName, int num)
        {
            return await _gameHelper.BuyGameItemAsync(GameName, itemName, num);
        }

        public async Task<Dictionary<string, int>> GetGameItemNumAsync()
        {
            return await _gameHelper.GetGameItemNumAsync(GameName);
        }
    }
}