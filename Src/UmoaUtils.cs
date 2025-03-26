using System;

public static class UomaUtils
{
    private static readonly string _baseUrl = "https://test.uoma.com";
    private static readonly string _token = "7ecbc25d8a174158051185fc6c7b641776f1bab4311f4e60d16b58a6871dc871e6f32cc475a9cd9f87aa24517267d94e23652";
    private static readonly string _acceptLanguage = "zh";
    private static readonly string _platform = "user";
    private static readonly string _userAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36";

    public static string BaseUrl => _baseUrl;
    public static string Token => _token;
    public static string AcceptLanguage => _acceptLanguage;
    public static string Platform => _platform;
    public static string UserAgent => _userAgent;
}