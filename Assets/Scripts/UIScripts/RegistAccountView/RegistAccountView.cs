using UnityEngine;
using System;
using System.Collections;

public class RegistAccountView
{
    public static string UIName ="RegistAccountView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_RegistAccountView;
    public UISprite Spt_Bg;
    public UISprite Spt_Line1;
    public UISprite Spt_Line2;
    public UIInput Ipt_Account;
    public UISprite Spt_InputAccountBG;
    public UILabel Lbl_InputAccountAccountlb;
    public UISprite Spt_InputAccountTip;
    public UIInput Ipt_Pwd;
    public UISprite Spt_InputPwdBG;
    public UILabel Lbl_InputPwdLabel;
    public UISprite Spt_InputPwdTip;
    public UIInput Ipt_ConfirmPwd;
    public UISprite Spt_InputConfirmPwdBG;
    public UILabel Lbl_InputConfirmPwdLabel;
    public UISprite Spt_InputConfirmPwdTip;
    public UIButton Btn_Confirm;
    public UISprite Spt_BtnConfirmBG;
    public UISprite Spt_BtnConfirmLabel;
    public UIButton Btn_Back;
    public UISprite Spt_BtnBackBG;
    public UISprite Spt_BtnBackLabel;
    public UISprite Spt_Logo;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/RegistAccountView");
        //UIPanel_RegistAccountView = _uiRoot.GetComponent<UIPanel>();
        Spt_Logo = _uiRoot.transform.FindChild("Logo").gameObject.GetComponent<UISprite>();
        //Spt_Line1 = _uiRoot.transform.FindChild("Bg/Line1").gameObject.GetComponent<UISprite>();
        //Spt_Line2 = _uiRoot.transform.FindChild("Bg/Line2").gameObject.GetComponent<UISprite>();
        Ipt_Account = _uiRoot.transform.FindChild("Account").gameObject.GetComponent<UIInput>();
        //Spt_InputAccountBG = _uiRoot.transform.FindChild("Account/BG").gameObject.GetComponent<UISprite>();
        Lbl_InputAccountAccountlb = _uiRoot.transform.FindChild("Account/Accountlb").gameObject.GetComponent<UILabel>();
        //Spt_InputAccountTip = _uiRoot.transform.FindChild("Account/Tip").gameObject.GetComponent<UISprite>();
        Ipt_Pwd = _uiRoot.transform.FindChild("Pwd").gameObject.GetComponent<UIInput>();
        //Spt_InputPwdBG = _uiRoot.transform.FindChild("Pwd/BG").gameObject.GetComponent<UISprite>();
        Lbl_InputPwdLabel = _uiRoot.transform.FindChild("Pwd/Label").gameObject.GetComponent<UILabel>();
        //Spt_InputPwdTip = _uiRoot.transform.FindChild("Pwd/Tip").gameObject.GetComponent<UISprite>();
        Ipt_ConfirmPwd = _uiRoot.transform.FindChild("ConfirmPwd").gameObject.GetComponent<UIInput>();
        //Spt_InputConfirmPwdBG = _uiRoot.transform.FindChild("ConfirmPwd/BG").gameObject.GetComponent<UISprite>();
        Lbl_InputConfirmPwdLabel = _uiRoot.transform.FindChild("ConfirmPwd/Label").gameObject.GetComponent<UILabel>();
        //Spt_InputConfirmPwdTip = _uiRoot.transform.FindChild("ConfirmPwd/Tip").gameObject.GetComponent<UISprite>();
        Btn_Confirm = _uiRoot.transform.FindChild("Confirm").gameObject.GetComponent<UIButton>();
        //Spt_BtnConfirmBG = _uiRoot.transform.FindChild("Confirm/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnConfirmLabel = _uiRoot.transform.FindChild("Confirm/Label").gameObject.GetComponent<UISprite>();
        Btn_Back = _uiRoot.transform.FindChild("Back").gameObject.GetComponent<UIButton>();
        //Spt_BtnBackBG = _uiRoot.transform.FindChild("Back/BG").gameObject.GetComponent<UISprite>();
        //Spt_BtnBackLabel = _uiRoot.transform.FindChild("Back/Label").gameObject.GetComponent<UISprite>();
       // SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_InputAccountAccountlb.text = ConstString.ACCOUNT_RULE;
        Lbl_InputPwdLabel.text = ConstString.ACCOUNT_RULE;
        Lbl_InputConfirmPwdLabel.text = ConstString.ACCOUNT_RULE;
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
