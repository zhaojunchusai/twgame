using UnityEngine;
using System;
using System.Collections;

public class LifeSpiritView
{
    public static string UIName = "LifeSpiritView";
    public GameObject _uiRoot;

    public GameObject Gobj_MainPanel;
    public UIButton Btn_CloseView;
    public GameObject Gobj_SoldierScrollView;
    public UIScrollView ScrView_SoldierScrollView;
    public GameObject Gobj_SoldierComp;
    public UIGrid Grd_SoldierScrollGrid;
    public UIWrapContent UIWrapContent_SoldierScrollGrid;
    public UILabel Lbl_SoldierCount;
    public GameObject Gobj_PlayerInfo;
    public GameObject Gobj_SelectMark;
    public UISlider Slider_MainProgressBar;
    public UILabel Lbl_MainSliderProgressBar;
    public UILabel Lbl_MainCenterLevel;
    public GameObject Gobj_PlayerModel;
    public UILabel Lbl_CenterCombatPower;
    public GameObject Gobj_CenterStarLevelGroup;
    public UISprite Gobj_EquipStarComp1;
    public UISprite Gobj_EquipStarComp2;
    public UISprite Gobj_EquipStarComp3;
    public UISprite Gobj_EquipStarComp4;
    public UISprite Gobj_EquipStarComp5;
    public UISprite Gobj_EquipStarComp6;
    public UILabel Lbl_CenterTalent;
    public UIGrid Grd_CenterAttGrid;
    public GameObject Gobj_CenterAttComp;
    public UILabel Lbl_CenterPosition;
    public UIButton Btn_LifeSpiritBackPack;
    public UIButton Btn_HuntingLifeSpirit;
    public GameObject Gobj_CenterObjName;
    public UILabel Lbl_CenterObjName;
    public UIGrid Grd_MainRightEquip;
    public GameObject Gobj_MainRightLifeSpriteComp1;
    public GameObject Gobj_MainRightLifeSpriteComp2;
    public GameObject Gobj_MainRightLifeSpriteComp3;
    public GameObject Gobj_MainRightLifeSpriteComp4;
    public GameObject Gobj_MainRightLifeSpriteComp5;
    public GameObject Gobj_MainRightLifeSpriteComp6;
    public GameObject Gobj_MainRightLifeSpriteComp7;
    public GameObject Gobj_MainRightLifeSpriteComp8;
    public GameObject Gobj_HeroBGGroup;
    public UIButton Btn_FastEquipButton;
    public UILabel Lbl_FastEquipButton;
    public GameObject Gobj_SoldierBGGroup;

    public GameObject Gobj_LifeSpriteChoose;
    public UISprite Spt_CloseChooseMark;
    public UIScrollView ScrView_LifeSpriteChooseScrollView;
    public UIGrid Grd_LifeSpiritChooseGrid;
    public UIWrapContent UIWrapContent_LifeSpiritChooseGrid;
    public GameObject Gobj_LifeSpriteComp;

    public GameObject Gobj_LifeSpiritDetailPanel;
    public UIButton Btn_LifeSpiritChange;
    public UIButton Btn_LifeSpiritUnLoad;
    public UIButton Btn_LifeSpiritIntensify;
    public UILabel Lbl_LifeSpiritInfoName;
    public UISprite Spt_LifeSpiritInfoIcon;
    public UISprite Spt_LifeSpiritInfoQuality;
    public UISprite Spt_LifeSpiritInfoTypeMark;
    public UISprite Spt_LifeSpiritInfoBg;
    public UILabel Lbl_LifeSpiritLevel;
    public UIGrid Grd_DetailAttGroup;
    public GameObject Gobj_LifeSpiritDetailAttComp;
    public UILabel Lbl_LifeSpiritDetailSkillDesc;
    public UILabel Lbl_LifeSpiritDescLabel;
    public UISprite Spt_LifeSpiritDetailMaskSprite;
    public UILabel lbl_Label_Step;
    public GameObject Obj_Step;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/LifeSpiritView");
        Gobj_MainPanel = _uiRoot.transform.FindChild("gobj_MainPanel").gameObject;
        Btn_CloseView = _uiRoot.transform.FindChild("gobj_MainPanel/Button_Close").gameObject.GetComponent<UIButton>();
        Gobj_SoldierScrollView = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/SoldierGroup/gobj_SoldierScrollView").gameObject;
        ScrView_SoldierScrollView = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/SoldierGroup/gobj_SoldierScrollView").gameObject.GetComponent<UIScrollView>();
        Gobj_SoldierComp = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/SoldierGroup/gobj_SoldierScrollView/gobj_SoldierComp").gameObject;
        Grd_SoldierScrollGrid = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/SoldierGroup/gobj_SoldierScrollView/gobj_Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_SoldierScrollGrid = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/SoldierGroup/gobj_SoldierScrollView/gobj_Grid").gameObject.GetComponent<UIWrapContent>();
        Lbl_SoldierCount = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/NumGroup/Count").gameObject.GetComponent<UILabel>();
        Gobj_PlayerInfo = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/gobj_PlayerInfo").gameObject;
        Gobj_SelectMark = _uiRoot.transform.FindChild("gobj_MainPanel/LeftGroup/gobj_PlayerInfo/gobj_SelectMark").gameObject;
        Slider_MainProgressBar = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/ProgressBar").gameObject.GetComponent<UISlider>();
        Lbl_MainSliderProgressBar = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        Lbl_MainCenterLevel = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/PlayerLevel").gameObject.GetComponent<UILabel>();
        Gobj_PlayerModel = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/Panel_player/player").gameObject;
        Lbl_CenterCombatPower = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/CombatPowerGroup/CombatPower").gameObject.GetComponent<UILabel>();
        Gobj_CenterStarLevelGroup = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup").gameObject;
        Gobj_EquipStarComp1 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp1/SelectSprite").GetComponent<UISprite>();
        Gobj_EquipStarComp2 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp2/SelectSprite").GetComponent<UISprite>();
        Gobj_EquipStarComp3 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp3/SelectSprite").GetComponent<UISprite>();
        Gobj_EquipStarComp4 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp4/SelectSprite").GetComponent<UISprite>();
        Gobj_EquipStarComp5 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp5/SelectSprite").GetComponent<UISprite>();
        Gobj_EquipStarComp6 = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/StarLevelGroup/EquipStarComp6/SelectSprite").GetComponent<UISprite>();
        Lbl_CenterTalent = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/Talent").gameObject.GetComponent<UILabel>();
        Grd_CenterAttGrid = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/AttributeGroup/AttGrid").gameObject.GetComponent<UIGrid>();
        Gobj_CenterAttComp = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/AttributeGroup/gobj_AttComp").gameObject;
        Lbl_CenterPosition = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/Position").gameObject.GetComponent<UILabel>();
        Btn_LifeSpiritBackPack = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/LifeSpiritBackPack").gameObject.GetComponent<UIButton>();
        Btn_HuntingLifeSpirit = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/HuntingLifeSpirit").gameObject.GetComponent<UIButton>();
        Gobj_CenterObjName = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/NameGoup").gameObject;

        Lbl_CenterObjName = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/CenterObjInfo/NameGoup/name").gameObject.GetComponent<UILabel>();
        Grd_MainRightEquip = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip").gameObject.GetComponent<UIGrid>();
        Gobj_MainRightLifeSpriteComp1 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp1").gameObject;
        Gobj_MainRightLifeSpriteComp2 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp2").gameObject;
        Gobj_MainRightLifeSpriteComp3 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp3").gameObject;
        Gobj_MainRightLifeSpriteComp4 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp4").gameObject;
        Gobj_MainRightLifeSpriteComp5 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp5").gameObject;
        Gobj_MainRightLifeSpriteComp6 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp6").gameObject;
        Gobj_MainRightLifeSpriteComp7 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp7").gameObject;
        Gobj_MainRightLifeSpriteComp8 = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/Equip/gobj_LifeSpriteComp8").gameObject;
        Gobj_HeroBGGroup = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/gobj_HeroBGGroup").gameObject;
        Btn_FastEquipButton = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/FastEquipButton").gameObject.GetComponent<UIButton>();
        Lbl_FastEquipButton = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/FastEquipButton/Label").gameObject.GetComponent<UILabel>();
        Gobj_SoldierBGGroup = _uiRoot.transform.FindChild("gobj_MainPanel/RightGroup/gobj_SoldierBGGroup").gameObject;
        Gobj_LifeSpriteChoose = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose").gameObject;
        Spt_CloseChooseMark = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose/MarkSprite").GetComponent<UISprite>();
        ScrView_LifeSpriteChooseScrollView = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose/Anim/LifeSpriteScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_LifeSpiritChooseGrid = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose/Anim/LifeSpriteScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_LifeSpiritChooseGrid = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose/Anim/LifeSpriteScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_LifeSpriteComp = _uiRoot.transform.FindChild("gobj_LifeSpriteChoose/Anim/LifeSpriteScrollView/Grid/gobj_LifeSpriteComp").gameObject;

        Gobj_LifeSpiritDetailPanel = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo").gameObject;
        Btn_LifeSpiritChange = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/ButtomGroup/LifeSpiritChange").gameObject.GetComponent<UIButton>();
        Btn_LifeSpiritUnLoad = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/ButtomGroup/LifeSpiritUnLoad").gameObject.GetComponent<UIButton>();
        Btn_LifeSpiritIntensify = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/ButtomGroup/LifeSpiritIntensify").gameObject.GetComponent<UIButton>();
        Lbl_LifeSpiritInfoName = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/ItemComp/LifeSpiritName").gameObject.GetComponent<UILabel>();
        Spt_LifeSpiritInfoIcon = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/ItemComp/ItemBaseComp/LifeSpiritInfoIcon").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritInfoQuality = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/ItemComp/ItemBaseComp/LifeSpiritInfoQuality").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritInfoTypeMark = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/ItemComp/ItemBaseComp/LifeSpiritInfoTypeMark").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritInfoBg = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/ItemComp/ItemBaseComp/LifeSpiritInfoBg").gameObject.GetComponent<UISprite>();
        Lbl_LifeSpiritLevel = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/InfoGroup/LifeSpiritLevel").gameObject.GetComponent<UILabel>();
        Grd_DetailAttGroup = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/AttributeComparisonGroup/AttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_LifeSpiritDetailAttComp = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/AttributeComparisonGroup/AttGroup/LifeSpiritAttComp").gameObject;
        Lbl_LifeSpiritDetailSkillDesc = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/AttributeComparisonGroup/LifeSpiritSkillDesc").gameObject.GetComponent<UILabel>();
        Lbl_LifeSpiritDescLabel = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/Descgroup/EquipDescLabel").gameObject.GetComponent<UILabel>();
        Spt_LifeSpiritDetailMaskSprite = _uiRoot.transform.FindChild("gobj_LifeSoulDetailInfo/MaskSprite").gameObject.GetComponent<UISprite>();
        Obj_Step = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/StepBack").gameObject;
        lbl_Label_Step = _uiRoot.transform.FindChild("gobj_MainPanel/CenterGroup/StepBack/Step").gameObject.GetComponent<UILabel>();
        Obj_Step.SetActive(GlobalConst.IsOpenStep);
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SoldierCount.text = "150/150";
        Lbl_MainCenterLevel.text = "Lv.23";
        Lbl_CenterCombatPower.text = "";
        Lbl_CenterTalent.text = "222";
        Lbl_CenterPosition.text = "12";
        Lbl_CenterObjName.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
