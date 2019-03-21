using UnityEngine;
using System;
using System.Collections;

public class GameActivityView
{
    public static string UIName ="GameActivityView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_GameActivityView;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBackground;
    public UILabel Lbl_BtnCloseLabel;
    public UIPanel UIPanel_DescScrollView;
    public UIScrollView ScrView_DescScrollView;
    public UITable Tbl_DescTable;
    public UIPanel UIPanel_ActivitesScrollView;
    public UIScrollView ScrView_ActivitesScrollView;
    public UIGrid Grd_ActivitesGrid;
    public GameObject Obj_BannerGroup;
    public UISprite Spt_BannerBG;
    public UISprite Spt_BannerIcon;
    public UISprite Spt_BannerDesc;
    public UISprite Spt_BannerTextBG;
    public UILabel Lbl_BannerTitle;
    public UILabel Lbl_BannerDesc;
    public GameObject Obj_GameActivitesItemSource;
    public GameObject Obj_AwardsItemSource;
    public GameObject Obj_BuyingItemSource;
    public GameObject Obj_SingleAwardSource;
    public GameObject Obj_BuyFoundSource;
    public GameObject Obj_BuyFoundCountSource;
    public GameObject Obj_TextDescSource;
    public GameObject Obj_ActivityRankSource;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/GameActivityView");
        UIPanel_GameActivityView = _uiRoot.GetComponent<UIPanel>();
        Btn_Close = _uiRoot.transform.FindChild("Layer1/Close").gameObject.GetComponent<UIButton>();
        Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Layer1/Close/Background").gameObject.GetComponent<UISprite>();
        UIPanel_DescScrollView = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_DescScrollView = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView").gameObject.GetComponent<UIScrollView>();
        Tbl_DescTable = _uiRoot.transform.FindChild("Layer1/DescGroup/DescScrollView/DescTable").gameObject.GetComponent<UITable>();
        UIPanel_ActivitesScrollView = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ActivitesScrollView = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_ActivitesGrid = _uiRoot.transform.FindChild("Layer1/ActivitesGroup/ActivitesScrollView/ActivitesGrid").gameObject.GetComponent<UIGrid>();

        Obj_BannerGroup = _uiRoot.transform.FindChild("Layer1/BannerGroup").gameObject;
        Spt_BannerBG = Obj_BannerGroup.transform.FindChild("BannerBGSprite").gameObject.GetComponent<UISprite>();
        Spt_BannerIcon = Obj_BannerGroup.transform.FindChild("BannerIconSprite").gameObject.GetComponent<UISprite>();
        Spt_BannerDesc = Obj_BannerGroup.transform.FindChild("BannerDescSprite").gameObject.GetComponent<UISprite>();
        Spt_BannerTextBG = Obj_BannerGroup.transform.FindChild("BannerTextBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_BannerTitle = Obj_BannerGroup.transform.FindChild("BannerTitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_BannerDesc = Obj_BannerGroup.transform.FindChild("BannerDescLabel").gameObject.GetComponent<UILabel>();

        Obj_GameActivitesItemSource = _uiRoot.transform.FindChild("Layer1/Source/GameActivitesItem").gameObject;
        Obj_AwardsItemSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityAwardsItem").gameObject;
        Obj_BuyingItemSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityBuyingItem").gameObject;
        Obj_SingleAwardSource = _uiRoot.transform.FindChild("Layer1/Source/ActivitySingleAwardItem").gameObject;
        Obj_BuyFoundSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityBuyFundItem").gameObject;
        Obj_BuyFoundCountSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityBuyFundCountItem").gameObject;
        Obj_TextDescSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityTextDescItem").gameObject;
        Obj_ActivityRankSource = _uiRoot.transform.FindChild("Layer1/Source/ActivityRankItem").gameObject;
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnCloseLabel.text = "";
        Lbl_BannerTitle.text = "";
        Lbl_BannerDesc.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
