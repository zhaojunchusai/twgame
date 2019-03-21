using UnityEngine;
using System;
using System.Collections;

public class QualifyingView
{
    public static string UIName = "QualifyingView";
    public GameObject _uiRoot;
    public GameObject Gobj_RulePanel;
    public UIButton Btn_CloseRule;
    public UIScrollView ScrollView_RuleDesc;
    public UILabel Lbl_RuleDesc;
    public UISprite Spt_RuleMask;


    public GameObject Gobj_Main;
    public UIButton Btn_RuleDesc;
    public UIButton Btn_Rank;
    public UIButton Btn_BattleLog;
    public UIButton Btn_StartMacth;
    public UIGrid Grd_DefenseSoldierGrid;
    public GameObject Gobj_DefenseSoldierComp;
    public UILabel Lbl_CombatPower;
    public UIButton Btn_AdjustLineup;
    public UIButton Btn_Exit;
    public UILabel Lbl_RankTitle;
    public UISprite Spt_RankIcon;
    public UILabel Lbl_RankNum;
    public UILabel Lbl_RankIntegral;
    public UILabel Lbl_PlaySessions;
    public UILabel Lbl_WinsSessions;
    public UILabel Lbl_SeasonTimeDescTitle;
    public UILabel Lbl_SeasonTimeDesc;
    public GameObject Gobj_EveryDayAwardGroup;
    public UISprite Spt_DayAwardIcon;
    public UISprite Spt_DayAwardTitle;
    public UILabel Lbl_DayBattleTipTitle;
    public UILabel Lbl_DayBattleTipNum;
    public GameObject Gobj_EveryWeekAwardGroup;
    public UISprite Spt_WeekAwardIcon;
    public UISprite Spt_WeekAwardTitle;
    public UILabel Lbl_WeekBattleTipTitle;
    public UILabel Lbl_WeekBattleTipNum;
    public GameObject Gobj_EverySeasonAwardGroup;
    public UISprite Spt_SeasonAwardIcon;
    public UISprite Spt_SeasonAwardTitle;
    public UILabel Lbl_SeasonBattleTipTitle;
    public UILabel Lbl_SeasonBattleTipNum;
    public GameObject Gobj_RightTopGroup;
    public UILabel Lbl_ChanglleTimeTip;
    public UIButton Btn_Purchase;

    public GameObject Gobj_BattleLogPanel;
    public UISprite Spt_BattleLogMask;
    public UIButton Btn_CloseBattleLog;
    public UIScrollView ScrView_BattleLog;
    public UIGrid Grd_BattleLog;
    public GameObject Gobj_BattleLogComp;

    public GameObject Gobj_AwardGroupPanel;
    public UILabel Lbl_AwardTitle;
    public UILabel Lbl_AwardTipTitle;
    public GameObject Gobj_LowestAwardGroup;
    public UISprite Spt_LowestAwardIcon;
    public UILabel Lbl_LowestAwardTip;
    public GameObject Gobj_MiddleAwardGroup;
    public UISprite Spt_MiddleAwardIcon;
    public UILabel Lbl_MiddleAwardTip;
    public GameObject Gobj_HighestAwardGroup;
    public UISprite Spt_HighestAwardIcon;
    public UILabel Lbl_HighestAwardTip;
    public UISlider Slider_AwradsSlider;
    public UIButton Btn_AwardSureButton;

    public GameObject Gobj_SeasonRewardsPanel;
    public UIScrollView ScrollView_SeasonRewards;
    public UISprite Spt_AwardDescSprite;
    public UITable Table_GetDivisionAwards;
    public GameObject Gobj_DivisionAwardGroup;
    public UISprite Spt_SeasonRewardsMask;

    public GameObject Gobj_SelectOpponent;
    public UIScrollView ScrollView_OpponentScroll;
    public UIGrid Grd_Opponent;
    public UIWrapContent WrapContent_Opponent;
    public GameObject Gobj_OpponentComp;
    public UILabel Lbl_ReMatchCDTip;
    public UIButton Btn_ReMatach;
    public UILabel Lbl_BtnReMatch;
    public UISprite Spt_SelectOpponentMask;

    public GameObject Gobj_DayAwardEffect;
    public GameObject Gobj_WeekAwardEffect;
    public GameObject Gobj_LowAwardEffect;
    public GameObject Gobj_MidAwardEffect;
    public GameObject Gobj_HighAwardEffect;

    public UISprite Spt_DivisionEffect;
    public UISpriteAnimation SptAnim_DivisionEffect;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/QualifyingView");
        Gobj_RulePanel = _uiRoot.transform.FindChild("gobj_Rule").gameObject;
        Btn_CloseRule = _uiRoot.transform.FindChild("gobj_Rule/Anim/CloseRule").gameObject.GetComponent<UIButton>();
        ScrollView_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        Lbl_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/RuleDescGroup/RuleDesc").gameObject.GetComponent<UILabel>();
        Spt_RuleMask = _uiRoot.transform.FindChild("gobj_Rule/Mask").gameObject.GetComponent<UISprite>();
        Gobj_Main = _uiRoot.transform.FindChild("gobj_Main").gameObject;
        Btn_RuleDesc = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/RuleDesc").gameObject.GetComponent<UIButton>();
        Btn_Rank = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/Rank").gameObject.GetComponent<UIButton>();
        Btn_BattleLog = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/BattleLog").gameObject.GetComponent<UIButton>();
        Btn_StartMacth = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/StartMacth").gameObject.GetComponent<UIButton>();
        Grd_DefenseSoldierGrid = _uiRoot.transform.FindChild("gobj_Main/Anim/DefenseGroup/DefenseSoldierGroup/DefenseSoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_DefenseSoldierComp = _uiRoot.transform.FindChild("gobj_Main/Anim/DefenseGroup/DefenseSoldierGroup/DefenseSoldierGrid/gobj_DefenseSoldierComp").gameObject;
        Lbl_CombatPower = _uiRoot.transform.FindChild("gobj_Main/Anim/DefenseGroup/AdjustGroup/CombatPowerGroup/CombatPower").gameObject.GetComponent<UILabel>();
        Btn_AdjustLineup = _uiRoot.transform.FindChild("gobj_Main/Anim/DefenseGroup/AdjustGroup/AdjustLineup").gameObject.GetComponent<UIButton>();
        Btn_Exit = _uiRoot.transform.FindChild("gobj_Main/Anim/Exit").gameObject.GetComponent<UIButton>();
        Lbl_RankTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankTitleGroup/RankTitle").gameObject.GetComponent<UILabel>();
        Spt_RankIcon = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankIconGroup/RankIcon").gameObject.GetComponent<UISprite>();
        Lbl_RankNum = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankNumGroup/RankNum").gameObject.GetComponent<UILabel>();
        Lbl_RankIntegral = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankIntegralGroup/RankIntegral").gameObject.GetComponent<UILabel>();
        Lbl_PlaySessions = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/PlaySessionsGroup/PlaySessions").gameObject.GetComponent<UILabel>();
        Lbl_WinsSessions = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/WinsSessionsGroup/WinsSessions").gameObject.GetComponent<UILabel>();
        Lbl_SeasonTimeDesc = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/MyWinningGroup/MyWinning").gameObject.GetComponent<UILabel>();
        Lbl_SeasonTimeDescTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/MyWinningGroup/Title").gameObject.GetComponent<UILabel>();
        Gobj_RightTopGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/RightTopGroup").gameObject;
        Lbl_ChanglleTimeTip = _uiRoot.transform.FindChild("gobj_Main/Anim/RightTopGroup/ChanglleGroup/ChanglleTimeTip").gameObject.GetComponent<UILabel>();
        Btn_Purchase = _uiRoot.transform.FindChild("gobj_Main/Anim/RightTopGroup/ChanglleGroup/Purchase").gameObject.GetComponent<UIButton>();
        Spt_DayAwardIcon = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup/Panel/DayAwardIcon").gameObject.GetComponent<UISprite>();
        Spt_WeekAwardIcon = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup/Panel/WeekAwardIcon").gameObject.GetComponent<UISprite>();
        Gobj_DayAwardEffect = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup/Chest").gameObject;
        Gobj_WeekAwardEffect = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup/Chest").gameObject;
        Spt_DivisionEffect = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankIconGroup/DivisionEffect").gameObject.GetComponent<UISprite>();
        SptAnim_DivisionEffect = _uiRoot.transform.FindChild("gobj_Main/Anim/RankAwardGroup/RankIconGroup/DivisionEffect").gameObject.GetComponent<UISpriteAnimation>();

        Gobj_AwardGroupPanel = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup").gameObject;
        Gobj_EveryDayAwardGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup").gameObject;
        Spt_DayAwardTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup/DayTitleGroup/DayAwardTitle").gameObject.GetComponent<UISprite>();
        Lbl_DayBattleTipTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup/DayBattleTipTitle").gameObject.GetComponent<UILabel>();
        Lbl_DayBattleTipNum = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryDayAwardGroup/DayBattleTipNum").gameObject.GetComponent<UILabel>();
        Gobj_EveryWeekAwardGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup").gameObject;
        Spt_WeekAwardTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup/WeekTitleGroup/WeekAwardTitle").gameObject.GetComponent<UISprite>();
        Lbl_WeekBattleTipTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup/WeekBattleTipTitle").gameObject.GetComponent<UILabel>();
        Lbl_WeekBattleTipNum = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EveryWeekAwardGroup/WeekBattleTipNum").gameObject.GetComponent<UILabel>();
        Gobj_EverySeasonAwardGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EverySeasonAwardGroup").gameObject;
        Spt_SeasonAwardIcon = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EverySeasonAwardGroup/SeasonAwardIcon").gameObject.GetComponent<UISprite>();
        Spt_SeasonAwardTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EverySeasonAwardGroup/SeasonTitleGroup/SeasonAwardTitle").gameObject.GetComponent<UISprite>();
        Lbl_SeasonBattleTipTitle = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EverySeasonAwardGroup/SeasonBattleTipTitle").gameObject.GetComponent<UILabel>();
        Lbl_SeasonBattleTipNum = _uiRoot.transform.FindChild("gobj_Main/Anim/gobj_AwardGroup/gobj_EverySeasonAwardGroup/SeasonBattleTipNum").gameObject.GetComponent<UILabel>();


        Gobj_BattleLogPanel = _uiRoot.transform.FindChild("gobj_BattleLog").gameObject;
        Spt_BattleLogMask = _uiRoot.transform.FindChild("gobj_BattleLog/Mask").gameObject.GetComponent<UISprite>();
        Btn_CloseBattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/CloseBattleLog").gameObject.GetComponent<UIButton>();
        ScrView_BattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLogPanel").gameObject.GetComponent<UIScrollView>();
        Grd_BattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLogPanel/BattleLog").gameObject.GetComponent<UIGrid>();
        Gobj_BattleLogComp = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLogPanel/BattleLog/gobj_BattleLogComp").gameObject;

        Gobj_AwardGroupPanel = _uiRoot.transform.FindChild("gobj_AwardGroup").gameObject;
        Lbl_AwardTitle = _uiRoot.transform.FindChild("gobj_AwardGroup/BGGroup/TitleGroup/AwardTitle").gameObject.GetComponent<UILabel>();
        Lbl_AwardTipTitle = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardTipTitle").gameObject.GetComponent<UILabel>();
        Gobj_LowestAwardGroup = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_LowestAwardGroup").gameObject;
        Spt_LowestAwardIcon = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_LowestAwardGroup/Panel/LowestAwardIcon").gameObject.GetComponent<UISprite>();
        Lbl_LowestAwardTip = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_LowestAwardGroup/LowestAwardTip").gameObject.GetComponent<UILabel>();
        Gobj_MiddleAwardGroup = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_MiddleAwardGroup").gameObject;
        Spt_MiddleAwardIcon = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_MiddleAwardGroup/Panel/MiddleAwardIcon").gameObject.GetComponent<UISprite>();
        Lbl_MiddleAwardTip = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_MiddleAwardGroup/MiddleAwardTip").gameObject.GetComponent<UILabel>();
        Gobj_HighestAwardGroup = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_HighestAwardGroup").gameObject;
        Spt_HighestAwardIcon = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_HighestAwardGroup/Panel/HighestAwardIcon").gameObject.GetComponent<UISprite>();
        Lbl_HighestAwardTip = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_HighestAwardGroup/HighestAwardTip").gameObject.GetComponent<UILabel>();
        Slider_AwradsSlider = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/AwradsSlider").gameObject.GetComponent<UISlider>();
        Btn_AwardSureButton = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardSureButton").gameObject.GetComponent<UIButton>();
        Gobj_LowAwardEffect = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_LowestAwardGroup/Chest").gameObject;
        Gobj_MidAwardEffect = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_MiddleAwardGroup/Chest").gameObject;
        Gobj_HighAwardEffect = _uiRoot.transform.FindChild("gobj_AwardGroup/AwardProgressGroup/gobj_HighestAwardGroup/Chest").gameObject;

        Gobj_SeasonRewardsPanel = _uiRoot.transform.FindChild("gobj_SeasonRewards").gameObject;
        ScrollView_SeasonRewards = _uiRoot.transform.FindChild("gobj_SeasonRewards/Anim/SeasonRewards").gameObject.GetComponent<UIScrollView>();
        Spt_AwardDescSprite = _uiRoot.transform.FindChild("gobj_SeasonRewards/Anim/SeasonRewards/AwardDescGroup/AwardDescSprite").gameObject.GetComponent<UISprite>();
        Table_GetDivisionAwards = _uiRoot.transform.FindChild("gobj_SeasonRewards/Anim/SeasonRewards/AwardDescGroup/GetDivisionAwards").gameObject.GetComponent<UITable>();
        Gobj_DivisionAwardGroup = _uiRoot.transform.FindChild("gobj_SeasonRewards/Anim/SeasonRewards/AwardDescGroup/GetDivisionAwards/gobj_AwardGroup").gameObject;
        Spt_SeasonRewardsMask = _uiRoot.transform.FindChild("gobj_SeasonRewards/Mask").gameObject.GetComponent<UISprite>();

        Gobj_SelectOpponent = _uiRoot.transform.FindChild("gobj_SelectOpponent").gameObject;
        ScrollView_OpponentScroll = _uiRoot.transform.FindChild("gobj_SelectOpponent/OpponentScroll").gameObject.GetComponent<UIScrollView>();
        Grd_Opponent = _uiRoot.transform.FindChild("gobj_SelectOpponent/OpponentScroll/Grid").gameObject.GetComponent<UIGrid>();
        WrapContent_Opponent = _uiRoot.transform.FindChild("gobj_SelectOpponent/OpponentScroll/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_OpponentComp = _uiRoot.transform.FindChild("gobj_SelectOpponent/OpponentScroll/OpponentComp").gameObject;
        Lbl_ReMatchCDTip = _uiRoot.transform.FindChild("gobj_SelectOpponent/ReMatchCDTip").gameObject.GetComponent<UILabel>();
        Btn_ReMatach = _uiRoot.transform.FindChild("gobj_SelectOpponent/ReMatch").gameObject.GetComponent<UIButton>();
        Lbl_BtnReMatch = _uiRoot.transform.FindChild("gobj_SelectOpponent/ReMatch/Label").gameObject.GetComponent<UILabel>();
        Spt_SelectOpponentMask = _uiRoot.transform.FindChild("gobj_SelectOpponent/BackGroundGroup/MarkSprite").gameObject.GetComponent<UISprite>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
