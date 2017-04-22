#define LAHM_DEBUG
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AssetManager
{
    public class AssetLoader : MonoBehaviour
    {
        //LoadIcon 加载图集的图标

        private static Dictionary<string, Sprite> _dict = new Dictionary<string, Sprite>();

        private void Awake()
        {
            StartCoroutine(LoadAssetBundle("Common"));
            StartCoroutine(LoadAssetBundle("BattlePreparePanel"));
        }

        //LoadSprite 加载面板上的固定资源
        private static char[] _splitChar = new char[] { '.' };
        public static Sprite LoadSprite(string panelName, string spriteName)
        {
#if LAHM_DEBUG
            return Resources.Load<GameObject>(string.Format("Sprite/{0}/{1}", panelName, spriteName)).GetComponent<SpriteRenderer>().sprite;
#elif LAHM_RELEASE
            return _dict[spriteName];
#endif
            return null;
            //return StartCoroutine(LoadAssetBundle(panelName, spriteName));
        }

        private static IEnumerator LoadAssetBundle(string panelName)
        {
            string assetBundleDir = "file://" + Application.dataPath + "/AssetBundles/";
            string assetBundlePath = assetBundleDir + panelName;

            WWW www = WWW.LoadFromCacheOrDownload(assetBundlePath, 0);
            yield return www;
            AssetBundle asssetBundle = www.assetBundle;

            #if UNITY_5_3
            UnityEngine.Object[] list = asssetBundle.LoadAllAssets();
            for(int i = 0; i < list.Length; i++)
            {
                //Debug.LogError("name:" + list[i].name);
                _dict.Add(list[i].name, list[i] as Sprite);
            }
            if(panelName == "BattlePreparePanel")
            {
                //PanelCreator.Instance.Create("BattlePreparePanel");
            }
            #endif
        }
    }
}
