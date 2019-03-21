using UnityEngine;
using System;
using System.Collections;

public class ChangeUnionIconView
{
    public static string UIName ="ChangeUnionIconView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_ChangeUnionIconView;
    public UILabel Lbl_CostLabel;
    public UISprite Spt_CostSprite;
    public UILabel Lbl_CostNumLabel;
    public UIButton Btn_CancelButton;
    public UILabel Lbl_BtnCancelButtonCancelFGSprite;
    public UISprite Spt_BtnCancelButtonCancelBGSprite;
    public UIButton Btn_ConfirmButton;
    public UILabel Lbl_BtnConfirmButtonConfirmFGSprite;
    public UISprite Spt_BtnConfirmButtonConfirmBGSprite;
    public UILabel Lbl_Title;
    public UISprite Spt_BGSprite;
    public UISprite Spt_ContentSprite;
    public UIPanel UIPanel_Icons;
    public UIScrollView ScrView_Icons;
    public UIGrid Grd_Icons;
    public UISprite Spt_MaskBGSprite;

    public GameObject Gobj_UnionIcon;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/ChangeUnionIconView");
        UIPanel_ChangeUnionIconView = _uiRoot.GetComponent<UIPanel>();
        Lbl_CostLabel = _uiRoot.transform.FindChild("OffsetRoot/CostLabel").gameObject.GetComponent<UILabel>();
        Spt_CostSprite = _uiRoot.transform.FindChild("OffsetRoot/CostSprite").gameObject.GetComponent<UISprite>();
        Lbl_CostNumLabel = _uiRoot.transform.FindChild("OffsetRoot/CostNumLabel").gameObject.GetComponent<UILabel>();
        Btn_CancelButton = _uiRoot.transform.FindChild("OffsetRoot/CancelButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnCancelButtonCancelFGSprite = _uiRoot.transform.FindChild("OffsetRoot/CancelButton/CancelFGSprite").gameObject.GetComponent<UILabel>();
        Spt_BtnCancelButtonCancelBGSprite = _uiRoot.transform.FindChild("OffsetRoot/CancelButton/CancelBGSprite").gameObject.GetComponent<UISprite>();
        Btn_ConfirmButton = _uiRoot.transform.FindChild("OffsetRoot/ConfirmButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnConfirmButtonConfirmFGSprite = _uiRoot.transform.FindChild("OffsetRoot/ConfirmButton/ConfirmFGSprite").gameObject.GetComponent<UILabel>();
        Spt_BtnConfirmButtonConfirmBGSprite = _uiRoot.transform.FindChild("OffsetRoot/ConfirmButton/ConfirmBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("OffsetRoot/Title").gameObject.GetComponent<UILabel>();
        Spt_BGSprite = _uiRoot.transform.FindChild("OffsetRoot/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_ContentSprite = _uiRoot.transform.FindChild("OffsetRoot/ContentSprite").gameObject.GetComponent<UISprite>();
        UIPanel_Icons = _uiRoot.transform.FindChild("OffsetRoot/Icons").gameObject.GetComponent<UIPanel>();
        ScrView_Icons = _uiRoot.transform.FindChild("OffsetRoot/Icons").gameObject.GetComponent<UIScrollView>();
        Grd_Icons = _uiRoot.transform.FindChild("OffsetRoot/Icons/Icons").gameObject.GetComponent<UIGrid>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();

        Gobj_UnionIcon = _uiRoot.transform.FindChild("Pre/UnionIconItem").gameObject;
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_CostLabel.text = "修改徽章将消耗";
        //Lbl_CostNumLabel.text = "200000";
        //Lbl_BtnCancelButtonCancelFGSprite.text = "取消";
        //Lbl_BtnConfirmButtonConfirmFGSprite.text = "确定";
        //Lbl_Title.text = "选择徽章";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
