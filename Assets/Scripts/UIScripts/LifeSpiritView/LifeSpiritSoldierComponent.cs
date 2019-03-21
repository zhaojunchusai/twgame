using UnityEngine;
using System.Collections.Generic;
public class LifeSpiritSoldierComponent : BaseComponent
{
    private UISprite Spt_SelectMark;
    private GameObject Gobj_MarkCareer;
    private UILabel Lbl_SoldierLevel;
    private UISprite Spt_Frame;
    private UISprite Spt_Shading;
    private UISprite Spt_Icon;
    private UISprite Spt_Star1;
    private UISprite Spt_Star2;
    private UISprite Spt_Star3;
    private UISprite Spt_Star4;
    private UISprite Spt_Star5;
    private UISprite Spt_Star6;
    public UILabel lbl_Label_Step;
    private List<UISprite> starList;

    private Soldier mSoldier;
    public Soldier Soldier
    {
        get
        {
            return mSoldier;
        }
    }

    private bool mIsSelect;
    public bool IsSelect
    {
        set
        {
            mIsSelect = value;
            Spt_SelectMark.enabled = mIsSelect;
        }
        get
        {
            return mIsSelect;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Star1 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp1/SelectSprite").GetComponent<UISprite>();
        Spt_Star2 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp2/SelectSprite").GetComponent<UISprite>();
        Spt_Star3 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp3/SelectSprite").GetComponent<UISprite>();
        Spt_Star4 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp4/SelectSprite").GetComponent<UISprite>();
        Spt_Star5 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp5/SelectSprite").GetComponent<UISprite>();
        Spt_Star6 = mRootObject.transform.FindChild("StarLevelGroup/EquipStarComp6/SelectSprite").GetComponent<UISprite>();
        Spt_SelectMark = mRootObject.transform.FindChild("SelectMark").GetComponent<UISprite>();
        Gobj_MarkCareer = mRootObject.transform.FindChild("MarkCareer").gameObject;
        Lbl_SoldierLevel = mRootObject.transform.FindChild("SoldierInfoGroup/Level").GetComponent<UILabel>();
        Spt_Frame = mRootObject.transform.FindChild("SoldierInfoGroup/Quality").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("SoldierInfoGroup/BackGround").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.FindChild("SoldierInfoGroup/Icon").GetComponent<UISprite>();
        lbl_Label_Step = mRootObject.transform.FindChild("Step").gameObject.GetComponent<UILabel>();
        lbl_Label_Step.gameObject.SetActive(GlobalConst.IsOpenStep);
        if (starList == null)
        {
            starList = new List<UISprite>();
            starList.Add(Spt_Star1);
            starList.Add(Spt_Star2);
            starList.Add(Spt_Star3);
            starList.Add(Spt_Star4);
            starList.Add(Spt_Star5);
            starList.Add(Spt_Star6);
        }
        IsSelect = false;
        Gobj_MarkCareer.SetActive(false);
    }


    public void UpdateCompInfo(Soldier info)
    {
        mSoldier = info;
        if (mSoldier == null)
            return;
        UpdateSoldierInfo();
        Gobj_MarkCareer.SetActive(CommonFunction.IsAlreadyBattle(mSoldier.uId));
    }

    private void UpdateSoldierInfo()
    {
        if (mSoldier == null)
            return;
        IsSelect = false;
        Lbl_SoldierLevel.text = mSoldier.Level.ToString();
        this.lbl_Label_Step.text = CommonFunction.GetStepShow(lbl_Label_Step,mSoldier.StepNum);
        CommonFunction.SetSpriteName(Spt_Icon, mSoldier.Att.Icon);
        CommonFunction.SetQualitySprite(Spt_Frame, mSoldier.Att.quality, Spt_Shading);
        UpdateSoldierStars();
    }

    private void UpdateSoldierStars()
    {
        if (mSoldier == null || starList == null)
            return;
        for (int i = 0; i < starList.Count; i++)
        {
            UISprite sprite = starList[i];
            if (sprite == null)
                continue;
            if (i < mSoldier.Att.Star)
            {
                sprite.enabled = true;
            }
            else
            {
                sprite.enabled = false;
            }
        }
    }
}
