using UnityEngine;
using System;
using System.Collections;

public class UnionDonationRecordView
{
    public static string UIName ="UnionDonationRecordView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_UnionDonationRecordView;
    public UILabel Lbl_Title;
    public UISprite Spt_Mask;
    public UIPanel UIPanel_Content;
    public UIScrollView ScrView_Content;
    public UIGrid Grd_Content;
    public GameObject Gobj_RecItem;
    public UIWrapContent UIWrapContent_Content;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionDonationRecordView");
        UIPanel_UnionDonationRecordView = _uiRoot.GetComponent<UIPanel>();
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        UIPanel_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIPanel>();
        ScrView_Content = _uiRoot.transform.FindChild("Content").gameObject.GetComponent<UIScrollView>();
        Grd_Content = _uiRoot.transform.FindChild("Content/Content").gameObject.GetComponent<UIGrid>();
        UIWrapContent_Content = _uiRoot.transform.FindChild("Content/Content").gameObject.GetComponent<UIWrapContent>();
        Gobj_RecItem = _uiRoot.transform.FindChild("Pre/DonationRecItem").gameObject;
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "捐献记录";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
