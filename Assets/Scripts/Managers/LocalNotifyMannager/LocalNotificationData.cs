using System.Collections.Generic;
using System.Collections;
using UnityEngine;
public class LocalNotificationData
{
    /// <summary>
    /// 消息通知ID
    /// </summary>
    public int notifyID;
    /// <summary>
    /// 延迟时间(单位:s)
    /// </summary>
    public long delayTime;
    /// <summary>
    /// 通知内容
    /// </summary>
    public string content;
    /// <summary>
    /// 是否重复通知
    /// </summary>
    public bool isRepeat = false;

#if UNITY_ANDROID
    /// <summary>
    /// 重复提醒延迟时间（单位:s） 
    /// </summary>
    public long repeatDealyTime;
    /// <summary>
    /// 通知标题
    /// </summary>
    public string title;
    /// <summary>
    /// 开启声音(系统默认声音)
    /// </summary>
    public bool openSound = true;
    /// <summary>
    /// 开启振动
    /// </summary>
    public bool openVibrate = true;
    /// <summary>
    /// T开启呼吸灯
    /// </summary>
    public bool openLights = true;
#endif

#if UNITY_IPHONE
    /// <summary>
    /// 设置锁屏界面的文字
    /// </summary>
    public string alertAction ="查看";
    /// <summary>
    /// 在锁屏时显示的动作标题(完整测标题:"滑动来" + alertAction)
    /// </summary>
    public bool hasAction = true;
    /// <summary>
    /// 设置音效(如果不设置就是系统默认的音效, 设置的话会在mainBundle中查找)
    /// </summary>
    public string soundName = LocalNotification.defaultSoundName;
    /// <summary>
    /// 初始化日历格式（默认为中国公历）
    /// </summary>
    public CalendarIdentifier repeatCalendar = CalendarIdentifier.ChineseCalendar;
    /// <summary>
    /// 通知循环周期(默认间隔为每天)
    /// </summary>
    public CalendarUnit repeatInterval = CalendarUnit.Day;
    /// <summary>
    /// 附加的额外信息
    /// </summary>
    public IDictionary userInfo = null;
    /// <summary>
    /// 应用图标上的通知数目
    /// </summary>
    public int applicationIconBadgeNumber = 1;
    /// <summary>
    /// 手机时区（一般跟随手机时区，一般不填）
    /// </summary>
    public string timeZone = string.Empty;
    /// <summary>
    /// 点击推送通知打开app时显示的启动图片(mainBundle 中提取图片
    /// </summary>
    public string alertLaunchImage = string.Empty;
#endif

}
