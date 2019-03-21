using UnityEngine;
using System.Collections;
using System;
public class DebugUtil : MonoBehaviour
{
    public static string tmpShowStr = "";
    public uint i = 45000001;

    public static void Log(string str) 
    {
        Debug.Log(str);
    }

    public static void LogException(Exception e) {
        Debug.Log(e.StackTrace);
    }

    public static void LogWarining(string str)
    {
        Debug.LogWarning(str);
    }

    public static void LogError(string str)
    {
        Debug.LogError(str);
    }



    private GUIStyle textStyle;
    void Start()
    {
        textStyle = new GUIStyle();
        textStyle.fontSize = 25;
        textStyle.normal.textColor = Color.green;
    }

    void OnGUI()
    {
        //if (GUI.Button(new Rect(100, 100, 100, 100), "A"))
        //{
        //    SDKManager.Instance.SDKOperate_DepositOperate("", "0", 0);
        //}
        //if (GUI.Button(new Rect(200, 100, 100, 100), "B"))
        //{
        //    SDKManager.Instance.SDKOperate_DepositOperate("", "0", 1);
        //}
        //if (GUI.Button(new Rect(300, 100, 100, 100), "C"))
        //{
        //    SDKManager.Instance.SDKOperate_DepositOperate("", "0", 0, 1);
        //}
        //if (GUI.Button(new Rect(400, 100, 100, 100), "D"))
        //{
        //    SDKManager.Instance.SDKOperate_DepositOperate("", "0", 1, 1);
        //}

        //if (GUI.Button(new Rect(100, 20, 50, 50), "add")) { }
        GUI.Label(new Rect(20, 60, 300, 50), tmpShowStr, textStyle);
    }

}
