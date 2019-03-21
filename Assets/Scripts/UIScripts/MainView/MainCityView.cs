using UnityEngine;
using System.Collections;

public class MainCityView
{
    public static string UIName = "MainCityView";
    public GameObject _uiRoot;
    public UILabel Lab_PlayerVip;
    public UILabel Lab_PlayerName;
    public UILabel Lab_PlayerLevel;
    public UISprite Spt_HeadIcon;
    public UIButton Btn_HeadUI;//打开设置界面
    public UIButton Btn_ChatButton;//聊天
    public UIScrollView ScrollView_MainCity;
    public UIPanel Pnl_MainCity;

    public UIButton Btn_MainGateButton;      //关卡界面
    public UIButton Btn_SlaveButton;         //奴隶界面
    public UIButton Btn_LegionButton;        //军团界面
    public UIButton Btn_ArenaButton;         //竞技场
    public UIButton Btn_ExpeditionButton;    //远征天下
    public UIButton Btn_BillboardButton;     //排行榜
    public UIButton Btn_BattlegroundButton;  //无尽战场
    public UIButton Btn_ActivitiesButton;    //活动界面
    public UIButton Btn_AgainstButton;       //招贤界面
    public UIButton Btn_MailButton;
    public UIButton Btn_ShopButton;

    public UIButton Btn_Grass_1;
    public UIButton Btn_Grass_2;
    public UIButton Btn_Grass_3;
    public UIButton Btn_Grass_4;
    public SkeletonAnimation SkeletonGrass_1;
    public SkeletonAnimation SkeletonGrass_2;
    public SkeletonAnimation SkeletonGrass_3;
    public SkeletonAnimation SkeletonGrass_4;

    public UILabel Lab_Battleground;
    public UISprite Spt_Battleground;
    public UILabel Lab_Billboard;
    public UISprite Spt_Billboard;
    public UILabel Lab_Activity;
    public UISprite Spt_Activity;
    public UILabel Lab_Expedition;
    public UISprite Spt_Expedition;
    public UILabel Lab_Legion;
    public UISprite Spt_Legion;
    public UISprite Spt_LegionNotice;
    public UILabel Lab_Slave;
    public UISprite Spt_Slave;
    public UILabel Lab_Arena;
    public UISprite Spt_Arena;
    public UISprite Spt_RecruitNotice;
    public UILabel Lab_Shop;
    public UISprite Spt_Shop;

    public UILabel BattlegroundLab;
    public UILabel BillboardLab;
    public UILabel ActivityLab;
    public UILabel ExpeditionLab;
    public UILabel ArenaLab;
    public UILabel LegionLab;
    public UILabel SlaveLab;
    public GameObject  ChatNotice;
    public SkeletonAnimation ChatNoticeAnim;


    public UISprite HeadFightBG;
    public UISprite HeadVipBG;
    public UISprite HeadExpBG;
    public UISprite HeadUIBG;
    public UISprite HeadState;
    public UILabel AgainstLab;
    public GameObject MailNotice;
    public GameObject SlaveNotice;

    public UIBoundary Boundary = new UIBoundary();
    public UISlider HeadExp_Slider;
    public UILabel HeadExp_Label;

    public GameObject Gobj_MarqueePanel;
    public GameObject Gobj_MarqueeGroup;
    public UILabel Lbl_Marquee1;
    public UILabel Lbl_Marquee2;
    public UILabel Lbl_Marquee3;
    public UILabel Lbl_Marquee4;
    public UILabel Lbl_Marquee5;

    public TweenScale Tscle_qipao;
    public UILabel Lbl_Tips;

    public UIButton Btn_Magical;
    public UILabel Lab_MagicalLab;
    public UIButton Btn_YuanBao;
    public GameObject YuanBaoNotice;
    public UILabel Lab_YuanBao;
    public UIButton Btn_BaZhu;
    public GameObject BaZhuEffect;
    public GameObject BaZhuNotice;

    public UIButton Btn_HeadVIP;
    public UILabel Lab_MaxFighting;

    public GameObject Gobj_VipEffect;
    public GameObject Gobj_UnlockEffect;
    public GameObject Gobj_UnlockEffectFore;
    public KeepPosRelativeToTarget Layer2Relative;
    public Transform Tran_MainCityFG;
    public GameObject Gobj_YuanBaoTitle;
    public GameObject Gobj_SlaveTitle;
    public GameObject Gobj_StoreTitle;
    public GameObject Gobj_MagicalTitle;
    public UILabel Lab_ShopName;
    public UIButton Btn_PaiWeiSai;
    public GameObject Gobj_TitlePaiWeiSai;

    public UISprite Spt_BattleGroundDouble;
    public UISprite Spt_ActivityDouble;
    public UISprite Spt_ExpeditionDouble;

    public UILabel Lab_BaZhu;

    public UILabel Lbl_BazhuName;
    public GameObject Gobj_BazhuName;

    public UILabel Lbl_PaiWeiSai;
    
    public UISprite Spt_PaiWeiSai;
    public UISprite Spt_YuanBao;
    public UISprite Spt_Recruit;
    public UISprite Spt_BaZhu;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/MainCityView");
        Gobj_BazhuName = _uiRoot.transform.FindChild("MainCity/MainCityFG/BaZhu/NameSprite").gameObject;
        Lbl_BazhuName = _uiRoot.transform.FindChild("MainCity/MainCityFG/BaZhu/NameSprite/Name").gameObject.GetComponent<UILabel>();
        Btn_PaiWeiSai = _uiRoot.transform.FindChild("MainCity/MainCityFG/PaiWeiSai").gameObject.GetComponent<UIButton>();
        ChatNotice = _uiRoot.transform.FindChild("MainButtonPanel/Chat/Sprite").gameObject; 
        HeadVipBG = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/VIPUIBG").gameObject.GetComponent<UISprite>();
        HeadExpBG = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/ExpUI/ExpUIBG").gameObject.GetComponent<UISprite>();
        HeadUIBG = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/HeadUIBG").gameObject.GetComponent<UISprite>();
        HeadState = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/HeadState").gameObject.GetComponent<UISprite>();
        Tscle_qipao = _uiRoot.transform.FindChild("MainCity/MainCityFG/MainGate/Qipao").gameObject.GetComponent<TweenScale>();
        Lbl_Tips = _uiRoot.transform.FindChild("MainCity/MainCityFG/MainGate/Qipao/Label").gameObject.GetComponent<UILabel>();
        Btn_Magical = _uiRoot.transform.FindChild("MainCity/MainCityFG/Magical").gameObject.GetComponent<UIButton>();
        Btn_YuanBao = _uiRoot.transform.FindChild("MainCity/MainCityFG/YuanBao").gameObject.GetComponent<UIButton>();
        HeadFightBG = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/MaxFighting/BG").gameObject.GetComponent<UISprite>();
        Btn_HeadVIP = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/VIPUIBG").gameObject.GetComponent<UIButton>();
        Lab_MaxFighting = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/MaxFighting/MaxFightingLab").gameObject.GetComponent<UILabel>();
        #region UI
        HeadExp_Slider = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/ExpUI").gameObject.GetComponent<UISlider>();
        HeadExp_Label = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/ExpUI/Exp").gameObject.GetComponent<UILabel >();
        Spt_HeadIcon = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/HeadUIIcon").gameObject.GetComponent<UISprite>();
        Lab_PlayerVip = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/VipUILabel").gameObject.GetComponent<UILabel>();
        Lab_PlayerName = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/NameUIBG/NameUILabel").gameObject.GetComponent<UILabel>();
        Lab_PlayerLevel = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/LevelUIBG/LevelUILabel").gameObject.GetComponent<UILabel>();
        Btn_HeadUI = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI").gameObject.GetComponent<UIButton>();
        Btn_ChatButton = _uiRoot.transform.FindChild("MainButtonPanel/Chat").gameObject.GetComponent<UIButton>();
        Gobj_VipEffect = _uiRoot.transform.FindChild("MainButtonPanel/HeadUI/VIPUIBG/lizitexiao").gameObject;
        Gobj_UnlockEffect = _uiRoot.transform.FindChild("MainButtonPanel/UnlockEffect").gameObject;
        Gobj_UnlockEffectFore = _uiRoot.transform.FindChild("MainButtonPanel/UnlockFore").gameObject;
        Layer2Relative = _uiRoot.transform.FindChild("MainCityBFG").gameObject.GetComponent<KeepPosRelativeToTarget>();
        
        #endregion
        #region BG
        ScrollView_MainCity = _uiRoot.transform.FindChild("MainCity").gameObject.GetComponent<UIScrollView>();
        Pnl_MainCity = _uiRoot.transform.FindChild("MainCity").gameObject.GetComponent<UIPanel>();
        Btn_MainGateButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/MainGate").gameObject.GetComponent<UIButton>();
        Btn_SlaveButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Slave").gameObject.GetComponent<UIButton>();
        Btn_LegionButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Legion").gameObject.GetComponent<UIButton>();
        Btn_ArenaButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Arena").gameObject.GetComponent<UIButton>();
        Btn_ExpeditionButton = _uiRoot.transform.FindChild("MainCityBG/Expedition").gameObject.GetComponent<UIButton>();
        Btn_BillboardButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Billboard").gameObject.GetComponent<UIButton>();
        Btn_BattlegroundButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Battleground").gameObject.GetComponent<UIButton>();
        Btn_AgainstButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Against").gameObject.GetComponent<UIButton>();
        Btn_ActivitiesButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Activity").gameObject.GetComponent<UIButton>();
        Btn_ShopButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Shop").gameObject.GetComponent<UIButton>();
        Btn_MailButton = _uiRoot.transform.FindChild("MainCity/MainCityFG/Mail").gameObject.GetComponent<UIButton>();
        #endregion
        #region 草丛
        Btn_Grass_1 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_1").gameObject.GetComponent<UIButton>();
        Btn_Grass_2 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_2").gameObject.GetComponent<UIButton>();
        Btn_Grass_3 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_3").gameObject.GetComponent<UIButton>();
        Btn_Grass_4 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_4").gameObject.GetComponent<UIButton>();
        SkeletonGrass_1 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_1/Grass_1").gameObject.GetComponent<SkeletonAnimation>();
        SkeletonGrass_2 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_2/Grass_2").gameObject.GetComponent<SkeletonAnimation>();
        SkeletonGrass_3 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_3/Grass_3").gameObject.GetComponent<SkeletonAnimation>();
        SkeletonGrass_4 = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Grass/Grass_4/Grass_4").gameObject.GetComponent<SkeletonAnimation>();
        #endregion
        #region 解锁
        MailNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Mail/Notice").gameObject;
        Lab_ShopName = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Shop/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Shop = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Shop/Label").gameObject.GetComponent<UILabel>();
        Spt_Shop = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Shop/Sprite").gameObject.GetComponent<UISprite>();
        Gobj_StoreTitle = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Shop").gameObject;
        Lab_MagicalLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Magical/Label").gameObject.GetComponent<UILabel>();
        Gobj_MagicalTitle = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Magical").gameObject;

        BattlegroundLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Battleground/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Battleground = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Battleground/Label").gameObject.GetComponent<UILabel>();
        Spt_Battleground = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Battleground/Sprite").gameObject.GetComponent<UISprite>();
        Spt_BattleGroundDouble = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Battleground/Double").gameObject.GetComponent<UISprite>();

        BillboardLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Billboard/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Billboard = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Billboard/Label").gameObject.GetComponent<UILabel>();
        Spt_Billboard = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Billboard/Sprite").gameObject.GetComponent<UISprite>();

        ActivityLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Activity/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Activity = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Activity/Label").gameObject.GetComponent<UILabel>();
        Spt_Activity = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Activity/Sprite").gameObject.GetComponent<UISprite>();
        Spt_ActivityDouble = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Activity/Double").gameObject.GetComponent<UISprite>();

        ArenaLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Arena/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Arena = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Arena/Label").gameObject.GetComponent<UILabel>();
        Spt_Arena = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Arena/Sprite").gameObject.GetComponent<UISprite>();

        LegionLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Legion/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Legion = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Legion/Label").gameObject.GetComponent<UILabel>();
        Spt_Legion = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Legion/Sprite").gameObject.GetComponent<UISprite>();
        Spt_LegionNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Legion/Notice").gameObject.GetComponent<UISprite>();

        SlaveLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Slave/Sprite1").gameObject.GetComponent<UILabel>();
        Lab_Slave = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Slave/Label").gameObject.GetComponent<UILabel>();
        Spt_Slave = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Slave/Sprite").gameObject.GetComponent<UISprite>();
        SlaveNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Slave/Notice").gameObject;
        Gobj_SlaveTitle = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Slave").gameObject;

        Lab_BaZhu = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/BaZhu/Label").gameObject.GetComponent<UILabel>();
        Spt_BaZhu = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/BaZhu/Sprite").gameObject.GetComponent<UISprite>();
        BaZhuNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/BaZhu/Notice").gameObject;

        AgainstLab = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Against/Label").gameObject.GetComponent<UILabel>();
        Spt_RecruitNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Against/Notice").gameObject.GetComponent<UISprite>();
        Spt_Recruit = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/Against/Sprite").gameObject.GetComponent<UISprite>();

        Gobj_YuanBaoTitle = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/YuanBao").gameObject;
        YuanBaoNotice = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/YuanBao/Notice").gameObject;
        Lab_YuanBao = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/YuanBao/Label").gameObject.GetComponent<UILabel>();
        Spt_YuanBao = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/YuanBao/Sprite").gameObject.GetComponent<UISprite>();

        Gobj_TitlePaiWeiSai = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/PaiWeiSai").gameObject;
        Lbl_PaiWeiSai = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/PaiWeiSai/Label").gameObject.GetComponent<UILabel>();
        Spt_PaiWeiSai = _uiRoot.transform.FindChild("MainCity/MainCityFG/Title/PaiWeiSai/Sprite").gameObject.GetComponent<UISprite>();

        Lab_Expedition = _uiRoot.transform.FindChild("MainCityBG/ExpeditionTitle/Label").gameObject.GetComponent<UILabel>();
        Spt_Expedition = _uiRoot.transform.FindChild("MainCityBG/ExpeditionTitle/Sprite").gameObject.GetComponent<UISprite>();
        ExpeditionLab = _uiRoot.transform.FindChild("MainCityBG/ExpeditionTitle/Sprite1").gameObject.GetComponent<UILabel>();
        Spt_RecruitNotice.gameObject.SetActive(false);
        #endregion
        ChatNoticeAnim = Btn_ChatButton.transform.FindChild("Chat_Talk/MainGateBG_Anim").GetComponent<SkeletonAnimation>();
        #region 跑马灯
        Gobj_MarqueePanel = _uiRoot.transform.FindChild("gobj_MarqueePanel").gameObject;
        Gobj_MarqueeGroup = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup").gameObject;
        Lbl_Marquee1 = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup/Marquee1").gameObject.GetComponent<UILabel>();
        Lbl_Marquee2 = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup/Marquee2").gameObject.GetComponent<UILabel>();
        Lbl_Marquee3 = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup/Marquee3").gameObject.GetComponent<UILabel>();
        Lbl_Marquee4 = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup/Marquee4").gameObject.GetComponent<UILabel>();
        Lbl_Marquee5 = _uiRoot.transform.FindChild("gobj_MarqueePanel/gobj_MarqueeGroup/Marquee5").gameObject.GetComponent<UILabel>();
        #endregion

        Tran_MainCityFG = _uiRoot.transform.FindChild("MainCity/MainCityFG");
        
        SetBoundary();

        Btn_BaZhu = _uiRoot.transform.FindChild("MainCity/MainCityFG/BaZhu").gameObject.GetComponent<UIButton>();
        BaZhuEffect = _uiRoot.transform.FindChild("MainCity/MainCityFG/BaZhu/Effect").gameObject;
        Spt_ExpeditionDouble = _uiRoot.transform.FindChild("MainCityBG/ExpeditionTitle/Double").gameObject.GetComponent<UISprite>();

        SetLabelValues();

    }
    public void Unitialize()
    {

    }
    public void SetBoundary()
    {
        Boundary.left = 512f;
        Boundary.right = 512f;
        Boundary.up = 288f;
        Boundary.down = -288f;
    }
    private void SetLabelValues()
    {
        Spt_BattleGroundDouble.gameObject.SetActive(false);
        Spt_ActivityDouble.gameObject.SetActive(false);
        Spt_ExpeditionDouble.gameObject.SetActive(false);
        Tscle_qipao.gameObject.SetActive(false);
        ChatNotice.gameObject.SetActive(false);
        Lab_PlayerVip.text ="VIP0";
        Lab_PlayerName.text ="";
        Lab_PlayerLevel.text ="0";
        AgainstLab.text = "";
        Lbl_BazhuName.text = "";
    }

    public void SetVisible(bool IsVisible)
    {
        _uiRoot.SetActive(IsVisible);
    }
	
}
