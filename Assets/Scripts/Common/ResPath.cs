using UnityEngine;
using System.Collections;
using System.IO;

public enum ResourceLoadPathType
{
    None,
    /// <summary>
    /// DLC
    /// </summary>
    PersistentDataPath,
    StreamingAssets,
    Resource,
}

public class ResPath
{
    //public const string CHECKVERSIONADDRESS = "http://61.219.16.52:8080/";   //私服资源服
    public static string CHECKVERSIONADDRESS = "http://192.168.0.252:81/";   //私服资源服
    public const string APKVERSIONXML = "Config/ApkVersion";
    public const string CHECKVERSION_ASSETNAME = "Version.xml";
    public const string RESPATHXML = "ResourcesList.xml";
    public static string ANDROIDPATH = "TD/Assetbundle/Android/";     //测试资源路径
    public static string IPHONEPATH = "TD/Assetbundle/Iphone/";       //测试资源路径
    public static string OTHERPATH = "TD/Assetbundle/Editor/";        //测试资源路径
    public static string OSXEDITORPATH = "TD/Assetbundle/IOSEditor/";        //测试资源路径
    public const string DIR_ATLAS = "Atlas/";
    public const string DIR_SOUND = "Sound/";
    public const string DIR_ALONERES = "AloneRes/";
    public const string DIR_EFFECT = "Effect/";
    public const string DIR_IMAGE = "Image/";
    public const string DIR_ROLE = "Role/";
    public const string DIR_VIEW = "View/";
    public const string DIR_CONFIG = "Config/";
    public const string DIR_LOACAL_VERSIONXML = "Version/";
    public const string DLLPATH = "/mnt/sdcard/com.pixone.LTD/files/Assembly-CSharp.dll";
    public const string APKPATH = "/mnt/sdcard/com.pixone.LTD/files";

    public const string PREFIX_VIEW = "view_";
    public const string PREFIX_SOUND = "sound_";
    public const string PREFIX_ATLAS = "atlas_";
    public const string PREFIX_ROLE = "role_";
    public const string PREFIX_CONFIG = "config_";
    public const string PREFIX_ALONERES = "aloneres_";
    public const string PREFIX_IMAGE = "image_";
    public const string PREFIX_EFFECT = "effect_";
    public const string PREFIX_FONT = "font_";
    public const string PREFIX_NGUIFONT = "nguifont_";
    public const string FILESUFFIX = ".assetbundle";


    public static string WLOCALPATH
    {
        get
        {
            string path = Application.persistentDataPath + "/";
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    {
                        path += IPHONEPATH;
                    } break;
                case RuntimePlatform.Android:
                    {
                        path += ANDROIDPATH;
                    } break;
                case RuntimePlatform.OSXEditor:
                    {
                        path += OSXEDITORPATH;
                    } break;
                case RuntimePlatform.WindowsEditor:
                default:
                    {
                        path += OTHERPATH;
                    } break;
            }
            return path;
        }
    }

    /// <summary>
    /// DLC
    /// </summary>
    public static string DOWNLOADPATH
    {
        get
        {
            //switch (Application.platform)
            //{

            string path = CHECKVERSIONADDRESS;
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer:
                    {
                        path += IPHONEPATH;
                    } break;
                case RuntimePlatform.Android:
                    {
                        path += ANDROIDPATH;
                    } break;
                case RuntimePlatform.OSXEditor:
                    {
                        path += OSXEDITORPATH;
                    } break;
                case RuntimePlatform.WindowsEditor:
                default:
                    {
                        path += OTHERPATH;
                    } break;
            }
            return path;

            //case RuntimePlatform.IPhonePlayer: return CHECKVERSIONADDRESS + IPHONEPATH;
            //case RuntimePlatform.Android: return CHECKVERSIONADDRESS + ANDROIDPATH;
            //case RuntimePlatform.WindowsEditor: return CHECKVERSIONADDRESS + OTHERPATH;
            //case RuntimePlatform.OSXEditor: return CHECKVERSIONADDRESS + OSXEDITORPATH;
            //default: return CHECKVERSIONADDRESS + OTHERPATH;
            //}
        }
    }

    public static string CHECHVERSIONXMLPATH
    {
        get
        {
            return DIR_LOACAL_VERSIONXML;
        }
    }

    /// <summary>
    /// 本地资源
    /// </summary>
    public static string WPRELOCALPATH
    {
        get
        {
            switch (Application.platform)
            {
                case RuntimePlatform.IPhonePlayer: return Application.streamingAssetsPath + "/" + IPHONEPATH;
                case RuntimePlatform.Android: return Application.streamingAssetsPath + "/" + ANDROIDPATH;
                case RuntimePlatform.WindowsEditor: return Application.streamingAssetsPath + "/" + OTHERPATH;
                case RuntimePlatform.OSXEditor: return Application.streamingAssetsPath + "/" + OSXEDITORPATH;
                default: return Application.streamingAssetsPath + "/" + OTHERPATH;
            }
        }
    }

    public static ResourceLoadPathType AssetPath(string fileName, string type, out string assetPath)
    {
        assetPath = string.Empty;
        System.Text.StringBuilder subPath = new System.Text.StringBuilder();
        System.Text.StringBuilder sub_res = new System.Text.StringBuilder();
        fileName = ReplaceFileName(fileName, true);
        switch (type)
        {
            case ResourceType.AloneAtlas:
                subPath.Append(DIR_VIEW);
                sub_res.Append(DIR_VIEW);
                break;
            case ResourceType.UIWindow:
                subPath.Append(DIR_VIEW);
                sub_res.Append(DIR_VIEW);
                break;
            case ResourceType.AloneImage:
                subPath.Append(DIR_IMAGE);
                sub_res.Append(DIR_IMAGE);
                break;
            case ResourceType.Character:
                subPath.Append(DIR_ROLE);
                sub_res.Append(DIR_ROLE);
                break;
            case ResourceType.Effect:
                subPath.Append(DIR_EFFECT);
                sub_res.Append(DIR_EFFECT);
                break;
            case ResourceType.Audio:
                subPath.Append(DIR_SOUND);
                sub_res.Append(DIR_SOUND);
                break;
            case ResourceType.Alone:
                subPath.Append(DIR_ALONERES);
                sub_res.Append(DIR_ALONERES);
                break;
            case ResourceType.Config:
                subPath.Append(DIR_CONFIG);
                sub_res.Append(DIR_CONFIG);
                break;
        }
        subPath.Append(fileName);
        if (GlobalConst.OPENDLC)
        {
            System.Text.StringBuilder sub_dlc = new System.Text.StringBuilder();
            sub_dlc.Append(WLOCALPATH);
            sub_dlc.Append(subPath);
            assetPath = sub_dlc.ToString();
            if (File.Exists(assetPath))
            {
                return ResourceLoadPathType.PersistentDataPath;
            }
        }
        System.Text.StringBuilder sub_local = new System.Text.StringBuilder();
        if (Application.platform == RuntimePlatform.Android)
        {
#if UNITY_ANDROID
            assetPath = ANDROIDPATH + subPath.ToString();
            if (CallAndroidFunManager.Instance.isFileExists(assetPath))
            {
                return ResourceLoadPathType.StreamingAssets;
            }
#endif
        }
        else
        {
            sub_local.Append(WPRELOCALPATH);
            sub_local.Append(subPath);
            assetPath = sub_local.ToString();
            if (File.Exists(assetPath))
            {
                return ResourceLoadPathType.StreamingAssets;
            }
        }
        string file = ReplaceFileName(fileName, false, true);
        sub_res.Append(file);
        assetPath = sub_res.ToString();
        return ResourceLoadPathType.Resource;
    }

    public static string GetResTypeByABName(string fileName)
    {
        string resType = string.Empty;
        if (fileName.Contains(FILESUFFIX))
        {
            Debug.LogError("param error! file name is must be assetbundle");
            return resType;
        }
        if (fileName.Contains(PREFIX_VIEW))
        {
            resType = ResourceType.UIWindow;
        }
        else if (fileName.Contains(PREFIX_SOUND))
        {
            resType = ResourceType.Audio;
        }
        else if (fileName.Contains(PREFIX_ROLE))
        {
            resType = ResourceType.Character;
        }
        else if (fileName.Contains(PREFIX_IMAGE))
        {
            resType = ResourceType.AloneImage;
        }
        else if (fileName.Contains(PREFIX_FONT))
        {
            resType = ResourceType.Font;
        }
        else if (fileName.Contains(PREFIX_NGUIFONT))
        {
            resType = ResourceType.NGUIFont;
        }
        else if (fileName.Contains(PREFIX_EFFECT))
        {
            resType = ResourceType.Effect;
        }
        else if (fileName.Contains(PREFIX_CONFIG))
        {
            resType = ResourceType.Config;
        }
        else if (fileName.Contains(PREFIX_ATLAS))
        {
            resType = ResourceType.AloneAtlas;
        }
        else if (fileName.Contains(PREFIX_ALONERES))
        {
            resType = ResourceType.Alone;
        }
        return resType;
    }

    public static string ReplaceFileName(string fileName, bool isAdd, bool clearPREFIX = false)
    {
        string name = fileName;
        if (isAdd)
        {
            if (!name.Contains(FILESUFFIX))
            {
                name = name + FILESUFFIX;
            }
        }
        else
        {
            if (clearPREFIX)
            {
                if (fileName.Contains(PREFIX_VIEW))
                {
                    name = fileName.Replace(PREFIX_VIEW, "");
                }
                else if (fileName.Contains(PREFIX_SOUND))
                {
                    name = fileName.Replace(PREFIX_SOUND, "");
                }
                else if (fileName.Contains(PREFIX_ROLE))
                {
                    name = fileName.Replace(PREFIX_ROLE, "");
                }
                else if (fileName.Contains(PREFIX_IMAGE))
                {
                    name = fileName.Replace(PREFIX_IMAGE, "");
                }
                else if (fileName.Contains(PREFIX_FONT))
                {
                    name = fileName.Replace(PREFIX_FONT, "");
                }
                else if (fileName.Contains(PREFIX_NGUIFONT))
                {
                    name = fileName.Replace(PREFIX_NGUIFONT, "");
                }
                else if (fileName.Contains(PREFIX_EFFECT))
                {
                    name = fileName.Replace(PREFIX_EFFECT, "");
                }
                else if (fileName.Contains(PREFIX_CONFIG))
                {
                    name = fileName.Replace(PREFIX_CONFIG, "");
                }
                else if (fileName.Contains(PREFIX_ATLAS))
                {
                    name = fileName.Replace(PREFIX_ATLAS, "");
                }
                else if (fileName.Contains(PREFIX_ALONERES))
                {
                    name = fileName.Replace(PREFIX_ALONERES, "");
                }
            }

            if (name.Contains(FILESUFFIX))
            {
                name = name.Replace(FILESUFFIX, "");
            }
        }
        return name;
    }


    public static string GetViewPath(bool local = true)
    {
        string path = string.Empty;
        if (local)
        {
            return "Prefabs/UI/";
        }
        return path;
    }
}
