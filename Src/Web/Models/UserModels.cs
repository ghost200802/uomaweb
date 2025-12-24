using System;

namespace UomaWeb.Models
{
    public class UserInfo
    {
        public string CountryCode { get; set; }
        public string PhoneCode { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FacebookId { get; set; }
        public string FacebookNickname { get; set; }
        public string GoogleId { get; set; }
        public string GoogleNickname { get; set; }
        public string AppleId { get; set; }
        public string AppleNickname { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Nickname { get; set; }
        public string AvatarUrl { get; set; }
        public string Birthday { get; set; }
        public int Gender { get; set; }
        public string Currency { get; set; }
        public int UserType { get; set; }
        public string InviteCode { get; set; }
        public string LoginCountryId { get; set; }
        public double AvailableBalanceCny { get; set; }
        public double AvailableBalanceUsd { get; set; }
        public string VirtualCurrency { get; set; }
        public string ReferrerId { get; set; }
        public int IsModifyReferrer { get; set; }
        public string Religion { get; set; }
        public string HomeUrl { get; set; }
        public string StoreUrl { get; set; }
        public string CreateTime { get; set; }
    }

    public class VirtualCurrencyBalanceData
    {
        [Newtonsoft.Json.JsonProperty("virtualCurrencyBalance")]
        public string VirtualCurrencyBalance { get; set; }
    }
}