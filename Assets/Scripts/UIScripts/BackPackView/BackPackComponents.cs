using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
/* File:BackPackComponents.cs
 * Desc:背包面板中用到的组件均放置于此
 * Date:2015-07-21 19:29
 * add by taiwei
 */

/// <summary>
/// 聊天界面 功能类型组件
/// </summary>
public class ChatFuncTypeComponent : BaseComponent
{
    public UISprite DefaultSprite;
    public UISprite SelectSprite;
    public UISprite DescSprite;
    public UISprite MarkSprite;
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
            DefaultSprite.enabled = !isSelect;
            DescSprite.enabled = !isSelect;
            SelectSprite.enabled = isSelect;
        }
    }

    public bool IsMark
    {
        set
        {
            MarkSprite.enabled = value;
        }
        get
        {
            return MarkSprite.enabled;
        }
    }

    public ChatFuncTypeComponent(GameObject root)
    {
        base.MyStart(root);
        DefaultSprite = root.transform.FindChild("DefaultSprite").gameObject.GetComponent<UISprite>();
        SelectSprite = root.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        DescSprite = root.transform.FindChild("DescSprite").gameObject.GetComponent<UISprite>();
        MarkSprite = root.transform.FindChild("MarkSprite").gameObject.GetComponent<UISprite>();
        IsMark = false;
        // base.AutoSetGoProperty<BPFuncTypeComponent>(this, root);
    }

    public override void Clear()
    {
        base.Clear();

    }
}

/// <summary>
/// 背包界面 功能类型组件
/// </summary>
public class BPFuncTypeComponent : BaseComponent
{
    public UISprite DefaultSprite;
    public UISprite SelectSprite;
    public UILabel DescSprite;
    public UISprite MarkSprite;
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
            DefaultSprite.enabled = !isSelect;
            SelectSprite.enabled = isSelect;
            if (isSelect)
            {
                DescSprite.color = Color.white; 
            }
            else
            {
                DescSprite.color = new Color(0.138f,0.075f,0f);
            }
        }
    }

    public bool IsMark
    {
        set
        {
            MarkSprite.enabled = value;
        }
        get 
        {
            return MarkSprite.enabled;
        }
    }

    public BPFuncTypeComponent(GameObject root)
    {
        base.MyStart(root);
        DefaultSprite = root.transform.FindChild("DefaultSprite").gameObject.GetComponent<UISprite>();
        SelectSprite = root.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        DescSprite = root.transform.FindChild("DescSprite").gameObject.GetComponent<UILabel>();
        MarkSprite = root.transform.FindChild("MarkSprite").gameObject.GetComponent<UISprite>();
        IsMark = false;
        // base.AutoSetGoProperty<BPFuncTypeComponent>(this, root);
    }

    public override void Clear()
    {
        base.Clear();

    }
}

/// <summary>
/// 背包界面 材料、消耗品、碎片组件
/// </summary>
public class BPNormalItemComponent : ItemBaseComponent
{
    public UISprite Spt_SelectSprite;
    public UILabel Lbl_CountLabel;
    public UISprite Spt_ChipMark;
    private UISprite Spt_ComposeMarkSprite;

    private bool isSelect;
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

    private ItemInfo itemInfo = null;
    public ItemInfo ItemInfo
    {
        get
        {
            return itemInfo;
        }
    }

    private Item itemPOD;
    public Item ItemPOD
    {
        get
        {
            return itemPOD;
        }
    }


    public BPNormalItemComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/Sprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Lbl_CountLabel = mRootObject.transform.FindChild("CountLabel").gameObject.GetComponent<UILabel>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Spt_ChipMark = mRootObject.transform.FindChild("ChipMarkSprite").gameObject.GetComponent<UISprite>();
        Spt_ComposeMarkSprite = mRootObject.transform.FindChild("ComposeMarkSprite").gameObject.GetComponent<UISprite>();
        // mRootObject = root;
        // base.AutoSetGoProperty<BPNormalItemComponent>(this, root);
        // Clear();
    }

    public void UpdateInfo(Item pod)
    {
        if (pod == null)
        {
            Clear();
            return;
        }
        itemPOD = pod;
        itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(itemPOD.id);
        if (itemInfo == null)
        {
            Clear();
            return;
        }
        base.UpdateInfo(itemInfo.icon, itemInfo.quality);
        UpdateCount(itemPOD.num);
        if (itemInfo.type == (int)ItemTypeEnum.EquipChip)
        {
            Spt_ChipMark.enabled = true;
            CommonFunction.SetSpriteName(Spt_ChipMark, GlobalConst.SpriteName.MarkEquipChip);
            UpdateComposeStatus();
        }
        else
        {
            Spt_ComposeMarkSprite.enabled = false;
            Spt_ChipMark.enabled = false;
        }
    }

    private void UpdateCount(int num)
    {
        if (num <= 1)
        {
            Lbl_CountLabel.text = string.Empty;
        }
        else
        {
            Lbl_CountLabel.text = string.Format("x{0}", num);
        }
        if (!Lbl_CountLabel.enabled)
            Lbl_CountLabel.enabled = true;
    }

    private void UpdateComposeStatus()
    {
        if (itemPOD == null || itemInfo == null) return;
        if (itemPOD.num >= itemInfo.compound_count)
        {
            Spt_ComposeMarkSprite.enabled = true;
            Lbl_CountLabel.enabled = false;
        }
        else
        {
            Spt_ComposeMarkSprite.enabled = false;
            Lbl_CountLabel.enabled = true;
        }
    }

    public override void Clear()
    {
        base.Clear();
        itemPOD = null;
        itemInfo = null;
        IsSelect = false;
        Spt_ChipMark.enabled = false;
        Lbl_CountLabel.text = string.Empty;
    }

}

/// <summary>
/// 背包界面神器属性组件
/// </summary>
public class BPArtifactAttComponent : BaseComponent
{
    private UILabel Lbl_AttValue;
    private UISprite Spt_AttIcon;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Lbl_AttValue = mRootObject.transform.FindChild("AttValue").GetComponent<UILabel>();
        Spt_AttIcon = mRootObject.transform.FindChild("AttIcon").GetComponent<UISprite>();
    }

    public void UpdateInfo(string icon, string att)
    {
        Lbl_AttValue.text = att.ToString();
        CommonFunction.SetSpriteName(Spt_AttIcon, icon);
        Spt_AttIcon.MakePixelPerfect();
    }

    public override void Clear()
    {
        base.Clear();
    }
}

/// <summary>
/// 背包界面 神器组件
/// </summary>
public class BPArtifactComponent : ItemBaseComponent
{
    private UISprite Spt_SelectSprite;
    private UILabel Lbl_UnLockTip;
    private UILabel Lbl_Level;
    private GameObject Gobj_LevelGroup;
    private UISprite Spt_IntensifyMarkSprite;

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

    private Weapon artifactPOD = null;
    public Weapon ArtifactPOD { get { return artifactPOD; } }

    public BPArtifactComponent(GameObject root)
    {
        base.MyStart(root);
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/Sprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Lbl_UnLockTip = mRootObject.transform.FindChild("UnLockTip").gameObject.GetComponent<UILabel>();
        Lbl_Level = mRootObject.transform.FindChild("LevelGroup/Level").GetComponent<UILabel>();
        Gobj_LevelGroup = mRootObject.transform.FindChild("LevelGroup").gameObject;
        Spt_IntensifyMarkSprite = mRootObject.transform.FindChild("IntensifyMarkSprite").gameObject.GetComponent<UISprite>();
        //mRootObject = root;
        // base.AutoSetGoProperty<BPArtifactComponent>(this, root);
    }

    public void UpdateInfo(Weapon pod)
    {
        artifactPOD = pod;
        if (pod == null)
        {
            Clear();
            return;
        }
        if (Lbl_UnLockTip != null)
        {
            Lbl_UnLockTip.enabled = artifactPOD.IsLock;
            if (artifactPOD.godEquipLockInfo != null) 
            {
                if (PlayerData.Instance._VipLv < artifactPOD.godEquipLockInfo.vipLv)
                {
                    Lbl_UnLockTip.text = string.Format(ConstString.BACKPACK_VIPLOCKTIP, artifactPOD.godEquipLockInfo.vipLv.ToString());
                }
                else if (PlayerData.Instance._Level < artifactPOD.godEquipLockInfo.level)
                {
                    Lbl_UnLockTip.text = string.Format(ConstString.BACKPACK_LEVELLOCKTIP, artifactPOD.godEquipLockInfo.level.ToString()); 
                }
                else
                {
                    Lbl_UnLockTip.text = ConstString.BACKPACK_ERRORLOCKTIP;
                }
            }
        }
        base.UpdateInfo(artifactPOD.Att.icon, artifactPOD.Att.quality);
        UpdateIntensifyStatus();
        Gobj_LevelGroup.SetActive(!artifactPOD.IsLock);
        if (!artifactPOD.IsLock)
            Lbl_Level.text = artifactPOD.Level.ToString();
        CommonFunction.UpdateWidgetGray(Spt_IconTexture, artifactPOD.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_ItemBgSprite, artifactPOD.IsLock);
        CommonFunction.UpdateWidgetGray(Spt_QualitySprite, artifactPOD.IsLock);
    }


    private void UpdateIntensifyStatus()
    {
        if (artifactPOD == null) return;
        if (artifactPOD.IsLock)
        {
            Spt_IntensifyMarkSprite.enabled = false;
        }
        else
        {
            switch (artifactPOD.enableStrong())
            {
                case WeaponCheck.Ok:
                    Spt_IntensifyMarkSprite.enabled = true;
                    break;
                default:
                    Spt_IntensifyMarkSprite.enabled = false;
                    break;
            }
        }
    }
    public override void Clear()
    {
        base.Clear();
        artifactPOD = null;
        IsSelect = false;
        Gobj_LevelGroup.SetActive(false);
        Lbl_Level.text = string.Empty;
    }
}

public class BPSoldierEquipComponent : ItemBaseComponent
{
    public UISprite Spt_SelectSprite;
    public UIGrid Grd_QualityGrid;
    public UISprite Spt_StarSprite;
    public UILabel Lbl_Level;

    private List<GameObject> sptStarList;

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



    private Weapon soldierEquipPOD = null;
    public Weapon SoldierEquipPOD { get { return soldierEquipPOD; } }

    public BPSoldierEquipComponent(GameObject root)
    {
        base.MyStart(root);
        sptStarList = new List<GameObject>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBaseComp/Sprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_SelectSprite = mRootObject.transform.FindChild("SelectSprite").gameObject.GetComponent<UISprite>();
        Grd_QualityGrid = mRootObject.transform.FindChild("QualityGrid").gameObject.GetComponent<UIGrid>();
        Spt_StarSprite = mRootObject.transform.FindChild("QualityGrid/StarSprite").gameObject.GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.FindChild("LevelGroup/Level").gameObject.GetComponent<UILabel>();
        Spt_StarSprite.gameObject.SetActive(false);
    }

    public void UpdateInfo(Weapon pod)
    {
        if (pod == null)
        {
            Clear();
            return;
        }
        soldierEquipPOD = pod;
        base.UpdateInfo(soldierEquipPOD.Att.icon, soldierEquipPOD.Att.quality);
        UpdateStar();
        Lbl_Level.text = soldierEquipPOD.Level.ToString();
    }

    private void UpdateStar()
    {
        if (soldierEquipPOD.Att.star <= sptStarList.Count)
        {
            for (int i = 0; i < sptStarList.Count; i++)
            {
                GameObject sprite = sptStarList[i];
                if (i < soldierEquipPOD.Att.star)
                {
                    sprite.SetActive(true);
                }
                else
                {
                    sprite.SetActive(false);
                }
            }
        }
        else
        {
            int index = sptStarList.Count;
            for (int i = 0; i < sptStarList.Count; i++)
            {
                GameObject sprite = sptStarList[i];
                sprite.SetActive(true);
            }
            for (int i = index; i < soldierEquipPOD.Att.star; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(Spt_StarSprite.gameObject, Grd_QualityGrid.transform);
                go.name = "Star-" + i.ToString();
                go.SetActive(true);
                sptStarList.Add(go);
            }
        }
        Grd_QualityGrid.hideInactive = true;
        Grd_QualityGrid.Reposition();
    }

    public override void Clear()
    {
        base.Clear();
        soldierEquipPOD = null;
        IsSelect = false;
        Lbl_Level.text = string.Empty;
    }
}