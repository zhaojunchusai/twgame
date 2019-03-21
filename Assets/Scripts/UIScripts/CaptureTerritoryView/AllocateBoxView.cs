using UnityEngine;
using System;
using System.Collections;

public class AllocateBoxView
{
    public static string UIName ="AllocateBoxView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_AllocateBoxView;
    public UISprite Spt_Mask;
    public UISprite Spt_BG;
    public UISprite Spt_FG;
    public UISprite Spt_FG1;
    public UISprite Spt_Flower1;
    public UISprite Spt_Flower2;
    public UILabel Lbl_TitleLb;
    public UISprite Spt_TitleLiftBG;
    public UISprite Spt_TitleRightBG;
    public UILabel Lbl_BoxCount;
    public UILabel Lbl_TitleRank;
    public UILabel Lbl_TitleName;
    public UILabel Lbl_TitleScore;
    public UILabel Lbl_TitleAllocate;
    public UIPanel UIPanel_Items;
    public UIScrollView ScrView_Items;
    public UIGrid Grd_Grid;
    public UIButton Btn_SeeBox;
    public UISprite Spt_BtnSeeBoxBackground;
    public UILabel Lbl_BtnSeeBoxLabel;
    public UIButton Btn_Confirm;
    public UISprite Spt_BtnConfirmBackground;
    public UILabel Lbl_BtnConfirmLabel;
    public UIButton Btn_Cancel;
    public UISprite Spt_BtnCancelBackground;
    public UILabel Lbl_BtnCancelLabel;
    public GameObject Gobj_AllocateBoxItem;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/AllocateBoxView");
        UIPanel_AllocateBoxView = _uiRoot.GetComponent<UIPanel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Spt_BG = _uiRoot.transform.FindChild("Anim/BGS/BG").gameObject.GetComponent<UISprite>();
        Spt_FG = _uiRoot.transform.FindChild("Anim/BGS/FG").gameObject.GetComponent<UISprite>();
        Spt_FG1 = _uiRoot.transform.FindChild("Anim/BGS/FG1").gameObject.GetComponent<UISprite>();
        Spt_Flower1 = _uiRoot.transform.FindChild("Anim/BGS/Flower1").gameObject.GetComponent<UISprite>();
        Spt_Flower2 = _uiRoot.transform.FindChild("Anim/BGS/Flower2").gameObject.GetComponent<UISprite>();
        Lbl_TitleLb = _uiRoot.transform.FindChild("Anim/TitleObj/TitleLb").gameObject.GetComponent<UILabel>();
        Spt_TitleLiftBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleBGLow/TitleLiftBG").gameObject.GetComponent<UISprite>();
        Spt_TitleRightBG = _uiRoot.transform.FindChild("Anim/TitleObj/TitleBGLow/TitleRightBG").gameObject.GetComponent<UISprite>();
        Lbl_BoxCount = _uiRoot.transform.FindChild("Anim/Labels/BoxCount").gameObject.GetComponent<UILabel>();
        Lbl_TitleRank = _uiRoot.transform.FindChild("Anim/Labels/TitleRank").gameObject.GetComponent<UILabel>();
        Lbl_TitleName = _uiRoot.transform.FindChild("Anim/Labels/TitleName").gameObject.GetComponent<UILabel>();
        Lbl_TitleScore = _uiRoot.transform.FindChild("Anim/Labels/TitleScore").gameObject.GetComponent<UILabel>();
        Lbl_TitleAllocate = _uiRoot.transform.FindChild("Anim/Labels/TitleAllocate").gameObject.GetComponent<UILabel>();
        UIPanel_Items = _uiRoot.transform.FindChild("Anim/Items").gameObject.GetComponent<UIPanel>();
        ScrView_Items = _uiRoot.transform.FindChild("Anim/Items").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Anim/Items/Grid").gameObject.GetComponent<UIGrid>();
        Btn_SeeBox = _uiRoot.transform.FindChild("Anim/SeeBox").gameObject.GetComponent<UIButton>();
        Spt_BtnSeeBoxBackground = _uiRoot.transform.FindChild("Anim/SeeBox/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnSeeBoxLabel = _uiRoot.transform.FindChild("Anim/SeeBox/Label").gameObject.GetComponent<UILabel>();
        Btn_Confirm = _uiRoot.transform.FindChild("Anim/Confirm").gameObject.GetComponent<UIButton>();
        Spt_BtnConfirmBackground = _uiRoot.transform.FindChild("Anim/Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Anim/Confirm/Label").gameObject.GetComponent<UILabel>();
        Btn_Cancel = _uiRoot.transform.FindChild("Anim/Cancel").gameObject.GetComponent<UIButton>();
        Spt_BtnCancelBackground = _uiRoot.transform.FindChild("Anim/Cancel/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCancelLabel = _uiRoot.transform.FindChild("Anim/Cancel/Label").gameObject.GetComponent<UILabel>();
        Gobj_AllocateBoxItem = _uiRoot.transform.FindChild("Pre/AllocateBoxItem").gameObject;        
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLb.text = ConstString.LBL_ALLOCATE_BOX;
        Lbl_BoxCount.text = ConstString.LBL_BOX_COUNT;
        Lbl_TitleRank.text = ConstString.LBL_RANK;
        Lbl_TitleName.text = ConstString.LBL_MEMBER_NAME;
        Lbl_TitleScore.text = ConstString.LBL_LAST_CAMPAIN_SCORE;
        Lbl_TitleAllocate.text = ConstString.LBL_ALLOCATE_BOX;
        Lbl_BtnSeeBoxLabel.text = ConstString.LBL_SEE_BOX;
        Lbl_BtnConfirmLabel.text = ConstString.LBL_CONFIRM;
        Lbl_BtnCancelLabel.text = ConstString.LBL_CANCEL;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
