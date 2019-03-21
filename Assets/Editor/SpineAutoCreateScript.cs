using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Spine;

public class SpineAutoCreateScript {

    private const string _screeningFile = ".meta";
    private const string _screeningDir = ".svn";
    private const string _initAtlas = ".atlas";
    private const string _EndAtlas = ".atlas.txt";
    private const string _initJson = ".json";
    private const string _EndJson = ".json.txt";
    private const string _FinalAsset = ".asset";


    /// <summary>
    /// 修改原始Spine文件名
    /// </summary>
    [MenuItem("Assets/SpineAutoOperate/Change Init FileName")]
    static void ChangeInitFileName()
    {
        SingleDirOperate_ChangeFilesName(SetRootDirPath());
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        Debug.Log("-------------ChangeInitFileName Success-------------");
    }
    /// <summary>
    /// 创建文件
    /// </summary>
    [MenuItem("Assets/SpineAutoOperate/Crate New Files")]
    static void CreateNewFiles()
    {
        SingleDirOperate_CreateNewFiles(SetRootDirPath());
        Debug.Log("-------------CreateNewFiles Success-------------");
    }

    /// <summary>
    /// 指定文件夹修改文件名
    /// </summary>
    /// <param name="vDirPath"></param>
    static void SingleDirOperate_ChangeFilesName(string vDirPath)
    {
        //判断文件夹路径是否正确//
        if (string.IsNullOrEmpty(vDirPath))
            return;
        if (!Directory.Exists(vDirPath))
            return;
        if (vDirPath.Contains(_screeningDir))
            return;

        //文件操作//
        string[] tmpFileArray = Directory.GetFiles(vDirPath);
        if ((tmpFileArray != null) && (tmpFileArray.Length > 0))
        {
            //更换对应文件名字//
            for (int i = 0; i < tmpFileArray.Length; i++)
            {
                //.meta不用操作//
                if (tmpFileArray[i].Contains(_screeningFile))
                    continue;
                //修改.atlas文件//
                if ((tmpFileArray[i].Contains(_initAtlas)) && (!tmpFileArray[i].Contains(_EndAtlas)))
                {
                    File.Move(tmpFileArray[i], tmpFileArray[i].Replace(_initAtlas, _EndAtlas));
                    continue;
                }
                //修改.json文件//
                if ((tmpFileArray[i].Contains(_initJson)) && (!tmpFileArray[i].Contains(_EndJson)))
                {
                    File.Move(tmpFileArray[i], tmpFileArray[i].Replace(_initJson, _EndJson));
                    continue;
                }
            }
        }

        //子文件夹操作//
        string[] tmpDirArray = Directory.GetDirectories(vDirPath);
        if (tmpDirArray == null)
            return;
        if (tmpDirArray.Length <= 0)
            return;
        for (int i = 0; i < tmpDirArray.Length; i++)
            SingleDirOperate_ChangeFilesName(tmpDirArray[i]);
    }

    /// <summary>
    /// 指定文件夹创建文件
    /// </summary>
    /// <param name="vDirPath"></param>
    static void SingleDirOperate_CreateNewFiles(string vDirPath)
    {
        vDirPath = GetReplaceStr(vDirPath);
        if (string.IsNullOrEmpty(vDirPath))
            return;
        if (!Directory.Exists(vDirPath))
            return;
        if (vDirPath.Contains(_screeningDir))
            return;

        //文件操作//
        CreateSingleSpine(vDirPath);

        //子文件夹操作//
        string[] tmpDirArray = Directory.GetDirectories(vDirPath);
        if (tmpDirArray == null)
            return;
        if (tmpDirArray.Length <= 0)
            return;
        for (int i = 0; i < tmpDirArray.Length; i++)
            SingleDirOperate_CreateNewFiles(tmpDirArray[i]);
    }

    /// <summary>
    /// 获取选中目录
    /// </summary>
    /// <returns></returns>
    static string SetRootDirPath()
    {
        string tmpResult = "Assets/";
        var tmpSelected = Selection.activeObject;
        if (tmpSelected != null)
        {
            string assetDir = AssetDatabase.GetAssetPath(tmpSelected.GetInstanceID());
            if (assetDir.Length > 0 && Directory.Exists(assetDir))
                tmpResult = assetDir + "/";
        }
        return tmpResult;
    }

    /// <summary>
    /// 创建文件
    /// </summary>
    /// <param name="vSourcePath">目标文件夹路径</param>
    static void CreateSingleSpine(string vSourcePath)
    {
        if (string.IsNullOrEmpty(vSourcePath))
            return;
        if (!Directory.Exists(vSourcePath))
            return;
        string[] tmpFileArray = Directory.GetFiles(vSourcePath);
        if (tmpFileArray == null)
            return;
        
        string tmpFileName = vSourcePath.Substring(vSourcePath.LastIndexOf("/") + 1);
        if (string.IsNullOrEmpty(tmpFileName))
        {
            tmpFileName = vSourcePath.Substring(0, vSourcePath.Length - 1);
            tmpFileName = tmpFileName.Substring(tmpFileName.LastIndexOf("/") + 1);
        }
        string jsonFileName = string.Empty;
        string atlasFileName = string.Empty;
        //string textureName = string.Empty;

        for (int i = 0; i < tmpFileArray.Length; i++)
        {
            tmpFileArray[i] = tmpFileArray[i].Replace("\\", "/");
            if (tmpFileArray[i].Contains(_screeningFile))
                continue;
            if (tmpFileArray[i].Contains(_EndJson))
            {
                jsonFileName = tmpFileArray[i];
                continue;
            }
            if (tmpFileArray[i].Contains(_FinalAsset))
            {
                atlasFileName = tmpFileArray[i];
                continue;
            }
            //if (tmpFileArray[i].Contains(_EndAtlas))
            //{
            //    atlasFileName = tmpFileArray[i];
            //    continue;
            //}
            //if (tmpFileArray[i].Contains(".png"))
            //{
            //    textureName = tmpFileArray[i];
            //    continue;
            //}
        }
        if (string.IsNullOrEmpty(jsonFileName) || 
            string.IsNullOrEmpty(atlasFileName))// || 
            //string.IsNullOrEmpty(textureName))
        {
            return;
        }



        //Material mat;
        /////1、 创建材质，并指贴图和shader
        //{
        //    Shader shader = Shader.Find("Spine/Bones");
        //    mat = new Material(shader);
        //    Texture tex = Resources.LoadAssetAtPath(textureName, typeof(Texture)) as Texture;
        //    mat.SetTexture("_MainTex", tex);
        //    AssetDatabase.CreateAsset(mat, vSourcePath + tmpFileName + ".mat");
        //    AssetDatabase.SaveAssets();
        //    AssetDatabase.Refresh();
        //}

        /////2、 创建atlas，并指xx
        //AtlasAsset m_AtlasAsset = AtlasAsset.CreateInstance<AtlasAsset>();
        //AssetDatabase.CreateAsset(m_AtlasAsset, vSourcePath + tmpFileName + ".asset");
        //Selection.activeObject = m_AtlasAsset;
        //EditorUtility.SetDirty(m_AtlasAsset);

        //TextAsset textAsset = Resources.LoadAssetAtPath(atlasFileName, typeof(TextAsset)) as TextAsset;
        //m_AtlasAsset.atlasFile = textAsset;
        //m_AtlasAsset.materials = new Material[1];
        //m_AtlasAsset.materials[0] = mat;
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

        /////3、 创建SkeletonDataAsset，并指相关
        //SkeletonDataAsset m_skeltonDataAsset = SkeletonDataAsset.CreateInstance<SkeletonDataAsset>();
        //AssetDatabase.CreateAsset(m_skeltonDataAsset, vSourcePath + tmpFileName + "_Animation.asset");
        //Selection.activeObject = m_skeltonDataAsset;

        //m_skeltonDataAsset.atlasAssets = new AtlasAsset[1];
        //m_skeltonDataAsset.atlasAssets[0] = m_AtlasAsset;
        //TextAsset m_jsonAsset = Resources.LoadAssetAtPath(jsonFileName, typeof(TextAsset)) as TextAsset;
        //m_skeltonDataAsset.skeletonJSON = m_jsonAsset;
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();

        //m_skeltonDataAsset.fromAnimation = new string[0];
        //m_skeltonDataAsset.toAnimation = new string[0];
        //m_skeltonDataAsset.duration = new float[0];
        //m_skeltonDataAsset.defaultMix = 0.2f;
        //m_skeltonDataAsset.scale = 1;
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();
        //EditorUtility.SetDirty(m_skeltonDataAsset);

        ///// 创建场景物件
        //GameObject gameObject = new GameObject(tmpFileName, typeof(SkeletonAnimation));
        //EditorUtility.FocusProjectWindow();
        //Selection.activeObject = gameObject;

        //SkeletonAnimation m_skelAnim = gameObject.GetComponent<SkeletonAnimation>();
        //m_skelAnim.skeletonDataAsset = m_skeltonDataAsset;
        //AssetDatabase.SaveAssets();
        //AssetDatabase.Refresh();



        //创建SkeletonDataAsset并设置相关参数
        SkeletonDataAsset m_skeltonDataAsset = CreateSkeletonDataAsset(vSourcePath, tmpFileName);
        string tmpAtlasPath = string.Empty;
        if (vSourcePath.Substring(vSourcePath.Length - 2, 1).Equals("/"))
            tmpAtlasPath = vSourcePath.Substring(0, vSourcePath.Length - 1);
        else
            tmpAtlasPath = vSourcePath;
        List<AtlasAsset> tmpAtlasAssetList = SpineEditorUtilities.FindAtlasesAtPath(tmpAtlasPath);
        AtlasAsset m_AtlasAsset = (AtlasAsset)Resources.LoadAssetAtPath(atlasFileName, typeof(AtlasAsset));
        m_skeltonDataAsset.atlasAssets = new AtlasAsset[tmpAtlasAssetList.Count];
        for (int i = 0; i < m_skeltonDataAsset.atlasAssets.Length; i++)
        {
            m_skeltonDataAsset.atlasAssets[i] = tmpAtlasAssetList[i];
        }

        TextAsset m_jsonAsset = Resources.LoadAssetAtPath(jsonFileName, typeof(TextAsset)) as TextAsset;
        m_skeltonDataAsset.skeletonJSON = m_jsonAsset;
        m_skeltonDataAsset.fromAnimation = new string[0];
        m_skeltonDataAsset.toAnimation = new string[0];
        m_skeltonDataAsset.duration = new float[0];
        m_skeltonDataAsset.defaultMix = 0.2f;
        m_skeltonDataAsset.scale = 1;
        //修改材质着色器//
        Shader tmpShader = Shader.Find("Spine/Bones");
        for (int i = 0; i < m_skeltonDataAsset.atlasAssets.Length; i++)
        {
            for (int j = 0; j < m_skeltonDataAsset.atlasAssets[i].materials.Length; j++)
            {
                m_skeltonDataAsset.atlasAssets[i].materials[j].shader = tmpShader;
            }
        }
        AssetDatabase.SaveAssets();
        EditorUtility.SetDirty(m_skeltonDataAsset);

        /// 创建场景物件
        GameObject gameObject = new GameObject(tmpFileName, typeof(SkeletonAnimation));
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = gameObject;

        SkeletonAnimation m_skelAnim = gameObject.GetComponent<SkeletonAnimation>();
        m_skelAnim.skeletonDataAsset = m_skeltonDataAsset;
        string tmpPath = "Assets/ExportRes/Role/";
        if (!Directory.Exists(tmpPath)) 
        {
            Directory.CreateDirectory(tmpPath);
        }
        UnityEngine.Object tmpPrefab = PrefabUtility.CreatePrefab(tmpPath + gameObject.name + ".prefab", gameObject);
        
        Debug.Log(string.Format("-------------{0} Success-------------", vSourcePath));
    }

    /// <summary>
    /// 创建SkeletonDataAsset
    /// </summary>
    /// <param name="vPath">文件夹路径</param>
    /// <param name="vName">文件名</param>
    /// <returns></returns>
    static SkeletonDataAsset CreateSkeletonDataAsset(string vPath, string vName)
    {
        string tmpPath = GetReplaceStr(string.Format("{0}/{1}{2}", vPath, vName, _FinalAsset));
        ScriptableObject asset = ScriptableObject.CreateInstance<SkeletonDataAsset>();
        AssetDatabase.CreateAsset(asset, tmpPath);
        AssetDatabase.SaveAssets();
        EditorUtility.FocusProjectWindow();
        Selection.activeObject = asset;
        return (SkeletonDataAsset)asset;
    }

    /// <summary>
    /// 转换路径中的非法字符
    /// </summary>
    /// <param name="vPath">源路径</param>
    /// <returns></returns>
    static string GetReplaceStr(string vPath)
    {
        if (string.IsNullOrEmpty(vPath))
            return string.Empty;
        return vPath.Replace("\\", "/").Replace("//", "/");
    }
}
