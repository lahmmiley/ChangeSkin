using UnityEngine;
using UnityEngine.EventSystems;

namespace UIComponent
{
    public class LContainer : UIBehaviour
    {
        protected T AddChildComponent<T>(string path) where T : UIBehaviour
        {
            Transform childTransform = transform.FindChild(path);
            if(childTransform == null)
            {
                Debug.LogWarning("Can't not find path:" + path);
                return null;
            }
            return childTransform.gameObject.AddComponent<T>();
        }

        protected T GetChildComponent<T>(string path) where T : UIBehaviour
        {
            Transform childTransform = transform.FindChild(path);
            if(childTransform == null)
            {
                Debug.LogWarning("Can't not find path:" + path);
                return null;
            }
            return childTransform.GetComponent<T>();
        }
    }
}
