using UnityEngine;
using System;
using System.Collections;

public class MailBatchRecieveView
{
    public static string UIName ="MailBatchRecieveView";
    public GameObject _uiRoot;
    public UIScrollView ScrView_AttachmentsScrollView;
    public RollSrollViewItem rollSrollViewItem;
    public UISprite Spt_MaskBG;
    public GameObject GameObj_SplitObj;
    public UILabel Lbl_FailedLabel;
    public UIGrid Grd_SuccessGrid;
    public UIGrid Grd_FailedGrid;
    public UIButton Btn_SureButton;
    public UILabel Lbl_SureButtonLabel;
    public UILabel Lbl_TitleLable;
    public GameObject Obj_Source;
    public TweenScale OffsetRoot_TScale;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/MailBatchRecieveView");
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot").gameObject.GetComponent<TweenScale>();
        ScrView_AttachmentsScrollView = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView").gameObject.GetComponent<UIScrollView>();
        rollSrollViewItem = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView").gameObject.GetComponent<RollSrollViewItem>();
        GameObj_SplitObj = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView/Root/Split").gameObject;
        Lbl_FailedLabel = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView/Root/Split/FailedLabel").gameObject.GetComponent<UILabel>();
        Grd_SuccessGrid = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView/Root/SuccessGrid").gameObject.GetComponent<UIGrid>();
        Grd_FailedGrid = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsScrollView/Root/FailedGrid").gameObject.GetComponent<UIGrid>();
        Btn_SureButton = _uiRoot.transform.FindChild("OffsetRoot/SureButton").gameObject.GetComponent<UIButton>();
        Lbl_SureButtonLabel = _uiRoot.transform.FindChild("OffsetRoot/SureButton/SureButtonLabel").gameObject.GetComponent<UILabel>();
        Lbl_TitleLable = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Obj_Source = _uiRoot.transform.FindChild("Source/MailAttachmentItem_S").gameObject;
        Spt_MaskBG = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_FailedLabel.text = ConstString.RECIEVE_FAILED;
        Lbl_TitleLable.text = ConstString.RECEIVE_RESULT;
        Lbl_SureButtonLabel.text = ConstString.MESSAGEBOXBTN_YES;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
