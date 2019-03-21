using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;
public class FriendViewController : UIBase
{
    public FriendView view;
    public enum ToggleEnum
    {
        Friend,
        FBFriend,
        FriendMission,
        InviteCode
    }
    public ToggleEnum toggleType = ToggleEnum.Friend;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new FriendView();
            view.Initialize();
        }
        BtnEventBinding();
        FriendModule.Instance.SendGetFriendsListReq();
        FriendModule.Instance.SendGetThirdFriendsTaskReq();
        this.view.Tog_TabFriend.Set(true);
        PlayerData.Instance.NotifyResetEvent += OnNotifyReset;
    }
    #region 好友页签
    public List<FriendComonent> _FriendItemList = new List<FriendComonent>();
    public List<fogs.proto.msg.BasePlayerInfo> tmpDataList = new List<fogs.proto.msg.BasePlayerInfo>();

    public void InitFriendTable()
    {
        this.view.friendGroup.FrienGroup_Lbl_Label_FriendCount.text = string.Format(ConstString.FIREND_HAD_COUNT, FriendModule.Instance.MyFriendList.Count, FriendModule.Instance.MAXFIRENDCOUNT);
        int count = FriendModule.Instance.HadGiveList == null ? FriendModule.Instance.MAXGIVEPOWER : FriendModule.Instance.MAXGIVEPOWER - FriendModule.Instance.HadGiveList.Count;
        this.view.friendGroup.FrienGroup_Lbl_Label_FreeCount.text = string.Format(ConstString.FIREND_FREE_GIVECOUNT, count);
        this.view.friendGroup.FriendPrompt.SetActive(PlayerData.Instance.FbfriendDepot.IsApplyNotice);
        tmpDataList = FriendModule.Instance.MyFriendList;
        this.InitFriendScrollView();
    }
    public void InitNotice()
    {
        if (this.view != null && this.view.friendGroup != null && this.view.friendGroup.FriendPrompt != null && PlayerData.Instance.FbfriendDepot != null)
            this.view.friendGroup.FriendPrompt.SetActive(PlayerData.Instance.FbfriendDepot.IsApplyNotice);
    }
    public void InitFriendScrollView()
    {
        Main.Instance.StartCoroutine(CreatTaskItem(tmpDataList));
    }
    private IEnumerator CreatTaskItem(List<fogs.proto.msg.BasePlayerInfo> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = _FriendItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.friendGroup.FrienGroup_UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.friendGroup.FrienGroup_UIWrapContent_Grid.minIndex = -index;
        this.view.friendGroup.FrienGroup_UIWrapContent_Grid.maxIndex = 0;

        if (count > 5)
        {
            this.view.friendGroup.FrienGroup_UIWrapContent_Grid.enabled = true;
            count = 5;
        }
        else
        {
            this.view.friendGroup.FrienGroup_UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _FriendItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.friendGroup.FriendGroup_Item, this.view.friendGroup.FrienGroup_Grd_Grid.transform);
                FriendComonent item = vGo.GetComponent<FriendComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<FriendComonent>();
                    item.MyStart(vGo);
                }
                _FriendItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _FriendItemList[i].GiveEvent = OnGive;
                _FriendItemList[i].GetEvent = OnGet;
            }
            else
            {
                _FriendItemList[i].gameObject.SetActive(true);
            }
            _FriendItemList[i].SetInfo(_data[i]);
        }

        this.view.friendGroup.FrienGroup_UIWrapContent_Grid.ReGetChild();
        this.view.friendGroup.FrienGroup_Grd_Grid.repositionNow = true;
        yield return 0;
        this.view.friendGroup.FrienGroup_ScrView_FriendList.ResetPosition();
        yield return 0.5;
        this.view.friendGroup.FrienGroup_Grd_Grid.repositionNow = true;
        this.view.friendGroup.FrienGroup_Grd_Grid.gameObject.SetActive(false);
        this.view.friendGroup.FrienGroup_Grd_Grid.gameObject.SetActive(true);
    }
    public void SetTaskInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpDataList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        FriendComonent item = _FriendItemList[wrapIndex];
        item.SetInfo(tmpDataList[realIndex]);
    }

    public void OnGive(FriendComonent com)
    {
        if (com == null)
            return;

        //if (FriendModule.Instance.FindHadGiveListByAccid(com.tmpInfo.accid) == null)
        FriendModule.Instance.SendDonateFriendsHpReq(fogs.proto.msg.FriendsOpType.FO_ONE, com.tmpInfo.accid);
    }
    public void OnGet(FriendComonent com)
    {
        if (com == null)
            return;

        FriendModule.Instance.SendRecieveFriendHpReq(fogs.proto.msg.FriendsOpType.FO_ONE, com.tmpInfo.accid);
    }
    #endregion
    #region 邀请码页签
    public List<FriendMissionComonent> InvitItemList = new List<FriendMissionComonent>();
    public List<fogs.proto.msg.TaskInfo> tmpInvitList = new List<fogs.proto.msg.TaskInfo>();

    /// <summary>
    /// 初始化邀请码页签
    /// </summary>
    public void InitInviteCodeTable()
    {
        this.view.inviteCodeGroup.Lbl_Share.text = PlayerData.Instance.InviteCode;
        this.view.inviteCodeGroup.Title.text = string.Format(ConstString.HAD_INVITECODE_NUM, PlayerData.Instance.InvitCodeNum);
        this.InitCreatInviteCodeItemView();
    }
    public void InitCreatInviteCodeItemView()
    {
        if (FriendModule.Instance.InvitCodeMissionList != null)
        {
            FriendModule.Instance.InvitCodeMissionList.Sort((left, right) =>
            {
                if (left.id != right.id)
                {
                    if (left.id < right.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });

        }
        Main.Instance.StartCoroutine(CreatInviteCodeItem(FriendModule.Instance.InvitCodeMissionList));
    }
    private IEnumerator CreatInviteCodeItem(List<fogs.proto.msg.TaskInfo> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = InvitItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.inviteCodeGroup.MissionWrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.inviteCodeGroup.MissionWrapContent.minIndex = -index;
        this.view.inviteCodeGroup.MissionWrapContent.maxIndex = 0;

        if (count > 5)
        {
            this.view.inviteCodeGroup.MissionWrapContent.enabled = true;
            count = 5;
        }
        else
        {
            this.view.inviteCodeGroup.MissionWrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                InvitItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {

                GameObject vGo = CommonFunction.InstantiateObject(this.view.inviteCodeGroup.MissionItem, this.view.inviteCodeGroup.MissionGrid.transform);
                FriendMissionComonent item = vGo.GetComponent<FriendMissionComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<FriendMissionComonent>();
                    item.MyStart(vGo);
                }
                InvitItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                InvitItemList[i].OnGiveEvent += FriendViewController_OnGiveEvent;
            }
            else
            {
                InvitItemList[i].gameObject.SetActive(true);
            }
            InvitItemList[i].SetInfo(_data[i]);
        }

        this.view.inviteCodeGroup.MissionWrapContent.ReGetChild();
        this.view.inviteCodeGroup.MissionGrid.repositionNow = true;
        yield return 0;
        this.view.inviteCodeGroup.MissionScrollView.ResetPosition();
        yield return 0.5;
        this.view.inviteCodeGroup.MissionGrid.repositionNow = true;
        this.view.inviteCodeGroup.MissionGrid.gameObject.SetActive(false);
        this.view.inviteCodeGroup.MissionGrid.gameObject.SetActive(true);
    }

    void FriendViewController_OnGiveEvent(FriendMissionComonent com)
    {
        if (com != null)
            FriendModule.Instance.SendGetInviteCodeTaskAwardReq(com.tmpInfo.id);
    }
    public void SetInviteCodeInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpInvitList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        FriendMissionComonent item = InvitItemList[wrapIndex];
        item.SetInfo(tmpInvitList[realIndex]);
    }
    #endregion
    #region 好友任务页签
    public List<FriendMissionComonent> FBMissionItemList = new List<FriendMissionComonent>();
    public List<fogs.proto.msg.TaskInfo> tmpFBMissionList = new List<fogs.proto.msg.TaskInfo>();

    public void InitFriendMissionTable()
    {
        this.view.friendMissionGroup.FB_Title.text = string.Format(ConstString.FRIEND_HAD_INVITEFB, FriendModule.Instance.FBInvitNum);
        this.view.friendMissionGroup.LINE_Title.text = string.Format(ConstString.FRIEND_HAD_INVITELINE, FriendModule.Instance.LINEInvitNum);
        this.tmpFBMissionList = FriendModule.Instance.FBInvitMissionList;
        if (this.tmpFBMissionList != null)
        {
            this.tmpFBMissionList.Sort((left, right) =>
            {
                if (left.id != right.id)
                {
                    if (left.id < right.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
            Main.Instance.StartCoroutine(CreatFBMissionItem(this.tmpFBMissionList));
        }
        this.tmpLINEMissionList = FriendModule.Instance.LINEInvitMissionList;
        if (this.tmpLINEMissionList != null)
        {
            this.tmpLINEMissionList.Sort((left, right) =>
            {
                if (left.id != right.id)
                {
                    if (left.id < right.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
            Main.Instance.StartCoroutine(CreatLINEMissionItem(this.tmpLINEMissionList));
        }

    }
    private IEnumerator CreatFBMissionItem(List<fogs.proto.msg.TaskInfo> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = FBMissionItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.friendMissionGroup.FB_WrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.friendMissionGroup.FB_WrapContent.minIndex = -index;
        this.view.friendMissionGroup.FB_WrapContent.maxIndex = 0;

        if (count > 5)
        {
            this.view.friendMissionGroup.FB_WrapContent.enabled = true;
            count = 5;
        }
        else
        {
            this.view.friendMissionGroup.FB_WrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                FBMissionItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {

                GameObject vGo = CommonFunction.InstantiateObject(this.view.friendMissionGroup.FB_Item, this.view.friendMissionGroup.FB_Grid.transform);
                FriendMissionComonent item = vGo.GetComponent<FriendMissionComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<FriendMissionComonent>();
                    item.MyStart(vGo);
                }
                FBMissionItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                FBMissionItemList[i].OnGiveEvent += FBFriendViewController_OnGiveEvent;
            }
            else
            {
                FBMissionItemList[i].gameObject.SetActive(true);
            }
            FBMissionItemList[i].SetInfo(_data[i]);
        }

        this.view.friendMissionGroup.FB_WrapContent.ReGetChild();
        this.view.friendMissionGroup.FB_Grid.repositionNow = true;
        yield return 0;
        this.view.friendMissionGroup.FB_ScrollView.ResetPosition();
        yield return 0.5;
        this.view.friendMissionGroup.FB_Grid.repositionNow = true;
        this.view.friendMissionGroup.FB_Grid.gameObject.SetActive(false);
        this.view.friendMissionGroup.FB_Grid.gameObject.SetActive(true);
    }
    void FBFriendViewController_OnGiveEvent(FriendMissionComonent com)
    {
        if (com != null)
            FriendModule.Instance.SendGetThirdFriendsTaskAwardReq(fogs.proto.msg.ThirdFriendType.FACEBOOK, com.tmpInfo.id);
    }

    public void SetFBMissionInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpFBMissionList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        FriendMissionComonent item = FBMissionItemList[wrapIndex];
        item.SetInfo(tmpFBMissionList[realIndex]);
    }
    public List<FriendMissionComonent> LINEMissionItemList = new List<FriendMissionComonent>();
    public List<fogs.proto.msg.TaskInfo> tmpLINEMissionList = new List<fogs.proto.msg.TaskInfo>();


    private IEnumerator CreatLINEMissionItem(List<fogs.proto.msg.TaskInfo> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = LINEMissionItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.friendMissionGroup.LINE_WrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.friendMissionGroup.LINE_WrapContent.minIndex = -index;
        this.view.friendMissionGroup.LINE_WrapContent.maxIndex = 0;

        if (count > 5)
        {
            this.view.friendMissionGroup.LINE_WrapContent.enabled = true;
            count = 5;
        }
        else
        {
            this.view.friendMissionGroup.LINE_WrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                LINEMissionItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {

                GameObject vGo = CommonFunction.InstantiateObject(this.view.friendMissionGroup.LINE_Item, this.view.friendMissionGroup.LINE_Grid.transform);
                FriendMissionComonent item = vGo.GetComponent<FriendMissionComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<FriendMissionComonent>();
                    item.MyStart(vGo);
                }
                LINEMissionItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                LINEMissionItemList[i].OnGiveEvent += LINEFriendViewController_OnGiveEvent;

            }
            else
            {
                LINEMissionItemList[i].gameObject.SetActive(true);
            }
            LINEMissionItemList[i].SetInfo(_data[i]);
        }

        this.view.friendMissionGroup.LINE_WrapContent.ReGetChild();
        this.view.friendMissionGroup.LINE_Grid.repositionNow = true;
        yield return 0;
        this.view.friendMissionGroup.LINE_ScrollView.ResetPosition();
        yield return 0.5;
        this.view.friendMissionGroup.LINE_Grid.repositionNow = true;
        this.view.friendMissionGroup.LINE_Grid.gameObject.SetActive(false);
        this.view.friendMissionGroup.LINE_Grid.gameObject.SetActive(true);
    }
    void LINEFriendViewController_OnGiveEvent(FriendMissionComonent com)
    {
        if (com != null)
            FriendModule.Instance.SendGetThirdFriendsTaskAwardReq(fogs.proto.msg.ThirdFriendType.LINE, com.tmpInfo.id);
    }

    public void SetLINEMissionInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpLINEMissionList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        FriendMissionComonent item = LINEMissionItemList[wrapIndex];
        item.SetInfo(tmpLINEMissionList[realIndex]);
    }

    #endregion
    #region 按钮响应事件
    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        this.Close(null, null);
    }

    public void ButtonEvent_OneKeyGive(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendDonateFriendsHpReq(fogs.proto.msg.FriendsOpType.FO_ALL, 0);
    }

    public void ButtonEvent_OneKeyGet(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendRecieveFriendHpReq(fogs.proto.msg.FriendsOpType.FO_ALL, 0);
    }
    public void ButtonEvent_FriendRequest(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FRIENDAPPLYVIEW);
    }
    public void ButtonEvent_FriendAdd(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FRIENDADDVIEW);
    }
    public void ButtonEvent_InviteOk(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendVerifyInviteCodeReq(this.view.inviteCodeGroup.CodeInput.value);
    }
    public void ButtonEvent_InviteShare(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        this.view.platformGroup.PlatformGroupBack.SetActive(true);
    }
    /// <summary>
    /// 平台选择界面打开
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FrienMissionShare(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (PlayerData.Instance.FbfriendDepot != null)
            PlayerData.Instance.FbfriendDepot.Init();
        this.view.platformGroup.PlatformGroupBack.SetActive(true);
    }
    public void ButtonEvent_FrienMissionClose(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        this.view.platformGroup.PlatformGroupBack.SetActive(false);
    }
    /// <summary>
    /// FB好友分享
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FrienMissionFBShare(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        this.view.platformGroup.PlatformGroupBack.SetActive(false);
        if (toggleType == ToggleEnum.FriendMission)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FRIENDINVITEVIEW);
            return;
        }
        if (toggleType == ToggleEnum.InviteCode)
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
            {
                List<string> list = new List<string>();
                string message = "";
                FriendSpecialInfo tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.FBSHARETITLE);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                list.Add(message);
                message = "";
                tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.FBSHAREDESCRIPT);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                list.Add(string.Format(message, PlayerData.Instance._NickName));
                message = "";
                tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.FBSHARELINE);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                list.Add(string.Format(message, LoginModule.Instance.CurServer.desc, PlayerData.Instance._NickName, PlayerData.Instance.InviteCode));
                message = "";
                tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.GOOGLEPLAY);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                list.Add(message);
                message = "";
                tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.SDKPLAY);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                list.Add(message);
                SDKManager.Instance.ShareToFacebook(list);
            }
        }
    }
    /// <summary>
    /// LINE好友分享
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_FrienMissionLINEShare(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            if (!SDKManager.Instance.IsAppInstall())
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.FRIEND_SETUP_LINE);
                return;
            }
        }
        if (toggleType == ToggleEnum.FriendMission)
            FriendModule.Instance.SendInviteThirdFriendsReq(fogs.proto.msg.ThirdFriendType.LINE, fogs.proto.msg.ThirdFriendsOpResultType.FAKE_SUCCESS, "");
        else
        {
            if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
            {
                string message = "";
                FriendSpecialInfo tmpSpecial = ConfigManager.Instance.mFriendSpecialData.FindById(FriendSpecialEnum.LINESHAREDESCRIPT);
                if (tmpSpecial != null)
                    message = tmpSpecial.Descript;
                SDKManager.Instance.ShareTextByLine(string.Format(message, LoginModule.Instance.CurServer.desc, PlayerData.Instance._NickName, PlayerData.Instance.InviteCode));
            }
        }
        this.view.platformGroup.PlatformGroupBack.SetActive(false);

    }

    public override void Uninitialize()
    {
        PlayerData.Instance.NotifyResetEvent -= OnNotifyReset;
    }
    public void ButtonEvent_Tog_TabFBFriend(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (PlayerData.Instance.FbfriendDepot != null)
            PlayerData.Instance.FbfriendDepot.Init();
        toggleType = ToggleEnum.FBFriend;
        //FriendModule.Instance.FbfriendDepot.TEXT();
        this.InitFBFriendTable();
    }
    public void ButtonEvent_Tog_TabFriend(GameObject btn)
    {
        toggleType = ToggleEnum.Friend;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendGetFriendsListReq();
    }
    public void ButtonEvent_Tog_TabFriendMisson(GameObject btn)
    {
        toggleType = ToggleEnum.FriendMission;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendGetThirdFriendsTaskReq();
    }
    public void ButtonEvent_Tog_TabWelcome(GameObject btn)
    {
        toggleType = ToggleEnum.InviteCode;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        FriendModule.Instance.SendGetInviteCodeTaskListReq();
    }

    #endregion
    #region FB好友
    public List<FBFriendComonent> _FBFriendItemList = new List<FBFriendComonent>();
    public List<FBFriend> tmpFBFriendDataList = new List<FBFriend>();

    public void InitFBFriendTable()
    {
        InitFBFriendLabel();
        tmpFBFriendDataList = PlayerData.Instance.FbfriendDepot.FBFriendList;
        this.InitFBFriendScrollView();
    }
    public void InitFBFriendLabel()
    {
        this.view.fbFriendGroup.Lbl_Label_FriendCount.text = string.Format(ConstString.FIREND_HAD_COUNTNOMAX, PlayerData.Instance.FbfriendDepot.GetFriendCount());
        if (FriendModule.Instance.FBInvitMissionList != null)
        {
            FriendModule.Instance.FBInvitMissionList.Sort((left, right) =>
            {
                if (left.id != right.id)
                {
                    if (left.id < right.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
            fogs.proto.msg.TaskInfo tmpInfo = FriendModule.Instance.FBInvitMissionList.Find((mis) => { if (mis == null)return false; return mis.status == fogs.proto.msg.TaskStatus.UNFINISHED; });
            if (tmpInfo != null)
            {
                FriendMissionInfo missionInfo = ConfigManager.Instance.mFriendMissionConfig.FindById(tmpInfo.id);
                if (missionInfo != null)
                    this.view.fbFriendGroup.Lbl_Label_FreeCount.text = string.Format(missionInfo.Descript, missionInfo.PlayerLevel, string.Format("{0}/{1}", tmpInfo.con_value, missionInfo.PlayerCount));

            }
            else
                this.view.fbFriendGroup.Lbl_Label_FreeCount.text = "";
        }
    }
    public void InitFBFriendScrollView()
    {
        Main.Instance.StartCoroutine(CreatFBFriendItem(tmpFBFriendDataList));
    }
    private IEnumerator CreatFBFriendItem(List<FBFriend> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = _FBFriendItemList.Count;

        int index = Mathf.CeilToInt((float)count / this.view.fbFriendGroup.UIWrapContent_Grid.wideCount) - 1;
        if (index == 0)
            index = 1;
        this.view.fbFriendGroup.UIWrapContent_Grid.minIndex = -index;
        this.view.fbFriendGroup.UIWrapContent_Grid.maxIndex = 0;

        if (count > 5)
        {
            this.view.fbFriendGroup.UIWrapContent_Grid.enabled = true;
            count = 5;
        }
        else
        {
            this.view.fbFriendGroup.UIWrapContent_Grid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _FBFriendItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(this.view.fbFriendGroup.Item, this.view.fbFriendGroup.Grd_Grid.transform);
                FBFriendComonent item = vGo.GetComponent<FBFriendComonent>();
                if (item == null)
                {
                    item = vGo.AddComponent<FBFriendComonent>();
                    item.MyStart(vGo);
                }
                _FBFriendItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
            }
            else
            {
                _FBFriendItemList[i].gameObject.SetActive(true);
            }
            _FBFriendItemList[i].SetInfo(_data[i]);
        }
        if (count > 3)
            this.view.fbFriendGroup.UIWrapContent_Grid.ReGetChild();
        this.view.fbFriendGroup.Grd_Grid.repositionNow = true;
        yield return 0;
        this.view.fbFriendGroup.ScrView_FriendList.ResetPosition();
        yield return 0.5;
        this.view.fbFriendGroup.Grd_Grid.repositionNow = true;
        this.view.fbFriendGroup.Grd_Grid.gameObject.SetActive(false);
        this.view.fbFriendGroup.Grd_Grid.gameObject.SetActive(true);
    }
    public void SetFBFriendInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tmpFBFriendDataList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        if (realIndex > PlayerData.Instance.FbfriendDepot.GetStarGetThirdFriendsInfoReqIndex())
            PlayerData.Instance.FbfriendDepot.StartGetThirdFriendsInfoReq();

        FBFriendComonent item = _FBFriendItemList[wrapIndex];
        item.SetInfo(tmpFBFriendDataList[realIndex]);
    }

    #endregion
    private void OnNotifyReset(NotifyReset data)
    {
        FriendModule.Instance.MyFriendInfo.recieve_ph_times = 0;
        FriendModule.Instance.HadGiveList.Clear();
        FriendModule.Instance.HadGetList.Clear();
        FriendModule.Instance.MyFriendInfo.donate_ph_times = 0;
        InitFriendTable();
    }

    public override void Destroy()
    {
        base.Destroy();
        this._FriendItemList.Clear();
        this.FBMissionItemList.Clear();
        this.InvitItemList.Clear();
        this._FBFriendItemList.Clear();
        this.LINEMissionItemList.Clear();
        this.tmpDataList.Clear();
        this.tmpFBFriendDataList.Clear();
        this.tmpFBMissionList.Clear();
        this.tmpInvitList.Clear();
        this.tmpLINEMissionList.Clear();
        this.view = null;
    }
    public void BtnEventBinding()
    {
        this.view.friendGroup.FrienGroup_UIWrapContent_Grid.onInitializeItem = SetTaskInfo;
        this.view.inviteCodeGroup.MissionWrapContent.onInitializeItem = SetInviteCodeInfo;
        this.view.friendMissionGroup.FB_WrapContent.onInitializeItem = SetFBMissionInfo;
        this.view.friendMissionGroup.LINE_WrapContent.onInitializeItem = SetLINEMissionInfo;
        this.view.fbFriendGroup.UIWrapContent_Grid.onInitializeItem = SetFBFriendInfo;

        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.friendGroup.FrienGroup_Btn_OneKeyGive.gameObject).onClick = ButtonEvent_OneKeyGive;
        UIEventListener.Get(view.friendGroup.FrienGroup_Btn_OneKeyGet.gameObject).onClick = ButtonEvent_OneKeyGet;
        UIEventListener.Get(view.friendGroup.FrienGroup_Btn_FriendRequest.gameObject).onClick = ButtonEvent_FriendRequest;
        UIEventListener.Get(view.friendGroup.FrienGroup_Btn_FriendAdd.gameObject).onClick = ButtonEvent_FriendAdd;
        UIEventListener.Get(view.Tog_TabFBFriend.gameObject).onClick = ButtonEvent_Tog_TabFBFriend;
        UIEventListener.Get(view.Tog_TabFriend.gameObject).onClick = ButtonEvent_Tog_TabFriend;
        UIEventListener.Get(view.Tog_TabFriendMisson.gameObject).onClick = ButtonEvent_Tog_TabFriendMisson;
        UIEventListener.Get(view.Tog_TabWelcome.gameObject).onClick = ButtonEvent_Tog_TabWelcome;

        UIEventListener.Get(view.friendMissionGroup.FriendShare.gameObject).onClick = ButtonEvent_FrienMissionShare;
        UIEventListener.Get(view.platformGroup.Mask.gameObject).onClick = ButtonEvent_FrienMissionClose;
        UIEventListener.Get(view.platformGroup.Btn_FBFriend.gameObject).onClick = ButtonEvent_FrienMissionFBShare;
        UIEventListener.Get(view.platformGroup.Btn_LINEFriend.gameObject).onClick = ButtonEvent_FrienMissionLINEShare;

        UIEventListener.Get(view.inviteCodeGroup.Btn_Ok.gameObject).onClick = ButtonEvent_InviteOk;
        UIEventListener.Get(view.inviteCodeGroup.Btn_Share.gameObject).onClick = ButtonEvent_InviteShare;
    }
}
