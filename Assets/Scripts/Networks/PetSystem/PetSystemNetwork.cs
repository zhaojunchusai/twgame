using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;
public class PetSystemNetwork
{
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.DressPetRespID, ReceiveDressPet);
        NetWorkManager.Instance.RegisterEvent(MessageID.ShowMountRespID, ReceiveShowMount);
        NetWorkManager.Instance.RegisterEvent(MessageID.PromotePetRespID, ReceivePromotePet);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpgradePetSKillRespID, ReceiveUpgradePetSKill);
    }

    public void SendDressPet(DressPetReq data)
    {
        Debug.Log("SendDressPet");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DressPetReq>(0, data, MessageID.DressPetReqID, PlayerData.Instance._SessionID);

    }

    private void ReceiveDressPet(Packet data)
    {
        Debug.Log("ReceiveDressPet");
        DressPetResp resp = Serializer.Deserialize<DressPetResp>(data.ms);
        PetSystemModule.Instance.ReceiveDressPet(resp);
    }


    public void SendShowMount(ShowMountReq data)
    {
        Debug.Log("SendShowMount");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ShowMountReq>(0, data, MessageID.ShowMountReqID, PlayerData.Instance._SessionID);
    }

    private void ReceiveShowMount(Packet data)
    {
        Debug.Log("ReceiveShowMount");
        ShowMountResp resp = Serializer.Deserialize<ShowMountResp>(data.ms);
        PetSystemModule.Instance.ReceiveShowMount(resp);
    }


    public void SendPromotePet(PromotePetLvReq data)
    {
        Debug.Log("SendPromotePet");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PromotePetLvReq>(0, data, MessageID.PromotePetReqID, PlayerData.Instance._SessionID);
    }

    private void ReceivePromotePet(Packet data)
    {
        Debug.Log("ReceivePromotePet");
        PromotePetLvResp resp = Serializer.Deserialize<PromotePetLvResp>(data.ms);
        PetSystemModule.Instance.ReceivePromotePet(resp);
    }

    public void SendUpgradePetSkill(UpgradePetSkillLvReq data)
    {
        Debug.Log("SendUpgradePetSkill");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpgradePetSkillLvReq>(0, data, MessageID.UpgradePetSKillReqID, PlayerData.Instance._SessionID);
    }

    private void ReceiveUpgradePetSKill(Packet data)
    {
        Debug.Log("ReceiveUpgradePetSKill");
        UpgradePetSkillLvResp resp = Serializer.Deserialize<UpgradePetSkillLvResp>(data.ms);
        PetSystemModule.Instance.ReceiveUpgradePetSKill(resp);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.DressPetRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ShowMountRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.PromotePetRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpgradePetSKillRespID);
    }
}
