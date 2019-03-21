using System;
using System.IO;
using System.Security.Cryptography;
using UnityEditor;
using UnityEngine;

class ExportCommand
{
    protected const string EXPORTRESPATH = "/exportres/";

    //初始化目录
    public static void InitAssetbundleDirectory()
    {
        if (!Directory.Exists(XmlPath))
        {
            Directory.CreateDirectory(XmlPath);
        }

        if (!Directory.Exists(AssetbundlePath))
        {
            Directory.CreateDirectory(AssetbundlePath);
        }

        //如果临时目录不存在，则在工程中创建一个临时目录给打包插件使用
        var tempFloder = AssetDatabase.LoadAssetAtPath("Assets/temp", typeof(UnityEngine.Object));
        if (tempFloder == null)
        {
            AssetDatabase.CreateFolder("Assets", "temp");
        }

    }

    public virtual void Execute()
    {
    }

    //目标平台
    public static BuildTarget TargetPlatform
    {
        get 
        {
            return EditorUserBuildSettings.activeBuildTarget;
        }
    }

    //生成选项
    public static BuildAssetBundleOptions BuildOption
    {
        get
        {
            return BuildAssetBundleOptions.CollectDependencies | BuildAssetBundleOptions.DeterministicAssetBundle;
        }
    }

    /*
     * Name: AssetbundlePath
     * Desc: 获取打包文件路径
     * Date: 21/04/2015
     * add by taiwei
     * Property: AssetbundlePath            
     * Return:   string                     返回打包路径
    */
    public static string AssetbundlePath
    {
        get
        {
            string savePath = string.Empty;
            switch (ExportCommand.TargetPlatform)
            {
                case BuildTarget.StandaloneWindows:
                    savePath = "Assetbundle/Windows";
                    break;
                case BuildTarget.iPhone:
                    savePath = "Assetbundle/IOS";
                    break;
                case BuildTarget.WP8Player:
                    savePath = "Assetbundle/WP8";
                    break;
                case BuildTarget.Android:
                    savePath = "Assetbundle/Android";
                    break;
                case BuildTarget.WebPlayer:
                    savePath = "Assetbundle/WebPlayer";
                    break;
                default:
                    savePath = "Assetbundle/Windows";
                    break;
            }
            savePath += Path.DirectorySeparatorChar;
            return savePath;
        }
    }

    public static string AssetbundleDetailPath(AssetBundleResType type)
    {
        string savePath = AssetbundlePath;
        switch (type) 
        {
            case AssetBundleResType.AloneRes: 
                {
                    savePath += "AloneRes/";
                }
                break;
            case AssetBundleResType.Atlas:
                {
                    savePath += "Atlas/";
                }
                break;
            case AssetBundleResType.Config:
                {
                    savePath += "Config/";
                }
                break;
            case AssetBundleResType.Effect:
                {
                    savePath += "Effect";
                }
                break;
            case AssetBundleResType.Font:
                {
                    savePath += "Font/";
                }
                break;
            case AssetBundleResType.Image:
                {
                    savePath += "Image/";
                }
                break;
            case AssetBundleResType.Role:
                {
                    savePath += "Role/";
                }
                break;
            case AssetBundleResType.Sound:
                {
                    savePath += "Sound/";
                }
                break;
            case AssetBundleResType.View:
                {
                    savePath += "View/";
                }
                break;
        }
        savePath += Path.DirectorySeparatorChar;
        if (Directory.Exists(savePath) == false)
            Directory.CreateDirectory(savePath);
        return savePath;
    }


    /*
     * Name: XmlPath
     * Desc: 获取打包文件信息路径
     * Date: 21/04/2015
     * Author：taiwei
     * Property: XmlPath            
     * Return:   string                     返回打包文件信息路径
    */
    public static string XmlPath
    {
        get { return "xml" + Path.DirectorySeparatorChar; }
    }


}

public enum AssetBundleResType 
{
    AloneRes = 0,
    Atlas = 1,
    Font = 2,
    Role = 3,
    Effect = 4,
    Image = 5,
    Sound = 6,
    View =7,
    Config = 8,
}
