using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class CaptureTerritoryNetwork
{

    public void SendGetCampaignTokenInfo(GetCampaignTokenInfoReq req,bool showLoading) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetCampaignTokenInfoReq>(0, req, MessageID.GetCampaignTokenInfoReqID, PlayerData.Instance._SessionID, showLoading);
    }
    public void ReceiveGetCampaignTokenInfo(Packet data)
    {
        GetCampaignTokenInfoResp tData = Serializer.Deserialize<GetCampaignTokenInfoResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveGetCampaignTokenInfo(tData);
    }

    public void SendGetCampaignRank(GetCampaignRankReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetCampaignRankReq>(0, req, MessageID.GetCampaignRankReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetCampaignRank(Packet data)
    {
        GetCampaignRankResp tData = Serializer.Deserialize<GetCampaignRankResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveGetCampaignRank(tData);
    }

    public void SendActivateToken(ActivateTokenReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ActivateTokenReq>(0, req, MessageID.ActivateTokenReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveActivateToken(Packet data)
    {
        ActivateTokenResp tData = Serializer.Deserialize<ActivateTokenResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveActivateToken(tData);
    }

    public void SendStartCampaignPvp(StartCampaignPvpReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartCampaignPvpReq>(0, req, MessageID.StartCampaignPvpReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveStartCampaignPvp(Packet data)
    {
        StartCampaignPvpResp tData = Serializer.Deserialize<StartCampaignPvpResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveStartCampaignPvp(tData);
    }

    public void SendCampaignReward(CampaignRewardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CampaignRewardReq>(0, req, MessageID.CampaignRewardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCampaignReward(Packet data)
    {
        CampaignRewardResp tData = Serializer.Deserialize<CampaignRewardResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveCampaignReward(tData);
    }

    public void SendAssignChest(AssignChestReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AssignChestReq>(0, req, MessageID.AssignChestReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAssignChest(Packet data)
    {
        AssignChestResp tData = Serializer.Deserialize<AssignChestResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveAssignChest(tData);
    }

    public void SendClearCampaignCD(ClearCampaignCDReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ClearCampaignCDReq>(0, req, MessageID.ClearCampaignCDReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveClearCampaignCD(Packet data)
    {
        ClearCampaignCDResp tData = Serializer.Deserialize<ClearCampaignCDResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveClearCampaignCD(tData);
    }
    public void SendGetScoreInfo(GetScoreInfoReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetScoreInfoReq>(0, req, MessageID.GetScoreInfoReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetScoreInfo(Packet data)
    {
        GetScoreInfoResp tData = Serializer.Deserialize<GetScoreInfoResp>(data.ms);
        CaptureTerritoryModule.Instance.ReceiveGetScoreInfo(tData);
    }
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetCampaignTokenInfoRespID, ReceiveGetCampaignTokenInfo);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetCampaignRankRespID,      ReceiveGetCampaignRank);
        NetWorkManager.Instance.RegisterEvent(MessageID.ActivateTokenRespID,        ReceiveActivateToken);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartCampaignPvpRespID,     ReceiveStartCampaignPvp);
        //NetWorkManager.Instance.RegisterEvent(MessageID.CampaignRewardRespID,       ReceiveCampaignReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.AssignChestRespID,          ReceiveAssignChest);
        NetWorkManager.Instance.RegisterEvent(MessageID.ClearCampaignCDRespID, ReceiveClearCampaignCD);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetScoreInfoRespID, ReceiveGetScoreInfo);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GetCampaignTokenInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetCampaignRankRespID     );
        NetWorkManager.Instance.RemoveEvent(MessageID.ActivateTokenRespID       );
        NetWorkManager.Instance.RemoveEvent(MessageID.StartCampaignPvpRespID    );
        //NetWorkManager.Instance.RemoveEvent(MessageID.CampaignRewardRespID      );
        NetWorkManager.Instance.RemoveEvent(MessageID.AssignChestRespID         );
        NetWorkManager.Instance.RemoveEvent(MessageID.ClearCampaignCDRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetScoreInfoRespID);
    }
}
