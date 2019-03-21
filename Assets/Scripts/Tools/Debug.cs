//********************************************************
//用于在非editor情况下屏蔽 除error以外的错误提示
//
//
//********************************************************

#if !UNITY_EDITOR
using System;
using System.ComponentModel;
using UnityEngine;

public class Debug
{
    public static bool isDebugBuild {
        get { return UnityEngine.Debug.isDebugBuild; }
    }
    public static void DrawLine(Vector3 start, Vector3 end, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        //UnityEngine.Debug.DrawLine(start, end, color, duration, depthTest);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color, float duration)
    {
        //UnityEngine.Debug.DrawLine(start, end, color, duration);
    }

    public static void DrawLine(Vector3 start, Vector3 end, Color color)
    {
        //UnityEngine.Debug.DrawLine(start, end, color);
    }

    public static void DrawLine(Vector3 start, Vector3 end)
    {
        //UnityEngine.Debug.DrawLine(start, end);
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color, float duration)
    {
        //UnityEngine.Debug.DrawRay(start, dir, color, duration);
    }

    public static void DrawRay(Vector3 start, Vector3 dir, Color color)
    {
        //UnityEngine.Debug.DrawRay(start, dir, color);
    }

    public static void DrawRay(Vector3 start, Vector3 dir)
    {
        //UnityEngine.Debug.DrawRay(start, dir);
    }

    public static void DrawRay(Vector3 start, Vector3 dir, [DefaultValue("Color.white")] Color color, [DefaultValue("0.0f")] float duration, [DefaultValue("true")] bool depthTest)
    {
        //UnityEngine.Debug.DrawRay(start, dir, color, duration, depthTest);
    }

    public static void Log(object message)
	{
        //UnityEngine.Debug.Log(message);
	}

    public static void Log(object message, UnityEngine.Object context)
    {
        //UnityEngine.Debug.Log(message, context);
    }

    public static void LogWarning(object message)
    {
        //UnityEngine.Debug.LogWarning(message);
    }

    public static void LogWarning(object message, UnityEngine.Object context)
    {
        //UnityEngine.Debug.LogWarning(message, context);
    }

    public static void LogError(object message, UnityEngine.Object context)
    {
        UnityEngine.Debug.LogError(message,context);
    }

    public static void LogError(object message)
    {
        UnityEngine.Debug.LogError(message);
    }
    
}
#endif