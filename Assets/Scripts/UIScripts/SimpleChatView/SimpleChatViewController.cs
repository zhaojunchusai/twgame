using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class SimpleChatViewController : UIBase
{
    public SimpleChatView view;
    private ChatTypeEnum chatType;
    private List<ChatInfo> chatList;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new SimpleChatView();
            view.Initialize();
        }
        UpdateNotify(false);
        BtnEventBinding();
        view.Lbl_ChatInfo.text = string.Empty;
        SystemSettingModule.Instance.ShieldPlayerEvent += OnShieldPlayer;
    }

    public void UpdateChatInfo(ChatTypeEnum type)
    {
        chatType = type;
        UpdateChatData();
    }

    public void UpdateChatData()
    {
        chatList = new List<ChatInfo>();
        List<ChatInfo> list = null;
        if (chatType == ChatTypeEnum.World)
        {
            list = ChatModule.Instance.worldChatList;
        }
        else if (chatType == ChatTypeEnum.Corps)
        {
            list = ChatModule.Instance.corpsChatList;
        }
        if (list == null || list.Count <= 0)
            return;
        for (int i = list.Count - 1; i >= 0; i--)
        {
            ChatInfo info = list[i];
            if (info == null)
                continue;
            if (chatList.Count >= 4)
            {
                break;
            }
            if (!isBlocked(info.other_accname))
            {
                chatList.Add(info);
            }
        }
        chatList.Reverse();
        UpdateChatLabel();
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

    private void UpdateChatLabel()
    {
        string colorSelf = "[25b01a]";
        string colorTitle = "[bc7c59]";
        string otherColor = "[8c8c8c]";
        string systemColor = "[c41c1c]";
        string content = string.Empty;
        UITextList textList = view.Lbl_ChatInfo.gameObject.GetComponent<UITextList>();
        if (textList == null)
        {
            textList = view.Lbl_ChatInfo.gameObject.AddComponent<UITextList>();
            textList.textLabel = view.Lbl_ChatInfo;
            textList.style = UITextList.Style.Chat;
        }
        textList.Clear();
        for (int i = 0; i < chatList.Count; i++)
        {
            ChatInfo info = chatList[i];
            string message = string.Empty;
            switch (info.chat_type)
            {
                case ChatType.CT_SYSTEM:
                    {
                        message += systemColor + ConstString.CHATVIEW_LABEL_SYSTEMTITLE + ":";
                        switch (chatType)
                        {
                            case ChatTypeEnum.Corps:
                                {
                                    switch (info.union_chat_info.union_type)
                                    {
                                        case UnionType.UT_ADD:
                                            {
                                                message += string.Format(ConstString.CHATVIEW_CONTENT_JOINCORPS, info.union_chat_info.member_charname);
                                            } break;
                                        case UnionType.UT_DROP_OUT:
                                            {
                                                message += string.Format(ConstString.CHATVIEW_CONTENT_EXITCORPS, info.union_chat_info.member_charname);
                                            } break;
                                        case UnionType.UT_EXPEL:
                                            {
                                                if (PlayerData.Instance._AccountName == info.union_chat_info.member_charname)
                                                {
                                                    message += string.Format(ConstString.CHATVIEW_CONTENT_KICKCORPS, ConstString.CHATVIEW_CONTENT_YOU, info.union_chat_info.member_charname);
                                                }
                                                else
                                                {
                                                    message += systemColor + string.Format(ConstString.CHATVIEW_CONTENT_KICKCORPS, info.union_chat_info.member_charname, info.union_chat_info.head_charname);
                                                }
                                            } break;
                                        case UnionType.UT_CHEST:
                                            {
                                                message += info.chat_body;
                                            } break;
                                    }
                                } break;
                            case ChatTypeEnum.World:
                                {
                                    message += info.chat_body;
                                } break;
                        }
                    } break;
                case ChatType.CT_NORMAL:
                    {
                        message += colorTitle + info.char_name + ":[-]";
                        if (PlayerData.Instance._AccountID == info.accid)
                        {
                            message += colorSelf + info.chat_body;
                        }
                        else
                        {
                            message += otherColor + info.chat_body;
                        }
                    } break;
                case ChatType.CT_HELP:
                    {
                        message += colorTitle + info.char_name + ":[-]";
                        message += otherColor + string.Format(ConstString.CHATVIEW_LABEL_HELP, info.host_char_name);
                    }
                    break;
            }
            message += "[-]";
            textList.Add(message);
        }
    }

    private void OnShieldPlayer()
    {
        UpdateChatData();
    }

    public void UpdateNotify(bool status)
    {
        view.Spt_Notify.enabled = status;
    }

    private void ButtonEvent_OpenChat(GameObject go)
    {
        if (GuideManager.Instance.GuideIsRunning())
        {
            return;
        }
        UpdateNotify(false);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CHATVIEW);
        UISystem.Instance.ChatView.UpdateViewInfo(chatType);
    }


    public void BtnEventBinding()
    {
        UIEventListener.Get(view.UIWidget_SimpleChat.gameObject).onClick = ButtonEvent_OpenChat;
    }

    public override void Uninitialize()
    {
        SystemSettingModule.Instance.ShieldPlayerEvent -= OnShieldPlayer;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }



}
