using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 角色-怪物
/// </summary>
public class RoleMonster : RoleBase
{
    /// <summary>
    /// 战斗场景状态
    /// </summary>
    private ESceneStatus sceneStatus;
    private int isBoss; 

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

    public override void Initialization(int vSceneRoleID, object vRoleInfo, ERoleType vRoleType, int vRolePathIndex,
        float vSpeed, ulong vUID, EFightCamp vFightCamp, EFightType vFightType, EHeroGender vHeroGender)
    {
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_ShowBossStatus, CommandEvent_ShowBossStatus);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);

        base.Initialization(vSceneRoleID, vRoleInfo, vRoleType, vRolePathIndex, vSpeed, vUID, vFightCamp, vFightType, vHeroGender);

        if (fightAttribute == null)
            return;
        MonsterAttributeInfo tmpMonster = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(fightAttribute.KeyData);
        if (tmpMonster == null)
            return;
        isBoss = tmpMonster.IsBoss;

        List<uint> tmpSkillIDList = new List<uint>();
        tmpSkillIDList.AddRange(tmpMonster.Skill);
        tmpSkillIDList.AddRange(tmpMonster.PassivitySkill);
        skillsDepot = new SkillsDepot();
        skillsDepot.multipleAdd(tmpSkillIDList);
        foreach (Skill sk in skillsDepot._skillsList)
        {
            sk.Level = tmpMonster.Level;
        }
        skillsDepot.ActivateHalo(this);

        if (isBoss == GlobalConst.MONSTER_TYPE_BOSS)
        {
            if (EffectImmuneManage != null)
                EffectImmuneManage.AddFilter(SkillFilter_Boss);
            if ((vFightType != EFightType.eftMain) && (vFightType != EFightType.eftActivity) && (vFightType != EFightType.eftEndless))
                return;
            if (RoleManager.Instance.Get_Hero != null)
            {
                if (!RoleManager.Instance.Get_Hero.IsLive())
                    return;
            }
            ShowBossStatus();
        }
        else if (isBoss == GlobalConst.MONSTER_TYPE_SUPER)
        {
            if (EffectImmuneManage != null)
                EffectImmuneManage.AddFilter(SkillFilter_SuperMonster);
        }
    }

    public override void UnInitialization(bool vIsDelete = true)
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightFinished, CommandEvent_FightFinished);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_CloseShowBossTalk, CommandEvent_CloseShowBossTalk);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_ShowBossStatus, CommandEvent_ShowBossStatus);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_BossInScene, CommandEvent_BossInScene);
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
        AddSingleAction<ActionDialogue>();
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
                    if ((Get_Enemy != null) && (Get_Enemy.Get_RoleType != ERoleType.ertTransfer))
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
    /// 关闭BOSS对话显示
    /// </summary>
    /// <param name="vDataObj"></param>
    private void CommandEvent_CloseShowBossTalk(object vDataObj)
    {
        if (!_CurrentActionName.Equals("ActionDeformation"))
        {
            ExChangeAction<ActionAIMove>();
        }
    }

    private void CommandEvent_ShowBossStatus(object vDataObj)
    {
        if (isBoss == GlobalConst.MONSTER_TYPE_COMMON)
            return;
        SetBossStatus();
    }

    private void CommandEvent_BossInScene(object vDataObj)
    {
        if (isBoss == GlobalConst.MONSTER_TYPE_COMMON)
            return;
        if (this.changeStatus == EChangeStatus.ecsNone)
            ExChangeAction<ActionDialogue>();
    }


    private bool ShowFightFinishedStatus(ESceneStatus vSceneStatus)
    {
        if (vSceneStatus == ESceneStatus.essVictory)
            ExChangeAction<ActionFailure>();
        else if (vSceneStatus == ESceneStatus.essFailure)
            ExChangeAction<ActionVictory>();
        else
            return false;
        return true;
    }

    /// <summary>
    /// 显示Boss出场状态
    /// </summary>
    private void ShowBossStatus()
    {
        //击退指定范围内的敌对阵营士兵//
        Vector3 tmpStartPos = this.transform.position;
        Vector3 tmpEndPos = new Vector3(this.transform.position.x - 600 / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X,
                                        this.transform.position.y, this.transform.position.z);
        Vector3 tmpBackEndPos = new Vector3(this.transform.position.x + 600 / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X,
                                        this.transform.position.y, this.transform.position.z);

        //List<RoleAttribute> tmpList = CommonFunction.FindHitFightObjects(tmpStartPos, tmpEndPos, ERoleDirection.erdLeft, EFightCamp.efcSelf);
        List<RoleAttribute> tmpList = CommonFunction.FindHitFightObjects(tmpBackEndPos, tmpEndPos, ERoleDirection.erdLeft, EFightCamp.efcSelf);
        if ((tmpList != null) && (tmpList.Count > 0))
        {
            for (int i = 0; i < tmpList.Count; i++)
            {
                if (tmpList[i] == null)
                    continue;
                if (!tmpList[i].IsLive())
                    continue;
                if ((tmpList[i].Get_RoleType != ERoleType.ertSoldier) && (tmpList[i].Get_RoleType != ERoleType.ertEscort))
                    continue;
                tmpList[i].Repel(500);
            }
        }

        //Boss出场对话//
        TalkManager.Instance.OpenFightTalk(EChatType.ectBossTalk);
    }

    private void SetBossStatus()
    {
        //播放特效//
        float tmpScaleValue = SkillTool.GetLocalScare(this);
        EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.BOSS_APPEAR_A, (GameObject gb) =>
        {
            if (this != null)
            {
                GameObject go = CommonFunction.SetParent(gb, this.transform);
                if (go != null)
                {
                    go.transform.localScale = new Vector3(1 / this.transform.localScale.x * tmpScaleValue, 1 / this.transform.localScale.y * tmpScaleValue, 1);
                    go.transform.localPosition = SkillTool.GetBonePosition(this, 2);

                    TdSpine.SpineBase tmpSpineBase = go.GetComponent<TdSpine.SpineBase>();
                    if(tmpSpineBase == null)
                        tmpSpineBase = go.AddComponent<TdSpine.SpineBase>();
                    if (tmpSpineBase != null)
                    {
                        tmpSpineBase.InitSkeletonAnimation();
                        if (_MainSpine != null)
                            tmpSpineBase.setSortingOrder(_MainSpine.GetMaxSort() + 1);
                        tmpSpineBase.pushAnimation("animation", false, 1);
                        tmpSpineBase.EndEvent += (string s) =>
                        {
                            if (go != null) EffectObjectCache.Instance.FreeObject(go);
                        };
                    }
                }
            }
        });

        //EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.BOSS_EFFECT_NAME, (GameObject gb) =>
        //{
        //    if (this != null)
        //    {
        //        GameObject go = CommonFunction.SetParent(gb, this.transform);
        //        if (go != null)
        //        {
        //            go.transform.localScale = new Vector3(1 / this.transform.localScale.x * tmpScaleValue, 1 / this.transform.localScale.y * tmpScaleValue, 1);
        //            //go.transform.localPosition = SkillTool.GetBonePosition(this, 2);
        //            go.transform.localPosition = Vector3.zero;

        //            TdSpine.SpineBase tmpSpineBase = go.GetComponent<TdSpine.SpineBase>();
        //            if (tmpSpineBase == null)
        //                tmpSpineBase = go.AddComponent<TdSpine.SpineBase>();
        //            if (tmpSpineBase != null)
        //            {
        //                tmpSpineBase.InitSkeletonAnimation();
        //                tmpSpineBase.setSortingOrder(_MainSpine.GetMaxSort() + 1);
        //                tmpSpineBase.pushAnimation("animation", true, 1);
        //                //tmpSpineBase.EndEvent += (string s) =>
        //                //{
        //                //    if (go != null) EffectObjectCache.Instance.FreeObject(go);
        //                //};
        //            }
        //        }
        //    }
        //});
        //SpecilEffect SE = new SpecilEffect();
        //SkillSpecilInfo tempInfo = new SkillSpecilInfo();
        //tempInfo.effectMusic = "0";
        //tempInfo.during = 600000;
        //tempInfo.effectName = GlobalConst.Effect.BOSS_EFFECT_NAME;
        //tempInfo.effectType = 4;
        //tempInfo.followSpecil = 1;
        //tempInfo.sortLayer = 1;
        //tempInfo.timeDelay = 0;
        //SE.initialize(tempInfo, this);
        //List<RoleAttribute> list = new List<RoleAttribute>();
        //list.Add(this);
        //SE.SetTarget(list, 1.0f);
        //SE.DoSpecilEffect();

    }

}
