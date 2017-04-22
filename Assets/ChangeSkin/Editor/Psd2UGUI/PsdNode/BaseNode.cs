using LitJson;
using UnityEngine;

namespace Psd2UGUI
{
    public abstract class BaseNode
    {
        public string Name;
        public string Type;
        public int X;
        public int Y;
        public int Width;
        public int Height;
        public BaseNode[] Children;

        protected GameObject gameObject;

        protected GameObject CreateGameObject(Transform parent)
        {
            GameObject go = new GameObject();
            go.name = Name;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = Vector2.up;
            rect.anchorMax = Vector2.up;
            rect.anchorMin = Vector2.up;
            rect.sizeDelta = new Vector2(Width, Height);
            rect.anchoredPosition3D = new Vector3(X, -Y, 0);
            go.transform.SetParent(parent, false);
            return go;
        }

        //TODO 这个接口不好
        protected GameObject CreateGameObject(JsonData jsonData, Transform parent)
        {
            GameObject go = new GameObject();
            go.name = Name;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = Vector2.up;
            rect.anchorMax = Vector2.up;
            rect.anchorMin = Vector2.up;
            rect.sizeDelta = new Vector2((int)jsonData[NodeField.WIDTH], (int)jsonData[NodeField.HEIGHT]);
            rect.anchoredPosition3D = new Vector3((int)jsonData[NodeField.X], -(int)jsonData[NodeField.Y], 0);
            go.transform.SetParent(parent, false);
            return go;
        }

        public string GetJson(int depth = 0)
        {
            string result = string.Empty;
            string prefix = new string(' ', depth * 4);
            
            result += prefix + "Name:" + Name + "   Type:" + GetType().ToString() + "\n";
            if(Children != null)
            {
                for(int i = 0; i < Children.Length; i++)
                {
                    BaseNode child = Children[i];
                    result += child.GetJson(depth + 1);
                }
            }
            return result;
        }

        protected void AdjustPosition(GameObject go, Transform parent)
        {
            RectTransform parentRect = parent.GetComponent<RectTransform>();
            //root节点的父节点没有 parentRect
            if(parentRect != null)
            {
                Vector3 parentAnchoredPosition = parentRect.anchoredPosition3D;
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.anchoredPosition3D = rect.anchoredPosition3D - parentAnchoredPosition;
            }
        }

        protected bool HaveChildren(JsonData jsonData)
        {
            if(jsonData.Keys.Contains(NodeField.CHILDREN))
            {
                return true;
            }
            return false;
        }

        //拆分开，不少是初始化
        public virtual void ProcessStruct(JsonData jsonData)
        {
        }

        public abstract void Build(Transform parent);
    }
}
