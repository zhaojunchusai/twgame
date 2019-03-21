using UnityEngine;
using System;
using System.Collections;

public class DialogueView
{
    public static string UIName ="DialogueView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskBG;
    public UISprite Spt_LeftSprite;
    public UISprite Spt_RightSprite;
    public UIWidget UIWidget_Content;
    public UILabel Lbl_ContentLabel;
    public TypewriterEffect TypewriterEffect_ContentLabel;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/DialogueView");
        Spt_MaskBG = _uiRoot.transform.FindChild("MaskBG").gameObject.GetComponent<UISprite>();
        Spt_LeftSprite = _uiRoot.transform.FindChild("LeftSprite").gameObject.GetComponent<UISprite>();
        Spt_RightSprite = _uiRoot.transform.FindChild("RightSprite").gameObject.GetComponent<UISprite>();
        UIWidget_Content = _uiRoot.transform.FindChild("Upper/Content").gameObject.GetComponent<UIWidget>();
        Lbl_ContentLabel = _uiRoot.transform.FindChild("Upper/Content/ContentLabel").gameObject.GetComponent<UILabel>();
        TypewriterEffect_ContentLabel = _uiRoot.transform.FindChild("Upper/Content/ContentLabel").gameObject.GetComponent<TypewriterEffect>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ContentLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
