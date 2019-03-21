using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class CastleNetWork
{
    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetCastleInfoRespID, ReceiveGetCastleInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpgradeCastleRespID, ReceiveUpgradeCastleResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.UpgradeShooterRespID, ReceiveUpgradeShooterResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnlockShooterRespID, ReceiveUnlockShooterResp);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.GetCastleInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpgradeCastleRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UpgradeShooterRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UnlockShooterRespID);
    }

    /// <summary>
    /// 获取城堡信息-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendPutonEquipReq(GetCastleInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetCastleInfoReq>(0, data, MessageID.GetCastleInfoReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 获取城堡信息-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveGetCastleInfoResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        if (data == null)
            return;
        GetCastleInfoResp tmpData = Serializer.Deserialize<GetCastleInfoResp>(data.ms);
        CastleModule.Instance.ReceiveGetCastleInfoResp(tmpData);
    }

    /// <summary>
    /// 升级城堡-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUpgradeCastleReq(UpgradeCastleReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpgradeCastleReq>(0, data, MessageID.UpgradeCastleReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 升级城堡-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUpgradeCastleResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        if (data == null)
            return;
        UpgradeCastleResp tmpData = Serializer.Deserialize<UpgradeCastleResp>(data.ms);
        CastleModule.Instance.ReceiveUpgradeCastleResp(tmpData);
    }

    /// <summary>
    /// 升级射手-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUpgradeShooterReq(UpgradeShooterReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UpgradeShooterReq>(0, data, MessageID.UpgradeShooterReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 升级射手-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUpgradeShooterResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        if (data == null)
            return;
        UpgradeShooterResp tmpData = Serializer.Deserialize<UpgradeShooterResp>(data.ms);
        CastleModule.Instance.ReceiveUpgradeShooterResp(tmpData);
    }

    /// <summary>
    /// 解锁射手-发送
    /// </summary>
    /// <param name="data"></param>
    public void SendUnlockShooterReq(UnlockShooterReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnlockShooterReq>(0, data, MessageID.UnlockShooterReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 解锁射手-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveUnlockShooterResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        if (data == null)
            return;
        UnlockShooterResp tmpData = Serializer.Deserialize<UnlockShooterResp>(data.ms);
        CastleModule.Instance.ReceiveUnlockShooterResp(tmpData);
    }
}
