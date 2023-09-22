using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Core;
using Module = TaleWorlds.MountAndBlade.Module;

namespace FantasyArena
{
    /*
     * Purpose: Read our XML data, and init our culture and characters.
     */
    public struct ArenaBattleData
    {
        public static IEnumerable<BasicCharacterObject> Characters
        {
            get
            {
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_1");
                if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
                {
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_2");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_3");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_4");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_5");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_6");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_7");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_8");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_9");
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_10");
                }
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_11");
                if (!Module.CurrentModule.IsOnlyCoreContentEnabled)
                {
                    yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("commander_12");
                }
                yield break;
            }
        }

        public static IEnumerable<BasicCharacterObject> ArenaParticipants
        {
            get
            {
                //REGULAR

                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("imperial_veteran_infantryman");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("imperial_archer");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("imperial_heavy_horseman");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("sturgian_spearman");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("sturgian_hardened_brigand");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("aserai_infantry");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("aserai_faris");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("vlandian_swordsman");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("vlandian_hardened_crossbowman");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("vlandian_knight");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_picked_warrior");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_hero");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_woodrunner");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("khuzait_spear_infantry");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("khuzait_lancer");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("khuzait_horse_archer");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_troll");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_troll_archer");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_giant");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("battanian_swamp_gholem");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("vlandian_pontarellian");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("aserai_trgo");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("western_mercenary");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("western_crossbow_t4");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("eastern_mercenary");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("jawwalcannibal_tier_2");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("jawwalcannibal_tier_3");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("jawwalcannibal_tier_1");
                yield return Game.Current.ObjectManager.GetObject<BasicCharacterObject>("forestwild_people_tier_1");


                yield break;    
            }
        }

        public List<BasicCharacterObject> Participants;

        public BasicCharacterObject PlayerCharacter;

        public string SceneId;
    }
}
