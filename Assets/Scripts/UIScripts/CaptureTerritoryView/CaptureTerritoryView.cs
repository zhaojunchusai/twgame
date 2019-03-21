using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryView
{
    public static string UIName ="CaptureTerritoryView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_CaptureTerritoryView;
    public UIPanel UIPanel_Map;
    public UIScrollView ScrView_Map;
    public Transform Trans_Map;
    public UIPanel UIPanel_Other;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseCloseButtonSP;
    public UILabel Lbl_Title;
    public UIButton Btn_Rank;
    public UISprite Spt_BtnRankBackground;
    public UILabel Lbl_BtnRankLabel;
    public UIButton Btn_Rule;
    public UISprite Spt_BtnRuleBackground;
    public UILabel Lbl_BtnRuleLabel;
    public UIButton Btn_Allocation;
    public UISprite Spt_BtnAllocationBackground;
    public UISprite Spt_BtnAllocationNotice;
    public UIButton Btn_AddScale;
    public UISprite Spt_BtnAddScaleBackground;
    public UIButton Btn_ReduceScale;
    public UISprite Spt_BtnReduceScaleBackground;
    public UISprite Spt_TokenBG;
    public UISprite Spt_Token1;
    public UISprite Spt_Token2;
    public UISprite Spt_Token3;
    public UILabel Lbl_CountryInc1;
    public UILabel Lbl_CountryInc2;
    public UILabel Lbl_CountryInc3;
    public UISprite Spt_CountryIncBG1;
    public UISprite Spt_CountryIncBG2;
    public UISprite Spt_CountryIncBG3;
    public UISprite Spt_CountryIncFG1;
    public UISprite Spt_CountryIncFG2;
    public UISprite Spt_CountryIncFG3;
    public UISprite Spt_NotFighting;
    public UILabel Lbl_Timer;
    public UISprite Spt_TimerBG;
    public UISprite Spt_Corner1;
    public UISprite Spt_Corner2;
    public UISprite Spt_Corner3;
    public UISprite Spt_Corner4;
    public GameObject Gobj_TokenIncrease;
    public GameObject Gobj_CityItem;
    public Transform Trans_CityParent;
    public GameObject Gobj_Title;
    public UILabel Lbl_FightCD;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureTerritoryView");
        UIPanel_CaptureTerritoryView = _uiRoot.GetComponent<UIPanel>();
        UIPanel_Map = _uiRoot.transform.FindChild("Map").gameObject.GetComponent<UIPanel>();
        ScrView_Map = _uiRoot.transform.FindChild("Map").gameObject.GetComponent<UIScrollView>();
        Trans_Map = _uiRoot.transform.FindChild("Map/MapObj");
        UIPanel_Other = _uiRoot.transform.FindChild("Other").gameObject.GetComponent<UIPanel>();
        Btn_Close = _uiRoot.transform.FindChild("Other/Close").gameObject.GetComponent<UIButton>();
        Spt_BtnCloseCloseButtonSP = _uiRoot.transform.FindChild("Other/Close/CloseButtonSP").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("Other/TitleBG/Title").gameObject.GetComponent<UILabel>();
        Lbl_FightCD = _uiRoot.transform.FindChild("Other/FightCD").gameObject.GetComponent<UILabel>();
        Gobj_Title = _uiRoot.transform.FindChild("Other/TitleBG").gameObject;
        Btn_Rank = _uiRoot.transform.FindChild("Other/Rank").gameObject.GetComponent<UIButton>();
        Spt_BtnRankBackground = _uiRoot.transform.FindChild("Other/Rank/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRankLabel = _uiRoot.transform.FindChild("Other/Rank/Label").gameObject.GetComponent<UILabel>();
        Btn_Rule = _uiRoot.transform.FindChild("Other/Rule").gameObject.GetComponent<UIButton>();
        Spt_BtnRuleBackground = _uiRoot.transform.FindChild("Other/Rule/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRuleLabel = _uiRoot.transform.FindChild("Other/Rule/Label").gameObject.GetComponent<UILabel>();
        Btn_Allocation = _uiRoot.transform.FindChild("Other/Allocation").gameObject.GetComponent<UIButton>();
        Spt_BtnAllocationBackground = _uiRoot.transform.FindChild("Other/Allocation/Background").gameObject.GetComponent<UISprite>();
        Spt_BtnAllocationNotice = _uiRoot.transform.FindChild("Other/Allocation/Notice").gameObject.GetComponent<UISprite>();
        Btn_AddScale = _uiRoot.transform.FindChild("Other/AddScale").gameObject.GetComponent<UIButton>();
        Spt_BtnAddScaleBackground = _uiRoot.transform.FindChild("Other/AddScale/Background").gameObject.GetComponent<UISprite>();
        Btn_ReduceScale = _uiRoot.transform.FindChild("Other/ReduceScale").gameObject.GetComponent<UIButton>();
        Spt_BtnReduceScaleBackground = _uiRoot.transform.FindChild("Other/ReduceScale/Background").gameObject.GetComponent<UISprite>();
        Spt_TokenBG = _uiRoot.transform.FindChild("Other/Token/TokenBG").gameObject.GetComponent<UISprite>();
        Spt_Token1 = _uiRoot.transform.FindChild("Other/Token/Token1").gameObject.GetComponent<UISprite>();
        Spt_Token2 = _uiRoot.transform.FindChild("Other/Token/Token2").gameObject.GetComponent<UISprite>();
        Spt_Token3 = _uiRoot.transform.FindChild("Other/Token/Token3").gameObject.GetComponent<UISprite>();
        Gobj_TokenIncrease = _uiRoot.transform.FindChild("Other/Token/Increase").gameObject;
        Lbl_CountryInc1 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryInc1").gameObject.GetComponent<UILabel>();
        Lbl_CountryInc2 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryInc2").gameObject.GetComponent<UILabel>();
        Lbl_CountryInc3 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryInc3").gameObject.GetComponent<UILabel>();
        Spt_CountryIncBG1 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncBG1").gameObject.GetComponent<UISprite>();
        Spt_CountryIncBG2 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncBG2").gameObject.GetComponent<UISprite>();
        Spt_CountryIncBG3 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncBG3").gameObject.GetComponent<UISprite>();
        Spt_CountryIncFG1 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncFG1").gameObject.GetComponent<UISprite>();
        Spt_CountryIncFG2 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncFG2").gameObject.GetComponent<UISprite>();
        Spt_CountryIncFG3 = _uiRoot.transform.FindChild("Other/Token/Increase/CountryIncFG3").gameObject.GetComponent<UISprite>();
        Spt_NotFighting = _uiRoot.transform.FindChild("Other/Token/NotFighting").gameObject.GetComponent<UISprite>();
        Lbl_Timer = _uiRoot.transform.FindChild("Other/Timer").gameObject.GetComponent<UILabel>();
        Spt_TimerBG = _uiRoot.transform.FindChild("Other/Timer/TimerBG").gameObject.GetComponent<UISprite>();
        Spt_Corner1 = _uiRoot.transform.FindChild("Other/Corner/Corner1").gameObject.GetComponent<UISprite>();
        Spt_Corner2 = _uiRoot.transform.FindChild("Other/Corner/Corner2").gameObject.GetComponent<UISprite>();
        Spt_Corner3 = _uiRoot.transform.FindChild("Other/Corner/Corner3").gameObject.GetComponent<UISprite>();
        Spt_Corner4 = _uiRoot.transform.FindChild("Other/Corner/Corner4").gameObject.GetComponent<UISprite>();
        Gobj_CityItem = _uiRoot.transform.FindChild("Pre/CompaignCityItem").gameObject;
        Trans_CityParent = _uiRoot.transform.FindChild("Map/MapObj/Citys");
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = ConstString.LBL_CAPTURE_TERRITORY;
        Lbl_BtnRankLabel.text = ConstString.LBL_RANK_VIEW;
        Lbl_BtnRuleLabel.text = ConstString.TITLE_RULE;
        Lbl_CountryInc1.text = "";
        Lbl_CountryInc2.text = "";
        Lbl_CountryInc3.text = "";
        Lbl_Timer.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
