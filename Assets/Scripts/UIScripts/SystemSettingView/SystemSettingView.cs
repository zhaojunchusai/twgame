using UnityEngine;
using System;
using System.Collections;
public class SystemSettingView
{
    public static string UIName ="SystemSettingView";
    #region MainSettings
    public GameObject _uiRoot;
    public UIPanel UIPanel_SystemSettingView;
    public UISprite Spt_SettingMaskBGSprite;
    public UIButton Btn_SettingTopCloseBtn;
    public UILabel Lbl_PlayerNameLabel;
    public UILabel Lbl_PlayerIDLabel;
    public UIButton Btn_RenameButton;
    public UISprite Spt_IconFrame;
    public UISprite Spt_IconBG;
    public UISprite Spt_IconSprite;
    public UIButton Btn_SoundButton;
    public UILabel  Spt_SoundFGSprite;
    public UISprite Spt_SoundBGSprite;
    public UIButton Btn_MusicButton;
    public UILabel  Spt_MusicFGSprite;
    public UISprite Spt_MusicBGSprite;
    public UIButton Btn_PushButton;
    public UIButton Btn_CDKeyButton;
    public UIButton Btn_BlockButton;
    public UIButton Btn_FeedbackButton;
    public UIButton Btn_ExitButton;
    public UIButton Btn_BindingButton;
    public UIButton Btn_IconChangeButton;
    public UIButton Btn_HeadChangeButton;
    public UISprite IconFrame;
    #endregion
    #region RemoveBlock
    public GameObject UIPanel_RemoveBlockView;
    public UIButton CloseRemoveBlockButton;
    public UIGrid RemoveBlockGrid;
    public UILabel NoBlockLabel;
    public GameObject Go_RemoveBlockItem;
    #endregion
    #region Push
    public UIPanel Panel_PushSetting;
    public UIButton Btn_PushCloseBtn;
    public UILabel Lab_PushBGTip;
    public UIButton Btn_SPPush;
    public UIButton Btn_SlavePush;
    public UIButton Btn_YuanBaoPush;
    public UIButton Btn_AgainstPush;
    public UIButton Btn_ShopPush;
    public UIButton Btn_ZhenBaPush;
    public UIButton Btn_GCLDPush;
    public UIButton Btn_QueRenPush;
    public UILabel Lab_SPPush;
    public UILabel Lab_SlavePush;
    public UILabel Lab_YuanBaoPush;
    public UILabel Lab_AgainstPush;
    public UILabel Lab_ShopPush;
    public UILabel Lab_ZhenBaPush;
    public UILabel Lab_GCLDFLab;
    public UISprite Spt_SPPush;
    public UISprite Spt_SlavePush;
    public UISprite Spt_YuanBaoPush;
    public UISprite Spt_AgainstPush;
    public UISprite Spt_ShopPush;
    public UISprite Spt_ZhenBaPush;
    public UISprite Spt_GCLDBGSprite;

    #endregion
    #region FeeedBack
    public UIPanel UIPanel_FeedbackView;
    public UISprite Spt_FeedbackMaskBGSprite;
    public UIButton Btn_FeedbackTopCloseButton;
    public UILabel Lbl_FeedbackTodayLastLabel;
    public UIInput Ipt_FeedbackTitleInupt;
    public UILabel Lbl_FeedbackTitleLabel;
    public UIInput Ipt_FeedbackContentInupt;
    public UILabel Lbl_FeedbackContentLabel;
    public UIButton Btn_FeedbackSendButton;
    public UILabel Lbl_FeedbackTimes;
    public UILabel  Spt_FeedBackSendFGSp;
    public UISprite Spt_FeedbackSendBGSprite;
    #endregion
    #region ChangeName
    public UIPanel UIPanel_ChangeNameView;
    public UISprite Spt_ChangeNameMaskBGSprite;
    public GameObject GameObj_InputNameObj;
    public UIButton Btn_RandomButton;
    public UIInput Ipt_NameInput;
    public UILabel Lbl_NameInputLabel;
    public UILabel Lbl_CostLabel;
    public UISprite Spt_CostSprite;
    public UILabel Lbl_CostNumLabel;
    public UIButton Btn_InputNameCancelButton;
    public UIButton Btn_InputNameConfirmButton;

    public GameObject GameObj_ConfirmChangeNameObj;
    public UILabel Lbl_NewNameLabel;
    public UIButton Btn_ChangeNameNoButton;
    public UIButton Btn_ChangeNameYseButton;
    #endregion
    #region CDKey
    public UIPanel UIPanel_CDKeyView;
    public UISprite Spt_CDKeyMaskBGSprite;
    public UIInput Ipt_CDKeyInput;
    public UILabel Lbl_CDKeyInputLabel;
    public UIButton Btn_CDKeyCancelButton;
    public UIButton Btn_CDKeyConfirmButton;
    #endregion
    #region Binding
    public UIPanel UIPanel_BindingView;
    public UISprite Spt_BindingMaskBGSprite;
    public UIButton Btn_BindingCancelButton;
    public UIButton Btn_BindingConfirmButton;
    public GameObject BindingPhoneNumObj;
    public UIInput Ipt_PhoneNumInput;
    public UILabel Lbl_PhoneNumInputLabel;
    public UIButton Btn_PhoneNumCancelButton;
    public UIButton Btn_PhoneNumConfirmButton;
    public GameObject BindingValidationCodeObj;
    public UIInput Ipt_ValidationInput;
    public UILabel Lbl_ValidationInputLabel;
    public UIButton Btn_GetValidationCodeButton;
    public UIButton Btn_ValidationCancelButton;
    public UIButton Btn_ValidationConfirmButton;
    public UIButton Btn_GetValidationButton;
    #endregion
    #region ChangeIcon
    public UIPanel UIPanel_IconChangeView;
    public UIButton Btn_CloseIconChange;
    public GameObject ChangeIconItem;
    public UIGrid Grid_ChangeIcon;
    public UIScrollView ScrView_FrameGroup;

    #endregion
    //public UIButton Btn_IconChangeItem1;
    //public UIButton Btn_IconChangeItem2;
    //public UIButton Btn_IconChangeItem3;

    //public UISprite Item1_HeadTex;
    //public UISprite Item1_IconTex;
    //public UILabel Item1_Label;
    //public GameObject Item1_ISClick;
    //public GameObject Item1_ISLock;

    //public UISprite Item2_HeadTex;
    //public UISprite Item2_IconTex;
    //public UILabel Item2_Label;
    //public GameObject Item2_ISClick;
    //public GameObject Item2_ISLock;

    //public UISprite Item3_HeadTex;
    //public UISprite Item3_IconTex;
    //public UILabel Item3_Label;
    //public GameObject Item3_ISClick;
    //public GameObject Item3_ISLock;
    #region ChangeHead
    public GameObject UIPanel_HeadChangeView;
    public UIButton CloseHeadChange;
    public GameObject ChangeHeadItem;

    public UIGrid BaseGrid;
    public UIGrid UAchievementGrid;
    public UIGrid USoldierGrid;
    public UIGrid LAchievementGrid;
    public UIGrid LSoldierGrid;

    public UITable Table;
    public UIScrollView ScrView_IconGroup;
    public GameObject Gobj_Desc;
    public UILabel Lbl_DescContent;
    public UISprite Spt_StrPageBG;
    public UIButton Btn_ChangeButton;

    public UIWrapContent UIWrapContent_UIGrid;
    #endregion
    public TweenScale OffsetRoot_TScale,FeedBack_TScale,ChangeName_TScale,CDK_TScale;
    public UILabel Lab_BlockFGLab;
    public UILabel Lab_BlockTitle;
    public UILabel Lab_ChangeFreeName;
    public void Initialize()
    {
        #region MainSettings

        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SystemSettingView");
       // UIPanel_SystemSettingView = _uiRoot.GetComponent<UIPanel>();
        Lab_ChangeFreeName = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/FreeLabel").gameObject.GetComponent<UILabel>();
        Lab_BlockFGLab = _uiRoot.transform.FindChild("OffsetRoot/Anim/BlockButton/BlockFGSprite").gameObject.GetComponent<UILabel>();
        Lab_BlockTitle = _uiRoot.transform.FindChild("RemoveBlockView/OffsetRoot/TitleBG/Label").gameObject.GetComponent<UILabel>();
        IconFrame = _uiRoot.transform.FindChild("OffsetRoot/Anim/Icon/IconFrame").gameObject.GetComponent<UISprite>();
        Btn_HeadChangeButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/HeadChangeButton").gameObject.GetComponent<UIButton>();
        Btn_IconChangeButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/IconChangeButton").gameObject.GetComponent<UIButton>();
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot/Anim").gameObject.GetComponent<TweenScale>();
        FeedBack_TScale = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot").gameObject.GetComponent<TweenScale>();
        //Spt_SettingMaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        Btn_SettingTopCloseBtn = _uiRoot.transform.FindChild("OffsetRoot/Anim/TopCloseBtn").gameObject.GetComponent<UIButton>();
        Lbl_PlayerNameLabel = _uiRoot.transform.FindChild("OffsetRoot/Anim/Name/PlayerNameLabel").gameObject.GetComponent<UILabel>();
        Lbl_PlayerIDLabel = _uiRoot.transform.FindChild("OffsetRoot/Anim/Name/PlayerIDLabel").gameObject.GetComponent<UILabel>();
        Btn_RenameButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/Name/RenameButton").gameObject.GetComponent<UIButton>();
        //Spt_IconFrame = _uiRoot.transform.FindChild("OffsetRoot/Anim/Icon/IconFrame").gameObject.GetComponent<UISprite>();
        //Spt_IconBG = _uiRoot.transform.FindChild("OffsetRoot/Anim/Icon/IconBG").gameObject.GetComponent<UISprite>();
        Spt_IconSprite = _uiRoot.transform.FindChild("OffsetRoot/Anim/Icon/IconSprite").gameObject.GetComponent<UISprite>();
        Btn_SoundButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/SoundButton").gameObject.GetComponent<UIButton>();
        Spt_SoundFGSprite = _uiRoot.transform.FindChild("OffsetRoot/Anim/SoundButton/SoundFGSprite").gameObject.GetComponent<UILabel >();
        Spt_SoundBGSprite = _uiRoot.transform.FindChild("OffsetRoot/Anim/SoundButton/SoundBGSprite").gameObject.GetComponent<UISprite>();
        Btn_MusicButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/MusicButton").gameObject.GetComponent<UIButton>();
        Spt_MusicFGSprite = _uiRoot.transform.FindChild("OffsetRoot/Anim/MusicButton/MusicFGSprite").gameObject.GetComponent<UILabel >();
        Spt_MusicBGSprite = _uiRoot.transform.FindChild("OffsetRoot/Anim/MusicButton/MusicBGSprite").gameObject.GetComponent<UISprite>();
        Btn_PushButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/PushButton").gameObject.GetComponent<UIButton>();
        Btn_CDKeyButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/CDKeyButton").gameObject.GetComponent<UIButton>();
        Btn_BlockButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/BlockButton").gameObject.GetComponent<UIButton>();
        Btn_FeedbackButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/FeedbackButton").gameObject.GetComponent<UIButton>();
        Btn_ExitButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/ExitButton").gameObject.GetComponent<UIButton>();
        Btn_BindingButton = _uiRoot.transform.FindChild("OffsetRoot/Anim/BindingButton").gameObject.GetComponent<UIButton>();
        
        #endregion
        #region RemoveBlock
        UIPanel_RemoveBlockView = _uiRoot.transform.FindChild("RemoveBlockView").gameObject;
        CloseRemoveBlockButton = _uiRoot.transform.FindChild("RemoveBlockView/OffsetRoot/TopCloseButton").gameObject.GetComponent<UIButton>();
        RemoveBlockGrid = _uiRoot.transform.FindChild("RemoveBlockView/OffsetRoot/RemoveBlockScrollView/Grid").gameObject.GetComponent<UIGrid>();
        NoBlockLabel = _uiRoot.transform.FindChild("RemoveBlockView/OffsetRoot/NoBlockLabel").gameObject.GetComponent<UILabel>();
        Go_RemoveBlockItem = _uiRoot.transform.FindChild("RemoveBlockView/OffsetRoot/RemoveBlockScrollView/RemoveBlockItem").gameObject;
        #endregion

        #region PushSetting
        Panel_PushSetting = _uiRoot.transform.FindChild("PushSettingView").gameObject.GetComponent<UIPanel>();
        Btn_PushCloseBtn = Panel_PushSetting.transform.FindChild("Anim/CloseBtn").gameObject.GetComponent<UIButton>();
        Lab_PushBGTip = Panel_PushSetting.transform.FindChild("Anim/BG/BGTip").gameObject.GetComponent<UILabel>();
        Btn_SPPush = Panel_PushSetting.transform.FindChild("Anim/Btn/SPPushButton").gameObject.GetComponent<UIButton>();
        Btn_SlavePush = Panel_PushSetting.transform.FindChild("Anim/Btn/SlavePushButton").gameObject.GetComponent<UIButton>();
        Btn_YuanBaoPush = Panel_PushSetting.transform.FindChild("Anim/Btn/YuanBaoPushButton").gameObject.GetComponent<UIButton>();
        Btn_AgainstPush = Panel_PushSetting.transform.FindChild("Anim/Btn/AgainstPushButton").gameObject.GetComponent<UIButton>();
        Btn_ShopPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ShopPushButton").gameObject.GetComponent<UIButton>();
        Btn_ZhenBaPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ZhenBaPushButton").gameObject.GetComponent<UIButton>();
        Btn_GCLDPush = Panel_PushSetting.transform.FindChild("Anim/Btn/GCLDButton").gameObject.GetComponent<UIButton>();
        Btn_QueRenPush = Panel_PushSetting.transform.FindChild("Anim/Btn/QueRenPushButton").gameObject.GetComponent<UIButton>();
        Lab_SPPush = Panel_PushSetting.transform.FindChild("Anim/Btn/SPPushButton/SPPushFGLab").gameObject.GetComponent<UILabel>();
        Lab_SlavePush = Panel_PushSetting.transform.FindChild("Anim/Btn/SlavePushButton/SlavePushFGLab").gameObject.GetComponent<UILabel>();
        Lab_YuanBaoPush = Panel_PushSetting.transform.FindChild("Anim/Btn/YuanBaoPushButton/YuanBaoPushFGLab").gameObject.GetComponent<UILabel>();
        Lab_AgainstPush = Panel_PushSetting.transform.FindChild("Anim/Btn/AgainstPushButton/AgainstPushFGLab").gameObject.GetComponent<UILabel>();
        Lab_ShopPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ShopPushButton/ShopPushFGLab").gameObject.GetComponent<UILabel>();
        Lab_ZhenBaPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ZhenBaPushButton/ZhenBaPushFGLab").gameObject.GetComponent<UILabel>();
        Lab_GCLDFLab = Panel_PushSetting.transform.FindChild("Anim/Btn/GCLDButton/GCLDFLab").gameObject.GetComponent<UILabel>();

        Spt_SPPush = Panel_PushSetting.transform.FindChild("Anim/Btn/SPPushButton/SPPushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_SlavePush = Panel_PushSetting.transform.FindChild("Anim/Btn/SlavePushButton/SlavePushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_YuanBaoPush = Panel_PushSetting.transform.FindChild("Anim/Btn/YuanBaoPushButton/YuanBaoPushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_AgainstPush = Panel_PushSetting.transform.FindChild("Anim/Btn/AgainstPushButton/AgainstPushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_ShopPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ShopPushButton/ShopPushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_ZhenBaPush = Panel_PushSetting.transform.FindChild("Anim/Btn/ZhenBaPushButton/ZhenBaPushBGSprite").gameObject.GetComponent<UISprite>();
        Spt_GCLDBGSprite = Panel_PushSetting.transform.FindChild("Anim/Btn/GCLDButton/GCLDBGSprite").gameObject.GetComponent<UISprite>();


        #endregion

        #region Feedback

        UIPanel_FeedbackView = _uiRoot.transform.FindChild("FeedbackView").gameObject.GetComponent<UIPanel>();
        //Spt_FeedbackMaskBGSprite = _uiRoot.transform.FindChild("FeedbackView/MaskBGSprite").gameObject.GetComponent<UISprite>();
        Btn_FeedbackTopCloseButton = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/TopCloseButton").gameObject.GetComponent<UIButton>();
        Lbl_FeedbackTodayLastLabel = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/ContentGroup/TodayLastLabel").gameObject.GetComponent<UILabel>();
        Ipt_FeedbackTitleInupt = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/ContentGroup/TitleInupt").gameObject.GetComponent<UIInput>();
        Lbl_FeedbackTitleLabel = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/ContentGroup/TitleInupt/TitleLabel").gameObject.GetComponent<UILabel>();
        Ipt_FeedbackContentInupt = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/ContentGroup/ContentInupt").gameObject.GetComponent<UIInput>();
        Lbl_FeedbackContentLabel = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/ContentGroup/ContentInupt/FeedbackLabel").gameObject.GetComponent<UILabel>();
        Btn_FeedbackSendButton = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/FeedbackSendButton").gameObject.GetComponent<UIButton>();
        Lbl_FeedbackTimes = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/FeedbackSendButton/FeedbackTimes").gameObject.GetComponent<UILabel>();
        Spt_FeedBackSendFGSp = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/FeedbackSendButton/FeedbackSendFGSprite").gameObject.GetComponent<UILabel >();
        Spt_FeedbackSendBGSprite = _uiRoot.transform.FindChild("FeedbackView/OffsetRoot/FeedbackSendButton/FeedbackSendBGSprite").gameObject.GetComponent<UISprite>();
        #endregion

        #region ChangeName
        ChangeName_TScale = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot").gameObject.GetComponent<TweenScale>();
        UIPanel_ChangeNameView = _uiRoot.transform.FindChild("ChangeNameView").gameObject.GetComponent<UIPanel>();
        //Spt_ChangeNameMaskBGSprite = _uiRoot.transform.FindChild("ChangeNameView/MaskBGSprite").gameObject.GetComponent<UISprite>();
        GameObj_InputNameObj = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup").gameObject;
        Btn_RandomButton = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/RandomButton").gameObject.GetComponent<UIButton>();
        Ipt_NameInput = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/NameInput").gameObject.GetComponent<UIInput>();
        Lbl_NameInputLabel = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/NameInput/NameInputLabel").gameObject.GetComponent<UILabel>();
        Lbl_CostLabel = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/CostLabel").gameObject.GetComponent<UILabel>();
        Spt_CostSprite = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/CostSprite").gameObject.GetComponent<UISprite>();
        Lbl_CostNumLabel = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/CostNumLabel").gameObject.GetComponent<UILabel>();
        Btn_InputNameCancelButton = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/CancelButton").gameObject.GetComponent<UIButton>();
        Btn_InputNameConfirmButton = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/InuptNameGroup/ConfirmButton").gameObject.GetComponent<UIButton>();
        GameObj_ConfirmChangeNameObj = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/ConfirmChangeNameGroup").gameObject;
        Lbl_NewNameLabel = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/ConfirmChangeNameGroup/NewNameLabel").gameObject.GetComponent<UILabel>();
        Btn_ChangeNameNoButton = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/ConfirmChangeNameGroup/NoButton").gameObject.GetComponent<UIButton>();
        Btn_ChangeNameYseButton = _uiRoot.transform.FindChild("ChangeNameView/OffsetRoot/ConfirmChangeNameGroup/YesButton").gameObject.GetComponent<UIButton>();
        #endregion

        #region CDKey

        UIPanel_CDKeyView = _uiRoot.transform.FindChild("CDKeyView").gameObject.GetComponent<UIPanel>();
        Spt_CDKeyMaskBGSprite = _uiRoot.transform.FindChild("CDKeyView/MaskBGSprite").gameObject.GetComponent<UISprite>();
        Ipt_CDKeyInput = _uiRoot.transform.FindChild("CDKeyView/OffsetRoot/CDKeyInput").gameObject.GetComponent<UIInput>();
        Lbl_CDKeyInputLabel = _uiRoot.transform.FindChild("CDKeyView/OffsetRoot/CDKeyInput/CDKeyInputLabel").gameObject.GetComponent<UILabel>();
        Btn_CDKeyCancelButton = _uiRoot.transform.FindChild("CDKeyView/OffsetRoot/CancelButton").gameObject.GetComponent<UIButton>();
        Btn_CDKeyConfirmButton = _uiRoot.transform.FindChild("CDKeyView/OffsetRoot/ConfirmButton").gameObject.GetComponent<UIButton>();
        CDK_TScale = _uiRoot.transform.FindChild("CDKeyView/OffsetRoot").gameObject.GetComponent<TweenScale>();
        #endregion

        #region Binding
        UIPanel_BindingView = _uiRoot.transform.FindChild("BindingView").gameObject.GetComponent<UIPanel>();
        //Spt_BindingMaskBGSprite = _uiRoot.transform.FindChild("BindingView/OffsetRoot/MaskBGSprite").gameObject.GetComponent<UISprite>();
        Btn_BindingCancelButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/CancelButton").gameObject.GetComponent<UIButton>();
        Btn_BindingConfirmButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/ConfirmButton").gameObject.GetComponent<UIButton>();
        Ipt_PhoneNumInput = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/PhoneNumInput").gameObject.GetComponent<UIInput>();
        Lbl_PhoneNumInputLabel = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/PhoneNumInput/PhoneNumInputLabel").gameObject.GetComponent<UILabel>();
        Btn_PhoneNumCancelButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/CancelButton").gameObject.GetComponent<UIButton>();
        Btn_PhoneNumConfirmButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/PhoneNumGroup/ConfirmButton").gameObject.GetComponent<UIButton>();
        Btn_ValidationCancelButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/ValidationCodeGroup/CancelButton").gameObject.GetComponent<UIButton>();
        Btn_ValidationConfirmButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/ValidationCodeGroup/ConfirmButton").gameObject.GetComponent<UIButton>();
        Ipt_ValidationInput = _uiRoot.transform.FindChild("BindingView/OffsetRoot/ValidationCodeGroup/ValidationInput").gameObject.GetComponent<UIInput>();
        Lbl_ValidationInputLabel = _uiRoot.transform.FindChild("BindingView/OffsetRoot/ValidationCodeGroup/ValidationInput/ValidationInputLabel").gameObject.GetComponent<UILabel>();
        Btn_GetValidationCodeButton = _uiRoot.transform.FindChild("BindingView/OffsetRoot/ValidationCodeGroup/GetValidationCodeButton").gameObject.GetComponent<UIButton>();
        #endregion

        #region ChangeIcon
        UIPanel_IconChangeView = _uiRoot.transform.FindChild("IconChangeView").gameObject.GetComponent<UIPanel>();
        Btn_CloseIconChange = _uiRoot.transform.FindChild("IconChangeView/Anim/CloseBtn").gameObject.GetComponent<UIButton>();
        ChangeIconItem = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/IconChangeItem").gameObject;
        Grid_ChangeIcon = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid").gameObject.GetComponent <UIGrid >();
        UIWrapContent_UIGrid = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrView_FrameGroup = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView").gameObject.GetComponent<UIScrollView>();
        //Btn_IconChangeItem1= _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1").gameObject .GetComponent <UIButton >();
        //Btn_IconChangeItem2 = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2").gameObject.GetComponent<UIButton>();
        //Btn_IconChangeItem3 = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3").gameObject.GetComponent<UIButton>();

        //Item1_HeadTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1/Head/HeadTex").gameObject.GetComponent<UISprite>();
        //Item1_IconTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1/Icon/HeadIcon").gameObject.GetComponent<UISprite>();
        //Item1_Label = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1/Label").gameObject.GetComponent<UILabel>();
        //Item1_ISClick = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1/Click").gameObject;
        //Item1_ISLock = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem1/Lock").gameObject;

        //Item2_HeadTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2/Head/HeadTex").gameObject.GetComponent<UISprite>();
        //Item2_IconTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2/Icon/HeadIcon").gameObject.GetComponent<UISprite>();
        //Item2_Label = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2/Label").gameObject.GetComponent<UILabel>();
        //Item2_ISClick = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2/Click").gameObject;
        //Item2_ISLock = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem2/Lock").gameObject;

        //Item3_HeadTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3/Head/HeadTex").gameObject.GetComponent<UISprite>();
        //Item3_IconTex = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3/Icon/HeadIcon").gameObject.GetComponent<UISprite>();
        //Item3_Label = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3/Label").gameObject.GetComponent<UILabel>();
        //Item3_ISClick = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3/Click").gameObject;
        //Item3_ISLock = _uiRoot.transform.FindChild("IconChangeView/Anim/gobj_ChangeIcon/ChangeScrollView/Grid/IconChangeItem3/Lock").gameObject;

        #endregion
        #region ChangeHead
        UIPanel_HeadChangeView = _uiRoot.transform.FindChild("HeadChangeView").gameObject;
        ChangeHeadItem = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/HeadChangeItem").gameObject;

        BaseGrid = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table/BaseGrid").gameObject.GetComponent<UIGrid>();
        UAchievementGrid = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table/UAchievementGrid").gameObject.GetComponent<UIGrid>();
        USoldierGrid = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table/USoldierGrid").gameObject.GetComponent<UIGrid>();
        LAchievementGrid = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table/LAchievementGrid").gameObject.GetComponent<UIGrid>();
        LSoldierGrid = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table/LSoldierGrid").gameObject.GetComponent<UIGrid>();
        Gobj_Desc = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/gobj_Desc").gameObject;
        Lbl_DescContent = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/gobj_Desc/DescContent").gameObject.GetComponent<UILabel>();
        Spt_StrPageBG = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/gobj_Desc/StrPageBG").gameObject.GetComponent<UISprite>();
        Btn_ChangeButton = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/ChangeButton").gameObject.GetComponent<UIButton>();
        ScrView_IconGroup = _uiRoot.transform.Find("HeadChangeView/OffsetRoot/HeadScrollView").gameObject.GetComponent<UIScrollView>();
        Table = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/HeadScrollView/Table").gameObject.GetComponent<UITable>();
        CloseHeadChange = _uiRoot.transform.FindChild("HeadChangeView/OffsetRoot/Mask").gameObject.GetComponent<UIButton>();
        #endregion

        SetLabelValues();
    }

    public void SetLabelValues()
    {
        NoBlockLabel.text = "你暫時沒有屏蔽任何玩家";
        //Lbl_PlayerNameLabel.text = "角色名字七个字";
        //Lbl_PlayerIDLabel.text = "账号ID：DIS342411";
        //Lbl_FeedbackTodayLastLabel.text = "今日剩余次数：0";
        //Lbl_FeedbackTitleLabel.text = "点此输入标题";
        //Lbl_FeedbackContentLabel.text = "点击此处，可输入200字的意见或建议";
        //Lbl_NameInputLabel.text = "点击输入新角色名";
        //Lbl_CostLabel.text = "修改角色名将消耗";
        //Lbl_CostNumLabel.text = "0";
        //Lbl_CDKeyInputLabel.text = "兑换码输入框";
        //Lbl_PhoneNumInputLabel.text = "请输入手机号码";
        //Lbl_ValidationInputLabel.text = "点击输入验证码";
        //Lbl_FeedbackTimes.text = "";
        Lab_BlockFGLab.text = Lab_BlockTitle.text = "屏蔽解除";
        Lab_ChangeFreeName.text = "免費改名次數1/1";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
