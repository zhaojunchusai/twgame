using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

public class ArtifactIntensifyViewController : UIBase
{
    public ArtifactIntensifyView view;

    private Weapon weaponPOD;

    private List<ArtifactIntensifyAttComponent> intensify_dic;
    private List<ArtifactIntensifyMaterialComponent> material_dic;
    //private List<string> AttName ;
    //private List<string> AttDesc ;
    public KeyValuePair<KeyValuePair<MaterialBag.MaterialResult, MaterialsBagAttributeInfo>, List<KeyValuePair<fogs.proto.msg.Item, int>>> result;

    public class IntensifyMaterialBag
    {
        public fogs.proto.msg.Item item;
        public int needNum;
    }

    private List<IntensifyMaterialBag> materialList;

    private System.Action callback;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new ArtifactIntensifyView();
            view.Initialize();
            BtnEventBinding();
        }

        if (material_dic == null)
            material_dic = new List<ArtifactIntensifyMaterialComponent>();
        if (intensify_dic == null)
            intensify_dic = new List<ArtifactIntensifyAttComponent>();
        view.Gobj_IntensifyMaterialComp.SetActive(false);
        view.Gobj_IntensifiedAttGroup.SetActive(false);
        PlayerData.Instance._WeaponDepot.WeaponDepotEvent += WeaponDepotDelet;
        PlayerData.Instance.UpdatePlayerItemsEvent += UpdateMaterailData;
        PlayerData.Instance.UpdatePlayerGoldEvent += UpdateMaterailData;

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
        //PlayOpenArtifactIntensifyAnim();
        Clearlabel();
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(HeroAttributeView.UIName);
        UISystem.Instance.RefreshUIToTop(ArtifactIntensifyView.UIName);
    }

    public void WeaponDepotDelet(WeaponChange change, int Slot, UInt64 uID)
    {
        if (uID != 0)
        {
            //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Ok, ConstString.BACKPACK_SOLDIEREQUIPINTENSIFY);
            UpdateViewInfo(PlayerData.Instance._WeaponDepot.FindByUid(uID), callback);
        }
    }

    private bool _btncd = false;
    public void ButtonEvent_ArtifactIntensify(GameObject btn)
    {
        if (_btncd)
            return;
        _btncd = true;
        Assets.Script.Common.Scheduler.Instance.AddTimer(0.2f, false, () =>
        {
            _btncd = false;
        });
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_LevelUp_Equip, view._uiRoot.transform.parent.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify,true))
        {
            return;
        }
        view.QuickMatItem.ShowTip(ButtonEvent_ArtifactIntensify_continue);
    }

    /// <summary>
    /// 由于插入了补全材料的确认面板
    /// 将原函数 ButtonEvent_ArtifactIntensify 中的一部分单独成这个函数
    /// </summary>
    private void ButtonEvent_ArtifactIntensify_continue()
    {
        switch (result.Key.Key)
        {
            case MaterialBag.MaterialResult.OK:
                {
                    PlayerData.Instance._WeaponDepot.Strong(weaponPOD.Slot);
                }
                break;
            case MaterialBag.MaterialResult.coin:
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                    UISystem.Instance.BuyCoinView.UpdateBuyCoin();
                }
                break;
            case MaterialBag.MaterialResult.diamond:
                {
                }
                break;
            case MaterialBag.MaterialResult.noId:
                {
                    UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NOT_FIND_MATERIAL);
                }
                break;
            case MaterialBag.MaterialResult.material:
                {
                    //UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_CHOOSE_MATERIAL);
                    PlayerData.Instance._WeaponDepot.Strong(weaponPOD.Slot);
                }
                break;
        }
    }

    public void ButtonEvent_CloseArtifactIntensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_ARTIFACTINTENSIFY);
        if (callback != null)
            callback();

        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGodWeaponStrengthen);
    }

    public void UpdateViewInfo(Weapon weapon, System.Action action)
    {
        if (weapon == null)
        {
            return;
        }
        callback = action;
        weaponPOD = weapon;

        if (this.weaponPOD._Skill != null)
        {
            if (this.weaponPOD._Skill.enableStrong() != SkillCheck.MaxLevel)
                this.view.Lbl_Label_skillBefor.text = string.Format(ConstString.ARTFACTI_SKILL_STRENGTH, this.weaponPOD._Skill.GetDescript(this.weaponPOD._Skill.Level));
            else
                this.view.Lbl_Label_skillBefor.text = this.weaponPOD._Skill.GetDescript(this.weaponPOD._Skill.Level);
        }
        if (this.weaponPOD.InfoAttribute != null)
        {
            //AttName.Clear();
            //AttDesc.Clear();
            Main.Instance.StartCoroutine(UpdateAttInfo(GetIntensifyAttribute(), view.Gobj_IntensifiedAttGroup, view.Grd_IntensifiedGroup, intensify_dic));
            //
        }
        UISprite Button_Back = this.view.Btn_ArtifactIntensify.transform.FindChild("Background").GetComponent<UISprite>();
        UILabel Button_Label = this.view.Btn_ArtifactIntensify.transform.FindChild("SpriteLabel").GetComponent<UILabel>();
        if (Button_Back != null && Button_Label)
        {
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
            {
                CommonFunction.UpdateWidgetGray(Button_Back, true);
                CommonFunction.SetUILabelColor(Button_Label, false);
            }
            else
            {
                CommonFunction.UpdateWidgetGray(Button_Back, false);
                CommonFunction.SetUILabelColor(Button_Label, true);
            }
        }

        if (this.view.Lbl_Label_Lv_Befor != null && this.view.Lbl_Label_Lv_After != null)
        {
            this.view.Lbl_Label_Lv_Befor.text = string.Format("Lv.{0}",this.weaponPOD.Level);
            if (this.weaponPOD.enableStrong() != WeaponCheck.MaxLevel)
            {
                this.view.MaxLevel.SetActive(false);
                this.view.AfterGroup.SetActive(true);
                this.view.Lbl_Label_Lv_After.text = string.Format("Lv.{0}", this.weaponPOD.Level + 1);
            }
            else
            {
                this.view.MaxLevel.SetActive(true);
                this.view.AfterGroup.SetActive(false);
            }
        }
        UpdateMaterailData();
        view.Lbl_IntensifyArtifactName.text = weapon.Att.name;
        view.Lbl_ArtifactIntensifyConsume.text = result.Key.Value.Cost.ToString();
        CommonFunction.SetQualitySprite(view.Spt_ArtifactIntensifyQuality, weapon.Att.quality, view.Spt_ArtifactIntensifyItemBg);
        CommonFunction.SetSpriteName(view.Tex_ArtifactIntensifyIcon, weapon.Att.icon);
    }
   
       
    private void UpdateMaterailData()
    {
        materialList = new List<IntensifyMaterialBag>();
        int lv = weaponPOD.Level - weaponPOD.Att.baseLevel + 1;
        if (!weaponPOD.Att.MaterialBagList.ContainsKey(lv)) return;
        
        result = MaterialBag.getResult(weaponPOD.Att.MaterialBagList[lv]);
        foreach (KeyValuePair<fogs.proto.msg.Item, int> tmp in result.Value)
        {
            IntensifyMaterialBag data = new IntensifyMaterialBag();
            data.item = tmp.Key;
            data.needNum = tmp.Value;
            materialList.Add(data);
        }
         Main.Instance.StartCoroutine(UpdateMaterialBag());
    }

    private System.Collections.IEnumerator UpdateAttInfo(List<KeyValuePair<string, string>> valueList, GameObject instanceGO, UIGrid parent, List<ArtifactIntensifyAttComponent> compList)
    {
        if (valueList.Count <= compList.Count)
        {
            for (int i = 0; i < compList.Count; i++)
            {
                ArtifactIntensifyAttComponent comp = compList[i];
                if (i < valueList.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(valueList[i].Key, valueList[i].Value);
                    //
                    //AttName.Add(valueList[i].Key);
                    //AttDesc.Add(valueList[i].Value);

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
                ArtifactIntensifyAttComponent comp = null;
                if (i < index)
                {
                    comp = compList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(instanceGO, parent.transform);
                    go.name = "att_" + i;
                    comp = new ArtifactIntensifyAttComponent(go);
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
        //temp.HP += weaponPOD.Att.u_hp;
        //temp.Attack += weaponPOD.Att.u_attack;
        //temp.Accuracy += weaponPOD.Att.u_accuracy;
        //temp.Dodge += weaponPOD.Att.u_dodge;
        //temp.Crit += weaponPOD.Att.u_crit;
        //temp.Tenacity += weaponPOD.Att.u_tenacity;
        //temp.MoveSpeed += weaponPOD.Att.u_speed;
        string color = "[c4ad87]"; //4FBE3C
        List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
        if (temp.HP != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, GetUpString(color, temp.HP,weaponPOD.Att.u_hp));
            attribute_dic.Add(tmp);
        }
        if (temp.Attack != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, GetUpString(color, temp.Attack,weaponPOD.Att.u_attack));
            attribute_dic.Add(tmp);
        }
        if (temp.Accuracy != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, GetUpString(color, temp.Accuracy,weaponPOD.Att.u_accuracy));
            attribute_dic.Add(tmp);
        }
        if (temp.Dodge != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, GetUpString(color , temp.Dodge,weaponPOD.Att.u_dodge));
            attribute_dic.Add(tmp);
        }
        if (temp.Crit != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, GetUpString(color, temp.Crit,weaponPOD.Att.u_crit));
            attribute_dic.Add(tmp);
        }
        if (temp.Tenacity != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, GetUpString(color, temp.Tenacity,weaponPOD.Att.u_tenacity));
            attribute_dic.Add(tmp);
        }
        if (temp.MoveSpeed != 0)
        {
            KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, GetUpString(color, temp.MoveSpeed,weaponPOD.Att.u_speed));
            attribute_dic.Add(tmp);
        }
        return attribute_dic;
    }
    //private List<KeyValuePair<string, string>> GetBreforeAttribute()
    //{
    //    string color = "[543722]";
    //    List<KeyValuePair<string, string>> attribute_dic = new List<KeyValuePair<string, string>>();
    //    if (weaponPOD.InfoAttribute.HP != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_HP, GetUpString(color + ConstString.hp_max, weaponPOD.InfoAttribute.HP));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.Attack != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Attack, GetUpString(color + ConstString.phy_atk, weaponPOD.InfoAttribute.Attack));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.Accuracy != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_AccRate, GetUpString(color + ConstString.acc_rate, weaponPOD.InfoAttribute.Accuracy));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.Dodge != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_DdgRate, GetUpString(color + ConstString.ddg_rate, weaponPOD.InfoAttribute.Dodge));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.Crit != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Crit, GetUpString(color + ConstString.crt_rate, weaponPOD.InfoAttribute.Crit));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.Tenacity != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Tenacity, GetUpString(color + ConstString.tnc_rate, weaponPOD.InfoAttribute.Tenacity));
    //        attribute_dic.Add(tmp);
    //    }
    //    if (weaponPOD.InfoAttribute.MoveSpeed != 0)
    //    {
    //        KeyValuePair<string, string> tmp = new KeyValuePair<string, string>(GlobalConst.SpriteName.Attribute_Speed, GetUpString(color + ConstString.speed, weaponPOD.InfoAttribute.MoveSpeed));
    //        attribute_dic.Add(tmp);
    //    }
    //    return attribute_dic;
    //}
    private string GetUpString(string str, int att,int upAtt)
    {
        string upStr = string.Empty;
        str += att;
        if (upAtt != 0)
        {
          str +=  "[FF0000]+" + upAtt + "[-]";
        } 
        upStr = str;
        return upStr;
    }

    private IEnumerator UpdateMaterialBag()
    {
        yield return null;
        view.QuickMatItem.Init(materialList);
        if (materialList.Count <= material_dic.Count)
        {
            for (int i = 0; i < material_dic.Count; i++) 
            {
                ArtifactIntensifyMaterialComponent comp = material_dic[i];
                if (i < materialList.Count) 
                {
                    IntensifyMaterialBag data = materialList[i];
                    if (i < materialList.Count)
                    {
                        comp.UpdateInfo(data.item, data.needNum);
                        comp.mRootObject.SetActive(true);
                    }
                    else
                    {
                        comp.mRootObject.SetActive(false);
                    }
                }
            }
        }
        else
        {
            int go_index = material_dic.Count;
            for (int i = 0; i < materialList.Count; i++)
            {
                IntensifyMaterialBag data = materialList[i];
                ArtifactIntensifyMaterialComponent comp = null;
                if (i < go_index)
                {
                    comp = material_dic[i];
                }
                else 
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_IntensifyMaterialComp, view.Grd_IntensifyMaterial.transform);
                    go.name = "item_" + i;
                    comp = new ArtifactIntensifyMaterialComponent(go);
                    material_dic.Add(comp);
                }
                if (comp == null) continue;
                comp.UpdateInfo(data.item, data.needNum);
                comp.mRootObject.SetActive(true);
            }
        }
        yield return null;
        view.Grd_IntensifyMaterial.repositionNow = true;
    }


    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_ArtifactIntensify.gameObject).onClick = ButtonEvent_ArtifactIntensify;
        UIEventListener.Get(view.Btn_CloseArtifactIntensify.gameObject).onClick = ButtonEvent_CloseArtifactIntensify;
    }


    //界面动画;
    //public void PlayOpenArtifactIntensifyAnim()
    //{
    //    view.ArtifactIntensify_TScale.gameObject.transform.localScale = Vector3.one *GlobalConst.ViewScaleAnim ;
    //    view.ArtifactIntensify_TScale.Restart();
    //    view.ArtifactIntensify_TScale.PlayForward();
    //}


    public override void Uninitialize()
    {
        PlayerData.Instance._WeaponDepot.WeaponDepotEvent -= WeaponDepotDelet;
        PlayerData.Instance.UpdatePlayerItemsEvent -= UpdateMaterailData;
        PlayerData.Instance.UpdatePlayerGoldEvent -= UpdateMaterailData;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        intensify_dic.Clear();
        material_dic.Clear();
    }
    //==========================================================================//.
    SetParticleSortingLayer Par_1,Par_2;

    public void ShowArtifactEffect()
    {
        view.SptA_ArtifactIntensifyQualityEffect.ResetToBeginning();
        view.SptA_ArtifactIntensifyQualityEffect.gameObject.SetActive(true);
        view.SptA_ArtifactIntensifyQualityEffect.Play();
        Assets.Script.Common.Scheduler.Instance.RemoveTimer(CloseArtifactEffect);
        Scheduler.Instance.AddTimer(0.5f, false, CloseArtifactEffect);
    }
    public void CloseArtifactEffect()
    {
        view.SptA_ArtifactIntensifyQualityEffect.gameObject.SetActive(false);
    }

    public void IntensifyEffect()
    {
        Par_1 = view.Go_FlyEffect.GetComponent<SetParticleSortingLayer>();
        Par_2 =view .Go_ItemEffect.GetComponent<SetParticleSortingLayer>();

        CommonFunction.ResetParticlePanelOrder(view.Item_IntensifyEffect, view._uiRoot.gameObject, Par_1);
        //CommonFunction.ResetParticlePanelOrder(view.Item_IntensifyEffect, view._uiRoot.gameObject, Par_2);
        
        var itemList = view.Grd_IntensifyMaterial.GetChildList();
        for (int i=0;i<itemList .Count;i++)
        {
            Main.Instance.StartCoroutine(FlayEffect(i * 0.3F, itemList[i].transform, view.Spt_ArtifactIntensifyQuality.gameObject, 1F));
        }
        Main.Instance.StartCoroutine(ShowIntensifyEffect(0.15F, view.Spt_ArtifactIntensifyQuality.gameObject));
    }
    private IEnumerator FlayEffect(float time, Transform From, GameObject To, float FlyTimes)
    {
        yield return new WaitForSeconds(time);
        //GameObject ItemGo = ShowEffectManager.Instance.ShowEffect(Go_ItemEffect, From);//物品分解特效
        GameObject go = ShowEffectManager.Instance.ShowEffect(view.Go_FlyEffect, From);
        iTween.MoveTo(go, To.transform.position, FlyTimes);
    }
    private IEnumerator ShowIntensifyEffect(float time, GameObject From)
    {

        //weaponPOD.InfoAttribute

        yield return new WaitForSeconds(time);

        //GameObject ItemGo = ShowEffectManager.Instance.ShowEffect(view.Go_ItemEffect, From.transform);
        ShowArtifactEffect();

        //for (int i = 0; i < AttName.Count; i++)
        //{
        //    string name = "";
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_HP)
        //    {
        //        name = ConstString.hp_max;
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_Attack)
        //    {
        //        name = ConstString.phy_atk; 
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_AccRate)
        //    {
        //        name = ConstString.acc_rate;
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_DdgRate)
        //    {
        //        name = ConstString.ddg_rate;
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_Crit )
        //    {
        //        name = ConstString.crt_rate ;
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_Tenacity )
        //    {
        //        name = ConstString.tnc_rate ;
        //    }
        //    if (AttName[i] == GlobalConst.SpriteName.Attribute_Speed)
        //    {
        //        name = ConstString.speed ;
        //    }
        //    GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
        //    IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
        //    LabelItem.UpdateItem(name+AttDesc[i]);
        //    LabelObj.SetActive(true);
        //}
        //yield return new WaitForSeconds(0.5F);
        Main.Instance.StartCoroutine(CreateIntensifyAttribute(0.1F));
        //CreateIntensifyAttribute();
    }
    List<GameObject> LabelList = new List<GameObject>();
    public void Clearlabel()//打开界面时清除残余属性Label
    {
        if (LabelList == null) return;
        for (int i=0;i<LabelList .Count;i++)
        {
                GameObject.Destroy(LabelList[i]);
        }
        LabelList.Clear();
    }
    private IEnumerator CreateIntensifyAttribute(float time)
    {
        ShowInfoWeapon temp = new ShowInfoWeapon();
        temp.CopyTo(weaponPOD.InfoAttribute);
        yield return new WaitForSeconds(time);
        if (temp.HP > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);
           
            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.hp_max + "+" + weaponPOD .Att .u_hp);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Attack > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.phy_atk + "+" + weaponPOD .Att .u_attack);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Accuracy > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.acc_rate + "+" + weaponPOD.Att.u_accuracy);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Dodge > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.ddg_rate + "+" + weaponPOD.Att.u_dodge);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Crit > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.crt_rate + "+" + weaponPOD.Att.u_crit);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.Tenacity > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.tnc_rate + "+" + weaponPOD.Att.u_tenacity);
            LabelObj.SetActive(true);
        }
        yield return new WaitForSeconds(time);
        if (temp.MoveSpeed > 0)
        {
            GameObject LabelObj = CommonFunction.InstantiateObject(view.Item_IntensifyEffect, view.Spt_ArtifactIntensifyQuality.transform);
            LabelList.Add(LabelObj);

            IntensifyLabelItem LabelItem = LabelObj.AddComponent<IntensifyLabelItem>();
            LabelItem.UpdateItem(ConstString.speed + "+" + weaponPOD.Att.u_speed);
            LabelObj.SetActive(true);
        }



    }
}
