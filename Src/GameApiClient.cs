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

public class GameApiClient
{
    private readonly HttpClient _httpClient;

    public GameApiClient()
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

    public async Task<ApiResponse<GameInfo>> GetGameInfoAsync(string gameId)
    {
        try
        {
            var request = new HttpRequestMessage(HttpMethod.Get, $"{UomaUtils.BaseUrl}/v1/games/{gameId}");
            
            // 输出请求URL
            Console.WriteLine($"\n请求URL: {UomaUtils.BaseUrl}/v1/games/{gameId}");
            
            SetCommonHeaders(request);
        
            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode(); // 确保HTTP响应状态码是成功的
            var responseContent = await response.Content.ReadAsStringAsync();
            //Console.WriteLine($"\n响应内容:\n{responseContent}\n");
            
            var settings = new JsonSerializerSettings
            {
                NullValueHandling = NullValueHandling.Ignore
            };
            var result = JsonConvert.DeserializeObject<ApiResponse<GameInfo>>(responseContent, settings);

            // // 输出游戏道具信息
            // if (result?.Data?.GameItem != null)
            // {
            //     Console.WriteLine("游戏道具:");
            //     foreach (var item in result.Data.GameItem)
            //     {
            //         Console.WriteLine($"  道具ID: {item.GameItemId}");
            //         Console.WriteLine($"  道具名称: {item.GameItemName}");
            //         Console.WriteLine($"  道具详情: {item.GameItemDesc}");
            //         Console.WriteLine($"  是否免费: {item.IsFree}");
            //         Console.WriteLine($"  虚拟币价格: {item.VirtualCurrencyPrice}");
            //         Console.WriteLine($"  库存: {item.Stock}");
            //         Console.WriteLine();
            //     }
            // }
        
            if (result == null)
            {
                throw new JsonException("无法将响应内容解析为有效的游戏数据");
            }
        
            // 如果解析成功，更新游戏数据管理器
            if (result.Data != null)
            {
                GameDataManager.UpdateGameData(gameId, result.Data);
                
                // 输出解析后的游戏信息
                Console.WriteLine("\n解析后的游戏信息:");
                Console.WriteLine($"游戏ID: {result.Data.GameId}");
                Console.WriteLine($"游戏名称: {result.Data.GameName}");
                Console.WriteLine($"游戏详情: {result.Data.GameDetail}");
                
                // if (result.Data.GameLevel?.Count > 0)
                // {
                //     Console.WriteLine("\n游戏关卡:");
                //     foreach (var level in result.Data.GameLevel)
                //     {
                //         Console.WriteLine($"  关卡ID: {level.GameLevelId}");
                //         Console.WriteLine($"  关卡名称: {level.GameLevelName}");
                //         Console.WriteLine($"  是否完成: {level.IsComplete}");
                //         Console.WriteLine();
                //     }
                // }
        
                // if (result.Data.GameItem?.Count > 0)
                // {
                //     Console.WriteLine("游戏道具:");
                //     foreach (var item in result.Data.GameItem)
                //     {
                //         Console.WriteLine($"  道具ID: {item.GameItemId}");
                //         Console.WriteLine($"  道具名称: {item.GameItemName}");
                //         Console.WriteLine($"  道具详情: {item.GameItemDesc}");
                //         Console.WriteLine($"  是否免费: {item.IsFree}");
                //         Console.WriteLine($"  虚拟币价格: {item.VirtualCurrencyPrice}");
                //         Console.WriteLine($"  库存: {item.Stock}");
                //         Console.WriteLine();
                //     }
                // }
            }
            else
            {
                Console.WriteLine("警告：服务器返回的游戏数据为空");
            }
        
            return result;
        }
        catch (HttpRequestException ex)
        {
            Console.WriteLine($"\n发生HTTP请求错误：\n消息：{ex.Message}\n堆栈跟踪：\n{ex.StackTrace}");
            throw;
        }
        catch (JsonException ex)
        {
            Console.WriteLine($"\n发生JSON解析错误：\n消息：{ex.Message}\n堆栈跟踪：\n{ex.StackTrace}");
            throw;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n发生未预期的错误：\n类型：{ex.GetType().Name}\n消息：{ex.Message}\n堆栈跟踪：\n{ex.StackTrace}");
            throw;
        }
    }

    public async Task<ApiResponse<UserInfo>> GetUserInfoAsync()
    {
        var request = new HttpRequestMessage(HttpMethod.Get, $"{UomaUtils.BaseUrl}/v1/users");

        // 输出请求URL
        Console.WriteLine($"\n请求URL: {UomaUtils.BaseUrl}/v1/users");
        
        SetCommonHeaders(request);

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        
        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        var result = JsonConvert.DeserializeObject<ApiResponse<UserInfo>>(responseContent, settings);

        // 如果解析成功，输出用户信息
        if (result?.Data != null)
        {
            Console.WriteLine("\n收到用户信息");
            Console.WriteLine("\n解析后的用户信息:");
            Console.WriteLine($"邀请码: {result.Data.InviteCode}");
            Console.WriteLine($"CNY余额: {result.Data.AvailableBalanceCny}");
            Console.WriteLine($"USD余额: {result.Data.AvailableBalanceUsd}");
            Console.WriteLine($"虚拟币: {result.Data.VirtualCurrency}");
            Console.WriteLine($"创建时间: {result.Data.CreateTime}");
        }

        return result;
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
        //Console.WriteLine($"\n响应内容:\n{responseContent}\n");

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<ConsumeUserGameItemReply>>(responseContent, settings);
    }

    public async Task<ApiResponse<PurchaseUserGameItemReply>> PurchaseUserGameItemAsync(string gameId, string gameItemId, string gameItemItemNum)
    {
        var request = new HttpRequestMessage(HttpMethod.Post, $"{UomaUtils.BaseUrl}/v1/userGameItems/purchase");
        SetCommonHeaders(request);

        var requestBody = new PurchaseUserGameItemRequest
        {
            GameId = gameId,
            GameItemId = gameItemId,
            GameItemItemNum = gameItemItemNum
        };

        request.Content = new StringContent(JsonConvert.SerializeObject(requestBody), Encoding.UTF8, "application/json");

        var response = await _httpClient.SendAsync(request);
        var responseContent = await response.Content.ReadAsStringAsync();
        //Console.WriteLine($"\n响应内容:\n{responseContent}\n");

        var settings = new JsonSerializerSettings
        {
            NullValueHandling = NullValueHandling.Ignore
        };
        return JsonConvert.DeserializeObject<ApiResponse<PurchaseUserGameItemReply>>(responseContent, settings);
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