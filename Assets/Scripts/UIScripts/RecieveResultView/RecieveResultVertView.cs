using UnityEngine;
using System;
using System.Collections;

public class RecieveResultVertView
{
    public static string UIName ="RecieveResultVertView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_RecieveResultVertView;
    public UISprite Spt_MaskBGSprite;
    public UISprite Spt_BGSprite;
    public UISprite Spt_ScorllViewBGSprite;
    public UIPanel UIPanel_ScorllView;
    public UIScrollView ScrView_ScorllView;
    public UIGrid Grd_ItemGrid;
    public UIButton Btn_SureButton;
    public UISprite Spt_BtnSureButtonButtonBG;
    public UILabel Lbl_ReturnSkillBookLabel;
    public GameObject Obj_Source;
    public UILabel Lbl_SureButtonLabel;
    public UILabel Lbl_TitleLabel;
    public UILabel Lbl_DescLabel;
    public UISprite Spt_DLSprite;
    public UISprite Spt_DRSprite;
    public TweenScale OffsetRoot_TScale;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/RecieveResultVertView");
        UIPanel_RecieveResultVertView = _uiRoot.GetComponent<UIPanel>();
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot").gameObject.GetComponent<TweenScale>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        UIPanel_ScorllView = _uiRoot.transform.FindChild("OffsetRoot/ScorllView").gameObject.GetComponent<UIPanel>();
        ScrView_ScorllView = _uiRoot.transform.FindChild("OffsetRoot/ScorllView").gameObject.GetComponent<UIScrollView>(); 
        Grd_ItemGrid = _uiRoot.transform.FindChild("OffsetRoot/ScorllView/ItemGrid").gameObject.GetComponent<UIGrid>();
        Lbl_TitleLabel = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Lbl_DescLabel = _uiRoot.transform.FindChild("OffsetRoot/DescLabel").gameObject.GetComponent<UILabel>();
        Btn_SureButton = _uiRoot.transform.FindChild("OffsetRoot/SureButton").gameObject.GetComponent<UIButton>();
        Lbl_SureButtonLabel = _uiRoot.transform.FindChild("OffsetRoot/SureButton/SureButtonLabel").gameObject.GetComponent<UILabel>();
        Lbl_ReturnSkillBookLabel = _uiRoot.transform.FindChild("OffsetRoot/ScorllView/ReturnSkillBookLabel").gameObject.GetComponent<UILabel>();
        Obj_Source = _uiRoot.transform.FindChild("Source/MailAttachmentItem").gameObject;
        Spt_DLSprite = _uiRoot.transform.FindChild("DLSprite").gameObject.GetComponent<UISprite>();
        Spt_DRSprite = _uiRoot.transform.FindChild("DRSprite").gameObject.GetComponent<UISprite>();
        Spt_BGSprite = _uiRoot.transform.FindChild("OffsetRoot/BGSprite").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_DescLabel.text = "";
        Lbl_ReturnSkillBookLabel.text = ConstString.RECEIVE_RETURNSKILLBOOK;
        Lbl_SureButtonLabel.text = ConstString.MESSAGEBOXBTN_YES;
        Lbl_TitleLabel.text = ConstString.RECEIVE_SUCCESS;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
