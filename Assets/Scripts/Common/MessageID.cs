using UnityEngine;
using System.Collections;

/// <summary>
/// 命令ID规则
/// 
/// 总共8位
/// 第一位表示命令种类[0-网络 1-界面 2-战斗 3-音效]
/// 
/// 命名以类型名字的缩写开头+功能[例: FightMessage-FM_CreateScene]
/// </summary>

/// <summary>
/// 网络命令
/// </summary>
public class NetMessage
{
    public uint NM_Test = 0x00000000;
}
/// <summary>
/// 界面命令
/// </summary>
public class UIMessage
{
    public uint UIM_TEST = 0x10000000;
}
/// <summary>
/// 战斗命令[0x2000002a]
/// </summary>
public class FightMessage
{
    /// <summary>
    /// 创建关卡场景[关卡ID]
    /// </summary>
    public uint FM_SceneCreate = 0x20000000;
    /// <summary>
    /// 删除关卡场景[]
    /// </summary>
    public uint FM_SceneDelete = 0x20000001;
    /// <summary>
    /// 更新关卡场景显示[]
    /// </summary>
    public uint FM_SceneReSet = 0x20000002;

    /// <summary>
    /// 在场景中创建一个角色[CData_CreateRole]
    /// </summary>
    public uint FM_RoleCreate = 0x20000003;
    /// <summary>
    /// 删除场景中一个角色[角色场景唯一ID]
    /// </summary>
    public uint FM_RoleDelete = 0x20000004;
    /// <summary>
    /// 清空场景中的角色[]
    /// </summary>
    public uint FM_RoleClear = 0x20000005;

    /// <summary>
    /// 开启角色移动
    /// </summary>
    public uint FM_RoleMove_Start = 0x20000006;
    /// <summary>
    /// 关闭角色移动
    /// </summary>
    public uint FM_RoleMove_Stop = 0x20000007;

    /// <summary>
    /// 战斗结束条件
    /// </summary>
    public uint FM_FightObjDestroy = 0x20000008;

    /// <summary>
    /// 战斗停止
    /// </summary>
    public uint FM_FightSetPause = 0x20000009;

    /// <summary>
    /// 战斗继续
    /// </summary>
    public uint FM_FightSetResume = 0x2000000a;

    /// <summary>
    /// 修改战斗速度[自动增加一倍速度]
    /// </summary>
    public uint FM_FightChangeSpeed = 0x2000000b;

    /// <summary>
    /// 战斗结束[战斗场景结束状态]
    /// </summary>
    public uint FM_FightFinished = 0x2000000c;

    /// <summary>
    /// 英雄使用技能
    /// </summary>
    public uint FM_FightUseSkill = 0x2000000d;
    /// <summary>
    /// 英雄技能释放完毕
    /// </summary>
    public uint FM_FightHeroSkillFinished = 0x20000014;

    /// <summary>
    /// 重置战斗星级
    /// </summary>
    public uint FM_FightReSetStar = 0x2000000e;

    /// <summary>
    /// 重置战斗积分
    /// </summary>
    public uint FM_FightReSetPlayerScore = 0x2000000f;

    /// <summary>
    /// 重置能量
    /// </summary>
    public uint FM_FightReSetPlayerEnergy = 0x20000010;

    /// <summary>
    /// 重置最大能量
    /// </summary>
    public uint FM_FightExchangeMaxEnergy = 0x20000011;

    /// <summary>
    /// 重置魔法
    /// </summary>
    public uint FM_FightReSetPlayerMagic = 0x20000012;

    /// <summary>
    /// 重置最大魔法
    /// </summary>
    public uint FM_FightExchangeMaxMagic = 0x20000013;

    /// <summary>
    /// 修改己方城堡血量/逃跑敌人数量
    /// </summary>
    public uint FM_FightExchangeInfo_Self = 0x20000015;

    /// <summary>
    /// 修改敌方城堡血量
    /// </summary>
    public uint FM_FightExchangeInfo_Enemy = 0x20000016;

    /// <summary>
    /// 结束英雄延迟 开启AI状态
    /// </summary>
    public uint FM_FightStartAIHero = 0x20000017;

    /// <summary>
    /// 开启角色的无尽模式过度状态
    /// </summary>
    public uint FM_FightEndlessExcessive = 0x20000018;

    /// <summary>
    /// 战斗场景闪红提示
    /// </summary>
    public uint FM_FightHurtHint = 0x20000019;

    /// <summary>
    /// 重置英雄移动状态
    /// </summary>
    public uint FM_ReSetMoveStatus = 0x2000001a;

    /// <summary>
    /// 新手引导第二次开启刷怪
    /// </summary>
    public uint FM_NewGuideReCreateEnemy = 0x2000001b;

    /// <summary>
    /// 新手引导开启释放技能
    /// </summary>
    public uint FM_NewGuideCanUseSkill = 0x2000001c;

    /// <summary>
    /// 开启魔法回复
    /// </summary>
    public uint FM_StartRecoveryMagic = 0x2000001d;

    /// <summary>
    /// 开启移动战斗摄像机
    /// </summary>
    public uint FM_StartShowBossTalk = 0x2000001e;

    /// <summary>
    /// 删除一个士兵数量
    /// </summary>
    public uint FM_DeleteSingleSoldierNum = 0x2000001f;

    /// <summary>
    /// 开启战斗对话
    /// </summary>
    public uint FM_BossInScene = 0x20000020;

    /// <summary>
    /// 关闭BOSS对话显示
    /// </summary>
    public uint FM_CloseShowBossTalk = 0x20000021;

    /// <summary>
    /// 显示Boss出场效果
    /// </summary>
    public uint FM_ShowBossStatus = 0x20000022;

    /// <summary>
    /// 倒计时操作
    /// </summary>
    public uint FM_FightCountdownOperate = 0x20000023;

    /// <summary>
    /// 改变战斗状态[手动-自动]
    /// </summary>
    public uint FM_ChangeFightAutoStatus = 0x20000024;

    /// <summary>
    /// 开启刷怪
    /// </summary>
    public uint FM_StartShowFightStatus = 0x20000025;

    /// <summary>
    /// 设置角色比例
    /// </summary>
    public uint FM_SetRoleScaleValue = 0x20000026;

    /// <summary>
    /// 读取自动战斗状态
    /// </summary>
    public uint FM_LoadAutoStatus = 0x20000027;

    /// <summary>
    /// 記錄全服爭霸傷害值
    /// </summary>
    public uint FM_RecordServerHegemonyHurt = 0x20000028;

    /// <summary>
    /// 修改宠物当前怒气值
    /// </summary>
    public uint FM_ChangeCurPetPower = 0x20000029;

    /// <summary>
    /// 宠物释放技能
    /// </summary>
    public uint FM_FightPetUseSkill = 0x2000002a;
}
/// <summary>
/// 音效命令[0x3000000b]
/// </summary>
public class SoundMessage
{
    /// <summary>
    /// 播放-背景音乐
    /// </summary>
    public uint SM_PlayMusic = 0x30000000;
    /// <summary>
    /// 播放-音效
    /// </summary>
    public uint SM_PlayAudio = 0x30000001;
    /// <summary>
    /// 调整音量-背景音乐
    /// </summary>
    public uint SM_ChangeVolume_Music = 0x30000002;
    /// <summary>
    /// 调整音量-音效
    /// </summary>
    public uint SM_ChangeVolume_Audio = 0x30000003;
    /// <summary>
    /// 设置静音状态-背景音乐
    /// </summary>
    public uint SM_SetMuteStatus_Music = 0x30000004;
    /// <summary>
    /// 设置静音状态-音效
    /// </summary>
    public uint SM_SetMuteStatus_Audio = 0x30000005;
    /// <summary>
    /// 清空声音
    /// </summary>
    public uint SM_Clear_Sound = 0x30000006;
    /// <summary>
    /// 清空背景音乐
    /// </summary>
    public uint SM_Clear_Music = 0x30000007;
    /// <summary>
    /// 清空音效
    /// </summary>
    public uint SM_Clear_Audio = 0x30000008;
    /// <summary>
    /// 保存设置
    /// </summary>
    public uint SM_Save_Sound_Status = 0x30000009;
    /// <summary>
    /// 删除一个音效
    /// </summary>
    public uint SM_DeleteSingle_Audio = 0x3000000a;
    /// <summary>
    /// 逐渐减小背景音乐声音
    /// </summary>
    public uint SM_ReduceVolume_Music = 0x3000000b;
}



public class MessageID
{
    private static NetMessage _Net;
    public static NetMessage Message_Net
    {
        get
        {
            if (_Net == null)
                _Net = new NetMessage();
            return _Net;
        }
    }
    private static UIMessage _UI;
    public static UIMessage Message_UI
    {
        get
        {
            if (_UI == null)
                _UI = new UIMessage();
            return _UI;
        }
    }
    private static FightMessage _Fight;
    public static FightMessage Message_Fight
    {
        get
        {
            if (_Fight == null)
                _Fight = new FightMessage();
            return _Fight;
        }
    }
    private static SoundMessage _Sound;
    public static SoundMessage Message_Sound
    {
        get
        {
            if (_Sound == null)
                _Sound = new SoundMessage();
            return _Sound;
        }
    }

    /// <summary>
    /// 推送消息反馈
    /// </summary>
    public const uint MsgRecvConfirmID = 0x0034;

    /// <summary>
    /// 新消息推送
    /// </summary>
    public const uint NotifyRefreshID = 0x0023;

    //登陆消息
    public const uint VersionReqID = 0x0003;
    public const uint VersionRespID = 0x0004;

    public const uint AuthorizeReqID = 0x0005;
    public const uint AuthorizeRespID = 0x0006;

    /// <summary>
    /// 注册
    /// </summary>
    public const uint RegisterReqID = 0x000D;

    //进入游戏//
    public const uint EnerGameServerReqID = 0x0007;
    public const uint EnerGameServerRespID = 0x0008;
    public const uint SelectAreaInfoID = 0x0030;
    public const uint SelectAreaInfoRespID = 0x0031;

    /// <summary>
    /// 更新剧情对话已显示的列表
    /// </summary>
    public const uint AddTalkReqID = 0x0156;
    public const uint AddTalkRespID = 0x0157;

    /// <summary>
    /// 随机名字
    /// </summary>
    public const uint CharnameReqID = 0x0010;
    public const uint CharnameRespID = 0x0011;

    /// <summary>
    /// 创建角色
    /// </summary>
    public const uint CreateCharReqID = 0x0012;
    public const uint CreateCharRespID = 0x0013;

    /// <summary>
    /// 请求体力恢复数据更新
    /// </summary>
    public const uint PHpowerRevertReqID = 0x0035;
    public const uint PHpowerRevertRespID = 0x0036;

    /// <summary>
    /// 金币兑换
    /// </summary>
    public const uint TouchGoldReqID = 0x0883;
    public const uint TouchGoldRespID = 0x0884;

    /// <summary>
    /// 用户登出
    /// </summary>
    public const uint PlayerLogoutReqID = 0x0009;

    /// <summary>
    /// 被服务器T下线
    /// </summary>
    public const uint NotifyPlayerOfflineID = 0x000A;

    /// <summary>
    /// 向login服务器发送当前登录的游戏服务器ID
    /// </summary>
    public const uint NotifyPlayerLoginAreaServerID = 0x0030;
    public const uint NotifyPlayerLoginAreaServerRespID = 0x0031;

    public const uint EnterConnecterSrvReqID = 0x0020;
    public const uint EnterConnecterSrvRespID = 0x0021;
    public const uint HeartbeatID = 0x0022;

    public const uint ReportServerLoadID = 0x0014;
    public const uint ReportServerLoadRespID = 0x0015;

    // 技能
    public const uint SkillDogUpStarReqID = 0x0100;
    public const uint SkillDogUpStarRespID = 0x0101;
    public const uint SkillUpgradeReqID = 0x0800;
    public const uint SkillUpgradeRespID = 0x0801;
    public const uint AutoUpgradeSkillReqID = 0x0802;
    public const uint AutoUpgradeSkillRespID = 0x0803;

    //士兵
    public const uint SoldierUpStarReqID = 0x0200;
    public const uint SoldierUpStarRespID = 0x0201;
    public const uint SoldierUpLvReqID = 0x0202;
    public const uint SoldierUpLvRespID = 0x0203;
    public const uint SoldierSelectReqID = 0x0204;
    public const uint SoldierSelectRespID = 0x0205;
    public const uint NotifyGetNewSoldierID = 0x0892;
    public const uint SoldierUpLevelStarReqID = 0x0206;
    public const uint SoldierUpLevelStarRespID = 0x0207;
    public const uint SoldierUpQualityReqID = 0x0208;
    public const uint SoldierUpQualityRespID = 0x0209;
    public const uint SoldierUpStepReqID = 0x020A;
    public const uint SoldierUpStepRespID = 0x020B;
    //好友系统
    public const uint ApplyAddFriendsReqID = 0x1270;   //申请加好友
    public const uint ApplyAddFriendsRespID = 0x1271;
    public const uint DonateFriendsHpReqID = 0x1272;    //赠送体力
    public const uint DonateFriendsHpRespID = 0x1273;
    public const uint RecieveFriendHpReqID = 0x1274;    //领取体力
    public const uint RecieveFriendHpRespID = 0x1275;
    public const uint GetFriendsListReqID = 0x1276;     //获取好友列表
    public const uint GetFriendsListRespID = 0x1277;
    public const uint GetAppendFriendsListReqID = 0x1278;		//获取好友申请列表
    public const uint GetAppendFriendsListRespID = 0x1279;
    public const uint AuditAppendFriendsReqID = 0x127A;    //处理好友申请
    public const uint AuditAppendFriendsRespID = 0x127B;
    public const uint SearchFriendsReqID = 0x127C;    //搜索好友
    public const uint SearchFriendsRespID = 0x127D;
    public const uint DelFriendsReqID = 0x127E;		//删除好友
    public const uint DelFriendsRespID = 0x127F;
    public const uint RefreshRecommendFriendsReqID = 0x1280;    //刷新推荐
    public const uint RefreshRecommendFriendsRespID = 0x1281;
    public const uint VerifyInviteCodeReqID = 0x128A;
    public const uint VerifyInviteCodeRespID = 0x128B;
    public const uint GetInviteCodeTaskListReqID = 0x128C;
    public const uint GetInviteCodeTaskListRespID = 0x128D;
    public const uint GetInviteCodeTaskAwardReqID = 0x128E;
    public const uint GetInviteCodeTaskAwardRespID = 0x128F;
    public const uint GetTopPlayerInfoReqID = 0x1282;    // 查看角色信息[最强阵容]
    public const uint GetTopPlayerInfoRespID = 0x1283;
    public const uint GetThirdFriendsInfoReqID = 0x1295;
    public const uint GetThirdFriendsInfoRespID = 0x1296;
    public const uint InviteThirdFriendsReqID = 0x1297;
    public const uint InviteThirdFriendsRespID = 0x1298;
    public const uint WakeThirdFriendsReqID = 0x1299;
    public const uint WakeThirdFriendsRespID = 0x129A;
    public const uint GetThirdFriendsTaskReqID = 0x129B;
    public const uint GetThirdFriendsTaskRespID = 0x129C;
    public const uint GetThirdFriendsTaskAwardReqID = 0x129D;
    public const uint GetThirdFriendsTaskAwardRespID = 0x129E;

    //奴役系统
    public const uint GetOtherPlayerInfoReqID = 0x0911;
    public const uint GetOtherPlayerInfoRespID = 0x0912;
    public const uint CollectMoneyReqID = 0x0913;
    public const uint CollectMoneyRespID = 0x0914;
    public const uint ReleaseEnslaveReqID = 0x0915;
    public const uint ReleaseEnslaveRespID = 0x0916;
    public const uint CollectAllMoneyReqID = 0x0917;
    public const uint CollectAllMoneyRespID = 0x0918;
    public const uint GetEnslaveRecordReqID = 0x0919;
    public const uint GetEnslaveRecordRespID = 0x090A;
    public const uint AskSaveEnslaveReqID = 0x090B;
    public const uint AskSaveEnslaveRespID = 0x090C;
    public const uint MatchStrangerReqID = 0x090D;
    public const uint MatchStrangerRespID = 0x090E;
    public const uint GetEnslaveInfoReqID = 0x0922;
    public const uint GetEnslaveInfoRespID = 0x0923;
    public const uint EnslaveFightBeforeReqID = 0x0924;
    public const uint EnslaveFightBeforeRespID = 0x0925;
    public const uint StartEnslaveFightReqID = 0x0926;
    public const uint StartEnslaveFightRespID = 0x0927;
    public const uint EnslaveFightOverReqID = 0x0928;
    public const uint EnslaveFightOverRespID = 0x0929;
    /// <summary>
    /// 退出奴隶准备
    /// </summary>
    public const uint EnslaveFightBeforeBackReqID = 0x092A;
    public const uint EnslaveFightBeforeBackRespID = 0x092B;
    public const uint GetEnemyPlayerReqID = 0x0930;
    public const uint GetEnemyPlayerRespID = 0x0931;
    public const uint GetRunAwayPlayerReqID = 0x0932;
    public const uint GetRunAwayPlayerRespID = 0x0937;
    public const uint EnslaveLockedReqID = 0x0933;
    public const uint EnslaveLockedRespID = 0x0934;
    public const uint EnslaveUnLockedReqID = 0x0935;
    public const uint EnslaveUnLockedRespID = 0x0936;
    public const uint AskHelpReqID = 0x0938;
    public const uint AskHelpRespID = 0x0939;
    public const uint WorldSaveCheckReqID = 0x093A;
    public const uint WorldSaveCheckRespID = 0x093B;

    //设置
    public const uint ChangeCharnameReqID = 0x0347;
    public const uint ChangeCharnameRespID = 0x0348;
    public const uint UseCDKeyReqID = 0x0349;
    public const uint UseCDKeyRespID = 0x034A;
    public const uint SuggestReqID = 0x034B;
    public const uint SuggestRespID = 0x034C;
    public const uint GetIconListReqID = 0x033F;
    public const uint GetIconListRespID = 0x033E;
    public const uint ChangeIconReqID = 0x033D;
    public const uint ChangeIconRespID = 0x033C;
    public const uint UnblockedPlayerReqID = 0x033B;
    public const uint UnblockedPlayerRespID = 0x033A;
    public const uint BlockedPlayerReqID = 0x0339;
    public const uint BlockedPlayerRespID = 0x0338;
    public const uint GetScreenInfoListReqID = 0x0337;
    public const uint GetScreenInfoListRespID = 0x0336;


    //BackPack
    public const uint SellItemReqID = 0x0102;
    public const uint SellItemRespID = 0x0103;
    public const uint UseItemReqID = 0x0104;
    public const uint UseItemRespID = 0x0105;
    public const uint ChipCompositeReqID = 0x0106;
    public const uint ChipCompositeRespID = 0x0107;
    /// <summary>
    /// 购买背包格子
    /// </summary>
    public const uint BuyEquipBagReqID = 0x0400;
    public const uint BuyEquipBagRespID = 0x0401;

    /// <summary>
    /// 购买体力
    /// </summary>
    public const uint BuyPHPowerReqID = 0x0324;
    public const uint BuyPHPowerRespID = 0x0325;


    // Equip 0x0600
    public const uint PutonEquipReqID = 0x0060;
    public const uint PutonEquipRespID = 0x0061;
    public const uint GetoffEquipReqID = 0x0062;
    public const uint GetoffEquipRespID = 0x0063;
    public const uint PromoteEquipReqID = 0x0064;
    public const uint PromoteEquipRespID = 0x0065;
    public const uint OneKeyPutOnAllReqID = 0x0066;
    public const uint OneKeyPutOnAllRespID = 0x0067;
    public const uint OneKeyPromoteAllReqID = 0x0068;
    public const uint OneKeyPromoteAllRespID = 0x0069;
    public const uint OneKeyPromoteOneReqID = 0x006A;
    public const uint OneKeyPromoteOneRespID = 0x006B;
    public const uint SoldierEquipStarReqID = 0x0070;
    public const uint SoldierEquipStarRespID = 0x0071;
    public const uint SellEquipReqID = 0x0072;
    public const uint SellEquipRespID = 0x0073;
    public const uint EquipUpQualityReqID = 0x0076;
    public const uint EquipUpQualityRespID = 0x0077;
    public const uint OneExtractEquipReqID = 0x1200;
    public const uint OneExtractEquipRespID = 0x1201;
    public const uint MultipleExtractReqID = 0x1202;
    public const uint MultipleExtractRespID = 0x1203;
    public const uint OneKeyOffEquipReqID = 0x1240;
    public const uint OneKeyOffEquipRespID = 0x1241;

    public const uint NotifyServerErrorID = 0x0033; //服务器报错//

    public const uint RecycleReqID = 0x1550;
    public const uint RecycleRespID = 0x1551;


    //=======================关卡=====================//
    /// <summary>
    /// 重新开始战斗-发送
    /// </summary>
    public const uint MajorMapRestartReqID = 0x0861;
    /// <summary>
    /// 重新开始战斗-接收
    /// </summary>
    public const uint MajorMapRestartRespID = 0x0862;
    /// <summary>
    /// 购买次数-发送
    /// </summary>
    public const uint BuyChallengeTimesReqID = 0x0863;
    /// <summary>
    /// 购买次数-接收
    /// </summary>
    public const uint BuyChallengeTimesRespID = 0x0864;
    /// <summary>
    /// 章节开放时间-发送
    /// </summary>
    public const uint ChapterOpenTimeReqID = 0x0865;
    /// <summary>
    /// 章节开放时间-接收
    /// </summary>
    public const uint ChapterOpenTimeRespID = 0x0866;
    /// <summary>
    /// 匹配敌方数据 -发送
    /// </summary>
    public const uint MatchEnemyReqID = 0x0869;
    /// <summary>
    /// 匹配敌方数据 -接收
    /// </summary>
    public const uint MatchEnemyRespID = 0x0870;
    /// <summary>
    /// 开始远征 - 发送
    /// </summary>
    public const uint StartExpeditionReqID = 0x0871;
    /// <summary>
    /// 开始远征 - 接收
    /// </summary>
    public const uint StartExpeditionRespID = 0x0872;
    /// <summary>
    /// 远征结算 - 发送 
    /// </summary>
    public const uint ExpeditionResultReqID = 0x0873;
    /// <summary>
    /// 远征结算 - 接收
    /// </summary>
    public const uint ExpeditionResultRespID = 0x0874;
    /// <summary>
    /// 远征奖励领取 - 发送
    /// </summary>
    public const uint ExpeditionRewardReqID = 0x0875;
    /// <summary>
    /// 远征奖励领取 - 接收
    /// </summary>
    public const uint ExpeditionRewardRespID = 0x0876;
    /// <summary>
    /// 进入PVE 
    /// </summary>
    public const uint DungeonInfoReqID = 0x0270;
    /// <summary>
    /// 进入PVE 
    /// </summary>
    public const uint DungeonInfoRespID = 0x0271;
    /// <summary>
    /// 开始PVE
    /// </summary>
    public const uint DungeonStartReqID = 0x0272;
    /// <summary>
    /// 开始PVE
    /// </summary>
    public const uint DungeonStartRespID = 0x0273;
    /// <summary>
    /// 主线副本扫荡
    /// </summary>
    public const uint MopupDungeonReqID = 0x0274;
    /// <summary>
    /// 主线副本扫荡
    /// </summary>
    public const uint MopupDungeonRespID = 0x0275;
    /// <summary>
    /// PVE结算
    /// </summary>
    public const uint DungeonRewardReqID = 0x0276;
    /// <summary>
    /// PVE结算
    /// </summary>
    public const uint DungeonRewardRespID = 0x0277;
    /// <summary>
    /// 无尽结算
    /// </summary>
    public const uint EndlessDungeonRewardReqID = 0x0278;
    /// <summary>
    /// 无尽结算
    /// </summary>
    public const uint EndlessDungeonRewardRespID = 0x0279;
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    public const uint DungeonStarRewardReqID = 0x027A;
    /// <summary>
    /// 主线副本星级奖励
    /// </summary>
    public const uint DungeonStarRewardRespID = 0x028B;
    /// <summary>
    /// 主线副本购买次数
    /// </summary>
    public const uint BuyDungeonTimesReqID = 0x028C;
    /// <summary>
    /// 主线副本购买次数
    /// </summary>
    public const uint BuyDungeonTimesRespID = 0x028D;
    /// <summary>
    /// 兵伐不臣购买次数
    /// </summary>
    public const uint BuyOtherDungeonTimesReqID = 0x028E;
    public const uint BuyOtherDungeonTimesRespID = 0x028F;


    /// <summary>
    /// 换装
    /// </summary>
    public const uint OneKeyReplaceEquipReqID = 0x0074;
    /// <summary>
    /// 换装
    /// </summary>
    public const uint OneKeyReplaceEquipRespID = 0x0075;
    //======================================================PVP================================================//
    /// <summary>
    /// 进入竞技场大厅 -发送
    /// </summary>
    public const uint EnterArenaLobbyReqID = 0x0250;
    /// <summary>
    /// 进入竞技场大厅 -接收
    /// </summary>
    public const uint EnterArenaLobbyRespID = 0x0251;
    /// <summary>
    /// 重新刷新对手 -发送
    /// </summary>
    public const uint RefreshOpponentsReqID = 0x0252;
    /// <summary>
    /// 重新刷新对手 -接收
    /// </summary>
    public const uint RefreshOpponentsRespID = 0x0253;
    /// <summary>
    /// 重置竞技场CD时间 
    /// </summary>
    public const uint ClearArenaCDReqID = 0x0254;
    /// <summary>
    /// 重置竞技场CD时间 
    /// </summary>
    public const uint ClearArenaCDRespID = 0x0255;
    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public const uint AddArenaTimesReqID = 0x0256;
    /// <summary>
    /// 增加竞技场次数
    /// </summary>
    public const uint AddArenaTimesRespID = 0x0257;
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public const uint ArenaRankListReqID = 0x0258;
    /// <summary>
    /// 请求竞技场列表
    /// </summary>
    public const uint ArenaRankListRespID = 0x0259;
    /// <summary>
    /// 竞技场战斗日志
    /// </summary>
    public const uint ArenaRecordReqID = 0x025A;
    /// <summary>
    /// 竞技场战斗日志
    /// </summary>
    public const uint ArenaRecordRespID = 0x025B;
    /// <summary>
    /// 竞技场开始
    /// </summary>
    public const uint StartArenaReqID = 0x025C;
    /// <summary>
    /// 竞技场开始
    /// </summary>
    public const uint StartArenaRespID = 0x025D;
    /// <summary>
    /// 竞技场结算
    /// </summary>
    public const uint ArenaRewardReqID = 0x025E;
    /// <summary>
    /// 竞技场结算
    /// </summary>
    public const uint ArenaRewardRespID = 0x025F;
    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public const uint SaveDefenceArrayReqID = 0x0260;
    /// <summary>
    /// 保存防御阵容
    /// </summary>
    public const uint SaveDefenceArrayRespID = 0x0261;
    /// <summary>
    /// 演舞台挑战
    /// </summary>
    public const uint ArenaChallengeReqID = 0x0262;
    public const uint ArenaChallengeRespID = 0x0263;
    //======================================================招募================================================//

    /// <summary>
    /// 单次招募请求
    /// </summary>
    public const uint OneRecruitReqID = 0x0300;

    /// <summary>
    /// 单次招募回复
    /// </summary>
    public const uint OneRecruitRespID = 0x0301;

    /// <summary>
    /// 多次招募请求
    /// </summary>
    public const uint MultipleRecruitReqID = 0x0302;

    public const uint MultipleRecruitRespID = 0x0303;

    /// <summary>
    /// 更新商店
    /// </summary>
    public const uint UpdateShopReqID = 0x0320;
    public const uint UpdateShopRespID = 0x0321;
    /// <summary>
    /// 购买商品
    /// </summary>
    public const uint BuyCommodityReqID = 0x0322;
    public const uint BuyCommodityRespID = 0x0323;

    public const uint BuySpecialCommodityReqID = 0x1230;
    public const uint BuySpecialCommodityRespID = 0x1231;

    public const uint BuyAllCommondityReqID = 0x0326;
    public const uint BuyAllCommondityRespID = 0x0327;
    //======================================================邮件================================================//

    public const uint GetMailReqID = 0x0706;
    public const uint GetMailRespID = 0x0707;
    public const uint ReadMailReqID = 0x0708;
    public const uint ReadMailRespID = 0x0709;
    public const uint GetMailAttReqID = 0x070A;
    public const uint GetMailAttRespID = 0x070B;
    public const uint NotifyNewMailID = 0x070C;

    public const uint DeleteMailReqID = 0x0710;
    public const uint DeleteMailRespID = 0x0711;


    public const uint OneKeyReadMailReqID = 0x0712;
    public const uint OneKeyReadMailRespID = 0x0713;

    //=====================================================任务================================================//
    /// <summary>
    /// 请求任务列表
    /// </summary>
    public const uint GetTaskListReqID = 0x0340;

    public const uint GetTaskListRespID = 0x0341;

    /// <summary>
    ///请求领取任务奖励 
    /// </summary>
    public const uint GetTaskAwardsReqID = 0x0342;

    public const uint GetTaskAwardsRespID = 0x0343;

    /// <summary>
    /// 任务推送
    /// </summary>
    public const uint NotifyRefreshTaskID = 0x0344;

    /// <summary>
    /// 充值
    /// </summary>
    public const uint RechargeReqID = 0x0037;
    public const uint RechargeRespID = 0x0038;

    /// <summary>
    /// 活跃度变化推送
    /// </summary>
    public const uint LivenessNotifyID = 0x0360;
    /// <summary>
    /// 刷新活跃度信息
    /// </summary>
    public const uint UpdateLivenessDataReqID = 0x0361;
    public const uint UpdateLivenessDataRespID = 0x0362;
    /// <summary>
    /// 活跃宝箱领取
    /// </summary>
    public const uint LivenessRewardReqID = 0x0363;
    public const uint LivenessRewardRespID = 0x0364;

    /// <summary>
    /// 连续登陆奖励领取
    /// </summary>
    public const uint ContinuAwardReqID = 0x0370;
    public const uint ContinuAwardRespID = 0x0371;
    /// <summary>
    /// 累计登陆奖励领取
    /// </summary>
    public const uint CumulativeAwardReqID = 0x0372;
    public const uint CumulativeAwardRespID = 0x0373;

    //=====================================================城堡================================================//
    /// <summary>
    /// 获取城堡信息
    /// </summary>
    public const uint GetCastleInfoReqID = 0x0350;
    public const uint GetCastleInfoRespID = 0x0351;
    /// <summary>
    /// 升级城堡
    /// </summary>
    public const uint UpgradeCastleReqID = 0x0352;
    public const uint UpgradeCastleRespID = 0x0353;
    /// <summary>
    /// 升级射手
    /// </summary>
    public const uint UpgradeShooterReqID = 0x0354;
    public const uint UpgradeShooterRespID = 0x0355;
    /// <summary>
    /// 解锁射手
    /// </summary>
    public const uint UnlockShooterReqID = 0x0356;
    public const uint UnlockShooterRespID = 0x0357;


    //=====================================================GM================================================//
    /// <summary>
    /// 修改数值
    /// </summary>
    public const uint GMCommandReqID = 0x0901;
    public const uint GMCommandRespID = 0x0902;
    /// <summary>
    /// 新手引导
    /// </summary>
    public const uint FinishGuideStepReqID = 0x0903;
    public const uint FinishGuideStepRespID = 0x0904;
    /// <summary>
    /// 新手引导开始
    /// </summary>
    public const uint StartGuideStepReqID = 0x0905;
    public const uint StartGuideStepRespID = 0x0906;

    /// <summary>
    /// 12点更新
    /// </summary>
    public const uint OnlineDayUpdateReqID = 0x0402;
    public const uint OnlineDayUpdateRespID = 0x0403;

    /// <summary>
    /// 活动
    /// </summary>
    public const uint ActivityRewardReqID = 0x0040;
    public const uint ActivityRewardRespID = 0x0041;
    public const uint BuyFundsReqID = 0x0042;
    public const uint BuyFundsRespID = 0x0043;
    public const uint QueryActivityReqID = 0x0044;
    public const uint QueryActivityRespID = 0x0045;
    public const uint QueryActivityTimeReqID = 0x1108;
    public const uint QueryActivityTimeRespID = 0x1109;

    /// <summary>
    /// 設置開關
    /// </summary>
    public const uint SetNotifyReqID = 0x1244;
    public const uint SetNotifyRespID = 0x1245;


    /// <summary>
    /// 新手引导选择关卡赠送物品
    /// </summary>
    public const uint SpecialItemReqID = 0x0893;
    public const uint SpecialItemRespID = 0x0894;


    /// <summary>
    /// 聊天
    /// </summary>
    public const uint SendChatReqID = 0x0888;
    public const uint SendChatRespID = 0x0889;
    /// <summary>
    /// 读取信息
    /// </summary>
    public const uint ReadChatInfoReqID = 0x088A;
    public const uint ReadChatInfoRespID = 0x088B;

    /// <summary>
    /// 请求玩家信息
    /// </summary>
    public const int GetPlayerInfoReqID = 0x088C;
    public const int GetPlayerInfoRespID = 0x088D;

    /// <summary>
    /// 聊天推送
    /// </summary>
    public const uint NotifyChatRefreshID = 0x0887;
    /// <summary>
    /// 跑马灯
    /// </summary>
    public const uint NotifyMarqueeID = 0x0017;

    //=================================军团===================================//

    public const uint OpenUnionReqID = 0x0810;
    public const uint OpenUnionRespID = 0x0811;

    public const uint CreateUnionReqID = 0x0812;
    public const uint CreateUnionRespID = 0x0813;

    public const uint JoinUnionReqID = 0x0814;
    public const uint JoinUnionRespID = 0x0815;

    public const uint LeaveUnionReqID = 0x0816;
    public const uint LeaveUnionRespID = 0x0817;

    public const uint UpdateUnionIconReqID = 0x0818;
    public const uint UpdateUnionIconRespID = 0x0819;

    public const uint UpdateUnionSettingReqID = 0x081A;
    public const uint UpdateUnionSettingRespID = 0x081B;

    public const uint ManageUnionMemberReqID = 0x081C;
    public const uint ManageUnionMemberRespID = 0x081D;

    public const uint QueryUnionReqID = 0x081E;
    public const uint QueryUnionRespID = 0x081F;

    public const uint VisitUnionMemberReqID = 0x0820;
    public const uint VisitUnionMemberRespID = 0x0821;

    public const uint UpdateUnionNameReqID = 0x0822;
    public const uint UpdateUnionNameRespID = 0x0823;

    public const uint AuditUnionMembersReqID = 0x0824;
    public const uint AuditUnionMembersRespID = 0x0825;

    public const uint DonateToUnionReqID = 0x0826;
    public const uint DonateToUnionRespID = 0x0827;

    public const uint GetUnionHpReqID = 0x0828;
    public const uint GetUnionHpRespID = 0x0829;

    public const uint NotifyPlayerJoinUnionID = 0x082A;

    public const uint LevelUpUnionReqID = 0x082B;
    public const uint LevelUpUnionRespID = 0x082C;

    public const uint PlayerUnionReqID = 0x082D;
    public const uint PlayerUnionRespID = 0x082E;

    public const uint AuditSingleUnionMemberReqID = 0x0824;
    public const uint AuditSingleUnionMemberRespID = 0x0825;
    public const uint AuditMultiUnionMemberReqID = 0x084D;
    public const uint AuditMultiUnionMemberRespID = 0x084E;

    /// <summary>
    /// 军团图标查询
    /// </summary>
    public const uint QueryUnionIconReqID = 0x0762;
    public const uint QueryUnionIconRespID = 0x0763;

    //=======================排行榜========================//
    public const uint RankInfoReqID = 0x1100;
    public const uint RankInfoRespID = 0x1101;
    public const uint RankPalyerInfoReqID = 0x1102;//查看玩家或军团信息//
    public const uint RankPlayerInfoRespID = 0x1103;

    //=======================异域探险========================//
    /// <summary>
    /// 打开异域探险
    /// </summary>
    public const uint OpenUnionPveDgnReqID = 0x084F;
    public const uint OpenUnionPveDgnRespID = 0x0850;
    /// <summary>
    /// 异域探险重置
    /// </summary>
    public const uint ResetUnionPveDgnReqID = 0x082F;
    public const uint ResetUnionPveDgnRespID = 0x0830;
    /// <summary>
    /// 开始异域探险
    /// </summary>
    public const uint StartUnionPveDgnReqID = 0x0831;
    public const uint StartUnionPveDgnRespID = 0x0832;
    /// <summary>
    /// 异域探险奖励
    /// </summary>
    public const uint UnionPveDgnRewardReqID = 0x0833;
    public const uint UnionPveDgnRewardRespID = 0x0834;
    /// <summary>
    /// 异域探险今日奖励
    /// </summary>
    public const uint UnionPveDgnTodayRwardReqID = 0x0835;
    public const uint UnionPveDgnTodayRwardRespID = 0x0836;
    /// <summary>
    /// 异域探险排行榜
    /// </summary>
    public const uint UnionPveDgnRankReqID = 0x0837;
    public const uint UnionPveDgnRankRespID = 0x0838;
    /// <summary>
    /// 查询异域探险boss状态
    /// </summary>
    public const uint QueryUnionPveDgnStateReqID = 0x0839;
    public const uint QueryUnionPveDgnStateRespID = 0x083A;

    public const uint CancelReadyUnionPveReqID = 0x0849;
    public const uint CancelReadyUnionPveRespID = 0x084A;

    /// <summary>
    /// 军团准备
    /// </summary>
    public const uint ReadyUnionPveDgnReqID = 0x083B;
    public const uint ReadyUnionPveDgnRespID = 0x083C;

    //============================军团争霸=========================//
    public const uint ApplyForUnionPvpReqID = 0x083D;       //申请军团争霸//
    public const uint ApplyForUnionPvpRespID = 0x083E;

    public const uint OpenUnionPvpReqID = 0x083F;           //
    public const uint OpenUnionPvpRespID = 0x0840;

    public const uint JoinUnionPvpReqID = 0x0841;           //加入军团争霸//
    public const uint JoinUnionPvpRespID = 0x0842;

    public const uint CancelUnionPvpReqID = 0x0843;       //取消军团争霸//
    public const uint CancelUnionPvpRespID = 0x0844;

    public const uint UnionPvpKillRankReqID = 0x0845;     //军团争霸击杀排行//
    public const uint UnionPvpKillRankRespID = 0x0846;

    public const uint UnionPvpRankReqID = 0x0847;       //军团排行//
    public const uint UnionPvpRankRespID = 0x0848;

    public const uint NotifyPlayerKickOutUnionID = 0x0851;//通知踢人//

    public const uint InvitePlayerJoinUnionReqID = 0x0852;//邀请人进入军团请求//

    public const uint InvitePlayerJoinUnionRespID = 0x0853;//邀请人进入军团回复//

    public const uint NotifyPlayerInvitedToUnionID = 0x0854;//通知邀请//

    public const uint OpenUnionCityReqID = 0x084B;       //查询城市请求//
    public const uint OpenUnionCityRespID = 0x084C;      //查询城市回复//

    public const uint FirstLoginAwardReqID = 0x110A;      //登陆即送奖励//
    public const uint FirstLoginAwardRespID = 0x110B;      //登陆即送奖励//



    public const uint FirstRechargeAwardReqID = 0x1104;//第一次充值奖励领取请求
    public const uint FirstRechargeAwardRespID = 0x1105;

    public const uint OnlineRewardReqID = 0x1106;		//在线奖励领取请求
    public const uint OnlineRewardRespID = 0x1107;		//在线奖励领取

    public const uint NotifyRechargeID = 0x0024;
    public const uint GetCommodityOrderNumReqID = 0x0404;
    public const uint GetCommodityOrderNumRespID = 0x0405;
    public const uint NotifyResetID = 0x0025;

    public const uint OnNeverRemindReqID = 0x1250;//5星评价不在提醒
    public const uint OnNeverRemindRespID = 0x1251;
    public const uint OnFiveStarCommentFinishReqID = 0x1252;//五星评价完成
    public const uint OnFiveStarCommentFinishRespID = 0x1253;


    //============================新手任务=========================//
    public const uint GetNewHandTasksReqID = 0x1220;
    public const uint GetNewHandTasksRespID = 0x1221;
    public const uint GetNewHandTasksAwardReqID = 0x1222;
    public const uint GetNewHandTasksAwardRespID = 0x1223;

    public const uint GetVipUpRewardReqID = 0x0039;
    public const uint GetVipUpRewardRespID = 0x003A;
    public const uint GetVipDailyRewardReqID = 0x003B;
    public const uint GetVipDailyRewardRespID = 0x003C;
    public const uint UpdateMaxViewCombatPowerID = 0x0027;

    public const uint BuildUnlockAnimeIdReqID = 0x1242;
    public const uint BuildUnlockAnimeIdRespID = 0x1243;

    //=====================================================全服霸主================================================//
    public const uint EnterOverlordReqID = 0x1501;  //进入霸主主界面;
    public const uint EnterOverlordRespID = 0x1502; //进入后返回的信息;
    public const uint BuyBuffReqID = 0x1503;        //购买buff;
    public const uint BuyBuffRespID = 0x1504;       //购买buff返回;
    public const uint ChallengeOverlordReqID = 0x1505;  //进入准备选取武将--检测cd;
    public const uint ChallengeOverlordRespID = 0x1506; //进入准备选取武将;
    public const uint StationReadyReqID = 0x1507;   //进入战斗;
    public const uint StationReadyRespID = 0x1508;  //返回允许战斗;
    public const uint buyCdReqID = 0x1509;      //购买CD;
    public const uint buyCdRespID = 0x150A;     //返回购买CD;
    public const uint worshipReqID = 0x150D;    //朝拜;
    public const uint worshipRespID = 0x150E;   //朝拜返回;
    public const uint BillingReqID = 0x150F;    //结算请求
    public const uint BillingRespID = 0x1510;   //结算返回

    //攻城略地
    public const uint GetCampaignTokenInfoReqID = 0x1310;
    public const uint GetCampaignTokenInfoRespID = 0x1311;
    public const uint GetCampaignRankReqID = 0x1312;
    public const uint GetCampaignRankRespID = 0x1313;
    public const uint ActivateTokenReqID = 0x1314;
    public const uint ActivateTokenRespID = 0x1315;
    public const uint ClearCampaignCDReqID = 0x1316;
    public const uint ClearCampaignCDRespID = 0x1317;
    public const uint StartCampaignPvpReqID = 0x1318;
    public const uint StartCampaignPvpRespID = 0x1319;
    public const uint CampaignRewardReqID = 0x131A;
    public const uint CampaignRewardRespID = 0x131B;
    public const uint AssignChestReqID = 0x131C;
    public const uint AssignChestRespID = 0x131D;
    public const uint GetScoreInfoReqID = 0x131E;
    public const uint GetScoreInfoRespID = 0x131F;
    //信仰神坛
    public const uint QueryAltarReqID = 0x0750;
    public const uint QueryAltarRespID = 0x0751;
    public const uint MatchAltarUnionReqID = 0x0752;
    public const uint MatchAltarUnionRespID = 0x0753;
    public const uint AltarHandleDependReqID = 0x0754;
    public const uint AltarHandleDependRespID = 0x0755;
    public const uint AltarFightReqID = 0x0756;
    public const uint AltarFightRespID = 0x0757;
    public const uint LockEnemyRebelUnionReqID = 0x0758;
    public const uint LockEnemyRebelUnionRespID = 0x0759;
    public const uint QueryAltarRecordReqID = 0x075A;
    public const uint QueryAltarRecordRespID = 0x075B;
    public const uint DeleteDependUnionReqID = 0x075C;
    public const uint DeleteDependUnionRespID = 0x075D;
    public const uint AltarSerachUnionReqID = 0x075E;
    public const uint AltarSerachUnionRespID = 0x075F;
    public const uint QueryAltarRecruitReqID = 0x0760;
    public const uint QueryAltarRecruitRespID = 0x0761;


    //=======================排位赛=========================//
    /// <summary>
    /// 进入排位赛
    /// </summary>
    public const uint EnterPoleLobbyReqID = 0x1520;
    public const uint EnterPoleLobbyRespID = 0x1521;
    /// <summary>
    /// 排位赛阵容调整
    /// </summary>
    public const uint SavePoleArrayReqID = 0x1522;
    public const uint SavePoleArrayRespID = 0x1523;
    /// <summary>
    /// 开始匹配
    /// </summary>
    public const uint StartPoleReqID = 0x1524;
    public const uint StartPoleRespID = 0x1525;
    /// <summary>
    /// 战斗结算
    /// </summary>
    public const uint EndPoleReqID = 0x1526;
    public const uint EndPoleRespID = 0x1527;
    /// <summary>
    /// 领奖(日奖、周奖、赛季奖)协议
    /// </summary>
    public const uint PoleRewardReqID = 0x1528;
    public const uint PoleRewardRespID = 0x1529;
    /// <summary>
    /// //请求战斗日志
    /// </summary>
    public const uint PoleRecordReqID = 0x152A;
    public const uint PoleRecordRespID = 0x152B;
    public const uint PoleRevengeReqID = 0x152C;
    public const uint PoleRevengeRespID = 0x152D;
    public const uint PoleBuyTimesReqID = 0x152E;
    public const uint PoleBuyTimesRespID = 0x152F;

    public const uint MatchPoleReqID = 0x1530;
    public const uint MatchPoleRespID = 0x1531;
    public const uint ClearPoleMatchCDReqID = 0x1532;
    public const uint ClearPoleMatchCDRespID = 0x1533;



    //=====================成就系统============================//
    public const uint EnterAchievementReqID = 0x1534;
    public const uint EnterAchievementRespID = 0x1535;
    public const uint GetAchievementReqID = 0x1536;
    public const uint GetAchievementRespID = 0x1537;
    public const uint EnterFrameReqID = 0x1538;
    public const uint EnterFrameRespID = 0x1539;
    public const uint EnterIconReqID = 0x153A;
    public const uint EnterIconRespID = 0x153B;


    //===================命魂================//
    public const uint PutOnSoulReqID = 0x1540;
    public const uint PutOnSoulRespID = 0x1541;
    public const uint TakeOffSoulReqID = 0x1542;
    public const uint TakeOffSoulRespID = 0x1543;
    public const uint UpgradeSoulReqID = 0x1544;
    public const uint UpgradeSoulRespID = 0x1545;
    public const uint SellSoulReqID = 0x1546;
    public const uint SellSoulRespID = 0x1547;
    public const uint BuySoulGridReqID = 0x1548;
    public const uint BuySoulGridRespID = 0x1549;
    public const uint ExploreSoulReqID = 0x154A;
    public const uint ExploreSoulRespID = 0x154B;
    public const uint CollectSoulReqID = 0x154C;
    public const uint CollectSoulRespID = 0x154D;

    //===================宠物系统================//
    public const uint DressPetReqID = 0x1560;
    public const uint DressPetRespID = 0x1561;
    public const uint ShowMountReqID = 0x1562;
    public const uint ShowMountRespID = 0x1563;
    public const uint PromotePetReqID = 0x1564;
    public const uint PromotePetRespID = 0x1565;
    public const uint UpgradePetSKillReqID = 0x1566;
    public const uint UpgradePetSKillRespID = 0x1567;
	
	    //=========================跨服战=====================//
    public const uint EnterCrossServerWarReqID = 0x1568;
    public const uint EnterCrossServerWarRespID = 0x1569;
    public const uint TileRankReqID = 0x156A;
    public const uint TileRankRespID = 0x156B;
    public const uint EnterServerAndAwardReqID = 0x156C;
    public const uint EnterServerAndAwardRespID = 0x156D;
    public const uint BuyCDClearReqID = 0x1570;
    public const uint BuyCDClearRespID = 0x1571;
    public const uint GotoBattleReqID = 0x1572;
    public const uint GotoBattleRespID = 0x1573;
    public const uint CombatSettlementReqID = 0x1574;
    public const uint CombatSettlementRespID = 0x1575;
}
