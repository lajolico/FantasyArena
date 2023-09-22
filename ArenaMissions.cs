using TaleWorlds.Core;
using TaleWorlds.MountAndBlade.Source.Missions;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers;
using TaleWorlds.MountAndBlade;
using SandBox.Missions.MissionLogics.Arena;
using SandBox.Missions.MissionLogics;
using TaleWorlds.MountAndBlade.Missions.Handlers;
using TaleWorlds.MountAndBlade.MissionSpawnHandlers;
using TaleWorlds.MountAndBlade.Source.Missions.Handlers.Logic;
using System.Collections.Generic;
using System.Linq;
using TaleWorlds.Network;

namespace FantasyArena
{
    [MissionManager]
    public static class ArenaMissions
    {
        [MissionMethod]
        public static Mission OpenArenaMission(string scene, List<BasicCharacterObject> AgentCharacters)
        {

            //MissionState, Open a new MissionUI, then init our MissionRecord, of what we need to start the mission
            //Definition of "Arena" can be found in ArenaMissionView
            return MissionState.OpenNew("Arena", new MissionInitializerRecord(scene)
            {
                DoNotUseLoadingScreen = false,
                PlayingInCampaignMode = false,
                SceneLevels = "",
                TimeOfDay = 9f,
                AtlasGroup = 2
            }, (Mission missionController) => new MissionBehavior[]
            {
                new MissionOptionsComponent(),
                new EquipmentControllerLeaveLogic(),
                new ArenaMissionController(AgentCharacters),
                new AgentMainDied(),
                new AgentHumanAILogic(),
                new ArenaAgentStateDeciderLogic(),
                new MissionAgentPanicHandler(),
                new ArenaAgentStateDeciderLogic()
 
            }, true,true);
        }
    }
}
