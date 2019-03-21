using UnityEngine;
using System;
using System.Collections;

public class ExoticAdvantureView
{
    public static string UIName = "ExoticAdvantureView";
    public GameObject _uiRoot;
    public UILabel Lbl_ChapterName;
    public GameObject Gobj_GateGroup;
    public GameObject Gobj_Gate;
    public UIButton Btn_Exit;
    public UIButton Btn_LeftArrow;
    public UIButton Btn_RightArrow;
    public UITexture Tex_ChapterBg;
    public UIButton Btn_Reset;
    public UILabel Lbl_BtnResetLabel;
    public UIButton Btn_AdvantureRole;
    public UILabel Lbl_BtnAdvantureRoleLabel;
    public UIButton Btn_InjureRank;
    public UILabel Lbl_BtnInjureRankLabel;
    public UIButton Btn_HighAwards;
    public UILabel Lbl_AwardsNum;
    public UISprite Spt_Chest;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ExoticAdvantureView");
        Lbl_ChapterName = _uiRoot.transform.FindChild("Anim/ChapterTitleGroup/TitleGroup/ChapterName").gameObject.GetComponent<UILabel>();
        Gobj_GateGroup = _uiRoot.transform.FindChild("Anim/gobj_GateGroup").gameObject;
        Gobj_Gate = _uiRoot.transform.FindChild("Anim/gobj_GateGroup/gobj_Gate").gameObject;
        Btn_Exit = _uiRoot.transform.FindChild("Anim/Exit").gameObject.GetComponent<UIButton>();
        Btn_LeftArrow = _uiRoot.transform.FindChild("Anim/ArrowGroup/LeftArrow").gameObject.GetComponent<UIButton>();
        Btn_RightArrow = _uiRoot.transform.FindChild("Anim/ArrowGroup/RightArrow").gameObject.GetComponent<UIButton>();
        Tex_ChapterBg = _uiRoot.transform.FindChild("Anim/TexGroup/ChapterBg").gameObject.GetComponent<UITexture>();
        Btn_Reset = _uiRoot.transform.FindChild("Anim/BottumGroup/Reset").gameObject.GetComponent<UIButton>();
        Lbl_BtnResetLabel = _uiRoot.transform.FindChild("Anim/BottumGroup/Reset/Label").gameObject.GetComponent<UILabel>();
        Btn_AdvantureRole = _uiRoot.transform.FindChild("Anim/BottumGroup/AdvantureRole").gameObject.GetComponent<UIButton>();
        Lbl_BtnAdvantureRoleLabel = _uiRoot.transform.FindChild("Anim/BottumGroup/AdvantureRole/Label").gameObject.GetComponent<UILabel>();
        Btn_InjureRank = _uiRoot.transform.FindChild("Anim/BottumGroup/InjureRank").gameObject.GetComponent<UIButton>();
        Lbl_BtnInjureRankLabel = _uiRoot.transform.FindChild("Anim/BottumGroup/InjureRank/Label").gameObject.GetComponent<UILabel>();
        Btn_HighAwards = _uiRoot.transform.FindChild("Anim/HighAwards").gameObject.GetComponent<UIButton>();
        Lbl_AwardsNum = _uiRoot.transform.FindChild("Anim/HighAwards/Chest/AwardsNum").gameObject.GetComponent<UILabel>();
        Spt_Chest = _uiRoot.transform.FindChild("Anim/HighAwards/Chest/Chest").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ChapterName.text = "";
        Lbl_BtnResetLabel.text = "重置";
        Lbl_BtnAdvantureRoleLabel.text = "探险规则";
        Lbl_BtnInjureRankLabel.text = "伤害排行";
        Lbl_AwardsNum.text = "18";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
