using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Text.RegularExpressions;
public class LanguageTool : Editor
{
    static string mXmlDirctory = Application.dataPath + "/__UI/output";
    public static string mXmlSave = Application.dataPath + "/__UI/output/XmlSave";
    static string mXmlPath = Application.dataPath + "/__UI/output/UILanguageXml.xml";

    static string mXmlSavePath = Application.dataPath + "/__UI/output/UILanguageSaveXml.xml";

    static string mConstStringPath = Application.dataPath + "/Scripts/Common/ConstString.cs";
    static string mErrorCodePath = Application.dataPath + "/Scripts/Common/ErrorCode.cs";

    static string ConstStringOut = "ConstStringOut";
    static string ConstStringIn = Application.dataPath + "/__UI/output/ConstStringIn/";

    static string ErrorCodeOut = "ErrorCodeOut";
    static string ErrorCodeIn = Application.dataPath + "/__UI/output/ErrorCodeIn/";

    static string mConstStringXmlSavePath = Application.dataPath + "/__UI/output/XmlSave/ConstStringLanguageSaveXml.xml";
    static string mErrorCodeXmlSavePath = Application.dataPath + "/__UI/output/XmlSave/ErrorCodeLanguageSaveXml.xml";

    string AppDir = Application.dataPath;

    static FileMode ExcelModel = FileMode.Append;
    [MenuItem("LanguageTools/一键导出所有代码和UI中文")]
    static void OneKeyOut()
    {
        if (!Directory.Exists(mXmlSave))
            Directory.CreateDirectory(mXmlSave);
        LanguageStatistics.LanguageStat();
        LanguageOutStart();
    }
    [MenuItem("LanguageTools/一键导入所有代码和UI中文")]
    static void OneKeyIn()
    {
        LanguageStatistics.LanguageToPrefabs();
        LanguageInStat();
    }
    [MenuItem("LanguageTools/导出所有代码中文")]
    static void LanguageOutStart()
    {
        if (!Directory.Exists(mXmlSave))
            Directory.CreateDirectory(mXmlSave);

        List<List<string>> tmpData = ReadCsFile(mConstStringPath, "public", " =", "\"", "\";", mConstStringXmlSavePath);
        List<List<string>> tmpValue = ReadCsFile(mConstStringPath, "public", " =", "{", "};", mConstStringXmlSavePath);
        tmpData.AddRange(tmpValue);
        if (!Directory.Exists(mXmlDirctory))
            Directory.CreateDirectory(mXmlDirctory);
        DataToExcel(tmpData,mXmlDirctory + "/" + ConstStringOut);

        tmpData = ReadCsFile(mErrorCodePath, "case", ":", "\"", "\";", mErrorCodeXmlSavePath);
        if (!Directory.Exists(mXmlDirctory))
            Directory.CreateDirectory(mXmlDirctory);
        DataToExcel(tmpData, mXmlDirctory + "/" + ErrorCodeOut);
        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    [MenuItem("LanguageTools/导入所有代码中文")]
    static void LanguageInStat()
    {
        DirectoryToCS(ConstStringIn, mConstStringPath, mConstStringXmlSavePath);
        DirectoryToCS(ErrorCodeIn, mErrorCodePath, mErrorCodeXmlSavePath);

        EditorUtility.DisplayDialog("", "Completed", "OK");
    }
    static void DirectoryToCS(string Path,string csFile,string saveName)
    {
        if (Directory.Exists(Path))
        {
            XmlDocument xmlSave = LanguageStatistics.LoadXml(saveName);
            XmlNode rootSave = xmlSave.SelectSingleNode("Data");

            string[] filenames = Directory.GetFiles(Path);
            foreach (string fileName in filenames)
            {
                string[] tmpFile = fileName.Split(new char[] { '.' });
                if (tmpFile.Length <= 0)
                    continue;
                if (!tmpFile[tmpFile.Length - 1].Equals("txt"))
                    continue;
                List<List<string>> tmpData = ExcelToData(fileName);
                FileReplace(tmpData, csFile, 1, 2, xmlSave,rootSave);
            }
            xmlSave.Save(saveName);
            xmlSave = null;
        }
        else
        {
            Directory.CreateDirectory(Path);
        }
    }
    static void FileReplace(List<List<string>> tmpData, string FileNalme, uint indexBefor, uint indexAfter, XmlDocument xmlSave, XmlNode rootSave)
    {
        if (tmpData == null)
            return;
        if (!File.Exists(FileNalme))
            return;

        string str = File.ReadAllText(FileNalme);
        string[] tmpStr = Regex.Split(str,"\r\n");
        str = "";
        foreach (string tmpLine in tmpStr)
        {
            string WriteLine = tmpLine;
            foreach(List<string> tmpDataLine in tmpData)
            {
                if (tmpDataLine.Count <= indexBefor || tmpDataLine.Count <= indexAfter)
                    continue;
                if (string.IsNullOrEmpty(WriteLine))
                    break;
                if (WriteLine.Contains(tmpDataLine[0]))
                {
                    //if (Regex.IsMatch(WriteLine,tmpDataLine[(int)indexBefor]))
                    //    Debug.LogError(WriteLine);
                    if (!LanguageStatistics.IsContain(rootSave, tmpDataLine[0], tmpDataLine[(int)indexAfter]))
                    {
                        XmlElement tmppath = xmlSave.CreateElement("Element");
                        tmppath.SetAttribute("KEY", tmpDataLine[0]);
                        XmlElement tmpvalue = xmlSave.CreateElement("VALUE");
                        tmpvalue.InnerText = tmpDataLine[(int)indexAfter];
                        tmppath.AppendChild(tmpvalue);
                        rootSave.AppendChild(tmppath);
                    }
                    WriteLine = WriteLine.Replace(tmpDataLine[(int)indexBefor], tmpDataLine[(int)indexAfter]);
                    break;
                }
            }
            str = str + WriteLine + "\r\n";
        }
        File.WriteAllText(FileNalme, str);
    }

    static List<List<string>> ReadCsFile(string vFileName, string keyMatchStart, string keyMatchEnd, string valueMathcStart, string valueMathcEnd,string XmlName)
    {
        List<List<string>> tmpData = new List<List<string>>();
        XmlDocument xmlSave = LanguageStatistics.LoadXml(XmlName);
        XmlNode rootSave = xmlSave.SelectSingleNode("Data");

        FileStream objFileStream;
        StreamReader objStreamReader;
        objFileStream = new FileStream(vFileName, FileMode.OpenOrCreate, FileAccess.Read);
        objStreamReader = new StreamReader(objFileStream, System.Text.Encoding.UTF8);
        string strLine = "";
        strLine = objStreamReader.ReadLine();

        while (strLine != null)
        {
            if (IsIgnore(strLine))
            {
                strLine = objStreamReader.ReadLine();
                continue;
            }
            string key = MatchStr(strLine, keyMatchStart, keyMatchEnd);
            string value = MatchStr(strLine, valueMathcStart, valueMathcEnd);
            if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
            {
                if (!LanguageStatistics.IsContain(rootSave, key, value))
                {
                    XmlElement tmppath = xmlSave.CreateElement("Element");
                    tmppath.SetAttribute("KEY", key);
                    XmlElement tmpvalue = xmlSave.CreateElement("VALUE");
                    tmpvalue.InnerText = value;
                    tmppath.AppendChild(tmpvalue);
                    rootSave.AppendChild(tmppath);
                    List<string> tmpLine = new List<string>();
                    tmpLine.Add(key);
                    tmpLine.Add(value);
                    tmpData.Add(tmpLine);
                }
            }
            strLine = objStreamReader.ReadLine();
        }
        xmlSave.Save(XmlName);
        xmlSave = null;
        return tmpData;
    }
    static bool IsIgnore(string vStr)
    {
        bool value = false;
        foreach(char tmpChr in vStr)
        {
            if (tmpChr == ' ')
                continue;
            if(tmpChr == '/')
            {
                if (value)
                    return true;
                value = true;
                continue;
            }
            break;
        }
        return false;
    }
    static string MatchStr(string resStr,string matchStrStar,string matchStrEnd)
    {
        string matchStr = matchStrStar + ".*" + matchStrEnd;
        Match tmpMatch = Regex.Match(resStr, matchStr);
        if(tmpMatch.Success)
        {
            matchStr = tmpMatch.Value;
            return matchStr;
        }
        return "";
    }
    //将dt导出到excel
    static void DataToExcel(List<List<string>> m_DataTable, string s_FileName)
    {
        if (true)
        {
            string FileName = s_FileName + ".xls";
            if (!File.Exists(FileName))
                ExcelModel = FileMode.OpenOrCreate;
            FileStream objFileStream;
            StreamWriter objStreamWriter;
            string strLine = "";
            objFileStream = new FileStream(FileName, ExcelModel, FileAccess.Write);

            objStreamWriter = new StreamWriter(objFileStream, System.Text.Encoding.Unicode);

            if (ExcelModel == FileMode.OpenOrCreate)
            {
                strLine = "请忽略（勿删）" + Convert.ToChar(9) + "需翻译" + Convert.ToChar(9) + "翻译后" + Convert.ToChar(9) + "备注：请保证左右一一对应，空白的地方不用留空白" + Convert.ToChar(9);

                byte[] buff = Encoding.Unicode.GetBytes(strLine);
                strLine = Encoding.Unicode.GetString(buff, 0, buff.Length);
                //strLine = Convert.ToChar(strLine);
                objStreamWriter.WriteLine(strLine);
            }
            strLine = "";

            for (int i = 0; i < m_DataTable.Count; i++)
            {
                List<string> tmpListString = m_DataTable[i];
                for (int j = 0; j < tmpListString.Count; j++)
                {
                    if (tmpListString[j] == null)
                        strLine = strLine + " " + Convert.ToChar(9);
                    else
                    {
                        string rowstr = "";
                        rowstr = tmpListString[j].ToString();
                        if (rowstr.IndexOf("\r\n") > 0)
                            rowstr = rowstr.Replace("\r\n", " ");
                        if (rowstr.IndexOf("\t") > 0)
                            rowstr = rowstr.Replace("\t", " ");
                        strLine = strLine + rowstr + Convert.ToChar(9);
                    }
                }
                strLine = Regex.Replace(strLine,"\"","#*");
                objStreamWriter.WriteLine(strLine);
                strLine = "";
            }
            objStreamWriter.Close();
            objFileStream.Close();
        }
    }
    static List<List<string>> ExcelToData(string s_FileName)
    {
        List<List<string>> m_DataTable = new List<List<string>>();
        string FileName = s_FileName;
        if (!File.Exists(FileName))
            return m_DataTable;
        FileStream objFileStream;
        StreamReader objStreamReader;
        string strLine = "";
        objFileStream = new FileStream(FileName, FileMode.OpenOrCreate, FileAccess.Read);
        objStreamReader = new StreamReader(objFileStream, System.Text.Encoding.Unicode);
        strLine = objStreamReader.ReadLine();
        while(strLine != null)
        {
            strLine = strLine.Replace("\"", "");
            strLine = strLine.Replace("#*", "\"");
            string[] tmpStr = Regex.Split(strLine, "\t");
            List<string> tmpLine = new List<string>();
            foreach(string tmpInfo in tmpStr)
            {
                tmpLine.Add(tmpInfo);
            }
            m_DataTable.Add(tmpLine);
            strLine = objStreamReader.ReadLine();
        }
        objFileStream.Close();
        objStreamReader.Close();
        return m_DataTable;
    }
}
