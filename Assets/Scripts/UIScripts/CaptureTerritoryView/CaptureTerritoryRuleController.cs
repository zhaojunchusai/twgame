using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryRuleController : UIBase 
{
    public CaptureTerritoryRule view;

    private PVPGetAwradsGroupComponet _myRankAwardItem;
    private PVPRankAwardDescGroupComponent _rankAwardsItem;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new CaptureTerritoryRule();
            view.Initialize();
            BtnEventBinding();
        }        
        Main.Instance.StartCoroutine(InitUI());
    }

    private IEnumerator InitUI()
    {
        view.ScrView_RuleDesc.ResetPosition();
        yield return null;
        InitMyRank();
        yield return null;
        InitRuleContent();
        yield return null;
        InitAwards();
    }

    private void InitMyRank()
    {
        if (_myRankAwardItem == null)
        {
            _myRankAwardItem = new PVPGetAwradsGroupComponet();
            _myRankAwardItem.MyStart(view.Gobj_GetAwardsGroup);
        }

        fogs.proto.msg.CampaignRankInfo rank = CaptureTerritoryModule.Instance.MyDamageRankInfo;        
        view.Lbl_RankDesc.text = string.Format(ConstString.FORMAT_RULE_MY_RANK, rank.score,
            rank.score > 0 ? rank.rank.ToString() : ConstString.NOT_JOIN_FIGHT);

        if (rank.score > 0)
        {
            _myRankAwardItem.mRootObject.SetActive(true);
            view.Spt_SelfRankSprite.height = 285;
            _myRankAwardItem.UpdateInfo(ConfigManager.Instance.mCaptureTerritoryConfig.GetArenaAwardInfoByRank(rank.rank));
        }
        else
        {
            view.Spt_SelfRankSprite.height = 145;
            _myRankAwardItem.mRootObject.SetActive(false);
        }

        view.UITable_Rule.Reposition();
    }

    private void InitRuleContent()
    {
        view.Lbl_RuleDesc.text = ConfigManager.Instance.mCaptureTerritoryConfig.Rule;
        view.UITable_Rule.Reposition();
    }

    public void InitAwards()
    {
        if (_rankAwardsItem == null)
        {
            _rankAwardsItem = new PVPRankAwardDescGroupComponent();
            _rankAwardsItem.MyStart(view.Gobj_GetAwards);
        }
        _rankAwardsItem.UpdateInfo(ConfigManager.Instance.mCaptureTerritoryConfig.GetArenaAwardList());
        view.UITable_Rule.Reposition();
    }
    private void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(CaptureTerritoryRule.UIName);
    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        view = null;
        _myRankAwardItem = null;
        _rankAwardsItem = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = CloseUI;
    }
}
