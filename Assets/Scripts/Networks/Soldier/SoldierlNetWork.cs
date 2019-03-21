using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class SoldierNetWork
{
    public void SendSoldierSelectReq(SoldierSelectReq data)//发送武将甄选
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierSelectReq>(0, data, MessageID.SoldierSelectReqID, PlayerData.Instance._SessionID);
    }
    public void SendSoldierUpLvReq(SoldierUpLvReq data)//发送武将甄选
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierUpLvReq>(0, data, MessageID.SoldierUpLvReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSoldierUpLvReq(Packet data)//接受武将升级请求回馈
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveSoldierUpLvReq(data);
    }
    public void ReceiveSoldierSelectResp(Packet data)//接受武将甄选请求回馈
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveSoldierSelectResp(data);
    }
    public void SendSoldierUpLevelStarReq(SoldierUpLevelStarReq data)//发送武将升级升星
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierUpLevelStarReq>(0, data, MessageID.SoldierUpLevelStarReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSoldierUpLevelStarResq(Packet data)//接受武将升级请求回馈
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveSoldierUpLevelStarResq(data);
    }

    public void ReceiveGetNewSoldier(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveGetNewSoldier(data);
    }
    public void SendSoldierUpQualityReq(SoldierUpQualityReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierUpQualityReq>(0, data, MessageID.SoldierUpQualityReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSoldierUpQualityResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveSoldierUpQualityResp(data);
    }
    public void SendSoldierUpStepReq(SoldierUpStepReq data)//
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<SoldierUpStepReq>(0, data, MessageID.SoldierUpStepReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveSoldierUpStepResp(Packet data)//接受武将升星请求回馈
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        SoldierModule.Instance.ReceiveSoldierUpStepResp(data);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierUpStepRespID, ReceiveSoldierUpStepResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierSelectRespID, ReceiveSoldierSelectResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierUpLvRespID, ReceiveSoldierUpLvReq);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyGetNewSoldierID, ReceiveGetNewSoldier);
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierUpLevelStarRespID, ReceiveSoldierUpLevelStarResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.SoldierUpQualityRespID, ReceiveSoldierUpQualityResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierUpStepRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierSelectRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierUpLvRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierUpLevelStarRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.SoldierUpQualityRespID);
    }

}
