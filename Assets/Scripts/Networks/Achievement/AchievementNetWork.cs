using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

public class AchievementNetWork {

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.EnterAchievementRespID, EnterAchievementResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetAchievementRespID, GetAchievementResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.EnterAchievementRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetAchievementRespID);
    }

    public void EnterAchievementReq(EnterAchievementReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnterAchievementReq>(0, data, MessageID.EnterAchievementReqID, PlayerData.Instance._SessionID);
    }
    public void EnterAchievementResp(Packet data)
    {
        EnterAchievementResp rData = Serializer.Deserialize<EnterAchievementResp>(data.ms);
        AchievementModule.Instance.ReceiveAchievementListResp(rData);
    }

    public void GetAchievementReq(GetAchievementReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetAchievementReq>(0, data, MessageID.GetAchievementReqID, PlayerData.Instance._SessionID);
    }
    public void GetAchievementResp(Packet data)
    {
        GetAchievementResp rData = Serializer.Deserialize<GetAchievementResp>(data.ms);
        AchievementModule.Instance.ReceiveGetAchievementAwardReq(rData);
    }
}
