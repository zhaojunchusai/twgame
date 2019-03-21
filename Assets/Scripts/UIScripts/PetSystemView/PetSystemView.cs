using UnityEngine;
using System;
using System.Collections;

public class PetSystemView
{
    public static string UIName = "PetSystemView";
    public GameObject _uiRoot;
    public UIButton Btn_Button_close;
    public UIScrollView ScrView_PetScrollView;
    public GameObject Gobj_PetInfoComp;
    public UIGrid Grd_PetManagementGrid;
    public UIWrapContent UIWrapContent_PetManagementGrid;
    public UILabel Lbl_AllPetCount;
    public UIButton Btn_EquipButton;
    public UISprite Spt_BtnEquipButtonBackground;
    public UILabel Lbl_BtnEquipButtonLabel;
    public UILabel Lbl_PetTalent;
    public UILabel Lbl_PetName;
    public GameObject Gobj_PlayerParent;
    public GameObject Gobj_AttDescGroup;
    public GameObject Gobj_LeftAttComp;
    public GameObject Gobj_RightAttComp;
    public UIButton Btn_UpgradeButton;
    public UISprite Spt_BtnUpgradeButtonBackground;
    public UISprite Spt_BtnUpgradeButtonprompt;
    public UILabel Lbl_BtnUpgradeButtonLabel;
    public GameObject Gobj_UpgradeMaterialGroup;
    public GameObject Gobj_GetPathGroup;
    public UILabel Lbl_GetPathDesc;
    public UIButton Btn_JumpButton;
    public UISprite Spt_BtnJumpButtonBackground;
    public UILabel Lbl_BtnJumpButtonLabel;
    public UIButton Btn_StrengthButton;
    public UISprite Spt_BtnStrengthButtonBackground;
    public UILabel Lbl_BtnStrengthButtonLabel;
    public UISprite Spt_BtnStrengthButtonprompt;
    public GameObject Gobj_SkillInfoComp;
    public GameObject Gobj_StrengthMaterialGroup;
    public UIScrollView ScrollView_SkillDesc;
    public UILabel Lbl_PetSkillDesc;
    public GameObject Gobj_TopAttGroup;
    public GameObject Gobj_phy_atk;
    public GameObject Gobj_hp_max;
    public GameObject Gobj_acc_rate;
    public GameObject Gobj_ddg_rate;
    public GameObject Gobj_crt_rate;
    public GameObject Gobj_tnc_rate;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/PetSystemView");
        Btn_Button_close = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        ScrView_PetScrollView = _uiRoot.transform.FindChild("Anim/LeftGroup/AllPetsGroup/gobj_PetScrollView").gameObject.GetComponent<UIScrollView>();
        Gobj_PetInfoComp = _uiRoot.transform.FindChild("Anim/LeftGroup/AllPetsGroup/gobj_PetScrollView/gobj_PetInfoComp").gameObject;
        Grd_PetManagementGrid = _uiRoot.transform.FindChild("Anim/LeftGroup/AllPetsGroup/gobj_PetScrollView/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_PetManagementGrid = _uiRoot.transform.FindChild("Anim/LeftGroup/AllPetsGroup/gobj_PetScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        Lbl_AllPetCount = _uiRoot.transform.FindChild("Anim/LeftGroup/NumGroup/Count").gameObject.GetComponent<UILabel>();
        Btn_EquipButton = _uiRoot.transform.FindChild("Anim/LeftGroup/EquipButton").gameObject.GetComponent<UIButton>();
        Spt_BtnEquipButtonBackground = _uiRoot.transform.FindChild("Anim/LeftGroup/EquipButton/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnEquipButtonLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/EquipButton/Label").gameObject.GetComponent<UILabel>();
        Lbl_PetTalent = _uiRoot.transform.FindChild("Anim/CenterGroup/PetInfoGroup/Talent").gameObject.GetComponent<UILabel>();
        Lbl_PetName = _uiRoot.transform.FindChild("Anim/CenterGroup/PetInfoGroup/NameGroup/PetName").gameObject.GetComponent<UILabel>();
        Gobj_PlayerParent = _uiRoot.transform.FindChild("Anim/CenterGroup/PetInfoGroup/Panel_player/player").gameObject;
        Gobj_AttDescGroup = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup").gameObject;
        Gobj_LeftAttComp = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/AddAttGroup/AttCompGroup/gobj_LeftAttComp").gameObject;
        Gobj_RightAttComp = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/AddAttGroup/AttCompGroup/gobj_RightAttComp").gameObject;
        Btn_UpgradeButton = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/UpgradeButton").gameObject.GetComponent<UIButton>();
        Spt_BtnUpgradeButtonBackground = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/UpgradeButton/Background").gameObject.GetComponent<UISprite>();
        Spt_BtnUpgradeButtonprompt = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/UpgradeButton/prompt").gameObject.GetComponent<UISprite>();
        Lbl_BtnUpgradeButtonLabel = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/UpgradeButton/Label").gameObject.GetComponent<UILabel>();
        Gobj_UpgradeMaterialGroup = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_AttDescGroup/UpgradeMaterialGroup").gameObject;
        Gobj_GetPathGroup = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_GetPathGroup").gameObject;
        Lbl_GetPathDesc = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_GetPathGroup/GetPathDesc").gameObject.GetComponent<UILabel>();
        Btn_JumpButton = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_GetPathGroup/JumpButton").gameObject.GetComponent<UIButton>();
        Spt_BtnJumpButtonBackground = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_GetPathGroup/JumpButton/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnJumpButtonLabel = _uiRoot.transform.FindChild("Anim/CenterGroup/ButtomGroup/gobj_GetPathGroup/JumpButton/Label").gameObject.GetComponent<UILabel>();
        Btn_StrengthButton = _uiRoot.transform.FindChild("Anim/RightGroup/StrengthButton").gameObject.GetComponent<UIButton>();
        Spt_BtnStrengthButtonBackground = _uiRoot.transform.FindChild("Anim/RightGroup/StrengthButton/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnStrengthButtonLabel = _uiRoot.transform.FindChild("Anim/RightGroup/StrengthButton/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnStrengthButtonprompt = _uiRoot.transform.FindChild("Anim/RightGroup/StrengthButton/prompt").gameObject.GetComponent<UISprite>();
        Gobj_SkillInfoComp = _uiRoot.transform.FindChild("Anim/RightGroup/gobj_SkillInfoComp").gameObject;
        Gobj_StrengthMaterialGroup = _uiRoot.transform.FindChild("Anim/RightGroup/gobj_StrengthMaterialGroup").gameObject;
        ScrollView_SkillDesc = _uiRoot.transform.FindChild("Anim/RightGroup/SkillDescScrollView").gameObject.GetComponent<UIScrollView>();
        Lbl_PetSkillDesc = _uiRoot.transform.FindChild("Anim/RightGroup/SkillDescScrollView/SkillDesc").gameObject.GetComponent<UILabel>();
        Gobj_TopAttGroup = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup").gameObject;
        Gobj_phy_atk = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_phy_atk").gameObject;
        Gobj_hp_max = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_hp_max").gameObject;
        Gobj_acc_rate = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_acc_rate").gameObject;
        Gobj_ddg_rate = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_ddg_rate").gameObject;
        Gobj_crt_rate = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_crt_rate").gameObject;
        Gobj_tnc_rate = _uiRoot.transform.FindChild("Anim/TopAttGroup/AttGroup/gobj_tnc_rate").gameObject;
    }

    public void SetLabelValues()
    {

    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
