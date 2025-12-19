using System;
using UnityEngine;

public static class UomaUtils
{
    private static readonly string _baseUrl_Ship = "https://www.uoma.com";
    private static readonly string _baseUrl_Test = "https://test.uoma.com";
    private static string _token = "dc488b07c652ebdac8605ca5b91165cbac4cfd7e4b91c61df6badaa4e961c99864fc91f2f136b84fd09430cd7deefc9257581";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/143.0.0.0 Safari/537.36";

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