using System;

public static class UomaUtils
{
    private static readonly string _baseUrl = "https://test.uoma.com";
    private static readonly string _token = "bee5032d252ff26149915cc09ed6329bb7cdad86552423f97d7f826015092dde3a8fddfcd8d58bd7de5cf04a4fdd630b60570";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0";

    public static string BaseUrl => _baseUrl;
    public static string Token => _token;
    public static string AcceptLanguage => _acceptLanguage;
    public static string Platform => _platform;
    public static string UserAgent => _userAgent;
}