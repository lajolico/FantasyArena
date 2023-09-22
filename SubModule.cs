using TaleWorlds.MountAndBlade;
using Module = TaleWorlds.MountAndBlade.Module;
using TaleWorlds.Localization;
using HarmonyLib;

//Purpose: Entry point into our mod
namespace FantasyArena
{
    public class SubModule : MBSubModuleBase
    {

        public const string ModuleId = "FantasyArena";

        /* */
        protected override void OnSubModuleLoad()
        {
            base.OnSubModuleLoad();

            Harmony harmony = new Harmony("arena_edition");
            harmony.PatchAll();

            
            Module.CurrentModule.AddInitialStateOption(new InitialStateOption("ArenaMenuItem", new TextObject("Arena"), 3,
                () =>
                {
                    MBGameManager.StartNewGame(new ArenaGameManager());
                }, ()=> (false, new TextObject())));
        }

        protected override void OnSubModuleUnloaded()
        {
            base.OnSubModuleUnloaded();
        }

        protected override void OnBeforeInitialModuleScreenSetAsRoot()
        {
            base.OnBeforeInitialModuleScreenSetAsRoot();

        }
    }
}