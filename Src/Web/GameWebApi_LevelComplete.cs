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
    public async Task<ApiResponse<LevelCompleteReply>> LevelCompleteAsync(string gameId, string gameLevelId, string gameLevelStar)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{UomaUtils.BaseUrl}/v1/userGameLevels");
        
        // 输出请求URL
        Console.WriteLine($"\n请求URL: {UomaUtils.BaseUrl}/v1/userGameLevels");
        
        SetCommonHeaders(request);
        request.Headers.Add("Accept", "application/json");

        var requestBody = new LevelCompleteRequest
        {
            GameId = gameId,
            GameLevelId = gameLevelId,
            GameLevelStar = gameLevelStar
        };

        var jsonBody = JsonConvert.SerializeObject(requestBody);
        request.Content = new StringContent(jsonBody, Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        Console.WriteLine($"\n响应内容:\n{responseContent}\n");

        if (!response.IsSuccessStatusCode)
        {
            return new ApiResponse<LevelCompleteReply>
            {
                Code = (int)response.StatusCode,
                Message = $"请求失败: {response.StatusCode} - {responseContent}"
            };
        }

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<LevelCompleteReply>>(responseContent, settings);
    }
}