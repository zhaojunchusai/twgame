using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 角色行为-无尽模式过度
/// </summary>
public class ActionExcessive : ActionBase
{

    /// <summary>
    /// 限制移动坐标
    /// </summary>
    private float limitMovePosX;
    /// <summary>
    /// 当前时间
    /// </summary>
    private float curTime;

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        if (pRoleBase == null)
            return false;

        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        curTime = Time.time;

        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        return true;
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveUpdator(ActionAI);
        base.SetDestroy();
    }

    public override void SetResume()
    {
        base.SetResume();
        curTime = Time.time;
    }

    protected override void ActionSingleTonSet()
    {
        Scheduler.Instance.AddUpdator(ActionAI);
    }

    /// <summary>
    /// 检测是否移出屏幕
    /// </summary>
    private void ActionAI()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;
        pRoleBase.transform.localPosition += new Vector3(pRoleBase.Get_FightAttribute.MoveSpeed * (Time.time - curTime), 0, 0);
        curTime = Time.time;
    }

}
