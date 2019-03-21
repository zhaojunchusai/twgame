using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Assets.Script.Common;

/// <summary>
/// 死亡行为
/// </summary>
public class ActionDeath : ActionBase
{
    private const float DELETE_TIME = 1.5f;

    /// <summary>
    /// 是否已经销毁
    /// </summary>
    private bool isDelete = false;
    private float startDeleteTime = 0;
    private float lastDeleteTime = 0;
    private float finalDeleteTime = -1;

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        if (pRoleBase.gameObject == null)
            return false;
        if (pRoleBase.Get_MainSpine != null)
        {
            if (pRoleBase.changeStatus != EChangeStatus.ecsNone)
            {
                pRoleBase.CloseChangeSkeletonDataAsset();
            }
            pRoleBase.Get_MainSpine.Resume();
        }
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstSkill);
        if (pRoleBase.SpecilDepotManage != null)
            pRoleBase.SpecilDepotManage.Destory();
        //关闭碰撞//
        if (pRoleBase.GetComponent<BoxCollider>() != null)
            pRoleBase.GetComponent<BoxCollider>().enabled = false;
        //播放动画//
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_DEATH, false);
        if (pRoleBase.pRpleHP != null)
            pRoleBase.pRpleHP.SetHpSpriteUnvisible();

        if (pRoleBase.Get_RoleType == ERoleType.ertHero)
        {
            if (pRoleBase.Get_RoleGender == EHeroGender.ehgMale)
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Dead_Male, pRoleBase.gameObject.transform));
            else if (pRoleBase.Get_RoleGender == EHeroGender.ehgFamale)
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Dead_Female, pRoleBase.gameObject.transform));
        }
        else if (pRoleBase.Get_RoleType == ERoleType.ertMonster)
        {
            if (pRoleBase.Get_FightAttribute != null)
            {
                MonsterAttributeInfo tmpInfo = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(pRoleBase.Get_FightAttribute.KeyData);
                if (tmpInfo != null)
                {
                    if (tmpInfo.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_Dead_Boss, pRoleBase.gameObject.transform));
                }
            }

            //判断是否新手引导//
            if (SceneManager.Instance.Get_CurSceneType == EFightType.eftNewGuide)
            {
                //通知新手引导打死小兵//
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_NewGuideReCreateEnemy);
            }
        }
        DeleteOperate_Role();
        return true;
    }

    public override void SetDestroy()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
            pRoleBase.Get_MainSpine.EndEvent -= Action_EndEvent;
        Scheduler.Instance.RemoveUpdator(CheckRoleIsDestroy);
        base.SetDestroy();
    }

    protected override void ActionSingleTonSet()
    {
        if ((pRoleBase != null) && (pRoleBase.Get_MainSpine != null))
            pRoleBase.Get_MainSpine.EndEvent += Action_EndEvent;
        Scheduler.Instance.AddUpdator(CheckRoleIsDestroy);
    }




    public override void SetPause()
    {
        base.SetPause();
        if (pRoleBase == null)
            return;
        if (pRoleBase.IsLive())
            return;
        lastDeleteTime = DELETE_TIME - (Time.time - startDeleteTime) * SceneManager.Instance.Get_FightSpeed;
        Scheduler.Instance.RemoveTimer(DeleteOperate_Delay);
    }

    public override void SetResume()
    {
        base.SetResume();
        if (pRoleBase == null)
            return;
        if (pRoleBase.IsLive())
            return;
        startDeleteTime = Time.time;
        Scheduler.Instance.AddTimer(lastDeleteTime / SceneManager.Instance.Get_FightSpeed, false, DeleteOperate_Delay);
    }

    protected override void Action_EndEvent(string vAnimationName)
    {
        if (pRoleBase == null)
            return;
        if (pRoleBase.Get_MainSpine == null)
            return;
        if (string.IsNullOrEmpty(Get_CurAnimationName))
            return;
        if (!vAnimationName.Equals(Get_CurAnimationName))
            return;
        //渐变Alpha//
        pRoleBase.Get_MainSpine.FadeOut(1);
        //延迟销毁//
        startDeleteTime = Time.time;
        Scheduler.Instance.AddTimer(1.2f, false, DeleteOperate_Delay);
        finalDeleteTime = Time.time;
    }

    /// <summary>
    /// 延迟销毁操作
    /// </summary>
    private void DeleteOperate_Delay()
    {
        if (isDelete)
            return;
        isDelete = true;
        if (pRoleBase == null)
            return;
        pRoleBase.gameObject.SetActive(false);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleDelete, pRoleBase.Get_SingleID);
    }
    private void DeleteOperate_Role()
    {
        if (pRoleBase == null)
            return;
        if (!pRoleBase.Get_IsSummon)
        {
            if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
            {
                if (pRoleBase.Get_RoleType == ERoleType.ertMonster)
                {
                    EPVEFinishStatus tmpStatus = EPVEFinishStatus.epvefsDieMonster;
                    if (pRoleBase.Get_FightAttribute != null)
                    {
                        MonsterAttributeInfo tmpInfo = ConfigManager.Instance.mMonsterData.GetMonsterAttributeByID(pRoleBase.Get_FightAttribute.KeyData);
                        if (tmpInfo != null)
                        {
                            if (tmpInfo.IsBoss == GlobalConst.MONSTER_TYPE_BOSS)
                                tmpStatus = EPVEFinishStatus.epvefsDisBoss;
                        }
                    }
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(tmpStatus, pRoleBase.Get_FightAttribute.KeyData));
                }
                else if (pRoleBase.Get_RoleType == ERoleType.ertHero)
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDisEnemyHero));
                }
                else
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDisEnemySoldier, pRoleBase.mUID));
                }
            }
            else
            {
                if (pRoleBase.Get_RoleType == ERoleType.ertHero)
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieHero));
                }
                else if (pRoleBase.Get_RoleType == ERoleType.ertSoldier)
                {
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieSoldier, pRoleBase.mUID));
                }
            }
        }
        pRoleBase.SpecilDepotManage.Destory();
    }


    private void CheckRoleIsDestroy()
    {
        if (finalDeleteTime > 0)
        {
            if (Time.time - finalDeleteTime >= 1.2f)
            {
                isDelete = false;
                DeleteOperate_Delay();
            }
        }
    }
}
