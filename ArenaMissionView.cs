using SandBox.View.Missions.Sound.Components;
using SandBox.View.Missions;
using SandBox.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.View;
using TaleWorlds.MountAndBlade.View.MissionViews;

namespace FantasyArena
{
    [ViewCreatorModule]
    public class ArenaMissionView
    {
        [ViewMethod("Arena")]
        public static MissionView[] OpenArenaMission(Mission mission)
        {
            return new List<MissionView>
            {
                new MissionConversationCameraView(),
                ViewCreator.CreateOptionsUIHandler(),
                ViewCreator.CreateMissionMainAgentEquipDropView(mission),
                ViewCreator.CreateMissionAgentStatusUIHandler(mission),
                ViewCreator.CreateMissionMainAgentEquipmentController(mission),
                ViewCreator.CreateMissionMainAgentCheerBarkControllerView(mission),
                ViewCreator.CreateMissionAgentLockVisualizerView(mission),
                ViewCreator.CreateMissionLeaveView(),
                new MissionItemContourControllerView(),
                new MissionAgentContourControllerView(),
                ViewCreator.CreatePhotoModeView()
            }.ToArray();
        }
    }
}
