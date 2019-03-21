using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class FunctionMenuViewController : UIBase
{
    protected enum FunctionViewType
    {
        None = 0,
        FromChatView = 1,
        FromFriendsView = 2,
    }

    private FunctionViewType fromViewType;

    public FunctionMenuView view;

    private ChatInfo ChatInfoPOD;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new FunctionMenuView();
            view.Initialize();
        }
        BtnEventBinding();
    }

    #region Button Event
    public void ButtonEvent_CheckInfo(GameObject btn)
    {
        if (fromViewType == FunctionViewType.FromFriendsView)
        {
            FriendModule.Instance.SendGetTopPlayerInfoReq(ChatInfoPOD.accid);
        }
        else
            ChatModule.Instance.SendGetPlayerInfo(ChatInfoPOD.area_id, ChatInfoPOD.other_accname, GetPlayerInfoSuccess);
        CloseView();
    }

    public void ButtonEvent_LaunchChat(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHATVIEW);
        UISystem.Instance.ChatView.UpdateViewInfo(ChatTypeEnum.Private, ChatInfoPOD);
        CloseView();
    }

    public void ButtonEvent_AddFriend(GameObject btn)
    {
        FriendModule.Instance.SendApplyAddFriendsReq(ChatInfoPOD.other_accname, ChatInfoPOD.accid);
        CloseView();
    }

    public void ButtonEvent_DeleteFriend(GameObject btn)
    {
        ChatInfo tmpInfo = this.ChatInfoPOD;
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.FIREND_DELET_READY, () =>
        {
            FriendModule.Instance.SendDelFriendsReq(tmpInfo.accid);
        });
        CloseView();
    }

    public void ButtonEvent_ShieldPlayer(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, view._uiRoot.transform.parent.transform));
        CommonFunction.SetShieldingPlayers(ChatInfoPOD.other_accname, ChatInfoPOD.area_id, ChatInfoPOD.char_name);
        CloseView();

    }

    public void ButtonEvent_CropsInvite(GameObject btn)
    {
        if (UnionModule.Instance.MyUnionMemberInfo.position == UnionPosition.UP_MEMBER)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.CHATVIEW_UNIONTIP_INVITE);
        }
        else
        {
            bool result = ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Sociaty, ChatInfoPOD.vip_level, ChatInfoPOD.player_level, false);
            if (result)
            {
                UnionModule.Instance.OnSendInvitePlayerToUnion(ChatInfoPOD.accid);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHATVIEW_UNIONTIP_NOTOPEN);
            }
        }
        CloseView();
    }

    public void ButtonEvent_Enslaved(GameObject btn)
    {
        if (ChatInfoPOD == null)
            return;
        PlayerData.Instance._Prison.GetOtherPlayerInfo(ChatInfoPOD.other_accname, ChatInfoPOD.area_id);
        CloseView();
    }

    public void ButtonEvent_Rescue(GameObject btn)
    {
        if (ChatInfoPOD == null) return;
        PlayerData.Instance._Prison.GetOtherPlayerInfo(ChatInfoPOD.other_accname, ChatInfoPOD.area_id);
        CloseView();
    }
    public void ButtonEvent_CloseFunc(GameObject go)
    {
        CloseView();
    }

    #endregion

    public void UpdateViewInfo(ChatInfo info, int type)
    {
        fromViewType = (FunctionViewType)type;
        ChatInfoPOD = info;
        if (info == null)
        {
            return;
        }
        //string accName = string.Empty;
        //Debug.LogError(ChatInfoPOD.send_type);
        //if (ChatInfoPOD.send_type == 1)   //自己发送给別人
        //{
        //    accName = ChatInfoPOD.char_name;
        //}
        //else
        //{
        //    accName = ChatInfoPOD.char_name;
        //}
        view.Lbl_PlayerName.text = ChatInfoPOD.char_name;
        view.Btn_CropsInvite.gameObject.SetActive(UnionModule.Instance.HasUnion);
        int funcCount = 2;
        switch (fromViewType)
        {
            case FunctionViewType.FromChatView:
                {
                    view.Btn_ShieldPlayer.gameObject.SetActive(true);
                    funcCount++;
                } break;
            case FunctionViewType.FromFriendsView:
                {
                    view.Btn_ShieldPlayer.gameObject.SetActive(false);
                } break;
        }
        if (ChatInfoPOD.chat_type == ChatType.CT_NORMAL)
        {
            if (ConfigManager.Instance.mOpenLevelConfig.CheckFuncIsOpen(OpenFunctionType.Slave, false) && !MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Slave))
            {
                if (ChatInfoPOD.enslave == 2)//是否被奴役（2表示被奴役，1表示自由）
                {
                    view.Btn_Enslaved.gameObject.SetActive(false);
                    view.Btn_Rescue.gameObject.SetActive(true);
                    funcCount++;
                }
                else if (ChatInfoPOD.enslave == 1)
                {
                    view.Btn_Enslaved.gameObject.SetActive(true);
                    view.Btn_Rescue.gameObject.SetActive(false);
                    funcCount++;
                }
                else
                {
                    view.Btn_Enslaved.gameObject.SetActive(false);
                    view.Btn_Rescue.gameObject.SetActive(false);
                }
            }
            else
            {
                view.Btn_Enslaved.gameObject.SetActive(false);
                view.Btn_Rescue.gameObject.SetActive(false);
            }
        }
        else
        {
            view.Btn_Enslaved.gameObject.SetActive(false);
            view.Btn_Rescue.gameObject.SetActive(false);
        }
        if (UnionModule.Instance.HasUnion && !MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Sociaty))
        {
            funcCount++;
        }
        if (GlobalConst.PLATFORM == TargetPlatforms.Android_7725 || GlobalConst.PLATFORM == TargetPlatforms.Android_7725OL)
        {
            switch (fromViewType)
            {
                case FunctionViewType.FromChatView:
                    {
                        bool isFriend = FriendModule.Instance.IsMyFriend(ChatInfoPOD.accid); //判断是否是己方好友
                        view.Btn_AddFriend.gameObject.SetActive(!isFriend);
                        view.Btn_DeleteFriend.gameObject.SetActive(isFriend);
                    } break;
                case FunctionViewType.FromFriendsView:
                    {
                        view.Btn_AddFriend.gameObject.SetActive(false);
                        view.Btn_DeleteFriend.gameObject.SetActive(true);
                    } break;
            }
            funcCount++;
        }
        else
        {
            view.Btn_AddFriend.gameObject.SetActive(false);
            view.Btn_DeleteFriend.gameObject.SetActive(false);
        }
        view.Grd_ButtonGroup.repositionNow = true;
        view.Spt_FunctionBG.height = 50 * funcCount + 75;
    }
    public void GetPlayerInfoSuccess(ArenaPlayer data)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
        UISystem.Instance.PlayerInfoView.UpdateViewInfo(PlayerInfoTypeEnum.Arena, data);
    }
    private void CloseView()
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_FUNCTIONMENUBVIEW);
    }

    public override void Uninitialize()
    {
        fromViewType = FunctionViewType.None;
        ChatInfoPOD = null;
    }
    public override void Destroy()
    {
        base.Destroy();
        this.view = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CheckInfo.gameObject).onClick = ButtonEvent_CheckInfo;
        UIEventListener.Get(view.Btn_LaunchChat.gameObject).onClick = ButtonEvent_LaunchChat;
        UIEventListener.Get(view.Btn_AddFriend.gameObject).onClick = ButtonEvent_AddFriend;
        UIEventListener.Get(view.Btn_DeleteFriend.gameObject).onClick = ButtonEvent_DeleteFriend;
        UIEventListener.Get(view.Btn_ShieldPlayer.gameObject).onClick = ButtonEvent_ShieldPlayer;
        UIEventListener.Get(view.Btn_CropsInvite.gameObject).onClick = ButtonEvent_CropsInvite;
        UIEventListener.Get(view.Btn_Enslaved.gameObject).onClick = ButtonEvent_Enslaved;
        UIEventListener.Get(view.Btn_Rescue.gameObject).onClick = ButtonEvent_Rescue;
        UIEventListener.Get(view.Spt_FuncMask.gameObject).onClick = ButtonEvent_CloseFunc;
    }


}
