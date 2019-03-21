using UnityEngine;
using System;
using System.Collections.Generic;

public class QuickMatItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UILabel Lbl_Cost;
    private bool _initialized = false;
    private MoneyFlowData _money = new MoneyFlowData();
    private bool _ignoreTip = false;
    private Action _tipYesCallBack = null;
    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }
        _initialized = true;
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Spt_Icon = transform.FindChild("Icon").gameObject.GetComponent<UISprite>();
        Lbl_Cost = transform.FindChild("Cost").gameObject.GetComponent<UILabel>();
        SetLabelValues();

    }

    public void Clear()
    {
        _money.Reset();
    }

    /// <summary>
    /// 英雄和武将升级技能用
    /// </summary>
    public void Init(ItemInfo item, int hasNum, int needNum)
    {
        Initialize();
        _money.Reset();
        if (hasNum >= needNum)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        _money.Type = item.price.Type;
        _money.Number = (needNum - hasNum) * item.price.Number;
        SetMoneyData();
    }

    /// <summary>
    /// 神器强化界面用
    /// </summary>
    /// <param name="data"></param>
    public void Init(List<ArtifactIntensifyViewController.IntensifyMaterialBag> data)
    {
        Initialize();
        _money.Reset();
        foreach (var item in data)
        {
            if (item.needNum > item.item.num)
            {
                ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.item.id);
                if (itemInfo == null)
                    return;                     
                if(itemInfo.price.Type != _money.Type)
                {
                    if (_money.Type == ECurrencyType.None)
                    {
                        _money.Type = itemInfo.price.Type;
                    }
                    else
                    {
                        gameObject.SetActive(false);
                        Debug.Log("ERROR :Quick Mat Item,MoneyType not same");
                        return;
                    }                    
                }
                _money.Number += itemInfo.price.Number * (item.needNum - item.item.num);                
            }
        }
        if (_money.Number <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);        
        SetMoneyData();
    }

    private void SetMoneyData()
    {
        CommonFunction.SetMoneyIcon(Spt_Icon, _money.Type);
        if (!CommonFunction.CheckMoneyEnough(_money.Type,_money.Number))
        {
            Lbl_Cost.text = string.Format(ConstString.QUICK_MAT_TIP_NOT_ENOUGH, _money.Number);
        }
        else
        {
            Lbl_Cost.text = string.Format(ConstString.QUICK_MAT_TIP_ENOUGH, _money.Number);
        }
    }
    public void ShowTip(Action yesCallback)
    {
        _tipYesCallBack = yesCallback;
        if ((_money.Type == ECurrencyType.None || _money.Number <= 0)
            && _tipYesCallBack != null)
        {
            _tipYesCallBack();
            return;
        }
        if (_ignoreTip && _tipYesCallBack != null)
        {
            _tipYesCallBack();
            return;
        }

        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark,
            string.Format(ConstString.QUICK_MAT_BUY_TIP, CommonFunction.GetMoneyNameByType(_money.Type), _money.Number), 
            _tipYesCallBack, null, ConstString.HINT_LEFTBUTTON_GOON, "", TipMarkCallBack);
    }
    private void TipMarkCallBack(bool value)
    {
        _ignoreTip = value;
    }
    private void SetLabelValues()
    {
        Lbl_Cost.text = "補全材料消耗       ";
    }

    public void Uninitialize()
    {

    }
}
