using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using fogs.proto.msg;

public class LifeSpiritIntensifyViewController : UIBase
{
    public LifeSpiritIntensifyView view;

    private List<LifeSpiritAttComponent> lifeSpiritAtt_dic;
    private List<LifeSpiritIntensifyMaterialComponent> lifeSpiritMaterial_dic;

    private LifeSoulData lifeSoulData;

    private List<LifeSoulData> materialsList;
    private List<LifeSoulData> selectMaterialsList;

    private int currentLifeSoulExp = 0;  //当前命魂等级经验
    private int selectMaterialExp;  //当前选择材料等级
    private int upgradelevel = 0;   //当前等级
    private int currentSurplusExp;  //当前等级剩余经验
    private UpgradeExpEnum upgradeExpEnum;
    private enum UpgradeExpEnum
    {
        None,
        Player,
        Low,
        Equal,
        High
    }

    public override void Initialize()
    {
        if (view == null)
        {
            view = new LifeSpiritIntensifyView();
            view.Initialize();
            BtnEventBinding();
        }
        if (lifeSpiritAtt_dic == null)
            lifeSpiritAtt_dic = new List<LifeSpiritAttComponent>();
        if (lifeSpiritMaterial_dic == null)
            lifeSpiritMaterial_dic = new List<LifeSpiritIntensifyMaterialComponent>();
        if (selectMaterialsList == null)
            selectMaterialsList = new List<LifeSoulData>();
        selectMaterialsList.Clear();
        view.Gobj_IntensifyLabelItem.SetActive(false);
        view.Gobj_LifeSpiritAttComp.SetActive(false);
        view.Gobj_MaterialComp.SetActive(false);
        //view.UIWrapContent_MaterialGrid.onInitializeItem = UpdateWrapMaterial;
        currentLifeSoulExp = 0;
    }

    #region Update Event

    //private void UpdateWrapMaterial(GameObject go, int wrapIndex, int realIndex)
    //{
    //    if (materialsList == null)
    //        return;
    //    go.SetActive(false);
    //    if (realIndex >= materialsList.Count)
    //    {
    //        go.SetActive(false);
    //        return;
    //    }
    //    else
    //    {
    //        //go.SetActive(true);
    //        LifeSpiritIntensifyMaterialComponent comp = lifeSpiritMaterial_dic[wrapIndex];
    //        if (comp == null) return;
    //        LifeSoulData soulData = materialsList[realIndex];
    //        comp.UpdateCompInfo(soulData);
    //        if (selectMaterialsList.Contains(soulData.SoulPOD.uid))
    //        {
    //            comp.IsSelect = true;
    //        }
    //        else
    //        {
    //            comp.IsSelect = false;
    //        }
    //    }

    //}

    public void UpdateViewInfo(LifeSoulData data)
    {
        selectMaterialsList.Clear();
        lifeSoulData = data;
        if (lifeSoulData == null)
            return;
        currentLifeSoulExp = 0;
        selectMaterialExp = 0;
        currentSurplusExp = lifeSoulData.SoulPOD.exp;
        for (int i = 0; i < lifeSoulData.SoulInfo.upgrade_consume.Count; i++)
        {
            UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume[i];
            if (info == null)
                continue;
            if (info.level < lifeSoulData.SoulPOD.level)
            {
                currentLifeSoulExp += info.exp;
            }
        }
        currentLifeSoulExp += lifeSoulData.SoulPOD.exp;
        upgradelevel = lifeSoulData.SoulPOD.level;
        UpdateLifeSpiritInfo();
        materialsList = PlayerData.Instance._LifeSoulDepot.GetAllLifeSoulsExcept(lifeSoulData.SoulPOD.uid);
        materialsList.Sort((left, right) =>
        {
            if (left == null || right == null)
                return 0;
            if (left.SoulPOD.level != right.SoulPOD.level)
            {
                if (left.SoulPOD.level < right.SoulPOD.level)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
            if (left.SoulInfo.quality != right.SoulInfo.quality)
            {
                if (left.SoulInfo.quality < right.SoulInfo.quality)
                {
                    return -1;
                }
                else
                {
                    return 1;
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
        Main.Instance.StartCoroutine(UpdateMaterials());
    }

    private void UpdateLifeSpiritInfo()
    {
        CommonFunction.SetSpriteName(view.Spt_LifeSpiritIcon, lifeSoulData.SoulInfo.icon);
        CommonFunction.SetQualitySprite(view.Spt_LifeSpiritQuality, lifeSoulData.SoulInfo.quality, view.Spt_LifeSpiritFrame);
        CommonFunction.SetLifeSpiritTypeMark(view.Spt_LifeSpiritTypeMark, lifeSoulData.SoulInfo.godEquip);
        view.Lbl_LifeSpiritName.text = lifeSoulData.SoulInfo.name;
        view.Lbl_LevelLabelLeft.text = "LV." + lifeSoulData.SoulPOD.level.ToString();
        if (lifeSoulData.SoulPOD.level >= lifeSoulData.SoulInfo.levelLimit)
        {
            view.Spt_LifeSpiritMaxLevel.enabled = true;
            view.Gobj_AfterGroup.SetActive(false);
        }
        else
        {
            view.Spt_LifeSpiritMaxLevel.enabled = false;
            view.Gobj_AfterGroup.SetActive(true);
            this.view.Lbl_LevelLabelRight.text = "LV." + upgradelevel.ToString();
        }
        if (lifeSoulData.SoulInfo.skillID == 0)
        {
            view.Lbl_SkillAttDesc.enabled = false;
            view.Grd_AttGrid.gameObject.SetActive(true);
            UpdateLifeSpiritDetailAttValue();
        }
        else
        {
            view.Lbl_SkillAttDesc.enabled = true;
            view.Grd_AttGrid.gameObject.SetActive(false);
            view.Lbl_SkillAttDesc.text = lifeSoulData.Skill.GetDescript(upgradelevel);
        }
        UpdateExpProgress();
    }

    private IEnumerator UpdateMaterials()
    {
        //int MAXCOUNT = 16;
        int count = materialsList.Count;
        int itemCount = lifeSpiritMaterial_dic.Count;
        //int index = Mathf.CeilToInt((float)count / view.UIWrapContent_MaterialGrid.highCount) - 1;
        //if (index == 0)
        //    index = 1;
        //view.UIWrapContent_MaterialGrid.minIndex = 1;
        //view.UIWrapContent_MaterialGrid.maxIndex = index;
        //if (count > MAXCOUNT)
        //{
        //    view.UIWrapContent_MaterialGrid.enabled = true;
        //    count = MAXCOUNT;
        //}
        //else
        //{
        //    view.UIWrapContent_MaterialGrid.enabled = false;
        //}
        //yield return null;
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                lifeSpiritMaterial_dic[i].mRootObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            LifeSoulData data = materialsList[i];
            LifeSpiritIntensifyMaterialComponent comp = null;
            if (i < itemCount)
            {

                comp = lifeSpiritMaterial_dic[i];
            }
            else
            {
                GameObject vGo = CommonFunction.InstantiateObject(view.Gobj_MaterialComp, view.Grd_MaterialGrid.transform);
                comp = new LifeSpiritIntensifyMaterialComponent();
                vGo.name = i.ToString();
                comp.MyStart(vGo);
                comp.AddEventListener(ButtonEvent_Material);
                lifeSpiritMaterial_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.mRootObject.SetActive(true);
            comp.IsSelect = false;
            comp.UpdateCompInfo(data);
        }
        //yield return null;
        //view.UIWrapContent_MaterialGrid.ReGetChild();
        yield return null;
        view.Grd_MaterialGrid.Reposition();
        yield return null;
        view.ScrView_MaterialScroll.ResetPosition();
    }

    private void UpdateLifeSpiritDetailAttValue()
    {
        List<KeyValuePair<string, string>> attributeList = GetWeaponAttributeDesc();
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
                GameObject go = CommonFunction.InstantiateObject(view.Gobj_LifeSpiritAttComp, view.Grd_AttGrid.transform);
                comp = new LifeSpiritAttComponent();
                comp.MyStart(go);
                lifeSpiritAtt_dic.Add(comp);
            }
            if (comp == null) continue;
            comp.UpdateInfo(tmpInfo.Key, tmpInfo.Value);
            comp.mRootObject.SetActive(true);
        }
        view.Grd_AttGrid.Reposition();
    }


    private List<KeyValuePair<string, string>> GetWeaponAttributeDesc()
    {
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        ShowInfoWeapon data = new ShowInfoWeapon();
        data.HP = lifeSoulData.SoulInfo.hp_initial + lifeSoulData.SoulInfo.hp_up * (lifeSoulData.SoulPOD.level - 1);
        data.Attack = lifeSoulData.SoulInfo.attack_initial + lifeSoulData.SoulInfo.attack_up * (lifeSoulData.SoulPOD.level - 1);
        data.Accuracy = lifeSoulData.SoulInfo.accrate_initial + lifeSoulData.SoulInfo.accrate_up * (lifeSoulData.SoulPOD.level - 1);
        data.Dodge = lifeSoulData.SoulInfo.ddgrate_initial + lifeSoulData.SoulInfo.ddgrate_up * (lifeSoulData.SoulPOD.level - 1);
        data.Crit = lifeSoulData.SoulInfo.crt_initial + lifeSoulData.SoulInfo.crt_up * (lifeSoulData.SoulPOD.level - 1);
        data.Tenacity = lifeSoulData.SoulInfo.tenacity_initial + lifeSoulData.SoulInfo.tenacity_up * (lifeSoulData.SoulPOD.level - 1);
        data.HP = Mathf.CeilToInt(data.HP / 10000);
        data.Attack = Mathf.CeilToInt(data.Attack / 10000);
        data.Accuracy = Mathf.CeilToInt(data.Accuracy / 10000);
        data.Dodge = Mathf.CeilToInt(data.Dodge / 10000);
        data.Crit = Mathf.CeilToInt(data.Crit / 10000);
        data.Tenacity = Mathf.CeilToInt(data.Tenacity / 10000);

        ShowInfoWeapon upData = new ShowInfoWeapon();
        upData.HP = lifeSoulData.SoulInfo.hp_initial + lifeSoulData.SoulInfo.hp_up * (upgradelevel - 1);
        upData.Attack = lifeSoulData.SoulInfo.attack_initial + lifeSoulData.SoulInfo.attack_up * (upgradelevel - 1);
        upData.Accuracy = lifeSoulData.SoulInfo.accrate_initial + lifeSoulData.SoulInfo.accrate_up * (upgradelevel - 1);
        upData.Dodge = lifeSoulData.SoulInfo.ddgrate_initial + lifeSoulData.SoulInfo.ddgrate_up * (upgradelevel - 1);
        upData.Crit = lifeSoulData.SoulInfo.crt_initial + lifeSoulData.SoulInfo.crt_up * (upgradelevel - 1);
        upData.Tenacity = lifeSoulData.SoulInfo.tenacity_initial + lifeSoulData.SoulInfo.tenacity_up * (upgradelevel - 1);
        upData.HP = Mathf.CeilToInt(upData.HP / 10000);
        upData.Attack = Mathf.CeilToInt(upData.Attack / 10000);
        upData.Accuracy = Mathf.CeilToInt(upData.Accuracy / 10000);
        upData.Dodge = Mathf.CeilToInt(upData.Dodge / 10000);
        upData.Crit = Mathf.CeilToInt(upData.Crit / 10000);
        upData.Tenacity = Mathf.CeilToInt(upData.Tenacity / 10000);

        string color = "[c4ad87]";
        if (data.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, GetUpString(color, data.HP, upData.HP));
            attribute_dic.Add(tmp);
        }
        if (data.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, GetUpString(color, data.Attack, upData.Attack));
            attribute_dic.Add(tmp);
        }
        if (data.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, GetUpString(color, data.Accuracy, upData.Accuracy));
            attribute_dic.Add(tmp);
        }
        if (data.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, GetUpString(color, data.Dodge, upData.Dodge));
            attribute_dic.Add(tmp);
        }
        if (data.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, GetUpString(color, data.Crit, upData.Crit));
            attribute_dic.Add(tmp);
        }
        if (data.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, GetUpString(color, data.Tenacity, upData.Tenacity));
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }

    private string GetUpString(string str, int att, int upAtt)
    {
        string upStr = string.Empty;
        str += att;
        if (upAtt != 0)
        {
            str += "[3abd22]->" + upAtt + "[-]";
        }
        upStr = str;
        return upStr;
    }

    private void UpdateSelectMaterials()
    {
        for (int i = 0; i < lifeSpiritMaterial_dic.Count; i++)
        {
            LifeSpiritIntensifyMaterialComponent comp = lifeSpiritMaterial_dic[i];
            if (comp == null)
                continue;
            LifeSoulData data = selectMaterialsList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulPOD.uid == comp.LifeSoulData.SoulPOD.uid;
            });
            if (data == null)
                comp.IsSelect = false;
            else
                comp.IsSelect = true;
        }
    }

    private void UpdateExpProgress()
    {
        if (upgradelevel >= lifeSoulData.SoulInfo.levelLimit)
        {
            view.Slider_LifeSpiritProgressBar.value = 1;
            if (lifeSoulData.SoulInfo.upgrade_consume != null && lifeSoulData.SoulInfo.upgrade_consume.Count > 0)
            {
                UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume[lifeSoulData.SoulInfo.upgrade_consume.Count - 1];
                view.Lbl_SliderProgressBarLabel.text = "[ff3600]" + string.Format(ConstString.PVP_TODAYSURPLUSCOUNT, currentSurplusExp, info.exp) + "[-]";
            }
            else
            {
                view.Lbl_SliderProgressBarLabel.text = "[ff3600]0/0[-]";
            }
        }
        else
        {
            UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.level == upgradelevel;
            });
            int exp = lifeSoulData.SoulPOD.exp;
            if (info != null)
            {
                exp = info.exp;
            }
            string text = string.Format(ConstString.PVP_TODAYSURPLUSCOUNT, currentSurplusExp, exp);
            if (upgradeExpEnum == UpgradeExpEnum.High)
            {
                text = "[ff3600]" + text + "[-]";
            }
            view.Lbl_SliderProgressBarLabel.text = text;
            view.Slider_LifeSpiritProgressBar.value = (float)currentSurplusExp / (float)exp;
        }
    }


    private bool IsCanUpgrade()
    {
        if (lifeSoulData.SoulPOD.level >= lifeSoulData.SoulInfo.levelLimit)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_UPGRADE_LEVELLIMIT);
            return false;
        }
        return true;
    }

    #endregion

    private bool IsCanSelectMaterial()
    {
        if (lifeSoulData.SoulPOD.level > PlayerData.Instance._Level)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_PLAYERLEVELLIMIT);
            return false;
        }
        if (lifeSoulData.SoulPOD.level > lifeSoulData.SoulInfo.levelLimit)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HAD_MAX_LEVEL);
            return false;
        }
        return true;
    }

    private UpgradeExpEnum CheckAddExp(int addexp)
    {
        if (upgradelevel > PlayerData.Instance._Level)
        {
            return UpgradeExpEnum.Player;
        }
        if (upgradelevel >= lifeSoulData.SoulInfo.levelLimit)  //已经最大等级
        {
            return UpgradeExpEnum.None;
        }
        int tmpSelectExp = selectMaterialExp;
        int tmpUpgradeLevel = upgradelevel;
        tmpSelectExp += addexp;
        int exp = 0;
        UpgradeCostInfo info2 = null;
        for (int i = 0; i < lifeSoulData.SoulInfo.upgrade_consume.Count; i++)
        {
            UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume[i];
            if (info == null)
                continue;
            int exp2 = exp + info.exp;
            if (exp2 < (tmpSelectExp + currentLifeSoulExp))
            {
                info2 = info;
                exp += info.exp;
                tmpUpgradeLevel = info.level + 1;
            }
            else if ((tmpSelectExp + currentLifeSoulExp) == exp2)
            {
                info2 = info;
                exp = tmpSelectExp + currentLifeSoulExp;
                tmpUpgradeLevel = info.level + 1;
            }
            else
            {
                tmpUpgradeLevel = info.level;
                break;
            }
        }
        if (tmpUpgradeLevel > PlayerData.Instance._Level)
        {
            return UpgradeExpEnum.Player;
        }
        if (tmpUpgradeLevel > lifeSoulData.SoulInfo.levelLimit)
        {
            return UpgradeExpEnum.None;
        }
        int tmpCurrent2 = (tmpSelectExp + currentLifeSoulExp) - exp;
        if (tmpCurrent2 > 0 && tmpUpgradeLevel >= lifeSoulData.SoulInfo.levelLimit)
        {
            if (lifeSoulData.SoulInfo.upgrade_consume != null && lifeSoulData.SoulInfo.upgrade_consume.Count > 0)
            {
                UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume[lifeSoulData.SoulInfo.upgrade_consume.Count - 1];
                currentSurplusExp = tmpSelectExp + currentLifeSoulExp - exp + info.exp;
                upgradelevel = tmpUpgradeLevel;
                selectMaterialExp = tmpSelectExp;

            }
            return UpgradeExpEnum.High;
        }
        else
        {
            currentSurplusExp = tmpSelectExp + currentLifeSoulExp - exp;
            upgradelevel = tmpUpgradeLevel;
            selectMaterialExp = tmpSelectExp;
            return UpgradeExpEnum.Low;
        }
    }

    private void ReduceExp(int reduceExp)
    {
        selectMaterialExp -= reduceExp;
        int exp = 0;
        for (int i = 0; i < lifeSoulData.SoulInfo.upgrade_consume.Count; i++)
        {
            UpgradeCostInfo info = lifeSoulData.SoulInfo.upgrade_consume[i];
            if (info == null)
                continue;
            int exp2 = exp + info.exp;
            if (exp2 < (selectMaterialExp + currentLifeSoulExp))
            {
                exp += info.exp;
                upgradelevel = info.level;
            }
            else if (exp2 == (selectMaterialExp + currentLifeSoulExp))
            {
                exp = selectMaterialExp + currentLifeSoulExp;
                upgradelevel = info.level + 1;
                break;
            }
            else
            {
                upgradelevel = info.level;
                break;
            }
        }
        int tmpCurrent2 = (selectMaterialExp + currentLifeSoulExp) - exp;
        if (tmpCurrent2 > 0 && upgradelevel >= lifeSoulData.SoulInfo.levelLimit)
        {
            currentSurplusExp = selectMaterialExp + currentLifeSoulExp - exp;
        }
        else
        {
            currentSurplusExp = selectMaterialExp + currentLifeSoulExp - exp;
            upgradeExpEnum = UpgradeExpEnum.Low;
        }
    }


    private void RemoveSelectMaterial(LifeSoulData info)
    {
        List<LifeSoulData> list = new List<LifeSoulData>();
        for (int i = 0; i < selectMaterialsList.Count; i++)
        {
            LifeSoulData data = selectMaterialsList[i];
            if (data == null)
                continue;
            if (data.SoulPOD.uid != info.SoulPOD.uid)
                list.Add(data);
        }
        selectMaterialsList = list;
    }

    #region Button Event

    private void ButtonEvent_Material(BaseComponent basecomp)
    {
        LifeSpiritIntensifyMaterialComponent comp = basecomp as LifeSpiritIntensifyMaterialComponent;
        if (comp == null)
            return;
        if (comp.IsSelect)
        {
            comp.IsSelect = false;
            int reduceExp = comp.LifeSoulData.SoulInfo.materialExp;
            for (int j = 0; j < comp.LifeSoulData.SoulInfo.upgrade_consume.Count; j++)
            {
                UpgradeCostInfo expInfo = comp.LifeSoulData.SoulInfo.upgrade_consume[j];
                if (expInfo == null)
                    continue;
                if (expInfo.level < comp.LifeSoulData.SoulPOD.level)
                {
                    reduceExp += expInfo.exp;
                }
                else
                {
                    break;
                }
            }
            reduceExp += comp.LifeSoulData.SoulPOD.exp;
            ReduceExp(reduceExp);
            RemoveSelectMaterial(comp.LifeSoulData);
            UpdateLifeSpiritInfo();
        }
        else
        {
            if (!IsCanSelectMaterial())
            {
                return;
            }
            int addExp = comp.LifeSoulData.SoulInfo.materialExp;
            for (int j = 0; j < comp.LifeSoulData.SoulInfo.upgrade_consume.Count; j++)
            {
                UpgradeCostInfo expInfo = comp.LifeSoulData.SoulInfo.upgrade_consume[j];
                if (expInfo == null)
                    continue;
                if (expInfo.level < comp.LifeSoulData.SoulPOD.level)
                {
                    addExp += expInfo.exp;
                }
                else
                {
                    break;
                }
            }
            addExp += comp.LifeSoulData.SoulPOD.exp;
            upgradeExpEnum = CheckAddExp(addExp);
            switch (upgradeExpEnum)
            {
                case UpgradeExpEnum.Player:
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_PLAYERLEVELLIMIT);
                    } break;
                case UpgradeExpEnum.None:
                    {
                        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_LEVELLIMIT);
                    } break;
                case UpgradeExpEnum.Low:
                case UpgradeExpEnum.Equal:
                case UpgradeExpEnum.High:
                    {
                        selectMaterialsList.Add(comp.LifeSoulData);
                        comp.IsSelect = true;
                        UpdateLifeSpiritInfo();
                    } break;
            }

        }
    }

    public void ButtonEvent_IntensifyButton(GameObject btn)
    {
        if (IsCanUpgrade())
        {
            if (selectMaterialsList != null && selectMaterialsList.Count > 0)
            {
                ulong uid = 0;
                if (lifeSoulData.IsEquipedPlayer)
                {
                    uid = 1;
                }
                else if (lifeSoulData.IsEquipedSoldier)
                {
                    uid = lifeSoulData.EquipedSoldier.uId;
                }
                else
                {
                    uid = 0;
                }
                List<ulong> list = new List<ulong>();
                for (int i = 0; i < selectMaterialsList.Count; i++)
                {
                    LifeSoulData data = selectMaterialsList[i];
                    if (data == null)
                        continue;
                    list.Add(data.SoulPOD.uid);
                }
                LifeSpiritModule.Instance.SendUpgradeSoul(lifeSoulData.SoulPOD.uid, uid, list);
            }
            else
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_NOCOUNT);
            }
        }
    }



    private void ButtonEvent_CloseView(GameObject go)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_LIFESPIRITINTENSIFY);
    }

    public void ButtonEvent_FastSelectButton(GameObject btn)
    {
        if (lifeSpiritMaterial_dic == null || lifeSpiritMaterial_dic.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_SELECTMATERIAL_NOCOUNT);
            return;
        }
        for (int i = 0; i < materialsList.Count; i++)
        {
            LifeSoulData data = materialsList[i];
            if (data == null)
                continue;
            if (data.SoulInfo.quality >= (int)ItemQualityEnum.Purple)
                continue;
            if (data.SoulPOD.level > 1)
                continue;
            LifeSoulData selectData = selectMaterialsList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.SoulPOD.uid == data.SoulPOD.uid;
            });
            if (selectData != null)
                continue;
            int addExp = data.SoulInfo.materialExp;
            for (int j = 0; j < data.SoulInfo.upgrade_consume.Count; j++)
            {
                UpgradeCostInfo expInfo = data.SoulInfo.upgrade_consume[j];
                if (expInfo == null)
                    continue;
                if (data.SoulPOD.level < expInfo.level)
                {
                    addExp += expInfo.exp;
                }
                else
                {
                    break;
                }
            }
            addExp += data.SoulPOD.exp;
            int exp = currentLifeSoulExp;
            int level = upgradelevel;
            upgradeExpEnum = CheckAddExp(addExp);
            if (upgradeExpEnum == UpgradeExpEnum.Player || upgradeExpEnum == UpgradeExpEnum.None)
            {
                currentLifeSoulExp = exp;
                level = upgradelevel;
                break;
            }
            else
            {
                selectMaterialsList.Add(data);
            }
        }

        if (selectMaterialsList == null || selectMaterialsList.Count <= 0)
        {
            return;
            //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_SELECTMATERIAL_NOCOUNT);
        }
        //else
        //{
        //switch (upgradeExpEnum)
        //{
        //    case UpgradeExpEnum.None:
        //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_LEVELLIMIT);
        //        return;
        //    case UpgradeExpEnum.Player:
        //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.LIFESPIRITINTENSIFY_LIFESPIRIT_PLAYERLEVELLIMIT);
        //        return;
        //}
        //}
        UpdateLifeSpiritInfo();
        UpdateSelectMaterials();
    }

    #endregion

    private void IntensifyEffect()
    {
        SetParticleSortingLayer Par_1, Par_2;
        Par_1 = view.Go_FlyEffect.GetComponent<SetParticleSortingLayer>();
        Par_2 = view.Go_ItemEffect.GetComponent<SetParticleSortingLayer>();

        CommonFunction.ResetParticlePanelOrder(view.Gobj_IntensifyLabelItem, view._uiRoot.gameObject, Par_1);
        CommonFunction.ResetParticlePanelOrder(view.Gobj_IntensifyLabelItem, view._uiRoot.gameObject, Par_2);

        List<Transform> itemList = view.Grd_MaterialGrid.GetChildList();
        int itemCount = 4;
        if (itemList.Count < 4)
        {
            itemCount = itemList.Count;
        }
        for (int i = 0; i < itemCount; i++)
        {
            Main.Instance.StartCoroutine(FlayEffect(i * 0.3F, itemList[i].transform, view.Spt_LifeSpiritQuality.gameObject, 1F));
        }
        if (lifeSoulData.SoulInfo.skillID == 0)
        {
            Main.Instance.StartCoroutine(ShowIntensifyEffect(0.075F, view.Spt_LifeSpiritQuality.gameObject));
        }

    }

    private IEnumerator FlayEffect(float time, Transform From, GameObject To, float FlyTimes)
    {
        yield return new WaitForSeconds(time);
        GameObject go = ShowEffectManager.Instance.ShowEffect(view.Go_FlyEffect, From);
        iTween.MoveTo(go, To.transform.position, FlyTimes);
    }
    private IEnumerator ShowIntensifyEffect(float time, GameObject From)
    {
        yield return new WaitForSeconds(time);
        GameObject ItemGo = ShowEffectManager.Instance.ShowEffect(view.Go_ItemEffect, From.transform);
        Main.Instance.StartCoroutine(CreateIntensifyAttribute(0.1F));
    }

    List<GameObject> LabelList = new List<GameObject>();
    private IEnumerator CreateIntensifyAttribute(float time)
    {
        ShowInfoWeapon temp = new ShowInfoWeapon();
        temp.HP = lifeSoulData.SoulInfo.hp_initial + lifeSoulData.SoulInfo.hp_up * (lifeSoulData.SoulPOD.level - 1);
        temp.Attack = lifeSoulData.SoulInfo.attack_initial + lifeSoulData.SoulInfo.attack_up * (lifeSoulData.SoulPOD.level - 1);
        temp.Accuracy = lifeSoulData.SoulInfo.accrate_initial + lifeSoulData.SoulInfo.accrate_up * (lifeSoulData.SoulPOD.level - 1);
        temp.Dodge = lifeSoulData.SoulInfo.ddgrate_initial + lifeSoulData.SoulInfo.ddgrate_up * (lifeSoulData.SoulPOD.level - 1);
        temp.Crit = lifeSoulData.SoulInfo.crt_initial + lifeSoulData.SoulInfo.crt_up * (lifeSoulData.SoulPOD.level - 1);
        temp.Tenacity = lifeSoulData.SoulInfo.tenacity_initial + lifeSoulData.SoulInfo.tenacity_up * (lifeSoulData.SoulPOD.level - 1);
        temp.HP = Mathf.CeilToInt(temp.HP / 10000);
        temp.Attack = Mathf.CeilToInt(temp.Attack / 10000);
        temp.Accuracy = Mathf.CeilToInt(temp.Accuracy / 10000);
        temp.Dodge = Mathf.CeilToInt(temp.Dodge / 10000);
        temp.Crit = Mathf.CeilToInt(temp.Crit / 10000);
        temp.Tenacity = Mathf.CeilToInt(temp.Tenacity / 10000);
        yield return new WaitForSeconds(time);
        if (temp.HP > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.hp_max + temp.HP);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Attack > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.phy_atk + temp.Attack);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Accuracy > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.acc_rate + temp.Accuracy);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Dodge > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.ddg_rate + temp.Dodge);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Crit > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.crt_rate + temp.Crit);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Tenacity > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Gobj_IntensifyLabelItem, view.Spt_LifeSpiritQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.tnc_rate + temp.Tenacity);
            LabelObj.SetActive(true);
        }
    }

    public void OnIntensifySuccess(UpgradeSoulResp data)
    {
        PlayerData.Instance.UpdateItem(data.update_items);
        if (lifeSoulData.IsEquipedPlayer)
        {
            PlayerData.Instance.UpdatePlayerAttribute(data.attr);
        }
        else if (lifeSoulData.IsEquipedSoldier)
        {
            lifeSoulData.EquipedSoldier.SerializeShowInfo(data.attr);
        }
        int level = lifeSoulData.SoulPOD.level;
        PlayerData.Instance._LifeSoulDepot.DeleteLifeSouls(data.consume_uid);
        PlayerData.Instance._LifeSoulDepot.Update(data.soul);
        lifeSoulData = PlayerData.Instance._LifeSoulDepot.GetLifeSoulDataByUID(lifeSoulData.SoulPOD.uid);
        if (lifeSoulData == null)
        {
            return;
        }
        UpdateViewInfo(lifeSoulData);
        if (level < lifeSoulData.SoulPOD.level)
        {
            IntensifyEffect();
        }
    }

    public override void Uninitialize()
    {
        selectMaterialsList.Clear();
        Main.Instance.StopCoroutine(UpdateMaterials());
        currentLifeSoulExp = 0;
        upgradelevel = 0;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        if (lifeSpiritAtt_dic != null)
            lifeSpiritAtt_dic.Clear();
        if (lifeSpiritMaterial_dic != null)
            lifeSpiritMaterial_dic.Clear();
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_IntensifyButton.gameObject).onClick = ButtonEvent_IntensifyButton;
        UIEventListener.Get(view.Btn_FastSelectButton.gameObject).onClick = ButtonEvent_FastSelectButton;
        UIEventListener.Get(view.Spt_MaskSprite.gameObject).onClick = ButtonEvent_CloseView;
    }


}
