using UnityEngine;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System;
public class SavePosition : MonoBehaviour {


    [MenuItem("PXTools/Stage/SavePosition")]
    public static void SaveObjectPosition()
    {
        UnityEngine.Object[] array = Selection.objects;
        string text = string.Empty;
        for (int i = 0; i < array.Length; i++)
        {
            GameObject go = array[i] as GameObject;
            text += go.name + "------";
            foreach (Transform tmp in go.transform) 
            {
                int x = Mathf.CeilToInt(tmp.localPosition.x);
                int y = Mathf.CeilToInt(tmp.localPosition.y);
                text += x + ":";
                text += y + ";";
            }
            text += "\r\n";
        }
        WriteTxtFile(ExportCommand.AssetbundlePath + "position.txt", text);
        Debug.Log("save position success");
    }
    [MenuItem("PXTools/Stage/Create Point")]
    private static  void CreatePoint()
    {
        UnityEngine.Object[] array = Selection.objects;
        if (array.Length > 1)
            return;
        UnityEngine.Object obj = array[0];
        GameObject icon_go = obj as GameObject;
        List<Transform> list = new List<Transform>();
        foreach (Transform tmp in icon_go.transform)
        {
            list.Add(tmp);
        }
        GameObject parent = new GameObject(icon_go.name);
        for (int i = 0; i < list.Count; i++)
        {
            if (i != 0)
            {
                Transform go_orig = list[i - 1];
                Transform go_target = list[i];
                List<GameObject> pointlist = new List<GameObject>();
                Vector2 orig = new Vector2(go_orig.localPosition.x, go_orig.localPosition.y);
                Vector2 target = new Vector2(go_target.localPosition.x, go_target.localPosition.y);
                float distance = Vector2.Distance(orig, target);
                float split_d = 16 * 1.5f;
                int point_count = 2;
                point_count = (int)(distance / split_d);
                for (int pi = 0; pi < point_count; pi++)
                {
                    GameObject tempgo = new GameObject();
                    tempgo.name = i + "_" + pi;
                    tempgo.transform.parent = parent.transform;
                    tempgo.gameObject.SetActive(true);
                    Vector2 myvec = (target - orig).normalized;
                    tempgo.transform.localPosition = new Vector3(orig.x, orig.y, 0) + new Vector3(myvec.x, myvec.y, 0) * ((pi + 1) * split_d);
                    pointlist.Add(tempgo);
                }

            }
        }
    }
      
    /// <summary>
    /// 把文本文件 写到磁盘上 
    /// </summary>
    /// <param name="path"></param>
    /// <param name="data"></param>
    public static void WriteTxtFile(string path, string data)
    {
        FileStream fs = null;
        StreamWriter sw = null;
        try
        {
            string strFile = path;
            bool fileExist = false;

            if (File.Exists(strFile))
            {
                fileExist = true;
                fs = new FileStream(strFile, FileMode.Create);
            }
            else
            {
                fileExist = false;
                fs = new FileStream(strFile, FileMode.Create);
            }
            
            sw = new StreamWriter(fs);
            sw.WriteLine(data);
            sw.Flush();
        }
        catch (Exception e)
        {
            Debug.LogError("Save file failed :" + "[ " + e.ToString() + " ]");
        }
        finally
        {
            if (null != sw)
            {
                sw.Close();
            }

            if (null != fs)
            {
                fs.Close();
            }
        }
    }
}
