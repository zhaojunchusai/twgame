using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Script.Common;

/// <summary>
/// 变形行为
/// </summary>
public class ActionDeformation : ActionBase
{

    private float moveDistance = 0; //移动距离//
    private float lastTime = 0;     //上次修改状态时间//
    private int actionStatus = 0;   //状态//

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        moveDistance = 0;
        lastTime = Time.time;
        actionStatus = UnityEngine.Random.Range(0, 3);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        return true;
    }

    public override bool SetStop()
    {
        if (!base.SetStop())
            return false;
        if (pRoleBase == null)
            return false;
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
        pRoleBase.CloseChangeSkeletonDataAssetOperate();
        return true;
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveUpdator(ActionAI);
        base.SetDestroy();
    }

    protected override void ActionSingleTonSet()
    {
        Scheduler.Instance.AddUpdator(ActionAI);
    }

    /// <summary>
    /// 行为AI
    /// </summary>
    private void ActionAI()
    {
        if (_CurStatus == EActionStatus.easAI)
        {
            if (pRoleBase != null)
            {
                if (Time.time - lastTime > 1)
                {
                    actionStatus = UnityEngine.Random.Range(0, 10);
                    lastTime = Time.time;
                }

                if ((actionStatus >= 2) && (actionStatus <= 7))
                {
                    IdleOperate();
                }
                else if (actionStatus <= 1)
                {
                    MoveOperate(true);
                }
                else
                {
                    MoveOperate(false);
                }
            }
        }
    }

    private void IdleOperate()
    {
        //设置方位//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
            pRoleBase.Get_Direction = ERoleDirection.erdRight;
        else
            pRoleBase.Get_Direction = ERoleDirection.erdLeft;
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
    }
    private void MoveOperate(bool vIsLeft)
    {
        //设置方位//
        if (vIsLeft)
        {
            pRoleBase.Get_Direction = ERoleDirection.erdLeft;
        }
        else
        {
            pRoleBase.Get_Direction = ERoleDirection.erdRight;
        }
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        //需要判断是否越界//
        moveDistance = 0;
        if (vIsLeft)
        {
            if (!pRoleBase.CheckIsOutSide(-0.01f))
            {
                moveDistance = -0.01f;
            }
        }
        else
        {
            if (!pRoleBase.CheckIsOutSide(0.01f))
            {
                moveDistance = 0.01f;
            }
        }
        pRoleBase.transform.localPosition += new Vector3(moveDistance, 0, 0);
    }

}