using UnityEngine;
using System.Collections;

/// <summary>
/// 角色-士兵
/// </summary>
public class RoleSoldier : RoleBase
{
    /// <summary>
    /// 战斗场景状态
    /// </summary>
    private ESceneStatus sceneStatus;

    //--------------------------------------------属性相关操作----------------------------------------------//
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

    protected override void FinalOperate()
    {
        ExChangeAction<ActionDeath>();
    }

    //--------------------------------------------角色相关操作----------------------------------------------//
    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightEndlessExcessive, CommandEvent_FightEndlessExcessive);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);

        Soldier tmpSoldier = null;
        if (vFightCamp == EFightCamp.efcSelf)
        {
            tmpSoldier = PlayerData.Instance._SoldierDepot.FindByUid(vUID);
        }
        else
        {
            if (vFightType == EFightType.eftExpedition)
                tmpSoldier = PlayerData.Instance.expeditionSoldierDopot.FindByUid(vUID);
            else if ((vFightType == EFightType.eftPVP) || (vFightType == EFightType.eftSlave) || 
                (vFightType == EFightType.eftServerHegemony) || (vFightType == EFightType.eftQualifying))
            {
                if ((UISystem.Instance.FightView != null) || (UISystem.Instance.FightView.fightPlayer_Enemy != null) || (UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList != null))
                {
                    for (int i = 0; i < UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList.Count; i++)
                    {
                        if (UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList[i].mSoldier.uId == vUID)
                            tmpSoldier = UISystem.Instance.FightView.fightPlayer_Enemy.mSoldierList[i].mSoldier;
                    }
                }
            }
        }
        if (tmpSoldier != null)
            skillsDepot = tmpSoldier._skillsDepot;
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightEndlessExcessive, CommandEvent_FightEndlessExcessive);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        base.UnInitialization(vIsDelete);
    }

    public override void InitAction()
    {
        sceneStatus = ESceneStatus.essNormal;

        AddSingleAction<ActionIdle>();
        AddSingleAction<ActionStatic>();
        AddSingleAction<ActionAIMove>();
        AddSingleAction<ActionAIFire>();
        AddSingleAction<ActionDeath>();
        AddSingleAction<ActionVictory>();
        AddSingleAction<ActionFailure>();
        AddSingleAction<ActionExcessive>();
        AddSingleAction<ActionDeformation>();

        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            ExChangeAction<ActionStatic>();
        else
            ExChangeAction<ActionAIMove>();
    }

    public override void ReSetRoleAction(string vActionName = "")
    {
        if (ShowFightFinishedStatus(sceneStatus))
            return;
        if (!string.IsNullOrEmpty(vActionName))
        {
            ExChangeAction(vActionName);
        }
        else
        {
            if (_CurrentActionName.Equals(typeof(ActionIdle).Name))
            {
                ExChangeAction<ActionAIFire>();
            }
            else if (_CurrentActionName.Equals(typeof(ActionAIMove).Name))
            {
                if (Get_Enemy == null)
                    ExChangeAction<ActionIdle>();
                else
                    ExChangeAction<ActionAIFire>();
            }
            else if (_CurrentActionName.Equals(typeof(ActionAIFire).Name))
            {
                if (isOpenCheckEnemy)
                {
                    if (Get_Enemy != null)
                        return;
                }
                ExChangeAction<ActionAIMove>();
            }
        }
    }

    public override float GetRoleActualSpeed()
    {
        float tmpResult = base.GetRoleActualSpeed();
        return tmpResult;
    }

    public override void ChangeAction()
    {
        base.ChangeAction();
        if (changeStatus != EChangeStatus.ecsNone)
        {
            ExChangeAction<ActionDeformation>();
        }
        else
        {
            ExChangeAction<ActionAIMove>();
        }
    }

    //--------------------------------------------命令操作函数----------------------------------------------//
    /// <summary>
    /// 战斗结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightFinished(object vDataObj)
    {
        if (vDataObj == null)
            return;
        if (!this.IsLive())
            return;
        sceneStatus = ((FightFinishedInfo)vDataObj)._SceneStatus;
        ShowFightFinishedStatus(sceneStatus);
    }

    /// <summary>
    /// 开启无尽战斗过度模式
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightEndlessExcessive(object vDataObj)
    {
        if (!IsLive())
            return;
        ExChangeAction<ActionExcessive>();
    }

    private void CommandEvent_CloseShowBossTalk(object vDataObj)
    {
        if (!_CurrentActionName.Equals("ActionDeformation"))
        {
            ExChangeAction<ActionAIMove>();
        }
    }

    private bool ShowFightFinishedStatus(ESceneStatus vSceneStatus)
    {
        if (vSceneStatus == ESceneStatus.essVictory)
        {
            if (roleCamp == EFightCamp.efcSelf)
                ExChangeAction<ActionVictory>();
            else
                ExChangeAction<ActionFailure>();
        }
        else if (vSceneStatus == ESceneStatus.essFailure)
        {
            if (roleCamp == EFightCamp.efcSelf)
                ExChangeAction<ActionFailure>();
            else
                ExChangeAction<ActionVictory>();
        }
        else
        {
            return false;
        }
        return true;
    }

}
