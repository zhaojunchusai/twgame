using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 角色-AI英雄
/// </summary>
public class RoleAIHero_PVE : RoleBase
{
    /// <summary>
    /// 是否正在移动
    /// </summary>
    private bool isMoving = false;
    /// <summary>
    /// 是否自动战斗
    /// </summary>
    private int autoStatus = 0;
    /// <summary>
    /// 宠物数据
    /// </summary>
    private PetData pPetData;

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

        isMoving = false;
        autoStatus = 0;
    }

    public override void InitRoleAttribute(ShowInfoBase vFightAttribute)
    {
        base.InitRoleAttribute(vFightAttribute);
        if (EffectImmuneManage != null)
            EffectImmuneManage.AddFilter(SkillFilter_Hero);
    }

    protected override void FinalOperate()
    {
        ExChangeAction<ActionDeath>();
    }

    //--------------------------------------------角色相关操作----------------------------------------------//
    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_ChangeFightAutoStatus, CommandEvent_ChangeAutoStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, CommandEvent_StartMove);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, CommandEvent_StopMove);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightUseSkill, CommandEvent_UseSkill);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightPetUseSkill, CommandEvent_PetUseSkill);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightHeroSkillFinished, CommandEvent_SkillFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightEndlessExcessive, CommandEvent_FightEndlessExcessive);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);

        if (UISystem.Instance.FightView == null)
            return;
        //激活光环 换装//
        if (PlayerData.Instance._SkillsDepot != null)
            PlayerData.Instance._SkillsDepot.ActivateHalo(this);
        System.Collections.Generic.List<LifeSoulData> tmpList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        if (tmpList != null)
        {
            for (int i = 0; i < tmpList.Count; ++i)
            {
                LifeSoulData tt = tmpList[i];
                if (tt != null && tt.Skill != null)
                    tt.Skill.UseSkill(this);
            }
        }
        pPetData = UISystem.Instance.FightView.pPetData;
        if (_MainSpine != null)
        {
            if ((PlayerData.Instance.IsShowPet) && (pPetData != null) && (pPetData.PetInfo != null))
            {
                _MainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot, pPetData.PetInfo.res_name, PlayerData.Instance.IsShowPet);
            }
            else
            {
                _MainSpine.RepleaceEquipment(PlayerData.Instance._ArtifactedDepot);
            }
        }
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_ChangeFightAutoStatus, CommandEvent_ChangeAutoStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleMove_Start, CommandEvent_StartMove);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, CommandEvent_StopMove);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightUseSkill, CommandEvent_UseSkill);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightPetUseSkill, CommandEvent_PetUseSkill);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightHeroSkillFinished, CommandEvent_SkillFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightEndlessExcessive, CommandEvent_FightEndlessExcessive);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        base.UnInitialization(false);
    }

    public override void InitAction()
    {
        AddSingleAction<ActionStatic>();
        AddSingleAction<ActionHeroAI_PVE>();
        AddSingleAction<ActionDeath>();
        AddSingleAction<ActionVictory>();
        AddSingleAction<ActionFailure>();

        AddSingleAction<ActionIdle>();
        AddSingleAction<ActionHeroMove>();
        AddSingleAction<ActionAIFire>();
        AddSingleAction<ActionExcessive>();
        AddSingleAction<ActionDialogue>();

        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            ExChangeAction<ActionStatic>();
        else
            ReSetHeroActionByAutoStatus();
    }

    public override void ReSetRoleAction(string vActionName = "")
    {
        if (!string.IsNullOrEmpty(vActionName))
        {
            ExChangeAction(vActionName);
        }
        else
        {
            if (_CurrentActionName.Equals(typeof(ActionAIFire).Name))
            {
                ExChangeAction<ActionIdle>();
            }
            else if (_CurrentActionName.Equals(typeof(ActionIdle).Name))
            {
                if (Get_Enemy == null)
                    return;
                ExChangeAction<ActionAIFire>();
            }
        }
    }

    /// <summary>
    /// 根据角色自动战斗状态设定行为
    /// </summary>
    private void ReSetHeroActionByAutoStatus()
    {
        if (!IsLive())
            return;
        if (_CurrentActionName.Equals(typeof(ActionHeroAI_PVE).Name))
        {
            Get_CurrentAction.SetRefresh();
        }
        if ((UISystem.Instance.FightView.Get_IsAutoMove) || (UISystem.Instance.FightView.Get_IsAutoSkill) || (UISystem.Instance.FightView.Get_IsAutoSummon))
        {
            if (_CurrentActionName.Equals(typeof(ActionHeroAI_PVE).Name))
            {
                Get_CurrentAction.SetRefresh();
            }
            else
            {
                ExChangeAction<ActionHeroAI_PVE>();
            }
        }
        else
        {
            if (roleCamp == EFightCamp.efcSelf)
                Get_Direction = ERoleDirection.erdRight;
            else
                Get_Direction = ERoleDirection.erdLeft;
            ExChangeAction<ActionIdle>();
        }
    }

    //--------------------------------------------命令操作函数----------------------------------------------//
    private void CommandEvent_ChangeAutoStatus(object vDataObj)
    {
        if (vDataObj == null)
            return;
        autoStatus = (int)vDataObj;
        ReSetHeroActionByAutoStatus();
    }

    /// <summary>
    /// 开启角色移动
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartMove(object vDataObj)
    {
        if (vDataObj == null)
            return;
        if (UISystem.Instance.FightView.Get_FightIsPause)
            return;
        if (isSkilling)
            return;
        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            return;
        if ((!string.IsNullOrEmpty(_CurrentActionName)) && (_CurrentActionName.Equals(typeof(ActionDeath).Name)))
            return;
        if (pSceneManager.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;
        isMoving = true;
        Get_Direction = (ERoleDirection)vDataObj;
        ExChangeAction<ActionHeroMove>();
    }

    /// <summary>
    /// 关闭角色移动
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StopMove(object vDataObj)
    {
        if (UISystem.Instance.FightView.Get_FightIsPause)
            return;
        if (vDataObj == null)
            return;
        if ((!string.IsNullOrEmpty(_CurrentActionName)) && (_CurrentActionName.Equals(typeof(ActionDeath).Name)))
            return;
        if (pSceneManager.Get_CurScene.Get_SceneStatus != ESceneStatus.essNormal)
            return;
        isMoving = false;
        ReSetHeroActionByAutoStatus();
    }

    /// <summary>
    /// 英雄使用技能
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_UseSkill(object vDataObj)
    {
        if (!this.IsLive())
            return;
        UseSkillInfo tmpInfo = (UseSkillInfo)vDataObj;
        if (tmpInfo == null)
            return;
        if (tmpInfo.SkillEquip == null)
            return;
        if (tmpInfo.SkillEquip._skill == null)
            return;
        if (string.IsNullOrEmpty(tmpInfo.SkillEquip.Att.skillAnimation))
            return;
        if (tmpInfo.SkillEquip.Att.skillAnimation.Equals("0"))
            return;
        if (roleCamp != tmpInfo.RoleCamp)
            return;

        if (roleCamp == EFightCamp.efcSelf)
            Get_Direction = ERoleDirection.erdRight;
        else
            Get_Direction = ERoleDirection.erdLeft;
        if (isMoving)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, ERoleAction.eraAIIdle);
        isSkilling = true;
        SetSpineTimeScale(ESpineSpeedType.esstSkill);

        tmpInfo.SkillEquip._skill.UseSkill(this);
        if (_MainSpine != null)
        {
            _MainSpine.RepleaceEquipment(tmpInfo.SkillEquip.Att.id);
            _MainSpine.repleaceAnimation(tmpInfo.SkillEquip.Att.skillAnimation, false);
        }
    }
    /// <summary>
    /// 宠物使用技能
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_PetUseSkill(object vDataObj)
    {
        if (!this.IsLive())
            return;
        if ((pPetData == null) || (pPetData.PetInfo == null))
            return;
        if (roleCamp == EFightCamp.efcSelf)
            Get_Direction = ERoleDirection.erdRight;
        else
            Get_Direction = ERoleDirection.erdLeft;
        if (isMoving)
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleMove_Stop, ERoleAction.eraAIIdle);
        isSkilling = true;
        if (_MainSpine != null)
        {
            _MainSpine.repleaceAnimation(pPetData.PetInfo.skill_anim, false);
        }
    }

    /// <summary>
    /// 技能释放完毕
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_SkillFinished(object vDataObj)
    {
        isSkilling = false;
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

    /// <summary>
    /// Boss出场
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_BossInScene(object vDataObj)
    {
        Effects buff = new BuffEffect();
        EffectInfo tmpInfo = new EffectInfo();
        tmpInfo.effectId = 203;
        tmpInfo.durTime = 4000;
        tmpInfo.percent = 0;
        buff.initialize(tmpInfo, 1, this);
        List<RoleAttribute> tmpList = new List<RoleAttribute>();
        tmpList.Add(this);
        buff.DoEffect(tmpList);

        Effects buff_Skill = new BuffEffect();
        EffectInfo tmpInfo_Skill = new EffectInfo();
        tmpInfo_Skill.effectId = 204;
        tmpInfo_Skill.durTime = 4000;
        tmpInfo_Skill.percent = 0;
        buff_Skill.initialize(tmpInfo_Skill, 1, this);
        List<RoleAttribute> tmpList_Skill = new List<RoleAttribute>();
        tmpList_Skill.Add(this);
        buff_Skill.DoEffect(tmpList_Skill);

        ExChangeAction<ActionDialogue>();
    }

    /// <summary>
    /// 关闭BOSS对话显示
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_CloseShowBossTalk(object vDataObj)
    {
        ReSetHeroActionByAutoStatus();
    }

    /// <summary>
    /// 战斗结束
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_FightFinished(object vDataObj)
    {
        if (!this.IsLive())
            return;
        if (vDataObj == null)
            return;
        ESceneStatus tmpSceneStatus = ((FightFinishedInfo)vDataObj)._SceneStatus;
        switch (tmpSceneStatus)
        {
            case ESceneStatus.essVictory:
                {
                    if (roleCamp == EFightCamp.efcSelf)
                        ExChangeAction<ActionVictory>();
                    else
                        ExChangeAction<ActionFailure>();
                }
                break;
            case ESceneStatus.essFailure:
                {
                    if (roleCamp == EFightCamp.efcSelf)
                        ExChangeAction<ActionFailure>();
                    else
                        ExChangeAction<ActionVictory>();
                }
                break;
            default:
                {
                    ExChangeAction<ActionFailure>();
                }
                break;
        }
    }

}
