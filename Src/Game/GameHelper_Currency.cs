using System;
using System.Collections;
using UomaWeb.Models;

namespace UomaWeb
{
    public partial class GameHelper
    {
        public IEnumerator GetVirtualCurrency(Action<int> callback)
        {
            yield return _gameWebApi.GetUserInfo((response) =>
            {
                if (response?.Data != null)
                {
                    GameDataManager.UpdateUserData(response.Data);
                    int.TryParse(GameDataManager.GetGameState().PlayerData?.VirtualCurrency, out int virtualCurrency);
                    callback?.Invoke(virtualCurrency);
                }
                else
                {
                    callback?.Invoke(0);
                }
            });
        }
    }
}