using UnityEngine;
using fogs.proto.msg;
using System.Collections;
using System.Collections.Generic;

public class ExoticAdvantureInfoViewController : UIBase
{
    public ExoticAdvantureInfoView view;
    private StageInfo stageInfo;
    private UnionPveDgnInfo unionDgnInfo;
    private MonsterAttributeInfo bossAttInfo;
    private List<GateEnemyInfoComponent> enemy_dic;
    private List<GateAwardsComponent> awards_dic;
    private bool isChallenge = false;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new ExoticAdvantureInfoView();
            view.Initialize();
            BtnEventBinding();
        }
        view.Gobj_EnemyInfoComp.SetActive(false);
        view.Gobj_AwardsInfoComp.SetActive(false);
        if (enemy_dic == null)
            enemy_dic = new List<GateEnemyInfoComponent>();
        if (awards_dic == null)
            awards_dic = new List<GateAwardsComponent>();
        PlayerData.Instance.NotifyResetEvent += UpdateNotifyReset;
    }
    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW);
    }

    #region  Update Event

    private void UpdateNotifyReset(NotifyReset data)
    {
        UpdateSurplusInfo();
    }

    public void UpdateViewInfo(StageInfo info, UnionPveDgnInfo dgnInfo, MonsterAttributeInfo att, bool state, uint challengerID, bool isLock)
    {
        stageInfo = info;
        unionDgnInfo = dgnInfo;
        bossAttInfo = att;
        isChallenge = state;
        if (stageInfo == null) return;
        view.Btn_ReadyBattle.gameObject.SetActive(!isLock);
        UpdatePanelInfo();
        UpdateChallenegeInfo(challengerID);
    }

    public void UpdateViewInfo(StageInfo info, UnionPveDgnInfo dgnInfo, MonsterAttributeInfo att, bool isLock)
    {
        UpdateViewInfo(info, dgnInfo, att, false, 0, isLock);
    }

    private void UpdatePanelInfo()
    {
        view.Lbl_GateCountLabel.text = string.Format(ConstString.EXOTICADVANTURE_INFO_GATEINDEX, stageInfo.SmallGateID);
        view.Lbl_TitleLabel.text = stageInfo.Name;
        view.Lbl_StageDesc.text = stageInfo.Describe;
        UpdateBossInfo();
        UpdateSP();
        UpdateSurplusInfo();
        UpdateEnemyCast();
        Main.Instance.StartCoroutine(UpdateAwards());
    }

    public void UpdateSurplusInfo()
    {
        //view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT,
        //    ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes
        //    - UnionModule.Instance.CharUnionInfo.today_pve_times);
        int times = UnionModule.Instance.TotalPveTimes - UnionModule.Instance.CharUnionInfo.today_pve_times;
        view.Lbl_SurplusCount.text = string.Format(ConstString.GATE_TODAYSWEEPCOUNT, times);
        if (ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.ExoticAdventure) < 0)
        {
            view.Btn_Purchase.gameObject.SetActive(false);            
        }
        else
        {
            if (times > 0)
            {
                view.Btn_Purchase.gameObject.SetActive(false);
            }
            else
            {
                view.Btn_Purchase.gameObject.SetActive(true);
            }
        }      
    }

    private void UpdateBossInfo()
    {
        view.Lbl_BossDesc.text = stageInfo.BossDesc;
        int surplusHP = bossAttInfo.HP - unionDgnInfo.total_hurt;
        if (surplusHP <= 0)
            surplusHP = 0;
        if (bossAttInfo.HP <= 0)
            return;
        float progress = (float)surplusHP / (float)bossAttInfo.HP;
        if (progress >= 1)
        {
            view.Slider_BossBloodSlider.value = 1;
            //view.Lbl_BossBlood.text = string.Format(ConstString.PLAYEREXP, 100);
        }
        else if (progress <= 0)
        {
            view.Slider_BossBloodSlider.value = 0;
            //view.Lbl_BossBlood.text = string.Format(ConstString.PLAYEREXP, 0);
        }
        else
        {
            if (progress < 0.001f)
            {
                progress = 0.001f;
                view.Slider_BossBloodSlider.value = progress;
            }
            else if ((0.999f <= progress) && (progress < 1f))
            {
                progress = 0.999f;
                view.Slider_BossBloodSlider.value = progress;
            }
            else
            {
                progress = Mathf.Ceil(progress * 1000) / 10f;
                view.Slider_BossBloodSlider.value = progress / 100;
            }
            //view.Lbl_BossBlood.text = string.Format(ConstString.PLAYEREXP, (progress).ToString());
        }
        view.Lbl_BossBlood.text = surplusHP.ToString() + "/" + bossAttInfo.HP;
    }

    private void UpdateChallenegeInfo(uint challengerID)
    {
        view.Gobj_ChallengeGroup.SetActive(isChallenge);
        if (isChallenge)
        {
            UnionMember info = UnionModule.Instance.GetUnionMember(challengerID);
            view.Lbl_ChallengeState.text = ConstString.EXOTICADVANTURE_INFO_CHALLENGING;
            if (info != null)
            {
                view.Lbl_PlayerName.text = info.charname;
                CommonFunction.SetHeadAndFrameSprite(view.Spt_PlayerIcon, view.Spt_PlayerQuality, info.icon, info.icon_frame, true);
            }
        }
    }

    private void UpdateSP()
    {
        if (stageInfo.Physical <= 0)
        {
            view.Gobj_SPGroup.SetActive(false);
        }
        else
        {
            view.Gobj_SPGroup.SetActive(true);
            view.Lbl_SPLabel.text = stageInfo.Physical.ToString();
        }
    }


    private void UpdateEnemyCast()
    {
        if (stageInfo.EnemySquad.Count <= enemy_dic.Count)
        {
            for (int index = 0; index < enemy_dic.Count; index++)
            {
                GateEnemyInfoComponent comp = enemy_dic[index];
                if (index < stageInfo.EnemySquad.Count)
                {
                    SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                    if (_singleinfo == null) continue;
                    MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                    if (_monster == null) continue;
                    if (_singleinfo.IsBoss == 1)
                    {
                        comp.UpdateInfo(_monster, true);
                    }
                    else
                    {
                        comp.UpdateInfo(_monster, false);
                    }
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.Clear();
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int enemyObj_count = enemy_dic.Count;
            for (int index = 0; index < stageInfo.EnemySquad.Count; index++)
            {
                SingleEnemyInfo _singleinfo = stageInfo.EnemySquad[index];
                if (_singleinfo == null) continue;
                MonsterAttributeInfo _monster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(_singleinfo.MonsterID);
                if (_monster == null) continue;
                GateEnemyInfoComponent comp = null;
                if (index < enemyObj_count)
                {
                    comp = enemy_dic[index];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EnemyInfoComp, view.Grd_EnemyGrid.transform);
                    comp = new GateEnemyInfoComponent(go);
                    go.name = _monster.ID.ToString();
                    enemy_dic.Add(comp);
                }
                if (_singleinfo.IsBoss == 1)
                {
                    comp.UpdateInfo(_monster, true);
                }
                else
                {
                    comp.UpdateInfo(_monster, false);
                }
                comp.AddPressLisetener(ButtonEvent_EnemyComp);
                comp.mRootObject.SetActive(true);
            }
        }
        view.Grd_EnemyGrid.repositionNow = true;
    }

    /// <summary>
    /// 更新掉落包数据
    /// </summary>
    private IEnumerator UpdateAwards()
    {
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(stageInfo.DropID);
        UnionBaseData mUnionBaseData = ConfigManager.Instance.mUnionConfig.GetUnionBaseData();
        if (mUnionBaseData != null)
        {
            List<CommonItemData> loseList = CommonFunction.GetCommonItemDataList(mUnionBaseData.mPvpLoseReward);
            for (int i = 0; i < loseList.Count; i++)
            {
                CommonItemData loseItemData = loseList[i];
                CommonItemData itemData = list.Find((tmp) =>
                {
                    if (tmp == null) return false;
                    return tmp.ID == loseItemData.ID;
                });
                if (itemData == null)
                {
                    list.Add(loseItemData);
                }
            }
        }
        if (list != null)
        {
            if (list.Count <= awards_dic.Count)
            {
                for (int i = 0; i < awards_dic.Count; i++)
                {
                    GateAwardsComponent comp = awards_dic[i];
                    if (comp == null) continue;
                    if (i < list.Count)
                    {
                        CommonItemData award = list[i];
                        bool isSpecialID = false;
                        IDType idType = CommonFunction.GetTypeOfID(award.ID.ToString());
                        switch (idType)
                        {
                            case IDType.SP:
                            case IDType.Gold:
                            case IDType.Diamond:
                            case IDType.Medal:
                            case IDType.Exp:
                            case IDType.SoldierExp:
                            case IDType.Honor:
                                isSpecialID = true;
                                break;
                        } if (isSpecialID)
                        {
                            comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), 1);
                        }
                        else
                        {
                            comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                        }
                        if (!comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.Clear();
                        if (comp.mRootObject.activeSelf)
                            comp.mRootObject.SetActive(false);
                    }
                }
            }
            else
            {
                int count = GlobalCoefficient.GateAwardsCountLimit;
                if (list.Count <= count)
                {
                    count = list.Count;
                }
                for (int i = 0; i < count; i++)
                {
                    CommonItemData award = list[i];
                    GateAwardsComponent comp = null;
                    if (i < awards_dic.Count)
                    {
                        comp = awards_dic[i];
                    }
                    else
                    {
                        GameObject go = CommonFunction.InstantiateObject(view.Gobj_AwardsInfoComp, view.Grd_AwardsGrid.transform);
                        comp = new GateAwardsComponent(go);
                        awards_dic.Add(comp);
                    }
                    if (comp == null) continue;
                    bool isSpecialID = false;
                    IDType idType = CommonFunction.GetTypeOfID(award.ID.ToString());
                    switch (idType)
                    {
                        case IDType.SP:
                        case IDType.Gold:
                        case IDType.Diamond:
                        case IDType.Medal:
                        case IDType.Exp:
                        case IDType.SoldierExp:
                        case IDType.Honor:
                            isSpecialID = true;
                            break;
                    }
                    if (isSpecialID)
                    {
                        comp.UpdateCompInfo(award.ID, award.SubType, CommonFunction.GetIconNameByID(award.ID), (int)ItemQualityEnum.White);
                    }
                    else
                    {
                        comp.UpdateCompInfo(award.ID, award.SubType, award.Icon, (int)award.Quality);
                    }
                    comp.AddPressLisetener(ButtonEvent_AwardComp);
                    comp.mRootObject.SetActive(true);
                }
            }
        }
        yield return null;
        view.Grd_AwardsGrid.repositionNow = true;
    }
    #endregion


    #region Button Event
    private void ButtonEvent_Purchase(GameObject btn)
    {
        VipData vipData = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        int times = UnionModule.Instance.CharUnionInfo.today_buy_times;
        if (vipData != null)
        {
            if (times < vipData.ExoticAdvantureLimit)
            {
                TimesExpendData timesExpendData = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)times + 1);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.PVP_PURCHASECHANGLLECOUNT, timesExpendData.ExoticAdvantureConsume.Number, ConfigManager.Instance.mTimesBuyConfig.GetTimesBuyData().ExoticAdvantureTimes, times, vipData.ExoticAdvantureLimit), () =>
                {
                    FightRelatedModule.Instance.SendBuyOtherDungeonTimes(OtherDungeonType.EXOTICADVANTURE, 0);
                });
            }
            else
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                if (PlayerData.Instance._VipLv < ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.ExoticAdventure))
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.NO_BUY_SP_TIMES_TO_VIP, () =>
                    {
                        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                        UISystem.Instance.VipRechargeView.ShowRecharge();
                    }, null, ConstString.FORMAT_RECHARGE);
                }
                else
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PVP_PURCHASECOUNTLIMIT);
                }
            }
        }
    }
    private void ButtonEvent_EnemyComp(GateEnemyInfoComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.MonsterInfo);
    }

    private void ButtonEvent_AwardComp(GateAwardsComponent baseComp, bool isPress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (baseComp == null) return;
        HintManager.Instance.SeeDetail(baseComp.mRootObject, isPress, baseComp.ItemID);
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW);
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_EXOTICADVANTUREVIEW))
        {
            UISystem.Instance.ExoticAdvantureView.UpdateViewInfo();
        }
    }

    private void ButtonEvent_ReadyBattle(GameObject btn)
    {
        if (isChallenge)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXOTICADVANTURE_INFO_CHALLENGINGTIP);
        }
        else
        {
            //if (UnionModule.Instance.CharUnionInfo.today_pve_times < ConfigManager.Instance.mUnionConfig.GetUnionBaseData().mMaxUnionPVETimes)
            if (UnionModule.Instance.CharUnionInfo.today_pve_times < UnionModule.Instance.TotalPveTimes)            
            {
                UnionModule.Instance.OnSendReadyUnionPveDgn(stageInfo.ID);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.UNION_HINT_TODAYCHALLENGEFINISH);
            }
        }
    }
    #endregion

    public void OnReadyUnionPveDgnResp(ReadyUnionPveDgnResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            isChallenge = false;
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PREPAREBATTLEVIEW);
            UISystem.Instance.PrepareBattleView.UpdateViewInfo(EFightType.eftUnion, unionDgnInfo, stageInfo);
        }
        else if (data.result == (int)ErrorCodeEnum.UnionDgnIsChallenged)
        {
            isChallenge = true;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXOTICADVANTURE_INFO_CHALLENGINGTIP);
            UpdateChallenegeInfo((uint)data.challenger);
        }
    }

    public override void Uninitialize()
    {
        PlayerData.Instance.NotifyResetEvent -= UpdateNotifyReset;
        isChallenge = false;
    }


    public override void Destroy()
    {
        base.Destroy();
        view = null;
        awards_dic.Clear();
        enemy_dic.Clear();
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_ReadyBattle.gameObject).onClick = ButtonEvent_ReadyBattle;
        UIEventListener.Get(view.Btn_Purchase.gameObject).onClick = ButtonEvent_Purchase;
    }
}
