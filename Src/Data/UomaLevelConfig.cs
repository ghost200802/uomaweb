using System.Collections.Generic;
using UomaWeb.Models;

namespace UomaWeb
{
    public static class UomaLevelConfig
    {
        public static LevelConfig GetConfig()
        {
            return new LevelConfig
            {
                Levels = new Dictionary<string, List<LevelAction>>
                {
                    { "CandySweet", new List<LevelAction>
                        {
                            new LevelAction { id = "1904083506381234176", name = "第1关" },
                            new LevelAction { id = "1904083506406400000", name = "第2关" },
                            new LevelAction { id = "1904083506427371520", name = "第3关" },
                            new LevelAction { id = "1904083506452537344", name = "第4关" },
                            new LevelAction { id = "1904083506473508864", name = "第5关" },
                            new LevelAction { id = "1904083506494480384", name = "第6关" },
                            new LevelAction { id = "1904083506515451904", name = "第7关" },
                            new LevelAction { id = "1904083506540617728", name = "第8关" },
                            new LevelAction { id = "1904083506565783552", name = "第9关" },
                            new LevelAction { id = "1904083506590949376", name = "第10关" },
                            new LevelAction { id = "1904083506611920896", name = "第11关" },
                            new LevelAction { id = "1904083506632892416", name = "第12关" },
                            new LevelAction { id = "1904083506658058240", name = "第13关" },
                            new LevelAction { id = "1904083506679029760", name = "第14关" },
                            new LevelAction { id = "1904083506700001280", name = "第15关" },
                            new LevelAction { id = "1904083506725167104", name = "第16关" },
                            new LevelAction { id = "1904083506750332928", name = "第17关" },
                            new LevelAction { id = "1904083506775498752", name = "第18关" },
                            new LevelAction { id = "1904083506800664576", name = "第19关" },
                            new LevelAction { id = "1904083506821636096", name = "第20关" },
                            new LevelAction { id = "1904083506842607616", name = "第21关" },
                            new LevelAction { id = "1904083506867773440", name = "第22关" },
                            new LevelAction { id = "1904083506892939264", name = "第23关" },
                            new LevelAction { id = "1904083506918105088", name = "第24关" },
                            new LevelAction { id = "1904083506943270912", name = "第25关" },
                            new LevelAction { id = "1904083506964242432", name = "第26关" },
                            new LevelAction { id = "1904083506989408256", name = "第27关" },
                            new LevelAction { id = "1904083507014574080", name = "第28关" },
                            new LevelAction { id = "1904083507043934208", name = "第29关" },
                            new LevelAction { id = "1904083507073294336", name = "第30关" },
                            new LevelAction { id = "1904083507098460160", name = "第31关" },
                            new LevelAction { id = "1904083507127820288", name = "第32关" },
                            new LevelAction { id = "1904083507157180416", name = "第33关" },
                            new LevelAction { id = "1904083507186540544", name = "第34关" },
                            new LevelAction { id = "1904083507211706368", name = "第35关" },
                            new LevelAction { id = "1904083507236872192", name = "第36关" },
                            new LevelAction { id = "1904083507262038016", name = "第37关" },
                            new LevelAction { id = "1904083507291398144", name = "第38关" },
                            new LevelAction { id = "1904083507324952576", name = "第39关" },
                            new LevelAction { id = "1904083507350118400", name = "第40关" },
                            new LevelAction { id = "1904083507379478528", name = "第41关" },
                            new LevelAction { id = "1904083507408838656", name = "第42关" },
                            new LevelAction { id = "1904083507434004480", name = "第43关" },
                            new LevelAction { id = "1904083507459170304", name = "第44关" },
                            new LevelAction { id = "1904083507501113344", name = "第45关" },
                            new LevelAction { id = "1904083507543056384", name = "第46关" },
                            new LevelAction { id = "1904083507572416512", name = "第47关" },
                            new LevelAction { id = "1904083507597582336", name = "第48关" },
                            new LevelAction { id = "1904083507622748160", name = "第49关" },
                            new LevelAction { id = "1904083507647913984", name = "第50关" }
                        }
                    }
                }
            };
        }
    }
}