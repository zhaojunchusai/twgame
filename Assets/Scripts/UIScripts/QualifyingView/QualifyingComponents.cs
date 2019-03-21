using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class QualifyingAwardComponent : ItemBaseComponent
{
    public UILabel Lbl_AwardCount;

    private CommonItemData mItemData;
    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_IconTexture = mRootObject.transform.FindChild("IconSprite").GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("ItemBgSprite").GetComponent<UISprite>();
        Spt_QualitySprite = mRootObject.transform.FindChild("QualitySprite").GetComponent<UISprite>();
        Lbl_AwardCount = mRootObject.transform.FindChild("CountLabel").GetComponent<UILabel>();
        UIEventListener.Get(mRootObject).onPress = ButtonEvent_Press;
    }

    public void UpdateAwardInfo(CommonItemData data)
    {
        mItemData = data;
        CommonFunction.SetSpriteName(Spt_IconTexture, data.Icon);
        CommonFunction.SetQualitySprite(Spt_QualitySprite, data.Quality, Spt_ItemBgSprite);

        Lbl_AwardCount.text = "x" + CommonFunction.GetTenThousandUnit(data.Num);
    }

    private void ButtonEvent_Press(GameObject go, bool isPress)
    {
        if (mItemData == null)
        {
            return;
        }
        HintManager.Instance.SeeDetail(go, isPress, mItemData.ID);
    }

}

public class QualifyingDivisionAwardsComponent : BaseComponent
{
    private UIGrid Grd_DivisionAwards;
    private GameObject Gobj_DivisionAwardComp;
    private UISprite Spt_DivisionIcon;
    private UILabel Lbl_DivisionScore;


    private List<QualifyingAwardComponent> award_dic;

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Grd_DivisionAwards = mRootObject.transform.FindChild("DivisionAwards").GetComponent<UIGrid>();
        Gobj_DivisionAwardComp = mRootObject.transform.FindChild("DivisionAwards/gobj_DivisionAwardComp").gameObject;
        Spt_DivisionIcon = mRootObject.transform.FindChild("DivisionGroup/DivisionIcon").GetComponent<UISprite>();
        Lbl_DivisionScore = mRootObject.transform.FindChild("DivisionGroup/DivisionScore").GetComponent<UILabel>();
        Gobj_DivisionAwardComp.SetActive(false);
    }


    public void UpdateCompInfo(QualifyingRankData data)
    {
        CommonFunction.SetSpriteName(Spt_DivisionIcon, data.icon);
        Spt_DivisionIcon.MakePixelPerfect();
        UpdateAwardComps(data.dropID);
        Lbl_DivisionScore.text = string.Format(ConstString.QUALIFYING_AWARD_SCOREREQUIRE, data.point_min.ToString());
    }

    private void UpdateAwardComps(uint dropid)
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(dropid);
        if (list == null)
        {
            list = new List<CommonItemData>();
        }
        if (award_dic == null)
            award_dic = new List<QualifyingAwardComponent>();
        int objCount = award_dic.Count;
        if (list.Count < objCount)
        {
            for (int i = list.Count; i < objCount; i++)
            {
                QualifyingAwardComponent comp = award_dic[i];
                if (comp == null)
                    continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            QualifyingAwardComponent comp = null;
            CommonItemData data = list[i];
            if (data == null)
                continue;
            if (i < objCount)
            {
                comp = award_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(Gobj_DivisionAwardComp, Grd_DivisionAwards.transform);
                comp = new QualifyingAwardComponent();
                go.name = "award_" + i.ToString();
                comp.MyStart(go);
                award_dic.Add(comp);
            }
            if (comp == null)
                continue;
            comp.UpdateAwardInfo(data);
            comp.mRootObject.SetActive(true);
        }
        Grd_DivisionAwards.repositionNow = true;
    }
}


public class QualifyBattleLogComponent : BaseComponent
{

    public enum AttackStatus
    {
        Attack = 1,
        Defense = 2,
        RevengeAttack = 3,
    }

    public enum RevengeStatus
    {
        None = 0,
        CanRevenge = 1,
        RevengeSuccess = 2,
    }

    /// <summary>
    /// 战斗结果
    /// </summary>
    private UISprite Spt_BattleResult;
    /// <summary>
    /// 排名变化
    /// </summary>
    private UILabel Lbl_RankChangeLabel;
    /// <summary>
    /// 玩家ICON
    /// </summary>
    private UISprite Spt_IconTexture;
    private UISprite Spt_ItemBgSprite;
    private UISprite Spt_QualitySprite;
    private GameObject Gobj_ItemBaseComp;
    /// <summary>
    /// 攻守标识
    /// </summary>
    private UISprite Spt_CampSprite;
    /// <summary>
    /// 玩家名称
    /// </summary>
    private UILabel Lbl_NameLabel;
    /// <summary>
    /// 玩家等级
    /// </summary>
    private UILabel Lbl_LevelLabel;
    /// <summary>
    /// 复仇按钮背景
    /// </summary>
    private UISprite Spt_RevengeButtonBG;
    /// <summary>
    /// 复仇按钮前置图
    /// </summary>
    private UILabel Lbl_RevengeButtonDesc;
    /// <summary>
    /// 战斗时间戳
    /// </summary>
    private UILabel Lbl_BattleTimeLabel;
    /// <summary>
    /// 复仇按钮
    /// </summary>
    private UIButton Btn_RevengeButton;
    private GameObject Gobj_ConsumeGroup;
    private UISprite Spt_ConsumeIcon;
    private UILabel Lbl_ConsumeNum;
    private UISprite Spt_UnionMark;
    private UISprite Spt_RevengeSuccess;

    private ArenaRecord recordData;

    public delegate void RevengeDelegate(GameObject go, ArenaRecord enemy);
    public RevengeDelegate revengedHandle;
    public delegate void PlayerInfoDelegate(GameObject go, ArenaPlayer info);
    public PlayerInfoDelegate playerInfoHandle;

    public bool IsShowRevage
    {
        set
        {
            if (Btn_RevengeButton != null)
                Btn_RevengeButton.gameObject.SetActive(value);
        }
    }

    public void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_BattleResult = mRootObject.transform.FindChild("BattleResultGroup/BattleResult").gameObject.GetComponent<UISprite>();
        Lbl_RankChangeLabel = mRootObject.transform.FindChild("BattleResultGroup/RankChangeLabel").gameObject.GetComponent<UILabel>();
        Gobj_ItemBaseComp = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp").gameObject;
        Spt_QualitySprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_ItemBgSprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/BG").gameObject.GetComponent<UISprite>();
        Spt_CampSprite = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/CampSprite").gameObject.GetComponent<UISprite>();
        Lbl_LevelLabel = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/LvGroup/LevelLabel").gameObject.GetComponent<UILabel>();
        Lbl_NameLabel = mRootObject.transform.FindChild("EnemyInfoGroup/InfoGroup/NameLabel").gameObject.GetComponent<UILabel>();
        Lbl_BattleTimeLabel = mRootObject.transform.FindChild("EnemyInfoGroup/InfoGroup/BattleTimeLabel").gameObject.GetComponent<UILabel>();
        Btn_RevengeButton = mRootObject.transform.FindChild("RevengeGroup/RevengeButton").gameObject.GetComponent<UIButton>();
        Spt_RevengeButtonBG = mRootObject.transform.FindChild("RevengeGroup/RevengeButton/RevengeButtonBG").gameObject.GetComponent<UISprite>();
        Lbl_RevengeButtonDesc = mRootObject.transform.FindChild("RevengeGroup/RevengeButton/RevengeButtonDesc").gameObject.GetComponent<UILabel>();
        Gobj_ConsumeGroup = mRootObject.transform.FindChild("RevengeGroup/ConsumeGroup").gameObject;
        Spt_ConsumeIcon = mRootObject.transform.FindChild("RevengeGroup/ConsumeGroup/ConsumeIcon").gameObject.GetComponent<UISprite>();
        Lbl_ConsumeNum = mRootObject.transform.FindChild("RevengeGroup/ConsumeGroup/ConsumeNum").gameObject.GetComponent<UILabel>();
        Spt_RevengeSuccess = mRootObject.transform.FindChild("RevengeSuccess").gameObject.GetComponent<UISprite>();
        Spt_UnionMark = mRootObject.transform.FindChild("EnemyInfoGroup/ItemBaseComp/UnionMark").gameObject.GetComponent<UISprite>();
        UIEventListener.Get(Btn_RevengeButton.gameObject).onClick = ButtonEvent_Revenge;
        UIEventListener.Get(Gobj_ItemBaseComp.gameObject).onClick = ButtonEvent_PlayerInfo;
    }

    public void UpdateInfo(ArenaRecord record, bool isPVP = true)
    {
        if (record == null)
        {
            Clear();
            return;
        }
        recordData = record;
        UpdateInfo();
    }

    public void UpdateInfo()
    {
        if (recordData == null) return;
        UpdateBattleResult();
        Lbl_NameLabel.text = recordData.opponent.hero.charname;
        UpdateRevengeButton();
        UpdateEnemyInfo();
        UpdateCampSprite();
        UpdateBattleTime();
        Spt_UnionMark.enabled = (recordData.altar_status == 2) ? true : false;
    }

    private void UpdateEnemyInfo()
    {
        CommonFunction.SetSpriteName(Spt_IconTexture, CommonFunction.GetHeroIconNameByID(recordData.opponent.hero.icon, true));
        QualifyingRankData data = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint(recordData.opponent.score);
        if (data != null)
            CommonFunction.SetSpriteName(Spt_QualitySprite, data.frame);
        Lbl_LevelLabel.text = recordData.opponent.hero.level.ToString();
    }

    /// <summary>
    /// 更新战斗结果
    /// </summary>
    /// <param name="result"></param>
    private void UpdateBattleResult()
    {
        switch ((AttackStatus)recordData.type) //约定1为攻击方  2为防御方
        {
            case AttackStatus.Attack:
            case AttackStatus.RevengeAttack:
                {
                    if (recordData.result == 1)  //约定1为胜利  2为失败
                    {
                        CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogVictory);
                        Lbl_RankChangeLabel.text = string.Format(ConstString.QUALIFYING_ADDINTEGRAL, Mathf.Abs(recordData.rank_change));
                    }
                    else
                    {
                        CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogDefault);
                        Lbl_RankChangeLabel.text = string.Format(ConstString.QUALIFYING_REDUCEINTEGRAL, Mathf.Abs(recordData.rank_change));
                    }

                } break;
            case AttackStatus.Defense:
                {
                    if (recordData.result == 1)  //约定1为胜利  2为失败
                    {
                        CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogVictory);
                        Lbl_RankChangeLabel.text = string.Format(ConstString.QUALIFYING_ADDINTEGRAL, Mathf.Abs(recordData.rank_change));
                    }
                    else
                    {
                        CommonFunction.SetSpriteName(Spt_BattleResult, GlobalConst.SpriteName.PVP_BattleLogDefault);
                        Lbl_RankChangeLabel.text = string.Format(ConstString.QUALIFYING_REDUCEINTEGRAL, Mathf.Abs(recordData.rank_change));
                    }
                } break;
            default:
                Spt_BattleResult.spriteName = string.Empty;
                Lbl_RankChangeLabel.text = string.Empty;
                break;
        }
    }

    private void UpdateCampSprite()
    {
        switch ((AttackStatus)recordData.type)
        {
            case AttackStatus.Attack:
                {
                    CommonFunction.SetSpriteName(Spt_CampSprite, GlobalConst.SpriteName.PVP_BattleLogCampAttack);
                } break;
            case AttackStatus.Defense:
                {
                    CommonFunction.SetSpriteName(Spt_CampSprite, GlobalConst.SpriteName.PVP_BattleLogCampDefense);
                } break;
            case AttackStatus.RevengeAttack:
                {
                    CommonFunction.SetSpriteName(Spt_CampSprite, GlobalConst.SpriteName.PVP_BattleLogCampRevenge);
                } break;
        }
    }

    private void UpdateBattleTime()
    {
        long battleTime = (long)Mathf.Abs(Main.mTime - recordData.time);
        if (battleTime <= 60)
            battleTime = 60;
        Lbl_BattleTimeLabel.text = string.Format(ConstString.PVP_BATTLETIME, CommonFunction.GetIntegerTimeString(battleTime));
    }

    /// <summary>
    /// 复仇按钮是否可用
    /// </summary>
    /// <param name="status"></param>
    public void UpdateRevengeButton()
    {
        switch ((RevengeStatus)recordData.can_revenge)
        {
            case RevengeStatus.None:
                {
                    Btn_RevengeButton.gameObject.SetActive(false);
                    Gobj_ConsumeGroup.SetActive(false);
                    Spt_RevengeSuccess.enabled = false;
                } break;
            case RevengeStatus.CanRevenge:
                {
                    QualifyingAwardData awardConfigData = ConfigManager.Instance.mQualifyingAwardConfig.GetQualifyingAwardData();
                    if (awardConfigData == null)
                        return;
                    int timeLimit = awardConfigData.revenge_time_limit - recordData.revenge_times;
                    //if (timeLimit <= 0)
                    //{
                    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.QUALIFYING_SEASONTIMEDESCTITLE_REVENGELIMIT);
                    //}
                    //else
                    //{
                    ///总复仇次数已达消耗上限
                    if (QualifyingModule.Instance.poleLobbyData.pole_recinf.total_revenge_times >= awardConfigData.total_revenge_limit)
                    {
                        CommonFunction.SetMoneyIcon(Spt_ConsumeIcon, (ECurrencyType)awardConfigData.total_revenge_consumetype);
                        Lbl_ConsumeNum.text = awardConfigData.total_revenge_comsumenum.ToString();
                    }
                    else
                    {
                        TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)(QualifyingModule.Instance.poleLobbyData.pole_recinf.total_revenge_times + 1));
                        if (data != null)
                        {
                            CommonFunction.SetMoneyIcon(Spt_ConsumeIcon, (ECurrencyType)data.QualifyingRevengeConsume.Type);
                            Lbl_ConsumeNum.text = data.QualifyingRevengeConsume.Number.ToString();
                        }
                    }
                    //}
                    Spt_RevengeSuccess.enabled = false;
                    Gobj_ConsumeGroup.SetActive(true);
                    Btn_RevengeButton.gameObject.SetActive(true);
                } break;
            case RevengeStatus.RevengeSuccess:
                {
                    Spt_RevengeSuccess.enabled = true;
                    Gobj_ConsumeGroup.SetActive(false);
                    Btn_RevengeButton.gameObject.SetActive(false);
                } break;
        }
    }

    private void ButtonEvent_PlayerInfo(GameObject go)
    {
        if (playerInfoHandle != null)
        {
            playerInfoHandle(go, recordData.opponent);
        }
    }

    private void ButtonEvent_Revenge(GameObject go)
    {
        if (revengedHandle != null)
        {
            revengedHandle(go, recordData);
        }
    }

    public override void Clear()
    {
        base.Clear();
    }
}


public class QualifyingOpponentComponent : BaseComponent
{
    private UISprite Spt_QualitySprite;
    private UISprite Spt_IconTexture;
    private UISprite Spt_FrameSprite;
    private UILabel Lbl_Level;
    private UILabel Lbl_PlayerName;
    private UILabel Lbl_Rank;
    private UILabel Lbl_GetIntegral;
    private UILabel Lbl_Integral;
    private UILabel Lbl_Power;
    private UIButton Btn_Battle;

    private ArenaPlayer mArenaPlayer;
    public ArenaPlayer ArenaPlayer
    {
        get
        {
            return mArenaPlayer;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        Spt_QualitySprite = mRootObject.transform.FindChild("PlayerInfo/QualitySprite").gameObject.GetComponent<UISprite>();
        Spt_IconTexture = mRootObject.transform.FindChild("PlayerInfo/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_FrameSprite = mRootObject.transform.FindChild("PlayerInfo/back").gameObject.GetComponent<UISprite>();
        Lbl_Level = mRootObject.transform.FindChild("PlayerInfo/LevelBG/Label").gameObject.GetComponent<UILabel>();
        Lbl_PlayerName = mRootObject.transform.FindChild("LabelGroup/Label_Name").gameObject.GetComponent<UILabel>();
        Lbl_Rank = mRootObject.transform.FindChild("LabelGroup/Label_Rank/Label").gameObject.GetComponent<UILabel>();
        Lbl_GetIntegral = mRootObject.transform.FindChild("LabelGroup/GetIntegral").gameObject.GetComponent<UILabel>();
        Lbl_Integral = mRootObject.transform.FindChild("LabelGroup/Label_Integral/Label").gameObject.GetComponent<UILabel>();
        Lbl_Power = mRootObject.transform.FindChild("LabelGroup/Label_Power/Label").gameObject.GetComponent<UILabel>();
        Btn_Battle = mRootObject.transform.FindChild("Battle").gameObject.GetComponent<UIButton>();
        UIEventListener.Get(Btn_Battle.gameObject).onClick = ButtonEvent_Battle;
    }

    public void UpdateCompInfo(ArenaPlayer info, int score)
    {
        mArenaPlayer = info;
        if (mArenaPlayer == null)
            return;
        UpdatePlayerInfo();

        Lbl_GetIntegral.text = string.Format(ConstString.QUALIFYING_OPPONENT_GETSCORE, score);
    }

    private void UpdatePlayerInfo()
    {
        CommonFunction.SetSpriteName(Spt_IconTexture, CommonFunction.GetHeroIconNameByID(mArenaPlayer.hero.icon, true));
        Lbl_PlayerName.text = mArenaPlayer.hero.charname;
        Lbl_Level.text = mArenaPlayer.hero.level.ToString();

        QualifyingRankData data = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint(mArenaPlayer.score);
        if (data != null)
        {
            CommonFunction.SetSpriteName(Spt_QualitySprite, data.frame);
            Lbl_Rank.text = mArenaPlayer.rank.ToString();
            Lbl_Integral.text = mArenaPlayer.score.ToString();
            Lbl_Power.text = mArenaPlayer.combat_power.ToString();
        }
    }

    private void ButtonEvent_Battle(GameObject go)
    {
        if (OnSelectObj != null)
        {
            OnSelectObj(this);
        }
    }

}