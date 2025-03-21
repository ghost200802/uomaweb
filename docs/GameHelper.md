1.查询虚拟币数量
    --返回：CurrencyNum(int)

    调用更新 UserInfoApi
    从DataManager中获取玩家虚拟币数量
    

2.查询玩家当前关卡 GetPlayerCurrLevel
    --输入:gameName(string)
    --返回：level(int)

    根据GameName,从config中查找gameId,调用更新GameInfo的接口
    从DataManager中,找到对应游戏的数据，查找最高通关关卡，将下一关作为当前关卡返回

3.使用道具 UseGameItem
    --输入: gameName(string),itemName(string)
    --返回：succesCode(int),返回：CurrencyNum(int),itemNum(int)

    根据GameName,从config中查找gameId,调用ConsumeItem接口
    如果调用成功，则调用更新UserInfo接口，调用更新GameInfo的接口
    从DataManager中,找到对应的数据，返回玩家的虚拟币数量,以及对应道具使用后的数量

4.购买道具 BuyGameItem
    --输入: gameName(string),itemName(string),num(int)
    --返回：succesCode(int),返回：CurrencyNum(int),itemNum(int)

    根据GameName,从config中查找gameId,调用PurchaseItem接口
    如果调用成功，则调用更新UserInfo接口，调用更新GameInfo的接口
    从DataManager中,找到对应的数据，返回玩家的虚拟币数量,以及对应道具使用后的数量

4.2查询道具数量 GetGameItemNum
    --输入: gameName(string)
    --返回：itemNums(Dictionary<string, int>)

    根据GameName,从config中查找gameId,调用更新GameInfo的接口
    从DataManager中，找到对应游戏的所有道具数量，根据config，组织成itemName和数量的字典返回

