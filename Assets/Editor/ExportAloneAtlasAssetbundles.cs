using UnityEditor;
using UnityEngine;

/// <summary>
/// 打包独立资源-场景
/// </summary>
class ExportAloneAtlasAssetbundles : ExportCommand
{
    private const string PRE_STRING = "atlas";
    private const string ASSETPATH = "/assetres/";
    public override void Execute()
    {
        Object[] alonePrefabs = Selection.GetFiltered(typeof(GameObject), SelectionMode.DeepAssets);
        foreach (Object prefab in alonePrefabs)
        {
            GameObject go = prefab as GameObject;
            if (go == null)
                continue;
            UIAtlas uiAtlas = go.GetComponent<UIAtlas>();
            if (uiAtlas == null)
                continue;
            //string path = AssetDatabase.GetAssetPath(prefab);

            //if (path.ToLower().Contains(EXPORTRESPATH + PRE_STRING + "/") || path.ToLower().Contains(ASSETPATH + PRE_STRING + "/"))
            //{
            this.BuildAloneAssetbundle(prefab);
            //}
        }
    }

    private void BuildAloneAssetbundle(Object prefab)
    {
        //Object prefab = AssetDatabase.LoadMainAssetAtPath(prefabPath);
        //System.Collections.Generic.List<string> depAssetPaths = new System.Collections.Generic.List<string>(AssetDatabase.GetDependencies(new string[] { prefabPath }));
        string path = AssetbundleDetailPath(AssetBundleResType.Atlas) + PRE_STRING + "_" + prefab.name + ".assetbundle";
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
