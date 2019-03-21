using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class DrowEquipNetWork
{
    public void SendOneExtractEquipReq(OneExtractEquipReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneExtractEquipReq>(0, data, MessageID.OneExtractEquipReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveOneExtractEquipResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        DrowEquipModule.Instance.ReceiveOneExtractEquipResp(data);
    }
    public void SendMultipleExtractReq(MultipleExtractReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MultipleExtractReq>(0, data, MessageID.MultipleExtractReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveMultipleExtractResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        DrowEquipModule.Instance.ReceiveMultipleExtractResp(data);
    }


    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.OneExtractEquipRespID, ReceiveOneExtractEquipResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.MultipleExtractRespID, ReceiveMultipleExtractResp);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.OneExtractEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MultipleExtractRespID);
    }

}
