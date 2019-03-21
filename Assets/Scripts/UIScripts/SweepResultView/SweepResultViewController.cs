using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

/// <summary>
/// 界面状态
/// </summary>
public enum ESweepShowStatus
{
    /// <summary>
    /// 显示动画
    /// </summary>
    esssShowAnimation,
    /// <summary>
    /// 结束演示
    /// </summary>
    esssShowFinished
}

public class SweepResultViewController : UIBase
{
    private const int INIT_SCROLLVIEW_POSY = 25;
    /// <summary>
    /// 点击间隔时间
    /// </summary>
    private const float DISTANCE_TIME = 0.5f;


    public SweepResultView view;
    /// <summary>
    /// 扫荡结果物件列表
    /// </summary>
    private List<SweepResultItem_Reward> list_SweepResult = new List<SweepResultItem_Reward>();
    private float sweep_Item_PosY;
    private int sweep_Item_Index;
    private fogs.proto.msg.AddExp addExp;
    private ESweepShowStatus viewStatus;
    private float preCloseTime = 0;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new SweepResultView();
            view.Initialize();
            BtnEventBinding();
        }
        preCloseTime = Time.time;
        EventDelegate.Remove(view.TweenPos_RewardScrollView.onFinished, StartShowReward);
        EventDelegate.Add(view.TweenPos_RewardScrollView.onFinished, StartShowReward);
        sweep_Item_PosY = 0;
        sweep_Item_Index = 0;
        view.UIPanel_RewardScrollView.transform.localPosition = new Vector3(0, INIT_SCROLLVIEW_POSY, 0);
        view.UIPanel_RewardScrollView.clipOffset = Vector2.zero;
        Scheduler.Instance.AddUpdator(Update);
    }

    private void UpdateExp()
    {
        if (addExp != null)
        {
            PlayerData.Instance.UpdateExpInfo(addExp);
            addExp = null;
        }
    }

    public override void Uninitialize()
    {
        InitUIStatus();
        sweep_Item_Index = 0;
        Scheduler.Instance.RemoveTimer(StartShowReward);
        Scheduler.Instance.RemoveTimer(DelayShowReward);
        Scheduler.Instance.RemoveUpdator(Update);
        UpdateExp();
    }


    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Mask.gameObject).onClick = ButtonEvent_Close;
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        if (Time.time - preCloseTime < DISTANCE_TIME)
            return;
        preCloseTime = Time.time;
        switch (viewStatus)
        {
            case ESweepShowStatus.esssShowAnimation:
                {
                    FinishedOperate();
                }
                break;
            case ESweepShowStatus.esssShowFinished:
                {
                    UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SWEEPRESULT);
                }
                break;
        }
    }

    void Update()
    {
        view.UIPanel_RewardScrollView.clipOffset = new Vector2(0, INIT_SCROLLVIEW_POSY - view.UIPanel_RewardScrollView.transform.localPosition.y);
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    private void InitUIStatus()
    {
        if (list_SweepResult.Count > 0)
        {
            for (int i = 0; i < list_SweepResult.Count; i++)
            {
                GameObject.Destroy(list_SweepResult[i].gameObject);
            }
            list_SweepResult.Clear();
        }

        if (view != null)
        {
            if (view.Spt_Hint != null)
                view.Spt_Hint.gameObject.SetActive(false);
            if (view.Obj_Reward_Item == null)
                view.Obj_Reward_Item.gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 复制战斗结果
    /// </summary>
    private void CopyFightReward(MopupDungeonResp vData)
    {
        if (vData == null)
            return;
        if (view.Obj_Reward_Item == null)
            return;
        if (vData.drop_items == null || vData.drop_items.Count <= 0)
            return;
        int tmpSweepCount = vData.drop_items.Count;
        int tmpEXP = vData.exp.add_exp / tmpSweepCount;
        for (int i = 0; i < tmpSweepCount; i++)
        {
            GameObject tmpObj = CommonFunction.InstantiateObject(view.Obj_Reward_Item.gameObject, view.Obj_Reward_Item.parent);
            if (tmpObj == null)
                continue;
            SweepResultItem_Reward tmpReward = tmpObj.AddComponent<SweepResultItem_Reward>();
            if (tmpReward == null)
                continue;
            tmpReward.Initialize(i, tmpEXP, vData.drop_items[i]);
            if (tmpReward.gameObject.collider != null)
                tmpReward.gameObject.collider.enabled = false;
            list_SweepResult.Add(tmpReward);
        }
    }

    private void OpenContentCollider()
    {
        for (int i = 0; i < list_SweepResult.Count; i++)
        {
            SweepResultItem_Reward comp = list_SweepResult[i];
            if (comp.gameObject.collider != null)
            {
                comp.gameObject.collider.enabled = true;
            }
        }
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    private void StartShowReward()
    {
        Scheduler.Instance.RemoveTimer(StartShowReward);
        if (list_SweepResult == null || list_SweepResult.Count <= 0)
            return;

        if (sweep_Item_Index >= list_SweepResult.Count)
        {
            FinishedOperate();
            return;
        }
        if (list_SweepResult[sweep_Item_Index] == null)
        {
            sweep_Item_Index += 1;
            StartShowReward();
            return;
        }

        if (view.UIPanel_RewardScrollView.transform.localPosition.y + sweep_Item_PosY < -120)
        {
            view.TweenPos_RewardScrollView.from = view.UIPanel_RewardScrollView.transform.localPosition;
            view.TweenPos_RewardScrollView.to = new Vector3(0, INIT_SCROLLVIEW_POSY - sweep_Item_PosY, 0);
            view.TweenPos_RewardScrollView.duration = 1;
            view.TweenPos_RewardScrollView.Restart();
            view.TweenPos_RewardScrollView.PlayForward();
            return;
        }

        //sweep_Item_PosY = list_SweepResult[sweep_Item_Index].transform.localPosition.y;
        list_SweepResult[sweep_Item_Index].gameObject.SetActive(true);
        sweep_Item_Index += 1;
        if (sweep_Item_Index < list_SweepResult.Count)
            sweep_Item_PosY = list_SweepResult[sweep_Item_Index].transform.localPosition.y;
        Scheduler.Instance.AddTimer(0.5f, false, StartShowReward);



        //Scheduler.Instance.RemoveTimer(StartShowReward);
        //if (list_SweepResult == null)
        //    return;

        //if (sweep_Item_Index >= list_SweepResult.Count)
        //{
        //    OpenContentCollider();
        //    UpdateExp();
        //    Scheduler.Instance.RemoveUpdator(Update);
        //    view.Spt_Hint.gameObject.SetActive(true);
        //    return;
        //}
        //if (list_SweepResult[sweep_Item_Index] == null)
        //    return;

        //if (view.UIPanel_RewardScrollView.transform.localPosition.y + sweep_Item_PosY < -120)
        //{
        //    view.TweenPos_RewardScrollView.from = view.UIPanel_RewardScrollView.transform.localPosition;
        //    view.TweenPos_RewardScrollView.to = new Vector3(0, INIT_SCROLLVIEW_POSY - sweep_Item_PosY, 0);
        //    view.TweenPos_RewardScrollView.Restart();
        //    view.TweenPos_RewardScrollView.PlayForward();
        //    return;
        //}

        //list_SweepResult[sweep_Item_Index].transform.localPosition = new Vector3(0, sweep_Item_PosY, 0);
        //sweep_Item_PosY -= list_SweepResult[sweep_Item_Index].ShowUIStatus();
        //list_SweepResult[sweep_Item_Index].gameObject.SetActive(true);
        //sweep_Item_Index += 1;
        //Scheduler.Instance.AddTimer(0.5f, false, StartShowReward);
    }

    /// <summary>
    /// 刷新所有扫荡次数物品
    /// </summary>
    private void RefreshWholeItem()
    {
        Scheduler.Instance.RemoveTimer(StartShowReward);
        if (list_SweepResult == null)
            return;
        int tmpItemPosY = 0;
        for (int i = 0; i < list_SweepResult.Count; i++)
        {
            if (list_SweepResult[i] == null)
                continue;
            list_SweepResult[i].transform.localPosition = new Vector3(0, tmpItemPosY, 0);
            tmpItemPosY -= list_SweepResult[i].ShowUIStatus();
            list_SweepResult[i].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// 结束操作
    /// </summary>
    private void FinishedOperate()
    {
        Scheduler.Instance.RemoveTimer(StartShowReward);
        if (list_SweepResult != null && list_SweepResult.Count > 0)
        {
            for (int i = 0; i < list_SweepResult.Count; i++)
            {
                if (list_SweepResult[i] == null)
                    continue;
                list_SweepResult[i].gameObject.SetActive(true);
            }
            view.TweenPos_RewardScrollView.enabled = false;
            sweep_Item_Index = list_SweepResult.Count - 1;
            view.TweenPos_RewardScrollView.transform.localPosition = new Vector3(0, INIT_SCROLLVIEW_POSY - list_SweepResult[sweep_Item_Index].transform.localPosition.y, 0);
        }
        OpenContentCollider();
        UpdateExp();
        view.Spt_Hint.gameObject.SetActive(true);
        viewStatus = ESweepShowStatus.esssShowFinished;
    }

    private bool CheckEquipsOverflow(List<DropList> list)
    {
        for (int i = 0; i < list.Count; i++)
        {
            DropList drop = list[i];
            if (drop == null)
                continue;
            if (drop.mail_list != null && drop.mail_list.Count > 0)  //装备溢出
            {
                return true;
            }
        }
        return false;
    }

    private bool CheckSoldierOverflow(List<DropList> list)
    {
        if (list == null) return false;
        for (int i = 0; i < list.Count; i++)
        {
            List<Attachment> mailList = list[i].mail_list;
            if (mailList == null)
                continue;
            for (int j = 0; j < mailList.Count; j++)
            {
                Attachment info = mailList[j];
                if (info == null)
                    continue;
                IDType idType = CommonFunction.GetTypeOfID(info.id.ToString());
                if (idType == IDType.Soldier)
                {
                    return true;
                }
            }
        }
        return false;
    }


    /// <summary>
    /// 更新界面
    /// </summary>
    /// <param name="vData"></param>
    public void RefreshUIStatus(MopupDungeonResp vData)
    {
        addExp = vData.exp;
        InitUIStatus();
        view.ScrView_RewardScrollView.ResetPosition();
        view.UIPanel_RewardScrollView.transform.localPosition = new Vector3(0, INIT_SCROLLVIEW_POSY, 0);
        view.UIPanel_RewardScrollView.clipOffset = Vector2.zero;
        view.TweenPos_RewardScrollView.enabled = true;
        view.TweenPos_RewardScrollView.from = view.UIPanel_RewardScrollView.transform.localPosition;
        view.TweenPos_RewardScrollView.to = view.UIPanel_RewardScrollView.transform.localPosition;
        view.TweenPos_RewardScrollView.duration = 1;
        view.TweenPos_RewardScrollView.Restart();
        if (vData == null)
            return;
        bool equipOverflow = CheckEquipsOverflow(vData.drop_items);
        bool soldierOverflow = CheckSoldierOverflow(vData.drop_items);
        if (equipOverflow && soldierOverflow)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GATE_HINTTIP_BACKPACKANDSOLDIEROVERFLOW);
        }
        else if (soldierOverflow)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GATE_HINTTIP_SOLDIEROVERFLOW, SoldierDepot.MAXCOUNT));
        }
        else if (equipOverflow)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
        }
        CopyFightReward(vData);
        Scheduler.Instance.AddTimer(0.5f, false, DelayShowReward);
    }

    private void DelayShowReward()
    {
        RefreshWholeItem();
        viewStatus = ESweepShowStatus.esssShowAnimation;
        StartShowReward();
    }


    public override void Destroy()
    {
        base.Destroy();
        Scheduler.Instance.RemoveTimer(StartShowReward);
        Scheduler.Instance.RemoveTimer(DelayShowReward);
        Scheduler.Instance.RemoveUpdator(Update);
        view = null;
        sweep_Item_Index = 0;
        list_SweepResult.Clear();
    }
}
