using UnityEngine;
using System.Collections;
using UnityEditor;

namespace Tool
{
    public class AssetBundleBuilder : MonoBehaviour
    {
        [MenuItem("MyMenu/Build AssetBundle")]
        static private void BuildAssetBundle()
        {
            //AssetBundleBuild assetBundle = new AssetBundleBuild();
            //assetBundle.assetNames = new string[] { "Assets/Resources/Image/b.png" };
            //assetBundle.assetBundleName = "gogo.assetbundle";

            //BuildPipeline.BuildAssetBundles("Assets/AssetBundles", new AssetBundleBuild[1] { assetBundle }, BuildAssetBundleOptions.ForceRebuildAssetBundle, BuildTarget.StandaloneWindows64);
        }
    }
}