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
    public IEnumerator PurchaseUserGameItem(string gameId, string gameItemId, string gameItemItemNum, Action<ApiResponse<PurchaseUserGameItemReply>> callback)
    {
        string requestUrl = $"{UomaUtils.BaseUrl}/v1/userGameItems/purchase";
        string requestKey = GetRequestKey(nameof(PurchaseUserGameItem));
        UnityWebRequest request = null;

        
        try
        {
            if (IsRequestInProgress(requestKey))
            {
                Debug.Log($"请求已在进行中: {requestUrl}");
                yield break;
            }

            SetRequestInProgress(requestKey, true);
        
            request = new UnityWebRequest(requestUrl, "POST");
            
            // 输出请求URL
            Debug.Log($"请求URL: {requestUrl}");

            SetCommonHeaders(request);

            var requestBody = new PurchaseUserGameItemRequest
            {
                GameId = gameId,
                GameItemId = gameItemId,
                GameItemItemNum = gameItemItemNum
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
                        errorMessage += $"响应内容: {request.downloadHandler.text}";
                    }

                    Debug.LogError(errorMessage);
                    callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
                    {
                        Code = request.responseCode > 0 ? (int)request.responseCode : -1,
                        Message = errorMessage
                    });
                    yield break;
                }

                var responseContent = request.downloadHandler.text;
                Debug.Log($"响应内容:\n{responseContent}\n");

                var settings = new JsonSerializerSettings
                {
                    NullValueHandling = NullValueHandling.Ignore
                };
                var result = JsonConvert.DeserializeObject<ApiResponse<PurchaseUserGameItemReply>>(responseContent, settings);

                Debug.Log("道具购买信息已更新");

                callback?.Invoke(result);
            }
            catch (Exception e)
            {
                Debug.LogError("处理响应时出错: " + e);
            }
        }
        finally
        {
            request?.Dispose();
            SetRequestInProgress(requestKey, false);
        }
    }
}