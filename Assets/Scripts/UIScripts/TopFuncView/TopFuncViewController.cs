using UnityEngine;
using System;
using System.Collections;
using Assets.Script.Common;
using fogs.proto.msg;

public class TopFuncViewController : UIBase
{
    public TopFuncView view;
    private TweenScale tscale, mainTscale;
    public bool isInit = false;
    public int OldGem, OldCoin, OldMaxSP, OldNowSP = 0;

    public override void Initialize()
    {

        if (view == null)
        {
            view = new TopFuncView();
            view.Initialize();
            InitCurrency();
        }
        view.Gobj_SPHint.SetActive(false);
        UpdatePlayerCurrency();
        PlayerData.Instance.UpdatePlayerDiamondEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerGoldEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerSPEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdateLevelEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerDiamondEvent += UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerGoldEvent += UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerSPEvent += UpdatePlayerCurrency;
        PlayerData.Instance.UpdateLevelEvent += UpdatePlayerCurrency;
        ShowIconBG();
        BtnEventBinding();
    }

    private void ButtonEvent_Coin(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.ExchangeGold))
            return;
        UISystem.Instance.ShowGameUI(BuyCoinView.UIName);
    }

    private void ButtonEvent_Gem(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckFuncIsOpen(OpenFunctionType.VIP))
            return;
        UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
        UISystem.Instance.VipRechargeView.ShowRecharge();
    }

    private void ButtonEvent_SPClick(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        CommonFunction.OpenBuySp();
    }

    private void ButtonEvent_SPPress(GameObject btn, bool ispress)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        view.Gobj_SPHint.SetActive(ispress);
        if (ispress)
        {
            Scheduler.Instance.AddUpdator(UpdateSpHintPanel);
        }
        else
        {
            Scheduler.Instance.RemoveUpdator(UpdateSpHintPanel);
        }
    }

    public void ShowIconBG()
    {
        int id = CommonFunction.FilterFramId((int)PlayerData.Instance.FrameID);
        string Frame_E = string.Format(GlobalConst.SpriteName.Frame_Name_E, id);
        CommonFunction.SetSpriteName(view.CoinBG, Frame_E);
        CommonFunction.SetSpriteName(view.GemBG, Frame_E);
        CommonFunction.SetSpriteName(view.SPBG, Frame_E);
    }
    private void UpdateSpHintPanel()
    {
        view.Lbl_NowTimeNum.text = CommonFunction.GetDateTimeString(Main.mTime);
        view.Lbl_RevertSPNum.text = CommonFunction.GetTimeString(MainCityModule.Instance.SPRecoverTimer
            + Mathf.Max(0, PlayerData.Instance.MaxPhysical - PlayerData.Instance._Physical - 1) * GlobalCoefficient.SPRecoverTimer);    //恢复全部体力
        view.Lbl_LastSPNum.text = CommonFunction.GetTimeString(MainCityModule.Instance.SPRecoverTimer);      //体力恢复
        view.Lbl_RevertTimeNum.text = string.Format(ConstString.FORMAT_MINUTE, GlobalCoefficient.SPRecoverTimer / 60);  //体力恢复间隔
        view.Lbl_BuySPNum.text = PlayerData.Instance.BuySPTimes.ToString();       //购买体力次数
    }

    /// <summary>
    /// 更新玩家货币类数据
    /// </summary>
    public void UpdatePlayerCurrency()
    {
        Scheduler.Instance.AddTimer(0.5f, false, UpdateCurrency);
        //GameActivityModule.Instance.TryToFreshActivityInfo();
    }
    private void UpdateCurrency()
    {
        if (view == null) return;
        if (PlayerData.Instance._Gold <= 1000000)
        {
            //Debug.LogError("PlayerData.Instance._Gold = " + PlayerData.Instance._Gold);
            //Debug.LogError("OldCoin = " + OldCoin);
            if (PlayerData.Instance._Gold != OldCoin)
                view.CoinAddValue.ShowAddValue(PlayerData.Instance._Gold, OldCoin);
            OldCoin = PlayerData.Instance._Gold;
        }
        else view.Lbl_BtnCoinLabel.text = CommonFunction.GetTenThousandUnit(PlayerData.Instance._Gold);

        if (PlayerData.Instance._Diamonds <= 1000000)
        {
            //Debug.LogError("PlayerData.Instance._Diamonds = "+PlayerData.Instance._Diamonds);
            //Debug.LogError("OldGem = " + OldGem);
            if (PlayerData.Instance._Diamonds != OldGem)
                view.GemAddValue.ShowAddValue(PlayerData.Instance._Diamonds, OldGem);
            OldGem = PlayerData.Instance._Diamonds;
        }
        else view.Lbl_BtnGemLabel.text = CommonFunction.GetTenThousandUnit(PlayerData.Instance._Diamonds);

        view.Lbl_BtnShowSPLabel.text = string.Format(ConstString.FORMAT_NUMBER_TOPFUNC, PlayerData.Instance.MaxPhysical);
        view.NowSPAddvalue.ShowAddValue(PlayerData.Instance._Physical, OldNowSP); OldNowSP = PlayerData.Instance._Physical;
        //view.Lbl_BtnShowNowSPLabel.text =PlayerData .Instance ._Physical .ToString();

    }
    private void InitCurrency()
    {
        OldCoin = PlayerData.Instance._Gold;
        OldGem = PlayerData.Instance._Diamonds;
        OldNowSP = PlayerData.Instance._Physical;
        OldMaxSP = PlayerData.Instance.MaxPhysical;

        view.Lbl_BtnCoinLabel.text = CommonFunction.GetTenThousandUnit(OldCoin);
        view.Lbl_BtnGemLabel.text = CommonFunction.GetTenThousandUnit(OldGem);
        view.Lbl_BtnShowNowSPLabel.text = OldNowSP.ToString();
        view.Lbl_BtnShowSPLabel.text = string.Format(ConstString.FORMAT_NUMBER_TOPFUNC, OldMaxSP);
    }

    public override void Uninitialize()
    {
        UnBtnEventBinding();
        Scheduler.Instance.RemoveTimer(UpdateCurrency);
        PlayerData.Instance.UpdatePlayerDiamondEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerGoldEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdatePlayerSPEvent -= UpdatePlayerCurrency;
        PlayerData.Instance.UpdateLevelEvent -= UpdatePlayerCurrency;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
    }

    private void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Coin.gameObject).onClick = ButtonEvent_Coin;
        UIEventListener.Get(view.Btn_Gem.gameObject).onClick = ButtonEvent_Gem;
        UIEventListener.Get(view.Btn_SP.gameObject).onClick = ButtonEvent_SPClick;
        UIEventListener.Get(view.Btn_BtnSPShowSP.gameObject).onPress = ButtonEvent_SPPress;//体力提示

    }


    private void UnBtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Coin.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_Gem.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_SP.gameObject).onClick = null;
        UIEventListener.Get(view.Btn_BtnSPShowSP.gameObject).onPress = null;//体力提示
    }

}

