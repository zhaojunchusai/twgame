using System.Text.RegularExpressions;
using UnityEngine;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;
using System.Text;

public class LoginModule : Singleton<LoginModule>
{
    public LoginNetWork mLoginNetWork;
    public string SocketIp = "";
    public int SocketPort = 0;
    public bool HeartBeatReceived = true;
    public ServerDetail CurServer = null;
    private const string ACC_FORMAT = "{0}@{1}";
    public bool isNormalLogin = false;

    public uint CurServerId
    {
        get { return (uint)CurServer.area_id; }
    }

    public AuthorizeResp AuthorizeResp;
    public List<int> RecommendSvrIDs = new List<int>();
    public List<ServerDetail> ServerList = new List<ServerDetail>();
    public bool IsLogining = false;
    public void Initialize()
    {
        if (mLoginNetWork == null)
        {
            mLoginNetWork = new LoginNetWork();
            mLoginNetWork.RegisterMsg();
        }
        GetAccountsFromLocal();
    }

    //public void SendAuthorize()
    //{
    //    AuthorizeReq data = new AuthorizeReq();
    //    data.code = SystemInfo.deviceUniqueIdentifier;
    //    data.sdk_type = 100;
    //    data.arg1 = SystemInfo.deviceUniqueIdentifier ;
    //    data.arg2 = GlobalConst.DEFAULTPASSWORD;
    //    mLoginNetWork.SendAuthorize(data);
    //    SetTmpAccount(data.arg1, data.arg2);
    //}

    public void AutoSendAuthorize()
    {
        if (IsLogining)
            return;

        if (_accounts.Count <= 0)
        {
            UISystem.Instance.LoginView.AccountLoginFailed();
            return;
        }
        if (!CheckCurServer())
        {
            UISystem.Instance.LoginView.AccountLoginFailed();
            return;
        }
        AuthorizeReq data = new AuthorizeReq();
        data.code = string.Format(ACC_FORMAT, _accounts[_accounts.Count - 1], CurServerId);
        data.sdk_type = (uint)GlobalConst.PLATFORM;
        data.arg1 = string.Format(ACC_FORMAT, _accounts[_accounts.Count - 1], CurServerId);
        data.arg2 = _passwords[_passwords.Count - 1];
        mLoginNetWork.SendAuthorize(data);
    }

    public void SendAuthorize(string userName, string password)
    {
        if (IsLogining)
            return;
        if (!CheckCurServer())
        {
            return;
        }
        AuthorizeReq data = new AuthorizeReq();
        data.code = string.Format(ACC_FORMAT, userName, CurServerId);
        data.sdk_type = (uint)GlobalConst.PLATFORM;
        data.arg1 = string.Format(ACC_FORMAT, userName, CurServerId);
        data.arg2 = password;
        mLoginNetWork.SendAuthorize(data);
    }
    public void SendAuthorize_TWSDK()
    {
        if (IsLogining)
            return;
        if (!CheckCurServer())
        {
            return;
        }
        if (GlobalConst.ISOPENSDKOPERATE)
        {
            switch (GlobalConst.PLATFORM)
            {
                case TargetPlatforms.Android_7725OL:
                case TargetPlatforms.Android_7725:
                    {
                        if (SDKManager.Instance.GetSDKUserData() != null)
                        {
                            AuthorizeReq data = new AuthorizeReq();
                            data.code = SDKManager.Instance.GetSDKUserData().sign;
                            data.sdk_type = (uint)GlobalConst.PLATFORM;
                            data.arg1 = SDKManager.Instance.GetSDKUserData().userid + "@" + CurServerId.ToString();
                            data.arg2 = SDKManager.Instance.GetSDKSignInfo();
                            mLoginNetWork.SendAuthorize(data);
                        }
                        else
                        {
                            Debug.LogError("get sdk user data is null!!!");
                        }
                    }
                    break;
                default:
                    { }
                    break;
            }
        }
    }

    public void SendEnterGameServer()
    {
        if (!CheckCurServer())
        {
            return;
        }
        SetGameNetWork(CurServer);

        EnterGameServerReq data = new EnterGameServerReq();
        data.area_id = CurServerId;
        data.session_id = PlayerData.Instance._SessionID;
        data.accid = PlayerData.Instance._AccountID;
        data.accname = PlayerData.Instance._AccountName;
        data.sign = PlayerData.Instance.Sign;
        data.timestamp = PlayerData.Instance.TimeStamp;
        data.device_id = SystemInfo.deviceUniqueIdentifier;
        data.sdk_type = (uint)GlobalConst.PLATFORM;
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            if (SDKManager.Instance.GetSDKUserData() != null)
            {
                if (SDKManager.Instance.IsBindingPlatform() == 0)
                {
                    data.arg1 = "1;";
                }
                else
                {
                    data.arg1 = "0;";
                }
                data.arg1 += SDKManager.Instance.GetSDKUserData().logintype;
                data.arg1 += ";";
                data.arg1 += SDKManager.Instance.GetSDKUserData().nickname;
            }
            else
            {
                Debug.LogError("get sdk user data is null!!!");
            }
        }
        mLoginNetWork.SendEnterGameServer(data);
    }

    public void SendReqCharacterName()
    {
        CharnameReq data = new CharnameReq();
        data.area_id = CurServerId;
        mLoginNetWork.SendCharacterName(data);
    }

    public void ReceiveAuthorize(AuthorizeResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.SetPlayerAuthorizeInfo(data);
            //AuthorizeResp = data;
            //UISystem.Instance.LoginView.AccountLoginSuccess();
            if (UISystem.Instance.UIIsOpen(RegistAccountView.UIName))
            {
                UISystem.Instance.CloseGameUI(RegistAccountView.UIName);
            }

            SaveTmpAccount();
            //SetLastLoginServer();
            /*
            if (data.notice_body != null && !CommonFunction.XmlStringIsNull(data.notice_body))
            {
                UISystem.Instance.ShowGameUI(AnnouncementView.UIName);
                UISystem.Instance.AnnouncementView.OnUpdateAnnouncement(data.notice_title, data.notice_body);
            }
            */
            SendEnterGameServer();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
            UISystem.Instance.LoginView.AccountLoginFailed();
            IsLogining = false;
        }
    }

    public void SetGameNetWork(ServerDetail info)
    {
        NetWorkManager.Instance.SetGameNetWork(info.url);
    }

    public void ReceiveEnterGame(EnterGameServerResp data)
    {
        Debug.Log("ReceiveEnterGame result = " + data.result + " session_id = " + data.session_id);
        NetWorkManager.Instance.mHttpGameNetWork.SetEncryptInfo(data.encrypt_args);
        switch ((ErrorCodeEnum)data.result)
        {
            case ErrorCodeEnum.SUCCESS:
                {
                    PlayerData.Instance._SessionID = data.session_id;
                    MainCityModule.Instance.LockFuncs = data.lock_funcs;
                    MainCityModule.Instance.SetGameAnnouncement(data.notice);
                    EnterGame(data.connecter_info, data.char_info, data.other_info, false);
                    break;
                }
            case ErrorCodeEnum.NoSuchAccount:
                {
                    PlayerData.Instance._SessionID = data.session_id;
                    MainCityModule.Instance.LockFuncs = data.lock_funcs;
                    Main.Instance.StateMachine.ChangeState(CreateRoleState.StateName);
                    UISystem.Instance.CreateCharacterView.SetNickName(data.default_charname);
                    break;
                }
            default:
                Debug.Log("ERROR: ReceiveEnterGame - data.result can't be recognized  code = " + data.result);
                ErrorCode.ShowErrorTip(data.result);
                break;
        }
        IsLogining = false;
    }

    private void SendPlayerLoginAreaServerID()
    {
        if (CurServer == null)
            return;
        NotifyPlayerLoginAreaServer data = new NotifyPlayerLoginAreaServer();
        data.accname = PlayerData.Instance._AccountName;
        data.area_id = CurServerId;
        mLoginNetWork.SendPlayerLoginAreaServerID(data);
    }

    /// <summary>
    /// 请求随机昵称
    /// </summary>
    /// <param name="serverid">当前服务器ID</param>
    public void SendCharname(uint serverid)
    {
        CharnameReq data = new CharnameReq();
        data.area_id = serverid;
        mLoginNetWork.SendCharname(data);
    }

    /// <summary>
    /// 随机昵称返回
    /// </summary>
    /// <param name="resp"></param>
    public void ReceiveCharname(CharnameResp resp)
    {
        if (UISystem.Instance.UIIsOpen(CreateCharacterView.UIName))
        {
            UISystem.Instance.CreateCharacterView.SetNickName(resp.charname);
        }
        else
        {
            SystemSettingModule.Instance.OnRandomResponse(resp.charname);

        }
    }

    public void RegisterAccountReq(string acc, string pwd)
    {
        if (!CheckCurServer())
        {
            return;
        }
        RegisterReq req = new RegisterReq();
        req.accname = string.Format(ACC_FORMAT, acc, CurServerId); ;
        req.password = pwd;
        mLoginNetWork.RegisterAccountReq(req);
        SetTmpAccount(acc, pwd);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="gender"></param>
    public void SendCreateCharacter(uint gender, string nickname, uint avatarid)
    {
        CreateCharReq data = new CreateCharReq();
        data.accid = PlayerData.Instance._AccountID;
        data.charname = nickname;
        data.avatar_id = avatarid;
        data.gender = gender;
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            if (SDKManager.Instance.IsBindingPlatform() == 0)
            {
                data.arg1 = "1;";
            }
            else
            {
                data.arg1 = "0;";
            }
            data.arg1 += SDKManager.Instance.GetSDKUserData().logintype;
            data.arg1 += ";";
            data.arg1 += SDKManager.Instance.GetSDKUserData().nickname;
        }
        mLoginNetWork.SendCreateCharacter(data);
    }

    public void ReceiveCreateCharacter(CreateCharResp resp)
    {
        //HintManager.Instance.CloseMiniloading();
        Debug.Log("ReceiveCreateCharacter resp.result = " + resp.result);
        switch ((ErrorCodeEnum)resp.result)
        {
            case ErrorCodeEnum.SUCCESS:
                {
                    //EnterGuideGame(resp.connecter_info, resp.char_info, resp.other_info);
                    EnterGame(resp.connecter_info, resp.char_info, resp.other_info, true);
                }
                break;
            case ErrorCodeEnum.REPEATNICKNAME:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,
                                                              ConstString.CREATEROLE_NICKNAME_REPEAT);
                }
                break;
            case ErrorCodeEnum.FAILED:
                {
                    Debug.LogError("create role fail！！");
                }
                break;
            default:
                ErrorCode.ShowErrorTip(resp.result);
                //Debug.LogError("Error Result:" + resp.result.ToString());
                break;
        }
    }

    //public void ReceiveServerError(NotifyServerError data)
    //{
    //    HintManager.Instance.AddHintStr(string.Format(ConstString.SERVER_ERROR,data.msg_id));
    //}

    public void EnterGame(SrvInfo info, CharInfo char_info, OtherInfo other_info, bool createRole)
    {
        if (info != null)
        {
            SocketIp = info.ip;
            SocketPort = info.port;
        }
        PlayerData.Instance.SetRoleData(char_info, other_info, createRole);
        PlayerData.Instance.UpdateCastleInfo(char_info.b_info.castle_info);
        SendPlayerLoginAreaServerID();
        ConectToSocketNetwork();
        if (GuideManager.Instance.IsSpecialFightFinish())
        {
            UISystem.Instance.HintView.ShowFightLoading(MainCityModule.Instance.CheckOpenMainCity, true);
            Scheduler.Instance.AddTimer(0.3f, false, GoToMainCity);
        }
        else
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.NeedSpecialFight);
            //StageInfo stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(GlobalConst.FIRST_STAGE_ID);
            //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
            //UISystem.Instance.FightView.SetViewInfo(EFightType.eftNewGuide, stageInfo, PlayerData.Instance._SoldierDepot._soldierList);
        }
        LocalNotificationManager.Instance.SPPresent();
        string serverID = PlayerData.Instance._ServerID.ToString();
        string roleID = PlayerData.Instance._RoleID.ToString();
        string nickname = PlayerData.Instance._NickName.ToString();
        uint accountID = PlayerData.Instance._AccountID;
        string accountName = PlayerData.Instance._AccountName.ToString();

        //SDK传值//
        SDKManager.Instance.QuickEnterGame(serverID, roleID, nickname, accountID, accountName);
        SDKManager.Instance.SetAliasAndTags(PlayerData.Instance._RoleID.ToString(), char_info.b_info.push_tag.ToArray());
    }

    private void GoToMainCity()
    {
        Main.Instance.StateMachine.ChangeState(MainCityState.StateName);
        MainCityModule.Instance.DelayShowCommentView(true);
        MainCityModule.Instance.ShowGameAnnouncement();
        TaskModule.Instance.StartLivenessTip();
    }

    public void EnterGuideGame(SrvInfo info, CharInfo char_info, OtherInfo other_info)
    {
        if (info != null)
        {
            SocketIp = info.ip;
            SocketPort = info.port;
        }
        PlayerData.Instance.SetRoleData(char_info, other_info, false);
        PlayerData.Instance.UpdateCastleInfo(char_info.b_info.castle_info);
        SendPlayerLoginAreaServerID();
        ConectToSocketNetwork();
        StageInfo stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(GlobalConst.FIRST_STAGE_ID);
        if (stageInfo == null)
        {
            UISystem.Instance.HintView.ShowFightLoading(null, true);
            Scheduler.Instance.AddTimer(0.3f, false, GoToMainCity);
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
            UISystem.Instance.FightView.SetViewInfo(EFightType.eftNewGuide, stageInfo, PlayerData.Instance._SoldierDepot._soldierList);
        }
    }

    private bool CheckCurServer()
    {
        if (CurServer == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SERVER_NULL);
            return false;
        }

        if (!GlobalConst.IS_TESTAPK && CurServer.status == (int)GameServerState.Maintenance)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_SERVER_BUILDING);
            return false;
        }

        return true;
    }

    #region Socket Connection & HeartBeat

    public void ConectToSocketNetwork()
    {
        Debug.LogWarning("ConectToSocketNetwork");
        if (string.IsNullOrEmpty(SocketIp) || SocketPort == 0)
        {
            Debug.LogError("Error: SocketIp or SocketPort is wrong!!");
            return;
        }

        Scheduler.Instance.RemoveTimer(CheckSocketState);
        Scheduler.Instance.AddTimer(CONNECT_INTERVAL, false, CheckSocketState);
        NetWorkManager.Instance.SetSocketNetWork(SocketIp, SocketPort);
        NetWorkManager.Instance.mSocketNetWork.SocketExceptionEvent = SocketErr;
        mLoginNetWork.EnterConnecterSrvReq();
    }

    public void CheckSocketState()
    {
        Debug.LogWarning("CheckSocketState :" + NetWorkManager.Instance.mSocketNetWork.IsConnect());
        if (NetWorkManager.Instance.mSocketNetWork.IsConnect())
        {
            StartHeartBeat();
        }
        else
        {
            SocketConnectFaild();
        }
    }

    private void SocketErr(bool state)
    {
        //Debug.Log("SocketErr" +state);
        //SocketConnectFaild();
    }
    private const int MAX_CONNECT_TIME = 3;//多少次没反应开始重连
    private const int MAX_RECONNECT_TIME = 20;//重连多少次没连上提示掉线
    private const int CONNECT_INTERVAL = 30;//检测间隔
    private int _connectTimer = MAX_CONNECT_TIME;
    private int _reconnectTimer = MAX_RECONNECT_TIME;
    private void SocketConnectFaild()
    {
        Debug.LogWarning("SocketConnectExption: _reconnectTimer=" + _reconnectTimer);
        if (_reconnectTimer > 0)
        {
            --_reconnectTimer;
            ConectToSocketNetwork();
        }
        else
        {
            // MAX_CONNECT_TIME 次链接均失败 提示掉线
            CommonFunction.ShowOfflineTip();
        }
    }

    public void StartHeartBeat()
    {
        _connectTimer = MAX_CONNECT_TIME;
        Scheduler.Instance.AddTimer(CONNECT_INTERVAL, true, SendHeartBeat);
    }

    public void ReceiveHeartBeat(long time)
    {
        if (time > Main.mTime)
            Main.mTime = time;
        _connectTimer = MAX_CONNECT_TIME;
        _reconnectTimer = MAX_RECONNECT_TIME;
        Debug.LogWarning("ReceiveHeartBeat: _connectTimer=" + _connectTimer + " _reconnectTimer=" + _reconnectTimer + "currentTime:" + time);
    }

    private void SendHeartBeat()
    {
        --_connectTimer;
        Debug.LogWarning("SendHeartBeat: _connectTimer=" + _connectTimer);
        if (_connectTimer <= 0)
        {
            _reconnectTimer = MAX_RECONNECT_TIME;
            ConectToSocketNetwork();
            Scheduler.Instance.RemoveTimer(SendHeartBeat);
        }
        else
        {
            mLoginNetWork.SendHeartBeat();
        }
    }
    #endregion

    private void SendLoginServerID()
    {
        //NotifyPlayerLoginAreaServer data = new NotifyPlayerLoginAreaServer();
        //data.accname = PlayerDataManager.Instance.Accname;
        //data.area_id = CurServerId;
        //mLoginNetWork.SendPlayerLoginAreaServerID(data);
        SelectAreaInfoReq info = new SelectAreaInfoReq();
        //info.accname = PlayerDataManager.Instance.Accname;
        info.area_id = CurServerId;
        mLoginNetWork.SendSelectGameServer(info);
    }

    public void PlayerLoginOut()
    {
        mLoginNetWork.SendPlayerLoginOut();
    }

    private void OpenCreateCharacter(EnterGameServerResp data)
    {

    }
    public bool CheckNameRule(string name)
    {
        bool result = false;

        if (string.IsNullOrEmpty(name))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHAR_NAME_NULL);
            return false;
        }
        else
        {
            result = CommonFunction.CheckStringRule(name, 4, 12);
        }
        return result;
    }

    public string GetServerStateString(int state)
    {
        string result = "";

        switch (state)
        {
            case (int)GameServerState.Free:
                {
                    result = ConstString.SERVER_STATE_FREE;
                    break;
                }
            case (int)GameServerState.Busy:
                {
                    result = ConstString.SERVER_STATE_BUSY;
                    break;
                }
            case (int)GameServerState.Maintenance:
                {
                    result = ConstString.SERVER_STATE_MAINTENANCE;
                    break;
                }
            default:
                Debug.LogError("Server State Can't Recognize!");
                break;
        }

        return result;
    }

    #region 账号操作  存取
    private string _tmpAcc;
    private string _tmpPwd;
    private List<string> _accounts = new List<string>();
    private List<string> _passwords = new List<string>();
    public void SetTmpAccount(string account, string password)
    {
        Debug.Log("SetTmpAccount " + " " + account + " " + password);
        _tmpAcc = account;
        _tmpPwd = password;
    }
    private void SaveTmpAccount()
    {
        Debug.Log("SaveTmpAccount " + " " + _tmpAcc + " " + _tmpPwd);
        SaveAccount(_tmpAcc, _tmpPwd);
    }

    private void SaveAccount(string account, string password)
    {
        if (string.IsNullOrEmpty(account))
            return;

        if (_accounts.Contains(account))
        {
            int index = _accounts.IndexOf(account);
            _passwords.RemoveAt(index);
            _accounts.RemoveAt(index);
        }
        if (_accounts.Count > 4)
        {
            _passwords.RemoveAt(0);
            _accounts.RemoveAt(0);
        }
        _accounts.Add(account);
        _passwords.Add(password);
        SaveAccountsToLocal();
    }
    public string GetPasswordByAccount(string account)
    {
        if (_accounts.Contains(account))
        {
            int index = _accounts.IndexOf(account);
            return _passwords[index];
        }

        return "";
    }

    public List<string> GetAllAccounts()
    {
        return _accounts;
    }

    private void SaveAccountsToLocal()
    {
        StringBuilder stringBuilder = new StringBuilder();
        for (int i = 0; i < _accounts.Count; i++)
        {
            if (string.IsNullOrEmpty(_accounts[i]))
                continue;
            stringBuilder.AppendFormat("{0},{1};", _accounts[i], _passwords[i]);
        }

        PlayerPrefsTool.WriteString(AppPrefEnum.Account, stringBuilder.ToString());
    }

    private void GetAccountsFromLocal()
    {
        _accounts.Clear();
        _passwords.Clear();

        string accstr = PlayerPrefsTool.ReadString(AppPrefEnum.Account);

        string[] accounts = accstr.Split(';');
        for (int i = 0; i < accounts.Length; i++)
        {
            string[] acc = accounts[i].Split(',');
            if (acc.Length > 1)
            {
                _accounts.Add(acc[0]);
                _passwords.Add(acc[1]);
            }
        }
    }
    #endregion

    public void Uninitialize()
    {
        if (mLoginNetWork != null)
            mLoginNetWork.RemoveMsg();
        mLoginNetWork = null;

        SocketIp = "";
        SocketPort = 0;
        HeartBeatReceived = true;
        CurServer = null;
        AuthorizeResp = null;
        Scheduler.Instance.RemoveTimer(SendHeartBeat);

    }

}
