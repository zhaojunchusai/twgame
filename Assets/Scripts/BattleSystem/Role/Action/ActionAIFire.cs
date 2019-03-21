using UnityEngine;
using System.Collections;
using System;
using System.Threading;
using Assets.Script.Common;
using System.Collections.Generic;

/// <summary>
/// 角色行为-AI攻击
/// </summary>
public class ActionAIFire : ActionBase
{
    /// <summary>
    /// 默认攻击音效
    /// </summary>
    private bool isActive = false;

    /// <summary>
    /// 是否已经开启攻击
    /// </summary>
    private bool isStartFire;

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        isStartFire = false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
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
        Scheduler.Instance.AddTimer(UnityEngine.Random.Range(0.08f, 0.2f), true, ActionAI);
    }

    protected override void Action_EventsEvent(string vAnimationName, string vEventName)
    {
        if (_CurStatus == EActionStatus.easNone)
            return;
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_FightAttribute == null)
            return;
        if (!GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName))
            return;
        if (!vEventName.Equals(GlobalConst.ANIMATION_EVENT_PROJECTILE) && !vEventName.Equals(GlobalConst.ANIMATION_EVENT_PORJECTILE))
            return;
        
        //判断攻击者攻击方式[英雄默认近程攻击]//
        ERoleSeat tmpStance = ERoleSeat.ersNear;
        string tmpAttEffectName = string.Empty;
        string tmpTrajectoryMusic = GlobalConst.Sound.DEFAULT_ATT_AUDIO;
        ETrajectoryType tmpTrajectoryType = ETrajectoryType.ettNone;
        if (pRoleBase.Get_RoleType == ERoleType.ertSoldier)
        {//武将//
            SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(pRoleBase.Get_FightAttribute.KeyData);
            if (tmpSoldier == null) return;
            tmpStance = (ERoleSeat)tmpSoldier.Stance;
            tmpAttEffectName = tmpSoldier.trajectory;
            tmpTrajectoryType = (ETrajectoryType)tmpSoldier.trajectoryType;
            tmpTrajectoryMusic = tmpSoldier.trajectoryMusic;
        }
        else if (pRoleBase.Get_RoleType == ERoleType.ertMonster)
        {//怪物//
            MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(pRoleBase.Get_FightAttribute.KeyData);
            if (tmpMonster == null) return;
            tmpStance = (ERoleSeat)tmpMonster.Seat;
            tmpAttEffectName = tmpMonster.trajectory;
            tmpTrajectoryType = (ETrajectoryType)tmpMonster.trajectoryType;
            tmpTrajectoryMusic = tmpMonster.trajectoryMusic;
        }

        if (tmpStance == ERoleSeat.ersNear)
            AttStatus_Near();
        else if (tmpStance == ERoleSeat.ersMiddle)
            AttStatus_Middle(tmpTrajectoryType,tmpAttEffectName);
        else
            AttStatus_Far(tmpTrajectoryType, tmpAttEffectName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(tmpTrajectoryMusic, pRoleBase.transform));
    }

    protected override void Action_EndEvent(string vAnimationName)
    {
        //检测是否当前行为动画//
        if (pRoleBase == null)
            return;
        bool tmpCheckName = false;
        if ((GlobalConst.ANIMATION_NAME_ABILITY_BASIC.Equals(vAnimationName)) || (GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE.Equals(vAnimationName)))
        {
            tmpCheckName = true;
        }
        else
        {
            for (int i = 0; i < GlobalConst.ANIMATION_NAME_SKILL_ARR.Length; i++)
            {
                if (GlobalConst.ANIMATION_NAME_SKILL_ARR[i].Equals(vAnimationName))
                {
                    tmpCheckName = true;
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightHeroSkillFinished, null);
                    break;
                }
            }
        }
        if (!tmpCheckName)
            return;

        isStartFire = false;
        //播放待机动画//
        PushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, GlobalConst.ANIMATION_TRACK_BASE);
    }
    private void AttStatus_Near()
    {
        AttCallBackEvent();
    }
    private void AttStatus_Middle(ETrajectoryType vType, string vAttEffectName)
    {
        //AttCallBackEvent();
        if (pRoleBase == null)
            return;
        if (string.IsNullOrEmpty(vAttEffectName) || vAttEffectName.Equals("0"))
        {
            AttCallBackEvent();
            return;
        }
        //武将长武器直线攻击弹道[不考虑Y轴]//
        Traectory tmpTraectory;

        switch(vType)
        {
            case ETrajectoryType.ettArrow: tmpTraectory = new Parabola(); break;
            case ETrajectoryType.ettMagic: tmpTraectory = new PerpenDicular(); break;
            case ETrajectoryType.ettATtack: tmpTraectory = new TransverseLine(); break;
            default: tmpTraectory = new TransverseLine(); break;
        }
        if (pRoleBase.Get_Enemy == null)
        {
            pRoleBase.CheckEnemyIsChange();
            if (pRoleBase.Get_Enemy == null)
                return;
        }
        tmpTraectory.Instance(SkillTool.GetWorldPosition(pRoleBase, 3), pRoleBase.Get_Enemy, vAttEffectName);
        tmpTraectory.ColliderEvent += AttCallBackEvent;
    }
    private void AttStatus_Far(ETrajectoryType vType, string vAttEffectName)
    {
        if (pRoleBase == null)
            return;

        Traectory tmpTraectory;
        if (ETrajectoryType.ettArrow == vType)
            tmpTraectory = new Parabola();
        else if (ETrajectoryType.ettMagic == vType)
            tmpTraectory = new PerpenDicular();
        else
            return;

        if (pRoleBase.Get_Enemy == null)
        {
            pRoleBase.CheckEnemyIsChange();
            if (pRoleBase.Get_Enemy == null)
                return;
        }
        tmpTraectory.Instance(SkillTool.GetWorldPosition(pRoleBase, 3), pRoleBase.Get_Enemy, vAttEffectName);
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
        if (!CheckIsCanFire())
            return;
        if (CheckFightIsCDTime())
            return;

        //重置攻击时间//
        isStartFire = true;
        pRoleBase._PreTime = Time.time;
        if (CommonFunction.RandomSkill(pRoleBase, pRoleBase.Get_skillsDepot))
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE, false);
        else
        {
            RepleaceAnimation(GlobalConst.ANIMATION_NAME_ABILITY_BASIC, false);
        }
    }

    /// <summary>
    /// 攻击回调
    /// </summary>
    /// <param name="id"></param>
    private void AttCallBackEvent(int id = 0)
    {
        if (pRoleBase == null)
            return;
        bool tmpIsChange = pRoleBase.CheckEnemyIsChange();
        RoleAttribute tmpEnemy = pRoleBase.Get_Enemy;
        if (tmpEnemy == null)
            return;
        if (tmpEnemy.Get_RoleType == ERoleType.ertTransfer)
        {
            pRoleBase.ReSetRoleAction();
            SetStop();
            return;
        }
        //if (tmpIsChange)
        //    return;
        if(pRoleBase.tmpSkill == null)
        {
            HurtType hurtResult = pRoleBase.CalculationCommonHurt(tmpEnemy);
            tmpEnemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, hurtResult);
        }
        else
        {
            pRoleBase.tmpSkill.UseSkill(pRoleBase);
        }

    }

    /// <summary>
    /// 检测战斗CD时间是否达成
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
    /// 检测战斗条件是否达成
    /// </summary>
    private bool CheckIsCanFire()
    {
        if (isStartFire)
            return false;

        if (pRoleBase == null)
            return false;

        if (pRoleBase.Get_IsSkilling)
            return false;

        if (!pRoleBase.IsLive())
        {
            SetStop();
            return false;
        }

        if (pRoleBase.CheckIsOutSide())
        {
            pRoleBase.ReSetRoleAction();
            SetStop();
            return false;
        }

        if (!pRoleBase.Get_isOpenCheckEnemy)
        {
            pRoleBase.ReSetRoleAction();
            SetStop();
            return false;
        }

        pRoleBase.CheckEnemyIsChange();
        if (pRoleBase.Get_Enemy == null)
        {
            pRoleBase.ReSetRoleAction();
            SetStop();
            return false;
        }
        else
        {
            if (pRoleBase.Get_Enemy.Get_RoleType == ERoleType.ertTransfer)
            {
                pRoleBase.ReSetRoleAction();
                SetStop();
                return false;
            }
        }

        return true;
    }
}