using System;
using System.Collections.Generic;
using UnityEngine;
public class PreyLockBtnComponent : BaseComponent
{
    private UISprite Spt_Frame;
    private UISprite Spt_Icon;
    private GameObject Gobj_ConsumeTip;
    private UISprite Spt_ConsumeIcon;
    private UILabel Lbl_ConsumeTip;
    private UISprite Spt_SelectMark;
    private UISpriteAnimation anim;

    public bool IsPlayAnim 
    {
        get 
        {
            if (anim.isPlaying)
            {
                return true;
            }
            else 
            {
                anim.gameObject.SetActive(false);
                return false;
            }
        }
    }

    private PreyLockTypeEnum mLockType;
    public PreyLockTypeEnum LockType
    {
        get
        {
            return mLockType;
        }
    }

    private bool mIsSelected;
    public bool IsSelected
    {
        set
        {
            mIsSelected = value;
            Spt_SelectMark.enabled = mIsSelected;
        }
        get
        {
            return mIsSelected;
        }
    }

    private bool mIsLock;

    public bool IsLock
    {
        get
        {
            return mIsLock;
        }
    }

    private LifeSoulBaseInfo mLifeSoulBaseInfo;
    public LifeSoulBaseInfo LifeSoulBaseInfo
    {
        get
        {
            return mLifeSoulBaseInfo;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Frame = mRootObject.transform.FindChild("Frame").GetComponent<UISprite>();
        Spt_Icon = mRootObject.transform.FindChild("Icon").GetComponent<UISprite>();
        Gobj_ConsumeTip = mRootObject.transform.FindChild("ConsumeTipGroup").gameObject;
        Spt_ConsumeIcon = mRootObject.transform.FindChild("ConsumeTipGroup/ConsumeIcon").GetComponent<UISprite>();
        Lbl_ConsumeTip = mRootObject.transform.FindChild("ConsumeTipGroup/ConsumeTip").GetComponent<UILabel>();
        Spt_SelectMark = mRootObject.transform.FindChild("SelectMark").GetComponent<UISprite>();
        anim = mRootObject.transform.FindChild("PreyLifeSpiritEffect").GetComponent<UISpriteAnimation>();
        anim.gameObject.SetActive(false);
        anim.Pause();
        IsSelected = false;
        mIsLock = false;
    }

    public void UpdateCompType(PreyLockTypeEnum type)
    {
        mLockType = type;
        switch (mLockType)
        {
            case PreyLockTypeEnum.White:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_White_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_White_Icon);
                } break;
            case PreyLockTypeEnum.Green:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_Green_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_Green_Icon);
                } break;
            case PreyLockTypeEnum.Blue:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_Blue_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_Blue_Icon);
                } break;
            case PreyLockTypeEnum.Purple:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_Purple_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_Purple_Icon);
                } break;
            case PreyLockTypeEnum.Orange:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_Orange_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_Orange_Icon);
                } break;
            case PreyLockTypeEnum.Red:
                {
                    CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.PreyButton_Red_Frame);
                    CommonFunction.SetSpriteName(Spt_Icon, GlobalConst.SpriteName.PreyButton_Red_Icon);
                } break;
            default:
                {
                } break;
        }
    }

    public void PlayAnim() 
    {
        anim.gameObject.SetActive(true);
        anim.ResetToBeginning();
        anim.Play();
    }


    public void UpdateCompInfo(LifeSoulBaseInfo info)
    {
        mLifeSoulBaseInfo = info;
        if (info == null)
            return;
        if (info.specify_price == null)
        {
            mIsLock = true;
            Gobj_ConsumeTip.SetActive(false);
        }
        else
        {

            mIsLock = false;
            Gobj_ConsumeTip.SetActive(true);
            CommonFunction.SetMoneyIcon(Spt_ConsumeIcon, info.specify_price.Type);
            Lbl_ConsumeTip.text = info.specify_price.Number.ToString();
        }
    }
}
