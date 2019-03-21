using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class PetSystemViewController : UIBase
{

    public PetSystemView view;

    private List<PetAttComponent> topPetAtt_dic;
    private List<PetAttComponent> centerPetAtt_dic;
    private List<PetInfoComponent> pet_dic;

    private PetMaterialComponent upgradeMatComp;
    private PetMaterialComponent skillMatComp;
    private PetSkillComponent petSkillComp;

    private PetInfoComponent currentPetComp;
    private uint currentPetID;


    private GameObject PetObj;
    private TdSpine.MainSpine petMainSpine;
    private string petAnimation = "";
    private GameObject Go_SkillIntensifyEffect;
    private GameObject Go_PetEquipEffect;

    private bool isSortPet = false;
    private List<string> cacheNameList;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new PetSystemView();
            view.Initialize();
            BtnEventBinding();
        }
        if (cacheNameList == null)
            cacheNameList = new List<string>();
        cacheNameList.Clear();
        if (pet_dic == null)
            pet_dic = new List<PetInfoComponent>();
        if (centerPetAtt_dic == null)
            centerPetAtt_dic = new List<PetAttComponent>();
        if (topPetAtt_dic == null)
            topPetAtt_dic = new List<PetAttComponent>();
        if (centerPetAtt_dic.Count == 0)
        {
            PetAttComponent centerLeft = new PetAttComponent();
            centerLeft.MyStart(view.Gobj_LeftAttComp);
            centerPetAtt_dic.Add(centerLeft);
            PetAttComponent centerRight = new PetAttComponent();
            centerRight.MyStart(view.Gobj_RightAttComp);
            centerPetAtt_dic.Add(centerRight);
        }
        if (topPetAtt_dic.Count == 0)
        {
            PetAttComponent phy_atk = new PetAttComponent();
            phy_atk.MyStart(view.Gobj_phy_atk);
            topPetAtt_dic.Add(phy_atk);

            PetAttComponent hp_max = new PetAttComponent();
            hp_max.MyStart(view.Gobj_hp_max);
            topPetAtt_dic.Add(hp_max);

            PetAttComponent acc_rate = new PetAttComponent();
            acc_rate.MyStart(view.Gobj_acc_rate);
            topPetAtt_dic.Add(acc_rate);

            PetAttComponent ddg_rate = new PetAttComponent();
            ddg_rate.MyStart(view.Gobj_ddg_rate);
            topPetAtt_dic.Add(ddg_rate);

            PetAttComponent crt_rate = new PetAttComponent();
            crt_rate.MyStart(view.Gobj_crt_rate);
            topPetAtt_dic.Add(crt_rate);

            PetAttComponent tnc_rate = new PetAttComponent();
            tnc_rate.MyStart(view.Gobj_tnc_rate);
            topPetAtt_dic.Add(tnc_rate);
        }


        if (upgradeMatComp == null)
        {
            upgradeMatComp = new PetMaterialComponent();
            upgradeMatComp.MyStart(view.Gobj_UpgradeMaterialGroup);
        }
        if (skillMatComp == null)
        {
            skillMatComp = new PetMaterialComponent();
            skillMatComp.MyStart(view.Gobj_StrengthMaterialGroup);
        }
        if (petSkillComp == null)
        {
            petSkillComp = new PetSkillComponent();
            petSkillComp.MyStart(view.Gobj_SkillInfoComp);
        }

        view.Gobj_PetInfoComp.SetActive(false);
        view.Gobj_AttDescGroup.SetActive(false);
        view.Gobj_GetPathGroup.SetActive(true);

        view.UIWrapContent_PetManagementGrid.onInitializeItem = UpdateWrapPet;

        UpdateViewInfo();
        if (PlayerPrefsTool.HasKey(AppPrefEnum.NewPet))
        {
            PlayerPrefsTool.DeleteLocalData(AppPrefEnum.NewPet);
        }
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenPetSystemView);
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_PETSYSTEM);
    }

    #region  Update Event
    private void UpdateWrapPet(GameObject go, int wrapIndex, int realIndex)
    {
        if (!view.UIWrapContent_PetManagementGrid.enabled) return;
        List<PetData> petdatas = PlayerData.Instance._PetDepot.GetPetList();
        if (currentPetComp == null || petdatas == null) return;
        if (realIndex >= petdatas.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        PetInfoComponent comp = pet_dic[wrapIndex];
        if (comp == null) return;
        PetData petData = petdatas[realIndex];
        if (petData == null)
            return;
        comp.UpdateCompInfo(petData);
        comp.IsSelect = comp.PetData.PetInfo.id.Equals(currentPetID);
        comp.IsEquiped = comp.PetData.IsEquiped;
    }

    private void UpdateViewInfo()
    {
        currentPetID = 0;
        currentPetComp = null;
        Main.Instance.StartCoroutine(UpdatePetComponent());
        UpdatePetCount();
    }

    private void UpdateInfo()
    {
        Main.Instance.StartCoroutine(UpdatePetComponent());
    }

    private void UpdatePetCount()
    {
        view.Lbl_AllPetCount.text = PlayerData.Instance._PetDepot.GetOwnedPets().Count + "/" + PlayerData.Instance._PetDepot.GetPetList().Count;
    }

    private void UpdateSelectPetInfo()
    {
        isSortPet = false;
        SetDefaultSelectPet();
        currentPetComp.UpdatePromptStatus();
        CreatePlayerObj();
        UpdatePetsStatus();
        UpdateTopAttInfo();
        UpdateCenterInfo();
        UpdateRightInfo();
        UpdateButtonInfo();
    }

    private void SetDefaultSelectPet()
    {
        for (int i = 0; i < pet_dic.Count; i++)
        {
            PetInfoComponent comp = pet_dic[i];
            if (comp == null)
                continue;
            if (comp.PetData.PetInfo.id.Equals(currentPetID))
            {
                currentPetComp = comp;
                break;
            }
        }
        if (currentPetComp == null)
        {
            if (pet_dic != null && pet_dic.Count > 0)
            {
                currentPetComp = pet_dic[0];
            }
        }
        currentPetID = currentPetComp.PetData.PetInfo.id;
    }

    private void UpdatePetsStatus()
    {
        for (int i = 0; i < pet_dic.Count; i++)
        {
            PetInfoComponent comp = pet_dic[i];
            if (comp == null)
                continue;
            comp.IsSelect = false;
            if (comp.PetData.PetInfo.id.Equals(currentPetID))
                comp.IsSelect = true;
            comp.IsEquiped = comp.PetData.IsEquiped;
        }
    }

    private void UpdateTopAttInfo()
    {
        List<KeyValuePair<string, string>> list = ComputePetProvideAtt();
        for (int i = 0; i < topPetAtt_dic.Count; i++)
        {
            PetAttComponent comp = topPetAtt_dic[i];
            if (comp == null)
                continue;
            if (i < list.Count)
            {
                KeyValuePair<string, string> tmp = list[i];
                comp.UpdateInfo(tmp.Key, tmp.Value);
                comp.mRootObject.SetActive(true);
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
    }

    /// <summary>
    /// 更新中间面板信息
    /// </summary>
    private void UpdateCenterInfo()
    {
        view.Lbl_PetName.text = currentPetComp.PetData.PetInfo.name;
        view.Lbl_PetTalent.text = currentPetComp.PetData.Level.ToString();
        UpdateCenterMaterialInfo();
        SetPetEquipEffect(currentPetComp.IsEquiped);
    }

    private void CreatePlayerObj()
    {
        if (currentPetComp.PetData == null)
            return;
        if (this.petAnimation.Equals(currentPetComp.PetData.PetInfo.res_name) && this.PetObj != null)
        {
            PetObj.SetActive(true);
            return;
        }
        if (this.PetObj != null)
        {
            GameObject.Destroy(this.PetObj);
        }
        ResourceLoadManager.Instance.LoadCharacter(currentPetComp.PetData.PetInfo.res_name, ResourceLoadType.AssetBundle, (obj) =>
        {
            if (obj != null)
            {
                string cacheName = ResPath.ReplaceFileName(currentPetComp.PetData.PetInfo.res_name, false);
                cacheNameList.Add(cacheName);
                this.petAnimation = currentPetComp.PetData.PetInfo.res_name;
                GameObject go = CommonFunction.InstantiateObject(obj, view.Gobj_PlayerParent.transform);
                go.SetActive(true);
                this.PetObj = go;
                TdSpine.MainSpine mainSpine = go.GetComponent<TdSpine.MainSpine>();
                if (mainSpine == null)
                    mainSpine = go.AddComponent<TdSpine.MainSpine>();
                this.petMainSpine = mainSpine;
                this.petMainSpine.InitSkeletonAnimation();
                this.petMainSpine.StartEvent += mainSpine_StartEvent;
                this.petMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
                go.transform.localScale *= this.currentPetComp.PetData.PetInfo.view_scale;
                //this.mainSpine.SetColor(new Color(0,0.14f,1));
                Main.Instance.StartCoroutine(SpineSorting());
            }
        });
    }


    private IEnumerator SpineSorting()
    {
        yield return 0;
        UIPanel panel = view._uiRoot.GetComponent<UIPanel>();
        this.petMainSpine.setSortingOrder(panel.sortingOrder + 2);
        this.petMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
    }

    /// <summary>
    /// 更新右侧面板信息
    /// </summary>
    private void UpdateRightInfo()
    {
        if (currentPetComp.PetData.IsOwned)
        {
            petSkillComp.UpdateInfo(currentPetComp.PetData.Skill.Att.Icon, currentPetComp.PetData.Skill.Att.Quality, currentPetComp.PetData.Skill.Att.Name, (int)currentPetComp.PetData.Skill.Level);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnStrengthButtonBackground, false);
            CommonFunction.SetUILabelColor(view.Lbl_BtnStrengthButtonLabel, false);
            view.Lbl_PetSkillDesc.text = currentPetComp.PetData.Skill.GetDescript(currentPetComp.PetData.Skill.Level);
            MaterialsBagAttributeInfo info = currentPetComp.PetData.SkillMatData;
            if (info == null || info.MaterialList == null)
            {
                view.Spt_BtnStrengthButtonprompt.enabled = false;
                skillMatComp.mRootObject.SetActive(false);
                return;
            }
            skillMatComp.mRootObject.SetActive(true);
            KeyValuePair<uint, int> mat_value = new KeyValuePair<uint, int>();
            foreach (KeyValuePair<uint, int> tmp in info.MaterialList)
            {
                mat_value = tmp;//界面设计上仅会出现一个 add by taiwei
                break;
            }
            int ownCount = PlayerData.Instance.GetItemCountByID(mat_value.Key);
            skillMatComp.UpdateCompInfo(mat_value.Key, ownCount, mat_value.Value, info.costType, info.Cost);
            if (ownCount >= mat_value.Value)
            {
                if (currentPetComp.PetData.Skill.isMaxLevel())
                {
                    SkillAttributeInfo temp = ConfigManager.Instance.mSkillAttData.FindById(currentPetComp.PetData.Skill.Att.evolveId);
                    if (temp == null)
                    {
                        view.Spt_BtnStrengthButtonprompt.enabled = false;
                    }
                    else
                    {
                        if (CommonFunction.CheckMoneyEnough(skillMatComp.ConsumeData.Type, skillMatComp.ConsumeData.Number, false))
                        {
                            view.Spt_BtnStrengthButtonprompt.enabled = true;
                        }
                        else
                        {
                            view.Spt_BtnStrengthButtonprompt.enabled = false;
                        }
                    }
                }
                else
                {
                    if (CommonFunction.CheckMoneyEnough(skillMatComp.ConsumeData.Type, skillMatComp.ConsumeData.Number, false))
                    {
                        view.Spt_BtnStrengthButtonprompt.enabled = true;
                    }
                    else
                    {
                        view.Spt_BtnStrengthButtonprompt.enabled = false;
                    }
                }
                skillMatComp.Gobj_CompletionGroup.SetActive(false);
            }
            else
            {
                skillMatComp.Gobj_CompletionGroup.SetActive(true);
                view.Spt_BtnStrengthButtonprompt.enabled = false;
            }
        }
        else
        {
            skillMatComp.mRootObject.SetActive(false);
            petSkillComp.UpdateInfo(currentPetComp.PetData.Skill.Att.Icon, currentPetComp.PetData.Skill.Att.Quality, currentPetComp.PetData.Skill.Att.Name, 1);
            view.Lbl_PetSkillDesc.text = currentPetComp.PetData.Skill.GetDescript(1);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnStrengthButtonBackground, true);
            CommonFunction.SetUILabelColor(view.Lbl_BtnStrengthButtonLabel, true);
            view.Spt_BtnStrengthButtonprompt.enabled = false;
        }
        view.ScrollView_SkillDesc.ResetPosition();
    }

    private void UpdateCenterMaterialInfo()
    {
        if (currentPetComp.PetData == null)
            return;
        if (currentPetComp.PetData.IsOwned)
        {
            view.Gobj_AttDescGroup.SetActive(true);
            view.Gobj_GetPathGroup.SetActive(false);
            UpdatePetAddAttInfo();
            UpdateUpgradeMatInfo();
        }
        else
        {
            view.Gobj_AttDescGroup.SetActive(false);
            view.Gobj_GetPathGroup.SetActive(true);
            view.Lbl_GetPathDesc.text = currentPetComp.PetData.PetInfo.unlock_desc;
        }
    }

    private void UpdatePetAddAttInfo()
    {
        ShowInfoWeapon info = new ShowInfoWeapon();
        int level = currentPetComp.PetData.Level - 1;
        if (level <= 0)
            level = 0;
        info.HP = currentPetComp.PetData.PetInfo.hp_max + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_hp);
        info.Attack = currentPetComp.PetData.PetInfo.phy_atk + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_attack);
        info.Accuracy = currentPetComp.PetData.PetInfo.acc_rate + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_accuracy);
        info.Dodge = currentPetComp.PetData.PetInfo.ddg_rate + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_dodge);
        info.Crit = currentPetComp.PetData.PetInfo.crt_rate + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_crit);
        info.Tenacity = currentPetComp.PetData.PetInfo.tnc_rate + Mathf.CeilToInt(level * currentPetComp.PetData.PetInfo.u_tenacity);
        List<KeyValuePair<string, string>> attributeList = CommonFunction.GetWeaponAttributeDesc(info);
        for (int i = 0; i < centerPetAtt_dic.Count; i++)
        {
            PetAttComponent comp = centerPetAtt_dic[i];
            if (comp == null)
                continue;
            if (i < attributeList.Count)
            {
                KeyValuePair<string, string> tmpInfo = attributeList[i];
                comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
                comp.mRootObject.SetActive(true);
            }
            else
            {
                comp.mRootObject.SetActive(false);
            }
        }
    }

    private void UpdateUpgradeMatInfo()
    {
        CombatPetsUpdateInfo info = currentPetComp.PetData.UpgradeMatData;  //最大等级
        if (info == null)
        {
            upgradeMatComp.mRootObject.SetActive(false);
            view.Spt_BtnUpgradeButtonprompt.enabled = false;
            return;
        }
        upgradeMatComp.mRootObject.SetActive(true);

        if (info.materials == null || info.materials.Count < 1)
            return;
        PetCommonMaterilData matData = info.materials[0];//界面设计上仅会出现一个 add by taiwei
        int ownCount = PlayerData.Instance.GetItemCountByID(matData.materailID);
        upgradeMatComp.UpdateCompInfo(matData.materailID, ownCount, matData.num, info.cerrencyType, info.cerrencyNum);
        if (ownCount >= matData.num)
        {
            if (CommonFunction.CheckMoneyEnough(upgradeMatComp.ConsumeData.Type, upgradeMatComp.ConsumeData.Number, false))
            {
                if (currentPetComp.PetData.PetInfo.suffixPetid == 0)  //最大等级
                {
                    view.Spt_BtnUpgradeButtonprompt.enabled = false;
                }
                else
                {
                    CombatPetInfo PetInfo = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(currentPetComp.PetData.PetInfo.suffixPetid);  //找不到后置数据 默认为最大等级
                    if (PetInfo == null)
                    {
                        view.Spt_BtnUpgradeButtonprompt.enabled = false;
                    }
                    else
                    {
                        view.Spt_BtnUpgradeButtonprompt.enabled = true;
                    }
                }
            }
            else
            {
                view.Spt_BtnUpgradeButtonprompt.enabled = false;
            }
            upgradeMatComp.Gobj_CompletionGroup.SetActive(false);
        }
        else
        {
            view.Spt_BtnUpgradeButtonprompt.enabled = false;
            upgradeMatComp.Gobj_CompletionGroup.SetActive(true);
        }

    }

    private void UpdateButtonInfo()
    {
        if (currentPetComp.PetData.IsOwned)
        {
            if (currentPetComp.PetData.IsEquiped)
            {
                view.Lbl_BtnEquipButtonLabel.text = ConstString.PET_GETOFFPET;
            }
            else
            {
                view.Lbl_BtnEquipButtonLabel.text = ConstString.PET_EQUIPPET;
            }
            CommonFunction.UpdateWidgetGray(view.Spt_BtnEquipButtonBackground, false);
            CommonFunction.SetUILabelColor(view.Lbl_BtnEquipButtonLabel, true);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnStrengthButtonBackground, false);
            CommonFunction.SetUILabelColor(view.Lbl_BtnStrengthButtonLabel, true);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnUpgradeButtonBackground, false);
            CommonFunction.SetUILabelColor(view.Lbl_BtnUpgradeButtonLabel, true);
        }
        else
        {
            view.Lbl_BtnEquipButtonLabel.text = ConstString.PET_EQUIPPET;
            CommonFunction.UpdateWidgetGray(view.Spt_BtnEquipButtonBackground, true);
            CommonFunction.SetUILabelColor(view.Lbl_BtnEquipButtonLabel, false);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnStrengthButtonBackground, true);
            CommonFunction.SetUILabelColor(view.Lbl_BtnStrengthButtonLabel, false);
            CommonFunction.UpdateWidgetGray(view.Spt_BtnUpgradeButtonBackground, true);
            CommonFunction.SetUILabelColor(view.Lbl_BtnUpgradeButtonLabel, false);
        }
    }

    private IEnumerator UpdatePetComponent()
    {
        isSortPet = true;
        int MAXCOUNT = 8;
        List<PetData> petdata_list = PlayerData.Instance._PetDepot.GetPetList();
        if (petdata_list == null)
            petdata_list = new List<PetData>();
        int count = petdata_list.Count;
        int itemCount = pet_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_PetManagementGrid.wideCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_PetManagementGrid.minIndex = -index;
        view.UIWrapContent_PetManagementGrid.maxIndex = 0;
        if (count > MAXCOUNT)
        {
            view.UIWrapContent_PetManagementGrid.enabled = true;
            count = MAXCOUNT;
        }
        else
        {
            view.UIWrapContent_PetManagementGrid.enabled = false;
        }
        yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                pet_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            PetData petData = petdata_list[i];
            PetInfoComponent comp = null;
            if (i < itemCount)
            {
                comp = pet_dic[i];

            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_PetInfoComp, view.Grd_PetManagementGrid.transform);
                comp = new PetInfoComponent();
                vGo.name = i.ToString();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_PetManagement);
                pet_dic.Add(comp);
            }
            if (comp == null)
                continue;
            comp.UpdateCompInfo(petData);
            comp.mRootObject.SetActive(true);
        }
        yield return null;
        view.UIWrapContent_PetManagementGrid.ReGetChild();
        yield return null;
        view.Grd_PetManagementGrid.Reposition();
        yield return null;
        view.ScrView_PetScrollView.ResetPosition();
        yield return null;
        UpdateSelectPetInfo();
    }

    private List<KeyValuePair<string, string>> ComputePetProvideAtt()
    {
        ShowInfoWeapon data = new ShowInfoWeapon();
        List<PetData> list = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (list != null)
        {
            for (int i = 0; i < list.Count; i++)
            {
                PetData tmp_data = list[i];
                if (tmp_data == null)
                    continue;
                data.Attack += tmp_data.PetInfo.phy_atk + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_attack);
                data.Crit += tmp_data.PetInfo.crt_rate + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_crit);
                data.Dodge += tmp_data.PetInfo.ddg_rate + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_dodge);
                data.Accuracy += tmp_data.PetInfo.acc_rate + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_accuracy);
                data.HP += tmp_data.PetInfo.hp_max + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_hp);
                data.Tenacity += tmp_data.PetInfo.tnc_rate + Mathf.CeilToInt((tmp_data.Level - 1) * tmp_data.PetInfo.u_tenacity);
            }
        }
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();

        KeyValuePair<string, string> hp_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, data.HP.ToString());
        attribute_dic.Add(hp_pair);

        KeyValuePair<string, string> att_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, data.Attack.ToString());
        attribute_dic.Add(att_pair);

        KeyValuePair<string, string> dodge_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, data.Dodge.ToString());
        attribute_dic.Add(dodge_pair);

        KeyValuePair<string, string> accuracy_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, data.Accuracy.ToString());
        attribute_dic.Add(accuracy_pair);


        KeyValuePair<string, string> crit_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, data.Crit.ToString());
        attribute_dic.Add(crit_pair);

        KeyValuePair<string, string> tenacity_pair = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, data.Tenacity.ToString());
        attribute_dic.Add(tenacity_pair);

        return attribute_dic;

    }
    #endregion

    #region Button Event
    void mainSpine_StartEvent(string animationName)
    {
        if (animationName.Equals(GlobalConst.ANIMATION_NAME_IDLE))
        {
            this.petMainSpine.gameObject.SetActive(true);
            this.petMainSpine.StartEvent -= mainSpine_StartEvent;
        }
    }
    private void ButtonEvent_PetManagement(BaseComponent baseComp)
    {
        PetInfoComponent comp = baseComp as PetInfoComponent;
        if (comp == null)
            return;
        if (comp.IsSelect)
            return;
        comp.IsSelect = true;
        comp.PetData.IsNew = false;
        currentPetID = comp.PetData.PetInfo.id;
        UpdateSelectPetInfo();
    }


    public void ButtonEvent_Button_close(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_PETSYSTEM);
    }

    public void ButtonEvent_EquipButton(GameObject btn)
    {
        if (currentPetComp.PetData.IsOwned == false)
            return;
        if (currentPetComp.PetData.IsEquiped)
        {
            PetSystemModule.Instance.SendDressPet(0);
        }
        else
        {
            PetSystemModule.Instance.SendDressPet(currentPetComp.PetData.PetInfo.id);
        }
    }

    public void ButtonEvent_UpgradeButton(GameObject btn)
    {
        if (!currentPetComp.PetData.IsOwned || isSortPet)
        {
            return;
        }
        if (currentPetComp.PetData.Level >= currentPetComp.PetData.PetInfo.changeLevel)
        {
            if (currentPetComp.PetData.PetInfo.suffixPetid == 0)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PET_PETLEVELLIMIT);
                return;
            }
            CombatPetInfo info = ConfigManager.Instance.mCombatPetsConfig.GetPetInfoByID(currentPetComp.PetData.PetInfo.suffixPetid);
            if (info == null)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PET_PETLEVELLIMIT);
                return;
            }
        }
        if (!CommonFunction.CheckMoneyEnough(upgradeMatComp.ConsumeData.Type, upgradeMatComp.ConsumeData.Number, true))
        {
            return;
        }
        if (upgradeMatComp.CompletionData.Number == 0 || PetSystemModule.Instance.IsIgnoreUpgradeTip)
        {
            if (CommonFunction.CheckMoneyEnough(upgradeMatComp.CompletionData.Type, upgradeMatComp.CompletionData.Number, true))
            {
                PetSystemModule.Instance.SendPromotePet(currentPetComp.PetData.PetInfo.id);
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark,
                 string.Format(ConstString.QUICK_MAT_BUY_TIP, CommonFunction.GetMoneyNameByType(upgradeMatComp.CompletionData.Type), upgradeMatComp.CompletionData.Number),
                 () =>
                 {
                     if (CommonFunction.CheckMoneyEnough(upgradeMatComp.CompletionData.Type, upgradeMatComp.CompletionData.Number, true))
                     {
                         PetSystemModule.Instance.SendPromotePet(currentPetComp.PetData.PetInfo.id);
                     }
                 }, null, ConstString.HINT_LEFTBUTTON_GOON, "", (value) =>
                 {
                     PetSystemModule.Instance.IsIgnoreUpgradeTip = value;
                 }, PetSystemModule.Instance.IsIgnoreUpgradeTip);
        }
    }

    public void ButtonEvent_JumpButton(GameObject btn)
    {
        CommonFunction.OpenTargetView((ETaskOpenView)currentPetComp.PetData.PetInfo.viewType);
    }

    public void ButtonEvent_StrengthButton(GameObject btn)
    {
        if (!currentPetComp.PetData.IsOwned || isSortPet)
        {
            return;
        }
        if (!CheckUpgradeSkill(currentPetComp.PetData.Skill, true))
        {
            return;
        }
        if (!CommonFunction.CheckMoneyEnough(skillMatComp.ConsumeData.Type, skillMatComp.ConsumeData.Number, true))
        {
            return;
        }
        if (skillMatComp.CompletionData.Number == 0 || PetSystemModule.Instance.IsIgnoreStrenghTip)
        {
            if (PlayerData.Instance.GoldIsEnough((int)skillMatComp.CompletionData.Type, skillMatComp.CompletionData.Number))
            {
                PetSystemModule.Instance.SendUpgradePetSKill(currentPetComp.PetData.PetInfo.id, currentPetComp.PetData.PetInfo.skillID);
            }
        }
        else
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_YesNo_Mark,
                 string.Format(ConstString.QUICK_MAT_BUY_TIP, CommonFunction.GetMoneyNameByType(skillMatComp.CompletionData.Type), skillMatComp.CompletionData.Number),
                 () =>
                 {
                     if (PlayerData.Instance.GoldIsEnough((int)skillMatComp.CompletionData.Type, skillMatComp.CompletionData.Number))
                     {
                         PetSystemModule.Instance.SendUpgradePetSKill(currentPetComp.PetData.PetInfo.id, currentPetComp.PetData.PetInfo.skillID);
                     }
                 }, null, ConstString.HINT_LEFTBUTTON_GOON, "", (value) =>
                 {
                     PetSystemModule.Instance.IsIgnoreStrenghTip = value;
                 }, PetSystemModule.Instance.IsIgnoreStrenghTip);
        }
    }

    public void ButtonEvent_PetModel(GameObject go)
    {
        if (this.petMainSpine == null) return;
        List<string> tempList = new List<string>();
        tempList.Add(GlobalConst.ANIMATION_NAME_MOVE);
        tempList.Add(GlobalConst.ANIMATION_NAME_VICTORY);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE6);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE8);
        tempList.Add(GlobalConst.ANIMATION_NAME_ABILITY_ULTIMATE10);
        int index = UnityEngine.Random.Range(0, tempList.Count - 1);

        this.petMainSpine.pushAnimation(tempList[index], true, 1);
        this.petMainSpine.EndEvent += (string animationName) =>
        {
            this.petMainSpine.pushAnimation(GlobalConst.ANIMATION_NAME_IDLE, true, 0);
        };

    }
    #endregion

    private bool CheckUpgradeSkill(Skill skill, bool showHint = true)
    {
        if (skill == null)
            return false;
        if (skill.isMaxLevel())
        {
            SkillAttributeInfo temp = ConfigManager.Instance.mSkillAttData.FindById(skill.Att.evolveId);
            if (temp == null)
            {
                if (showHint)
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PET_SKILL_MAXLEVEL);
                }
                return false;
            }
        }
        if (skill.Level >= currentPetComp.PetData.Level)
        {
            if (showHint)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PET_SKILL_PETLEVELLIMIT);
            }
            return false;
        }
        return true;
    }

    public void ReceiveDressPet()
    {
        UpdateInfo();
    }

    public void ReceivePromotePet(uint newID)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.STRENGTH_SUCESS);
        PetData petData = PlayerData.Instance._PetDepot.GetPetDataByID(newID);
        if (petData == null)
            return;
        currentPetComp.UpdateCompInfo(petData);
        UpdateInfo();
    }

    public void ReceiveUpgradePetSKill(uint id)
    {
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.STRENGTH_SUCESS);
        PetData petData = PlayerData.Instance._PetDepot.GetPetDataByID(id);
        if (petData == null)
            return;
        currentPetComp.UpdateCompInfo(petData);
        UpdateInfo();
        PlaySkillIntensifyEffect();
    }


    public override void Uninitialize()
    {
        currentPetComp = null;
        if (PlayerPrefsTool.HasKey(AppPrefEnum.NewPet))
        {
            PlayerPrefsTool.DeleteLocalData(AppPrefEnum.NewPet);
        }
        PlayerData.Instance._PetDepot.UpdatePetStatus();
        ResourceLoadManager.Instance.ReleaseBundleForName(cacheNameList);
        if (cacheNameList != null)
            cacheNameList.Clear();
    }

    public void PlaySkillIntensifyEffect()
    {
        if (Go_SkillIntensifyEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_HEROSKILL, (GameObject gb) => { Go_SkillIntensifyEffect = gb; });

        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_SkillIntensifyEffect, petSkillComp.Gobj_SkillInfo);
    }

    private void SetPetEquipEffect(bool isShow)
    {
        if (Go_PetEquipEffect == null)
        {
            EffectObjectCache.Instance.LoadGameObject(GlobalConst.Effect.PET_EQUIP, (GameObject gb) =>
            {
                if (this != null)
                {
                    Go_PetEquipEffect = CommonFunction.SetParent(gb, view.Gobj_PlayerParent.transform);
                    if (Go_PetEquipEffect != null)
                    {
                        Go_PetEquipEffect.transform.localScale = Vector3.one * 0.7f;
                        Go_PetEquipEffect.transform.localPosition = Vector3.zero;
                        TdSpine.SpineBase tmpSpineBase = Go_PetEquipEffect.GetComponent<TdSpine.SpineBase>();
                        if (tmpSpineBase == null)
                            tmpSpineBase = Go_PetEquipEffect.AddComponent<TdSpine.SpineBase>();
                        if (tmpSpineBase != null)
                        {
                            tmpSpineBase.InitSkeletonAnimation();
                            UIPanel panel = view._uiRoot.GetComponent<UIPanel>();
                            if (panel != null)
                                tmpSpineBase.setSortingOrder(panel.sortingOrder + 1);
                            tmpSpineBase.pushAnimation("animation", true, 1);
                        }
                    }
                }
            });
        }
        if (Go_PetEquipEffect != null)
            Go_PetEquipEffect.SetActive(isShow);
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_EquipButton.gameObject).onClick = ButtonEvent_EquipButton;
        UIEventListener.Get(view.Btn_UpgradeButton.gameObject).onClick = ButtonEvent_UpgradeButton;
        UIEventListener.Get(view.Btn_JumpButton.gameObject).onClick = ButtonEvent_JumpButton;
        UIEventListener.Get(view.Btn_StrengthButton.gameObject).onClick = ButtonEvent_StrengthButton;
        UIEventListener.Get(view.Gobj_PlayerParent).onClick = ButtonEvent_PetModel;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (pet_dic != null)
            pet_dic.Clear();
        if (centerPetAtt_dic != null)
            centerPetAtt_dic.Clear();
        if (topPetAtt_dic != null)
            topPetAtt_dic.Clear();
        upgradeMatComp = null;
        skillMatComp = null;
        petSkillComp = null;
        if (PetObj != null)
        {
            GameObject.Destroy(PetObj);
        }
    }
}
