using UnityEngine;
using System.Collections;
using Assets.Script.Common;

/// <summary>
/// 角色行为-英雄控制移动
/// </summary>
public class ActionHeroMove : ActionBase
{
    /// <summary>
    /// 场景边框宽度-己方
    /// </summary>
    private const float LIMIT_SCENE_WIDTH_SELF = 50;
    /// <summary>
    /// 场景边框宽度-敌方
    /// </summary>
    private const float LIMIT_SCENE_WIDTH_ENEMY = 160;

    /// <summary>
    /// 真实移动速度
    /// </summary>
    private float _ActualMoveSpeed;
    /// <summary>
    /// 场景宽度
    /// </summary>
    private float _ScreenWidth;
    /// <summary>
    /// 场景中心点
    /// </summary>
    private float _ScreenCenter;
    /// <summary>
    /// 场景左端限制坐标
    /// </summary>
    private float _LimitLeft;
    /// <summary>
    /// 场景右端限制坐标
    /// </summary>
    private float _LimitRight;
    /// <summary>
    /// 当前移动方向
    /// </summary>
    private ERoleDirection _CurrentDirection;
    /// <summary>
    /// 当前场景
    /// </summary>
    private FightSceneBase pFightSceneBase;
    /// <summary>
    /// 战斗场景摄像机比例
    /// </summary>
    private float cameraOrthographicSize;
    /// <summary>
    /// 场景背景图宽
    /// </summary>
    private float sceneBackGroundWidth;

    private float monsterBarracksPosX;
    private float limitMoveEnemy;
    private float limitMoveSelf;

    /// <summary>
    /// AI上一次执行时间
    /// </summary>
    private float _PreTime;


    public override void SetInit(RoleBase vRoleBase, string vActionName)
    {
        pFightSceneBase = SceneManager.Instance.Get_CurScene;
        if (pFightSceneBase != null)
        {
            _ScreenWidth = pFightSceneBase.Get_CenterScreenWidth;
            //Debug.LogWarning("_ScreenWidth: " + _ScreenWidth);
            _LimitLeft = pFightSceneBase.limitLeft;
            _LimitRight = pFightSceneBase.limitRight;
            cameraOrthographicSize = pFightSceneBase.SceneWidthSize;
            sceneBackGroundWidth = pFightSceneBase.MaxGroundWidth;
        }
        else
        {
            _ScreenWidth = 1024;
            _LimitLeft = -516;
            _LimitRight = -1532;
            cameraOrthographicSize = 1;
            sceneBackGroundWidth = 2048;
        }
        _ScreenCenter = _ScreenWidth / 2;
        _CurrentDirection = ERoleDirection.erdNone;
        base.SetInit(vRoleBase, vActionName);
    }

    public override bool SetActive()
    {
        _PreTime = Time.time;
        if (!base.SetActive())
            return false;
        pRoleBase.SetSpineTimeScale(ESpineSpeedType.esstMove);
        RepleaceAnimation(GlobalConst.ANIMATION_NAME_MOVE, true);
        SetRefresh();
        _PreStatus = EActionStatus.easAI;
        _CurStatus = EActionStatus.easAI;
        _PreTime = Time.time;
        return true;
    }

    public override bool SetStop()
    {
        if (!base.SetStop())
            return false;
        if (pRoleBase == null)
            return false;
        //角色默认朝右//
        pRoleBase.Get_Direction = ERoleDirection.erdRight;
        return true;
    }

    public override void SetRefresh()
    {
        if (pRoleBase == null)
            return;
        if (_CurrentDirection == pRoleBase.Get_Direction)
            return;
        if (pRoleBase.Get_FightAttribute == null)
            return;
        if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
            _ActualMoveSpeed = pRoleBase.GetRoleActualSpeed();
        else
            _ActualMoveSpeed = -pRoleBase.GetRoleActualSpeed();
    }

    public override void SetDestroy()
    {
        Scheduler.Instance.RemoveUpdator(ActionAI);
        base.SetDestroy();
    }

    /// <summary>
    /// 继续
    /// </summary>
    public override void SetResume()
    {
        base.SetResume();
        _PreTime = Time.time;
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
        if (pFightSceneBase == null)
            return;
        if (pFightSceneBase.sceneCamera == null)
            return;
        if (pFightSceneBase.transOther == null)
            return;
        bool tempIsMove = false;
        float tmpCurTime = Time.time;
        //float tmpSpeed = _ActualMoveSpeed * (tmpCurTime - _PreTime) * SceneManager.Instance.Get_FightSpeed;
        float tmpSpeed = _ActualMoveSpeed * CommonFunction.GetReasonableIntervalTime(tmpCurTime, _PreTime) * SceneManager.Instance.Get_FightSpeed;
        _PreTime = tmpCurTime;

        //获取角色当前屏幕坐标//
        float tmpCurScreenPosX = pFightSceneBase.sceneCamera.WorldToScreenPoint(pRoleBase.transform.position).x;
        //场景物件坐标移动//
        if (pFightSceneBase.transOther.transform.localPosition.x > _LimitLeft)
        {
            pFightSceneBase.transOther.transform.localPosition = new Vector3(_LimitLeft, 0, 0);
        }
        else if (pFightSceneBase.transOther.transform.localPosition.x < _LimitRight)
        {
            pFightSceneBase.transOther.transform.localPosition = new Vector3(_LimitRight, 0, 0);
        }
        else
        {
            if (Mathf.Abs(_ScreenCenter - tmpCurScreenPosX) <= GlobalConst.SCENE_CENTER_LIMIT_WIDTH)
            {
                float tmpValue = pFightSceneBase.transOther.transform.localPosition.x - tmpSpeed;
                if (tmpValue >= _LimitLeft)
                {
                    pFightSceneBase.transOther.transform.localPosition = new Vector3(_LimitLeft, 0, 0);
                }
                else if (tmpValue <= _LimitRight)
                {
                    pFightSceneBase.transOther.transform.localPosition = new Vector3(_LimitRight, 0, 0);
                }
                else
                {
                    pFightSceneBase.transOther.transform.localPosition -= new Vector3(tmpSpeed, 0, 0);
                    return;
                }
            }
        }

        //角色能否移动 
        //if (pRoleBase.Get_Direction == ERoleDirection.erdLeft)
        //{
        //    if (tmpCurScreenPosX + tmpSpeed > LIMIT_SCENE_WIDTH_SELF * cameraOrthographicSize)
        //        tempIsMove = true;
        //    else
        //        tempIsMove = false;
        //}
        //else if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
        //{
        //    if (tmpCurScreenPosX + tmpSpeed < (_ScreenWidth - LIMIT_SCENE_WIDTH_ENEMY) * cameraOrthographicSize)
        //        tempIsMove = true;
        //    else
        //        tempIsMove = false;
        //}
        ////角色移动//
        //if (tempIsMove)
        //{
        //    //判断对方城堡是否存在//
        //    if (SceneManager.Instance.Get_EnemyBarracksPosX != 0)
        //    {
        //        //判断是否到达对方城堡下方//
        //        if (pRoleBase.transform.localPosition.x + tmpSpeed >= (SceneManager.Instance.Get_EnemyBarracksPosX + _LimitRight) * cameraOrthographicSize)
        //            return;
        //    }
        //    pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
        //}


        if (pRoleBase.Get_Direction == ERoleDirection.erdLeft)
        {
            if (tmpCurScreenPosX + tmpSpeed > LIMIT_SCENE_WIDTH_SELF * cameraOrthographicSize)
                tempIsMove = true;
            else
                tempIsMove = false;
        }
        else if (pRoleBase.Get_Direction == ERoleDirection.erdRight)
        {
            if (tmpCurScreenPosX + tmpSpeed < _ScreenWidth - LIMIT_SCENE_WIDTH_ENEMY * cameraOrthographicSize)
                tempIsMove = true;
            else
                tempIsMove = false;
        }
        //角色移动//
        if (tempIsMove)
        {
            //判断对方城堡是否存在//
            if (SceneManager.Instance.Get_EnemyBarracksPosX != 0)
            {
                //判断是否到达对方城堡下方//
                if (tmpCurScreenPosX + tmpSpeed >= _ScreenWidth - (sceneBackGroundWidth - SceneManager.Instance.Get_EnemyBarracksPosX) * cameraOrthographicSize)
                    return;
            }
            pRoleBase.transform.localPosition += new Vector3(tmpSpeed, 0, 0);
        }
    }
}
