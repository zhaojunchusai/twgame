using UnityEngine;
using System;
using System.Collections;

public class GMView
{
    public const string UIName ="GMView";
    public GameObject _uiRoot;
    public UISprite Spt_BG;
    public UIInput Ipt_Input;
    public UISprite Spt_InputInputBackground;
    public UILabel Lbl_InputInputLabel;
    public UIButton Btn_Confirm;
    public UISprite Spt_BtnConfirmBackground;
    public UILabel Lbl_BtnConfirmLabel;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBackground;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/GMView");
        Spt_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Ipt_Input = _uiRoot.transform.FindChild("Input").gameObject.GetComponent<UIInput>();
        Spt_InputInputBackground = _uiRoot.transform.FindChild("Input/Background").gameObject.GetComponent<UISprite>();
        Lbl_InputInputLabel = _uiRoot.transform.FindChild("Input/Label").gameObject.GetComponent<UILabel>();
        Btn_Confirm = _uiRoot.transform.FindChild("Confirm").gameObject.GetComponent<UIButton>();
        Spt_BtnConfirmBackground = _uiRoot.transform.FindChild("Confirm/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirmLabel = _uiRoot.transform.FindChild("Confirm/Label").gameObject.GetComponent<UILabel>();
        Btn_Close = _uiRoot.transform.FindChild("Close").gameObject.GetComponent<UIButton>();
        Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Close/Background").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_InputInputLabel.text = "You can type here";
        Lbl_BtnConfirmLabel.text = "提交";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
