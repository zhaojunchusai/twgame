using UnityEngine;
using System.Collections;

public class AnnouncementView
{
    public static string UIName ="AnnouncementView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_AnnouncementView;
    public UISprite Spt_LightSprite;
    public UISprite Spt_MaskBGSprite;
    public UISprite Spt_BGSprite;
    public UISprite Spt_ContextBGSprite;
    public UISprite Spt_TitleLeftSprite;
    public UISprite Spt_TitleRightSprite;
    public UILabel Lbl_TitleLabel;
    public UIPanel UIPanel_ScrollView;
    public UIScrollView ScrView_ScrollView;
    public UILabel Lbl_ContentLabel;
    public UIButton Btn_SureButton;
    public UISprite Spt_BtnSureButtonButtonBG;
    public UILabel Lbl_BtnSureButtonRecieveButtonLabel;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/AnnouncementView");
        UIPanel_AnnouncementView = _uiRoot.GetComponent<UIPanel>();
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Spt_BGSprite = _uiRoot.transform.FindChild("OffsetRoot/BGSprite").gameObject.GetComponent<UISprite>();
        Spt_LightSprite = _uiRoot.transform.FindChild("OffsetRoot/LightSprite").gameObject.GetComponent<UISprite>();
        Spt_ContextBGSprite = _uiRoot.transform.FindChild("OffsetRoot/ContextBGSprite").gameObject.GetComponent<UISprite>();
        Spt_TitleLeftSprite = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLeftSprite").gameObject.GetComponent<UISprite>();
        Spt_TitleRightSprite = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleRightSprite").gameObject.GetComponent<UISprite>();
        Lbl_TitleLabel = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        UIPanel_ScrollView = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView").gameObject.GetComponent<UIPanel>();
        ScrView_ScrollView = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView").gameObject.GetComponent<UIScrollView>();
        Lbl_ContentLabel = _uiRoot.transform.FindChild("OffsetRoot/ContextGroup/ScrollView/ContentLabel").gameObject.GetComponent<UILabel>();
        Btn_SureButton = _uiRoot.transform.FindChild("OffsetRoot/SureButton").gameObject.GetComponent<UIButton>();
        Spt_BtnSureButtonButtonBG = _uiRoot.transform.FindChild("OffsetRoot/SureButton/ButtonBG").gameObject.GetComponent<UISprite>();
        Lbl_BtnSureButtonRecieveButtonLabel = _uiRoot.transform.FindChild("OffsetRoot/SureButton/RecieveButtonLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_TitleLabel.text = ConstString.ANNOUNCEMENTVIEW_LABEL_TITLE;
        Lbl_ContentLabel.text = "";
        Lbl_BtnSureButtonRecieveButtonLabel.text =ConstString.ANNOUNCEMENTVIEW_LABEL_IKNOW;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
