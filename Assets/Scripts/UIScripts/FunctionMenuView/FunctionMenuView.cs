using UnityEngine;
using System;
using System.Collections;

public class FunctionMenuView
{
    public static string UIName ="FunctionMenuView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_FunctionMenuView;
    public UISprite Spt_FuncMask;
    public GameObject Gobj_FunctionGroup;
    public UIGrid Grd_ButtonGroup;
    public UIButton Btn_CheckInfo;
    public UILabel Lbl_BtnCheckInfoLabel;
    public UISprite Spt_BtnCheckInfoBackground;
    public UIButton Btn_LaunchChat;
    public UISprite Spt_BtnLaunchChatBackground;
    public UILabel Lbl_BtnLaunchChatLabel;
    public UIButton Btn_AddFriend;
    public UISprite Spt_BtnAddFriendBackground;
    public UILabel Lbl_BtnAddFriendLabel;
    public UIButton Btn_DeleteFriend;
    public UISprite Spt_BtnDeleteFriendBackground;
    public UILabel Lbl_BtnDeleteFriendLabel;
    public UIButton Btn_ShieldPlayer;
    public UISprite Spt_BtnShieldPlayerBackground;
    public UILabel Lbl_BtnShieldPlayerLabel;
    public UIButton Btn_CropsInvite;
    public UISprite Spt_BtnCropsInviteBackground;
    public UILabel Lbl_BtnCropsInviteLabel;
    public UIButton Btn_Enslaved;
    public UISprite Spt_BtnEnslavedBackground;
    public UILabel Lbl_BtnEnslavedLabel;
    public UIButton Btn_Rescue;
    public UISprite Spt_BtnRescueBackground;
    public UILabel Lbl_BtnRescueLabel;
    public UILabel Lbl_PlayerName;
    public UISprite Spt_FunctionBG;

    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/FunctionMenuView");
        UIPanel_FunctionMenuView = _uiRoot.GetComponent<UIPanel>();
        Spt_FuncMask = _uiRoot.transform.FindChild("FuncMask").gameObject.GetComponent<UISprite>();
        Gobj_FunctionGroup = _uiRoot.transform.FindChild("gobj_FunctionGroup").gameObject;
        Grd_ButtonGroup = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup").gameObject.GetComponent<UIGrid>();
        Btn_CheckInfo = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CheckInfo").gameObject.GetComponent<UIButton>();
        Lbl_BtnCheckInfoLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CheckInfo/Label").gameObject.GetComponent<UILabel>();
        Spt_BtnCheckInfoBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CheckInfo/Background").gameObject.GetComponent<UISprite>();
        Btn_LaunchChat = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/LaunchChat").gameObject.GetComponent<UIButton>();
        Spt_BtnLaunchChatBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/LaunchChat/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnLaunchChatLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/LaunchChat/Label").gameObject.GetComponent<UILabel>();
        Btn_AddFriend = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/AddFriend").gameObject.GetComponent<UIButton>();
        Spt_BtnAddFriendBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/AddFriend/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnAddFriendLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/AddFriend/Label").gameObject.GetComponent<UILabel>();
        Btn_DeleteFriend = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/DeleteFriend").gameObject.GetComponent<UIButton>();
        Spt_BtnDeleteFriendBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/DeleteFriend/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnDeleteFriendLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/DeleteFriend/Label").gameObject.GetComponent<UILabel>();
        Btn_ShieldPlayer = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/ShieldPlayer").gameObject.GetComponent<UIButton>();
        Spt_BtnShieldPlayerBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/ShieldPlayer/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnShieldPlayerLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/ShieldPlayer/Label").gameObject.GetComponent<UILabel>();
        Btn_CropsInvite = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CropsInvite").gameObject.GetComponent<UIButton>();
        Spt_BtnCropsInviteBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CropsInvite/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCropsInviteLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/CropsInvite/Label").gameObject.GetComponent<UILabel>();
        Btn_Enslaved = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Enslaved").gameObject.GetComponent<UIButton>();
        Spt_BtnEnslavedBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Enslaved/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnEnslavedLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Enslaved/Label").gameObject.GetComponent<UILabel>();
        Btn_Rescue = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Rescue").gameObject.GetComponent<UIButton>();
        Spt_BtnRescueBackground = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Rescue/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRescueLabel = _uiRoot.transform.FindChild("gobj_FunctionGroup/ButtonGroup/Rescue/Label").gameObject.GetComponent<UILabel>();
        Lbl_PlayerName = _uiRoot.transform.FindChild("gobj_FunctionGroup/PlayerName").gameObject.GetComponent<UILabel>();
        Spt_FunctionBG = _uiRoot.transform.FindChild("gobj_FunctionGroup/FunctionBG").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_BtnCheckInfoLabel.text = "查看信息";
        Lbl_BtnLaunchChatLabel.text = "发起私聊";
        Lbl_BtnAddFriendLabel.text = "添加好友";
        Lbl_BtnDeleteFriendLabel.text = "删除好友";
        Lbl_BtnShieldPlayerLabel.text = "屏蔽玩家";
        Lbl_BtnCropsInviteLabel.text = "军团邀请";
        Lbl_BtnEnslavedLabel.text = "发起奴役";
        Lbl_BtnRescueLabel.text = "发起解救";
        Lbl_PlayerName.text = "六个字六个字";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
