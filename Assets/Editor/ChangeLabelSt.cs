using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

public class LabelData
{
    public Color mColor = new Color(111 / 255f, 52 / 255f, 14 / 255f);
    public Color mOutlineColor = new Color(234 / 255f, 201 / 255f, 93 / 255f);
    public Vector2 mEffectDiatance = new Vector2(1f, 1f);
    public int mSpancingX = -1;
    public int mSpancingy = 0;
    public UILabel.Effect mEffectStyle = UILabel.Effect.Outline;
}

public class ChangeLabelSt
{
    public static LabelData data = new LabelData();
    [MenuItem("Tools/Change Lable Style")]
    static void ChangeLabelStyle()
    {
        LabelData data = new LabelData();
        GameObject[] objs = Selection.gameObjects;
        foreach (GameObject go in objs)
        {
            UILabel label = go.GetComponent<UILabel>();
            if (label != null)
            {
                ChangeLabel(label);
            }
        }
    }

    public static void ChangeLabel(UILabel label)
    {
        label.color = data.mColor;
        label.effectStyle = data.mEffectStyle;
        label.effectColor = data.mOutlineColor;
        label.applyGradient = false;
        label.effectDistance = data.mEffectDiatance;
        label.spacingX = data.mSpancingX;
        label.spacingY = data.mSpancingy;
    }
    
}

