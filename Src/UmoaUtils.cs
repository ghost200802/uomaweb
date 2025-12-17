using System;
using UnityEngine;

public static class UomaUtils
{
    private static readonly string _baseUrl_Ship = "https://www.uoma.com";
    private static readonly string _baseUrl_Test = "https://test.uoma.com";
    private static string _token = "751f8cac1bb9f8200314f77c281bb58bab6aed26931cd79e5a643aba1fb422078def08195d96d81e6db7d047572dbb0162085";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/142.0.0.0 Safari/537.36";

    public static bool IsTestPlatform = false;
    
    public static string BaseUrl => IsTestPlatform ? _baseUrl_Test : _baseUrl_Ship;

    public static string Token
    {
        get => _token;
        set => _token = value;
    }
    public static string AcceptLanguage => _acceptLanguage;
    public static string Platform => _platform;
    
    public static string TestUserAgent => _userAgent;

    public static void GameLogout()
    {
        #if UNITY_WEBGL && !UNITY_EDITOR
            UomaGameLogout();
        #else
            UnityEngine.Debug.Log("GameLogout called, but not in WebGL build");
        #endif
    }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void UomaGameLogout();
}