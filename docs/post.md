### 游戏相关接口

GET /v1/games/{gameld} -> GetGameData.json

PosT /v1/userGameltems/consume
    Body 参数
    application/json
    string gameId
    string gameItemId

    例子：
    {
        "gameId": "1900020634239934464",
        "gameItemId": "1900020669073629184"
    }

    

PosT /v1/userGameltems/purchase
    Body 参数
    application/json
    string gameId
    string gameItemId
    string gameItemItemNum
    
    例子：
    {
        "gameId": "1900020634239934464",
        "gameItemId": "1900020669073629184",
        "gameItemItemNum": "2"
    }

PosT /v1/userGameLevels
    Body 参数
    application/json
    string gameId
    string gameLevelId
    string gameLevelStar

    例子：
    {
        "gameId": "1900020634239934464",
        "gameLevelId": "1900020657178583040",
        "gameLevelStar": "2"
    }

Get /v1/users

### 接口请求头

timestamp: 当前时间戳
token：63640cd232093159c6b70378ab833d832bade56471bb32efaa8b342c64eab1bbb22a6647943c75e7f8721739e1f644e825314
nonce: 随机32位字符串
sign： md5(nonce+token+nonce+timestamp)
accept-language：zh
platform：'user'
User-Agent：Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/134.0.0.0 Safari/537.36 Edg/134.0.0.0


timestamp:{% mock 'timestamp' , 'ms' %}
nonce:{% mock 'string' , '0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz' , 32 , 32 %}
token:63640cd232093159c6b70378ab833d832bade56471bb32efaa8b342c64eab1bbb22a6647943c75e7f8721739e1f644e825314
sign:{{nonce}}{{token}}{{nonce}}{{timestamp}}