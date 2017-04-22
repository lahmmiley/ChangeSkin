using LitJson;
using UnityEngine;
using UnityEngine.UI;

namespace Psd2UGUI
{
    public class NodeField
    {
        public const string NAME = "Name";
        public const string TYPE = "Type";
        public const string X = "X";
        public const string Y = "Y";
        public const string WIDTH = "Width";
        public const string HEIGHT = "Height";
        public const string CHILDREN = "Children";
        public const string PARAM = "Param";
        public const string ATTACH = "Attach";
        public const string BELONG_PSD = "BelongPsd";
        public const string SIZE = "Size";
        public const string TEXT = "Text";
        public const string COLOR = "Color";
    }

    public class ScrollViewNode : ContainerNode
    {
        public override void ProcessStruct(JsonData jsonData)
        {
            int maskIndex = FindIndexByName(jsonData, MaskNode.MASK);
            JsonData mask = jsonData[NodeField.CHILDREN][maskIndex];
            JsonData jArray = new JsonData();
            for (int i = 0; i < jsonData[NodeField.CHILDREN].Count; i++)
            {
                JsonData child = jsonData[NodeField.CHILDREN][i];
                if(i != maskIndex)
                {
                    jArray.Add(child);
                }
            }
            mask[NodeField.CHILDREN] = jArray;
            mask[NodeField.TYPE] = MaskNode.MASK;

            jsonData[NodeField.CHILDREN].Clear();
            jsonData[NodeField.CHILDREN].Add(mask);
        }

        public override void Build(Transform parent)
        {
            base.Build(parent);

            Transform transform = gameObject.GetComponent<Transform>(); 
            //TODO
            GameObject goMask = transform.FindChild("mask").gameObject;
            GameObject goContent = null;
            //TODO
            if(transform.FindChild("mask/content") != null)
            {
                goContent = transform.FindChild("mask/content").gameObject;
            }
            else
            {
                goContent = transform.FindChild("mask/Content").gameObject;
            }
            AddScrollRect(goMask, goContent);
            AddLayoutComponent(goContent);
        }

        private void AddScrollRect(GameObject goMask, GameObject goContent)
        {
            ScrollRect scrollRect = gameObject.AddComponent<ScrollRect>();
            scrollRect.content = goContent.GetComponent<RectTransform>();
            //scrollRect= goMask.GetComponent<RectTransform>();
        }


        private void AddLayoutComponent(GameObject goContent)
        {
            goContent.AddComponent<HorizontalLayoutGroup>();
            goContent.AddComponent<ContentSizeFitter>();
        }

        private int FindIndexByName(JsonData jsonData, string name)
        {
            int result = -1;
            for (int i = 0; i < jsonData[NodeField.CHILDREN].Count; i++)
            {
                JsonData child = jsonData[NodeField.CHILDREN][i];
                if(child[NodeField.NAME].ToString().ToLower() == MaskNode.MASK)
                {
                    result = i;
                    break;
                }
            }
            return result;
        }
    }
}
