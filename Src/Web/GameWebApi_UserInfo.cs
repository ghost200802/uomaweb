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
}