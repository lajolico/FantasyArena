using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using TaleWorlds.Core;
using TaleWorlds.Library;
using TaleWorlds.Localization;
using TaleWorlds.ModuleManager;

namespace FantasyArena
{
    public class ArenaState : GameState
    {

      
        public List<ArenaSceneData> _arenaScenes = new List<ArenaSceneData>();

        public override bool IsMusicMenuState
        {
            get
            {
                return true;
            }
        }

        protected override void OnInitialize()
        {
            base.OnInitialize();
            XmlDocument doc = this.LoadXmlFile(ModuleHelper.GetXmlPath(SubModule.ModuleId, "arena_scenes"));
            LoadScenes(doc);
        }

 

        private void LoadScenes(XmlDocument doc)
        {

            if (doc.ChildNodes.Count == 0)
            {
                throw new TWXmlLoadException("Incorrect XML document format. XML document has no nodes.");
            }
            bool flag = doc.ChildNodes.Item(0).Name.ToLower().Equals("xml");
            if (flag && doc.ChildNodes.Count == 1)
            {
                throw new TWXmlLoadException("Incorrect XML document format. XML document must have at least one child node");
            }
            XmlNode xmlNode = flag ? doc.ChildNodes.Item(1) : doc.ChildNodes.Item(0);
            if (xmlNode.Name != "ArenaScenes")
            {
                throw new TWXmlLoadException("Incorrect XML document format. Root node's name must be CustomBattleScenes.");
            }
            if (xmlNode.Name == "ArenaScenes")
            {
                foreach (object obj in xmlNode.ChildNodes)
                {
                    XmlNode xmlNode2 = (XmlNode)obj;
                    if (xmlNode2.NodeType != XmlNodeType.Comment)
                    {
                        string sceneID = null;
                        TextObject name = null;

                        for (int i = 0; i < xmlNode2.Attributes.Count; i++)
                        {
                            if (xmlNode2.Attributes.Item(i).Name == "id")
                            {
                                sceneID = xmlNode2.Attributes.Item(i).InnerText;
                            }
                            else if (xmlNode2.Attributes.Item(i).Name == "name")
                            {
                                name = new TextObject(xmlNode2.Attributes.Item(i).InnerText, null);
                            }
                        }
                        this._arenaScenes.Add(new ArenaSceneData(sceneID, name));
                    }
                }
            }
        }

        private XmlDocument LoadXmlFile(string path)
        {
            XmlDocument xmlDocument = new XmlDocument();
            StreamReader streamReader = new StreamReader(path);
            string text = streamReader.ReadToEnd();
            xmlDocument.LoadXml(text);
            streamReader.Close();
            return xmlDocument;
        }
    }
}
