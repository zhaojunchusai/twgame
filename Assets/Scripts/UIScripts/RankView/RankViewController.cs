using UnityEngine;
using Assets.Script.Common;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;
/****************
 * 排行榜界面 
 * 演武榜事实刷新 
 * 其他三个排行榜服务端按照配置的时间来刷新，刷新时间服务端会在登陆的时候传给客户端，这个时间只做显示用
 * 刷新时间较长，其他三个排行榜客户端会缓存排行榜数据，减少不必要的通信
 * 刷新的时候服务端会推送小红点 type == NotifyType.RANK_UPDATE，客户端收到小红点，再次进入排行榜就要重新请求并缓存数据
 * 数据缓存在RankModule里面，由于排行榜每次都要截取自己的信息，所以每次都是copy一份过来
 */
public class RankViewController : UIBase
{
    public RankView view;
    const int MAXCOUNT = 5;
    const string ITEMNAME = "RankItem_{0}";
    private RankType _currentType = RankType.LEVEL_RANK;
    private List<RankItem> _itemList;
    private RankInfo _selfInfo;
    private List<RankInfo> _currentList;
    private string refreshTimeStr;

    private List<RankViewTypeBtn> _typeBtnList = new List<RankViewTypeBtn>();
    private Dictionary<uint, RankViewSubTypeObj> _subTypeObjDic = new Dictionary<uint, RankViewSubTypeObj>();
    private uint _selectedTypeID = 0;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new RankView();
            view.Initialize();
            BtnEventBinding();
            _itemList = new List<RankItem>();
            _currentList = new List<RankInfo>();
            _selfInfo = null;
            InitTypeBtns();
        }
        Assets.Script.Common.Scheduler.Instance.AddLateUpdator(UpdateTable);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenRankView);
    }

    public override void Uninitialize()
    {
        Assets.Script.Common.Scheduler.Instance.RemoveLateUpdator(UpdateTable);
    }

    public override void Destroy()
    {
        base.Destroy();
        DestroyItemList();
        _itemList.Clear();
        _currentList.Clear();
        view = null;
        _typeBtnList.Clear();
        _subTypeObjDic.Clear();
        _selectedTypeID = 0;
    }

    private void UpdateTable()
    {
        view.UITable_UITable.Reposition();
    }

    private void InitTypeBtns()
    {
        List<RankviewData> list = ConfigManager.Instance.mRankviewConfig.DataList;

        foreach (RankviewData item in list)
        {
            if (item.Level == 2)
                continue;
            RankViewTypeBtn btn = CommonFunction.InstantiateItem<RankViewTypeBtn>(view.Gobj_TypeBtn, view.UITable_UITable.transform);
            btn.Init(item, ClickType);
            _typeBtnList.Add(btn);
            if (item.ChildIDs != null && item.ChildIDs.Count > 0)
            {
                RankViewSubTypeObj sub = CommonFunction.InstantiateItem<RankViewSubTypeObj>(view.Gobj_RankViewSubTypeObj, view.UITable_UITable.transform);
                sub.Init(view.Trans_SelectedSprite, view.Gobj_SubTypeBtn, item, ClickType, 1);
                sub.transform.localScale = new Vector3(1, 0.001f, 1);
                sub.gameObject.SetActive(false);
                _subTypeObjDic.Add(item.ID, sub);
                btn.SubObj = sub;
            }
        }
        view.UITable_UITable.repositionNow = true;
    }

    private void ClickType(uint id)
    {
        if (id == 0)
            return;
        RankviewData selectdata = ConfigManager.Instance.mRankviewConfig.GetRankviewDataByID(_selectedTypeID);
        //两个ID一致
        if (_selectedTypeID == id && _selectedTypeID != 0)
        {
            //点了有子项的同一个类型按钮，
            if (selectdata.Level == 1 && selectdata.HasChild)
            {
                if (_subTypeObjDic.ContainsKey(_selectedTypeID))
                {
                    _subTypeObjDic[_selectedTypeID].SwitchState();
                }
            }
            else
            {
                OnRequestRank((RankType)selectdata.Type);
            }
            return;
        }
        RankviewData newdata = ConfigManager.Instance.mRankviewConfig.GetRankviewDataByID(id);
        //ID不一致
        if (_selectedTypeID != id)
        {
            //二级菜单跳一级菜单，一级菜单跳一级菜单
            if (newdata.Level == 1)
            {
                //选中的是其父级，则关闭，且不需要发送消息
                if (_selectedTypeID != 0 && selectdata.ParentID == id)
                {
                    if (_subTypeObjDic.ContainsKey(id))
                    {
                        _selectedTypeID = id;
                        _subTypeObjDic[id].Close();
                        return;
                    }
                }

                uint parID = _selectedTypeID;
                if (_selectedTypeID != 0 && selectdata.Level == 2)
                {
                    parID = selectdata.ParentID;
                }
                foreach (RankViewTypeBtn item in _typeBtnList)
                {
                    if (item.ID == parID)
                    {
                        item.SetBtnState(false);
                        if (item.Data.HasChild && item.SubObj.IsOpen)
                        {
                            item.SubObj.Close();
                        }
                    }
                }
            }
            else
            {
                if (newdata.ParentID != _selectedTypeID && newdata.ParentID != selectdata.ParentID)
                    Debug.LogError("ERROR:" + _selectedTypeID + " " + id);
                //一级菜单跳二级，或者二级跳二级，只能是自己该类别
                //否则就是逻辑有问题
                //自己类别中的跳转不需要在此做UI显示变化
            }

        }
        _selectedTypeID = id;
        foreach (RankViewTypeBtn item in _typeBtnList)
        {
            if (item.ID == id)
            {
                item.SetBtnState(true);
                if (item.Data.HasChild)
                {
                    item.SubObj.Open();
                }
            }
        }

        //当选择的是有子项类别的时候，不发送请求，而是由sub物体打开时默认选择的选项发送请求
        if (!newdata.HasChild)
            OnRequestRank((RankType)newdata.Type);
    }

    public void ShowRank(RankType type)
    {
        ClickType(ConfigManager.Instance.mRankviewConfig.GetRankIDByType(type));
    }

    public void ButtonEvent_ClosePanel(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_RANKVIEW);
    }

    public void ButtonEvent_RankItem(GameObject go)
    {
        RankItem item = go.GetComponent<RankItem>();
        if (item != null && item._type == RankType.CAMPAIGN_UNION)
            return;
        if (item._type == RankType.POLE_RANK||item._type==RankType.CROSSSERVERWAR_UNION)
            return;
        if ((item._type == RankType.UNIONOVERLORD) || (item._type == RankType.EXPLORE_RANK) || (item._type == RankType.GRID_RANK))
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_UNIONPRISONINFOVIEW);
            List<string> listInfo = new List<string>();
            listInfo.Add(item.mInformation.union_info.union_name);
            listInfo.Add(item.mInformation.union_info.host_union_name);
            listInfo.Add(string.Format("{0}/{1}", item.mInformation.union_info.members, item.mInformation.union_info.max_members));
            listInfo.Add(item.mInformation.union_info.chairman);
            UISystem.Instance.UnionPrisonInfoView.SetInfo(item.mInformation.union_info.icon, (int)item.mInformation.union_info.union_level,
                                                    item.mInformation.union_info.altar_status, 15, listInfo, 1, item.ObtainUnionMembersInfo);
            return;
        }
        RankModule.Instance.SendRankPalyerInfoRequset(_currentType, item.mInformation);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_ClosePanel;
        view.UIWrapContent_UIGrid.onInitializeItem = UpdateRankItemInfo;
    }

    private void OnRequestRank(RankType type)
    {
        //if (type == RankType.UNIONOVERLORD)
        //    type = RankType.GRID_RANK;
        _currentType = type;
        RankModule.Instance.SendRankInfoRequsest(_currentType);
    }

    public void OnUpdateRankInfo(List<RankInfo> source)
    {
        //最少要包含自己的信息 未上榜也要
        if (source == null || source.Count < 1)
        {
            Debug.LogError("OnUpdateRankInfo list is null !!!! ");
            return;
        }
        _currentList.Clear();
        _currentList = CommonFunction.CopyRankInfo(source);
        //排行榜第一条是自己的信息
        int selfIndex = 0;
        _selfInfo = _currentList[selfIndex];
        _currentList.RemoveAt(selfIndex);
        view.RankItem_SelfRankItem.UpdateItem(_currentType, _selfInfo);
        Main.Instance.StartCoroutine(UpdateRankContentInspector(_currentList));
        //排行榜有几个刷新时间 这个是策划配置给服务端的，所以需要服务的发给客户端 只需要在登陆的时候发一次就可以了
        //if (_currentType != RankType.ARENA_RANK && _currentType != RankType.CAMPAIGN_PLAYER && _currentType != RankType.CAMPAIGN_UNION)
        //{
        if (_currentType == RankType.POLE_RANK || _currentType == RankType.ARENA_RANK || _currentType == RankType.CROSSSERVERWAR_PERSONAL)
        {
            view.Lbl_RefreshLabel.text = ConstString.RANK_LABEL_REFRESHREALTIME;
        }
        else if (_currentType == RankType.CAMPAIGN_PLAYER || _currentType == RankType.CAMPAIGN_ALL_PLAYER
            || _currentType == RankType.CAMPAIGN_UNION || _currentType == RankType.UNIONOVERLORD)
        {
            view.Lbl_RefreshLabel.text = ConfigManager.Instance.mCaptureTerritoryConfig.RankRefreshTime;//string.Format(ConstString.RANK_LABEL_REFRESHATTIME_CAMPAIGN, RankModule.Instance.refreshTimeStr_campaign);
        }
        else if (_currentType == RankType.CROSSSERVERWAR_UNION )
        {
            view.Lbl_RefreshLabel.text = ConfigManager.Instance.mCrossServerWarConfig.GetCrossServerWarSettingData().rank_update_time;
        }
        else
        {
            view.Lbl_RefreshLabel.text = string.Format(ConstString.RANK_LABEL_REFRESHATTIME, RankModule.Instance.refreshTimeStr);
        }
        //}

    }

    private IEnumerator UpdateRankContentInspector(List<RankInfo> list)
    {
        //if (list.Count < 1 || list == null)
        //    yield break;
        view.UIWrapContent_UIGrid.CleanChild();
        yield return null;
        int infoCount = list.Count;
        int itemCount = _itemList.Count;
        int index = Mathf.CeilToInt((float)infoCount / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        if (infoCount > MAXCOUNT)
        {
            view.UIWrapContent_UIGrid.enabled = true;
            infoCount = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > infoCount)
        {

            for (int i = 0; i < itemCount; i++)
            {
                _itemList[i].gameObject.SetActive(false);
            }

        }

        for (int i = 0; i < infoCount; i++)
        {
            if (itemCount <= i)
            {
                GameObject go = CommonFunction.InstantiateObject(view.RankItem_SelfRankItem.gameObject, view.Grd_UIGrid.transform);
                RankItem item = go.GetComponent<RankItem>();
                item.name = string.Format(ITEMNAME, i);
                item.IsSelf = false;
                UIEventListener.Get(go).onClick = ButtonEvent_RankItem;
                _itemList.Add(item);
            }
            else
            {
                _itemList[i].gameObject.SetActive(true);
            }
            _itemList[i].UpdateItem(_currentType, list[i]);
        }
        if (infoCount > (MAXCOUNT - 3))
            view.UIWrapContent_UIGrid.ReGetChild();
        yield return 0;
        view.Grd_UIGrid.repositionNow = true;
        yield return 0;
        view.ScrView_ContentScrollView.ResetPosition();
        view.Grd_UIGrid.repositionNow = true;
    }

    private void UpdateRankItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _currentList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        RankItem item = _itemList[wrapIndex];
        item.UpdateItem(_currentType, _currentList[realIndex]);
    }

    private void DestroyItemList()
    {
        if (_itemList == null || _itemList.Count < 1)
        {
            return;
        }
        for (int i = _itemList.Count - 1; i >= 0; i--)
        {
            RankItem item = _itemList[i];
            _itemList.RemoveAt(i);
            GameObject.Destroy(item.gameObject);
        }
        _itemList.Clear();
        view.UIWrapContent_UIGrid.ReGetChild();
    }

}
