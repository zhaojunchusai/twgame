using UnityEngine;
using System;
using System.Collections;

public class GuildHegemonyView
{
    public static string UIName = "GuildHegemonyView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_GuildHegemonyView;
    public UISprite Spt_MaskSprite;
    public UIButton Btn_BackBtn;
    public UISprite Spt_BtnBackBtnBG;
    public UITexture Tex_Map;
    public UISprite Spt_BG;
    public UISprite Spt_Pattern;
    public UIButton Btn_RuleBtn;
    public UISprite Spt_BtnRuleBtnBG;
    public UILabel Lbl_BtnRuleBtnLb;
    public UIButton Btn_HegemonyRankBtn;
    public UISprite Spt_BtnHegemonyRankBtnBG;
    public UILabel Lbl_BtnHegemonyRankBtnLb;
    public UIButton Btn_KillRank;
    public UISprite Spt_BtnKillRankBG;
    public UILabel Lbl_BtnKillRankLb;
    public GameObject CityBtn1;
    public GameObject CityBtn4;
    public GameObject CityBtn5;
    public GameObject CityBtn3;
    public GameObject CityBtn2;
    public UISprite Spt_MyselfGuild;
    public UISprite Spt_MyselfGuildIcon;
    public UISprite Spt_MyselfGuidState;
    public UISprite Spt_MyselfQuality;
    public UISprite Spt_MyselfGuildBG;
    public UILabel Lbl_SelfGuildNum;
    public UILabel Lbl_SelfGuildName;
    public UISprite Spt_OtherGuild;
    public UISprite Spt_OtherGuildIcon;
    public UISprite Spt_OtherGuildState;
    public UISprite Spt_OtherQuality;
    public UISprite Spt_OtherGuildBG;
    public UILabel Lbl_OtherGuildNum;
    public UILabel Lbl_OtherGuildName;
    public UISprite Spt_IntegralBG;
    public UILabel Lbl_IntegralLb;
    public UILabel Lbl_SelfIntegralLb;
    public UILabel Lbl_SelfIntegralNum;
    public UILabel Lbl_OtherIntegralLb;
    public UILabel Lbl_OtherIntegralNum;
    public GameObject Gobj_SignUp;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/GuildHegemonyView");
        UIPanel_GuildHegemonyView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Btn_BackBtn = _uiRoot.transform.FindChild("Anim/BackBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnBackBtnBG = _uiRoot.transform.FindChild("Anim/BackBtn/BG").gameObject.GetComponent<UISprite>();
        Tex_Map = _uiRoot.transform.FindChild("Anim/BgGroup/Map").gameObject.GetComponent<UITexture>();
        Spt_BG = _uiRoot.transform.FindChild("Anim/BgGroup/BG").gameObject.GetComponent<UISprite>();
        Spt_Pattern = _uiRoot.transform.FindChild("Anim/BgGroup/Pattern").gameObject.GetComponent<UISprite>();
        Btn_RuleBtn = _uiRoot.transform.FindChild("Anim/RuleBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnRuleBtnBG = _uiRoot.transform.FindChild("Anim/RuleBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnRuleBtnLb = _uiRoot.transform.FindChild("Anim/RuleBtn/Lb").gameObject.GetComponent<UILabel>();
        Btn_HegemonyRankBtn = _uiRoot.transform.FindChild("Anim/HegemonyRankBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnHegemonyRankBtnBG = _uiRoot.transform.FindChild("Anim/HegemonyRankBtn/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnHegemonyRankBtnLb = _uiRoot.transform.FindChild("Anim/HegemonyRankBtn/Lb").gameObject.GetComponent<UILabel>();
        Btn_KillRank = _uiRoot.transform.FindChild("Anim/KillRank").gameObject.GetComponent<UIButton>();
        Spt_BtnKillRankBG = _uiRoot.transform.FindChild("Anim/KillRank/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnKillRankLb = _uiRoot.transform.FindChild("Anim/KillRank/Lb").gameObject.GetComponent<UILabel>();
        CityBtn1 = _uiRoot.transform.FindChild("Anim/City1").gameObject;
        CityBtn4 = _uiRoot.transform.FindChild("Anim/City4").gameObject;
        CityBtn5 = _uiRoot.transform.FindChild("Anim/City5").gameObject;
        CityBtn3 = _uiRoot.transform.FindChild("Anim/City3").gameObject;
        CityBtn2 = _uiRoot.transform.FindChild("Anim/City2").gameObject;
        Spt_MyselfGuild = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuild").gameObject.GetComponent<UISprite>();
        Gobj_SignUp = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuild/SignUp").gameObject;
        Spt_MyselfGuildIcon = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildIcon").gameObject.GetComponent<UISprite>();
        Spt_MyselfGuidState = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildIcon/state").gameObject.GetComponent<UISprite>();
        Spt_MyselfQuality = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildIcon/MyselfQuality").gameObject.GetComponent<UISprite>();
        Spt_MyselfGuildBG = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildBG").gameObject.GetComponent<UISprite>();
        Lbl_SelfGuildNum = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildBG/SelfGuildNum").gameObject.GetComponent<UILabel>();
        Lbl_SelfGuildName = _uiRoot.transform.FindChild("Anim/MyselfGuildObj/MyselfGuildBG/SelfGuildName").gameObject.GetComponent<UILabel>();
        Spt_OtherGuild = _uiRoot.transform.FindChild("Anim/OtherGuild").gameObject.GetComponent<UISprite>();
        Spt_OtherGuildIcon = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildIcon").gameObject.GetComponent<UISprite>();
        Spt_OtherGuildState = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildIcon/state").gameObject.GetComponent<UISprite>();
        Spt_OtherQuality = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildIcon/OtherQuality").gameObject.GetComponent<UISprite>();
        Spt_OtherGuildBG = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildBG").gameObject.GetComponent<UISprite>();
        Lbl_OtherGuildNum = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildBG/OtherGuildNum").gameObject.GetComponent<UILabel>();
        Lbl_OtherGuildName = _uiRoot.transform.FindChild("Anim/OtherGuild/OtherGuildBG/OtherGuildName").gameObject.GetComponent<UILabel>();
        Spt_IntegralBG = _uiRoot.transform.FindChild("Anim/Integral/IntegralBG").gameObject.GetComponent<UISprite>();
        Lbl_IntegralLb = _uiRoot.transform.FindChild("Anim/Integral/IntegralBG/IntegralLb").gameObject.GetComponent<UILabel>();
        Lbl_SelfIntegralLb = _uiRoot.transform.FindChild("Anim/Integral/myself/SelfIntegralLb").gameObject.GetComponent<UILabel>();
        Lbl_SelfIntegralNum = _uiRoot.transform.FindChild("Anim/Integral/myself/SelfIntegralNum").gameObject.GetComponent<UILabel>();
        Lbl_OtherIntegralLb = _uiRoot.transform.FindChild("Anim/Integral/Other/OtherIntegralLb").gameObject.GetComponent<UILabel>();
        Lbl_OtherIntegralNum = _uiRoot.transform.FindChild("Anim/Integral/Other/OtherIntegralNum").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnRuleBtnLb.text = "争霸规则";
        Lbl_BtnHegemonyRankBtnLb.text = "争霸排行";
        Lbl_BtnKillRankLb.text = "击杀排行";
        Lbl_SelfGuildNum.text = "1244556";
        Lbl_SelfGuildName.text = "1244556";
        Lbl_OtherGuildNum.text = "1244556";
        Lbl_OtherGuildName.text = "1244556";
        Lbl_IntegralLb.text = "距本赛季第2场争霸战报名还有9天23:59:59";
        Lbl_SelfIntegralLb.text = "本季度军团积分:";
        Lbl_SelfIntegralNum.text = "9999999999";
        Lbl_OtherIntegralLb.text = "本场度军团积分：";
        Lbl_OtherIntegralNum.text = "555555555";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
