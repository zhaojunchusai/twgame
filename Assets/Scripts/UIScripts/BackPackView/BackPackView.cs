using UnityEngine;
using System;
using System.Collections;

public class BackPackView
{
    public static string UIName = "BackPackView";
    public GameObject _uiRoot;
    public GameObject Gobj_MainPanel;
    public UIButton Btn_CloseView;
    public GameObject Gobj_ConsumeInfo;
    public UISprite Spt_ConsumeQuality;
    public UISprite Spt_ConsumeItemBg;
    public UISprite Tex_ConsumeIcon;
    public UILabel Lbl_ConsumeItemName;
    public UILabel Lbl_ConsumeItemCount;
    public UILabel Lbl_ConsumeDesc;
    public UILabel Lbl_ConsumeItemPrice;
    public UIButton Btn_ConsumeSell;
    public UISprite Spt_ConsumeSellBtnBg;
    public UILabel Lbl_ConsumeSellBtnLabel;
    public UIButton Btn_ConsumeUse;
    public UISprite Spt_ConsumeUseBtnBg;
    public UILabel Spt_ConsumeUseBtnLabel;
    public UIButton Btn_ConsumeGet;
    public UISprite Spt_ConsumeGetBtnBg;
    public UILabel Lbl_ConsumeGetBtnLabel;
    public UIButton Btn_ConsumeType;
    public UIButton Btn_MaterialType;
    public UIButton Btn_ArtifactType;
    public UIButton Btn_SoldierEquipType;

    public GameObject Gobj_MaterialInfo;
    public UISprite Spt_MaterialQuality;
    public UISprite Spt_MaterialItemBg;
    public UISprite Tex_MaterialIcon;
    public UILabel Lbl_MaterialItemName;
    public UILabel Lbl_MaterialItemCount;
    public UILabel Lbl_MaterialDesc;
    public UILabel Lbl_ComposeTip;
    public UILabel Lbl_ComposeTitle;
    public GameObject Gobj_ComposeCostGroup;
    public UILabel Lbl_MaterialComposeCost;
    public UILabel Lbl_MaterialItemPrice;
    public UIButton Btn_MaterialSell;
    public UISprite Spt_MaterialSellBtnBg;
    public UILabel Lbl_MaterialSellBtnLabel;
    public UIButton Btn_ChipCompose;
    public UIButton Btn_GetMaterial;
    public UISprite Spt_GetMaterialBtnBg;
    public UILabel Spt_GetMaterialBtnLabel;
    public UILabel Lbl_SelectItemTip;

    public GameObject Gobj_ArtifactInfo;
    public UISprite Spt_ArtifactInfoQuality;
    public UISprite Tex_ArtifactInfoIcon;
    public UISprite Spt_ArtifactItemBg;
    public UILabel Lbl_ArtifactItemName;
    public UILabel Lbl_ArtifactItemLevel;

    public UITable Table_ArtifactAtt;
    public UILabel Lbl_UnlockDesc;
    public UIGrid Grd_ArtifactAttGroup;
    public GameObject Gobj_ArtifactAttComp;
    public UILabel Lbl_ArtifactDesc;
    public UIButton Btn_ArtifactSell;
    public UISprite Spt_BtnArtifactSellBg;
    public UILabel Spt_BtnArtifactSellLabel;
    public UIButton Btn_ArtifactStrengthen;
    public UISprite Btn_ArtifactStrengthenBg;
    public UILabel Btn_ArtifactStrengthenLabel;

    public GameObject Gobj_SoldierEquipInfo;
    public UISprite Spt_SoldierEquipQuality;
    public UISprite Spt_SoldierEquipItemBg;
    public UISprite Tex_SoldierEquipIcon;
    public UILabel Lbl_SoldierEquipName;
    public UILabel Lbl_SoldierEquipInfoLevel;
    public UIScrollView ScrView_SuitScrollView;
    public UITable UITable_SuitTable;
    public GameObject Gobj_SuitNameGroup;
    public UILabel Lbl_SuitName;
    public UILabel Lbl_SuitEquipDesc;
    public UISprite Spt_DividingLine;
    public UILabel Lbl_SoldierEquipDesc;
    public UILabel Lbl_SoldierEquipPrice;
    public UIButton Btn_SoldierEquipSell;
    public UISprite Spt_BtnSoldierEquipSellBg;
    public UILabel Spt_BtnSoldierEquipSellLabel;
    public UIButton Btn_SoldierEquipDetail;
    public UISprite Spt_BtnSoldierEquipDetailBg;
    public UILabel Spt_BtnSoldierEquipDetailLabel;
    public UIGrid Grd_BtnFuncGroup;
    public UIButton Btn_AddSoldierEquip;
    public UILabel Lbl_BtnAddSoldierEquipLabel;

    public UIScrollView ScrView_ConsumeScroll;
    public UIGrid Grd_ConsumeGrid;
    public UIWrapContent UIWrapContent_ConsumeGrid;
    public GameObject Gobj_ConsumeComp;

    public UIScrollView ScrView_MaterialScroll;
    public UIGrid Grd_MaterialGrid;
    public UIWrapContent UIWrapContent_MaterialGrid;
    public GameObject Gobj_MaterialComp;

    public UIScrollView ScrView_ArtifactScroll;
    public UIGrid Grd_ArtifactGrid;
    public UIWrapContent UIWrapContent_ArtifactGrid;
    public GameObject Gobj_ArtifactComp;

    public UIScrollView ScrView_SoldierEquipScroll;
    public UIGrid Grd_SoldierEquipGrid;
    public UIWrapContent UIWrapContent_SoldierEquipGrid;
    public GameObject Gobj_SoldierEquipComp;
    public GameObject Gobj_SoldierEquipGrid;
    public UILabel Lbl_SoldierEquipGridCount;
    public UILabel Lbl_NoItemTip;
    //public GameObject Gobj_SoldierEquipDetailInfo;
    //public UIButton Btn_CloseSoldierEquipInfo;
    //public UIButton Btn_SoldierEquipObtain;
    //public UIButton Btn_SoldierEquipIntensify;
    //public UIButton Btn_SoldierEquipUpgrade;
    //public UISprite Spt_SoldierEquipInfoIcon;
    //public UISprite Spt_SoldierEquipInfoQuality;
    //public UILabel Lbl_SoldierEquipInfoName;
    //public UILabel Lbl_SoldierEquipLevel;
    //public UIGrid Grd_StarLevelGroup;
    //public UISprite Spt_SoldierEquipStar;
    //public UISprite Spt_SoldierEquipInfoBg;
    //public UIGrid Grid_AttGroup;
    //public GameObject Gobj_EquipAttComp;
    public TweenScale Anim_TScale;

    public void Initialize()
    {
        _uiRoot = GameObject.Find("UISystem/UICamera/Anchor/Panel/BackPackView");
        Anim_TScale = _uiRoot.transform.FindChild("Anim").gameObject.GetComponent<TweenScale>();
        Btn_CloseView = _uiRoot.transform.FindChild("Anim/CloseView").gameObject.GetComponent<UIButton>();
        Gobj_ConsumeInfo = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo").gameObject;
        Spt_ConsumeQuality = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ItemInfoGroup/ItemBaseComp/ConsumeQuality").gameObject.GetComponent<UISprite>();
        Spt_ConsumeItemBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ItemInfoGroup/ItemBaseComp/BgSprite").gameObject.GetComponent<UISprite>();
        Tex_ConsumeIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ItemInfoGroup/ItemBaseComp/ConsumeIcon").gameObject.GetComponent<UISprite>();
        Lbl_ConsumeItemName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ItemInfoGroup/NameGroup/ConsumeItemName").gameObject.GetComponent<UILabel>();
        Lbl_ConsumeItemCount = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ItemInfoGroup/NameGroup/ConsumeItemCount").gameObject.GetComponent<UILabel>();
        Lbl_ConsumeDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/InfoGroup/ConsumeDesc").gameObject.GetComponent<UILabel>();
        Lbl_ConsumeItemPrice = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/SellGroup/ConsumeItemPrice").gameObject.GetComponent<UILabel>();
        Btn_ConsumeSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeSell").gameObject.GetComponent<UIButton>();
        Spt_ConsumeSellBtnBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeSell/Bg").gameObject.GetComponent<UISprite>();
        Lbl_ConsumeSellBtnLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeSell/Label").gameObject.GetComponent<UILabel>();
        Btn_ConsumeUse = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeUse").gameObject.GetComponent<UIButton>();
        Spt_ConsumeUseBtnBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeUse/Bg").gameObject.GetComponent<UISprite>();
        Spt_ConsumeUseBtnLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeUse/Label").gameObject.GetComponent<UILabel>();
        Btn_ConsumeGet = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeGet").gameObject.GetComponent<UIButton>();
        Spt_ConsumeGetBtnBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeGet/Bg").gameObject.GetComponent<UISprite>();
        Lbl_ConsumeGetBtnLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ConsumeInfo/ButtonGroup/ConsumeGet/Label").gameObject.GetComponent<UILabel>();

        Gobj_MaterialInfo = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo").gameObject;
        Spt_MaterialQuality = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ItemInfoGroup/ItemBaseComp/MaterialQuality").gameObject.GetComponent<UISprite>();
        Spt_MaterialItemBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ItemInfoGroup/ItemBaseComp/BgSprite").gameObject.GetComponent<UISprite>();
        Tex_MaterialIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ItemInfoGroup/ItemBaseComp/MaterialIcon").gameObject.GetComponent<UISprite>();
        Lbl_MaterialItemName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ItemInfoGroup/NameGroup/MaterialItemName").gameObject.GetComponent<UILabel>();
        Lbl_MaterialItemCount = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ItemInfoGroup/NameGroup/MaterialItemCount").gameObject.GetComponent<UILabel>();
        Lbl_MaterialDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/InfoGroup/MaterialDesc").gameObject.GetComponent<UILabel>();
        Lbl_ComposeTip = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/InfoGroup/ComposeTip").gameObject.GetComponent<UILabel>();
        Gobj_ComposeCostGroup = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/InfoGroup/ComposeCostGroup").gameObject;
        Lbl_ComposeTitle = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/InfoGroup/ComposeCostGroup/ComposeTitle").gameObject.GetComponent<UILabel>();
        Lbl_MaterialComposeCost = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/InfoGroup/ComposeCostGroup/ComposeCost").gameObject.GetComponent<UILabel>();
        Lbl_MaterialItemPrice = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/SellGroup/MaterialItemPrice").gameObject.GetComponent<UILabel>();

        Btn_MaterialSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/MaterialSell").gameObject.GetComponent<UIButton>();
        Spt_MaterialSellBtnBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/MaterialSell/BG").gameObject.GetComponent<UISprite>();
        Lbl_MaterialSellBtnLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/MaterialSell/Label").gameObject.GetComponent<UILabel>();

        Btn_ChipCompose = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/ChipCompose").gameObject.GetComponent<UIButton>();
        Btn_GetMaterial = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/GetMaterial").gameObject.GetComponent<UIButton>();
        Spt_GetMaterialBtnBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/GetMaterial/Bg").gameObject.GetComponent<UISprite>();
        Spt_GetMaterialBtnLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_MaterialInfo/ButtonGroup/GetMaterial/Label").gameObject.GetComponent<UILabel>();
        Lbl_SelectItemTip = _uiRoot.transform.FindChild("Anim/LeftGroup/SelectItemTip").gameObject.GetComponent<UILabel>();
        Gobj_ArtifactInfo = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo").gameObject;
        Spt_ArtifactInfoQuality = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/ArtifactInfoQuality").gameObject.GetComponent<UISprite>();
        Tex_ArtifactInfoIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/ArtifactInfoIcon").gameObject.GetComponent<UISprite>();
        Spt_ArtifactItemBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/ItemBaseComp/BgSprite").gameObject.GetComponent<UISprite>();
        Lbl_ArtifactItemName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/NameGroup/ArtifactItemName").gameObject.GetComponent<UILabel>();
        Lbl_ArtifactItemLevel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ItemInfoGroup/NameGroup/ArtifactItemLevel").gameObject.GetComponent<UILabel>();
        Lbl_ArtifactDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactDesc").gameObject.GetComponent<UILabel>();
        Btn_ArtifactSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell").gameObject.GetComponent<UIButton>();
        Spt_BtnArtifactSellBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell/Bg").gameObject.GetComponent<UISprite>();
        Spt_BtnArtifactSellLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactSell/Label").gameObject.GetComponent<UILabel>();
        Btn_ArtifactStrengthen = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen").gameObject.GetComponent<UIButton>();
        Btn_ArtifactStrengthenBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen/Bg").gameObject.GetComponent<UISprite>();
        Btn_ArtifactStrengthenLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/ButtonGroup/ArtifactStrengthen/Label").gameObject.GetComponent<UILabel>();

        Table_ArtifactAtt = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt").gameObject.GetComponent<UITable>();
        Lbl_UnlockDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/UnlockDesc").gameObject.GetComponent<UILabel>();
        Grd_ArtifactAttGroup = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactAttGroup").gameObject.GetComponent<UIGrid>();
        Gobj_ArtifactAttComp = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactAttGroup/ArtifactAttComp").gameObject;
        Lbl_ArtifactDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_ArtifactInfo/InfoGroup/ArtifactAtt/ArtifactDesc").gameObject.GetComponent<UILabel>();

        Gobj_SoldierEquipInfo = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo").gameObject;
        Spt_SoldierEquipQuality = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ItemInfoGroup/ItemBaseComp/SoldierEquipQuality").gameObject.GetComponent<UISprite>();
        Spt_SoldierEquipItemBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ItemInfoGroup/ItemBaseComp/BgSprite").gameObject.GetComponent<UISprite>();
        Tex_SoldierEquipIcon = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ItemInfoGroup/ItemBaseComp/SoldierEquipIcon").gameObject.GetComponent<UISprite>();
        Lbl_SoldierEquipName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ItemInfoGroup/NameGroup/SoldierEquipName").gameObject.GetComponent<UILabel>();
        Lbl_SoldierEquipInfoLevel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ItemInfoGroup/NameGroup/SoldierEquipCount").gameObject.GetComponent<UILabel>();
        ScrView_SuitScrollView = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView").gameObject.GetComponent<UIScrollView>();
        UITable_SuitTable = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable").gameObject.GetComponent<UITable>();
        Gobj_SuitNameGroup = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable/SuitNameGroup").gameObject;
        Lbl_SuitName = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable/SuitNameGroup/Label").gameObject.GetComponent<UILabel>();
        Lbl_SuitEquipDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable/SuitEquipDesc").gameObject.GetComponent<UILabel>();
        Spt_DividingLine = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable/DividingLine").gameObject.GetComponent<UISprite>();
        Lbl_SoldierEquipDesc = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/InfoGroup/SuitScrollView/SuitTable/SoldierEquipDesc").gameObject.GetComponent<UILabel>();
        Lbl_SoldierEquipPrice = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/SellGroup/SoldierEquipPrice").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipSell = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipSell").gameObject.GetComponent<UIButton>();
        Spt_BtnSoldierEquipSellBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipSell/Bg").gameObject.GetComponent<UISprite>();
        Spt_BtnSoldierEquipSellLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipSell/Label").gameObject.GetComponent<UILabel>();
        Btn_SoldierEquipDetail = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipDetail").gameObject.GetComponent<UIButton>();
        Spt_BtnSoldierEquipDetailBg = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipDetail/Bg").gameObject.GetComponent<UISprite>();
        Spt_BtnSoldierEquipDetailLabel = _uiRoot.transform.FindChild("Anim/LeftGroup/gobj_SoldierEquipInfo/ButtonGroup/SoldierEquipDetail/Label").gameObject.GetComponent<UILabel>();
        Grd_BtnFuncGroup = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup").gameObject.GetComponent<UIGrid>();
        Btn_ConsumeType = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/ConsumeType").gameObject.GetComponent<UIButton>();
        Btn_MaterialType = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/MaterailType").gameObject.GetComponent<UIButton>();
        Btn_ArtifactType = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/ArtifactType").gameObject.GetComponent<UIButton>();
        Btn_SoldierEquipType = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/SoldierEquipType").gameObject.GetComponent<UIButton>();
        Btn_AddSoldierEquip = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/AddSoldierEquip").gameObject.GetComponent<UIButton>();
        Lbl_BtnAddSoldierEquipLabel = _uiRoot.transform.FindChild("Anim/RightGroup/BtnFuncGroup/AddSoldierEquip/Label").gameObject.GetComponent<UILabel>();
        ScrView_ConsumeScroll = _uiRoot.transform.FindChild("Anim/RightGroup/ConsumeScroll").gameObject.GetComponent<UIScrollView>();
        Grd_ConsumeGrid = _uiRoot.transform.FindChild("Anim/RightGroup/ConsumeScroll/ConsumeGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_ConsumeGrid = _uiRoot.transform.FindChild("Anim/RightGroup/ConsumeScroll/ConsumeGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_ConsumeComp = _uiRoot.transform.FindChild("Anim/RightGroup/ConsumeScroll/gobj_ConsumeComp").gameObject;
        ScrView_MaterialScroll = _uiRoot.transform.FindChild("Anim/RightGroup/MaterialScroll").gameObject.GetComponent<UIScrollView>();
        Grd_MaterialGrid = _uiRoot.transform.FindChild("Anim/RightGroup/MaterialScroll/MaterialGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_MaterialGrid = _uiRoot.transform.FindChild("Anim/RightGroup/MaterialScroll/MaterialGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_MaterialComp = _uiRoot.transform.FindChild("Anim/RightGroup/MaterialScroll/gobj_MaterialComp").gameObject;
        ScrView_ArtifactScroll = _uiRoot.transform.FindChild("Anim/RightGroup/ArtifactScroll").gameObject.GetComponent<UIScrollView>();
        Grd_ArtifactGrid = _uiRoot.transform.FindChild("Anim/RightGroup/ArtifactScroll/ArtifactGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_ArtifactGrid = _uiRoot.transform.FindChild("Anim/RightGroup/ArtifactScroll/ArtifactGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_ArtifactComp = _uiRoot.transform.FindChild("Anim/RightGroup/ArtifactScroll/gobj_ArtifactComp").gameObject;
        ScrView_SoldierEquipScroll = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll").gameObject.GetComponent<UIScrollView>();
        Grd_SoldierEquipGrid = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/SoldierEquipGrid").gameObject.GetComponent<UIGrid>();
        UIWrapContent_SoldierEquipGrid = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/SoldierEquipGrid").gameObject.GetComponent<UIWrapContent>();
        Gobj_SoldierEquipComp = _uiRoot.transform.FindChild("Anim/RightGroup/SoldierEquipScroll/gobj_SoldierEquipComp").gameObject;
        Lbl_NoItemTip = _uiRoot.transform.FindChild("Anim/RightGroup/NoItemTip").gameObject.GetComponent<UILabel>();
        //Gobj_SoldierEquipDetailInfo = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo").gameObject;
        //Btn_CloseSoldierEquipInfo = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/CloseSoldierEquipInfo").gameObject.GetComponent<UIButton>();
        //Btn_SoldierEquipObtain = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/ButtomGroup/SoldierEquipObtain").gameObject.GetComponent<UIButton>();
        //Btn_SoldierEquipIntensify = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/ButtomGroup/SoldierEquipIntensify").gameObject.GetComponent<UIButton>();
        //Btn_SoldierEquipUpgrade = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/ButtomGroup/SoldierEquipUpgrade").gameObject.GetComponent<UIButton>();
        //Spt_SoldierEquipInfoIcon = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/InfoGroup/ItemComp/SoldierEquipInfoIcon").gameObject.GetComponent<UISprite>();
        //Spt_SoldierEquipInfoQuality = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/InfoGroup/ItemComp/SoldierEquipInfoQuality").gameObject.GetComponent<UISprite>();
        //Lbl_SoldierEquipInfoName = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/InfoGroup/ItemComp/SoldierEquipInfoName").gameObject.GetComponent<UILabel>();
        //Grid_AttGroup = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/AttributeComparisonGroup/AttGroup").gameObject.GetComponent<UIGrid>();
        //Gobj_EquipAttComp = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/AttributeComparisonGroup/AttGroup/EquipAttComp").gameObject;
        //Lbl_SoldierEquipLevel = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/SoldierEquipLevel").gameObject.GetComponent<UILabel>();
        //Grd_StarLevelGroup = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/StarLevelGroup").gameObject.GetComponent<UIGrid>();
        //Spt_SoldierEquipStar = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/StarLevelGroup/SoldierEquipStar").gameObject.GetComponent<UISprite>();
        //Spt_SoldierEquipInfoBg = _uiRoot.transform.FindChild("Anim/gobj_SoldierEquipDetailInfo/InfoGroup/ItemComp/SoldierEquipInfoBg").gameObject.GetComponent<UISprite>();
        Gobj_SoldierEquipGrid = _uiRoot.transform.FindChild("Anim/RightGroup/gobj_SoldierEquipCount").gameObject;
        Lbl_SoldierEquipGridCount = _uiRoot.transform.FindChild("Anim/RightGroup/gobj_SoldierEquipCount/SoldierEquipCount").gameObject.GetComponent<UILabel>();
        //SetLabelValues();
    }

    public void SetLabelValues()
    {
        Lbl_ConsumeItemName.text = string.Empty;
        Lbl_ConsumeItemCount.text = string.Empty;
        Lbl_ConsumeDesc.text = "";
        Lbl_MaterialItemName.text = string.Empty;
        Lbl_MaterialItemCount.text = string.Empty;
        Lbl_MaterialDesc.text = string.Empty;
        Lbl_ComposeTip.text = string.Empty;
        Lbl_ComposeTitle.text = "合成花费:";
        Lbl_MaterialComposeCost.text = string.Empty;
        Lbl_SelectItemTip.text = "请选择一个物品";
        Lbl_ArtifactItemName.text = string.Empty;
        Lbl_ArtifactItemLevel.text = string.Empty;
        Lbl_ArtifactDesc.text = string.Empty;
        Lbl_SoldierEquipName.text = string.Empty;
        Lbl_SoldierEquipInfoLevel.text = string.Empty;
        Lbl_SoldierEquipDesc.text = string.Empty;
        Lbl_SoldierEquipPrice.text = "99";
        Lbl_BtnAddSoldierEquipLabel.text = "购买格子";
        Lbl_NoItemTip.text = "当前无道具";
        Lbl_SoldierEquipName.text = "";
        //Lbl_SoldierEquipInfoName.text = "";
        //Lbl_SoldierEquipLevel.text = "";
    }

    public void Uninitialize()
    {

    }

    public void SetVisible(bool isVisible)
    {
        _uiRoot.SetActive(isVisible);
    }


}


