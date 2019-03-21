using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 角色行为-AI移动
/// </summary>
public class ActionAIMove : ActionBase
{

    private const float LIMIT_MOVE_MIN_SELF = 0;
    private const float LIMIT_MOVE_MAX_SELF_DOUBLE = 1920;  //双屏限制//
    private const float LIMIT_MOVE_MAX_SELF_SINGLE = 890;   //单屏限制//
    private const float LIMIT_MOVE_MIN_ENEMY = 2048;        //
    private const float LIMIT_MOVE_MAX_ENEMY = 130;

    /// <summary>
    /// AI上一次执行时间
    /// </summary>
    private float _PreAITime = -1;
    private float transferPosX;
    private float limitMoveMin = 0;
    private float limitMoveMax = 0;
    private float scenePosXSelf = 0;
    private float scenePosXEnemy = 0;
    private int isCheckEnemy = 0;


    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;
        _PreAITime = Time.time;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        if (SceneManager.Instance.Get_CurScene != null)
        {
            if (SceneManager.Instance.Get_CurScene.transSelf != null)
                scenePosXSelf = SceneManager.Instance.Get_CurScene.transSelf.localPosition.x;
            if (SceneManager.Instance.Get_CurScene.transEnemy != null)
                scenePosXEnemy = SceneManager.Instance.Get_CurScene.transEnemy.localPosition.x;
        }

        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            limitMoveMin = LIMIT_MOVE_MIN_SELF;
            if (pRoleBase.Get_FightType == EFightType.eftUnion)
                limitMoveMax = LIMIT_MOVE_MAX_SELF_SINGLE;
            else
                limitMoveMax = LIMIT_MOVE_MAX_SELF_DOUBLE;
        }
        else
        {
            limitMoveMin = LIMIT_MOVE_MIN_ENEMY;
            limitMoveMax = LIMIT_MOVE_MAX_ENEMY;
        }
        isCheckEnemy = 0;
        return true;
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveUpdator(ActionAI);
        base.SetDestroy();
    }

    public override void SetResume()
    {
        base.SetResume();
        _PreAITime = Time.time;
    }

    protected override void ActionSingleTonSet()
    {
        Scheduler.Instance.AddUpdator(ActionAI);
        transferPosX = SceneManager.Instance.Get_TransferPosX;
    }


    /// <summary>
    /// 行为AI
    /// </summary>
    //private void ActionAI()
    //{
    //    if (_CurStatus != EActionStatus.easAI)
    //        return;
    //    if (pRoleBase == null)
    //        return;
    //    if (pRoleBase.transform == null)
    //        return;
    //    if (pRoleBase.gameObject == null)
    //        return;

    //    float tmpCurTime = Time.time;
    //    float tmpSpeed = pRoleBase.GetRoleActualSpeed() * (tmpCurTime - _PreAITime) * SceneManager.Instance.Get_FightSpeed;
    //    _PreAITime = tmpCurTime;

    //    //继续移动//
    //    if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
    //    {
    //        if (!pRoleBase.Get_IsSubverted)
    //        {
    //            if (pRoleBase.transform.localPosition.x + scenePosXSelf + tmpSpeed < limitMoveMax)
    //            {
    //                pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
    //            }
    //            else
    //            {
    //                pRoleBase.ReSetRoleAction();
    //                SetStop();
    //            }
    //            if (pRoleBase.transform.localPosition.x + scenePosXSelf < limitMoveMin)
    //                return;
    //        }
    //        else
    //        {
    //            if (pRoleBase.transform.localPosition.x + scenePosXSelf - tmpSpeed > limitMoveMin)
    //            {
    //                pRoleBase.transform.localPosition += new Vector3(-tmpSpeed, 0, 0);
    //            }
    //            else
    //            {
    //                pRoleBase.ReSetRoleAction();
    //                SetStop();
    //            }
    //            if (pRoleBase.transform.localPosition.x + scenePosXSelf > limitMoveMax)
    //                return;
    //        }
    //    }
    //    else if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
    //    {
    //        if (!pRoleBase.Get_IsSubverted)
    //        {
    //            if (pRoleBase.transform.localPosition.x + scenePosXEnemy - tmpSpeed > limitMoveMax)
    //            {
    //                pRoleBase.transform.localPosition += new Vector3(-tmpSpeed, 0, 0);
    //            }
    //            else
    //            {
    //                pRoleBase.ReSetRoleAction();
    //                SetStop();
    //            }
    //            if (pRoleBase.transform.localPosition.x + scenePosXEnemy > limitMoveMin)
    //                return;

    //            //将到达传送门的角色删除//
    //            if (pRoleBase.transform.parent.localPosition.x + pRoleBase.transform.localPosition.x <= transferPosX)
    //            {
    //                pRoleBase.gameObject.SetActive(false);
    //                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieTransfer));
    //                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleDelete, pRoleBase.Get_SingleID);
    //                SetStop();
    //                return;
    //            }
    //        }
    //        else
    //        {
    //            if (pRoleBase.transform.localPosition.x + scenePosXEnemy + tmpSpeed < limitMoveMin)
    //            {
    //                pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
    //            }
    //            else
    //            {
    //                pRoleBase.ReSetRoleAction();
    //                SetStop();
    //            }
    //            if (pRoleBase.transform.localPosition.x + scenePosXEnemy < limitMoveMax)
    //                return;
    //        }
    //    }

    //    //查找敌人//
    //    if (pRoleBase.CheckIsOutSide())
    //        return;
    //    if (!pRoleBase.Get_isOpenCheckEnemy)
    //        return;
    //    ++isCheckEnemy;
    //    if (isCheckEnemy % 2 != 0)
    //    {
    //        pRoleBase.CheckEnemyIsChange();
    //    }
    //    if (pRoleBase.Get_Enemy == null)
    //        return;
    //    //判断是否是传送门 如果不是就切换到攻击状态//
    //    if (pRoleBase.Get_Enemy.name.Equals("TransferObj"))
    //        return;
    //    pRoleBase.ReSetRoleAction();
    //    SetStop();
    //}
    private void ActionAI()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;
        if (pRoleBase.transform == null)
            return;
        if (pRoleBase.gameObject == null)
            return;

        float tmpCurTime = Time.time;
        //float tmpSpeed = pRoleBase.GetRoleActualSpeed() * (tmpCurTime - _PreAITime) * SceneManager.Instance.Get_FightSpeed;
        float tmpSpeed = pRoleBase.GetRoleActualSpeed() * CommonFunction.GetReasonableIntervalTime(tmpCurTime, _PreAITime) * SceneManager.Instance.Get_FightSpeed;
        _PreAITime = tmpCurTime;

        //继续移动//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            if (!pRoleBase.Get_IsSubverted)
            {
                if (pRoleBase.transform.localPosition.x + scenePosXSelf + tmpSpeed < limitMoveMax)
                {
                    pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
                }
                else
                {
                    pRoleBase.ReSetRoleAction();
                    SetStop();
                }
                if (pRoleBase.transform.localPosition.x + scenePosXSelf < limitMoveMin)
                    return;
            }
            else
            {
                if (pRoleBase.transform.localPosition.x + scenePosXSelf - tmpSpeed > limitMoveMin)
                {
                    pRoleBase.transform.localPosition += new Vector3(-tmpSpeed, 0, 0);
                }
                else
                {
                    pRoleBase.ReSetRoleAction();
                    SetStop();
                }
                if (pRoleBase.transform.localPosition.x + scenePosXSelf > limitMoveMax)
                    return;
            }
        }
        else if (pRoleBase.Get_RoleCamp == EFightCamp.efcEnemy)
        {
            if (!pRoleBase.Get_IsSubverted)
            {
                if (pRoleBase.transform.localPosition.x + scenePosXEnemy - tmpSpeed > limitMoveMax)
                {
                    pRoleBase.transform.localPosition += new Vector3(-tmpSpeed, 0, 0);
                }
                else
                {
                    pRoleBase.ReSetRoleAction();
                    SetStop();
                }
                if (pRoleBase.transform.localPosition.x + scenePosXEnemy > limitMoveMin)
                    return;

                //将到达传送门的角色删除//
                if (pRoleBase.transform.parent.localPosition.x + pRoleBase.transform.localPosition.x <= transferPosX)
                {
                    pRoleBase.gameObject.SetActive(false);
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsDieTransfer));
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleDelete, pRoleBase.Get_SingleID);
                    SetStop();
                    return;
                }
            }
            else
            {
                if (pRoleBase.transform.localPosition.x + scenePosXEnemy + tmpSpeed < limitMoveMin)
                {
                    pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
                }
                else
                {
                    pRoleBase.ReSetRoleAction();
                    SetStop();
                }
                if (pRoleBase.transform.localPosition.x + scenePosXEnemy < limitMoveMax)
                    return;
            }
        }

        //查找敌人//
        if (pRoleBase.CheckIsOutSide())
            return;
        if (!pRoleBase.Get_isOpenCheckEnemy)
            return;
        pRoleBase.CheckEnemyIsChange();
        if (pRoleBase.Get_Enemy == null)
            return;
        //判断是否是传送门 如果不是就切换到攻击状态//
        if (pRoleBase.Get_Enemy.name.Equals("TransferObj"))
            return;
        pRoleBase.ReSetRoleAction();
        SetStop();
    }
}
