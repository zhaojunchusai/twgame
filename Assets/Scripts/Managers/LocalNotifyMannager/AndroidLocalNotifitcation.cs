using System;
using System.Collections.Generic;
using UnityEngine;


public class AndroidLocalNotifitcation : ILocalNotification
{
#if UNITY_ANDROID
    AndroidJavaObject m_javaObj = new AndroidJavaObject("net.agasper.unitynotification.UnityNotificationManager");
#endif
    string MAINACTIVITYCLASSNAME = "com.idplay.legu.binglin.MainUnityActivity";
    public void SendNotification(LocalNotificationData data)
    {
#if UNITY_ANDROID
        if (data == null)
            return;
        m_javaObj.CallStatic("SetNotification", data.notifyID, data.delayTime * 1000L, data.title, data.content, data.content, data.openSound ? 1 : 0, data.openVibrate ? 1 : 0, data.openLights ? 1 : 0, MAINACTIVITYCLASSNAME);
#endif
    }

    public void CancelNotification(int id)
    {
#if UNITY_ANDROID
        m_javaObj.CallStatic("CancelNotification", id);
#endif
    }

    public void CancelAllNotifications()
    {
#if UNITY_ANDROID
        m_javaObj.CallStatic("CancelAll");
#endif
    }

    #region IDisposable

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
#if UNITY_ANDROID
        if (disposing)
        {
            m_javaObj.Dispose();
            m_javaObj = null;
        }
#endif
    }

    ~AndroidLocalNotifitcation()
    {
        Dispose(false);
    }

    #endregion
}
