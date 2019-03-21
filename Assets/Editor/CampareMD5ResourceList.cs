using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;

public class CampareMD5ResourceList
{

    // 对比对应版本目录下的VersionMD5和VersionMD5-old，得到最新的版本号文件VersionNum.xml
    public static void Execute()
    {

//#if UNITY_STANDALONE
//        ExportCommand.TargetPlatform  = BuildTarget.StandaloneWindows;
//#elif UNITY_IPHONE
//        ExportCommand.TargetPlatform = BuildTarget.iPhone;
//#elif UNITY_ANDROID
//        ExportCommand.TargetPlatform = BuildTarget.Android;
//#elif UNITY_WEBPLAYER
//        ExportCommand.TargetPlatform  = BuildTarget.WebPlayer;
//#elif UNITY_WP8
//        ExportCommand.TargetPlatform  = BuildTarget.WP8Player;
//#endif
        // 读取新旧MD5列表
        string newVersionMD5 = ExportCommand.AssetbundlePath + "/ResourcesXML/ResourcesList.xml";
        string oldVersionMD5 = ExportCommand.AssetbundlePath + "/ResourcesXML/ResourcesList-old.xml";
        Dictionary<string, VersionData> dicNewMD5Info = ReadMD5File(newVersionMD5);
        Dictionary<string, VersionData> dicOldMD5Info = ReadMD5File(oldVersionMD5);

        // 读取版本号记录文件ResourcesList.xml
        string oldVersionNum = ExportCommand.AssetbundlePath + "/ResourcesXML/ResourcesList_Compare.xml";
        Dictionary<string, VersionData> dicVersionNumInfo = ReadVersionNumFile(oldVersionNum);

        // 对比新旧MD5信息，并更新版本号，即对比dicNewMD5Info&&dicOldMD5Info来更新dicVersionNumInfo
        foreach (KeyValuePair<string, VersionData> newPair in dicNewMD5Info)
        {
            VersionData newVersionData = newPair.Value;
            if (dicOldMD5Info.ContainsKey(newPair.Key))
            {
                foreach (KeyValuePair<string, VersionData> oldPair in dicOldMD5Info)
                {
                    if (oldPair.Key == newPair.Key)  //旧版本中有该文件信息
                    {
                        if (oldPair.Value.MD5 != newPair.Value.MD5) //MD5不一样  则说明版本有变化
                        {
                            dicVersionNumInfo.Add(newPair.Key, newPair.Value);
                        }
                        break;
                    }
                }
            }
            else  // 旧版本中没有，则添加新纪录
            {
                dicVersionNumInfo.Add(newPair.Key, newPair.Value);
            }
        }
        // 不可能出现旧版本中有，而新版本中没有的情况，原因见生成MD5List的处理逻辑

        // 存储最新的ResourcesList_Compare.xml
        SaveVersionNumFile(dicVersionNumInfo, oldVersionNum);
        AssetDatabase.Refresh();
    }

    static Dictionary<string, VersionData> ReadMD5File(string fileName)
    {
        Dictionary<string, VersionData> DicMD5 = new Dictionary<string, VersionData>();

        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicMD5;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlElement XmlRoot = XmlDoc.DocumentElement;
        foreach (XmlNode node in XmlRoot.ChildNodes)
        {
            if ((node is XmlElement) == false)
                continue;
            string file = (node as XmlElement).GetAttribute("Name");
            string md5 = (node as XmlElement).GetAttribute("MD5");
            string path = (node as XmlElement).GetAttribute("Path");
            string size = (node as XmlElement).GetAttribute("Size");
            VersionData versionData = new VersionData();
            versionData.fileName = file;
            versionData.filePath = path;
            versionData.fileSize = int.Parse(size);
            versionData.MD5 = md5;
            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, versionData);
            }
        }
        XmlRoot = null;
        XmlDoc = null;

        return DicMD5;
    }

    static Dictionary<string, VersionData> ReadVersionNumFile(string fileName)
    {
        Dictionary<string, VersionData> DicVersionNum = new Dictionary<string, VersionData>();

        // 如果文件不存在，则直接返回
        if (System.IO.File.Exists(fileName) == false)
            return DicVersionNum;

        XmlDocument XmlDoc = new XmlDocument();
        XmlDoc.Load(fileName);
        XmlNodeList nodelist = XmlDoc.SelectSingleNode("Items").ChildNodes;
        foreach (XmlElement element in nodelist) 
        {
            foreach (XmlElement xe in element) 
            {
                VersionData data = new VersionData();
                if (xe.Name == "Name")
                {
                    data.fileName = xe.InnerText;
                }
                else if (xe.Name == "Path")
                {
                    data.filePath = xe.InnerText;
                }
                else if (xe.Name == "MD5")
                {
                    data.MD5 = xe.InnerText;
                }
                else if (xe.Name == "Size")
                {
                    data.fileSize = int.Parse(xe.InnerText);
                }
                if (DicVersionNum.ContainsKey(data.fileName) == false)
                {
                    DicVersionNum.Add(data.fileName, data);
                }
            }
        }
        return DicVersionNum;
    }

    static void SaveVersionNumFile(Dictionary<string, VersionData> data, string savePath)
    {
        XmlDocument XmlDoc = new XmlDocument();
        XmlElement XmlRoot = XmlDoc.CreateElement("Items");
        XmlDoc.AppendChild(XmlRoot);
        foreach (KeyValuePair<string, VersionData> pair in data)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("Item");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute("Name", pair.Value.fileName);
            xmlElem.SetAttribute("MD5",pair.Value.MD5.ToString());
            xmlElem.SetAttribute("Path", pair.Value.filePath.ToString());
            xmlElem.SetAttribute("Size", XmlConvert.ToString(pair.Value.fileSize));
        }
        XmlDoc.Save(savePath);
        XmlRoot = null;
        XmlDoc = null;
    }
}

public class VersionData 
{
    public string fileName;
    public string filePath;
    public string MD5;
    public long fileSize;
}