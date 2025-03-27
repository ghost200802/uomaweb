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
                            { "Stripes", new ItemAction{ id = "1904083507719217152", name = "Stripes"} },
                            { "Bomb", new ItemAction{ id = "1904083507744382976", name = "Bomb"} },
                            { "MulticolorCandy", new ItemAction{ id = "1904083507773743104", name = "MulticolorCandy"} },
                            { "FreeMove", new ItemAction{ id = "1905122217961234432", name = "FreeMove"} },
                            { "ExplodeArea", new ItemAction{ id = "1905122218015760384", name = "ExplodeArea"} },
                            { "Marmalade", new ItemAction{ id = "1905122218045120512", name = "Marmalade"} }
                        }
                    },
                    { "ScrewRemoval", new Dictionary<string, ItemAction>
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