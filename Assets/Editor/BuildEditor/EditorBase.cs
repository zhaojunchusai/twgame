using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EditorBase : EditorWindow
{
    protected bool OnShowBuildEditor = false;
    protected static Dictionary<string, BuildConfig> BuildConfigDic = new Dictionary<string, BuildConfig>();

    protected GUIStyle titleStyle = new GUIStyle();
    protected GUIStyleState titleState = new GUIStyleState();
    protected static float totalWidth = 650;
    protected float btnWidth = 100;
    protected float labelWidth = 200;
    protected float fieldWidth = 150;
    protected float border = 4;

    protected virtual void OnEnabling() { }

    protected void LeftNameLabel(string name, float offsetWidth = 0)
    {
        EditorGUILayout.LabelField(name, GUILayout.Width(labelWidth + offsetWidth));
    }

    protected void AddTitleLabel(string title)
    {
        EditorGUILayout.Space();
        EditorGUILayout.LabelField(new GUIContent(title), titleStyle);
        EditorGUILayout.Space();
    }

    void OnEnable()
    {
        OnShowBuildEditor = false;
        BuildConfigDic.Clear();
        titleState.textColor = Color.white;
        titleStyle.normal = titleState;
        titleStyle.fontStyle = FontStyle.Bold;
        CommonTools.LoadBuildXml(ref BuildConfigDic);
        OnEnabling();
        OnShowBuildEditor = true;
    }
}
