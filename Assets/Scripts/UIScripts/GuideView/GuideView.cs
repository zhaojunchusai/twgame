using UnityEngine;
using System;
using System.Collections;

public class GuideView
{
    public static string UIName = "GuideView";
    public GameObject _uiRoot;
    public UITexture Tex_GuideMask;
    public UISprite Spt_HighLight01;
    public UISprite Spt_HighLight02;
    public GameObject Gobj_Colliders;
    public UITexture Tex_gobj_Colliders;
    public GameObject Gobj_Desc;
    public UISprite Spt_DescBG;
    public UILabel Lbl_Desc;
    public FingerRingAni FingerRingAni_Finger;
    public UISprite Spt_Finger;
    public UITexture Tex_Finger;
    public UISprite Spt_FingerRing;
    public Transform Trans_Arrow01;
    public Transform Trans_Arrow02;
    public UISprite Spt_WhiteCanvas;
    public WidgetAlphaAni WhiteCanvas;
    public Transform Trans_Finger;
    public GameObject Gobj_DragArrow;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/GuideView");
        Tex_GuideMask = _uiRoot.transform.FindChild("GuideMask").gameObject.GetComponent<UITexture>();
        Spt_HighLight01 = _uiRoot.transform.FindChild("HighLight01").gameObject.GetComponent<UISprite>();
        Spt_HighLight02 = _uiRoot.transform.FindChild("HighLight02").gameObject.GetComponent<UISprite>();
        Gobj_Colliders = _uiRoot.transform.FindChild("gobj_Colliders").gameObject;
        Tex_gobj_Colliders = _uiRoot.transform.FindChild("gobj_Colliders").gameObject.GetComponent<UITexture>();
        Gobj_Desc = _uiRoot.transform.FindChild("gobj_Desc").gameObject;
        Spt_DescBG = _uiRoot.transform.FindChild("gobj_Desc/DescBG").gameObject.GetComponent<UISprite>();
        Lbl_Desc = _uiRoot.transform.FindChild("gobj_Desc/Desc").gameObject.GetComponent<UILabel>();
        FingerRingAni_Finger = _uiRoot.transform.FindChild("Finger").gameObject.GetComponent<FingerRingAni>();
        Spt_Finger = _uiRoot.transform.FindChild("Finger/Finger").gameObject.GetComponent<UISprite>();
        Tex_Finger = _uiRoot.transform.FindChild("Finger/Finger").gameObject.GetComponent<UITexture>();
        Spt_FingerRing = _uiRoot.transform.FindChild("Finger/FingerRing").gameObject.GetComponent<UISprite>();

        Trans_Arrow01 = _uiRoot.transform.FindChild("Arrow01");
        Trans_Arrow02 = _uiRoot.transform.FindChild("Arrow02");
        Spt_WhiteCanvas = _uiRoot.transform.FindChild("WhiteCanvas").gameObject.GetComponent<UISprite>();
        WhiteCanvas = _uiRoot.transform.FindChild("WhiteCanvas").gameObject.GetComponent<WidgetAlphaAni>();
        Trans_Finger = _uiRoot.transform.FindChild("Finger");
        Gobj_DragArrow = _uiRoot.transform.FindChild("gobj_DragArrow").gameObject;
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Desc.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
