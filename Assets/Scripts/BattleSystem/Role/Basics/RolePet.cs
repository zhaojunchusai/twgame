using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 宠物
/// </summary>
public class RolePet : RoleBase
{
    private bool isDestroy;
    private RoleBase tmpTarget;
    private Vector3 leftPos;
    private Vector3 rightPos;
    protected override void InitRoleCoefficient()
    {
        //需要放在子类中进行设置//
        _Coeff_Attack_Hurt = 1;
        _Coeff_Attack_Restraint = 1;
        _Coeff_Attack_Crit = 1.5f;
        _Coeff_Attack_AccuracyRate_1 = 0.8f;
        _Coeff_Attack_AccuracyRate_2 = 0.45f;
        _Coeff_Attack_CritRate_1 = 0.8f;
        _Coeff_Attack_CritRate_2 = -0.35f;
        _Coeff_Skill_Hurt = 1;
        _Coeff_Skill_Restraint = 1;
        _Coeff_Skill_Passive = 0;
    }

    public override void InitRoleAttribute(ShowInfoBase vFightAttribute)
    {
        base.InitRoleAttribute(vFightAttribute);
    }

    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);
        if (this.gameObject.GetComponent<BoxCollider>() != null)
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

        SoldierAttributeInfo tmpShooterInfo = ConfigManager.Instance.mSoldierData.FindById(fightAttribute.KeyData);
        if (tmpShooterInfo == null)
            return;
        List<uint> tmpSkillIDList = new List<uint>();
        tmpSkillIDList.Add(tmpShooterInfo.initiativeSkill);
        //tmpSkillIDList.Add(tmpMonster.PassivitySkill);
        skillsDepot = new SkillsDepot();
        skillsDepot.multipleAdd(tmpSkillIDList);
        isDestroy = false;
    }
    public override void StartSummonPet(RoleAttribute target, int attack)
    {
        base.StartSummonPet(target, attack);
        if (target == null || target.Get_MainSpine == null)
            return;

        this.transform.parent = target.transform;
        this.leftPos = new Vector3(-target.Get_MainSpine.GetStartSize().x * SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X * 2, 0, 0);
        this.rightPos = new Vector3(target.Get_MainSpine.GetStartSize().x * SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X * 2, 0, 0);
        this.transform.localPosition = leftPos;
        this.tmpTarget = target as RoleBase;
        if (this.tmpTarget == null)
            return;
        //this.tmpTarget.ActionEvent += OnAction;
        this.tmpTarget.Get_MainSpine.StartEvent += Get_MainSpine_StartEvent;
        pRoleBase.Get_MainSpine.EventsEvent += Action_EventsEvent;
        this.tmpTarget.ChangeEulerAngles += tmpTarget_ChangeEulerAngles;
        if (this.tmpTarget.Get_Direction == ERoleDirection.erdLeft)
        {
            this.transform.localEulerAngles = new Vector3(0,0,0);
        }
        tmpTarget_ChangeEulerAngles();
        this._MainSpine.repleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
    }

    void tmpTarget_ChangeEulerAngles()
    {
        if (this.tmpTarget.Get_Direction == ERoleDirection.erdLeft)
            this.transform.localPosition = rightPos;
        else
            this.transform.localPosition = leftPos;
    }
    void Get_MainSpine_StartEvent(string animationName)
    {
        switch(animationName)
        {
            case GlobalConst.ANIMATION_NAME_IDLE: 
                this._MainSpine.repleaceAnimation(animationName, true);
                break;
            case GlobalConst.ANIMATION_NAME_MOVE: 
                this._MainSpine.repleaceAnimation(animationName, true);
                break;
            case GlobalConst.ANIMATION_NAME_ABILITY_BASIC: 
                this._MainSpine.repleaceAnimation(animationName, true);
                break;
            default: this._MainSpine.repleaceAnimation(GlobalConst.ANIMATION_NAME_IDLE, true);
                break;
        }
    }
    protected void Action_EventsEvent(string vAnimationName, string vEventName)
    {
        if (!vAnimationName.Equals(GlobalConst.ANIMATION_NAME_ABILITY_BASIC))
            return;
        if (!vEventName.Equals(GlobalConst.ANIMATION_EVENT_PROJECTILE) && !vEventName.Equals(GlobalConst.ANIMATION_EVENT_PORJECTILE))
            return;
        if (this.tmpTarget == null || this.tmpTarget.Get_Enemy == null)
            return;
        SoldierAttributeInfo tmpSoldier = ConfigManager.Instance.mSoldierData.FindById(pRoleBase.Get_FightAttribute.KeyData);
        if (tmpSoldier == null) return;

        if (tmpSoldier.Stance == (int)ERoleSeat.ersNear)
        {
            if (pRoleBase.tmpSkill == null)
            {
                HurtType hurtResult = pRoleBase.CalculationCommonHurt(this.tmpTarget.Get_Enemy);
                this.tmpTarget.Get_Enemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, hurtResult);
            }
            else
            {
                pRoleBase.tmpSkill.UseSkill(pRoleBase);
            }
        }
        else
        {
            Traectory tmpTraectory;
            if (ETrajectoryType.ettArrow == (ETrajectoryType)tmpSoldier.trajectoryType)
                tmpTraectory = new Parabola();
            else if (ETrajectoryType.ettMagic == (ETrajectoryType)tmpSoldier.trajectoryType)
                tmpTraectory = new PerpenDicular();
            else
                return;
            tmpTraectory.Instance(SkillTool.GetWorldPosition(pRoleBase, 3), this.tmpTarget.Get_Enemy, tmpSoldier.trajectory);
            tmpTraectory.ColliderEvent += AttCallBackEvent;
        }
    }
    private void AttCallBackEvent(int id)
    {
        if (pRoleBase == null)
            return;
        if (this.tmpTarget == null || this.tmpTarget.Get_Enemy == null)
            return;
        if (pRoleBase.tmpSkill == null)
        {
            HurtType hurtResult = pRoleBase.CalculationCommonHurt(this.tmpTarget.Get_Enemy);
            this.tmpTarget.Get_Enemy.ReSetRoleHP(-(int)pRoleBase._AttackHurt, hurtResult);
        }
        else
        {
            pRoleBase.tmpSkill.UseSkill(pRoleBase);
        }
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        base.UnInitialization();
        if (this.tmpTarget != null && this.tmpTarget.Get_MainSpine != null)
            this.tmpTarget.Get_MainSpine.StartEvent -= Get_MainSpine_StartEvent;
        if (pRoleBase != null && pRoleBase.Get_MainSpine != null)
            pRoleBase.Get_MainSpine.EventsEvent += Action_EventsEvent;
        if(this.tmpTarget != null)
            this.tmpTarget.ChangeEulerAngles -= tmpTarget_ChangeEulerAngles;
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightFinished(object vDataObj)
    {
        if (isDestroy)
            return;
        if (vDataObj == null)
            return;
        ESceneStatus tmpSceneStatus = ((FightFinishedInfo)vDataObj)._SceneStatus;
        Get_CurrentAction.SetStop();

        if (tmpSceneStatus == ESceneStatus.essVictory)
        {
            ExChangeAction<ActionVictory>();
        }
        else if (tmpSceneStatus == ESceneStatus.essFailure)
        {
            ExChangeAction<ActionFailure>();
        }
    }
}
