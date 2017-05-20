using UnityEngine;

namespace AssetManager
{
    public class AssetLoader : MonoBehaviour
    {
        public static Sprite LoadSprite(string panelName, string spriteName)
        {
            return Resources.Load<GameObject>(string.Format("Sprite/{0}/{1}", panelName, spriteName)).GetComponent<SpriteRenderer>().sprite;
        }
    }
}
