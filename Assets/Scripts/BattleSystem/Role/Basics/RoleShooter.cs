using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 城堡射手
/// </summary>
public class RoleShooter : RoleBase
{

    private bool isDestroy;

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
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);

        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);
        if (this.gameObject.GetComponent<BoxCollider>() != null)
            this.gameObject.GetComponent<BoxCollider>().enabled = false;

        CastleAttributeInfo tmpShooterInfo = ConfigManager.Instance.mCastleConfig.FindByID(fightAttribute.KeyData);
        if (tmpShooterInfo == null)
            return;
        List<uint> tmpSkillIDList = new List<uint>();
        tmpSkillIDList.Add(tmpShooterInfo.unlock_positive.ID);
        //tmpSkillIDList.Add(tmpMonster.PassivitySkill);
        skillsDepot = new SkillsDepot();
        skillsDepot.multipleAdd(tmpSkillIDList);
        isDestroy = false;
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, CommandEvent_FinishOperate);
        base.UnInitialization(vIsDelete);
    }

    public override void InitAction()
    {
        AddSingleAction<ActionStatic>();
        AddSingleAction<ActionShooterFire>();
        AddSingleAction<ActionVictory>();
        AddSingleAction<ActionFailure>();
        
        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            ExChangeAction<ActionStatic>();
        else
            ExChangeAction<ActionShooterFire>();
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

    /// <summary>
    /// 死亡目标判断
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FinishOperate(object vDataObj)
    {
        FightDestroyInfo tmpInfo = (FightDestroyInfo)vDataObj;
        if (tmpInfo == null)
            return;

        if (tmpInfo._DestroyStatus != EPVEFinishStatus.epvefsDieBarracksSelf)
            return;

        isDestroy = true;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleDelete, pRoleBase.Get_SingleID);
    }
}
