using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using fogs.proto.msg;
using Assets.Script.Common;

/// <summary>
/// 战斗界面演示-PVP
/// </summary>
public class FightViewPanel_PVP : Singleton<FightViewPanel_PVP>
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
    /// 技能列表-自己
    /// </summary>
    public List<FightViewItem_Skill> skillList_Self = new List<FightViewItem_Skill>();
    /// <summary>
    /// 宠物技能-自己
    /// </summary>
    public FightViewItem_Skill petSkill_Self = null;
    /// <summary>
    /// 士兵列表-自己
    /// </summary>
    private List<FightViewItem_Soldier> soldierList_Self = new List<FightViewItem_Soldier>();
    /// <summary>
    /// 技能列表-敌人
    /// </summary>
    public List<FightViewItem_Skill> skillList_Enemy = new List<FightViewItem_Skill>();
    /// <summary>
    /// 宠物技能-敌人
    /// </summary>
    public FightViewItem_Skill petSkill_Enemy = null;
    /// <summary>
    /// 士兵列表-敌人
    /// </summary>
    private List<FightViewItem_Soldier> soldierList_Enemy = new List<FightViewItem_Soldier>();



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
        skillList_Self.Clear();
        soldierList_Self.Clear();
        skillList_Enemy.Clear();
        soldierList_Enemy.Clear();
    }

    private void CommandEvent_FightSetPause(object vDataObj)
    {
        if (_ItemStatus[PETDATA_INDEX_SELF] != EFightViewSkillStatus.eskPause)
        {
            _PreStatus[PETDATA_INDEX_SELF] = _ItemStatus[PETDATA_INDEX_SELF];
            _ItemStatus[PETDATA_INDEX_SELF] = EFightViewSkillStatus.eskPause;
        }
        if (_ItemStatus[PETDATA_INDEX_ENEMY] != EFightViewSkillStatus.eskPause)
        {
            _PreStatus[PETDATA_INDEX_ENEMY] = _ItemStatus[PETDATA_INDEX_ENEMY];
            _ItemStatus[PETDATA_INDEX_ENEMY] = EFightViewSkillStatus.eskPause;
        }
    }
    private void CommandEvent_FightSetResume(object vDataObj)
    {
        if (_ItemStatus[PETDATA_INDEX_SELF] == EFightViewSkillStatus.eskPause)
        {
            _ItemStatus[PETDATA_INDEX_SELF] = _PreStatus[PETDATA_INDEX_SELF];
            _ClickTime[PETDATA_INDEX_SELF] = Time.time;
        }
        if (_ItemStatus[PETDATA_INDEX_ENEMY] == EFightViewSkillStatus.eskPause)
        {
            _ItemStatus[PETDATA_INDEX_ENEMY] = _PreStatus[PETDATA_INDEX_ENEMY];
            _ClickTime[PETDATA_INDEX_ENEMY] = Time.time;
        }
    }
    private void CommandEvent_ChangeSpeed(object vDataObj)
    {
        curFightSpeed = (float)vDataObj;
    }


    public void InitPanel()
    {
        if (fightView == null)
            return;

        fightView.Lbl_SMagic_Title.text = string.Empty;
        fightView.UIProgressBar_Self_Magic.value = 0;
        fightView.Spt_Self_PetSkill_Slider.fillAmount = 0;
        fightView.Spt_Self_PetSkill_Icon.gameObject.SetActive(false);
        _ItemStatus[PETDATA_INDEX_SELF] = EFightViewSkillStatus.eskNormal;
        _PreStatus[PETDATA_INDEX_SELF] = EFightViewSkillStatus.eskNormal;
        _ClickTime[PETDATA_INDEX_SELF] = 0;
        _CDTime[PETDATA_INDEX_SELF] = 0;
        fightView.Spt_Self_PetSkill_Mask.fillAmount = 0;
        fightView.Spt_Self_PetSkill_Mask.gameObject.SetActive(true);

        fightView.Lbl_EMagic_Title.text = string.Empty;
        fightView.UIProgressBar_Enemy_Magic.value = 0;
        fightView.Spt_Enemy_PetSkill_Slider.fillAmount = 0;
        fightView.Spt_Enemy_PetSkill_Icon.gameObject.SetActive(false);
        _ItemStatus[PETDATA_INDEX_ENEMY] = EFightViewSkillStatus.eskNormal;
        _PreStatus[PETDATA_INDEX_ENEMY] = EFightViewSkillStatus.eskNormal;
        _ClickTime[PETDATA_INDEX_ENEMY] = 0;
        _CDTime[PETDATA_INDEX_ENEMY] = 0;
        fightView.Spt_Enemy_PetSkill_Mask.fillAmount = 0;
        fightView.Spt_Enemy_PetSkill_Mask.gameObject.SetActive(true);

        for (int i = 0; i < MAX_ITEM_COUNT_SKILL; i++)
        {
            if (i < skillList_Self.Count)
            {
                skillList_Self[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Skill_Item.gameObject, fightView.Obj_PVP_Self_Skill);
                if (tmpObj == null)
                    continue;
                FightViewItem_Skill tmpSkill = tmpObj.AddComponent<FightViewItem_Skill>();
                tmpSkill.Initialize();
                tmpSkill.transform.localPosition = new Vector3(70 * i, 0, 0);
                tmpSkill.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                tmpSkill.name = string.Format("Skill_{0}", i);
                tmpSkill.gameObject.SetActive(true);
                skillList_Self.Add(tmpSkill);
            }

            if (i < skillList_Enemy.Count)
            {
                skillList_Enemy[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Skill_Item.gameObject, fightView.Obj_PVP_Enemy_Skill);
                if (tmpObj == null)
                    continue;
                FightViewItem_Skill tmpSkill = tmpObj.AddComponent<FightViewItem_Skill>();
                tmpSkill.Initialize();
                tmpSkill.transform.localPosition = new Vector3(-70 * i, 0, 0);
                tmpSkill.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f);
                tmpSkill.name = string.Format("Skill_{0}", i);
                tmpSkill.gameObject.SetActive(true);
                skillList_Enemy.Add(tmpSkill);
            }
        }

        for (int i = 0; i < MAX_ITEM_COUNT_SOLDIER; i++)
        {
            if (i < soldierList_Self.Count)
            {
                soldierList_Self[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Soldier_Item.gameObject, fightView.Obj_PVP_Self_Soldier);
                if (tmpObj == null)
                    continue;
                FightViewItem_Soldier tmpSoldier = tmpObj.AddComponent<FightViewItem_Soldier>();
                tmpSoldier.Initialize();
                tmpSoldier.transform.localPosition = new Vector3(85 * i, 0, 0);
                tmpSoldier.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                tmpSoldier.name = string.Format("Soldier_{0}", i);
                tmpSoldier.gameObject.SetActive(true);
                soldierList_Self.Add(tmpSoldier);
            }

            if (i < soldierList_Enemy.Count)
            {
                soldierList_Enemy[i].InitStatus();
            }
            else
            {
                GameObject tmpObj = CommonFunction.InstantiateObject(fightView.Obj_Soldier_Item.gameObject, fightView.Obj_PVP_Enemy_Soldier);
                if (tmpObj == null)
                    continue;
                FightViewItem_Soldier tmpSoldier = tmpObj.AddComponent<FightViewItem_Soldier>();
                tmpSoldier.Initialize();
                tmpSoldier.transform.localPosition = new Vector3(-85 * i, 0, 0);
                tmpSoldier.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
                tmpSoldier.name = string.Format("Soldier_{0}", i);
                tmpSoldier.gameObject.SetActive(true);
                soldierList_Enemy.Add(tmpSoldier);
            }
        }

        fightView.Obj_PVP.gameObject.SetActive(false);
    }

    public void RefreshPanel(EFightType vFightType, FightPlayerInfo vPlayerInfoSelf, FightPlayerInfo vPlayerInfoEnemy)
    {
        if ((vPlayerInfoSelf == null) || (vPlayerInfoEnemy == null))
            return;

        if ((UISystem.Instance.FightView.pPetData != null) && (UISystem.Instance.FightView.pPetData.Skill != null) && (UISystem.Instance.FightView.pPetData.Skill.Att != null))
        {
            fightView.Btn_Self_PetSkill.gameObject.SetActive(true);
            CommonFunction.SetSpriteName(fightView.Spt_Self_PetSkill_Icon, UISystem.Instance.FightView.pPetData.Skill.Att.Icon);
            fightView.Spt_Self_PetSkill_Icon.gameObject.SetActive(true);
            _CDTime[PETDATA_INDEX_SELF] = CommonFunction.GetSecondTimeByMilliSecond(UISystem.Instance.FightView.pPetData.Skill.Att.waitTime);
        }
        if ((vPlayerInfoEnemy.mPetInfo != null) && (vPlayerInfoEnemy.mPetInfo.skill != null) && (vPlayerInfoEnemy.mPetInfo.skill.Count >= 1))
        {
            if (vPlayerInfoEnemy.mPetInfo.skill[0] != null)
            {
                SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vPlayerInfoEnemy.mPetInfo.skill[0].id);
                if (tmpSkillInfo != null)
                {
                    fightView.Btn_Enemy_PetSkill.gameObject.SetActive(true);
                    CommonFunction.SetSpriteName(fightView.Spt_Enemy_PetSkill_Icon, tmpSkillInfo.Icon);
                    fightView.Spt_Enemy_PetSkill_Icon.gameObject.SetActive(true);
                    _CDTime[PETDATA_INDEX_ENEMY] = CommonFunction.GetSecondTimeByMilliSecond(tmpSkillInfo.waitTime);
                }
            }
        }

        Refresh_Magic();
        Refresh_PetPower();
        Refresh_Skill(vPlayerInfoSelf.mEquip, vPlayerInfoEnemy.mEquip);
        Refresh_Soldier(vPlayerInfoSelf.mSoldierList, vPlayerInfoEnemy.mSoldierList);
        fightView.Obj_PVP.gameObject.SetActive(true);
    }

    public void Refresh_Magic()
    {
        if (fightView == null)
            return;
        if (fightViewController == null)
            return;
        fightView.Lbl_SMagic_Title.text = string.Format("{0}/{1}", fightViewController.Get_CurValue_Magic, fightViewController.Get_MAXValue_Magic);
        fightView.UIProgressBar_Self_Magic.value = (float)fightViewController.Get_CurValue_Magic / fightViewController.Get_MAXValue_Magic;
        fightView.Lbl_EMagic_Title.text = string.Format("{0}/{1}", fightViewController.Get_CurValue_EnemyMagic, fightViewController.Get_MAXValue_EnemyMagic);
        fightView.UIProgressBar_Enemy_Magic.value = (float)fightViewController.Get_CurValue_EnemyMagic / fightViewController.Get_MAXValue_EnemyMagic;
    }

    public void Refresh_PetPower()
    {
        ShowPetSkillEffect(EFightCamp.efcSelf, false);
        ShowPetSkillEffect(EFightCamp.efcEnemy, false);
        if (fightView == null)
            return;
        if (fightViewController == null)
            return;
        fightView.Spt_Self_PetSkill_Slider.fillAmount = (float)fightViewController.Get_CurValue_PetPower / fightViewController.Get_MAXValue_PetPower;
        if (fightView.Spt_Self_PetSkill_Slider.fillAmount >= 1)
        {
            //演示特效//
            ShowPetSkillEffect(EFightCamp.efcSelf, true);
        }
        fightView.Spt_Enemy_PetSkill_Slider.fillAmount = (float)fightViewController.Get_CurValue_EnemyPetPower / fightViewController.Get_MAXValue_EnemyPetPower;
        if (fightView.Spt_Enemy_PetSkill_Slider.fillAmount >= 1)
        {
            //演示特效//
            ShowPetSkillEffect(EFightCamp.efcEnemy, true);
        }
    }

    private void Refresh_Skill(ArtifactedDepot vSkillSelf, ArtifactedDepot vSkillEnemy)
    {
        int tmpIndex = 0;
        if ((vSkillSelf != null) && (vSkillSelf._EquiptList != null))
        {
            for (int i = 0; i < vSkillSelf._EquiptList.Count; i++)
            {
                if (vSkillSelf._EquiptList[i] == null)
                    continue;
                if (vSkillSelf._EquiptList[i].Att == null)
                    continue;
                if (vSkillSelf._EquiptList[i].Att.type != 0)
                    continue;
                SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vSkillSelf._EquiptList[i].Att.skillID);
                if (tmpSkillInfo == null)
                    continue;
                if (tmpIndex >= skillList_Self.Count)
                    continue;
                skillList_Self[tmpIndex].RefreshInfo_ShowStatus(new SkillBaseData(tmpSkillInfo.nId, tmpSkillInfo.Icon, tmpSkillInfo.expendMagic,
                    tmpSkillInfo.expendMagic, CommonFunction.GetSecondTimeByMilliSecond(tmpSkillInfo.waitTime), vSkillSelf._EquiptList[i]));
                tmpIndex++;
            }
        }
        tmpIndex = 0;
        if ((vSkillEnemy != null) && (vSkillEnemy._EquiptList != null))
        {
            for (int i = 0; i < vSkillEnemy._EquiptList.Count; i++)
            {
                if (vSkillEnemy._EquiptList[i] == null)
                    continue;
                if (vSkillEnemy._EquiptList[i].Att == null)
                    continue;
                if (vSkillEnemy._EquiptList[i].Att.type != 0)
                    continue;
                SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vSkillEnemy._EquiptList[i].Att.skillID);
                if (tmpSkillInfo == null)
                    continue;
                if (tmpIndex >= skillList_Enemy.Count)
                    continue;
                skillList_Enemy[tmpIndex].RefreshInfo_ShowStatus(new SkillBaseData(tmpSkillInfo.nId, tmpSkillInfo.Icon, tmpSkillInfo.expendMagic,
                    tmpSkillInfo.expendMagic, CommonFunction.GetSecondTimeByMilliSecond(tmpSkillInfo.waitTime), vSkillEnemy._EquiptList[i]));
                tmpIndex++;
            }
        }
    }

    private void Refresh_Soldier(List<FightSoldierInfo> vSoldierSelf, List<FightSoldierInfo> vSoldierEnemy)
    {
        if (vSoldierSelf != null)
        {
            for (int i = 0; i < soldierList_Self.Count; i++)
            {
                soldierList_Self[i].InitStatus();
                if (i >= vSoldierSelf.Count)
                    continue;
                soldierList_Self[i].RefreshInfo_ShowStatus(vSoldierSelf[i].mSoldier, EFightCamp.efcSelf, vSoldierSelf[i].mNum);
            }
        }

        if (vSoldierEnemy != null)
        {
            for (int i = 0; i < soldierList_Enemy.Count; i++)
            {
                soldierList_Enemy[i].InitStatus();
                if (i >= vSoldierEnemy.Count)
                    continue;
                soldierList_Enemy[i].RefreshInfo_ShowStatus(vSoldierEnemy[i].mSoldier, EFightCamp.efcEnemy, vSoldierEnemy[i].mNum);
            }
        }
    }

    /// <summary>
    /// 宠物技能特效提示
    /// </summary>
    /// <param name="vFightCamp"></param>
    /// <param name="vIsShow"></param>
    private void ShowPetSkillEffect(EFightCamp vFightCamp, bool vIsShow)
    {
        if (fightView != null)
        {
            if (vFightCamp == EFightCamp.efcSelf)
            {
                if (fightView.Obj_Self_PetSkill_Effect != null)
                {
                    fightView.Obj_Self_PetSkill_Effect.SetActive(vIsShow);
                }
            }
            else
            {
                if (fightView.Obj_Enemy_PetSkill_Effect != null)
                {
                    fightView.Obj_Enemy_PetSkill_Effect.SetActive(vIsShow);
                }
            }
        }
    }

    /// <summary>
    /// 显示CD
    /// </summary>
    private void ShowPetSkillCD_Self()
    {
        if (_ItemStatus[PETDATA_INDEX_SELF] != EFightViewSkillStatus.eskCD)
            return;

        showCDTime[PETDATA_INDEX_SELF] += curFightSpeed * (Time.time - _ClickTime[PETDATA_INDEX_SELF]);
        _ClickTime[PETDATA_INDEX_SELF] = Time.time;
        float tmpValue = (_CDTime[PETDATA_INDEX_SELF] - showCDTime[PETDATA_INDEX_SELF]) / _CDTime[PETDATA_INDEX_SELF];
        if (tmpValue <= 0)
        {
            tmpValue = 0;
            Scheduler.Instance.RemoveUpdator(ShowPetSkillCD_Self);
            _ItemStatus[PETDATA_INDEX_SELF] = EFightViewSkillStatus.eskNormal;
        }

        fightView.Spt_Self_PetSkill_Mask.fillAmount = tmpValue;
    }
    private void ShowPetSkillCD_Enemy()
    {
        if (_ItemStatus[PETDATA_INDEX_ENEMY] != EFightViewSkillStatus.eskCD)
            return;

        showCDTime[PETDATA_INDEX_ENEMY] += curFightSpeed * (Time.time - _ClickTime[PETDATA_INDEX_ENEMY]);
        _ClickTime[PETDATA_INDEX_ENEMY] = Time.time;
        float tmpValue = (_CDTime[PETDATA_INDEX_ENEMY] - showCDTime[PETDATA_INDEX_ENEMY]) / _CDTime[PETDATA_INDEX_ENEMY];
        if (tmpValue <= 0)
        {
            tmpValue = 0;
            Scheduler.Instance.RemoveUpdator(ShowPetSkillCD_Enemy);
            _ItemStatus[PETDATA_INDEX_ENEMY] = EFightViewSkillStatus.eskNormal;
        }

        fightView.Spt_Enemy_PetSkill_Mask.fillAmount = tmpValue;
    }
    public void ClickPetSkillOperate(EFightCamp vFightCampe)
    {
        curFightSpeed = SceneManager.Instance.Get_FightSpeed;
        if (vFightCampe == EFightCamp.efcSelf)
        {
            _ItemStatus[PETDATA_INDEX_SELF] = EFightViewSkillStatus.eskCD;
            _ClickTime[PETDATA_INDEX_SELF] = Time.time;
            showCDTime[PETDATA_INDEX_SELF] = 0;
            Scheduler.Instance.AddUpdator(ShowPetSkillCD_Self);
        }
        else
        {
            _ItemStatus[PETDATA_INDEX_ENEMY] = EFightViewSkillStatus.eskCD;
            _ClickTime[PETDATA_INDEX_ENEMY] = Time.time;
            showCDTime[PETDATA_INDEX_ENEMY] = 0;
            Scheduler.Instance.AddUpdator(ShowPetSkillCD_Enemy);
        }
    }
    private const int PETDATA_INDEX_SELF = 0;
    private const int PETDATA_INDEX_ENEMY = 1;
    private EFightViewSkillStatus[] _ItemStatus = new EFightViewSkillStatus[2];
    private EFightViewSkillStatus[] _PreStatus = new EFightViewSkillStatus[2];
    private float[] showCDTime = new float[2];
    private float curFightSpeed;
    private float[] _ClickTime = new float[2];
    private float[] _CDTime = new float[2];

    public EFightViewSkillStatus Get_ItemStatus(EFightCamp vFightCamp)
    {
        int tmpIndex = (int)vFightCamp - 1;
        if ((tmpIndex >= 0) && (tmpIndex < _ItemStatus.Length))
        {
            return _ItemStatus[tmpIndex];
        }
        else
        {
            return EFightViewSkillStatus.eskNone;
        }
    }
}