using UnityEngine;
using System;
using System.Collections;

public class ItemSellView
{
    public static string UIName ="ItemSellView";
    public GameObject _uiRoot;
    public UIButton Btn_ConfirmSell;
    public UISprite Tex_SellItemIcon;
    public UISprite Spt_SellItemQuality;
    public UISprite Spt_SellItemTypeMark;
    public UISprite Spt_IconBGSprite;
    public UILabel Lbl_SellItemName;
    public UILabel Lbl_HadItemCount;
    public UILabel Lbl_SellItemPrice;
    public UIButton Btn_ColseItemSell;
    public UISlider Slider_SellSlider;
    public UIButton Btn_ReduceButton;
    public UIButton Btn_AddButton;
    public UILabel Lbl_SellItemCount;
    public UILabel Lbl_PriceTitle;
    public UILabel Lbl_SellGetGold;
    public UILabel Lbl_CostTile;
    public TweenScale Anim_TScale;
    public UILabel Lbl_Title;
    public Transform Trans_SellGroup;
    public GameObject Gobj_DescGroup;
    public UILabel Lbl_Desc;
    public UISprite Spt_PriceIcon;
    public UISprite Spt_CostIcon;
    public UILabel Lbl_BtnConfirm;
    public void Initialize()
    {
        _uiRoot =  GameObject.Find("UISystem/UICamera/Anchor/Panel/ItemSellView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_ConfirmSell = _uiRoot.transform.FindChild("Anim/ConfirmSell").gameObject.GetComponent<UIButton>();
        Tex_SellItemIcon = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/SellItemIcon").gameObject.GetComponent<UISprite>();
        Spt_SellItemQuality = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/SellItemQuality").gameObject.GetComponent<UISprite>();
        Spt_SellItemTypeMark = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/SellItemTypeMark").gameObject.GetComponent<UISprite>();
        Spt_IconBGSprite = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/IconBGSprite").gameObject.GetComponent<UISprite>();
        Lbl_SellItemName = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/SellItemName").gameObject.GetComponent<UILabel>();
        Lbl_HadItemCount = _uiRoot.transform.FindChild("Anim/ItemInfoGroup/HadItemCount").gameObject.GetComponent<UILabel>();
        Lbl_SellItemPrice = _uiRoot.transform.FindChild("Anim/PriceGroup/SellItemPrice").gameObject.GetComponent<UILabel>();
        Btn_ColseItemSell = _uiRoot.transform.FindChild("Anim/ColseItemSell").gameObject.GetComponent<UIButton>();
        Slider_SellSlider = _uiRoot.transform.FindChild("Anim/SellGroup/SellSlider").gameObject.GetComponent<UISlider>();
        Btn_ReduceButton = _uiRoot.transform.FindChild("Anim/SellGroup/ReduceButton").gameObject.GetComponent<UIButton>();
        Btn_AddButton = _uiRoot.transform.FindChild("Anim/SellGroup/AddButton").gameObject.GetComponent<UIButton>();
        Lbl_SellItemCount = _uiRoot.transform.FindChild("Anim/SellGroup/SellCountGroup/SellItemCount").gameObject.GetComponent<UILabel>();
        Lbl_SellGetGold = _uiRoot.transform.FindChild("Anim/CostGroup/SellGetGold").gameObject.GetComponent<UILabel>();
        Lbl_PriceTitle = _uiRoot.transform.FindChild("Anim/PriceGroup/Title").gameObject.GetComponent<UILabel>();
        Lbl_CostTile = _uiRoot.transform.FindChild("Anim/CostGroup/CostTile").gameObject.GetComponent<UILabel>();
        Lbl_Title = _uiRoot.transform.FindChild("Anim/BgGroup/TitleSprite").gameObject.GetComponent<UILabel>();
        Trans_SellGroup = _uiRoot.transform.FindChild("Anim/SellGroup");
        Gobj_DescGroup = _uiRoot.transform.FindChild("Anim/DescGroup").gameObject;
        Lbl_Desc = _uiRoot.transform.FindChild("Anim/DescGroup/Desc").gameObject.GetComponent<UILabel>();
        Spt_PriceIcon = _uiRoot.transform.FindChild("Anim/PriceGroup/IconSprite").gameObject.GetComponent<UISprite>();
        Spt_CostIcon = _uiRoot.transform.FindChild("Anim/CostGroup/Sprite").gameObject.GetComponent<UISprite>();
        Lbl_BtnConfirm = _uiRoot.transform.FindChild("Anim/ConfirmSell/ButtonFG").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SellItemName.text = string.Empty;
        Lbl_HadItemCount.text = string.Empty;
        Lbl_SellItemPrice.text = string.Empty;
        Lbl_SellItemCount.text = string.Empty;
        Lbl_SellGetGold.text = string.Empty;
        Lbl_PriceTitle.text = "出售单价:";
        Lbl_CostTile.text = "可获得:";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
         _uiRoot.SetActive(isVisible);
    }

}
