using System.Collections.Generic;

namespace UomaWeb.Models
{
    public class UomaData
    {
        public string Token { get; set; }
        public PlayerData PlayerData { get; set; }
        public Dictionary<string, GameData> Games { get; set; }
    }

    public class PlayerData
    {
        public string InviteCode { get; set; }
        public string LoginCountryId { get; set; }
        public double AvailableBalanceCny { get; set; }
        public double AvailableBalanceUsd { get; set; }
        public string VirtualCurrency { get; set; }
    }

    public class GameData
    {
        public Dictionary<string, GameItemData> Items { get; set; }
        public List<GameLevelData> Levels { get; set; }
    }

    public class GameLevelData
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int IsComplete { get; set; }
    }

    public class GameItemData
    {
        public int IsFree { get; set; }
        public string VirtualCurrencyPrice { get; set; }
        public string GameItemItemNum { get; set; }
    }
}