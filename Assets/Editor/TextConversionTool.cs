using UnityEngine;
using System.Collections;
using System.Runtime.InteropServices;
using System.Text;
using System.IO;
using UnityEditor;
public class TextConversionTool
{
    private static bool isConversionSimple = false;

    [MenuItem("PXTools/繁转简")]
    public static void ToSimple()
    {
        isConversionSimple = true;
        StartConversion();
    }

    [MenuItem("PXTools/简转繁")]
    public static void ToTraditional()
    {
        isConversionSimple = false;
        StartConversion();
    }

    private static void StartConversion()
    {
        bool importDLL = ImportDLL();
        if (!importDLL)
        {
            return;
        }
        CheckSelectAsset();
    }

    private static bool ImportDLL()
    {
        string dllname = "kernel32.dll";
        string toPath = Application.dataPath + "/Plugins/Editor/";
        if (!Directory.Exists(toPath))
        {
            Directory.CreateDirectory(toPath);
        }
        if (File.Exists(toPath + dllname))
        {
            return true;
        }
        string fromPath = System.Environment.SystemDirectory + "\\";
        if (!File.Exists(fromPath + dllname))
        {
            Debug.LogError("can not find " + dllname + " by " + fromPath);
            return false;
        }
        File.Copy(fromPath + dllname, toPath + dllname, true);
        AssetDatabase.Refresh();
        return true;
    }

    private static void CheckSelectAsset()
    {
        Object[] obj_Array = Selection.GetFiltered(typeof(Object), SelectionMode.DeepAssets);
        if (obj_Array == null || obj_Array.Length == 0)
            return;
        int index = 0;
        float length = obj_Array.Length;
        string path = string.Empty;
        foreach (Object obj in obj_Array)
        {
            path = AssetDatabase.GetAssetPath(obj);
            if (obj is GameObject)
            {
                ReplacePrefabText((GameObject)obj);
            }
            else if (obj is MonoScript)
            {
                ReplaceTextAsset(obj);
            }
            else if (obj is TextAsset)
            {
                ReplaceTextAsset(obj);
            }
            index++;
            UnityEditor.EditorUtility.DisplayProgressBar("Hold On", path, index / length);
        }
        UnityEditor.EditorUtility.ClearProgressBar();
        UnityEditor.EditorUtility.DisplayDialog("提示", "转换完成", "确定");
        AssetDatabase.Refresh();
    }

    private static void ReplacePrefabText(GameObject go)
    {
        UILabel[] lbl_Array = go.GetComponentsInChildren<UILabel>(true);
        for (int i = 0; i < lbl_Array.Length; i++)
        {
            UILabel lbl = lbl_Array[i];
            if (lbl == null)
                continue;
            string fromTxt = lbl.text;
            if (isConversionSimple)
            {
                lbl.text = ConsersionTool.ToSimplified(fromTxt);
            }
            else
            {
                lbl.text = ConsersionTool.ToTraditional(fromTxt);
            }
        }
        EditorUtility.SetDirty(go);
    }

    private static void ReplaceTextAsset(Object obj)
    {
        string scriptPath = AssetDatabase.GetAssetPath(obj.GetInstanceID());
        if (File.Exists(scriptPath))
        {
            string fromText = File.ReadAllText(scriptPath);
            string toText;
            if (isConversionSimple)
            {
                toText = ConsersionTool.ToSimplified(fromText);
            }
            else
            {
                toText = ConsersionTool.ToTraditional(fromText);
            }
            File.WriteAllText(scriptPath, toText);
        }
    }
}


public class ConsersionTool
{

    /// <summary>
    /// 中文字符工具类
    /// </summary>
    private const int LOCALE_SYSTEM_DEFAULT = 0x0800;
    private const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
    private const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

    [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
    private static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

    /// <summary>
    /// 将字符转换成简体中文
    /// </summary>
    /// <param name="source">输入要转换的字符串</param>
    /// <returns>转换完成后的字符串</returns>
    public static string ToSimplified(string source)
    {
        System.String tTarget = new System.String(' ', source.Length);
        int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, source, source.Length, tTarget, source.Length);
        return tTarget;

    }

    /// <summary>
    /// 讲字符转换为繁体中文
    /// </summary>
    /// <param name="source">输入要转换的字符串</param>
    /// <returns>转换完成后的字符串</returns>
    public static string ToTraditional(string source)
    {
        System.String tTarget = new System.String(' ', source.Length);
        int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, source, source.Length, tTarget, source.Length);
        return tTarget;
    }
}
