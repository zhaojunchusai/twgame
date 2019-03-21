using UnityEngine;
using System.Collections;

public class MenuView
{
    public static string UIName = "MenuView";
    public GameObject _uiRoot;

    public GameObject Gobj_Menu;
    public UIButton Btn_Hero;
    public UISprite Spt_HeroNotice;
    public UIButton Btn_General;
    public UIButton Btn_Bag;
    public UISprite Spt_BagNotice;
    public UIButton Btn_Shop;

    public GameObject Gobj_Icon;
    public UIButton Btn_SignInButton;//签到
    public UIButton Btn_RechargeButton;//充值
    public UIButton Btn_NoticeButton;//公告
    public UIButton Btn_TaskButton;//任务
    public UIButton Btn_ActiveButton;//活跃
    //public UIButton Btn_MailButton;//邮件
    //public UISprite Spt_MailNotice;//邮件
    //public UILabel Lbl_MailNoticeNum;//邮件
    public UISprite Spt_SignNotice;
    public UILabel Lbl_SignNoticeNum;
    public UISprite Spt_ActivityNotice;
    public UILabel Lbl_ActivityNoticeNum;
    public UISprite Spt_TaskNotice;
    public UILabel Lbl_TaskNoticeNum;
    public UISprite Spt_LivenessNotice;
    public UILabel Lbl_LivenessNoticeNum;
    public UIButton Btn_OpenMenu;
    public UISprite Spt_BtnOpenMenuBG;
    public UIButton Btn_Illustrated;
    public GameObject Spt_GeneralNotice;
    public UIButton Btn_OnlinePackage;
    public UILabel Lbl_TimeLabel;
    public UISprite Spt_OnlineFG;
    public UISprite Spt_OnlineEffect;

    public UIButton Btn_NoviceTask;
    public UILabel Lbl_NoviceTaskTime;
    public UISprite Spt_NoviceTaskFG;
    public UISprite Spt_NoviceTaskEffect;
    public GameObject Gobj_NoviceTaskNotify;

    public UIButton Btn_FirstPay;//首冲
    public UIButton Btn_FirstLogin;//豪礼
    public UIButton Btn_Mall;
    public UIGrid Grd_Up;

    public UIButton Btn_Achievement;
    public GameObject Gobj_AchievementNotify;
    public UILabel Lbl_AchievementNotifyNum;
	public UIButton Btn_LifeSpirit;
    public UISprite Gobj_LifeSpiritNotify;
    public UIButton Btn_Recycle;
  
    public GameObject Gobj_TotalNotify;

    public UIButton Btn_HaoYou;
    public UIButton Btn_Raiders;
    public UIButton Btn_Announcement;
    public GameObject Go_HaoYouNotify;
    public UILabel Lab_HaoYouNotify;

    public GameObject Gobj_LivenessTip;
    public TweenPosition TipTweenPos;
    public TweenScale TipTweenScle;
    public UISprite Spt_LivenessTipBG;
    public UILabel Lbl_LivenessTip;

    public GameObject Gobj_Pet;
    public UISprite Spt_PetNotify;
    public UISprite Spt_PetLock;
    public UISprite Spt_LivenessLock;
    public UISprite Spt_LifeSoulLock;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/MenuView");
        Btn_HaoYou = _uiRoot.transform.FindChild("Menu/gobj_Menu/HaoYou").gameObject.GetComponent<UIButton>();
        Go_HaoYouNotify = Btn_HaoYou.transform.FindChild("Notify").gameObject;
        Lab_HaoYouNotify = Go_HaoYouNotify.transform.FindChild("NotifyNum").gameObject.GetComponent<UILabel>();
        Gobj_Menu = _uiRoot.transform.FindChild("Menu/gobj_Menu").gameObject;
        Btn_Hero = _uiRoot.transform.FindChild("Menu/gobj_Icon/Hero").gameObject.GetComponent<UIButton>();
        Spt_HeroNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/Hero/Notify").gameObject.GetComponent<UISprite>();
        Gobj_Pet = _uiRoot.transform.FindChild("Menu/gobj_Icon/Pet").gameObject;
        Spt_PetNotify = _uiRoot.transform.FindChild("Menu/gobj_Icon/Pet/Notify").gameObject.GetComponent<UISprite>();
        Spt_PetLock = _uiRoot.transform.FindChild("Menu/gobj_Icon/Pet/Lock").gameObject.GetComponent<UISprite>();
        Btn_General = _uiRoot.transform.FindChild("Menu/gobj_Icon/General").gameObject.GetComponent<UIButton>();
        Spt_GeneralNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/General/Notify").gameObject;
        Btn_Bag = _uiRoot.transform.FindChild("Menu/gobj_Icon/Bag").gameObject.GetComponent<UIButton>();
        Spt_BagNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/Bag/Notify").gameObject.GetComponent<UISprite>();
        Btn_Illustrated = _uiRoot.transform.FindChild("Menu/gobj_Menu/illustrated").gameObject.GetComponent<UIButton>();
        // Btn_Shop = _uiRoot.transform.FindChild("Menu/gobj_Menu/Shop").gameObject.GetComponent<UIButton>();
        Btn_Mall = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Mall").gameObject.GetComponent<UIButton>();
        Btn_Raiders = _uiRoot.transform.FindChild("Menu/gobj_Menu/Raiders").gameObject.GetComponent<UIButton>();
        Gobj_Icon = _uiRoot.transform.FindChild("Menu/gobj_Icon").gameObject;
        Btn_SignInButton = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/SignIn").gameObject.GetComponent<UIButton>();
        Btn_RechargeButton = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Recharge").gameObject.GetComponent<UIButton>();
        Btn_FirstPay = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/FirstPay").gameObject.GetComponent<UIButton>();
        Btn_FirstLogin = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/FirstLogin").gameObject.GetComponent<UIButton>();
        Btn_NoticeButton = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Notice").gameObject.GetComponent<UIButton>();
        Btn_TaskButton = _uiRoot.transform.FindChild("Menu/gobj_Icon/Task").gameObject.GetComponent<UIButton>();
        Btn_ActiveButton = _uiRoot.transform.FindChild("Menu/gobj_Icon/Active").gameObject.GetComponent<UIButton>();
        Spt_LivenessLock = _uiRoot.transform.FindChild("Menu/gobj_Icon/Active/Lock").gameObject.GetComponent<UISprite>();
        //Btn_MailButton = _uiRoot.transform.FindChild("Menu/gobj_Icon/Mail").gameObject.GetComponent<UIButton>();
        //Spt_MailNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/Mail/Notify").gameObject.GetComponent<UISprite>();
        //Lbl_MailNoticeNum = _uiRoot.transform.FindChild("Menu/gobj_Icon/Mail/Notify/NotifyNum").gameObject.GetComponent<UILabel>();
        Spt_SignNotice = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/SignIn/Notify").gameObject.GetComponent<UISprite>();
        Lbl_SignNoticeNum = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/SignIn/Notify/NotifyNum").gameObject.GetComponent<UILabel>();
        Spt_ActivityNotice = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Notice/Notify").gameObject.GetComponent<UISprite>();
        Lbl_ActivityNoticeNum = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Notice/Notify/NotifyNum").gameObject.GetComponent<UILabel>();
        Spt_TaskNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/Task/Notify").gameObject.GetComponent<UISprite>();
        Lbl_TaskNoticeNum = _uiRoot.transform.FindChild("Menu/gobj_Icon/Task/Notify/NotifyNum").gameObject.GetComponent<UILabel>();
        Spt_LivenessNotice = _uiRoot.transform.FindChild("Menu/gobj_Icon/Active/Notify").gameObject.GetComponent<UISprite>();
        Lbl_LivenessNoticeNum = _uiRoot.transform.FindChild("Menu/gobj_Icon/Active/Notify/NotifyNum").gameObject.GetComponent<UILabel>();

        Btn_NoviceTask = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/NoviceTask").gameObject.GetComponent<UIButton>();
        Lbl_NoviceTaskTime = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/NoviceTask/TimeLabel").gameObject.GetComponent<UILabel>();
        Spt_NoviceTaskFG = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/NoviceTask/NoviceTaskFG").gameObject.GetComponent<UISprite>();
        Spt_NoviceTaskEffect = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/NoviceTask/Effect").gameObject.GetComponent<UISprite>();
        Gobj_NoviceTaskNotify = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/NoviceTask/Notify").gameObject;

        Btn_OnlinePackage = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/OnlinePackage").gameObject.GetComponent<UIButton>();
        Lbl_TimeLabel = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/OnlinePackage/TimeLabel").gameObject.GetComponent<UILabel>();
        Spt_OnlineFG = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/OnlinePackage/OnlineFG").gameObject.GetComponent<UISprite>();
        Spt_OnlineEffect = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/OnlinePackage/Effect").gameObject.GetComponent<UISprite>();

        Btn_Announcement = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up/Announce").gameObject.GetComponent<UIButton>();

        Btn_Achievement = _uiRoot.transform.FindChild("Menu/gobj_Menu/Achievement").gameObject.GetComponent<UIButton>();
        Gobj_AchievementNotify = _uiRoot.transform.FindChild("Menu/gobj_Menu/Achievement/Notify").gameObject;
        Lbl_AchievementNotifyNum = _uiRoot.transform.FindChild("Menu/gobj_Menu/Achievement/Notify/NotifyNum").gameObject.GetComponent<UILabel>();

        Btn_LifeSpirit = _uiRoot.transform.FindChild("Menu/gobj_Icon/LifeSpirit").gameObject.GetComponent<UIButton>();
        Gobj_LifeSpiritNotify = _uiRoot.transform.FindChild("Menu/gobj_Icon/LifeSpirit/Notify").gameObject.GetComponent<UISprite>();
        Spt_LifeSoulLock = _uiRoot.transform.FindChild("Menu/gobj_Icon/LifeSpirit/Lock").gameObject.GetComponent<UISprite>();

        Btn_Recycle = _uiRoot.transform.FindChild("Menu/gobj_Menu/Recycle").gameObject.GetComponent<UIButton>();

        Btn_OpenMenu = _uiRoot.transform.FindChild("OpenMenu").gameObject.GetComponent<UIButton>();
        Spt_BtnOpenMenuBG = _uiRoot.transform.FindChild("OpenMenu/BG").gameObject.GetComponent<UISprite>();
        Gobj_TotalNotify = _uiRoot.transform.FindChild("OpenMenu/Notify").gameObject;
        Grd_Up = _uiRoot.transform.FindChild("Menu/Gobj_Up/Up").gameObject.GetComponent<UIGrid>();

        Gobj_LivenessTip = _uiRoot.transform.FindChild("LivenessTipObj").gameObject;
        TipTweenPos = Gobj_LivenessTip.GetComponent<TweenPosition>();
        TipTweenScle = Gobj_LivenessTip.GetComponent<TweenScale>();
        Spt_LivenessTipBG = _uiRoot.transform.FindChild("LivenessTipObj/BG").gameObject.GetComponent<UISprite>();
        Lbl_LivenessTip = _uiRoot.transform.FindChild("LivenessTipObj/LivenessTip").gameObject.GetComponent<UILabel>();
        Gobj_LivenessTip.SetActive(false);
        Grd_Up.hideInactive = true;
        Lab_HaoYouNotify.gameObject.SetActive(false);
    }
    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
