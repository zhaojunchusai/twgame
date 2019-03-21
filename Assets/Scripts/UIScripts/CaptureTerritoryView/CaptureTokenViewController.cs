using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;
public class CaptureTokenViewController : UIBase 
{
    public CaptureTokenView view;

    private int[] _priceNum = new int[3];
    private UILabel[] _desc1 = new UILabel[3];
    private UILabel[] _desc2 = new UILabel[3];
    private UILabel[] _desc3 = new UILabel[3];
    private UILabel[] _price = new UILabel[3];
    private NationType _nationType;
    public override void Initialize()
    {
        if (view == null)
            view = new CaptureTokenView();
        view.Initialize();
        BtnEventBinding();
        Bindings();
        InitUI();
    }

    private void Bindings()
    {
        _desc1[0] = view.Lbl_LeftDesc1;
        _desc2[0] = view.Lbl_LeftDesc2;
        _desc3[0] = view.Lbl_LeftDesc3;
        _desc1[1] = view.Lbl_MidDesc1;
        _desc2[1] = view.Lbl_MidDesc2;
        _desc3[1] = view.Lbl_MidDesc3;
        _desc1[2] = view.Lbl_RightDesc1;
        _desc2[2] = view.Lbl_RightDesc2;
        _desc3[2] = view.Lbl_RightDesc3;
        _price[0] = view.Lbl_LeftPrice;
        _price[1] = view.Lbl_MidPrice;
        _price[2] = view.Lbl_RightPrice;
    }

    public void InitUI()
    {

        CaptureTokenData tokenData;
        TokenLevel tokenLv;

        for (int i = 0; i < 3; i++)
        {
            tokenData = ConfigManager.Instance.mCaptureTerritoryConfig.GetCaptureTokenDataByType((NationType)(i + 1));
            tokenLv = CaptureTerritoryModule.Instance.GetTokenLevel((NationType)(i + 1));
            if (tokenData == null || tokenLv == null)
            {
                Debug.LogError("ERROR: capture token error");
                continue;
            }

            _desc1[i].text = string.Format(tokenData.mTitleDesc, tokenData.GetDamageIncreaseByLv(tokenLv.level) *100);
            _desc2[i].text = string.Format(ConstString.FORMAT_TOKEN_LAYER, tokenLv.level, tokenData.mMaxLayerCount);     
            if (tokenLv.level >= tokenData.mMaxLayerCount)
            {
                //_priceNum[i] = 0;
                _desc3[i].text = ConstString.MAX_TOKEN_LAYER;
            }
            else
            {
                _desc3[i].text = string.Format(tokenData.mDesc, tokenData.GetDamageIncreaseByLv(tokenLv.level + 1) *100);
            }
            _priceNum[i] = tokenData.GetCostDiamond(tokenLv.level);
            _price[i].text = _priceNum[i].ToString();
        }
    }

    public void ShowEffect(NationType type)
    {
        switch (type)
        {
            case NationType.WEI:
                view.Effect1.ResetToBeginning();
                view.Effect1.loop = false;
                view.Effect1.gameObject.SetActive(true);
                Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseEffect);
                break;
            case NationType.SHU:
                view.Effect2.ResetToBeginning();
                view.Effect2.loop = false;
                view.Effect2.gameObject.SetActive(true);
                Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseEffect);
                break;
            case NationType.WU:
                view.Effect3.ResetToBeginning();
                view.Effect3.loop = false;
                view.Effect3.gameObject.SetActive(true);
                Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseEffect);
                break;
            default:
                break;
        }
        Assets.Script.Common.Scheduler.Instance.AddTimer(1.34f, false, CloseEffect);
    }

    private void CloseEffect()
    {
        view.Effect1.gameObject.SetActive(false);
        view.Effect2.gameObject.SetActive(false);
        view.Effect3.gameObject.SetActive(false);
    }

    private void BtnEvt_WEI(GameObject go)
    {
        if (CaptureTerritoryModule.Instance.GetTokenLevel(NationType.WEI).level
            >= ConfigManager.Instance.mCaptureTerritoryConfig.GetCaptureTokenDataByType(NationType.WEI).mMaxLayerCount)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TOKEN_MAX_LEVEL);
            return;
        }

        if (!CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond,_priceNum[0],true))
        {
            return;
        }
        
        _nationType = NationType.WEI;
        string str = string.Format(ConstString.TIP_BUY_TOKEN, _priceNum[0]);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, str, SendActivateToken);
    }
    private void BtnEvt_SHU(GameObject go)
    {
        if (CaptureTerritoryModule.Instance.GetTokenLevel(NationType.SHU).level
            >= ConfigManager.Instance.mCaptureTerritoryConfig.GetCaptureTokenDataByType(NationType.SHU).mMaxLayerCount)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TOKEN_MAX_LEVEL);
            return;
        }

        if (!CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond, _priceNum[1], true))
        {
            return;
        }

        _nationType = NationType.SHU;
        string str = string.Format(ConstString.TIP_BUY_TOKEN, _priceNum[1]);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, str, SendActivateToken);
    }
    private void BtnEvt_WU(GameObject go)
    {
        if (CaptureTerritoryModule.Instance.GetTokenLevel(NationType.WU).level
            >= ConfigManager.Instance.mCaptureTerritoryConfig.GetCaptureTokenDataByType(NationType.WU).mMaxLayerCount)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.TOKEN_MAX_LEVEL);
            return;
        }

        if (!CommonFunction.CheckMoneyEnough(ECurrencyType.Diamond, _priceNum[2], true))
        {
            return;
        }

        _nationType = NationType.WU;
        string str = string.Format(ConstString.TIP_BUY_TOKEN, _priceNum[2]);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, str, SendActivateToken);
    }

    private void SendActivateToken()
    {
        CaptureTerritoryModule.Instance.SendActivateToken(_nationType);
    }

    private void CloseUI(GameObject go)
    {
        UISystem.Instance.CloseGameUI(CaptureTokenView.UIName);
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
        UIEventListener.Get(view.Spt_LeftBG.gameObject).onClick = BtnEvt_WEI;
        UIEventListener.Get(view.Spt_MidBG.gameObject).onClick = BtnEvt_SHU;
        UIEventListener.Get(view.Spt_RightBG.gameObject).onClick = BtnEvt_WU;
        UIEventListener.Get(view.Spt_Mask.gameObject).onClick = CloseUI;        
    }
}
