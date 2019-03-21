using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 资源类型
/// </summary>
public enum EResType
{
    ertatlas,
    ertfont,
    ertview,
    ertrole,
    erteffect,
    ertimage,
    ertsound,
    ertaloneRes
}

public class LoadManager : Singleton<LoadManager>
{

    private const string PREFIX_ATLAS = "Atlas";
    private const string PREFIX_FONT = "Font";
    private const string PREFIX_VIEW = "View";

    public Dictionary<string, object> Dic_Atlas = new Dictionary<string, object>();
    public Dictionary<string, object> Dic_Font = new Dictionary<string, object>();
    public Dictionary<string, object> Dic_View = new Dictionary<string, object>();


    /// <summary>
    /// 获取资源类型名
    /// </summary>
    /// <param name="vResType"></param>
    /// <returns></returns>
    private string GetResTypeName(EResType vResType)
    {
        string tmpFileName = vResType.ToString();
        tmpFileName.Replace("ert", "");
        return vResType.ToString();
    }

    /// <summary>
    /// 检测文件是否外部文件
    /// </summary>
    /// <param name="vFileName"></param>
    /// <returns></returns>
    private bool CheckFileIsAssetBundle(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return false;
        return true;
    }

    /// <summary>
    /// 预加载资源
    /// </summary>
    /// <param name="vBaseName"></param>
    public void AdvanceLoad(string vBaseName)
    {
        if (string.IsNullOrEmpty(vBaseName))
            return;
        AdvanceLoad_Atlas(string.Format("{0}{1}/", vBaseName, GetResTypeName(EResType.ertatlas)));
        AdvanceLoad_Font(string.Format("{0}{1}/", vBaseName, GetResTypeName(EResType.ertatlas)));
        AdvanceLoad_View(string.Format("{0}{1}/", vBaseName, GetResTypeName(EResType.ertatlas)));
    }
    /// <summary>
    /// 预加载图集
    /// </summary>
    /// <param name="vFileName"></param>
    private void AdvanceLoad_Atlas(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return;
        if (!File.Exists(vFileName))
            return;
        //读取文件夹中子文件//
        WWW tmpWWW = new WWW(vFileName);
        if (tmpWWW == null)
            return;
        //添加到字典//
    }
    /// <summary>
    /// 预加载字体
    /// </summary>
    /// <param name="vFileName"></param>
    private void AdvanceLoad_Font(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return;
        if (!File.Exists(vFileName))
            return;
    }
    /// <summary>
    /// 预加载界面
    /// </summary>
    /// <param name="vFileName"></param>
    private void AdvanceLoad_View(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return;
        if (!File.Exists(vFileName))
            return;
    }

    /// <summary>
    /// 读取资源
    /// </summary>
    /// <param name="vFileName"></param>
    /// <returns></returns>
    public GameObject LoadSingleRes(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return null;
        //检测文件存放位置//
        if (CheckFileIsAssetBundle(vFileName))
        {//外部文件//
            return LoadSingleResByAssetBundle(vFileName);
        }
        else
        {//内部文件//
            return LoadSingleResByObject(vFileName);
        }

        return null;
    }

    /// <summary>
    /// 读取单个外部资源
    /// </summary>
    /// <param name="vFileName"></param>
    /// <returns></returns>
    private GameObject LoadSingleResByAssetBundle(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return null;
        return null;
    }

    /// <summary>
    /// 读取单个内部资源
    /// </summary>
    /// <param name="vFileName"></param>
    /// <returns></returns>
    private GameObject LoadSingleResByObject(string vFileName)
    {
        if (string.IsNullOrEmpty(vFileName))
            return null;
        return null;
    }
}
