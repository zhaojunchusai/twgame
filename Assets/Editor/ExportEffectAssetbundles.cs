using UnityEditor;
using UnityEngine;
using System.IO;
/// <summary>
/// 打包特效
/// </summary>
class ExportEffectAssetbundles : ExportCommand
{
    private const string PRE_STRING = "effect";

    public override void Execute()
    {
        var effectPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (var prefab in effectPrefabs)
        {
            string path = AssetDatabase.GetAssetPath(prefab);
            if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/"))
            {
                this.BuildEffectAssetbundle(path);
            }
        }
    }

    /*
     * Name: 生成特效资源包
     * Desc: 
     * Date: 2015-04-20
     * add by taiwei
     * 
     * Function:    BuildEffectAssetbundle
     * Param:       prefabPath       [IN]      预制体路径
     * 
     * Return:      无
     */
    private void BuildEffectAssetbundle(string prefabPath)
    {
        Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);

        string path = AssetbundleDetailPath(AssetBundleResType.Effect) + PRE_STRING + "_" + prefab.name + ".assetbundle";
        bool success = BuildPipeline.BuildAssetBundle(prefab, null, path, BuildAssetBundleOptions.CollectDependencies, TargetPlatform);
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