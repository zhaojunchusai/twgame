using ProtoBuf;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class GateNetWork
{
    public void RegisterMsg() 
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.SpecialItemRespID, SendGuideGateGiftResp);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SpecialItemRespID);
    }

    public void SendGuideGateGiftReq(SpecialItemReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SpecialItemReq>(0, data, MessageID.SpecialItemReqID, PlayerData.Instance._SessionID);
    }

    public void SendGuideGateGiftResp(Packet data)
    {
        if (data == null)
            return;
        SpecialItemResp tmpData = Serializer.Deserialize<SpecialItemResp>(data.ms);
        GateModule.Instance.SendGuideGateGiftResp(tmpData);
    }
}
