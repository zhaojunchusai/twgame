using UnityEngine;
using System;
using System.Collections;

public class BuyCoinView
{
    public static string UIName = "BuyCoinView";
    public GameObject _uiRoot;
    public UILabel Lbl_DiamondNumLabel;
    public UILabel Lbl_CoinNumLabel;
    public UILabel Lbl_TodayRestLabel;
    public UILabel Lbl_TodayRestNumLabel;
    public UILabel Lbl_TodayTotalNumLabel;
    public UILabel Lbl_AllCostLabel;
    public UILabel Lbl_AllCostNumLabel;
    public UIScrollView ScrView_ExchangeScrollView;
    public UIGrid Grd_ExchangeGrid;
    public UIButton Btn_ContinueButton;
    public UILabel Lbl_BtnContinueButtonContinueTimesLabel;
    public UIButton Btn_ExchangeButton;
    public UIPanel Pnl_ExchangeResultPanel;
    public UILabel Lbl_TitleLabel;
    public UISprite Spt_MaskBGSprite;
    public GameObject Obj_BuyCoinResultSource;
    public GameObject Obj_BuyCoinItemSource;
    public TweenScale OffsetRoot_TScale;
    public RollSrollViewItem mRollSrollViewItem;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/BuyCoinView");
        OffsetRoot_TScale = _uiRoot.transform.FindChild("OffsetRoot").gameObject.GetComponent<TweenScale>();
        Lbl_DiamondNumLabel = _uiRoot.transform.FindChild("OffsetRoot/CostGroup/DiamondGroup/DiamondNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_CoinNumLabel = _uiRoot.transform.FindChild("OffsetRoot/CostGroup/CoinGroup/CoinNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_TodayRestLabel = _uiRoot.transform.FindChild("OffsetRoot/CostGroup/TodayRestLabel").gameObject.GetComponent<UILabel>();
        Lbl_TodayRestNumLabel = _uiRoot.transform.FindChild("OffsetRoot/CostGroup/TodayRestNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_TodayTotalNumLabel = _uiRoot.transform.FindChild("OffsetRoot/CostGroup/TodayTotalNumLabel").gameObject.GetComponent<UILabel>();
        Lbl_AllCostLabel = _uiRoot.transform.FindChild("OffsetRoot/ExchangeResultGroup/AllCostLabel").gameObject.GetComponent<UILabel>();
        Lbl_AllCostNumLabel = _uiRoot.transform.FindChild("OffsetRoot/ExchangeResultGroup/AllCostNumLabel").gameObject.GetComponent<UILabel>();
        ScrView_ExchangeScrollView = _uiRoot.transform.FindChild("OffsetRoot/ExchangeResultGroup/ExchangeScrollView").gameObject.GetComponent<UIScrollView>();
        Grd_ExchangeGrid = _uiRoot.transform.FindChild("OffsetRoot/ExchangeResultGroup/ExchangeScrollView/ExchangeGrid").gameObject.GetComponent<UIGrid>();
        Btn_ContinueButton = _uiRoot.transform.FindChild("OffsetRoot/ContinueButton").gameObject.GetComponent<UIButton>();
        Lbl_BtnContinueButtonContinueTimesLabel = _uiRoot.transform.FindChild("OffsetRoot/ContinueButton/ContinueTimesLabel").gameObject.GetComponent<UILabel>();
        Btn_ExchangeButton = _uiRoot.transform.FindChild("OffsetRoot/ExchangeButton").gameObject.GetComponent<UIButton>();
        Pnl_ExchangeResultPanel = _uiRoot.transform.FindChild("OffsetRoot/ExchangeResultGroup/ExchangeResultPanel").gameObject.GetComponent<UIPanel>();
        Lbl_TitleLabel = _uiRoot.transform.FindChild("OffsetRoot/TitleGroup/TitleLabel").gameObject.GetComponent<UILabel>();
        Obj_BuyCoinItemSource = _uiRoot.transform.FindChild("Source/BuyCoinItem").gameObject;
        Obj_BuyCoinResultSource = _uiRoot.transform.FindChild("Source/BuyCoinResultItem").gameObject;
        Spt_MaskBGSprite = _uiRoot.transform.FindChild("MaskBGSprite").gameObject.GetComponent<UISprite>();
        mRollSrollViewItem = ScrView_ExchangeScrollView.gameObject.AddComponent<RollSrollViewItem>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_DiamondNumLabel.text = "0";
        Lbl_CoinNumLabel.text = "";
        Lbl_TodayRestLabel.text = ConstString.EXCHANGE_GOLD_TODAYUSE;
        Lbl_TodayRestNumLabel.text = string.Format(ConstString.FORMAT_BUY_COIN_TIMES, 0);
        Lbl_TodayTotalNumLabel.text = "0";
        Lbl_AllCostLabel.text = ConstString.EXCHANGE_GOLD_ALLCOSTDESC;
        Lbl_AllCostNumLabel.text = string.Format(ConstString.EXCHANGE_GOLD_ALLCOST, 0);
        Lbl_BtnContinueButtonContinueTimesLabel.text = "0";
        Lbl_TitleLabel.text = ConstString.EXCHANGE_GOLD_TITLE; 
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
