using System;
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

        [SerializeField]
        private string gameName = "";

        [SerializeField]
        private string gameId = "";
        
        public string GameName => gameName;
        
        public string GameId => gameId;

        private void Awake()
        {
            Debug.unityLogger.filterLogType = LogType.Warning;
            DontDestroyOnLoad(this.gameObject);
            if (gameObject != null) _gameHelper = gameObject.AddComponent<GameHelper>();

            // 从URL中获取gameId参数
            string url = Application.absoluteURL;
            Debug.Log($"WebUrl: {url}");
            if (!string.IsNullOrEmpty(url))
            {
                int idIndex = url.IndexOf("id=", StringComparison.OrdinalIgnoreCase);
                if (idIndex != -1)
                {
                    string idValue = url.Substring(idIndex + 3);
                    int endIndex = idValue.IndexOf('&');
                    if (endIndex != -1)
                    {
                        idValue = idValue.Substring(0, endIndex);
                    }
                    gameId = idValue;
                    Debug.Log($"从URL获取到gameId: {gameId}");
                }
            }

            Instance = this;
            Debug.Log($"UomaController Awake Done - {gameId}");
#if UNITY_EDITOR
            ReceivePlayerToken(null);
#endif
        }

        public void ReceivePlayerToken(string token) {
            Debug.Log($"Received Token: {token}");
            if (!string.IsNullOrEmpty(token))
            {
                UomaUtils.Token = token;
            }
            Debug.Log($"Use Token: {UomaUtils.Token}");
            UomaDataManager.Init();
            UomaDataManager.SetToken(UomaUtils.Token);
        }

        public IEnumerator GetCompleteLevel(System.Action<int> callback)
        {
            yield return _gameHelper.GetPlayerCompleteLevel((result)=>
            {
                Debug.Log($"CompleteLevel:{result}");
                PlayerPrefs.SetInt($"{UomaUtils.Token}.{UomaController.Instance.GameName}.CompleteLevel", result);
                PlayerPrefs.Save();
                callback?.Invoke(result);
            });
        }

        public IEnumerator CompleteLevel(int gameLevel, int star, System.Action<(int successCode, int currLevel, int currencyNum)> callback)
        {
            yield return _gameHelper.CompleteLevel(gameLevel, star, callback);
        }

        public IEnumerator GetVirtualCurrency(System.Action<int> callback)
        {
            yield return _gameHelper.GetVirtualCurrency(callback);
        }

        public IEnumerator UseGameItem(string itemName, System.Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            yield return _gameHelper.UseGameItem(itemName, callback);
        }

        public IEnumerator BuyGameItem(string itemName, int num, System.Action<(int successCode, int currencyNum, int itemNum)> callback)
        {
            yield return _gameHelper.BuyGameItem(itemName, num, callback);
        }

        public IEnumerator GetGameItemNum(System.Action<Dictionary<string, int>> callback)
        {
            yield return _gameHelper.GetGameItemNum(callback);
        }
    }
}