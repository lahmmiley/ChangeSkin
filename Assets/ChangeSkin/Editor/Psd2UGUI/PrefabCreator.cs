using LitJson;
using System.IO;
using UnityEngine;
using Tool;

namespace Psd2UGUI
{
    public class PrefabCreator
    {
        private static PrefabCreator _instance;
        public static PrefabCreator Instance
        {
            get
            {
                if(_instance == null)
                {
                    _instance = new PrefabCreator();
                }
                return _instance;
            }
        }
        private PrefabCreator(){ }

        public string CurrentName;

        public void Create(string name)
        {
            CurrentName = name;

            StreamReader sr = new StreamReader(string.Format("{0}{1}{2}", FileUtility.UI_DATA_DIR, name, FileUtility.JSON_POSTFIX));
            string content = sr.ReadToEnd();
            JsonData jsonData = JsonMapper.ToObject(content);
            BaseNode root = CreateNodeTree(jsonData);
            GameObject goParent = GameObject.Find("Canvas/New");
            root.Build(goParent.transform);
            GameObject goRoot = goParent.transform.FindChild("root").gameObject;
            goRoot.name = name;
        }

        private BaseNode CreateNodeTree(JsonData jsonData)
        {
            BaseNode node = NodeFactory.Create(jsonData);
            node.ProcessStruct(jsonData);
            if(jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                int length = jsonData[NodeField.CHILDREN].Count;
                node.Children = new BaseNode[length];
                JsonData children = jsonData[NodeField.CHILDREN];
                for(int i = 0; i < length; i++)
                {
                    node.Children[i] =  CreateNodeTree(children[i]);
                }
            }
            return node;
        }
    }
}
