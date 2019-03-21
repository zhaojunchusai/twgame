using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class PVPNetWork
{
    public void RegisterMsg() 
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterArenaLobbyRespID, ReceiveEnterArenaLobby);
        NetWorkManager.Instance.RegisterEvent(MessageID.RefreshOpponentsRespID, ReceiveRefreshOpponents);
        NetWorkManager.Instance.RegisterEvent(MessageID.ClearArenaCDRespID, ReceiveClearArenaCD);
        NetWorkManager.Instance.RegisterEvent(MessageID.AddArenaTimesRespID,ReceiveAddArenaTimes);
        NetWorkManager.Instance.RegisterEvent(MessageID.ArenaRankListRespID,ReceiveArenaRankList);
        NetWorkManager.Instance.RegisterEvent(MessageID.ArenaRecordRespID, ReceiveArenaRecord);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartArenaRespID,ReceiveStartArena);
        NetWorkManager.Instance.RegisterEvent(MessageID.ArenaRewardRespID, ReceiveArenaReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.SaveDefenceArrayRespID, ReceiveSaveDefenceArray);
        NetWorkManager.Instance.RegisterEvent(MessageID.ArenaChallengeRespID, ReceiveArenaChallenge);
    }

    public void SendArenaChallenge(ArenaChallengeReq data)
    {
        Debug.Log("SendArenaChallenge");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ArenaChallengeReq>(0, data, MessageID.ArenaChallengeReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveArenaChallenge(Packet data) 
    {
        Debug.Log("ReceiveArenaChallenge");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ArenaChallengeResp resp = Serializer.Deserialize<ArenaChallengeResp>(data.ms);
        PVPModule.Instance.ReceiveArenaChallenge(resp);
    }

    /// <summary>
    /// 竞技场战斗日志
    /// </summary>
    public void SendArenaRecord(ArenaRecordReq data) 
    {
        Debug.Log("SendArenaRecord");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ArenaRecordReq>(0, data, MessageID.ArenaRecordReqID, PlayerData.Instance._SessionID);
   
    }
    /// <summary>
    /// 竞技场战斗日志
    /// </summary>
    public void ReceiveArenaRecord(Packet data) 
    {
        Debug.Log("ReceiveArenaRecord");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ArenaRecordResp resp = Serializer.Deserialize<ArenaRecordResp>(data.ms);
        PVPModule.Instance.ReceiveArenaRecord(resp);
    }

    /// <summary>
    /// 进入竞技场大厅
    /// </summary>
    public void SendEnterArenaLobby(EnterArenaLobbyReq data)
    {
        Debug.Log("SendEnterArenaLobby");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterArenaLobbyReq>(0, data, MessageID.EnterArenaLobbyReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 进入竞技场大厅
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEnterArenaLobby(Packet data)
    {
        Debug.Log("ReceiveEnterArenaLobby");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EnterArenaLobbyResp resp = Serializer.Deserialize<EnterArenaLobbyResp>(data.ms);
        PVPModule.Instance.ReceiveEnterArenaLobby(resp);
    }

    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public void SendSaveDefenceArray(SaveDefenceArrayReq data) 
    {
        Debug.Log("SendSaveDefenceArray");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SaveDefenceArrayReq>(0, data, MessageID.SaveDefenceArrayReqID, PlayerData.Instance._SessionID);

    }
    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public void ReceiveSaveDefenceArray(Packet data) 
    {
        Debug.Log("ReceiveSaveDefenceArray");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SaveDefenceArrayResp resp = Serializer.Deserialize<SaveDefenceArrayResp>(data.ms);
        PVPModule.Instance.ReceiveSaveDefenceArray(resp);
    }
    /// <summary>
    /// 开始竞技场
    /// </summary>
    public void SendStartArena(StartArenaReq data)
    {
        Debug.Log("SendStartArena");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartArenaReq>(0, data, MessageID.StartArenaReqID, PlayerData.Instance._SessionID);

    }

    /// <summary>
    /// 开始竞技场
    /// </summary>
    public void ReceiveStartArena(Packet data)
    {
        Debug.Log("ReceiveStartArena");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        StartArenaResp resp = Serializer.Deserialize<StartArenaResp>(data.ms);
        PVPModule.Instance.ReceiveStartArena(resp);
    }

    /// <summary>
    /// 竞技场结算
    /// </summary>
    public void SendArenaReward(ArenaRewardReq data)
    {
        Debug.Log("SendArenaReward");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ArenaRewardReq>(0, data, MessageID.ArenaRewardReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 竞技场结算
    /// </summary>
    public void ReceiveArenaReward(Packet data)
    {
        Debug.Log("ReceiveArenaReward");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ArenaRewardResp resp = Serializer.Deserialize<ArenaRewardResp>(data.ms);
        PVPModule.Instance.ReceiveArenaReward(resp);

    }
    /// <summary>
    /// 重新刷新对手
    /// </summary>
    public void SendRefreshOpponents(RefreshOpponentsReq data)
    {
        Debug.Log("SendRefreshOpponents");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RefreshOpponentsReq>(0, data, MessageID.RefreshOpponentsReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 重新刷新对手
    /// </summary>
    public void ReceiveRefreshOpponents(Packet data) 
    {
        Debug.Log("ReceiveRefreshOpponents");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        RefreshOpponentsResp resp = Serializer.Deserialize<RefreshOpponentsResp>(data.ms);
        PVPModule.Instance.ReceiveRefreshOpponents(resp);
    }
    /// <summary>
    /// 重置竞技场CD
    /// </summary>
    public void SendClearArenaCD(ClearArenaCDReq data) 
    {
        Debug.Log("SendClearArenaCD");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ClearArenaCDReq>(0, data, MessageID.ClearArenaCDReqID, PlayerData.Instance._SessionID);

    }
    /// <summary>
    /// 重置竞技场CD
    /// </summary>
    public void ReceiveClearArenaCD(Packet data)
    {
        Debug.Log("ReceiveClearArenaCD");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ClearArenaCDResp resp = Serializer.Deserialize<ClearArenaCDResp>(data.ms);
        PVPModule.Instance.ReceiveClearArenaCD(resp);
    }


    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public void SendAddArenaTimes(AddArenaTimesReq data)
    {
        Debug.Log("SendAddArenaTimes");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AddArenaTimesReq>(0, data, MessageID.AddArenaTimesReqID, PlayerData.Instance._SessionID);

    }
    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public void ReceiveAddArenaTimes(Packet data)
    {
        Debug.Log("ReceiveAddArenaTimes");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        AddArenaTimesResp resp = Serializer.Deserialize<AddArenaTimesResp>(data.ms);
        PVPModule.Instance.ReceiveAddArenaTimes(resp);
    }
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public void SendArenaRankList(ArenaRankListReq data)
    {
        Debug.Log("SendArenaRankList");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ArenaRankListReq>(0, data, MessageID.ArenaRankListReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public void ReceiveArenaRankList(Packet data) 
    {
        Debug.Log("ReceiveArenaRankList");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ArenaRankListResp resp = Serializer.Deserialize<ArenaRankListResp>(data.ms);
        PVPModule.Instance.ReceiveArenaRankList(resp);
    }

    public void RemoveMsg() 
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterArenaLobbyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.RefreshOpponentsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ClearArenaCDRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AddArenaTimesRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ArenaRankListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ArenaRecordRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartArenaRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ArenaRewardRespID);
    }
}
