using UnityEngine;
using System;
using System.Collections;

public class GetServerInfoUI
{
    public static string UIName ="GetServerInfoUI";
    public GameObject _uiRoot;
    //public UITexture Texture_BG;
    //public UISprite Spt_progressBar;
    public UISlider Slider_progressBar;
    //public UISprite Spt_SliderprogressBarBackdrop;
    public UILabel Lbl_progressLabel;
    //public UISprite Spt_SliderprogressBarOverlay;
    public UILabel Lbl_TipsLB;
    //public UISprite Spt_TipsBG;
    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/GetServerInfoUI");
        //Texture_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UITexture>();
        //Spt_progressBar = _uiRoot.transform.FindChild("progressBar").gameObject.GetComponent<UISprite>();
        Slider_progressBar = _uiRoot.transform.FindChild("progressBar").gameObject.GetComponent<UISlider>();
        //Spt_SliderprogressBarBackdrop = _uiRoot.transform.FindChild("progressBar/Backdrop").gameObject.GetComponent<UISprite>();
        Lbl_progressLabel = _uiRoot.transform.FindChild("progressBar/Thumb/progressLabel").gameObject.GetComponent<UILabel>();
        //Spt_SliderprogressBarOverlay = _uiRoot.transform.FindChild("progressBar/Overlay").gameObject.GetComponent<UISprite>();
        Lbl_TipsLB = _uiRoot.transform.FindChild("Tips/TipsLB").gameObject.GetComponent<UILabel>();
        //Spt_TipsBG = _uiRoot.transform.FindChild("Tips/TipsBG").gameObject.GetComponent<UISprite>();

        SetLabelValues();
        SetBoundary();
    }


    public void SetBoundary()
    {
        Boundary.left = -461.517f;
        Boundary.right = 458.483f;
        Boundary.up = 288f;
        Boundary.down = -288f;
    }

    public void SetLabelValues()
    {
        Lbl_progressLabel.text = "1%";
        Lbl_TipsLB.text = ConstString.UI_GETSERVERINFO_CS;
    }

    public void Uninitialize()
    {

    }



    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
