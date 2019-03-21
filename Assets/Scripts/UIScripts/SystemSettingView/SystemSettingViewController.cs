using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
using System.Collections.Generic;
using fogs.proto.msg;
using System.Linq;

public class SystemSettingViewController : UIBase
{
    private List<FrameInfo> _fUnlockFrameList;
    private List<FrameInfo> _tUnlockFrameList;
    private List<FrameInfo> _LockedFrameList;
    private List<FrameInfo> _totalFrameList;
    private List<FrameData> tmpFrameList;
    private GameObject _frameItem;
    private static int MAXCOUNT = 6;
    public uint selIconId;
    public uint selDescId;
    public uint changeId;
    private string brown = "[D8BC81]";
    private string green = "[56DB67]";
    private string white = "[FFFFFF]";

    private List<IconInformation> _basicIconList;
    private List<IconInformation> _unlockAchievementIconList;
    private List<IconInformation> _lockAchievementIconList;
    private List<IconInformation> _unlockSoldierIconList;
    private List<IconInformation> _lockSoldierIconList;
    private List<HeadChangeItem> _basicIconItemList;
    private List<HeadChangeItem> _unlockAchievementIconItemList;
    private List<HeadChangeItem> _lockAchievementIconItemList;
    private List<HeadChangeItem> _unlockSoldierIconItemList;
    private List<HeadChangeItem> _lockSoldierIconItemList;
    private List<PlayerPortraitData> tmpIconList;

    public SystemSettingView view;
    private List<IconChangeItem> _iconChangeList;
    private List<HeadChangeItem> _headChangeList;
    private List<HeadChangeItem> _SoldHeadChangeList;
    private List<RemoveBlockItem> _RemoveBlockList;
    private bool IsOpenSound;
    private bool IsOpenAudio;
    private int _coolDownSeconds;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new SystemSettingView();
            view.Initialize();
            BtnEventBinding();
            _iconChangeList = new List<IconChangeItem>();
            _headChangeList = new List<HeadChangeItem>();
            _SoldHeadChangeList = new List<HeadChangeItem>();
            _RemoveBlockList = new List<RemoveBlockItem>();
        }
        InitPlayerInfo();
        InitAudioSound();//判断当前的音效的状态
        InitFeedbackBtn();
        FeedbackCount(PlayerData.Instance.FeedbackRestCount);
        PlayerData.Instance.NotifyResetEvent += UpdateFeedBackItem;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

        LoadFrameRes();
        InitAndClearFrameLists();
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(SystemSettingView.UIName);
    }
    #region 初始化

    public void InitFeedbackBtn()
    {
        if (PlayerData.Instance.FeedbackRestCount <= 0)
        {
            SetFeedBackSendBtn(false);
            view.Spt_FeedBackSendFGSp.gameObject.SetActive(true);


        }
        else
        {
            SetFeedBackSendBtn(true);
            view.Spt_FeedBackSendFGSp.gameObject.SetActive(true);
        }

    }
    public void FeedbackCount(uint times)//意见反馈剩余次数
    {
        PlayerData.Instance.FeedbackRestCount = times;
        //Debug.LogError("PlayerData.Instance.FeedbackRestTime = " + PlayerData.Instance.FeedbackRestCount);
        view.Lbl_FeedbackTodayLastLabel.text = ConstString.TODAY_LAST_LABEL + times.ToString();
    }
    public void InitAudioSound() //初始化音频音效
    {
        IsOpenAudio = SoundManager.Instance.Get_AudioMute;
        IsOpenSound = SoundManager.Instance.Get_MusicMute;
        Debug.Log("AudioSound  isOpenAudio:" + IsOpenAudio + "   isOpenSound:" + IsOpenSound);
        if (IsOpenSound)
        {
            view.Spt_MusicFGSprite.text = ConstString.MUSIC_OFF;
            CommonFunction.SetLabelColor_I(view.Spt_MusicFGSprite, 182, 101, 24, 255, 71, 37, 5, 255);
            //CommonFunction.SetSpriteName(view.Spt_MusicFGSprite, GlobalConst.SpriteName.MUSIC_OFF); 
            CommonFunction.SetSpriteName(view.Spt_MusicBGSprite, GlobalConst.SpriteName.MUSIC_BGOFF);
        }
        else
        {
            CommonFunction.SetSpriteName(view.Spt_MusicBGSprite, GlobalConst.SpriteName.MUSIC_BGON);
            CommonFunction.SetLabelColor_I(view.Spt_MusicFGSprite, 111, 52, 14, 255, 234, 201, 93, 255);
            //CommonFunction.SetSpriteName(view.Spt_MusicFGSprite, GlobalConst.SpriteName.MUSIC_ON);
            view.Spt_MusicFGSprite.text = ConstString.MUSIC_ON;
        }
        if (IsOpenAudio)
        {
            view.Spt_SoundFGSprite.text = ConstString.SOUND_OFF;
            CommonFunction.SetLabelColor_I(view.Spt_SoundFGSprite, 182, 101, 24, 255, 71, 37, 5, 255);

            //CommonFunction.SetSpriteName(view.Spt_SoundFGSprite, GlobalConst.SpriteName.SOUND_OFF);
            CommonFunction.SetSpriteName(view.Spt_SoundBGSprite, GlobalConst.SpriteName.MUSIC_BGOFF);
        }
        else
        {
            view.Spt_SoundFGSprite.text = ConstString.SOUND_ON;
            CommonFunction.SetLabelColor_I(view.Spt_SoundFGSprite, 111, 52, 14, 255, 234, 201, 93, 255);

            //CommonFunction.SetSpriteName(view.Spt_SoundFGSprite, GlobalConst.SpriteName.SOUND_ON);
            CommonFunction.SetSpriteName(view.Spt_SoundBGSprite, GlobalConst.SpriteName.MUSIC_BGON);
        }

    }
    public void InitPlayerInfo() //初始化角色信息
    {
        view.Lbl_PlayerNameLabel.text = PlayerData.Instance._NickName.ToString();
        view.Lbl_PlayerIDLabel.text = ConstString.PLAYER_IDCARD + PlayerData.Instance._AccountID.ToString();
        CommonFunction.SetSpriteName(view.Spt_IconSprite, CommonFunction.GetHeroIconNameByID(PlayerData.Instance.HeadID, true));
        SetSystemPlayerIcon();
    }
    public void UpdateSettingUI()//隐藏所有子界面
    {
        view.UIPanel_RemoveBlockView.gameObject.SetActive(false);
        view.Panel_PushSetting.gameObject.SetActive(false);
        view.UIPanel_FeedbackView.gameObject.SetActive(false);
        view.UIPanel_ChangeNameView.gameObject.SetActive(false);
        view.UIPanel_CDKeyView.gameObject.SetActive(false);
        view.UIPanel_BindingView.gameObject.SetActive(false);
    }

    #endregion
    #region 头像
    public void ButtonEvent_HeadChangeButton(GameObject Btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.UIPanel_HeadChangeView.SetActive(true);
        SystemSettingModule.Instance.SendEnterIconReq();
        //FillIconTest();
        //InitIconList(_testIconList);
        selIconId = 0;
        selDescId = 0;
    }
    public void ButtonEvent_CloseHeadChangeButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.ScrView_IconGroup.ResetPosition();
        view.UIPanel_HeadChangeView.SetActive(false);
        CloseHeadChange();
    }
    public void CloseHeadChange()//CloseItem
    {
        for (int i = _SoldHeadChangeList.Count - 1; i >= 0; i--)
        {
            HeadChangeItem item = _SoldHeadChangeList[i];
            item.StopTimeUpdate();
            _SoldHeadChangeList.Remove(item);
            GameObject.Destroy(item.gameObject);
        }
        UnInitIconItemList();
        view.Gobj_Desc.SetActive(false);

    }
    public void ButtonEvent_ChangeButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        if (changeId!=0)
        {
            SystemSettingModule.Instance.selfChangeIcon = true;
            SystemSettingModule.Instance.SendHeadChangeRequset(changeId);
            Debug.Log("send for change icon");
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ACHIEVEMENT_CHANGEICONERROR);
        }

    }
    /*
    public void InitChangeHeadView(List<uint> soldierData)//Instantiate
    {
        List<PlayerPortraitData> list = ConfigManager.Instance.mPlayerPortraitConfig.GetPTList();
        //普通头像：（直接读取配置表数据）
        if (_headChangeList.Count < 1)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].type == 1)
                {
                    GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.BaseGrid.transform);
                    obj.SetActive(true);
                    HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                    item.InitItem(list[i]);
                    _headChangeList.Add(item);
                }
            }
        }
        else
        {
            for (int i = 0; i < list.Count; i++)
            {
                _headChangeList[i].InitItem(list[i]);
            }
        }
        //武将头像：
        List<uint> data = SortHead(soldierData);
        for (int i = 0; i < soldierData.Count; i++)
        {

            //服务端进行排序:
            //GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.SoldierGrid.transform);
            //obj.SetActive(true);
            //HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
            //PlayerPortraitData sData = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID(soldierData[i]);
            //item.InitItem(sData);
            //_SoldHeadChangeList.Add(item);
            //客户端进行排序:
            GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.SoldierGrid.transform);
            obj.SetActive(true);
            HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
            PlayerPortraitData sData = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID(data[i]);
            item.InitItem(sData);
            _SoldHeadChangeList.Add(item);
        }
        view.BaseGrid.Reposition();
        view.SoldierGrid.Reposition();
        view.Table.Reposition();
    }
    */
    public List<uint> SortHead(List<uint> soldierData)//头像排序
    {
        List<uint> data = new List<uint>();
        data = soldierData;
        for (int i = 0; i < data.Count - 1; ++i)
        {
            for (int j = 0; j < data.Count - i - 1; ++j)
            {
                if (data[j] > data[j + 1])
                {
                    uint tmp = data[j];
                    data[j] = data[j + 1];
                    data[j + 1] = tmp;
                }
            }
        }
        return data;
    }
    #endregion
    #region 边框
    public void ButtonEvent_IconChangeButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.UIPanel_IconChangeView.gameObject.SetActive(true);
        //InitChangeView();
        SystemSettingModule.Instance.SendEnterFrameReq();
        //FillFrameTest();
        //InitFrameList(_testFrameList);
    }
    public void ButtonEvent_CloseButton(GameObject Btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        view.UIPanel_IconChangeView.gameObject.SetActive(false);
    }
    public void SetSystemPlayerIcon()//设置玩家头像框
    {
        string Frame_A = string.Format(GlobalConst.SpriteName.Frame_Name_A, PlayerData.Instance.FrameID);
        CommonFunction.SetSpriteName(view.IconFrame, Frame_A);
    }
    /*
    public void InitChangeView()
    {
        List<FrameData> data = ConfigManager.Instance.mFrameConfig.GetFrameList();
        if (_iconChangeList.Count < 1)//初始化头像框数据
        {
            for (int i = 0; i < data.Count; i++)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.ChangeIconItem, view.Grid_ChangeIcon.transform);
                obj.SetActive(true);
                IconChangeItem item = obj.AddComponent<IconChangeItem>();
                item.InitItem(data[i]);
                _iconChangeList.Add(item);
            }
        }
        else//更新头像框数据
        {
            for (int i = 0; i < data.Count; i++)
            {
                _iconChangeList[i].InitItem(data[i]);
            }
        }
        view.Grid_ChangeIcon.Reposition();
    }
    */
    #endregion
    #region 音频
    public void ButtonEvent_SoundButton(GameObject btn)//音效
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (IsOpenAudio == false)
        {//开启
            // CommonFunction.SetSpriteName(view.Spt_SoundFGSprite, GlobalConst.SpriteName.SOUND_OFF);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Audio, true);
            IsOpenAudio = true;
        }
        else
        {//关闭
            //CommonFunction.SetSpriteName(view.Spt_SoundFGSprite, GlobalConst.SpriteName.SOUND_ON);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Audio, false);
            IsOpenAudio = false;
        }
        InitAudioSound();
    }

    public void ButtonEvent_MusicButton(GameObject btn)//音乐
    {

        if (IsOpenSound == false)
        {//开启
            // CommonFunction.SetSpriteName(view.Spt_MusicFGSprite, GlobalConst.SpriteName.MUSIC_OFF);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Music, true);
            IsOpenSound = true;
        }
        else
        {
            // CommonFunction.SetSpriteName(view.Spt_MusicFGSprite, GlobalConst.SpriteName.MUSIC_ON);
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_SetMuteStatus_Music, false);
            IsOpenSound = false;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio,
            new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        InitAudioSound();
    }

    #endregion
    #region 意见
    private void UpdateFeedBackItem(NotifyReset data)
    {
        InitFeedbackBtn();
        UpdateFeedbackCoolDownTime();
    }
    public void ButtonEvent_FeedbackButton(GameObject btn)//意见反馈
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        switch (GlobalConst.PLATFORM)
        {
            case TargetPlatforms.Android_7725OL:
            case TargetPlatforms.Android_7725:
                {
                    SDKManager.Instance.ShowCustomerService();
                } break;
            default:
                {
                    InitFeedbackBtn();
                    UpdateFeedbackCoolDownTime();
                    view.UIPanel_FeedbackView.gameObject.SetActive(true);
                }break;
        }
      
    }
    public void ButtonEvent_FeedbackSendButton(GameObject btn)//发送意见反馈
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (PlayerData.Instance.FeedbackRestCount <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FEEDBACK_REST_WRONG);
            return;
        }
        if (string.IsNullOrEmpty(view.Ipt_FeedbackTitleInupt.value))
        {//标题为空
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.INPUT_FEEDBACK_TITLE_WRONG);
            return;
        }
        else if (string.IsNullOrEmpty(view.Ipt_FeedbackContentInupt.value))
        {//内容为空
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.INPUT_FEEDBACK_CONTENT_WRONG);
            return;
        }
        string Title = view.Ipt_FeedbackTitleInupt.value;
        string Content = view.Ipt_FeedbackContentInupt.value;

        SystemSettingModule.Instance.SendFeedBackRequest(Title, Content);

    }
    public void ButtonEvent_FeedbackTopCloseButton(GameObject btn)//关闭意见反馈
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        CloseFeedBack();

    }
    public void CloseFeedBack()
    {
        view.UIPanel_FeedbackView.gameObject.SetActive(false);
        view.Ipt_FeedbackTitleInupt.value = null;
        view.Ipt_FeedbackContentInupt.value = null;
        _coolDownSeconds = 0;
    }


    private void FeedbackCoolDownTimer()
    {
        if (_coolDownSeconds > 0)
        {
            _coolDownSeconds--;
            view.Lbl_FeedbackTimes.text = CommonFunction.GetTimeString(_coolDownSeconds);
            SetFeedBackSendBtn(false);
        }
        else
        {

            _coolDownSeconds = 0;
            view.Lbl_FeedbackTimes.text = " ";
            //Debug.Log(" FeedbackCoolDownTimer count down end ");
            view.Spt_FeedBackSendFGSp.gameObject.SetActive(true);
            Scheduler.Instance.RemoveTimer(FeedbackCoolDownTimer);
            SetFeedBackSendBtn(true);
        }
    }

    private void SetFeedBackSendBtn(bool state)
    {
        view.Btn_FeedbackSendButton.enabled = state;
        view.Btn_FeedbackSendButton.GetComponent<BoxCollider>().enabled = state;
        view.Spt_FeedbackSendBGSprite.color = state ? Color.white : Color.black;
    }
    public void UpdateFeedbackCoolDownTime()
    {
        if (PlayerData.Instance.FeedbackNextTime == 0)
            return;
        //System.DateTime oTime = new System.DateTime(1970, 1, 1);
        //System.TimeSpan span = System.DateTime.Now.Subtract(oTime);
        ulong totalSeconds = (ulong)Main.mTime;
        Debug.LogWarning(" totalSeconds = " + totalSeconds + "   PlayerData.Instance.FeedbackNextTime = " + PlayerData.Instance.FeedbackNextTime + " —— _coolDownSeconds" + _coolDownSeconds);
        if (PlayerData.Instance.FeedbackNextTime > totalSeconds)
        {
            _coolDownSeconds = (int)(PlayerData.Instance.FeedbackNextTime - totalSeconds);
            SetFeedBackSendBtn(false);
            view.Spt_FeedBackSendFGSp.gameObject.SetActive(false);
            view.Lbl_FeedbackTimes.text = CommonFunction.GetTimeString(_coolDownSeconds);
            Scheduler.Instance.AddTimer(1.0f, true, FeedbackCoolDownTimer);
        }
        FeedbackCoolDownTimer();
    }
    #endregion
    #region 兑换
    public void ButtonEvent_CDKeyButton(GameObject btn)//打开兑换
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

        view.UIPanel_CDKeyView.gameObject.SetActive(true);
    }
    public void ButtonEvent_CDKeyCancelButton(GameObject btn)//取消兑换
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        CloseCDKey();
    }
    public void ButtonEvent_CDKeyConfirmButton(GameObject btn)//确认兑换
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        if (string.IsNullOrEmpty(view.Ipt_CDKeyInput.value))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.INPUT_CDKEY_WRONG);
            return;
        }
        string CDKEY = view.Ipt_CDKeyInput.value;
        SystemSettingModule.Instance.SendCDKeyRequest(CDKEY);
    }
    public void CloseCDKey()
    {
        view.Ipt_CDKeyInput.value = null;
        view.UIPanel_CDKeyView.gameObject.SetActive(false);
    }
    #endregion
    #region 重登
    public void ButtonEvent_ExitButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        SystemSettingModule.Instance.SendExitRequest();
    }
    #endregion
    #region 推送
    //private const string SAVEKEY_PUSH_SP = "Push_SP";//体力
    //private const string SAVEKEY_PUSH_SLAVE = "Push_SLAVE";//奴隶
    //private const string SAVEKEY_PUSH_AGAINST = "Push_AGAINST";//招贤
    //private const string SAVEKEY_PUSH_YUANBAO = "Push_YUANBAO";//缘宝
    //private const string SAVEKEY_PUSH_SHOP = "Push_SHOP";//商店
    //private const string SAVEKEY_PUSH_ZHENBA = "Push_ZHENBA";//争霸
    //------------------------------------------------------------------
    private bool Push_SP_State;//T-1  F-0
    private bool Push_Slave_State;
    private bool Push_YuanBao_State;
    private bool Push_Against_State;
    private bool Push_Shop_State;
    private bool Puah_ZhenBa_State;
    private bool Push_GCLD_State;

    private void InitPush()//初始化界面
    {
        //CommandManager.Instance.AddSingleCommand(MessageID.Message_Push.PM_SP, CommandEvent_PushSP);
        LoadPushInfo();
        SetInitPushView();
    }
    //private void UnInitPush()//反初始化
    //{
    //    CommandManager.Instance.DelSingleCommand(MessageID.Message_Push.PM_SP, CommandEvent_PushSP);
    //}
    private void SavePushInfo()//保存用户配置
    {
        NotifySwitch data = new NotifySwitch();
        data.ph_power_switch = SetPlayerState(Push_SP_State);
        //Debug.LogError("Push_SP_State=" + Push_SP_State + "    " + data.ph_power_switch);
        data.enslave_switch = SetPlayerState(Push_Slave_State);
        //Debug.LogError("Push_Slave_State=" + Push_Slave_State + "    " + data.enslave_switch);
        data.draw_equip_switch = SetPlayerState(Push_YuanBao_State);
        //Debug.LogError("Push_YuanBao_State=" + Push_YuanBao_State + "    " + data.draw_equip_switch);
        data.recruit_switch = SetPlayerState(Push_Against_State);
        //Debug.LogError("Push_Against_State=" + Push_Against_State + "    " + data.recruit_switch);
        data.shop_fresh_switch = SetPlayerState(Push_Shop_State);
        //Debug.LogError("Push_Shop_State=" + Push_Shop_State + "    " + data.shop_fresh_switch);
        data.union_clash_switch = SetPlayerState(Puah_ZhenBa_State);
        data.campaign_switch = SetPlayerState(Push_GCLD_State);
        //Debug.LogError("Puah_ZhenBa_State=" + Puah_ZhenBa_State + "    " + data.union_clash_switch);
        SystemSettingModule.Instance.SendSetLocalNotificationRequset(data);
        ////SP：
        //if (Push_SP_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SP, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SP, 0);
        ////Slave
        //if (Push_Slave_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SLAVE, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SLAVE, 0);
        ////YuanBao
        //if (Push_YuanBao_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_YUANBAO, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_YUANBAO, 0);
        ////Against
        //if (Push_Against_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_AGAINST, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_AGAINST, 0);
        ////Shop
        //if (Push_Shop_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SHOP, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_SHOP, 0);
        ////ZhenBa
        //if (Puah_ZhenBa_State)
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_ZHENBA, 1);
        //else
        //    PlayerPrefs.SetInt(SAVEKEY_PUSH_ZHENBA, 0);
    }
    private void LoadPushInfo()//获取用户配置
    {
        Push_SP_State = SetPlayerSetting(PlayerData.Instance.PHPowerSystemPush);
        Push_Slave_State = SetPlayerSetting(PlayerData.Instance.EnSlaveSystemPush);
        Push_YuanBao_State = SetPlayerSetting(PlayerData.Instance.DrawEquipSystemPush);
        Push_Against_State = SetPlayerSetting(PlayerData.Instance.RecruitSystemPush);
        Push_Shop_State = SetPlayerSetting(PlayerData.Instance.ShopFreshSystemPush);
        Puah_ZhenBa_State = SetPlayerSetting(PlayerData.Instance.UnionClashSystemPush);
        Push_GCLD_State = SetPlayerSetting(PlayerData.Instance.GCLDSystemPush);
        //Debug.LogError("Push_SP_State=" + Push_SP_State + "      Push_Slave_State=" + Push_Slave_State + "      Push_YuanBao_State=" + Push_YuanBao_State + "      Push_Against_State=" + Push_Against_State + "    Push_Shop_State=" + Push_Shop_State + "      Puah_ZhenBa_State=" + Puah_ZhenBa_State);

        ////SP:
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_SP))//第一次初始设置
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_SP,1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_SP, 0) == 1)
        //     Push_SP_State = true;
        // else
        //     Push_SP_State = false;
        // //Slave
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_SLAVE))
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_SLAVE, 1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_SLAVE, 0) == 1)
        //     Push_Slave_State = true;
        // else
        //     Push_Slave_State = false;
        // //YuanBao
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_YUANBAO))
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_YUANBAO, 1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_YUANBAO, 0) == 1)
        //     Push_YuanBao_State = true;
        // else
        //     Push_YuanBao_State = false;
        // //Against
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_AGAINST))
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_AGAINST, 1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_AGAINST, 0) == 1)
        //     Push_Against_State = true;
        // else
        //     Push_Against_State = false;
        // //Shop
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_SHOP))
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_SHOP, 1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_SHOP, 0) == 1)
        //     Push_Shop_State = true;
        // else
        //     Push_Shop_State = false;
        // //ZhenBa
        // if (!PlayerPrefs.HasKey(SAVEKEY_PUSH_ZHENBA))
        //     PlayerPrefs.SetInt(SAVEKEY_PUSH_ZHENBA, 1);
        // if (PlayerPrefs.GetInt(SAVEKEY_PUSH_ZHENBA, 0) == 1)
        //     Puah_ZhenBa_State = true;
        // else
        //     Puah_ZhenBa_State = false;

    }
    public bool SetPlayerSetting(int ServerSetting)
    {
        if (ServerSetting == 1)
        {
            return true;
        }
        if (ServerSetting == 2)
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public int SetPlayerState(bool State)
    {

        if (State)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    private void SetInitPushView()//设置Push界面显示
    {
        SetPushViewSPState();
        SetPushViewShopState();
        SetPushViewSlaveState();
        SetPushViewZhenBaState();
        SetPushViewYuanBaoState();
        SetPushViewAgainstState();
        SetGCLDBaState();
    }
    private void SetPushViewSPState()
    {
        if (Push_SP_State)
        {
            view.Lab_SPPush.text = ConstString.PUSH_SP_ON;
            CommonFunction.SetLabelColor_I(view.Lab_SPPush, 111, 52, 14, 255, 234, 201, 93, 255);
        }
        else
        {
            view.Lab_SPPush.text = ConstString.PUSH_SP_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_SPPush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_SPPush, Push_SP_State);
    }
    private void SetPushViewSlaveState()
    {
        if (MainCityModule.Instance.LockFuncs.Contains((int)OpenFunctionType.Slave))
        {
            view.Btn_SlavePush.gameObject.SetActive(false);
            return;
        }

        if (Push_Slave_State)
        {
            view.Lab_SlavePush.text = ConstString.PUSH_SLAVE_ON;
            CommonFunction.SetLabelColor_I(view.Lab_SlavePush, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_SlavePush.text = ConstString.PUSH_SLAVE_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_SlavePush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_SlavePush, Push_Slave_State);

    }
    private void SetPushViewYuanBaoState()
    {
        if (Push_YuanBao_State)
        {
            view.Lab_YuanBaoPush.text = ConstString.PUSH_YUANBAO_ON;
            CommonFunction.SetLabelColor_I(view.Lab_YuanBaoPush, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_YuanBaoPush.text = ConstString.PUSH_YUANBAO_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_YuanBaoPush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_YuanBaoPush, Push_YuanBao_State);

    }
    private void SetPushViewAgainstState()
    {
        if (Push_Against_State)
        {
            view.Lab_AgainstPush.text = ConstString.PUSH_AGAINST_ON;
            CommonFunction.SetLabelColor_I(view.Lab_AgainstPush, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_AgainstPush.text = ConstString.PUSH_AGAINST_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_AgainstPush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_AgainstPush, Push_Against_State);


    }

    private void SetPushViewShopState()
    {
        if (Push_Shop_State)
        {
            view.Lab_ShopPush.text = ConstString.PUSH_SHOP_ON;
            CommonFunction.SetLabelColor_I(view.Lab_ShopPush, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_ShopPush.text = ConstString.PUSH_SHOP_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_ShopPush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_ShopPush, Push_Shop_State);

    }
    private void SetPushViewZhenBaState()
    {
        if (Puah_ZhenBa_State)
        {
            view.Lab_ZhenBaPush.text = ConstString.PUSH_ZHENBA_ON;
            CommonFunction.SetLabelColor_I(view.Lab_ZhenBaPush, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_ZhenBaPush.text = ConstString.PUSH_ZHENBA_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_ZhenBaPush, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_ZhenBaPush, Puah_ZhenBa_State);

    }

    private void SetGCLDBaState()
    {
        if (Push_GCLD_State)
        {
            view.Lab_GCLDFLab.text = ConstString.PUSH_GONGCHENGLVEDI_ON;
            CommonFunction.SetLabelColor_I(view.Lab_GCLDFLab, 111, 52, 14, 255, 234, 201, 93, 255);

        }
        else
        {
            view.Lab_GCLDFLab.text = ConstString.PUSH_GONGCHENGLVEDI_OFF;
            CommonFunction.SetLabelColor_I(view.Lab_GCLDFLab, 182, 101, 24, 255, 71, 37, 5, 255);
        }
        SetPushButtonState(view.Spt_GCLDBGSprite, Push_GCLD_State);

    }

    private void SetPushButtonState(UISprite Spt, bool State)
    {
        if (State)
            CommonFunction.SetSpriteName(Spt, GlobalConst.SpriteName.MUSIC_BGON);
        else
            CommonFunction.SetSpriteName(Spt, GlobalConst.SpriteName.MUSIC_BGOFF);
    }



    #region ButtonEvent
    public void ButtonEvent_PushButton(GameObject btn)//打开界面
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (GlobalConst.ISOPENGM)
            UISystem.Instance.ShowGameUI(GMView.UIName);

        //if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)//台湾版
        //{
            view.Panel_PushSetting.gameObject.SetActive(true);
            InitPush();
       // }
    }
    public void ButtonEvent_SPPushButton(GameObject Btn)
    {
        Push_SP_State = !Push_SP_State;
        SetPushViewSPState();
        //Debug.LogError("Push_SP_State = "+Push_SP_State);
    }
    public void ButtonEvent_SlavePushButton(GameObject Btn)
    {
        Push_Slave_State = !Push_Slave_State;
        SetPushViewSlaveState();
        //Debug.LogError("Push_Slave_State = " + Push_Slave_State);
    }
    public void ButtonEvent_YuanBaoPushButton(GameObject Btn)
    {
        Push_YuanBao_State = !Push_YuanBao_State;
        SetPushViewYuanBaoState();
        //Debug.LogError("Push_YuanBao_State = "+ Push_YuanBao_State);
    }
    public void ButtonEvent_AgainstPushButton(GameObject Btn)
    {
        Push_Against_State = !Push_Against_State;
        SetPushViewAgainstState();
        //Debug.LogError("Push_Against_State = " + Push_Against_State);
    }
    public void ButtonEvent_ShopPushButton(GameObject Btn)//修改为体力赠送
    {
        Push_Shop_State = !Push_Shop_State;
        SetPushViewShopState();
        //Debug.LogError("Push_Shop_State = " + Push_Shop_State);

    }
    public void ButtonEvent_ZhenBaPushButton(GameObject Btn)
    {
        Puah_ZhenBa_State = !Puah_ZhenBa_State;
        SetPushViewZhenBaState();
        //Debug.LogError("Puah_ZhenBa_State = " + Puah_ZhenBa_State);

    }

    public void ButtonEvent_GCLDButton(GameObject Btn)
    {
        Push_GCLD_State = !Push_GCLD_State;
        SetGCLDBaState();
        //Debug.LogError("Puah_ZhenBa_State = " + Puah_ZhenBa_State);

    }

    public void ButtonEvent_QueRenPushButton(GameObject Btn)//保存设置
    {
        SavePushInfo();
        view.Panel_PushSetting.gameObject.SetActive(false);
    }

    public void ButtonEvent_CloseBtn(GameObject Btn)//关闭界面
    {
        view.Panel_PushSetting.gameObject.SetActive(false);
    }
    #endregion
    //#region CommandEvent
    //private void CommandEvent_PushSP(object vDataObj)
    //{

    //}
    //#endregion
    #endregion
    #region 屏蔽
    public void ButtonEvent_BlockButton(GameObject btn)//OpenView
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.UIPanel_RemoveBlockView.SetActive(true);
        SystemSettingModule.Instance.SendBlockItemRequest();
    }
    public void ButtonEvent_RemoveBlockTopCloseButton(GameObject btn)//CloseView
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        CloseRemoveBlockView();
    }
    public void CloseRemoveBlockView()
    {
        CloseRemoveBlock();
        view.UIPanel_RemoveBlockView.SetActive(false);
    }
    public void InitRemoveBlockItem(List<BlockedInfos> block)//Instantiate
    {
        if (block.Count == 0)
        {
            view.NoBlockLabel.gameObject.SetActive(true);
        }
        else
        {
            view.NoBlockLabel.gameObject.SetActive(false);
            for (int i = 0; i < block.Count; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Go_RemoveBlockItem, view.RemoveBlockGrid.transform);
                go.SetActive(true);
                RemoveBlockItem item = go.AddComponent<RemoveBlockItem>();
                item.InitItem(block[i]);
                _RemoveBlockList.Add(item);
            }
        }
        view.RemoveBlockGrid.Reposition();
    }
    public void CloseRemoveBlock()//Destory
    {
        for (int i = _RemoveBlockList.Count - 1; i >= 0; i--)
        {
            RemoveBlockItem item = _RemoveBlockList[i];
            _RemoveBlockList.Remove(item);
            GameObject.Destroy(item.gameObject);
        }
    }
    #endregion
    #region 绑定
    public void ButtonEvent_BindingButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        //if (GlobalConst.ISOPENSDKOPERATE)
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)//台湾版
            {
                if (SDKManager.Instance.IsBindingPlatform() == 0)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SYSTEM_BINDINGACCOUNT);
                }
                else
                {
                    string desc = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_IDPLAYBIND_AWARDDESC);
                    SDKManager.Instance.OpenTouristBinding(desc);
                }
                return;
            }
        }
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, "暂无功能");
    }
    public void ButtonEvent_BindingCancelButton(GameObject btn)
    {

    }
    public void ButtonEvent_BindingConfirmButton(GameObject btn)
    {

    }
    public void ButtonEvent_PhoneNumCancelButton(GameObject btn)
    {

    }
    public void ButtonEvent_PhoneNumConfirmButton(GameObject btn)
    {

    }
    public void ButtonEvent_ValidationCancelButton(GameObject btn)
    {

    }
    public void ButtonEvent_ValidationConfirmButton(GameObject btn)
    {

    }
    public void ButtonEvent_GetValidationCodeButton(GameObject btn)
    {

    }


    #endregion
    #region 改名
    public void ButtonEvent_RenameButton(GameObject btn)//OpenView
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        ShowChangeNameView();
    }
    public void ButtonEvent_InputNameCancelButton(GameObject btn)//CloseView
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        HideChangeNameView();
    }
    public void ButtonEvent_InputNameConfirmButton(GameObject btn)//确认改名
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!LoginModule.Instance.CheckNameRule(view.Ipt_NameInput.value))
            return;

        string name = view.Ipt_NameInput.value;
        if (name.Equals(PlayerData.Instance._NickName))
        {
            HideChangeNameView();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NAME_WRONG);
            return;
        }
        view.Lbl_NewNameLabel.text = string.Format(ConstString.INPUT_NAME_CONFIRM, view.Ipt_NameInput.value);
        view.GameObj_InputNameObj.SetActive(false);
        view.GameObj_ConfirmChangeNameObj.SetActive(true);

    }
    public void ButtonEvent_ChangeNameYesButton(GameObject btn)//确认改名->是
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        //if (PlayerData.Instance._Diamonds < GlobalConst.RENAME_CHAR_PRICE)
        //{
        //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_DIAMOND_AND_BUY);
        //    return;
        //}
        string name = view.Ipt_NameInput.value;

        SystemSettingModule.Instance.SendChangeNameRequest(name);
    }
    public void ButtonEvent_ChangeNameNoButton(GameObject btn) //确认改名->否
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        view.GameObj_InputNameObj.SetActive(true);
        view.GameObj_ConfirmChangeNameObj.SetActive(false);

    }
    public void ShowChangeNameView()//显示修改姓名界面
    {
        if (PlayerData.Instance._FreeRenameNum == 0)//没免费次数
        {
            view.Lab_ChangeFreeName.gameObject.SetActive(false);
            view.Lbl_CostNumLabel.gameObject.SetActive(true);
            view.Spt_CostSprite.gameObject.SetActive(true);
            view.Lbl_CostLabel.gameObject.SetActive(true);

            view.UIPanel_ChangeNameView.gameObject.SetActive(true);
            view.GameObj_InputNameObj.gameObject.SetActive(true);
            view.GameObj_ConfirmChangeNameObj.gameObject.SetActive(false);
            view.Lbl_CostNumLabel.text = GlobalConst.RENAME_CHAR_PRICE.ToString();//改名消耗金币
        }
        else
        {
            view.Lab_ChangeFreeName.gameObject.SetActive(true);
            view.Lab_ChangeFreeName.text = string.Format(ConstString.SYSTEMCHANGENAME, PlayerData.Instance._FreeRenameNum, 1);
            view.Lbl_CostNumLabel.gameObject.SetActive(false);
            view.Spt_CostSprite.gameObject.SetActive(false);
            view.Lbl_CostLabel.gameObject.SetActive(false);

            view.UIPanel_ChangeNameView.gameObject.SetActive(true);
            view.GameObj_InputNameObj.gameObject.SetActive(true);
            view.GameObj_ConfirmChangeNameObj.gameObject.SetActive(false);
        }
    }
    public void HideChangeNameView()//隐藏修改姓名界面
    {
        view.UIPanel_ChangeNameView.gameObject.SetActive(false);
        view.GameObj_InputNameObj.gameObject.SetActive(false);
        view.GameObj_ConfirmChangeNameObj.gameObject.SetActive(false);
        view.Ipt_NameInput.value = "";

    }
    public void ButtonEvent_RandomNameButton(GameObject btn)//随机改名
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        SystemSettingModule.Instance.SendRandomName();
    }

    public void OnUpdateNickName(string name)
    {
        view.Ipt_NameInput.value = name;
    }
    #endregion
    #region 绑定事件
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SPPush.gameObject).onClick = ButtonEvent_SPPushButton;
        UIEventListener.Get(view.Btn_SlavePush.gameObject).onClick = ButtonEvent_SlavePushButton;
        UIEventListener.Get(view.Btn_YuanBaoPush.gameObject).onClick = ButtonEvent_YuanBaoPushButton;
        UIEventListener.Get(view.Btn_AgainstPush.gameObject).onClick = ButtonEvent_AgainstPushButton;
        UIEventListener.Get(view.Btn_ShopPush.gameObject).onClick = ButtonEvent_ShopPushButton;
        UIEventListener.Get(view.Btn_ZhenBaPush.gameObject).onClick = ButtonEvent_ZhenBaPushButton;
        UIEventListener.Get(view.Btn_GCLDPush.gameObject).onClick = ButtonEvent_GCLDButton;
        UIEventListener.Get(view.Btn_QueRenPush.gameObject).onClick = ButtonEvent_QueRenPushButton;
        UIEventListener.Get(view.Btn_PushCloseBtn.gameObject).onClick = ButtonEvent_CloseBtn;

        UIEventListener.Get(view.Btn_HeadChangeButton.gameObject).onClick = ButtonEvent_HeadChangeButton;
        UIEventListener.Get(view.CloseHeadChange.gameObject).onClick = ButtonEvent_CloseHeadChangeButton;
        UIEventListener.Get(view.Btn_IconChangeButton.gameObject).onClick = ButtonEvent_IconChangeButton;
        UIEventListener.Get(view.Btn_CloseIconChange.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_SettingTopCloseBtn.gameObject).onClick = ButtonEvent_SettingTopCloseBtn;
        UIEventListener.Get(view.Btn_RenameButton.gameObject).onClick = ButtonEvent_RenameButton;
        UIEventListener.Get(view.Btn_SoundButton.gameObject).onClick = ButtonEvent_SoundButton;
        UIEventListener.Get(view.Btn_MusicButton.gameObject).onClick = ButtonEvent_MusicButton;
        UIEventListener.Get(view.Btn_PushButton.gameObject).onClick = ButtonEvent_PushButton;
        UIEventListener.Get(view.Btn_CDKeyButton.gameObject).onClick = ButtonEvent_CDKeyButton;
        UIEventListener.Get(view.Btn_BlockButton.gameObject).onClick = ButtonEvent_BlockButton;
        UIEventListener.Get(view.Btn_FeedbackButton.gameObject).onClick = ButtonEvent_FeedbackButton;
        UIEventListener.Get(view.Btn_ExitButton.gameObject).onClick = ButtonEvent_ExitButton;
        UIEventListener.Get(view.Btn_BindingButton.gameObject).onClick = ButtonEvent_BindingButton;
        UIEventListener.Get(view.CloseRemoveBlockButton.gameObject).onClick = ButtonEvent_RemoveBlockTopCloseButton;
        UIEventListener.Get(view.Btn_FeedbackSendButton.gameObject).onClick = ButtonEvent_FeedbackSendButton;
        UIEventListener.Get(view.Btn_FeedbackTopCloseButton.gameObject).onClick = ButtonEvent_FeedbackTopCloseButton;
        UIEventListener.Get(view.Btn_RandomButton.gameObject).onClick = ButtonEvent_RandomNameButton;
        UIEventListener.Get(view.Btn_InputNameCancelButton.gameObject).onClick = ButtonEvent_InputNameCancelButton;
        UIEventListener.Get(view.Btn_InputNameConfirmButton.gameObject).onClick = ButtonEvent_InputNameConfirmButton;
        UIEventListener.Get(view.Btn_ChangeNameNoButton.gameObject).onClick = ButtonEvent_ChangeNameNoButton;
        UIEventListener.Get(view.Btn_ChangeNameYseButton.gameObject).onClick = ButtonEvent_ChangeNameYesButton;
        UIEventListener.Get(view.Btn_CDKeyCancelButton.gameObject).onClick = ButtonEvent_CDKeyCancelButton;
        UIEventListener.Get(view.Btn_CDKeyConfirmButton.gameObject).onClick = ButtonEvent_CDKeyConfirmButton;
        UIEventListener.Get(view.Btn_PhoneNumCancelButton.gameObject).onClick = ButtonEvent_PhoneNumCancelButton;
        UIEventListener.Get(view.Btn_PhoneNumConfirmButton.gameObject).onClick = ButtonEvent_PhoneNumConfirmButton;
        UIEventListener.Get(view.Btn_BindingCancelButton.gameObject).onClick = ButtonEvent_BindingCancelButton;
        UIEventListener.Get(view.Btn_BindingConfirmButton.gameObject).onClick = ButtonEvent_BindingConfirmButton;
        UIEventListener.Get(view.Btn_ValidationCancelButton.gameObject).onClick = ButtonEvent_ValidationCancelButton;
        UIEventListener.Get(view.Btn_ValidationConfirmButton.gameObject).onClick = ButtonEvent_ValidationConfirmButton;
        UIEventListener.Get(view.Btn_GetValidationCodeButton.gameObject).onClick = ButtonEvent_GetValidationCodeButton;

        UIEventListener.Get(view.Btn_ChangeButton.gameObject).onClick = ButtonEvent_ChangeButton;
        view.UIWrapContent_UIGrid.onInitializeItem = SetFrameInfo;

    }
    #endregion
    public override void Destroy()
    {
        // UnInitPush();
        view = null;
        UnInitFrameList();
        UnInitIconInformationList();
    }
    public override void Uninitialize()
    {
        PlayerData.Instance.NotifyResetEvent -= UpdateFeedBackItem;
    }
    public void ButtonEvent_SettingTopCloseBtn(GameObject btn)//CloseSettingView
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        UISystem.Instance.CloseGameUI(SystemSettingView.UIName);
    }
    //----------------------------------------frame--------------------------------------//
    /// <summary>
    /// 获取头像框资源
    /// </summary>
    private void LoadFrameRes()
    {
        if (_frameItem == null)
            _frameItem = view.ChangeIconItem;
        if (_frameItem == null) Debug.LogError("Can't find FameItem");
        _frameItem.SetActive(false);
    }
    /// <summary>
    /// 初始化头像框列表
    /// </summary>
    public void InitFrameList(List<FrameInfo> data)
    {
        InitAndClearFrameLists();
        OrderFrameLists(data);
        Main.Instance.StartCoroutine(ShowFrameList());
    }
    /// <summary>
    /// 初始化相关列表
    /// </summary>
    private void InitAndClearFrameLists()
    {
        if (tmpFrameList == null)
            tmpFrameList = ConfigManager.Instance.mFrameConfig.GetFrameList();

        if (_fUnlockFrameList == null)
        {
            _fUnlockFrameList = new List<FrameInfo>();
        }
        else
        {
            _fUnlockFrameList.Clear();
        }
        if (_tUnlockFrameList == null)
        {
            _tUnlockFrameList = new List<FrameInfo>();
        }
        else
        {
            _tUnlockFrameList.Clear();
        }
        if (_LockedFrameList == null)
        {
            _LockedFrameList = new List<FrameInfo>();
        }
        else
        {
            _LockedFrameList.Clear();
        }
        if (_totalFrameList == null)
        {
            _totalFrameList = new List<FrameInfo>();
        }
        else
        {
            _totalFrameList.Clear();
        }
    }
    /// <summary>
    /// 逆始化相关列表
    /// </summary>
    private void UnInitFrameList()
    {
        foreach(IconChangeItem item in _iconChangeList)
        {
            item.StopTimeUpdate();
        }

        if (_fUnlockFrameList != null)
        {
            _fUnlockFrameList.Clear();
            _fUnlockFrameList = null;
        }
        if (_tUnlockFrameList != null)
        {
            _tUnlockFrameList.Clear();
            _tUnlockFrameList = null;
        }
        if (_LockedFrameList != null)
        {
            _LockedFrameList.Clear();
            _LockedFrameList = null;
        }
        if (_totalFrameList != null)
        {
            _totalFrameList.Clear();
            _totalFrameList = null;
        }
        //if (tmpFrameList != null)
        //{
        //    tmpFrameList.Clear();
        //    tmpFrameList = null;
        //}
    }
    /// <summary>
    /// 头像框列表排序
    /// </summary>
    private void OrderFrameLists(List<FrameInfo> data)
    {
        /*原本方案
        foreach (FrameInfo info in data)
        {
            switch (info.status)
            {
                case 0:
                    _LockedFrameList.Add(info);
                    break;
                case 1:
                    if (info.resettime == 0)
                    {
                        _fUnlockFrameList.Add(info);
                    }
                    else
                    {
                        _tUnlockFrameList.Add(info);
                    }
                    break;
            }
        }
        _totalFrameList.AddRange(_fUnlockFrameList.OrderBy(s => s.id).ToList());
        _totalFrameList.AddRange(_tUnlockFrameList.OrderByDescending(s => s.resettime).ToList());
        _totalFrameList.AddRange(_LockedFrameList.OrderBy(s => s.id).ToList());
        */

        //新方案
        for(int i = 0; i < tmpFrameList.Count; i++)
        {
            FrameInfo tmpData = new FrameInfo();
            if (data.Find(s => s.id == tmpFrameList[i].id) == null)
            {              
                tmpData.id = (int)tmpFrameList[i].id;
                tmpData.resettime = 0;
                tmpData.status = 0;
                _LockedFrameList.Add(tmpData);
            }
            else
            {
                tmpData = data.Find(s => s.id == tmpFrameList[i].id);
                tmpData.status = 1;
                if (tmpData.resettime > 0)
                {
                    _tUnlockFrameList.Add(tmpData);
                }
                else
                {
                    _fUnlockFrameList.Add(tmpData);
                }
                //Debug.LogError("id:" + tmpData.id + "  status:" + tmpData.status + "  resettime:" + tmpData.resettime);

            }
        }

        _totalFrameList.AddRange(_fUnlockFrameList.OrderBy(s => s.id).ToList());
        _totalFrameList.AddRange(_tUnlockFrameList.OrderByDescending(s => s.resettime).ToList());
        _totalFrameList.AddRange(_LockedFrameList.OrderBy(s => s.id).ToList());
    }
    /// <summary>
    /// 获取并展示头像框信息
    /// </summary>
    /// <returns></returns>
    private IEnumerator ShowFrameList()
    {
        int count = _totalFrameList.Count;
        int itemCount = _iconChangeList.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_UIGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                _iconChangeList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(_frameItem, view.Grid_ChangeIcon.transform);
                IconChangeItem item = vGo.GetComponent<IconChangeItem>();
                vGo.SetActive(true);
                _iconChangeList.Insert(i, item);
                vGo.name = i.ToString();
            }
            else
            {
                _iconChangeList[i].gameObject.SetActive(true);
            }
            _iconChangeList[i].InitItem(_totalFrameList[i]);
        }
        view.UIWrapContent_UIGrid.ReGetChild();
        yield return 0;
        view.Grid_ChangeIcon.repositionNow = true;
        yield return 0;
        view.ScrView_FrameGroup.ResetPosition();
        view.Grid_ChangeIcon.repositionNow = true;
    }
    /// <summary>
    /// 更新成就条目信息
    /// </summary>
    public void SetFrameInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _totalFrameList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        IconChangeItem item = _iconChangeList[wrapIndex];
        item.InitItem(_totalFrameList[realIndex]);
    }
    //--------------------------------------icon-----------------------------------------//
    /// <summary>
    /// 初始化头像信息列表
    /// </summary>
    public void InitIconList(List<IconInformation> data)
    {
        InitAndClearIconLists();
        OrderIconLists(data);
        Main.Instance.StartCoroutine(InitAndShowIcons());
    }
    /// <summary>
    /// 初始化相关列表
    /// </summary>
    private void InitAndClearIconLists()
    {
        if (tmpIconList == null)
            tmpIconList = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitList();
        if (_basicIconList == null)
        {
            _basicIconList = new List<IconInformation>();
        }
        else
        {
            _basicIconList.Clear();
        }
        if (_unlockAchievementIconList == null)
        {
            _unlockAchievementIconList = new List<IconInformation>();
        }
        else
        {
            _unlockAchievementIconList.Clear();
        }
        if (_lockAchievementIconList == null)
        {
            _lockAchievementIconList = new List<IconInformation>();
        }
        else
        {
            _lockAchievementIconList.Clear();
        }
        if (_unlockSoldierIconList == null)
        {
            _unlockSoldierIconList = new List<IconInformation>();
        }
        else
        {
            _unlockSoldierIconList.Clear();
        }
        if (_lockSoldierIconList == null)
        {
            _lockSoldierIconList = new List<IconInformation>();
        }
        else
        {
            _lockSoldierIconList.Clear();
        }
        InitIconItemList();
    }
    /// <summary>
    /// 头像列表排序
    /// </summary>
    private void OrderIconLists(List<IconInformation> data)
    {
        /*原本方案
        foreach (IconInformation info in data)
        {
            if (info.type == 1)
            {
                _basicIconList.Add(info);
            }
            else if (info.type == 2)
            {
                if (info.status == 0)
                {
                    _lockSoldierIconList.Add(info);
                }
                else if (info.status == 1)
                {
                    _unlockSoldierIconList.Add(info);
                }
                else
                {
                    Debug.LogError("soldier icon status error");
                }
            }
            else if (info.type == 3)
            {
                if (info.status == 0)
                {
                    _lockAchievementIconList.Add(info);
                }
                else if (info.status == 1)
                {
                    _unlockAchievementIconList.Add(info);
                }
                else
                {
                    Debug.LogError("achievement icon status error");
                }
            }
            else
            {
                Debug.LogError("icon type error");
            }
        }

        _basicIconList.OrderBy(s => s.id);
        _unlockAchievementIconList.OrderBy(s => s.id);
        _lockAchievementIconList.OrderBy(s => s.id);
        _unlockSoldierIconList.OrderBy(s => s.id);
        _lockSoldierIconList.OrderBy(s => s.id);
        */

        //新方案
        for(int i = 0; i < tmpIconList.Count; i++)
        {
            IconInformation tmpData = new IconInformation();
            if (tmpIconList[i].type == 1)
            {
                tmpData.id = (int)tmpIconList[i].id;
                tmpData.status = 1;
                tmpData.resettime = 0;
                tmpData.type = 1;
                _basicIconList.Add(tmpData);
            }
            else
            {
                if (data.Find(s => s.id == tmpIconList[i].id) == null)
                {
                    tmpData.status = 0;
                    tmpData.id = (int)tmpIconList[i].id;
                    tmpData.resettime = 0;
                    if (tmpIconList[i].type == 2)
                    {
                        tmpData.type = 2;                     
                        _lockSoldierIconList.Add(tmpData);
                    }
                    else if (tmpIconList[i].type == 3)
                    {
                        tmpData.type = 3;
                        _lockAchievementIconList.Add(tmpData);
                    }
                    else
                    {
                        Debug.LogError("tmpIconList error  " + tmpIconList[i].type);
                    }
                }
                else
                {
                    tmpData = data.Find(s => s.id == tmpIconList[i].id);
                    tmpData.status = 1;
                    if (tmpIconList[i].type == 2)
                    {
                        tmpData.type = 2;
                        _unlockSoldierIconList.Add(tmpData);
                    }
                    else if (tmpIconList[i].type == 3)
                    {
                        tmpData.type = 3;
                        _unlockAchievementIconList.Add(tmpData);
                    }
                    else
                    {
                        Debug.LogError("tmpIconList error  " + tmpIconList[i].type);
                    }
                }
            }           
        }

        _basicIconList.OrderBy(s => s.id);
        _unlockAchievementIconList.OrderBy(s => s.id);
        _lockAchievementIconList.OrderBy(s => s.id);
        _unlockSoldierIconList.OrderBy(s => s.id);
        _lockSoldierIconList.OrderBy(s => s.id);
    }
    /// <summary>
    /// 初始化头像图标列表
    /// </summary>
    private void InitIconItemList()
    {
        if (_basicIconItemList == null)
        {
            _basicIconItemList = new List<HeadChangeItem>();
        }
        else
        {
            _basicIconItemList.Clear();
        }
        if (_unlockAchievementIconItemList == null)
        {
            _unlockAchievementIconItemList = new List<HeadChangeItem>();
        }
        else
        {
            _unlockAchievementIconItemList.Clear();
        }
        if (_lockAchievementIconItemList == null)
        {
            _lockAchievementIconItemList = new List<HeadChangeItem>();
        }
        else
        {
            _lockAchievementIconItemList.Clear();
        }
        if (_unlockSoldierIconItemList == null)
        {
            _unlockSoldierIconItemList = new List<HeadChangeItem>();
        }
        else
        {
            _unlockSoldierIconItemList.Clear();
        }
        if (_lockSoldierIconItemList == null)
        {
            _lockSoldierIconItemList = new List<HeadChangeItem>();
        }
        else
        {
            _lockSoldierIconItemList.Clear();
        }
    }
    /// <summary>
    /// 清空头像图标列表
    /// </summary>
    private void UnInitIconItemList()
    {
        if (_basicIconItemList != null)
        {
            _basicIconItemList.Clear();
            _basicIconItemList = null;
        }
        if (_unlockAchievementIconItemList != null)
        {
            _unlockAchievementIconItemList.Clear();
            _unlockAchievementIconItemList = null;
        }
        if (_lockAchievementIconItemList != null)
        {
            _lockAchievementIconItemList.Clear();
            _lockAchievementIconItemList = null;
        }
        if (_unlockSoldierIconItemList != null)
        {
            _unlockSoldierIconItemList.Clear();
            _unlockSoldierIconItemList = null;
        }
        if (_lockSoldierIconItemList != null)
        {
            _lockSoldierIconItemList.Clear();
            _lockSoldierIconItemList = null;
        }

    }
    /// <summary>
    /// 生成并显示头像
    /// </summary>
    /// <returns></returns>
    private IEnumerator InitAndShowIcons()
    {
        if (_basicIconItemList.Count < 1)
        {
            for (int i = 0; i < _basicIconList.Count; i++)
            {
                    GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.BaseGrid.transform);
                    obj.SetActive(true);
                    HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                    item.InitItem(_basicIconList[i]);
                    _basicIconItemList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < _basicIconList.Count; i++)
            {
                _basicIconItemList[i].InitItem(_basicIconList[i]);
            }
        }
        yield return 0;
        view.BaseGrid.Reposition();
        yield return 0;
        view.Table.Reposition();

        if (_unlockAchievementIconItemList.Count < 1)
        {
            for (int i = 0; i < _unlockAchievementIconList.Count; i++)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.UAchievementGrid.transform);
                obj.SetActive(true);
                HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                item.InitItem(_unlockAchievementIconList[i]);
                _unlockAchievementIconItemList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < _unlockAchievementIconList.Count; i++)
            {
                _unlockAchievementIconItemList[i].InitItem(_unlockAchievementIconList[i]);
            }
        }
        yield return 0;
        view.UAchievementGrid.Reposition();
        yield return 0;
        view.Table.Reposition();

        if (_unlockSoldierIconItemList.Count < 1)
        {
            for (int i = 0; i < _unlockSoldierIconList.Count; i++)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.USoldierGrid.transform);
                obj.SetActive(true);
                HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                item.InitItem(_unlockSoldierIconList[i]);
                _unlockSoldierIconItemList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < _unlockSoldierIconList.Count; i++)
            {
                _unlockSoldierIconItemList[i].InitItem(_unlockSoldierIconList[i]);
            }
        }
        yield return 0;
        view.USoldierGrid.Reposition();
        yield return 0;
        view.Table.Reposition();

        if (_lockAchievementIconItemList.Count < 1)
        {
            for (int i = 0; i < _lockAchievementIconList.Count; i++)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.LAchievementGrid.transform);
                obj.SetActive(true);
                HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                item.InitItem(_lockAchievementIconList[i]);
                _lockAchievementIconItemList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < _lockAchievementIconList.Count; i++)
            {
                _lockAchievementIconItemList[i].InitItem(_lockAchievementIconList[i]);
            }
        }
        yield return 0;
        view.LAchievementGrid.Reposition();
        yield return 0;
        view.Table.Reposition();

        if (_lockSoldierIconItemList.Count < 1)
        {
            for (int i = 0; i < _lockSoldierIconList.Count; i++)
            {
                GameObject obj = CommonFunction.InstantiateObject(view.ChangeHeadItem, view.LSoldierGrid.transform);
                obj.SetActive(true);
                HeadChangeItem item = obj.AddComponent<HeadChangeItem>();
                item.InitItem(_lockSoldierIconList[i]);
                _lockSoldierIconItemList.Add(item);
            }
        }
        else
        {
            for (int i = 0; i < _lockSoldierIconList.Count; i++)
            {
                _lockSoldierIconItemList[i].InitItem(_lockSoldierIconList[i]);
            }
        }
        yield return 0;
        view.LSoldierGrid.Reposition();
        yield return 0;
        view.Table.Reposition();

        _SoldHeadChangeList.AddRange(_basicIconItemList);
        _SoldHeadChangeList.AddRange(_unlockAchievementIconItemList);
        _SoldHeadChangeList.AddRange(_unlockSoldierIconItemList);
        _SoldHeadChangeList.AddRange(_lockAchievementIconItemList);
        _SoldHeadChangeList.AddRange(_lockSoldierIconItemList);

    }
    /// <summary>
    /// 逆初始化头像信息列表
    /// </summary>
    private void UnInitIconInformationList()
    {
        //foreach (HeadChangeItem item in _unlockAchievementIconItemList)
        //{
        //    item.StopTimeUpdate();
        //}
        if (_basicIconList != null)
        {
            _basicIconList.Clear();
            _basicIconList = null;
        }
        if (_unlockAchievementIconList != null)
        {
            _unlockAchievementIconList.Clear();
            _unlockAchievementIconList = null;
        }
        if (_lockAchievementIconList != null)
        {
            _lockAchievementIconList.Clear();
            _lockAchievementIconList = null;
        }
        if (_unlockSoldierIconList != null)
        {
            _unlockSoldierIconList.Clear();
            _unlockSoldierIconList = null;
        }
        if (_lockSoldierIconList != null)
        {
            _lockSoldierIconList.Clear();
            _lockSoldierIconList = null;
        }
        //if (tmpIconList != null)
        //{
        //    tmpIconList.Clear();
        //    tmpIconList = null;
        //}
    }
    /// <summary>
    ///清除上一个选中的头像
    /// </summary>
    public void UnSelectPreIcon()
    {
        if (ConfigManager.Instance.mPlayerPortraitConfig.IsIdCorrect(selIconId))
            _SoldHeadChangeList.Find((s) => {return s.itemID == selIconId; }).Spt_Selected.gameObject.SetActive(false);
        changeId = 0;
    }
    /// <summary>
    /// 计算位置并打开头像说明
    /// </summary>
    public void OpenDescSelected(int quadrant,Vector3 pos,float hw,float hh)
    {
        float halfheight = view.Lbl_DescContent.height / 2f;
        view.Gobj_Desc.transform.position = pos;
        Vector3 position = view.Gobj_Desc.transform.localPosition;

        if (quadrant == 1)
        {
            position.x -= (view.Lbl_DescContent.width + hw);
            position.y -= (halfheight + hh);
            view.Gobj_Desc.transform.localPosition = position;
        }
        else if (quadrant == 2)
        {
            position.x += hw;
            position.y -= (halfheight + hh);
            view.Gobj_Desc.transform.localPosition = position;
        }
        else if (quadrant == 3)
        {
            position.x += hw;
            position.y += (halfheight + hh);
            view.Gobj_Desc.transform.localPosition = position;
        }
        else
        {
            position.x -= (view.Lbl_DescContent.width + hw);
            position.y += (halfheight + hh);
            view.Gobj_Desc.transform.localPosition = position;
        }
        view.Gobj_Desc.SetActive(true);
    }
    /// <summary>
    /// 设置头像说明框文本
    /// </summary>
    /// <param name="status"></param>
    public void SetDescLabels(int status,string countTime)
    {
        PlayerPortraitData data = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID(selDescId);
        if (data != null)
        {
            string str = string.Format(ConstString.ACHIEVEMENT_ICON_NAME, brown, white, data.name);

            str += '\n';
            if (data.type == 1)
            {
                str += string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
            }
            else if (data.type == 3)
            {
                if (status == 0)
                {
                    if (data.const_time != 0)
                    {
                        string tmp = string.Format(ConstString.ACHIEVEMENT_CONSTTIME, data.const_time / 3600);
                        str += string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, tmp);

                    }
                    else
                        str += string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                }
                else if (status == 1)
                {
                    if(data.const_time!=0)
                        str += string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, countTime);
                    else str+= string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
                }
                else
                {
                    Debug.LogError("icon status error");
                }
            }
            else if (data.type == 2)
            {
                str += string.Format(ConstString.ACHIEVEMENT_FRAME_TIME, brown, white, ConstString.ACHIEVEMENT_FRAME_FOREVERTIME);
            }
            else
            {
                Debug.LogError("icon type error");
            }

            if (data.effect_desc != null && data.effect_desc != "" && data.effect_desc != "0")
            {
                str += '\n';
                str += brown + string.Format(data.effect_desc, SetEffectLabel(data));
            }

            if (data.unlock_desc != null && data.unlock_desc != "" && data.unlock_desc != "0")
            {
                str += '\n' + green;
                //str += data.unlock_desc;
                if (status == 0)
                {
                    if (data.type == 2)
                    {
                        str += string.Format(ConfigManager.Instance.mPlayerPortraitConfig.GetUnlockDescByID(selIconId),
                            ConfigManager.Instance.mSoldierData.FindById(ConfigManager.Instance.mPlayerPortraitConfig.GetSoldierId(selDescId)).Name);
                    }
                    if (data.type == 3)
                    {
                        str += string.Format(ConfigManager.Instance.mPlayerPortraitConfig.GetUnlockDescByID(selIconId),
                            ConfigManager.Instance.mAchievementConfig.GetNameByID((int)ConfigManager.Instance.mPlayerPortraitConfig.GetSoldierId(selDescId)));
                    }
                }
                else if (status == 1)
                {
                    str += ConstString.ACHIEVEMENT_ICONUNLOCK;
                }
                else Debug.LogError("status error");
            }

            view.Lbl_DescContent.text = CommonFunction.ReplaceEscapeChar(str);
        }
        else
        {
            Debug.LogError("can't get icon desc");
        }
    }
    /// <summary>
    /// 效果描述变量
    /// </summary>
    private string[] SetEffectLabel(PlayerPortraitData data)
    {
        List<string> strList = new List<string>();
        if (data.hp != 0)
        {
            strList.Add(white + data.hp.ToString() + brown);
        }
        if (data.attack != 0)
        {
            strList.Add(white + data.attack.ToString() + brown);
        }
        if (data.hit != 0)
        {
            strList.Add(white + data.hit.ToString() + brown);
        }
        if (data.dodge != 0)
        {
            strList.Add(white + data.dodge.ToString() + brown);
        }
        if (data.crit != 0)
        {
            strList.Add(white + data.crit.ToString() + brown);
        }
        if (data.crit_def != 0)
        {
            strList.Add(white + data.crit_def.ToString() + brown);
        }
        if (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill) != null && (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name != "")
        {
            strList.Add(white + (ConfigManager.Instance.mSkillAttData.FindById((uint)data.skill)).Name + brown);
        }

        if (strList.Count == 0)
            Debug.LogWarning("icon effect label error");

        string[] items = new string[strList.Count];
        for (int i = 0; i < strList.Count; i++)
        {
            items[i] = strList[i];
        }
        return items;
    }





    /*
    //----------------------------------------test----------------------------------------//
    List<FrameInfo> _testFrameList = new List<FrameInfo>();
    private void FillFrameTest()
    {
        if (_testFrameList == null)
        {
            _testFrameList = new List<FrameInfo>();
        }
        else
            _testFrameList.Clear();

        FrameInfo test1 = new FrameInfo();
        test1.id = 10001;
        test1.resettime = 0;
        _testFrameList.Add(test1);

        FrameInfo test2 = new FrameInfo();
        test2.id = 20001;
        test2.resettime = 0;
        _testFrameList.Add(test2);

        FrameInfo test3 = new FrameInfo();
        test3.id = 20002;
        test3.resettime = 0;
        _testFrameList.Add(test3);

        //FrameInfo test4 = new FrameInfo();
        //test4.id = 3200004;
        //test4.resettime = (int)Main.mTime + 20;
        //_testFrameList.Add(test4);

        FrameInfo test5 = new FrameInfo();
        test5.id = 3200005;
        test5.resettime = (int)Main.mTime + 40;
        _testFrameList.Add(test5);

        FrameInfo test6 = new FrameInfo();
        test6.id = 3200008;
        test6.resettime = 0;
        _testFrameList.Add(test6);

        FrameInfo test7 = new FrameInfo();
        test7.id = 3200001;
        test7.resettime =0;
        _testFrameList.Add(test7);

        FrameInfo test8 = new FrameInfo();
        test8.id = 3200002;
        test8.resettime = 0;
        _testFrameList.Add(test8);

        FrameInfo test9 = new FrameInfo();
        test9.id = 3200003;
        test9.resettime = 0;
        _testFrameList.Add(test9);

        FrameInfo test10 = new FrameInfo();
        test10.id = 3200010;
        test10.resettime = 0;
        _testFrameList.Add(test10);
    }
    List<IconInformation> _testIconList = new List<IconInformation>();
    private void FillIconTest()
    {
        if (_testIconList == null)
        {
            _testIconList = new List<IconInformation>();
        }
        else
            _testIconList.Clear();

        IconInformation test1 = new IconInformation();
        test1.id = 20003;
        test1.resettime = 0;
        _testIconList.Add(test1);

        IconInformation test2 = new IconInformation();
        test2.id = 20001;
        test2.resettime = 0;
        _testIconList.Add(test2);

        IconInformation test3 = new IconInformation();
        test3.id = 20002;
        test3.resettime = 0;
        _testIconList.Add(test3);

        IconInformation test4 = new IconInformation();
        test4.id = 20035;
        test4.resettime = 0;
        _testIconList.Add(test4);

        IconInformation test5 = new IconInformation();
        test5.id = 20039;
        test5.resettime = 0;
        _testIconList.Add(test5);

        IconInformation test6 = new IconInformation();
        test6.id = 2300012;
        test6.resettime = (int)Main.mTime + 12;
        _testIconList.Add(test6);

        IconInformation test7 = new IconInformation();
        test7.id = 2300014;
        test7.resettime = 0;
        _testIconList.Add(test7);

        IconInformation test8 = new IconInformation();
        test8.id = 2300013;
        test8.resettime = (int)Main.mTime + 23;
        _testIconList.Add(test8);

        IconInformation test9 = new IconInformation();
        test9.id = 2300018;
        test9.resettime = (int)Main.mTime + 46;
        _testIconList.Add(test9);

        IconInformation test10 = new IconInformation();
        test10.id = 2300019;
        test10.resettime = (int)Main.mTime + 50;
        _testIconList.Add(test10);

        IconInformation test11 = new IconInformation();
        test11.id = 2300022;
        test11.resettime = 0;
        _testIconList.Add(test11);

    }
    */
}
