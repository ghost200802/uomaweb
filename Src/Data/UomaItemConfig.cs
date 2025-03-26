using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaItemConfig
    {
        public static ItemConfig GetConfig()
        {
            return new ItemConfig
            {
                Items = new Dictionary<string, Dictionary<string, ItemAction>>
                {
                    { "CandySweet", new Dictionary<string, ItemAction>
                        {
                            { "ExtraMoves", new ItemAction{ id = "1904083507677274112", name = "ExtraMoves"} },
                            { "Packages", new ItemAction{ id = "1904083507677274112", name = "Packages"} },
                            { "Stripes", new ItemAction{ id = "1904083507744382976", name = "炸弹"} },
                            { "ExtraTime", new ItemAction{ id = "1904083507773743104", name = "消除"} },
                            { "Bomb", new ItemAction{ id = "1904083507773743104", name = "消除"} },
                            { "MulticolorCandy", new ItemAction{ id = "1904083507773743104", name = "消除"} },
                            { "FreeMove", new ItemAction{ id = "1904083507773743104", name = "消除"} },
                            { "ExplodeArea", new ItemAction{ id = "1904083507773743104", name = "消除"} },
                            { "Marmalade", new ItemAction{ id = "1904083507773743104", name = "消除"} }
                        }
                    },
                    { "Snail", new Dictionary<string, ItemAction>
                        {
                            { "unlock", new ItemAction{ id = "1900022219774926848", name = "解锁钉子孔"} },
                            { "skip", new ItemAction{ id = "1900022221792387072", name = "跳关"} },
                            { "guide", new ItemAction{ id = "1900022225047166976", name = "攻略"} }
                        }
                    }
                }
            };
        }
    }
}