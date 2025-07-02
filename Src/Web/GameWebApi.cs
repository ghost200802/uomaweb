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
    private readonly Dictionary<string, long> _requestInProgress = new Dictionary<string, long>();

    private bool IsRequestInProgress(string requestKey)
    {
        if (!_requestInProgress.ContainsKey(requestKey))
            return false;

        var lastRequestTime = _requestInProgress[requestKey];
        var currentTime = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        var timeDiff = currentTime - lastRequestTime;

        return timeDiff < 2000; // 小于2秒不允许重发
    }

    private void SetRequestInProgress(string requestKey, bool inProgress)
    {
        if (inProgress)
            _requestInProgress[requestKey] = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds();
        else
            _requestInProgress.Remove(requestKey);
    }

    private string GetRequestKey(string functionName)
    {
        return $"function:{functionName}";
    }
    private void SetCommonHeaders(UnityWebRequest request)
    {
        var timestamp = DateTimeOffset.UtcNow.ToUnixTimeMilliseconds().ToString();
        var nonce = GenerateNonce();
        var sign = GenerateSign(nonce, UomaUtils.Token, timestamp);

        UomaDataManager.SetToken(UomaUtils.Token);

        request.SetRequestHeader("timestamp", timestamp);
        request.SetRequestHeader("token", UomaUtils.Token);
        request.SetRequestHeader("nonce", nonce);
        request.SetRequestHeader("sign", sign);
        request.SetRequestHeader("accept-language", UomaUtils.AcceptLanguage);
        request.SetRequestHeader("platform", UomaUtils.Platform);
#if UNITY_EDITOR
        request.SetRequestHeader("user-agent", UomaUtils.TestUserAgent);
#endif
        request.SetRequestHeader("Content-Type", "application/json");
    }

    private string GenerateNonce()
    {
        var random = new System.Random();
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
}