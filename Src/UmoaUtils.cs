using System;
using UnityEngine;

public static class UomaUtils
{
    private static readonly string _baseUrl_Ship = "https://www.uoma.com";
    private static readonly string _baseUrl_Test = "https://test.uoma.com";
    private static string _token = "15c01ed8c0b9a86200e7b76fba03d53cf413089e4f2bbcc8c29cb57a105abf5f6931d2f30efa34ef30bf40b1f27844b434589";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/137.0.0.0 Safari/537.36";

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