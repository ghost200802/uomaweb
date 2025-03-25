using System;

public static class UomaUtils
{
    private static readonly string _baseUrl = "https://test.uoma.com";
    private static readonly string _token = "37eabfd7c31e865b8f0b8066838e376df4e9049dbb8de9441826ca3b6fba1257c8870f5d146abc91dd816682698a6f7295384";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0";

    public static string BaseUrl => _baseUrl;
    public static string Token => _token;
    public static string AcceptLanguage => _acceptLanguage;
    public static string Platform => _platform;
    public static string UserAgent => _userAgent;
}