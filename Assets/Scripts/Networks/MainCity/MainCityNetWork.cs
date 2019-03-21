using UnityEngine;
using System.Collections;
using fogs.proto.msg;
using ProtoBuf;

/* File：CreateRoleNetWork.cs
 * Desc: 主城网络通信消息
 * Date: 2015-06-27 13:44
 * Author: taiwei
 */

public class MainCityNetWork 
{

    public void SendFirstRechargeAward(FirstRechargeAwardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<FirstRechargeAwardReq>(0, req, MessageID.FirstRechargeAwardReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveFirstRechargeAward(Packet data)
    {
        FirstRechargeAwardResp tData = Serializer.Deserialize<FirstRechargeAwardResp>(data.ms);
        MainCityModule.Instance.ReceiveFirstRechargeAward(tData);
    }
    /// <summary>
    /// 请求体力回复  更新体力
    /// </summary>
    public void SendSpRevert(PHpowerRevertReq data)
    {
        Debug.Log("SendSpRevert");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<PHpowerRevertReq>(0, data, MessageID.PHpowerRevertReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveSpRevert(Packet data)
    {
        Debug.Log("ReceiveSpRevert");
        PHpowerRevertResp tData = Serializer.Deserialize<PHpowerRevertResp>(data.ms);
        MainCityModule.Instance.ReceiveSpRevert(tData);
    }

    public void SendExchangeGold(int exchageTimes)
    {
        Debug.Log("SendExchangeGold...... exchageTimes = " + exchageTimes);
        TouchGoldReq req = new TouchGoldReq();
        req.count = exchageTimes;
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<TouchGoldReq>(0, req, MessageID.TouchGoldReqID, PlayerData.Instance._SessionID);

    }
    public void ReceiveExchangeGold(Packet data)
    {
        Debug.Log("ReceiveExchangeGold");
        TouchGoldResp resp = Serializer.Deserialize<TouchGoldResp>(data.ms);
        MainCityModule.Instance.ReceiveExchangeGold(resp);
    }

    public void SendBuySP()
    {
        Debug.Log("SendBuySP");
        BuyPHPowerReq req = new BuyPHPowerReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyPHPowerReq>(0, req, MessageID.BuyPHPowerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveBuySP(Packet data)
    {
        Debug.Log("ReceiveBuySP");
        BuyPHPowerResp resp = Serializer.Deserialize<BuyPHPowerResp>(data.ms);
        MainCityModule.Instance.ReceiveBuySP(resp);
    }

    public void ReceiveNotify(Packet data)
    {
        NotifyRefresh resp = Serializer.Deserialize<NotifyRefresh>(data.ms);
        MainCityModule.Instance.ReceiveNotify(resp);
    }

    public void ReceiveNotifyMarquee(Packet data) 
    {
        NotifyMarquee resp = Serializer.Deserialize<NotifyMarquee>(data.ms);
        MainCityModule.Instance.ReceiveNotifyMarquee(resp);
    }

    public void SendFinishGuideStepReq(FinishGuideStepReq data)//发送新手引导步骤完成请求
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<FinishGuideStepReq>(0, data, MessageID.FinishGuideStepReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveFinishGuideStepResp(Packet data)//接受新手引导步骤完成请求
    {
        
    }
    public void SendStartGuideStepReq(StartGuideStepReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartGuideStepReq>(0, data, MessageID.StartGuideStepReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveStartGuideStepResp(Packet data)
    {

    }

    public void OnlineDayUpdateInfoReq()
    {
        Debug.Log("OnlineDayUpdateInfoReq");
        OnlineDayUpdateInfoReq data = new OnlineDayUpdateInfoReq();
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OnlineDayUpdateInfoReq>(0, data, MessageID.OnlineDayUpdateReqID, PlayerData.Instance._SessionID);
    }

    public void OnlineDayUpdateInfoResp(Packet data)
    {
        Debug.Log("OnlineDayUpdateInfoResp");
        OnlineDayUpdateInfoResp resp = Serializer.Deserialize<OnlineDayUpdateInfoResp>(data.ms);
        MainCityModule.Instance.OnlineDayUpdateInfoResp(resp);
    }

    public void OnSendOnlinePackageReward(OnlineRewardReq request)
    {
        Debug.LogWarning("OnSendOnlinePackageReward");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OnlineRewardReq>(0, request, MessageID.OnlineRewardReqID, PlayerData.Instance._SessionID);
    }

    public void OnReceiveOnlinePackageReward(Packet data)
    {
        OnlineRewardResp resp = Serializer.Deserialize<OnlineRewardResp>(data.ms);
        MainCityModule.Instance.OnReceiveOnlineReward(resp);
        Debug.LogWarning("OnReceiveOnlinePackageReward... result =  " + resp.result);
    }

    public void NotifyChatRefresh(Packet data)
    {
        Debug.Log("NotifyChatRefresh");
        NotifyChatRefresh tData = Serializer.Deserialize<NotifyChatRefresh>(data.ms);
        ChatModule.Instance.NotifyChatRefresh(tData);
    }

    public void NotifyResetData(Packet data) 
    {
        Debug.Log("NotifyResetData");
        NotifyReset tData = Serializer.Deserialize<NotifyReset>(data.ms);
        MainCityModule.Instance.NotifyResetData(tData);
    }

    public void MaxCombatPowerNotify(Packet data)
    {
        Debug.Log("MaxCombatPowerNotify");
        UpdateMaxViewCombatPower tData = Serializer.Deserialize<UpdateMaxViewCombatPower>(data.ms);
        MainCityModule.Instance.MaxCombatPowerNotify(tData);
    }

    public void SendBuildingEffectRecord(BuildUnlockAnimeIdReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuildUnlockAnimeIdReq>(0, req, MessageID.BuildUnlockAnimeIdReqID, PlayerData.Instance._SessionID,false);
    }

    public void ReceiveBuildingEffectRecord(Packet data)
    {

    }

    public void SendGetFirstLoginReward(FirstLoginAwardReq req)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<FirstLoginAwardReq>(0, req, MessageID.FirstLoginAwardReqID, PlayerData.Instance._SessionID, false);        
    }
    public void ReceiveGetFirstLoginReward(Packet data)
    {
        FirstLoginAwardResp tData = Serializer.Deserialize<FirstLoginAwardResp>(data.ms);
        MainCityModule.Instance.ReceiveGetFirstLoginReward(tData);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.PHpowerRevertRespID, ReceiveSpRevert);
        NetWorkManager.Instance.RegisterEvent(MessageID.TouchGoldRespID, ReceiveExchangeGold);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyPHPowerRespID, ReceiveBuySP);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyRefreshID, ReceiveNotify);
        NetWorkManager.Instance.RegisterEvent(MessageID.FinishGuideStepRespID, ReceiveFinishGuideStepResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.OnlineDayUpdateRespID, OnlineDayUpdateInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartGuideStepRespID, ReceiveStartGuideStepResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyMarqueeID, ReceiveNotifyMarquee);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyChatRefreshID, NotifyChatRefresh);
        NetWorkManager.Instance.RegisterEvent(MessageID.FirstRechargeAwardRespID, ReceiveFirstRechargeAward);
        NetWorkManager.Instance.RegisterEvent(MessageID.NotifyResetID, NotifyResetData);
        NetWorkManager.Instance.RegisterEvent(MessageID.OnlineRewardRespID, OnReceiveOnlinePackageReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpdateMaxViewCombatPowerID, MaxCombatPowerNotify);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuildUnlockAnimeIdRespID, ReceiveBuildingEffectRecord);
        NetWorkManager.Instance.RegisterEvent(MessageID.FirstLoginAwardRespID, ReceiveGetFirstLoginReward);
    }

    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.PHpowerRevertRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.TouchGoldRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyPHPowerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyRefreshID);
        NetWorkManager.Instance.RemoveEvent(MessageID.FinishGuideStepRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OnlineDayUpdateRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartGuideStepRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyMarqueeID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyChatRefreshID);
        NetWorkManager.Instance.RemoveEvent(MessageID.FirstRechargeAwardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.NotifyResetID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OnlineRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpdateMaxViewCombatPowerID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuildUnlockAnimeIdRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.FirstLoginAwardRespID);
    }


}