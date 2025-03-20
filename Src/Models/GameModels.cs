using System.Collections.Generic;

namespace UomaWeb.Models
{
    public class ApiResponse<T>
    {
        public int Code { get; set; }
        public T Data { get; set; }
    }

    public class GameInfo
    {
        public string GameId { get; set; }
        public string GameName { get; set; }
        public List<string> GameImg { get; set; }
        public string GameDetail { get; set; }
        public List<GameLevel> GameLevel { get; set; }
        public List<GameItem> GameItem { get; set; }
    }

    public class GameLevel
    {
        public string GameLevelId { get; set; }
        public string GameLevelName { get; set; }
        public int IsComplete { get; set; }
    }

    public class GameItem
    {
        public string GameItemId { get; set; }
        public string GameItemName { get; set; }
        public List<string> GameItemImg { get; set; }
        public string GameItemDesc { get; set; }
        public int IsFree { get; set; }
        public string VirtualCurrencyPrice { get; set; }
        public string Stock { get; set; }
        public decimal Price { get; set; }
        public string Status { get; set; }
    }
}