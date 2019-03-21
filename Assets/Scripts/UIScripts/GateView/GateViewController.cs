using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using fogs.proto.msg;
using Assets.Script.Common;
using Assets.Script.Common.StateMachine;
public class GateViewController : UIBase
{
    public GateView view;

    private ChapterInfo chapterinfo;

    private enum PlayAnimState
    {
        None = 0,
        RightToLeft = 1,
        LeftToRight = 2,
    }

    private uint crusadeChapterID;
    private uint escortChapterID;
    private uint eliteCrusadeChapterID;
    private PlayAnimState PlayAnimStatus = PlayAnimState.None;
    private bool isPlayingAnim = false;
    /// <summary>
    /// 当前战斗类型
    /// </summary>
    private MainBattleType battletype;
    private List<UISprite> gatePointsList;
    private List<CrusadeStageButtonComponent> crusadeBtn_Dic;
    private List<EliteCrusadeStageBtnComponent> eliteCrusadeBtn_Dic;
    private List<EscortStageComponent> currentEscortBtn_Dic;
    private List<EscortStageComponent> hideEscortBtn_Dic;
    private List<StageData> stagedatalist;
    /// <summary>
    /// 当前章节关卡数据
    /// </summary>
    private List<StageData> currentChapterList;
    /// <summary>
    /// 当前模式星级数
    /// </summary>
    private uint currentStar = 0;
    private Vector3 hidePos = new Vector3(820, 0, 0);
    private Vector3 currentPos = new Vector3(0, 0, 0);
    public override void Initialize()
    {
        if (view == null)
        {
            view = new GateView();
            view.Initialize();
        }
        chapterinfo = null;
        crusadeChapterID = 0;
        escortChapterID = 0;
        eliteCrusadeChapterID = 0;
        InitView();
        view.Btn_LeftArrow.gameObject.SetActive(false);
        view.Btn_RightArrow.gameObject.SetActive(false);
        view.Gobj_IndicateGroup.SetActive(false);
        view.Gobj_GateGroup.SetActive(false);
        view.Gobj_EscortGroup.SetActive(false);
        view.Gobj_EliteCrusadeGroup.SetActive(false);
        view.Gobj_EscortComp.SetActive(false);
        view.Gobj_EliteCrusade.SetActive(false);
        view.Gobj_CurrentGroup.transform.localPosition = currentPos;
        view.Gobj_HideGroup.transform.localPosition = hidePos;
        isPlayingAnim = false;
        BtnEventBinding();
        PlayAnimStatus = PlayAnimState.None;
        battletype = MainBattleType.Crusade;
        view.Spt_Point.gameObject.SetActive(false);
        view.Gobj_Gate.SetActive(false);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenGateView);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        view.UIWrapContent_CurrentGrid.onInitializeItem += UpdateCurrentGrd;
        view.UIWrapContent_HideGrid.onInitializeItem += UpdateHideGrd;
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(GateView.UIName);
    }

    public void UpdateViewInfo(MainBattleType type = MainBattleType.Crusade)
    {
        battletype = type;
        UpdateTitleGroup();
        if (PlayerData.Instance._MajorDungeonInfoList == null)
        {
            FightRelatedModule.Instance.SendDungeonInfo(DungeonType.DGT_MAJOR);
        }
        else
        {
            ReceiveDungeonInfo();
        }
        // Main.Instance.StartCoroutine(PlayOpenAnim());
    }

    public void UpdateViewInfo(uint gateID)
    {
        List<ChapterInfo> list = ConfigManager.Instance.mChaptersData.GetMainBattleChapters();
        for (int i = 0; i < list.Count; i++)
        {
            ChapterInfo tmpChapter = list[i];
            if (tmpChapter.gates.Contains(gateID))
            {
                chapterinfo = tmpChapter;
                break;
            }
        }
        StageInfo stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(gateID);
        battletype = (MainBattleType)stageInfo.IsElite;
        if (battletype == MainBattleType.Escort)
        {
            PlayAnimStatus = PlayAnimState.None;
        }
        UpdateViewInfo(battletype);
    }

    public void ReceiveDungeonInfo()
    {
        stagedatalist = PlayerData.Instance.GetAvailableGates();
        List<ChapterInfo> list = ConfigManager.Instance.mChaptersData.GetMainBattleChapters();
        if (chapterinfo == null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ChapterInfo tmpChapter = list[i];
                if (tmpChapter.LVLimit > PlayerData.Instance._Level)
                {
                    continue;
                }
                StageData tmpStage = stagedatalist.Find((tmp) =>
                {
                    if (tmp == null) return false;
                    if (tmp.stageinfo.IsElite != (int)battletype)
                    {
                        return false;
                    }
                    return tmp.stageinfo.ChapterID == tmpChapter.id;
                });
                if (tmpStage != null)
                {
                    if (chapterinfo != null)
                    {
                        if (chapterinfo.LVLimit < tmpChapter.LVLimit)
                        {
                            chapterinfo = tmpChapter;
                        }
                    }
                    else
                    {
                        chapterinfo = tmpChapter;
                    }
                }
            }
            if (chapterinfo == null)
                chapterinfo = ConfigManager.Instance.mChaptersData.GetFirstChapterInfo();
        }
        //chapterinfo = GetAvailableChapter();
        UpdatePanelInfo();
    }

    private void InitView()
    {
        view.Spt_Point.gameObject.SetActive(false);
        if (stagedatalist == null)
            stagedatalist = new List<StageData>();
        if (currentChapterList == null)
            currentChapterList = new List<StageData>();
        if (gatePointsList == null)
            gatePointsList = new List<UISprite>();
        if (crusadeBtn_Dic == null)
            crusadeBtn_Dic = new List<CrusadeStageButtonComponent>();
        if (eliteCrusadeBtn_Dic == null)
            eliteCrusadeBtn_Dic = new List<EliteCrusadeStageBtnComponent>();
        if (currentEscortBtn_Dic == null)
            currentEscortBtn_Dic = new List<EscortStageComponent>();
        if (hideEscortBtn_Dic == null)
            hideEscortBtn_Dic = new List<EscortStageComponent>();
    }

    private void UpdateTitleGroup()
    {
        switch (battletype)
        {
            case MainBattleType.Escort:
                {
                    view.Gobj_CrusadeTitle.SetActive(false);
                    view.Gobj_EliteCrusadeTitle.SetActive(false);
                    view.Gobj_EscortTitle.SetActive(true);
                }
                break;
            case MainBattleType.Crusade:
                {
                    view.Gobj_EliteCrusadeTitle.SetActive(false);
                    view.Gobj_EscortTitle.SetActive(false);
                    view.Gobj_CrusadeTitle.SetActive(true);
                }
                break;
            case MainBattleType.EliteCrusade:
                {
                    view.Gobj_EscortTitle.SetActive(false);
                    view.Gobj_CrusadeTitle.SetActive(false);
                    view.Gobj_EliteCrusadeTitle.SetActive(true);
                }
                break;
        }
    }

    private void UpdateChapterData()
    {
        List<StageData> list = new List<StageData>();
        List<StageInfo> stagelist = SortGatesData(chapterinfo, battletype);
        for (int i = 0; i < stagelist.Count; i++)
        {
            StageData tdata = new StageData();
            StageInfo tstage = stagelist[i];
            tdata.remainRaidTimes = tstage.ChallengeCount;
            for (int j = 0; j < stagedatalist.Count; j++)
            {
                StageData _stagedata = stagedatalist[j];
                if (tstage.ID == _stagedata.stageinfo.ID)
                {
                    tdata.gateinfo = _stagedata.gateinfo;
                    tdata.remainRaidTimes = _stagedata.remainRaidTimes;
                    break;
                }
            }
            tdata.stageinfo = tstage;
            list.Add(tdata);
        }
        currentChapterList = list;
    }
    /// <summary>
    /// 更新整个关卡选择面板信息
    /// </summary>
    public void UpdatePanelInfo()
    {
        if (chapterinfo != null)
        {
            switch (battletype)
            {
                case MainBattleType.Crusade:
                    crusadeChapterID = chapterinfo.id;
                    break;
                case MainBattleType.Escort:
                    escortChapterID = chapterinfo.id;
                    break;
                case MainBattleType.EliteCrusade:
                    eliteCrusadeChapterID = chapterinfo.id;
                    break;
            }
        }
        UpdateTitleGroup();
        UpdateChapterData();
        UpdateBattleTypeStatus();
        UpdateAwardDesc();
        UpdateAwardsButton();
        UpdateArrowStatus();
        UpdateChapterName();
        UpdateAwardsStatus();
        UpdateButtonStatus();
        Scheduler.Instance.AddFrame(1, false, UpdateStageType);
    }


    private void UpdateButtonStatus()
    {
        CommonFunction.UpdateWidgetGray(view.Spt_BtnEscortType, !CheckGateActive(MainBattleType.Escort));
        CommonFunction.UpdateWidgetGray(view.Spt_BtnEliteCrusadeType, !CheckGateActive(MainBattleType.EliteCrusade));
    }

    private void UpdateStageType()
    {
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    view.Gobj_EliteCrusadeGroup.SetActive(false);
                    view.Gobj_EscortGroup.SetActive(false);
                    view.Gobj_GateGroup.SetActive(true);
                    view.Gobj_TexGroup.SetActive(true);
                    view.Gobj_IndicateGroup.SetActive(false);
                    view.Spt_EscortBgSprite.gameObject.SetActive(false);
                    Scheduler.Instance.AddFrame(1, false, UpdateChapterBG);
                    Main.Instance.StartCoroutine(UpdateGateButton());
                }
                break;
            case MainBattleType.Escort:
                {
                    view.Gobj_GateGroup.SetActive(false);
                    view.Gobj_EliteCrusadeGroup.SetActive(false);
                    view.Gobj_TexGroup.SetActive(false);
                    view.Gobj_EscortGroup.SetActive(true);
                    view.Gobj_IndicateGroup.SetActive(false);
                    view.Spt_EscortBgSprite.gameObject.SetActive(true);
                    switch (PlayAnimStatus)
                    {
                        case PlayAnimState.None:
                            {
                                GuideManager.Instance.CheckTrigger(GuideTrigger.EnterProtectCityView);
                                if (view.Gobj_CurrentGroup.transform.localPosition == currentPos)
                                {
                                    Main.Instance.StartCoroutine(UpdateCurrentItem());
                                }
                                else
                                {
                                    Main.Instance.StartCoroutine(UpdateHideItem());
                                }
                            } break;
                        case PlayAnimState.RightToLeft:
                        case PlayAnimState.LeftToRight:
                            {
                                if (view.Gobj_CurrentGroup.transform.localPosition == currentPos)
                                {
                                    Main.Instance.StartCoroutine(UpdateHideItem());
                                }
                                else
                                {
                                    Main.Instance.StartCoroutine(UpdateCurrentItem());
                                }
                            } break;
                    }
                }
                break;
            case MainBattleType.EliteCrusade:
                {
                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenAttackCityView);
                    view.Gobj_EscortGroup.SetActive(false);
                    view.Gobj_GateGroup.SetActive(false);
                    view.Gobj_TexGroup.SetActive(true);
                    view.Gobj_IndicateGroup.SetActive(false);
                    view.Gobj_EliteCrusadeGroup.SetActive(true);
                    view.Spt_EscortBgSprite.gameObject.SetActive(false);
                    Scheduler.Instance.AddFrame(1, false, UpdateChapterBG);
                    Main.Instance.StartCoroutine(UpdateEliteGateButton());
                }
                break;
        }
    }

    public void UpdateAwardsButton()
    {
        currentStar = 0;

        for (int i = 0; i < currentChapterList.Count; i++)
        {
            StageData _stagedata = currentChapterList[i];
            if (_stagedata.gateinfo != null)
            {
                if (_stagedata.stageinfo.ChapterID == chapterinfo.id)
                {
                    //非BOSS关卡星星不进入计数
                    if (_stagedata.stageinfo.IsElite == (byte)battletype && _stagedata.stageinfo.IsBoss)
                    {
                        currentStar += _stagedata.gateinfo.star_level;
                    }
                }
            }
        }
        view.Lbl_ChapterStar.text = currentStar.ToString();
    }

    public void UpdateAwardsStatus()
    {
        if (chapterinfo == null)
        {
            return;
        }
        List<CommonItemData> list = null;
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    crusadeChapterID = chapterinfo.id;
                    list = chapterinfo.crusadeAwards;
                }
                break;
            case MainBattleType.Escort:
                {
                    escortChapterID = chapterinfo.id;
                    list = chapterinfo.escortAwards;
                }
                break;
            case MainBattleType.EliteCrusade:
                {
                    eliteCrusadeChapterID = chapterinfo.id;
                    list = chapterinfo.eliteCrusadeAwards;
                }
                break;
        }
        if (list == null)
        {
            return;
        }
        view.Lbl_LowAwardsNum.text = list[0].Num.ToString();
        view.Lbl_MidAwardsNum.text = list[1].Num.ToString();
        view.Lbl_HighAwardsNum.text = list[2].Num.ToString();
        int allStar = list[2].Num;
        view.Slider_AwradsSlider.value = (float)currentStar / (float)allStar;
        if (currentStar < list[0].Num)
        {
            view.Gobj_LowChests.SetActive(false);
            CommonFunction.SetSpriteName(view.Spt_LowAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[0]);
        }
        else
        {
            if (IsHadGetAwards(list[0].Num))
            {
                view.Gobj_LowChests.SetActive(false);
                CommonFunction.SetSpriteName(view.Spt_LowAwardsChest, GlobalConst.SpriteName.LivenessBoxIconsOpen[0]);
            }
            else
            {
                view.Gobj_LowChests.SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_LowAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[0]);
            }
        }
        if (currentStar < list[1].Num)
        {
            view.Gobj_MidChests.SetActive(false);
            CommonFunction.SetSpriteName(view.Spt_MidAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[4]);
        }
        else
        {
            if (IsHadGetAwards(list[1].Num))
            {
                view.Gobj_MidChests.SetActive(false);
                CommonFunction.SetSpriteName(view.Spt_MidAwardsChest, GlobalConst.SpriteName.LivenessBoxIconsOpen[4]);
            }
            else
            {
                view.Gobj_MidChests.SetActive(true);

                CommonFunction.SetSpriteName(view.Spt_MidAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[4]);
            }
        }
        if (currentStar < list[2].Num)
        {
            view.Gobj_HightChests.SetActive(false);
            CommonFunction.SetSpriteName(view.Spt_HighAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[5]);
        }
        else
        {
            if (IsHadGetAwards(list[2].Num))
            {
                view.Gobj_HightChests.SetActive(false);
                CommonFunction.SetSpriteName(view.Spt_HighAwardsChest, GlobalConst.SpriteName.LivenessBoxIconsOpen[5]);
            }
            else
            {
                view.Gobj_HightChests.SetActive(true);
                CommonFunction.SetSpriteName(view.Spt_HighAwardsChest, GlobalConst.SpriteName.LivenessBoxIcons[5]);
            }
        }
    }

    public void UpdateChapterBG()
    {
        if (chapterinfo == null)
        {
            return;
        }
        ResourceLoadManager.Instance.LoadAloneImage(chapterinfo.resicon, (texture) =>
        {
            if (view.Tex_ChapterBg != null)
            {
                view.Tex_ChapterBg.mainTexture = texture;
                //view.Tex_ChapterBg.MakePixelPerfect();
            }
        });
    }

    /// <summary>
    /// 生成关卡按钮
    /// </summary>
    private IEnumerator UpdateGateButton()
    {
        if (currentChapterList != null)
        {
            if (currentChapterList.Count <= crusadeBtn_Dic.Count)
            {
                for (int i = 0; i < crusadeBtn_Dic.Count; i++)
                {
                    CrusadeStageButtonComponent comp = crusadeBtn_Dic[i];
                    if (i < currentChapterList.Count)
                    {
                        StageData tStageData = currentChapterList[i];
                        comp.UpdateInfo(tStageData);
                        comp.mRootObject.transform.localPosition = new Vector3(tStageData.stageinfo.IconPos.x, tStageData.stageinfo.IconPos.y, 0);
                        if (comp.IsCurrent)
                        {
                            UpdateIndicate(comp.mRootObject, tStageData.stageinfo.IsBoss);
                        }
                        comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.mRootObject.SetActive(false);
                    }
                }
            }
            else
            {
                int btn_index = crusadeBtn_Dic.Count;
                for (int i = 0; i < currentChapterList.Count; i++)
                {
                    StageData tStageData = currentChapterList[i];
                    if (tStageData == null)
                    {
                        continue;
                    }
                    CrusadeStageButtonComponent comp = null;
                    if (i < btn_index)
                    {
                        comp = crusadeBtn_Dic[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_Gate.gameObject, view.Gobj_GateGroup.transform);
                        comp = new CrusadeStageButtonComponent();
                        comp.MyStart(go);
                        go.name = tStageData.stageinfo.ID.ToString();
                        comp.AddEventListener(ButtonEvent_StageButton);
                        crusadeBtn_Dic.Add(comp);
                    }
                    if (comp == null || comp.mRootObject == null) continue;
                    comp.mRootObject.transform.localPosition = new Vector3(tStageData.stageinfo.IconPos.x, tStageData.stageinfo.IconPos.y, 0);
                    comp.UpdateInfo(tStageData);
                    if (comp.IsCurrent)
                    {
                        UpdateIndicate(comp.mRootObject, tStageData.stageinfo.IsBoss);
                    }
                    comp.mRootObject.SetActive(true);
                }
            }
        }
        yield return null;
        if (chapterinfo != null)
        {
            List<Vector2> pointLocals = new List<Vector2>(chapterinfo.pointPos);
            if (pointLocals.Count <= gatePointsList.Count)
            {
                for (int i = 0; i < gatePointsList.Count; i++)
                {
                    UISprite pointObj = gatePointsList[i];
                    if (i < pointLocals.Count)
                    {
                        Vector2 vector = pointLocals[i];
                        pointObj.name = "point_" + i;
                        pointObj.transform.localPosition = new Vector3(vector.x, vector.y, 0);
                        pointObj.enabled = true;
                    }
                    else
                    {
                        pointObj.enabled = false;
                    }
                }
            }
            else
            {
                int go_index = gatePointsList.Count;
                for (int i = 0; i < pointLocals.Count; i++)
                {
                    Vector2 vector = pointLocals[i];
                    UISprite tempgo = null;
                    if (i < go_index)
                    {
                        tempgo = gatePointsList[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Spt_Point.gameObject, view.Gobj_GateGroup.transform);
                        go.name = "point_" + i;
                        tempgo = go.GetComponent<UISprite>();
                        if (tempgo == null) continue;
                        gatePointsList.Add(tempgo);
                    }
                    if (tempgo == null) continue;
                    tempgo.transform.localPosition = new Vector3(vector.x, vector.y, 0);
                    tempgo.gameObject.SetActive(true);
                    tempgo.enabled = true;
                }
            }
        }
    }

    private IEnumerator UpdateEliteGateButton()
    {
        if (currentChapterList != null)
        {
            if (currentChapterList.Count <= eliteCrusadeBtn_Dic.Count)
            {
                for (int i = 0; i < eliteCrusadeBtn_Dic.Count; i++)
                {
                    EliteCrusadeStageBtnComponent comp = eliteCrusadeBtn_Dic[i];
                    if (i < currentChapterList.Count)
                    {
                        StageData tStageData = currentChapterList[i];
                        comp.UpdateInfo(tStageData);
                        comp.mRootObject.transform.localPosition = new Vector3(tStageData.stageinfo.IconPos.x, tStageData.stageinfo.IconPos.y, 0);
                        if (comp.IsCurrent)
                        {
                            UpdateIndicate(comp.mRootObject, tStageData.stageinfo.IsBoss);
                        }
                        comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.mRootObject.SetActive(false);
                    }
                }
            }
            else
            {
                int btn_index = eliteCrusadeBtn_Dic.Count;
                for (int i = 0; i < currentChapterList.Count; i++)
                {
                    StageData tStageData = currentChapterList[i];
                    if (tStageData == null)
                    {
                        continue;
                    }
                    EliteCrusadeStageBtnComponent comp = null;
                    if (i < btn_index)
                    {
                        comp = eliteCrusadeBtn_Dic[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_EliteCrusade.gameObject, view.Gobj_EliteCrusadeGroup.transform);
                        comp = new EliteCrusadeStageBtnComponent();
                        comp.MyStart(go);
                        go.name = tStageData.stageinfo.ID.ToString();
                        comp.AddEventListener(ButtonEvent_EliteCrusadeObj);
                        eliteCrusadeBtn_Dic.Add(comp);
                    }
                    if (comp == null || comp.mRootObject == null) continue;
                    comp.mRootObject.transform.localPosition = new Vector3(tStageData.stageinfo.IconPos.x, tStageData.stageinfo.IconPos.y, 0);
                    comp.UpdateInfo(tStageData);
                    if (comp.IsCurrent)
                    {
                        UpdateIndicate(comp.mRootObject, tStageData.stageinfo.IsBoss);
                    }
                    comp.mRootObject.SetActive(true);
                }
            }
        }
        yield return null;
        if (chapterinfo != null)
        {
            List<Vector2> pointLocals = new List<Vector2>(chapterinfo.pointPos);
            if (pointLocals.Count <= gatePointsList.Count)
            {
                for (int i = 0; i < gatePointsList.Count; i++)
                {
                    UISprite pointObj = gatePointsList[i];
                    if (i < pointLocals.Count)
                    {
                        Vector2 vector = pointLocals[i];
                        pointObj.name = "point_" + i;
                        pointObj.transform.localPosition = new Vector3(vector.x, vector.y, 0);
                        pointObj.enabled = true;
                    }
                    else
                    {
                        pointObj.enabled = false;
                    }
                }
            }
            else
            {
                int go_index = gatePointsList.Count;
                for (int i = 0; i < pointLocals.Count; i++)
                {
                    Vector2 vector = pointLocals[i];
                    UISprite tempgo = null;
                    if (i < go_index)
                    {
                        tempgo = gatePointsList[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Spt_Point.gameObject, view.Gobj_GateGroup.transform);
                        go.name = "point_" + i;
                        tempgo = go.GetComponent<UISprite>();
                        if (tempgo == null) continue;
                        gatePointsList.Add(tempgo);
                    }
                    if (tempgo == null) continue;
                    tempgo.transform.localPosition = new Vector3(vector.x, vector.y, 0);
                    tempgo.gameObject.SetActive(true);
                    tempgo.enabled = true;
                }
            }
        }
        yield return null;
    }

    public IEnumerator UpdateCurrentItem()
    {
        if (currentChapterList == null)
            currentChapterList = new List<StageData>();
        view.ScrView_CurrentScrollView.ResetPosition();
        yield return null;
        int MAXCOUNT = 6;
        int count = currentChapterList.Count;
        int itemCount = currentEscortBtn_Dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_CurrentGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_CurrentGrid.minIndex = -index;
        view.UIWrapContent_CurrentGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_CurrentGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_CurrentGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                currentEscortBtn_Dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            StageData stagedata = currentChapterList[i];
            if (stagedata == null) continue;
            EscortStageComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_EscortComp, view.Grd_CurrentGrid.transform);
                vGo.name = i.ToString();
                comp = new EscortStageComponent();
                comp.MyStart(vGo);
                currentEscortBtn_Dic.Add(comp);
            }
            else
            {
                comp = currentEscortBtn_Dic[i];
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(stagedata);
            comp.onChallengeHandle = ButtonEvent_Challenge;
            comp.onAwradItemHandle = ButtonEvent_AwardItem;
            comp.mRootObject.SetActive(true);
        }
        view.UIWrapContent_CurrentGrid.ReGetChild();
        yield return null;
        view.Grd_CurrentGrid.Reposition();
        yield return null;
        view.ScrView_CurrentScrollView.ResetPosition();
        yield return null;
        SetAnimation();
    }


    private IEnumerator UpdateHideItem()
    {
        if (currentChapterList == null)
            currentChapterList = new List<StageData>();
        view.ScrView_HideScrollView.ResetPosition();
        yield return null;
        int MAXCOUNT = 6;
        int count = currentChapterList.Count;
        int itemCount = hideEscortBtn_Dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_HideGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_HideGrid.minIndex = -index;
        view.UIWrapContent_HideGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_HideGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_HideGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                hideEscortBtn_Dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            StageData stagedata = currentChapterList[i];
            if (stagedata == null) continue;
            EscortStageComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_EscortComp, view.Grd_HideGrid.transform);
                vGo.name = i.ToString();
                comp = new EscortStageComponent();
                comp.MyStart(vGo);
                hideEscortBtn_Dic.Add(comp);
            }
            else
            {
                comp = hideEscortBtn_Dic[i];
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(stagedata);
            comp.onChallengeHandle = ButtonEvent_Challenge;
            comp.onAwradItemHandle = ButtonEvent_AwardItem;
            comp.mRootObject.SetActive(true);
        }
        view.UIWrapContent_HideGrid.ReGetChild();
        yield return null;
        view.Grd_HideGrid.Reposition();
        yield return null;
        view.ScrView_HideScrollView.ResetPosition();
        yield return null;
        SetAnimation();
    }


    private void UpdateIndicate(GameObject go, bool isBoss)
    {
        Vector3 vector3 = new Vector3(0, 40, 0);
        if (isBoss)
        {
            vector3 = new Vector3(0, 70, 0);
        }
        view.Gobj_IndicateGroup.SetActive(true);
        view.Gobj_IndicateGroup.transform.localPosition = go.transform.localPosition + vector3;
    }
    private bool CheckGateActive(MainBattleType type)
    {
        bool isEnable = false;
        for (int i = 0; i < stagedatalist.Count; i++)
        {
            StageData data = stagedatalist[i];
            if (data == null || data.stageinfo == null) continue;
            if (data.stageinfo.IsElite == (byte)type)
            {
                isEnable = true;
                break;
            }
        }
        return isEnable;
    }

    #region Button Event
    private void ButtonEvent_AwardItem(EscortItemComponent comp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (comp == null) return;
        HintManager.Instance.SeeDetail(comp.mRootObject, isPress, comp.ItemID);
    }
    private void ButtonEvent_Challenge(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        EscortStageComponent comp = baseComp as EscortStageComponent;
        if (comp == null) return;
        if (!comp.IsEnable)
        {
            return;
        }
        if (comp.StageData.stageinfo.UnlockLV > PlayerData.Instance._Level)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, comp.StageData.stageinfo.UnlockLV));
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
            UISystem.Instance.GateInfoView.UpdateViewInfo(comp.StageData.stageinfo, comp.StageData.gateinfo, comp.StageData.remainRaidTimes, EFightType.eftMain);
        }
    }

    /// <summary>
    /// 点击关卡
    /// </summary>
    /// <param name="baseComp"></param>
    private void ButtonEvent_StageButton(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        CrusadeStageButtonComponent comp = baseComp as CrusadeStageButtonComponent;
        if (comp == null) return;
        if (!comp.IsEnable)
        {
            return;
        }
        if (comp.stageData.stageinfo.UnlockLV > PlayerData.Instance._Level)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, comp.stageData.stageinfo.UnlockLV));
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
            UISystem.Instance.GateInfoView.UpdateViewInfo(comp.stageData.stageinfo, comp.stageData.gateinfo, comp.stageData.remainRaidTimes, EFightType.eftMain);
        }
    }

    private void ButtonEvent_EliteCrusadeObj(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        EliteCrusadeStageBtnComponent comp = baseComp as EliteCrusadeStageBtnComponent;
        if (comp == null) return;
        if (!comp.IsEnable)
        {
            return;
        }
        if (comp.StageData.stageinfo.UnlockLV > PlayerData.Instance._Level)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_LINEUPPANEL_LEVELLIMIT, comp.StageData.stageinfo.UnlockLV));
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GATEINFO);
            UISystem.Instance.GateInfoView.UpdateViewInfo(comp.StageData.stageinfo, comp.StageData.gateinfo, comp.StageData.remainRaidTimes, EFightType.eftMain);
        }
    }
    public void ButtonEvent_Crusade(GameObject btn)
    {
        if (battletype == MainBattleType.Crusade)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (crusadeChapterID.Equals(0))
        {
            chapterinfo = null;
            UpdateViewInfo(MainBattleType.Crusade);
        }
        else
        {
            battletype = MainBattleType.Crusade;
            chapterinfo = ConfigManager.Instance.mChaptersData.GetChapterByID(crusadeChapterID);
            UpdatePanelInfo();
        }
    }

    public void ButtonEvent_Escort(GameObject btn)
    {
        if (battletype == MainBattleType.Escort)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (CheckGateActive(MainBattleType.Escort) == false)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LOCKELITE);
            return;
        }
        isPlayingAnim = false;
        view.Gobj_CurrentGroup.transform.localPosition = currentPos;
        view.Gobj_HideGroup.transform.localPosition = hidePos;
        PlayAnimStatus = PlayAnimState.None;
        if (escortChapterID.Equals(0))
        {
            chapterinfo = null;
            UpdateViewInfo(MainBattleType.Escort);
        }
        else
        {
            chapterinfo = ConfigManager.Instance.mChaptersData.GetChapterByID(escortChapterID);
            battletype = MainBattleType.Escort;
            UpdatePanelInfo();
        }
    }

    public void ButtonEvent_EliteCrusade(GameObject btn)
    {
        if (battletype == MainBattleType.EliteCrusade)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (CheckGateActive(MainBattleType.EliteCrusade) == false)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_LOCKELITE);
            return;
        }
        if (eliteCrusadeChapterID.Equals(0))
        {
            chapterinfo = null;
            UpdateViewInfo(MainBattleType.EliteCrusade);
        }
        else
        {
            chapterinfo = ConfigManager.Instance.mChaptersData.GetChapterByID(eliteCrusadeChapterID);
            battletype = MainBattleType.EliteCrusade;
            UpdatePanelInfo();
        }
    }
    public void ButtonEvent_LowChapterAwardsButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (chapterinfo == null) return;
        CommonItemData commonData = null;
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    commonData = chapterinfo.crusadeAwards[0];
                } break;
            case MainBattleType.Escort:
                {
                    commonData = chapterinfo.escortAwards[0];
                } break;
            case MainBattleType.EliteCrusade:
                {
                    commonData = chapterinfo.eliteCrusadeAwards[0];
                } break;
        }
        if (commonData == null)
        {
            return;
        }
        if (currentStar < commonData.Num)
        {
            List<CommonItemData> list = CommonFunction.GetCommonItemDataList(commonData.ID);
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
        }
        else
        {
            if (!IsHadGetAwards(commonData.Num))
            {
                List<CommonItemData> list = CommonFunction.GetCommonItemDataList(commonData.ID);
                FightRelatedModule.Instance.SendDungeonStarReward(chapterinfo.id, (uint)battletype, commonData.Num);
            }
        }
    }
    public void ButtonEvent_MidChapterAwardsButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (chapterinfo == null) return;
        CommonItemData commonData = null;
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    commonData = chapterinfo.crusadeAwards[1];
                } break;
            case MainBattleType.Escort:
                {
                    commonData = chapterinfo.escortAwards[1];
                } break;
            case MainBattleType.EliteCrusade:
                {
                    commonData = chapterinfo.eliteCrusadeAwards[1];
                } break;
        }
        if (commonData == null)
        {
            return;
        }
        if (currentStar < commonData.Num)
        {
            List<CommonItemData> list = CommonFunction.GetCommonItemDataList(commonData.ID);
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
        }
        else
        {
            if (!IsHadGetAwards(commonData.Num))
            {
                FightRelatedModule.Instance.SendDungeonStarReward(chapterinfo.id, (uint)battletype, commonData.Num);
            }
        }
    }
    public void ButtonEvent_HighChapterAwardsButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (chapterinfo == null) return;
        CommonItemData commonData = null;
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    commonData = chapterinfo.crusadeAwards[2];
                } break;
            case MainBattleType.Escort:
                {
                    commonData = chapterinfo.escortAwards[2];
                } break;
            case MainBattleType.EliteCrusade:
                {
                    commonData = chapterinfo.eliteCrusadeAwards[2];
                } break;
        }
        if (commonData == null)
        {
            return;
        }
        if (currentStar < commonData.Num)
        {
            List<CommonItemData> list = null;
            list = CommonFunction.GetCommonItemDataList(commonData.ID);
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_CANGET);
        }
        else
        {
            if (!IsHadGetAwards(commonData.Num))
            {
                FightRelatedModule.Instance.SendDungeonStarReward(chapterinfo.id, (uint)battletype, commonData.Num);
            }
        }
    }
    private bool IsHadGetAwards(int star)
    {
        if (PlayerData.Instance._GetAwardChapters == null) return false;
        fogs.proto.msg.DungeonStar award = PlayerData.Instance._GetAwardChapters.Find((tmp) =>
        {
            if (tmp == null) return false;
            if (tmp.chapter_id.Equals(chapterinfo.id) && tmp.dgn_type == (int)battletype)
            {
                return true;
            }
            else { return false; }
        });
        if (award != null)
        {
            if (award.stars.Contains(star))
            {
                return true;
            }
        }
        return false;
    }

    public void ReceiveDungeonStarReward(DropList data)
    {
        UpdateAwardsButton();
        UpdateAwardsStatus();
        List<fogs.proto.msg.ItemInfo> specialList = new List<fogs.proto.msg.ItemInfo>();
        specialList.AddRange(data.special_list);
        specialList.AddRange(data.item_list);
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(specialList, data.equip_list, data.soldier_list);
        UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_SUCCESS);

        if (data.mail_list != null && data.mail_list.Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.RECIEVE_FAILED);
        }
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_GETAWARDSUCCESS);
    }

    public void ButtonEvent_ExitButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GATE);
        //UISystem.Instance.GateView.PlayOpenAnim();
        //Main.Instance.ChangeState(MainCityState.StateName);
    }

    public void ButtonEvent_LeftArrowButton(GameObject btn)
    {
        if (battletype == MainBattleType.Escort)
        {
            if (isPlayingAnim)
                return;
            isPlayingAnim = true;
            PlayAnimStatus = PlayAnimState.RightToLeft;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        chapterinfo = ConfigManager.Instance.mChaptersData.GetChapterByID(chapterinfo.previd);
        UpdatePanelInfo();
    }

    private void UpdateChapterName()
    {
        view.Lbl_ChapterName.text = chapterinfo.name;
        //if (chapterinfo != null)
        //{
        //    string name = chapterinfo.name;
        //    string tmp = string.Empty;
        //    char[] str = name.ToCharArray();
        //    for (int i = 0; i < str.Length; i++)
        //    {
        //        tmp += str[i] + "\n";
        //    }
        //    view.Lbl_ChapterName.text = tmp;
        //}
    }

    public void ButtonEvent_RightArrowButton(GameObject btn)
    {
        if (battletype == MainBattleType.Escort)
        {
            if (isPlayingAnim)
                return;
            isPlayingAnim = true;
            PlayAnimStatus = PlayAnimState.LeftToRight;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ChapterInfo _info = ConfigManager.Instance.mChaptersData.GetChapterByID(chapterinfo.nextid);
        if (_info == null)
        {
            return;
        }
        if (PlayerData.Instance._Level < _info.LVLimit)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, string.Format(ConstString.GATE_OPENNEXTCHAPTER, _info.LVLimit));
            return;
        }
        //if (!IsEnableChapter(chapterinfo.nextid))
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.GATE_GATENOTOPEN);
        //    return;
        //}
        chapterinfo = _info;// ConfigManager.Instance.mChaptersData.GetChapterByID(chapterinfo.nextid);
        UpdatePanelInfo();
    }
    #endregion

    private void SetAnimation()
    {
        isPlayingAnim = true;
        switch (PlayAnimStatus)
        {
            case PlayAnimState.None:
                isPlayingAnim = false;
                return;
            case PlayAnimState.RightToLeft:
                {
                    if (view.Gobj_CurrentGroup.transform.localPosition.x == currentPos.x)
                    {
                        view.Gobj_HideGroup.transform.localPosition = -hidePos;
                        Gobj_EscortLeft = view.Gobj_HideGroup.gameObject;
                        Gobj_EscortRight = view.Gobj_CurrentGroup.gameObject;
                    }
                    else
                    {
                        view.Gobj_CurrentGroup.transform.localPosition = -hidePos;
                        Gobj_EscortLeft = view.Gobj_CurrentGroup.gameObject;
                        Gobj_EscortRight = view.Gobj_HideGroup.gameObject;
                    }
                    Scheduler.Instance.AddTimer(0.001f, true, PlayAnimation);
                }
                break;
            case PlayAnimState.LeftToRight:
                {
                    if (view.Gobj_CurrentGroup.transform.localPosition.x == currentPos.x)
                    {
                        view.Gobj_HideGroup.transform.localPosition = hidePos;
                        Gobj_EscortLeft = view.Gobj_CurrentGroup.gameObject;
                        Gobj_EscortRight = view.Gobj_HideGroup.gameObject;
                    }
                    else
                    {
                        view.Gobj_CurrentGroup.transform.localPosition = hidePos;
                        Gobj_EscortLeft = view.Gobj_HideGroup.gameObject;
                        Gobj_EscortRight = view.Gobj_CurrentGroup.gameObject;
                    }
                    Scheduler.Instance.AddTimer(0.001f, true, PlayAnimation);
                }
                break;
        }
    }
    private GameObject Gobj_EscortLeft;
    private GameObject Gobj_EscortRight;

    private void PlayAnimation()
    {
        switch (PlayAnimStatus)
        {
            case PlayAnimState.None: return;
            case PlayAnimState.RightToLeft:
                {
                    if ((Gobj_EscortLeft.transform.localPosition.x >= 0) && (Gobj_EscortRight.transform.localPosition.x >= hidePos.x))
                    {
                        Gobj_EscortLeft.transform.localPosition = currentPos;
                        Gobj_EscortRight.transform.localPosition = hidePos;
                        Gobj_EscortLeft.SetActive(false);
                        Gobj_EscortLeft.SetActive(true);
                        Scheduler.Instance.RemoveTimer(PlayAnimation);
                        isPlayingAnim = false;
                    }
                    else
                    {
                        Gobj_EscortLeft.transform.localPosition += 50 * Vector3.right;
                        Gobj_EscortRight.transform.localPosition += 50 * Vector3.right;
                    }
                }
                break;
            case PlayAnimState.LeftToRight:
                {
                    if ((Gobj_EscortLeft.transform.localPosition.x <= -hidePos.x) && (Gobj_EscortRight.transform.localPosition.x <= 0))
                    {
                        Gobj_EscortLeft.transform.localPosition = -hidePos;
                        Gobj_EscortRight.transform.localPosition = currentPos;
                        Gobj_EscortRight.SetActive(false);
                        Gobj_EscortRight.SetActive(true);
                        Scheduler.Instance.RemoveTimer(PlayAnimation);
                        isPlayingAnim = false;
                    }
                    else
                    {
                        Gobj_EscortLeft.transform.localPosition += 50 * Vector3.left;
                        Gobj_EscortRight.transform.localPosition += 50 * Vector3.left;
                    }
                }
                break;
        }
    }

    private void UpdateCurrentGrd(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_CurrentGrid.enabled) return;
        if (realIndex >= currentChapterList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        EscortStageComponent comp = currentEscortBtn_Dic[wrapIndex];
        comp.UpdateCompInfo(currentChapterList[realIndex]);
    }

    private void UpdateHideGrd(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_HideGrid.enabled) return;
        if (realIndex >= currentChapterList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        EscortStageComponent comp = hideEscortBtn_Dic[wrapIndex];
        comp.UpdateCompInfo(currentChapterList[realIndex]);
    }

    /// <summary>
    /// 更新章节选择按钮状态
    /// </summary>
    private void UpdateArrowStatus()
    {
        if (chapterinfo != null)
        {
            if (chapterinfo.previd == 0)   //无上一章节
            {
                view.Btn_LeftArrow.gameObject.SetActive(false);
            }
            else
            {
                view.Btn_LeftArrow.gameObject.SetActive(true);
            }
            if (chapterinfo.nextid == 0)   //无下一章节
            {
                view.Btn_RightArrow.gameObject.SetActive(false);
            }
            else
            {
                bool ieEnable = false;
                for (int i = 0; i < stagedatalist.Count; i++)
                {
                    StageData stageData = stagedatalist[i];
                    if (stageData.stageinfo.ChapterID == chapterinfo.nextid)
                    {
                        if (stageData.stageinfo.IsElite == (int)battletype)
                        {
                            ieEnable = true;
                            break;
                        }
                    }
                }
                view.Btn_RightArrow.gameObject.SetActive(ieEnable);
            }
        }
    }

    private void UpdateAwardDesc()
    {
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    string desc = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.CRUSADE_AWARD_DESC);
                    view.Lbl_AwardDesc.text = desc;
                } break;
            case MainBattleType.Escort:
                {
                    string desc = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.ESCORT_AWARD_DESC);
                    view.Lbl_AwardDesc.text = desc;
                } break;
            case MainBattleType.EliteCrusade:
                {
                    string desc = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.ELITE_AWARD_DESC);
                    view.Lbl_AwardDesc.text = desc;
                } break;
        }
    }

    private void UpdateBattleTypeStatus()
    {
        switch (battletype)
        {
            case MainBattleType.Crusade:
                {
                    CommonFunction.SetSpriteName(view.Spt_BtnCrusadeType, GlobalConst.SpriteName.GATE_CRUSADE_SELECT);
                    CommonFunction.SetSpriteName(view.Spt_BtnEscortType, GlobalConst.SpriteName.GATE_ESCORT_NORMAL);
                    CommonFunction.SetSpriteName(view.Spt_BtnEliteCrusadeType, GlobalConst.SpriteName.GATE_ELITECRUSADE_NORMAL);
                } break;
            case MainBattleType.Escort:
                {
                    CommonFunction.SetSpriteName(view.Spt_BtnCrusadeType, GlobalConst.SpriteName.GATE_CRUSADE_NORMAL);
                    CommonFunction.SetSpriteName(view.Spt_BtnEscortType, GlobalConst.SpriteName.GATE_ESCORT_SELECT);
                    CommonFunction.SetSpriteName(view.Spt_BtnEliteCrusadeType, GlobalConst.SpriteName.GATE_ELITECRUSADE_NORMAL);
                } break;
            case MainBattleType.EliteCrusade:
                {
                    CommonFunction.SetSpriteName(view.Spt_BtnCrusadeType, GlobalConst.SpriteName.GATE_CRUSADE_NORMAL);
                    CommonFunction.SetSpriteName(view.Spt_BtnEscortType, GlobalConst.SpriteName.GATE_ESCORT_NORMAL);
                    CommonFunction.SetSpriteName(view.Spt_BtnEliteCrusadeType, GlobalConst.SpriteName.GATE_ELITECRUSADE_SELECT);
                } break;
        }
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Crusade.gameObject).onClick = ButtonEvent_Crusade;
        UIEventListener.Get(view.Btn_Escort.gameObject).onClick = ButtonEvent_Escort;
        UIEventListener.Get(view.Btn_EliteCrusade.gameObject).onClick = ButtonEvent_EliteCrusade;
        UIEventListener.Get(view.Btn_LowAwards.gameObject).onClick = ButtonEvent_LowChapterAwardsButton;
        UIEventListener.Get(view.Btn_MidAwards.gameObject).onClick = ButtonEvent_MidChapterAwardsButton;
        UIEventListener.Get(view.Btn_HighAwards.gameObject).onClick = ButtonEvent_HighChapterAwardsButton;
        UIEventListener.Get(view.Btn_Exit.gameObject).onClick = ButtonEvent_ExitButton;
        UIEventListener.Get(view.Btn_LeftArrow.gameObject).onClick = ButtonEvent_LeftArrowButton;
        UIEventListener.Get(view.Btn_RightArrow.gameObject).onClick = ButtonEvent_RightArrowButton;
    }

    /// <summary>
    /// 关卡排序
    /// </summary>
    private List<StageInfo> SortGatesData(ChapterInfo info, MainBattleType type)
    {
        List<StageInfo> list = null;
        switch (type)
        {
            case MainBattleType.Crusade:
                list = ConfigManager.Instance.mStageData.GetStagesByID(info.gates, (byte)MainBattleType.Crusade);
                break;
            case MainBattleType.Escort:
                list = ConfigManager.Instance.mStageData.GetStagesByID(info.gates, (byte)MainBattleType.Escort);
                break;
            case MainBattleType.EliteCrusade:
                list = ConfigManager.Instance.mStageData.GetStagesByID(info.gates, (byte)MainBattleType.EliteCrusade);
                break;
            default:
                break;
        }
        if (list == null || list.Count == 0)
        {
            return list;
        }
        List<uint> idlist = new List<uint>();
        for (int i = 0; i < list.Count; i++)
        {
            idlist.Add(list[i].ID);
        }
        StageInfo firststage = null;
        for (int i = 0; i < list.Count; i++)
        {
            if (!idlist.Contains(list[i].PrevID))   //取得当前章节的第一个关卡
            {
                firststage = list[i];
            }
        }
        List<StageInfo> sortedlist = new List<StageInfo>();
        sortedlist.Add(firststage);
        uint tmpid = firststage.NextID;
        for (int i = 1; i < list.Count; i++)
        {
            StageInfo tmp = null;
            for (int j = 0; j < list.Count; j++)
            {
                if (tmpid == list[j].ID)
                {
                    tmp = list[j];
                    sortedlist.Add(tmp);
                    tmpid = tmp.NextID;
                    break;
                }
            }
        }
        return sortedlist;
    }
    //界面动画
    //public System.Collections.IEnumerator PlayOpenAnim()
    //{
    //    view.Anim_TScale.from = new Vector3(0.001f, 0.001f, 0.001f);
    //    view.Anim_TScale.to = Vector3.one;
    //    view.Anim_TScale.Restart();
    //    yield return null;
    //    view.Anim_TScale.PlayForward();
    //    view.Anim_TScale.SetOnFinished(ReceiveDungeonInfo);
    //}
    public override void Uninitialize()
    {
        base.Uninitialize();
        Scheduler.Instance.RemoveTimer(PlayAnimation);
        if (view.Tex_ChapterBg != null)
            view.Tex_ChapterBg.mainTexture = null;
        ResourceLoadManager.Instance.ReleaseRequestBundle();
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        crusadeBtn_Dic.Clear();
        gatePointsList.Clear();
        eliteCrusadeBtn_Dic.Clear();
        currentEscortBtn_Dic.Clear();
        hideEscortBtn_Dic.Clear();
    }
}

public class StageData
{
    public int remainRaidTimes;
    /// <summary>
    /// 服务器数据
    /// </summary>
    public fogs.proto.msg.PassDungeon gateinfo;
    /// <summary>
    /// 配置表数据
    /// </summary>
    public StageInfo stageinfo;

}