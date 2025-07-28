using System;
using UnityEngine;

public static class UomaUtils
{
    private static readonly string _baseUrl_Ship = "https://www.uoma.com";
    private static readonly string _baseUrl_Test = "https://test.uoma.com";
    private static string _token = "c222e5c71c92d947a5363bf1d1eedcec9743dae9e3f9d437f27e17997ed837e89fa735405e998ec3b3ee2337bdc9ea2085605";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/138.0.0.0 Safari/537.36";

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