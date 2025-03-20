using System;

public static class UomaUtils
{
    private static readonly string _baseUrl = "http://test.uoma.com";
    private static readonly string _token = "63640cd232093159c6b70378ab833d832bade56471bb32efaa8b342c64eab1bbb22a6647943c75e7f8721739e1f644e825314";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0";

    public static string BaseUrl => _baseUrl;
    public static string Token => _token;
    public static string AcceptLanguage => _acceptLanguage;
    public static string Platform => _platform;
    public static string UserAgent => _userAgent;
}