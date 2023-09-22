using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.ScreenSystem;
using TaleWorlds.MountAndBlade.View.Screens;
using TaleWorlds.Core;
using TaleWorlds.Engine;
using TaleWorlds.Engine.GauntletUI;
using TaleWorlds.GauntletUI.Data;
using TaleWorlds.InputSystem;
using TaleWorlds.Library;
using TaleWorlds.MountAndBlade.CustomBattle;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;

namespace FantasyArena
{
    [GameStateScreen(typeof(ArenaState))]
    internal class ArenaScreenManager : ScreenBase, IGameStateListener
    {

        public ArenaScreenManager(ArenaState arenaState)
        {
            this._arenaState = arenaState; 
        }

        void IGameStateListener.OnActivate(){}

        void IGameStateListener.OnDeactivate(){}

        void IGameStateListener.OnInitialize() { }

        void IGameStateListener.OnFinalize()
        {
            this._dataSource.OnFinalize();
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            this._dataSource = new ArenaMenuVM(this._arenaState);
            this._dataSource.SetStartInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Start"));
            this._dataSource.SetCancelInputKey(HotKeyManager.GetCategory("GenericPanelGameKeyCategory").GetHotKey("Exit"));
            this._gauntletLayer = new GauntletLayer(1, "GauntletLayer", true);
            this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericPanelGameKeyCategory"));
            this._gauntletLayer.Input.RegisterHotKeyCategory(HotKeyManager.GetCategory("GenericCampaignPanelsGameKeyCategory"));
            this.LoadMovie();
            this._gauntletLayer.InputRestrictions.SetInputRestrictions(true, InputUsageMask.All);
            this._dataSource.SetActiveState(true);
            base.AddLayer(this._gauntletLayer);

        }

        protected override void OnFrameTick(float dt)
        {
            base.OnFrameTick(dt);
            if (this._isFirstFrameCounter >= 0)
            {
                if (this._isFirstFrameCounter == 0)
                {
                    LoadingWindow.DisableGlobalLoadingWindow();
                }
                else
                {
                    this._shouldTickLayersThisFrame = false;
                }
                this._isFirstFrameCounter--;
            }
            if (!this._gauntletLayer.IsFocusedOnInput())
            {
               
                if (this._gauntletLayer.Input.IsHotKeyDownAndReleased("Exit"))
                {
                    this._dataSource.ExecuteBack();
                    return;
                }
                if (this._gauntletLayer.Input.IsHotKeyDownAndReleased("Start"))
                {
                    this._dataSource.ExecuteStart();
                }
                
            }
        }

        // Token: 0x060000AF RID: 175 RVA: 0x00007346 File Offset: 0x00005546
        protected override void OnFinalize()
        {
            this.UnloadMovie();
            base.RemoveLayer(this._gauntletLayer);
            this._dataSource = null;
            this._gauntletLayer = null;
            base.OnFinalize();
        }

        // Token: 0x060000B0 RID: 176 RVA: 0x0000736E File Offset: 0x0000556E
        protected override void OnActivate()
        {
            this.LoadMovie();
            ArenaMenuVM dataSource = this._dataSource;
            if (dataSource != null)
            {
                dataSource.SetActiveState(true);
            }
            this._gauntletLayer.IsFocusLayer = true;
            ScreenManager.TrySetFocus(this._gauntletLayer);
            this._isFirstFrameCounter = 2;
            base.OnActivate();
        }

        // Token: 0x060000B1 RID: 177 RVA: 0x000073AC File Offset: 0x000055AC
        protected override void OnDeactivate()
        {
            base.OnDeactivate();
            this.UnloadMovie();
            ArenaMenuVM dataSource = this._dataSource;
            if (dataSource == null)
            {
                return;
            }
            dataSource.SetActiveState(false);
        }

        // Token: 0x060000B2 RID: 178 RVA: 0x000073CB File Offset: 0x000055CB
        public override void UpdateLayout()
        {
            base.UpdateLayout();
            if (!this._isMovieLoaded)
            {
                ArenaMenuVM dataSource = this._dataSource;
                if (dataSource == null)
                {
                    return;
                }
                dataSource.RefreshValues();
            }
        }

        // Token: 0x060000B3 RID: 179 RVA: 0x000073EB File Offset: 0x000055EB
        private void LoadMovie()
        {
            if (!this._isMovieLoaded)
            {
                this._gauntletMovie = this._gauntletLayer.LoadMovie("ArenaScreenMain", this._dataSource);
                this._isMovieLoaded = true;
            }
        }

        // Token: 0x060000B4 RID: 180 RVA: 0x00007418 File Offset: 0x00005618
        private void UnloadMovie()
        {
            if (this._isMovieLoaded)
            {
                this._gauntletLayer.ReleaseMovie(this._gauntletMovie);
                this._gauntletMovie = null;
                this._isMovieLoaded = false;
                this._gauntletLayer.IsFocusLayer = false;
                ScreenManager.TryLoseFocus(this._gauntletLayer);
            }
        }

        private ArenaState _arenaState; //When the game is finally loaded, we can init our Menu

        private ArenaMenuVM _dataSource; //Our menu for our game


        /* Deals with all our UI*/
        private GauntletLayer _gauntletLayer;

        private IGauntletMovie _gauntletMovie;

        private bool _isMovieLoaded;

        private int _isFirstFrameCounter;
    }
}
