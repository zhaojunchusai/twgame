using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using fogs.proto.msg;



public class BuySPViewController : UIBase 
{
    public enum ConfirmState
    {
        Close,
        Buy,
        ToVip
    }

    public BuySPView view;
    private ConfirmState _confirmState = ConfirmState.Buy;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new BuySPView();
            view.Initialize();
        }
        BtnEventBinding();
        InitUI();
        //PlayOpenBuySPAnim();
    }
    public void InitUI()
    {
        uint maxCount = ConfigManager.Instance.mVipConfig.GetVipDataByLv(PlayerData.Instance._VipLv).EnergyBuyCount;
        
        if(PlayerData.Instance.BuySPTimes < maxCount)
        {
            int price = ConfigManager.Instance.mTimesExpendConfig
                .GetTimesExpendData((uint)PlayerData.Instance.BuySPTimes + 1).BuySP.Number;
            view.Lbl_Diamond.text = string.Format(ConstString.FORMAT_NUM_X, price);
            view.Lbl_Sp.text = string.Format(ConstString.FORMAT_NUM_X, GlobalCoefficient.BuySPCount);
            view.Lbl_Content.text = string.Format(ConstString.FORMAT_BUY_SP_TIP , PlayerData.Instance.BuySPTimes, maxCount);

            _confirmState = ConfirmState.Buy;
        }
        else
        {
            if(PlayerData.Instance._VipLv >= ConfigManager.Instance.mVipConfig.LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES.Energy))
            {
                view.Lbl_Content.text = string.Format(ConstString.NO_BUY_SP_TIMES, PlayerData.Instance.BuySPTimes, maxCount);
                _confirmState = ConfirmState.Close;
            }else
            {
                view.Lbl_Content.text = string.Format(ConstString.NO_BUY_SP_TIMES_TO_VIP, PlayerData.Instance.BuySPTimes, maxCount);
                _confirmState = ConfirmState.ToVip;
            }
        }
    }

    public void ButtonEvent_Confirm(GameObject btn)
    {
        switch (_confirmState)
        {
                case ConfirmState.Close:
                {
                    UISystem.Instance.CloseGameUI(BuySPView.UIName);
                    break;
                }
                case ConfirmState.Buy:
                {
                    MainCityModule.Instance.SendBuySP();
                    break;
                }
                case ConfirmState.ToVip:
                {
                    UISystem.Instance.CloseGameUI(BuySPView.UIName);
                    UISystem.Instance.ShowGameUI(VipRechargeView.UIName);
                    UISystem.Instance.VipRechargeView.ShowVipPrivilege(PlayerData.Instance._VipLv+1);
                    break;
                }

        }
    }

    public void ButtonEvent_Close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(BuySPView.UIName);
    }

    public override void Uninitialize()
    {

    }
    public override void Destroy()
    {
        view = null;
    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Confirm.gameObject).onClick = ButtonEvent_Confirm;
        UIEventListener.Get(view.Btn_Close.gameObject).onClick = ButtonEvent_Close;
    }

    //界面动画
    //public void PlayOpenBuySPAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();

    //}
}
