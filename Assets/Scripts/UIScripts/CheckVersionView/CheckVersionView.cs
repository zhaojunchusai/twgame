using UnityEngine;
using System;
using System.Collections;

public class CheckVersionView
{
    public static string UIName = "CheckVersionView";
    public GameObject _uiRoot;
    public UITexture Tex_BG;
    public UISprite Spt_Logo;
    public UILabel Lbl_Hint;
    public GameObject Gobj_ProgressBar;
    public UITexture Tex_ProgressBarB;
    public UITexture Tex_ProgressBarF;
    public UIProgressBar ProgressBar_Version;
    public UILabel Lbl_ProgressLb;
    public UILabel Lbl_TipsLb;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/CheckVersionView");
        //Tex_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UITexture>();
        Spt_Logo = _uiRoot.transform.FindChild("Logo").gameObject.GetComponent<UISprite>();
        Lbl_Hint = _uiRoot.transform.FindChild("Lab/Hint").gameObject.GetComponent<UILabel>();
        Gobj_ProgressBar = _uiRoot.transform.FindChild("gobj_ProgressBar").gameObject;
        //Tex_ProgressBarB = _uiRoot.transform.FindChild("gobj_ProgressBar/ProgressBarB").gameObject.GetComponent<UITexture>();
        //Tex_ProgressBarF = _uiRoot.transform.FindChild("gobj_ProgressBar/ProgressBarB/ProgressBarF").gameObject.GetComponent<UITexture>();
        ProgressBar_Version = _uiRoot.transform.FindChild("gobj_ProgressBar/ProgressBar").gameObject.GetComponent<UIProgressBar>();
        Lbl_ProgressLb = _uiRoot.transform.FindChild("gobj_ProgressBar/ProgressLb").gameObject.GetComponent<UILabel>();
        Lbl_TipsLb = _uiRoot.transform.FindChild("TipsObj/TipsLb").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {

    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
