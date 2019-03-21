using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

public class ChatModule : Singleton<ChatModule>
{
    public ChatNetWork _ChatNetWork;
    /// <summary>
    /// 世界聊天
    /// </summary>
    public List<ChatInfo> worldChatList;
    /// <summary>
    /// 军团聊天
    /// </summary>
    public List<ChatInfo> corpsChatList;
    /// <summary>
    /// 私聊
    /// </summary>
    public List<ChatInfo> privateChatList;

    private System.Action<ArenaPlayer> callBack;

    public void Initialize()
    {
        if (_ChatNetWork == null)
        {
            _ChatNetWork = new ChatNetWork();
        }
        _ChatNetWork.RegisterMsg();
        worldChatList = new List<ChatInfo>();
        privateChatList = new List<ChatInfo>();
        corpsChatList = new List<ChatInfo>();
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
        if (list.Count > GlobalCoefficient.ServerChatCountLimit)
        {
            List<ChatInfo> infos = new List<ChatInfo>();
            int index = list.Count - GlobalCoefficient.ServerChatCountLimit;
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

    public void UpdateWorldChat(List<ChatInfo> list)
    {
        if (worldChatList == null)
        {
            worldChatList = new List<ChatInfo>();
        }
        worldChatList.AddRange(list);
        SortAndRemoveMsg(worldChatList);
    }

    public void UpdateWorldChat(ChatInfo list)
    {
        if (worldChatList == null)
        {
            worldChatList = new List<ChatInfo>();
        }
        worldChatList.Add(list);
        SortAndRemoveMsg(worldChatList);
    }

    public void UpdateCorpsChat(List<ChatInfo> list)
    {
        if (corpsChatList == null)
        {
            corpsChatList = new List<ChatInfo>();
        }
        corpsChatList.AddRange(list);
        SortAndRemoveMsg(corpsChatList);
    }
    public void UpdateCorpsChat(ChatInfo list)
    {
        if (corpsChatList == null)
        {
            corpsChatList = new List<ChatInfo>();
        }
        corpsChatList.Add(list);
        SortAndRemoveMsg(corpsChatList);
    }

    public void UpdatePrivateChat(List<ChatInfo> list)
    {
        if (privateChatList == null)
        {
            privateChatList = new List<ChatInfo>();
        }
        privateChatList.AddRange(list);
        SortAndRemoveMsg(privateChatList);
    }

    public void UpdatePrivateChat(ChatInfo list)
    {
        if (privateChatList == null)
        {
            privateChatList = new List<ChatInfo>();
        }
        privateChatList.Add(list);
        SortAndRemoveMsg(privateChatList);
    }

    public void SendChat(string content, uint type, uint id, uint icon, uint icon_frame, uint level, uint vipLevel, string name, string accname, uint enslave)
    {
        SendChatMessageReq data = new SendChatMessageReq();
        data.accid = id;
        data.name = name;
        data.type = type;
        data.icon = icon;
        data.icon_frame = icon_frame;
        data.chat_body = content;
        data.accname = accname;
        data.player_level = level;
        data.player_vip_level = vipLevel;
        data.enslave = enslave;
        _ChatNetWork.SendChat(data);
    }

    public void SendChat(string content, uint type, uint id = 0)
    {
        SendChatMessageReq data = new SendChatMessageReq();
        data.accid = id;
        data.type = type;
        data.chat_body = content;
        _ChatNetWork.SendChat(data);
    }

    public void ReceiveChat(SendChatMessageResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            switch ((ChatTypeEnum)data.type)
            {
                case ChatTypeEnum.World:
                    {
                        //PlayerData.Instance.WorldChatCDTime = data.next_time;
                        long worldCD = 5;
                        if (!long.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SPECIALID_CHAT_WORLDCHATCD), out worldCD))
                        {
                            worldCD = 5;
                        }
                        if ((data.next_time - Main.mTime) > worldCD)
                        {
                            PlayerData.Instance.WorldChatCDTime = Main.mTime + worldCD;
                        }
                        else
                        {
                            PlayerData.Instance.WorldChatCDTime = data.next_time;
                        }
                        PlayerData.Instance.UpdateDiamond((int)data.diamond);
                        PlayerData.Instance.WorldChatCount = data.surplus_num;
                    }
                    break;
                case ChatTypeEnum.Private:
                    break;
                case ChatTypeEnum.Corps:
                    break;
            }
            PlayerData.Instance.WorldSendTip = data.chat_by_diamond;
            UISystem.Instance.ChatView.SendChatSuccess();
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendGetPlayerInfo(uint areaID, string accName, System.Action<ArenaPlayer> action)
    {
        callBack = action;
        GetPlayerInfoReq data = new GetPlayerInfoReq();
        data.area_id = areaID;
        data.accname = accName;
        _ChatNetWork.SendGetPlayerInfo(data);
    }

    public void ReceviePlayerInfo(GetPlayerInfoResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            if (callBack != null)
            {
                callBack(data.palyer_info);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    public void SendReadChatInfo(ChatTypeEnum type)
    {
        ReadChatInfoReq data = new ReadChatInfoReq();
        data.type = (uint)type;
        _ChatNetWork.SendReadChatInfo(data);
    }

    public void ReceiveReadChatInfo(ReadChatInfoResp data)
    {
        if (data.result == (int)ErrorCodeEnum.SUCCESS)
        {
            switch ((ChatTypeEnum)data.type)
            {
                case ChatTypeEnum.World:
                    UpdateChatInfoReadStatus(worldChatList, data.id_list);
                    break;
                case ChatTypeEnum.Private:
                    UpdateChatInfoReadStatus(privateChatList, data.id_list);
                    break;
                case ChatTypeEnum.Corps:                    
                    UpdateChatInfoReadStatus(corpsChatList, data.id_list);
                    break;
            }
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_CHATVIEW))
            {
                UISystem.Instance.ChatView.UpdateChatReadStatus((ChatTypeEnum)data.type, data.id_list);
            }
        }
        else
        {
            ErrorCode.ShowErrorTip(data.result);
        }
    }

    private void UpdateChatInfoReadStatus(List<ChatInfo> chatList, List<ulong> IDList)
    {
        if (chatList == null) return;
        for (int i = 0; i < chatList.Count; i++)
        {
            chatList[i].status = 2;
        }
        
        //if (chatList == null || IDList == null) return;
        //for (int i = 0; i < chatList.Count; i++)
        //{
        //    ChatInfo info = chatList[i];
        //    if (info == null) continue;
        //    if (IDList.Contains(info.chat_id))
        //    {
        //        info.status = 2;
        //    }
        //}
    }

    public void NotifyChatRefresh(NotifyChatRefresh data)
    {
        switch ((ChatTypeEnum)data.chat_type)
        {
            case ChatTypeEnum.World:
                {
                    UpdateWorldChat(data.chat_info);
                }
                break;
            case ChatTypeEnum.Corps:
                {
                    UpdateCorpsChat(data.chat_info);
                }
                break;
            case ChatTypeEnum.Private:
                {
                    UpdatePrivateChat(data.chat_info);
                }
                break;
        }
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_CHATVIEW))
        {
            UISystem.Instance.ChatView.NotifyChatRefresh(data.chat_type, data.chat_info);
        }
        //Debug.LogError("data " + data.chat_type.ToString() + " " + data.chat_info.status
        //    + " send type=" + data.chat_info.send_type + " " + data.chat_info.chat_body);
        
        if (IsMsgNew(data.chat_info,(ChatTypeEnum)data.chat_type))
        {
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_MAINCITY))
            {
                UISystem.Instance.MainCityView.ShowChat(true);
            }
            if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_SIMPLECHAT))
            {
                UISystem.Instance.SimpleChatView.UpdateNotify(true);
            }            
        }
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_SIMPLECHAT))
        {
            UISystem.Instance.SimpleChatView.UpdateChatData();
        }
    }

    public bool IsMsgNew(ChatInfo info,ChatTypeEnum type)
    {
        return (info.status == 1 &&
            (
            (type == ChatTypeEnum.Corps && info.accid != PlayerData.Instance._AccountID)
            || (type == ChatTypeEnum.Private && info.send_type == 2)
            )
        );
    }

    public void Uninitialize()
    {
        if (_ChatNetWork != null)
            _ChatNetWork.RemoveMsg();
        privateChatList.Clear();
        corpsChatList.Clear();
        worldChatList.Clear();
        _ChatNetWork = null;
    }
}