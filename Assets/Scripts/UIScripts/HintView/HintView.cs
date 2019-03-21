using UnityEngine;
using System;
using System.Collections;

public class HintView
{
    public static string UIName ="HintView";
    public GameObject _uiRoot;
    //public UIPanel UIPanel_HintView;
    public GameObject Gobj_YesNo;
    //public UISprite Spt_YesNoMask;
    //public UISprite Spt_YesNoBG;
    public UILabel Lbl_YesNoInfoLb;
    public UIButton Btn_YesBtn;
    //public UISprite Spt_BtnYesBtnBG;
    public UILabel Lbl_BtnYesBtnLb;
    public UIButton Btn_NoBtn;
    //public UISprite Spt_BtnNoBtnBG;
    public UILabel Lbl_BtnNoBtnLb;
    public GameObject Gobj_Ok;
    public UIButton Btn_OkYesBtn;
    //public UISprite Spt_BtnOkYesBtnBG;
    public UILabel Lbl_BtnOkYesBtnLb;
    public UILabel Lbl_OkInfoLb;
    //public UISprite Spt_OkBG;
    //public UISprite Spt_OkMask;
    public GameObject Gobj_Loading;
    public UISprite Spt_LoadingMask;
    public UILabel Lbl_Label;
    public GameObject Spt_Loading;
    public GameObject Gobj_CommonTipParent;
    public GameObject Gobj_CommonTipLabelGroup;
    public GameObject Gobj_RichText;
    //public UISprite Spt_Mask;
    //public UISprite Spt_BG;
    public UILabel Lbl_RichTextInfoLb;
    public UIButton Btn_RichTextYesBtn;
    //public UISprite Spt_BtnRichTextYesBtnBG;
    public UILabel Lbl_BtnRichTextYesBtnLb;
    public UIButton Btn_RichTextNoBtn;
    //public UISprite Spt_BtnRichTextNoBtnBG;
    public UILabel Lbl_BtnRichTextNoBtnLb;
    public GameObject Gobj_FightLoading;
    //public UISprite Spt_FightLoading;
    //public UISprite Spt_Icon;
    public UISlider Slider_Progress;
    //public UISprite Spt_SliderProgressBackground;
    //public UISprite Spt_SliderProgressForeground;
    //public UISprite Spt_SliderProgressThumb;
    public UILabel Lbl_FightLoadingTip;

    public GameObject Gobj_Rule;
    public UILabel Lbl_RuleValue;
    public UILabel Lbl_TitleValue;
    public UIScrollView ScrollView;
    public UIButton Btn_CloseMask;
    public UITexture Tex_FightLoadingBG;
    public UIBoundary Boundary = new UIBoundary();

    public GameObject gobj_YesNo_Mark;
    public UIButton leftBtn;
    public UIButton rightBtn;
    public UILabel DescriptLabel;
    public GameObject Toggle;
    public GameObject BG;
    public GameObject HL;
    public UILabel leftBtnLbl;
    public UILabel rightBtnLbl;
    public UILabel Toggle_Label;

    public TweenScale YesNo_TScale,OK_TScale,RichText_TScale;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/HintView");

        Gobj_Rule = _uiRoot.transform.FindChild("gobj_Rule").gameObject;
        Lbl_RuleValue = _uiRoot.transform.FindChild("gobj_Rule/RuleValueMask/RuleValue").gameObject.GetComponent<UILabel>();
        Lbl_TitleValue = _uiRoot.transform.FindChild("gobj_Rule/TitleValue").gameObject.GetComponent<UILabel>();
        Btn_CloseMask = _uiRoot.transform.FindChild("gobj_Rule/CloseMask").gameObject.GetComponent<UIButton>();
        ScrollView = _uiRoot.transform.FindChild("gobj_Rule/RuleValueMask").gameObject.GetComponent<UIScrollView>();

        //UIPanel_HintView = _uiRoot.GetComponent<UIPanel>();
        Gobj_YesNo = _uiRoot.transform.FindChild("gobj_YesNo").gameObject;
        YesNo_TScale = _uiRoot.transform.FindChild("gobj_YesNo/Anim").gameObject.GetComponent<TweenScale>();
        //Spt_YesNoMask = _uiRoot.transform.FindChild("gobj_YesNo/YesNoMask").gameObject.GetComponent<UISprite>();
        //Spt_YesNoBG = _uiRoot.transform.FindChild("gobj_YesNo/Anim/YesNoBG").gameObject.GetComponent<UISprite>();
        Lbl_YesNoInfoLb = _uiRoot.transform.FindChild("gobj_YesNo/Anim/YesNoInfoLb").gameObject.GetComponent<UILabel>();
        Btn_YesBtn = _uiRoot.transform.FindChild("gobj_YesNo/Anim/YesBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnYesBtnBG = _uiRoot.transform.FindChild("gobj_YesNo/Anim/YesBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnYesBtnLb = _uiRoot.transform.FindChild("gobj_YesNo/Anim/YesBtn/Lb").gameObject.GetComponent<UILabel>();
        Btn_NoBtn = _uiRoot.transform.FindChild("gobj_YesNo/Anim/NoBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnNoBtnBG = _uiRoot.transform.FindChild("gobj_YesNo/Anim/NoBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnNoBtnLb = _uiRoot.transform.FindChild("gobj_YesNo/Anim/NoBtn/Lb").gameObject.GetComponent<UILabel>();

        Gobj_Ok = _uiRoot.transform.FindChild("gobj_Ok").gameObject;
        OK_TScale = _uiRoot.transform.FindChild("gobj_Ok/Anim").gameObject.GetComponent<TweenScale>();
        Btn_OkYesBtn = _uiRoot.transform.FindChild("gobj_Ok/Anim/OkYesBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnOkYesBtnBG = _uiRoot.transform.FindChild("gobj_Ok/Anim/OkYesBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnOkYesBtnLb = _uiRoot.transform.FindChild("gobj_Ok/Anim/OkYesBtn/Lb").gameObject.GetComponent<UILabel>();
        Lbl_OkInfoLb = _uiRoot.transform.FindChild("gobj_Ok/Anim/OkInfoLb").gameObject.GetComponent<UILabel>();
        //Spt_OkBG = _uiRoot.transform.FindChild("gobj_Ok/Anim/OkBG").gameObject.GetComponent<UISprite>();
        //Spt_OkMask = _uiRoot.transform.FindChild("gobj_Ok/OkMask").gameObject.GetComponent<UISprite>();

        Gobj_Loading = _uiRoot.transform.FindChild("gobj_Loading").gameObject;
        Spt_LoadingMask = _uiRoot.transform.FindChild("gobj_Loading/LoadingMask").gameObject.GetComponent<UISprite>();
        Lbl_Label = _uiRoot.transform.FindChild("gobj_Loading/Label").gameObject.GetComponent<UILabel>();
        Spt_Loading = _uiRoot.transform.FindChild("gobj_Loading/Loading").gameObject;
        Gobj_CommonTipParent = _uiRoot.transform.FindChild("CommonTipLabelGroup").gameObject;
        Gobj_CommonTipLabelGroup = _uiRoot.transform.FindChild("CommonTipLabelGroup/Group").gameObject;

        Gobj_RichText = _uiRoot.transform.FindChild("gobj_RichText").gameObject;
        RichText_TScale = _uiRoot.transform.FindChild("gobj_RichText/Anim").gameObject.GetComponent<TweenScale>();
        //Spt_Mask = _uiRoot.transform.FindChild("gobj_RichText/Mask").gameObject.GetComponent<UISprite>();
        //Spt_BG = _uiRoot.transform.FindChild("gobj_RichText/Anim/BG").gameObject.GetComponent<UISprite>();
        Lbl_RichTextInfoLb = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextInfoLb").gameObject.GetComponent<UILabel>();
        Btn_RichTextYesBtn = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextYesBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnRichTextYesBtnBG = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextYesBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRichTextYesBtnLb = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextYesBtn/Lb").gameObject.GetComponent<UILabel>();
        Btn_RichTextNoBtn = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextNoBtn").gameObject.GetComponent<UIButton>();
        //Spt_BtnRichTextNoBtnBG = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextNoBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRichTextNoBtnLb = _uiRoot.transform.FindChild("gobj_RichText/Anim/RichTextNoBtn/Lb").gameObject.GetComponent<UILabel>();

        Gobj_FightLoading = _uiRoot.transform.FindChild("gobj_FightLoading").gameObject;
        //Spt_FightLoading = _uiRoot.transform.FindChild("gobj_FightLoading/FightLoading").gameObject.GetComponent<UISprite>();
        //Spt_Icon = _uiRoot.transform.FindChild("gobj_FightLoading/Icon").gameObject.GetComponent<UISprite>();
        Slider_Progress = _uiRoot.transform.FindChild("gobj_FightLoading/Progress").gameObject.GetComponent<UISlider>();
        //Spt_SliderProgressBackground = _uiRoot.transform.FindChild("gobj_FightLoading/Progress/Background").gameObject.GetComponent<UISprite>();
        //Spt_SliderProgressForeground = _uiRoot.transform.FindChild("gobj_FightLoading/Progress/Foreground").gameObject.GetComponent<UISprite>();
        //Spt_SliderProgressThumb = _uiRoot.transform.FindChild("gobj_FightLoading/Progress/Thumb").gameObject.GetComponent<UISprite>();
        Lbl_FightLoadingTip = _uiRoot.transform.FindChild("gobj_FightLoading/FightLoadingTip").gameObject.GetComponent<UILabel>();
        Tex_FightLoadingBG = _uiRoot.transform.FindChild("gobj_FightLoading/FightLoadingBG").gameObject.GetComponent<UITexture>();

        gobj_YesNo_Mark = _uiRoot.transform.FindChild("gobj_YesNo_Mark").gameObject;
        leftBtn = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/NoBtn").gameObject.GetComponent<UIButton>();
        rightBtn = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/YesBtn").gameObject.GetComponent<UIButton>();
        DescriptLabel = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/YesNoInfoLb").gameObject.GetComponent<UILabel>();
        Toggle = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/Toggle").gameObject;
        BG = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/Toggle/BG").gameObject;
        HL = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/Toggle/HL").gameObject;
        leftBtnLbl = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/NoBtn/Lb").gameObject.GetComponent<UILabel>();
        rightBtnLbl = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/YesBtn/Lb").gameObject.GetComponent<UILabel>();
        Toggle_Label = _uiRoot.transform.FindChild("gobj_YesNo_Mark/Anim/Toggle/Label").gameObject.GetComponent<UILabel>();

        SetLabelValues();
    }
    public void SetLabelValues()
    {
        Toggle_Label.text = ConstString.HINVIEW_MARK;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
