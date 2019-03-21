using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public enum PrisonType
{
    /// <summary>
    /// 获取玩家信息
    /// </summary>
    GetOtherPlayerInfoResp,
    /// <summary>
    /// 匹配陌生人
    /// </summary>
    MatchStrangerResp,
    /// <summary>
    /// 奴隶记录
    /// </summary>
    GetEnslaveRecordResp,
    /// <summary>
    /// 收取一个囚牢的金钱
    /// </summary>
    CollectMoneyReq,
    /// <summary>
    /// 手动释放某个囚牢信息
    /// </summary>
    ReleaseEnslaveReq,
    /// <summary>
    /// 收取所有囚牢金币
    /// </summary>
    CollectAllMoneyReq,
    /// <summary>
    /// 获取囚牢信息
    /// </summary>
    GetEnslaveInfoResp,
    /// <summary>
    /// 奴役战斗准备
    /// </summary>
    EnslaveFightBeforeResp,
    /// <summary>
    /// 奴役战斗开始
    /// </summary>
    StartEnslaveFightResp,
    /// <summary>
    /// 奴役战斗结算
    /// </summary>
    EnslaveFightOverResp,
    /// <summary>
    /// 获取仇人列表
    /// </summary>
    GetEnemyPlayerResp,
    /// <summary>
    /// 获取逃奴列表
    /// </summary>
    GetRunAwayPlayerResp,
    /// <summary>
    /// 锁定
    /// </summary>
    EnslaveLockedResp,
    /// <summary>
    /// 解锁
    /// </summary>
    EnslaveUnLockedResp,
    /// <summary>
    /// 奴役记录
    /// </summary>
    IsNotify,
    /// <summary>
    /// 发起世界求援
    /// </summary>
    ReceiveAskWorldHelpResp,
    /// <summary>
    /// 发起军团求援
    /// </summary>
    ReceiveAskUnionHelpResp
}
public class Prison
{
    /// <summary>
    /// 囚牢总信息
    /// </summary>
    private fogs.proto.msg.EnslaveInfo PrisonInfo;
    /// <summary>
    /// 囚牢列表
    /// </summary>
    private List<fogs.proto.msg.PrisonInfo> PrisonList;
    /// <summary>
    /// 奴役记录
    /// </summary>
    private List<fogs.proto.msg.EnslaveRecord> EnslaveList;
    /// <summary>
    /// 陌生玩家列表
    /// </summary>
    private List<fogs.proto.msg.OtherPlayer> StrangerList = new List<OtherPlayer>();
    /// <summary>
    /// 仇人玩家列表
    /// </summary>
    public List<fogs.proto.msg.OtherPlayer> EnemyList = new List<OtherPlayer>();
    /// <summary>
    /// 逃奴玩家列表
    /// </summary>
    public List<fogs.proto.msg.OtherPlayer> RunAwayList = new List<OtherPlayer>();

    /// <summary>
    /// 最新匹配到的玩家
    /// </summary>
    public fogs.proto.msg.ArenaPlayer _atenaPlayer;

    public fogs.proto.msg.EnslaveFightBeforeReq SaveFightBeforeReq = new EnslaveFightBeforeReq();

    private KeyValuePair<string, uint> SaveAskHelp = new KeyValuePair<string, uint>();
    private bool IsNotify = false;
    public int prison_id = 0;
    public int MaxCount = 4;
    public fogs.proto.msg.EnslaveMatchType type = EnslaveMatchType.EMT_LEVEL;
    public delegate void PrisonControlDelegate(PrisonType type, int errorCode);
    public event PrisonControlDelegate PrisonControlEvent;
    public void Serialize(fogs.proto.msg.EnslaveInfo info)
    {
        if (info != null)
            this.PrisonInfo = info;
        if (info.prison_info != null)
            this.PrisonList = info.prison_info;
        if (info.enslave_record != null)
            this.EnslaveList = info.enslave_record;
    }
    public void SetNotify(bool notify)
    {
        this.IsNotify = notify;
        int result = 0;
        if (!notify)
            result = 1;
        this.OnPrisonControlEvent(PrisonType.IsNotify, result);
    }
    public bool GetNotify()
    {
        return this.IsNotify;
    }
    private void TemporaryDataClear()
    {
        this._atenaPlayer = null;
        if (this.SaveFightBeforeReq != null)
        {
            this.SaveFightBeforeReq.accname = "";
            this.SaveFightBeforeReq.area_id = 0;
            this.SaveFightBeforeReq.host_accname = "";
            this.SaveFightBeforeReq.host_area_id = 0;
            this.SaveFightBeforeReq.type = EnslaveFightType.EFT_NONE;
        }
    }
    public void Clear()
    {
        if (this.PrisonList != null)
            this.PrisonList.Clear();
        if (this.EnemyList != null)
            this.EnemyList.Clear();
        if (this.EnslaveList != null)
            this.EnslaveList.Clear();
    }
    public void OnPrisonControlEvent(PrisonType type, int errorCode)
    {
        if (this.PrisonControlEvent != null)
            this.PrisonControlEvent(type, errorCode);
    }
    /// <summary>
    /// 判断是否是自己的奴隶
    /// </summary>
    /// <param name="accName"></param>
    /// <returns></returns>
    public bool IsSelfEnslave(string accName)
    {
        if (this.PrisonList == null)
            return false;
        return this.PrisonList.Find((info) => { if (info == null)return false; return info.accname == accName; }) != null;
    }
    /// <summary>
    /// 判断是否是自己的主人
    /// </summary>
    /// <param name="accName"></param>
    /// <returns></returns>
    public bool IsSelfMaster(string accName)
    {
        if (this.PrisonInfo == null)
            return false;
        return this.PrisonInfo.slave_holder.accname == accName;
    }
    public bool EnableConquer(string accName)
    {
        return false;
    }
    public bool EnableRescue(string accName)
    {
        return false;
    }
    /// <summary>
    /// 发起世界救援
    /// </summary>
    /// <param name="accName"></param>
    public void StartWorldRescue(string accName, uint id)
    {
        PrisonModule.Instance.SendAskHelpReq(1);
        return;
    }
    /// <summary>
    /// 发起军团救援
    /// </summary>
    /// <param name="accName"></param>
    public void StartUnionRescue(string accName,uint id)
    {
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PRISON_UNOND_LOCK);
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Sociaty, true))
            PrisonModule.Instance.SendAskHelpReq(2);
        return;
    }

    /// <summary>
    /// 发起解救
    /// </summary>
    /// <param name="accName"></param>
    public void GetOtherPlayerInfo(string accname, uint id)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Slave, true))
            return;
        this.prison_id = 0;
        this.StartGetOtherPlayerInfo(accname,id);
        return;
    }
    /// <summary>
    /// 世界救援发起解救
    /// </summary>
    /// <param name="accName"></param>
    public void GetAskInfo(string accname, uint id,ulong time)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Slave, true))
            return;
        if (accname.Equals(PlayerData.Instance._AccountName))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PRISON_SELD_HELP);
            return;
        }

        this.SendWorldSaveCheckReq(1,accname,id,time);
        return;
    }

    /// <summary>
    /// 匹配陌生人
    /// </summary>
    public void StartGetStronger(fogs.proto.msg.EnslaveMatchType type, uint prison_id)
    {
        Debug.LogWarning("     StartGetStronger: " + type.ToString() + "|" + prison_id.ToString());
        this.type = type;
        PrisonModule.Instance.SendMatchStrangerReq(type, prison_id);
        if (prison_id > 0)
            this.prison_id = (int)prison_id;
    }
    public void ReceiveGetStronger(fogs.proto.msg.MatchStrangerResp result)
    {
        if (this.StrangerList != null)
            this.StrangerList.Clear();
        if(result.result == 0)
        {
            this.StrangerList = result.stranger_list;
            if (this.StrangerList == null || this.StrangerList.Count <= 0)
            {
                string message = "";
                if (type == EnslaveMatchType.EMT_LEVEL)
                    message = ConstString.NO_HAVE_PEORSON_LEVEL;
                else
                    message = ConstString.NO_HAVE_PEORSON_POWER;
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, message);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.MatchStrangerResp, (int)result.result);

    }
    /// <summary>
    /// 获取某人信息
    /// </summary>
    public void StartGetOtherPlayerInfo(string accname,uint id)
    {
        PrisonModule.Instance.SendGetOtherPlayerInfoReq(accname,id);
    }
    public void ReceiveGetOtherPlayerInfo(fogs.proto.msg.GetOtherPlayerInfoResp result)
    {
        if(result.result == 0)
        {
            TemporaryDataClear();
            this._atenaPlayer = result.player_info;
            this.SaveFightBeforeReq.accname = result.accname;
            this.SaveFightBeforeReq.area_id = result.area_id;
            this.SaveFightBeforeReq.host_accname = result.host_accname;
            this.SaveFightBeforeReq.host_area_id = result.host_area_id;
            this.SaveFightBeforeReq.type = result.type;
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
            PlayerInfoTypeEnum tmpEnum = PlayerInfoTypeEnum.SlaveAttack;
            if (this.SaveFightBeforeReq.type == EnslaveFightType.EFT_AGAINST)
                tmpEnum = PlayerInfoTypeEnum.Revolt;
            UISystem.Instance.PlayerInfoView.UpdateViewInfo(tmpEnum, PlayerData.Instance._Prison._atenaPlayer, !PlayerData.Instance._Prison.SaveFightBeforeReq.host_accname.Equals(""));
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.GetOtherPlayerInfoResp, 0);
    }
    /// <summary>
    /// 获取奴役记录
    /// </summary>
    public void StartGetEnslaveRecordReq()
    {
        PrisonModule.Instance.SendGetEnslaveRecordReq();
    }

    public void ReceiveGetEnslaveRecordResp(fogs.proto.msg.GetEnslaveRecordResp result)
    {
        if(result.result == 0)
        {
            this.EnslaveList = result.enslave_record_list;
            this.EnslaveList.Sort((left, right) => 
            {
                if(left == null || right == null)
                {
                    if (left == null)
                        return 1;
                    else
                        return -1;
                }
                if(left.time != right.time)
                {
                    if (left.time > right.time)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.GetEnslaveRecordResp, (int)result.result);
    }
    /// <summary>
    /// 收取某个囚牢的金钱
    /// </summary>
    /// <param name="prison_id"></param>
    public void StartCollectMoneyReq(uint prison_id)
    {
        PrisonModule.Instance.SendCollectMoneyReq(prison_id);
    }
    public void ReceiveCollectMoneyResp(fogs.proto.msg.CollectMoneyResp result)
    {
        if(result.result == 0)
        {
            UISystem.Instance.PrisonView.PlayOnesFlyEffect(result.prison_info.prison_id);
            PlayerData.Instance.MoneyRefresh(1, result.gold_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();

            this.PrisonInfo.total_money = result.total_gold_num;
            bool IsHad = false;
            for (int i = 0; i < this.PrisonList.Count; ++i )
            {
                if (this.PrisonList[i] == null)
                    continue;

                if (this.PrisonList[i].prison_id == result.prison_info.prison_id)
                {
                    IsHad = true;
                    this.PrisonList[i] = result.prison_info;
                }
            }
            if(!IsHad)
            {
                this.PrisonList.Add(result.prison_info);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }

        this.OnPrisonControlEvent(PrisonType.CollectMoneyReq, (int)result.result);

        return;
    }
    /// <summary>
    /// 收取所有囚牢的游戏币
    /// </summary>
    public void StartCollectAllMoneyReq()
    {
        PrisonModule.Instance.SendCollectAllMoneyReq();
    }
    public void ReceiveCollectAllMoneyResp(fogs.proto.msg.CollectAllMoneyResp result)
    {
        if(result.result == 0)
        {
            UISystem.Instance.PrisonView.PlayALLFlyEffect();
            PlayerData.Instance.MoneyRefresh(1, result.gold_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();

            this.PrisonInfo.total_money = result.total_gold_num;
            this.PrisonList = result.prison_info;
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.CollectAllMoneyReq, (int)result.result);

    }
    /// <summary>
    /// 指定释放某个囚牢的囚犯
    /// </summary>
    /// <param name="prison_id"></param>
    public void StartReleaseEnslaveReq(uint prison_id)
    {
        PrisonModule.Instance.SendReleaseEnslaveReq(prison_id);
    }
    public void ReceiveReleaseEnslaveResp(fogs.proto.msg.ReleaseEnslaveResp result)
    {
        if(result.result == 0)
        {
            PlayerData.Instance.MoneyRefresh(1, result.gold_num);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();

            this.PrisonInfo.total_money = result.total_gold_num;
            int index = this.PrisonList.FindIndex((tmp) => 
            {
                if (tmp == null)
                    return false;
                if (tmp.prison_id == result.prison_info.prison_id)
                    return true;
                return false;
            });

            if (index >= 0)
            {
                fogs.proto.msg.PrisonInfo tmp = new PrisonInfo();
                tmp.locked = 1;
                tmp.status = 1;
                tmp.prison_id = (uint)index + 1;
                this.PrisonList[index] = tmp;
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.ReleaseEnslaveReq, (int)result.result);
    }
    /// <summary>
    /// 获取奴役囚牢信息
    /// </summary>
    public void SendGetEnslaveInfoReq()
    {
        PrisonModule.Instance.SendGetEnslaveInfoReq();
    }
    public void ReceiveGetEnslaveInfoResp(GetEnslaveInfoResp result)
    {
        if (result.result == 0)
        {
            this.PrisonInfo = result.enslave_info;
            this.PrisonList = result.enslave_info.prison_info;
            this.EnslaveList = result.enslave_info.enslave_record;
            PlayerData.Instance.MoneyRefresh(1, (int)result.current_money);
            //if (UISystem.Instance.TopFuncView != null)
            //    UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.GetEnslaveInfoResp, (int)result.result);
    }
    /// <summary>
    /// 发起战前准备请求
    /// </summary>
    public void SendEnslaveFightBeforeReq()
    {
        if (this.SaveFightBeforeReq == null)
            return;
        PrisonModule.Instance.SendEnslaveFightBeforeReq(this.SaveFightBeforeReq.type, this.SaveFightBeforeReq.accname, this.SaveFightBeforeReq.area_id, this.SaveFightBeforeReq.host_accname, this.SaveFightBeforeReq.host_area_id,(uint)this.prison_id);
    }
    public void ReceiveEnslaveFightBeforeResq(EnslaveFightBeforeResp result)
    {
        if (result.result == 0)
        {
            UISystem.Instance.PlayerInfoView.OnReadySlaveBattle();
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
        }
        this.OnPrisonControlEvent(PrisonType.EnslaveFightBeforeResp, (int)result.result);
    }

    public void SendGetEnemyPlayerReq()
    {
        PrisonModule.Instance.SendGetEnemyPlayerReq();
    }
    public void ReceiveGetEnemyPlayerResq(GetEnemyPlayerResp result)
    {
        if (this.EnemyList != null)
            this.EnemyList.Clear();

        if (result.result == 0)
        {
            this.EnemyList = result.enemy_info;
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.GetEnemyPlayerResp, (int)result.result);
    }
    public void SendGetRunAwayPlayerReq()
    {
        PrisonModule.Instance.SendGetRunAwayPlayerReq();
    }
    public void ReceiveGetRunAwayPlayerResq(GetRunAwayPlayerResp result)
    {
        if (this.RunAwayList != null)
            this.RunAwayList.Clear();

        if (result.result == 0)
        {
            this.RunAwayList = result.runaway_info;
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.GetRunAwayPlayerResp, (int)result.result);
    }
    public void SendEnslaveLockedReq(string accname, LockedType type)
    {
        PrisonModule.Instance.SendEnslaveLockedReq(accname, type);
    }
    public void ReceiveEnslaveLockedResq(EnslaveLockedResp result)
    {
        if (result.result == 0)
        {
            List<fogs.proto.msg.OtherPlayer> tmpList = PlayerData.Instance._Prison.EnemyList;
            if(result.type == LockedType.LT_RUNAWAY)
            {
                tmpList = PlayerData.Instance._Prison.RunAwayList;
            }
            for (int i = 0, behind = tmpList.Count - 1; i < tmpList.Count; )
            {
                if (behind < i)
                    break;
                fogs.proto.msg.OtherPlayer startInfo = tmpList[i];
                fogs.proto.msg.OtherPlayer behindInfo = tmpList[behind];

                if (startInfo.locked == 0)
                {
                    ++i;
                }
                if (behindInfo.accname != result.accname)
                {
                    --behind;
                }
                if ((startInfo.locked != 0 || i == behind)&& behindInfo.accname == result.accname)
                {
                    tmpList[behind].locked = 0;
                    fogs.proto.msg.OtherPlayer tmpInfo;
                    tmpInfo = tmpList[i];
                    tmpList[i] = tmpList[behind];
                    tmpList[behind] = tmpInfo;
                    break;
                }
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.EnslaveLockedResp, (int)result.result);
    }
    public void SendEnslaveUnLockedReq(string accname, LockedType type)
    {
        PrisonModule.Instance.SendEnslaveUnLockedReq(accname, type);
    }
    public void ReceiveEnslaveUnLockedResq(EnslaveUnLockedResp result)
    {
        if (result.result == 0)
        {
            List<fogs.proto.msg.OtherPlayer> tmpList = PlayerData.Instance._Prison.EnemyList;
            if (result.type == LockedType.LT_RUNAWAY)
            {
                tmpList = PlayerData.Instance._Prison.RunAwayList;
            }

            int index = tmpList.FindIndex((info) => { if (info == null)return false; return info.accname == result.accname; });
            if(index != -1)
            {
                tmpList[index].locked = 1;
                fogs.proto.msg.OtherPlayer tmpInfo = tmpList[tmpList.Count - 1];
                tmpList[tmpList.Count - 1] = tmpList[index];
                tmpList[index] = tmpInfo;
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(result.result);
        }
        this.OnPrisonControlEvent(PrisonType.EnslaveUnLockedResp, (int)result.result);
    }
    /// <summary>
    /// 点击世界救援之前请求验证
    /// </summary>
    /// <param name="type"></param>
    /// <param name="accname"></param>
    /// <param name="area_id"></param>
    /// <param name="time"></param>
    public void SendWorldSaveCheckReq(int type, string accname, uint area_id, ulong time)
    {
        this.SaveAskHelp = new KeyValuePair<string, uint>(accname,area_id);
        PrisonModule.Instance.SendWorldSaveCheckReq(type,accname,area_id,time);
    }
    public void ReceiveWorldSaveCheckResq(WorldSaveCheckResp data)
    {
        if(data.result == 0)
        {
            this.StartGetOtherPlayerInfo(this.SaveAskHelp.Key, this.SaveAskHelp.Value);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 获取奴隶信息
    /// </summary>
    /// <returns></returns>
    public List<fogs.proto.msg.PrisonInfo> GetPrisonList()
    {
        List<fogs.proto.msg.PrisonInfo> tmpList = new List<PrisonInfo>(5);
        for (uint i = 0; i < this.MaxCount;++i )
        {
            uint mark = i + 1;
            tmpList.Add(PrisonList.Find((info) => { if (info == null)return false; return info.prison_id == mark; }));
        }
        return tmpList;
    }
    /// <summary>
    /// 获取囚牢信息
    /// </summary>
    /// <returns></returns>
    public fogs.proto.msg.EnslaveInfo GetPrisonInfo()
    {
        return this.PrisonInfo;
    }
    /// <summary>
    /// 获取奴役记录
    /// </summary>
    /// <returns></returns>
    public List<fogs.proto.msg.EnslaveRecord> GetEnslaveRecord()
    {
        return this.EnslaveList;
    }
    /// <summary>
    /// 获取陌生人列表
    /// </summary>
    /// <returns></returns>
    public List<fogs.proto.msg.OtherPlayer> GetStrangerList()
    {
        return this.StrangerList;
    }
    /// <summary>
    /// 获取占囚牢个数
    /// </summary>
    /// <returns></returns>
    public int GetPrisonInfoCount()
    {
        if (this.PrisonList == null)
            return 0;

        return this.PrisonList.FindAll((tmpInfo) => { if (tmpInfo == null)return false; return tmpInfo.locked == 1; }).Count;
    }
    public int GetPrisonCount()
    {
        if (this.PrisonList == null)
            return 0;

        return this.PrisonList.FindAll((tmpInfo) => { if (tmpInfo == null)return false; return tmpInfo.accname != ""; }).Count;
    }
}
