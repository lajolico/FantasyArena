using HarmonyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem.ViewModelCollection.CharacterDeveloper;
using TaleWorlds.Core;

namespace FantasyArena
{
    public static class ArenaMatch
    { 
        public static void StartMatch(ArenaBattleData data)
        {
            
          /*  if (data.SceneId == null || data.SceneId.IsEmpty())
            {
                data.SceneId = "arena_empire_a";
            }*/

            Game.Current.PlayerTroop = data.PlayerCharacter;

            ArenaMissions.OpenArenaMission(data.SceneId, data.Participants);
        }

        public static ArenaBattleData PrepareArenaData(BasicCharacterObject character, string scene, List<BasicCharacterObject> participants)
        {
            ArenaBattleData data = new ArenaBattleData
            {
                PlayerCharacter = character,
                SceneId = scene,
                Participants = participants
            };

            return data;
        }
    }
}
