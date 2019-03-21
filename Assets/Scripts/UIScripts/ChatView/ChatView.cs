using UnityEngine;
using System;
using System.Collections;

public class ChatView
{
    public static string UIName = "ChatView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskSprite;
    public GameObject Gobj_Chat;
    public UIButton Btn_WroldChat;
    public UIButton Btn_CorpsChat;
    public UIButton Btn_PrivateChat;
    public UIScrollView ScrView_ContentScrollView;
    public UITable UITable_Table;
    public GameObject Gobj_ContentComp;
    public UIButton Btn_CloseButton;

    public GameObject Gobj_SendWorldChat;
    public UIInput Ipt_WorldInput;
    public UILabel Lbl_WroldFreeTip;
    public UIButton Btn_SendChat;
    public UISprite Spt_BtnSendChat;
    public UILabel Lbl_BtnSendChat;
    public GameObject Gobj_SendCorpsChat;
    public UIInput Ipt_CorpsInput;
    public GameObject Gobj_SendPrivateChat;
    public UIInput Ipt_PrivateInput;
    public UILabel Lbl_FriendName;

    public GameObject Gobj_FunctionPanel;
    public UISprite Spt_FuncMask;
    public GameObject Gobj_FunctionGroup;
    public UIGrid Grd_ButtonGroup;
    public UISprite Spt_FunctionBG;
    public UILabel Lbl_PlayerName;
    public UIButton Btn_CheckInfo;
    public UILabel Lbl_BtnCheckInfoLabel;
    public UIButton Btn_LaunchChat;
    public UILabel Lbl_BtnLaunchChatLabel;
    public UIButton Btn_ShieldPlayer;
    public UILabel Lbl_BtnShieldPlayerLabel;
    public UIButton Btn_CropsInvite;
    public UILabel Lbl_BtnCropsInviteLabel;
    public UIButton Btn_Enslaved;
    public UILabel Lbl_BtnEnslavedLabel;
    public UIButton Btn_Rescue;
    public UILabel Lbl_BtnRescueLabel;
    public GameObject Gobj_PurchaseSend;
    public UILabel Lbl_PurchaseSend;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/ChatView");
        Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Gobj_Chat = _uiRoot.transform.FindChild("gobj_Chat").gameObject;
        Btn_WroldChat = _uiRoot.transform.FindChild("gobj_Chat/TypeGroup/WroldChat").gameObject.GetComponent<UIButton>();
        Btn_CorpsChat = _uiRoot.transform.FindChild("gobj_Chat/TypeGroup/CorpsChat").gameObject.GetComponent<UIButton>();
        Btn_PrivateChat = _uiRoot.transform.FindChild("gobj_Chat/TypeGroup/PrivateChat").gameObject.GetComponent<UIButton>();
        ScrView_ContentScrollView = _uiRoot.transform.FindChild("gobj_Chat/ContentGroup/ContentScrollView").gameObject.GetComponent<UIScrollView>();
        UITable_Table = _uiRoot.transform.FindChild("gobj_Chat/ContentGroup/ContentScrollView/Table").gameObject.GetComponent<UITable>();
        Gobj_ContentComp = _uiRoot.transform.FindChild("gobj_Chat/ContentGroup/ContentScrollView/Table/gobj_ContentComp").gameObject;
        Btn_CloseButton = _uiRoot.transform.FindChild("gobj_Chat/CloseButton").gameObject.GetComponent<UIButton>();
        Gobj_SendWorldChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendWorldChat").gameObject;
        Ipt_WorldInput = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendWorldChat/WorldInput").gameObject.GetComponent<UIInput>();
        Lbl_WroldFreeTip = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendWorldChat/WroldFreeTip").gameObject.GetComponent<UILabel>();
        Btn_SendChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/SendChat").gameObject.GetComponent<UIButton>();
        Spt_BtnSendChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/SendChat/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnSendChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/SendChat/SendChat").gameObject.GetComponent<UILabel>();
        Gobj_SendCorpsChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendCorpsChat").gameObject;
        Ipt_CorpsInput = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendCorpsChat/CorpsInput").gameObject.GetComponent<UIInput>();
        Gobj_SendPrivateChat = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendPrivateChat").gameObject;
        Ipt_PrivateInput = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendPrivateChat/PrivateInput").gameObject.GetComponent<UIInput>();
        Lbl_FriendName = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendPrivateChat/FriendGroup/FriendName").gameObject.GetComponent<UILabel>();
        Gobj_FunctionPanel = _uiRoot.transform.FindChild("gobj_FunctionPanel").gameObject;
        Spt_FuncMask = _uiRoot.transform.FindChild("gobj_FunctionPanel/FuncMask").gameObject.GetComponent<UISprite>();
        Grd_ButtonGroup = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup").gameObject.GetComponent<UIGrid>();
        Spt_FunctionBG = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/FunctionBG").gameObject.GetComponent<UISprite>();
        Lbl_PlayerName = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/PlayerName").gameObject.GetComponent<UILabel>();
        Btn_CheckInfo = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/CheckInfo").gameObject.GetComponent<UIButton>();
        Lbl_BtnCheckInfoLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/CheckInfo/Label").gameObject.GetComponent<UILabel>();
        Btn_LaunchChat = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/LaunchChat").gameObject.GetComponent<UIButton>();
        Lbl_BtnLaunchChatLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/LaunchChat/Label").gameObject.GetComponent<UILabel>();
        Btn_ShieldPlayer = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/ShieldPlayer").gameObject.GetComponent<UIButton>();
        Lbl_BtnShieldPlayerLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/ShieldPlayer/Label").gameObject.GetComponent<UILabel>();
        Btn_CropsInvite = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/CropsInvite").gameObject.GetComponent<UIButton>();
        Lbl_BtnCropsInviteLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/CropsInvite/Label").gameObject.GetComponent<UILabel>();
        Btn_Enslaved = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/Enslaved").gameObject.GetComponent<UIButton>();
        Lbl_BtnEnslavedLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/Enslaved/Label").gameObject.GetComponent<UILabel>();
        Btn_Rescue = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/Rescue").gameObject.GetComponent<UIButton>();
        Lbl_BtnRescueLabel = _uiRoot.transform.FindChild("gobj_FunctionPanel/gobj_FunctionGroup/ButtonGroup/Rescue/Label").gameObject.GetComponent<UILabel>();
        Gobj_PurchaseSend = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendWorldChat/gobj_PurchaseSend").gameObject;
        Lbl_PurchaseSend = _uiRoot.transform.FindChild("gobj_Chat/ButtomGroup/gobj_SendWorldChat/gobj_PurchaseSend/PurchaseSend").gameObject.GetComponent<UILabel>();       
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_BtnWroldChatDescSprite.text = "世界";
        //Lbl_BtnCorpsChatDescSprite.text = "军团";
        //Lbl_BtnPrivateChatDescSprite.text = "私聊";
        //Lbl_PlayerName.text = "六个字六个字";
        //Lbl_PlayerLevel.text = "发送给";
        //Lbl_SendTime.text = "11月11日  20:20:20";
        //Lbl_PlayerVIP.text = "Vip8";
        //Lbl_ChatContent.text = "发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给发送给000000000000000000000000000000000000000000000000000000000";
        //Lbl_InputWorldInputWorldChatMsg.text = "点击输入，字数限制50字";
        Lbl_WroldFreeTip.text = "";
        Lbl_BtnSendChat.text = ConstString.CHATVIEW_LABEL_SEND;
        //Lbl_InputCorpsInputCorpsChatMsg.text = "点击输入，字数限制50字";
        //Lbl_InputPrivateInputPrivateChatMsg.text = "点击输入，字数限制50字";
        Lbl_FriendName.text = ConstString.CHATVIEW_LABEL_SELECTCHATPLAYER;
        Lbl_PlayerName.text = string.Empty;
        Lbl_BtnCheckInfoLabel.text = ConstString.CHATVIEW_LABEL_CHECK_INFO;
        Lbl_BtnLaunchChatLabel.text = ConstString.CHATVIEW_LABEL_LAUNCH_CHAT;
        Lbl_BtnShieldPlayerLabel.text = ConstString.CHATVIEW_LABEL_SHIELD_PLAYER;
        Lbl_BtnCropsInviteLabel.text = ConstString.CHATVIEW_LABEL_UNION_INVITE;
        Lbl_BtnEnslavedLabel.text = ConstString.CHATVIEW_LABEL_ENSLAVE;
        Lbl_BtnRescueLabel.text = ConstString.CHATVIEW_LABEL_RESCUE;
    }

    public void Uninitialize()
    {

    }
}
