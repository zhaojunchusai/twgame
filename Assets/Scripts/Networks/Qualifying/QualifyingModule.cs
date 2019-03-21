using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class QualifyingModule : Singleton<QualifyingModule>
{
    private QualifyingNetwork mQualifyingNetwork;

    public EnterPoleLobbyResp poleLobbyData;

    private bool mIsRevenge = false;
    public bool IsRevenge
    {
        get
        {
            return mIsRevenge;
        }
    }

    private string mLogUID = string.Empty;
    public string LogUID
    {
        get
        {
            return mLogUID;
        }
    }

    private uint petTypeID = 0;

    public void SetIsRevenge(bool status, string uid = "")
    {
        mIsRevenge = status;
        mLogUID = uid;
    }

    public void Initialize()
    {
        if (mQualifyingNetwork == null)
        {
            mQualifyingNetwork = new QualifyingNetwork();
        }
        petTypeID = 0;
        mQualifyingNetwork.RegisterMsg();
    }

    public void SendEnterPoleLobby()
    {
        EnterPoleLobbyReq data = new EnterPoleLobbyReq();
        mQualifyingNetwork.SendEnterPoleLobby(data);
    }

    public void ReceiveEnterPoleLobby(EnterPoleLobbyResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            poleLobbyData = data;
            PlayerData.Instance._QualifyingInfo.defence_soldiers.Clear();
            PlayerData.Instance._QualifyingInfo.defence_soldiers.AddRange(data.defence_soldiers);
            PlayerData.Instance._QualifyingInfo.defence_equips.Clear();
            PlayerData.Instance._QualifyingInfo.defence_equips.AddRange(data.defence_equips);
            PlayerData.Instance.QualifyingScore = data.pole_recinf.season_score;
            poleLobbyData.combat_power = data.combat_power;
            UISystem.Instance.QualifyingView.ReceiveEnterQualify();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendSavePoleArray(List<SoldierList> soldiers, List<EquipList> equips, int combat_power, uint pettypeid)
    {
        SavePoleArrayReq data = new SavePoleArrayReq();
        data.defence_soldiers.Clear();
        data.defence_soldiers.AddRange(soldiers);
        data.defence_equips.Clear();
        data.defence_equips.AddRange(equips);
        data.combat_power = combat_power;
        data.pettypeid = (int)pettypeid;
        petTypeID = pettypeid;
        mQualifyingNetwork.SendSavePoleArray(data);
    }

    public void ReceiveSavePoleArray(SavePoleArrayResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            poleLobbyData.pettypeid = (int)petTypeID;
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW))
            {
                UISystem.Instance.PrepareBattleView.OnSavePoleArrayResp(data.combat_power);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    public void SendStartPole(int type, uint accid, string uid, List<SoldierList> soldeirs, List<EquipList> equips)
    {
        StartPoleReq data = new StartPoleReq();
        data.accid = accid;
        data.fight_type = type;
        data.uid = uid;
        data.defence_soldiers.Clear();
        data.defence_soldiers.AddRange(soldeirs);
        data.defence_equips.Clear();
        data.defence_equips.AddRange(equips);
        mQualifyingNetwork.SendStartPole(data);
    }

    public void ReceiveStartPole(StartPoleResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (data.fight_type == 2)
            {
                fogs.proto.msg.ItemInfo iteminfo = new fogs.proto.msg.ItemInfo();
                iteminfo.id = data.consume.id;
                iteminfo.num = data.consume.num;
                PlayerData.Instance.UpdateItem(iteminfo);
                poleLobbyData.pole_recinf.total_revenge_times = data.total_revenge_times;
            }
            UISystem.Instance.PrepareBattleView.OnReadyQualifyingBattle(data);
        }
        else if (data.result == (int)ErrorCodeEnum.FAILED)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.QUALIFYING_MATCHFAILD_TIP);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendPoleReward(int key)
    {
        PoleRewardReq data = new PoleRewardReq();
        data.key = key;
        mQualifyingNetwork.SendPoleReward(data);
    }

    public void ReceivePoleReward(PoleRewardResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDropData(data.drop_list);
            UISystem.Instance.QualifyingView.OnReceiveAwardSuccess(data.key, data.drop_list);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    public void SendPoleRecord()
    {
        PoleRecordReq data = new PoleRecordReq();
        mQualifyingNetwork.SendPoleRecord(data);
    }

    public void ReceivePoleRecord(PoleRecordResp data)
    {
        if (data.result == (uint)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.QualifyingView.OpenBattleLogPanel(data.records);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendPoleRevenge(uint accid, string uid)
    {
        PoleRevengeReq data = new PoleRevengeReq();
        data.accid = accid;
        data.uid = uid;
        mQualifyingNetwork.SendPoleRevenge(data);
    }

    public void ReceivePoleRevenge(PoleRevengeResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.QualifyingView.OnPoleRevengeSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendPoleBuyTimes()
    {
        PoleBuyTimesReq data = new PoleBuyTimesReq();
        mQualifyingNetwork.SendPoleBuyTimes(data);
    }


    public void ReceivePoleBuyTimes(PoleBuyTimesResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            poleLobbyData.buy_times = data.times;
            PlayerData.Instance.UpdateDiamond(data.diamond);
            UISystem.Instance.QualifyingView.UpdatePurchaseStatus();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 匹配对手  1表主动匹配  2表重新匹配
    /// </summary>
    /// <param name="type"></param>
    public void SendMatchPole(int type)
    {
        MatchPoleReq data = new MatchPoleReq();
        data.type = type;
        mQualifyingNetwork.SendMatchPole(data);
    }

    public void ReceiveMatchPole(MatchPoleResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.QualifyingView.OnReMatchOppentSuccess(data.type, data.next_tick, data.opponent, data.can_get);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// tip = 1 不再提醒
    /// </summary>
    /// <param name="tip"></param>
    public void SendClearPoleMatchCD(int tip)
    {
        ClearPoleMatchCDReq data = new ClearPoleMatchCDReq();
        data.clear_cd_tip = tip;
        mQualifyingNetwork.SendClearPoleMatchCD(data);
    }

    public void ReceiveClearPoleMatchCD(ClearPoleMatchCDResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            fogs.proto.msg.ItemInfo iteminfo = new fogs.proto.msg.ItemInfo();
            iteminfo.id = data.consume.id;
            iteminfo.num = data.consume.num;
            PlayerData.Instance.UpdateItem(iteminfo);
            UISystem.Instance.QualifyingView.UpdateRematchCD(0);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void Uninitialize()
    {
        if (mQualifyingNetwork != null)
            mQualifyingNetwork.RemoveMsg();
    }
}