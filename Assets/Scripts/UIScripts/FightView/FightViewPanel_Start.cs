using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 战斗开始界面
/// </summary>
public class FightViewPanel_Start : Singleton<FightViewPanel_Start>
{

    private const float ANIMATION_DELAY_TIME = 1.1f;

    private FightView fightView;
    private FightViewController fightViewController;
    private float startAnimationTime;

    public void Initialize(FightView vFightView)
    {
        fightView = vFightView;
        fightViewController = UISystem.Instance.FightView;
        InitPanel();
    }

    public void Uninitialize()
    {
        InitPanel();
    }

    public void Destroy()
    {
        fightView = null;
        fightViewController = null;
    }


    
    /// <summary>
    /// 初始化界面
    /// </summary>
    public void InitPanel()
    {
        if (fightView == null)
            return;
        fightView.UIPanel_FightStart.gameObject.SetActive(false);
    }

    /// <summary>
    /// 初始显示
    /// </summary>
    private void InitShowStatuss()
    {
        if (fightView.Spt_Start_Content != null)
            fightView.Spt_Start_Content.gameObject.SetActive(false);
        if (fightView.Lbl_Start_Endless_Count != null)
        {
            fightView.Lbl_Start_Endless_Count.gameObject.SetActive(false);
            fightView.Lbl_Start_Endless_Count.text = string.Empty;
        }
        if (fightView.Lbl_Start_Endless_Hint != null)
        {
            fightView.Lbl_Start_Endless_Hint.gameObject.SetActive(false);
            fightView.Lbl_Start_Endless_Hint.text = string.Empty;
        }
        if (fightView.TP_Start_Info != null)
        {
            fightView.TP_Start_Info.gameObject.transform.localPosition = new Vector3(-1000, 0, 0);
            fightView.TP_Start_Info.enabled = false;
            fightView.TP_Start_Info.Restart();
        }
        if (fightView.TA_Start_SuccessIcon != null)
        {
            fightView.TA_Start_SuccessIcon.gameObject.SetActive(false);
            fightView.TA_Start_SuccessIcon.Restart();
        }
        if (fightView.TS_Start_SuccessIcon != null)
        {
            fightView.TS_Start_SuccessIcon.Restart();
        }
        if (fightView.TR_Start_SuccessIcon != null)
        {
            fightView.TR_Start_SuccessIcon.Restart();
        }
    }

    /// <summary>
    /// 刷新显示
    /// </summary>
    /// <param name="vType"></param>
    /// <param name="vExpeCount"></param>
    private EFightStartType fightStartType;
    private int fightCount;
    public void RefreshShowStatus(EFightStartType vType, int vFightCount = 0)
    {
        InitShowStatuss();
        if (fightView == null)
            return;
        if (fightView.Spt_Start_Content == null)
            return;

        fightStartType = vType;
        fightCount = vFightCount;
        startAnimationTime = -2;
        fightView.Spt_Start_Content.gameObject.SetActive(true);
        switch (vType)
        {
            case EFightStartType.efstAtt:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_1);
                }
                break;
            case EFightStartType.efstDef:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_2);
                }
                break;
            case EFightStartType.efstEscort:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_3);
                }
                break;
            case EFightStartType.efstTranfer:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_4);
                }
                break;
            case EFightStartType.efstEndless:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_6);
                    if (fightView.Lbl_Start_Endless_Count != null)
                    {
                        fightView.Lbl_Start_Endless_Count.text = vFightCount.ToString();
                        fightView.Lbl_Start_Endless_Count.gameObject.SetActive(true);
                    }
                    if (fightView.Lbl_Start_Endless_Hint != null)
                    {
                        fightView.Lbl_Start_Endless_Hint.text = string.Format(ConstString.ENDLESS_TOTALGATECOUNT, 15);
                        fightView.Lbl_Start_Endless_Hint.gameObject.SetActive(true);
                    }

                    if (vFightCount > 1)
                    {
                        startAnimationTime = Time.time;
                        //startAnimationTime = -2;
                        if (fightView.TA_Start_SuccessIcon != null)
                        {
                            fightView.TA_Start_SuccessIcon.duration = 0.75f;
                            fightView.TA_Start_SuccessIcon.gameObject.SetActive(true);
                            fightView.TA_Start_SuccessIcon.PlayForward();
                        }
                        if (fightView.TS_Start_SuccessIcon != null)
                        {
                            fightView.TS_Start_SuccessIcon.duration = 0.75f;
                            fightView.TS_Start_SuccessIcon.PlayForward();
                        }
                        if (fightView.TR_Start_SuccessIcon != null)
                        {
                            fightView.TR_Start_SuccessIcon.duration = 0.75f;
                            fightView.TR_Start_SuccessIcon.PlayForward();
                        }
                    }
                }
                break;
            case EFightStartType.efstExpedition:
                {
                    CommonFunction.SetSpriteName(fightView.Spt_Start_Content, GlobalConst.SpriteName.FightStart_6);
                    if (fightView.Lbl_Start_Endless_Count != null)
                    {
                        fightView.Lbl_Start_Endless_Count.text = vFightCount.ToString();
                        fightView.Lbl_Start_Endless_Count.gameObject.SetActive(true);
                    }
                }
                break;
            case EFightStartType.efstPVP:
                {
                    fightView.Spt_Start_Content.gameObject.SetActive(false);
                }
                break;
            case EFightStartType.efstNewGuide:
                {
                    fightView.Spt_Start_Content.gameObject.SetActive(false);
                }
                break;
            case EFightStartType.efstNone:
            default:
                {
                    return;
                }
        }

        fightView.Spt_Start_Content.MakePixelPerfect();
        fightView.UIPanel_FightStart.gameObject.SetActive(true);
        //if ((vType == EFightStartType.efstEndless) && (vFightCount > 1))
        //    return;


        //ShowUIAnimation();
        Scheduler.Instance.AddUpdator(ShowUIAnimation);
    }

    /// <summary>
    /// 显示界面动画
    /// </summary>
    //public void ShowUIAnimation()
    //{
    //    if (Time.time - startAnimationTime >= ANIMATION_DELAY_TIME)
    //    {
    //        Scheduler.Instance.AddTimer(2, false, CloseFightStartOperate);
    //        if (fightView == null)
    //            return;
    //        if (fightView.TP_Start_Info == null)
    //            return;
    //        fightView.TP_Start_Info.PlayForward();

    //        if (fightStartType != EFightStartType.efstEndless)
    //            return;
    //        //if (fightCount <= 1)
    //        //    return;
    //        //if (fightViewController == null)
    //        //    return;
    //        UISystem.Instance.FightView.EndlessExcessiveEnd();
    //    }
    //    else
    //    {
    //        float tmpTime = ANIMATION_DELAY_TIME + startAnimationTime - Time.time;
    //        if (tmpTime <= 0)
    //            ShowUIAnimation();
    //        else
    //            Scheduler.Instance.AddTimer(tmpTime, false, ShowUIAnimation);
    //    }
    //}
    public void ShowUIAnimation()
    {
        if (Time.time - startAnimationTime < ANIMATION_DELAY_TIME)
            return;
        Scheduler.Instance.RemoveUpdator(ShowUIAnimation);
        Scheduler.Instance.AddTimer(2, false, CloseFightStartOperate);
        if ((fightView != null) && (fightView.TP_Start_Info != null))
        {
            fightView.TP_Start_Info.PlayForward();
        }
        if (fightStartType == EFightStartType.efstEndless)
        {
            UISystem.Instance.FightView.EndlessExcessiveEnd();
        }
    }

    /// <summary>
    /// 关闭战斗开始操作
    /// </summary>
    private void CloseFightStartOperate()
    {
        if (fightViewController != null)
            fightViewController.StartFightStatus();
        InitPanel();
    }
}
