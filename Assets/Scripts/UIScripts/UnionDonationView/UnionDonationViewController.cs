using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class UnionDonationViewController : UIBase 
{
    public UnionDonationView view;
    private UnionDonateRate _nextDiamondDonateData;
    private UnionDonateRate _nextGoldDonateData;

    public override void Initialize()
    {
        if (view == null)
            view = new UnionDonationView();
        view.Initialize();
        BtnEventBinding();
        InitUI();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        _nextDiamondDonateData = null;
        _nextGoldDonateData = null;
    }
    public void InitUI()
    {
        RefreshDonateTimes();
    }

    public void RefreshDonateTimes()
    {
        view.Lbl_BtnDiamondTimesCount.text = string.Format(ConstString.FORMAT_UNION_DONATE_TIMES, UnionModule.Instance.CharUnionInfo.diamond_donate.times,
                                                                ConfigManager.Instance.mUnionConfig.GetUnionBaseData().
                                                                    mDiamondDonate.Count);
        view.Lbl_BtnGoldTimesCount.text = string.Format(ConstString.FORMAT_UNION_DONATE_TIMES, UnionModule.Instance.CharUnionInfo.gold_donate.times,
                                                                ConfigManager.Instance.mUnionConfig.GetUnionBaseData().
                                                                    mGoldDonate.Count);
        SetDonateCost();
    }

    public void SetDonateCost()
    {
        UnionBaseData baseData = ConfigManager.Instance.mUnionConfig.GetUnionBaseData();
        int diamondDonateTimes = UnionModule.Instance.CharUnionInfo.diamond_donate.times;
        int goldDonateTimes = UnionModule.Instance.CharUnionInfo.gold_donate.times;
        
        if (diamondDonateTimes >= baseData.mDiamondDonate.Count)
        {
            view.Lbl_BtnDiamondNum.text = "0";
            view.Lbl_BtnDiamondTokenNum.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_TOKEN,0);
            view.Lbl_BtnDiamondSupplyTip.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_SUPPLY,0);
            _nextDiamondDonateData = null;
        }
        else
        {
            _nextDiamondDonateData = baseData.mDiamondDonate[diamondDonateTimes];
            view.Lbl_BtnDiamondNum.text = _nextDiamondDonateData.Cost.ToString();
            view.Lbl_BtnDiamondTokenNum.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_TOKEN,
                                                          _nextDiamondDonateData.UnionToken);
            view.Lbl_BtnDiamondSupplyTip.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_SUPPLY,
                                                          _nextDiamondDonateData.SupplyNum);
        }

        if (goldDonateTimes >= baseData.mGoldDonate.Count)
        {
            view.Lbl_BtnGoldNum.text = "0";
            view.Lbl_BtnGoldTokenNum.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_TOKEN, 0);
            view.Lbl_BtnGoldSupplyTip.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_SUPPLY, 0);
            _nextGoldDonateData = null;
        }
        else
        {
            _nextGoldDonateData = baseData.mGoldDonate[goldDonateTimes];

            view.Lbl_BtnGoldNum.text = _nextGoldDonateData.Cost.ToString();
            view.Lbl_BtnGoldTokenNum.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_TOKEN,
                                                          _nextGoldDonateData.UnionToken);
            view.Lbl_BtnGoldSupplyTip.text = string.Format(ConstString.FORMAT_UNION_DONATE_GET_SUPPLY,
                                                          _nextGoldDonateData.SupplyNum);
        }
    }
    
    public void ButtonEvent_Gold(GameObject btn)
    {
        //if (_nextGoldDonateData == null)
        //{
        //    ErrorCode.ShowErrorTip((int)ErrorCodeEnum.MaxDonateTimes);
        //    return;
        //}
        //if (!CommonFunction.CheckMoneyEnough(ECurrencyType.Gold, (int)_nextGoldDonateData.Cost, true))
        //    return;
        UnionModule.Instance.OnSendDonateToUnion(UnionDonateType.UDT_GOLD);
    }

    public void ButtonEvent_Record(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(UnionDonationRecordView.UIName);
    }

    public void ButtonEvent_Diamond(GameObject btn)
    {
        //if (_nextDiamondDonateData == null)
        //{
        //    ErrorCode.ShowErrorTip((int)ErrorCodeEnum.MaxDonateTimes);
        //    return;
        //}
        //if (!CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond, (int)_nextDiamondDonateData.Cost, true))
        //    return;
        UnionModule.Instance.OnSendDonateToUnion(UnionDonateType.UDT_DIAMOND);
    }

    public void ButtonEvent_Mask(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(UnionDonationView.UIName);
    }

    public override void Uninitialize()
    {

    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Gold.gameObject).onClick = ButtonEvent_Gold;
        UIEventListener.Get(view.Btn_Record.gameObject).onClick = ButtonEvent_Record;
        UIEventListener.Get(view.Btn_Diamond.gameObject).onClick = ButtonEvent_Diamond;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = ButtonEvent_Mask;
    }
    //==============================================================================
    public GameObject Go_Effect;
    public void ShowEffect(bool IsGold)
    {
        if (Go_Effect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_UNIONDONATION, (GameObject gb) => { Go_Effect = gb; });
        }
        if(IsGold)
        {
            GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_Effect,view.Spt_BtnGoldIcon.transform);
        }
        else
        {
            GameObject itemEffect = ShowEffectManager.Instance.ShowEffect(Go_Effect, view.Spt_BtnDiamondIcon.transform);
        }
        view.Go_EffectMask.SetActive(true);
        Main.Instance.StartCoroutine(CloseEffectMask(1.0F));
    }
    public IEnumerator CloseEffectMask(float time)
    {
        yield return new WaitForSeconds(time);
        view.Go_EffectMask.SetActive(false);
    }

}
