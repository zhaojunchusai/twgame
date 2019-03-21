using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class GameActivityNetwork : Singleton<GameActivityNetwork>
{

    public void OnSendActivityRewardRequest(uint id,uint condition)
    {
        ActivityRewardReq request = new ActivityRewardReq();
        request.activity_id = (int)id;
        request.condition = (int)condition;
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ActivityRewardReq>(0, request, MessageID.ActivityRewardReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveActivityRewardResponse(Packet data)
    {
        ActivityRewardResp response = Serializer.Deserialize<ActivityRewardResp>(data.ms);
        GameActivityModule.Instance.OnReceiveActivityRewardResponse(response);
    }

    public void OnSendBuyFundRequest()
    {
        BuyFundsReq request = new BuyFundsReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyFundsReq>(0, request, MessageID.BuyFundsReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveBuyFundResponse(Packet data)
    {
        BuyFundsResp response = Serializer.Deserialize<BuyFundsResp>(data.ms);
        GameActivityModule.Instance.OnReceiveBuyFundResponse(response);
    }

    public void OnSendQueryActivityRequest(uint id)
    {
        QueryActivityReq request = new QueryActivityReq();
        request.activity_id = (int)id;
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryActivityReq>(0, request, MessageID.QueryActivityReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveQueryActivityResponse(Packet data)
    {
        QueryActivityResp response = Serializer.Deserialize<QueryActivityResp>(data.ms);
        GameActivityModule.Instance.OnReceiveQueryActivityResponse(response);
    }

    public void OnSendQueryActivityTimeReq(QueryActivityTimeReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<QueryActivityTimeReq>(0, req, MessageID.QueryActivityTimeReqID, PlayerData.Instance._SessionID,false);
    }

    public void OnReceiveQueryActivityTimeResp(Packet data)
    {
        QueryActivityTimeResp response = Serializer.Deserialize<QueryActivityTimeResp>(data.ms);
        GameActivityModule.Instance.OnReceiveQueryActivityTimeResp(response);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.ActivityRewardRespID, OnReceiveActivityRewardResponse);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryActivityRespID,OnReceiveQueryActivityResponse);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyFundsRespID, OnReceiveBuyFundResponse);
        NetWorkManager.Instance.RegisterEvent(MessageID.QueryActivityTimeRespID, OnReceiveQueryActivityTimeResp);
    
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.ActivityRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryActivityRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyFundsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.QueryActivityTimeRespID);
    }
}
