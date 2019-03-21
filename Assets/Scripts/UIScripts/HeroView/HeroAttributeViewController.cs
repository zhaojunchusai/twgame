using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
public class HeroAttributeViewController : UIBase
{
    public HeroAttributeView view;
    public RightPanel rightPanel;
    public CenterPanel centerPanel;
    WeaponsDepot weaponsDepot;
    int filterType = -1;
    public override void Initialize()
    {
        weaponsDepot = PlayerData.Instance._WeaponDepot;
        if (view == null)
            view = new HeroAttributeView();
        view.weaponsDepot = this.weaponsDepot;
        view.Initialize();

        if (rightPanel == null)
        {
            rightPanel = new RightPanel();
        }
        rightPanel.init(view._uiRoot);

        if (centerPanel == null)
        {
            centerPanel = new CenterPanel();
        }
        centerPanel.init(view._uiRoot);

        BtnEventBinding();
        SetTowerIcon();
        PlayerData.Instance._WeaponDepot.WeaponDepotEvent += OnEquipedDepotEvent;
        PlayerData.Instance._WeaponDepot.ErrotDeleteEvent += _WeaponDepot_ErrotDeleteEvent;
        PlayerData.Instance._SkillsDepot.SkillsErrorEvent += _SkillsDepot_SkillsErrorEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent += Instance_UpdatePlayerItemsEvent;
        // PlayOpenAnim();       
        PlayerData.Instance.UpdateAttributeEvent += Instance_UpdateAttributeEvent;
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_OpenL, view._uiRoot.transform.parent.transform));
        GuideManager.Instance.CheckTrigger(GuideTrigger.OpenHeroView);
    }

    public override void ReturnTop()
    {
        base.ReturnTop();
        UISystem.Instance.RefreshUIToTop(HeroAttributeView.UIName);
    }

    void Instance_UpdatePlayerItemsEvent()
    {
        if (this.rightPanel.skillIntensifyPanel.root.gameObject.activeSelf)
        {
            this.rightPanel.skillIntensifyPanel.RefreshMaterial();
        }
        if (this.rightPanel != null)
        {
            rightPanel.SetEquiped();
        }
    }

    void Instance_UpdatePlayerGoldEvent()
    {
        if (this.rightPanel.skillIntensifyPanel.root.gameObject.activeSelf)
        {
            this.rightPanel.skillIntensifyPanel.RefreshCost();
        }
        if (this.rightPanel != null)
        {
            rightPanel.SetEquiped();
        }
    }

    void Instance_UpdateAttributeEvent()
    {
        if (this.centerPanel != null)
            this.centerPanel._setAtt();
    }

    void _SkillsDepot_SkillsErrorEvent(SkillControl control, int errorCode)
    {
        //技能操作回馈
    }

    void _WeaponDepot_ErrotDeleteEvent(EquipControl control, int errorCode)
    {
        //神器操作回馈
        if (control == EquipControl.PromoteEquipAndArtifactResp && errorCode == 0)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.GodWeaponStrengthenSucceed);
        }
        if (control == EquipControl.PutonEquipAndArtifactResp && errorCode == 0)
        {
            GuideManager.Instance.CheckTrigger(GuideTrigger.GodWeaponEquipSucceed);
        }
    }
    ~HeroAttributeViewController()
    {

    }
    public void SetToggle(bool left)
    {
        Main.Instance.StartCoroutine(this.rightPanel.SetToggle(left));
    }

    public bool Filter(Weapon wp)
    {
        if (filterType >= 8) return false;
        return wp.Att.type == EquptedName.DIR_EQUPTED_NAME[filterType];
    }
    public void setWeaponsDepot(WeaponsDepot p)
    {
        weaponsDepot = p;
    }
    public void OnEquipedDepotEvent(WeaponChange change, int index = -1, UInt64 uID = 0)
    {
        //if (change != WeaponChange.EquiptList) return;

        rightPanel.SetEquiped();
        centerPanel._setAtt();
        rightPanel.PowerLabel.text = PlayerData.Instance._Attribute.CombatPower.ToString();
    }

    /// <summary>
    /// 城堡升级按钮
    /// </summary>
    /// <param name="btn"></param>
    public void ButtonEvent_Button_Castle(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        //CastleModule.Instance.SendPutonEquipReq(PlayerData.Instance._AccountID);
        UISystem.Instance.CloseGameUI(ViewType.DIR_VIEWNAME_HEROATT);
        UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_CASTLEVIEW);

    }
    public void ButtonEvent_Button_close(GameObject btn)
    {
        //英雄界面关闭按钮
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));
        Close(null, null);
        //UISystem.Instance.CloseGameUI(view.UIName);
        //view._uiRoot.SetActive(false);
    }
    public void OneqInfoPanel_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        //装备信息界面关闭
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseGodWeaponDetail);

    }
    public void OnskillOnekeyintensify_Button_intensify(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        //技能升级界面升级按钮
        if (rightPanel.skillIntensifyPanel.infoSkill == null) return;
        SkillCheck result = PlayerData.Instance._SkillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot,false);

        if (result == SkillCheck.Ok)
        {
            PlayerData.Instance._SkillsDepot.AutoStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot);
        }
        else
        {
            if (result == SkillCheck.Money)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                return;
            }

            if (result == SkillCheck.HeroLevelLower)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.HERO_LV_ONTENOUGH, Mathf.CeilToInt((rightPanel.skillIntensifyPanel.infoSkill.Level + 1) * rightPanel.skillIntensifyPanel.infoSkill.StrongCoefficient())));
                return;
            }

            ErrorCode.ShowErrorTip((uint)PlayerData.Instance._SkillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot));
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

        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Btn_Click, view._uiRoot.transform.parent.transform));

        //技能升级界面升级按钮
        if (rightPanel.skillIntensifyPanel.infoSkill == null) return;
        if (rightPanel.skillIntensifyPanel.QuickMatItem == null)return;
        rightPanel.skillIntensifyPanel.QuickMatItem.ShowTip(OnskillIntensifyPanel_Button_intensify_continue);        
    }

    /// <summary>
    /// 由于插入了补全材料的确认面板
    /// 将原函数 OnskillIntensifyPanel_Button_intensify 中的一部分单独成这个函数
    /// </summary>
    private void OnskillIntensifyPanel_Button_intensify_continue()
    {
        SkillCheck result = PlayerData.Instance._SkillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot,false);
        if (result == SkillCheck.Ok)
        {
            PlayerData.Instance._SkillsDepot.Strong(rightPanel.skillIntensifyPanel.infoSkill.Slot);
        }
        else
        {
            if (result == SkillCheck.Money)
            {
                UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW);
                return;
            }

            if (result == SkillCheck.HeroLevelLower)
            {
                UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.HERO_LV_ONTENOUGH, Mathf.CeilToInt((rightPanel.skillIntensifyPanel.infoSkill.Level + 1) * rightPanel.skillIntensifyPanel.infoSkill.StrongCoefficient())));
                return;
            }

            ErrorCode.ShowErrorTip((uint)PlayerData.Instance._SkillsDepot.TextStrong(rightPanel.skillIntensifyPanel.infoSkill.Slot));
        }
    }
    public void OnskillIntensifyPanel_Button_close(GameObject btn)
    {
        CommandManager.Instance.SendSingleCommand(MessageID.Message_Sound.SM_PlayAudio, new ShowAudioInfo(GlobalConst.Sound.AUDIO_UI_Close, view._uiRoot.transform.parent.transform));

        //技能升级界面关闭按钮
        rightPanel.skillIntensifyPanel.OnClose();
        GuideManager.Instance.CheckTrigger(GuideTrigger.CloseHeroSkillLvUp);

    }
    public void SetTowerIcon()
    {
        if (CommonFunction.GetCastleFileName() == string.Empty)
        {
            return;
        }
        ResourceLoadManager.Instance.LoadAloneImage(CommonFunction.GetCastleFileName(), (tx) =>
        {
            this.view.Texture_Tower.mainTexture = tx;
            this.view.Texture_Tower.MakePixelPerfect();
        });
    }
    public override UIBoundary GetUIBoundary()
    {
        return view.Boundary;
    }
    public override void Uninitialize()
    {
        //BtnUnRegist();
        PlayerData.Instance._WeaponDepot.WeaponDepotEvent -= OnEquipedDepotEvent;
        PlayerData.Instance._SkillsDepot.SkillsErrorEvent -= _SkillsDepot_SkillsErrorEvent;
        PlayerData.Instance.UpdateAttributeEvent -= Instance_UpdateAttributeEvent;
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent -= Instance_UpdatePlayerItemsEvent;
        view.Texture_Tower.mainTexture = null;
        rightPanel.BeforClose();
        centerPanel.OnClose();
    }
    public override void Destroy()
    {
        base.Destroy();
        view = null;
        centerPanel = null;
        rightPanel = null;

    }
    public void BtnEventBinding()
    {
        UIEventListener.Get(view.Btn_Button_close.gameObject).onClick = ButtonEvent_Button_close;
        UIEventListener.Get(view.Btn_Button_Castle.gameObject).onClick = ButtonEvent_Button_Castle;
        //技能强化界面
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Btn_Button_intensify.gameObject).onClick = OnskillIntensifyPanel_Button_intensify;
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Marsk).onClick = OnskillIntensifyPanel_Button_close;
        UIEventListener.Get(rightPanel.skillIntensifyPanel.Btn_Button_onekeyintensify.gameObject).onClick = OnskillOnekeyintensify_Button_intensify;

    }

    //界面动画
    //public void PlayOpenAnim()
    //{
    //    view.Anim_TScale.gameObject.transform.localScale =  GlobalConst.ViewScaleAnimVec;
    //    view.Anim_TScale.Restart();
    //    view.Anim_TScale.PlayForward();
    //}
    //public void PlayOpenEquipAnim()
    //{
    //    view.Equip_TScale.gameObject.transform.localScale =  GlobalConst.ViewScaleAnimVec;
    //    view.Equip_TScale.Restart(); 
    //    view.Equip_TScale.PlayForward();
    //}
    //public void PlayOpenArtifictAnim()
    //{
    //    view.Artifact_TScale.gameObject.transform.localScale =  GlobalConst.ViewScaleAnimVec;
    //    view.Artifact_TScale.Restart(); 
    //    view.Artifact_TScale.PlayForward();
    //}
    //public void PlayOpenSkillAnim()
    //{
    //    view.Skill_TScale.gameObject.transform.localScale =  GlobalConst.ViewScaleAnimVec;
    //    view.Skill_TScale.Restart(); 
    //    view.Skill_TScale.PlayForward();
    //}
}

