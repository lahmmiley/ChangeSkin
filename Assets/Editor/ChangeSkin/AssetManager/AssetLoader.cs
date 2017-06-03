using UnityEngine;

namespace AssetManager
{
    public class AssetLoader : MonoBehaviour
    {
        public static Sprite LoadSprite(string panelName, string spriteName)
        {
            try
            {
                return Resources.Load<GameObject>(string.Format("Sprite/{0}/{1}", panelName, spriteName)).GetComponent<SpriteRenderer>().sprite;
            }
            catch
            {
                Debug.LogError("找不到资源:" + string.Format("Sprite/{0}/{1}", panelName, spriteName));
                return null;
            }
        }
    }
}
