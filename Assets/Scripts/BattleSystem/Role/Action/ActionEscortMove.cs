using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 被护送目标移动行为
/// </summary>
public class ActionEscortMove : ActionBase
{
    /// <summary>
    /// 受击停止移动时间
    /// </summary>
    private const float ESCORT_HITSTOP_TIME = 2;

    /// <summary>
    /// AI上一次执行时间
    /// </summary>
    private float _PreAITime = -1;
    /// <summary>
    /// 受击停止记录时间
    /// </summary>
    private float recordTime = -1;
    /// <summary>
    /// 剩余暂停时间
    /// </summary>
    private float lastPauseTime = -1;
    /// <summary>
    /// 终点X坐标
    /// </summary>
    public int FinishPosX = 0;
    

    public override bool SetActive()
    {
        if (!base.SetActive())
            return false;

        uint tmpStageID = SceneManager.Instance.Get_CurScene.Get_CurrentSceneID;
        StageInfo tmpStageInfo = ConfigManager.Instance.mStageData.GetInfoByID(tmpStageID);
        if (tmpStageInfo != null)
            FinishPosX = tmpStageInfo.MarkPos;
        else
            FinishPosX = 0;

        recordTime = -1;
        lastPauseTime = -1;
        _PreAITime = Time.time;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        return true;
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveUpdator(ActionAI);
        base.SetDestroy();
    }

    public override void SetPause()
    {
        base.SetPause();
        if (recordTime != -1)
        {
            lastPauseTime = ESCORT_HITSTOP_TIME - (Time.time - recordTime);
        }
    }

    public override void SetResume()
    {
        base.SetResume();
        _PreAITime = Time.time;
        if (recordTime != -1)
        {
            recordTime = Time.time - lastPauseTime;
        }
    }

    protected override void ActionSingleTonSet()
    {
        Scheduler.Instance.AddUpdator(ActionAI);
    }

    /// <summary>
    /// 行为AI
    /// </summary>
    private void ActionAI()
    {
        if (_CurStatus != EActionStatus.easAI)
            return;
        if (pRoleBase == null)
            return;
        if (Time.time - recordTime < ESCORT_HITSTOP_TIME)
        {
            _PreAITime = Time.time;
            return;
        }
        recordTime = -1;

        float tmpCurTime = Time.time;
        //float tmpSpeed = pRoleBase.GetRoleActualSpeed() * (tmpCurTime - _PreAITime) * SceneManager.Instance.Get_FightSpeed;
        float tmpSpeed = pRoleBase.GetRoleActualSpeed() * CommonFunction.GetReasonableIntervalTime(tmpCurTime, _PreAITime) * SceneManager.Instance.Get_FightSpeed;
        _PreAITime = tmpCurTime;

        //继续移动//
        if (pRoleBase.Get_RoleCamp == EFightCamp.efcSelf)
        {
            if (pRoleBase.transform.localPosition.x + tmpSpeed < FinishPosX)
                pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
            else
            {
                SetStop();
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightObjDestroy, new FightDestroyInfo(EPVEFinishStatus.epvefsOutEscort, pRoleBase));
            }
        }
    }

    public override void SetHittedStatus(int vHpChangeValue)
    {
        if (vHpChangeValue >= 0)
            return;
        recordTime = Time.time;
    }

}
