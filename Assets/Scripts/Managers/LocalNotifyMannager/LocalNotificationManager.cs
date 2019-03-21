using System;
using System.Collections.Generic;
using UnityEngine;

// 本地推送的类型,注意：id不能重复


public class LocalNotificationManager
{
    // 本地推送的类型,注意：id不能重复
    private enum ENotificationType
    {

    }

    #region Singleton

    static LocalNotificationManager s_instance;

    public static LocalNotificationManager Instance
    {
        get
        {
            if (s_instance == null)
                s_instance = new LocalNotificationManager();
            return s_instance;
        }
    }

    #endregion

    #region DefaultNotification

    class DefaultNotification : ILocalNotification
    {
        public void SendNotification(LocalNotificationData data)
        {
        }


        public void CancelNotification(int id)
        {
        }

        public void CancelAllNotifications()
        {
        }
        public void Dispose()
        {

        }
    }

    #endregion

    private ILocalNotification m_notification;

    private void GetNotification()
    {
        if (m_notification == null)
        {
#if UNITY_EDITOR
            m_notification = new DefaultNotification();
#elif UNITY_ANDROID
        m_notification = new AndroidLocalNotifitcation();
#elif  UNITY_IPHONE
        m_notification = new IOSLocalNotification();
#else
        m_notification = new DefaultNotification();
        Debug.LogWarning("Local push not support this platform");
#endif
        }
    }

    public void SendNotification(LocalNotificationData data)
    {
        if (data == null || data.delayTime <= 0)
            return;
        GetNotification();
        m_notification.SendNotification(data);
    }

    public void CancelNotification(int id)
    {
        GetNotification();
        m_notification.CancelNotification(id);
    }

    public void CancelAllNotifications()
    {
        GetNotification();
        m_notification.CancelAllNotifications();
    }

    private int GetLocalNotificationID(ELocalNotificationType type, int id)
    {
        int notifyID = 0;
        switch (type)
        {
            case ELocalNotificationType.Device:
                {
                    notifyID = id;
                } break;
            case ELocalNotificationType.Server:
                {
                    notifyID = (int)PlayerData.Instance._ServerID * 100 + id;
                } break;
            case ELocalNotificationType.Role:
                {
                    notifyID = (int)PlayerData.Instance._RoleID * 100 + id;
                } break;
        }
        return notifyID;
    }

    /// <summary>
    /// 体力回满
    /// </summary>
    public void SPRecoverMax()
    {
        int notifyID = GetLocalNotificationID(ELocalNotificationType.Role, GlobalConst.NotificationID.PHPower);
        if (PlayerData.Instance.PHPowerSystemPush == 1)//判斷是否開啟功能
        {
            long LastSPMAXTime = MainCityModule.Instance.SPRecoverTimer + Mathf.Max(0, PlayerData.Instance.MaxPhysical - PlayerData.Instance._Physical - 1) * GlobalCoefficient.SPRecoverTimer;

            LocalNotificationData data = new LocalNotificationData();
            data.content = string.Format(ConstString.AGAINSTCONTSNT, PlayerData.Instance._NickName);
            data.delayTime = LastSPMAXTime;
#if UNITY_ANDROID
            data.notifyID = notifyID;
            data.title = GlobalConst.GAMENAME;
#elif UNITY_IPHONE
            data.userInfo = new Dictionary<string, int>();
            data.userInfo.Add("notifyID", notifyID);
#endif
            SendNotification(data);
        }
        else
        {
            CancelNotification(notifyID);
        }
    }

    /// <summary>
    /// 招贤推送->T-綠色 F-藍色
    /// </summary>
    public void GreenAgainst()
    {
        int notifyID = GetLocalNotificationID(ELocalNotificationType.Role, GlobalConst.NotificationID.RecruitBraveID);
        if (PlayerData.Instance.RecruitSystemPush == 1 && !MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Recruit))//功能是否開啟
        {
            if (CommonFunction.CheckIsOpen(OpenFunctionType.Recruit))//招贤馆解锁
            {
                if (PlayerData.Instance.RecruitFreeCount > 0 && !CommonFunction.IsEndTime((long)PlayerData.Instance.LastBraveRecruitTime))//有免费次数且绿色卡牌冷却状态
                {
                    //计算下次推送时间
                    long LastBraveTime = CommonFunction.GetLfetTime((long)PlayerData.Instance.LastBraveRecruitTime);//绿色卡牌下次免费秒数
                    LocalNotificationData data = new LocalNotificationData();
                    data.content = string.Format(ConstString.AGAINSTCONTSNT, PlayerData.Instance._NickName);
                    data.delayTime = LastBraveTime;
#if UNITY_ANDROID
                    data.notifyID = notifyID;
                    data.title = GlobalConst.GAMENAME;
#elif UNITY_IPHONE
            data.userInfo = new Dictionary<string, int>();
            data.userInfo.Add("notifyID", notifyID);
#endif
                    SendNotification(data);
                }
            }
        }
        else
        {
            CancelNotification(notifyID);
        }
    }
    /// <summary>
    /// 招贤推送 藍色
    /// </summary>
    public void BlueAgainst()
    {
        int notifyID = GetLocalNotificationID(ELocalNotificationType.Role, GlobalConst.NotificationID.RecruitRiotID);
        if (PlayerData.Instance.RecruitSystemPush == 1 && !MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Recruit))//功能是否開啟
        {
            if (CommonFunction.CheckIsOpen(OpenFunctionType.Recruit))//招贤馆解锁
            {
                if (!CommonFunction.IsEndTime((long)PlayerData.Instance.LastRiotRecruitTime))//蓝色卡牌冷却状态
                {
                    long LastRiotTime = CommonFunction.GetLfetTime((long)PlayerData.Instance.LastRiotRecruitTime);
                    LocalNotificationData data = new LocalNotificationData();
                    data.content = string.Format(ConstString.MAXSPCONTENT, PlayerData.Instance._NickName);
                    data.delayTime = LastRiotTime;
#if UNITY_ANDROID
                    data.notifyID = notifyID;
                    data.title = GlobalConst.GAMENAME;
#elif UNITY_IPHONE
            data.userInfo = new Dictionary<string, int>();
            data.userInfo.Add("notifyID", notifyID);
#endif
                    SendNotification(data);
                }
            }
        }
        else
        {
            CancelNotification(notifyID);
        }
    }

    /// <summary>
    /// 体力赠送
    /// </summary>
    public void SPPresent()
    {
        int morningNotifyID = GetLocalNotificationID(ELocalNotificationType.Device, GlobalConst.NotificationID.NoonSPID);
        int afternoonNotifyID = GetLocalNotificationID(ELocalNotificationType.Device, GlobalConst.NotificationID.AfternoonID);
        int nightNotifyID = GetLocalNotificationID(ELocalNotificationType.Device, GlobalConst.NotificationID.EveningID);
        if (PlayerData.Instance.ShopFreshSystemPush == 1)
        {
            DateTime MainTime = CommonFunction.GetDateTime(Main.mTime);
            //---------------------12:00 -------------------------//
            DateTime SendNoonHPTime = new DateTime(MainTime.Year, MainTime.Month, MainTime.Day, 12, 0, 0);
            TimeSpan NoonTime = SendNoonHPTime - MainTime;
            long LastDrowEquipTime = PlayerData.Instance.DrowEquipFreeTime - Main.mTime;
            LocalNotificationData data_morning = new LocalNotificationData();
            data_morning.content = string.Format(ConstString.NOONSPCONTENT, PlayerData.Instance._NickName);
            if (NoonTime.TotalSeconds < 0)
            {
                data_morning.delayTime = (long)NoonTime.TotalSeconds + 86400;
            }
            else
            {
                data_morning.delayTime = (long)NoonTime.TotalSeconds;
            }
            data_morning.isRepeat = true;
#if UNITY_ANDROID
            data_morning.notifyID = morningNotifyID;
            data_morning.title = GlobalConst.GAMENAME;
            data_morning.repeatDealyTime = 86400;
#elif UNITY_IPHONE
            data_morning.repeatInterval = CalendarUnit.Day;
            data_morning.userInfo = new Dictionary<string, int>();
            data_morning.userInfo.Add("notifyID", morningNotifyID);
#endif
            SendNotification(data_morning);
            //---------------------18:00 -------------------------//
            DateTime SendAfternoonHPTime = new DateTime(MainTime.Year, MainTime.Month, MainTime.Day, 18, 0, 0);
            TimeSpan AfternoonTime = SendAfternoonHPTime - MainTime;

            LocalNotificationData data_afternoon = new LocalNotificationData();
            data_afternoon.content = string.Format(ConstString.AFTERNOONCONTENT, PlayerData.Instance._NickName);
            if (AfternoonTime.TotalSeconds < 0)
            {
                data_afternoon.delayTime = (long)AfternoonTime.TotalSeconds + 86400;
            }
            else
            {
                data_afternoon.delayTime = (long)AfternoonTime.TotalSeconds;
            }
            data_afternoon.isRepeat = true;
#if UNITY_ANDROID
            data_afternoon.notifyID = afternoonNotifyID;
            data_afternoon.title = GlobalConst.GAMENAME;
            data_afternoon.repeatDealyTime = 86400;
#elif UNITY_IPHONE
            data_afternoon.repeatInterval = CalendarUnit.Day;
            data_afternoon.userInfo = new Dictionary<string, int>();
            data_afternoon.userInfo.Add("notifyID", afternoonNotifyID);
#endif
            SendNotification(data_afternoon);
            //---------------------21:00 -------------------------//
            DateTime SendEveningHPTime = new DateTime(MainTime.Year, MainTime.Month, MainTime.Day, 21, 0, 0);
            TimeSpan EveningTime = SendEveningHPTime - MainTime;

            LocalNotificationData data_night = new LocalNotificationData();
            data_night.content = string.Format(ConstString.EVENINGCONTENT, PlayerData.Instance._NickName);
            if (EveningTime.TotalSeconds < 0)
            {
                data_night.delayTime = (long)EveningTime.TotalSeconds + 86400;
            }
            else
            {
                data_night.delayTime = (long)EveningTime.TotalSeconds;
            }
            data_night.isRepeat = true;
#if UNITY_ANDROID
            data_night.notifyID = nightNotifyID;
            data_night.title = GlobalConst.GAMENAME;
            data_night.repeatDealyTime = 86400;
#elif UNITY_IPHONE
            data_night.repeatInterval = CalendarUnit.Day;
            data_night.userInfo = new Dictionary<string, int>();
            data_night.userInfo.Add("notifyID", nightNotifyID);
#endif
            SendNotification(data_night);
        }
        else
        {//取消整点推送消息
            CancelNotification(morningNotifyID);
            CancelNotification(afternoonNotifyID);
            CancelNotification(nightNotifyID);
        }
    }
    /// <summary>
    /// 缘宝阁
    /// </summary>
    public void DrawEquip()
    {
        int notifyID = GetLocalNotificationID(ELocalNotificationType.Role, GlobalConst.NotificationID.DrawEquipID);
        if (PlayerData.Instance.DrawEquipSystemPush == 1 && !MainCityModule.Instance.LockFuncs.Contains((uint)OpenFunctionType.Magical))//判断功能是否开启
        {
            if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.YuanBaoGe))//缘宝阁解锁
            {
                if (PlayerData.Instance.DrowEquipFreeCount > 0 && Main.mTime >= PlayerData.Instance.DrowEquipFreeTime)//有免费次数且单次抽取处于冷却状态
                {
                    long LastDrowEquipTime = PlayerData.Instance.DrowEquipFreeTime - Main.mTime;
                    LocalNotificationData data = new LocalNotificationData();
                    data.content = string.Format(ConstString.DROWEQUIPCONTENT, PlayerData.Instance._NickName);
                    data.delayTime = LastDrowEquipTime;
#if UNITY_ANDROID
                    data.notifyID = notifyID;
                    data.title = GlobalConst.GAMENAME;
#elif UNITY_IPHONE
            data.userInfo = new Dictionary<string, int>();
            data.userInfo.Add("notifyID", notifyID);
#endif
                    SendNotification(data);
                }
            }
        }
        else
        {
            CancelNotification(notifyID);
        }
    }
}
