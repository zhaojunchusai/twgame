using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class RankNetwork : Singleton<RankNetwork>
{
    public void SendRankInfoRequsest(RankInfoReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RankInfoReq>(0, request, MessageID.RankInfoReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("SendRankInfoRequsest ......  rank_type = " + request.rank_type);
    }
    
    public void OnRankInfoResponse(Packet data)
    {
        RankInfoResp response = Serializer.Deserialize<RankInfoResp>(data.ms);
        Debug.LogWarning("OnRankInfoResponse ...... result = " + response.result + " count = " + response.rank_info.Count);
        RankModule.Instance.OnRankInfoResponse(response);

    }

    public void SendRankPalyerInfoRequset(RankPlayerInfoReq request)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RankPlayerInfoReq>(0, request, MessageID.RankPalyerInfoReqID, PlayerData.Instance._SessionID);
        Debug.LogWarning("SendRankPalyerInfoRequset ......  rank_type = " + request.rank_type + " ; playerID = " + request.charid + " ; areaid = "+ request.area_id);
    }

    public void OnRankPlayerInfoResponse(Packet data)
    {
        RankPlayerInfoResp response = Serializer.Deserialize<RankPlayerInfoResp>(data.ms);
        RankModule.Instance.OnRankPlayerInfoResponse(response);
        Debug.LogWarning("OnRankPlayerInfoResponse ...... result = " + response.result);

    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.RankInfoRespID,OnRankInfoResponse);
        NetWorkManager.Instance.RegisterEvent(MessageID.RankPlayerInfoRespID,OnRankPlayerInfoResponse);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.RankInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.RankPlayerInfoRespID);
    }
}
