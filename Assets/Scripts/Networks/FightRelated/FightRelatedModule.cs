using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using Assets.Script.Common.StateMachine;

public class FightRelatedModule : Singleton<FightRelatedModule>
{
    public int maxTimes = 0;

    public FightRelatedNetWork _FightRelatedNetWork;
    /// <summary>
    /// 本地端无尽数据
    /// </summary>
    private EndlessDungeonRewardResp localEndlessData;
    public List<EndlessDungeonRewardResp> listEndlessData;

    private EFightType dungeonType;
    /*  是否进入下一关  
     *  下一关功能仅会在主线副本功能中出现
     *  下一关要进行特殊处理，无论从获取途径或章节界面进入，关卡信息界面最终都会返回章节界面
     *  add by taiwei  2015-09-09 16:52
     */
    public bool isOpenNext = false;
    public bool isSweep = false;
    public delegate void MopupDungeonDelegate(MopupDungeonResp resp);
    public event MopupDungeonDelegate MopupDungeonEvent;
    /// <summary>
    /// 己方英雄臨時數據
    /// </summary>
    public ShowInfoHero SelfHeroInfo;
    /// <summary>
    /// 全服爭霸傷害值
    /// </summary>
    public int ServerHegemonyHurtValue = 0;

    public bool isFightState = false;
    /// <summary>
    /// 特殊新手引导关卡
    /// </summary>
    public Dictionary<uint, uint> DicSpeTutorial;
    /// <summary>
    /// 特殊结算关卡
    /// </summary>
    public Dictionary<uint, uint> DicSpeFightResult;


    /// <summary>
    /// 玩家城堡信息
    /// </summary>
    public PlayerCastleInfo mCastleInfo;
    /// <summary>
    /// 射手信息列表
    /// </summary>
    public List<SingleShooterInfo> mShooterList;


    public void Initialize()
    {
        if (_FightRelatedNetWork == null)
        {
            _FightRelatedNetWork = new FightRelatedNetWork();
        }
        _FightRelatedNetWork.RegisterMsg();
        ObtainSpecialStageID();
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RecordServerHegemonyHurt, CommandEvent_RecordServerHegemonyHurt);
    }

    public void Uninitialize()
    {
        if (_FightRelatedNetWork != null)
            _FightRelatedNetWork.RemoveMsg();
        _FightRelatedNetWork = null;
        isFightState = false;
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RecordServerHegemonyHurt, CommandEvent_RecordServerHegemonyHurt);
    }
    #region 私有防范
    /// <summary>
    /// 获取所有特殊处理关卡ID
    /// </summary>
    private void ObtainSpecialStageID()
    {
        string tmpTutorial = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_TUTORIAL);
        string tmpFightResult = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_FIGHTRESULTID);

        if (DicSpeTutorial == null)
            DicSpeTutorial = new Dictionary<uint, uint>();
        DicSpeTutorial.Clear();
        if (!string.IsNullOrEmpty(tmpTutorial))
        {
            string[] tmpArrInfo = tmpTutorial.Split(';');
            for (int i = 0; i < tmpArrInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(tmpArrInfo[i]))
                    continue;
                string[] tmpArrData = tmpArrInfo[i].Split(',');
                if (tmpArrData.Length < 2)
                    continue;
                uint tmpKey, tmpValue;
                if (!uint.TryParse(tmpArrData[0], out tmpKey))
                    tmpKey = 0;
                if (!uint.TryParse(tmpArrData[1], out tmpValue))
                    tmpValue = 0;
                if (!DicSpeTutorial.ContainsKey(tmpKey))
                    DicSpeTutorial.Add(tmpKey, tmpValue);
            }
        }
        if (DicSpeFightResult == null)
            DicSpeFightResult = new Dictionary<uint, uint>();
        DicSpeFightResult.Clear();
        if (!string.IsNullOrEmpty(tmpFightResult))
        {
            string[] tmpArrInfo = tmpFightResult.Split(';');
            for (int i = 0; i < tmpArrInfo.Length; i++)
            {
                if (string.IsNullOrEmpty(tmpArrInfo[i]))
                    continue;
                string[] tmpArrData = tmpArrInfo[i].Split(',');
                if (tmpArrData.Length < 2)
                    continue;
                uint tmpKey, tmpValue;
                if (!uint.TryParse(tmpArrData[0], out tmpKey))
                    tmpKey = 0;
                if (!uint.TryParse(tmpArrData[1], out tmpValue))
                    tmpValue = 0;
                if (!DicSpeFightResult.ContainsKey(tmpKey))
                    DicSpeFightResult.Add(tmpKey, tmpValue);
            }
        }
    }
    #endregion

    /// <summary>
    /// 記錄全服霸主傷害值
    /// </summary>
    /// <param name="vData"></param>
    private void CommandEvent_RecordServerHegemonyHurt(object vData)
    {
        if (vData != null)
        {
            ServerHegemonyHurtValue += (int)vData;
        }
    }

    /// <summary>
    /// 匹配敌方数据(不需要消耗任何东西) -发送
    /// </summary>
    /// <param name="data"></param>
    public void SendMatchEnemy(int type)
    {
        MatchEnemyReq data = new MatchEnemyReq();
        data.type = type;
        _FightRelatedNetWork.SendMatchEnemy(data);
    }

    /// <summary>
    /// 匹配敌方数据-接收
    /// </summary>
    /// <param name="data"></param>
    //public void ReceiveMatchEnemy(MatchEnemyResp data)
    //{
    //    if (data.result == (int)ErrorCodeEnum.SUCCESS)
    //    {
    //        if (data.expdt_info != null)
    //        {
    //            PlayerData.Instance.UpdateExpeditionInfo(data.expdt_info);

    //            //設置城堡數據//
    //            if (mCastleInfo == null)
    //                mCastleInfo = new PlayerCastleInfo();
    //            else
    //                mCastleInfo.InitInfo();
    //            mCastleInfo.ResetInfo(data.expdt_info.Ecastle_info.castle);

    //            //設置射手數據//
    //            if (mShooterList == null)
    //                mShooterList = new List<SingleShooterInfo>();
    //            else
    //                mShooterList.Clear();
    //            if (data.expdt_info.Ecastle_info.shooters != null)
    //            {
    //                for (int i = 0; i < data.expdt_info.Ecastle_info.shooters.Count; i++)
    //                {
    //                    SingleShooterInfo tmpInfo = new SingleShooterInfo();
    //                    tmpInfo.ResetInfo(data.expdt_info.Ecastle_info.shooters[i]);
    //                    mShooterList.Add(tmpInfo);
    //                }
    //                mShooterList.Sort((left, right) =>
    //                {
    //                    int leftNum = (int)left.mID % 10;
    //                    int rightNum = (int)right.mID % 10;
    //                    if (leftNum != rightNum)
    //                    {
    //                        if (leftNum < rightNum)
    //                            return 1;
    //                        else
    //                            return -1;
    //                    }
    //                    return 0;
    //                });
    //            }
    //        }
    //        else
    //        {
    //            Debug.LogError("server send data error ");
    //        }
    //        maxTimes = data.max_times;
    //        if (data.type == (int)MatchType.None)
    //        {
    //            //UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
    //            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXPEDITION);
    //        }
    //        else
    //        {
    //            UISystem.Instance.ExpeditionView.RefreshStatus();
    //        }
    //    }
    //    else if (data.result == (int)ErrorCodeEnum.PassAllExpedition)
    //    {
    //        //UISystem.Instance.ExpeditionView.ReStartOperate();
    //    }
    //    else
    //    {
    //        ErrorCode.ShowErrorTip(data.result);
    //    }
    //}
    public void ReceiveMatchEnemy(MatchEnemyResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                maxTimes = data.max_times;
                if (mCastleInfo == null)
                    mCastleInfo = new PlayerCastleInfo();
                else
                    mCastleInfo.InitInfo();
                if (mShooterList == null)
                    mShooterList = new List<SingleShooterInfo>();
                else
                    mShooterList.Clear();
                if (data.expdt_info != null)
                {
                    PlayerData.Instance.UpdateExpeditionInfo(data.expdt_info);
                    if (data.expdt_info.Ecastle_info != null)
                    {
                        //設置城堡數據//
                        if (data.expdt_info.Ecastle_info.castle != null)
                        {
                            mCastleInfo.ResetInfo(data.expdt_info.Ecastle_info.castle);
                        }

                        //設置射手數據//
                        if (data.expdt_info.Ecastle_info.shooters != null)
                        {
                            for (int i = 0; i < data.expdt_info.Ecastle_info.shooters.Count; i++)
                            {
                                SingleShooterInfo tmpInfo = new SingleShooterInfo();
                                tmpInfo.ResetInfo(data.expdt_info.Ecastle_info.shooters[i]);
                                mShooterList.Add(tmpInfo);
                            }
                            //排序//
                            mShooterList.Sort((left, right) =>
                            {
                                int leftNum = (int)left.mID % 10;
                                int rightNum = (int)right.mID % 10;
                                if (leftNum != rightNum)
                                {
                                    if (leftNum < rightNum)
                                        return 1;
                                    else
                                        return -1;
                                }
                                return 0;
                            });
                        }
                    }
                }
                else
                {
                    Debug.LogError("Expedition Res Error: ecastle is null");
                    return;
                }
                if (data.type == (int)MatchType.None)
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXPEDITION);
                }
                else
                {
                    UISystem.Instance.ExpeditionView.RefreshStatus(true);
                }
            }
            else if (data.result == (int)ErrorCodeEnum.PassAllExpedition)
            {
                //UISystem.Instance.ExpeditionView.ReStartOperate();
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
    }

    /// <summary>
    ///  远征奖励 - 发送 
    /// </summary>
    /// <param name="data"></param>
    public void SendExpeditionReward(uint gateID)
    {
        ExpeditionRewardReq data = new ExpeditionRewardReq();
        data.gate_id = gateID;
        _FightRelatedNetWork.SendExpeditionReward(data);
    }
    public ExpeditionRewardResp ExpeditionChestsData;
    /// <summary>
    ///  远征奖励 -接收
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveExpeditionReward(ExpeditionRewardResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            ExpeditionChestsData = data;
            //PlayerData.Instance._Level = data.playerLevel;

            PlayerData.Instance.RefreshSingleExpeditionStatus(data.gate_id, ExpeditionInfo.GateStatus.PASSED_REWARED);

            PlayerData.Instance.UpdateDropData(data.drop_item);
            // UISystem.Instance.ExpeditionView.GetAwardsSuccess(data.drop_item);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_OPENCHESTSEFFECT);
            UISystem.Instance.OpenChestsEffect.isLiveness = true;
        }
        else
        {

        }
    }

    ///// <summary>
    ///// 章节开放—发送
    ///// </summary>
    //public void SendChapterOpen(uint chapterID)
    //{
    //    ChapterOpenTimeReq data = new ChapterOpenTimeReq();
    //    data.chapterID = chapterID;
    //    _FightRelatedNetWork.SendChapterOpen(data);
    //}
    ///// <summary>
    ///// 章节开放 - 接收
    ///// </summary>
    //public void ReceiveChapterOpen(ChapterOpenTimeResp data)
    //{
    //    if (data.result == 0)
    //    {
    //        UISystem.Instance.EndlessView.EnterGateInfo();
    //    }
    //    else
    //    {
    //        Debug.LogError("error result!!" + data.result);
    //    }
    //}


    ///// <summary>
    ///// 重新开始战斗-发送
    ///// </summary>
    ///// <param name="vGateID">战斗关卡ID</param>
    //public void SendMajorMapRestart(uint vGateID)
    //{
    //    //Debug.LogWarning("FightRelatedModule: SendMajorMapRestart");
    //    MajorMapRestartReq tmpData = new MajorMapRestartReq();
    //    tmpData.gateID = vGateID;
    //    _FightRelatedNetWork.SendMajorMapRestart(tmpData);
    //}
    ///// <summary>
    ///// 重新开始战斗-接收
    ///// </summary>
    ///// <param name="vData"></param>
    //public void ReceiveMajorMapRestart(MajorMapRestartResp vData)
    //{
    //    //Debug.LogWarning("FightRelatedModule: ReceiveMajorMapRestart");
    //    if (vData == null)
    //        return;
    //    PlayerData.Instance.UpdatePlayerSP((int)vData.phsicalPower);
    //    if (vData.result == 0)
    //    {
    //        UISystem.Instance.FightView.ReSetUIStatus();
    //    }
    //    else
    //    { }
    //}


    private bool isReStartExpedition;
    public void SendStartExpedition(List<SoldierList> list, bool vIsReStart = false)
    {
        isReStartExpedition = vIsReStart;
        Debug.Log("SendStartExpedition");
        if (list != null)
        {
            StartExpeditionReq data = new StartExpeditionReq();
            data.army.AddRange(list);
            _FightRelatedNetWork.SendStartExpedition(data);
        }
    }


    public void ReceiveStartExpedition(StartExpeditionResp data)
    {
        Debug.Log("ReceiveStartExpedition");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (!isReStartExpedition)
                UISystem.Instance.ExpeditionInfoView.EnterTheBattle();
            else
                UISystem.Instance.FightView.ReSetUIStatus(false);
        }
        else
        {
            isFightState = false;
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 远征结算
    /// </summary>
    /// <param name="result">1表示成功, 2表示失败</param>
    /// <param name="own">死亡士兵</param>
    /// <param name="enemy">剩余敌人</param>
    public void SendExpeditionResult(int result, List<SoldierList> own, fogs.proto.msg.ArenaPlayer enemy, int vCastleHP)
    {
        Debug.LogWarning(string.Format("SendExpeditionResult: [{0}]", result));
        ExpeditionResultReq data = new ExpeditionResultReq();
        data.exp_res = result;
        data.dead_soldiers.AddRange(own);
        data.remain_enemies = enemy;
        data.surplus_hp = (uint)vCastleHP;
        _FightRelatedNetWork.SendExpeditionResult(data);
    }
    /// <summary>
    /// 远征结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveExpeditionResult(ExpeditionResultResp data)
    {
        Debug.Log("ReceiveExpeditionResult");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else if (data.result == (int)ErrorCodeEnum.PlayerNotInFight)
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXPEDITION_NOTINFIGHT);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.EXPEDITION_NOTINFIGHT, UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    //================New =======================//

    /// <summary>
    /// 进入PVE
    /// </summary>
    public void SendDungeonInfo(DungeonType type)
    {
        Debug.Log("SendDungeonInfo");
        DungeonInfoReq data = new DungeonInfoReq();
        data.type = type;
        _FightRelatedNetWork.SendDungeonInfo(data);
    }
    /// <summary>
    /// 进入PVE
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonInfo(DungeonInfoResp data)
    {
        Debug.Log("ReceiveDungeonInfo");
        switch (data.type)
        {
            case DungeonType.DGT_ENDLESS:
                {
                    UISystem.Instance.EndlessView.ReceiveDungeonInfo(data.endless_info);
                } break;
            case DungeonType.DGT_ACTIVITY:
                {
                    UISystem.Instance.ActivitiesView.ReceiveDungeonInfo(data.activity_info);
                } break;
            case DungeonType.DGT_MAJOR:
                {
                    PlayerData.Instance._MajorDungeonInfoList = data.major_info;
                    PlayerData.Instance._TodayPlayDungeons = data.today_elite_dgn;
                    PlayerData.Instance._MajorDungeonSoldierList = data.load_soldiers;
                    UISystem.Instance.GateView.ReceiveDungeonInfo();
                } break;
        }
    }

    /// <summary>
    /// 开始PVE
    /// </summary>
    private bool isReStartPVE;
    public void SendDungeonStart(uint gateID, List<ulong> soldiers, bool vIsReStart = false)
    {
        isReStartPVE = vIsReStart;
        Debug.Log("SendDungeonStart");
        DungeonStartReq data = new DungeonStartReq();
        data.dgn_id = gateID;
        data.soldiers.Clear();
        data.soldiers.AddRange(soldiers);
        _FightRelatedNetWork.SendDungeonStart(data);
    }
    /// <summary>
    /// 开始PVE
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonStart(DungeonStartResp data)
    {
        Debug.Log("ReceiveDungeonStart");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (!isReStartPVE)
                UISystem.Instance.GateInfoView.EnterTheBattle();
            else
                UISystem.Instance.FightView.ReSetUIStatus(false);
        }
        else if (data.result == (int)ErrorCodeEnum.EnterTimesLimited)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_GATE_PREGATESTARNOTENOUGH);
        }
        else if (data.result == (int)ErrorCodeEnum.PrevDungeonStarLevelNotEnough)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_GATE_PREGATESTARNOTENOUGH);
        }
        else if (data.result == (int)ErrorCodeEnum.HeroLevelNotEngouh)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERR_GATE_HEROLEVELNOTENGOUGH);
        }
        else if (data.result == (int)ErrorCodeEnum.OpenTimeLimited)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.ACTIVITIES_LOCKTIME, () =>
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ACTIVITIES);
            });
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
    /// PVE 结算 
    /// </summary>
    public void SendDungeonReward(int result, uint gateID, int reward)
    {
        Debug.Log("SendDungeonReward: " + result + "|" + gateID + "|" + reward);
        DungeonRewardReq data = new DungeonRewardReq();
        data.fight_result = result;
        data.dgn_id = gateID;
        data.reward_arg = reward;
        _FightRelatedNetWork.SendDungeonReward(data);
    }
    /// <summary>
    /// PVE 结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonReward(DungeonRewardResp data)
    {
        Debug.Log("ReceiveDungeonReward");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
                if (UISystem.Instance.MainCityView != null)
                    UISystem.Instance.MainCityView.ShowMainGateHint(PlayerData.Instance.IsShowGateTip());
            }
            else
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }

    /// <summary>
    /// 扫荡
    /// </summary>
    public void SendMopupDungeon(DungeonType type, uint gateID, int count)
    {
        Debug.Log("SendMopupDungeon");
        isSweep = true;
        MopupDungeonReq data = new MopupDungeonReq();
        data.type = type;
        data.dgn_id = gateID;
        data.times = count;
        _FightRelatedNetWork.SendMopupDungeon(data);
    }
    /// <summary>
    /// 扫荡
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveMopupDungeon(MopupDungeonResp data)
    {
        Debug.Log("ReceiveMopupDungeon");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            for (int i = 0; i < data.drop_items.Count; i++)
            {
                PlayerData.Instance.UpdateDropData(data.drop_items[i]);
                //UISystem.Instance.GateInfoView.ReceiveSweep(data.drop_items[i]);
            }
            PlayerData.Instance.UpdateSP(data.ph_power);
            PlayerData.Instance.UpdateItem(data.mopup_item);
            switch (data.type)
            {
                case DungeonType.DGT_MAJOR:
                    {
                        PlayerData.Instance._TodayPlayDungeons = data.today_elite_dgn;
                    } break;
                case DungeonType.DGT_ACTIVITY:
                    {
                        if (PlayerData.Instance._ActivityDungeonList != null && data.activity_info != null)
                        {
                            ActivityDungeonInfo info = PlayerData.Instance._ActivityDungeonList.Find((tmp) =>
                            {
                                if (tmp == null) return false;
                                return tmp.chapter_id == data.activity_info.chapter_id;
                            });
                            if (info != null)
                            {
                                //info = data.activity_info;
                                info.chapter_id = data.activity_info.chapter_id;
                                info.max_times = data.activity_info.max_times;
                                info.today_times = data.activity_info.today_times;
                                // info.today_buy_times = data.activity_info.today_buy_times;
                            }
                        }
                    } break;
                case DungeonType.DGT_ENDLESS:
                    {
                        if (PlayerData.Instance._EndlessDungeonList != null && data.endless_info != null)
                        {
                            EndlessDungeonInfo info = PlayerData.Instance._EndlessDungeonList.Find((tmp) =>
                            {
                                if (tmp == null) return false;
                                return tmp.chapter_id == data.endless_info.chapter_id;
                            });
                            if (info != null)
                            {
                                info.chapter_id = data.endless_info.chapter_id;
                                info.max_times = data.endless_info.max_times;
                                info.today_times = data.endless_info.today_times;
                            }
                        }
                    } break;
            }
            UISystem.Instance.GateInfoView.SweepSuccess(data);
            if (MopupDungeonEvent != null)
                MopupDungeonEvent(data);
        }
        else if (data.result == (int)ErrorCodeEnum.PrevDungeonNotPass)
        {
            isSweep = false;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_PREGATEUNLOCK);
        }
        else
        {
            isSweep = false;
            ErrorCode.ShowErrorTip(data.result);
            //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result));
        }
    }

    /// <summary>
    /// 无尽模式结算
    /// </summary>
    public void SendEndlessDungeonReward(int result, uint gateID, int enemy_soldier_num, int enemy_boss_num, int enemy_castle_num, int my_soldier_num, int my_hero_num, int my_castle_num, int use_time = 0)
    {
        Debug.Log("SendEndlessDungeonReward");
        EndlessDungeonRewardReq data = new EndlessDungeonRewardReq();
        data.fight_result = result;
        data.dgn_id = gateID;
        data.enemy_soldier_num = enemy_soldier_num;
        data.enemy_boss_num = enemy_boss_num;
        data.enemy_castle_num = enemy_castle_num;
        data.my_soldier_num = my_soldier_num;
        data.my_hero_num = my_hero_num;
        data.my_castle_num = my_castle_num;
        data.use_time = use_time;
        _FightRelatedNetWork.SendEndlessDungeonReward(data);
    }
    /// <summary>
    /// 无尽模式结算
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveEndlessDungeonReward(EndlessDungeonRewardResp data)
    {
        Debug.Log("ReceiveEndlessDungeonReward");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                //记录累加数据//
                AddEndlessResultData(data);
                //更新数据//
                //PlayerData.Instance.UpdateBattleSettle(EFightType.eftEndless, data.info.chapter_id, 0, data.drop_items, data.exp, data.ph_power, null);
                //判断是否需要结算[0-未结束 1-结束]//
                if (data.game_over == 0)
                {
                    UISystem.Instance.FightView.EndlessExcessiveOperate();
                }
                else
                {
                    UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(localEndlessData);
                    //更新数据//
                    PlayerData.Instance.UpdateBattleSettle(EFightType.eftEndless, localEndlessData.info.chapter_id, 0, localEndlessData.drop_items, localEndlessData.exp, localEndlessData.ph_power, null);
                }
            }
            else
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ErrorCode.GetString(data.result));
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    public void SendDungeonStarReward(uint chapterID, uint type, int star)
    {
        Debug.Log("SendDungeonStarReward");
        DungeonStarRewardReq data = new DungeonStarRewardReq();
        data.chapter_id = chapterID;
        data.fight_mode = type;
        data.star = star;
        _FightRelatedNetWork.SendDungeonStarReward(data);
    }
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveDungeonStarReward(DungeonStarRewardResp data)
    {
        Debug.Log("ReceiveDungeonStarReward");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (PlayerData.Instance._GetAwardChapters == null)
            {
                PlayerData.Instance._GetAwardChapters = new List<DungeonStar>();
            }
            PlayerData.Instance.UpdateDropData(data.drop_items);
            fogs.proto.msg.DungeonStar hadAward = PlayerData.Instance._GetAwardChapters.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                if (tmp.chapter_id.Equals(data.chapter_id) && tmp.dgn_type == (int)data.fight_mode)
                {
                    return true;
                }
                return false;
            });
            if (hadAward == null)
            {
                fogs.proto.msg.DungeonStar award = new DungeonStar();
                award.chapter_id = data.chapter_id;
                award.dgn_type = (int)data.fight_mode;
                award.stars.Clear();
                award.stars.Add(data.star);
                PlayerData.Instance._GetAwardChapters.Add(award);
            }
            else
            {
                //if (hadAward.dgn_type == data.fight_mode)
                //{
                if (!hadAward.stars.Contains(data.star))
                {
                    hadAward.stars.Add(data.star);
                }
                //}

            }
            UISystem.Instance.GateView.ReceiveDungeonStarReward(data.drop_items);
        }
        else if (data.result == (int)ErrorCodeEnum.GetRewardAready)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_HADGETAWARDS);
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }
    /// <summary>
    /// 购买次数
    /// </summary>
    public void SendBuyDungeonTimes(uint gateID)
    {
        Debug.Log("SendBuyDungeonTimes");
        BuyDungeonTimesReq data = new BuyDungeonTimesReq();
        data.dgn_id = gateID;
        _FightRelatedNetWork.SendBuyDungeonTimes(data);
    }
    /// <summary>
    /// 购买次数
    /// </summary>
    /// <param name="data"></param>
    public void ReceiveBuyDungeonTimes(BuyDungeonTimesResp data)
    {
        Debug.Log("ReceiveBuyDungeonTimes");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance._TodayPlayDungeons = data.today_elite_dgn;
            PlayerData.Instance.UpdateDiamond(data.diamond);
            UISystem.Instance.GateInfoView.ReceiveBuyDungeonTimes();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    ///非主线副本类型  购买挑战次数
    /// </summary>
    /// <param name="data"></param>
    public void SendBuyOtherDungeonTimes(OtherDungeonType type, uint chapterID)
    {
        BuyOtherDungeonTimesReq data = new BuyOtherDungeonTimesReq();
        data.chapter_id = chapterID;
        data.dgn_type = type;
        _FightRelatedNetWork.SendBuyOtherDungeonTimes(data);
    }

    public void ReceiveBuyOtherDungeonTimes(BuyOtherDungeonTimesResp data)
    {
        Debug.Log("ReceiveBuyOtherDungeonTimes");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateDiamond(data.diamond);
            switch (data.dgn_type)
            {
                case OtherDungeonType.ACTIVITY_DGN:
                    {
                        ActivityDungeonInfo info = PlayerData.Instance._ActivityDungeonList.Find((tmp) =>
                        {
                            if (tmp == null)
                                return false;
                            return tmp.chapter_id == data.chapter_id;
                        });
                        if (info != null)
                        {
                            info.max_times = data.buy_times;
                            info.today_times = (int)data.today_times;
                            info.today_buy_times = data.today_buy_times;
                        }
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_ACTIVITIES))
                        {
                            UISystem.Instance.ActivitiesView.ReceiveBuyDungeonInfo(data);
                        }
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_GATEINFO))
                        {
                            UISystem.Instance.GateInfoView.ReceiveBuyOtherDungeonTimes();
                        }
                    } break;
                case OtherDungeonType.ENDLESS_DGN:
                    {
                        EndlessDungeonInfo info = PlayerData.Instance._EndlessDungeonList.Find((tmp) =>
                        {
                            if (tmp == null)
                                return false;
                            return tmp.chapter_id == data.chapter_id;
                        });
                        if (info != null)
                        {
                            info.max_times = data.buy_times;
                            info.today_times = (int)data.today_times;
                            info.today_buy_times = data.today_buy_times;
                        }
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_ENDLESS))
                        {
                            if (data.chapter_id == GlobalCoefficient.NormalEndless)
                                UISystem.Instance.EndlessView.UpdateNormalInfo(info);
                            if (data.chapter_id == GlobalCoefficient.EliteEndless)
                                UISystem.Instance.EndlessView.UpdateEliteInfo(info);
                            if (data.chapter_id == GlobalCoefficient.HeroEndless)
                                UISystem.Instance.EndlessView.UpdateHeroInfo(info);
                        }
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_GATEINFO))
                        {
                            UISystem.Instance.GateInfoView.ReceiveBuyOtherDungeonTimes();
                        }
                    }
                    break;
                case OtherDungeonType.EXOTICADVANTURE:
                    {
                        UnionModule.Instance.CharUnionInfo.today_pve_times = (int)data.today_times;
                        UnionModule.Instance.CharUnionInfo.today_buy_times = (int)data.today_buy_times;
                        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW))
                        {
                            UISystem.Instance.ExoticAdvantureInfoView.UpdateSurplusInfo();
                        }
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_BUYDUNGEONCOUNTSUCCESS);
                    }
                    break;
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 清空本地端无尽结算数据
    /// </summary>
    private void ClearLocalEndlessData()
    {
        localEndlessData.cur_grade = 0;
        localEndlessData.cur_victory = 0;
        localEndlessData.ph_power = 0;
        localEndlessData.result = 0;
        localEndlessData.fight_result = 0;
        localEndlessData.game_over = 0;

        if (localEndlessData.exp == null)
        {
            localEndlessData.exp = new AddExp();
            localEndlessData.exp.attr = new Attribute();
        }
        localEndlessData.exp.add_exp = 0;
        localEndlessData.exp.exp = 0;
        localEndlessData.exp.level = 0;
        localEndlessData.exp.attr.acc_rate = 0;
        localEndlessData.exp.attr.atk_interval = 0;
        localEndlessData.exp.attr.atk_space = 0;
        localEndlessData.exp.attr.combat_power = 0;
        localEndlessData.exp.attr.crt_rate = 0;
        localEndlessData.exp.attr.ddg_rate = 0;
        localEndlessData.exp.attr.energy_max = 0;
        localEndlessData.exp.attr.energy_revert = 0;
        localEndlessData.exp.attr.hp_max = 0;
        localEndlessData.exp.attr.hp_revert = 0;
        localEndlessData.exp.attr.leader = 0;
        localEndlessData.exp.attr.mp_max = 0;
        localEndlessData.exp.attr.mp_revert = 0;
        localEndlessData.exp.attr.phy_atk = 0;
        localEndlessData.exp.attr.speed = 0;
        localEndlessData.exp.attr.tnc_rate = 0;
        if (localEndlessData.exp.skills != null)
            localEndlessData.exp.skills.Clear();
        localEndlessData.exp.ph_power = 0;
        localEndlessData.exp.ph_power_max = 0;
        if (localEndlessData.exp.equip_list != null)
            localEndlessData.exp.equip_list.Clear();

        if (localEndlessData.info == null)
            localEndlessData.info = new EndlessDungeonInfo();
        localEndlessData.info.chapter_id = 0;
        localEndlessData.info.fight_tick = 0;
        localEndlessData.info.high_grade = 0;
        localEndlessData.info.high_victory = 0;
        localEndlessData.info.rank = 0;
        localEndlessData.info.today_times = 0;
        localEndlessData.info.use_time = 0;
        localEndlessData.info.passed = 0;
        localEndlessData.info.max_times = 0;

        if (localEndlessData.drop_items == null)
            localEndlessData.drop_items = new DropList();
        if (localEndlessData.drop_items.equip_list != null)
            localEndlessData.drop_items.equip_list.Clear();
        if (localEndlessData.drop_items.item_list != null)
            localEndlessData.drop_items.item_list.Clear();
        if (localEndlessData.drop_items.mail_list != null)
            localEndlessData.drop_items.mail_list.Clear();
        if (localEndlessData.drop_items.soldier_list != null)
            localEndlessData.drop_items.soldier_list.Clear();
        if (localEndlessData.drop_items.special_list != null)
            localEndlessData.drop_items.special_list.Clear();
        if (localEndlessData.drop_items.soul_list != null)
            localEndlessData.drop_items.soul_list.Clear();
    }

    /// <summary>
    /// 初始化本地端无尽数据
    /// </summary>
    public void InitLocalEndlessData()
    {
        if (localEndlessData == null)
            localEndlessData = new EndlessDungeonRewardResp();
        if (listEndlessData == null)
            listEndlessData = new List<EndlessDungeonRewardResp>();
        else
            listEndlessData.Clear();
        ClearLocalEndlessData();
    }

    /// <summary>
    /// 记录本地端无尽数据累加值
    /// </summary>
    /// <param name="data"></param>
    private void AddEndlessResultData(EndlessDungeonRewardResp data)
    {
        //Debug.LogError("====================================================");
        if (data != null)
        {
            listEndlessData.Add(data);

            #region [初始数据]
            localEndlessData.cur_grade = data.cur_grade;        //当前积分//
            localEndlessData.cur_victory = data.cur_victory;    //当前进度//
            localEndlessData.ph_power = data.ph_power;          //体力//
            localEndlessData.result = data.result;              //数据结果[0-成功/正确]//
            localEndlessData.fight_result = data.fight_result;  //战斗结果[1-战斗胜利]//
            localEndlessData.game_over = data.game_over;        //游戏是否结算[0-未结束 1-结束]//
            localEndlessData.cur_use_time = data.cur_use_time;  //当前战斗使用时间//
            #endregion

            #region exp [增加经验的结构体]
            int tmpIndex = 0;
            if (data.exp != null)
            {
                //Debug.LogError(string.Format("level: [{0}]", data.exp.level));
                #region addexp [增加经验]
                localEndlessData.exp.add_exp += data.exp.add_exp;
                //Debug.LogError(string.Format("addexp: [{0}, {1}]", localEndlessData.exp.add_exp, data.exp.add_exp));
                #endregion

                #region exp [最新经验值]
                localEndlessData.exp.exp = data.exp.exp;
                //Debug.LogError(string.Format("exp: [{0}, {1}]", localEndlessData.exp.exp, data.exp.exp));
                #endregion

                #region skills [英雄技能列表，如果等级变化，才会有值	]
                if (data.exp.skills != null)
                {
                    if (localEndlessData.exp.skills.Count <= 0)
                    {
                        for (int i = 0; i < data.exp.skills.Count; i++)
                        {
                            if (data.exp.skills[i] != null)
                            {
                                localEndlessData.exp.skills.Add(GetProtoSkill(data.exp.skills[i]));
                            }
                        }
                    }
                    else
                    {
                        List<fogs.proto.msg.Skill> tmpSkillList = new List<fogs.proto.msg.Skill>();
                        for (int i = 0; i < data.exp.skills.Count; i++)
                        {
                            tmpIndex = 0;
                            for (int j = 0; j < localEndlessData.exp.skills.Count; j++)
                            {
                                if (localEndlessData.exp.skills[j].id == data.exp.skills[i].id)
                                {
                                    localEndlessData.exp.skills[j].level = data.exp.skills[i].level;
                                    break;
                                }
                                tmpIndex = j;
                            }
                            if ((tmpIndex != 0) && (tmpIndex == localEndlessData.exp.skills.Count - 1))
                            {
                                if (data.exp.skills[i] != null)
                                {
                                    tmpSkillList.Add(GetProtoSkill(data.exp.skills[i]));
                                }
                            }

                        }
                        localEndlessData.exp.skills.AddRange(tmpSkillList);
                    }
                }
                #endregion

                #region attr [最新属性，如果等级变化，才会有值]
                //if ((localEndlessData.exp.level != data.exp.level) && (data.exp.attr != null))
                if (data.exp.attr != null)
                {
                    localEndlessData.exp.attr.acc_rate = data.exp.attr.acc_rate;
                    localEndlessData.exp.attr.atk_interval = data.exp.attr.atk_interval;
                    localEndlessData.exp.attr.atk_space = data.exp.attr.atk_space;
                    localEndlessData.exp.attr.combat_power = data.exp.attr.combat_power;
                    localEndlessData.exp.attr.crt_rate = data.exp.attr.crt_rate;
                    localEndlessData.exp.attr.ddg_rate = data.exp.attr.ddg_rate;
                    localEndlessData.exp.attr.energy_max = data.exp.attr.energy_max;
                    localEndlessData.exp.attr.energy_revert = data.exp.attr.energy_revert;
                    localEndlessData.exp.attr.hp_max = data.exp.attr.hp_max;
                    localEndlessData.exp.attr.hp_revert = data.exp.attr.hp_revert;
                    localEndlessData.exp.attr.leader = data.exp.attr.leader;
                    localEndlessData.exp.attr.mp_max = data.exp.attr.mp_max;
                    localEndlessData.exp.attr.mp_revert = data.exp.attr.mp_revert;
                    localEndlessData.exp.attr.phy_atk = data.exp.attr.phy_atk;
                    localEndlessData.exp.attr.speed = data.exp.attr.speed;
                    localEndlessData.exp.attr.tnc_rate = data.exp.attr.tnc_rate;
                }
                //Debug.LogError(string.Format("[{0}][{1}, {2}][{3}, {4}]", data.exp.level, data.exp.attr.leader, data.exp.attr.hp_max, localEndlessData.exp.attr.leader, localEndlessData.exp.attr.hp_max));
                #endregion

                #region level [最新等级]
                localEndlessData.exp.level = data.exp.level;
                #endregion

                #region ph_power [玩家当前体力值]
                localEndlessData.exp.ph_power = data.exp.ph_power;
                #endregion

                #region ph_power_max [玩家体力上限]
                localEndlessData.exp.ph_power_max = data.exp.ph_power_max;
                #endregion

                #region equip_list [升级解锁的神器列表]
                if (data.exp.equip_list != null)
                {
                    localEndlessData.exp.equip_list.AddRange(data.exp.equip_list);
                }
                #endregion
            }
            #endregion

            #region info [战斗信息]
            if (data.info != null)
            {
                localEndlessData.info.chapter_id = data.info.chapter_id;    //章节ID//
                localEndlessData.info.fight_tick = data.info.fight_tick;    //进入时间//
                localEndlessData.info.high_grade = data.info.high_grade;    //最高积分//
                localEndlessData.info.high_victory = data.info.high_victory;//最高战绩//
                localEndlessData.info.rank = data.info.rank;                //排名//
                localEndlessData.info.today_times = data.info.today_times;  //进入次数//
                localEndlessData.info.use_time = data.info.use_time;        //最高积分的战斗用时//
                localEndlessData.info.passed = data.info.passed;            //是否全部通关[0-false !0-true]//
                localEndlessData.info.max_times = data.info.max_times;      //最大挑战次数//
            }
            #endregion

            #region items [掉落包]
            if (data.drop_items != null)
            {
                #region drop_equip [装备]
                if (data.drop_items.equip_list != null)
                {
                    localEndlessData.drop_items.equip_list.AddRange(data.drop_items.equip_list);
                }
                #endregion
                #region drop_soldier [武将]
                if (data.drop_items.soldier_list != null)
                {
                    localEndlessData.drop_items.soldier_list.AddRange(data.drop_items.soldier_list);
                }
                #endregion
                #region drop_soul [命魂]
                if (data.drop_items.soul_list != null)
                {
                    localEndlessData.drop_items.soul_list.AddRange(data.drop_items.soul_list);
                }
                #endregion
                #region drop_mail [邮件]
                if (data.drop_items.mail_list != null)
                {
                    if (localEndlessData.drop_items.mail_list.Count <= 0)
                    {
                        for (int i = 0; i < data.drop_items.mail_list.Count; i++)
                        {
                            if (data.drop_items.mail_list[i] != null)
                            {
                                localEndlessData.drop_items.mail_list.Add(GetProtoAttachment(data.drop_items.mail_list[i]));
                            }
                        }
                    }
                    else
                    {
                        List<fogs.proto.msg.Attachment> tmpMailList = new List<fogs.proto.msg.Attachment>();
                        for (int i = 0; i < data.drop_items.mail_list.Count; i++)
                        {
                            tmpIndex = 0;
                            for (int j = 0; j < localEndlessData.drop_items.mail_list.Count; j++)
                            {
                                if (localEndlessData.drop_items.mail_list[j].id == data.drop_items.mail_list[i].id)
                                {
                                    localEndlessData.drop_items.mail_list[j].num += data.drop_items.mail_list[i].num;
                                    break;
                                }
                                tmpIndex = j;
                            }
                            if ((tmpIndex != 0) && (tmpIndex == localEndlessData.drop_items.mail_list.Count - 1))
                            {
                                if (data.drop_items.mail_list[i] != null)
                                {
                                    tmpMailList.Add(GetProtoAttachment(data.drop_items.mail_list[i]));
                                }
                            }
                        }
                        localEndlessData.drop_items.mail_list.AddRange(tmpMailList);
                    }
                }
                #endregion
                #region drop_item [道具]
                if (data.drop_items.item_list != null)
                {
                    //for (int i = 0; i < data.drop_items.item_list.Count; i++)
                    //{
                    //    Debug.LogError(string.Format("[{0}, {1}, {2}]", data.drop_items.item_list[i].id, data.drop_items.item_list[i].num, data.drop_items.item_list[i].change_num));
                    //}
                    //Debug.LogError("----------------------------------------------------");
                    if (localEndlessData.drop_items.item_list.Count <= 0)
                    {
                        for (int i = 0; i < data.drop_items.item_list.Count; i++)
                        {
                            if (data.drop_items.item_list[i] != null)
                            {
                                localEndlessData.drop_items.item_list.Add(GetProtoItemInfo(data.drop_items.item_list[i]));
                            }
                        }
                    }
                    else
                    {
                        List<fogs.proto.msg.ItemInfo> tmpItemList = new List<fogs.proto.msg.ItemInfo>();
                        for (int i = 0; i < data.drop_items.item_list.Count; i++)
                        {
                            tmpIndex = 0;
                            for (int j = 0; j < localEndlessData.drop_items.item_list.Count; j++)
                            {
                                if (localEndlessData.drop_items.item_list[j].id == data.drop_items.item_list[i].id)
                                {
                                    localEndlessData.drop_items.item_list[j].num = data.drop_items.item_list[i].num;
                                    break;
                                }
                                tmpIndex = j;
                            }
                            if ((tmpIndex != 0) && (tmpIndex == localEndlessData.drop_items.item_list.Count - 1))
                            {
                                if (data.drop_items.item_list[i] != null)
                                {
                                    tmpItemList.Add(GetProtoItemInfo(data.drop_items.item_list[i]));
                                }
                            }
                        }
                        localEndlessData.drop_items.item_list.AddRange(tmpItemList);
                    }
                    //for (int i = 0; i < localEndlessData.drop_items.item_list.Count; i++)
                    //{
                    //    Debug.LogError(string.Format("[{0}, {1}, {2}]", localEndlessData.drop_items.item_list[i].id, localEndlessData.drop_items.item_list[i].num, localEndlessData.drop_items.item_list[i].change_num));
                    //}
                }
                #endregion
                #region drop_special [特殊物品]
                if (data.drop_items.special_list != null)
                {
                    if (localEndlessData.drop_items.special_list.Count <= 0)
                    {
                        for (int i = 0; i < data.drop_items.special_list.Count; i++)
                        {
                            if (data.drop_items.special_list[i] != null)
                            {
                                localEndlessData.drop_items.special_list.Add(GetProtoItemInfo(data.drop_items.special_list[i]));
                            }
                        }
                    }
                    else
                    {
                        List<fogs.proto.msg.ItemInfo> tmpSpeInfoList = new List<fogs.proto.msg.ItemInfo>();
                        for (int i = 0; i < data.drop_items.special_list.Count; i++)
                        {
                            tmpIndex = 0;
                            for (int j = 0; j < localEndlessData.drop_items.special_list.Count; j++)
                            {
                                if (localEndlessData.drop_items.special_list[j].id == data.drop_items.special_list[i].id)
                                {
                                    localEndlessData.drop_items.special_list[j].num = data.drop_items.special_list[i].num;
                                    localEndlessData.drop_items.special_list[j].change_num += data.drop_items.special_list[i].change_num;
                                    break;
                                }
                                tmpIndex = j;
                            }
                            if ((tmpIndex != 0) && (tmpIndex == localEndlessData.drop_items.special_list.Count - 1))
                            {
                                if (data.drop_items.special_list[i] != null)
                                {
                                    tmpSpeInfoList.Add(GetProtoItemInfo(data.drop_items.special_list[i]));
                                }
                            }
                        }
                        localEndlessData.drop_items.special_list.AddRange(tmpSpeInfoList);
                    }
                }
                #endregion
            }
            #endregion
        }
    }

    private fogs.proto.msg.Skill GetProtoSkill(fogs.proto.msg.Skill vSourceData)
    {
        if (vSourceData != null)
        {
            fogs.proto.msg.Skill tmpResult = new fogs.proto.msg.Skill();
            tmpResult.id = vSourceData.id;
            tmpResult.level = vSourceData.level;
            return tmpResult;
        }
        else
        {
            return null;
        }
    }
    private fogs.proto.msg.Attachment GetProtoAttachment(fogs.proto.msg.Attachment vSourceData)
    {
        if (vSourceData != null)
        {
            fogs.proto.msg.Attachment tmpResult = new Attachment();
            tmpResult.id = vSourceData.id;
            tmpResult.num = vSourceData.num;
            return tmpResult;
        }
        else
        {
            return null;
        }
    }
    private fogs.proto.msg.ItemInfo GetProtoItemInfo(fogs.proto.msg.ItemInfo vSourceData)
    {
        if (vSourceData != null)
        {
            fogs.proto.msg.ItemInfo tmpResult = new fogs.proto.msg.ItemInfo();
            tmpResult.id = vSourceData.id;
            tmpResult.num = vSourceData.num;
            tmpResult.change_num = vSourceData.change_num;
            return tmpResult;
        }
        else
        {
            return null;
        }
    }

    public void SendOneKeyReplaceEquip(EFightType mapType, List<ulong> offEquip, List<ReplaceEquip> upEquips)
    {
        Debug.Log("SendOneKeyReplaceEquip");
        dungeonType = mapType;
        OneKeyReplaceEquipReq data = new OneKeyReplaceEquipReq();
        data.offequip_list.Clear();
        data.offequip_list.AddRange(offEquip);
        data.onequip_list.Clear();
        data.onequip_list.AddRange(upEquips);
        _FightRelatedNetWork.SendOneKeyReplaceEquip(data);
    }

    public void ReceiveOneKeyReplaceEquip(OneKeyReplaceEquipResp data)
    {
        Debug.Log("ReceiveOneKeyReplaceEquip");
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance._ArtifactedDepot.RefreshList(data.update_equip_list);
            PlayerData.Instance.UpdatePlayerAttribute(data.atttr);
            switch (dungeonType)
            {
                case EFightType.eftMain:
                case EFightType.eftEndless:
                case EFightType.eftActivity:
                    {
                        UISystem.Instance.GateInfoView.OneKeyEquipSuccess();
                    }
                    break;
                case EFightType.eftExpedition:
                    {
                        UISystem.Instance.ExpeditionInfoView.OneKeyEquipSuccess();
                    }
                    break;
            }
        }
        else
        {
            FightRelatedModule.Instance.isFightState = false;
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    /// <summary>
    /// 异域探险
    /// </summary>
    public void SendUnionPveDgnReward(uint dgnID, int damage, int fight_result)
    {
        Debug.Log("SendUnionPveDgnReward");
        UnionPveDgnRewardReq data = new UnionPveDgnRewardReq();
        data.dgn_id = dgnID;
        data.damage = damage;
        data.fight_result = fight_result;
        _FightRelatedNetWork.UnionPveDgnRewardReq(data);
    }
    public void ReceiveUnionPveDgnReward(UnionPveDgnRewardResp data)
    {
        Debug.Log("ReceiveUnionPveDgnReward");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                PlayerData.Instance.UpdateExoticBattleSettle(data);
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else if (data.result == (int)ErrorCodeEnum.NotHaveUnion)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.UNION_TIP_KICK, ExpelFromUnion);
            }
            else
            {
                //ErrorCode.ShowErrorTip(data.result);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    /// <summary>
    /// 被军团踢出
    /// </summary>
    private void ExpelFromUnion()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_FIGHT);
        Main.Instance.StateMachine.ChangeState(MainCityState.StateName);
    }


    /// <summary>
    /// 开始奴役战斗
    /// </summary>
    /// <param name="data"></param>
    public void SendStartEslaveFightReq(EnslaveFightType type, List<SoldierList> soldiers, List<EquipList> equips)
    {
        if (isFightState)
            return;
        isFightState = true;
        StartEnslaveFightReq tmpInfo = new StartEnslaveFightReq();
        tmpInfo.type = type;
        tmpInfo.soldiers.AddRange(soldiers);
        tmpInfo.attack_equips.AddRange(equips);
        _FightRelatedNetWork.SendStartEslaveFightReq(tmpInfo);
    }
    public void ReceiveStartEnslaveFightResp(StartEnslaveFightResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            PlayerData.Instance.UpdateSP(data.ph_power);
            UISystem.Instance.PrepareBattleView.OnStartEnslaveFightResp(data.hero_attr);
        }
        else
        {
            isFightState = false;
            ErrorCode.ShowErrorTip(data.result);
        }
        //PlayerData.Instance._Prison.ReceiveStartEnslaveFightResp(data);
    }
    /// <summary>
    /// 奴役战斗结算
    /// </summary>
    /// <param name="data"></param>
    public void SendEnslaveFightOverReq(EnslaveFightResult result, int key, string sign)
    {
        Debug.LogWarning(string.Format("SendEnslaveFightOverReq"));
        EnslaveFightOverReq tmpInfo = new EnslaveFightOverReq();
        tmpInfo.result = result;
        tmpInfo.key = key;
        tmpInfo.sign = sign;
        _FightRelatedNetWork.SendEnslaveFightOverReq(tmpInfo);
    }
    public void ReceiveEnslaveFightOverResp(EnslaveFightOverResp data)
    {
        Debug.LogWarning("ReceiveEnslaveFightOverResp");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else
            {
                //ErrorCode.ShowErrorTip(data.result);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    public void SendEnslaveFightBeforeBack()
    {
        EnslaveFightBeforeBackReq data = new EnslaveFightBeforeBackReq();
        _FightRelatedNetWork.SendEnslaveFightBeforeBack(data);
    }
    public void ReceiveEnslaveFightBeforeBack(EnslaveFightBeforeBackResp data)
    {
        UISystem.Instance.PrepareBattleView.OnEnslaveFightBeforeBack();
    }

    /// <summary>
    /// 攻城略地结算
    /// </summary>
    /// <param name="vCityID">城市ID</param>
    /// <param name="vScore">积分</param>
    /// <param name="vDamage">伤害</param>
    /// <param name="vSign">回发的验证签名 暂留</param>
    public void SendCaptureTerritoryReq(int vScore, int vDamage, string vSign = "")
    {
        Debug.LogWarning(string.Format("SendCaptureTerritoryReq: " + vScore + "|" + vDamage));
        CampaignRewardReq data = new CampaignRewardReq();
        data.id = CaptureTerritoryModule.Instance.CurrentCityID;
        data.score = vScore;
        data.damage = vDamage;
        data.sign = vSign;
        _FightRelatedNetWork.SendCaptureTerritoryReq(data);
    }
    public void ReceiveCaptureTerritoryResp(CampaignRewardResp vData)
    {
        Debug.Log("ReceiveCaptureTerritoryResp");
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(vData);
            }
            else
            {
                //ErrorCode.ShowErrorTip(vData.result);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(vData.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }

    /// <summary>
    /// 全服爭霸
    /// </summary>
    /// <param name="vInitHurt"></param>
    public void SendServerHegemonyReq()
    {
        //BillingResp tmpRespData = new BillingResp();
        //tmpRespData.result = (int)ErrorCodeEnum.SUCCESS;
        //tmpRespData.totalhurt = 520671;
        //ReceiveServerHegemonyResp(tmpRespData);
        //return;

        Debug.LogWarning(string.Format("SendServerHegemonyReq"));
        BillingReq tmpData = new BillingReq();
        tmpData.accid = PlayerData.Instance.GetAccountID();
        tmpData.hurt = (uint)FightRelatedModule.Instance.ServerHegemonyHurtValue;
        _FightRelatedNetWork.SendServerHegemonyReq(tmpData);
    }
    public void ReceiveServerHegemonyResp(BillingResp vData)
    {
        Debug.Log("ReceiveServerHegemonyResp");
        if (vData != null)
        {
            if (vData.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(vData);
            }
            else
            {
                //ErrorCode.ShowErrorTip(vData.result);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(vData.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }

    /// <summary>
    /// 排位赛结算
    /// </summary>
    /// <param name="data"></param>
    public void SendEndPole(PoleResult result, string uid)
    {
        Debug.Log("SendEndPole");
        EndPoleReq data = new EndPoleReq();
        data.pole_result = result;
        data.uid = uid;
        _FightRelatedNetWork.SendEndPole(data);

        //EndPoleResp tmpdata = new EndPoleResp();
        //tmpdata.result = (int)ErrorCodeEnum.SUCCESS;
        //tmpdata.pole_result = result;
        //tmpdata.score = 120;
        //ReceiveEndPole(tmpdata);
    }

    public void ReceiveEndPole(EndPoleResp data)
    {
        Debug.Log("ReceiveEndPole");
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                Debug.Log("data.score=" + data.score);
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else
            {
                //ErrorCode.ShowErrorTip(data.result);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ErrorCode.GetString(data.result), UISystem.Instance.FightView.DelayToSupermacy);
            }
        }
        else
        {
            UISystem.Instance.FightView.DelayToSupermacy();
        }
    }
    /// <summary>
    /// 跨服战结算
    /// </summary>
    /// <param name="_damage"></param>
    public void OnSendCombatSettlement(uint _damage)
    {
        CombatSettlementReq req = new CombatSettlementReq();
        req.accid = PlayerData.Instance._AccountID;
        req.hurt = _damage;
        req.tile_id = UISystem.Instance.CrossServerWarView.cur_tileID;
        _FightRelatedNetWork.SendCombatSettlementReq(req);
    }
    public void OnReceiveCombatSettlement(CombatSettlementResp data)
    {
        if (data != null)
        {
            if (data.result == (int)ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.FightView.RefreshInfo_ResultOfBattle(data);
            }
            else
            {
                ErrorCode.ShowErrorTip(data.result);
            }
        }
        else
        {
            Debug.LogError("CombatSettlementResp is null");
        }
    }
}
