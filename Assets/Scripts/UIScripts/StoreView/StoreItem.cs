using UnityEngine;
using System;
using System.Collections;
using fogs.proto.msg;

public class StoreItem : MonoBehaviour
{
    [HideInInspector]public UISprite Spt_BG;
    [HideInInspector]public UILabel Lbl_Name;
    [HideInInspector]public UISprite Spt_MoneyIcon;
    [HideInInspector]public UILabel Lbl_Money;
    [HideInInspector]public UILabel Lbl_Num;
    [HideInInspector]public UISprite Spt_Icon;
    [HideInInspector]public UISprite Spt_PropBG;
    [HideInInspector]public UISprite Spt_Quality;
    [HideInInspector]public UISprite Spt_Mark;
    [HideInInspector]public GameObject Gobj_SellOut;
    [HideInInspector]public UISprite Spt_SellOutMask;
    [HideInInspector]public UISprite Spt_SellOutTip;
    [HideInInspector]public UISprite Spt_Lock;

    private bool _initialized = false;
    private StoreGoodsData _data = null;
    private CommodityInfo _info;
    CommonItemData _commonItemData;
    private MallGoodsData _mallGoodsData;
    private int _totalMoney;
    private bool _isMall = false;  //是否是商城  否则是商店
    private bool _isLock = false; //商店有开启条件
    public void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        Spt_BG = transform.FindChild("BG").gameObject.GetComponent<UISprite>();
        Lbl_Name = transform.FindChild("Name").gameObject.GetComponent<UILabel>();
        Spt_MoneyIcon = transform.FindChild("MoneyIcon").gameObject.GetComponent<UISprite>();
        Lbl_Money = transform.FindChild("Money").gameObject.GetComponent<UILabel>();
        Lbl_Num = transform.FindChild("Num").gameObject.GetComponent<UILabel>();
        Spt_Icon = transform.FindChild("Prop/Icon").gameObject.GetComponent<UISprite>();
        Spt_PropBG = transform.FindChild("Prop/PropBG").gameObject.GetComponent<UISprite>();
        Spt_Quality = transform.FindChild("Prop/Quality").gameObject.GetComponent<UISprite>();
        Spt_Mark = transform.FindChild("Prop/Mark").gameObject.GetComponent<UISprite>();
        Gobj_SellOut = transform.FindChild("gobj_SellOut").gameObject;
        Spt_Lock = transform.FindChild("Lock").gameObject.GetComponent<UISprite>();
        //Spt_SellOutMask = transform.FindChild("gobj_SellOut/SellOutMask").gameObject.GetComponent<UISprite>();
        Spt_SellOutTip = transform.FindChild("gobj_SellOut/SellOutTip").gameObject.GetComponent<UISprite>();
        //SetLabelValues();
        UIEventListener.Get(Spt_BG.gameObject).onClick = ClickItem;
        //UIEventListener.Get(Spt_BG.gameObject).onPress = PressItem;
    }

    public void InitItem(CommodityInfo info)
    {
        Initialize();
        _isMall = false;
        _info = info;
        bool isNorShop = false;
        if(UISystem.Instance.UIIsOpen(StoreView.UIName))
        {
            isNorShop = UISystem.Instance.StoreView.ShopType == ShopType.ST_NomalShop;
        }
        SetLock(isNorShop && !CommonFunction.CheckFuncIsOpen(OpenFunctionType.Store,false));
        if (_data == null || _info.id != _data.ID)
        {
            _data = ConfigManager.Instance.mStoreGoodsConfig.GetStoreGoodsDataByID((uint)info.id);
            if (_data != null)
                SetProp();
            else
            {
                gameObject.SetActive(false);
                return;
            }
        }
        Lbl_Num.text = string.Format(ConstString.FORMAT_NUM_X, _info.num);
        _totalMoney = (int)(_info.num * _data.UnitPrice);
        Lbl_Money.text = _totalMoney.ToString();
        Gobj_SellOut.SetActive(_info.sell_out == (int)ECommodityState.HasBought);
        

    }

    public void InitItem(MallGoodsData info)
    {
        Initialize();
        _isMall = true;
        _mallGoodsData = info;
        SetLock(false);
        if (_mallGoodsData == null)
        {
            gameObject.SetActive(false);
            return;
        }
        SetProp();
        Gobj_SellOut.SetActive(false);
        Lbl_Money.text = info.mPrice.ToString();
        Lbl_Num.text = string.Empty;
    }

    private void SetLock(bool isLock)
    {
        _isLock = isLock;
        Spt_Lock.gameObject.SetActive(_isLock);        
    }

    private void SetProp()
    {
        GoodsConfigType type;
        uint propid;
        ECurrencyType currencyType;

        if(_isMall)
        {
            type = (GoodsConfigType) _mallGoodsData.mCommodityType;
            propid = _mallGoodsData.mGoodsID;
            currencyType = _mallGoodsData.mCurrencyType;
        }
        else
        {
            type = _data.PropType;
            propid = _data.PropID;
            currencyType = _data.CurrencyType;
        }
        if (type == GoodsConfigType.GCT_Equip)
        {
            _commonItemData = new CommonItemData(ConfigManager.Instance.mEquipData.FindById(propid));

        }
        else
        {
            _commonItemData = new CommonItemData(ConfigManager.Instance.mItemData.GetItemInfoByID(propid));
        }
        Lbl_Name.text = _commonItemData.Name;
        if(!_isMall)
            _commonItemData.Num = _info.num;
        CommonFunction.SetSpriteName(Spt_Icon, _commonItemData.Icon);
        CommonFunction.SetQualitySprite(Spt_Quality, _commonItemData.Quality, Spt_PropBG);
        CommonFunction.SetMoneyIcon(Spt_MoneyIcon, currencyType);
        SetMark();
    }

    private void SetMark()
    {
        CommonFunction.SetChipMark(Spt_Mark, _commonItemData.SubType, new Vector3(-25, 28, 0), new Vector3(-29, 25, 0));
    }

    private void PressItem(GameObject go, bool press)
    {
        ItemInfo info = ConfigManager.Instance.mItemData.GetItemInfoByID(_data.PropID);
        HintManager.Instance.SeeDetail(go, press, info);
    }

    private void ClickItem(GameObject go)
    {
        if(!_isMall)
        {
            if(_isLock)
            {
                //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint,string.Format(ConstString.STORE_LOCK_TIP2, 
                //    ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Store).vipLevel));

                CommonFunction.ShowVipLvNotEnoughTip(string.Format(ConstString.STORE_LOCK_TIP2,
                    ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.Store).vipLevel));
                return;
            }

            if (_info.sell_out == (int)ECommodityState.HasBought)
                return;
            UISystem.Instance.StoreView.ShowStoreItemDetail(_info, _data, _commonItemData);
        }
        else
        {
            UISystem.Instance.ShowGameUI(ItemSellView.UIName);
            UISystem.Instance.ItemSellView.UpdateViewInfo(_mallGoodsData);
        }
        
    }

    public void SetLabelValues()
    {
        Lbl_Name.text = "";
        Lbl_Money.text = "";
        Lbl_Num.text = "";
    }

}
