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
        var gameName = UomaController.Instance.GameName;
        var gameId = UomaController.Instance.GameId;
        
        UnityWebRequest request = new UnityWebRequest($"{UomaUtils.BaseUrl}/v1/games/{gameId}", "GET");
        try
        {
        
            // 输出请求URL
            Debug.Log($"请求URL: {UomaUtils.BaseUrl}/v1/games/{gameId}");
            
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

                if (result?.Data != null)
                {
                    GameDataManager.UpdateGameData(result.Data);
                    Debug.Log("游戏信息已更新");
                }
                else
                {
                    Debug.LogWarning("响应成功但未包含游戏数据");
                }

                callback?.Invoke(result);
            }
            catch (Exception e) {
                Debug.LogError("处理响应时出错: " + e);
            }
        }
        finally
        {
                request.Dispose();
        }
    }
}