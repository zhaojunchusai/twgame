using UnityEngine;
using System;
using System.Collections;

public class SeeDetailView
{
    public static string UIName ="SeeDetailView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_SeeDetailView;
    public GameObject Gobj_ItemDesPage;
    public UISprite Spt_Icon;
    public UISprite Spt_BG;
    public UISprite Spt_Quality;
    public UISprite Spt_ChipMark;
    public UISprite Spt_ItemDesPageBG;
    public UILabel Lbl_ItemDesPageName;
    public UILabel Lbl_ItemDesPageDesc;
    public UISprite Spt_ItemDesPageFG;
    public UISprite Spt_ItemDesPageFGUp;
    public UILabel Lbl_ItemDesPageOwnNumTip;
    public UILabel Lbl_ItemDesPageOwnNum;
    public GameObject Gobj_SuitNameGroup;
    public UILabel Lbl_SuitName;

    public GameObject Gobj_StrPage;
    public UISprite Spt_StrPageBG;
    public UILabel Lbl_StrPageContent;
    public TweenScale Item_TScale, Str_TScale;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SeeDetailView");
        //UIPanel_SeeDetailView = _uiRoot.GetComponent<UIPanel>();
        Str_TScale = _uiRoot.transform.FindChild("gobj_StrPage").gameObject.GetComponent<TweenScale>();
        Item_TScale = _uiRoot.transform.FindChild("gobj_ItemDesPage").gameObject.GetComponent<TweenScale>();
        Gobj_ItemDesPage = _uiRoot.transform.FindChild("gobj_ItemDesPage").gameObject;
        Spt_Icon = _uiRoot.transform.FindChild("gobj_ItemDesPage/Prop/Icon").gameObject.GetComponent<UISprite>();
        Spt_BG = _uiRoot.transform.FindChild("gobj_ItemDesPage/Prop/BG").gameObject.GetComponent<UISprite>();
        Spt_Quality = _uiRoot.transform.FindChild("gobj_ItemDesPage/Prop/Quality").gameObject.GetComponent<UISprite>();
        Spt_ChipMark = _uiRoot.transform.FindChild("gobj_ItemDesPage/Prop/Mark").gameObject.GetComponent<UISprite>();
        Spt_ItemDesPageBG = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageBG").gameObject.GetComponent<UISprite>();
        Lbl_ItemDesPageName = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageName").gameObject.GetComponent<UILabel>();
        Lbl_ItemDesPageDesc = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageDesc").gameObject.GetComponent<UILabel>();
        Spt_ItemDesPageFG = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageFG").gameObject.GetComponent<UISprite>();
        //Spt_ItemDesPageFGUp = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageFGUp").gameObject.GetComponent<UISprite>();
        Lbl_ItemDesPageOwnNumTip = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageOwnNumTip").gameObject.GetComponent<UILabel>();
        Lbl_ItemDesPageOwnNum = _uiRoot.transform.FindChild("gobj_ItemDesPage/ItemDesPageOwnNum").gameObject.GetComponent<UILabel>();
        Gobj_StrPage = _uiRoot.transform.FindChild("gobj_StrPage").gameObject;
        Spt_StrPageBG = _uiRoot.transform.FindChild("gobj_StrPage/StrPageBG").gameObject.GetComponent<UISprite>();
        Lbl_StrPageContent = _uiRoot.transform.FindChild("gobj_StrPage/StrPageContent").gameObject.GetComponent<UILabel>();
        Gobj_SuitNameGroup = _uiRoot.transform.FindChild("gobj_ItemDesPage/SuitNameGroup").gameObject;
        Lbl_SuitName = _uiRoot.transform.FindChild("gobj_ItemDesPage/SuitNameGroup/Label").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ItemDesPageName.text = "道具名道具名道";
        Lbl_ItemDesPageDesc.text = "道具名道具名道道具名道具名道道具名道具名道具名";
        Lbl_ItemDesPageOwnNumTip.text = "当前拥有";
        Lbl_ItemDesPageOwnNum.text = "999";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
