using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using Assets.Script.Common.StateMachine;


public class SingleOverlordRanking
{
    public uint Rank;
    public string Nick;
    public uint Hurt;

    public SingleOverlordRanking()
    {
        Init();
    }
    public void Init()
    {
        Rank = 0;
        Nick = "";
        Hurt = 0;
    }
    public void CopyTo(SingleOverlordRanking vData)
    {
        if (vData != null)
        {
            CopyTo(vData.Rank, vData.Nick, vData.Hurt);
        }
        else
        {
            Init();
        }
    }
    public void CopyTo(uint vRank, string vNick, uint vHurt)
    {
        Rank = vRank;
        Nick = vNick;
        Hurt = vHurt;
    }
}


/// <summary>
/// 全服霸主
/// </summary>
public class SupermacyModule : Singleton<SupermacyModule>
{

    public SupermacyNetWork _SupermacyNetWork;

    private ArenaPlayer supermacyPlayer;
    /// <summary>
    /// 霸主信息
    /// </summary>
    public ArenaPlayer SupermacyPlayer
    {
        get
        {
            if (supermacyPlayer == null)
                supermacyPlayer = new ArenaPlayer();
            return supermacyPlayer;
        }
    }

    /// <summary>
    /// 全服霸主名次信息
    /// </summary>
    public List<SingleOverlordRanking> ListRankInfo;
    /// <summary>
    /// 自己排名信息
    /// </summary>
    public SingleOverlordRanking SelfRankInfo;
    /// <summary>
    /// 排名獎勵信息-包括參與獎
    /// </summary>
    public List<uint> ListAwardInfo;
    /// <summary>
    /// 開啟界面獲取數據結果[0: 活動開啟 非零: 活動未開啟]
    /// </summary>
    public int StartResult = -1;
    /// <summary>
    /// 開始時間
    /// </summary>
    //public string OpenTime;
    /// <summary>
    /// 时间点，活动中=结束时间/活动外=下次开始时间
    /// </summary>
    public int ContTime;
    /// <summary>
    /// 最終血量比
    /// </summary>
    public int FinalHP;
    /// <summary>
    /// 所在军团排名
    /// </summary>
    public uint unionRank;
    /// <summary>
    /// Buff等級
    /// </summary>
    public int BUFFLevel;
    /// <summary>
    /// 銅錢購買Buff次數
    /// </summary>
    public int BUFFBuyTime_Gold;
    /// <summary>
    /// 元寶購買Buff次數
    /// </summary>
    public int BUFFBuyTime_Diamond;
    /// <summary>
    /// 購買BUFF貨幣類型
    /// </summary>
    public ECurrencyType BuyBuffCoinType;
    /// <summary>
    /// 挑戰CD
    /// </summary>
    public uint ChallengesCDTime;
    /// <summary>
    /// 自己英雄神器信息(显示用)
    /// </summary>
    public List<EquipList> equips;
    /// <summary>
    /// 己方武將信息(显示用)
    /// </summary>
    public List<SoldierList> soldiers;
    /// <summary>
    /// 朝拜數值[0-已朝拜 其他值-未朝拜]
    /// </summary>
    public int WorshipValue;
    /// <summary>
    /// 是否开启自动战斗
    /// </summary>
    public bool isAutoBattle;
    /// <summary>
    /// 是否需要刷新
    /// </summary>
    public bool needRefresh;
    /// <summary>
    /// 霸主实时血量[剩余百分比]
    /// </summary>
    public float HPValue
    {
        get
        {
            return (1.0f * (ContTime - Main.mTime) / ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().cont_time * (100 - FinalHP) + FinalHP) / 100f;
        }
    }

    public uint PetTypeID;
    //public long CurrentTime
    //{
    //    get
    //    {
    //        return (Main.mTime + 9 * 3600) % 86400;
    //    }
    //}

    public void Initialize()
    {

        if (_SupermacyNetWork == null)
        {
            _SupermacyNetWork = new SupermacyNetWork();
        }

        ObtainRankAwardInfo();

        _SupermacyNetWork.RegisterMsg();

        supermacyPlayer = new ArenaPlayer();


        //test----------------------------------------
        //StartResult = 0;
        //OpenTime = "17:00:00";
        //ContTime = 3000;
        //FinalHP = 25;
        //BUFFLevel = 0;
        //BUFFBuyTime_Gold = 0;
        //BUFFBuyTime_Diamond = 0;
        //BuyBuffCoinType = ECurrencyType.None;
        //ChallengesCDTime = 30;
        //WorshipValue = 1;
        //supermacyPlayer = new ArenaPlayer();
        //supermacyPlayer.hero.charname = "feasr";
        //supermacyPlayer.hero.gender = 1;
        //end-----------------------------------------
    }
    public void Uninitialize()
    {
        if (_SupermacyNetWork != null)
            _SupermacyNetWork.RemoveMsg();
        _SupermacyNetWork = null;
    }


    /// <summary>
    /// 活動是否開啟
    /// </summary>
    /// <returns></returns>
    public bool IsOpenActivity()
    {
        return (StartResult == 0);
    }
    /// <summary>
    /// 是否已經朝拜
    /// </summary>
    /// <returns></returns>
    public bool IsWorship()
    {
        return (WorshipValue == 0);
    }

    /// <summary>
    /// 初始化數據
    /// </summary>
    private void InitData()
    {
        if (ListRankInfo == null)
        {
            ListRankInfo = new List<SingleOverlordRanking>();
        }
        else
        {
            ListRankInfo.Clear();
        }
        if (SelfRankInfo == null)
        {
            SelfRankInfo = new SingleOverlordRanking();
        }
        else
        {
            SelfRankInfo.Init();
        }

        StartResult = -1;
        //OpenTime = "";
        ContTime = 0;
        FinalHP = 0;
        BUFFLevel = 0;
        BUFFBuyTime_Gold = 0;
        BUFFBuyTime_Diamond = 0;
        BuyBuffCoinType = ECurrencyType.None;
        ChallengesCDTime = 0;
        unionRank = 0;
        WorshipValue = -1;

    }

    /// <summary>
    /// 獲取排名獎勵信息
    /// </summary>
    private void ObtainRankAwardInfo()
    {
        if (ListAwardInfo == null)
        {
            ListAwardInfo = new List<uint>();
        }
        ListAwardInfo.Clear();
        if ((ConfigManager.Instance.mServerHegemonyData != null) && (ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo() != null))
        {
            foreach (KeyValuePair<int, uint> tmpInfo in ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().hegemony_award)
            {
                ListAwardInfo.Add(tmpInfo.Value);
            }
            ListAwardInfo.Add(ConfigManager.Instance.mServerHegemonyData.GetServerHegemonyInfo().min_award);
        }
    }

    /// <summary>
    /// 进入自动战斗
    /// </summary>
    public void EnterAutoBattle()
    {
        //Scheduler.Instance.RemoveTimer(EnterAutoBattle);
        UISystem.Instance.CloseAllUI();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        UISystem.Instance.HintView.ShowFightLoading(UISystem.Instance.FightView.ShowFightTitleInfo);
        Dictionary<ulong, int> mReadyEquip_dic = new Dictionary<ulong, int>();
        if (equips != null)
        {
            for (int i = 0; i < equips.Count; i++)
            {
                EquipList equipData = equips[i];
                if (equipData == null)
                    continue;
                mReadyEquip_dic.Add(equipData.uid, equipData.pos);
            }
        }

        Dictionary<ulong, int> mSoldiers_dic = new Dictionary<ulong, int>();
        if (soldiers != null)
        {
            for (int i = 0; i < soldiers.Count; i++)
            {
                SoldierList readySoldier = soldiers[i];
                if (readySoldier == null)
                {
                    continue;
                }
                Soldier soldier = PlayerData.Instance._SoldierDepot.FindByUid(readySoldier.uid);
                if (soldier == null)
                    continue;
                mSoldiers_dic.Add(readySoldier.uid, readySoldier.num);
            }
        }
        PetData petdata = null;
        if (PetTypeID != 0) 
        {
            petdata = PlayerData.Instance._PetDepot.GetPetDataByTypeID(PetTypeID);
        }
        UISystem.Instance.FightView.SetViewInfo(EFightType.eftServerHegemony, PlayerData.Instance._Attribute, mReadyEquip_dic, mSoldiers_dic, supermacyPlayer, petdata);
    }

    /// <summary>
    /// 进入全服霸主主界面主动向Server获取数据
    /// </summary>
    /// <param name="data"></param>
    public void SendEnterOverlord()
    {
        Debug.Log("SendEnterOverlord");
        EnterOverlordReq tmpData = new EnterOverlordReq();
        _SupermacyNetWork.SendEnterOverlord(tmpData);
    }
    public void ReceiveEnterOverlord(EnterOverlordResp vData)
    {
        Debug.Log("ReceiveEnterOverlord");
        // Debug.LogError(vData);
        InitData();
        if (vData != null)
        {
            if (vData.hurt_buff_Place != null)
            {
                SelfRankInfo.CopyTo(vData.hurt_buff_Place.place, PlayerData.Instance._NickName, (uint)vData.hurt_buff_Place.hurt);
            }
            if (vData.Overlord_Ranking != null)
            {
                foreach (fogs.proto.msg.Overlord_Rankinfo tmpInfo in vData.Overlord_Ranking)
                {
                    if (tmpInfo != null)
                    {
                        SingleOverlordRanking tmpData = new SingleOverlordRanking();
                        tmpData.CopyTo(tmpInfo.place, tmpInfo.name, tmpInfo.hurt);
                        ListRankInfo.Add(tmpData);
                    }
                }
            }

            StartResult = vData.result;
            //OpenTime = vData.boss_time;
            ContTime = vData.cont_time;
            FinalHP = (int)vData.min_hp;
            BUFFLevel = vData.hurt_buff_Place.buff;
            BUFFBuyTime_Gold = vData.hurt_buff_Place.buffbuynum.buygold;
            BUFFBuyTime_Diamond = vData.hurt_buff_Place.buffbuynum.buydiamond;
            ChallengesCDTime = vData.hurt_buff_Place.challengescd;
            unionRank = vData.unionplace;
            try
            {
                if (Main.mTime < vData.datatime)
                {
                    Main.mTime = vData.datatime;
                }
            }
            catch
            {
                Debug.LogError("get maintime fail");
            }

            //Debug.LogError(CommonFunction.GetTimeByLong(ChallengesCDTime) + "   " + CommonFunction.GetTimeByLong(Main.mTime));
            //Debug.LogError(FinalHP);
            //Debug.LogError(vData.result);
            equips = vData.hurt_buff_Place.equips;
            soldiers = vData.hurt_buff_Place.soldiers;
            if (equips == null)
                equips = new List<EquipList>();
            if (soldiers == null)
                soldiers = new List<SoldierList>();
            PetTypeID = vData.hurt_buff_Place.pettypeid;
            WorshipValue = vData.hurt_buff_Place.worship;
            supermacyPlayer = vData.overlord;
            if (UISystem.Instance.MainCityView.View != null)
                UISystem.Instance.MainCityView.View.Lbl_BazhuName.text = supermacyPlayer.hero.charname;
            needRefresh = false;

            ///////////////test//////////

        }
        else
        {
            Debug.LogError("All supermacyinfo is null");
        }
        if (UISystem.Instance.SupermacyView != null && UISystem.Instance.SupermacyView.view != null)
        {
            //Debug.Log("go to update supermacy UI");
            UISystem.Instance.SupermacyView.UpdateInfo();
        }

    }


    /// <summary>
    /// 重置挑战霸主CD
    /// </summary>
    public void SendClearSupermacyCD()
    {
        Debug.Log("send CD CLEARE");
        BuyCdReq data = new BuyCdReq();
        _SupermacyNetWork.SendClearSupermacyCD(data);
    }
    /// <summary>
    /// 重置挑战霸主CD
    /// </summary>
    public void ReceiveClearSupermacyCD(BuyCdResp data)
    {
        //Debug.Log(data.result);
        if (data == null) return;
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            Debug.Log("CD CLEARED");
            PlayerData.Instance.UpdateItem(data.Buyitemnum);
            //Debug.LogError(data.Buyitemnum.id + "         " + data.Buyitemnum.num);
            UISystem.Instance.SupermacyView.ClearCDTimeSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
            UISystem.Instance.SupermacyView.DisAutoClearCD();
        }

    }

    /// <summary>
    /// 购买BUFF
    /// </summary>
    public void SendBuyBuff(ECurrencyType vCoinType)
    {
        Debug.Log("SendBuyBuff");
        BuyBuffCoinType = vCoinType;
        BuyBuffReq tmpData = new BuyBuffReq();
        //tmpData.accid = PlayerData.Instance.GetAccountID();
        tmpData.buyType = (int)vCoinType;
        _SupermacyNetWork.SendBuyBuff(tmpData);
    }
    public void ReceiveBuyBuff(BuyBuffResp vData)
    {
        Debug.Log("ReceiveBuyBuff");
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                BUFFLevel = (int)vData.buffnum;
                if (vData.buffbuyNum != null)
                {
                    BUFFBuyTime_Gold = vData.buffbuyNum.buygold;
                    BUFFBuyTime_Diamond = vData.buffbuyNum.buydiamond;
                }
                UISystem.Instance.SupermacyView.BuyBuffOperate(vData);
            }
            else
            {

                ErrorCode.ShowErrorTip(vData.result);
            }
        }
    }

    /// <summary>
    /// 领取朝拜礼包
    /// </summary>
    public void SendWorship()
    {
        Debug.Log("SendWorship");
        _SupermacyNetWork.SendWorship(new WorshipReq());
    }
    public void ReceiveWorship(WorshipResp vData)
    {
        Debug.Log("ReceiveWorship");
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                WorshipValue = 0;
                UISystem.Instance.MainCityView.View.BaZhuNotice.gameObject.SetActive(false);
            }
            else
            {
                ErrorCode.ShowErrorTip(vData.result);
            }
            UISystem.Instance.SupermacyView.WorshipOperate(vData);
        }
        else
        {
            ErrorCode.ShowErrorTip(vData.result);
        }
    }
    /// <summary>
    /// 主界面挑战霸主
    /// </summary>
    public void SendChallenge()
    {
        Debug.Log("SendChallenge");
        UISystem.Instance.HintView.SetLoadingVisible(true);
        _SupermacyNetWork.SendChallenge(new ChallengeOverlordReq());
    }
    public void ReceiveChallenge(ChallengeOverlordResp vdata)
    {
        Debug.Log("receive challenge");
        if (vdata == null) return;
        if (vdata.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.HintView.SetLoadingVisible(false);
            if (isAutoBattle)
            {
                EnterAutoBattle();
            }
            else
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SERVERHEGEMONYINFO);
                UISystem.Instance.ServerHegemonyInfoView.UpdateViewInfo(supermacyPlayer, ConstString.SUPERMACY_MEMBER);
            }

        }
        else
        {
            Debug.LogError(vdata.result);
            ErrorCode.ShowErrorTip(vdata.result);
            UISystem.Instance.SupermacyView.DisAutoBattle();
        }

    }

    public void SendStationReady(List<SoldierList> soldiers, List<EquipList> equips,uint petTypeID)
    {
        Debug.Log("SendStationReady");
        StationReadyReq data = new StationReadyReq();
        data.soldiers.Clear();
        data.soldiers.AddRange(soldiers);
        data.equips.Clear();
        data.equips.AddRange(equips);
        data.pettypeid = petTypeID;
        _SupermacyNetWork.SendStationReady(data);
    }

    public void ReceiveStationReady(StationReadyResp data)
    {
        Debug.Log("ReceiveStationReady");
        if (data == null) return;
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.PrepareBattleView.OnStartSeverHegemony(data.attribute);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void ShowSupermacyName(bool isStart)
    {
        if (UISystem.Instance.UIIsOpen(SupermacyView.UIName)) return;
        if (!isStart)
        {
            UISystem.Instance.MainCityView.View.Gobj_BazhuName.gameObject.SetActive(true);
            SendEnterOverlord();
            Debug.Log("send for get supermacy's name when update");
        }
        else if (isStart && PlayerData.Instance._Level >= 15)
        {
            UISystem.Instance.MainCityView.View.Gobj_BazhuName.gameObject.SetActive(true);
            SendEnterOverlord();
            Debug.Log("send for get supermacy's name when start");
        }
        else
        {
            UISystem.Instance.MainCityView.View.Gobj_BazhuName.gameObject.SetActive(false);
        }
    }

    public void GetOriginalSoldiersAndEquips(List<SoldierList> _soldiers, List<EquipList> _equips)
    {
        if (equips == null)
        {
            equips = new List<EquipList>();
        }
        else
        {
            equips.Clear();
        }
        if (soldiers == null)
        {
            soldiers = new List<SoldierList>();
        }
        else
        {
            soldiers.Clear();
        }
        foreach (EquipList tmpInfo in _equips)
        {
            if (tmpInfo != null)
            {
                equips.Add(tmpInfo);
            }
        }
        foreach (SoldierList tmpInfo in _soldiers)
        {
            if (tmpInfo != null && PlayerData.Instance._SoldierDepot.FindByUid(tmpInfo.uid) != null)
            {
                soldiers.Add(tmpInfo);
            }
        }
    }
}