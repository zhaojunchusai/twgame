using UnityEngine;
using System;
using System.Collections;

public class WalkthroughView
{
    public static string UIName ="WalkthroughView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_PanelBack;
    public UISprite Spt_BackGround;
    public UISprite Spt_BackFloor;
    public UIButton Btn_BackClose;
    public UISprite Spt_BtnBackCloseBackground;
    public UIPanel UIPanel_PanelLeft;
    public UISprite Spt_LeftFloor;
    public UIPanel UIPanel_LeftScrollView;
    public UIScrollView ScrView_LeftScrollView;
    public UIGrid Grd_LeftGrid;
    public UISprite Spt_LeftItem_Back;
    public UILabel Lbl_LeftItem_Title;
    public UISprite Spt_LeftItem_Click;
    public UIPanel UIPanel_PanelRight;
    public UISprite Spt_RightFloot;
    public UILabel Lbl_RightTitle;
    public UISprite Spt_RightFrame1;
    public UISprite Spt_RightFrame2;
    public UISprite Spt_RightFrame3;
    public UISprite Spt_RightFrame4;
    public UIPanel UIPanel_RightScrollView;
    public UIScrollView ScrView_RightScrollView;
    public UIGrid Grd_RightGrid;
    public UISprite Spt_RightItem_IconBack;
    public UISprite Spt_RightItem_IconMask;
    public UISprite Spt_RightItem_Icon;
    public UILabel Lbl_RightItem_ConditionHint;
    public UILabel Lbl_RightContent;
    public UILabel Lbl_RightHint;
    public UISprite Spt_RightSplit;

    public Transform Trans_LeftItem;
    public Transform Trans_RightItemJumpTo;
    public Transform Trans_SingleJumpToItem;
    public Transform Trans_RightItemContent;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/WalkthroughView");
        UIPanel_PanelBack = _uiRoot.transform.FindChild("PanelBack").gameObject.GetComponent<UIPanel>();
        Spt_BackGround = _uiRoot.transform.FindChild("PanelBack/BackGround").gameObject.GetComponent<UISprite>();
        Spt_BackFloor = _uiRoot.transform.FindChild("PanelBack/BackFloor").gameObject.GetComponent<UISprite>();
        Btn_BackClose = _uiRoot.transform.FindChild("PanelBack/BackClose").gameObject.GetComponent<UIButton>();
        Spt_BtnBackCloseBackground = _uiRoot.transform.FindChild("PanelBack/BackClose/Background").gameObject.GetComponent<UISprite>();
        UIPanel_PanelLeft = _uiRoot.transform.FindChild("PanelLeft").gameObject.GetComponent<UIPanel>();
        Spt_LeftFloor = _uiRoot.transform.FindChild("PanelLeft/LeftFloor").gameObject.GetComponent<UISprite>();
        UIPanel_LeftScrollView = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_LeftScrollView = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_LeftGrid = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView/LeftGrid").gameObject.GetComponent<UIGrid>();
        Spt_LeftItem_Back = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView/LeftGrid/LeftItem/LeftItem_Back").gameObject.GetComponent<UISprite>();
        Lbl_LeftItem_Title = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView/LeftGrid/LeftItem/LeftItem_Title").gameObject.GetComponent<UILabel>();
        Spt_LeftItem_Click = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView/LeftGrid/LeftItem_Click").gameObject.GetComponent<UISprite>();
        UIPanel_PanelRight = _uiRoot.transform.FindChild("PanelRight").gameObject.GetComponent<UIPanel>();
        Spt_RightFloot = _uiRoot.transform.FindChild("PanelRight/RightFloot").gameObject.GetComponent<UISprite>();
        Lbl_RightTitle = _uiRoot.transform.FindChild("PanelRight/RightTitle").gameObject.GetComponent<UILabel>();
        Spt_RightFrame1 = _uiRoot.transform.FindChild("PanelRight/RightFrame1").gameObject.GetComponent<UISprite>();
        Spt_RightFrame2 = _uiRoot.transform.FindChild("PanelRight/RightFrame2").gameObject.GetComponent<UISprite>();
        Spt_RightFrame3 = _uiRoot.transform.FindChild("PanelRight/RightFrame3").gameObject.GetComponent<UISprite>();
        Spt_RightFrame4 = _uiRoot.transform.FindChild("PanelRight/RightFrame4").gameObject.GetComponent<UISprite>();
        UIPanel_RightScrollView = _uiRoot.transform.FindChild("PanelRight/RightScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_RightScrollView = _uiRoot.transform.FindChild("PanelRight/RightScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_RightGrid = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid").gameObject.GetComponent<UIGrid>();
        Spt_RightItem_IconBack = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightItem_SingleInfo/RightItem_IconBack").gameObject.GetComponent<UISprite>();
        Spt_RightItem_IconMask = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightItem_SingleInfo/RightItem_IconBack/RightItem_IconMask").gameObject.GetComponent<UISprite>();
        Spt_RightItem_Icon = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightItem_SingleInfo/RightItem_IconBack/RightItem_Icon").gameObject.GetComponent<UISprite>();
        Lbl_RightItem_ConditionHint = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightItem_SingleInfo/RightItem_IconBack/RightItem_ConditionHint").gameObject.GetComponent<UILabel>();
        Lbl_RightContent = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemContent/RightContent").gameObject.GetComponent<UILabel>();
        Lbl_RightHint = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightHint").gameObject.GetComponent<UILabel>();
        Spt_RightSplit = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemContent/RightSplit").gameObject.GetComponent<UISprite>();

        Trans_LeftItem = _uiRoot.transform.FindChild("PanelLeft/LeftScrollView/LeftGrid/LeftItem");
        Trans_RightItemJumpTo = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo");
        Trans_SingleJumpToItem = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemJumpTo/RightItem_SingleInfo");
        Trans_RightItemContent = _uiRoot.transform.FindChild("PanelRight/RightScrollView/RightGrid/RightItemContent");
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_LeftItem_Title.text = "";// 最長6個字 切換
        Lbl_RightTitle.text = "";
        Lbl_RightItem_ConditionHint.text = "";
        Lbl_RightContent.text = "";
        Lbl_RightHint.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
