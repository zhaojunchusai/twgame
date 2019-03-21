using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class QualifyingViewController : UIBase
{
    public QualifyingView view;

    public class QualifyingInfo
    {
        public ArenaPlayer arenaPlayer;

        public int score;
    }

    private enum GetAwardTypeEnum : byte
    {
        LowDay = 1,
        MidDay = 2,
        HighDay = 3,
        LowWeek = 4,
        MidWeek = 5,
        HighWeek = 6,
    }


    private enum AwardStateEnum
    {
        /// <summary>
        /// 不能领取
        /// </summary>
        None = 1,
        /// <summary>
        /// 可领取  未领取
        /// </summary>
        NotGet = 2,
        /// <summary>
        /// 已经领取
        /// </summary>
        HasGet = 3,
    }

    private class DefenseSoldier
    {
        public Soldier soldier;
        public int count;
    }
    /// <summary>
    /// 己方防御阵容
    /// </summary>
    private List<PVPSoldierComponent> defenseSoldier_dic;

    /// <summary>
    /// 战斗日志
    /// </summary>
    private List<QualifyBattleLogComponent> battleLog_dic;

    private List<QualifyingDivisionAwardsComponent> divisionAward_dic;

    private List<QualifyingOpponentComponent> opponent_dic;

    private QualifyingAwardData awardConfigData;
    private QualifyingRankData rankConfigData;
    private bool isDayAward = false;
    private ArenaPlayer revengeEnemy;
    private Attribute self_att;
    private PetData currentPetData;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new QualifyingView();
            view.Initialize();
            BtnEventBinding();
        }
        if (defenseSoldier_dic == null)
            defenseSoldier_dic = new List<PVPSoldierComponent>();
        if (battleLog_dic == null)
            battleLog_dic = new List<QualifyBattleLogComponent>();
        if (divisionAward_dic == null)
            divisionAward_dic = new List<QualifyingDivisionAwardsComponent>();
        if (opponent_dic == null)
            opponent_dic = new List<QualifyingOpponentComponent>();
        view.Gobj_BattleLogComp.SetActive(false);
        view.Gobj_DefenseSoldierComp.SetActive(false);
        view.Gobj_BattleLogPanel.SetActive(false);
        view.Gobj_AwardGroupPanel.SetActive(false);
        view.Gobj_RulePanel.SetActive(false);
        view.Gobj_SeasonRewardsPanel.SetActive(false);
        view.Gobj_DivisionAwardGroup.SetActive(false);
        view.Gobj_SelectOpponent.SetActive(false);
        view.Gobj_Main.SetActive(true);
        view.Gobj_DayAwardEffect.SetActive(false);
        view.Gobj_HighAwardEffect.SetActive(false);
        view.Gobj_LowAwardEffect.SetActive(false);
        view.Gobj_MidAwardEffect.SetActive(false);
        view.Gobj_WeekAwardEffect.SetActive(false);
        view.Gobj_OpponentComp.SetActive(false);
        view.Spt_DivisionEffect.enabled = false;
        PlayerData.Instance.NotifyResetEvent += OnNotifyReset;
        QualifyingModule.Instance.SendEnterPoleLobby();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenQualifyingView);
    }

    private void OnNotifyReset(NotifyReset data)
    {
        if (QualifyingModule.Instance.poleLobbyData != null)
        {
            QualifyingModule.Instance.poleLobbyData.pole_recinf = data.pole_recinf;
            QualifyingModule.Instance.poleLobbyData.buy_times = 0;
        }
        UpdateMainPanelInfo();
    }

    public void ReceiveEnterQualify()
    {
        UpdateViewInfo();
    }

    #region Update Event

    public void UpdateDefenseInfo(List<SoldierList> soldiers, List<EquipList> equips, int combat_power)
    {
        QualifyingModule.Instance.poleLobbyData.defence_soldiers.Clear();
        QualifyingModule.Instance.poleLobbyData.defence_soldiers.AddRange(soldiers);
        QualifyingModule.Instance.poleLobbyData.defence_equips.Clear();
        QualifyingModule.Instance.poleLobbyData.defence_equips.AddRange(equips);
        QualifyingModule.Instance.poleLobbyData.combat_power = combat_power;
        UpdateDefenseSoldiers();
        UpdateCombatPower();
    }

    private void UpdateViewInfo()
    {
        rankConfigData = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByPoint(QualifyingModule.Instance.poleLobbyData.pole_recinf.season_score);
        awardConfigData = ConfigManager.Instance.mQualifyingAwardConfig.GetQualifyingAwardData();
        UpdateDefenseSoldiers();
        UpdateCombatPower();
        UpdateMainPanelInfo();
        if (QualifyingModule.Instance.poleLobbyData.rank_up == 1)
        {
            PlayDivisionEffect();
        }
    }

    private void PlayDivisionEffect()
    {
        CommonFunction.SetSpriteAtlasByName(view.Spt_DivisionEffect, rankConfigData.divisionup_effect);
        if (view.Spt_DivisionEffect.atlas != null)
        {
            view.Spt_DivisionEffect.enabled = true;
            CommonFunction.SetSpriteName(view.Spt_DivisionEffect, view.Spt_DivisionEffect.atlas.spriteList[0].name);
            view.SptAnim_DivisionEffect.RebuildSpriteList();
            view.SptAnim_DivisionEffect.ResetToBeginning();
            view.SptAnim_DivisionEffect.Play();
            Scheduler.Instance.AddUpdator(DivisionEffectEnd);
        }
    }

    private void DivisionEffectEnd()
    {
        if (!view.SptAnim_DivisionEffect.isPlaying)
        {
            view.Spt_DivisionEffect.enabled = false;
            Scheduler.Instance.RemoveUpdator(DivisionEffectEnd);
        }
    }

    private void UpdateMainPanelInfo()
    {
        view.Lbl_RankIntegral.text = QualifyingModule.Instance.poleLobbyData.pole_recinf.season_score.ToString();
        if (QualifyingModule.Instance.poleLobbyData.pole_recinf.season_rank < 0) //约定小于0时  表示未上榜 add by taiwei
        {
            view.Lbl_RankNum.text = ConstString.RANK_LABEL_OUTRANK;
        }
        else
        {
            view.Lbl_RankNum.text = QualifyingModule.Instance.poleLobbyData.pole_recinf.season_rank.ToString();
        }
        view.Lbl_PlaySessions.text = QualifyingModule.Instance.poleLobbyData.pole_recinf.season_times.ToString();
        view.Lbl_WinsSessions.text = QualifyingModule.Instance.poleLobbyData.pole_recinf.season_wintimes.ToString();
        UpdateAwardDesc();
        UpdateRankIcon();
        UpdateSeasonTimeDesc();
        UpdatePurchaseStatus();
        UpdateAwardsEffect();
    }

    private void UpdateSeasonTimeDesc()
    {
        // 0 未设置赛季开始时间, 1 已设置开始时间 但未开始, 2 已设置开始时间 且已开始
        switch (QualifyingModule.Instance.poleLobbyData.status)
        {
            case 0:
                {
                    view.Lbl_SeasonTimeDescTitle.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_SEASONSTATUS;
                    view.Lbl_SeasonTimeDesc.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_OFFSEASON;
                } break;
            case 1:
                {
                    Scheduler.Instance.AddUpdator(UpdateSeasonStartCD);
                } break;
            case 2:
                {
                    Scheduler.Instance.AddUpdator(UpdateSeasonEndCD);
                } break;
        }
    }

    private void UpdateSeasonStartCD()
    {
        if (QualifyingModule.Instance.poleLobbyData.start_time <= Main.mTime) //赛季已开始
        {
            Scheduler.Instance.RemoveUpdator(UpdateSeasonStartCD);
            Scheduler.Instance.AddUpdator(UpdateSeasonEndCD);
        }
        else
        {
            QualifyingModule.Instance.poleLobbyData.status = 1;  //修改赛季状态为 2（赛季已开始）
            view.Lbl_SeasonTimeDescTitle.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_SEASONSTART;
            long startCD = (long)QualifyingModule.Instance.poleLobbyData.start_time - Main.mTime;
            long day = startCD / 86400;
            if (day > 0)
            {
                view.Lbl_SeasonTimeDesc.text = string.Format(ConstString.FORMAT_INTEGERTIME_DAY, day);
            }
            else
            {
                view.Lbl_SeasonTimeDesc.text = CommonFunction.GetTimeString(startCD);
            }
        }
    }

    private void UpdateSeasonEndCD()
    {
        if (QualifyingModule.Instance.poleLobbyData.end_time <= Main.mTime)
        {
            SeasonOverReset();
            view.Lbl_SeasonTimeDescTitle.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_SEASONSTATUS;
            view.Lbl_SeasonTimeDesc.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_OFFSEASON;
            Scheduler.Instance.RemoveUpdator(UpdateSeasonEndCD);
            UpdateMainPanelInfo();
        }
        else
        {
            QualifyingModule.Instance.poleLobbyData.status = 2;  //修改赛季状态为 2（赛季已开始）
            view.Lbl_SeasonTimeDescTitle.text = ConstString.QUALIFYING_SEASONTIMEDESCTITLE_SEASONEND;
            long endCD = (long)QualifyingModule.Instance.poleLobbyData.end_time - Main.mTime;
            long day = endCD / 86400;
            if (day > 0)
            {
                view.Lbl_SeasonTimeDesc.text = string.Format(ConstString.FORMAT_INTEGERTIME_DAY, day);
            }
            else
            {
                view.Lbl_SeasonTimeDesc.text = CommonFunction.GetTimeString(endCD);
            }
        }
    }

    private void SeasonOverReset()
    {
        QualifyingModule.Instance.poleLobbyData.status = 0;
        QualifyingModule.Instance.poleLobbyData.buy_times = 0;
        QualifyingModule.Instance.poleLobbyData.combat_power = 0;
        QualifyingModule.Instance.poleLobbyData.end_time = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.week_times = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.season_score = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.season_rank = -1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.season_times = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.season_wintimes = 0;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.day_low = 1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.day_mid = 1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.day_high = 1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.week_low = 1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.week_mid = 1;
        QualifyingModule.Instance.poleLobbyData.pole_recinf.week_high = 1;
    }


    public void UpdatePurchaseStatus()
    {
        // 0 未设置赛季开始时间, 1 已设置开始时间 但未开始, 2 已设置开始时间 且已开始
        switch (QualifyingModule.Instance.poleLobbyData.status)
        {
            case 0:
            case 1:
                {
                    view.Gobj_RightTopGroup.SetActive(false);
                } break;
            case 2:
                {
                    view.Gobj_RightTopGroup.SetActive(true);
                    int challengeTimes = awardConfigData.challenge_time_limit * (QualifyingModule.Instance.poleLobbyData.buy_times + 1);
                    int surplusTimes = challengeTimes - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                    if (surplusTimes <= 0)
                    {
                        surplusTimes = 0;
                        view.Lbl_ChanglleTimeTip.text = string.Format(ConstString.ACITIVITIES_CDTODAY, surplusTimes + "/" + awardConfigData.challenge_time_limit);
                        view.Btn_Purchase.gameObject.SetActive(true);
                    }
                    else
                    {
                        view.Lbl_ChanglleTimeTip.text = string.Format(ConstString.ACITIVITIES_CDTODAY, surplusTimes + "/" + awardConfigData.challenge_time_limit);
                        view.Btn_Purchase.gameObject.SetActive(false);
                    }
                } break;
        }
    }


    /// <summary>
    /// 更新主面板  战斗奖励提示
    /// </summary>
    private void UpdateAwardDesc()
    {
        view.Lbl_DayBattleTipTitle.text = ConstString.QUALIFYING_AWARDRANK_BATTLEREQUIRE;
        view.Lbl_WeekBattleTipTitle.text = ConstString.QUALIFYING_AWARDRANK_WINSREQUIRE;
        view.Lbl_SeasonBattleTipTitle.text = ConstString.QUALIFYING_AWARDRANK_SCOREREQUIRE;
        if (awardConfigData == null)
        {
            view.Lbl_DayBattleTipNum.text = "0";
        }
        else
        {
            //===========每日奖励=================//
            if (QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times >= awardConfigData.day_require_high)
            {
                view.Lbl_DayBattleTipNum.text = "0";
            }
            else if (awardConfigData.day_require_mid <= QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times && QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times < awardConfigData.day_require_high)
            {
                int num = (int)awardConfigData.day_require_high - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                view.Lbl_DayBattleTipNum.text = num.ToString();
            }
            else if (awardConfigData.day_require_low <= QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times && QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times < awardConfigData.day_require_mid)
            {
                int num = (int)awardConfigData.day_require_mid - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                view.Lbl_DayBattleTipNum.text = num.ToString();
            }
            else if (QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times < awardConfigData.day_require_low)
            {
                int num = (int)awardConfigData.day_require_low - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                view.Lbl_DayBattleTipNum.text = num.ToString();
            }
            //===========每周奖励=================//
            if (QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes >= awardConfigData.week_require_high)
            {
                view.Lbl_WeekBattleTipNum.text = "0";
            }
            else if (awardConfigData.week_require_mid <= QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes && QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes < awardConfigData.week_require_high)
            {
                int num = (int)awardConfigData.week_require_high - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                view.Lbl_WeekBattleTipNum.text = num.ToString();
            }
            else if (awardConfigData.week_require_low <= QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes && QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes < awardConfigData.week_require_mid)
            {
                int num = (int)awardConfigData.week_require_mid - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                view.Lbl_WeekBattleTipNum.text = num.ToString();
            }
            else if (QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes < awardConfigData.week_require_low)
            {
                int num = (int)awardConfigData.week_require_low - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                view.Lbl_WeekBattleTipNum.text = num.ToString();
            }
        }
        if (rankConfigData == null)
        {
            view.Lbl_SeasonBattleTipNum.text = "0";
            view.Spt_RankIcon.spriteName = string.Empty;
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_RankIcon, rankConfigData.icon);
            if (rankConfigData.nextID == 0) //最高等级
            {
                view.Lbl_SeasonBattleTipNum.text = "0";
            }
            else
            {
                QualifyingRankData tmpData = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataByID(rankConfigData.nextID);
                view.Lbl_SeasonBattleTipNum.text = (tmpData.point_min - QualifyingModule.Instance.poleLobbyData.pole_recinf.season_score).ToString();
            }
        }
    }

    private void UpdateAwardsEffect() //更新主面板奖励显示 
    {
        PoleRecInf lPolerecInf = QualifyingModule.Instance.poleLobbyData.pole_recinf;
        AwardStateEnum lIsShowDayEffect = AwardStateEnum.None;
        if (((AwardStateEnum)lPolerecInf.day_low == AwardStateEnum.NotGet) || ((AwardStateEnum)lPolerecInf.day_mid == AwardStateEnum.NotGet) || ((AwardStateEnum)lPolerecInf.day_high == AwardStateEnum.NotGet))
        {
            lIsShowDayEffect = AwardStateEnum.NotGet;
        }
        else if (((AwardStateEnum)lPolerecInf.day_low == AwardStateEnum.HasGet) && ((AwardStateEnum)lPolerecInf.day_mid == AwardStateEnum.HasGet) && ((AwardStateEnum)lPolerecInf.day_high == AwardStateEnum.HasGet))
        {
            lIsShowDayEffect = AwardStateEnum.HasGet;
        }
        else
        {
            lIsShowDayEffect = AwardStateEnum.None;
        }

        AwardStateEnum lIsShowWeekEffect = AwardStateEnum.None;
        if (((AwardStateEnum)lPolerecInf.week_low == AwardStateEnum.NotGet) || ((AwardStateEnum)lPolerecInf.week_mid == AwardStateEnum.NotGet) || ((AwardStateEnum)lPolerecInf.week_high == AwardStateEnum.NotGet))
        {
            lIsShowWeekEffect = AwardStateEnum.NotGet;
        }
        else if (((AwardStateEnum)lPolerecInf.week_low == AwardStateEnum.HasGet) && ((AwardStateEnum)lPolerecInf.week_mid == AwardStateEnum.HasGet) && ((AwardStateEnum)lPolerecInf.week_high == AwardStateEnum.HasGet))
        {
            lIsShowWeekEffect = AwardStateEnum.HasGet;
        }
        else
        {
            lIsShowWeekEffect = AwardStateEnum.None;
        }
        switch (lIsShowDayEffect)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_DayAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_DayAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                } break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_DayAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_DayAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_DayAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_DayAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Open);
                } break;
        }
        switch (lIsShowWeekEffect)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_WeekAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_WeekAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                } break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_WeekAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_WeekAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_WeekAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_WeekAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Open);
                } break;
        }
    }

    private void UpdateRankIcon()
    {
        if (rankConfigData == null)
        {
            view.Spt_RankIcon.spriteName = string.Empty;
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_RankIcon, rankConfigData.icon);
            view.Spt_RankIcon.MakePixelPerfect();
        }
    }

    private void UpdateCombatPower()
    {
        view.Lbl_CombatPower.text = QualifyingModule.Instance.poleLobbyData.combat_power.ToString();
    }


    /// <summary>
    /// 更新主面板 防御阵容
    /// </summary>
    private void UpdateDefenseSoldiers()
    {
        List<DefenseSoldier> list = new List<DefenseSoldier>();
        for (int i = 0; i < QualifyingModule.Instance.poleLobbyData.defence_soldiers.Count; i++)
        {
            SoldierList defenceSoldier = QualifyingModule.Instance.poleLobbyData.defence_soldiers[i];
            DefenseSoldier readySoldier = new DefenseSoldier();
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(QualifyingModule.Instance.poleLobbyData.defence_soldiers[i].uid);
            if (soldier == null) continue;  //有可能士兵已经被消耗
            readySoldier.soldier = soldier;
            readySoldier.count = defenceSoldier.num;
            list.Add(readySoldier);
        }
        if (list.Count <= defenseSoldier_dic.Count)
        {
            for (int i = 0; i < defenseSoldier_dic.Count; i++)
            {
                PVPSoldierComponent comp = defenseSoldier_dic[i];
                if (i < list.Count)
                {
                    comp.UpdateInfo(list[i].soldier, list[i].count);
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int goCount = defenseSoldier_dic.Count;
            for (int i = 0; i < list.Count; i++)
            {
                DefenseSoldier info = list[i];
                PVPSoldierComponent comp = null;
                if (i < goCount)
                {
                    comp = defenseSoldier_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_DefenseSoldierComp, view.Grd_DefenseSoldierGrid.transform);
                    go.name = "Soldier_" + i.ToString();
                    comp = new PVPSoldierComponent(go);
                    defenseSoldier_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(info.soldier, info.count);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_DefenseSoldierGrid.Reposition();
    }

    private IEnumerator UpdateBattleLogPanelInfo(List<ArenaRecord> recordList)
    {
        view.ScrView_BattleLog.ResetPosition();
        yield return null;
        recordList.Sort((ArenaRecord left, ArenaRecord right) =>
        {
            if (left == null || right == null) return 0;
            return right.time.CompareTo(left.time);
        });
        if (recordList.Count <= battleLog_dic.Count)
        {
            for (int i = 0; i < battleLog_dic.Count; i++)
            {
                QualifyBattleLogComponent comp = battleLog_dic[i];
                if (i < recordList.Count)
                {
                    comp.UpdateInfo(recordList[i], false);
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int ob_count = battleLog_dic.Count;   //已经生成的物件数量
            for (int i = 0; i < recordList.Count; i++)
            {
                QualifyBattleLogComponent comp = null;
                if (i < ob_count)
                {
                    comp = battleLog_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_BattleLogComp.gameObject, view.Grd_BattleLog.transform);
                    go.name = "battlelog_" + i.ToString();
                    comp = new QualifyBattleLogComponent();
                    comp.MyStart(go);
                    comp.playerInfoHandle = ButtonEvent_PlayerInfo;
                    comp.revengedHandle = ButtonEvent_Revenge;
                    battleLog_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(recordList[i], false);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_BattleLog.repositionNow = true;
        yield return null;
        view.ScrView_BattleLog.ResetPosition();
    }

    private void OpenRulePanel()
    {
        view.Gobj_RulePanel.SetActive(true);
        view.ScrollView_RuleDesc.ResetPosition();
        if (awardConfigData == null)
        {
            view.Lbl_RuleDesc.text = string.Empty;
        }
        else
        {
            view.Lbl_RuleDesc.text = CommonFunction.ReplaceEscapeChar(awardConfigData.ruledesc);
        }
    }

    public void OpenBattleLogPanel(List<ArenaRecord> list)
    {
        view.Gobj_BattleLogPanel.SetActive(true);
        Main.Instance.StartCoroutine(UpdateBattleLogPanelInfo(list));
    }

    public void OpenAwardPanel(bool isDay)
    {
        view.Gobj_AwardGroupPanel.SetActive(true);
        isDayAward = isDay;
        //图片一样 
        if (isDayAward)
        {
            UpdateDayAwardStatus();
        }
        else
        {
            UpdateWeekAwardStatus();
        }
        UpdateAwardPanelSlider();
    }

    private void UpdateDayAwardStatus()
    {
        view.Lbl_AwardTipTitle.text = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.QUALIFYING_DAYAWARDDESC);
        view.Lbl_AwardTitle.text = ConstString.QUALIFYING_AWARDPANEL_DAYAWARDTITLE;
        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_low)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_LowAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_LowestAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.day_require_low - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_LowestAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDBATTLEREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_LowAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                    view.Lbl_LowestAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_LowAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Open);
                    view.Lbl_LowestAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }
        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_mid)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_MidAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_MiddleAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.day_require_mid - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_MiddleAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDBATTLEREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_MidAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                    view.Lbl_MiddleAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_MidAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Open);
                    view.Lbl_MiddleAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }

        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_high)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_HighAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_HighestAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.day_require_high - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_HighestAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDBATTLEREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_HighAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Close);
                    view.Lbl_HighestAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_HighAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Open);
                    view.Lbl_HighestAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }
    }

    private void UpdateWeekAwardStatus()
    {
        view.Lbl_AwardTipTitle.text = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.QUALIFYING_WEEKAWARDDESC);
        view.Lbl_AwardTitle.text = ConstString.QUALIFYING_AWARDPANEL_WEEKAWARDTITLE;
        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_low)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_LowAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_LowestAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.week_require_low - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_LowestAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDWINSREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_LowAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Close);
                    view.Lbl_LowestAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_LowAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_LowestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Low_Open);
                    view.Lbl_LowestAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }
        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_mid)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_MidAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_MiddleAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.week_require_mid - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_MiddleAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDWINSREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_MidAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Close);
                    view.Lbl_MiddleAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_MidAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_MiddleAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_Mid_Open);
                    view.Lbl_MiddleAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }

        switch ((AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_high)
        {
            case AwardStateEnum.None:
                {
                    view.Gobj_HighAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Close);
                    if (awardConfigData == null)
                    {
                        view.Lbl_HighestAwardTip.text = string.Empty;
                    }
                    else
                    {
                        int num = (int)awardConfigData.week_require_high - QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes;
                        if (num <= 0)
                        {
                            num = 0;
                        }
                        view.Lbl_HighestAwardTip.text = string.Format(ConstString.QUALIFYING_AWARDPANEL_NEEDWINSREQUIRE, num);
                    }
                }
                break;
            case AwardStateEnum.NotGet:
                {
                    view.Gobj_HighAwardEffect.SetActive(true);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Close);
                    view.Lbl_HighestAwardTip.text = ConstString.GATE_CANGETAWARDS;
                } break;
            case AwardStateEnum.HasGet:
                {
                    view.Gobj_HighAwardEffect.SetActive(false);
                    CommonFunction.SetSpriteName(view.Spt_HighestAwardIcon, GlobalConst.SpriteName.Qualifuing_Award_High_Open);
                    view.Lbl_HighestAwardTip.text = ConstString.GAMEACTIVTIY_LABEL_ALREADYGETAWARD;
                } break;
        }
    }

    private void UpdateAwardPanelSlider()
    {
        if (isDayAward)
        {
            view.Slider_AwradsSlider.value = (float)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times / (float)awardConfigData.day_require_high;
        }
        else
        {
            view.Slider_AwradsSlider.value = (float)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_wintimes / (float)awardConfigData.week_require_high;
        }
    }

    private void OpenSeasonRewardsPanel()
    {
        view.Gobj_SeasonRewardsPanel.SetActive(true);
        List<QualifyingRankData> list = ConfigManager.Instance.mQualifyingRankConfig.GetRankDataList();
        if (list == null)
            list = new List<QualifyingRankData>();
        Main.Instance.StartCoroutine(UpdateSeasonRewards(list));
    }

    private IEnumerator UpdateSeasonRewards(List<QualifyingRankData> list)
    {
        int objCount = divisionAward_dic.Count;
        if (list.Count < objCount)
        {
            for (int i = objCount; i < divisionAward_dic.Count; i++)
            {
                QualifyingDivisionAwardsComponent comp = divisionAward_dic[i];
                if (comp == null)
                    continue;
                comp.mRootObject.SetActive(false);
            }
        }
        yield return null;
        for (int i = 0; i < list.Count; i++)
        {
            QualifyingRankData data = list[i];
            if (data == null)
                continue;
            QualifyingDivisionAwardsComponent comp = null;
            if (i < objCount)
            {
                comp = divisionAward_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_DivisionAwardGroup, view.Table_GetDivisionAwards.transform);
                go.name = "divisionAward_" + i;
                comp = new QualifyingDivisionAwardsComponent();
                comp.MyStart(go);
                divisionAward_dic.Add(comp);
            }
            if (comp == null)
                continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data);
        }
        yield return null;
        view.Table_GetDivisionAwards.Reposition();
        yield return null;
        view.ScrollView_SeasonRewards.ResetPosition();
    }

    private IEnumerator UpdateMatchedOpponents(List<QualifyingInfo> list)
    {
        view.ScrollView_OpponentScroll.ResetPosition();
        yield return null;
        int obj_count = opponent_dic.Count;
        if (list.Count <= obj_count)
        {
            for (int i = list.Count; i < obj_count; i++)
            {
                QualifyingOpponentComponent comp = opponent_dic[i];
                comp.mRootObject.SetActive(false);
            }
        }
        //已经生成的物件数量
        for (int i = 0; i < list.Count; i++)
        {
            QualifyingInfo info = list[i];
            if (info == null)
                continue;
            QualifyingOpponentComponent comp = null;
            if (i < obj_count)
            {
                comp = opponent_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_OpponentComp.gameObject, view.Grd_Opponent.transform);
                go.name = "opponent_" + i.ToString();
                comp = new QualifyingOpponentComponent();
                comp.MyStart(go);
                comp.OnSelectObj = ButtonEvent_SelectOpponent;
                opponent_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(info.arenaPlayer, info.score);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_Opponent.repositionNow = true;
        yield return null;
        view.ScrollView_OpponentScroll.ResetPosition();
    }

    private float mNextRemacthTick = 0;

    public void UpdateSelectOpponentPanel(bool isRematch, uint next_tick, List<QualifyingInfo> list)
    {
        view.Gobj_SelectOpponent.SetActive(true);
        Main.Instance.StartCoroutine(UpdateMatchedOpponents(list));
        if (awardConfigData.clearcd_consumeType == 0 && awardConfigData.clearcd_consumeNum == 0)
        {
            view.Btn_ReMatach.gameObject.SetActive(false);
        }
        else
        {
            view.Btn_ReMatach.gameObject.SetActive(true);
        }
        mNextRemacthTick = (float)((long)next_tick - Main.mTime);
        UpdateRematchCD(mNextRemacthTick);
    }

    public void UpdateRematchCD(float cdtime)
    {
        mNextRemacthTick = cdtime;
        if (mNextRemacthTick >= awardConfigData.clear_cdtime)
        {
            mNextRemacthTick = awardConfigData.clear_cdtime;
        }
        if (mNextRemacthTick > 0)
        {
            Scheduler.Instance.AddUpdator(UpdateRematchTip);
        }
        else
        {
            view.Lbl_ReMatchCDTip.text = string.Empty;
            view.Lbl_BtnReMatch.text = ConstString.QUALIFYING_REMATCH_REMATCH;
        }
    }

    private void UpdateRematchTip()
    {
        if (mNextRemacthTick <= 0)
        {
            view.Lbl_ReMatchCDTip.text = string.Empty;
            view.Lbl_BtnReMatch.text = ConstString.QUALIFYING_REMATCH_REMATCH;
            Scheduler.Instance.RemoveUpdator(UpdateRematchTip);
        }
        else
        {
            mNextRemacthTick -= Time.deltaTime;
            view.Lbl_ReMatchCDTip.text = string.Format(ConstString.QUALIFYING_REMATCH_CDTIP, CommonFunction.GetTimeString((long)mNextRemacthTick));
            view.Lbl_BtnReMatch.text = ConstString.QUALIFYING_REMATCH_CLEARCD;
        }
    }

    #endregion

    #region Button Event

    private void ButtonEvent_CloseRule(GameObject btn)
    {
        view.Gobj_RulePanel.SetActive(false);
    }

    private void ButtonEvent_RuleDesc(GameObject btn)
    {
        OpenRulePanel();
    }

    private void ButtonEvent_Rank(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RANKVIEW);
        UISystem.Instance.RankView.ShowRank(RankType.POLE_RANK);
    }

    private void ButtonEvent_BattleLog(GameObject btn)
    {
        QualifyingModule.Instance.SendPoleRecord();
    }

    private void ButtonEvent_Remacth(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (mNextRemacthTick <= 0)   //重新匹配
        {
            QualifyingModule.Instance.SendMatchPole(2);
        }
        else
        {
            if (CommonFunction.CheckMoneyEnough((ECurrencyType)awardConfigData.clearcd_consumeType, awardConfigData.clearcd_consumeNum, true))
            {
                if (PlayerData.Instance._QualifyingCDTip)
                {
                    string consumedesc = awardConfigData.clearcd_consumeNum.ToString() + CommonFunction.GetConsumeTypeDesc((ECurrencyType)awardConfigData.clearcd_consumeType);
                    string content = string.Format(ConstString.QUALIFYING_CLEARCD_CONSUME, consumedesc);
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark, content, () =>
                    {
                        if (PlayerData.Instance._QualifyingCDTip)
                        {
                            QualifyingModule.Instance.SendClearPoleMatchCD(0);
                        }
                        else
                        {
                            QualifyingModule.Instance.SendClearPoleMatchCD(1);
                        }
                    }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO, (tip) =>
                    {
                        PlayerData.Instance._QualifyingCDTip = !tip;
                    }, false);
                }
                else
                {
                    QualifyingModule.Instance.SendClearPoleMatchCD(1);
                }
            }
        }
    }

    private void ButtonEvent_StartMacth(GameObject btn)
    {
        if (QualifyingModule.Instance.poleLobbyData.status == 2)
        {
            int challengeTimes = awardConfigData.challenge_time_limit * (QualifyingModule.Instance.poleLobbyData.buy_times + 1);
            int surplusTimes = challengeTimes - QualifyingModule.Instance.poleLobbyData.pole_recinf.day_times;
            if (surplusTimes <= 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CHALLENGE_NOCOUNT);
            }
            else
            {
                QualifyingModule.Instance.SendMatchPole(1);
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.QUALIFYING_MAIN_OFFSEASONTIP, () =>
            {
                QualifyingModule.Instance.SendMatchPole(1);
            });

        }
    }

    private void ButtonEvent_AdjustLineup(GameObject btn)
    {
        PrepareCommonData lCommonData = new PrepareCommonData();
        lCommonData.IsAdjust = true;
        lCommonData.FightType = EFightType.eftQualifying;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(lCommonData);
    }

    private void ButtonEvent_Exit(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_QUALIFYING);
    }

    private void ButtonEvent_CloseBattleLog(GameObject btn)
    {
        view.Gobj_BattleLogPanel.SetActive(false);
    }

    private void ButtonEvent_AwardSureButton(GameObject btn)
    {
        view.Gobj_AwardGroupPanel.SetActive(false);
    }

    private void ButtonEvent_PlayerInfo(GameObject go, ArenaPlayer info)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (info != null)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
            UISystem.Instance.PlayerInfoView.UpdateQualifyingViewInfo(PlayerInfoTypeEnum.QualifyingLog, info);
        }
    }

    private void ButtonEvent_SelectOpponent(BaseComponent basecomp)
    {
        QualifyingOpponentComponent comp = basecomp as QualifyingOpponentComponent;
        if (comp == null)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
        UISystem.Instance.PlayerInfoView.UpdateQualifyingViewInfo(PlayerInfoTypeEnum.QualifyingAttack, comp.ArenaPlayer);
    }

    private void ButtonEvent_Revenge(GameObject go, ArenaRecord enemy)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        revengeEnemy = null;
        if (enemy == null || awardConfigData == null)
            return;
        int timeLimit = awardConfigData.revenge_time_limit - enemy.revenge_times;
        if (timeLimit <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.QUALIFYING_SEASONTIMEDESCTITLE_REVENGELIMIT);
        }
        else
        {
            ///总复仇次数已达消耗上限
            if (QualifyingModule.Instance.poleLobbyData.pole_recinf.total_revenge_times >= awardConfigData.total_revenge_limit)
            {
                string consumedesc = awardConfigData.total_revenge_comsumenum.ToString() + CommonFunction.GetConsumeTypeDesc((ECurrencyType)awardConfigData.total_revenge_consumetype);
                string content = string.Format(ConstString.QUALIFYING_BATTLELOG_CONSUME, consumedesc);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content, () =>
                {
                    revengeEnemy = enemy.opponent;
                    QualifyingModule.Instance.SetIsRevenge(true, enemy.uid);
                    QualifyingModule.Instance.SendPoleRevenge(enemy.opponent.hero.accid, enemy.uid);
                }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
            }
            else
            {
                TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)(QualifyingModule.Instance.poleLobbyData.pole_recinf.total_revenge_times + 1));
                if (data != null)
                {
                    string consumedesc = data.QualifyingRevengeConsume.Number.ToString() + CommonFunction.GetConsumeTypeDesc(data.QualifyingRevengeConsume.Type);
                    string content = string.Format(ConstString.QUALIFYING_BATTLELOG_CONSUME, consumedesc);
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content, () =>
                    {
                        revengeEnemy = enemy.opponent;
                        QualifyingModule.Instance.SetIsRevenge(true, enemy.uid);
                        QualifyingModule.Instance.SendPoleRevenge(enemy.opponent.hero.accid, enemy.uid);
                    }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                }
                else
                {
                    revengeEnemy = enemy.opponent;
                    QualifyingModule.Instance.SetIsRevenge(true, enemy.uid);
                    QualifyingModule.Instance.SendPoleRevenge(enemy.opponent.hero.accid, enemy.uid);
                }
            }
        }
    }

    /// <summary>
    /// 每日奖励
    /// </summary>
    private void ButtonEvent_EveryDayAward(GameObject go)
    {
        OpenAwardPanel(true);
    }

    /// <summary>
    /// 每周奖励
    /// </summary>
    private void ButtonEvent_EveryWeekAward(GameObject go)
    {
        OpenAwardPanel(false);
    }

    /// <summary>
    /// 赛季奖励
    /// </summary>
    private void ButtonEvent_EverySeasonAward(GameObject go)
    {
        OpenSeasonRewardsPanel();
    }

    private void ButtonEvent_LowestAward(GameObject go)
    {
        AwardStateEnum awardStatus = AwardStateEnum.None;
        if (isDayAward)
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_low;
        }
        else
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_low;
        }
        switch (awardStatus)
        {
            case AwardStateEnum.None:
                {
                    List<CommonItemData> list = null;
                    if (isDayAward)
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.day_reward_low);
                    }
                    else
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.week_reward_low);
                    }
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                    UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
                } break;
            case AwardStateEnum.NotGet:
                {
                    if (isDayAward)
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.LowDay);
                    else
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.LowWeek);
                } break;
            case AwardStateEnum.HasGet:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_HADGETAWARDS);
                } break;
        }
    }
    private void ButtonEvent_MiddleAward(GameObject go)
    {
        AwardStateEnum awardStatus = AwardStateEnum.None;
        if (isDayAward)
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_mid;
        }
        else
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_mid;
        }
        switch (awardStatus)
        {
            case AwardStateEnum.None:
                {
                    List<CommonItemData> list = null;
                    if (isDayAward)
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.day_reward_mid);
                    }
                    else
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.week_reward_mid);
                    }
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                    UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
                } break;
            case AwardStateEnum.NotGet:
                {
                    if (isDayAward)
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.MidDay);
                    else
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.MidWeek);
                } break;
            case AwardStateEnum.HasGet:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_HADGETAWARDS);
                } break;
        }
    }
    private void ButtonEvent_HighestAward(GameObject go)
    {
        AwardStateEnum awardStatus = AwardStateEnum.None;
        if (isDayAward)
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.day_high;
        }
        else
        {
            awardStatus = (AwardStateEnum)QualifyingModule.Instance.poleLobbyData.pole_recinf.week_high;
        }
        switch (awardStatus)
        {
            case AwardStateEnum.None:
                {
                    List<CommonItemData> list = null;
                    if (isDayAward)
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.day_reward_high);
                    }
                    else
                    {
                        list = CommonFunction.GetCommonItemDataList(awardConfigData.week_reward_high);
                    }
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                    UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
                } break;
            case AwardStateEnum.NotGet:
                {
                    if (isDayAward)
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.HighDay);
                    else
                        QualifyingModule.Instance.SendPoleReward((int)GetAwardTypeEnum.HighWeek);
                } break;
            case AwardStateEnum.HasGet:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_HADGETAWARDS);
                } break;
        }
    }

    private void ButtonEvent_ColseSeasonRewards(GameObject go)
    {
        view.Gobj_SeasonRewardsPanel.SetActive(false);
    }
    private void ButtonEvent_CloseSelectOpponent(GameObject go)
    {
        Scheduler.Instance.RemoveUpdator(UpdateRematchTip);
        view.Gobj_SelectOpponent.SetActive(false);
    }

    private void ButtonEvnet_Purchase(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        int buyTime = QualifyingModule.Instance.poleLobbyData.buy_times;
        if (buyTime <= 0)
            buyTime = 0;
        VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        if (vipData != null)
        {
            if (buyTime >= vipData.QualifyingLimit)
            {
                if (PlayerData.Instance._VipLv < ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Qualifying))
                {
                    string tip = ConstString.PVP_PURCHASELIMITTIP;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
            }
            else
            {
                TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)buyTime + 1);
                if (data == null || data.QualifyingPurchaseConsume == null)// data为NULL 则可默认为已经达到购买上限
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
                else if (data.QualifyingPurchaseConsume.Type == ECurrencyType.None && data.QualifyingPurchaseConsume.Number == 0)  // 消耗金币类型为0则说明无购买次数
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASECHANGLLECOUNT, data.QualifyingPurchaseConsume.Number, awardConfigData.challenge_time_limit, buyTime, vipData.QualifyingLimit), () =>
                        {
                            if (CommonFunction.CheckMoneyEnough(data.QualifyingPurchaseConsume.Type, data.QualifyingPurchaseConsume.Number, true))
                            {
                                QualifyingModule.Instance.SendPoleBuyTimes();
                            }
                        }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                }
            }
        }
    }

    #endregion

    private int GetPlayerCombatPower(List<SoldierList> soldierList, List<EquipList> equipList)
    {
        Dictionary<Soldier, int> soldier_dic = new Dictionary<Soldier, int>();
        List<CalBaseData> equips = new List<CalBaseData>();
        List<CalBaseData> skills = new List<CalBaseData>();
        for (int i = 0; i < equipList.Count; i++)
        {
            EquipList tmpEquip = equipList[i];
            if (tmpEquip == null)
                continue;
            Weapon equip = PlayerData.Instance._WeaponDepot.FindByUid(tmpEquip.uid);
            if (equip != null)
            {
                CalBaseData equipData = new CalBaseData(equip.Att.id, equip.Level);
                if (equip._Skill != null)
                {
                    CalBaseData skillData = new CalBaseData(equip._Skill.Att.nId, equip._Skill.Level);
                    skills.Add(skillData);
                }
                equips.Add(equipData);
            }
        }
        if (PlayerData.Instance._SkillsDepot != null && PlayerData.Instance._SkillsDepot._skillsList != null)
        {
            for (int i = 0; i < PlayerData.Instance._SkillsDepot._skillsList.Count; i++)
            {
                Skill skill = PlayerData.Instance._SkillsDepot._skillsList[i];
                if (skill == null)
                    continue;
                CalBaseData skillData = new CalBaseData(skill.Att.nId, skill.Lv);
                skills.Add(skillData);
            }
        }
        for (int i = 0; i < soldierList.Count; i++)
        {
            SoldierList tmpSoldier = soldierList[i];
            if (tmpSoldier == null)
                continue;
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(tmpSoldier.uid);
            if (soldier == null)
                continue;
            soldier_dic.Add(soldier, tmpSoldier.num);
        }
        if (currentPetData != null)
        {
            CalBaseData skillData = new CalBaseData(currentPetData.Skill.Att.nId, currentPetData.Skill.Lv);
            skills.Add(skillData);
        }
        int PlayerCombatPower = Calculation_Attribute.Instance.Calculation_PlayerCombatPower(PlayerData.Instance._Level, soldier_dic, equips, skills);
        return PlayerCombatPower;
    }

    public void OnReceiveAwardSuccess(int key, DropList droplist)
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(droplist);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETSPECIALITEM);
        UISystem.Instance.GetSpecialItemView.SetInfo(list);
        switch ((GetAwardTypeEnum)key)
        {
            case GetAwardTypeEnum.LowDay:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.day_low = (int)AwardStateEnum.HasGet;
                } break;
            case GetAwardTypeEnum.MidDay:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.day_mid = (int)AwardStateEnum.HasGet;
                } break;
            case GetAwardTypeEnum.HighDay:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.day_high = (int)AwardStateEnum.HasGet;
                } break;
            case GetAwardTypeEnum.LowWeek:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.week_low = (int)AwardStateEnum.HasGet;
                } break;
            case GetAwardTypeEnum.MidWeek:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.week_mid = (int)AwardStateEnum.HasGet;
                } break;
            case GetAwardTypeEnum.HighWeek:
                {
                    QualifyingModule.Instance.poleLobbyData.pole_recinf.week_high = (int)AwardStateEnum.HasGet;
                } break;
        }
        OpenAwardPanel(isDayAward);
        UpdateAwardsEffect();
    }


    public void OnPoleRevengeSuccess()
    {
        PrepareCommonData lCommonData = new PrepareCommonData();
        lCommonData.AccountID = revengeEnemy.hero.accid;
        lCommonData.FightType = EFightType.eftQualifying;
        lCommonData.IsAdjust = false;
        lCommonData.ArenaPlayer = revengeEnemy;
        lCommonData.Other = 2;
        lCommonData.UID = QualifyingModule.Instance.LogUID;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
        UISystem.Instance.PrepareBattleView.UpdateViewInfo(lCommonData);
    }

    public void OnReMatchOppentSuccess(int type, uint next_tick, List<ArenaPlayer> players, List<AccidScore> scores)
    {
        List<QualifyingInfo> infos = new List<QualifyingInfo>();
        for (int i = 0; i < players.Count; i++)
        {
            ArenaPlayer player = players[i];
            if (player == null)
                continue;
            AccidScore score = scores.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.accid == player.hero.accid;
            });
            if (score == null)
                continue;
            QualifyingInfo info = new QualifyingInfo();
            info.arenaPlayer = player;
            info.score = score.score;
            infos.Add(info);
        }
        if (type == 1)
        {
            UpdateSelectOpponentPanel(false, next_tick, infos);
        }
        else if (type == 2)
        {
            UpdateSelectOpponentPanel(true, next_tick, infos);
        }
    }

    public override void Uninitialize()
    {
        currentPetData = null;
        PlayerData.Instance.NotifyResetEvent -= OnNotifyReset;
        Main.Instance.StopCoroutine(UpdateBattleLogPanelInfo(null));
        Main.Instance.StopCoroutine(UpdateMatchedOpponents(null));
        Scheduler.Instance.RemoveUpdator(UpdateSeasonStartCD);
        Scheduler.Instance.RemoveUpdator(UpdateSeasonEndCD);
        Scheduler.Instance.RemoveUpdator(UpdateRematchTip);
        Scheduler.Instance.RemoveUpdator(DivisionEffectEnd);
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (defenseSoldier_dic != null)
            defenseSoldier_dic.Clear();
        if (battleLog_dic != null)
            battleLog_dic.Clear();
        if (divisionAward_dic != null)
            divisionAward_dic.Clear();
        if (opponent_dic != null)
            opponent_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_RuleMask.gameObject).onClick = ButtonEvent_CloseRule;
        UIEventListener.Get(view.Btn_RuleDesc.gameObject).onClick = ButtonEvent_RuleDesc;
        UIEventListener.Get(view.Btn_Rank.gameObject).onClick = ButtonEvent_Rank;
        UIEventListener.Get(view.Btn_BattleLog.gameObject).onClick = ButtonEvent_BattleLog;
        UIEventListener.Get(view.Btn_StartMacth.gameObject).onClick = ButtonEvent_StartMacth;
        UIEventListener.Get(view.Btn_AdjustLineup.gameObject).onClick = ButtonEvent_AdjustLineup;
        UIEventListener.Get(view.Btn_Exit.gameObject).onClick = ButtonEvent_Exit;
        UIEventListener.Get(view.Spt_BattleLogMask.gameObject).onClick = ButtonEvent_CloseBattleLog;
        UIEventListener.Get(view.Btn_AwardSureButton.gameObject).onClick = ButtonEvent_AwardSureButton;
        UIEventListener.Get(view.Gobj_EveryDayAwardGroup).onClick = ButtonEvent_EveryDayAward;
        UIEventListener.Get(view.Gobj_EveryWeekAwardGroup).onClick = ButtonEvent_EveryWeekAward;
        UIEventListener.Get(view.Gobj_EverySeasonAwardGroup).onClick = ButtonEvent_EverySeasonAward;
        UIEventListener.Get(view.Gobj_LowestAwardGroup).onClick = ButtonEvent_LowestAward;
        UIEventListener.Get(view.Gobj_MiddleAwardGroup).onClick = ButtonEvent_MiddleAward;
        UIEventListener.Get(view.Gobj_HighestAwardGroup).onClick = ButtonEvent_HighestAward;
        UIEventListener.Get(view.Spt_SeasonRewardsMask.gameObject).onClick = ButtonEvent_ColseSeasonRewards;
        UIEventListener.Get(view.Btn_Purchase.gameObject).onClick = ButtonEvnet_Purchase;
        UIEventListener.Get(view.Spt_SelectOpponentMask.gameObject).onClick = ButtonEvent_CloseSelectOpponent;
        UIEventListener.Get(view.Btn_ReMatach.gameObject).onClick = ButtonEvent_Remacth;
    }
}
