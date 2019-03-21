using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;

/// <summary>
/// 远征关卡数据
/// </summary>
public class ExpeditionStageInfo
{
    public ExpeditionStageComponent StageData;
    public ExpeditionStageComponent AwardData;

    public ExpeditionStageInfo(ExpeditionStageComponent vStage, ExpeditionStageComponent vAward)
    {
        StageData = vStage;
        AwardData = vAward;
    }

    public void Destroy()
    {
        StageData = null;
        AwardData = null;
    }
}

/// <summary>
/// 匹配类型
/// </summary>
public enum MatchType
{
    /// <summary>
    /// 请求匹配数据
    /// </summary>
    None = 0,
    /// <summary>
    /// 重新匹配
    /// </summary>
    ReMatch = 2
}

public class ExpeditionViewController : UIBase
{
    private const int BG_WIDTH = 830;
    private const int BG_HEIGHT = 288;
    private const int OFFSET_FOG_POSX = 455;
    private const int OFFSET_FOG_WIDTH = 40;
    private const int INIT_FOG_MOVE = 100;
    private const int LIMIT_MIN_FOG_WIDTH = 400;
    private const int LIMIT_SCREEN_POSX_LEFT = 0;
    private const int LIMIT_SCREEN_POSX_RIGHT = -1659;


    public ExpeditionView view;
    private Dictionary<uint, ExpeditionStageInfo> dicExpedition = new Dictionary<uint, ExpeditionStageInfo>();
    private bool isCreateItem = false;
    private int bgCount = 1;
    private int limitFogPos = 0;
    private ExpeditionChapterInfo infoExpeditionChapter;
    private List<UITexture> BGTextureList;


    public override void Initialize()
    {
        if (view == null)
        {
            view = new ExpeditionView();
            view.Initialize();
            isCreateItem = false;
            if (dicExpedition == null)
                dicExpedition = new Dictionary<uint, ExpeditionStageInfo>();
        }
        if (BGTextureList == null)
            BGTextureList = new List<UITexture>();
        BtnEventBinding();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        InitStatus();
        RefreshStatus();
        UpdateAwardDesc();
        GameActivityModule.Instance.OnSendQueryActivityTimeReq();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenExpeditionView);
    }

    public void InitDouble(List<ActivityTimeInfo> ActivityInfo)
    {
        view.CountDouble.SetActive(false);
        view.RewardDouble.SetActive(false);
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
    public void UpdateDoubelHit( ActivityTimeInfo timeinfo)
    {           

        switch ((GameActivityType)timeinfo.activity_type)
        {
            case GameActivityType.DropDouble_Expedition://
               // Debug.LogError(data.mName + "         " + data.mType + "            " + data.mID);

                view.RewardDouble.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            case GameActivityType.DropDouble_Endness://远征
               // Debug.LogError(data.mName + "         " + data.mType + "            " + data.mID);

                view.CountDouble.SetActive(CommonFunction.CheckActivityTime(timeinfo));
                break;
            default:
                //Debug.LogError(data.mName+"    "+data.mType);
                break;
        }
    }

    public override void Destroy()
    {
        base.Destroy();
        if (BGTextureList != null)
        {
            foreach (UITexture tmpSingleTex in BGTextureList)
            {
                if (tmpSingleTex != null)
                {
                    GameObject.Destroy(tmpSingleTex.gameObject);
                }
            }
            BGTextureList.Clear();
        }
        if (dicExpedition != null)
        {
            foreach (KeyValuePair<uint, ExpeditionStageInfo> tmpInfo in dicExpedition)
            {
                if (tmpInfo.Value == null)
                    continue;
                tmpInfo.Value.Destroy();
            }
            dicExpedition.Clear();
        }
        dicExpedition = null;
        infoExpeditionChapter = null;
        view = null;
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_EXPEDITION);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_Exchange.gameObject).onClick = ButtonEvent_Exchange;
        UIEventListener.Get(view.Btn_Rule.gameObject).onClick = ButtonEvent_Rule;
        UIEventListener.Get(view.Btn_ReStart.gameObject).onClick = ButtonEvent_ReStart;
    }


    /// <summary>
    /// 关闭视图
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EXPEDITION);
        Main.Instance.ChangeState(Assets.Script.Common.StateMachine.MainCityState.StateName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    }

    /// <summary>
    /// 兑换奖励
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Exchange(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(StoreView.UIName);
        UISystem.Instance.StoreView.ShowStore(ShopType.ST_HonorShop);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    }

    /// <summary>
    /// 规则
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Rule(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RULEVIEW);
        UISystem.Instance.RuleView.UpdateViewInfo(3);
    }

    /// <summary>
    /// 重置
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_ReStart(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        int count = FightRelatedModule.Instance.maxTimes - PlayerData.Instance._ExpeditionInfo.reset_times;
        if (count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXPEDTION_RESTARTCOUNT_NULL);
        }
        else
        {
            ReStartOperate();
        }
    }

    /// <summary>
    /// 点击关卡
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Stage(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ExpeditionStageComponent tmpInfo = (ExpeditionStageComponent)baseComp;
        if (tmpInfo == null)
            return;
        switch (tmpInfo.mGateStatus)
        {
            case ExpeditionInfo.GateStatus.UNLOCKED://解锁-战斗中//
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXPEDITIONINFO);
                    UISystem.Instance.ExpeditionInfoView.UpdateInfo();
                }
                break;
            case ExpeditionInfo.GateStatus.LOCKED://未解锁//
            case ExpeditionInfo.GateStatus.PASSED://通关-未奖励//
            case ExpeditionInfo.GateStatus.PASSED_REWARED://通关-已奖励//
            default:
                { }
                break;
        }
    }

    /// <summary>
    /// 点击奖励
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Award(BaseComponent baseComp)
    {
        ExpeditionStageComponent tmpInfo = (ExpeditionStageComponent)baseComp;
        if (tmpInfo == null)
            return;
        if (tmpInfo.mExpeditionData == null)
            return;
        switch (tmpInfo.mGateStatus)
        {
            case ExpeditionInfo.GateStatus.LOCKED://未解锁//
            case ExpeditionInfo.GateStatus.UNLOCKED://解锁-战斗中//
                {
                    List<CommonItemData> list = null;
                    list = CommonFunction.GetCommonItemDataList(tmpInfo.mExpeditionData.drop_bagid);
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
                    UISystem.Instance.RecieveResultVertView.UpdateExpedetionPreviewInfo(list, ConstString.RECEIVE_CANGET);
                }
                break;
            case ExpeditionInfo.GateStatus.PASSED://通关-未奖励//
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Chest, view._uiRoot.transform.parent.transform));
                    if (!CheckIsCanObtainGateReward(tmpInfo.mExpeditionData.drop_bagid))
                    {
                        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, string.Format(ConstString.EXPEDITION_MAXSOLDIER_FOR_REWARD, SoldierDepot.MAXCOUNT));
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.EXPEDITION_MAXSOLDIER_FOR_REWARD, SoldierDepot.MAXCOUNT));
                        return;
                    }
                    FightRelatedModule.Instance.SendExpeditionReward(tmpInfo.mExpeditionData.id);
                }
                break;
            case ExpeditionInfo.GateStatus.PASSED_REWARED://通关-已奖励//
                {
                    //其它操作//
                }
                break;
            default:
                { }
                break;
        }
    }


    /// <summary>
    /// 初始化界面
    /// </summary>
    private void InitStatus()
    {
        infoExpeditionChapter = ConfigManager.Instance.mExpeditionChapterConfig.FindByID(PlayerData.Instance._ExpeditionInfo.cur_chapter_id);//通过Server传来的数据查找本地配置表//
        bgCount = 1;
        limitFogPos = 0;
        view.Spt_FrontMask.gameObject.SetActive(false);
        InitBackGround();
        InitFog();
        CopyItem();
    }

    private void InitBackGround()
    {
        if (view.Tex_CurrentBG != null)
        {
            if (infoExpeditionChapter != null)
            {
                bgCount = infoExpeditionChapter.backdrop_pic.Count;
            }
            if (!isCreateItem)
            {
                if (BGTextureList != null)
                {
                    foreach (UITexture tmpSingleTex in BGTextureList)
                    {
                        if (tmpSingleTex != null)
                        {
                            GameObject.Destroy(tmpSingleTex.gameObject);
                        }
                    }
                    BGTextureList.Clear();
                }
                else
                {
                    BGTextureList = new List<UITexture>();
                }
                if ((infoExpeditionChapter != null) && (infoExpeditionChapter.backdrop_pic != null))
                {
                    for (int i = 0; i < bgCount; i++)
                    {
                        GameObject tmpObj_StageBack = CommonFunction.InstantiateObject(view.Tex_CurrentBG.gameObject, view.Tex_CurrentBG.transform.parent);
                        tmpObj_StageBack.transform.localPosition = new Vector3(i * BG_WIDTH, 0, 0);
                        ResourceLoadManager.Instance.LoadAloneImage(infoExpeditionChapter.backdrop_pic[i], (texture) =>
                        {
                            UITexture uiTexture = tmpObj_StageBack.GetComponent<UITexture>();
                            if (uiTexture != null)
                            {
                                uiTexture.mainTexture = texture;
                                uiTexture.gameObject.SetActive(true);
                                BGTextureList.Add(uiTexture);
                            }
                        });
                    }
                }
            }
            view.Tex_CurrentBG.gameObject.SetActive(false);
        }
    }

    private void InitFog()
    {
        limitFogPos = bgCount * BG_WIDTH - BG_WIDTH / 2;
        if (view.Spt_CurrentFog == null)
            return;
        view.Spt_CurrentFog.transform.localPosition = new Vector3(limitFogPos, 0, 0);
        ReSetFolgInfo(bgCount * BG_WIDTH);
    }

    private void UpdateAwardDesc()
    {
        view.Gobj_DescGroup.SetActive(true);
        view.Lbl_AwardDesc.text= ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.EXPEDITION_AWARD_DESC);
    }

    private void CopyItem()
    {
        if (isCreateItem)
            return;
        if (view == null)
            return;
        if (view.Spt_CurrentItem == null)
            return;
        view.Spt_CurrentItem.gameObject.SetActive(false);
        if (infoExpeditionChapter == null)
            return;

        isCreateItem = true;
        if (dicExpedition != null)
        {
            foreach (KeyValuePair<uint, ExpeditionStageInfo> tmpStage in dicExpedition)
            {
                if (tmpStage.Value != null)
                {
                    if (tmpStage.Value.StageData != null)
                    {
                        tmpStage.Value.StageData.Clear();
                    }
                    if (tmpStage.Value.AwardData != null)
                    {
                        tmpStage.Value.AwardData.Clear();
                    }
                }
            }
            dicExpedition.Clear();
        }
        else
        {
            dicExpedition = new Dictionary<uint, ExpeditionStageInfo>();
        }
        if (infoExpeditionChapter.dungeon_id != null)
        {
            for (int i = 0; i < infoExpeditionChapter.dungeon_id.Count; i++)
            {
                ExpeditionData tmpSingleInfo = ConfigManager.Instance.mExpeditionConfig.GetExpeditionDataByID(infoExpeditionChapter.dungeon_id[i]);
                if (tmpSingleInfo == null)
                    continue;

                GameObject tmpObj_Stage = CommonFunction.InstantiateObject(view.Spt_CurrentItem.gameObject, view.Spt_CurrentItem.transform.parent);
                ExpeditionStageComponent tmpComp_Stage = new ExpeditionStageComponent();
                tmpComp_Stage.MyStart(tmpObj_Stage);
                tmpComp_Stage.InitStatus(tmpSingleInfo, true);
                tmpComp_Stage.AddEventListener(ButtonEvent_Stage);

                GameObject tmpObj_Award = CommonFunction.InstantiateObject(view.Spt_CurrentItem.gameObject, view.Spt_CurrentItem.transform.parent);
                ExpeditionStageComponent tmpComp_Award = new ExpeditionStageComponent();
                tmpComp_Award.MyStart(tmpObj_Award);
                tmpComp_Award.InitStatus(tmpSingleInfo, false);
                tmpComp_Award.AddEventListener(ButtonEvent_Award);

                ExpeditionStageInfo tmpData = new ExpeditionStageInfo(tmpComp_Stage, tmpComp_Award);
                dicExpedition.Add(infoExpeditionChapter.dungeon_id[i], tmpData);
                //Debug.LogError(string.Format("[{0}][{1}, {2}, {3}]", i, infoExpeditionChapter.dungeon_id[i], tmpData.StageData.mExpeditionData.id, tmpData.AwardData.mExpeditionData.id));
            }
        }
    }

    private void ResetAwardStatus(uint vStageID)
    {
        if (dicExpedition == null)
            return;
        if (!dicExpedition.ContainsKey(vStageID))
            return;
        if (dicExpedition[vStageID].AwardData == null)
            return;
        dicExpedition[vStageID].AwardData.RefreshStatus(ExpeditionInfo.GateStatus.PASSED_REWARED);
    }

    private void ReSetArrowPos(GameObject vObj)
    {
        if (vObj == null)
            return;
        if (view.Trans_CurrentArrow == null)
            return;
        view.Trans_CurrentArrow.gameObject.SetActive(true);
        view.Trans_CurrentArrow.transform.localPosition = new Vector3(vObj.transform.localPosition.x - 415, vObj.transform.localPosition.y + 50, vObj.transform.localPosition.z);
    }

    private void ReSetFolgInfo(int vWidth)
    {
        //Debug.LogError("ReSetFolgInfo: " + vWidth);
        if (view.Spt_CurrentFog == null)
            return;

        view.Spt_CurrentFog.width = vWidth;
        view.Spt_CurrentFog.height = BG_HEIGHT; ;
        view.Spt_CurrentFog.GetComponent<BoxCollider>().size = new Vector3(vWidth, BG_HEIGHT, 0);
        view.Spt_CurrentFog.GetComponent<BoxCollider>().center = new Vector3(-vWidth / 2, 0, 0);
        if (vWidth < LIMIT_MIN_FOG_WIDTH)
            view.Spt_CurrentFog.gameObject.SetActive(false);
        else
            view.Spt_CurrentFog.gameObject.SetActive(true);
    }

    private void ReSetBGPos(GameObject vTargetObj = null)
    {
        int tmpPosX = LIMIT_SCREEN_POSX_LEFT;
        if (vTargetObj != null)
        {
            tmpPosX = (int)(415 - vTargetObj.transform.localPosition.x);
        }
        if (tmpPosX > LIMIT_SCREEN_POSX_LEFT)
            tmpPosX = LIMIT_SCREEN_POSX_LEFT;
        if (tmpPosX < LIMIT_SCREEN_POSX_RIGHT)
            tmpPosX = LIMIT_SCREEN_POSX_RIGHT;

        if (view.UIPanel_gobj_Content == null)
            return;
        view.UIPanel_gobj_Content.transform.localPosition = new Vector3(tmpPosX, 20, 0);
        view.UIPanel_gobj_Content.clipOffset = new Vector2(-tmpPosX, 0);
    }

    /// <summary>
    /// 检测是否能够获得关卡奖励
    /// </summary>
    /// <returns>true-可以领取奖励 false-不能领取奖励</returns>
    private bool CheckIsCanObtainGateReward(uint vDropID)
    {
        List<CommonItemData> tmpList = CommonFunction.GetCommonItemDataList(vDropID);
        if ((tmpList == null) || (tmpList.Count <= 0))
            return true;
        if (!PlayerData.Instance.IsSoldierFull())
            return true;
        foreach (CommonItemData tmpSingleInfo in tmpList)
        {
            if (tmpSingleInfo == null)
                continue;
            IDType tmpType = CommonFunction.GetTypeOfID(tmpSingleInfo.ID.ToString());
            if (tmpType == IDType.Soldier)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 刷新界面显示-获取Server端数据之后执行
    /// </summary>
    //public void RefreshStatus()
    //{
    //    if (dicExpedition == null)
    //        return;
    //    bool tmpIsSetBGPos = false;
    //    view.Spt_CurrentFog.gameObject.SetActive(false);
    //    view.Trans_CurrentArrow.gameObject.SetActive(false);
    //    for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.exp_gates.Count; i++)
    //    {
    //        uint tmpStageID = PlayerData.Instance._ExpeditionInfo.exp_gates[i].id;
    //        if (!dicExpedition.ContainsKey(tmpStageID))
    //            continue;
    //        if (dicExpedition[tmpStageID] == null)
    //            continue;
    //        if (dicExpedition[tmpStageID].StageData != null)
    //        {
    //            dicExpedition[tmpStageID].StageData.RefreshStatus(PlayerData.Instance._ExpeditionInfo.exp_gates[i].status);
    //            if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.UNLOCKED)
    //            {
    //                ReSetArrowPos(dicExpedition[tmpStageID].StageData.mRootObject);
    //                ReSetBGPos(dicExpedition[tmpStageID].StageData.mRootObject);
    //                tmpIsSetBGPos = true;
    //                view.Lbl_ChallengeNum.text = string.Format(ConstString.EXPEDITION_PROGRESS_CURRENT, dicExpedition[tmpStageID].StageData.mExpeditionData.progress, GlobalCoefficient.ExpeditionGateCount);
    //            }
    //        }
    //        if (dicExpedition[tmpStageID].AwardData != null)
    //        {
    //            dicExpedition[tmpStageID].AwardData.RefreshStatus(PlayerData.Instance._ExpeditionInfo.exp_gates[i].status);
    //            if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.UNLOCKED)
    //            {
    //                if (dicExpedition[tmpStageID].AwardData.mRootObject != null)
    //                {
    //                    ReSetFolgInfo((int)(view.Spt_CurrentFog.transform.localPosition.x - 40 - dicExpedition[tmpStageID].AwardData.mRootObject.transform.localPosition.x + 415));
    //                }
    //            }
    //        }
    //        if ((i == PlayerData.Instance._ExpeditionInfo.exp_gates.Count - 1) && (!tmpIsSetBGPos))
    //        {
    //            view.Lbl_ChallengeNum.text = string.Format(ConstString.EXPEDITION_PROGRESS_CURRENT, dicExpedition[tmpStageID].StageData.mExpeditionData.progress, GlobalCoefficient.ExpeditionGateCount);
    //            ReSetBGPos(dicExpedition[tmpStageID].StageData.mRootObject);
    //        }
    //    }

    //    int tmpCount = FightRelatedModule.Instance.maxTimes - PlayerData.Instance._ExpeditionInfo.reset_times;
    //    if (tmpCount <= 0)
    //        tmpCount = 0;
    //    view.Lbl_RestartNum.text = string.Format(ConstString.EXPEDITION_CURRENT_CHALLAGE, tmpCount);
    //}
    public void RefreshStatus(bool isReset = false)
    {
        if (isReset)
        {
            isCreateItem = false;
            InitStatus();
        }
        if (dicExpedition != null)
        {
            bool tmpIsSetBGPos = false;
            view.Spt_CurrentFog.gameObject.SetActive(false);
            view.Trans_CurrentArrow.gameObject.SetActive(false);

            if (PlayerData.Instance._ExpeditionInfo != null)
            {
                if (PlayerData.Instance._ExpeditionInfo.exp_gates != null)
                {
                    if (PlayerData.Instance._ExpeditionInfo.exp_gates.Count > 0)
                    {
                        for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.exp_gates.Count; i++)
                        {
                            uint tmpStageID = PlayerData.Instance._ExpeditionInfo.exp_gates[i].id;
                            if (dicExpedition.ContainsKey(tmpStageID))
                            {
                                if (dicExpedition[tmpStageID] != null)
                                {
                                    if (dicExpedition[tmpStageID].StageData != null)
                                    {
                                        dicExpedition[tmpStageID].StageData.RefreshStatus(PlayerData.Instance._ExpeditionInfo.exp_gates[i].status);
                                        if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.UNLOCKED)
                                        {
                                            ReSetArrowPos(dicExpedition[tmpStageID].StageData.mRootObject);
                                            ReSetBGPos(dicExpedition[tmpStageID].StageData.mRootObject);
                                            tmpIsSetBGPos = true;
                                            view.Lbl_ChallengeNum.text = string.Format(ConstString.EXPEDITION_PROGRESS_CURRENT, dicExpedition[tmpStageID].StageData.mExpeditionData.progress, GlobalCoefficient.ExpeditionGateCount);
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError(string.Format("Expedition Single Error: stage is null [{0}, {1}]", i, tmpStageID));
                                    }
                                    if (dicExpedition[tmpStageID].AwardData != null)
                                    {
                                        dicExpedition[tmpStageID].AwardData.RefreshStatus(PlayerData.Instance._ExpeditionInfo.exp_gates[i].status);
                                        if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.UNLOCKED)
                                        {
                                            if (dicExpedition[tmpStageID].AwardData.mRootObject != null)
                                            {
                                                ReSetFolgInfo((int)(view.Spt_CurrentFog.transform.localPosition.x - 40 - dicExpedition[tmpStageID].AwardData.mRootObject.transform.localPosition.x + 415));
                                            }
                                        }
                                    }
                                    else
                                    {
                                        Debug.LogError(string.Format("Expedition Single Error: award is null != null [{0}, {1}]", i, tmpStageID));
                                    }
                                    if ((i == PlayerData.Instance._ExpeditionInfo.exp_gates.Count - 1) && (!tmpIsSetBGPos))
                                    {
                                        view.Lbl_ChallengeNum.text = string.Format(ConstString.EXPEDITION_PROGRESS_CURRENT, dicExpedition[tmpStageID].StageData.mExpeditionData.progress, GlobalCoefficient.ExpeditionGateCount);
                                        ReSetBGPos(dicExpedition[tmpStageID].StageData.mRootObject);
                                    }
                                }
                                else
                                {
                                    Debug.LogError(string.Format("Expedition Single Error: data is null [{0}, {1}]", i, tmpStageID));
                                }
                            }
                            else
                            {
                                Debug.LogError(string.Format("Expedition Single Error: not find datas [{0}, {1}]", i, tmpStageID));
                            }
                        }
                    }
                    else
                    {
                        Debug.LogError("Expedition Error: count is wrong)");
                    }
                }
                else
                {
                    Debug.LogError("Expedition Error: gates is null)");
                }

                int tmpCount = FightRelatedModule.Instance.maxTimes - PlayerData.Instance._ExpeditionInfo.reset_times;
                if (tmpCount <= 0)
                    tmpCount = 0;
                view.Lbl_RestartNum.text = string.Format(ConstString.EXPEDITION_CURRENT_CHALLAGE, tmpCount);
            }
            else
            {
                Debug.LogError("Expedition Error: Info is null)");
            }
        }
        else
        {
            Debug.LogError("Expedition Error: DictionaryInfo is null)");
        }
    }

    /// <summary>
    /// 重置操作
    /// </summary>
    public void ReStartOperate()
    {
        if ((PlayerData.Instance._ExpeditionInfo == null) || (PlayerData.Instance._ExpeditionInfo.exp_gates == null))
        {
            FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.ReMatch);
            return;
        }

        bool tmpIsHaveAward = false;
        bool tmpIsNoPass = false;
        for (int i = 0; i < PlayerData.Instance._ExpeditionInfo.exp_gates.Count; i++)
        {
            if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.PASSED_REWARED)
            {
                continue;
            }
            else if (PlayerData.Instance._ExpeditionInfo.exp_gates[i].status == ExpeditionInfo.GateStatus.PASSED)
            {
                tmpIsHaveAward = true;
            }
            else
            {
                tmpIsNoPass = true;
            }
        }

        if (tmpIsHaveAward)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.EXPEDITION_RESTART_HINT_AWARD, () =>
            {
                if (tmpIsNoPass)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.EXPEDITION_RESTART_HINT, () =>
                    {
                        FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.ReMatch);
                    });
                }
                else
                {
                    FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.ReMatch);
                }
            });
            return;
        }
        if (tmpIsNoPass)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.EXPEDITION_RESTART_HINT, () =>
            {
                FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.ReMatch);
            });
            return;
        }
        FightRelatedModule.Instance.SendMatchEnemy((int)MatchType.ReMatch);
    }



    /// <summary>
    /// 获取奖励成功
    /// </summary>
    public void GetAwardsSuccess(uint vStageID, DropList vDrop)
    {
        view.Spt_FrontMask.gameObject.SetActive(false);
        ResetAwardStatus(vStageID);
        List<CommonItemData> list = new List<CommonItemData>();
        if (vDrop == null)
            return;
        if (vDrop.item_list != null)
        {
            for (int i = 0; i < vDrop.item_list.Count; i++)
            {
                fogs.proto.msg.ItemInfo item = vDrop.item_list[i];
                CommonItemData data = new CommonItemData(item.id, item.change_num);
                list.Add(data);
            }
        }
        if (vDrop.soldier_list != null)
        {
            for (int i = 0; i < vDrop.soldier_list.Count; i++)
            {
                fogs.proto.msg.Soldier soldier = vDrop.soldier_list[i];
                CommonItemData data = new CommonItemData(soldier.id, 1);
                list.Add(data);
            }
        }
        if (vDrop.equip_list != null)
        {
            for (int i = 0; i < vDrop.equip_list.Count; i++)
            {
                fogs.proto.msg.Equip equip = vDrop.equip_list[i];
                CommonItemData data = new CommonItemData(equip.id, 1);
                list.Add(data);
            }
        }
        if (vDrop.special_list != null)
        {
            for (int i = 0; i < vDrop.special_list.Count; i++)
            {
                fogs.proto.msg.ItemInfo item = vDrop.special_list[i];
                CommonItemData data = new CommonItemData(item.id, item.change_num);
                list.Add(data);
            }
        }
        if (vDrop.mail_list != null && vDrop.mail_list.Count > 0)
        {
            for (int i = 0; i < vDrop.mail_list.Count; i++)
            {
                fogs.proto.msg.Attachment item = vDrop.mail_list[i];
                CommonItemData data = new CommonItemData(item.id, (int)item.num);
                list.Add(data);
            }
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
        }
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
        UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list);
    }

    public override void Uninitialize()
    {
    }

}