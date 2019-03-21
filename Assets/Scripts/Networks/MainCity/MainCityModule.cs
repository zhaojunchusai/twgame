using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
using System;
using Assets.Script.Common;

/* File：CreateRoleNetWork.cs
 * Desc: 主城的数据处理
 * Date: 2015-05-12 10:45
 * Author: taiwei
 */
public class MainCityModule : Singleton<MainCityModule>
{
    public MainCityNetWork mMainCityNetWork;

    private int _timer = 0;
    public int SPRecoverTimer
    {
        get { return _timer; }
    }
    private bool _recovering = false;
    public List<int> UnlockBuildings = new List<int>();
    public bool ShowCommentView = false;
    public List<uint> LockFuncs = new List<uint>();
    private NoticeServerInfo _enterGameNotice = null;
    public void Initialize()
    {
        if (mMainCityNetWork == null)
        {
            mMainCityNetWork = new MainCityNetWork();
            mMainCityNetWork.RegisterMsg();
        }
        if (backpackMarqueeQueue == null)
        {
            backpackMarqueeQueue = new List<NotifyMarquee>();
        }
        if (arenaMarqueeQueue == null)
        {
            arenaMarqueeQueue = new List<NotifyMarquee>();
        }
        if (systemOepnMarqueeQueue == null)
        {
            systemOepnMarqueeQueue = new List<NotifyMarquee>();
        }
        if (otherMarqueeQueue == null)
        {
            otherMarqueeQueue = new List<NotifyMarquee>();
        }
        if (supermacyMarqueeQueue == null)
        {
            supermacyMarqueeQueue = new List<NotifyMarquee>();
        }
        PlayerData.Instance.UpdatePlayerSPEvent += OnSPUpdate;
        PlayerData.Instance.NotifyResetEvent += OnResetOnlinePackage;
        UISystem.Instance.ViewCloseEvent += CloseUIEvent;
    }
    private int _restOnlineTime;
    public int RestOnlineTime
    {
        get { return _restOnlineTime; }
    }

    public void CloseUIEvent(string uiname)
    {
        ManageSimpleChat(uiname);
        CheckOpenMainCity();
    }
    public void CheckOpenMainCity()
    {
        if (ReturnMainCity())
        {
            if (UISystem.Instance.HintView.FightLoadingCompletedEvent != null)
                UISystem.Instance.HintView.FightLoadingCompletedEvent -= CheckOpenMainCity;
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenMainCity);
            if (PlayerData.Instance.BuildingUnlockList != null && PlayerData.Instance.BuildingUnlockList.Count > 0)
            {
                UnlockBuildings.AddRange(PlayerData.Instance.BuildingUnlockList);
                PlayerData.Instance.BuildingUnlockList.Clear();
            }
            UISystem.Instance.MainCityView.CheckUnlockBuilding(UnlockBuildings);
            ShowGoToCommentView();
        }
    }
    /// <summary>
    /// 关闭界面的时候处理simpleChat界面
    /// 用以解决类似从背包获取途径界面打开攻城界面后，再关闭simpleChat会显示在背包界面上
    /// 
    /// </summary>
    private void ManageSimpleChat(string uiname)
    {
        if (uiname.Equals(CaptureTerritoryView.UIName)
            || uiname.Equals(SupermacyView.UIName)||uiname.Equals(CrossServerWarView.UIName))
        {
            UISystem.Instance.ResortSimpleChat();
        }        
    }

    public bool HasGameAnnouncement()
    {
        return _enterGameNotice != null && !string.IsNullOrEmpty(_enterGameNotice.title) && !string.IsNullOrEmpty(_enterGameNotice.body);
    }
    public void SetGameAnnouncement(NoticeServerInfo notice)
    {
        _enterGameNotice = notice;
    }
    public void ShowGameAnnouncement()
    {
        if (HasGameAnnouncement())
        {
            UISystem.Instance.ShowGameUI(AnnouncementView.UIName);
            UISystem.Instance.AnnouncementView.OnUpdateAnnouncement(_enterGameNotice.title, _enterGameNotice.body, EAnnouncementType.MainCity);
        }
    }

    /// <summary>
    /// 延迟显示五星评定界面
    /// true-进入游戏时调用 false-强制新手引导完成时调用
    /// </summary>
    /// <param name="isEnterGame">true-进入游戏时调用 false-强制新手引导完成时调用</param>
    public void DelayShowCommentView(bool isEnterGame)
    {
        if (!PlayerData.Instance.RemindToComment)
            return;

        if (isEnterGame)
        {
            string guideData = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.FORCE_GUIDE_LAST_ID);
            if (GuideManager.Instance.IsGuideFinish(
            uint.Parse(guideData)))
            {
                Scheduler.Instance.AddTimer(
                    60f, false, () =>
                    {
                        MainCityModule.Instance.ShowCommentView = true;
                        if (ReturnMainCity())
                            ShowGoToCommentView();
                    }
                                         );
            }
        }
        else
        {
            Scheduler.Instance.AddTimer(
                   3600f, false, () =>
                   {
                       MainCityModule.Instance.ShowCommentView = true;
                       if (ReturnMainCity())
                           ShowGoToCommentView();
                   }
                                        );
        }
    }

    public void ShowGoToCommentView()
    {
        if (!PlayerData.Instance.RemindToComment)
            return;

        if (!GuideManager.Instance.GuideIsRunning() && ShowCommentView && UnlockBuildings.Count <= 0)
        {
            MainCityModule.Instance.ShowCommentView = false;
            UISystem.Instance.ShowGameUI(CommentView.UIName);
        }
    }

    private bool ReturnMainCity()
    {
        List<string> list = UISystem.Instance.openingView;
        if ((list.Count <= 5
            && list.Contains(ViewType.DIR_VIEWNAME_HINT)
            && list.Contains(ViewType.DIR_VIEWNAME_MAINCITY)
            && list.Contains(ViewType.DIR_VIEWNAME_MENU)
            && list.Contains(ViewType.DIR_VIEWNAME_TOPFUNC)
            && list.Contains(ViewType.DIR_VIEWNAME_SIMPLECHAT))
            || (list.Count <= 6
            && list.Contains(ViewType.DIR_VIEWNAME_HINT)
            && list.Contains(ViewType.DIR_VIEWNAME_MAINCITY)
            && list.Contains(ViewType.DIR_VIEWNAME_MENU)
            && list.Contains(ViewType.DIR_VIEWNAME_TOPFUNC)
            && list.Contains(ViewType.DIR_VIEWNAME_GUIDE)
            && list.Contains(ViewType.DIR_VIEWNAME_SIMPLECHAT)))
            return true;
        return false;
    }

    private void OnSPUpdate()
    {
        if (PlayerData.Instance._Physical < PlayerData.Instance.MaxPhysical)
        {
            if (_recovering)
                return;
            StartRecoverSP(GlobalCoefficient.SPRecoverTimer);
        }
        else
        {
            CompletedRecover();
        }
    }

    public void StartRecoverSP(int timer)
    {
        if (timer == 0)
            CompletedRecover();
        _timer = timer;
        _recovering = true;
        Assets.Script.Common.Scheduler.Instance.AddTimer(1, true, UpdateRecoverTimer);
    }

    private void CompletedRecover()
    {
        _timer = 0;
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(UpdateRecoverTimer);
        _recovering = false;
    }

    private void UpdateRecoverTimer()
    {
        --_timer;

        if (_timer <= 0)
        {
            CompletedRecover();
            PlayerData.Instance.UpdateSP(PlayerData.Instance._Physical + 1);
        }
    }

    #region OnlinePackage

    public void OnlinePackageCountDownComplete()
    {
        PlayerData.Instance.OnlineRewardInfo.need_online_time = 0;
        PlayerData.Instance.OnlineRewardInfo.reward_sign = 1;
        Scheduler.Instance.RemoveTimer(OnlinePackageCountTimer);
    }
    /// <summary>
    ///  0 倒计时 1 可以领取 2 已经领取完
    /// </summary>
    public void StartOnlinePackageCountDown()
    {
        OnlineRewardInfo info = PlayerData.Instance.OnlineRewardInfo;
        //info.need_online_time = 10;
        //Debug.LogWarning(" _restOnlineTime " + _restOnlineTime + " sign " + PlayerData.Instance.OnlineRewardInfo.reward_sign);
        if (info.reward_sign == 0)
        {
            _restOnlineTime = info.need_online_time;
            Scheduler.Instance.AddTimer(1f, true, OnlinePackageCountTimer);
        }
    }

    private void OnlinePackageCountTimer()
    {
        if (_restOnlineTime > 1)
        {
            _restOnlineTime -= 1;
        }
        else
        {
            _restOnlineTime = 0;
            OnlinePackageCountDownComplete();
        }
        PlayerData.Instance.UpdateOnlineTime();
    }

    private void OnResetOnlinePackage(NotifyReset data)
    {
        //Debug.LogWarning("OnResetOnlinePackage ... " + data.online_reward_info.reward_sign + " next id = " + data.online_reward_info.next_reward_id + " need " + data.online_reward_info.need_online_time);
        Scheduler.Instance.RemoveTimer(OnlinePackageCountTimer);
        StartOnlinePackageCountDown();
    }

    #endregion
    public void SendSpRevert()
    {
        PHpowerRevertReq data = new PHpowerRevertReq();
        mMainCityNetWork.SendSpRevert(data);
    }

    public void ReceiveSpRevert(PHpowerRevertResp data)
    {
        if (data.result == 0)
        {
            Assets.Script.Common.Scheduler.Instance.AddTimer((float)data.surplus_time, false, SendSpRevert);
            PlayerData.Instance.UpdateSP(data.ph_power);
            PlayerData.Instance.SpRevertTime = data.surplus_time;
            Debug.Log("current sp:" + PlayerData.Instance.SpRevertTime);
        }
        else
        {
            Debug.LogError("Error Code:" + data.result);
        }
    }
    public void SendFirstRechargeAward()
    {
        FirstRechargeAwardReq req = new FirstRechargeAwardReq();
        mMainCityNetWork.SendFirstRechargeAward(req);
    }

    public void ReceiveFirstRechargeAward(FirstRechargeAwardResp resp)
    {
        if (resp.result == 0)
        {
            PlayerData.Instance.UpdateDropData(resp.drop_items);
            UISystem.Instance.CloseGameUI(FirstPayView.UIName);
            PlayerData.Instance.FirstPayGift = 2;
            if (UISystem.Instance.UIIsOpen(MenuView.UIName))
                UISystem.Instance.MenuView.SetFirstPayBtn();

            ShowDropAward(resp.drop_items);

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }


    public void SendExchangeGold(int exchageTimes)
    {
        mMainCityNetWork.SendExchangeGold(exchageTimes);
    }

    public void ReceiveExchangeGold(TouchGoldResp resp)
    {
        if (resp.result == 0)
        {

            UISystem.Instance.BuyCoinView.UpdateBuyCoinResult(resp);

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendBuySP()
    {
        mMainCityNetWork.SendBuySP();
    }
    public void ReceiveBuySP(BuyPHPowerResp resp)
    {
        if (resp.result == 0)
        {
            PlayerData.Instance.UpdateSP(resp.ph_power);
            PlayerData.Instance.UpdateItem(resp.money_item);
            PlayerData.Instance.BuySPTimes = resp.buy_phpower_num;
            UISystem.Instance.CloseGameUI(BuySPView.UIName);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SP_SUCCESS);
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void OnSendOnlineReward()
    {
        mMainCityNetWork.OnSendOnlinePackageReward(new OnlineRewardReq());
    }

    public void OnReceiveOnlineReward(OnlineRewardResp resp)
    {
        if (resp.result == 0)
        {
            //Debug.Log("resp.online before " + resp.need_online_time + " sign " + resp.reward_sign + " curRewardID " + PlayerData.Instance.OnlineRewardInfo.now_reward_id);

            PlayerData.Instance.OnlineRewardInfo.need_online_time = resp.need_online_time;
            PlayerData.Instance.OnlineRewardInfo.now_reward_id = resp.next_reward_id;
            PlayerData.Instance.OnlineRewardInfo.next_reward_id = resp.next_reward_id;
            PlayerData.Instance.OnlineRewardInfo.reward_sign = resp.reward_sign;
            PlayerData.Instance.UpdateOnlineTime();
            //Debug.Log("resp.online After " + resp.need_online_time + " sign " + resp.reward_sign + " curRewardID " + PlayerData.Instance.OnlineRewardInfo.now_reward_id + " next " + PlayerData.Instance.OnlineRewardInfo.next_reward_id);

            if (resp.drop_items.mail_list != null && resp.drop_items.mail_list.Count > 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
            }
            else
            {
                List<CommonItemData> list = CommonFunction.GetCommonItemDataList(resp.drop_items.item_list, resp.drop_items.equip_list, resp.drop_items.soldier_list);
                list.AddRange(CommonFunction.GetCommonItemDataList(resp.drop_items.special_list, null, null));
                PlayerData.Instance.UpdateItem(resp.drop_items.item_list);
                PlayerData.Instance.MultipleAddWeapon(resp.drop_items.equip_list);
                PlayerData.Instance.MultipleAddSoldier(resp.drop_items.soldier_list);
                PlayerData.Instance.UpdateItem(resp.drop_items.special_list);
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list);
            }
            StartOnlinePackageCountDown();
        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }

    public void SendBuildingEffectRecord(bool isAdd, List<int> list)
    {
        BuildUnlockAnimeIdReq req = new BuildUnlockAnimeIdReq();
        req.handle_type = isAdd ? 1 : 2; //1 加  2减
        req.id_list.Clear();
        req.id_list.AddRange(list);
        mMainCityNetWork.SendBuildingEffectRecord(req);
    }

    public void ReceiveBuildingEffectRecord(BuildUnlockAnimeIdResp resp)
    {

    }

    public void SendGetFirstLoginReward()
    {
        FirstLoginAwardReq req = new FirstLoginAwardReq();
        mMainCityNetWork.SendGetFirstLoginReward(req);
    }
    public void ReceiveGetFirstLoginReward(FirstLoginAwardResp resp)
    {
        if (resp.result == 0)
        {
            PlayerData.Instance.UpdateDropData(resp.drop_items);
            UISystem.Instance.CloseGameUI(FirstPayView.UIName);
            PlayerData.Instance.GotFirstLoginReward = true;
            if (UISystem.Instance.UIIsOpen(MenuView.UIName))
                UISystem.Instance.MenuView.SetFirstLoginBtn();
            ShowDropAward(resp.drop_items);

        }
        else
        {
            ErrorCode.ShowErrorTip(resp.result);
        }
    }
    private void ShowDropAward(fogs.proto.msg.DropList droplist)
    {
        List<fogs.proto.msg.ItemInfo> list = new List<fogs.proto.msg.ItemInfo>();
        if (droplist.item_list != null) list.AddRange(droplist.item_list);
        if (droplist.special_list != null) list.AddRange(droplist.special_list);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(
            CommonFunction.GetCommonItemDataList(list, droplist.equip_list,
            droplist.soldier_list));
    }

    #region 新消息推送

    public List<NotifyMarquee> backpackMarqueeQueue;
    public List<NotifyMarquee> arenaMarqueeQueue;
    public List<NotifyMarquee> systemOepnMarqueeQueue;
    public List<NotifyMarquee> otherMarqueeQueue;
    public List<NotifyMarquee> supermacyMarqueeQueue;
    public void ReceiveNotifyMarquee(NotifyMarquee data)
    {
        switch (data.type)
        {
            case MarqueeType.BACKGROUND:
                backpackMarqueeQueue.Add(data);
                break;
            case MarqueeType.ARENA:
                arenaMarqueeQueue.Add(data);
                break;
            case MarqueeType.OPEN_SYSTEM:
                systemOepnMarqueeQueue.Add(data);
                break;
            case MarqueeType.OTHER:
                otherMarqueeQueue.Add(data);
                break;
            case MarqueeType.PMDOVERLORD:
                Debug.Log("-------------receive supermacy login-----------------");
                supermacyMarqueeQueue.Add(data);
                break;
                
        }
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_MAINCITY))
            UISystem.Instance.MainCityView.PlayMarquee();
    }


    private List<NotifyRefresh> _waitingNotifyList = new List<NotifyRefresh>();
    public void ReceiveNotify(NotifyRefresh resp)
    {
        Debug.Log("ReceiveNotify type=" + resp.type.ToString() + " state=" + resp.status.ToString() + " num=" + resp.num.ToString());
        switch (resp.type)
        {
            case  NotifyType.IN_ACTIVITY_TIME:
                {
                    GameActivityModule.Instance.OnSendQueryActivityTimeReq();
                    break;
                }
            case NotifyType.NOTICE:
                {
                    break;
                }
            case NotifyType.TASK:
                {
                    TaskModule.Instance.NotifyRefreshTask(resp);
                    break;
                }
            case NotifyType.LIVENESS:
                {
                    TaskModule.Instance.NotifyLiveness(resp);
                    break;
                }
            case NotifyType.MAIL:
                {
                    MailModule.Instance.OnReceiveNewMailNotify(resp);
                    break;
                }
            case NotifyType.SIGN:
                {
                    TaskModule.Instance.NotifySign(resp);
                    break;
                }
            case NotifyType.ONLINE_DAY:
                {
                    OnlineDayUpdateInfoReq();
                    break;
                }
            case NotifyType.ARENA_LOG:
                {
                    PVPModule.Instance.IsBattleLog = true;
                    break;
                }
            case NotifyType.ENSLAVE_RECOED:
                {
                    PlayerData.Instance._Prison.SetNotify(resp.num > 0);
                } break;

            case NotifyType.RANK_UPDATE:
                {
                    RankModule.Instance.NeedRefreshRank = true;
                } break;
            case NotifyType.ACTIVITY:
                {
                    PlayerData.Instance.NewGameActivityIDList.Clear();
                    PlayerData.Instance.NewGameActivityIDList.AddRange(resp.activities);
                    GameActivityModule.Instance.OnUpdateNewActivity();
                }
                break;
            case NotifyType.INTERNEL_FRIENDS_APPEND:
                {
                    PlayerData.Instance.FbfriendDepot.IsApplyNotice = resp.num > 0;
                    if (UISystem.Instance.FriendView != null)
                        UISystem.Instance.FriendView.InitNotice();
                }
                break;
            case NotifyType.NEWHAND_TASK:
                {
                    break;
                }
            case NotifyType.CAMPAIGN:
                {
                    CaptureTerritoryModule.Instance.CaptureTerritoryNotify(resp);
                    break;
                }
            case NotifyType.OVERLORD:
                {
                    if(resp.status==2 && UISystem.Instance.UIIsOpen(SupermacyView.UIName)&& resp.num == 1 && SupermacyModule.Instance.StartResult != (int)ErrorCodeEnum.SUCCESS)
                    {
                        Debug.Log("----------------recerive refresh--------------" + CommonFunction.GetTimeByLong(Main.mTime));
                        SupermacyModule.Instance.needRefresh = true;
                    }
                    break;
                }
            case NotifyType.ALTAR_UNION:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.UnionPrison, false))
                        UnionPrisonModule.Instance.Prompt = resp.num > 0;
                    else
                        UnionPrisonModule.Instance.Prompt = false;
                    break;
                }
            case NotifyType.ACHIEVEMENT:
                {
                    if (resp.status == 2 && resp.num==PlayerData.Instance.HeadID)
                    {
                        uint defaultNum;
                        if (PlayerData.Instance._Gender == 1)
                            defaultNum = 10002;
                        else defaultNum = 10001;
                        SystemSettingModule.Instance.SendHeadChangeRequset(defaultNum);

                        //PlayerData.Instance.HeadID = defaultNum;
                        //UISystem.Instance.MainCityView.UpdateHeadIcon();
                        //UISystem.Instance.SystemSettingView.InitPlayerInfo();
                    }
                    if (resp.status == 3 && resp.num == PlayerData.Instance.FrameID)
                    {
                        uint defaultNum = 10001;
                        SystemSettingModule.Instance.SendIconChangeRequset(defaultNum);
                        //PlayerData.Instance.FrameID = defaultNum;
                        //UISystem.Instance.MainCityView.ShowIconChange();
                        //UISystem.Instance.TopFuncView.ShowIconBG();
                        //UISystem.Instance.SystemSettingView.SetSystemPlayerIcon();
                    }
                    break;
                }
            case NotifyType.CHANGE_UNION_ICON:
                {
                    UnionModule.Instance.RefreshBadgeInfo();
                    break;
                }
            case NotifyType.NT_CROSSSERVERWAR:
                {
                    CrossServerWarModule.Instance.CrossServerWarNotify(resp);
                    break;
                }
        }
        PlayerData.Instance.SendNotifyRefreshEvent(resp);
        ShowNotify(resp);
    }

    private void ShowNotify(NotifyRefresh resp, bool doWaitList = false)
    {
        bool showNotifyFailed = true;
        if (showNotifyFailed && UISystem.Instance.UIIsOpen(MainCityView.UIName))
        {
            showNotifyFailed = !UISystem.Instance.MainCityView.ShowNofity(resp);
        }
        if (showNotifyFailed && UISystem.Instance.UIIsOpen(MenuView.UIName))
        {
            showNotifyFailed = !UISystem.Instance.MenuView.ShowNofity(resp);
        }
        if (showNotifyFailed && UISystem.Instance.UIIsOpen(PVPView.UIName))
        {
            UISystem.Instance.PvpView.ShowNotify(resp);
        }
        if (!doWaitList && showNotifyFailed)
            _waitingNotifyList.Add(resp);

        if (doWaitList && !showNotifyFailed)
            _waitingNotifyList.Remove(resp);
    }

    public void GetWaitingNotify()
    {
        if (_waitingNotifyList == null || _waitingNotifyList.Count < 1)
        {
            return;
        }

        for (int i = _waitingNotifyList.Count - 1; i >= 0; i--)
        {
            ShowNotify(_waitingNotifyList[i], true);
        }
    }

    #endregion

    public void MaxCombatPowerNotify(UpdateMaxViewCombatPower resp)
    {
        PlayerData.Instance.UpdateMaxCombatPower(resp.max_view_combat_power);
    }

    public void FinishNewbieGuideReq(uint type)
    {
        FinishGuideStepReq req = new FinishGuideStepReq();
        req.step = type;
        mMainCityNetWork.SendFinishGuideStepReq(req);
    }

    public void StartNewbieGuideReq(uint type)
    {
        StartGuideStepReq req = new StartGuideStepReq();
        req.step = type;
        mMainCityNetWork.SendStartGuideStepReq(req);
    }

    public void OnlineDayUpdateInfoReq()
    {
        mMainCityNetWork.OnlineDayUpdateInfoReq();
    }

    public void OnlineDayUpdateInfoResp(OnlineDayUpdateInfoResp resp)
    {
        if (resp.result == 0)
        {
            PlayerData.Instance.SetOnlineDayUpdate(resp);
        }
    }

    public void NotifyResetData(NotifyReset data)
    {
        PlayerData.Instance.NotifyResetData(data);
    }

    public void Uninitialize()
    {
        if (mMainCityNetWork != null)
            mMainCityNetWork.RemoveMsg();
        mMainCityNetWork = null;
        arenaMarqueeQueue.Clear();
        backpackMarqueeQueue.Clear();
        otherMarqueeQueue.Clear();
        systemOepnMarqueeQueue.Clear();
        supermacyMarqueeQueue.Clear();
        UnlockBuildings.Clear();
        PlayerData.Instance.UpdatePlayerSPEvent -= OnSPUpdate;
        PlayerData.Instance.NotifyResetEvent -= OnResetOnlinePackage;
        UISystem.Instance.ViewCloseEvent -= CloseUIEvent;
        CompletedRecover();
        OnlinePackageCountDownComplete();
    }
}
