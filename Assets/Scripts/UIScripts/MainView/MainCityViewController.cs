using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
using UnityEngine.UI;
public class MainCityViewController : UIBase
{
    public MainCityView View;
    private bool IsOpenMenus = false;//是否开启菜单
    private float DelayOpenTime = 0f;//开启界面延迟时间
    private List<MarqueeData> marqueeList;
    public enum MarqueeState
    {
        Idle,
        Playing,
        NextPlay,
        PlayEnd,
    }

    private MarqueeState marqueeState = MarqueeState.Idle;
    private bool marqueePlayStatus = false;
    private class MarqueeData
    {
        public MarqueeState status = MarqueeState.Idle;
        public UILabel label = null;
    }
    public override void Initialize()
    {
        if (View == null)
        {
            View = new MainCityView();
            View.Initialize();
            //Scheduler.Instance.AddTimer((uint)PlayerData.Instance.SpRevertTime, false, () => { MainCityModule.Instance.SendSpRevert(); });
            SupermacyModule.Instance.ShowSupermacyName(true);
        }

        InitMarquee();
        StartMarquee();
        //UpdateCurrency();
        UpdatePlayerHeadUI();
        //InitTweenComps();
        UpdateLevelUnLock();
        //UpdateMarquee(); 
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_MENU);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SIMPLECHAT);
        UISystem.Instance.SimpleChatView.UpdateChatInfo(ChatTypeEnum.World);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayMusic, GlobalConst.Sound.MUSIC_MAIN);
        PlayerData.Instance.UpdateLevelEvent -= UpdateLevel;
        PlayerData.Instance.UpdateVipEvent -= UpdateVip;
        PlayerData.Instance.UpdateLevelEvent += UpdateLevel;
        PlayerData.Instance.UpdateVipEvent += UpdateVip;
        //PlayerData.Instance.UpdateLevelEvent -= UpdateLevelUnLock;
        //PlayerData.Instance.UpdateLevelEvent += UpdateLevelUnLock;
        PlayerData.Instance.UpdateMaxCombatPowerEvent -= UpdateMaxFighting;
        PlayerData.Instance.UpdateMaxCombatPowerEvent += UpdateMaxFighting;
        PlayerData.Instance.UpdateVipRewardInfoEvent -= UpdateVipRewardNotice;
        PlayerData.Instance.UpdateVipRewardInfoEvent += UpdateVipRewardNotice;

        //Assets.Script.Common.Scheduler.Instance.AddFrame(1,false,CheckGuide);
        ShowIconChange();
        UpdateIsUnlockRecruit();
        ShowChat(CheckChatStatus());
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_SIMPLECHAT))
        {
            UISystem.Instance.SimpleChatView.UpdateNotify(CheckChatStatus());
        }
        MainCityModule.Instance.GetWaitingNotify();
        UnionModule.Instance.OnSendPlayerUnion();
        UISystem.Instance.HintView.FightIsReady();
        PlayMarquee();
        UpdateChatBtnStatus();
        UnionModule.Instance.ShowWaitingNotify();
        GameActivityModule.Instance.OnSendQueryActivityTimeReq();
        BtnEventBinding();
        ShowMainGateHint(PlayerData.Instance.IsShowGateTip());
        UpdateMaxFighting();
        InitFuncState(MainCityModule.Instance.LockFuncs);
        Scheduler.Instance.AddTimer(0.1f, false, UpdateVipRewardNotice);
        Scheduler.Instance.AddLateUpdator(MoveUpdate);
    }

    private void UpdateVipRewardNotice()
    {
        if (View == null || View.Gobj_VipEffect == null) return;
        if (PlayerData.Instance._VipLv <= 0)
        {
            View.Gobj_VipEffect.gameObject.SetActive(false);
            return;
        }
        VipRewardInfo info = PlayerData.Instance.VipRewardInfo;
        if (info == null)
            return;
        bool canGet = info.level.Count < PlayerData.Instance._VipLv || info.draw_time == 0
                      || CommonFunction.GetDateTime(Main.mTime).Day > CommonFunction.GetDateTime((long)info.draw_time).Day;
        View.Gobj_VipEffect.gameObject.SetActive(canGet);
    }
    public void ForDoubel(List<ActivityTimeInfo> ActivityInfo)
    {
        if (View == null
            || View.Spt_ActivityDouble == null || View.Spt_ActivityDouble.gameObject == null
            || View.Spt_ExpeditionDouble == null || View.Spt_ExpeditionDouble.gameObject == null
            || View.Spt_BattleGroundDouble == null || View.Spt_BattleGroundDouble.gameObject == null)
            return;

        View.Spt_ActivityDouble.gameObject.SetActive(false);
        View.Spt_ExpeditionDouble.gameObject.SetActive(false);
        View.Spt_BattleGroundDouble.gameObject.SetActive(false);

        //List<GameActivityData> data = ConfigManager.Instance.mGameActivityConfig.GetVaildAcitvityListByIDs(ActivityInfo);
        //if (data == null)
        //    return;
        //for (int i = 0; i < data.Count; i++)
        //{
        //    UpdateDoubelHit(data[i]);
        //}

        //GameActivityData tmp;
        for (int i = 0; i < ActivityInfo.Count; i++)
        {
            //tmp = ConfigManager.Instance.mGameActivityConfig.GetDataByID((uint)ActivityInfo[i].id);
            //if (tmp == null)
                //continue;
            UpdateDoubelHit(ActivityInfo[i]);
        }
    }
    public void UpdateDoubelHit(ActivityTimeInfo timeinfo)
    {
        switch ((GameActivityType)timeinfo.activity_type)
        {
            case GameActivityType.DropDouble_Expedition://远征
                View.Spt_BattleGroundDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.ChallengeDouble_Expedition://兵法
                View.Spt_ActivityDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;

            case GameActivityType.DropDouble_Endness://远征
                View.Spt_BattleGroundDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.DropDouble_EventDungeons://兵法
                View.Spt_ActivityDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;

            case GameActivityType.ChallengeDouble_Endness://无尽
                View.Spt_ExpeditionDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.ChallengeDouble_EventDungeons://无尽
                View.Spt_ExpeditionDouble.gameObject.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            default:
                //Debug.LogError(data.mName+"    "+data.mType);
                break;
        }
    }    

    private bool CheckChatStatus()
    {
        if (ChatModule.Instance.privateChatList == null || ChatModule.Instance.privateChatList.Count == 0)
            return false;
        for (int i = 0; i < ChatModule.Instance.privateChatList.Count; i++)
        {
            ChatInfo info = ChatModule.Instance.privateChatList[i];
            if (info.status == 1)
                return true;
        }
        return false;
    }

    private void InitMarquee()
    {
        if (marqueeList == null)
        {
            marqueeList = new List<MarqueeData>();
            MarqueeData data1 = new MarqueeData()
            {
                status = MarqueeState.Idle,
                label = View.Lbl_Marquee1,
            };
            MarqueeData data2 = new MarqueeData()
            {
                status = MarqueeState.Idle,
                label = View.Lbl_Marquee2,
            }; MarqueeData data3 = new MarqueeData()
            {
                status = MarqueeState.Idle,
                label = View.Lbl_Marquee3,
            };
            MarqueeData data4 = new MarqueeData()
            {
                status = MarqueeState.Idle,
                label = View.Lbl_Marquee4,
            };
            MarqueeData data5 = new MarqueeData()
            {
                status = MarqueeState.Idle,
                label = View.Lbl_Marquee5,
            };
            marqueeList.Add(data1);
            marqueeList.Add(data2);
            marqueeList.Add(data3);
            marqueeList.Add(data4);
            marqueeList.Add(data5);
        }

        View.Gobj_MarqueePanel.SetActive(false);
        marqueeState = MarqueeState.Idle;
        marqueePlayStatus = false;
        ResetMarqueeList();
    }

    private void ResetMarqueeList()
    {
        for (int i = 0; i < marqueeList.Count; i++)
        {
            MarqueeData data = marqueeList[i];
            data.status = MarqueeState.Idle;
            data.label.text = string.Empty;
            data.label.transform.localPosition = new Vector3(380, 0, 0);
        }
    }

    public void EnableMainUIDrag(bool enable)
    {
        if (View == null || View.ScrollView_MainCity == null) return;
        View.ScrollView_MainCity.enabled = enable;
    }

    public void LocateMainCity(Vector2 pos)
    {
        View.ScrollView_MainCity.transform.localPosition = new Vector3(pos.x, pos.y, 0);
        View.ScrollView_MainCity.GetComponent<UIPanel>().clipOffset = new Vector2(-pos.x, -pos.y);
        GuideManager.Instance.CheckTrigger(GuideTrigger.MainCityLocCompleted);
    }
    public void ShowChat(bool haveChat)
    {
        View.ChatNoticeAnim.gameObject.SetActive(false);
        //View.ChatNotice.SetActive(haveChat);
        //if (haveChat)
        //{
        //    View.ChatNoticeAnim.state.SetAnimation(0, GlobalConst.OPENCHAT, true);
        //}
        //else
        //{
        //    View.ChatNoticeAnim.state.SetAnimation(0, GlobalConst.OFFCHAT, true);
        //}
    }
    public bool ShowNofity(NotifyRefresh resp)
    {

        bool result = false;
        switch (resp.type)
        {
            case NotifyType.RECRUIT:
                {
                    if (CommonFunction.CheckIsOpen(OpenFunctionType.Recruit))
                    {
                        View.Spt_RecruitNotice.gameObject.SetActive(resp.num > 0);
                        result = true;
                    }
                    break;
                }
            case NotifyType.MAIL:
                {
                    View.MailNotice.gameObject.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.ENSLAVE_RECOED:
                {
                    View.SlaveNotice.gameObject.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.UNION_JION_REQ:
                {
                    bool tmpNum = (resp.num > 0);
                    bool tmpCaptureTerritory = false;
                    if (UnionModule.Instance.HasUnion && !MainCityModule.Instance.LockFuncs.Contains((int)OpenFunctionType.GongCheng))
                    {
                        tmpCaptureTerritory = (CaptureTerritoryModule.Instance.FightState == ECaptureTerritoryFightState.Fighting);
                    }

                    if (tmpNum || tmpCaptureTerritory)
                    {
                        View.Spt_LegionNotice.gameObject.SetActive(true);
                    }
                    else
                    {
                        View.Spt_LegionNotice.gameObject.SetActive(false);
                    }
                    //View.Spt_LegionNotice.gameObject.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.EXTRACT_EQUIP:
                {
                    View.YuanBaoNotice.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.OVERLORD:
                {
                    if (resp.status == 1)
                    {
                        View.BaZhuNotice.SetActive(resp.num > 0);
                        if (resp.num == 1)
                        {
                            SupermacyModule.Instance.ShowSupermacyName(false);
                        }
                    }
                    if (resp.status == 2)
                    {
                        View.BaZhuEffect.SetActive(resp.num > 0);                      
                    }
                                   
                    result = true;
                    break;
                }
            case NotifyType.ALTAR_CHAIRMAN:
                {
                    PlayerData.Instance.FrameID = (uint)resp.num;
                    int id = CommonFunction.FilterFramId((int)PlayerData.Instance.FrameID);
                    string Frame_B = string.Format(GlobalConst.SpriteName.Frame_Name_B, id);
                    CommonFunction.SetSpriteName(View.HeadUIBG, Frame_B);
                    if (id == PlayerData.Instance.FrameID)
                        this.View.HeadState.gameObject.SetActive(false);
                    else
                        this.View.HeadState.gameObject.SetActive(true);
                    result = true;
                    break;
                }
        }
        return result;
    }
    #region 功能逐步开放 元宝阁，囚牢，特权商店，天降神兵

    private void InitFuncState(List<uint> funcList)
    {
        if (funcList == null)
            return;
        for (int i = 0; i < funcList.Count; i++)
        {
            SetFuncState((OpenFunctionType)funcList[i]);
        }
    }

    private void SetFuncState(OpenFunctionType func)
    {
        switch (func)
        {
            case OpenFunctionType.YuanBaoGe:
                {
                    View.Gobj_YuanBaoTitle.SetActive(false);
                    View.Btn_YuanBao.gameObject.SetActive(false);
                    break;
                }
            case OpenFunctionType.Slave:
                {
                    View.Gobj_SlaveTitle.SetActive(false);
                    View.Btn_SlaveButton.gameObject.SetActive(false);
                    break;
                }
            case OpenFunctionType.Magical:
                {
                    View.Gobj_MagicalTitle.SetActive(false);
                    View.Btn_Magical.gameObject.SetActive(false);
                    break;
                }
        }
    }

    #endregion

    #region 建筑解锁
    private enum UnlockFuncState
    {
        Done,
        Moveing,
        ShowingEffect
    }
    private UnlockFuncState _unlockState = UnlockFuncState.Done;
    private List<int> _openFuncs = new List<int>();
    public float _moveSpeed = 1300;
    private Vector3 _effectPos;
    private Vector3 _moveStartPos;
    private Vector3 _moveEndPos;
    private float _moveFactor = 0;
    private float _moveDis = 0;

    public iTween.EaseType EaseType = iTween.EaseType.easeInOutQuad;
    public void CheckUnlockBuilding(List<int> openFuncs)
    {
        return;
        _openFuncs.AddRange(openFuncs);
        openFuncs.Clear();
        if (_unlockState == UnlockFuncState.Done)
        {
            BeginUnlock();
        }
    }

    private int GetOpenFunc()
    {
        int result = 0;
        if (_openFuncs.Count > 0)
        {
            result = _openFuncs[0];
        }
        return result;
    }

    private void BeginUnlock()
    {
        if (View == null)
            return;
        if (GuideManager.Instance.GuideIsRunning())
            return;
        int funcId = GetOpenFunc();
        if (funcId == 0) return;
        MainCityUnlockData data = ConfigManager.Instance.mMainCityUnlockConfig.GetDataByFuncID(funcId);
        _moveStartPos = View.ScrollView_MainCity.transform.localPosition;
        _moveEndPos = new Vector3(data.mCityPos, 0, 0);
        _moveFactor = 0;
        _moveDis = Mathf.Abs(data.mCityPos - _moveStartPos.x);
        if (data.mBuildingLayer == 1)
        {
            _effectPos = data.mBuildingPos + _moveEndPos;
        }
        else
        {
            _effectPos = data.mBuildingPos + _moveEndPos * View.Layer2Relative.factor;
        }
        _moveEndPos = _moveEndPos * View.Tran_MainCityFG.localScale.x;
        _effectPos = _effectPos * View.Tran_MainCityFG.localScale.x;

        _unlockState = UnlockFuncState.Moveing;
        EnableMainUIDrag(false);
        UISystem.Instance.ShowGameUI(RechargeWebMask.UIName);
        UISystem.Instance.RechargeWebMask.SetState(false);
        //Scheduler.Instance.AddLateUpdator(MoveUpdate);
        iTween.MoveTo(View.ScrollView_MainCity.gameObject, iTween.Hash("position", _moveEndPos, "islocal", true,
            "time", _moveDis / _moveSpeed, "easetype", EaseType));
        Scheduler.Instance.AddTimer(_moveDis / _moveSpeed + 0.1f, false, EndMove);
    }

    private void MoveUpdate()
    {
        //if (_unlockState != UnlockFuncState.Moveing) return;
        View.Pnl_MainCity.clipOffset = new Vector2(-View.ScrollView_MainCity.transform.localPosition.x, 0);
    }

    public void EndMove()
    {
        //Scheduler.Instance.RemoveLateUpdator(MoveUpdate);
        ShowUnlockEffectFore();
    }


    private void ShowUnlockEffectFore()
    {
        if (View == null || View.Gobj_UnlockEffectFore == null)
        {
            EnableMainUIDrag(true);
            _unlockState = UnlockFuncState.Done;
            return;
        }
        _unlockState = UnlockFuncState.ShowingEffect;
        View.Gobj_UnlockEffectFore.transform.localPosition = _effectPos;
        View.Gobj_UnlockEffectFore.SetActive(false);
        View.Gobj_UnlockEffectFore.SetActive(true);
        Scheduler.Instance.AddTimer(0f, false, ShowUnlockEffect);
    }

    private void ShowUnlockEffect()
    {
        if (View == null)
        {
            return;
        }
        if (View.Gobj_UnlockEffect != null)
        {
            View.Gobj_UnlockEffect.transform.localPosition = _effectPos;
            View.Gobj_UnlockEffect.SetActive(false);
            View.Gobj_UnlockEffect.SetActive(true);
        }
        Scheduler.Instance.AddTimer(0.8f, false, ClearUnlockTxt);
        Scheduler.Instance.AddTimer(4.5f, false, FinishUnlock);
    }

    private void ClearUnlockTxt()
    {
        UnlockBuildingTitle((OpenFunctionType)_openFuncs[0]);
    }

    private void FinishUnlock()
    {
        EnableMainUIDrag(true);
        UISystem.Instance.CloseGameUI(RechargeWebMask.UIName);
        _unlockState = UnlockFuncState.Done;
        if (View.Gobj_UnlockEffect != null)
            View.Gobj_UnlockEffect.SetActive(false);
        if (View.Gobj_UnlockEffectFore != null)
            View.Gobj_UnlockEffectFore.SetActive(false);
        MainCityModule.Instance.SendBuildingEffectRecord(false, new List<int>() { _openFuncs[0] });
        _openFuncs.RemoveAt(0);
        if (_openFuncs.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUILDING_UNLOCK_TIP);
            MainCityModule.Instance.ShowGoToCommentView();
        }
        else
        {
            BeginUnlock();
        }
    }

    public void UnlockBuildingTitle(OpenFunctionType func)
    {
        switch (func)
        {
            case OpenFunctionType.Endless:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Endless, false))
                    {
                        View.Lab_Expedition.gameObject.SetActive(false);
                        View.Spt_Expedition.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Activity:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Activity, false))
                    {
                        View.Lab_Activity.gameObject.SetActive(false);
                        View.Spt_Activity.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Expedition:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Expedition, false))
                    {
                        View.Lab_Battleground.gameObject.SetActive(false);
                        View.Spt_Battleground.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Rank:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Rank, false))
                    {
                        View.Lab_Billboard.gameObject.SetActive(false);
                        View.Spt_Billboard.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Slave:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Slave, false))
                    {
                        View.Lab_Slave.gameObject.SetActive(false);
                        View.Spt_Slave.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Sociaty:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Sociaty, false))
                    {
                        View.Lab_Legion.gameObject.SetActive(false);
                        View.Spt_Legion.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Arena:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Arena, false))
                    {
                        View.Lab_Arena.gameObject.SetActive(false);
                        View.Spt_Arena.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Magical:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Magical, false))
                    {
                        View.Lab_MagicalLab.gameObject.SetActive(false);
                    }
                    break;
                }
            case OpenFunctionType.YuanBaoGe:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.YuanBaoGe, false))
                    {
                        View.Lab_YuanBao.gameObject.SetActive(false);
                        View.Spt_YuanBao.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.BaZhu:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.BaZhu, false))
                    {
                        View.Lab_BaZhu.gameObject.SetActive(false);
                        View.Spt_BaZhu.gameObject.SetActive(true);
                    }
                    break;
                }
            case OpenFunctionType.Qualifying:
                {
                    if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Qualifying, false))
                    {
                        View.Lbl_PaiWeiSai.gameObject.SetActive(false);
                        View.Spt_PaiWeiSai.gameObject.SetActive(true);
                    }
                    break;
                }
        }
    }

    #endregion

    #region 更新信息

    public void UpdateChatBtnStatus()
    {
        List<ChatInfo> corps = ChatModule.Instance.corpsChatList;
        List<ChatInfo> privates = ChatModule.Instance.privateChatList;
        bool status = false;
        if (corps != null)
        {
            for (int i = 0; i < corps.Count; i++)
            {
                ChatInfo info = corps[i];
                if (info == null) continue;
                if (info.status == 1)
                {
                    status = true;
                    break;
                }
            }
        }
        if ((privates != null) && (status == false))
        {
            for (int i = 0; i < privates.Count; i++)
            {
                ChatInfo info = privates[i];
                if (info == null) continue;
                if (info.status == 1)
                {
                    status = true;
                    break;
                }
            }
        }
        ShowChat(status);
    }

    public void PlayMarquee()
    {
        switch (marqueeState)
        {
            case MarqueeState.Idle:
                StartMarquee();
                break;
            case MarqueeState.Playing:
            case MarqueeState.NextPlay:
                break;
        }
    }

    public void StartMarquee()
    {
        marqueePlayStatus = false;
        if (MainCityModule.Instance.backpackMarqueeQueue.Count <= 0 && MainCityModule.Instance.arenaMarqueeQueue.Count <= 0 && MainCityModule.Instance.systemOepnMarqueeQueue.Count <= 0 && MainCityModule.Instance.otherMarqueeQueue.Count <= 0 && MainCityModule.Instance.supermacyMarqueeQueue.Count <= 0)
        {
            if (marqueeState != MarqueeState.Playing && marqueeState != MarqueeState.PlayEnd)
            {
                marqueeState = MarqueeState.Idle;
            }
        }
        else
        {
            marqueeState = MarqueeState.Playing;
        }
        switch (marqueeState)
        {
            case MarqueeState.Idle:
                {
                    View.Gobj_MarqueePanel.SetActive(false);
                    Scheduler.Instance.RemoveUpdator(PlayMarqueeAnimation);
                }
                break;
            case MarqueeState.Playing:
                {
                    List<NotifyMarquee> list = null;
                    if (MainCityModule.Instance.backpackMarqueeQueue.Count > 0)
                    {
                        list = MainCityModule.Instance.backpackMarqueeQueue;
                    }
                    else if (MainCityModule.Instance.arenaMarqueeQueue.Count > 0)
                    {
                        list = MainCityModule.Instance.arenaMarqueeQueue;
                    }
                    else if (MainCityModule.Instance.systemOepnMarqueeQueue.Count > 0)
                    {
                        list = MainCityModule.Instance.systemOepnMarqueeQueue;
                    }
                    else if (MainCityModule.Instance.otherMarqueeQueue.Count > 0)
                    {
                        list = MainCityModule.Instance.otherMarqueeQueue;
                    }
                    else if (MainCityModule.Instance.supermacyMarqueeQueue.Count > 0)
                    {
                        list = MainCityModule.Instance.supermacyMarqueeQueue;
                    }
                    if (list == null || list.Count <= 0) return;
                    NotifyMarquee data = list[0];
                    if (data == null) return;
                    bool enable = false;
                    for (int i = 0; i < marqueeList.Count; i++)
                    {
                        MarqueeData marqueeObject = marqueeList[i];
                        if (marqueeObject.status == MarqueeState.Idle)
                        {
                            enable = true;
                            marqueeObject.status = MarqueeState.Playing;
                            marqueeObject.label.supportEncoding = true;
                            marqueeObject.label.text = data.body;
                            break;
                        }
                    }
                    if (enable)
                    {
                        View.Gobj_MarqueePanel.SetActive(true);
                        Scheduler.Instance.AddUpdator(PlayMarqueeAnimation);
                        list.RemoveAt(0);
                    }
                }
                break;
            case MarqueeState.PlayEnd:
                {
                    marqueeState = MarqueeState.Idle;
                    ResetMarqueeList();
                    View.Gobj_MarqueePanel.SetActive(false);
                    Scheduler.Instance.RemoveUpdator(PlayMarqueeAnimation);
                } break;
        }

    }
    private void PlayMarqueeAnimation()
    {
        marqueePlayStatus = false;
        for (int i = 0; i < marqueeList.Count; i++)
        {
            MarqueeData data = marqueeList[i];
            if (data.label.transform.localPosition.x < (-380 - data.label.width))
            {
                data.status = MarqueeState.Idle;
            }

            switch (data.status)
            {
                case MarqueeState.Idle:
                    {
                        data.label.enabled = false;
                        data.label.transform.localPosition = new Vector3(380, 0, 0);
                    } break;
                case MarqueeState.Playing:
                    {
                        data.label.enabled = true;
                        if (data.label.transform.localPosition.x < (180 - data.label.width))
                        {
                            data.status = MarqueeState.NextPlay;
                        }
                        data.label.transform.localPosition += Vector3.left * 50 * Time.deltaTime;
                    } break;
                case MarqueeState.NextPlay:
                    {
                        data.label.enabled = true;
                        data.status = MarqueeState.PlayEnd;
                        StartMarquee();
                    } break;
                case MarqueeState.PlayEnd:
                    {
                        data.label.enabled = true;
                        data.label.transform.localPosition += Vector3.left * 50 * Time.deltaTime;
                        if (data.label.transform.localPosition.x < (-380 - data.label.width))
                        {
                            data.status = MarqueeState.Idle;
                        }
                    }
                    break;
            }
            if (data.status != MarqueeState.Idle)
            {
                marqueePlayStatus = true;
            }
        }
        if (!marqueePlayStatus)
        {
            marqueeState = MarqueeState.PlayEnd;
            StartMarquee();
        }
    }

    public void UpdateLevelUnLock()
    {
        //if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Store, false))
        {
            View.Lab_Shop.gameObject.SetActive(false);
            //CommonFunction.SetLabelColor(View.ShopLab , 247, 206, 0, 255, 36, 18, 13, 255);
            //CommonFunction.UpdateWidgetGray(View.Spt_Shop ,false);
            //CommonFunction.SetLabelColor_I(View.ShopLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        //else
        //{
        //View.Lab_Shop.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Store).openLevel);
        //View.Lab_Shop.gameObject.SetActive(true);
        //// CommonFunction.UpdateWidgetGray(View.Spt_Shop, true);
        //CommonFunction.SetLabelColor_I(View.ShopLab, 255, 255, 255, 255, 36, 18, 13, 255);

        //}

        #region 无尽战场
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Endless, false))
        {
            View.Lab_Expedition.gameObject.SetActive(false);
            View.Spt_Expedition.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Expedition.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Endless).openLevel);
            View.Lab_Expedition.gameObject.SetActive(true);
            View.Spt_Expedition.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.ExpeditionLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 兵伐不臣
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Activity, false))
        {
            View.Lab_Activity.gameObject.SetActive(false);
            View.Spt_Activity.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Activity.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Activity).openLevel);
            View.Lab_Activity.gameObject.SetActive(true);
            CommonFunction.SetLabelColor_I(View.ActivityLab, 255, 255, 255, 255, 36, 18, 13, 255);
            View.Spt_Activity.gameObject.SetActive(false);
        }
        #endregion
        #region 远征天下
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Expedition, false))
        {
            View.Lab_Battleground.gameObject.SetActive(false);
            View.Spt_Battleground.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Battleground.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Expedition).openLevel);
            View.Lab_Battleground.gameObject.SetActive(true);
            View.Spt_Battleground.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.BattlegroundLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 排行榜
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Rank, false))
        {
            View.Lab_Billboard.gameObject.SetActive(false);
            View.Spt_Billboard.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Billboard.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Rank).openLevel);
            View.Lab_Billboard.gameObject.SetActive(true);
            View.Spt_Billboard.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.BillboardLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 奴隶
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Slave, false))
        {
            View.Lab_Slave.gameObject.SetActive(false);
            View.Spt_Slave.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Slave.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Slave).openLevel);
            View.Lab_Slave.gameObject.SetActive(true);
            View.Spt_Slave.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.SlaveLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 军团
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Sociaty, false))
        {
            View.Lab_Legion.gameObject.SetActive(false);
            View.Spt_Legion.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Legion.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Sociaty).openLevel);
            View.Lab_Legion.gameObject.SetActive(true);
            View.Spt_Legion.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.LegionLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 演武台
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Arena, false))
        {
            View.Lab_Arena.gameObject.SetActive(false);
            View.Spt_Arena.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_Arena.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Arena).openLevel);
            View.Lab_Arena.gameObject.SetActive(true);
            View.Spt_Arena.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.ArenaLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 天将神兵
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Magical, false))
        {
            View.Lab_MagicalLab.gameObject.SetActive(false);
        }
        else
        {
            View.Lab_MagicalLab.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Magical).openLevel);
            View.Lab_MagicalLab.gameObject.SetActive(true);
            CommonFunction.SetLabelColor_I(View.Lab_MagicalLab, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 缘宝阁
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.YuanBaoGe, false))
        {
            View.Lab_YuanBao.gameObject.SetActive(false);
            View.Spt_YuanBao.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_YuanBao.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.YuanBaoGe).openLevel);
            View.Lab_YuanBao.gameObject.SetActive(true);
            View.Spt_YuanBao.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.Lab_YuanBao, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 全服霸主
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.BaZhu, false))
        {
            View.Lab_BaZhu.gameObject.SetActive(false);
            View.Spt_BaZhu.gameObject.SetActive(true);
        }
        else
        {
            View.Lab_BaZhu.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.BaZhu).openLevel);
            View.Lab_BaZhu.gameObject.SetActive(true);
            View.Spt_BaZhu.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.Lab_BaZhu, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
        #region 排位赛
        if (CommonFunction.CheckFuncIsOpen(OpenFunctionType.Qualifying, false))
        {
            View.Lbl_PaiWeiSai.gameObject.SetActive(false);
            View.Spt_PaiWeiSai.gameObject.SetActive(true);
        }
        else
        {
            View.Lbl_PaiWeiSai.text = string.Format(ConstString.UNLOCK_TEXT, ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Qualifying).openLevel);
            View.Lbl_PaiWeiSai.gameObject.SetActive(true);
            View.Spt_PaiWeiSai.gameObject.SetActive(false);
            CommonFunction.SetLabelColor_I(View.Lbl_PaiWeiSai, 255, 255, 255, 255, 36, 18, 13, 255);
        }
        #endregion
    }
    public void UpdateIsUnlockRecruit()
    {
        if (CommonFunction.CheckIsOpen(OpenFunctionType.Recruit))
        {
            View.AgainstLab.gameObject.SetActive(false);
            View.Spt_Recruit.gameObject.SetActive(true);
        }
        else
        {
            OpenLevelData tmp = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Recruit);
            if (tmp != null && tmp.gateId != -1)
            {
                StageInfo tmpInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)tmp.gateId);
                if (tmpInfo != null)
                {
                    ChapterInfo tmpCInfo = ConfigManager.Instance.mChaptersData.GetChapterByID(tmpInfo.ChapterID);
                    if (tmpCInfo != null)
                    {
                        View.AgainstLab.gameObject.SetActive(true);
                        View.AgainstLab.text = string.Format(ConstString.FUNCTION_MAINCITYLOCK, tmpCInfo.name, tmpInfo.SmallGateID);
                        View.Spt_Recruit.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
    public void UpdatePlayerHeadUI()//更新人物
    {
        //UpdateExp();
        UpdateVip();
        UpdateName();
        UpdateLevel();
        UpdateHeadIcon();
        //UpdateMaxFighting();
    }
    public void UpdateMaxFighting()
    {
        if (View.Lab_MaxFighting != null)
            View.Lab_MaxFighting.text = PlayerData.Instance.MaxCombatPower.ToString();
    }
    public void UpdateExp()
    {
        //PlayerData.Instance._CurrentExp;
        float NowExp = (float)PlayerData.Instance._CurrentExp / PlayerData.Instance._NextLvExp;
        View.HeadExp_Label.text = string.Format(ConstString.PLAYEREXP, ConversionNum(NowExp));
        if (NowExp > 0 && NowExp < 0.01)
        {
            View.HeadExp_Slider.value = 0.001F;
        }
        else
        {
            View.HeadExp_Slider.value = NowExp;
        }
    }
    public int ConversionNum(float Num)
    {
        int NewNum = 0;
        if (Num <= 0) NewNum = 0;
        if (Num >= 1) NewNum = 100;
        else
        {
            if (Num > 0 && Num < 0.01)
            {
                return NewNum = 1;
            }
            NewNum = (int)(Num * 100);
        }
        return NewNum;
    }
    public void UpdateVip()
    {
        View.Lab_PlayerVip.text = "VIP" + PlayerData.Instance._VipLv.ToString();
    }
    public void UpdateName()
    {
        View.Lab_PlayerName.text = PlayerData.Instance._NickName.ToString();
    }
    public void UpdateLevel()
    {
        View.Lab_PlayerLevel.text = PlayerData.Instance._Level.ToString();
        UpdateExp();
        UpdateMaxFighting();
    }
    public void UpdateHeadIcon()
    {
        //设置人物头像ICON       
        //CommonFunction.SetSpriteName(View.Spt_HeadIcon, CommonFunction.GetHeroIconNameByGender((EHeroGender)PlayerData.Instance._Gender,true ));
        string icon = CommonFunction.GetHeroIconNameByID(PlayerData.Instance.HeadID, false);
        //Debug.LogError("1" + icon);
        CommonFunction.SetSpriteName(View.Spt_HeadIcon, icon);
        View.Spt_HeadIcon.width = 109;
        View.Spt_HeadIcon.height = 87;
    }


    public void ShowIconChange()
    {
        int id = CommonFunction.FilterFramId((int)PlayerData.Instance.FrameID);
        string Frame_B = string.Format(GlobalConst.SpriteName.Frame_Name_B, id);
        string Frame_D = string.Format(GlobalConst.SpriteName.Frame_Name_D, id);
        string Frame_C = string.Format(GlobalConst.SpriteName.Frame_Name_C, id);
        string Frame_F = string.Format(GlobalConst.SpriteName.Frame_Name_F, id);
        CommonFunction.SetSpriteName(View.HeadUIBG, Frame_B);
        CommonFunction.SetSpriteName(View.HeadVipBG, Frame_C);
        CommonFunction.SetSpriteName(View.HeadExpBG, Frame_D);
        CommonFunction.SetSpriteName(View.HeadFightBG, Frame_F);
        if (id == PlayerData.Instance.FrameID)
            this.View.HeadState.gameObject.SetActive(false);
        else
            this.View.HeadState.gameObject.SetActive(true);
    }
    #endregion
    #region 绑定事件
    private void BtnEventBinding()
    {
        UIEventListener.Get(View.Btn_BaZhu.gameObject).onClick = ButtonEvent_BaZhu_Button;
        UIEventListener.Get(View.Btn_HeadVIP.gameObject).onClick = ButtonEvent_HeadVIP_Button;
        UIEventListener.Get(View.Btn_YuanBao.gameObject).onClick = ButtonEvent_YuanBao_Button;
        UIEventListener.Get(View.Btn_HeadUI.gameObject).onClick = ButtonEvent_HeadUIButton;
        //UIEventListener.Get(View.Btn_CoinButton.gameObject).onClick = ButtonEvent_BuyCoinButton;
        //UIEventListener.Get(View.Btn_GemButton.gameObject).onClick = ButtonEvent_BuyGemButton;
        //UIEventListener.Get(View.Btn_SPIconButton.gameObject).onClick = ButtonEvent_BuySPButton;
        //UIEventListener.Get(View.Btn_SPLabelButton.gameObject).onPress = ButtonEvent_ClickSPButton;//体力提示

        //UIEventListener.Get(View.Btn_SignInButton.gameObject).onClick = ButtonEvent_SignInButton;
        //UIEventListener.Get(View.Btn_RechargeButton.gameObject).onClick = ButtonEvent_RechargeButton;
        //UIEventListener.Get(View.Btn_NoticeButton.gameObject).onClick = ButtonEvent_NoticeButton;
        //UIEventListener.Get(View.Btn_TaskButton.gameObject).onClick = ButtonEvent_TaskButton;
        //UIEventListener.Get(View.Btn_ActiveButton.gameObject).onClick = ButtonEvent_ActiveButton;

        //UIEventListener.Get(View.Btn_MailButton.gameObject).onClick = ButtonEvent_MailButton;
        //UIEventListener.Get(View.Btn_OpenMenuButton.gameObject).onClick = ButtonEvent_MenuButton;
        //UIEventListener.Get(View.Btn_HeroButton.gameObject).onClick = ButtonEvent_HeroButton;
        //UIEventListener.Get(View.Btn_GeneralButton.gameObject).onClick = ButtonEvent_GeneralButton;
        //UIEventListener.Get(View.Btn_BagButton.gameObject).onClick = ButtonEvent_BagButton;

        //UIEventListener.Get(View.Btn_ShopButton.gameObject).onClick = ButtonEvent_ShopButton;
        UIEventListener.Get(View.Btn_ChatButton.gameObject).onClick = ButtonEvent_ChatButton;

        UIEventListener.Get(View.Btn_MainGateButton.gameObject).onClick = ButtonEvent_MainGateButton;
        UIEventListener.Get(View.Btn_ActivitiesButton.gameObject).onClick = ButtonEvent_ActivitiesButton;
        UIEventListener.Get(View.Btn_SlaveButton.gameObject).onClick = ButtonEvent_SlaveButton;
        UIEventListener.Get(View.Btn_LegionButton.gameObject).onClick = ButtonEvent_LegionButton;
        UIEventListener.Get(View.Btn_ArenaButton.gameObject).onClick = ButtonEvent_ArenaButton;

        UIEventListener.Get(View.Btn_ExpeditionButton.gameObject).onClick = ButtonEvent_ExpeditionButton;
        UIEventListener.Get(View.Btn_BillboardButton.gameObject).onClick = ButtonEvent_BillboardButton;
        UIEventListener.Get(View.Btn_BattlegroundButton.gameObject).onClick = ButtonEvent_BattlegroundButton;
        UIEventListener.Get(View.Btn_AgainstButton.gameObject).onClick = ButtonEvent_AgainstButton;
        UIEventListener.Get(View.Btn_ShopButton.gameObject).onClick = ButtonEvent_ShopButton;

        UIEventListener.Get(View.Btn_MailButton.gameObject).onClick = ButtonEvent_MailButton;
        UIEventListener.Get(View.Btn_Grass_1.gameObject).onClick = ButtonEvent_Grass_1_Button;
        UIEventListener.Get(View.Btn_Grass_2.gameObject).onClick = ButtonEvent_Grass_2_Button;
        UIEventListener.Get(View.Btn_Grass_3.gameObject).onClick = ButtonEvent_Grass_3_Button;
        UIEventListener.Get(View.Btn_Grass_4.gameObject).onClick = ButtonEvent_Grass_4_Button;
        UIEventListener.Get(View.Btn_PaiWeiSai.gameObject).onClick = ButtonEvent_PaiWeiSai;

        UIEventListener.Get(View.Btn_Magical.gameObject).onClick = ButtonEvent_Magical_Button;
    }
    #endregion
    #region 点击事件
    public void ButtonEvent_PaiWeiSai(GameObject Btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Qualifying))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_QUALIFYING);
    }
    public void ButtonEvent_BaZhu_Button(GameObject Btn)
    {
        //Debug.LogError("s");
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.BaZhu))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SUPERMACY);
    }

    public void ButtonEvent_HeadVIP_Button(GameObject Btn)
    {
        //Debug.LogError("sssssssssssss");
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECHARGE);
        UISystem.Instance.VipRechargeView.ShowVipPrivilege(PlayerData.Instance._VipLv);
    }
    public void ButtonEvent_YuanBao_Button(GameObject Btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.YuanBaoGe))
            return;

        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_DROWEQUIPVIEW);
    }
    public void ButtonEvent_Magical_Button(GameObject btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Magical))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SACRIFICIAL);
        //if (CommonFunction.CheckIsOpen(OpenFunctionType.Magical,true ))
        //{
        //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SACRIFICIAL);
        //}
    }
    public void ButtonEvent_Grass_1_Button(GameObject btn)
    {
        View.SkeletonGrass_1.Reset();
        //View.SkeletonGrass_1.AnimationName = ConstString.ANIM_NAME;
        //View.SkeletonGrass_1.
        View.SkeletonGrass_1.state.SetAnimation(0, ConstString.ANIM_NAME, false);
    }
    public void ButtonEvent_Grass_2_Button(GameObject btn)
    {
        View.SkeletonGrass_2.Reset();
        View.SkeletonGrass_2.state.SetAnimation(0, ConstString.ANIM_NAME, false);

    }
    public void ButtonEvent_Grass_3_Button(GameObject btn)
    {
        //View.SkeletonGrass_3.Reset();
        View.SkeletonGrass_3.state.SetAnimation(0, ConstString.ANIM_NAME, false);

    }
    public void ButtonEvent_Grass_4_Button(GameObject btn)
    {
        //View.SkeletonGrass_4.Reset();
        View.SkeletonGrass_4.state.SetAnimation(0, ConstString.ANIM_NAME, false);
    }
    public void ButtonEvent_MailButton(GameObject btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Mail))
            return;
        //Main.Instance.StartCoroutine(OpenMail(DelayOpenTime));
        OpenMail(DelayOpenTime);
    }

    public void ButtonEvent_ShopButton(GameObject btn)
    {
        //if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Store))
        //    return;
        //Main.Instance.StartCoroutine(OpenStore(DelayOpenTime));
        OpenStore(DelayOpenTime);
    }
    public void ButtonEvent_ActivitiesButton(GameObject Btn)  //活动副本
    {
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Activity))
            return;
        //Main.Instance.StartCoroutine(OpenActivities(DelayOpenTime));
        OpenActivities(DelayOpenTime);
    }
    public void ButtonEvent_MainGateButton(GameObject btn)    //主线副本
    {
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.PVE))
            return;
        //Main.Instance.StartCoroutine(OpenMainGate(DelayOpenTime));
        OpenMainGate(DelayOpenTime);
        //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE, EShowUIType.Default);
        //UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
    }
    public void ButtonEvent_SlaveButton(GameObject Btn)       //奴隶系统
    {
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Slave))
            return;
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,"奴隶集中营:暂未开放！");
        //Main.Instance.StartCoroutine(OpenSlave(DelayOpenTime));
        OpenSlave(DelayOpenTime);
    }
    public void ButtonEvent_LegionButton(GameObject Btn)      //军团系统
    {
        //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        //UISystem.Instance.ShowGameUI(GMView.UIName); return;

        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, "军团系统暂未开放");
        //return;

        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Sociaty))
            return;
        //Main.Instance.StartCoroutine(OpenLegion(DelayOpenTime));
        OpenLegion(DelayOpenTime);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,"军团系统：暂未开放！");
    }
    public void ButtonEvent_ArenaButton(GameObject Btn)       //演武台
    {
        //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Arena))
            return;
        //Main.Instance.StartCoroutine(OpenArena(DelayOpenTime));
        OpenArena(DelayOpenTime);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"演武台：不是演舞台？");
    }
    public void ButtonEvent_ExpeditionButton(GameObject Btn)  //无尽
    {
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));

        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"远征天下：公公与板栗爱の旅行");
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Endless))
            return;
        //Main.Instance.StartCoroutine(OpenEndless(DelayOpenTime));
        OpenEndless(DelayOpenTime);
    }
    public void ButtonEvent_BillboardButton(GameObject Btn)   //排行榜
    {
        //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Rank))
            return;
        //Main.Instance.StartCoroutine(OpenBillboard(DelayOpenTime));
        OpenBillboard(DelayOpenTime);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"排行榜：福布斯等着你");
    }
    public void ButtonEvent_BattlegroundButton(GameObject Btn)//远征
    {
        //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Expedition))
            return;
        //Main.Instance.StartCoroutine(OpenExpedition(DelayOpenTime));
        OpenExpedition(DelayOpenTime);
    }
    public void ButtonEvent_AgainstButton(GameObject Btn)     //招贤系统
    {
        // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.Recruit, true))
            return;
        //Main.Instance.StartCoroutine(OpenAgainst(DelayOpenTime));
        OpenAgainst(DelayOpenTime);
    }
    private void ButtonEvent_HeadUIButton(GameObject Btn)  //设置界面
    {
        //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.SystemSetting))
            return;
        UISystem.Instance.ShowGameUI(SystemSettingView.UIName);
        UISystem.Instance.SystemSettingView.UpdateSettingUI();
    }
    //private void ButtonEvent_BuyCoinButton(GameObject Btn) //购买金币
    //{
    //    //   CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "购买金币");
    //}
    //private void ButtonEvent_BuyGemButton(GameObject Btn)  //购买代币
    //{
    //    //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "购买代币");
    //}
    //private void ButtonEvent_BuySPButton(GameObject Btn)   //购买体力
    //{
    //    // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    int GemNum = 50; int SPNum = 100; int BuyNum = 0;
    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "花费" + GemNum + "钻石，补充" + SPNum + "体力\n（今天已购买" + BuyNum + "次）");
    //}
    //private void ButtonEvent_ClickSPButton(GameObject Btn, bool isPressed) //体力提示
    //{
    //    // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    if (isPressed)
    //    {//按下
    //        View.GO_SPHint.SetActive(true);
    //        UpdateSPHint();
    //    }
    //    else
    //    {//抬起
    //        View.GO_SPHint.SetActive(false);
    //    }

    //}
    //private void ButtonEvent_SignInButton(GameObject Btn)  //签到界面
    //{
    //    ///  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"签到界面");
    //    UISystem.Instance.ShowGameUI(SignView.UIName);
    //}
    //private void ButtonEvent_RechargeButton(GameObject Btn)//充值界面
    //{
    //    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"充值界面");
    //    // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
    //    UISystem.Instance.VipRechargeView.ShowRecharge();
    //}
    //private void ButtonEvent_NoticeButton(GameObject Btn)  //公告界面
    //{
    //    //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "公告界面");
    //}
    //private void ButtonEvent_TaskButton(GameObject Btn)    //任务界面
    //{
    //    // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TASKVIEW);
    //}
    //private void ButtonEvent_ActiveButton(GameObject Btn)  //活跃界面
    //{
    //    //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(LivenessView.UIName);
    //    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "活跃界面");
    //}
    //private void ButtonEvent_MailButton(GameObject Btn)    //邮件界面
    //{
    //    //CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    MailModule.Instance.OpenMailView();

    //}
    //private void ButtonEvent_MenuButton(GameObject Btn)    //菜单按钮
    //{
    //    // CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));

    //    float MoveTime = 0.5F;//菜单开启速度
    //    if (IsOpenMenus)
    //    {//开启 -> 关闭
    //        View.Btn_OpenMenuButton.transform.Rotate(0, 0, -180);
    //        iTween.MoveTo(View.GO_Menus, View.GO_MoveTarget.transform.position, MoveTime);
    //        IsOpenMenus = false;
    //    }
    //    else
    //    {//关闭 -> 开启
    //        View.Btn_OpenMenuButton.transform.Rotate(0, 0, 180);
    //        iTween.MoveTo(View.GO_Menus, Vector3.zero, MoveTime);
    //        IsOpenMenus = true;
    //    }
    //}
    //private void ButtonEvent_HeroButton(GameObject Btn)    //英雄界面
    //{
    //    //        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));

    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
    //}
    //private void ButtonEvent_GeneralButton(GameObject Btn) //武将界面
    //{
    //    //      CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERATT);
    //}
    //private void ButtonEvent_BagButton(GameObject Btn)     //背包界面
    //{
    //    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BACKPACK);
    //}
    //private void ButtonEvent_ShopButton(GameObject Btn)    //商店界面
    //{
    //    //  CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
    //    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok,"商店界面");
    //    UISystem.Instance.ShowGameUI(StoreView.UIName);
    //    UISystem.Instance.StoreView.ShowStore(ShopType.ST_NomalShop);
    //}
    private void ButtonEvent_ChatButton(GameObject Btn)    //聊天界面
    {
        //CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, View._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Chat))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHATVIEW);
        UISystem.Instance.ChatView.UpdateViewInfo();

        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "聊天界面");
    }
    #endregion
    #region 延迟事件
    private void OpenMail(float opentime)
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(MailView.UIName);
        UISystem.Instance.MailView.OnSendGetMail();
    }
    private void OpenStore(float opentime)
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(MallView.UIName);
    }
    private void OpenMainGate(float opentime)//主界面
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATE);
        UISystem.Instance.GateView.UpdateViewInfo();
        //UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
    }
    private void OpenActivities(float opentime)//活动
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ACTIVITIES);
    }
    private void OpenSlave(float opentime)//奴隶
    {
        //yield return new WaitForSeconds(opentime);

        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PRISONVIEW);
    }
    private void OpenLegion(float opentime)//军团
    {
        //yield return new WaitForSeconds(opentime);
        // UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, "军团系统：暂未开放！");
        UnionModule.Instance.OpenUnion();
    }
    private void OpenArena(float opentime)//演武台
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PVPVIEW);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "演武台：不是演舞台？");

    }
    private void OpenExpedition(float opentime)//远征
    {
        //yield return new WaitForSeconds(opentime);
        //UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_MAINCITY);
        FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.None);
        //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXPEDITION);

        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "远征天下：公公与板栗爱の旅行");

    }
    private void OpenBillboard(float opentime)//公告
    {
        //yield return new WaitForSeconds(opentime);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "排行榜：福布斯等着你");
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RANKVIEW);
        UISystem.Instance.RankView.ShowRank(RankType.LEVEL_RANK);
    }
    private void OpenEndless(float opentime)//无尽
    {
        //yield return new WaitForSeconds(opentime);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ENDLESS);
        //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
    }
    private void OpenAgainst(float opentime)//招贤
    {
        //yield return new WaitForSeconds(opentime);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "招贤馆:闲人客栈");
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.Recruit))
        {
            OpenLevelData tmp = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Recruit);
            if (tmp != null && tmp.gateId != -1)
            {
                StageInfo tmpInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)tmp.gateId);

                if (tmpInfo != null)
                {
                    ChapterInfo tmpCInfo = ConfigManager.Instance.mChaptersData.GetChapterByID(tmpInfo.ChapterID);
                    if (tmpCInfo != null)
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.FUNCTION_LOCK, tmpCInfo.name, tmpInfo.SmallGateID));
                }
            }
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECRUITVIEW);
        }
    }
    #endregion
    #region


    //public void OnNewMailNotify()
    //{
    //    View.TAlpha_NewMailTweenAlpha.gameObject.SetActive(true);
    //    View.TAlpha_NewMailTweenAlpha.style = UITweener.Style.PingPong;
    //    View.TAlpha_NewMailTweenAlpha.PlayForward();
    //}

    //public void OnCloseNewMailNotice()
    //{
    //    View.TAlpha_NewMailTweenAlpha.Restart();
    //    View.TAlpha_NewMailTweenAlpha.gameObject.SetActive(false);
    //}

    //public void InitTweenComps()
    //{
    //    View.TAlpha_NewMailTweenAlpha.Restart();
    //    View.TAlpha_NewMailTweenAlpha.gameObject.SetActive(false);
    //}
    #endregion

    private void UnBtnEventBinding()
    {
        UIEventListener.Get(View.Btn_BaZhu.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_HeadVIP.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Magical.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_HeadUI.gameObject).onClick = null;
        //UIEventListener.Get(View.Btn_CoinButton.gameObject).onClick = ButtonEvent_BuyCoinButton;
        //UIEventListener.Get(View.Btn_GemButton.gameObject).onClick = ButtonEvent_BuyGemButton;
        //UIEventListener.Get(View.Btn_SPIconButton.gameObject).onClick = ButtonEvent_BuySPButton;
        //UIEventListener.Get(View.Btn_SPLabelButton.gameObject).onPress = ButtonEvent_ClickSPButton;//体力提示

        //UIEventListener.Get(View.Btn_SignInButton.gameObject).onClick = ButtonEvent_SignInButton;
        //UIEventListener.Get(View.Btn_RechargeButton.gameObject).onClick = ButtonEvent_RechargeButton;
        //UIEventListener.Get(View.Btn_NoticeButton.gameObject).onClick = ButtonEvent_NoticeButton;
        //UIEventListener.Get(View.Btn_TaskButton.gameObject).onClick = ButtonEvent_TaskButton;
        //UIEventListener.Get(View.Btn_ActiveButton.gameObject).onClick = ButtonEvent_ActiveButton;

        //UIEventListener.Get(View.Btn_MailButton.gameObject).onClick = ButtonEvent_MailButton;
        //UIEventListener.Get(View.Btn_OpenMenuButton.gameObject).onClick = ButtonEvent_MenuButton;
        //UIEventListener.Get(View.Btn_HeroButton.gameObject).onClick = ButtonEvent_HeroButton;
        //UIEventListener.Get(View.Btn_GeneralButton.gameObject).onClick = ButtonEvent_GeneralButton;
        //UIEventListener.Get(View.Btn_BagButton.gameObject).onClick = ButtonEvent_BagButton;

        //UIEventListener.Get(View.Btn_ShopButton.gameObject).onClick = ButtonEvent_ShopButton;
        UIEventListener.Get(View.Btn_ChatButton.gameObject).onClick = null;

        UIEventListener.Get(View.Btn_MainGateButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_ActivitiesButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_SlaveButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_LegionButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_ArenaButton.gameObject).onClick = null;

        UIEventListener.Get(View.Btn_ExpeditionButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_BillboardButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_BattlegroundButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_AgainstButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_ShopButton.gameObject).onClick = null;

        UIEventListener.Get(View.Btn_MailButton.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Grass_1.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Grass_2.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Grass_3.gameObject).onClick = null;
        UIEventListener.Get(View.Btn_Grass_4.gameObject).onClick = null;
    }

    public override void Uninitialize()
    {
        UnBtnEventBinding();
        PlayerData.Instance.UpdateLevelEvent -= UpdateLevel;
        PlayerData.Instance.UpdateVipEvent -= UpdateVip;
        //PlayerData.Instance.UpdateLevelEvent -= UpdateLevelUnLock;
        PlayerData.Instance.UpdateMaxCombatPowerEvent -= UpdateMaxFighting;
        PlayerData.Instance.UpdateVipRewardInfoEvent -= UpdateVipRewardNotice;
        //Scheduler.Instance.RemoveTimer(PlayMarqueeAnimation);
        Scheduler.Instance.RemoveUpdator(PlayMarqueeAnimation);
        Scheduler.Instance.RemoveLateUpdator(MoveUpdate);

    }

    public override void Destroy()
    {
        View = null;
        IsOpenMenus = false;
        marqueeList = null;
        DelayOpenTime = 0.25F;
    }

    public override UIBoundary GetUIBoundary()
    {
        return View.Boundary;
    }

    public void ShowMainGateHint(string vMessage)//显示主线战斗提示气泡
    {
        if (this.View == null)
            return;

        if (!string.IsNullOrEmpty(vMessage))
        {
            View.Tscle_qipao.transform.localScale = Vector3.zero;
            View.Lbl_Tips.text = vMessage;
            View.Tscle_qipao.ResetToBeginning();
            View.Tscle_qipao.PlayForward();
            View.Tscle_qipao.gameObject.SetActive(true);
        }
        else
        {
            View.Tscle_qipao.gameObject.SetActive(false);
        }
    }
}
