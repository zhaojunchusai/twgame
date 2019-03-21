using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class SoldierEquipIntensifyViewController : UIBase 
{
    public SoldierEquipIntensifyView view;
    private List<GameObject> starList;
    private List<SEIAttComponent> before_dic;
    private List<SEIAttComponent> intensify_dic;

    private Weapon weaponPOD;

    private System.Action callBack;

    private List<int> OldHP;
    private List<int> OldATT;
    private List<int> OldACC;
    private List<int> OldDod ;
    private List<int> OldCrt ;
    private List<int> OldTen ;
    private List<int> OldSpe ;
    public GameObject Go_ItemEffect;

    public override void Initialize()
    {
        if (view == null) 
        {
            view = new SoldierEquipIntensifyView();
            view.Initialize();
            BtnEventBinding();
        }
        if (starList == null)
            starList = new List<GameObject>();
        if (before_dic == null)
            before_dic = new List<SEIAttComponent>();
        if (intensify_dic == null)
            intensify_dic = new List<SEIAttComponent>();
        if (OldHP == null)
            OldHP = new List<int>();
        if (OldATT == null)
            OldATT = new List<int>();
        if (OldACC == null)
            OldACC = new List<int>();
        if (OldDod == null)
            OldDod = new List<int>();
        if (OldCrt == null)
            OldCrt = new List<int>();
        if (OldTen == null)
            OldTen = new List<int>();
        if (OldSpe == null)
            OldSpe = new List<int>();
        view.Spt_ArtifactStar.gameObject.SetActive(false);
        view.Gobj_BeforeAttGroup.gameObject.SetActive(false);
        view.Gobj_IntensifiedAttGroup.gameObject.SetActive(false);
       
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent += OnSlodierEquipIntensify;
        PlayerData.Instance.UpdatePlayerGoldEvent += OnUpdatePlayerCoin;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierEquipStrengthenDetail);
        this.Button_Init();
    
    }
    void Button_Init()
    {
        if (CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip))
        {
            if (this.view.Btn_SoldierEquipIntensify && this.view.Btn_QuickSoldierEquipIntensify)
            {
                if (this.view.Spt_BtnSoldierEquipIntensifyBG != null && this.view.Lbl_BtnSoldierEquipIntensifyLabel != null)
                {
                    CommonFunction.UpdateWidgetGray(this.view.Spt_BtnSoldierEquipIntensifyBG, false);
                    CommonFunction.SetUILabelColor(this.view.Lbl_BtnSoldierEquipIntensifyLabel, true);
                }
                if (this.view.Lbl_BtnQuickSoldierEquipIntensifyLabel != null && this.view.Spt_BtnQuickSoldierEquipIntensifyBG != null)
                {
                    CommonFunction.UpdateWidgetGray(this.view.Spt_BtnQuickSoldierEquipIntensifyBG, false);
                    CommonFunction.SetUILabelColor(this.view.Lbl_BtnQuickSoldierEquipIntensifyLabel, true);
                }
            }
        }
        else
        {
            if (this.view.Btn_SoldierEquipIntensify && this.view.Btn_QuickSoldierEquipIntensify)
            {
                if (this.view.Spt_BtnSoldierEquipIntensifyBG != null && this.view.Lbl_BtnSoldierEquipIntensifyLabel != null)
                {
                    CommonFunction.UpdateWidgetGray(this.view.Spt_BtnSoldierEquipIntensifyBG, true);
                    CommonFunction.SetUILabelColor(this.view.Lbl_BtnSoldierEquipIntensifyLabel, false);
                }
                if (this.view.Lbl_BtnQuickSoldierEquipIntensifyLabel != null && this.view.Spt_BtnQuickSoldierEquipIntensifyBG != null)
                {
                    CommonFunction.UpdateWidgetGray(this.view.Spt_BtnQuickSoldierEquipIntensifyBG, true);
                    CommonFunction.SetUILabelColor(this.view.Lbl_BtnQuickSoldierEquipIntensifyLabel, false);
                }
            }
        }
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(SoldierEquipIntensifyView.UIName);
    }
    private void OnSlodierEquipIntensify(WeaponChange change, int Slot , System.UInt64 uID) 
    {
        
        //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.BACKPACK_SOLDIEREQUIPINTENSIFY);
        if(uID != 0 && this.view._uiRoot.activeSelf)
        {
            Weapon tmpWp = PlayerData.Instance._SoldierEquip.FindByUid(uID);
            if (tmpWp == null)
                return;

            UpdateViewInfo(tmpWp);
            //InitOldAtt();
           // Main.Instance.StartCoroutine(CreateIntensifyAtt(0.35F));
        }
    }

    public void OnUpdatePlayerCoin() 
    {
        UpdateEquipedInfo();
    }

    public void UpdateViewInfo(Weapon weapon,System.Action action) 
    {
        weaponPOD = weapon;
        callBack = action;
        UpdateViewInfo(weapon);
        InitOldAtt();
    }

    private void UpdateViewInfo(Weapon weapon)
    {
        if (weaponPOD == null)
        {
            return;
        }
        weaponPOD = weapon;
        UpdateEquipedInfo();
    }

    private void UpdateEquipedInfo() 
    {
        CommonFunction.SetQualitySprite(view.Spt_SoldierEquipIntensifyQuality, weaponPOD.Att.quality, view.Spt_SoldierEquipIntensifyBg);
        CommonFunction.SetSpriteName(view.Spt_SoldierEquipIntensifyIcon, weaponPOD.Att.icon);
        view.Lbl_ArtifactName.text = weaponPOD.Att.name;
        UpdateStar();
        if (this.view.Lbl_LevelLabelLeft)
            view.Lbl_LevelLabelLeft.text = weaponPOD.Level.ToString();
        if (this.view.Lbl_LevelLabelRight != null)
        {
            if (this.weaponPOD.isMaxLevel())
            {
                this.view.MaxLevel.SetActive(true);
                this.view.AfterLevelGroup.SetActive(false);
            }
            else
            {
                this.view.MaxLevel.SetActive(false);
                this.view.AfterLevelGroup.SetActive(true);
                this.view.Lbl_LevelLabelRight.text = (this.weaponPOD.Lv + 1).ToString();
            }
        }


        if (this.weaponPOD.InfoAttribute != null)
        {
            this.view.Grd_BeforeGroup.gameObject.SetActive(true);
            this.view.Grd_IntensifiedGroup.gameObject.SetActive(true);
            if (this.weaponPOD.isMaxLevel())
            {
                this.view.Grd_IntensifiedGroup.gameObject.SetActive(false);
                Main.Instance.StartCoroutine(UpdateAttInfo(GetBreforeAttribute(), view.Gobj_BeforeAttGroup, view.Grd_BeforeGroup, before_dic));
            }
            else
            {
                this.view.Grd_IntensifiedGroup.gameObject.SetActive(true);
                Main.Instance.StartCoroutine(UpdateAttInfo(GetBreforeAttribute(), view.Gobj_BeforeAttGroup, view.Grd_BeforeGroup, before_dic));
                Main.Instance.StartCoroutine(UpdateAttInfo(GetIntensifyAttribute(), view.Gobj_IntensifiedAttGroup, view.Grd_IntensifiedGroup, intensify_dic));
            }
        }
        else
        {
            this.view.Grd_BeforeGroup.gameObject.SetActive(true);
            this.view.Grd_IntensifiedGroup.gameObject.SetActive(true);
        }


        int lv = weaponPOD.Level - weaponPOD.Att.baseLevel;
        if (!weaponPOD.Att.MaterialBagList.ContainsKey(lv + 1)) return;
        KeyValuePair<KeyValuePair<MaterialBag.MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<fogs.proto.msg.Item, int>>> result;
        result = MaterialBag.getResult(weaponPOD.Att.MaterialBagList[lv + 1]);
        if (result.Key.Key == MaterialBag.MaterialResult.noId) return;
        if (this.weaponPOD.isMaxLevel())
        {
            view.Lbl_CountLabel.text = string.Empty;
        }
        else 
        {
            view.Lbl_CountLabel.text = CommonFunction.GetTenThousandUnit(result.Key.Value.Cost);
        }
      
    }

    private System.Collections.IEnumerator UpdateAttInfo(List<KeyValuePair<string, string>> valueList, GameObject instanceGO, UIGrid parent, List<SEIAttComponent> compList)
    {
        if (valueList.Count <= compList.Count)
        {
            for (int i = 0; i < compList.Count; i++)
            {
                SEIAttComponent comp = compList[i];
                if (i < valueList.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(valueList[i].Key, valueList[i].Value);
                }
                else 
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else 
        {
            int index = compList.Count;
            for (int i = 0; i < valueList.Count; i++)
            {
                SEIAttComponent comp = null;
                if (i < index)
                {
                    comp = compList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(instanceGO, parent.transform);
                    go.name = "att_" + i;
                    comp = new SEIAttComponent(go);
                    compList.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.SetActive(true);
                comp.UpdateInfo(valueList[i].Key, valueList[i].Value);
            }
        }
       
        yield return null;
        parent.Reposition();
    }

    private List<KeyValuePair<string, string>> GetIntensifyAttribute()
    {
        ShowInfoWeapon temp = new ShowInfoWeapon();
     
        temp.CopyTo(weaponPOD.InfoAttribute);
        temp.HP += weaponPOD.Att.u_hp;
        temp.Attack += weaponPOD.Att.u_attack;
        temp.Accuracy += weaponPOD.Att.u_accuracy;
        temp.Dodge += weaponPOD.Att.u_dodge;
        temp.Crit += weaponPOD.Att.u_crit;
        temp.Tenacity += weaponPOD.Att.u_tenacity;
        temp.MoveSpeed += weaponPOD.Att.u_speed;
        string color = "[3abd22]";
        
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (temp.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, GetUpString(color + ConstString.hp_max, temp.HP));
            attribute_dic.Add(tmp);
        }
        if (temp.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, GetUpString(color + ConstString.phy_atk, temp.Attack));
            attribute_dic.Add(tmp);
        }
        if (temp.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, GetUpString(color + ConstString.acc_rate, temp.Accuracy));
            attribute_dic.Add(tmp);
        }
        if (temp.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, GetUpString(color + ConstString.ddg_rate, temp.Dodge));
            attribute_dic.Add(tmp);
        }
        if (temp.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, GetUpString(color + ConstString.crt_rate, temp.Crit));
            attribute_dic.Add(tmp);
        }
        if (temp.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, GetUpString(color + ConstString.tnc_rate, temp.Tenacity));
            attribute_dic.Add(tmp);
        }
        if (temp.MoveSpeed != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, GetUpString(color + ConstString.speed, temp.MoveSpeed));
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }
    private List<KeyValuePair<string, string>> GetBreforeAttribute()
    {
        string color = "[c4ad87]";
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (weaponPOD.InfoAttribute.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, GetUpString(color + ConstString.hp_max,weaponPOD.InfoAttribute.HP));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, GetUpString(color + ConstString.phy_atk, weaponPOD.InfoAttribute.Attack));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, GetUpString(color + ConstString.acc_rate, weaponPOD.InfoAttribute.Accuracy));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, GetUpString(color + ConstString.ddg_rate, weaponPOD.InfoAttribute.Dodge));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, GetUpString(color + ConstString.crt_rate, weaponPOD.InfoAttribute.Crit));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, GetUpString(color + ConstString.tnc_rate, weaponPOD.InfoAttribute.Tenacity));
            attribute_dic.Add(tmp);
        }
        if (weaponPOD.InfoAttribute.MoveSpeed != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, GetUpString(color + ConstString.speed, weaponPOD.InfoAttribute.MoveSpeed));
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }
    private string GetUpString(string str, int att)
    {
        string upStr = string.Empty;
        str += ":" + att.ToString() + "[-]";
        upStr = str;
        return upStr;
    }

    private void UpdateStar() 
    {
        if (weaponPOD.Att.star <= starList.Count)
        {
            for (int i = 0; i < starList.Count; i++)
            {
                if (i < weaponPOD.Att.star)
                {
                    starList[i].SetActive(true);
                }
                else
                {
                    starList[i].SetActive(false);
                }
            }
        }
        else 
        {
            int dataIndex = starList.Count;
            for (int i = 0; i < starList.Count; i++)
            {
                    starList[i].SetActive(true);
               
            }
            for (int i = dataIndex; i < weaponPOD.Att.star; i++) 
            {
                GameObject go = CommonFunction.InstantiateObject(view.Spt_ArtifactStar.gameObject, view.Grd_StarLevel.transform);
                go.name = "star_" + i.ToString();
                go.SetActive(true);
                starList.Add(go);
            }
        }
        view.Grd_StarLevel.Reposition();
    }

    public void ButtonEvent_QuickSoldierEquipIntensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip,true))
        {
            return;
        }
        PlayerData.Instance._SoldierEquip.OneKeyStrong(weaponPOD.uId);
        //CloseOldAtt();
    }
    public void ButtonEvent_SoldierEquipIntensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip,true))
        {
            return;
        }
        PlayerData.Instance._SoldierEquip.Strong(weaponPOD.Slot);
    }

    public void ButtonEvent_CloseArtifact(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close , view._uiRoot.transform.parent.transform));
        if (callBack != null)
            callBack();
        CloseOldAtt();
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseSoldierEquipStrengthen);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPINTENSIFY);
    }

    public override void Uninitialize()
    {
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent -= OnSlodierEquipIntensify;
        PlayerData.Instance.UpdatePlayerGoldEvent -= OnUpdatePlayerCoin;
    }

    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SoldierEquipIntensify.gameObject).onClick = ButtonEvent_SoldierEquipIntensify;
        UIEventListener.Get(view.Btn_CloseArtifact.gameObject).onClick = ButtonEvent_CloseArtifact;
        UIEventListener.Get(view.Btn_QuickSoldierEquipIntensify.gameObject).onClick = ButtonEvent_QuickSoldierEquipIntensify;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        starList.Clear() ;
        before_dic.Clear() ;
        intensify_dic.Clear();
    }
    SetParticleSortingLayer Par;
    //============================================================================//
    public void PlaySoldierEquipEffect()
    {
        InitOldAtt();
        if(Go_ItemEffect==null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_SOLDIEREQUIP, (GameObject gb) => { Go_ItemEffect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_ItemEffect, view.Spt_SoldierEquipIntensifyQuality.transform);
        Par = go.GetComponent<SetParticleSortingLayer>();
        Main.Instance.StartCoroutine(CreateIntensifyAtt(0.15F));
    }

    public  void InitOldAtt()
    {
        OldHP .Add (weaponPOD.InfoAttribute.HP);
        OldATT .Add( weaponPOD.InfoAttribute.Attack);
        OldACC .Add(weaponPOD.InfoAttribute.Accuracy);
        OldDod .Add( weaponPOD.InfoAttribute.Dodge);
        OldCrt .Add (weaponPOD.InfoAttribute.Crit);
        OldTen .Add ( weaponPOD.InfoAttribute.Tenacity);
        OldSpe .Add( weaponPOD.InfoAttribute.MoveSpeed);
    }
    private void CloseOldAtt()
    {
        OldHP.Clear();
        OldATT.Clear();
        OldACC.Clear();
        OldDod.Clear();
        OldCrt.Clear();
        OldTen.Clear();
        OldSpe.Clear();

    }
    private IEnumerator CreateIntensifyAtt(float time)
    {
        ShowInfoWeapon temp = new ShowInfoWeapon();
        temp.CopyTo(weaponPOD.InfoAttribute);
        CommonFunction.ResetParticlePanelOrder(view .Item_IntensifyEffect, view._uiRoot.gameObject, Par);

        if (OldHP != null && OldHP.Count > 0) 
        {
            if ((temp.HP - OldHP[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.hp_max + "+" + (temp.HP - OldHP[0]));
                OldHP.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }       
        if (OldATT != null && OldATT.Count > 0) 
        {
            if ((temp.Attack - OldATT[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.phy_atk + "+" + (weaponPOD.InfoAttribute.Attack - OldATT[0]));
                OldATT.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }
        if (OldACC != null && OldACC.Count > 0)
        {
            if ((temp.Accuracy - OldACC[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.acc_rate + "+" + (weaponPOD.InfoAttribute.Accuracy - OldACC[0]));
                OldACC.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }
        if (OldDod != null && OldDod.Count > 0)
        {
            if ((temp.Dodge - OldDod[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.ddg_rate + "+" + (weaponPOD.InfoAttribute.Dodge - OldDod[0]));
                OldDod.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }
        if (OldDod != null && OldDod.Count > 0)
        {
            if ((temp.Crit - OldDod[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.crt_rate + "+" + (weaponPOD.InfoAttribute.Crit - OldCrt[0]));
                OldCrt.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }
        if (OldTen != null && OldTen.Count > 0) 
        {
            if ((temp.Tenacity - OldTen[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.tnc_rate + "+" + (weaponPOD.InfoAttribute.Tenacity - OldTen[0]));
                OldTen.RemoveAt(0);
                LabelObj.SetActive(true);
            }
        }
        if (OldSpe != null && OldSpe.Count > 0) 
        {
            if ((temp.MoveSpeed - OldSpe[0]) > 0)
            {
                GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_SoldierEquipIntensifyQuality.transform);

                IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
                LabelItem.UpdateItem(ConstString.speed + "+" + (weaponPOD.InfoAttribute.MoveSpeed - OldSpe[0]));
                OldSpe.RemoveAt(0);
                LabelObj.SetActive(true);
            }
            yield return new WaitForSeconds(time);
        }
    }
}
