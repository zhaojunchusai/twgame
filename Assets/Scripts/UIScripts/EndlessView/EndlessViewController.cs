using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
/* File:BattlegroundViewController.cs
 * Desc: 无尽模式
 * Date: 2015-06-15 15:02
 * add by zhouyuxiang
 */
public class EndlessViewController : UIBase
{
    public EndlessView View;
    public ChapterInfo ChapterPT;
    public StageData StageDataPT;
    public ChapterInfo ChapterJY;
    public StageData StageDataJY;
    public ChapterInfo ChapterYX;
    public StageData StageDataYX;
    private StageInfo stageinfo;
    private EndlessDungeonInfo endlessChaptperPT;
    private EndlessDungeonInfo endlessChaptperJY;
    private EndlessDungeonInfo endlessChaptperYX;

    private EndlessDungeonInfo currentEndlessDungeon;

    public override void Initialize()
    {
        if (View == null)
        {
            View = new EndlessView();
            View.Initialize();
        }
        //UpdateCurrency();
        BtnEventBinding();
        //UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        FightRelatedModule.Instance.SendDungeonInfo(DungeonType.DGT_ENDLESS);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, View._uiRoot.transform.parent.transform));
        //PlayOpenAnim();
        PlayerData.Instance.NotifyResetEvent += UpdateChallenge;
        FightRelatedModule.Instance.MopupDungeonEvent += OnMopupDungeon;
        GameActivityModule.Instance.OnSendQueryActivityTimeReq();

        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenEndlessView);
        View.Lbl_ClearanceDesc.text = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.ENDLESS_CLEARANCEDESC);
    }
    public void InitDouble(List<ActivityTimeInfo> ActivityInfo)
    {
        View.DoubleCount.SetActive(false);
        View.DoubleReward.SetActive(false);
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
            case GameActivityType.ChallengeDouble_EventDungeons://远征
                View.DoubleCount.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.ChallengeDouble_Endness://远征
                View.DoubleReward.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            default:
                break;
        }
    }
    public override void ReturnTop()
    {
        base.ReturnTop();
        Debug.LogError("ReturnTop");
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_ENDLESS);
    }

    public void ReceiveDungeonInfo(List<EndlessDungeonInfo> list)
    {
        PlayerData.Instance._EndlessDungeonList = list;
        if (PlayerData.Instance._EndlessDungeonList == null)
            PlayerData.Instance._EndlessDungeonList = new List<EndlessDungeonInfo>();
        UpdatePTLabel(GlobalCoefficient.NormalEndless);
        UpdateJYLabel(GlobalCoefficient.EliteEndless);
        UpdateYXLabel(GlobalCoefficient.HeroEndless);
    }

    #region 更新信息

    private void UpdateChallenge(NotifyReset data)
    {
        if (data == null || data.endless_dgn == null) return;
        if (endlessChaptperPT == null)
        {
            endlessChaptperPT = GetChapterPODByID(GlobalCoefficient.NormalEndless);
        }
        if (endlessChaptperJY == null)
        {
            endlessChaptperJY = GetChapterPODByID(GlobalCoefficient.EliteEndless);
        }
        if (endlessChaptperYX == null)
        {
            endlessChaptperYX = GetChapterPODByID(GlobalCoefficient.HeroEndless);
        }
        for (int i = 0; i < data.endless_dgn.Count; i++)
        {
            NotifyReset.IdToTimes ids = data.endless_dgn[i];
            if (ids == null) continue;
            if (ids.id == endlessChaptperPT.chapter_id)
            {
                endlessChaptperPT.max_times = ids.times;
                endlessChaptperPT.use_time = 0;
            }
            else if (ids.id == endlessChaptperJY.chapter_id)
            {
                endlessChaptperJY.max_times = ids.times;
                endlessChaptperJY.use_time = 0;
            }
            else if (ids.id == endlessChaptperYX.chapter_id)
            {
                endlessChaptperYX.max_times = ids.times;
                endlessChaptperYX.use_time = 0;
            }
        }
        UpdateNormalInfo(endlessChaptperPT);
        UpdateEliteInfo(endlessChaptperJY);
        UpdateHeroInfo(endlessChaptperYX);
    }

    public void UpdatePTLabel(uint ChapterID)//更新普通难度信息
    {
        ChapterPT = ConfigManager.Instance.mChaptersData.GetChapterByID(ChapterID);//获取章节数据
        if (!PlayerData.Instance.IsOutstripLevel(ChapterPT.LVLimit))//等级不够，关卡锁定
        {
            View.GO_PTLevelLabel.SetActive(false);
            //View.Lab_PTIsLockLabel.text = string.Format(ConstString.ACTIVITIES_NEEDLEVEL, ChapterPT.LVLimit);
            View.Lab_PTIsLockLabel.gameObject.SetActive(true);
        }
        else//等级足够
        {
            View.GO_PTLevelLabel.SetActive(true);
            View.Lab_PTIsLockLabel.gameObject.SetActive(false);
            endlessChaptperPT = GetChapterPODByID(ChapterID);
            UpdateNormalInfo(endlessChaptperPT);
            UIEventListener.Get(View.Btn_PTChallengeButton.gameObject).onClick = ButtonEvent_PTChallengeButton;
        }
    }

    public void UpdateNormalInfo(EndlessDungeonInfo info)
    {
        View.Lab_PTMaxDPLabel.text = string.Format(ConstString.ENDLESS_TOPGRADE, info.high_victory);
        View.Lab_PTMaxScoreLabel.text = string.Format(ConstString.ENDLESS_TOPSCORE, info.high_grade);
        //View.Lab_PTEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TOPRANK, info.rank);
        int count = info.max_times - info.today_times;
        if (count <= 0)
        {
            count = 0;
        }
        View.Lab_PTEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TODAYCHALLENGETIME, count);
    }

    public void UpdateEliteInfo(EndlessDungeonInfo info)
    {
        View.Lab_JYMaxDPLabel.text = string.Format(ConstString.ENDLESS_TOPGRADE, info.high_victory);
        View.Lab_JYMaxScoreLabel.text = string.Format(ConstString.ENDLESS_TOPSCORE, info.high_grade);
        //View.Lab_JYEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TOPRANK, info.rank);
        int count = info.max_times - info.today_times;
        if (count <= 0)
        {
            count = 0;
        }
        View.Lab_JYEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TODAYCHALLENGETIME, count);
    }

    public void UpdateHeroInfo(EndlessDungeonInfo info)
    {
        View.Lab_YXMaxDPLabel.text = string.Format(ConstString.ENDLESS_TOPGRADE, info.high_victory);
        View.Lab_YXMaxScoreLabel.text = string.Format(ConstString.ENDLESS_TOPSCORE, info.high_grade);
        //View.Lab_YXEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TOPRANK, info.rank);
        int count = info.max_times - info.today_times;
        if (count <= 0)
        {
            count = 0;
        }
        View.Lab_YXEndlessRankLabel.text = string.Format(ConstString.ENDLESS_TODAYCHALLENGETIME, count);
    }

    public void UpdateJYLabel(uint id)//更新精英难度信息
    {
        ChapterJY = ConfigManager.Instance.mChaptersData.GetChapterByID(id);//获取章节数据
        if (!PlayerData.Instance.IsOutstripLevel(ChapterJY.LVLimit))//等级不够，关卡锁定
        {
            View.GO_JYLevelLabel.SetActive(false);
            //View.Lab_JYIsLockLabel.text = string.Format(ConstString.ACTIVITIES_NEEDLEVEL, ChapterJY.LVLimit);
            View.Lab_JYIsLockLabel.gameObject.SetActive(true);
        }
        else//等级足够
        {
            View.Lab_JYIsLockLabel.gameObject.SetActive(false);
            View.GO_JYLevelLabel.SetActive(true);
            endlessChaptperJY = GetChapterPODByID(id);
            UpdateEliteInfo(endlessChaptperJY);
            UIEventListener.Get(View.Btn_JYChallengeButton.gameObject).onClick = ButtonEvent_JYChallengeButton;
        }
    }

    public void UpdateYXLabel(uint id)//更新英雄难度信息
    {
        ChapterYX = ConfigManager.Instance.mChaptersData.GetChapterByID(id);//获取章节数据
        if (!PlayerData.Instance.IsOutstripLevel(ChapterYX.LVLimit))//等级不够，关卡锁定
        {
            View.GO_YXLevelLabel.SetActive(false);
            //View.Lab_YXIsLockLabel.text = string.Format(ConstString.ACTIVITIES_NEEDLEVEL, ChapterYX.LVLimit);
            View.Lab_YXIsLockLabel.gameObject.SetActive(true);
        }
        else//等级足够
        {
            View.GO_YXLevelLabel.SetActive(true);
            View.Lab_YXIsLockLabel.gameObject.SetActive(false);
            endlessChaptperYX = GetChapterPODByID(id);
            UpdateHeroInfo(endlessChaptperYX);
            UIEventListener.Get(View.Btn_YXChallengeButton.gameObject).onClick = ButtonEvent_YXChallengeButton;
        }
    }

    public void EnterGateInfo(EndlessDungeonInfo info)
    {
        currentEndlessDungeon = info;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
        UISystem.Instance.GateInfoView.UpdateViewInfo(currentEndlessDungeon, stageinfo, EFightType.eftEndless);
    }
    #endregion
    #region 绑定事件
    private void BtnEventBinding()
    {
        UIEventListener.Get(View.Btn_CloseButton.gameObject).onClick = ButtonEvent_CloseButton;
        // UIEventListener.Get(View.Btn_MenuButton.gameObject).onClick = ButtonEvent_MenuButton;
    }

    #endregion

    private void OnMopupDungeon(MopupDungeonResp data)
    {
        UpdatePTLabel(GlobalCoefficient.NormalEndless);
        UpdateJYLabel(GlobalCoefficient.EliteEndless);
        UpdateYXLabel(GlobalCoefficient.HeroEndless);
    }

    /// <summary>
    /// 获取服务器保存的该模式数据  
    /// </summary>
    private EndlessDungeonInfo GetChapterPODByID(uint chapterID)
    {
        EndlessDungeonInfo info = null;
        for (int i = 0; i < PlayerData.Instance._EndlessDungeonList.Count; i++)
        {
            EndlessDungeonInfo pod = PlayerData.Instance._EndlessDungeonList[i];
            if (pod.chapter_id == chapterID)
            {
                info = pod;
                break;
            }
        }
        if (info == null)
        {
            info = new EndlessDungeonInfo();
            info.chapter_id = chapterID;
            info.high_grade = 0;
            info.high_victory = 0;
            info.rank = 0;
            info.today_times = 0;
        }
        return info;
    }

    #region 点击事件
    private void ButtonEvent_PTChallengeButton(GameObject Btn)//普通挑战
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, View._uiRoot.transform.parent.transform));
        if (ChapterPT == null)
        {
            return;
        }
        if (!PlayerData.Instance.IsOutstripLevel(ChapterPT.LVLimit))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_LEVEL_UNLOCK);
        }
        else
        {
            if (endlessChaptperPT == null)
            {
                endlessChaptperPT = GetChapterPODByID(GlobalCoefficient.NormalEndless);
            }
            if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Endless) < 0)
            {
                int count = endlessChaptperPT.max_times - endlessChaptperPT.today_times;
                if (count <= 0)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CHALLENGE_NOCOUNT);
                    return;
                }
            }           
            List<StageInfo> stagelist = CommonFunction.SortStages(ChapterPT.gates);
            if (stagelist == null || stagelist.Count == 0)
            {
                return;
            }
            int index = stagelist.Count - 1;// 策划需求仅显示最后一关的数据 add by taiwei 
            stageinfo = stagelist[index];
            stageinfo.Physical = stagelist[0].Physical;  //填的体力在第一条数据
            EnterGateInfo(endlessChaptperPT);
        }
    }
    private void ButtonEvent_JYChallengeButton(GameObject Btn)//精英挑战
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, View._uiRoot.transform.parent.transform));
        if (ChapterJY == null)
        {
            return;
        }
        if (!PlayerData.Instance.IsOutstripLevel(ChapterJY.LVLimit))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_LEVEL_UNLOCK);
        }
        else
        {
            if (endlessChaptperJY == null)
            {
                endlessChaptperJY = GetChapterPODByID(GlobalCoefficient.EliteEndless);
            }
            if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Endless) < 0)
            {
                int count = endlessChaptperJY.max_times - endlessChaptperJY.today_times;
                if (count <= 0)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CHALLENGE_NOCOUNT);
                    return;
                }
            }            
            List<StageInfo> stagelist = CommonFunction.SortStages(ChapterJY.gates);
            if (stagelist == null || stagelist.Count == 0)
            {
                return;
            }
            int index = stagelist.Count - 1;
            stageinfo = stagelist[index];
            stageinfo.Physical = stagelist[0].Physical;  //填的体力在第一条数据
            EnterGateInfo(endlessChaptperJY);
        }

    }
    private void ButtonEvent_YXChallengeButton(GameObject Btn)//英雄挑战
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, View._uiRoot.transform.parent.transform));
        if (ChapterYX == null)
        {
            return;
        }
        if (!PlayerData.Instance.IsOutstripLevel(ChapterYX.LVLimit))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_LEVEL_UNLOCK);
        }
        else
        {
            if (endlessChaptperYX == null)
            {
                endlessChaptperYX = GetChapterPODByID(GlobalCoefficient.HeroEndless);
            }
            if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Endless) < 0)
            {
                int count = endlessChaptperYX.max_times - endlessChaptperYX.today_times;
                if (count <= 0)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_CHALLENGE_NOCOUNT);
                    return;
                }
            }            
            List<StageInfo> stagelist = CommonFunction.SortStages(ChapterYX.gates);
            if (stagelist == null || stagelist.Count == 0)
            {
                return;
            }

            int index = stagelist.Count - 1;
            stageinfo = stagelist[index];
            stageinfo.Physical = stagelist[0].Physical;  //填的体力在第一条数据
            EnterGateInfo(endlessChaptperYX);
            //FightRelatedModule.Instance.SendChapterOpen(ChapterYX.id);
        }

    }
    private void ButtonEvent_CloseButton(GameObject Btn)//关闭界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, View._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ENDLESS);
    }
    #endregion
    public override void Uninitialize()
    {
        endlessChaptperPT = null;
        endlessChaptperJY = null;
        endlessChaptperYX = null;
        PlayerData.Instance.NotifyResetEvent -= UpdateChallenge;
        FightRelatedModule.Instance.MopupDungeonEvent -= OnMopupDungeon;
    }

    public override void Destroy()
    {
        base.Destroy();
        View = null;
    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    View.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    View.Anim_TScale.Restart();
    //    View.Anim_TScale.PlayForward();

    //}
}