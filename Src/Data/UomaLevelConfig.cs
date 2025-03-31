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
                    { "ScrewRemoval", new List<LevelAction>
                        {
                            new LevelAction { id = "1904079870133575680", name = "第1关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870171324416", name = "第2关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870200684544", name = "第3关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870230044672", name = "第4关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870259404800", name = "第5关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870288764928", name = "第6关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870318125056", name = "第7关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870347485184", name = "第8关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870381039616", name = "第9关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870410399744", name = "第10关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870439759872", name = "第11关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870469120000", name = "第12关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870498480128", name = "第13关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870527840256", name = "第14关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870557200384", name = "第15关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870586560512", name = "第16关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870615920640", name = "第17关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870645280768", name = "第18关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870678835200", name = "第19关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870708195328", name = "第20关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870733361152", name = "第21关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870762721280", name = "第22关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870792081408", name = "第23关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870850801664", name = "第24关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870884356096", name = "第25关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870913716224", name = "第26关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870943076352", name = "第27关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079870989213696", name = "第28关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079871052128256", name = "第29关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904079871089876992", name = "第30关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904416925304791040", name = "第31关", isComplete = 0, gameLevelStar = "0" }
                        }
                    },
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
                    },
                    { "WaterSort", new List<LevelAction>
                        {
                            new LevelAction { id = "1904084068396998656", name = "第1关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068556382208", name = "第2关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068581548032", name = "第3关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068610908160", name = "第4关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068640268288", name = "第5关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068669628416", name = "第6关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068698988544", name = "第7关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068724154368", name = "第8关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068749320192", name = "第9关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068782874624", name = "第10关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068812234752", name = "第11关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068841594880", name = "第12关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068866760704", name = "第13关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068900315136", name = "第14关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068925480960", name = "第15关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068959035392", name = "第16关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084068992589824", name = "第17关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084069021949952", name = "第18关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084069055504384", name = "第19关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904084069084864512", name = "第20关", isComplete = 0, gameLevelStar = "0" }
                        }
                    },
                    { "KnifeHit", new List<LevelAction>
                        {
                            new LevelAction { id = "1904086017104191488", name = "第1关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017146134528", name = "第2关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017171300352", name = "第3关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017196466176", name = "第4关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017217437696", name = "第5关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017238409216", name = "第6关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017263575040", name = "第7关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017288740864", name = "第8关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017318100992", name = "第9关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017343266816", name = "第10关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017368432640", name = "第11关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017393598464", name = "第12关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017418764288", name = "第13关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017443930112", name = "第14关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017469095936", name = "第15关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017494261760", name = "第16关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017519427584", name = "第17关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017540399104", name = "第18关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017569759232", name = "第19关", isComplete = 0, gameLevelStar = "0" },
                            new LevelAction { id = "1904086017594925056", name = "第20关", isComplete = 0, gameLevelStar = "0" }
                        }
                    }
                }
            };
        }
    }
}