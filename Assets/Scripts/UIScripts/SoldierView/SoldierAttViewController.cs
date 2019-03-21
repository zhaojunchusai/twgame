using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
public class SoldierAttViewController : UIBase
{
    public SoldierAttView view;
    public SoldierRightPanel rightPanel;
    public SoldierCenterPanel centerPanel;
    public SoldierLeftPanel leftPanel;
    public bool isSoldier = true;
    public bool isEquip = true;
    public List<GameObject> ItemPointList = new List<GameObject>();
    public List<GameObject> FilterPointList = new List<GameObject>();

    public GameObject Go_CompoundEffect;
    public GameObject Go_ItemEffect;
    public GameObject Go_ItemEffect1;
    public int SoldierStrong_Att;
    public int SoldierStorng_HP;
    public bool markValue = false;
    public GameObject Go_FilterEffect;

    public override void Initialize()
    {
        if (view == null)
        {
            view = new SoldierAttView();
            view.Initialize();
        }

        if (centerPanel == null)
        {
            centerPanel = new SoldierCenterPanel();
            centerPanel.Initialize(view._uiRoot);
        }
        centerPanel.init(view._uiRoot, null);

        if (rightPanel == null)
        {
            rightPanel = new SoldierRightPanel();
            rightPanel.Initialize(view._uiRoot);
        }
        rightPanel.init(view._uiRoot, null);
        rightPanel.father = this;

        if (leftPanel == null)
        {
            leftPanel = new SoldierLeftPanel();
            leftPanel.Initialize(view._uiRoot);
        }

        leftPanel.init(view._uiRoot, this);
        InitItemPoint();
        //PlayOpenAnim();
        BtnEventBinding();
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierView);

        if (CommonFunction.CheckIsOpen(OpenFunctionType.SoldierLevelUp))
        {
            if (this.view.Btn_StrengthButton)
            {
                UISprite Button_Back = this.view.Btn_StrengthButton.transform.FindChild("Background").GetComponent<UISprite>();
                UILabel Button_Label = this.view.Btn_StrengthButton.transform.FindChild("Label").GetComponent<UILabel>();
                if (Button_Back != null)
                {
                    CommonFunction.UpdateWidgetGray(Button_Back, false);
                    CommonFunction.SetUILabelColor(Button_Label, true);
                }
            }
        }
        else
        {
            if (this.view.Btn_StrengthButton)
            {
                UISprite Button_Back = this.view.Btn_StrengthButton.transform.FindChild("Background").GetComponent<UISprite>();
                UILabel Button_Label = this.view.Btn_StrengthButton.transform.FindChild("Label").GetComponent<UILabel>();
                if (Button_Back != null)
                {
                    CommonFunction.UpdateWidgetGray(Button_Back, true);
                    CommonFunction.SetUILabelColor(Button_Label, false);
                }
            }
        }
        this.isEquip = true;
        this.isSoldier = true;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenM, view._uiRoot.transform.parent.transform));
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(SoldierAttView.UIName);
    }

    void Instance_UpdatePlayerItemsEvent()
    {
        if (this.rightPanel.skillIntensifyPanel.root.gameObject.activeSelf)
        {
            this.rightPanel.skillIntensifyPanel.RefreshMaterial();
        }
    }

    void Instance_UpdatePlayerGoldEvent()
    {
        if (this.rightPanel.skillIntensifyPanel.root.activeSelf)
        {
            this.rightPanel.skillIntensifyPanel.RefreshCost();
        }
        if (this.centerPanel.soldierUpStar.root.activeSelf)
        {
            this.centerPanel.soldierUpStar.RefreshCost();
        }
    }
    void _SoldierDepot_SoldierErrorEvent(SoldierControl control, int errorCode, UInt64 uid)
    {
        //武将操作回馈
        if (control == SoldierControl.SoldierSelectResp)
        {
            if (errorCode == 0)
            {

                Soldier temp = PlayerData.Instance._SoldierDepot.FindByUid(uid);
                RoleBase tmpRole = null;
                this.view.SelectShow_Sprite.gameObject.SetActive(true);
                if (temp != null)
                {
                    UIEventListener.Get(this.view.Btn_SelectShowButton.gameObject).onClick = (go) =>
                    {
                        GameObject.Destroy(tmpRole.gameObject);
                        this.view.SelectShow_Panel.SetActive(false);
                    };

                    RoleManager.Instance.CreateSingleRole(temp.showInfoSoldier, 0, 1, ERoleType.ertSoldier, 0,
                    EHeroGender.ehgNone, EFightCamp.efcNone, EFightType.eftUI, this.view.SelectShow_Panel.transform, (vRoleBase) =>
                    {
                        this.view.SelectShow_Panel.SetActive(true);
                        tmpRole = vRoleBase;
                        tmpRole.name = "soldier";
                        TdSpine.SpineBase spine = vRoleBase.gameObject.GetComponent<TdSpine.SpineBase>();
                        if (spine == null)
                            spine = vRoleBase.gameObject.AddComponent<TdSpine.SpineBase>();
                        if (spine != null)
                            spine.setSortingOrder(20);
                        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SOLDIERSETLES);//武将甄选暂代特效
                        UISystem.Instance.SoldierText.PlayEffect();
                        Scheduler.Instance.AddTimer(0.5f, false, () =>
                        {
                            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SOLDIERSETLES);
                        });
                    });
                }
            }
        }
        //if (control ==SoldierControl.SoldierUpStarResp)
        //{
        //    centerPanel.soldierUpStar.ShowEffect();
        //}
        if (errorCode == 0)
        {
            this.centerPanel.RefreshPanel(this.leftPanel.soldier);
            if (control == SoldierControl.SoldierUpLvResq)
            {
                GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierLvUpSucceed);
                centerPanel.soldierIntensify.PlaySoldierUpLevelEffect();
            }
        }
        if (errorCode == (int)ErrorCodeEnum.NotEnoughGold)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
        }
    }
    void _WeaponDepot_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        //操作回馈
        if (errorCode == 0 && control == EquipControl.PutonEquipAndArtifactResp)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierEquipSucceed);
        }

        if (errorCode == 0 && control == EquipControl.PromoteEquipAndArtifactResp)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierEquipStrengthenSucceed);
        }
    }
    public void SetToggle(bool left)
    {
        Main.Instance.StartCoroutine(this.rightPanel.SetToggle(left));
    }
    public void SetOneStepButton(bool isleft, bool value)
    {
        if (isleft)
        {
            this.isSoldier = value;
        }
        else
        {
            this.isEquip = value;
        }
        if (this.isSoldier)
        {
            if (!this.isEquip)
            {
                view.Btn_FastStrengthButton.gameObject.SetActive(false);
                view.Btn_FastEquipButton.gameObject.SetActive(false);
            }
            else
            {
                view.Btn_FastStrengthButton.gameObject.SetActive(true);
                view.Btn_FastEquipButton.gameObject.SetActive(true);
            }
        }
        else
        {
            view.Btn_FastStrengthButton.gameObject.SetActive(false);
            view.Btn_FastEquipButton.gameObject.SetActive(false);
        }
        if (this.isSoldier && !this.isEquip)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierSkillPage);
        }
        if (!this.isSoldier || !this.isEquip)
        {
            //view.Btn_SuitEquipButton.SetActive(false);
            rightPanel.UpdateSuitEffect(null);
        }
        else
        {
            //view.Btn_SuitEquipButton.SetActive(true);
            UpdateSuitEquip();
        }
    }
    public void Refresh(Soldier sd)
    {
        if (sd == null) return;
        if (sd.IsEquipFull())
            this.view.Lbl_FastEquipButton.text = ConstString.FAST_GETOFF;
        else
            this.view.Lbl_FastEquipButton.text = ConstString.FAST_EQUIP;
        this.view.Spt_SuitEquipButton.gameObject.SetActive(true);
        rightPanel.RefreshPanel(sd);
        centerPanel.RefreshPanel(sd);
        UpdateSuitEquip();
    }

    public void UpdateSuitEquip()
    {
        Dictionary<uint, Color> dic = GetSuitEquipAtt();
        if (dic == null || dic.Count <= 0)
        {
            CommonFunction.SetSpriteName(this.view.Spt_SuitEquipButton, "TZ_icon_shuxing2");
        }
        else
        {
            CommonFunction.SetSpriteName(this.view.Spt_SuitEquipButton, "TZ_icon_shuxing");
        }
        rightPanel.UpdateSuitEffect(dic);
    }
    private Dictionary<uint, Color> GetSuitEquipAtt()
    {
        if (centerPanel == null || centerPanel.soldier == null)
            return null;
        Dictionary<uint, SuitEquipedData> dic = new Dictionary<uint, SuitEquipedData>();
        for (int i = 0; i < centerPanel.soldier._equippedDepot._EquiptList.Count; i++)   //首先得到神装装备的套装散件
        {
            Weapon weapon = centerPanel.soldier._equippedDepot._EquiptList[i];
            if (weapon == null)
                continue;
            if (weapon.Att.CoordID == 0)
                continue;
            if (dic.ContainsKey(weapon.Att.CoordID))
            {
                dic[weapon.Att.CoordID].num++;
            }
            else
            {
                SuitEquipedData data = new SuitEquipedData();
                data.soldierUID = centerPanel.soldier.uId;
                data.suitID = weapon.Att.CoordID;
                data.num = 1;
                dic.Add(weapon.Att.CoordID, data);
            }
        }
        Dictionary<uint, Color> suitEquip_dic = new Dictionary<uint, Color>();
        foreach (KeyValuePair<uint, SuitEquipedData> tmp in dic)
        {
            if (tmp.Value == null)
                continue;
            EquipCoordinatesInfo info = ConfigManager.Instance.mEquipCoordinatesConfig.GetEquipCoordinatesInfoByID(tmp.Key);
            if (info == null || info.attributes == null)
                continue;
            for (int i = 0; i < info.attributes.Count; i++)
            {
                EquipCoordAttribute att = info.attributes[i];
                if (att == null)
                    continue;
                if (att.list == null || att.list.Count <= 0)
                    continue;
                if (att.condition <= tmp.Value.num)
                {
                    Color color = Color.green;
                    if (suitEquip_dic.Count > 0)
                    {
                        color = Color.cyan;
                    }
                    if (!suitEquip_dic.ContainsKey(info.coordID))
                    {
                        suitEquip_dic.Add(info.coordID, color);
                    }
                }
            }
        }
        return suitEquip_dic;
    }

    public void SetNull()
    {
        rightPanel.SetNull();
        centerPanel.SetNull();
        this.view.Spt_SuitEquipButton.gameObject.SetActive(false);
    }
    public void OnSoldierEvent(SoldierChange change, int Slot, UInt64 uID)
    {
        if (leftPanel != null)
            leftPanel.RefreshSoldier();
        if (leftPanel.soldier == null)
            return;
        if (leftPanel != null && uID == leftPanel.soldier.uId)
        {
            if (centerPanel != null && centerPanel.soldierIntensify != null && centerPanel.soldierIntensify.root.activeSelf)
            {
                centerPanel.soldierIntensify.setInfo(leftPanel.soldier);
            }
            if (centerPanel != null && centerPanel.soldierUpStar != null && centerPanel.soldierUpStar.root.activeSelf)
            {
                //centerPanel.soldierUpStar.ShowEffect();
                centerPanel.soldierUpStar.OnClose();
            }
        }
    }
    public void OnSoldierDebrisEvent(DebrisChange change, int Slot, UInt64 uID)
    {
        if (change == DebrisChange.Soldier)
        {
            leftPanel.RefreshDebris();

        }
    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, this.view._uiRoot.transform));
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SOLDIERATT);
    }
    public void ButtonEvent_StrengthButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierLevelUp, true))
        {
            return;
        }

        if (leftPanel.soldier == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_SOLDIER);
            return;
        }
        if (leftPanel.soldier.isMaxLevel())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HAD_MAX_LEVEL);
        }
        else
        {
            centerPanel.soldierIntensify.setInfo(leftPanel.soldier);
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierLevelUp);
        }
    }
    public void ButtonEvent_SelectButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (leftPanel.soldier == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_SOLDIER);
            return;
        }
        SoldierAttributeInfo temp = ConfigManager.Instance.mSoldierData.FindById(leftPanel.soldier.Att.evolveId);

        if (temp == null && leftPanel.soldier.IsMaxStep())
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HAD_SOLDIER_MAX_STAR);
            return;
        }

        centerPanel.soldierUpStar.setInfo(leftPanel.soldier);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierStarUp);

        //centerPanel.soldierSelect.setInfo();
    }
    public void ButtonEvent_FastStrengthButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (leftPanel == null || leftPanel.soldier == null) return;
        bool isAllMax = false;
        if (leftPanel.soldier._equippedDepot != null || leftPanel.soldier._equippedDepot._EquiptList != null || leftPanel.soldier._equippedDepot._EquiptList.Count > 0)
        {
            for (int i = 0; i < leftPanel.soldier._equippedDepot._EquiptList.Count; ++i)
            {
                Weapon equip = leftPanel.soldier._equippedDepot._EquiptList[i];
                if (equip == null)
                    continue;

                if (!equip.isMaxLevel())
                {
                    isAllMax = false;
                    break;
                }
                else
                {
                    isAllMax = true;
                }
            }
        }
        if (isAllMax)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_EQUIPA_MAXLEVEL);
            return;
        }
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.OneKeyEnchance, true))
        {
            return;
        }
        EquipModule.Instance.SendOneKeyPromoteAllReq(leftPanel.soldier.uId);
    }
    public void ButtonEvent_FastEquipButton(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (rightPanel == null || rightPanel.soldier == null) return;
        if (rightPanel.soldier.IsEquipFull())
        {
            EquipModule.Instance.SendOneKeyOffEquipReq(rightPanel.soldier.uId);
            return;
        }
        rightPanel.soldier._equippedDepot.FastSelect();
    }
    public void OnskillOnekeyintensify_Button_intensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        //技能升级界面升级按钮
        if (rightPanel.skillIntensifyPanel.infoSkill == null) return;
        if (leftPanel.soldier == null) return;
        if (leftPanel.soldier._skillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot) == SkillCheck.Ok)
        {
            leftPanel.soldier._skillsDepot.AutoStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot);
        }
        else
        {
            if (leftPanel.soldier._skillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot) == SkillCheck.Money)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            }
            ErrorCode.ShowErrorTip((uint)leftPanel.soldier._skillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot));
        }
    }
    private bool _btncd = false;
    public void OnskillIntensifyPanel_Button_intensify(GameObject btn)
    {
        if (_btncd)
            return;
        _btncd = true;
        Assets.Script.Common.Scheduler.Instance.AddTimer(0.2f, false, () =>
        {
            _btncd = false;
        });
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        //技能升级界面升级按钮
        if (rightPanel.skillIntensifyPanel.infoSkill == null) return;
        if (leftPanel.soldier == null) return;
        if (rightPanel.skillIntensifyPanel.QuickMatItem == null) return;
        rightPanel.skillIntensifyPanel.QuickMatItem.ShowTip(OnskillIntensifyPanel_Button_intensify_continue);
    }

    /// <summary>
    /// 由于插入了补全材料的确认面板
    /// 将原函数 OnskillIntensifyPanel_Button_intensify 中的一部分单独成这个函数
    /// </summary>
    public void OnskillIntensifyPanel_Button_intensify_continue()
    {
        SkillCheck result = leftPanel.soldier._skillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot, false);
        if (result == SkillCheck.Ok)
        {
            leftPanel.soldier._skillsDepot.Strong(rightPanel.skillIntensifyPanel.infoSkill.Slot);
        }
        else
        {
            if (result == SkillCheck.Money)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            }
            ErrorCode.ShowErrorTip((uint)leftPanel.soldier._skillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot));
        }
    }

    public void OnskillIntensifyPanel_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //技能升级界面关闭按钮
        rightPanel.skillIntensifyPanel.OnClose();
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseSoldierSkillLvUp);
    }


    public void OnsoldierIntensify_Button_close(GameObject btn)
    {

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //武将升级界面关闭
        centerPanel.soldierIntensify.OnClose();
    }
    public void OnsoldierIntensify_Button_fastSelect(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //武将升级界面快速选择
        centerPanel.soldierIntensify.OnFastChoose();
    }
    public void OnesoldierIntensify_Button_intensify(GameObject btn)
    {
        SoldierStorng_HP = centerPanel.soldierIntensify.infoSoldier.showInfoSoldier.HP;
        SoldierStrong_Att = centerPanel.soldierIntensify.infoSoldier.showInfoSoldier.Attack;
        //Debug.LogError(SoldierStorng_HP + "------------" + SoldierStrong_Att);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Super, this.view._uiRoot.transform));
        if (centerPanel.soldierIntensify.chooseUid.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_CHOOSE_MATERIAL);
            return;
        }
        if (CommonFunction.GetFullBagsItem(centerPanel.soldierIntensify.chooseUid).Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SKIIBOOK_FULL);
            return;
        }
        if (CommonFunction.CheckSholierLifeSoul(centerPanel.soldierIntensify.chooseUid))
        {
            return;
        }

        if (this.markValue)
        {
            PlayerData.Instance._SoldierDepot.Strong(centerPanel.soldierIntensify.infoSoldier.uId, centerPanel.soldierIntensify.chooseUid);
            return;
        }
        //武将升级界面升级
        foreach (UInt64 uid in centerPanel.soldierIntensify.chooseUid)
        {
            Soldier choose = PlayerData.Instance._SoldierDepot.FindByUid(uid);
            if (choose == null)
                continue;
            ChooseCheck check = this.centerPanel.soldierIntensify.ChooseFilter(choose);
            if (check != ChooseCheck.OK)
            {
                string message = "";
                if (check == ChooseCheck.HadHightQuality || check == ChooseCheck.HightQualityButOneLevel)
                    message = ConstString.CHOOSED_HAD_HIGHTQUALITY;
                if (check == ChooseCheck.HadHighStar)
                    message = ConstString.CHOOSE_HAD_HIGHTSTAR;
                if (check == ChooseCheck.HadIntoBattle)
                    message = ConstString.CHOOSED_HAD_INTOBATTLE;
                UISystem.Instance.HintView.ShowMessageBox(
                    MessageBoxType.mb_YesNo_Mark,
                    message,
                    () =>
                    {
                        PlayerData.Instance._SoldierDepot.Strong(centerPanel.soldierIntensify.infoSoldier.uId, centerPanel.soldierIntensify.chooseUid);
                    },
                    null,
                    ConstString.MESSAGEBOXBTN_YES,
                    ConstString.MESSAGEBOXBTN_NO,
                    (bool value) => { this.markValue = value; });
                return;
            }
        }
        PlayerData.Instance._SoldierDepot.Strong(centerPanel.soldierIntensify.infoSoldier.uId, centerPanel.soldierIntensify.chooseUid);
    }
    public void OnsoldierUpStar_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //武将升星界面关闭
        this.centerPanel.soldierUpStar.OnClose();
    }
    /// <summary>
    /// 武将升星升星按钮
    /// </summary>
    /// <param name="btn"></param>
    public void OnesoldierUpStar_Button_intensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (centerPanel == null)
            return;
        if (centerPanel.soldierUpStar == null || centerPanel.soldierUpStar.infoSoldier == null)
            return;

        //武将升星界面升级按钮
        if (!centerPanel.soldierUpStar.isCanNext)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_CHOOSE_STARMATERIAL);
            return;
        }
        centerPanel.soldierUpStar.ExpAndMoney();
        if (centerPanel.soldierUpStar.ExpNeed > 0)
        {
            centerPanel.soldierUpStar.SetInfo_Instance();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_LEVEL_NOTENOUGHT);
            return;
        }
        if (centerPanel.soldierUpStar.needMoney > 0)
        {
            centerPanel.soldierUpStar.SetInfo_Instance();
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_LEVEL_NOTENOUGHT);
            return;
        }
        if (!CommonFunction.CheckMoneyEnough((ECurrencyType)centerPanel.soldierUpStar.StarOrStepMoneyType, centerPanel.soldierUpStar.StarOrStepMoney))
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            return;
        }
        if (CommonFunction.GetSkillReturnBagsItem(centerPanel.soldierUpStar.chooseUid).Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SKIIBOOK_FULL);
            return;
        }
        if (CommonFunction.CheckSholierLifeSoul(centerPanel.soldierUpStar.chooseUid))
        {
            return;
        }
        string message = "";

        if (!markValue)
        {
            foreach (UInt64 uid in centerPanel.soldierUpStar.chooseUid)
            {
                Soldier choose = PlayerData.Instance._SoldierDepot.FindByUid(uid);
                if (choose == null)
                    continue;
                ChooseCheck check = this.centerPanel.soldierIntensify.ChooseFilter(choose);
                if (check == ChooseCheck.HadIntoBattle)
                {
                    message = "";
                    if (check == ChooseCheck.HadIntoBattle)
                        message = ConstString.CHOOSED_HAD_INTOBATTLE;
                    UISystem.Instance.HintView.ShowMessageBox(
                        MessageBoxType.mb_YesNo_Mark,
                        message,
                        () =>
                        {
                            if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
                                SoldierModule.Instance.SendSoldierUpLevelStarReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                            else
                                SoldierModule.Instance.SendSoldierUpStepReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                        },
                        null,
                        ConstString.MESSAGEBOXBTN_YES,
                        ConstString.MESSAGEBOXBTN_NO,
                        (bool value) => { this.markValue = value; });
                    return;
                }
            }
        }
        message = "";
        for (int i = 0; i < centerPanel.soldierUpStar.chooseUid.Count; ++i)
        {
            Soldier chooseSoldier = PlayerData.Instance._SoldierDepot.FindByUid(centerPanel.soldierUpStar.chooseUid[i]);
            if (chooseSoldier._skillsDepot.GetReturnNum() != 0)
            {
                if (chooseSoldier._equippedDepot.IsEquiped())
                    message = string.Format(ConstString.SOLDIER_UPSTAR_SKILLANDEQUIP, chooseSoldier._skillsDepot.GetReturnNum());
                else
                    message = string.Format(ConstString.SOLDIER_UPSTAR_SKILL, chooseSoldier._skillsDepot.GetReturnNum());
            }
            else
            {
                if (chooseSoldier._equippedDepot.IsEquiped())
                {
                    message = ConstString.SOLDIER_UPSTAR_DEQUIP;
                }
            }
        }
        if (!message.Equals(""))
        {
            UInt64 soldierUid = centerPanel.soldier.uId;
            List<UInt64> chooseUid = new List<ulong>(centerPanel.soldierUpStar.chooseUid);
            UISystem.Instance.HintView.ShowMessageBox
                (
                MessageBoxType.mb_YesNo,
                message,
                () =>
                {
                    if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
                        SoldierModule.Instance.SendSoldierUpLevelStarReq(soldierUid, chooseUid, new List<ulong>());
                    else
                        SoldierModule.Instance.SendSoldierUpStepReq(soldierUid, chooseUid, new List<ulong>());

                },
                null,
                ConstString.HINT_LEFTBUTTON_GOON,
                ConstString.HINT_RIGHTBUTTON_CANCEL
                );
            return;
        }
        if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
            SoldierModule.Instance.SendSoldierUpLevelStarReq(centerPanel.soldier.uId, centerPanel.soldierUpStar.chooseUid, new List<ulong>());
        else
            SoldierModule.Instance.SendSoldierUpStepReq(centerPanel.soldier.uId, centerPanel.soldierUpStar.chooseUid, new List<ulong>());
    }
    /// <summary>
    /// 武将升星经验补充按钮
    /// </summary>
    /// <param name="btn"></param>
    public void OnesoldierUpStar_intensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        int expMlp = (centerPanel.soldierUpStar.ExpNeed - centerPanel.soldierUpStar.EXP) > 0 ? centerPanel.soldierUpStar.ExpNeed - centerPanel.soldierUpStar.EXP : 0;
        int num = centerPanel.soldierUpStar.needMoney + expMlp * int.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SOLDIERUPSTAR_EXPADD));
        if (!PlayerData.Instance.GoldIsEnough(centerPanel.soldierUpStar.StarOrStepMoneyType, centerPanel.soldierUpStar.StarOrStepMoney + num))
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HINT_NO_GOLD);
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
            return;
        }
        List<ulong> tmpList = new List<ulong>();
        tmpList.AddRange(centerPanel.soldierUpStar.chooseUid);
        tmpList.AddRange(centerPanel.soldierUpStar.chooseList);
        if (CommonFunction.GetSkillReturnBagsItem(tmpList).Count > 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SKIIBOOK_FULL);
            return;
        }
        if (CommonFunction.CheckSholierLifeSoul(tmpList))
        {
            return;
        }
        string message = "";

        if (!markValue)
        {
            ChooseCheck check = ChooseCheck.OK;
            foreach (UInt64 uid in centerPanel.soldierUpStar.chooseUid)
            {
                Soldier choose = PlayerData.Instance._SoldierDepot.FindByUid(uid);
                if (choose == null)
                    continue;
                check = this.centerPanel.soldierIntensify.ChooseFilter(choose);
                if (check == ChooseCheck.HadIntoBattle)
                    break;
                else
                    check = ChooseCheck.OK;
            }
            if(check == ChooseCheck.OK)
            {
                foreach (UInt64 uid in centerPanel.soldierUpStar.chooseList)
                {
                    Soldier choose = PlayerData.Instance._SoldierDepot.FindByUid(uid);
                    if (choose == null)
                        continue;
                    ChooseCheck tmpcheck = this.centerPanel.soldierIntensify.ChooseFilter(choose);
                    if ((int)tmpcheck >= (int)check)
                        check = tmpcheck;
                    if (check == ChooseCheck.HadIntoBattle)
                        break;
                }
            }
            if (check != ChooseCheck.OK)
            {
                message = "";
                if (check == ChooseCheck.HadHightQuality || check == ChooseCheck.HightQualityButOneLevel)
                    message = ConstString.CHOOSED_HAD_HIGHTQUALITY;
                if (check == ChooseCheck.HadHighStar)
                    message = ConstString.CHOOSE_HAD_HIGHTSTAR;
                if (check == ChooseCheck.HadIntoBattle)
                    message = ConstString.CHOOSED_HAD_INTOBATTLE;
                UISystem.Instance.HintView.ShowMessageBox(
                    MessageBoxType.mb_YesNo_Mark,
                    message,
                    () =>
                    {
                        if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
                            SoldierModule.Instance.SendSoldierUpLevelStarReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                        else
                            SoldierModule.Instance.SendSoldierUpStepReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                    },
                    null,
                    ConstString.MESSAGEBOXBTN_YES,
                    ConstString.MESSAGEBOXBTN_NO,
                    (bool value) => { this.markValue = value; });
                return;
            } 
        }
        message = "";
        for (int i = 0; i < tmpList.Count; ++i)
        {
            Soldier chooseSoldier = PlayerData.Instance._SoldierDepot.FindByUid(tmpList[i]);
            if (chooseSoldier._skillsDepot.GetReturnNum() != 0)
            {
                if (chooseSoldier._equippedDepot.IsEquiped())
                    message = string.Format(ConstString.SOLDIER_UPSTAR_SKILLANDEQUIP, chooseSoldier._skillsDepot.GetReturnNum());
                else
                    message = string.Format(ConstString.SOLDIER_UPSTAR_SKILL, chooseSoldier._skillsDepot.GetReturnNum());
            }
            else
            {
                if (chooseSoldier._equippedDepot.IsEquiped())
                {
                    message = ConstString.SOLDIER_UPSTAR_DEQUIP;
                }
            }
        }
        if (!message.Equals(""))
        {
            UInt64 soldierUid = centerPanel.soldier.uId;
            UISystem.Instance.HintView.ShowMessageBox
                (
                MessageBoxType.mb_YesNo,
                message,
                () =>
                {
                    if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
                        SoldierModule.Instance.SendSoldierUpLevelStarReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                    else
                        SoldierModule.Instance.SendSoldierUpStepReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
                },
                null,
                ConstString.HINT_LEFTBUTTON_GOON,
                ConstString.HINT_RIGHTBUTTON_CANCEL
                );
            return;
        }
        if (centerPanel.soldierUpStar.type == STEPORSTAR.STAR)
            SoldierModule.Instance.SendSoldierUpLevelStarReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
        else
            SoldierModule.Instance.SendSoldierUpStepReq(centerPanel.soldierUpStar.infoSoldier.uId, centerPanel.soldierUpStar.chooseUid, centerPanel.soldierUpStar.chooseList);
    }
    public void OnesoldierUpStar_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        centerPanel.soldierUpStar.OnReturn();
    }
    public void OndebrisCompound_Button(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));
        if (leftPanel.debris == null)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NO_HAD_DERIS);
            return;
        }
        //碎片合成
        PlayerData.Instance._SoldierDebrisDepot.compound(leftPanel.debris.Att.id);
    }
    public void OnsoldierSelect_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //甄选关闭
        centerPanel.soldierSelect.OnClose();
    }
    public void OnsoldierSelect_Button_fastSelect(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //甄选快速选择
        centerPanel.soldierSelect.FastChoose();
    }
    public void OnesoldierSelect_Button_intensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this.view._uiRoot.transform));

        //甄选按钮
        if (centerPanel.soldierSelect.selectList.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_CHOOSE_MATERIAL);
            return;
        }
        int star = 0;
        foreach (Soldier temp in centerPanel.soldierSelect.selectList)
        {
            if (temp == null)
                continue;
            star += temp.Att.Star;
        }
        if (star < centerPanel.soldierSelect.leftList.Count)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.PLEASE_CHOOSE_MATERIAL);
            return;
        }
        //int cost = ConfigManager.Instance.mSoldierSelectData.FindByQuality
        //(
        //    (centerPanel.soldierSelect.selectList[0].Att.quality) < 5 ? (centerPanel.soldierSelect.selectList[0].Att.quality) : 5
        //).money_num;
        //if (cost > PlayerData.Instance._Gold)
        //{
        //    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);

        //    ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold);
        //    return;
        //}

        List<UInt64> material = new List<ulong>();
        foreach (var temp in centerPanel.soldierSelect.selectList)
        {
            material.Add(temp.uId);
        }
        if (CommonFunction.GetFullBagsItem(material).Count > 0)
        {
            ErrorCode.ShowErrorTip((int)ErrorCodeEnum.OverItem);
            return;
        }
        SoldierModule.Instance.SendSoldierSelectReq(material);
    }
    public void ButtonEvent_Filter_Button(GameObject btn)
    {
        this.leftPanel.SetFilterValue(true);
    }
    public void ButtonEvent_FilterCancel_Button(GameObject btn)
    {
        this.leftPanel.SetFilterValue(false);
    }
    public void PressEvent_SuitEquipAtt(GameObject go, bool isPress)
    {
        if (isPress)
        {
            UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_SUITEQUIPATT);
            if (UISystem.Instance.SuitEquipAttView != null)
                UISystem.Instance.SuitEquipAttView.UpdateInfo(this.centerPanel.soldier);
        }
        else
        {
            UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_SUITEQUIPATT);
        }
    }

    public override UIBoundary GetUIBoundary()
    {
        return view.Boundary;
    }

    public override void Uninitialize()
    {
        if (PlayerData.Instance._SoldierEquip != null)
            PlayerData.Instance._SoldierEquip.ErrotDeleteEvent -= _WeaponDepot_ErrotDeleteEvent;
        if (PlayerData.Instance._SoldierDepot != null)
            PlayerData.Instance._SoldierDepot.SoldierErrorEvent -= _SoldierDepot_SoldierErrorEvent;
        if (PlayerData.Instance._SoldierDepot != null)
            PlayerData.Instance._SoldierDepot.SoldierDepotEvent -= OnSoldierEvent;
        if (PlayerData.Instance._SoldierDebrisDepot != null)
            PlayerData.Instance._SoldierDebrisDepot.SoldierDebrisEvent -= OnSoldierDebrisEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent -= Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
        if (leftPanel != null)
            leftPanel.OnClose();
        if (rightPanel != null)
            rightPanel.OnClose();
        if (centerPanel != null)
            centerPanel.OnClose();
        ResourceLoadManager.Instance.ReleaseRequestBundle();
    }

    public void BtnEventBinding()
    {
        PlayerData.Instance._SoldierDepot.SoldierDepotEvent += OnSoldierEvent;
        PlayerData.Instance._SoldierDebrisDepot.SoldierDebrisEvent += OnSoldierDebrisEvent;
        PlayerData.Instance._SoldierEquip.ErrotDeleteEvent += _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SoldierDepot.SoldierErrorEvent += _SoldierDepot_SoldierErrorEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent += Instance_UpdatePlayerItemsEvent;

        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_StrengthButton.gameObject).onClick = ButtonEvent_StrengthButton;
        UIEventListener.Get(view.Btn_SelectButton.gameObject).onClick = ButtonEvent_SelectButton;
        UIEventListener.Get(view.Btn_FastStrengthButton.gameObject).onClick = ButtonEvent_FastStrengthButton;
        UIEventListener.Get(view.Btn_FastEquipButton.gameObject).onClick = ButtonEvent_FastEquipButton;
        UIEventListener.Get(leftPanel.Filter_Button.gameObject).onClick = ButtonEvent_Filter_Button;
        UIEventListener.Get(leftPanel.FilterCancel_Button.gameObject).onClick = ButtonEvent_FilterCancel_Button;
        //技能升级界面
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Btn_Button_onekeyintensify.gameObject).onClick = OnskillOnekeyintensify_Button_intensify;
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Btn_Button_intensify.gameObject).onClick = OnskillIntensifyPanel_Button_intensify;
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Marsk).onClick = OnskillIntensifyPanel_Button_close;
        //武将升级
        UIEventListener.Get(centerPanel.soldierIntensify.Btn_Button_close.gameObject).onClick = OnsoldierIntensify_Button_close;
        UIEventListener.Get(centerPanel.soldierIntensify.Btn_Button_fastSelect.gameObject).onClick = OnsoldierIntensify_Button_fastSelect;
        UIEventListener.Get(centerPanel.soldierIntensify.Btn_Button_intensify.gameObject).onClick = OnesoldierIntensify_Button_intensify;
        //武将升星
        UIEventListener.Get(centerPanel.soldierUpStar.Btn_Button_close.gameObject).onClick = OnsoldierUpStar_Button_close;
        UIEventListener.Get(centerPanel.soldierUpStar.Btn_Button_intensify.gameObject).onClick = OnesoldierUpStar_Button_intensify;
        UIEventListener.Get(centerPanel.soldierUpStar.intensify.gameObject).onClick = OnesoldierUpStar_intensify;
        UIEventListener.Get(centerPanel.soldierUpStar.back.gameObject).onClick = OnesoldierUpStar_close;
        //碎片合成
        UIEventListener.Get(leftPanel.Debris_Compound.gameObject).onClick = OndebrisCompound_Button;
        //武将甄选
        UIEventListener.Get(centerPanel.soldierSelect.Btn_Button_close.gameObject).onClick = OnsoldierSelect_Button_close;
        UIEventListener.Get(centerPanel.soldierSelect.Btn_Button_fastChoose.gameObject).onClick = OnsoldierSelect_Button_fastSelect;
        UIEventListener.Get(centerPanel.soldierSelect.Btn_Button_intensify.gameObject).onClick = OnesoldierSelect_Button_intensify;

        UIEventListener.Get(view.Btn_SuitEquipButton).onPress = PressEvent_SuitEquipAtt;
    }

    public override void Destroy()
    {
        base.Destroy();
        view = null;
        centerPanel = null;
        rightPanel = null;
        leftPanel = null;
        this.markValue = false;
    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}
    //public void PlayOpenEquipInfoAnim()
    //{
    //    view.EquipInfo_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.EquipInfo_TScale.Restart();
    //    view.EquipInfo_TScale.PlayForward();
    //}
    //public void PlayOpenSoldierIntensifyAnim()
    //{
    //    view.Intensify_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Intensify_TScale.Restart();
    //    view.Intensify_TScale.PlayForward();
    //}
    //public void PlayOpenSelectAnim()
    //{
    //    view.Select_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Select_TScale.Restart(); view.Select_TScale.PlayForward();
    //}
    //public void PlayOpenSkilAnim()
    //{
    //    view.Skil_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Skil_TScale.Restart(); view.Skil_TScale.PlayForward();
    //}
    //public void PlayOpenArtifactAnim()
    //{
    //    view.Artifact_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.Artifact_TScale.Restart(); view.Artifact_TScale.PlayForward();
    //}
    //public void PlayOpenSoliderUpStarAnim()
    //{
    //    view.SoldierUPstar_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.SoldierUPstar_TScale.Restart(); view.SoldierUPstar_TScale.PlayForward();
    //}
    //public void PlayOpenEquipUpStarAnim()
    //{
    //    view.EquipUPstar_TScale.gameObject.transform.localScale = Vector3.one * GlobalConst.ViewScaleAnim;
    //    view.EquipUPstar_TScale.Restart(); view.EquipUPstar_TScale.PlayForward();
    //}
    public void PlayFastStrengthEffect(int postion)
    {
        //Debug.LogError("sssssssssssssssssssss" + postion);
        //    for (int i = 0; i < count; i++)
        //    {
        if (Go_ItemEffect1 == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_FASTINTENSIFY, (GameObject gb) => { Go_ItemEffect1 = gb; });

        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_ItemEffect1, ItemPointList[postion - 1].transform);
        //}
    }
    public void PlayFastIntensifyEffect(int postion)
    {
        //Debug.LogError("sssssssssssssssssssss" + postion);
        //    for (int i = 0; i < count; i++)
        //    {
        if (Go_ItemEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_FASTSTRENGTH, (GameObject gb) => { Go_ItemEffect = gb; });

        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_ItemEffect, ItemPointList[postion - 1].transform);
        //}
    }
    public void PlayOndebrisCompound()//碎片合成
    {
        if (Go_CompoundEffect == null)
        {
            ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_ONDEBRISCOMPOUND, (GameObject gb) => { Go_CompoundEffect = gb; });
        }
        GameObject go = ShowEffectManager.Instance.ShowEffect(Go_CompoundEffect, centerPanel.UIPanel_Panel_player.transform);
    }

    public void OneKeyPromoteAllEffect()
    {

    }
    private void InitItemPoint()
    {
        ItemPointList.Clear();
        ItemPointList.Add(view.ItemPoint1);
        ItemPointList.Add(view.ItemPoint2);
        ItemPointList.Add(view.ItemPoint3);
        ItemPointList.Add(view.ItemPoint4);
        ItemPointList.Add(view.ItemPoint5);
        ItemPointList.Add(view.ItemPoint6);
        FilterPointList.Clear();
        FilterPointList.Add(view.FilterEffectPos_0);
        FilterPointList.Add(view.FilterEffectPos_1);
        FilterPointList.Add(view.FilterEffectPos_2);
        FilterPointList.Add(view.FilterEffectPos_3);
        FilterPointList.Add(view.FilterEffectPos_4);
        FilterPointList.Add(view.FilterEffectPos_5);
    }

    public void ShowSoldierFilterEffect(List<Soldier> data, bool IsFilter)//筛选特效
    {
        if (!IsFilter) { return; }
        if (data.Count == 0) { return; }
        if (Go_FilterEffect == null) { ResourceLoadManager.Instance.LoadEffect(GlobalConst.DIR_EFFECT_FASTINTENSIFY, (GameObject gb) => { Go_FilterEffect = gb; }); }
        int F = data.Count;
        if (F > 6) { F = 6; };
        for (int i = 0; i < F; i++)
        {
            GameObject go = ShowEffectManager.Instance.ShowEffect(Go_FilterEffect, FilterPointList[i].transform);

        }
        Main.Instance.StartCoroutine(CloseEffectMask(0.5F));
    }
    public IEnumerator CloseEffectMask(float time)
    {
        view.Go_EffectMask.SetActive(true);
        yield return new WaitForSeconds(time);
        view.Go_EffectMask.SetActive(false);
    }

}
