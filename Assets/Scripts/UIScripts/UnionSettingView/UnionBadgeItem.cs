using UnityEngine;
using System.Collections;
using Assets.Script.Common;

public class UnionBadgeItem : MonoBehaviour
{

    private UISprite Spt_BadgeIcon;
    private UISprite Spt_BadgeLock;
    private UILabel Lbl_BadgeTime;


    public uint BadgeID;        //徽章ID//
    public string IconName;     //徽章名字//
    public EBadgeType lockType; //锁定类型//
    public string UnLockHint;   //解锁条件//
    private uint lastTime;


    public void InitItem()
    {
        if (Spt_BadgeLock != null)
        {
            Spt_BadgeLock.gameObject.SetActive(false);
        }
        if (Lbl_BadgeTime != null)
        {
            Lbl_BadgeTime.text = "";
        }
        this.gameObject.SetActive(false);
        Scheduler.Instance.RemoveTimer(ShowLastTime);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="vItemName">预制名</param>
    /// <param name="vItemPos">预制位置</param>
    /// <param name="vBadgeID">徽章ID</param>
    /// <param name="vIconName">徽章图片名</param>
    /// <param name="vLockType">锁定类型</param>
    /// <param name="vLastTime">解锁后剩余时间</param>
    /// <param name="vIsFirst">是否第一次开启</param>
    public void RefreshItem(string vItemName, Vector3 vItemPos, uint vBadgeID, string vIconName, EBadgeType vLockType, uint vLastTime, string vUnLockHint, bool vIsFirst = false)
    {
        BadgeID = vBadgeID;
        IconName = vIconName;
        lockType = vLockType;
        UnLockHint = vUnLockHint;
        lastTime = vLastTime;
        //lastTime = 60;
        if (vIsFirst)
        {
            SetComponentOperate();
        }
        InitItem();
        if (Spt_BadgeIcon != null)
        {
            CommonFunction.SetSpriteName(Spt_BadgeIcon, IconName);
            if (lockType == EBadgeType.ebtLock)
            {
                Spt_BadgeIcon.color = Color.black;
            }
            else
            {
                Spt_BadgeIcon.color = Color.white;
            }
        }
        if (Spt_BadgeLock != null)
        {
            Spt_BadgeLock.gameObject.SetActive(lockType == EBadgeType.ebtLock);
        }
        if (lockType == EBadgeType.ebtLockLimit)
        {
            ShowLastTime();
            Scheduler.Instance.AddTimer(1, true, ShowLastTime);
        }

        this.name = vItemName;
        this.transform.localPosition = vItemPos;
        this.gameObject.SetActive(true);
    }

    public void Destroy()
    {
        if (this != null)
        {
            GameObject.Destroy(this.gameObject);
        }
    }

    /// <summary>
    /// 设置组件
    /// </summary>
    private void SetComponentOperate()
    {
        Spt_BadgeIcon = this.transform.FindChild("BadgeIcon").gameObject.GetComponent<UISprite>();
        Spt_BadgeLock = this.transform.FindChild("BadgeLock").gameObject.GetComponent<UISprite>();
        Lbl_BadgeTime = this.transform.FindChild("BadgeTime").gameObject.GetComponent<UILabel>();
    }

    /// <summary>
    /// 显示时间
    /// </summary>
    private void ShowLastTime()
    {
        string tmpTime = "";
        if (lastTime <= 0)
        {
            Scheduler.Instance.RemoveTimer(ShowLastTime);
            if (lockType != EBadgeType.ebtUnLock)
            {
                lockType = EBadgeType.ebtLock;
                if (Spt_BadgeLock != null)
                {
                    Spt_BadgeLock.gameObject.SetActive(lockType == EBadgeType.ebtLock);
                }
            }
        }
        else
        {
            if (lockType == EBadgeType.ebtLockLimit)
            {
                if (lastTime > 0)
                {
                    if (lastTime < 3600 * 24)
                    {
                        tmpTime = string.Format("{0:D2}:{1:D2}:{2:D2}", lastTime / 3600, lastTime % 3600 / 60, lastTime % 60);
                    }
                    else
                    {
                        tmpTime = string.Format(ConstString.FORMAT_INTEGERTIME_DAY, lastTime / 3600 / 24);
                    }
                    lastTime -= 1;
                }
            }
        }
        if (Lbl_BadgeTime != null)
        {
            Lbl_BadgeTime.text = tmpTime;
        }
    }
}