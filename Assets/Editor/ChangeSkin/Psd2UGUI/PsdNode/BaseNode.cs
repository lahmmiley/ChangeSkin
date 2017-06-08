using LitJson;
using UnityEngine;
using Tool;

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
        public bool Hide = false;

        protected GameObject gameObject;

        public BaseNode(JsonData jsonData)
        {
            this.Name = jsonData[NodeField.NAME].ToString();
            this.X = (int)jsonData[NodeField.X];
            this.Y = (int)jsonData[NodeField.Y];
            this.Width = (int)jsonData[NodeField.WIDTH];
            this.Height = (int)jsonData[NodeField.HEIGHT];
            if (jsonData.ContainKey(NodeField.HIDE))
            {
                this.Hide = true;
            }
        }

        protected GameObject CreateGameObject(string name, 
            float width, float height, float x, float y)
        {
            GameObject go = new GameObject();
            go.layer = LayerMask.NameToLayer("UI");
            go.name = name;
            RectTransform rect = go.AddComponent<RectTransform>();
            rect.localScale = Vector3.one;
            rect.pivot = Vector2.up;
            rect.anchorMax = Vector2.up;
            rect.anchorMin = Vector2.up;
            rect.sizeDelta = new Vector2(width, height);
            rect.localScale = Vector3.one;
            rect.anchoredPosition3D = new Vector3(x, y, 0);
            return go;
        }

        protected GameObject CreateGameObject(Transform parent)
        {
            GameObject go = CreateGameObject(Name, Width, Height, X, -Y);
            go.transform.SetParent(parent, false);
            if(this.Hide)
            {
                go.SetActive(false);
            }
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

        public virtual void ProcessStruct(JsonData jsonData)
        {
        }

        public abstract void Build(Transform parent);
    }
}
