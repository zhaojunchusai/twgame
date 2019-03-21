using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;
public class RecruitNetWork 
{
    public void SendOneRecruit(OneRecruitReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneRecruitReq>(0, data, MessageID.OneRecruitReqID, PlayerData.Instance._SessionID);
    }

    public void SendTenRecruit(MultipleRecruitReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MultipleRecruitReq>(0, data, MessageID.MultipleRecruitReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveOneRecruit(Packet data)
    {
        OneRecruitResp tdata = Serializer.Deserialize<OneRecruitResp>(data.ms);
        RecruitModule.Instance.ReceiveOneRecruit(tdata);
    }

    public void ReceiveTenRecruit(Packet data)
    {
        MultipleRecruitResp tdata = Serializer.Deserialize<MultipleRecruitResp>(data.ms);
        RecruitModule.Instance.ReceiveTenRecruit(tdata);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.OneRecruitRespID, ReceiveOneRecruit);
        NetWorkManager.Instance.RegisterEvent(MessageID.MultipleRecruitRespID, ReceiveTenRecruit);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.OneRecruitRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MultipleRecruitRespID);
    }
}
