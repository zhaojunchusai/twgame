using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class RoleEscort : RoleBase
{
    /// <summary>
    /// 战斗场景状态
    /// </summary>
    private ESceneStatus sceneStatus;

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
            EffectImmuneManage.AddFilter(SkillFilter_Escort);
    }

    protected override void FinalOperate()
    {
        ExChangeAction<ActionDeath>();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieEscort));
    }

    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetStar, 100);

        Scheduler.Instance.AddTimer(1, false, AddShowEffect);
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        base.UnInitialization(vIsDelete);
    }

    public override void InitAction()
    {
        sceneStatus = ESceneStatus.essNormal;

        AddSingleAction<ActionStatic>();
        AddSingleAction<ActionEscortMove>();
        AddSingleAction<ActionDeath>();
        AddSingleAction<ActionVictory>();
        AddSingleAction<ActionFailure>();

        if (!UISystem.Instance.FightView.Get_FightStartIsOver)
            ExChangeAction<ActionStatic>();
        else
            ExChangeAction<ActionEscortMove>();
    }

    public override float GetRoleActualSpeed()
    {
        return base.GetRoleActualSpeed();
    }

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

        if (sceneStatus == ESceneStatus.essVictory)
            ExChangeAction<ActionVictory>();
        else
            ExChangeAction<ActionFailure>();
    }

    private void CommandEvent_CloseShowBossTalk(object vDataObj)
    {
        ExChangeAction<ActionEscortMove>();
    }


    /// <summary>
    /// 添加特效
    /// </summary>
    private void AddShowEffect()
    {
        float tmpScaleValue = SkillTool.GetLocalScare(this);
        EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.ESCORT_EFFECT_NAME, (GameObject gb) =>
        {
            if (this != null)
            {
                GameObject go = CommonFunction.SetParent(gb, this.transform);
                if (go != null)
                {
                    go.transform.localScale = new Vector3(1 / this.transform.localScale.x * tmpScaleValue, 1 / this.transform.localScale.y * tmpScaleValue, 1);
                    //go.transform.localPosition = SkillTool.GetBonePosition(this, 2);
                    go.transform.localPosition = Vector3.zero;

                    TdSpine.SpineBase tmpSpineBase = go.GetComponent<TdSpine.SpineBase>();
                    if (tmpSpineBase == null)
                        tmpSpineBase = go.AddComponent<TdSpine.SpineBase>();
                    if (tmpSpineBase != null)
                    {
                        tmpSpineBase.InitSkeletonAnimation();
                        if (_MainSpine != null)
                            tmpSpineBase.setSortingOrder(_MainSpine.GetMaxSort());
                        tmpSpineBase.pushAnimation("animation", true, 1);
                    }
                }
            }
        });
        //SpecilEffect SE = new SpecilEffect();
        //SkillSpecilInfo tempInfo = new SkillSpecilInfo();
        //tempInfo.effectMusic = "0";
        //tempInfo.during = 600000;
        //tempInfo.effectName = GlobalConst.Effect.ESCORT_EFFECT_NAME;
        //tempInfo.effectType = 4;
        //tempInfo.followSpecil = 1;
        //tempInfo.sortLayer = 1;
        //tempInfo.timeDelay = 500;
        //SE.initialize(tempInfo, this);
        //List<RoleAttribute> list = new List<RoleAttribute>();
        //list.Add(this);
        //SE.SetTarget(list, 1.0f);
        //SE.DoSpecilEffect();

    }

}
