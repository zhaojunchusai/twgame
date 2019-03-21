using UnityEngine;
using System;
using System.Collections;

public class BuyCoinItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_DiamondSprite;
    [HideInInspector]public UILabel Lbl_CostNumLabel;
    [HideInInspector]public UISprite Spt_CoinSprite;
    [HideInInspector]public UILabel Lbl_GetCoinNumLabel;
    [HideInInspector]public UILabel Lbl_CritNumLabel;
    private bool _isInit;

    public void Initialize()
    {
        if (_isInit)
            return;

        Spt_DiamondSprite = transform.FindChild("DiamondSprite").gameObject.GetComponent<UISprite>();
        Lbl_CostNumLabel = transform.FindChild("CostNumLabel").gameObject.GetComponent<UILabel>();
        Spt_CoinSprite = transform.FindChild("CoinSprite").gameObject.GetComponent<UISprite>();
        Lbl_GetCoinNumLabel = transform.FindChild("GetCoinNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_CritNumLabel = transform.FindChild("CritNumLabel").gameObject.GetComponent<UILabel>();
        SetLabelValues();
        _isInit = true;

    }

    void Awake()
    {
        Initialize(); 
    }

    public void SetLabelValues()
    {

        Lbl_CostNumLabel.text = "";
        Lbl_GetCoinNumLabel.text = "";
        Lbl_CritNumLabel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void UpdateBuyCoinInfo(int costNum, int getCoinNum, int critNum)
    {
        if (!_isInit)
            Initialize();
        Lbl_CostNumLabel.text = string.Format(ConstString.EXCHANGE_GOLD_COSTDIAMOND,costNum);
        Lbl_GetCoinNumLabel.text = getCoinNum.ToString();
        if (critNum <= 1)
        {
            Lbl_CritNumLabel.text ="";
        }
        else
        {
            Lbl_CritNumLabel.text = string.Format(ConstString.EXCHANGE_GOLD_CRITTIMES, critNum);
        }
    }


}
