using UnityEngine;
using UnityEditor;
using System;
using System.IO;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BuildConfigEditor : EditorBase
{
    private static BuildConfigEditor buildConfigWindow = null;
    private static bool isShowBuildTip = false;
    /// <summary>
    /// 当前要发布的渠道的配置关键字
    /// </summary>
    private string configName = string.Empty;

    private static BuildConfig CurrentBuildConfig;
    private bool CreateNewBuildConfig;
    private bool ChangePathConfig = false;
    private bool ChangeProductName = false;
    private bool ChangeProjectSetting = false;
    private bool ChangeKeystore = false;
    private int SelectBuildIndex;
    private Texture AppIcon = null;
    private Vector2 scrollViewVector2 = Vector2.zero;
    private string resTempPath = string.Empty;

    private const string tips = "把SDK按渠道归类,比如'C:/Users/Aklan/Desktop/SDK/MI'为小米渠道,且不要放到Assets目录下\n编辑时：\n" +
                               "1.新建渠道配置,或选择一个要编辑的配置\n" +
                               "2.选择本渠道包对应的icon\n" +
                               "3.设置本渠道包的相关名字\n" +
                               "4.选择SDK目录,比如'C:/Users/Aklan/Desktop/SDK/MI'\n" +
                               "5.填写包名,版本等信息\n" +
                               "6.选择keystore\n" +
                               "7.点击保存或删除\n" +
                               "注意：路径不能有中文";

    [MenuItem("BuildMenu/Build Config")]
    public static void BuildMenu()
    {
        if (EditorUtility.DisplayDialog("注意事项", tips, "确定"))
        {
            ShowBuildConfigWindow();
        }
    }

    public static void ShowBuildConfigWindow()
    {
        buildConfigWindow = (BuildConfigEditor)EditorWindow.GetWindow(typeof(BuildConfigEditor));
        buildConfigWindow.minSize = new Vector2(400, 400);
        buildConfigWindow.Show();
    }


    private BuildConfig AddNewBuildConfig(string name)
    {
        BuildConfig config = new BuildConfig(name);
        BuildConfigDic.Add(name, config);
        return config;
    }

    private void DeleteBuildConfig()
    {
        BuildConfigDic.Remove(CurrentBuildConfig.configName);
        CommonTools.SaveBuildXml(BuildConfigDic);
        SelectBuildIndex -= 1;
    }

    protected override void OnEnabling()
    {
        isShowBuildTip = false;
        CreateNewBuildConfig = false;
        ChangePathConfig = false;
        ChangeProductName = false;
        ChangeKeystore = false;
        ChangeProjectSetting = false;
        SelectBuildIndex = 0;
    }

    void OnGUI()
    {
        if (BuildConfigDic == null || BuildConfigDic.Count <= 0)
        {
            CreateNewBuildConfig = true;
        }
        if (!OnShowBuildEditor)
            return;
        scrollViewVector2 = GUILayout.BeginScrollView(scrollViewVector2);
        string[] tempCodeList;
        CreateNewBuildConfig = EditorGUILayout.BeginToggleGroup("创建新的版本配置", CreateNewBuildConfig);
        configName = EditorGUILayout.TextField("当前要发布的渠道的配置关键字", configName, GUILayout.Width(labelWidth + fieldWidth));
        if (!string.IsNullOrEmpty(configName))
        {
            if (GUILayout.Button("Create", GUILayout.Width(btnWidth)))
            {
                if (!BuildConfigDic.ContainsKey(configName))
                {
                    AddNewBuildConfig(configName);
                    // 把index切换到新增的这个配置
                    tempCodeList = BuildConfigDic.Keys.ToArray();
                    for (int i = 0; i < tempCodeList.Length; i++)
                    {
                        if (tempCodeList[i] == configName)
                        {
                            SelectBuildIndex = i;
                            break;
                        }
                    }
                    CreateNewBuildConfig = false;
                }
                else
                {
                    EditorUtility.DisplayDialog("提示", "已创建相同版本配置文件,不可重复创建", "确定");
                }
            }
        }
        EditorGUILayout.EndToggleGroup();
        if (BuildConfigDic == null || BuildConfigDic.Count <= 0)
        {
            CreateNewBuildConfig = true;
        }
        EditorGUILayout.Space();
        if (!CreateNewBuildConfig)
        {
            tempCodeList = BuildConfigDic.Keys.ToArray();
            SelectBuildIndex = EditorGUILayout.Popup("渠道名", SelectBuildIndex, tempCodeList, GUILayout.Width(labelWidth + fieldWidth));
            if (SelectBuildIndex < tempCodeList.Length && SelectBuildIndex >= 0)
            {
                BuildConfigDic.TryGetValue(tempCodeList[SelectBuildIndex], out CurrentBuildConfig);
                CurrentBuildConfig.channelType = (int)(TargetPlatforms)EditorGUILayout.EnumPopup("选择要发布的渠道类型", (TargetPlatforms)CurrentBuildConfig.channelType, GUILayout.Width(labelWidth + fieldWidth));
                CurrentBuildConfig.resAddress = EditorGUILayout.TextField("资源检测服务器IP(包含端口)", CurrentBuildConfig.resAddress, GUILayout.Width(labelWidth + fieldWidth));
                CurrentBuildConfig.loginAddress = EditorGUILayout.TextField("账号登录服务器IP(包含端口)", CurrentBuildConfig.loginAddress, GUILayout.Width(labelWidth + fieldWidth));
                CurrentBuildConfig.loginServerType = (int)(EServerType)EditorGUILayout.EnumPopup("选择请求的服务器列表类型", (EServerType)CurrentBuildConfig.loginServerType, GUILayout.Width(labelWidth + fieldWidth));
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                CurrentBuildConfig.apkVersion = EditorGUILayout.IntField("Apk Version", CurrentBuildConfig.apkVersion, GUILayout.Width(labelWidth + fieldWidth));
                CurrentBuildConfig.versionName = EditorGUILayout.TextField("版本名称(Bugly用)", CurrentBuildConfig.versionName, GUILayout.Width(labelWidth + fieldWidth));
                EditorGUILayout.Space();
                EditorGUILayout.Space();
                CurrentBuildConfig.isOpenCodeUpdate = EditorGUILayout.Toggle("是否打开代码更新", CurrentBuildConfig.isOpenCodeUpdate);
                CurrentBuildConfig.isOpenMD5 = EditorGUILayout.Toggle("是否打开文件MD5码检查", CurrentBuildConfig.isOpenMD5);
                CurrentBuildConfig.isOpenDLC = EditorGUILayout.Toggle("是否打开资源更新", CurrentBuildConfig.isOpenDLC);
                CurrentBuildConfig.isOpenGuide = EditorGUILayout.Toggle("是否打开新手引导", CurrentBuildConfig.isOpenGuide);
                CurrentBuildConfig.isOpenGM = EditorGUILayout.Toggle("是否打开GM工具", CurrentBuildConfig.isOpenGM);
                CurrentBuildConfig.isOpenSDK = EditorGUILayout.Toggle("是否打开SDK", CurrentBuildConfig.isOpenSDK);
                EditorGUILayout.Space();
                EditorGUILayout.Space();

                //======================================================================================//
                //CurrentBuildConfig.isTDTest = EditorGUILayout.BeginToggleGroup("是否为TDTest路径", CurrentBuildConfig.isTDTest);
                //EditorGUILayout.EndToggleGroup();
                //if (CurrentBuildConfig.isTDTest)
                //{
                //    CurrentBuildConfig.resAndroidPath = "TDTest/TD/Assetbundle/Android/";
                //    CurrentBuildConfig.resIOSPath = "TDTest/TD/Assetbundle/Iphone/";
                //    CurrentBuildConfig.resWinEditorPath = "TDTest/TD/Assetbundle/Editor/";
                //    CurrentBuildConfig.resMacEditorPath = "TDTest/TD/Assetbundle/IOSEditor/";
                //}
                //else
                //{
                //    CurrentBuildConfig.resAndroidPath = "TD/Assetbundle/Android/";
                //    CurrentBuildConfig.resIOSPath = "TD/Assetbundle/Iphone/";
                //    CurrentBuildConfig.resWinEditorPath = "TD/Assetbundle/Editor/";
                //    CurrentBuildConfig.resMacEditorPath = "TD/Assetbundle/IOSEditor/";
                //}
                //--------------------------------------------------------------------------------------//
                resTempPath = EditorGUILayout.TextField("测试路径：", resTempPath, GUILayout.Width(labelWidth + fieldWidth));
                CurrentBuildConfig.isTDTest = EditorGUILayout.BeginToggleGroup("是否为TDTest路径", CurrentBuildConfig.isTDTest);
                EditorGUILayout.EndToggleGroup();
                //if (CurrentBuildConfig.isTDTest)
                if ((CurrentBuildConfig.isTDTest) && (!string.IsNullOrEmpty(resTempPath)))
                {
                    CurrentBuildConfig.resAndroidPath = string.Format("{0}/Assetbundle/Android/", resTempPath);
                    CurrentBuildConfig.resIOSPath = string.Format("{0}/Assetbundle/Iphone/", resTempPath);
                    CurrentBuildConfig.resWinEditorPath = string.Format("{0}/Assetbundle/Editor/", resTempPath);
                    CurrentBuildConfig.resMacEditorPath = string.Format("{0}/Assetbundle/IOSEditor/", resTempPath);
                }
                //======================================================================================//

                LeftNameLabel("当前资源路径", labelWidth + fieldWidth);
                EditorGUILayout.LabelField("Android资源读取路径", CurrentBuildConfig.resAndroidPath, GUILayout.Width(labelWidth + fieldWidth * 2));
                EditorGUILayout.LabelField("IOS资源读取路径", CurrentBuildConfig.resIOSPath, GUILayout.Width(labelWidth + fieldWidth * 2));
                EditorGUILayout.LabelField("Windows下编辑器资源读取路径", CurrentBuildConfig.resWinEditorPath, GUILayout.Width(labelWidth + fieldWidth * 2));
                EditorGUILayout.LabelField("Mac下编辑器资源读取路径", CurrentBuildConfig.resMacEditorPath, GUILayout.Width(labelWidth + fieldWidth * 2));
                EditorGUILayout.Space();
                CurrentBuildConfig.isReplaceRes = EditorGUILayout.BeginToggleGroup("是否有替换资源", CurrentBuildConfig.isReplaceRes);
                EditorGUILayout.BeginHorizontal();
                if (GUILayout.Button("Assets Path", GUILayout.Width(btnWidth)))
                {
                    CurrentBuildConfig.resPath = EditorUtility.OpenFolderPanel("Select Assets Path", CurrentBuildConfig.resPath, "assetbundle");
                }
                LeftNameLabel(CurrentBuildConfig.resPath, labelWidth + fieldWidth);
                EditorGUILayout.EndHorizontal();
                EditorGUILayout.EndToggleGroup();

                EditorGUILayout.Space();
                EditorGUILayout.Space();

            }
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // 图标
            LeftNameLabel("应用图标");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select Icon", GUILayout.Width(btnWidth)))
            {
                CurrentBuildConfig.iconPath = EditorUtility.OpenFilePanel("Select Icon", "", "png");
                if (!string.IsNullOrEmpty(CurrentBuildConfig.iconPath))
                {
                    string[] temp = CurrentBuildConfig.iconPath.Split(new string[] { "Resources/" }, StringSplitOptions.None);
                    CurrentBuildConfig.iconPath = temp[1];
                    CurrentBuildConfig.iconPath = CurrentBuildConfig.iconPath.Split(new string[] { ".png" }, StringSplitOptions.None)[0];
                }
            }
            AppIcon = Resources.Load(CurrentBuildConfig.iconPath) as Texture;
            if (AppIcon != null)
            {
                EditorGUI.DrawPreviewTexture(new Rect(170, 410, AppIcon.width / 2, AppIcon.height / 2), AppIcon);
            }

            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();
            EditorGUILayout.Space();

            ChangeProductName = EditorGUILayout.BeginToggleGroup("更新项目名字相关", ChangeProductName);
            CurrentBuildConfig.fileName = EditorGUILayout.TextField("电脑上显示的文件名", CurrentBuildConfig.fileName, GUILayout.Width(labelWidth + fieldWidth));
            CurrentBuildConfig.appName = EditorGUILayout.TextField("手机上显示的游戏名", CurrentBuildConfig.appName, GUILayout.Width(labelWidth + fieldWidth));
            CurrentBuildConfig.companyName = EditorGUILayout.TextField("公司名", CurrentBuildConfig.companyName, GUILayout.Width(labelWidth + fieldWidth));
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            LeftNameLabel("AndroidSDKPath");
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Select SDK", GUILayout.Width(btnWidth)))
            {
                CurrentBuildConfig.androidSDKPath = EditorUtility.OpenFolderPanel("Select Android File", CurrentBuildConfig.androidSDKPath, "Android");
            }
            LeftNameLabel(CurrentBuildConfig.androidSDKPath, labelWidth + fieldWidth);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            EditorGUILayout.Space();

            // 包信息设置//

            ChangeProjectSetting = EditorGUILayout.BeginToggleGroup("包信息设置", ChangeProjectSetting);
            EditorGUILayout.BeginVertical();
            EditorGUILayout.BeginHorizontal();
            LeftNameLabel("Bundle Identifier");
            CurrentBuildConfig.bundleIdentifier = EditorGUILayout.TextField(CurrentBuildConfig.bundleIdentifier, GUILayout.Width(fieldWidth));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            LeftNameLabel("Bundle Version");
            CurrentBuildConfig.bundleVersion = EditorGUILayout.TextField(CurrentBuildConfig.bundleVersion, GUILayout.Width(fieldWidth / 2));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            LeftNameLabel("Version Code");
            CurrentBuildConfig.bundleVersionCode = EditorGUILayout.IntField(CurrentBuildConfig.bundleVersionCode, GUILayout.Width(fieldWidth / 2));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndToggleGroup();

            EditorGUILayout.Space();
            ChangeKeystore = EditorGUILayout.BeginToggleGroup("Keystore Setting", ChangeKeystore);
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("Browse Keystore", GUILayout.Width(btnWidth * 2)))
            {
                CurrentBuildConfig.keystorePath = EditorUtility.OpenFilePanel("Select Keystore", "", "keystore");
            }
            EditorGUILayout.LabelField(CurrentBuildConfig.keystorePath);
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();


            EditorGUILayout.BeginHorizontal();
            LeftNameLabel("Keystore password", 20);
            CurrentBuildConfig.keystorePassword = EditorGUILayout.TextField(CurrentBuildConfig.keystorePassword, GUILayout.Width(fieldWidth - 20));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            CurrentBuildConfig.keyName = EditorGUILayout.TextField("Key Name", CurrentBuildConfig.keyName, GUILayout.Width(fieldWidth * 2));
            CurrentBuildConfig.keyPassword = EditorGUILayout.TextField("Key  Password", CurrentBuildConfig.keyPassword, GUILayout.Width(fieldWidth * 2));
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndToggleGroup();
            EditorGUILayout.Space();
            EditorGUILayout.BeginHorizontal(GUILayout.Width(btnWidth * 5));
            if (GUILayout.Button("保存配置", GUILayout.Width(btnWidth * 2)))
            {
                CheckConfigSet(CheckConfigState.Channel);
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("删除配置", GUILayout.Width(btnWidth * 2)))
            {
                DeleteBuildConfig();
            }
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.Space();
            if (GUILayout.Button("修改为当前所选版本类型", GUILayout.Width(btnWidth * 2)))
            {
                if (EditorUtility.DisplayDialog("提示", "请确认所选设置是否正确？已确认请点击继续.", "继续", "取消"))
                {
                    if (CurrentBuildConfig.isReplaceRes)
                        CommonTools.CopyAssetbundle(CurrentBuildConfig.resPath, Application.streamingAssetsPath);
                    CodePath.ChangeCode(CurrentBuildConfig);

                    ApkBuilder.CopyAndroidAssets(CurrentBuildConfig);

                    AssetDatabase.Refresh();
                    isShowBuildTip = true;
                }
            }
            EditorGUILayout.Space();
            if (GUILayout.Button("开始打包APK", GUILayout.Width(btnWidth * 2)))
            {
                if (EditorUtility.DisplayDialog("提示", "请确认所选设置是否正确？已确认请点击继续.", "继续", "取消"))
                {
                    ApkBuilder.ShowApkBuildWindow();
                    buildConfigWindow.Close();
                    isShowBuildTip = false;
                }
            }
        }
        GUILayout.EndScrollView();
    }

    private static void CheckConfigSet(CheckConfigState state)
    {
        if (CurrentBuildConfig == null)
        {
            EditorUtility.DisplayDialog("提示", "请选择一个配置进行修改.", "确定");
            return;
        }
        switch (state)
        {
            case CheckConfigState.Channel:
                {
                    if (CurrentBuildConfig.channelType == (int)TargetPlatforms.None)
                    {
                        if (!EditorUtility.DisplayDialog("提示", "确定不选择任何渠道类型吗", "确定", "取消"))
                        {
                            return;
                        }
                    }
                    CheckConfigSet(CheckConfigState.ResAddress);
                } break;
            case CheckConfigState.ResAddress:
                {
                    if (!CommonTools.CheckIsUrlFormat(CurrentBuildConfig.resAddress))
                    {
                        EditorUtility.DisplayDialog("提示", "请输入正确的资源版本检查地址!!!", "确定");
                        return;
                    }
                    CheckConfigSet(CheckConfigState.LoginAddress);
                } break;
            case CheckConfigState.LoginAddress:
                {
                    if (!CommonTools.CheckIsUrlFormat(CurrentBuildConfig.loginAddress))
                    {
                        EditorUtility.DisplayDialog("提示", "请输入正确的登陆服务器IP地址!!!", "确定");
                        return;
                    }
                    CheckConfigSet(CheckConfigState.ServerType);
                } break;
            case CheckConfigState.ServerType:
                {
                    if (CurrentBuildConfig.loginServerType == (int)EServerType.None)
                    {
                        if (EditorUtility.DisplayDialog("提示", "请选择一个请求的服务器列表类型!!!", "确定"))
                            return;
                    }
                    CheckConfigSet(CheckConfigState.VersionName);
                } break;
            case CheckConfigState.VersionName:
                {
                    if (string.IsNullOrEmpty(CurrentBuildConfig.versionName))
                    {
                        if (EditorUtility.DisplayDialog("提示", "请输入一个版本名称!", "确定"))
                            return;
                    }
                    CheckConfigSet(CheckConfigState.AssetPathStr);
                } break;
            case CheckConfigState.AssetPathStr:
                {
                    
                    CheckConfigSet(CheckConfigState.ProjectName);
                } break;
            //case CheckConfigState.AssetPath:
            //    {
            //        if (!Directory.Exists(CurrentBuildConfig.resPath))
            //        {
            //            if (EditorUtility.DisplayDialog("提示", "请选择对应本渠道的资源目录", "确定"))
            //                return;
            //        }
            //        CheckConfigSet(CheckConfigState.ProjectName);
            //    } break;
            case CheckConfigState.ProjectName:
                {
                    if (string.IsNullOrEmpty(CurrentBuildConfig.companyName) || string.IsNullOrEmpty(CurrentBuildConfig.fileName) || string.IsNullOrEmpty(CurrentBuildConfig.appName))
                    {
                        if (EditorUtility.DisplayDialog("提示", "请输入公司名、文件名以及应用名称!", "确定"))
                            return;
                    }
                    CheckConfigSet(CheckConfigState.SDKPath);
                } break;
            case CheckConfigState.SDKPath:
                {
                    if (!Directory.Exists(CurrentBuildConfig.androidSDKPath))
                    {
                        if (EditorUtility.DisplayDialog("提示", "请选择对应本渠道的SDK目录", "确定"))
                            return;
                    }
                    CheckConfigSet(CheckConfigState.BundleSet);
                } break;
            case CheckConfigState.BundleSet:
                {
                    if (string.IsNullOrEmpty(CurrentBuildConfig.bundleIdentifier) || string.IsNullOrEmpty(CurrentBuildConfig.bundleVersion) || CurrentBuildConfig.bundleVersionCode == 0)
                    {
                        if (EditorUtility.DisplayDialog("提示", "请输入应用包相关信息!", "确定"))
                            return;
                    }

                    CheckConfigSet(CheckConfigState.KeyStore);
                } break;
            case CheckConfigState.KeyStore:
                {
                    if (!File.Exists(CurrentBuildConfig.keystorePath))
                    {
                        if (EditorUtility.DisplayDialog("提示", "请选择对应本渠道的Keystore", "确定"))
                            return;
                    }
                    CheckConfigSet(CheckConfigState.KeyStoreName);
                } break;
            case CheckConfigState.KeyStoreName:
                {
                    if (string.IsNullOrEmpty(CurrentBuildConfig.keystorePassword) || string.IsNullOrEmpty(CurrentBuildConfig.keyName) || string.IsNullOrEmpty(CurrentBuildConfig.keyPassword))
                    {
                        if (EditorUtility.DisplayDialog("提示", "请选择对应本渠道的Keystore相关信息设置", "确定"))
                            return;
                    }
                    CommonTools.SaveBuildXml(BuildConfigDic);
                } break;
        }
    }


    private enum CheckConfigState
    {
        Channel,
        ResAddress,
        LoginAddress,
        ServerType,
        VersionName,
        AssetPathStr,
        AssetPath,
        ProjectName,
        SDKPath,
        BundleSet,
        KeyStore,
        KeyStoreName
    }


    [UnityEditor.Callbacks.DidReloadScripts]
    public static void OnUnityBuildComplete()
    {
        if (isShowBuildTip)
        {
            if (EditorUtility.DisplayDialog("提示", "资源导入以及代码修改完成,是否立即打包APK？", "是", "否"))
            {
                ApkBuilder.ShowApkBuildWindow();
                buildConfigWindow.Close();
                isShowBuildTip = false;
            }
        }
    }

    void OnDestory()
    {
        isShowBuildTip = false;
        OnShowBuildEditor = false;
    }
}

public class CodePath
{
    public static string AppPath = Application.dataPath;
    public const string AppConfigPath = "/Resources/Config/AppConfig.xml";
    public const string ApkVersionConfigPath = "/Resources/Config/ApkVersion.xml";

    public const string GlobalConstFilePath = "/Scripts/Common/GlobalConst.cs";
    public const string ResPathFilePath = "/Scripts/Common/ResPath.cs";
    public const string GuideManagerFilePath = "/Scripts/Managers/GuideManager.cs";
    public const string ResSavePath = "/StreamingAssets";


    //public const bool IS_OPEN_GUIDE = false;
    public static void ChangeCode(BuildConfig config)
    {
        ChangeFile(AppConfigPath, "<verType>.*</verType>", "<verType>" + 1 + "</verType>");
        ChangeFile(AppConfigPath, "<webip>.*</webip>", "<webip>" + config.loginAddress + "</webip>");
        ChangeFile(GlobalConstFilePath, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms." + (TargetPlatforms)config.channelType + ";");
        ChangeFile(GlobalConstFilePath, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = " + config.isOpenMD5.ToString().ToLower() + ";");
        ChangeFile(GlobalConstFilePath, "public const bool OPENDLC = .*;", "public const bool OPENDLC = " + config.isOpenDLC.ToString().ToLower() + ";");
        //ChangeFile(GlobalConstFilePath, "public const string VersionName = .*;", "public const string VersionName = \"" + config.versionName + "\";");
        ChangeFile(GlobalConstFilePath, "public const string VersionName = .*;", "public const string VersionName = \"" + string.Format("{0}_{1}", config.versionName, config.apkVersion) + "\";");
        ChangeFile(GlobalConstFilePath, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = " + config.isOpenCodeUpdate.ToString().ToLower() + ";");
        ChangeFile(ResPathFilePath, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"" + config.resAddress + "\";");
        ChangeFile(ResPathFilePath, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"" + config.resAndroidPath + "\";");
        ChangeFile(ResPathFilePath, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"" + config.resIOSPath + "\";");
        ChangeFile(ResPathFilePath, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"" + config.resWinEditorPath + "\";");
        ChangeFile(ResPathFilePath, "public const string OSXEDITORPATH = \".*\";", "public const string OSXEDITORPATH = \"" + config.resMacEditorPath + "\";");

        //ChangeFile(GuideManagerFilePath, "private bool openGuide = .*;", "private bool openGuide = " + config.isOpenGuide.ToString().ToLower() + ";");
        ChangeFile(GlobalConstFilePath, "public const bool IS_OPEN_GUIDE = .*;", "public const bool IS_OPEN_GUIDE = " + config.isOpenGuide.ToString().ToLower() + ";");

        ChangeFile(GlobalConstFilePath, "public const bool ISOPENSDKOPERATE = .*;", "public const bool ISOPENSDKOPERATE = " + config.isOpenSDK.ToString().ToLower() + ";");


        ChangeFile(ResPathFilePath, "public const bool ISOPENGM = \".*\";", "public const bool ISOPENGM = \"" + config.isOpenGM.ToString().ToLower() + "\";");
        ChangeFile(GlobalConstFilePath, "public const EServerType SERVER_TYPE = EServerType.*;", "public const EServerType SERVER_TYPE = EServerType." + ((EServerType)config.loginServerType).ToString() + ";");
        ChangeFile(ApkVersionConfigPath, "<apkversion>.*</apkversion>", string.Format("<apkversion>{0}</apkversion>", config.apkVersion));
    }


    private static void ChangeFile(string fitleName, string Vsrc, string Vdest)
    {
        if (File.Exists(AppPath + fitleName))
        {
            string str = File.ReadAllText(AppPath + fitleName);
            str = System.Text.RegularExpressions.Regex.Replace(str, Vsrc, Vdest);
            File.WriteAllText(AppPath + fitleName, str);
        }
    }

}
