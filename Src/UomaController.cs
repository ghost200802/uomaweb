using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;
using UnityEngine.Serialization;
using Debug = UnityEngine.Debug;

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
            DontDestroyOnLoad(this.gameObject);
            if (gameObject != null) _gameHelper = gameObject.AddComponent<GameHelper>();

            // 从URL中获取gameId参数
            string url = Application.absoluteURL;
            Debug.Log($"WebUrl: {url}");
            if (!string.IsNullOrEmpty(url))
            {
                // 直接检查URL是否以localhost开头（没有协议前缀的情况）
                if (url.StartsWith("localhost") || url.StartsWith("localhost:"))
                {
                    UomaUtils.IsTestPlatform = true;
                    Debug.Log("检测到本地开发环境URL（无协议前缀），已设置IsTestPlatform为true");
                }
                else
                {
                    // 检查URL中//后的第一个部分是否为test或者是否为localhost
                    int protocolIndex = url.IndexOf("//");
                    if (protocolIndex != -1)
                    {
                        string afterProtocol = url.Substring(protocolIndex + 2);
                        int firstSlashIndex = afterProtocol.IndexOf('/');
                        string domain = firstSlashIndex != -1 ? afterProtocol.Substring(0, firstSlashIndex) : afterProtocol;
                        
                        // 检查是否为localhost
                        if (domain == "localhost" || domain.StartsWith("localhost:"))
                        {
                            UomaUtils.IsTestPlatform = true;
                            Debug.Log("检测到本地开发环境URL，已设置IsTestPlatform为true");
                        }
                        else
                        {
                            // 检查是否为test开头的域名
                            string[] parts = domain.Split('.');
                            if (parts.Length > 0 && parts[0] == "test")
                            {
                                UomaUtils.IsTestPlatform = true;
                                Debug.Log("检测到测试环境URL，已设置IsTestPlatform为true");
                            }
                        }
                    }
                }

                Debug.unityLogger.filterLogType = UomaUtils.IsTestPlatform ? LogType.Log : LogType.Warning;
                
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
            UomaDataManager.Init();
        }

        public void ReceivePlayerToken(string token) {
            Debug.Log($"Received Token: {token}");
            if (!string.IsNullOrEmpty(token))
            {
                UomaUtils.Token = token;
            }
            Debug.Log($"Use Token: {UomaUtils.Token}");
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