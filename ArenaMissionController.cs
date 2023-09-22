using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Library;
using TaleWorlds.CampaignSystem;
using SandBox;
using TaleWorlds.Localization;
using TaleWorlds.CampaignSystem.CharacterDevelopment;
using TaleWorlds.CampaignSystem.ComponentInterfaces;
using System.Runtime.CompilerServices;

namespace FantasyArena
{
    public class ArenaMissionController : MissionLogic
    {

        private BasicCharacterObject playerCharacter;

        private List<Team> AllTeams;

        private List<MatrixFrame> initialSpawnFrames; 
    
        private List<MatrixFrame> spawnFrames;

        private List<Agent> Agents;

        private List<BasicCharacterObject> AgentCharacters;

        private int totalAgentsSpawned = 0;

        private int currentAgentsAlive = 0;

        private float nextSpawnTime;

        private int maxParticipants = 30;

        private bool isPlayerInArena;

        private int OpponentsBeatenByPlayer;

        private bool isFreeForAll;

        private BasicMissionTimer _teleportTimer;

        private int teamAmount = 10;

        public bool IsPlayerSurvived { get; private set; }

        private int AISpawnIndex
        {
            get
            {
                return this.totalAgentsSpawned;
            }
        }

        public int RemainingOpponentCount
        {
            get
            {
                return this.maxParticipants - this.totalAgentsSpawned + this.currentAgentsAlive;
            }
        }



        public ArenaMissionController(List<BasicCharacterObject> AgentCharacters) 
        {
            this.playerCharacter = Game.Current.PlayerTroop;
            this.AgentCharacters = AgentCharacters;
        }
        public override void AfterStart()
        {
            this.InitializeTeamLogic();
            this.CleanSceneData();

            //Get out spawns for the arena, for our AI to use.
            this.initialSpawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena")
                                        select e.GetGlobalFrame()).ToList<MatrixFrame>();
            this.spawnFrames = (from e in base.Mission.Scene.FindEntitiesWithTag("sp_arena_respawn")
                                 select e.GetGlobalFrame()).ToList<MatrixFrame>();
            for (int i = 0; i < this.initialSpawnFrames.Count; i++)
            {
                MatrixFrame value = this.initialSpawnFrames[i];
                value.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
                this.initialSpawnFrames[i] = value;
            }
            for (int j = 0; j < this.spawnFrames.Count; j++)
            {
                MatrixFrame value2 = this.spawnFrames[j];
                value2.rotation.OrthonormalizeAccordingToForwardAndKeepUpAsZAxis();
                this.spawnFrames[j] = value2;
            }
            this.isPlayerInArena = false;
            this.Agents = new List<Agent>();
            this.StartMatch();
            this.SpawnPlayer();
        }

 
        private void StartMatch()
        {
            this.isPlayerInArena = true;
            SandBoxHelpers.MissionHelper.FadeOutAgents(from agent in base.Mission.Agents
            where this.Agents.Contains(agent) || agent.IsMount || agent.IsPlayerControlled
            select agent, true, false);
            ArrangePlayerTeamEnmity();
            if(this.isPlayerInArena) 
            {
                Agent agent2 = this.SpawnArenaAgent(base.Mission.PlayerTeam, this.GetSpawnFrame(false, true));
                agent2.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp);
                this.OpponentsBeatenByPlayer = 0;
                this.Agents.Add(agent2);
 
            }
            int count = this.AllTeams.Count;
            int num = 0;
            while (this.totalAgentsSpawned < 10)
            {
                this.Agents.Add(this.SpawnArenaAgent(this.AllTeams[num % count], this.GetSpawnFrame(true, true)));
                num++;
            }
            this.nextSpawnTime = base.Mission.CurrentTime + 14f;

        }

        //Spawn our NPC, with a spawn point, returning it back to it's caller
        private Agent SpawnArenaAgent(Team team, MatrixFrame frame)
        {
            BasicCharacterObject characterObject;

            characterObject = this.AgentCharacters[MBRandom.RandomInt(AgentCharacters.Count() - 1)];

            //Build our Agent
            AgentBuildData agentBuildData = new AgentBuildData(new BasicBattleAgentOrigin(characterObject)).Team(team).InitialPosition(frame.origin);
            Vec2 vec = frame.rotation.f.AsVec2;
            vec = vec.Normalized();
            Agent agent = base.Mission.SpawnAgent(agentBuildData.InitialDirection(vec).Controller(Agent.ControllerType.AI), false);
            this.currentAgentsAlive++;
            this.totalAgentsSpawned++;
            agent.SetWatchState(Agent.WatchState.Alarmed);
            return agent;
        }

        private void SpawnPlayer()
        {
            MatrixFrame matrixFrame = GetSpawnFrame(false , true);
            //Init our team and position
            AgentBuildData agentBuildData = new AgentBuildData(this.playerCharacter).Team(base.Mission.PlayerTeam).InitialPosition(matrixFrame.origin);
            Vec2 vec = matrixFrame.rotation.f.AsVec2;
            vec = vec.Normalized();
            //Init our extra data, such as a mount, equipmnent, color clothing, et.c
            AgentBuildData agentBuildData2 = agentBuildData.InitialDirection(vec).
                CivilianEquipment(false).NoHorses(false).NoWeapons(false).ClothingColor1(base.Mission.PlayerTeam.Color)
                .ClothingColor2(base.Mission.PlayerTeam.Color2).Controller(Agent.ControllerType.Player);
            Agent agent = base.Mission.SpawnAgent(agentBuildData, false);
            //So we can know when they die...
            base.Mission.MainAgent = agent;
            agent.WieldInitialWeapons(Agent.WeaponWieldActionType.InstantAfterPickUp);
        
        }

        //Create the teams we need, and set all AI that join that team, to be enemies against each other.
        private void InitializeTeamLogic()
        {
            this.AllTeams = new List<Team>();
            base.Mission.Teams.Add(BattleSideEnum.Defender, uint.MaxValue, uint.MaxValue, null, true, false, true);
            base.Mission.PlayerTeam = base.Mission.DefenderTeam;
            while (this.AllTeams.Count < teamAmount)
            {
                this.AllTeams.Add(base.Mission.Teams.Add(BattleSideEnum.Attacker, uint.MaxValue, uint.MaxValue, null, true, false, true));
            }
            for (int i = 0; i < this.AllTeams.Count; i++)
            {
                 for (int j = i + 1; j < this.AllTeams.Count; j++)
                {   
                    this.AllTeams[i].SetIsEnemyOf(this.AllTeams[j], true);
                }
            }
        }

        private bool CheckArenaEndedForPlayer()
        {
            return base.Mission.MainAgent == null || !base.Mission.MainAgent.IsActive() || this.RemainingOpponentCount == 0;
        }

        private void ArrangePlayerTeamEnmity()
        {
            foreach (Team team in this.AllTeams)
            {
                team.SetIsEnemyOf(base.Mission.PlayerTeam, true);
            }
        }

        private void CleanSceneData()
        {
            //Find extra spawn frames, etc, and remove them
            GameEntity item = base.Mission.Scene.FindEntityWithTag("tournament_practice") ?? base.Mission.Scene.FindEntityWithTag("tournament_fight");
            List<GameEntity> list = Mission.Current.Scene.FindEntitiesWithTag("arena_set").ToList<GameEntity>();
            list.Remove(item);
            foreach (GameEntity gameEntity in list)
            {
                gameEntity.Remove(88);
            }
        }

        public override InquiryData OnEndMissionRequest(out bool canPlayerLeave)
        {
            canPlayerLeave = true;
            if (!this.isPlayerInArena)
            {
                return null;
            }
            return new InquiryData(new TextObject("Arena Fight", null).ToString(), GameTexts.FindText("str_give_up_fight", null).ToString(), true, true, GameTexts.FindText("str_ok", null).ToString(), GameTexts.FindText("str_cancel", null).ToString(), new Action(base.Mission.OnEndMissionResult), null, "", 0f, null, null, null);
        }
        public override bool MissionEnded(ref MissionResult missionResult)
        {
            return false;
        }

        //Mission Behaviour, used in our mod
        public override void OnScoreHit(Agent affectedAgent, Agent affectorAgent, WeaponComponentData attackerWeapon, bool isBlocked, bool isSiegeEngineHit, in Blow blow, in AttackCollisionData collisionData, float damagedHp, float hitDistance, float shotDifficulty)
        {
            if (affectorAgent == null)
            {
                return;
            }
            if (affectorAgent.IsMount && affectorAgent.RiderAgent != null)
            {
                affectorAgent = affectorAgent.RiderAgent;
            }
            if (affectorAgent.Character == null || affectedAgent.Character == null)
            {
                return;
            }
            float num = (float)blow.InflictedDamage;
            if (num > affectedAgent.HealthLimit)
            {
                num = affectedAgent.HealthLimit;
            }
            float num2 = num / affectedAgent.HealthLimit;
            this.EnemyHitReward(affectedAgent, affectorAgent, blow.MovementSpeedDamageModifier, shotDifficulty, attackerWeapon, blow.AttackType, 0.5f * num2, num);
        }

        private void EnemyHitReward(Agent affectedAgent, Agent affectorAgent, float lastSpeedBonus, float lastShotDifficulty, WeaponComponentData attackerWeapon, AgentAttackType attackType, float hitpointRatio, float damageAmount)
        {
            BasicCharacterObject affectedCharacter =  affectedAgent.Character;
            BasicCharacterObject affectorCharacter =  affectorAgent.Character;
            if (affectedAgent.Origin != null && affectorAgent != null && affectorAgent.Origin != null)
            {
                bool flag = affectorAgent.MountAgent != null;
                bool isHorseCharge = flag && attackType == AgentAttackType.Collision;
            }
        }

        public override void OnAgentRemoved(Agent affectedAgent, Agent affectorAgent, AgentState agentState, KillingBlow killingBlow)
        {
            if (affectedAgent != null && affectedAgent.IsHuman)
            {
                if (affectedAgent != Agent.Main)
                {
                    this.currentAgentsAlive--;
                }
                if (affectorAgent != null && affectorAgent.IsHuman && affectorAgent == Agent.Main && affectedAgent != Agent.Main)
                {
                    int opponentCountBeatenByPlayer = this.OpponentsBeatenByPlayer;
                    this.OpponentsBeatenByPlayer = opponentCountBeatenByPlayer + 1;
                }
            }
            if (this.Agents.Contains(affectedAgent))
            {
                this.Agents.Remove(affectedAgent);
            }
        }


        public override void OnMissionTick(float dt)
        {
            base.OnMissionTick(dt);
            if (this.currentAgentsAlive < 6 && this.totalAgentsSpawned < 30 && (this.currentAgentsAlive == 2 || this.nextSpawnTime < base.Mission.CurrentTime))
            {
                Team team = this.SelectRandomAITeam();
                Agent agent = this.SpawnArenaAgent(team, this.GetSpawnFrame(true, false));
                this.Agents.Add(agent);
                this.nextSpawnTime = base.Mission.CurrentTime + 14f - (float)this.totalAgentsSpawned / 3f;
                if (this.totalAgentsSpawned == maxParticipants && !this.isPlayerInArena)
                {
                    this.totalAgentsSpawned = 0;
                }
            }
            if (this._teleportTimer == null && this.isPlayerInArena && this.CheckArenaEndedForPlayer())
            {
                this._teleportTimer = new BasicMissionTimer();
                this.IsPlayerSurvived = (base.Mission.MainAgent != null && base.Mission.MainAgent.IsActive());
                if (this.IsPlayerSurvived)
                {
                    MBInformationManager.AddQuickInformation(new TextObject("{=seyti8xR}Victory!", null), 0, null, "event:/ui/mission/arena_victory");
                }
            }
        }

        private Team SelectRandomAITeam()
        {
            Team team = null;
            foreach (Team item in this.AllTeams)
            {
                if (!item.HasBots)
                {
                    team = item;
                    break;
                }
            }
            if (team == null)
            {
                team = this.AllTeams[MBRandom.RandomInt(this.AllTeams.Count - 1) + 1];
            }
            return team;

        }

        private MatrixFrame GetSpawnFrame(bool considerPlayerDistance, bool isInitialSpawn)
        {
            List<MatrixFrame> list = (isInitialSpawn || this.spawnFrames.IsEmpty<MatrixFrame>()) ? this.initialSpawnFrames : this.spawnFrames;
            if (list.Count == 1)
            {
                Debug.FailedAssert("Spawn point count is wrong! Arena practice spawn point set should be used in arena scenes.", "ArenaMissionController.cs", "GetSpawnFrame", 212);
                return list[0];
            }
            MatrixFrame result;
            if (considerPlayerDistance && Agent.Main != null && Agent.Main.IsActive())
            {
                int num = MBRandom.RandomInt(list.Count);
                result = list[num];
                float num2 = float.MinValue;
                for (int i = num + 1; i < num + list.Count; i++)
                {
                    MatrixFrame matrixFrame = list[i % list.Count];
                    float num3 = this.CalculateLocationScore(matrixFrame);
                    if (num3 >= 100f)
                    {
                        result = matrixFrame;
                        break;
                    }
                    if (num3 > num2)
                    {
                        result = matrixFrame;
                        num2 = num3;
                    }
                }
            }
            else
            {
                int num4 = this.totalAgentsSpawned;
                if (this.isPlayerInArena && Agent.Main != null)
                {
                    num4++;
                }
                result = list[num4 % list.Count];
            }
            return result;
        }

        private float CalculateLocationScore(MatrixFrame matrixFrame)
        {
            float num = 100f;
            float num2 = 0.25f;
            float num3 = 0.75f;
            if (matrixFrame.origin.DistanceSquared(Agent.Main.Position) < 144f)
            {
                num *= num2;
            }
            for (int i = 0; i < this.Agents.Count; i++)
            {
                if (this.Agents[i].Position.DistanceSquared(matrixFrame.origin) < 144f)
                {
                    num *= num3;
                }
            }
            return num;
        }

    }
}
