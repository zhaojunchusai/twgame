using UnityEngine;
using System;
using System.Collections;
using System.Reflection;

/// <summary>
/// 错误处理[接受服务端发送信息]
/// </summary>
/// modify by taiwei 
/// date: 2015-06-29 16:36
public static class ErrorCode
{
    public static void ShowErrorTip(uint code)
    {
        ShowErrorTip((int)code);
    }
    /// <summary>
    /// 根据错误码提示自动弹出对应的提示消息
    /// </summary>
    /// <param name="code">错误码</param>
    public static void ShowErrorTip(int code)
    {
        string mes = "";
        mes = GetString(code);
        if (!string.IsNullOrEmpty(mes))
        {
            if ((ErrorCodeEnum)code != ErrorCodeEnum.OFFLINE)
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, mes);
            else
            {
                CommonFunction.ShowOfflineTip();
            }
        }
    }

    /// <summary>
    /// 获取错误描述
    /// </summary>
    public static string GetString(int code)
    {
        string msg = "";

        if (ConfigManager.Instance != null && ConfigManager.Instance.mErrorCodeConfig != null)
            msg = ConfigManager.Instance.mErrorCodeConfig.GetTipString((uint)code);

        if (msg != "")
            return msg;

        switch ((ErrorCodeEnum)code)
        {
            case ErrorCodeEnum.DataUnusual: msg = "數據異常"; break;
            case ErrorCodeEnum.ConfigUnusual: msg = "配置異常"; break;
            case ErrorCodeEnum.NotEnoughGold: msg = "銅錢不足"; break;
            case ErrorCodeEnum.NotEnoughDiamond: msg = "元寶不足"; break;
            case ErrorCodeEnum.NotEnoughphysical:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_SP_VIEW);
                    UISystem.Instance.BuySPView.InitUI();
                }
                break;
            case ErrorCodeEnum.OverItem: msg = "物品達到上限"; break;
            case ErrorCodeEnum.ItemCanNotUse: msg = "物品禁止使用"; break;
            case ErrorCodeEnum.HadEquipHerr: msg = "該裝備已經穿在該位置"; break;
            case ErrorCodeEnum.HadUninstall: msg = "裝備已經卸下"; break;
            case ErrorCodeEnum.EquipTypeError: msg = "不同類型的裝備(裝備部位出錯)"; break;
            case ErrorCodeEnum.HadMaxLevel: msg = "裝備等級達到上限，請升星裝備提高上限"; break;
            case ErrorCodeEnum.SoldierMaxLevel: msg = "武將等級達到上限"; break;
            case ErrorCodeEnum.SkillUpToTop: msg = "技能達到最高等級"; break;
            case ErrorCodeEnum.SoldierNotHaveEquip: msg = "武將沒有穿裝備"; break;
            case ErrorCodeEnum.SoldierNotHaveEquipProme: msg = "武將沒有可強化的裝備"; break;
            case ErrorCodeEnum.NotEnoughPlayerLevel: msg = "玩家等級不足"; break;
            case ErrorCodeEnum.CanotSail: msg = "不能出售"; break;
            case ErrorCodeEnum.NotEnoughSoldierLevel: msg = "武將材料等級不足"; break;
            case ErrorCodeEnum.NextIDError: msg = "後置ID錯誤"; break;
            case ErrorCodeEnum.NotEnoughMaterial: msg = "材料不足"; break;
            case ErrorCodeEnum.SoldierEquipFull: msg = "裝備數量將超出裝備背包上限,請清理背包"; break;
            case ErrorCodeEnum.NotEnoughItem: msg = "道具不足"; break;
            case ErrorCodeEnum.VIPLvNotEnough: msg = "vip等級不足"; break;
            case ErrorCodeEnum.TimesLimited: msg = "剩餘挑戰次數不足"; break;
            case ErrorCodeEnum.Medal_Insufficient: msg = "勳章不足"; break;
            case ErrorCodeEnum.Honor_Insufficient: msg = "戰功不足"; break;
            case ErrorCodeEnum.ShopUpdateTimeUndue: msg = "免費刷新時間未到"; break;
            case ErrorCodeEnum.GoodsHadSellOut: msg = "該物品已購買"; break;
            case ErrorCodeEnum.HadGetLivenessAward: msg = "已經領取此活躍寶箱不能再領取"; break;
            case ErrorCodeEnum.LivenessNotEnough: msg = "領取此活躍寶箱活躍度不夠"; break;
            case ErrorCodeEnum.HadGetCumulativeLoginAward: msg = "已經領取了該累計登入天數獎勵"; break;
            case ErrorCodeEnum.HadGetContinuLoginAward: msg = "已經領取了連續登入獎勵"; break;
            case ErrorCodeEnum.SuggestTimesUseup: msg = "回饋次數非法"; break;
            case ErrorCodeEnum.CharnameIllegal: msg = "改名非法"; break;
            case ErrorCodeEnum.ExchangeGoldTimesOver: msg = ConstString.EXCHANGE_GOLD_ALLUSEOUT; break;
            case ErrorCodeEnum.HadMonthCard: msg = "已經購買了月卡不能再買"; break;
            case ErrorCodeEnum.CDKeyEarly: msg = "序號尚未開放"; break;
            case ErrorCodeEnum.CDKeyInputEmpty: msg = "序號輸入為空"; break;
            case ErrorCodeEnum.CDKeyInvalid: msg = "無效序號，請驗證後輸入"; break;
            case ErrorCodeEnum.CDKeyPastDue: msg = "序號已過期，請驗證後輸入"; break;
            case ErrorCodeEnum.CDKeyUsed: msg = "序號已使用，請驗證後輸入"; break;
            case ErrorCodeEnum.BuyPHPowerUsedOver: msg = "今日購買體力次數已使用完"; break;
            case ErrorCodeEnum.OverMaxPHPower: msg = "超過體力最大上限"; break;
            case ErrorCodeEnum.NoSuchAccount: msg = "帳號不存在"; break;
            case ErrorCodeEnum.DBInsertFailed: msg = ConstString.ERR_DB_INSERT_FAILED; break;
            case ErrorCodeEnum.AccountPasswordEmpty: msg = ConstString.ERR_ACCOUNT_PASSWORD_EMPTY; break;
            case ErrorCodeEnum.AccountLengthWrong: msg = ConstString.ERR_ACCNAME_LENGTH_WRONG; break;
            case ErrorCodeEnum.PasswordLengthWrong: msg = ConstString.ERR_PASSWORD_LENGTH_WRONG; break;
            case ErrorCodeEnum.REPEATNICKNAME: msg = "角色名已存在"; break;
            case ErrorCodeEnum.PasswordWrong: msg = "密碼錯誤"; break;
            case ErrorCodeEnum.HeroLevelLower: msg = "英雄等級不足"; break;
            case ErrorCodeEnum.SoldierLevelLower: msg = "武將等級不足"; break;
            case ErrorCodeEnum.ForbiddenWorld: msg = "該暱稱含敏感詞彙，無法使用"; break;
            case ErrorCodeEnum.PlayerArenaRankChange: msg = ConstString.PVP_PLAYERARENARANKCHANGE; break;
            case ErrorCodeEnum.ChallengeTimesNotEnough: msg = ConstString.GATE_CHALLENGETIMESNOTENOUGH; break;
            case ErrorCodeEnum.MopupItemNotEnough: msg = "掃蕩卡不足"; break;
            case ErrorCodeEnum.PwdOrNameEasy: msg = "密碼或者帳號過於簡單"; break;
            case ErrorCodeEnum.OFFLINE: msg = ConstString.OFFLINE_TIP; break;
            case ErrorCodeEnum.OpenTimeLimited: msg = ConstString.ACTIVITIES_LOCKTIME; break;
            case ErrorCodeEnum.SameTypeSolider: msg = ConstString.GATE_SAMETYPESOLDIER; break;
            case ErrorCodeEnum.PlayerOptInCD: msg = ConstString.PVP_PLAYERINCD; break;
            case ErrorCodeEnum.OpponentRankTooLow: msg = "對手排名太低"; break;
            case ErrorCodeEnum.PrevDungeonNotPass: msg = ConstString.GATE_PREGATEUNLOCK; break;
            case ErrorCodeEnum.BuyTimesLimited: msg = ConstString.GATE_PURCHASECOUNTLIMIT; break;
            case ErrorCodeEnum.PlayerBuyFundsAlready: msg = ConstString.GAMEACTIVTIY_FOUND_ALREADYBUYFUND; break;
            case ErrorCodeEnum.ActivityNotFinish: msg = ConstString.GAMEACTIVTIY_REWAED_NOTFINISH; break;
            case ErrorCodeEnum.ActivityRewardAlready: msg = ConstString.GAMEACTIVTIY_REWAED_ALREADYRECIEVED; break;
            case ErrorCodeEnum.NotReachExchangeCondition: msg = ConstString.GAMEACTIVTIY_REWAED_NOTREACHCONDITION; break;
            case ErrorCodeEnum.WorldChatTimeInCD: msg = ConstString.CHATVIEW_ERROCODE_CDTIME; break;
            case ErrorCodeEnum.UnionNotExist: msg = "你查找的軍團不存在"; break;
            case ErrorCodeEnum.ApplyForUnionTimesLimited: msg = "今日軍團申請次數已用完"; break;
            case ErrorCodeEnum.UnionNameExist: msg = "軍團名已存在"; break;
            case ErrorCodeEnum.UnionNameIllegal: msg = "軍團名非法"; break;
            case ErrorCodeEnum.NotHaveUnion: msg = "玩家沒有軍團"; break;
            case ErrorCodeEnum.NoPrivilege: msg = "玩家沒有許可權操作"; break;
            case ErrorCodeEnum.NotJoinTheUnion: msg = "玩家沒有加入對應的軍團"; break;
            case ErrorCodeEnum.ChairmanCannotExitUnion: msg = "軍團長無法退出軍團"; break;
            case ErrorCodeEnum.HaveJoinedUnionAready: msg = "玩家已經加入軍團"; break;
            case ErrorCodeEnum.PendingUnionApplication: msg = "軍團申請待審核"; break;
            case ErrorCodeEnum.UnionIsFull: msg = "軍團人數已達上限"; break;
            case ErrorCodeEnum.MaxUnionPromotionTimes: msg = "今日軍團2個升職名額已用盡"; break;
            case ErrorCodeEnum.MaxUnionAbdicateTimes: msg = "今日軍團讓位機會已用盡"; break;
            case ErrorCodeEnum.MaxUnionKickMemberTimes: msg = "軍團長每日開除成員上限3名，無法再開除"; break;
            case ErrorCodeEnum.VMaxUnionKickMemberTimes: msg = "副軍團長每日開除成員上限1名，無法再開除"; break;
            case ErrorCodeEnum.ViceChairmanReachMax: msg = "副軍團長達到上限"; break;
            case ErrorCodeEnum.CannotPromotion: msg = "不必升職"; break;
            case ErrorCodeEnum.CannotDemotion: msg = "玩家不能降職"; break;
            case ErrorCodeEnum.CannotAbdicate: msg = "無法退位"; break;
            case ErrorCodeEnum.MaxDonateTimes: msg = "達到最大捐獻次數"; break;
            case ErrorCodeEnum.ExceedMaxVisitTimes: msg = "超過最大拜訪次數"; break;
            case ErrorCodeEnum.HaveVisitedAready: msg = "已經拜訪過該玩家"; break;
            case ErrorCodeEnum.HadBlockedPlayer: msg = "已遮罩過該玩家"; break;
            case ErrorCodeEnum.BlockedNumFull: msg = "超出遮罩玩家上限"; break;
            case ErrorCodeEnum.DungeonCannotMopup: msg = "該副本無法進行掃蕩"; break;
            case ErrorCodeEnum.ApplyForUnionAready: msg = "已經申請軍團"; break;
            case ErrorCodeEnum.UnionDgnIsChallenged: msg = "該BOSS正被其他玩家挑戰中"; break;
            case ErrorCodeEnum.NoPlayerToMatch: msg = "奴役匹配陌生人沒有可匹配的玩家"; break;
            case ErrorCodeEnum.PrisonLocked: msg = "該囚牢未解鎖"; break;
            case ErrorCodeEnum.PrisonFul: msg = "該囚牢已經有奴隸了"; break;
            case ErrorCodeEnum.InEnslaveFight: msg = "正在奴役戰鬥中"; break;
            case ErrorCodeEnum.EnslaveStatusChange: msg = "奴役狀態改變"; break;
            case ErrorCodeEnum.EnslaveFightTimeOut: msg = "奴役戰鬥準備時間超時"; break;
            case ErrorCodeEnum.EnslaveHostChange: msg = "奴隸主改變"; break;
            case ErrorCodeEnum.PrisonIsNull: msg = "囚牢是空的"; break;
            case ErrorCodeEnum.PrisonMoneyZero: msg = "囚牢沒有可收取的銅錢"; break;
            case ErrorCodeEnum.NoneEnemyPlayer: msg = "沒有仇人"; break;
            case ErrorCodeEnum.NoneRunawayPlayer: msg = "沒有逃奴"; break;
            case ErrorCodeEnum.NotHavePlayerInfo: msg = "沒有該玩家的逃奴或仇人資訊"; break;
            case ErrorCodeEnum.NotLockedPlayer: msg = "沒有鎖定該玩家"; break;
            case ErrorCodeEnum.AskHelpTimeOut: msg = "請求救援時間間隔不足"; break;
            case ErrorCodeEnum.PlayerHaveLeaveUnion: msg = "玩家已經離開軍團"; break;
            case ErrorCodeEnum.MaxUnionLevel: msg = "最大軍團等級了，無法再升級"; break;
            case ErrorCodeEnum.UnionResourceNotEnough: msg = "軍團物資不夠"; break;
            case ErrorCodeEnum.CannotGetRewards: msg = "領取條件不足，無法領取獎勵"; break;
            case ErrorCodeEnum.NoRecharge: msg = "領取條件不足，請儲值後領取"; break;
            case ErrorCodeEnum.HadGetFirstRechargeAward: msg = "已經領取過首儲禮包"; break;
            case ErrorCodeEnum.UnionTokenNotEnough: msg = ConstString.HINT_NO_TOKEN; break;
            case ErrorCodeEnum.WillTimeOver: msg = "玩家即將滿24小時"; ; break;
            case ErrorCodeEnum.PlayerIsYouEnslave: msg = "無法解救，該玩家是您的奴隸，可在囚牢中釋放該玩家"; break;
            case ErrorCodeEnum.PlayerIsSameHolder: msg = "玩家與你同一奴隸主，不能解救"; break;
            case ErrorCodeEnum.DiffrentLevelToMuch: msg = "等級差異過大（10級），不能奴役"; break;
            case ErrorCodeEnum.AskHelpTimeOver: msg = "救援請求已過期"; break;
            case ErrorCodeEnum.NotApplyUnionClashTime: msg = "無法報名，不在報名期間"; break;
            case ErrorCodeEnum.ErrorState: msg = "狀態不對"; break;
            case ErrorCodeEnum.HaveJoinedUnionClash: msg = "玩家已經參加軍團戰"; break;
            case ErrorCodeEnum.NotJoinedUnionClash: msg = "玩家沒有參加軍團戰"; break;
            case ErrorCodeEnum.SoldierMaxNum: msg = "擁有武將數量達到上限300,操作失敗"; break;
            case ErrorCodeEnum.HavePassedUnionPve: msg = "已通關當前軍團副本關卡"; break;
            case ErrorCodeEnum.EnterTimesLimited: msg = "當前小關卡已通關,不能再次挑戰"; break;
            case ErrorCodeEnum.ExceedMaxGetHpTimes: msg = "玩家超過最大領取體力次數"; break;
            case ErrorCodeEnum.InformationExpired: msg = "玩家的審核信息已過期"; break;
            case ErrorCodeEnum.CanNotEnslaveHolder: msg = "不能奴役奴隸主"; break;
            case ErrorCodeEnum.GetNothing: msg = "沒有可領取的體力"; break;
            case ErrorCodeEnum.PlayerOffline: msg = "目標玩家不在線上"; break;
            case ErrorCodeEnum.UionPVEMemberFight: msg = "當前有軍團成員正在進行挑戰，無法重置"; break;
            case ErrorCodeEnum.MaterialSoldierNotEnough: msg = "武將升級經驗不足"; break;
            case ErrorCodeEnum.GoldIsMax: msg = "銅錢達到最大值"; break;
            case ErrorCodeEnum.DiamondIsMax: msg = "元寶達到最大值"; break;
            case ErrorCodeEnum.PhPowerIsMax: msg = "體力達到最大值"; break;
            case ErrorCodeEnum.SoldierChipMaxNum: msg = "有武魂滿出,已發放到郵箱"; break;
            case ErrorCodeEnum.CastleLevelLow: msg = "等級已達上限"; break;
            case ErrorCodeEnum.FAILED: msg = "操作失敗"; break;
            case ErrorCodeEnum.QualityExpNotEnough: msg = "天將神兵經驗不足"; break;
            case ErrorCodeEnum.QualityWrong: msg = "品質錯誤"; break;
            case ErrorCodeEnum.ActivityNotInTime: msg = "活動已過期"; break;
            case ErrorCodeEnum.FrozenAccount: msg = "檢測到您的帳號有不當行為，現已封停.如有疑問請聯繫客服."; break;
            case ErrorCodeEnum.FrozenChat: msg = "您已被系統禁言,如有疑問請聯繫客服."; break;
            case ErrorCodeEnum.CDKPackUsed: msg = "該批次CDKey禮包已領過"; break;
            case ErrorCodeEnum.HasVipUpReward: msg = "該豪華禮包已被領取過"; break;
            case ErrorCodeEnum.SoldierNoEquipOn: msg = "武將沒有穿裝備"; break;
            case ErrorCodeEnum.GetVipDailySameDay: msg = "今天的每日獎勵已領取，請明天再來"; break;
            case ErrorCodeEnum.InBlockedList: msg = "該玩家被你屏蔽，無法添加為好友"; break;
            case ErrorCodeEnum.FriendsOverFlow: msg = "好友列表已滿，操作失敗"; break;
            case ErrorCodeEnum.ApplyFriendsRepeated: msg = "好友申請中有對方的申請，親處理后再操作"; break;
            case ErrorCodeEnum.ApplyNotDealed: msg = "你以前發送的申請對方還未處理，發送失敗"; break;
            case ErrorCodeEnum.ApplyNotDealedOther: msg = "你以前發送的申請對方還未處理，發送失敗"; break;
            case ErrorCodeEnum.DonateFriendsPhTimes: msg = "今日贈送體力次數用完"; break;
            case ErrorCodeEnum.RecieveFriendsPhTimes: msg = "今日領取體力次數用完"; break;
            case ErrorCodeEnum.NoRevieveHp: msg = "沒有可領取的體力"; break;
            case ErrorCodeEnum.FriendsPartOverFlow: msg = "你的好友清單已滿，部分好友添加失敗"; break;
            case ErrorCodeEnum.FriendsPartBlockList: msg = "部分玩家被你屏蔽，無法添加入好友列表"; break;
            case ErrorCodeEnum.FirendsCharNotExist: msg = "該角色不存在"; break;
            case ErrorCodeEnum.DonateFriendsPhAlready: msg = "今日已贈送此人體力"; break;
            case ErrorCodeEnum.VerifyInviteCodeNotExist: msg = "邀請碼不存在"; break;
            case ErrorCodeEnum.VerifyInviteCodeIllegal: msg = "不能輸入自己的邀請碼"; break;
            case ErrorCodeEnum.VerifyInviteCodeUsed: msg = "邀請碼已使用"; break;
            case ErrorCodeEnum.VerifyInviteCodeTaskStatus: msg = "邀請任務狀態錯誤"; break;
            case ErrorCodeEnum.ThirdFriendsInviteTime: msg = "邀請時間未到"; break;
            case ErrorCodeEnum.ThirdFriendsWakeupTime: msg = "玩家離線不足7天無法喚醒"; break;
            case ErrorCodeEnum.HadGotFirstReward: msg = "已經領取過新手豪禮"; break;
            case ErrorCodeEnum.RefreshFriendsCD: msg = "刷新推薦好友cd中"; break;
            case ErrorCodeEnum.ReachedPlayerMaxLevel: msg = "已達到英雄最大等級上限"; break;
            case ErrorCodeEnum.AltarHandleTimesNotEnough: msg = "神壇安慰,調戲次數不足"; break;
            case ErrorCodeEnum.DependUnionNotExist: msg = "依附軍團不存在"; break;
            case ErrorCodeEnum.DependPositionNotNil: msg = "依附位置不是空閒狀態"; break;
            case ErrorCodeEnum.AltarFightSelfUnion: msg = "不能攻擊自己軍團"; break;
            case ErrorCodeEnum.RevoltNotDependHost: msg = "反抗軍團不再是依附主"; break;
            case ErrorCodeEnum.MemberNotPrivilege: msg = "只有軍團長和副軍團長可操作"; break;
            case ErrorCodeEnum.SameDependHost: msg = "被打軍團和自己軍團屬於同一依附主"; break;
            case ErrorCodeEnum.DotFightDependHost: msg = "不能攻擊依附主軍團"; break;
            case ErrorCodeEnum.UnionNotExistOrFree: msg = "該軍團不存在或者該軍團已被釋放"; break;
            case ErrorCodeEnum.DotFightSelfDepend: msg = "無法攻擊自己的附庸軍團"; break;
            case ErrorCodeEnum.MaterialSoldierEnergy: msg = "材料星級價值不正確"; break;
            case ErrorCodeEnum.SerialNumError: { CommonFunction.ShowOfflineTip(); } break;
            case ErrorCodeEnum.MultipleLogins: msg = "您的賬號已在其他設備登陸,當前設備無法再次登陸"; break;
            case ErrorCodeEnum.PlevelAndVipLow: msg = "玩家等級不足或VIP等級不足"; break;
            case ErrorCodeEnum.NewHandTaskTimeOver: msg = "新手任務時間結束"; break;
            case ErrorCodeEnum.FullBuff: msg = "Buff層數已滿"; break;
            case ErrorCodeEnum.FullBuyGold: msg = "金幣購買次數已滿"; break;
            case ErrorCodeEnum.FullBuyDiamond: msg = "鑽石購買次數已滿"; break;
            case ErrorCodeEnum.AlreadyWorship: msg = "已朝拜"; break;
            case ErrorCodeEnum.BuyBuffInWrongTime: msg = "活動未開始 無法購買BUFF"; break;
            case ErrorCodeEnum.OverlordNoTimes: msg = "活動未開始 無法挑戰霸主"; break;
            case ErrorCodeEnum.EndOfActivity: msg = ConstString.SUPERMACY_ENDOFACTIVITY; break;
            case ErrorCodeEnum.OverlordCD: msg = ConstString.SUPERMACY_OVERLORDCD; break;
            case ErrorCodeEnum.PoleMatchError: msg = "沒有匹配到玩家"; break;
            case ErrorCodeEnum.PoleRewardDurationForbid: msg = "休賽期無法領取獎勵"; break;
            case ErrorCodeEnum.PoleRewardRepeated: msg = "重複領取"; break;
            case ErrorCodeEnum.PoleRewardForbid: msg = "該獎勵無法領取"; break;
            case ErrorCodeEnum.PoleRevengeTimes: msg = "復仇次數限制"; break;
            case ErrorCodeEnum.PoleBuytimesLimit: msg = "購買次數限制"; break;
            case ErrorCodeEnum.PoleFightTimesLimit: msg = "已達挑戰次數限制"; break;
            case ErrorCodeEnum.PoleMatchCD: msg = "匹配冷卻中"; break;
            case ErrorCodeEnum.CanNotUseUnionIcon: msg = "軍團徽章已鎖定，不能使用"; break;
            case ErrorCodeEnum.CanNotUseSameUnionIcon: msg = "軍團徽章已使用，不能重複設置"; break;
            case ErrorCodeEnum.SoulGridNumMax: msg = "格子已滿"; break;
            case ErrorCodeEnum.SoulCannotUp: msg = "命魂無法升級"; break;
            case ErrorCodeEnum.SoulExploreMax: msg = "獵魂區域已滿"; break;
            case ErrorCodeEnum.SoulBagOverFlow: msg = "命魂背包已滿"; break;
            case ErrorCodeEnum.SoulTypeConflict: msg = "命魂類型衝突"; break;
            case ErrorCodeEnum.SoulLevelMax: msg = "命魂等級已達最大等級"; break;
            case ErrorCodeEnum.SoulLevelPassPlayer: msg = "命魂等級達到英雄等級"; break;
            case ErrorCodeEnum.SoulBuyGridTimesMax: msg = "命魂背包格子購買次數已達上限"; break;
            case ErrorCodeEnum.SoulHasNoAvailable: msg = "沒有可以裝備的命魂 "; break;
            case ErrorCodeEnum.SoulOneKeyExploreMax: msg = "獵魂區域已滿,一鍵獵魂結束"; break;
            case ErrorCodeEnum.SoulOneKeyExploreGold: msg = "銅錢不足,一鍵獵魂結束"; break;
            case ErrorCodeEnum.SoulOneKeyCollectMax: msg = "命魂倉庫已滿,一鍵拾取結束"; break;
            case ErrorCodeEnum.ExpeditionCastleDestroy: msg = "你的城堡已被摧毀無法繼續遠征"; break;
            case ErrorCodeEnum.SoulPositionLocked: msg = "命魂位置未解鎖"; break;
            case ErrorCodeEnum.RecycleNoItems: msg = "尚未選擇物品"; break;
            case ErrorCodeEnum.RecycleOverflow: msg = ConstString.TIP_RECYCLE_OVERFLOW; break;
            case ErrorCodeEnum.SkillBookOverflow: msg = "煉化返還技能書達到上限，煉化失敗"; break;
            case ErrorCodeEnum.SoldierCannotUpStep: msg = "該武將不能升階。"; break;
            case ErrorCodeEnum.SoldierMaxStep: msg = "武將已經達到最大階級。"; break;
            case ErrorCodeEnum.SoldierNotMaxStep: msg = "武將沒有達到滿階，無法升星。"; break;
            case ErrorCodeEnum.PetLevelLower: msg = "宠物等级不足"; break;
            case ErrorCodeEnum.TileIsSafeNotFight: msg = "地塊是邊界無法攻擊"; break;
            case ErrorCodeEnum.CSWarNotStart: msg = "活動尚未開始"; break;
            case ErrorCodeEnum.TileNotAck: msg = "安全區無法攻擊"; break;
            case ErrorCodeEnum.CSWARCDTIME:msg = "冷卻時間未結束，不能攻擊"; break;
            case ErrorCodeEnum.ActivityGetRewardMaxTime: msg = "活動領取已達最大次數"; break;
            default: msg = code.ToString();
                break;
        }
        return msg;
    }
}

public enum ErrorCodeEnum
{
    SUCCESS = 0,
    /// <summary>
    /// 失败
    /// </summary>
    FAILED = 1,
    /// <summary>
    /// DB插入失败(accname已经存在)
    /// </summary>
    DBInsertFailed = 12,
    /// <summary>
    /// 账号不存在
    /// </summary>
    NoSuchAccount = 14,
    /// <summary>
    /// 角色重名
    /// </summary>
    REPEATNICKNAME = 16,
    /// <summary>
    /// 账号或密码不能为空
    /// </summary>
    AccountPasswordEmpty = 17,
    /// <summary>
    /// 账号长度错误（6~20个英文母或数字）
    /// </summary>
    AccountLengthWrong = 18,
    /// <summary>
    /// 密码长度错误（6~20个英文母或数字）
    /// </summary>
    PasswordLengthWrong = 19,
    /// <summary>
    /// 密码错误
    /// </summary>
    PasswordWrong = 20,
    /// <summary>
    /// 密码或者账号过于简单
    /// </summary>
    PwdOrNameEasy = 21,
    /// <summary>
    /// 玩家断线
    /// </summary>
    OFFLINE = 100,
    /// <summary>
    /// 配置错误
    /// </summary>
    ConfigError = 110,
    /// <summary>
    /// 多处登陆
    /// </summary>
    MultipleLogins = 154,
    /// <summary>
    /// 输入码为空
    /// </summary>
    CDKeyInputEmpty = 160, //输入空
    /// <summary>
    /// CDKey无效
    /// </summary>
    CDKeyInvalid = 161, 	//CDKey无效
    /// <summary>
    /// CDK尚未开放
    /// </summary>
    CDKeyEarly = 162,  //CDKey尚未开放
    /// <summary>
    /// CDK已过期
    /// </summary>
    CDKeyPastDue = 163, 	//CDKey已过期
    /// <summary>
    /// CDK已使用
    /// </summary>
    CDKeyUsed = 164,	//CDKey已被使用
    /// <summary>
    /// 数据异常
    /// </summary>
    DataUnusual = 200,
    /// <summary>
    /// 配置异常
    /// </summary>
    ConfigUnusual = 201,
    /// <summary>
    /// 游戏币不足
    /// </summary>
    NotEnoughGold = 202,
    /// <summary>
    /// 钻石不足
    /// </summary>
    NotEnoughDiamond = 203,
    /// <summary>
    /// 体力不足
    /// </summary>
    NotEnoughphysical = 905,
    /// <summary>
    /// 物品达到上限
    /// </summary>
    OverItem = 205,
    /// <summary>
    /// vip等级不足
    /// </summary>
    VIPLvNotEnough = 206,
    /// <summary>
    /// 次数限制
    /// </summary>
    TimesLimited = 207,
    /// <summary>
    /// 竞技场积分不足（勋章）
    /// </summary>
    Medal_Insufficient = 208,
    /// <summary>
    /// 远征积分不足（军功）
    /// </summary>
    Honor_Insufficient = 209,
    /// <summary>
    /// 物品禁止使用
    /// </summary>
    ItemCanNotUse = 210,
    /// <summary>
    /// 军团令牌不足
    /// </summary>
    UnionTokenNotEnough = 218,

    /// <summary>
    /// 已经购买了月不能再买
    /// </summary>
    HadMonthCard = 211,
    /// <summary>
    /// 已经领取此活跃宝箱不能再领取
    /// </summary>
    HadGetLivenessAward = 212,
    /// <summary>
    /// 领取此活跃宝箱活跃度不够
    /// </summary>
    LivenessNotEnough = 213,
    /// <summary>
    /// 改名非法
    /// </summary>
    CharnameIllegal = 214,
    /// <summary>
    /// 反馈次数限制
    /// </summary>
    SuggestTimesUseup = 215,
    /// <summary>
    /// 已达玩家最大等级上限
    /// </summary>
    ReachedPlayerMaxLevel = 223,
    /// <summary>
    /// 该装备已经穿在该位置
    /// </summary>
    HadEquipHerr = 301,
    /// <summary>
    /// 装备已经卸下
    /// </summary>
    HadUninstall = 302,
    /// <summary>
    /// 不同类型的装备(装备部位出错)
    /// </summary>
    EquipTypeError = 303,
    /// <summary>
    /// 装备等级达到上限
    /// </summary>
    HadMaxLevel = 304,
    /// <summary>
    /// 玩家等级不足
    /// </summary>
    NotEnoughPlayerLevel = 305,
    /// <summary>
    /// 不能出售
    /// </summary>
    CanotSail = 306,
    /// <summary>
    /// 材料武将等级不足
    /// </summary>
    NotEnoughSoldierLevel = 309,
    /// <summary>
    /// 后置ID错误
    /// </summary>
    NextIDError = 312,
    /// <summary>
    /// 商店免费刷新时间未到
    /// </summary>
    ShopUpdateTimeUndue = 313,
    /// <summary>
    /// 已经购买过此商品 
    /// </summary>
    GoodsHadSellOut = 314,
    /// <summary>
    /// 武将装备背包已满
    /// </summary>
    SoldierEquipFull = 315,
    /// <summary>
    /// 已经领取了连续登录奖励
    /// </summary>
    HadGetContinuLoginAward = 317,
    /// <summary>
    /// 已经领取了该累计登录天数奖励
    /// </summary>
    HadGetCumulativeLoginAward = 318,
    /// <summary>
    /// 武将等级达到上限
    /// </summary>
    SoldierMaxLevel = 319,
    /// <summary>
    /// 今日金币兑换次数已用完
    /// </summary>
    ExchangeGoldTimesOver = 320,
    /// <summary>
    /// 今日购买体力次数已使用完
    /// </summary>
    BuyPHPowerUsedOver = 322,
    /// <summary>
    /// 超过体力最大上限
    /// </summary>
    OverMaxPHPower = 323,
    /// <summary>
    /// 技能达到最高等级
    /// </summary>
    SkillUpToTop = 325,
    /// <summary>
    /// --不能再购买背包格子上限
    /// </summary>
    CanNotBuyEquipBagNum = 326,
    /// <summary>
    ///  --英雄等级不足
    /// </summary>
    HeroLevelLower = 327,
    /// <summary>
    /// --武将等级不足
    /// </summary>
    SoldierLevelLower = 328,
    /// <summary>
    /// 敏感词
    /// </summary>
    ForbiddenWorld = 329,

    /// <summary>
    /// 未参加战斗
    /// </summary>
    PlayerNotInFight = 403,
    /// <summary>
    /// 玩家没有足够的竞技场次数
    /// </summary>
    NotEnoughArenaTimes = 410,
    /// <summary>
    /// 对手的排名太低了
    /// </summary>
    OpponentRankTooLow = 411,
    /// <summary>
    /// 竞技场还在冷却中
    /// </summary>
    PlayerOptInCD = 412,
    /// <summary>
    /// 对手正在竞技场中
    /// </summary>
    OpponentInArena = 413,
    /// <summary>
    /// 玩家正在竞技场中
    /// </summary>
    PlayerInArena = 414,
    /// <summary>
    /// 玩家演武台排名改变
    /// </summary>
    PlayerArenaRankChange = 415,
    /// <summary>
    /// 武将没有穿装备
    /// </summary>
    SoldierNotHaveEquip = 500,
    /// <summary>
    /// 武将没有可强化的装备
    /// </summary>
    SoldierNotHaveEquipProme = 501,

    /// <summary>
    /// 玩家已经购买基金
    /// </summary>
    PlayerBuyFundsAlready = 600,
    /// <summary>
    /// 玩家活动未完成，不能领取奖励
    /// </summary>
    ActivityNotFinish = 601,
    /// <summary>
    /// 玩家已经领取过活动奖励
    /// </summary>
    ActivityRewardAlready = 602,
    /// <summary>
    /// 玩家无法达到兑换条件
    /// </summary>
    NotReachExchangeCondition = 603,

    /// <summary>
    /// 材料不足
    /// </summary>
    NotEnoughMaterial = 903,
    /// <summary>
    /// 道具不足
    /// </summary>
    NotEnoughItem = 904,
    /// <summary>
    /// 城堡等级太低
    /// </summary>
    CastleLevelLow = 910,
    /// <summary>
    /// 世界聊天CD中
    /// </summary>
    WorldChatTimeInCD = 913,
    /// <summary>
    /// 小关卡已通关
    /// </summary>
    EnterTimesLimited = 1001,
    /// <summary>
    ///  --统御力不足
    /// </summary>
    LeaderShipNotEnough = 1002,
    /// <summary>
    /// --前置关卡未通过
    /// </summary>
    PrevDungeonNotPass = 1003,
    /// <summary>
    /// --前置关卡星级未达到解锁条件
    /// </summary>
    PrevDungeonStarLevelNotEnough = 1004,
    /// <summary>
    /// --英雄等级不能解锁该关卡
    /// </summary>
    HeroLevelNotEngouh = 1005,
    /// <summary>
    /// --关卡该时间不能打
    /// </summary>
    OpenTimeLimited = 1006,
    /// <summary>
    ///  --相同武将
    /// </summary>
    SameTypeSolider = 1007,
    /// <summary>
    ///  --章节该时间不开放
    /// </summary>
    ChapterOpenTimeNotEnough = 1008,
    /// <summary>
    /// --挑战次数不够
    /// </summary>
    ChallengeTimesNotEnough = 1009,
    /// <summary>
    ///  --玩家星级不够，无法领取
    /// </summary>
    StarLevelNotEnough = 1010,
    /// <summary>
    /// --玩家已经领取奖励
    /// </summary>
    GetRewardAready = 1011,
    /// <summary>
    /// --该副本无法扫荡
    /// </summary>
    DungeonCannotMopup = 1012,
    /// <summary>
    ///  --该副本玩家未通关
    /// </summary>
    DungeonNotPass = 1013,
    /// <summary>
    ///  --玩家没有足够的扫荡卡
    /// </summary>
    MopupItemNotEnough = 1014,
    /// <summary>
    /// --副本类型错误
    /// </summary>
    ErrorDungeonType = 1015,
    /// <summary>
    /// --玩家无需购买次数
    /// </summary>
    NotNeedBuyTimes = 1016,
    /// <summary>
    /// --玩家购买次数已达上限
    /// </summary>
    BuyTimesLimited = 1017,
    /// <summary>
    ///  --你已经通关，请重新开始远征
    /// </summary>
    PassAllExpedition = 1018,

    UnionNotExist = 1050, //你查找的军团不存在
    LeaveUnionInCD = 1051, //离开军团24小时内将无法加入其它军团
    ApplyForUnionTimesLimited = 1052, //今日军团申请次数已用完
    UnionNameExist = 1053,  //军团名已存在
    UnionNameIllegal = 1054,  //军团名非法
    NotHaveUnion = 1055,  //玩家没有军团
    NoPrivilege = 1056,  //玩家没有权限操作      
    NotJoinTheUnion = 1057,  //玩家没有加入对应的军团  
    ChairmanCannotExitUnion = 1058,  //军团长无法退出军团         
    HaveJoinedUnionAready = 1059,  //玩家已经加入军团    
    PendingUnionApplication = 1060,  //军团申请待审核
    UnionIsFull = 1061,  //军团人数已达上限
    MaxUnionPromotionTimes = 1062,  //今日军团2个升职名额已用尽
    MaxUnionAbdicateTimes = 1063,  //今日军团让位机会已用尽
    MaxUnionKickMemberTimes = 1064,  //今日军团开除成员已达到5名，无法再开除
    ViceChairmanReachMax = 1065,  //副军团长达到上限
    CannotPromotion = 1066,  //不必升职
    CannotDemotion = 1067,  //玩家不能降职
    CannotAbdicate = 1068,  //无法退位
    MaxDonateTimes = 1069,  //达到最大捐献次数
    ExceedMaxVisitTimes = 1070,  //超过最大拜访次数
    HaveVisitedAready = 1071,  //已经拜访过该玩家
    UnionNameNull = 1072,//  --军团名为空

    ApplyForUnionAready = 1073,  //已经申请军团
    PlayerHaveLeaveUnion = 1074, //--玩家已经离开军团
    MaxUnionLevel = 1075, //最大军团等级了，无法再升级
    UnionDgnIsChallenged = 1076, // 异域探险副本正在被挑战中
    UnionResourceNotEnough = 1077,//-军团物资不够
    CannotGetRewards = 1078, //条件不足，无法领取奖励

    NotApplyUnionClashTime = 1079, //无法报名，不在报名期间 
    ErrorState = 1080, //状态不对,
    HaveJoinedUnionClash = 1081, //玩家已经参加军团战
    NotJoinedUnionClash = 1082, //玩家没有参加军团战
    HavePassedUnionPve = 1083,      //已通关当前军团副本关卡


    HadBlockedPlayer = 1090,//已经屏蔽该玩家
    BlockedNumFull = 1091,//屏蔽玩家上限

    NoPlayerToMatch = 1200,  //奴役匹配陌生人没有课匹配的玩家 
    PrisonLocked = 1201,  //该囚牢未解锁 
    PrisonFul = 1202,  //该囚牢已经有奴隶了
    InEnslaveFight = 1203,  //正在奴役战斗中
    EnslaveStatusChange = 1204,  //奴役状态改变}
    EnslaveFightTimeOut = 1205,  //奴役战斗准备时间超时
    EnslaveHostChange = 1206,  //奴隶主改变
    PrisonIsNull = 1207,  //囚牢是空的
    PrisonMoneyZero = 1208,  //囚牢没有可收取的金币
    NoneEnemyPlayer = 1209,  //没有仇人
    NoneRunawayPlayer = 1210,  //没有逃奴
    NotHavePlayerInfo = 1211,  //没有该玩家的逃奴或仇人信息
    NotLockedPlayer = 1212,  //没有锁定该玩家
    AskHelpTimeOut = 1213,  //请求救援时间间隔不足
    WillTimeOver = 1214,  //玩家即将满24小时
    PlayerIsYouEnslave = 1215,  //玩家已经是你的奴隶,无法解救
    PlayerIsSameHolder = 1216,  //玩家与你同一奴隶主，不能解救
    DiffrentLevelToMuch = 1217,  //等级差异过大（5级），不能奴役
    AskHelpTimeOver = 1218,  //救援请求已过期

    NoRecharge = 216,  //玩家未充值
    HadGetFirstRechargeAward = 217,  //已经领取过第一次充值奖励
    SoldierMaxNum = 330,//达到武将上限300
    ExceedMaxGetHpTimes = 1084,//玩家超过最大领取体力次数
    InformationExpired = 1085,
    CanNotEnslaveHolder = 1219,
    GetNothing = 1086,
    PlayerOffline = 1087, //目标玩家不在线
    UionPVEMemberFight = 1089, //--军团PVE有成员正在战斗
    MaterialSoldierNotEnough = 331,//武将升到升星等级材料不足
    VMaxUnionKickMemberTimes = 1120,//[副军团长]今日军团开除成员已达到1名，无法再开除
    GoldIsMax = 1301, //货币最大值，金币
    DiamondIsMax = 1302, //货币最大值，钻石
    PhPowerIsMax = 1303, //体力到达最大值 

    SoldierChipMaxNum = 332,//武魂上限
    MaterialSoldierEnergy = 333, //材料星级价值不对
    QualityExpNotEnough = 1401,  //天将神兵经验不足      
    QualityWrong = 1402,  //品质错误    
    NewHandTaskTimeOver = 1403, //新手任务时间结束 
    SoldierNoEquipOn = 1404,  //武将没有穿装备

    ActivityNotInTime = 604, //活动已过期
    FrozenAccount = 155,  //封号
    FrozenChat = 156,  //禁言
    SerialNumError = 101, //消息序列号不一致
    CDKPackUsed = 165,  //该批次CDKey礼包已领过

    HasVipUpReward = 219,
    GetVipDailySameDay = 220,

    InBlockedList = 1450,   //在屏蔽列表中
    FriendsOverFlow = 1451,   //好友满
    ApplyFriendsRepeated = 1452,   //重复申请
    ApplyNotDealed = 1453,   //申请未处理
    ApplyNotDealedOther = 1454,   //对方有你申请 还未处理
    DonateFriendsPhTimes = 1455,   //赠送体力次数用完
    RecieveFriendsPhTimes = 1456,   //领取体力次数用完
    NoRevieveHp = 1457,     //没有可领取的体力
    FriendsPartOverFlow = 1458,  //好友部分溢出
    FriendsPartBlockList = 1459,  //好友部分屏蔽
    FirendsCharNotExist = 1460,  //角色好友不存在
    DonateFriendsPhAlready = 1461,  //今日已赠送此人体力
    VerifyInviteCodeNotExist = 1470, //邀请码不存在
    VerifyInviteCodeIllegal = 1471, //邀请码非法
    VerifyInviteCodeUsed = 1472, //邀请码已使用
    VerifyInviteCodeTaskStatus = 1473, //邀请任务 状态错误
    HadGotFirstReward = 221,
    ThirdFriendsInviteTime = 1475, //邀请时间未到    
    ThirdFriendsWakeupTime = 1476, //唤醒时间未到
    RefreshFriendsCD = 1462, //刷新推荐好友cd
    PlevelAndVipLow = 222, //玩家等级不足或vip等级不足
    FullBuff = 1478,//Buff层数已满//
    FullBuyGold = 1479,//金币购买次数已满//
    FullBuyDiamond = 1480,//钻石购买次数已满//
    AlreadyWorship = 1481,//已朝拜//
    BuyBuffInWrongTime = 1482,//活动未开始无法购买BUFF//
    OverlordNoTimes = 1483,//活动未开始 无法挑战霸主//
    EndOfActivity = 1484,//活动结束 本次结算不参与计算//
    OverlordCD = 1485,//全服霸主战斗CD
    AltarHandleTimesNotEnough = 1121,  //神坛安慰。调戏次数不足
    DependUnionNotExist = 1122,  //依附军团不存在
    DependPositionNotNil = 1123,  //依附位置不是空闲状态
    AltarFightSelfUnion = 1124, //不能攻击自己军团 
    RevoltNotDependHost = 1125, //反抗军团不再是依附主
    MemberNotPrivilege = 1126, //只有军团长和副军团长可操作
    SameDependHost = 1127, //被打军团和自己军团属于同一依附主
    DotFightDependHost = 1128, //不能攻击依附主军团
    UnionNotExistOrFree = 1129, //该军团不存在或者该军团已被释放
    DotFightSelfDepend = 1130, //无法攻击自己的附庸军团

    PoleMatchError = 1500,// --没有匹配到玩家
    PoleRewardDurationForbid = 1501, //--休赛期无法领取奖励
    PoleRewardRepeated = 1502, //--重复领取
    PoleRewardForbid = 1503, //--该奖励无法领取
    PoleRevengeTimes = 1504, //--复仇次数限制
    PoleBuytimesLimit = 1505, //--购买次数限制
    PoleFightTimesLimit = 1506, //--挑战次数限制
    PoleMatchCD = 1507, //--匹配CD

    SoulGridNumMax = 1510, //--格子已满
    SoulCannotUp = 1511,// --命魂无法升级
    SoulExploreMax = 1512, //--猎魂区域已满
    SoulBagOverFlow = 1513,// --命魂背包已满
    SoulTypeConflict = 1514,// --类型冲突
    SoulLevelMax = 1515,// --命魂等级已达最大等级
    SoulLevelPassPlayer = 1516,// --命魂等级达到英雄等级
    SoulBuyGridTimesMax = 1517, //--命魂背包格子购买次数已达上限
    SoulHasNoAvailable = 1518, //--无可装备命魂 
    CanNotUseUnionIcon = 1093,//军团徽章已锁定，不能使用
    CanNotUseSameUnionIcon = 1094,//军团徽章已使用，不能重复设置
    SoulOneKeyExploreMax = 1519, //--一键猎魂 区域已满
    SoulOneKeyExploreGold = 1520, //--一键猎魂铜钱
    SoulOneKeyCollectMax = 1521, //--一键拾取 背包已满
    SoulPositionLocked = 1522,    //--命魂位置未解锁 
    ExpeditionCastleDestroy = 406, //远征城堡已被摧毁

    RecycleNoItems = 1600, //没有选择物品
    RecycleOverflow = 1601, //货币达到上限，无法炼化
    SkillBookOverflow = 1602, //货币达到上限，无法炼化
    SoldierCannotUpStep = 334, //该武将不能升阶 
    SoldierMaxStep = 335, //已达最大阶级
    SoldierNotMaxStep = 336, //未满级 无法升星

    PetLevelLower = 1700, //宠物等级不足

    TileIsSafeNotFight = 700,  //地块是边界无法攻击
    CSWarNotStart = 701,  //活动尚未开始
    TileNotAck = 702,  //安全区无法攻击
    CSWARCDTIME = 703,  //冷却时间未结束，不能攻击
    ActivityGetRewardMaxTime = 605,//活动领取已达最大次数
}





















