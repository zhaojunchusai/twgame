using UnityEngine;
using System;
using System.Collections;

public class PVPView
{
    public static string UIName ="PVPView";
    public GameObject _uiRoot;
    public GameObject Gobj_LineupSelect;
    public UIButton Btn_CloseLineupSelect;
    public UIGrid Grd_RedaySoldierGrid;
    public GameObject Gobj_ReadySoldierComp;
    public UIScrollView ScrView_OwnSoldier;
    public UIGrid Grd_OwnSoldier;
    public UIWrapContent UIWrapContent_OwnSoldier;
    public GameObject Gobj_OwnSoldierComp;
    public UILabel Lbl_Energy;
    public UIButton Btn_Next;
    public GameObject Gobj_EquipSelect;
    public UIButton Btn_CloseEquipSelect;
    public UIGrid Grd_RedayEquipGrid;
    public GameObject Gobj_ReadyEquipComp;
    public UIPanel UIPanel_OwnEquip;
    public UIScrollView ScrView_OwnEquip;
    public UIGrid Grd_OwnEquip;
    public UIWrapContent UIWrapContent_OwnEquip;
    public GameObject Gobj_OwnEquipComp;
    public UILabel Lbl_EquipCombatPower;
    public UILabel Lbl_DefenseCombatPower;
    public UIButton Btn_StartBattle;
    public UIButton Btn_Save;
    public GameObject Gobj_Main;
    public UILabel Lbl_RankDesc;
    public UILabel Lbl_RankNum;
    public UIButton Btn_RuleDesc;
    public UIButton Btn_Rank;
    public UIButton Btn_BattleLog;
    public UISprite Spt_BattleLogMark;
    public UIButton Btn_Reward;
    public UIGrid Grd_EnemyGroup;
    public GameObject Gobj_EnemyComp;
    public UILabel Lbl_SurplusCount;
    public UILabel Lbl_ChallengeCountDown;
    public UISprite Spt_ConsumeGoldIcon;
    public UILabel Lbl_ConsumeGold;
    public UIButton Btn_Purchase;
    public UIGrid Grd_DefenseSoldierGrid;
    public GameObject Gobj_DefenseSoldierComp;
    public UIButton Btn_AdjustLineup;
    public GameObject Gobj_BattleLog;
    public GameObject Gobj_BattleLogMask;
    //public UIButton Btn_CloseBattleLog;
    public UIScrollView ScrView_BattleLog;
    public UIGrid Grd_BattleLog;
    public GameObject Gobj_BattleLogComp;
    public GameObject Gobj_Rule;
    public GameObject Gobj_RuleMask;
    //public UIButton Btn_CloseRule;
    public GameObject Gobj_Countdown;
    public UILabel Lbl_ReadyCountDown;
    public UIButton Btn_Exit;
    public UIButton Btn_ClearTick;
    public UIButton Btn_RefreshPlayer;
    public GameObject Gobj_GoldGroup;
    public UILabel Lbl_LineUpTitle;
    //public UISprite Spt_AttackTitle;
    public UILabel Lbl_LogSurplus;
    public UILabel Lbl_LogCountDown;

    public UIScrollView ScrView_RuleDesc;
    public UITable Table_Rule;
    public UILabel Lbl_RuleRankDesc;
    public UILabel Lbl_RuleDesc;
    public UITable Table_GetAwards;
    public GameObject Gobj_AwardGroup;
    public GameObject Gobj_GetAwardsGroup;
    public UISprite Spt_SelfRankSprite;

    //public GameObject ItemPoint1;
    //public GameObject ItemPoint2;
    //public GameObject ItemPoint3;
    //public GameObject ItemPoint4;
    //public GameObject Go_PVPRefreshEffect;
    public TweenScale LineupSelect_TScale, EquipSelect_TScale, Main_TScale, BattleLog_TScale, Rule_TScale;
    public GameObject Gobj_EquipPet;
    public UILabel Lbl_EquipedPetName;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/PVPView");
        //ItemPoint1 = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ItemPoint/ItemPoint1").gameObject;
        //ItemPoint2 = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ItemPoint/ItemPoint2").gameObject;
        //ItemPoint3 = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ItemPoint/ItemPoint3").gameObject;
        //ItemPoint4 = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ItemPoint/ItemPoint4").gameObject;
        Gobj_LineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject;
        Rule_TScale = _uiRoot.transform.FindChild("gobj_Rule/Anim").gameObject.GetComponent<TweenScale>();
        Main_TScale = _uiRoot.transform.FindChild("gobj_Main/Anim").gameObject.GetComponent<TweenScale>();
        BattleLog_TScale = _uiRoot.transform.FindChild("gobj_BattleLog/Anim").gameObject.GetComponent<TweenScale>();
        EquipSelect_TScale = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim").gameObject.GetComponent<TweenScale>();
        LineupSelect_TScale = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim").gameObject.GetComponent<TweenScale>();
        Btn_CloseEquipSelect = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/CloseEquipSelect").gameObject.GetComponent<UIButton>();
        Btn_CloseLineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/CloseLineupSelect").gameObject.GetComponent<UIButton>();
        Grd_RedaySoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/RedaySoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadySoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/RedaySoldierGrid/gobj_ReadySoldierComp").gameObject;
        ScrView_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIScrollView>();
        Grd_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIGrid>();
        UIWrapContent_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/gobj_OwnSoldierComp").gameObject;
        Lbl_Energy = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/EnergyGroup/Energy").gameObject.GetComponent<UILabel>();
        Btn_Next = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/Next").gameObject.GetComponent<UIButton>();
        Gobj_EquipSelect = _uiRoot.transform.FindChild("gobj_EquipSelect").gameObject;
        Grd_RedayEquipGrid = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/RedayEquipGroup/RedayEquipGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadyEquipComp = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/RedayEquipGroup/gobj_ReadyEquipComp").gameObject;
        UIPanel_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/OwnEquipGroup/OwnEquip").gameObject.GetComponent<UIPanel>();
        ScrView_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/OwnEquipGroup/OwnEquip").gameObject.GetComponent<UIScrollView>();
        Grd_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/OwnEquipGroup/OwnEquip/OwnEquip").gameObject.GetComponent<UIGrid>();
        UIWrapContent_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/OwnEquipGroup/OwnEquip/OwnEquip").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnEquipComp = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/OwnEquipGroup/gobj_OwnEquipComp").gameObject;
        Lbl_EquipCombatPower = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/InfoGroup/BattleGroup/CombatPower").gameObject.GetComponent<UILabel>();
        Btn_StartBattle = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/StartBattle").gameObject.GetComponent<UIButton>();
        Btn_Save = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/Save").gameObject.GetComponent<UIButton>();
        Gobj_Main = _uiRoot.transform.FindChild("gobj_Main").gameObject;
        Lbl_RankDesc = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/RankGroup/RankDesc").gameObject.GetComponent<UILabel>();
        Lbl_RankNum = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/RankGroup/RankNum").gameObject.GetComponent<UILabel>();
        Btn_RuleDesc = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/RuleDesc").gameObject.GetComponent<UIButton>();
        Btn_Rank = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/Rank").gameObject.GetComponent<UIButton>();
        Btn_BattleLog = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/BattleLog").gameObject.GetComponent<UIButton>();
        Spt_BattleLogMark = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/BattleLog/BattleLogMark").gameObject.GetComponent<UISprite>();
        Btn_Reward = _uiRoot.transform.FindChild("gobj_Main/Anim/CenterGroup/BtnGroup/Reward").gameObject.GetComponent<UIButton>();
        Grd_EnemyGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/EnemyGroup").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyComp = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/EnemyGroup/gobj_EnemyComp").gameObject;
        Lbl_SurplusCount = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/SurplusGroup/SurplusCount").gameObject.GetComponent<UILabel>();
        Lbl_ChallengeCountDown = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/CountDown").gameObject.GetComponent<UILabel>();
        Spt_ConsumeGoldIcon = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/GoldGroup/Icon").gameObject.GetComponent<UISprite>();
        Lbl_ConsumeGold = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/GoldGroup/CosumeGold").gameObject.GetComponent<UILabel>();
        Btn_Purchase = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/Purchase").gameObject.GetComponent<UIButton>();
        Grd_DefenseSoldierGrid = _uiRoot.transform.FindChild("gobj_Main/Anim/TopGroup/DefenseSoldierGroup/DefenseSoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_DefenseSoldierComp = _uiRoot.transform.FindChild("gobj_Main/Anim/TopGroup/DefenseSoldierGroup/DefenseSoldierGrid/gobj_DefenseSoldierComp").gameObject;
        Lbl_DefenseCombatPower = _uiRoot.transform.FindChild("gobj_Main/Anim/TopGroup/AdjustGroup/CombatPowerGroup/CombatPower").gameObject.GetComponent<UILabel>();
        Btn_AdjustLineup = _uiRoot.transform.FindChild("gobj_Main/Anim/TopGroup/AdjustGroup/AdjustLineup").gameObject.GetComponent<UIButton>();
        Gobj_BattleLog = _uiRoot.transform.FindChild("gobj_BattleLog").gameObject;
        //Btn_CloseBattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/CloseBattleLog").gameObject.GetComponent<UIButton>();
        Gobj_BattleLogMask = _uiRoot.transform.FindChild("gobj_BattleLog/Mask").gameObject;
        ScrView_BattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLog").gameObject.GetComponent<UIScrollView>();
        Grd_BattleLog = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLog/BattleLog").gameObject.GetComponent<UIGrid>();
        Gobj_BattleLogComp = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/LogGroup/BattleLog/BattleLog/gobj_BattleLogComp").gameObject;
        Gobj_Rule = _uiRoot.transform.FindChild("gobj_Rule").gameObject;
        //Btn_CloseRule = _uiRoot.transform.FindChild("gobj_Rule/Anim/CloseRule").gameObject.GetComponent<UIButton>();
        Gobj_RuleMask = _uiRoot.transform.FindChild("gobj_Rule/Mask").gameObject;
        Gobj_Countdown = _uiRoot.transform.FindChild("gobj_Countdown").gameObject;
        Lbl_ReadyCountDown = _uiRoot.transform.FindChild("gobj_Countdown/Countdown/ReadyCountDown").gameObject.GetComponent<UILabel>();
        Btn_Exit = _uiRoot.transform.FindChild("gobj_Main/Anim/Exit").gameObject.GetComponent<UIButton>();
        Lbl_LineUpTitle = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        //Spt_AttackTitle = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/BGGroup/AttackTitle").gameObject.GetComponent<UISprite>();
        Btn_ClearTick = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/ClearTick").gameObject.GetComponent<UIButton>();
        Btn_RefreshPlayer = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/RefreshPlayer").gameObject.GetComponent<UIButton>();
        Gobj_GoldGroup = _uiRoot.transform.FindChild("gobj_Main/Anim/ButtomGroup/ChanglleInfoGroup/GoldGroup").gameObject;
        Lbl_LogSurplus = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/InfoGroup/LogSurplus").gameObject.GetComponent<UILabel>();
        Lbl_LogCountDown = _uiRoot.transform.FindChild("gobj_BattleLog/Anim/InfoGroup/LogCountDown").gameObject.GetComponent<UILabel>();
        ScrView_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        Lbl_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/RuleDescGroup/RuleDesc").gameObject.GetComponent<UILabel>();
        Lbl_RuleRankDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/SelfRank/RankDesc").gameObject.GetComponent<UILabel>();
        Table_Rule = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule").gameObject.GetComponent<UITable>();
        Table_GetAwards = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/AwardDescGroup/GetAwards").gameObject.GetComponent<UITable>();
        Gobj_AwardGroup = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/AwardDescGroup").gameObject;
        Gobj_GetAwardsGroup = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup").gameObject;
        Spt_SelfRankSprite = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/Rule/SelfRank/SelfRankSprite").gameObject.GetComponent<UISprite>();

        Gobj_EquipPet = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/Gobj_EquipPet").gameObject;
        Lbl_EquipedPetName = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/Gobj_EquipPet/PetName").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_RuleDesc.text = string.Empty;
        Lbl_EquipCombatPower.text = string.Empty;
        //Lbl_RankDesc.text = "我的排名";
        Lbl_RankNum.text = string.Empty;
        Lbl_SurplusCount.text = string.Empty;
        Lbl_ChallengeCountDown.text = string.Empty;
        Lbl_ConsumeGold.text = string.Empty;
        Lbl_EquipCombatPower.text = string.Empty;
        Lbl_ChallengeCountDown.text = string.Empty;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
