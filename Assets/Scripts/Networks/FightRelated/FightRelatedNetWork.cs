using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class FightRelatedNetWork
{

    public void RegisterMsg()
    {
        //NetWorkManager.Instance.RegisterEvent(MessageID.MajorMapRestartRespID, ReceiveMajorMapRestart);
        //NetWorkManager.Instance.RegisterEvent(MessageID.ChapterOpenTimeRespID, ReceiveChapterOpen);
        NetWorkManager.Instance.RegisterEvent(MessageID.StartExpeditionRespID, ReceiveStartExpedition);
        NetWorkManager.Instance.RegisterEvent(MessageID.ExpeditionResultRespID, ReceiveExpeditionResult);
        NetWorkManager.Instance.RegisterEvent(MessageID.MatchEnemyRespID, ReceiveMatchEnemy);
        NetWorkManager.Instance.RegisterEvent(MessageID.ExpeditionRewardRespID, ReceiveExpeditionReward);

        NetWorkManager.Instance.RegisterEvent(MessageID.DungeonInfoRespID, ReceiveDungeonInfo);
        NetWorkManager.Instance.RegisterEvent(MessageID.DungeonRewardRespID, ReceiveDungeonReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.DungeonStarRewardRespID, ReceiveDungeonStarReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.DungeonStartRespID, ReceiveDungeonStart);
        NetWorkManager.Instance.RegisterEvent(MessageID.EndlessDungeonRewardRespID, ReceiveEndlessDungeonReward);
        NetWorkManager.Instance.RegisterEvent(MessageID.MopupDungeonRespID, ReceiveMopupDungeon);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyDungeonTimesRespID, ReceiveBuyDungeonTimes);
        NetWorkManager.Instance.RegisterEvent(MessageID.BuyOtherDungeonTimesRespID, ReceiveBuyOtherDungeonTimes);
        NetWorkManager.Instance.RegisterEvent(MessageID.OneKeyReplaceEquipRespID, ReceiveOneKeyReplaceEquip);
        NetWorkManager.Instance.RegisterEvent(MessageID.UnionPveDgnRewardRespID, UnionPveDgnRewardResp);

        NetWorkManager.Instance.RegisterEvent(MessageID.StartEnslaveFightRespID, ReceiveStartEnslaveFightResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnslaveFightOverRespID, ReceiveEnslaveFightOverResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.EnslaveFightBeforeBackRespID, ReceiveEnslaveFightBeforeBack);

        NetWorkManager.Instance.RegisterEvent(MessageID.CampaignRewardRespID, ReceiveCaptureTerritoryResp);
        NetWorkManager.Instance.RegisterEvent(MessageID.BillingRespID, ReceiveServerHegemonyResp);

        NetWorkManager.Instance.RegisterEvent(MessageID.EndPoleRespID, ReceiveEndPole);

        NetWorkManager.Instance.RegisterEvent(MessageID.CombatSettlementRespID, ReceiveCombatSettlementResp);
    }

    public void RemoveMsg()
    {
        //NetWorkManager.Instance.RemoveEvent(MessageID.MajorMapRestartRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ChapterOpenTimeRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.StartExpeditionRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ExpeditionResultRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MatchEnemyRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.ExpeditionRewardRespID);

        NetWorkManager.Instance.RemoveEvent(MessageID.DungeonInfoRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DungeonRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DungeonStarRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.DungeonStartRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EndlessDungeonRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.MopupDungeonRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyOtherDungeonTimesRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BuyDungeonTimesRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.OneKeyReplaceEquipRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.UnionPveDgnRewardRespID);

        NetWorkManager.Instance.RemoveEvent(MessageID.StartEnslaveFightRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveFightOverRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveFightOverRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.EnslaveFightBeforeBackRespID);

        NetWorkManager.Instance.RemoveEvent(MessageID.CampaignRewardRespID);
        NetWorkManager.Instance.RemoveEvent(MessageID.BillingRespID);

        NetWorkManager.Instance.RemoveEvent(MessageID.EndPoleRespID);

        NetWorkManager.Instance.RemoveEvent(MessageID.CombatSettlementRespID);

    }

    /// <summary>
    /// 匹配敌方数据(不需要消耗任何东西) -发送
    /// </summary>
    /// <param name="data"></param>
    public void SendMatchEnemy(MatchEnemyReq data)
    {
        Debug.Log("SendMatchEnemy");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MatchEnemyReq>(0, data, MessageID.MatchEnemyReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 匹配敌方数据(不需要消耗任何东西)-接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveMatchEnemy(Packet data)
    {
        Debug.Log("ReceiveMatchEnemy");
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        MatchEnemyResp tData = Serializer.Deserialize<MatchEnemyResp>(data.ms);
        FightRelatedModule.Instance.ReceiveMatchEnemy(tData);
    }

    /// <summary>
    ///  远征奖励 - 发送 
    /// </summary>
    /// <param name="data"></param>
    public void SendExpeditionReward(ExpeditionRewardReq data)
    {
        Debug.Log("SendExpeditionReward");
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ExpeditionRewardReq>(0, data, MessageID.ExpeditionRewardReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    ///  远征奖励 -接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveExpeditionReward(Packet data)
    {
        Debug.Log("ReceiveExpeditionReward");
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        ExpeditionRewardResp tData = Serializer.Deserialize<ExpeditionRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveExpeditionReward(tData);
    }

    ///// <summary>
    ///// 章节开放—发送
    ///// </summary>
    //public void SendChapterOpen(ChapterOpenTimeReq data)
    //{
    //    //Debug.Log("FightRelatedNetWork: SendChapterOpen");
    //    UISystem.Instance.HintView.SetLoadingVisible(true);
    //    NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ChapterOpenTimeReq>(0, data, MessageID.ChapterOpenTimeReqID, PlayerData.Instance._SessionID);

    //}
    ///// <summary>
    ///// 章节开放 - 接收
    ///// </summary>
    //public void ReceiveChapterOpen(Packet data)
    //{
    //    //Debug.Log("FightRelatedNetWork: ReceiveChapterOpen");
    //    UISystem.Instance.HintView.SetLoadingVisible(false);
    //    ChapterOpenTimeResp tData = Serializer.Deserialize<ChapterOpenTimeResp>(data.ms);
    //    FightRelatedModule.Instance.ReceiveChapterOpen(tData);
    //}

    ///// <summary>
    ///// 重新开始战斗-发送
    ///// </summary>
    ///// <param name="data"></param>
    //public void SendMajorMapRestart(MajorMapRestartReq data)
    //{
    //    //Debug.LogWarning("FightRelatedNetWork: SendMajorMapRestart");
    //    UISystem.Instance.HintView.SetLoadingVisible(true);
    //    NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MajorMapRestartReq>(0, data, MessageID.MajorMapRestartReqID, PlayerData.Instance._SessionID);
    //}
    ///// <summary>
    ///// 重新开始战斗-接收
    ///// </summary>
    ///// <param name="data"></param>
    //public void ReceiveMajorMapRestart(Packet data)
    //{
    //    //Debug.LogWarning("FightRelatedNetWork: ReceiveMajorMapRestart");
    //    UISystem.Instance.HintView.SetLoadingVisible(false);
    //    MajorMapRestartResp tmpData = Serializer.Deserialize<MajorMapRestartResp>(data.ms);
    //    FightRelatedModule.Instance.ReceiveMajorMapRestart(tmpData);
    //}

    /// <summary>
    /// 远征开始战斗
    /// </summary>
    /// <param name="data"></param>
    public void SendStartExpedition(StartExpeditionReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartExpeditionReq>(0, data, MessageID.StartExpeditionReqID, PlayerData.Instance._SessionID);
    }

    /// <summary>
    /// 远征开始战斗
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveStartExpedition(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        StartExpeditionResp tmpData = Serializer.Deserialize<StartExpeditionResp>(data.ms);
        FightRelatedModule.Instance.ReceiveStartExpedition(tmpData);
    }

    /// <summary>
    /// 远征结算
    /// </summary>
    /// <param name="data"></param>
    public void SendExpeditionResult(ExpeditionResultReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<ExpeditionResultReq>(0, data, MessageID.ExpeditionResultReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 远征结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveExpeditionResult(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        ExpeditionResultResp tmpData = Serializer.Deserialize<ExpeditionResultResp>(data.ms);
        FightRelatedModule.Instance.ReceiveExpeditionResult(tmpData);
    }

    /// <summary>
    /// 攻城略地结算
    /// </summary>
    /// <param name="data"></param>
    public void SendCaptureTerritoryReq(CampaignRewardReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CampaignRewardReq>(0, data, MessageID.CampaignRewardReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCaptureTerritoryResp(Packet data)
    {
        CampaignRewardResp tmpData = Serializer.Deserialize<CampaignRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveCaptureTerritoryResp(tmpData);
    }

    /// <summary>
    /// 全服爭霸
    /// </summary>
    /// <param name="data"></param>
    public void SendServerHegemonyReq(BillingReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BillingReq>(0, data, MessageID.BillingReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveServerHegemonyResp(Packet data)
    {
        BillingResp tmpData = Serializer.Deserialize<BillingResp>(data.ms);
        FightRelatedModule.Instance.ReceiveServerHegemonyResp(tmpData);
    }
    /// <summary>
    /// 跨服战
    /// </summary>
    /// <param name="req"></param>
    public void SendCombatSettlementReq(CombatSettlementReq req)
    {
        Debug.Log("SendCombatSettlementReq");
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<CombatSettlementReq>(0, req, MessageID.CombatSettlementReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveCombatSettlementResp(Packet resp)
    {
        Debug.Log("ReceiveCombatSettlementResp");
        CombatSettlementResp data = Serializer.Deserialize<CombatSettlementResp>(resp.ms);
        FightRelatedModule.Instance.OnReceiveCombatSettlement(data);
    }
    //================New =======================//


    /// <summary>
    /// 进入PVE
    /// </summary>
    public void SendDungeonInfo(DungeonInfoReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DungeonInfoReq>(0, data, MessageID.DungeonInfoReqID, PlayerData.Instance._SessionID);

    }
    /// <summary>
    /// 进入PVE
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonInfo(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        DungeonInfoResp tmpData = Serializer.Deserialize<DungeonInfoResp>(data.ms);
        FightRelatedModule.Instance.ReceiveDungeonInfo(tmpData);
    }


    /// <summary>
    /// 开始PVE
    /// </summary>
    public void SendDungeonStart(DungeonStartReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DungeonStartReq>(0, data, MessageID.DungeonStartReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 开始PVE
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonStart(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        DungeonStartResp tmpData = Serializer.Deserialize<DungeonStartResp>(data.ms);
        FightRelatedModule.Instance.ReceiveDungeonStart(tmpData);
    }

    /// <summary>
    /// 扫荡
    /// </summary>
    public void SendMopupDungeon(MopupDungeonReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<MopupDungeonReq>(0, data, MessageID.MopupDungeonReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 扫荡
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveMopupDungeon(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        MopupDungeonResp tmpData = Serializer.Deserialize<MopupDungeonResp>(data.ms);
        FightRelatedModule.Instance.ReceiveMopupDungeon(tmpData);
    }

    /// <summary>
    /// PVE 结算
    /// </summary>
    public void SendDungeonReward(DungeonRewardReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DungeonRewardReq>(0, data, MessageID.DungeonRewardReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// PVE 结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonReward(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        DungeonRewardResp tmpData = Serializer.Deserialize<DungeonRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveDungeonReward(tmpData);
    }

    /// <summary>
    /// 无尽模式结算
    /// </summary>
    public void SendEndlessDungeonReward(EndlessDungeonRewardReq data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EndlessDungeonRewardReq>(0, data, MessageID.EndlessDungeonRewardReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 无尽模式结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEndlessDungeonReward(Packet data)
    {
        UISystem.Instance.HintView.SetLoadingVisible(false);
        EndlessDungeonRewardResp tmpData = Serializer.Deserialize<EndlessDungeonRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveEndlessDungeonReward(tmpData);
    }
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    public void SendDungeonStarReward(DungeonStarRewardReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<DungeonStarRewardReq>(0, data, MessageID.DungeonStarRewardReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonStarReward(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        DungeonStarRewardResp tmpData = Serializer.Deserialize<DungeonStarRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveDungeonStarReward(tmpData);
    }
    /// <summary>
    /// 购买次数
    /// </summary>
    public void SendBuyDungeonTimes(BuyDungeonTimesReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyDungeonTimesReq>(0, data, MessageID.BuyDungeonTimesReqID, PlayerData.Instance._SessionID);
    }
    /// <summary>
    /// 购买次数
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveBuyDungeonTimes(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        BuyDungeonTimesResp tmpData = Serializer.Deserialize<BuyDungeonTimesResp>(data.ms);
        FightRelatedModule.Instance.ReceiveBuyDungeonTimes(tmpData);
    }

    public void SendBuyOtherDungeonTimes(BuyOtherDungeonTimesReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<BuyOtherDungeonTimesReq>(0, data, MessageID.BuyOtherDungeonTimesReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveBuyOtherDungeonTimes(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        BuyOtherDungeonTimesResp tmpData = Serializer.Deserialize<BuyOtherDungeonTimesResp>(data.ms);
        FightRelatedModule.Instance.ReceiveBuyOtherDungeonTimes(tmpData);
    }

    public void SendOneKeyReplaceEquip(OneKeyReplaceEquipReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<OneKeyReplaceEquipReq>(0, data, MessageID.OneKeyReplaceEquipReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveOneKeyReplaceEquip(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        OneKeyReplaceEquipResp tmpData = Serializer.Deserialize<OneKeyReplaceEquipResp>(data.ms);
        FightRelatedModule.Instance.ReceiveOneKeyReplaceEquip(tmpData);
    }
    /// <summary>
    /// 异域探险 结算
    /// </summary>
    /// <param name="req"></param>
    public void UnionPveDgnRewardReq(UnionPveDgnRewardReq req)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<UnionPveDgnRewardReq>(0, req, MessageID.UnionPveDgnRewardReqID, PlayerData.Instance._SessionID);
    }

    public void UnionPveDgnRewardResp(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        UnionPveDgnRewardResp tData = Serializer.Deserialize<UnionPveDgnRewardResp>(data.ms);
        FightRelatedModule.Instance.ReceiveUnionPveDgnReward(tData);
    }

    /// <summary>
    /// 奴隶战  开始 
    /// </summary>
    /// <param name="data"></param>
    public void SendStartEslaveFightReq(StartEnslaveFightReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<StartEnslaveFightReq>(0, data, MessageID.StartEnslaveFightReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveStartEnslaveFightResp(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        StartEnslaveFightResp tData = Serializer.Deserialize<StartEnslaveFightResp>(data.ms);
        FightRelatedModule.Instance.ReceiveStartEnslaveFightResp(tData);
    }

    /// <summary>
    /// 奴隶战 结算
    /// </summary>
    /// <param name="data"></param>
    public void SendEnslaveFightOverReq(EnslaveFightOverReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnslaveFightOverReq>(0, data, MessageID.EnslaveFightOverReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnslaveFightOverResp(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        EnslaveFightOverResp tData = Serializer.Deserialize<EnslaveFightOverResp>(data.ms);
        FightRelatedModule.Instance.ReceiveEnslaveFightOverResp(tData);
    }
    public void SendEnslaveFightBeforeBack(EnslaveFightBeforeBackReq data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(true);
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EnslaveFightBeforeBackReq>(0, data, MessageID.EnslaveFightBeforeBackReqID, PlayerData.Instance._SessionID);
    }
    public void ReceiveEnslaveFightBeforeBack(Packet data)
    {
        //UISystem.Instance.HintView.SetLoadingVisible(false);
        EnslaveFightBeforeBackResp tData = Serializer.Deserialize<EnslaveFightBeforeBackResp>(data.ms);
        FightRelatedModule.Instance.ReceiveEnslaveFightBeforeBack(tData);
    }

    public void SendEndPole(EndPoleReq data)
    {
        NetWorkManager.Instance.mHttpGameNetWork.SendMsg<EndPoleReq>(0, data, MessageID.EndPoleReqID, PlayerData.Instance._SessionID);
    }

    public void ReceiveEndPole(Packet data)
    {
        EndPoleResp resp = Serializer.Deserialize<EndPoleResp>(data.ms);
        FightRelatedModule.Instance.ReceiveEndPole(resp);
    }

}
