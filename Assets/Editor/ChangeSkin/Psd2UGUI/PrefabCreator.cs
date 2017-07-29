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
            ImageDataReader.Instance.Read(name);

            JsonData jsonData = FileUtility.ReadJsonData(string.Format("{0}{1}{2}", FileUtility.UI_DATA_DIR, name, FileUtility.JSON_POSTFIX));
            BaseNode root = CreateNodeTree(jsonData);
            GameObject goParent = GameObject.Find("Canvas/New");
            root.Build(goParent.transform);
            //GameObject goRoot = goParent.transform.FindChild("root").gameObject;
        }

        private BaseNode CreateNodeTree(JsonData jsonData)
        {
            BaseNode node = NodeFactory.Create(jsonData);
            node.ProcessStruct(jsonData);
            if(jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                JsonData children = jsonData[NodeField.CHILDREN];
                int length = children.Count;
                node.Children = new BaseNode[length];
                for(int i = 0; i < length; i++)
                {
                    node.Children[i] =  CreateNodeTree(children[i]);
                }
            }
            return node;
        }
    }
}
