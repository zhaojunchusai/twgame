using UnityEngine;
using System.Collections.Generic;

public class GateInfoOwnEquipComponent : GateInfoEquipComponent
{
    public delegate void DetailInfoAction(BaseComponent baseComp,bool state);
    public DetailInfoAction callBack;

    private UISprite Spt_MaskSprite;
    //private UISprite Spt_LowLevel;
    private UISprite Spt_EquipType;
    private UILabel Lbl_UnLockTip;
    //private bool isLowLevel;
    //public bool IsLowLevel
    //{
    //    get
    //    {
    //        return isLowLevel;
    //    }
    //    set
    //    {
    //        isLowLevel = value;
    //        Spt_LowLevel.enabled = isLowLevel;
    //        Spt_MaskSprite.enabled = isLowLevel;
    //    }
    //}


    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_MaskSprite = mRootObject.transform.FindChild("MaskSprite").GetComponent<UISprite>();
        //Spt_LowLevel = mRootObject.transform.FindChild("LowLevelSprite").GetComponent<UISprite>();
        Spt_EquipType = mRootObject.transform.FindChild("EquipType").GetComponent<UISprite>();
        Lbl_UnLockTip = mRootObject.transform.FindChild("UnLockTip").GetComponent<UILabel>();
        Spt_MaskSprite.enabled = false;
        UIEventListener.Get(Spt_EquipType.gameObject).onPress = DetailHandle;
        //IsLowLevel = true;
    }

    private void DetailHandle(GameObject go,bool state) 
    {
        if (callBack != null)
        {
            callBack(this, state);
        }
    }

    public void UpdateCompInfo(Weapon equip)
    {
        if (equip == null)
        {
            Clear();
            return;
        }
        //if (PlayerData.Instance._Level < equip.Att.levelLimit)
        //{
        //    IsLowLevel = true;
        //}
        //else
        //{
        //    IsLowLevel = false;
        //}
        base.UpdateInfo(equip);
        UpdateEquipTypeIcon(equip.Att.type);
        IsShowEnergy = !equip.IsLock;
        CommonFunction.UpdateWidgetGray(Spt_EquipType, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_IconTexture, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_ItemBgSprite, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_QualitySprite, equip.IsLock);
        Lbl_UnLockTip.enabled = equip.IsLock;
        if (PlayerData.Instance._VipLv < equip.godEquipLockInfo.vipLv)
        {
            Lbl_UnLockTip.text = string.Format(ConstString.BACKPACK_VIPLOCKTIP, equip.godEquipLockInfo.vipLv.ToString());
        }
        else if (PlayerData.Instance._Level < equip.godEquipLockInfo.level)
        {
            Lbl_UnLockTip.text = string.Format(ConstString.BACKPACK_LEVELLOCKTIP, equip.godEquipLockInfo.level.ToString());
        }
        else
        {
            Lbl_UnLockTip.text =  ConstString.BACKPACK_ERRORLOCKTIP;
        }
    }

    private void UpdateEquipTypeIcon(int type)
    {
        //CommonFunction.SetSpriteName(Spt_EquipType, CommonFunction.GetEquipTypeIcon(type));
        //Spt_EquipType.MakePixelPerfect();
    }

    public override void Clear()
    {
        base.Clear();
    }
}


public class GateInfoEquipComponent : ItemBaseComponent
{
    /// <summary>
    /// 名称
    /// </summary>
    protected UILabel Lbl_NameLabel;
    /// <summary>
    /// 武器携带技能消耗魔法值
    /// </summary>
    protected UILabel Lbl_EnergyLabel;
    /// <summary>
    /// 武器携带技能消耗魔法值
    /// </summary>
    protected UILabel Lbl_LevelLabel;
    protected GameObject Gobj_LevelGroup;
    /// <summary>
    /// 选中
    /// </summary>
    protected UISprite Spt_SelectSprite;

    protected GameObject Gobj_EnergyGroup;
    public bool IsShowLevel
    {
        set
        {
            Gobj_LevelGroup.SetActive(value);
        }
    }
    public bool IsShowEnergy
    {
        set
        {
            Gobj_EnergyGroup.SetActive(value);
        }
    }

    private bool isSelect = false;
    public bool IsSelect
    {
        get
        {
            return isSelect;
        }
        set
        {
            isSelect = value;
            Spt_SelectSprite.enabled = isSelect;
        }
    }
    protected Weapon equipInfo;
    public Weapon EquipInfo
    {
        get
        {
            return equipInfo;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_NameLabel = mRootObject.transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Lbl_EnergyLabel = mRootObject.transform.FindChild("EnergyGroup/EnergyLabel").gameObject.GetComponent<UILabel>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("LevelGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Gobj_EnergyGroup = mRootObject.transform.FindChild("EnergyGroup").gameObject;
        Gobj_LevelGroup = mRootObject.transform.FindChild("LevelGroup").gameObject;
        IsSelect = false;
    }


    public void UpdateInfo(Weapon equip)
    {
        if (equip == null)
        {
            Clear();
            return;
        }
        equipInfo = equip;
        Lbl_NameLabel.text = equipInfo.Att.name;
        if (equipInfo._Skill == null)
        {
            Lbl_EnergyLabel.text = (0).ToString();
        }
        else
        {
            Lbl_EnergyLabel.text = equipInfo._Skill.Att.expendMagic.ToString();
        }
        if (equipInfo.IsLock)
        {
            IsShowLevel = false;
        }
        else
        {
            IsShowLevel = true;
            Lbl_LevelLabel.text = equipInfo.Level.ToString();
        }
        base.UpdateInfo(equipInfo.Att.icon, equipInfo.Att.quality);
    }
    public override void Clear()
    {
        base.Clear();
    }
}