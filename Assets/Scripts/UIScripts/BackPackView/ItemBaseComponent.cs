using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;

/// <summary>
/// 物品基础属性组件 (仅包含ICON以及品质)
/// </summary>
public class ItemBaseComponent : BaseComponent
{
    /// <summary>
    /// Icon
    /// </summary>
    public UISprite Spt_IconTexture;
    /// <summary>
    /// 品质
    /// </summary>
    public UISprite Spt_QualitySprite;
    /// <summary>
    /// 背景底框
    /// </summary>
    public UISprite Spt_ItemBgSprite;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
    }

    public void UpdateInfo(string icon,int quality)
    {
        Spt_IconTexture.gameObject.SetActive(true);
        CommonFunction.SetSpriteName(Spt_IconTexture, icon);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, quality, Spt_ItemBgSprite);
    }

    public override void Clear()
    {
        base.Clear();
        Spt_IconTexture.spriteName = string.Empty;
        Spt_QualitySprite.spriteName = string.Empty;
    }

}
