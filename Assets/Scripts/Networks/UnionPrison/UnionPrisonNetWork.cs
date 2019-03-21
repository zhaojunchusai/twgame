using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class UnionPrisonNetWork : Singleton<UnionPrisonNetWork> 
{
    public void SendQueryAltarReq(QueryAltarReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryAltarReq>(0, data, MessageID.QueryAltarReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveQueryAltarResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveQueryAltarResp(data);
    }
    public void SendMatchAltarUnionReq(MatchAltarUnionReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MatchAltarUnionReq>(0, data, MessageID.MatchAltarUnionReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveMatchAltarUnionResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveMatchAltarUnionResp(data);
    }
    public void SendAltarHandleDependReq(AltarHandleDependReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AltarHandleDependReq>(0, data, MessageID.AltarHandleDependReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAltarHandleDependResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveAltarHandleDependResp(data);
    }
    public void SendAltarFightReq(AltarFightReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AltarFightReq>(0, data, MessageID.AltarFightReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAltarFightResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveAltarFightResp(data);
    }
    public void SendLockEnemyRebelUnionReq(LockEnemyRebelUnionReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<LockEnemyRebelUnionReq>(0, data, MessageID.LockEnemyRebelUnionReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveLockEnemyRebelUnionResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveLockEnemyRebelUnionResp(data);
    }
    public void SendQueryAltarRecordReq(QueryAltarRecordReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryAltarRecordReq>(0, data, MessageID.QueryAltarRecordReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveQueryAltarRecordResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveQueryAltarRecordResp(data);
    }
    public void SendDeleteDependUnionReq(DeleteDependUnionReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DeleteDependUnionReq>(0, data, MessageID.DeleteDependUnionReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveDeleteDependUnionResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveDeleteDependUnionResp(data);
    }

    public void SendAltarSerachUnionReq(AltarSerachUnionReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AltarSerachUnionReq>(0, data, MessageID.AltarSerachUnionReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAltarSerachUnionResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveAltarSerachUnionResp(data);
    }

    public void SendQueryAltarRecruitReq(QueryAltarRecruitReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryAltarRecruitReq>(0, data, MessageID.QueryAltarRecruitReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveQueryAltarRecruitResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPrisonModule.Instance.ReceiveQueryAltarRecruitResp(data);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryAltarRespID, ReceiveQueryAltarResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.MatchAltarUnionRespID, ReceiveMatchAltarUnionResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.AltarHandleDependRespID, ReceiveAltarHandleDependResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.AltarFightRespID, ReceiveAltarFightResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.LockEnemyRebelUnionRespID, ReceiveLockEnemyRebelUnionResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryAltarRecordRespID, ReceiveQueryAltarRecordResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.DeleteDependUnionRespID, ReceiveDeleteDependUnionResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.AltarSerachUnionRespID, ReceiveAltarSerachUnionResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryAltarRecruitRespID, ReceiveQueryAltarRecruitResp);

    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryAltarRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MatchAltarUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AltarHandleDependRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AltarFightRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.LockEnemyRebelUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryAltarRecordRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DeleteDependUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AltarSerachUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryAltarRecruitRespID);
    }
}
