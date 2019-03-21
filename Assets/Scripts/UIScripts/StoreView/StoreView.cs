using UnityEngine;
using System;
using System.Collections;

public class StoreView
{
    public static string UIName = "StoreView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_StoreUI;
    public UISprite Spt_TitleBG;
    public UILabel Lbl_Title;
    public UISprite Spt_StoreBG;
    public UISprite Spt_StoreFG;
    public UITexture Tex_BG;
    public UIButton Btn_Close;
    public UISprite Spt_BtnCloseBackground;
    public UILabel Lbl_BtnCloseLabel;
    public UIPanel UIPanel_ItemPan;
    public UIScrollView ScrView_ItemPan;
    public UIGrid Grd_StoreItems;
    public UIPanel UIPanel_Layer3;
    public GameObject Gobj_StoreItemDetail;
    public UIButton Btn_CloseDetail;
    public UISprite Spt_Mask;
    public UIButton Btn_Buy;
    public UISprite Spt_BtnBuyBackground;
    public UILabel Lbl_BtnBuyLabel;
    public UISprite Spt_DescriptionMainBG;
    public UILabel Lbl_DescriptionMain;
    public UISprite Spt_PropBG;
    public UISprite Spt_PropIcon;
    public UILabel Lbl_PropName;
    public UILabel Lbl_PropNumber;
    public UISprite Spt_Quality;
    public UISprite Spt_Mark;
    public UISprite Spt_PropItemFG;
    public UISprite Spt_PropIconBG;
    public UILabel Lbl_BuyPriceTip;
    public UISprite Spt_BuyMoneyIcon;
    public UILabel Lbl_BuyPrice;
    public UISprite Spt_BG;
    public UILabel Lbl_BuyCountTip;
    public UILabel Lbl_BuyCount;
    public UISprite Spt_DescBG;
    public UISprite Spt_OtherCurrencyIcon;
    public UILabel Lbl_OtherCurrencyNum;
    public UISprite Spt_OtherCurrencyBG;
    public UILabel Lbl_RefreashTimeTip;
    public UILabel Lbl_RefreashTime;
    public UIButton Btn_Refreash;
    public UISprite Spt_BtnRefreashBackground;
    public UILabel Lbl_BtnRefreashLabel;
    public UILabel Lbl_PropNumberTip;
    public UISprite Spt_Decoration;
    public GameObject Gobj_StoreItem;
    public GameObject Gobj_Talk;
    public UILabel Lbl_Talk;
    public TweenScale Anim_TScale;
    public UISprite EffectMask;
    public UIButton Btn_OneKeyBuy;
    public GameObject Gobj_Lock;
    public UILabel Lbl_LockTip;
    public UISprite Spt_RefreshCostIcon;
    public UILabel Lbl_RefreshCostNum;

    public GameObject ItemPoint1;
    public GameObject ItemPoint2;
    public GameObject ItemPoint3;
    public GameObject ItemPoint4;
    public GameObject ItemPoint5;
    public GameObject ItemPoint6;
    public GameObject ItemPoint7;
    public GameObject ItemPoint8;
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/StoreView");
        //UIPanel_StoreUI = _uiRoot.GetComponent<UIPanel>();
        ItemPoint1 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint1").gameObject;
        ItemPoint2 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint2").gameObject;
        ItemPoint3 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint3").gameObject;
        ItemPoint4 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint4").gameObject;
        ItemPoint5 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint5").gameObject;
        ItemPoint6 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint6").gameObject;
        ItemPoint7 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint7").gameObject;
        ItemPoint8 = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/ItemPoint/ItemPoint8").gameObject;
        EffectMask = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/EffectMask").gameObject.GetComponent<UISprite>();
        
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Gobj_StoreItem = _uiRoot.transform.FindChild("Anim/Pre/StoreItem").gameObject;
        Spt_TitleBG = _uiRoot.transform.FindChild("Anim/Layer1/TitleBG").gameObject.GetComponent<UISprite>();
        Lbl_Title = _uiRoot.transform.FindChild("Anim/Layer1/Title").gameObject.GetComponent<UILabel>();
        //Spt_StoreBG = _uiRoot.transform.FindChild("Anim/Layer1/StoreBG").gameObject.GetComponent<UISprite>();
        //Spt_StoreFG = _uiRoot.transform.FindChild("Anim/Layer1/StoreFG").gameObject.GetComponent<UISprite>();
        //Tex_BG = _uiRoot.transform.FindChild("BG").gameObject.GetComponent<UITexture>();
        Btn_Close = _uiRoot.transform.FindChild("Anim/Layer1/Close").gameObject.GetComponent<UIButton>();
        //Spt_BtnCloseBackground = _uiRoot.transform.FindChild("Anim/Layer1/Close/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnCloseLabel = _uiRoot.transform.FindChild("Anim/Layer1/Close/Label").gameObject.GetComponent<UILabel>();
        //UIPanel_ItemPan = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan").gameObject.GetComponent<UIPanel>();
        ScrView_ItemPan = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan").gameObject.GetComponent<UIScrollView>();
        Grd_StoreItems = _uiRoot.transform.FindChild("Anim/Layer2/ItemPan/StoreItems").gameObject.GetComponent<UIGrid>();
        //UIPanel_Layer3 = _uiRoot.transform.FindChild("Anim/Layer3").gameObject.GetComponent<UIPanel>();
        Gobj_StoreItemDetail = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail").gameObject;
        //Spt_Mask = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Mask").gameObject.GetComponent<UISprite>();
        Btn_CloseDetail = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Close").gameObject.GetComponent<UIButton>();
        Btn_Buy = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Buy").gameObject.GetComponent<UIButton>();
        //Spt_BtnBuyBackground = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Buy/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnBuyLabel = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Buy/Label").gameObject.GetComponent<UILabel>();
       // Spt_DescriptionMainBG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Description/DescriptionMainBG").gameObject.GetComponent<UISprite>();
        Lbl_DescriptionMain = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/Description/DescriptionMain").gameObject.GetComponent<UILabel>();
        //Spt_PropBG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropBG").gameObject.GetComponent<UISprite>();
        Spt_PropIcon = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropIcon").gameObject.GetComponent<UISprite>();
        Lbl_PropName = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropName").gameObject.GetComponent<UILabel>();
        Lbl_PropNumber = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropNumber").gameObject.GetComponent<UILabel>();
        Lbl_PropNumberTip = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropNumberTip").gameObject.GetComponent<UILabel>();
        Spt_Quality = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/Quality").gameObject.GetComponent<UISprite>();
        Spt_Mark = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/Mark").gameObject.GetComponent<UISprite>();
        //Spt_PropItemFG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropItemFG").gameObject.GetComponent<UISprite>();
        //Spt_PropIconBG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropItem/PropIconBG").gameObject.GetComponent<UISprite>();
        Lbl_BuyPriceTip = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BuyPriceTip").gameObject.GetComponent<UILabel>();
        Spt_BuyMoneyIcon = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BuyPriceTip/BuyMoneyIcon").gameObject.GetComponent<UISprite>();
        Lbl_BuyPrice = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BuyPriceTip/BuyPrice").gameObject.GetComponent<UILabel>();
        //Spt_BG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BG").gameObject.GetComponent<UISprite>();
        Lbl_BuyCountTip = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BuyCountTip").gameObject.GetComponent<UILabel>();
        Lbl_BuyCount = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/PropSellInfo/BuyCountTip/BuyCount").gameObject.GetComponent<UILabel>();
        //Spt_DescBG = _uiRoot.transform.FindChild("Anim/Layer3/gobj_StoreItemDetail/Page/DescBG").gameObject.GetComponent<UISprite>();
        Spt_OtherCurrencyIcon = _uiRoot.transform.FindChild("Anim/Layer3/OtherCurrency/Icon").gameObject.GetComponent<UISprite>();
        Lbl_OtherCurrencyNum = _uiRoot.transform.FindChild("Anim/Layer3/OtherCurrency/Label").gameObject.GetComponent<UILabel>();
        Spt_OtherCurrencyBG = _uiRoot.transform.FindChild("Anim/Layer3/OtherCurrency/BG").gameObject.GetComponent<UISprite>();
        Lbl_RefreashTimeTip = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/RefreashTimeTip").gameObject.GetComponent<UILabel>();
        Lbl_RefreashTime = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/RefreashTime").gameObject.GetComponent<UILabel>();
        Btn_Refreash = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/Refreash").gameObject.GetComponent<UIButton>();
        Btn_OneKeyBuy = _uiRoot.transform.FindChild("Anim/Layer3/OneKeyBuy").gameObject.GetComponent<UIButton>();
        //Spt_BtnRefreashBackground = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/Refreash/Background").gameObject.GetComponent<UISprite>();
        Lbl_BtnRefreashLabel = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/Refreash/Label").gameObject.GetComponent<UILabel>();
        Spt_Decoration = _uiRoot.transform.FindChild("Anim/Layer3/Decoration").gameObject.GetComponent<UISprite>();
        Gobj_Talk = _uiRoot.transform.FindChild("Anim/Layer3/Talk").gameObject;
        Lbl_Talk = _uiRoot.transform.FindChild("Anim/Layer3/Talk/Talk").gameObject.GetComponent<UILabel>();
        EffectMask.gameObject.SetActive(false);
        Gobj_Lock = _uiRoot.transform.FindChild("Anim/Layer3/LockTip").gameObject;
        Lbl_LockTip = _uiRoot.transform.FindChild("Anim/Layer3/LockTip/Label").gameObject.GetComponent<UILabel>();

        Spt_RefreshCostIcon = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/RefreshCostIcon").gameObject.GetComponent<UISprite>();
        Lbl_RefreshCostNum = _uiRoot.transform.FindChild("Anim/Layer3/RefreashItems/RefreshCostIcon/RefreshCostNum").gameObject.GetComponent<UILabel>();

        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_Title.text = "商铺";
        Lbl_BtnCloseLabel.text = "";
        Lbl_BtnBuyLabel.text = "确认购买";
        Lbl_DescriptionMain.text = "";
        Lbl_PropName.text = "名字最长六个";
        Lbl_PropNumber.text = "999";
        Lbl_PropNumberTip.text = "拥有：";
        Lbl_BuyPriceTip.text = "花费：";
        Lbl_BuyPrice.text = "99999";
        Lbl_BuyCountTip.text = "购买       个";
        Lbl_BuyCount.text = "999";
        Lbl_OtherCurrencyNum.text = "999999";
        Lbl_RefreashTimeTip.text = "下次刷新时间：";
        Lbl_RefreashTime.text = "12:00";
        Lbl_BtnRefreashLabel.text = "刷新";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
