using UnityEngine;
using System;
using System.Collections;

public class AdvanceTipView
{
    public static string UIName ="AdvanceTipView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_AdvanceTipView;
    public UIButton Btn_Accept;
    public UISprite Spt_BtnAcceptBackground;
    public UILabel Lbl_BtnAcceptLabel;
    public UISprite Spt_BG;
    public UISprite Spt_Title1;
    public UISprite Spt_Title2;
    public UISprite Spt_People;
    public UILabel Lbl_Content;
    public UISprite Spt_Mask;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/AdvanceTipView");
        UIPanel_AdvanceTipView = _uiRoot.GetComponent<UIPanel>();
        Btn_Accept = _uiRoot.transform.FindChild("Accept").gameObject.GetComponent<UIButton>();
        Spt_BtnAcceptBackground = _uiRoot.transform.FindChild("Accept/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnAcceptLabel = _uiRoot.transform.FindChild("Accept/Label").gameObject.GetComponent<UILabel>();
        Spt_BG = _uiRoot.transform.FindChild("Common/BG").gameObject.GetComponent<UISprite>();
        Spt_Title1 = _uiRoot.transform.FindChild("Common/Title1").gameObject.GetComponent<UISprite>();
        Spt_Title2 = _uiRoot.transform.FindChild("Common/Title2").gameObject.GetComponent<UISprite>();
        Spt_People = _uiRoot.transform.FindChild("Common/People").gameObject.GetComponent<UISprite>();
        Lbl_Content = _uiRoot.transform.FindChild("Common/Content").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
