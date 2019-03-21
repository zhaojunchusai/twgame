using UnityEngine;
using System;
using System.Collections;

public class BuySPView
{
    public static string UIName ="BuySPView";
    public GameObject _uiRoot;
    public UIButton Btn_Confirm;
    public UIButton Btn_Close;
    public UILabel Lbl_Content;
    public UILabel Lbl_Diamond;
    public UILabel Lbl_Sp;

    public TweenScale Anim_TScale;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/BuySPView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_Confirm = _uiRoot.transform.FindChild("Anim/Btn/Confirm").gameObject.GetComponent<UIButton>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Btn/Close").gameObject.GetComponent<UIButton>();
        Lbl_Content = _uiRoot.transform.FindChild("Anim/Content").gameObject.GetComponent<UILabel>();
        Lbl_Diamond = _uiRoot.transform.FindChild("Anim/ContentUp/DiamondNum").gameObject.GetComponent<UILabel>();
        Lbl_Sp = _uiRoot.transform.FindChild("Anim/ContentUp/SpNum").gameObject.GetComponent<UILabel>();
        //Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        //Spt_FG = _uiRoot.transform.FindChild("Anim/FG").gameObject.GetComponent<UISprite>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Content.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
