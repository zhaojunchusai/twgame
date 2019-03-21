using UnityEngine;
using System;
using System.Collections;

public class ChangeUnionBadgeView
{
    public static string UIName ="ChangeUnionBadgeView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_ChangeUnionBadgeView;
    public UISprite Spt_MaskBGSprite;
    public UILabel Lbl_TitleValue;
    public UISprite Spt_TitleLeft;
    public UISprite Spt_TitleRight;
    public UIButton Btn_Confirm;
    public UILabel Lbl_BtnConfirmConfirmFGSprite;
    public UISprite Spt_BtnConfirmConfirmBGSprite;
    public UISprite Spt_CostSprite;
    public UILabel Lbl_CostNumLabel;
    public UISprite Spt_BGSprite;
    public UISprite Spt_ContentSprite;
    public UISprite Spt_LaceLeft;
    public UISprite Spt_LaceRight;
    public UIPanel UIPanel_ScrollView;
    public UIScrollView ScrView_ScrollView;
    public UISprite Spt_BadgeItem;
    public UISprite Spt_BadgeIcon;
    public UISprite Spt_BadgeFrame;
    public UISprite Spt_BadgeMask;
    public UISprite Spt_BadgeLock;
    public UILabel Lbl_BadgeTime;
    public UISprite Spt_BadgeCheck;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/ChangeUnionBadgeView");
        UIPanel_ChangeUnionBadgeView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_TitleValue = _uiRoot.transform.FindChild("Content/Title/TitleValue").gameObject.GetComponent<UILabel>();
        Spt_TitleLeft = _uiRoot.transform.FindChild("Content/Title/TitleLeft").gameObject.GetComponent<UISprite>();
        Spt_TitleRight = _uiRoot.transform.FindChild("Content/Title/TitleRight").gameObject.GetComponent<UISprite>();
        Btn_Confirm = _uiRoot.transform.FindChild("Content/Confirm").gameObject.GetComponent<UIButton>();
        Lbl_BtnConfirmConfirmFGSprite = _uiRoot.transform.FindChild("Content/Confirm/ConfirmFGSprite").gameObject.GetComponent<UILabel>();
        Spt_BtnConfirmConfirmBGSprite = _uiRoot.transform.FindChild("Content/Confirm/ConfirmBGSprite").gameObject.GetComponent<UISprite>();
        Spt_CostSprite = _uiRoot.transform.FindChild("Content/CostSprite").gameObject.GetComponent<UISprite>();
        Lbl_CostNumLabel = _uiRoot.transform.FindChild("Content/CostSprite/CostNumLabel").gameObject.GetComponent<UILabel>();
        Spt_BGSprite = _uiRoot.transform.FindChild("Content/BackGround/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_ContentSprite = _uiRoot.transform.FindChild("Content/BackGround/ContentSprite").gameObject.GetComponent<UISprite>();
        Spt_LaceLeft = _uiRoot.transform.FindChild("Content/BackGround/LaceLeft").gameObject.GetComponent<UISprite>();
        Spt_LaceRight = _uiRoot.transform.FindChild("Content/BackGround/LaceRight").gameObject.GetComponent<UISprite>();
        UIPanel_ScrollView = _uiRoot.transform.FindChild("Content/Badges/ScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ScrollView = _uiRoot.transform.FindChild("Content/Badges/ScrollView").gameObject.GetComponent<UIScrollView>();
        Spt_BadgeItem = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeItem").gameObject.GetComponent<UISprite>();
        Spt_BadgeIcon = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeItem/BadgeIcon").gameObject.GetComponent<UISprite>();
        Spt_BadgeFrame = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeItem/BadgeFrame").gameObject.GetComponent<UISprite>();
        Spt_BadgeLock = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeItem/BadgeLock").gameObject.GetComponent<UISprite>();
        Lbl_BadgeTime = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeItem/BadgeTime").gameObject.GetComponent<UILabel>();
        Spt_BadgeCheck = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeCheck").gameObject.GetComponent<UISprite>();
        Spt_BadgeMask = _uiRoot.transform.FindChild("Content/Badges/ScrollView/BadgeCheck/BadgeMask").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleValue.text = "選擇徽章";
        Lbl_BtnConfirmConfirmFGSprite.text = "確定";
        Lbl_CostNumLabel.text = "0";
        Lbl_BadgeTime.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
