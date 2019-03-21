using UnityEngine;
using System;
using System.Collections;

public class FightView
{
    public static string UIName = "FightView";
    public GameObject _uiRoot;
    public UIProgressBar UIProgressBar_Progress_Bar;
    public UISprite Spt_Progress_Mark;
    public UILabel Lbl_Time_Content;
    public UILabel Lbl_Fight_Score;
    public UISprite Spt_Fight_Star;
    public UIButton Btn_Main_Pause;
    public UIButton Btn_Main_Quicken;
    public UISprite Spt_BtnMain_QuickenSpeed;
    public UISprite Spt_BtnMain_LockQuicken;
    public UIButton Btn_Main_Auto;
    public UISprite Spt_BtnMain_AutoBG;
    public UISprite Spt_BtnMain_AutoLock;
    public Transform Trans_BtnMain_AutoEffect;
    public UIButton Btn_Main_AutoSummon;
    public UISprite Spt_BtnMain_AutoSummonBG;
    public UISprite Spt_BtnMain_AutoSummonLock;
    public Transform Trans_BtnMain_AutoSummonEffect;
    public UISprite Spt_MoveHint_Left;
    public UISprite Spt_MoveHint_Right;
    public UIProgressBar UIProgressBar_PVE_Energy;
    public UILabel Lbl_Energy_Title;
    public UIProgressBar UIProgressBar_PVE_Magic;
    public UILabel Lbl_Magic_Title;
    public UIButton Btn_PVE_PetSkill;
    public UISprite Spt_PetSkill_Icon;
    public UISprite Spt_PetSkill_Slider;
    public UISprite Spt_PetSkill_Mask;
    public GameObject Obj_PetSkill_Effect;
    public UIProgressBar UIProgressBar_Self_Magic;
    public UILabel Lbl_SMagic_Title;
    public UIButton Btn_Self_PetSkill;
    public UISprite Spt_Self_PetSkill_Icon;
    public UISprite Spt_Self_PetSkill_Slider;
    public UISprite Spt_Self_PetSkill_Mask;
    public GameObject Obj_Self_PetSkill_Effect;
    public UIProgressBar UIProgressBar_Enemy_Magic;
    public UILabel Lbl_EMagic_Title;
    public UIButton Btn_Enemy_PetSkill;
    public UISprite Spt_Enemy_PetSkill_Icon;
    public UISprite Spt_Enemy_PetSkill_Slider;
    public UISprite Spt_Enemy_PetSkill_Mask;
    public GameObject Obj_Enemy_PetSkill_Effect;
    public UIPanel UIPanel_HurtHint;
    public UIPanel UIPanel_FightPause;
    public UIButton Btn_Pause_ReStart;
    public UIButton Btn_Pause_Exit;
    public UIButton Btn_Pause_Keep;
    public UISprite Spt_Pause_BG;
    public UIPanel UIPanel_FightResult;
    public UIButton Btn_Result_Hint;
    public TweenAlpha TweenAlpha_Result_Hint;
    public UISprite Spt_Result_Title;
    public UILabel Lbl_Activity_Hint;
    public UIButton Btn_Activity_ReStart;
    public UIButton Btn_Activity_Exit;
    public UIButton Btn_Activity_Next;
    public UILabel Lbl_Activity_Gold;
    public UILabel Lbl_Activity_EXP;
    public UISprite Spt_Activity_Star_0;
    public UISprite Spt_Activity_Star_1;
    public UISprite Spt_Activity_Star_2;
    public UISprite Spt_Activity_ItemBG;
    public UIPanel UIPanel_Activity_ItemList;
    public UIGrid Grd_Activity_ItemGrid;

    public UIButton Btn_Endless_Detail;
    public UILabel Lbl_Endless_EXP;
    public UILabel Lbl_Endless_Gold;
    public UISprite Spt_Endless_ItemBG;
    public UISprite Spt_Endless_ItemQuality;
    public UILabel Lbl_Endless_Num;
    public UILabel Lbl_Endless_CurScore;
    public UILabel Lbl_Endless_MaxScore;
    public UIPanel UIPanel_Endless_ItemList;
    public UIGrid Grd_Endless_ItemGrid;
    public TweenPosition TP_Cur_Integral;
    public UILabel Lbl_Cur_Integral;

    public TweenPosition TP_CSW_IntegralTitle;
    public UILabel Lbl_CSW_IntegralValue;
    public TweenPosition TP_CSW_AwardTitle;
    public UILabel Lbl_CSW_AwardValue;

    public UILabel Lbl_ST_Title;
    public TweenPosition TP_ST_Title;
    public UILabel Lbl_ST_FightHurt;
    public TweenPosition TP_ST_FightHurt;
    public UILabel Lbl_ST_FightHurtValue;
    public TweenPosition TP_ST_FightHurtValue;
    public UILabel Lbl_ST_BUFFHurt;
    public TweenPosition TP_ST_BUFFHurt;
    public UILabel Lbl_ST_BUFFHurtValue;
    public TweenPosition TP_ST_BUFFHurtValue;
    public UILabel Lbl_ST_TotleHurt;
    public TweenPosition TP_ST_TotleHurt;
    public UILabel Lbl_ST_TotleHurtValue;
    public TweenPosition TP_ST_TotleHurtValue;
    public UILabel Lbl_ST_Award;
    public TweenPosition TP_ST_Award;
    public UILabel Lbl_ST_AwardValue;
    public TweenPosition TP_ST_AwardValue;

    public UILabel Lbl_Qualifying_Title;
    public TweenPosition TP_Qualifying_Title;
    public UILabel Lbl_Qualifying_Pre;
    public TweenScale TS_Qualifying_Pre;
    public UILabel Lbl_Qualifying_Cur;
    public TweenScale TS_Qualifying_Cur;
    public TweenScale TS_Qualifying_UpIcon;
    public TweenScale TS_Qualifying_BG_UpHint;
    public UILabel Lbl_Qualifying_UpHint;


    public UISprite Spt_Expedition_ItemBG;
    public UISprite Spt_Expedition_ItemQuality;
    public UILabel Lbl_Expedition_Num;
    public UILabel Lbl_PVP_Hint;
    public UILabel Lbl_PVP_Pre;
    public UILabel Lbl_PVP_Cur;
    public UISprite Spt_PVP_UpIcon;
    public UISprite Spt_PVP_BG_UpHint;
    public UILabel Lbl_UpHint;
    public UISprite Spt_Fail_Hint_Activity;

    public UILabel Lbl_PVP_His_Hint;
    public UISprite Spt_PVP_His_UpIcon;
    public UILabel Lbl_PVP_His_Pre;
    public UILabel Lbl_PVP_His_Cur;

    public UILabel Lbl_PVP_Rew_Hint;
    public UISprite Spt_PVP_Rew_UpIcon;
    public UILabel Lbl_PVP_Rew_Cur;
    
    public TweenPosition TP_Union_HurtTitle;
    public UILabel Lbl_Slave_Hint;

    public TweenPosition TP_Union_RewardTitle;
    public UILabel Lbl_Union_HurtValue;
    public UISprite Spt_Union_RewardIconGold;
    public UILabel Lbl_Union_RewardValueGold;
    public UISprite Spt_Union_RewardIconToken;
    public UILabel Lbl_Union_RewardValueToken;

    public UIPanel UIPanel_FightStart;
    public UISprite Spt_Start_Content;
    public UILabel Lbl_Start_Endless_Count;
    public UILabel Lbl_Start_Endless_Hint;
    public TweenPosition TP_Start_Info;
    public TweenAlpha TA_Start_SuccessIcon;
    public TweenScale TS_Start_SuccessIcon;
    public TweenRotation TR_Start_SuccessIcon;

    public UIWidget UIWidget_Main_StageInfo;
    public UISprite Spt_StageInfo_VS;
    public UISprite Spt_Self_Icon;
    public UILabel Lbl_Self_Info;
    public UISlider Sli_Self_Progress;
    public UISprite Spt_Enemy_Icon;
    public UILabel Lbl_Enemy_Info;
    public UISlider Sli_Enemy_Progress;

    public UILabel Lbl_Fail_Hint_Info;


    public UIWidget UIWidget_Main_PVPInfo;
    public UILabel Lbl_PVPInfo_Text;
    public UISlider Sli_SelfPVP_ProgressUp;
    public UISlider Sli_SelfPVP_ProgressDown;
    public UILabel Lbl_SelfPVP_Info;
    public UISlider Sli_EnemyPVP_ProgressUp;
    public UISlider Sli_EnemyPVP_ProgressDown;
    public UILabel Lbl_EnemyPVP_Info;

    public TweenPosition TP_Main_BossHint;




    //自定义组件--------------------------------------------------------------//
    public Transform Obj_Soldier_Item;
    public Transform Obj_Skill_Item;
    public Transform Obj_PlayerInfo_Fight;

    public Transform Obj_Result_Activity;
    public Transform Obj_Result_Endless;
    public Transform Obj_Result_Expedition;
    public Transform Obj_Result_PVP;
    public Transform Obj_Result_Slave;
    public Transform Obj_Result_Union;
    public Transform Obj_Result_Fail;
    public Transform Obj_Result_CaptureTerritory;
    public Transform Obj_Result_CrossServerWar;
    public Transform Obj_Result_ServerHegemony;
    public Transform Obj_Result_Qualifying;
    public Transform Obj_Result_BG_Down;

    public Transform Obj_PVE;
    public Transform Obj_PVP;
    public Transform Obj_PVE_Skill;
    public Transform Obj_PVE_Soldier;
    public Transform Obj_PVP_Self_Skill;
    public Transform Obj_PVP_Self_Soldier;
    public Transform Obj_PVP_Enemy_Skill;
    public Transform Obj_PVP_Enemy_Soldier;

    public Transform Obj_StageInfo_Self;
    public Transform Obj_StageInfo_Enemy;
    public Transform Obj_Main_Progress;

    public Transform Obj_Energy_Effect;
    public Transform Obj_Magic_Effect;
    public Transform Obj_Magic_FireEffect;
    //自定义组件--------------------------------------------------------------//
    public UISprite Result_TitleWin;
    public TweenPosition TitleWin_Position;
    public TweenScale TitleWin_Scale;
    public TweenRotation TitleWin_Rotation;
    public TweenAlpha TitleWin_Alpha;

    public TweenPosition Title_Position;
    public TweenScale Title_Scale;
    public TweenRotation Title_Rotation;
    public TweenAlpha Title_Alpha;

    public TweenScale Restart_Scale;
    public TweenScale Exit_Scale;
    public TweenScale Next_Scale;
    public TweenScale Detail_Scale;

    public TweenScale Star0_Scale;
    public TweenScale Star1_Scale;
    public TweenScale Star2_Scale;
    public TweenScale ItemBG_Scale;
    public TweenAlpha Star0_Alpha;
    public TweenAlpha Star1_Alpha;
    public TweenAlpha Star2_Alpha;
    public TweenPosition Gold_Position;
    public TweenPosition Exp_Position;
    //Fail战斗失败
    public TweenPosition FailHint_Position;
    public TweenScale FailItem0_Scale;
    public TweenScale FailItem1_Scale;
    public TweenScale FailItem2_Scale;
    public TweenScale FailItem3_Scale;
    public TweenScale FailItem4_Scale;
    
    public UIButton Btn_FailItem0;
    public UIButton Btn_FailItem1;
    public UIButton Btn_FailItem2;
    public UIButton Btn_FailItem3;
    public UIButton Btn_FailItem4;
    //endless
    public TweenScale EndlessExp_Scale;
    public TweenScale EndlessGold_Scale;
    public TweenScale EndlessCurScore_Scale;
    public TweenScale EndlessMaxScore_Scale;
    public TweenPosition EndlessNum_Position;
    //expedition
    public TweenScale ExpeditionItem_Scale;
    public TweenPosition ExpeditionHint_Position;
    //pvp
    public TweenPosition PVPHint_Position;
    public TweenScale PVPPre_Scale;
    public TweenScale PVPCur_Scale;
    public TweenScale PVPUphint_Scale;
    public TweenScale PVPUpicon_Scale;
    public SkeletonAnimation SA_Shengli;

    public TweenPosition TP_PVPHisHint;
    public TweenScale TS_PVPHisPre;
    public TweenScale TS_PVPHisCur;
    public TweenScale TS_PVPHisUpIcon;

    public TweenPosition TP_PVPRewHint;
    public TweenScale TS_PVPRewCur;
    public TweenScale TS_PVPRewUpIcon;
    //slave
    public TweenPosition SlaveHint_Position;

    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/FightView");
        PVPHint_Position = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Hint").gameObject.GetComponent<TweenPosition>();
        PVPPre_Scale = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Pre").gameObject.GetComponent<TweenScale>();
        PVPCur_Scale = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Cur").gameObject.GetComponent<TweenScale>();
        PVPUpicon_Scale = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_UpIcon").gameObject.GetComponent<TweenScale>();
        PVPUphint_Scale = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_BG_UpHint").gameObject.GetComponent<TweenScale>();

        TP_PVPHisHint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Hint").gameObject.GetComponent<TweenPosition>();
        TS_PVPHisPre = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Pre").gameObject.GetComponent<TweenScale>();
        TS_PVPHisCur = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Cur").gameObject.GetComponent<TweenScale>();
        TS_PVPHisUpIcon = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_UpIcon").gameObject.GetComponent<TweenScale>();

        TP_PVPRewHint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_Hint").gameObject.GetComponent<TweenPosition>();
        TS_PVPRewCur = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_Cur").gameObject.GetComponent<TweenScale>();
        TS_PVPRewUpIcon = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_UpIcon").gameObject.GetComponent<TweenScale>();

        ExpeditionHint_Position = _uiRoot.transform.FindChild("FightResult/Result_Expedition/Expedition_Hint").gameObject.GetComponent<TweenPosition>();
        ExpeditionItem_Scale = _uiRoot.transform.FindChild("FightResult/Result_Expedition/Expedition_ItemBG").gameObject.GetComponent<TweenScale>();
        EndlessNum_Position = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Num").gameObject.GetComponent<TweenPosition>();
        EndlessExp_Scale = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_EXP").gameObject.GetComponent<TweenScale >();
        EndlessGold_Scale = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Gold").gameObject.GetComponent<TweenScale>();
        EndlessCurScore_Scale = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_CurScore").gameObject.GetComponent<TweenScale>();
        EndlessMaxScore_Scale = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_MaxScore").gameObject.GetComponent<TweenScale>();
        Btn_Endless_Detail = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Detail").gameObject.GetComponent<UIButton>();

        Star0_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_0").gameObject.GetComponent<TweenScale>();
        Star1_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_1").gameObject.GetComponent<TweenScale>();
        Star2_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_2").gameObject.GetComponent<TweenScale>();
        Star0_Alpha = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_0").gameObject.GetComponent<TweenAlpha>();
        Star1_Alpha = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_1").gameObject.GetComponent<TweenAlpha>();
        Star2_Alpha = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_2").gameObject.GetComponent<TweenAlpha>();
        ItemBG_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ItemBG").gameObject.GetComponent<TweenScale>();
        Gold_Position = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Gold").gameObject.GetComponent<TweenPosition>();
        Exp_Position = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_EXP").gameObject.GetComponent<TweenPosition>();
        Title_Alpha = _uiRoot.transform.FindChild("FightResult/Result_Title").gameObject.GetComponent<TweenAlpha>();
        Title_Scale = _uiRoot.transform.FindChild("FightResult/Result_Title").gameObject.GetComponent<TweenScale >();
        Title_Position = _uiRoot.transform.FindChild("FightResult/Result_Title").gameObject.GetComponent<TweenPosition >();
        Title_Rotation =_uiRoot.transform.FindChild("FightResult/Result_Title").gameObject.GetComponent<TweenRotation >();
        TitleWin_Alpha = _uiRoot.transform.FindChild("FightResult/WinTitle/Result_TitleWin").gameObject.GetComponent<TweenAlpha>();
        TitleWin_Scale = _uiRoot.transform.FindChild("FightResult/WinTitle/Result_TitleWin").gameObject.GetComponent<TweenScale>();
        TitleWin_Position = _uiRoot.transform.FindChild("FightResult/WinTitle/Result_TitleWin").gameObject.GetComponent<TweenPosition>();
        TitleWin_Rotation = _uiRoot.transform.FindChild("FightResult/WinTitle/Result_TitleWin").gameObject.GetComponent<TweenRotation>();
        SA_Shengli = _uiRoot.transform.FindChild("FightResult/shengli").gameObject.GetComponent<SkeletonAnimation>();
        SlaveHint_Position = _uiRoot.transform.FindChild("FightResult/Result_Slave/Slave_Hint").gameObject.GetComponent<TweenPosition>();
        Restart_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ReStart").gameObject.GetComponent<TweenScale >();
        Exit_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Exit").gameObject.GetComponent<TweenScale>();
        Next_Scale = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Next").gameObject.GetComponent<TweenScale>();
        Detail_Scale = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Detail").gameObject.GetComponent<TweenScale>();
        FailHint_Position = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Hint_Activity").gameObject.GetComponent<TweenPosition>();
        FailItem0_Scale = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_0").gameObject.GetComponent<TweenScale>();
        FailItem1_Scale = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_1").gameObject.GetComponent<TweenScale>();
        FailItem2_Scale = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_2").gameObject.GetComponent<TweenScale>();
        FailItem3_Scale = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_3").gameObject.GetComponent<TweenScale>();
        FailItem4_Scale = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_4").gameObject.GetComponent<TweenScale>();
        Btn_FailItem0 = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_0").gameObject.GetComponent<UIButton>();
        Btn_FailItem1 = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_1").gameObject.GetComponent<UIButton>();
        Btn_FailItem2 = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_2").gameObject.GetComponent<UIButton>();
        Btn_FailItem3 = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_3").gameObject.GetComponent<UIButton>();
        Btn_FailItem4 = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Item_4").gameObject.GetComponent<UIButton>(); 

        UIProgressBar_Progress_Bar = _uiRoot.transform.FindChild("FightMain/Main_Progress/ProgressContent/Progress_Bar").gameObject.GetComponent<UIProgressBar>();
        Spt_Progress_Mark = _uiRoot.transform.FindChild("FightMain/Main_Progress/ProgressContent/Progress_Mark").gameObject.GetComponent<UISprite>();
        Lbl_Time_Content = _uiRoot.transform.FindChild("FightMain/Main_Progress/ProgressContent/Time_Content").gameObject.GetComponent<UILabel>();
        Lbl_Fight_Score = _uiRoot.transform.FindChild("FightMain/Main_PlayerInfo/PlayerInfo_Fight/Fight_BG_Down/Fight_BG_Middle/Fight_Score").gameObject.GetComponent<UILabel>();
        Spt_Fight_Star = _uiRoot.transform.FindChild("FightMain/Main_PlayerInfo/PlayerInfo_Fight/Fight_BG_Down/Fight_BG_Middle/Fight_Star").gameObject.GetComponent<UISprite>();
        Btn_Main_Pause = _uiRoot.transform.FindChild("FightMain/Main_Btn/Main_Pause").gameObject.GetComponent<UIButton>();
        Btn_Main_Quicken = _uiRoot.transform.FindChild("FightMain/Main_Btn/Main_Quicken").gameObject.GetComponent<UIButton>();
        Spt_BtnMain_QuickenSpeed = _uiRoot.transform.FindChild("FightMain/Main_Btn/Main_Quicken/Speed").gameObject.GetComponent<UISprite>();
        Spt_BtnMain_LockQuicken = _uiRoot.transform.FindChild("FightMain/Main_Btn/Main_Quicken/LockQuicken").gameObject.GetComponent<UISprite>(); 
        Btn_Main_Auto = _uiRoot.transform.FindChild("FightMain/Main_AutoFight").gameObject.GetComponent<UIButton>();
        Spt_BtnMain_AutoBG = _uiRoot.transform.FindChild("FightMain/Main_AutoFight/BGFight").gameObject.GetComponent<UISprite>();
        Spt_BtnMain_AutoLock = _uiRoot.transform.FindChild("FightMain/Main_AutoFight/LockFight").gameObject.GetComponent<UISprite>();
        Trans_BtnMain_AutoEffect = _uiRoot.transform.FindChild("FightMain/Main_AutoFight/EffectFight");
        Btn_Main_AutoSummon = _uiRoot.transform.FindChild("FightMain/Main_AutoSummon").gameObject.GetComponent<UIButton>();
        Spt_BtnMain_AutoSummonBG = _uiRoot.transform.FindChild("FightMain/Main_AutoSummon/BGSummon").gameObject.GetComponent<UISprite>();
        Spt_BtnMain_AutoSummonLock = _uiRoot.transform.FindChild("FightMain/Main_AutoSummon/LockSummon").gameObject.GetComponent<UISprite>();
        Trans_BtnMain_AutoSummonEffect = _uiRoot.transform.FindChild("FightMain/Main_AutoSummon/EffectSummon");
        Spt_MoveHint_Left = _uiRoot.transform.FindChild("FightMain/Main_MoveHint_Left/MoveHint_Left").gameObject.GetComponent<UISprite>();
        Spt_MoveHint_Right = _uiRoot.transform.FindChild("FightMain/Main_MoveHint_Right/MoveHint_Right").gameObject.GetComponent<UISprite>();
        UIProgressBar_PVE_Energy = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Energy").gameObject.GetComponent<UIProgressBar>();
        Lbl_Energy_Title = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Energy/Energy_Title").gameObject.GetComponent<UILabel>();
        UIProgressBar_PVE_Magic = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Magic").gameObject.GetComponent<UIProgressBar>();
        Lbl_Magic_Title = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Magic/Magic_Title").gameObject.GetComponent<UILabel>();
        Btn_PVE_PetSkill = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_PetSkill").gameObject.GetComponent<UIButton>();
        Spt_PetSkill_Icon = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_PetSkill/PetSkill_Icon").gameObject.GetComponent<UISprite>();
        Spt_PetSkill_Slider = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_PetSkill/PetSkill_Slider/PetSkill_Slider").gameObject.GetComponent<UISprite>();
        Spt_PetSkill_Mask = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_PetSkill/PetSkill_Mask").gameObject.GetComponent<UISprite>();
        Obj_PetSkill_Effect = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_PetSkill/PetSkill_Effect").gameObject;

        UIProgressBar_Self_Magic = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_Magic").gameObject.GetComponent<UIProgressBar>();
        Lbl_SMagic_Title = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_Magic/SMagic_Title").gameObject.GetComponent<UILabel>();
        Btn_Self_PetSkill = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_PetSkill").gameObject.GetComponent<UIButton>();
        Spt_Self_PetSkill_Icon = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_PetSkill/PetSkill_Icon").gameObject.GetComponent<UISprite>();
        Spt_Self_PetSkill_Slider = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_PetSkill/PetSkill_Slider/PetSkill_Slider").gameObject.GetComponent<UISprite>();
        Spt_Self_PetSkill_Mask = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_PetSkill/PetSkill_Mask").gameObject.GetComponent<UISprite>();
        Obj_Self_PetSkill_Effect = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_PetSkill/PetSkill_Effect").gameObject;

        UIProgressBar_Enemy_Magic = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_Magic").gameObject.GetComponent<UIProgressBar>();
        Lbl_EMagic_Title = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_Magic/EMagic_Title").gameObject.GetComponent<UILabel>();
        Btn_Enemy_PetSkill = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_PetSkill").gameObject.GetComponent<UIButton>();
        Spt_Enemy_PetSkill_Icon = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_PetSkill/PetSkill_Icon").gameObject.GetComponent<UISprite>();
        Spt_Enemy_PetSkill_Slider = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_PetSkill/PetSkill_Slider/PetSkill_Slider").gameObject.GetComponent<UISprite>();
        Spt_Enemy_PetSkill_Mask = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_PetSkill/PetSkill_Mask").gameObject.GetComponent<UISprite>();
        Obj_Enemy_PetSkill_Effect = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_PetSkill/PetSkill_Effect").gameObject;

        UIPanel_HurtHint = _uiRoot.transform.FindChild("HurtHint").gameObject.GetComponent<UIPanel>();
        UIPanel_FightPause = _uiRoot.transform.FindChild("FightPause").gameObject.GetComponent<UIPanel>();
        Btn_Pause_ReStart = _uiRoot.transform.FindChild("FightPause/Pause_ReStart").gameObject.GetComponent<UIButton>();
        Btn_Pause_Exit = _uiRoot.transform.FindChild("FightPause/Pause_Exit").gameObject.GetComponent<UIButton>();
        Btn_Pause_Keep = _uiRoot.transform.FindChild("FightPause/Pause_Keep").gameObject.GetComponent<UIButton>();
        Spt_Pause_BG = _uiRoot.transform.FindChild("FightPause/Pause_BG").gameObject.GetComponent<UISprite>();
        UIPanel_FightResult = _uiRoot.transform.FindChild("FightResult").gameObject.GetComponent<UIPanel>();
        Btn_Result_Hint = _uiRoot.transform.FindChild("FightResult/Result_Hint").gameObject.GetComponent<UIButton>();
        TweenAlpha_Result_Hint = _uiRoot.transform.FindChild("FightResult/Result_Hint").gameObject.GetComponent<TweenAlpha>();
        Spt_Result_Title = _uiRoot.transform.FindChild("FightResult/Result_Title").gameObject.GetComponent<UISprite>();
        Result_TitleWin = _uiRoot.transform.FindChild("FightResult/WinTitle/Result_TitleWin").gameObject.GetComponent<UISprite>();

        Lbl_Activity_Hint = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Hint").gameObject.GetComponent<UILabel>();
        Btn_Activity_ReStart = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ReStart").gameObject.GetComponent<UIButton>();
        Btn_Activity_Exit = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Exit").gameObject.GetComponent<UIButton>();
        Btn_Activity_Next = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Next").gameObject.GetComponent<UIButton>();
        Lbl_Activity_Gold = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Gold/Activity_Gold").gameObject.GetComponent<UILabel>();
        Lbl_Activity_EXP = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_EXP/Activity_EXP").gameObject.GetComponent<UILabel>();
        Spt_Activity_Star_0 = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_0").gameObject.GetComponent<UISprite>();
        Spt_Activity_Star_1 = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_1").gameObject.GetComponent<UISprite>();
        Spt_Activity_Star_2 = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_Star/Activity_Star_2").gameObject.GetComponent<UISprite>();
        Spt_Activity_ItemBG = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ItemBG").gameObject.GetComponent<UISprite>();
        Lbl_Endless_EXP = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_EXP/Endless_EXP").gameObject.GetComponent<UILabel>();
        Lbl_Endless_Gold = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Gold/Endless_Gold").gameObject.GetComponent<UILabel>();
        Spt_Endless_ItemBG = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_ItemBG").gameObject.GetComponent<UISprite>();
        Spt_Endless_ItemQuality = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_ItemBG/Endless_ItemQuality").gameObject.GetComponent<UISprite>();
        Lbl_Endless_Num = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_Num/Endless_Num").gameObject.GetComponent<UILabel>();
        Lbl_Endless_CurScore = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_CurScore/Endless_CurScore").gameObject.GetComponent<UILabel>();
        Lbl_Endless_MaxScore = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_MaxScore/Endless_MaxScore").gameObject.GetComponent<UILabel>();
        Spt_Expedition_ItemBG = _uiRoot.transform.FindChild("FightResult/Result_Expedition/Expedition_ItemBG").gameObject.GetComponent<UISprite>();
        Spt_Expedition_ItemQuality = _uiRoot.transform.FindChild("FightResult/Result_Expedition/Expedition_ItemBG/Expedition_ItemQuality").gameObject.GetComponent<UISprite>();
        Lbl_Expedition_Num = _uiRoot.transform.FindChild("FightResult/Result_Expedition/Expedition_ItemBG/Expedition_Num").gameObject.GetComponent<UILabel>();
        Lbl_PVP_Hint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Hint").gameObject.GetComponent<UILabel>();
        Lbl_PVP_Pre = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Pre").gameObject.GetComponent<UILabel>();
        Lbl_PVP_Cur = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_Cur").gameObject.GetComponent<UILabel>();
        Spt_PVP_UpIcon = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_UpIcon").gameObject.GetComponent<UISprite>();
        Spt_PVP_BG_UpHint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_BG_UpHint").gameObject.GetComponent<UISprite>();
        Lbl_UpHint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Current/Cur_BG_UpHint/UpHint").gameObject.GetComponent<UILabel>();

        Lbl_PVP_His_Hint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Hint").gameObject.GetComponent<UILabel>();
        Spt_PVP_His_UpIcon = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_UpIcon").gameObject.GetComponent<UISprite>();
        Lbl_PVP_His_Pre = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Pre").gameObject.GetComponent<UILabel>();
        Lbl_PVP_His_Cur = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_History/His_Cur").gameObject.GetComponent<UILabel>();

        Lbl_PVP_Rew_Hint = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_Hint").gameObject.GetComponent<UILabel>();
        Spt_PVP_Rew_UpIcon = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_UpIcon").gameObject.GetComponent<UISprite>();
        Lbl_PVP_Rew_Cur = _uiRoot.transform.FindChild("FightResult/Result_PVP/Rank_Reward/Rew_Cur").gameObject.GetComponent<UILabel>();


        Spt_Fail_Hint_Activity = _uiRoot.transform.FindChild("FightResult/Result_Fail/Fail_Hint_Activity").gameObject.GetComponent<UISprite>();

        Lbl_Slave_Hint = _uiRoot.transform.FindChild("FightResult/Result_Slave/Slave_Hint").gameObject.GetComponent<UILabel>();

        TP_Union_HurtTitle = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_HurtTitle").gameObject.GetComponent<TweenPosition>();
        Lbl_Union_HurtValue = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_HurtTitle/Union_HurtValue").gameObject.GetComponent<UILabel>();

        TP_Union_RewardTitle = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_RewardTitle").gameObject.GetComponent<TweenPosition>();
        Spt_Union_RewardIconGold = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_RewardTitle/Union_RewardIconGold").gameObject.GetComponent<UISprite>();
        Lbl_Union_RewardValueGold = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_RewardTitle/Union_RewardIconGold/Union_RewardValueGold").gameObject.GetComponent<UILabel>();
        Spt_Union_RewardIconToken = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_RewardTitle/Union_RewardIconToken").gameObject.GetComponent<UISprite>();
        Lbl_Union_RewardValueToken = _uiRoot.transform.FindChild("FightResult/Result_Union/Union_RewardTitle/Union_RewardIconToken/Union_RewardValueToken").gameObject.GetComponent<UILabel>();
        
        UIPanel_FightStart = _uiRoot.transform.FindChild("FightStart").gameObject.GetComponent<UIPanel>();
        Spt_Start_Content = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_Content").gameObject.GetComponent<UISprite>();
        Lbl_Start_Endless_Count = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_Content/Start_Endless_Count").gameObject.GetComponent<UILabel>();
        Lbl_Start_Endless_Hint = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_Content/Start_Endless_Hint").gameObject.GetComponent<UILabel>();
        TP_Start_Info = _uiRoot.transform.FindChild("FightStart/Stage_Info").gameObject.GetComponent<TweenPosition>();
        TA_Start_SuccessIcon = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_SuccessIcon").gameObject.GetComponent<TweenAlpha>();
        TS_Start_SuccessIcon = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_SuccessIcon").gameObject.GetComponent<TweenScale>();
        TR_Start_SuccessIcon = _uiRoot.transform.FindChild("FightStart/Stage_Info/Start_SuccessIcon").gameObject.GetComponent<TweenRotation>();

        UIWidget_Main_StageInfo = _uiRoot.transform.FindChild("FightMain/Main_StageInfo").gameObject.GetComponent<UIWidget>();
        Spt_StageInfo_VS = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_VS").gameObject.GetComponent<UISprite>();
        Spt_Self_Icon = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Self/Self_Icon").gameObject.GetComponent<UISprite>();
        Lbl_Self_Info = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Self/Self_Info").gameObject.GetComponent<UILabel>();
        Sli_Self_Progress = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Self/Self_Progress").gameObject.GetComponent<UISlider>();
        Spt_Enemy_Icon = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Enemy/Enemy_Icon").gameObject.GetComponent<UISprite>();
        Lbl_Enemy_Info = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Enemy/Enemy_Info").gameObject.GetComponent<UILabel>();
        Sli_Enemy_Progress = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Enemy/Enemy_Progress").gameObject.GetComponent<UISlider>();

        Lbl_Fail_Hint_Info = _uiRoot.transform.FindChild("FightResult/Fail_Hint_Info").gameObject.GetComponent<UILabel>();


        UIWidget_Main_PVPInfo = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo").gameObject.GetComponent<UIWidget>();
        Lbl_PVPInfo_Text = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Text").gameObject.GetComponent<UILabel>();
        Sli_SelfPVP_ProgressUp = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Self/SelfPVP_ProgressUp").gameObject.GetComponent<UISlider>();
        Sli_SelfPVP_ProgressDown = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Self/SelfPVP_ProgressDown").gameObject.GetComponent<UISlider>();
        Lbl_SelfPVP_Info = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Self/SelfPVP_Info").gameObject.GetComponent<UILabel>();
        Sli_EnemyPVP_ProgressUp = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Enemy/EnemyPVP_ProgressUp").gameObject.GetComponent<UISlider>();
        Sli_EnemyPVP_ProgressDown = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Enemy/EnemyPVP_ProgressDown").gameObject.GetComponent<UISlider>();
        Lbl_EnemyPVP_Info = _uiRoot.transform.FindChild("FightMain/Main_PVPInfo/PVPInfo_Enemy/EnemyPVP_Info").gameObject.GetComponent<UILabel>();

        TP_Main_BossHint = _uiRoot.transform.FindChild("FightMain/Main_BossHint").gameObject.GetComponent<TweenPosition>();


        UIPanel_Activity_ItemList = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ItemList").gameObject.GetComponent<UIPanel>();
        Grd_Activity_ItemGrid = _uiRoot.transform.FindChild("FightResult/Result_Activity/Activity_ItemList/Activity_ItemGrid").gameObject.GetComponent<UIGrid>();
        UIPanel_Endless_ItemList = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_ItemList").gameObject.GetComponent<UIPanel>();
        Grd_Endless_ItemGrid = _uiRoot.transform.FindChild("FightResult/Result_Endless/Endless_ItemList/Endless_ItemGrid").gameObject.GetComponent<UIGrid>();

        TP_Cur_Integral = _uiRoot.transform.FindChild("FightResult/Result_CaptureTerritory/Cur_Integral").gameObject.GetComponent<TweenPosition>();
        Lbl_Cur_Integral = _uiRoot.transform.FindChild("FightResult/Result_CaptureTerritory/Cur_Integral").gameObject.GetComponent<UILabel>();

        TP_CSW_IntegralTitle = _uiRoot.transform.FindChild("FightResult/Result_CrossServerWar/CSW_IntegralTitle").gameObject.GetComponent<TweenPosition>();
        Lbl_CSW_IntegralValue = _uiRoot.transform.FindChild("FightResult/Result_CrossServerWar/CSW_IntegralTitle/CSW_IntegralValue").gameObject.GetComponent<UILabel>();
        TP_CSW_AwardTitle = _uiRoot.transform.FindChild("FightResult/Result_CrossServerWar/CSW_AwardTitle").gameObject.GetComponent<TweenPosition>();
        Lbl_CSW_AwardValue = _uiRoot.transform.FindChild("FightResult/Result_CrossServerWar/CSW_AwardTitle/CSW_AwardValue").gameObject.GetComponent<UILabel>();

        Lbl_ST_Title = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_Title").gameObject.GetComponent<UILabel>();
        TP_ST_Title = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_Title").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_FightHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_FightHurt").gameObject.GetComponent<UILabel>();
        TP_ST_FightHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_FightHurt").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_FightHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_FightHurtValue").gameObject.GetComponent<UILabel>();
        TP_ST_FightHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_FightHurtValue").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_BUFFHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_BUFFHurt").gameObject.GetComponent<UILabel>();
        TP_ST_BUFFHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_BUFFHurt").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_BUFFHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_BUFFHurtValue").gameObject.GetComponent<UILabel>();
        TP_ST_BUFFHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_BUFFHurtValue").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_TotleHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_TotleHurt").gameObject.GetComponent<UILabel>();
        TP_ST_TotleHurt = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_TotleHurt").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_TotleHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_TotleHurtValue").gameObject.GetComponent<UILabel>();
        TP_ST_TotleHurtValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_TotleHurtValue").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_Award = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_Award").gameObject.GetComponent<UILabel>();
        TP_ST_Award = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_Award").gameObject.GetComponent<TweenPosition>();
        Lbl_ST_AwardValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_AwardValue").gameObject.GetComponent<UILabel>();
        TP_ST_AwardValue = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony/ST_AwardValue").gameObject.GetComponent<TweenPosition>();


        Lbl_Qualifying_Title = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Title").gameObject.GetComponent<UILabel>();
        TP_Qualifying_Title = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Title").gameObject.GetComponent<TweenPosition>();
        Lbl_Qualifying_Pre = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Pre").gameObject.GetComponent<UILabel>();
        TS_Qualifying_Pre = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Pre").gameObject.GetComponent<TweenScale>();
        Lbl_Qualifying_Cur = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Cur").gameObject.GetComponent<UILabel>();
        TS_Qualifying_Cur = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_Cur").gameObject.GetComponent<TweenScale>();
        TS_Qualifying_UpIcon = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_UpIcon").gameObject.GetComponent<TweenScale>();
        TS_Qualifying_BG_UpHint = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_BG_UpHint").gameObject.GetComponent<TweenScale>();
        Lbl_Qualifying_UpHint = _uiRoot.transform.FindChild("FightResult/Result_Qualifying/Qualifying_Content/Qualifying_BG_UpHint/Qualifying_UpHint").gameObject.GetComponent<UILabel>();


        //自定义组件--------------------------------------------------------------//
        Obj_Skill_Item = _uiRoot.transform.FindChild("FightMain/Skill_Item").gameObject.GetComponent<Transform>();
        Obj_Soldier_Item = _uiRoot.transform.FindChild("FightMain/Soldier_Item").gameObject.GetComponent<Transform>();
        Obj_PlayerInfo_Fight = _uiRoot.transform.FindChild("FightMain/Main_PlayerInfo/PlayerInfo_Fight").gameObject.GetComponent<Transform>();

        Obj_Result_Activity = _uiRoot.transform.FindChild("FightResult/Result_Activity").gameObject.GetComponent<Transform>();
        Obj_Result_Endless = _uiRoot.transform.FindChild("FightResult/Result_Endless").gameObject.GetComponent<Transform>();
        Obj_Result_Expedition = _uiRoot.transform.FindChild("FightResult/Result_Expedition").gameObject.GetComponent<Transform>();
        Obj_Result_PVP = _uiRoot.transform.FindChild("FightResult/Result_PVP").gameObject.GetComponent<Transform>();
        Obj_Result_Slave = _uiRoot.transform.FindChild("FightResult/Result_Slave").gameObject.GetComponent<Transform>();
        Obj_Result_Union = _uiRoot.transform.FindChild("FightResult/Result_Union").gameObject.GetComponent<Transform>();
        Obj_Result_Fail = _uiRoot.transform.FindChild("FightResult/Result_Fail").gameObject.GetComponent<Transform>();
        Obj_Result_CaptureTerritory = _uiRoot.transform.FindChild("FightResult/Result_CaptureTerritory");
        Obj_Result_CrossServerWar = _uiRoot.transform.FindChild("FightResult/Result_CrossServerWar");
        Obj_Result_ServerHegemony = _uiRoot.transform.FindChild("FightResult/Result_ServerHegemony");
        Obj_Result_Qualifying = _uiRoot.transform.FindChild("FightResult/Result_Qualifying");
        Obj_Result_BG_Down = _uiRoot.transform.FindChild("FightResult/Result_BG_Down");

        Obj_PVE = _uiRoot.transform.FindChild("FightMain/Main_PVE");
        Obj_PVP = _uiRoot.transform.FindChild("FightMain/Main_PVP");
        Obj_PVE_Skill = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Skill");
        Obj_PVE_Soldier = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Soldier");
        Obj_PVP_Self_Skill = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_Skill");
        Obj_PVP_Self_Soldier = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Self/Self_Soldier");
        Obj_PVP_Enemy_Skill = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_Skill");
        Obj_PVP_Enemy_Soldier = _uiRoot.transform.FindChild("FightMain/Main_PVP/PVP_Enemy/Enemy_Soldier");

        Obj_StageInfo_Self = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Self");
        Obj_StageInfo_Enemy = _uiRoot.transform.FindChild("FightMain/Main_StageInfo/StageInfo_Enemy");
        Obj_Main_Progress = _uiRoot.transform.FindChild("FightMain/Main_Progress/ProgressContent");

        Obj_Energy_Effect = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Energy/Energy_Effect");
        Obj_Magic_Effect = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Magic/Magic_Effect");
        Obj_Magic_FireEffect = _uiRoot.transform.FindChild("FightMain/Main_PVE/PVE_Magic/Magic_FireEffect");
        //自定义组件--------------------------------------------------------------//
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Time_Content.text = "00:16";
        Lbl_Fight_Score.text = "";
        Lbl_Energy_Title.text = "0/0";
        Lbl_Magic_Title.text = "0/0";
        Lbl_SMagic_Title.text = "0/0";
        Lbl_EMagic_Title.text = "0/0";
        Lbl_Activity_Hint.text = "";
        Lbl_Activity_Gold.text = "";
        Lbl_Activity_EXP.text = "";
        Lbl_Endless_EXP.text = "";
        Lbl_Endless_Gold.text = "";
        Lbl_Endless_Num.text = "";
        Lbl_Endless_CurScore.text = "";
        Lbl_Endless_MaxScore.text = "";
        Lbl_Expedition_Num.text = "";
        Lbl_PVP_Pre.text = "";
        Lbl_PVP_Cur.text = "";
        Lbl_UpHint.text = "";
        Lbl_Slave_Hint.text = "";
        Lbl_Start_Endless_Count.text = "";
        Lbl_Start_Endless_Hint.text = "";
        Lbl_Self_Info.text = "";
        Lbl_Enemy_Info.text = "";
        Lbl_Fail_Hint_Info.text = "";
        Lbl_PVPInfo_Text.text = "";
        Lbl_SelfPVP_Info.text = "";
        Lbl_EnemyPVP_Info.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}