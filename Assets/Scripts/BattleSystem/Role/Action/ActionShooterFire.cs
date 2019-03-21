using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using Assets.Script.Common;
using System.Collections.Generic;

/// <summary>
/// 城堡射手攻击行为
/// </summary>
public class ActionShooterFire : ActionBase
{
    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, GlobalConst.ANIMATION_TRACK_BASE);
        pRoleBase._PreTime = Time.time;
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        return true;
    }

    public override void SetDestroy()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
        {
            pRoleBase.Get_MainSpine.EndEvent -= Action_EndEvent;
            pRoleBase.Get_MainSpine.EventsEvent -= Action_EventsEvent;
        }
        Scheduler.Instance.RemoveTimer(ActionAI);
        base.SetDestroy();
    }

    public override void SetResume()
    {
        base.SetResume();
        if (pRoleBase != null)
            pRoleBase._PreTime = Time.time;
    }

    protected override void ActionSingleTonSet()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
        {
            pRoleBase.Get_MainSpine.EndEvent += Action_EndEvent;
            pRoleBase.Get_MainSpine.EventsEvent += Action_EventsEvent;
        }
        Scheduler.Instance.AddTimer(0.1f, true, ActionAI);
    }

    protected override void Action_EventsEvent(string vAnimationName, string vEventName)
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if (pRoleBase == null)
            return;
        //检测是否当前行为动画//
        if (!GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName))
            return;
        if (!vEventName.Equals(GlobalConst.ANIMATION_EVENT_PROJECTILE) && !vEventName.Equals(GlobalConst.ANIMATION_EVENT_PORJECTILE))
            return;
        AttStatus_Far();
    }

    protected override void Action_EndEvent(string vAnimationName)
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if (pRoleBase == null)
            return;
        //检测是否当前行为动画//
        if (!GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName))
            return;
        //重置攻击时间//
        pRoleBase._PreTime = Time.time;
        //播放待机动画//
        PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, GlobalConst.ANIMATION_TRACK_BASE);
    }

    private void AttStatus_Far()
    {
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_Enemy == null)
            return;

        Traectory tmpTraectory = new Parabola();
        tmpTraectory.Instance(SkillTool.GetWorldPosition(pRoleBase, 3), pRoleBase.Get_Enemy, "effect_arrow_ding_feng.assetbundle");
        //tmpTraectory.SetSpeed(8);
        tmpTraectory.ColliderEvent += AttCallBackEvent;

    }

    /// <summary>
    /// 检测AI
    /// </summary>
    private void ActionAI()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;
        //检测战斗条件是否达成//
        if (!CheckIsCanFire())
            return;
        //检测时间是否达成//
        if (CheckFightIsCDTime())
            return;
        //检测是否使用技能//
        if (CommonFunction.RandomSkill(pRoleBase, pRoleBase.Get_skillsDepot))
        {
            //重置攻击时间//
            pRoleBase._PreTime = Time.time;
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE, false);
            return;
        }
        //重新开始攻击//
        //PushAnimation(GlobalConst.ANIMATION_NAME_ABILITY_BASIC, false, GlobalConst.ANIMATION_TRACK_SKILL);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_ABILITY_BASIC, false);
    }

    /// <summary>
    /// 远程攻击回调
    /// </summary>
    /// <param name="id"></param>
    private void AttCallBackEvent(int id)
    {
        if (pRoleBase == null)
            return;
        //检测敌人是否存在//
        bool tmpIsChange = pRoleBase.CheckEnemyIsChange();
        RoleAttribute tmpEnemy = pRoleBase.Get_Enemy;
        //if ((tmpEnemy != null) && (!tmpIsChange))
        if (tmpEnemy != null)
        {
            if (pRoleBase.tmpSkill == null)
            {
                HurtType hurtResult = pRoleBase.CalculationCommonHurt(tmpEnemy);
                tmpEnemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, hurtResult);
            }
            else
            {
                pRoleBase.tmpSkill.UseSkill(pRoleBase);
            }
        }
    }

    /// <summary>
    /// 检测战斗时间
    /// </summary>
    /// <returns>true-CD时间 false-超过CD</returns>
    private bool CheckFightIsCDTime()
    {
        if (pRoleBase == null)
            return true;
        if ((pRoleBase._PreTime + pRoleBase._AttRateTime / SceneManager.Instance.Get_FightSpeed) > Time.time)
            return true;
        return false;
    }

    /// <summary>
    /// 检测战斗条件
    /// </summary>
    private bool CheckIsCanFire()
    {
        if (pRoleBase == null)
            return false;

        //检测敌方是否死亡//
        pRoleBase.CheckEnemyIsChange();
        if (pRoleBase.Get_Enemy == null)
            return false;

        return true;
    }
}
