using UnityEngine;
using System;
using System.Collections;

public class SacrificialSystem
{
    public static string UIName ="SacrificialSystem";
    public GameObject _uiRoot;
    public UIPanel UIPanel_SacrificialSystem;
    public OffestSortingOrder OffestSortingOrder_SacrificialSystem;
    public UIButton Btn_FastEquipButton;
    public UIToggle Tog_TabSoldier;
    public UIToggle Tog_TabDebirs;
    public UIButton Btn_Button_close;
    public UIButton Btn_RuleButton;
    public UIButton Btn_SummonButton;
    public UILabel Lbl_Label_Summon;
    public UIPanel UIPanel_MaterialScroll;
    public UIScrollView ScrView_MaterialScroll;
    public UIGrid Grd_MaterialGrid;
    public UIWrapContent UIWrapContent_MaterialGrid;
    public UISlider Slider_MaterialSlider;
    public UISprite Spt_CostIcon;
    public UILabel Lbl_Label_CostGroup;
    public UISlider Slider_ProgressBar;
    public UISprite Spt_SliderProgressBarForeground;
    public UILabel Lbl_SliderProgressBarLabel;
    public UISprite Spt_SliderProgressBarBackColor;
    public UILabel Lbl_Label_Title;
    public UILabel Lbl_Label_CountGroup;
    public UILabel Lbl_Label_Tips;

    public UIToggle Tog_TabOrange;
    public UIToggle Tog_TabRed;
    public UIToggle Tog_TabOrange_Equip;
    public UIToggle Tog_TabRed_Equip;

    public GameObject ItemBase;

    public GameObject GO_GrayBackSpt;

    public GameObject Go_MaskGroup;
    public UILabel Lb_LockDep;
   // public GameObject GO_SoldierBackSpt;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SacrificialSystem");
        UIPanel_SacrificialSystem = _uiRoot.GetComponent<UIPanel>();
        OffestSortingOrder_SacrificialSystem = _uiRoot.GetComponent<OffestSortingOrder>();
        Btn_FastEquipButton = _uiRoot.transform.FindChild("Anim/ButtonGroup/FastEquipButton").gameObject.GetComponent<UIButton>();
        Tog_TabSoldier = _uiRoot.transform.FindChild("Anim/ButtonGroup/TabSoldier").gameObject.GetComponent<UIToggle>();
        Tog_TabDebirs = _uiRoot.transform.FindChild("Anim/ButtonGroup/TabDebirs").gameObject.GetComponent<UIToggle>();
        Tog_TabOrange = _uiRoot.transform.FindChild("Anim/AttributeGroup/SoldierBack/TabOrange").gameObject.GetComponent<UIToggle>();
        Tog_TabRed = _uiRoot.transform.FindChild("Anim/AttributeGroup/SoldierBack/TabRed").gameObject.GetComponent<UIToggle>();
        Tog_TabOrange_Equip = _uiRoot.transform.FindChild("Anim/AttributeGroup/GrayBack/TabOrange").gameObject.GetComponent<UIToggle>();
        Tog_TabRed_Equip = _uiRoot.transform.FindChild("Anim/AttributeGroup/GrayBack/TabRed").gameObject.GetComponent<UIToggle>();

        Btn_Button_close = _uiRoot.transform.FindChild("Anim/ButtonGroup/Button_close").gameObject.GetComponent<UIButton>();
        Btn_RuleButton = _uiRoot.transform.FindChild("Anim/ButtonGroup/RuleButton").gameObject.GetComponent<UIButton>();
        Btn_SummonButton = _uiRoot.transform.FindChild("Anim/ButtonGroup/SummonButton").gameObject.GetComponent<UIButton>();
        Lbl_Label_Summon = _uiRoot.transform.FindChild("Anim/ButtonGroup/SummonButton/Label").gameObject.GetComponent<UILabel>();
        ItemBase = _uiRoot.transform.FindChild("Anim/MaterialScroll/MaterialComp").gameObject;
        UIPanel_MaterialScroll = _uiRoot.transform.FindChild("Anim/MaterialScroll").gameObject.GetComponent<UIPanel>();
        ScrView_MaterialScroll = _uiRoot.transform.FindChild("Anim/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_MaterialGrid = _uiRoot.transform.FindChild("Anim/MaterialScroll/MaterialGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_MaterialGrid = _uiRoot.transform.FindChild("Anim/MaterialScroll/MaterialGrid").gameObject.GetComponent<UIWrapContent>();

        //Slider_MaterialSlider = _uiRoot.transform.FindChild("Anim/MaterialSlider").gameObject.GetComponent<UISlider>();
        Spt_CostIcon = _uiRoot.transform.FindChild("Anim/AttributeGroup/CostGroup").gameObject.GetComponent<UISprite>();
        Lbl_Label_CostGroup = _uiRoot.transform.FindChild("Anim/AttributeGroup/CostGroup/Label").gameObject.GetComponent<UILabel>();
        Slider_ProgressBar = _uiRoot.transform.FindChild("Anim/AttributeGroup/ProgressBar").gameObject.GetComponent<UISlider>();
        Spt_SliderProgressBarForeground = _uiRoot.transform.FindChild("Anim/AttributeGroup/ProgressBar/Foreground").gameObject.GetComponent<UISprite>();
        Lbl_SliderProgressBarLabel = _uiRoot.transform.FindChild("Anim/AttributeGroup/ProgressBar/Label").gameObject.GetComponent<UILabel>();
        Spt_SliderProgressBarBackColor = _uiRoot.transform.FindChild("Anim/AttributeGroup/ProgressBar/BackColor").gameObject.GetComponent<UISprite>();
        Lbl_Label_Title = _uiRoot.transform.FindChild("Anim/AttributeGroup/Title/Label").gameObject.GetComponent<UILabel>();
        Lbl_Label_CountGroup = _uiRoot.transform.FindChild("Anim/AttributeGroup/CountGroup").gameObject.GetComponent<UILabel>();
        Lbl_Label_Tips = _uiRoot.transform.FindChild("Anim/Tips").gameObject.GetComponent<UILabel>();
        Go_MaskGroup = _uiRoot.transform.FindChild("Anim/AttributeGroup/MaskObject").gameObject;
        Lb_LockDep = _uiRoot.transform.FindChild("Anim/AttributeGroup/MaskObject/Label").gameObject.GetComponent<UILabel>();
        ItemBase.SetActive(false);
        SetLabelValues();

        GO_GrayBackSpt = _uiRoot.transform.FindChild("Anim/AttributeGroup/GrayBack/Sprite").gameObject;
        //GO_SoldierBackSpt = _uiRoot.transform.FindChild("Anim/AttributeGroup/SoldierBack/Men").gameObject;
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
