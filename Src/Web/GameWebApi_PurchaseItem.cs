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
        string requestKey = GetRequestKey(nameof(PurchaseUserGameItem));
        
        if (IsRequestInProgress(requestKey))
        {
            Debug.Log($"请求已在进行中: PurchaseUserGameItem");
            yield break;
        }

        SetRequestInProgress(requestKey, true);

        string idempotentKey = Guid.NewGuid().ToString();
        string paymentAmount = "";
        string orderId = "";
        UnityWebRequest request = null;

        try
        {
            // Step 1: Generate Payment Amount
            string step1Url = $"{UomaUtils.BaseUrl}/v1/orders/paymentAmount";
            request = new UnityWebRequest(step1Url, "POST");
            SetCommonHeaders(request);
            request.SetRequestHeader("idempotentKey", idempotentKey);

            var step1Request = new GenerateOrderPaymentAmountRequest
            {
                OrderType = 12,
                OrderAmountCurrency = "virtualCurrency",
                OrderItem = new OrderItem
                {
                    GameItemOrderItem = new GameItemOrderItem
                    {
                        GameId = gameId,
                        GameItemId = gameItemId,
                        GameItemQuantity = gameItemItemNum
                    }
                }
            };

            var jsonBody = JsonConvert.SerializeObject(step1Request);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log($"Step 1 URL: {step1Url}");
        }
        catch (Exception e)
        {
            Debug.LogError("Step 1 Setup Exception: " + e);
            callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = -1,
                Message = e.Message
            });
            request?.Dispose();
            SetRequestInProgress(requestKey, false);
            yield break;
        }
        
        yield return request.SendWebRequest();

        try
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                HandlePurchaseError(request, callback, "Step 1");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }

            Debug.Log($"响应内容: {request.downloadHandler.text}");
            var step1Response = JsonConvert.DeserializeObject<GenerateOrderPaymentAmountReply>(request.downloadHandler.text);
            
            if (step1Response.Code != 200 || step1Response.Data == null)
            {
                HandlePurchaseError(request, callback, "Step 1 Logic");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }
            paymentAmount = step1Response.Data.PaymentAmount;
        }
        catch (Exception e)
        {
             Debug.LogError("Step 1 Process Exception: " + e);
             callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
             {
                 Code = -1,
                 Message = e.Message
             });
             request.Dispose();
             SetRequestInProgress(requestKey, false);
             yield break;
        }
        finally
        {
            request.Dispose();
        }

        // Step 2: Create Order
        try
        {
            string step2Url = $"{UomaUtils.BaseUrl}/v1/orders";
            request = new UnityWebRequest(step2Url, "POST");
            SetCommonHeaders(request);
            request.SetRequestHeader("idempotentKey", idempotentKey);

            var step2Request = new CreateOrderRequest
            {
                OrderType = 12,
                PaymentAmount = paymentAmount,
                OrderAmountCurrency = "virtualCurrency",
                PaymentChannel = 8,
                ClientPlatform = "webH5",
                OrderItem = new OrderItem
                {
                    GameItemOrderItem = new GameItemOrderItem
                    {
                        GameId = gameId,
                        GameItemId = gameItemId,
                        GameItemQuantity = gameItemItemNum
                    }
                }
            };

            var jsonBody = JsonConvert.SerializeObject(step2Request);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log($"Step 2 URL: {step2Url}");
        }
        catch (Exception e)
        {
            Debug.LogError("Step 2 Setup Exception: " + e);
            callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = -1,
                Message = e.Message
            });
            request?.Dispose();
            SetRequestInProgress(requestKey, false);
            yield break;
        }

        yield return request.SendWebRequest();

        try
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                HandlePurchaseError(request, callback, "Step 2");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }

            Debug.Log($"响应内容: {request.downloadHandler.text}");
            var step2Response = JsonConvert.DeserializeObject<CreateOrderReply>(request.downloadHandler.text);
            if (step2Response.Code != 200 || step2Response.Data == null)
            {
                HandlePurchaseError(request, callback, "Step 2 Logic");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }
            orderId = step2Response.Data.OrderId;
        }
        catch (Exception e)
        {
             Debug.LogError("Step 2 Process Exception: " + e);
             callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
             {
                 Code = -1,
                 Message = e.Message
             });
             request.Dispose();
             SetRequestInProgress(requestKey, false);
             yield break;
        }
        finally
        {
            request.Dispose();
        }

        // Step 3: Pay Order
        try
        {
            string step3Url = $"{UomaUtils.BaseUrl}/v1/orders/{orderId}/payment";
            request = new UnityWebRequest(step3Url, "PUT");
            SetCommonHeaders(request);
            request.SetRequestHeader("idempotentKey", idempotentKey);

            var step3Request = new PayOrderRequest
            {
                OrderId = orderId,
                OrderType = 12,
                ClientPlatform = "webH5"
            };

            var jsonBody = JsonConvert.SerializeObject(step3Request);
            var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
            request.uploadHandler = new UploadHandlerRaw(bodyRaw);
            request.downloadHandler = new DownloadHandlerBuffer();

            Debug.Log($"Step 3 URL: {step3Url}");
        }
        catch (Exception e)
        {
            Debug.LogError("Step 3 Setup Exception: " + e);
            callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = -1,
                Message = e.Message
            });
            request?.Dispose();
            SetRequestInProgress(requestKey, false);
            yield break;
        }

        yield return request.SendWebRequest();

        try
        {
            if (request.result != UnityWebRequest.Result.Success)
            {
                HandlePurchaseError(request, callback, "Step 3");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }

            Debug.Log($"响应内容: {request.downloadHandler.text}");
            var step3Response = JsonConvert.DeserializeObject<PayOrderReply>(request.downloadHandler.text);
            if (step3Response.Code != 200)
            {
                HandlePurchaseError(request, callback, "Step 3 Logic");
                request.Dispose();
                SetRequestInProgress(requestKey, false);
                yield break;
            }

            Debug.Log("Purchase complete!");
            
            callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = 200,
                Data = new PurchaseUserGameItemReply 
                { 
                    Code = 200,
                    Data = new PurchaseUserGameItemReplyData { ShortUrl = "" }
                }
            });
        }
        catch (Exception e)
        {
            Debug.LogError("Step 3 Process Exception: " + e);
            callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = -1,
                Message = e.Message
            });
        }
        finally
        {
            request?.Dispose();
            SetRequestInProgress(requestKey, false);
        }
    }

    private void HandlePurchaseError(UnityWebRequest request, Action<ApiResponse<PurchaseUserGameItemReply>> callback, string prefix)
    {
        var errorMessage = $"{prefix} Request failed: {request.error}";
        if (request.responseCode > 0)
        {
            errorMessage += $" (HTTP {request.responseCode})";
        }
        if (!string.IsNullOrEmpty(request.downloadHandler?.text))
        {
            errorMessage += $" Response: {request.downloadHandler.text}";
        }
        Debug.LogError(errorMessage);
        callback?.Invoke(new ApiResponse<PurchaseUserGameItemReply>
        {
            Code = request.responseCode > 0 ? (int)request.responseCode : -1,
            Message = errorMessage
        });
    }
}