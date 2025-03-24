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
    public IEnumerator ConsumeUserGameItem(string gameId, string gameItemId, Action<ApiResponse<ConsumeUserGameItemReply>> callback)
    {
        UnityWebRequest request = null;
        request = new UnityWebRequest($"{UomaUtils.BaseUrl}/v1/userGameItems/consume", "POST");
        
        // 输出请求URL
        Debug.Log($"\n请求URL: {UomaUtils.BaseUrl}/v1/userGameItems/consume");
        
        SetCommonHeaders(request);

        var requestBody = new ConsumeUserGameItemRequest
        {
            GameId = gameId,
            GameItemId = gameItemId
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        var bodyRaw = Encoding.UTF8.GetBytes(jsonBody);
        request.uploadHandler = new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();
        
        request.Dispose();
    }
}