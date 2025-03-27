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
                    UomaDataManager.UpdateUserData(response.Data);
                    int.TryParse(UomaDataManager.GetState().PlayerData?.VirtualCurrency, out int virtualCurrency);
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