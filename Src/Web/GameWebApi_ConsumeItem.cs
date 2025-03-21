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
    public async Task<ApiResponse<ConsumeUserGameItemReply>> ConsumeUserGameItemAsync(string gameId, string gameItemId)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{UomaUtils.BaseUrl}/v1/userGameItems/consume");
        SetCommonHeaders(request);

        var requestBody = new ConsumeUserGameItemRequest
        {
            GameId = gameId,
            GameItemId = gameItemId
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"\n响应内容:\n{responseContent}\n");

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<ConsumeUserGameItemReply>>(responseContent, settings);
    }
}