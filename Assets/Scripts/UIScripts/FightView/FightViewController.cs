using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using Assets.Script.Common.StateMachine;

/// <summary>
/// 战斗界面
/// </summary>
public class FightViewController : UIBase
{

    private const int MAX_STAR_COUNT = 3;           //最大星级数量//
    private const int DELAY_HEARMOVETIME_SELF = 10; //己方英雄开启自动移动时间//
    private const int DELAY_HEARMOVETIME_ENEMY = 8; //敌方英雄开启自动移动时间//


    private FightView view;//战斗界面//
    private List<Transform> listFightStar = new List<Transform>();//战斗星级列表//
    private List<Transform> listFightNode = new List<Transform>();//战斗节点列表//
    private EFightType fightType;//战斗类型//
    private bool fightIsPause;//是否暂停//
    private float fightSpeed;//战斗速度//
    private FightViewInfo fightViewInfo;//战斗界面数据//
    private StageInfo fightStageInfo;//关卡数据//
    private int maxValue_FightTime;//战斗总时间//
    private int curValue_FightTime;//当前战斗时间//
    private int recovery_Energy;//能量恢复速度//
    private int maxValue_Energy;//能量总值//
    private int curValue_Energy;//当前能量值//
    private int recovery_Magic;//魔法恢复速度//
    private int maxValue_Magic;//魔法总值//
    private int curValue_Magic;//当前魔法值//
    private int recovery_EnemyMagic;//敌人魔法恢复速度//
    private int maxValue_EnemyMagic;//敌人魔法总值//
    private int curValue_EnemyMagic;//敌人当前魔法值//
    private int curValue_PetPower;//当前宠物怒气值//
    private int maxValue_PetPower;//宠物最大怒气值//
    private int addValue_PetPower;//宠物怒气值增长值//
    private int curValue_EnemyPetPower;//当前敌方宠物怒气值
    private int maxValue_EnemyPetPower;//敌方宠物最大怒气值//
    private int addValue_EnemyPetPower;//敌方宠物怒气值增长值
    private bool isStopEndless;//是否停止无尽模式[true-停止 false-继续]//
    private int fightNum;//战斗次数[无尽]
    private List<SoldierList> serverInfo = new List<SoldierList>();//重新开始服务端需要数据
    private bool fightStartIsOver;//战斗开始状态是否已经关闭
    private int max_StageValue_Self;//己方信息最大值//
    private int max_StageValue_Enemy;//敌方信息最大值//
    private int starValue;//星级评定数值//
    private int pvpSoldierCount_Self = 0;//己方武将总数//
    private int pvpSoldierCount_Enemy = 0;//敌方武将总数//
    private bool isChangeSelfSoldier = false;//是否修改己方武将信息//
    private bool isChangeEnemySoldier = false;//是否修改敌方武将信息//
    private int curSoldier_Self = 0;//当前己方武将数量//
    private int curSoldier_Enemy = 0;//当前敌方武将数量//
    private bool isSecondNewGuide = false;
    private List<Soldier> soldierListInfo = new List<Soldier>();
    private float btnTime_Quick = 0;//上次点击加速按钮时间//
    private bool heroMoveStatus;//英雄是否在移动
    private EHeroMoveType heroMoveType;//英雄移动屏蔽类型//
    private int bossHintTime = -1;//BOSS出场提示时间//
    private int initUnionHurt;//异域探险初始受伤害//
    private bool fightIsFinished;//战斗是否结束//
    private Dictionary<ulong, int> dicTemporaryEquipInfo = new Dictionary<ulong, int>();//己方臨時裝備信息//
    private bool isQuickResult = false;//是否开启快速显示结算//
    private float petSkillWidthScale_Self = 0;//宠物按钮暂用屏幕横向比例[己方]//
    private float petSkillWidthScale_Enemy = 1;//宠物按钮暂用屏幕横向比例[敌方]//
    private int autoFightStatus = 0;
    private float refreshSelfSoldierTime = -1;
    private float refreshEnemySoldierTime = -1;
    List<RefreshMonsterInfo> stageMonsterList_Endless;
    private float prePetSkillTime = 0;
    private ERoleDirection inputDirection = ERoleDirection.erdNone;
    private bool hasPet_Self;//是否拥有宠物[己方]//
    private bool hasPet_Enemy;//是否拥有宠物[敌方]//
    private Skill petSkillInfo_Self;//宠物技能数据[己方]//
    private Skill petSkillInfo_Enemy;//宠物技能数据[敌方]//
    private bool isAutoMove = false;
    private bool isAutoSkill = false;
    private bool isAutoSummon = false;

    /// <summary>
    /// 战斗玩家数据-自己
    /// </summary>
    public FightPlayerInfo fightPlayer_Self;
    /// <summary>
    /// 战斗玩家数据-敌人
    /// </summary>
    public FightPlayerInfo fightPlayer_Enemy;
    /// <summary>
    /// 玩家当前分数
    /// </summary>
    public float curValue_Score;
    /// <summary>
    /// 士兵列表
    /// </summary>
    public List<FightSoldierInfo> listSoldierInfo = new List<FightSoldierInfo>();
    /// <summary>
    /// 自己宠物信息
    /// </summary>
    public PetData pPetData;


    #region Get方法
    /// <summary>
    /// 获取当前能量值
    /// </summary>
    public int Get_CurValue_Energy
    {
        get
        {
            return curValue_Energy;
        }
    }
    /// <summary>
    /// 获取最大能量值
    /// </summary>
    public int Get_MAXValue_Energy
    {
        get
        {
            return maxValue_Energy;
        }
    }
    /// <summary>
    /// 获取当前魔法值
    /// </summary>
    public int Get_CurValue_Magic
    {
        get
        {
            return curValue_Magic;
        }
    }
    /// <summary>
    /// 获取最大魔法值
    /// </summary>
    public int Get_MAXValue_Magic
    {
        get
        {
            return maxValue_Magic;
        }
    }
    /// <summary>
    /// 获取敌人最大魔法值
    /// </summary>
    public int Get_MAXValue_EnemyMagic
    {
        get
        {
            return maxValue_EnemyMagic;
        }
    }
    /// <summary>
    /// 获取敌人当前魔法值
    /// </summary>
    public int Get_CurValue_EnemyMagic
    {
        get
        {
            return curValue_EnemyMagic;
        }
    }
    /// <summary>
    /// 当前宠物怒气值
    /// </summary>
    public int Get_CurValue_PetPower
    {
        get
        {
            return curValue_PetPower;
        }
        set
        {
            curValue_PetPower = value;
        }
    }
    /// <summary>
    /// 宠物最大怒气值
    /// </summary>
    public int Get_MAXValue_PetPower
    {
        get
        {
            return maxValue_PetPower;
        }
    }
    /// <summary>
    /// 当前敌方宠物怒气值
    /// </summary>
    public int Get_CurValue_EnemyPetPower
    {
        get
        {
            return curValue_EnemyPetPower;
        }
        set
        {
            curValue_EnemyPetPower = value;
        }
    }
    /// <summary>
    /// 敌方宠物最大怒气值
    /// </summary>
    public int Get_MAXValue_EnemyPetPower
    {
        get
        {
            return maxValue_EnemyPetPower;
        }
    }
    /// <summary>
    /// 获取战斗类型
    /// </summary>
    public EFightType Get_FightType
    {
        get
        {
            return fightType;
        }
    }
    /// <summary>
    /// 战斗开始状态是否已经结束
    /// </summary>
    public bool Get_FightStartIsOver
    {
        get
        {
            return fightStartIsOver;
        }
    }
    /// <summary>
    /// 是否暂停
    /// </summary>
    public bool Get_FightIsPause
    {
        get
        {
            return fightIsPause;
        }
    }
    /// <summary>
    /// 异域探险初始受伤害
    /// </summary>
    public int Get_InitUnionHurt
    {
        get
        {
            return initUnionHurt;
        }
    }
    /// <summary>
    /// 是否开启自动移动
    /// </summary>
    public bool Get_IsAutoMove
    {
        get
        {
            return isAutoMove;
        }
    }
    /// <summary>
    /// 是否开启自动释放技能
    /// </summary>
    public bool Get_IsAutoSkill
    {
        get
        {
            return isAutoSkill;
        }
    }
    /// <summary>
    /// 是否开启自动召唤武将
    /// </summary>
    public bool Get_IsAutoSummon
    {
        get
        {
            return isAutoSummon;
        }
    }
    #endregion

    #region 界面结构方法
    public override void Initialize()
    {
        if (view == null)
        {
            view = new FightView();
            view.Initialize();
            FightViewPanel_Pause.Instance.Initialize(view);
            FightViewPanel_Result.Instance.Initialize(view);
            FightViewPanel_PVE.Instance.Initialize(view);
            FightViewPanel_PVP.Instance.Initialize(view);
            FightViewPanel_Start.Instance.Initialize(view);
            petSkillWidthScale_Self = 0;
            petSkillWidthScale_Enemy = 1;

            if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.PET_POWER_MAX), out maxValue_PetPower))
            {
                maxValue_PetPower = 100;
            }
            maxValue_EnemyPetPower = maxValue_PetPower;
            if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.PET_POWER_ADD), out addValue_PetPower))
            {
                addValue_PetPower = 100;
            }
            addValue_EnemyPetPower = addValue_PetPower;
        }
        FightRelatedModule.Instance.isOpenNext = false;
        Main.Instance.StateMachine.ChangeState(FightState.StateName);
        BtnEventBinding();
        InitShowView();
        Scheduler.Instance.AddUpdator(FrameUpdate);
        Scheduler.Instance.AddTimer(1, true, FightTimeStatistical);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightReSetStar, CommandEvent_ReSetFightStarValue);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, CommandEvent_ReSetPlayerScore);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerEnergy, CommandEvent_ReSetPlayerEnergy);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerMagic, CommandEvent_ReSetPlayerMagic);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Self, CommandEvent_ExchangeInfo_Self);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Enemy, CommandEvent_ExchangeInfo_Enemy);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightHurtHint, CommandEvent_FightHurtHint);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_ReSetMoveStatus, CommandEvent_ReSetMoveStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_StartRecoveryMagic, CommandEvent_StartRecoveryMagic);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, CommandEvent_StartMove);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, CommandEvent_StoptMove);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_ChangeCurPetPower, CommandEvent_ChangePetPower);

        view.Btn_Main_Auto.state = UIButtonColor.State.Normal;
        view.Btn_Main_AutoSummon.state = UIButtonColor.State.Normal;
    }

    public override void Uninitialize()
    {
        fightStartIsOver = false;

        FightViewPanel_Pause.Instance.Uninitialize();
        FightViewPanel_Result.Instance.Uninitialize();
        FightViewPanel_PVE.Instance.Uninitialize();
        FightViewPanel_PVP.Instance.Uninitialize();
        FightViewPanel_Start.Instance.Uninitialize();

        Scheduler.Instance.RemoveUpdator(FrameUpdate);
        Scheduler.Instance.RemoveTimer(FightTimeStatistical);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightReSetStar, CommandEvent_ReSetFightStarValue);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerScore, CommandEvent_ReSetPlayerScore);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerEnergy, CommandEvent_ReSetPlayerEnergy);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerMagic, CommandEvent_ReSetPlayerMagic);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Self, CommandEvent_ExchangeInfo_Self);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightExchangeInfo_Enemy, CommandEvent_ExchangeInfo_Enemy);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightHurtHint, CommandEvent_FightHurtHint);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_ReSetMoveStatus, CommandEvent_ReSetMoveStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_StartRecoveryMagic, CommandEvent_StartRecoveryMagic);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, CommandEvent_StartMove);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, CommandEvent_StoptMove);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_ChangeCurPetPower, CommandEvent_ChangePetPower);
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        serverInfo.Clear();
        fightViewInfo = null;
        fightStageInfo = null;
        fightPlayer_Self = null;
        fightPlayer_Enemy = null;
        listFightStar.Clear();
        listFightNode.Clear();
        listSoldierInfo.Clear();
        FightRelatedModule.Instance.SelfHeroInfo = null;
        FightViewPanel_Pause.Instance.Destroy();
        FightViewPanel_Result.Instance.Destroy();
        FightViewPanel_PVE.Instance.Destroy();
        FightViewPanel_PVP.Instance.Destroy();
        FightViewPanel_Start.Instance.Destroy();
    }

    public override UIBoundary GetUIBoundary()
    {
        return view.Boundary;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Main_Pause.gameObject).onClick = ButtonEvent_Main_Pause;
        UIEventListener.Get(view.Btn_Main_Quicken.gameObject).onClick = ButtonEvent_Main_Quicken;
        UIEventListener.Get(view.Btn_Main_Auto.gameObject).onClick = ButtonEvent_Main_Auto;
        UIEventListener.Get(view.Btn_Main_AutoSummon.gameObject).onClick = ButtonEvent_Main_AutoSummon;
        UIEventListener.Get(view.Btn_Pause_ReStart.gameObject).onClick = ButtonEvent_Main_Pause_ReStart;
        UIEventListener.Get(view.Btn_Pause_Exit.gameObject).onClick = ButtonEvent_Main_Pause_Exit;
        UIEventListener.Get(view.Btn_Pause_Keep.gameObject).onClick = ButtonEvent_Main_Pause_Keep;
        UIEventListener.Get(view.Btn_Activity_ReStart.gameObject).onClick = ButtonEvent_Activity_ReStart;
        UIEventListener.Get(view.Btn_Activity_Exit.gameObject).onClick = ButtonEvent_Activity_Exit;
        UIEventListener.Get(view.Btn_Activity_Next.gameObject).onClick = ButtonEvent_Activity_Next;
        UIEventListener.Get(view.Btn_Result_Hint.gameObject).onClick = ButtonEvent_Result_Hint;
        UIEventListener.Get(view.Btn_Endless_Detail.gameObject).onClick = ButtonEvent_Endless_Detail;

        UIEventListener.Get(view.Btn_FailItem0.gameObject).onClick = ButtonEvent_FailItem0;
        UIEventListener.Get(view.Btn_FailItem1.gameObject).onClick = ButtonEvent_FailItem1;
        UIEventListener.Get(view.Btn_FailItem2.gameObject).onClick = ButtonEvent_FailItem2;
        UIEventListener.Get(view.Btn_FailItem3.gameObject).onClick = ButtonEvent_FailItem3;
        UIEventListener.Get(view.Btn_FailItem4.gameObject).onClick = ButtonEvent_FailItem4;

        UIEventListener.Get(view.Obj_Result_BG_Down.gameObject).onClick = ButtonEvent_QuickResult;

        UIEventListener.Get(view.Btn_PVE_PetSkill.gameObject).onClick = ButtonEvent_PVEPetSkill;
        UIEventListener.Get(view.Btn_Self_PetSkill.gameObject).onClick = ButtonEvent_PVPPetSkill_Self;
        UIEventListener.Get(view.Btn_Enemy_PetSkill.gameObject).onClick = ButtonEvent_PVPPetSkill_Enemy;
    }
    #endregion

    #region 战斗开始接口
    /// <summary>
    /// 设置界面显示-[新手引导 主线 活动 无尽-数据是无尽系列最后一个关卡的信息]
    /// </summary>
    /// <param name="vFightType">战斗类型</param>
    /// <param name="vStageInfo">关卡数据</param>
    /// <param name="list">武将阵容</param>
    /// <param name="vIsSecond">是否第二次开启新手引导战斗</param>
    public void SetViewInfo(EFightType vFightType, StageInfo vStageInfo, 
        List<Soldier> list, bool vIsSecond = false, PetData vPetData = null)
    {
        if (vStageInfo == null)
            return;
        if (fightStageInfo == null)
            fightStageInfo = new StageInfo();
        fightStageInfo.CopyTo(vStageInfo);
        isSecondNewGuide = vIsSecond;

        soldierListInfo.Clear();
        soldierListInfo.AddRange(list);
        fightType = vFightType;
        if (!vIsSecond)
        {
            CheckPetData(vPetData);
        }
        ReSetDataInfo(fightType, fightStageInfo);
        if (listSoldierInfo == null)
            listSoldierInfo = new List<FightSoldierInfo>();
        listSoldierInfo.Clear();
        for (int i = 0; i < soldierListInfo.Count; i++)
        {
            listSoldierInfo.Add(new FightSoldierInfo(soldierListInfo[i], 0));
        }

        ReSetUIStatus();
        if (fightStageInfo.IsBoss)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BOSS);
        else
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BATTLE);
        if (fightType == EFightType.eftEndless)
            FightRelatedModule.Instance.InitLocalEndlessData();

        RefreshMainStageInfo();
    }

    /// <summary>
    /// 设置界面显示-[攻城略地][跨服战]
    /// </summary>
    /// <param name="vFightType">战斗类型</param>
    /// <param name="vStageInfo">关卡数据</param>
    /// <param name="vSoldierListInfo">己方士兵信息</param>
    public void SetViewInfo(EFightType vFightType, ShowInfoHero vSelfHero, StageInfo vStageInfo, 
        Dictionary<ulong, int> vSelfEquip, List<FightSoldierInfo> vSoldierListInfo, PetData vPetData = null)
    {
        if ((vStageInfo == null) || (vSelfEquip == null) || (vSoldierListInfo == null))
            return;
        if (fightStageInfo == null)
            fightStageInfo = new StageInfo();
        fightStageInfo.CopyTo(vStageInfo);
        fightType = vFightType;
        CheckPetData(vPetData);
        ReSetDataInfo(fightType, fightStageInfo);
        if (FightRelatedModule.Instance.SelfHeroInfo == null)
            FightRelatedModule.Instance.SelfHeroInfo = new ShowInfoHero();
        if (vSelfHero != null)
            FightRelatedModule.Instance.SelfHeroInfo.CopyTo(vSelfHero);
        if (listSoldierInfo == null)
            listSoldierInfo = new List<FightSoldierInfo>();
        else
            listSoldierInfo.Clear();
        if (vSoldierListInfo != null)
        {
            listSoldierInfo.AddRange(vSoldierListInfo);
        }
        else
        {
            for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.remain_army.Count; i++)
            {
                Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(PlayerData.Instance._ExpeditionInfo.remain_army[i].uid);
                if (tmpSoldier == null)
                    continue;
                listSoldierInfo.Add(new FightSoldierInfo(tmpSoldier, PlayerData.Instance._ExpeditionInfo.remain_army[i].num));
            }
        }
        if (dicTemporaryEquipInfo == null)
            dicTemporaryEquipInfo = new Dictionary<ulong, int>();
        dicTemporaryEquipInfo.Clear();
        foreach (KeyValuePair<ulong, int> tmpInfo in vSelfEquip)
        {
            dicTemporaryEquipInfo.Add(tmpInfo.Key, tmpInfo.Value);
        }

        ReSetUIStatus();
        if (fightStageInfo.IsBoss)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BOSS);
        else
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BATTLE);
    }

    /// <summary>
    /// 设置界面显示-[远征]
    /// </summary>
    /// <param name="vFightType">战斗类型</param>
    /// <param name="vServerInfo">再次开始战斗时服务端需要数据</param>
    /// <param name="vSoldierListInfo">己方士兵信息</param>
    public void SetViewInfo(EFightType vFightType, List<SoldierList> vServerInfo, 
        List<FightSoldierInfo> vSoldierListInfo = null, PetData vPetData = null)
    {
        if (PlayerData.Instance._ExpeditionInfo == null)
            return;

        serverInfo.Clear();
        if (vServerInfo != null)
            serverInfo.AddRange(vServerInfo);
        fightType = vFightType;
        CheckPetData(vPetData);
        ExpeditionData tmpExpeditionInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(PlayerData.Instance._ExpeditionInfo.cur_gate_id);
        ReSetDataInfo(fightType, tmpExpeditionInfo);
        if (listSoldierInfo == null)
            listSoldierInfo = new List<FightSoldierInfo>();
        else
            listSoldierInfo.Clear();
        if (vSoldierListInfo != null)
        {
            listSoldierInfo.AddRange(vSoldierListInfo);
        }
        else
        {
            for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.remain_army.Count; i++)
            {
                Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(PlayerData.Instance._ExpeditionInfo.remain_army[i].uid);
                if (tmpSoldier == null)
                    continue;
                listSoldierInfo.Add(new FightSoldierInfo(tmpSoldier, PlayerData.Instance._ExpeditionInfo.remain_army[i].num));
            }
        }

        ReSetUIStatus();
        RefreshMainStageInfo();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BATTLE);
    }

    /// <summary>
    /// 设置界面显示-[演武台 奴役 全服争霸 排位賽]
    /// </summary>
    /// <param name="vFightType">战斗类型</param>
    /// <param name="vSelfHero">自己英雄临时数据</param>
    /// <param name="vSelfEquip">自己装备临时数据[UID 位置索引][没有衣服和坐骑 需要从PlayerData中查找]</param>
    /// <param name="vSelfSoldier">自己出战武将数据[UID 数量]</param>
    /// <param name="vEnemyInfo">对方数据</param>
    public void SetViewInfo(EFightType vFightType, ShowInfoHero vSelfHero, Dictionary<ulong, int> vSelfEquip, 
        Dictionary<ulong, int> vSelfSoldier, ArenaPlayer vEnemyInfo, PetData vPetData = null)
    {
        if ((vSelfHero == null) || (vSelfEquip == null) ||
            (vSelfSoldier == null) || (vEnemyInfo == null))
            return;

        if (fightPlayer_Self == null)
            fightPlayer_Self = new FightPlayerInfo(vSelfHero, vSelfEquip, vSelfSoldier);
        else
            fightPlayer_Self.ReSetInfo(vSelfHero, vSelfEquip, vSelfSoldier);

        if (fightPlayer_Enemy == null)
            fightPlayer_Enemy = new FightPlayerInfo(vEnemyInfo);
        else
            fightPlayer_Enemy.ReSetInfo(vEnemyInfo);

        fightType = vFightType;
        CheckPetData(vPetData);
        FightRelatedModule.Instance.ServerHegemonyHurtValue = 0;
        ReSetDataInfo(fightType, null);
        ReSetUIStatus();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BATTLE);
    }

    /// <summary>
    /// 设置界面显示-[异域探险]
    /// </summary>
    /// <param name="vFightType">战斗类型</param>
    /// <param name="vStageInfo">关卡数据</param>
    /// <param name="vList">己方士兵信息</param>
    /// <param name="vSelfEquip">己方英雄装备信息</param>
    /// <param name="vHurtValue">BOSS已受伤害数值</param>
    public void SetViewInfo(EFightType vFightType, ShowInfoHero showInfoHero, StageInfo vStageInfo, 
        List<FightSoldierInfo> vSoldierListInfo, Dictionary<ulong, int> vSelfEquip, int vHurtValue, PetData vPetData = null)
    {
        if (vStageInfo == null)
            return;
        if (fightStageInfo == null)
            fightStageInfo = new StageInfo();
        fightStageInfo.CopyTo(vStageInfo);
        fightType = vFightType;
        CheckPetData(vPetData);
        ReSetDataInfo(fightType, fightStageInfo);
        if (FightRelatedModule.Instance.SelfHeroInfo == null)
            FightRelatedModule.Instance.SelfHeroInfo = new ShowInfoHero();
        if (showInfoHero != null)
            FightRelatedModule.Instance.SelfHeroInfo.CopyTo(showInfoHero);
        initUnionHurt = vHurtValue;

        if (listSoldierInfo == null)
            listSoldierInfo = new List<FightSoldierInfo>();
        else
            listSoldierInfo.Clear();
        if (vSoldierListInfo != null)
            listSoldierInfo.AddRange(vSoldierListInfo);
        if (dicTemporaryEquipInfo == null)
            dicTemporaryEquipInfo = new Dictionary<ulong, int>();
        dicTemporaryEquipInfo.Clear();
        foreach (KeyValuePair<ulong, int> tmpInfo in vSelfEquip)
        {
            dicTemporaryEquipInfo.Add(tmpInfo.Key, tmpInfo.Value);
        }
        ReSetUIStatus();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_BATTLE);
    }
    #endregion

    #region 按钮点击方法
    /// <summary>
    /// 主界面-暂停
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Pause(GameObject btn)
    {
        if (fightIsFinished)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, null));

        if (fightType == EFightType.eftMain)
        {
            if (fightStageInfo != null)
            {
                if (FightRelatedModule.Instance.DicSpeTutorial != null)
                {
                    foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeTutorial)
                    {
                        if (fightStageInfo.ID != tmpInfo.Key)
                            continue;
                        if (GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                            break;
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NOPAUSE_TUTORIAL);
                        return;
                    }
                }
            }
        }
        else if (fightType == EFightType.eftNewGuide)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NOPAUSE_TUTORIAL);
            return;
        }

        if (SceneManager.Instance.Get_CurScene == null)
            return;
        if (SceneManager.Instance.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;
        fightIsPause = true;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetPause, null);
        FightViewPanel_Pause.Instance.OpenPanel(fightType);
    }

    /// <summary>
    /// 主界面-加速
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Quicken(GameObject btn)
    {
        if (fightIsFinished)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (Time.time - btnTime_Quick < 1f)
            return;
        if ((fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_AUTO_FIGHT);
            return;
        }
        btnTime_Quick = Time.time;
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.DoubleFightSpeed))
            return;
        if (SceneManager.Instance.Get_CurScene == null)
            return;
        if (SceneManager.Instance.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;

        if (fightSpeed == GlobalConst.MAX_FIGHT_SPEED)
        {
            fightSpeed = GlobalConst.MIN_FIGHT_SPEED;
            CommonFunction.SetSpriteName(view.Spt_BtnMain_QuickenSpeed, GlobalConst.SpriteName.FightSpeed_1);
        }
        else
        {
            fightSpeed = GlobalConst.MAX_FIGHT_SPEED;
            CommonFunction.SetSpriteName(view.Spt_BtnMain_QuickenSpeed, GlobalConst.SpriteName.FightSpeed_2);
        }

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, fightSpeed);
        Scheduler.Instance.RemoveTimer(FightTimeStatistical);
        Scheduler.Instance.AddTimer(1 / fightSpeed, true, FightTimeStatistical);
    }

    /// <summary>
    /// 主界面-自动战斗
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Auto(GameObject btn)
    {
        if (fightIsFinished)
            return;
        GuideManager.Instance.CheckTrigger(GuideTrigger.ClickAutoFight);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_AUTO_FIGHT);
            return;
        }
        if ((fightType == EFightType.eftNewGuide) || (autoFightStatus == (int)EAutoFightStatus.eafsLock))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_AUTOFIGHT_UNLOCKHINT));
            return;
        }
        isAutoMove = !isAutoMove;
        isAutoSkill = !isAutoSkill;
        view.Trans_BtnMain_AutoEffect.gameObject.SetActive(isAutoMove);
        if ((isAutoMove) && (isAutoSkill))
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_COMMON);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_LOCK);
        }
        if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
        {
            SceneManager.Instance.IsAutoMove = isAutoMove;
            SceneManager.Instance.IsAutoSkill = isAutoSkill;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ChangeFightAutoStatus, autoFightStatus);
    }

    /// <summary>
    /// 自动招兵
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_AutoSummon(GameObject btn)
    {
        if (fightIsFinished)
            return;
        GuideManager.Instance.CheckTrigger(GuideTrigger.ClickAutoCall);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) ||
            (fightType == EFightType.eftExpedition) || (fightType == EFightType.eftUnion) || (fightType == EFightType.eftCrossServerWar) ||
            (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_AUTO_FIGHT);
            return;
        }
        if ((fightType == EFightType.eftNewGuide) || (autoFightStatus == (int)EAutoFightStatus.eafsLock))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_AUTOFIGHT_UNLOCKHINT));
            return;
        }

        isAutoSummon = !isAutoSummon;
        view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(isAutoSummon);
        if (isAutoSummon)
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_COMMON);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_LOCK);
        }
        if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
        {
            SceneManager.Instance.IsAutoSummon = isAutoSummon;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ChangeFightAutoStatus, autoFightStatus);
    }

    /// <summary>
    /// 暂停界面-重新开始
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Pause_ReStart(GameObject btn)
    {
        if (fightIsFinished)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, false);
        ReStartOperate();
    }

    /// <summary>
    /// 暂停界面-退出[无尽模式需要进入结算流程]
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Pause_Exit(GameObject btn)
    {
        if (fightIsFinished)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (fightType == EFightType.eftEndless)
        {
            isStopEndless = true;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsStop));
            FightViewPanel_Pause.Instance.ClosePanel();
        }
        else if ((fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftCrossServerWar))
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsStop));
            FightViewPanel_Pause.Instance.ClosePanel();
        }
        else
        {
            ExitOperate();
        }
    }

    /// <summary>
    /// 暂停界面-继续
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Main_Pause_Keep(GameObject btn)
    {
        if (fightIsFinished)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        fightIsPause = false;
        FightViewPanel_Pause.Instance.ClosePanel();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
    }

    /// <summary>
    /// 结算界面-重新开始
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Activity_ReStart(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, false);

        if (fightViewInfo == null)
            return;
        if (fightType == EFightType.eftNewGuide)
        {
            StageInfo tmpStageInfo = new StageInfo();
            tmpStageInfo.CopyTo(fightStageInfo);
            List<Soldier> tmpListSoldier = new List<Soldier>();
            if (soldierListInfo != null)
                tmpListSoldier.AddRange(soldierListInfo);

            NewGuideOperate();

            if (fightViewInfo.curID == GlobalConst.FIRST_STAGE_ID)
                GuideManager.Instance.ClearAndRestartGuide();
            else
                SetViewInfo(fightType, tmpStageInfo, tmpListSoldier, true);
            return;
        }

        if ((fightType != EFightType.eftMain) && (fightType != EFightType.eftActivity))
            return;
        ReStartOperate();
    }

    /// <summary>
    /// 结算界面-下一关
    /// </summary>
    /// <param name="go"></param>
    public void ButtonEvent_Activity_Next(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (fightViewInfo == null)
            return;

        List<StageData> list = PlayerData.Instance.GetAvailableGates();
        if (list == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
            return;
        }
        StageData stageData = list.Find((tmp) =>
        {
            if (tmp == null || tmp.gateinfo == null)
            {
                return false;
            }
            return tmp.gateinfo.dgn_id == fightViewInfo.nextID;
        });
        if (stageData == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_GATENOTOPEN);
            return;
        }
        if (!stageData.stageinfo.IsBoss && stageData.gateinfo.star_level > 0)
        {
            //既然已经取得数据  则说明该关卡已经解锁
            //星级大于0 则说明该小关卡已经被打过
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_RECHALLENGELIMIT);
            return;
        }
        FightRelatedModule.Instance.isOpenNext = true;
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        Main.Instance.StateMachine.ChangeState(MainCityState.StateName);
        switch (fightType)
        {
            case EFightType.eftMain:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
                    UISystem.Instance.GateInfoView.UpdateViewInfo(stageData.stageinfo, stageData.gateinfo, stageData.remainRaidTimes, EFightType.eftMain);
                }
                break;
        }

    }

    /// <summary>
    /// 结算界面-退出
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Activity_Exit(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGameSettlement);
        ExitOperate();
    }

    /// <summary>
    /// 结算界面-点击任意位置退出
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Result_Hint(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGameSettlement);
        ExitOperate();
    }

    /// <summary>
    /// 结算界面-无尽模式详情
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Endless_Detail(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ENDLESSRESULTLIST);
    }

    /// <summary>
    /// 神器强化[点击后跳转到背包中的神器页签-修改为跳转到英雄界面的神器标签]
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FailItem0(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CheckIsStillNewGuide())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIGHTVIEW_FAILTOREDIRECT);
            return;
        }
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Hero))
        {
            return;
        }
        ExitOperate(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
        UISystem.Instance.HeroAttView.SetToggle(true);
    }

    /// <summary>
    /// 技能升级[点击后跳转至英雄界面，且右边“神器/技能”页签中，自动选中技能页签。]
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FailItem1(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CheckIsStillNewGuide())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIGHTVIEW_FAILTOREDIRECT);
            return;
        }
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Hero))
        {
            return;
        }
        ExitOperate(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
        UISystem.Instance.HeroAttView.SetToggle(false);
    }

    /// <summary>
    /// 武将升级[文字“武将升级”修改为“提升武将”，点击后跳转至武将界面，且“装备/技能”中自动选中技能页签。]
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FailItem2(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CheckIsStillNewGuide())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIGHTVIEW_FAILTOREDIRECT);
            return;
        }
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Soldier))
        {
            return;
        }
        ExitOperate(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERATT);
        UISystem.Instance.SoldierAttView.SetToggle(false);
    }

    /// <summary>
    /// 装备强化[点击后跳转至武将界面]
    /// 修改为-命魂
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FailItem3(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CheckIsStillNewGuide())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIGHTVIEW_FAILTOREDIRECT);
            return;
        }
        if (MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.LifeSoulSystm))
        {
            return;
        }
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.LifeSoulSystm))
        {
            return;
        }
        ExitOperate(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITVIW);
    }

    /// <summary>
    /// 储值
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FailItem4(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CheckIsStillNewGuide())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FIGHTVIEW_FAILTOREDIRECT);
            return;
        }
        ExitOperate(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECHARGE);
        UISystem.Instance.VipRechargeView.ShowVipPrivilege();
    }

    /// <summary>
    /// 加快速度显示结算信息
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_QuickResult(GameObject btn)
    {
        if (isQuickResult)
        {
            FightViewPanel_Result.Instance.FindTweenOperate();
            isQuickResult = false;
        }
    }

    /// <summary>
    /// PVE宠物技能
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_PVEPetSkill(GameObject btn)
    {
        if (curValue_PetPower >= maxValue_PetPower)
        {
            if (hasPet_Self)
            {
                if (FightViewPanel_PVE.Instance.Get_ItemStatus == EFightViewSkillStatus.eskNormal)
                {
                    curValue_PetPower = 0;
                    RefreshInfo_PetPower();
                    if (petSkillInfo_Self != null)
                    {
                        petSkillInfo_Self.UseSkill(RoleManager.Instance.Get_Hero);
                    }
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightPetUseSkill);
                    FightViewPanel_PVE.Instance.ClickPetSkillOperate();
                }
            }
        }
    }

    /// <summary>
    /// PVP宠物技能-己方
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_PVPPetSkill_Self(GameObject btn)
    {
        if (btn != null)
        {
            if (curValue_PetPower >= maxValue_PetPower)
            {
                curValue_PetPower = 0;
                RefreshInfo_PetPower();
                if (hasPet_Self)
                {
                    if (petSkillInfo_Self != null)
                    {
                        petSkillInfo_Self.UseSkill(btn.GetComponent<RoleBase>());
                    }
                }
                FightViewPanel_PVP.Instance.ClickPetSkillOperate(EFightCamp.efcSelf);
            }
        }
    }

    /// <summary>
    /// PVP宠物技能-敌方
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_PVPPetSkill_Enemy(GameObject btn)
    {
        if (btn != null)
        {
            if (curValue_EnemyPetPower >= maxValue_EnemyPetPower)
            {
                curValue_EnemyPetPower = 0;
                RefreshInfo_PetPower();
                if (hasPet_Enemy)
                {
                    if (petSkillInfo_Enemy != null)
                    {
                        petSkillInfo_Enemy.UseSkill(btn.GetComponent<RoleBase>());
                    }
                }
                FightViewPanel_PVP.Instance.ClickPetSkillOperate(EFightCamp.efcEnemy);
            }
        }
    }
    #endregion

    #region 命令方法
    /// <summary>
    /// 重置玩家积分
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetPlayerScore(object vDataObj)
    {
        if (vDataObj == null)
            return;
        float tmpValue = (float)vDataObj;
        if (fightType == EFightType.eftCaptureTerritory)
        {
            tmpValue = (tmpValue * ConfigManager.Instance.mCaptureTerritoryConfig.BloodScoreFactor) * (1 + CaptureTerritoryModule.Instance.GetCurrentTokenIncrease());
        }
        else if (fightType == EFightType.eftCrossServerWar)
        {
            tmpValue = tmpValue * ConfigManager.Instance.mCrossServerWarConfig.ExchangePointScale();
        }
        curValue_Score += tmpValue;
        if (curValue_Score < 0)
            curValue_Score = 0;
        RefreshInfo_PlayerScore();
    }

    /// <summary>
    /// 修改当前能量值
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetPlayerEnergy(object vDataObj)
    {
        if (vDataObj == null)
            return;
        curValue_Energy += (int)vDataObj;
        RefreshInfo_Energy();
    }

    /// <summary>
    /// 修改当前魔法值
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetPlayerMagic(object vDataObj)
    {
        FightDecMP tmpInfo = (FightDecMP)vDataObj;
        if (tmpInfo == null)
            return;
        if (tmpInfo.mFightCamp == EFightCamp.efcNone)
            return;
        else if (tmpInfo.mFightCamp == EFightCamp.efcSelf)
            curValue_Magic += tmpInfo.mMPChangeValue;
        else
            curValue_EnemyMagic += tmpInfo.mMPChangeValue;
        RefreshInfo_Magic();
    }

    /// <summary>
    /// 修改己方城堡血量/逃亡敌人数量信息
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ExchangeInfo_Self(object vDataObj)
    {
        if (vDataObj == null)
            return;
        int tmpCurValue = (int)vDataObj;
        if (tmpCurValue < 0)
            tmpCurValue = 0;
        float tmpValue = (float)tmpCurValue / max_StageValue_Self;
        if (tmpValue > 1)
            tmpValue = 1;
        else if (tmpValue < 0)
            tmpValue = 0;
        view.Sli_Self_Progress.value = tmpValue;
        view.Lbl_Self_Info.text = string.Format("{0}/{1}", tmpCurValue, max_StageValue_Self);

        if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
        {
            if (fightStageInfo == null)
                return;
            if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestDefen)
            {
                CommandEvent_ReSetFightStarValue((int)(tmpValue * 100));
            }
            else if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestTransfer)
            {
                CommandEvent_ReSetFightStarValue(max_StageValue_Self - tmpCurValue);
            }
        }
    }

    /// <summary>
    /// 修改敌方城堡血量信息
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ExchangeInfo_Enemy(object vDataObj)
    {
        if (vDataObj == null)
            return;
        int tmpCurValue = (int)vDataObj;
        if (tmpCurValue < 0)
            tmpCurValue = 0;
        float tmpValue = (float)tmpCurValue / max_StageValue_Enemy;
        if (tmpValue > 1)
            tmpValue = 1;
        else if (tmpValue < 0)
            tmpValue = 0;
        view.Sli_Enemy_Progress.value = tmpValue;
        view.Lbl_Enemy_Info.text = string.Format("{0}/{1}", tmpCurValue, max_StageValue_Enemy);
    }

    /// <summary>
    /// 重置星级评定数值
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetFightStarValue(object vDataObj)
    {
        if (vDataObj == null)
            return;
        starValue = (int)vDataObj;
    }

    /// <summary>
    /// 闪红提示
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightHurtHint(object vDataObj)
    {
        view.UIPanel_HurtHint.gameObject.SetActive(!view.UIPanel_HurtHint.gameObject.active);
    }

    /// <summary>
    /// 重置英雄移动状态
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ReSetMoveStatus(object vDataObj)
    {
        heroMoveType = (EHeroMoveType)vDataObj;
    }

    /// <summary>
    /// 开启魔法回复
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartRecoveryMagic(object vDataObj)
    {
        recovery_Energy = PlayerData.Instance._Attribute.EnergyRecovery;
        recovery_Magic = PlayerData.Instance._Attribute.MPRecovery;
    }

    /// <summary>
    /// Boss出场
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_BossInScene(object vDataObj)
    {
        if ((EChatType)vDataObj == EChatType.ectNone)
            return;
        DelayShowBossTalk(vDataObj);
    }

    /// <summary>
    /// BOSS出场对话结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_CloseShowBossTalk(object vDataObj)
    {
        if (!fightIsPause)
            return;
        fightIsPause = false;
        FightViewPanel_Pause.Instance.ClosePanel();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
    }

    /// <summary>
    /// BOSS出场提示
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_BossHint(object vDataObj)
    {
        if (view.TP_Main_BossHint == null)
            return;
        view.TP_Main_BossHint.Restart();
        view.TP_Main_BossHint.from = new Vector3(1024, 150, 0);
        view.TP_Main_BossHint.to = new Vector3(45, 150, 0);
        Scheduler.Instance.AddTimer(2, false, CloseShowBossHint);
        view.TP_Main_BossHint.PlayForward();
    }

    /// <summary>
    /// 开启移动
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartMove(object vDataObj)
    {
        if (vDataObj == null)
            return;
        ERoleDirection tmpDir = (ERoleDirection)vDataObj;
        if (inputDirection == tmpDir)
            return;
        ReSetMoveBtnStatus(tmpDir);
    }

    /// <summary>
    /// 停止移动
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StoptMove(object vDataObj)
    {
        if (inputDirection == ERoleDirection.erdNone)
            return;
        ReSetMoveBtnStatus(ERoleDirection.erdNone);
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightFinished(object vDataObj)
    {
        fightIsFinished = true;
    }

    /// <summary>
    /// 修改宠物当前怒气值
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_ChangePetPower(object vDataObj)
    {
        if (vDataObj != null)
        {
            EFightCamp tmpPetCamp = (EFightCamp)vDataObj;
            if (tmpPetCamp == EFightCamp.efcSelf)
            {
                curValue_PetPower += addValue_PetPower;
                if (curValue_PetPower > maxValue_PetPower)
                {
                    curValue_PetPower = maxValue_PetPower;
                }
            }
            else
            {
                curValue_EnemyPetPower += addValue_EnemyPetPower;
                if (curValue_EnemyPetPower > maxValue_EnemyPetPower)
                {
                    curValue_EnemyPetPower = maxValue_EnemyPetPower;
                }
            }
        }
        RefreshInfo_PetPower();
    }
    #endregion

    #region 私有方法
    /// <summary>
    /// 设置移动指示按钮状态
    /// </summary>
    /// <param name="vDirection"></param>
    private void ReSetMoveBtnStatus(ERoleDirection vDirection)
    {
        if (view != null)
        {
            switch (vDirection)
            {
                case ERoleDirection.erdNone:
                    {
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Left, "HXZD_btn_zuoyouanniu2");
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Right, "HXZD_btn_zuoyouanniu2");
                    }
                    break;
                case ERoleDirection.erdLeft:
                    {
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Left, "HXZD_btn_zuoyouanniu1");
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Right, "HXZD_btn_zuoyouanniu2");
                    }
                    break;
                case ERoleDirection.erdRight:
                    {
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Left, "HXZD_btn_zuoyouanniu2");
                        CommonFunction.SetSpriteName(view.Spt_MoveHint_Right, "HXZD_btn_zuoyouanniu1");
                    }
                    break;
            }
            inputDirection = vDirection;
        }
    }

    /// <summary>
    /// 关闭BOSS出场提示
    /// </summary>
    private void CloseShowBossHint()
    {
        if ((view != null) && (view.TP_Main_BossHint != null))
        {
            view.TP_Main_BossHint.Restart();
            view.TP_Main_BossHint.from = new Vector3(45, 150, 0);
            view.TP_Main_BossHint.to = new Vector3(-1024, 150, 0);
            view.TP_Main_BossHint.PlayForward();
        }
    }

    /// <summary>
    /// 延迟演示BOSS出场对话操作
    /// </summary>
    /// <param name="vDataObj"></param>
    private void DelayShowBossTalk(object vDataObj)
    {
        //判断是否有数据[有数据-暂停显示对话信息 没有数据-镜头还原]//
        if (fightStageInfo == null)
            return;

        if (!ConfigManager.Instance.mBossTalkConfig.CheckIsHaveData(fightStageInfo.ID, (EChatType)vDataObj))
        {
            SceneManager.Instance.Get_CurScene.ReSetScenePos();
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk);
        }
        else
        {
            PauseForFightTalk();
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_StartShowBossTalk, vDataObj);

            if ((EChatType)vDataObj == EChatType.ectBossTalk)
            {
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ShowBossStatus);
            }
        }
    }

    private void PauseForFightTalk()
    {
        fightIsPause = true;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetPause, null);
        FightViewPanel_Pause.Instance.OpenPanel();
    }

    /// <summary>
    /// 初始化界面显示
    /// </summary>
    private void InitShowView()
    {
        FightViewPanel_Pause.Instance.InitPanel();
        FightViewPanel_Result.Instance.InitPanel();
        FightViewPanel_PVE.Instance.InitPanel();
        FightViewPanel_PVP.Instance.InitPanel();
        FightViewPanel_Start.Instance.InitPanel();

        fightIsPause = true;
        fightSpeed = SceneManager.Instance.Get_FightSpeed;
        if ((fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftPVP))
        {
            fightSpeed = GlobalConst.MAX_FIGHT_SPEED;
        }
        curValue_Score = 0;
        maxValue_FightTime = 1;
        curValue_FightTime = 0;
        bossHintTime = 0;
        maxValue_Energy = 1;
        curValue_Energy = 0;
        recovery_Energy = 0;
        maxValue_Magic = 1;
        curValue_Magic = 0;
        recovery_Magic = 0;
        maxValue_EnemyMagic = 1;
        curValue_EnemyMagic = 0;
        recovery_EnemyMagic = 0;
        fightNum = 1;
        fightIsFinished = false;
        curValue_PetPower = 0;
        curValue_EnemyPetPower = 0;
        view.Btn_PVE_PetSkill.gameObject.SetActive(false);
        view.Btn_Self_PetSkill.gameObject.SetActive(false);
        view.Btn_Enemy_PetSkill.gameObject.SetActive(false);
        view.Btn_Self_PetSkill.GetComponent<BoxCollider>().enabled = false;
        view.Btn_Enemy_PetSkill.GetComponent<BoxCollider>().enabled = false;

        if (fightSpeed == GlobalConst.MIN_FIGHT_SPEED)
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_QuickenSpeed, GlobalConst.SpriteName.FightSpeed_1);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_BtnMain_QuickenSpeed, GlobalConst.SpriteName.FightSpeed_2);
        }
        InitInfo_FightStar();
        RefreshInfo_PlayerScore(false);
        RefreshInfo_FightProgress();
        RefreshInfo_FightNodeList(null);
        InitMainStageInfo();
        InitMainPVPInfo();
        RefreshInfo_FightTime();
        view.UIPanel_HurtHint.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始化PVP显示
    /// </summary>
    private void InitMainPVPInfo()
    {
        view.UIWidget_Main_PVPInfo.gameObject.SetActive(false);
        view.Lbl_PVPInfo_Text.text = string.Empty;
        view.Sli_SelfPVP_ProgressUp.value = 0;
        view.Sli_SelfPVP_ProgressDown.value = 0;
        view.Lbl_SelfPVP_Info.text = string.Empty;
        view.Sli_EnemyPVP_ProgressUp.value = 0;
        view.Sli_EnemyPVP_ProgressDown.value = 0;
        view.Lbl_EnemyPVP_Info.text = string.Empty;
    }

    /// <summary>
    /// 刷新PVP显示
    /// </summary>
    /// <param name="vSelfSoldierCount"></param>
    /// <param name="vEnemySoldierCount"></param>
    private void RefreshMainPVPInfo(int vSelfSoldierCount, int vEnemySoldierCount)
    {
        if (vSelfSoldierCount <= 0)
        {
            vSelfSoldierCount = 0;
            view.Sli_SelfPVP_ProgressUp.value = 0;
            view.Sli_SelfPVP_ProgressDown.value = 0;
        }
        else
        {
            view.Sli_SelfPVP_ProgressUp.value = 1;
            view.Sli_SelfPVP_ProgressDown.value = 1;
        }
        curSoldier_Self = vSelfSoldierCount;
        curSoldier_Enemy = vEnemySoldierCount;
        view.Lbl_SelfPVP_Info.text = string.Format("{0}/{1}", vSelfSoldierCount, vSelfSoldierCount);
        if (vEnemySoldierCount <= 0)
        {
            vEnemySoldierCount = 0;
            view.Sli_EnemyPVP_ProgressUp.value = 0;
            view.Sli_EnemyPVP_ProgressDown.value = 0;
        }
        else
        {
            view.Sli_EnemyPVP_ProgressUp.value = 1;
            view.Sli_EnemyPVP_ProgressDown.value = 1;
        }
        view.Lbl_EnemyPVP_Info.text = string.Format("{0}/{1}", vEnemySoldierCount, vEnemySoldierCount);
        view.UIWidget_Main_PVPInfo.gameObject.SetActive(true);
    }

    /// <summary>
    /// 刷新己方士兵信息
    /// </summary>
    private void RefreshSelfSoldier()
    {
        view.Sli_SelfPVP_ProgressDown.value = view.Sli_SelfPVP_ProgressUp.value;
        isChangeSelfSoldier = false;
    }

    /// <summary>
    /// 刷新敌方士兵信息
    /// </summary>
    private void RefreshEnemySoldier()
    {
        view.Sli_EnemyPVP_ProgressDown.value = view.Sli_EnemyPVP_ProgressUp.value;
        isChangeEnemySoldier = false;
    }

    /// <summary>
    /// 初始化城堡血量显示信息
    /// </summary>
    private void InitMainStageInfo()
    {
        view.UIWidget_Main_StageInfo.gameObject.SetActive(false);
        view.Obj_StageInfo_Enemy.gameObject.SetActive(false);
        view.Spt_StageInfo_VS.gameObject.SetActive(false);
        view.Obj_StageInfo_Self.gameObject.SetActive(false);

        view.Sli_Self_Progress.value = 1;
        view.Sli_Enemy_Progress.value = 1;
        view.Lbl_Self_Info.text = string.Empty;
        view.Lbl_Enemy_Info.text = string.Empty;
    }

    /// <summary>
    /// 刷新城堡血量显示信息
    /// </summary>
    private void RefreshMainStageInfo()
    {
        int tmpCurValue_Self = 0;
        int tmpCurValue_Enemy = 0;
        max_StageValue_Self = 1;
        max_StageValue_Enemy = 1;
        string tmpIcon_Self = GlobalConst.SpriteName.SPRITE_FIGHTVIEW_CASTLE;
        string tmpIcon_Enemy = GlobalConst.SpriteName.SPRITE_FIGHTVIEW_CASTLE;

        if (fightType == EFightType.eftExpedition)
        {
            view.Obj_StageInfo_Self.gameObject.SetActive(true);
            view.Spt_StageInfo_VS.gameObject.SetActive(false);
            view.Obj_StageInfo_Enemy.gameObject.SetActive(false);

            CastleAttributeInfo tmpInfo = ConfigManager.Instance.mCastleConfig.FindByID(FightRelatedModule.Instance.mCastleInfo.mID);
            if (tmpInfo == null)
                return;
            tmpCurValue_Self = FightRelatedModule.Instance.mCastleInfo.mAttribute.HP;
            max_StageValue_Self = tmpInfo.hp_max + tmpInfo.hp_grow * (FightRelatedModule.Instance.mCastleInfo.mLevel - tmpInfo.level);
        }
        else if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
        {
            view.Obj_StageInfo_Self.gameObject.SetActive(true);
            if (fightStageInfo == null)
                return;
            EPVESceneType tmpFireType = (EPVESceneType)fightStageInfo.FireType;
            if ((tmpFireType != EPVESceneType.epvestAttack) && (tmpFireType != EPVESceneType.epvestDefen) && (tmpFireType != EPVESceneType.epvestEscort) && (tmpFireType != EPVESceneType.epvestTransfer))
                return;

            view.Spt_StageInfo_VS.gameObject.SetActive(true);
            view.Obj_StageInfo_Enemy.gameObject.SetActive(true);

            if (tmpFireType == EPVESceneType.epvestAttack)
            {
                tmpCurValue_Self = PlayerData.Instance.mCastleInfo.mAttribute.HP + fightStageInfo.OwnBaseHP;
                max_StageValue_Self = PlayerData.Instance.mCastleInfo.mAttribute.HP + fightStageInfo.OwnBaseHP;
            }
            else if (tmpFireType == EPVESceneType.epvestDefen)
            {
                tmpCurValue_Self = PlayerData.Instance.mCastleInfo.mAttribute.HP + fightStageInfo.OwnBaseHP;
                max_StageValue_Self = PlayerData.Instance.mCastleInfo.mAttribute.HP + fightStageInfo.OwnBaseHP;
                CommandEvent_ReSetFightStarValue(100);
            }
            else if (tmpFireType == EPVESceneType.epvestEscort)
            {
                view.Spt_StageInfo_VS.gameObject.SetActive(false);
                view.Obj_StageInfo_Self.gameObject.SetActive(false);
            }
            else
            {
                tmpCurValue_Self = fightStageInfo.MaxEnemyCount;
                max_StageValue_Self = fightStageInfo.MaxEnemyCount;
                tmpIcon_Self = GlobalConst.SpriteName.SPRITE_FIGHTVIEW_PROTL;
                CommandEvent_ReSetFightStarValue(0);
            }

            tmpCurValue_Enemy = fightStageInfo.EnemyBaseHP;
            max_StageValue_Enemy = fightStageInfo.EnemyBaseHP;
            if (max_StageValue_Enemy <= 0)
                max_StageValue_Enemy = 1;
            view.Sli_Enemy_Progress.value = ((float)tmpCurValue_Enemy / max_StageValue_Enemy);
            view.Lbl_Enemy_Info.text = string.Format("{0}/{1}", tmpCurValue_Enemy, max_StageValue_Enemy);
            CommonFunction.SetSpriteName(view.Spt_Enemy_Icon, tmpIcon_Enemy);
            view.Spt_Enemy_Icon.MakePixelPerfect();

            if ((fightType == EFightType.eftActivity) && (tmpFireType != EPVESceneType.epvestAttack))
            {
                view.Spt_StageInfo_VS.gameObject.SetActive(false);
                view.Obj_StageInfo_Enemy.gameObject.SetActive(false);
            }
        }

        if (max_StageValue_Self <= 0)
            max_StageValue_Self = 1;
        view.Sli_Self_Progress.value = ((float)tmpCurValue_Self / max_StageValue_Self);
        view.Lbl_Self_Info.text = string.Format("{0}/{1}", tmpCurValue_Self, max_StageValue_Self);
        CommonFunction.SetSpriteName(view.Spt_Self_Icon, tmpIcon_Self);
        view.Spt_Self_Icon.MakePixelPerfect();
        view.UIWidget_Main_StageInfo.gameObject.SetActive(true);
    }

    /// <summary>
    /// 修改战斗时间
    /// </summary>
    private void FightTimeStatistical()
    {
        if (fightIsPause)
            return;
        //修改时间//
        curValue_FightTime += 1;
        if (bossHintTime > 0)
        {
            if (curValue_FightTime == bossHintTime)
                CommandEvent_BossHint(null);
        }
        ////显示星级//
        //CheckFightStar();
        //显示进度//
        RefreshInfo_FightProgress();
        //显示时间//
        RefreshInfo_FightTime();
        //显示能量//
        curValue_Energy += recovery_Energy;
        RefreshInfo_Energy();
        //显示魔法//
        curValue_Magic += recovery_Magic;
        curValue_EnemyMagic += recovery_EnemyMagic;
        RefreshInfo_Magic();

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightCountdownOperate);

        //设定AI英雄行走延迟//
        if (curValue_FightTime > DELAY_HEARMOVETIME_SELF)
            return;
        if (curValue_FightTime == DELAY_HEARMOVETIME_ENEMY)
        {
            if ((fightType != EFightType.eftExpedition) && (fightType != EFightType.eftPVP) && (fightType != EFightType.eftSlave) && (fightType != EFightType.eftServerHegemony) && (fightType != EFightType.eftQualifying))
                return;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightStartAIHero, EFightCamp.efcEnemy);
        }
        if (curValue_FightTime == DELAY_HEARMOVETIME_SELF)
        {
            if ((fightType != EFightType.eftPVP) && (fightType != EFightType.eftSlave) && (fightType != EFightType.eftServerHegemony) && (fightType != EFightType.eftQualifying))
                return;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightStartAIHero, EFightCamp.efcSelf);
        }
    }

    /// <summary>
    /// 设置界面数据
    /// </summary>
    /// <param name="vFightType"></param>
    /// <param name="vData"></param>
    private void ReSetDataInfo(EFightType vFightType, object vData)
    {
        if (fightViewInfo == null)
            fightViewInfo = new FightViewInfo();

        FightRelatedModule.Instance.SelfHeroInfo = null;
        if ((vFightType != EFightType.eftPVP) && (vFightType != EFightType.eftSlave) && (vFightType != EFightType.eftServerHegemony) && (vFightType != EFightType.eftQualifying))
        {
            fightViewInfo.ReSetInfo(vFightType, vData);
        }
        else
        {
            float tmpLimitTime = 0;
            int tmpInitMagic = 0;
            List<string> tmpBackGround = new List<string>();
            string tmpLimitTimeValue = string.Empty;
            string tmpInitMagicValue = string.Empty;
            string tmpBackGroundValue = string.Empty;
            if (vFightType == EFightType.eftPVP)
            {
                tmpLimitTimeValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_LIMITTIME_PVP);
                tmpInitMagicValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_INITMAGIC_PVP);
                tmpBackGroundValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_BACKGROUND_PVP);
            }
            else if (vFightType == EFightType.eftSlave)
            {
                tmpLimitTimeValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_LIMITTIME_SLAVE);
                tmpInitMagicValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_INITMAGIC_SLAVE);
                tmpBackGroundValue = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHT_BACKGROUND_SLAVE);
            }
            else if (vFightType == EFightType.eftServerHegemony)
            {
                tmpLimitTimeValue = ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().fight_time.ToString();
                tmpInitMagicValue = ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().init_magic.ToString();
                tmpBackGroundValue = ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().fight_backpic;
            }
            else if (vFightType == EFightType.eftQualifying)
            {
                tmpLimitTimeValue = ConfigManager.Instance.mQualifyingAwardConfig.GetQualifyingAwardData().limittime.ToString();
                tmpInitMagicValue = ConfigManager.Instance.mQualifyingAwardConfig.GetQualifyingAwardData().initmagic.ToString();
                tmpBackGroundValue = ConfigManager.Instance.mQualifyingAwardConfig.GetQualifyingAwardData().background;
            }
            if (!float.TryParse(tmpLimitTimeValue, out tmpLimitTime))
                tmpLimitTime = 0;
            if (!int.TryParse(tmpInitMagicValue, out tmpInitMagic))
                tmpInitMagic = 0;
            if (!string.IsNullOrEmpty(tmpBackGroundValue))
            {
                string[] tmpArrInfo = tmpBackGroundValue.Split(',');
                for (int i = 0; i < tmpArrInfo.Length; i++)
                {
                    if (string.IsNullOrEmpty(tmpArrInfo[i]))
                        continue;
                    tmpBackGround.Add(tmpArrInfo[i]);
                }
            }
            if (tmpBackGround.Count > 0)
                fightViewInfo.ReSetInfo(vFightType, 0, 0, 0, 0, 0, tmpInitMagic, tmpBackGround[UnityEngine.Random.Range(0, tmpBackGround.Count - 1)], tmpLimitTime, 0, null, 0, 0, 0, true, 0);
            else
                fightViewInfo.ReSetInfo(vFightType, 0, 0, 0, 0, 0, tmpInitMagic, "", tmpLimitTime, 0, null, 0, 0, 0, true, 0);
        }
    }

    /// <summary>
    /// 刷新战斗积分
    /// </summary>
    /// <param name="vScore"></param>
    private void RefreshInfo_PlayerScore(bool vIsShow = true)
    {
        view.Lbl_Fight_Score.text = string.Format(ConstString.FIGHTVIEW_SCORE, (int)curValue_Score);
        view.Lbl_Fight_Score.gameObject.SetActive(vIsShow);
    }

    /// <summary>
    /// 刷新战斗进度
    /// </summary>
    private void RefreshInfo_FightProgress()
    {
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftNewGuide) ||
            (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
            return;
        if (maxValue_FightTime == -1)
            return;
        if (maxValue_FightTime == 0)
        {
            view.UIProgressBar_Progress_Bar.value = 0;
            return;
        }
        float tmpValue = (float)curValue_FightTime / maxValue_FightTime;
        if (tmpValue < 0)
            tmpValue = 0;
        else if (tmpValue >= 1)
        {
            tmpValue = 1;
        }
        view.UIProgressBar_Progress_Bar.value = tmpValue;
    }

    /// <summary>
    /// 刷新战斗节点
    /// </summary>
    private void RefreshInfo_FightNodeList(List<int> vNodeInfoList)
    {
        view.Spt_Progress_Mark.gameObject.SetActive(false);
        //清空界面显示//
        for (int i = 0; i < listFightNode.Count; i++)
            GameObject.Destroy(listFightNode[i].gameObject);
        listFightNode.Clear();

        if ((vNodeInfoList == null) || (vNodeInfoList.Count <= 0))
            return;
        for (int i = 0; i < vNodeInfoList.Count; i++)
        {
            if (vNodeInfoList[i] <= 0)
                continue;
            GameObject tmpMark = CommonFunction.InstantiateObject(view.Spt_Progress_Mark.gameObject, view.Spt_Progress_Mark.transform.parent);
            if (tmpMark == null)
                continue;
            tmpMark.transform.localPosition = new Vector3(375 * vNodeInfoList[i] / 100 - 220, -22, 0);
            tmpMark.name = string.Format("{0}_{1}", view.Spt_Progress_Mark.name, i);
            tmpMark.gameObject.SetActive(true);
            listFightNode.Add(tmpMark.transform);
        }
    }

    /// <summary>
    /// 刷新战斗时间
    /// </summary>
    private void RefreshInfo_FightTime()
    {
        if (fightType == EFightType.eftNewGuide)
            return;
        int tmpLastTime = maxValue_FightTime - curValue_FightTime;
        if (tmpLastTime < 0)
        {
            tmpLastTime = 0;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsOutTime));
        }
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
            view.Lbl_PVPInfo_Text.text = CommonFunction.GetTimeStringNoHours(tmpLastTime);
        else
            view.Lbl_Time_Content.text = CommonFunction.GetTimeStringNoHours(tmpLastTime);
    }

    /// <summary>
    /// 刷新能量
    /// </summary>
    private void RefreshInfo_Energy()
    {
        if (maxValue_Energy == 0)
        {
            curValue_Energy = 0;
            maxValue_Energy = 1;
        }
        if (curValue_Energy > maxValue_Energy)
            curValue_Energy = maxValue_Energy;
        else if (curValue_Energy < 0)
            curValue_Energy = 0;

        if ((fightType != EFightType.eftPVP) && (fightType != EFightType.eftSlave) && (fightType != EFightType.eftServerHegemony) && (fightType != EFightType.eftQualifying))
        {
            if (curValue_Energy == maxValue_Energy)
                view.Obj_Energy_Effect.gameObject.SetActive(true);
            else
                view.Obj_Energy_Effect.gameObject.SetActive(false);
        }
        if ((fightType == EFightType.eftExpedition) || (fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftCrossServerWar) ||
            (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
            return;
        FightViewPanel_PVE.Instance.Refresh_Energy();

        if (curValue_Energy == 30)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingFoodNum30);
        }
        if (curValue_Energy == 75)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingFoodNum75);
        }

    }

    /// <summary>
    /// 刷新魔法
    /// </summary>
    private void RefreshInfo_Magic()
    {
        if (maxValue_Magic == 0)
        {
            curValue_Magic = 0;
            maxValue_Magic = 1;
        }
        if (curValue_Magic > maxValue_Magic)
            curValue_Magic = maxValue_Magic;
        else if (curValue_Magic < 0)
            curValue_Magic = 0;

        if (maxValue_EnemyMagic == 0)
        {
            curValue_EnemyMagic = 0;
            maxValue_EnemyMagic = 1;
        }
        if (curValue_EnemyMagic > maxValue_EnemyMagic)
            curValue_EnemyMagic = maxValue_EnemyMagic;
        else if (curValue_EnemyMagic < 0)
            curValue_EnemyMagic = 0;

        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) ||
            (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            FightViewPanel_PVP.Instance.Refresh_Magic();
        }
        else
        {
            if (curValue_Magic == maxValue_Magic)
            {
                view.Obj_Magic_Effect.gameObject.SetActive(true);
                view.Obj_Magic_FireEffect.gameObject.SetActive(true);
            }
            else
            {
                view.Obj_Magic_Effect.gameObject.SetActive(false);
                view.Obj_Magic_FireEffect.gameObject.SetActive(false);
            }
            FightViewPanel_PVE.Instance.Refresh_Magic();
        }

        if (curValue_Magic == 90)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingMagicNum90);
        }
    }

    /// <summary>
    /// 检测是否在新手引导中
    /// </summary>
    /// <returns></returns>
    private bool CheckIsStillNewGuide()
    {
        if (GlobalConst.IS_OPEN_GUIDE)
        {
            if (fightViewInfo != null)
            {
                if (FightRelatedModule.Instance.DicSpeTutorial != null)
                {
                    foreach (KeyValuePair<uint, uint> tmpInfo in FightRelatedModule.Instance.DicSpeTutorial)
                    {
                        if (tmpInfo.Key == fightViewInfo.curID)
                        {
                            if (!GuideManager.Instance.IsGuideFinish(tmpInfo.Value))
                            {
                                return false;
                            }
                        }
                    }
                }
            }
        }
        return true;
    }

    /// <summary>
    /// 重新开始战斗操作
    /// </summary>
    private void ReStartOperate()
    {
        if (fightViewInfo == null)
            return;
        //检测体力是否足够//
        if (!PlayerData.Instance.IsEnoughSP(fightViewInfo.physical))
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughphysical);
            return;
        }

        if (fightType == EFightType.eftExpedition)
            FightRelatedModule.Instance.SendStartExpedition(serverInfo, true);
        else
        {
            List<ulong> tmpList = new List<ulong>();
            if (listSoldierInfo != null)
            {
                for (int i = 0; i < listSoldierInfo.Count; i++)
                {
                    tmpList.Add(listSoldierInfo[i].mSoldier.uId);
                }
            }
            FightRelatedModule.Instance.SendDungeonStart(fightViewInfo.curID, tmpList, true);
        }
    }

    /// <summary>
    /// 延迟设置加快结算演示
    /// </summary>
    private void DelayOpenQuickOperate()
    {
        isQuickResult = true;
    }

    /// <summary>
    /// 设置自动战斗状态数据
    /// </summary>
    /// <param name="vMove"></param>
    /// <param name="vSkill"></param>
    /// <param name="vSummon"></param>
    private void SetAutoStatus(bool vMove, bool vSkill, bool vSummon)
    {
        isAutoMove = vMove;
        isAutoSkill = vSkill;
        isAutoSummon = vSummon;
    }
    
    /// <summary>
    /// 重新设置界面操作
    /// </summary>
    private void ReSetUIStatusOperate()
    {
        fightStartIsOver = false;
        heroMoveStatus = false;
        InitShowView();

        maxValue_FightTime = (int)fightViewInfo.timeLimit;
        curValue_FightTime = 0;
        curValue_Magic = fightViewInfo.initMagic;
        curValue_Energy = fightViewInfo.initEnergy;
        isStopEndless = false;
        pvpSoldierCount_Self = 0;
        pvpSoldierCount_Enemy = 0;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, fightSpeed);

        view.Spt_MoveHint_Left.gameObject.SetActive(true);
        view.Spt_MoveHint_Right.gameObject.SetActive(true);
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            view.Spt_MoveHint_Left.gameObject.SetActive(false);
            view.Spt_MoveHint_Right.gameObject.SetActive(false);
        }

        view.Btn_Main_Auto.GetComponent<BoxCollider>().enabled = true;
        view.Btn_Main_AutoSummon.GetComponent<BoxCollider>().enabled = true;
        autoFightStatus = fightViewInfo.autoStatus;
        SetAutoStatus(false, false, false);
        view.Spt_BtnMain_AutoLock.gameObject.SetActive(false);
        view.Spt_BtnMain_AutoSummonLock.gameObject.SetActive(false);
        view.Spt_BtnMain_LockQuicken.gameObject.SetActive(false);
        if ((fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            view.Spt_BtnMain_LockQuicken.gameObject.SetActive(true);
        }
        if (autoFightStatus == (int)EAutoFightStatus.eafsLock)
        {
            if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_COMMON);
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_COMMON);
                view.Trans_BtnMain_AutoEffect.gameObject.SetActive(true);
                view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(true);
            }
            else
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_LOCK);
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_LOCK);
                view.Trans_BtnMain_AutoEffect.gameObject.SetActive(false);
                view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(false);
            }
            view.Spt_BtnMain_AutoLock.gameObject.SetActive(true);
            view.Spt_BtnMain_AutoSummonLock.gameObject.SetActive(true);
        }
        else
        {
            if (autoFightStatus == (int)EAutoFightStatus.eafsNonActive)
            {
                if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
                {
                    SetAutoStatus(SceneManager.Instance.IsAutoMove, SceneManager.Instance.IsAutoSkill, SceneManager.Instance.IsAutoSummon);
                }
            }
            else if (autoFightStatus == (int)EAutoFightStatus.eafsActive)
            {
                if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
                {
                    SetAutoStatus(SceneManager.Instance.IsAutoMove, SceneManager.Instance.IsAutoSkill, SceneManager.Instance.IsAutoSummon);
                }
            }
            else if (autoFightStatus == (int)EAutoFightStatus.eafsActive_Fight)
            {
                SetAutoStatus(true, true, false);
            }
            else if (autoFightStatus == (int)EAutoFightStatus.eafsActive_Summon)
            {
                SetAutoStatus(false, false, true);
            }

            if ((isAutoMove) && (isAutoSkill))
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_COMMON);
                view.Trans_BtnMain_AutoEffect.gameObject.SetActive(true);
            }
            else
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoBG, GlobalConst.SpriteName.AUTO_FIGHT_BG_LOCK);
                view.Trans_BtnMain_AutoEffect.gameObject.SetActive(false);
            }
            if (isAutoSummon)
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_COMMON);
                view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(true);
            }
            else
            {
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_LOCK);
                view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(false);
            }

            if ((fightType == EFightType.eftExpedition) || (fightType == EFightType.eftUnion) || (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftCrossServerWar))
            {
                isAutoSummon = false;
                CommonFunction.SetSpriteName(view.Spt_BtnMain_AutoSummonBG, GlobalConst.SpriteName.AUTO_SUMMON_BG_LOCK);
                view.Trans_BtnMain_AutoSummonEffect.gameObject.SetActive(false);
                view.Spt_BtnMain_AutoSummonLock.gameObject.SetActive(true);
            }
        }
        view.Btn_Main_Auto.gameObject.SetActive(true);
        view.Btn_Main_AutoSummon.gameObject.SetActive(true);

        view.Obj_PlayerInfo_Fight.gameObject.SetActive(false);
        view.Obj_Main_Progress.localPosition = Vector3.zero;
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            if (fightPlayer_Self != null)
            {
                maxValue_Magic = fightPlayer_Self.mAttribute.MP;
                recovery_Magic = fightPlayer_Self.mAttribute.MPRecovery;
                if (fightPlayer_Self.mSoldierList != null)
                {
                    for (int i = 0; i < fightPlayer_Self.mSoldierList.Count; i++)
                        pvpSoldierCount_Self += fightPlayer_Self.mSoldierList[i].mNum;
                }
            }
            if (fightPlayer_Enemy != null)
            {
                curValue_EnemyMagic = fightViewInfo.initMagic;
                maxValue_EnemyMagic = fightPlayer_Enemy.mAttribute.MP;
                recovery_EnemyMagic = fightPlayer_Enemy.mAttribute.MPRecovery;
                if (fightPlayer_Enemy.mSoldierList != null)
                {
                    for (int i = 0; i < fightPlayer_Enemy.mSoldierList.Count; i++)
                        pvpSoldierCount_Enemy += fightPlayer_Enemy.mSoldierList[i].mNum;
                }
            }

            RefreshMainPVPInfo(pvpSoldierCount_Self, pvpSoldierCount_Enemy);
            view.Obj_Main_Progress.gameObject.SetActive(false);

            view.Btn_Main_Pause.gameObject.SetActive(false);
            view.Btn_Main_Quicken.gameObject.transform.localPosition = new Vector3(-60, -45, 0);
        }
        else
        {
            ShowInfoHero tmpHeroAtt = new ShowInfoHero();
            if ((fightType == EFightType.eftUnion) || (fightType == EFightType.eftCaptureTerritory))
            {
                tmpHeroAtt.CopyTo(FightRelatedModule.Instance.SelfHeroInfo);
            }
            else
            {
                tmpHeroAtt.CopyTo(PlayerData.Instance._Attribute);
            }
            view.Obj_Main_Progress.gameObject.SetActive(true);
            maxValue_Energy = tmpHeroAtt.Energy;
            maxValue_Magic = tmpHeroAtt.MP;
            if (fightType != EFightType.eftNewGuide)
            {
                recovery_Energy = tmpHeroAtt.EnergyRecovery;
                recovery_Magic = tmpHeroAtt.MPRecovery;
            }
            else
            {
                recovery_Energy = 0;
                recovery_Magic = 0;
            }

            if ((fightType == EFightType.eftEndless) || (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftCrossServerWar))
            {
                RefreshInfo_PlayerScore();
                view.Obj_PlayerInfo_Fight.gameObject.SetActive(true);
                view.Obj_Main_Progress.localPosition = new Vector3(0, 30, 0);
            }
            else if (fightType == EFightType.eftNewGuide)
            {
                view.Obj_Main_Progress.gameObject.SetActive(false);
            }
            else if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity) || (fightType == EFightType.eftEndless))
            {
                bossHintTime = fightStageInfo.BossHintTime;
            }
            if (fightType == EFightType.eftUnion)
            {
                view.Btn_Main_Pause.gameObject.SetActive(false);
                view.Btn_Main_Quicken.gameObject.transform.localPosition = new Vector3(-60, -45, 0);
            }
            else
            {
                view.Btn_Main_Pause.gameObject.SetActive(true);
                view.Btn_Main_Quicken.gameObject.transform.localPosition = new Vector3(-154, -45, 0);
            }
        }

        //创建场景//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_SceneCreate, new CreateSceneInfo(fightViewInfo.fightType, fightViewInfo.curID, fightViewInfo.scenBackGround));

        //设置数据//
        Scheduler.Instance.RemoveTimer(FightTimeStatistical);
        Scheduler.Instance.AddTimer(1 / fightSpeed, true, FightTimeStatistical);
        this.LoadingObject();
        RefreshInfo_FightNodeList(fightViewInfo.timeNodes);
        RefreshInfo_FightProgress();
        RefreshInfo_FightTime();
        RefreshInfo_Energy();
        RefreshInfo_Magic();
        RefreshInfo_PetPower();
        RefreshMainStageInfo();

        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) || (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            FightViewPanel_PVP.Instance.RefreshPanel(fightType, fightPlayer_Self, fightPlayer_Enemy);
        }
        else if ((fightType == EFightType.eftUnion) || (fightType == EFightType.eftCaptureTerritory) || fightType == EFightType.eftCrossServerWar)
        {
            List<Weapon> tmpList = new List<Weapon>();
            if (dicTemporaryEquipInfo != null)
            {
                foreach (KeyValuePair<ulong, int> tmpInfo in dicTemporaryEquipInfo)
                {
                    Weapon tmpWeapon = PlayerData.Instance._WeaponDepot.FindByUid(tmpInfo.Key);
                    if (tmpWeapon == null)
                        continue;
                    if (tmpWeapon.Att == null)
                        continue;
                    if (tmpWeapon.Att.type != 0)
                        continue;
                    tmpList.Add(tmpWeapon);
                }
            }
            FightViewPanel_PVE.Instance.RefreshPanel(fightType, tmpList, listSoldierInfo);
        }
        else
        {
            List<Weapon> tmpList = PlayerData.Instance._ArtifactedDepot._EquiptList;
            if (fightType == EFightType.eftNewGuide)
            {
                if (!isSecondNewGuide)
                    FightViewPanel_PVE.Instance.RefreshPanel(fightType, tmpList, null);
                else
                    FightViewPanel_PVE.Instance.RefreshPanel(fightType, tmpList, listSoldierInfo);
            }
            else
                FightViewPanel_PVE.Instance.RefreshPanel(fightType, tmpList, listSoldierInfo);
        }
    }

    /// <summary>
    /// 退出操作
    /// </summary>
    /// <param name="vIsFailUpdate">是否为失败提升接口[true-是;false-不是]</param>
    private void ExitOperate(bool vIsFailUpdate = false)
    {
        Scheduler.Instance.RemoveTimer(DelayToSupermacy);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        Main.Instance.StateMachine.ChangeState(MainCityState.StateName);

        if (!vIsFailUpdate)
        {
            switch (fightType)
            {
                case EFightType.eftMain:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
                        UISystem.Instance.GateView.UpdateViewInfo(fightStageInfo.ID);
                    }
                    break;
                case EFightType.eftActivity:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ACTIVITIES);
                    }
                    break;
                case EFightType.eftEndless:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ENDLESS);
                    }
                    break;
                case EFightType.eftExpedition:
                    {
                        FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.None);
                    }
                    break;
                case EFightType.eftPVP:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PVPVIEW);
                    }
                    break;
                case EFightType.eftSlave:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONVIEW);
                    }
                    break;
                case EFightType.eftUnion:
                    {
                        UnionModule.Instance.OpenUnionPve();
                    }
                    break;
                case EFightType.eftNewGuide:
                    {
                        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenMainCity);
                    }
                    break;
                case EFightType.eftCaptureTerritory:
                    {
                        CaptureTerritoryModule.Instance.OpenCaptureTerritory();
                    }
                    break;
                case EFightType.eftServerHegemony:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SUPERMACY);
                    }
                    break;
                case EFightType.eftQualifying:
                    {
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_QUALIFYING);
                    }
                    break;
                case EFightType.eftCrossServerWar:
                    {
                        CrossServerWarModule.Instance.ReturnToBattleField();
                    }
                    break;
                default:
                    break;
            }
        }
        else
        { }
    }

    /// <summary>
    /// 预加载特效资源
    /// </summary>
    private void LoadingObject()
    {
        EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.BOSS_APPEAR_A, (go) =>
        {
            EffectObjectCache.Instance.FreeObject(go);
        });
        GameObject tmpPrefabNormal = ResourceLoadManager.Instance.LoadView("HP_NormalHurt");
        if (tmpPrefabNormal != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabNormal, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_NormalHurt", 10);
        }
        tmpPrefabNormal = ResourceLoadManager.Instance.LoadView("HP_AddBlood");
        if (tmpPrefabNormal != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabNormal, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_AddBlood", 10);
        }
        GameObject tmpPrefabBuffSelf = ResourceLoadManager.Instance.LoadView("HP_BuffSelf");
        if (tmpPrefabBuffSelf != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabBuffSelf, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_BuffSelf", 5);
        }
        GameObject tmpPrefabBuffEnemy = ResourceLoadManager.Instance.LoadView("HP_BuffEnemy");
        if (tmpPrefabBuffEnemy != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabBuffEnemy, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_BuffEnemy", 5);
        }
        GameObject tmpPrefabHP_Buff = ResourceLoadManager.Instance.LoadView("HP_Buff");
        if (tmpPrefabHP_Buff != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabHP_Buff, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_Buff", 5);
        }
        GameObject tmpPrefabHP_DeBuff = ResourceLoadManager.Instance.LoadView("HP_DeBuff");
        if (tmpPrefabHP_DeBuff != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabHP_DeBuff, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_DeBuff", 2);
        }
        GameObject tmpPrefabHP_CriteHurt = ResourceLoadManager.Instance.LoadView("HP_CriteHurt");
        if (tmpPrefabHP_CriteHurt != null)
        {
            AloneObjectCache.Instance.LoadGameObject(tmpPrefabHP_CriteHurt, (GO_HP) =>
            {
                AloneObjectCache.Instance.FreeObject(GO_HP);
            }, "HP_CriteHurt", 5);
        }
    }

    /// <summary>
    /// 刷新无尽士兵
    /// </summary>
    private void RefreshSoldier()
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleClear, new ClearRoleInfo(EFightCamp.efcEnemy, ERoleType.ertMonster));

        List<string> tmpName = new List<string>();
        List<uint> tmpListID = new List<uint>();
        if (stageMonsterList_Endless != null)
        {
            for (int i = 0; i < stageMonsterList_Endless.Count; i++)
            {
                if (stageMonsterList_Endless[i] == null)
                    continue;
                for (int j = 0; j < stageMonsterList_Endless[i].ArrMonsterID.Length; j++)
                {
                    uint tmpMonsterID = stageMonsterList_Endless[i].ArrMonsterID[j];
                    if (tmpListID.Contains(tmpMonsterID))
                        continue;
                    tmpListID.Add(tmpMonsterID);
                    MonsterAttributeInfo tmpSingleMonsterData = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(tmpMonsterID);
                    if (tmpSingleMonsterData == null)
                        continue;
                    tmpName.Add(tmpSingleMonsterData.ResourceID);
                }
            }
        }
        stageMonsterList_Endless.Clear();
        if (AloneObjectCache.Instance != null)
            AloneObjectCache.Instance.DeleteByName(tmpName);
    }


    /// <summary>
    /// 检测战斗星级[仅限主线和活动副本]
    /// </summary>
    private void CheckFightStar()
    {
        if (fightType == EFightType.eftNewGuide)
            return;
        if ((fightType != EFightType.eftMain) && (fightType != EFightType.eftActivity))
            return;
        if (fightStageInfo == null)
            return;

        if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestAttack)
        {
            starValue = curValue_FightTime;
        }

        switch ((EPVESceneType)fightStageInfo.FireType)
        {
            case EPVESceneType.epvestAttack:
            case EPVESceneType.epvestTransfer:
                {
                    if (starValue <= fightStageInfo.Star3)
                    {
                        RefreshInfo_FightStar(3);
                    }
                    else if ((starValue > fightStageInfo.Star3) && (starValue <= fightStageInfo.Star2))
                    {
                        RefreshInfo_FightStar(2);
                    }
                    else
                    {
                        RefreshInfo_FightStar(1);
                    }
                    break;
                }
            case EPVESceneType.epvestDefen:
            case EPVESceneType.epvestEscort:
                {
                    if (starValue >= fightStageInfo.Star3)
                    {
                        RefreshInfo_FightStar(3);
                    }
                    else if ((starValue < fightStageInfo.Star3) && (starValue >= fightStageInfo.Star2))
                    {
                        RefreshInfo_FightStar(2);
                    }
                    else
                    {
                        RefreshInfo_FightStar(1);
                    }
                    break;
                }
            default:
                {
                    break;
                }
        }
    }

    /// <summary>
    /// 初始化星级
    /// </summary>
    private void InitInfo_FightStar()
    {
        view.Spt_Fight_Star.gameObject.SetActive(false);
        //删除组件//
        for (int i = 0; i < listFightStar.Count; i++)
            GameObject.Destroy(listFightStar[i].gameObject);
        listFightStar.Clear();
        //创建组件//
        for (int i = 0; i < MAX_STAR_COUNT; i++)
        {
            GameObject tmpStar = CommonFunction.InstantiateObject(view.Spt_Fight_Star.gameObject, view.Spt_Fight_Star.transform.parent);
            if (tmpStar == null)
                continue;
            tmpStar.transform.localPosition = new Vector3(40 * (i - 1), -15, 0);
            tmpStar.name = string.Format(string.Format("{0}_{1}", view.Spt_Fight_Star.name, i));
            tmpStar.gameObject.SetActive(false);
            listFightStar.Add(tmpStar.transform);
        }
    }

    /// <summary>
    /// 刷新战斗星级显示
    /// </summary>
    /// <param name="vStar">星级等级</param>
    private void RefreshInfo_FightStar(int vStar)
    {
        for (int i = 0; i < listFightStar.Count; i++)
        {
            listFightStar[i].gameObject.SetActive(false);
            if (i <= vStar - 1)
                listFightStar[i].gameObject.SetActive(true);
        }
    }

    private void CheckPetData(PetData vPetData = null)
    {
        hasPet_Self = false;
        hasPet_Enemy = false;
        pPetData = vPetData;

        if ((pPetData != null) && (pPetData.Skill != null) && (pPetData.Skill.Att != null))
        {
            hasPet_Self = true;
            pPetData.Skill.CacheEffect();
        }

        if ((fightPlayer_Enemy != null) && (fightPlayer_Enemy.mPetInfo != null) && (fightPlayer_Enemy.mSkill != null) && (fightPlayer_Enemy.mPetInfo.skill.Count > 0))
        {
            if (fightPlayer_Enemy.mPetInfo.skill[0] != null)
                hasPet_Enemy = true;
        }
        GetPetSkillInfo();
    }
    
    private void GetPetSkillInfo()
    {
        petSkillInfo_Self = null;
        petSkillInfo_Enemy = null;
        if (hasPet_Self)
        {
            petSkillInfo_Self = Skill.createByID(pPetData.Skill.Att.nId);
            if (petSkillInfo_Self != null)
            {
                petSkillInfo_Self.Strong(pPetData.Skill.Level);
            }
        }
        if (hasPet_Enemy)
        {
            SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(fightPlayer_Enemy.mPetInfo.skill[0].id);
            if (tmpSkillInfo != null)
            {
                petSkillInfo_Enemy = Skill.createByID(tmpSkillInfo.nId);
                if (petSkillInfo_Enemy != null)
                {
                    petSkillInfo_Enemy.Strong((int)fightPlayer_Enemy.mPetInfo.skill[0].level);
                }
            }
        }
    }

    /// <summary>
    /// 刷新宠物怒气值
    /// </summary>
    private void RefreshInfo_PetPower()
    {
        if ((fightType == EFightType.eftPVP) || (fightType == EFightType.eftSlave) ||
            (fightType == EFightType.eftServerHegemony) || (fightType == EFightType.eftQualifying))
        {
            FightViewPanel_PVP.Instance.Refresh_PetPower();
        }
        else
        {
            FightViewPanel_PVE.Instance.Refresh_PetPower();
        }
    }

    /// <summary>
    /// 新手引导重置操作
    /// </summary>
    private void NewGuideOperate()
    {
        FightRelatedModule.Instance.isOpenNext = false;
        InitShowView();
        Scheduler.Instance.AddUpdator(FrameUpdate);
        Scheduler.Instance.AddTimer(1, true, FightTimeStatistical);
    }
    #endregion

    #region 公有方法
    /// <summary>
    /// 退出操作
    /// </summary>
    public void DelayToSupermacy()
    {
        ExitOperate();
    }

    /// <summary>
    /// 设置战斗暂停状态
    /// </summary>
    /// <param name="vIsPause"></param>
    public void SetFightPauseStatus(bool vIsPause, EFightPauseType vPauseType = EFightPauseType.efptNormal)
    {
        fightIsPause = vIsPause;
        if (fightIsPause)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetPause, vPauseType);
        else
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightSetResume, true);
    }

    /// <summary>
    /// 是否可进行下一关
    /// </summary>
    /// <returns></returns>
    public bool IsOpenNextBtn()
    {
        if (fightViewInfo == null)
            return false;
        if (fightViewInfo.nextID.Equals(0))
            return false;
        return (!PlayerData.Instance.IsPassedGate(fightViewInfo.nextID));
    }

    /// <summary>
    /// 重新设定界面
    /// </summary>
    /// <param name="vIsFirst"></param>
    public void ReSetUIStatus(bool vIsFirst = true)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Music, SoundManager.Instance.Get_MusicVolume);
        if (!vIsFirst)
        {
            UISystem.Instance.HintView.ShowFightLoading(ShowFightTitleInfo);
            FightRelatedModule.Instance.isFightState = false;
            Scheduler.Instance.AddTimer(0.5f, false, ReSetUIStatusOperate);
            return;
        }
        ReSetUIStatusOperate();
    }

    /// <summary>
    /// 显示战斗开场信息
    /// </summary>
    public void ShowFightTitleInfo()
    {
        EFightStartType tmpType = EFightStartType.efstNone;
        switch (fightType)
        {
            case EFightType.eftMain:
            case EFightType.eftActivity:
                {
                    if (fightViewInfo != null)
                    {
                        if (fightStageInfo != null)
                        {
                            switch ((EPVESceneType)fightStageInfo.FireType)
                            {
                                case EPVESceneType.epvestAttack:
                                    {
                                        tmpType = EFightStartType.efstAtt;
                                    }
                                    break;
                                case EPVESceneType.epvestDefen:
                                    {
                                        tmpType = EFightStartType.efstDef;
                                    }
                                    break;
                                case EPVESceneType.epvestEscort:
                                    {
                                        tmpType = EFightStartType.efstEscort;
                                    }
                                    break;
                                case EPVESceneType.epvestTransfer:
                                    {
                                        tmpType = EFightStartType.efstTranfer;
                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
            case EFightType.eftNewGuide:
                {
                    tmpType = EFightStartType.efstNewGuide;
                }
                break;
            case EFightType.eftEndless:
                {
                    tmpType = EFightStartType.efstEndless;
                }
                break;
            case EFightType.eftExpedition:
                {
                    tmpType = EFightStartType.efstExpedition;
                }
                break;
            case EFightType.eftPVP:
            case EFightType.eftSlave:
            case EFightType.eftUnion:
            case EFightType.eftCaptureTerritory:
            case EFightType.eftServerHegemony:
            case EFightType.eftQualifying:
            case EFightType.eftCrossServerWar:
                {
                    tmpType = EFightStartType.efstPVP;
                }
                break;
        }

        int tmpExpeCount = 0;
        if (tmpType == EFightStartType.efstExpedition)
        {
            if (PlayerData.Instance._ExpeditionInfo != null)
            {
                ExpeditionData tmpExpeditionInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(PlayerData.Instance._ExpeditionInfo.cur_gate_id);
                if (tmpExpeditionInfo != null)
                    tmpExpeCount = tmpExpeditionInfo.progress;
            }
        }
        else if (tmpType == EFightStartType.efstEndless)
        {
            tmpExpeCount = fightNum;
        }

        FightViewPanel_Start.Instance.RefreshShowStatus(tmpType, tmpExpeCount);
    }

    /// <summary>
    /// 开始战斗状态
    /// </summary>
    public void StartFightStatus()
    {
        fightIsPause = false;
        fightStartIsOver = true;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_ChangeFightAutoStatus, autoFightStatus);
        foreach (KeyValuePair<int, RoleBase> tmpInfo in RoleManager.Instance.Get_RoleDic)
        {
            tmpInfo.Value.InitAction();
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_StartShowFightStatus);

        if (SceneManager.Instance.Get_CurScene != null)
        {
            petSkillWidthScale_Self = (SceneManager.Instance.Get_CurScene.Get_ActualWidth / 2 - Mathf.Abs(view.Btn_PVE_PetSkill.transform.localPosition.x) + 108) / SceneManager.Instance.Get_CurScene.Get_ActualWidth;
            petSkillWidthScale_Enemy = 1 - petSkillWidthScale_Self;
        }
        //判断是否上阵宠物//
        if (!hasPet_Self)
        {
            petSkillWidthScale_Self = 0;
        }
        if (!hasPet_Enemy)
        {
            petSkillWidthScale_Enemy = 1;
        }

        if (fightStageInfo == null)
            return;

        if (fightType == EFightType.eftNewGuide)
            GuideManager.Instance.CheckTrigger(GuideTrigger.EnterNewGuideFight);
        else
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.EnterFighting);
        }

        if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestAttack)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingAttackCastle);
        }
        else if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestEscort)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingProtectNpc);
        }
        else if ((EPVESceneType)fightStageInfo.FireType == EPVESceneType.epvestTransfer)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.FightingProtectDoor);
        }
    }

    /// <summary>
    /// 重置己方士兵信息
    /// </summary>
    public void ReSetSelfSoldierInfo()
    {
        if ((fightType != EFightType.eftPVP) && (fightType != EFightType.eftSlave) && (fightType != EFightType.eftServerHegemony) && (fightType != EFightType.eftQualifying))
            return;
        curSoldier_Self -= 1;
        if (curSoldier_Self < 0)
            curSoldier_Self = 0;
        view.Sli_SelfPVP_ProgressUp.value = ((float)curSoldier_Self / pvpSoldierCount_Self);
        view.Lbl_SelfPVP_Info.text = string.Format("{0}/{1}", curSoldier_Self, pvpSoldierCount_Self);
        if (Time.time - refreshSelfSoldierTime < 1)
            return;
        refreshSelfSoldierTime = Time.time;
        if (!isChangeSelfSoldier)
        {
            Scheduler.Instance.AddTimer(1, false, RefreshSelfSoldier);
            isChangeSelfSoldier = true;
        }
    }

    /// <summary>
    /// 重置敌方士兵信息
    /// </summary>
    public void ReSetEnemySoldierInfo()
    {
        if ((fightType != EFightType.eftPVP) && (fightType != EFightType.eftSlave) && (fightType != EFightType.eftServerHegemony) && (fightType != EFightType.eftQualifying))
            return;
        curSoldier_Enemy -= 1;
        if (curSoldier_Enemy < 0)
            curSoldier_Enemy = 0;
        view.Sli_EnemyPVP_ProgressUp.value = ((float)curSoldier_Enemy / pvpSoldierCount_Enemy);
        view.Lbl_EnemyPVP_Info.text = string.Format("{0}/{1}", curSoldier_Enemy, pvpSoldierCount_Enemy);
        if (Time.time - refreshEnemySoldierTime < 1)
            return;
        refreshEnemySoldierTime = Time.time;
        if (!isChangeEnemySoldier)
        {
            Scheduler.Instance.AddTimer(1, false, RefreshEnemySoldier);
            isChangeEnemySoldier = true;
        }
    }

    /// <summary>
    /// 无尽模式过场操作
    /// </summary>
    public void EndlessExcessiveOperate()
    {
        if (fightType != EFightType.eftEndless)
            return;
        fightIsPause = true;
        FightViewPanel_Pause.Instance.OpenPanel(fightType, true);
        if (isStopEndless)
            return;
        if (fightViewInfo == null)
            return;
        if (fightViewInfo.nextID == 0)
            return;
        StageInfo tmpNextStage = ConfigManager.Instance.mStageData.GetInfoByID(fightViewInfo.nextID);
        if (tmpNextStage == null)
            return;
        stageMonsterList_Endless = SceneManager.Instance.GetStageMonsterInfo(ConfigManager.Instance.mStageMonsterData.FindByID(fightViewInfo.curID));

        if (EffectObjectCache.Instance != null)
            EffectObjectCache.Instance.Delete(true);
        SkillManage.Instance.RemoveAll();
        RefreshSoldier();
        System.GC.Collect();
        Resources.UnloadUnusedAssets();
        System.GC.Collect();

        FightViewPanel_PVE.Instance.Refresh_EndlessSoldierSkill();
        FightViewPanel_PVE.Instance.Refresh_EndlessHeroSkill();

        EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.BOSS_APPEAR_A, (go) =>
        {
            EffectObjectCache.Instance.FreeObject(go);
        });

        fightStageInfo.CopyTo(tmpNextStage);
        //获取新数据//
        ReSetDataInfo(fightType, tmpNextStage);
        fightNum += 1;
        //刷新进度表数据//
        curValue_FightTime = 0;
        maxValue_FightTime = (int)fightViewInfo.timeLimit;
        //通知角色离开场景//
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightEndlessExcessive);
        ShowFightTitleInfo();
    }

    /// <summary>
    /// 无尽模式过场结束
    /// </summary>
    public void EndlessExcessiveEnd()
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_ChangeVolume_Music, SoundManager.Instance.Get_MusicVolume);
        RefreshInfo_FightProgress();
        fightStartIsOver = false;
        fightIsFinished = false;
        //通知场景刷新//
        if ((fightViewInfo != null) && (!isStopEndless))
        {
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_SceneReSet, new CreateSceneInfo(fightViewInfo.fightType, fightViewInfo.curID, fightViewInfo.scenBackGround));
        }
        fightIsPause = false;
        FightViewPanel_Pause.Instance.ClosePanel();
    }

    /// <summary>
    /// 战斗结算
    /// </summary>
    public void RefreshInfo_ResultOfBattle(object vResultData)
    {
        if (vResultData == null)
            return;
        if (!CommonFunction.CheckFightType(fightType))
            return;

        fightIsPause = true;
        if ((fightType == EFightType.eftMain) || (fightType == EFightType.eftActivity))
        {
            if (fightViewInfo == null)
                return;
            FightViewPanel_Result.Instance.Refresh_Activity(vResultData, fightType, fightViewInfo.curID);
        }
        else if (fightType == EFightType.eftNewGuide)
        {
            FightViewPanel_Result.Instance.Refresh_NewGuide(vResultData, fightViewInfo.curID);
        }
        else if (fightType == EFightType.eftEndless)
        {
            if (fightViewInfo == null)
                return;
            FightViewPanel_Result.Instance.Refresh_Endless((EndlessDungeonRewardResp)vResultData, fightViewInfo.chapterID, fightNum);
        }
        else if (fightType == EFightType.eftExpedition)
        {
            FightViewPanel_Result.Instance.Refresh_Expedition(vResultData);
        }
        else if (fightType == EFightType.eftPVP)
        {
            FightViewPanel_Result.Instance.Refresh_PVP(vResultData);
        }
        else if (fightType == EFightType.eftSlave)
        {
            FightViewPanel_Result.Instance.Refresh_Slave(vResultData);
        }
        else if (fightType == EFightType.eftUnion)
        {
            FightViewPanel_Result.Instance.Refresh_Union(vResultData);
        }
        else if (fightType == EFightType.eftCaptureTerritory)
        {
            FightViewPanel_Result.Instance.Refresh_CaptureTerritory(vResultData);
        }
        else if (fightType == EFightType.eftServerHegemony)
        {
            FightViewPanel_Result.Instance.Refresh_ServerHegemony(vResultData);
        }
        else if (fightType == EFightType.eftQualifying)
        {
            FightViewPanel_Result.Instance.Refresh_Qualifying(vResultData);
        }
        else if (fightType == EFightType.eftCrossServerWar)
        {
            FightViewPanel_Result.Instance.Refresh_CrossServerWar(vResultData);
        }
        Scheduler.Instance.AddTimer(0.5f, false, FightViewPanel_Pause.Instance.ClosePanel);
        Scheduler.Instance.AddTimer(0.5f, false, DelayOpenQuickOperate);
    }

    /// <summary>
    /// 返回战斗结算计算时间
    /// </summary>
    /// <returns></returns>
    public int GetFightDisTime()
    {
        return curValue_FightTime;
    }
    #endregion

    #region 英雄移动方法
    /// <summary>
    /// 用于检测输入[需要修改]
    /// </summary>
    private void FrameUpdate()
    {
        if (SceneManager.Instance.Get_CurScene == null || SceneManager.Instance.Get_CurScene.sceneCamera == null)
            return;
        if (fightIsPause)
            return;

#if UNITY_EDITOR
        if (Input.GetMouseButton(0))
        {
            if (CheckToucheStatus(Input.mousePosition))
                return;
        }
#elif UNITY_ANDROID || UNITY_IPHONE
        if (Input.touchCount == 1)
        {
            if (CheckToucheStatus(Input.touches[0].position))
                return;
        }
        else if (Input.touchCount > 1)
        {
            for (int i = 0; i < Input.touchCount; i++)
            {
                if (CheckToucheStatus(Input.touches[i].position))
                    return;
            }
        }
#endif

        if (!heroMoveStatus)
            return;

        heroMoveStatus = false;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, ERoleAction.eraAIIdle);
    }

    /// <summary>
    /// 检测触碰状态
    /// </summary>
    /// <param name="vIndex">触碰点索引</param>
    /// <returns>true-开启移动 false-关闭移动</returns>
    private bool CheckToucheStatus(Vector3 vPos)
    {
        Ray ray = SceneManager.Instance.Get_CurScene.sceneCamera.ScreenPointToRay(vPos);
        RaycastHit hit;
        LayerMask tmpNGUIMask = LayerMaskTool.GetLayerMask(LayerMaskEnum.NGUI);
        if (Physics.Raycast(ray, out hit, 100, tmpNGUIMask.value))
        {
            return false;
        }
        LayerMask mask = LayerMaskTool.GetLayerMask(LayerMaskEnum.UI);
        if (!Physics.Raycast(ray, out hit, 100, mask.value))
            return false;
        //if ((vPos.y <= Screen.height * 0.2) || (vPos.y >= Screen.height * 0.8))
        //    return false;
        //if ((vPos.y <= Screen.height * 0.46) && ((vPos.x <= Screen.width * petSkillWidthScale_Self) || (vPos.x >= Screen.width * petSkillWidthScale_Enemy)))
        //    return false;

        heroMoveStatus = true;
        if ((vPos.x <= Screen.width / 2))
        {
            if (heroMoveType == EHeroMoveType.ehmtOnlyRight)
                return false;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, ERoleDirection.erdLeft);
        }
        else if (vPos.x > Screen.width / 2)
        {
            if (heroMoveType == EHeroMoveType.ehmtOnlyLeft)
                return false;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, ERoleDirection.erdRight);
        }
        return true;
    }
    #endregion

}