using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;

public class ChatViewController : UIBase
{
    public ChatView view;
    private ChatTypeEnum currentType = ChatTypeEnum.World;
    private List<ChatInfoComponent> chatInfoList;
    private ChatInfo chatInfoPOD;
    private List<ChatInfo> worldInfoList;
    private List<ChatInfo> corpsInfoList;
    private List<ChatInfo> privateInfoList;
    private ChatFuncTypeComponent worldTypeComp;
    private ChatFuncTypeComponent corpsTypeComp;
    private ChatFuncTypeComponent privateTypeComp;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new ChatView();
            view.Initialize();
        }
        if (chatInfoList == null)
        {
            chatInfoList = new List<ChatInfoComponent>();
        }
        InitView();
        //UpdateViewInfo();
        BtnEventBinding();
        PlayerData.Instance.NotifyResetEvent += UpdateResetChatTime;
        SystemSettingModule.Instance.ShieldPlayerEvent += OnShieldPlayer;
    }

    private void InitView()
    {
        currentType = ChatTypeEnum.World;
        view.Gobj_Chat.SetActive(true);
        view.Gobj_ContentComp.SetActive(false);
        //view.Gobj_FunctionPanel.SetActive(false);
        if (worldTypeComp == null)
        {
            worldTypeComp = new ChatFuncTypeComponent(view.Btn_WroldChat.gameObject);
        }
        if (corpsTypeComp == null)
        {
            corpsTypeComp = new ChatFuncTypeComponent(view.Btn_CorpsChat.gameObject);
        }
        if (privateTypeComp == null)
        {
            privateTypeComp = new ChatFuncTypeComponent(view.Btn_PrivateChat.gameObject);
        }
        if (ChatModule.Instance.worldChatList == null)
        {
            ChatModule.Instance.worldChatList = new List<ChatInfo>();
        }
        if (ChatModule.Instance.corpsChatList == null)
        {
            ChatModule.Instance.corpsChatList = new List<ChatInfo>();
        }
        if (ChatModule.Instance.privateChatList == null)
        {
            ChatModule.Instance.privateChatList = new List<ChatInfo>();
        }
        if (corpsInfoList == null)
        {
            corpsInfoList = new List<ChatInfo>();
        }
        if (worldInfoList == null)
        {
            worldInfoList = new List<ChatInfo>();
        }
        if (privateInfoList == null)
        {
            privateInfoList = new List<ChatInfo>();
        }
        worldInfoList.Clear();
        worldInfoList.AddRange(ChatModule.Instance.worldChatList);
        corpsInfoList.Clear();

        corpsInfoList.AddRange(ChatModule.Instance.corpsChatList);
        privateInfoList.Clear();
        privateInfoList.AddRange(ChatModule.Instance.privateChatList);
    }


    #region Update Event

    private void UpdateResetChatTime(NotifyReset data)
    {
        if (data == null) return;
        UpdateWorldTip();
    }

    public void UpdateViewInfo(ChatTypeEnum type, ChatInfo pod)
    {
        currentType = type;
        chatInfoPOD = pod;
        if (pod == null)
            return;
        UpdateViewInfo();
        //string accname = string.Empty;
        //if (chatInfoPOD.send_type == 1)   //自己发送给別人send
        //{
        //    accname = chatInfoPOD.char_name;
        //}
        //else
        //{
        //    accname = chatInfoPOD.other_accname;
        //}
        UpdateFriendName(chatInfoPOD.char_name);
    }

    public void UpdateViewInfo(ChatTypeEnum type)
    {
        currentType = type;
        UpdateViewInfo();
    }

    public void UpdateViewInfo()
    {
        UpdateChatInfoList();
        UpdateButtomStatus();
        UpdateFuncTypeStatus();
    }

    private bool isBlocked(string accname)
    {
        if (PlayerData.Instance.BlockedPlayers == null)
            PlayerData.Instance.BlockedPlayers = new List<OtherList>();
        for (int i = 0; i < PlayerData.Instance.BlockedPlayers.Count; i++)
        {
            OtherList blocked = PlayerData.Instance.BlockedPlayers[i];
            if (blocked.accname.Equals(accname))
                return true;
        }
        return false;
    }

    private void UpdateChatInfoList()
    {
        switch (currentType)
        {
            case ChatTypeEnum.Corps:
                {
                    Main.Instance.StartCoroutine(UpdateChatList(corpsInfoList));
                }
                break;
            case ChatTypeEnum.Private:
                {
                    Main.Instance.StartCoroutine(UpdateChatList(privateInfoList));
                }
                break;
            case ChatTypeEnum.World:
            default:
                {
                    Main.Instance.StartCoroutine(UpdateChatList(worldInfoList));
                }
                break;
        }
    }

    private void UpdateChatInfoReadStatus(List<ChatInfo> chatList, List<ulong> IDList)
    {
        if (chatList == null || IDList == null) return;
        for (int i = 0; i < chatList.Count; i++)
        {
            ChatInfo info = chatList[i];
            if (info == null) continue;
            if (IDList.Contains(info.chat_id))
            {
                info.status = 2;
            }
        }
    }

    private IEnumerator UpdateChatList(List<ChatInfo> list)
    {
        view.ScrView_ContentScrollView.ResetPosition();
        yield return null;
        int itemCount = chatInfoList.Count;
        if (list.Count <= itemCount)
        {
            for (int i = list.Count; i < itemCount; i++)
            {
                ChatInfoComponent comp = chatInfoList[i];
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < list.Count; i++)
        {
            ChatInfo data = list[i];
            ChatInfoComponent comp = null;
            if (i < itemCount)
            {
                comp = chatInfoList[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ContentComp, view.UITable_Table.transform);
                go.name = "chatComp" + i.ToString();
                comp = new ChatInfoComponent();
                comp.MyStart(go);
                //comp.AddEventListener(ButtonEvent_ChatInfo);
                comp.playerNameHandle = ButtonEvent_PlayerName;
                comp.rescueNameHandle = ButtonEvent_RescuePlayer;
                comp.playerIconHandle = ButtonEvent_PlayerIcon;
                chatInfoList.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateCompInfo(currentType, data);
            if (isBlocked(data.other_accname))
            {
                comp.mRootObject.SetActive(false);
            }
            else
            {
                comp.mRootObject.SetActive(true);
            }
        }
        //yield return null;
        view.UITable_Table.repositionNow = true;
        yield return null;
        if (view.ScrView_ContentScrollView.shouldMoveVertically)
        {
            view.ScrView_ContentScrollView.contentPivot = UIWidget.Pivot.BottomLeft;
        }
        else
        {
            view.ScrView_ContentScrollView.contentPivot = UIWidget.Pivot.TopLeft;
        }
        view.ScrView_ContentScrollView.ResetPosition();
    }

    private void UpdateButtomStatus()
    {
        CommonFunction.UpdateWidgetGray(view.Spt_BtnSendChat, false);
        switch (currentType)
        {
            case ChatTypeEnum.Corps:
                {
                    view.Gobj_SendCorpsChat.SetActive(true);
                    view.Gobj_SendPrivateChat.SetActive(false);
                    view.Gobj_SendWorldChat.SetActive(false);
                    view.Lbl_BtnSendChat.text = ConstString.CHATVIEW_LABEL_BTNSEND;
                    Scheduler.Instance.RemoveUpdator(UpdateWorldSendCD);
                }
                break;
            case ChatTypeEnum.Private:
                {
                    view.Gobj_SendCorpsChat.SetActive(false);
                    view.Gobj_SendWorldChat.SetActive(false);
                    view.Gobj_SendPrivateChat.SetActive(true);
                    view.Lbl_BtnSendChat.text = ConstString.CHATVIEW_LABEL_BTNSEND;
                    Scheduler.Instance.RemoveUpdator(UpdateWorldSendCD);
                }
                break;
            case ChatTypeEnum.World:
            default:
                {
                    view.Gobj_SendCorpsChat.SetActive(false);
                    view.Gobj_SendPrivateChat.SetActive(false);
                    view.Gobj_SendWorldChat.SetActive(true);
                    UpdateWorldTip();
                    long cdTime = PlayerData.Instance.WorldChatCDTime - Main.mTime;
                    if (cdTime > 0)
                    {
                        Scheduler.Instance.AddUpdator(UpdateWorldSendCD);
                    }
                    else
                    {
                        view.Lbl_BtnSendChat.text = ConstString.CHATVIEW_LABEL_BTNSEND;
                        Scheduler.Instance.RemoveUpdator(UpdateWorldSendCD);
                    }
                }
                break;
        }
    }

    private void UpdateWorldTip()
    {
        if (PlayerData.Instance.WorldChatCount <= 0)
        {
            view.Lbl_WroldFreeTip.enabled = false;
            view.Gobj_PurchaseSend.SetActive(true);
            view.Lbl_PurchaseSend.text = GlobalCoefficient.PurchaseWorldChatCount.ToString();
        }
        else
        {
            view.Lbl_WroldFreeTip.enabled = true;
            view.Gobj_PurchaseSend.SetActive(false);
            view.Lbl_WroldFreeTip.text = string.Format(ConstString.CHATVIEW_LABEL_WORLDFREETIP, PlayerData.Instance.WorldChatCount);
        }
    }

    private void UpdateWorldSendCD()
    {
        long cdTime = PlayerData.Instance.WorldChatCDTime - Main.mTime;
        if (cdTime > 0)
        {
            CommonFunction.UpdateWidgetGray(view.Spt_BtnSendChat, true);
            view.Lbl_BtnSendChat.text = cdTime.ToString() + "s";
        }
        else
        {
            CommonFunction.UpdateWidgetGray(view.Spt_BtnSendChat, false);
            view.Lbl_BtnSendChat.text = ConstString.CHATVIEW_LABEL_BTNSEND;
            Scheduler.Instance.RemoveUpdator(UpdateWorldSendCD);
        }
    }

    private void UpdateFuncTypeStatus()
    {
        switch (currentType)
        {
            case ChatTypeEnum.Corps:
                {
                    corpsTypeComp.IsSelect = true;
                    privateTypeComp.IsSelect = false;
                    worldTypeComp.IsSelect = false;
                }
                break;
            case ChatTypeEnum.Private:
                {
                    privateTypeComp.IsSelect = true;
                    worldTypeComp.IsSelect = false;
                    corpsTypeComp.IsSelect = false;
                }
                break;
            case ChatTypeEnum.World:
            default:
                {
                    worldTypeComp.IsSelect = true;
                    privateTypeComp.IsSelect = false;
                    corpsTypeComp.IsSelect = false;
                }
                break;
        }
        UpdateFuncTypeMarkStatus();
    }

    private void UpdateFuncTypeMarkStatus()
    {
        //if (privateTypeComp.IsMark)
        //{
        privateTypeComp.IsMark = IsNewMessage(privateInfoList, ChatTypeEnum.Private);
        //}
        //if (corpsTypeComp.IsMark)
        //{
        corpsTypeComp.IsMark = IsNewMessage(corpsInfoList, ChatTypeEnum.Corps);
        //}

        if (!privateTypeComp.IsMark && !corpsTypeComp.IsMark)
        {
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_MAINCITY))
            {
                UISystem.Instance.MainCityView.ShowChat(false);
            }
        }
    }

    private bool IsNewMessage(List<ChatInfo> list, ChatTypeEnum type)
    {
        bool status = false;
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                ChatInfo info = list[i];
                if (info == null) continue;
                if (ChatModule.Instance.IsMsgNew(info, type))
                {
                    status = true;
                    break;
                }
            }
        }
        return status;
    }

    private void UpdateFuncStatus()
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_FUNCTIONMENUBVIEW);
        UISystem.Instance.FunctionMenuView.UpdateViewInfo(chatInfoPOD, 1);
    }

    private void UpdateFriendName(string name)
    {
        view.Lbl_FriendName.text = name;
    }

    #endregion

    #region  Handle Event

    public void ButtonEvent_PlayerName(BaseComponent baseComp)
    {
        ChatInfoComponent currentChatComp = baseComp as ChatInfoComponent;
        if (currentChatComp.ChatInfoPOD.accid == PlayerData.Instance._AccountID) return;  //如果是点击的自己的消息 则无任何反应
        chatInfoPOD = currentChatComp.ChatInfoPOD;
        if (currentChatComp.ChatInfoPOD.chat_type != ChatType.CT_SYSTEM)
            UpdateFuncStatus();
    }

    public void ButtonEvent_WroldChat(GameObject btn)
    {
        if (currentType == ChatTypeEnum.World) return;
        currentType = ChatTypeEnum.World;
        UpdateViewInfo();
    }

    public void ButtonEvent_CorpsChat(GameObject btn)
    {
        if (currentType == ChatTypeEnum.Corps) return;
        if (UnionModule.Instance.HasUnion == false)  //等于0 表示无军团 
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHATVIEW_TIP_NOCORPS);
        }
        else
        {
            ChatModule.Instance.SendReadChatInfo(ChatTypeEnum.Corps);
        }
    }

    public void ButtonEvent_PrivateChat(GameObject btn)
    {
        if (currentType == ChatTypeEnum.Private) return;
        ChatModule.Instance.SendReadChatInfo(ChatTypeEnum.Private);
    }

    public void ButtonEvent_CloseButton(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_CHATVIEW);
    }


    public void ButtonEvent_SendChat(GameObject btn)
    {
        switch (currentType)
        {
            case ChatTypeEnum.Corps:
                {
                    if (UnionModule.Instance.HasUnion == false)  //等于0 表示无军团 
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.CHATVIEW_TIP_NOCORPS);
                    }
                    else
                    {
                        if (string.IsNullOrEmpty(view.Ipt_CorpsInput.value))
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_INPUT);
                        }
                        else
                        {
                            ChatModule.Instance.SendChat(view.Ipt_CorpsInput.value, (uint)currentType);
                        }
                    }
                }
                break;
            case ChatTypeEnum.World:
                {
                    long cdTime = PlayerData.Instance.WorldChatCDTime - Main.mTime;
                    if (cdTime > 0) return;
                    OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.WorldChatLockLevel);
                    if (data != null && data.openLevel > PlayerData.Instance._Level)
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.CHATVIEW_WORLDCHAT_LOCKLEVEL, data.openLevel));
                    }
                    else if (string.IsNullOrEmpty(view.Ipt_WorldInput.value))
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_INPUT);
                    }
                    else if (PlayerData.Instance.WorldChatCount <= 0)
                    {
                        if (PlayerData.Instance.WorldSendTip == 1) // 约定为1时  说明今天已经提示
                        {
                            ChatModule.Instance.SendChat(view.Ipt_WorldInput.value, (uint)currentType);
                        }
                        else
                        {
                            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.CHATVIEW_LABEL_NOWORLDCOUNT, GlobalCoefficient.PurchaseWorldChatCount), () =>
                            {
                                ChatModule.Instance.SendChat(view.Ipt_WorldInput.value, (uint)currentType);
                            });
                        }
                    }
                    else
                    {
                        ChatModule.Instance.SendChat(view.Ipt_WorldInput.value, (uint)currentType);
                    }
                }
                break;
            case ChatTypeEnum.Private:
                {
                    if (string.IsNullOrEmpty(view.Ipt_PrivateInput.value))
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_INPUT);
                    }
                    else if (string.IsNullOrEmpty(view.Lbl_FriendName.text) || view.Lbl_FriendName.text.Equals(ConstString.CHATVIEW_LABEL_SELECTCHATPLAYER))
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_PLAYER);
                    }
                    else
                    {
                        ChatModule.Instance.SendChat(view.Ipt_PrivateInput.value, (uint)currentType, chatInfoPOD.accid, chatInfoPOD.icon, chatInfoPOD.icon_frame, chatInfoPOD.player_level, chatInfoPOD.vip_level, chatInfoPOD.char_name, chatInfoPOD.other_accname, chatInfoPOD.enslave);
                    }
                }
                break;
        }
    }

    public void ButtonEvent_PlayerIcon(BaseComponent baseComp)
    {
        ChatInfoComponent currentChatComp = baseComp as ChatInfoComponent;
        chatInfoPOD = currentChatComp.ChatInfoPOD;
        if (currentType == ChatTypeEnum.Private)
        {
            if (chatInfoPOD.send_type == 1) return;  //如果是点击的自己的消息 则无任何反应
        }
        if (chatInfoPOD.accid == PlayerData.Instance._AccountID) return;
        if (chatInfoPOD.chat_type != ChatType.CT_SYSTEM)
            UpdateFuncStatus();
    }

    public void ButtonEvent_RescuePlayer(BaseComponent baseComp)
    {
        ChatInfoComponent comp = baseComp as ChatInfoComponent;
        if (comp == null)
            return;
        PlayerData.Instance._Prison.GetAskInfo(comp.ChatInfoPOD.other_accname, comp.ChatInfoPOD.area_id, comp.ChatInfoPOD.send_time);
    }


    #endregion

    private void OnShieldPlayer()
    {
        UpdateChatInfoList();
    }

    public void UpdateWorldChat(ChatInfo list)
    {
        if (worldInfoList == null)
        {
            worldInfoList = new List<ChatInfo>();
        }
        worldInfoList.Add(list);
        SortAndRemoveMsg(worldInfoList);
    }

    public void UpdateCorpsChat(ChatInfo list)
    {
        if (corpsInfoList == null)
        {
            corpsInfoList = new List<ChatInfo>();
        }
        corpsInfoList.Add(list);
        SortAndRemoveMsg(corpsInfoList);
    }
    public void UpdatePrivateChat(ChatInfo list)
    {
        if (privateInfoList == null)
        {
            privateInfoList = new List<ChatInfo>();
        }
        privateInfoList.Add(list);
        SortAndRemoveMsg(privateInfoList);
    }

    private void SortAndRemoveMsg(List<ChatInfo> list)
    {
        list.Sort((ChatInfo left, ChatInfo right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            else
            {
                return left.send_time.CompareTo(right.send_time);
            }
        });
        if (list.Count > GlobalCoefficient.LocalChatCountLimit)
        {
            List<ChatInfo> infos = new List<ChatInfo>();
            int index = list.Count - GlobalCoefficient.LocalChatCountLimit;
            for (int i = index; i < list.Count; i++)
            {
                ChatInfo info = list[i];
                if (info != null)
                {
                    infos.Add(info);
                }
            }
            list.Clear();
            list.AddRange(infos);
        }
    }

    public void GetPlayerInfoSuccess(ArenaPlayer data)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_PLAYERINFO);
        UISystem.Instance.PlayerInfoView.UpdateViewInfo(PlayerInfoTypeEnum.Arena, data);
    }

    public void SendChatSuccess()
    {
        switch (currentType)
        {
            case ChatTypeEnum.World:
                {
                    view.Ipt_WorldInput.value = string.Empty;
                }
                break;
            case ChatTypeEnum.Corps:
                {
                    view.Ipt_CorpsInput.value = string.Empty;
                }
                break;
            case ChatTypeEnum.Private:
                {
                    view.Ipt_PrivateInput.value = string.Empty;
                }
                break;
        }
        UpdateButtomStatus();
    }

    public void NotifyChatRefresh(uint type, ChatInfo info)
    {
        switch ((ChatTypeEnum)type)
        {
            case ChatTypeEnum.World:
                UpdateWorldChat(info);
                break;
            case ChatTypeEnum.Corps:
                UpdateCorpsChat(info);
                break;
            case ChatTypeEnum.Private:
                UpdatePrivateChat(info);
                break;
        }
        if (currentType == (ChatTypeEnum)type)
        {
            UpdateChatInfoList();
        }
        UpdateFuncTypeMarkStatus();
    }

    public void UpdateChatReadStatus(ChatTypeEnum type, List<ulong> list)
    {
        currentType = type;
        UpdateChatInfoReadStatus(privateInfoList, list);
        UpdateChatInfoReadStatus(corpsInfoList, list);
        UpdateViewInfo();
    }

    public override void Uninitialize()
    {
        chatInfoPOD = null;
        currentType = ChatTypeEnum.World;
        if (worldInfoList != null)
            worldInfoList.Clear();
        if (privateInfoList != null)
            privateInfoList.Clear();
        if (corpsInfoList != null)
            corpsInfoList.Clear();
        if (view != null)
        {
            view.Ipt_CorpsInput.value = string.Empty;
            view.Ipt_WorldInput.value = string.Empty;
            view.Ipt_PrivateInput.value = string.Empty;
            view.Lbl_FriendName.text = ConstString.CHATVIEW_LABEL_SELECTCHATPLAYER;
        }
        Scheduler.Instance.RemoveUpdator(UpdateWorldSendCD);
        Main.Instance.StopCoroutine(UpdateChatList(null));
        SystemSettingModule.Instance.ShieldPlayerEvent -= OnShieldPlayer;
        PlayerData.Instance.NotifyResetEvent -= UpdateResetChatTime;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_WroldChat.gameObject).onClick = ButtonEvent_WroldChat;
        UIEventListener.Get(view.Btn_CorpsChat.gameObject).onClick = ButtonEvent_CorpsChat;
        UIEventListener.Get(view.Btn_PrivateChat.gameObject).onClick = ButtonEvent_PrivateChat;
        UIEventListener.Get(view.Btn_CloseButton.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_SendChat.gameObject).onClick = ButtonEvent_SendChat;
        //UIEventListener.Get(view.Btn_CheckInfo.gameObject).onClick = ButtonEvent_CheckInfo;
        //UIEventListener.Get(view.Btn_LaunchChat.gameObject).onClick = ButtonEvent_LaunchChat;
        //UIEventListener.Get(view.Btn_ShieldPlayer.gameObject).onClick = ButtonEvent_ShieldPlayer;
        //UIEventListener.Get(view.Btn_CropsInvite.gameObject).onClick = ButtonEvent_CropsInvite;
        //UIEventListener.Get(view.Btn_Enslaved.gameObject).onClick = ButtonEvent_Enslaved;
        //UIEventListener.Get(view.Btn_Rescue.gameObject).onClick = ButtonEvent_Rescue;
        //UIEventListener.Get(view.Spt_FuncMask.gameObject).onClick = ButtonEvent_CloseFunc;
    }


    public override void Destroy()
    {
        base.Destroy();
        view = null;
        worldTypeComp = null;
        privateTypeComp = null;
        corpsTypeComp = null;
        chatInfoList = null;
    }
}
