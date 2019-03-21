using UnityEngine;
using System.Collections.Generic;
public class GateAwardsComponent : ItemBaseComponent
{
    private uint itemID;
    public uint ItemID
    {
        get
        {
            return itemID;
        }
    }

    private UISprite Spt_AwardType;

    public delegate void PressDelegate(GateAwardsComponent comp, bool isPress);
    public PressDelegate pressDelegate;

    public GateAwardsComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_QualitySprite = mRootObject.transform.FindChild("QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("Sprite").gameObject.GetComponent<UISprite>();
        Spt_AwardType = mRootObject.transform.FindChild("AwardType").gameObject.GetComponent<UISprite>();
    }

    public void AddPressLisetener(PressDelegate callBack)
    {
        pressDelegate = callBack;
        UIEventListener.Get(mRootObject).onPress = PressHandle;
    }

    private void PressHandle(GameObject go, bool IsPress)
    {
        if (pressDelegate != null)
        {
            pressDelegate(this, IsPress);
        }
    }
    public void UpdateCompInfo(uint id, ItemTypeEnum subType, string icon, int quality)
    {
        itemID = id;
        base.UpdateInfo(icon, quality);
        if (subType == ItemTypeEnum.EquipChip)
        {
            Spt_AwardType.enabled = true;
            CommonFunction.SetSpriteName(Spt_AwardType, GlobalConst.SpriteName.MarkEquipChip);
            Spt_AwardType.MakePixelPerfect();
        }
        else if (subType == ItemTypeEnum.SoldierChip)
        {
            Spt_AwardType.enabled = true;
            CommonFunction.SetSpriteName(Spt_AwardType, GlobalConst.SpriteName.MarkSoldierChip);
            Spt_AwardType.MakePixelPerfect();
        }
        else
        {
            Spt_AwardType.enabled = false;
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}
