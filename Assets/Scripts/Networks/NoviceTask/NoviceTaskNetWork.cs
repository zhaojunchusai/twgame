using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;
public class NoviceTaskNetWork
{
    public void SendGetNewHandTasks(GetNewHandTasksReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetNewHandTasksReq>(0, req, MessageID.GetNewHandTasksReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetNewHandTasks(Packet data)
    {
        GetNewHandTasksResp tData = Serializer.Deserialize<GetNewHandTasksResp>(data.ms);
        NoviceTaskModule.Instance.ReceiveGetNewHandTasks(tData);
    }


    public void SendGetNewHandTasksAward(GetNewHandTasksAwardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetNewHandTasksAwardReq>(0, req, MessageID.GetNewHandTasksAwardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetNewHandTasksAward(Packet data)
    {
        GetNewHandTasksAwardResp tData = Serializer.Deserialize<GetNewHandTasksAwardResp>(data.ms);
        NoviceTaskModule.Instance.ReceiveGetNewHandTasksAward(tData);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetNewHandTasksRespID, ReceiveGetNewHandTasks);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetNewHandTasksAwardRespID, ReceiveGetNewHandTasksAward);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GetNewHandTasksRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetNewHandTasksAwardRespID);
    }
}