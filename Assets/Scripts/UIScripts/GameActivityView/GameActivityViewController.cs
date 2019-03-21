using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
using Assets.Script.Common;
using System.Linq;

public class GameActivityViewController : UIBase
{
    const string ITEMNAME = "Item_{0}";
    public GameActivityView view;
    private List<GameActivitesItem> _activiesList;
    private GameActivitesItem _selectedItem;
    private ActivityBuyFundItem _activityBuyFundItem;
    private ActivityBuyFundCountItem _activityFundCountItem;
    private ActivityTextDescItem _activityTescDescItem;
    private ActivityRankItem _activityRankItem;
    private List<GameActivityContentItemBase> _activityBaseList;
    private uint _curActivityID;
    public uint CurActivityID
    {
        get { return _curActivityID; }

    }
    private GameActivityType _curActivityType;
    private float _bgHeightL = 75f;
    private float _bgHeightS = 31f;
    private int _backDepth = 8;
    private int _frontDepth = 11;
    private long _restTime = 0;
    private string _titleStr = "";
    private bool _isUpdating = false;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new GameActivityView();
            view.Initialize();
            BtnEventBinding();
            _activiesList = new List<GameActivitesItem>();
            _activityBaseList = new List<GameActivityContentItemBase>();
            _selectedItem = null;
        }

        view.Tbl_DescTable.sorting = UITable.Sorting.Alphabetic;

        if (_activiesList.Count < 1)
        {
            Main.Instance.StartCoroutine(InitActivitiesListInspector(GameActivityModule.Instance.ActivityTimeInfos));
            GameActivityModule.Instance.DayRefresh = false;
        }
        else
        {
            TryUpdateGameActivityIsNew(PlayerData.Instance.NewGameActivityIDList, true);
        }

        if (_curActivityID != 0)
        {
            OnSendActivityQueryInfo();
        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        if (GameActivityModule.Instance.DayRefresh)
            UISystem.Instance.DelGameUI(ViewType.DIR_VIEWNAME_GAMEACTIVITY);
        else
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_GAMEACTIVITY);
        }

    }

    public override void Uninitialize()
    {

    }

    public override void Destroy()
    {
        base.Destroy();
        _curActivityID = 0;
        _isUpdating = false;
        _curActivityType = GameActivityType.None;
        Scheduler.Instance.RemoveTimer(CountDownTimer);
        ClearPanel();
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }

    public void OnUpdateActivityRewardInfo(ActivityRewardResp response)
    {
        UpdateActivityRankContentInfo(response.info.rank_info);
        UpdateActivityContentListState(response.info.num, response.info.rewards);
        if (response.drop_items == null)
        {
            Debug.LogError("OnUpdateActivityRewardInfo no drop info !!! id = " + response.activity_id + " condition =" + response.condition);
            return;
        }
        DropList data = response.drop_items;
        List<CommonItemData> list = CommonFunction.GetCommonItemDataList(data.item_list, data.equip_list, data.soldier_list, data.soul_list, data.pet_list);
        if (data.special_list != null)
        {
            for (int i = 0; i < data.special_list.Count; i++)
            {
                fogs.proto.msg.ItemInfo info = data.special_list[i];
                IDType type = CommonFunction.GetTypeOfID(info.id.ToString());
                switch (type)
                {
                    case IDType.Diamond:
                    case IDType.Gold:
                    case IDType.SP:
                    case IDType.Medal:
                    case IDType.Honor:
                    case IDType.Exp:
                    case IDType.SoldierExp:
                    case IDType.UnionToken:
                    case IDType.Pet:
                        {
                            CommonItemData item = new CommonItemData(info.id, info.change_num);
                            list.Add(item);
                            break;
                        }
                }
            }
        }
        if (list.Count > 0)
        {
            UISystem.Instance.ShowGameUI(RecieveResultVertView.UIName);
            UISystem.Instance.RecieveResultVertView.UpdateRecieveInfo(list, ConstString.RECEIVE_SUCCESS);
        }
        if (data.mail_list != null && data.mail_list.Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPOVERFLOW);
        }
    }

    public void OnUpdateActivityBuyFundInfo(BuyFundsResp response)
    {
        if (_activityBuyFundItem != null)
        {
            _activityBuyFundItem.UpdateState();
            _activityBuyFundItem.UpdateCountNum(response.buy_nums);
        }
        if (_activityFundCountItem != null)
        {
            _activityFundCountItem.UpdateState();
            _activityFundCountItem.UpdateCountNum(response.buy_nums);
        }

        OnSendActivityQueryInfo();
    }
    /// <summary>
    /// 初始化完成了之后再做查询 保证有数据
    /// </summary>
    /// <param name="response"></param>
    public void OnUpdateActivityQueryInfo(QueryActivityResp response)
    {
        if (response.info == null)
        {
            Debug.LogError("OnUpdateActivityQueryInfo can find the info !!! id = " + response.info.activity_id);
            return;
        }
        PlayerData.Instance.BuyFundNum = response.info.buy_funds;
        //Debug.Log("response.info.buy_funds "+ response.info.buy_funds+ " buyNum "+ response.info.num);
        if (_activityBuyFundItem != null)
        {
            _activityBuyFundItem.UpdateState();
            _activityBuyFundItem.UpdateCountNum(response.info.num);
        }
        if (_activityFundCountItem != null)
        {
            _activityFundCountItem.UpdateState();
            _activityFundCountItem.UpdateCountNum(response.info.num);
        }
        UpdateActivityContentListState(response.info.num, response.info.rewards);
        UpdateActivityRankContentInfo(response.info.rank_info);

        if (UISystem.Instance.UIIsOpen(ViewType.DIR_VIEWNAME_MENU))
        {
            UISystem.Instance.MenuView.TryCloseGameActivityNotify();
        }
    }

    public void OnSendActivityQueryInfo()
    {
        GameActivityModule.Instance.OnSendQueryActivityRequest((uint)_curActivityID);
    }

    private void InitAcitvityContent(int id)
    {
        ActivityTimeInfo data = GameActivityModule.Instance.ActivityTimeInfos.Find(s => { return s.id == id; });

        if (data == null)
        {
            Debug.LogError("Cannt find activity data id = " + id);
            return;
        }
        UpdateBannerInfo(_curActivityType, data);
        UpdateDescContent(data);
    }

    private void ButtonEvent_GameActiviyItem(GameObject go)
    {
        if (_isUpdating)
        {
            return;
        }
        GameActivitesItem item = go.GetComponent<GameActivitesItem>();
        if (_selectedItem != null && _selectedItem.ID != item.ID)
        {
            _selectedItem.IsSelect = false;
        }
        _selectedItem = item;
        _selectedItem.IsSelect = true;
        if (_curActivityID != item.ID)
        {
            _curActivityID = (uint)item.ID;
            _curActivityType = item.Type;
            ClearActivityContent();
            InitAcitvityContent(item.ID);
        }
        else
        {
            OnSendActivityQueryInfo();
        }
    }

    public void TryGotoBuyFound()
    {
        int count = _activiesList.Count;
        for (int i = 0; i < count; i++)
        {
            GameActivitesItem item = _activiesList[i];
            if (item.Type == GameActivityType.Fund)
            {
                ButtonEvent_GameActiviyItem(item.gameObject);
                break;
            }
        }
    }
    /// <summary>
    /// 实例化描述之后的内容：
    /// SingleCharge = 1,           充值: N条 多奖励信息 进度条
    /// TotalCharge = 2,            累冲: N条 多奖励信息 进度条
    /// LoginAward = 3,             登陆: N条 多奖励数据 根据配置来
    /// Welfare = 4,                全名福利： 一条购买人数，N调单奖励
    /// Fund = 5,                   开服基金： 一条购买，N调单奖励
    /// BuyingItem = 6,             限时抢购：N调购买数据
    /// Cost = 7,                   消耗：N条 单奖励信息进度条
    /// ExpeditionDouble = 8,       掉落双倍
    /// EventDungeonsDouble = 9,    掉落双倍
    /// ExpeditionChangeDoble = 10, 挑战双倍
    /// EventDungeonsChangeDoble = 11,  挑战双倍
    /// </summary>
    /// <param name="data"></param>
    private void UpdateDescContent(ActivityTimeInfo data)
    {
        GameActivityType _type = (GameActivityType)data.activity_type;
        switch (_type)
        {
            case GameActivityType.SingleCharge:
            case GameActivityType.TotalCharge:
            case GameActivityType.Cost:
            case GameActivityType.OneDayCharge:
            case GameActivityType.PVEGift:
            case GameActivityType.RechargeSingleReturn:
            case GameActivityType.RechargeDoubleReturn:
                Main.Instance.StartCoroutine(CreateChargeAwardsInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityItemDescByType(_type)));
                break;
            case GameActivityType.LoginAward:
                Main.Instance.StartCoroutine(CreateLoginAwardInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityItemDescByType(_type)));
                break;
            case GameActivityType.Welfare:
                Main.Instance.StartCoroutine(CreateWelfareInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityItemDescByType(_type)));
                break;
            case GameActivityType.Fund:
                int diamond = data.@params;
                Main.Instance.StartCoroutine(CreateFundInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), diamond, ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityItemDescByType(_type)));
                break;
            case GameActivityType.BuyingItem:
                Main.Instance.StartCoroutine(CreateBuyingItemInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), data.@params));
                break;
            case GameActivityType.DropDouble_Expedition:
            case GameActivityType.DropDouble_EventDungeons:
            case GameActivityType.DropDouble_Endness:
            case GameActivityType.ChallengeDouble_Expedition:
            case GameActivityType.ChallengeDouble_EventDungeons:
            case GameActivityType.ChallengeDouble_Endness:
            case GameActivityType.DropDouble_Crusade:
            case GameActivityType.DropDouble_Elite:
            case GameActivityType.DropDouble_Guard:
            case GameActivityType.CoinDouble_Arena:
                Main.Instance.StartCoroutine(CreateTextDescInspector(data));
                break;
            case GameActivityType.Rank_Combat:
            case GameActivityType.Rank_Level:
                Main.Instance.StartCoroutine(CreateRankAwardInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), ""));
                break;
            case GameActivityType.RechargeRank:
            case GameActivityType.CostRank:
            case GameActivityType.CrossRealmRechargeRank:
            case GameActivityType.CrossRealmCostRank:
                Main.Instance.StartCoroutine(CreateSinglePayInspector(GameActivityModule.Instance.GetGameActivityReqList(data.reward_infos), ""));
                break;
            //case GameActivityType.AllPay:
            //    Main.Instance.StartCoroutine(CreateAllPayInspector(data.mReqList, ""));
            //    break;
            default:

                break;
        }
    }
    /// <summary>
    /// 充值
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator CreateChargeAwardsInspector(List<GameActivityReqData> list, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create ChargeAward item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_AwardsItemSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivityAwardsItem item = go.AddComponent<ActivityAwardsItem>();
            bool showprogress = _curActivityType != GameActivityType.SingleCharge;
            item.UpdateItemInfo(_curActivityID, data, showprogress, _curActivityType);
            if (_curActivityType == GameActivityType.PVEGift)
            {
                StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)data.condition);
                item.UpdateDesc(string.Format(formatStr, info.GateSequence));
            }
            else
                item.UpdateDesc(string.Format(formatStr, data.condition));
            item.UpdateProgress(0);
            if (_curActivityType == GameActivityType.SingleCharge || _curActivityType == GameActivityType.RechargeSingleReturn || _curActivityType == GameActivityType.RechargeDoubleReturn)
                item.mProgressRelayOnState = true;
            else
                item.mProgressRelayOnState = false;
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }
    /// <summary>
    /// 单服充值
    /// </summary>
    /// <param name="list"></param>
    /// <param name="formatStr"></param>
    /// <returns></returns>
    private IEnumerator CreateSinglePayInspector(List<GameActivityReqData> list, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create SinglePay item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_ActivityRankSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivityRankItem item = go.AddComponent<ActivityRankItem>();

            item.UpdateItemInfo(_curActivityID, data, true);
            item.UpdateRankTypeByActivity(_curActivityType);
            //item.UpdateDesc(string.Format(formatStr, data.mReqNum));
            item.UpdateRank(i + 1);
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }
    ///// <summary>
    ///// 跨服充值
    ///// </summary>
    ///// <param name="list"></param>
    ///// <param name="formatStr"></param>
    ///// <returns></returns>
    //private IEnumerator CreateAllPayInspector(List<GameActivityReqData> list,string formatStr)
    //{
    //    if (list == null || list.Count < 1)
    //    {
    //        Debug.LogError("Cannt create SinglePay item because GameActivityReqData list is null!!! " + _curActivityID);
    //        yield break;
    //    }
    //    _isUpdating = true;
    //    int count = list.Count;
    //    for (int i = 0; i < count; i++)
    //    {
    //        if (!_isUpdating)
    //            yield break;
    //        GameObject go = CommonFunction.InstantiateObject(view.Obj_AwardsItemSource, view.Tbl_DescTable.transform);
    //        GameActivityReqData data = list[i];
    //        ActivityAwardsItem item = go.AddComponent<ActivityAwardsItem>();
    //        item.UpdateItemInfo(_curActivityID, data, true);
    //        item.UpdateDesc(string.Format(formatStr, data.mReqNum));
    //        _activityBaseList.Add(item);
    //        if (!_isUpdating)
    //            yield break;
    //        view.Tbl_DescTable.Reposition();
    //        yield return null;
    //    }
    //    yield return null;
    //    InitContentComplete();
    //}
    /// <summary>
    /// 登陆奖励
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator CreateLoginAwardInspector(List<GameActivityReqData> list, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create LoginAward item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_AwardsItemSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivityAwardsItem item = go.AddComponent<ActivityAwardsItem>();
            item.UpdateItemInfo(_curActivityID, data, false, _curActivityType);
            item.UpdateDesc(string.Format(formatStr, (i + 1)));
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }
    /// <summary>
    /// 限时抢购
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator CreateBuyingItemInspector(List<GameActivityReqData> list, int param)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create Buying item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        //这里要求必须要一个ID
        //uint costID = pList[0];
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_BuyingItemSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivityBuyingItem item = go.AddComponent<ActivityBuyingItem>();
            item.UpdateItemInfo(_curActivityID, data, (uint)param);
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }
    /// <summary>
    /// 全名福利
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator CreateWelfareInspector(List<GameActivityReqData> list, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create Welfare item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        GameObject go1 = CommonFunction.InstantiateObject(view.Obj_BuyFoundCountSource, view.Tbl_DescTable.transform);
        _activityFundCountItem = go1.AddComponent<ActivityBuyFundCountItem>();
        go1.name = "item_first";
        _activityFundCountItem.UpdateCountNum(0);
        view.Tbl_DescTable.Reposition();
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_SingleAwardSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivitySingleAwardItem item = go.AddComponent<ActivitySingleAwardItem>();
            item.UpdateItemInfo(_curActivityID, data);
            item.UpdateDesc(string.Format(formatStr, data.condition));
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }
    /// <summary>
    /// 开服基金
    /// </summary>
    /// <param name="list"></param>
    /// <returns></returns>
    private IEnumerator CreateFundInspector(List<GameActivityReqData> list, int diamondNum, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create Fund item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        GameObject go1 = CommonFunction.InstantiateObject(view.Obj_BuyFoundSource, view.Tbl_DescTable.transform);
        go1.name = "item_first";
        _activityBuyFundItem = go1.AddComponent<ActivityBuyFundItem>();
        _activityBuyFundItem.UpdateItemInfo(_curActivityID, diamondNum);
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_SingleAwardSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivitySingleAwardItem item = go.AddComponent<ActivitySingleAwardItem>();
            item.UpdateItemInfo(_curActivityID, data);
            item.UpdateDesc(string.Format(formatStr, data.condition));
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }

    private IEnumerator CreateTextDescInspector(ActivityTimeInfo data)
    {
        if (data == null)
        {
            Debug.LogError("Cannt create TextDesc item because GameActivityData  is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        GameObject go = CommonFunction.InstantiateObject(view.Obj_TextDescSource, view.Tbl_DescTable.transform);
        _activityTescDescItem = go.AddComponent<ActivityTextDescItem>();
        _activityTescDescItem.UpdateItem(data);
        yield return null;
        view.Tbl_DescTable.Reposition();
        yield return null;
        InitContentComplete();
    }
    /// <summary>
    /// 这个冲榜活动 描述要是chunwenzi
    /// </summary>
    /// <param name="list"></param>
    /// <param name="formatStr"></param>
    /// <returns></returns>
    private IEnumerator CreateRankAwardInspector(List<GameActivityReqData> list, string formatStr)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt create LoginAward item because GameActivityReqData list is null!!! " + _curActivityID);
            yield break;
        }
        _isUpdating = true;
        int count = list.Count;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            GameObject go = CommonFunction.InstantiateObject(view.Obj_ActivityRankSource, view.Tbl_DescTable.transform);
            go.name = string.Format("item{0:D3}", i);
            GameActivityReqData data = list[i];
            ActivityRankItem item = go.AddComponent<ActivityRankItem>();
            item.UpdateItemInfo(_curActivityID, data, false);
            item.UpdateRankTypeByActivity(_curActivityType);
            item.UpdateRank(i + 1);
            _activityBaseList.Add(item);
            if (!_isUpdating)
                yield break;
            view.Tbl_DescTable.Reposition();
            yield return null;
        }
        yield return null;
        InitContentComplete();
    }

    private IEnumerator InitActivitiesListInspector(List<ActivityTimeInfo> list)
    {
        if (list == null || list.Count < 1)
        {
            Debug.LogError("Cannt find any Activities info in local config!!!");
            yield break;
        }
        //for (int i = 0; i < list.Count; i++)
        //{
        //    Debug.LogError("id: " + list[i].id + "////type:" + (GameActivityType)list[i].activity_type);
        //}
        list = list.OrderBy(s => ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityPriotityByType((GameActivityType)s.activity_type)).ToList();
        int count = list.Count;
        _isUpdating = true;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            //GameActivityData data = list[i];
            //data.UpdateState();
            GameObject go = CommonFunction.InstantiateObject(view.Obj_GameActivitesItemSource, view.Grd_ActivitesGrid.transform);
            GameActivitesItem item = go.AddComponent<GameActivitesItem>();
            item.name = string.Format(ITEMNAME, list[i].id);
            item.UpdateItemInfo(list[i]);
            _activiesList.Add(item);
            UIEventListener.Get(go).onClick = ButtonEvent_GameActiviyItem;
            if (!_isUpdating)
                yield break;
            view.Grd_ActivitesGrid.Reposition();
            yield return null;
        }
        _isUpdating = false;
        ButtonEvent_GameActiviyItem(_activiesList[0].gameObject);
        view.ScrView_ActivitesScrollView.ResetPosition();
        TryUpdateGameActivityIsNew(PlayerData.Instance.NewGameActivityIDList, true);
    }

    private void ClearPanel()
    {
        ClearActivites();
        ClearActivityContent();
    }

    private void ClearActivites()
    {
        if (_activiesList != null && _activiesList.Count > 0)
        {
            int count = _activiesList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                GameActivitesItem item = _activiesList[i];
                _activiesList.Remove(item);
                GameObject.Destroy(item.gameObject);
            }
            _activiesList.Clear();
        }
    }

    private void ClearActivityContent()
    {
        if (_activityBuyFundItem != null)
        {
            GameObject.Destroy(_activityBuyFundItem.gameObject);
        }
        if (_activityFundCountItem != null)
        {
            GameObject.Destroy(_activityFundCountItem.gameObject);
        }
        if (_activityTescDescItem != null)
        {
            GameObject.Destroy(_activityTescDescItem.gameObject);
        }
        if (_activityBaseList != null && _activityBaseList.Count > 0)
        {
            int count = _activityBaseList.Count;
            for (int i = count - 1; i >= 0; i--)
            {
                GameActivityContentItemBase item = _activityBaseList[i];
                _activityBaseList.Remove(item);
                GameObject.Destroy(item.gameObject);
            }
            _activityBaseList.Clear();
        }
        if (view != null)
        {
            if (view.Tbl_DescTable != null)
                view.Tbl_DescTable.Reposition();

            if (view.ScrView_DescScrollView != null)
                view.ScrView_DescScrollView.ResetPosition();
        }

    }
    /// <summary>
    /// 更新领取状态 右边的 有没有已经领取 或则进度条更新的
    /// </summary>
    private void UpdateActivityContentListState(int num, List<ActivityReward> list)
    {
        //Debug.Log(num + " list.count " + list.Count);
        if (_curActivityType == GameActivityType.Welfare || _curActivityType == GameActivityType.Fund)
        {
            if (_activityFundCountItem != null)
                _activityFundCountItem.UpdateCountNum(num);
            if (_activityBuyFundItem != null)
                _activityBuyFundItem.UpdateCountNum(num);
        }

        if (list == null || list.Count < 1)
            return;
        int count1 = list.Count;
        int count2 = _activityBaseList.Count;
        for (int i = 0; i < count1; i++)
        {
            ActivityReward info = list[i];
            //Debug.LogWarning("info condition  " + info.condition + " state " + info.state);
            for (int j = 0; j < count2; j++)
            {
                GameActivityContentItemBase item = _activityBaseList[j];
                //Debug.Log(" " +  item.SignID);
                if (item.SignID == (uint)info.condition)
                {
                    if (_curActivityType == GameActivityType.PVEGift && (GameAcitvityAwardStateEnum)info.state == GameAcitvityAwardStateEnum.Recieved)
                    {
                        item.gameObject.name = string.Format("itemdone{0:D3}", j);
                    }
                    item.UpdateState((GameAcitvityAwardStateEnum)info.state);
                    if (_curActivityType == GameActivityType.SingleCharge)
                    {
                        item.UpdateProgress(info);
                    }
                    else
                    {
                        item.UpdateProgress((uint)num);
                    }
                    break;
                }
            }
        }
        view.Tbl_DescTable.Reposition();
    }

    private void UpdateActivityRankContentInfo(List<ActivityRankInfo> list)
    {
        if (list == null || list.Count < 1)
            return;
        int count1 = Mathf.Min(_activityBaseList.Count, list.Count);
        for (int i = 0; i < count1; i++)
        {
            ActivityRankInfo info = list[i];
            GameActivityContentItemBase item = _activityBaseList[i];
            item.UpdateRankInfo(info);
        }
    }

    private void InitContentComplete()
    {
        _isUpdating = false;
        if (view != null)
        {
            if (view.Tbl_DescTable != null)
                view.Tbl_DescTable.Reposition();
            if (view.ScrView_DescScrollView != null)
                view.ScrView_DescScrollView.ResetPosition();
        }
        OnSendActivityQueryInfo();
    }

    public void TryUpdateGameActivityIsNew(List<int> activityIDList, bool isNew)
    {
        if (activityIDList == null || activityIDList.Count < 1)
        {
            if (_activiesList != null)
            {
                for (int i = 0; i < _activiesList.Count; i++)
                {
                    GameActivitesItem item = _activiesList[i];
                    item.IsNewActivity = false;
                }
            }
            return;
        }
        if (_activiesList == null || _activiesList.Count < 1)
            return;
        int count1 = _activiesList.Count;
        int count2 = activityIDList.Count;
        for (int i = 0; i < count1; i++)
        {
            GameActivitesItem item = _activiesList[i];
            item.IsNewActivity = false;
            for (int j = 0; j < count2; j++)
            {
                int id = activityIDList[j];
                if (item.ID == id)
                {
                    item.IsNewActivity = isNew;
                    break;
                }
            }
        }
    }
    /// <summary>
    /// 开服 全民 双倍掉落 挑战次数
    /// </summary>
    /// <param name="type"></param>
    /// <param name="data"></param>
    private void UpdateBannerInfo(GameActivityType type, ActivityTimeInfo data)
    {
        GameActivityType _type = (GameActivityType)data.activity_type;
        Scheduler.Instance.RemoveTimer(CountDownTimer);
        _restTime = 0;
        CommonFunction.SetSpriteName(view.Spt_BannerBG, ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityBannerBgByType(_type));
        CommonFunction.SetSpriteName(view.Spt_BannerDesc, ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityBannerDescByType(_type));
        CommonFunction.SetSpriteName(view.Spt_BannerIcon, ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityBannerIconByType(_type));
        view.Spt_BannerBG.MakePixelPerfect();
        view.Spt_BannerDesc.MakePixelPerfect();
        view.Spt_BannerIcon.MakePixelPerfect();
        switch (type)
        {
            case GameActivityType.Fund:
            case GameActivityType.Welfare:
                view.Lbl_BannerTitle.text = "";
                view.Lbl_BannerDesc.text = "";
                view.Spt_BannerTextBG.height = (int)_bgHeightS;
                view.Spt_BannerTextBG.depth = _frontDepth;
                break;
            case GameActivityType.DropDouble_EventDungeons:
            case GameActivityType.DropDouble_Expedition:
            case GameActivityType.DropDouble_Endness:
            case GameActivityType.ChallengeDouble_Expedition:
            case GameActivityType.ChallengeDouble_EventDungeons:
            case GameActivityType.ChallengeDouble_Endness:
            case GameActivityType.DropDouble_Crusade:
            case GameActivityType.DropDouble_Elite:
            case GameActivityType.DropDouble_Guard:
            case GameActivityType.CoinDouble_Arena:
                view.Lbl_BannerTitle.text = "";
                view.Lbl_BannerDesc.text = "";
                view.Spt_BannerTextBG.height = 0;
                view.Spt_BannerTextBG.depth = _frontDepth;
                break;
            case GameActivityType.Rank_Combat:
            case GameActivityType.Rank_Level:
                view.Lbl_BannerTitle.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityNameByType(_type);
                view.Lbl_BannerDesc.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityDescriptionByType(_type);
                view.Spt_BannerTextBG.height = (int)_bgHeightL;
                view.Spt_BannerTextBG.depth = _backDepth;
                break;
            case GameActivityType.OneDayCharge:
                view.Lbl_BannerTitle.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityNameByType(_type);
                view.Lbl_BannerDesc.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityDescriptionByType(_type);
                view.Spt_BannerTextBG.height = (int)_bgHeightL;
                view.Spt_BannerTextBG.depth = _backDepth;
                _titleStr = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityNameByType(_type);
                DateTime eDateTime = CommonFunction.GetTimeByLong((long)data.end_time);
                DateTime nDateTime = CommonFunction.GetTimeByLong(Main.mTime);
                if (eDateTime.CompareTo(nDateTime) > 0)
                {
                    TimeSpan ts = eDateTime.Subtract(nDateTime);
                    _restTime = (long)ts.TotalSeconds;
                    view.Lbl_BannerTitle.text = _titleStr + string.Format(ConstString.GAMEACTIVTIY_LABEL_UNTILEND, CommonFunction.GetTimeString(_restTime));
                    Scheduler.Instance.AddTimer(1f, true, CountDownTimer);
                }
                break;
            default:
                view.Lbl_BannerTitle.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityNameByType(_type);
                view.Lbl_BannerDesc.text = ConfigManager.Instance.mGameAcitivityTypeConfig.GetGameActivityDescriptionByType(_type);
                view.Spt_BannerTextBG.height = (int)_bgHeightL;
                view.Spt_BannerTextBG.depth = _backDepth;
                break;
        }
    }

    private void CountDownTimer()
    {
        if (_restTime > 1)
        {
            _restTime -= 1;
        }
        else
        {
            _restTime = 0;
            Scheduler.Instance.RemoveTimer(CountDownTimer);
            view.Lbl_BannerTitle.text = _titleStr;
        }
        view.Lbl_BannerTitle.text = _titleStr + string.Format(ConstString.GAMEACTIVTIY_LABEL_UNTILEND, CommonFunction.GetTimeString(_restTime));
    }

}
