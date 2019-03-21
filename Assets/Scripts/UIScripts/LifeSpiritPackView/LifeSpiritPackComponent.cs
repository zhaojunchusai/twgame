using System.Collections.Generic;
using UnityEngine;

public class LifeSpiritPackComponent : LifeSpiritBaseComponent
{
    private UISprite Spt_SelectMark;
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
            Spt_SelectMark.enabled = mIsSelect;
        }
    }



    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Icon = mRootObject.transform.Find("ItemBaseComp/IconTexture").GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.Find("ItemBaseComp/QualitySprite").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.Find("ItemBaseComp/Sprite").GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.Find("LevelGroup/Level").GetComponent<UILabel>();
        Spt_SelectMark = mRootObject.transform.Find("SelectSprite").GetComponent<UISprite>();
        Spt_TypeMark = mRootObject.transform.Find("TypeMark").GetComponent<UISprite>();
    }

    public void UpdateCompInfo(LifeSoulData data)
    {
        mLifeSoulData = data;
        if (mLifeSoulData == null)
            return;
        base.UpdateLifeSpiritInfo();
        CommonFunction.SetLifeSpiritTypeMark(Spt_TypeMark, data.SoulInfo.godEquip);
    }
}
