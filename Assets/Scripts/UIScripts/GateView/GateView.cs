using UnityEngine;
using System;
using System.Collections;

public class GateView
{
    public static string UIName = "GateView";
    public GameObject _uiRoot;
    public UILabel Lbl_ChapterName;
    public GameObject Gobj_CrusadeTitle;
    public GameObject Gobj_EscortTitle;
    public GameObject Gobj_EliteCrusadeTitle;
    public UILabel Lbl_ChapterStar;
    public UISlider Slider_AwradsSlider;
    public UIButton Btn_LowAwards;
    public UISprite Spt_LowAwardsChest;
    public UILabel Lbl_LowAwardsNum;
    public UIButton Btn_MidAwards;
    public UISprite Spt_MidAwardsChest;
    public UILabel Lbl_MidAwardsNum;
    public UIButton Btn_HighAwards;
    public UISprite Spt_HighAwardsChest;
    public UILabel Lbl_HighAwardsNum;

    public GameObject Gobj_GateGroup;
    public GameObject Gobj_Gate;
    public UISprite Spt_Point;
    public UIButton Btn_Exit;
    public UIButton Btn_LeftArrow;
    public UIButton Btn_RightArrow;
    public GameObject Gobj_TexGroup;
    public UITexture Tex_ChapterBg;
    public UIButton Btn_Crusade;
    public UISprite Spt_BtnCrusadeType;
    public UIButton Btn_Escort;
    public UISprite Spt_BtnEscortType;
    public UIButton Btn_EliteCrusade;
    public UISprite Spt_BtnEliteCrusadeType;
    public GameObject Gobj_EscortGroup;
    public GameObject Gobj_CurrentGroup;
    public GameObject Gobj_CurrentScrollView;
    public UIScrollView ScrView_CurrentScrollView;
    public UIGrid Grd_CurrentGrid;
    public UIWrapContent UIWrapContent_CurrentGrid;
    public GameObject Gobj_HideGroup;
    public GameObject Gobj_HideScrollView;
    public UIScrollView ScrView_HideScrollView;
    public UIGrid Grd_HideGrid;
    public UIWrapContent UIWrapContent_HideGrid;
    public GameObject Gobj_EscortComp;
    public GameObject Gobj_EliteCrusadeGroup;
    public GameObject Gobj_EliteCrusade;
    public GameObject Gobj_IndicateGroup;

    public GameObject Gobj_LowChests;
    public GameObject Gobj_MidChests;
    public GameObject Gobj_HightChests;
    public UISprite Spt_EscortBgSprite;

    public GameObject Gobj_DescGroup;
    public UILabel Lbl_AwardDesc;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/GateView");
        Gobj_LowChests = _uiRoot.transform.FindChild("Anim/RateGroup/LowAwards/Chest1").gameObject;
        Gobj_MidChests = _uiRoot.transform.FindChild("Anim/RateGroup/MidAwards/Chest1").gameObject;
        Gobj_HightChests = _uiRoot.transform.FindChild("Anim/RateGroup/HighAwards/Chest1").gameObject;
        Lbl_ChapterName = _uiRoot.transform.FindChild("Anim/ChapterTitleGroup/TitleGroup/ChapterName").gameObject.GetComponent<UILabel>();
        Gobj_CrusadeTitle = _uiRoot.transform.FindChild("Anim/ChapterTitleGroup/TitleGroup/gobj_CrusadeTitle").gameObject;
        Gobj_EscortTitle = _uiRoot.transform.FindChild("Anim/ChapterTitleGroup/TitleGroup/gobj_EscortTitle").gameObject;
        Gobj_EliteCrusadeTitle = _uiRoot.transform.FindChild("Anim/ChapterTitleGroup/TitleGroup/gobj_EliteCrusadeTitle").gameObject;
        Lbl_ChapterStar = _uiRoot.transform.FindChild("Anim/RateGroup/StarGroup/ChapterStar").gameObject.GetComponent<UILabel>();
        Slider_AwradsSlider = _uiRoot.transform.FindChild("Anim/RateGroup/AwradsSlider").gameObject.GetComponent<UISlider>();
        Btn_LowAwards = _uiRoot.transform.FindChild("Anim/RateGroup/LowAwards").gameObject.GetComponent<UIButton>();
        Spt_LowAwardsChest = _uiRoot.transform.FindChild("Anim/RateGroup/LowAwards/Chest/Chest").gameObject.GetComponent<UISprite>();
        Lbl_LowAwardsNum = _uiRoot.transform.FindChild("Anim/RateGroup/LowAwards/Chest/StarGroup/Num").gameObject.GetComponent<UILabel>();
        Btn_MidAwards = _uiRoot.transform.FindChild("Anim/RateGroup/MidAwards").gameObject.GetComponent<UIButton>();
        Spt_MidAwardsChest = _uiRoot.transform.FindChild("Anim/RateGroup/MidAwards/Chest/Chest").gameObject.GetComponent<UISprite>();
        Lbl_MidAwardsNum = _uiRoot.transform.FindChild("Anim/RateGroup/MidAwards/Chest/StarGroup/Num").gameObject.GetComponent<UILabel>();
        Btn_HighAwards = _uiRoot.transform.FindChild("Anim/RateGroup/HighAwards").gameObject.GetComponent<UIButton>();
        Spt_HighAwardsChest = _uiRoot.transform.FindChild("Anim/RateGroup/HighAwards/Chest/Chest").gameObject.GetComponent<UISprite>();
        Lbl_HighAwardsNum = _uiRoot.transform.FindChild("Anim/RateGroup/HighAwards/Chest/StarGroup/Num").gameObject.GetComponent<UILabel>();
        Gobj_GateGroup = _uiRoot.transform.FindChild("Anim/gobj_GateGroup").gameObject;
        Gobj_Gate = _uiRoot.transform.FindChild("Anim/gobj_GateGroup/gobj_Gate").gameObject;
        Spt_Point = _uiRoot.transform.FindChild("Anim/gobj_GateGroup/Point").gameObject.GetComponent<UISprite>();
        Btn_Exit = _uiRoot.transform.FindChild("Anim/Exit").gameObject.GetComponent<UIButton>();
        Btn_LeftArrow = _uiRoot.transform.FindChild("Anim/ArrowGroup/LeftArrow").gameObject.GetComponent<UIButton>();
        Btn_RightArrow = _uiRoot.transform.FindChild("Anim/ArrowGroup/RightArrow").gameObject.GetComponent<UIButton>();
        Gobj_TexGroup = _uiRoot.transform.FindChild("Anim/gobj_TexGroup").gameObject;
        Tex_ChapterBg = _uiRoot.transform.FindChild("Anim/gobj_TexGroup/ChapterBg").gameObject.GetComponent<UITexture>();
        Btn_Crusade = _uiRoot.transform.FindChild("Anim/ButtonGroup/Crusade").gameObject.GetComponent<UIButton>();
        Spt_BtnCrusadeType = _uiRoot.transform.FindChild("Anim/ButtonGroup/Crusade/Type").gameObject.GetComponent<UISprite>();
        Btn_Escort = _uiRoot.transform.FindChild("Anim/ButtonGroup/Escort").gameObject.GetComponent<UIButton>();
        Spt_BtnEscortType = _uiRoot.transform.FindChild("Anim/ButtonGroup/Escort/Type").gameObject.GetComponent<UISprite>();
        Btn_EliteCrusade = _uiRoot.transform.FindChild("Anim/ButtonGroup/EliteCrusade").gameObject.GetComponent<UIButton>();
        Spt_BtnEliteCrusadeType = _uiRoot.transform.FindChild("Anim/ButtonGroup/EliteCrusade/Type").gameObject.GetComponent<UISprite>();
        Gobj_EscortGroup = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup").gameObject;
        Gobj_CurrentGroup = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_CurrentGroup").gameObject;
        Gobj_CurrentScrollView = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_CurrentGroup/gobj_CurrentScrollView").gameObject;
        ScrView_CurrentScrollView = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_CurrentGroup/gobj_CurrentScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_CurrentGrid = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_CurrentGroup/gobj_CurrentScrollView/gobj_CurrentGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_CurrentGrid = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_CurrentGroup/gobj_CurrentScrollView/gobj_CurrentGrid").gameObject.GetComponent<UIWrapContent>();

        Gobj_HideGroup = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_HideGroup").gameObject;
        Gobj_HideScrollView = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_HideGroup/gobj_HideScrollView").gameObject;
        ScrView_HideScrollView = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_HideGroup/gobj_HideScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_HideGrid = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_HideGroup/gobj_HideScrollView/gobj_HideGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_HideGrid = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_HideGroup/gobj_HideScrollView/gobj_HideGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_EscortComp = _uiRoot.transform.FindChild("Anim/gobj_EscortGroup/gobj_EscortComp").gameObject;
        Gobj_EliteCrusadeGroup = _uiRoot.transform.FindChild("Anim/gobj_EliteCrusadeGroup").gameObject;
        Gobj_EliteCrusade = _uiRoot.transform.FindChild("Anim/gobj_EliteCrusadeGroup/gobj_EliteCrusade").gameObject;
        Gobj_IndicateGroup = _uiRoot.transform.FindChild("Anim/gobj_IndicateGroup").gameObject;
        Spt_EscortBgSprite = _uiRoot.transform.FindChild("Anim/BgGroup/EscortSprite").gameObject.GetComponent<UISprite>();
        Gobj_DescGroup = _uiRoot.transform.FindChild("Anim/DescGroup").gameObject;
        Lbl_AwardDesc = _uiRoot.transform.FindChild("Anim/DescGroup/AwardDesc").gameObject.GetComponent<UILabel>(); 
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ChapterName.text = "";
        Lbl_ChapterStar.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }
}
