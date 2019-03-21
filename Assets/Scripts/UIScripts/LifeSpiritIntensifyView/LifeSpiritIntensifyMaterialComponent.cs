using System;
using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritIntensifyMaterialComponent : BaseComponent
{
    private UISprite Spt_Frame;
    private UISprite Spt_Icon;
    private UISprite Spt_Shading;
    private GameObject Gobj_MarkGroup;
    private UILabel Lbl_Level;
    private UISprite Spt_EquipedMark;
    private UISprite Spt_TypeMark;

    private bool mIsSelect;
    public bool IsSelect
    {
        get
        {
            return mIsSelect;
        }
        set
        {
            mIsSelect = value;
            Gobj_MarkGroup.SetActive(mIsSelect);
        }
    }

    private bool mIsEquiped;
    public bool IsEquiped
    {
        get
        {
            return mIsEquiped;
        }
        set
        {
            mIsEquiped = value;
            Spt_EquipedMark.enabled = mIsEquiped;
        }
    }

    private LifeSoulData mLifeSoulData;
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
        Spt_Frame = mRootObject.transform.FindChild("MaterialGroup/Frame").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.FindChild("MaterialGroup/IconTexture").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("MaterialGroup/Shading").GetComponent<UISprite>();
        Gobj_MarkGroup = mRootObject.transform.FindChild("MarkGroup").gameObject;
        Lbl_Level = mRootObject.transform.FindChild("LevelGroup/Level").GetComponent<UILabel>();
        Spt_EquipedMark = mRootObject.transform.FindChild("EquipedMark").GetComponent<UISprite>();
        Spt_TypeMark = mRootObject.transform.FindChild("TypeMark").GetComponent<UISprite>();
        IsSelect = false;
    }

    public void UpdateCompInfo(LifeSoulData data)
    {
        mLifeSoulData = data;
        if (mLifeSoulData == null)
            return;
        UpdateInfo();
        if (LifeSoulData.IsEquipedPlayer || LifeSoulData.IsEquipedSoldier)
        {
            IsEquiped = true;
        }
        else
        {
            IsEquiped = false;
        }
        CommonFunction.SetLifeSpiritTypeMark(Spt_TypeMark, data.SoulInfo.godEquip);
    }

    private void UpdateInfo()
    {
        CommonFunction.SetQualitySprite(Spt_Frame, LifeSoulData.SoulInfo.quality, Spt_Shading);
        CommonFunction.SetSpriteName(Spt_Icon, LifeSoulData.SoulInfo.icon);
        Lbl_Level.text = LifeSoulData.SoulPOD.level.ToString();
    }
}
