using UnityEngine;
using System;
using System.Collections;

public class CreateUnionView
{
    public static string UIName ="CreateUnionView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_CreateUnionView;
    public UIButton Btn_CreateUnion;
    public UIButton Btn_ChooseBadge;
    public UILabel Lbl_BtnCreateUnionLabel;
    public UIPanel UIPanel_Icons;
    public UIGrid Grd_Icons;
    public UIInput Ipt_UnionName;
    public UILabel Lbl_InputUnionNameLabel;
    public UIInput Ipt_Notice;
    public UILabel Lbl_InputNoticeLabel;
    public UILabel Lbl_CostTip;
    public UISprite Spt_CostIcon;
    public UILabel Lbl_CostNum;
    public UISprite Spt_Mask;
    public UILabel Lbl_Title;
    public UILabel Lbl_IconTitle;
    public GameObject Gobj_UnionIcon;
    public UISprite Spt_UnionIcon;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/CreateUnionView");
        UIPanel_CreateUnionView = _uiRoot.GetComponent<UIPanel>();
        Btn_CreateUnion = _uiRoot.transform.FindChild("CreateUnion").gameObject.GetComponent<UIButton>();
        Btn_ChooseBadge = _uiRoot.transform.FindChild("Icons/Icons/Pre/ChooseBadge").gameObject.GetComponent<UIButton>();
        Lbl_BtnCreateUnionLabel = _uiRoot.transform.FindChild("CreateUnion/Label").gameObject.GetComponent<UILabel>();
        UIPanel_Icons = _uiRoot.transform.FindChild("Icons").gameObject.GetComponent<UIPanel>();
        Grd_Icons = _uiRoot.transform.FindChild("Icons/Icons").gameObject.GetComponent<UIGrid>();
        Ipt_UnionName = _uiRoot.transform.FindChild("UnionName").gameObject.GetComponent<UIInput>();
        Lbl_InputUnionNameLabel = _uiRoot.transform.FindChild("UnionName/Label").gameObject.GetComponent<UILabel>();
        Ipt_Notice = _uiRoot.transform.FindChild("Notice").gameObject.GetComponent<UIInput>();
        Lbl_InputNoticeLabel = _uiRoot.transform.FindChild("Notice/Label").gameObject.GetComponent<UILabel>();
        Lbl_CostTip = _uiRoot.transform.FindChild("CostTip").gameObject.GetComponent<UILabel>();
        Spt_CostIcon = _uiRoot.transform.FindChild("CostIcon").gameObject.GetComponent<UISprite>();
        Lbl_CostNum = _uiRoot.transform.FindChild("CostNum").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Lbl_IconTitle = _uiRoot.transform.FindChild("IconTitle").gameObject.GetComponent<UILabel>();

        Gobj_UnionIcon = _uiRoot.transform.FindChild("Icons/Icons/Pre/UnionIconItem").gameObject;
        Spt_UnionIcon = _uiRoot.transform.FindChild("Icons/Icons/Pre/UnionIconItem/Icon").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnCreateUnionLabel.text = "创建军团";
        Lbl_InputUnionNameLabel.text = "点击输入军团名，最长6个字";
        Lbl_InputNoticeLabel.text = "点击输入公告，最长50个字";
        Lbl_CostTip.text = "创建军团将消耗:";
        Lbl_CostNum.text = "99999";
        Lbl_Title.text = "创建军团";
        Lbl_IconTitle.text = "选择团徽";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
