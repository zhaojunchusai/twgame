using UnityEngine;
using System;
using System.Collections;

public class EquipDetailInfoView
{
    public static string UIName = "EquipDetailInfoView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskSprite;
    //public UIGrid Grd_ButtomGroup;
    public UIButton Btn_SoldierEquipObtain;
    public UISprite Spt_SoldierEquipObtainBg;
    public UILabel Lbl_BtnSoldierEquipObtain;
    public UIButton Btn_SoldierEquipChange;
    public UILabel Lbl_BtnSoldierEquipChange;
    public UIButton Btn_SoldierEquipUnLoad;
    public UILabel Lbl_BtnSoldierEquipUnLoad;
    public UIButton Btn_SoldierEquipIntensify;
    public UILabel Lbl_BtnSoldierEquipIntensify;
    public UIButton Btn_SoldierEquipUpgrade;
    public UILabel Lbl_BtnSoldierEquipUpgrade;
    public UILabel Lbl_SoldierEquipInfoName;
    public UILabel Lbl_SoldierEquipLevel;
    public UISprite Spt_SoldierEquipInfoIcon;
    public UISprite Spt_SoldierEquipInfoQuality;
    public UISprite Spt_SoldierEquipInfoBg;
    public UIGrid Grd_StarLevelGroup;
    public UISprite Spt_SoldierEquipStar;
    public UILabel Lbl_Title;
    public UILabel Lbl_EquipDescLabel;
    public UILabel Lbl_EquipSkillDescLabel;

    public UIGrid Grd_NormalGroup;
    public GameObject Gobj_EquipAttComp;
    public GameObject Gobj_SuitGroup;
    
    public GameObject Gobj_BaseAttGroup;
    public UIGrid Grd_NormalAttGrid;
    public GameObject Gobj_NormalEquipAttComp;
    public GameObject Gobj_SuitAttGroup;
    public UIScrollView ScrView_SuitAtt;
    public UIGrid Grd_SuitAttGroup;
    public GameObject Gobj_SuitAttComp;

    public GameObject Gobj_SuitNameGroup;
    public UILabel Lbl_SuitName;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/EquipDetailInfoView");
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Gobj_SuitNameGroup = _uiRoot.transform.FindChild("InfoGroup/SuitNameGroup").gameObject;
        Lbl_SuitName = _uiRoot.transform.FindChild("InfoGroup/SuitNameGroup/Label").gameObject.GetComponent<UILabel>();
        //Grd_ButtomGroup = _uiRoot.transform.FindChild("ButtomGroup").gameObject.GetComponent<UIGrid>();
        Btn_SoldierEquipObtain = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipObtain").gameObject.GetComponent<UIButton>();
        Spt_SoldierEquipObtainBg = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipObtain/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnSoldierEquipObtain = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipObtain/tt").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipChange = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipChange").gameObject.GetComponent<UIButton>();
        Lbl_BtnSoldierEquipChange = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipChange/tt").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipUnLoad = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipUnLoad").gameObject.GetComponent<UIButton>();
        Lbl_BtnSoldierEquipUnLoad = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipUnLoad/tt").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipIntensify = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipIntensify").gameObject.GetComponent<UIButton>();
        Lbl_BtnSoldierEquipIntensify = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipIntensify/tt").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipUpgrade = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipUpgrade").gameObject.GetComponent<UIButton>();
        Lbl_BtnSoldierEquipUpgrade = _uiRoot.transform.FindChild("ButtomGroup/SoldierEquipUpgrade/tt").gameObject.GetComponent<UILabel>();
        Lbl_SoldierEquipInfoName = _uiRoot.transform.FindChild("InfoGroup/ItemComp/SoldierEquipInfoName").gameObject.GetComponent<UILabel>();
        Lbl_SoldierEquipLevel = _uiRoot.transform.FindChild("InfoGroup/ItemComp/SoldierEquipLevel").gameObject.GetComponent<UILabel>();
        Spt_SoldierEquipInfoIcon = _uiRoot.transform.FindChild("InfoGroup/ItemComp/ItemBaseComp/SoldierEquipInfoIcon").gameObject.GetComponent<UISprite>();
        Spt_SoldierEquipInfoQuality = _uiRoot.transform.FindChild("InfoGroup/ItemComp/ItemBaseComp/SoldierEquipInfoQuality").gameObject.GetComponent<UISprite>();
        Spt_SoldierEquipInfoBg = _uiRoot.transform.FindChild("InfoGroup/ItemComp/ItemBaseComp/SoldierEquipInfoBg").gameObject.GetComponent<UISprite>();
        Grd_StarLevelGroup = _uiRoot.transform.FindChild("InfoGroup/StarLevelGroup").gameObject.GetComponent<UIGrid>();
        Spt_SoldierEquipStar = _uiRoot.transform.FindChild("InfoGroup/StarLevelGroup/SoldierEquipStar").gameObject.GetComponent<UISprite>();
        Grd_NormalGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/NormalAttGroup").gameObject.GetComponent<UIGrid>();
        Lbl_Title = _uiRoot.transform.FindChild("BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Lbl_EquipDescLabel = _uiRoot.transform.FindChild("Descgroup/EquipDescLabel").gameObject.GetComponent<UILabel>();
        Lbl_EquipSkillDescLabel = _uiRoot.transform.FindChild("AttributeComparisonGroup/SkillDesc").gameObject.GetComponent<UILabel>();
        Gobj_EquipAttComp = _uiRoot.transform.FindChild("AttributeComparisonGroup/NormalAttGroup/EquipAttComp").gameObject;
        Gobj_SuitGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup").gameObject;
        Gobj_BaseAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/BaseAttGroup").gameObject;
        Grd_NormalAttGrid = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/BaseAttGroup/AttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_NormalEquipAttComp = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/BaseAttGroup/AttGroup/EquipAttComp").gameObject;
        Gobj_SuitAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/SuitAttGroup").gameObject;
        ScrView_SuitAtt = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/SuitAttGroup/SuitAttScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_SuitAttGroup = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/SuitAttGroup/SuitAttScrollView/AttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_SuitAttComp = _uiRoot.transform.FindChild("AttributeComparisonGroup/SuitAttGroup/SuitAttGroup/SuitAttScrollView/AttGroup/EquipAttComp").gameObject;


        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnSoldierEquipObtain.text = "获取";
        Lbl_BtnSoldierEquipChange.text = "更换";
        Lbl_BtnSoldierEquipUnLoad.text = "卸下";
        Lbl_BtnSoldierEquipIntensify.text = "强化";
        Lbl_BtnSoldierEquipUpgrade.text = "升星";
        Lbl_SoldierEquipInfoName.text = "555555";
        Lbl_SoldierEquipLevel.text = "24";
        Lbl_Title.text = "详细";
        Lbl_EquipDescLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
