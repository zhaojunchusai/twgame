using UnityEngine;
using System;
using System.Collections;

public class GateInfoView
{
    public static string UIName = "GateInfoView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_GateInfoView;
    public GameObject Gobj_StageInfoGroup;
    public GameObject Gobj_StarsGroup;
    public UIButton Btn_Close;
    //public UISprite Spt_BtnCloseBG;
    //public UISprite Spt_Reel;
    //public UISprite Spt_Titlle;
    public UILabel Lbl_TitleLabel;
    //public UISprite Spt_BGSprite;
    public UISprite Spt_Elite;
    public UIGrid Grd_StarsGrid;
    public UISprite Spt_Star;
    public UILabel Lbl_StageDesc;
    //public UIPanel UIPanel_EnemyGroup;
    //public UISprite Spt_EnemyTip;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    //public UIPanel UIPanel_AwardsGroup;
    public UIGrid Grd_AwardsGrid;
    public GameObject Gobj_AwardsInfoComp;
    //public UISprite Spt_IconSprite;
    public GameObject Gobj_SPGroup;
    public UILabel Lbl_SPLabel;
    public UIButton Btn_ReadyBattle;

    public UIButton Btn_DirectBattle;

    public GameObject Gobj_SweepGroup;
    public GameObject Gobj_SweepTip;
    public GameObject Gobj_SingleSweepGroup;
    public GameObject Gobj_MulitSweepGroup;
    public GameObject Gobj_MulitSweepTip;
    public UILabel Lbl_MulitSweepTipLevel;
    public UISprite Spt_MulitSweepVIP;
    public UILabel Lbl_MulitSweepTip;
    public GameObject Gobj_SurplusSweepCard;
    public UILabel Lbl_SweepCardCount;
    public GameObject Gobj_SingleSweepTip;
    public UISprite Spt_SingleSweepVIP;
    public UILabel Lbl_SingleSweepTipLevel;
    public UILabel Lbl_SingleSweepTip;
    public UILabel Lbl_StarUnlockTip;
    public GameObject Gobj_PurchaesGroup;
    public UILabel Lbl_SurplusCount;
    //public GameObject Gobj_SurplusGroup;
    public UIButton Btn_Purchase;
    public UIGrid Grd_SweepBtnGrid;
    public GameObject Gobj_SingleSweepDiamondTip;
    public UILabel Lbl_SingleSweepDiamondTip;

    public UIButton Btn_SingleSweep;
    public UILabel Lbl_BtnSingleSweep;
    public UISprite Spt_BtnSingleSweepBG;
    public UIButton Btn_MulitSweep;
    public UILabel Lbl_BtnMulitSweep;
    public GameObject Gobj_MulitSweepDiamondTip;
    public UILabel Lbl_MulitSweepDiamondTip;
    public UISprite Spt_BtnMulitSweepBG;
    public GameObject Gobj_LineupSelect;
    public UIButton Btn_Back;
    //public UISprite Spt_PanelMask;
    public UIGrid Grd_RedaySoldierGrid;
    public GameObject Gobj_ReadySoldierComp;
    //public UIPanel UIPanel_OwnSoldier;
    public UIScrollView ScrView_OwnSoldier;
    public UIGrid Grd_OwnSoldier;
    public GameObject Gobj_OwnSoldierComp;
    public GameObject Gobj_LeadershipLimitGroup;
    public UILabel Lbl_Leadership;
    public UIButton Btn_Next;
    //public UISprite Spt_BtnStartBattleBG;
    //public UISprite Spt_BtnStartBattleLabel;
    public UIWrapContent Wrap_OwnSoldier;
    public UILabel Lbl_MutilSweepCount;

    public TweenScale StageInfoGroup_TScale;
    public TweenScale LineupSelect_TScale;

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
    public GameObject Gobj_GateCountGroup;
    public UILabel Lbl_GateCountLabel;
    public UIGrid Grd_LockSoldierGrid;
    public GameObject Gobj_LockSoldierComp;

    public GameObject Gobj_EquipPet;
    public UILabel Lbl_EquipedPetName;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/GateInfoView");
        LineupSelect_TScale = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject.GetComponent<TweenScale>();
        StageInfoGroup_TScale = _uiRoot.transform.FindChild("gobj_StageInfoGroup").gameObject.GetComponent<TweenScale>();
        //UIPanel_GateInfoView = _uiRoot.GetComponent<UIPanel>();
        Gobj_StageInfoGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup").gameObject;
        Btn_Close = _uiRoot.transform.FindChild("gobj_StageInfoGroup/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBG = _uiRoot.transform.FindChild("gobj_StageInfoGroup/Close/BG").gameObject.GetComponent<UISprite>();
        //Spt_Reel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/BGGroup/Reel").gameObject.GetComponent<UISprite>();
        //Spt_Titlle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/BGGroup/Titlle").gameObject.GetComponent<UISprite>();
        Gobj_StarsGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_StarsGroup").gameObject;
        Lbl_TitleLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        //Spt_BGSprite = _uiRoot.transform.FindChild("gobj_StageInfoGroup/TitleGroup/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_Elite = _uiRoot.transform.FindChild("gobj_StageInfoGroup/TitleGroup/Elite").gameObject.GetComponent<UISprite>();
        Grd_StarsGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_StarsGroup/StarsGrid").gameObject.GetComponent<UIGrid>();
        Spt_Star = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_StarsGroup/StarsGrid/Star").gameObject.GetComponent<UISprite>();
        Lbl_StageDesc = _uiRoot.transform.FindChild("gobj_StageInfoGroup/DescGroup/StageDesc").gameObject.GetComponent<UILabel>();
        //UIPanel_EnemyGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup").gameObject.GetComponent<UIPanel>();
        //Spt_EnemyTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyTip").gameObject.GetComponent<UISprite>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;
        //UIPanel_AwardsGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup").gameObject.GetComponent<UIPanel>();
        Grd_AwardsGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid").gameObject.GetComponent<UIGrid>();
        Gobj_AwardsInfoComp = _uiRoot.transform.FindChild("gobj_StageInfoGroup/AwardsGroup/AwardsGrid/gobj_AwardsInfoComp").gameObject;
        //Spt_IconSprite = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/SPGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Gobj_SPGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/SPGroup").gameObject;
        Lbl_SPLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/SPGroup/SPLabel").gameObject.GetComponent<UILabel>();
        Btn_ReadyBattle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/ReadyBattle").gameObject.GetComponent<UIButton>();
        Btn_DirectBattle = _uiRoot.transform.FindChild("gobj_StageInfoGroup/ReadyBattleGroup/DirectBattle").gameObject.GetComponent<UIButton>();
        Gobj_SweepGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup").gameObject;
        Gobj_SweepTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip").gameObject;
        Gobj_MulitSweepTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_MulitSweepTip").gameObject;
        Lbl_MulitSweepTipLevel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_MulitSweepTip/Level").gameObject.GetComponent<UILabel>();
        Lbl_MulitSweepTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_MulitSweepTip/Tip").gameObject.GetComponent<UILabel>();
        Spt_MulitSweepVIP = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_MulitSweepTip/Sprite").gameObject.GetComponent<UISprite>();
        Gobj_SurplusSweepCard = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SurplusSweepCard").gameObject;
        Lbl_SweepCardCount = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SurplusSweepCard/SweepCardCount").gameObject.GetComponent<UILabel>();
        Gobj_SingleSweepTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SingleSweepTip").gameObject;
        Lbl_SingleSweepTipLevel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SingleSweepTip/Level").gameObject.GetComponent<UILabel>();
        Lbl_SingleSweepTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SingleSweepTip/Tip").gameObject.GetComponent<UILabel>();
        Spt_SingleSweepVIP = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/gobj_SingleSweepTip/Sprite").gameObject.GetComponent<UISprite>();
        Lbl_StarUnlockTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_SweepTip/StarUnlockTip").gameObject.GetComponent<UILabel>();
        Gobj_PurchaesGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_PurchaesGroup").gameObject;
        Lbl_SurplusCount = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_PurchaesGroup/SurplusGroup/SurplusCount").gameObject.GetComponent<UILabel>();
        //Gobj_SurplusGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/gobj_PurchaesGroup/SurplusGroup").gameObject;
        Btn_Purchase = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_PurchaesGroup/Purchase").gameObject.GetComponent<UIButton>();
        Grd_SweepBtnGrid = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid").gameObject.GetComponent<UIGrid>();
        Gobj_SingleSweepGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup").gameObject;
        Gobj_MulitSweepGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup").gameObject;
        Btn_SingleSweep = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup/SingleSweep").gameObject.GetComponent<UIButton>();
        Spt_BtnSingleSweepBG = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup/SingleSweep/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnSingleSweep = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup/SingleSweep/Label").gameObject.GetComponent<UILabel>();
        Gobj_SingleSweepDiamondTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup/SingleSweepDiamondTip").gameObject;
        Lbl_SingleSweepDiamondTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/SingleSweepGroup/SingleSweepDiamondTip/SingleSweepDiamondNum").gameObject.GetComponent<UILabel>();
        Btn_MulitSweep = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweep").gameObject.GetComponent<UIButton>();
        Lbl_BtnMulitSweep = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweep/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnMulitSweepBG = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweep/BG").gameObject.GetComponent<UISprite>();
        Gobj_MulitSweepDiamondTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweepDiamondTip").gameObject;
        Lbl_MulitSweepDiamondTip = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweepDiamondTip/MulitSweepDiamondNum").gameObject.GetComponent<UILabel>();
        Lbl_MutilSweepCount = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_SweepGroup/SweepBtnGrid/MulitSweepGroup/MulitSweep/Label").gameObject.GetComponent<UILabel>();
        Gobj_LineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject;
        Btn_Back = _uiRoot.transform.FindChild("gobj_LineupSelect/Back").gameObject.GetComponent<UIButton>();
        //Spt_PanelMask = _uiRoot.transform.FindChild("gobj_LineupSelect/PanelMask").gameObject.GetComponent<UISprite>();
        Grd_RedaySoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/RedaySoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadySoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/RedaySoldierGrid/gobj_ReadySoldierComp").gameObject;
        //UIPanel_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIPanel>();
        ScrView_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIScrollView>();
        Grd_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIGrid>();
        Wrap_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/gobj_OwnSoldierComp").gameObject;
        Gobj_LeadershipLimitGroup = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/LeadershipLimitGroup").gameObject;
        Lbl_Leadership = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/LeadershipLimitGroup/Leadership").gameObject.GetComponent<UILabel>();
        Btn_Next = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/Next").gameObject.GetComponent<UIButton>();
        //Spt_BtnStartBattleBG = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/Next/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnStartBattleLabel = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/Next/Label").gameObject.GetComponent<UISprite>();
        Gobj_EquipSelect = _uiRoot.transform.FindChild("gobj_EquipSelect").gameObject;
        Btn_CloseEquipSelect = _uiRoot.transform.FindChild("gobj_EquipSelect/CloseEquipSelect").gameObject.GetComponent<UIButton>();
        Grd_RedayEquipGrid = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/RedayEquipGroup/RedayEquipGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadyEquipComp = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/RedayEquipGroup/gobj_ReadyEquipComp").gameObject;
        UIPanel_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/OwnEquipGroup/OwnEquip").gameObject.GetComponent<UIPanel>();
        ScrView_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/OwnEquipGroup/OwnEquip").gameObject.GetComponent<UIScrollView>();
        Grd_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/OwnEquipGroup/OwnEquip/OwnEquip").gameObject.GetComponent<UIGrid>();
        UIWrapContent_OwnEquip = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/OwnEquipGroup/OwnEquip/OwnEquip").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnEquipComp = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/OwnEquipGroup/gobj_OwnEquipComp").gameObject;
        Lbl_EquipCombatPower = _uiRoot.transform.FindChild("gobj_EquipSelect/InfoGroup/BattleGroup/CombatPower").gameObject.GetComponent<UILabel>();
        Btn_StartBattle = _uiRoot.transform.FindChild("gobj_EquipSelect/StartBattle").gameObject.GetComponent<UIButton>();
        Gobj_GateCountGroup = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup").gameObject;
        Lbl_GateCountLabel = _uiRoot.transform.FindChild("gobj_StageInfoGroup/gobj_GateCountGroup/GateCountLabel").gameObject.GetComponent<UILabel>();
        Grd_LockSoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/LockSoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_LockSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/LockSoldierGrid/gobj_LockSoldierComp").gameObject;
        Gobj_EquipPet = _uiRoot.transform.FindChild("gobj_EquipSelect/Gobj_EquipPet").gameObject;
        Lbl_EquipedPetName = _uiRoot.transform.FindChild("gobj_EquipSelect/Gobj_EquipPet/PetName").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLabel.text = "New Label";
        Lbl_StageDesc.text = "New Label";
        Lbl_SPLabel.text = "30";
        Lbl_MulitSweepTipLevel.text = "4";
        Lbl_SweepCardCount.text = "100";
        Lbl_SingleSweepTipLevel.text = "1";
        Lbl_SurplusCount.text = "";
        Lbl_Leadership.text = "19/20";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
