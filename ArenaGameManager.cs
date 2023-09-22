using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.CampaignSystem;
using TaleWorlds.Core;
using TaleWorlds.MountAndBlade;
using TaleWorlds.MountAndBlade.CustomBattle;

namespace FantasyArena
{
    internal class ArenaGameManager : MBGameManager
    {
        /*
         
         */
        protected override void DoLoadingForGameManager(GameManagerLoadingSteps gameManagerLoadingStep, out GameManagerLoadingSteps nextStep)
        {
            nextStep = GameManagerLoadingSteps.None;
            switch (gameManagerLoadingStep)
            {
                case GameManagerLoadingSteps.PreInitializeZerothStep:
                    MBGameManager.LoadModuleData(false);
                    MBGlobals.InitializeReferences();
                    Game.CreateGame(new CustomGame(), this).DoLoading();
                    nextStep = GameManagerLoadingSteps.FirstInitializeFirstStep;
                    return;
                case GameManagerLoadingSteps.FirstInitializeFirstStep:
                    {
                        bool flag = true;
                        foreach (MBSubModuleBase mbsubModuleBase in Module.CurrentModule.SubModules)
                        {
                            flag = (flag && mbsubModuleBase.DoLoading(Game.Current));
                        }
                        nextStep = (flag ? GameManagerLoadingSteps.WaitSecondStep : GameManagerLoadingSteps.FirstInitializeFirstStep);
                        return;
                    }
                case GameManagerLoadingSteps.WaitSecondStep:
                    MBGameManager.StartNewGame();
                    nextStep = GameManagerLoadingSteps.SecondInitializeThirdState;
                    return;
                case GameManagerLoadingSteps.SecondInitializeThirdState:
                    nextStep = (Game.Current.DoLoading() ? GameManagerLoadingSteps.PostInitializeFourthState : GameManagerLoadingSteps.SecondInitializeThirdState);
                    return;
                case GameManagerLoadingSteps.PostInitializeFourthState:
                    nextStep = GameManagerLoadingSteps.FinishLoadingFifthStep;
                    return;
                case GameManagerLoadingSteps.FinishLoadingFifthStep:
                    nextStep = GameManagerLoadingSteps.None;
                    return;
                default:
                    return;
            }
        }

        //Once our game is fully loaded
        public override void OnLoadFinished()
        {
            base.OnLoadFinished();
            //Init our UI
            Game.Current.GameStateManager.CleanAndPushState(Game.Current.GameStateManager.CreateState<ArenaState>(), 0);
        }
    }
}
