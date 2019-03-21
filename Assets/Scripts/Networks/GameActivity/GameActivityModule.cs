using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using ProtoBuf;

public class GameActivityModule : Singleton<GameActivityModule>
{
    public GameActivityNetwork mNetWork;
    public List<ActivityTimeInfo> ActivityTimeInfos = new List<ActivityTimeInfo>();
    public bool DayRefresh = false;
    public void SetDayReSet()
    {
        if (!UISystem.Instance.UIIsOpen(GameActivityView.UIName))
        {
            UISystem.Instance.DelGameUI(GameActivityView.UIName);
        }
        OnSendQueryActivityTimeReq();
    }

    public void OnSendBuyFoundRequest()
    {
        //VIP4才可以购买
        if (PlayerData.Instance._VipLv < int.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SERVER_FOUNDATION_VIP)))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.GAMEACTIVTIY_FOUND_REQVIP,
                ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SERVER_FOUNDATION_VIP)));
        }
        else
        {
            mNetWork.OnSendBuyFundRequest();
            Debug.Log("OnSendBuyFoundRequest......");
        }
    }

    public void OnReceiveBuyFundResponse(BuyFundsResp response)
    {
        Debug.Log("OnReceiveBuyFundResponse...... reslut = " + response.result);
        if (response.result == 0)
        {
            UISystem.Instance.GameActivityView.OnUpdateActivityBuyFundInfo(response);
            PlayerData.Instance.UpdateDiamond(response.diamond);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GAMEACTIVTIY_FOUND_SUCCEED);
        }
        else
            ErrorCode.ShowErrorTip(response.result);
    }

    public void OnSendQueryActivityRequest(uint id)
    {
        Debug.Log("OnSendQueryActivityRequest ...... id = " + id);
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.GAMEACTIVTIY_WELFARE_IWANTTOBUY);
        mNetWork.OnSendQueryActivityRequest(id);
    }

    public void OnReceiveQueryActivityResponse(QueryActivityResp response)
    {
        Debug.Log("OnReceiveQueryActivityResponse ...... result = " + response.result);
        if (response.result == 0)
        {
            UISystem.Instance.GameActivityView.OnUpdateActivityQueryInfo(response);
        }
        else
            ErrorCode.ShowErrorTip(response.result);

    }

    public void OnSendActivityRewardRequest(uint id, uint condition)
    {
        Debug.Log("OnSendActivityRewardRequest ...... id = " + id + " condition " + condition);
        mNetWork.OnSendActivityRewardRequest(id, condition);
    }

    public void OnReceiveActivityRewardResponse(ActivityRewardResp response)
    {
        if (response.result == 0)
        {
            UISystem.Instance.GameActivityView.OnUpdateActivityRewardInfo(response);
            if (response.drop_items != null)
            {
                //CommonFunction.LogDropData(response.drop_items);
                PlayerData.Instance.UpdatePlayerAttribute(response.attr);
                PlayerData.Instance.UpdateDropData(response.drop_items);
            }
            if (response.consume_item != null)
            {
                //Debug.Log("COST>>>>>>>>>>> "+ response.consume_item.id + " num " + response.consume_item.num + " chang_num " + response.consume_item.change_num);
                PlayerData.Instance.UpdateItem(response.consume_item);
            }
        }
        else
            ErrorCode.ShowErrorTip(response.result);
    }

    /// <summary>
    /// 用于刷新消耗的完成情况
    /// </summary>
    public void OnTryRefreshCurrentActivityInfo()
    {
        //TODO: 充值VIP并没有关闭游戏活动界面，所以没有及时刷新 这里需要及时刷新消耗类的活动信息
        if (UISystem.Instance.GameActivityView != null)
        {
            uint curID = (uint)UISystem.Instance.GameActivityView.CurActivityID;
            if (curID > 0)
                OnSendQueryActivityRequest(curID);
        }
    }

    public void OnUpdateNewActivity()
    {
        //Debug.LogWarning("PlayerData.Instance.NewGameActivityIDList.Count = " + PlayerData.Instance.NewGameActivityIDList.Count);
        //foreach (uint acitvityID in PlayerData.Instance.NewGameActivityIDList)
        //{
        //    Debug.LogWarning("new id = " + acitvityID);
        //}
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_GAMEACTIVITY))
        {
            UISystem.Instance.GameActivityView.TryUpdateGameActivityIsNew(PlayerData.Instance.NewGameActivityIDList, true);
        }
    }

    public void OnSendQueryActivityTimeReq()
    {
        QueryActivityTimeReq req = new QueryActivityTimeReq();
        mNetWork.OnSendQueryActivityTimeReq(req);
    }

    public void OnReceiveQueryActivityTimeResp(QueryActivityTimeResp data)
    {
        ActivityTimeInfos = data.time_info;
        DayRefresh = true;

        if (UISystem.Instance.UIIsOpen(MainCityView.UIName))
            UISystem.Instance.MainCityView.ForDoubel(data.time_info);

        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_EXPEDITION))
        {
            UISystem.Instance.ExpeditionView.InitDouble(data.time_info);
        }
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_ACTIVITIES))
        {
            UISystem.Instance.ActivitiesView.InitDouble(data.time_info);
        }
        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_ENDLESS))
        {
            UISystem.Instance.EndlessView.InitDouble(data.time_info);
        }

        //PrintAllInfo();
    }

    public void Initialize()
    {
        if (mNetWork == null)
        {
            mNetWork = new GameActivityNetwork();
            mNetWork.RegisterMsg();
        }

        PlayerData.Instance.UpdatePlayerDiamondEvent += OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdatePlayerGoldEvent += OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdatePlayerSPEvent += OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdateMedalEvent += OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdateHonorEvent += OnTryRefreshCurrentActivityInfo;
    }

    public void Uninitialize()
    {
        mNetWork.RemoveMsg();
        mNetWork = null;
        PlayerData.Instance.UpdatePlayerDiamondEvent -= OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdatePlayerGoldEvent -= OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdatePlayerSPEvent -= OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdateMedalEvent -= OnTryRefreshCurrentActivityInfo;
        PlayerData.Instance.UpdateHonorEvent -= OnTryRefreshCurrentActivityInfo;
    }

    public List<GameActivityReqData> GetGameActivityReqList(List<RewardInfo> infos)
    {
        if (infos != null && infos.Count > 0)
        {

            List<GameActivityReqData> list = new List<GameActivityReqData>();
            foreach (RewardInfo info in infos)
            {
                if (info != null && info.rewards.Count > 0)
                {
                    GameActivityReqData tmp = new GameActivityReqData();
                    tmp.rewards_id = (uint)info.reward_id;
                    tmp.condition = info.condition;
                    if (info.rewards != null && info.rewards.Count > 0)
                    {
                        List<ActivityDropItems> dropsList = new List<ActivityDropItems>();
                        foreach (var data in info.rewards)
                        {
                            ActivityDropItems item = new ActivityDropItems();
                            item.drop_id = (uint)data.dropItemID;
                            item.drop_number = data.dropItemNum;
                            item.drop_type = (uint)data.dropItemType;

                            dropsList.Add(item);
                        }
                        tmp.mDrops = dropsList;
                    }
                    list.Add(tmp);
                }
            }
            if (list != null && list.Count > 0)
            {
                return list;
            }
        }
        else
        {
            Debug.LogError("rewardinfo is null");
        }
        return null;
    }
    private void PrintAllInfo()
    {
        foreach (var a in ActivityTimeInfos)
        {
            foreach (var b in a.reward_infos)
            {
                foreach (var c in b.rewards)
                {
                    Debug.LogError("id: " + a.id + "\t type: " + a.activity_type + "\t param: " +
                        a.@params + "\t time: " + CommonFunction.GetTimeByLong(a.start_time) + "-" + CommonFunction.GetTimeByLong(a.end_time) +
                        "\t reward_id: " + b.reward_id + "\t condition: " + b.condition +
                        "\t drop_id: " + c.dropItemID + "\t drop_num: " + c.dropItemNum + "\t drop_type: " + c.dropItemType
                        );

                }
            }
        }
    }
}
