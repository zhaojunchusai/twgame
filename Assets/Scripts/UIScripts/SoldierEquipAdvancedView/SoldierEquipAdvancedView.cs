using UnityEngine;
using System;
using System.Collections;

public class SoldierEquipAdvancedView
{
    public static string UIName = "SoldierEquipAdvancedView";
    public GameObject _uiRoot;

    public UIButton Btn_SoldierEquipAdvanced;
    public UIButton Btn_FastSelect;
    public UIButton Btn_CloseEquipAdvance;

    public UISprite Btn_SoldierEquipAdvanced_Back;
    public UISprite Btn_FastSelect_Back;
    public UILabel Btn_SoldierEquipAdvanced_Label;
    public UILabel Btn_FastSelect_Label;

    public GameObject Gobj_SoldierEquipComp;
    public UILabel Lbl_SoldierEquipAdvancedName;
    public UISlider Slider_SoldierEquipProgress;
    public UILabel Lbl_SliderSoldierEquipProgress;

    public UIScrollView ScrView_SoldierEquipMaterial;
    public UIGrid Grd_MaterialGrid;
    public UIWrapContent UIWrapContent_Grid;
    public GameObject Gobj_SoldierEquipMaterialComp;

    public UIGrid Grd_SoldierEquipStarLevel;
    public GameObject Gobj_EquipStarComp;

    public UIGrid Grd_AttGroup;
    public GameObject Gobj_EquipAttComp;
    public UIPanel UIPanel_SoldierEquipMaterial;
    public UILabel Lbl_UpStarTip;
    public GameObject LessLv;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierEquipAdvancedView");

        Gobj_SoldierEquipComp = _uiRoot.transform.FindChild("AttributeBaseGroup/gobj_SoldierEquipComp").gameObject;
        Btn_SoldierEquipAdvanced = _uiRoot.transform.FindChild("ButtonGroup/SoldierEquipAdvanced").gameObject.GetComponent<UIButton>();
        Btn_SoldierEquipAdvanced_Back = _uiRoot.transform.FindChild("ButtonGroup/SoldierEquipAdvanced/Background").gameObject.GetComponent<UISprite>();
        Btn_SoldierEquipAdvanced_Label = _uiRoot.transform.FindChild("ButtonGroup/SoldierEquipAdvanced/Label").gameObject.GetComponent<UILabel>();

        Lbl_SoldierEquipAdvancedName = _uiRoot.transform.FindChild("AttributeBaseGroup/NameGroup/SoldierEquipAdvancedName").gameObject.GetComponent<UILabel>();
        Slider_SoldierEquipProgress = _uiRoot.transform.FindChild("AttributeBaseGroup/SoldierEquipProgress").gameObject.GetComponent<UISlider>();
        Lbl_SliderSoldierEquipProgress = _uiRoot.transform.FindChild("AttributeBaseGroup/SoldierEquipProgress/SoldierEquipProgress").gameObject.GetComponent<UILabel>();
        ScrView_SoldierEquipMaterial = _uiRoot.transform.FindChild("MaterialGroup/SoldierEquipMaterial").gameObject.GetComponent<UIScrollView>();
        UIPanel_SoldierEquipMaterial = _uiRoot.transform.FindChild("MaterialGroup/SoldierEquipMaterial").gameObject.GetComponent<UIPanel>();
        Grd_MaterialGrid = _uiRoot.transform.FindChild("MaterialGroup/SoldierEquipMaterial/Grid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Grid = _uiRoot.transform.FindChild("MaterialGroup/SoldierEquipMaterial/Grid").gameObject.GetComponent<UIWrapContent>();
        Gobj_SoldierEquipMaterialComp = _uiRoot.transform.FindChild("MaterialGroup/SoldierEquipMaterial/gobj_SoldierEquipMaterialComp").gameObject;
        Btn_FastSelect = _uiRoot.transform.FindChild("ButtonGroup/FastSelect").gameObject.GetComponent<UIButton>();
        Btn_FastSelect_Back = _uiRoot.transform.FindChild("ButtonGroup/FastSelect/Background").gameObject.GetComponent<UISprite>();
        Btn_FastSelect_Label = _uiRoot.transform.FindChild("ButtonGroup/FastSelect/Lable").gameObject.GetComponent<UILabel>();

        Grd_SoldierEquipStarLevel = _uiRoot.transform.FindChild("AttributeBaseGroup/SoldierEquipStarLevel").gameObject.GetComponent<UIGrid>();
        Gobj_EquipStarComp = _uiRoot.transform.FindChild("AttributeBaseGroup/SoldierEquipStarLevel/gobj_EquipStarComp").gameObject;
        Grd_AttGroup = _uiRoot.transform.FindChild("AttributeBaseGroup/AttributeComparisonGroup/AttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_EquipAttComp = _uiRoot.transform.FindChild("AttributeBaseGroup/AttributeComparisonGroup/AttGroup/gobj_EquipAttComp").gameObject;
        Btn_CloseEquipAdvance = _uiRoot.transform.FindChild("ButtonGroup/CloseEquipAdvance").gameObject.GetComponent<UIButton>();
        Lbl_UpStarTip = _uiRoot.transform.FindChild("MaterialGroup/UpStarTip").gameObject.GetComponent<UILabel>();
        LessLv = _uiRoot.transform.FindChild("AttributeBaseGroup/gobj_SoldierEquipComp/LessLv").gameObject;
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SoldierEquipAdvancedName.text = "";
        Lbl_SliderSoldierEquipProgress.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
