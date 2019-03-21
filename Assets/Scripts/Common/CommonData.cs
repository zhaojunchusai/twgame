using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using fogs.proto.msg;
using CodeStage.AntiCheat.ObscuredTypes;
//-------------------------------------------枚举----------------------------------------------------//

public enum ELocalNotificationType
{
    None = 0,
    /// <summary>
    /// 根据设备推送
    /// </summary>
    Device = 1,
    /// <summary>
    /// 根据区服推送
    /// </summary>
    Server = 2,
    /// <summary>
    /// 根据角色推送
    /// </summary>
    Role = 3,
}

public enum EServerType :byte
{
    None = 0,
    Nei249_10201 = 1,
    Nei252_10201 = 2,
    Nei251_8201 = 3,
    Nei251_9201 = 4,
    Nei251_10201 = 5,
    Nei251_18201 = 6,
    Wai_Sifu = 7,
    Wai_BeiJing = 8,
    Wai_SifuB = 9,
    Wai_QuickSDK = 10,
    Wai_7725 = 11,
    Wai_7725Test = 12,
    Wai_KoreaReal = 13,
    Wai_KoreaBeta = 14,
    Wai_GuoNei = 16,
    Wai_7725IOSTest = 17,
    Wai_JapanCP = 18,
}

public enum EDropTipType
{
    None,
    /// <summary>
    /// 仅武将超限
    /// </summary>
    Soldier,
    /// <summary>
    /// 仅物品上限 超限
    /// </summary>
    Item,
    /// <summary>
    /// 仅装备上限
    /// </summary>
    Equip,
}


/// <summary>
/// 自动战斗状态
/// </summary>
public enum EAutoFightStatus
{
    eafsLock = 0,           //锁定//
    eafsNonActive = 1,      //未激活//
    eafsActive = 2,         //全部激活//
    eafsActive_Fight = 3,   //激活战斗//
    eafsActive_Summon = 4   //激活召唤//
}

/// <summary>
/// 战斗开始界面文字类型
/// </summary>
public enum EFightStartType
{
    efstNone,
    efstAtt,
    efstDef,
    efstEscort,
    efstTranfer,
    efstEndless,
    efstExpedition,
    efstPVP,
    efstSlave,
    efstUnion,
    efstNewGuide
}

/// <summary>
/// 英雄移动类型
/// </summary>
public enum EHeroMoveType
{
    /// <summary>
    /// 没有屏蔽
    /// </summary>
    ehmtNone = 3,
    /// <summary>
    /// 只能左移
    /// </summary>
    ehmtOnlyLeft = 1,
    /// <summary>
    /// 只能右移
    /// </summary>
    ehmtOnlyRight = 2
}

/// <summary>
/// 射手状态
/// </summary>
public enum EShooterStatus
{
    /// <summary>
    /// 未解锁
    /// </summary>
    essLock = 0,
    /// <summary>
    /// 已解锁
    /// </summary>
    essUnLock = 1
}

/// <summary>
/// 战斗随机属性种类
/// </summary>
public enum ERandomFightAttributeType
{
    erfatAttack = 1,
    erfatAttRate = 2,
    erfatAttDistance = 3,
    erfatMoveSpeed = 4,
}
/// <summary>
/// Spine动画速度类型
/// </summary>
public enum ESpineSpeedType
{
    /// <summary>
    /// 技能速度
    /// </summary>
    esstSkill,
    /// <summary>
    /// 移动速度
    /// </summary>
    esstMove
}

/// <summary>
/// 角色名字颜色
/// </summary>
public enum ERoleNameQuality
{
    ernqWhite = 1,
    ernqGreen = 2,
    ernqBlue = 3,
    ernqPurple = 4,
    ernqOrange = 5,
    ernqRed = 6
}

/// <summary>
/// 普攻弹道类型
/// </summary>
public enum ETrajectoryType
{
    /// <summary>
    /// 无弹道
    /// </summary>
    ettNone = 0,
    /// <summary>
    /// 抛物线
    /// </summary>
    ettArrow = 1,
    /// <summary>
    /// 法师直线[考虑Y轴的斜线]
    /// </summary>
    ettMagic = 2,
    /// <summary>
    /// 长柄近战直线[不考虑Y轴的直线]
    /// </summary>
    ettATtack = 3
}

/// <summary>
/// 城堡图片种类
/// </summary>
public enum ECastleIconType
{
    /// <summary>
    /// 界面
    /// </summary>
    ecitUI = 0,
    /// <summary>
    /// 场景-上部
    /// </summary>
    ecitSceneTop = 1,
    /// <summary>
    /// 场景-下部
    /// </summary>
    ecitSceneDown = 2
}

/// <summary>
/// 技能触发类型
/// </summary>
public enum ETriggerType
{
    /// <summary>
    /// 被动
    /// </summary>
    ettPassive = 0,
    /// <summary>
    /// 主动-英雄
    /// </summary>
    ettHero = 1,
    /// <summary>
    /// 主动-武将 怪物
    /// </summary>
    ettOther = 2,
    /// <summary>
    /// 光环
    /// </summary>
    ettHalo = 3,
    /// <summary>
    /// 客户端被动
    /// </summary>
    ettCPassive = 4
}

/// <summary>
/// 被动技能效果类型
/// </summary>
public enum ESkillEffectType
{
    /// <summary>
    /// 攻击
    /// </summary>
    esetAttack = 101,
    /// <summary>
    /// 生命
    /// </summary>
    esetHP = 102,
    /// <summary>
    /// 命中
    /// </summary>
    esetAccuracy = 103,
    /// <summary>
    /// 闪避
    /// </summary>
    esetDodge = 104,
    /// <summary>
    /// 暴击
    /// </summary>
    esetCrit = 105,
    /// <summary>
    /// 韧性
    /// </summary>
    esetTenacity = 106,
    /// <summary>
    /// 攻击速度
    /// </summary>
    esetAttSpeed = 107,
    /// <summary>
    /// 移动速度
    /// </summary>
    esetMoveSpeed = 108,
    /// <summary>
    /// 生命恢复
    /// </summary>
    esetHPRecovery = 109,
    /// <summary>
    /// 能量上限
    /// </summary>
    esetEnergyMax = 110,
    /// <summary>
    /// 能量恢复
    /// </summary>
    esetEnergyRecovery = 111,
    /// <summary>
    /// 魔法上限
    /// </summary>
    esetMagicMax = 112,
    /// <summary>
    /// 魔法恢复
    /// </summary>
    esetMagicRecovery = 113
}

/// <summary>
/// 角色站位
/// </summary>
public enum ERoleSeat
{
    /// <summary>
    /// 进程
    /// </summary>
    ersNear = 1,
    /// <summary>
    /// 中程
    /// </summary>
    ersMiddle = 2,
    /// <summary>
    /// 远程
    /// </summary>
    ersFar = 3,
    /// <summary>
    /// 英雄
    /// </summary>
    ersHero = 10
}
public enum HurtType
{
    /// <summary>
    /// 未造成伤害
    /// </summary>
    none,
    /// <summary>
    /// 正常伤害
    /// </summary>
    Normal,
    /// <summary>
    /// 暴击伤害
    /// </summary>
    Crite,
    /// <summary>
    /// 未命中
    /// </summary>
    UnAccuracyRate,
    /// <summary>
    /// 护盾吸收
    /// </summary>
    Shield,
    /// <summary>
    /// 免疫
    /// </summary>
    Immune,
    /// <summary>
    /// 抵抗
    /// </summary>
    Resist,
}
public enum EffectPromtType
{
    none,
    /// <summary>
    /// buff类型
    /// </summary>
    Buff,
    /// <summary>
    /// debuff类型
    /// </summary>
    DeBuff
}
public enum ECommodityState
{
    Onsell = 0,
    HasBought = 1,
}

public enum GuideStepType
{
    None = 0,
    Dialogue = 1,
    Desc = 2,
    ActionGuide = 3,
    Arrow = 4,
    Finger = 5,
    Mask = 6,
    ScreenEffect = 7,
    MainCityLocation = 8,
    Close = 9,
    HighLight = 10,
    PauseFighting = 11,
    /// <summary>
    /// 特殊战斗漫画
    /// </summary>
    SpecialFightMovie = 12,
    /// <summary>
    /// 执行特殊战斗
    /// </summary>
    SpecialFight = 13,
    /// <summary>
    /// 限制移动方向
    /// </summary>
    SpecialFightLimitMoveDir = 14,
    /// <summary>
    /// 设置特殊战斗技能状态可用
    /// </summary>
    SpecialFightSetSkillCanUse = 15,
    /// <summary>
    /// 播放声音
    /// </summary>
    PlaySound = 16,
    /// <summary>
    /// 送礼
    /// </summary>
    GateGift = 17,
    /// <summary>
    /// 战斗播放对话
    /// </summary>
    FightTalk = 18,
    /// <summary>
    /// 战斗产出士兵
    /// </summary>
    ShowFightSoldier = 19,
    /// <summary>
    /// 背景碰撞
    /// </summary>
    Collider = 20,
    /// <summary>
    /// 主界面指向建筑物
    /// </summary>
    MainCityClickBuilding = 21,
    /// <summary>
    /// 招募新手模式
    /// </summary>
    RecruitGuideMode = 22,
    /// <summary>
    /// 招募限制左划
    /// </summary>
    RecruitCloseLeft = 23,
    /// <summary>
    /// 招募限制右划
    /// </summary>
    RecruitCloseRight = 24,
    /// <summary>
    /// 展开菜单栏
    /// </summary>
    OpenMenuTool = 25,
}

public enum GuideMaskType
{
    None,
    Rect,
    Circle
}


public enum GuideLimitType
{
    None = 0,
    /// <summary>
    /// 玩家等级
    /// </summary>
    CharLevel = 1,
    /// <summary>
    /// 需要前置引导完成
    /// </summary>
    NeedPreGuide = 2,
    /// <summary>
    /// 通关关卡
    /// </summary>
    PassLevel = 3,
    /// <summary>
    /// 前置关卡且神器第一格有武器（参数填关卡编号）
    /// </summary>
    PassLevelAndFirstGodWeaponEquip = 4,
    /// <summary>
    /// 前置关卡且神器第一格没武器（参数填关卡编号）
    /// </summary>
    PassLevelAndFirstGodWeaponEquipNot = 5,
    /// <summary>
    /// 领取任务奖励成功（参数填任务编号）
    /// </summary>
    GetTastAward = 6,
    /// <summary>
    /// 选择指定关卡
    /// </summary>
    ChooseLevelWithID = 7,
    /// <summary>
    /// 选择指定关卡并装备21610003
    /// </summary>
    ChooseLevelWithIDEquip21610003 = 8,
    /// <summary>
    /// 选择指定关卡没装备21610003
    /// </summary>
    ChooseLevelWithIDNoEquip21610003 = 9,
    /// <summary>
    /// 进入指定ID的关卡
    /// </summary>
    EnterLevelWithID = 10,
}

/// <summary>
/// 暂停类型
/// </summary>
public enum EFightPauseType
{
    /// <summary>
    /// 暂停
    /// </summary>
    efptNormal = 1,
    /// <summary>
    /// 暂停-开启召唤
    /// </summary>
    efptNoSol = 2,
    /// <summary>
    /// 暂停-开启技能
    /// </summary>
    efptNoSkill = 3,
    /// <summary>
    /// 暂停-自动战斗
    /// </summary>
    efptAutoFight = 4,
    /// <summary>
    /// 不暂停
    /// </summary>
    noPause = 5
}

public enum GuideTrigger
{
    None,
    /// <summary>
    /// 打开主界面
    /// </summary>
    OpenMainCity = 1,
    /// <summary>
    /// 对话关闭
    /// </summary>
    DialogueClose = 2,
    /// <summary>
    /// 打开招募
    /// </summary>
    OpenRecruit = 3,
    /// <summary>
    /// 打开招募二级面板
    /// </summary>
    OpenRecruitSecond = 4,
    /// <summary>
    /// 招募结果面板
    /// </summary>
    OpenRecruitResult = 5,
    /// <summary>
    /// 关闭招募结果
    /// </summary>
    CloseRecruitResult = 6,
    /// <summary>
    /// 打开剧情战役界面
    /// </summary>
    OpenGateView = 7,
    /// <summary>
    /// 关卡信息界面
    /// </summary>
    OpenLevelInfoView = 8,
    /// <summary>
    /// 进入武将选择界面
    /// </summary>
    OpenSoldierChoose = 9,
    /// <summary>
    /// 选择上阵武将
    /// </summary>
    ChoosedSoldier = 10,
    /// <summary>
    /// 选择神器界面
    /// </summary>
    OpenGodWeaponChoose = 11,
    /// <summary>
    /// 选择神器后
    /// </summary>
    ChoosedGodWeapon = 12,
    /// <summary>
    /// 进入战场
    /// </summary>
    EnterFighting = 13,
    /// <summary>
    /// 点击引导
    /// </summary>
    ClickGuideMask = 14,
    /// <summary>
    /// 战斗结束
    /// </summary>
    FightOver = 15,
    /// <summary>
    /// 关闭升级提示窗口
    /// </summary>
    CloseLevelUpView = 16,
    /// <summary>
    /// 关闭胜利结算界面
    /// </summary>
    CloseGameSettlement = 17,
    /// <summary>
    /// 打开任务界面
    /// </summary>
    OpenTaskView = 18,
    /// <summary>
    /// 领取任务奖励
    /// </summary>
    GetReward = 19,
    /// <summary>
    /// 关闭领取成功窗口
    /// </summary>
    CloseRewardView = 20,
    /// <summary>
    /// 展开按钮
    /// </summary>
    ClickMenuBtn = 21,
    /// <summary>
    /// 进入英雄界面
    /// </summary>
    OpenHeroView = 22,
    /// <summary>
    /// 打开神器详细窗口
    /// </summary>
    OpenGodWeaponDesc = 23,
    /// <summary>
    /// 打开神器强化窗口
    /// </summary>
    OpenGodWeaponStrengthen = 24,
    /// <summary>
    /// 强化成功后
    /// </summary>
    GodWeaponStrengthenSucceed = 25,
    /// <summary>
    /// 进入城堡界面
    /// </summary>
    OpenCastleView = 26, //-----
    /// <summary>
    /// 进入武将界面
    /// </summary>
    OpenSoldierView = 27,
    /// <summary>
    /// 打开武将升级窗口
    /// </summary>
    OpenSoldierLevelUp = 28,
    /// <summary>
    /// 打开武将升星窗口
    /// </summary>
    OpenSoldierStarUp = 29,
    /// <summary>
    /// 进入模式为攻击城堡的战场
    /// </summary>
    FightingAttackCastle = 30,
    /// <summary>
    /// 进入模式为守护传送门的战场
    /// </summary>
    FightingProtectDoor = 31,
    /// <summary>
    /// 进入模式为护送NPC的战场
    /// </summary>
    FightingProtectNpc = 32,
    /// <summary>
    /// 主界面定位完成
    /// </summary>
    MainCityLocCompleted = 33,
    /// <summary>
    /// 粮食回复满75点
    /// </summary>
    FightingFoodNum75 = 34,
    /// <summary>
    /// 粮食回复满30点
    /// </summary>
    FightingFoodNum30 = 35,
    /// <summary>
    /// 精神回复满90点
    /// </summary>
    FightingMagicNum90 = 36,
    /// <summary>
    /// 战斗招兵
    /// </summary>
    FightingCallSoldier = 37,
    /// <summary>
    /// 战斗放技能
    /// </summary>
    FightingUseSkill = 38,
    /// <summary>
    /// 关闭神器详情
    /// </summary>
    CloseGodWeaponDetail = 39,
    /// <summary>
    /// 打开武将更换装备窗口
    /// </summary>
    OpenSoldierChangeEquip = 40,
    /// <summary>
    /// 武将装备穿戴成功
    /// </summary>
    SoldierEquipSucceed = 41,
    /// <summary>
    /// 打开武将装备详情
    /// </summary>
    OpenSoldierEquipDetail = 42,
    /// <summary>
    /// 打开武将装备强化
    /// </summary>
    OpenSoldierEquipStrengthenDetail = 43,
    /// <summary>
    /// 武将装备强化成功
    /// </summary>
    SoldierEquipStrengthenSucceed = 44,
    /// <summary>
    /// 关闭装备详细窗口
    /// </summary>
    CloseSoldierEquipDetail = 45,
    /// <summary>
    /// 武将升级到MAX
    /// </summary>
    SoldierLevelMax = 46,
    /// <summary>
    /// 关闭武将升级
    /// </summary>
    CloseSoldierLevelUp = 47,
    /// <summary>
    /// 城堡被摧毁
    /// </summary>
    CastleBeenDestory = 48,
    /// <summary>
    /// 打开神器装备更换窗口
    /// </summary>
    OpenGodWeaponChange = 49,
    /// <summary>
    /// 神器装备成功
    /// </summary>
    GodWeaponEquipSucceed = 50,
    /// <summary>
    /// 打开英雄技能页签
    /// </summary>
    OpenHeroSkillTab = 51,
    /// <summary>
    /// 打开英雄技能强化界面
    /// </summary>
    OpenHeroSkillDetail = 52,
    /// <summary>
    /// 技能强化成功
    /// </summary>
    HeroSkillUpSucceed = 53,
    /// <summary>
    /// 打开武将不足提示
    /// </summary>
    OpenNoSoldierToFight = 54,
    /// <summary>
    /// 新手引导关卡下一关按钮
    /// </summary>
    ClickNewGuideStageNext = 55,
    /// <summary>
    /// 进入新手引导战斗
    /// </summary>
    EnterNewGuideFight = 56,
    /// <summary>
    /// 特殊战斗未完成 -
    /// </summary>
    NeedSpecialFight = 57,
    /// <summary>
    /// 执行特殊战斗可以释放技能 -
    /// </summary>
    SpecialFightUseSkill = 58,
    /// <summary>
    /// 特殊战斗中移动到指定地点完成 -
    /// </summary>
    MoveDirCompleted = 59,
    /// <summary>
    /// 关闭武将装备强化窗口 -
    /// </summary>
    CloseSoldierEquipStrengthen = 60,
    /// <summary>
    /// 选择材料武将成功 -
    /// </summary>
    ChooseLvUpMatSoldierSucceed = 61,
    /// <summary>
    /// 武将升级成功 -
    /// </summary>
    SoldierLvUpSucceed = 62,
    /// <summary>
    /// 打开武将技能页签 -
    /// </summary>
    OpenSoldierSkillPage = 63,
    /// <summary>
    /// 打开武将技能强化 -
    /// </summary>
    OpenSoldierSkillStrengthen = 64,
    /// <summary>
    /// 武将技能强化成功 -
    /// </summary>
    SoldierSkillStrengthenSucceed = 65,
    /// <summary>
    /// 进入连续登陆界面 -
    /// </summary>
    OpenSignView = 66,
    /// <summary>
    /// 关闭神器强化窗口 -
    /// </summary>
    CloseGodWeaponStrengthen = 67,
    /// <summary>
    /// 特殊战斗普攻干死小兵 -
    /// </summary>
    SpecialFightNorAtkSoldier = 68,
    /// <summary>
    /// 关闭武将技能强化窗口 -
    /// </summary>
    CloseSoldierSkillLvUp = 69,
    /// <summary>
    /// 关闭英雄技能强化窗口
    /// </summary>
    CloseHeroSkillLvUp = 70,
    /// <summary>
    /// 英雄血过半
    /// </summary>
    HeroBlood60 = 71,
    /// <summary>
    /// 武将选择界面下将
    /// </summary>
    SoldierCancel = 72,
    /// <summary>
    /// 武将召唤CD到
    /// </summary>
    SoldierCallCD = 73,
    /// <summary>
    /// 播放特殊武将获得动画
    /// </summary>
    StartGetSpecialSoldier = 74,
    /// <summary>
    /// 关闭特殊武将获得动画
    /// </summary>
    CloseGetSpecialSoldier = 75,
    /// <summary>
    /// 播放特殊道具获得动画
    /// </summary>
    StartGetSpecialProp = 76,
    /// <summary>
    /// 场景对话完毕
    /// </summary>
    FightSceneTalkFinish = 77,
    /// <summary>
    /// 战斗产出士兵
    /// </summary>
    ShowFightSoldierFinish = 78,
    /// <summary>
    /// 进入商店
    /// </summary>
    OpenStoreView = 79,
    /// <summary>
    /// 进入兵伐不臣界面
    /// </summary>
    OpenFightBadMinisterView = 80,
    /// <summary>
    /// 进入活跃宝箱界面
    /// </summary>
    OpenLivenessView = 81,
    /// <summary>
    /// 进入排行榜界面
    /// </summary>
    OpenRankView = 82,
    /// <summary>
    /// 进入无尽战场界面
    /// </summary>
    OpenEndlessView = 83,
    /// <summary>
    /// 进入奴隶界面
    /// </summary>
    OpenSlaveView = 84,
    /// <summary>
    /// 进入远征天下界面
    /// </summary>
    OpenExpeditionView = 85,
    /// <summary>
    /// 进入加入军团界面
    /// </summary>
    OpenJoinUnionView = 86,
    /// <summary>
    /// 进入演舞台界面
    /// </summary>
    OpenPVPView = 87,
    /// <summary>
    /// 进入演舞台防守武将选择界面
    /// </summary>
    OpenPVPSelectSoldierView = 88,
    /// <summary>
    /// 演舞台防守上阵武将成功
    /// </summary>
    PVPSelectSoldierSucceed = 89,
    /// <summary>
    /// 进入演舞台防守神器选择界面
    /// </summary>
    OpenPVPSelectGodWeaponView = 90,
    /// <summary>
    /// 演舞台防守上阵神器成功
    /// </summary>
    PVPSelectGodWeaponSucceed = 91,
    /// <summary>
    /// 演舞台防守阵容保存成功
    /// </summary>
    PVPSaveTeamSucceed = 92,
    /// <summary>
    /// 进入护送玩法界面
    /// </summary>
    EnterProtectCityView = 93,
    /// <summary>
    /// 进入攻城玩法界面
    /// </summary>
    OpenAttackCityView = 94,
    /// <summary>
    /// 招募滑动
    /// </summary>
    RecruitDrag = 95,
    /// <summary>
    /// 打开武将升星界面
    /// </summary>
    OpenSoldierEquipUpStar = 96,
    /// <summary>
    /// 点击自动战斗
    /// </summary>
    ClickAutoFight = 97,
    /// <summary>
    /// 打开商城窗口
    /// </summary>
    OpenMall = 98,
    /// <summary>
    /// 开启自动召唤
    /// </summary>
    ClickAutoCall = 99,
    /// <summary>
    /// 展开主界面菜单条
    /// </summary>
    MainCityMenuOpen = 100,
    /// <summary>
    /// 收起主界面菜单条
    /// </summary>
    MainCityMenuClose = 101,
    /// <summary>
    /// 打开排位赛界面
    /// </summary>
    OpenQualifyingView = 102,
    /// <summary>
    /// 打开命魂界面
    /// </summary>
    OpenLifeSoulView = 103,
    /// <summary>
    /// 退出命魂界面
    /// </summary>
    CloseLifeSoulView = 104,
    /// <summary>
    /// 打开猎魂界面
    /// </summary>
    OpenHuntSoulView = 105,
    /// <summary>
    /// 退出猎魂界面
    /// </summary>
    CloseHuntSoulView = 106,
    /// <summary>
    /// 点击猎魂按钮
    /// </summary>
    ClickHuntSoulBtn = 107,
    /// <summary>
    /// 拾取命魂成功
    /// </summary>
    PickSoulSuccess = 108,
    /// <summary>
    /// 打开更换命魂窗口
    /// </summary>
    OpenChangeSoul = 109,
    /// <summary>
    /// 装备命魂成功
    /// </summary>
    EquipSoulSuccess = 110,
    /// <summary>
    /// 进入宠物系统界面
    /// </summary>
    OpenPetSystemView = 111,
    /// <summary>
    /// 打开战备宠物窗口
    /// </summary>
    OpenChoosePetView = 112,
    /// <summary>
    /// 展开菜单栏完成
    /// </summary>
    OpenMenuToolCompleted = 113,
}

public class UpgradeCostInfo 
{
    public int level;
    public int exp;
}

/// <summary>
/// 关卡刷新怪物信息
/// </summary>
public class RefreshMonsterInfo
{
    /// <summary>
    /// 刷新时间[秒]
    /// </summary>
    public float RefreshTime;
    /// <summary>
    /// 怪物数组
    /// </summary>
    public uint[] ArrMonsterID;
    /// <summary>
    /// 数组下标
    /// </summary>
    private int indexArray;
    /// <summary>
    /// 初始X坐标
    /// </summary>
    public int initPosX;

    public RefreshMonsterInfo(float vTime, int vArrLength, int vInitPosX)
    {
        RefreshTime = vTime;
        ArrMonsterID = new uint[vArrLength];
        indexArray = 0;
        initPosX = vInitPosX;
    }

    public void AddSingleID(uint vMonsterID)
    {
        if (indexArray >= ArrMonsterID.Length)
            return;
        ArrMonsterID[indexArray] = vMonsterID;
        indexArray += 1;
    }

}

public class MoneyFlowData
{
    public ECurrencyType Type;
    public int Number;

    public MoneyFlowData()
    {
        Type = ECurrencyType.None;
        Number = 0;
    }

    public void Reset()
    {
        Type = ECurrencyType.None;
        Number = 0;
    }

    public void CopyValue(MoneyFlowData data)
    {
        if (data == null)
            return;
        Type = data.Type;
        Number = data.Number;
    }
}

/// <summary>
/// 角色行为状态
/// </summary>
public enum EActionStatus
{
    /// <summary>
    /// 行为状态-初始化 销毁 停止
    /// </summary>
    easNone,
    /// <summary>
    /// 行为状态-一般
    /// </summary>
    easNormal,
    /// <summary>
    /// 行为状态-暂停
    /// </summary>
    easPause,
    /// <summary>
    /// 行为状态-AI
    /// </summary>
    easAI
}

/// <summary>
/// 英雄角色性别
/// </summary>
public enum EHeroGender : byte
{
    /// <summary>
    /// 女
    /// </summary>
    ehgFamale = 2,
    /// <summary>
    /// 男
    /// </summary>
    ehgMale = 1,
    /// <summary>
    /// 读取PlayerData数据
    /// </summary>
    ehgNone = 0,
}

/// <summary>
/// 背包物品种类
/// </summary>
public enum EGoodsType
{
    egtItem,    //道具//
    egtEquip,   //装备//
    egtMaterial //材料//
}

/// <summary>
/// 角色状态
/// </summary>
public enum ERoleState
{
    ersIdle,
    ersMove,
    ersFire,
    ersDeath
}

/// <summary>
/// 方向
/// </summary>
public enum ERoleDirection : byte
{
    erdNone = 0,
    erdRight,
    erdLeft
}

/// <summary>
/// 战斗阵营
/// </summary>
public enum EFightCamp : byte
{
    /// <summary>
    /// 战斗阵营-无
    /// </summary>
    efcNone = 0,
    /// <summary>
    /// 战斗阵营-己方
    /// </summary>
    efcSelf,
    /// <summary>
    /// 战斗阵营-敌方
    /// </summary>
    efcEnemy
}

//角色种类//
public enum ERoleType : byte
{
    /// <summary>
    /// 英雄
    /// </summary>
    ertHero,
    /// <summary>
    /// 士兵
    /// </summary>
    ertSoldier,
    /// <summary>
    /// 城堡射手
    /// </summary>
    ertShooter,
    /// <summary>
    /// 怪物
    /// </summary>
    ertMonster,
    /// <summary>
    /// 护送目标
    /// </summary>
    ertEscort,
    /// <summary>
    /// 传送门
    /// </summary>
    ertTransfer,
    /// <summary>
    /// 城堡
    /// </summary>
    ertBarracks,
    /// <summary>
    /// 召唤的宠物
    /// </summary>
    ertPet,
}

/// <summary>
/// 战斗场景类型
/// </summary>
public enum EFightType : byte
{
    None = 0,
    /// <summary>
    /// 主线副本1
    /// </summary>
    eftMain = 1,
    /// <summary>
    /// 活动2
    /// </summary>
    eftActivity = 2,
    /// <summary>
    /// 无尽3
    /// </summary>
    eftEndless = 3,
    /// <summary>
    /// 远征4
    /// </summary>
    eftExpedition,
    /// <summary>
    /// PVP5
    /// </summary>
    eftPVP,
    /// <summary>
    /// 界面6
    /// </summary>
    eftUI,
    /// <summary>
    /// 新手引导9
    /// </summary>
    eftNewGuide = 9,
    /// <summary>
    /// 奴隶10
    /// </summary>
    eftSlave,
    /// <summary>
    /// 军团11
    /// </summary>
    eftUnion,
    /// <summary>
    /// 群雄争霸12
    /// </summary>
    eftHegemony,
    /// <summary>
    /// 攻城略地13
    /// </summary>
    eftCaptureTerritory,
    /// <summary>
    /// 全服霸主14
    /// </summary>
    eftServerHegemony,
    /// <summary>
    /// 排位赛
    /// </summary>
    eftQualifying,
    /// <summary>
    /// 好友对战
    /// </summary>
    eftFriend,
    /// <summary>
    /// 跨服战场17
    /// </summary>
    eftCrossServerWar
}

/// <summary>
/// 主线副本战斗模式
/// </summary>
public enum MainBattleType
{
    /// <summary>
    /// 讨伐之战
    /// </summary>
    Crusade = 0,
    /// <summary>
    /// 护送模式
    /// </summary>
    Escort = 2,
    /// <summary>
    /// 精英护送
    /// </summary>
    EliteCrusade = 1,
}

/// <summary>
/// 角色行为种类
/// </summary>
public enum ERoleAction : byte
{
    /// <summary>
    /// 无行为操作[非法]
    /// </summary>
    eraNone = 0,
    /// <summary>
    /// 普通待机状态[没有任何其它动画 适用于等待 UI演示]
    /// </summary>
    eraIdle,
    /// <summary>
    /// AI待机[加入寻找敌人判断]
    /// </summary>
    eraAIIdle,
    /// <summary>
    /// 控制移动[适用于英雄移动操作]
    /// </summary>
    eraMove,
    /// <summary>
    /// AI移动[自动移动并带有寻找敌人功能 不能操作]
    /// </summary>
    eraAIMove,
    /// <summary>
    /// 控制战斗[适用于英雄战斗操作]
    /// </summary>
    eraFire,
    /// <summary>
    /// AI战斗[自动战斗并带有寻找敌人功能 不能操作]
    /// </summary>
    eraAIFire,
    /// <summary>
    /// 死亡
    /// </summary>
    eraDeath,
    /// <summary>
    /// 战斗结束-失败
    /// </summary>
    eraFailure,
    /// <summary>
    /// 战斗结束-胜利
    /// </summary>
    eraVictory
}

/// <summary>
/// PVE战斗场景类型
/// </summary>
public enum EPVESceneType : byte
{
    /// <summary>
    /// 进攻
    /// </summary>
    epvestAttack = 1,
    /// <summary>
    /// 防守
    /// </summary>
    epvestDefen = 2,
    /// <summary>
    /// 护送
    /// </summary>
    epvestEscort = 3,
    /// <summary>
    /// 传送
    /// </summary>
    epvestTransfer = 4
}

/// <summary>
/// PVE斗结束条件
/// </summary>
public enum EPVEFinishStatus
{
    /// <summary>
    /// 英雄死亡
    /// </summary>
    epvefsDieHero,
    /// <summary>
    /// 己方基地被摧毁
    /// </summary>
    epvefsDieBarracksSelf,
    /// <summary>
    /// 敌方基地被摧毁
    /// </summary>
    epvefsDieBarracksEnemy,
    /// <summary>
    /// 护送目标死亡
    /// </summary>
    epvefsDieEscort,
    /// <summary>
    /// 护送目标到终点
    /// </summary>
    epvefsOutEscort,
    /// <summary>
    /// 传送目标销毁
    /// </summary>
    epvefsDieTransfer,
    /// <summary>
    /// 关卡Boss死亡
    /// </summary>
    epvefsDisBoss,
    /// <summary>
    /// 关卡怪物死亡
    /// </summary>
    epvefsDieMonster,
    /// <summary>
    /// 士兵死亡
    /// </summary>
    epvefsDieSoldier,
    /// <summary>
    /// 停止战斗
    /// </summary>
    epvefsStop,
    /// <summary>
    /// 敌方英雄死亡
    /// </summary>
    epvefsDisEnemyHero,
    /// <summary>
    /// 敌方士兵死亡
    /// </summary>
    epvefsDisEnemySoldier,
    /// <summary>
    /// 超出战斗时间
    /// </summary>
    epvefsOutTime
}

/// <summary>
/// 场景目标物件类型
/// </summary>
public enum ESceneMarkType : byte
{
    /// <summary>
    /// 己方基地
    /// </summary>
    esmtBarracks_Self = 0,
    /// <summary>
    /// 敌方基地
    /// </summary>
    esmtBarracks_Enemy,
    /// <summary>
    /// 传送门
    /// </summary>
    esmtTransfer,
    /// <summary>
    /// 护送目标
    /// </summary>
    esmtEscort
}

/// <summary>
/// 场景状态
/// </summary>
public enum ESceneStatus : byte
{
    /// <summary>
    /// 异常
    /// </summary>
    essNone = 0,
    /// <summary>
    /// 胜利
    /// </summary>
    essVictory = 1,
    /// <summary>
    /// 失败
    /// </summary>
    essFailure = 2,
    /// <summary>
    /// 正常
    /// </summary>
    essNormal = 3
}

public enum ECurrencyType
{
    /// <summary>
    /// 无
    /// </summary>
    None = 0,
    /// <summary>
    /// 金币
    /// </summary>
    Gold = 1,
    /// <summary>
    /// 钻石
    /// </summary>
    Diamond = 2,
    /// <summary>
    /// 勋章
    /// </summary>
    Medal = 3,
    /// <summary>
    /// 战功
    /// </summary>
    Honor = 4,
    /// <summary>
    /// UnionToken
    /// </summary>
    UnionToken = 5,
    /// <summary>
    /// 造化之源
    /// </summary>
    RecycleCoin = 6,
    /// <summary>
    /// 坚城令
    /// </summary>
    CityToken = 7,
}

/// <summary>
/// 战斗召唤技能状态
/// </summary>
public enum EFightViewSkillStatus
{
    /// <summary>
    /// 无状态
    /// </summary>
    eskNone,
    /// <summary>
    /// 普通状态
    /// </summary>
    eskNormal,
    /// <summary>
    /// 暂停状态
    /// </summary>
    eskPause,
    /// <summary>
    /// CD状态
    /// </summary>
    eskCD,
    /// <summary>
    /// 锁定状态
    /// </summary>
    eskLock
}

/// <summary>
/// 战斗召唤士兵状态
/// </summary>
public enum EFightViewSoldierStatus
{
    /// <summary>
    /// 无状态
    /// </summary>
    esoNone,
    /// <summary>
    /// 普通状态
    /// </summary>
    esoNormal,
    /// <summary>
    /// 暂停状态
    /// </summary>
    esoPause,
    /// <summary>
    /// CD状态
    /// </summary>
    esoCD,
    /// <summary>
    /// 锁定状态
    /// </summary>
    esoLock
}

public enum ESoldierType
{
    None = 0,
    ///// <summary>
    ///// 近程
    ///// </summary>
    //ShortRange = 1,
    ///// <summary>
    ///// 中程
    ///// </summary>
    //MidRange = 2,
    ///// <summary>
    ///// 远程
    ///// </summary>
    //LongRange = 3,


    /// <summary>
    /// 装备类型攻击 
    /// </summary>
    Attack = 1,
    /// <summary>
    /// 装备类型防守
    /// </summary>
    Defense = 2,
    /// <summary>
    /// 装备类型辅助
    /// </summary>
    Suport = 3,
    /// <summary>
    /// 装备类型控制
    /// </summary>
    Control = 4,

}


//-------------------------------------------结构----------------------------------------------------//

//-------------------------------------------类----------------------------------------------------//
/// <summary>
/// 扣除魔法值
/// </summary>
public class FightDecMP
{
    public EFightCamp mFightCamp;
    public int mMPChangeValue;
    public FightDecMP(EFightCamp vFightCamp, int vMPChangeValue)
    {
        mFightCamp = vFightCamp;
        mMPChangeValue = vMPChangeValue;
    }
}

/// <summary>
/// 战斗力加成系数
/// </summary>
public class Attribute_PlusCoefficient
{
    public float PlusCoeff_N1;
    public float PlusCoeff_N2;

    public Attribute_PlusCoefficient()
    {
        PlusCoeff_N1 = 0;
        PlusCoeff_N2 = 1;
    }
}

/// <summary>
/// 战斗力计算装备 技能信息
/// </summary>
public class CalBaseData
{
    public uint ID;
    public int Level;
    public CalBaseData(uint vID, int vLevel)
    {
        ID = vID;
        Level = vLevel;
    }
}
/// <summary>
/// 使用技能数据
/// </summary>
public class UseSkillInfo
{
    /// <summary>
    /// 使用技能ID
    /// </summary>
    public Weapon SkillEquip;
    /// <summary>
    /// 角色阵营
    /// </summary>
    public EFightCamp RoleCamp;

    public UseSkillInfo(Weapon vSkillEquip, EFightCamp vCamp)
    {
        SkillEquip = vSkillEquip;
        RoleCamp = vCamp;
    }
}

/// <summary>
/// 英雄技能数据
/// </summary>
public class SkillBaseData
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 技能图标
    /// </summary>
    public string icon;
    /// <summary>
    /// 技能消耗
    /// </summary>
    public int _Magic;
    /// <summary>
    /// 技能等级
    /// </summary>
    public int level;
    /// <summary>
    /// CD时间
    /// </summary>
    public float _CDTime;
    /// <summary>
    /// 装备ID
    /// </summary>
    public Weapon Equip;

    public SkillBaseData(uint vID, string vIcon, int vMagic, int vLevel, float vCDTime, Weapon vEquip = null)
    {
        id = vID;
        icon = vIcon;
        _Magic = vMagic;
        level = vLevel;
        _CDTime = vCDTime;
        Equip = vEquip;
    }
    public void CacheEffect()
    {
        if (this.Equip != null)
        {
            EquipAttributeInfo tmpAtt = this.Equip.Att;
            if (!tmpAtt.EffectName.Equals("0") && !string.IsNullOrEmpty(tmpAtt.EffectName))
            {
                EffectObjectCache.Instance.LoadGameObject(tmpAtt.EffectName, (go) =>
                {
                    EffectObjectCache.Instance.FreeObject(go);
                });
            }
        }

        SkillAttributeInfo Att = ConfigManager.Instance.mSkillAttData.FindById(this.id);
        if (Att == null)
            return;

        if (!Att.conjure.Equals("0") && !Att.conjure.Equals(string.Empty))
        {
            EffectObjectCache.Instance.LoadGameObject(Att.conjure, (go) =>
            {
                EffectObjectCache.Instance.FreeObject(go);
            });
        }
        if (!Att.trajectory.Equals("0") && !Att.trajectory.Equals(string.Empty))
        {
            EffectObjectCache.Instance.LoadGameObject(Att.trajectory, (go) =>
            {
                EffectObjectCache.Instance.FreeObject(go);
            });
        }

        List<SkillEffectInfo> EffectList = ConfigManager.Instance.mSkillEffectData.FindAllByID(Att.nId);

        if (EffectList == null)
            return;

        for (int i = 0; i < EffectList.Count; ++i)
        {
            SkillEffectInfo tmpInfo = EffectList[i];
            if (tmpInfo == null)
                continue;
            foreach (SkillSpecilInfo sf in tmpInfo.specilInfoList)
            {
                if (sf == null)
                    continue;
                if (!sf.effectName.Equals("0") && !sf.effectName.Equals(string.Empty))
                {
                    EffectObjectCache.Instance.LoadGameObject(sf.effectName, (go) =>
                    {
                        EffectObjectCache.Instance.FreeObject(go);
                    }, sf.effectName, tmpInfo.targetFindInfo.maxTarget);
                }

            }
        }
    }
}

/// <summary>
/// 战斗销毁物件信息
/// </summary>
public class FightDestroyInfo
{
    /// <summary>
    /// 删除方式
    /// </summary>
    public EPVEFinishStatus _DestroyStatus;
    /// <summary>
    /// 删除ID
    /// </summary>
    public object _DestroyID;

    public FightDestroyInfo(EPVEFinishStatus vStatus, object vID = null)
    {
        _DestroyStatus = vStatus;
        _DestroyID = vID;
    }
}


/// <summary>
/// 创建角色数据
/// </summary>
public class CData_CreateRole
{
    /// <summary>
    /// 角色信息
    /// </summary>
    public object RoleInfo;
    /// <summary>
    /// 角色UID
    /// </summary>
    public ulong UID;
    /// <summary>
    /// 路径索引[界面角色表示层级]
    /// </summary>
    public int PathIndex;
    /// <summary>
    /// 角色种类
    /// </summary>
    public ERoleType Type;
    /// <summary>
    /// 英雄性别[None表示查找PlayerData数据]
    /// </summary>
    public EHeroGender HeroGender;
    /// <summary>
    /// 战斗阵营
    /// </summary>
    public EFightCamp FightCamp;
    /// <summary>
    /// 战斗类型
    /// </summary>
    public EFightType FightType;
    /// <summary>
    /// 父物件
    /// </summary>
    public Transform Parent;
    /// <summary>
    /// 回调函数
    /// </summary>
    public System.Action<RoleBase> OnLoad;
    /// <summary>
    /// 初始位置
    /// </summary>
    public int InitPosX;
    /// <summary>
    /// 是否激活显示
    /// </summary>
    public bool IsActive;

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vUID">界面演示 英雄 怪物全部传0; 士兵传服务端发送的UID</param>
    /// <param name="vRoleInfo"></param>
    /// <param name="vType"></param>
    /// <param name="vPathIndex"></param>
    /// <param name="vParent"></param>
    public CData_CreateRole(object vRoleInfo, ulong vUID, int vPathIndex, ERoleType vType,
        EHeroGender vHeroGender, EFightCamp vFightCamp, EFightType vFightType, int vInitPosX = 0,
        Transform vParent = null, System.Action<RoleBase> vOnLoad = null, bool vIsActive = true)
    {
        RoleInfo = vRoleInfo;
        UID = vUID;
        PathIndex = vPathIndex;
        Type = vType;
        HeroGender = vHeroGender;
        FightCamp = vFightCamp;
        FightType = vFightType;
        InitPosX = vInitPosX;
        Parent = vParent;
        OnLoad = vOnLoad;
        IsActive = vIsActive;
    }
    public void CopyTo(CData_CreateRole vData)
    {
        if (vData == null)
            return;
        this.RoleInfo = vData.RoleInfo;
        this.UID = vData.UID;
        this.PathIndex = vData.PathIndex;
        this.Type = vData.Type;
        this.HeroGender = vData.HeroGender;
        this.FightCamp = vData.FightCamp;
        this.FightType = vData.FightType;
        this.InitPosX = vData.InitPosX;
        this.Parent = vData.Parent;
        this.OnLoad = vData.OnLoad;
        this.IsActive = vData.IsActive;
    }
}

/// <summary>
/// 战斗结束信息
/// </summary>
public class FightFinishedInfo
{
    /// <summary>
    /// 场景状态
    /// </summary>
    public ESceneStatus _SceneStatus;
    /// <summary>
    /// 结算数值
    /// </summary>
    public int _FinishValue;
    /// <summary>
    /// 怪物数量
    /// </summary>
    public int _MonsterNum;
    /// <summary>
    /// Boss数量
    /// </summary>
    public int _BossNum;
    /// <summary>
    /// 自己战损士兵
    /// </summary>
    public List<SoldierList> _OwnSoldier;
    /// <summary>
    /// 自己英雄数量
    /// </summary>
    public int _HeroNum;

    public FightFinishedInfo(FightFinishedInfo vInfo)
    {
        CopyTo(vInfo);
    }
    /// <summary>
    /// 创建战斗结束信息对象
    /// </summary>
    /// <param name="vSceneStatus">场景状态</param>
    /// <param name="vFinishValue">结算数值</param>
    public FightFinishedInfo(ESceneStatus vSceneStatus, int vFinishValue, int vMonsterNum = 0, int vBossNum = 0, List<SoldierList> vOwnSoldier = null, int vHeroNum = 0)
    {
        SetInfo(vSceneStatus, vFinishValue, vMonsterNum, vBossNum, vOwnSoldier, vHeroNum);
    }

    /// <summary>
    /// 复制数据
    /// </summary>
    /// <param name="vInfo"></param>
    public void CopyTo(FightFinishedInfo vInfo)
    {
        Init();
        if (vInfo == null)
            return;
        this._SceneStatus = vInfo._SceneStatus;
        this._FinishValue = vInfo._FinishValue;
        this._MonsterNum = vInfo._MonsterNum;
        this._BossNum = vInfo._BossNum;
        if (vInfo._OwnSoldier != null)
            this._OwnSoldier.AddRange(vInfo._OwnSoldier);
        this._HeroNum = vInfo._HeroNum;
    }

    private void Init()
    {
        _SceneStatus = ESceneStatus.essNone;
        _FinishValue = 0;
        _MonsterNum = 0;
        _BossNum = 0;
        if (_OwnSoldier == null)
            _OwnSoldier = new List<SoldierList>();
        _OwnSoldier.Clear();
        _HeroNum = 0;
    }

    private void SetInfo(ESceneStatus vSceneStatus, int vFinishValue, int vMonsterNum = 0, int vBossNum = 0, List<SoldierList> vOwnSoldier = null, int vHeroNum = 0)
    {
        Init();
        _SceneStatus = vSceneStatus;
        _FinishValue = vFinishValue;
        _MonsterNum = vMonsterNum;
        _BossNum = vBossNum;
        if (vOwnSoldier != null)
            _OwnSoldier.AddRange(vOwnSoldier);
        _HeroNum = vHeroNum;
    }
}

/// <summary>
/// 基础战斗信息
/// </summary>
public class ShowInfoBase
{
    /// <summary>
    /// 配置表Key值[英雄表-等级; 士兵表-ID; 怪物表-ID]
    /// </summary>
    public uint KeyData;
    /// <summary>
    /// 生命
    /// </summary>
    public ObscuredInt HP;
    /// <summary>
    /// 生命恢复
    /// </summary>
    public ObscuredInt HPRecovery;
    /// <summary>
    /// 魔法
    /// </summary>
    public ObscuredInt MP;
    /// <summary>
    /// 魔法恢复
    /// </summary>
    public ObscuredInt MPRecovery;
    /// <summary>
    /// 能量
    /// </summary>
    public ObscuredInt Energy;
    /// <summary>
    /// 能量恢复
    /// </summary>
    public ObscuredInt EnergyRecovery;
    /// <summary>
    /// 攻击力
    /// </summary>
    public ObscuredInt Attack;
    /// <summary>
    /// 攻击间隔[秒]
    /// </summary>
    public ObscuredFloat AttRate;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public ObscuredInt AttDistance;
    /// <summary>
    /// 命中
    /// </summary>
    public ObscuredInt Accuracy;
    /// <summary>
    /// 暴击
    /// </summary>
    public ObscuredInt Crit;
    /// <summary>
    /// 闪避
    /// </summary>
    public ObscuredInt Dodge;
    /// <summary>
    /// 韧性
    /// </summary>
    public ObscuredInt Tenacity;
    /// <summary>
    /// 移动速度
    /// </summary>
    public ObscuredInt MoveSpeed;
    /// <summary>
    /// 战斗力
    /// </summary>
    public ObscuredInt CombatPower;
    /// <summary>
    /// 统御力
    /// </summary>
    public ObscuredUShort Leadership;
    ///// <summary>
    ///// 魔法消耗增减数值
    ///// </summary>
    //public int _Num_MP_Cost;
    ///// <summary>
    ///// 魔法消耗增减系数
    ///// </summary>
    //public float _Coeff_Mp_Cost = 1;
    ///// <summary>
    ///// 召唤冷却增减数值
    ///// </summary>
    //public int Num_Summon_Interval;
    ///// <summary>
    ///// 召唤冷却增减系数
    ///// </summary>
    //public float _Coeff_Summon_Interval = 1;

    public ShowInfoBase()
    {
        ClearInfo();
    }

    public virtual void ClearInfo()
    {
        KeyData = 0;
        HP = 0;
        HPRecovery = 0;
        Attack = 0;
        AttRate = 0;
        AttDistance = 0;
        Accuracy = 0;
        Crit = 0;
        Dodge = 0;
        Tenacity = 0;
        MoveSpeed = 0;
        CombatPower = 0;
        MP = 0;
        MPRecovery = 0;
        Energy = 0;
        EnergyRecovery = 0;
        Leadership = 0;
        //_Num_MP_Cost = 0;
        //_Coeff_Mp_Cost = 1;
        //Num_Summon_Interval = 0;
        //_Coeff_Summon_Interval = 1;
    }

    public virtual void CopyTo(object vData)
    {
        if (vData == null)
            return;
        ShowInfoBase tmpData = (ShowInfoBase)vData;
        if (tmpData == null)
            return;

        this.KeyData = tmpData.KeyData;
        this.HP = tmpData.HP;
        this.HPRecovery = tmpData.HPRecovery;
        this.Attack = tmpData.Attack;
        this.AttRate = tmpData.AttRate;
        this.AttDistance = tmpData.AttDistance;
        this.Accuracy = tmpData.Accuracy;
        this.Crit = tmpData.Crit;
        this.Dodge = tmpData.Dodge;
        this.Tenacity = tmpData.Tenacity;
        this.MoveSpeed = tmpData.MoveSpeed;
        this.MP = tmpData.MP;
        this.MPRecovery = tmpData.MPRecovery;
        this.Energy = tmpData.Energy;
        this.EnergyRecovery = tmpData.EnergyRecovery;
        this.CombatPower = tmpData.CombatPower;
        this.Leadership = tmpData.Leadership;

        //UISystem.Instance.MainCityView.UpdateMaxFighting();
        //this._Num_MP_Cost = tmpData._Num_MP_Cost;
        //this._Coeff_Mp_Cost = tmpData._Coeff_Mp_Cost;
        //this.Num_Summon_Interval = tmpData.Num_Summon_Interval;
        //this._Coeff_Summon_Interval = tmpData._Coeff_Summon_Interval;
    }

    public void ReSetFightAttribute(uint vID, int vHP, int vHPRecovery, int vAttack, float vAttRate,
        int vAttDistance, int vAccuracy, int vCrit, int vDodge, int vTenacity, int vMoveSpeed,
        int vMP = 0, int vMPRecovery = 0, int vEnergy = 0, int vEnergyRecovery = 0, int vCombatPower = 0,
        ushort vLeadership = 0, int vNum_MP_Cost = 0, float vCoeff_Mp_Cost = 1, int vNum_Summon_Interval = 0, float vCoeff_Summon_Interval = 1)
    {
        KeyData = vID;
        HP = vHP;
        HPRecovery = vHPRecovery;
        Attack = vAttack;
        AttRate = vAttRate;
        AttDistance = vAttDistance;
        Accuracy = vAccuracy;
        Crit = vCrit;
        Dodge = vDodge;
        Tenacity = vTenacity;
        MoveSpeed = vMoveSpeed;
        MP = vMP;
        MPRecovery = vMPRecovery;
        Energy = vEnergy;
        EnergyRecovery = vEnergyRecovery;
        CombatPower = vCombatPower;
        Leadership = vLeadership;
        //_Num_MP_Cost = vNum_MP_Cost;
        //_Coeff_Mp_Cost = vCoeff_Mp_Cost;
        //Num_Summon_Interval = vNum_Summon_Interval;
        //_Coeff_Summon_Interval = vCoeff_Summon_Interval;
    }
}
/// <summary>
/// 英雄信息
/// </summary>
public class ShowInfoHero : ShowInfoBase
{

    public ShowInfoHero()
        : base()
    { }

    public override void ClearInfo()
    {
        base.ClearInfo();
    }

    public override void CopyTo(object vInfo)
    {
        if (vInfo == null)
            return;
        base.CopyTo(vInfo);
    }

}



/// <summary>
/// 玩家战斗信息[远征-敌方 PVP-双方]
/// </summary>
public class FightPlayerInfo
{
    /// <summary>
    /// 玩家唯一ID[账号唯一ID]
    /// </summary>
    public uint mAccID;
    /// <summary>
    /// 服务器唯一ID[角色唯一ID]
    /// </summary>
    public uint mCharID;
    /// <summary>
    /// 英雄配置表ID
    /// </summary>
    public uint mLevel;
    /// <summary>
    /// 英雄性别
    /// </summary>
    public uint mGender;
    /// <summary>
    /// 英雄战斗属性
    /// </summary>
    public ShowInfoHero mAttribute;
    /// <summary>
    /// 英雄技能
    /// </summary>
    public SkillsDepot mSkill;
    /// <summary>
    /// 英雄装备
    /// </summary>
    public ArtifactedDepot mEquip;
    /// <summary>
    /// 士兵列表
    /// </summary>
    public List<FightSoldierInfo> mSoldierList;
    /// <summary>
    /// 是否己方数据
    /// </summary>
    public bool mIsSelf;
    /// <summary>
    /// 敌方是否骑乘宠物[非0 骑乘宠物 ,0 骑乘坐骑]
    /// </summary>
    public uint mIsShowPet;
    /// <summary>
    /// 敌方宠物[为空的说明未装备]
    /// </summary>
    public Pet mPetInfo;

    /// <summary>
    /// 己方临时数据
    /// </summary>
    public FightPlayerInfo(ShowInfoHero vSelfHero, Dictionary<ulong, int> vSelfEquip, Dictionary<ulong, int> vSelfSoldier)
    {
        ReSetInfo(vSelfHero, vSelfEquip, vSelfSoldier);
    }
    /// <summary>
    /// 对方临时数据
    /// </summary>
    public FightPlayerInfo(fogs.proto.msg.ArenaPlayer vPlayerInfo, bool vIsSelf = false)
    {
        ReSetInfo(vPlayerInfo);
    }

    /// <summary>
    /// 己方临时数据
    /// </summary>
    /// <param name="vSelfHero"></param>
    /// <param name="vSelfEquip"></param>
    /// <param name="vSelfSoldier"></param>
    public void ReSetInfo(ShowInfoHero vSelfHero, Dictionary<ulong, int> vSelfEquip, Dictionary<ulong, int> vSelfSoldier)
    {
        InitInfo();

        if (vSelfHero != null)
        {
            mAccID = PlayerData.Instance._AccountID;
            mCharID = PlayerData.Instance._RoleID;
            mLevel = PlayerData.Instance._Level;
            mGender = PlayerData.Instance._Gender;

            this.mAttribute.CopyTo(vSelfHero);

            if (PlayerData.Instance._SkillsDepot != null)
            {
                mSkill = PlayerData.Instance._SkillsDepot;
            }

            if (vSelfEquip != null)
            {
                foreach (KeyValuePair<ulong, int> tmpInfo in vSelfEquip)
                {

                    Weapon tmpEquipInfo = PlayerData.Instance._WeaponDepot.FindByUid(tmpInfo.Key);
                    if (tmpEquipInfo == null)
                        continue;
                    if ((tmpInfo.Value - 1 >= 0) && (tmpInfo.Value - 1 < mEquip._EquiptList.Count))
                    {
                        mEquip._EquiptList[tmpInfo.Value - 1] = tmpEquipInfo;
                    }
                }
                //未更改装备数据//
                mEquip._EquiptList[3] = PlayerData.Instance._ArtifactedDepot._EquiptList[3];
                mEquip._EquiptList[7] = PlayerData.Instance._ArtifactedDepot._EquiptList[7];
            }

            if (vSelfSoldier != null)
            {
                foreach (KeyValuePair<ulong, int> tmpInfo in vSelfSoldier)
                {
                    Soldier tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(tmpInfo.Key);
                    if (tmpSoldier == null)
                        continue;
                    mSoldierList.Add(new FightSoldierInfo(tmpSoldier, tmpInfo.Value));
                }
            }
        }
    }

    /// <summary>
    /// 对方临时数据
    /// </summary>
    /// <param name="vPlayerInfo"></param>
    public void ReSetInfo(fogs.proto.msg.ArenaPlayer vPlayerInfo, bool vIsSelf = false)
    {
        InitInfo();
        if (vPlayerInfo != null)
        {
            if (vPlayerInfo.hero != null)
            {
                mAccID = vPlayerInfo.hero.accid;
                mCharID = vPlayerInfo.hero.charid;
                mLevel = (uint)vPlayerInfo.hero.level;
                mGender = vPlayerInfo.hero.gender;

                RefreshAttributeInfo(vPlayerInfo.hero.attr);
                RefreshSkillInfo(vPlayerInfo.hero.skills);
                RefreshEquipInfo(vPlayerInfo.hero.equips);
                if (vPlayerInfo.hero.souls != null)
                {
                    for (int i = 0; i < vPlayerInfo.hero.souls.Count; ++i)
                    {
                        if (vPlayerInfo.hero.souls[i] != null)
                            mSkill.oneAdd(vPlayerInfo.hero.souls[i].skill);
                    }
                }

                RefreshPetInfo(vPlayerInfo.hero.show_pet, vPlayerInfo.hero.pet);
            }

            if (vPlayerInfo.soldiers != null)
            {
                RefreshSoldierInfo(vPlayerInfo.soldiers);
            }
        }
        mIsSelf = false;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void InitInfo()
    {
        mAccID = 0;
        mCharID = 0;
        mLevel = 0;
        mGender = 0;
        if (mAttribute == null)
            mAttribute = new ShowInfoHero();
        mAttribute.ClearInfo();
        mSkill = new SkillsDepot();
        mEquip = new ArtifactedDepot();
        if (mSoldierList == null)
            mSoldierList = new List<FightSoldierInfo>();
        mSoldierList.Clear();
        mIsSelf = true;
        mIsShowPet = 0;
        mPetInfo = null;
    }

    /// <summary>
    /// 刷新英雄战斗属性
    /// </summary>
    /// <param name="attribute"></param>
    private void RefreshAttributeInfo(fogs.proto.msg.Attribute attribute)
    {
        if (attribute == null)
        {
            this.mAttribute = null;
            return;
        }
        this.mAttribute.KeyData = 1;
        this.mAttribute.Attack = attribute.phy_atk;
        this.mAttribute.Crit = attribute.crt_rate;
        this.mAttribute.Dodge = attribute.ddg_rate;
        this.mAttribute.Accuracy = attribute.acc_rate;
        this.mAttribute.AttDistance = attribute.atk_space;
        this.mAttribute.AttRate = attribute.atk_interval;
        this.mAttribute.Energy = attribute.energy_max;
        this.mAttribute.EnergyRecovery = attribute.energy_revert;
        this.mAttribute.HP = attribute.hp_max;
        this.mAttribute.HPRecovery = attribute.hp_revert;
        this.mAttribute.Leadership = (ushort)attribute.leader;
        this.mAttribute.MoveSpeed = attribute.speed;
        this.mAttribute.MP = attribute.mp_max;
        this.mAttribute.MPRecovery = attribute.mp_revert;
        this.mAttribute.Tenacity = attribute.tnc_rate;
    }

    /// <summary>
    /// 刷新英雄技能信息
    /// </summary>
    /// <param name="vSkillList"></param>
    private void RefreshSkillInfo(List<fogs.proto.msg.Skill> vSkillList)
    {
        if (vSkillList == null)
            return;
        mSkill.Serialize(vSkillList);
    }

    /// <summary>
    /// 刷新英雄装备信息
    /// </summary>
    /// <param name="vEquipList"></param>
    private void RefreshEquipInfo(List<fogs.proto.msg.Equip> vEquipList)
    {
        if (vEquipList == null)
            return;
        mEquip.Serialize(vEquipList);
    }

    /// <summary>
    /// 刷新士兵列表信息
    /// </summary>
    /// <param name="vSoldierList"></param>
    private void RefreshSoldierInfo(List<fogs.proto.msg.ArenaSoldier> vSoldierList)
    {
        if (vSoldierList != null)
        {
            foreach (fogs.proto.msg.ArenaSoldier tmpSingleInfo in vSoldierList)
            {
                if (tmpSingleInfo != null)
                {
                    if (tmpSingleInfo.soldier != null)
                    {
                        Soldier tmpSoldier = Soldier.createByID(tmpSingleInfo.soldier.id);
                        if (tmpSoldier != null)
                        {
                            tmpSoldier.Serialize(tmpSingleInfo.soldier);
                            tmpSoldier.SerializeShowInfo(tmpSingleInfo.soldier.attr);
                            mSoldierList.Add(new FightSoldierInfo(tmpSoldier, tmpSingleInfo.num));
                        }
                    }
                }
            }
        }
    }

    /// <summary>
    /// 刷新宠物信息
    /// </summary>
    /// <param name="vIsShowPet">敌方是否骑乘宠物[非0 骑乘宠物 ,0 骑乘坐骑]</param>
    /// <param name="vPetInfo">宠物数据[null表示没有装备]</param>
    private void RefreshPetInfo(uint vIsShowPet = 0, Pet vPetInfo = null)
    {
        mIsShowPet = vIsShowPet;
        mPetInfo = vPetInfo;
    }
}

/// <summary>
/// 战斗场景士兵信息-远征与PVP
/// </summary>
public class FightSoldierInfo
{
    /// <summary>
    /// 出战士兵信息
    /// </summary>
    public Soldier mSoldier;
    /// <summary>
    /// 出战士兵数量
    /// </summary>
    public int mNum;

    public FightSoldierInfo(Soldier vSoldier, int vNum)
    {
        mSoldier = vSoldier;
        mNum = vNum;
    }
}



/// <summary>
/// 战斗界面需要数据
/// </summary>
public class FightViewInfo
{

    /// <summary>
    /// 玩法类型
    /// </summary>
    public EFightType fightType;
    /// <summary>
    /// 当前关卡
    /// </summary>
    public uint curID;
    /// <summary>
    /// 下一关卡
    /// </summary>
    public uint nextID;
    /// <summary>
    /// 章节ID
    /// </summary>
    public uint chapterID;
    /// <summary>
    /// 体力消耗
    /// </summary>
    public int physical;
    /// <summary>
    /// 初始能量
    /// </summary>
    public int initEnergy;
    /// <summary>
    /// 初始魔法
    /// </summary>
    public int initMagic;
    /// <summary>
    /// 场景背景图
    /// </summary>
    public string scenBackGround;
    /// <summary>
    /// 限制时间
    /// </summary>
    public float timeLimit;
    /// <summary>
    /// 标准时间
    /// </summary>
    public float timeNormal;
    /// <summary>
    /// 时间节点
    /// </summary>
    public List<int> timeNodes;
    /// <summary>
    /// 一星条件
    /// </summary>
    public int conditionStar_1;
    /// <summary>
    /// 二星条件
    /// </summary>
    public int conditionStar_2;
    /// <summary>
    /// 三星条件
    /// </summary>
    public int conditionStar_3;
    /// <summary>
    /// 是否Boss关卡[主线 活动]
    /// </summary>
    public bool isBoss;
    /// <summary>
    /// 关卡自动战斗初始状态[0-锁定 1-未激活 2-激活全部 3-激活战斗 4-激活召唤]
    /// </summary>
    public int autoStatus;

    public FightViewInfo()
    {
        InitInfo();
    }

    public void ReSetInfo(EFightType vFightType, object vFightInfo)
    {
        if ((vFightType == EFightType.eftMain) || (vFightType == EFightType.eftActivity) || (vFightType == EFightType.eftEndless) ||
            (vFightType == EFightType.eftNewGuide) || (vFightType == EFightType.eftUnion) || (vFightType == EFightType.eftCaptureTerritory) || (vFightType == EFightType.eftCrossServerWar))
        {
            StageInfo tmpInfo = vFightInfo as StageInfo;
            if (vFightInfo == null)
                return;
            ReSetInfo(vFightType, tmpInfo.ID, tmpInfo.NextID, tmpInfo.ChapterID, tmpInfo.Physical, tmpInfo.InitEnergy, tmpInfo.InitMagic,
                tmpInfo.BackGroundID, tmpInfo.LimitTime, tmpInfo.NormTime, tmpInfo.MarkInfo, tmpInfo.Star1, tmpInfo.Star2, tmpInfo.Star3, tmpInfo.IsBoss, tmpInfo.AutoStatus);
        }
        else if ((vFightType == EFightType.eftExpedition))
        {
            ExpeditionData tmpInfo = vFightInfo as ExpeditionData;
            if (tmpInfo == null)
                return;
            ReSetInfo(vFightType, tmpInfo.id, tmpInfo.next_id, tmpInfo.chapter_id, 0, 0, 0, tmpInfo.background, tmpInfo.time_limit, 0, null, 0, 0, 0, true, tmpInfo.autoStatus);
        }
    }
    public void ReSetInfo(EFightType vFightType, uint vCurID, uint vNextID, uint vChapterID, int vPhysical, int vEnergy, int vMagic,
                                string vTexture, float vLimit, float vNormal, List<int> vNodes, int vStar1, int vStar2, int vStar3,
                                bool vIsBoss = true, int vAutoStatus = 0)
    {
        InitInfo();
        fightType = vFightType;
        curID = vCurID;
        nextID = vNextID;
        chapterID = vChapterID;
        physical = vPhysical;
        initEnergy = vEnergy;
        initMagic = vMagic;
        scenBackGround = vTexture;
        timeLimit = vLimit;
        timeNormal = vNormal;
        if (vNodes != null)
            timeNodes.AddRange(vNodes);
        conditionStar_1 = vStar1;
        conditionStar_2 = vStar2;
        conditionStar_3 = vStar3;
        isBoss = vIsBoss;
        autoStatus = vAutoStatus;
    }

    private void InitInfo()
    {
        curID = 0;
        nextID = 0;
        chapterID = 0;
        physical = 0;
        initEnergy = 0;
        initMagic = 0;
        scenBackGround = string.Empty;
        timeLimit = 0;
        timeNormal = 0;
        if (timeNodes == null)
            timeNodes = new List<int>();
        else
            timeNodes.Clear();
        conditionStar_1 = 0;
        conditionStar_2 = 0;
        conditionStar_3 = 0;
        isBoss = true;
        autoStatus = 0;
    }

}

/// <summary>
/// 场景创建数据
/// </summary>
public class CreateSceneInfo
{
    /// <summary>
    /// 战斗种类
    /// </summary>
    public EFightType sceneType;
    /// <summary>
    /// 配置表ID
    /// </summary>
    public uint sceneID;
    /// <summary>
    /// 背景图
    /// </summary>
    public string sceneBackGround;

    public CreateSceneInfo(EFightType vSceneType, uint vSceneID, string vBackGround)
    {
        CopyTo(vSceneType, vSceneID, vBackGround);
    }

    public void CopyTo(EFightType vSceneType, uint vSceneID, string vBackGround)
    {
        InitInfo();
        sceneType = vSceneType;
        sceneID = vSceneID;
        sceneBackGround = vBackGround;
    }

    public void CopyTo(CreateSceneInfo vInfo)
    {
        if (vInfo == null)
            return;
        CopyTo(vInfo.sceneType, vInfo.sceneID, vInfo.sceneBackGround);
    }

    private void InitInfo()
    {
        sceneType = EFightType.eftMain;
        sceneID = 0;
        sceneBackGround = string.Empty;
    }
}
//------------------------------------------------------------------------------------------------------------------//
//------------------------------------------------------------------------------------------------------------------//
//------------------------------------------------------------------------------------------------------------------//




public enum EShowUIType
{
    Default
}

public struct UIBoundary
{
    public float left;
    public float right;
    public float up;
    public float down;

    public static UIBoundary zero
    {
        get
        {
            UIBoundary boundary = new UIBoundary();
            boundary.down = 0;
            boundary.left = 0;
            boundary.right = 0;
            boundary.up = 0;
            return boundary;
        }
    }
}

public enum EMoveDirection
{
    LeftToRight,
    RightToLeft,
    UpToDown,
    DownToUp
}

/// <summary>
/// 游戏服务器状态
/// </summary>
public enum GameServerState
{
    /// <summary>
    /// 空闲
    /// </summary>
    Free,

    /// <summary>
    /// 繁忙
    /// </summary>
    Busy,

    /// <summary>
    /// 维护中
    /// </summary>
    Maintenance,
}

public enum MessageBoxType : byte
{
    None,
    /// <summary>
    /// 仅带有一个按钮的提示框
    /// </summary>
    mb_Ok = 1,
    /// <summary>
    /// 带有两个左右两个按钮的提示框
    /// </summary>
    mb_YesNo = 2,
    /// <summary>
    /// loading
    /// </summary>
    mb_Loading = 3,
    /// <summary>
    /// 富文本
    /// </summary>
    mb_RichText,
    /// <summary>
    /// 仅飘过一行文字
    /// </summary>
    mb_Hint,
    /// <summary>
    /// 带有两个左右两个按钮还有标记不显示的提示框
    /// </summary>
    mb_YesNo_Mark,

}

public enum ChatTypeEnum
{
    /// <summary>
    /// 世界聊天
    /// </summary>
    World = 3,
    /// <summary>
    /// 军团聊天
    /// </summary>
    Corps = 2,
    /// <summary>
    /// 私聊
    /// </summary>
    Private = 1,
}

public enum IDType : byte
{
    None = 0,
    Soldier = 1,
    EQ = 2,
    Prop = 3,
    SP = 4,
    Gold = 5,
    Diamond = 6,
    Medal = 7,
    Honor = 8,
    Exp = 9,
    SoldierExp = 10,
    UnionToken = 11,
    Free = 12,
    LifeSoul = 13,
    RecycleCoin = 14,
    CityToken = 15,
    Pet = 16,

}


/// <summary>
/// 0-客户端没有“前往”按钮
/// 1-跳转到具体的副本界面（选择关卡的界面）
/// 2-跳转到当前已解锁的最后一个副本界面
/// 3-跳转到当前已解锁的最后一个精英副本界面
/// 4-跳转到远征天下主界面
/// 5-跳转到无尽战场主界面
/// 6-跳转到活动副本主界面
/// 7-跳转到VIP特权界面
/// 8-跳转到演武台界面
/// 9-跳转到“充值”界面
/// 10-跳转到奴隶系统主界面（暂时未做）
/// 11-跳转到招贤馆界面
/// 12-跳转到背包界面中的神器页签
/// 13-跳转到背包界面中的装备页签
/// 14-跳转到英雄界面
/// 15-跳转到武将界面
/// 16-跳转到当前已解锁的最后一个护送副本界面
/// 17-跳转到军团界面
/// 18-跳转到商城界面
/// 19-跳转到特权商店界面
/// 20-跳转到勋章商店界面
/// 21-跳转到军需库界面
/// 22-跳转到军团商店界面
/// 23-跳转到异域探险界面
/// 24-跳转到连续登录界面
/// 25-跳转到任务界面
/// 26-跳转到活跃宝箱界面
/// 27-跳转到缘宝阁界面
/// 28-跳转7725账号绑定窗口
/// 29-跳转跳转到GP/APPLE市场
/// 30-打开兑换铜钱窗口
/// 31-跳转到天将神兵天将页签
/// 32-跳转到天将神兵神兵页签
/// 33-跳转到攻城略地
/// 34-跳转到全服霸主
/// </summary>
public enum ETaskOpenView
{
    None = 0,
    DungeonWithId = 1,
    LastestNormalDungeon = 2,
    LastestAdvancedDungeon = 3,
    Expedition = 4,//远征天下//
    Endless = 5,
    ActivityDungeon = 6,
    VipPage = 7,
    Arena = 8,
    RechargePage = 9,
    Slave = 10,
    Recruit = 11,//招贤馆界面//
    Bag_GodWeapon = 12,
    Bag_Equip = 13,
    Hero = 14,
    Soldier = 15,
    LastestProtectDungeon = 16,
    Union = 17,
    Mall = 18,
    Shop_Nor = 19,
    Shop_Medal = 20,
    Shop_Honor = 21,
    Shop_Union = 22,
    UnionPVE = 23,
    SignView = 24,
    TaskView = 25,
    LivenessView = 26,
    DrowEquipView = 27,
    BindingAccount = 28,//跳转7725账号绑定窗口//
    GPMarket = 29,//跳转跳转到GP/APPLE市场//
    BuyCoin = 30,//打开兑换铜钱窗口//
    Sacrificial_Soldier = 31,//跳转到天将神兵天将页签//
    Sacrificial_Equip = 32,//跳转到天将神兵神兵页签//
    CaptureTerritory = 33,//攻城略地//
    Supermacy = 34,//全服霸主//
    QualifyingView = 35,//排位赛//
    ChatView = 36,
    LifeSoulView = 37,
    PetSystemView = 38,
}

/// <summary>
/// 开启的功能类型
/// 英雄系统
/// 武将系统
/// 主线战斗
/// 活动副本
/// 无尽模式
/// 远征玩法
/// 竞技场
/// 背包系统
/// 商店系统
/// 系统设置
/// 聊天系统
/// 邮件系统
/// 升级提示
/// 点金手
/// VIP与充值
/// 招募系统
/// 连续登录
/// 活动公告
/// 任务系统
/// 活跃宝箱
/// 排行榜系统
/// 公会系统
/// 奴隶系统
/// 扫荡
/// 批量扫荡
/// 一键强化
/// 战斗两倍加速
/// 奴隶位置二
/// 奴隶位置三
/// 奴隶位置四
/// </summary>
public enum OpenFunctionType : int
{
    None = 0,
    Hero = 1001,
    Soldier = 1002,
    PVE = 1003,
    Activity = 1004,//活动副本//
    Endless = 1005,
    Expedition = 1006,//远征玩法//
    Arena = 1007,
    Package = 1008,
    Store = 1009,
    SystemSetting = 1010,
    Chat = 1011,
    Mail = 1012,
    LevelUp = 1013,
    ExchangeGold = 1014,
    VIP = 1015,
    Recruit = 1016,
    ContinuousLogin = 1017,
    ActivityAnnouncement = 1018,
    Task = 1019,
    Liveness = 1020,
    Rank = 1021,
    Sociaty = 1022,//公会系统//
    Slave = 1023,//奴隶系统//
    Magical = 1024,//天降神兵//
    YuanBaoGe = 1025,
    ShangCheng = 1026,
    GongLue = 1027,
    TuJian = 1028,
    GongCheng = 1029,
    BaZhu = 1030,
    UnionPrison = 1031,//信仰神殿
    /// <summary>
    /// 排位赛
    /// </summary>
    Qualifying = 1032,
    /// <summary>
    /// 宠物系统
    /// </summary>
    Pet = 1033,

    Clear = 2001,//主线副本单次扫荡//
    BatchClear = 2002,//主线副本多次扫荡//
    OneKeyEnchance = 2003,//一键强化//
    DoubleFightSpeed = 2004,
    SlaveSlot2 = 2005,
    SlaveSlot3 = 2006,
    SlaveSlot4 = 2007,
    SoldierEquip = 2008,//武将装备//
    ArtifactIntensify = 2009,//神器强化//
    SoldierSkill = 2010,//武将技能//
    GateSoldierLockLevel3 = 2020,//剧情战役士兵锁定等级//
    GateSoldierLockLevel4 = 2011,//剧情战役士兵锁定等级//
    GateSoldierLockLevel5 = 2012,//剧情战役士兵锁定等级//
    GateSoldierLockLevel6 = 2013,//剧情战役士兵锁定等级//
    SoldierLevelUp = 2014,//武将升级//
    WorldChatLockLevel = 2015,
    ArtifactControl = 2016,//神器装备操作//
    HeroSkillControl = 2017,//英雄技能操作//
    GetPath = 2018,//获取途径//

    P7725ThirdPartyPayment = 2019,

    SacrificialSystem_SoldierRed = 2024,
    SacrificialSystem_SoldierOrange = 2023,
    SacrificialSystem_EquipRed = 2026,
    SacrificialSystem_EquipOrange = 2025,
    jineng1 = 3001,//征伐止域//
    jineng2 = 3002,
    jineng3 = 3003,
    jineng4 = 3004,
    jineng5 = 3005,
    jineng6 = 3006,
    jineng7 = 3007,
    jineng8 = 3008,
    jineng9 = 3009,
    jineng10 = 3010,
    jineng11 = 3011,
    jineng12 = 3012,
    jineng13 = 3013,
    jineng14 = 3014,
    shenqi1 = 4001,//神器//
    shenqi2 = 4002,
    shenqi3 = 4003,
    shenqi4 = 4004,
    shenqi5 = 4005,
    shenqi6 = 4006,
    shenqi7 = 4007,
    shenqi8 = 4008,
    shenqi9 = 4009,
    shenqi10 = 4010,
    shenqi11 = 4011,
    shenqi12 = 4012,
    shenqi13 = 4013,
    shenqi14 = 4014,
    shenqi15 = 4015,
    shenqi16 = 4016,
    shenqi17 = 4017,
    shenqi18 = 4018,
    shenqi19 = 4019,
    shenqi20 = 4020,
    shenqi21 = 4021,
    TaoFaZhiLu = 5001,
    JingYingTaoFa = 5002,
    VIPTeQuan = 5003,
    ChongZhi = 5004,
    BeiBaoShenQi = 5005,
    BeiBaoZhuangBei = 5006,
    HuSongYouJun = 5007,
    XunZhangShangDian = 5008,
    JunXuKu = 5009,
    JunTuanShangDian = 5010,
    YiYuTanXian = 5011,
    TianJIangXiTong = 5012,
    ShenBingXiTong = 5013,

    SupermacyAutoBattle=2021,//全服霸主自动战斗//
    supermacyAutoClearCD=2022,//全服霸主自动清除CD//

    AchievementSystem=2027,//成就系统
    LifeSoulSystm=1035,//命魂系统

    CrossServerWar=1034,//跨服战
    UnionHegemony = 1036//群雄争霸
}

public enum ELoginAwardGetContinous
{
    NotGet = 0,
    Got = 1
}

public enum UpdateFileState : byte
{
    None = 0,
    CheckVersion = 1,
    GetUpdateList = 2,
    ComparedVersion = 3,
    UpdateResource = 4,
    DeleteFile = 5,
    ReadResPath = 6,
    Over,
    DownResWarn,
    DownApkWarn,
    DownCodeWarn,
    DownCode,
    DownAPK,
    DownCodeOver

}

public enum TargetPlatforms : byte
{
    None = 100,
    Android_360 = 1,
    Android_SF = 2,
    IOS_QuickSDK = 101,
    /// <summary>
    /// 衝殺小英雄
    /// </summary>
    Android_7725 = 102,
    SF_7723 = 103,
    /// <summary>
    /// 倒退噜骑士
    /// </summary>
    Android_7725OL = 104,

    Kakao_Gameshop = 201,
    Kakao_Google = 202,
    Kakao_OneStore = 203,
    /// <summary>
    /// 国内版[兵临城下]
    /// </summary>
    Android_GuoNei = 210
}



/// <summary>
/// 对话说话人
/// </summary>
public enum DialogueTalker
{
    Player = 1,
    Npc = 2
}

/// <summary>
/// 说话人位置
/// </summary>
public enum DialogueTalkerPos
{
    Left = 1,
    Right = 2
}

/// <summary>
/// 触发对话类型
/// 1 首次进入章节                                            -zhu
/// 2 首次通关关卡战斗结算时弹对话，奖励结算也同时结算        -ren
/// 3 首次进入关卡（备战前）                                  -zhu
/// 4 首次关卡战斗（战斗开始）                                -ren
/// 5 NPC血量低于等于                                         -ren
/// 6 NPC死亡（结算界面前）                                   -ren
/// </summary>
public enum DialogueTriggerType
{
    FirstChooseDungeon = 1,
    FirstPassLevel = 2,
    FirstChooseLevel = 3,
    FirstFight = 4,
    NpcHp = 5,
    NpcDie = 6
}


/// <summary>
/// 
/// 玩家填0 
/// 非玩家填1 2
/// 进入商店 3 
/// 点击商店NPC 4
/// 进入军需 5
/// 点击军需NPC 6
/// 进入勋章商店 7
/// 点击勋章商店NPC 8
/// </summary>
public enum NPCTalkType
{
    Player = 0,
    Guider1 = 1,
    Guider2 = 2,
    StoreAccess = 3,
    StoreClick = 4,
    HonorAccess = 5,
    HonorClick = 6,
    MedalAccess = 7,
    MedalClick = 8,
    UnionAccess = 9,
    UnionClick = 10,
    RecycleAccess = 11,
    RecycleClick = 12,
}
/// <summary>
/// 活动系统 活动类型
/// 1=单充福利
/// 2=累充送礼
/// 3=登陆好礼
/// 4=全民福利
/// 5=开服基金
/// 6=限时抢购
/// 7=消耗活动
/// 8=双倍爆率（远征）
/// 9=双倍爆率（兵伐不臣）
/// 10=双倍爆率翻倍（远征）
/// 11=挑战次数翻倍（兵法）
/// 12=挑战次数翻倍（无尽）
/// 13=双倍爆率（无尽）
/// 14=单日充值
/// 15=等级榜
/// 16=战力榜
/// 17=讨伐之路奖励双倍
/// 18=护送奖励双倍
/// 19=精英奖励双倍
/// 20=演武台双倍竞技币
/// 21=单服充值
/// 22=闯关送将
/// 23=单倍充值奖励
/// 24=双倍充值奖励
/// 25=单服消耗排行
/// 26=跨服储值排行
/// 27=跨服消耗排行
/// </summary>
public enum GameActivityType
{
    None = 0,
    SingleCharge = 1,
    TotalCharge = 2,
    LoginAward = 3,
    Welfare = 4,
    Fund = 5,
    BuyingItem = 6,
    Cost = 7,
    DropDouble_Expedition = 8,
    DropDouble_EventDungeons = 9,
    DropDouble_Endness = 10,
    ChallengeDouble_Expedition = 11,
    ChallengeDouble_EventDungeons = 12,
    ChallengeDouble_Endness = 13,
    OneDayCharge = 14,
    Rank_Level = 15,
    Rank_Combat = 16,
    DropDouble_Crusade = 17,
    DropDouble_Guard = 18,
    DropDouble_Elite = 19,
    CoinDouble_Arena = 20,
    RechargeRank = 21,
    PVEGift = 22,
    RechargeSingleReturn = 23,
    RechargeDoubleReturn = 24,
    CostRank = 25,
    CrossRealmRechargeRank = 26,
    CrossRealmCostRank = 27
}

/// <summary>
///  0表示不能领取
///  1表示可以领取
///  2表示已经领取
/// </summary>
public enum GameAcitvityAwardStateEnum
{
    CanNotReceive = 0,
    CanReceive = 1,
    Recieved = 2,
}

/// <summary>
/// 0未开始
/// 1正在进行
/// 2过期
/// 3永久的
/// </summary>
public enum GameAcitvityStateEnum
{
    NotStart = 0,
    InProgress = 1,
    Expired = 2,
    Eternal = 3,
}


public enum PlayerInfoTypeEnum
{
    /// <summary>
    /// 军团
    /// </summary>
    Corps,
    /// <summary>
    /// 反抗
    /// </summary>
    Revolt,
    /// <summary>
    /// 竞技场
    /// </summary>
    Arena,
    /// <summary>
    /// 奴役攻击
    /// </summary>
    SlaveAttack,
    /// <summary>
    /// 显示无尽战力
    /// </summary>
    EndlessPlayerInfo,
    /// <summary>
    /// 最强阵容
    /// </summary>
    MostLineup,
    /// <summary>
    /// 显示VIP
    /// </summary>
    VIP,
    /// <summary>
    /// 积分
    /// </summary>
    Integral,
    /// <summary>
    /// 排位赛主动攻击
    /// </summary>
    QualifyingAttack,
    /// <summary>
    /// 排位赛日志玩家信息
    /// </summary>
    QualifyingLog,
    /// <summary>
    /// 好友
    /// </summary>
    Friend,
}

public enum GuildFightState
{
    SignUp,  //开始报名
    Signed,  //已报名
    Match,   //匹配
    Ready,   //准备
    Fight,   //开始战斗
    CoolDown, //冷却
    Cancel    //争霸战取消
}

public enum UnionRankType
{
    Kill = 0,
    Damage = 1,
    Hegemony = 2,
}

public enum UnionRankItemType
{
    PlayerRank,
    UnionRank,
}

/// <summary>
/// 默认 0
/// 自己 1
/// 敌人 2
/// 双输 3
/// </summary>
public enum HegemonyBattleResult
{
    None = 0,
    Self = 1,
    Enemy = 2,
    Lose_Lose = 3
}
public enum ECaptureTerritoryFightState
{
    NotFighting = 2,
    Fighting = 1,
}

public enum EAnnouncementType
{
    Login,
    MainCity,
}

/// <summary>
/// 攻城略地进攻机制
/// </summary>
public enum ECaptureFightTimesState
{
    /// <summary>
    /// 无限购买和进攻  算总伤害
    /// </summary>
    Endless,
    /// <summary>
    /// 进攻次数限制可购买
    /// </summary>
    TimesLimit,
}

/// <summary>
/// 此枚举对应的int值参与了数组下标等计算 ，请勿随意变化
/// </summary>
public enum ERecycleContentType
{
    None = 0,
    /// <summary>
    /// 消耗品
    /// </summary>
    Prop = 1,
    /// <summary>
    /// 材料
    /// </summary>
    Material = 2,
    /// <summary>
    /// 装备碎片
    /// </summary>
    EquipChip = 3,
    /// <summary>
    /// 士兵碎片
    /// </summary>
    SoldierChip = 4,
    /// <summary>
    /// 装备
    /// </summary>
    Equip = 5,
    /// <summary>
    /// 武将
    /// </summary>
    Soldier = 6,
    /// <summary>
    /// 命魂
    /// </summary>
    LifeSoul = 7,
}

public enum ETopFuncState
{
    ShowSp = 1,
    ShowCityToken = 2,
}