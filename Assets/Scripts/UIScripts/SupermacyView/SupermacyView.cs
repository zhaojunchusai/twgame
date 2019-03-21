using UnityEngine;
using System;
using System.Collections;

public class SupermacyView
{
    public static string UIName ="SupermacyView";
    public GameObject _uiRoot;
    public UISprite Spt_MaskSprite;
    public GameObject Gobj_MainGroup;
    public UIPanel UIPanel_gobj_MainGroup;
    public UIButton Btn_Exit;
    public UISprite Spt_BtnExitExitBG;
    public UISprite Spt_TotalBg;
    public UILabel Lbl_Title;
    public UISprite Spt_LeftTitleBG;
    public UISprite Spt_RightTitleBG;
    public UISprite Spt_LeftDecorate;
    public UISprite Spt_RightDecorate;
    public UIButton Btn_RuleDesc;
    public UISprite Spt_BtnRuleDescRuleBg;
    public UILabel Lbl_BtnRuleDescRuleDesc;
    public UISprite Spt_TimeBG;
    public UILabel Lbl_TimeLabel;
    public UILabel Lbl_StateLabel;
    public UISprite Spt_SelfBG;
    public UILabel Lbl_MyOrder;
    public UILabel Lbl_MyDamage;
    public UILabel Lbl_MyNick;
    public UISprite Spt_OrderBG;
    public UIPanel UIPanel_OrderListWindow;
    public UIScrollView ScrView_OrderListWindow;
    public UITable UITable_Order;
    public UILabel Lbl_Damage;
    public UILabel Lbl_Order;
    public UISprite Spt_OSprite;
    public UILabel Lbl_Nick;
    public UISprite Spt_AwardBG;
    public UILabel Lbl_Tittle;
    public UISprite Spt_LeftSymbol;
    public UISprite Spt_RightSymbol;
    public UIPanel UIPanel_AwardListWindow;
    public UIScrollView ScrView_AwardListWindow;
    public UITable UITable_Award;
    public UISprite Spt_ASprite;
    public UILabel Lbl_AOrder;
    public UISprite Spt_AIcon1;
    public UILabel Lbl_AValue1;
    public UISprite Spt_AIcon2;
    public UILabel Lbl_AValue2;
    public UISprite Spt_RightLine;
    public UISprite Spt_LeftLine;
    public UIButton Btn_WorshipBtn;
    public UISprite Spt_BtnWorshipBtnWorshipBg;
    public UILabel Lbl_BtnWorshipBtnWorshipDesc;
    public UIButton Btn_ChallengeBtn;
    public UISprite Spt_BtnChallengeBtnChallengBg;
    public UILabel Lbl_BtnChallengeBtnChallengDesc;
    public GameObject Gobj_SupermacyRole;
    public GameObject Gobj_FemaleRole;
    public UISprite Spt_PicBG;
    public UISprite Spt_SupermacyBG;
    public UISprite Spt_NamePic;
    public UILabel Lbl_NameText;
    public UISprite Spt_HPSlider;
    public UISlider Slider_HPSlider;
    public UISprite Spt_SliderHPSliderHPBack;
    public UISprite Spt_SliderHPSliderHPFore;
    //public UILabel Lbl_MaxHP;
    public UILabel Lbl_CurHP;
    //public UILabel Lbl_HPLabel;
    public UIButton Btn_ByGoldBtn;
    public UISprite Spt_BtnByGoldBtnGoldBg;
    public UILabel Lbl_BtnByGoldBtnGoldPrice;
    public UISprite Spt_BtnByGoldBtnGold;
    public UILabel Lbl_BtnByGoldBtnGoldHint;
    public UIButton Btn_ByDiamondBtn;
    public UISprite Spt_BtnByDiamondBtnDiamondBg;
    public UILabel Lbl_BtnByDiamondBtnDiamondPrice;
    public UISprite Spt_BtnByDiamondBtnDiamond;
    public UILabel Lbl_BtnByDiamondBtnDiamondHint;
    public UILabel Lbl_DescLabel;
    public UILabel Lbl_EffectLabel;
    public UISprite Spt_AutoBG;
    public UISprite Spt_BattleCheck;
    public UIButton Btn_BattleCheck;
    public GameObject Gobj_BattleGetCheck;
    public UILabel Lbl_AutoBattleLabel;
    public UISprite Spt_ClearCDCheck;
    public UIButton Btn_ClearCDCheck;
    public GameObject Gobj_CDGetCheck;
    public UILabel Lbl_AutoClearCDLabel;
    public GameObject Gobj_SupermacyRule;
    public UIPanel UIPanel_gobj_SupermacyRule;
    public UIPanel UIPanel_RuleDesc;
    public UIScrollView ScrView_RuleDesc;
    public UITable UITable_Rule;
    public UISprite Spt_Bg;
    public UILabel Lbl_RTitle;
    public UISprite Spt_RLeftTitleBG;
    public UISprite Spt_RRightTitleBG;
    public UISprite Spt_RBg;
    public GameObject Gobj_RuleMask;
    public UILabel Lbl_RuleDesc;
    public UIButton Btn_RuleExit;
    public UIButton Btn_Rank;
    public UILabel Lbl_WDes;
    public UILabel Lbl_WTittle;
    public UILabel Lbl_CDTIME;

    public GameObject Gobj_RankAward;
    public UIPanel UIPanel_gobl_RankAward;
    public GameObject Gobj_RA_Mask;
    public GameObject Gobj_Notice;
    public GameObject Gobj_GotTip;

    //自定义组件//
    public Transform Obj_Order;
    public Transform Obj_Award;
    public Transform Obj_RankItem;
    public Transform Obj_AwardItem;



    //新奖励系统组件//
    public GameObject Gobj_RankAwardNew;
    public GameObject Gobj_Mask;
    public UIScrollView ScrView_RankAwardDesc;
    public UISprite Spt_RankSprite;
    public UILabel Lbl_SelfRankDesc;
    public UILabel Lbl_UnionRankDesc;
    public UITable Tbl_RankAward;
    //public UITable Tbl_SelfGetAwards;
    //public UITable Tbl_UnionGetAwards;
    public GameObject Gobj_UnionRankAward;

    public GameObject Gobj_UnionRankGetAwards;
    public GameObject Gobj_SelfRankGetAwards;
    public GameObject Gobj_SelfGetAwards;
    public GameObject Gobj_UnionGetAwards;
    public GameObject Gobj_BattleTime;


    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/SupermacyView");

        Gobj_RankAwardNew = _uiRoot.transform.FindChild("gobj_RankAwardNew").gameObject;
        Gobj_Mask= _uiRoot.transform.FindChild("gobj_RankAwardNew/Mask").gameObject;
        ScrView_RankAwardDesc = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc").gameObject.GetComponent<UIScrollView>();
        Gobj_SelfGetAwards= _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/SelfRank").gameObject;
        Gobj_UnionGetAwards = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/UnionRank").gameObject;
        Spt_RankSprite = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/RankSprite").gameObject.GetComponent<UISprite>();
        Gobj_UnionRankGetAwards = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/UnionAwardDescGroup/AwardDescGroup").gameObject;
        Gobj_SelfRankGetAwards = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/SelfAwardDescGroup/AwardDescGroup").gameObject;
        Tbl_RankAward = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward").gameObject.GetComponent<UITable>();
        Lbl_SelfRankDesc= _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/SelfRank/SelfRankDesc").gameObject.GetComponent<UILabel>();
        Lbl_UnionRankDesc = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/UnionRank/UnionRankDesc").gameObject.GetComponent<UILabel>();
        Gobj_BattleTime=_uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/Rank/BattleTime").gameObject;
        Gobj_UnionRankAward = _uiRoot.transform.FindChild("gobj_RankAwardNew/Anim/RankAwardDesc/RankAward/UnionAwardDescGroup").gameObject;
        //Spt_MaskSprite = _uiRoot.transform.FindChild("MaskSprite").gameObject.GetComponent<UISprite>();
        Gobj_MainGroup = _uiRoot.transform.FindChild("gobj_MainGroup").gameObject;
        UIPanel_gobj_MainGroup = _uiRoot.transform.FindChild("gobj_MainGroup").gameObject.GetComponent<UIPanel>();
        Btn_Exit = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/Exit").gameObject.GetComponent<UIButton>();
        Spt_BtnExitExitBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/Exit/ExitBG").gameObject.GetComponent<UISprite>();
        Spt_TotalBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/TotalBg").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/TitleGroup/Title").gameObject.GetComponent<UILabel>();
        Spt_LeftTitleBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/TitleGroup/LeftTitleBG").gameObject.GetComponent<UISprite>();
        Spt_RightTitleBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/TitleGroup/RightTitleBG").gameObject.GetComponent<UISprite>();
        Spt_LeftDecorate = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/DecorateGroup/LeftDecorate").gameObject.GetComponent<UISprite>();
        Spt_RightDecorate = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BGGroup/DecorateGroup/RightDecorate").gameObject.GetComponent<UISprite>();
        Btn_RuleDesc = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/RuleDesc").gameObject.GetComponent<UIButton>();
        Spt_BtnRuleDescRuleBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/RuleDesc/RuleBg").gameObject.GetComponent<UISprite>();
        Lbl_BtnRuleDescRuleDesc = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/RuleDesc/RuleDesc").gameObject.GetComponent<UILabel>();
        Spt_TimeBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/TimeCount/TimeBG").gameObject.GetComponent<UISprite>();
        Lbl_TimeLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/TimeCount/TimeLabel").gameObject.GetComponent<UILabel>();
        Lbl_StateLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/TimeCount/StateLabel").gameObject.GetComponent<UILabel>();
        Spt_SelfBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/SelfOrder/SelfBG").gameObject.GetComponent<UISprite>();
        Lbl_MyOrder = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/SelfOrder/MyOrder").gameObject.GetComponent<UILabel>();
        Lbl_MyDamage = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/SelfOrder/MyDamage").gameObject.GetComponent<UILabel>();
        Lbl_MyNick = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/SelfOrder/MyNick").gameObject.GetComponent<UILabel>(); 
        Spt_OrderBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderBG").gameObject.GetComponent<UISprite>();
        UIPanel_OrderListWindow = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow").gameObject.GetComponent<UIPanel>();
        ScrView_OrderListWindow = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow").gameObject.GetComponent<UIScrollView>();
        UITable_Order = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order").gameObject.GetComponent<UITable>();
        Lbl_Damage = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order/Order/Damage").gameObject.GetComponent<UILabel>();
        Lbl_Order = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order/Order/Order").gameObject.GetComponent<UILabel>();
        Spt_OSprite = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order/Order/OSprite").gameObject.GetComponent<UISprite>();
        Lbl_Nick = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order/Order/Nick").gameObject.GetComponent<UILabel>();
        Spt_AwardBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardBG").gameObject.GetComponent<UISprite>();
        Lbl_Tittle = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/TitleGroup/Tittle").gameObject.GetComponent<UILabel>();
        Spt_LeftSymbol = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/TitleGroup/LeftSymbol").gameObject.GetComponent<UISprite>();
        Spt_RightSymbol = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/TitleGroup/RightSymbol").gameObject.GetComponent<UISprite>();
        UIPanel_AwardListWindow = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow").gameObject.GetComponent<UIPanel>();
        ScrView_AwardListWindow = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow").gameObject.GetComponent<UIScrollView>();
        UITable_Award = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award").gameObject.GetComponent<UITable>();
        Spt_ASprite = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/ASprite").gameObject.GetComponent<UISprite>();
        Lbl_AOrder = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/AOrder").gameObject.GetComponent<UILabel>();
        Spt_AIcon1 = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/AIcon1").gameObject.GetComponent<UISprite>();
        Lbl_AValue1 = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/AValue1").gameObject.GetComponent<UILabel>();
        Spt_AIcon2 = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/AIcon2").gameObject.GetComponent<UISprite>();
        Lbl_AValue2 = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award/AValue2").gameObject.GetComponent<UILabel>();
        Spt_RightLine = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/Line/RightLine").gameObject.GetComponent<UISprite>();
        Spt_LeftLine = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/Line/LeftLine").gameObject.GetComponent<UISprite>();
        Btn_WorshipBtn = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/WorshipBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnWorshipBtnWorshipBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/WorshipBtn/WorshipBg").gameObject.GetComponent<UISprite>();
        Lbl_BtnWorshipBtnWorshipDesc = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/WorshipBtn/WorshipDesc").gameObject.GetComponent<UILabel>();
        Gobj_Notice= _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/WorshipBtn/Notice").gameObject;
        Gobj_GotTip=_uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/WorshipBtn/GotTip").gameObject;
        Btn_ChallengeBtn = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/ChallengeBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnChallengeBtnChallengBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/ChallengeBtn/ChallengBg").gameObject.GetComponent<UISprite>();
        Lbl_BtnChallengeBtnChallengDesc = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/ChallengeBtn/ChallengDesc").gameObject.GetComponent<UILabel>();
        Lbl_CDTIME= _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyButtonGroup/ChallengeBtn/CDTIME").gameObject.GetComponent<UILabel>();
        Gobj_SupermacyRole = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/SupermacyRole").gameObject;
        Gobj_FemaleRole = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/FemaleRole").gameObject;
        Spt_PicBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/BGGroup/PicBG").gameObject.GetComponent<UISprite>();
        Spt_SupermacyBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/BGGroup/SupermacyBG").gameObject.GetComponent<UISprite>();
        Spt_NamePic = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/BGGroup/NamePic").gameObject.GetComponent<UISprite>();
        Lbl_NameText = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/BGGroup/NameText").gameObject.GetComponent<UILabel>();
        Spt_HPSlider = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPSlider").gameObject.GetComponent<UISprite>();
        Slider_HPSlider = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPSlider").gameObject.GetComponent<UISlider>();
        Spt_SliderHPSliderHPBack = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPSlider/HPBack").gameObject.GetComponent<UISprite>();
        Spt_SliderHPSliderHPFore = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPSlider/HPFore").gameObject.GetComponent<UISprite>();
        //Lbl_MaxHP = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPLabel/MaxHP").gameObject.GetComponent<UILabel>();
        Lbl_CurHP = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPLabel/CurHP").gameObject.GetComponent<UILabel>();
        //Lbl_HPLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/SupermacyWindow/HPGroup/HPLabel/HPLabel").gameObject.GetComponent<UILabel>();
        Btn_ByGoldBtn = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByGoldBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnByGoldBtnGoldBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByGoldBtn/GoldBg").gameObject.GetComponent<UISprite>();
        Lbl_BtnByGoldBtnGoldPrice = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByGoldBtn/GoldPrice").gameObject.GetComponent<UILabel>();
        Spt_BtnByGoldBtnGold = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByGoldBtn/Gold").gameObject.GetComponent<UISprite>();
        Lbl_BtnByGoldBtnGoldHint = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByGoldBtn/GoldHint").gameObject.GetComponent<UILabel>();
        Btn_ByDiamondBtn = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByDiamondBtn").gameObject.GetComponent<UIButton>();
        Spt_BtnByDiamondBtnDiamondBg = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByDiamondBtn/DiamondBg").gameObject.GetComponent<UISprite>();
        Lbl_BtnByDiamondBtnDiamondPrice = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByDiamondBtn/DiamondPrice").gameObject.GetComponent<UILabel>();
        Spt_BtnByDiamondBtnDiamond = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByDiamondBtn/Diamond").gameObject.GetComponent<UISprite>();
        Lbl_BtnByDiamondBtnDiamondHint = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/ByDiamondBtn/DiamondHint").gameObject.GetComponent<UILabel>();
        Lbl_DescLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/BuffEffectLabel/DescLabel").gameObject.GetComponent<UILabel>();
        Lbl_EffectLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/BuffButtonGroup/BuffEffectLabel/EffectLabel").gameObject.GetComponent<UILabel>();
        Spt_AutoBG = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoBG").gameObject.GetComponent<UISprite>();
        Spt_BattleCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoBattle/BattleCheck").gameObject.GetComponent<UISprite>();
        Btn_BattleCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoBattle/BattleCheck").gameObject.GetComponent<UIButton>();
        Gobj_BattleGetCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoBattle/BattleCheck/gobj_BattleGetCheck").gameObject;
        Lbl_AutoBattleLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoBattle/AutoBattleLabel").gameObject.GetComponent<UILabel>();
        Spt_ClearCDCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoClearCD/ClearCDCheck").gameObject.GetComponent<UISprite>();
        Btn_ClearCDCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoClearCD/ClearCDCheck").gameObject.GetComponent<UIButton>();
        Gobj_CDGetCheck = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoClearCD/ClearCDCheck/gobj_CDGetCheck").gameObject;
        Lbl_AutoClearCDLabel = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/AutoSelectionGroup/AutoClearCD/AutoClearCDLabel").gameObject.GetComponent<UILabel>();
        Gobj_SupermacyRule = _uiRoot.transform.FindChild("gobj_SupermacyRule").gameObject;
        UIPanel_gobj_SupermacyRule = _uiRoot.transform.FindChild("gobj_SupermacyRule").gameObject.GetComponent<UIPanel>();
        UIPanel_RuleDesc = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/RuleDesc").gameObject.GetComponent<UIPanel>();
        ScrView_RuleDesc = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/RuleDesc").gameObject.GetComponent<UIScrollView>();
        UITable_Rule = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/RuleDesc/Rule").gameObject.GetComponent<UITable>();
        Spt_Bg = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/BGGroup/Bg").gameObject.GetComponent<UISprite>();
        Lbl_RTitle = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/BGGroup/TitleGroup/RTitle").gameObject.GetComponent<UILabel>();
        Spt_RLeftTitleBG = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/BGGroup/TitleGroup/RLeftTitleBG").gameObject.GetComponent<UISprite>();
        Spt_RRightTitleBG = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/BGGroup/TitleGroup/RRightTitleBG").gameObject.GetComponent<UISprite>();
        Spt_RBg = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/BGGroup/RBg").gameObject.GetComponent<UISprite>();
        Gobj_RuleMask = _uiRoot.transform.FindChild("gobj_SupermacyRule/Mask").gameObject;
        Lbl_RuleDesc = _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/RuleDesc/Rule/RuleDesc").gameObject.GetComponent<UILabel>();
        Btn_RuleExit= _uiRoot.transform.FindChild("gobj_SupermacyRule/Anim/RuleExit").gameObject.GetComponent<UIButton>();
        Btn_Rank = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardPanel/Rank").gameObject.GetComponent<UIButton>();
        Lbl_WDes = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardPanel/Worship/WDes").gameObject.GetComponent<UILabel>();
        Lbl_WTittle = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardPanel/Worship/WTittle").gameObject.GetComponent<UILabel>();
        Gobj_RankAward = _uiRoot.transform.FindChild("gobj_RankAward").gameObject;
        UIPanel_gobl_RankAward = _uiRoot.transform.FindChild("gobj_RankAward").gameObject.GetComponent<UIPanel>();
        Gobj_RA_Mask = _uiRoot.transform.FindChild("gobj_RankAward/RA_Mask").gameObject;
        
        Obj_Order = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/OrderList/OrderListWindow/Order/Order");
        Obj_Award = _uiRoot.transform.FindChild("gobj_MainGroup/Anim/ListGroup/AwardList/AwardListWindow/Award/Award");
        Obj_RankItem = _uiRoot.transform.FindChild("gobj_RankAward/RA_Anim/RankPanel/RankItem");
        Obj_AwardItem = _uiRoot.transform.FindChild("gobj_RankAward/RA_Anim/RankPanel/RankItem/AwardItem");

        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = ConstString.SUPERMACY_SUPERMACY;
        Lbl_BtnRuleDescRuleDesc.text = ConstString.TITLE_RULE;
        Lbl_TimeLabel.text = "00:00:00";
        Lbl_StateLabel.text = ConstString.SUPERMACY_TIMESTATE1;
        Lbl_MyOrder.text = "";
        Lbl_MyDamage.text = "";
        Lbl_MyNick.text = "";
        Lbl_Damage.text = "";
        Lbl_Order.text = "";
        Lbl_Tittle.text = "排名獎勵";
        Lbl_BtnWorshipBtnWorshipDesc.text = ConstString.SUPERMACY_WORSHIPSUPERMACY;
        Lbl_BtnChallengeBtnChallengDesc.text = ConstString.SUPERMACY_CHALLENGE;
        Lbl_NameText.text = ConstString.SUPERMACY_SUPERMACY;
        //Lbl_MaxHP.text = "0";
        Lbl_CurHP.text = "100%";
        //Lbl_HPLabel.text = "/";
        Lbl_BtnByGoldBtnGoldPrice.text = "0";
        Lbl_BtnByDiamondBtnDiamondPrice.text = "0";
        Lbl_BtnByGoldBtnGoldHint.text = "購買0/0";
        Lbl_BtnByDiamondBtnDiamondHint.text = "購買0/0";
        Lbl_DescLabel.text = "神威附體: 增加攻擊+";
        Lbl_EffectLabel.text = "0%";
        Lbl_AutoBattleLabel.text = "自動戰鬥";
        Lbl_AutoClearCDLabel.text = "快速清除CD";
        Lbl_RuleDesc.text = "";
        Lbl_WTittle.text = ConstString.SUPERMACY_WORSHIPAWARD;
        Lbl_RTitle.text = "霸主規則";
		Lbl_WDes.text = "";
        Lbl_CDTIME.text = "";
        Lbl_SelfRankDesc.text = "";
        Lbl_UnionRankDesc.text = "";
    }

    public void Uninitialize()
    {
        
    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);

    }

}
