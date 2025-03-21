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
    private readonly HttpClient _httpClient;

    public GameWebApi()
    {
        _httpClient = new HttpClient();
    }

    private void SetCommonHeaders(HttpRequestMessage request)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var nonce = GenerateNonce();
        var sign = GenerateSign(nonce, UomaUtils.Token, timestamp);

        GameDataManager.SetToken(UomaUtils.Token);

        request.Headers.Add("timestamp", timestamp);
        request.Headers.Add("token", UomaUtils.Token);
        request.Headers.Add("nonce", nonce);
        request.Headers.Add("sign", sign);
        request.Headers.Add("accept-language", UomaUtils.AcceptLanguage);
        request.Headers.Add("platform", UomaUtils.Platform);
        request.Headers.Add("User-Agent", UomaUtils.UserAgent);
    }

    private string GenerateNonce()
    {
        var random = new Random();
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
        var stringBuilder = new StringBuilder(32);

        for (int i = 0; i < 32; i++)
        {
            stringBuilder.Append(chars[random.Next(chars.Length)]);
        }

        return stringBuilder.ToString();
    }

    private string GenerateSign(string nonce, string token, string timestamp)
    {
        var signString = $"{nonce}{token}{nonce}{timestamp}";
        using (var md5 = MD5.Create())
        {
            var inputBytes = Encoding.UTF8.GetBytes(signString);
            var hashBytes = md5.ComputeHash(inputBytes);
            var sb = new StringBuilder();
            
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("x2"));
            }
            
            return sb.ToString();
        }
    }

    public async Task<ApiResponse<CreateUserGameLevelReply>> CreateUserGameLevelAsync(string gameId, string gameLevelId, string gameLevelStar)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{UomaUtils.BaseUrl}/v1/userGameLevels");
        SetCommonHeaders(request);

        var requestBody = new CreateUserGameLevelRequest
        {
            GameId = gameId,
            GameLevelId = gameLevelId,
            GameLevelStar = gameLevelStar
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        //Console.WriteLine($"\n响应内容:\n{responseContent}\n");

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<CreateUserGameLevelReply>>(responseContent, settings);
    }
}