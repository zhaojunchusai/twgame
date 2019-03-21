using ProtoBuf;
using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class UnionNetwork
{
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.OpenUnionRespID, OnReceiveOpenUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryUnionRespID, OnReceiveSearchUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.CreateUnionRespID, OnReceiveCreateUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.JoinUnionRespID, OnReceiveJoinUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.LeaveUnionRespID, OnReceiveLeaveUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.VisitUnionMemberRespID, OnReceiveVisitUnionMember);
        NetWorkManager.Instance.RegisterEvent(MessageID.LevelUpUnionRespID, OnReceiveLevelUpUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateUnionIconRespID, OnReceiveUpdateUnionIcon);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateUnionNameRespID, OnReceiveUpdateUnionName);
        NetWorkManager.Instance.RegisterEvent(MessageID.DonateToUnionRespID, OnReceiveDonateToUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateUnionSettingRespID, OnReceiveUpdateUnionSetting);
        NetWorkManager.Instance.RegisterEvent(MessageID.ManageUnionMemberRespID, OnReceiveManageUnionMember);
        NetWorkManager.Instance.RegisterEvent(MessageID.PlayerUnionRespID, OnReceivePlayerUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.ResetUnionPveDgnRespID, OnReceiveResetUnionPveDgn);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartUnionPveDgnRespID, OnReceiveStartUnionPvpDgn);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnionPveDgnTodayRwardRespID, OnReceiveUnionPveDgnTodayReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnionPveDgnRankRespID, OnReceiveUnionPveDgnRank);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryUnionPveDgnStateRespID, OnReceiveQueryUnionPveDgnState);
        NetWorkManager.Instance.RegisterEvent(MessageID.ReadyUnionPveDgnRespID, OnReceiveReadyUnionPveDgn);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyPlayerJoinUnionID, NotifyPlayerJoinUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnionPvpKillRankRespID, OnReceiveUnionPvpKillRank);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnionPvpRankRespID, OnReceiveUnionPvpRank);
        NetWorkManager.Instance.RegisterEvent(MessageID.ApplyForUnionPvpRespID, OnReceiveApplyUnionPVP);
        NetWorkManager.Instance.RegisterEvent(MessageID.OpenUnionPvpRespID, OnReceiveOpenUnionPVP);
        NetWorkManager.Instance.RegisterEvent(MessageID.JoinUnionPvpRespID, OnReceiveJionUnionPVP);
        NetWorkManager.Instance.RegisterEvent(MessageID.CancelUnionPvpRespID, OnReceiveCancelUnionPVP);
        NetWorkManager.Instance.RegisterEvent(MessageID.CancelReadyUnionPveRespID, OnReceiveCancelReadyUnionPve);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetUnionHpRespID, OnReceiveGetUnionHp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OpenUnionCityRespID, OnReceiveOpenUnionCity);
        NetWorkManager.Instance.RegisterEvent(MessageID.AuditSingleUnionMemberRespID, OnReceiveAuditSingleUnionMember);
        NetWorkManager.Instance.RegisterEvent(MessageID.AuditMultiUnionMemberRespID, OnReceiveAuditMultiUnionMember);
        NetWorkManager.Instance.RegisterEvent(MessageID.OpenUnionPveDgnRespID, OnReceiveOpenUnionPveDgn);
        NetWorkManager.Instance.RegisterEvent(MessageID.InvitePlayerJoinUnionRespID, OnReceiveInvitePlayerToUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyPlayerKickOutUnionID, OnReceiveNotifyPlayerKickOutToUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyPlayerInvitedToUnionID, OnReceiveNotifyPlayerInviteToUnion);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryUnionIconRespID, OnReceiveQueryUnionIcon);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.OpenUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CreateUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.JoinUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.LeaveUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.VisitUnionMemberRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.LevelUpUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateUnionIconRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateUnionNameRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DonateToUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateUnionSettingRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ManageUnionMemberRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetUnionHpRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PlayerUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ResetUnionPveDgnRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartUnionPveDgnRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UnionPveDgnTodayRwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UnionPveDgnRankRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryUnionPveDgnStateRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ReadyUnionPveDgnRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyPlayerJoinUnionID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ApplyForUnionPvpRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OpenUnionPvpReqID);
        NetWorkManager.Instance.RemoveEvent(MessageID.JoinUnionPvpReqID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CancelUnionPvpReqID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CancelReadyUnionPveRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AuditSingleUnionMemberRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AuditMultiUnionMemberRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OpenUnionPveDgnRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.InvitePlayerJoinUnionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyPlayerKickOutUnionID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyPlayerInvitedToUnionID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryUnionIconRespID);
    //public const uint QueryUnionIconReqID = 0x0762;
    //public const uint QueryUnionIconRespID = 0x0763;
    }

    public void NotifyPlayerJoinUnion(Packet data)
    {
        NotifyPlayerJoinUnion tData = Serializer.Deserialize<NotifyPlayerJoinUnion>(data.ms);
        UnionModule.Instance.NotifyPlayerJoinUnion(tData);
    }

    public void OnSendPlayerUnion(PlayerUnionReq req, bool showLoading)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PlayerUnionReq>(0, req, MessageID.PlayerUnionReqID, PlayerData.Instance._SessionID, showLoading);
    }

    public void OnReceivePlayerUnion(Packet data)
    {
        PlayerUnionResp tData = Serializer.Deserialize<PlayerUnionResp>(data.ms);
        UnionModule.Instance.OnReceivePlayerUnion(tData);
    }

    public void OnSendGetUnionHp(GetUnionHpReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetUnionHpReq>(0, req, MessageID.GetUnionHpReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveGetUnionHp(Packet data)
    {
        GetUnionHpResp tData = Serializer.Deserialize<GetUnionHpResp>(data.ms);
        UnionModule.Instance.OnReceiveGetUnionHp(tData);
    }

    public void OnSendManageUnionMember(ManageUnionMemberReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ManageUnionMemberReq>(0, req, MessageID.ManageUnionMemberReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveManageUnionMember(Packet data)
    {
        ManageUnionMemberResp tData = Serializer.Deserialize<ManageUnionMemberResp>(data.ms);
        UnionModule.Instance.OnReceiveManageUnionMember(tData);
    }

    public void OnSendUpdateUnionSetting(UpdateUnionSettingReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpdateUnionSettingReq>(0, req, MessageID.UpdateUnionSettingReqID, PlayerData.Instance._SessionID);

    }

    public void OnReceiveUpdateUnionSetting(Packet data)
    {
        UpdateUnionSettingResp tData = Serializer.Deserialize<UpdateUnionSettingResp>(data.ms);
        UnionModule.Instance.OnReceiveUpdateUnionSetting(tData);
    }

    public void OnSendDonateToUnion(DonateToUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DonateToUnionReq>(0, req, MessageID.DonateToUnionReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveDonateToUnion(Packet data)
    {
        DonateToUnionResp tData = Serializer.Deserialize<DonateToUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveDonateToUnion(tData);
    }

    public void OnSendUpdateUnionName(UpdateUnionNameReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpdateUnionNameReq>(0, req, MessageID.UpdateUnionNameReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveUpdateUnionName(Packet data)
    {
        UpdateUnionNameResp tData = Serializer.Deserialize<UpdateUnionNameResp>(data.ms);
        UnionModule.Instance.OnReceiveUpdateUnionName(tData);
    }

    public void OnSendUpdateUnionIcon(UpdateUnionIconReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpdateUnionIconReq>(0, req, MessageID.UpdateUnionIconReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveUpdateUnionIcon(Packet data)
    {
        UpdateUnionIconResp tData = Serializer.Deserialize<UpdateUnionIconResp>(data.ms);
        UnionModule.Instance.OnReceiveUpdateUnionIcon(tData);
    }

    public void OnSendLevelUpUnion(LevelUpUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<LevelUpUnionReq>(0, req, MessageID.LevelUpUnionReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveLevelUpUnion(Packet data)
    {
        LevelUpUnionResp tData = Serializer.Deserialize<LevelUpUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveLevelUpUnion(tData);
    }

    public void OnSendVisitUnionMember(VisitUnionMemberReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<VisitUnionMemberReq>(0, req, MessageID.VisitUnionMemberReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveVisitUnionMember(Packet data)
    {
        VisitUnionMemberResp tData = Serializer.Deserialize<VisitUnionMemberResp>(data.ms);
        UnionModule.Instance.OnReceiveVisitUnionMember(tData);
    }

    public void OnSendLeaveUnion(LeaveUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<LeaveUnionReq>(0, req, MessageID.LeaveUnionReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveLeaveUnion(Packet data)
    {
        LeaveUnionResp tData = Serializer.Deserialize<LeaveUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveLeaveUnion(tData);
    }

    public void OnSendJoinUnion(JoinUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<JoinUnionReq>(0, req, MessageID.JoinUnionReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveJoinUnion(Packet data)
    {
        JoinUnionResp tData = Serializer.Deserialize<JoinUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveJoinUnion(tData);
    }

    public void OnSendCreateUnion(CreateUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CreateUnionReq>(0, req, MessageID.CreateUnionReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveCreateUnion(Packet data)
    {
        CreateUnionResp tData = Serializer.Deserialize<CreateUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveCreateUnion(tData);
    }

    public void OnSendUnionPage(OpenUnionReq req, bool needLoading = true)
    {
        Debug.Log("OnSendUnionPage req.pag " + req.page);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OpenUnionReq>(0, req, MessageID.OpenUnionReqID, PlayerData.Instance._SessionID, needLoading);
    }

    public void OnReceiveOpenUnion(Packet data)
    {
        OpenUnionResp tData = Serializer.Deserialize<OpenUnionResp>(data.ms);
        Debug.Log("OnReceiveOpenUnion data.result " + tData.result);
        UnionModule.Instance.OnReceiveOpenUnion(tData);
    }

    public void OnSendUnionPage(QueryUnionReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryUnionReq>(0, req, MessageID.QueryUnionReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveSearchUnion(Packet data)
    {
        QueryUnionResp tData = Serializer.Deserialize<QueryUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveSearchUnion(tData);
    }

    public void OnSendOpenUnionPveDgn(OpenUnionPveDgnReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OpenUnionPveDgnReq>(0, data, MessageID.OpenUnionPveDgnReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveOpenUnionPveDgn(Packet data)
    {
        OpenUnionPveDgnResp tData = Serializer.Deserialize<OpenUnionPveDgnResp>(data.ms);
        UnionModule.Instance.OnReceiveOpenUnionPveDgn(tData);
    }

    public void OnSendResetUnionPveDgn(ResetUnionPveDgnReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ResetUnionPveDgnReq>(0, req, MessageID.ResetUnionPveDgnReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveResetUnionPveDgn(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        ResetUnionPveDgnResp tData = Serializer.Deserialize<ResetUnionPveDgnResp>(data.ms);
        UnionModule.Instance.OnReceiveResetUnionPveDgn(tData);
    }

    public void OnSendStartUnionPveDgn(StartUnionPveDgnReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartUnionPveDgnReq>(0, req, MessageID.StartUnionPveDgnReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveStartUnionPvpDgn(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        StartUnionPveDgnResp tData = Serializer.Deserialize<StartUnionPveDgnResp>(data.ms);
        UnionModule.Instance.OnReceiveStartUnionPvpDgn(tData);
    }

    public void OnSendUnionPveDgnTodayRward(UnionPveDgnTodayRewardReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnionPveDgnTodayRewardReq>(0, req, MessageID.UnionPveDgnTodayRwardReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveUnionPveDgnTodayReward(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        UnionPveDgnTodayRewardResp tData = Serializer.Deserialize<UnionPveDgnTodayRewardResp>(data.ms);
        UnionModule.Instance.OnReceiveUnionPveDgnTodayReward(tData);
    }
    public void OnSendUnionPveDgnRank(UnionPveDgnRankReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnionPveDgnRankReq>(0, req, MessageID.UnionPveDgnRankReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendUnionPveDgnRank ...... ");
    }

    public void OnReceiveUnionPveDgnRank(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        UnionPveDgnRankResp tData = Serializer.Deserialize<UnionPveDgnRankResp>(data.ms);
        Debug.LogWarning("OnReceiveUnionPveDgnRank ...... result = " + tData.result + " list.Count = " + tData.rank_list.Count);
        UnionModule.Instance.OnReceiveUnionPveDgnRank(tData);
    }

    public void OnSendQueryUnionPveDgnState(QueryUnionPveDgnStateReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryUnionPveDgnStateReq>(0, req, MessageID.QueryUnionPveDgnStateReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveQueryUnionPveDgnState(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        QueryUnionPveDgnStateResp tData = Serializer.Deserialize<QueryUnionPveDgnStateResp>(data.ms);
        UnionModule.Instance.OnReceiveQueryUnionPveDgnState(tData);
    }

    public void OnSendReadyUnionPveDgn(ReadyUnionPveDgnReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ReadyUnionPveDgnReq>(0, req, MessageID.ReadyUnionPveDgnReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveReadyUnionPveDgn(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        ReadyUnionPveDgnResp tData = Serializer.Deserialize<ReadyUnionPveDgnResp>(data.ms);
        UnionModule.Instance.OnReceiveReadyUnionPveDgn(tData);
    }

    public void OnSendUnionPvpRankReq(UnionPvpRankReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnionPvpRankReq>(0, req, MessageID.UnionPvpRankReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendUnionPvpRankReq ...... ");
    }

    public void OnReceiveUnionPvpRank(Packet data)
    {
        UnionPvpRankResp resp = Serializer.Deserialize<UnionPvpRankResp>(data.ms);
        Debug.LogWarning("OnReceiveUnionPvpRank ...... " + resp.result + " count " + resp.rank_list.Count);
        UnionModule.Instance.OnReceiveUnionPvpRank(resp);
    }

    public void OnSendUnionPvpKillRankReq(UnionPvpKillRankReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnionPvpKillRankReq>(0, req, MessageID.UnionPvpKillRankReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendUnionPvpKillRankReq ...... ");
    }

    public void OnReceiveUnionPvpKillRank(Packet data)
    {
        UnionPvpKillRankResp resp = Serializer.Deserialize<UnionPvpKillRankResp>(data.ms);
        Debug.LogWarning("OnReceiveUnionPvpKillRank ...... " + resp.result + " count " + resp.rank_list.Count);
        UnionModule.Instance.OnReceiveUnionPvpKillRank(resp);

    }


    public void OnSendApplyUnionPVP(ApplyForUnionPvpReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ApplyForUnionPvpReq>(0, req, MessageID.ApplyForUnionPvpReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveApplyUnionPVP(Packet data)
    {
        ApplyForUnionPvpResp resp = Serializer.Deserialize<ApplyForUnionPvpResp>(data.ms);
        UnionModule.Instance.OnReceiveApplyUnionPVP(resp);
    }

    public void OnSendOpenUnionPVP(OpenUnionPvpReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OpenUnionPvpReq>(0, req, MessageID.OpenUnionPvpReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendOpenUnionPVP ...... ");

    }

    public void OnReceiveOpenUnionPVP(Packet data)
    {
        OpenUnionPvpResp resp = Serializer.Deserialize<OpenUnionPvpResp>(data.ms);
        UnionModule.Instance.OnReceiveOpenUnionPVP(resp);
        Debug.LogWarning("OnReceiveOpenUnionPVP ...... city = " + resp.city);

    }

    public void OnSendJoinUnionPVP(JoinUnionPvpReq req)
    {
        Debug.LogWarning("OnSendJionUnionPVP ...... city = " + req.city);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<JoinUnionPvpReq>(0, req, MessageID.JoinUnionPvpReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveJionUnionPVP(Packet data)
    {
        JoinUnionPvpResp resp = Serializer.Deserialize<JoinUnionPvpResp>(data.ms);
        UnionModule.Instance.OnReceiveJoinUnionPVP(resp);
        Debug.LogWarning("OnReceiveJionUnionPVP ...... resp  = " + resp.result + " city " + resp.city);
    }

    public void OnSendCancelUnionPVP(CancelUnionPvpReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CancelUnionPvpReq>(0, req, MessageID.CancelUnionPvpReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendCancelUnionPVP ...... ");
    }

    public void OnReceiveCancelUnionPVP(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        CancelUnionPvpResp resp = Serializer.Deserialize<CancelUnionPvpResp>(data.ms);
        UnionModule.Instance.OnReceiveCancelUnionPVP(resp);
        Debug.LogWarning("OnReceiveCancelUnionPVP ...... " + resp.result + "; self_array.Count = " + resp.self_array.Count);
    }


    public void OnSendCancelReadyUnionPve(CancelReadyUnionPveReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CancelReadyUnionPveReq>(0, req, MessageID.CancelReadyUnionPveReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveCancelReadyUnionPve(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        CancelReadyUnionPveResp resp = Serializer.Deserialize<CancelReadyUnionPveResp>(data.ms);
        UnionModule.Instance.OnReceiveCancelReadyUnionPve(resp);
    }


    public void OnSendOpenUnionCity(OpenUnionCityReq req)
    {
        UISystem.Instance.HintView.ShowLoading(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OpenUnionCityReq>(0, req, MessageID.OpenUnionCityReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("OnSendOpenUnionCity ...... " + req.city);
    }

    public void OnReceiveOpenUnionCity(Packet data)
    {
        UISystem.Instance.HintView.ShowLoading(false);
        OpenUnionCityResp resp = Serializer.Deserialize<OpenUnionCityResp>(data.ms);
        UnionModule.Instance.OnReceiveOpenUnionCity(resp);
        Debug.LogWarning("OnReceiveOpenUnionCity ...... result = " + resp.result + "; city = " + resp.city + "; fight_result = " + resp.fight_result + "; selfArray.Count =  " + resp.self_array.Count + "; enemyArray.Count = " + resp.enemy_array.Count + "; record = " + resp.record.Count);
    }

    public void OnSendAuditSingleUnionMember(AuditSingleUnionMemberReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AuditSingleUnionMemberReq>(0, req, MessageID.AuditSingleUnionMemberReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveAuditSingleUnionMember(Packet data)
    {
        AuditSingleUnionMemberResp resp = Serializer.Deserialize<AuditSingleUnionMemberResp>(data.ms);
        UnionModule.Instance.OnReceiveAuditSingleUnionMember(resp);
    }
    public void OnSendAuditMultiUnionMember(AuditMultiUnionMemberReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AuditMultiUnionMemberReq>(0, req, MessageID.AuditMultiUnionMemberReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveAuditMultiUnionMember(Packet data)
    {
        AuditMultiUnionMemberResp resp = Serializer.Deserialize<AuditMultiUnionMemberResp>(data.ms);
        UnionModule.Instance.OnReceiveAuditMultiUnionMember(resp);
    }
    
    public void OnSendInvitePlayerToUnion(InvitePlayerJoinUnionReq req)
    {
        Debug.LogWarning("OnSendInvitePlayerToUnion charid = "+ req.invitees);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<InvitePlayerJoinUnionReq>(0, req, MessageID.InvitePlayerJoinUnionReqID, PlayerData.Instance._SessionID);
    }
   
    public void OnReceiveInvitePlayerToUnion(Packet data)
    {
        InvitePlayerJoinUnionResp resp = Serializer.Deserialize<InvitePlayerJoinUnionResp>(data.ms);
        UnionModule.Instance.OnReceiveInvitePlayerToUnion(resp);
        Debug.LogWarning("OnReceiveInvitePlayerToUnion result " + resp.result);
    }

    public void OnReceiveNotifyPlayerInviteToUnion(Packet data)
    {
        NotifyPlayerInvitedToUnion resp = Serializer.Deserialize<NotifyPlayerInvitedToUnion>(data.ms);
        UnionModule.Instance.NotifyBeInvitedToUnion(resp);
        Debug.LogWarning("OnReceiveNotifyPlayerInviteToUnion result " + resp.union_id + " unionname = "+ resp.union_name + " char name = "+ resp.charname);
    }

    public void OnReceiveNotifyPlayerKickOutToUnion(Packet data)
    {
        NotifyPlayerKickOutUnion resp = Serializer.Deserialize<NotifyPlayerKickOutUnion>(data.ms);
        UnionModule.Instance.NotifyPlayerKickOutUnion(resp);
        Debug.LogWarning("OnReceiveNotifyPlayerKickOutToUnion resp.union_name " + resp.union_name);
    }

    /// <summary>
    /// 军团图标查询
    /// </summary>
    /// <param name="req"></param>
    public void OnSendQueryUnionIcon(QueryUnionIconReq req)
    {
        Debug.LogWarning("OnSendQueryUnionIcon");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryUnionIconReq>(0, req, MessageID.QueryUnionIconReqID, PlayerData.Instance._SessionID);
    }
    public void OnReceiveQueryUnionIcon(Packet data)
    {
        QueryUnionIconResp resp = Serializer.Deserialize<QueryUnionIconResp>(data.ms);
        UnionModule.Instance.OnReceiveQueryUnionIcon(resp);
        Debug.LogWarning("OnReceiveQueryUnionIcon");
    }
}
