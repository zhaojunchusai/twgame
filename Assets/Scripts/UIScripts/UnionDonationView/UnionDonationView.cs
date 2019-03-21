using UnityEngine;
using System;
using System.Collections;

public class UnionDonationView
{
    public static string UIName ="UnionDonationView";
    public GameObject _uiRoot;
    public UILabel Lbl_Title;
    public UISprite Spt_Mask;
    public UIButton Btn_Gold;
    public UISprite Spt_BtnGoldBG;
    public UILabel Lbl_BtnGoldNum;
    public UISprite Spt_BtnGoldIcon;
    public UILabel Lbl_BtnGoldTokenNum;
    public UILabel Lbl_BtnGoldSupplyTip;
    public UILabel Lbl_BtnGoldTimesCount;
    public UIButton Btn_Record;
    public UILabel Lbl_BtnRecordLabel;
    public UIButton Btn_Diamond;
    public UISprite Spt_BtnDiamondBG;
    public UILabel Lbl_BtnDiamondNum;
    public UISprite Spt_BtnDiamondIcon;
    public UILabel Lbl_BtnDiamondTokenNum;
    public UILabel Lbl_BtnDiamondSupplyTip;
    public UILabel Lbl_BtnDiamondTimesCount;
    public GameObject Go_EffectMask;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/UnionDonationView");
        Go_EffectMask = _uiRoot.transform.FindChild("EffectMask").gameObject;
        Lbl_Title = _uiRoot.transform.FindChild("Title").gameObject.GetComponent<UILabel>();
        Spt_Mask = _uiRoot.transform.FindChild("Mask").gameObject.GetComponent<UISprite>();
        Btn_Gold = _uiRoot.transform.FindChild("Gold").gameObject.GetComponent<UIButton>();
        Spt_BtnGoldBG = _uiRoot.transform.FindChild("Gold/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnGoldNum = _uiRoot.transform.FindChild("Gold/Num").gameObject.GetComponent<UILabel>();
        Spt_BtnGoldIcon = _uiRoot.transform.FindChild("Gold/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnGoldTokenNum = _uiRoot.transform.FindChild("Gold/TokenNum").gameObject.GetComponent<UILabel>();
        Lbl_BtnGoldSupplyTip = _uiRoot.transform.FindChild("Gold/SupplyTip").gameObject.GetComponent<UILabel>();
        Lbl_BtnGoldTimesCount = _uiRoot.transform.FindChild("Gold/TimesCount").gameObject.GetComponent<UILabel>();
        Btn_Record = _uiRoot.transform.FindChild("Record").gameObject.GetComponent<UIButton>();
        Lbl_BtnRecordLabel = _uiRoot.transform.FindChild("Record/Label").gameObject.GetComponent<UILabel>();
        Btn_Diamond = _uiRoot.transform.FindChild("Diamond").gameObject.GetComponent<UIButton>();
        Spt_BtnDiamondBG = _uiRoot.transform.FindChild("Diamond/BG").gameObject.GetComponent<UISprite>();
        Lbl_BtnDiamondNum = _uiRoot.transform.FindChild("Diamond/Num").gameObject.GetComponent<UILabel>();
        Spt_BtnDiamondIcon = _uiRoot.transform.FindChild("Diamond/Icon").gameObject.GetComponent<UISprite>();
        Lbl_BtnDiamondTokenNum = _uiRoot.transform.FindChild("Diamond/TokenNum").gameObject.GetComponent<UILabel>();
        Lbl_BtnDiamondSupplyTip = _uiRoot.transform.FindChild("Diamond/SupplyTip").gameObject.GetComponent<UILabel>();
        Lbl_BtnDiamondTimesCount = _uiRoot.transform.FindChild("Diamond/TimesCount").gameObject.GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        //Lbl_Title.text = "军团捐献";
        //Lbl_BtnGoldNum.text = "50000";
        //Lbl_BtnGoldTokenNum.text = "可获得      x555";
        //Lbl_BtnGoldSupplyTip.text = "军团可获得资源555";
        //Lbl_BtnGoldTimesCount.text = "今日捐献次数:1/5";
        //Lbl_BtnRecordLabel.text = "捐献记录";
        //Lbl_BtnDiamondNum.text = "50000";
        //Lbl_BtnDiamondTokenNum.text = "可获得      x555";
        //Lbl_BtnDiamondSupplyTip.text = "军团可获得资源555";
        //Lbl_BtnDiamondTimesCount.text = "今日捐献次数:1/5";
        Go_EffectMask.gameObject.SetActive(false);
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
