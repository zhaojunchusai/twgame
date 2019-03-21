using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 场景移动镜头管理
/// </summary>
public class SceneCameraMoveManager : Singleton<SceneCameraMoveManager>
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
    public void ReSetPosition(FightSceneBase vScene, ESceneCameraMoveType vType, int vTargetPosX, EventDelegate.Callback vCallFunction = null)
    {
        if (vScene == null)
        {
            if (vCallFunction != null)
                vCallFunction();
            return;
        }
        TweenPosition tmpTP = vScene.sceneCamera.transform.FindChild("SceneContent").gameObject.GetComponent<TweenPosition>();
        if (tmpTP == null)
        {
            if (vCallFunction != null)
                vCallFunction();
            return;
        }

        //设置目标位置到合法范围//
        int tmpLimitMin = (int)(vScene.limitRight - vScene.transOther.localPosition.x);
        int tmpLimitMax = 0;
        targetPosX = vTargetPosX;
        if (targetPosX < tmpLimitMin)
            targetPosX = tmpLimitMin;
        if (targetPosX > tmpLimitMax)
            targetPosX = tmpLimitMax;

        //开启移动//
        if (vType == ESceneCameraMoveType.escmtMove)
        {
            tmpTP.Restart();
            tmpTP.from = tmpTP.transform.localPosition;
            tmpTP.to = new Vector3(targetPosX, tmpTP.transform.localPosition.y, tmpTP.transform.localPosition.z);
            if (vCallFunction != null)
                EventDelegate.Add(tmpTP.onFinished, vCallFunction);
            tmpTP.PlayForward();
        }
        else
        {
            tmpTP.transform.localPosition = new Vector3(targetPosX, tmpTP.transform.localPosition.y, tmpTP.transform.localPosition.z);
            if (vCallFunction != null)
                vCallFunction();
        }
    }

}