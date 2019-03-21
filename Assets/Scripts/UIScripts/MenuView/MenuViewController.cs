using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;
public class MenuViewController : UIBase
{
    public MenuView view;
    private TweenScale tscale, mainTscale;
    private GameAcitvityAwardStateEnum _onlineState = 0;
    //没有首冲 在线礼包往左移动  
    private Vector3 _leftPos = new Vector3(-484, 631, 0);
    private Vector3 _normalPos = new Vector3(-404, 631, 0);
    private Vector3 _rightPos = new Vector3(-324, 631, 0);
    private List<CommonItemData> _onlineAwards;
    private bool isOpen;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new MenuView();
            view.Initialize();

            CommonFunction.SetSpriteName(view.Spt_BtnOpenMenuBG, GlobalConst.SpriteName.MUEN_OFF);
            view.Gobj_Menu.transform.localScale = _one;
            view.Gobj_Icon.transform.localScale = _one;
            isOpen = true;
        }

        view.Gobj_LifeSpiritNotify.gameObject.SetActive(true);
        MainCityModule.Instance.GetWaitingNotify();
        view.Spt_ActivityNotice.gameObject.SetActive(false);
        //view.Spt_BagNotice.gameObject.SetActive(false);
        ShowSingleNotify(view.Spt_ActivityNotice.gameObject, view.Lbl_ActivityNoticeNum,
                         PlayerData.Instance.NewGameActivityIDList.Count);
        SetAnnounceBtn();
        SetFirstPayBtn();
        SetFirstLoginBtn();
        //InitFuncState(MainCityModule.Instance.LockFuncs);
        UpdateHeroNotice();
        UpdateBagNotice();
        UpdatePetNotify();
        UpdateLifeSoulNotice();
        OnUpdateOnlinePackageEvent();
        BtnEventBinding();
        UpdateNoviceTaskBtnStatus();
        SetLivenessLock();
        Scheduler.Instance.AddTimer(1, true, UpdateNoviceTaskBtnStatus);
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent += _SoldierDepot_SoldierErrorEvent;
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent += UpdateLifeSoulNotice;
        PlayerData.Instance._PetDepot.PetAddEvent += UpdatePetNotify;
        PlayerData.Instance._SoldierDepot.CheckSoldierPromt();
        view.Grd_Up.Reposition();

        CheckTotalNotify();
    }

    private void InitFuncState(List<uint> funcList)
    {
        if (funcList == null)
            return;
        for (int i = 0; i < funcList.Count; i++)
        {
            SetFuncState((OpenFunctionType)funcList[i]);
        }
    }

    private void SetAnnounceBtn()
    {
        view.Btn_Announcement.gameObject.SetActive(MainCityModule.Instance.HasGameAnnouncement());
    }

    private void SetFuncState(OpenFunctionType func)
    {
        switch (func)
        {
            case OpenFunctionType.Store:
                {
                    view.Btn_Mall.gameObject.SetActive(false);
                    break;
                }
        }
    }

    void _SoldierDepot_SoldierErrorEvent(SoldierControl control, int errorCode, ulong uid = 0)
    {
        if (control == SoldierControl.CanEPISTANUPSTAR)
        {
            if (errorCode == 0)
                this.view.Spt_GeneralNotice.gameObject.SetActive(true);
            else
                this.view.Spt_GeneralNotice.gameObject.SetActive(false);
        }
        CheckTotalNotify();
    }
    public void SetFirstPayBtn()
    {
        if (PlayerData.Instance.FirstPayGift == 2)
        {
            view.Btn_FirstPay.gameObject.SetActive(false);
            //view.Btn_OnlinePackage.transform.localPosition = _leftPos;
        }
        else
        {
            view.Btn_FirstPay.gameObject.SetActive(true);
            //view.Btn_OnlinePackage.transform.localPosition = _normalPos;
        }
        SetNoviceTaskBtn();
        //view.Grd_Up.Reposition();

    }

    public void SetFirstLoginBtn()
    {
        if (GlobalConst.PLATFORM == TargetPlatforms.SF_7723
            && !PlayerData.Instance.GotFirstLoginReward)
        {
            view.Btn_FirstLogin.gameObject.SetActive(true);
        }
        else
        {
            view.Btn_FirstLogin.gameObject.SetActive(false);
        }
        view.Grd_Up.Reposition();
    }

    public void SetNoviceTaskBtn()
    {
        view.Grd_Up.Reposition();
        return;
        if (view.Btn_OnlinePackage.gameObject.activeSelf)
        {
            if (view.Btn_FirstPay.gameObject.activeSelf)
            {
                view.Btn_NoviceTask.transform.localPosition = _rightPos;
            }
            else
            {
                view.Btn_NoviceTask.transform.localPosition = _normalPos;
            }
        }
        else
        {
            if (view.Btn_FirstPay.gameObject.activeSelf)
            {
                view.Btn_NoviceTask.transform.localPosition = _normalPos;
            }
            else
            {
                view.Btn_NoviceTask.transform.localPosition = _leftPos;
            }
        }
    }


    private void UpdateNoviceTaskBtnStatus()
    {
        long deadline = (long)PlayerData.Instance._NoviceTaskEndTime - Main.mTime;
        if (deadline <= 0)
        {
            view.Btn_NoviceTask.gameObject.SetActive(false);
            view.Lbl_NoviceTaskTime.text = string.Empty;
            Scheduler.Instance.RemoveTimer(UpdateNoviceTaskBtnStatus);
        }
        else
        {
            view.Btn_NoviceTask.gameObject.SetActive(true);
            long day = deadline / 86400;
            if (day > 0)
            {
                view.Lbl_NoviceTaskTime.text = string.Format(ConstString.TASK_DAY_NOBRACKETS, day);
            }
            else
            {
                long time = deadline % 86400;
                view.Lbl_NoviceTaskTime.text = CommonFunction.GetTimeString(time);
            }
        }
    }

    private void UpdateHeroNotice()
    {
        if (view == null || view.Spt_HeroNotice == null)
        {
            return;
        }

        if (PlayerData.Instance._SkillsDepot.IsHadSkillCanStrong() ||
           PlayerData.Instance._ArtifactedDepot.IsHadEquipCanStrong() ||
           PlayerData.Instance._ArtifactedDepot.IsHadEquipCanEquip())
        {
            view.Spt_HeroNotice.gameObject.SetActive(true);
        }
        else
        {
            view.Spt_HeroNotice.gameObject.SetActive(false);
        }
        CheckTotalNotify();
    }

    private void UpdateBagNotice()
    {
        view.Spt_BagNotice.gameObject.SetActive(false);
        bool isIntensify = false;
        for (int i = 0; i < PlayerData.Instance._WeaponDepot._weaponList.Count; i++)
        {
            Weapon weapon = PlayerData.Instance._WeaponDepot._weaponList[i];
            if (weapon.enableStrong() == WeaponCheck.Ok)
            {
                isIntensify = true;
                break;
            }
        }
        if (isIntensify)
        {
            view.Spt_BagNotice.gameObject.SetActive(true);
            CheckTotalNotify();
            return;
        }
        bool isCompose = false;
        for (int i = 0; i < PlayerData.Instance._ChipBag.Count; i++)
        {
            Item item = PlayerData.Instance._ChipBag[i];
            ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (itemInfo != null)
            {
                if (itemInfo.type != (int)ItemTypeEnum.SoldierChip && item.num != 0)
                {
                    if (item.num >= itemInfo.compound_count)
                    {
                        isCompose = true;
                        break;
                    }
                }
            }
        }
        view.Spt_BagNotice.gameObject.SetActive(isCompose);
        CheckTotalNotify();
    }
    public void SetLivenessLock()
    {
        bool state = CommonFunction.CheckFuncIsOpen(OpenFunctionType.Liveness, false);
        view.Spt_LivenessLock.gameObject.SetActive(!state);
        PlayerData.Instance.UpdateLevelEvent -= SetLivenessLock;
        if (!state)
        {
            PlayerData.Instance.UpdateLevelEvent += SetLivenessLock;
        }
    }
    private void UpdatePetNotify()
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Pet, false))
        {
            view.Spt_PetNotify.enabled = false;
            view.Spt_PetLock.enabled = true;
            return;
        }
        else
        {
            view.Spt_PetLock.enabled = false;
        }
        view.Spt_PetNotify.gameObject.SetActive(true);
        bool isNotify = false;
        List<PetData> pet_list = PlayerData.Instance._PetDepot.GetOwnedPets();
        for (int i = 0; i < pet_list.Count; i++)
        {
            PetData tmp = pet_list[i];
            if (tmp == null)
                continue;
            if (tmp.IsNew)
            {
                isNotify = true;
                break;
            }
            if (tmp.UpgradeMatData != null)
            {
                if (tmp.UpgradeMatData != null && tmp.UpgradeMatData.materials != null && tmp.UpgradeMatData.materials.Count > 0)
                {
                    PetCommonMaterilData matData = tmp.UpgradeMatData.materials[0];//界面设计上仅会出现一个 add by taiwei
                    int ownUpgradeMatCount = PlayerData.Instance.GetItemCountByID(matData.materailID);
                    if (ownUpgradeMatCount >= matData.num && PlayerData.Instance.GoldIsEnough(tmp.UpgradeMatData.cerrencyType, tmp.UpgradeMatData.cerrencyNum))
                    {
                        isNotify = true;
                        break;
                    }
                }

            }

            if (tmp.SkillMatData != null)
            {
                if (tmp.SkillMatData.MaterialList != null && tmp.SkillMatData.MaterialList.Count > 0)
                {
                    KeyValuePair<uint, int> mat_value = new KeyValuePair<uint, int>();
                    foreach (KeyValuePair<uint, int> t in tmp.SkillMatData.MaterialList)
                    {
                        mat_value = t;
                        break;
                    }
                    int ownSkillmatCount = PlayerData.Instance.GetItemCountByID(mat_value.Key);
                    if (ownSkillmatCount >= mat_value.Value && PlayerData.Instance.GoldIsEnough(tmp.SkillMatData.costType, tmp.SkillMatData.Cost))
                    {
                        isNotify = true;
                        break;
                    }
                }
            }
        }
        view.Spt_PetNotify.enabled = isNotify;
        CheckTotalNotify();
    }

    private void UpdateLifeSoulNotice()
    {
        view.Gobj_LifeSpiritNotify.enabled = false;

        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.LifeSoulSystm, false))
        {
            view.Spt_LifeSoulLock.enabled = true;
            return;
        }
        else
        {
            view.Spt_LifeSoulLock.enabled = false;
        }

        CheckTotalNotify();
        List<LifeSoulBaseInfo> infoList = ConfigManager.Instance.mLifeSoulBaseInfoConfig.GetBaseInfoList();
        if (infoList == null || infoList.Count <= 0)
            return;
        int index = 0;
        for (int i = 0; i < infoList.Count; i++)
        {
            LifeSoulBaseInfo info = infoList[i];
            if (info == null)
                continue;
            if (info.player_unlock_level <= PlayerData.Instance._Level)
                index++;
        }

        List<LifeSoulData> changeList = PlayerData.Instance._LifeSoulDepot.GetAllExchangeLifeSouls(true);
        if (changeList == null || changeList.Count <= 0)
        {
            return;
        }
        List<LifeSoulData> equipedList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        if (equipedList == null || equipedList.Count <= 0)
        {
            view.Gobj_LifeSpiritNotify.enabled = true;
            CheckTotalNotify();
            return;
        }
        for (int i = 0; i < changeList.Count; i++)
        {
            LifeSoulData data = changeList[i];
            if (data == null)
                continue;
            LifeSoulData tmpData = equipedList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulInfo.type == data.SoulInfo.type;
            });
            if (tmpData == null)
            {
                if (equipedList.Count < index)
                {
                    view.Gobj_LifeSpiritNotify.enabled = true;
                    CheckTotalNotify();
                    return;
                }
            }
            else
            {
                if (tmpData.SoulInfo.quality < data.SoulInfo.quality)
                {
                    view.Gobj_LifeSpiritNotify.enabled = true;
                    CheckTotalNotify();
                    return;
                }
            }
        }
    }

    private Vector3 _one = new Vector3(0.8f, 0.8f, 1);

    private void UpdateDropDownMenu()
    {
        if (tscale == null)
        {
            tscale = view.Gobj_Menu.GetComponent<TweenScale>();
        }
        if (tscale == null)
        {
            tscale = view.Gobj_Menu.AddComponent<TweenScale>();
        }
        if (view.Gobj_Menu.transform.localScale == _one)
        {
            CommonFunction.SetSpriteName(view.Spt_BtnOpenMenuBG, GlobalConst.SpriteName.MUEN_ON);
            tscale.Restart();
            tscale.from = _one;
            tscale.to = new Vector3(0.8f, 0, 1);
            tscale.PlayForward();
            isOpen = false;
            GuideManager.Instance.CheckTrigger(GuideTrigger.MainCityMenuClose);
        }
        else if (view.Gobj_Menu.transform.localScale == new Vector3(0.8f, 0, 1))
        {
            CommonFunction.SetSpriteName(view.Spt_BtnOpenMenuBG, GlobalConst.SpriteName.MUEN_OFF);

            tscale.Restart();
            tscale.from = new Vector3(0.8f, 0, 1);
            tscale.to = _one;
            tscale.PlayForward();
            isOpen = true;
            GuideManager.Instance.CheckTrigger(GuideTrigger.MainCityMenuOpen);
        }
        TweenLivenessTip();
    }

    private void UpdateMainMenu()
    {
        if (mainTscale == null)
        {
            mainTscale = view.Gobj_Icon.GetComponent<TweenScale>();
        }
        if (mainTscale == null)
        {
            mainTscale = view.Gobj_Icon.AddComponent<TweenScale>();
        }
        if (view.Gobj_Icon.transform.localScale == _one)
        {
            mainTscale.Restart();
            mainTscale.from = _one;
            mainTscale.to = new Vector3(0, 0.8f, 1);
            mainTscale.PlayForward();
        }
        if (view.Gobj_Icon.transform.localScale == new Vector3(0, 0.8f, 1))
        {
            mainTscale.Restart();
            mainTscale.from = new Vector3(0, 0.8f, 1);
            mainTscale.to = _one;
            mainTscale.PlayForward();
        }
    }

    /// <summary>
    ///  0 倒计时 1 可以领取 2 已经领取完
    /// </summary>
    private void OnUpdateOnlinePackageEvent()
    {
        if (this.view == null)
            return;

        _onlineState = (GameAcitvityAwardStateEnum)PlayerData.Instance.OnlineRewardInfo.reward_sign;
        if (_onlineState == GameAcitvityAwardStateEnum.CanNotReceive)
        {
            view.Btn_OnlinePackage.gameObject.SetActive(true);
            view.Spt_OnlineFG.gameObject.SetActive(false);
            view.Spt_OnlineEffect.gameObject.SetActive(false);
            view.Lbl_TimeLabel.text = CommonFunction.GetTimeString(MainCityModule.Instance.RestOnlineTime);
        }
        else if (_onlineState == GameAcitvityAwardStateEnum.CanReceive)
        {
            view.Btn_OnlinePackage.gameObject.SetActive(true);
            view.Lbl_TimeLabel.text = "";
            view.Spt_OnlineFG.gameObject.SetActive(true);
            view.Spt_OnlineEffect.gameObject.SetActive(true);
        }
        else if (_onlineState == GameAcitvityAwardStateEnum.Recieved)
        {
            view.Btn_OnlinePackage.gameObject.SetActive(false);
            SetNoviceTaskBtn();
        }
        //SetNoviceTaskBtn();
    }

    public bool ShowNofity(NotifyRefresh resp)
    {
        bool result = false;
        //Debug.LogError(resp.type + "         " + resp.num);
        switch (resp.type)
        {
            case NotifyType.NOTICE:
                {
                    ShowSingleNotify(view.Spt_ActivityNotice.gameObject, view.Lbl_ActivityNoticeNum, resp.num);
                    result = true;
                    break;
                }
            case NotifyType.TASK:
                {
                    ShowSingleNotify(view.Spt_TaskNotice.gameObject, view.Lbl_TaskNoticeNum, resp.num);
                    result = true;
                    break;
                }
            case NotifyType.LIVENESS:
                {
                    ShowSingleNotify(view.Spt_LivenessNotice.gameObject, view.Lbl_LivenessNoticeNum, resp.num);
                    result = true;
                    break;
                }
            case NotifyType.MAIL:
                {
                    //ShowSingleNotify(view.Spt_MailNotice.gameObject, view.Lbl_MailNoticeNum, resp.num);
                    result = true;
                    break;
                }
            case NotifyType.SIGN:
                {
                    ShowSingleNotify(view.Spt_SignNotice.gameObject, view.Lbl_SignNoticeNum, resp.num);
                    result = true;
                    break;
                }
            case NotifyType.ACTIVITY:
                {
                    int num = 1;
                    if (resp.activities != null)
                    {
                        num = resp.activities.Count;
                    }
                    ShowSingleNotify(view.Spt_ActivityNotice.gameObject, view.Lbl_ActivityNoticeNum, num);
                    result = true;
                    break;
                }
            case NotifyType.NEWHAND_TASK:
                {
                    view.Gobj_NoviceTaskNotify.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.INTERNEL_FRIENDS:
                {
                    //Debug.LogError("ssssssssssssssssssssssss  =" + resp.num);
                    view.Go_HaoYouNotify.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
            case NotifyType.ACHIEVEMENT:
                {
                    if (resp.status == 1)
                        view.Gobj_AchievementNotify.SetActive(resp.num > 0);
                    result = true;
                    break;
                }
        }
        CheckTotalNotify();
        return result;
    }

    public void TryCloseGameActivityNotify()
    {
        ShowSingleNotify(view.Spt_ActivityNotice.gameObject, view.Lbl_ActivityNoticeNum, PlayerData.Instance.NewGameActivityIDList.Count);
    }

    private void ShowSingleNotify(GameObject go, UILabel lbl, int num)
    {
        go.SetActive(num > 0);
        lbl.text = string.Empty;// num.ToString();
    }

    public void OpenMenu()
    {
        if (!isOpen)
            ButtonEvent_OpenMenu(null);

        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenMenuToolCompleted);
    }

    #region 活跃任务提示框
    Vector3 _tipPosOpen = new Vector3(276, -200, 0);
    Vector3 _tipPosClose = new Vector3(416, -200, 0);
    float _tipScleClose = 0.001f;
    float _tipTweenDuration = 0;
    LivenessTask _tipTask;
    LivenessData _tipTaskData;
    bool _tipIsShowing = false;
    public void ShowUnFinishLivenessTip(bool show, LivenessTask task)
    {
        _tipTask = task;
        if (show)
        {
            _tipTaskData = ConfigManager.Instance.mLivenessConfig.GetLivenessDataByID(_tipTask.id);
            string format = "";
            if (_tipTaskData.OpenUIType == null)
            {
                format = ConstString.FORMAT_LIVENESS_TIP;
            }
            else
            {
                format = ConstString.FORMAT_LIVENESS_TIP_CANGO;
            }
            view.Lbl_LivenessTip.text = string.Format(format, _tipTaskData.TimesLimit - _tipTask.num, _tipTaskData.Name);
            UIEventListener.Get(view.Spt_LivenessTipBG.gameObject).onClick = ClickLivenessTip;
            view.Gobj_LivenessTip.SetActive(true);
            ShowTipAni(true);
        }
        else
        {
            UIEventListener.Get(view.Spt_LivenessTipBG.gameObject).onClick = null;
            ShowTipAni(false);
        }
    }

    private void ShowTipAni(bool show)
    {
        if (_tipIsShowing == show)
            return;
        _tipIsShowing = show;
        if (!show)
        {
            view.TipTweenScle.from = Vector3.one;
            view.TipTweenScle.to = Vector3.one * _tipScleClose;
            view.TipTweenScle.ResetToBeginning();
            view.TipTweenScle.PlayForward();
        }
        else
        {
            view.TipTweenScle.from = Vector3.one * _tipScleClose;
            view.TipTweenScle.to = Vector3.one;
            view.TipTweenScle.ResetToBeginning();
            view.TipTweenScle.PlayForward();
        }
    }

    private void TweenLivenessTip()
    {
        if (_tipTweenDuration == 0)
        {
            _tipTweenDuration = tscale.duration;
        }

        if (!view.Gobj_LivenessTip.activeInHierarchy)
        {
            if (!isOpen)
            {
                view.Gobj_LivenessTip.transform.localPosition = _tipPosClose;
            }
            else
            {
                view.Gobj_LivenessTip.transform.localPosition = _tipPosOpen;
            }
            return;
        }

        if (!isOpen)
        {
            view.TipTweenPos.Restart();
            view.TipTweenPos.duration = _tipTweenDuration;
            view.TipTweenPos.from = _tipPosOpen;
            view.TipTweenPos.to = _tipPosClose;
            view.TipTweenPos.PlayForward();
        }
        else
        {
            view.TipTweenPos.Restart();
            view.TipTweenPos.duration = _tipTweenDuration;
            view.TipTweenPos.from = _tipPosClose;
            view.TipTweenPos.to = _tipPosOpen;
            view.TipTweenPos.PlayForward();
        }
    }

    private void ClickLivenessTip(GameObject go)
    {
        if (_tipTask != null)
        {
            ShowUnFinishLivenessTip(false, null);
            CommonFunction.OpenTargetView(_tipTaskData.OpenUI);
        }
    }

    #endregion

    private void ButtonEvent_OpenMenu(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));

        UpdateDropDownMenu();
        UpdateMainMenu();
        //GuideManager.Instance.CheckTrigger(GuideTrigger.ClickMenuBtn);
        CheckTotalNotify();

    }
    private void ButtonEvent_HaoYou(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FIRENDVIEW);
        //if (PlayerData.Instance.FbfriendDepot != null)
        //    PlayerData.Instance.FbfriendDepot.Init();
    }
    private void ButtonEvent_Hero(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Hero))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_HEROATT);
    }
    private void ButtonEvent_General(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Soldier))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERATT);
    }

    private void ButtonEvent_Pet(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Pet, true))
        {
            return;
        }
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PETSYSTEM);
    }

    private void ButtonEvent_Bag(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Package))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BACKPACK);
        UISystem.Instance.BackPackView.UpdateViewInfo();
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
    }
    //private void ButtonEvent_Shop(GameObject btn)
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    //    if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Store))
    //        return;

    //    UISystem.Instance.ShowGameUI(StoreView.UIName);
    //    UISystem.Instance.StoreView.ShowStore(ShopType.ST_NomalShop);
    //}
    private void ButtonEvent_SignInButton(GameObject Btn)  //签到界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.ContinuousLogin))
            return;
        UISystem.Instance.ShowGameUI(SignView.UIName);
    }

    private void ButtonEvent_RaidersButton(GameObject Btn)
    {
        //Debug.LogError("攻略");
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (!CommonFunction.CheckIsOpen(OpenFunctionType.GongLue, true))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_WALKTHROUGHVIEW);
    }
    private void ButtonEvent_RechargeButton(GameObject Btn)//充值界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        //SDKManager.Instance.SDKOperate_DepositOperate("", "1", 0);
        //if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.VIP))
        //    return;
        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
        UISystem.Instance.VipRechargeView.ShowRecharge();
    }
    private void ButtonEvent_NoticeButton(GameObject Btn)  //公告界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.ActivityAnnouncement))
            return;
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, "公告界面");
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GAMEACTIVITY);
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
    }
    private void ButtonEvent_TaskButton(GameObject Btn)    //任务界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Task))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TASKVIEW);
    }
    private void ButtonEvent_ActiveButton(GameObject Btn)  //活跃界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Liveness))
            return;
        UISystem.Instance.ShowGameUI(LivenessView.UIName);
    }
    public void ButtonEvent_IllustratedButton(GameObject btn)
    {

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.TuJian, true))
            return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERILLVIEW);


    }
    public void ButtonEvent_FirstPay(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(FirstPayView.UIName);
        UISystem.Instance.FirstPayView.ShowGift(true);
    }

    public void ButtonEvent_NoviceTask(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_NOVICETASKVIEW);
    }

    public void ButtonEvent_OnlinePackage(GameObject btn)
    {
        GameOnlinePackageData data = ConfigManager.Instance.mGameOnlinePackageConfig.GetDataByID((uint)PlayerData.Instance.OnlineRewardInfo.next_reward_id);
        List<CommonItemData> list = new List<CommonItemData>();
        if (data != null)
        {
            list.Clear();
            list.AddRange(CommonFunction.GetCommonItemDataList(data.mDropID));
        }
        _onlineState = (GameAcitvityAwardStateEnum)PlayerData.Instance.OnlineRewardInfo.reward_sign;
        if (_onlineState == GameAcitvityAwardStateEnum.CanNotReceive)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_RECIEVERESLUTVERTVIEW);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.ONLINEPACKAGE_LABEL_SHOWAWARD);
        }
        else if (_onlineState == GameAcitvityAwardStateEnum.CanReceive)
        {
            if (list != null && list.Count > 0)
            {
                if (CommonFunction.GetItemOverflowTip(list))
                    return;
            }
            MainCityModule.Instance.OnSendOnlineReward();
        }
    }

    private void ButtonEvent_FirstLogin(GameObject go)
    {
        UISystem.Instance.ShowGameUI(FirstPayView.UIName);
        UISystem.Instance.FirstPayView.ShowGift(false);
    }
    //private void ButtonEvent_MailButton(GameObject Btn)    //邮件界面
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    //    if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.Mail))
    //        return;
    //    MailModule.Instance.OpenMailView();
    //}
    public void ButtonEvent_Mall(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_STORE);
        UISystem.Instance.StoreView.ShowStore(ShopType.ST_NomalShop);
    }
    public void ButtonEvent_Announce(GameObject btn)
    {
        MainCityModule.Instance.ShowGameAnnouncement();
    }

    public void ButtonEvent_Achievement(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ACHIEVEMENT);
    }

    public void ButtonEvent_LifeSpirit(GameObject btn)
    {
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.LifeSoulSystm, false))
        {
            int lv = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.LifeSoulSystm).openLevel;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.LIFESOULSYSTEM_LOCK, lv));
        }
        else
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITVIW);
        }
    }
    public void ButtonEvent_Recycle(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(RecycleView.UIName);
    }

    private void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Announcement.gameObject).onClick = ButtonEvent_Announce;
        UIEventListener.Get(view.Btn_FirstLogin.gameObject).onClick = ButtonEvent_FirstLogin;
        UIEventListener.Get(view.Btn_HaoYou.gameObject).onClick = ButtonEvent_HaoYou;
        UIEventListener.Get(view.Btn_Raiders.gameObject).onClick = ButtonEvent_RaidersButton;
        UIEventListener.Get(view.Btn_Illustrated.gameObject).onClick = ButtonEvent_IllustratedButton;
        UIEventListener.Get(view.Btn_SignInButton.gameObject).onClick = ButtonEvent_SignInButton;
        UIEventListener.Get(view.Btn_RechargeButton.gameObject).onClick = ButtonEvent_RechargeButton;
        UIEventListener.Get(view.Btn_NoticeButton.gameObject).onClick = ButtonEvent_NoticeButton;
        UIEventListener.Get(view.Btn_TaskButton.gameObject).onClick = ButtonEvent_TaskButton;
        UIEventListener.Get(view.Btn_ActiveButton.gameObject).onClick = ButtonEvent_ActiveButton;
        //UIEventListener.Get(view.Btn_MailButton.gameObject).onClick = ButtonEvent_MailButton;
        UIEventListener.Get(view.Btn_Hero.gameObject).onClick = ButtonEvent_Hero;
        UIEventListener.Get(view.Btn_General.gameObject).onClick = ButtonEvent_General;
        UIEventListener.Get(view.Btn_Bag.gameObject).onClick = ButtonEvent_Bag;
        UIEventListener.Get(view.Btn_Mall.gameObject).onClick = ButtonEvent_Mall;
        //UIEventListener.Get(view.Btn_Shop.gameObject).onClick = ButtonEvent_Shop;
        UIEventListener.Get(view.Btn_OpenMenu.gameObject).onClick = ButtonEvent_OpenMenu;
        UIEventListener.Get(view.Btn_FirstPay.gameObject).onClick = ButtonEvent_FirstPay;
        UIEventListener.Get(view.Btn_OnlinePackage.gameObject).onClick = ButtonEvent_OnlinePackage;
        UIEventListener.Get(view.Btn_NoviceTask.gameObject).onClick = ButtonEvent_NoviceTask;
        UIEventListener.Get(view.Btn_Achievement.gameObject).onClick = ButtonEvent_Achievement;
        UIEventListener.Get(view.Btn_LifeSpirit.gameObject).onClick = ButtonEvent_LifeSpirit;
        UIEventListener.Get(view.Btn_Recycle.gameObject).onClick = ButtonEvent_Recycle;
        UIEventListener.Get(view.Gobj_Pet).onClick = ButtonEvent_Pet;
        PlayerData.Instance._WeaponDepot.ErrotDeleteEvent += _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SkillsDepot.SkillsDepotEvent += _SkillsDepot_SkillsDepotEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent += Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdateLevelEvent += OnPlayerLevelUpEvent;
        PlayerData.Instance.UpdateVipEvent += OnPlayerVIPEvent;

        PlayerData.Instance.UpdateOnlineEvent += OnUpdateOnlinePackageEvent;

    }

    private void OnPlayerLevelUpEvent()
    {
        UpdateBagNotice();
        UpdatePetNotify();
        UpdateLifeSoulNotice();
    }

    private void OnPlayerVIPEvent()
    {
        UpdateBagNotice();
        UpdatePetNotify();
    }

    void Instance_UpdatePlayerGoldEvent()
    {
        UpdateHeroNotice();
        UpdatePetNotify();
    }

    void Instance_UpdatePlayerItemsEvent()
    {
        UpdateHeroNotice();
        UpdateBagNotice();
        UpdatePetNotify();
    }

    void _SkillsDepot_SkillsDepotEvent(SkillChange change, int Slot = -1, ulong uID = 0)
    {
        if (change == SkillChange.All)
        {
            UpdateHeroNotice();
        }
    }

    void _WeaponDepot_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        if (errorCode == 0)
        {
            if (control == EquipControl.GetoffEquipAndArtifactResp ||
                control == EquipControl.PromoteEquipAndArtifactResp ||
                control == EquipControl.PutonEquipAndArtifactResp)
            {
                UpdateHeroNotice();
            }
        }
        UpdateBagNotice();
    }

    private void UnBtnEventBinding()
    {
        UIEventListener.Get(view.Btn_HaoYou.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Illustrated.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_SignInButton.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_RechargeButton.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_NoticeButton.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_TaskButton.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_ActiveButton.gameObject).onClick = null;
        //UIEventListener.Get(view.Btn_MailButton.gameObject).onClick = ButtonEvent_MailButton;
        UIEventListener.Get(view.Btn_Hero.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_General.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Bag.gameObject).onClick = null;
        //UIEventListener.Get(view.Btn_Shop.gameObject).onClick = ButtonEvent_Shop;
        UIEventListener.Get(view.Btn_OpenMenu.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_FirstPay.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_OnlinePackage.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Announcement.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Achievement.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_LifeSpirit.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Recycle.gameObject).onClick = null;
    }

    public override void Uninitialize()
    {
        //view.Gobj_Menu.transform.localScale = new Vector3(1, 0, 1);
        //view.Gobj_Icon.transform.localScale = new Vector3(0, 1, 1);
        UnBtnEventBinding();
        PlayerData.Instance._WeaponDepot.ErrotDeleteEvent -= _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SkillsDepot.SkillsDepotEvent -= _SkillsDepot_SkillsDepotEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent -= Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdateLevelEvent -= OnPlayerLevelUpEvent;
        PlayerData.Instance.UpdateVipEvent -= OnPlayerVIPEvent;
        PlayerData.Instance.UpdateOnlineEvent -= OnUpdateOnlinePackageEvent;
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent -= UpdateLifeSoulNotice;
        PlayerData.Instance._PetDepot.PetAddEvent -= UpdatePetNotify;
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent -= _SoldierDepot_SoldierErrorEvent;
        Scheduler.Instance.RemoveTimer(UpdateNoviceTaskBtnStatus);
    }

    public override void Destroy()
    {
        PlayerData.Instance._WeaponDepot.ErrotDeleteEvent -= _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SkillsDepot.SkillsDepotEvent -= _SkillsDepot_SkillsDepotEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent -= Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdateOnlineEvent -= OnUpdateOnlinePackageEvent;
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent -= _SoldierDepot_SoldierErrorEvent;
        view = null;
        _tipIsShowing = false;
    }
    private void CheckTotalNotify()
    {

        if (!isOpen)
        {

            if (view.Gobj_AchievementNotify.gameObject.activeSelf || view.Spt_TaskNotice.gameObject.activeSelf || view.Spt_LivenessNotice.gameObject.activeSelf ||
                view.Spt_HeroNotice.gameObject.activeSelf || view.Spt_GeneralNotice.gameObject.activeSelf || view.Spt_BagNotice.gameObject.activeSelf ||
                view.Go_HaoYouNotify.gameObject.activeSelf || (view.Gobj_LifeSpiritNotify.gameObject.activeSelf && view.Gobj_LifeSpiritNotify.enabled) ||
                (view.Spt_PetNotify.gameObject.activeSelf && view.Spt_PetNotify.enabled))
            {
                view.Gobj_TotalNotify.SetActive(true);
            }
            else
            {
                view.Gobj_TotalNotify.SetActive(false);
            }
        }
        else view.Gobj_TotalNotify.SetActive(false);
    }
}
