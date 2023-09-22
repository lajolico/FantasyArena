using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaleWorlds.Library;
using TaleWorlds.Core;
using TaleWorlds.InputSystem;
using TaleWorlds.Localization;
using TaleWorlds.MountAndBlade.CustomBattle.CustomBattle;
using TaleWorlds.MountAndBlade.ViewModelCollection.Input;
using TaleWorlds.MountAndBlade;
using TaleWorlds.Engine;
using System.Runtime.InteropServices;

namespace FantasyArena
{
    internal class ArenaMenuVM : ViewModel
    {

        private const string titleText = "Arena";

        public List<BasicCharacterObject> characters = new List<BasicCharacterObject>();

        public List<BasicCharacterObject> participants = new List<BasicCharacterObject>();

        private ArenaState _arenaState;

        private string _backButtonText;

        private string _startButtonText;

        private string _titleText;

        private InputKeyItemVM _startInputKey;

        private InputKeyItemVM _cancelInputKey;

        public ArenaMenuVM(ArenaState arenaState) 
        {
            _arenaState = arenaState;
            this.RefreshValues();
            foreach (BasicCharacterObject character in ArenaBattleData.Characters)
            {
                this.characters.Add(character);
            }

            foreach (BasicCharacterObject participant in ArenaBattleData.ArenaParticipants)
            {
                this.participants.Add(participant);
            }

        }

        public override void RefreshValues()
        {
            base.RefreshValues();
            this.StartButtonText = GameTexts.FindText("str_start", null).ToString();
            this.BackButtonText = GameTexts.FindText("str_back", null).ToString();
            this.TitleText = GameTexts.FindText("str_back", null).ToString(); //FIX THIS!
        }
        public void SetActiveState(bool isActive)
        {
            if (isActive)
            {
                return;
            }
        }

        public void ExecuteBack()
        {
            Game.Current.GameStateManager.PopState(0);
        }

      
        private ArenaBattleData PrepareArenaData()
        {

            BasicCharacterObject playerCharacter = null;

            string sceneID = null; 
 
            playerCharacter = characters[MBRandom.RandomInt(0, characters.Count-1)];


            sceneID = "arena_rocks";
            

            return ArenaMatch.PrepareArenaData(playerCharacter, sceneID, participants);
        }

        public void ExecuteStart()
        {
            ArenaMatch.StartMatch(this.PrepareArenaData());

        }


        [DataSourceProperty]
        public string TitleText
        {
            get
            {
                return this._titleText;
            }
            set
            {
                if (value != this._titleText)
                {
                    this._titleText = titleText;
                    base.OnPropertyChangedWithValue<string>(value, "TitleText");
                }
            }
        }

        [DataSourceProperty]
        public string BackButtonText
        {
            get
            {
                return this._backButtonText;
            }
            set
            {
                if (value != this._backButtonText)
                {
                    this._backButtonText = value;
                    base.OnPropertyChangedWithValue<string>(value, "BackButtonText");
                }
            }
        }

        [DataSourceProperty]
        public string StartButtonText
        {
            get
            {
                return this._startButtonText;
            }
            set
            {
                if (value != this._startButtonText)
                {
                    this._startButtonText = value;
                    base.OnPropertyChangedWithValue<string>(value, "StartButtonText");
                }
            }
        }

        public void SetStartInputKey(HotKey hotkey)
        {
            this.StartInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
        }

        public void SetCancelInputKey(HotKey hotkey)
        {
            this.CancelInputKey = InputKeyItemVM.CreateFromHotKey(hotkey, true);
        }

        public InputKeyItemVM StartInputKey
        {
            get
            {
                return this._startInputKey;
            }
            set
            {
                if (value != this._startInputKey)
                {
                    this._startInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "StartInputKey");
                }
            }
        }

        public InputKeyItemVM CancelInputKey
        {
            get
            {
                return this._cancelInputKey;
            }
            set
            {
                if (value != this._cancelInputKey)
                {
                    this._cancelInputKey = value;
                    base.OnPropertyChangedWithValue<InputKeyItemVM>(value, "CancelInputKey");
                }
            }
        }



    }
}
