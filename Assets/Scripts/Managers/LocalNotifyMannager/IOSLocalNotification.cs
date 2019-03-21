using System;
using System.Collections.Generic;
using UnityEngine;
public class IOSLocalNotification : ILocalNotification
{
    public IOSLocalNotification()
    {
#if UNITY_IPHONE
        NotificationServices.RegisterForLocalNotificationTypes(LocalNotificationType.Badge | LocalNotificationType.Alert | LocalNotificationType.Sound);
#endif
    }
    public void SendNotification(LocalNotificationData data)
    {
        if (data == null)
        {
            return;
        }
#if UNITY_IPHONE
        LocalNotification localNotification = new LocalNotification();
        localNotification.alertAction = data.alertAction;
        localNotification.alertBody = data.content;
        localNotification.alertLaunchImage = data.alertLaunchImage;
        localNotification.hasAction = data.hasAction;
        DateTime dt = DateTime.Now.AddSeconds(data.delayTime);
        localNotification.fireDate = dt;
        localNotification.soundName = data.soundName;
        localNotification.applicationIconBadgeNumber = data.applicationIconBadgeNumber;
        localNotification.userInfo = data.userInfo;
        localNotification.timeZone = data.timeZone;
        localNotification.alertLaunchImage = data.alertLaunchImage;
        if (data.isRepeat)
        {
            localNotification.repeatCalendar = CalendarIdentifier.ChineseCalendar;
            localNotification.repeatInterval = data.repeatInterval;
        }
        NotificationServices.ScheduleLocalNotification(localNotification);
#endif
    }

    public void CancelNotification(int id)
    {
#if UNITY_IPHONE
        LocalNotification[] notificationArray = NotificationServices.scheduledLocalNotifications;
        LocalNotification localnotification = null;
        for (int i = 0; i < notificationArray.Length; i++)
        {
            LocalNotification notification = notificationArray[i];
            if (notification == null)
            {
                continue;
            }
            foreach (int notify in notification.userInfo.Values)
            {
                if (notify == id)
                {
                    localnotification = notification;
                }
            }
            if (localnotification != null)
                break;
        }
        if (localnotification == null)
        {
            Debug.LogError("can not get LocalNotification by id:" + id.ToString());
            return;
        }
        NotificationServices.CancelLocalNotification(localnotification);
#endif
    }

    public void CancelAllNotifications()
    {
#if UNITY_IPHONE
        LocalNotification ln = new LocalNotification();
        ln.applicationIconBadgeNumber = -1;
        NotificationServices.PresentLocalNotificationNow(ln);
        NotificationServices.CancelAllLocalNotifications();
        NotificationServices.ClearLocalNotifications();
#endif
    }
    #region IDisposable

    public void Dispose()
    {
    }

    #endregion

}