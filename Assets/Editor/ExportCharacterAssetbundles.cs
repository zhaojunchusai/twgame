using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 打包角色
/// </summary>
class ExportCharacterAssetbundles : ExportCommand
{
    private const string PRE_STRING = "role";

    public override void Execute()
    {
        Object[] chaPrefabs = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        foreach (Object chaPrefab in chaPrefabs)
        {
            string path = AssetDatabase.GetAssetPath(chaPrefab);
            if (path.ToLower().Contains(EXPORTRESPATH+PRE_STRING+"/"))
            {
                BuildCharacterAssetbundle((Object)chaPrefab);
            }
        }
    }
    private void BuildCharacterAssetbundle(Object chaprefab)
    {
        GameObject cha = (GameObject)chaprefab;
        List<string> depBundleNames = new List<string>();
        
        //获取角色依赖资源
        string prefabPath = AssetDatabase.GetAssetPath(chaprefab.GetInstanceID());
        List<string> depAssetPaths = new List<string>(AssetDatabase.GetDependencies(new string[] { prefabPath })); 
        string t_chaBundlePath = AssetbundleDetailPath(AssetBundleResType.Role) + PRE_STRING +"_"+ chaprefab.name + ".assetbundle";
        if (BuildPipeline.BuildAssetBundle(chaprefab, null, t_chaBundlePath, BuildAssetBundleOptions.CollectDependencies, TargetPlatform))
        {
            Debug.Log(chaprefab.name + "资源打包成功");
        }
        else
        {
            Debug.Log(chaprefab.name + "资源打包失败");
        }
        AssetDatabase.Refresh();
        return;
        ////便利依赖资源，对共享资源打包
        ////Debug.LogWarning(depAssetPaths.Count + "]------------------------------------------------");
        //for (int i = 0; i < depAssetPaths.Count; i++)
        //{
        //    Object depAsset = AssetDatabase.LoadAssetAtPath(depAssetPaths[i], typeof(UnityEngine.Object));
        //    string depAssetPath = depAssetPaths[i];
        //    string bundlePath = string.Empty;
        //    string bundleName = string.Empty;
        //    //Debug.LogWarning(string.Format("[{0}, {1}]", i, depAssetPath));
        //    if (depAssetPath.ToLower().Contains(".asset"))
        //    {
        //        string str = depAsset.name;
        //        bundlePath = AssetbundlePath + "Asset_" + str + ".assetbundle";
        //        bundleName = "Asset_" + str + ".assetbundle";
        //    }
        //    else if (depAsset is Texture)
        //    {
        //        bundlePath = AssetbundlePath + "Atlas_" + depAsset.name + ".assetbundle";
        //        bundleName = "Atlas_" + depAsset.name + ".assetbundle";
        //    }
        //    if (!string.IsNullOrEmpty(bundlePath))
        //    {
        //            BuildPipeline.PushAssetDependencies();
        //            if (BuildPipeline.BuildAssetBundle(depAsset, null, bundlePath, BuildOption, TargetPlatform))
        //            {
        //                depBundleNames.Add(bundleName);
        //                UnityEngine.Debug.Log("Build " + bundlePath + " Success !");
        //            }
        //            else
        //            {
        //                UnityEngine.Debug.LogError("Build " + bundlePath + " Error !");
        //            }
        //    }
        //}

        ////建立依赖性描述资源
        //StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
        //holder.content = depBundleNames.ToArray();
        //AssetDatabase.CreateAsset(holder, "Assets/temp/DependentBundleNames.asset");
        //Object depBundleDescription = AssetDatabase.LoadAssetAtPath("Assets/temp/DependentBundleNames.asset", typeof(StringHolder));

        ////建立窗口主资源包, 并写入依赖资源包列表
        //string chaBundlePath = AssetbundlePath + PRE_STRING + chaprefab.name + ".assetbundle";
        //BuildPipeline.PushAssetDependencies();
        //  //建立窗口主资源包, 并写入依赖资源包列表
        //if (BuildPipeline.BuildAssetBundle(chaprefab, new UnityEngine.Object[] { depBundleDescription }, chaBundlePath, BuildAssetBundleOptions.CollectDependencies, TargetPlatform))
        //{
        //    UnityEngine.Debug.Log("Create Assetbundle " + chaBundlePath + " Suncess !");
        //}
        //else
        //{
        //    UnityEngine.Debug.LogError("Create Assetbundle " + chaBundlePath + " Failed !");
        //}
        //BuildPipeline.PopAssetDependencies();

        //for (int i = 0; i < depBundleNames.Count; i++)
        //{
        //    BuildPipeline.PopAssetDependencies();
        //}

        ////删除临时资源
        //AssetDatabase.DeleteAsset("Assets/temp/DependentBundleNames.asset");
        //AssetDatabase.Refresh();
    }
}
