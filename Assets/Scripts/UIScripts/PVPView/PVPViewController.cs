using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
public class PVPViewController : UIBase
{
    public PVPView view;
    //private List<GameObject> ItemList = new List<GameObject>();

    private class ReadyEquipData
    {
        public Weapon weapon;
        public int index;
        public ReadyEquipData(Weapon data, int slot)
        {
            weapon = data;
            index = slot;
        }
    }

    /// <summary>
    /// 敌方玩家
    /// </summary>
    private List<PVPEnemyComponent> enemy_dic;
    /// <summary>
    /// 己方防御阵容
    /// </summary>
    private List<PVPSoldierComponent> defenseSoldier_Dic;
    /// <summary>
    /// 当前拥有的可出战士兵
    /// </summary>
    private List<OwnSoldierLineupComponent> ownSoldier_dic;
    /// <summary>
    /// 已经准备出战队的士兵
    /// </summary>
    private List<LineupItemComponent> readySoldier_dic;
    /// <summary>
    /// 当前拥有的神器
    /// </summary>
    private List<PVPOwnEquipTypeComponent> ownEquip_dic;
    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    private List<PVPEquipComponent> readyEquip_dic;
    /// <summary>
    /// 战斗日志
    /// </summary>
    private List<PVPBattleLogComponent> battleLog_dic;
    private ReadyEquipData[] readyEquipArray = new ReadyEquipData[6];
    /// <summary>
    /// 准备穿戴的装备
    /// </summary>
    //private Weapon[] readyEquipArray = new Weapon[6];

    private PVPRankAwardDescGroupComponent ruleAwradDescComp;
    private PVPGetAwradsGroupComponet ruleGetAwardComp;

    /// <summary>
    /// 准备出战的士兵
    /// </summary>
    private List<ReadySoldier> readySoldierList = new List<ReadySoldier>();
    /// <summary>
    /// 当前拥有的装备数据
    /// </summary>
    private List<Weapon> ownEquipList = new List<Weapon>();
    /// <summary>
    /// 当前拥有的士兵数据
    /// </summary>
    private List<Soldier> ownSoldierList = new List<Soldier>();
    /// <summary>
    /// 是否是攻击阵容 true:是 false:调整阵容
    /// </summary>
    private bool isAttack = false;
    /// <summary>
    /// 演舞台配置表数据
    /// </summary>
    private ArenaInfo arenaInfo;
    private GameObject Go_PVPRefreshEffect;
    public class ReadySoldier
    {
        public Soldier soldier;
        public int count;
    }
    /// <summary>
    /// 当前能量
    /// </summary>
    private int currentEnergy;
    private PetData currentPetData;
    private bool isRefresh = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new PVPView();
            view.Initialize();
            BtnEventBinding();
        }
        PVPModule.Instance.isFightState = false;
        InitView();
        if (enemy_dic == null)
            enemy_dic = new List<PVPEnemyComponent>();
        if (defenseSoldier_Dic == null)
            defenseSoldier_Dic = new List<PVPSoldierComponent>();
        if (ownSoldier_dic == null)
            ownSoldier_dic = new List<OwnSoldierLineupComponent>();
        if (readySoldier_dic == null)
            readySoldier_dic = new List<LineupItemComponent>();
        if (ownEquip_dic == null)
            ownEquip_dic = new List<PVPOwnEquipTypeComponent>();
        if (readyEquip_dic == null)
            readyEquip_dic = new List<PVPEquipComponent>();
        if (battleLog_dic == null)
            battleLog_dic = new List<PVPBattleLogComponent>();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenPVPView);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        PlayerData.Instance.NotifyResetEvent += ResetChallengeCount;
        PetSystemModule.Instance.PetChooseEvent += OnUpdatePetInfo;
        PVPModule.Instance.SendEnterArenaLobby();
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_PVPVIEW);
    }
    private void InitView()
    {
        view.Gobj_Main.SetActive(true);
        view.Gobj_LineupSelect.SetActive(false);
        view.Gobj_EquipSelect.SetActive(false);
        view.Gobj_Rule.SetActive(false);
        view.Gobj_BattleLog.SetActive(false);
        view.Gobj_Countdown.SetActive(false);
        view.Gobj_DefenseSoldierComp.SetActive(false);
        view.Gobj_BattleLogComp.SetActive(false);
        view.Gobj_DefenseSoldierComp.SetActive(false);
        view.Gobj_EnemyComp.SetActive(false);
        view.Gobj_OwnEquipComp.SetActive(false);
        view.Gobj_ReadyEquipComp.SetActive(false);
        view.Gobj_OwnSoldierComp.SetActive(false);
        view.Gobj_ReadySoldierComp.SetActive(false);
        view.Btn_ClearTick.gameObject.SetActive(false);
        view.Btn_Purchase.gameObject.SetActive(false);
        view.Btn_RefreshPlayer.gameObject.SetActive(false);
        view.Gobj_GoldGroup.SetActive(false);
        UpdateLogMark();
        isRefresh = false;
        readySoldierList = new List<ReadySoldier>();
        arenaInfo = ConfigManager.Instance.mArenaConfig.GetArenaInfo();
        view.UIWrapContent_OwnSoldier.onInitializeItem = UpdateOwnSoldierInfo;
        view.UIWrapContent_OwnEquip.onInitializeItem = UpdateOwnEquipInfo;

    }
    #region Update Event

    private void ResetChallengeCount(NotifyReset data)
    {
        PVPModule.Instance.ChallengeCount = data.arena_times;
        UpdateChallengeCount();
        UpdateFunctionStatus();
        UpdateLogChallengeCount();
        UpdateLogListInfo();
    }

    /// <summary>
    /// 保存防御阵容完成
    /// </summary>
    public void SaveDefenceArraySuccess()
    {
        PVPModule.Instance.PvpData.defence_equips.Clear();
        PVPModule.Instance.PvpData.defence_equips.AddRange(GetReadyEquipArray());
        PVPModule.Instance.PvpData.defence_soldiers.Clear();
        PVPModule.Instance.PvpData.defence_soldiers.AddRange(GetReadySoldierList());
        if (currentPetData == null)
        {
            PVPModule.Instance.PvpData.pettypeid = 0;
        }
        else
        {
            PVPModule.Instance.PvpData.pettypeid = (int)currentPetData.PetInfo.PetID;
        }
        //TODO: 此处需要优化  add by taiwei
        PlayerData.Instance._ArenaInfo.defence_equips.Clear();
        PlayerData.Instance._ArenaInfo.defence_equips.AddRange(PVPModule.Instance.PvpData.defence_equips);
        PlayerData.Instance._ArenaInfo.defence_soldiers.Clear();
        PlayerData.Instance._ArenaInfo.defence_soldiers.AddRange(PVPModule.Instance.PvpData.defence_soldiers);
        CloseEquipPanel();
        CloseLineupPanel();
        UpdateViewInfo();
    }

    /// <summary>
    /// 更新PVP视图
    /// </summary>
    public void UpdateViewInfo()
    {
        if (PVPModule.Instance.PvpData == null || arenaInfo == null) return;
        UpdateLogMark();
        UpdateEnemyList();
        UpdateDefenseSoldiers();
        PVPModule.Instance.ChallengeCount = PVPModule.Instance.PvpData.buy_times * arenaInfo.every_buy_num + arenaInfo.free_times - PVPModule.Instance.PvpData.today_times;
        UpdateChallengeCount();
        view.Gobj_Main.SetActive(true);
        view.Lbl_DefenseCombatPower.text = PVPModule.Instance.PvpData.defence_combat_power.ToString();
        if (PVPModule.Instance.PvpData.rank < 10000)
        {
            //view.Lbl_RankDesc.enabled = true;
            //view.Lbl_RankNum.enabled = true;
            view.Lbl_RankNum.text = PVPModule.Instance.PvpData.rank.ToString();
        }
        else
        {
            //view.Lbl_RankDesc.enabled = false;
            view.Lbl_RankNum.text = ConstString.RANK_LABEL_NOUNION;
        }

        PVPModule.Instance.CDTime = PVPModule.Instance.PvpData.next_tick - Main.mTime;
        if (PVPModule.Instance.CDTime <= 0)
        {
            view.Lbl_ChallengeCountDown.text = string.Empty;
        }
        else
        {
            Scheduler.Instance.AddUpdator(UpdateChallengeCountDown);
        }
        UpdateFunctionStatus();
    }
    /// <summary>
    /// 更新挑战次数
    /// </summary>
    private void UpdateChallengeCount()
    {
        if (PVPModule.Instance.ChallengeCount <= 0)   //剩余挑战次数
        {
            view.Lbl_SurplusCount.text = string.Format(ConstString.PVP_TODAYSURPLUSCOUNT, 0, arenaInfo.max_times);
        }
        else
        {
            view.Lbl_SurplusCount.text = string.Format(ConstString.PVP_TODAYSURPLUSCOUNT, PVPModule.Instance.ChallengeCount, arenaInfo.max_times);
        }
    }

    /// 更新相同类型的士兵状态
    /// </summary>
    public void UpdateSameSoldierStatus()
    {
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            OwnSoldierLineupComponent comp = ownSoldier_dic[i];
            comp.IsEnable = !CheckSameSoldier(comp.soldierAtt);
            if (comp.IsEnable)
                comp.IsMask = !CheckSoldierCountLimit(comp.soldierAtt);
        }
    }

    #region 规则说明

    private void OpenRuleDescPanel()
    {
        //Main.Instance.StartCoroutine(PlayOpenRuleAnim());
        view.Gobj_Rule.SetActive(true);
        if (ruleAwradDescComp == null)
        {
            ruleAwradDescComp = new PVPRankAwardDescGroupComponent();
            ruleAwradDescComp.MyStart(view.Gobj_AwardGroup);
        }
        if (ruleGetAwardComp == null)
        {
            ruleGetAwardComp = new PVPGetAwradsGroupComponet();
            ruleGetAwardComp.MyStart(view.Gobj_GetAwardsGroup.gameObject);
        }

        Main.Instance.StartCoroutine(UpdateRuleDescPanel());
    }

    /// <summary>
    /// 更新规则说明面板信息
    /// </summary>
    public IEnumerator UpdateRuleDescPanel()
    {
        view.ScrView_RuleDesc.ResetPosition();
        yield return null;
        view.Lbl_RuleRankDesc.text = string.Empty;
        view.Lbl_RuleDesc.text = string.Empty;
        CommonFunction.AddNewLine(view.Lbl_RuleRankDesc, string.Format(ConstString.PVP_SELFRANK, PVPModule.Instance.PvpData.rank));
        CommonFunction.AddNewLine(view.Lbl_RuleRankDesc, string.Format(ConstString.PVP_HISTORY_RANK, PVPModule.Instance.PvpData.best_rank));
        ArenaAwardInfo info = ConfigManager.Instance.mArenaAwardConfig.GetArenaAwardInfoByRank(PVPModule.Instance.PvpData.rank);
        if (info != null)
        {
            ruleGetAwardComp.mRootObject.SetActive(true);
            view.Spt_SelfRankSprite.height = 285;
            //CommonFunction.AddNewLine(view.Lbl_RuleRankDesc, ConstString.PVP_GETAWARDS);
            ruleGetAwardComp.UpdateInfo(info);
        }
        else
        {
            view.Spt_SelfRankSprite.height = 145;
            ruleGetAwardComp.mRootObject.SetActive(false);
        }
        RuleDescData ruledesc = ConfigManager.Instance.mRuleDescConfig.GetRuleDescData();
        string pvpDesc = string.Empty;
        if (ruledesc != null)
        {
            pvpDesc = ruledesc.pvp_rule;
        }
        pvpDesc = CommonFunction.ReplaceEscapeChar(pvpDesc);
        CommonFunction.AddNewLine(view.Lbl_RuleDesc, pvpDesc);
        yield return null;
        ruleAwradDescComp.UpdateInfo();
        yield return null;
        view.Table_Rule.Reposition();
    }



    private void CloseRuleDescPanel()
    {
        view.Gobj_Rule.SetActive(false);
    }

    #endregion

    #region 战斗日志
    /// <summary>
    /// 打开战斗日志面板
    /// </summary>
    public void OpenBattleLogPanel()
    {
        view.Gobj_BattleLog.SetActive(true);
        PVPModule.Instance.IsBattleLog = false;
        UpdateLogMark();
        //  Main.Instance.StartCoroutine(PlayOpenBattleLogAnim());
        PVPModule.Instance.SendArenaRecord();
        UpdateLogChallengeCount();
        if (PVPModule.Instance.CDTime > 0)
        {
            Scheduler.Instance.AddUpdator(UpdateLogCD);
        }
        else
        {
            view.Lbl_LogCountDown.text = string.Empty;
        }
    }


    private void UpdateLogChallengeCount()
    {
        if (PVPModule.Instance.ChallengeCount <= 0)   //剩余挑战次数
        {
            view.Lbl_LogSurplus.text = string.Format(ConstString.PVP_TODAYSURPLUSCOUNTDESC, 0, arenaInfo.max_times);
        }
        else
        {
            view.Lbl_LogSurplus.text = string.Format(ConstString.PVP_TODAYSURPLUSCOUNTDESC, PVPModule.Instance.ChallengeCount, arenaInfo.max_times);
        }
    }

    private void UpdateLogCD()
    {
        view.Lbl_LogCountDown.text = string.Format(ConstString.PVP_CHALLENGE_CDING, CommonFunction.GetTimeString(Mathf.CeilToInt(PVPModule.Instance.CDTime)));
        if (PVPModule.Instance.CDTime <= 0)
        {
            for (int i = 0; i < battleLog_dic.Count; i++)
            {
                PVPBattleLogComponent comp = battleLog_dic[i];
                if (comp.mRootObject.activeSelf)
                {
                    comp.UpdateRevengeButton();
                }
            }
            view.Lbl_LogCountDown.text = string.Empty;
            Scheduler.Instance.RemoveUpdator(UpdateLogCD);
        }
    }

    public void ShowNotify(NotifyRefresh resp)
    {
        UpdateLogMark();
    }

    private void UpdateLogMark()
    {
        view.Spt_BattleLogMark.enabled = PVPModule.Instance.IsBattleLog;
    }

    public void UpdateBattleLogPanel()
    {
        Main.Instance.StartCoroutine(UpdateBattleLogPanelInfo());
    }

    /// <summary>
    /// 更新战斗日志面板信息
    /// </summary>
    private IEnumerator UpdateBattleLogPanelInfo()
    {
        view.ScrView_BattleLog.ResetPosition();
        yield return null;
        List<ArenaRecord> recordList = new List<ArenaRecord>(PVPModule.Instance.recordList);
        recordList.Sort((ArenaRecord left, ArenaRecord right) =>
        {
            if (left == null || right == null) return 0;
            return right.time.CompareTo(left.time);
            //if (left.time < right.time) 
            //{
            //    return 1;
            //}
            //else if (left.time == right.time) 
            //{
            //    return 0;
            //}else
        });

        if (recordList.Count <= battleLog_dic.Count)
        {
            for (int i = 0; i < battleLog_dic.Count; i++)
            {
                PVPBattleLogComponent comp = battleLog_dic[i];
                if (i < recordList.Count)
                {
                    comp.UpdateInfo(recordList[i]);
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
                PVPBattleLogComponent comp = null;
                if (i < ob_count)
                {
                    comp = battleLog_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_BattleLogComp.gameObject, view.Grd_BattleLog.transform);
                    comp = new PVPBattleLogComponent(go);
                    comp.revengedHandle = ButtonEvent_Revenge;
                    comp.playerInfoHandle = ButtonEvent_PlayerInfo;
                    battleLog_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(recordList[i]);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_BattleLog.repositionNow = true;
        yield return null;
        view.ScrView_BattleLog.ResetPosition();
    }

    private void UpdateLogListInfo()
    {
        for (int i = 0; i < battleLog_dic.Count; i++)
        {
            PVPBattleLogComponent comp = battleLog_dic[i];
            comp.UpdateInfo();
        }
    }

    /// <summary>
    /// 关闭战斗日志面板
    /// </summary>
    public void CloseBattleLogPanel()
    {
        view.Gobj_BattleLog.SetActive(false);
    }

    #endregion

    #region 战斗士兵  装备

    #region 阵容
    /// <summary>
    /// 检测同名武将  true:存在同名武将
    /// </summary>
    /// <param name="soldierID"></param>
    /// <returns></returns>
    private bool CheckSameSoldier(Soldier ownSoldier)
    {
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            Soldier soldier = readySoldierList[i].soldier;
            if (soldier.uId == ownSoldier.uId)   //PVP可以多个数量武将 add by taiwei
            {
                return false;
            }
            else
            {
                if (soldier.Att.type.Equals(ownSoldier.Att.type))
                {
                    return true;
                }
            }
        }
        return false;
    }

    private bool CheckSoldierCountLimit(Soldier ownSoldier)
    {
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            ReadySoldier soldier = readySoldierList[i];
            if (soldier.soldier.uId.Equals(ownSoldier.uId))
            {
                if (soldier.count <= GlobalCoefficient.PVPSoldierCountLimit)
                {
                    return true;
                }
                else return false;
            }
        }
        return true;
    }
    /// <summary>
    /// 检测是否超过能量上限  true:超过能量上限
    /// </summary>
    /// <param name="ownSoldier"></param>
    /// <returns></returns>
    private bool CheckEnergyLimit(Soldier ownSoldier)
    {
        int energy = currentEnergy + ownSoldier.Att.call_energy;
        if (energy > arenaInfo.max_energy)
        {
            return true;
        }
        return false;
    }
    /// <summary>
    /// PVP士兵数量限制 true:未越界
    /// </summary>
    private bool CheckSoldierCountLimit(int count)
    {
        return count < GlobalCoefficient.PVPSoldierCountLimit;
    }

    private bool CheckEnergyLimit()
    {
        return false;  //LTD_20150824版本优化 
        int energy = currentEnergy;
        for (int i = 0; i < ownSoldierList.Count; i++)
        {
            Soldier _soldier = ownSoldierList[i];
            if (energy + _soldier.Att.call_energy <= arenaInfo.max_energy)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 获得当前已出战士兵的能量总和
    /// </summary>
    /// <returns></returns>
    private int GetCurrentEnergy()
    {
        int energy = 0;
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            ReadySoldier readySoldier = readySoldierList[i];
            energy += readySoldier.soldier.Att.call_energy * readySoldier.count;
        }
        return energy;
    }
    /// <summary>
    /// 更新准备出战的士兵数据 默认删除已出战士兵
    /// 返回false 则说明已达出战士兵上线
    /// </summary>
    private bool UpdateReadySoldierData(Soldier data, bool isAdd = false)
    {
        if (isAdd)
        {
            ReadySoldier ready = readySoldierList.Find((tmp) => { return tmp.soldier.uId == data.uId; });
            if (ready == null)  //如果为NULL 则说明已准备出战的士兵中无该武将 
            {
                if (readySoldierList.Count >= GlobalCoefficient.LineupSoldierLimit)  //判定是否达到出战士兵类型上限
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LINEUPPANEL_SOLDIERLIMIT);
                    return false;
                }
                else
                {
                    ReadySoldier info = new ReadySoldier();
                    info.soldier = data;
                    info.count = 1;
                    currentEnergy += info.soldier.Att.call_energy;
                    readySoldierList.Add(info);
                }
            }
            else
            {
                if (!CheckSoldierCountLimit(ready.count))
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.PVPSOLDIERCOUNTLIMIT, GlobalCoefficient.PVPSoldierCountLimit));
                    return false;
                }
                ready.count++;
                currentEnergy += ready.soldier.Att.call_energy;

            }
        }
        else
        {
            ReadySoldier ready = readySoldierList.Find((tmp) => { return tmp.soldier.uId == data.uId; });
            if (ready == null)  //如果为null  则说明无该武将 
            {
                return false;
            }
            else
            {
                ready.count--;
                currentEnergy -= ready.soldier.Att.call_energy;
                if (ready.count <= 0)   //如果该武将已经为0  则删除该数据
                {
                    readySoldierList.Remove(ready);
                }
            }
        }
        return true;
    }

    private void DelOwnSoldierData(Soldier soldier)
    {
        ReadySoldier ready = readySoldierList.Find((tmp) => { return tmp.soldier.uId == soldier.uId; });
        if (ready != null && ready.count > 0) return;//如果该武将数量大于0 则不处理
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            OwnSoldierLineupComponent comp = ownSoldier_dic[i];
            if (comp.soldierAtt.Equals(soldier))
            {
                comp.IsSelect = false;
                break;
            }

        }
    }

    public void OpenLineupPanel()
    {
        currentEnergy = 0;
        SetDefaultReadyEquip();
        SetDefaultReadySoldier();
        view.Gobj_Countdown.SetActive(isAttack);
        UpdateLineupPanel();
        if (isAttack)
        {
            battleReadyTime = (float)GlobalCoefficient.PVPReadyTime;
            Scheduler.Instance.AddUpdator(UpdateBattleReadyTime);
        }
        view.Gobj_LineupSelect.SetActive(true);
        //    Main.Instance.StartCoroutine(PlayOpenLineupSelectAnim());
        view.Gobj_Main.SetActive(false);
    }

    private void SetDefaultReadySoldier()
    {
        readySoldierList.Clear();
        if (isAttack)
        {
            if (PlayerPrefsTool.HasKey(AppPrefEnum.PVPAttackSoldier))
            {
                List<CommonSoldierData> list = PlayerPrefsTool.ReadObject<List<CommonSoldierData>>(AppPrefEnum.PVPAttackSoldier);
                if (list != null)
                {
                    for (int i = 0; i < list.Count; i++)
                    {
                        CommonSoldierData tmpData = list[i];
                        ReadySoldier readySoldier = new ReadySoldier();
                        Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(tmpData.uID);
                        if (soldier == null) continue;
                        readySoldier.soldier = soldier;
                        readySoldier.count = tmpData.index;
                        readySoldierList.Add(readySoldier);
                    }
                }

            }
        }
        else
        {
            for (int i = 0; i < PVPModule.Instance.PvpData.defence_soldiers.Count; i++)
            {
                SoldierList tSoldierList = PVPModule.Instance.PvpData.defence_soldiers[i];
                ReadySoldier readySoldier = new ReadySoldier();
                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(tSoldierList.uid);
                if (soldier == null) continue;
                readySoldier.soldier = soldier;
                readySoldier.count = tSoldierList.num;
                readySoldierList.Add(readySoldier);
            }
        }

    }
    private void SetDefaultReadyEquip()
    {
        InitReadyEquipData();
        if (isAttack)
        {
            if (PlayerPrefsTool.HasKey(AppPrefEnum.PVPAttackEquip))
            {
                List<CommonEquipData> list = PlayerPrefsTool.ReadObject<List<CommonEquipData>>(AppPrefEnum.PVPAttackEquip);
                if (list != null)
                {
                    for (int i = 0; i < readyEquipArray.Length; i++)
                    {
                        ReadyEquipData readyData = readyEquipArray[i];
                        for (int j = 0; j < list.Count; j++)
                        {
                            CommonEquipData data = list[j];
                            if (data.index == readyData.index)
                            {
                                readyData.weapon = PlayerData.Instance._WeaponDepot.FindByUid(data.uID);
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (int j = 0; j < readyEquipArray.Length; j++)
            {
                ReadyEquipData readyData = readyEquipArray[j];
                for (int i = 0; i < PVPModule.Instance.PvpData.defence_equips.Count; i++)
                {
                    EquipList tEquipList = PVPModule.Instance.PvpData.defence_equips[i];
                    if (readyData.index == tEquipList.pos)
                    {
                        Weapon weapon = PlayerData.Instance._WeaponDepot.FindByUid(tEquipList.uid);
                        readyData.weapon = weapon;
                    }
                    //Weapon weapon = PlayerData.Instance._WeaponDepot.FindByUid(tEquipList.uid);
                    //readyData.weapon = weapon;
                    //readyData.index = tEquipList.pos;
                }
            }

        }
    }

    private void InitReadyEquipData()
    {
        readyEquipArray = new ReadyEquipData[6];
        readyEquipArray[0] = new ReadyEquipData(null, 1);
        readyEquipArray[1] = new ReadyEquipData(null, 2);
        readyEquipArray[2] = new ReadyEquipData(null, 3);
        readyEquipArray[3] = new ReadyEquipData(null, 5);
        readyEquipArray[4] = new ReadyEquipData(null, 6);
        readyEquipArray[5] = new ReadyEquipData(null, 7);
    }

    private void CloseLineupPanel()
    {
        view.Gobj_LineupSelect.SetActive(false);
        view.Gobj_Main.SetActive(true);
        //Main.Instance.StartCoroutine(PlayOpenMainAnim());
        view.Gobj_Countdown.SetActive(false);
        Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
    }

    /// <summary>
    /// 阵容面板信息
    /// </summary>
    private void UpdateLineupPanel()
    {
        if (isAttack)
        {
            view.Lbl_LineUpTitle.text = ConstString.PVP_LINEUPATTACKTITLE;
        }
        else
        {
            view.Lbl_LineUpTitle.text = ConstString.PVP_LINEUPDEFENSETITLE;
        }
        //view.Spt_AttackTitle.enabled = isAttack;
        //view.Spt_DefenseTitle.enabled = !isAttack;
        Main.Instance.StartCoroutine(CreateOwnSoldier());
        UpdateReadySoldiers();
        currentEnergy = GetCurrentEnergy();
        UpdateEnergy();
    }

    /// <summary>
    /// 更新能量
    /// </summary>
    private void UpdateEnergy()
    {
        view.Lbl_Energy.text = string.Format(ConstString.PVP_BATTLE_ENERGY, currentEnergy.ToString(), arenaInfo.max_energy);
    }
    /// <summary>
    /// 拥有的士兵
    /// </summary>
    private IEnumerator CreateOwnSoldier()
    {
        view.ScrView_OwnSoldier.ResetPosition();
        int MAXCOUNT = 28;
        ownSoldierList = new List<Soldier>(PlayerData.Instance._SoldierDepot._soldierList);
        int count = ownSoldierList.Count;
        int itemCount = ownSoldier_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_OwnSoldier.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_OwnSoldier.minIndex = -index;
        view.UIWrapContent_OwnSoldier.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_OwnSoldier.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_OwnSoldier.enabled = false;
        }
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                ownSoldier_dic[i].mRootObject.SetActive(false);
            }
        }
        yield return null;
        for (int i = 0; i < count; i++)
        {
            OwnSoldierLineupComponent comp = null;
            if (i < itemCount)
            {
                comp = ownSoldier_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnSoldierComp, view.Grd_OwnSoldier.transform);
                vGo.name = i.ToString();
                comp = new OwnSoldierLineupComponent();
                comp.MyStart(vGo);
                comp.AddPressEventListener(PressEvent_OwnSoldier);
                comp.AddEventListener(ButtonEvent_OwnSoldier);
                ownSoldier_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(ownSoldierList[i]);
            comp.IsEnable = true;
            comp.IsSelect = IsSelectedOwnSoldier(ownSoldierList[i]);
            comp.IsShowEnergy = true;
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_OwnSoldier.ReGetChild();
        yield return null;
        view.Grd_OwnSoldier.repositionNow = true;
        yield return null;
        view.ScrView_OwnSoldier.ResetPosition();
        UpdateSoldierEnergyStatus();
        UpdateSameSoldierStatus();
    }

    /// <summary>
    /// 准备出战的士兵
    /// </summary>
    private void UpdateReadySoldiers()
    {
        UpdateEnergy();
        if (readySoldierList.Count <= readySoldier_dic.Count)
        {
            for (int i = 0; i < readySoldier_dic.Count; i++)
            {
                LineupItemComponent comp = readySoldier_dic[i];
                if (i < readySoldierList.Count)
                {
                    ReadySoldier _info = readySoldierList[i];
                    comp.UpdateInfo(_info.soldier);
                    comp.UpdateNum(_info.count, GlobalCoefficient.PVPSoldierCountLimit);
                    comp.IsSelect = false;
                    comp.IsShowEnergy = true;
                    comp.IsShowLeader = false;
                    comp.IsEnable = true;
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
            int ob_count = readySoldier_dic.Count;   //已经生成的物件数量
            for (int i = 0; i < readySoldierList.Count; i++)
            {
                LineupItemComponent comp = null;
                ReadySoldier _info = readySoldierList[i];
                if (i < ob_count)
                {
                    comp = readySoldier_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadySoldierComp.gameObject, view.Grd_RedaySoldierGrid.transform);
                    comp = new LineupItemComponent();
                    comp.MyStart(go);
                    comp.AddEventListener(ButtonEvent_ReadySoldier);
                    readySoldier_dic.Add(comp);
                }
                comp.UpdateInfo(_info.soldier);
                comp.UpdateNum(_info.count, GlobalCoefficient.PVPSoldierCountLimit);
                comp.IsSelect = false;
                comp.IsShowEnergy = true;
                comp.IsShowLeader = false;
                comp.IsEnable = true;
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_RedaySoldierGrid.repositionNow = true;
    }
    #endregion

    #region 装备

    /// <summary>
    /// 检测是否超过装备上限 true:已超限
    /// </summary>
    /// <param name="weapon"></param>
    /// <returns></returns>
    private bool CheckEquipLimit(Weapon weapon)
    {
        switch ((EquiptType)weapon.Att.type)
        {
            case EquiptType._Weapon:
                {
                    if (readyEquipArray[0].weapon == null || readyEquipArray[1].weapon == null || readyEquipArray[2].weapon == null)
                    {
                        return false;
                    }
                }
                break;
            case EquiptType._ring:
                {
                    if (readyEquipArray[3].weapon == null || readyEquipArray[4].weapon == null || readyEquipArray[5].weapon == null)
                    {
                        return false;
                    }
                }
                break;
            //case EquiptType._necklace:
            //    {
            //        if (readyEquipArray[3].weapon == null)
            //        {
            //            return false;
            //        }
            //    }
            //    break;
        }
        return true;
    }

    /// <summary>
    /// 更新当前准备携带的装备数据
    /// </summary>
    /// <param name="weapon"></param>
    private void UpdateReadyEquipData(Weapon weapon, bool isAdd)
    {
        if (isAdd)
        {
            switch ((EquiptType)weapon.Att.type)
            {
                case EquiptType._Weapon:
                    {
                        if (readyEquipArray[0].weapon == null)
                        {
                            readyEquipArray[0].weapon = weapon;
                        }
                        else if (readyEquipArray[1].weapon == null)
                        {
                            readyEquipArray[1].weapon = weapon;
                        }
                        else if (readyEquipArray[2].weapon == null)
                        {
                            readyEquipArray[2].weapon = weapon;
                        }
                    }
                    break;
                case EquiptType._ring:
                    {
                        if (readyEquipArray[3].weapon == null)
                        {
                            readyEquipArray[3].weapon = weapon;
                        }
                        else if (readyEquipArray[4].weapon == null)
                        {
                            readyEquipArray[4].weapon = weapon;
                        }
                        else if (readyEquipArray[5].weapon == null)
                        {
                            readyEquipArray[5].weapon = weapon;
                        }
                    }
                    break;
                //case EquiptType._necklace:
                //    {
                //        if (readyEquipArray[3].weapon == null)
                //        {
                //            readyEquipArray[3].weapon = weapon;
                //        }
                //    }
                //    break;
            }
        }
        else
        {
            for (int i = 0; i < readyEquipArray.Length; i++)
            {
                ReadyEquipData readyData = readyEquipArray[i];
                if (readyData != null && readyData.weapon != null)
                {
                    if (readyData.weapon.uId == weapon.uId)
                    {
                        readyData.weapon = null;
                        break;
                    }
                }

            }
        }
    }

    private void DelOwnEquipsData(Weapon weapon)
    {
        for (int i = 0; i < ownEquip_dic.Count; i++)
        {
            PVPOwnEquipTypeComponent comp = ownEquip_dic[i];
            if (comp.EquipInfo.uId.Equals(weapon.uId))
            {
                comp.IsSelect = false;
                break;
            }
        }
    }

    /// <summary>
    /// 获得当前战斗力
    /// </summary>
    private ShowInfoHero GetShowInfoHero()
    {
        List<CalBaseData> equips = new List<CalBaseData>();
        List<CalBaseData> skills = new List<CalBaseData>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            Weapon equip = readyEquipArray[i].weapon;
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
        ShowInfoHero tmpHeroInfo = Calculation_Attribute.Instance.Calculation_Attribute_Hero(PlayerData.Instance._Level, equips, skills);
        return tmpHeroInfo;
    }

    private int GetPlayerCombatPower()
    {
        Dictionary<Soldier, int> soldierList = new Dictionary<Soldier, int>();
        List<CalBaseData> equips = new List<CalBaseData>();
        List<CalBaseData> skills = new List<CalBaseData>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            Weapon equip = readyEquipArray[i].weapon;
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
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            ReadySoldier ready = readySoldierList[i];
            Soldier soldier = ready.soldier;
            soldierList.Add(soldier, ready.count);
        }
        if (currentPetData != null)
        {
            CalBaseData skillData = new CalBaseData(currentPetData.Skill.Att.nId, currentPetData.Skill.Lv);
            skills.Add(skillData);
        }
        int PlayerCombatPower = Calculation_Attribute.Instance.Calculation_PlayerCombatPower(PlayerData.Instance._Level, soldierList, equips, skills);
        //PlayerCombatPower = CombatPowerCalculation(soldierList, equips, skills);
        return PlayerCombatPower;
    }

    public void OpenEquipPanel()
    {
        if (!isAttack)
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenPVPSelectGodWeaponView);
        view.Gobj_LineupSelect.SetActive(false);
        view.Gobj_EquipSelect.SetActive(true);
        // Main.Instance.StartCoroutine(PlayOpenEquipSelectAnim());
        UpdateEquipPanel();
        //if (isAttack)
        //{
        //    Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);   //先移除士兵选择准备时间
        //    battleReadyTime = GlobalCoefficient.PVPReadyTime;
        //    Scheduler.Instance.AddUpdator(UpdateBattleReadyTime);
        //}
        Main.Instance.StartCoroutine(CreateReadyEquipComps());
        InitPetInfo();

    }

    private void InitPetInfo()
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Pet, false))
        {
            currentPetData = null;
            view.Gobj_EquipPet.SetActive(false);
            return;
        }
        List<PetData> petList = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (petList == null || petList.Count == 0)
        {
            view.Gobj_EquipPet.SetActive(false);
        }
        else
        {
            if (isAttack)
            {
                uint petTypeID = 0;
                if (PlayerPrefsTool.HasKey(AppPrefEnum.PVPAttackPet))
                {
                    petTypeID = (uint)PlayerPrefsTool.ReadInt(AppPrefEnum.PVPAttackPet);
                }
                if (petTypeID == 0)
                {
                    currentPetData = null;
                }
                else
                {
                    currentPetData = PlayerData.Instance._PetDepot.GetPetDataByTypeID(petTypeID);
                }
            }
            else
            {
                currentPetData = PlayerData.Instance._PetDepot.GetPetDataByTypeID((uint)PVPModule.Instance.PvpData.pettypeid);
            }
            UpdatePetInfo();
            view.Gobj_EquipPet.SetActive(true);
        }
    }

    private void OnUpdatePetInfo(uint petID)
    {
        if (petID == 0)
        {
            currentPetData = null;
        }
        else
        {
            currentPetData = PlayerData.Instance._PetDepot.GetPetDataByID(petID);
        }
        UpdatePetInfo();
        UpdateCombatPower();
    }

    private void UpdatePetInfo()
    {
        if (currentPetData == null)
        {
            view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, ConstString.HINT_NO);
        }
        else
        {
            view.Lbl_EquipedPetName.text = string.Format(ConstString.PET_PREPAREBATTLE_EQUIPEDPET, currentPetData.PetInfo.name);
        }
    }

    private void CloseEquipPanel()
    {
        view.Gobj_LineupSelect.SetActive(true);
        //  Main.Instance.StartCoroutine(PlayOpenLineupSelectAnim());
        view.Gobj_EquipSelect.SetActive(false);
    }

    private void UpdateEquipPanel()
    {
        view.Btn_Save.gameObject.SetActive(!isAttack);
        view.Btn_StartBattle.gameObject.SetActive(isAttack);
        UpdateOwnEquipsData();
        Main.Instance.StartCoroutine(CreateOwnEquips());
        UpdateCombatPower();
    }
    /// <summary>
    /// 更新战斗力
    /// </summary>
    private void UpdateCombatPower()
    {
        view.Lbl_EquipCombatPower.text = GetPlayerCombatPower().ToString();
    }

    private void UpdateOwnEquipsData()
    {
        if (ownEquipList == null)
            ownEquipList = new List<Weapon>();
        ownEquipList.Clear();
        List<Weapon> list = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList();
        if (list == null) return;
        for (int i = 0; i < list.Count; i++)
        {
            Weapon weapon = list[i];
            if (weapon.Att.type != (int)EquiptType._necklace && weapon.Att.type != (int)EquiptType._mount && weapon.Att.type != (int)EquiptType._clothing)
            {
                ownEquipList.Add(weapon);
            }
        }
    }

    /// <summary>
    /// 当前拥有的装备
    /// </summary>
    private IEnumerator CreateOwnEquips()
    {
        view.ScrView_OwnEquip.ResetPosition();
        int MAXCOUNT = 28;
        int count = ownEquipList.Count;
        int itemCount = ownEquip_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_OwnEquip.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_OwnEquip.minIndex = -index;
        view.UIWrapContent_OwnEquip.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_OwnEquip.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_OwnEquip.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                ownEquip_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            PVPOwnEquipTypeComponent comp = null;
            if (i < itemCount)
            {
                comp = ownEquip_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_OwnEquipComp, view.Grd_OwnEquip.transform);
                comp = new PVPOwnEquipTypeComponent();
                comp.MyStart(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_OwnEquip);
                ownEquip_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(ownEquipList[i]);
            comp.callBack = ButtonEvent_ArtifactDetail;
            comp.IsSelect = IsSelectedOwnEquip(ownEquipList[i]);
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_OwnEquip.ReGetChild();
        yield return null;
        view.Grd_OwnEquip.Reposition();
        yield return null;
        view.ScrView_OwnEquip.ResetPosition();
    }

    private void ButtonEvent_ArtifactDetail(BaseComponent baseComp, bool state)
    {
        PVPOwnEquipTypeComponent comp = baseComp as PVPOwnEquipTypeComponent;
        if (comp == null) return;
        if (state)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ARTIFACTDETAILINFO);
            UISystem.Instance.ArtifactDetailView.UpdateViewInfo(comp.mRootObject, comp.EquipInfo);
        }
        else
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ARTIFACTDETAILINFO);
        }
    }
    private IEnumerator CreateReadyEquipComps()
    {
        if (readyEquip_dic.Count < 6)
        {
            int index = readyEquip_dic.Count;
            for (int i = index; i < 6; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ReadyEquipComp.gameObject, view.Grd_RedayEquipGrid.transform);
                PVPEquipComponent comp = new PVPEquipComponent();
                comp.MyStart(go);
                comp.AddEventListener(ButtonEvent_ReadyEquip);
                go.name = i.ToString();
                go.SetActive(false);
                readyEquip_dic.Add(comp);
            }
            yield return null;
            view.Grd_RedayEquipGrid.repositionNow = true;
        }
        yield return null;
        UpdateReadyEquips();
    }

    /// <summary>
    /// 当前准备携带的装备
    /// </summary>
    public void UpdateReadyEquips()
    {
        UpdateCombatPower();
        for (int index = 0; index < readyEquip_dic.Count; index++)
        {
            PVPEquipComponent comp = readyEquip_dic[index];
            if (index < readyEquipArray.Length)
            {
                ReadyEquipData readyData = readyEquipArray[index];
                Weapon weapon = readyData.weapon;
                if (weapon == null)
                {
                    comp.mRootObject.SetActive(false);
                }
                else
                {
                    comp.UpdateInfo(weapon);
                    comp.mRootObject.SetActive(true);
                }
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
    }

    #endregion

    private float battleReadyTime;
    /// <summary>
    /// 更新战斗准备倒计时  true：打开 
    /// </summary>
    private void UpdateBattleReadyTime()
    {
        battleReadyTime -= Time.deltaTime;
        view.Lbl_ReadyCountDown.text = CommonFunction.GetTimeString(Mathf.CeilToInt(battleReadyTime));
        if (battleReadyTime <= 0)
        {
            Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_CHALLENGE_TIMEOUT,
                () =>
                {
                    CloseEquipPanel();
                    CloseLineupPanel();
                    UpdateViewInfo();
                });
        }
    }
    /// <summary>
    /// 更改按钮状态
    /// </summary>
    private void UpdateFunctionStatus()
    {
        if (PVPModule.Instance.CDTime > 0)
        {
            view.Btn_ClearTick.gameObject.SetActive(true);
            view.Btn_Purchase.gameObject.SetActive(false);
            view.Btn_RefreshPlayer.gameObject.SetActive(false);
            view.Gobj_GoldGroup.SetActive(true);
            view.Lbl_ConsumeGold.text = arenaInfo.clear_cd_diamond.ToString();
            CommonFunction.SetSpriteName(view.Spt_ConsumeGoldIcon, GlobalConst.SpriteName.Diamond);
            //view.Spt_ConsumeGoldIcon.MakePixelPerfect();
        }
        else
        {
            view.Btn_ClearTick.gameObject.SetActive(false);
            if (PVPModule.Instance.ChallengeCount <= 0)
            {
                view.Btn_Purchase.gameObject.SetActive(true);
                view.Gobj_GoldGroup.SetActive(true);
                view.Btn_RefreshPlayer.gameObject.SetActive(false);
                int buyTime = PVPModule.Instance.PvpData.buy_times;
                if (buyTime <= 0)
                    buyTime = 0;
                VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
                int count = 0;
                if (vipData != null)
                {
                    TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)buyTime + 1);
                    if (data != null)
                        count = data.AsynPvpTimes.Number;
                }
                view.Lbl_ConsumeGold.text = count.ToString();
                CommonFunction.SetSpriteName(view.Spt_ConsumeGoldIcon, GlobalConst.SpriteName.Diamond);
            }
            else
            {
                view.Btn_Purchase.gameObject.SetActive(false);
                view.Gobj_GoldGroup.SetActive(false);
                view.Btn_RefreshPlayer.gameObject.SetActive(true);
            }
        }
    }

    #endregion

    public void ClearCDTimeSuccess()
    {
        PVPModule.Instance.CDTime = 0f;
        PVPModule.Instance.PvpData.next_tick = (int)Main.mTime;
    }

    private fogs.proto.msg.Attribute playerAtt;
    /// <summary>
    /// 进入战斗
    /// </summary>
    public void EnterTheBattle(Attribute att)
    {
        playerAtt = att;
        Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
        UISystem.Instance.CloseAllUI();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        UISystem.Instance.HintView.ShowFightLoading(UISystem.Instance.FightView.ShowFightTitleInfo);
        PVPModule.Instance.isFightState = false;
        Scheduler.Instance.AddTimer(0.5f, false, DelayEnterBattle);
    }

    private void DelayEnterBattle()
    {
        List<CommonEquipData> readyAttackEquipList = new List<CommonEquipData>();
        List<CommonSoldierData> readyAttackSoldierList = new List<CommonSoldierData>();
        List<EquipList> ready_equips = GetReadyEquipArray();
        Dictionary<ulong, int> equip_list = new Dictionary<ulong, int>();
        for (int i = 0; i < ready_equips.Count; i++)
        {
            EquipList tmp = ready_equips[i];
            equip_list.Add(tmp.uid, tmp.pos);
        }
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            ReadyEquipData readyData = readyEquipArray[i];
            if (readyData == null) continue;
            Weapon tmp = readyData.weapon;
            if (tmp == null) continue;
            CommonEquipData readyAttackEquip = new CommonEquipData();
            readyAttackEquip.uID = tmp.uId;
            readyAttackEquip.index = readyData.index;
            readyAttackEquipList.Add(readyAttackEquip);
        }
        Dictionary<ulong, int> soldiers_dic = new Dictionary<ulong, int>();
        List<SoldierList> soldiers_list = GetReadySoldierList();
        for (int i = 0; i < soldiers_list.Count; i++)
        {
            SoldierList tmp = soldiers_list[i];
            soldiers_dic.Add(tmp.uid, tmp.num);
            CommonSoldierData readyAttackSoldier = new CommonSoldierData();
            readyAttackSoldier.uID = tmp.uid;
            readyAttackSoldier.index = tmp.num;
            readyAttackSoldierList.Add(readyAttackSoldier);
        }
        ShowInfoHero heroinfo = GetShowInfoHero(playerAtt);
        if (readyAttackEquipList != null && readyAttackEquipList.Count > 0)
            PlayerPrefsTool.WriteObject<List<CommonEquipData>>(AppPrefEnum.PVPAttackEquip, readyAttackEquipList);
        if (readyAttackSoldierList != null && readyAttackSoldierList.Count > 0)
        {
            PlayerPrefsTool.WriteObject<List<CommonSoldierData>>(AppPrefEnum.PVPAttackSoldier, readyAttackSoldierList);
            ReadyBattleSoldierManager.Instance.UpdatePvpAttackSoldier(readyAttackSoldierList);
        }
        uint petTypeID = 0;
        if (currentPetData != null)
            petTypeID = currentPetData.PetInfo.PetID;
        PlayerPrefsTool.WriteInt(AppPrefEnum.PVPAttackPet, (int)petTypeID);

        UISystem.Instance.FightView.SetViewInfo(EFightType.eftPVP, heroinfo, equip_list, soldiers_dic, PVPModule.Instance.enemyPlayer, currentPetData);
    }

    private ShowInfoHero GetShowInfoHero(Attribute att)
    {
        ShowInfoHero attribute = new ShowInfoHero();
        if (att == null)
            return attribute;
        attribute.KeyData = PlayerData.Instance._Level;
        attribute.Attack = att.phy_atk;
        attribute.Crit = att.crt_rate;
        attribute.Dodge = att.ddg_rate;
        attribute.Accuracy = att.acc_rate;
        attribute.AttDistance = att.atk_space;
        attribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(att.atk_interval);
        attribute.Energy = att.energy_max;
        attribute.EnergyRecovery = att.energy_revert;
        attribute.HP = att.hp_max;
        attribute.HPRecovery = att.hp_revert;
        attribute.Leadership = (ushort)att.leader;
        attribute.MoveSpeed = att.speed;
        attribute.MP = att.mp_max;
        attribute.MPRecovery = att.mp_revert;
        attribute.Tenacity = att.tnc_rate;
        attribute.CombatPower = att.combat_power;
        return attribute;
    }

    public void ExitReadyBattle()
    {
        Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
        CloseEquipPanel();
        CloseLineupPanel();
        UpdateViewInfo();
    }

    /// <summary>
    /// 更新挑战冷却时间
    /// </summary>
    public void UpdateChallengeCountDown()
    {
        PVPModule.Instance.CDTime -= Time.deltaTime;
        view.Lbl_ChallengeCountDown.text = string.Format(ConstString.PVP_CHALLENGECOUNTDOWN, CommonFunction.GetTimeString(Mathf.CeilToInt(PVPModule.Instance.CDTime)));
        if (PVPModule.Instance.CDTime <= 0)
        {
            UpdateFunctionStatus();
            view.Lbl_ChallengeCountDown.text = string.Empty;
            Scheduler.Instance.RemoveUpdator(UpdateChallengeCountDown);
        }
    }

    /// <summary>
    /// 更新主面板 防御阵容
    /// </summary>
    private void UpdateDefenseSoldiers()
    {
        List<ReadySoldier> list = new List<ReadySoldier>();
        for (int i = 0; i < PVPModule.Instance.PvpData.defence_soldiers.Count; i++)
        {
            SoldierList defenceSoldier = PVPModule.Instance.PvpData.defence_soldiers[i];
            ReadySoldier readySoldier = new ReadySoldier();
            Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(PVPModule.Instance.PvpData.defence_soldiers[i].uid);
            if (soldier == null) continue;  //有可能士兵已经被消耗
            readySoldier.soldier = soldier;
            readySoldier.count = defenceSoldier.num;
            list.Add(readySoldier);
        }
        if (list.Count <= defenseSoldier_Dic.Count)
        {
            for (int i = 0; i < defenseSoldier_Dic.Count; i++)
            {
                PVPSoldierComponent comp = defenseSoldier_Dic[i];
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
            int goCount = defenseSoldier_Dic.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ReadySoldier info = list[i];
                PVPSoldierComponent comp = null;
                if (i < goCount)
                {
                    comp = defenseSoldier_Dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_DefenseSoldierComp, view.Grd_DefenseSoldierGrid.transform);
                    go.name = "Soldier_" + i.ToString();
                    comp = new PVPSoldierComponent(go);
                    defenseSoldier_Dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(info.soldier, info.count);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_DefenseSoldierGrid.Reposition();
    }

    /// <summary>
    /// 更新敌方玩家List
    /// </summary>
    private void UpdateEnemyList()
    {
        List<ArenaPlayer> list = new List<ArenaPlayer>(PVPModule.Instance.PvpData.opponents);
        list.Sort((left, right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            if (left.rank != right.rank)
            {
                if (left.rank < right.rank)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            return 0;
        });

        int itemCount = enemy_dic.Count;
        if (list.Count < itemCount)
        {
            for (int i = list.Count; i < itemCount; i++)
            {
                PVPEnemyComponent comp = enemy_dic[i];
                if (comp == null) continue;
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            PVPEnemyComponent comp = null;
            if (i < itemCount)
            {
                comp = enemy_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyComp, view.Grd_EnemyGroup.transform);
                go.name = "Enemy_" + i.ToString();
                comp = new PVPEnemyComponent(go);
                enemy_dic.Add(comp);
                comp.playerInfoHandle = ButtonEvent_PlayerInfo;
                comp.challengeHandle = ButtonEvent_Challenge;
            }
            if (comp == null) continue;
            comp.UpdateInfo(list[i]);
            comp.mRootObject.SetActive(true);
            if (isRefresh)
            {
                if (Go_PVPRefreshEffect == null)
                {
                    ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_PVPREFRESH, (GameObject gb) =>
                    {
                        Go_PVPRefreshEffect = gb;
                        GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_PVPRefreshEffect, comp.Gobj_EffectItemPoint.transform);
                    });
                }
                else
                {
                    GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_PVPRefreshEffect, comp.Gobj_EffectItemPoint.transform);
                }
            }
        }
        isRefresh = false;
        view.Grd_EnemyGroup.Reposition();
    }
    private void UpdateOwnSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_OwnSoldier.enabled == false) return;
        if (realIndex >= ownSoldierList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            OwnSoldierLineupComponent comp = ownSoldier_dic[wrapIndex];
            if (comp == null) return;
            comp.UpdateCompInfo(ownSoldierList[realIndex]);
            comp.IsShowEnergy = true;
            comp.IsSelect = IsSelectedOwnSoldier(ownSoldierList[realIndex]);
            if (comp.IsSelect)
            {
                comp.IsMask = false;
            }
            if (!comp.IsSelect)
            {
                comp.IsEnable = !CheckSameSoldier(comp.soldierAtt);
                if (comp.IsEnable)
                {
                    comp.IsMask = CheckEnergyLimit(comp.soldierAtt);
                }
            }

        }
    }
    private void UpdateOwnEquipInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_OwnEquip.enabled == false) return;
        if (realIndex >= ownEquipList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            PVPOwnEquipTypeComponent comp = ownEquip_dic[wrapIndex];
            if (comp == null) return;
            comp.UpdateCompInfo(ownEquipList[realIndex]);
            comp.IsSelect = IsSelectedOwnEquip(ownEquipList[realIndex]);
        }
    }

    /// <summary>
    /// 当前士兵是否被选中
    /// </summary>
    /// <param name="equip"></param>
    /// <returns></returns>
    private bool IsSelectedOwnSoldier(Soldier soldier)
    {
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            Soldier readySoldier = readySoldierList[i].soldier;
            if (readySoldier.uId == soldier.uId)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// 当前装备是否被选中
    /// </summary>
    /// <param name="equip"></param>
    /// <returns></returns>
    private bool IsSelectedOwnEquip(Weapon equip)
    {
        if (equip == null) return false;
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            ReadyEquipData readyData = readyEquipArray[i];
            if (readyData == null || readyData.weapon == null) continue;
            Weapon readyEquip = readyData.weapon;
            if (readyEquip.uId == equip.uId)
            {
                return true;
            }
        }
        return false;
    }

    #endregion
    /// <summary>
    /// 取得当前准备携带的装备
    /// </summary>
    private List<EquipList> GetReadyEquipArray()
    {
        List<EquipList> equips = new List<EquipList>();
        for (int i = 0; i < readyEquipArray.Length; i++)
        {
            ReadyEquipData readyData = readyEquipArray[i];
            if (readyData == null || readyData.weapon == null) continue;
            Weapon weapon = readyData.weapon;
            if (weapon != null)
            {
                EquipList equip = new EquipList();
                equip.uid = weapon.uId;
                equip.pos = readyData.index;
                //switch ((EquiptType)weapon.Att.type)
                //{
                //    case EquiptType._Weapon:
                //        equip.pos = readyData.index;
                //        break;
                //    case EquiptType._ring:
                //        equip.pos = readyData.index;
                //        break;
                //    case EquiptType._necklace:
                //        equip.pos = readyData.index;
                //        break;
                //}
                equips.Add(equip);
            }
        }
        return equips;
    }

    /// <summary>
    /// 取得当前准备上阵的士兵
    /// </summary>
    private List<SoldierList> GetReadySoldierList()
    {
        List<SoldierList> list = new List<SoldierList>();
        for (int i = 0; i < readySoldierList.Count; i++)
        {
            ReadySoldier readySoldier = readySoldierList[i];
            SoldierList soldier = new SoldierList();
            soldier.uid = readySoldier.soldier.uId;
            soldier.num = readySoldier.count;
            list.Add(soldier);
        }
        return list;
    }

    private bool IsAbleChallenge()
    {
        if (PVPModule.Instance.CDTime > 0)
        {
            OpenResetCDTip();
            return false;
        }
        else if (PVPModule.Instance.ChallengeCount <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CHALLENGE_NOCOUNT);
            return false;
        }
        return true;
    }

    private void OpenResetCDTip()
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_CLEARCHALENGECD, arenaInfo.clear_cd_diamond),
          () =>
          {
              if (PVPModule.Instance.CDTime <= 0)
              {
                  UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CLEARCD_TIMEOUT);
              }
              else
              {
                  if (CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond, arenaInfo.clear_cd_diamond, true))
                      PVPModule.Instance.SendClearArenaCD();
              }
          }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
    }

    #region Button Event

    private void ButtonEvent_Revenge(GameObject go, ArenaRecord enemyRecord)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (IsAbleChallenge())
        {
            PVPModule.Instance.enemyPlayer = enemyRecord.opponent;

            PVPModule.Instance.SendArenaChallenge(PVPModule.Instance.enemyPlayer.hero.charid, PVPModule.Instance.enemyPlayer.rank);
            //CloseBattleLogPanel();
            //OpenLineupPanel();
            //view.Main_TScale.gameObject.SetActive(false);
            // Main.Instance.StartCoroutine(PlayOpenLineupSelectAnim());
        }
    }

    private void ButtonEvent_PlayerInfo(GameObject go, ArenaPlayer info)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (info != null)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
            UISystem.Instance.PlayerInfoView.UpdateViewInfo(PlayerInfoTypeEnum.Arena, info);
        }
    }

    private void ButtonEvent_Challenge(GameObject go, ArenaPlayer info)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (IsAbleChallenge())
        {
            PVPModule.Instance.enemyPlayer = info;
            PVPModule.Instance.SendArenaChallenge(PVPModule.Instance.enemyPlayer.hero.charid, PVPModule.Instance.enemyPlayer.rank);
        }
    }

    public void EnterReverge()
    {
        isAttack = true;
        CloseBattleLogPanel();
        OpenLineupPanel();
    }

    public void EnterChallenge()
    {
        isAttack = true;
        OpenLineupPanel();
    }

    private void PressEvent_OwnSoldier(BaseComponent baseComp, bool status)
    {
        if (status)
        {
            ownComp = baseComp as LineupItemComponent;
            Scheduler.Instance.AddTimer(0.5f, true, AddReadySoldier);
        }
        else
        {
            Scheduler.Instance.RemoveTimer(AddReadySoldier);
            ownComp = null;
        }
    }

    private void ButtonEvent_OwnSoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LineupItemComponent comp = baseComp as LineupItemComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent ");
            return;
        }
        if (CheckSameSoldier(comp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SOLDIER_SAME);
            return;
        }
        else if (CheckEnergyLimit(comp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYLIMIT);
            return;
        }

        if (UpdateReadySoldierData(comp.soldierAtt, true))
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.PVPSelectSoldierSucceed);
            comp.IsSelect = true;
            UpdateReadySoldiers();
            UpdateSoldierEnergyStatus();
            UpdateSameSoldierStatus();
        }
    }

    private LineupItemComponent ownComp = null;
    private void AddReadySoldier()
    {
        if (ownComp == null)
        {
            Debug.LogError("can not get LineupItemComponent ");
            Scheduler.Instance.RemoveTimer(AddReadySoldier);
            ownComp = null;
            return;
        }
        if (CheckSameSoldier(ownComp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SOLDIER_SAME);
            Scheduler.Instance.RemoveTimer(AddReadySoldier);
            ownComp = null;
            return;
        }
        else if (CheckEnergyLimit(ownComp.soldierAtt))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ENERGYLIMIT);
            Scheduler.Instance.RemoveTimer(AddReadySoldier);
            ownComp = null;
            return;
        }
        if (UpdateReadySoldierData(ownComp.soldierAtt, true))
        {
            ownComp.IsSelect = true;
            UpdateReadySoldiers();
            UpdateSoldierEnergyStatus();
            UpdateSameSoldierStatus();
        }
        else
        {
            Scheduler.Instance.RemoveTimer(AddReadySoldier);
            ownComp = null;
        }
    }

    private void UpdateSoldierEnergyStatus()
    {
        for (int i = 0; i < ownSoldier_dic.Count; i++)
        {
            OwnSoldierLineupComponent comp = ownSoldier_dic[i];
            comp.IsMask = CheckEnergyLimit(comp.soldierAtt);
        }
    }

    /// <summary>
    /// 已准备出站的士兵
    /// </summary>
    private void ButtonEvent_ReadySoldier(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LineupItemComponent comp = baseComp as LineupItemComponent;
        if (comp == null)
        {
            Debug.LogError("can not get LineupItemComponent");
            return;
        }
        if (UpdateReadySoldierData(comp.soldierAtt, false))
        {
            DelOwnSoldierData(comp.soldierAtt);
            UpdateReadySoldiers();
            UpdateSoldierEnergyStatus();
            UpdateSameSoldierStatus();
        }
    }

    /// <summary>
    /// 当前拥有的装备
    /// </summary>
    /// <param name="go"></param>
    private void ButtonEvent_OwnEquip(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        PVPOwnEquipTypeComponent comp = baseComp as PVPOwnEquipTypeComponent;
        if (comp == null) return;
        if (comp.IsSelect) return;
        if (comp.EquipInfo.IsLock) return;
        //if (comp.EquipInfo.Att.levelLimit >= PlayerData.Instance._Level)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_EQUIP_LEVELLIMIT);
        //    return;
        //}
        if (CheckEquipLimit(comp.EquipInfo))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_EXCEED_ARTIFACTLIMIT);
            return;
        }
        comp.IsSelect = true;
        UpdateReadyEquipData(comp.EquipInfo, comp.IsSelect);
        UpdateReadyEquips();
        GuideManager.Instance.CheckTrigger(GuideTrigger.PVPSelectGodWeaponSucceed);

    }
    /// <summary>
    /// 当前选择的装备
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_ReadyEquip(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        PVPEquipComponent comp = baseComp as PVPEquipComponent;
        if (comp == null) return;
        UpdateReadyEquipData(comp.EquipInfo, false);
        DelOwnEquipsData(comp.EquipInfo);
        UpdateReadyEquips();
    }
    /// <summary>
    /// 退出PVP
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Exit(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        ClearView();
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PVPVIEW);
    }

    /// <summary>
    /// 下一步
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Next(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (GuideManager.Instance.GuideIsRunning())
        {
            OpenEquipPanel();
            return;
        }
        if (readySoldierList.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.GATE_SOLDIERTIP_NOSOLDIER, () =>
            {
                if (CheckEnergyLimit())
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_EXCESS_ENERGY,
                                () =>
                                {
                                    OpenEquipPanel();
                                }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
                }
                else
                {
                    OpenEquipPanel();
                }
            });
        }
        else
        {
            if (readySoldierList.Count < GlobalCoefficient.CastSoldierTypeLimit)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_GENERALTYPE_LIMIT,
                    () =>
                    {
                        if (CheckEnergyLimit())
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_EXCESS_ENERGY,
                                () =>
                                {
                                    OpenEquipPanel();
                                }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
                        }
                        else
                        {
                            OpenEquipPanel();
                        }
                    }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
            }
            else
            {
                if (CheckEnergyLimit())
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_EXCESS_ENERGY,
                        () =>
                        {
                            OpenEquipPanel();
                        }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
                }
                else
                {
                    OpenEquipPanel();
                }
            }
        }

    }

    /// <summary>
    /// 关闭阵容选择
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_CloseLineupSelect(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseLineupPanel();
        UpdateViewInfo();
    }

    /// <summary>
    /// 开始PVP
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_StartBattle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        //int count = 0;    //LTD_20150824版本优化
        //for (int i = 0; i < readyEquipArray.Length; i++)
        //{
        //    if (readyEquipArray[i] != null)
        //        count++;
        //}
        //if (count < GlobalCoefficient.PVPEXCESSARTIFACTLIMIT)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_EXCESS_EQUIPGRID,
        //                    () =>
        //                    {
        //                        PVPModule.Instance.SendStartArena(PVPModule.Instance.enemyPlayer.hero.charid, GetReadyEquipArray(), GetReadySoldierList(), PVPModule.Instance.enemyPlayer.rank);
        //                    }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
        //}
        //else
        //{
        if (PVPModule.Instance.isFightState)
            return;
        PVPModule.Instance.isFightState = true;
        PVPModule.Instance.SendStartArena(PVPModule.Instance.enemyPlayer.hero.charid, GetReadyEquipArray(), GetReadySoldierList(), PVPModule.Instance.enemyPlayer.rank);
        //}
    }

    /// <summary>
    /// 保存阵容
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Save(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        //int count = 0;   //LTD_20150824版本优化
        //for (int i = 0; i < readyEquipArray.Length; i++)
        //{
        //    if (readyEquipArray[i] != null)
        //        count++;
        //}
        //if (count < GlobalCoefficient.PVPEXCESSARTIFACTLIMIT)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_EXCESS_EQUIPGRID,
        //                    () =>
        //                    {
        //                        PVPModule.Instance.SendSaveDefenceArray(GetReadyEquipArray(), GetReadySoldierList(), GetPlayerCombatPower());
        //                    }, null, ConstString.HINT_LEFTBUTTON_GOON, ConstString.HINT_RIGHTBUTTON_CANCEL);
        //}
        //else
        //{
        uint PetID = 0;
        if (currentPetData != null)
        {
            PetID = currentPetData.PetInfo.PetID;
        }




        PVPModule.Instance.SendSaveDefenceArray(GetReadyEquipArray(), GetReadySoldierList(), GetPlayerCombatPower(), PetID);
        GuideManager.Instance.CheckTrigger(GuideTrigger.PVPSaveTeamSucceed);

    }

    /// <summary>
    /// 规则说明
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_RuleDesc(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        OpenRuleDescPanel();
    }

    /// <summary>
    /// 排行榜
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Rank(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RANKVIEW);
        UISystem.Instance.RankView.ShowRank(RankType.ARENA_RANK);
    }

    /// <summary>
    /// 显示战斗日志
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_BattleLog(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        OpenBattleLogPanel();
    }

    /// <summary>
    /// 兑换奖励
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Reward(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_STORE);
        UISystem.Instance.StoreView.ShowStore(ShopType.ST_MedalShop);
    }

    /// <summary>
    /// 购买pvp挑战次数
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_Purchase(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        int buyTime = PVPModule.Instance.PvpData.buy_times;
        if (buyTime <= 0)
            buyTime = 0;
        VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        if (vipData != null)
        {
            if (buyTime >= vipData.ChallengeBuyCount)
            {
                if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Challenge))
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
                else
                {
                    string tip = ConstString.PVP_PURCHASELIMITTIP;
                    CommonFunction.ShowVipLvNotEnoughTip(tip);
                    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASELIMIT, buyTime, vipData.ChallengeBuyCount), () =>
                    //{
                    //    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                    //    UISystem.Instance.VipRechargeView.ShowRecharge();
                    //}, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                }
            }
            else
            {
                TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)buyTime + 1);
                if (data == null || data.AsynPvpTimes == null)// data为NULL 则可默认为已经达到购买上限
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
                else if (data.AsynPvpTimes.Type == ECurrencyType.None && data.AsynPvpTimes.Number == 0)  // 消耗金币类型为0则说明无购买次数
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
                else
                {
                    if (vipData.ChallengeBuyCount <= 0)
                    {
                        string tip = ConstString.PVP_PURCHASELIMITTIP;
                        CommonFunction.ShowVipLvNotEnoughTip(tip);
                    }
                    else
                    {
                        if (CommonFunction.CheckMoneyEnough(data.AsynPvpTimes.Type, data.AsynPvpTimes.Number, true))
                        {
                            if (buyTime >= vipData.ChallengeBuyCount)
                            {
                                string tip = ConstString.PVP_PURCHASELIMITTIP;
                                CommonFunction.ShowVipLvNotEnoughTip(tip);
                                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASELIMIT, buyTime, vipData.ChallengeBuyCount), () =>
                                //{
                                //    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                                //    UISystem.Instance.VipRechargeView.ShowRecharge();
                                //}, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                            }
                            else
                            {
                                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASECHANGLLECOUNT, data.AsynPvpTimes.Number, arenaInfo.max_times, buyTime, vipData.ChallengeBuyCount), () =>
                                {
                                    PVPModule.Instance.SendAddArenaTimes();
                                }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                            }
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 阵容调整
    /// </summary>
    /// <param name="btn"></param>
    private void ButtonEvent_AdjustLineup(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        isAttack = false;
        OpenLineupPanel();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenPVPSelectSoldierView);

    }

    /// <summary>
    /// 关闭装备选择 
    /// </summary>
    private void ButtonEvent_CloseEquipSelect(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseEquipPanel();
    }

    /// <summary>
    /// 关闭规则说明
    /// </summary>
    private void ButtonEvent_CloseRule(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseRuleDescPanel();
    }
    /// <summary>
    /// 关闭战斗日志
    /// </summary>
    private void ButtonEvent_CloseBattleLog(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseBattleLogPanel();
    }
    /// <summary>
    /// 清除挑战CD时间
    /// </summary>
    private void ButtonEvent_ClearTick(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        OpenResetCDTip();
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_CLEARCHALENGECD, arenaInfo.clear_cd_diamond),
        //    () =>
        //    {
        //        if (CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond, arenaInfo.clear_cd_diamond, true))
        //            PVPModule.Instance.SendClearArenaCD();
        //    }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
    }

    /// <summary>
    /// 刷新对手
    /// </summary>
    /// <param name="go"></param>
    private void ButtonEvent_RefreshPlayer(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PVP_REFRESHPLAYER,
        //    () =>
        //    {
        isRefresh = true;
        PVPModule.Instance.SendRefreshOpponents();
        //}, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
    }

    private void ButtonEvent_ChangePet(GameObject go)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETCHOOSE);
        uint petID = 0;
        if (currentPetData != null)
            petID = currentPetData.PetInfo.id;
        UISystem.Instance.PetChooseView.UpdateViewInfo(EFightType.eftPVP, petID);
    }
    #endregion

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Gobj_BattleLogMask.gameObject).onClick = ButtonEvent_CloseBattleLog;
        UIEventListener.Get(view.Gobj_RuleMask.gameObject).onClick = ButtonEvent_CloseRule;
        UIEventListener.Get(view.Btn_CloseLineupSelect.gameObject).onClick = ButtonEvent_CloseLineupSelect;
        UIEventListener.Get(view.Btn_CloseEquipSelect.gameObject).onClick = ButtonEvent_CloseEquipSelect;
        UIEventListener.Get(view.Btn_Exit.gameObject).onClick = ButtonEvent_Exit;
        UIEventListener.Get(view.Btn_Next.gameObject).onClick = ButtonEvent_Next;
        UIEventListener.Get(view.Btn_StartBattle.gameObject).onClick = ButtonEvent_StartBattle;
        UIEventListener.Get(view.Btn_Save.gameObject).onClick = ButtonEvent_Save;
        UIEventListener.Get(view.Btn_RuleDesc.gameObject).onClick = ButtonEvent_RuleDesc;
        UIEventListener.Get(view.Btn_Rank.gameObject).onClick = ButtonEvent_Rank;
        UIEventListener.Get(view.Btn_BattleLog.gameObject).onClick = ButtonEvent_BattleLog;
        UIEventListener.Get(view.Btn_Reward.gameObject).onClick = ButtonEvent_Reward;
        UIEventListener.Get(view.Btn_Purchase.gameObject).onClick = ButtonEvent_Purchase;
        UIEventListener.Get(view.Btn_AdjustLineup.gameObject).onClick = ButtonEvent_AdjustLineup;
        UIEventListener.Get(view.Btn_ClearTick.gameObject).onClick = ButtonEvent_ClearTick;
        UIEventListener.Get(view.Btn_RefreshPlayer.gameObject).onClick = ButtonEvent_RefreshPlayer;
        UIEventListener.Get(view.Gobj_EquipPet).onClick = ButtonEvent_ChangePet;
    }

    //界面动画
    //public IEnumerator PlayOpenMainAnim()
    //{
    //    yield return 0;
    //    view.Main_TScale.from = new Vector3(0.001f, 0.001f, 0.001f);
    //    view.Main_TScale.to = Vector3.one;
    //    view.Main_TScale.gameObject.transform.localScale = GlobalConst.ViewScaleAnimVec;
    //    view.Main_TScale.Restart();
    //    view.Main_TScale.PlayForward();
    //}
    //public IEnumerator PlayOpenRuleAnim()
    //{
    //    yield return 0;
    //    view.Rule_TScale.gameObject.transform.localScale = GlobalConst.ViewScaleAnimVec;
    //    view.Rule_TScale.Restart();
    //    view.Rule_TScale.PlayForward();
    //}
    //public IEnumerator PlayOpenEquipSelectAnim()
    //{
    //    yield return 0;
    //    view.EquipSelect_TScale.gameObject.transform.localScale = GlobalConst.ViewScaleAnimVec;
    //    view.EquipSelect_TScale.Restart();
    //    view.EquipSelect_TScale.PlayForward();
    //}
    //public IEnumerator PlayOpenBattleLogAnim()
    //{
    //    yield return 0;
    //    view.BattleLog_TScale.gameObject.transform.localScale = GlobalConst.ViewScaleAnimVec;
    //    view.BattleLog_TScale.Restart(); 
    //    view.BattleLog_TScale.PlayForward();
    //}
    //public IEnumerator PlayOpenLineupSelectAnim()
    //{
    //    yield return 0;
    //    view.LineupSelect_TScale.gameObject.transform.localScale = GlobalConst.ViewScaleAnimVec;
    //    view.LineupSelect_TScale.Restart(); 
    //    view.LineupSelect_TScale.PlayForward();
    //}

    public override void Uninitialize()
    {
        Scheduler.Instance.RemoveUpdator(UpdateChallengeCountDown);
        Scheduler.Instance.RemoveUpdator(UpdateLogCD);
        Scheduler.Instance.RemoveUpdator(UpdateBattleReadyTime);
        PVPModule.Instance.isFightState = false;
        PetSystemModule.Instance.PetChooseEvent -= OnUpdatePetInfo;
    }
    /// <summary>
    /// 重置PVP视图
    /// </summary>
    private void ClearView()
    {
        currentPetData = null;
        readySoldierList.Clear();
        ownEquipList.Clear();
        ownSoldierList.Clear();
        PlayerData.Instance.NotifyResetEvent -= ResetChallengeCount;
        arenaInfo = null;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        enemy_dic.Clear();
        battleLog_dic.Clear();
        defenseSoldier_Dic.Clear();
        ownEquip_dic.Clear();
        ownSoldier_dic.Clear();
        readyEquip_dic.Clear();
        readySoldier_dic.Clear();
        Scheduler.Instance.RemoveTimer(DelayEnterBattle);
        ruleAwradDescComp = null;
        ruleGetAwardComp = null;
        //ItemList.Clear();
        ClearView();
    }
    //=======================================================================//
    //public void PlayPVPRefreshEffect()
    //{
    //    for (int i = 0; i < ItemList.Count; i++)
    //    {
    //        GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(view.Go_PVPRefreshEffect, ItemList[i].transform);
    //    }
    //}
    //private void InitItemList()
    //{
    //    ItemList.Clear();
    //    ItemList.Add(view.ItemPoint1);
    //    ItemList.Add(view.ItemPoint2);
    //    ItemList.Add(view.ItemPoint3);
    //    ItemList.Add(view.ItemPoint4);

    //}
}

