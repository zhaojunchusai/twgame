using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

public class StoreViewController : UIBase 
{
    public StoreView view;
    public ShopType ShopType
    {
        get
        {
            return _curShop.shop_type;
        }
    }
    private List<StoreItem> _storeItems = new List<StoreItem>();
    private CommodityInfo _selectedCommodity;
    private StoreGoodsData _selectedCommodityData;
    private CommonItemData _selectedItemData = new CommonItemData();
    private ShopInfo _curShop;
    private List<GameObject> ItemPointList = new List<GameObject>();
    public GameObject Go_RefreashEffect;

    public override void Initialize()
    {
        if (view == null) 
        {
            view = new StoreView();
            view.Initialize();
            BtnEventBinding();
            InitItemPointTransfrom();
        }
       // PlayOpenAnim();
        UISystem.Instance.ShowGameUI(TopFuncView.UIName);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
    }
    //public void  PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart(); view.Anim_TScale.PlayForward();
    //}

    public void RefreshUI()
    {
        ShowStore(_curShop.shop_type);
    }

    public void ShowStore(ShopType type)
    {
        Debug.Log("open store ShopType = " + type.ToString());
        ShopInfo shop = null;
        if (PlayerData.Instance.ShopInfos.TryGetValue(type,out shop))
        {
            if (CheckNeedToRefresh((long)shop.next_update_time))
            {
                StoreModule.Instance.SendUpdateShopReq(UpdateType.UT_AUTO, type);
            }
            else
            {
                ShowStore(shop);
            }
            ShowOtherCurrency(type);
            SetTitle(type);
        }
        else
        {
            Debug.LogError("error shop = null");
            Close(null, null);
        }
    }

    private void SetTitle(ShopType type)
    {
        string name = "";
        switch (type)
        {
                case ShopType.ST_NomalShop:
                {
                    //name = GlobalConst.SpriteName.StoreNameNormal;
                    name = ConstString.NAME_NORMAL_STORE;
                    ShowTalk(NPCTalkType.StoreAccess);
                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenStoreView);
                    break;
                }
                case ShopType.ST_MedalShop:
                {
                    //name = GlobalConst.SpriteName.StoreNameMedal;
                    name = ConstString.NAME_MEDAL_STORE;
                    ShowTalk(NPCTalkType.MedalAccess);
                    break;
                }
                case ShopType.ST_HonorShop:
                {
                    //name = GlobalConst.SpriteName.StoreNameHonor;
                    name = ConstString.NAME_HONOR_STORE;
                    ShowTalk(NPCTalkType.HonorAccess);
                    break;
                }
                case ShopType.ST_UnionShop:
                {
                    //name = GlobalConst.SpriteName.StoreNameUnion;
                    name = ConstString.NAME_UNION_STORE;
                    ShowTalk(NPCTalkType.UnionAccess);
                    break;
                }
                case ShopType.ST_RecycleShop:
                {
                    //name = GlobalConst.SpriteName.StoreNameUnion;
                    name = ConstString.NAME_RECYCLE_STORE;
                    ShowTalk(NPCTalkType.RecycleAccess);
                    break;
                }
        }
        //CommonFunction.SetSpriteName(view.Spt_TitleBG, name);
        //view.Spt_TitleBG.MakePixelPerfect();
        view.Lbl_Title.text = name;

    }

    public void ShowStore(ShopInfo shop)
    {
        _curShop = shop;
        SetRefreshTime((long)shop.next_update_time);
        SetRefreshCostNum();
        ShowStoreItems(shop.commodity_info);
    }

    private bool CheckNeedToRefresh(long nextRefreshTime)
    {
        Debug.Log("CheckNeedToRefresh time=" + Main.mTime + " next refresh =" + nextRefreshTime);
        return Main.mTime >= nextRefreshTime;
    }

    private void SetRefreshTime(long nextRefreshTime)
    {
        System.DateTime time = System.DateTime.MinValue;
        System.DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime(new System.DateTime(1970, 1, 1));
        time = startTime.AddSeconds(nextRefreshTime);
        string str = string.Format(ConstString.FORMAT_TIME_NOHOURS, time.Hour, time.Minute);
        view.Lbl_RefreashTime.text = str;
    }

    private void ShowOtherCurrency(ShopType type )
    {
        bool state = false;
        if(type == ShopType.ST_MedalShop)
        {
            state = true;
            SetMedal();
            CommonFunction.SetSpriteName(view.Spt_OtherCurrencyIcon,GlobalConst.SpriteName.Medal);
            PlayerData.Instance.UpdateMedalEvent += SetMedal;
            view.Spt_OtherCurrencyIcon.width = 60;
            view.Spt_OtherCurrencyIcon.height = 60;
            view.Spt_OtherCurrencyIcon.transform.localPosition = new Vector3(-70, 10, 0);
            //CommonFunction.SetSpriteName(view.Spt_RefreshCostIcon, GlobalConst.SpriteName.Medal);
        }
        else if (type == ShopType.ST_HonorShop)
        {
            state = true;
            SetHonor();
            CommonFunction.SetSpriteName(view.Spt_OtherCurrencyIcon, GlobalConst.SpriteName.Honor);
            PlayerData.Instance.UpdateHonorEvent += SetHonor;
            view.Spt_OtherCurrencyIcon.width = 70;
            view.Spt_OtherCurrencyIcon.height = 70;
            view.Spt_OtherCurrencyIcon.transform.localPosition = new Vector3(-70,16,0);
            //CommonFunction.SetSpriteName(view.Spt_RefreshCostIcon, GlobalConst.SpriteName.Honor);
        }
        else if (type == ShopType.ST_UnionShop)
        {
            state = true;
            SetUnionToken();
            CommonFunction.SetSpriteName(view.Spt_OtherCurrencyIcon, GlobalConst.SpriteName.UnionToken);
            PlayerData.Instance.UpdateUnionTokenEvent += SetUnionToken;
            view.Spt_OtherCurrencyIcon.width = 60;
            view.Spt_OtherCurrencyIcon.height = 60;
            view.Spt_OtherCurrencyIcon.transform.localPosition = new Vector3(-70, 10, 0);
            //CommonFunction.SetSpriteName(view.Spt_RefreshCostIcon, GlobalConst.SpriteName.UnionToken);
        }
        else if (type == ShopType.ST_RecycleShop)
        {
            state = true;
            SetRecycleCoin();
            CommonFunction.SetSpriteName(view.Spt_OtherCurrencyIcon, GlobalConst.SpriteName.RecycleCoin);
            PlayerData.Instance.UpdateRecycleCoinEvent += SetRecycleCoin;
            view.Spt_OtherCurrencyIcon.width = 60;
            view.Spt_OtherCurrencyIcon.height = 60;
            view.Spt_OtherCurrencyIcon.transform.localPosition = new Vector3(-70, 10, 0);
            //CommonFunction.SetSpriteName(view.Spt_RefreshCostIcon, GlobalConst.SpriteName.RecycleCoin);
        }
        view.Lbl_OtherCurrencyNum.transform.parent.gameObject.SetActive(state);
        view.Lbl_OtherCurrencyNum.gameObject.SetActive(state);
        view.Spt_OtherCurrencyBG.gameObject.SetActive(state);
        view.Spt_OtherCurrencyIcon.gameObject.SetActive(state);
        view.Btn_OneKeyBuy.gameObject.SetActive(!state);
        if (type == ShopType.ST_NomalShop)
        {
            OpenLevelData data = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Store);
            if (data.vipLevel > PlayerData.Instance._VipLv)
            {
                view.Gobj_Lock.SetActive(true);
                view.Lbl_LockTip.text = string.Format(ConstString.STORE_LOCK_TIP, data.vipLevel);
                view.Btn_OneKeyBuy.gameObject.SetActive(false);
                view.Btn_Refreash.gameObject.SetActive(false);
            }
            else
            {
                view.Gobj_Lock.SetActive(false);
                view.Btn_OneKeyBuy.gameObject.SetActive(true);
                view.Btn_Refreash.gameObject.SetActive(true);
            }
        }
        else
        {
            view.Gobj_Lock.SetActive(false);
            view.Btn_Refreash.gameObject.SetActive(true);
        }
    }

    public void SetRefreshCostNum()
    {
        if (_curShop.shop_type == fogs.proto.msg.ShopType.ST_NomalShop)
        {
            view.Spt_RefreshCostIcon.gameObject.SetActive(false);
            return;        
        }

        VipData vip = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        if (_curShop.hand_update_num >= vip.StoreRefreshCount[_curShop.shop_type]
            && PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.StoreRefresh))
        {
            view.Spt_RefreshCostIcon.gameObject.SetActive(false);            
            return;
        }

        view.Spt_RefreshCostIcon.gameObject.SetActive(true);
        TimesExpendData times =
            ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)_curShop.hand_update_num + 1);
        CommonFunction.SetMoneyIcon(view.Spt_RefreshCostIcon, times.ShopRefresh[_curShop.shop_type].Type);
        view.Lbl_RefreshCostNum.text = string.Format(ConstString.FORMAT_NUM_X, times.ShopRefresh[_curShop.shop_type].Number);
    }

    private void SetMedal()
    {
        if (view != null && view.Lbl_OtherCurrencyNum != null)
            view.Lbl_OtherCurrencyNum.text = PlayerData.Instance._Medal.ToString();
    }

    private void SetHonor()
    {
        if (view != null && view.Lbl_OtherCurrencyNum != null)
            view.Lbl_OtherCurrencyNum.text = PlayerData.Instance._Honor.ToString();
    }

    private void SetUnionToken()
    {
        if (view != null && view.Lbl_OtherCurrencyNum != null)
            view.Lbl_OtherCurrencyNum.text = PlayerData.Instance.UnionToken.ToString();
    }
    private void SetRecycleCoin()
    {
        if (view != null && view.Lbl_OtherCurrencyNum != null)
            view.Lbl_OtherCurrencyNum.text = PlayerData.Instance.RecycleCoin.ToString();
    }

    private void ShowStoreItems(List<CommodityInfo> list)
    {
        view.ScrView_ItemPan.ResetPosition();
        for (int i = 0; i < list.Count; i++)
        {
            if(i >= _storeItems.Count)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_StoreItem, view.Grd_StoreItems.transform);
                _storeItems.Add(go.AddComponent<StoreItem>());
            }
            if (!_storeItems[i].gameObject.activeSelf)
                _storeItems[i].gameObject.SetActive(true);
            _storeItems[i].InitItem(list[i]);
        }

        for (int i = list.Count; i < _storeItems.Count; i++)
        {
            if (_storeItems[i].gameObject.activeSelf)
                _storeItems[i].gameObject.SetActive(false);
        }

        view.Grd_StoreItems.Reposition();
    }
    
    public void ShowStoreItemDetail(CommodityInfo info,StoreGoodsData data,CommonItemData commdata)
    {
        _selectedCommodity = info;
        _selectedCommodityData = data;
        _selectedItemData = commdata;
        view.Gobj_StoreItemDetail.gameObject.SetActive(true);
        CommonFunction.SetMoneyIcon(view.Spt_BuyMoneyIcon, data.CurrencyType);
        view.Lbl_BuyCount.text = info.num.ToString();
        view.Lbl_BuyPrice.text = (info.num * data.UnitPrice).ToString();

        view.Lbl_DescriptionMain.text = commdata.Desc;
        view.Lbl_PropName.text = commdata.Name;
        CommonFunction.SetSpriteName(view.Spt_PropIcon,commdata.Icon);
        CommonFunction.SetQualitySprite(view.Spt_Quality,commdata.Quality);
        //view.Spt_Mark
        CommonFunction.SetChipMark(view.Spt_Mark,commdata.SubType,new Vector3(-81,34,0),new Vector3(-81,38,0));
        if(data.PropType == GoodsConfigType.GCT_Item)
            view.Lbl_PropNumber.text = PlayerData.Instance.GetItemCountByID(data.PropID).ToString();
        else
        {
            view.Lbl_PropNumber.text = PlayerData.Instance._SoldierEquip.FindById(data.PropID).Count.ToString();
        }

    }

    public void CloseStoreItemDetail()
    {
        view.Gobj_StoreItemDetail.gameObject.SetActive(false);
    }

    private void ShowTalk(NPCTalkType type)
    {
        Scheduler.Instance.RemoveTimer(CloseTalk);
        view.Lbl_Talk.text = ConfigManager.Instance.mDialogueConfig.GetRandomString(type);
        view.Gobj_Talk.SetActive(true);
        Scheduler.Instance.AddTimer(5, false, CloseTalk);
    }

    private void CloseTalk()
    {
        Scheduler.Instance.RemoveTimer(CloseTalk);
        if (view != null && view.Gobj_Talk)
            view.Gobj_Talk.SetActive(false);
    }

    public void ButtonEvent_Npc(GameObject btn)
    {
        switch (_curShop.shop_type)
        {
            case ShopType.ST_NomalShop:
                {
                    ShowTalk(NPCTalkType.StoreClick);
                    break;
                }
            case ShopType.ST_MedalShop:
                {
                    ShowTalk(NPCTalkType.MedalClick);
                    break;
                }
            case ShopType.ST_HonorShop:
                {
                    ShowTalk(NPCTalkType.HonorClick);
                    break;
                }
            case ShopType.ST_UnionShop:
                {
                    ShowTalk(NPCTalkType.UnionClick);
                    break;
                }
            case ShopType.ST_RecycleShop:
                {
                    ShowTalk(NPCTalkType.RecycleClick);
                    break;
                }
        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(StoreView.UIName);
    }

    public void ButtonEvent_Buy(GameObject btn)
    {
        if (!CommonFunction.CheckMoneyEnough(_selectedCommodityData.CurrencyType, (int)(_selectedCommodity.num * _selectedCommodityData.UnitPrice),true))
        {
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        
        List<CommonItemData> data = new List<CommonItemData>();
        data.Add(_selectedItemData);
        if (CommonFunction.GetItemOverflowTip(data))
        {
            return;
        }

        StoreModule.Instance.SendBuyCommodityReq(_curShop.shop_type,_selectedCommodity.id);
    }

    public void ButtonEvent_CloseDetail(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        CloseStoreItemDetail();
    }

    public void ButtonEvent_Refreash(GameObject btn)
    {
        VipData vip = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv);
        if (vip == null)
        {
            Debug.LogError("vip is null :lv =" + PlayerData.Instance._VipLv);
            Debug.LogError("vip is null :shop =" + _curShop.shop_type.ToString());
            return;
        }
        //Debug.Log(_curShop.hand_update_num + " " + vip.StoreRefreshCount[_curShop.shop_type]);
        if(_curShop.hand_update_num >= vip.StoreRefreshCount[_curShop.shop_type])
        {

            if (PlayerData.Instance._VipLv < ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.StoreRefresh,_curShop.shop_type))
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.STORE_GO_TO_VIP, SeeVip);
                CommonFunction.ShowVipLvNotEnoughTip(ConstString.STORE_GO_TO_VIP);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_REFRESH_TIMES);
            }
            return;
        }

        string tip = "";
        TimesExpendData times =
            ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)_curShop.hand_update_num + 1);
        switch (times.ShopRefresh[_curShop.shop_type].Type)
        {
                case ECurrencyType.Diamond:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_DIAMOND,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
                case ECurrencyType.Gold:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_GOLD,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
                case ECurrencyType.Medal:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_MEDAL,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
                case ECurrencyType.Honor:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_HONOR,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
                case ECurrencyType.UnionToken:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_UNIONTOKEN,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
                case ECurrencyType.RecycleCoin:
                {
                    tip = string.Format(ConstString.FORMAT_REFRESH_TIP, ConstString.NAME_RECYCLECOIN,
                                        times.ShopRefresh[_curShop.shop_type].Number);
                    break;
                }
        }

        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, tip, RefreshStore);
        
    }
    private long _oneKeyNeedGold = 0;
    private long _oneKeyNeedDiamond = 0;
    private List<CommonItemData> _oneKeyBuyList = new List<CommonItemData>(); 
    public void ButtonEvent_OneKeyBuy(GameObject btn)
    {
        if(_curShop.shop_type != ShopType.ST_NomalShop)
        {
            view.Btn_OneKeyBuy.gameObject.SetActive(false);
            return;
        }
        _oneKeyNeedGold = 0;
        _oneKeyNeedDiamond = 0;
        _oneKeyBuyList.Clear();
        for (int i = 0; i < _curShop.commodity_info.Count; i++)
        {
            StoreGoodsData data =
                ConfigManager.Instance.mStoreGoodsConfig.GetStoreGoodsDataByID((uint)_curShop.commodity_info[i].id);
            if (_curShop.commodity_info[i].sell_out == (int)ECommodityState.Onsell && data.CurrencyType == ECurrencyType.Gold)
            {
                _oneKeyNeedGold += data.UnitPrice * _curShop.commodity_info[i].num;
                _oneKeyBuyList.Add(new CommonItemData(data.PropID, _curShop.commodity_info[i].num,true));
            }
            else if (_curShop.commodity_info[i].sell_out == (int)ECommodityState.Onsell && data.CurrencyType == ECurrencyType.Diamond)
            {
                _oneKeyNeedDiamond += data.UnitPrice * _curShop.commodity_info[i].num;
                _oneKeyBuyList.Add(new CommonItemData(data.PropID, _curShop.commodity_info[i].num, true));
            }
        }

        if(_oneKeyNeedGold >0 && _oneKeyNeedDiamond >0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
                string.Format(ConstString.STORE_ONE_KEY_BUY_TIP_GD, _oneKeyNeedGold, _oneKeyNeedDiamond)
                , OneKeyBuy);
        }
        else if (_oneKeyNeedGold > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
                string.Format(ConstString.STORE_ONE_KEY_BUY_TIP_GOLD, _oneKeyNeedGold)
                , OneKeyBuy);
        }
        else if (_oneKeyNeedDiamond > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo,
                string.Format(ConstString.STORE_ONE_KEY_BUY_TIP_DIAMOND, _oneKeyNeedDiamond)
                , OneKeyBuy);
        }
    }

    private void OneKeyBuy()
    {
        if(PlayerData.Instance._Diamonds < _oneKeyNeedDiamond)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,ConstString.HINT_NO_DIAMOND);
            return;
        }
        if (PlayerData.Instance._Gold < _oneKeyNeedGold)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_GOLD);
            return;
        }

        if (CommonFunction.GetItemOverflowTip(_oneKeyBuyList))
        {
            return;
        }

        StoreModule.Instance.SendOneKeyBuy();
    }

    private void SeeVip()
    {
        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
        UISystem.Instance.VipRechargeView.ShowVipPrivilege(PlayerData.Instance._VipLv +1);
    }

    private void RefreshStore()
    {
        TimesExpendData times =
            ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)_curShop.hand_update_num + 1);

        if (!CommonFunction.CheckMoneyEnough(times.ShopRefresh[_curShop.shop_type].Type, times.ShopRefresh[_curShop.shop_type].Number, true))
        {
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        StoreModule.Instance.SendUpdateShopReq(UpdateType.UT_COST, _curShop.shop_type);
    }

    public override void Uninitialize()
    {
        PlayerData.Instance.UpdateHonorEvent -= SetHonor;
        PlayerData.Instance.UpdateMedalEvent -= SetMedal;
        PlayerData.Instance.UpdateUnionTokenEvent -= SetUnionToken;
        PlayerData.Instance.UpdateRecycleCoinEvent -= SetRecycleCoin;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _storeItems.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
        UIEventListener.Get(view.Btn_Buy.gameObject).onClick = ButtonEvent_Buy;
        UIEventListener.Get(view.Btn_Refreash.gameObject).onClick = ButtonEvent_Refreash;
        UIEventListener.Get(view.Btn_CloseDetail.gameObject).onClick = ButtonEvent_CloseDetail;
        UIEventListener.Get(view.Spt_Decoration.gameObject).onClick = ButtonEvent_Npc;
        UIEventListener.Get(view.Btn_OneKeyBuy.gameObject).onClick = ButtonEvent_OneKeyBuy;
    }
    public void PlayStoreRefreashEffect()
    {
        view.ScrView_ItemPan.ResetPosition();
        view.ScrView_ItemPan.enabled = false;
        view.EffectMask.gameObject.SetActive(true);
        Main.Instance.StartCoroutine(OpenScrview(0.5F));

        int count = ItemPointList.Count;
        if(_curShop != null && _curShop.commodity_info != null)
            count = Mathf.Min(_curShop.commodity_info.Count, ItemPointList.Count);
        for (int i=0;i<count;i++)
        {
            if(Go_RefreashEffect==null)
            {
                ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_STOREREFRESH, (GameObject gb) => { Go_RefreashEffect = gb; });

            }
            GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_RefreashEffect, ItemPointList[i].transform);
        }
    }
    private IEnumerator OpenScrview(float time)
    {
        yield return new WaitForSeconds(time);
        view.ScrView_ItemPan.enabled = true;
        view.EffectMask.gameObject.SetActive(false );

    }
    private void InitItemPointTransfrom()
    {
        ItemPointList.Clear();
        ItemPointList.Add(view.ItemPoint1);
        ItemPointList.Add(view.ItemPoint5);
        ItemPointList.Add(view.ItemPoint2);
        ItemPointList.Add(view.ItemPoint6);
        ItemPointList.Add(view.ItemPoint3);
        ItemPointList.Add(view.ItemPoint7);
        ItemPointList.Add(view.ItemPoint4);
        ItemPointList.Add(view.ItemPoint8);
        //var GrdItems = view.Grd_StoreItems.GetChildList ();
        //for (int i=0;i<ItemPointList.Count;i++)
        //{
        //    ItemPointList[i].transform.position  = GrdItems[i].transform.position ;
        //}
    }

}
