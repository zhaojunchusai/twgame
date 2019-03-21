using UnityEngine;
using System;
using System.Collections;

public class RuleView
{
    public static string UIName = "RuleView";
    public GameObject _uiRoot;
    public UIScrollView ScrView_RuleDesc;
    public UITable UITable_Rule;
    public UILabel Lbl_RankDesc;
    public UILabel Lbl_RuleTitle;
    public UISprite Spt_Sprite;
    public UISprite Spt_RuleDescSprite;
    //public UITable UITable_GetAwards;
    public GameObject Gobj_AwardGroup;
    public UILabel Lbl_Rank;
    //public UITable UITable_Awards;
    public UILabel Lbl_Title;
    public UISprite Spt_Mask;
    public GameObject Gobj_RuleDescGroup;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/RuleView");
        ScrView_RuleDesc = _uiRoot.transform.FindChild("Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        UITable_Rule = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule").gameObject.GetComponent<UITable>();
        Lbl_RankDesc = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RankDesc").gameObject.GetComponent<UILabel>();
        Lbl_RuleTitle = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/RuleTitle").gameObject.GetComponent<UILabel>();
        Spt_Sprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/Sprite").gameObject.GetComponent<UISprite>();
        Spt_RuleDescSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleDescSprite").gameObject.GetComponent<UISprite>();
        //UITable_GetAwards = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/GetAwards").gameObject.GetComponent<UITable>();
        Gobj_AwardGroup = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/GetAwards/gobj_AwardGroup").gameObject;
        Lbl_Rank = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/GetAwards/gobj_AwardGroup/Rank").gameObject.GetComponent<UILabel>();
        //UITable_Awards = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/GetAwards/gobj_AwardGroup/Awards").gameObject.GetComponent<UITable>();
        Gobj_RuleDescGroup = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup").gameObject;
        Lbl_Title = _uiRoot.transform.FindChild("Anim/BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_RankDesc.text = "";
        Lbl_RuleTitle.text = "";
        Lbl_Rank.text = "第20名:";
        Lbl_Title.text = "演武规则";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
