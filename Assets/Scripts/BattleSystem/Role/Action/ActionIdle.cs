using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 角色行为-待机[原地静止 带攻击检测 攻击完还原到待机]
/// </summary>
public class ActionIdle : ActionBase
{

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        return true;
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveTimer(ActionAI);
        base.SetDestroy();
    }

    protected override void ActionSingleTonSet()
    {
        Scheduler.Instance.AddTimer(0.1f, true, ActionAI);
    }

    /// <summary>
    /// 行为AI
    /// </summary>
    private void ActionAI()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;

        //查找敌人//
        pRoleBase.CheckEnemyIsChange();
        if (pRoleBase.Get_Enemy != null)
        {
            pRoleBase.ReSetRoleAction();
            SetStop();
            return;
        }
    }

}
