using UnityEditor;
using UnityEngine;

/// <summary>
/// 打包独立资源-场景
/// </summary>
class ExportConfigAssetbundles : ExportCommand
{
    private const string PRE_STRING = "config";

    public override void Execute()
    {
        Object[] alonePrefabs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object prefab in alonePrefabs)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/"))
            {
                this.BuildAloneAssetbundle(path);
            }
        }
    }

    private void BuildAloneAssetbundle(string prefabPath)
    {
        Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);

        string path = AssetbundleDetailPath(AssetBundleResType.Config) + PRE_STRING + "_" + prefab.name + ".assetbundle";
        bool success = BuildPipeline.BuildAssetBundle(prefab, null, path, BuildOption, TargetPlatform);
        if (success)
        {
            UnityEngine.Debug.Log("Export " + path + " Success !");
        }
        else
        {
            UnityEngine.Debug.Log("Export " + path + " Error !");
        }

    }
}
