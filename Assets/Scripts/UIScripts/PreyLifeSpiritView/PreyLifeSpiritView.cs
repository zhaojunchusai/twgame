using UnityEngine;
using System;
using System.Collections;

public class PreyLifeSpiritView
{
    public static string UIName = "PreyLifeSpiritView";
    public GameObject _uiRoot;
    public UIButton Btn_Close;
    public GameObject Gobj_LifeSpiritScrView;
    public UIScrollView ScrView_LifeSpiritScrView;
    public UIGrid Grd_LifeSpiritGrid;
    public UIWrapContent UIWrapContent_LifeSpiritGrid;
    public GameObject Gobj_LifeSpiritComp;
    public UIButton Btn_White;
    public UIButton Btn_Green;
    public UIButton Btn_Blue;
    public UIButton Btn_Purple;
    public UIButton Btn_Orange;
    public UIButton Btn_Red;
    public UIButton Btn_QuickPickup;
    public UIButton Btn_QuickPrey;
    public UIButton Btn_Prey;
    public UISprite Spt_PreyConsumeIcon;
    public UILabel Lbl_PreyConsumeTip;
    public UIButton Btn_Rule;

    public GameObject Gobj_RulePanel;
    public UIButton Btn_CloseRule;
    public UIScrollView ScrollView_RuleDesc;
    public UILabel Lbl_RuleDesc;
    public UISprite Spt_RuleMask;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/PreyLifeSpiritView");
        Btn_Close = _uiRoot.transform.FindChild("Close").gameObject.GetComponent<UIButton>();
        Gobj_LifeSpiritScrView = _uiRoot.transform.FindChild("LifeSpiritGroup/gobj_LifeSpiritScrView").gameObject;
        ScrView_LifeSpiritScrView = _uiRoot.transform.FindChild("LifeSpiritGroup/gobj_LifeSpiritScrView").gameObject.GetComponent<UIScrollView>();
        Grd_LifeSpiritGrid = _uiRoot.transform.FindChild("LifeSpiritGroup/gobj_LifeSpiritScrView/LifeSpiritGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_LifeSpiritGrid = _uiRoot.transform.FindChild("LifeSpiritGroup/gobj_LifeSpiritScrView/LifeSpiritGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_LifeSpiritComp = _uiRoot.transform.FindChild("LifeSpiritGroup/gobj_LifeSpiritScrView/gobj_LifeSpiritComp").gameObject;
        Btn_White = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_White").gameObject.GetComponent<UIButton>();
        Btn_Green = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_Green").gameObject.GetComponent<UIButton>();
        Btn_Blue = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_Blue").gameObject.GetComponent<UIButton>();
        Btn_Purple = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_Purple").gameObject.GetComponent<UIButton>();
        Btn_Orange = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_Orange").gameObject.GetComponent<UIButton>();
        Btn_Red = _uiRoot.transform.FindChild("LockGroup/LockBtnGroup/gobj_Red").gameObject.GetComponent<UIButton>();
        Btn_QuickPickup = _uiRoot.transform.FindChild("ButtonGroup/QuickPickup").gameObject.GetComponent<UIButton>();
        Btn_QuickPrey = _uiRoot.transform.FindChild("ButtonGroup/QuickPrey").gameObject.GetComponent<UIButton>();
        Btn_Prey = _uiRoot.transform.FindChild("ButtonGroup/Prey").gameObject.GetComponent<UIButton>();
        Spt_PreyConsumeIcon = _uiRoot.transform.FindChild("ButtonGroup/Prey/ConsumeTipGroup/ConsumeIcon").gameObject.GetComponent<UISprite>();
        Lbl_PreyConsumeTip = _uiRoot.transform.FindChild("ButtonGroup/Prey/ConsumeTipGroup/ConsumeTip").gameObject.GetComponent<UILabel>();
        Btn_Rule = _uiRoot.transform.FindChild("ButtonGroup/Rule").gameObject.GetComponent<UIButton>();
        Gobj_RulePanel = _uiRoot.transform.FindChild("gobj_Rule").gameObject;
        Btn_CloseRule = _uiRoot.transform.FindChild("gobj_Rule/Anim/CloseRule").gameObject.GetComponent<UIButton>();
        ScrollView_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        Lbl_RuleDesc = _uiRoot.transform.FindChild("gobj_Rule/Anim/RuleDesc/RuleDescGroup/RuleDesc").gameObject.GetComponent<UILabel>();
        Spt_RuleMask = _uiRoot.transform.FindChild("gobj_Rule/Mask").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_PreyConsumeTip.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
