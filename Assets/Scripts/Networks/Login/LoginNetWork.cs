using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class LoginNetWork
{
    public void SendAuthorize(AuthorizeReq data)
    {
        Debug.LogWarning("data.sdk_type: " + data.sdk_type);
        Debug.Log("SendAuthorize " + data.arg1 + " "+data.arg2);
//        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpLoginNetWork.SendMsg<AuthorizeReq>(0, data, MessageID.AuthorizeReqID, 0);
    }

    public void SendSelectGameServer(SelectAreaInfoReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        //NetWorkManager.Instance.mHttpLoginNetWork.SendMsg<SelectAreaInfoReq>(0, data, MessageID.SelectAreaInfoID,
                                                                             //PlayerDataManager.Instance.SessionID);
    }

    public void ReceiveSelectGameServer(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
    }

    public void SendEnterGameServer(EnterGameServerReq data)
    {
        Debug.Log("SendEnterGameServer");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterGameServerReq>(0, data, MessageID.EnerGameServerReqID,
                                                                            PlayerData.Instance._SessionID);
    }


    public void SendCharacterName(CharnameReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        //NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CharnameReq>(0, data, MessageID.CharnameReqID,
        //                                                              PlayerDataManager.Instance.SessionID);
    }

    public void SendPlayerLoginAreaServerID(NotifyPlayerLoginAreaServer data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        Debug.Log("SendPlayerLoginAreaServerID");
        NetWorkManager.Instance.mHttpLoginNetWork.SendMsg<NotifyPlayerLoginAreaServer>(0, data, MessageID.NotifyPlayerLoginAreaServerID, 0,false);
    }

    public void SendPlayerLoginOut()
    {
        Debug.Log("SendPlayerLoginOut");
        PlayerLogoutReq data = new PlayerLogoutReq();
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        //NetWorkManager.Instance.mHttpLoginNetWork.SendMsg<PlayerLogoutReq>(0, data, MessageID.PlayerLogoutReqID, PlayerDataManager.Instance.SessionID);
    }

    public void EnterConnecterSrvReq()
    {
        EnterConnecterSrvReq req = new EnterConnecterSrvReq();
        req.accid = PlayerData.Instance._AccountID;
        NetWorkManager.Instance.mSocketNetWork.SendMsg<EnterConnecterSrvReq>(0, req, MessageID.EnterConnecterSrvReqID, PlayerData.Instance._SessionID);
    }

    public void EnterConnecterSrvResp(Packet data)
    {
        EnterConnecterSrvResp resp = Serializer.Deserialize<EnterConnecterSrvResp>(data.ms);
        
    }

    public void RegisterAccountReq(RegisterReq req)
    {
        NetWorkManager.Instance.mHttpLoginNetWork.SendMsg<RegisterReq>(0, req, MessageID.RegisterReqID, PlayerData.Instance._SessionID);
    }

    public void SendHeartBeat()
    {
        Heartbeat req = new Heartbeat();
        req.accid = PlayerData.Instance._AccountID;
        NetWorkManager.Instance.mSocketNetWork.SendMsg<Heartbeat>(0, req, MessageID.HeartbeatID,
            PlayerData.Instance._AccountID);
    }

    public void ReceiveHeartBeat(Packet data)
    {
        Heartbeat tData = Serializer.Deserialize<Heartbeat>(data.ms);
        LoginModule.Instance.ReceiveHeartBeat(tData.server_time);
    }
    
    public void ReceivePlayerLoginAreaServerID(Packet data)
    {

        //UISystem.Instance.HintView.SetLoadingVisible(false);
    }


    public void ReceiveAuthorize(Packet data)
    {
        Debug.Log("ReceiveAuthorize");
//        UISystem.Instance.HintView.SetLoadingVisible(false);
        AuthorizeResp tData = Serializer.Deserialize<AuthorizeResp>(data.ms);
        LoginModule.Instance.ReceiveAuthorize(tData);
    }

    public void ReceiveEnterGame(Packet data)
    {
        Debug.Log("ReceiveEnterGame");
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        EnterGameServerResp tData = Serializer.Deserialize<EnterGameServerResp>(data.ms);
        LoginModule.Instance.ReceiveEnterGame(tData);
    }

    public void ReceiveServerError(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        //NotifyServerError tData = Serializer.Deserialize<NotifyServerError>(data.ms);
        //LoginModule.Instance.ReceiveServerError(tData);
    }

    /// <summary>
    /// 请求默认名字
    /// </summary>
    public void SendCharname(CharnameReq data)
    {
        Debug.Log("SendCharname");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CharnameReq>(0, data, MessageID.CharnameReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveCharname(Packet data)
    {
        Debug.Log("ReceiveCharname");
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        CharnameResp tData = Serializer.Deserialize<CharnameResp>(data.ms);
        LoginModule.Instance.ReceiveCharname(tData);
    }

    /// <summary>
    /// 创建角色
    /// </summary>
    /// <param name="data"></param>
    public void SendCreateCharacter(CreateCharReq data)
    {
        Debug.Log("SendCreateCharacter");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CreateCharReq>(0, data, MessageID.CreateCharReqID,
                                                                        PlayerData.Instance._SessionID);
    }

    public void ReceiveCreateCharacter(Packet data)
    {
        Debug.Log("ReceiveCreateCharacter");

        //UISystem.Instance.HintView.SetLoadingVisible(false);
        CreateCharResp tData = Serializer.Deserialize<CreateCharResp>(data.ms);
        LoginModule.Instance.ReceiveCreateCharacter(tData);
    }


    public void RegisterMsg() 
    {
        //NetWorkManager.Instance.RegisterEvent(MessageID.NotifyServerErrorID, ReceiveServerError);
        NetWorkManager.Instance.RegisterEvent(MessageID.AuthorizeRespID, ReceiveAuthorize);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnerGameServerRespID, ReceiveEnterGame);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyPlayerLoginAreaServerRespID, ReceivePlayerLoginAreaServerID);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterConnecterSrvRespID, EnterConnecterSrvResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.HeartbeatID, ReceiveHeartBeat);
        NetWorkManager.Instance.RegisterEvent(MessageID.SelectAreaInfoRespID, ReceiveSelectGameServer);
        NetWorkManager.Instance.RegisterEvent(MessageID.CharnameRespID, ReceiveCharname);
        NetWorkManager.Instance.RegisterEvent(MessageID.CreateCharRespID, ReceiveCreateCharacter);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyServerErrorID);
        NetWorkManager.Instance.RemoveEvent(MessageID.VersionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AuthorizeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnerGameServerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyPlayerLoginAreaServerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CharnameRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CreateCharRespID);
    }

}
