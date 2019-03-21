using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
public class ExoticAdvantureViewController : UIBase
{
    public ExoticAdvantureView view;
    private List<ExoticAdvantureGateComponent> gateBtn_dic;
    /// <summary>
    /// 当前章节关卡数据
    /// </summary>
    private List<StageData> currentChapterList;
    private class ExoticAdvantureData
    {
        public UnionPveDgnInfo dgnPOD;
        public StageInfo stageInfo;
        /// <summary>
        /// 1 已通过 2 解锁未通过 3 未解锁
        /// </summary>
        public int lockStatus;
    }

    private List<ExoticAdvantureData> exoticAddventureDataList;

    private ExoticAdvantureGateComponent currentExoticAdventurComp;

    private List<ExoticAdvantureData> currentChapterExoticList;

    private ChapterInfo chapterInfo;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new ExoticAdvantureView();
            view.Initialize();
            BtnEventBinding();
        }
        view.Gobj_Gate.SetActive(false);
        view.Btn_Reset.gameObject.SetActive(false);
        view.Btn_AdvantureRole.transform.localPosition = new Vector3(-230, 0, 0);
        if (gateBtn_dic == null)
            gateBtn_dic = new List<ExoticAdvantureGateComponent>();
        PlayerData.Instance.NotifyResetEvent += UpdateResetData;
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ExoticAdvantureView.UIName);
    }

    private List<ExoticAdvantureData> GetExoticAdvantureList()
    {
        List<ExoticAdvantureData> exoticAdvantureLsit = new List<ExoticAdvantureData>();
        if (UnionModule.Instance.UnionInfo.dgn_info.Count == 0)
        {
            ChapterInfo mChapter = ConfigManager.Instance.mChaptersData.GetFirstUnionChapterInfo();
            List<StageInfo> mStageList = ConfigManager.Instance.mStageData.GetStageListByIDs(mChapter.gates);
            mStageList = CommonFunction.SortStages(mStageList);
            if (mStageList != null && mStageList.Count > 0)  //不排除数据处理错误的可能
            {
                StageInfo mStageInfo = mStageList[0];
                ExoticAdvantureData mExoticAdvantureData = new ExoticAdvantureData();
                mExoticAdvantureData.stageInfo = mStageInfo;
                mExoticAdvantureData.dgnPOD = new UnionPveDgnInfo();
                mExoticAdvantureData.dgnPOD.dgn_id = mStageInfo.ID;
                mExoticAdvantureData.dgnPOD.finished = 0;
                mExoticAdvantureData.dgnPOD.total_hurt = 0;
                mExoticAdvantureData.lockStatus = 2;
                exoticAdvantureLsit.Add(mExoticAdvantureData);
            }
        }
        else
        {
            List<uint> passedGateIDList = new List<uint>();
            for (int i = 0; i < UnionModule.Instance.UnionInfo.dgn_info.Count; i++)
            {
                UnionPveDgnInfo mUnionPveDgnInfo = UnionModule.Instance.UnionInfo.dgn_info[i];
                ExoticAdvantureData mExoticAdvantureData = new ExoticAdvantureData();
                mExoticAdvantureData.dgnPOD = mUnionPveDgnInfo;
                StageInfo mStageInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)mUnionPveDgnInfo.dgn_id);
                mExoticAdvantureData.stageInfo = mStageInfo;
                if (mUnionPveDgnInfo.finished == 1)
                {
                    mExoticAdvantureData.lockStatus = 1;
                }
                else
                {
                    mExoticAdvantureData.lockStatus = 2;
                }
                exoticAdvantureLsit.Add(mExoticAdvantureData);
                passedGateIDList.Add(mUnionPveDgnInfo.dgn_id);
            }
            for (int i = 0; i < exoticAdvantureLsit.Count; i++)
            {
                ExoticAdvantureData mExoticAdvantureData = exoticAdvantureLsit[i];
                if (mExoticAdvantureData == null || mExoticAdvantureData.dgnPOD == null || mExoticAdvantureData.stageInfo == null)
                    continue;
                if (mExoticAdvantureData.dgnPOD.finished == 1)  //认为1时  表通过
                {
                    List<StageInfo> nextStages = ConfigManager.Instance.mStageData.GetStageListByIDs(mExoticAdvantureData.stageInfo.NextStage);
                    if (nextStages == null || nextStages.Count <= 0)
                        continue;
                    for (int index = 0; index < nextStages.Count; index++)  //取得后置关卡数据
                    {
                        StageInfo next_info = nextStages[index];
                        if (next_info == null)
                            continue;
                        if (passedGateIDList.Contains(next_info.ID))  //已经包含的数据 不在处理
                            continue;
                        bool isEnable = true;
                        for (int pre_index = 0; pre_index < next_info.PreStage.Count; pre_index++)  //判断后置关卡的前置条件时候都已经满足
                        {
                            PreStageInfo pre_info = next_info.PreStage[pre_index];
                            ExoticAdvantureData passGateinfo = exoticAdvantureLsit.Find((tmp) =>    //取已经通过的数据  如果没有则说明该关卡没有打过
                            {
                                if (tmp == null) return false;
                                return pre_info.StageID == tmp.dgnPOD.dgn_id;
                            });
                            if (passGateinfo == null) //如果关卡未通过或者低于关卡所需评星 则不添加新数据
                            {
                                isEnable = false;
                            }
                        }
                        if (isEnable)
                        {
                            ExoticAdvantureData next_data = new ExoticAdvantureData();
                            next_data.dgnPOD = new UnionPveDgnInfo();
                            next_data.dgnPOD.finished = 0;
                            next_data.dgnPOD.dgn_id = next_info.ID;
                            next_data.dgnPOD.total_hurt = 0;
                            next_data.stageInfo = new StageInfo();
                            next_data.stageInfo.CopyTo(next_info);
                            next_data.lockStatus = 2;
                            exoticAdvantureLsit.Add(next_data);
                        }
                    }
                }
            }
        }
        return exoticAdvantureLsit;
    }

    #region Update Event
    private void UpdateResetData(NotifyReset data)
    {
        UpdateAwardsStatus();
        if (data.reset_union_pve == 1)
        {
            UnionModule.Instance.UnionInfo.dgn_info.Clear();
            UISystem.Instance.UnionView.InitUI();
            UpdateViewInfo();
        }
    }

    public void UpdateViewInfo()
    {
        exoticAddventureDataList = GetExoticAdvantureList();
        List<ChapterInfo> list = ConfigManager.Instance.mChaptersData.GetExoticAdvantureChapters();
        if (chapterInfo != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ChapterInfo tmpChapter = list[i];
                ExoticAdvantureData mExoticAdvantureData = exoticAddventureDataList.Find((data) =>
                {
                    if (data == null || data.stageInfo == null) return false;
                    return data.stageInfo.ChapterID == tmpChapter.id;
                });
                if (mExoticAdvantureData != null)
                {
                    if (chapterInfo.id < tmpChapter.id)
                        chapterInfo = tmpChapter;
                }
            }
        }
        if (chapterInfo == null)
            chapterInfo = ConfigManager.Instance.mChaptersData.GetFirstUnionChapterInfo();
        UpdatePanelInfo();

    }


    private void UpdateChapterData()
    {
        if (currentChapterExoticList == null)
            currentChapterExoticList = new List<ExoticAdvantureData>();
        currentChapterExoticList.Clear();
        List<StageInfo> mStageList = ConfigManager.Instance.mStageData.GetStageListByIDs(chapterInfo.gates);
        if (mStageList == null) mStageList = new List<StageInfo>();
        for (int i = 0; i < mStageList.Count; i++)
        {
            StageInfo mStageInfo = mStageList[i];
            ExoticAdvantureData mPassedData = exoticAddventureDataList.Find((tmp) =>
            {
                if (tmp == null || tmp.dgnPOD == null) return false;
                return tmp.dgnPOD.dgn_id == mStageInfo.ID;
            });
            if (mPassedData == null)
            {
                mPassedData = new ExoticAdvantureData();
                mPassedData.stageInfo = mStageInfo;
                mPassedData.dgnPOD = new UnionPveDgnInfo();
                mPassedData.dgnPOD.dgn_id = mStageInfo.ID;
                mPassedData.dgnPOD.finished = 0;
                mPassedData.dgnPOD.total_hurt = 0;
                mPassedData.lockStatus = 3;
            }
            currentChapterExoticList.Add(mPassedData);
        }
    }

    private void UpdatePanelInfo()
    {
        UpdateChatperBG();
        UpdateArrowStatus();
        UpdateChapterName();
        UpdateAwardsStatus();
        UpdateChapterData();
        Main.Instance.StartCoroutine(UpdateStageButton());
    }

    private IEnumerator UpdateStageButton()
    {
        if (chapterInfo != null)
        {
            if (currentChapterExoticList.Count <= gateBtn_dic.Count)
            {
                for (int i = currentChapterExoticList.Count; i < gateBtn_dic.Count; i++)
                {
                    gateBtn_dic[i].mRootObject.SetActive(false);
                }
            }
            yield return null;
            for (int i = 0; i < currentChapterExoticList.Count; i++)
            {
                ExoticAdvantureData mExoticAdvantureData = currentChapterExoticList[i];
                ExoticAdvantureGateComponent comp = null;
                if (i < gateBtn_dic.Count)
                {
                    comp = gateBtn_dic[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_Gate.gameObject, view.Gobj_GateGroup.transform);
                    comp = new ExoticAdvantureGateComponent();
                    comp.MyStart(go);
                    go.name = mExoticAdvantureData.stageInfo.ID.ToString();
                    comp.AddEventListener(ButtonEvent_StageButton);
                    gateBtn_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.transform.localPosition = new Vector3(mExoticAdvantureData.stageInfo.IconPos.x, mExoticAdvantureData.stageInfo.IconPos.y, 0);
                comp.UpdateCompInfo(mExoticAdvantureData.dgnPOD, mExoticAdvantureData.stageInfo, mExoticAdvantureData.lockStatus);
                comp.mRootObject.SetActive(true);
            }
        }
    }

    public void UpdateAwardsStatus()
    {
        if (UnionModule.Instance.CharUnionInfo.get_today_rewards == 0)  //0未领取 1已领取
        {
            CommonFunction.SetSpriteName(view.Spt_Chest, GlobalConst.SpriteName.LivenessBoxIcons[5]);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_Chest, GlobalConst.SpriteName.LivenessBoxIconsOpen[5]);
        }
        int count = Mathf.Min(UnionModule.Instance.CharUnionInfo.today_pve_times, (int)ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes);
        view.Lbl_AwardsNum.text = string.Format(ConstString.EXOTICADVANTURE_AWARD_TODAYCHALLENGE,
            count, ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes);
    }

    private void UpdateArrowStatus()
    {
        if (chapterInfo != null)
        {
            if (chapterInfo.previd == 0)   //无上一章节
            {
                view.Btn_LeftArrow.gameObject.SetActive(false);
            }
            else
            {
                view.Btn_LeftArrow.gameObject.SetActive(true);
            }
            if (chapterInfo.nextid == 0)   //无下一章节
            {
                view.Btn_RightArrow.gameObject.SetActive(false);
            }
            else
            {

                view.Btn_RightArrow.gameObject.SetActive(true);
            }
        }
    }
    private void UpdateChatperBG()
    {
        if (chapterInfo == null)
        {
            view.Tex_ChapterBg.mainTexture = null;
            return;
        }
        ResourceLoadManager.Instance.LoadAloneImage(chapterInfo.resicon, (texture) =>
        {
            if (view.Tex_ChapterBg != null)
            {
                view.Tex_ChapterBg.mainTexture = texture;
                //view.Tex_ChapterBg.MakePixelPerfect();
            }
        });
    }

    private void UpdateChapterName()
    {
        if (chapterInfo == null)
            view.Lbl_ChapterName.text = string.Empty;
        else
            view.Lbl_ChapterName.text = chapterInfo.name;
    }

    #endregion

    #region Button Event

    private void ButtonEvent_StageButton(BaseComponent baseComp)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        ExoticAdvantureGateComponent comp = baseComp as ExoticAdvantureGateComponent;
        if (comp == null) return;
        switch (comp.LockStatus)
        {
            case 3:
            case 1:
                return;
            case 2:
                {
                    currentExoticAdventurComp = comp;
                    UnionModule.Instance.OnSendQueryUnionPveDgnState(comp.StageInfo.ID);
                } break;
            //case 3:
            //    {
            //        currentExoticAdventurComp = comp;
            //        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW);
            //        UISystem.Instance.ExoticAdvantureInfoView.UpdateViewInfo(currentExoticAdventurComp.StageInfo, currentExoticAdventurComp.DgnData, currentExoticAdventurComp.Monster, true);
            //    }
            //    break;
        }

    }

    public void ButtonEvent_Exit(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EXOTICADVANTUREVIEW);
    }

    public void ButtonEvent_LeftArrow(GameObject btn)
    {

    }

    public void ButtonEvent_RightArrow(GameObject btn)
    {

    }

    public void ButtonEvent_Reset(GameObject btn)
    {
        if (UnionModule.Instance.UnionInfo.dgn_info == null || UnionModule.Instance.UnionInfo.dgn_info.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXOTICADVANTURE_INFOTIP_RESET);
            return;
        }
        UnionPosition unionPosition = UnionModule.Instance.MyUnionMemberInfo.position;
        if (unionPosition == UnionPosition.UP_MEMBER)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_HINT_RESETEXOTIC);
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.UNION_HINT_RESETCONSUME, ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mResetDungeonCost), () =>
            {
                UnionModule.Instance.OnSendResetUnionPveDgn();
            });
        }
    }

    public void ButtonEvent_AdvantureRule(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RULEVIEW);
        UISystem.Instance.RuleView.UpdateViewInfo(1);
    }

    public void ButtonEvent_InjureRank(GameObject btn)
    {
        UnionModule.Instance.OnSendUnionPveDgnRank();
    }

    public void ButtonEvent_HighAwards(GameObject btn)
    {
        UnionBaseData unionBaseData = ConfigManager.Instance.mUnionConfig.GetUnionBaseData();
        if (unionBaseData == null) return;
        if (UnionModule.Instance.CharUnionInfo.today_pve_times < unionBaseData.mMaxUnionPVETimes)
        {
            List<CommonItemData> awardList = CommonFunction.GetCommonItemDataList(unionBaseData.mEveryAdvantureReward);
            string strTitle = ConstString.RECEIVE_CANGET;
            string strTip = string.Format(ConstString.UNION_HINT_ADVANTURE, unionBaseData.mMaxUnionPVETimes);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(awardList, strTitle, strTip);
        }
        else
        {
            if (UnionModule.Instance.CharUnionInfo.get_today_rewards == 0)
            {
                UnionModule.Instance.OnSendUnionPveDgnTodayRward();
            }
        }
    }
    #endregion

    public void OnQueryUnionPveDgnState(bool state, uint gateID, uint challengerID)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW);
        UISystem.Instance.ExoticAdvantureInfoView.UpdateViewInfo(currentExoticAdventurComp.StageInfo, currentExoticAdventurComp.DgnData, currentExoticAdventurComp.Monster, state, challengerID, false);
    }

    public void OnResetUnionPveDgnResp()
    {
        UpdateViewInfo();
    }

    public void OnUnionPveDgnTodayRewardResp(DropList dropItem, bool status = false)
    {
        UpdateAwardsStatus();
        if (status)
        {
            UnionBaseData unionBaseData = ConfigManager.Instance.mUnionConfig.GetUnionBaseData();
            if (unionBaseData == null) return;
            List<CommonItemData> awardList = CommonFunction.GetCommonItemDataList(unionBaseData.mEveryAdvantureReward);
            string strTitle = ConstString.GATE_HADGETAWARDS;
            string strTip = string.Format(ConstString.UNION_HINT_ADVANTURE, unionBaseData.mMaxUnionPVETimes);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(awardList, strTitle, strTip);
        }
        else
        {
            List<CommonItemData> list = new List<CommonItemData>();
            if (dropItem != null)
            {
                if (dropItem.item_list != null)
                {
                    for (int i = 0; i < dropItem.item_list.Count; i++)
                    {
                        fogs.proto.msg.ItemInfo item = dropItem.item_list[i];
                        CommonItemData data = new CommonItemData(item.id, item.change_num);
                        list.Add(data);
                    }
                }
                if (dropItem.soldier_list != null)
                {
                    for (int i = 0; i < dropItem.soldier_list.Count; i++)
                    {
                        fogs.proto.msg.Soldier soldier = dropItem.soldier_list[i];
                        CommonItemData data = new CommonItemData(soldier.id, 1);
                        list.Add(data);
                    }
                }
                if (dropItem.equip_list != null)
                {
                    for (int i = 0; i < dropItem.equip_list.Count; i++)
                    {
                        fogs.proto.msg.Equip equip = dropItem.equip_list[i];
                        CommonItemData data = new CommonItemData(equip.id, 1);
                        list.Add(data);
                    }
                }
                if (dropItem.special_list != null)
                {
                    for (int i = 0; i < dropItem.special_list.Count; i++)
                    {
                        fogs.proto.msg.ItemInfo item = dropItem.special_list[i];
                        CommonItemData data = new CommonItemData(item.id, item.change_num);
                        list.Add(data);
                    }
                }
                if (dropItem.mail_list != null && dropItem.mail_list.Count > 0)
                {
                    for (int i = 0; i < dropItem.mail_list.Count; i++)
                    {
                        fogs.proto.msg.Attachment item = dropItem.mail_list[i];
                        CommonItemData data = new CommonItemData(item.id, (int)item.num);
                        list.Add(data);
                    }
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
                }
            }
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            string strTitle = ConstString.RECEIVE_SUCCESS;
            string strTip = string.Empty;
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, strTitle, strTip);
        }
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Exit.gameObject).onClick = ButtonEvent_Exit;
        UIEventListener.Get(view.Btn_LeftArrow.gameObject).onClick = ButtonEvent_LeftArrow;
        UIEventListener.Get(view.Btn_RightArrow.gameObject).onClick = ButtonEvent_RightArrow;
        UIEventListener.Get(view.Btn_Reset.gameObject).onClick = ButtonEvent_Reset;
        UIEventListener.Get(view.Btn_AdvantureRole.gameObject).onClick = ButtonEvent_AdvantureRule;
        UIEventListener.Get(view.Btn_InjureRank.gameObject).onClick = ButtonEvent_InjureRank;
        UIEventListener.Get(view.Btn_HighAwards.gameObject).onClick = ButtonEvent_HighAwards;
    }

    public override void Uninitialize()
    {
        currentChapterExoticList.Clear();
        PlayerData.Instance.NotifyResetEvent -= UpdateResetData;
        if (view.Tex_ChapterBg != null)
            view.Tex_ChapterBg.mainTexture = null;
        currentExoticAdventurComp = null;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        gateBtn_dic.Clear();
    }


}
