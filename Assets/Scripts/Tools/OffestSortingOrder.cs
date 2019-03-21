using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 挂载在UI上需要进行Order排序的物件上面  add by taiwei
/// </summary>
public class OffestSortingOrder : MonoBehaviour
{
    /// <summary>
    /// UI的类型枚举
    /// </summary>
    public enum NGUILayerEnum   
    {
        UI = 1,
        /// <summary>
        /// 角色
        /// </summary>
        Role = 2,
        /// <summary>
        /// 特效
        /// </summary>
        Effect = 3,
    }
    /// <summary>
    /// 相对根节点的Order偏移值
    /// </summary>
    public int offestOrder = 0;

    public NGUILayerEnum nguiLayer = NGUILayerEnum.UI;

}
