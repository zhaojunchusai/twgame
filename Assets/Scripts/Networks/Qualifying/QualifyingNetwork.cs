using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class QualifyingNetwork
{
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterPoleLobbyRespID, ReceiveEnterPoleLobby);
        NetWorkManager.Instance.RegisterEvent(MessageID.SavePoleArrayRespID, ReceiveSavePoleArray);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartPoleRespID, ReceiveStartPole);
        NetWorkManager.Instance.RegisterEvent(MessageID.PoleRewardRespID, ReceivePoleReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.PoleRecordRespID, ReceivePoleRecord);
        NetWorkManager.Instance.RegisterEvent(MessageID.PoleRevengeRespID, ReceivePoleRevenge);
        NetWorkManager.Instance.RegisterEvent(MessageID.PoleBuyTimesRespID, ReceivePoleBuyTimes);
        NetWorkManager.Instance.RegisterEvent(MessageID.MatchPoleRespID, ReceiveMatchPole);
        NetWorkManager.Instance.RegisterEvent(MessageID.ClearPoleMatchCDRespID, ReceiveClearPoleMatchCD);
    }

    public void SendEnterPoleLobby(EnterPoleLobbyReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterPoleLobbyReq>(0, data, MessageID.EnterPoleLobbyReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveEnterPoleLobby(Packet data)
    {
        EnterPoleLobbyResp resp = Serializer.Deserialize<EnterPoleLobbyResp>(data.ms);
        QualifyingModule.Instance.ReceiveEnterPoleLobby(resp);
    }

    public void SendSavePoleArray(SavePoleArrayReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SavePoleArrayReq>(0, data, MessageID.SavePoleArrayReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveSavePoleArray(Packet data)
    {
        SavePoleArrayResp resp = Serializer.Deserialize<SavePoleArrayResp>(data.ms);
        QualifyingModule.Instance.ReceiveSavePoleArray(resp);
    }

    public void SendStartPole(StartPoleReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartPoleReq>(0, data, MessageID.StartPoleReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveStartPole(Packet data)
    {
        StartPoleResp resp = Serializer.Deserialize<StartPoleResp>(data.ms);
        QualifyingModule.Instance.ReceiveStartPole(resp);
    }

    public void SendPoleReward(PoleRewardReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PoleRewardReq>(0, data, MessageID.PoleRewardReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePoleReward(Packet data)
    {
        PoleRewardResp resp = Serializer.Deserialize<PoleRewardResp>(data.ms);
        QualifyingModule.Instance.ReceivePoleReward(resp);
    }
    public void SendPoleRecord(PoleRecordReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PoleRecordReq>(0, data, MessageID.PoleRecordReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePoleRecord(Packet data)
    {
        PoleRecordResp resp = Serializer.Deserialize<PoleRecordResp>(data.ms);
        QualifyingModule.Instance.ReceivePoleRecord(resp);
    }

    public void SendPoleRevenge(PoleRevengeReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PoleRevengeReq>(0, data, MessageID.PoleRevengeReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePoleRevenge(Packet data) 
    {
        PoleRevengeResp resp = Serializer.Deserialize<PoleRevengeResp>(data.ms);
        QualifyingModule.Instance.ReceivePoleRevenge(resp);
    }

    public void SendPoleBuyTimes(PoleBuyTimesReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PoleBuyTimesReq>(0, data, MessageID.PoleBuyTimesReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePoleBuyTimes(Packet data) 
    {
        PoleBuyTimesResp resp = Serializer.Deserialize<PoleBuyTimesResp>(data.ms);
        QualifyingModule.Instance.ReceivePoleBuyTimes(resp);
    }

    public void SendMatchPole(MatchPoleReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MatchPoleReq>(0, data, MessageID.MatchPoleReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveMatchPole(Packet data)
    {
        MatchPoleResp resp = Serializer.Deserialize<MatchPoleResp>(data.ms);
        QualifyingModule.Instance.ReceiveMatchPole(resp);
    }

    public void SendClearPoleMatchCD(ClearPoleMatchCDReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ClearPoleMatchCDReq>(0, data, MessageID.ClearPoleMatchCDReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveClearPoleMatchCD(Packet data)
    {
        ClearPoleMatchCDResp resp = Serializer.Deserialize<ClearPoleMatchCDResp>(data.ms);
        QualifyingModule.Instance.ReceiveClearPoleMatchCD(resp);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterPoleLobbyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SavePoleArrayRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartPoleRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PoleRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PoleRecordRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PoleRevengeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PoleBuyTimesRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MatchPoleRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ClearPoleMatchCDRespID);
    }
}
