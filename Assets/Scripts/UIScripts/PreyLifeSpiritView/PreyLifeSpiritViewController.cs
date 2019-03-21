using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using fogs.proto.msg;
using Assets.Script.Common;
public class PreyLifeSpiritViewController : UIBase
{
    public PreyLifeSpiritView view;

    private List<PreyLifeSpiritComponent> preyLifeSpirit_dic;

    private List<PreyLifeSoulData> preyLifeSoulDataList;

    private List<PreyLockBtnComponent> lockComp_dic;
    private bool isPlayAnim;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new PreyLifeSpiritView();
            view.Initialize();
            BtnEventBinding();
            Init();
        }
        view.UIWrapContent_LifeSpiritGrid.onInitializeItem = UpdateWrapLifeSoul;
        UpdateViewInfo();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenHuntSoulView);
    }

    private void Init()
    {
        if (preyLifeSpirit_dic == null)
            preyLifeSpirit_dic = new List<PreyLifeSpiritComponent>();
        if (lockComp_dic == null)
            lockComp_dic = new List<PreyLockBtnComponent>();
        if (lockComp_dic == null)
            lockComp_dic = new List<PreyLockBtnComponent>();
        lockComp_dic.Clear();
        PreyLockBtnComponent comp_white = new PreyLockBtnComponent();
        comp_white.MyStart(view.Btn_White.gameObject);
        comp_white.UpdateCompType(PreyLockTypeEnum.White);
        comp_white.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_white);

        PreyLockBtnComponent comp_green = new PreyLockBtnComponent();
        comp_green.MyStart(view.Btn_Green.gameObject);
        comp_green.UpdateCompType(PreyLockTypeEnum.Green);
        comp_green.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_green);

        PreyLockBtnComponent comp_blue = new PreyLockBtnComponent();
        comp_blue.MyStart(view.Btn_Blue.gameObject);
        comp_blue.UpdateCompType(PreyLockTypeEnum.Blue);
        comp_blue.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_blue);

        PreyLockBtnComponent comp_purple = new PreyLockBtnComponent();
        comp_purple.MyStart(view.Btn_Purple.gameObject);
        comp_purple.UpdateCompType(PreyLockTypeEnum.Purple);
        comp_purple.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_purple);

        PreyLockBtnComponent comp_orange = new PreyLockBtnComponent();
        comp_orange.MyStart(view.Btn_Orange.gameObject);
        comp_orange.UpdateCompType(PreyLockTypeEnum.Orange);
        comp_orange.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_orange);


        PreyLockBtnComponent comp_red = new PreyLockBtnComponent();
        comp_red.MyStart(view.Btn_Red.gameObject);
        comp_red.UpdateCompType(PreyLockTypeEnum.Red);
        comp_red.AddEventListener(ButtonEvent_LockBtn);
        lockComp_dic.Add(comp_red);

    }

    #region Update Event

    private void UpdateWrapLifeSoul(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_LifeSpiritGrid.enabled) return;
        if (realIndex >= preyLifeSoulDataList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PreyLifeSpiritComponent comp = preyLifeSpirit_dic[wrapIndex];
        if (comp == null) return;
        PreyLifeSoulData soulData = preyLifeSoulDataList[realIndex];
        comp.UpdateCompInfo(soulData);
    }

    public void UpdateViewInfo()
    {
        isPlayAnim = false;
        UpdateLifeSoulData();
        Main.Instance.StartCoroutine(UpdatePreyLifeSpirit());
        UpdateLockBtnStatus();
        UpdatePreyButton();
    }

    private void UpdateLifeSoulData()
    {
        if (preyLifeSoulDataList == null)
            preyLifeSoulDataList = new List<PreyLifeSoulData>();
        preyLifeSoulDataList.Clear();
        int count = 12;
        if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.LIFESOUL_PREY_LIMIT), out count))
        {
            count = 12;
        }
        for (int i = 0; i < count; i++)
        {
            PreyLifeSoulData data = null;
            if (i < PlayerData.Instance._PreyLifeSoulInfo.uids.Count)
            {
                fogs.proto.msg.UidToId uidInfo = PlayerData.Instance._PreyLifeSoulInfo.uids[i];
                if (uidInfo == null)
                    continue;
                LifeSoulConfigInfo info = ConfigManager.Instance.mLifeSoulConfig.GetLifeDataByID((int)uidInfo.id);
                if (info == null)
                    continue;
                data = new PreyLifeSoulData();
                data.uid = uidInfo.uid;
                data.info = info;
            }
            preyLifeSoulDataList.Add(data);
        }
    }

    private IEnumerator UpdatePreyLifeSpirit()
    {
        view.ScrView_LifeSpiritScrView.enabled = false;
        int MAXCOUNT = 24;
        int count = preyLifeSoulDataList.Count;
        int itemCount = preyLifeSpirit_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_LifeSpiritGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_LifeSpiritGrid.minIndex = -index;
        view.UIWrapContent_LifeSpiritGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_LifeSpiritGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_LifeSpiritGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                preyLifeSpirit_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            PreyLifeSoulData data = preyLifeSoulDataList[i];
            PreyLifeSpiritComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_LifeSpiritComp, view.Grd_LifeSpiritGrid.transform);
                comp = new PreyLifeSpiritComponent();
                vGo.name = i.ToString();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_PickupLifeSpirit);
                preyLifeSpirit_dic.Add(comp);
            }
            else
            {
                comp = preyLifeSpirit_dic[i];
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data);
        }
        yield return null;
        view.UIWrapContent_LifeSpiritGrid.ReGetChild();
        yield return null;
        view.Grd_LifeSpiritGrid.Reposition();
        yield return null;
        //  view.ScrView_LifeSpiritScrView.ResetPosition();
    }

    private void UpdateLockBtnStatus(bool playAnim = false)
    {
        List<LifeSoulBaseInfo> list = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetBaseInfoList();
        if (list == null)
            return;
        for (int i = 0; i < lockComp_dic.Count; i++)
        {
            PreyLockBtnComponent comp = lockComp_dic[i];
            if (i < list.Count)
            {
                comp.UpdateCompInfo(list[i]);
            }
            if (PlayerData.Instance._PreyLifeSoulInfo.lock_level == (int)comp.LockType)
            {
                comp.IsSelected = true;
                if (playAnim)
                    comp.PlayAnim();
            }
            else
            {
                comp.IsSelected = false;
            }
        }
    }

    private void UpdatePreyButton()
    {
        LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType(PlayerData.Instance._PreyLifeSoulInfo.lock_level);
        if (info == null)
            return;
        CommonFunction.SetMoneyIcon(view.Spt_PreyConsumeIcon, info.unspecify_price.Type);
        view.Lbl_PreyConsumeTip.text = info.unspecify_price.Number.ToString();
    }

    private void OpenRulePanel()
    {
        view.Gobj_RulePanel.SetActive(true);
        view.ScrollView_RuleDesc.ResetPosition();
        RuleDescData ruledesc = ConfigManager.Instance.mRuleDescConfig.GetRuleDescData();
        if (ruledesc == null)
        {
            view.Lbl_RuleDesc.text = string.Empty;
        }
        else
        {
            view.Lbl_RuleDesc.text = CommonFunction.ReplaceEscapeChar(ruledesc.preyLifeSoul_rule);
        }
    }

    #endregion

    private bool IsCanPreyLifeSpirit()
    {
        int count = 12;
        if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.LIFESOUL_PREY_LIMIT), out count))
        {
            count = 12;
        }
        if (PlayerData.Instance._PreyLifeSoulInfo.uids.Count >= count)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PREYLIFESPIRIT_LIFESPIRIT_COUNTLIMIT);
            return false;
        }
        return true;
    }

    #region PlayAnim

    private int soulIndex = 0;
    private List<LevelToSoulShow> LifeSoulShowList;
    private ErrorCodeEnum errorCode;
    private List<UidToId> uidList;

    public void OnPreyLifeSpiritSuccess(ExploreSoulResp data)
    {
        soulIndex = 0;
        if (data.result == (int)ErrorCodeEnum.SUCCESS || (data.result == (int)ErrorCodeEnum.SoulOneKeyExploreMax) || (data.result == (int)ErrorCodeEnum.SoulOneKeyExploreGold))
        {
            PlayerData.Instance.UpdateItem(data.update_items);
            LifeSoulShowList = data.soul_show_list;
            errorCode = (ErrorCodeEnum)data.result;
            PlayerData.Instance._PreyLifeSoulInfo.lock_level = data.last_level;
            PlayPreyAnim();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    private void StopPreyAnim()
    {
        for (int i = 0; i < preyLifeSpirit_dic.Count; i++)
        {
            if (i < soulIndex)
            {
                PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
                if (comp == null)
                    continue;
                comp.StopAnim();
            }

        }
    }

    private void PlayPreyAnim()
    {
        isPlayAnim = true;
        //for (int i = 0; i < preyLifeSpirit_dic.Count; i++)
        //{
        //    PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
        //    if (comp == null)
        //        continue;
        //    comp.StopAnim();
        //}
        if (LifeSoulShowList == null || LifeSoulShowList.Count <= soulIndex)
        {
            isPlayAnim = false;
            Scheduler.Instance.RemoveTimer(PlayPreyAnim);
            if (errorCode == ErrorCodeEnum.SUCCESS)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PREYLIFESPIRIT_LIFESPIRITSUCCESS);
            }
            else
            {
                ErrorCode.ShowErrorTip((int)errorCode);
            }
            return;
        }
        LevelToSoulShow data = LifeSoulShowList[soulIndex];
        AddPreyLifeSoul(data);
    }


    private void AddPreyLifeSoul(LevelToSoulShow data)
    {
        soulIndex++;
        PlayerData.Instance._PreyLifeSoulInfo.uids.AddRange(data.soul_id);
        UpdateLifeSoulData();
        int count = preyLifeSoulDataList.Count;
        int itemCount = preyLifeSpirit_dic.Count;
        for (int i = 0; i < count; i++)
        {
            PreyLifeSoulData souldata = preyLifeSoulDataList[i];
            if (souldata == null)
                continue;
            PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
            comp.UpdateCompInfo(souldata);
            UidToId uid = data.soul_id.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.uid == souldata.uid;
            });
            if (uid == null)
                continue;
            if (uid.uid == souldata.uid)
                comp.PlayPreyAnim();
        }
        PlayerData.Instance._PreyLifeSoulInfo.lock_level = data.level;
        UpdateLockBtnStatus(true);
        UpdatePreyButton();
        Scheduler.Instance.RemoveTimer(PlayPreyAnim);
        Scheduler.Instance.AddTimer(0.2f, false, PlayPreyAnim);
        //Scheduler.Instance.RemoveTimer(StopPreyAnim);
        //Scheduler.Instance.AddTimer(0.3f, false, StopPreyAnim);
        //Scheduler.Instance.AddUpdator();
    }


    private void PlayCollectAnim()
    {
        isPlayAnim = false;
        Scheduler.Instance.RemoveTimer(PlayCollectAnim);
        if (errorCode == ErrorCodeEnum.SUCCESS)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.COLLECTLIFESPIRIT_LIFESPIRITSUCCESS);
        }
        else
        {
            ErrorCode.ShowErrorTip((int)errorCode);
        }
        UpdateLifeSoulData();
        for (int i = 0; i < preyLifeSpirit_dic.Count; i++)
        {
            PreyLifeSoulData souldata = preyLifeSoulDataList[i];
            PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
            if (comp == null)
                continue;
            if (souldata == null)
                comp.Clear();
            else
            {
                comp.UpdateCompInfo(souldata);
            }
        }
    }


    //private void RemoveCollectLifeSoul(UidToId data)
    //{
    //    soulIndex++;
    //    UidToId tmpuid = PlayerData.Instance._PreyLifeSoulInfo.uids.Find((tmp) =>
    //    {
    //        if (tmp == null)
    //            return false;
    //        return tmp.uid == data.uid;
    //    });
    //    if (tmpuid != null)
    //        PlayerData.Instance._PreyLifeSoulInfo.uids.Remove(tmpuid);
    //    for (int i = 0; i < preyLifeSpirit_dic.Count; i++)
    //    {
    //        PreyLifeSoulData souldata = preyLifeSoulDataList[i];
    //        PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
    //        if (souldata == null)
    //        {
    //            comp.Clear();
    //            continue;
    //        }
    //        if (data.uid == souldata.uid)
    //            comp.PlayCollectAnim();
    //    }
    //    UpdateLifeSoulData();
    //    Scheduler.Instance.RemoveTimer(PlayCollectAnim);
    //    Scheduler.Instance.AddTimer(1f, false, PlayCollectAnim);
    //}

    #endregion
    public void OnPickupLifeSpiritSuccess(CollectSoulResp data)
    {
        GuideManager.Instance.CheckTrigger(GuideTrigger.PickSoulSuccess);
        soulIndex = 0;
        uidList = data.success_items;
        errorCode = (ErrorCodeEnum)data.result;
        isPlayAnim = true;
        for (int i = 0; i < data.success_items.Count; i++)
        {
            UidToId uidInfo = data.success_items[i];
            if (uidInfo == null)
                continue;
            UidToId tmpuid = PlayerData.Instance._PreyLifeSoulInfo.uids.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.uid == uidInfo.uid;
            });
            if (tmpuid != null)
                PlayerData.Instance._PreyLifeSoulInfo.uids.Remove(tmpuid);
        }
        for (int i = 0; i < preyLifeSpirit_dic.Count; i++)
        {
            PreyLifeSoulData souldata = preyLifeSoulDataList[i];
            PreyLifeSpiritComponent comp = preyLifeSpirit_dic[i];
            if (souldata == null)
            {
                comp.Clear();
                continue;
            }
            UidToId tmpuid = data.success_items.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.uid == souldata.uid;
            });
            if (tmpuid == null)  //有可能只能拾取一部分
                continue;
            if (tmpuid.uid == souldata.uid)
                comp.PlayCollectAnim();
        }
        Scheduler.Instance.AddTimer(0.3f, false, PlayCollectAnim);
    }

    #region Button Event

    private void ButtonEvent_PickupLifeSpirit(BaseComponent baseComp)
    {
        if (isPlayAnim)
            return;
        PreyLifeSpiritComponent comp = baseComp as PreyLifeSpiritComponent;
        if (comp == null)
            return;
        if (comp.PreyLifeSoulData == null)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance._LifeSoulDepot.IsPackNotFull(false))
        {
            LifeSpiritModule.Instance.SendCollectSoul(LifeSoulOpType.ONCE, comp.PreyLifeSoulData.uid);
        }
        else
        {
            TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)(PlayerData.Instance._PreyLifeSoulInfo.buy_times + 1));
            if (data == null || data.LifeSoulPackConsume == null) // data为NULL 则可默认为已经达到购买上限
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTFULL);
            }
            else if (data.LifeSoulPackConsume.Type == ECurrencyType.None && data.LifeSoulPackConsume.Number == 0)  // 消耗金币类型为0则说明无购买次数
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTFULL);
            }
            else
            {
                string consumedesc = data.LifeSoulPackConsume.Number.ToString() + CommonFunction.GetConsumeTypeDesc(data.LifeSoulPackConsume.Type);
                int count = 5;
                if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.LIFESOUL_BUYGRID_COUNT), out count))
                {
                    count = 5;
                }
                string content = string.Format(ConstString.LIFESPIRITPACK_ADDPACKGRID, consumedesc, count);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content, () =>
                {
                    if (CommonFunction.CheckMoneyEnough(data.LifeSoulPackConsume.Type, data.LifeSoulPackConsume.Number, true))
                    {
                        LifeSpiritModule.Instance.SendBuySoulGrid();
                    }
                }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
            }
        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        if (isPlayAnim)
            return;
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PREYLIFESPIRITVIEW);
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseHuntSoulView);
    }

    public void ButtonEvent_LockBtn(BaseComponent baseComp)
    {
        if (isPlayAnim)
            return;
        PreyLockBtnComponent comp = baseComp as PreyLockBtnComponent;
        if (comp == null)
            return;
        if (comp.IsLock)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if ((int)comp.LockType < PlayerData.Instance._PreyLifeSoulInfo.lock_level)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.PREYLIFESPIRIT_LOCKLIFESPIRIT_HIGHTIP, () =>
            {
                LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType((int)comp.LockType);
                if (info == null)
                    return;
                if (CommonFunction.CheckMoneyEnough(info.specify_price.Type, info.specify_price.Number, true))
                {
                    if (IsCanPreyLifeSpirit())
                    {
                        LifeSpiritModule.Instance.SendExploreSoul(LifeSoulOpType.ONCE, (int)comp.LockType);
                    }
                }
            }, null, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
        }
        else
        {
            LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType((int)comp.LockType);
            if (info == null)
                return;
            if (CommonFunction.CheckMoneyEnough(info.specify_price.Type, info.specify_price.Number, true))
            {
                if (IsCanPreyLifeSpirit())
                {
                    LifeSpiritModule.Instance.SendExploreSoul(LifeSoulOpType.ONCE, (int)comp.LockType);
                }
            }
        }

    }


    public void ButtonEvent_QuickPickup(GameObject btn)
    {
        if (isPlayAnim)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (PlayerData.Instance._PreyLifeSoulInfo.uids == null || PlayerData.Instance._PreyLifeSoulInfo.uids.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PICKUPLIFESPIRIT_LIFESPIRIT_NOLIFESPIRIT);
            return;
        }
        if (PlayerData.Instance._LifeSoulDepot.IsPackNotFull())
        {
            LifeSpiritModule.Instance.SendCollectSoul(LifeSoulOpType.ONE_KEY, 0);
        }
        else
        {
            TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)(PlayerData.Instance._PreyLifeSoulInfo.buy_times + 1));
            if (data == null || data.LifeSoulPackConsume == null) // data为NULL 则可默认为已经达到购买上限
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTFULL);
            }
            else if (data.LifeSoulPackConsume.Type == ECurrencyType.None && data.LifeSoulPackConsume.Number == 0)  // 消耗金币类型为0则说明无购买次数
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_LIFESPIRIT_COUNTFULL);
            }
            else
            {
                string consumedesc = data.LifeSoulPackConsume.Number.ToString() + CommonFunction.GetConsumeTypeDesc(data.LifeSoulPackConsume.Type);
                int count = 5;
                if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.LIFESOUL_BUYGRID_COUNT), out count))
                {
                    count = 5;
                }
                string content = string.Format(ConstString.LIFESPIRITPACK_ADDPACKGRID, consumedesc, count);
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content, () =>
                {
                    if (CommonFunction.CheckMoneyEnough(data.LifeSoulPackConsume.Type, data.LifeSoulPackConsume.Number, true))
                    {
                        LifeSpiritModule.Instance.SendBuySoulGrid();
                    }
                }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
            }
        }
    }

    public void ButtonEvent_QuickPrey(GameObject btn)
    {
        if (isPlayAnim)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType(PlayerData.Instance._PreyLifeSoulInfo.lock_level);
        if (info == null)
            return;
        if (CommonFunction.CheckMoneyEnough(info.unspecify_price.Type, info.unspecify_price.Number, true))
        {
            if (IsCanPreyLifeSpirit())
            {
                LifeSpiritModule.Instance.SendExploreSoul(LifeSoulOpType.ONE_KEY, (int)PreyLockTypeEnum.None);
            }
        }
    }

    public void ButtonEvent_Prey(GameObject btn)
    {
        if (isPlayAnim)
            return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        LifeSoulBaseInfo info = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetDataByType(PlayerData.Instance._PreyLifeSoulInfo.lock_level);
        if (info == null)
            return;
        if (CommonFunction.CheckMoneyEnough(info.unspecify_price.Type, info.unspecify_price.Number, true))
        {
            if (IsCanPreyLifeSpirit())
            {
                LifeSpiritModule.Instance.SendExploreSoul(LifeSoulOpType.ONCE, (int)PreyLockTypeEnum.None);
            }
        }
        GuideManager.Instance.CheckTrigger(GuideTrigger.ClickHuntSoulBtn);

    }

    public void ButtonEvent_Rule(GameObject btn)
    {
        if (isPlayAnim)
            return;
        OpenRulePanel();
    }
    private void ButtonEvent_CloseRule(GameObject btn)
    {
        if (isPlayAnim)
            return;
        view.Gobj_RulePanel.SetActive(false);
    }


    #endregion

    public override void Uninitialize()
    {
        isPlayAnim = false;
        Scheduler.Instance.RemoveTimer(PlayCollectAnim);
        //Scheduler.Instance.RemoveUpdator(CheckLockBtnAnim);
        Scheduler.Instance.RemoveTimer(PlayPreyAnim);
        Main.Instance.StopCoroutine(UpdatePreyLifeSpirit());
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (preyLifeSpirit_dic != null)
            preyLifeSpirit_dic.Clear();
        if (lockComp_dic != null)
            lockComp_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_QuickPickup.gameObject).onClick = ButtonEvent_QuickPickup;
        UIEventListener.Get(view.Btn_QuickPrey.gameObject).onClick = ButtonEvent_QuickPrey;
        UIEventListener.Get(view.Btn_Prey.gameObject).onClick = ButtonEvent_Prey;
        UIEventListener.Get(view.Btn_Rule.gameObject).onClick = ButtonEvent_Rule;
        UIEventListener.Get(view.Spt_RuleMask.gameObject).onClick = ButtonEvent_CloseRule;
    }


}
