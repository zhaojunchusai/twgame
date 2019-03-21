using UnityEngine;
using System;
using System.Collections;

public class PrepareBattleView
{
    public static string UIName = "PrepareBattleView";
    public GameObject _uiRoot;
    public GameObject Gobj_LineupSelect;
    public UILabel Lbl_LineUpTitle;
    public UIButton Btn_CloseLineupSelect;
    public UIGrid Grd_RedaySoldierGrid;
    public GameObject Gobj_ReadySoldierComp;
    public UIScrollView ScrView_OwnSoldier;
    public UIGrid Grd_OwnSoldier;
    public UIWrapContent UIWrapContent_OwnSoldier;
    public GameObject Gobj_OwnSoldierComp;
    public GameObject Gobj_EnergyGroup;
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

    public GameObject Gobj_Countdown;
    public UILabel Lbl_ReadyCountDown;
    public UIGrid Grd_LockSoldierGrid;
    public GameObject Gobj_LockSoldierComp;
    public GameObject Gobj_GeneralNum;
    public UILabel Lbl_GeneralNum;
    public GameObject Gobj_LeadershipGroup;
    public UILabel Lbl_LeadershipNum;

    public GameObject Gobj_EquipPet;
    public UILabel Lbl_EquipedPetName;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/PrepareBattleView");
        Gobj_LineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject;
        Lbl_LineUpTitle = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Btn_CloseEquipSelect = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/CloseEquipSelect").gameObject.GetComponent<UIButton>();
        Btn_CloseLineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/CloseLineupSelect").gameObject.GetComponent<UIButton>();
        Grd_RedaySoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/RedaySoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadySoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/RedaySoldierGrid/gobj_ReadySoldierComp").gameObject;
        ScrView_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIScrollView>();
        Grd_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIGrid>();
        UIWrapContent_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/OwnSoldierGroup/gobj_OwnSoldierComp").gameObject;
        Gobj_EnergyGroup = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/EnergyGroup").gameObject;
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
        Gobj_Countdown = _uiRoot.transform.FindChild("gobj_Countdown").gameObject;
        Lbl_ReadyCountDown = _uiRoot.transform.FindChild("gobj_Countdown/Countdown/ReadyCountDown").gameObject.GetComponent<UILabel>();
        Grd_LockSoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/LockSoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_LockSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/RedaySoldierGroup/LockSoldierGrid/gobj_LockSoldierComp").gameObject;
        Gobj_GeneralNum = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/ReadyGeneralGroup").gameObject;
        Lbl_GeneralNum = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/ReadyGeneralGroup/GeneralNum").gameObject.GetComponent<UILabel>();
        Gobj_LeadershipGroup = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/LeadershipGroup").gameObject;
        Lbl_LeadershipNum = _uiRoot.transform.FindChild("gobj_LineupSelect/Anim/InfoGroup/LeadershipGroup/Leadership").gameObject.GetComponent<UILabel>();
        Gobj_EquipPet = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/Gobj_EquipPet").gameObject;
        Lbl_EquipedPetName = _uiRoot.transform.FindChild("gobj_EquipSelect/Anim/Gobj_EquipPet/PetName").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_EquipCombatPower.text = string.Empty;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
