using System;
using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritBaseComponent : BaseComponent
{
    protected UISprite Spt_Icon;
    protected UISprite Spt_Frame;
    protected UISprite Spt_Shading;
    protected UILabel Lbl_Level;

    protected LifeSoulData mLifeSoulData;
    public LifeSoulData LifeSoulData
    {
        get
        {
            return mLifeSoulData;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
    }


    protected void UpdateLifeSpiritInfo()
    {
        if (mLifeSoulData == null)
            return;
        CommonFunction.SetSpriteName(Spt_Icon, mLifeSoulData.SoulInfo.icon);
        CommonFunction.SetQualitySprite(Spt_Frame, mLifeSoulData.SoulInfo.quality, Spt_Shading);
        Lbl_Level.text = mLifeSoulData.SoulPOD.level.ToString();

    }
}