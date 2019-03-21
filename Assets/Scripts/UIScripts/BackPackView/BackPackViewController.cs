using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;

public class BackPackViewController : UIBase
{
    public BackPackView view;
    private class NormalItemData
    {
        public fogs.proto.msg.Item itemPOD;
        public ItemInfo itemInfo;
    }

    private BPFuncTypeComponent consumeTypeComp;
    private BPFuncTypeComponent materialTypeComp;
    private BPFuncTypeComponent artifactTypeComp;
    private BPFuncTypeComponent soldierEquipTypeComp;

    private List<BPArtifactAttComponent> artifactAttList;
    private List<BPNormalItemComponent> consumeItem_dic;
    private List<BPNormalItemComponent> materialItem_dic;
    private List<BPArtifactComponent> artifactItem_dic;
    private List<BPSoldierEquipComponent> soldierEquipItem_dic;

    private List<fogs.proto.msg.Item> consumeItemList;  //消耗品
    private List<fogs.proto.msg.Item> materialItemList;  //材料以及碎片
    private List<Weapon> artifactList;                   //神器
    private List<Weapon> soldierEquipList;               //武将装备
    private NormalItemData currentConsumeData;
    private NormalItemData currentMaterialData;
    private Weapon currentArtifactData;
    private Weapon currentSoldierEquipData;
    private bool isOpening = false;
    private bool isMatrialOpening = false;
    private enum BackPackFuncType
    {
        None,
        Consume = 1,
        Material = 2,
        Artifact = 3,
        SoldierEquip = 4,
    }
    private BackPackFuncType currentFuncType;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new BackPackView();
            view.Initialize();
            BtnEventBinding();
        }
        isOpening = false;
        isMatrialOpening = false;
        //view.Gobj_SoldierEquipDetailInfo.SetActive(false);
        if (consumeItem_dic == null)
            consumeItem_dic = new List<BPNormalItemComponent>();
        if (materialItem_dic == null)
            materialItem_dic = new List<BPNormalItemComponent>();
        if (artifactItem_dic == null)
            artifactItem_dic = new List<BPArtifactComponent>();
        if (soldierEquipItem_dic == null)
            soldierEquipItem_dic = new List<BPSoldierEquipComponent>();
        if (artifactAttList == null)
        {
            artifactAttList = new List<BPArtifactAttComponent>();
        }
        InitView();
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_TOPFUNC);
        currentFuncType = BackPackFuncType.Consume;
        //UpdateViewInfo();
        //PlayOpenBackPackAnim();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
    }

    private void InitView()
    {
        view.Gobj_ConsumeComp.SetActive(false);
        view.Gobj_ArtifactComp.SetActive(false);
        view.Gobj_MaterialComp.SetActive(false);
        view.Gobj_SoldierEquipComp.SetActive(false);
        view.Gobj_ArtifactAttComp.SetActive(false);
        view.Gobj_SoldierEquipGrid.SetActive(false);
        if (consumeTypeComp == null)
            consumeTypeComp = new BPFuncTypeComponent(view.Btn_ConsumeType.gameObject);
        if (materialTypeComp == null)
            materialTypeComp = new BPFuncTypeComponent(view.Btn_MaterialType.gameObject);
        if (artifactTypeComp == null)
            artifactTypeComp = new BPFuncTypeComponent(view.Btn_ArtifactType.gameObject);
        if (soldierEquipTypeComp == null)
            soldierEquipTypeComp = new BPFuncTypeComponent(view.Btn_SoldierEquipType.gameObject);
        if (consumeItemList == null)
            consumeItemList = new List<Item>();
        else
            consumeItemList.Clear();
        if (materialItemList == null)
            materialItemList = new List<Item>();
        else
            materialItemList.Clear();
        if (artifactList == null)
            artifactList = new List<Weapon>();
        else
            artifactList.Clear();
        if (soldierEquipList == null)
            soldierEquipList = new List<Weapon>();
        view.UIWrapContent_ConsumeGrid.onInitializeItem = UpdateConsumeItemInfo;
        view.UIWrapContent_MaterialGrid.onInitializeItem = UpdateMaterialItemInfo;
        view.UIWrapContent_ArtifactGrid.onInitializeItem = UpdateArtifactItemInfo;
        view.UIWrapContent_SoldierEquipGrid.onInitializeItem = UpdateSoldierEquipItemInfo;
        PlayerData.Instance.UpdatePlayerItemsEvent += OnUpdatePlayerItemsEvent;

        //view.Spt_SoldierEquipStar.gameObject.SetActive(false);
        //view.Gobj_EquipAttComp.SetActive(false);
    }

    #region Update Event

    #region 主面板

    private void UpdateConsumeItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_ConsumeGrid.enabled) return;
        if (realIndex >= consumeItemList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        BPNormalItemComponent comp = consumeItem_dic[wrapIndex];
        comp.UpdateInfo(consumeItemList[realIndex]);
        comp.IsSelect = comp.ItemPOD.id.Equals(currentConsumeData.itemPOD.id);
    }

    private void UpdateMaterialItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_MaterialGrid.enabled) return;
        if (realIndex >= materialItemList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        BPNormalItemComponent comp = materialItem_dic[wrapIndex];
        comp.UpdateInfo(materialItemList[realIndex]);
        comp.IsSelect = comp.ItemPOD.id.Equals(currentMaterialData.itemPOD.id);
    }
    private void UpdateArtifactItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_ArtifactGrid.enabled) return;
        if (realIndex >= artifactList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        BPArtifactComponent comp = artifactItem_dic[wrapIndex];
        if (comp == null) return;
        Weapon soldierEquip = artifactList[realIndex];
        comp.UpdateInfo(soldierEquip);
        if (!soldierEquip.IsLock)
        {
            comp.IsSelect = comp.ArtifactPOD.uId.Equals(currentArtifactData.uId);
        }
        else
        {
            comp.IsSelect = comp.ArtifactPOD.Att.id.Equals(currentArtifactData.Att.id);
        }
    }
    private void UpdateSoldierEquipItemInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (view.UIWrapContent_SoldierEquipGrid.enabled == false) return;
        if (realIndex >= soldierEquipList.Count)
        {
            go.SetActive(false);
        }
        else
        {
            go.SetActive(true);
            BPSoldierEquipComponent comp = soldierEquipItem_dic[wrapIndex];
            Weapon weapon = soldierEquipList[realIndex];
            comp.UpdateInfo(weapon);
            comp.IsSelect = weapon.uId.Equals(currentSoldierEquipData.uId);
        }
    }

    private void OnUpdatePlayerItemsEvent()
    {
        UpdateViewInfo();
    }

    private void SetDefaultItemData()
    {
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    if (consumeItemList == null || consumeItemList.Count == 0)
                    {
                        currentConsumeData = null;
                    }
                    else
                    {
                        if (currentConsumeData != null)
                        {
                            NormalItemData data = new NormalItemData();
                            Item t_item = consumeItemList.Find((item) => { return item.id == currentConsumeData.itemPOD.id; });
                            if (t_item != null)
                            {
                                data.itemPOD = t_item;

                            }
                            else
                            {
                                data.itemPOD = consumeItemList[0];
                            }
                            data.itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(data.itemPOD.id);
                            currentConsumeData = data;
                        }
                        else
                        {
                            NormalItemData data = new NormalItemData();
                            data.itemPOD = consumeItemList[0];
                            data.itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(data.itemPOD.id);
                            currentConsumeData = data;
                        }
                    }
                } break;
            case BackPackFuncType.Material:
                {
                    if (materialItemList == null || materialItemList.Count == 0)
                    {
                        currentMaterialData = null;
                    }
                    else
                    {
                        if (currentMaterialData != null)
                        {
                            NormalItemData data = new NormalItemData();
                            Item t_item = materialItemList.Find((item) => { return item.id == currentMaterialData.itemPOD.id; });
                            if (t_item != null)
                            {
                                data.itemPOD = t_item;

                            }
                            else
                            {
                                data.itemPOD = materialItemList[0];
                            }
                            data.itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(data.itemPOD.id);
                            currentMaterialData = data;
                        }
                        else
                        {
                            NormalItemData data = new NormalItemData();
                            data.itemPOD = materialItemList[0];
                            data.itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(data.itemPOD.id);
                            currentMaterialData = data;
                        }
                    }
                } break;
            case BackPackFuncType.Artifact:
                {
                    if (artifactList == null || artifactList.Count == 0)
                    {
                        currentArtifactData = null;
                    }
                    else
                    {
                        if (currentArtifactData == null)
                        {
                            currentArtifactData = artifactList[0];
                        }
                        else
                        {
                            if (!currentArtifactData.IsLock)
                            {
                                Weapon t_artifact = PlayerData.Instance._WeaponDepot.FindByUid(currentArtifactData.uId);
                                if (t_artifact == null)
                                {
                                    currentArtifactData = artifactList[0];
                                }
                                else
                                {
                                    currentArtifactData = t_artifact;
                                }
                            }
                            else
                            {
                                Weapon weapon = artifactList.Find((tmp) =>
                                {
                                    if (tmp == null)
                                        return false;
                                    if (!tmp.IsLock == false)
                                    {
                                        return tmp.Att.id == currentArtifactData.Att.id;
                                    }
                                    return false;
                                });
                                if (weapon == null)
                                {
                                    currentArtifactData = artifactList[0];
                                }
                                else
                                {
                                    currentArtifactData = weapon;
                                }
                            }
                        }
                    }
                } break;
            case BackPackFuncType.SoldierEquip:
                {
                    if (soldierEquipList == null || soldierEquipList.Count == 0)
                    {
                        currentSoldierEquipData = null;
                    }
                    else
                    {
                        if (currentSoldierEquipData != null)
                        {
                            Weapon t_SoldierEquip = PlayerData.Instance._SoldierEquip.FindByUid(currentSoldierEquipData.uId);
                            if (t_SoldierEquip == null)
                            {
                                currentSoldierEquipData = soldierEquipList[0];
                            }
                            else
                            {
                                currentSoldierEquipData = t_SoldierEquip;
                            }
                        }
                        else
                        {
                            currentSoldierEquipData = soldierEquipList[0];
                        }
                    }
                } break;
        }
    }

    private void UpdateSelectItem()
    {
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    BPNormalItemComponent comp = null;
                    for (int i = 0; i < consumeItem_dic.Count; i++)
                    {
                        BPNormalItemComponent tmpComp = consumeItem_dic[i];
                        tmpComp.IsSelect = false;
                        if (tmpComp.mRootObject.activeSelf && tmpComp.ItemPOD != null)
                        {
                            if (tmpComp.ItemPOD.id == currentConsumeData.itemPOD.id)
                            {
                                comp = consumeItem_dic[i];
                            }
                        }
                    }
                    if (comp != null)
                    {
                        comp.IsSelect = true;
                    }
                }
                break;
            case BackPackFuncType.Material:
                {
                    BPNormalItemComponent comp = null;
                    for (int i = 0; i < materialItem_dic.Count; i++)
                    {
                        BPNormalItemComponent tmpComp = materialItem_dic[i];
                        tmpComp.IsSelect = false;
                        if (tmpComp.mRootObject.activeSelf && tmpComp.ItemPOD != null)
                        {
                            if (tmpComp.ItemPOD.id == currentMaterialData.itemPOD.id)
                            {
                                comp = materialItem_dic[i];
                            }
                        }
                    }
                    if (comp != null) comp.IsSelect = true;
                }
                break;
            case BackPackFuncType.Artifact:
                {
                    BPArtifactComponent comp = null;
                    for (int i = 0; i < artifactItem_dic.Count; i++)
                    {
                        BPArtifactComponent tmpComp = artifactItem_dic[i];
                        tmpComp.IsSelect = false;
                        if (!tmpComp.ArtifactPOD.IsLock)
                        {
                            if (tmpComp.ArtifactPOD.uId == currentArtifactData.uId)
                            {
                                comp = tmpComp;
                            }
                        }
                        else
                        {
                            if (tmpComp.ArtifactPOD.Att.id == currentArtifactData.Att.id)
                            {
                                comp = tmpComp;
                            }
                        }
                    }
                    if (comp != null) comp.IsSelect = true;
                }
                break;
            case BackPackFuncType.SoldierEquip:
                {
                    BPSoldierEquipComponent comp = null;
                    for (int i = 0; i < soldierEquipItem_dic.Count; i++)
                    {
                        BPSoldierEquipComponent tmpComp = soldierEquipItem_dic[i];
                        tmpComp.IsSelect = false;
                        if (tmpComp.mRootObject.activeSelf && tmpComp.SoldierEquipPOD != null)
                        {
                            if (tmpComp.SoldierEquipPOD.uId == currentSoldierEquipData.uId)
                            {
                                comp = soldierEquipItem_dic[i];
                            }
                        }
                    }
                    if (comp != null) comp.IsSelect = true;
                }
                break;
        }
    }

    private void UpdateLeftInfoGroup()
    {
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    if (currentConsumeData == null)
                    {
                        ClearLeftGroup();
                        return;
                    }
                    view.Lbl_SelectItemTip.enabled = false;
                    UpdateConsumeButtonStatus();
                    ItemInfo _itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(currentConsumeData.itemPOD.id);
                    if (_itemInfo == null) return;
                    CommonFunction.SetQualitySprite(view.Spt_ConsumeQuality, _itemInfo.quality, view.Spt_ConsumeItemBg);
                    CommonFunction.SetSpriteName(view.Tex_ConsumeIcon, _itemInfo.icon);
                    view.Lbl_ConsumeItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, currentConsumeData.itemPOD.num);
                    view.Lbl_ConsumeItemName.text = _itemInfo.name;
                    view.Lbl_ConsumeItemPrice.text = _itemInfo.sell_price.ToString();
                    view.Lbl_ConsumeDesc.text = CommonFunction.ReplaceEscapeChar(_itemInfo.desc);
                    if (_itemInfo.sell_type == 0)
                    {
                        view.Btn_ConsumeSell.collider.enabled = false;
                        CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnBg, true);
                        CommonFunction.SetUILabelColor(view.Lbl_ConsumeSellBtnLabel, false);
                        //view.Lbl_ConsumeSellBtnLabel.color = new Color(54, 54, 54,255);
                        //CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnFg, true);
                        CommonFunction.UpdateWidgetGray(view.Spt_ConsumeUseBtnBg, false);
                        CommonFunction.SetUILabelColor(view.Spt_ConsumeUseBtnLabel, true);
                    }
                    else
                    {
                        view.Btn_ConsumeSell.collider.enabled = true;
                        CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnBg, false);
                        CommonFunction.SetUILabelColor(view.Lbl_ConsumeSellBtnLabel, true);

                        CommonFunction.UpdateWidgetGray(view.Spt_ConsumeUseBtnBg, true);
                        CommonFunction.SetUILabelColor(view.Spt_ConsumeUseBtnLabel, false);
                        //CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnFg, false);
                    }

                    view.Btn_ConsumeGet.collider.enabled = true;
                    CommonFunction.UpdateWidgetGray(view.Spt_ConsumeGetBtnBg, false);
                    CommonFunction.SetUILabelColor(view.Lbl_ConsumeGetBtnLabel, true);
                } break;
            case BackPackFuncType.Material:
                {
                    if (currentMaterialData == null)
                    {
                        ClearLeftGroup();
                        return;
                    }
                    view.Lbl_SelectItemTip.enabled = false;
                    UpdateButtonStatus();
                    ItemInfo _itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(currentMaterialData.itemPOD.id);
                    if (_itemInfo == null) return;
                    CommonFunction.SetQualitySprite(view.Spt_MaterialQuality, _itemInfo.quality, view.Spt_MaterialItemBg);
                    CommonFunction.SetSpriteName(view.Tex_MaterialIcon, _itemInfo.icon);
                    view.Lbl_MaterialItemName.text = _itemInfo.name;
                    view.Lbl_MaterialItemPrice.text = _itemInfo.sell_price.ToString();
                    view.Lbl_MaterialItemCount.text = string.Format(ConstString.BACKPACK_HADITEMCOUNT, currentMaterialData.itemPOD.num);
                    view.Lbl_MaterialDesc.text = CommonFunction.ReplaceEscapeChar(_itemInfo.desc);
                    if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.EquipChip)
                    {
                        view.Gobj_ComposeCostGroup.SetActive(true);
                        view.Lbl_MaterialComposeCost.text = _itemInfo.cost.ToString();
                        string chipDesc = string.Format(ConstString.BACKPACK_COMPOSECHIPCOUNT, currentMaterialData.itemPOD.num, currentMaterialData.itemInfo.compound_count);
                        CommonFunction.AddNewLine(view.Lbl_MaterialDesc, chipDesc);
                    }
                    else
                    {
                        view.Gobj_ComposeCostGroup.SetActive(false);
                    }
                    if (_itemInfo.sell_type == 0)
                    {
                        view.Gobj_ComposeCostGroup.SetActive(true);
                        view.Btn_MaterialSell.collider.enabled = false;
                        CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnBg, true);
                        CommonFunction.SetUILabelColor(view.Lbl_MaterialSellBtnLabel, false);
                        //CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnFg, true);
                    }
                    else
                    {
                        view.Btn_MaterialSell.collider.enabled = true;
                        CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnBg, false);
                        CommonFunction.SetUILabelColor(view.Lbl_MaterialSellBtnLabel, true);
                        //CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnFg, false);
                    }
                    view.Btn_GetMaterial.collider.enabled = true;
                    CommonFunction.UpdateWidgetGray(view.Spt_GetMaterialBtnBg, false);
                    CommonFunction.SetUILabelColor(view.Spt_GetMaterialBtnLabel, true);
                } break;
            case BackPackFuncType.Artifact:
                {
                    if (currentArtifactData == null)
                    {
                        ClearLeftGroup();
                        return;
                    }
                    view.Lbl_SelectItemTip.enabled = false;
                    view.Btn_ArtifactSell.collider.enabled = false;
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnArtifactSellBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_BtnArtifactSellLabel, false);
                    view.Btn_ArtifactStrengthen.collider.enabled = !currentArtifactData.IsLock;
                    CommonFunction.UpdateWidgetGray(view.Btn_ArtifactStrengthenBg, currentArtifactData.IsLock);
                    if (currentArtifactData.IsLock)
                    {
                        CommonFunction.SetUILabelColor(view.Btn_ArtifactStrengthenLabel, false);
                    }
                    else
                    {
                        CommonFunction.SetUILabelColor(view.Btn_ArtifactStrengthenLabel, true);
                    }
                    CommonFunction.SetQualitySprite(view.Spt_ArtifactInfoQuality, currentArtifactData.Att.quality, view.Spt_ArtifactItemBg);
                    CommonFunction.SetSpriteName(view.Tex_ArtifactInfoIcon, currentArtifactData.Att.icon);
                    view.Lbl_ArtifactItemLevel.text = string.Format(ConstString.BACKPACK_SOLDIEREQUIPLEVEL, currentArtifactData.Level);
                    view.Lbl_ArtifactItemName.text = currentArtifactData.Att.name;
                    UpdateArtifactAtt();

                } break;
            case BackPackFuncType.SoldierEquip:
                {
                    if (currentSoldierEquipData == null)
                    {
                        ClearLeftGroup();
                        return;
                    }
                    view.Lbl_SelectItemTip.enabled = false;
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipSellBg, false);
                    //CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipSellLabel, false);
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipDetailBg, false);
                    //CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipDetailLabel, false);
                    CommonFunction.SetQualitySprite(view.Spt_SoldierEquipQuality, currentSoldierEquipData.Att.quality, view.Spt_SoldierEquipItemBg);
                    CommonFunction.SetSpriteName(view.Tex_SoldierEquipIcon, currentSoldierEquipData.Att.icon);
                    EquipCoordinatesInfo info = null;
                    if (currentSoldierEquipData.Att.CoordID != 0)
                    {
                        info = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(currentSoldierEquipData.Att.CoordID);
                    }
                    if (info == null)
                    {
                        view.Gobj_SuitNameGroup.SetActive(false);
                        view.Lbl_SuitEquipDesc.gameObject.SetActive(false);
                        view.Spt_DividingLine.gameObject.SetActive(false);
                    }
                    else
                    {
                        view.Gobj_SuitNameGroup.SetActive(true);
                        view.Lbl_SuitEquipDesc.gameObject.SetActive(true);
                        view.Spt_DividingLine.gameObject.SetActive(true);
                        view.Lbl_SuitName.text = info.name;
                        List<string> list = CommonFunction.GetDescByEquipCoordinatesConfig(info);
                        System.Text.StringBuilder sub = new System.Text.StringBuilder();
                        for (int i = 0; i < list.Count; i++)
                        {
                            if (i > 0)
                            {
                                sub.Append('\n');
                            }
                            sub.Append(list[i]);
                        }
                        view.Lbl_SuitEquipDesc.text = sub.ToString();
                    }
                    view.Lbl_SoldierEquipDesc.text = CommonFunction.ReplaceEscapeChar(currentSoldierEquipData.Att.descript);
                    view.UITable_SuitTable.Reposition();
                    view.ScrView_SuitScrollView.ResetPosition();
                    view.Lbl_SoldierEquipName.text = currentSoldierEquipData.Att.name;
                    view.Lbl_SoldierEquipPrice.text = currentSoldierEquipData.Att.price.ToString();
                    view.Lbl_SoldierEquipInfoLevel.text = string.Format(ConstString.BACKPACK_SOLDIEREQUIPLEVEL, currentSoldierEquipData.Level);
                } break;
        }
    }

    private void UpdateArtifactAtt()
    {
        if (currentArtifactData == null)
        {
            ClearLeftGroup();
            return;
        }
        view.Lbl_ArtifactDesc.text = CommonFunction.ReplaceEscapeChar(currentArtifactData.Att.descript);
        view.Lbl_UnlockDesc.text = ConstString.BACKPACK_LOCKARTIFACTATT;
        UpdateArtifactAttValue();
        view.Table_ArtifactAtt.repositionNow = true;
    }

    private void UpdateArtifactAttValue()
    {
        List<KeyValuePair<string, string>> attributeList = CommonFunction.GetWeaponAttributeDesc(currentArtifactData.InfoAttribute);
        int count = artifactAttList.Count;
        if (attributeList.Count < count)
        {
            for (int i = attributeList.Count; i < count; i++)
            {
                BPArtifactAttComponent comp = artifactAttList[i];
                if (comp != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < attributeList.Count; i++)
        {
            KeyValuePair<string, string> tmpInfo = attributeList[i];
            BPArtifactAttComponent comp = null;
            if (i < count)
            {
                comp = artifactAttList[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_ArtifactAttComp, view.Grd_ArtifactAttGroup.transform);
                comp = new BPArtifactAttComponent();
                comp.MyStart(go);
                artifactAttList.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_ArtifactAttGroup.Reposition();
    }


    private void ClearLeftGroup()
    {
        view.Lbl_SelectItemTip.enabled = true;
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    view.Spt_ConsumeQuality.spriteName = string.Empty;
                    view.Tex_ConsumeIcon.spriteName = string.Empty;
                    view.Spt_ConsumeItemBg.spriteName = string.Empty;
                    view.Btn_ConsumeUse.gameObject.SetActive(true);
                    view.Btn_ConsumeGet.gameObject.SetActive(false);
                    view.Lbl_ConsumeItemCount.text = string.Empty;
                    view.Lbl_ConsumeItemName.text = string.Empty;
                    view.Lbl_ConsumeItemPrice.text = string.Empty;
                    view.Lbl_ConsumeDesc.text = string.Empty;
                    view.Btn_ConsumeSell.collider.enabled = false;
                    CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnBg, true);
                    CommonFunction.SetUILabelColor(view.Lbl_ConsumeSellBtnLabel, false);
                    //CommonFunction.UpdateWidgetGray(view.Spt_ConsumeSellBtnFg, true);
                    CommonFunction.UpdateWidgetGray(view.Spt_ConsumeUseBtnBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_ConsumeUseBtnLabel, false);
                    view.Btn_ConsumeGet.collider.enabled = false;
                    CommonFunction.UpdateWidgetGray(view.Spt_ConsumeGetBtnBg, true);
                    CommonFunction.SetUILabelColor(view.Lbl_ConsumeGetBtnLabel, false);
                    //CommonFunction.UpdateWidgetGray(view.Spt_ConsumeUseBtnLabel, true);
                } break;
            case BackPackFuncType.Material:
                {
                    view.Btn_GetMaterial.gameObject.SetActive(true);
                    view.Btn_ChipCompose.gameObject.SetActive(false);
                    view.Spt_MaterialQuality.spriteName = string.Empty;
                    view.Spt_MaterialItemBg.spriteName = string.Empty;
                    view.Tex_MaterialIcon.spriteName = string.Empty;
                    view.Lbl_MaterialItemName.text = string.Empty;
                    view.Lbl_MaterialItemPrice.text = string.Empty;
                    view.Lbl_MaterialItemCount.text = string.Empty;
                    view.Lbl_MaterialComposeCost.text = string.Empty;
                    view.Lbl_MaterialDesc.text = string.Empty;
                    view.Btn_MaterialSell.collider.enabled = false;
                    CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnBg, true);
                    CommonFunction.SetUILabelColor(view.Lbl_MaterialSellBtnLabel, false);
                    //CommonFunction.UpdateWidgetGray(view.Spt_MaterialSellBtnFg, true);
                    //if (currentMaterialData.itemInfo.ishad_way)
                    //{
                    view.Btn_GetMaterial.collider.enabled = true;
                    CommonFunction.UpdateWidgetGray(view.Spt_GetMaterialBtnBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_GetMaterialBtnLabel, false);
                    //}
                    //else
                    //{
                    //    view.Btn_GetMaterial.collider.enabled = false;
                    //    CommonFunction.UpdateWidgetGray(view.Spt_GetMaterialBtnBg, false);
                    //    CommonFunction.SetUILabelColor(view.Spt_GetMaterialBtnLabel, true);
                    //}
                    //CommonFunction.UpdateWidgetGray(view.Spt_GetMaterialBtnLabel, true);
                } break;
            case BackPackFuncType.Artifact:
                {
                    view.Btn_ArtifactSell.collider.enabled = false;
                    view.Btn_ArtifactStrengthen.collider.enabled = false;
                    view.Spt_ArtifactInfoQuality.spriteName = string.Empty;
                    view.Tex_ArtifactInfoIcon.spriteName = string.Empty;
                    view.Spt_ArtifactItemBg.spriteName = string.Empty;
                    view.Lbl_ArtifactItemLevel.text = string.Empty;
                    view.Lbl_ArtifactDesc.text = string.Empty;
                    view.Lbl_ArtifactItemName.text = string.Empty;
                    CommonFunction.UpdateWidgetGray(view.Btn_ArtifactStrengthenBg, true);
                    CommonFunction.SetUILabelColor(view.Btn_ArtifactStrengthenLabel, false);
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnArtifactSellBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_BtnArtifactSellLabel, false);

                } break;
            case BackPackFuncType.SoldierEquip:
                {
                    view.Spt_SoldierEquipQuality.spriteName = string.Empty;
                    view.Tex_SoldierEquipIcon.spriteName = string.Empty;
                    view.Lbl_SoldierEquipDesc.text = string.Empty;
                    view.Lbl_SoldierEquipName.text = string.Empty;
                    view.Lbl_SoldierEquipInfoLevel.text = string.Empty;
                    view.Spt_SoldierEquipItemBg.spriteName = string.Empty;
                    view.Lbl_SoldierEquipPrice.text = string.Empty;
                    view.Gobj_SuitNameGroup.SetActive(false);
                    view.Lbl_SuitEquipDesc.gameObject.SetActive(false);
                    view.Spt_DividingLine.gameObject.SetActive(false);
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipSellBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_BtnSoldierEquipSellLabel, false);
                    CommonFunction.UpdateWidgetGray(view.Spt_BtnSoldierEquipDetailBg, true);
                    CommonFunction.SetUILabelColor(view.Spt_BtnSoldierEquipDetailLabel, false);
                } break;
        }
    }

    /// <summary>
    /// 更新View
    /// </summary>
    public void UpdateViewInfo()
    {
        UpdateViewInfo(currentFuncType);
    }

    public void UpdateViewInfo(int type)
    {
        currentFuncType = (BackPackFuncType)type;
        UpdateViewInfo(currentFuncType);
    }

    private void UpdateViewInfo(BackPackFuncType func)
    {
        currentFuncType = func;
        if (UISystem.Instance.TopFuncView != null)
        {
            UISystem.Instance.TopFuncView.UpdatePlayerCurrency();
        }
        UpdateFuncType();
        UpdateMaterialMark();
        UpdateArtifactMark();
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    UpdateConsumeData();
                    consumeTypeComp.IsMark = false;
                    SetDefaultItemData();
                    UpdateConsumeItems();
                    view.Gobj_SoldierEquipGrid.SetActive(false);
                }
                break;
            case BackPackFuncType.Material:
                {
                    UpdateMaterialData();
                    SetDefaultItemData();
                    UpdateMaterialItems();
                    view.Gobj_SoldierEquipGrid.SetActive(false);
                }
                break;
            case BackPackFuncType.Artifact:
                {
                    artifactList.Clear();
                    List<Weapon> list = PlayerData.Instance._WeaponDepot.GetLockAndUnlockList();
                    artifactList.AddRange(list);
                    //List<EquipAttributeInfo> allArtifactList = ConfigManager.Instance.mEquipData.GetAllArtifacts();
                    //if (allArtifactList == null)
                    //{
                    //    allArtifactList = new List<EquipAttributeInfo>();
                    //}
                    //for (int i = 0; i < PlayerData.Instance._WeaponDepot._weaponList.Count; i++)
                    //{
                    //    Weapon weapon = PlayerData.Instance._WeaponDepot._weaponList[i];
                    //    SoldierEquip soldierEquip = new SoldierEquip();
                    //    soldierEquip.weapon = weapon;
                    //    soldierEquip.isOwn = true;
                    //    artifactList.Add(soldierEquip);
                    //}
                    //List<SoldierEquip> withoutArtifactList = new List<SoldierEquip>();
                    //for (int i = 0; i < allArtifactList.Count; i++)
                    //{
                    //    EquipAttributeInfo info = allArtifactList[i];
                    //    SoldierEquip artifact = new SoldierEquip();
                    //    List<Weapon> tmpList = PlayerData.Instance._WeaponDepot.FindById(info.id);
                    //    if (tmpList == null || tmpList.Count == 0)  //如果未获得  则仅显示一组
                    //    {
                    //        if (info.quality == (int)ItemQualityEnum.White)  //仅显示白色品质
                    //        {
                    //            artifact.weapon = Weapon.createByID(info.id);
                    //            artifact.isOwn = false;
                    //            withoutArtifactList.Add(artifact);
                    //        }
                    //    }
                    //}
                    //artifactList.AddRange(withoutArtifactList);
                    SetDefaultItemData();
                    UpdateArtifactItems();
                    view.Gobj_SoldierEquipGrid.SetActive(false);
                }
                break;
            case BackPackFuncType.SoldierEquip:
                {
                    soldierEquipList.Clear();
                    for (int i = 0; i < PlayerData.Instance._SoldierEquip._weaponList.Count; i++)
                    {
                        Weapon weapon = PlayerData.Instance._SoldierEquip._weaponList[i];
                        if (weapon != null && !weapon.isEquiped)
                        {
                            soldierEquipList.Add(weapon);
                        }
                    }
                    soldierEquipTypeComp.IsMark = false;
                    SetDefaultItemData();
                    UpdateSoldierEquips();
                    UpdateEquipGrid();
                }
                break;
            default:
                break;
        }
    }

    private void UpdateConsumeData()
    {
        consumeItemList.Clear();
        for (int i = 0; i < PlayerData.Instance._PropBag.Count; i++)
        {
            Item item = PlayerData.Instance._PropBag[i];
            ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (itemInfo != null)
            {
                if (itemInfo.type != (int)ItemTypeEnum.SoldierChip && item.num != 0)
                {
                    consumeItemList.Add(item);
                }
            }
        }
        consumeItemList = CommonFunction.SortItemByQuality(consumeItemList);
    }

    private void UpdateMaterialData()
    {
        List<fogs.proto.msg.Item> materialList = new List<Item>();
        for (int i = 0; i < PlayerData.Instance._MaterialBag.Count; i++)
        {
            Item item = PlayerData.Instance._MaterialBag[i];
            ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (itemInfo != null)
            {
                if (itemInfo.type != (int)ItemTypeEnum.SoldierChip && item.num != 0)
                {
                    materialList.Add(item);
                }
            }
        }
        materialList = CommonFunction.SortItemByQuality(materialList);
        List<fogs.proto.msg.Item> chipList = new List<Item>();
        List<fogs.proto.msg.Item> able_ChipList = new List<fogs.proto.msg.Item>();
        for (int i = 0; i < PlayerData.Instance._ChipBag.Count; i++)
        {
            Item item = PlayerData.Instance._ChipBag[i];
            ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (itemInfo != null)
            {
                if (itemInfo.type != (int)ItemTypeEnum.SoldierChip && item.num != 0)
                {
                    if (item.num >= itemInfo.compound_count)
                    {
                        able_ChipList.Add(item);
                    }
                    else
                    {
                        chipList.Add(item);
                    }
                }
            }
        }
        able_ChipList = CommonFunction.SortItemByQuality(able_ChipList);
        chipList = CommonFunction.SortItemByQuality(chipList);
        materialItemList.Clear();
        materialItemList.AddRange(able_ChipList);
        materialItemList.AddRange(materialList);
        materialItemList.AddRange(chipList);
    }


    private void UpdateMaterialMark()
    {
        bool isCompose = false;
        for (int i = 0; i < PlayerData.Instance._ChipBag.Count; i++)
        {
            Item item = PlayerData.Instance._ChipBag[i];
            ItemInfo itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(item.id);
            if (itemInfo != null)
            {
                if (itemInfo.type != (int)ItemTypeEnum.SoldierChip && item.num != 0)
                {
                    if (item.num >= itemInfo.compound_count)
                    {
                        isCompose = true;
                        break;
                    }
                }
            }
        }
        materialTypeComp.IsMark = isCompose;
    }

    private void UpdateArtifactMark()
    {
        bool isIntensify = false;
        for (int i = 0; i < PlayerData.Instance._WeaponDepot._weaponList.Count; i++)
        {
            Weapon weapon = PlayerData.Instance._WeaponDepot._weaponList[i];
            if (weapon.enableStrong() == WeaponCheck.Ok)
            {
                isIntensify = true;
                break;
            }
        }
        artifactTypeComp.IsMark = isIntensify;
    }

    private void UpdateFuncType()
    {
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    view.Gobj_MaterialInfo.SetActive(false);
                    view.Gobj_ArtifactInfo.SetActive(false);
                    view.Gobj_SoldierEquipInfo.SetActive(false);
                    view.Gobj_ConsumeInfo.SetActive(true);
                    view.ScrView_MaterialScroll.gameObject.SetActive(false);
                    view.ScrView_ArtifactScroll.gameObject.SetActive(false);
                    view.ScrView_SoldierEquipScroll.gameObject.SetActive(false);
                    view.ScrView_ConsumeScroll.gameObject.SetActive(true);
                    consumeTypeComp.IsSelect = true;
                    materialTypeComp.IsSelect = false;
                    artifactTypeComp.IsSelect = false;
                    soldierEquipTypeComp.IsSelect = false;
                    view.Btn_AddSoldierEquip.gameObject.SetActive(false);
                }
                break;
            case BackPackFuncType.Artifact:
                {
                    view.Gobj_SoldierEquipInfo.SetActive(false);
                    view.Gobj_ConsumeInfo.SetActive(false);
                    view.Gobj_MaterialInfo.SetActive(false);
                    view.Gobj_ArtifactInfo.SetActive(true);
                    view.ScrView_MaterialScroll.gameObject.SetActive(false);
                    view.ScrView_SoldierEquipScroll.gameObject.SetActive(false);
                    view.ScrView_ConsumeScroll.gameObject.SetActive(false);
                    view.ScrView_ArtifactScroll.gameObject.SetActive(true);
                    artifactTypeComp.IsSelect = true;
                    consumeTypeComp.IsSelect = false;
                    materialTypeComp.IsSelect = false;
                    soldierEquipTypeComp.IsSelect = false;
                    view.Btn_AddSoldierEquip.gameObject.SetActive(false);
                }
                break;
            case BackPackFuncType.Material:
                {
                    view.Gobj_ArtifactInfo.SetActive(false);
                    view.Gobj_SoldierEquipInfo.SetActive(false);
                    view.Gobj_ConsumeInfo.SetActive(false);
                    view.Gobj_MaterialInfo.SetActive(true);
                    view.ScrView_ArtifactScroll.gameObject.SetActive(false);
                    view.ScrView_SoldierEquipScroll.gameObject.SetActive(false);
                    view.ScrView_ConsumeScroll.gameObject.SetActive(false);
                    view.ScrView_MaterialScroll.gameObject.SetActive(true);
                    materialTypeComp.IsSelect = true;
                    consumeTypeComp.IsSelect = false;
                    artifactTypeComp.IsSelect = false;
                    soldierEquipTypeComp.IsSelect = false;
                    view.Btn_AddSoldierEquip.gameObject.SetActive(false);
                }
                break;
            case BackPackFuncType.SoldierEquip:
                {
                    view.Gobj_ConsumeInfo.SetActive(false);
                    view.Gobj_MaterialInfo.SetActive(false);
                    view.Gobj_ArtifactInfo.SetActive(false);
                    view.Gobj_SoldierEquipInfo.SetActive(true);
                    view.ScrView_MaterialScroll.gameObject.SetActive(false);
                    view.ScrView_ArtifactScroll.gameObject.SetActive(false);
                    view.ScrView_ConsumeScroll.gameObject.SetActive(false);
                    view.ScrView_SoldierEquipScroll.gameObject.SetActive(true);
                    soldierEquipTypeComp.IsSelect = true;
                    consumeTypeComp.IsSelect = false;
                    materialTypeComp.IsSelect = false;
                    artifactTypeComp.IsSelect = false;

                    view.Btn_AddSoldierEquip.gameObject.SetActive(true);
                }
                break;
            default:
                break;
        }
    }

    private void UpdateConsumeItems()
    {
        if (consumeItemList.Count == 0)
        {
            ClearConsumeItems();
            view.Lbl_NoItemTip.enabled = true;
            view.Lbl_NoItemTip.text = ConstString.BACKPACK_NOITEMSTIP;
        }
        else
        {
            view.Lbl_NoItemTip.enabled = false;
            Main.Instance.StartCoroutine(CreateConsumeItems());
        }
    }

    private void ClearConsumeItems()
    {
        ClearLeftGroup();
        for (int i = 0; i < consumeItem_dic.Count; i++)
        {
            consumeItem_dic[i].Clear();
            consumeItem_dic[i].mRootObject.SetActive(false);
        }
    }

    private IEnumerator CreateConsumeItems()
    {
        isOpening = true;
        view.ScrView_ConsumeScroll.ResetPosition();
        yield return null;
        int MAXCOUNT = 25;
        int count = consumeItemList.Count;
        int itemCount = consumeItem_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_ConsumeGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_ConsumeGrid.minIndex = -index;
        view.UIWrapContent_ConsumeGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_ConsumeGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_ConsumeGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                if (!isOpening) yield break;
                consumeItem_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isOpening) yield break;
            BPNormalItemComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_ConsumeComp, view.Grd_ConsumeGrid.transform);
                comp = new BPNormalItemComponent(vGo);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                comp.AddEventListener(ButtonEvent_ConsumeItem);
                consumeItem_dic.Add(comp);
            }
            else
            {
                consumeItem_dic[i].mRootObject.SetActive(true);
            }
            consumeItem_dic[i].UpdateInfo(consumeItemList[i]);
            consumeItem_dic[i].IsSelect = false;
        }
        isOpening = false;
        yield return null;
        view.UIWrapContent_ConsumeGrid.ReGetChild();
        view.Grd_ConsumeGrid.Reposition();
        yield return null;
        view.ScrView_ConsumeScroll.ResetPosition();
        yield return null;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }

    private void UpdateMaterialItems()
    {
        if (materialItemList.Count == 0)
        {
            ClearMaterialItems();
            view.Lbl_NoItemTip.enabled = true;
            view.Lbl_NoItemTip.text = ConstString.BACKPACK_NOITEMSTIP;
        }
        else
        {
            view.Lbl_NoItemTip.enabled = false;
            Main.Instance.StartCoroutine(CreateMatrialItems());
        }
    }

    private void ClearMaterialItems()
    {
        ClearLeftGroup();
        for (int i = 0; i < materialItem_dic.Count; i++)
        {
            materialItem_dic[i].Clear();
            materialItem_dic[i].mRootObject.SetActive(false);
        }
    }

    private IEnumerator CreateMatrialItems()
    {
        isMatrialOpening = true;
        view.ScrView_MaterialScroll.ResetPosition();
        yield return null;
        int MAXCOUNT = 25;
        int count = materialItemList.Count;
        int itemCount = materialItem_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_MaterialGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_MaterialGrid.minIndex = -index;
        view.UIWrapContent_MaterialGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_MaterialGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_MaterialGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                if (!isMatrialOpening) yield break;
                materialItem_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isMatrialOpening) yield break;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_MaterialComp, view.Grd_MaterialGrid.transform);
                BPNormalItemComponent comp = new BPNormalItemComponent(vGo);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                comp.AddEventListener(ButtonEvent_MaterialItem);
                materialItem_dic.Add(comp);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isMatrialOpening) yield break;
            if (materialItem_dic[i] == null) continue;
            materialItem_dic[i].UpdateInfo(materialItemList[i]);
            materialItem_dic[i].IsSelect = false;
            materialItem_dic[i].mRootObject.SetActive(true);
        }
        isMatrialOpening = false;
        yield return null;
        view.UIWrapContent_MaterialGrid.ReGetChild();
        view.Grd_MaterialGrid.Reposition();
        yield return null;
        view.ScrView_MaterialScroll.ResetPosition();
        yield return null;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }

    private void UpdateArtifactItems()
    {
        if (artifactList.Count == 0)
        {
            ClearArtifactItems();
            view.Lbl_NoItemTip.enabled = true;
            view.Lbl_NoItemTip.text = ConstString.BACKPACK_NOITEMSTIP;
        }
        else
        {
            view.Lbl_NoItemTip.enabled = false;
            Main.Instance.StartCoroutine(CreateArtifactItems());
        }
    }
    private void ClearArtifactItems()
    {
        ClearLeftGroup();
        for (int i = 0; i < artifactItem_dic.Count; i++)
        {
            artifactItem_dic[i].Clear();
            artifactItem_dic[i].mRootObject.SetActive(false);
        }
    }
    private IEnumerator CreateArtifactItems()
    {
        isOpening = true;
        view.ScrView_ArtifactScroll.ResetPosition();
        yield return null;
        int MAXCOUNT = 25;
        int count = artifactList.Count;
        int itemCount = artifactItem_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_ArtifactGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_ArtifactGrid.minIndex = -index;
        view.UIWrapContent_ArtifactGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_ArtifactGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_ArtifactGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = itemCount - count; i < itemCount; i++)
            {
                artifactItem_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isOpening) yield break;
            BPArtifactComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_ArtifactComp, view.Grd_ArtifactGrid.transform);
                comp = new BPArtifactComponent(vGo);
                vGo.name = i.ToString();
                comp.AddEventListener(ButtonEvent_ArtifactItem);
                artifactItem_dic.Add(comp);
            }
            else
            {
                comp = artifactItem_dic[i];

            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateInfo(artifactList[i]);
            comp.IsSelect = false;
        }
        isOpening = false;
        yield return null;
        view.UIWrapContent_ArtifactGrid.ReGetChild();
        yield return null;
        view.Grd_ArtifactGrid.Reposition();
        yield return null;
        view.ScrView_ArtifactScroll.ResetPosition();
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }

    private void UpdateSoldierEquips()
    {
        if (soldierEquipList.Count == 0)
        {
            ClearSoldierEquipItems();
            view.Lbl_NoItemTip.enabled = true;
            view.Lbl_NoItemTip.text = ConstString.BACKPACK_NOITEMSTIP;
        }
        else
        {
            view.Lbl_NoItemTip.enabled = false;
            Main.Instance.StartCoroutine(CreateSoldierEquips());
        }
    }
    private void ClearSoldierEquipItems()
    {
        ClearLeftGroup();
        for (int i = 0; i < soldierEquipItem_dic.Count; i++)
        {
            soldierEquipItem_dic[i].Clear();
            soldierEquipItem_dic[i].mRootObject.SetActive(false);
        }
    }
    private IEnumerator CreateSoldierEquips()
    {
        isOpening = true;
        view.ScrView_SoldierEquipScroll.ResetPosition();
        int MAXCOUNT = 25;
        int count = soldierEquipList.Count;
        int itemCount = soldierEquipItem_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_SoldierEquipGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_SoldierEquipGrid.minIndex = -index;
        view.UIWrapContent_SoldierEquipGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_SoldierEquipGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_SoldierEquipGrid.enabled = false;
        }
        yield return null;   //延迟一帧执行 保证UIWrapContent初始化完成
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                if (!isOpening) yield break;
                soldierEquipItem_dic[i].Clear();
                soldierEquipItem_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (!isOpening) yield break;
            BPSoldierEquipComponent comp = null;
            if (i < itemCount)
            {
                comp = soldierEquipItem_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_SoldierEquipComp, view.Grd_SoldierEquipGrid.transform);
                comp = new BPSoldierEquipComponent(vGo);
                comp.AddEventListener(ButtonEvent_SoldierEquipItem);
                vGo.name = i.ToString();
                soldierEquipItem_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateInfo(soldierEquipList[i]);
            comp.IsSelect = false;
        }
        yield return null;
        view.UIWrapContent_SoldierEquipGrid.ReGetChild();
        yield return null;
        view.Grd_SoldierEquipGrid.repositionNow = true;
        yield return null;
        view.ScrView_SoldierEquipScroll.ResetPosition();
        isOpening = false;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }

    #endregion

    #region 出售
    private void OpenSellPanel()
    {
        switch (currentFuncType)
        {
            case BackPackFuncType.Consume:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
                    UISystem.Instance.ItemSellView.UpdateViewInfo(currentConsumeData.itemPOD, OtherViewCallBack);
                } break;
            case BackPackFuncType.Material:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
                    UISystem.Instance.ItemSellView.UpdateViewInfo(currentMaterialData.itemPOD, OtherViewCallBack);
                } break;
            case BackPackFuncType.SoldierEquip:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
                    UISystem.Instance.ItemSellView.UpdateViewInfo(currentSoldierEquipData, OtherViewCallBack);
                } break;
        }
    }

    private void OtherViewCallBack()
    {
        UpdateViewInfo();
    }

    //private void DetailViewCallBack()
    //{
    //    UpdateViewInfo();
    //    UpdateSoldierEquipDetailInfo();
    //}


    #endregion

    #region 士兵装备详细属性

    #endregion
    #endregion

    private void ClearDefaultData()
    {
        currentConsumeData = null;
        currentArtifactData = null;
        currentMaterialData = null;
        currentSoldierEquipData = null;
    }

    public void UpdateButtonStatus()
    {
        if (currentMaterialData == null) return;
        if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.Material)
        {
            view.Btn_GetMaterial.gameObject.SetActive(true);
            view.Btn_ChipCompose.gameObject.SetActive(false);
        }
        else if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.EquipChip)
        {
            view.Btn_GetMaterial.gameObject.SetActive(false);
            view.Btn_ChipCompose.gameObject.SetActive(true);
        }
    }

    public void UpdateConsumeButtonStatus()
    {
        if (currentConsumeData == null) return;
        ItemInfo _itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(currentConsumeData.itemPOD.id);

        if (_itemInfo.use_type != 0)
        {
            view.Btn_ConsumeUse.gameObject.SetActive(true);
            view.Btn_ConsumeGet.gameObject.SetActive(false);
        }
        else if (_itemInfo.use_type == 0)
        {
            view.Btn_ConsumeUse.gameObject.SetActive(false);
            view.Btn_ConsumeGet.gameObject.SetActive(true);
        }
    }

    /// <summary>
    /// 更新背包格子数
    /// </summary>
    public void UpdateEquipGrid()
    {
        view.Gobj_SoldierEquipGrid.SetActive(true);
        view.Lbl_SoldierEquipGridCount.text = string.Format("{0}/{1}", soldierEquipList.Count, PlayerData.Instance._SoldierEquipGrid);
    }


    public void ReceiveEquipGrid()
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BUY_SUCCESS);
        UpdateEquipGrid();
    }

    #region Button Event

    public void ButtonEvent_ConsumeSell(GameObject btn)
    {
        if (currentConsumeData == null) return;
        ItemInfo _itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(currentConsumeData.itemPOD.id);
        if (_itemInfo.sell_type == 0)
        {
            return;
        }
        OpenSellPanel();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_ConsumeUse(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (currentConsumeData != null)
        {
            if (currentConsumeData.itemInfo.use_type == 3)
            {
                List<CommonItemData> list = CommonFunction.GetCommonItemDataList(currentConsumeData.itemInfo.proplist);
                if (!CommonFunction.GetItemOverflowTip(list))
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERPROPSPACKAGEVIEW);
                    UISystem.Instance.SoldierPropsPackageView.UpdateViewInfo(currentConsumeData.itemInfo);
                }
            }
            else
            {
                BackPackModule.Instance.SendUseItem(currentConsumeData.itemPOD.id, 1, null);
            }
        }
    }
    public void ButtonEvent_ConsumeGet(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (currentConsumeData != null)
        {
            if (CommonFunction.CheckIsOpen(OpenFunctionType.GetPath, true))
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                UISystem.Instance.GetPathView.UpdateViewInfo(currentConsumeData.itemPOD.id, 1);
            }
        }
    }

    public void ButtonEvent_MaterialSell(GameObject btn)
    {
        if (currentMaterialData == null) return;
        ItemInfo _itemInfo = ConfigManager.Instance.mItemData.GetItemInfoByID(currentMaterialData.itemPOD.id);
        if (_itemInfo.sell_type == 0)
        {
            return;
        }
        OpenSellPanel();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_ChipCompose(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (currentMaterialData == null) return;
        if (CommonFunction.CheckMoneyEnough((ECurrencyType)currentMaterialData.itemInfo.costType, currentMaterialData.itemInfo.cost, true))
        {
            if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.EquipChip)
            {
                BackPackModule.Instance.SendChipCompositeReq(currentMaterialData.itemPOD.id);
            }
        }
        else
        {
            UISystem.Instance.ShowGameUI(BuyCoinView.UIName);
        }

    }

    public void ButtonEvent_GetMaterial(GameObject btn)
    {
        if (currentMaterialData == null) return;
        if (CommonFunction.CheckIsOpen(OpenFunctionType.GetPath, true))
        {
            if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.Material)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                UISystem.Instance.GetPathView.UpdateViewInfo(currentMaterialData.itemPOD.id, 1);
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

            }
            else if (currentMaterialData.itemInfo.type == (int)ItemTypeEnum.EquipChip)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
                UISystem.Instance.GetPathView.UpdateViewInfo(currentMaterialData.itemPOD.id, 2);
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

            }
        }
    }
    public void ButtonEvent_ArtifactStrengthen(GameObject btn)//强化按钮
    {
        if (currentArtifactData == null) return;
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ARTIFACTINTENSIFY);
        UISystem.Instance.ArtifactIntensifyView.UpdateViewInfo(currentArtifactData, ArtifactStrengthenCallBack);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }
    private void ArtifactStrengthenCallBack()
    {
        UpdateViewInfo();
    }
    public void ButtonEvent_SoldierEquipSell(GameObject btn)
    {
        if (currentSoldierEquipData == null) return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        OpenSellPanel();
    }
    public void ButtonEvent_SoldierEquipDetail(GameObject btn)
    {
        if (currentSoldierEquipData == null) return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        //view.Gobj_SoldierEquipDetailInfo.SetActive(true);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
        UISystem.Instance.EquipDetailInfoView.UpdateViewInfo(currentSoldierEquipData, 3, UpdateViewInfo);
        //UpdateSoldierEquipDetailInfo();
        //  detailView.UpdateViewInfo(currentSoldierEquipData, OtherViewCallBack);
        //UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIEREQUIPDETAILINFOVIEW);
        //UISystem.Instance.SoldierEquipDetailInfoView.UpdateViewInfo(currentSoldierEquipData, OtherViewCallBack);
    }

    public void ButtonEvent_AddSoldierEquipGrid(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)PlayerData.Instance._BuySoldierEquipGridNum + 1);
        if (data == null || data.EquipBagLimit == null) // data为NULL 则可默认为已经达到购买上限
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPGIRDLIMIT);
        }
        else if (data.EquipBagLimit.Type == ECurrencyType.None && data.EquipBagLimit.Number == 0)  // 消耗金币类型为0则说明无购买次数
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_EQUIPGIRDLIMIT);
        }
        else
        {
            if (CommonFunction.CheckMoneyEnough(data.EquipBagLimit.Type, data.EquipBagLimit.Number, true))
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, string.Format(ConstString.BACKPACK_PURCHASEEQUIPGRID, data.EquipBagLimit.Number), () =>
                {
                    BackPackModule.Instance.SendBuyEquipBag();
                });
            }
        }
    }

    private void ButtonEvent_ConsumeItem(BaseComponent com)
    {
        if (currentConsumeData == null) return;
        BPNormalItemComponent comp = com as BPNormalItemComponent;
        if (comp.IsSelect) return;
        comp.IsSelect = true;
        NormalItemData data = new NormalItemData();
        data.itemInfo = comp.ItemInfo;
        data.itemPOD = comp.ItemPOD;
        currentConsumeData = data;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    private void ButtonEvent_MaterialItem(BaseComponent com)
    {
        BPNormalItemComponent comp = com as BPNormalItemComponent;
        if (comp.IsSelect) return;
        comp.IsSelect = true;
        NormalItemData data = new NormalItemData();
        data.itemInfo = comp.ItemInfo;
        data.itemPOD = comp.ItemPOD;
        currentMaterialData = data;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    private void ButtonEvent_ArtifactItem(BaseComponent com)
    {
        BPArtifactComponent comp = com as BPArtifactComponent;
        if (comp.IsSelect) return;
        comp.IsSelect = true;
        currentArtifactData = comp.ArtifactPOD;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    }

    private void ButtonEvent_SoldierEquipItem(BaseComponent com)
    {
        BPSoldierEquipComponent comp = com as BPSoldierEquipComponent;
        if (comp.IsSelect) return;
        comp.IsSelect = true;
        currentSoldierEquipData = comp.SoldierEquipPOD;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    }

    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_BACKPACK);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    }

    public void ButtonEvent_ConsumeType(GameObject btn)
    {
        if (currentFuncType == BackPackFuncType.Consume) return;
        ClearDefaultData();
        UpdateViewInfo(BackPackFuncType.Consume);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }
    public void ButtonEvent_ArtifactType(GameObject btn)
    {
        if (currentFuncType == BackPackFuncType.Artifact) return;
        ClearDefaultData();
        UpdateViewInfo(BackPackFuncType.Artifact);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    }

    public void ButtonEvent_SoldierEquipType(GameObject btn)
    {
        if (currentFuncType == BackPackFuncType.SoldierEquip) return;
        ClearDefaultData();
        UpdateViewInfo(BackPackFuncType.SoldierEquip);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    }

    public void ButtonEvent_MaterialType(GameObject btn)
    {
        if (currentFuncType == BackPackFuncType.Material) return;
        ClearDefaultData();
        UpdateViewInfo(BackPackFuncType.Material);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
    }

    //private void ButtonEvent_CloseSoldierEquipInfo(GameObject btn)
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    //    //view.Gobj_SoldierEquipDetailInfo.SetActive(false);
    //    //currentSoldierEquipData = PlayerData.Instance._SoldierEquip.FindByUid(currentSoldierEquipData.uId);
    //    //UpdateSoldierEquipDetailInfo();
    //}

    //private void ButtonEvent_SoldierEquipObtain(GameObject btn)
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
    //    UISystem.Instance.GetPathView.UpdateViewInfo(currentSoldierEquipData.Att.id, 2);
    //}

    //private void ButtonEvent_SoldierEquipIntensify(GameObject btn)
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPINTENSIFY);
    //    UISystem.Instance.SoldierEquipIntensifyView.UpdateViewInfo(currentSoldierEquipData, DetailViewCallBack);
    //}

    //private void ButtonEvent_SoldierEquipUpgrade(GameObject btn)
    //{
    //    CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
    //    if (ConfigManager.Instance.mEquipData.FindById(currentSoldierEquipData.Att.evolveId) == null)
    //    {
    //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.HAD_MAX_STAR);
    //        return;
    //    }
    //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPADVANCED);
    //    UISystem.Instance.SoldierEquipAdvancedView.UpdateViewInfo(currentSoldierEquipData, DetailViewCallBack);
    //    //if (currentSoldierEquipData.isMaxLevel())
    //    //{

    //    //}
    //    //else
    //    //{
    //    //    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.EQUIP_NOTENGHT_LEVEL);
    //    //}
    //}

    #endregion
    public void UseItemSuccess()
    {
        UpdateViewInfo();
    }

    public void ReciveChipCompose(int result)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_COMPOSECHIPSUCCESS);
        UpdateViewInfo();
    }

    public override void Uninitialize()
    {
        base.Uninitialize();
        isMatrialOpening = false;
        isOpening = false;
        PlayerData.Instance.UpdatePlayerItemsEvent -= OnUpdatePlayerItemsEvent;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_ConsumeType.gameObject).onClick = ButtonEvent_ConsumeType;
        UIEventListener.Get(view.Btn_MaterialType.gameObject).onClick = ButtonEvent_MaterialType;
        UIEventListener.Get(view.Btn_ArtifactType.gameObject).onClick = ButtonEvent_ArtifactType;
        UIEventListener.Get(view.Btn_SoldierEquipType.gameObject).onClick = ButtonEvent_SoldierEquipType;
        UIEventListener.Get(view.Btn_CloseView.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(view.Btn_ConsumeSell.gameObject).onClick = ButtonEvent_ConsumeSell;
        UIEventListener.Get(view.Btn_ConsumeUse.gameObject).onClick = ButtonEvent_ConsumeUse;
        UIEventListener.Get(view.Btn_ConsumeGet.gameObject).onClick = ButtonEvent_ConsumeGet;
        UIEventListener.Get(view.Btn_MaterialSell.gameObject).onClick = ButtonEvent_MaterialSell;
        UIEventListener.Get(view.Btn_GetMaterial.gameObject).onClick = ButtonEvent_GetMaterial;
        UIEventListener.Get(view.Btn_ChipCompose.gameObject).onClick = ButtonEvent_ChipCompose;
        UIEventListener.Get(view.Btn_ArtifactStrengthen.gameObject).onClick = ButtonEvent_ArtifactStrengthen;
        UIEventListener.Get(view.Btn_SoldierEquipSell.gameObject).onClick = ButtonEvent_SoldierEquipSell;
        UIEventListener.Get(view.Btn_SoldierEquipDetail.gameObject).onClick = ButtonEvent_SoldierEquipDetail;
        UIEventListener.Get(view.Btn_AddSoldierEquip.gameObject).onClick = ButtonEvent_AddSoldierEquipGrid;
        //UIEventListener.Get(view.Btn_CloseSoldierEquipInfo.gameObject).onClick = ButtonEvent_CloseSoldierEquipInfo;
        //UIEventListener.Get(view.Btn_SoldierEquipObtain.gameObject).onClick = ButtonEvent_SoldierEquipObtain;
        //UIEventListener.Get(view.Btn_SoldierEquipIntensify.gameObject).onClick = ButtonEvent_SoldierEquipIntensify;
        //UIEventListener.Get(view.Btn_SoldierEquipUpgrade.gameObject).onClick = ButtonEvent_SoldierEquipUpgrade;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        consumeTypeComp = null;
        materialTypeComp = null;
        artifactTypeComp = null;
        soldierEquipTypeComp = null;
        soldierEquipList = null;
        consumeItem_dic.Clear();
        materialItem_dic.Clear();
        artifactItem_dic.Clear();
        soldierEquipItem_dic.Clear();
        consumeItemList.Clear();
        materialItemList.Clear();
        artifactList.Clear();
        //soldierEquipStarList.Clear();
        artifactAttList.Clear();
        //soldierEquipAttributeList.Clear();
    }

    //界面动画
    //public void PlayOpenBackPackAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();

    //}
}

