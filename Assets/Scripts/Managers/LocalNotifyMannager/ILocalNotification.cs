using System;

public interface ILocalNotification : IDisposable
{

    /// <summary>
    /// 发送单条信息
    /// </summary>
    void SendNotification(LocalNotificationData data);
    
    /// <summary>
    /// 取消特定通知
    /// </summary>
    /// <param name="id"></param>
    void CancelNotification(int id);
    /// <summary>
    /// 清空所有通知
    /// </summary>
    void CancelAllNotifications();
}
