using UnityEngine;
using System;
using System.Collections;

public class UnionMemberInfoView
{
    public static string UIName ="UnionMemberInfoView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionMemberInfoView;
    public UISprite Spt_Mask;
    public UISprite Spt_BackGround;
    public UISprite Spt_MidGround;
    public UISprite Spt_TitleLeft;
    public UISprite Spt_TitleRight;
    public UILabel Lbl_TitleValue;
    public UIPanel UIPanel_ScrollView;
    public UIScrollView ScrView_ScrollView;
    public UIGrid Grd_Grid;
    public UISprite Spt_MemberItem;
    public UISprite Spt_ItemSplit;
    public UISprite Spt_HeadIcon;
    public UISprite Spt_HeadFrame;
    public UISprite Spt_LevelBack;
    public UILabel Lbl_LevelValue;
    public UISprite Spt_HeadBack;
    public UILabel Lbl_ItemName;
    public UILabel Lbl_ItemJob;
    public UILabel Lbl_ItemActive;
    public UILabel Lbl_ItemOffLine;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionMemberInfoView");
        UIPanel_UnionMemberInfoView = _uiRoot.GetComponent<UIPanel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Spt_BackGround = _uiRoot.transform.FindChild("Content/BackGround").gameObject.GetComponent<UISprite>();
        Spt_MidGround = _uiRoot.transform.FindChild("Content/MidGround").gameObject.GetComponent<UISprite>();
        Spt_TitleLeft = _uiRoot.transform.FindChild("Content/Title/TitleLeft").gameObject.GetComponent<UISprite>();
        Spt_TitleRight = _uiRoot.transform.FindChild("Content/Title/TitleRight").gameObject.GetComponent<UISprite>();
        Lbl_TitleValue = _uiRoot.transform.FindChild("Content/Title/TitleValue").gameObject.GetComponent<UILabel>();
        UIPanel_ScrollView = _uiRoot.transform.FindChild("Content/Items/ScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ScrollView = _uiRoot.transform.FindChild("Content/Items/ScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid").gameObject.GetComponent<UIGrid>();
        Spt_MemberItem = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem").gameObject.GetComponent<UISprite>();
        Spt_ItemSplit = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemSplit").gameObject.GetComponent<UISprite>();
        Spt_HeadIcon = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemHeadInfo/HeadIcon").gameObject.GetComponent<UISprite>();
        Spt_HeadFrame = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemHeadInfo/HeadFrame").gameObject.GetComponent<UISprite>();
        Spt_LevelBack = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemHeadInfo/LevelBack").gameObject.GetComponent<UISprite>();
        Lbl_LevelValue = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemHeadInfo/LevelBack/LevelValue").gameObject.GetComponent<UILabel>();
        Spt_HeadBack = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemHeadInfo/HeadBack").gameObject.GetComponent<UISprite>();
        Lbl_ItemName = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemName").gameObject.GetComponent<UILabel>();
        Lbl_ItemJob = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemJob").gameObject.GetComponent<UILabel>();
        Lbl_ItemActive = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemActive").gameObject.GetComponent<UILabel>();
        Lbl_ItemOffLine = _uiRoot.transform.FindChild("Content/Items/ScrollView/Grid/MemberItem/ItemOffLine").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleValue.text = "軍團成員";
        Lbl_LevelValue.text = "";
        Lbl_ItemName.text = "";
        Lbl_ItemJob.text = "";
        Lbl_ItemActive.text = "";
        Lbl_ItemOffLine.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
