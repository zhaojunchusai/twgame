using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using fogs.proto.msg;

public class ActivitiesViewController : UIBase
{
    private enum ActivityState
    {
        /// <summary>
        /// 可挑战
        /// </summary>
        None = 0,
        /// <summary>
        /// 没有挑战次数
        /// </summary>
        NoCount = 1,
        /// <summary>
        /// 未到开放时间
        /// </summary>
        Lock = 2,
    }

    private class ActivityData
    {
        public ActivityDungeonInfo activityPOD;
        public ChapterInfo chapterInfo;
        public ActivityState state;
    }

    public ActivitiesView view;
    private List<ActivitiesDifficultComponent> difficult_dic;
    private ActivityData normalActivityData;
    private ActivityData eliteActicityData;
    private ActivityDungeonInfo currentDungeonInfo;

    public override void Initialize()//初始化UI
    {
        if (view == null)
        {
            view = new ActivitiesView();
            view.Initialize();
            BtnEventBinding();
        }
        isNormalOpen = false;
        isEliteOpen = false;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        InitView();
        //PlayOpenMainGroupAnim();
        PlayerData.Instance.NotifyResetEvent += UpdateChallengeCount;
        FightRelatedModule.Instance.MopupDungeonEvent += OnMopupDungeon;
        GameActivityModule.Instance.OnSendQueryActivityTimeReq();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenFightBadMinisterView);
        FightRelatedModule.Instance.SendDungeonInfo(fogs.proto.msg.DungeonType.DGT_ACTIVITY);
    }
    public void InitDouble(List<ActivityTimeInfo> ActivityInfo)
    {
        view.DoubleCount.SetActive(false);
        view.DoubleReward.SetActive(false);
        //List<GameActivityData> data = ConfigManager.Instance.mGameActivityConfig.GetVaildAcitvityListByIDs(ActivityInfo);
        //for (int i = 0; i < data.Count; i++)
        //{
        //    UpdateDoubelHit(data[i]);
        //}
        //GameActivityData tmp;
        for (int i = 0; i < ActivityInfo.Count; i++)
        {
            //tmp = ConfigManager.Instance.mGameActivityConfig.GetDataByID((uint)ActivityInfo[i].id);
            //if (tmp == null)
            //    continue;
            UpdateDoubelHit(ActivityInfo[i]);
        }
    }
    public void UpdateDoubelHit(ActivityTimeInfo timeinfo)
    {
        switch ((GameActivityType)timeinfo.activity_type)
        {
            case GameActivityType.DropDouble_EventDungeons://远征
               // Debug.LogError(data.mName + "         " + data.mType + "            " + data.mID);

                view.DoubleReward.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.ChallengeDouble_Expedition://远征
              //  Debug.LogError(data.mName + "         " + data.mType + "            " + data.mID);

                view.DoubleCount.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            default:
                //Debug.LogError(data.mName+"    "+data.mType);
                break;
        }
    }

    private void InitView()
    {
        if (difficult_dic == null)
            difficult_dic = new List<ActivitiesDifficultComponent>();
        view.Gobj_SelectDifficult.SetActive(false);
        view.MainGroup_TScale.gameObject.SetActive(true);
    }

    public void ReceiveDungeonInfo(List<fogs.proto.msg.ActivityDungeonInfo> list)
    {
        PlayerData.Instance._ActivityDungeonList = list;
        if (PlayerData.Instance._ActivityDungeonList == null)
            PlayerData.Instance._ActivityDungeonList = new List<ActivityDungeonInfo>();
        UpdateViewInfo();
        Assets.Script.Common.Scheduler.Instance.AddUpdator(CheckOpen);
    }

    public void ReceiveBuyDungeonInfo(BuyOtherDungeonTimesResp data)
    {
        if (normalActivityData != null && normalActivityData.activityPOD != null)
        {
            if (normalActivityData.activityPOD.chapter_id == data.chapter_id)
            {
                normalActivityData.activityPOD.today_times = (int)data.today_times;
                normalActivityData.activityPOD.today_buy_times = (int)data.today_buy_times;
            }
        }
        if (eliteActicityData != null && eliteActicityData.activityPOD != null)
        {
            if (eliteActicityData.activityPOD.chapter_id == data.chapter_id)
            {
                eliteActicityData.activityPOD.today_times = (int)data.today_times;
                eliteActicityData.activityPOD.today_buy_times = data.today_buy_times;
            }
        }
        UpdateViewInfo();
        if (currentDungeonInfo != null)
        {
            if (currentDungeonInfo.chapter_id == data.chapter_id)
            {
                currentDungeonInfo.today_times = (int)data.today_times;
                currentDungeonInfo.today_buy_times = data.today_buy_times;
            }
            UpdateDifficultyPanelStatus(currentDungeonInfo.max_times - currentDungeonInfo.today_times);
        }
    }

    #region Update Event

    public void UpdateDifficultyPanelStatus(int count)
    {
        view.Lbl_SurplusChanllenge.text = string.Format(ConstString.ACITIVITIES_CDTODAY, count);
        if (count > 0)
        {
            view.Btn_Purchase.gameObject.SetActive(false);
        }
        else
        {
            view.Btn_Purchase.gameObject.SetActive(true);
        }
    }

    private void UpdateChallengeCount(NotifyReset data)
    {
        if (data == null || data.activity_dgn == null) return;
        for (int i = 0; i < data.activity_dgn.Count; i++)
        {
            NotifyReset.IdToTimes id = data.activity_dgn[i];
            if (id == null) continue;
            ActivityDungeonInfo info = PlayerData.Instance._ActivityDungeonList.Find((tmp) => { return tmp.chapter_id == id.id; });
            if (info == null) continue;
            info.today_times = 0;
            info.max_times = id.times;
        }
        if (normalActivityData == null)
        {
            normalActivityData = new ActivityData();
        }
        if (eliteActicityData == null)
        {
            eliteActicityData = new ActivityData();
        }
        UpdateChapterStatus(normalActivityData, GlobalCoefficient.NormalActivity, view.Tex_BtnLeftActivity, view.Spt_BtnLeftActivitiesTip, view.Lbl_BtnLeftChallengeTime);
        UpdateChapterStatus(eliteActicityData, GlobalCoefficient.EliteActivity, view.Tex_BtnRightActivity, view.Spt_BtnRightActivitiesTip, view.Lbl_BtnRightChallengeTime);
    }

    /// <summary>
    /// 更新难度选择面板数据
    /// </summary>
    private void OpenDifficultInfo(ActivityDungeonInfo chapter, List<StageData> list, int count)
    {
        currentDungeonInfo = chapter;
        view.MainGroup_TScale.gameObject.SetActive(false);
        view.Gobj_SelectDifficult.SetActive(true);
        UpdateDifficultyPanelStatus(count);
        Main.Instance.StartCoroutine(UpdateActivityComps(list));
    }

    private IEnumerator UpdateActivityComps(List<StageData> list)
    {
        if (view.Gobj_DifficultComp.activeSelf)
        {
            view.Gobj_DifficultComp.SetActive(false);
        }
        if (list == null) list = new List<StageData>();
        if (list.Count <= difficult_dic.Count)
        {
            for (int i = 0; i < difficult_dic.Count; i++)
            {
                ActivitiesDifficultComponent comp = difficult_dic[i];
                if (comp == null) continue;
                if (i < list.Count)
                {
                    StageData stage = list[i];
                    if (stage == null)
                    {
                        comp.mRootObject.SetActive(false);
                    }
                    else
                    {
                        comp.mRootObject.SetActive(true);
                        comp.UpdateInfo(stage);
                    }
                }
                else
                {
                    if (comp.mRootObject.activeSelf)
                    {
                        comp.mRootObject.SetActive(false);
                    }

                }
            }
        }
        else
        {
            int go_index = difficult_dic.Count;
            for (int i = 0; i < list.Count; i++)
            {
                ActivitiesDifficultComponent comp = null;
                StageData stage = list[i];
                if (stage == null)
                    continue;
                if (i < go_index)
                {
                    comp = difficult_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_DifficultComp, view.Grd_DifficultGrid.transform);
                    go.name = "Item_" + i;
                    comp = new ActivitiesDifficultComponent(go);
                    comp.AddEventListener(ButtonEvent_ActivityComp);
                    difficult_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(stage);
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        view.Grd_DifficultGrid.Reposition();
    }

    /// <summary>
    /// 更新视图
    /// </summary>
    private void UpdateViewInfo()
    {
        normalActivityData = new ActivityData();
        eliteActicityData = new ActivityData();
        UpdateChapterStatus(normalActivityData, GlobalCoefficient.NormalActivity, view.Tex_BtnLeftActivity, view.Spt_BtnLeftActivitiesTip, view.Lbl_BtnLeftChallengeTime);
        UpdateChapterStatus(eliteActicityData, GlobalCoefficient.EliteActivity, view.Tex_BtnRightActivity, view.Spt_BtnRightActivitiesTip, view.Lbl_BtnRightChallengeTime);
    }

    public void OnMopupDungeon(MopupDungeonResp data)
    {
        if (PlayerData.Instance._ActivityDungeonList == null) return;
        if (data.type == DungeonType.DGT_ACTIVITY)
        {
            ActivityDungeonInfo info = PlayerData.Instance._ActivityDungeonList.Find((tmp) =>
            {
                if (tmp == null || data.activity_info == null) return false;
                return tmp.chapter_id == data.activity_info.chapter_id;
            });
            if (info != null)
            {
                UpdateViewInfo();
                if (view.Gobj_SelectDifficult.activeSelf)
                {
                    //理论上 扫荡后返回的数据为当前章节的数据
                    int count = info.max_times - info.today_times;
                    view.Lbl_SurplusChanllenge.text = string.Format(ConstString.ACITIVITIES_CDTODAY, count);
                }
            }
        }
    }

    #endregion

    /// <summary>
    /// 更新副本状态
    /// </summary>
    /// <param name="data"></param>
    /// <param name="chapterID"></param>
    private void UpdateChapterStatus(ActivityData data, uint chapterID, UISprite sprite, UISprite tipSprite, UILabel label)
    {
        if (data == null) return;
        data.chapterInfo = ConfigManager.Instance.mChaptersData.GetChapterByID(chapterID);
        if (data.chapterInfo == null)
        {
            return;
        }
        if (!IsOpenTime(data.chapterInfo.openTime))
        {
            CommonFunction.UpdateWidgetGray(sprite, true);
            CommonFunction.UpdateWidgetGray(tipSprite, true);
            label.text = GetOpenTime(data.chapterInfo.openTime);
            data.state = ActivityState.Lock;
            return;
        }
        CommonFunction.UpdateWidgetGray(sprite, false);
        CommonFunction.UpdateWidgetGray(tipSprite, false);
        data.activityPOD = PlayerData.Instance._ActivityDungeonList.Find((tmp) => { return tmp.chapter_id == chapterID; });
        if (data.activityPOD == null)
        {
            data.activityPOD = new ActivityDungeonInfo();
            data.activityPOD.chapter_id = chapterID;
            data.activityPOD.pass_dgns.Clear();
            data.activityPOD.today_times = 0;
        }
        int sweepCount = data.activityPOD.max_times - data.activityPOD.today_times;
        if (sweepCount <= 0)
            sweepCount = 0;
        if (sweepCount <= 0)//剩余次数不足
        {
            //CommonFunction.UpdateWidgetGray(sprite, true);
            //CommonFunction.UpdateWidgetGray(tipSprite, true);
            label.text = string.Format(ConstString.ACTIVITIES_NUMLESS, sweepCount);//更新挑战次数
            data.state = ActivityState.NoCount;
            //return;
        }
        else//挑战次数及时间均解锁状态
        {
            //CommonFunction.UpdateWidgetGray(sprite, false);//取消置灰
            //CommonFunction.UpdateWidgetGray(tipSprite, false);
            label.text = string.Format(ConstString.ACTIVITIES_NUMLESS, sweepCount);
            data.state = ActivityState.None;
        }
    }

    private bool isNormalOpen = false;
    private bool isEliteOpen = false;
    private void CheckOpen()
    {
        if (normalActivityData != null)
        {
            if (isNormalOpen)
            {
                if (!IsOpenTime(normalActivityData.chapterInfo.openTime))
                {
                    UpdateChapterStatus(normalActivityData, GlobalCoefficient.NormalActivity, view.Tex_BtnLeftActivity, view.Spt_BtnLeftActivitiesTip, view.Lbl_BtnLeftChallengeTime);
                    isNormalOpen = false;
                }
            }
            else
            {
                if (IsOpenTime(normalActivityData.chapterInfo.openTime))
                {
                    UpdateChapterStatus(normalActivityData, GlobalCoefficient.NormalActivity, view.Tex_BtnLeftActivity, view.Spt_BtnLeftActivitiesTip, view.Lbl_BtnLeftChallengeTime);
                    isNormalOpen = true;
                }
            }
        }
        if (eliteActicityData != null)
        {
            if (isEliteOpen)
            {
                if (!IsOpenTime(eliteActicityData.chapterInfo.openTime))
                {
                    UpdateChapterStatus(eliteActicityData, GlobalCoefficient.EliteActivity, view.Tex_BtnRightActivity, view.Spt_BtnRightActivitiesTip, view.Lbl_BtnRightChallengeTime);
                    isEliteOpen = false;
                }
            }
            else
            {
                if (IsOpenTime(eliteActicityData.chapterInfo.openTime))
                {
                    UpdateChapterStatus(eliteActicityData, GlobalCoefficient.EliteActivity, view.Tex_BtnRightActivity, view.Spt_BtnRightActivitiesTip, view.Lbl_BtnRightChallengeTime);
                    isEliteOpen = true;
                }
            }
        }
    }

    /// <summary>
    /// 是否满足开放时间 返回-1 则说明在开放时间内
    /// </summary>
    /// <param name="dic"></param>
    /// <returns></returns>
    private bool IsOpenTime(List<ChapterOpenTime> list)
    {
        DateTime date = CommonFunction.GetTimeByLong(Main.mTime);
        int week = Convert.ToInt32(date.DayOfWeek);
        for (int i = 0; i < list.Count; i++)
        {
            ChapterOpenTime opentime = list[i];
            if (opentime.week == week)
            {
                if (opentime.startTime <= date.Hour && date.Hour < opentime.endTime)
                {
                    return true;
                }
            }
        }
        return false;
    }

    private string GetOpenTime(List<ChapterOpenTime> list)
    {
        string time = string.Empty;
        DateTime date = CommonFunction.GetTimeByLong(Main.mTime);
        int week = Convert.ToInt32(date.DayOfWeek);
        ChapterOpenTime opentime = null;
        for (int i = 0; i < 7; i++)
        {
            opentime = list.Find((tmp) => { return tmp.week == week; });
            if (opentime == null)
            {
                week++;
                if (week > 6) week = 0;  //如果是周末 为0
            }
            else
            {
                break;
            }
        }
        if (opentime == null)
        {
            time = ConstString.ACTIVITIES_OPENTIMENULL;
        }
        else
        {
            System.Text.StringBuilder sub = new System.Text.StringBuilder();
            sub.Append(CommonFunction.ConvertToWeek((WeekEnum)opentime.week));
            sub.Append(string.Format(ConstString.FORMAT_TIME, opentime.startTime, 0, 0));
            sub.Append("-");
            sub.Append(string.Format(ConstString.FORMAT_TIME, opentime.endTime, 0, 0));
            time = string.Format(ConstString.ACTIVITIES_OPENTIME, sub.ToString());
        }
        return time;
    }


    #region 响应事件

    private void ButtonEvent_RightActivity(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        if (eliteActicityData == null)
        {
            return;
        }
        switch (eliteActicityData.state)
        {
            case ActivityState.Lock:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACTIVITIES_OPENTIMENULL);
                } break;
            case ActivityState.NoCount:
            //    {
            //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACTIVITIES_NUMLESSNULL);
            //    } break;
            case ActivityState.None:
                {
                    List<StageData> list = GetStageDatas(eliteActicityData);
                    OpenDifficultInfo(eliteActicityData.activityPOD, list, eliteActicityData.activityPOD.max_times - eliteActicityData.activityPOD.today_times);
                }
                break;
        }
    }

    private void ButtonEvent_LeftActivity(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        if (normalActivityData == null)
        {
            return;
        }
        switch (normalActivityData.state)
        {
            case ActivityState.Lock:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACTIVITIES_OPENTIMENULL);
                } break;
            case ActivityState.NoCount:
            //{
            //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACTIVITIES_NUMLESSNULL);
            //} break;
            case ActivityState.None:
                {
                    List<StageData> list = GetStageDatas(normalActivityData);
                    OpenDifficultInfo(normalActivityData.activityPOD, list, normalActivityData.activityPOD.max_times - normalActivityData.activityPOD.today_times);
                }
                break;
        }
    }

    private void ButtonEvent_ActivityComp(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ActivitiesDifficultComponent comp = baseComp as ActivitiesDifficultComponent;
        if (comp == null) return;
        if (comp.IsLock)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.ACTIVITIES_LOCK, comp.StageData.stageinfo.UnlockLV));
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
            UISystem.Instance.GateInfoView.UpdateViewInfo(currentDungeonInfo, comp.StageData.stageinfo, comp.StageData.gateinfo, comp.StageData.remainRaidTimes, EFightType.eftActivity);
        }
    }

    private void ButtonEvent_CloseView(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ACTIVITIES);
    }

    private void ButtonEvent_CloseDifficult(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.Gobj_SelectDifficult.SetActive(false);
        view.MainGroup_TScale.gameObject.SetActive(true);
        //PlayOpenMainGroupAnim();
    }

    private void ButtonEvent_Purchase(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        int count = currentDungeonInfo.today_buy_times;
        if (count <= 0)
        {
            count = 0;
        }
        VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        if (vipData != null)
        {
            if (count >= vipData.ActivityCount)
            {
                if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Activity))
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ActivityCount), () =>
                    {
                        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                        UISystem.Instance.VipRechargeView.ShowRecharge();
                    }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                }
            }
            else
            {
                TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)count + 1);
                if (data == null || data.ActivityDgnChallengeTimes == null)// data为NULL 则可默认为已经达到购买上限
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                }
                else if (data.ActivityDgnChallengeTimes.Type == ECurrencyType.None && data.ActivityDgnChallengeTimes.Number == 0)  // 消耗金币类型为0则说明无购买次数
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_SWEEPPURCHASELIMIT);
                }
                else
                {
                    if (count >= vipData.ActivityCount)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(string.Format(ConstString.PVP_PURCHASELIMIT, count, vipData.ActivityCount), count, vipData.ActivityCount), () =>
                        {
                            UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                            UISystem.Instance.VipRechargeView.ShowRecharge();
                        }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
                    }
                    else
                    {
                        if (CommonFunction.CheckMoneyEnough(data.ActivityDgnChallengeTimes.Type, data.ActivityDgnChallengeTimes.Number, true))
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, 
                                string.Format(ConstString.GATE_PURCHASESWEEPCOUNT, 
                                data.ActivityDgnChallengeTimes.Number, count, vipData.ActivityCount), () =>
                            {
                                FightRelatedModule.Instance.SendBuyOtherDungeonTimes(OtherDungeonType.ACTIVITY_DGN, currentDungeonInfo.chapter_id);
                            });
                        }
                    }
                }
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ERRORDATA);
        }
    }


    private List<StageData> GetStageDatas(ActivityData activityInfo)
    {
        ActivityData info = activityInfo;
        List<StageData> list = new List<StageData>();
        List<uint> gateids = new List<uint>();
        for (int i = 0; i < info.activityPOD.pass_dgns.Count; i++)
        {
            gateids.Add(info.activityPOD.pass_dgns[i].dgn_id);
        }
        List<StageInfo> stages = CommonFunction.SortStages(info.chapterInfo.gates);
        for (int i = 0; i < stages.Count; i++)
        {
            StageInfo _stageinfo = stages[i];
            if (_stageinfo == null) continue;
            StageData stagedata = new StageData();
            stagedata.stageinfo = _stageinfo;
            if (gateids.Contains(_stageinfo.ID))
            {
                fogs.proto.msg.PassDungeon gateinfo = info.activityPOD.pass_dgns.Find((fogs.proto.msg.PassDungeon _gateinfo) => { return _gateinfo.dgn_id == _stageinfo.ID; });
                stagedata.gateinfo = gateinfo;
            }
            else
            {
                stagedata.gateinfo = new fogs.proto.msg.PassDungeon();
                stagedata.gateinfo.dgn_id = _stageinfo.ID;
                stagedata.gateinfo.star_level = 0;
            }
            stagedata.remainRaidTimes = activityInfo.activityPOD.max_times - activityInfo.activityPOD.today_times;
            list.Add(stagedata);
        }
        return list;
    }

    #endregion

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_RightActivity.gameObject).onClick = ButtonEvent_RightActivity;
        UIEventListener.Get(view.Btn_LeftActivity.gameObject).onClick = ButtonEvent_LeftActivity;
        UIEventListener.Get(view.Btn_CloseView.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(view.Btn_CloseDifficult.gameObject).onClick = ButtonEvent_CloseDifficult;
        UIEventListener.Get(view.Btn_Purchase.gameObject).onClick = ButtonEvent_Purchase;
    }

    public override void Uninitialize()
    {
        currentDungeonInfo = null;
        Assets.Script.Common.Scheduler.Instance.RemoveUpdator(CheckOpen);
        PlayerData.Instance.NotifyResetEvent -= UpdateChallengeCount;
        FightRelatedModule.Instance.MopupDungeonEvent -= OnMopupDungeon;


    }
    //界面动画：
    //public void PlayOpenMainGroupAnim()
    //{
    //    view.MainGroup_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim ;
    //    view.MainGroup_TScale.Restart();
    //    view.MainGroup_TScale.PlayForward();
    //}
    //public void PlayOpenSelectDifficultAnim()
    //{
    //    view.SelectDifficult_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim ;
    //    view.SelectDifficult_TScale.Restart();
    //    view.SelectDifficult_TScale.PlayForward();
    //}

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        difficult_dic.Clear();
        normalActivityData = null;
        eliteActicityData = null;
    }
}
