using UnityEngine;
using System.Collections.Generic;

public class PVPOwnEquipTypeComponent : PVPEquipComponent
{
    public delegate void DetailInfoAction(BaseComponent baseComp, bool state);
    public DetailInfoAction callBack;
    private UISprite Spt_EquipType;
    private UISprite Spt_MaskSprite;
    //private UISprite Spt_LowLevel;
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
        Spt_EquipType = mRootObject.transform.FindChild("EquipType").gameObject.GetComponent<UISprite>();
        Spt_MaskSprite = mRootObject.transform.FindChild("MaskSprite").GetComponent<UISprite>();
        //Spt_LowLevel = mRootObject.transform.FindChild("LowLevelSprite").GetComponent<UISprite>();
        Lbl_UnLockTip = mRootObject.transform.FindChild("UnLockTip").gameObject.GetComponent<UILabel>();
        //IsLowLevel = true;
        UIEventListener.Get(Spt_EquipType.gameObject).onPress = DetailHandle;
        Spt_MaskSprite.enabled = false;
    }
    private void DetailHandle(GameObject go, bool state)
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
        base.UpdateInfo(equip);
      
        //if (PlayerData.Instance._Level < equip.Att.levelLimit)
        //{
        //    IsLowLevel = true;
        //}
        //else
        //{
        //    IsLowLevel = false;
        //}
        UpdateEquipTypeIcon(equip.Att.type);
        CommonFunction.UpdateWidgetGray(Spt_EquipType, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_IconTexture, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_ItemBgSprite, equip.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_QualitySprite, equip.IsLock);
        Lbl_UnLockTip.enabled = equip.IsLock;
        if (equip.godEquipLockInfo !=null )
        {
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
                Lbl_UnLockTip.text = ConstString.BACKPACK_ERRORLOCKTIP;
            }
        }
    }

    private void UpdateEquipTypeIcon(int type) 
    {
        //CommonFunction.SetSpriteName(Spt_EquipType,CommonFunction.GetEquipTypeIcon(type));
    }

    public override void Clear()
    {
        base.Clear();
    }
}

