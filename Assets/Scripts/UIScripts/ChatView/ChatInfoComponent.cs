using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using fogs.proto.msg;

public class ChatInfoComponent : BaseComponent
{
    private UILabel Lbl_PlayerName;
    private GameObject Gobj_PlayerLevel;
    private UILabel Lbl_PlayerLevel;
    private UILabel Lbl_SendTip;
    private UISprite Spt_PlayerQuality;
    private UISprite Spt_PlayerIcon;
    private UISprite Spt_PlayerItemBg;
    private GameObject Gobj_VIPGroup;
    private UILabel Lbl_PlayerVIP;
    private UILabel Lbl_SendTime;
    private UILabel Lbl_ChatContent;
    private UISprite Spt_ContentBg;
    private UIButton Btn_Rescue;
    private GameObject Gobj_ItemBaseComp;
    public delegate void PlayerIconHandle(BaseComponent comp);
    public PlayerIconHandle playerIconHandle;
    public delegate void PlayerNameHandle(BaseComponent comp);
    public PlayerNameHandle playerNameHandle;
    public delegate void RescuePlayerHandle(BaseComponent comp);
    public RescuePlayerHandle rescueNameHandle;


    private ChatInfo chatInfo;
    public ChatInfo ChatInfoPOD
    {
        get
        {
            return chatInfo;
        }
    }

    public override void MyStart(GameObject root)
    {
        base.MyStart(root);
        FindWidgetInChild();
        Gobj_VIPGroup.SetActive(false);
        Btn_Rescue.gameObject.SetActive(false);
        UIEventListener.Get(Lbl_PlayerName.gameObject).onClick = NameHandle;
        UIEventListener.Get(Btn_Rescue.gameObject).onClick = RescueHandle;
        UIEventListener.Get(Gobj_ItemBaseComp.gameObject).onClick = IconHandle;
    }

    private void FindWidgetInChild()
    {
        Lbl_PlayerName = mRootObject.transform.FindChild("PlayerName").gameObject.GetComponent<UILabel>();
        Gobj_PlayerLevel = mRootObject.transform.FindChild("LevelBG").gameObject;
        Lbl_PlayerLevel = mRootObject.transform.FindChild("LevelBG/LevelLabel").gameObject.GetComponent<UILabel>();
        Spt_PlayerItemBg = mRootObject.transform.FindChild("ItemBaseComp/PlayerBG").gameObject.GetComponent<UISprite>();
        Spt_PlayerIcon = mRootObject.transform.FindChild("ItemBaseComp/IconTexture").gameObject.GetComponent<UISprite>();
        Spt_PlayerQuality = mRootObject.transform.FindChild("ItemBaseComp/QualitySprite").gameObject.GetComponent<UISprite>();
        Gobj_VIPGroup = mRootObject.transform.FindChild("VIPGroup").gameObject;
        Lbl_PlayerVIP = mRootObject.transform.FindChild("VIPGroup/PlayerVIP").gameObject.GetComponent<UILabel>();
        Lbl_SendTime = mRootObject.transform.FindChild("SendTime").gameObject.GetComponent<UILabel>();
        Lbl_ChatContent = mRootObject.transform.FindChild("ChatContent").gameObject.GetComponent<UILabel>();
        Spt_ContentBg = mRootObject.transform.FindChild("ContentBg").gameObject.GetComponent<UISprite>();
        Btn_Rescue = mRootObject.transform.FindChild("Rescue").gameObject.GetComponent<UIButton>();
        Lbl_SendTip = mRootObject.transform.FindChild("SendTip").gameObject.GetComponent<UILabel>();
        Gobj_ItemBaseComp = mRootObject.transform.FindChild("ItemBaseComp").gameObject;
    }

    public void UpdateCompInfo(ChatTypeEnum chatType, ChatInfo pod)
    {
        Lbl_SendTip.enabled = false;
        chatInfo = pod;
        if (chatInfo == null)
        {
            Clear();
            return;
        }
        UpdateTime(chatInfo.send_time);
        if (chatInfo.accid == PlayerData.Instance._AccountID)
        {
            UpdateVIPInfo(PlayerData.Instance._VipLv);
        }
        else
        {
            UpdateVIPInfo(chatInfo.vip_level);
        }
        Spt_ContentBg.width = 500;
        bool isPrivateAndSendTo = false;
        switch (chatType)
        {
            case ChatTypeEnum.Private:
                {
                    Lbl_ChatContent.color = Color.white;
                    //Lbl_ChatContent.text = chatInfo.chat_body;
                    if (chatInfo.send_type == 1) //1表示自己发给其他人，2表示别人发给自己
                    {
                        //Lbl_SendTip.text = ConstString.CHATVIEW_LABEL_SENDTO;
                        //Lbl_SendTip.enabled = true;
                        //Lbl_ChatContent.color = Color.green;
                        isPrivateAndSendTo = true;
                    }
                    else
                    {
                        //Lbl_SendTip.enabled = false;
                        //CommonFunction.SetLabelColor_I(Lbl_ChatContent, 159, 138, 113);
                    }
                }
                break;
            case ChatTypeEnum.Corps:
            case ChatTypeEnum.World:
                Lbl_ChatContent.color = Color.white;
                //Lbl_ChatContent.text = chatInfo.chat_body;
                if (PlayerData.Instance._AccountID == chatInfo.accid)
                {
                    //Lbl_ChatContent.color = Color.green;
                }
                else
                {

                    //CommonFunction.SetLabelColor_I(Lbl_ChatContent, 159, 138, 113);
                }
                Lbl_SendTip.enabled = false;
                break;
        }
        UpdateChatContent(chatType, isPrivateAndSendTo, chatInfo);
        UpdatePlayerInfo();
    }

    private void UpdateChatContent(ChatTypeEnum chatType, bool isPrivateAndSendTo, ChatInfo info)
    {
        string colorSelf = "[25b01a]";
        string colorTitle = "[bc7c59]";
        string otherColor = "[8c8c8c]";
        string systemColor = "[c41c1c]";
        switch (chatInfo.chat_type)
        {
            case ChatType.CT_NORMAL:
                {
                    Lbl_ChatContent.width = 470;
                    Btn_Rescue.gameObject.SetActive(false);
                    if (isPrivateAndSendTo)
                    {
                        Lbl_SendTip.text = ConstString.CHATVIEW_LABEL_SENDTO;
                        Lbl_SendTip.enabled = true;
                        Lbl_PlayerName.text = colorTitle + string.Format(ConstString.CHATVIEW_LABEL_SENDTO, chatInfo.char_name) + "[-]";
                        Gobj_VIPGroup.transform.localPosition = new Vector3(100, 16, 0);
                        Lbl_ChatContent.text = colorSelf + info.chat_body + "[-]";
                    }
                    else
                    {
                        Lbl_PlayerName.text = colorTitle + chatInfo.char_name + "[-]";
                        Gobj_VIPGroup.transform.localPosition = new Vector3(55, 16, 0);
                        if (PlayerData.Instance._AccountID == chatInfo.accid)
                        {
                            Lbl_ChatContent.text = colorSelf + info.chat_body + "[-]";
                        }
                        else
                        {
                            Lbl_ChatContent.text = otherColor + info.chat_body + "[-]";
                        }
                    }
                }
                break;
            case ChatType.CT_SYSTEM:
                {
                    Lbl_ChatContent.width = 470;
                    Btn_Rescue.gameObject.SetActive(false);
                    Lbl_PlayerName.text = systemColor + ConstString.CHATVIEW_LABEL_SYSTEMTITLE + "[-]";
                    switch (chatType)
                    {
                        case ChatTypeEnum.Corps:
                            {
                                switch (info.union_chat_info.union_type)
                                {
                                    case UnionType.UT_ADD:
                                        {
                                            Lbl_ChatContent.text = systemColor + string.Format(ConstString.CHATVIEW_CONTENT_JOINCORPS, info.union_chat_info.member_charname) + "[-]";
                                        } break;
                                    case UnionType.UT_DROP_OUT:
                                        {
                                            Lbl_ChatContent.text = systemColor + string.Format(ConstString.CHATVIEW_CONTENT_EXITCORPS, info.union_chat_info.member_charname) + "[-]";
                                        } break;
                                    case UnionType.UT_EXPEL:
                                        {
                                            if (PlayerData.Instance._AccountName == info.union_chat_info.member_charname)
                                            {
                                                Lbl_ChatContent.text = systemColor + string.Format(ConstString.CHATVIEW_CONTENT_KICKCORPS, ConstString.CHATVIEW_CONTENT_YOU, info.union_chat_info.member_charname) + "[-]";
                                            }
                                            else
                                            {
                                                Lbl_ChatContent.text = systemColor + string.Format(ConstString.CHATVIEW_CONTENT_KICKCORPS, info.union_chat_info.member_charname, info.union_chat_info.head_charname) + "[-]";
                                            }
                                        } break;
                                    case UnionType.UT_CHEST:
                                        {
                                            Lbl_ChatContent.text = systemColor + info.chat_body + "[-]";
                                        } break;
                                }
                            } break;
                        case ChatTypeEnum.Private:
                        case ChatTypeEnum.World:
                        default:
                            {
                                Lbl_ChatContent.text = systemColor + info.chat_body + "[-]";
                            }
                            break;
                    }
                }
                break;
            case ChatType.CT_HELP:
                {
                    Lbl_ChatContent.width = 315;
                    Gobj_VIPGroup.transform.localPosition = new Vector3(55, 16, 0);
                    Btn_Rescue.gameObject.SetActive(true);
                    Lbl_PlayerName.text = colorTitle + chatInfo.char_name + "[-]";
                    Spt_ContentBg.width = 343;
                    Lbl_ChatContent.text = otherColor + string.Format(ConstString.CHATVIEW_LABEL_HELP, info.host_char_name) + "[-]";
                }
                break;
        }
    }

    //private void UpdateContentBG() 
    //{
    //    Spt_ContentBg.height = Lbl_ChatContent.height + 50;
    //}

    private void UpdatePlayerInfo()
    {
        if (chatInfo.chat_type == ChatType.CT_SYSTEM)
        {
            Gobj_PlayerLevel.SetActive(false);
            CommonFunction.SetHeadAndFrameSprite(Spt_PlayerIcon, Spt_PlayerQuality, GlobalConst.SYSTEMICONID, GlobalConst.SYSTEMFRAME, true);
        }
        else if (chatInfo.accid == PlayerData.Instance._AccountID)
        {
            Gobj_PlayerLevel.SetActive(true);
            CommonFunction.SetHeadAndFrameSprite(Spt_PlayerIcon, Spt_PlayerQuality, PlayerData.Instance.HeadID, PlayerData.Instance.FrameID, true);
            Lbl_PlayerLevel.text = PlayerData.Instance._Level.ToString();
        }
        else
        {
            Gobj_PlayerLevel.SetActive(true);
            CommonFunction.SetHeadAndFrameSprite(Spt_PlayerIcon, Spt_PlayerQuality, chatInfo.icon, chatInfo.icon_frame, true);
            Lbl_PlayerLevel.text = chatInfo.player_level.ToString();
        }
    }


    private void UpdateVIPInfo(uint vipLV)
    {
        if (vipLV <= 0)
        {
            Gobj_VIPGroup.SetActive(false);
        }
        else
        {
            Gobj_VIPGroup.SetActive(true);
            Lbl_PlayerVIP.text = vipLV.ToString();
        }
    }

    private void NameHandle(GameObject go)
    {
        if (playerNameHandle != null)
        {
            playerNameHandle(this);
        }
    }
    private void IconHandle(GameObject go)   //TODO:需要奴役系统配合 
    {
        if (playerIconHandle != null)
            playerIconHandle(this);
    }
    private void RescueHandle(GameObject go)   //TODO:需要奴役系统配合 
    {
        if (rescueNameHandle != null)
            rescueNameHandle(this);
    }

    private void UpdateColliderSize()
    {
        Spt_ContentBg.height = Lbl_ChatContent.height + 70;
        BoxCollider mCollier = mRootObject.collider as BoxCollider;
        if (mCollier == null)
        {
            mCollier = mRootObject.AddComponent<BoxCollider>();
        }
        mCollier.size = new Vector3(mCollier.size.x, Spt_ContentBg.height - 10, 0);
    }

    private void UpdateContent(string content)
    {
        Lbl_ChatContent.text = content;
        if (chatInfo.send_type == 1)
        {
            Lbl_ChatContent.color = Color.green;
        }
        else
        {
            CommonFunction.SetLabelColor_I(Lbl_ChatContent, 159, 138, 113);
        }
    }

    private void UpdateTime(long time)
    {
        DateTime sendDT = CommonFunction.GetTimeByLong(time);
        //Lbl_SendTime.text = sendDT.ToString("MM:dd:hh:mm:ss");
        DateTime currentDT = CommonFunction.GetTimeByLong(Main.mTime);
        TimeSpan l_TimeSpan = currentDT - sendDT;
        if (l_TimeSpan.Days != 0)
        {
            Lbl_SendTime.text = sendDT.ToString("MM月dd日", System.Globalization.DateTimeFormatInfo.InvariantInfo);//.GetDateTimeFormats('M')[0].ToString();//.ToString("MM:dd");
        }
        else
        {
            Lbl_SendTime.text = sendDT.ToString("HH:mm");
        }
    }

    public override void Clear()
    {
        base.Clear();
        Lbl_PlayerName.text = string.Empty;
        Lbl_PlayerLevel.text = string.Empty;
        Lbl_PlayerVIP.text = string.Empty;
        Lbl_SendTime.text = string.Empty;
        Lbl_ChatContent.text = string.Empty;
    }
}
