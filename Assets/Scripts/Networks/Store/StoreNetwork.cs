using ProtoBuf;
using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class StoreNetwork
{
    public void SendOneKeyBuy( BuyAllCommodityReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyAllCommodityReq>(0, req, MessageID.BuyAllCommondityReqID, PlayerData.Instance._SessionID);        
    }
    public void ReceiveOneKeyBuy(Packet data)
    {
        BuyAllCommodityResp tData = Serializer.Deserialize<BuyAllCommodityResp>(data.ms);
        StoreModule.Instance.ReceiveOneKeyBuy(tData);
    }

    public void SendUpdateShopReq(UpdateShopReq req)
    {
        Debug.Log("SendUpdateShopReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpdateShopReq>(0, req, MessageID.UpdateShopReqID, PlayerData.Instance._SessionID);

    }
    public void ReceiveUpdateShopResp(Packet data)
    {
        Debug.Log("ReceiveUpdateShopResp");
        UpdateShopResp tData = Serializer.Deserialize<UpdateShopResp>(data.ms);
        StoreModule.Instance.ReceiveUpdateShopResp(tData);
    }

    public void SendBuyCommodityReq(BuyCommodityReq req)
    {
        Debug.Log("SendBuyCommodityReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyCommodityReq>(0, req, MessageID.BuyCommodityReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBuyCommodityResp(Packet data)
    {
        Debug.Log("ReceiveBuyCommodityResp");
        BuyCommodityResp tData = Serializer.Deserialize<BuyCommodityResp>(data.ms);
        StoreModule.Instance.ReceiveBuyCommodityResp(tData);
    }

    //public void SendRechargeReq(RechargeReq req)
    //{
    //    Debug.Log("SendRechargeReq " + req.id);
    //    NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RechargeReq>(0, req, MessageID.RechargeReqID, PlayerData.Instance._SessionID);
    //}
    //public void ReceiveRechargeResp(Packet data)
    //{
    //    Debug.Log("ReceiveRechargeResp ");
    //    RechargeResp tData = Serializer.Deserialize<RechargeResp>(data.ms);
    //    StoreModule.Instance.ReceiveRechargeResp(tData);
    //}

    public void SendGetCommodityOrderNum(GetCommodityOrderNumReq req)
    {
        Debug.Log("SendRechargeReq " + req.plat_type+" "+req.commodity_id);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetCommodityOrderNumReq>(0, req, MessageID.GetCommodityOrderNumReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetCommodityOrderNum(Packet data)
    {
        Debug.Log("ReceiveRechargeResp ");
        GetCommodityOrderNumResp tData = Serializer.Deserialize<GetCommodityOrderNumResp>(data.ms);
        StoreModule.Instance.ReceiveGetCommodityOrderNum(tData);
    }
    public void ReceiveNotifyRecharge(Packet data)
    {
        Debug.Log("ReceiveNotifyRecharge ");
        RechargeResp tData = Serializer.Deserialize<RechargeResp>(data.ms);
        StoreModule.Instance.ReceiveNotifyRecharge(tData);
    }

    public void SendBuySpecialCommodity(BuySpecialCommodityReq req)
    {
        Debug.Log("SendBuySpecialCommodity ");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuySpecialCommodityReq>(0, req, MessageID.BuySpecialCommodityReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBuySpecialCommodity(Packet data)
    {
        Debug.Log("ReceiveBuySpecialCommodity ");
        BuySpecialCommodityResp tData = Serializer.Deserialize<BuySpecialCommodityResp>(data.ms);
        StoreModule.Instance.ReceiveMallBuy(tData);
    }

    public void SendGetVipUpReward(GetVipUpRewardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetVipUpRewardReq>(0, req, MessageID.GetVipUpRewardReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveGetVipUpReward(Packet data)
    {
        GetVipUpRewardResp tData = Serializer.Deserialize<GetVipUpRewardResp>(data.ms);
        StoreModule.Instance.ReceiveGetVipUpReward(tData);
    }

    public void SendVipDailyReward(GetVipDailyRewardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetVipDailyRewardReq>(0, req, MessageID.GetVipDailyRewardReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveVipDailyReward(Packet data)
    {
        GetVipDailyRewardResp tData = Serializer.Deserialize<GetVipDailyRewardResp>(data.ms);
        StoreModule.Instance.ReceiveVipDailyReward(tData);
    }
    public void SendRecycleSell(RecycleReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<RecycleReq>(0, req, MessageID.RecycleReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveRecycleSell(Packet data)
    {
        RecycleResp resp = Serializer.Deserialize<RecycleResp>(data.ms);
        StoreModule.Instance.ReceiveRecycleSell(resp);

    }
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateShopRespID, ReceiveUpdateShopResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyCommodityRespID, ReceiveBuyCommodityResp);
        //NetWorkManager.Instance.RegisterEvent(MessageID.RechargeRespID, ReceiveRechargeResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetCommodityOrderNumRespID, ReceiveGetCommodityOrderNum);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyRechargeID, ReceiveNotifyRecharge);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyAllCommondityRespID, ReceiveOneKeyBuy);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuySpecialCommodityRespID, ReceiveBuySpecialCommodity);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetVipDailyRewardRespID, ReceiveVipDailyReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetVipUpRewardRespID, ReceiveGetVipUpReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.RecycleRespID, ReceiveRecycleSell);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateShopRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyCommodityRespID);
        //NetWorkManager.Instance.RemoveEvent(MessageID.RechargeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetCommodityOrderNumRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyRechargeID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyAllCommondityRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuySpecialCommodityRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetVipDailyRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetVipUpRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.RecycleRespID);
    }
}
