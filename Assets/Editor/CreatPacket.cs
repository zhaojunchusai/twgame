using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
public class CreatPacket : EditorWindow
{
    public static CreatPacket window;
    string AppDir = Application.dataPath;
    
    string APPCONFIGPAHT = "/Resources/Config/AppConfig.xml";
    string APKVERTIONPAHT = "/Resources/Config/ApkVersion.xml";
    string GLOBCONSTPAHT = "/Scripts/Common/GlobalConst.cs";

    string RESPPATH = "/Scripts/Common/ResPath.cs";
    string GUIDEMANAGEPAHT = "/Scripts/Managers/GuideManager.cs";

    string TDFILEPATH = "/StreamingAssets/TD/Assetbundle/";
    string TDTESTFILEPATH = "/StreamingAssets/TDTest/TD/Assetbundle/";
    string TDBFILEPATH = "/StreamingAssets/TDB/TD/Assetbundle/";
    string TEMPFILEPATH = "/StreamingAssets/";
    string WINDOWSFILENAME = "Editor";
    string ANDROIDFILENAME = "Android";
    string SOURCEFILE = "Android";

    string APKPATH = "E:/LTDAPK/";
    //string APKNAME = "BLCX_ANDROID";
    string APKNAME = "QMZSG_ANDROID";
    string APKNET = "";
    string APKVERTION = "";
    string APKEnd = ".apk";
    int APKVertion = 0;
    [MenuItem("Tools/CreatePacket")]
    public static void ExecuteTargetBuild()
    {
        if (window == null)
        {
            window = (CreatPacket)GetWindow(typeof(CreatPacket));
        }
        //window.minSize = new Vector2(300,450);
        window.Show();
    }
    void OnGUI()
    {
        if (GUI.Button(new Rect(10f, 310, 200f, 50f), "NeiWang249"))
        {
            this.APKNET = "_LAN_249";
            this.ChangeFile(APPCONFIGPAHT,"<verType>.</verType>", "<verType>0</verType>");
            this.ChangeFile(APPCONFIGPAHT,"<webip_local>.*</webip_local>", "<webip_local>http://192.168.0.249:10201/</webip_local>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.Android_SF;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"SF_249\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Nei249_10201;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://192.168.0.252:81/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");
            
        }
        if (GUI.Button(new Rect(10f, 70f, 200f, 50f), "NeiWang252"))
        {
            this.APKNET = "_LAN_252";

            this.ChangeFile(APPCONFIGPAHT, "<verType>.</verType>", "<verType>0</verType>");
            this.ChangeFile(APPCONFIGPAHT, "<webip_local>.*</webip_local>", "<webip_local>http://192.168.0.252:10201/</webip_local>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.Android_SF;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"SF_252\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Nei252_10201;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://192.168.0.252:81/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TDTest/TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TDTest/TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TDTest/TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDTESTFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");

           
        }
        if (GUI.Button(new Rect(10f, 130f, 200f, 50f), "WaiWang"))
        {
            this.APKNET = "_WWW";

            this.ChangeFile(APPCONFIGPAHT, "<verType>.</verType>", "<verType>1</verType>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.None;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"None_1.0\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = false;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Wai_BeiJing;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://182.92.64.167:80/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");
        }
        if (GUI.Button(new Rect(10f, 190f, 200f, 50f), "WaiWangSF_TD"))
        {
            this.APKNET = "_WWW_SFTD";

            this.ChangeFile(APPCONFIGPAHT, "<verType>.</verType>", "<verType>2</verType>");
            this.ChangeFile(APPCONFIGPAHT, "<webip_SF>.*</webip_SF>", "<webip_SF>http://218.93.248.115:10201/</webip_SF>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.Android_SF;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"Android_SF_218\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = false;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Wai_Sifu;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://218.93.248.115:8080/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");
        }
        if (GUI.Button(new Rect(210f, 190f, 200f, 50f), "WaiWangSF_TDTEXT"))
        {
            this.APKNET = "_WWW_SFTDTEXT";

            this.ChangeFile(APPCONFIGPAHT, "<verType>.</verType>", "<verType>2</verType>");
            this.ChangeFile(APPCONFIGPAHT, "<webip_SF>.*</webip_SF>", "<webip_SF>http://218.93.248.115:20201/</webip_SF>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.None;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"None_SF_218\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = false;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Wai_Sifu;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://218.93.248.115:8080/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TDTest/TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TDTest/TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TDTest/TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDTESTFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");
        }
        if (GUI.Button(new Rect(10f, 250f, 200f, 50f), "WaiWangSF_TDB"))
        {
            this.APKNET = "_WWW_SFTDB";

            this.ChangeFile(APPCONFIGPAHT, "<verType>.</verType>", "<verType>2</verType>");
            this.ChangeFile(APPCONFIGPAHT, "<webip_SF>.*</webip_SF>", "<webip_SF>http://218.93.248.115:31201/</webip_SF>");
            this.ChangeFile(GLOBCONSTPAHT, "TargetPlatforms PLATFORM = TargetPlatforms.*;", "TargetPlatforms PLATFORM = TargetPlatforms.Android_SF;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENMD5 = .*;", "public const bool OPENMD5 = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool OPENDLC = .*;", "public const bool OPENDLC = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const string VersionName = .*;", "public const string VersionName = \"None_SF_218\";");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENCODEUPDATE = .*;", "public const bool ISOPENCODEUPDATE = true;");
            this.ChangeFile(GLOBCONSTPAHT, "public const bool ISOPENGM = .*;", "public const bool ISOPENGM = false;");
            this.ChangeFile(GLOBCONSTPAHT, "public const EServerType SERVER_TYPE = .*;", "public const EServerType SERVER_TYPE = EServerType.Wai_SifuB;");

            this.ChangeFile(RESPPATH, "public const string CHECKVERSIONADDRESS = \".*\";", "public const string CHECKVERSIONADDRESS = \"http://218.93.248.115:8080/\";");
            this.ChangeFile(RESPPATH, "public const string ANDROIDPATH = \".*\";", "public const string ANDROIDPATH = \"TDB/TD/Assetbundle/Android/\";");
            this.ChangeFile(RESPPATH, "public const string IPHONEPATH = \".*\";", "public const string IPHONEPATH = \"TDB/TD/Assetbundle/Iphone/\";");
            this.ChangeFile(RESPPATH, "public const string OTHERPATH = \".*\";", "public const string OTHERPATH = \"TDB/TD/Assetbundle/Editor/\";");
            this.ChangeFile(GUIDEMANAGEPAHT, "private bool openGuide = .*;", "private bool openGuide = true;");
            if (this.GetFile(TDBFILEPATH + SOURCEFILE))
                EditorUtility.DisplayDialog("", "Completed", "OK");
            else
                EditorUtility.DisplayDialog("", "Fail", "OK");
        }
        if (GUI.Button(new Rect(10f, 10, 80, 50f), "Windows"))
        {
            this.SOURCEFILE = WINDOWSFILENAME;
            EditorUtility.DisplayDialog("", "Windows", "OK");
        }
        if (GUI.Button(new Rect(100f, 10, 80, 50f), "Android"))
        {
            this.SOURCEFILE = ANDROIDFILENAME;
            EditorUtility.DisplayDialog("", string.Format("Android",this.APKVertion), "OK");
        }
        if (GUI.Button(new Rect(10, 370, 200, 50f), "CreateAndroid"))
        {
            this.GetApkVertion();

            this.StartBuild();
            EditorUtility.DisplayDialog("", "CreatPacket Completed", "OK");
        }
        if (GUI.Button(new Rect(10, 450, 200, 50f), "++APKVertion"))
        {
            this.GetApkVertion();

            this.ChangeFile(APKVERTIONPAHT, "<apkversion>.*</apkversion>", string.Format("<apkversion>{0}</apkversion>",++this.APKVertion));

            EditorUtility.DisplayDialog("", "Completed", "OK");
        }
    }
    void ChangeFile(string fitleName, string Vsrc, string Vdest)
    {
        if (File.Exists(AppDir + fitleName))
        {
            string str = File.ReadAllText(AppDir + fitleName);
            str = Regex.Replace(str, Vsrc, Vdest);
            File.WriteAllText(AppDir + fitleName, str);
        }
    }
    bool MoveFile(string path)
    {
        if (Directory.Exists(AppDir + path))
        {
            Directory.Delete(AppDir + path, true);
        }
        if (Directory.Exists(AppDir+TEMPFILEPATH + SOURCEFILE))
        {
            Directory.Move(AppDir + TEMPFILEPATH + SOURCEFILE, AppDir + path);
            return true;
        }
        return false;
    }
    void CleanFile()
    {
        if (Directory.Exists(AppDir + TEMPFILEPATH + WINDOWSFILENAME))
        {
            Directory.Delete(AppDir + TEMPFILEPATH + WINDOWSFILENAME, true);
        }

        if (Directory.Exists(AppDir + TEMPFILEPATH + ANDROIDFILENAME))
        {
            Directory.Delete(AppDir + TEMPFILEPATH + ANDROIDFILENAME, true);
        }
    }
    bool GetFile(string path)
    {
        this.CleanFile();
        if (Directory.Exists(AppDir + TDFILEPATH + WINDOWSFILENAME))
        {
            Directory.Move(AppDir + TDFILEPATH + WINDOWSFILENAME, AppDir + TEMPFILEPATH + SOURCEFILE);
        }
        else if (Directory.Exists(AppDir + TDFILEPATH + ANDROIDFILENAME))
        {
            Directory.Move(AppDir + TDFILEPATH + ANDROIDFILENAME, AppDir + TEMPFILEPATH + SOURCEFILE);
        }
        else if (Directory.Exists(AppDir + TDTESTFILEPATH + WINDOWSFILENAME))
        {
             Directory.Move(AppDir + TDTESTFILEPATH + WINDOWSFILENAME, AppDir + TEMPFILEPATH + SOURCEFILE);
        }
        else if (Directory.Exists(AppDir + TDTESTFILEPATH + ANDROIDFILENAME))
        {
              Directory.Move(AppDir + TDTESTFILEPATH + ANDROIDFILENAME, AppDir + TEMPFILEPATH + SOURCEFILE);
        }
        else if (Directory.Exists(AppDir + TDBFILEPATH + ANDROIDFILENAME))
        {
              Directory.Move(AppDir + TDBFILEPATH + ANDROIDFILENAME, AppDir + TEMPFILEPATH + SOURCEFILE);
        }

        if(this.MoveFile(path))
        {
            this.CleanFile();
            return true;
        }
        return false;
    }
    void StartBuild()
    {
        this.APKVERTION = string.Format("_{0}.0", APKVertion);
        string[] scenes = FindEnabledEditorScenes();
        if (Directory.Exists(this.APKPATH))
        {
            if (File.Exists(this.APKNAME + this.APKNET + this.APKVERTION + this.APKEnd))
            {
                File.Delete(this.APKNAME + this.APKNET + this.APKVERTION + this.APKEnd);
            }
        }
        else
        {
            Directory.CreateDirectory(this.APKPATH);
        }
        PlayerSettings.keyaliasPass = "android";
        PlayerSettings.keystorePass = "android";
        PlayerSettings.bundleIdentifier = "com.pixone.LTD";
        PlayerSettings.bundleVersion = string.Format("Alpaha{0}.0",APKVertion);
        this.GenericBuild(scenes, this.APKPATH + this.APKNAME + this.APKNET + this.APKVERTION + this.APKEnd, BuildTarget.Android, BuildOptions.None);
    }
    private static string[] FindEnabledEditorScenes()
    {
        List<string> EditorScenes = new List<string>();
        foreach (EditorBuildSettingsScene scene in EditorBuildSettings.scenes)
        {
            if (!scene.enabled) continue;
            EditorScenes.Add(scene.path);
        }
        return EditorScenes.ToArray();
    }
    bool GenericBuild(string[] scenes, string target_dir, BuildTarget build_target, BuildOptions build_options)
    {
        EditorUserBuildSettings.SwitchActiveBuildTarget(build_target);
        string res = BuildPipeline.BuildPlayer(scenes, target_dir, build_target, build_options);

        if (res.Length > 0)
        {
            throw new Exception("BuildPlayer failure: " + res);
        }
        return true;
    }
    void GetApkVertion()
    {
        if (File.Exists(AppDir + APKVERTIONPAHT))
        {
            string str = File.ReadAllText(AppDir + APKVERTIONPAHT);
            Match math = Regex.Match(str, "<apkversion>.*</apkversion>");
            str = math.Value;

            str = Regex.Replace(str, "<apkversion>", "");
            str = Regex.Replace(str, "</apkversion>", "");

            this.APKVertion = int.Parse(str);
        }
    }
}
