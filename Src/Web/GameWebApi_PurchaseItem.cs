using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using System.Threading.Tasks;
using UomaWeb.Models;
using System.Collections.Generic;
using UomaWeb;
using System.IO;

public partial class GameWebApi
{
    public async Task<ApiResponse<PurchaseUserGameItemReply>> PurchaseUserGameItemAsync(string gameId, string gameItemId, string gameItemItemNum)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{UomaUtils.BaseUrl}/v1/userGameItems/purchase");
        
        // 输出请求URL
        Console.WriteLine($"\n请求URL: {UomaUtils.BaseUrl}/v1/userGameItems/purchase");
        
        SetCommonHeaders(request);
        request.Headers.Add("Accept", "application/json");

        var requestBody = new PurchaseUserGameItemRequest
        {
            GameId = gameId,
            GameItemId = gameItemId,
            GameItemItemNum = gameItemItemNum
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);

        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<PurchaseUserGameItemReply>
            {
                Code = (int)response.StatusCode,
                Message = $"请求失败: {response.StatusCode} - {responseContent}"
            };
        }

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<PurchaseUserGameItemReply>>(responseContent, settings);
    }
}