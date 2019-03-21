using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class FriendNetWork
{
    public void SendDonateFriendsHpReq(DonateFriendsHpReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DonateFriendsHpReq>(0, data, MessageID.DonateFriendsHpReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveDonateFriendsHpResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveDonateFriendsHpResp(data);
    }
    public void SendRecieveFriendHpReq(RecieveFriendHpReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RecieveFriendHpReq>(0, data, MessageID.RecieveFriendHpReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveRecieveFriendHpResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveRecieveFriendHpResp(data);
    }
    public void SendApplyAddFriendsReq(ApplyAddFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ApplyAddFriendsReq>(0, data, MessageID.ApplyAddFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveApplyAddFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveApplyAddFriendsResp(data);
    }
    public void SendGetFriendsListReq(GetFriendsListReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetFriendsListReq>(0, data, MessageID.GetFriendsListReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetFriendsListResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetFriendsListResp(data);
    }
    public void SendGetAppendFriendsListReq(GetAppendFriendsListReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetAppendFriendsListReq>(0, data, MessageID.GetAppendFriendsListReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetAppendFriendsListResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetAppendFriendsListResp(data);
    }
    public void SendAuditAppendFriendsReq(AuditAppendFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AuditAppendFriendsReq>(0, data, MessageID.AuditAppendFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAuditAppendFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveAuditAppendFriendsResp(data);
    }
    public void SendSearchFriendsReq(SearchFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SearchFriendsReq>(0, data, MessageID.SearchFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSearchFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveSearchFriendsResp(data);
    }
    public void SendDelFriendsReq(DelFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DelFriendsReq>(0, data, MessageID.DelFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveDelFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveDelFriendsResp(data);
    }
    public void SendRefreshRecommendFriendsReq(RefreshRecommendFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RefreshRecommendFriendsReq>(0, data, MessageID.RefreshRecommendFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveRefreshRecommendFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveRefreshRecommendFriendsResp(data);
    }
    public void SendVerifyInviteCodeReq(VerifyInviteCodeReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<VerifyInviteCodeReq>(0, data, MessageID.VerifyInviteCodeReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveVerifyInviteCodeResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveVerifyInviteCodeResp(data);
    }
    public void SendGetInviteCodeTaskListReq(GetInviteCodeTaskListReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetInviteCodeTaskListReq>(0, data, MessageID.GetInviteCodeTaskListReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetInviteCodeTaskListResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetInviteCodeTaskListResp(data);
    }
    public void SendGetInviteCodeTaskAwardReq(GetInviteCodeTaskAwardReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetInviteCodeTaskAwardReq>(0, data, MessageID.GetInviteCodeTaskAwardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetInviteCodeTaskAwardResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetInviteCodeTaskAwardResp(data);
    }
    public void SendGetTopPlayerInfoReq(GetTopPlayerInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetTopPlayerInfoReq>(0, data, MessageID.GetTopPlayerInfoReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetTopPlayerInfoResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetTopPlayerInfoResp(data);
    }
    public void SendGetThirdFriendsInfoReq(GetThirdFriendsInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetThirdFriendsInfoReq>(0, data, MessageID.GetThirdFriendsInfoReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetThirdFriendsInfoResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetThirdFriendsInfoResp(data);
    }
    public void SendInviteThirdFriendsReq(InviteThirdFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<InviteThirdFriendsReq>(0, data, MessageID.InviteThirdFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveInviteThirdFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveInviteThirdFriendsResp(data);
    }
    public void SendWakeThirdFriendsReq(WakeThirdFriendsReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<WakeThirdFriendsReq>(0, data, MessageID.WakeThirdFriendsReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveWakeThirdFriendsResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveWakeThirdFriendsResp(data);
    }
    public void SendGetThirdFriendsTaskReq(GetThirdFriendsTaskReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetThirdFriendsTaskReq>(0, data, MessageID.GetThirdFriendsTaskReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetThirdFriendsTaskResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetThirdFriendsTaskResp(data);
    }
    public void SendGetThirdFriendsTaskAwardReq(GetThirdFriendsTaskAwardReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetThirdFriendsTaskAwardReq>(0, data, MessageID.GetThirdFriendsTaskAwardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetThirdFriendsTaskAwardResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        FriendModule.Instance.ReceiveGetThirdFriendsTaskAwardResp(data);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.DonateFriendsHpRespID, ReceiveDonateFriendsHpResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.RecieveFriendHpRespID, ReceiveRecieveFriendHpResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.ApplyAddFriendsRespID, ReceiveApplyAddFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetFriendsListRespID, ReceiveGetFriendsListResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetAppendFriendsListRespID, ReceiveGetAppendFriendsListResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.AuditAppendFriendsRespID, ReceiveAuditAppendFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SearchFriendsRespID, ReceiveSearchFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.DelFriendsRespID, ReceiveDelFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.RefreshRecommendFriendsRespID, ReceiveRefreshRecommendFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.VerifyInviteCodeRespID, ReceiveVerifyInviteCodeResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetInviteCodeTaskListRespID, ReceiveGetInviteCodeTaskListResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetInviteCodeTaskAwardRespID, ReceiveGetInviteCodeTaskAwardResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetTopPlayerInfoRespID, ReceiveGetTopPlayerInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetThirdFriendsInfoRespID, ReceiveGetThirdFriendsInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.InviteThirdFriendsRespID, ReceiveInviteThirdFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.WakeThirdFriendsRespID, ReceiveWakeThirdFriendsResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetThirdFriendsTaskRespID, ReceiveGetThirdFriendsTaskResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetThirdFriendsTaskAwardRespID, ReceiveGetThirdFriendsTaskAwardResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.DonateFriendsHpRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.RecieveFriendHpRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ApplyAddFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetFriendsListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetAppendFriendsListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AuditAppendFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SearchFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DelFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.RefreshRecommendFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.VerifyInviteCodeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetInviteCodeTaskListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetInviteCodeTaskAwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetTopPlayerInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetThirdFriendsInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.InviteThirdFriendsReqID);
        NetWorkManager.Instance.RemoveEvent(MessageID.WakeThirdFriendsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetThirdFriendsTaskRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetThirdFriendsTaskAwardRespID);
    }
}
