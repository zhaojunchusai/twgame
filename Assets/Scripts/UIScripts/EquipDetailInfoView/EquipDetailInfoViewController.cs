using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class EquipDetailInfoViewController : UIBase
{
    public EquipDetailInfoView view;

    private System.Action callBack;
    private enum FuncTypeEnum
    {
        ArtifactInHero = 1,
        ArtifactInBackPack = 3,
        SoldierEquip = 2,
    }

    private List<SoldierEquipDetailAttComponent> normalSoldierEquipAttributeList;
    private List<SoldierEquipDetailAttComponent> suitSoldierEquipAttributeList;
    private List<SoldierSuitEquipAttComponent> suitEquipAttDescList;
    private List<GameObject> soldierEquipStarList;

    private Weapon weaponPOD;
    private FuncTypeEnum funcType;
    private Soldier soldierPOD;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new EquipDetailInfoView();
            view.Initialize();
        }
        view.Gobj_SuitAttComp.SetActive(false);
        view.Gobj_NormalEquipAttComp.SetActive(false);
        if (soldierEquipStarList == null)
        {
            soldierEquipStarList = new List<GameObject>();
        }
        if (normalSoldierEquipAttributeList == null)
            normalSoldierEquipAttributeList = new List<SoldierEquipDetailAttComponent>();
        if (suitSoldierEquipAttributeList == null)
            suitSoldierEquipAttributeList = new List<SoldierEquipDetailAttComponent>();
        if (suitEquipAttDescList == null)
            suitEquipAttDescList = new List<SoldierSuitEquipAttComponent>();
        PlayerData.Instance._WeaponDepot.ErrotDeleteEvent += _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent += _SoldierEquip_ErrotDeleteEvent;
        BtnEventBinding();
    }
    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
    }

    public void UpdateViewInfo(Weapon pod, int type, Soldier sd)
    {
        if (pod == null || sd == null)
        {
            return;
        }
        this.soldierPOD = sd;
        this.UpdateViewInfo(pod, type);
    }

    #region Button Event
    private void ButtonEvent_SoldierEquipObtain(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));
        if (CommonFunction.CheckIsOpen(OpenFunctionType.GetPath, true))
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_GETPATH);
            UISystem.Instance.GetPathView.UpdateViewInfo(weaponPOD.Att.id, 2);
        }
    }

    private void ButtonEvent_SoldierEquipChange(GameObject btn)
    {
        Weapon tp = this.weaponPOD;
        var aa = PlayerData.Instance._SoldierEquip;
        if (tp == null) return;
        if (this.funcType == FuncTypeEnum.ArtifactInHero)
        {
            int index = PlayerData.Instance._ArtifactedDepot._EquiptList.FindIndex((Weapon wp) => { if (wp == null) return false; return wp.uId == tp.uId; });
            if (index != -1)
            {
                EquipChoosePanel choosePanel = EquipChoosePanel.create();
                choosePanel.SetParent(this.view._uiRoot.transform);
                choosePanel.view.InitWeaponItem(index);
                UISystem.Instance.ResortViewOrder();
            }

            return;
        }
        if (this.funcType == FuncTypeEnum.SoldierEquip && this.soldierPOD != null)
        {
            int index = this.soldierPOD._equippedDepot._EquiptList.FindIndex((Weapon wp) => { if (wp == null) return false; return wp.uId == tp.uId; });
            if (index != -1)
            {
                SoldierEquipChoosePanel choosePanel = SoldierEquipChoosePanel.create(this.soldierPOD);
                choosePanel.SetParent(this.view._uiRoot.transform);
                choosePanel.view.InitWeaponItem(index, this.soldierPOD.Att.PosList[index + 1]);
                UISystem.Instance.ResortViewOrder();
            }
            return;
        }
    }

    private void ButtonEvent_SoldierEquipUnLoad(GameObject btn)
    {
        if (this.soldierPOD == null)
            PlayerData.Instance._ArtifactedDepot.uninstallEquipt(weaponPOD.uId);
        else
            soldierPOD._equippedDepot.uninstallEquipt(weaponPOD.uId);
        //UISystem.Instance.CloseGameUI(ViewType.)
    }

    private void ButtonEvent_SoldierEquipIntensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        switch (funcType)
        {
            case FuncTypeEnum.ArtifactInHero:
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_ARTIFACTINTENSIFY);
                UISystem.Instance.ArtifactIntensifyView.UpdateViewInfo(weaponPOD, UpdateSoldierEquipDetailInfo);
                GuideManager.Instance.CheckTrigger(GuideTrigger.OpenGodWeaponStrengthen);
                break;
            case FuncTypeEnum.SoldierEquip:
            case FuncTypeEnum.ArtifactInBackPack:
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPINTENSIFY);
                UISystem.Instance.SoldierEquipIntensifyView.UpdateViewInfo(weaponPOD, UpdateSoldierEquipDetailInfo);
                break;
        }
    }

    private void ButtonEvent_SoldierEquipUpgrade(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        if (ConfigManager.Instance.mEquipData.FindById(weaponPOD.Att.evolveId) == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HAD_MAX_STAR);
            return;
        }
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEW_SOLDIEREQUIPADVANCED);
        UISystem.Instance.SoldierEquipAdvancedView.UpdateViewInfo(weaponPOD, UpdateSoldierEquipDetailInfo);
    }

    private void ButtonEvent_CloseView(GameObject btn)
    {
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
        if (callBack != null)
        {
            callBack();
        }
        if (this.funcType == FuncTypeEnum.SoldierEquip)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.CloseSoldierEquipDetail);
        }
        if (this.funcType == FuncTypeEnum.ArtifactInHero)
        {
            //装备信息界面关闭
            GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGodWeaponDetail);
        }
    }
    #endregion

    #region Update Event
    void _SoldierEquip_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        if (errorCode == 0)
        {
            if (control == EquipControl.GetoffEquipAndArtifactResp)
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
            }
            if (control == EquipControl.PutonEquipAndArtifactResp)
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
            }
        }
    }

    void _WeaponDepot_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        if (errorCode == 0)
        {
            if (control == EquipControl.GetoffEquipAndArtifactResp)
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
            }
            if (control == EquipControl.PutonEquipAndArtifactResp)
            {
                UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
            }
        }
    }



    /// <summary>
    ///  1:英雄界面神器  2:武将装备 3:背包界面神器
    /// </summary>
    /// <param name="pod"></param>
    /// <param name="type"></param>
    public void UpdateViewInfo(Weapon pod, int type, System.Action action = null)
    {
        //view.Grd_ButtomGroup.enabled = false;
        weaponPOD = pod;
        callBack = action;
        if (weaponPOD == null)
        {
            return;
        }
        funcType = (FuncTypeEnum)type;
        switch (funcType)
        {
            case FuncTypeEnum.SoldierEquip:
                {
                    view.Btn_SoldierEquipChange.gameObject.SetActive(true);
                    view.Btn_SoldierEquipUnLoad.gameObject.SetActive(true);
                    view.Btn_SoldierEquipObtain.gameObject.SetActive(false);
                    view.Btn_SoldierEquipIntensify.gameObject.SetActive(true);
                    view.Btn_SoldierEquipUpgrade.gameObject.SetActive(true);
                    //view.Grd_ButtomGroup.cellWidth = 105;
                    view.Btn_SoldierEquipChange.transform.localPosition = new Vector3(-157.5f, 0, 0);
                    view.Btn_SoldierEquipUnLoad.transform.localPosition = new Vector3(-52.5f, 0, 0);
                    view.Btn_SoldierEquipIntensify.transform.localPosition = new Vector3(52.5f, 0, 0);
                    view.Btn_SoldierEquipUpgrade.transform.localPosition = new Vector3(157.5f, 0, 0);
                }
                break;
            case FuncTypeEnum.ArtifactInHero:
                {
                    view.Btn_SoldierEquipChange.gameObject.SetActive(true);
                    view.Btn_SoldierEquipUnLoad.gameObject.SetActive(true);
                    view.Btn_SoldierEquipObtain.gameObject.SetActive(false);
                    view.Btn_SoldierEquipIntensify.gameObject.SetActive(true);
                    view.Btn_SoldierEquipUpgrade.gameObject.SetActive(false);
                    //view.Grd_ButtomGroup.cellWidth = 160;
                    view.Btn_SoldierEquipChange.transform.localPosition = new Vector3(-160, 0, 0);
                    view.Btn_SoldierEquipUnLoad.transform.localPosition = Vector3.zero;
                    view.Btn_SoldierEquipIntensify.transform.localPosition = new Vector3(160, 0, 0);
                }
                break;
            case FuncTypeEnum.ArtifactInBackPack:
                {
                    view.Btn_SoldierEquipChange.gameObject.SetActive(false);
                    view.Btn_SoldierEquipUnLoad.gameObject.SetActive(false);
                    view.Btn_SoldierEquipObtain.gameObject.SetActive(true);
                    view.Btn_SoldierEquipIntensify.gameObject.SetActive(true);
                    view.Btn_SoldierEquipUpgrade.gameObject.SetActive(true);
                    view.Btn_SoldierEquipObtain.transform.localPosition = new Vector3(-160, 0, 0);
                    view.Btn_SoldierEquipIntensify.transform.localPosition = Vector3.zero;
                    view.Btn_SoldierEquipUpgrade.transform.localPosition = new Vector3(160, 0, 0);
                    //if (weaponPOD.Att.isHadWay)
                    //{
                    view.Btn_SoldierEquipObtain.collider.enabled = true;
                    CommonFunction.UpdateWidgetGray(view.Spt_SoldierEquipObtainBg, false);
                    CommonFunction.SetUILabelColor(view.Lbl_BtnSoldierEquipObtain, true);
                    //}
                    //else
                    //{
                    //    view.Btn_SoldierEquipObtain.collider.enabled = false;
                    //    CommonFunction.UpdateWidgetGray(view.Spt_SoldierEquipObtainBg, true);
                    //    CommonFunction.SetUILabelColor(view.Lbl_BtnSoldierEquipObtain, false);
                    //}
                    //view.Grd_ButtomGroup.cellWidth = 160;
                }
                break;
            default:
                break;
        }
        //view.Grd_ButtomGroup.repositionNow = true;
        UpdateSoldierEquipDetailInfo();
    }


    private void UpdateSoldierEquipDetailInfo()
    {

        if (this.soldierPOD != null)
        {
            this.weaponPOD = PlayerData.Instance._SoldierEquip.FindByUid(this.weaponPOD.uId);
            if (this.weaponPOD == null)
                return;
        }
        else
        {
            if (weaponPOD == null)
            {
                return;
            }
            if (weaponPOD.Att.godEquip == 1)
            {
                this.weaponPOD = PlayerData.Instance._WeaponDepot.FindByUid(this.weaponPOD.uId);
            }
            else if (weaponPOD.Att.godEquip == 2)
            {
                this.weaponPOD = PlayerData.Instance._SoldierEquip.FindByUid(this.weaponPOD.uId);
            }
        }
        if (weaponPOD == null)
        {
            return;
        }
        if (weaponPOD._Skill == null)
        {
            view.Lbl_EquipSkillDescLabel.enabled = false;
            if (weaponPOD.Att.CoordID == 0)  //为0则说明不是套装
            {
                view.Gobj_SuitGroup.SetActive(false);
                view.Grd_NormalGroup.gameObject.SetActive(true);
                view.Gobj_SuitNameGroup.SetActive(false);
                UpdateNormalEquipAttribute();

            }
            else
            {
                view.Grd_NormalGroup.gameObject.SetActive(false);
                view.Gobj_SuitGroup.SetActive(true);
                EquipCoordinatesInfo info = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(weaponPOD.Att.CoordID);
                if (info == null) return;
                view.Gobj_SuitNameGroup.SetActive(true);
                view.Lbl_SuitName.text = info.name;
                UpdateSuitEquipAttribute(info);
            }
        }
        else
        {
            view.Lbl_EquipSkillDescLabel.enabled = true;
            view.Gobj_SuitGroup.SetActive(false);
            view.Grd_NormalGroup.gameObject.SetActive(false);
            view.Gobj_SuitNameGroup.SetActive(false);
            view.Lbl_EquipSkillDescLabel.text = weaponPOD._Skill.GetDescript(weaponPOD._Skill.Level);
        }
        UpdateEquipInfo();
        UpdateEquipStars();
        view.Lbl_EquipDescLabel.text = CommonFunction.ReplaceEscapeChar(weaponPOD.Att.descript);
    }

    private void UpdateNormalEquipAttribute()
    {
        if (weaponPOD == null || weaponPOD.InfoAttribute == null)
        {
            return;
        }
        List<KeyValuePair<string, string>> attribute_dic = CommonFunction.GetWeaponAttributeDesc(weaponPOD.InfoAttribute);
        if (attribute_dic == null)
        {
            attribute_dic = new List<KeyValuePair<string, string>>();
        }
        if (attribute_dic.Count <= normalSoldierEquipAttributeList.Count)
        {
            for (int i = 0; i < normalSoldierEquipAttributeList.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = normalSoldierEquipAttributeList[i];
                if (i < attribute_dic.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int count = normalSoldierEquipAttributeList.Count;
            for (int i = 0; i < attribute_dic.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = null;
                if (i < count)
                {
                    comp = normalSoldierEquipAttributeList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_EquipAttComp, view.Grd_NormalGroup.transform);
                    go.name = "att_" + i;
                    comp = new SoldierEquipDetailAttComponent();
                    comp.MyStart(go);
                    normalSoldierEquipAttributeList.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.SetActive(true);
                comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
            }
        }
        view.Grd_NormalGroup.repositionNow = true;
    }

    private void UpdateSuitEquipAttribute(EquipCoordinatesInfo info)
    {
        UpdateSuitNormalAttribute();
        Main.Instance.StartCoroutine(UpdateSuitAttribute(info));
    }

    private void UpdateSuitNormalAttribute()
    {
        if (weaponPOD == null || weaponPOD.InfoAttribute == null)
        {
            return;
        }
        List<KeyValuePair<string, string>> attribute_dic = CommonFunction.GetWeaponAttributeDesc(weaponPOD.InfoAttribute);
        if (attribute_dic == null)
        {
            attribute_dic = new List<KeyValuePair<string, string>>();
        }
        if (attribute_dic.Count <= suitSoldierEquipAttributeList.Count)
        {
            for (int i = 0; i < suitSoldierEquipAttributeList.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = suitSoldierEquipAttributeList[i];
                if (i < attribute_dic.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int count = suitSoldierEquipAttributeList.Count;
            for (int i = 0; i < attribute_dic.Count; i++)
            {
                SoldierEquipDetailAttComponent comp = null;
                if (i < count)
                {
                    comp = suitSoldierEquipAttributeList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_NormalEquipAttComp, view.Grd_NormalAttGrid.transform);
                    go.name = "att_" + i;
                    comp = new SoldierEquipDetailAttComponent();
                    comp.MyStart(go);
                    suitSoldierEquipAttributeList.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.SetActive(true);
                comp.UpdateInfo(attribute_dic[i].Key, attribute_dic[i].Value.ToString());
            }
        }
        view.Grd_NormalAttGrid.repositionNow = true;
    }

    private IEnumerator UpdateSuitAttribute(EquipCoordinatesInfo info)
    {
        if (weaponPOD == null || weaponPOD.InfoAttribute == null)
        {
            yield break;
        }
        if (info == null)
            yield break;
        List<string> list = CommonFunction.GetDescByEquipCoordinatesConfig(info);
        if (list.Count < suitEquipAttDescList.Count)
        {
            for (int i = 0; i < suitEquipAttDescList.Count; i++)
            {
                SoldierSuitEquipAttComponent comp = suitEquipAttDescList[i];
                if (i < list.Count)
                {
                    comp.mRootObject.SetActive(true);
                    comp.UpdateDesc(list[i]);
                }
                else
                {
                    comp.mRootObject.SetActive(false);
                }
            }
        }
        else
        {
            int count = suitEquipAttDescList.Count;
            for (int i = 0; i < list.Count; i++)
            {
                SoldierSuitEquipAttComponent comp = null;
                if (i < count)
                {
                    comp = suitEquipAttDescList[i];
                }
                else
                {
                    GameObject go = CommonFunction.InstantiateObject(view.Gobj_SuitAttComp, view.Grd_SuitAttGroup.transform);
                    go.name = "att_" + i;
                    comp = new SoldierSuitEquipAttComponent();
                    comp.MyStart(go);
                    suitEquipAttDescList.Add(comp);
                }
                if (comp == null) continue;
                comp.mRootObject.SetActive(true);
                comp.UpdateDesc(list[i]);
            }
        }
        yield return null;
        view.Grd_SuitAttGroup.repositionNow = true;
        yield return null;
        view.ScrView_SuitAtt.ResetPosition();
    }

    private void UpdateEquipInfo()
    {
        CommonFunction.SetQualitySprite(view.Spt_SoldierEquipInfoQuality, weaponPOD.Att.quality, view.Spt_SoldierEquipInfoBg);
        CommonFunction.SetSpriteName(view.Spt_SoldierEquipInfoIcon, weaponPOD.Att.icon);
        view.Lbl_SoldierEquipInfoName.text = weaponPOD.Att.name;
        view.Lbl_SoldierEquipLevel.text = "Lv." + weaponPOD.Level.ToString();
    }

    private void UpdateEquipStars()
    {
        if (weaponPOD.Att.star <= soldierEquipStarList.Count)
        {
            for (int i = 0; i < soldierEquipStarList.Count; i++)
            {
                if (i < weaponPOD.Att.star)
                {
                    soldierEquipStarList[i].gameObject.SetActive(true);
                }
                else
                {
                    soldierEquipStarList[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            int index = soldierEquipStarList.Count;
            for (int i = 0; i < soldierEquipStarList.Count; i++)
            {
                soldierEquipStarList[i].gameObject.SetActive(true);
            }
            for (int i = index; i < weaponPOD.Att.star; i++)
            {
                GameObject go = CommonFunction.InstantiateObject(view.Spt_SoldierEquipStar.gameObject, view.Grd_StarLevelGroup.transform);
                go.name = "star_" + i;
                go.SetActive(true);
                soldierEquipStarList.Add(go);
            }
        }
        view.Grd_StarLevelGroup.Reposition();
    }
    #endregion
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_SoldierEquipObtain.gameObject).onClick = ButtonEvent_SoldierEquipObtain;
        UIEventListener.Get(view.Btn_SoldierEquipChange.gameObject).onClick = ButtonEvent_SoldierEquipChange;
        UIEventListener.Get(view.Btn_SoldierEquipUnLoad.gameObject).onClick = ButtonEvent_SoldierEquipUnLoad;
        UIEventListener.Get(view.Btn_SoldierEquipIntensify.gameObject).onClick = ButtonEvent_SoldierEquipIntensify;
        UIEventListener.Get(view.Btn_SoldierEquipUpgrade.gameObject).onClick = ButtonEvent_SoldierEquipUpgrade;
        UIEventListener.Get(view.Spt_MaskSprite.gameObject).onClick = ButtonEvent_CloseView;
    }
    public override void Uninitialize()
    {
        weaponPOD = null;
        this.soldierPOD = null;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        normalSoldierEquipAttributeList.Clear();
        suitSoldierEquipAttributeList.Clear();
        suitEquipAttDescList.Clear();
        soldierEquipStarList.Clear();
    }

}
