using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class CreateMD5ResourceList
{
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
        Dictionary<string, VersionData> DicFileMD5 = new Dictionary<string, VersionData>();
        MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
        string dir = ExportCommand.AssetbundlePath;
        foreach (string dir_Path in Directory.GetDirectories(dir))
        {
            foreach (string filePath in Directory.GetFiles(dir_Path))
            {
                if (string.IsNullOrEmpty(filePath))
                    continue;
                if (filePath.Contains(".meta") || filePath.Contains("ResourcesXML") || filePath.Contains(".svn"))
                    continue;
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] hash = md5Generator.ComputeHash(file);
                string strMD5 = System.BitConverter.ToString(hash);
                file.Close();
                string extension = string.Empty;
                FileInfo fileinfo = new FileInfo(filePath);
                string key = Path.GetFileNameWithoutExtension(filePath);
                if (string.IsNullOrEmpty(key))
                    continue;
                if (DicFileMD5.ContainsKey(key) == false)
                {
                    VersionData data = new VersionData();
                    data.fileName = key;
                    data.filePath = filePath.Substring(dir.Length, filePath.Length - dir.Length);
                    data.MD5 = strMD5;
                    data.fileSize = fileinfo.Length;
                    DicFileMD5.Add(key, data);
                }
                else
                    Debug.LogWarning("<Two File has the same name> name = " + filePath);
            }

        }
        string savePath = ExportCommand.AssetbundlePath + "/ResourcesXML";
        if (Directory.Exists(savePath) == false)
            Directory.CreateDirectory(savePath);

        //// 删除前一版的old数据
        if (File.Exists(savePath + "/ResourcesList-old.xml"))
        {
            System.IO.File.Delete(savePath + "/ResourcesList-old.xml");
        }

        // 如果之前的版本存在，则将其名字改为ResourcesList-old.xml
        if (File.Exists(savePath + "/ResourcesList.xml"))
        {
            System.IO.File.Move(savePath + "/ResourcesList.xml", savePath + "/ResourcesList-old.xml");
        }

        XmlDocument XmlDoc = new XmlDocument();
        XmlNode xnode = XmlDoc.CreateXmlDeclaration("1.0", "UTF-8", "");
        XmlDoc.AppendChild(xnode);
        XmlElement XmlRoot = XmlDoc.CreateElement("Items");
        XmlDoc.AppendChild(XmlRoot);
        foreach (KeyValuePair<string, VersionData> pair in DicFileMD5)
        {
            XmlElement xmlElem = XmlDoc.CreateElement("Item");
            XmlRoot.AppendChild(xmlElem);
            xmlElem.SetAttribute("Name", pair.Key);
            xmlElem.SetAttribute("MD5", pair.Value.MD5);
            pair.Value.filePath = pair.Value.filePath.Replace("\\", "/");
            xmlElem.SetAttribute("Path", pair.Value.filePath);
            int filesize = Mathf.CeilToInt(pair.Value.fileSize / 1024f);
            xmlElem.SetAttribute("Size", XmlConvert.ToString(filesize));
        }
        // 读取旧版本的MD5
        Dictionary<string, VersionData> dicOldMD5 = ReadMD5File(savePath + "/ResourcesList-old.xml");
        // ResourcesList-old中有，而ResourcesList中没有的信息，手动添加到ResourcesList
        foreach (KeyValuePair<string, VersionData> pair in dicOldMD5)
        {
            if (DicFileMD5.ContainsKey(pair.Key) == false)
                DicFileMD5.Add(pair.Key, pair.Value);
        }
        XmlDoc.Save(savePath + "/ResourcesList.xml");
        XmlDoc = null;
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
            VersionData data = new VersionData();
            data.fileName = file;
            data.filePath = path;
            data.fileSize = int.Parse(size);
            data.MD5 = md5;
            if (DicMD5.ContainsKey(file) == false)
            {
                DicMD5.Add(file, data);
            }
        }

        XmlRoot = null;
        XmlDoc = null;

        return DicMD5;
    }

}