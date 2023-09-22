using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;

namespace FantasyArena
{
    public class AgentMainDied : MissionLogic
    {

        private Timer _isAgentDeadTimer;

        public override bool MissionEnded(ref MissionResult missionResult)
        {
            if (base.Mission.MainAgent != null)
            {
                return !base.Mission.MainAgent.IsActive();
            }
            return false;
        }

        public override void OnMissionTick(float dt)
        {
            if (Agent.Main == null || !Agent.Main.IsActive())
            {
                if (_isAgentDeadTimer == null)
                {
                    _isAgentDeadTimer = new Timer(Mission.Current.CurrentTime, 5f);
                }
                if (_isAgentDeadTimer.Check(Mission.Current.CurrentTime))
                {
                    Mission.Current.NextCheckTimeEndMission = 0f;
                    Mission.Current.EndMission();
                }
            }
            else if (_isAgentDeadTimer != null)
            {
                _isAgentDeadTimer = null;
            }
        }
    }
}
