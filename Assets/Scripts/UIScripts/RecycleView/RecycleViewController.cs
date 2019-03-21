using UnityEngine;
using System.Collections.Generic;
using System;
using System.Collections;
using fogs.proto.msg;

public class RecycleViewController : UIBase
{        
    public RecycleView view;
    private List<RankViewTypeBtn> _typeBtnList = new List<RankViewTypeBtn>();
    private Dictionary<uint, RankViewSubTypeObj> _subTypeObjDic = new Dictionary<uint, RankViewSubTypeObj>();
    private uint _selectedTypeID = 0;

    private ERecycleContentType _curRecycleType;
    private List<RecycleItem> _recycleItems = new List<RecycleItem>();
    private List<CommonItemData> _dataList = new List<CommonItemData>();
    private int _dataCount = 0;

    private List<uint>[] _choosedIds = new List<uint>[4];
    private List<UInt64>[] _choosedUids = new List<UInt64>[3];
    private ECurrencyType[] _priceType = new ECurrencyType[7];
    private int[] _sumPrice = new int[7];
    private bool _firstOpen = true;
    private bool _noRecycleTip = false;
    private int[] _purpleNum = new int[7];
    private bool[] _noPurpleTip = new bool[7];
    private const int ITEM_PER_PAGE = 35;
    private int _page = 1;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new RecycleView();
            view.Initialize();
            InitTypeBtns();
            for (int i = 0; i < _noPurpleTip.Length; i++)
            {
                _noPurpleTip[i] = false;
            }
            _noRecycleTip = false;
            //MonoBehaviour.Destroy(view.UIWrapContent_UIGrid);
        }
        view.Lbl_MoneyNum.text = "0";
        Assets.Script.Common.Scheduler.Instance.AddLateUpdator(UpdateTable);
        if (_firstOpen)
            ShowByType(ERecycleContentType.None);
        else
            RefreshUI();
        _firstOpen = false;
        SetRecycleMoney();
        UIEventListener.Get(view.Btn_Confirm).onClick = BtnEvnt_Confirm;
        UIEventListener.Get(view.Btn_OneKeyChoose).onClick = BtnEvnt_OneKeyChoose;
        UIEventListener.Get(view.Btn_Store).onClick = BtnEvnt_Store;
        UIEventListener.Get(view.MaskBGSprite).onClick = CloseUI;
        PlayerData.Instance.UpdateRecycleCoinEvent += SetRecycleMoney;
        view.ScrView_ContentScrollView.onDragFinishedDown = ReqNewPage;
    }
    public override void Uninitialize()
    {
        ClearChoosedList();
        Assets.Script.Common.Scheduler.Instance.RemoveLateUpdator(UpdateTable);
        UIEventListener.Get(view.Btn_Confirm).onClick = null;
        UIEventListener.Get(view.Btn_OneKeyChoose).onClick = null;
        UIEventListener.Get(view.Btn_Store).onClick = null;
        UIEventListener.Get(view.MaskBGSprite).onClick = null;
        PlayerData.Instance.UpdateRecycleCoinEvent -= SetRecycleMoney;
        for (int i = ITEM_PER_PAGE; i < _recycleItems.Count; i++)
        {
            _recycleItems[i].gameObject.SetActive(false);
        }
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _recycleItems.Clear();
        _dataList.Clear();
        _selectedTypeID = 0;
        _typeBtnList.Clear();
        _subTypeObjDic.Clear();
        _firstOpen = true;
    }
    private void SetRecycleMoney()
    {
        view.Lbl_RecycleMoneyNum.text = PlayerData.Instance.RecycleCoin.ToString();
    }
    void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(RecycleView.UIName);
    }
    public void RefreshUI()
    {
        ShowContent(_curRecycleType);
    }
    public void ShowEffs()
    {
        for (int i = 0; i < _dataCount; i++)
        {
            if (i <_recycleItems.Count)
            {
                _recycleItems[i].ShowEff();
            }
        }
    }
    #region 左边的类型处理代码
    private void UpdateTable()
    {
        view.UITable_UITable.Reposition();
    }
    private void InitTypeBtns()
    {
        List<RankviewData> list = ConfigManager.Instance.mRecycleViewConfig.DataList;

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
                sub.Init(view.Trans_SelectedSprite, view.Gobj_SubTypeBtn, item, ClickType,2);
                sub.transform.localScale = new Vector3(1, 0.001f, 1);
                sub.gameObject.SetActive(false);
                _subTypeObjDic.Add(item.ID, sub);
                btn.SubObj = sub;
            }
        }
        view.UITable_UITable.Reposition();
    }
    public void ShowByType(ERecycleContentType type)
    {
        ClickType(ConfigManager.Instance.mRecycleViewConfig.GetRankIDByType(type));
    }
    private void ClickType(uint id)
    {
        if (id == 0)
            return;
        RankviewData selectdata = ConfigManager.Instance.mRecycleViewConfig.GetRankviewDataByID(_selectedTypeID);
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
                ShowContent((ERecycleContentType)selectdata.Type);
            }
            return;
        }
        RankviewData newdata = ConfigManager.Instance.mRecycleViewConfig.GetRankviewDataByID(id);
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
            ShowContent((ERecycleContentType)newdata.Type);
    }
    #endregion

    private void ShowContent(ERecycleContentType type)
    {
        //view.UIWrapContent_UIGrid.onInitializeItem = SetSoldierInfo;
        _page = 1;
        int count = 0;
        switch (type)
        {
            case ERecycleContentType.Prop:
                {
                    List<Item> props = PlayerData.Instance._PropBag.FindAll((Item item) =>
                    {
                        return item != null && item.num > 0;
                    });
                    count = props.Count;
                    PushData(props);
                }
                break;
            case ERecycleContentType.Material:
                {
                    List<Item> mats = PlayerData.Instance._MaterialBag.FindAll((Item item) =>
                    {
                        return item != null && item.num > 0;
                    });
                    count = mats.Count;
                    PushData(mats);
                }
                break;
            case ERecycleContentType.EquipChip:
                {
                    List<Item> chips = PlayerData.Instance._ChipBag.FindAll((Item item) =>
                    {
                        return item != null && item.num > 0;
                    });
                    count = chips.Count;
                    PushData(chips);
                }
                break;
            case ERecycleContentType.SoldierChip:
                {
                    List<Debris> debris = new List<Debris>(PlayerData.Instance._SoldierDebrisDepot._SoldierDebrisList.FindAll((Debris d) =>
                    {
                        return d != null;
                    }));
                    count = debris.Count;
                    PushData(debris);
                }
                break;
            case ERecycleContentType.Equip:
                List<Weapon> equips = PlayerData.Instance._SoldierEquip.getWeaponList((Weapon wp) =>
                {
                    if (wp == null)
                        return false;
                    return !wp.isEquiped;
                });
                count = equips.Count;
                PushData(equips);
                break;
            case ERecycleContentType.Soldier:
                {
                    List<Soldier> soldiers = PlayerData.Instance._SoldierDepot.getSoldierList((Soldier sd) =>
                    {
                        if (sd == null)
                            return false;
                        return !CommonFunction.IsAlreadyBattle(sd.uId);
                    });
                    count = soldiers.Count;
                    PushData(soldiers);
                }
                break;
            case ERecycleContentType.LifeSoul:
                List<LifeSoulData> souls = PlayerData.Instance._LifeSoulDepot.GetPackLifeSoul();
                count = souls.Count;
                PushData(souls);
                break;
            default:
                break;
        }
        _curRecycleType = type;
        _dataCount = count;
        CommonFunction.SetMoneyIcon(view.Spt_MoneyIcon, ConfigManager.Instance.mRecycleViewConfig.GetCurrencyType((uint)type));
        UpdatePriceNum(0);//显示总价格
        _dataList.Sort(0, _dataCount,new CommonItemDataComparer());
        //Main.Instance.StartCoroutine(CreatSoldierItem(_dataList));
        //Main.Instance.StartCoroutine(CreateSoldierItemAll(_dataList));
        CreateSoldierItemByPage();
    }
    private void ReqNewPage()
    {
        if (ITEM_PER_PAGE * _page >= _dataCount)
        {
            return;
        }
        _page++;
        CreateSoldierItemByPage();
    }
    private void CreateSoldierItemByPage()
    {
        view.UIWrapContent_UIGrid.enabled = false;
        if(_page < 2)
            view.ScrView_ContentScrollView.ResetPosition();
        int startIndex = ITEM_PER_PAGE * (_page - 1);
        int pagecount = _page * ITEM_PER_PAGE;
        int count = _dataCount < pagecount ? _dataCount : pagecount;
        int itemCount = _recycleItems.Count;

        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                RecycleItem item = CommonFunction.InstantiateItem<RecycleItem>(view.Gobj_RecycleItem, view.Grd_UIGrid.transform);
                item.gameObject.name = i.ToString();
                item.gameObject.SetActive(true);
                _recycleItems.Add(item);
            }
            _recycleItems[i].gameObject.SetActive(true);
            _recycleItems[i].InitItem(_curRecycleType, _dataList[i], SelectedItem, GetItemChooseState);
        }

        for (int i = count; i < itemCount; i++)
        {
            _recycleItems[i].gameObject.SetActive(false);
        }
        view.Grd_UIGrid.Reposition();
    }
    private IEnumerator CreateSoldierItemAll(List<CommonItemData> data)
    {
        view.UIWrapContent_UIGrid.enabled = false;
        view.ScrView_ContentScrollView.ResetPosition();
        int count = _dataCount;
        int itemCount = _recycleItems.Count;
        int spawnCount = 10;
        int index = 1;
        
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                RecycleItem item = CommonFunction.InstantiateItem<RecycleItem>(view.Gobj_RecycleItem, view.Grd_UIGrid.transform);
                item.gameObject.name = i.ToString();
                item.gameObject.SetActive(true);
                _recycleItems.Add(item);
            }
            _recycleItems[i].gameObject.SetActive(true);
            _recycleItems[i].InitItem(_curRecycleType, data[i], SelectedItem, GetItemChooseState);
            index++;
            if (index % spawnCount == 0)
            {
                view.Grd_UIGrid.Reposition();
                yield return 0;
            }
        }

        for (int i = count; i < itemCount; i++)
        {
            _recycleItems[i].gameObject.SetActive(false);
            index++;
            if (index % spawnCount == 0)
            {
                view.Grd_UIGrid.Reposition();
                yield return 0;
            }
        }
        view.Grd_UIGrid.Reposition();
    }
    private IEnumerator CreatSoldierItem(List<CommonItemData> data)
    {
        yield return 0;
        view.ScrView_ContentScrollView.ResetPosition();
        yield return 0;
        view.Grd_UIGrid.repositionNow = true;
        yield return 0;
        view.UIWrapContent_UIGrid.CleanChild();
        yield return 0;

        int count = _dataCount;
        int itemCount = _recycleItems.Count;
        int MaxCount = 20;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_UIGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_UIGrid.minIndex = -index;
        view.UIWrapContent_UIGrid.maxIndex = 0;
        this.view.UIWrapContent_UIGrid.cullContent = true;
        if (count % 5 != 0)
        {
            this.view.UIWrapContent_UIGrid.cullContent = false;
        }
        else
        {
            this.view.UIWrapContent_UIGrid.cullContent = true;
        }
        if (count > MaxCount)
        {
            view.UIWrapContent_UIGrid.enabled = true;            
            count = MaxCount;
        }
        else
        {
            view.UIWrapContent_UIGrid.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _recycleItems[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_RecycleItem, view.Grd_UIGrid.transform);
                RecycleItem item = vGo.GetComponent<RecycleItem>();
                if (item == null)
                {
                    item = vGo.AddComponent<RecycleItem>();
                }
                _recycleItems.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
            }
            else
            {
                if (_recycleItems[i] != null)
                    _recycleItems[i].gameObject.SetActive(true);
            }
            _recycleItems[i].InitItem(_curRecycleType, data[i], SelectedItem, GetItemChooseState);
        }
        yield return 0;
        view.UIWrapContent_UIGrid.ReGetChild();
        yield return 0;
        view.Grd_UIGrid.Reposition();        
        yield return 0;
        view.ScrView_ContentScrollView.ResetPosition();
        yield return 0;
        view.Grd_UIGrid.repositionNow = true;
        this.view.Grd_UIGrid.gameObject.SetActive(false);
        this.view.Grd_UIGrid.gameObject.SetActive(true);
    }
    public void SetSoldierInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= _dataCount)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        RecycleItem item = _recycleItems[wrapIndex];
        item.InitItem(_curRecycleType, _dataList[realIndex], SelectedItem, GetItemChooseState);        
    }
    public bool GetItemChooseState(object obj)
    {
        if ((int)_curRecycleType < (int)ERecycleContentType.Equip)
        {
            List<uint> list = _choosedIds[(int)_curRecycleType - (int)ERecycleContentType.Prop];
            if (list == null) return false;
            return list.Contains((uint)obj);
        }
        else
        {
            List<UInt64> list = _choosedUids[(int)_curRecycleType - (int)ERecycleContentType.Equip];
            if (list == null) return false;
            return list.Contains((UInt64)obj);
        }
    }
    public void SelectedItem(object id,object price,int num,bool highQuality)
    {
        if ((int)_curRecycleType < (int)ERecycleContentType.Equip)
        {
            UpdataChoosedList(ref _choosedIds[(int)_curRecycleType - (int)ERecycleContentType.Prop],
                (uint)id, (MoneyFlowData)price, num, highQuality);
        }
        else
        {
            UpdataChoosedList(ref _choosedUids[(int)_curRecycleType - (int)ERecycleContentType.Equip],
                (UInt64)id, (MoneyFlowData)price, num, highQuality);
        }
    }
    private void UpdataChoosedList<T>(ref List<T> list, T item, MoneyFlowData money, int num, bool highQuality)
    {
        if (list == null)
        {
            list = new List<T>();
        }
        CommonFunction.SetMoneyIcon(view.Spt_MoneyIcon, money.Type);
        if (list.Contains(item))
        {
            list.Remove(item);
            UpdatePriceNum(-money.Number *num);
            UpdateQualityNum(highQuality ? -1 : 0);
        }
        else
        {
            list.Add(item);
            UpdatePriceNum(money.Number*num);
            UpdateQualityNum(highQuality ? 1 : 0);
        }
    }
    private void UpdateQualityNum(int num)
    {
        _purpleNum[(int)_curRecycleType - 1] += num;
    }
    private void UpdatePriceNum(int price)
    {
        _sumPrice[(int)_curRecycleType - 1] += price;
        if (_sumPrice[(int)_curRecycleType - 1] < 0)
        {
            _sumPrice[(int)_curRecycleType - 1] = 0;
        }
        view.Lbl_MoneyNum.text = _sumPrice[(int)_curRecycleType - 1].ToString();
    }
    private void ClearChoosedList()
    {
        for (int i = 0; i < _choosedIds.Length; i++)
        {
            if (_choosedIds[i] != null)
                _choosedIds[i].Clear();
        }
        for (int i = 0; i < _choosedUids.Length; i++)
        {
            if (_choosedUids[i] != null)
                _choosedUids[i].Clear();
        }
        for (int i = 0; i < _sumPrice.Length; i++)
        {
            _sumPrice[i] = 0;
        }
    }

    public void ClearChoosedList(ERecycleContentType type)
    {
        if ((int)_curRecycleType < (int)ERecycleContentType.Equip)
        {
            _choosedIds[(int)_curRecycleType - (int)ERecycleContentType.Prop].Clear();
        }
        else
        {
            _choosedUids[(int)_curRecycleType - (int)ERecycleContentType.Equip].Clear();
        }
        _sumPrice[(int)type -1] = 0;
        _purpleNum[(int)type - 1] = 0;
    }
    private List<RecycleItem> GetVisibleItems()
    {
        List<RecycleItem> result = new List<RecycleItem>();

        float y = view.ScrView_ContentScrollView.transform.localPosition.y;
        // 290为裁剪区域高   136为item竖直间距  15为初始时ScrView_ContentScrollView的Y值
        int row = Mathf.CeilToInt((float)(290 + y - 15) / 136); 
        float offsetY = (290 + y - 15) % 136;
        if (offsetY < 20) //实际测量值  当最后一行可见高度<34的时候，row 行不加入一键选择范围
        {
            result.AddRange(GetVisibleItemsByRow(row - 1));
            result.AddRange(GetVisibleItemsByRow(row - 2));
        }
        else if (offsetY > 50)//实际测量值  当最后一行可见高度>94的时候，row-2 行不加入一键选择范围
        {
            result.AddRange(GetVisibleItemsByRow(row));
            result.AddRange(GetVisibleItemsByRow(row - 1));
        }
        else //一键选择 row ,row -1 ,row -2行
        {
            result.AddRange(GetVisibleItemsByRow(row));
            result.AddRange(GetVisibleItemsByRow(row - 1));
            result.AddRange(GetVisibleItemsByRow(row - 2));
        }
        return result;
    }
    private List<RecycleItem> GetVisibleItemsByRow(int row)
    {
        List<RecycleItem> res = new List<RecycleItem>();
        int n = row - 1;//        (row - 1) % 4;//20个item共4行
        for (int i = n * 5 ; i < (n + 1) * 5; i++)
        {
            if (_recycleItems.Count > i && _recycleItems[i].gameObject.activeInHierarchy)
            {
                res.Add(_recycleItems[i]);
            }
        }
        return res;
    }
    private void PushData(List<Soldier> soldiers)
    {
        for (int i = 0,j = 0; i < soldiers.Count; i++)
        {
            if (soldiers[i] == null)
                continue;

            if (j < _dataList.Count)
            {
                _dataList[j].SetValue(soldiers[i]);
            }
            else
            {
                _dataList.Add(new CommonItemData(soldiers[i]));
            }
            _dataList[j].Num = 1;
            j++;
        }
    }
    private void PushData(List<Item> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (i < _dataList.Count)
            {
                _dataList[i].SetValue(data[i].id, data[i].num, true);
            }
            else
            {
                _dataList.Add(new CommonItemData(data[i].id,data[i].num,true));
            }
        }
    }
    private void PushData(List<Weapon> data)
    {
        for (int i = 0,j=0; i < data.Count; i++)
        {
            if (data[i] == null)
                continue;
            if (j < _dataList.Count)
            {
                _dataList[j].SetValue(data[i]);
            }
            else
            {
                _dataList.Add(new CommonItemData(data[i]));
            }
            _dataList[j].Num = 1;
            j++;
        }
    }
    private void PushData(List<LifeSoulData> data)
    {
        for (int i = 0; i < data.Count; i++)
        {
            if (i < _dataList.Count)
            {
                _dataList[i].SetValue(data[i]);
            }
            else
            {
                _dataList.Add(new CommonItemData(data[i]));
            }
            _dataList[i].Num = 1;
        }
    }
    private void PushData(List<Debris> data)
    {
        for (int i = 0,j=0; i < data.Count; i++)
        {
            if (i < _dataList.Count)
            {
                _dataList[i].SetValue(data[i].Att);
                _dataList[i].Num = data[i].count;
            }
            else
            {
                _dataList.Add(new CommonItemData(data[i].Att));
                _dataList[i].Num = data[i].count;
            }
        }
    }
    private void BtnEvnt_OneKeyChoose(GameObject go)
    {
        List<RecycleItem> list = GetVisibleItems();
        for (int i = 0; i < list.Count; i++)
        {
            list[i].ChooseItem();
        }
    }
    private void BtnEvnt_Confirm(GameObject go)
    {
        if (_sumPrice[(int)_curRecycleType - 1] <= 0)
            return;
        if (!_noPurpleTip[(int)_curRecycleType -1] && _purpleNum[(int)_curRecycleType -1]>0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark,
            string.Format(ConstString.TIP_RECYCLE_QUALITY_CONFIRM,
            ConfigManager.Instance.mRecycleViewConfig.GetRankviewDataByID(_selectedTypeID).Name),
            CheckRecycleTip, null, "", "", (bool b) =>
            {
                _noPurpleTip[(int)_curRecycleType - 1] = b;
            }, false);
            return;
        }
        CheckRecycleTip();        
    }
    private void CheckRecycleTip()
    {
        if (!_noRecycleTip)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark,
            string.Format(ConstString.TIP_RECYCLE_CONFIRM, _sumPrice[(int)_curRecycleType - 1]),
            SendSell, null, "", "", (bool b) =>
            {
                _noRecycleTip = b;
            }, false);
            return;
        }
        SendSell();
    }
    private void SendSell()
    {
        if (CommonFunction.CheckMoneyOverflow(ECurrencyType.RecycleCoin, _sumPrice[(int)_curRecycleType - 1]))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_RECYCLE_OVERFLOW);
            return;
        }

        if ((int)_curRecycleType < (int)ERecycleContentType.Equip)
        {
            StoreModule.Instance.SendRecycleSell(_curRecycleType, _choosedIds[(int)_curRecycleType - (int)ERecycleContentType.Prop]);
        }
        else
        {
            StoreModule.Instance.SendRecycleSell(_curRecycleType, _choosedUids[(int)_curRecycleType - (int)ERecycleContentType.Equip]);
        }   
    }
    private void BtnEvnt_Store(GameObject go)
    {
        UISystem.Instance.ShowGameUI(StoreView.UIName);
        UISystem.Instance.StoreView.ShowStore(ShopType.ST_RecycleShop);
    }
}
