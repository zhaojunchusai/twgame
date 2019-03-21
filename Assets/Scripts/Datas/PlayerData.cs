using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Attribute = fogs.proto.msg.Attribute;

/// <summary>
/// 单个射手信息
/// </summary>
public class SingleShooterInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public uint mID;
    /// <summary>
    /// 等级
    /// </summary>
    public int mLevel;
    /// <summary>
    /// 状态[0-未解锁 1-已解锁]
    /// </summary>
    public int mStatus;
    /// <summary>
    /// 技能列表
    /// </summary>
    public List<CalBaseData> mSkillList;
    /// <summary>
    /// 属性
    /// </summary>
    public ShowInfoBase mAttribute;
    /// <summary>
    /// 攻击成长
    /// </summary>
    public int mAtt_Grow;

    public SingleShooterInfo()
    {
        InitInfo();
    }

    public void InitInfo()
    {
        mID = 0;
        mLevel = 0;
        mStatus = 0;
        if (mSkillList == null)
            mSkillList = new List<CalBaseData>();
        else
            mSkillList.Clear();
        if (mAttribute == null)
            mAttribute = new ShowInfoBase();
        else
            mAttribute.ClearInfo();
        mAtt_Grow = 0;
    }

    public void ResetInfo(fogs.proto.msg.Shooter vShooter)
    {
        InitInfo();
        if (vShooter == null)
            return;
        mID = vShooter.id;
        mLevel = vShooter.level;
        mStatus = vShooter.status;
        for (int i = 0; i < vShooter.skills.Count; i++)
        {
            mSkillList.Add(new CalBaseData(vShooter.skills[i].id, (int)vShooter.skills[i].level));
        }
        mAttribute.KeyData = mID;
        mAttribute.Attack = vShooter.attr.phy_atk;
        mAttribute.Crit = 0;
        mAttribute.Dodge = 0;
        mAttribute.Accuracy = 0;
        mAttribute.AttDistance = vShooter.attr.atk_space;
        mAttribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(vShooter.attr.atk_interval);
        mAttribute.Energy = 0;
        mAttribute.EnergyRecovery = 0;
        mAttribute.HP = 0;
        mAttribute.HPRecovery = 0;
        mAttribute.MoveSpeed = 0;
        mAttribute.MP = 0;
        mAttribute.MPRecovery = 0;
        mAttribute.Tenacity = 0;
        mAttribute.CombatPower = vShooter.attr.combat_power;
        mAtt_Grow = vShooter.attr.phy_atk_grow;
    }
}

/// <summary>
/// 城堡信息
/// </summary>
public class PlayerCastleInfo
{
    /// <summary>
    /// 城堡ID
    /// </summary>
    public uint mID;
    /// <summary>
    /// 城堡等级
    /// </summary>
    public int mLevel;
    /// <summary>
    /// 城堡技能
    /// </summary>
    public List<CalBaseData> mSkillList;
    /// <summary>
    /// 城堡属性
    /// </summary>
    public ShowInfoBase mAttribute;
    /// <summary>
    /// 生命成长
    /// </summary>
    public int mHP_Grow;

    public PlayerCastleInfo()
    {
        InitInfo();
    }

    public void InitInfo()
    {
        mID = 0;
        mLevel = 0;
        if (mSkillList == null)
            mSkillList = new List<CalBaseData>();
        else
            mSkillList.Clear();
        if (mAttribute == null)
            mAttribute = new ShowInfoBase();
        else
            mAttribute.ClearInfo();
        mHP_Grow = 0;
    }

    public void ResetInfo(fogs.proto.msg.Castle vCastle)
    {
        InitInfo();
        if (vCastle == null)
            return;
        mID = vCastle.id;
        mLevel = vCastle.level;
        for (int i = 0; i < vCastle.skills.Count; i++)
        {
            mSkillList.Add(new CalBaseData(vCastle.skills[i].id, (int)vCastle.skills[i].level));
        }
        mAttribute.KeyData = mID;
        mAttribute.Attack = 0;
        mAttribute.Crit = 0;
        mAttribute.Dodge = 0;
        mAttribute.Accuracy = 0;
        mAttribute.AttDistance = 0;
        mAttribute.AttRate = 0;
        mAttribute.Energy = 0;
        mAttribute.EnergyRecovery = 0;
        mAttribute.HP = vCastle.attr.hp_max;
        mAttribute.HPRecovery = 0;
        mAttribute.MoveSpeed = 0;
        mAttribute.MP = 0;
        mAttribute.MPRecovery = 0;
        mAttribute.Tenacity = 0;
        mAttribute.CombatPower = 0;
        mHP_Grow = vCastle.attr.hp_grow;
    }
}

public class PlayerData : Singleton<PlayerData>
{
    /// <summary>
    /// 账号ID
    /// </summary>
    public uint _AccountID;
    /// <summary>
    /// 角色ID
    /// </summary>
    public uint _RoleID;

    public uint _ServerID;
    /// <summary>
    /// 角色昵称
    /// </summary>
    public string _NickName;
    /// <summary>
    /// 性别[2-女 1-男]
    /// </summary>
    public byte _Gender;
    /// <summary>
    /// 角色等级
    /// </summary>
    public uint _Level { get; private set; }//等级[配置表查找ID]//
    /// <summary>
    /// VIP等级
    /// </summary>
    public byte _VipLv { get; private set; }

    /// <summary>
    /// VIP经验
    /// </summary>
    public int VipExp { get; private set; }

    /// <summary>
    /// 金币
    /// </summary>
    public int _Gold { get; private set; }
    /// <summary>
    /// 钻石
    /// </summary>
    public int _Diamonds { get; private set; }

    /// <summary>
    /// 体力
    /// </summary>
    public int _Physical { get; private set; }

    /// <summary>
    /// 排位賽積分
    /// </summary>
    public int QualifyingScore;

    public int MaxPhysical;
    /// <summary>
    /// 扫荡卡
    /// </summary>
    public uint _SweepCard;
    /// <summary>
    /// 当前经验值
    /// </summary>
    public uint _CurrentExp;
    /// <summary>
    /// 到达下一级需要的经验
    /// </summary>
    public int _NextLvExp;
    /// <summary>
    /// 会话ID
    /// </summary>
    public uint _SessionID;
    /// <summary>
    /// 账号名称
    /// </summary>
    public string _AccountName;
    /// <summary>
    /// 武将装备格子
    /// </summary>
    public int _SoldierEquipGrid;
    /// <summary>
    /// 购买士兵装备格子数次数
    /// </summary>
    public int _BuySoldierEquipGridNum;
    public List<fogs.proto.msg.DungeonStar> _GetAwardChapters;
    /// <summary>
    /// 竞技场积分
    /// </summary>
    public int _Medal { get; private set; }
    /// <summary>
    /// 远征积分
    /// </summary>
    public int _Honor { get; private set; }
    /// <summary>
    ///团令牌
    /// </summary>
    public int UnionToken
    {
        get { return CharUnionInfo.token; }
        private set { CharUnionInfo.token = value; }
    }
    /// <summary>
    /// 造化之源
    /// </summary>
    public int RecycleCoin;

    public int Sacrificial_Ora_Soldier_EXP = 0;
    public int Sacrificial_Soldier_EXP = 0;
    public int Sacrificial_Equip_EXP = 0;
    public int Sacrificial_Ora_Equip_EXP = 0;

    private bool mIsCreateRole = false;
    public bool IsCreateRole
    {
        get
        {
            return mIsCreateRole;
        }
    }

    /// <summary>
    /// 时间加速系数
    /// </summary>
    public float _TimeScale = 1;
    /// <summary>
    /// 请求体力更新时间
    /// </summary>
    public uint SpRevertTime = 0;
    /// <summary>
    /// 自身的邀请码
    /// </summary>
    public string InviteCode = "";
    /// <summary>
    /// 邀请码邀请的好友个数
    /// </summary>
    public int InvitCodeNum = 0;
    public uint refresh_tick = 0;

    public List<int> NoFirstPayList = new List<int>();

    public double LastBraveRecruitTime = 0;

    public double LastRiotRecruitTime = 0;

    public byte RecruitFreeCount = 0;   //招募次数

    public int HasMonthCard = 0; //没买是0 买了是1

    public ulong MonthCardDeadline; //时间戳

    public int BuySPTimes; // 体力购买次数//

    public string Sign = ""; // 服务端用//

    public uint TimeStamp = 0; // 服务端用//

    public string SDKURL = "";//7725用//

    public uint FeedbackRestCount = 0;//设置界面-反馈剩余次数

    public ulong FeedbackNextTime = 0;//设置界面-下一次可以反馈的时间戳

    public uint FrameID = 0;

    public uint HeadID = 0;
    public int _FreeRenameNum;

    public long DrowEquipFreeTime = 0;

    public int DrowEquipFreeCount = 0;

    /// <summary>
    /// 新手任务已完成列表
    /// </summary>
    public List<NewHandTask> _NoviceTaskCompleteList;

    /// <summary>
    /// 新手任务结束时间
    /// </summary>
    public ulong _NoviceTaskEndTime = 0;

    /// <summary>
    /// 金币兑换次数
    /// </summary>
    public int ExchangeGoldTimes = 0;

    public int BuyFundNum = 0;

    public List<uint> DoneNewbieGuideList = new List<uint>();

    public List<int> NewGameActivityIDList = new List<int>();

    //設置推送
    public int PHPowerSystemPush = 0;
    public int EnSlaveSystemPush = 0;
    public int RecruitSystemPush = 0;
    public int DrawEquipSystemPush = 0;
    public int ShopFreshSystemPush = 0;
    public int UnionClashSystemPush = 0;
    public int GCLDSystemPush = 0;

    /// <summary>
    /// 世界聊天冷却时间
    /// </summary>
    public long WorldChatCDTime = 0; //约定为下次发送可用时间
    /// <summary>
    /// 世界聊天可用次数
    /// </summary>
    public uint WorldChatCount = 0;

    public uint WorldSendTip = 0;
    /// <summary>
    /// 碎片
    /// </summary>
    public List<Item> _ChipBag;
    /// <summary>
    /// 材料包
    /// </summary>
    public List<Item> _MaterialBag;
    /// <summary>
    /// 消耗品
    /// </summary>
    public List<Item> _PropBag;
    /// <summary>
    /// 主线副本已通过关卡数据
    /// </summary>
    //public List<GateInfo> _GateInfoList;
    /// <summary>
    /// 主线副本已通过关卡数据
    /// </summary>
    public List<fogs.proto.msg.PassDungeon> _MajorDungeonInfoList;
    /// <summary>
    /// 今日关卡数
    /// </summary>
    public fogs.proto.msg.TodayPlayDungeons _TodayPlayDungeons;
    /// <summary>
    /// 活动副本数据
    /// </summary>
    public List<fogs.proto.msg.ActivityDungeonInfo> _ActivityDungeonList;
    /// <summary>
    /// 无尽模式数据
    /// </summary>
    public List<fogs.proto.msg.EndlessDungeonInfo> _EndlessDungeonList;

    /// <summary>
    /// 主线副本上阵阵容
    /// </summary>
    public List<ulong> _MajorDungeonSoldierList;

    public fogs.proto.msg.ArenaInfo _QualifyingInfo;

    public bool _QualifyingCDTip = false;

    /// <summary>
    /// 无尽模式章节数据
    ///最多只有三条数据  对应无尽模式三种难度 add by taiwei
    /// </summary>
    //public List<EndlessChaptperInfo> _EndlessChapterList;
    /// <summary>  
    /// 英雄属性
    /// </summary>
    public ShowInfoHero _Attribute;
    /// <summary>
    /// 玩家城堡信息
    /// </summary>
    public PlayerCastleInfo mCastleInfo;
    /// <summary>
    /// 射手信息列表
    /// </summary>
    public List<SingleShooterInfo> mShooterList;
    /// <summary>
    /// 神器装备栏
    /// </summary>
    public WeaponsDepot _WeaponDepot;
    /// <summary>
    /// 英雄身上已穿戴的装备栏
    /// </summary>
    public ArtifactedDepot _ArtifactedDepot;
    /// <summary>
    /// 英雄身上的技能
    /// </summary>
    public SkillsDepot _SkillsDepot;
    /// <summary>
    /// 拥有的武将装备
    /// </summary>
    public WeaponsDepot _SoldierEquip;
    /// <summary>
    /// 玩家拥有的武将
    /// </summary>
    public SoldierDepot _SoldierDepot;
    /// <summary>
    /// 玩家拥有的武将碎片
    /// </summary>
    public SoldierDebrisDepot _SoldierDebrisDepot;
    /// <summary>
    /// 玩家拥有的命魂（包含已装备的）
    /// </summary>
    public LifeSoulDepot _LifeSoulDepot;

    public fogs.proto.msg.LifeSoulInfo _PreyLifeSoulInfo;

    public FBFriendSys FbfriendDepot;
    /// <summary>
    /// 武将图鉴
    /// </summary>
    public SoldierMap _SoldierMap;
    /// <summary>
    /// 囚牢系统
    /// </summary>
    public Prison _Prison;
    public fogs.proto.msg.ArenaInfo _ArenaInfo;

    public PetDepot _PetDepot;
    public uint EquipPetID;
    public bool IsShowPet;

    /// <summary>
    /// 各个商店信息
    /// </summary>
    public Dictionary<ShopType, ShopInfo> ShopInfos;

    public LivenessInfo LivenessInfo;
    /// <summary>
    /// 开启界面-功能对应关系
    /// </summary>
    private Dictionary<ETaskOpenView, OpenFunctionType> dicOpenRelationShip;

    /// <summary>
    /// 远征天下数据
    /// </summary>
    public fogs.proto.msg.ExpeditionInfo _ExpeditionInfo;
    public SoldierDepot expeditionSoldierDopot;

    public LoginChangeInfo LoginChangeInfo;

    public CharUnionInfo CharUnionInfo;
    public int MyUnionID;
    public int MaxCombatPower = 0;//最强战斗力 
    public List<fogs.proto.msg.OtherList> BlockedPlayers;
    /// <summary>
    /// 第一次充值(0表示未充值，1表示充值过但未领取, 2表示领取过)
    /// </summary>
    public uint FirstPayGift = 0;

    public bool GotFirstLoginReward = true;// 登陆就送大礼包
    public VipRewardInfo VipRewardInfo;
    public List<int> BuildingUnlockList;
    public bool RemindToComment = false; //提醒去评价游戏

    public delegate void NotifyResetEventHandler(NotifyReset data);
    public event NotifyResetEventHandler NotifyResetEvent;

    public delegate void NotifyRefreshEventHandler(NotifyRefresh data);
    public event NotifyRefreshEventHandler NotifyRefreshEvent;

    public delegate void VoidUpdateEventHandler();
    public event VoidUpdateEventHandler UpdateMedalEvent;
    public event VoidUpdateEventHandler UpdateHonorEvent;
    public event VoidUpdateEventHandler UpdateUnionTokenEvent;
    public event VoidUpdateEventHandler UpdateRecycleCoinEvent;
    public event VoidUpdateEventHandler UpdateVipEvent;
    public event VoidUpdateEventHandler UpdatePlayerSPEvent;
    public event VoidUpdateEventHandler UpdatePlayerDiamondEvent;
    public event VoidUpdateEventHandler UpdatePlayerGoldEvent;
    public event VoidUpdateEventHandler UpdateLevelEvent;
    public event VoidUpdateEventHandler UpdateAttributeEvent;
    public event VoidUpdateEventHandler UpdateOnlineEvent;
    public event VoidUpdateEventHandler UpdateMaxCombatPowerEvent;
    public event VoidUpdateEventHandler UpdateVipRewardInfoEvent;
    /// <summary>
    /// 碎片、材料、消耗品荣誉等等数据更新时 通知事件
    /// </summary>
    public event VoidUpdateEventHandler UpdatePlayerItemsEvent;
    public delegate void LevelUpEventHandler(int old, int now);
    public event LevelUpEventHandler LevelUpEvent;

    public fogs.proto.msg.OnlineRewardInfo OnlineRewardInfo;

    public void Initialize()
    {
        if (_Attribute == null)
            _Attribute = new ShowInfoHero();
        if (mShooterList == null)
            mShooterList = new List<SingleShooterInfo>();
        if (mCastleInfo == null)
            mCastleInfo = new PlayerCastleInfo();
    }
    public void Uninitialize()
    {
        Clear();
    }
    public PlayerData()
    {
        _WeaponDepot = new WeaponsDepot();
        _SoldierEquip = new WeaponsDepot();

        _ArtifactedDepot = new ArtifactedDepot();

        _SkillsDepot = new SkillsDepot();
        _SoldierDepot = new SoldierDepot();
        _SoldierDebrisDepot = new SoldierDebrisDepot();
        FbfriendDepot = new FBFriendSys();
        _SoldierMap = new SoldierMap();
        _Prison = new Prison();
        //_GateInfoList = new List<GateInfo>();
        _MajorDungeonInfoList = new List<PassDungeon>();
        _GetAwardChapters = new List<DungeonStar>();
        OnlineRewardInfo = new OnlineRewardInfo();
        _LifeSoulDepot = new LifeSoulDepot();
        _PetDepot = new PetDepot();
    }
    /// <summary>
    /// 清空数据
    /// </summary>
    public void Clear()
    {
        _AccountID = 0;
        _RoleID = 0;
        _NickName = string.Empty;
        _Gender = 1;
        _Level = 0;
        _VipLv = 0;
        VipExp = 0;
        _Gold = 0;
        _Diamonds = 0;
        _Physical = 0;
        MaxPhysical = 0;
        _SweepCard = 0;
        _CurrentExp = 0;
        _NextLvExp = 0;
        _SessionID = 0;
        _AccountName = "";
        _Medal = 0;
        _Honor = 0;
        _TimeScale = 0;
        PHPowerSystemPush = 0;
        EnSlaveSystemPush = 0;
        RecruitSystemPush = 0;
        DrawEquipSystemPush = 0;
        ShopFreshSystemPush = 0;
        UnionClashSystemPush = 0;
        GCLDSystemPush = 0;
        SpRevertTime = 0;
        if (NoFirstPayList != null)
            NoFirstPayList.Clear();
        LastBraveRecruitTime = 0;
        LastRiotRecruitTime = 0;
        RecruitFreeCount = 0;
        HasMonthCard = 0;
        MonthCardDeadline = 0;
        BuySPTimes = 0;
        Sign = "";
        TimeStamp = 0;
        SDKURL = "";
        FeedbackRestCount = 0;
        FeedbackNextTime = 0;
        ExchangeGoldTimes = 0;
        if (_ChipBag != null)
            _ChipBag.Clear();
        if (_MaterialBag != null)
            _MaterialBag.Clear();
        if (_PropBag != null)
            _PropBag.Clear();
        if (_MajorDungeonInfoList != null)
            _MajorDungeonInfoList.Clear();
        _TodayPlayDungeons = null;

        if (_GetAwardChapters != null)
            _GetAwardChapters.Clear();
        if (_Attribute != null)
            _Attribute.ClearInfo();
        if (mCastleInfo != null)
            mCastleInfo.InitInfo();
        if (mShooterList != null)
            mShooterList.Clear();

        if (_WeaponDepot != null)
            _WeaponDepot.Clear();
        if (_ArtifactedDepot != null)
            _ArtifactedDepot.Clear();
        if (_SkillsDepot != null)
            _SkillsDepot.Clear();
        if (_SoldierEquip != null)
            _SoldierEquip.Clear();
        if (_SoldierDepot != null)
            _SoldierDepot.Clear();
        if (_SoldierDebrisDepot != null)
            _SoldierDebrisDepot.Clear();
        if (FbfriendDepot != null)
            FbfriendDepot.Clear();
        if (_SoldierMap != null)
            _SoldierMap.Clear();
        if (_Prison != null)
            _Prison.Clear();
        if (ShopInfos != null)
            ShopInfos.Clear();
        if (LivenessInfo != null)
            LivenessInfo = null;
        _PreyLifeSoulInfo = null;
        _ExpeditionInfo = null;
        expeditionSoldierDopot = null;
        LoginChangeInfo = null;
        OnlineRewardInfo = new OnlineRewardInfo();
        NewGameActivityIDList.Clear();
        _LifeSoulDepot.Clear();
        _PetDepot.Clear();
    }
    public void SetRoleData(CharInfo char_info, OtherInfo other_info, bool createRole)
    {
        mIsCreateRole = createRole;
        //this._AccountID = char_info.accid;
        this._RoleID = char_info.charid;
        this._ServerID = char_info.area_id;
        this._NickName = char_info.charname;
        this._Level = char_info.level;
        this._TimeScale = 1;
        RecycleCoin = char_info.b_info.recycle;
        SetMaxPhysical(this._Level);
        this._Gold = char_info.gold;
        _GetAwardChapters = char_info.b_info.dgn_star_rewards;
        //char_info.b_info.dgn_star_rewards;
        this._Diamonds = char_info.diamond;
        _Gender = (byte)char_info.gender;
        this._Physical = char_info.ph_power;
        this._CurrentExp = char_info.exp;
        //this._ChipBag = char_info.b_info.chip_bag;
        this._ChipBag = new List<Item>();
        for (int i = 0; i < char_info.b_info.chip_bag.Count; i++)
        {
            Item item = char_info.b_info.chip_bag[i];
            if (item == null) continue;
            ItemInfo iteminfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (iteminfo == null)      //配置表中找不到该项数据则跳过
                continue;
            if ((ItemTypeEnum)iteminfo.type == ItemTypeEnum.EquipChip)
                UpdateChipBag(item);
        }
        this._MaterialBag = char_info.b_info.material_bag;
        VipExp = char_info.b_info.vip_exp;
        this._PropBag = char_info.b_info.prop_bag;
        this._Medal = char_info.b_info.medal;
        this._Honor = char_info.b_info.honor;
        this.DrowEquipFreeCount = char_info.b_info.extract_count;

        this.DrowEquipFreeTime = (long)char_info.b_info.next_extract_tm;
        this.Sacrificial_Soldier_EXP = char_info.b_info.soldier_quality_value;
        this.Sacrificial_Ora_Soldier_EXP = char_info.b_info.soldier_ora_quality_value;
        this.Sacrificial_Equip_EXP = (int)char_info.b_info.equip_quality_value;
        this.Sacrificial_Ora_Equip_EXP = char_info.b_info.equip_org_quality_value;
        this.InviteCode = char_info.invite_code;
        this.refresh_tick = char_info.b_mutable_info.friends_info.refresh_tick;
        NoFirstPayList = char_info.b_info.first_recharge_list;
        _SoldierEquipGrid = char_info.b_info.equip_grid_num;
        _BuySoldierEquipGridNum = char_info.b_info.buy_equip_bag_num;
        _ArenaInfo = char_info.b_info.arena_info;
        /// this._Intergral = char_info.b_info.intergral;
        _MajorDungeonInfoList = char_info.b_info.major_dgn;  //获取途径依赖于剧情战役
        _TodayPlayDungeons = char_info.b_info.today_elite_dgn;
        _MajorDungeonSoldierList = char_info.b_info.load_soldiers;
        InitSpRevertTime(other_info.power_surplus_time);
        LivenessInfo = char_info.b_info.liveness_info;
        BuySPTimes = char_info.b_info.buy_phpower_num;
        this._SoldierEquip.Serialize(char_info.b_info.soldier_equip_bag);
        this._WeaponDepot.Serialize(char_info.b_info.equip_bag);
        if (this._ArtifactedDepot != null)
            _ArtifactedDepot.BindFather(_WeaponDepot);
        this._ArtifactedDepot.Serialize(char_info.b_info.equip_bag);
        ExchangeGoldTimes = char_info.b_info.touch_gold_degree;
        this._SoldierDepot.Serialize(char_info.b_info.soldier_bag);
        //this._SoldierDepot.SerializeAtt(other_info.soldier_attr);
        this._SkillsDepot.Serialize(char_info.b_info.skills);
        this._SoldierDebrisDepot.Serialize(char_info.b_info.chip_bag);
        this._SoldierMap.Serialize(char_info.b_info.soldier_id_list);
        this._Prison.Serialize(char_info.b_mutable_info.enslave_info);
        this._LifeSoulDepot.AddPlayerLifeSouls(char_info.b_info.soul_bag_on);
        this._LifeSoulDepot.AddPackLifeSouls(char_info.b_info.soul_bag);
        this._LifeSoulDepot.AddSoldierEquipedSouls(this._SoldierDepot._soldierList);
        _PreyLifeSoulInfo = char_info.b_info.soul_info;
        LoginChangeInfo = char_info.b_info.login_time_info;
        DoneNewbieGuideList = char_info.b_info.guide.step;
        MaxCombatPower = char_info.rank_combat_power;
        if (ShopInfos == null)
            ShopInfos = new Dictionary<ShopType, ShopInfo>();
        for (int i = 0; i < char_info.b_info.shop_list.Count; i++)
        {
            if (!ShopInfos.ContainsKey(char_info.b_info.shop_list[i].shop_type))
            {
                ShopInfos.Add(char_info.b_info.shop_list[i].shop_type, char_info.b_info.shop_list[i]);
            }
        }
        //_TodayPlayDungeons = char_info.b_info.today_elite_dgn;
        _FreeRenameNum = char_info.b_info.free_renamed_times;
        _VipLv = (byte)char_info.b_info.vip_lv;
        Main.Instance.InitTime((long)other_info.time);
        LastBraveRecruitTime = char_info.b_info.last_braverecruit_tm;
        LastRiotRecruitTime = char_info.b_info.last_riotrecruit_tm;
        RecruitFreeCount = (byte)char_info.b_info.recruit_count;
        HasMonthCard = char_info.b_info.has_month_card;
        MonthCardDeadline = char_info.b_info.month_card_deadline;
        UpdatePlayerAttribute(other_info.attr_info);
        FeedbackRestCount = (uint)char_info.b_info.setting.suggest_times;
        PHPowerSystemPush = char_info.b_info.setting.setting_switch.ph_power_switch;
        EnSlaveSystemPush = char_info.b_info.setting.setting_switch.enslave_switch;
        RecruitSystemPush = char_info.b_info.setting.setting_switch.recruit_switch;
        DrawEquipSystemPush = char_info.b_info.setting.setting_switch.draw_equip_switch;
        ShopFreshSystemPush = char_info.b_info.setting.setting_switch.shop_fresh_switch;
        UnionClashSystemPush = char_info.b_info.setting.setting_switch.union_clash_switch;
        GCLDSystemPush = char_info.b_info.setting.setting_switch.campaign_switch;
        FeedbackNextTime = char_info.b_info.setting.suggest_cool_to;

        FrameID = char_info.b_info.setting.hero_icon.frame.current_on;
        HeadID = char_info.b_info.setting.hero_icon.icon.current_on;

        TalkingDataManager.Instance.SetAccountName(_NickName);
        TalkingDataManager.Instance.SetAccountLevel((int)_Level);
        ChatModule.Instance.UpdateWorldChat(other_info.world_chat);
        WorldChatCDTime = (long)char_info.b_info.chat_next_time;
        WorldChatCount = char_info.b_info.chat_surplus_num;
        WorldSendTip = char_info.b_info.chat_by_diamond;

        ChatModule.Instance.UpdateCorpsChat(other_info.union_chat);
        ChatModule.Instance.UpdatePrivateChat(char_info.b_info.chat_infos);
        CharUnionInfo = char_info.b_mutable_info.union_info;
        BlockedPlayers = char_info.b_mutable_info.blocked_list;
        FirstPayGift = char_info.b_info.is_first_recharge;
        MyUnionID = char_info.union_id;

        OnlineRewardInfo = char_info.b_info.online_reward;
        MainCityModule.Instance.StartOnlinePackageCountDown();

        _NoviceTaskCompleteList = char_info.b_info.new_hand_tasks;
        _NoviceTaskEndTime = char_info.b_info.new_hand_over_tm;
        VipRewardInfo = char_info.b_info.vip_reward_info;
        //Debug.LogError(" sign " + OnlineRewardInfo.reward_sign + "need " + OnlineRewardInfo.need_online_time);
        BuildingUnlockList = char_info.b_info.build_unlock_list;
        RemindToComment = char_info.b_info.fsct_never_remind == 0;
        GotFirstLoginReward = char_info.b_info.has_got_first_login_award == 1;
        _QualifyingInfo = char_info.b_mutable_info.pole_info;
        _QualifyingCDTip = true;
        CaptureTerritoryModule.Instance.SetFightCDTimer(char_info.b_mutable_info.campaign_info.next_tick);
        SupermacyModule.Instance.GetOriginalSoldiersAndEquips(char_info.b_mutable_info.Overinfo.soldiers, char_info.b_mutable_info.Overinfo.equips);
        ReadyBattleSoldierManager.Instance.Init();
        EquipPetID = char_info.b_info.EquipPet;
        _PetDepot.Init(char_info.b_info.pet_bag);
        IsShowPet = (char_info.b_info.ShowMount == 1) ? true : false;
    }

    /// <summary>
    /// 取得玩家账号ID
    /// </summary>
    public uint GetAccountID()
    {
        return PlayerData.Instance._AccountID;
    }

    /// <summary>
    /// 取得玩家当前区服的角色ID
    /// </summary>
    public uint GetRoleID()
    {
        return PlayerData.Instance._RoleID;
    }

    public void UpdateBlockedPlayer(string accname, uint areaID)
    {
        if (BlockedPlayers == null)
            BlockedPlayers = new List<OtherList>();
        OtherList blockInfo = new OtherList();
        blockInfo.accname = accname;
        blockInfo.area_id = areaID;
        BlockedPlayers.Add(blockInfo);
    }

    public void SetOnlineDayUpdate(OnlineDayUpdateInfoResp resp)
    {
        LoginChangeInfo = resp.login_change_info;
        BuySPTimes = resp.buy_phpower_num;
        RecruitFreeCount = (byte)resp.recruit_num;
        ExchangeGoldTimes = resp.touch_gold_degree;
        LastBraveRecruitTime = resp.last_braverecruit_tm;
    }

    public void InitSpRevertTime(uint time)
    {
        SpRevertTime = time;
        Debug.Log("InitSpRevertTime " + time);
        if (SpRevertTime > 0)
            MainCityModule.Instance.StartRecoverSP((int)SpRevertTime);
    }

    private void SetMaxPhysical(uint lv)
    {
        HeroAttributeInfo info = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(lv);
        if (info != null)
        {
            MaxPhysical = info.MaxPhysical;
            _NextLvExp = info.EXP;
        }
    }

    public void UpdateMaxCombatPower(int power)
    {
        MaxCombatPower = power;
        if (UpdateMaxCombatPowerEvent != null)
            UpdateMaxCombatPowerEvent();
    }

    public void UpdateVipRewardInfo(VipRewardInfo info)
    {
        VipRewardInfo = info;
        if (UpdateVipRewardInfoEvent != null)
        {
            UpdateVipRewardInfoEvent();
        }
    }

    /// <summary>
    /// 取得扫荡卡个数
    /// </summary>
    /// <returns></returns>
    public int GetSweepCard()
    {
        int count = 0;
        fogs.proto.msg.Item item = _PropBag.Find((tmp) => { return tmp.id == GlobalCoefficient.SweepCardID; });
        if (item != null)
            count = item.num;
        return count;
    }

    /// <summary>
    /// 更新远征数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateExpeditionInfo(fogs.proto.msg.ExpeditionInfo data)
    {
        _ExpeditionInfo = null;
        _ExpeditionInfo = data;
        expeditionSoldierDopot = null;
        if (_ExpeditionInfo.enemies == null)
            return;
        if (_ExpeditionInfo.enemies.soldiers == null)
            return;
        expeditionSoldierDopot = new SoldierDepot();
        foreach (var tmpInfo in _ExpeditionInfo.enemies.soldiers)
            expeditionSoldierDopot.oneAdd(tmpInfo.soldier, false);
    }

    /// <summary>
    /// 更新单个远征关卡状态
    /// </summary>
    /// <param name="vGateID"></param>
    /// <param name="vGateStatus"></param>
    public void RefreshSingleExpeditionStatus(uint vGateID, ExpeditionInfo.GateStatus vGateStatus)
    {
        if (_ExpeditionInfo == null)
            return;
        if (_ExpeditionInfo.exp_gates == null)
            return;
        for (int i = 0; i < _ExpeditionInfo.exp_gates.Count; i++)
        {
            if (_ExpeditionInfo.exp_gates[i].id != vGateID)
                continue;
            _ExpeditionInfo.exp_gates[i].status = vGateStatus;
        }
    }

    /// <summary>
    /// 更新玩家数据
    /// </summary>
    /// <param name="attribute"></param>
    public void UpdatePlayerAttribute(Attribute attribute)
    {
        if (attribute == null)        //服务器在人物属性没变化时，不会发送数据过来 此时attribute为null   add by taiwei
        {
            return;
        }
        this._Attribute.KeyData = this._Level;
        this._Attribute.Attack = attribute.phy_atk;
        this._Attribute.Crit = attribute.crt_rate;
        this._Attribute.Dodge = attribute.ddg_rate;
        this._Attribute.Accuracy = attribute.acc_rate;
        this._Attribute.AttDistance = attribute.atk_space;
        this._Attribute.AttRate = CommonFunction.GetSecondTimeByMilliSecond(attribute.atk_interval);
        this._Attribute.Energy = attribute.energy_max;
        this._Attribute.EnergyRecovery = attribute.energy_revert;
        this._Attribute.HP = attribute.hp_max;
        this._Attribute.HPRecovery = attribute.hp_revert;
        this._Attribute.Leadership = (ushort)attribute.leader;
        this._Attribute.MoveSpeed = attribute.speed;
        this._Attribute.MP = attribute.mp_max;
        this._Attribute.MPRecovery = attribute.mp_revert;
        this._Attribute.Tenacity = attribute.tnc_rate;
        this._Attribute.CombatPower = attribute.combat_power;
        if (UpdateAttributeEvent != null)
            UpdateAttributeEvent();
    }

    public void AddPlayerAttribute(Attribute attribute)
    {
        this._Attribute.KeyData = 1;
        this._Attribute.Attack += attribute.phy_atk;
        this._Attribute.Crit += attribute.crt_rate;
        this._Attribute.Dodge += attribute.ddg_rate;
        this._Attribute.Accuracy += attribute.acc_rate;
        this._Attribute.AttDistance += attribute.atk_space;
        this._Attribute.AttRate += CommonFunction.GetSecondTimeByMilliSecond(attribute.atk_interval);
        this._Attribute.Energy += attribute.energy_max;
        this._Attribute.EnergyRecovery += attribute.energy_revert;
        this._Attribute.HP += attribute.hp_max;
        this._Attribute.HPRecovery += attribute.hp_revert;
        this._Attribute.Leadership += (ushort)attribute.leader;
        this._Attribute.MoveSpeed += attribute.speed;
        this._Attribute.MP += attribute.mp_max;
        this._Attribute.MPRecovery += attribute.mp_revert;
        this._Attribute.Tenacity += attribute.tnc_rate;
        this._Attribute.CombatPower = attribute.combat_power;
    }

    /// <summary>
    /// 更新城堡信息
    /// </summary>
    /// <param name="vCastleInfo"></param>
    public void UpdateCastleInfo(CastleInfo vCastleInfo)
    {
        if (vCastleInfo == null)
            return;

        if (vCastleInfo.castle != null)
        {
            if (mCastleInfo == null)
                mCastleInfo = new PlayerCastleInfo();
            else
                mCastleInfo.InitInfo();
            mCastleInfo.ResetInfo(vCastleInfo.castle);
        }

        if (vCastleInfo.shooters != null)
        {
            if (mShooterList == null)
                mShooterList = new List<SingleShooterInfo>();
            else
                mShooterList.Clear();

            for (int i = 0; i < vCastleInfo.shooters.Count; i++)
            {
                SingleShooterInfo tmpInfo = new SingleShooterInfo();
                tmpInfo.ResetInfo(vCastleInfo.shooters[i]);
                mShooterList.Add(tmpInfo);
            }
            mShooterList.Sort((left, right) =>
            {
                int leftNum = (int)left.mID % 10;
                int rightNum = (int)right.mID % 10;
                if (leftNum != rightNum)
                {
                    if (leftNum < rightNum)
                        return 1;
                    else
                        return -1;
                }
                return 0;
            });
        }
    }

    /// <summary>
    /// 批量添加武将
    /// </summary>
    /// <param name="soldier"></param>
    /// <returns></returns>
    public List<Soldier> MultipleAddSoldier(List<fogs.proto.msg.Soldier> soldier)
    {
        if (soldier == null || soldier.Count <= 0)
            return null;

        return this._SoldierDepot.multipleAdd(soldier);
    }
    /// <summary>
    /// 批量添加装备
    /// </summary>
    /// <param name="equipt"></param>
    /// <returns></returns>
    public List<Weapon> MultipleAddWeapon(List<fogs.proto.msg.Equip> equipt)
    {
        if (equipt == null || equipt.Count <= 0)
            return null;

        List<Weapon> wp = new List<Weapon>(equipt.Count + 1);
        foreach (fogs.proto.msg.Equip temp in equipt)
        {
            EquipAttributeInfo att = ConfigManager.Instance.mEquipData.FindById(temp.id);
            if (att == null)
                continue;
            if (att.godEquip == 1)
            {
                wp.Add(this._WeaponDepot.OneAdd(temp));
            }
            else
            {
                wp.Add(this._SoldierEquip.OneAdd(temp));
            }
        }
        return wp;
    }


    /// <summary>
    /// 批量添加技能
    /// </summary>
    /// <param name="skillList"></param>
    /// <returns></returns>
    public List<Skill> MultipleAddSkill(List<fogs.proto.msg.Skill> skillList)
    {
        List<Skill> list = new List<Skill>(skillList.Count + 1);
        foreach (fogs.proto.msg.Skill sk in skillList)
        {
            list.Add(this._SkillsDepot.oneAdd(sk));
        }
        return list;
    }
    /// <summary>
    /// 批量添加武魂
    /// </summary>
    /// <param name="itemList"></param>
    public void MultipleAddDebris(List<fogs.proto.msg.ItemInfo> itemList)
    {
        foreach (fogs.proto.msg.ItemInfo item in itemList)
        {
            this._SoldierDebrisDepot.oneAdd(item);
        }
    }
    public void OneAddDebris(fogs.proto.msg.ItemInfo item)
    {
        this._SoldierDebrisDepot.oneAdd(item);
    }
    public int GetEquipCount(uint id)
    {
        int count = 0;
        if (PlayerData.Instance._WeaponDepot != null)
            count += PlayerData.Instance._WeaponDepot.FindById(id).Count;
        if (PlayerData.Instance._SoldierEquip != null)
            count += PlayerData.Instance._SoldierEquip.FindById(id).Count;

        return count;
    }
    public int GetSoldierListCount()
    {
        if (this._SoldierDepot == null)
            return 0;
        return this._SoldierDepot.GetSoldierCount();
    }
    public bool IsSoldierFull(int count = 0)
    {
        if (this._SoldierDepot == null)
            return false;

        return this._SoldierDepot.IsFull(count);
    }
    /// <summary>
    /// 判断金币是否溢出
    /// </summary>
    public bool IsGoldOverflow(int count)
    {
        return CommonFunction.CheckMoneyOverflow(ECurrencyType.Gold, count);
    }

    /// <summary>
    /// 判断钻石是否溢出
    /// </summary>
    public bool IsDiamondOverflow(int count)
    {
        return CommonFunction.CheckMoneyOverflow(ECurrencyType.Diamond, count);
    }

    public bool IsEquipGridOverflow(int count)
    {
        int num = _SoldierEquip.getWeaponList((wp) =>
          {
              if (wp == null)
                  return false;
              return !wp.isEquiped;
          }).Count;
        return num + count > PlayerData.Instance._SoldierEquipGrid;
    }

    /// <summary>
    /// 判断某个ID的装备是否已装备
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public bool GetIsEquipById(uint id)
    {
        bool isEquip = false;
        if (PlayerData.Instance._WeaponDepot != null)
        {
            List<Weapon> tmp = PlayerData.Instance._WeaponDepot.FindById(id);
            if (tmp.Count > 0)
                isEquip = tmp[0].isEquiped;
        }

        if (PlayerData.Instance._SoldierEquip != null)
        {
            List<Weapon> tmp = PlayerData.Instance._SoldierEquip.FindById(id);
            if (tmp.Count > 0)
                isEquip = tmp[0].isEquiped;
        }
        return isEquip;
    }

    /// <summary>
    /// 获取神器某格是否已经装备
    /// </summary>
    /// <param name="index">0-7</param>
    /// <returns></returns>
    public bool GetIsEquipByIndex(int index = 0)
    {
        if (this._ArtifactedDepot == null)
            return false;
        return this._ArtifactedDepot._EquiptList[index] != null;
    }
    /// <summary>
    /// 刷新技能列表
    /// </summary>
    /// <param name="skillList"></param>
    public void SerializeHeroSkill(List<fogs.proto.msg.Skill> skillList)
    {
        this._SkillsDepot.RefreshList(skillList);
    }
    /// <summary>
    /// 随机技能
    /// </summary>
    /// <param name="role"> 技能发起者</param>
    /// <returns></returns>
    public bool RandomSkill(RoleAttribute role)
    {
        return this._SkillsDepot.RandomSkill(role);
    }
    /// <summary>
    /// 取得可用的主线副本关卡
    /// </summary>
    public List<StageData> GetAvailableGates()
    {
        List<StageData> stagedatalist = new List<StageData>();
        if (PlayerData.Instance._MajorDungeonInfoList == null)
        {
            PlayerData.Instance._MajorDungeonInfoList = new List<PassDungeon>();
        }
        if (PlayerData.Instance._MajorDungeonInfoList.Count == 0) //如果没有已经通过的关卡  则添加第一章节第一个普通关卡 
        {
            ChapterInfo _chapter = ConfigManager.Instance.mChaptersData.GetFirstChapterInfo();  //取得第一章节
            if (_chapter.LVLimit <= PlayerData.Instance._Level)
            {
                List<StageInfo> _stages = ConfigManager.Instance.mStageData.GetStagesByID(_chapter.gates, (byte)MainBattleType.Crusade);
                _stages = CommonFunction.SortStages(_stages);
                if (_stages != null && _stages.Count > 0)  //不排除数据处理错误的可能
                {
                    StageInfo _firstinfo = _stages[0];
                    if (_firstinfo.UnlockLV <= PlayerData.Instance._Level)
                    {
                        StageData _stagedata = new StageData();
                        _stagedata.gateinfo = new PassDungeon();
                        _stagedata.gateinfo.dgn_id = _firstinfo.ID;
                        _stagedata.gateinfo.star_level = 0;
                        _stagedata.stageinfo = _firstinfo;
                        _stagedata.remainRaidTimes = _firstinfo.ChallengeCount;
                        if (PlayerData.Instance._TodayPlayDungeons != null)
                        {
                            fogs.proto.msg.TodayPlayDungeons.DungeonTimes dungeonTime = PlayerData.Instance._TodayPlayDungeons.dungeons.Find((dungeon) => { return dungeon.id == _firstinfo.ID; });
                            if (dungeonTime != null)
                                _stagedata.remainRaidTimes = _firstinfo.ChallengeCount * (1 + dungeonTime.buy_times) - dungeonTime.times;
                        }
                        stagedatalist.Add(_stagedata);
                    }
                }
                else
                {
                    Debug.LogError("Deal Gate Data Error！");
                }
            }
        }
        else
        {
            stagedatalist.Clear();
            List<uint> passedGateIDList = new List<uint>();
            List<StageInfo> _passNormalStages = new List<StageInfo>();
            for (int i = 0; i < PlayerData.Instance._MajorDungeonInfoList.Count; i++)   //取得所有已通过关卡数据
            {
                PassDungeon _info = PlayerData.Instance._MajorDungeonInfoList[i];
                if (_info == null)
                    continue;
                StageInfo _stageInfo = ConfigManager.Instance.mStageData.GetInfoByID(_info.dgn_id);   //预防服务端与客户端配置表不一致导致的BUG
                if (_stageInfo == null)
                    continue;
                //if (_stageInfo.UnlockLV > PlayerData.Instance._Level)
                //{
                //    continue;
                //}
                passedGateIDList.Add(_info.dgn_id);
                _passNormalStages.Add(_stageInfo);
                StageData _stagedata = new StageData();
                _stagedata.gateinfo = new PassDungeon();
                _stagedata.gateinfo.dgn_id = _info.dgn_id;
                _stagedata.gateinfo.star_level = _info.star_level;
                _stagedata.stageinfo = new StageInfo();
                _stagedata.stageinfo.CopyTo(_stageInfo);
                if (PlayerData.Instance._TodayPlayDungeons != null)
                {
                    fogs.proto.msg.TodayPlayDungeons.DungeonTimes dungeonTime = PlayerData.Instance._TodayPlayDungeons.dungeons.Find((dungeon) => { return dungeon.id == _stageInfo.ID; });
                    if (dungeonTime != null)
                    {
                        _stagedata.remainRaidTimes = _stageInfo.ChallengeCount * (1 + dungeonTime.buy_times) - dungeonTime.times;
                    }
                    else
                    {
                        _stagedata.remainRaidTimes = _stageInfo.ChallengeCount;
                    }
                }
                stagedatalist.Add(_stagedata);
            }
            for (int i = 0; i < stagedatalist.Count; i++)
            {
                StageData stageData = stagedatalist[i];
                if (stageData == null || stageData.gateinfo == null || stageData.stageinfo == null)
                    continue;
                if (stageData.gateinfo.star_level > 0)  //如果星级评为0 则必然是未通过  不添加新数据
                {
                    List<StageInfo> nextStages = ConfigManager.Instance.mStageData.GetStageListByIDs(stageData.stageinfo.NextStage);
                    if (nextStages == null || nextStages.Count <= 0)
                        continue;
                    for (int index = 0; index < nextStages.Count; index++)  //取得后置关卡数据
                    {
                        StageInfo next_info = nextStages[index];
                        if (next_info == null)
                            continue;
                        if (passedGateIDList.Contains(next_info.ID))  //已经包含的数据 不在处理
                            continue;
                        bool isEnable = true;
                        //ChapterInfo chapter = ConfigManager.Instance.mChaptersData.GetChapterByGateID(next_info.ID);
                        //if (next_info.UnlockLV <= PlayerData.Instance._Level && chapter.LVLimit <= PlayerData.Instance._Level)
                        //{
                        for (int pre_index = 0; pre_index < next_info.PreStage.Count; pre_index++)  //判断后置关卡的前置条件时候都已经满足
                        {
                            PreStageInfo pre_info = next_info.PreStage[pre_index];
                            StageData passGateinfo = stagedatalist.Find((tmp) =>    //取已经通过的数据  如果没有则说明该关卡没有打过
                            {
                                if (tmp == null) return false;
                                return pre_info.StageID == tmp.gateinfo.dgn_id;
                            });
                            if (passGateinfo == null || passGateinfo.gateinfo.star_level < pre_info.StarNum) //如果关卡未通过或者低于关卡所需评星 则不添加新数据
                            {
                                isEnable = false;
                            }
                        }
                        //}
                        //else
                        //{
                        //    isEnable = false;
                        //}
                        if (isEnable)
                        {
                            StageData next_stageData = new StageData();
                            next_stageData.gateinfo = new PassDungeon();
                            next_stageData.gateinfo.dgn_id = next_info.ID;
                            next_stageData.gateinfo.star_level = 0;
                            next_stageData.stageinfo = new StageInfo();
                            next_stageData.stageinfo.CopyTo(next_info);
                            if (PlayerData.Instance._TodayPlayDungeons != null)
                            {
                                fogs.proto.msg.TodayPlayDungeons.DungeonTimes dungeonTime = PlayerData.Instance._TodayPlayDungeons.dungeons.Find((dungeon) => { return dungeon.id == next_info.ID; });
                                if (dungeonTime != null)
                                {
                                    next_stageData.remainRaidTimes = next_info.ChallengeCount * (1 + dungeonTime.buy_times) - dungeonTime.times;
                                }
                                else
                                {
                                    next_stageData.remainRaidTimes = next_info.ChallengeCount;
                                }
                            }
                            stagedatalist.Add(next_stageData);
                        }
                    }
                }
            }
        }
        return stagedatalist;
    }

    public int GetCurrencyNumByType(ECurrencyType type)
    {
        switch (type)
        {
            case ECurrencyType.Honor:
                {
                    return _Honor;
                }
            case ECurrencyType.Gold:
                {
                    return _Gold;
                }
            case ECurrencyType.Diamond:
                {
                    return _Diamonds;
                }
            case ECurrencyType.Medal:
                {
                    return _Medal;
                }
            case ECurrencyType.UnionToken:
                {
                    return UnionToken;
                }
            case ECurrencyType.RecycleCoin:
                {
                    return RecycleCoin;
                }
            default:
                return 0;
        }
        return 0;
    }


    /// <summary>
    /// 获取背包中拥有的物品数量（碎片，材料，消耗品） 
    /// 如果背包中无该物品则返回0
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public int GetItemCountByID(uint id)
    {
        Item item = _ChipBag.Find((tmp) => { return tmp.id == id; });
        if (item != null)
        {
            return item.num;
        }
        else
        {
            Debris tmp = this._SoldierDebrisDepot.FindByid(id);
            if (tmp != null)
                return tmp.count;
        }
        item = _MaterialBag.Find((tmp) => { return tmp.id == id; });
        if (item != null)
        {
            return item.num;
        }
        item = _PropBag.Find((tmp) => { return tmp.id == id; });
        if (item != null)
        {
            return item.num;
        }
        return 0;
    }
    /// <summary>
    /// 判定当前关卡是否通关  （true：已通关、false：未通过)
    /// </summary>
    /// <param name="gateID">关卡ID</param>
    public bool IsPassedGate(uint gateID)
    {
        if (_MajorDungeonInfoList == null)
        {
            _MajorDungeonInfoList = new List<PassDungeon>();
        }
        PassDungeon data = _MajorDungeonInfoList.Find((tmp) =>
          {
              if (tmp == null) return false;
              return tmp.dgn_id.Equals(gateID);
          });
        if (data == null) return false;
        return true;
    }
    /// <summary>
    /// 判定当前关卡是否可玩（true：已激活false：未激活）
    /// </summary>
    public bool IsAvailableGate(uint gateID)
    {
        List<StageData> list = GetAvailableGates();
        if (list == null || list.Count < 0)
        {
            return false;
        }
        StageData stagedata = list.Find((tmp) =>
        {
            if (tmp == null || tmp.stageinfo == null)
            {
                return false;
            }
            return tmp.stageinfo.ID == gateID;
        });
        if (stagedata == null)
        {
            return false;
        }
        return true;
    }
    /// <summary>
    /// 是否可用的战斗模式 true:该模式可用
    /// </summary>
    public bool IsAvailableModeByType(MainBattleType type)
    {
        List<StageData> list = GetAvailableGates();
        if (list == null || list.Count < 0)
        {
            return false;
        }
        StageData stagedata = list.Find((tmp) =>
        {
            if (tmp == null || tmp.stageinfo == null)
            {
                return false;
            }
            return tmp.stageinfo.IsElite == (int)type;
        });
        if (stagedata == null)
        {
            return false;
        }
        return true;
    }

    /// <summary>
    /// 主城显示 提示
    /// </summary>
    public string IsShowGateTip()
    {
        string message = "";
        List<GuideTips> tmpList = ConfigManager.Instance.mGuideTipsConfig.GetConfigList();
        GuideTips tmpInfo = tmpList[0];
        List<uint> UidCache = new List<uint>(tmpList.Count + 1);
        while (tmpInfo != null)
        {
            if (UidCache.Contains(tmpInfo.TipsID))
            {
                Debug.LogError("GuideTipsConfig is close loop");
                break;
            }
            UidCache.Add(tmpInfo.TipsID);
            if (!PlayerData.Instance.IsPassedGate(tmpInfo.CloseGate) && PlayerData.Instance.IsPassedGate(tmpInfo.OpenGate))
            {
                message = tmpInfo.Descript;
                break;
            }
            tmpInfo = ConfigManager.Instance.mGuideTipsConfig.FindById(tmpInfo.NextId);
        }
        return message;
    }

    /// <summary>
    /// 体力是否足够 进当前关卡
    /// </summary>
    /// <returns></returns>
    public bool IsEnoughSP(int Physical, bool showTip = false)
    {
        bool isEnough = (PlayerData.Instance._Physical >= Physical) ? true : false;
        if (!isEnough && showTip)
        {
            CommonFunction.OpenBuySp();
        }
        return isEnough;
    }

    /// <summary>
    /// 玩家货币数据更新
    /// </summary>
    /// <param name="moneyType">货币类型</param>
    /// <param name="moneyNum">货币具体数值</param>
    public void MoneyRefresh(int moneyType, int moneyNum)
    {
        switch (moneyType)
        {
            case 1:
                UpdateGold(moneyNum);
                break;
            case 2:
                UpdateDiamond(moneyNum);
                break;
        }
    }

    public void MoneyChange(int moneyType, int moneyNum)
    {
        switch (moneyType)
        {
            case 1:
                {
                    int gold = this._Gold + moneyNum;
                    UpdateGold(gold);
                } return;
            case 2:
                {
                    int diamond = this._Diamonds + moneyNum;
                    UpdateDiamond(diamond);
                } return;
        }
        return;
    }
    /// <summary>
    /// 更新碎片、材料、消耗品等数据
    /// </summary>
    /// <param name="infoList"></param>
    public void UpdateItem(List<fogs.proto.msg.ItemInfo> infoList)
    {
        if (infoList == null || infoList.Count == 0)
            return;
        for (int i = 0; i < infoList.Count; i++)
        {
            fogs.proto.msg.ItemInfo itemInfo = infoList[i];
            UpdateItem(itemInfo);
        }
        if (UpdatePlayerItemsEvent != null)
            UpdatePlayerItemsEvent();
    }

    /// <summary>
    /// 更新单个物品数据（碎片、材料、消耗品）
    /// </summary>
    /// <param name="itemInfo"></param>
    public void UpdateItem(fogs.proto.msg.ItemInfo itemInfo)
    {
        if (itemInfo == null)
            return;
        IDType type = CommonFunction.GetTypeOfID(itemInfo.id.ToString());
        switch (type)
        {
            case IDType.Diamond:
                {
                    UpdateDiamond(itemInfo.num);
                    break;
                }
            case IDType.Gold:
                {
                    UpdateGold(itemInfo.num);
                    break;
                }
            case IDType.SP:
                {
                    UpdateSP(itemInfo.num);
                    break;
                }
            case IDType.Medal:
                {
                    UpdateMedal(itemInfo.num);
                    break;
                }
            case IDType.Honor:
                {
                    UpdateHonor(itemInfo.num);
                    break;
                }
            case IDType.UnionToken:
                {
                    UpdateUnionToken(itemInfo.num);
                    break;
                }
            case IDType.RecycleCoin:
                {
                    UpdateRecycleCoin(itemInfo.num);
                    break;
                }
            case IDType.Prop:
                {
                    ItemInfo item = ConfigManager.Instance.mItemData.GetItemInfoByID(itemInfo.id);
                    if (item == null)      //配置表中找不到该项数据则跳过
                        break;
                    Item data = new Item();
                    data.uid = itemInfo.id;
                    data.id = itemInfo.id;
                    data.num = itemInfo.num;
                    if (item.type == (int)ItemTypeEnum.EquipChip)
                    {
                        UpdateChipBag(data);
                    }
                    else if (item.type == (int)ItemTypeEnum.SoldierChip)
                    {
                        OneAddDebris(itemInfo);
                    }
                    else if (item.type == (int)ItemTypeEnum.Material)
                    {
                        UpdateMaterialBag(data);
                    }
                    else if (item.type == (int)ItemTypeEnum.Prop)
                    {
                        UpdatePropBag(data);
                    }
                    break;
                }
            case IDType.EQ:
                {
                    break;
                }
            case IDType.Soldier:
                {
                    break;
                }
        }
        if (UpdatePlayerItemsEvent != null)
            UpdatePlayerItemsEvent();
    }

    /// <summary>
    /// 更新掉落包数据
    /// </summary>
    /// <param name="data"></param>
    public void UpdateDropData(fogs.proto.msg.DropList data)
    {
        if (data == null)
            return;
        //掉落装备
        if (data.equip_list != null)
        {
            PlayerData.Instance.MultipleAddWeapon(data.equip_list);
        }
        //掉落物品数据
        if (data.item_list != null)
        {
            for (int i = 0; i < data.item_list.Count; i++)
            {
                ItemInfo item = ConfigManager.Instance.mItemData.GetItemInfoByID(data.item_list[i].id);
                if (item == null) continue; //配置表中找不到该数据时  跳过
                fogs.proto.msg.ItemInfo info = data.item_list[i];
                Item _item = new Item();
                _item.id = info.id;
                _item.num = info.num;
                _item.uid = info.id;
                switch ((ItemTypeEnum)item.type)
                {
                    case ItemTypeEnum.EquipChip:
                        {
                            UpdateChipBag(_item);
                        }
                        break;
                    case ItemTypeEnum.SoldierChip:
                        {
                            OneAddDebris(info);
                        }
                        break;
                    case ItemTypeEnum.Material:
                        {
                            UpdateMaterialBag(_item);
                        } break;
                    case ItemTypeEnum.Prop:
                        {
                            UpdatePropBag(_item);
                        }
                        break;
                }
            }
        }
        //掉落士兵数据
        if (data.soldier_list != null)
        {
            for (int i = 0; i < data.soldier_list.Count; i++)
            {
                _SoldierDepot.oneAdd(data.soldier_list[i]);
            }
        }
        //掉落数值类数据
        if (data.special_list != null)
        {
            UpdateItem(data.special_list);
        }
        if (data.soul_list != null)
        {
            this._LifeSoulDepot.AddPackLifeSouls(data.soul_list);
        }
        if (data.pet_list != null)
        {
            this._PetDepot.AddPetList(data.pet_list);
        }
    }

    /// <summary>
    /// 更新玩家体力
    /// </summary>
    /// <param name="sp"></param>
    public void UpdateSP(int sp)
    {
        _Physical = sp;
        LocalNotificationManager.Instance.SPRecoverMax();
        if (UpdatePlayerSPEvent != null)
            UpdatePlayerSPEvent();
    }

    /// <summary>
    /// 经验更新
    /// </summary>
    /// <param name="exp"></param>
    public void UpdateExpInfo(AddExp exp)
    {
        if (exp == null)
            return;
        _CurrentExp = (uint)exp.exp;
        UpdateSP(exp.ph_power);
        if (LevelUpEvent != null && exp.level > _Level)
        {
            SetMaxPhysical((uint)exp.level);
            LevelUpEvent((int)_Level, exp.level);
        }
        _Level = (uint)exp.level;
        UpdatePlayerAttribute(exp.attr);
        SerializeHeroSkill(exp.skills);
        this.MultipleAddWeapon(exp.equip_list);
        _PetDepot.AddPetList(exp.pet_list);
        if (UpdateLevelEvent != null)
        {
            UpdateLevelEvent();
        }
        TalkingDataManager.Instance.SetAccountLevel((int)_Level);
    }

    public void UpdateHonor(int honor)
    {
        _Honor = honor;
        if (UpdateHonorEvent != null)
            UpdateHonorEvent();
    }

    public void UpdateUnionToken(int unionToken)
    {
        UnionToken = unionToken;
        if (UpdateUnionTokenEvent != null)
            UpdateUnionTokenEvent();
    }
    public void UpdateRecycleCoin(int recycleCoin)
    {
        RecycleCoin = recycleCoin;
        if (UpdateRecycleCoinEvent != null)
            UpdateRecycleCoinEvent();
    }

    public void UpdateMedal(int medal)
    {
        _Medal = medal;
        if (UpdateMedalEvent != null)
            UpdateMedalEvent();
    }

    /// <summary>
    /// 更新玩家銅錢
    /// </summary>
    /// <param name="coin"></param>
    public void UpdateGold(int coin)
    {
        _Gold = coin;

        if (UpdatePlayerGoldEvent != null)
            UpdatePlayerGoldEvent();
    }

    /// <summary>
    /// 更新玩家代币（宝石）
    /// </summary>
    /// <param name="gem"></param>
    public void UpdateDiamond(int gem)
    {
        _Diamonds = gem;

        if (UpdatePlayerDiamondEvent != null)
            UpdatePlayerDiamondEvent();
    }
    /// <summary>
    /// 更新玩家名称
    /// </summary>
    /// <param name="name"></param>
    public void UpdatePlayerNickName(string name)
    {
        _NickName = name;
    }
    /// <summary>
    /// 更新碎片数据
    /// </summary>
    public void UpdateChipBag(Item data)
    {
        if (data == null) return;
        Item _item = null;
        if (_ChipBag == null)
        {
            _ChipBag = new List<Item>();
        }
        _item = _ChipBag.Find((t_item) => { return data.id == t_item.id; });
        if (_item == null)
        {
            _ChipBag.Add(data);
        }
        else
        {
            if (data.num == 0)   //理论是增加数据 不存在为0的情况  
            {
                _ChipBag.Remove(_item);
            }
            else
            {
                _item.num = data.num;
            }
        }
    }
    /// <summary>
    /// 更新消耗品数据
    /// </summary>
    public void UpdatePropBag(Item data)
    {
        if (data == null) return;
        Item _item = null;
        if (_PropBag == null)
        {
            _PropBag = new List<Item>();
        }
        _item = _PropBag.Find((t_item) => { return t_item.id == data.id; });
        if (_item == null)
        {
            _PropBag.Add(data);
        }
        else
        {
            if (data.num == 0)   //理论是增加数据 不存在为0的情况  
            {
                _PropBag.Remove(_item);
            }
            else
            {
                _item.num = data.num;
            }
        }
    }
    /// <summary>
    /// 更新材料数据
    /// </summary>
    public void UpdateMaterialBag(Item data)
    {
        if (data == null) return;
        Item _item = null;
        if (_MaterialBag == null)
        {
            _MaterialBag = new List<Item>();
        }
        _item = _MaterialBag.Find((t_item) => { return t_item.id == data.id; });
        if (_item == null)
        {
            _MaterialBag.Add(data);
        }
        else
        {
            if (data.num == 0)   //理论是增加数据 不存在为0的情况  
            {
                _MaterialBag.Remove(_item);
            }
            else
            {
                _item.num = data.num;
            }

        }
    }

    public void UpdateOnlineTime()
    {
        if (UpdateOnlineEvent != null)
            UpdateOnlineEvent();
    }

    /// <summary>
    /// 获取玩家拥有的物品(消耗品，碎片，材料)
    /// 返回该物品的ID以及数量
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public Item GetOwnItemByID(uint id)
    {
        Item _item = null;
        if (_ChipBag != null)
        {
            _item = _ChipBag.Find((tmp) => { return tmp.id == id; });
            if (_item != null) return _item;
        }
        if (_PropBag != null)
        {
            _item = _PropBag.Find((tmp) => { return tmp.id == id; });
            if (_item != null) return _item;
        }
        if (_MaterialBag != null)
        {
            _item = _MaterialBag.Find((tmp) => { return tmp.id == id; });
            if (_item != null) return _item;
        }
        return _item;
    }

    /// <summary>
    /// 更新装备合成成功后数据
    /// </summary>
    public void UpdateEquipChipCompositeData(List<fogs.proto.msg.ItemInfo> updateitems, fogs.proto.msg.Equip equip)
    {
        UpdateItemInfoData(updateitems);
        _SoldierEquip.OneAdd(equip);
    }

    /// <summary>
    /// 更新服务器发送的Item数据
    /// </summary>
    /// <param name="items"></param>
    public void UpdateItemInfoData(List<fogs.proto.msg.ItemInfo> items)
    {
        for (int i = 0; i < items.Count; i++)
        {
            fogs.proto.msg.ItemInfo _info = items[i];
            Item _item = null;
            _item = _MaterialBag.Find((t_item) => { return items[i].id == t_item.id; });
            if (_item != null)
            {
                if (_item.num == 0)   //数量为0时  删除该条数据
                {
                    _MaterialBag.Remove(_item);
                }
                else
                {
                    _item.num = _info.num;
                }
                continue;
            }
            _item = _ChipBag.Find((t_item) => { return items[i].id == t_item.id; });
            if (_item != null)
            {
                if (_item.num == 0)   //数量为0时  删除该条数据
                {
                    _ChipBag.Remove(_item);
                }
                else
                {
                    _item.num = _info.num;
                }
                continue;
            }
            _item = _PropBag.Find((t_item) => { return items[i].id == t_item.id; });
            if (_item != null)
            {
                if (_item.num == 0)   //数量为0时  删除该条数据
                {
                    _PropBag.Remove(_item);
                }
                else
                {
                    _item.num = _info.num;
                }
                continue;
            }
        }
        if (UpdatePlayerItemsEvent != null)
            UpdatePlayerItemsEvent();
    }

    public void UpdateShopInfo(ShopType shopType, ShopInfo info)
    {
        if (ShopInfos.ContainsKey(shopType))
        {
            ShopInfos.Remove(shopType);
        }
        ShopInfos.Add(shopType, info);
    }

    public void UpdateBuyCommodity(ShopType shopType, int id)
    {
        if (ShopInfos.ContainsKey(shopType))
        {
            for (int i = 0; i < ShopInfos[shopType].commodity_info.Count; i++)
            {
                if (ShopInfos[shopType].commodity_info[i].id == id)
                {
                    ShopInfos[shopType].commodity_info[i].sell_out = (int)ECommodityState.HasBought;
                }
            }

        }
    }


    /// <summary>
    /// 是否满足等级限制条件 true 则说明满足等级条件
    /// </summary>
    /// <param name="info"></param>
    /// <returns></returns>
    public bool IsOutstripLevel(int lockLevel)
    {
        return PlayerData.Instance._Level >= lockLevel;
    }

    public void SetPlayerAuthorizeInfo(AuthorizeResp data)
    {
        _AccountName = data.accname;
        _AccountID = data.accid;
        Sign = data.sign;
        TimeStamp = data.timestamp;
        //SDKURL = data.arg1;
        //SDKURL = string.Format("{0}?{1}_{2}", data.arg1, SDKManager.Instance.Get_UserID, LoginModule.Instance.CurServerId);
        TalkingDataManager.Instance.SetAccount(_AccountID.ToString());
        BuglyManager.Instance.SetUserID(_AccountID.ToString());

    }

    private void _SoldierEquipInit(List<fogs.proto.msg.Equip> equipList)
    {
        foreach (var temp in equipList)
        {
            if (temp.postion > 0)
            {
                Weapon wp = this._SoldierEquip.FindByUid(temp.uid);
                if (wp == null) continue;

                Soldier sd = this._SoldierDepot.FindByUid(temp.uid);
                if (sd == null) continue;

                if (temp.postion > 6) continue;
                sd._equippedDepot._EquiptList[temp.postion - 1] = wp;
            }
        }
    }


    public void UpdateVip(int vipLv, int vipExp)
    {
        _VipLv = (byte)vipLv;
        VipExp = vipExp;

        if (UpdateVipEvent != null)
        {
            UpdateVipEvent();
        }
    }
    /// <summary>
    /// 激活光环技能
    /// </summary>
    /// <param name="role"></param>
    public void ActivateHalo(RoleAttribute role)
    {
        this._SkillsDepot.ActivateHalo(role);
    }

    /// <summary>
    /// 刷新数据
    /// </summary>
    /// <param name="type"></param>
    /// <param name="gateID"></param>
    /// <param name="star_level"></param>
    /// <param name="dropItem"></param>
    /// <param name="addexp"></param>
    /// <param name="ph_power"></param>
    /// <param name="todayCount"></param>
    public void UpdateBattleSettle(EFightType type, uint gateID, uint star_level, DropList dropItem, fogs.proto.msg.AddExp addexp, int ph_power, fogs.proto.msg.TodayPlayDungeons todayCount)
    {
        fogs.proto.msg.PassDungeon info = null;
        switch (type)
        {
            case EFightType.eftMain:
                {
                    List<fogs.proto.msg.PassDungeon> gatelist = _MajorDungeonInfoList;
                    info = gatelist.Find((PassDungeon tmp) => { return tmp.dgn_id == gateID; });
                    if (info == null)   //说明当前无该项数据  属于新关卡 则直接添加数据
                    {
                        info = new PassDungeon();
                        info.dgn_id = gateID;
                        info.star_level = star_level;
                        gatelist.Add(info);
                    }
                    else
                    {
                        if (info.star_level <= star_level)   //如果是低于当前评级  则不刷新数据
                        {
                            info.star_level = star_level;
                        }
                    }
                    _TodayPlayDungeons = todayCount;
                }
                break;
            case EFightType.eftActivity:
                {

                }
                break;
            case EFightType.eftEndless:
                {

                }
                break;
            default: break;
        }
        //_TodayPlayDungeons = todayCount;
        UpdateSP(ph_power);
        UpdateExpInfo(addexp);
        UpdateDropData(dropItem);
    }

    /// <summary>
    /// 异域探险结算
    /// </summary>
    public void UpdateExoticBattleSettle(UnionPveDgnRewardResp resp)
    {
        UnionPveDgnInfo info = UnionModule.Instance.UnionInfo.dgn_info.Find((tmp) =>
        {
            if (tmp == null) return false;
            return tmp.dgn_id == resp.dgn_id;
        });
        if (info == null)
        {
            info = new UnionPveDgnInfo();
            info.dgn_id = resp.dgn_id;
            info.finished = resp.is_pass;
            info.total_hurt = resp.total_damage;
            UnionModule.Instance.UnionInfo.dgn_info.Add(info);
        }
        else
        {
            info.finished = resp.is_pass;
            info.total_hurt = resp.total_damage;
        }
        UpdateDropData(resp.drop_items);
    }

    public void AddGuideFinishList(uint type)
    {
        if (!DoneNewbieGuideList.Contains(type))
            DoneNewbieGuideList.Add(type);
    }
    public bool GoldIsEnough(int type, int num)
    {
        switch (type)
        {
            case 1: return this._Gold >= num;
            case 2: return this._Diamonds >= num;
        }
        return false;
    }

    public void NotifyResetData(NotifyReset data)
    {
        if (data == null) return;
        PlayerData.Instance._TodayPlayDungeons = data.reset_dgn;
        PlayerData.Instance.WorldChatCount = (uint)data.world_chat_times;
        RecruitFreeCount = (byte)data.recruit_brave_times;
        //RankModule.Instance.NeedRefreshRank = true;
        LastBraveRecruitTime = 0;
        FeedbackRestCount = 3;
        BuySPTimes = data.buy_phpower_times;
        ExchangeGoldTimes = 0;
        LoginChangeInfo.continu_num = data.login_continu_num;
        LoginChangeInfo.cumulative_num = data.login_cumulative_num;
        LoginChangeInfo.get_continu_award = 0;
        UnionModule.Instance.CharUnionInfo = null;
        CharUnionInfo = data.union_info;
        OnlineRewardInfo = data.online_reward_info;
        this.DrowEquipFreeCount = data.extract_count;
        if (data.query_activity_sign == 1) GameActivityModule.Instance.SetDayReSet();
        if (ShopInfos.ContainsKey(ShopType.ST_NomalShop))
            ShopInfos[ShopType.ST_NomalShop].hand_update_num = 0;
        if (ShopInfos.ContainsKey(ShopType.ST_HonorShop))
            ShopInfos[ShopType.ST_HonorShop].hand_update_num = 0;
        if (ShopInfos.ContainsKey(ShopType.ST_MedalShop))
            ShopInfos[ShopType.ST_MedalShop].hand_update_num = 0;
        if (ShopInfos.ContainsKey(ShopType.ST_UnionShop))
            ShopInfos[ShopType.ST_UnionShop].hand_update_num = 0;
        if (ShopInfos.ContainsKey(ShopType.ST_RecycleShop))
            ShopInfos[ShopType.ST_UnionShop].hand_update_num = 0;
        if (NotifyResetEvent != null)
        {
            NotifyResetEvent(data);
        }
    }

    /// <summary>
    /// 获取开启界面-功能对应关系
    /// </summary>
    public Dictionary<ETaskOpenView, OpenFunctionType> ObtainOpenRelationShip
    {
        get
        {
            if (dicOpenRelationShip == null)
            {
                dicOpenRelationShip = new Dictionary<ETaskOpenView, OpenFunctionType>();

                string tmpOpenInfo = ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.OPEN_VIEW_FUNC_RELATIONSHIP);
                if (!string.IsNullOrEmpty(tmpOpenInfo))
                {
                    string[] tmpArr = tmpOpenInfo.Split(';');
                    if ((tmpArr != null) && (tmpArr.Length > 0))
                    {
                        for (int i = 0; i < tmpArr.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tmpArr[i]))
                                continue;
                            string[] tmpInfoArr = tmpArr[i].Split(',');
                            int tmpTaskID = 0;
                            int tmpOpenID = 0;
                            if ((tmpInfoArr != null) && (tmpInfoArr.Length >= 2))
                            {
                                if (int.TryParse(tmpInfoArr[0], out tmpTaskID) && int.TryParse(tmpInfoArr[1], out tmpOpenID))
                                {
                                    dicOpenRelationShip.Add((ETaskOpenView)tmpTaskID, (OpenFunctionType)tmpOpenID);
                                }
                            }
                        }
                    }
                }
            }
            return dicOpenRelationShip;
        }
    }
    public void SendNotifyRefreshEvent(NotifyRefresh vData)
    {
        if (this.NotifyRefreshEvent != null)
            this.NotifyRefreshEvent(vData);
    }
}



