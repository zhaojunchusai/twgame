using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using TdSpine;
using Assets.Script.Common;

public class ConfigManager : Singleton<ConfigManager>
{
    /// <summary>
    /// 英雄数据
    /// </summary>
    public HeroAttributeConfig mHeroData;
    /// <summary>
    /// 士兵数据
    /// </summary>
    public SoldierAttributeConfig mSoldierData;
    /// <summary>
    /// 武将甄选
    /// </summary>
    public SoldierStepConfig mSoldierStepData;
    /// <summary>
    /// 技能书返还
    /// </summary>
    public ReturnMaterialConfig mReturnMaterialData;
    /// <summary>
    /// 怪物表
    /// </summary>
    public MonsterAttributeConfig mMonsterData;
    /// <summary>
    /// 关卡数据
    /// </summary>
    public StageConfig mStageData;
    /// <summary>
    /// 全服霸主
    /// </summary>
    public ServerHegemonyConfig mServerHegemonyData;
    /// <summary>
    /// 关卡出怪数据
    /// </summary>
    public StageMonsterConfig mStageMonsterData;
    /// <summary>
    /// 武器数据
    /// </summary>
    public EquipAttributeConfig mEquipData;
    /// <summary>
    /// 神器解锁数据
    /// </summary>
    public GodEquipLockConfig mGodEquipLockData;
    /// <summary>
    /// 技能数据
    /// </summary>
    public SkillAttributeConfig mSkillAttData;
    /// <summary>
    /// 天将神兵系统数据
    /// </summary>
    public SacrificialSystemConfig mSacrificialData;
    /// <summary>
    /// 技能特效数据
    /// </summary>
    public SkillEffectConfig mSkillEffectData;
    /// <summary>
    /// 缘宝阁配置表数据
    /// </summary>
    public DrowEquipConfig mDrowEquipData;
    /// <summary>
    /// 关卡提示气泡配置
    /// </summary>
    public _GuideTipsConfig mGuideTipsConfig;
    /// <summary>
    /// 消耗材料包数据
    /// </summary>
    public MaterialsBagAttributeConfig mMaterialsBagData;
    /// <summary>
    /// 副本章节数据
    /// </summary>
    public ChapterConfig mChaptersData;
    /// <summary>
    /// 物品表
    /// </summary>
    public ItemConfig mItemData;
    /// <summary>
    /// 掉落包数据
    /// </summary>
    public DroppackConfig mDroppackData;
    /// <summary>
    /// 关卡角色积分表
    /// </summary>
    public ChapterGradeConfig mChapterGradeData;
    /// <summary>
    /// 特殊数据配置表
    /// </summary>
    public SpecialIDConfig mSpecialIDData;
    /// <summary>
    /// 好友特殊配置表
    /// </summary>
    public FriendSpecialConfig mFriendSpecialData;
    /// <summary>
    /// 招募系统配置表
    /// </summary>
    public RecruitConfig mRecruitData;

    /// <summary>
    /// 战斗力计算配置表
    /// </summary>
    public CombatPowerConfig mCombatPowerData;

    /// <summary>
    /// 商品表
    /// </summary>
    public StoreGoodsConfig mStoreGoodsConfig;
    public VipConfig mVipConfig;
    public RechargeConfig mRechargeConfig;
    public TimesExpendConfig mTimesExpendConfig;
    public TimesBuyConfig mTimesBuyConfig;
    public LivenessConfig mLivenessConfig;
    public ExchangeGoldConfig mExchangeGoldConfig;
    /// <summary>
    /// 远征配置表
    /// </summary>
    public ExpeditionConfig mExpeditionConfig;
    public ExpeditionChapterConfig mExpeditionChapterConfig;
    /// <summary>
    /// 演舞台配置表
    /// </summary>
    public ArenaConfig mArenaConfig;
    /// <summary>
    /// 演舞台奖励配置表
    /// </summary>
    public ArenaAwardConfig mArenaAwardConfig;
    public TaskConfig mTaskConfig;
    public LoginAwardConfig mLoginAwardConfig;
    /// <summary>
    /// 城堡配置表
    /// </summary>
    public CastleAttributeConfig mCastleConfig;
    /// <summary>
    /// 升级开启功能配置表
    /// </summary>
    public OpenLevelConfig mOpenLevelConfig;
    public AppConfig mAppConfig;
    public GuideConfig mGuideConfig;

    public DialogueConfig mDialogueConfig;
    public DialogueTriggerConfig mDialogueTriggerConfig;

    //public GameActivityConfig mGameActivityConfig;
    public GameActivityTypeConfig mGameAcitivityTypeConfig;

    public RandomAttributeConfig mRandomAttributeConfig;

    public BossTalkConfig mBossTalkConfig;

    public FrameConfig mFrameConfig;
    public PlayerPortraitConfig mPlayerPortraitConfig;
    public UnionConfig mUnionConfig;
    public SlaveIncomeConfig mSlaveIncomeConfig;
    public GameOnlinePackageConfig mGameOnlinePackageConfig;
    public TipsConfig mTipsConfig;
    public MallConfig mMallConfig;
    /// <summary>
    /// 新手任务
    /// </summary>
    public NoviceTaskConfig mNoviceTaskConfig;
    /// <summary>
    /// 新手任务子任务配置表
    /// </summary>
    public NoviceSubtasksConfig mNoviceSubtasksConfig;
    /// <summary>
    /// 攻略配置表
    /// </summary>
    public WalkthroughConfig mWalkthroughConfig;

    public MainCityUnlockConfig mMainCityUnlockConfig;
    public FriendMissionConfig mFriendMissionConfig;
    public ErrorCodeConfig mErrorCodeConfig;
    public CaptureTerritoryConfig mCaptureTerritoryConfig;

    /// <summary>
    /// 排位赛奖励配置表
    /// </summary>
    public QualifyingAwardConfig mQualifyingAwardConfig;
    /// <summary>
    /// 排位赛段位配置表
    /// </summary>
    public QualifyingRankConfig mQualifyingRankConfig;
    /// <summary>
    /// 规则描述配置表
    /// </summary>
    public RuleDescConfig mRuleDescConfig;
    public RankviewConfig mRankviewConfig;
    public LifeSoulBaseInfoConfig mLifeSoulBaseInfoConfig;
    public LifeSoulConfig mLifeSoulConfig;
    public RecycleViewConfig mRecycleViewConfig;
    /// <summary>
    /// 计数器
    /// </summary>
    public CounterTool _CounterTool;
    /// <summary>
    /// 成就系统配置表
    /// </summary>
    public AchievementConfig mAchievementConfig;

    public EquipCoordinatesConfig mEquipCoordinatesConfig;
    /// <summary>
    /// 跨服战场配置表
    /// </summary>
    public CrossServerWarConfig mCrossServerWarConfig;
    public CombatPetsConfig mCombatPetsConfig;
    public CombatPetsUpdateConfig mCombatPetsUpdateConfig;


    private System.Action LoadCompleted;

    public void Initialize()
    {
        mTipsConfig = new TipsConfig();
    }

    public IEnumerator LoadConfig(System.Action callback)
    {
        _CounterTool = new CounterTool();
        LoadCompleted = callback;
        //mAppConfig = new AppConfig();  //暂时注释  modify by taiwei
        mHeroData = new HeroAttributeConfig();
        mEquipData = new EquipAttributeConfig();
        yield return 0;
        mGodEquipLockData = new GodEquipLockConfig();
        mMaterialsBagData = new MaterialsBagAttributeConfig();
        yield return 0;
        mSoldierData = new SoldierAttributeConfig();
        mSoldierStepData = new SoldierStepConfig();
        yield return 0;
        mReturnMaterialData = new ReturnMaterialConfig();
        mSkillAttData = new SkillAttributeConfig();
        mSacrificialData = new SacrificialSystemConfig();
        yield return 0;
        mSkillEffectData = new SkillEffectConfig();
        mDrowEquipData = new DrowEquipConfig();
        mGuideTipsConfig = new _GuideTipsConfig();
        mMonsterData = new MonsterAttributeConfig();
        yield return 0;
        mStageData = new StageConfig();
        mServerHegemonyData = new ServerHegemonyConfig();
        mStageMonsterData = new StageMonsterConfig();
        yield return 0;
        mChaptersData = new ChapterConfig();
        mItemData = new ItemConfig();
        yield return 0;
        mDroppackData = new DroppackConfig();
        mChapterGradeData = new ChapterGradeConfig();
        yield return 0;
        mSpecialIDData = new SpecialIDConfig();
        mFriendSpecialData = new FriendSpecialConfig();
        mRecruitData = new RecruitConfig();
        yield return 0;
        mCombatPowerData = new CombatPowerConfig();
        mStoreGoodsConfig = new StoreGoodsConfig();
        yield return 0;
        mVipConfig = new VipConfig();
        mTimesExpendConfig = new TimesExpendConfig();
        mTimesBuyConfig = new TimesBuyConfig();
        yield return 0;
        mExpeditionConfig = new ExpeditionConfig();
        mExpeditionChapterConfig = new ExpeditionChapterConfig();
        yield return 0;
        mTaskConfig = new TaskConfig();
        mRechargeConfig = new RechargeConfig();
        yield return 0;
        mLoginAwardConfig = new LoginAwardConfig();
        mArenaAwardConfig = new ArenaAwardConfig();
        yield return 0;
        mArenaConfig = new ArenaConfig();
        mOpenLevelConfig = new OpenLevelConfig();
        yield return 0;
        mLivenessConfig = new LivenessConfig();
        mCastleConfig = new CastleAttributeConfig();
        yield return 0;
        mExchangeGoldConfig = new ExchangeGoldConfig();
        mGuideConfig = new GuideConfig();
        yield return 0;
        mDialogueConfig = new DialogueConfig();
        mDialogueTriggerConfig = new DialogueTriggerConfig();
        yield return 0;
        //mGameActivityConfig = new GameActivityConfig();
        mRandomAttributeConfig = new RandomAttributeConfig();
        yield return 0;
        mBossTalkConfig = new BossTalkConfig();
        mFrameConfig = new FrameConfig();
        mErrorCodeConfig = new ErrorCodeConfig();
        yield return 0;
        mPlayerPortraitConfig = new PlayerPortraitConfig();
        mUnionConfig = new UnionConfig();
        mSlaveIncomeConfig = new SlaveIncomeConfig();
        yield return 0;
        mGameOnlinePackageConfig = new GameOnlinePackageConfig();
        mMainCityUnlockConfig = new MainCityUnlockConfig();
        mFriendMissionConfig = new FriendMissionConfig();
        yield return 0;
        mNoviceTaskConfig = new NoviceTaskConfig();
        mNoviceSubtasksConfig = new NoviceSubtasksConfig();
        yield return 0;
        mWalkthroughConfig = new WalkthroughConfig();
        yield return 0;
        mMallConfig = new MallConfig();
        yield return 0;
        mCaptureTerritoryConfig = new CaptureTerritoryConfig();
        yield return 0;
        mQualifyingAwardConfig = new QualifyingAwardConfig();
        mQualifyingRankConfig = new QualifyingRankConfig();
        yield return 0;
        mRuleDescConfig = new RuleDescConfig();
        mRankviewConfig = new RankviewConfig();
        yield return 0;
        mLifeSoulBaseInfoConfig = new LifeSoulBaseInfoConfig();
        mLifeSoulConfig = new LifeSoulConfig();
        yield return 0;
        mAchievementConfig = new AchievementConfig();
        mGameAcitivityTypeConfig = new GameActivityTypeConfig();
        yield return 0;
        mRecycleViewConfig = new RecycleViewConfig();
        mEquipCoordinatesConfig = new EquipCoordinatesConfig();
        yield return 0;
        mCombatPetsConfig = new CombatPetsConfig();
        mCombatPetsUpdateConfig = new CombatPetsUpdateConfig();
        yield return 0;
        mCrossServerWarConfig = new CrossServerWarConfig();

		Scheduler.Instance.AddUpdator(Update);
    }

    public void Update()
    {
        if ((_CounterTool != null) && (_CounterTool.Get_Count <= 0))
        {
            Scheduler.Instance.RemoveUpdator(Update);
            ResourceLoadManager.Instance.LoadedCount++;
            LoadCompleted();
            System.GC.Collect();
        }
    }

    public void Uninitialize()
    {
        mHeroData = null;
        mSoldierData = null;
        mSoldierStepData = null;
        mReturnMaterialData = null;
        mMonsterData = null;
        mStageData = null;
        mServerHegemonyData = null;
        mStageMonsterData = null;
        mEquipData = null;
        mGodEquipLockData = null;
        mSkillAttData = null;
        mSacrificialData = null;
        mSkillEffectData = null;
        mDrowEquipData = null;
        mGuideTipsConfig = null;
        mMaterialsBagData = null;
        mChaptersData = null;
        mItemData = null;
        mDroppackData = null;
        mChapterGradeData = null;
        mSpecialIDData = null;
        mFriendSpecialData = null;
        mRecruitData = null;
        mCombatPowerData = null;
        mStoreGoodsConfig = null;
        mVipConfig = null;
        mRechargeConfig = null;
        mTimesExpendConfig = null;
        mTimesBuyConfig = null;
        mLivenessConfig = null;
        mExchangeGoldConfig = null;
        mExpeditionConfig = null;
        mExpeditionChapterConfig = null;
        mArenaConfig = null;
        mArenaAwardConfig = null;
        mTaskConfig = null;
        mLoginAwardConfig = null;
        mCastleConfig = null;
        mOpenLevelConfig = null;
        mAppConfig = null;
        mGuideConfig = null;
        mDialogueConfig = null;
        mDialogueTriggerConfig = null;
        mGameAcitivityTypeConfig = null;
        mRandomAttributeConfig = null;
        mBossTalkConfig = null;
        mFrameConfig = null;
        mPlayerPortraitConfig = null;
        mUnionConfig = null;
        mSlaveIncomeConfig = null;
        mGameOnlinePackageConfig = null;
        mMallConfig = null;
        mNoviceTaskConfig = null;
        mNoviceSubtasksConfig = null;
        mWalkthroughConfig = null;
        mMainCityUnlockConfig = null;
        mFriendMissionConfig = null;
        mErrorCodeConfig = null;
        mCaptureTerritoryConfig = null;
        mQualifyingAwardConfig = null;

        mQualifyingRankConfig = null;
        mRuleDescConfig = null;
        mRankviewConfig = null;
        mLifeSoulBaseInfoConfig = null;
        mLifeSoulConfig = null;
        mRecycleViewConfig = null;
        mAchievementConfig = null;
        mEquipCoordinatesConfig = null;
        _CounterTool = null;
		mCrossServerWarConfig = null;
    }
}
