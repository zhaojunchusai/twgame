using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 场景移动镜头管理
/// </summary>
public class SceneMoveOperate : Singleton<SceneMoveOperate>
{

    /// <summary>
    /// 目标位置
    /// </summary>
    private int targetPosX;


    /// <summary>
    /// 初始化数据
    /// </summary>
    public void Initialize()
    {
        targetPosX = 0;
    }

    /// <summary>
    /// 销毁数据
    /// </summary>
    public void Uninitialize()
    { }


    /// <summary>
    /// 设定移动
    /// </summary>
    /// <param name="vScene"></param>
    /// <param name="vType"></param>
    /// <param name="vTargetPosX"></param>
    public void ReSetPosition(FightSceneBase vScene, TweenPosition vTP, ESceneCameraMoveType vType, int vTargetPosX, int vMoveSpeed, EventDelegate.Callback vCallFunction = null)
    {
        if ((vScene == null) || (vTP == null))
        {
            if (vCallFunction != null)
                vCallFunction();
            return;
        }

        //设置目标位置到合法范围//
        int tmpLimitMin = (int)(vScene.limitRight - vScene.transOther.localPosition.x);
        //int tmpLimitMax = 0;
        int tmpLimitMax = (int)(vScene.limitLeft - vScene.transOther.localPosition.x);
        targetPosX = vTargetPosX;
        if (targetPosX < tmpLimitMin)
            targetPosX = tmpLimitMin;
        if (targetPosX > tmpLimitMax)
            targetPosX = tmpLimitMax;

        //开启移动//
        if (vTP.gameObject.GetComponent<ShakeGameObj>() != null)
        {
            vTP.gameObject.GetComponent<ShakeGameObj>().Stop();
        }
        if (vType == ESceneCameraMoveType.escmtMove)
        {
            vTP.Restart();
            vTP.from = vTP.transform.localPosition;
            vTP.to = new Vector3(targetPosX, vTP.transform.localPosition.y, vTP.transform.localPosition.z);
            if (vMoveSpeed == 0)
                vTP.duration = 1;
            else
            {
                float tmpDistance = Mathf.Abs(vTP.from.x - vTP.to.x);
                vTP.duration = tmpDistance / vMoveSpeed;
            }
            if (vCallFunction != null)
                EventDelegate.Add(vTP.onFinished, vCallFunction);
            vTP.PlayForward();
        }
        else
        {
            vTP.transform.localPosition = new Vector3(targetPosX, vTP.transform.localPosition.y, vTP.transform.localPosition.z);
            if (vCallFunction != null)
                vCallFunction();
        }
    }

}