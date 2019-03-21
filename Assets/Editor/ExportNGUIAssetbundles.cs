/*
 * Name: NGUI���ڴ��
 * File: CreateNGUIAssetbundles.cs
 * Desc: ��NGUI���ڼ�UIʹ�õ����塢ͼ���������Դ���д��
 *       �ص㣺����ٶȿ죬֧�ֶ�UI�����������
 * Date:  2015-04-20
 *  add by taiwei
 *  modify by taiwei  2016-08-25 15:03
*/
using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// ���NGUI���
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
    /// ���NGUI���ڼ���������
    /// Date: 2015-04-20
    /// add by taiwei
    ///  ��֧�ֶ�̬���� ͼƬ������ʱû��֧��
    /// </summary>
    /// <param name="windowprefab">Ŀ�괰��</param>
    private void BuildWindowAssetbundle(GameObject windowprefab)
    {
        List<string> depBundleNames = new List<string>();
        // ��ȡ����������Դ
        string prefabPath = AssetDatabase.GetAssetPath(windowprefab.GetInstanceID());
        List<string> depAssetPaths = new List<string>(AssetDatabase.GetDependencies(new string[] { prefabPath }));
        // this.SortPaths(ref depAssetPaths);
        List<string> atlas = new List<string>();
        //����������Դ���Թ�����Դ���
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
        /* ���Ƕ����һ��Push Pop��Ŀ���Ǳ�����Դ���� ÿ��ͼ���������  ����������ͼ��
         * ����������ʱ��Ϊ�˱����ظ���� ��ѹ�����н��潫��������Դ
         * ��������һ���������� ͼ��A��B��C�����ͼ����������ԴDʱ��A�����B��C�Ͳ��ٰ���D�����Դ�� ����ֱ������A��
         * ������н���������Ժ���Ҫ�ٴ��ͼ����  ����������ϵ  ��ͼ���໥���� ������������ϵ
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
        /*������� ÿ�����������������ͼ����Դ 
         *����������ÿ�����涼���������� ���Pushÿ���������Ժ��ִ��Pop
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
        /* ���´��������������ͼ����Դ   ʹ��ͼ�������໥���� 
         * ���ڽ�����������ͼ���� ���ֱ�Ӹ���ͼ�����ᶪʧ���߼��������ϵ
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