using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class SoldierEquipAdvancedViewController : UIBase
{
    public SoldierEquipAdvancedView view;

    private Weapon weaponPOD;

    private System.Action callBack;
    public GameObject GO_StarEffect;
    public GameObject Go_ItemEffect;

    private List<SEAMaterialComponent> material_dic;
    private List<SEAAttributeComponent> attributeList;
    private SEAEquipInfoComponent equipInfoComp;

    private List<Weapon> materialList;

    private List<Weapon> selectMaterials;

    private List<SEAStarComponent> starList;

    private Vector3 startPos;
    /// <summary>
    /// 选择材料提供的经验
    /// </summary>
    private int materialExp = 0;
    public override void Initialize()
    {
        if (view == null)
        {
            view = new SoldierEquipAdvancedView();
            view.Initialize();
            BtnEventBinding();
        }
        materialExp = 0;
        if (material_dic == null)
        {
            material_dic = new List<SEAMaterialComponent>();
        }
        if (equipInfoComp == null)
        {
            equipInfoComp = new SEAEquipInfoComponent();
            equipInfoComp.MyStart(view.Gobj_SoldierEquipComp);
        }
        if (starList == null)
        {
            starList = new List<SEAStarComponent>();
        }
        if (attributeList == null)
            attributeList = new List<SEAAttributeComponent>();
        view.Gobj_EquipStarComp.SetActive(false);
        view.Gobj_SoldierEquipMaterialComp.SetActive(false);
        view.Gobj_EquipAttComp.SetActive(false);
        view.UIWrapContent_Grid.onInitializeItem = UpdateMaterials;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent += OnUpStar;
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent += _SoldierEquip_WeaponDepotEvent;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        startPos = view.UIPanel_SoldierEquipMaterial.transform.localPosition;
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierEquipUpStar);
    }
    void _SoldierEquip_WeaponDepotEvent(WeaponChange change, int Slot = -1, ulong uID = 0)
    {
        if (uID == this.weaponPOD.uId)
        {
            this.weaponPOD = PlayerData.Instance._SoldierEquip.FindByUid(this.weaponPOD.uId);
            if (this.weaponPOD == null)
                return;

            this.UpdateViewInfo(this.weaponPOD, this.callBack);
        }
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(SoldierEquipAdvancedView.UIName);
    }

    public void UpdateViewInfo(Weapon pod, System.Action action)
    {
        weaponPOD = pod;
        callBack = action;
        if (pod == null)
            return;
        this.materialExp = pod.starExp;
        UpdateViewInfo();
    }

    private void UpdateViewInfo()
    {
        selectMaterials = new List<Weapon>();
        if (weaponPOD == null)
        {
            return;
        }
        if (equipInfoComp == null) return;
        equipInfoComp.UpdateInfo(weaponPOD);
        UpdateProgress();
        UpdateEquipStars();
        UpdateAttribute();
        if (this.view.Lbl_SoldierEquipAdvancedName)
        {
            this.view.Lbl_SoldierEquipAdvancedName.text = this.weaponPOD.Att.name;
        }
        EquipAttributeInfo tempAtt = ConfigManager.Instance.mEquipData.FindById(this.weaponPOD.Att.evolveId);
        if (tempAtt == null)
            view.Lbl_UpStarTip.text = string.Empty;
        else
            view.Lbl_UpStarTip.text = string.Format(ConstString.PROMPT_UP_STAR_EQUIP, tempAtt.evolveLevel - this.weaponPOD.Att.evolveLevel);
        if (!weaponPOD.isMaxLevel())
            this.view.LessLv.SetActive(true);
        else
            this.view.LessLv.SetActive(false);
        if (this.view.Btn_SoldierEquipAdvanced_Back)
        {

            CommonFunction.UpdateWidgetGray(this.view.Btn_SoldierEquipAdvanced_Back, !weaponPOD.isMaxLevel());
            if (this.weaponPOD.isMaxLevel())
            {
                CommonFunction.SetUILabelColor(this.view.Btn_SoldierEquipAdvanced_Label, true);
                CommonFunction.SetUILabelColor(this.view.Btn_FastSelect_Label, true);
            }
            else
            {
                CommonFunction.SetUILabelColor(this.view.Btn_SoldierEquipAdvanced_Label, false);
                CommonFunction.SetUILabelColor(this.view.Btn_FastSelect_Label, false);
            }
        }
        if (this.view.Btn_FastSelect_Back)
        {
            CommonFunction.UpdateWidgetGray(this.view.Btn_FastSelect_Back, !weaponPOD.isMaxLevel());
        }
        materialList = PlayerData.Instance._SoldierEquip.getWeaponList(CheckEquip);
        if (materialList == null)
            materialList = new List<Weapon>();
        materialList.Reverse();
        Main.Instance.StartCoroutine(CreateMaterials());
    }

    private void UpdateAttribute()
    {
        List<KeyValuePair<string, string>> attribute_dic = GetAttribute();
        if (attribute_dic.Count <= attributeList.Count)
        {
            for (int i = 0; i < attributeList.Count; i++)
            {
                SEAAttributeComponent comp = attributeList[i];
                if (i < attribute_dic.Count)
                {
                    comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
                    comp.mRootObject.SetActive(true);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int count = attributeList.Count;
            for (int i = 0; i < attribute_dic.Count; i++)
            {
                SEAAttributeComponent comp = null;
                if (i < count)
                {
                    comp = attributeList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EquipAttComp, view.Grd_AttGroup.transform);
                    comp = new SEAAttributeComponent();
                    comp.MyStart(go);
                    go.name = "att_" + i;
                    attributeList.Add(comp);
                }
                comp.mRootObject.SetActive(true);
                comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
            }
        }
        view.Grd_AttGroup.Reposition();
    }

    private List<KeyValuePair<string, string>> GetAttribute()
    {
        int exp = 0;
        for (int i = 0; i < selectMaterials.Count; i++)
        {
            Weapon weapon = selectMaterials[i];
            if (weapon == null) continue;
            exp += weapon.getBeExp();
        }
        Weapon after = this.weaponPOD;
        if (exp >= this.weaponPOD.Att.evolveExp)
            after = Weapon.createByID(weaponPOD.Att.evolveId);
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (weaponPOD.InfoAttribute.HP != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.HP - weaponPOD.InfoAttribute.HP;
            }
            upStr = GetUpString(ConstString.hp_max + ":" + weaponPOD.InfoAttribute.HP.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.HPRecovery != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.HPRecovery - weaponPOD.InfoAttribute.HPRecovery;
            }
            upStr = GetUpString(ConstString.hp_revert + ":" + weaponPOD.InfoAttribute.HPRecovery.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HPRecovery, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Attack != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.Attack - weaponPOD.InfoAttribute.Attack;
            }
            upStr = GetUpString(ConstString.phy_atk + ":" + weaponPOD.InfoAttribute.Attack.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.AttDistance != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.AttDistance - weaponPOD.InfoAttribute.AttDistance;
            }
            upStr = GetUpString(ConstString.atk_space + ":" + weaponPOD.InfoAttribute.AttDistance.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AttDistance, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.AttRate != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.AttRate - weaponPOD.InfoAttribute.AttRate;
            }
            upStr = GetUpString(ConstString.atk_interval + ":" + weaponPOD.InfoAttribute.AttRate.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AttRate, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Crit != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.Crit - weaponPOD.InfoAttribute.Crit;
            }
            upStr = GetUpString(ConstString.crt_rate + ":" + weaponPOD.InfoAttribute.Crit.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Energy != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.Energy - weaponPOD.InfoAttribute.Energy;
            }
            upStr = GetUpString(ConstString.energy_max + ":" + weaponPOD.InfoAttribute.Energy.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Energy, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.EnergyRecovery != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.EnergyRecovery - weaponPOD.InfoAttribute.EnergyRecovery;
            }
            upStr = GetUpString(ConstString.energy_revert + ":" + weaponPOD.InfoAttribute.EnergyRecovery.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_EnergyRecovery, upStr);
            attribute_dic.Add(tmp);
        }

        if (weaponPOD.InfoAttribute.MP != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.MP - weaponPOD.InfoAttribute.MP;
            }
            upStr = GetUpString(ConstString.mp_max + ":" + weaponPOD.InfoAttribute.MP.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_MP, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.MPRecovery != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.MPRecovery - weaponPOD.InfoAttribute.MPRecovery;
            }
            upStr = GetUpString(ConstString.mp_revert + ":" + weaponPOD.InfoAttribute.MPRecovery.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_MPRecovery, upStr);
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Tenacity != 0)
        {
            string upStr = string.Empty;
            float upAtt = 0f;
            if (after != null)
            {
                upAtt = after.InfoAttribute.Tenacity - weaponPOD.InfoAttribute.Tenacity;
            }
            upStr = GetUpString(ConstString.tnc_rate + ":" + weaponPOD.InfoAttribute.Tenacity.ToString(), upAtt);
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, upStr);
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }

    private string GetUpString(string before, float up = 0f)
    {
        System.Text.StringBuilder sub = new System.Text.StringBuilder();
        sub.Append("[c4ad87]");
        sub.Append(before);
        if (!up.Equals(0f))
        {
            sub.Append("(");
            sub.Append("[3abd22]+");
            sub.Append(up.ToString());
            sub.Append(")[-]");
        }
        sub.Append("[-]");
        return sub.ToString();
    }

    private void UpdateEquipStars()
    {
        int index = starList.Count;
        if (index < 6)
        {
            for (int i = index; i < 6; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_EquipStarComp, view.Grd_SoldierEquipStarLevel.transform);
                SEAStarComponent comp = new SEAStarComponent();
                comp.MyStart(go);
                go.name = "star_" + i;
                starList.Add(comp);
                go.SetActive(true);
            }
            view.Grd_SoldierEquipStarLevel.Reposition();
        }
        int exp = 0;
        for (int i = 0; i < selectMaterials.Count; i++)
        {
            Weapon weapon = selectMaterials[i];
            if (weapon == null) continue;
            exp += weapon.getBeExp();
        }
        Weapon after = this.weaponPOD;
        //关闭升星选择后星级增加
        //if (exp >= this.weaponPOD.Att.evolveExp && weaponPOD.Att.evolveId != 0 && ConfigManager.Instance.mEquipData.FindById(weaponPOD.Att.evolveId) != null)
        //    after = Weapon.createByID(weaponPOD.Att.evolveId);

        for (int i = 0; i < starList.Count; i++)
        {
            if (i < after.Att.star)
            {
                //if (i == after.Att.star - 1)
                //{
                //    PlayUpStarEffect(starList[i].Spt_SelectSprite.gameObject);
                //}
                starList[i].IsSelect = true;
            }
            else
            {
                starList[i].IsSelect = false;
            }
        }
    }
    private bool CheckEquip(Weapon wp)
    {
        if (wp == null) return false;
        if (wp.isEquiped) return false;
        if (wp.uId == weaponPOD.uId) return false;
        return true;
    }

    private void UpdateMaterials(GameObject go, int wrapIndex, int realindex)
    {
        if (view.UIWrapContent_Grid.enabled == false) return;
        if (realindex >= materialList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
            Weapon weaponPOD = materialList[realindex];
            SEAMaterialComponent comp = material_dic[wrapIndex];
            comp.UpdateInfo(materialList[realindex], go);
            comp.IsSelect = this.IsSelectMaterial(weaponPOD);
        }
    }

    private bool IsSelectMaterial(Weapon weapon)
    {
        for (int i = 0; i < selectMaterials.Count; i++)
        {
            Weapon pod = selectMaterials[i];
            if (pod.uId == weapon.uId)
            {
                return true;
            }
        }
        return false;
    }

    private void UpdateProgress()
    {
        if (weaponPOD == null)
            return;

        int exp = weaponPOD.starExp;
        for (int i = 0; i < selectMaterials.Count; i++)
        {
            Weapon weapon = selectMaterials[i];
            if (weapon == null) continue;
            exp += weapon.getBeExp();
        }
        UpdateAttribute();
        UpdateEquipStars();
        view.Slider_SoldierEquipProgress.value = (float)exp / (float)weaponPOD.Att.evolveExp;
        view.Lbl_SliderSoldierEquipProgress.text = string.Format("{0}/{1}", exp, weaponPOD.Att.evolveExp);
    }
    private IEnumerator CreateMaterials()
    {
        view.Grd_MaterialGrid.Reposition();
        //view.Grd_MaterialGrid.Reposition();
        yield return 0;
        view.ScrView_SoldierEquipMaterial.ResetPosition();
        yield return 0;
        view.UIWrapContent_Grid.CleanChild();
        int MAXCOUNT = 14;
        int count = materialList.Count;
        int itemCount = material_dic.Count;
        int index = Mathf.CeilToInt((float)count / view.UIWrapContent_Grid.highCount) - 1;
        if (index == 0)
            index = 1;
        view.UIWrapContent_Grid.minIndex = 0;
        view.UIWrapContent_Grid.maxIndex = index;
        if (count % 2 != 0)
        {
            view.UIWrapContent_Grid.cullContent = false;
        }
        else
        {
            view.UIWrapContent_Grid.cullContent = true;
        }

        if (count > MAXCOUNT)
        {
            view.UIWrapContent_Grid.enabled = true;
            if (count % 2 != 0)
            {
                view.UIWrapContent_Grid.cullContent = false;
            }
            else
            {
                view.UIWrapContent_Grid.cullContent = true;
            }

            count = MAXCOUNT;
        }
        else
        {
            //view.UIWrapContent_Grid.enabled = true;
            view.UIWrapContent_Grid.enabled = false;
        }
        if (count < itemCount)
        {
            for (int i = count; i < itemCount; i++)
            {
                SEAMaterialComponent comp = material_dic[i];
                comp.mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            //SEAMaterialComponent comp = null;
            Weapon weaponData = materialList[i];
            GameObject vGo = null;
            if (itemCount <= i)
            {
                vGo = CommonFunction.InstantiateObject(view.Gobj_SoldierEquipMaterialComp, view.Grd_MaterialGrid.transform);
                SEAMaterialComponent comp = vGo.GetComponent<SEAMaterialComponent>();
                if (comp == null)
                {
                    comp = vGo.AddComponent<SEAMaterialComponent>();
                    comp.MyStart(vGo);
                }
                vGo.name = "material_" + i.ToString();
                material_dic.Insert(i, comp);
                vGo.SetActive(true);
                UIEventListener.Get(vGo).onClick = ButtonEvent_SoldierEquipMaterial;
                //comp.AddEventListener(ButtonEvent_SoldierEquipMaterial);
            }
            else
            {
                material_dic[i].gameObject.SetActive(true);
            }
            //Debug.LogError(go.name + "----" + wrapIndex);
            material_dic[i].UpdateInfo(materialList[i], vGo);
            if (this.IsSelectMaterial(materialList[i]))
                material_dic[i].IsSelect = true;
            else
                material_dic[i].IsSelect = false;
        }
        //if (view.UIWrapContent_Grid.enabled)
        //    view.UIWrapContent_Grid.ReGetChild();
        //yield return null;
        //view.Grd_MaterialGrid.Reposition();// = true;
        //yield return null;
        //view.ScrView_SoldierEquipMaterial.ResetPosition();
        if (count > 12)
            view.UIWrapContent_Grid.ReGetChild();
        yield return 0;
        view.Grd_MaterialGrid.Reposition();
        //view.Grd_MaterialGrid.Reposition();
        yield return 0;
        view.ScrView_SoldierEquipMaterial.ResetPosition();
        view.Grd_MaterialGrid.gameObject.SetActive(false);
        view.Grd_MaterialGrid.gameObject.SetActive(true);
        this.startPos = this.view.UIPanel_SoldierEquipMaterial.transform.localPosition;
    }

    private void ButtonEvent_SoldierEquipMaterial(GameObject go)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!weaponPOD.isMaxLevel())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.EQUIP_LESSLEVEL, weaponPOD.Att.evolveLevel));
            return;
        }
        SEAMaterialComponent comp = go.GetComponent<SEAMaterialComponent>();
        if (comp == null) return;
        if (!comp.IsSelect)
        {
            if ((materialExp) >= weaponPOD.Att.evolveExp)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.MATERIAL_ENOUGH);
                return;
            }
        }
        comp.IsSelect = !comp.IsSelect;
        UpdateSelectedMaterial(comp.IsSelect, comp.WeaponPOD);
        UpdateProgress();
    }

    private void UpdateSelectedMaterial(bool isAdd, Weapon weapon)
    {
        if (isAdd)
        {
            if (selectMaterials.Contains(weapon))
            {
                return;
            }
            else
            {
                materialExp += weapon.getBeExp();
                selectMaterials.Add(weapon);
            }
        }
        else
        {
            if (selectMaterials.Contains(weapon))
            {
                selectMaterials.Remove(weapon);
                materialExp -= weapon.getBeExp();
                if (materialExp <= 0)
                {
                    materialExp = 0;
                }
            }
        }
    }

    private List<System.UInt64> GetSelectedData()
    {
        List<System.UInt64> list = new List<ulong>();
        for (int i = 0; i < selectMaterials.Count; i++)
        {
            list.Add(selectMaterials[i].uId);
        }
        return list;
    }

    public void ButtonEvent_SoldierEquipAdvanced(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_LevelUp_Quality, view._uiRoot.transform.parent.transform));
        if (!weaponPOD.isMaxLevel())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.EQUIP_LESSLEVEL, weaponPOD.Att.evolveLevel));
            return;
        }
        List<ulong> list = GetSelectedData();
        if (list == null || list.Count == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_SOLDIEREQUIPNOTSELECT);
            return;
        }
        foreach (ulong uid in list)
        {
            Weapon choose = PlayerData.Instance._SoldierEquip.FindByUid(uid);
            if (choose == null)
                continue;
            ChooseCheck check = this.ChooseFilter(choose);
            if (check != ChooseCheck.OK)
            {
                string message = "";
                if (check == ChooseCheck.HadHightQuality)
                    message = ConstString.CHOOSED_HAD_HIGHTQUALITY_EQUIP;
                if (check == ChooseCheck.HadHighStar)
                    message = ConstString.CHOOSED_HAD_HIGHTSTAR_EQUIP;
                UISystem.Instance.HintView.ShowMessageBox(
                    MessageBoxType.mb_YesNo,
                    message,
                    () =>
                    {
                        PlayerData.Instance._SoldierEquip.UpStar(weaponPOD.Slot, list);
                    });

                return;
            }
        }

        PlayerData.Instance._SoldierEquip.UpStar(weaponPOD.Slot, list);
    }

    public void ButtonEvent_FastSelect(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!weaponPOD.isMaxLevel())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.EQUIP_LESSLEVEL, weaponPOD.Att.evolveLevel));
            return;
        }

        if (materialList == null || materialList.Count == 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_SOLDIEREQUIPNOMATERIAL);
            return;
        }
        Vector3 afterPos = view.UIPanel_SoldierEquipMaterial.transform.localPosition;
        float increment = Math.Abs(afterPos.x - startPos.x);
        int num = (int)increment / 100;
        int k = 12;
        int j = 0;
        if ((increment % 100) <= 88 && (increment % 100) >= 12)
            k += 2;
        if (increment % 100 >= 90 || increment % 100 == 0)
        {
            if (increment != 0)
            {
                j += 2;
                k += 2;
            }
        }
        var temp = view.Grd_MaterialGrid.GetChildList();

        List<Transform> list = new List<Transform>(temp);
        list.Sort((left, right) =>
        {
            if (left.localPosition.x != right.localPosition.x)
            {
                if (left.localPosition.x < right.localPosition.x)
                    return -1;
                else
                    return 1;
            }
            if (left.localPosition.y != right.localPosition.y)
            {
                if (left.localPosition.y > right.localPosition.y)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        k = k < list.Count ? k : list.Count;
        for (int i = j; i < k; ++i)
        {
            //GameObject mark = list[i].FindChild("SelectSprite").gameObject;
            SEAMaterialComponent comp = list[i].gameObject.GetComponent<SEAMaterialComponent>();

            if (comp == null) continue;
            if (!selectMaterials.Contains(comp.WeaponPOD))
            {
                if (this.materialExp >= this.weaponPOD.Att.evolveExp)
                {
                    //UpdateProgress();
                    break;
                }
                if (this.ChooseFilter(comp.WeaponPOD) != ChooseCheck.OK)
                    continue;
                this.materialExp += comp.WeaponPOD.getBeExp();
                comp.IsSelect = true;
                //mark.SetActive(true);
                //UISprite tempSprite = mark.GetComponent<UISprite>();
                //if (tempSprite)
                //    tempSprite.enabled = true;
                selectMaterials.Add(comp.WeaponPOD);
            }
        }
        UpdateProgress();
    }

    public void ButtonEvent_CloseEquipAdvance(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        CloseView();
    }

    private void CloseView()
    {
        if (callBack != null)
            callBack();
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPADVANCED);
    }

    public void OnUpStar(EquipControl control, int errorCode)
    {
        switch (control)
        {
            case EquipControl.SoldierEquipStarResp:
                {
                    if (errorCode == (int)ErrorCodeEnum.SUCCESS && !this.weaponPOD.isMaxLevel())
                    {
                        this.weaponPOD = PlayerData.Instance._SoldierEquip.FindByUid(this.weaponPOD.uId);
                        if (this.weaponPOD != null)
                            this.UpdateViewInfo(this.weaponPOD, this.callBack);
                    }
                    // CloseView();
                    else
                    {
                    }
                } break;
        }
    }

    public override void Uninitialize()
    {
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent -= OnUpStar;
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent -= _SoldierEquip_WeaponDepotEvent;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SoldierEquipAdvanced.gameObject).onClick = ButtonEvent_SoldierEquipAdvanced;
        UIEventListener.Get(view.Btn_FastSelect.gameObject).onClick = ButtonEvent_FastSelect;
        UIEventListener.Get(view.Btn_CloseEquipAdvance.gameObject).onClick = ButtonEvent_CloseEquipAdvance;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        materialList.Clear();
        starList.Clear();
        equipInfoComp = null;
        material_dic.Clear();
        attributeList.Clear();
    }
    //========================================================================================//
    public void PlayEquipAdvancedEffect()
    {
        if (Go_ItemEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_EQUIPADVANCED, (GameObject gb) => { Go_ItemEffect = gb; });

        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_ItemEffect, view.Gobj_SoldierEquipComp.transform);
    }
    public void PlayUpStarEffect(GameObject target)
    {
        if (GO_StarEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_UPSTAR, (GameObject gb) => { GO_StarEffect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(GO_StarEffect, target.transform);
    }
    public ChooseCheck ChooseFilter(Weapon wp)
    {
        if (wp.Att.quality >= 4)
            return ChooseCheck.HadHightQuality;
        if (wp.Att.star >= 4)
            return ChooseCheck.HadHighStar;
        if (wp.isEquiped)
            return ChooseCheck.HadEquip;

        return ChooseCheck.OK;
    }
}
