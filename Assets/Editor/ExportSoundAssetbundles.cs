 
using UnityEditor;
using UnityEngine;

/// <summary>
/// 打包音效
/// </summary>
class CreateSoundAssetbundles : ExportCommand
{
    private const string PRE_STRING = "sound";

    public override void Execute()
    {
        var soundPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (var prefab in soundPrefabs)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/"))
            {
                this.BuildSoundAssetbundle(path);
            }
        }
    }
     
    /// <summary>
    /// 生成声音资源包   声音 依附在AudioSource上做成Prefab后再打包
    /// </summary>
    /// <param name="prefabPath"></param>
    private void BuildSoundAssetbundle(string prefabPath)
    {
        Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);

        string path = AssetbundleDetailPath(AssetBundleResType.Sound) + PRE_STRING +"_"+ prefab.name + ".assetbundle";
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
