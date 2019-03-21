using System;
using System.Collections.Generic;
using UnityEngine;
public class LifeSpiritEquipedComponent : LifeSpiritBaseComponent
{
    private UISprite Spt_Replaceable;
    private UISprite Spt_Equipable;
    private GameObject Gobj_LevelGroup;
    private UILabel Lbl_UnlockTip;
    public bool IsReplaceable
    {
        set
        {
            Spt_Replaceable.enabled = value;
        }
    }

    public bool IsEquipable
    {
        set
        {
            Spt_Equipable.enabled = value;
        }
    }

    private int mIndex;
    public int Index
    {
        get
        {
            return mIndex;
        }
        set
        {
            mIndex = value;
        }
    }

    private bool mIsLock;
    public bool IsLock
    {
        get
        {
            return mIsLock;
        }
        set
        {
            mIsLock = value;
            if (mIsLock)
            {
                Spt_Icon.spriteName = string.Empty;
                Gobj_LevelGroup.SetActive(false);
                IsEquipable = false;
                IsReplaceable = false;
                CommonFunction.SetSpriteName(Spt_Shading, GlobalConst.SpriteName.LIFESPIRIT_SHADING);
                CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.Quality_1);
                CommonFunction.UpdateWidgetGray(Spt_Shading, true);
            }
            else
            {
                CommonFunction.UpdateWidgetGray(Spt_Shading, false);
            }
        }
    }


    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_Icon = mRootObject.transform.FindChild("Icon").GetComponent<UISprite>();
        Spt_Frame = mRootObject.transform.FindChild("Frame").GetComponent<UISprite>();
        Spt_Shading = mRootObject.transform.FindChild("Shading").GetComponent<UISprite>();
        Spt_Replaceable = mRootObject.transform.FindChild("Replaceable").GetComponent<UISprite>();
        Spt_Equipable = mRootObject.transform.FindChild("Equipable").GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.FindChild("LevelGroup/Level").GetComponent<UILabel>();
        Gobj_LevelGroup = mRootObject.transform.FindChild("LevelGroup").gameObject;
        Lbl_UnlockTip = mRootObject.transform.FindChild("UnlockTip").GetComponent<UILabel>();
        IsEquipable = false;
        IsReplaceable = false;
    }

    public void UpdateEquipCompInfo(LifeSoulData data, bool isPlayer, int level)
    {
        mLifeSoulData = data;
        LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType(Index);
        if (info == null)
            return;
        if (isPlayer)
        {
            if (level < info.player_unlock_level)
            {
                IsLock = true;
                Lbl_UnlockTip.enabled = true;
                Lbl_UnlockTip.text = string.Format(ConstString.BACKPACK_LEVELLOCKTIP, info.player_unlock_level);
                return;
            }
            else
            {
                IsLock = false;
                Lbl_UnlockTip.enabled = false;
            }
        }
        else
        {
            if (level < info.soldier_unlock_level)
            {
                IsLock = true;
                Lbl_UnlockTip.enabled = true;
                Lbl_UnlockTip.text = string.Format(ConstString.LIFESPIRIT_SOLDIERSOUL_UNLOCK, info.soldier_unlock_level);
                return;
            }
            else
            {
                IsLock = false;
                Lbl_UnlockTip.enabled = false;
            }
        }
        if (!mIsLock)
        {
            if (mLifeSoulData == null)
            {
                Spt_Icon.spriteName = string.Empty;
                Gobj_LevelGroup.SetActive(false);
                IsEquipable = false;
                CommonFunction.SetSpriteName(Spt_Shading, GlobalConst.SpriteName.LIFESPIRIT_SHADING);
                CommonFunction.SetSpriteName(Spt_Frame, GlobalConst.SpriteName.Quality_1);
            }
            else
            {
                Gobj_LevelGroup.SetActive(true);
                base.UpdateLifeSpiritInfo();
            }
        }
    }

  

}