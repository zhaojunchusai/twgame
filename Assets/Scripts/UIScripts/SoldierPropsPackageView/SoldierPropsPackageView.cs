using UnityEngine;
using System;
using System.Collections;

public class SoldierPropsPackageView
{
    public static string UIName = "SoldierPropsPackageView";
    public GameObject _uiRoot;
    public UIScrollView ScrView_ScorllView;
    public UIGrid Grd_ItemGrid;
    public UIWrapContent UIWrapContent_ItemGrid;
    public UIButton Btn_SummonButton;
    public UISprite Spt_MaskBGSprite;
    public GameObject Gobj_OwnSoldierComp;
    public UILabel Lbl_Title;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/SoldierPropsPackageView");
        ScrView_ScorllView = _uiRoot.transform.FindChild("OffsetRoot/ScorllView").gameObject.GetComponent<UIScrollView>();
        Grd_ItemGrid = _uiRoot.transform.FindChild("OffsetRoot/ScorllView/ItemGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_ItemGrid = _uiRoot.transform.FindChild("OffsetRoot/ScorllView/ItemGrid").gameObject.GetComponent<UIWrapContent>();
        Btn_SummonButton = _uiRoot.transform.FindChild("OffsetRoot/SummonButton").gameObject.GetComponent<UIButton>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Gobj_OwnSoldierComp = _uiRoot.transform.FindChild("Source/gobj_OwnSoldierComp").gameObject;
        Lbl_Title = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
    }



    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
