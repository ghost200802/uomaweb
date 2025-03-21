using System.Threading.Tasks;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public async Task<int> GetVirtualCurrencyAsync()
        {
            var response = await _gameWebApi.GetUserInfoAsync();
            if (response?.Data != null)
            {
                GameDataManager.UpdateUserData(response.Data);
                int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out int virtualCurrency);
                return virtualCurrency;
            }
            return 0;
        }
    }
}