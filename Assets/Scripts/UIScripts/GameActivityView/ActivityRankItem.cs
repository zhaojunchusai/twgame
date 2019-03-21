using System;
using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;

public class ActivityRankItem : GameActivityContentItemBase
{
    [HideInInspector]
    public UISprite Spt_AwardsBGSprite;
    [HideInInspector]
    public UIGrid Grd_ItemsGrid;
    [HideInInspector]
    public UILabel Lbl_ValueLabel;
    [HideInInspector]
    public UILabel Lbl_NameLabel;
    [HideInInspector]
    public UISprite Spt_SplitSprite;
    [HideInInspector]
    public UISprite Spt_RankNumBG;
    [HideInInspector]
    public UILabel Lbl_RankNumLabel;

    [HideInInspector]
    public MailAttachmentItem Item_0;
    [HideInInspector]
    public MailAttachmentItem Item_1;
    [HideInInspector]
    public MailAttachmentItem Item_2;
    private List<MailAttachmentItem> _itemList;

    //private RankType _type;
    private GameActivityType _type;

    public void Initialize()
    {
        if (mIsInit)
            return;
        mIsInit = true;

        Spt_AwardsBGSprite = transform.FindChild("AwardsBGSprite").gameObject.GetComponent<UISprite>();
        Grd_ItemsGrid = transform.FindChild("ItemsGrid").gameObject.GetComponent<UIGrid>();
        Lbl_ValueLabel = transform.FindChild("ValueLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = transform.FindChild("NameLabel").gameObject.GetComponent<UILabel>();
        Spt_SplitSprite = transform.FindChild("SplitSprite").gameObject.GetComponent<UISprite>();
        Spt_RankNumBG = transform.FindChild("RankNumBG").gameObject.GetComponent<UISprite>();
        Lbl_RankNumLabel = transform.FindChild("RankNumLabel").gameObject.GetComponent<UILabel>();

        Item_0 = transform.FindChild("ItemsGrid/Item_0").gameObject.AddComponent<MailAttachmentItem>();
        Item_1 = transform.FindChild("ItemsGrid/Item_1").gameObject.AddComponent<MailAttachmentItem>();
        Item_2 = transform.FindChild("ItemsGrid/Item_2").gameObject.AddComponent<MailAttachmentItem>();

        Item_0.mHideNameLabel = true;
        Item_1.mHideNameLabel = true;
        Item_2.mHideNameLabel = true;

        UIEventListener.Get(Item_0.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_1.gameObject).onPress = PressItem;
        UIEventListener.Get(Item_2.gameObject).onPress = PressItem;

        _itemList = new List<MailAttachmentItem>() { Item_0, Item_1, Item_2 };


        SetLabelValues();
    }

    void Awake()
    {
        Init();
    }

    void Init()
    {
        Initialize();
        HideItems();
    }

    public void SetLabelValues()
    {
        Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, "------");
        Lbl_NameLabel.text = ConstString.RANK_LABEL_OUTRANK;
        Lbl_RankNumLabel.text = "1";
    }

    public void Uninitialize()
    {

    }

    public void UpdateItemInfo(uint activityID, GameActivityReqData data, bool showProgress)
    {
        Init();
        _id = activityID;
        List<CommonItemData> list = new List<CommonItemData>();
        if (data.mDrops != null && data.mDrops.Count > 0)
            foreach (var tmp in data.mDrops)
            {
                CommonItemData item = new CommonItemData(tmp.drop_id, tmp.drop_number);
                list.Add(item);
            }
        int leng = Mathf.Min(list.Count, _itemList.Count);
        for (int i = 0; i < leng; i++)
        {
            MailAttachmentItem item = _itemList[i];
            CommonItemData itemData = list[i];
            item.gameObject.SetActive(true);
            item.UpdateItemInfo(itemData.ID, itemData.Num);
        }
        Grd_ItemsGrid.Reposition();
    }

    public void UpdateRankTypeByActivity(GameActivityType type)
    {
        _type = type;
        switch (type)
        {
            case GameActivityType.Rank_Combat:
                //_type = RankType.COMBAT_RANK;
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, "------");
                break;
            case GameActivityType.Rank_Level:
                //_type = RankType.LEVEL_RANK;
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LEVEL, 0);
                break;
            case  GameActivityType.RechargeRank:
                //_type = RankType.RECHARGE_RANK;
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_RECHARGE, 0);
                break;
            case GameActivityType.CostRank:
                Lbl_ValueLabel.text = string.Format(ConstString.GAMEACTIVITY_RANK_COST, 0);
                break;
            case GameActivityType.CrossRealmRechargeRank:
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_RECHARGE, 0);
                break;
            case GameActivityType.CrossRealmCostRank:
                Lbl_ValueLabel.text = string.Format(ConstString.GAMEACTIVITY_RANK_COST, 0);
                break;
        }
    }

    public void UpdateRank(int rank)
    {
        Lbl_RankNumLabel.text = rank.ToString();
        if (rank == 1)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG, GlobalConst.SpriteName.RANK_BG1_1);
        }
        else if (rank == 2)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG, GlobalConst.SpriteName.RANK_BG1_2);
        }
        else if (rank == 3)
        {
            CommonFunction.SetSpriteName(Spt_RankNumBG, GlobalConst.SpriteName.RANK_BG1_3);
        }
        else
        {
            Spt_RankNumBG.gameObject.SetActive(false);
        }
    }

    private void HideItems()
    {
        foreach (MailAttachmentItem item in _itemList)
        {
            item.gameObject.SetActive(false);
        }
    }

    public override void UpdateRankInfo(ActivityRankInfo info)
    {
        Lbl_NameLabel.text = info.charname;
        switch (_type)
        {
            //case RankType.COMBAT_RANK:
            case GameActivityType.Rank_Combat:
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_BFIGHTPOWER, CommonFunction.GetTenThousandUnit(info.rank_combat_power, 10000));
                break;
            //case RankType.LEVEL_RANK: 
            case GameActivityType.Rank_Level:
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_LEVEL, info.level);
                break;
            //case RankType.RECHARGE_RANK:
            case GameActivityType.RechargeRank:
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_RECHARGE, info.recharge_money);
                break;
            case GameActivityType.CostRank:
                Lbl_ValueLabel.text = string.Format(ConstString.GAMEACTIVITY_RANK_COST, info.recharge_money);
                break;
            case GameActivityType.CrossRealmRechargeRank:
                Lbl_ValueLabel.text = string.Format(ConstString.RANK_ITEMLABLE_RECHARGE, info.recharge_money);
                break;
            case GameActivityType.CrossRealmCostRank:
                Lbl_ValueLabel.text = string.Format(ConstString.GAMEACTIVITY_RANK_COST, info.recharge_money);
                break;

        }
    }
}
