/****************************
 * 
 * 
 * 
 * 
 * *******************************/

using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Text;
using System.IO;

public class ResourceCheck{

    static List<string> oldAtlasName = new List<string>{ 
        "CommonAtlas_1", "CommonAtlas_2", "CommonAtlas_3",
        "CommonAtlas_4", "CommonAtlas_5", "CommonAtlas_6", 
        "CommonAtlas_7","CommonAtlas_8","CommonAtlas_9",
        "CommonAtlas_10","CommonAtlas_11"
    };

    /// <summary>
    /// 该名字等于文件夹名和prefab名字
    /// </summary>
    static List<string> newAtlasName = new List<string>{ 
        "CommonAtlas_New_1", "CommonAtlas_New_2", "CommonAtlas_New_3",
        "CommonAtlas_New_4", "CommonAtlas_New_5", "CommonAtlas_New_6"
        , "CommonAtlas_New_7", "CommonAtlas_New_8"
    };

    private static List<UIAtlas> newAtlas = new List<UIAtlas>();
    private static List<string> oldAtlasPaths = new List<string>();
    private static List<string> noneSptPaths = new List<string>();
    private static string texAssetsPath = "Assets/AssetRes/Texture/UITexture/{0}/{1}.png";

    [MenuItem("Tools/Change Old Resource")]
    private static void CheckOldResource()
    {
        if(!LoadNewAtlas())
        {
            return;
        }
        
        oldAtlasPaths.Clear();
        noneSptPaths.Clear();
        GameObject[] prefabs = Selection.gameObjects;

        for (int i = 0; i < prefabs.Length; i++)
        {
            CheckGameObj(prefabs[i].transform, "");
            EditorUtility.SetDirty(prefabs[i]);
        }

        StringBuilder sb = new StringBuilder();

        for (int i = 0; i < noneSptPaths.Count; i++)
        {
            sb.Append(noneSptPaths[i]);
            sb.Append("\r\n");
            Debug.Log(noneSptPaths[i]);
        }
        for (int i = 0; i < oldAtlasPaths.Count; i++)
        {
            sb.Append(oldAtlasPaths[i]);
            sb.Append("\r\n");
            Debug.Log(oldAtlasPaths[i]);
        }

        OutPutLog(sb);
        AssetDatabase.Refresh();
    }
    
    private static void CheckGameObj(Transform go,string path)
    {
        if(go.gameObject.name.ToLower() == "itempoint")
        {
            return;
        }

        UISprite spt = go.GetComponent<UISprite>();
        if(spt != null)
        {
            if (spt.atlas == null)
            {
                noneSptPaths.Add(string.Format("spt.atlas == null \n{0}{1}", path, go.gameObject.name));
            }
            else
            {
                if (oldAtlasName.Contains(spt.atlas.name))
                {
                    if (!SetNewAtlas(spt))
                    {
                        oldAtlasPaths.Add(string.Format("{2} {3} :{0}{1}",
                            path, go.gameObject.name, spt.atlas.name, spt.spriteName));
                       // spt.atlas = null;
                        
                        if (AssetDatabase.LoadAssetAtPath(string.Format(texAssetsPath, "Tmp", spt.spriteName),typeof(Texture)) == null)
                        {
                            AssetDatabase.CopyAsset(
                            string.Format(texAssetsPath, string.Format("CommonAtlasTex_{0}", GetIndexNum(spt.atlas.name)), spt.spriteName)
                            , string.Format(texAssetsPath, "Tmp", spt.spriteName)
                            );
                        }
                    }
                }
            }
        }

        if(go.childCount >0)
        {
            for (int i = 0; i < go.childCount; i++)
            {
                CheckGameObj(go.GetChild(i), path + go.gameObject.name +"/");
            }
        }
    }

    private static bool SetNewAtlas(UISprite spt)
    {
        for (int i = 0; i < newAtlas.Count; i++)
        {
            string name = spt.spriteName;
            if(newAtlas[i].GetSprite(name) != null)
            {
                spt.atlas = newAtlas[i];
                spt.spriteName = name;
                return true;
            }
        }
        return false;
    }

    private static string GetIndexNum(string name)
    {
        string result = string.Empty;
        result = name.Substring(name.LastIndexOf('_') +1);
        return result;
    }

    private static bool LoadNewAtlas()
    {
        if (newAtlas.Count <= 0)
        {
            for (int i = 0; i < newAtlasName.Count; i++)
            {
                UIAtlas tmp = (AssetDatabase.LoadAssetAtPath(
                    string.Format("Assets/AssetRes/Atlas/Views/{0}/{0}.prefab", newAtlasName[i]),
            typeof(GameObject)) as GameObject).GetComponent<UIAtlas>();
                if (tmp == null)
                {
                    Debug.Log(string.Format("UIAtlas({0}) == null", newAtlasName[i]));
                    newAtlas.Clear();
                    return false;
                }
                else
                {
                    newAtlas.Add(tmp);
                }
            }
        }
        return true;
    }

    private static void OutPutLog(StringBuilder sb)
    {
        string outputPath = Application.dataPath +
        string.Format("/Log/UIChangeLog/{0:D2}_{1:D2}_{2:D2}_{3:D2}.txt",
            System.DateTime.Now.Month,System.DateTime.Now.Day,
            System.DateTime.Now.Hour,System.DateTime.Now.Minute);
        if(!Directory.Exists(Application.dataPath +"/Log/UIChangeLog"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Log/UIChangeLog");
        }
        byte[] logText = Encoding.Convert(Encoding.Unicode, Encoding.UTF8, Encoding.Unicode.GetBytes(sb.ToString()));
        File.WriteAllBytes(outputPath, logText);
    }

}
