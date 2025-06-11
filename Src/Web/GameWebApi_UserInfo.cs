using System;
using System.Collections;
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

public partial class GameWebApi
{
    public IEnumerator GetUserInfo(Action<ApiResponse<UserInfo>> callback)
    {
        string requestUrl = $"{UomaUtils.BaseUrl}/v1/users";
        DateTime epochStart = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        DateTime currentUtcTime = DateTime.UtcNow;
        long timestamp = (long)(currentUtcTime - epochStart).TotalSeconds;
        requestUrl = $"{requestUrl}?timestamp={timestamp}";
        string requestKey = GetRequestKey(nameof(GetUserInfo));

        // if (IsRequestInProgress(requestKey))
        // {
        //     Debug.Log($"请求已在进行中: {requestUrl}");
        //     yield break;
        // }

        SetRequestInProgress(requestKey, true);
        UnityWebRequest request = null;
        request = new UnityWebRequest(requestUrl, "GET");
        try
        {
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
                        errorMessage += $"\n响应内容: {request.downloadHandler.text}";
                    }
    
                    Debug.LogError(errorMessage);
                    callback?.Invoke(new ApiResponse<UserInfo>
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
                var result = JsonConvert.DeserializeObject<ApiResponse<UserInfo>>(responseContent, settings);


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
                            UomaDataManager.UpdateUserData(result.Data);
                            Debug.Log("用户信息已更新");
                            // Debug.Log("收到用户信息");
                            // Debug.Log("解析后的用户信息:");
                            // Debug.Log($"邀请码: {result.Data.InviteCode}");
                            // Debug.Log($"CNY余额: {result.Data.AvailableBalanceCny}");
                            // Debug.Log($"USD余额: {result.Data.AvailableBalanceUsd}");
                            Debug.Log($"虚拟币: {result.Data.VirtualCurrency}");
                            // Debug.Log($"创建时间: {result.Data.CreateTime}");
                        }
                        else
                        {
                            Debug.LogWarning("响应成功但未包含用户数据");
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
            request.Dispose();
            SetRequestInProgress(requestKey, false);
        }
    }
}