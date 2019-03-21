using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using System.Collections.Generic;
using ProtoBuf;
public class LifeSpiritNetwork
{
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.PutOnSoulRespID, ReceivePutOnSoul);
        NetWorkManager.Instance.RegisterEvent(MessageID.TakeOffSoulRespID, ReceiveTakeOffSoul);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpgradeSoulRespID, ReceiveUpgradeSoul);
        NetWorkManager.Instance.RegisterEvent(MessageID.SellSoulRespID, ReceiveSellSoul);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuySoulGridRespID, ReceiveBuySoulGrid);
        NetWorkManager.Instance.RegisterEvent(MessageID.ExploreSoulRespID, ReceiveExploreSoul);
        NetWorkManager.Instance.RegisterEvent(MessageID.CollectSoulRespID, ReceiveCollectSoul);
    }

    public void SendPutOnSoul(PutOnSoulReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PutOnSoulReq>(0, data, MessageID.PutOnSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceivePutOnSoul(Packet data)
    {
        PutOnSoulResp tmpData = Serializer.Deserialize<PutOnSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceivePutOnSoul(tmpData);
    }

    public void SendTakeOffSoul(TakeOffSoulReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<TakeOffSoulReq>(0, data, MessageID.TakeOffSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveTakeOffSoul(Packet data)
    {
        TakeOffSoulResp tmpData = Serializer.Deserialize<TakeOffSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveTakeOffSoul(tmpData);
    }

    public void SendUpgradeSoul(UpgradeSoulReq data) 
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpgradeSoulReq>(0, data, MessageID.UpgradeSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveUpgradeSoul(Packet data) 
    {
        UpgradeSoulResp tmpData = Serializer.Deserialize<UpgradeSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveUpgradeSoul(tmpData);
    }

    public void SendSellSoul(SellSoulReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SellSoulReq>(0, data, MessageID.SellSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveSellSoul(Packet data)
    {
        SellSoulResp tmpData = Serializer.Deserialize<SellSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveSellSoul(tmpData);
    }

    public void SendBuySoulGrid(BuySoulGridReq data)
    {
        Debug.Log("SendBuySoulGrid");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuySoulGridReq>(0, data, MessageID.BuySoulGridReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveBuySoulGrid(Packet data)
    {
        Debug.Log("ReceiveBuySoulGrid");
        BuySoulGridResp tmpData = Serializer.Deserialize<BuySoulGridResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveBuySoulGrid(tmpData);
    }

    public void SendExploreSoul(ExploreSoulReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ExploreSoulReq>(0, data, MessageID.ExploreSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveExploreSoul(Packet data)
    {
        ExploreSoulResp tmpData = Serializer.Deserialize<ExploreSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveExploreSoul(tmpData);
    }

    public void SendCollectSoul(CollectSoulReq data)
    {
        Debug.Log("SendCollectSoul");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CollectSoulReq>(0, data, MessageID.CollectSoulReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveCollectSoul(Packet data)
    {
        Debug.Log("ReceiveCollectSoul");
        CollectSoulResp tmpData = Serializer.Deserialize<CollectSoulResp>(data.ms);
        LifeSpiritModule.Instance.ReceiveCollectSoul(tmpData);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.PutOnSoulRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.TakeOffSoulRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpgradeSoulRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SellSoulRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuySoulGridRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ExploreSoulRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CollectSoulRespID);
    }
}
