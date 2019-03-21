using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;

public class AssetBundleWindowController : EditorWindow
{
    public static AssetBundleWindowController window;

    [MenuItem("PXTools/AssetBundle", false, 1)]
    public static void ExecuteTargetBuild()
    {
        if (window == null)
        {
            window = (AssetBundleWindowController)GetWindow(typeof(AssetBundleWindowController));
        }
        window.Show();
    }




    void OnGUI()
    {
        if (GUI.Button(new Rect(10f, 10f, 200f, 50f), "(1)CreateAssetBundle"))
        {
//            ExportCommand.TargetPlatform = EditorUserBuildSettings.activeBuildTarget;
////#if UNITY_STANDALONE
////        ExportCommand.TargetPlatform  = BuildTarget.StandaloneWindows;
////#elif UNITY_IPHONE
////        ExportCommand.TargetPlatform = BuildTarget.iPhone;
////#elif UNITY_ANDROID
////            ExportCommand.TargetPlatform = BuildTarget.Android;
////#elif UNITY_WEBPLAYER
////        ExportCommand.TargetPlatform  = BuildTarget.WebPlayer;
////#elif UNITY_WP8
////        ExportCommand.TargetPlatform  = BuildTarget.WP8Player;
////#endif
            ExportBundles.ExecuteCurrent();
            EditorUtility.DisplayDialog("", "CreateAssetBundle Completed", "OK");
        }
        if (GUI.Button(new Rect(10f, 80f, 200f, 50f), "(2)Generate MD5"))
        {
            CreateMD5ResourceList.Execute();
            EditorUtility.DisplayDialog("", "Step (2) Completed", "OK");
        }
        if (GUI.Button(new Rect(10f, 150f, 200f, 50f), "(3)Compare MD5"))
        {
            CampareMD5ResourceList.Execute();
            EditorUtility.DisplayDialog("", "Step (3) Completed", "OK");
        }
    }
}


public class CreateMD5Controller : EditorWindow
{
    public static CreateMD5Controller window;

    private static Object[] selectObjArray = null;
    private static bool isCreateMD5 = false;
    private static Dictionary<string, string> md5_dic;
    [MenuItem("PXTools/CreateMD5", false, 1)]
    public static void ExecuteTargetBuild()
    {
        isCreateMD5 = false;
        if (window == null)
        {
            window = (CreateMD5Controller)GetWindow(typeof(CreateMD5Controller));
        }
        window.Show();
    }

    void OnGUI()
    {
        if (GUI.Button(new Rect(10, 10, 200, 50), "Create MD5"))
        {
            selectObjArray = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
            isCreateMD5 = true;
            md5_dic = new Dictionary<string, string>();
            for (int i = 0; i < selectObjArray.Length; i++)
            {
                Object obj = selectObjArray[i];
                MD5CryptoServiceProvider md5Generator = new MD5CryptoServiceProvider();
                string filePath = AssetDatabase.GetAssetPath(obj);
                FileStream file = new FileStream(filePath, FileMode.Open, FileAccess.Read, FileShare.Read);
                byte[] hash = md5Generator.ComputeHash(file);
                string strMD5 = System.BitConverter.ToString(hash);
                file.Close();
                md5_dic.Add(obj.name, strMD5);
            }
        }
        if (isCreateMD5)
        {
            if (selectObjArray != null && selectObjArray.Length > 0)
            {
                float height = selectObjArray.Length * 20f;
                GUI.BeginGroup(new Rect(10, 70, 600, height));
                for (int i = 0; i < selectObjArray.Length; i++)
                {
                    Object obj = selectObjArray[i];
                    string strMD5 = string.Empty;
                    if (md5_dic.ContainsKey(obj.name))
                    {
                        strMD5 = md5_dic[obj.name];
                    }
                    EditorGUILayout.TextField(obj.name, strMD5);
                }
                GUI.EndGroup();
            }
        }
    }

    void OnDestory()
    {
        isCreateMD5 = false;
        selectObjArray = null;
        md5_dic = null;
    }
}