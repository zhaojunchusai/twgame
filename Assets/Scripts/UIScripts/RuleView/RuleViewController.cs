using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class RuleViewController : UIBase
{
    public RuleView view;
    public List<RuleDescGroupComponent> ruleDesc_dic = new List<RuleDescGroupComponent>();

    private class RuleInfoData
    {
        public string title;
        public List<CommonRuleAwardItem> list;
    }

    public override void Initialize()
    {
        if (view == null)
        {
            view = new RuleView();
            view.Initialize();
            BtnEventBinding();
        }
        view.Gobj_RuleDescGroup.SetActive(false);
    }
    #region Update Event
    /// <summary>
    /// type 1:探险规则  2:争霸规则
    /// </summary>
    /// <param name="type"></param>
    public void UpdateViewInfo(int type)
    {
        UnionBaseData unionBaseData = ConfigManager.Instance.mUnionConfig.GetUnionBaseData();
        Dictionary<uint, UnionRankRewardData> unionRankData_dic = ConfigManager.Instance.mUnionConfig.GetUnionRankData();
        if (unionBaseData == null) return;
        List<RuleInfoData> list = new List<RuleInfoData>();
        switch (type)
        {
            case 1:
                {
                    view.Lbl_Title.text = ConstString.UNION_RULE_ADVANTURETITLE;
                    view.Lbl_RankDesc.text = CommonFunction.ReplaceEscapeChar(unionBaseData.mAdvantureRule);
                    //================ 通过奖励 ==================//
                    RuleInfoData passAwardData = new RuleInfoData();
                    passAwardData.title = ConstString.UNION_RULE_ADVANTUREAWARD;
                    passAwardData.list = new List<CommonRuleAwardItem>();
                    for (int i = 0; i < unionBaseData.mPassAdvantureRewards.Count; i++)
                    {
                        ExoticPassedAward award = unionBaseData.mPassAdvantureRewards[i];
                        if (award == null)
                            continue;
                        string passRankTitle = string.Format(ConstString.UNION_RULE_ADVANTUREPASSAWARD, award.gateID);
                        List<CommonItemData> commonList = CommonFunction.GetCommonItemDataList(award.awardID);
                        for (int j = 0; j < commonList.Count; j++)
                        {
                            CommonItemData itemData = commonList[j];
                            if (CommonFunction.IsUnionTokenID(itemData.ID))
                            {
                                itemData.Quality = ItemQualityEnum.None;
                            }
                        }
                        CommonRuleAwardItem passAwardItem = new CommonRuleAwardItem(passRankTitle, commonList);
                        passAwardData.list.Add(passAwardItem);
                    }
                    list.Add(passAwardData);
                    //================ 伤害排行奖励 ==================//
                    RuleInfoData rankAwardData = new RuleInfoData();
                    rankAwardData.title = ConstString.UNION_RULE_ADVANTURERANKAWARD;
                    rankAwardData.list = new List<CommonRuleAwardItem>();
                    foreach (KeyValuePair<uint, UnionRankRewardData> item in unionRankData_dic)
                    {
                        string rank = string.Format(ConstString.PVP_RULEDESC_RANK, item.Key);
                        List<CommonItemData> mAwards = new List<CommonItemData>();
                        CommonItemData m_Token = new CommonItemData();
                        m_Token.ID = GlobalCoefficient.UnionTokenID;
                        m_Token.Num = item.Value.mAdvantureTokenReward;
                        m_Token.Icon = GlobalConst.SpriteName.UnionToken;
                        CommonItemData m_Gold = new CommonItemData();
                        m_Gold.ID = GlobalCoefficient.CoinID;
                        m_Gold.Icon = GlobalConst.SpriteName.Gold;
                        m_Gold.Num = item.Value.mAdvantureGoldReward;
                        mAwards.Add(m_Token);
                        mAwards.Add(m_Gold);
                        CommonRuleAwardItem awardItem = new CommonRuleAwardItem(rank, mAwards);
                        rankAwardData.list.Add(awardItem);
                    }
                    list.Add(rankAwardData);
                }
                break;
            case 2:
                {
                    view.Lbl_Title.text = ConstString.UNION_RULE_HEGEMONYTITLE;
                    view.Lbl_RankDesc.text = CommonFunction.ReplaceEscapeChar(unionBaseData.mHegemonyRule);
                    //================ 争霸排行奖励 ==================//
                    RuleInfoData hegemonyRankAwardData = new RuleInfoData();
                    hegemonyRankAwardData.title = ConstString.UNION_RULE_HEGEMONYRANKAWARD;
                    hegemonyRankAwardData.list = new List<CommonRuleAwardItem>();
                    foreach (KeyValuePair<uint, UnionRankRewardData> item in unionRankData_dic)
                    {
                        string rank = string.Format(ConstString.PVP_RULEDESC_RANK, item.Key);
                        List<CommonItemData> mAwards = new List<CommonItemData>();
                        CommonItemData m_Token = new CommonItemData();
                        m_Token.ID = GlobalCoefficient.UnionTokenID;
                        m_Token.Icon = GlobalConst.SpriteName.UnionToken;
                        m_Token.Num = item.Value.mPvpTokenReward;
                        mAwards.Add(m_Token);
                        CommonRuleAwardItem awardItem = new CommonRuleAwardItem(rank, mAwards);
                        hegemonyRankAwardData.list.Add(awardItem);
                    }
                    list.Add(hegemonyRankAwardData);
                    //================ 击杀排行奖励 ==================//
                    RuleInfoData killRankAwardData = new RuleInfoData();
                    killRankAwardData.title = ConstString.UNION_RULE_HEGEMONYEACHGAMEAWARD;
                    killRankAwardData.list = new List<CommonRuleAwardItem>();
                    foreach (KeyValuePair<uint, UnionRankRewardData> item in unionRankData_dic)
                    {
                        string rank = string.Format(ConstString.PVP_RULEDESC_RANK, item.Key);
                        List<CommonItemData> mAwards = new List<CommonItemData>();
                        CommonItemData m_Token = new CommonItemData();
                        m_Token.ID = GlobalCoefficient.UnionTokenID;
                        m_Token.Icon = GlobalConst.SpriteName.UnionToken;
                        m_Token.Num = item.Value.mKillTokenReward;
                        mAwards.Add(m_Token);
                        CommonRuleAwardItem awardItem = new CommonRuleAwardItem(rank, mAwards);
                        killRankAwardData.list.Add(awardItem);
                    }
                    list.Add(killRankAwardData);
                } break;
            case 3:
                {
                    string strRule = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_EXPEDITION_RULE);
                    view.Lbl_Title.text = ConstString.EXPEDITION_RULE_TITLE;
                    view.Lbl_RankDesc.text = CommonFunction.ReplaceEscapeChar(strRule);
                } break;
        }
        Main.Instance.StartCoroutine(UpdateRuleDescGroup(list));
    }

    private IEnumerator UpdateRuleDescGroup(List<RuleInfoData> list)
    {
        if (ruleDesc_dic == null)
            yield break;
        int obj_Index = ruleDesc_dic.Count;
        int list_Count = list.Count;
        if (list_Count <= obj_Index)
        {
            for (int i = list_Count; i < ruleDesc_dic.Count; i++)
            {
                RuleDescGroupComponent comp = ruleDesc_dic[i];
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int index = 0; index < list_Count; index++)
        {
            RuleInfoData mItemData = list[index];
            if (mItemData == null) continue;
            RuleDescGroupComponent comp = null;
            if (index < obj_Index)
            {
                comp = ruleDesc_dic[index];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_RuleDescGroup, view.UITable_Rule.transform);// UITable_GetAwards.transform);
                go.name = "RuleDescGroup_" + index.ToString();
                comp = new RuleDescGroupComponent();
                comp.MyStart(go);
                ruleDesc_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(mItemData.title, mItemData.list);
        }
        yield return null;
        view.UITable_Rule.repositionNow = true;
        yield return 1;
        view.ScrView_RuleDesc.ResetPosition();
    }

    #endregion

    private void ButtonEvent_Close(GameObject go)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_RULEVIEW);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_Close;
    }


    public override void Destroy()
    {
        base.Destroy();
        view = null;
        ruleDesc_dic.Clear();
    }
}
