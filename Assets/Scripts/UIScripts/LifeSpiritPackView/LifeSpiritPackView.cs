using UnityEngine;
using System;
using System.Collections;

public class LifeSpiritPackView
{
    public static string UIName = "LifeSpiritPackView";
    public GameObject _uiRoot;
    public UIButton Btn_CloseView;
    public UILabel Lbl_SelectItemTip;
    public GameObject Gobj_LifeSpiritInfo;
    public UISprite Spt_LifeSpiritInfoFrame;
    public UISprite Spt_LifeSpiritShading;
    public UISprite Spt_LifeSpiritTypeMark;
    public UISprite Spt_LifeSpiritIcon;
    public UILabel Lbl_ArtifactItemName;
    public UILabel Lbl_ArtifactItemLevel;
    public UITable UITable_ArtifactAtt;
    public UILabel Lbl_SkillDesc;
    public UIGrid Grd_LifeSpiritAttGroup;
    public GameObject gobj_LifeSpiritAttComp;
    public UILabel Lbl_ArtifactDesc;
    public UIButton Btn_ArtifactSell;
    public UISprite Btn_LifeSpiritSellBg;
    public UILabel Lbl_LifeSpiritSell;
    public UIButton Btn_ArtifactStrengthen;
    public UISprite Spt_LifeSpiritStrengthenBg;
    public UILabel Lbl_LifeSpiritStrengthen;
    public UIButton Btn_AddLifeSpiritGrid;
    public UILabel Lbl_LifeSpiritCount;
    public UIScrollView ScrView_LifeSpiritScroll;
    public UIGrid Grd_LifeSpiritGrid;
    public UIWrapContent UIWrapContent_LifeSpiritGrid;
    public GameObject Gobj_LifeSpiritComp;
    public UILabel Lbl_NoItemTip;
    public GameObject Gobj_SellGroup;
    public UISprite Spt_SellIcon;
    public UILabel Lbl_SellPrice;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/LifeSpiritPackView");
        Btn_CloseView = _uiRoot.transform.FindChild("Anim/CloseView").gameObject.GetComponent<UIButton>();
        Lbl_SelectItemTip = _uiRoot.transform.FindChild("Anim/LeftGroup/SelectItemTip").gameObject.GetComponent<UILabel>();
        Gobj_LifeSpiritInfo = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo").gameObject;
        Spt_LifeSpiritInfoFrame = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/ArtifactInfoQuality").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritShading = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/BgSprite").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/ArtifactInfoIcon").gameObject.GetComponent<UISprite>();
        Spt_LifeSpiritTypeMark = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/TypeMark").gameObject.GetComponent<UISprite>();
        Lbl_ArtifactItemName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/NameGroup/ArtifactItemName").gameObject.GetComponent<UILabel>();
        Lbl_ArtifactItemLevel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/NameGroup/ArtifactItemLevel").gameObject.GetComponent<UILabel>();
        UITable_ArtifactAtt = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt").gameObject.GetComponent<UITable>();
        Lbl_SkillDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/SkillDesc").gameObject.GetComponent<UILabel>();
        Grd_LifeSpiritAttGroup = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactAttGroup").gameObject.GetComponent<UIGrid>();
        Lbl_ArtifactDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactDesc").gameObject.GetComponent<UILabel>();
        Btn_ArtifactSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell").gameObject.GetComponent<UIButton>();
        Btn_LifeSpiritSellBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell/Bg").gameObject.GetComponent<UISprite>();
        Lbl_LifeSpiritSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell/Label").gameObject.GetComponent<UILabel>();
        Btn_ArtifactStrengthen = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen").gameObject.GetComponent<UIButton>();
        Spt_LifeSpiritStrengthenBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen/Bg").gameObject.GetComponent<UISprite>();
        Lbl_LifeSpiritStrengthen = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen/Label").gameObject.GetComponent<UILabel>();
        Btn_AddLifeSpiritGrid = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/AddSoldierEquip").gameObject.GetComponent<UIButton>();
        Lbl_LifeSpiritCount = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/SoldierEquipCount").gameObject.GetComponent<UILabel>();
        ScrView_LifeSpiritScroll = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll").gameObject.GetComponent<UIScrollView>();
        Grd_LifeSpiritGrid = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/SoldierEquipGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_LifeSpiritGrid = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/SoldierEquipGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_LifeSpiritComp = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/gobj_SoldierEquipComp").gameObject;
        Lbl_NoItemTip = _uiRoot.transform.FindChild("Anim/RightGroup/NoItemTip").gameObject.GetComponent<UILabel>();
        gobj_LifeSpiritAttComp = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactAttGroup/gobj_ArtifactAttComp").gameObject;
        Gobj_SellGroup = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/SellGroup").gameObject;
        Spt_SellIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/SellGroup/SellIcon").GetComponent<UISprite>();
        Lbl_SellPrice = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/SellGroup/SoldierEquipPrice").GetComponent<UILabel>();
        SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_SelectItemTip.text = "請選擇一個物品";
        Lbl_ArtifactItemName.text = "";
        Lbl_ArtifactItemLevel.text = "";
        Lbl_SkillDesc.text = "";
        Lbl_ArtifactDesc.text = "";
        Lbl_LifeSpiritCount.text = "";
        Lbl_NoItemTip.text = "當前無道具";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }

}
