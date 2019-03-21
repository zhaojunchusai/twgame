using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class PVPModule : Singleton<PVPModule>
{
    private PVPNetWork mPVPNetWork;

    private EnterArenaLobbyResp pvpData;

    /// <summary>
    /// 竞技场数据
    /// </summary>
    public EnterArenaLobbyResp PvpData
    {
        get 
        {
            if (pvpData == null)
                pvpData = new EnterArenaLobbyResp();
            return pvpData;
        }
    }
    /// <summary>
    /// 战斗日志
    /// </summary>
    public List<ArenaRecord> recordList;
    /// <summary>
    /// 挑战的敌方玩家
    /// </summary>
    public ArenaPlayer enemyPlayer;
    /// <summary>
    /// 冷却时间
    /// </summary>
    public float CDTime;

    /// <summary>
    /// 剩余挑战次数
    /// </summary>
    public int ChallengeCount;
    public bool isFightState = false;
    public class DefenseSoldier
    {
        public Soldier soldier;
        public int count;
    }
    //public List<DefenseSoldier> defenseSoldiers;

    public bool IsBattleLog = false;

    public void Initialize()
    {
        if (mPVPNetWork == null)
        {
            mPVPNetWork = new PVPNetWork();
        }
        mPVPNetWork.RegisterMsg();
        pvpData = new EnterArenaLobbyResp();
        recordList = new List<ArenaRecord>();
        enemyPlayer = new ArenaPlayer();
        CDTime = 0;
        ChallengeCount = 0;
    }
    /// <summary>
    /// 请求挑战
    /// </summary>
    /// <param name="data"></param>
    public void SendArenaChallenge(uint charID, uint rank, bool reverge = false)
    {
        isReverge = reverge;
        ArenaChallengeReq data = new ArenaChallengeReq();
        data.charid = charID;
        data.rank_num = rank;
        mPVPNetWork.SendArenaChallenge(data);
    }

    private bool isReverge = false;

    public void ReceiveArenaChallenge(ArenaChallengeResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (isReverge)
            {
                UISystem.Instance.PvpView.EnterReverge();
            }
            else
            {
                UISystem.Instance.PvpView.EnterChallenge();
            }
        }
        else if (data.result == (int)ErrorCodeEnum.PlayerArenaRankChange)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PLAYERARENARANKCHANGE);
        }
        else if (data.result == (int)ErrorCodeEnum.OpponentInArena)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_OPPONENtINARENA);
        }
        else if (data.result == (int)ErrorCodeEnum.PlayerInArena)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PLAYERINARENA);
        }
        else if (data.result == (int)ErrorCodeEnum.OpponentRankTooLow)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_OPPONENTRANKTOOLOW);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 请求竞技场战斗日志
    /// </summary>
    public void SendArenaRecord()
    {
        ArenaRecordReq data = new ArenaRecordReq();
        mPVPNetWork.SendArenaRecord(data);
    }
    /// <summary>
    /// 竞技场战斗日志
    /// </summary>
    public void ReceiveArenaRecord(ArenaRecordResp data)
    {
        if (data.records != null)
        {
            recordList = data.records;
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_PVPVIEW))
            {
                UISystem.Instance.PvpView.UpdateBattleLogPanel();
            }
        }
    }

    /// <summary>
    /// 进入竞技场大厅
    /// </summary>
    public void SendEnterArenaLobby()
    {
        EnterArenaLobbyReq data = new EnterArenaLobbyReq();
        mPVPNetWork.SendEnterArenaLobby(data);
    }
    /// <summary>
    /// 进入竞技场大厅
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEnterArenaLobby(EnterArenaLobbyResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            pvpData = data;
            UISystem.Instance.PvpView.UpdateViewInfo();
        }
        else
        {
            pvpData = null;
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public void SendSaveDefenceArray(List<EquipList> equips, List<fogs.proto.msg.SoldierList> soldiers, int combat_power,uint petID)
    {
        SaveDefenceArrayReq data = new SaveDefenceArrayReq();
        data.defence_equips.AddRange(equips);
        data.defence_soldiers.AddRange(soldiers);
        data.combat_power = combat_power;
        data.pettypeid = (int)petID;
        mPVPNetWork.SendSaveDefenceArray(data);
    }
    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public void ReceiveSaveDefenceArray(SaveDefenceArrayResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PVPModule.Instance.PvpData.defence_combat_power = data.combat_power;
            UISystem.Instance.PvpView.SaveDefenceArraySuccess();
        }
    }
    /// <summary>
    /// 开始竞技场
    /// </summary>
    public void SendStartArena(uint charID, List<EquipList> equips, List<fogs.proto.msg.SoldierList> soldiers, uint rank)
    {
        StartArenaReq data = new StartArenaReq();
        data.charid = charID;
        data.attack_equips.AddRange(equips);
        data.attack_soldiers.AddRange(soldiers);
        data.rank_num = rank;
        mPVPNetWork.SendStartArena(data);
    }

    /// <summary>
    /// 开始竞技场
    /// </summary>
    public void ReceiveStartArena(StartArenaResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.PvpView.EnterTheBattle(data.hero_attr);
        }
        else if (data.result == (int)ErrorCodeEnum.PlayerInArena)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_PLAYERINARENA, () => { UISystem.Instance.PvpView.ExitReadyBattle(); });
        }
        else if (data.result == (int)ErrorCodeEnum.OpponentInArena)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_OPPONENtINARENA, () => { UISystem.Instance.PvpView.ExitReadyBattle(); });
        }
        else if (data.result == (int)ErrorCodeEnum.PlayerOptInCD)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_PLAYEROPTINCD, () => { UISystem.Instance.PvpView.ExitReadyBattle(); });
        }
        else if (data.result == (int)ErrorCodeEnum.PlayerArenaRankChange)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.PVP_PLAYERARENARANKCHANGE, () => { UISystem.Instance.PvpView.ExitReadyBattle(); });
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
        if (data.result != (int)ErrorCodeEnum.SUCCESS)
        {
            isFightState = false;
        }
    }

    /// <summary>
    /// 竞技场结算
    /// </summary>
    public void SendArenaReward(ArenaResult result, int key, string sign)
    {
        ArenaRewardReq data = new ArenaRewardReq();
        data.result = result;
        data.key = key;
        data.sign = sign;
        mPVPNetWork.SendArenaReward(data);
    }
    /// <summary>
    /// 竞技场结算
    /// </summary>
    public void ReceiveArenaReward(ArenaRewardResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    /// <summary>
    /// 重新刷新对手
    /// </summary>
    public void SendRefreshOpponents()
    {
        RefreshOpponentsReq data = new RefreshOpponentsReq();
        mPVPNetWork.SendRefreshOpponents(data);
    }
    /// <summary>
    /// 重新刷新对手
    /// </summary>
    public void ReceiveRefreshOpponents(RefreshOpponentsResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            //PlayerData.Instance._Gold = data.;
            PvpData.opponents.Clear();
            PvpData.opponents.AddRange(data.opponents);
            //pvpData.refresh_times = (uint)data.;
            UISystem.Instance.PvpView.UpdateViewInfo();

            //UISystem.Instance.PvpView.PlayPVPRefreshEffect();
        }
    }
    /// <summary>
    /// 重置竞技场CD
    /// </summary>
    public void SendClearArenaCD()
    {
        ClearArenaCDReq data = new ClearArenaCDReq();
        mPVPNetWork.SendClearArenaCD(data);
    }
    /// <summary>
    /// 重置竞技场CD
    /// </summary>
    public void ReceiveClearArenaCD(ClearArenaCDResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDiamond((int)data.diamond);
            UISystem.Instance.PvpView.ClearCDTimeSuccess();
        }
    }


    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public void SendAddArenaTimes()
    {
        AddArenaTimesReq data = new AddArenaTimesReq();
        mPVPNetWork.SendAddArenaTimes(data);
    }
    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public void ReceiveAddArenaTimes(AddArenaTimesResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDiamond(data.diamond);
            PvpData.buy_times = data.buy_times;
            PvpData.today_times = data.today_times;
            UISystem.Instance.PvpView.UpdateViewInfo();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public void SendArenaRankList(int page)
    {
        ArenaRankListReq data = new ArenaRankListReq();
        data.page = page;
        mPVPNetWork.SendArenaRankList(data);
    }
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public void ReceiveArenaRankList(ArenaRankListResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {

        }
    }

    public void Uninitialize()
    {
        if (mPVPNetWork != null)
            mPVPNetWork.RemoveMsg();
        mPVPNetWork = null;
        isFightState = false;
    }
}