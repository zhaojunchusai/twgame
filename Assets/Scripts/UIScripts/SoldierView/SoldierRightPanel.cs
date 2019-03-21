using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class SoldierRightPanel
{
    static int SkillMaxCount = 6;
    public GameObject _uiRoot;
    public Soldier soldier;
    public SoldierEquipChoosePanel choosePanel;
    public SDSKIntensifyPanel skillIntensifyPanel;
    public SoldierAttViewController father;
    private UIWrapContent WrapContent;
    public UIScrollView ScrollView;
    public UISprite equipPrompt;
    public UISprite skillPrompt;

    public UIGrid Grd_Grid;
    public GameObject Item;
    public GameObject Lock;
    public UILabel PowerLabel;
    private List<Skill> tempList;
    private List<SkillItemComponent> _skillItemList = new List<SkillItemComponent>();

    public bool ScrollToolInit = false;
    public void Initialize(GameObject _uiRoot)
    {
        this._uiRoot = _uiRoot;
        Grd_Grid = this._uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIGrid>();
        Item = this._uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/item").gameObject;
        PowerLabel = this._uiRoot.transform.FindChild("Anim/center/fighting/Label_Fighting").gameObject.GetComponent<UILabel>();
        WrapContent = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView").gameObject.GetComponent<UIScrollView>();
        equipPrompt = this._uiRoot.transform.FindChild("Anim/right/TabEquip/prompt").gameObject.GetComponent<UISprite>();
        skillPrompt = this._uiRoot.transform.FindChild("Anim/right/TabSkill/prompt").gameObject.GetComponent<UISprite>();
        Lock = this._uiRoot.transform.FindChild("Anim/PanelLock/Lock").gameObject;
        Lock.SetActive(false);
        equipPrompt.gameObject.SetActive(false);
        skillPrompt.gameObject.SetActive(false);
    }
    public void init(GameObject _uiRoot, Soldier soldier)
    {
        this.soldier = soldier;
        UIToggle toggle = this._uiRoot.transform.FindChild("Anim/right/TabEquip").gameObject.GetComponent<UIToggle>();
        toggle.Set(true);

        EventDelegate.Add(toggle.onChange, Toggle);

        if (skillIntensifyPanel == null)
        {
            skillIntensifyPanel = new SDSKIntensifyPanel();
            skillIntensifyPanel.init(_uiRoot);
        }
        if (this.soldier != null)
        {
            SetEquiped();
            soldier._equippedDepot.father.WeaponDepotEvent += OnEquipEvent;
            if (ScrollToolInit)
                InitTaskItem();
            soldier._skillsDepot.SkillsDepotEvent += OnSkillEvent;
            soldier._skillsDepot.SkillsErrorEvent += _skillsDepot_SkillsErrorEvent;
        }
        BtnEventBinding();
    }
    public IEnumerator SetToggle(bool left)
    {
        yield return 0;
        if (left)
        {
            UIToggle toggle = this._uiRoot.transform.FindChild("Anim/right/TabEquip").gameObject.GetComponent<UIToggle>();
            toggle.Set(left);
        }
        else
        {
            UIToggle toggle = this._uiRoot.transform.FindChild("Anim/right/TabSkill").gameObject.GetComponent<UIToggle>();
            toggle.Set(!left);
        }
    }
    public void Toggle()
    {
        bool val = UIToggle.current.value;
        if (val)
        {
            ScrollToolInit = false;
            father.SetOneStepButton(false, true);
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip))
            {
                this.Lock.SetActive(true);
                UILabel label = Lock.transform.FindChild("Label_Promt").gameObject.GetComponent<UILabel>();
                if (label)
                {
                    OpenLevelData tmp = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.SoldierEquip);
                    if (tmp != null && tmp.gateId != -1)
                    {
                        StageInfo tmpInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)tmp.gateId);

                        if (tmpInfo != null)
                        {
                            label.text = string.Format(ConstString.FUNCTION_LOCK, tmpInfo.GateSequence);
                        }
                    }
                }
            }
            else
            {
                this.Lock.SetActive(false);
            }
        }
        else
        {
            ScrollToolInit = true;
            father.SetOneStepButton(false, false);
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierSkill))
            {
                this.Lock.SetActive(true);
                UILabel label = Lock.transform.FindChild("Label_Promt").gameObject.GetComponent<UILabel>();
                if (label)
                {
                    OpenLevelData tmp = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.SoldierSkill);
                    if (tmp != null && tmp.gateId != -1)
                    {
                        StageInfo tmpInfo = ConfigManager.Instance.mStageData.GetInfoByID((uint)tmp.gateId);
                        if (tmpInfo != null)
                        {
                            label.text = string.Format(ConstString.FUNCTION_LOCK, tmpInfo.GateSequence);
                        }
                    }
                }
            }
            else
            {
                this.Lock.SetActive(false);
            }
            this.InitTaskItem();
        }
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
    }

    void _skillsDepot_SkillsErrorEvent(SkillControl control, int errorCode)
    {
        //技能操作回馈
        if (errorCode == 0 && control == SkillControl.UpgradeSkillResp)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.SoldierSkillStrengthenSucceed);
        }
    }
    public void OnEquipEvent(WeaponChange change, int index, UInt64 uID)
    {
        //if (change == WeaponChange.EquiptList)
        {
            if (this._uiRoot == null)
                return;
            SetEquiped();
            if (this.soldier != null)
            {
                father.centerPanel.RefreshPanel(this.soldier);
                this.PowerLabel.text = this.soldier.showInfoSoldier.CombatPower.ToString();
                if (father != null)
                {
                    if (this.soldier.IsEquipFull())
                        father.view.Lbl_FastEquipButton.text = ConstString.FAST_GETOFF;
                    else
                        father.view.Lbl_FastEquipButton.text = ConstString.FAST_EQUIP;
                }
            }
            father.UpdateSuitEquip();
        }
    }
    public void OnSkillEvent(SkillChange change, int index, UInt64 uID)
    {
        if (change == SkillChange.All)
        {
            this.SkillPrompt();
            if (ScrollToolInit)
                InitTaskItem();
        }
        else
        {
            this.PowerLabel.text = this.soldier.showInfoSoldier.CombatPower.ToString();

            if (skillIntensifyPanel.root.activeSelf)
            {
                if (skillIntensifyPanel.infoSkill.uId == uID)
                {
                    Skill p = this.soldier._skillsDepot.FindByUid(uID);
                    if (p != null)
                        skillIntensifyPanel.setInfo(p);
                }
            }
        }
    }
    public void RefreshPanel(Soldier soldier)
    {
        if (this.soldier != null)
        {
            if (this.soldier._equippedDepot.father != null)
                this.soldier._equippedDepot.father.WeaponDepotEvent -= OnEquipEvent;
            this.soldier._skillsDepot.SkillsDepotEvent -= OnSkillEvent;
            this.soldier._skillsDepot.SkillsErrorEvent -= _skillsDepot_SkillsErrorEvent;
        }
        this.soldier = soldier;
        if (this.soldier != null)
        {
            if (ScrollToolInit)
                this.InitTaskItem();
            this.PowerLabel.text = this.soldier.showInfoSoldier.CombatPower.ToString();
            this.SetEquiped();
            this.SkillPrompt();
            if (this.soldier._equippedDepot.father != null)
            {
                this.soldier._equippedDepot.father.WeaponDepotEvent -= OnEquipEvent;
                this.soldier._equippedDepot.father.WeaponDepotEvent += OnEquipEvent;
            }
            
            this.soldier._skillsDepot.SkillsDepotEvent += OnSkillEvent;
            this.soldier._skillsDepot.SkillsErrorEvent += _skillsDepot_SkillsErrorEvent;
        }
        UpdateSuitEquip();
    }

    private void UpdateSuitEquip()
    {
        Dictionary<uint, SuitEquipedData> dic = new Dictionary<uint, SuitEquipedData>();
        for (int i = 0; i < this.soldier._equippedDepot._EquiptList.Count; i++)   //首先得到神装装备的套装散件
        {
            Weapon weapon = this.soldier._equippedDepot._EquiptList[i];
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
                data.soldierUID = this.soldier.uId;
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
            bool status = false;  //记录当前是否满足基本条件  满足基本条件的才进入计算
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
                    if (suitEquip_dic.Count > 1)
                    {
                        color = Color.blue;
                    }
                    if (!suitEquip_dic.ContainsKey(info.coordID))
                    {
                        suitEquip_dic.Add(info.coordID, color);
                    }
                }
            }
        }
        UpdateSuitEffect(suitEquip_dic);
    }

    public void OnClose()
    {
        if (this.soldier == null) return;
        soldier._skillsDepot.SkillsDepotEvent -= OnSkillEvent;
        soldier._skillsDepot.SkillsErrorEvent -= _skillsDepot_SkillsErrorEvent;
        if (this.soldier._equippedDepot.father != null)
            this.soldier._equippedDepot.father.WeaponDepotEvent -= OnEquipEvent;
    }
    public void SetNull()
    {
        this.soldier = null;
        this.equipPrompt.gameObject.SetActive(false);
        this.skillPrompt.gameObject.SetActive(false);
        for (int i = 0; i < 6; ++i)
        {
            int count = i;
            GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", i)).gameObject;
            UISprite SPT_Quality = TC_equip.transform.FindChild("quality").GetComponent<UISprite>();

            UIEventListener.Get(TC_equip).onClick = (go) =>
            {
                return;
            };

            UISprite Tex_equipt = TC_equip.transform.FindChild("equip").gameObject.GetComponent<UISprite>();
            if (Tex_equipt)
            {
                Tex_equipt.gameObject.SetActive(false);
            }
            UISprite STP_back = TC_equip.transform.FindChild("back").gameObject.GetComponent<UISprite>();
            if (STP_back)
            {
                STP_back.gameObject.SetActive(false);
            }
            if (SPT_Quality)
            {
                SPT_Quality.gameObject.SetActive(false);
            }
            UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
            if (Level)
            {
                Level.gameObject.SetActive(false);
            }
            UILabel Lbl_Label = TC_equip.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
            if (Lbl_Label)
            {
                Lbl_Label.text = "";
            }
            GameObject check = TC_equip.transform.FindChild("check").gameObject;
            if (check)
            {
                check.SetActive(false);
            }
            UIGrid Grid_star = TC_equip.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
            if (Grid_star)
            {
                Grid_star.gameObject.SetActive(false);
            }
            GameObject CanChange = TC_equip.transform.FindChild("CanChange").gameObject;
            if (CanChange)
            {
                CanChange.gameObject.SetActive(false);
            }
            GameObject canEquip = TC_equip.transform.FindChild("back/canEquip").gameObject;
            if (canEquip)
            {
                canEquip.SetActive(false);
            }
        }
        Main.Instance.StartCoroutine(CreatTaskItem(new List<Skill>()));
    }
    public void SetEquiped()
    {
        if (this.soldier == null) return;
        if (this._uiRoot == null) return;
        var playerAtt = PlayerData.Instance;
        if (this.equipPrompt != null)
            this.equipPrompt.gameObject.SetActive(false);
        for (int i = 0; i < 6; ++i)
        {
            int count = i;
            GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", i)).gameObject;
            UISprite SPT_Quality = TC_equip.transform.FindChild("quality").GetComponent<UISprite>();
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip))
            {
                UISprite STP_back = TC_equip.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                if (STP_back != null)
                {
                    if (this.soldier.Att.PosList.ContainsKey(i + 1))
                    {
                        CommonFunction.SetSpriteName(STP_back, CommonFunction.EquipBackSpriteName(this.soldier.Att.PosList[i + 1]));
                        STP_back.gameObject.SetActive(true);
                    }
                    CommonFunction.UpdateWidgetGray(STP_back, true);
                }
                UISprite Tex_equipt = TC_equip.transform.FindChild("equip").gameObject.GetComponent<UISprite>();
                if (Tex_equipt)
                {
                    Tex_equipt.gameObject.SetActive(false);
                }
                UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
                if (Level)
                {
                    Level.gameObject.SetActive(false);
                }
                GameObject canEquip = TC_equip.transform.FindChild("back/canEquip").gameObject;
                if (canEquip)
                {
                    canEquip.SetActive(false);
                }
                UIGrid Grid_star = TC_equip.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
                if (Grid_star)
                {
                    Grid_star.gameObject.SetActive(false);
                }
                UILabel Lbl_Label = TC_equip.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
                if (Lbl_Label)
                {
                    Lbl_Label.text = "";
                }
                if (SPT_Quality)
                {
                    SPT_Quality.gameObject.SetActive(false);
                }
                if (Lock)
                {
                    Lock.SetActive(true);
                }
                continue;
            }
            else
            {
                if (Lock)
                {
                    Lock.SetActive(false);
                }
                UISprite STP_back = TC_equip.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                if (STP_back != null)
                {
                    CommonFunction.UpdateWidgetGray(STP_back, false);
                }
            }
            if (this.soldier._equippedDepot == null)
                return;
            Weapon tp = soldier._equippedDepot._EquiptList[i];
            UIEventListener.Get(TC_equip).onClick = (go) =>
            {
                if (this.father == null)
                    return;
                if (this.soldier == null)
                    return;
                if (!father.isSoldier)
                {
                    return;
                }

                if (!soldier.Att.PosList.ContainsKey(count + 1)) return;
                //装备点击事件
                int index = count;

                if (tp == null)
                {
                    choosePanel = SoldierEquipChoosePanel.create(this.soldier);
                    choosePanel.SetParent(_uiRoot.transform);
                    if (this.soldier.Att.PosList.ContainsKey(index + 1))
                        choosePanel.view.InitWeaponItem(index, this.soldier.Att.PosList[index + 1]);

                    //UISystem.Instance.ResortViewOrder();
                    choosePanel.view._uiRoot.SetActive(true);
                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierChangeEquip);
                }
                else
                {
                    //装备详情
                    //this.eqInfoPanel.setInfo(tp);
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
                    UISystem.Instance.EquipDetailInfoView.UpdateViewInfo(tp, 2, this.soldier);
                    //UISystem.Instance.ResortViewOrder();
                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenSoldierEquipDetail);
                }
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));

            };

            if (tp != null)
            {
                UISprite Tex_equipt = TC_equip.transform.FindChild("equip").gameObject.GetComponent<UISprite>();
                if (Tex_equipt)
                {
                    CommonFunction.SetSpriteName(Tex_equipt, tp.Att.icon);
                    Tex_equipt.gameObject.SetActive(true);
                }
                UISprite STP_back = TC_equip.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                if (STP_back)
                {
                    STP_back.gameObject.SetActive(true);
                }
                UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
                if (Level)
                {
                    Level.gameObject.SetActive(true);
                }

                UILabel Lbl_Label = TC_equip.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
                if (Lbl_Label)
                {
                    Lbl_Label.text = tp.Lv.ToString();
                }
                if (SPT_Quality)
                {
                    CommonFunction.SetQualitySprite(SPT_Quality, tp.Att.quality, STP_back);
                    SPT_Quality.gameObject.SetActive(true);
                }
                GameObject check = TC_equip.transform.FindChild("check").gameObject;
                if (check)
                {
                    if (tp.enableStrong() == WeaponCheck.Ok)
                    {
                        check.SetActive(true);
                    }
                    else
                    {
                        check.SetActive(false);
                    }
                }
                GameObject canEquip = TC_equip.transform.FindChild("back/canEquip").gameObject;
                if (canEquip)
                {
                    canEquip.SetActive(false);
                }
                UIGrid Grid_star = TC_equip.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
                if (Grid_star)
                {
                    Grid_star.gameObject.SetActive(true);
                    var tempList = Grid_star.GetChildList();
                    for (int j = 0; j < tempList.Count; ++j)
                    {
                        GameObject star = tempList[j].FindChild("SelectSprite").gameObject;
                        if (j < tp.Att.star)
                        {
                            star.SetActive(true);
                        }
                        else
                        {
                            star.SetActive(false);
                        }
                    }
                }
                GameObject CanChange = TC_equip.transform.FindChild("CanChange").gameObject;

                if (CanChange)
                {
                    if (
                            PlayerData.Instance._SoldierEquip.getWeaponList
                            (
                                 (wp) =>
                                 {
                                     if (count == -1)
                                         return false;
                                     if (this.soldier._equippedDepot._EquiptList.Count >= count)
                                     {
                                         Weapon tmpWp = this.soldier._equippedDepot._EquiptList[count];

                                         if (tmpWp == null)
                                             return false;
                                         if (wp.isEquiped)
                                             return false;
                                         if (wp.Att.quality <= tmpWp.Att.quality)
                                             return false;
                                         if (this.soldier.Att.PosList[count + 1] != (int)wp.Att.type)
                                             return false;
                                         return this.soldier.Level >= (int)wp.Att.levelLimit;
                                     }
                                     else
                                         return false;
                                 }

                            ).Count > 0
                      )
                    {
                        CanChange.SetActive(true);
                        if (CommonFunction.IsAlreadyBattle(this.soldier.uId) && this.equipPrompt != null)
                            this.equipPrompt.gameObject.SetActive(true);
                    }
                    else
                        CanChange.SetActive(false);

                }
            }
            else
            {
                UISprite Tex_equipt = TC_equip.transform.FindChild("equip").gameObject.GetComponent<UISprite>();
                if (Tex_equipt)
                {
                    Tex_equipt.gameObject.SetActive(false);
                }
                UISprite STP_back = TC_equip.transform.FindChild("back").gameObject.GetComponent<UISprite>();
                if (STP_back)
                {
                    if (this.soldier.Att.PosList.ContainsKey(i + 1))
                    {
                        CommonFunction.SetSpriteName(STP_back, CommonFunction.EquipBackSpriteName(this.soldier.Att.PosList[i + 1]));
                        STP_back.gameObject.SetActive(true);
                    }
                }
                UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
                if (Level)
                {
                    Level.gameObject.SetActive(false);
                }

                if (SPT_Quality)
                {
                    SPT_Quality.gameObject.SetActive(false);
                }
                UILabel Lbl_Label = TC_equip.transform.FindChild("Label").gameObject.GetComponent<UILabel>();
                if (Lbl_Label)
                {
                    Lbl_Label.text = "";
                }
                GameObject check = TC_equip.transform.FindChild("check").gameObject;
                if (check)
                {
                    check.SetActive(false);
                }
                UIGrid Grid_star = TC_equip.transform.FindChild("StarLevelGroup").gameObject.GetComponent<UIGrid>();
                if (Grid_star)
                {
                    Grid_star.gameObject.SetActive(false);
                }
                GameObject canEquip = TC_equip.transform.FindChild("back/canEquip").gameObject;
                if (canEquip)
                {
                    if (father.isSoldier)
                    {
                        if (
                                PlayerData.Instance._SoldierEquip.getWeaponList
                                (
                                           (wp) =>
                                           {
                                               if (count == -1)
                                                   return false;
                                               if (this.soldier._equippedDepot._EquiptList.Count >= count)
                                               {
                                                   if (this.soldier._equippedDepot._EquiptList[count] != null)
                                                   {
                                                       if (wp == this.soldier._equippedDepot._EquiptList[count])
                                                           return false;
                                                   }
                                               }
                                               if (wp.isEquiped)
                                                   return false;
                                               if (this.soldier.Att.PosList[count + 1] != (int)wp.Att.type)
                                                   return false;
                                               return this.soldier.Level >= (int)wp.Att.levelLimit;
                                           }

                                ).Count > 0
                          )
                        {
                            canEquip.SetActive(true);
                            if (CommonFunction.IsAlreadyBattle(this.soldier.uId) && this.equipPrompt != null)
                                this.equipPrompt.gameObject.SetActive(true);
                        }
                        else
                        {
                            canEquip.SetActive(false);
                        }
                    }
                    else
                    {
                        canEquip.SetActive(false);
                    }
                }
                GameObject CanChange = TC_equip.transform.FindChild("CanChange").gameObject;
                if (CanChange)
                {
                    CanChange.SetActive(false);
                }
            }
        }
    }

    public void UpdateSuitEffect(Dictionary<uint, Color> dic)
    {
        for (int j = 0; j < 6; ++j)
        {
            GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", j)).gameObject;
            if (TC_equip == null)
                continue;
            GameObject go = TC_equip.transform.FindChild("SuitEffect").gameObject;
            UISprite sprite = go.GetComponent<UISprite>();
            TweenScale scale = go.GetComponent<TweenScale>();
            sprite.enabled = false;
            scale.enabled = false;
            if (soldier == null)
                continue;
            Weapon tp = soldier._equippedDepot._EquiptList[j];
            if (tp == null)
            {
                continue;
            }
            if (dic == null)
                continue;
            if (dic.ContainsKey(tp.Att.CoordID))
            {
                sprite.enabled = true;
                scale.enabled = true;
                sprite.color = dic[tp.Att.CoordID];
            }
        }
    }

    public void InitTaskItem()
    {
        if (this.soldier == null) return;
        //this.soldier._skillsDepot.Sort();
        tempList = this.soldier._skillsDepot._skillsList;
        this.SkillPrompt();
        Main.Instance.StartCoroutine(CreatTaskItem(tempList));
    }
    private void SkillPrompt()
    {
        this.skillPrompt.gameObject.SetActive(false);
        if (father == null || father.isSoldier)
        {
            foreach (Skill skill in this.soldier._skillsDepot._skillsList)
            {
                if ((skill.enableStrong() == SkillCheck.Ok) && (CommonFunction.IsAlreadyBattle(this.soldier.uId)))
                {
                    this.skillPrompt.gameObject.SetActive(true);
                    break;
                }
            }
        }
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierSkill))
            this.skillPrompt.gameObject.SetActive(false);
    }
    private IEnumerator CreatTaskItem(List<Skill> _data)
    {
        yield return 0;
        if (_data.Count == 1 || _data.Count > SkillMaxCount)
        {
            this.ScrollView.ResetPosition();
            yield return 0;
        }
        yield return 0;
        int count = _data.Count;
        int itemCount = _skillItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContent.minIndex = -index;
        WrapContent.maxIndex = 0;

        if (count > SkillMaxCount)
        {
            WrapContent.enabled = true;
            count = SkillMaxCount;
        }
        else
        {
            WrapContent.enabled = false;
        }
        if (itemCount > count)
        {
            for (int i = count; i < itemCount; i++)
            {
                _skillItemList[i].gameObject.SetActive(false);
            }
        }
        for (int i = 0; i < count; i++)
        {
            if (itemCount <= i)
            {
                GameObject vGo = CommonFunction.InstantiateObject(Item, Grd_Grid.transform);
                SkillItemComponent item = vGo.GetComponent<SkillItemComponent>();
                if (item == null)
                {
                    item = vGo.AddComponent<SkillItemComponent>();
                    item.isSoldier = true;
                    item.MyStart(vGo);
                }
                _skillItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                if (father.isSoldier)
                    _skillItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _skillItemList[i].gameObject.SetActive(true);
            }
            _skillItemList[i].SetInfo(_data[i]);
        }
        if (count != 1)
            WrapContent.ReGetChild();
        Grd_Grid.repositionNow = true;
        yield return 0;
        ScrollView.ResetPosition();
        Grd_Grid.gameObject.SetActive(false);
        Grd_Grid.gameObject.SetActive(true);
    }
    public void SetTaskInfo(GameObject go, int wrapIndex, int realIndex)
    {
        if (realIndex >= tempList.Count)
        {
            go.SetActive(false);
            return;
        }
        else
        {
            go.SetActive(true);
        }
        SkillItemComponent item = _skillItemList[wrapIndex];
        item.SetInfo(tempList[realIndex]);

    }
    public void OnItemTouch(SkillItemComponent comp)
    {
        if (!father.isSoldier)
            return;

        if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierSkill))
        {
            return;
        }

        Skill temp = tempList.Find((sk) => { if (sk == null)return false; return sk.Att.nId == comp.id; });
        if (temp == null)
            return;

        skillIntensifyPanel.setInfo(temp);
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
        return;
    }
    public void BtnEventBinding()
    {
        WrapContent.onInitializeItem = SetTaskInfo;
    }
}