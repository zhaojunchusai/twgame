using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
/* File: LoginViewController.cs
 * Desc: 登陆界面
 * Date：2015-5-5 19:00
 * Add by taiwei
 * modify by taiwei 2015-05-18 20:25
 */
public class LoginViewController : UIBase
{
    public LoginView view;
    private List<ServerComponent> servercomp_dic;
    private int maxNUM = 0;
    private ServerComponent lastServerComp;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new LoginView();
            view.Initialize();
        }
        LoginModule.Instance.isNormalLogin = true;
        SetLogo();
        InitViewData();
        InitViewStatus();
        BtnEventBinding();
        EventDelegate.Add(view.UIPopupList_AccountList.onChange, OnAccountListChange);
        SetDefaultAccount();
        view.Spt_BtnAutoLoginCheckMark.gameObject.SetActive(PlayerPrefsTool.ReadBool(AppPrefEnum.AutoLogin));
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        //CheckNeedToAutoLogin();
        //List<CommonEquipData> readyAttackEquipList = new List<CommonEquipData>();
        //CommonEquipData tmp = new CommonEquipData();
        //tmp.uID = 1;
        //tmp.index = 1;
        //readyAttackEquipList.Add(tmp);
        //PlayerPrefsTool.WriteString<List<CommonEquipData>>(AppPrefEnum.PVPAttackEquip, readyAttackEquipList);
        if (!GlobalConst.ISOPENSDKOPERATE)
            ServerListManager.Instance.StartDownloadServerList(view.Ipt_Account.value);

        //if (ServerListManager.Instance.DownloadServerListFinish)
        //{
        //    ShowNoticeAndServerInfo();
        //}
        //else
        {
            ServerListManager.Instance.OnDownloadFinish = ShowNoticeAndServerInfo;
        }
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            SDKManager.Instance.KeyCodeBackEvent += ButtonEvent_KeyCodeBack;
        }
    }

    /// <summary>
    /// 设置视图初始化状态
    /// </summary>
    private void InitViewStatus()
    {
        view.Tex_LoginBG.mainTexture = ResourceLoadManager.Instance.LoadResources(GlobalConst.SpriteName.LoginBG) as Texture;
        ShowLoginAnim();
        //if (GlobalConst.ISOPENSDKOPERATE)
        //{
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            SetGobjVisible(view.Gobj_Login, false);
            SetGobjVisible(view.Gobj_Main, true);
            view.Gobj_Main.transform.localPosition = new Vector3(0, -118, 0);
            view.Btn_Start.transform.localPosition = new Vector3(10, -133, 0);
            view.Btn_Start.gameObject.SetActive(true);
            SetGobjVisible(view.Gobj_AutoLogin, false);
            SetLoginChildActive(false);
            SDKManager.Instance.PlatformLogin();
            //SDKManager.Instance.SDKOperate_StartLogin();
            return;
        }
        //}
        SetGobjVisible(view.Gobj_Login, true);
        SetGobjVisible(view.Gobj_Main, true);
        view.Gobj_Main.transform.localPosition = new Vector3(0, -180, 0);
        view.Btn_Start.gameObject.SetActive(false);
        SetGobjVisible(view.Gobj_AutoLogin, false);
        SetLoginChildActive(true);
    }

    private void SetLogo()
    {
        switch (GlobalConst.SERVER_TYPE)
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
                    view.Spt_MainLogo.enabled = false;
                    CommonFunction.SetSpriteName(view.Logo_Login, GlobalConst.SpriteName.LogoNormal);
                    view.Logo_Login.width = 406;
                    view.Logo_Login.height = 232;
                    view.Tpos_LoginLogo.from = new Vector3(5, 500, 0);
                    view.Tpos_LoginLogo.to = new Vector3(5, 223, 0);
                    CommonFunction.SetSpriteName(view.Logo_AutoLogin, GlobalConst.SpriteName.LogoNormal);
                    view.Logo_AutoLogin.width = 406;
                    view.Logo_AutoLogin.height = 232;
                    view.Logo_AutoLogin.transform.localPosition = new Vector3(5, 223, 0);
                    break;
                }
            case EServerType.Wai_7725Test:
            case EServerType.Wai_7725IOSTest:
            case EServerType.Wai_7725:
            case EServerType.Wai_JapanCP:
                {
                    view.Spt_MainLogo.enabled = true;
                    CommonFunction.SetSpriteName(view.Spt_MainLogo, GlobalConst.SpriteName.Logo_P7725);
                    view.Spt_MainLogo.width = 455;
                    view.Spt_MainLogo.height = 287;
                    CommonFunction.SetSpriteName(view.Logo_Login, GlobalConst.SpriteName.Logo_P7725);
                    view.Spt_MainLogo.width = 455;
                    view.Spt_MainLogo.height = 287;
                    view.Logo_Login.MakePixelPerfect();
                    //view.Logo_Login.width = 366;
                    //view.Logo_Login.height = 250;
                    view.Tpos_LoginLogo.from = new Vector3(6, 500, 0);
                    view.Tpos_LoginLogo.to = new Vector3(6, 233, 0);
                    CommonFunction.SetSpriteName(view.Logo_AutoLogin, GlobalConst.SpriteName.Logo_P7725);
                    view.Logo_AutoLogin.width = 366;
                    view.Logo_AutoLogin.height = 250;
                    view.Logo_AutoLogin.transform.localPosition = new Vector3(6, 233, 0);
                }
                break;
            case EServerType.Wai_BeiJing:
                {
                    view.Spt_MainLogo.enabled = false;
                    CommonFunction.SetSpriteName(view.Logo_Login, GlobalConst.SpriteName.Logo_BingLin);
                    view.Logo_Login.width = 366;
                    view.Logo_Login.height = 250;
                    view.Tpos_LoginLogo.from = new Vector3(6, 500, 0);
                    view.Tpos_LoginLogo.to = new Vector3(6, 233, 0);
                    CommonFunction.SetSpriteName(view.Logo_AutoLogin, GlobalConst.SpriteName.Logo_BingLin);
                    view.Logo_AutoLogin.width = 366;
                    view.Logo_AutoLogin.height = 250;
                    view.Logo_AutoLogin.transform.localPosition = new Vector3(6, 233, 0);
                    break;
                }
        }
        view.Tpos_LoginLogo.AddOnFinished(SetUIScale);
        ////if (GlobalConst.PLATFORM == TargetPlatforms.Android_SF)
        ////{
        ////    CommonFunction.SetSpriteName(view.Logo_Login, GlobalConst.SpriteName.LogoSf);
        ////    view.Logo_Login.width = 294;
        ////    view.Logo_Login.height = 266;
        ////    view.Tpos_LoginLogo.from = new Vector3(10, 500, 0);
        ////    view.Tpos_LoginLogo.to = new Vector3(10, 233, 0);
        ////    CommonFunction.SetSpriteName(view.Logo_AutoLogin, GlobalConst.SpriteName.LogoSf);
        ////    view.Logo_AutoLogin.width = 294;
        ////    view.Logo_AutoLogin.height = 266;
        ////    view.Logo_AutoLogin.transform.localPosition = new Vector3(10, 233, 0);

        ////}
        ////else
        //{
        //    CommonFunction.SetSpriteName(view.Logo_Login, GlobalConst.SpriteName.LogoNormal);
        //    view.Logo_Login.width = 406;
        //    view.Logo_Login.height = 232;
        //    view.Tpos_LoginLogo.from = new Vector3(5, 500, 0);
        //    view.Tpos_LoginLogo.to = new Vector3(5, 223, 0);
        //    CommonFunction.SetSpriteName(view.Logo_AutoLogin, GlobalConst.SpriteName.LogoNormal);
        //    view.Logo_AutoLogin.width = 406;
        //    view.Logo_AutoLogin.height = 232;
        //    view.Logo_AutoLogin.transform.localPosition = new Vector3(5, 223, 0);
        //}
    }

    private void SetUIScale()
    {
        view.Gobj_LoginChild.transform.localScale = Vector3.one;
        view.Tpos_LoginLogo.onFinished = null;
    }

    public void CheckNeedToAutoLogin()
    {
        if (PlayerPrefsTool.ReadBool(AppPrefEnum.AutoLogin))
        {
            SetGobjVisible(view.Gobj_Login, false);
            SetAutoLogin(true);
            Assets.Script.Common.Scheduler.Instance.AddTimer(1.5f, false, AutoLogin);
        }
        else
        {
            SetAutoLogin(false);
        }
    }
    private void AutoLogin()
    {
        LoginModule.Instance.AutoSendAuthorize();
    }

    /// <summary>
    /// 设置视图初始化数据
    /// </summary>
    private void InitViewData()
    {
        servercomp_dic = new List<ServerComponent>();
        lastServerComp = new ServerComponent();
        //lastServerComp.MyStart(view.Btn_Last.gameObject);
    }

    /// <summary>
    /// 设置账号
    /// </summary>
    private void SetDefaultAccount()
    {
        view.UIPopupList_AccountList.Clear();
        List<string> list = LoginModule.Instance.GetAllAccounts();
        for (int i = list.Count - 1; i >= 0; i--)
        {
            view.UIPopupList_AccountList.AddItem(list[i]);
        }
        if (list.Count <= 0)
            return;
        view.Ipt_Account.value = list[list.Count - 1];
        view.Ipt_Pwd.value = LoginModule.Instance.GetPasswordByAccount(view.Ipt_Account.value);
    }
    private void OnAccountListChange()
    {
        view.Ipt_Account.value = view.UIPopupList_AccountList.value;
        view.Ipt_Pwd.value = LoginModule.Instance.GetPasswordByAccount(view.Ipt_Account.value);
        view.Spt_BtnRememberPwdCheckMark.gameObject.SetActive(!string.IsNullOrEmpty(view.Ipt_Pwd.value));
    }

    public void ShowNoticeAndServerInfo()
    {
        if (!CommonFunction.XmlStringIsNull(ServerListManager.Instance.NoticeTitle)
            && !CommonFunction.XmlStringIsNull(ServerListManager.Instance.NoticeContent))
        {
            UISystem.Instance.ShowGameUI(AnnouncementView.UIName);
            UISystem.Instance.AnnouncementView.OnUpdateAnnouncement(ServerListManager.Instance.NoticeTitle
                , ServerListManager.Instance.NoticeContent);
            ServerListManager.Instance.NoticeTitle = string.Empty;
        }
        else
        {
            CheckNeedToAutoLogin();
        }

        UpdateServerInfo();
    }

    /// <summary>
    /// 更新选择服务器显示信息
    /// </summary>
    public void UpdateServerInfo()
    {
        if (LoginModule.Instance.CurServer == null)
        {
            view.Lbl_BtnServerLabel.text = ConstString.LOGIN_SERVER_SELECT;
            view.Lbl_BtnServerState.text = string.Empty;

        }
        else
        {
            view.Lbl_BtnServerLabel.text = LoginModule.Instance.CurServer.desc;
            view.Lbl_BtnServerState.text = LoginModule.Instance.GetServerStateString(LoginModule.Instance.CurServer.status);
        }
    }

    private void SetGobjVisible(GameObject go, bool status)     //设置对象是否激活 
    {
        go.SetActive(status);
    }

    #region Button Event
    private void ButtonEvent_Login(GameObject btn)  //账号登陆
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (string.IsNullOrEmpty(view.Ipt_Account.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_ACCOUNT_EMPTY);
            return;
        }
        if (string.IsNullOrEmpty(view.Ipt_Pwd.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_PASSWORD_EMPTY);
            return;
        }
        if (LoginModule.Instance.CurServer == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SERVER_NULL);
            return;
        }
        LoginModule.Instance.SetTmpAccount(view.Ipt_Account.value, view.Spt_BtnRememberPwdCheckMark.gameObject.activeSelf ? view.Ipt_Pwd.value : "");
        LoginModule.Instance.SendAuthorize(view.Ipt_Account.value, view.Ipt_Pwd.value);
        PlayerPrefsTool.WriteBool(AppPrefEnum.AutoLogin, view.Spt_BtnAutoLoginCheckMark.gameObject.activeSelf);
    }

    private void ButtonEvent_Start(GameObject btn)  //开始游戏
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        LoginModule.Instance.SendAuthorize_TWSDK();
    }

    private void ButtonEvent_Server(GameObject btn)  //更换服务器
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ChooseServerView.UIName);
    }

    private void ButtonEvent_RemenberPwd(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Spt_BtnRememberPwdCheckMark.gameObject.SetActive(!view.Spt_BtnRememberPwdCheckMark.gameObject.activeSelf);
    }

    private void ButtonEvent_AutoLogin(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Spt_BtnAutoLoginCheckMark.gameObject.SetActive(!view.Spt_BtnAutoLoginCheckMark.gameObject.activeSelf);
    }

    private void ButtonEvent_CancelAutoLogin(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        SetGobjVisible(view.Gobj_Login, true);
        SetAutoLogin(false);
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(AutoLogin);
    }


    private void ButtonEvent_KeyCodeBack()
    {
        if (!SDKManager.Instance.IsLoginActivity())
        {
            Main.Instance.LoginOut();
        }
    }

    private void ButtonEvent_Regist(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        SetLoginChildActive(false);
        UISystem.Instance.ShowGameUI(RegistAccountView.UIName);
    }
    #endregion

    public void SetLoginChildActive(bool state)
    {
        view.Gobj_LoginChild.SetActive(state);
    }

    /// <summary>
    /// 登陆服务器请求成功返回服务器列表数据
    /// </summary>
    public void AccountLoginSuccess()
    {
        //UpdateServerInfo();
        SetGobjVisible(view.Gobj_Login, true);
        SetGobjVisible(view.Gobj_Main, true);
        SetAutoLogin(false);
    }
    public void AccountLoginFailed()
    {
        SetGobjVisible(view.Gobj_Login, true);
        SetGobjVisible(view.Gobj_Main, true);
        SetAutoLogin(false);
    }

    private void SetAutoLogin(bool state)
    {
        SetGobjVisible(view.Gobj_AutoLogin, state);
        view.Spt_Lock.enabled = state;
        SetGobjVisible(view.Lbl_ClickChooseServer.gameObject, !state);
    }

    public void ShowLoginAnim()
    {
        //view.Tpos_LoginBGL.ResetToBeginning();
        //view.Tpos_LoginBGR.ResetToBeginning();
        //view.Tpos_LoginLogo.ResetToBeginning();
        //view.Tscale_LoginLogin.ResetToBeginning();
        //view.Talpha_LoginLogo.ResetToBeginning();

        //view.Tpos_LoginBGL.PlayForward();
        //view.Tpos_LoginBGR.PlayForward();
        //view.Tpos_LoginLogo.PlayForward();
        //view.Tscale_LoginLogin.PlayForward();
        //view.Talpha_LoginLogo.PlayForward();


    }
    public void EnterMainCity()
    {
        UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_LOGIN);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
    }

    public override void Uninitialize()
    {
        SetLoginChildActive(true);
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            SDKManager.Instance.KeyCodeBackEvent -= ButtonEvent_KeyCodeBack;
        }
    }


    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Login.gameObject).onClick = ButtonEvent_Login;
        //UIEventListener.Get(view.Btn_Last.gameObject).onClick = ButtonEvent_Last;
        UIEventListener.Get(view.Btn_Start.gameObject).onClick = ButtonEvent_Start;
        UIEventListener.Get(view.Btn_Server.gameObject).onClick = ButtonEvent_Server;
        UIEventListener.Get(view.Btn_RememberPwd.gameObject).onClick = ButtonEvent_RemenberPwd;
        UIEventListener.Get(view.Btn_AutoLogin.gameObject).onClick = ButtonEvent_AutoLogin;
        UIEventListener.Get(view.Btn_CancelAutoLogin.gameObject).onClick = ButtonEvent_CancelAutoLogin;
        UIEventListener.Get(view.Btn_Register.gameObject).onClick = ButtonEvent_Regist;
    }
}

