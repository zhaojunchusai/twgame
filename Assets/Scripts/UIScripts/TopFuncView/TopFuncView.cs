using UnityEngine;
using System;
using System.Collections;

public class TopFuncView
{
    public static string UIName = "TopFuncView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_TopFuncView;
    public UIButton Btn_Coin;
    public UISprite Spt_BtnCoinIcon;
    public UILabel Lbl_BtnCoinLabel;
    public UISprite Spt_BtnCoinBG;
    public UIButton Btn_Gem;
    public UISprite Spt_BtnGemIcon;
    public UILabel Lbl_BtnGemLabel;
    public UISprite Spt_BtnGemBG;
    public UIButton Btn_SP;
    public UISprite Spt_BtnSPIcon;
    public UIButton Btn_BtnSPShowSP;
    public UISprite Spt_BtnShowSPBG;
    public UILabel Lbl_BtnShowSPLabel;
    public UILabel Lbl_BtnShowNowSPLabel;

    public GameObject Gobj_SPHint;
    public UIPanel UIPanel_gobj_SPHint;
    public UILabel Lbl_NowTimeNum;
    public UILabel Lbl_BuySP;
    public UILabel Lbl_BuySPNum;
    public UILabel Lbl_LastSP;
    public UILabel Lbl_LastSPNum;
    public UILabel Lbl_RevertSP;
    public UILabel Lbl_RevertSPNum;
    public UILabel Lbl_RevertTimeNum;
    public UILabel Lbl_RevertTime;
    public UISprite Spt_BG;
    public UILabel Lbl_NowTime;


    public AddValueItem CoinAddValue;
    public AddValueItem GemAddValue;
    public AddValueItem MaxSPAddvalue;
    public AddValueItem NowSPAddvalue;

    public UISprite CoinBG;
    public UISprite GemBG;
    public UISprite SPBG;


    public UIBoundary Boundary = new UIBoundary();
    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/TopFuncView");
        CoinAddValue = _uiRoot.transform.FindChild("Coin/Label").gameObject.GetComponent<AddValueItem>();
        GemAddValue = _uiRoot.transform.FindChild("Gem/Label").gameObject.GetComponent<AddValueItem>();
        MaxSPAddvalue = _uiRoot.transform.FindChild("SP/ShowSP/Label").gameObject.GetComponent<AddValueItem>();
        NowSPAddvalue = _uiRoot.transform.FindChild("SP/ShowSP/Label1").gameObject.GetComponent<AddValueItem>();
        CoinBG = _uiRoot.transform.FindChild("Coin/BG").gameObject.GetComponent<UISprite>();
        GemBG = _uiRoot.transform.FindChild("Gem/BG").gameObject.GetComponent<UISprite>();
        SPBG = _uiRoot.transform.FindChild("SP/ShowSP/BG").gameObject.GetComponent<UISprite>();
        //UIPanel_TopFuncView = _uiRoot.GetComponent<UIPanel>();
        Btn_Coin = _uiRoot.transform.FindChild("Coin").gameObject.GetComponent<UIButton>();
        //Spt_BtnCoinIcon = _uiRoot.transform.FindChild("Coin/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnCoinLabel = _uiRoot.transform.FindChild("Coin/Label").gameObject.GetComponent<UILabel>();
        //Spt_BtnCoinBG = _uiRoot.transform.FindChild("Coin/BG").gameObject.GetComponent<UISprite>();
        Btn_Gem = _uiRoot.transform.FindChild("Gem").gameObject.GetComponent<UIButton>();
        Spt_BtnGemIcon = _uiRoot.transform.FindChild("Gem/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnGemLabel = _uiRoot.transform.FindChild("Gem/Label").gameObject.GetComponent<UILabel>();
        //Spt_BtnGemBG = _uiRoot.transform.FindChild("Gem/BG").gameObject.GetComponent<UISprite>();
        Btn_SP = _uiRoot.transform.FindChild("SP").gameObject.GetComponent<UIButton>();
        //Spt_BtnSPIcon = _uiRoot.transform.FindChild("SP/Icon").gameObject.GetComponent<UISprite>();
        Btn_BtnSPShowSP = _uiRoot.transform.FindChild("SP/ShowSP").gameObject.GetComponent<UIButton>();
        //Spt_BtnShowSPBG = _uiRoot.transform.FindChild("SP/ShowSP/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnShowSPLabel = _uiRoot.transform.FindChild("SP/ShowSP/Label").gameObject.GetComponent<UILabel>();
        Lbl_BtnShowNowSPLabel = _uiRoot.transform.FindChild("SP/ShowSP/Label1").gameObject.GetComponent<UILabel>();

        Gobj_SPHint = _uiRoot.transform.FindChild("gobj_SPHint").gameObject;
        //UIPanel_gobj_SPHint = _uiRoot.transform.FindChild("gobj_SPHint").gameObject.GetComponent<UIPanel>();
        Lbl_NowTimeNum = _uiRoot.transform.FindChild("gobj_SPHint/NowTimeNum").gameObject.GetComponent<UILabel>();
        Lbl_BuySP = _uiRoot.transform.FindChild("gobj_SPHint/BuySP").gameObject.GetComponent<UILabel>();
        Lbl_BuySPNum = _uiRoot.transform.FindChild("gobj_SPHint/BuySPNum").gameObject.GetComponent<UILabel>();
        Lbl_LastSP = _uiRoot.transform.FindChild("gobj_SPHint/LastSP").gameObject.GetComponent<UILabel>();
        Lbl_LastSPNum = _uiRoot.transform.FindChild("gobj_SPHint/LastSPNum").gameObject.GetComponent<UILabel>();
        Lbl_RevertSP = _uiRoot.transform.FindChild("gobj_SPHint/RevertSP").gameObject.GetComponent<UILabel>();
        Lbl_RevertSPNum = _uiRoot.transform.FindChild("gobj_SPHint/RevertSPNum").gameObject.GetComponent<UILabel>();
        Lbl_RevertTimeNum = _uiRoot.transform.FindChild("gobj_SPHint/RevertTimeNum").gameObject.GetComponent<UILabel>();
        Lbl_RevertTime = _uiRoot.transform.FindChild("gobj_SPHint/RevertTime").gameObject.GetComponent<UILabel>();
        //Spt_BG = _uiRoot.transform.FindChild("gobj_SPHint/BG").gameObject.GetComponent<UISprite>();
        Lbl_NowTime = _uiRoot.transform.FindChild("gobj_SPHint/NowTime").gameObject.GetComponent<UILabel>();

        //SetLabelValues();
        //SetBoundary();
    }


    public void SetBoundary()
    {
        Boundary.left = -512.5f;
        Boundary.right = 1332.5f;
        Boundary.up = 782f;
        Boundary.down = -294f;
    }

    public void SetLabelValues()
    {
        Lbl_BtnCoinLabel.text = "0";
        Lbl_BtnGemLabel.text = "0";
        Lbl_BtnShowSPLabel.text = "0";
        Lbl_NowTimeNum.text = "23:59:59";
        Lbl_BuySP.text = "已买体力次数";
        Lbl_BuySPNum.text = "0";
        Lbl_LastSP.text = "下点体力恢复";
        Lbl_LastSPNum.text = "05:59";
        Lbl_RevertSP.text = "恢复全部体力";
        Lbl_RevertSPNum.text = "23:25:25";
        Lbl_RevertTimeNum.text = "5Min";
        Lbl_RevertTime.text = "恢复体力间隔";
        Lbl_NowTime.text = "当前时间";
    }

    public void Uninitialize()
    {

    }
    
    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
