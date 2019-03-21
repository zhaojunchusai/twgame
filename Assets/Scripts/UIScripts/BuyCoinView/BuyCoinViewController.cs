using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class BuyCoinViewController : UIBase 
{
    public BuyCoinView view;
    private int _exchangeGetCoins;      //点金手获得金币
    private int _exchangeNeedDiamond;   //点金手所需代币
    private int _exchangeCritical;      //点金手暴击倍数
    private int _exchangeRestTimes;     //点金手剩余次数
    private int _exchangeMaxTimes;      //点金手最大次数
    private int _exchangeWantedTimes;   //连续的兑换次数
    private int _exchangeBaseNum;       //点金手每次的金币基数
    private int _exchangeLastDiamond;   //记录上一次所需代币
    private float _intervalTime = 0.5f;
    private List<BuyCoinItem> _buyList;
    private List<BuyCoinResultItem> _buyResultList;
    private TouchGoldResp _tempInfo;         //玩家退出界面，就将没有加上的金币加上
    private int _buyCoinItemCount;
    private float _buyCoinItemHeight = 42f;
    private float _gridHeight = 50f;
    private bool _isUpdating;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new BuyCoinView();
            view.Initialize();
            BtnEventBinding();
        }
        UpdateBuyCoin();
        UpdateAllCostLabel();
        UpdateContinueLabel();
        _buyList = new List<BuyCoinItem>();
        _buyResultList = new List<BuyCoinResultItem>();
        _gridHeight = view.Grd_ExchangeGrid.cellHeight;
       
        //PlayOpenOffsetRootAnim();
    }

    public void UpdateBuyCoinResult(TouchGoldResp resp)
    {
        if (resp.touch_info == null || resp.touch_info.Count < 1)
            return;
        //ClearBuyItems();
        _tempInfo = resp;
        _buyCoinItemCount = _buyList.Count;
        Main.Instance.StartCoroutine(ShowResultInspector());
    }
    /// <summary>
    /// 更新点金手信息
    /// </summary>
    public void UpdateBuyCoin()
    {
        _exchangeMaxTimes = (int)ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv).ExchangeGoldCount;
        TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)PlayerData.Instance.ExchangeGoldTimes + 1);
        if (data == null) // ==null  即在表中找不到数据，可用于表示次数已达到最大上限//
        {
            _exchangeNeedDiamond = 0;
            _exchangeRestTimes = 0;
            _exchangeGetCoins = 0;
        }
        else
        {
            _exchangeNeedDiamond = data.ExchangeGold.Number;
            _exchangeRestTimes = _exchangeMaxTimes - PlayerData.Instance.ExchangeGoldTimes;
            _exchangeBaseNum = (int)ConfigManager.Instance.mExchangeGoldConfig.GetVipDataByLv(PlayerData.Instance._Level).BaseGold;
            _exchangeGetCoins = CommonFunction.GetExchangeCoins(GlobalConst.EXCHANGE_GOLD_INCREASE_FACTOR, PlayerData.Instance.ExchangeGoldTimes, _exchangeBaseNum);
        }
        UpdateDiamondLabel();
        UpdateGoldLabel();
        UpdateTodayRestLabel();
        UpdateTodayTotalLabel();
        UpdateAllCostLabel();
    }

    public void ButtonEvent_TopCloseButton(GameObject btn)
    {
        if (_isUpdating)
        {
            _isUpdating = false;
            TouchGoldOneInfo lastInfo = _tempInfo.touch_info[_tempInfo.touch_info.Count - 1];
            PlayerData.Instance.ExchangeGoldTimes = lastInfo.degree;
            PlayerData.Instance.UpdateGold(lastInfo.gold_item[0].num);
            PlayerData.Instance.UpdateDiamond(lastInfo.diamond_item[0].num);
        }
        ClearBuyItems();
        ClearBuyResultItems();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(BuyCoinView.UIName);
    }

    public void ButtonEvent_ContinueButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        _exchangeWantedTimes = GetContinueTimes();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (_exchangeRestTimes <= 0)
        {
            if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.ExchangeGold))
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,ConstString.EXCHANGE_GOLD_ALLUSEOUT);
            else
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.EXCHANGE_GOLD_GO_TO_VIP, ShowVipView);
                CommonFunction.ShowVipLvNotEnoughTip(ConstString.EXCHANGE_GOLD_GO_TO_VIP);
            }
            return;
        }
        int totalNeedDiamond = _exchangeNeedDiamond * _exchangeWantedTimes;
        if (totalNeedDiamond > PlayerData.Instance._Diamonds)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_TOP_DIAMONDLACK);
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, view._uiRoot.transform.parent.transform));
        MainCityModule.Instance.SendExchangeGold(_exchangeWantedTimes);
    }

    public void ButtonEvent_ExchangeButton(GameObject btn)
    {
        if (_isUpdating)
            return;
        _exchangeWantedTimes = 1;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (_exchangeRestTimes <= 0)
        {
            if (PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.ExchangeGold))
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.EXCHANGE_GOLD_ALLUSEOUT);
            else
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, ConstString.EXCHANGE_GOLD_GO_TO_VIP, ShowVipView);
            return;
        }
        if (_exchangeNeedDiamond > PlayerData.Instance._Diamonds)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_TOP_DIAMONDLACK);
            return;
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Get_Reward, view._uiRoot.transform.parent.transform));
        MainCityModule.Instance.SendExchangeGold(_exchangeWantedTimes);
    }

    public override void Uninitialize()
    {
        _isUpdating = false;
    }

    public override void Destroy()
    {
        _isUpdating = false;
        _exchangeGetCoins = 0;
        _exchangeNeedDiamond = 0;
        _exchangeCritical = 0;
        _exchangeRestTimes = 0;
        _exchangeMaxTimes = 0;
        _exchangeWantedTimes = 0;
        _exchangeLastDiamond = 0;
        _tempInfo = null;
        view = null;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Spt_MaskBGSprite.gameObject).onClick = ButtonEvent_TopCloseButton;
        UIEventListener.Get(view.Btn_ContinueButton.gameObject).onClick = ButtonEvent_ContinueButton;
        UIEventListener.Get(view.Btn_ExchangeButton.gameObject).onClick = ButtonEvent_ExchangeButton;
        PlayerData.Instance.NotifyResetEvent -= UpdateExchangeCount;
        PlayerData.Instance.NotifyResetEvent += UpdateExchangeCount;
    }

    private void ShowVipView()
    {
        UISystem.Instance.CloseGameUI(BuyCoinView.UIName);
        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
        UISystem.Instance.VipRechargeView.ShowVipPrivilege(PlayerData.Instance._VipLv + 1);
    }
    /// <summary>
    /// 更新代币数量
    /// </summary>
    private void UpdateDiamondLabel()
    {
        view.Lbl_DiamondNumLabel.text = _exchangeNeedDiamond.ToString();
    }
    /// <summary>
    /// 更新金币数量
    /// </summary>
    private void UpdateGoldLabel()
    {
        view.Lbl_CoinNumLabel.text =CommonFunction.GetTenThousandUnit((int)_exchangeGetCoins);
    }
    /// <summary>
    /// 更新已用次数
    /// </summary>
    private void UpdateTodayRestLabel()
    {
        view.Lbl_TodayRestNumLabel.text = string.Format(ConstString.FORMAT_BUY_COIN_TIMES, (_exchangeMaxTimes -_exchangeRestTimes));
    }
    /// <summary>
    /// 更新总次数
    /// </summary>
    private void UpdateTodayTotalLabel()
    {
        view.Lbl_TodayTotalNumLabel.text = _exchangeMaxTimes.ToString();
    }
    /// <summary>
    /// 更新上次兑换结果的统计
    /// </summary>
    private void UpdateAllCostLabel()
    {
        _exchangeLastDiamond = _exchangeNeedDiamond;
        view.Lbl_AllCostNumLabel.text = string.Format(ConstString.EXCHANGE_GOLD_ALLCOST, _exchangeLastDiamond);
    }
    /// <summary>
    /// 更新连续兑换的次数
    /// </summary>
    private void UpdateContinueLabel()
    {
        view.Lbl_BtnContinueButtonContinueTimesLabel.text = GetContinueTimes().ToString();
        view.Lbl_AllCostLabel.text = string.Format(ConstString.TIP_TIMES_COST, GetContinueTimes());
    }

    private IEnumerator ShowResultInspector()
    {
        if (_tempInfo == null)
            yield break;
        int count = _tempInfo.touch_info.Count;
        
        yield return null;
        _isUpdating = true;
        for (int i = 0; i < count; i++)
        {
            if (!_isUpdating)
                yield break;
            TouchGoldOneInfo info = _tempInfo.touch_info[i];
            if (info == null) {
                Debug.LogError("BuyCoinViewController ---------- ShowResultInspector inUpdating info is null i = " + i);
            }
            PlayerData.Instance.ExchangeGoldTimes = info.degree;
            if (info.gold_item == null)
            {
                Debug.LogError("BuyCoinViewController ---------- ShowResultInspector inUpdating info.gold_item is null i = " + i);
            }
            PlayerData.Instance.UpdateGold(info.gold_item[0].num);
            PlayerData.Instance.UpdateDiamond(info.diamond_item[0].num);
            if (info.gold_item == null)
            {
                Debug.LogError("BuyCoinViewController ---------- ShowResultInspector inUpdating info.diamond_item is null i = " + i);
            }
            //每次的表现
            GameObject buyCoinResultObj =CommonFunction.InstantiateObject(view.Obj_BuyCoinResultSource, view.Pnl_ExchangeResultPanel.transform);
            BuyCoinResultItem buyCoinResultItem = buyCoinResultObj.AddComponent<BuyCoinResultItem>();
            buyCoinResultItem.UpdateExchangeResult(info.gold_item[0].change_num, info.multiple,i);
            _buyResultList.Add(buyCoinResultItem);
            _buyCoinItemCount += 1;
            //每次的兑换记录
            GameObject buyCoinObj = CommonFunction.InstantiateObject(view.Obj_BuyCoinItemSource, view.Grd_ExchangeGrid.transform);
            BuyCoinItem buyCoinItem = buyCoinObj.AddComponent<BuyCoinItem>();
            buyCoinItem.UpdateBuyCoinInfo(_exchangeNeedDiamond, info.gold_item[0].change_num, info.multiple);
            _buyList.Add(buyCoinItem);
            view.Grd_ExchangeGrid.Reposition();
            if (i == 0)
            {
                view.ScrView_ExchangeScrollView.ResetPosition();
            }
            yield return new WaitForSeconds(0.2F);
            TryRollScrView(_buyCoinItemCount, _intervalTime * 0.9f);
            yield return new WaitForSeconds(_intervalTime);
            UpdateBuyCoin();
        }
        _isUpdating = false;
        UpdateContinueLabel();
    }

    private int GetContinueTimes()
    {
        int continueCount = ConfigManager.Instance.mTimesExpendConfig.GetNextExchageGoldCount(PlayerData.Instance.ExchangeGoldTimes, _exchangeNeedDiamond);
        continueCount = Mathf.Min(_exchangeRestTimes, continueCount);
        return continueCount;
    }

    private void ClearBuyItems()
    {
        if (_buyList == null || _buyList.Count < 1)
        {
            view.Grd_ExchangeGrid.Reposition();
            view.ScrView_ExchangeScrollView.ResetPosition();
            return;
        }
        for (int i = _buyList.Count - 1; i >= 0; i--)
        {
            BuyCoinItem item = _buyList[i];
            _buyList.Remove(item);
            item.gameObject.SetActive(false);
            GameObject.Destroy(item.gameObject);
        }
        _buyList.Clear();
        _buyCoinItemCount = 0;
        view.Grd_ExchangeGrid.Reposition();
        view.ScrView_ExchangeScrollView.ResetPosition();
    }

    private void ClearBuyResultItems()
    {
        if (_buyResultList == null || _buyResultList.Count < 1)
        {
            return;
        }
        for (int i = _buyResultList.Count - 1; i >= 0; i--)
        {
            BuyCoinResultItem item = _buyResultList[i];
            if (item == null)
                continue;
            _buyResultList.Remove(item);
            item.DestroyItem();
        }
        _buyResultList.Clear();
    }

    private void TryRollScrView(int itemCount, float time)
    {
        if (itemCount <= 3)
            return;
        float fromAmount = itemCount - 1 / itemCount;
        view.mRollSrollViewItem.Roll(fromAmount, 1f, time);
    }

    private void UpdateExchangeCount(NotifyReset data)
    {
        if (_isUpdating)
        {
            TouchGoldOneInfo lastInfo = _tempInfo.touch_info[_tempInfo.touch_info.Count - 1];
            lastInfo.degree = 0;
            PlayerData.Instance.ExchangeGoldTimes = 0;
        }
    }
    //界面动画
    //public void PlayOpenOffsetRootAnim()
    //{
    //    view.OffsetRoot_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.OffsetRoot_TScale.Restart();
    //    view.OffsetRoot_TScale.PlayForward();
    //}
}
