using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class LifeSpiritPackViewController : UIBase
{
    public LifeSpiritPackView view;

    private List<LifeSpiritPackComponent> lifeSpirit_dic;
    private List<LifeSpiritAttComponent> lifeSpiritAtt_dic;
    private List<LifeSoulData> packSoulList;

    private LifeSoulData currentSpiritData;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new LifeSpiritPackView();
            view.Initialize();
            BtnEventBinding();
        }
        Init();
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent += LifeSoulChange;
    }

    private void LifeSoulChange()
    {
        UpdateViewInfo();
    }

    private void Init()
    {
        currentSpiritData = null;
        if (lifeSpirit_dic == null)
            lifeSpirit_dic = new List<LifeSpiritPackComponent>();
        if (lifeSpiritAtt_dic == null)
            lifeSpiritAtt_dic = new List<LifeSpiritAttComponent>();
        view.gobj_LifeSpiritAttComp.SetActive(false);
        view.UIWrapContent_LifeSpiritGrid.onInitializeItem = UpdateWrapLifeSoul;
        UpdateViewInfo();
    }

    #region Update Event

    private void UpdateWrapLifeSoul(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_LifeSpiritGrid.enabled) return;
        if (currentSpiritData == null || packSoulList == null) return;
        if (realIndex >= packSoulList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        LifeSpiritPackComponent comp = lifeSpirit_dic[wrapIndex];
        if (comp == null) return;
        LifeSoulData soulData = packSoulList[realIndex];
        if (soulData == null)
            return;
        comp.UpdateCompInfo(soulData);
        comp.IsSelect = comp.LifeSoulData.SoulPOD.uid.Equals(currentSpiritData.SoulPOD.uid);
    }

    public void UpdateViewInfo()
    {
        packSoulList = PlayerData.Instance._LifeSoulDepot.GetPackLifeSoul();
        currentSpiritData = null;
        if (packSoulList == null || packSoulList.Count <= 0)
        {
            ClearLeftGroup();
            view.Lbl_NoItemTip.enabled = true;
            view.ScrView_LifeSpiritScroll.gameObject.SetActive(false);
        }
        else
        {
            packSoulList.Sort((left, right) =>
            {
                if (left == null || right == null)
                    return 0;
                if (left.SoulPOD.level != right.SoulPOD.level)
                {
                    if (left.SoulPOD.level < right.SoulPOD.level)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                if (left.SoulInfo.quality != right.SoulInfo.quality)
                {
                    if (left.SoulInfo.quality < right.SoulInfo.quality)
                    {
                        return 1;
                    }
                    else
                    {
                        return -1;
                    }
                }
                if (left.SoulInfo.type != right.SoulInfo.type)
                {
                    if (left.SoulInfo.type < right.SoulInfo.type)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                if (left.SoulInfo.id != right.SoulInfo.id)
                {
                    if (left.SoulInfo.id < right.SoulInfo.id)
                    {
                        return -1;
                    }
                    else
                    {
                        return 1;
                    }
                }
                return 0;
            });
            view.Lbl_NoItemTip.enabled = false;
            view.ScrView_LifeSpiritScroll.gameObject.SetActive(true);
            Main.Instance.StartCoroutine(UpdatePackLifeSoul());
        }
        UpdatePackGrid();
    }

    private void UpdatePackGrid()
    {
        view.Lbl_LifeSpiritCount.text = string.Format(ConstString.LIFESPIRITPACK_LIFESOUL_COUNT, packSoulList.Count, PlayerData.Instance._PreyLifeSoulInfo.grid_num);
    }

    private IEnumerator UpdatePackLifeSoul()
    {
        int MAXCOUNT = 25;
        int count = packSoulList.Count;
        int itemCount = lifeSpirit_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_LifeSpiritGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_LifeSpiritGrid.minIndex = -index;
        view.UIWrapContent_LifeSpiritGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_LifeSpiritGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_LifeSpiritGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                lifeSpirit_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            LifeSoulData data = packSoulList[i];
            if (data == null)
                continue;
            LifeSpiritPackComponent comp = null;
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_LifeSpiritComp, view.Grd_LifeSpiritGrid.transform);
                comp = new LifeSpiritPackComponent();
                vGo.name = i.ToString();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_LifeSpiritItem);
                lifeSpirit_dic.Add(comp);
            }
            else
            {
                comp = lifeSpirit_dic[i];

            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.UpdateCompInfo(data);
            comp.IsSelect = false;
        }
        yield return null;
        view.UIWrapContent_LifeSpiritGrid.ReGetChild();
        yield return null;
        view.Grd_LifeSpiritGrid.Reposition();
        yield return null;
        view.ScrView_LifeSpiritScroll.ResetPosition();
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }


    private void UpdateSelectItem()
    {
        if (currentSpiritData == null && lifeSpirit_dic.Count > 0)
        {
            LifeSpiritPackComponent comp = lifeSpirit_dic[0];
            currentSpiritData = comp.LifeSoulData;
            comp.IsSelect = true;
        }
        else
        {
            LifeSpiritPackComponent comp = null;
            for (int i = 0; i < lifeSpirit_dic.Count; i++)
            {
                LifeSpiritPackComponent tmpComp = lifeSpirit_dic[i];
                tmpComp.IsSelect = false;
                if (tmpComp.LifeSoulData.SoulPOD.uid == currentSpiritData.SoulPOD.uid)
                {
                    comp = tmpComp;
                    comp.IsSelect = true;
                }
            }
        }
    }

    private void UpdateLeftInfoGroup()
    {
        if (currentSpiritData == null)
        {
            ClearLeftGroup();
            return;
        }
        view.Lbl_SelectItemTip.enabled = false;
        view.Btn_ArtifactSell.collider.enabled = true;
        CommonFunction.UpdateWidgetGray(view.Btn_LifeSpiritSellBg, false);
        CommonFunction.SetUILabelColor(view.Lbl_LifeSpiritSell, true);
        view.Btn_ArtifactStrengthen.collider.enabled = true;
        CommonFunction.UpdateWidgetGray(view.Spt_LifeSpiritStrengthenBg, false);
        CommonFunction.SetUILabelColor(view.Lbl_LifeSpiritStrengthen, true);

        CommonFunction.SetQualitySprite(view.Spt_LifeSpiritInfoFrame, currentSpiritData.SoulInfo.quality, view.Spt_LifeSpiritShading);
        CommonFunction.SetSpriteName(view.Spt_LifeSpiritIcon, currentSpiritData.SoulInfo.icon);
        CommonFunction.SetLifeSpiritTypeMark(view.Spt_LifeSpiritTypeMark, currentSpiritData.SoulInfo.godEquip);
        view.Lbl_ArtifactItemLevel.text = string.Format(ConstString.BACKPACK_SOLDIEREQUIPLEVEL, currentSpiritData.SoulPOD.level.ToString());
        view.Lbl_ArtifactItemName.text = currentSpiritData.SoulInfo.name;
        if (currentSpiritData.SoulInfo.skillID == 0)
        {
            view.Lbl_SkillDesc.enabled = false;
            UpdateArtifactAtt();
        }
        else
        {
            view.Grd_LifeSpiritAttGroup.gameObject.SetActive(false);
            view.Lbl_SkillDesc.enabled = true;

            view.Lbl_SkillDesc.text = currentSpiritData.Skill.GetDescript(currentSpiritData.Skill.Level);
            view.UITable_ArtifactAtt.repositionNow = true;
        }

        view.Lbl_ArtifactDesc.text = CommonFunction.ReplaceEscapeChar(currentSpiritData.SoulInfo.desc);
        view.Gobj_SellGroup.SetActive(true);
        CommonFunction.SetMoneyIcon(view.Spt_SellIcon, currentSpiritData.SoulInfo.sellPrice.Type);
        view.Lbl_SellPrice.text = currentSpiritData.SoulInfo.sellPrice.Number.ToString();
    }

    private void UpdateArtifactAtt()
    {
        if (currentSpiritData == null)
        {
            ClearLeftGroup();
            return;
        }
        view.Grd_LifeSpiritAttGroup.gameObject.SetActive(true);
        UpdateArtifactAttValue();
        view.UITable_ArtifactAtt.repositionNow = true;
    }

    private void UpdateArtifactAttValue()
    {
        ShowInfoWeapon data = new ShowInfoWeapon();
        data.HP = currentSpiritData.SoulInfo.hp_initial + currentSpiritData.SoulInfo.hp_up * (currentSpiritData.SoulPOD.level - 1);
        data.Attack = currentSpiritData.SoulInfo.attack_initial + currentSpiritData.SoulInfo.attack_up * (currentSpiritData.SoulPOD.level - 1);
        data.Accuracy = currentSpiritData.SoulInfo.accrate_initial + currentSpiritData.SoulInfo.accrate_up * (currentSpiritData.SoulPOD.level - 1);
        data.Dodge = currentSpiritData.SoulInfo.ddgrate_initial + currentSpiritData.SoulInfo.ddgrate_up * (currentSpiritData.SoulPOD.level - 1);
        data.Crit = currentSpiritData.SoulInfo.crt_initial + currentSpiritData.SoulInfo.crt_up * (currentSpiritData.SoulPOD.level - 1);
        data.Tenacity = currentSpiritData.SoulInfo.tenacity_initial + currentSpiritData.SoulInfo.tenacity_up * (currentSpiritData.SoulPOD.level - 1);
        data.HP = Mathf.CeilToInt(data.HP / 10000);
        data.Attack = Mathf.CeilToInt(data.Attack / 10000);
        data.Accuracy = Mathf.CeilToInt(data.Accuracy / 10000);
        data.Dodge = Mathf.CeilToInt(data.Dodge / 10000);
        data.Crit = Mathf.CeilToInt(data.Crit / 10000);
        data.Tenacity = Mathf.CeilToInt(data.Tenacity / 10000);
        List<KeyValuePair<string, string>> attributeList = CommonFunction.GetWeaponAttributeDescNoWord(data);
        int count = lifeSpiritAtt_dic.Count;
        if (attributeList.Count < count)
        {
            for (int i = attributeList.Count; i < count; i++)
            {
                LifeSpiritAttComponent comp = lifeSpiritAtt_dic[i];
                if (comp != null)
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        for (int i = 0; i < attributeList.Count; i++)
        {
            KeyValuePair<string, string> tmpInfo = attributeList[i];
            LifeSpiritAttComponent comp = null;
            if (i < count)
            {
                comp = lifeSpiritAtt_dic[i];
            }
            else
            {
                GameObject go = CommonFunction.InstantiateObject(view.gobj_LifeSpiritAttComp, view.Grd_LifeSpiritAttGroup.transform);
                comp = new LifeSpiritAttComponent();
                comp.MyStart(go);
                lifeSpiritAtt_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_LifeSpiritAttGroup.Reposition();
    }

    private void ClearLeftGroup()
    {
        view.Spt_LifeSpiritTypeMark.enabled = false;
        view.Lbl_SelectItemTip.enabled = true;
        view.Btn_ArtifactSell.collider.enabled = false;
        view.Btn_ArtifactStrengthen.collider.enabled = false;
        view.Spt_LifeSpiritIcon.spriteName = string.Empty;
        view.Spt_LifeSpiritInfoFrame.spriteName = string.Empty;
        view.Spt_LifeSpiritShading.spriteName = string.Empty;
        view.Lbl_ArtifactItemLevel.text = string.Empty;
        view.Lbl_ArtifactDesc.text = string.Empty;
        view.Lbl_ArtifactItemName.text = string.Empty;
        view.Gobj_SellGroup.SetActive(false);
        view.Lbl_SkillDesc.enabled = false;
        view.Grd_LifeSpiritAttGroup.gameObject.SetActive(false);
        CommonFunction.UpdateWidgetGray(view.Btn_LifeSpiritSellBg, true);
        CommonFunction.SetUILabelColor(view.Lbl_LifeSpiritStrengthen, false);
        CommonFunction.UpdateWidgetGray(view.Spt_LifeSpiritStrengthenBg, true);
        CommonFunction.SetUILabelColor(view.Lbl_LifeSpiritSell, false);
    }


    #endregion

    private void LifeSoulSellCallBack()
    {
        //currentSpiritComp = null;
        // UpdateViewInfo();
    }

    public void OnBuyGirdSuccess()
    {
        currentSpiritData = null;
        UpdatePackGrid();
    }

    #region Button Event

    private void ButtonEvent_LifeSpiritItem(BaseComponent baseComp)
    {
        LifeSpiritPackComponent comp = baseComp as LifeSpiritPackComponent;
        if (comp == null)
            return;
        if (comp.IsSelect) return;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        currentSpiritData = comp.LifeSoulData;
        UpdateSelectItem();
        UpdateLeftInfoGroup();
    }

    public void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITPACKVIEW);
    }

    public void ButtonEvent_LifeSpiritSell(GameObject btn)
    {
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ITEMSELL);
        UISystem.Instance.ItemSellView.UpdateViewInfo(currentSpiritData, LifeSoulSellCallBack);
    }

    public void ButtonEvent_LifeSpiritStrengthen(GameObject btn)
    {
        if (currentSpiritData == null)
            return;
        if (currentSpiritData.IsAbleToUpgrade())
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITINTENSIFY);
            UISystem.Instance.LifeSpiritIntensifyView.UpdateViewInfo(currentSpiritData);
        }
    }

    public void ButtonEvent_AddSoldierGrid(GameObject btn)
    {
        TimesExpendData data = ConfigManager.Instance.mTimesExpendConfig.GetTimesExpendData((uint)(PlayerData.Instance._PreyLifeSoulInfo.buy_times + 1));
        if (data == null || data.LifeSoulPackConsume == null) // data为NULL 则可默认为已经达到购买上限
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_ADDPACKGRID_LIMIT);
        }
        else if (data.LifeSoulPackConsume.Type == ECurrencyType.None && data.LifeSoulPackConsume.Number == 0)  // 消耗金币类型为0则说明无购买次数
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITPACK_ADDPACKGRID_LIMIT);
        }
        else
        {
            string consumedesc = data.LifeSoulPackConsume.Number.ToString() + CommonFunction.GetConsumeTypeDesc(data.LifeSoulPackConsume.Type);
            int count = 5;
            if (!int.TryParse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.LIFESOUL_BUYGRID_COUNT), out count))
            {
                count = 5;
            }
            string content = string.Format(ConstString.LIFESPIRITPACK_ADDPACKGRID, consumedesc, count);
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo, content, () =>
            {
                if (CommonFunction.CheckMoneyEnough(data.LifeSoulPackConsume.Type, data.LifeSoulPackConsume.Number, true))
                {
                    LifeSpiritModule.Instance.SendBuySoulGrid();
                }
            }, () => { }, ConstString.MESSAGEBOXBTN_YES, ConstString.MESSAGEBOXBTN_NO);
        }
    }
    #endregion

    public override void Uninitialize()
    {
        currentSpiritData = null;
        PlayerData.Instance._LifeSoulDepot.LifeSoulChangeEvent -= LifeSoulChange;
        Main.Instance.StopCoroutine(UpdatePackLifeSoul());
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (lifeSpirit_dic != null)
            lifeSpirit_dic.Clear();
        if (lifeSpiritAtt_dic != null)
            lifeSpiritAtt_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_CloseView.gameObject).onClick = ButtonEvent_CloseView;
        UIEventListener.Get(view.Btn_ArtifactSell.gameObject).onClick = ButtonEvent_LifeSpiritSell;
        UIEventListener.Get(view.Btn_ArtifactStrengthen.gameObject).onClick = ButtonEvent_LifeSpiritStrengthen;
        UIEventListener.Get(view.Btn_AddLifeSpiritGrid.gameObject).onClick = ButtonEvent_AddSoldierGrid;
    }


}
