using System.Collections.Generic;
using UnityEngine;
using fogs.proto.msg;
using Assets.Script.Common;

public class ItemSellViewController : UIBase
{
    private enum EViewState
    {
        MallBuy,
        BackpackSell,
        LifeSpiritSell,
    }

    private enum EDataResState
    {
        Item,
        Weapon,
        LifeSoul,
    }

    public ItemSellView view;

    private SliderChangeComponent sliderChangeComp;
    private Item itemPOD;
    private ItemInfo itemInfo;
    private Weapon weaponPOD;
    private LifeSoulData lifesoulData;

    private const int weaponCount = 1;
    private const int lifeSoulCount = 1;

    private System.Action callBack;

    private EViewState _viewState;
    private EDataResState _dataResState;
    private MallGoodsData _mallGoodsData;
    private bool _buyItemReachMax = false;
    /// <summary>
    /// 出售数量
    /// </summary>
    private int sellcount;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new ItemSellView();
            view.Initialize();
        }
        sliderChangeComp = view.Slider_SellSlider.GetComponent<SliderChangeComponent>();
        if (sliderChangeComp == null)
        {
            sliderChangeComp = view.Slider_SellSlider.gameObject.AddComponent<SliderChangeComponent>();
        }
        sliderChangeComp.OnSliderChange = SliderChange;
        BtnEventBinding(); //PlayOpenAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));

    }

    /// <summary>
    /// 商城使用
    /// </summary>
    public void UpdateViewInfo(MallGoodsData data)
    {
        SetViewState(EViewState.MallBuy, data.mCurrencyType);
        _mallGoodsData = data;
        callBack = null;
        _dataResState = EDataResState.Item;
        Item item = new Item();
        item.id = data.mGoodsID;
        item.num = ConfigManager.Instance.mItemData.GetItemInfoByID(data.mGoodsID).overlap_max
             - PlayerData.Instance.GetItemCountByID(data.mGoodsID);
        if (item.num <= 0)
        {
            item.num = 1;
            _buyItemReachMax = true;
        }
        else
        {
            _buyItemReachMax = false;
        }
        itemPOD = item;
        itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(data.mGoodsID);
        itemInfo.sell_price = (int)data.mPrice;
        view.Lbl_Desc.text = itemInfo.desc;

        view.Slider_SellSlider.numberOfSteps = itemPOD.num;
        view.Slider_SellSlider.value = 0f;
        sellcount = 1;

        CommonFunction.SetQualitySprite(view.Spt_SellItemQuality, itemInfo.quality, view.Spt_IconBGSprite);
        CommonFunction.SetSpriteName(view.Tex_SellItemIcon, itemInfo.icon);
        view.Spt_SellItemTypeMark.enabled = false;
        view.Lbl_HadItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, PlayerData.Instance.GetItemCountByID(data.mGoodsID));
        view.Lbl_SellItemName.text = itemInfo.name;
        view.Lbl_SellItemPrice.text = data.mPrice.ToString();
        view.Lbl_SellGetGold.text = (sellcount * data.mPrice).ToString();
        SetSellItemCount();
    }

    private void SetSellItemCount()
    {
        if (!_buyItemReachMax)
            view.Lbl_SellItemCount.text = sellcount.ToString();
        else
        {
            view.Lbl_SellItemCount.text = string.Format("[ff0000]{0}[-]", sellcount);
        }
    }

    /// <summary>
    /// 更新面板数据
    /// </summary>
    /// <param name="id">道具ID</param>
    /// <param name="count">道具数量</param>
    public void UpdateViewInfo(Item item, System.Action action)
    {
        _dataResState = EDataResState.Item;
        callBack = action;
        itemPOD = item;
        itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(itemPOD.id);
        if (item == null) return;
        SetViewState(EViewState.BackpackSell);
        weaponPOD = null;
        lifesoulData = null;
        view.Slider_SellSlider.numberOfSteps = itemPOD.num;
        view.Slider_SellSlider.value = 1f;
        sellcount = itemPOD.num;
        if (!CheckSellCount())
        {
            view.Slider_SellSlider.value = 0f;
        }
        CommonFunction.SetQualitySprite(view.Spt_SellItemQuality, itemInfo.quality, view.Spt_IconBGSprite);
        CommonFunction.SetSpriteName(view.Tex_SellItemIcon, itemInfo.icon);
        view.Spt_SellItemTypeMark.enabled = false;
        view.Lbl_HadItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, itemPOD.num);
        view.Lbl_SellItemName.text = itemInfo.name;
        view.Lbl_SellItemPrice.text = itemInfo.sell_price.ToString();
        SetSellItemCount();
    }

    public void UpdateViewInfo(Weapon pod, System.Action action)
    {
        weaponPOD = pod;
        _dataResState = EDataResState.Weapon;
        if (weaponPOD == null) return;
        SetViewState(EViewState.BackpackSell);
        callBack = action;
        itemInfo = null;
        itemPOD = null;
        lifesoulData = null;
        view.Slider_SellSlider.value = 1f;
        view.Slider_SellSlider.numberOfSteps = weaponCount;
        //sellcount = Mathf.CeilToInt(view.Slider_SellSlider.value * weaponCount);
        sellcount = weaponCount;
        if (!CheckSellCount())
        {
            view.Slider_SellSlider.value = 0f;
        }
        CommonFunction.SetQualitySprite(view.Spt_SellItemQuality, weaponPOD.Att.quality, view.Spt_IconBGSprite);
        CommonFunction.SetSpriteName(view.Tex_SellItemIcon, weaponPOD.Att.icon);
        view.Spt_SellItemTypeMark.enabled = false;
        view.Lbl_HadItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, weaponCount);
        view.Lbl_SellItemName.text = weaponPOD.Att.name;
        view.Lbl_SellItemPrice.text = weaponPOD.Att.price.ToString();
        SetSellItemCount();
    }

    public void UpdateViewInfo(LifeSoulData data, System.Action action)
    {
        lifesoulData = data;
        _dataResState = EDataResState.LifeSoul;
        if (lifesoulData == null)
            return;
        SetViewState(EViewState.LifeSpiritSell);
        callBack = action;
        itemInfo = null;
        itemPOD = null;
        weaponPOD = null;
        view.Slider_SellSlider.numberOfSteps = lifeSoulCount;
        view.Slider_SellSlider.value = 1f;
        sellcount = lifeSoulCount;
        if (!CheckSellCount())
        {
            view.Slider_SellSlider.value = 0f;
        }
        CommonFunction.SetQualitySprite(view.Spt_SellItemQuality, lifesoulData.SoulInfo.quality, view.Spt_IconBGSprite);
        CommonFunction.SetSpriteName(view.Tex_SellItemIcon, lifesoulData.SoulInfo.icon);
        CommonFunction.SetLifeSpiritTypeMark(view.Spt_SellItemTypeMark, lifesoulData.SoulInfo.godEquip);
        view.Lbl_HadItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, lifeSoulCount);
        view.Lbl_SellItemName.text = lifesoulData.SoulInfo.name;
        view.Lbl_SellItemPrice.text = lifesoulData.SoulInfo.sellPrice.Number.ToString();
        SetSellItemCount();
    }

    /// <summary>
    /// 根据UI用途（商城购买/背包出售）调整面板显示
    /// </summary>
    /// <param name="isBuy">是否是购买</param>
    private void SetViewState(EViewState type, ECurrencyType currencyType = ECurrencyType.Gold)
    {
        _viewState = type;
        switch (_viewState)
        {
            case EViewState.MallBuy:
                {
                    view.Lbl_Title.text = ConstString.TITLE_BUY;
                    view.Lbl_PriceTitle.text = ConstString.BUY_SINGLE_PRICE;
                    view.Lbl_CostTile.text = ConstString.COST_TIP;
                    view.Lbl_BtnConfirm.text = ConstString.CONFIRM_BUY;
                    view.Trans_SellGroup.localPosition = new Vector3(-88, -112, 0);
                    view.Gobj_DescGroup.SetActive(true);
                    CommonFunction.SetMoneyIcon(view.Spt_PriceIcon, currencyType);
                    CommonFunction.SetMoneyIcon(view.Spt_CostIcon, currencyType);
                } break;
            case EViewState.BackpackSell:
                {
                    view.Lbl_Title.text = ConstString.TITLE_SELL;
                    view.Lbl_PriceTitle.text = ConstString.SELL_SINGLE_PRICE;
                    view.Lbl_CostTile.text = ConstString.CAN_GET_TIP;
                    view.Lbl_BtnConfirm.text = ConstString.CONFIRM_SELL;
                    view.Trans_SellGroup.localPosition = new Vector3(-88, -60, 0);
                    view.Gobj_DescGroup.SetActive(false);
                    CommonFunction.SetMoneyIcon(view.Spt_PriceIcon, ECurrencyType.Gold);
                    CommonFunction.SetMoneyIcon(view.Spt_CostIcon, ECurrencyType.Gold);
                    _buyItemReachMax = false;
                } break;
            case EViewState.LifeSpiritSell:
                {
                    view.Lbl_Title.text = ConstString.TITLE_SELL;
                    view.Lbl_PriceTitle.text = ConstString.SELL_SINGLE_PRICE;
                    view.Lbl_CostTile.text = ConstString.CAN_GET_TIP;
                    view.Lbl_BtnConfirm.text = ConstString.CONFIRM_SELL;
                    view.Trans_SellGroup.localPosition = new Vector3(-88, -60, 0);
                    view.Gobj_DescGroup.SetActive(false);
                    CommonFunction.SetMoneyIcon(view.Spt_PriceIcon, lifesoulData.SoulInfo.sellPrice.Type);
                    CommonFunction.SetMoneyIcon(view.Spt_CostIcon, lifesoulData.SoulInfo.sellPrice.Type);
                } break;
        }
    }

    private void SliderChange()
    {
        if (view.Slider_SellSlider.value <= 0)
        {
            sellcount = 1;
        }
        else
        {
            switch (_dataResState)
            {
                case EDataResState.Item:
                    {
                        sellcount = Mathf.CeilToInt(view.Slider_SellSlider.value * (itemPOD.num));
                    } break;
                case EDataResState.Weapon:
                    {
                        sellcount = Mathf.CeilToInt(view.Slider_SellSlider.value * (weaponCount));
                    } break;
                case EDataResState.LifeSoul:
                    {
                        sellcount = Mathf.CeilToInt(view.Slider_SellSlider.value * (lifeSoulCount));
                    } break;
            }
        }
        CheckSellCount();
        UpdateSellCount();
    }

    public void ReceiveItemCell()
    {
        if (callBack != null)
        {
            callBack();
        }
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
    }


    private void UpdateSellCount()
    {
        switch (_dataResState)
        {
            case EDataResState.Item:
                {
                    if ((itemPOD.num <= 1) || (view.Slider_SellSlider.numberOfSteps <= 1))
                    {
                        view.Slider_SellSlider.value = 0f;
                    }
                    else
                    {
                        float step = (view.Slider_SellSlider.numberOfSteps > 1f) ? 1f / (view.Slider_SellSlider.numberOfSteps - 1) : 0.125f;
                        view.Slider_SellSlider.value = (sellcount - 1) * step;// / (float)(view.Slider_SellSlider.numberOfSteps + 1);
                    }
                    view.Lbl_SellGetGold.text = (sellcount * itemInfo.sell_price).ToString();
                } break;
            case EDataResState.Weapon:
                {
                    if ((weaponCount <= 1) || (view.Slider_SellSlider.numberOfSteps <= 1))
                    {
                        view.Slider_SellSlider.value = 0f;
                    }
                    else
                    {
                        float step = (view.Slider_SellSlider.numberOfSteps > 1f) ? 1f / (view.Slider_SellSlider.numberOfSteps - 1) : 0.125f;
                        view.Slider_SellSlider.value = (sellcount - 1) * step;/// (float)(view.Slider_SellSlider.numberOfSteps + 1);
                    }
                    view.Lbl_SellGetGold.text = (sellcount * weaponPOD.Att.price).ToString();
                } break;
            case EDataResState.LifeSoul:
                {
                    if ((lifeSoulCount <= 1) || (view.Slider_SellSlider.numberOfSteps <= 1))
                    {
                        view.Slider_SellSlider.value = 0f;
                    }
                    else
                    {
                        float step = (view.Slider_SellSlider.numberOfSteps > 1f) ? 1f / (view.Slider_SellSlider.numberOfSteps - 1) : 0.125f;
                        view.Slider_SellSlider.value = (sellcount - 1) * step;// / (float)(view.Slider_SellSlider.numberOfSteps + 1);
                    }
                    view.Lbl_SellGetGold.text = (sellcount * lifesoulData.SoulInfo.sellPrice.Number).ToString();
                } break;
        }
        SetSellItemCount();
    }

    private bool CheckSellCount()
    {
        switch (_dataResState)
        {
            case EDataResState.Item:
                {
                    if ((itemPOD.num >= 1) && (sellcount <= 0))
                    {
                        sellcount = 1;
                    }
                } break;
            case EDataResState.Weapon:
                {
                    if ((weaponCount >= 1) && (sellcount <= 0))
                    {
                        sellcount = 1;
                    }
                } break;
            case EDataResState.LifeSoul:
                {
                    if ((lifeSoulCount >= 1) && (sellcount <= 0))
                    {
                        sellcount = 1;
                    }
                } break;
        }
        if (sellcount <= 1) return false;
        return true;
    }

    /// <summary>
    /// 关闭面板
    /// </summary>
    public void ButtonEvent_CloseButton(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    }
    /// <summary>
    /// 确认出售
    /// </summary>
    public void ButtonEvent_ConfirmButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (_viewState == EViewState.MallBuy)
        {
            if (_buyItemReachMax)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TIP_BUY_ITEM_MAX);
                return;
            }

            if (!CommonFunction.CheckMoneyEnough(_mallGoodsData.mCurrencyType, (int)_mallGoodsData.mPrice * sellcount, true))
            {
                return;
            }

            StoreModule.Instance.SendMallBuy(_mallGoodsData.mID, sellcount);
        }
        else
        {
            if (sellcount == 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_SELECTITEM);
                return;
            }
            switch (_dataResState)
            {
                case EDataResState.Item:
                    {
                        BackPackModule.Instance.SendSellItem((uint)itemInfo.id, sellcount);
                    } break;
                case EDataResState.Weapon:
                    {
                        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent += SaleSoldierEquip;
                        PlayerData.Instance._SoldierEquip.Sale(weaponPOD.uId);
                    } break;
                case EDataResState.LifeSoul:
                    {
                        LifeSpiritModule.Instance.SendSellSoul(lifesoulData.SoulPOD.uid);
                    } break;
            }
        }
    }

    private void SaleSoldierEquip(EquipControl control, int errorCode)
    {
        if (control == EquipControl.SellEquipResp)
        {
            if (errorCode == 0)
            {
                PlayerData.Instance._SoldierEquip.ErrotDeleteEvent -= SaleSoldierEquip;
                if (callBack != null)
                    callBack();
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
            }
        }
    }

    private long PressEnterTime = 0;
    private long PressStartTime = 1;
    /// <summary>
    /// 增加出售数量
    /// </summary>
    public void ButtonEvent_AddPress(GameObject btn, bool state)
    {
        if (state)
        {
            PressEnterTime = Main.mTime;
            Scheduler.Instance.AddUpdator(AddCellCount);
        }
        else
        {
            PressEnterTime = 0;
            Scheduler.Instance.RemoveUpdator(AddCellCount);
        }
    }


    private void AddCellCount()
    {
        if (PressEnterTime + PressStartTime > Main.mTime)
        {
            return;
        }
        switch (_dataResState)
        {
            case EDataResState.Item:
                {
                    if (sellcount < itemPOD.num)
                    {
                        sellcount++;
                    }
                } break;
            case EDataResState.Weapon:
                {
                    if (sellcount < weaponCount)
                    {
                        sellcount++;
                    }
                } break;
            case EDataResState.LifeSoul:
                {
                    if (sellcount < lifeSoulCount)
                    {
                        sellcount++;
                    }
                } break;
        }
        UpdateSellCount();
    }

    private void ReduceCellCount()
    {
        if (PressEnterTime + PressStartTime > Main.mTime)
            return;
        if (sellcount > 1)
        {
            sellcount--;
        }
        else
        {
            return;
        }
        UpdateSellCount();
    }

    /// <summary>
    /// 减少出售数量
    /// </summary>
    public void ButtonEvent_ReducePress(GameObject btn, bool state)
    {

        if (state)
        {
            if (CheckSellCount())
            {
                PressEnterTime = Main.mTime;
                Scheduler.Instance.AddUpdator(ReduceCellCount);
            }
        }
        else
        {
            PressEnterTime = 0;
            Scheduler.Instance.RemoveUpdator(ReduceCellCount);
        }
    }

    /// <summary>
    /// 增加出售数量
    /// </summary>
    public void ButtonEvent_AddButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        AddCellCount();
    }
    /// <summary>
    /// 减少出售数量
    /// </summary>
    public void ButtonEvent_ReduceButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (CheckSellCount())
        {
            ReduceCellCount();
        }
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_ConfirmSell.gameObject).onClick = ButtonEvent_ConfirmButton;
        UIEventListener.Get(view.Btn_ColseItemSell.gameObject).onClick = ButtonEvent_CloseButton;
        UIEventListener.Get(view.Btn_ReduceButton.gameObject).onClick = ButtonEvent_ReduceButton;
        UIEventListener.Get(view.Btn_AddButton.gameObject).onClick = ButtonEvent_AddButton;
        UIEventListener.Get(view.Btn_AddButton.gameObject).onPress = ButtonEvent_AddPress;
        UIEventListener.Get(view.Btn_ReduceButton.gameObject).onPress = ButtonEvent_ReducePress;

    }

    public override void Uninitialize()
    {
        //base.Clear();
        view.Slider_SellSlider.value = 0;
        itemPOD = null;
        weaponPOD = null;
        sellcount = 0;
        view.Lbl_SellItemPrice.text = (0).ToString();
        Scheduler.Instance.RemoveUpdator(ReduceCellCount);
        Scheduler.Instance.RemoveUpdator(AddCellCount);
        //Lbl_GetCoinLabel.text = string.Empty;
        //cellcount = 0;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        sliderChangeComp = null;
    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();

    //}
}
