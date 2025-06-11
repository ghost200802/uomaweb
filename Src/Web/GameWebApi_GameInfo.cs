using System;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UomaWeb.Models;
using System.Collections.Generic;
using UomaWeb;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using System.Collections;
public partial class GameWebApi
{
    public IEnumerator GetGameInfo(Action<ApiResponse<GameInfo>> callback)
    {
        string requestUrl = $"{UomaUtils.BaseUrl}/v1/games/{UomaController.Instance.GameId}/info";
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime currentUtcTime = DateTime.UtcNow;
        long timestamp = (long)(currentUtcTime - epochStart).TotalSeconds;
        requestUrl = $"{requestUrl}?timestamp={timestamp}";
        string requestKey = GetRequestKey(nameof(GetGameInfo));
        UnityWebRequest request = null;
        try
        {
            // if (IsRequestInProgress(requestKey))
            // {
            //     Debug.Log($"请求已在进行中: {requestUrl}");
            //     yield break;
            // }
            
            SetRequestInProgress(requestKey, true);
            
            request = new UnityWebRequest(requestUrl, "GET");
            // 输出请求URL
            Debug.Log($"请求URL: {requestUrl}");
            
            SetCommonHeaders(request);
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            try
            {
                if (request.result != UnityWebRequest.Result.Success)
                {
                    var errorMessage = $"请求失败: {request.error}";
                    if (request.responseCode > 0)
                    {
                        errorMessage += $" (HTTP {request.responseCode})";
                    }
                    if (!string.IsNullOrEmpty(request.downloadHandler?.text))
                    {
                        errorMessage += $"响应内容: {request.downloadHandler.text}";
                    }
                    Debug.LogError(errorMessage);
                    callback?.Invoke(new ApiResponse<GameInfo>
                    {
                        Code = request.responseCode > 0 ? (int)request.responseCode : -1,
                        Message = errorMessage
                    });
                    yield break;
                }

                var responseContent = request.downloadHandler.text;
                Debug.Log($"响应内容: {responseContent}");

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var result = JsonConvert.DeserializeObject<ApiResponse<GameInfo>>(responseContent, settings);

                switch (result.Code)
                {
                    case 401:
                    {
                        Debug.Log("用户登录已过期，请重新登录");
                        UomaUtils.GameLogout();
                        break;
                    }
                    default:
                    {
                        if (result?.Data != null)
                        {
                            UomaDataManager.UpdateGameData(result.Data);
                            Debug.Log("游戏信息已更新");
                        }
                        else
                        {
                            Debug.LogWarning("响应成功但未包含游戏数据");
                        }

                        break;
                    }
                }

                callback?.Invoke(result);
            }
            catch (Exception e) {
                Debug.LogError("处理响应时出错: " + e);
            }
        }
        finally
        {
            SetRequestInProgress(requestKey, false);
            request?.Dispose();
        }
    }
}