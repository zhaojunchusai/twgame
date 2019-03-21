using UnityEngine;
using System.Collections;

public class GlobalConst
{
    private static ConfigName _config;
    /// <summary>
    /// 配置表
    /// </summary>
    public static ConfigName Config
    {
        get
        {
            if (_config == null)
            {
                _config = new ConfigName();
            }
            return _config;
        }
    }

    private static UIName _ui;

    /// <summary>
    /// UI
    /// </summary>
    public static UIName UI
    {
        get
        {
            if (_ui == null)
            {
                _ui = new UIName();
            }
            return _ui;
        }
    }
    private static SystemNotificationID _notificationID;
    public static SystemNotificationID NotificationID
    {
        get
        {
            if (_notificationID == null)
            {
                _notificationID = new SystemNotificationID();
            }
            return _notificationID;
        }
    }
    private static SpriteName _spriteName;
    public static SpriteName SpriteName
    {
        get
        {
            if (_spriteName == null)
            {
                _spriteName = new SpriteName();
            }
            return _spriteName;
        }
    }

    private static SoundName _sound;

    /// <summary>
    /// 音乐或者音效
    /// </summary>
    public static SoundName Sound
    {
        get
        {
            if (_sound == null)
            {
                _sound = new SoundName();
            }
            return _sound;
        }
    }

    public static TextureName _texture;
    /// <summary>
    /// Texture
    /// </summary>
    public static TextureName Texture
    {
        get
        {
            if (_texture == null)
            {
                _texture = new TextureName();
            }
            return _texture;
        }
    }

    public static GameObjectName _gameobject;
    /// <summary>
    /// Gameobject
    /// </summary>
    public static GameObjectName GameObject
    {
        get
        {
            if (_gameobject == null)
            {
                _gameobject = new GameObjectName();
            }
            return _gameobject;
        }
    }

    public static EffectName _effect;

    /// <summary>
    /// 特效
    /// </summary>
    public static EffectName Effect
    {
        get
        {
            if (_effect == null)
            {
                _effect = new EffectName();
            }
            return _effect;
        }
    }

    #region 发包设置参数
    /// <summary>
    /// GM开关 发布正式版 改为false
    /// </summary>
    public static bool ISOPENGM = true;

    /// <summary>
    /// 是否打开新手引导
    /// </summary>
    public static bool IS_OPEN_GUIDE = false;

    /// <summary>
    /// 是否为测试包  发正式版时需更改为false
    /// </summary>
    public const bool IS_TESTAPK = true;
    /// <summary>
    /// 是否是Google包
    /// </summary>
    public const bool IS_GOOGLEAPK = true;

    public const float VERSION_CODENUMBER = 1;

    public const bool ISOPENCODEUPDATE = true;

    public const string GAMENAME = "倒退嚕騎士";//設置介面推送功能標題
    public const TargetPlatforms PLATFORM = TargetPlatforms.None;
    public static EServerType SERVER_TYPE = EServerType.Nei249_10201;
    public static string VersionName = "10.0.3";
    /// <summary>
    /// 是否优先使用下载资源
    /// 如果启用 则会检查服务端资源 并下载资源 在资源加载时会优先使用下载资源
    /// 正式版本  需要修改为true
    /// </summary>
    public const bool OPENDLC = false;
    /// <summary>
    /// 是否开启SDK操作
    /// </summary>
    public const bool ISOPENSDKOPERATE = false;
    #endregion

    public const float DEFAULT_SCREEN_SIZE_X = 1024;
    public const float DEFAULT_SCREEN_SIZE_Y = 576;
    public const int RENAME_CHAR_PRICE = 100;//改名消耗元宝数量
    public const int HEART_BEAT_TIME_OUT = 5;
    public const int FEEDBACK_COOLDOWN_TIME = 300;
    public const bool IsOpenStep = true;
    public const string COMMON_UNCONDITIONAL = "0";

    public const float EXCHANGE_GOLD_INCREASE_FACTOR = 100F;

    public const float ViewScaleAnim = 0.0001F;

    public static Vector3 ViewScaleAnimVec = new Vector3(ViewScaleAnim, ViewScaleAnim, ViewScaleAnim);

    //============================================== Battle ================================================================//
    public const string PATH_INTERNAL_TEXTURE = "InternalTexture/{0}";
    public const string PATH_PREFAB = "Prefabs/";
    public const string PATH_ROLE = PATH_PREFAB + "Role/Role_{0}";

    public const string RESOURCE_SUFFIX = "{0}.assetbundle";
    public const string RESOURCE_SCENE = "aloneres_Scene_{0}.assetbundle";
    public const string RESOURCE_ROLE = "role_" + RESOURCE_SUFFIX;
    public const string OPENCHAT = "2";
    public const string OFFCHAT = "1";

    //----------动画层级----------//
    public const int ANIMATION_TRACK_BASE = 0;      //基础[待机]//
    public const int ANIMATION_TRACK_BEHAVIOR = 1;  //行为[移动]//
    public const int ANIMATION_TRACK_SKILL = 2;     //技能[攻击]//
    public const int ANIMATION_TRACK_SPECIAL = 3;   //特殊[死亡]//

    //----------动画名字----------//
    public const string ANIMATION_NAME_IDLE = "idle";
    public const string ANIMATION_NAME_MOVE = "move";
    public const string ANIMATION_NAME_RUSH = "rush";
    public const string ANIMATION_NAME_VICTORY = "victory";
    public const string ANIMATION_NAME_ABILITY_BASIC = "basic_ability";
    public const string ANIMATION_NAME_ABILITY_MULTI = "multi_ability";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE = "ultimate_ability";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE6 = "ultimate_ability_6";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE8 = "ultimate_ability_8";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE10 = "ultimate_ability_10";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE12 = "ultimate_ability_12";
    public const string ANIMATION_NAME_ABILITY_ULTIMATE14 = "ultimate_ability_14";


    public static string[] ANIMATION_NAME_SKILL_ARR = new string[] { ANIMATION_NAME_ABILITY_ULTIMATE6, ANIMATION_NAME_ABILITY_ULTIMATE8, ANIMATION_NAME_ABILITY_ULTIMATE10, ANIMATION_NAME_ABILITY_ULTIMATE12, ANIMATION_NAME_ABILITY_ULTIMATE14 };
    public const string ANIMATION_NAME_ABILITY_TERMINATIVE = "terminative_ability";
    public const string ANIMATION_NAME_DEATH = "death";

    //============================================== ItemPath ================================================================//
    public const string DIR_ITEM_GETPATH = "Prefabs/UI/GetPathItem";
    public const string DIR_ITEM_ACTIVITIES = "Prefabs/UI/ActivitiesViewItem";
    public const string DIR_ITEM_STOREITEM = "Prefabs/UI/StoreItem";
    public const string DIR_ITEM_MAILITEM = "Prefabs/UI/MailItem";
    public const string DIR_ITEM_MAILATTACHMENTITEM = "Prefabs/UI/MailAttachmentItem";
    public const string DIR_ITEM_MAILATTACHMENTITEM_S = "Prefabs/UI/MailAttachmentItem_S";
    public const string DIR_ITEM_RECHARGEITEM = "Prefabs/UI/RechargeItem";
    public const string DIR_ITEM_VIPDESCITEM = "Prefabs/UI/VipDese";
    public const string DIR_ITEM_SIGNITEM = "Prefabs/UI/SignItem";

    //==================================================Effect===============================================================//
    public const int SYSTEMICONID = 1;
    public const int SYSTEMFRAME = 10001;
    public const string SYSTEMICON = "SystemIcon";
    public const string DIR_EFFECT_TRAILBLUE = "Trail";
    public const string DIR_EFFECT_LIZI_Z = "Lizi_Z";
    public const string DIR_EFFECT_LIZI_Y = "Lizi_Y";
    public const string DIR_EFFECT_LIZI_R = "Lizi_R";
    public const string DIR_EFFECT_VIP = "VIP";
    public const string DIR_EFFECT_TOPFUNC = "TopFunc";
    //public const string DIR_EFFECT_INTENSIFYITEM = "ZYX_IntensifyItem";//神器强化暂代资源
    //public const string DIR_EFFECT_CLICKEFFECT = "LHF_ClickEffect";      //界面点击特效
    public const string DIR_EFFECT_PVPREFRESH = "yanwutaishuaxin";//"LHF_PVPRefresh";//演舞台刷新特效
    public const string DIR_EFFECT_EQINTENSIFYITEM = "WZP_zhuangbeiqianghua";//"wujiangzhuangbeishengji";//"LHF_HEROEQIntensify";//英雄界面神器强化特效
    public const string DIR_EFFECT_SOLDIEREQUIP = "WZP_zhuangbeiqianghua1";//"LHF_HEROEQIntensify";//武将界面装备强化特效
    public const string DIR_EFFECT_HEROSKILL = "WZP_jinengshengji";//"LHF_HEROSKILLLEVELUP";//英雄界面技能升级特效
    public const string DIR_EFFECT_SOLDIERSKILL = "WZP_jinengshengji";// "LHF_SKILLLEVELUP";//武将界面技能升级特效
    public const string DIR_EFFECT_STOREREFRESH = "LHF_StoreRefresh";//商店刷新界面特效
    public const string DIR_EFFECT_CASTLELEVELUP = "WZP_sheshoushengji";//"chengbaoxiaobingshengji";//"LHF_CASTLELEVELUP";//城堡小兵升级特效
    public const string DIR_EFFECT_EQUIPADVANCED = "WZP_jinengshengji_1";//"wujiangshengxing";//"LHF_EQUIPADVANCED";//装备升星特效
    public const string DIR_EFFECT_FASTSTRENGTH = "WZP_yijianqianghua";//"wuqizhuangbeiyijianqianghua";//"LHF_EQIntensify";//一件强化
    public const string DIR_EFFECT_FASTINTENSIFY = "wuqizhuangbeiyijianqianghua";//"LHF_EQIntensifyItem";//一件装备
    public const string DIR_EFFECT_ONDEBRISCOMPOUND = "WZP_suipianhecheng";//"wuqisuipianhecheng";//"LHF_ONDEBRISCOMPOUND";//武将界面碎片合成特效
    public const string DIR_EFFECT_SOLDIERADVANCED = "WZP_suipianhecheng";//"wujiangshengxing";//武将升星
    public const string DIR_EFFECT_UPCASTLE = "WZP_chengbaoshengji";//"chengbaoshengji";//"LHF_HEROEQIntensify";//城堡升级特效
    public const string DIR_EFFECT_UPSTAR = "LHF_UPStar";//装备升星星星特效
    public const string DIR_EFFECT_SOLDIERLEVELUP = "WZP_wujiangshengji";//"LHF_EQIntensifyItem";//武将升级特效
    //public const string DIR_EFFECT_WARPGATE = "chuansongmen";//传送门
    //public const string DIR_EFFECT_MAGIC = "jingshenman";  //战斗界面蓝色能量条
    //public const string DIR_EFFECT_ENERGY = "nengliangman";//战斗界面红色能量条
    //public const string DIR_EFFECT_SKILL = "fangjineng";   //战斗界面技能按钮
    //public const string DIR_EFFECT_SLODIER = "zhaobing";   //战斗界面招兵按钮
    //public const string DIR_EFFECT_CHECKED = "yeqianxuanzhon";//活动界面选中特效
    public const string DIR_EFFECT_EQINTENSIFYITEMTRAIL = "WZP_dandao";//"ZYX_EQIntensifyItem_Taril";      //英雄界面神器强化拖尾粒子
    public const string DIR_EFFECT_SKILLINTENSIFYITEMTRAIL = "ZYX_SkillIntensifyItem_Taril";//英雄界面技能升级拖尾粒子
    public const string DIR_EFFECT_UNIONBATTLE = "effect_WZP_daotexiao";//英雄界面技能升级拖尾粒子
    public const string DIR_EFFECT_HEGEMONYLEFT = "effect_HegemonyLeft";//信仰神殿特效
    public const string DIR_EFFECT_HEGEMONYRIGHT = "effect_HegemonyRight";//信仰神殿特效
    public const string DIR_EFFECT_HEGEMONYRefresh = "effect_HegemonyRefresh";//信仰神殿特效
    public const string DIR_EFFECT_UNIONDONATION = "WZP_juanxiantexiao";//军团捐献
    public const string DIR_EFFECT_PRISONSTRANGER = "yanwutaishuaxin";  //奴隶界面陌生人刷新特效
    public const string DIR_EFFECT_PRISONENEMY = "yanwutaishuaxin";//奴隶界面仇人刷新特效
    public const string DIR_EFFECT_PRISONFLYEFFECT = "WZP_dandaotou";//囚牢飞行特效
    public const string DIR_EFFECT_PRISONBOOMEFFECT = "WZP_shouqu1";//囚牢爆炸特效
    public const string DIR_EFFECT_JOINUNION = "WZP_shuaxin";//军团界面刷新特效
    public const string DIR_EFFECT_UNIONLVUP = "WZP_juntuanwuzi1";//军团升级特效
    //============================================== Layer ================================================================//
    public const string LAYER_UI = "UI";
    public const string LAYER_FIGHT = "Fight";
    public const string LAYER_NGUI = "NGUI";


    //============================================== HeroResource ================================================================//
    //public const string HERO_RES_MALE = "role_10000001.assetbundle";
    //public const string HERO_RES_FAMALE = "role_10000000.assetbundle";
    //public const string HERO_ICON_MALE = "Icon_Head_10000001";
    //public const string HERO_ICON_FAMALE = "Icon_Head_10000000";
    public const uint HERO_COMID_MALE = 10000001;
    public const uint HERO_COMID_FAMALE = 10000000;
    public const uint HERO_SPEID_MALE = 10000003;
    public const uint HERO_SPEID_FAMALE = 10000002;
    public const string HERO_RES_STR = "role_{0}.assetbundle";
    public const string HERO_ICON_STR = "Icon_Head_{0}";

    //============================================== 战斗速度 ================================================================//
    /// <summary>
    /// 1倍速
    /// </summary>
    public const float MIN_FIGHT_SPEED = 1.5f;
    /// <summary>
    /// 2倍速
    /// </summary>
    public const float MID_FIGHT_SPEED = 1.5f;
    /// <summary>
    /// 2倍速
    /// </summary>
    public const float MAX_FIGHT_SPEED = 2.2f;
    //==============================================背包上限===================================================================//
    /// <summary>
    /// 背包格子堆叠上限
    /// </summary>
    public const int MAX_Item_Spill = 999;

    /// <summary>
    /// 技能释放特效开始播放
    /// </summary>
    public const string ANIMATION_EVENT_CAST = "cast";
    /// <summary>
    /// 近战-目标命中 远程-发射飞行物
    /// </summary>
    public const string ANIMATION_EVENT_PROJECTILE = "projectile";
    public const string ANIMATION_EVENT_PORJECTILE = "porjectile";//错误名字 需要美术同学修改资源文件//

    /// <summary>
    /// 第一个关卡ID
    /// </summary>
    public const uint FIRST_STAGE_ID = 70000001;

    /// <summary>
    /// 怪物类别-小怪
    /// </summary>
    public const int MONSTER_TYPE_COMMON = 0;
    /// <summary>
    /// 怪物类别-BOSS
    /// </summary>
    public const int MONSTER_TYPE_BOSS = 1;
    /// <summary>
    /// 怪物类别-超级兵
    /// </summary>
    public const int MONSTER_TYPE_SUPER = 2;
    /// <summary>
    /// 战斗界面英雄移动中场限制
    /// </summary>
    public const float SCENE_CENTER_LIMIT_WIDTH = 180;

    /// <summary>
    /// 角色对话临时层级
    /// </summary>
    public const int TEMPORARY_SORT_ORDER = 50;
}

/// <summary>
/// 特殊字段ID集[SpecialID配置表对应ID]
/// 10000056
/// </summary>
public class SpecialField
{
    /// <summary>
    /// 强制新手引导关卡
    /// </summary>
    public const uint SPECIALID_TUTORIAL = 10000002;
    /// <summary>
    /// 结算特殊处理关卡
    /// </summary>
    public const uint SPECIALID_FIGHTRESULTID = 10000003;

    /// <summary>
    /// 远征规则
    /// </summary>
    public const uint SPECIALID_EXPEDITION_RULE = 10000004;
    /// <summary>
    /// 奴隶规则
    /// </summary>
    public const uint SPECIALID_PRISON_RULE = 10000005;

    /// <summary>
    /// 免疫效果器-城堡
    /// </summary>
    public const uint SPECIALID_IMMUNEID_BARRACKS = 10000007;
    /// <summary>
    /// 免疫效果器-英雄
    /// </summary>
    public const uint SPECIALID_IMMUNEID_HERO = 10000008;
    /// <summary>
    /// 免疫效果器-BOSS
    /// </summary>
    public const uint SPECIALID_IMMUNEID_BOSS = 10000009;

    /// <summary>
    /// 角色比例-界面
    /// </summary>
    public const uint SPECIALID_ROLESCALE_UIHERO = 10000010;
    /// <summary>
    /// 角色比例-城楼射手
    /// </summary>
    public const uint SPECIALID_ROLESCALE_UISHOOTER = 10000011;
    /// <summary>
    /// 角色比例-英雄
    /// </summary>
    public const uint SPECIALID_ROLESCALE_HERO = 10000012;
    /// <summary>
    /// 角色比例-士兵
    /// </summary>
    public const uint SPECIALID_ROLESCALE_SOLDIER = 10000013;
    /// <summary>
    /// 角色比例-怪物
    /// </summary>
    public const uint SPECIALID_ROLESCALE_MONSTER = 10000014;
    /// <summary>
    /// 角色比例-BOSS
    /// </summary>
    public const uint SPECIALID_ROLESCALE_BOSS = 10000015;
    /// <summary>
    /// 角色比例-护送目标
    /// </summary>
    public const uint SPECIALID_ROLESCALE_ESCORT = 10000016;

    /// <summary>
    /// 限制时间-PVP
    /// </summary>
    public const uint SPECIALID_FIGHT_LIMITTIME_PVP = 10000017;
    /// <summary>
    /// 限制时间-奴隶
    /// </summary>
    public const uint SPECIALID_FIGHT_LIMITTIME_SLAVE = 10000018;
    /// <summary>
    /// 初始魔法-PVP
    /// </summary>
    public const uint SPECIALID_FIGHT_INITMAGIC_PVP = 10000019;
    /// <summary>
    /// 初始魔法-奴隶
    /// </summary>
    public const uint SPECIALID_FIGHT_INITMAGIC_SLAVE = 10000020;
    /// <summary>
    /// 战斗背景图-PVP
    /// </summary>
    public const uint SPECIALID_FIGHT_BACKGROUND_PVP = 10000021;
    /// <summary>
    /// 战斗背景图-奴隶
    /// </summary>
    public const uint SPECIALID_FIGHT_BACKGROUND_SLAVE = 10000022;
    /// <summary>
    /// 充值1块钱等于多少VIP经验
    /// </summary>
    public const int RECHARGE_TO_VIP_EXP = 10000023;
    /// <summary>
    /// 月卡描述
    /// </summary>
    public const int MONTH_CARD_DESC = 10000024;
    /// <summary>
    /// 远征奖励描述
    /// </summary>
    public const int EXPEDITION_AWARD_DESC = 10000025;
    /// <summary>
    /// 普通模式描述
    /// </summary>
    public const int CRUSADE_AWARD_DESC = 10000026;
    /// <summary>
    /// 护送描述
    /// </summary>
    public const int ESCORT_AWARD_DESC = 10000027;
    /// <summary>
    /// 精英描述
    /// </summary>
    public const int ELITE_AWARD_DESC = 10000028;
    /// <summary>
    /// 开启界面-功能对应关系
    /// </summary>
    public const int OPEN_VIEW_FUNC_RELATIONSHIP = 10000029;
    /// <summary>
    /// 强制引导的最后一个ID
    /// </summary>
    public const int FORCE_GUIDE_LAST_ID = 10000030;
    /// <summary>
    /// 登陆豪礼掉落包ID
    /// </summary>
    public const int FIRST_LOGIN_REWARD_DROP_ID = 10000031;
    /// <summary>
    /// 角色比例-超级兵
    /// </summary>
    public const int SPECIALID_ROLESCALE_SUPER = 10000032;
    /// <summary>
    /// 免疫效果器-传送门
    /// </summary>
    public const int SPECIALID_IMMUNEID_TRANSFER = 10000033;
    /// <summary>
    /// 免疫效果器-护送目标
    /// </summary>
    public const int SPECIALID_IMMUNEID_ESCORT = 10000034;
    /// <summary>
    /// 周卡描述
    /// </summary>
    public const int WEEK_CARD_DESC = 10000035;
    /// <summary>
    /// 永久卡描述
    /// </summary>
    public const int LIFE_CARD_DESC = 10000036;
    /// <summary>
    /// 绑定复制说明[QuickSDK]
    /// </summary>
    public const int BIND_COPY_DESC = 10000037;
    /// <summary>
    /// IDPlay 绑定奖励文字说明[台湾]
    /// </summary>
    public const int SPECIALID_IDPLAYBIND_AWARDDESC = 10000038;
    /// <summary>
    /// 自動戰鬥開啟條件提示[台湾]
    /// </summary>
    public const int SPECIALID_AUTOFIGHT_UNLOCKHINT = 10000039;
    /// <summary>
    /// 单次扫荡消耗元宝数
    /// </summary>
    public const int SPECIALID_SWEEP_CONSUMEDIAMOND = 10000040;
    /// <summary>
    /// 免疫效果器-超级怪
    /// </summary>
    public const uint SPECIALID_IMMUNEID_SUPERMONSTER = 10000041;
    /// <summary>
    /// 世界聊天CD时间
    /// </summary>
    public const uint SPECIALID_CHAT_WORLDCHATCD = 10000042;
    /// <summary>
    /// 好友刷新时间
    /// </summary>
    public const uint SPECIALID_FRIENDS_REFRESHCD = 10000043;
    /// <summary>
    /// 世界求救冷却时间
    /// </summary>
    public const uint SPECIALID_WORLD_WORLDHELPCD = 10000044;
    /// <summary>
    /// 军团求救冷却时间
    /// </summary>
    public const uint SPECIALID_HELP_CORPSHELPCD = 10000045;
    /// <summary>
    /// 奴役系统时间
    /// </summary>
    public const uint SPECIALID_PRSION_FREETIME = 10000046;
    /// <summary>
    /// 全服基金购买VIP等级限制
    /// </summary>
    public const uint SERVER_FOUNDATION_VIP = 10000047;
    /// <summary>
    /// 无尽通关描述
    /// </summary>
    public const uint ENDLESS_CLEARANCEDESC = 10000048;

    /// <summary>
    /// 排位赛每日奖励描述
    /// </summary>
    public const uint QUALIFYING_DAYAWARDDESC = 10000049;
    /// <summary>
    /// 排位赛每周奖励描述
    /// </summary>
    public const uint QUALIFYING_WEEKAWARDDESC = 10000050;
    /// <summary>
    /// 武將升星经验补足
    /// </summary>
    public const uint SOLDIERUPSTAR_EXPADD = 10000051;
    /// <summary>
    /// 军团拜访赠送的体力数
    /// </summary>
    public const uint UNION_SEND_HP_COUNT = 10000052;
    /// <summary>
    /// 命魂仓库 展示区域个数上限
    /// </summary>
    public const uint LIFESOUL_PREY_LIMIT = 10000053;
    /// <summary>
    /// 命魂仓库 单次购买命魂格子数
    /// </summary>
    public const uint LIFESOUL_BUYGRID_COUNT = 10000054;
    /// <summary>
    /// 宠物怒气值增长值
    /// </summary>
    public const uint PET_POWER_ADD = 10000055;
    /// <summary>
    /// 宠物怒气值最大值
    /// </summary>
    public const uint PET_POWER_MAX = 10000056;
}
/// <summary>
/// 配置文件名称
/// </summary>
public class ConfigName
{
    /// <summary>
    /// 关卡
    /// </summary>
    public string DIR_XML_STAGE = "config_Stage.assetbundle";
    /// <summary>
    /// 关卡怪物表
    /// </summary>
    public string DIR_XML_STAGEMONSTER = "config_StageMonster.assetbundle";
    /// <summary>
    /// 英雄表
    /// </summary>
    public string DIR_XML_HEROATTRIBUTE = "config_HeroAttribute.assetbundle";
    /// <summary>
    /// 怪物表
    /// </summary>
    public string DIR_XML_MONSTERATTRIBUTE = "config_MonsterAttribute.assetbundle";
    /// <summary>
    /// 武将表
    /// </summary>
    public string DIR_XML_SOLDIERATTRIBUTE = "config_SoldierAttribute.assetbundle";
    /// <summary>
    /// 武将甄选配置表
    /// </summary>
    public string DIR_XML_SOLDIERSELECTATTRIBUTE = "config_SoldierStep.assetbundle";
    /// <summary>
    /// 技能书返还配置表
    /// </summary>
    public string DIR_XML_SKILLRETURNMATERIALATTRIBUTE = "config_ReturnMaterial.assetbundle";
    /// <summary>
    /// 装备表
    /// </summary>
    public string DIR_XML_EQUIPTTRIBUTE = "config_EquipAttribute.assetbundle";
    /// <summary>
    /// 装备解锁表
    /// </summary>
    public string DIR_XML_GOGEQUIPLOCK = "config_GodEquipLock.assetbundle";
    /// <summary>
    /// 天将神兵
    /// </summary>
    public string DIR_XML_SACRIFICIALSYSYTEM = "config_SacrificialSystem.assetbundle";
    /// <summary>
    /// 技能表
    /// </summary>
    public string DIR_XML_SKILLATTRIBUTE = "config_SkillAttribute.assetbundle";
    /// <summary>
    /// 技能效果
    /// </summary>
    public string DIR_XML_SKILLEFFECT = "config_SkillEffect.assetbundle";
    /// <summary>
    /// 奴役战斗
    /// </summary>
    public string DIR_XML_SLAVEINCOME = "config_SlaveIncome.assetbundle";
    /// <summary>
    /// 材料包表
    /// </summary>
    public string DIR_XML_MATERIALBAGATTRIBUTE = "config_MaterialsBagAttribute.assetbundle";
    /// <summary>
    /// 游戏基础配置表
    /// </summary>
    public string DIR_XML_APPCONFIG = "config_AppConfig.assetbundle";
    /// <summary>
    /// 章节配置表
    /// </summary>
    public string DIR_XML_CHAPTER = "config_Chapter.assetbundle";
    /// <summary>
    /// 物品表
    /// </summary>
    public string DIR_XML_ITEM = "config_Item.assetbundle";
    /// <summary>
    /// 掉落包
    /// </summary>
    public string DIR_XML_DROPPACK = "config_Droppack.assetbundle";
    //public string DIR_XML_GAMEACTIVITY_DROPPACK = "config_GameActivityDroppack.assetbundle";
    public string DIR_XML_SONPACK = "config_Sonpack.assetbundle";
    /// <summary>
    /// 关卡角色积分对应配置表
    /// </summary>
    public string DIR_XML_CHAPTERGRADE = "config_ChapterGrade.assetbundle";
    /// <summary>
    /// 特殊数据配置表
    /// </summary>
    public string DIR_XML_SPECIALID = "config_SpecialID.assetbundle";

    /// <summary>
    /// 招募系统XML
    /// </summary>
    public string DIR_XIM_RECRUIT = "config_Recruit.assetbundle";
    /// <summary>
    /// 商品表
    /// </summary>
    public string DIR_XML_COMMODITY = "config_CommodityList.assetbundle";
    public string DIR_XML_VIP = "config_WelfareVIP.assetbundle";
    public string DIR_XML_TIMESEXPEND = "config_TimesExpend.assetbundle";
    public string DIR_XML_TIMESBUY = "config_TimesBuy.assetbundle";
    public string DIR_XML_RECHARGE = "config_Recharge.assetbundle";
    public string DIR_XML_LOGINAWARD = "config_LoginAward.assetbundle";
    public string DIR_XML_LOGINAWARD_CON = "config_ContinuLogin.assetbundle";
    public string DIR_XML_LOGINAWARD_TOTAL = "config_CumulativeLogin.assetbundle";
    public string DIR_XML_LIVENESS_AWARD = "config_LivenessAward.assetbundle";
    public string DIR_XML_LIVENESS_TASK = "config_LivenessTask.assetbundle";
    public string DIR_XML_EXCHANGE_GOLD = "config_ExchangeCurrency.assetbundle";
    public string DIR_XML_GUIDE_DATA = "config_GuideData.assetbundle";
    public string DIR_XML_GUIDE_STEP = "config_GuideStep.assetbundle";
    public string DIR_XML_MALL = "config_MallCommodityList.assetbundle";
    public string DIR_XML_MAIN_CITY_UNLOCK = "config_MainCityUnlock.assetbundle";
    /// <summary>
    /// 远征表
    /// </summary>
    public string DIR_XML_EXPEDITION = "config_Expedition.assetbundle";
    public string DIR_XML_EXPEDITIONCHAPTER = "config_ExpeditionChapter.assetbundle";

    public string DIR_XML_TASK = "config_Task.assetbundle";
    /// <summary>
    /// 战斗力计算配置表
    /// </summary>
    public string DIR_XML_COMBATPOWER = "config_CombatPower.assetbundle";
    /// <summary>
    /// 竞技场
    /// </summary>
    public string DIR_XML_ARENA = "config_Arena.assetbundle";
    /// <summary>
    /// 竞技场
    /// </summary>
    public string DIR_XML_ARENAAWARD = "config_ArenaAward.assetbundle";
    /// <summary>
    /// 升级解锁表
    /// </summary>
    public string DIR_XML_OPENLEVEL = "config_OpenLevel.assetbundle";
    public string DIR_XML_DROWEQUIP = "config_DrawEquip.assetbundle";
    public string DIR_XML_GUIDETIPS = "config_GuideTips.assetbundle";
    public string DIR_XML_FRIENDMISSION = "config_FriendsTask.assetbundle";
    public string DIR_XML_FRIENDSPECIAL = "config_FriendsSpecial.assetbundle";
    /// <summary>
    /// 城堡配置表
    /// </summary>
    public string DIR_XML_CASTLEATTRIBUTE = "config_CastleAttribute.assetbundle";
    /// <summary>
    /// Boss出场对话表
    /// </summary>
    public string DIR_XML_BOSSTALK = "config_BossTalk.assetbundle";
    /// <summary>
    /// 成就系统及头像、头像框表
    /// </summary>
    public string DIR_XML_ACHIEVEMENT = "config_PlayerAchievement.assetbundle";
    public string DIR_XML_FRAME = "config_PlayerFrame.assetbundle";
    public string DIR_XML_PLAYERPORTAIT = "config_PlayerPortrait.assetbundle";

    public string DIR_XML_DIALOGUE = "config_Talks.assetbundle";

    public string DIR_XML_DIALOGUETRIGGER = "config_TriggerTalks.assetbundle";

    //public string DIR_XML_GAMEACTIVITY = "config_GameActivity.assetbundle";

    public string DIR_XML_RANDOMATTRIBUTE = "config_RandomAttribute.assetbundle";

    public string DIR_XML_GAMEACTIVITYTYPE = "config_GameActivityType.assetbundle";
    /// <summary>
    /// 公会图标
    /// </summary>
    public string DIR_XML_UNION_ICON = "config_UnionBadge.assetbundle";
    public string DIR_XML_UNION_BASE = "config_UnionBaseInfo.assetbundle";
    public string DIR_XML_UNION_UPGRADE = "config_UnionUpgrade.assetbundle";
    public string DIR_XML_UNION_RANKREWARD = "config_UnionRankReward.assetbundle";
    public string DIR_XML_ONLINEPACKAGE = "config_GameOnlinePackage.assetbundle";

    /// <summary>
    /// 新手任务
    /// </summary>
    public string DIR_XML_NOVICETASK = "config_NoviceTask.assetbundle";

    /// <summary>
    /// 新手任务子任务表
    /// </summary>
    public string DIR_XML_NOVICESUBTASKS = "config_NoviceSubtasks.assetbundle";

    /// <summary>
    /// 攻略配置表
    /// </summary>
    public string DIR_XML_WALKTHROUGH = "config_Walkthrough.assetbundle";

    public string DIR_XML_ERRORCODE = "config_ErrorCode.assetbundle";
    /// <summary>
    /// 全服霸主配置表
    /// </summary>
    public string DIR_XML_SERVERHEGEMONY = "config_ServerHegemony.assetbundle";
    public string DIR_XML_SERVERHEGEMONYUNIONAWARD = "config_ServerHegemonyUnionAward.assetbundle";
    public string DIR_XML_SERVERHEGEMONYSELFAWARD = "config_ServerHegemonySelfAward.assetbundle";

    /// <summary>
    /// 征战配置
    /// </summary>
    public string DIR_XML_CAMPAIGN = "config_Campaign.assetbundle";
    /// <summary>
    /// 令牌配置
    /// </summary>
    public string DIR_XML_CAMPAIGN_TOKEN = "config_CampaignToken.assetbundle";
    /// <summary>
    /// 城堡配置
    /// </summary>
    public string DIR_XML_CAMPAIGN_CASTLE = "config_CampaignCastle.assetbundle";
    /// <summary>
    /// 攻城略地奖励
    /// </summary>
    public string DIR_XML_CAMPAIGN_AWARD = "config_CampaignAward.assetbundle";

    public string DIR_XML_QUALIFYING_AWARD = "config_QualifyingReward.assetbundle";
    public string DIR_XML_QUALIFYING_RANK = "config_QualifyingRank.assetbundle";
    public string DIR_XML_RULEDESC = "config_RuleDesc.assetbundle";
    public string DIR_XML_RANKVIEW = "config_RankView.assetbundle";
    public string DIR_XML_RECYCLEVIEW = "config_RecycleView.assetbundle";


    public string DIR_XML_LIFESOUL = "config_LifeSoul.assetbundle";
    public string DIR_XML_LIFESOULBASE = "config_LifeSoulBaseInfo.assetbundle";
    public string DIR_XML_EQUIPCOORDINATE = "config_EquipCoordinates.assetbundle";
    public string DIR_XML_COMBATPETS = "config_CombatPets.assetbundle";
    public string DIR_XML_PETSUPDATE = "config_CombatPetsUpdate.assetbundle";
    /// <summary>
    /// 跨服战场
    /// </summary>
    public string DIR_XML_CROSSSERVERWAR = "config_CrossServerWar.assetbundle";
    public string DIR_XML_CROSSSERVERTIME = "config_CrossServerTime.assetbundle";
    public string DIR_XML_CROSSSERVERTERRITORY = "config_CrossServerTerritory.assetbundle";
    public string DIR_XML_CROSSSERVERBATTLE = "config_CrossServerBattle.assetbundle";
    public string DIR_XML_CROSSSERVERSEASON = "config_CrossServerSeason.assetbundle";
    public string DIR_XML_CROSSSERVERMILITARYRANK = "config_CrossServerMilitaryRank.assetbundle";

}
public class EquptedName
{
    public static int[] DIR_EQUPTED_NAME = new int[8] { 0, 0, 0, 2, 1, 1, 1, 4 };
    public static int[] frameId = new int[] { 10001, 20001, 20002, 3200001, 3200002, 3200003, 3200004, 3200005, 3200006, 3200007, 3200008, 3200009, 3200010, 3200011 };
    public static int[] SoldierStar = new int[7] { 1, 1, 2, 4, 8, 16, 32 };
}
public class GlobColor
{
    public static Color EquipEffectType2 = new Color(0.047f, 0.435f, 0.043f);
}
/// <summary>
/// UI
/// </summary>
public class UIName
{


    public string DIR_UINAME_GAMEUI = "GameUI";

    public string DIR_UINAME_GAMESETTLEMENTUI = "GameSettlementUI";
    public string DIR_UINAME_GETSERVERINFOUI = "GetServerinfoUI";
    public string DIR_UI_UISYSTEMPAR = "UISystem/UICamera/Anchor/Panel";
    public string DIR_UINAME_SEEDETAILUI = string.Empty;
    public string CHAPTERBGNAME = "image_ChapterBG.assetbundle";

    public string LonginView = "view_LoginView.assetbundle";

}

public class SpriteName
{
    public string LoginBG = "InternalTexture/GameLoginBG";

    /// <summary>
    /// 元宝
    /// </summary>
    public string Diamond = "ZCJ_icon_daibi_l";
    /// <summary>
    /// 金币
    /// </summary>
    public string Gold = "ZCJ_icon_jinbi_l";
    /// <summary>
    /// 战功
    /// </summary>
    public string Honor = "CMN_icon_zg";
    /// <summary>
    /// 战功
    /// </summary>
    public string UnionToken = "JTGL_Icon_jthb";
    /// <summary>
    /// 勋章
    /// </summary>
    public string Medal = "CMN_icon_xz";
    /// <summary>
    /// 体力
    /// </summary>
    public string SP = "ZCJ_icon_tili_l";
    /// <summary>
    /// 造化之源
    /// </summary>
    public string RecycleCoin = "ZHRL_icon_zhzy";
    /// <summary>
    /// 元宝
    /// </summary>
    public string Diamond_L = "ZCJ_icon_daibi_l";

    /// <summary>
    /// 金币
    /// </summary>
    public string Gold_L = "ZCJ_icon_jinbi_l";

    /// <summary>
    /// 体力 大图标
    /// </summary>
    public string SP_L = "ZCJ_icon_tili_l";
    /// <summary>
    /// 英雄经验，主角经验
    /// </summary>
    public string HeroExp = "CMN_icon_yxjy";
    /// <summary>
    /// 士兵经验
    /// </summary>
    public string SoldierExp = "CMN_icon_sbjy";
    public string SacrifialEffectBG_Men = "MenBGEFFECT";
    public string SacrifialEffectBG_Refining = "DINGEFFECT";
    public string GetEquipEffectBG_YuanBaoGe = "YuanBaoGe";

    public string Quality_1 = "Cmn_BG_Big";
    public string Quality_2 = "Cmn_BG_Big_Green";
    public string Quality_3 = "Cmn_BG_Big_Blue";
    public string Quality_4 = "Cmn_BG_Big_Purple";
    public string Quality_5 = "Cmn_BG_Big_Yellow";
    public string Quality_6 = "Cmn_BG_Big_Red";
    public string QualityBG_1 = "Cmn_FG_Gray";
    public string QualityBG_2 = "Cmn_FG_Green";
    public string QualityBG_3 = "Cmn_FG_Blue";
    public string QualityBG_4 = "Cmn_FG_Purple";
    public string QualityBG_5 = "Cmn_FG_Orange";
    public string QualityBG_6 = "Cmn_FG_Red";

    public string SpecailQuality = "CMN_bg_guaiwukuang";

    public string[] SoldierRecruitQuality = new string[] { "ZXG_BG_DZ_White", "ZXG_BG_DZ_Green", "ZXG_BG_DZ_Blue",
    "ZXG_BG_DZ_Purple","ZXG_BG_DZ_Yellow","ZXG_BG_DZ_red"};

    public string RecruitSoldierQualityBgRed = "ZXG_BG_red";
    public string RecruitSoldierQualityBgYellow = "ZXG_BG_Yellow";
    public string RecruitSoldierQualityBgPurple = "ZXG_BG_Purple";

    public string BRAVE = "ZXG_Lbl_Troopers";

    public string RIOT = "ZXG_Lbl_Good";

    public string MATCHLESS = "ZXG_Lbl_God";

    public string StoreNameNormal = "SP_Lbl_sp";
    public string StoreNameHonor = "SP_Lbl_jxk";
    public string StoreNameMedal = "SP_Lbl_xzsd";
    public string StoreNameUnion = "SP_Lbl_jtsd";


    public string BtnRecharge = "CZ_lbl_chongzhi 1";
    public string BtnVipPrivilege = "CZ_lbl_tequan";
    public string TitleRecharge = "CZ_lbl_chongzhi";
    public string TitleVipPrivilege = "CZ_lbl_tequan 1";

    public string CanGet = "PLLQ_lbl_klq";
    public string GetSuccess = "PLLQ_lbl_lingquchonggong";

    public string MarkEquipChip = "Cmn_Icon_sui";
    public string MarkSoldierChip = "Cmn_Icon_hun";

    public string TaskStateFinish = "CMN_bg_yiwancheng";
    public string TaskStateReceive = "CMN_bg_yilingqu";
    /// <summary>
    /// PVP胜利 排名上升
    /// </summary>
    public string PVP_BattleLogVictory = "ZDRZ_icon_sheng";
    /// <summary>
    /// PVP失败 排名下降
    /// </summary>
    public string PVP_BattleLogDefault = "ZDRZ_icon_bai";
    /// <summary>
    /// PVP失败  排名不变
    /// </summary>
    public string PVP_BattleLogDefaultDeuce = "ZDRZ_icon_bai2";
    /// <summary>
    /// PVP胜利  排名不变
    /// </summary>
    public string PVP_BattleLogVictoryDeuce = "ZDRZ_icon_sheng2";
    /// <summary>
    /// 攻击标识
    /// </summary>
    public string PVP_BattleLogCampAttack = "ZDRZ_icon_gong";
    /// <summary>
    /// 防守标志
    /// </summary>
    public string PVP_BattleLogCampDefense = "ZDRZ_icon_shou";
    /// <summary>
    /// 复仇标志
    /// </summary>
    public string PVP_BattleLogCampRevenge = "ZDRZ_icon_fu";
    /// <summary>
    /// 活动副本 关卡难度图标
    /// </summary>
    public string ActivityDiffcultIcon = "NDXZ_Icon_{0}";
    /// <summary>
    /// 活动副本 关卡难度等级
    /// </summary>
    public string ActivityDiffcultIndex = "NDXZ_Fg_{0}";

    /// <summary>
    /// 邮件 未读 没附件
    /// </summary>
    public string EMAIL_NOTREAD_NOATTACH = "YJ_icon_yjwd";
    /// <summary>
    /// 邮件 已读 没附件
    /// </summary>
    public string EMAIL_READ_NOATTACH = "YJ_icon_yjyd";
    /// <summary>
    /// 邮件 未读 有附件
    /// </summary>
    public string EMAIL_NOTREAD_ATTACH = "YJ_icon_yjwdf";
    /// <summary>
    /// 邮件 已读 有附件
    /// </summary>
    public string EMAIL_READ_ATTACH = "YJ_icon_yjydf";
    /// <summary>
    /// 邮件 系统邮件
    /// </summary>
    public string EMAIL_SYSTEM = "XTYJ_lbl_xitongyoujian";
    /// <summary>
    /// 邮件 公告邮件
    /// </summary>
    public string EMAIL_NOTICE = "XTYJ_lbl_xitongyoujian";

    /// <summary>
    /// 邮件 私聊邮件
    /// </summary>
    public string EMAIL_PRIVATE = "??";
    /// <summary>
    /// 邮件 公会邮件
    /// </summary>
    public string EMAIL_UNION = "??";
    /// <summary>
    /// 邮件 竞技邮件
    /// </summary>
    public string EMAIL_ARENA = "??";

    public string[] LivenessBoxIcons = new string[] { "Box_Icon_one", "Box_Icon_two", "Box_Icon_three", "Box_Icon_four", "Box_Icon_Five", "Box_Icon_six" };
    public string[] LivenessBoxIconsOpen = new string[] { "Box_Icon_one_open", "Box_Icon_two_open", "Box_Icon_three_open", "Box_Icon_four_open", "Box_Icon_Five_open", "Box_Icon_six_open" };


    public string MUSIC_BGOFF = "XTSZ_Bth_Off";
    public string MUSIC_BGON = "ZCJ_bg_jiaanniu";
    public string MUEN_OFF = "ZJM_btn_kuozhananniu1";
    public string MUEN_ON = "ZJM_btn_kuozhananniu";
    public string MAINCITY_UNLOCK = "ZCJ_bg_taitoukuang02";
    public string MAINCITY_LOCK = "ZCJ_bg_taitoukuang";

    public string OpenFuncHero = "";
    public string OpenFuncSlaveSlot4 = "";
    public string OpenFuncSoldier = "";
    public string OpenFuncPVE = "";
    public string OpenFuncActivity = "SJTS_Lbl_hdfb";
    public string OpenFuncEndless = "SJTS_Lbl_wjms";
    public string OpenFuncExpedition = "SJTS_Lbl_yzwf";
    public string OpenFuncArena = "SJTS_Lbl_jjc";
    public string OpenFuncPackage = "";
    public string OpenFuncStore = "SJTS_Lbl_sdxt";
    public string OpenFuncSystemSetting = "";
    public string OpenFuncChat = "";
    public string OpenFuncMail = "";
    public string OpenFuncLevelUp = "";
    public string OpenFuncExchangeGold = "";
    public string OpenFuncVIP = "";
    public string OpenFuncRecruit = "";
    public string OpenFuncContinuousLogin = "";
    public string OpenFuncActivityAnnouncement = "";
    public string OpenFuncTask = "";
    public string OpenFuncLiveness = "SJTS_Lbl_hybx";
    public string OpenFuncRank = "SJTS_Lbl_phb";
    public string OpenFuncSociaty = "SJTS_Lbl_ghxt";
    public string OpenFuncSlave = "SJTS_Lbl_nlxt";
    public string OpenFuncClear = "SJTS_Lbl_dcsd";
    public string OpenFuncBatchClear = "";
    public string OpenFuncOneKeyEnchance = "SJTS_Lbl_zbqh";
    public string OpenFuncDoubleFightSpeed = "SJTS_Lbl_zdjs";
    public string OpenFuncSlaveSlot2 = "";
    public string OpenFuncSlaveSlot3 = "";
    public string MAXK_BACK = "WJXZ_Bg_hszz";
    public string OwnSoldierSelected = "WJXZ_bg_xuanzhong";
    public string OwnSoldierDefault = "CMN_bg_tubiaodiban";

    public string ExpeditionAwardOpen = "Box_Icon_Five_open";
    public string ExpeditionAwardClose = "Box_Icon_Five";
    /// <summary>
    /// 战斗速度-1
    /// </summary>
    public string FightSpeed_1 = "HXZD_lbl_x1";
    /// <summary>
    /// 战斗速度-2
    /// </summary>
    public string FightSpeed_2 = "HXZD_lbl_x2";
    /// <summary>
    /// 战斗速度-3
    /// </summary>
    public string FightSpeed_3 = "HXZD_lbl_x3";////

    /// <summary>
    /// 未命中
    /// </summary>
    public string FightMiss = "CMN_lbl_wmz";
    /// <summary>
    /// 闪避
    /// </summary>
    public string FightShanbi = "CMN_lbl_sanbi";
    /// <summary>
    /// 护盾吸收
    /// </summary>
    public string FightShield = "CMN_lbl_xishou";
    /// <summary>
    /// 抵抗
    /// </summary>
    public string FightResist = "CMN_lbl_dk";
    /// <summary>
    /// 免疫
    /// </summary>
    public string FightImmune = "CMN_lbl_my";

    /// <summary>
    /// 进攻
    /// </summary>
    public string FightStart_1 = "ZDKS_Lbl_jgys";
    /// <summary>
    /// 守护城堡
    /// </summary>
    public string FightStart_2 = "ZDKS_Lbl_shcc";
    /// <summary>
    /// 护送
    /// </summary>
    public string FightStart_3 = "ZDKS_Lbl_hsyj";
    /// <summary>
    /// 守护传送门
    /// </summary>
    public string FightStart_4 = "ZDKS_Lbl_szrk";
    /// <summary>
    /// 无尽
    /// </summary>
    public string FightStart_5 = "ZDKS_Lbl_wjzy";
    /// <summary>
    /// 远征
    /// </summary>
    public string FightStart_6 = "ZDKS_Lbl_dng";
    /// <summary>
    /// 演舞台(PVP)
    /// </summary>
    public string FightStart_7 = "ZDKS_Lbl_dng";

    /// <summary>
    /// 攻击力
    /// </summary>
    public string Attribute_Attack = "YX_icon_gjl";
    /// <summary>
    /// 命中
    /// </summary>
    public string Attribute_AccRate = "YX_icon_mz";
    /// <summary>
    /// 移速
    /// </summary>
    public string Attribute_Speed = "YX_icon_ydsd";
    /// <summary>
    /// 闪避
    /// </summary>
    public string Attribute_DdgRate = "YX_icon_sb";
    /// <summary>
    /// 血量
    /// </summary>
    public string Attribute_HP = "YX_icon_smz";
    /// <summary>
    /// 生命恢复
    /// </summary>
    public string Attribute_HPRecovery = "YX_icon_smzhf";
    /// <summary>
    /// 魔法
    /// </summary>
    public string Attribute_MP = "YX_icon_mf";
    /// <summary>
    /// 魔法回复
    /// </summary>
    public string Attribute_MPRecovery = "YX_icon_mfhf";
    /// <summary>
    /// 召唤能量
    /// </summary>
    public string Attribute_Energy = "YX_icon_zhnl";
    /// <summary>
    /// 召唤能量回复
    /// </summary>
    public string Attribute_EnergyRecovery = "YX_icon_zhnlhf";
    /// <summary>
    /// 暴击
    /// </summary>
    public string Attribute_Crit = "YX_icon_bj";
    /// <summary>
    /// 韧性
    /// </summary>
    public string Attribute_Tenacity = "YX_icon_rx";
    /// <summary>
    /// 攻击速度
    /// </summary>
    public string Attribute_AttRate = "YX_icon_gjsd";
    /// <summary>
    /// 攻击距离
    /// </summary>
    public string Attribute_AttDistance = "YX_icon_jl";
    /// <summary>
    /// 统御力
    /// </summary>
    public string Attribute_Leadership = "YX_icon_tyl";


    public string SoldierType_Short = "WJXZ_icon_jin";
    public string SoldierType_Mid = "WJXZ_icon_zhong";
    public string SoldierType_Long = "WJXZ_icon_yuan";
    /// <summary>
    /// 士兵类型防守
    /// </summary>
    public string SoldierType_Defense = "WJXZ_icon_fang";
    /// <summary>
    /// 士兵类型辅助
    /// </summary>
    public string SoldierType_Suport = "WJXZ_icon_fu";
    /// <summary>
    /// 士兵类型攻击 
    /// </summary>
    public string SoldierType_Attack = "WJXZ_icon_gong";
    /// <summary>
    /// 士兵类型控制
    /// </summary>
    public string SoldierType_Control = "WJXZ_icon_kong";

    public string EquipType_Ring = "SQXZ_icon_jz";
    public string EquipType_Weapon = "SQXZ_icon_wq";
    public string EquipType_Necklace = "SQXZ_icon_xl";

    /// <summary>
    /// 星级图片-达到条件
    /// </summary>
    public string SPRITE_NAME_STAR_VICTORY = "Cmn_Icon_star1";
    /// <summary>
    /// 星级图片-未达条件
    /// </summary>
    public string SPRITE_NAME_STAR_FAILURE = "Cmn_Icon_star2";
    /// <summary>
    /// 结算图片-胜利
    /// </summary>
    public string SPRITE_NAME_VICTORY = "JS_Fg_shengli";
    /// <summary>
    /// 结算图片-失败
    /// </summary>
    public string SPRITE_NAME_FAILURE = "JS_Fg_shibai";
    /// <summary>
    /// 结算图片-完成
    /// </summary>
    public string SPRITE_NAME_FINAL = "JS_Fg_wanchen";
    /// <summary>
    /// 失败提示-
    /// </summary>
    public string SPRITE_NAME_HINT_PVP = "JS_Lbl_phwbh";
    /// <summary>
    /// 失败提示
    /// </summary>
    public string SPRITE_NAME_HINT_ACTIVITY = "JS_Lbl_sltsff";

    public string SweepCount_01 = "GKXX_Num_01";
    public string SweepCount_02 = "GKXX_Num_02";
    public string SweepCount_03 = "GKXX_Num_03";
    public string SweepCount_04 = "GKXX_Num_04";
    public string SweepCount_05 = "GKXX_Num_05";
    public string SweepCount_06 = "GKXX_Num_06";
    public string SweepCount_07 = "GKXX_Num_07";
    public string SweepCount_08 = "GKXX_Num_08";
    public string SweepCount_09 = "GKXX_Num_09";
    public string SweepCount_10 = "GKXX_Num_10";

    /// <summary>
    /// 战斗界面-传送门
    /// </summary>
    public string SPRITE_FIGHTVIEW_PROTL = "CMN_Icon_men";
    /// <summary>
    /// 战斗界面-城堡
    /// </summary>
    public string SPRITE_FIGHTVIEW_CASTLE = "CMN_Icon_cheng";

    public string DIALOGUE_SELF = "XSYD_Icon_JS01";

    public string DIALOGUE_GUIDER_1 = "XSYD_Icon_JS01";

    public string DIALOGUE_GUIDER_2 = "XSYD_Icon_JS02";

    public string DIALOGUE_Player_1 = "XSYD_Icon_JS04";

    public string DIALOGUE_Player_2 = "XSYD_Icon_JS03";

    //==========活动小标题 一共9个==========//
    public string GAMEACTIVITY_SINGLECHARGE = "HDYM_Lbl_title_dcfl";

    public string GAMEACTIVITY_TOTALCHARGE = "HDYM_Lbl_title_lcsl";

    public string GAMEACTIVITY_LOGINAWARD = "HDYM_Lbl_title_dlhl";

    public string GAMEACTIVITY_WELFARE = "HDYM_Lbl_title_qmfl";

    public string GAMEACTIVITY_FUND = "HDYM_Lbl_kfjj_Small";

    public string GAMEACTIVITY_BUYINGITEM = "HDYM_Lbl_title_xsqg";

    public string GAMEACTIVITY_COST = "HDYM_Lbl_title_xhhd";

    public string GAMEACTIVITY_DROPDOUBLE = "HDYM_Lbl_title_sbbl";

    public string GAMEACTIVITY_CHANGEDOUBLE = "HDYM_Lbl_title_tzcsfb";

    //========== 活动大标题 ==========//
    public string GAMEACTIVITY_SINGLECHARGE_TITLE = "HDYM_Lbl_dcfl";

    public string GAMEACTIVITY_TOTALCHARGE_TITLE = "HDYM_Lbl_lcsl";

    public string GAMEACTIVITY_LOGINAWARD_TITLE = "HDYM_Lbl_dljl";

    public string GAMEACTIVITY_WELFARE_TITLE = "HDYM_Lbl_qmfl";

    public string GAMEACTIVITY_FUND_TITLE = "HDYM_Lbl_kfjj";

    public string GAMEACTIVITY_BUYINGITEM_TITLE = "HDYM_Lbl_xsqg";

    public string GAMEACTIVITY_COST_TITLE = "HDYM_Lbl_xhsl";

    public string GAMEACTIVITY_DROPDOUBLE_TITLE = "HDYM_Lbl_sbbl";

    public string GAMEACTIVITY_CHANGEDOUBLE_TITLE = "HDYM_Lbl_tzcsfb";

    public string GAMEACTIVITY_DEFAULT_TITLE = "HDYM_Lbl_jchd";

    //========== 活动描述的小标题 ==========//
    public string GAMEACTIVITY_SINGLECHARGE_DESC = "HDYM_Lbl_dcsl_Small";

    public string GAMEACTIVITY_TOTALCHARGE_DESC = "HDYM_Lbl_title_lcsl";

    public string GAMEACTIVITY_LOGINAWARD_DESC = "HDYM_Lbl_dlhl_Small";

    public string GAMEACTIVITY_WELFARE_DESC = "HDYM_Lbl_qmfl_Small";

    public string GAMEACTIVITY_FUND_DESC = "HDYM_Lbl_kfjj_Small";

    public string GAMEACTIVITY_BUYINGITEM_DESC = "HDYM_Lbl_xgwp_Small";

    public string GAMEACTIVITY_COST_DESC = "HDYM_Lbl_xhsl_Small";

    public string GAMEACTIVITY_DROPDOUBLE_DESC = "SPT_null";

    public string GAMEACTIVITY_CHANGEDOUBLE_DESC = "SPT_null";

    public string GAMEACTIVITY_DEFAULT_DESC = "HDYM_Lbl_hdjl_Small";


    //========== 活动小背景图和ICON 4张 ==========//

    public string GAMEACTIVITY_BG_1 = "HDYM_BG_cb";

    public string GAMEACTIVITY_BG_2 = "HDYM_BG_cl";

    public string GAMEACTIVITY_BG_3 = "HDYM_BG_sl";

    public string GAMEACTIVITY_BG_4 = "HDYM_BG_xd";

    public string GAMEACTIVITY_ICON_1 = "HDYM_BG_cbr";

    public string GAMEACTIVITY_ICON_2 = "HDYM_BG_clr";

    public string GAMEACTIVITY_ICON_3 = "HDYM_BG_slr";

    public string GAMEACTIVITY_ICON_4 = "HDYM_BG_xdr";

    //========== 活动小背景框 4张 ==========//
    public string GAMEACTIVITY_FRAME_GREEN = "HDYM_BG_Green";

    public string GAMEACTIVITY_FRAME_BLUE = "HDYM_BG_Bule";

    public string GAMEACTIVITY_FRAME_RED = "HDYM_BG_Red";

    public string GAMEACTIVITY_FRAME_YELLOW = "HDYM_BG_Yellow";

    public string GAMEACTIVITY_ICON_FUND = "Icon_Item_31500001";

    //=========================任务================================//
    public string TASK_UNFINISHBG = "CMN_bg_huodongt2";

    public string TASK_FINISHBG = "CMN_bg_huodongt1";

    //=========================关卡选择================================//
    public string GTAE_INFO_BOSSBG = "GKXX_BG";
    public string GTAE_INFO_NORMALBG = "CMN_bg_tubiaodiban";
    //=========================头像边框==========================//
    public string Frame_Name_A = "Frame_{0}_A";//方框
    public string Frame_Name_B = "Frame_{0}_B";//斜框
    public string Frame_Name_C = "Frame_{0}_C";
    public string Frame_Name_D = "Frame_{0}_D";
    public string Frame_Name_E = "Frame_{0}_E";
    public string Frame_Name_F = "Frame_{0}_F";
    //=========================排行榜================================//
    public string RANK_BG1_1 = "PHB_bg_1";
    public string RANK_BG1_2 = "PHB_bg_2";
    public string RANK_BG1_3 = "PHB_bg_3";
    public string RANK_BG2_1 = "PHB_bg_jindi";
    public string RANK_BG2_2 = "PHB_bg_yindi";
    public string RANK_BG2_3 = "PHB_bg_tongdi";

    public string RANK_SPILT_YELLOW = "CMN_bg_hzhuangshitiaohuang";
    public string RANK_SPILT_DARK = "CMN_bg_hzhuangshitiao";
    public string RANK_BTN_NOTSELECT = "PHB_btn_weixuanzhong";
    public string RANK_BTN_SELECT = "PHB_btn_xuanzhong";
    public string UNIONRANK_PROGRESS_GREEN = "JTWF_fg_jindulv";
    public string UNIONRANK_PROGRESS_YELLOW = "WJSJ_fg_jindu";
    public string AUTO_FIGHT_BG_COMMON = "HXZD_btn_zidongzhandou";
    public string AUTO_FIGHT_BG_LOCK = "HXZD_btn_zidongzhandou2";
    public string AUTO_SUMMON_BG_COMMON = "HXZD_btn_quxiaozhaohuan1";
    public string AUTO_SUMMON_BG_LOCK = "HXZD_btn_zidongzhaohuan1";
    //=================================军团===============================//
    public string UNION_FIGHTWIN = "JTWJ_lbl_yjp";
    public string UNION_FIGHTLOSE = "JTWJ_lbl_sb";
    public string UNION_MIRRORING = "CMN_icon_006";


    public string GATE_CRUSADE_NORMAL = "GKXZ_Bth_pt";
    public string GATE_CRUSADE_SELECT = "GKXZ_Bth_pt_Bright";
    public string GATE_ESCORT_NORMAL = "GKXZ_Bth_yj";
    public string GATE_ESCORT_SELECT = "GKXZ_Bth_yj_Bright";
    public string GATE_ELITECRUSADE_NORMAL = "GKXZ_Bth_jy";
    public string GATE_ELITECRUSADE_SELECT = "GKXZ_Bth_jy_Bright";
    public string GATE_ELITECRUSADE_MARK = "Cmn_Lbl_jy";
    public string GATE_ESCORT_MARK = "Cmn_Lbl_hs";

    public string[] FightLoaingBgs = { "loading_bg_01", "loading_bg_02", "loading_bg_03", "loading_bg_04", "loading_bg_05" };
    public string[] MainCityLoaingBgs = { "loading_bg_01", "loading_bg_02", "loading_bg_03", "loading_bg_04", "loading_bg_05" };

    public string RecruitFree = "ZXG_Lbl_kmf";
    public string RecruitCard = "ZXG_Lbl_zml";

    public string QUESTION = "wenhao";

    public string LogoSf = "GameLogo";
    public string LogoNormal = "LOGO";
    public string Logo_BingLin = "LOGO_1";
    public string Logo_P7725 = "p7725logo";
    public string FIRST_PAY_BG = "SCLB_bg_yh";
    public string FIRST_LOGIN_REWARD_BG = "FirstLoginRewardBG";

    public string NOTICE_RED_POINT = "ZCJ_BG_xxts";
    public string NOTICE_NEW = "HDYM_Lbl_new";

    public string CaptureTokenCanClick = "GCLD_Lbl_djjh";
    public string CaptureTokenCanNotClick = "GCLD_Lbl_djjh02";
    public string CaptureRankSelectSpt = "HDYM_FG_xzhg";


    //=================================获取途径===============================//
    public string GetPath_ViewIcon_Activity = "HQTJ_Icon_bfbc";
    public string GetPath_ViewIcon_Vip = "HQTJ_Icon_vip";
    public string GetPath_ViewIcon_Escort = "HQTJ_Icon_hsyj";
    public string GetPath_ViewIcon_Elite = "HQTJ_Icon_jytf";
    public string GetPath_ViewIcon_UnionStore = "HQTJ_Icon_jtsd";
    public string GetPath_ViewIcon_MedalStore = "HQTJ_Icon_jxk";
    public string GetPath_ViewIcon_Mall = "HQTJ_Icon_sc";
    public string GetPath_ViewIcon_NormalStore = "HQTJ_Icon_sd";
    public string GetPath_ViewIcon_NormalBattle = "HQTJ_Icon_tfzl";
    public string GetPath_ViewIcon_Endless = "HQTJ_Icon_wjzy";
    public string GetPath_ViewIcon_HonorStore = "HQTJ_Icon_xzsd";
    public string GetPath_ViewIcon_Expedition = "HQTJ_Icon_yztx";

    //=================================排位赛===============================//
    public string Qualifuing_Award_Low_Close = "PWS_Icon_box_1_1";
    public string Qualifuing_Award_Low_Open = "PWS_Icon_box_1_2";
    public string Qualifuing_Award_Mid_Close = "PWS_Icon_box_2_1";
    public string Qualifuing_Award_Mid_Open = "PWS_Icon_box_2_2";
    public string Qualifuing_Award_High_Close = "PWS_Icon_box_3_1";
    public string Qualifuing_Award_High_Open = "PWS_Icon_box_3_2";


    //=================================猎魂===============================//
    public string PreyButton_White_Frame = "MHJM_Icon_bsk";
    public string PreyButton_White_Icon = "MHJM_Icon_bs";
    public string PreyButton_Green_Frame = "MHJM_Icon_lvsk";
    public string PreyButton_Green_Icon = "MHJM_Icon_lvs";
    public string PreyButton_Blue_Frame = "MHJM_Icon_lsk";
    public string PreyButton_Blue_Icon = "MHJM_Icon_ls";
    public string PreyButton_Purple_Frame = "MHJM_Icon_zsk";
    public string PreyButton_Purple_Icon = "MHJM_Icon_zs";
    public string PreyButton_Orange_Frame = "MHJM_Icon_csk";
    public string PreyButton_Orange_Icon = "MHJM_Icon_cs";
    public string PreyButton_Red_Frame = "MHJM_Icon_hsk";
    public string PreyButton_Red_Icon = "MHJM_Icon_hs";
    /// <summary>
    /// 命魂底框
    /// </summary>
    public string LIFESPIRIT_SHADING = "MHJM_bg_mhk";
    public string LIFESPIRIT_HERO_MARK = "MHJM_Icon_yx";
    public string LIFESPIRIT_SOLDIER_MARK = "MHJM_Icon_wj";


    public string PETSYSTEM_PETBG = "CWJM_bg_cwk";
}

public class ViewType
{
    public const string DIR_VIEWNAME_CHANGEUNIONBADGEVIEW = "ChangeUnionBadgeView";
    /// <summary>
    /// 军团成员信息列表
    /// </summary>
    public const string DIR_VIEWNAME_UNIONMEMBERINFO = "UnionMemberInfoView";
    /// <summary>
    /// 菜单
    /// </summary>
    public const string DIR_VIEWNAME_MENU = "MenuView";
    /// <summary>
    /// 创建角色
    /// </summary>
    public const string DIR_VIEWNAME_CREATEROLE = "CreateCharacterView";
    /// <summary>
    /// 战斗
    /// </summary>
    public const string DIR_VIEWNAME_FIGHT = "FightView";
    /// <summary>
    /// 登陆
    /// </summary>
    public const string DIR_VIEWNAME_LOGIN = "LoginView";
    /// <summary>
    /// DLC
    /// </summary>
    public const string DIR_VIEWNAME_DLC = "DLCView";
    /// <summary>
    /// 通用
    /// </summary>
    public const string DIR_VIEWNAME_COMMON = "HintView";
    /// <summary>
    /// 主城
    /// </summary>
    public const string DIR_VIEWNAME_MAINCITY = "MainCityView";
    /// <summary>
    /// 聊天信息界面
    /// </summary>
    public const string DIR_VIEWNAME_SIMPLECHAT = "SimpleChatView";
    /// <summary>
    /// 英雄
    /// </summary>
    public const string DIR_VIEWNAME_HEROATT = "HeroAttributeView";
    /// <summary>
    /// 武将
    /// </summary>
    public const string DIR_VIEWNAME_SOLDIERATT = "SoldierAttView";
    /// <summary>
    /// 套装属性界面
    /// </summary>
    public const string DIR_VIEWNAME_SUITEQUIPATT = "SuitEquipAttView";
    /// <summary>
    /// 天将神兵
    /// </summary>
    public const string DIR_VIEWNAME_SACRIFICIAL = "SacrificialSystem";
    /// <summary>
    /// 关卡
    /// </summary>
    public const string DIR_VIEWNAME_GATE = "GateView";
    /// <summary>
    /// 背包
    /// </summary>
    public const string DIR_VIEWNAME_BACKPACK = "BackPackView";
    /// <summary>
    /// 关卡信息
    /// </summary>
    public const string DIR_VIEWNAME_GATEINFO = "GateInfoView";
    /// <summary>
    /// 获取途径
    /// </summary>
    public const string DIR_VIEWNAME_GETPATH = "GetPathView";
    /// <summary>
    /// 活动副本
    /// </summary>
    public const string DIR_VIEWNAME_ACTIVITIES = "ActivitiesView";
    public const string DIR_VIEW_DROWEQUIPVIEW = "DrowEquipView";
    /// <summary>
    /// 提示框
    /// </summary>
    public const string DIR_VIEWNAME_HINT = "HintView";
    /// <summary>
    /// 无尽战场
    /// </summary>
    public const string DIR_VIEWNAME_ENDLESS = "EndlessView";
    /// <summary>
    /// 等级提示
    /// </summary>
    public const string DIR_VIEWNAME_LEVELUP = "LevelUPView";
    /// <summary>
    /// 远征天下
    /// </summary>
    public const string DIR_VIEWNAME_EXPEDITION = "ExpeditionView";
    /// <summary>
    /// 右上角功能
    /// </summary>
    public const string DIR_VIEWNAME_TOPFUNC = "TopFuncView";
    /// <summary>
    /// 远征模式关卡信息
    /// </summary>
    public const string DIR_VIEWNAME_EXPEDITIONINFO = "ExpeditionInfoView";
    /// <summary>
    /// 全服争霸霸主信息
    /// </summary>
    public const string DIR_VIEWNAME_SERVERHEGEMONYINFO = "ServerHegemonyInfoView";
    public const string DIR_VIEWNAME_RECRUITVIEW = "RecruitView";

    public const string DIR_VIEWNAME_STORE = "StoreView";
    public const string DIR_VIEWNAME_RECHARGE = "VipRechargeView";
    public const string DIR_VIEWNAME_SEEDETAIL = "SeeDetailView";
    public const string DIR_VIEWNAME_GUIDE = "GuideView";
    /// <summary>
    /// 邮件面板
    /// </summary>
    public const string DIR_VIEWNAME_MAILVIEW = "MailView";
    /// <summary>
    /// 邮件内容
    /// </summary>
    public const string DIR_VIEWNAME_MAILINFOVIEW = "MailInfoView";
    /// <summary>
    /// 批量领取面板
    /// </summary>
    public const string DIR_VIEWNAME_MAILBATCHRECIEVEVIEW = "MailBatchRecieveView";
    /// <summary>
    /// 领取成功面板 竖版
    /// </summary>
    public const string DIR_VIEWNAME_RECIEVERESLUTVERTVIEW = "RecieveResultVertView";

    public const string DIR_VIEWNAME_RECRUITRESULTVIEW = "RecruitResultView";

    public const string DIR_VIEWNAME_TASKVIEW = "TaskView";

    public const string DIR_VIEWNAME_SIGNVIEW = "SignView";
    public const string DIR_VIEWNAME_LIVENESS = "LivenessView";

    public const string DIR_VIEWNAME_ACHIEVEMENT = "AchievementView";
    /// <summary>
    /// 演舞台
    /// </summary>
    public const string DIR_VIEWNAME_PVPVIEW = "PVPView";
    /// <summary>
    /// 演舞台
    /// </summary>
    public const string DIR_VIEWNAME_PLAYERINFO = "PlayerInfoView";
    /// <summary>
    /// Effect
    /// </summary>
    public const string DIR_VIEWNAME_OPENCHESTSEFFECT = "OpenChestsEffectView";  //开启宝箱

    public const string DIR_VIEWNAME_RECRUITMENTEFFECTVIEW = "RecruitmentEffectView";//招募

    public const string DIR_VIEWNAME_TOPBUYCOINEFFECTVIEW = "TopBuyCoinEffectView";
    public const string DIR_VIEWNAME_SOLDIERSETLES = "TextEffectView";//甄选暂代
    /// <summary>
    /// 系统设置
    /// </summary>
    public const string DIR_VIEWNAME_SYSTEMSETTINGVIEW = "SystemSettingView";
    /// <summary>
    /// 代币兑换金币
    /// </summary>
    public const string DIR_VIEWNAME_EXCHANGEGOLD = "ExchangeGoldView";
    /// <summary>
    /// 出售
    /// </summary>
    public const string DIR_VIEWNAME_ITEMSELL = "ItemSellView";
    /// <summary>
    /// 神器强化
    /// </summary>
    public const string DIR_VIEWNAME_ARTIFACTINTENSIFY = "ArtifactIntensifyView";
    public const string DIR_VIEWNAME_CASTLEVIEW = "CastleView";
    ///// <summary>
    ///// 士兵装备信息
    ///// </summary>
    //public const string DIR_VIEWNAME_SOLDIEREQUIPDETAILINFOVIEW = "SoldierEquipDetailInfoView";
    /// <summary>
    /// 武将装备升星
    /// </summary>
    public const string DIR_VIEW_SOLDIEREQUIPADVANCED = "SoldierEquipAdvancedView";
    public const string DIR_VIEW_SACRIFICIALEFFECT = "SacrificialSystemEffectView";
    public const string DIR_VIEW_GATEQUIPEFFECT = "GetEquipEffectView";
    /// <summary>
    /// 装备强化
    /// </summary>
    public const string DIR_VIEW_SOLDIEREQUIPINTENSIFY = "SoldierEquipIntensifyView";

    //检查更新
    public const string DIR_VIEWNAME_CHECKVERSIONVIEW = "CheckVersionView";

    public const string DIR_VIEWNAME_GMVIEW = "GMView";
    public const string DIR_VIEWNAME_BUY_SP_VIEW = "BuySPView";
    public const string DIR_VIEWNAME_REGIST_ACCOUNT_VIEW = "RegistAccountView";
    public const string DIR_VIEWNAME_CHOOSE_SERVER_VIEW = "ChooseServerView";
    public const string DIR_VIEWNAME_BUY_COIN_VIEW = "BuyCoinView";

    //扫荡//
    public const string DIR_VIEWNAME_SWEEPRESULT = "SweepResultView";
    //无尽结算详情//
    public const string DIR_VIEWNAME_ENDLESSRESULTLIST = "EndlessResultListView";

    public const string DIR_VIEWNAME_DIALOGUE = "DialogueView";

    public const string DIR_VIEWNAME_GAMEACTIVITY = "GameActivityView";
    /// <summary>
    /// 装备、神器等详细信息
    /// </summary>
    public const string DIR_VIEWNAME_EQUIPDETAILINFO = "EquipDetailInfoView";

    /// <summary>
    /// 神器选择界面详细信息
    /// </summary>
    public const string DIR_VIEWNAME_ARTIFACTDETAILINFO = "ArtifactDetailView";

    public const string DIR_VIEWNAME_ANNCOUNCEMENT = "AnnouncementView";
    public const string DIR_VIEWNAME_CHATVIEW = "ChatView";
    public const string DIR_VIEWNAME_SOLDIERILLINFO = "SoldierIllInfoView";
    public const string DIR_VIEWNAME_SOLDIERILLVIEW = "SoldierIllView";

    public const string DIR_VIEWNAME_RANKVIEW = "RankView";
    public const string DIR_VIEWNAME_GETSPECIALITEM = "GetSpecialItemView";

    public const string DIR_VIEWNAME_JOINUNIONVIEW = "JoinUnionView";
    public const string DIR_VIEWNAME_UNIONSETTINGVIEW = "UnionSettingView";

    public const string DIR_VIEWNAME_CREATEUNIONVIEW = "CreateUnionView";
    public const string DIR_VIEWNAME_UNIONVIEW = "UnionView";

    public const string DIR_VIEWNAME_UNIONDONATIONVIEW = "UnionDonationView";
    public const string DIR_VIEWNAME_UNIONDONATIONRECVIEW = "UnionDonationRecordView";
    public const string DIR_VIEWNAME_UNIONHALLVIEW = "UnionHallView";
    public const string DIR_VIEWNAME_UNIONAPPLYVIEW = "UnionApplyView";
    public const string DIR_VIEWNAME_CHANGE_UNION_ICON_VIEW = "ChangeUnionIconView";
    public const string DIR_VIEWNAME_CHANGE_UNION_NAME_VIEW = "ChangeUnionNameView";
    public const string DIR_VIEWNAME_GUILDHEGEMONYVIEW = "GuildHegemonyView";
    public const string DIR_VIEWNAME_PREPAREBATTLEVIEW = "PrepareBattleView";
    public const string DIR_VIEWNAME_EXOTICADVANTUREVIEW = "ExoticAdvantureView";
    /// <summary>
    /// 奴役系统界面
    /// </summary>
    public const string DIR_VIEWNAME_PRISONVIEW = "PrisonView";
    public const string DIR_VIEWNAME_PRISONRULEVIEW = "PrisonRuleView";
    public const string DIR_VIEWNAME_PRISONMARKVIEW = "PrisonMarkView";
    public const string DIR_VIEWNAME_CHOOSEPRISONVIEW = "ChoosePrisonView";

    public const string DIR_VIEWNAME_EXOTICADVANTUREINFOVIEW = "ExoticAdvantureInfoView";

    public const string DIR_VIEWNAME_UNIONRANKVIEW = "UnionRankView";
    public const string DIR_VIEWNAME_FIRSTPAYVIEW = "FirstPayView";

    public const string DIR_VIEWNAME_UNIONREADINESSVIEW = "UnionReadinessView";
    public const string DIR_VIEWNAME_RULEVIEW = "RuleView";
    public const string DIR_VIEWNAME_UNIONHEGEMONYVIEW = "UnionHegemonyView";
    public const string DIR_VIEWNAME_RECHARGEWEBMASK = "RechargeWebMask";
    public const string DIR_VIEWNAME_NOVICETASKVIEW = "NoviceTaskView";
    public const string DIR_VIEWNAME_MALLVIEW = "MallView";
    public const string DIR_VIEWNAME_COMMENTVIEW = "CommentView";
    public const string DIR_VIEWNAME_WALKTHROUGHVIEW = "WalkthroughView";
    public const string DIR_VIEWNAME_FIRENDVIEW = "FriendView";
    public const string DIR_VIEWNAME_FRIENDAPPLYVIEW = "FriendApplyView";
    public const string DIR_VIEWNAME_FRIENDADDVIEW = "FriendAddView";
    public const string DIR_VIEWNAME_FRIENDINVITEVIEW = "FriendInviteView";
    public const string DIR_VIEWNAME_FUNCTIONMENUBVIEW = "FunctionMenuView";

    public const string DIR_VIEWNAME_CAPTURE_TERRITORY = "CaptureTerritoryView";
    public const string DIR_VIEWNAME_CAPTURE_TERRITORY_INFO = "CaptureTerritoryInfoView";
    public const string DIR_VIEWNAME__ALLOCATE_BOX = "AllocateBoxView";
    public const string DIR_VIEWNAME_CAPTURE_TOKEN = "CaptureTokenView";
    public const string DIR_VIEWNAME_CAPTURE_CITY_INFO = "CaptureCityInfoView";
    public const string DIR_VIEWNAME_SOLDIERPROPSPACKAGEVIEW = "SoldierPropsPackageView";
    public const string DIR_VIEWNAME_CAPTURETERRITORYCAOMPLETE = "CaptureTerritoryCompleteView";
    public const string DIR_VIEWNAME_UNIONPRISONVIEW = "UnionPrisonView";
    public const string DIR_VIEWNAME_UNIONPRISONINFOVIEW = "UnionPrisonInfoView";
    public const string DIR_VIEWNAME_UNIONPRISONCHOOSEVIEW = "UnionPrisonChooseView";
    public const string DIR_VIEWNAME_CAPTURE_TERRITORY_RULE = "CaptureTerritoryRule";
    public const string DIR_VIEWNAME_PETSYSTEM = "PetSystemView";
    public const string DIR_VIEWNAME_PETCHOOSE = "PetChooseView";
    /// <summary>
    /// 全服霸主
    /// </summary>
    public const string DIR_VIEWNAME_SUPERMACY = "SupermacyView";
    /// <summary>
    /// 排位赛
    /// </summary>
    public const string DIR_VIEWNAME_QUALIFYING = "QualifyingView";

    public const string DIR_VIEWNAME_LIFESPIRITVIW = "LifeSpiritView";

    public const string DIR_VIEWNAME_LIFESPIRITPACKVIEW = "LifeSpiritPackView";

    public const string DIR_VIEWNAME_PREYLIFESPIRITVIEW = "PreyLifeSpiritView";
    public const string DIR_VIEWNAME_LIFESPIRITINTENSIFY = "LifeSpiritIntensifyView";
    public const string DIR_VIEWNAME_RECYCLE = "RecycleView";
    public const string DIR_VIEWNAME_ADVANCETIP = "AdvanceTipView";
    /// <summary>
    /// 跨服战场
    /// </summary>
    public const string DIR_VIEWNAME_CROSSSERVERWAR = "CrossServerWarView";
}

/// <summary>
/// 音乐和音效
/// </summary>
public class SoundName
{

    private const string SOUND_NAME_BASE = "sound_{0}.assetbundle";

    //音效//
    public string AUDIO_ANNIU = string.Format(SOUND_NAME_BASE, "anniu");
    public string AUDIO_CUOWUTISHI = string.Format(SOUND_NAME_BASE, "cuowutishi");
    public string AUDIO_DAOJISHI = string.Format(SOUND_NAME_BASE, "daojishi");
    public string AUDIO_DENGLU = string.Format(SOUND_NAME_BASE, "denglu");
    public string AUDIO_FIGHT = string.Format(SOUND_NAME_BASE, "fight");
    public string AUDIO_FIRE = string.Format(SOUND_NAME_BASE, "Fire");
    public string AUDIO_HIT = string.Format(SOUND_NAME_BASE, "Hit");



    /// <summary>
    /// 主界面背景音乐
    /// </summary>
    public string MUSIC_MAIN = string.Format(SOUND_NAME_BASE, "music_main");
    /// <summary>
    /// 登录界面背景音乐
    /// </summary>
    public string MUSIC_LOGIN = string.Format(SOUND_NAME_BASE, "music_login");
    /// <summary>
    /// 角色创建背景音乐
    /// </summary>
    public string MUSIC_CREAT = string.Format(SOUND_NAME_BASE, "music_creat");
    /// <summary>
    /// PVE界面背景音乐
    /// </summary>
    public string MUSIC_PVE = string.Format(SOUND_NAME_BASE, "music_pve");
    /// <summary>
    /// 通用战斗背景音乐
    /// </summary>
    public string MUSIC_BATTLE = string.Format(SOUND_NAME_BASE, "music_battle");
    /// <summary>
    /// BOSS关战斗背景音乐
    /// </summary>
    public string MUSIC_BOSS = string.Format(SOUND_NAME_BASE, "music_boss");

    /// <summary>
    /// 打开卷轴界面
    /// </summary>
    public string AUDIO_UI_OpenL = string.Format(SOUND_NAME_BASE, "ui_open_l");
    /// <summary>
    /// 打开弹窗界面
    /// </summary>
    public string AUDIO_UI_OpenM = string.Format(SOUND_NAME_BASE, "ui_open_m");
    /// <summary>
    /// 点击普通按钮
    /// </summary>
    public string AUDIO_UI_Btn_Click = string.Format(SOUND_NAME_BASE, "ui_btn_click");
    /// <summary>
    /// 点击特殊按钮
    /// </summary>
    public string AUDIO_UI_Btn_Super = string.Format(SOUND_NAME_BASE, "ui_btn_super");
    /// <summary>
    /// 点击开战按钮
    /// </summary>
    public string AUDIO_UI_Btn_Play = string.Format(SOUND_NAME_BASE, "ui_btn_play");
    /// <summary>
    /// 点击关闭按钮
    /// </summary>
    public string AUDIO_UI_Close = string.Format(SOUND_NAME_BASE, "ui_close");
    /// <summary>
    /// 获得物品/道具/奖励提示框
    /// </summary>
    public string AUDIO_UI_Get_Reward = string.Format(SOUND_NAME_BASE, "ui_get_reward");
    /// <summary>
    /// 通用界面操作成功音效
    /// </summary>
    public string AUDIO_UI_Operate_Success = string.Format(SOUND_NAME_BASE, "ui_operation_success");
    /// <summary>
    /// 通用界面操作失败音效
    /// </summary>
    public string AUDIO_UI_Operate_Fail = string.Format(SOUND_NAME_BASE, "ui_operation_fail");
    /// <summary>
    /// 英雄升级提示
    /// </summary>
    public string AUDIO_UI_LevelUp_Hero = string.Format(SOUND_NAME_BASE, "ui_level_up_hero");
    /// <summary>
    /// 招贤馆招武将动画效果配套音效
    /// </summary>
    public string AUDIO_UI_Summon = string.Format(SOUND_NAME_BASE, "ui_summon");
    /// <summary>
    /// 开宝箱动画效果配套音效
    /// </summary>
    public string AUDIO_UI_Get_Chest = string.Format(SOUND_NAME_BASE, "ui_get_chest");
    /// <summary>
    /// 装备强化动画效果配套音效
    /// </summary>
    public string AUDIO_UI_LevelUp_Equip = string.Format(SOUND_NAME_BASE, "ui_equip_up");
    /// <summary>
    /// 武将升星动画效果配套音效
    /// </summary>
    public string AUDIO_UI_LevelUp_Quality = string.Format(SOUND_NAME_BASE, "ui_quality_up");
    /// <summary>
    /// 武将甄选动画效果配套音效
    /// </summary>
    public string AUDIO_UI_Collect_Up = string.Format(SOUND_NAME_BASE, "ui_collect_up");
    /// <summary>
    /// 武将升级动画效果配套音效
    /// </summary>
    public string AUDIO_UI_LevelUp_Soldier = string.Format(SOUND_NAME_BASE, "ui_level_up_soldier");
    /// <summary>
    /// 技能升级成功音效
    /// </summary>
    public string AUDIO_UI_LevelUp_Skill = string.Format(SOUND_NAME_BASE, "ui_skill_up");
    /// <summary>
    /// 胜利结算音效
    /// </summary>
    public string AUDIO_UI_Battle_Win = string.Format(SOUND_NAME_BASE, "ui_battle_win");
    /// <summary>
    /// 失败结算音效
    /// </summary>
    public string AUDIO_UI_Battle_Lose = string.Format(SOUND_NAME_BASE, "ui_battle_lose");
    /// <summary>
    /// 通用普通攻击音效（刀砍）
    /// </summary>
    public string AUDIO_Attack_Normal_Sword = string.Format(SOUND_NAME_BASE, "attack_normal_sword");
    /// <summary>
    /// 通用普通攻击音效（弓箭带弹道）
    /// </summary>
    public string AUDIO_Attack_Normal_Arrow = string.Format(SOUND_NAME_BASE, "attack_normal_Arrow");
    /// <summary>
    /// 通用普通攻击音效（施法）
    /// </summary>
    public string AUDIO_Attack_Normal_Magic = string.Format(SOUND_NAME_BASE, "attack_normal_magic");
    /// <summary>
    /// 通用普通受击音效
    /// </summary>
    public string AUDIO_Hit_Normal = string.Format(SOUND_NAME_BASE, "hit_normal");
    /// <summary>
    /// 通用暴击受击音效
    /// </summary>
    public string AUDIO_Hit_Power = string.Format(SOUND_NAME_BASE, "hit_power");
    /// <summary>
    /// 城楼受击音效
    /// </summary>
    public string AUDIO_Hit_Building = string.Format(SOUND_NAME_BASE, "hit_building");
    /// <summary>
    /// 火属性攻击受击音效
    /// </summary>
    public string AUDIO_Hit_Skill_fire = string.Format(SOUND_NAME_BASE, "hit_skill_fire");
    /// <summary>
    /// 闪电攻击受击音效
    /// </summary>
    public string AUDIO_Hit_Skill_lightning = string.Format(SOUND_NAME_BASE, "hit_skill_lightning");
    /// <summary>
    /// 加血命中音效
    /// </summary>
    public string AUDIO_Hit_Heal = string.Format(SOUND_NAME_BASE, "hit_heal");
    /// <summary>
    /// BUFF命中音效
    /// </summary>
    public string AUDIO_Hit_Buff = string.Format(SOUND_NAME_BASE, "hit_buff");
    /// <summary>
    /// DEBUFF命中音效
    /// </summary>
    public string AUDIO_Hit_Debuff = string.Format(SOUND_NAME_BASE, "hit_debuff");
    /// <summary>
    /// 主角男死亡音效
    /// </summary>
    public string AUDIO_Dead_Male = string.Format(SOUND_NAME_BASE, "dead_male");
    /// <summary>
    /// 主角女死亡音效
    /// </summary>
    public string AUDIO_Dead_Female = string.Format(SOUND_NAME_BASE, "dead_female");
    /// <summary>
    /// 城楼垮塌音效
    /// </summary>
    public string AUDIO_Dead_Building = string.Format(SOUND_NAME_BASE, "dead_building");
    /// <summary>
    /// BOSS死亡音效
    /// </summary>
    public string AUDIO_Dead_Boss = string.Format(SOUND_NAME_BASE, "dead_boss");
    /// <summary>
    /// 战斗胜利欢呼声
    /// </summary>
    public string AUDIO_Voice_Win = string.Format(SOUND_NAME_BASE, "voice_win");
    /// <summary>
    /// 战斗失败欢呼声
    /// </summary>
    public string AUDIO_Voice_Lose = string.Format(SOUND_NAME_BASE, "voice_lose");
    /// <summary>
    /// 结算星级音效
    /// </summary>
    public string AUDIO_Star_1 = string.Format(SOUND_NAME_BASE, "star_1");
    public string AUDIO_Star_2 = string.Format(SOUND_NAME_BASE, "star_2");
    public string AUDIO_Star_3 = string.Format(SOUND_NAME_BASE, "star_3");
    /// <summary>
    /// 默认攻击音效
    /// </summary>
    public string DEFAULT_ATT_AUDIO = string.Format(SOUND_NAME_BASE, "attack_normal_sword");
}

/// <summary>
/// Texture
/// </summary>
public class TextureName
{
    public string DIR_TEX_HEAD = "Head";
    public string DIR_TEX_FRAME = "Frame";
    public string DIR_TEX_SKILL = "Skill_{0}";
}

public class GameObjectName
{
    public string DIR_PLAYER_FIGURE = "Figure";
}

public class SystemNotificationID
{
    //通知ID= PlayerData.Instance._AccountID*100+ID 
    public int RecruitBraveID = 10;//招贤馆绿色卡牌
    public int RecruitRiotID = 11;//招贤馆蓝色卡牌
    public int DrawEquipID = 12;//缘宝阁
    public int PHPower = 13;//体力回满
    public int NoonSPID = 14;//中午推送
    public int AfternoonID = 15;//六点推送
    public int EveningID = 16;//九点推送
}
public class EffectName
{
    public string DIR_EFFECT = "Effect";
    /// <summary>
    /// 宠物已装备特效
    /// </summary>
    public string PET_EQUIP = "effect_skill_hero_guanghuan_4b.assetbundle";

    /// <summary>
    /// 受击-普通
    /// </summary>
    public string GETHIT_COMMON = "effect_general_hit.assetbundle";
    /// <summary>
    /// 受击-暴击
    /// </summary>
    public string GETHIT_CRIT = "effect_general_purple_hit.assetbundle";
    /// <summary>
    /// 受击-城堡
    /// </summary>
    public string GETHIT_CASTLE = "effect_chengloushouji.assetbundle";
    /// <summary>
    /// 城堡垮塌
    /// </summary>
    public string CASTLE_COLLAPSE = "effect_chengloudaota.assetbundle";
    /// <summary>
    /// BOSS出场
    /// </summary>
    public string BOSS_APPEAR_A = "effect_skill_boss_jituiquanchang_1.assetbundle";
    /// <summary>
    /// 传送门
    /// </summary>
    public string PORTAL = "effect_Portal.assetbundle";
    /// <summary>
    /// 护送终点
    /// </summary>
    public string ESCORT_TERMINAL = "effect_qiang.assetbundle";
    /// <summary>
    /// 护送目标显示特效
    /// </summary>
    public string ESCORT_EFFECT_NAME = "effect_skill_general_husongmubiao_4.assetbundle";
    /// <summary>
    /// BOSS显示特效
    /// </summary>
    public string BOSS_EFFECT_NAME = "effect_skill_boss_guanghuan_4.assetbundle";
}

public class HpSliderName
{
    /// <summary>
    /// 血条物件名字
    /// </summary>
    //public static string HP_OBJECT_NAME = "aloneres_RoleProgress.assetbundle";
    public static string HP_OBJECT_NAME = "RoleProgress";
    /// <summary>
    /// 红血条
    /// </summary>
    public static string HP_SPRITE_RED = "CMN_Slider_hongxuetiao";
    /// <summary>
    /// 绿血条
    /// </summary>
    public static string HP_SPRITE_GREEN = "CMN_Slider_lvxuetiao";
    /// <summary>
    /// 普通血条背景
    /// </summary>
    public static string HPBACK_COMMON = "CMN_Slider_xuetiaodi";
    /// <summary>
    /// 英雄红血条
    /// </summary>
    public static string HP_SPRITE_RED_HERO = "CMN_BG_yxxt02";
    /// <summary>
    /// 英雄绿血条
    /// </summary>
    public static string HP_SPRITE_GREEN_HERO = "CMN_BG_yxxt01";
    /// <summary>
    /// 英雄血条背景
    /// </summary>
    public static string HPBACK_HERO = "CMN_BG_yxxt";
}
/// <summary>
/// 全局系数
/// </summary>
public class GlobalCoefficient
{
    /// <summary>
    /// 单次体力购买获得体力
    /// </summary>
    public const int BuySPCount = 100;
    /// <summary>
    /// 体力恢复间隔  秒
    /// </summary>
    public const int SPRecoverTimer = 300;
    /// <summary>
    /// 关卡信息面板 BOSS放大系数
    /// </summary>
    public const float BossScale = 1.1f;
    /// <summary>
    /// 多次扫荡次数
    /// </summary>
    public const uint RepeatSweepCount = 10;
    /// <summary>
    /// 出战士兵上限
    /// </summary>
    public const int LineupSoldierLimit = 6;
    /// <summary>
    /// 军团勋章ID
    /// </summary>
    public const uint UnionTokenID = 57000000;
    /// <summary>
    /// 金币ID
    /// </summary>
    public const uint CoinID = 51000000;
    /// <summary>
    /// 钻石ID
    /// </summary>
    public const uint GemID = 52000000;
    /// <summary>
    /// 体力ID
    /// </summary>
    public const int SpID = 50000000;
    /// <summary>
    /// 荣誉ID
    /// </summary>
    public const int HonorID = 56000000;
    /// <summary>
    /// 勋章ID
    /// </summary>
    public const int MedalID = 53000000;
    /// <summary>
    /// 主角经验ID
    /// </summary>
    public const int PlayerExpID = 54000000;
    /// <summary>
    /// 竞技场积分ID
    /// </summary>
    public const int ArenaIntegralID = 53000000;
    /// <summary>
    /// 士兵经验ID
    /// </summary>
    public const int SoldierExpID = 55000000;
    /// <summary>
    /// 远征积分ID
    /// </summary>
    public const int ExpeditionIntegralID = 56000000;
    /// <summary>
    /// 活动副本一
    /// </summary>
    public const int NormalActivity = 73000000;
    /// <summary>
    /// 活动副本二
    /// </summary>
    public const int EliteActivity = 74000000;
    /// <summary>
    /// 普通无尽
    /// </summary>
    public const int NormalEndless = 71010000;
    /// <summary>
    /// 精英无尽
    /// </summary>
    public const int EliteEndless = 71020000;
    /// <summary>
    /// 英雄无尽
    /// </summary>
    public const int HeroEndless = 71030000;

    /// <summary>
    /// 扫荡卡ID
    /// </summary>
    public const int SweepCardID = 31300002;

    /// <summary>
    /// 远征士兵等级限制
    /// </summary>
    public const int ExpeditonSoldierLevel = 20;
    /// <summary>
    /// 上阵士兵兵种数量限制
    /// </summary>
    public const int CastSoldierTypeLimit = 6;
    /// <summary>
    /// 远征关卡上限
    /// </summary>
    public const int ExpeditionGateCount = 15;
    /// <summary>
    /// 扫荡等级限制
    /// </summary>
    public const int SweepLevelLimit = 20;
    /// <summary>
    /// 扫荡VIP等级限制
    /// </summary>
    public const int SingleSweepVipLimit = 1;
    /// <summary>
    /// 连续扫荡VIP等级限制
    /// </summary>
    public const int MulitSweepVipLimit = 3;
    /// <summary>
    /// 每日最大重置次数
    /// </summary>
    public const int RematchExpeditionCount = 1;
    /// <summary>
    /// PVP每天挑战次数限制
    /// </summary>
    public const int PVPSurplusCount = 200;
    /// <summary>
    /// PVP能量上限
    /// </summary>
    public const int PVPEnergyLimit = 750;
    /// <summary>
    /// 清除CD消耗
    /// </summary>
    public const int PVPCDCONSUME = 50;
    /// <summary>
    /// 战斗准备时间
    /// </summary>
    public const int PVPReadyTime = 240;
    /// <summary>
    /// 购买次数消耗
    /// </summary>
    public const int PVPPURCHASECOUNTCONSUME = 50;
    /// <summary>
    /// 装备
    /// </summary>
    public const int PVPEXCESSARTIFACTLIMIT = 6;


    /// <summary>
    /// PVP士兵上限
    /// </summary>
    public static int PVPSoldierCountLimit = 3;
    /// <summary>
    /// 关卡奖励显示个数
    /// </summary>
    public static int GateAwardsCountLimit = 7;
    /// <summary>
    /// 章节奖励星级总数
    /// </summary>
    //public static int ChapterAwardStars = 18;
    //public static int ChapterLowAwardStars = 6;
    //public static int ChapterMidAwardStars = 12;

    /// <summary>
    /// 开服基金
    /// </summary>
    public static uint FundID = 30000000;

    /// <summary>
    /// 聊天信息上限
    /// </summary>
    public static int ServerChatCountLimit = 20;

    /// <summary>
    /// 聊天信息上限
    /// </summary>
    public static int LocalChatCountLimit = 50;
    /// <summary>
    /// 购买世界聊天次数所需代币
    /// </summary>
    public static int PurchaseWorldChatCount = 4;
    /// <summary>
    /// 奴隶战消耗体力
    /// </summary>
    public static int PrisonCosumeSP = 1;
    /// <summary>
    /// 首冲礼包ID
    /// </summary>
    public static uint FirstPayGiftDroppackID = 10000;
    /// <summary>
    /// 招募卡ID
    /// </summary>
    public static uint[] RecruitItemIDList = { 31300001, 31400001, 31500001 };

    public static uint PVPRankLimit = 10000;
    /// <summary>
    /// 奴役消耗体力
    /// </summary>
    public static uint SlaveSPConsume = 1;
    /// <summary>
    /// 金币上限
    /// </summary>
    public static uint Gold_Max = int.MaxValue;
    /// <summary>
    /// 钻石上限
    /// </summary>
    public static uint Diamond_Max = int.MaxValue;

}