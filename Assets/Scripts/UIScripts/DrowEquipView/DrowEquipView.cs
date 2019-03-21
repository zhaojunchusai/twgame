using UnityEngine;
using System;
using System.Collections;

public class DrowEquipView
{
    public static string UIName ="DrowEquipView";
    public GameObject _uiRoot;
    public UIPanel UIPanel_DrowEquipView;
    public OffestSortingOrder OffestSortingOrder_DrowEquipView;
    public UISprite Spt_ChooseOnce;
    public UILabel Lbl_OnceFreeLb;
    public UISprite Spt_Sprite;
    public UISprite Spt_OnceFreeSP;
    public UISprite Spt_ChooseTen;
    public UISprite Spt_CurrencyIcon;
    public UILabel Lbl_CurrencyNum;
    public UISprite Spt_TenCurrencyIcon;
    public UILabel Lbl_TenCurrencyNum;
    public UIButton Btn_Button_close;
    public GameObject Dis;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/DrowEquipView");
        Spt_ChooseOnce = _uiRoot.transform.FindChild("Anim/ChooseOnce").gameObject.GetComponent<UISprite>();
        Lbl_OnceFreeLb = _uiRoot.transform.FindChild("Anim/ChooseOnce/gobj_OnceFree/OnceFreeLb").gameObject.GetComponent<UILabel>();
        Spt_Sprite = _uiRoot.transform.FindChild("Anim/ChooseOnce/gobj_OnceFree/OnceFreeLb/Sprite").gameObject.GetComponent<UISprite>();
        Spt_OnceFreeSP = _uiRoot.transform.FindChild("Anim/ChooseOnce/gobj_OnceFree/OnceFreeSP").gameObject.GetComponent<UISprite>();
        Spt_ChooseTen = _uiRoot.transform.FindChild("Anim/ChooseTen").gameObject.GetComponent<UISprite>();
        Spt_CurrencyIcon = _uiRoot.transform.FindChild("Anim/OnceCurrencyBG/CurrencyIcon").gameObject.GetComponent<UISprite>();
        Lbl_CurrencyNum = _uiRoot.transform.FindChild("Anim/OnceCurrencyBG/CurrencyNum").gameObject.GetComponent<UILabel>();
        Spt_TenCurrencyIcon = _uiRoot.transform.FindChild("Anim/TenCurrencyBG/TenCurrencyIcon").gameObject.GetComponent<UISprite>();
        Lbl_TenCurrencyNum = _uiRoot.transform.FindChild("Anim/TenCurrencyBG/TenCurrencyNum").gameObject.GetComponent<UILabel>();
        Btn_Button_close = _uiRoot.transform.FindChild("Anim/Button_close").gameObject.GetComponent<UIButton>();
        Dis = _uiRoot.transform.FindChild("Anim/TenCurrencyBG/Dis").gameObject;
        Spt_OnceFreeSP.gameObject.SetActive(false);
        Lbl_OnceFreeLb.gameObject.SetActive(false);
       // SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_OnceFreeLb.text = "今日剩余免费次数：5";
        Lbl_CurrencyNum.text = "999999";
        Lbl_TenCurrencyNum.text = "999999";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
