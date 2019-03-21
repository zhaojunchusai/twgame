using System.Collections.Generic;
using UnityEngine;
public enum LayerMaskEnum
{
    /// <summary>
    /// 战斗场景 物件
    /// </summary>
    UI = 5,
    /// <summary>
    /// 战斗特效 血条
    /// </summary>
    Fight = 8,
    /// <summary>
    /// 界面
    /// </summary>
    NGUI = 9,
}

public class LayerMaskTool 
{
    public static LayerMask GetLayerMask(LayerMaskEnum layer) 
    {
        return 1 << (int)layer;
    }

    public static string GetLayerName(LayerMaskEnum layer) 
    {
        return LayerMask.LayerToName((int)layer);
    }

    public static LayerMaskEnum GetLayerMaskEnum(GameObject go) 
    {
        return (LayerMaskEnum)go.layer;
    }
}

