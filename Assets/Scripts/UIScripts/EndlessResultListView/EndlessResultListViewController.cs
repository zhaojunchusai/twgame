using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class EndlessResultListViewController : UIBase 
{

    private const float INIT_SCROLLVIEW_POSY = -10f;

    public EndlessResultListView view;
    private List<EndlessResultItem> listRewardItem = new List<EndlessResultItem>();

    public override void Initialize()
    {
        if (view == null)
            view = new EndlessResultListView();
        view.Initialize();
        BtnEventBinding();
        ShowUIStatus();
    }

    public override void Uninitialize()
    {
        InitUIStatus();
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        listRewardItem.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Mask.gameObject).onClick = ButtonEvent_Mask;
    }

    public void ButtonEvent_Mask(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ENDLESSRESULTLIST);
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    private void InitUIStatus()
    {
        if (listRewardItem.Count > 0)
        {
            for (int i = 0; i < listRewardItem.Count; i++)
                GameObject.Destroy(listRewardItem[i].gameObject);
            listRewardItem.Clear();
        }

        if (view != null)
        {
            if (view.Obj_Reward_Item == null)
                view.Obj_Reward_Item.gameObject.SetActive(false);
            if (view.ScrView_RewardScrollView != null)
                view.ScrView_RewardScrollView.ResetPosition();
            if (view.UIPanel_RewardScrollView != null)
            {
                view.UIPanel_RewardScrollView.transform.localPosition = new Vector3(0, INIT_SCROLLVIEW_POSY, 0);
                view.UIPanel_RewardScrollView.clipOffset = Vector2.zero;
            }
        }
    }

    /// <summary>
    /// 显示界面
    /// </summary>
    private void ShowUIStatus()
    {
        InitUIStatus();
        CopyRewardItem();
    }

    /// <summary>
    /// 复制奖励
    /// </summary>
    private void CopyRewardItem()
    {
        if (view == null)
            return;
        if (view.Obj_Reward_Item == null)
            return;
        if (FightRelatedModule.Instance.listEndlessData == null)
            return;
        float tmpItemPosY = 0;
        for (int i = 0; i < FightRelatedModule.Instance.listEndlessData.Count; i++)
        {
            if (FightRelatedModule.Instance.listEndlessData[i].fight_result != 1)
                continue;
            GameObject tmpObj = CommonFunction.InstantiateObject(view.Obj_Reward_Item.gameObject, view.Obj_Reward_Item.parent);
            if (tmpObj == null)
                continue;
            EndlessResultItem tmpReward = tmpObj.AddComponent<EndlessResultItem>();
            if (tmpReward == null)
                continue;
            tmpItemPosY = tmpReward.Initialize(i, FightRelatedModule.Instance.listEndlessData[i], tmpItemPosY);
            tmpObj.SetActive(true);
            listRewardItem.Add(tmpReward);
        }
    }
}
