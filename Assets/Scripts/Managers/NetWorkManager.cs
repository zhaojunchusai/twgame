using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class NetWorkManager : Singleton<NetWorkManager>
{
    public TSocket mSocketNetWork;
    /// <summary>
    /// 登陆服务器
    /// </summary>
    public HttpNetWork mHttpLoginNetWork;
    /// <summary>
    /// 游戏服务器
    /// </summary>
    public HttpNetWork mHttpGameNetWork;
    /// <summary>
    /// 资源服务器
    /// </summary>
    public HttpNetWork mHttpDLCNetWork;
    private Dictionary<uint, PacketHandle> _eventList = new Dictionary<uint, PacketHandle>();
    public void ModuleInitialize()
    {
        //DLCModule.Instance.Initialize();
        LoginModule.Instance.Initialize();
        BackPackModule.Instance.Initialize();
        EquipModule.Instance.Initialize();
        DrowEquipModule.Instance.Initialize();
        SoldierModule.Instance.Initialize();
        SkillModule.Instance.Initialize();
        FightRelatedModule.Instance.Initialize();
        MainCityModule.Instance.Initialize();
        RecruitModule.Instance.Initialize();
        MailModule.Instance.Initialize();
        TaskModule.Instance.Initialize();
        FriendModule.Instance.Initialize();
        SystemSettingModule.Instance.Initialize();
        PVPModule.Instance.Initialize();
        CastleModule.Instance.Initialize();
        StoreModule.Instance.Initialize();
        GMModule.Instance.Initialize();
        GameActivityModule.Instance.Initialize();
        GateModule.Instance.Initialize();
        ChatModule.Instance.Initialize();
        RankModule.Instance.Initialize();
        UnionModule.Instance.Initialize();
        UnionPrisonModule.Instance.Initialize();
        PrisonModule.Instance.Initialize();
        NoviceTaskModule.Instance.Initialize();
        SupermacyModule.Instance.Initialize();
        CaptureTerritoryModule.Instance.Initialize();
        QualifyingModule.Instance.Initialize();
        LifeSpiritModule.Instance.Initialize();
        AchievementModule.Instance.Initialize();
        PetSystemModule.Instance.Initialize();
		CrossServerWarModule.Instance.Initialize();
    }
    public void ModuleUninitialize()
    {
        LoginModule.Instance.Uninitialize();
        //DLCModule.Instance.Uninitialize();
        BackPackModule.Instance.Uninitialize();
        EquipModule.Instance.Uninitialize();
        DrowEquipModule.Instance.Uninitialize();
        SoldierModule.Instance.Uninitialize();
        SkillModule.Instance.Uninitialize();
        FightRelatedModule.Instance.Uninitialize();
        MainCityModule.Instance.Uninitialize();
        RecruitModule.Instance.Uninitialize();
        MailModule.Instance.Uninitialize();
        TaskModule.Instance.Uninitialize();
        FriendModule.Instance.Uninitialize();
        SystemSettingModule.Instance.Uninitialize();
        PVPModule.Instance.Uninitialize();
        CastleModule.Instance.Uninitialize();
        StoreModule.Instance.Uninitialize();
        GMModule.Instance.Uninitialize();
        ChatModule.Instance.Uninitialize();
        RankModule.Instance.Uninitialize();
        UnionModule.Instance.Uninitialize();
        UnionPrisonModule.Instance.Uninitialize();
        PrisonModule.Instance.Uninitialize();
        NoviceTaskModule.Instance.Uninitialize();
        GateModule.Instance.Uninitialize();
        SupermacyModule.Instance.Uninitialize();
        CaptureTerritoryModule.Instance.Uninitialize();
        QualifyingModule.Instance.Uninitialize();
        LifeSpiritModule.Instance.Uninitialize();
        AchievementModule.Instance.Uninitialize();
        PetSystemModule.Instance.Uninitialize();
		CrossServerWarModule.Instance.Uninitialize();
    }
    public void Initialize()
    {
        //if (mHttpDLCNetWork == null)
        //{
        // mHttpDLCNetWork = new HttpNetWork(AppConfig.GetAppInfo().GetDlcIP());
        //int index = UnityEngine.Random.Range(0, ConfigManager.Instance.mServerAddConfig.mServerAddress.Count);

        //}
        //mHttpDLCNetWork.mEventProcessor = EnvntReceive;
        //mHttpLoginNetWork = new HttpNetWork(ConfigManager.Instance.mAppConfig.GetAppInfo().GetWebIP());
        ModuleInitialize();
    }
    public void SetLoginNetWork()
    {
        if (mHttpLoginNetWork == null)
        {
            mHttpLoginNetWork = new HttpNetWork(ServerInfoManager.Instance._currentServerinfo.serverAddress);
        }
        mHttpLoginNetWork.mEventProcessor = EnvntReceive;
        mHttpLoginNetWork.mErrorMsg = ErrorMsg;
    }
    public void SetSocketNetWork(string ip, int port, TSocket.ConnectResult function = null)
    {
        Debug.LogWarning(" SetSocketNetWork >>>>>>>>>>>>>> ip = " + ip + " port = " + port);
        if (mSocketNetWork == null)
            mSocketNetWork = new TSocket(ip, port, "GameSocket");
        else
        {
            mSocketNetWork.DisConnect();
            mSocketNetWork.SetIPandPort(ip, port, "GameSocket");
        }
        if (function != null)
            mSocketNetWork.mConnectResult = function;
        mSocketNetWork.Connect();
        mSocketNetWork.mEventProcessor = EnvntReceive;
    }
    public void SetGameNetWork(string url)
    {
        if (mHttpGameNetWork != null)
            mHttpGameNetWork.DisConnect();
        mHttpGameNetWork = new HttpNetWork(url);
        mHttpGameNetWork.mEventProcessor = EnvntReceive;
        mHttpGameNetWork.mErrorMsg = ErrorMsg;
    }
    /// <summary>
    /// 当接到服务器推送信息时
    /// 给服务器反馈
    /// 此处引用 fogs.proto.msg;
    /// </summary>
    /// <param name="msgId"></param>
    public void SocketMsgFeedBack(uint msgId)
    {
        //Debug.Log("SocketMsgFeedBack");
        //MsgRecvConfirm confirm = new MsgRecvConfirm();
        //confirm.msg_id = msgId;
        //confirm.accid = PlayerDataManager.Instance.Accid;
        //mSocketNetWork.SendMsg<MsgRecvConfirm>(0, confirm, MessageID.MsgRecvConfirmID, PlayerDataManager.Instance.SessionID);
    }
    public void EnvntReceive(Packet data)
    {
        if (data == null) return;
        if (_eventList.ContainsKey(data.header.mMsgID))
        {
            _eventList[data.header.mMsgID](data);
        }
        else
        {
            Debug.Log("MSGID == NULL  " + data.header.mMsgID.ToString("x5") + "  MSGTYPE == " + data.header.mType + "  mAccountID == " + data.header.mAccountID);
        }
    }
    public void RegisterEvent(uint msgID, PacketHandle eventReceive)
    {
        if (_eventList.ContainsKey(msgID))
        {
            _eventList.Remove(msgID);
        }
        _eventList.Add(msgID, eventReceive);
    }

    public void ErrorMsg(byte[] bytes, string error)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        ErrorCode.ShowErrorTip((int)ErrorCodeEnum.OFFLINE);
        //switch (error)
        //{
        //    case "TimeOut":
        //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.OFFLINE_TIP);
        //        break;
        //    default:
        //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.OFFLINE_TIP);
        //        break;
        //}
        //Debug.LogError(error);
    }

    public void RemoveEvent(uint msgID)
    {
        if (_eventList.ContainsKey(msgID))
        {
            _eventList.Remove(msgID);
        }
    }
    public void Uninitialize()
    {
        if (mSocketNetWork != null)
            mSocketNetWork.Uninitialize();

        if (mHttpLoginNetWork != null)
            mHttpLoginNetWork.Uninitialize();

        ModuleUninitialize();
    }
}
