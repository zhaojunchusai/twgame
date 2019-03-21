using UnityEngine;
using System.Collections;

public class MailInfoView
{
    public static string UIName ="MailInfoView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskBGSprite;

    public UILabel Lbl_MailTitleLabel;
    public UIPanel UIPanel_ScrollView;
    public UIScrollView ScrView_ScrollView;
    public UITable UITable_Table;
    public UILabel Lbl_InfoLabel;
    public UILabel Lbl_TimeLabel;
    public UISprite Spt_SplitSprite;
    public UIGrid Grd_Grid;
    public UIScrollView ScrView_AttachScrollView;
   
    public MailAttachmentItem MailAttachmentItem;

    public UIButton Btn_CloseButton;
    public UILabel Lbl_CloseBtnLabel;
    public UIButton Btn_DeleteButton;
    public UILabel Lbl_DeleteBtnLabel;
    public UIButton Btn_RecieveButton;
    public UILabel Lbl_RecieveBtnLabel;

    public TweenScale OffsetRoot_TScale;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/MailInfoView");
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot").gameObject.GetComponent<TweenScale>();
        UIPanel_ScrollView = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ScrollView = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView").gameObject.GetComponent<UIScrollView>();
        UITable_Table = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView/Table").gameObject.GetComponent<UITable>();
        Lbl_InfoLabel = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView/Table/InfoLabel").gameObject.GetComponent<UILabel>();
        Lbl_TimeLabel = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView/Table/TimeLabel").gameObject.GetComponent<UILabel>();
        Spt_SplitSprite = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/SplitSprite").gameObject.GetComponent<UISprite>();
        Grd_Grid = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsGroup/ScrollView/Grid").gameObject.GetComponent<UIGrid>();
        ScrView_AttachScrollView = _uiRoot.transform.FindChild("OffsetRoot/AttachmentsGroup/ScrollView").gameObject.GetComponent<UIScrollView>();
        
        MailAttachmentItem = _uiRoot.transform.FindChild("Source/EmailAttachmentItem").gameObject.GetComponent<MailAttachmentItem>();
      
        Btn_CloseButton = _uiRoot.transform.FindChild("OffsetRoot/CloseButton").gameObject.GetComponent<UIButton>();
        Lbl_CloseBtnLabel = _uiRoot.transform.FindChild("OffsetRoot/CloseButton/CloseButtonLabel").gameObject.GetComponent<UILabel>();
        Btn_DeleteButton = _uiRoot.transform.FindChild("OffsetRoot/DeleteButton").gameObject.GetComponent<UIButton>();
        Lbl_DeleteBtnLabel = _uiRoot.transform.FindChild("OffsetRoot/DeleteButton/DeleteButtonLabel").gameObject.GetComponent<UILabel>();
        Btn_RecieveButton = _uiRoot.transform.FindChild("OffsetRoot/RecieveButton").gameObject.GetComponent<UIButton>();
        Lbl_RecieveBtnLabel = _uiRoot.transform.FindChild("OffsetRoot/RecieveButton/RecieveButtonLabel").gameObject.GetComponent<UILabel>();
        Lbl_MailTitleLabel = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_InfoLabel.text = "";
        Lbl_TimeLabel.text = "";
        Lbl_MailTitleLabel.text = ConstString.EMAIL_TITLE;
        Lbl_CloseBtnLabel.text =  ConstString.EMAIL_CLOSE;
        Lbl_DeleteBtnLabel.text = ConstString.EMAIL_DELETE;
        Lbl_RecieveBtnLabel.text = ConstString.EMAIL_RECEIVE;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
