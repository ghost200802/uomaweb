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
        string requestUrl = $"{UomaUtils.BaseUrl}/v1/users/info";
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
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
            Debug.Log($"请求URL_UserInfo: {requestUrl}");
            

            SetCommonHeaders(request);
            string[] headerKeys = {"timestamp","token","nonce","sign","accept-language","platform","User-Agent","Content-Type"};
            
            foreach (var key in headerKeys)
            {
                Debug.Log($"请求Header: {key}-{request.GetRequestHeader(key)}");
            }
            
            request.downloadHandler = new DownloadHandlerBuffer();

            yield return request.SendWebRequest();

            ApiResponse<UserInfo> result = null;
            bool processSuccess = false;

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
                else
                {
                    Debug.Log("响应成功~");
                }
    
                var responseContent = request.downloadHandler.text;
                Debug.Log($"响应内容: {responseContent}");
                
                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                result = JsonConvert.DeserializeObject<ApiResponse<UserInfo>>(responseContent, settings);
                processSuccess = true;
            }
            catch (Exception e) {
                Debug.LogError("处理响应时出错: " + e);
            }

            if (processSuccess && result != null)
            {
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
                            // 获取虚拟币余额
                            string balanceRequestUrl = $"{UomaUtils.BaseUrl}/v1/users/virtualCurrencyBalance";
                            var balanceTimestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
                            balanceRequestUrl = $"{balanceRequestUrl}?timestamp={balanceTimestamp}";
                            
                            using (UnityWebRequest balanceRequest = new UnityWebRequest(balanceRequestUrl, "GET"))
                            {
                                SetCommonHeaders(balanceRequest);
                                balanceRequest.downloadHandler = new DownloadHandlerBuffer();
                                
                                Debug.Log($"请求URL_Balance: {balanceRequestUrl}");
                                yield return balanceRequest.SendWebRequest();
                                
                                if (balanceRequest.result == UnityWebRequest.Result.Success)
                                {
                                    var balanceContent = balanceRequest.downloadHandler.text;
                                    Debug.Log($"余额响应: {balanceContent}");
                                    try 
                                    {
                                        var balanceResult = JsonConvert.DeserializeObject<ApiResponse<VirtualCurrencyBalanceData>>(balanceContent);
                                        if (balanceResult?.Data != null)
                                        {
                                            result.Data.VirtualCurrency = balanceResult.Data.VirtualCurrencyBalance;
                                            Debug.Log($"更新虚拟币余额: {result.Data.VirtualCurrency}");
                                        }
                                    }
                                    catch(Exception ex)
                                    {
                                         Debug.LogError($"解析余额失败: {ex}");
                                    }
                                }
                                else
                                {
                                     Debug.LogError($"获取余额失败: {balanceRequest.error}");
                                }
                            }

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
            
        }
        finally
        {
            request.Dispose();
            SetRequestInProgress(requestKey, false);
        }
    }
}