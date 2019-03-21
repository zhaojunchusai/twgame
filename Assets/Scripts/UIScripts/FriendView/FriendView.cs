using UnityEngine;
using System;
using System.Collections;

public class FriendView
{
    public static string UIName ="FriendView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_FriendView;
    public UIToggle Tog_TabFriend;
    public UIToggle Tog_TabFBFriend;
    public UIToggle Tog_TabFriendMisson;
    public UIToggle Tog_TabWelcome;
    public UIButton Btn_Close;

    public class FriendGroup
    {
        public UILabel FrienGroup_Lbl_Label_FriendCount;
        public UILabel FrienGroup_Lbl_Label_FreeCount;
        public UIButton FrienGroup_Btn_OneKeyGive;
        public UIButton FrienGroup_Btn_OneKeyGet;
        public UIButton FrienGroup_Btn_FriendRequest;
        public UISprite FrienGroup_Spt_BtnFriendRequestNotice;
        public UIButton FrienGroup_Btn_FriendAdd;
        public UIScrollView FrienGroup_ScrView_FriendList;
        public UIGrid FrienGroup_Grd_Grid;
        public UIWrapContent FrienGroup_UIWrapContent_Grid;
        public GameObject FriendGroup_Item;
        public GameObject FriendPrompt;
    }
    public FriendGroup friendGroup = new FriendGroup();

    public class InviteCodeGroup
    {
        public UIInput CodeInput;
        public UIButton Btn_Ok;
        public UIButton Btn_Share;
        public UIScrollView MissionScrollView;
        public UIGrid MissionGrid;
        public UIWrapContent MissionWrapContent;
        public GameObject MissionItem;
        public UILabel Lbl_Share;
        public UILabel Title;
    }
    public InviteCodeGroup inviteCodeGroup = new InviteCodeGroup();

    public class FriendMissionGroup
    {
        public UIButton FriendShare;
        public UILabel FB_Title;
        public UIScrollView FB_ScrollView;
        public UIWrapContent FB_WrapContent;
        public UIGrid FB_Grid;
        public GameObject FB_Item;

        public UILabel LINE_Title;
        public UIScrollView LINE_ScrollView;
        public UIWrapContent LINE_WrapContent;
        public UIGrid LINE_Grid;
        public GameObject LINE_Item;

    }
    public FriendMissionGroup friendMissionGroup = new FriendMissionGroup();

    public class FBFriendGRroup
    {
        public UILabel Lbl_Label_FriendCount;
        public UILabel Lbl_Label_FreeCount;
        public UIScrollView ScrView_FriendList;
        public UIGrid Grd_Grid;
        public UIWrapContent UIWrapContent_Grid;
        public GameObject Item;
    }
    public FBFriendGRroup fbFriendGroup = new FBFriendGRroup();

    public class PlatformGroup
    {
        public GameObject PlatformGroupBack;
        public UIButton Btn_FBFriend;
        public UIButton Btn_LINEFriend;
        public GameObject Mask;
    }
    public PlatformGroup platformGroup = new PlatformGroup();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/FriendView");

        Tog_TabFriend = _uiRoot.transform.FindChild("ToggleGroup/TabFriend").gameObject.GetComponent<UIToggle>();
        Tog_TabFBFriend = _uiRoot.transform.FindChild("ToggleGroup/TabFBFriend").gameObject.GetComponent<UIToggle>();
        Tog_TabFriendMisson = _uiRoot.transform.FindChild("ToggleGroup/TabFriendMisson").gameObject.GetComponent<UIToggle>();
        Btn_Close = _uiRoot.transform.FindChild("Close").gameObject.GetComponent<UIButton>();

        Tog_TabWelcome = _uiRoot.transform.FindChild("ToggleGroup/TabWelcome").gameObject.GetComponent<UIToggle>();
 
        friendGroup.FrienGroup_Lbl_Label_FriendCount = _uiRoot.transform.FindChild("FrienGroup/Label_FriendCount").gameObject.GetComponent<UILabel>();
        friendGroup.FrienGroup_Lbl_Label_FreeCount = _uiRoot.transform.FindChild("FrienGroup/Label_FreeCount").gameObject.GetComponent<UILabel>();
        friendGroup.FrienGroup_Btn_OneKeyGive = _uiRoot.transform.FindChild("FrienGroup/Btns/OneKeyGive").gameObject.GetComponent<UIButton>();
        friendGroup.FrienGroup_Btn_OneKeyGet = _uiRoot.transform.FindChild("FrienGroup/Btns/OneKeyGet").gameObject.GetComponent<UIButton>();
        friendGroup.FrienGroup_Btn_FriendRequest = _uiRoot.transform.FindChild("FrienGroup/Btns/FriendRequest").gameObject.GetComponent<UIButton>();
        friendGroup.FrienGroup_Spt_BtnFriendRequestNotice = _uiRoot.transform.FindChild("FrienGroup/Btns/FriendRequest/Notice").gameObject.GetComponent<UISprite>();
        friendGroup.FrienGroup_Btn_FriendAdd = _uiRoot.transform.FindChild("FrienGroup/Btns/FriendAdd").gameObject.GetComponent<UIButton>();
        friendGroup.FrienGroup_ScrView_FriendList = _uiRoot.transform.FindChild("FrienGroup/FriendList").gameObject.GetComponent<UIScrollView>();
        friendGroup.FrienGroup_Grd_Grid = _uiRoot.transform.FindChild("FrienGroup/FriendList/Grid").gameObject.GetComponent<UIGrid>();
        friendGroup.FrienGroup_UIWrapContent_Grid = _uiRoot.transform.FindChild("FrienGroup/FriendList/Grid").gameObject.GetComponent<UIWrapContent>();
        friendGroup.FriendGroup_Item = _uiRoot.transform.FindChild("FrienGroup/FriendList/FriendMemberItem").gameObject;
        friendGroup.FriendPrompt = _uiRoot.transform.FindChild("FrienGroup/Btns/FriendRequest/Notice").gameObject;
        friendGroup.FriendGroup_Item.SetActive(false);

        inviteCodeGroup.CodeInput = _uiRoot.transform.FindChild("InviteCodeGroup/InputGroup/Input").gameObject.GetComponent<UIInput>();
        inviteCodeGroup.Btn_Ok = _uiRoot.transform.FindChild("InviteCodeGroup/Btns/Ok").gameObject.GetComponent<UIButton>();
        inviteCodeGroup.Btn_Share = _uiRoot.transform.FindChild("InviteCodeGroup/Btns/Share").gameObject.GetComponent<UIButton>();
        inviteCodeGroup.MissionScrollView = _uiRoot.transform.FindChild("InviteCodeGroup/MissionGroup/MissonList").gameObject.GetComponent<UIScrollView>();
        inviteCodeGroup.MissionGrid = _uiRoot.transform.FindChild("InviteCodeGroup/MissionGroup/MissonList/Grid").gameObject.GetComponent<UIGrid>();
        inviteCodeGroup.MissionWrapContent = _uiRoot.transform.FindChild("InviteCodeGroup/MissionGroup/MissonList/Grid").gameObject.GetComponent<UIWrapContent>();
        inviteCodeGroup.MissionItem = _uiRoot.transform.FindChild("InviteCodeGroup/MissionGroup/MissonList/FriendMemberItem").gameObject;
        inviteCodeGroup.Lbl_Share = _uiRoot.transform.FindChild("InviteCodeGroup/MyCodeBg/Label").gameObject.GetComponent<UILabel>();
        inviteCodeGroup.Title = _uiRoot.transform.FindChild("InviteCodeGroup/MissionGroup/Title").gameObject.GetComponent<UILabel>();
        inviteCodeGroup.MissionItem.SetActive(false);

        friendMissionGroup.FriendShare = _uiRoot.transform.FindChild("FriendMissionGroup/Btns/Share").gameObject.GetComponent<UIButton>();
        friendMissionGroup.FB_Title = _uiRoot.transform.FindChild("FriendMissionGroup/FBMissionGroup/Title").gameObject.GetComponent<UILabel>();
        friendMissionGroup.FB_ScrollView = _uiRoot.transform.FindChild("FriendMissionGroup/FBMissionGroup/MissonList").gameObject.GetComponent<UIScrollView>();
        friendMissionGroup.FB_WrapContent = _uiRoot.transform.FindChild("FriendMissionGroup/FBMissionGroup/MissonList/Grid").gameObject.GetComponent<UIWrapContent>();
        friendMissionGroup.FB_Grid = _uiRoot.transform.FindChild("FriendMissionGroup/FBMissionGroup/MissonList/Grid").gameObject.GetComponent<UIGrid>();
        friendMissionGroup.FB_Item = _uiRoot.transform.FindChild("FriendMissionGroup/FBMissionGroup/MissonList/FriendMemberItem").gameObject;
        friendMissionGroup.LINE_Title = _uiRoot.transform.FindChild("FriendMissionGroup/LINEMissionGroup/Title").gameObject.GetComponent<UILabel>();
        friendMissionGroup.LINE_ScrollView = _uiRoot.transform.FindChild("FriendMissionGroup/LINEMissionGroup/MissonList").gameObject.GetComponent<UIScrollView>();
        friendMissionGroup.LINE_WrapContent = _uiRoot.transform.FindChild("FriendMissionGroup/LINEMissionGroup/MissonList/Grid").gameObject.GetComponent<UIWrapContent>();
        friendMissionGroup.LINE_Grid = _uiRoot.transform.FindChild("FriendMissionGroup/LINEMissionGroup/MissonList/Grid").gameObject.GetComponent<UIGrid>();
        friendMissionGroup.LINE_Item = _uiRoot.transform.FindChild("FriendMissionGroup/LINEMissionGroup/MissonList/FriendMemberItem").gameObject;
        friendMissionGroup.LINE_Item.SetActive(false);
        friendMissionGroup.FB_Item.SetActive(false);

        platformGroup.PlatformGroupBack = _uiRoot.transform.FindChild("PlatformGroup").gameObject;
        platformGroup.Btn_FBFriend = _uiRoot.transform.FindChild("PlatformGroup/ButtonFB").gameObject.GetComponent<UIButton>();
        platformGroup.Btn_LINEFriend = _uiRoot.transform.FindChild("PlatformGroup/ButtonLINE").gameObject.GetComponent<UIButton>();
        platformGroup.Mask = _uiRoot.transform.FindChild("PlatformGroup/BackGroup/Mask").gameObject;

        fbFriendGroup.Lbl_Label_FriendCount = _uiRoot.transform.FindChild("FBFrienGroup/Label_FriendCount").gameObject.GetComponent<UILabel>();
        fbFriendGroup.Lbl_Label_FreeCount = _uiRoot.transform.FindChild("FBFrienGroup/Label_FreeCount").gameObject.GetComponent<UILabel>();
        fbFriendGroup.ScrView_FriendList = _uiRoot.transform.FindChild("FBFrienGroup/FriendList").gameObject.GetComponent<UIScrollView>();
        fbFriendGroup.Grd_Grid = _uiRoot.transform.FindChild("FBFrienGroup/FriendList/Grid").gameObject.GetComponent<UIGrid>();
        fbFriendGroup.UIWrapContent_Grid = _uiRoot.transform.FindChild("FBFrienGroup/FriendList/Grid").gameObject.GetComponent<UIWrapContent>();
        fbFriendGroup.Item = _uiRoot.transform.FindChild("FBFrienGroup/FriendList/FriendMemberItem").gameObject;
        fbFriendGroup.Item.SetActive(false);
        SetLabelValues();
    }

    public void SetLabelValues()
    {
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
