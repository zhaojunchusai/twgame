using System.Collections.Generic;
using UnityEngine;


/******** 游戏中的数值公式 ********/
public static class GameFormula
{
    /// <summary>
    /// 获取初始速度
    /// </summary>
    /// <param name="initalSpeed"></param>
    /// <returns></returns>
    public static uint GetActionSpeed(int initalSpeed)
    {
        float min = initalSpeed * 95f / 100 - 2;
        float max = initalSpeed * 105f / 100 + 2;
        return (uint)Mathf.RoundToInt(UnityEngine.Random.Range(min, max));
    }

    /// <summary>
    /// 物理伤害  普攻伤害F * ( 1 + 棋子物理伤害/10 + 消除数加成) * combo伤害加成; F = 普攻伤害F=普攻系数*（攻方攻击-守方防御）
    /// </summary>
    /// <param name="phyAttFactor">普攻系数</param>param>
    /// <param name="attackerPhyAtt">攻方攻击力</param>
    /// <param name="defenderPhyDef">守方防御力</param>
    /// <param name="chessPhyAtt">棋子物理伤害</param>
    /// <param name="matchAddition">消除数加成</param>
    /// <param name="comoboAddition">combo加成</param>
    /// <returns></returns>
    public static float GetPhysicalAttack(float phyAttFactor, float attackerPhyAtt, float defenderPhyDef, uint chessPhyAtt, float matchAddition, float comoboAddition)
    {
        phyAttFactor = Mathf.Max(0, phyAttFactor);
        float f = phyAttFactor * (attackerPhyAtt - defenderPhyDef);
        f = Mathf.Max(1.0f, f);
        float damage = f * (1 + (float)chessPhyAtt / 10 + matchAddition) * comoboAddition;
        return Mathf.Max(1, damage);
    }

    /// <summary>
    /// 技能伤害= 技能系数*(攻方普攻 + 攻方X系技能攻击-守方防御）
    /// </summary>
    /// <param name="skillAttFactor">技能系数</param>
    /// <param name="attackerPhyAtt">攻方普攻</param>
    /// <param name="defenderPhyDef">守方防御</param>
    /// <param name="skillBasicAtt">技能魔攻</param>
    /// <returns></returns>
    public static float GetSkillAttack(float skillAttFactor, float attackerPhyAtt, float defenderPhyDef, float skillBasicAtt)
    {
        skillAttFactor = Mathf.Max(0, skillAttFactor);
        float damage = skillAttFactor * (attackerPhyAtt + skillBasicAtt - defenderPhyDef);
        return  Mathf.Max(1.0F, damage);
    }

    /// <summary>
    /// 普攻暴击
    /// </summary>
    /// <param name="phyAttack"></param>
    /// <returns></returns>
    public static float GetPhysicalCrit(float phyAttack)
    {
        return phyAttack * 1.5f;
    }

    /// <summary>
    /// 技能暴击
    /// </summary>
    /// <param name="skillAttack"></param>
    /// <returns></returns>
    public static float GetSkillCrit(float skillAttack)
    {
        return skillAttack * 1.5f;
    }

    /// <summary>
    /// 普攻反弹 普攻反伤=ROUNDUP(普攻伤害*反伤加成,0)
    /// </summary>
    /// <param name="phyAttack">普攻伤害</param>
    /// <param name="backlashAddition">反伤加成</param>
    /// <returns></returns>
    public static uint GetBacklashPhyAttack(uint phyAttack, float backlashAddition)
    {
        backlashAddition = Mathf.Abs(backlashAddition);
        return (uint)Mathf.FloorToInt(phyAttack * backlashAddition +0.5F);
    }

    /// <summary>
    /// 技能反弹 技能反伤=ROUNDUP(技能伤害*反伤加成,0)
    /// </summary>
    /// <param name="skillAttack">技能伤害</param>
    /// <param name="backlashAddition">反伤加成</param>
    /// <returns></returns>
    public static uint GetBacklashSkillAttack(uint skillAttack, float backlashAddition)
    {
        backlashAddition = Mathf.Abs(backlashAddition);
        return (uint)Mathf.FloorToInt(skillAttack * backlashAddition +0.5F);
    }

    /// <summary>
    /// 物攻回血 吸血值=ROUNDUP(普攻伤害*吸血加成,0)
    /// </summary>
    /// <param name="phyAttack">普攻伤害</param>
    /// <param name="restoreAddition">回血加成</param>
    /// <returns></returns>
    public static uint GetPhyAttackRestoreHP(uint phyAttack, float restoreAddition)
    {
        restoreAddition = Mathf.Abs(restoreAddition);
        return (uint)Mathf.FloorToInt(phyAttack * restoreAddition  +0.5F);
    }

    /// <summary>
    /// 技能回血
    /// </summary>
    /// <param name="skillAttack"></param>
    /// <param name="restoreAddition"></param>
    /// <returns></returns>
    public static uint GetSkillAttackRestoreHP(uint skillAttack, float restoreAddition)
    {
        restoreAddition = Mathf.Abs(restoreAddition);
        return (uint)Mathf.FloorToInt(skillAttack * restoreAddition + 0.5F);
    }

    /// <summary>
    /// 暴击率
    /// </summary>
    /// <returns></returns>
    public static float GetCritRate(float minRate,float attackerCrit,float defenderToughness, float critFactor1, float critFactor2)
    {
        minRate = Mathf.Max(0, minRate);
        float denominator = attackerCrit + defenderToughness +critFactor1;
        float rate =0;
        if(denominator > 0)
         rate = (attackerCrit - defenderToughness) / denominator + critFactor2;
        return Mathf.Max(minRate, rate);
    }

    /// <summary>
    /// 命中率 攻方命中/（攻方命中+守方闪避）+命中系数
    /// </summary>
    /// <param name="attackerHit"></param>
    /// <param name="defenderDodge"></param>
    /// <param name="hitFactor"></param>
    /// <returns></returns>
    public static float GetHitRate(float attackerHit, float defenderDodge,float hitFactor)
    {
        attackerHit = Mathf.Max(0, attackerHit);
        float denominator = attackerHit + defenderDodge;
        if (denominator > 0)
            return attackerHit / denominator + hitFactor;
        else
            return 0;
    }

    /// <summary>
    /// 单次消除的能量
    /// </summary>
    /// <param name="chessBasicEnergy"></param>
    /// <param name="number"></param>
    /// <param name="comboAddition"></param>
    /// <returns></returns>
    public static uint GetEnergy(uint chessBasicEnergy, uint number, float comboAddition)
    {
        comboAddition = Mathf.Max(0, comboAddition);
        return (uint)  Mathf.FloorToInt(chessBasicEnergy * number * comboAddition +0.5F);
    }

}
