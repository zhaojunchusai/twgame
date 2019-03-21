/*
 * Name: NGUI窗口打包
 * File: CreateNGUIAssetbundles.cs
 * Desc: 将NGUI窗口及UI使用的字体、图集等相关资源进行打包
 *       特点：打包速度快，支持对UI窗口批量打包
 * Date:  2015-04-20
 *  add by taiwei
 *  modify by taiwei  2016-08-25 15:03
*/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 打包NGUI相关
/// </summary>
class CreateNGUIAssetbundles : ExportCommand
{
    private const string PRE_STRING = "view";
    private const string FONT_STRING = "font";
    private const string ATLAS_STRING = "atlas";

    private Dictionary<string, AssetTargetInfo> deepBundle_dic;
    private Dictionary<string, AssetTargetInfo> mainBundle_dic;

    public override void Execute()
    {
        //font_to_ab = true;
        deepBundle_dic = new Dictionary<string, AssetTargetInfo>();
        mainBundle_dic = new Dictionary<string, AssetTargetInfo>();
        var wndPrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (var wndPrefab in wndPrefabs)
        {
            string path = AssetDatabase.GetAssetPath(wndPrefab);
            if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/"))
            {
                this.BuildWindowAssetbundle((GameObject)wndPrefab);
            }
        }
        Export();
    }

    /// <summary>
    /// 打包NGUI窗口及其依赖项
    /// Date: 2015-04-20
    /// add by taiwei
    ///  仅支持动态字体 图片字体暂时没有支持
    /// </summary>
    /// <param name="windowprefab">目标窗口</param>
    private void BuildWindowAssetbundle(GameObject windowprefab)
    {
        List<string> depBundleNames = new List<string>();
        // 获取窗口依赖资源
        string prefabPath = AssetDatabase.GetAssetPath(windowprefab.GetInstanceID());
        List<string> depAssetPaths = new List<string>(AssetDatabase.GetDependencies(new string[] { prefabPath }));
        // this.SortPaths(ref depAssetPaths);
        List<string> atlas = new List<string>();
        //便利依赖资源，对共享资源打包
        AssetTargetInfo mainTargetInfo = new AssetTargetInfo();
        mainTargetInfo.depBundleNames = new List<string>();
        for (int i = 0; i < depAssetPaths.Count; i++)
        {
            Object depAsset = AssetDatabase.LoadAssetAtPath(depAssetPaths[i], typeof(UnityEngine.Object));
            string depAssetPath = depAssetPaths[i];
            string bundlePath = string.Empty;
            string bundleName = string.Empty;

            if (depAsset is Font)
            {
                string str = depAsset.name.StartsWith("Font_") ? depAsset.name.Remove(0, 5) : depAsset.name;
                bundleName = FONT_STRING + "_" + str + ".assetbundle";
                bundlePath = AssetbundleDetailPath(AssetBundleResType.View) + bundleName;
            }
            else if (depAsset is GameObject && ((GameObject)depAsset).GetComponent<UIFont>() != null)
            {
                string str = depAsset.name.StartsWith("Font_") ? depAsset.name.Remove(0, 5) : depAsset.name;
                bundleName = "nguifont_" + str + ".assetbundle";
                bundlePath = AssetbundleDetailPath(AssetBundleResType.View) + bundleName;
            }
            else if (depAsset is GameObject && ((GameObject)depAsset).GetComponent<UIAtlas>() != null)
            {
                bundleName = ATLAS_STRING + "_" + depAsset.name + ".assetbundle";
                bundlePath = AssetbundleDetailPath(AssetBundleResType.View) + bundleName;

            }

            if (!string.IsNullOrEmpty(bundlePath))
            {
                if (!deepBundle_dic.ContainsKey(bundleName))
                {
                    AssetTargetInfo info = new AssetTargetInfo();
                    info.assetObj = depAsset;
                    info.bundlePath = bundlePath;
                    info.depBundleNames = null;
                    deepBundle_dic.Add(bundleName, info);
                }
                mainTargetInfo.depBundleNames.Add(bundleName);

            }
        }
        string windowBundlePath = AssetbundleDetailPath(AssetBundleResType.View) + PRE_STRING + "_" + windowprefab.name + ".assetbundle";
        if (!mainBundle_dic.ContainsKey(windowBundlePath))
        {
            mainTargetInfo.assetObj = windowprefab;
            mainTargetInfo.bundlePath = windowBundlePath;
            mainBundle_dic.Add(windowprefab.name, mainTargetInfo);
        }
    }

    private void Export()
    {
        int pushCount = 0;
        AssetDatabase.Refresh();
        /* 这儿嵌套了一个Push Pop的目的是避免资源冗余 每个图集单独打包  界面依赖于图集
         * 打包多个界面时，为了避免重复打包 先压入所有界面将依赖的资源
         * 这样会有一个问题在于 图集A、B、C，多个图集均包含资源D时，A打包后，B、C就不再包含D这个资源了 而是直接依赖A的
         * 因而所有界面打包完成以后需要再打包图集等  更新依赖关系  各图集相互独立 不产生依赖关系
         */
        BuildPipeline.PushAssetDependencies();
        foreach (KeyValuePair<string, AssetTargetInfo> info2 in deepBundle_dic)
        {
            string prefabPath = AssetDatabase.GetAssetPath(info2.Value.assetObj.GetInstanceID());
            Object obj = AssetDatabase.LoadMainAssetAtPath(prefabPath);
            if (BuildPipeline.BuildAssetBundle(obj, null, info2.Value.bundlePath, BuildOption, TargetPlatform))
            {
                pushCount++;
                UnityEngine.Debug.Log("Build " + info2.Value.bundlePath + " Success !");
            }
            else
            {
                UnityEngine.Debug.LogError("Build " + info2.Value.bundlePath + " Error !");
            }
            AssetDatabase.Refresh();
        }
        /*打包界面 每个界面均依赖于上述图集资源 
         *但我们期望每个界面都不互相依赖 因而Push每个界面打包以后就执行Pop
         */
        foreach (KeyValuePair<string, AssetTargetInfo> info in mainBundle_dic)
        {
            StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
            holder.content = info.Value.depBundleNames.ToArray();
            string assetName = "Assets/temp/DependentBundleNames.asset";
            AssetDatabase.CreateAsset(holder, assetName);
            Object depBundleDescription = AssetDatabase.LoadAssetAtPath(assetName, typeof(StringHolder));
            BuildPipeline.PushAssetDependencies();
            if (BuildPipeline.BuildAssetBundle(info.Value.assetObj, new UnityEngine.Object[] { depBundleDescription }, info.Value.bundlePath, BuildOption, TargetPlatform))
            {
                UnityEngine.Debug.Log("Build " + info.Value.bundlePath + " Success !");
            }
            else
            {
                UnityEngine.Debug.LogError("Build " + info.Value.bundlePath + " Error !");
            }
            BuildPipeline.PopAssetDependencies();
            AssetDatabase.DeleteAsset(assetName);
            AssetDatabase.Refresh();
        }
        //for (int i = 0; i < pushCount; i++)
        //{
        BuildPipeline.PopAssetDependencies();
        //}
        AssetDatabase.Refresh();
        /* 重新打包被界面依赖的图集资源   使各图集不在相互依赖 
         * 由于界面是依赖于图集的 因而直接更新图集不会丢失两者间的依赖关系
         */
        foreach (KeyValuePair<string, AssetTargetInfo> info2 in deepBundle_dic)
        {
            string prefabPath = AssetDatabase.GetAssetPath(info2.Value.assetObj.GetInstanceID());
            Object obj = AssetDatabase.LoadMainAssetAtPath(prefabPath);
            if (BuildPipeline.BuildAssetBundle(obj, null, info2.Value.bundlePath, BuildOption, TargetPlatform))
            {
                UnityEngine.Debug.Log("Build " + info2.Value.bundlePath + " Success !");
            }
            else
            {
                UnityEngine.Debug.LogError("Build " + info2.Value.bundlePath + " Error !");
            }
        }
        AssetDatabase.Refresh();
    }

}


public class AssetTargetInfo
{
    public Object assetObj;

    public string bundlePath;

    public List<string> depBundleNames;
}