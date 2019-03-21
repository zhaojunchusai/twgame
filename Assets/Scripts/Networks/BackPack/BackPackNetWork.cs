using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

/* File：BackPackNetWork.cs
 * Desc: 背包界面与服务器的网络通信消息
 * Date: 2015-05-12 10:45
 * Author: taiwei
 */

public class BackPackNetWork
{
    public void SendUseItem(UseItemReq data)
    {
        Debug.Log("SendUseItem");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UseItemReq>(0, data, MessageID.UseItemReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveUseItem(Packet data)
    {
        Debug.Log("ReceiveSellItem");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        UseItemResp tData = Serializer.Deserialize<UseItemResp>(data.ms);
        BackPackModule.Instance.ReceiveUseItem(tData);
    }

    public void SendSellItem(SellItemReq data) 
    {
        Debug.Log("SendSellItem"); 
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SellItemReq>(0, data, MessageID.SellItemReqID, PlayerData.Instance._SessionID);
    }

    private void ReceiveSellItem(Packet data) 
    {
        Debug.Log("ReceiveSellItem");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SellItemResp tData = Serializer.Deserialize<SellItemResp>(data.ms);
        BackPackModule.Instance.ReceiveSellItem(tData);
    }
    public void SendChipCompositeReq(ChipCompositeReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ChipCompositeReq>(0, data, MessageID.ChipCompositeReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveChipCompositeResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ChipCompositeResp tData = Serializer.Deserialize<ChipCompositeResp>(data.ms);
        BackPackModule.Instance.ReceiveChipCompositeResp(tData);
    }

    public void SendBuyEquipBag(BuyEquipBagReq data)
    {
        Debug.Log("SendBuyEquipBag");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyEquipBagReq>(0, data, MessageID.BuyEquipBagReqID, PlayerData.Instance._SessionID);
   
    }

    public void ReceiveBuyEquipBag(Packet data)
    {
        Debug.Log("ReceiveBuyEquipBag");
        UISystem.Instance.HintView.SetLoadingVisible(false);
        BuyEquipBagResp tData = Serializer.Deserialize<BuyEquipBagResp>(data.ms);
        BackPackModule.Instance.ReceiveBuyEquipBag(tData);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.SellItemRespID, ReceiveSellItem);
        NetWorkManager.Instance.RegisterEvent(MessageID.UseItemRespID, ReceiveUseItem);
        NetWorkManager.Instance.RegisterEvent(MessageID.ChipCompositeRespID, ReceiveChipCompositeResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyEquipBagRespID, ReceiveBuyEquipBag);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SellItemRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UseItemRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ChipCompositeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyEquipBagRespID);


    }
}
