using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class RightPanel
{
    public GameObject _uiRoot;
    public EquipChoosePanel choosePanel;
    public HESKIntensifyPanel skillIntensifyPanel;
    public UILabel PowerLabel;

    private UIWrapContent WrapContent;
    public UIScrollView ScrollView;

    public UIGrid Grd_Grid;
    public GameObject Item;
    public UISprite equipPrompt;
    public UISprite skillPrompt;
    public UIToggle toggleSkill;
    public UIToggle toggle;

    private List<SkillItemComponent> _skillItemList = new List<SkillItemComponent>();

    private List<Skill> tempList;
    public bool NeedInitSkill = false;
    public bool isSkill = false;
    public void init(GameObject _uiRoot)
    {
        this._uiRoot = _uiRoot;
        Grd_Grid = this._uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIGrid>();
        Item = this._uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/item").gameObject;
        PowerLabel = this._uiRoot.transform.FindChild("Anim/center/fighting/Label_Fighting").gameObject.GetComponent<UILabel>();
        WrapContent = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView/Grid").gameObject.GetComponent<UIWrapContent>();
        ScrollView = _uiRoot.transform.FindChild("Anim/right/EquipAndSkillScrollView").gameObject.GetComponent<UIScrollView>();
        toggle = this._uiRoot.transform.FindChild("Anim/right/TabEquip").gameObject.GetComponent<UIToggle>();
        toggleSkill = this._uiRoot.transform.FindChild("Anim/right/TabSkill").gameObject.GetComponent<UIToggle>();
        equipPrompt = this._uiRoot.transform.FindChild("Anim/right/TabEquip/prompt").gameObject.GetComponent<UISprite>();
        skillPrompt = this._uiRoot.transform.FindChild("Anim/right/TabSkill/prompt").gameObject.GetComponent<UISprite>();
        equipPrompt.gameObject.SetActive(false);
        skillPrompt.gameObject.SetActive(false);
        toggle.Set(true);
        EventDelegate.Add(toggle.onChange, Toggle);

        if (skillIntensifyPanel == null)
        {
            skillIntensifyPanel = new HESKIntensifyPanel();
        }
        skillIntensifyPanel.init(_uiRoot);
        PowerLabel.text = PlayerData.Instance._Attribute.CombatPower.ToString();

        SetEquiped();
        InitTaskItem();
        BtnEventBinding();
        PlayerData.Instance._SkillsDepot.SkillsDepotEvent += OnSkillEvent;
        PlayerData.Instance._SkillsDepot.SkillsErrorEvent += _SkillsDepot_SkillsErrorEvent;
        Main.Instance.StartCoroutine(EquipGray());
    }
    private IEnumerator EquipGray()
    {
        yield return 0;
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
        {
            for (int i = 0; i < 8; ++i)
            {
                int count = i;
                GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", i)).gameObject;
                GameObject check = TC_equip.transform.FindChild("check").gameObject;
                if (check)
                    check.SetActive(false);
                this.equipPrompt.gameObject.SetActive(false);
                this._uiRoot.SetActive(false);
                this._uiRoot.SetActive(true);
                CommonFunction.SetGameObjectGray(TC_equip.gameObject, true);
            }
        }
        else
        {
            for (int i = 0; i < 8; ++i)
            {
                int count = i;
                GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", i)).gameObject;
                this._uiRoot.SetActive(false);
                this._uiRoot.SetActive(true);
                CommonFunction.SetGameObjectGray(TC_equip.gameObject, false);
            }
        }
    }
    void _SkillsDepot_SkillsErrorEvent(SkillControl control, int errorCode)
    {
        if(errorCode == 0)
        {
            if(control == SkillControl.UpgradeSkillResp)
            {
                GuideManager.Instance.CheckTrigger(GuideTrigger.HeroSkillUpSucceed);                
            }
        }
    }
    public IEnumerator SetToggle(bool left)
    {
        yield return 0;
        if (left)
        {
            toggle.Set(left);
        }
        else
        {
            this.toggleSkill.Set(!left);
        }
    }
    public void Toggle()
    {
        bool val = UIToggle.current.value;
        if (val)
        {
            isSkill = false;
        }
        else
        {
            this.InitSkill();
            isSkill = true;
            GuideManager.Instance.CheckTrigger(GuideTrigger.OpenHeroSkillTab);
        }

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
    }

    public void OnSkillEvent(SkillChange change, int index, UInt64 uID)
    {
        PowerLabel.text = PlayerData.Instance._Attribute.CombatPower.ToString();
        InitTaskItem();
        if (skillIntensifyPanel.root.activeSelf)
        {
            if (skillIntensifyPanel.infoSkill.uId == uID)
            {
                Skill p = PlayerData.Instance._SkillsDepot.FindByUid(uID);
                if (p != null)
                    skillIntensifyPanel.setInfo(p);
            }
        }
    }
    public void BeforClose()
    {
        if (PlayerData.Instance._SkillsDepot != null) 
        {
            PlayerData.Instance._SkillsDepot.SkillsDepotEvent -= OnSkillEvent;
            PlayerData.Instance._SkillsDepot.SkillsErrorEvent -= _SkillsDepot_SkillsErrorEvent;
        }
        
    }
    public void SetEquiped()
    {
        var playerAtt = PlayerData.Instance;
        this.equipPrompt.gameObject.SetActive(false);
        for (int i = 0; i < 8; ++i)
        {
            int count = i;
            GameObject TC_equip = _uiRoot.transform.FindChild(String.Format("Anim/right/Equip/equipt{0}", i)).gameObject;
            UISprite SPT_Quality = TC_equip.transform.FindChild("quality").GetComponent<UISprite>();
            Weapon tp = PlayerData.Instance._ArtifactedDepot._EquiptList[i];

            UIEventListener.Get(TC_equip).onClick = (go) =>
            {
                //装备点击事件
                if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify,true))
                {
                    //OpenLevelData tmpData = ConfigManager.Instance.mOpenLevelConfig.GetDataByType(OpenFunctionType.ArtifactIntensify);
                    //if (tmpData.gateId != -1)
                    //{
                    //    StageInfo info = ConfigManager.Instance.mStageData.GetInfoByID((uint)tmpData.gateId);
                    //    if (info != null)
                    //    {
                    //        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.BACKPACK_GATELOCKTIP, info.GateSequence));
                    //    }
                    //}
                    return;
                }
                int index = count;
                if (tp == null)
                {
                    choosePanel = EquipChoosePanel.create();
                    choosePanel.SetParent(_uiRoot.transform);
                    choosePanel.view.InitWeaponItem(index);
                    //UISystem.Instance.ResortViewOrder();

                    choosePanel.view._uiRoot.SetActive(true);
                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenGodWeaponChange);

                }
                else
                {
                    UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_EQUIPDETAILINFO);
                    UISystem.Instance.EquipDetailInfoView.UpdateViewInfo(tp,1);

                    GuideManager.Instance.CheckTrigger(GuideTrigger.OpenGodWeaponDesc);

                }
                CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, go.transform));
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
                UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
                if (Level)
                {
                    Level.gameObject.SetActive(true);
                }

                UILabel Lbl_Label = TC_equip.transform.FindChild("Label").GetComponent<UILabel>();
                if (Lbl_Label)
                {
                    Lbl_Label.text = tp.Lv.ToString();
                }
                GameObject check = TC_equip.transform.FindChild("check").gameObject;
                if(check)
                {
                    if(tp.enableStrong() == WeaponCheck.Ok || tp.enableStrong() == WeaponCheck.Level)
                    {
                        check.SetActive(true);
                        this.equipPrompt.gameObject.SetActive(true);
                    }
                    else
                    {
                        check.SetActive(false);
                    }
                    if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
                    {
                        check.SetActive(false);
                        this.equipPrompt.gameObject.SetActive(false);
                    }
                }
                GameObject canEquip = TC_equip.transform.FindChild("CanEquip").gameObject;
                if (canEquip)
                {
                    canEquip.SetActive(false);
                }

                if (SPT_Quality)
                {
                    CommonFunction.SetQualitySprite(SPT_Quality, tp.Att.quality, STP_back);
                    SPT_Quality.gameObject.SetActive(true);
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
                    CommonFunction.SetSpriteName(STP_back,CommonFunction.EquipBackSpriteName(EquptedName.DIR_EQUPTED_NAME[i]));
                    STP_back.gameObject.SetActive(true);
                }
                UISprite Level = TC_equip.transform.FindChild("Lv").gameObject.GetComponent<UISprite>();
                if (Level)
                {
                    Level.gameObject.SetActive(false);
                }

                UILabel Lbl_Label = TC_equip.transform.FindChild("Label").GetComponent<UILabel>();
                if (Lbl_Label)
                {
                    Lbl_Label.text = "";
                }
                GameObject check = TC_equip.transform.FindChild("check").gameObject;
                if (check)
                {
                    check.SetActive(false);
                }
                GameObject canEquip = TC_equip.transform.FindChild("CanEquip").gameObject;
                if(canEquip)
                {
                    if (PlayerData.Instance._ArtifactedDepot.IsHadEquipCanEquip(i))
                        canEquip.SetActive(true);
                    else
                        canEquip.SetActive(false);
                }
                if (SPT_Quality)
                {
                    SPT_Quality.gameObject.SetActive(false);
                }
            }
        }
    }
    public void InitTaskItem()
    {
        this.NeedInitSkill = true;
        if (this.skillPrompt != null)
            this.skillPrompt.gameObject.SetActive(false);
        foreach (Skill skill in PlayerData.Instance._SkillsDepot._skillsList)
        {
            if (skill.enableStrong() == SkillCheck.Ok && CommonFunction.CheckIsOpen(OpenFunctionType.HeroSkillControl))
            {
                if (this.skillPrompt != null)
                    this.skillPrompt.gameObject.SetActive(true);
                break;
            }
        }
        if (isSkill)
            InitSkill();
    }
    public void InitSkill()
    {
        if (!this.NeedInitSkill)
            return;
        PlayerData.Instance._SkillsDepot.Sort();
        tempList = new List<Skill>(PlayerData.Instance._SkillsDepot._skillsList);
        tempList.AddRange(PlayerData.Instance._SkillsDepot.GetUnLockSkill());
        this.NeedInitSkill = false;
        Main.Instance.StartCoroutine(CreatTaskItem(tempList));
    }
    private IEnumerator CreatTaskItem(List<Skill> _data)
    {
        yield return 0.5;
        int count = _data.Count;
        int itemCount = _skillItemList.Count;

        int index = Mathf.CeilToInt((float)count / WrapContent.wideCount) - 1;
        if (index == 0)
            index = 1;
        WrapContent.minIndex = -index;
        WrapContent.maxIndex = 0;

        if (count > 5)
        {
            WrapContent.enabled = true;
            count = 5;
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
                    item.MyStart(vGo);
                }
                _skillItemList.Insert(i, item);
                vGo.name = i.ToString();
                vGo.SetActive(true);
                _skillItemList[i].TouchEvent += OnItemTouch;
            }
            else
            {
                _skillItemList[i].gameObject.SetActive(true);
            }
            _skillItemList[i].SetInfo(_data[i]);
        }

        WrapContent.ReGetChild();
        Grd_Grid.repositionNow = true;
        yield return 0;
        ScrollView.ResetPosition();
        yield return 0.5;
        Grd_Grid.repositionNow = true;
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
        Skill temp = tempList.Find((sk) => { if (sk == null)return false; return sk.Att.nId == comp.id; });
        if (temp == null)
            return;
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.HeroSkillControl, true))
            return;

        if(temp.islock)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.THIS_IS_UNLOCK);
            return;
        }
        skillIntensifyPanel.setInfo(temp);
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenHeroSkillDetail);

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, this._uiRoot.transform));
        return;
    }
    public void BtnEventBinding()
    {
        WrapContent.onInitializeItem = SetTaskInfo;
    }

}

