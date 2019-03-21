using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// 计算战斗属性
/// </summary>
public class Calculation_Attribute : Singleton<Calculation_Attribute>
{
    public Dictionary<ESkillEffectType, Attribute_PlusCoefficient> PlusCoeffDic = new Dictionary<ESkillEffectType, Attribute_PlusCoefficient>();

    /*
    /// <summary>
    /// 计算玩家战斗力[远征 竞技场]
    /// 战斗力 = 英雄战力 + 布阵武将战力之和 / 布阵武将总数 * 出战士兵种类数量
    /// </summary>
    /// <param name="vLevel">英雄等级[0-直接读取PlayerData数据]</param>
    /// <param name="vSoldierDic">武将数据 - 服务端数据ID</param>
    /// <param name="vEquipList">英雄身上装备[ID, 等级][其他人数据-自己英雄不用传这个数据]</param>
    /// <param name="vSkillList">英雄被动技能[ID，等级][其他人数据-自己英雄不用传这个数据]</param>
    /// <returns>战斗力数值</returns>
    public int Calculation_PlayerCombatPower(uint vLevel = 0, Dictionary<Soldier, int> vSoldierDic = null, List<CalBaseData> vEquipList = null, List<CalBaseData> vSkillList = null)
    {
        int tmpResult = 0;
        ShowInfoHero tmpHero = Calculation_Attribute_Hero(vLevel, vEquipList, vSkillList);
        if (tmpHero != null)
            tmpResult += tmpHero.CombatPower;

        //士兵总战斗力//
        int tmpSoldierPower = 0;
        //士兵数量//
        int tmpSoldierNum = 0;
        if (vSoldierDic != null)
        {
            foreach (KeyValuePair<Soldier, int> tmpInfo in vSoldierDic)
            {
                ShowInfoSoldiers tmpSingleSoldier = Calculation_Attribute_Soldier(tmpInfo.Key);
                if (tmpSingleSoldier == null)
                    continue;
                tmpSoldierPower += tmpSingleSoldier.CombatPower * tmpInfo.Value;
                tmpSoldierNum += tmpInfo.Value;
            }
            if (!tmpSoldierNum.Equals(0))
                tmpResult += tmpSoldierPower / tmpSoldierNum * vSoldierDic.Count;
        }
        return tmpResult;
    }
    */

    public int CalculationPlayerCombatPower()
    {
        List<Weapon> weapons = PlayerData.Instance._ArtifactedDepot._EquiptList;
        List<CalBaseData> equips = new List<CalBaseData>();
        List<CalBaseData> skills = new List<CalBaseData>();
        if (weapons != null)
        {
            for (int i = 0; i < weapons.Count; i++)
            {
                Weapon weapon = weapons[i];
                if (weapon == null)
                    continue;
                CalBaseData equipData = new CalBaseData(weapon.Att.id, weapon.Level);
                if (weapon._Skill != null)
                {
                    CalBaseData skillData = new CalBaseData(weapon._Skill.Att.nId, weapon._Skill.Level);
                    skills.Add(skillData);
                }
                equips.Add(equipData);
            }
        }
        if (PlayerData.Instance._SkillsDepot != null && PlayerData.Instance._SkillsDepot._skillsList != null)
        {
            for (int i = 0; i < PlayerData.Instance._SkillsDepot._skillsList.Count; i++)
            {
                Skill skill = PlayerData.Instance._SkillsDepot._skillsList[i];
                if (skill == null)
                    continue;
                CalBaseData skillData = new CalBaseData(skill.Att.nId, skill.Lv);
                skills.Add(skillData);
            }
        }
        PetData petData = PlayerData.Instance._PetDepot.GetEquipedPet();
        if (petData != null)
        {
            CalBaseData skillData = new CalBaseData(petData.Skill.Att.nId, petData.Skill.Lv);
            skills.Add(skillData);
        }
        return Calculation_PlayerCombatPower(PlayerData.Instance._Level, null, equips, skills);
    }

    /// <summary>
    /// 计算玩家战斗力[远征 竞技场]
    /// 总战力 = 英雄战力 + 武将A战力 * 武将A数量 /3 + 武将B战力 * 武将B数量 / 3 + 武将C战力 * 武将C数量 + …… + 武将F战力 * 武将F数量 /3
    /// </summary>
    /// <param name="vLevel">英雄等级[0-直接读取PlayerData数据]</param>
    /// <param name="vSoldierDic">武将数据 - 服务端数据ID</param>
    /// <param name="vEquipList">英雄身上装备[ID, 等级][其他人数据-自己英雄不用传这个数据]</param>
    /// <param name="vSkillList">英雄被动技能[ID，等级][其他人数据-自己英雄不用传这个数据]</param>
    /// <returns>战斗力数值</returns>
    public int Calculation_PlayerCombatPower(uint vLevel = 0, Dictionary<Soldier, int> vSoldierDic = null, List<CalBaseData> vEquipList = null, List<CalBaseData> vSkillList = null)
    {
        int tmpResult = 0;
        ShowInfoHero tmpHero = Calculation_Attribute_Hero(vLevel, vEquipList, vSkillList);
        if (tmpHero != null)
            tmpResult += tmpHero.CombatPower;

        int tmpSoldierPower = 0;
        if (vSoldierDic != null)
        {
            foreach (KeyValuePair<Soldier, int> tmpInfo in vSoldierDic)
            {
                ShowInfoSoldiers tmpSingleSoldier = Calculation_Attribute_Soldier(tmpInfo.Key);
                if (tmpSingleSoldier == null)
                    continue;
                tmpSoldierPower += tmpSingleSoldier.CombatPower * tmpInfo.Value;
            }
            tmpResult += tmpSoldierPower / 3;
        }
        return tmpResult;
    }

    /// <summary>
    /// 计算英雄属性
    /// </summary>
    /// <param name="vLevel">英雄等级[0-直接读取PlayerData数据]</param>
    /// <param name="vEquipList">英雄身上装备[ID, 等级][null或者没有记录-直接读取PlayerData数据]</param>
    /// <param name="vSkillList">英雄被动技能[ID，等级][null或者没有记录-直接读取PlayerData数据]</param>
    /// <returns>英雄战斗属性</returns>
    public ShowInfoHero Calculation_Attribute_Hero(uint vLevel = 0, List<CalBaseData> vEquipList = null, List<CalBaseData> vSkillList = null)
    {
        ShowInfoHero tmpResult = new ShowInfoHero();
        uint tmpLevel = vLevel;
        if (tmpLevel == 0)
            tmpLevel = PlayerData.Instance._Level;
        HeroAttributeInfo tmpHeroInfo = ConfigManager.Instance.mHeroData.GetHeroAttributeByLV(tmpLevel);
        if (tmpHeroInfo == null)
            return tmpResult;

        float tmpSkillCombatPower = 0;
        //设置初始属性//
        tmpResult.ReSetFightAttribute(tmpLevel, tmpHeroInfo.HP, tmpHeroInfo.HPRecovery, tmpHeroInfo.Attack, tmpHeroInfo.AttRate,
            tmpHeroInfo.AttDistance, tmpHeroInfo.Accuracy, tmpHeroInfo.Crit, tmpHeroInfo.Dodge, tmpHeroInfo.Tenacity, tmpHeroInfo.MoveSpeed,
            tmpHeroInfo.MP, tmpHeroInfo.MPRecovery, tmpHeroInfo.Energy, tmpHeroInfo.EnergyRecovery, 0);

        //初始化加成系数//
        Init_PlusCoefficient();

        //计算英雄身上装备加成系数//
        if (PlayerData.Instance._WeaponDepot != null)
        {
            List<Weapon> tmpWeaponList = PlayerData.Instance._WeaponDepot.getWeaponList();
            if (tmpWeaponList != null)
            {
                for (int i = 0; i < tmpWeaponList.Count; i++)
                {
                    if (tmpWeaponList[i] == null)
                        continue;
                    Calculation_SingleEquip_PlusAttribute(tmpWeaponList[i].Att.id, tmpWeaponList[i].Level);
                }
            }
        }
        //计算英雄被动技能加成系数//
        for (int i = 0; i < vSkillList.Count; i++)
        {
            if (vSkillList[i] == null)
                continue;
            tmpSkillCombatPower += Calculation_SingleSkill_PlusCoefficient(vSkillList[i].ID, vSkillList[i].Level);
        }

        //计算英雄命魂加成系数//
        tmpSkillCombatPower += Calculation_LifeSoul_PlusAttribute_Hero();
        //计算宠物加成系数
        tmpSkillCombatPower += Calculation_Pet_PlusAttribute_Hero();

        //计算最终属性//
        float tmpAttack = 0;
        float tmpHP = 0;
        float tmpAccuracy = 0;
        float tmpDodge = 0;
        float tmpCrit = 0;
        float tmpTenacity = 0;
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
        {
            tmpAttack = tmpResult.Attack * PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N1;
            tmpResult.Attack = Mathf.FloorToInt(tmpAttack);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
        {
            tmpHP = tmpResult.HP * PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N1;
            tmpResult.HP = Mathf.FloorToInt(tmpHP);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
        {
            tmpAccuracy = tmpResult.Accuracy * PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N1;
            tmpResult.Accuracy = Mathf.FloorToInt(tmpAccuracy);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
        {
            tmpDodge = tmpResult.Dodge * PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N1;
            tmpResult.Dodge = Mathf.FloorToInt(tmpDodge);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
        {
            tmpCrit = tmpResult.Crit * PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N1;
            tmpResult.Crit = Mathf.FloorToInt(tmpCrit);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
        {
            tmpTenacity = tmpResult.Tenacity * PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N1;
            tmpResult.Tenacity = Mathf.FloorToInt(tmpTenacity);
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttSpeed))
            tmpResult.AttRate = Mathf.FloorToInt(tmpResult.AttRate * PlusCoeffDic[ESkillEffectType.esetAttSpeed].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAttSpeed].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMoveSpeed))
            tmpResult.MoveSpeed = Mathf.FloorToInt(tmpResult.MoveSpeed * PlusCoeffDic[ESkillEffectType.esetMoveSpeed].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMoveSpeed].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHPRecovery))
            tmpResult.HPRecovery = Mathf.FloorToInt(tmpResult.HPRecovery * PlusCoeffDic[ESkillEffectType.esetHPRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetHPRecovery].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetEnergyMax))
            tmpResult.Energy = Mathf.FloorToInt(tmpResult.Energy * PlusCoeffDic[ESkillEffectType.esetEnergyMax].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetEnergyMax].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetEnergyRecovery))
            tmpResult.EnergyRecovery = Mathf.FloorToInt(tmpResult.EnergyRecovery * PlusCoeffDic[ESkillEffectType.esetEnergyRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetEnergyRecovery].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMagicMax))
            tmpResult.MP = Mathf.FloorToInt(tmpResult.MP * PlusCoeffDic[ESkillEffectType.esetMagicMax].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMagicMax].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMagicRecovery))
            tmpResult.MPRecovery = Mathf.FloorToInt(tmpResult.MPRecovery * PlusCoeffDic[ESkillEffectType.esetMagicRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMagicRecovery].PlusCoeff_N1);
        tmpResult.AttRate = CommonFunction.GetSecondTimeByMilliSecond(tmpResult.AttRate);


        //计算头像框追加系能系数//
        Init_PlusCoefficient();
        tmpSkillCombatPower += CalculateAchievement_PlusAttribute();
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack) && PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2 > 1)
        {
            tmpAttack = tmpResult.Attack * PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2;
            tmpResult.Attack = (int)tmpAttack;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP) && PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2 > 1)
        {
            tmpHP = tmpResult.HP * PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2;
            tmpResult.HP = (int)tmpHP;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy) && PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2 > 1)
        {
            tmpAccuracy = tmpResult.Accuracy * PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2;
            tmpResult.Accuracy = (int)tmpAccuracy;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge) && PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2 > 1)
        {
            tmpDodge = tmpResult.Dodge * PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2;
            tmpResult.Dodge = (int)tmpDodge;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit) && PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2 > 1)
        {
            tmpCrit = tmpResult.Crit * PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2;
            tmpResult.Crit = (int)tmpCrit;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity) && PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2 > 1)
        {
            tmpTenacity = tmpResult.Tenacity * PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2;
            tmpResult.Tenacity = (int)tmpTenacity;
        }

        //计算战斗力//
        tmpResult.CombatPower = Mathf.FloorToInt(Calculation_CombatPower((int)ERoleSeat.ersHero, 0, tmpAttack, tmpHP, tmpCrit, tmpTenacity, tmpAccuracy, tmpDodge, tmpSkillCombatPower));
        return tmpResult;
    }

    /// <summary>
    /// 计算单个武将属性
    /// </summary>
    /// <param name="vSoldierInfo">服务端数据ID</param>
    /// <param name="isReSetValue">是否需要计算[true-要计算 false-不计算]</param>
    /// <returns>武将战斗属性</returns>
    public ShowInfoSoldiers Calculation_Attribute_Soldier(Soldier vSoldierInfo, bool isReSetValue = false)
    {
        ShowInfoSoldiers tmpResult = new ShowInfoSoldiers();
        if (vSoldierInfo == null)
            return tmpResult;
        if (vSoldierInfo.Att == null)
            return tmpResult;

        //设置初始属性//
        tmpResult.CopyTo(vSoldierInfo.showInfoSoldier);

        if (!isReSetValue)
            return tmpResult;

        float tmpSkillCombatPower = 0;
        //初始化加成系数//
        Init_PlusCoefficient();

        //计算装备加成 武将身上装备//
        if (vSoldierInfo != null && vSoldierInfo._equippedDepot != null && vSoldierInfo._equippedDepot._EquiptList != null)
        {
            foreach (Weapon tmpInfo in vSoldierInfo._equippedDepot._EquiptList)
            {
                if (tmpInfo == null)
                    continue;
                Calculation_SingleEquip_PlusAttribute(tmpInfo.Att.id, tmpInfo.Level);
                tmpSkillCombatPower += Calculation_SingleSkill_PlusCoefficient(tmpInfo.Att.skillID, tmpInfo.Level);
            }
        }
        //计算技能加成 武将身上的技能
        if (vSoldierInfo != null && vSoldierInfo._skillsDepot != null && vSoldierInfo._skillsDepot._skillsList != null)
        {
            foreach (Skill tmpInfo in vSoldierInfo._skillsDepot._skillsList)
            {
                if (tmpInfo == null)
                    continue;
                tmpSkillCombatPower += Calculation_SingleSkill_PlusCoefficient(tmpInfo.Att.nId, tmpInfo.Level);
            }
        }

        //计算最终属性//
        float tmpAttack = 0;
        float tmpHP = 0;
        float tmpAccuracy = 0;
        float tmpDodge = 0;
        float tmpCrit = 0;
        float tmpTenacity = 0;
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
        {
            tmpAttack = tmpResult.Attack * PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N1;
            tmpResult.Attack = (int)tmpAttack;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
        {
            tmpHP = tmpResult.HP * PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N1;
            tmpResult.HP = (int)tmpHP;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
        {
            tmpAccuracy = tmpResult.Accuracy * PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N1;
            tmpResult.Accuracy = (int)tmpAccuracy;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
        {
            tmpDodge = tmpResult.Dodge * PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N1;
            tmpResult.Dodge = (int)tmpDodge;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
        {
            tmpCrit = tmpResult.Crit * PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N1;
            tmpResult.Crit = (int)tmpCrit;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
        {
            tmpTenacity = tmpResult.Tenacity * PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N1;
            tmpResult.Tenacity = (int)tmpTenacity;
        }
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttSpeed))
            tmpResult.AttRate = (int)(tmpResult.AttRate * PlusCoeffDic[ESkillEffectType.esetAttSpeed].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetAttSpeed].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMoveSpeed))
            tmpResult.MoveSpeed = (int)(tmpResult.MoveSpeed * PlusCoeffDic[ESkillEffectType.esetMoveSpeed].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMoveSpeed].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHPRecovery))
            tmpResult.HPRecovery = (int)(tmpResult.HPRecovery * PlusCoeffDic[ESkillEffectType.esetHPRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetHPRecovery].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetEnergyMax))
            tmpResult.Energy = (int)(tmpResult.Energy * PlusCoeffDic[ESkillEffectType.esetEnergyMax].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetEnergyMax].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetEnergyRecovery))
            tmpResult.EnergyRecovery = (int)(tmpResult.EnergyRecovery * PlusCoeffDic[ESkillEffectType.esetEnergyRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetEnergyRecovery].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMagicMax))
            tmpResult.MP = (int)(tmpResult.MP * PlusCoeffDic[ESkillEffectType.esetMagicMax].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMagicMax].PlusCoeff_N1);
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetMagicRecovery))
            tmpResult.MPRecovery = (int)(tmpResult.MPRecovery * PlusCoeffDic[ESkillEffectType.esetMagicRecovery].PlusCoeff_N2 + PlusCoeffDic[ESkillEffectType.esetMagicRecovery].PlusCoeff_N1);
        //计算战斗力//
        //tmpResult.CombatPower = Calculation_CombatPower(vSoldierInfo.Att.Stance, vSoldierInfo.Att.Career, tmpAttack, tmpHP, tmpCrit, tmpTenacity, tmpAccuracy, tmpDodge, tmpSkillCombatPower);
        tmpResult.CombatPower = Mathf.FloorToInt(Calculation_CombatPower(vSoldierInfo.Att.Stance, vSoldierInfo.Att.Career, tmpResult.Attack, tmpResult.HP, tmpResult.Crit, tmpResult.Tenacity, tmpResult.Accuracy, tmpResult.Dodge, tmpSkillCombatPower));
        return tmpResult;
    }

    /// <summary>
    /// 初始化数据
    /// </summary>
    private void Init_PlusCoefficient()
    {
        PlusCoeffDic.Clear();
        PlusCoeffDic.Add(ESkillEffectType.esetAttack, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetHP, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetAccuracy, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetDodge, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetCrit, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetTenacity, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetAttSpeed, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetMoveSpeed, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetHPRecovery, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetEnergyMax, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetEnergyRecovery, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetMagicMax, new Attribute_PlusCoefficient());
        PlusCoeffDic.Add(ESkillEffectType.esetMagicRecovery, new Attribute_PlusCoefficient());
    }

    /// <summary>
    /// 计算单个装备加成
    /// </summary>
    /// <param name="vID">配置表ID</param>
    /// <param name="vLV">当前等级</param>
    private void Calculation_SingleEquip_PlusAttribute(uint vID, int vLV)
    {
        EquipAttributeInfo tmpEquipInfo = ConfigManager.Instance.mEquipData.FindById(vID);
        if (tmpEquipInfo == null)
            return;

        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
            PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N1 += (tmpEquipInfo.phy_atk + tmpEquipInfo.u_attack * (vLV - tmpEquipInfo.baseLevel));
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
            PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N1 += (tmpEquipInfo.hp_max + tmpEquipInfo.u_hp * (vLV - tmpEquipInfo.baseLevel));
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
            PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N1 += (tmpEquipInfo.acc_rate + tmpEquipInfo.u_accuracy * (vLV - tmpEquipInfo.baseLevel));
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
            PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N1 += (tmpEquipInfo.ddg_rate + tmpEquipInfo.u_dodge * (vLV - tmpEquipInfo.baseLevel));
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
            PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N1 += (tmpEquipInfo.crt_rate + tmpEquipInfo.u_crit * (vLV - tmpEquipInfo.baseLevel));
        if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
            PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N1 += (tmpEquipInfo.tnc_rate + tmpEquipInfo.u_tenacity * (vLV - tmpEquipInfo.baseLevel));
    }

    /// <summary>
    /// 计算单个技能加成
    /// </summary>
    /// <param name="vID">配置表ID</param>
    /// <param name="vLV">当前等级</param>
    private float Calculation_SingleSkill_PlusCoefficient(uint vID, int vLV)
    {
        SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vID);
        if (tmpSkillInfo == null)
            return 0;
        float tmpResult = tmpSkillInfo.CombatPoint + tmpSkillInfo.CombatGrow * (vLV - tmpSkillInfo.initLevel);
        if (tmpSkillInfo.triggerType != (int)ETriggerType.ettPassive)
            return tmpResult;
        if (tmpSkillInfo.effect == null)
            return tmpResult;
        for (int i = 0; i < tmpSkillInfo.effect.Count; i++)
        {
            if (!PlusCoeffDic.ContainsKey((ESkillEffectType)tmpSkillInfo.effect[i].effectId))
                continue;
            PlusCoeffDic[(ESkillEffectType)tmpSkillInfo.effect[i].effectId].PlusCoeff_N1 += tmpSkillInfo.effect[i].num + (vLV - tmpSkillInfo.initLevel) * tmpSkillInfo.effect[i].u_num;
            if (tmpSkillInfo.effect[i].percent + (vLV - tmpSkillInfo.initLevel) * tmpSkillInfo.effect[i].u_percent != 0)
                PlusCoeffDic[(ESkillEffectType)tmpSkillInfo.effect[i].effectId].PlusCoeff_N2 *= tmpSkillInfo.effect[i].percent + (vLV - tmpSkillInfo.initLevel) * tmpSkillInfo.effect[i].u_percent;
        }
        return tmpResult;
    }
    private float Calculation_SingleSkill_PlusCoefficient(uint vID)
    {
        SkillAttributeInfo tmpSkillInfo = ConfigManager.Instance.mSkillAttData.FindById(vID);
        if (tmpSkillInfo == null)
            return 0;
        float tmpResult = tmpSkillInfo.CombatPoint;
        if (tmpSkillInfo.triggerType != (int)ETriggerType.ettPassive)
            return tmpResult;
        if (tmpSkillInfo.effect == null)
            return tmpResult;
        for (int i = 0; i < tmpSkillInfo.effect.Count; i++)
        {
            if (!PlusCoeffDic.ContainsKey((ESkillEffectType)tmpSkillInfo.effect[i].effectId))
                continue;
            PlusCoeffDic[(ESkillEffectType)tmpSkillInfo.effect[i].effectId].PlusCoeff_N1 += tmpSkillInfo.effect[i].num;
            if (tmpSkillInfo.effect[i].percent != 0)
                PlusCoeffDic[(ESkillEffectType)tmpSkillInfo.effect[i].effectId].PlusCoeff_N2 *= tmpSkillInfo.effect[i].percent;
        }
        return tmpResult;
    }

    /// <summary>
    /// 计算最终战斗力
    /// </summary>
    /// <param name="vStance">站位</param>
    /// <param name="vCareer">职业</param>
    /// <param name="vAttack">攻击</param>
    /// <param name="vHP">生命</param>
    /// <param name="vCrit">暴击</param>
    /// <param name="vTenacity">韧性</param>
    /// <param name="vAccuracy">命中</param>
    /// <param name="vDodge">闪避</param>
    /// <param name="vSkillCombatPower">技能增加战斗力</param>
    /// <returns></returns>
    private float Calculation_CombatPower(int vStance, int vCareer, float vAttack, float vHP, float vCrit, float vTenacity, float vAccuracy, float vDodge, float vSkillCombatPower)
    {
        CombatPowerInfo tmpInfo = ConfigManager.Instance.mCombatPowerData.FindSingleInfo(vStance, vCareer);
        if (tmpInfo == null)
            return 0;
        float tmpResult = vAttack * tmpInfo.phy_atk + vHP * tmpInfo.hp_max + vCrit * tmpInfo.crt_rate + vTenacity * tmpInfo.tnc_rate + vAccuracy * tmpInfo.acc_rate + vDodge * tmpInfo.ddg_rate + vSkillCombatPower;
        //Debug.LogWarning("---------------------------------------------------------------------");
        //Debug.LogWarning(string.Format("Power: [{0}, {1}]", vSkillCombatPower, tmpResult));
        //Debug.LogWarning(string.Format("Att: [{0}, {1}, {2}]", vAttack, tmpInfo.phy_atk, vAttack * tmpInfo.phy_atk));
        //Debug.LogWarning(string.Format("HP: [{0}, {1}, {2}]", vHP, tmpInfo.hp_max, vHP * tmpInfo.hp_max));
        //Debug.LogWarning(string.Format("Crit: [{0}, {1}, {2}]", vCrit, tmpInfo.crt_rate, vCrit * tmpInfo.crt_rate));
        //Debug.LogWarning(string.Format("Tenacity: [{0}, {1}, {2}]", vTenacity, tmpInfo.tnc_rate, vTenacity * tmpInfo.tnc_rate));
        //Debug.LogWarning(string.Format("Accuracy: [{0}, {1}, {2}]", vAccuracy, tmpInfo.acc_rate, vAccuracy * tmpInfo.acc_rate));
        //Debug.LogWarning(string.Format("Dodge: [{0}, {1}, {2}]", vDodge, tmpInfo.ddg_rate, vDodge * tmpInfo.ddg_rate));
        return tmpResult;
    }


    /// <summary>
    /// 计算英雄命魂
    /// </summary>
    private float Calculation_LifeSoul_PlusAttribute_Hero()
    {
        float tmpResult = 0;
        List<LifeSoulData> tmpList = PlayerData.Instance._LifeSoulDepot.GetPlayerEquipedLifeSoul();
        if (tmpList != null)
        {
            int tmpCount = tmpList.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                if (tmpList[i] != null)
                {
                    tmpResult += Calculation_SingleLifeSoul_PlusAttribute(tmpList[i]);
                }
            }
        }
        return tmpResult;
    }
    /// <summary>
    /// 计算单个命魂
    /// </summary>
    /// <param name="vDataInfo"></param>
    /// <returns></returns>
    private float Calculation_SingleLifeSoul_PlusAttribute(LifeSoulData vDataInfo)
    {
        float tmpResult = 0;
        if (vDataInfo != null)
        {
            //属性：(int)((基础值 + 成长值 * 等级差) / 10000)
            if ((vDataInfo.SoulPOD != null) && (vDataInfo.SoulInfo != null))
            {
                int tmpUpLV = vDataInfo.SoulPOD.level - 1;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
                    PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N1 += (vDataInfo.SoulInfo.attack_initial + vDataInfo.SoulInfo.attack_up * tmpUpLV) / 10000;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
                    PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N1 += (vDataInfo.SoulInfo.hp_initial + vDataInfo.SoulInfo.hp_up * tmpUpLV) / 10000;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
                    PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N1 += (vDataInfo.SoulInfo.accrate_initial + vDataInfo.SoulInfo.accrate_up * tmpUpLV) / 10000;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
                    PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N1 += (vDataInfo.SoulInfo.ddgrate_initial + vDataInfo.SoulInfo.ddgrate_up * tmpUpLV) / 10000;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
                    PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N1 += (vDataInfo.SoulInfo.crt_initial + vDataInfo.SoulInfo.crt_up * tmpUpLV) / 10000;
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
                    PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N1 += (vDataInfo.SoulInfo.tenacity_initial + vDataInfo.SoulInfo.tenacity_up * tmpUpLV) / 10000;
            }

            if (vDataInfo.Skill != null)
            {
                tmpResult += Calculation_SingleSkill_PlusCoefficient(vDataInfo.Skill.Att.nId, vDataInfo.Skill.Lv);
            }
        }
        return tmpResult;
    }
    /// <summary>
    /// 计算头像头像框的加成
    /// </summary>
    /// <returns></returns>
    private float CalculateAchievement_PlusAttribute()
    {
        float tmpResult = 0;
        PlayerPortraitData icon = ConfigManager.Instance.mPlayerPortraitConfig.GetPlayerPortraitByID(PlayerData.Instance.HeadID);

        if (icon != null && icon.type == 3)
        {
            if (icon.hp > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
                PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2 += icon.hp / 100f;
            if (icon.attack > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
                PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2 += icon.attack / 100f;
            if (icon.hit > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
                PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2 += icon.hit / 100f;
            if (icon.dodge > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
                PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2 += icon.dodge / 100f;
            if (icon.crit > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
                PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2 += icon.crit / 100f;
            if (icon.crit_def > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
                PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2 += icon.crit_def / 100f;
            if (icon.skill > 0)
                tmpResult += Calculation_SingleSkill_PlusCoefficient((uint)icon.skill);
        }

        FrameData frame = ConfigManager.Instance.mFrameConfig.GetFrameDataByID(PlayerData.Instance.FrameID);
        if (frame != null && frame.type == 3)
        {
            if (frame.hp > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
                PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N2 += frame.hp / 100f;
            if (frame.attack > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
                PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N2 += frame.attack / 100f;
            if (frame.hit > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
                PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N2 += frame.hit / 100f;
            if (frame.dodge > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
                PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N2 += frame.dodge / 100f;
            if (frame.crit > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
                PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N2 += frame.crit / 100f;
            if (frame.crit_def > 0 && PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
                PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N2 += frame.crit_def / 100f;

            if (frame.skill > 0)
                tmpResult += Calculation_SingleSkill_PlusCoefficient((uint)frame.skill);
        }

        return tmpResult;
    }

    /// <summary>
    /// 计算宠物加成
    /// </summary>
    private float Calculation_Pet_PlusAttribute_Hero()
    {
        float tmpResult = 0;
        List<PetData> tmpList = PlayerData.Instance._PetDepot.GetOwnedPets();
        if (tmpList != null)
        {
            int tmpCount = tmpList.Count;
            for (int i = 0; i < tmpCount; i++)
            {
                if (tmpList[i] != null)
                {
                    tmpResult += Calculation_SinglePet_PlusAttribute(tmpList[i]);
                }
            }
        }
        return tmpResult;
    }

    private float Calculation_SinglePet_PlusAttribute(PetData petdata)
    {
        float tmpResult = 0;
        if (petdata != null)
        {
            if ((petdata.PetPOD != null) && (petdata.PetInfo != null))
            {
                int tmpUpLV = petdata.PetPOD.level - 1;

                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAttack))
                {
                    PlusCoeffDic[ESkillEffectType.esetAttack].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.phy_atk + (tmpUpLV * petdata.PetInfo.u_attack));
                }
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetHP))
                {
                    PlusCoeffDic[ESkillEffectType.esetHP].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.hp_max + (tmpUpLV * petdata.PetInfo.u_hp));
                }
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetAccuracy))
                {
                    PlusCoeffDic[ESkillEffectType.esetAccuracy].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.acc_rate + (tmpUpLV * petdata.PetInfo.u_accuracy));
                }
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetDodge))
                {
                    PlusCoeffDic[ESkillEffectType.esetDodge].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.ddg_rate + (tmpUpLV * petdata.PetInfo.u_dodge));
                }
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetCrit))
                {
                    PlusCoeffDic[ESkillEffectType.esetCrit].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.crt_rate + (tmpUpLV * petdata.PetInfo.u_crit));
                }
                if (PlusCoeffDic.ContainsKey(ESkillEffectType.esetTenacity))
                {
                    PlusCoeffDic[ESkillEffectType.esetTenacity].PlusCoeff_N1 += Mathf.FloorToInt(petdata.PetInfo.tnc_rate + (tmpUpLV * petdata.PetInfo.u_tenacity));
                }
            }
            if (petdata.Skill != null && petdata.Skill.Att.triggerType == (int)ETriggerType.ettPassive)
            {
                tmpResult += Calculation_SingleSkill_PlusCoefficient(petdata.Skill.Att.nId, petdata.Skill.Lv);
            }
        }
        return tmpResult;
    }
}