using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

public class ApkBuilder : EditorBase
{
    private static ApkBuilder apkBuildWindow = null;
    public string localAndroidSDKPath = Application.dataPath + "/Plugins/Android";
    private static string streamingAssetsPath = Application.streamingAssetsPath + "/";
    private string TDDirectoryPath = "TD/Assetbundle/";
    private string TDTestDirectoryPath = "TDTest/TD/Assetbundle/";
    private string windowsDirectoryPath = "Editor";
    private string androidDirectoryPath = "Android";
    public int columnCount = 5;

    [MenuItem("BuildMenu/Auto Build")]
    public static void ApkBuild()
    {
        if (EditorUtility.DisplayDialog("提示", "是否需要修改相应配置在进行打包？点击是，进入配置修改界面", "是", "否"))
        {
            BuildConfigEditor.ShowBuildConfigWindow();
        }
        else
        {
            ShowApkBuildWindow();
        }
    }

    public static void ShowApkBuildWindow()
    {
        apkBuildWindow = (ApkBuilder)EditorWindow.GetWindow(typeof(ApkBuilder));
        apkBuildWindow.Show();
    }

    void OnGUI()
    {
        AddTitleLabel("BuildConfigEditor");
        if (!OnShowBuildEditor) return;

        EditorGUILayout.BeginHorizontal();
        LeftNameLabel("单行最大显示数量");
        columnCount = EditorGUILayout.IntField(columnCount, GUILayout.Width(fieldWidth / 2));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        LeftNameLabel("所有渠道打包");
        if (GUILayout.Button("打包所有渠道", GUILayout.Width(btnWidth)))
        {
            BuildAllApk();
        }

        EditorGUILayout.Space();
        EditorGUILayout.Space();

        LeftNameLabel("单个渠道打包");
        int index = 0;
        foreach (var iter in BuildConfigDic)
        {
            if (index % columnCount == 0)
                EditorGUILayout.BeginHorizontal();

            if (GUILayout.Button(iter.Value.configName, GUILayout.Width(btnWidth)))
            {
                Debug.Log("Start build single apk");
                BuildSingleApk(iter.Value.configName);
                ShowBuildPath();
            }

            if (index % columnCount == (columnCount - 1))
                EditorGUILayout.EndHorizontal();
            index++;
        }
    }

    private void ShowBuildPath()
    {
        System.Diagnostics.Process.Start(CommonTools.buildPath);
    }

    private void BuildAllApk()
    {
        Debug.Log("Start build all apk");
        List<string> buildList = BuildConfigDic.Keys.ToList();
        for (int i = 0; i < buildList.Count; i++)
        {
            BuildSingleApk(buildList[i]);
        }
        ShowBuildPath();
    }

    private void BuildSingleApk(string channelName)
    {
        BuildConfig config;
        BuildConfigDic.TryGetValue(channelName, out config);
        if (config == null) return;
        if (config.isReplaceRes)
            CommonTools.CopyAssetbundle(config.resPath, Application.streamingAssetsPath);
        CodePath.ChangeCode(config);
        AssetDatabase.Refresh();
        SaveBuildValue(config);
        SDKCopyDirectory(config.androidSDKPath, localAndroidSDKPath);

        if (CopyAndroidAssets(config))
        {
            BuildToAndroid(config);
        }
    }

    public static bool CopyAndroidAssets(BuildConfig config)
    {
        if (config == null)
            return false;

        if (Directory.Exists(streamingAssetsPath))
        {
            string[] tmpRootDirectorys = Directory.GetDirectories(streamingAssetsPath);
            int tmpRootDirectorysLength = tmpRootDirectorys.Length;
            for (int i = 0; i < tmpRootDirectorysLength; i++)
            {
                if (tmpRootDirectorys[i].Contains(".svn"))
                {
                    tmpRootDirectorysLength -= 1;
                }
            }
            if (tmpRootDirectorysLength > 1)
            {
                EditorUtility.DisplayDialog("提示", streamingAssetsPath + "文件夹下同时存在多个子文件夹，请删除无用的文件夹", "确认");
                return false;
            }
            else
            {
                string tmpCurrentPath = GetCurResPath(streamingAssetsPath) + "Assetbundle";
                //Debug.LogWarning("tmpCurrentPath: " + tmpCurrentPath);
                string tmpResPath = streamingAssetsPath + config.resAndroidPath.Replace("/Android/", "/");
                string[] tmpResDirectorys = Directory.GetDirectories(tmpCurrentPath);
                int tmpResDirectorysLength = tmpResDirectorys.Length;
                for (int i = 0; i < tmpResDirectorysLength; i++)
                {
                    if (tmpResDirectorys[i].Contains(".svn"))
                    {
                        tmpResDirectorysLength -= 1;
                    }
                }
                if (tmpResDirectorysLength > 1)
                {
                    EditorUtility.DisplayDialog("提示", tmpCurrentPath + "文件夹下同时存在多个子文件夹，请删除无用的文件夹", "确认");
                    return false;
                }
                else
                {
                    if (!CopyAssets(config.resAndroidPath))
                        return false;
                }
            }
        }
        else
        {
            EditorUtility.DisplayDialog("提示", streamingAssetsPath + "文件夹不存在", "确认");
            return false;
        }

        AssetDatabase.Refresh();
        return true;
    }
    /*
    public bool CopyAndroidAssets(BuildConfig config)
    {
        string toPath = string.Empty;
        string fromPath = string.Empty;
        if (Directory.Exists(streamingAssetsPath + TDDirectoryPath) && Directory.Exists(streamingAssetsPath + TDTestDirectoryPath))
        {
            EditorUtility.DisplayDialog("提示", "streamingAssetsPath 文件夹下同时存在TDTest以及TD,请删除无用的文件夹", "确认");
            return false;
        }
        else
        {
            if (config.isTDTest)
            {
                if (Directory.Exists(streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath) && Directory.Exists(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath))
                {
                    EditorUtility.DisplayDialog("提示", TDTestDirectoryPath + "文件夹下同时存在Android以及Editor文件夹,请删除无用的文件夹", "确认");
                    return false;
                }
                else
                {
                    if (CopyAssets(true) == false)
                        return false;
                }
            }
            else
            {
                if (Directory.Exists(streamingAssetsPath + TDDirectoryPath + androidDirectoryPath) && Directory.Exists(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath))
                {
                    EditorUtility.DisplayDialog("提示", TDDirectoryPath + "文件夹下同时存在Android以及Editor文件夹,请删除无用的文件夹", "确认");
                    return false;
                }
                else
                {
                    if (CopyAssets(false) == false)
                        return false;
                }
            }
        }
        AssetDatabase.Refresh();
        return true;
    }
    //*/

    private static bool IsExistFiles(string path)
    {
        if (Directory.Exists(path))
        {
            DirectoryInfo directory = new DirectoryInfo(path);
            FileInfo[] files = directory.GetFiles("*.assetbundle", SearchOption.AllDirectories);
            if (files != null && files.Length > 0)
            {
                return true;
            }
        }
        return false;
    }

    private static string GetCurResPath(string vRootPath)
    {
        if (string.IsNullOrEmpty(vRootPath))
            return string.Empty;
        if (!Directory.Exists(vRootPath))
            return string.Empty;

        string[] tmpRoot = Directory.GetDirectories(vRootPath);
        for (int i = 0; i < tmpRoot.Length; i++)
        {
            if (!tmpRoot[i].Contains(".svn"))
            {
                string tmpPath = tmpRoot[i].Replace("\\", "/");
                if (tmpPath.Contains("Assetbundle"))
                    return tmpPath.Replace("/Assetbundle", "/");
                return GetCurResPath(tmpPath).Replace("/Assetbundle", "/");
            }
        }

        return string.Empty;
    }

    private static bool CopyAssets(string vTargetPath)
    {
        if (string.IsNullOrEmpty(vTargetPath))
            return false;
        if (!IsExistFiles(streamingAssetsPath))
        {
            EditorUtility.DisplayDialog("提示", streamingAssetsPath + "文件夹下不存在任何有效Assetbundle文件,请检查资源是否导入正确", "确认");
            return false;
        }

        string tmpCurrentPath = GetCurResPath(streamingAssetsPath);
        string tmpTargetPath = (streamingAssetsPath + vTargetPath).Replace("/Assetbundle/Android/", "/");

        string[] tmpChildDirectorys = Directory.GetDirectories(tmpCurrentPath + "Assetbundle/");
        string tmpTargetAndroidPath = tmpCurrentPath + "Assetbundle/Android";
        for (int i = 0; i < tmpChildDirectorys.Length; i++)
        {
            if ((!tmpChildDirectorys[i].Contains(".svn")) && (!tmpChildDirectorys[i].Contains(".meta")))
            {
                if (!tmpChildDirectorys[i].Equals(tmpTargetAndroidPath))
                {
                    FileUtil.MoveFileOrDirectory(tmpChildDirectorys[i], tmpTargetAndroidPath);
                    FileUtil.DeleteFileOrDirectory(tmpChildDirectorys[i]);
                }
                break;
            }
        }
        AssetDatabase.Refresh();

        if (!tmpCurrentPath.Equals(tmpTargetPath))
        {
            Directory.CreateDirectory(tmpTargetPath);
            FileUtil.MoveFileOrDirectory(string.Format("{0}Assetbundle", tmpCurrentPath), string.Format("{0}Assetbundle", tmpTargetPath));
            if (tmpCurrentPath.Contains("/StreamingAssets/TD/"))
                FileUtil.DeleteFileOrDirectory(tmpCurrentPath);
            else
                FileUtil.DeleteFileOrDirectory(tmpCurrentPath.Replace("/TD/", "/"));
        }
        AssetDatabase.Refresh();

        return true;
    }
    /*
    private bool CopyAssets(bool isTest)
    {
        if (IsExistFiles(streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath))
        {
            if (isTest)
            {
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TD");
            }
            else
            {
                string resPath = streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath;
                string toPath = streamingAssetsPath + TDDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TDTest");
            }
        }
        else if (IsExistFiles(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath))
        {
            if (isTest)
            {
                string resPath = streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath;
                string toPath = streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TD");
            }
            else
            {
                string resPath = streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath;
                string toPath = streamingAssetsPath + TDDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TDTest");
            }
        }
        else if (IsExistFiles(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath))
        {
            if (isTest)
            {
                string resPath = streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath;
                string toPath = streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TD");
            }
            else
            {
                string resPath = streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath;
                string toPath = streamingAssetsPath + TDDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TDTest");
            }
        }
        else if (IsExistFiles(streamingAssetsPath + TDDirectoryPath + androidDirectoryPath))
        {
            if (isTest)
            {
                string resPath = streamingAssetsPath + TDDirectoryPath + androidDirectoryPath;
                string toPath = streamingAssetsPath + TDTestDirectoryPath + androidDirectoryPath;
                if (Directory.Exists(toPath) == false)
                    Directory.CreateDirectory(toPath);
                FileUtil.ReplaceDirectory(resPath, toPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDTestDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TD");
            }
            else
            {

                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + TDDirectoryPath + windowsDirectoryPath);
                FileUtil.DeleteFileOrDirectory(streamingAssetsPath + "TDTest");
            }
        }
        else
        {
            EditorUtility.DisplayDialog("提示", streamingAssetsPath + "文件夹下不存在任何有效Assetbundle文件,请检查资源是否导入正确", "确认");
            return false;
        }
        AssetDatabase.Refresh();
        return true;
    }
    //*/

    private void SDKCopyDirectory(string fromPath, string toPath)
    {
        FileUtil.DeleteFileOrDirectory(toPath);
        AssetDatabase.Refresh();
        if (!Directory.Exists(fromPath)) return;

        if (!Directory.Exists(toPath))
            Directory.CreateDirectory(toPath);

        string[] files = Directory.GetFiles(fromPath + "/");
        foreach (string file in files)
        {
            string name = file.Substring(file.LastIndexOf("/"));
            FileUtil.CopyFileOrDirectory(file, toPath + "/" + name);
        }
        string[] dirs = Directory.GetDirectories(fromPath + "/");
        foreach (string dir in dirs)
        {
            string name = dir.Substring(dir.LastIndexOf("/"));
            FileUtil.CopyFileOrDirectory(dir, toPath + "/" + name);
        }
        AssetDatabase.Refresh();
    }

    public void SaveBuildValue(BuildConfig config)
    {
        PlayerSettings.bundleIdentifier = config.bundleIdentifier;
        //PlayerSettings.bundleVersion = config.bundleVersion;
        PlayerSettings.bundleVersion = string.Format("{0}_{1}", config.bundleVersion, config.apkVersion);
        //Debug.LogError("PlayerSettings.bundleVersion: " + PlayerSettings.bundleVersion);
        PlayerSettings.Android.bundleVersionCode = config.bundleVersionCode;
        PlayerSettings.Android.keystoreName = config.keystorePath;
        PlayerSettings.Android.keystorePass = config.keystorePassword;
        PlayerSettings.Android.keyaliasName = config.keyName;
        PlayerSettings.Android.keyaliasPass = config.keyPassword;
        PlayerSettings.productName = config.appName;
        PlayerSettings.companyName = config.companyName;
        PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Unknown, new Texture2D[] { Resources.Load(config.iconPath) as Texture2D });
    }

    public void BuildToAndroid(BuildConfig config)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(BuildTarget.Android);
        DateTime dateTime = System.DateTime.Now;
        //string fileName = config.fileName + "_" + dateTime.ToString("yyyy-MM-dd") + "-" + dateTime.Hour + "-" + dateTime.Minute + ".apk";
        string fileName = string.Format("{0}_{1}_{2}-{3}-{4}.apk", config.fileName, config.apkVersion, dateTime.ToString("yyyy-MM-dd"), dateTime.Hour, dateTime.Minute);
        if (!Directory.Exists(CommonTools.buildPath))
            Directory.CreateDirectory(CommonTools.buildPath);
        string path = CommonTools.buildPath + fileName;
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        string error = BuildPipeline.BuildPlayer(AutoBuilder.GetScenePaths(), path, BuildTarget.Android, BuildOptions.None);
        if (!string.IsNullOrEmpty(error))
            Debug.LogError("Build fail:" + fileName);
        else
            Debug.Log("Build success:" + fileName);
    }
}
