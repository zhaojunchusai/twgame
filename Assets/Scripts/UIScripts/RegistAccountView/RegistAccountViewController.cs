using System.Text.RegularExpressions;
using UnityEngine;
using System;
using System.Collections;

public class RegistAccountViewController : UIBase 
{
    public RegistAccountView view;

    public override void Initialize()
    {
        if (view == null) 
        {
            view = new RegistAccountView();
            view.Initialize();
            SetLogo();
            BtnEventBinding();
        }
            
    }

    private void SetLogo()
    {
        switch(GlobalConst.SERVER_TYPE)
        {
            case EServerType.Nei251_8201:
            case EServerType.Nei251_9201:
            case EServerType.Nei251_10201:
            case EServerType.Nei251_18201:
            case EServerType.Nei252_10201:
            case EServerType.Nei249_10201:
            case EServerType.Wai_Sifu:
            case EServerType.Wai_SifuB:
                {
                    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.LogoNormal);
                    view.Spt_Logo.width = 406;
                    view.Spt_Logo.height = 232;
                    view.Spt_Logo.transform.localPosition = new Vector3(5, 154, 0);
                    break;
                }
            case EServerType.Wai_JapanCP:
            case EServerType.Wai_7725:
            case EServerType.Wai_7725Test:
            case EServerType.Wai_7725IOSTest:
            case EServerType.Wai_BeiJing:
                {
                    CommonFunction.SetSpriteName(view.Spt_Logo, GlobalConst.SpriteName.Logo_BingLin);
                    view.Spt_Logo.width = 366;
                    view.Spt_Logo.height = 250;
                    view.Spt_Logo.transform.localPosition = new Vector3(6, 164, 0);
                    break;
                }
        }        
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        if(string.IsNullOrEmpty(view.Ipt_Account.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,ConstString.ERR_ACCOUNT_EMPTY);
            return;
        }
        if (string.IsNullOrEmpty(view.Ipt_Pwd.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_PASSWORD_EMPTY);
            return;
        }
        if (!CheckLetter(view.Ipt_Account.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_ACCNAME_FORMAT_WRONG);
            return;
        }
        if (!CheckLetter(view.Ipt_Pwd.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_PASSWORD_FORMAT_WRONG);
            return;
        }
        if (!view.Ipt_Pwd.value.Equals(view.Ipt_ConfirmPwd.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_PASSWORD_DIFFERENT);
            return;
        }
        //LoginModule.Instance.SetTmpAccount(view.Ipt_Account.value, UISystem.Instance.LoginView.view.Spt_BtnRememberPwdCheckMark.gameObject.activeSelf ? view.Ipt_Pwd.value : "");
        LoginModule.Instance.RegisterAccountReq(view.Ipt_Account.value, view.Ipt_Pwd.value);
    }

    private bool CheckLetter(string str)
    {
        string patten = @"^[a-zA-Z0-9]+$";
        Match match = Regex.Match(str, patten);
        return match.Success;
    }

    public void ButtonEvent_Back(GameObject btn)
    {
        if (UISystem.Instance.UIIsOpen(LoginView.UIName))
        {
            UISystem.Instance.LoginView.SetLoginChildActive(true);
        }
        UISystem.Instance.CloseGameUI(RegistAccountView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(view.Btn_Back.gameObject).onClick = ButtonEvent_Back;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }

}
