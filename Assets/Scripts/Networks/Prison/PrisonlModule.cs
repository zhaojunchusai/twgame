using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using ProtoBuf;
using Assets.Script.Common.StateMachine;

public class PrisonModule : Singleton<PrisonModule>
{
    public PrisonNetWork mPrisonNetWork;
    public void Initialize()
    {
        if (mPrisonNetWork == null)
        {
            mPrisonNetWork = new PrisonNetWork();
            mPrisonNetWork.RegisterMsg();
        }
    }
    public void Uninitialize() 
    {
        if (mPrisonNetWork != null)
            mPrisonNetWork.RemoveMsg();
        mPrisonNetWork = null;
    }
    /// <summary>
    /// 发起奴役
    /// </summary>
    public void SendConquer(string accName)
    {
    }
    /// <summary>
    /// 发起拯救
    /// </summary>
    public void SendRescue(string accName)
    {
    }
    /// <summary>
    /// 获取地方玩家信息
    /// </summary>
    /// <param name="data"></param>
    public void SendGetOtherPlayerInfoReq(string accname,uint id)
    {
        GetOtherPlayerInfoReq tmpInfo = new GetOtherPlayerInfoReq();
        tmpInfo.accname = accname;
        tmpInfo.area_id = id;

        mPrisonNetWork.SendGetOtherPlayerInfoReq(tmpInfo);
    }
    public void ReceiveGetOtherPlayerInfoResp(Packet data)
    {
        GetOtherPlayerInfoResp tData = Serializer.Deserialize<GetOtherPlayerInfoResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetOtherPlayerInfo(tData);
    }
    /// <summary>
    /// 收取一个囚牢金钱
    /// </summary>
    /// <param name="data"></param>
    public void SendCollectMoneyReq(uint id)
    {
        CollectMoneyReq tmpInfo = new CollectMoneyReq();
        tmpInfo.prison_id = id;

        mPrisonNetWork.SendCollectMoneyReq(tmpInfo);
    }
    public void ReceiveCollectMoneyResq(Packet data)
    {
        CollectMoneyResp tData = Serializer.Deserialize<CollectMoneyResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveCollectMoneyResp(tData);
    }
    /// <summary>
    /// 释放一个囚牢
    /// </summary>
    /// <param name="data"></param>
    public void SendReleaseEnslaveReq(uint id)
    {
        ReleaseEnslaveReq tmpInfo = new ReleaseEnslaveReq();
        tmpInfo.prison_id = id;

        mPrisonNetWork.SendReleaseEnslaveReq(tmpInfo);
    }
    public void ReceiveReleaseEnslaveResq(Packet data)
    {
        ReleaseEnslaveResp tData = Serializer.Deserialize<ReleaseEnslaveResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveReleaseEnslaveResp(tData);

    }
    /// <summary>
    /// 收取所以囚牢金币
    /// </summary>
    /// <param name="data"></param>
    public void SendCollectAllMoneyReq()
    {
        CollectAllMoneyReq tmpInfo = new CollectAllMoneyReq();

        mPrisonNetWork.SendCollectAllMoneyReq(tmpInfo);
    }
    public void ReceiveCollectAllMoneyResq(Packet data)
    {
        CollectAllMoneyResp tData = Serializer.Deserialize<CollectAllMoneyResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveCollectAllMoneyResp(tData);

    }

    /// <summary>
    /// 获取奴役记录
    /// </summary>
    /// <param name="data"></param>
    public void SendGetEnslaveRecordReq()
    {
        GetEnslaveRecordReq tmpInfo = new GetEnslaveRecordReq();

        mPrisonNetWork.SendGetEnslaveRecordReq(tmpInfo);
    }
    public void ReceiveGetEnslaveRecordResq(Packet data)
    {
        GetEnslaveRecordResp tData = Serializer.Deserialize<GetEnslaveRecordResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetEnslaveRecordResp(tData);

    }
    /// <summary>
    /// 请求解救
    /// </summary>
    /// <param name="data"></param>
    public void SendAskSaveEnslaveReq()
    {
        AskSaveEnslaveReq tmpInfo = new AskSaveEnslaveReq();

        mPrisonNetWork.SendAskSaveEnslaveReq(tmpInfo);
    }
    public void ReceiveAskSaveEnslaveResq(Packet data)
    {
        AskSaveEnslaveResp tData = Serializer.Deserialize<AskSaveEnslaveResp>(data.ms);

    }

    /// <summary>
    /// 匹配陌生人
    /// </summary>
    /// <param name="data"></param>
    public void SendMatchStrangerReq(fogs.proto.msg.EnslaveMatchType type,uint prison_id)
    {
        MatchStrangerReq tmpInfo = new MatchStrangerReq();
        tmpInfo.prison_id = prison_id;
        tmpInfo.type = type;
        mPrisonNetWork.SendMatchStrangerReq(tmpInfo);
    }
    public void ReceiveMatchStrangerResq(Packet data)
    {
        MatchStrangerResp tData = Serializer.Deserialize<MatchStrangerResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetStronger(tData);
    }
    /// <summary>
    /// 获取囚牢信息
    /// </summary>
    /// <param name="data"></param>
    public void SendGetEnslaveInfoReq()
    {
        GetEnslaveInfoReq data = new GetEnslaveInfoReq();
        mPrisonNetWork.SendGetEnslaveInfoReq(data);
    }
    public void ReceiveGetEnslaveInfoResp(Packet data)
    {
        GetEnslaveInfoResp tData = Serializer.Deserialize<GetEnslaveInfoResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetEnslaveInfoResp(tData);
    }
    /// <summary>
    /// 奴役站前准备
    /// </summary>
    /// <param name="data"></param>
    public void SendEnslaveFightBeforeReq(EnslaveFightType type,string accname,uint area_id,string host_accname,uint host_area_id,uint prison_id)
    {
        EnslaveFightBeforeReq tmpData = new EnslaveFightBeforeReq();
        tmpData.type = type;
        tmpData.accname = accname;
        tmpData.area_id = area_id;
        tmpData.host_accname = host_accname;
        tmpData.host_area_id = host_area_id;
        tmpData.prison_id = prison_id;
        mPrisonNetWork.SendEnslaveFightBeforeReq(tmpData);
    }
    public void ReceiveEnslaveFightBeforeResq(Packet data)
    {
        EnslaveFightBeforeResp tData = Serializer.Deserialize<EnslaveFightBeforeResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveEnslaveFightBeforeResq(tData);
    }
    /// <summary>
    /// 获取仇人列表
    /// </summary>
    public void SendGetEnemyPlayerReq()
    {
        GetEnemyPlayerReq tmpData = new GetEnemyPlayerReq();
        mPrisonNetWork.SendGetEnemyPlayerReq(tmpData);
    }
    public void ReceiveGetEnemyPlayerResq(Packet data)
    {
        GetEnemyPlayerResp tData = Serializer.Deserialize<GetEnemyPlayerResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetEnemyPlayerResq(tData);
    }
    /// <summary>
    /// 获取逃奴列表
    /// </summary>
    /// <param name="data"></param>
    public void SendGetRunAwayPlayerReq()
    {
        GetRunAwayPlayerReq tmpData = new GetRunAwayPlayerReq();
        mPrisonNetWork.SendGetRunAwayPlayerReq(tmpData);
    }
    public void ReceiveGetRunAwayPlayerResq(Packet data)
    {
        GetRunAwayPlayerResp tData = Serializer.Deserialize<GetRunAwayPlayerResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveGetRunAwayPlayerResq(tData);
    }
    /// <summary>
    /// 锁定
    /// </summary>
    /// <param name="accname"></param>
    /// <param name="type"></param>
    public void SendEnslaveLockedReq(string accname,LockedType type)
    {
        EnslaveLockedReq tmpData = new EnslaveLockedReq();
        tmpData.accname = accname;
        tmpData.type = type;
        mPrisonNetWork.SendEnslaveLockedReq(tmpData);
    }
    public void ReceiveEnslaveLockedResq(Packet data)
    {
        EnslaveLockedResp tData = Serializer.Deserialize<EnslaveLockedResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveEnslaveLockedResq(tData);
    }
    /// <summary>
    /// 解锁
    /// </summary>
    /// <param name="accname"></param>
    /// <param name="type"></param>
    public void SendEnslaveUnLockedReq(string accname, LockedType type)
    {
        EnslaveUnLockedReq tmpData = new EnslaveUnLockedReq();
        tmpData.accname = accname;
        tmpData.type = type;
        mPrisonNetWork.SendEnslaveUnLockedReq(tmpData);
    }
    public void ReceiveEnslaveUnLockedResq(Packet data)
    {
        EnslaveUnLockedResp tData = Serializer.Deserialize<EnslaveUnLockedResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveEnslaveUnLockedResq(tData);
    }
    public void SendAskHelpReq(int type)
    {
        AskHelpReq tmp = new AskHelpReq();
        tmp.type = type;
        mPrisonNetWork.SendAskHelpReq(tmp);
    }
    public void ReceiveAskHelpResp(Packet data)
    {
        AskHelpResp tData = Serializer.Deserialize<AskHelpResp>(data.ms);
        if (tData.result == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PRISON_SUCCESS_ASKHELP);
            
        }
        else
        {
            ErrorCode.ShowErrorTip(tData.result);
        }
        if (tData.type == 1)
        {
            PlayerData.Instance._Prison.GetPrisonInfo().last_time = tData.last_time;
            PlayerData.Instance._Prison.OnPrisonControlEvent(PrisonType.ReceiveAskWorldHelpResp, tData.result);
        }
        else
        {
            PlayerData.Instance._Prison.GetPrisonInfo().union_last_time = tData.last_time;
            PlayerData.Instance._Prison.OnPrisonControlEvent(PrisonType.ReceiveAskUnionHelpResp, tData.result);
        }
    }
    public void SendWorldSaveCheckReq(int type,string accname,uint area_id,ulong time)
    {
        WorldSaveCheckReq tmpData = new WorldSaveCheckReq();
        tmpData.type = type;
        tmpData.accname = accname;
        tmpData.area_id = area_id;
        tmpData.time = time;
        mPrisonNetWork.SendWorldSaveCheckReq(tmpData);
    }
    public void ReceiveWorldSaveCheckResq(Packet data)
    {
        WorldSaveCheckResp tData = Serializer.Deserialize<WorldSaveCheckResp>(data.ms);
        PlayerData.Instance._Prison.ReceiveWorldSaveCheckResq(tData);
    }
}
