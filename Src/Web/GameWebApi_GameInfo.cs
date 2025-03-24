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
    public IEnumerator GetGameInfo(string gameId, Action<ApiResponse<GameInfo>> callback)
    {
        UnityWebRequest request = null;
        request = new UnityWebRequest($"{UomaUtils.BaseUrl}/v1/games/{gameId}", "GET");
        
        // 输出请求URL
        Debug.Log($"\n请求URL: {UomaUtils.BaseUrl}/v1/games/{gameId}");
        
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
                    errorMessage += $"\n响应内容: {request.downloadHandler.text}";
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
            Debug.Log($"\n响应内容:\n{responseContent}\n");

            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var result = JsonConvert.DeserializeObject<ApiResponse<GameInfo>>(responseContent, settings);

            if (result?.Data != null)
            {
                Debug.Log("游戏信息已更新");
            }
            else
            {
                Debug.LogWarning("响应成功但未包含游戏数据");
            }

            callback?.Invoke(result);
        }
        catch (JsonException jsonEx)
        {
            var errorMessage = $"解析响应数据失败: {jsonEx.Message}";
            Debug.LogError($"{errorMessage}\n响应内容: {request.downloadHandler.text}");
            callback?.Invoke(new ApiResponse<GameInfo>
            {
                Code = -1,
                Message = errorMessage
            });
        }
        catch (Exception ex)
        {
            Debug.LogError($"获取游戏信息时发生异常: {ex.Message}\n{ex.StackTrace}");
            callback?.Invoke(new ApiResponse<GameInfo>
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