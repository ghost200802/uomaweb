using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace UomaWeb
{
    public class UomaController : MonoBehaviour
    {
        public static UomaController Instance = null;
        
        private GameHelper _gameHelper;

         public string gameName = "";

        private void Awake()
        {
            if (gameObject != null) _gameHelper = gameObject.AddComponent<GameHelper>();
            Instance = this;
        }

        public IEnumerator GetCurrentLevel(System.Action<int> callback)
        {
            yield return _gameHelper.GetPlayerCurrLevel(gameName, callback);
        }

        public IEnumerator CompleteLevel(int gameLevel, int star, System.Action<(int successCode, int currLevel, int currencyNum)> callback)
        {
            yield return _gameHelper.CompleteLevel(gameName, gameLevel, star, callback);
        }

        public IEnumerator GetVirtualCurrency(System.Action<int> callback)
        {
            yield return _gameHelper.GetVirtualCurrency(callback);
        }

        public IEnumerator UseGameItem(string itemName, System.Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            yield return _gameHelper.UseGameItem(gameName, itemName, callback);
        }

        public IEnumerator BuyGameItem(string itemName, int num, System.Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            yield return _gameHelper.BuyGameItem(gameName, itemName, num, callback);
        }

        public IEnumerator GetGameItemNum(System.Action<Dictionary<string, int>> callback)
        {
            yield return _gameHelper.GetGameItemNum(gameName, callback);
        }
    }
}