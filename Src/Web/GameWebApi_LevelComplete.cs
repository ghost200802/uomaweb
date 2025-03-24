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
    public IEnumerator LevelComplete(string gameId, string gameLevelId, string gameLevelStar, Action<ApiResponse<LevelCompleteReply>> callback)
    {
        UnityWebRequest request = null;
        request = new UnityWebRequest($"{UomaUtils.BaseUrl}/v1/userGameLevels", "POST");
        
        // 输出请求URL
        Debug.Log($"\n请求URL: {UomaUtils.BaseUrl}/v1/userGameLevels");
        
        SetCommonHeaders(request);

        var requestBody = new LevelCompleteRequest
        {
            GameId = gameId,
            GameLevelId = gameLevelId,
            GameLevelStar = gameLevelStar
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
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
                    errorMessage += $"\n响应内容: {request.downloadHandler.text}";
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
            Debug.Log($"\n响应内容:\n{responseContent}\n");

            try
            {
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var result = JsonConvert.DeserializeObject<ApiResponse<LevelCompleteReply>>(responseContent, settings);

                if (result?.Data != null)
                {
                    Debug.Log("关卡完成信息已更新");
                }
                else
                {
                    Debug.LogWarning("响应成功但未包含关卡完成数据");
                }

                callback?.Invoke(result);
            }
            catch (JsonException jsonEx)
            {
                var errorMessage = $"解析响应数据失败: {jsonEx.Message}";
                Debug.LogError($"{errorMessage}\n响应内容: {responseContent}");
                callback?.Invoke(new ApiResponse<LevelCompleteReply>
                {
                    Code = -1,
                    Message = errorMessage
                });
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"更新关卡完成信息时发生异常: {ex.Message}\n{ex.StackTrace}");
            callback?.Invoke(new ApiResponse<LevelCompleteReply>
            {
                Code = -1,
                Message = $"系统错误: {ex.Message}"
            });
        }
        finally
        {
            if (request != null)
            {
                request.Dispose();
            }
        }
    }
}