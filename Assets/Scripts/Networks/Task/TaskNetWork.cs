using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;
public class TaskNetWork  
{
    public void SendGetTaskList(GetTaskListReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetTaskListReq>(0, data, MessageID.GetTaskListReqID, PlayerData.Instance._SessionID);
    }

    public void SendGetTaskAward(GetTaskAwardsReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetTaskAwardsReq>(0, data, MessageID.GetTaskAwardsReqID, PlayerData.Instance._SessionID);
    }


    public void ReceiveTaskList(Packet data) 
    {
        GetTaskListResp tdata = Serializer.Deserialize<GetTaskListResp>(data.ms);
        TaskModule.Instance.ReceiveTaskList(tdata);
    }

    public void ReceiveAward(Packet data) 
    {
        GetTaskAwardsResp tdata = Serializer.Deserialize<GetTaskAwardsResp>(data.ms);
        TaskModule.Instance.ReceiveAward(tdata);
    }

    public void NotifyRefreshTask(Packet data) 
    {

    }

    public void UpdateLivenessDataReq(bool needmask = true)
    {
        UpdateLivenessDataReq req = new UpdateLivenessDataReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpdateLivenessDataReq>(0, req, MessageID.UpdateLivenessDataReqID, PlayerData.Instance._SessionID, needmask);
    }
    public void UpdateLivenessDataResp(Packet data)
    {
        UpdateLivenessDataResp tData = Serializer.Deserialize<UpdateLivenessDataResp>(data.ms);
        TaskModule.Instance.UpdateLivenessDataResp(tData);
    }
    public void LivenessRewardReq(LivenessRewardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<LivenessRewardReq>(0, req, MessageID.LivenessRewardReqID, PlayerData.Instance._SessionID);
    }
    public void LivenessRewardResp(Packet data)
    {
        LivenessRewardResp tData = Serializer.Deserialize<LivenessRewardResp>(data.ms);
        TaskModule.Instance.LivenessRewardResp(tData);
    }

    public void LivenessNotify(Packet data)
    {
        //TaskModule.Instance.LivenessNotify();
    }

    public void ContinuAwardReq()
    {
        ContinuAwardReq req = new ContinuAwardReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ContinuAwardReq>(0, req, MessageID.ContinuAwardReqID, PlayerData.Instance._SessionID);

    }
    public void ContinuAwardResp(Packet data)
    {
        ContinuAwardResp tData = Serializer.Deserialize<ContinuAwardResp>(data.ms);
        TaskModule.Instance.ContinuAwardResp(tData);
    }

    public void CumulativeAwardReq(CumulativeAwardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CumulativeAwardReq>(0, req, MessageID.CumulativeAwardReqID, PlayerData.Instance._SessionID);
    }
    public void CumulativeAwardResp(Packet data)
    {
        CumulativeAwardResp tData = Serializer.Deserialize<CumulativeAwardResp>(data.ms);
        TaskModule.Instance.CumulativeAwardResp(tData);
    }

    public void SendNoRemindComment()
    {
        OnNeverRemindReq req = new OnNeverRemindReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OnNeverRemindReq>(0, req, MessageID.OnNeverRemindReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveNoRemindComment(Packet data)
    {
    }

    public void SendCommentTaskFinish()
    {
        OnFiveStarCommentFinishReq req = new OnFiveStarCommentFinishReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OnFiveStarCommentFinishReq>(0, req, MessageID.OnFiveStarCommentFinishReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCommentTaskFinish(Packet data)
    {

    }

    public void RegisterMsg() 
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetTaskListRespID, ReceiveTaskList);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetTaskAwardsRespID, ReceiveAward);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyRefreshTaskID, NotifyRefreshTask);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateLivenessDataRespID, UpdateLivenessDataResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.LivenessRewardRespID, LivenessRewardResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.LivenessNotifyID, LivenessNotify);
        NetWorkManager.Instance.RegisterEvent(MessageID.ContinuAwardRespID, ContinuAwardResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.CumulativeAwardRespID, CumulativeAwardResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OnNeverRemindRespID, ReceiveNoRemindComment);
        NetWorkManager.Instance.RegisterEvent(MessageID.OnFiveStarCommentFinishRespID, ReceiveCommentTaskFinish);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GetTaskListRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetTaskAwardsRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyRefreshTaskID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateLivenessDataRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.LivenessRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.LivenessNotifyID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ContinuAwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CumulativeAwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OnNeverRemindRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OnFiveStarCommentFinishRespID);
    }
}
