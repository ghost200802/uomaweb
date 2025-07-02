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
    public IEnumerator LevelComplete(string gameId, string gameInstanceId, string gameLevelId, int gameLevelStar, Action<ApiResponse<LevelCompleteReply>> callback)
    {
        string requestUrl = $"{UomaUtils.BaseUrl}/v1/userGameLevels";
        string requestKey = GetRequestKey(nameof(LevelComplete));

        if (IsRequestInProgress(requestKey))
        {
            Debug.Log($"请求已在进行中: {requestUrl}");
            yield break;
        }

        SetRequestInProgress(requestKey, true);
        UnityWebRequest request = new UnityWebRequest(requestUrl, "POST");

        try
        {
            // 输出请求URL
            Debug.Log($"请求URL: {UomaUtils.BaseUrl}/v1/userGameLevels");

            SetCommonHeaders(request);

            var requestBody = new LevelCompleteRequest
            {
                GameId = gameId,
                GameLevelId = gameLevelId,
                GameLevelStar = gameLevelStar.ToString(),
                GamePlatformHash = gameInstanceId
            };

            var jsonBody = JsonConvert.SerializeObject(requestBody);
            byte[] bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
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
                    callback?.Invoke(new ApiResponse<LevelCompleteReply>
                    {
                        Code = request.responseCode > 0 ? (int)request.responseCode : -1,
                        Message = errorMessage
                    });
                    yield break;
                }

                var responseContent = request.downloadHandler.text;
                Debug.Log($"响应内容:{responseContent}\n");
                
                callback?.Invoke(new ApiResponse<LevelCompleteReply>
                {
                    Code = request.responseCode > 0 ? (int)request.responseCode : -1,
                    Message = request.downloadHandler?.text
                });
            }
            catch (Exception e)
            {
                Debug.LogError("处理响应时出错: " + e);
            }
        }
        finally
        {
            request.Dispose();
            SetRequestInProgress(requestKey, false);
        }
    }
}