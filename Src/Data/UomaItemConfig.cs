using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaItemConfig
    {
        public static ItemConfig GetTestConfig()
        {
            return new ItemConfig
            {
                Items = new Dictionary<string, Dictionary<string, ItemAction>>
                {
                    {
                        "CandySweet", new Dictionary<string, ItemAction>
                        {
                            { "ExtraMoves", new ItemAction { id = "1904083507677274112", name = "ExtraMoves" } },
                            { "Stripes", new ItemAction { id = "1904083507719217152", name = "Stripes" } },
                            { "Bomb", new ItemAction { id = "1904083507744382976", name = "Bomb" } },
                            { "MulticolorCandy", new ItemAction { id = "1904083507773743104", name = "MulticolorCandy" } },
                            { "FreeMove", new ItemAction { id = "1905122217961234432", name = "FreeMove" } },
                            { "ExplodeArea", new ItemAction { id = "1905122218015760384", name = "ExplodeArea" } },
                            { "Marmalade", new ItemAction { id = "1905122218045120512", name = "Marmalade" } }
                        }
                    },
                    {
                        "ScrewRemoval", new Dictionary<string, ItemAction>
                        {
                            { "unlock", new ItemAction { id = "1904079871152791552", name = "解锁钉子孔" } },
                            { "skip", new ItemAction { id = "1904079871119237120", name = "跳关" } },
                            { "guide", new ItemAction { id = "1904383674548789248", name = "攻略" } }
                        }
                    },
                    {
                        "WaterSort", new Dictionary<string, ItemAction>
                        {
                            { "skip", new ItemAction { id = "1904084069114224640", name = "下一关" } },
                            { "undo", new ItemAction { id = "1904084069156167680", name = "上一步" } }
                        }
                    },
                    {
                        "KnifeHit", new Dictionary<string, ItemAction>
                        {
                            { "skip", new ItemAction { id = "1904086017620090880", name = "下一关" } },
                            { "undo", new ItemAction { id = "1904086017741725696", name = "上一步" } }
                        }
                    }
                }
            };
        }
        public static ItemConfig GetConfig()
        {
            return new ItemConfig
            {
                Items = new Dictionary<string, Dictionary<string, ItemAction>>
                {
                    { "CandySweet", new Dictionary<string, ItemAction>
                        {
                            { "ExtraMoves", new ItemAction{ id = "1935602532030390272", name = "ExtraMoves"} },
                            { "Stripes", new ItemAction{ id = "1935602532080721920", name = "Stripes"} },
                            { "Bomb", new ItemAction{ id = "1935602532110082048", name = "Bomb"} },
                            { "MulticolorCandy", new ItemAction{ id = "1935602532143636480", name = "MulticolorCandy"} },
                            { "FreeMove", new ItemAction{ id = "1935602532172996608", name = "FreeMove"} },
                            { "ExplodeArea", new ItemAction{ id = "1935602532206551040", name = "ExplodeArea"} },
                            { "Marmalade", new ItemAction{ id = "1935602532240105472", name = "Marmalade"} }
                        }
                    },
                    { "ScrewRemoval", new Dictionary<string, ItemAction>
                        {
                            { "unlock", new ItemAction{ id = "1935595976379375616", name = "解锁钉子孔"} },
                            { "skip", new ItemAction{ id = "1935595976438095872", name = "跳关"} },
                            { "guide", new ItemAction{ id = "1935595976463261696", name = "攻略"} }
                        }
                    },
                    { "WaterSort", new Dictionary<string, ItemAction>
                        {
                            { "skip", new ItemAction{ id = "1935597425414938624", name = "下一关"} },
                            { "undo", new ItemAction{ id = "1935597425465270272", name = "上一步"} }
                        }
                    },
                    { "KnifeHit", new Dictionary<string, ItemAction>
                        {
                            { "skip", new ItemAction{ id = "1935600433095811072", name = "下一关"} },
                            { "undo", new ItemAction{ id = "1935600433125171200", name = "上一步"} }
                        }
                    }
                }
            };
        }
    }
}