using UnityEngine;
using System;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
public static class CommonTools
{
    public static string projectPath = Application.dataPath.Split(new string[] { "Assets" }, StringSplitOptions.None)[0];
    public static string buildPath = projectPath + "Builds/Android/";
    public static string cfgPath = projectPath + "SDK/SDKConfig.xml";

    public static void LoadBuildXml(ref Dictionary<string, BuildConfig> dic)
    {
        XmlDocument xmlDoc = new XmlDocument();
        if (!File.Exists(cfgPath))
        {
            return;
        }
        xmlDoc.Load(cfgPath);
        XmlNodeList nodeList = xmlDoc.SelectSingleNode("Config").ChildNodes;
        foreach (XmlNode node in nodeList)
        {
            if (node.Name != "Child") continue;
            BuildConfig config = new BuildConfig();
            config.configName = AnalyzeXmlToString(node, "configName");
            config.resAddress = AnalyzeXmlToString(node, "resAddress");
            config.loginAddress = AnalyzeXmlToString(node, "loginAddress");
            config.loginServerType = AnalyzeXmlToInt(node, "loginServerType");

            config.isOpenCodeUpdate = AnalyzeXmlToBool(node, "openCodeUpdate");
            config.isOpenMD5 = AnalyzeXmlToBool(node, "openMD5");
            config.isOpenDLC = AnalyzeXmlToBool(node, "openDLC");
            config.isOpenGuide = AnalyzeXmlToBool(node, "openGuide");
            config.isOpenGM = AnalyzeXmlToBool(node, "openGM");
            config.isOpenSDK = AnalyzeXmlToBool(node, "openSDK");
            config.isTDTest = AnalyzeXmlToBool(node, "isTDTest");
            config.isReplaceRes = AnalyzeXmlToBool(node, "ReplaceRes");

            config.versionName = AnalyzeXmlToString(node, "versionName");

            config.resAndroidPath = AnalyzeXmlToString(node, "resAndroidPath");
            config.resIOSPath = AnalyzeXmlToString(node, "resIOSPath");
            config.resMacEditorPath = AnalyzeXmlToString(node, "resMacEditorPath");
            config.resWinEditorPath = AnalyzeXmlToString(node, "resWinEditorPath");
            config.apkVersion = AnalyzeXmlToInt(node, "apkVersion");
            config.resPath = AnalyzeXmlToString(node, "resPath");
            config.channelType = AnalyzeXmlToInt(node, "channelType");

            /**********************渠道相关*************************/
            config.iconPath = AnalyzeXmlToString(node, "iconPath");
            config.fileName = AnalyzeXmlToString(node, "fileName");
            config.appName = AnalyzeXmlToString(node, "appName");
            config.companyName = AnalyzeXmlToString(node, "companyName");
            config.androidSDKPath = AnalyzeXmlToString(node, "androidSDKPath");
            config.bundleIdentifier = AnalyzeXmlToString(node, "bundleIdentifier");
            config.bundleVersion = AnalyzeXmlToString(node, "bundleVersion");
            config.bundleVersionCode = AnalyzeXmlToInt(node, "bundleVersionCode");
            config.keystorePath = AnalyzeXmlToString(node, "keystorePath");
            config.keystorePassword = AnalyzeXmlToString(node, "keystorePassword");
            config.keyName = AnalyzeXmlToString(node, "keyName");
            config.keyPassword = AnalyzeXmlToString(node, "keyPassword");
            dic.Add(config.configName, config);
        }
    }



    public static void SaveBuildXml(Dictionary<string, BuildConfig> dic)
    {
        if (File.Exists(cfgPath))   //如果已有文件 则删除重写
            File.Delete(cfgPath);
        XmlDocument xmlDoc = new XmlDocument();
        //创建Xml声明部分，即<?xml version="1.0" encoding="utf-8" ?>
        XmlDeclaration dec = xmlDoc.CreateXmlDeclaration("1.0", "utf-8", null);
        xmlDoc.AppendChild(dec);
        XmlNode rootNode = xmlDoc.CreateElement("Config");
        xmlDoc.AppendChild(rootNode);

        foreach (var iter in dic)
        {
            XmlNode child = xmlDoc.CreateElement("Child");
            XmlAttribute attr = xmlDoc.CreateAttribute("configName");
            attr.Value = iter.Value.configName.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resAddress");
            attr.Value = iter.Value.resAddress.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("loginAddress");
            attr.Value = iter.Value.loginAddress.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("loginServerType");
            attr.Value = iter.Value.loginServerType.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("openCodeUpdate");
            attr.Value = BoolToInt(iter.Value.isOpenCodeUpdate).ToString();
            child.Attributes.Append(attr);



            attr = xmlDoc.CreateAttribute("openMD5");
            attr.Value = BoolToInt(iter.Value.isOpenMD5).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("openDLC");
            attr.Value = BoolToInt(iter.Value.isOpenDLC).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("openGuide");
            attr.Value = BoolToInt(iter.Value.isOpenGuide).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("openSDK");
            attr.Value = BoolToInt(iter.Value.isOpenSDK).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("openGM");
            attr.Value = BoolToInt(iter.Value.isOpenGM).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("versionName");
            attr.Value = iter.Value.versionName;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("isTDTest");
            //attr.Value = BoolToInt(iter.Value.isTDTest).ToString();
            attr.Value = BoolToInt(false).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("ReplaceRes");
            attr.Value = BoolToInt(iter.Value.isReplaceRes).ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resAndroidPath");
            attr.Value = iter.Value.resAndroidPath.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resIOSPath");
            attr.Value = iter.Value.resIOSPath.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resMacEditorPath");
            attr.Value = iter.Value.resMacEditorPath.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resWinEditorPath");
            attr.Value = iter.Value.resWinEditorPath.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("apkVersion");
            attr.Value = iter.Value.apkVersion.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("resPath");
            attr.Value = iter.Value.resPath.ToString();
            child.Attributes.Append(attr);

            /**********************渠道相关*************************/
            attr = xmlDoc.CreateAttribute("channelType");
            attr.Value = iter.Value.channelType.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("iconPath");
            attr.Value = iter.Value.iconPath;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("fileName");
            attr.Value = iter.Value.fileName;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("appName");
            attr.Value = iter.Value.appName;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("companyName");
            attr.Value = iter.Value.companyName;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("androidSDKPath");
            attr.Value = iter.Value.androidSDKPath;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("bundleIdentifier");
            attr.Value = iter.Value.bundleIdentifier;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("bundleVersion");
            attr.Value = iter.Value.bundleVersion;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("bundleVersionCode");
            attr.Value = iter.Value.bundleVersionCode.ToString();
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("keystorePath");
            attr.Value = iter.Value.keystorePath;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("keystorePassword");
            attr.Value = iter.Value.keystorePassword;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("keyName");
            attr.Value = iter.Value.keyName;
            child.Attributes.Append(attr);

            attr = xmlDoc.CreateAttribute("keyPassword");
            attr.Value = iter.Value.keyPassword;
            child.Attributes.Append(attr);

            rootNode.AppendChild(child);
        }
        string directoryName = Path.GetDirectoryName(cfgPath);
        if (!Directory.Exists(directoryName))
        {
            Directory.CreateDirectory(directoryName);
        }
        xmlDoc.Save(cfgPath);
    }

    private static int AnalyzeXmlToInt(XmlNode node, string key)
    {
        if (node.Attributes[key] != null)
        {
            string tempValue = node.Attributes[key].Value;
            if (tempValue.Length > 0)
                return int.Parse(tempValue);
        }
        return 0;
    }

    private static string AnalyzeXmlToString(XmlNode node, string key)
    {
        if (node.Attributes[key] != null)
            return node.Attributes[key].Value;
        return "";
    }

    private static bool AnalyzeXmlToBool(XmlNode node, string key)
    {
        int status = AnalyzeXmlToInt(node, key);
        if (status != 0)
        {
            return true;
        }
        return false;
    }


    private static int BoolToInt(bool state)
    {
        if (state)
            return 1;
        return 0;
    }



    /// <summary>
    /// 检测串值是否为合法的网址格式
    /// </summary>
    /// <param name="strValue">要检测的String值</param>
    /// <returns>成功返回true 失败返回false</returns>
    public static bool CheckIsUrlFormat(string strValue)
    {
        return CheckIsFormat(@"(http://)?([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", strValue);
    }

    /// <summary>
    /// 检测串值是否为合法的格式
    /// </summary>
    /// <param name="strRegex">正则表达式</param>
    /// <param name="strValue">要检测的String值</param>
    /// <returns>成功返回true 失败返回false</returns>
    public static bool CheckIsFormat(string strRegex, string strValue)
    {
        if (strValue != null && strValue.Trim() != "")
        {
            System.Text.RegularExpressions.Regex re = new System.Text.RegularExpressions.Regex(strRegex);
            if (re.IsMatch(strValue))
            {
                return true;
            }
            else
            {
                return false;
            }
        }
        return false;
    }

    /// <summary>
    /// 拷贝文件夹
    /// </summary>
    /// <param name="srcdir"></param>
    /// <param name="desdir"></param>
    public static void CopyDirectory(string srcdir, string desdir)
    {
        string folderName = srcdir.Substring(srcdir.LastIndexOf("/") + 1);
        string desfolderdir = desdir + "/" + folderName;
        if (desdir.LastIndexOf("/") == (desdir.Length - 1))
        {
            desfolderdir = desdir + folderName;
        }
        string[] filenames = Directory.GetFileSystemEntries(srcdir);
        foreach (string file in filenames)// 遍历所有的文件和目录
        {
            string path = file.Replace("\\", "/");
            if (Directory.Exists(path))// 先当作目录处理如果存在这个目录就递归Copy该目录下面的文件
            {
                string currentdir = desfolderdir + "/" + path.Substring(path.LastIndexOf("/") + 1);
                if (!Directory.Exists(currentdir))
                {
                    Directory.CreateDirectory(currentdir);
                }
                CopyDirectory(path, desfolderdir);
            }
            else // 否则直接copy文件
            {
                string srcfileName = path.Substring(path.LastIndexOf("/") + 1);
                srcfileName = desfolderdir + "/" + srcfileName;
                if (!Directory.Exists(desfolderdir))
                {
                    Directory.CreateDirectory(desfolderdir);
                }
                File.Copy(path, srcfileName);
            }
        }
    }

    public static void CopyAssetbundle(string srcdir, string desdir)
    {
        if (Directory.Exists(srcdir) == false)
        {
            Debug.LogError("resource files error:" + srcdir);
            return;
        }
        if (Directory.Exists(desdir) == false)
        {
            Debug.LogError("target files error:" + desdir);
            return;
        }
        DirectoryInfo srcInfo = new DirectoryInfo(srcdir);
        DirectoryInfo desInfo = new DirectoryInfo(desdir);
        FileInfo[] srcFiles = srcInfo.GetFiles("*.assetbundle", SearchOption.AllDirectories);
        for (int i = 0; i < srcFiles.Length; i++)
        {
            FileInfo srcFile = srcFiles[i];
            if (srcFile == null)
                continue;
            FileInfo[] desFiles = desInfo.GetFiles(srcFile.Name, SearchOption.AllDirectories);
            for (int j = 0; j < desFiles.Length; j++)
            {
                FileInfo desFile = desFiles[i];
                UnityEditor.FileUtil.ReplaceFile(srcFile.FullName, desFile.FullName);
            }
        }
    }
}


