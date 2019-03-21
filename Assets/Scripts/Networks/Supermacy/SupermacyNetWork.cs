using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

/// <summary>
/// 全服霸主
/// OverlordStruc.proto
/// Overlord.proto
/// </summary>
public class SupermacyNetWork
{

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterOverlordRespID, ReceiveEnterOverlord);
        NetWorkManager.Instance.RegisterEvent(MessageID.buyCdRespID, ReceiveClearSupermacyCD);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyBuffRespID, ReceiveBuyBuff);
        NetWorkManager.Instance.RegisterEvent(MessageID.worshipRespID, ReceiveWorship);
        NetWorkManager.Instance.RegisterEvent(MessageID.StationReadyRespID, ReceiveStationReady);
        NetWorkManager.Instance.RegisterEvent(MessageID.ChallengeOverlordRespID, ReceiveChallenge);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterOverlordRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.buyCdRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyBuffRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.worshipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StationReadyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ChallengeOverlordRespID);
    }



    /// <summary>
    /// 进入全服霸主主界面主动向Server获取数据
    /// </summary>
    /// <param name="data"></param>
    public void SendEnterOverlord(EnterOverlordReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterOverlordReq>(0, data, MessageID.EnterOverlordReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnterOverlord(Packet data)
    {
        EnterOverlordResp tmpData = Serializer.Deserialize<EnterOverlordResp>(data.ms);
        SupermacyModule.Instance.ReceiveEnterOverlord(tmpData);
    }

    /// <summary>
    /// 重置挑战霸主CD
    /// </summary>
    public void SendClearSupermacyCD(BuyCdReq data)
    {
        Debug.Log("SendClearSupermacyCD");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyCdReq>(0, data, MessageID.buyCdReqID, PlayerData.Instance._SessionID);

    }
    /// <summary>
    /// 重置挑战霸主CD
    /// </summary>
    public void ReceiveClearSupermacyCD(Packet data)
    {
        Debug.Log("ReceiveClearSupermacyCD");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        BuyCdResp resp = Serializer.Deserialize<BuyCdResp>(data.ms);
        SupermacyModule.Instance.ReceiveClearSupermacyCD(resp);
    }

    /// <summary>
    /// 购买BUFF
    /// </summary>
    public void SendBuyBuff(BuyBuffReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyBuffReq>(0, data, MessageID.BuyBuffReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBuyBuff(Packet data)
    {
        BuyBuffResp tmpData = Serializer.Deserialize<BuyBuffResp>(data.ms);
        SupermacyModule.Instance.ReceiveBuyBuff(tmpData);
    }

    /// <summary>
    /// 领取朝拜礼包
    /// </summary>
    public void SendWorship(WorshipReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<WorshipReq>(0, data, MessageID.worshipReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveWorship(Packet data)
    {
        WorshipResp tmpData = Serializer.Deserialize<WorshipResp>(data.ms);
        SupermacyModule.Instance.ReceiveWorship(tmpData);
    }
    /// <summary>
    /// 进入战斗挑战
    /// </summary>
    public void SendStationReady(StationReadyReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StationReadyReq>(0, data, MessageID.StationReadyReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveStationReady(Packet data)
    {
        StationReadyResp resp = Serializer.Deserialize<StationReadyResp>(data.ms);
        SupermacyModule.Instance.ReceiveStationReady(resp);
    }

    /// <summary>
    /// 获取霸主信息
    /// </summary>
    /// <param name="data"></param>
    public void SendChallenge(ChallengeOverlordReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ChallengeOverlordReq>(0, data, MessageID.ChallengeOverlordReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveChallenge(Packet data)
    {
        ChallengeOverlordResp tmpdata = Serializer.Deserialize<ChallengeOverlordResp>(data.ms);
        SupermacyModule.Instance.ReceiveChallenge(tmpdata);
    }
}