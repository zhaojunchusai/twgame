using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;

/// <summary>
/// 战斗界面演示-PVE
/// </summary>
public class FightViewPanel_PVE : Singleton<FightViewPanel_PVE>
{
    /// <summary>
    /// 最大技能数量
    /// </summary>
    private const int MAX_ITEM_COUNT_SKILL = 3;
    /// <summary>
    /// 最大武将数量
    /// </summary>
    private const int MAX_ITEM_COUNT_SOLDIER = 6;


    /// <summary>
    /// 战斗界面物件
    /// </summary>
    private FightView fightView;
    /// <summary>
    /// 战斗界面
    /// </summary>
    private FightViewController fightViewController;
    /// <summary>
    /// 战斗类型
    /// </summary>
    private EFightType fightType;
    /// <summary>
    /// 技能列表
    /// </summary>
    public List<FightViewItem_Skill> skillItemList = new List<FightViewItem_Skill>();
    /// <summary>
    /// 宠物技能
    /// </summary>
    public FightViewItem_Skill petSkill = null;
    /// <summary>
    /// 士兵列表
    /// </summary>
    public List<FightViewItem_Soldier> soldierItemList = new List<FightViewItem_Soldier>();

    private EFightViewSkillStatus _ItemStatus;
    private EFightViewSkillStatus _PreStatus;
    private float showCDTime;
    private float curFightSpeed;
    private float _ClickTime;
    private float _CDTime;

    public EFightViewSkillStatus Get_ItemStatus
    {
        get {
            return _ItemStatus;
        }
    }


    public void Initialize(FightView vFightView)
    {
        fightView = vFightView;
        fightViewController = UISystem.Instance.FightView;
        InitPanel();
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.AddSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }

    public void Uninitialize()
    {
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetPause, CommandEvent_FightSetPause);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightSetResume, CommandEvent_FightSetResume);
        CommandManager.Instance.DelSingleCommand(MessageID.Message_Fight.FM_FightChangeSpeed, CommandEvent_ChangeSpeed);
    }

    public void Destroy()
    {
        fightView = null;
        fightViewController = null;
        skillItemList.Clear();
        soldierItemList.Clear();
    }

    private void CommandEvent_FightSetPause(object vDataObj)
    {
        if (_ItemStatus != EFightViewSkillStatus.eskPause)
        {
            _PreStatus = _ItemStatus;
            _ItemStatus = EFightViewSkillStatus.eskPause;
        }
    }
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        if (_ItemStatus != EFightViewSkillStatus.eskPause)
            return;
        _ItemStatus = _PreStatus;
        _ClickTime = Time.time;
    }
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        curFightSpeed = (float)vDataObj;
    }

    /// <summary>
    /// 初始化界面
    /// </summary>
    public void InitPanel()
    {
        if (fightView == null)
            return;

        fightView.Lbl_Energy_Title.text = string.Empty;
        fightView.UIProgressBar_PVE_Energy.value = 0;
        fightView.UIProgressBar_PVE_Energy.gameObject.SetActive(true);

        fightView.Lbl_Magic_Title.text = string.Empty;
        fightView.UIProgressBar_PVE_Magic.value = 0;
        fightView.Spt_PetSkill_Icon.gameObject.SetActive(false);
        fightView.Spt_PetSkill_Slider.fillAmount = 1;
        _ItemStatus = EFightViewSkillStatus.eskNormal;
        _PreStatus = EFightViewSkillStatus.eskNormal;
        _ClickTime = 0;
        _CDTime = 0;
        fightView.Spt_PetSkill_Mask.fillAmount = 0;
        fightView.Spt_PetSkill_Mask.gameObject.SetActive(true);

        for (int i = 0; i < MAX_ITEM_COUNT_SKILL; i++)
        {
            if (i < skillItemList.Count)
            {
                skillItemList[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Skill_Item.gameObject, fightView.Obj_PVE_Skill);
                if (tmpObj == null)
                    continue;
                FightViewItem_Skill tmpSkill = tmpObj.AddComponent<FightViewItem_Skill>();
                tmpSkill.Initialize();
                tmpSkill.transform.localPosition = new Vector3(118 * (i - 1), 1, 0);
                tmpSkill.transform.localScale = Vector3.one;
                tmpSkill.name = string.Format("Skill_{0}", i);
                tmpSkill.gameObject.SetActive(true);
                skillItemList.Add(tmpSkill);
            }
        }

        for (int i = 0; i < MAX_ITEM_COUNT_SOLDIER; i++)
        {
            if (i < soldierItemList.Count)
            {
                soldierItemList[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Soldier_Item.gameObject, fightView.Obj_PVE_Soldier);
                if (tmpObj == null)
                    continue;
                FightViewItem_Soldier tmpSoldier = tmpObj.AddComponent<FightViewItem_Soldier>();
                tmpSoldier.Initialize();
                tmpSoldier.transform.localPosition = new Vector3(i * 105 - 300, 0, 0);
                tmpSoldier.transform.localScale = Vector3.one;
                tmpSoldier.name = string.Format("Soldier_{0}", i);
                tmpSoldier.gameObject.SetActive(true);
                soldierItemList.Add(tmpSoldier);
            }
        }

        fightView.Obj_PVE.gameObject.SetActive(false);
    }

    /// <summary>
    /// 刷新界面
    /// </summary>
    /// <param name="vEquipList"></param>
    /// <param name="vSoldierInfoList"></param>
    public void RefreshPanel(EFightType vFightType, List<Weapon> vEquipList = null, List<FightSoldierInfo> vSoldierInfoList = null)
    {
        fightType = vFightType;

        if ((UISystem.Instance.FightView.pPetData != null) && (UISystem.Instance.FightView.pPetData.Skill != null) && (UISystem.Instance.FightView.pPetData.Skill.Att != null))
        {
            fightView.Btn_PVE_PetSkill.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(fightView.Spt_PetSkill_Icon, UISystem.Instance.FightView.pPetData.Skill.Att.Icon);
            fightView.Spt_PetSkill_Icon.gameObject.SetActive(true);
            _CDTime = CommonFunction.GetSecondTimeByMilliSecond(UISystem.Instance.FightView.pPetData.Skill.Att.waitTime);
        }

        Refresh_Energy();
        Refresh_Magic();
        Refresh_PetPower();
        Refresh_Skill(vEquipList);
        Refresh_Soldier(vSoldierInfoList);
        fightView.Obj_PVE.gameObject.SetActive(true);
    }
    
    /// <summary>
    /// 刷新能量
    /// </summary>
    public void Refresh_Energy()
    {
        if (fightView == null)
            return;
        if (fightViewController == null)
            return;
        if ((fightType == EFightType.eftExpedition) || (fightType == EFightType.eftCaptureTerritory) || (fightType == EFightType.eftCrossServerWar))
            fightView.UIProgressBar_PVE_Energy.gameObject.SetActive(false);
        else
            fightView.UIProgressBar_PVE_Energy.gameObject.SetActive(true);
        fightView.Lbl_Energy_Title.text = string.Format("{0}/{1}", fightViewController.Get_CurValue_Energy, fightViewController.Get_MAXValue_Energy);
        fightView.UIProgressBar_PVE_Energy.value = (float)fightViewController.Get_CurValue_Energy / fightViewController.Get_MAXValue_Energy;
    }

    /// <summary>
    /// 刷新魔法
    /// </summary>
    public void Refresh_Magic()
    {
        if (fightView == null)
            return;
        if (fightViewController == null)
            return;
        fightView.Lbl_Magic_Title.text = string.Format("{0}/{1}", fightViewController.Get_CurValue_Magic, fightViewController.Get_MAXValue_Magic);
        fightView.UIProgressBar_PVE_Magic.value = (float)fightViewController.Get_CurValue_Magic / fightViewController.Get_MAXValue_Magic;        
    }

    /// <summary>
    /// 刷新宠物技能怒气点
    /// </summary>
    public void Refresh_PetPower()
    {
        ShowPetSkillEffect(false);
        if (fightView == null)
            return;
        if (fightViewController == null)
            return;
        fightView.Spt_PetSkill_Slider.fillAmount = (float)fightViewController.Get_CurValue_PetPower / fightViewController.Get_MAXValue_PetPower;
        if (fightView.Spt_PetSkill_Slider.fillAmount >= 1)
        {
            //演示特效//
            ShowPetSkillEffect(true);
        }
    }

    /// <summary>
    /// 重新预加载武将技能特效
    /// </summary>
    public void Refresh_EndlessSoldierSkill()
    {
        if (soldierItemList == null)
            return;
        for (int i = 0; i < soldierItemList.Count; i++)
        {
            if (soldierItemList[i] != null)
            {
                //soldierItemList[i].InitStatus();
                soldierItemList[i].PreLoadResource_Skill();
                soldierItemList[i].PreLoadResource_Trajectory();
            }
        }
    }
    public void Refresh_EndlessHeroSkill()
    {
        if (skillItemList != null)
        {
            for (int i = 0; i < skillItemList.Count; i++)
            {
                if (skillItemList[i] != null)
                {
                    skillItemList[i].PreLoadResource_Skill();
                }
            }
        }
    }
    /// <summary>
    /// 刷新技能
    /// </summary>
    /// <param name="vSkillIDList"></param>
    private void Refresh_Skill(List<Weapon> vSkillIDList)
    {
        if (vSkillIDList == null)
            return;

        for (int i = 0; i < skillItemList.Count; i++)
        {
            skillItemList[i].InitStatus();
            if (i >= vSkillIDList.Count)
                continue;
            if (vSkillIDList[i] == null)
                continue;
            if (vSkillIDList[i].Att == null)
                continue;
            SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vSkillIDList[i].Att.skillID);
            if (tmpSkillInfo == null)
                continue;
            skillItemList[i].RefreshInfo_ShowStatus(new SkillBaseData(tmpSkillInfo.nId, tmpSkillInfo.Icon, tmpSkillInfo.expendMagic,
                tmpSkillInfo.expendMagic, CommonFunction.GetSecondTimeByMilliSecond(tmpSkillInfo.waitTime), vSkillIDList[i]));
        }
    }

    /// <summary>
    /// 刷新武将
    /// </summary>
    /// <param name="vSoldierInfoList"></param>
    private void Refresh_Soldier(List<FightSoldierInfo> vSoldierInfoList)
    {
        if (vSoldierInfoList == null)
            return;
        for (int i = 0; i < soldierItemList.Count; i++)
        {
            soldierItemList[i].InitStatus();
            if (i >= vSoldierInfoList.Count)
                continue;
            soldierItemList[i].RefreshInfo_ShowStatus(vSoldierInfoList[i].mSoldier, EFightCamp.efcSelf, vSoldierInfoList[i].mNum);
        }
    }

    /// <summary>
    /// 宠物技能特效提示
    /// </summary>
    /// <param name="vIsShow"></param>
    private void ShowPetSkillEffect(bool vIsShow)
    {
        if (fightView != null)
        {
            if (fightView.Obj_PetSkill_Effect != null)
            {
                fightView.Obj_PetSkill_Effect.SetActive(vIsShow);
            }
        }
    }

    /// <summary>
    /// 显示宠物技能CD
    /// </summary>
    private void ShowPetSkillCD()
    {
        if (_ItemStatus != EFightViewSkillStatus.eskCD)
            return;

        showCDTime += curFightSpeed * (Time.time - _ClickTime);
        _ClickTime = Time.time;
        float tmpValue = (_CDTime - showCDTime) / _CDTime;
        if (tmpValue <= 0)
        {
            tmpValue = 0;
            Scheduler.Instance.RemoveUpdator(ShowPetSkillCD);
            _ItemStatus = EFightViewSkillStatus.eskNormal;
        }

        if ((fightView != null) && (fightView.Spt_PetSkill_Mask != null))
            fightView.Spt_PetSkill_Mask.fillAmount = tmpValue;
    }
    public void ClickPetSkillOperate()
    {
        _ItemStatus = EFightViewSkillStatus.eskCD;
        _ClickTime = Time.time;
        showCDTime = 0;
        curFightSpeed = SceneManager.Instance.Get_FightSpeed;
        Scheduler.Instance.AddUpdator(ShowPetSkillCD);
    }
}
