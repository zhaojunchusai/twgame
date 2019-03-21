using UnityEngine;
using System.Collections;

public class TalkingDataManager : Singleton<TalkingDataManager>
{
    private string _appID = "8AD8977DD0BE57F5AA68D8BE2F27D7E4"; 
    private TDGAAccount _account;

    public void Initialize()
    {
        TalkingDataGA.OnStart(_appID, GlobalConst.PLATFORM.ToString());
    }

    public void SetAccount(string sAccount) 
    {
        _account = TDGAAccount.SetAccount(sAccount);
    }

    public void SetAccountType(AccountType type) 
    {
        if (_account != null)
            _account.SetAccountType(type);
    }

    public void SetAccountName(string sAccountName) 
    {
        if (_account != null)
            _account.SetAccountName(sAccountName);
    }

    public void SetAccountLevel(int iLevel) 
    {
        if (_account != null)
            _account.SetLevel(iLevel);
    }

    public void SetAccountGender(Gender vGender) 
    {
        if (_account != null)
            _account.SetGender(vGender);
    }

    public void SetAccountAge(int iAge) 
    {
        if (_account != null)
            _account.SetAge(iAge);
    }

    public void SetAccountGameServer(string sGameServer) 
    {
        if (_account != null)
            _account.SetGameServer(sGameServer);
    }

    public void OnChargeRequest(string  sOrderId, string sIapId, double dCurrencyAmount,string sCurrencyType,double dVirtualCurrencyAmout,string sPaymentType)
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAVirtualCurrency.OnChargeRequest(sOrderId, sIapId, dCurrencyAmount, sCurrencyType, dVirtualCurrencyAmout, sPaymentType);
        }
    }

    public void OnChargeSuccess(string orderId) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAVirtualCurrency.OnChargeSuccess(orderId);
        }
    }

    public void OnReward(double dVirtualCurrencyAmount,string sReason ) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAVirtualCurrency.OnReward(dVirtualCurrencyAmount, sReason);
        }
    }

    public void OnPurchase(string item,int itemNum,double preceInVirtualCurrency) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAItem.OnPurchase(item, itemNum, preceInVirtualCurrency);
        }
    }

    public void OnUse(string item,int itemNum) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAItem.OnUse(item, itemNum);
        }
    }

    public void OnBegin(string missionID) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAMission.OnBegin(missionID);
        }
    }

    public void OnCompleted(string missionId) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TDGAMission.OnCompleted(missionId);
        }
    }

    public void OnFailed(string missionId,string cause) 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor) 
        {
            TDGAMission.OnFailed(missionId, cause);
        }
        
    }

    public void OnKill() 
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TalkingDataGA.OnKill();
        }
    }

    public void Uninitialize()
    {
        if (Application.platform != RuntimePlatform.OSXEditor && Application.platform != RuntimePlatform.WindowsEditor)
        {
            TalkingDataGA.OnEnd();
        }
    }
	
}
