using UnityEngine;
using System;
using System.Collections;

public class MessageBoxUI
{
    public static string UIName ="MessageBoxUI";
    public GameObject _uiRoot;
    //public UISprite Spt_BG;
    public UIButton Btn_Confirm;
    //public UISprite Spt_BtnConfirmBackground;
    public UILabel Lbl_BtnConfirmLabel;
    //public UISprite Spt_FG;
    public UIScrollView ScrView_Pan;
    public UILabel Lbl_Content;
    public UILabel Lbl_Title;
    public UISprite Spt_Mask;
    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/MessageBoxUI");
        //Spt_BG = _uiRoot.transform.FindChild("Page/BG").gameObject.GetComponent<UISprite>();
        Btn_Confirm = _uiRoot.transform.FindChild("Page/Confirm").gameObject.GetComponent<UIButton>();
        //Spt_BtnConfirmBackground = _uiRoot.transform.FindChild("Page/Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Page/Confirm/Label").gameObject.GetComponent<UILabel>();
        //Spt_FG = _uiRoot.transform.FindChild("Page/FG").gameObject.GetComponent<UISprite>();
        ScrView_Pan = _uiRoot.transform.FindChild("Page/PanRoot/Pan").gameObject.GetComponent<UIScrollView>();
        Lbl_Content = _uiRoot.transform.FindChild("Page/PanRoot/Pan/Content").gameObject.GetComponent<UILabel>();
        Lbl_Title = _uiRoot.transform.FindChild("Page/Title").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();

        //SetLabelValues();
        SetBoundary();
    }


    public void SetBoundary()
    {
        Boundary.left = -512f;
        Boundary.right = 512f;
        Boundary.up = 288f;
        Boundary.down = -288f;
    }

    public void SetLabelValues()
    {
        Lbl_BtnConfirmLabel.text = "确定";
        Lbl_Content.text = "她朋友的单，子";
        Lbl_Title.text = "获得物品";
    }

    public void Uninitialize()
    {

    }



    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
