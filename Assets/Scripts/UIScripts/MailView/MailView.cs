using UnityEngine;
using System.Collections;

public class MailView
{
    public static string UIName ="MailView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskSprite;
    public UILabel Lbl_MailTitleLabel;
    public UIScrollView Scr_ScorllView;
    public UIGrid Grd_Grid;
    public UIButton Btn_BatchReceiveButton;
    public UILabel Lbl_BtnBatchRecieveLabel;
    public GameObject Obj_Source;

    public TweenScale Anim_TScale;
    public TweenPosition TPos;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/MailView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Lbl_MailTitleLabel = _uiRoot.transform.FindChild("Anim/TitleLabel").gameObject.GetComponent<UILabel>();
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Scr_ScorllView = _uiRoot.transform.FindChild("Anim/ScorllView").gameObject.GetComponent<UIScrollView>();
        Grd_Grid = _uiRoot.transform.FindChild("Anim/ScorllView/Grid").gameObject.GetComponent<UIGrid>();
        Btn_BatchReceiveButton = _uiRoot.transform.FindChild("Anim/BatchReceiveButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnBatchRecieveLabel = _uiRoot.transform.FindChild("Anim/BatchReceiveButton/RecieveButtonLabel").gameObject.GetComponent<UILabel>();
        Obj_Source = _uiRoot.transform.FindChild("Source/MailItem").gameObject;
     
        SetLabelValues();
    }

    public void Uninitialize()
    {
        Lbl_BtnBatchRecieveLabel.text = ConstString.EMAIL_BATCHRECEIVE;
        Lbl_MailTitleLabel.text = ConstString.EMAIL_BOX;
        
    }

    public void SetLabelValues()
    {
        
    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
