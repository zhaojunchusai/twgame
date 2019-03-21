using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.Xml;
using System.IO;
using System.Text.RegularExpressions;

public class LanguageStatistics : Editor
{
    static string mXmlDirctory = Application.dataPath + "/__UI/output";
    static string mXmlPath = Application.dataPath + "/__UI/output/UILanguageXml.xml";
    static string mXmlGetInPath = Application.dataPath + "/__UI/output/UILanguageGetIn/";

    static string mXmlSavePath = Application.dataPath + "/__UI/output/XmlSave/UILanguageSaveXml.xml";
    static string mPrefabsPath = Application.dataPath + "/ExportRes/View";
    static bool IsComparValue = true;
    static class UIXmlNodeName
    {
        public const string UIRoot = "uiRoot";
        public const string Name = "name";
        public const string Label = "uiLabel";
        public const string NodePath = "path";
        public const string LabelValue = "value";
    }

    [MenuItem("LanguageTools/导出所有UI中文")]
    public static void LanguageStat()
    {
        if (!Directory.Exists(LanguageTool.mXmlSave))
            Directory.CreateDirectory(LanguageTool.mXmlSave);

        DirectoryInfo directoryInfo = new DirectoryInfo(mPrefabsPath);
        FileInfo[] fileInfos = directoryInfo.GetFiles("*.prefab", SearchOption.AllDirectories);
        string assetPath = string.Empty;
        XmlDocument xmlDoc = LoadXml();
        XmlDocument xmlSave = LoadXml(mXmlSavePath);
        foreach (FileInfo fileInfo in fileInfos)
        {
            assetPath = fileInfo.FullName;
            assetPath = assetPath.Substring(assetPath.IndexOf("Assets\\"));
            assetPath = assetPath.Replace('\\', '/');
            GameObject go = AssetDatabase.LoadAssetAtPath(assetPath, typeof(GameObject)) as GameObject;
            WriteXml(xmlDoc, go, xmlSave);
        }
        if (!Directory.Exists(mXmlDirctory))
            Directory.CreateDirectory(mXmlDirctory);
        xmlDoc.Save(mXmlPath);
        xmlSave.Save(mXmlSavePath);
        AssetDatabase.Refresh();
        xmlDoc = null;
        xmlSave = null;
    }

    //[MenuItem("Tools/导出选中预置的中文")]
    //static void SingleLanguageStat()
    //{
    //    GameObject[] uis = Selection.gameObjects;
    //    for (int i = 0; i < uis.Length; i++)
    //    {
    //        WriteXml(uis[i]);
    //    }
    //}

    [MenuItem("LanguageTools/导入所有UI翻译文字")]
    public static void LanguageToPrefabs()
    {
        if (Directory.Exists(mXmlGetInPath))
        {
            string[] filenames = Directory.GetFiles(mXmlGetInPath);
            foreach (string fileName in filenames)
            {
                string[] tmpFile = fileName.Split(new char[] { '.' });
                if (tmpFile.Length <= 0)
                    continue;
                if (!tmpFile[tmpFile.Length - 1].Equals("xml"))
                    continue;
                ChangePrefabLanguage(fileName);
            }
        }
        else
        {
            Directory.CreateDirectory(mXmlGetInPath);
        }
    }

    static XmlDocument LoadXml()
    {
        XmlDocument result = new XmlDocument();

        if (File.Exists(mXmlPath))
        {
            File.Delete(mXmlPath);
        }
        XmlNode xnode = result.CreateXmlDeclaration("1.0", "UTF-8", "");
        XmlElement content = result.CreateElement("Data");
        result.AppendChild(xnode);
        result.AppendChild(content);

        return result;
    }
    public static XmlDocument LoadXml(string path)
    {
        XmlDocument result = new XmlDocument();
        if (File.Exists(path))
        {
            result.Load(path);
        }
        else
        {
            XmlNode xnode = result.CreateXmlDeclaration("1.0", "UTF-8", "");
            XmlElement content = result.CreateElement("Data");
            result.AppendChild(xnode);
            result.AppendChild(content);
        }
        return result;
    }
    static void WriteXml(XmlDocument xmlDoc, GameObject ui, XmlDocument xmlSvae)
    {
        if (xmlDoc == null)
            return;

        XmlNode root = xmlDoc.SelectSingleNode("Data");
        XmlNode rootSave = xmlSvae.SelectSingleNode("Data");

        XmlElement uiRoot = xmlDoc.CreateElement(UIXmlNodeName.UIRoot);
        uiRoot.SetAttribute(UIXmlNodeName.Name, ui.name);

        GetLabelComponent(xmlDoc, uiRoot, ui.transform, "", rootSave, xmlSvae);

        if (!uiRoot.IsEmpty)
        {
            root.AppendChild(uiRoot);
        }
    }

    static void GetLabelComponent(XmlDocument xmlDoc, XmlNode uiRoot, Transform trans, string parentPath, XmlNode xmlSave, XmlDocument xmlSaveDoc)
    {
        string path = "";
        if (string.IsNullOrEmpty(parentPath))
        {
            path = trans.name;
        }
        else
        {
            path = string.Format("{0}/{1}", parentPath, trans.name);
        }

        if (trans.GetComponent<UILabel>() != null && trans.GetComponent<UILabel>().text != "")
        {
            string value = "";
            value = trans.GetComponent<UILabel>().text;

            if (!IgnoreLabel(value))
            {
                XmlElement labelnode = xmlDoc.CreateElement(UIXmlNodeName.Label);
                labelnode.SetAttribute(UIXmlNodeName.Name, trans.name);
                XmlElement labelpath = xmlDoc.CreateElement(UIXmlNodeName.NodePath);
                XmlElement labelvalue = xmlDoc.CreateElement(UIXmlNodeName.LabelValue);
                labelpath.InnerText = path;
                labelvalue.InnerText = value;

                XmlElement tmppath = xmlSaveDoc.CreateElement("Element");
                tmppath.SetAttribute(UIXmlNodeName.NodePath, path);
                XmlElement tmpvalue = xmlSaveDoc.CreateElement(UIXmlNodeName.LabelValue);
                tmpvalue.InnerText = value;
                tmppath.AppendChild(tmpvalue);

                labelnode.AppendChild(labelpath);
                labelnode.AppendChild(labelvalue);
                if (!IsContain(xmlSave, path, value))
                {
                    uiRoot.AppendChild(labelnode);
                    xmlSave.AppendChild(tmppath);
                }
            }
        }

        for (int i = 0; i < trans.childCount; i++)
        {
            GetLabelComponent(xmlDoc, uiRoot, trans.GetChild(i), path, xmlSave, xmlSaveDoc);
        }
    }
    public static bool IsContain(XmlNode xmlSave, string InnerTex, string value)
    {
        for (int i = 0; i < xmlSave.ChildNodes.Count; ++i)
        {
            XmlNode tmpNode = xmlSave.ChildNodes[i];
            if (tmpNode == null)
                continue;
            if (tmpNode.Attributes == null)
                continue;
            if (tmpNode.Attributes.Count <= 0)
                continue;
            if (tmpNode.Attributes[0].Value.Equals(InnerTex))
            {
                if (!IsComparValue)
                    return true;
                XmlNode child = tmpNode.ChildNodes[0];
                if (child.InnerText == null)
                    continue;
                if (child != null && child.InnerText.Equals(value))
                    return true;
            }
        }
        return false;
    }
    static void ChangePrefabLanguage(string FielPath)
    {
        XmlDocument xmlDoc = LoadXml(FielPath);
        XmlDocument xmlSave = LoadXml(mXmlSavePath);
        XmlNode rootSave = xmlSave.SelectSingleNode("Data");
        if (xmlDoc == null)
            return;

        XmlNode root = xmlDoc.SelectSingleNode("Data");

        foreach (XmlElement element in root)
        {
            string uiName = element.GetAttribute(UIXmlNodeName.Name);

            GameObject ui = AssetDatabase.LoadAssetAtPath(
                string.Format("Assets/ExportRes/View/{0}.prefab", uiName), typeof(GameObject)) as GameObject;

            if (ui == null)
                continue;

            Transform uiTrans = ui.transform;

            string path = "";
            string labeltext = "";

            XmlNodeList labels = element.SelectNodes(UIXmlNodeName.Label);

            foreach (XmlElement label in labels)
            {
                foreach (XmlElement e in label)
                {
                    if (e.Name == UIXmlNodeName.NodePath)
                    {
                        path = e.InnerText;
                    }

                    if (e.Name == UIXmlNodeName.LabelValue)
                    {
                        labeltext = e.InnerText;
                    }
                }
                Transform tmpTransform = uiTrans.FindChild(path.Substring(path.IndexOf("/") + 1));
                if (tmpTransform != null)
                {
                    UILabel tmpLabel = tmpTransform.GetComponent<UILabel>();
                    if (tmpLabel != null)
                    {
                        tmpLabel.text = labeltext;
                        if (!IsContain(rootSave, path, labeltext))
                        {
                            XmlElement tmppath = xmlSave.CreateElement("Element");
                            tmppath.SetAttribute(UIXmlNodeName.NodePath, path);
                            XmlElement tmpvalue = xmlSave.CreateElement(UIXmlNodeName.LabelValue);
                            tmpvalue.InnerText = labeltext;
                            tmppath.AppendChild(tmpvalue);
                            rootSave.AppendChild(tmppath);
                        }
                    }

                }
                // uiTrans.FindChild(path.Substring(path.IndexOf("/")+1)).GetComponent<UILabel>().text = labeltext;
            }
            EditorUtility.SetDirty(ui);
        }
        xmlSave.Save(mXmlSavePath);
        xmlSave = null;
    }

    static bool IgnoreLabel(string str)
    {
        if (str == "New Label") return true;
        if (str == "Button") return true;

        return CheckHasNumber(str);
    }

    static bool CheckHasNumber(string str)
    {
        //Match ma = Regex.Match(str, "[\u4e00-\u9fa5],{0,}");
        //Match ma1 = Regex.Match(str, "[A-Za-z]+");
        //return !(ma.Success || ma1.Success);
        Match ma = Regex.Match(str, "[\u4e00-\u9fa5],{0,}");
        return !ma.Success;

    }
}
