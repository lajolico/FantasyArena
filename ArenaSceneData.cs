using System.Collections.Generic;
using TaleWorlds.Core;
using TaleWorlds.Localization;

namespace FantasyArena
{
    /*
     * Purpose: In charge of holding our Arena Scenes and what needs to be included the game to init our scenes.
     */
    public struct ArenaSceneData
    {
        //Get our SceneID from XML
        public string SceneID { get; private set; }

        public TextObject Name { get; private set; }
 
        public ArenaSceneData(string sceneID, TextObject name)
        {
            this.SceneID = sceneID;
            this.Name = name;
        }
    }
}