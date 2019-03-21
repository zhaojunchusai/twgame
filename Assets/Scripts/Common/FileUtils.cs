using UnityEngine;
using System.IO;
using System.Security.Cryptography;
using System;

public class FileUtils 
{
    /*
    public static string GetAssetBundlePath()
    {
        string localPath = Application.persistentDataPath + "/" + GlobalConst.DIR_ASSETBUNDLE;
        return localPath;
    }


    public static string GetEditorSavePath()
    {
        string localPath = Application.dataPath + "/StreamingAssets/" + GlobalConst.DIR_ASSETBUNDLE;

        if (!File.Exists(localPath)) {
            Directory.CreateDirectory(localPath);
        }
        return localPath;
    }
    */

    /**
     * 读取StreamingAssets目录下文件
     * */
    public static string GetSteamingAssetsFile()
    {
        string PathURL;
#if UNITY_ANDROID
       PathURL = "jar:file://" + Application.dataPath + "!/assets/";
#elif UNITY_IPHONE
       PathURL = Application.dataPath + "/Raw/";
#elif UNITY_STANDALONE_WIN || UNITY_EDITOR
        PathURL = "file://" + Application.dataPath + "/StreamingAssets/";
#else
        PathURL = string.Empty;
#endif
        return PathURL;
    }



    public static string GetServerAssetBundleURL()
    {
        // TODO
        //string url = GlobalConst.ASSETSERVER_HTTP + GlobalConst.DIR_ASSETBUNDLE + "test/";
        return "";//url+ GetPlatformString() + "/";
    }


    public static string[] GetDirectoryInfo(string path)
    {
        string[] directoryEntries;
        directoryEntries = Directory.GetFiles(path, "*", SearchOption.AllDirectories);
        return directoryEntries;
    }


    public static void GetFileNameFromURLWithNoExtension(string url)
    { 
        
    }


    // 获得文件MD5
    public static bool GetFileMD5(string vFilePath, out string vFileMD5)
    {
        vFileMD5 = "";
        if (!File.Exists(vFilePath))
        {
            return false;
        }
        try
        {
            MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
            FileStream tFile = new FileStream(vFilePath, FileMode.Open, FileAccess.Read, FileShare.Read);
            byte[] hash = md5Generator.ComputeHash(tFile);
            vFileMD5 = System.BitConverter.ToString(hash);
            tFile.Close();
            return true;
        }
        catch (Exception ex)
        {
            DebugUtil.Log(ex.Message);
            return false;
        }
    }


    public static bool CheckFileMD5(string vFilePath, string serverFileMD5)
    {
        string fileMD5;
        GetFileMD5(vFilePath, out fileMD5);

        return fileMD5.Equals(serverFileMD5);
    }
}
