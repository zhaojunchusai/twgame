using UnityEngine;
using System;
using System.Collections;

public class ExpeditionInfoView
{
    public static string UIName = "ExpeditionInfoView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_ExpeditionInfoView;
    public UIButton Btn_Exit;
    //public UISprite Spt_BtnExitBackground;
    public UISprite Spt_ViewMask;
    //public UISprite Spt_StageName;
    public UIGrid Grd_EnemyGrid;
    public GameObject Gobj_EnemyInfoComp;
    public UIButton Btn_ReadyBattle;
    //public UISprite Spt_BtnReadyBattleBG;
    //public UISprite Spt_BtnReadyBattleLabel;
    public GameObject Gobj_PlayerInfoComp;
    public GameObject Gobj_LineupSelect;
    //public UIPanel UIPanel_gobj_LineupSelect;
    public UIButton Btn_Colse;
    //public UISprite Spt_BtnColseBG;
    public UIGrid Grd_RedaySoldierGrid;
    public GameObject Gobj_ReadySoldierComp;
    //public UIPanel UIPanel_OwnSoldier;
    public UIScrollView ScrView_OwnSoldier;
    public UIGrid Grd_OwnSoldier;
    public UIWrapContent Wrap_OwnSoldier;
    public GameObject Gobj_OwnSoldierComp;
    public GameObject Gobj_StageInfoGroup;
    public UILabel Lbl_PlayerName;
    public UILabel Lbl_UnionName;
    public UILabel Lbl_GeneralNum;
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

    public TweenScale LineupSelect_TScale;
    public TweenScale Anim_TScale;

    public GameObject Gobj_EquipPet;
    public UILabel Lbl_EquipedPetName;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ExpeditionInfoView");
        //UIPanel_ExpeditionInfoView = _uiRoot.GetComponent<UIPanel>();
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        LineupSelect_TScale = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject.GetComponent<TweenScale>();
        Btn_Exit = _uiRoot.transform.FindChild("Anim/Exit").gameObject.GetComponent<UIButton>();
        //Spt_BtnExitBackground = _uiRoot.transform.FindChild("Anim/Exit/Background").gameObject.GetComponent<UISprite>();
        Spt_ViewMask = _uiRoot.transform.FindChild("ViewMask").gameObject.GetComponent<UISprite>();
        Gobj_StageInfoGroup = _uiRoot.transform.FindChild("Anim/StageInfoGroup").gameObject;
        //Spt_StageName = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Title/StageName").gameObject.GetComponent<UISprite>();
        Grd_EnemyGrid = _uiRoot.transform.FindChild("Anim/StageInfoGroup/EnemyGroup/EnemyGrid").gameObject.GetComponent<UIGrid>();
        Gobj_EnemyInfoComp = _uiRoot.transform.FindChild("Anim/StageInfoGroup/EnemyGroup/EnemyGrid/gobj_EnemyInfoComp").gameObject;
        Btn_ReadyBattle = _uiRoot.transform.FindChild("Anim/StageInfoGroup/ReadyBattle").gameObject.GetComponent<UIButton>();
        Lbl_PlayerName = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/Name/Label").gameObject.GetComponent<UILabel>();
        Lbl_UnionName = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/Union/Label").gameObject.GetComponent<UILabel>();
        //Spt_BtnReadyBattleBG = _uiRoot.transform.FindChild("Anim/StageInfoGroup/ReadyBattle/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnReadyBattleLabel = _uiRoot.transform.FindChild("Anim/StageInfoGroup/ReadyBattle/Label").gameObject.GetComponent<UISprite>();
        Gobj_PlayerInfoComp = _uiRoot.transform.FindChild("Anim/StageInfoGroup/Information/gobj_PlayerInfoComp").gameObject;
        Gobj_LineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject;
        //UIPanel_gobj_LineupSelect = _uiRoot.transform.FindChild("gobj_LineupSelect").gameObject.GetComponent<UIPanel>();
        Btn_Colse = _uiRoot.transform.FindChild("gobj_LineupSelect/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnColseBG = _uiRoot.transform.FindChild("gobj_LineupSelect/Colse/BG").gameObject.GetComponent<UISprite>();
        Grd_RedaySoldierGrid = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/RedaySoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_ReadySoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/RedaySoldierGroup/RedaySoldierGrid/gobj_ReadySoldierComp").gameObject;

        //UIPanel_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIPanel>();
        ScrView_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier").gameObject.GetComponent<UIScrollView>();
        Grd_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIGrid>();
        Wrap_OwnSoldier = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/OwnSoldier").gameObject.GetComponent<UIWrapContent>();
        Gobj_OwnSoldierComp = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/OwnSoldierGroup/OwnSoldier/gobj_OwnSoldierComp").gameObject;
        Lbl_GeneralNum = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/ReadyGeneralGroup/GeneralNum").gameObject.GetComponent<UILabel>();
        Btn_Next = _uiRoot.transform.FindChild("gobj_LineupSelect/InfoGroup/Next").gameObject.GetComponent<UIButton>();

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
        Gobj_EquipPet = _uiRoot.transform.FindChild("gobj_EquipSelect/Gobj_EquipPet").gameObject;
        Lbl_EquipedPetName = _uiRoot.transform.FindChild("gobj_EquipSelect/Gobj_EquipPet/PetName").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_GeneralNum.text = "19";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
