using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 角色行为基类
/// </summary>
public class ActionBase
{
    /// <summary>
    /// 对应角色
    /// </summary>
    public RoleBase Get_RoleBase
    {
        get {
            return pRoleBase;
        }
    }
    protected RoleBase pRoleBase;
    /// <summary>
    /// 行为类名
    /// </summary>
    public string Get_CurActionName
    {
        get {
            return _CurActionName;
        }
    }
    protected string _CurActionName;
    /// <summary>
    /// 对应动画
    /// </summary>
    public string Get_CurAnimationName
    {
        get {
            return _CurAnimationName;
        }
        set {
            _CurAnimationName = value;
        }
    }
    protected string _CurAnimationName;
    /// <summary>
    /// 角色行为状态
    /// </summary>
    protected EActionStatus _CurStatus;
    /// <summary>
    /// 角色上一个行为状态
    /// </summary>
    protected EActionStatus _PreStatus;



    //行为方法//
    /// <summary>
    /// 初始化
    /// </summary>
    /// <param name="vRoleBase"></param>
    public virtual void SetInit(RoleBase vRoleBase, string vActionName)
    {
        pRoleBase = vRoleBase;
        _CurActionName = vActionName;
        Get_CurAnimationName = string.Empty;
        _PreStatus = EActionStatus.easNone;
        _CurStatus = EActionStatus.easNone;
        ActionSingleTonSet();
    }

    /// <summary>
    /// 激活
    /// </summary>
    public virtual bool SetActive()
    {
        if (_CurStatus != EActionStatus.easNone)
            return false;
        if (pRoleBase == null)
            return false;
        ChangeActionStatus(EActionStatus.easNormal);
        return true;
    }

    /// <summary>
    /// 停止
    /// </summary>
    public virtual bool SetStop()
    {
        if (_CurStatus == EActionStatus.easNone)
            return false;
        ChangeActionStatus(EActionStatus.easNone);
        return true;
    }

    /// <summary>
    /// 刷新
    /// </summary>
    public virtual void SetRefresh()
    { }

    /// <summary>
    /// 销毁
    /// </summary>
    public virtual void SetDestroy()
    {
        ChangeActionStatus(EActionStatus.easNone);
        pRoleBase = null;
    }

    /// <summary>
    /// 暂停
    /// </summary>
    public virtual void SetPause()
    {
        ChangeActionStatus(EActionStatus.easPause);
    }

    /// <summary>
    /// 继续
    /// </summary>
    public virtual void SetResume()
    {
        if (_PreStatus == EActionStatus.easPause)
            return;
        ChangeActionStatus(_PreStatus);
    }

    /// <summary>
    /// 行为唯一设置方法
    /// </summary>
    /// <param name="vIsSet">是否设置[true-设置 false-销毁]</param>
    protected virtual void ActionSingleTonSet()
    { }

    /// <summary>
    /// 修改行为状态
    /// </summary>
    /// <param name="vStatus"></param>
    protected void ChangeActionStatus(EActionStatus vStatus)
    {
        //判断是否与当前行为相同//
        if (vStatus == _CurStatus)
            return;
        _PreStatus = _CurStatus;
        _CurStatus = vStatus;
    }

    //委托回调方法//
    /// <summary>
    /// 帧事件回调
    /// </summary>
    protected virtual void Action_EventsEvent(string vAnimationName, string vEventName) { }
    /// <summary>
    /// 动作播放完成回调
    /// </summary>
    protected virtual void Action_EndEvent(string vAnimationName) { }
    /// <summary>
    /// 动作播放开始回调
    /// </summary>
    protected virtual void Action_StartEvent(string vAnimationName) { }


    //动画相关[需要设置当前播放动画以便在委托回调时作判断]//
    /// <summary>
    /// 强制添加动画
    /// </summary>
    /// <param name="vAnimationName">动画名字</param>
    /// <param name="vLoop">是否循环</param>
    protected void RepleaceAnimation(string vAnimationName, bool vLoop)
    {
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_MainSpine == null)
            return;
        if (string.IsNullOrEmpty(vAnimationName))
            return;
        if (!pRoleBase.Get_MainSpine.repleaceAnimation(vAnimationName, vLoop))
            return;
        Get_CurAnimationName = vAnimationName;
    }

    /// <summary>
    /// 添加延迟播放动画
    /// </summary>
    /// <param name="vAnimationName">动画名字</param>
    /// <param name="vLoop">是否循环</param>
    /// <param name="vDelayTime">延迟时间</param>
    protected void AddDelayAnimation(string vAnimationName, bool vLoop, float vDelayTime)
    {
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_MainSpine == null)
            return;
        if (string.IsNullOrEmpty(vAnimationName))
            return;
        if (!pRoleBase.Get_MainSpine.addDelayAnimation(vAnimationName, vLoop, vDelayTime))
            return;
        Get_CurAnimationName = vAnimationName;
    }

    /// <summary>
    /// 将动作加入动作序列 优先级越高越先播放
    /// </summary>
    /// <param name="vAnimationName">动画名字</param>
    /// <param name="vLoop">是否循环</param>
    /// <param name="vProprity">优先级</param>
    protected void PushAnimation(string vAnimationName, bool vLoop, uint vProprity = 0)
    {
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_MainSpine == null)
            return;
        if (string.IsNullOrEmpty(vAnimationName))
            return;
        if (!pRoleBase.Get_MainSpine.pushAnimation(vAnimationName, vLoop, vProprity))
            return;
        Get_CurAnimationName = vAnimationName;
    }

    /// <summary>
    /// 设置受击状态
    /// </summary>
    /// <param name="vHpChangeValue"></param>
    public virtual void SetHittedStatus(int vHpChangeValue)
    { }
}