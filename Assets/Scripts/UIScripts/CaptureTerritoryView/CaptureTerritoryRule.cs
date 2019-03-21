using UnityEngine;
using System;
using System.Collections;

public class CaptureTerritoryRule
{
    public static string UIName ="CaptureTerritoryRule";
    public GameObject _uiRoot;
    public UIPanel UIPanel_CaptureTerritoryRule;
    public UIPanel UIPanel_RuleDesc;
    public UIScrollView ScrView_RuleDesc;
    public UITable UITable_Rule;
    public UISprite Spt_SelfRankSprite;
    //public UISprite Spt_LeftSprite;
    //public UISprite Spt_RightSprite;
    public UILabel Lbl_SelfRankTitle;
    //public UISprite Spt_Sprite;
    public UILabel Lbl_RankDesc;
    public GameObject Gobj_GetAwardsGroup;

    //public UISprite Spt_QualitySprite;
    //public UISprite Spt_ItemBgSprite;
    //public UISprite Spt_IconSprite;
    //public UILabel Lbl_CountLabel;

    //public UISprite Spt_LeftSprite;
    //public UISprite Spt_RightSprite;
    public UILabel Lbl_AwardTitle;
    //public UISprite Spt_Sprite;
    //public UISprite Spt_LeftSprite;
    //public UISprite Spt_RightSprite;
    public UILabel Lbl_RuleTitle;
    //public UISprite Spt_Sprite;
    public UILabel Lbl_RuleDesc;
    public UISprite Spt_RuleDescSprite;
    public UISprite Spt_AwardDescSprite;
    public GameObject Gobj_GetAwards;
    public UIGrid Grd_GetAwards;
    //public GameObject Gobj_AwardGroup;

    //public UILabel Lbl_PVPRank;
    //public UIGrid Grd_Awards;
    //public GameObject Gobj_AwardComp;
    //public UISprite Spt_QualitySprite;
    //public UISprite Spt_ItemBgSprite;
    //public UISprite Spt_IconSprite;
    //public UILabel Lbl_CountLabel;

    //public UISprite Spt_LeftSprite;
    //public UISprite Spt_RightSprite;
    public UILabel Lbl_GetAwardTitle;
    public UISprite Spt_Sprite;
    //public UISprite Spt_Bg;
    public UILabel Lbl_Title;
    //public UISprite Spt_LeftTitleBG;
    //public UISprite Spt_RightTitleBG;
    //public UISprite Spt_FG;
    public UISprite Spt_Mask;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CaptureTerritoryRule");
        UIPanel_CaptureTerritoryRule = _uiRoot.GetComponent<UIPanel>();
        UIPanel_RuleDesc = _uiRoot.transform.FindChild("Anim/RuleDesc").gameObject.GetComponent<UIPanel>();
        ScrView_RuleDesc = _uiRoot.transform.FindChild("Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        UITable_Rule = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule").gameObject.GetComponent<UITable>();
        Spt_SelfRankSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/SelfRankSprite").gameObject.GetComponent<UISprite>();
        //Spt_LeftSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/RankTitleGroup/LeftSprite").gameObject.GetComponent<UISprite>();
        //Spt_RightSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/RankTitleGroup/RightSprite").gameObject.GetComponent<UISprite>();
        Lbl_SelfRankTitle = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/RankTitleGroup/SelfRankTitle").gameObject.GetComponent<UILabel>();
        Spt_Sprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/RankTitleGroup/Sprite").gameObject.GetComponent<UISprite>();
        Lbl_RankDesc = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/RankDesc").gameObject.GetComponent<UILabel>();
        Gobj_GetAwardsGroup = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup").gameObject;

        //Spt_QualitySprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/GetAwardsGroup/gobj_AwardComp/QualitySprite").gameObject.GetComponent<UISprite>();
        //Spt_ItemBgSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/GetAwardsGroup/gobj_AwardComp/ItemBgSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/GetAwardsGroup/gobj_AwardComp/IconSprite").gameObject.GetComponent<UISprite>();
        //Lbl_CountLabel = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/GetAwardsGroup/gobj_AwardComp/CountLabel").gameObject.GetComponent<UILabel>();
        //Spt_LeftSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/AwardsTitleGroup/LeftSprite").gameObject.GetComponent<UISprite>();
        //Spt_RightSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/AwardsTitleGroup/RightSprite").gameObject.GetComponent<UISprite>();
        Lbl_AwardTitle = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/AwardsTitleGroup/AwardTitle").gameObject.GetComponent<UILabel>();
        Spt_Sprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/SelfRank/GetAwardsGroup/AwardsTitleGroup/Sprite").gameObject.GetComponent<UISprite>();
        //Spt_LeftSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/LeftSprite").gameObject.GetComponent<UISprite>();
        //Spt_RightSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/RightSprite").gameObject.GetComponent<UISprite>();
        Lbl_RuleTitle = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/RuleTitle").gameObject.GetComponent<UILabel>();
        //Spt_Sprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleTitleGroup/Sprite").gameObject.GetComponent<UISprite>();
        Lbl_RuleDesc = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleDesc").gameObject.GetComponent<UILabel>();
        Spt_RuleDescSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/RuleDescGroup/RuleDescSprite").gameObject.GetComponent<UISprite>();
        Spt_AwardDescSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/AwardDescSprite").gameObject.GetComponent<UISprite>();
        Gobj_GetAwards = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup").gameObject;
        Grd_GetAwards = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards").gameObject.GetComponent<UIGrid>();
        //Gobj_AwardGroup = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup").gameObject;

        //Lbl_PVPRank = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/PVPRank").gameObject.GetComponent<UILabel>();
        //Grd_Awards = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards").gameObject.GetComponent<UIGrid>();
        //Gobj_AwardComp = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards/gobj_AwardComp").gameObject;
        //Spt_QualitySprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards/gobj_AwardComp/QualitySprite").gameObject.GetComponent<UISprite>();
        //Spt_ItemBgSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards/gobj_AwardComp/ItemBgSprite").gameObject.GetComponent<UISprite>();
        //Spt_IconSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards/gobj_AwardComp/IconSprite").gameObject.GetComponent<UISprite>();
        //Lbl_CountLabel = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwards/gobj_AwardGroup/Awards/gobj_AwardComp/CountLabel").gameObject.GetComponent<UILabel>();
        
        //Spt_LeftSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwardTitleGroup/LeftSprite").gameObject.GetComponent<UISprite>();
        //Spt_RightSprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwardTitleGroup/RightSprite").gameObject.GetComponent<UISprite>();
        Lbl_GetAwardTitle = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwardTitleGroup/GetAwardTitle").gameObject.GetComponent<UILabel>();
        Spt_Sprite = _uiRoot.transform.FindChild("Anim/RuleDesc/Rule/AwardDescGroup/GetAwardTitleGroup/Sprite").gameObject.GetComponent<UISprite>();
        //Spt_Bg = _uiRoot.transform.FindChild("Anim/BGGroup/Bg").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("Anim/BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        //Spt_LeftTitleBG = _uiRoot.transform.FindChild("Anim/BGGroup/TitleGroup/LeftTitleBG").gameObject.GetComponent<UISprite>();
        //Spt_RightTitleBG = _uiRoot.transform.FindChild("Anim/BGGroup/TitleGroup/RightTitleBG").gameObject.GetComponent<UISprite>();
        //Spt_FG = _uiRoot.transform.FindChild("Anim/BGGroup/FG").gameObject.GetComponent<UISprite>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SelfRankTitle.text = ConstString.LBL_YOUR_RANK;
        Lbl_RankDesc.text = "";
        //Lbl_CountLabel.text = "*25";
        Lbl_AwardTitle.text = ConstString.LBL_GET_AWARD;
        Lbl_RuleTitle.text = ConstString.TITLE_RULE;
        Lbl_RuleDesc.text = "";
        //Lbl_PVPRank.text = "第20名:";
        //Lbl_CountLabel.text = "*25";
        Lbl_GetAwardTitle.text = ConstString.LBL_AWARD;
        Lbl_Title.text = ConstString.TITLE_RULE;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
