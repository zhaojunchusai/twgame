using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class EquipNetWork
{
    public void SendPutonEquipReq(PutonEquipReq data)//发送穿装备请求
    {
        Debug.Log("PutonEquipReq");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PutonEquipReq>(0, data, MessageID.PutonEquipReqID, PlayerData.Instance._SessionID);
    }
    public void SendGetoffEquipReq(GetoffEquipReq data)//发送脱装备请求
    {
        Debug.Log("SendGetoffEquip");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetoffEquipReq>(0, data, MessageID.GetoffEquipReqID, PlayerData.Instance._SessionID);
    }
    public void SendPromoteEquipReq(PromoteEquipReq data)//发送装备强化请求
    {
        Debug.Log("SendPromoteEquip");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PromoteEquipReq>(0, data, MessageID.PromoteEquipReqID, PlayerData.Instance._SessionID);
    }
    public void SendOneKeyPromoteAllReq(OneKeyPromoteAllReq data)//发送一键强化请求
    {
        Debug.Log("SendOneKeyPromoteAll");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyPromoteAllReq>(0, data, MessageID.OneKeyPromoteAllReqID, PlayerData.Instance._SessionID);
    }
    public void SendOneKeyPutOnAllReq(OneKeyPutOnAllReq data)//发送一键换装请求
    {
        Debug.Log("SendOneKeyPutOnAll");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyPutOnAllReq>(0, data, MessageID.OneKeyPutOnAllReqID, PlayerData.Instance._SessionID);
    }
    public void SendSoldierEquipStarReq(SoldierEquipStarReq data)//发送武将装备升星请求
    {
        Debug.Log("SendSoldierEquipStar");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierEquipStarReq>(0, data, MessageID.SoldierEquipStarReqID, PlayerData.Instance._SessionID);
    }
    public void SendSellEquipReq(SellEquipReq data)//发送装备出售请求
    {
        Debug.Log("SellEquipReq");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SellEquipReq>(0, data, MessageID.SellEquipReqID, PlayerData.Instance._SessionID);
    }
    public void SendOneKeyPromoteOneReq(OneKeyPromoteOneReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyPromoteOneReq>(0, data, MessageID.OneKeyPromoteOneReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveOneKeyPromoteOneResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveOneKeyPromoteOneResp(data);
    }
    public void ReceivemSellEquipResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceivemSellEquipResp(data);
    }
    public void ReceiveSoldierEquipStarResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveSoldierEquipStarResp(data);
    }
    public void ReceivePutonEquipResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceivePutonEquipResp(data);
    }
    public void ReceiveGetoffEquipResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveGetoffEquipResp(data);
    }
    public void ReceivePromoteEquipResp(Packet data)
    {
        Debug.Log("ReceivePromoteEquipResp");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceivePromoteEquipResp(data);
    }
    public void ReceiveOneKeyPutOnAllResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveOneKeyPutOnAllResp(data);
    }
    public void ReceiveOneKeyPromoteAllResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveOneKeyPromoteAllResp(data);
    }
    public void SendEquipUpQualityReq(EquipUpQualityReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EquipUpQualityReq>(0, data, MessageID.EquipUpQualityReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEquipUpQualityResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveEquipUpQualityResp(data);
    }
    public void SendOneKeyOffEquipReq(OneKeyOffEquipReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyOffEquipReq>(0, data, MessageID.OneKeyOffEquipReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveOneKeyOffEquipResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EquipModule.Instance.ReceiveOneKeyOffEquipResp(data);
    }

    public void RegisterMsg() 
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.PutonEquipRespID, ReceivePutonEquipResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetoffEquipRespID, ReceiveGetoffEquipResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.PromoteEquipRespID, ReceivePromoteEquipResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyPutOnAllRespID, ReceiveOneKeyPutOnAllResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyPromoteAllRespID, ReceiveOneKeyPromoteAllResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierEquipStarRespID, ReceiveSoldierEquipStarResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SellEquipRespID,ReceivemSellEquipResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyPromoteOneRespID, ReceiveOneKeyPromoteOneResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EquipUpQualityRespID, ReceiveEquipUpQualityResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyOffEquipRespID, ReceiveOneKeyOffEquipResp);

    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.PutonEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetoffEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PromoteEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyPutOnAllRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyPromoteAllRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierEquipStarRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SellEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyPromoteOneRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EquipUpQualityRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyOffEquipRespID);

    }

}
