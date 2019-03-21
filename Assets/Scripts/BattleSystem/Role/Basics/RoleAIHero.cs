using UnityEngine;
using System.Collections;

/// <summary>
/// 角色-AI英雄
/// </summary>
public class RoleAIHero : RoleBase
{

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
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightStartAIHero, CommandEvent_StartAIHero);
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);

        if (UISystem.Instance.FightView == null)
            return;

        //激活光环 换装//
        if (roleCamp == EFightCamp.efcSelf)
        {
            if (UISystem.Instance.FightView.fightPlayer_Self == null)
                return;
            if (UISystem.Instance.FightView.fightPlayer_Self.mSkill == null)
                return;
            UISystem.Instance.FightView.fightPlayer_Self.mSkill.ActivateHalo(this);
            System.Collections.Generic.List<LifeSoulData> tmpList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
            if(tmpList != null)
            {
                for (int i = 0; i < tmpList.Count; ++i)
                {
                    LifeSoulData tt = tmpList[i];
                    if (tt != null && tt.Skill != null)
                        tt.Skill.UseSkill(this);
                }
            }
            if (_MainSpine != null)
            {
                pPetData = UISystem.Instance.FightView.pPetData;
                //if ((PlayerData.Instance.IsShowPet) && (pPetData != null) && (pPetData.PetInfo != null))
                //{
                //    _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip, pPetData.PetInfo.res_name, PlayerData.Instance.IsShowPet);
                //}
                //else
                //{
                //    _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip);
                //}
                if ((pPetData != null) && (pPetData.PetInfo != null))
                {
                    if (PlayerData.Instance._PetDepot.GetEquipedPet() == null)
                    {
                        _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip, pPetData.PetInfo.res_name, true);
                    }
                    else
                    {
                        if (PlayerData.Instance.IsShowPet)
                        {
                            _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip, pPetData.PetInfo.res_name, PlayerData.Instance.IsShowPet);
                        }
                        else
                        {
                            _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip);
                        }
                    }
                }
                else
                {
                    _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Self.mEquip);
                }
            }
        }
        else if (roleCamp == EFightCamp.efcEnemy)
        {
            if (UISystem.Instance.FightView.fightPlayer_Enemy == null)
                return;
            if (UISystem.Instance.FightView.fightPlayer_Enemy.mSkill == null)
                return;
            UISystem.Instance.FightView.fightPlayer_Enemy.mSkill.ActivateHalo(this);
            if (_MainSpine != null)
            {
                if ((UISystem.Instance.FightView.fightPlayer_Enemy != null) && (UISystem.Instance.FightView.fightPlayer_Enemy.mIsShowPet != 0) && (UISystem.Instance.FightView.fightPlayer_Enemy.mPetInfo != null))
                {
                    CombatPetInfo tmpPet = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(UISystem.Instance.FightView.fightPlayer_Enemy.mPetInfo.id);
                    if (tmpPet != null)
                    {
                        _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Enemy.mEquip, tmpPet.res_name, true);
                    }
                    else
                    {
                        _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Enemy.mEquip);
                    }
                }
                else
                {
                    _MainSpine.RepleaceEquipment(UISystem.Instance.FightView.fightPlayer_Enemy.mEquip);
                }
            }
        }
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightStartAIHero, CommandEvent_StartAIHero);
        base.UnInitialization(vIsDelete);
    }

    public override void InitAction()
    {
        AddSingleAction<ActionStatic>();
        AddSingleAction<ActionHeroAIFire>();
        AddSingleAction<ActionDeath>();
        AddSingleAction<ActionVictory>();
        AddSingleAction<ActionFailure>();

        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            ExChangeAction<ActionStatic>();
    }

    public override float GetRoleActualSpeed()
    {
        float tmpResult = base.GetRoleActualSpeed();
        return tmpResult;
    }

    //--------------------------------------------命令操作函数----------------------------------------------//
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

    /// <summary>
    /// 开启英雄AI 结束延迟
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_StartAIHero(object vDataObj)
    {
        if (!this.IsLive())
            return;
        if (vDataObj == null)
            return;
        EFightCamp tmpFightCamp = (EFightCamp)vDataObj;
        if (tmpFightCamp != roleCamp)
            return;
        ExChangeAction<ActionHeroAIFire>();
    }

}
