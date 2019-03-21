using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;
public class PrisonNetWork
{
    public void SendGetOtherPlayerInfoReq(GetOtherPlayerInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetOtherPlayerInfoReq>(0, data, MessageID.GetOtherPlayerInfoReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetOtherPlayerInfoResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveGetOtherPlayerInfoResp(data);
    }

    public void SendCollectMoneyReq(CollectMoneyReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CollectMoneyReq>(0, data, MessageID.CollectMoneyReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCollectMoneyResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveCollectMoneyResq(data);
    }

    public void SendReleaseEnslaveReq(ReleaseEnslaveReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ReleaseEnslaveReq>(0, data, MessageID.ReleaseEnslaveReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveReleaseEnslaveResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveReleaseEnslaveResq(data);
    }

    public void SendCollectAllMoneyReq(CollectAllMoneyReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CollectAllMoneyReq>(0, data, MessageID.CollectAllMoneyReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCollectAllMoneyResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveCollectAllMoneyResq(data);
    }
    public void SendGetEnslaveRecordReq(GetEnslaveRecordReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetEnslaveRecordReq>(0, data, MessageID.GetEnslaveRecordReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetEnslaveRecordResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveGetEnslaveRecordResq(data);
    }
    public void SendAskSaveEnslaveReq(AskSaveEnslaveReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AskSaveEnslaveReq>(0, data, MessageID.AskSaveEnslaveReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAskSaveEnslaveResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveAskSaveEnslaveResq(data);
    }
    public void SendMatchStrangerReq(MatchStrangerReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MatchStrangerReq>(0, data, MessageID.MatchStrangerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveMatchStrangerResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveMatchStrangerResq(data);
    }
    public void SendGetEnslaveInfoReq(GetEnslaveInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetEnslaveInfoReq>(0, data, MessageID.GetEnslaveInfoReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetEnslaveInfoResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveGetEnslaveInfoResp(data);
    }
    public void SendEnslaveFightBeforeReq(EnslaveFightBeforeReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnslaveFightBeforeReq>(0, data, MessageID.EnslaveFightBeforeReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnslaveFightBeforeResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveEnslaveFightBeforeResq(data);
    }
    public void SendGetEnemyPlayerReq(GetEnemyPlayerReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetEnemyPlayerReq>(0, data, MessageID.GetEnemyPlayerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetEnemyPlayerResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveGetEnemyPlayerResq(data);
    }
    public void SendGetRunAwayPlayerReq(GetRunAwayPlayerReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<GetRunAwayPlayerReq>(0, data, MessageID.GetRunAwayPlayerReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveGetRunAwayPlayerResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveGetRunAwayPlayerResq(data);
    }
    public void SendEnslaveLockedReq(EnslaveLockedReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnslaveLockedReq>(0, data, MessageID.EnslaveLockedReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnslaveLockedResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveEnslaveLockedResq(data);
    }
    public void SendEnslaveUnLockedReq(EnslaveUnLockedReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnslaveUnLockedReq>(0, data, MessageID.EnslaveUnLockedReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnslaveUnLockedResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveEnslaveUnLockedResq(data);
    }
    public void SendAskHelpReq(AskHelpReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<AskHelpReq>(0, data, MessageID.AskHelpReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveAskHelpResp(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveAskHelpResp(data);
    }
    public void SendWorldSaveCheckReq(WorldSaveCheckReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<WorldSaveCheckReq>(0, data, MessageID.WorldSaveCheckReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveWorldSaveCheckResq(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        PrisonModule.Instance.ReceiveWorldSaveCheckResq(data);
    }

    public void RegisterMsg()
    {
        NetWorkManager.Instance.RegisterEvent(MessageID.GetOtherPlayerInfoRespID, ReceiveGetOtherPlayerInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.CollectMoneyRespID, ReceiveCollectMoneyResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.ReleaseEnslaveRespID, ReceiveReleaseEnslaveResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.CollectAllMoneyRespID, ReceiveCollectAllMoneyResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetEnslaveRecordRespID, ReceiveGetEnslaveRecordResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.AskSaveEnslaveRespID, ReceiveAskSaveEnslaveResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.MatchStrangerRespID, ReceiveMatchStrangerResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetEnslaveInfoRespID, ReceiveGetEnslaveInfoResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnslaveFightBeforeRespID, ReceiveEnslaveFightBeforeResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetEnemyPlayerRespID, ReceiveGetEnemyPlayerResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.GetRunAwayPlayerRespID, ReceiveGetRunAwayPlayerResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnslaveLockedRespID, ReceiveEnslaveLockedResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnslaveUnLockedRespID, ReceiveEnslaveUnLockedResq);
        NetWorkManager.Instance.RegisterEvent(MessageID.AskHelpRespID, ReceiveAskHelpResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.WorldSaveCheckRespID, ReceiveWorldSaveCheckResq);
    }
    public void RemoveMsg()
    {
        NetWorkManager.Instance.RemoveEvent(MessageID.SkillUpgradeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CollectMoneyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ReleaseEnslaveRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.CollectAllMoneyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetEnslaveRecordRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AskSaveEnslaveRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MatchStrangerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetEnslaveInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveFightBeforeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartEnslaveFightRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveFightOverRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetEnemyPlayerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.GetRunAwayPlayerRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveLockedRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveUnLockedRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.AskHelpRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.WorldSaveCheckRespID);
    }
}
