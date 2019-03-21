using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Assets.Script.Common;
using fogs.proto.msg;
#region 效果器基类
public class Effects
{
    public EffectInfo info;
    protected float num = 0.0f;
    protected RoleAttribute role;
    protected List<object> TargetList;
    protected List<KeyValuePair<RoleAttribute, HurtType>> DeleteList;
    public List<string> EffectNameList;
    public List<KeyValuePair<RoleAttribute, HurtType>> Deletelist
    {
        get
        {
            return this.DeleteList;
        }
    }
    protected int Level = 1;

    public virtual void initialize(EffectInfo info,int lv ,RoleAttribute role = null)
    {
        this.info = info;
        this.role = role;
        this.Level = lv;
        TargetList = new List<object>();
        DeleteList = new List<KeyValuePair<RoleAttribute, HurtType> >();
        this.EffectNameList = new List<string>(4);
        return; 
    }
    public virtual string getDescript() { return "Base Effect"; }
    public virtual bool IsContains() { return false; }
    public virtual List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        if (roleList == null)
            return new List<RoleAttribute>();
        List<RoleAttribute> result = new List<RoleAttribute>(roleList.Count + 1);
        for (int i = 0; i < roleList.Count;++i )
        {
            RoleAttribute temp = roleList[i];
            
            if(temp .EffectImmuneManage == null)
                result.Add(temp);
            else
            {
                if (temp.EffectImmuneManage.FilterEffect(this))
                    result.Add(temp);
                else
                    this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(temp, HurtType.Immune));
            }
        }
        return result;
    }
    public virtual bool DoEffect(PlayerData data)
    {
        return false;
    }
    public virtual bool DoEffect(Soldier soldier)
    {
        return false;
    }
    public virtual bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null)
            return false;

        if (this.info.effectType == 3 || this.info.effectType == 4)
        {
            if (this.Deletelist != null)
                this.Deletelist.Clear();
            return true;
        }

        foreach(KeyValuePair<RoleAttribute, HurtType> temp in this.DeleteList)
        {
            if (temp.Key != null)
                temp.Key.ReSetRoleHP(0, temp.Value);
        }
        if (this.Deletelist != null)
            this.DeleteList.Clear();
        return true;
    }
    public virtual bool DoEffect(List<GameObject> gameObjectList)
    {
        return false;
    }
    public virtual bool RevocateEffect()
    {
        return false;
    }
    public virtual bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.EffectNameList == null)
            return true;
        
        for (int i = 0; i < roleList.Count; ++i )
        {
            RoleAttribute tmpRole = roleList[i];
            if (tmpRole == null)
                continue;
            if (tmpRole.SpecilDepotManage == null)
                continue;
            for(int j = 0; j < this.EffectNameList.Count; ++j)
            {
                if (tmpRole.SpecilDepotManage == null)
                    continue;

                tmpRole.SpecilDepotManage.RemoveSpecilBuff(this.EffectNameList[j]);
            }
        }
        return true;
    }
    public virtual float WorthPrice()
    {
        return this.info.num + this.info.percent * 1000;
    }
    public void AddEffectName(string Name)
    {
        if (this.EffectNameList == null)
            this.EffectNameList = new List<string>(4);
        this.EffectNameList.Clear();
        this.EffectNameList.Add(Name);
    }
}
#endregion
#region 基础属性效果器101-113
public class BaseAttributeEffect : Effects
{
    public override string getDescript()
    {
        string descript = "Attribute Effect";
        return descript;
    }
    public override bool DoEffect(PlayerData data)
    {
        TargetList.Add(data);
        data.AddPlayerAttribute(this._TypeAnalysis(data._Attribute));
        return true;
    }
    public override bool DoEffect(Soldier soldier)
    {
        TargetList.Add(soldier);
        soldier.AddShowInfo(this._TypeAnalysis(soldier.showInfoSoldier));
        return true;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        base.DoEffect(roleList);
        foreach(RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;
            SkillTool.EffectPromt(temp,this.info.effectType,this.info.effectId);
            if(this.info.effectId == 102)
            {
                int hp_max = temp.basefightAttribute.HP;
                hp_max = SkillTool.NumCalculate(this.info.num, this.info.percent, hp_max);
                temp.ReSetMaxHP(hp_max);
                this.num += hp_max;
            }
            else if (this.info.effectId == 107)
            {
                int atk_interval = (int)(temp.basefightAttribute.AttRate * 1000.0f);
                atk_interval = SkillTool.NumCalculate(this.info.num, this.info.percent, atk_interval);
                this.num += atk_interval;
                temp.ReSetSkillValue(((float)this.num / 1000.0f));
            }
            else if (this.info.effectId == 108)
            {
                int speed = temp.basefightAttribute.MoveSpeed;
                speed = SkillTool.NumCalculate(this.info.num, this.info.percent, speed); this.num += speed;
                temp.ReSetMoveValue(speed);
            }
            else
                temp.RoleShowInfoAdd(this._TypeAnalysis(temp.basefightAttribute));
           TargetList.Add(temp);
        }
        return true;
    }
    public override bool RevocateEffect()
    {
        if (this.TargetList.Count == 0) return true;

        foreach(object temp in TargetList)
        {
            if((PlayerData)temp != null)
            {
                PlayerData tempData = (PlayerData)temp;
                tempData.AddPlayerAttribute(this._DeTypeAnalysis());
                continue;
            }
            if((Soldier)temp != null)
            {
                Soldier soldier = (Soldier)temp;
                soldier.AddShowInfo(this._DeTypeAnalysis());
                continue;
            }
            if((RoleAttribute)temp != null)
            {
                RoleAttribute role = temp as RoleAttribute;
                if(this.info.effectId == 102)
                {
                    role.ReSetMaxHP((int)-num);
                    this.num = 0;
                }
                else if (this.info.effectId == 107)
                {
                    role.ReSetSkillValue(-((float)this.num / 1000.0f));
                    this.num = 0;
                }
                else if (this.info.effectId == 108)
                {
                    role.ReSetMoveValue((int)-this.num);
                    this.num = 0;
                }
                else
                    role.RoleShowInfoAdd(this._DeTypeAnalysis());
                continue;
            }
        }
        this.num = 0;
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);
        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            if(this.info.effectId == 102)
            {
                temp.ReSetMaxHP((int)-num);
                this.num = 0;
            }
            else if (this.info.effectId == 107)
            {
                temp.ReSetSkillValue(-((float)this.num / 1000.0f));
                this.num = 0;
            }
            else if (this.info.effectId == 108)
            {
                temp.ReSetMoveValue((int)-this.num);
                this.num = 0;
            }
            else
                temp.RoleShowInfoAdd(this._DeTypeAnalysis());
        }
        this.num = 0;
        return true;
    }
    private fogs.proto.msg.Attribute _TypeAnalysis(ShowInfoBase destAtt)
    {
        fogs.proto.msg.Attribute att = new fogs.proto.msg.Attribute();
        if (this.info.effectId == 0) return att;
        switch (this.info.effectId)
        {
            case 101:att.phy_atk = destAtt.Attack; att.phy_atk = SkillTool.NumCalculate(this.info.num, this.info.percent, att.phy_atk); this.num += att.phy_atk; break;
            case 102:att.hp_max = destAtt.HP; att.hp_max = SkillTool.NumCalculate(this.info.num, this.info.percent, att.hp_max); this.num += att.hp_max; break;
            case 103:att.acc_rate = destAtt.Accuracy; att.acc_rate = SkillTool.NumCalculate(this.info.num, this.info.percent, att.acc_rate); this.num += att.acc_rate; break;
            case 104:att.ddg_rate = destAtt.Dodge; att.ddg_rate = SkillTool.NumCalculate(this.info.num, this.info.percent, att.ddg_rate); this.num += att.ddg_rate; break;
            case 105:att.crt_rate = destAtt.Crit; att.crt_rate = SkillTool.NumCalculate(this.info.num, this.info.percent, att.crt_rate); this.num += att.crt_rate; break;
            case 106:att.tnc_rate = destAtt.Tenacity; att.tnc_rate = SkillTool.NumCalculate(this.info.num, this.info.percent, att.tnc_rate); this.num += att.tnc_rate; break;
            case 107:att.atk_interval = (int)(destAtt.AttRate * 1000.0f); att.atk_interval = SkillTool.NumCalculate(this.info.num, this.info.percent, att.atk_interval); this.num += att.atk_interval; break;
            case 108:att.speed = destAtt.MoveSpeed; att.speed = SkillTool.NumCalculate(this.info.num, this.info.percent, att.speed); this.num += att.speed; break;
            case 109:att.hp_revert = destAtt.HPRecovery; att.hp_revert = SkillTool.NumCalculate(this.info.num, this.info.percent, att.hp_revert); this.num += att.hp_revert; break;
            case 110:att.energy_max = destAtt.EnergyRecovery; att.energy_max = SkillTool.NumCalculate(this.info.num, this.info.percent, att.energy_max); this.num += att.energy_max; break;
            case 111:att.energy_revert = destAtt.EnergyRecovery;  att.energy_revert = SkillTool.NumCalculate(this.info.num, this.info.percent, att.energy_revert); this.num += att.energy_revert; break;
            case 112:att.mp_max = destAtt.MPRecovery; att.mp_max = SkillTool.NumCalculate(this.info.num, this.info.percent, att.mp_max); this.num += att.mp_max; break;
            case 113: att.mp_revert = destAtt.MPRecovery; att.mp_revert = SkillTool.NumCalculate(this.info.num, this.info.percent, att.mp_revert); this.num += att.mp_revert; break;
        }
        return att;
    }
    private fogs.proto.msg.Attribute _DeTypeAnalysis()
    {
        fogs.proto.msg.Attribute att = new fogs.proto.msg.Attribute();
        if (this.info.effectId == 0) return att;
        switch (this.info.effectId)
        {
            case 101: att.phy_atk = -(int)this.num; break;
            case 102: att.hp_max = -(int)this.num; break;
            case 103: att.acc_rate = -(int)this.num; break;
            case 104: att.ddg_rate = -(int)this.num; break;
            case 105: att.crt_rate = -(int)this.num; break;
            case 106: att.tnc_rate = -(int)this.num; break;
            case 107: att.atk_interval = -(int)this.num; break;
            case 108: att.speed = -(int)this.num; break;
            case 109: att.hp_revert = -(int)this.num; break;
            case 110: att.energy_max = -(int)this.num; break;
            case 111: att.energy_revert = -(int)this.num; break;
            case 112: att.mp_max = -(int)this.num; break;
            case 113: att.mp_revert = -(int)this.num; break;
        }
        return att;
    }
}
#endregion
#region 特殊属性效果器201,202,205,206,207
public class SpecilAttributeEffect : Effects
{
    public int targetType = 1;
    public override string getDescript()
    {
        string descript = "SpecilAttributeEffect";
        return descript;
    }

    public override bool DoEffect(PlayerData data)
    {
        return true;
    }
    public override bool DoEffect(Soldier soldier)
    {
        return true;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;
            SkillTool.EffectPromt(temp, this.info.effectType, this.info.effectId);

            switch (this.info.effectId)
            {
                case 201: temp._Coeff_Skill_Passive = (temp._Coeff_Skill_Passive + 1) * (this.info.percent + 1); break;
                case 202: temp._Coeff_Skill_Hurt = (temp._Coeff_Skill_Hurt + 1) * (this.info.percent + 1); break;
                case 205: temp._Coeff_Trigger_Pro *= this.info.percent;break;
                case 206: temp._Num_MP_Cost += (int)this.info.num; temp._Coeff_Mp_Cost *= this.info.percent; break;
                case 207: temp.Num_Summon_Interval += (int)this.info.num; temp._Coeff_Summon_Interval *= this.info.percent; break;
            }
            TargetList.Add(temp);

        }
        this.num = this.info.percent;
        return true;
    }
    public override bool RevocateEffect()
    {
        if (this.TargetList.Count == 0) return true;

        foreach (object temp in TargetList)
        {
            if ((PlayerData)temp != null)
            {
                
                continue;
            }
            if ((Soldier)temp != null)
            {
               
                continue;

            }
            if ((RoleAttribute)temp != null)
            {
                RoleAttribute role = (RoleAttribute)temp;
                switch (this.info.effectId)
                {
                    case 201: role._Coeff_Skill_Passive /= (1 + this.info.percent); break;
                    case 202: role._Coeff_Skill_Hurt /= (1 + this.info.percent); break;
                    case 205: role._Coeff_Trigger_Pro /= this.info.percent; break;
                    case 206: role._Num_MP_Cost -= (int)this.info.num; role._Coeff_Mp_Cost /= this.info.percent; break;
                    case 207: role.Num_Summon_Interval -= (int)this.info.num; role._Coeff_Summon_Interval /= this.info.percent; break;
                }
                continue;
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            switch (this.info.effectId)
            {
                case 201: temp._Coeff_Skill_Passive /= (1 + this.info.percent); break;
                case 202: temp._Coeff_Skill_Hurt /= (1 + this.info.percent); break;
                case 205: temp._Coeff_Trigger_Pro /= this.info.percent; break;
                case 206: temp._Num_MP_Cost -= (int)this.info.num; temp._Coeff_Mp_Cost /= this.info.percent; break;
                case 207: temp.Num_Summon_Interval -= (int)this.info.num; temp._Coeff_Summon_Interval /= this.info.percent; break;
            }
        }
        return true;
    }
    public override float WorthPrice()
    {
        if (this.info.effectId == 205 || this.info.effectId == 206)
            return -this.info.percent * 100;
        if (this.info.effectId == 207)
            return -this.info.num - this.info.percent * 100;
        return base.WorthPrice();
    }

}
#endregion
#region 普攻减伤效果器 203
public class ReduceAttackEffect : Effects
{
    public override string getDescript()
    {
        string descript = "SpecilAttributeEffect";
        return descript;
    }
    public override bool DoEffect(PlayerData data)
    {
        return true;
    }
    public override bool DoEffect(Soldier soldier)
    {
        return true;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp._Num_Attack_Weaken += (int)this.info.num; 
            temp._Coeff_Attack_Weaken *= this.info.percent;
           TargetList.Add(temp);
           SkillTool.EffectPromt(temp, this.info.effectType, this.info.effectId);
        }
        return true;
    }
    public override bool RevocateEffect()
    {
        if (this.TargetList.Count == 0) return true;

        foreach (object temp in TargetList)
        {
            if ((PlayerData)temp != null)
            {
                continue;
            }
            if ((Soldier)temp != null)
            {
                continue;
            }
            if ((RoleAttribute)temp != null)
            {
                RoleAttribute role = (RoleAttribute)temp;
                role._Num_Attack_Weaken -= (int)this.info.num;
                if (this.info.percent != 0)
                    role._Coeff_Attack_Weaken /= this.info.percent;
                else
                    role._Coeff_Attack_Weaken = 1;
                continue;
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp._Num_Attack_Weaken -= (int)this.info.num;
            if (this.info.percent != 0)
                temp._Coeff_Attack_Weaken /= this.info.percent;
            else
                role._Coeff_Attack_Weaken = 1;
        }
        return true;
    }
    public override float WorthPrice()
    {
        int blood = 1000;
        if(this.role != null)
            blood = this.role.Get_MaxHPValue;

        return this.info.num + (float)blood / this.info.percent;
    }
}
#endregion
#region 特殊伤害效果器 801
/// <summary>
/// 技能伤害值=N1+敌方生命上限*N2
/// </summary>
public class SpecilAttackEffect : Effects
{
    public override string getDescript()
    {
        string descript = "AttackEffect";
        return descript;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.role == null) return false;
        if (this.info == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;

            float AttHurt = temp.Get_MaxHPValue * this.info.percent + this.info.num;
            if (this.info.durTime != 0 && this.info.interval != 0)
            {
                AttHurt = AttHurt * this.info.interval / this.info.durTime;
            }
            int hurt = SkillTool.CalculationSkillHurt(AttHurt, this.role, temp);
            if (hurt < 1)
                hurt = 1;
            temp.ReSetRoleHP(-hurt,HurtType.Normal);
        }
        return true;
    }
}
#endregion
#region 秒人效果器 802
/// <summary>
/// N2表示成功杀死目标的几率，N1==0参数无用
/// </summary>
public class SeckillEffect : Effects
{
    public override string getDescript()
    {
        string descript = "SeckillEffect";
        return descript;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;

            temp.ReSetRoleHP(-temp.Get_FightAttribute.HP, HurtType.Normal);
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));
                result.RemoveAt(i);
                continue;
            }
        }
        return result;
    }
}
#endregion
#region 技能减伤效果器 204
public class ReduceSkillEffect : Effects
{
    public override string getDescript()
    {
        string descript = "SpecilAttributeEffect";
        return descript;
    }

    public override bool DoEffect(PlayerData data)
    {
        return true;
    }
    public override bool DoEffect(Soldier soldier)
    {
        return true;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp._Num_Skill_Weaken += (int)this.info.num; temp._Coeff_Skill_Weaken *= this.info.percent;
            TargetList.Add(temp);
        }
        return true;
    }
    public override bool RevocateEffect()
    {
        if (this.TargetList.Count == 0) return true;

        foreach (object temp in TargetList)
        {
            if ((PlayerData)temp != null)
            {
                continue;
            }
            if ((Soldier)temp != null)
            {
                continue;
            }
            if ((RoleAttribute)temp != null)
            {
                RoleAttribute role = (RoleAttribute)temp;
                role._Num_Skill_Weaken -= (int)this.info.num;
                if (this.info.percent != 0)
                    role._Coeff_Skill_Weaken /= this.info.percent;
                else
                    role._Coeff_Skill_Weaken = 1;
                continue;
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp._Num_Skill_Weaken -= (int)this.info.num;
            if (this.info.percent != 0)
                temp._Coeff_Skill_Weaken /= this.info.percent;
            else
                temp._Coeff_Skill_Weaken = 1;
        }
        return true;
    }
    public override float WorthPrice()
    {
        int blood = 1000;
        if (this.role != null)
            blood = this.role.Get_MaxHPValue;

        return this.info.num - this.info.percent * blood;
    }

}
#endregion
#region 造成伤害效果器 301
/// <summary>
/// 技能总伤害值=N1+面板攻击*N2；每跳伤害=（技能总伤害值*每跳间隔）/持续时间
/// </summary>
public class AttackEffect : Effects
{
    public override string getDescript()
    {
        string descript = "AttackEffect";
        return descript;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.role == null) return false;
        if (this.info == null) return false;
        base.DoEffect(roleList);
        foreach (RoleAttribute temp in roleList)
        {
            if(temp == null) continue;

            float AttHurt = this.role.Get_FightAttribute.Attack * this.info.percent + this.info.num;
            if(this.info.durTime != 0 && this.info.interval != 0)
            {
                AttHurt = AttHurt * this.info.interval / this.info.durTime;
            }
            int hurt = SkillTool.CalculationSkillHurt(AttHurt,this.role,temp);
            if (hurt < 1)
                hurt = 1;
            temp.ReSetRoleHP(-hurt, HurtType.Normal);
        }
        return true;
    }
}
#endregion
#region 治疗效果器 401
/// <summary>
/// 技能总治疗值=N1+面板攻击*N2；每跳治疗=（技能总治疗值*每跳间隔）/持续时间
/// </summary>
public class CureEffect : Effects
{
    public override string getDescript()
    {
        string descript = "CureEffect";
        return descript;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.role == null) return false;
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;

            float AttHurt = this.role.Get_FightAttribute.Attack * this.info.percent + this.info.num;
            if (this.info.durTime != 0 && this.info.interval != 0)
            {
                AttHurt = AttHurt * this.info.interval / this.info.durTime;
            }
            if (AttHurt < 1)
                AttHurt = 1;
            temp.ReSetRoleHP((int)AttHurt, HurtType.Normal);
        }
        return true;
    }
}
#endregion
#region 击退效果器 503
public class RepelEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;
            temp.Repel(this.info.num);
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));
                result.RemoveAt(i);
            }
        }
        return result;
    }

}
#endregion
#region 净化效果器 601
public class CleanEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;
            if (temp.BuffManage == null) continue;
            temp.BuffManage.DeleteBuff((Effects effect) =>
            {
                if(effect != null && effect.info != null)
                {
                    if (effect.info.effectType == 2)
                        return true;
                }
                return false;
            });
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));

                result.RemoveAt(i);
            }
        }
        return result;
    }
}
#endregion
#region 驱散效果器 602
public class DispelEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;
            if (temp.BuffManage == null) continue;
            temp.BuffManage.DeleteBuff((Effects effect) =>
            {
                if (effect != null && effect.info != null)
                {
                    if (effect.info.effectType == 1)
                        return true;
                }
                return false;
            });
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                result.RemoveAt(i);
            }
        }
        return result;
    }

}
#endregion
#region 免疫BUFF效果器 701-707
public class ImmuneBuffEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;
            if (temp.BuffManage == null || temp.EffectImmuneManage == null)
                continue;

            switch(this.info.effectId)
            {
                case 701: temp.BuffManage.AddFilter(this.ImmuneBuff); temp.EffectImmuneManage.AddFilter(this.ImmuneBuff); break;
                case 702: temp.BuffManage.AddFilter(this.ImmuneDeBuff); temp.EffectImmuneManage.AddFilter(this.ImmuneDeBuff); break;
                case 703: temp.BuffManage.AddFilter(this.ImmuneDizziness); temp.EffectImmuneManage.AddFilter(this.ImmuneDizziness); break;
                case 704: temp.BuffManage.AddFilter(this.ImmuneFrozen); temp.EffectImmuneManage.AddFilter(this.ImmuneFrozen); break;
                case 705: temp.BuffManage.AddFilter(this.ImmuneRepel); temp.EffectImmuneManage.AddFilter(this.ImmuneRepel); break;
                case 706: temp.BuffManage.AddFilter(this.ImmuneSpecilAttribute); temp.EffectImmuneManage.AddFilter(this.ImmuneSpecilAttribute); break;
                case 707: temp.BuffManage.AddFilter(this.ImmuneAttack); temp.EffectImmuneManage.AddFilter(this.ImmuneAttack); break;
            }
            this.TargetList.Add(temp);
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;
            if (temp.BuffManage == null || temp.EffectImmuneManage == null)
                continue;

            switch (this.info.effectId)
            {
                case 701: temp.BuffManage.DeleteUnFilter(this.ImmuneBuff); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneBuff); break;
                case 702: temp.BuffManage.DeleteUnFilter(this.ImmuneDeBuff); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneDeBuff); break;
                case 703: temp.BuffManage.DeleteUnFilter(this.ImmuneDizziness); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneDizziness); break;
                case 704: temp.BuffManage.DeleteUnFilter(this.ImmuneFrozen); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneFrozen); break;
                case 705: temp.BuffManage.DeleteUnFilter(this.ImmuneRepel); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneRepel); break;
                case 706: temp.BuffManage.DeleteUnFilter(this.ImmuneSpecilAttribute); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneSpecilAttribute); break;
                case 707: temp.BuffManage.DeleteUnFilter(this.ImmuneAttack); temp.EffectImmuneManage.DeleteUnFilter(this.ImmuneAttack); break;
            }
        }
        return true;
    }
    bool ImmuneBuff(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectType != 1;
        }
        return true;
    }

    bool ImmuneDeBuff(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectType != 2;
        }
        return true;
    }

    bool ImmuneDizziness(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectId != 502;
        }
        return true;
    }

    bool ImmuneFrozen(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectId != 501;
        }
        return true;
    }

    bool ImmuneRepel(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectId != 503;
        }
        return true;
    }

    bool ImmuneSpecilAttribute(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectId != 205;
        }
        return true;
    }

    bool ImmuneAttack(Effects effect)
    {
        if (effect != null && effect.info != null)
        {
            return effect.info.effectId != 801;
        }
        return true;
    }

}
#endregion
#region 冰冻效果器 501
public class FrozenEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.SetPause();
            //temp.SetControled();
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.SetResume();
            //temp.SetUnControled();
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;
            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));

                result.RemoveAt(i);
            }
        }
        return result;
    }
}
#endregion
#region 眩晕效果器 502
public class DizzinessEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.SetPause();
            //temp.SetControled();
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.SetResume();
            //temp.SetUnControled();
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));

                result.RemoveAt(i);
            }
        }
        return result;
    }

}
#endregion
#region 能量增加效果器 803
/// <summary>
/// N1表示增加的能量数值；N2==0参数无用
/// </summary>
public class EnergyAddEffect : Effects
{
    public override string getDescript()
    {
        string descript = "EnergyAddEffect";
        return descript;
    }
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;
            if (temp.Get_RoleType != ERoleType.ertHero)
                continue;
            //RoleHero hero = temp as RoleHero;
            //if (hero == null) continue;
            CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_FightReSetPlayerEnergy,(int)this.info.num);
        }
        return true;
    }
}
#endregion
#region 吸收伤害效果器 804
public class ShieldEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.OpenShield(this, (int)this.info.num);
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null)
                continue;

            temp.CloseShield();
        }
        return true;
    }
}
#endregion
#region 增加光环半径效果器 805
public class HaloRangeEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (this.info.percent == 0)
                continue;
            if (temp == null)
                continue;

            temp._Coeff_Halo_Range *= this.info.percent;
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (this.info.percent == 0)
                continue;
            if (temp == null)
                continue;

            temp._Coeff_Halo_Range /= this.info.percent;
        }
        return true;
    }
}
#endregion
#region 隐身效果器 809
public class InVisbleEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if(temp != null)
            {
                SkillTool.EffectPromt(temp, this.info.effectType, this.info.effectId);
                temp.StartInvisible(1);
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
             if(temp != null)
            {
                temp.CloseInvisible();
            }
        }
        return true;
    }
}
#endregion
#region 策反效果器 810
public class SubvertedEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                temp.Subverted(this.info.durTime / 1000);
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.RevocateEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                temp.CloseSubverted();
            }
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));

                result.RemoveAt(i);
            }
        }
        return result;
    }
}
#endregion
#region 拉人效果器 811
public class PullEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                temp.Pull(this.role, this.info.num);
            }
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;
            if (!SkillTool.ProbMachine(this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));

                result.RemoveAt(i);
            }
        }
        return result;
    }
}
#endregion
#region 召唤效果器 812
public class SummonEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                //MonsterAttributeInfo tmpInfo
                Soldier tmpSoldier = Soldier.createByID((uint)this.info.percent);
                if (tmpSoldier == null)
                    return false;

                tmpSoldier.Level = this.Level;
                tmpSoldier.ShowInfoAdd(this.Level);
                ERoleType tmpRoleType = ERoleType.ertSoldier;
                if (temp.Get_RoleCamp == EFightCamp.efcEnemy)
                    tmpRoleType = ERoleType.ertMonster;
                for (int i = 0; i < this.info.num; ++i)
                {
                    //RoleManager.Instance.CreateSingleRole(tmpSoldier.showInfoSoldier, 0, temp.Get_RolePathIndex + (SkillTool.ProbMachine(0.5f) ? 1 : -1), 
                    //    tmpRoleType, (int)temp.transform.localPosition.x, EHeroGender.ehgNone, temp.Get_RoleCamp, temp.Get_FightType, null,
                    //    (obj) =>
                    //    {
                    //        obj.Summon(this.info.durTime / 1000);
                    //    });
                    CommandManager.Instance.SendSingleCommand(MessageID.Message_Fight.FM_RoleCreate, new CData_CreateRole(tmpSoldier.showInfoSoldier, 0,
                        temp.Get_RolePathIndex + (SkillTool.ProbMachine(0.5f) ? 1 : -1), tmpRoleType, EHeroGender.ehgNone, temp.Get_RoleCamp,
                        temp.Get_FightType, (int)temp.transform.localPosition.x, null, (obj) => { obj.Summon(this.info.durTime / 1000); }));
                }
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);

        return true;
    }
}
#endregion
#region 改变武器特效效果器 813
public class ChangeEquipEFEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);
        for (int i = 0; i < roleList.Count; ++i)
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null || tmp.Get_MainSpine == null)
                continue;
            tmp.Get_MainSpine.SetEquipEffectType((int)this.info.num);
            Skill sk = Skill.createByID((uint)(this.info.percent));
            sk.Strong(this.Level);
            tmp.StartAttackToSkill(sk);
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);
        for (int i = 0; i < roleList.Count; ++i)
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null || tmp.Get_MainSpine == null)
                continue;
            tmp.Get_MainSpine.SetEquipEffectType(1);
            tmp.EndAttackToSkill();
        }
        return true;
    }
}
#endregion
#region 受到的伤害转治疗效果器 814
public class HurtToCureEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);
        for (int i = 0; i < roleList.Count;++i )
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null)
                continue;
            tmp.ReSetRoleHPEvent -= OnReSetRoleHPEvent;
            tmp.ReSetRoleHPEvent += OnReSetRoleHPEvent;
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);
        for (int i = 0; i < roleList.Count; ++i)
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null)
                continue;
            tmp.ReSetRoleHPEvent -= OnReSetRoleHPEvent;
        }
        return true;
    }
    public void OnReSetRoleHPEvent(int vValue, HurtType vType, RoleAttribute vTarget)
    {
        if (vTarget == null)
            return;
        if(vValue < 0)
        {
            vTarget.ReSetRoleHP((int)(-vValue * this.info.percent + this.info.num),HurtType.none);
        }
    }
}
#endregion
#region 变羊效果器 815
public class TurnToSheepEFEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);
        for (int i = 0; i < roleList.Count; ++i)
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null)
                continue;
            SoldierAttributeInfo tmpInfo = ConfigManager.Instance.mSoldierData.FindById((uint)this.info.num);
            if (tmpInfo == null)
                return false;
            tmp.StartChangeSkeletonDataAsset(tmpInfo.Animation);
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);
        for (int i = 0; i < roleList.Count; ++i)
        {
            RoleAttribute tmp = roleList[i];
            if (tmp == null)
                continue;
            tmp.CloseChangeSkeletonDataAsset();
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        List<RoleAttribute> result = base.IsCanActive(roleList);
        for (int i = result.Count - 1; i >= 0; --i)
        {
            if (result[i] == null)
                continue;

            if (!SkillTool.LevelSuppress(this.Level, result[i].Get_RoleLevel, this.info.percent))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(result[i], HurtType.Resist));
                result.RemoveAt(i);
            }
        }
        return result;
    }
}
#endregion
#region 召唤联动宠物 816
public class SummonPetEffect : Effects
{
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;
        if (roleList == null) return false;
        base.DoEffect(roleList);

        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                temp.SummonPet((int)this.info.num, (int)(this.info.percent));
            }
        }
        return true;
    }
    public override bool RevocateEffect(List<RoleAttribute> roleList)
    {
        base.RevocateEffect(roleList);
        foreach (RoleAttribute temp in roleList)
        {
            if (temp != null)
            {
                temp.RecyclePet();
            }
        }
        return true;
    }
}
#endregion
#region buff效果器
public class BuffEffect : Effects
{
    private List<RoleAttribute> OutRoleList = null;
    public override bool DoEffect(List<RoleAttribute> roleList)
    {
        if (this.info == null) return false;

        base.DoEffect(roleList);
        foreach (RoleAttribute temp in roleList)
        {
            if (temp == null) continue;
            if (temp.BuffManage == null)
                continue;

            Effects tempEffect = SkillTool.EffectCreate(this.info, this.Level, this.role, false);
            if (tempEffect.EffectNameList == null)
                tempEffect.EffectNameList = new List<string>();
            if (this.EffectNameList != null)
                tempEffect.EffectNameList.AddRange(this.EffectNameList);
            Buff tempBuff = new Buff();
            tempBuff.Initialize(this.info.durTime, this.info.interval, tempEffect, temp);
            if (temp.BuffManage.AddBuff(tempBuff))
                tempBuff.Activate();
        }
        return true;
    }
    public override List<RoleAttribute> IsCanActive(List<RoleAttribute> roleList)
    {
        Effects tempEffect = SkillTool.EffectCreate(this.info, this.Level, this.role, false);
        
        this.OutRoleList = tempEffect.IsCanActive(roleList);
        this.DeleteList.AddRange(tempEffect.Deletelist);
        if (this.OutRoleList.Count <= 0)
        {
            return this.OutRoleList;
        }

        Buff tempBuff = new Buff();
        tempBuff.Initialize(this.info.durTime, this.info.interval, tempEffect, null);

        for (int i = this.OutRoleList.Count - 1; i >= 0;--i )
        {
            RoleAttribute temp = this.OutRoleList[i];
            if (temp.BuffManage == null)
                continue;

            if (!temp.BuffManage.TextCanAdd(tempBuff))
            {
                this.DeleteList.Add(new KeyValuePair<RoleAttribute, HurtType>(temp, HurtType.Immune));
                OutRoleList.RemoveAt(i);
            }
            else
            {
                if (!temp.BuffManage.DepotFilter(tempBuff.effect))
                    OutRoleList.RemoveAt(i);
            }
        }
        return this.OutRoleList;
    }
    
}
#endregion
#region 技能效果器函数工具
public class SkillTool
{
    /// <summary>
    /// N1和N2的计算
    /// </summary>
    /// <param name="info.num"></param>
    /// <param name="info.percent"></param>
    /// <param name="dest"></param>
    /// <returns></returns>
    public static int NumCalculate(float num,float percent,int dest)
    {
        return (int)(num + dest * (percent - 1));
    }
    /// <summary>
    /// 目标选取返回描述
    /// </summary>
    /// <param name="targetType"></param>
    /// <returns></returns>
    public static string TargetTypeDescript(int targetType)
    {
        string result = "";
        switch (targetType)
        {
            case 2: result = ConstString.TargetType2; break;
            case 3: result = ConstString.TargetType3; break;
        }
        return result;
    }

    /// <summary>
    /// 技能伤害计算公式
    /// 技能伤害=（攻方技能攻击*技能伤害系数*（1+技能加成N2）*克制关系系数）* (受击放技能减伤系数) +　受击方技能减伤数值
    /// </summary>
    /// <param name="vSkillID">技能配置表ID</param>
    /// <param name="vEnemy">敌方角色</param>
    public static int CalculationSkillHurt(float hurt, RoleAttribute target, RoleAttribute vEnemy)
    {
        float _SkillHurt = 0;
        if (target == null)
            return 0;
        if (vEnemy == null)
            return 0;
        //计算技能伤害//

        _SkillHurt = hurt * target._Coeff_Skill_Hurt * (1 + target._Add_Skill_Hurt) *(vEnemy._Coeff_Skill_Weaken) + vEnemy._Num_Skill_Weaken;

        return (int)_SkillHurt;
    }
    /// <summary>
    /// 概率计算机器（计算是否随机到需要的概率之内）
    /// </summary>
    /// <returns>概率</returns>
    public static bool ProbMachine(double prob)
    {
        if (prob <= 0.0) return false;
        if (prob >= 1.0) return true;

        //string temp = prob.ToString();
        //string[] anyLevel = Regex.Split(temp, ".");
        //int count = 0;
        //if (anyLevel.Length == 2)
        //{
        //    count = anyLevel[1].Length;
        //}
        return (double)UnityEngine.Random.Range(0.0f, 1.0f) <= prob;
    }
    /// <summary>
    /// 效果器创建工具
    /// </summary>
    /// <param name="info">效果器配置表数据</param>
    /// <param name="role">发起者</param>
    /// <returns></returns>
    public static Effects EffectCreate(EffectInfo info,int lv,RoleAttribute role = null,bool isBuff = true)
    {
        Effects effect = new Effects();
        if(info == null) return effect;

        int id = info.effectId;
        if (info.durTime > 0 && isBuff)
        {
            effect = new BuffEffect();
        }
        else if(id >= 101 && id <= 113)
        {
            effect = new BaseAttributeEffect();
        }
        else if (id == 201 || id == 201 || id == 202 || id == 205 || id == 206 || id == 207)
        {
            effect = new SpecilAttributeEffect();
        }
        else if(id == 203)
        {
            effect = new ReduceAttackEffect();
        }
        else if(id == 801)
        {
            effect = new SpecilAttackEffect();
        }
        else if(id == 802)
        {
            effect = new SeckillEffect();
        }
        else if(id == 204)
        {
            effect = new ReduceSkillEffect();
        }
        else if(id == 301)
        {
            effect = new AttackEffect();
        }
        else if(id == 401)
        {
            effect = new CureEffect();
        }
        else if(id == 501)
        {
            effect = new FrozenEffect();
        }
        else if (id == 502)
        {
            effect = new DizzinessEffect();
        }
        else if(id == 503)
        {
            effect = new RepelEffect();
        }
        else if(id == 601)
        {
            effect = new CleanEffect();
        }
        else if(id == 602)
        {
            effect = new DispelEffect();
        }
        else if (id >= 701 && id <= 707)
        {
            effect = new ImmuneBuffEffect();
        }
        else if (id == 803)
        {
            effect = new EnergyAddEffect();
        }
        else if(id == 804)
        {
            effect = new ShieldEffect();
        }
        else if(id == 805)
        {
            effect = new HaloRangeEffect();
        }
        else if(id == 809)
        {
            effect = new InVisbleEffect();
        }
        else if (id == 810)
        {
            effect = new SubvertedEffect();
        }
        else if (id == 811)
        {
            effect = new PullEffect();
        }
        else if (id == 812)
        {
            effect = new SummonEffect();
        }
        else if(id == 813)
        {
            effect = new ChangeEquipEFEffect();
        }
        else if (id == 814)
        {
            effect = new HurtToCureEffect();
        }
        else if (id == 815)
        {
            effect = new TurnToSheepEFEffect();
        }
        else if(id == 816)
        {
            effect = new SummonPetEffect();
        }
        effect.initialize(info,lv, role);
        return effect;
    }
    public static Vector3 GetWorldPosition(RoleAttribute role, int type)
    {
        SkeletonAnimation temp = role.gameObject.GetComponent<SkeletonAnimation>();
        if (temp == null) return role.transform.position;
        Spine.Bone bone = null;
        switch (type)
        {
            case 1: bone = temp.skeleton.FindBone("bone_hp"); break;
            case 2: bone = temp.skeleton.FindBone("point_cast"); break;
            case 3: bone = temp.skeleton.FindBone("point_projectile"); break;
            case 4: bone = temp.skeleton.FindBone("root"); break;
            case 5: bone = temp.skeleton.FindBone("bone_effect1"); break;
            case 6: bone = temp.skeleton.FindBone("bone_effect2"); break;
            default: bone = null; break;
        }
        if (bone == null) return temp.transform.position;
        if (SceneManager.Instance.Get_CurScene == null) return temp.transform.position;
        float CurSceneX = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
        float CurSceneY = SceneManager.Instance.Get_CurScene .Get_ScreenProportion_Y;
        float boneX = bone.worldX;
        if (role.transform != null)
        {
            if (role.transform.rotation.y == 1 || role.transform.rotation.y == -1)
                boneX = -boneX;
        }

        float x = (boneX / CurSceneX) * role.transform.lossyScale.x * CurSceneX;
        float y = (bone.y / CurSceneY) * role.transform.lossyScale.y * CurSceneY;
        Vector3 pos = new Vector3(role.transform.position.x + x, role.transform.position.y + y, role.transform.position.z);
        return pos;
    }
    public static Vector3 GetBonePosition(RoleAttribute role, int type)
    {
        if (role == null) return new Vector3(0, 0, 0);
        SkeletonAnimation temp = role.gameObject.GetComponent<SkeletonAnimation>();
        if (temp == null) return new Vector3(0, 0, 0);
        Spine.Bone bone = null;
        switch (type)
        {
            case 1: bone = temp.skeleton.FindBone("bone_hp"); break;
            case 2: bone = temp.skeleton.FindBone("point_cast"); break;
            case 3: bone = temp.skeleton.FindBone("point_projectile"); break;
            case 4: bone = temp.skeleton.FindBone("root"); break;
            case 5: bone = temp.skeleton.FindBone("bone_effect1"); break;
            case 6: bone = temp.skeleton.FindBone("bone_effect2"); break;
            default: bone = null; break;
        }
        if (bone == null) return new Vector3(0, 0, 0);

        Vector3 pos = new Vector3(bone.x, bone.y, 0);
        return pos;
    }
    public static float GetLocalScare(RoleAttribute role)
    {
        if (role == null)
            return 1.2f;
        if (RoleManager.Instance.Get_Hero == null)
            return 1.2f;
        if (role.Get_MainSpine == null)
            return 1.2f;
        if (RoleManager.Instance.Get_Hero.Get_MainSpine == null)
            return 1.2f;

        Vector3 heroSize = RoleManager.Instance.Get_Hero.Get_MainSpine.GetStartSize();
        if (heroSize == Vector3.zero)
            return 1.2f;
        Vector3 roleSize = role.Get_MainSpine.GetStartSize();
        if (roleSize == Vector3.zero)
            return 1.2f;

        float scaleX = roleSize.x / heroSize.x;
        float scaleY = roleSize.y / heroSize.y;
        float scaleResult = (scaleX + scaleY) / 2.0f;
        if (scaleX == 1.0f)
            return scaleResult * 1.2f;
        else
            return scaleResult * 1.6f;
    }
    //N2-(目标等级-技能等级)*N2/(N2+8), 成功率最高为1，最低为0
    public static bool LevelSuppress(int vSkillLv, int vDestLv, float vN2)
    {
        float probability = vN2 - (vDestLv - vSkillLv) * vN2 / (vN2 + 30);
        
        return ProbMachine(probability);
    }
    public static void EffectPromt(RoleAttribute vRole,int effectType,int effectId)
    {
        if (vRole == null)
            return;
        if (vRole.pRpleHP == null)
            return;

        EffectPromtType type = EffectPromtType.none;
        if (effectType == 1)
            type = EffectPromtType.Buff;
        if (effectType == 2)
            type = EffectPromtType.DeBuff;
        string tmpMessage = "";
        switch (effectId)
        {
            case 101: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_101; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_101; break;
            case 102: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_102; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_102; break;
            case 103: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_103; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_103; break;
            case 104: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_104; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_104; break;
            case 105: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_105; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_105; break;
            case 106: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_106; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_106; break;
            case 107: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_107; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_107; break;
            case 108: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_108; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_108; break;
            case 201: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_201; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_201; break;
            case 202: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_202; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_202; break;
            case 203: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_203; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_DEBUFF_203; break;
            case 809: if (type == EffectPromtType.Buff) tmpMessage = ConstString.SKILL_BUFF_809; if (type == EffectPromtType.DeBuff) tmpMessage = ConstString.SKILL_BUFF_809; break;
        }
        if (vRole.pRpleHP != null && !tmpMessage.Equals(""))
        {
            vRole.pRpleHP.RfreshEffectPromt(tmpMessage, type);
        }
    }
}
#endregion
#region 目标锁定
/// <summary>
/// 效果器目标锁定
/// </summary>
public class TargetLockon
{
    //技能发起者
    protected Soldier soldier = null;
    protected PlayerData player = null;
    protected RoleAttribute role = null;
    public TargetLockonInfo info = null;
    public bool isFollow = false;
    public float duringTime;
    public object Target = null;
    public void initialize(TargetLockonInfo info, bool isFollow = false, float duringTime = 0.0f)
    {
        this.info = info;
        this.isFollow = isFollow;
        this.duringTime = duringTime;
    }
    public bool IsEmpty()
    {
        if (soldier != null) return false;
        if (player != null) return false;
        if (role != null) return false;
        return true;
    }
    public void SetResource(Soldier soldier)
    {
        this.soldier = soldier;
        this.Target = soldier;
    }
    public void SetResource(PlayerData player)
    {
        this.player = player;
        this.Target = player;
    }
    public void SetResource(RoleAttribute role)
    {
        this.role = role;
        this.LockonTarget();
    }
    public void LockonTarget()
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            return;
        }
        float tmpScreenProportion_X = SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;

        float minDistance = this.info.minDistance;
        float maxDistance = this.info.maxDistance;

        Vector3 starPos = new Vector3();
        Vector3 endPos = new Vector3();

        if (this.role != null)
        {
            Transform rolePos = this.role.transform;
            starPos.y = 0;
            starPos.z = 0;
            endPos.y = 0;
            endPos.z = 0;
            if (this.role.Get_Direction == ERoleDirection.erdRight)
            {
                starPos.x = rolePos.position.x + minDistance / tmpScreenProportion_X;
                endPos.x = rolePos.position.x + maxDistance / tmpScreenProportion_X;
            }
            else
            {
                starPos.x = rolePos.position.x - maxDistance / tmpScreenProportion_X;
                endPos.x = rolePos.position.x - minDistance / tmpScreenProportion_X;
            }
        }
        List<RoleAttribute> tempList = null;
        if(starPos != endPos)
        {
            if (this.role.Get_Direction == ERoleDirection.erdRight)
            {
                tempList = CommonFunction.FindHitFightObjects(starPos, endPos, ERoleDirection.erdRight);
            }
            else
            {
                tempList = CommonFunction.FindHitFightObjects(endPos, starPos, ERoleDirection.erdLeft);
            }
        }
        else
        {
            tempList = new List<RoleAttribute>(2);
            if (this.role != null)
                tempList.Add(this.role);
        }

        for (int i = tempList.Count - 1; i >= 0; --i)
        {
            if (tempList[i].Get_RoleType == ERoleType.ertBarracks || tempList[i].Get_RoleType == ERoleType.ertTransfer)
            {
                tempList.RemoveAt(i);
            }
        }
        List<RoleAttribute> list = this.CampFilter(this.role, tempList, this.info.targetType);
        RoleAttribute role = this.TargerModule(this.role, list, this.info.targetMoudle);
        this.Target = role;
    }
    public object GetLockonTarget()
    {
        return this.Target;
    }
    public List<RoleAttribute> CampFilter(RoleAttribute resource, List<RoleAttribute> roleList, int i)
    {
        List<RoleAttribute> list = null;
        if (resource.Get_IsSubverted)
        {
            if (i == 2)
                i = 3;
            else if (i == 3)
                i = 2;
        }
        switch (i)
        {
            case 1: list = roleList.FindAll((RoleAttribute r) => { if (r == null) return false; return r == resource; }); break;
            case 2: list = roleList.FindAll((RoleAttribute r) => { if (r == null) return false; return r.Get_RoleCamp != resource.Get_RoleCamp; }); break;
            case 3: list = roleList.FindAll((RoleAttribute r) => { if (r == null) return false; return r.Get_RoleCamp == resource.Get_RoleCamp; ; }); break;
        }
        return list;
    }
    public RoleAttribute TargerModule(RoleAttribute resource, List<RoleAttribute> roleList, int i)
    {
        RoleAttribute role = null;
        if (roleList == null)
            return role;
        switch (i)
        {
            case 0: if (roleList.Count > 0) role = roleList[0]; break;
            case 1: role = TheMinHpFilter(roleList); break;
            case 2: role = TheMaxHpFilter(roleList); break;
            case 3: role = TheMinDistanceFilter(resource, roleList); break;
            case 4: role = TheMaxDistanceFilter(resource, roleList); break;
        }
        return role;
    }
    public RoleAttribute TheMinHpFilter(List<RoleAttribute> roleList)
    {
        if (roleList == null || roleList.Count <= 0) return null;
        RoleAttribute role = roleList[0];
        float roleHpPercent = (float)(role.Get_FightAttribute.HP) / (float)(role.Get_MaxHPValue);
        foreach (RoleAttribute ro in roleList)
        {
            float roHpPercent = (float)(ro.Get_FightAttribute.HP) / (float)(ro.Get_MaxHPValue);
            if (roHpPercent < roleHpPercent)
            {
                role = ro;
                roleHpPercent = roHpPercent;
            }
        }
        return role;
    }
    public RoleAttribute TheMaxHpFilter(List<RoleAttribute> roleList)
    {
        if (roleList == null || roleList.Count <= 0) return null;
        RoleAttribute role = roleList[0];
        float roleHpPercent = (float)(role.Get_FightAttribute.HP) / (float)(role.Get_MaxHPValue);
        foreach (RoleAttribute ro in roleList)
        {
            float roHpPercent = (float)(ro.Get_FightAttribute.HP) / (float)(ro.Get_MaxHPValue);
            if (roHpPercent > roleHpPercent)
            {
                role = ro;
                roleHpPercent = roHpPercent;
            }
        }
        return role;
    }
    public RoleAttribute TheMinDistanceFilter(RoleAttribute resource, List<RoleAttribute> roleList)
    {
        if (roleList == null || roleList.Count <= 0) return null;
        RoleAttribute role = roleList[0];
        foreach (RoleAttribute ro in roleList)
        {
            if (Math.Abs(ro.gameObject.transform.position.x - resource.transform.position.x)
                <
                Math.Abs(role.gameObject.transform.position.x - resource.transform.position.x))
            {
                role = ro;
            }
        }
        return role;
    }
    public RoleAttribute TheMaxDistanceFilter(RoleAttribute resource, List<RoleAttribute> roleList)
    {
        if (roleList == null || roleList.Count <= 0) return null;
        RoleAttribute role = roleList[0];
        foreach (RoleAttribute ro in roleList)
        {
            if (Math.Abs(ro.gameObject.transform.position.x - resource.transform.position.x)
                >
                Math.Abs(role.gameObject.transform.position.x - resource.transform.position.x))
            {
                role = ro;
            }
        }
        return role;
    }
}
#endregion
#region 效果器目标查找
public class TargetFind
{
    protected Soldier soldier = null;
    protected PlayerData player = null;
    protected RoleAttribute role = null;
    public RoleAttribute resource = null;
    public SkillEffectInfo skillEffectInfo = null;

    protected TargetFindInfo targetFindInfo = null;

    protected List<EffectInfo> effectInfoList = null;
    protected List<SkillSpecilInfo> skillSpecilInfoList = null;
    /// <summary>
    /// 所有的目标
    /// </summary>
    protected List<RoleAttribute> targetList = null;
    /// <summary>
    /// 新加进来的目标
    /// </summary>
    protected List<RoleAttribute> newTargetList = null;
    /// <summary>
    /// 刚出范围的目标
    /// </summary>
    protected List<RoleAttribute> oldTargetList = null;
    /// <summary>
    /// 真正需要效果器作用的目标
    /// </summary>
    protected List<RoleAttribute> applyTargetList = null;
    protected List<RoleAttribute> specilList = null;

    protected List<SpecilEffect> specildEffectList = null;

    protected List<EffectManage> effectList = null;

    protected float currTime = 0.0f;
    public Vector3 destWorldPosition;
    public Vector3 sencePosition;
    public EFightCamp _roleCamp = EFightCamp.efcNone;
    public float RangeScale = 1.0f;

    public void initialize(SkillEffectInfo skillEffectInfo,RoleAttribute role = null)
    {
        this.resource = role;
        this.skillEffectInfo = skillEffectInfo;
        this.skillSpecilInfoList = skillEffectInfo.specilInfoList;
        this.targetFindInfo = skillEffectInfo.targetFindInfo;
        this.specildEffectList = new List<SpecilEffect>(this.skillSpecilInfoList.Count + 1);
        this.targetList = new List<RoleAttribute>(this.targetFindInfo.maxTarget + 1);
        this.newTargetList = new List<RoleAttribute>(this.targetFindInfo.maxTarget + 1);
        this.oldTargetList = new List<RoleAttribute>(this.targetFindInfo.maxTarget + 1);
        foreach (SkillSpecilInfo temp in this.skillSpecilInfoList)
        {
            SpecilEffect SE = new SpecilEffect();
            SE.initialize(temp,this.resource);
            this.specildEffectList.Add(SE);
        }
        if(this.targetFindInfo.frequencyTime == 0)
        {
            this.applyTargetList = this.newTargetList;
        }
        else
        {
            this.applyTargetList = this.targetList;
        }
        this.specilList = new List<RoleAttribute>();
    }
    public bool IsEmpty()
    {
        if (soldier != null) return false;
        if (player != null) return false;
        if (role != null) return false;
        return true;
    }
    public void SetEffectInfoList(List<EffectInfo> effectInfoList,int lv)
    {
        this.effectInfoList = effectInfoList;
        this.effectList = new List<EffectManage>(this.effectInfoList.Count + 1);
        for (int i = 0; i < this.effectInfoList.Count; ++i)
        {
            if (i >= this.skillEffectInfo.effect.Count)
                continue;

            EffectManage tempManage = new EffectManage();

            tempManage.init(this.skillEffectInfo.effect[i].Value, this.effectInfoList[i], lv, this.resource);
            this.effectList.Add(tempManage);
        }
    }
    public void SetSkillAttribute(SkillAttributeInfo att)
    {
        if(att.triggerType == 3)
        {
            if (this.resource != null)
                this.RangeScale = this.resource._Coeff_Halo_Range;
        }
    }
    public void SetResource(Soldier soldier)
    {
        if (!this.IsEmpty()) return;

        this.soldier = soldier;
        Activate();
    }
    public void SetResource(PlayerData player)
    {
        if (!this.IsEmpty()) return;

        this.player = player;
        Activate();
    }
    public void SetResource(RoleAttribute role)
    {
        if (role == null) return;
        this.role = role;
        this.destWorldPosition = this.role.transform.position;
        if (SceneManager.Instance.Get_CurScene != null && SceneManager.Instance.Get_CurScene.transOther != null)
            this.sencePosition = SceneManager.Instance.Get_CurScene.transOther.position;
        this._roleCamp = role.Get_RoleCamp;
        Activate();
    }
    public void SetResource(Vector3 pos, EFightCamp roleCamp)
    {
        this.destWorldPosition = pos;
        this._roleCamp = roleCamp;
        if (SceneManager.Instance.Get_CurScene != null && SceneManager.Instance.Get_CurScene.transOther != null)
            this.sencePosition = SceneManager.Instance.Get_CurScene.transOther.position;
        Activate();
    }
    public void Activate()
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            Close();
            return;
        }
        if (this.targetFindInfo.followTarget == 1)
        {
            UpdateTimeTool.Instance.RemoveUpdator(this.TargetFindUpdate);
            UpdateTimeTool.Instance.AddUpdator(this.TargetFindUpdate);
        }
        if (this.targetFindInfo.durTime > 0)
        {
            int frequnce = this.targetFindInfo.frequencyTime;
            if (frequnce == 0)
                frequnce = 500;
            RemoveEffectUpDate();
            UpdateTimeTool.Instance.AddTimer((float)frequnce / 1000.0f, true, this.EffectUpDate);
            UpdateTimeTool.Instance.AddTimer((float)this.targetFindInfo.durTime / 1000.0f, false, this.RemoveEffectUpDate);
        }
        FindTarget();
        this.DoTarget();
        if (this.targetFindInfo.durTime == 0)
            this.Close();
    }
    public List<RoleAttribute> FindTarget()
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            Close();
            return null;
        }
        float width = (float)this.targetFindInfo.rang / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;

        List<RoleAttribute> list = new List<RoleAttribute>();
        if (this.targetFindInfo.rang != 0)
        {
            float rang = (float)this.targetFindInfo.rang / SceneManager.Instance.Get_CurScene.Get_ScreenProportion_X;
            if(this.targetFindInfo.followTarget == 0)
            {
                if (SceneManager.Instance.Get_CurScene != null && SceneManager.Instance.Get_CurScene.transOther != null)
                {
                    destWorldPosition += (SceneManager.Instance.Get_CurScene.transOther.position - this.sencePosition);
                    this.sencePosition = SceneManager.Instance.Get_CurScene.transOther.position;
                }
            }
            list = CommonFunction.FindHitFightObjects(destWorldPosition, rang * this.RangeScale, this._roleCamp);
        }
        else
        {
            list = new List<RoleAttribute>(2);
            if (this.role != null)
                list.Add(this.role);
        }
        List<RoleAttribute> DeleteList = new List<RoleAttribute>();
        for (int i = list.Count - 1; i >= 0; --i)
        {
            if (list[i] == null)
                continue;

            if (list[i].Get_RoleType == ERoleType.ertBarracks || list[i].Get_RoleType == ERoleType.ertTransfer || !TargerModule(list[i]))
            {
                DeleteList.Add(list[i]);
            }
        }
        for (int i = 0; i < DeleteList.Count; ++i )
        {
            list.Remove(DeleteList[i]);
        }
        DeleteList = null;
        list.RemoveAll((role) =>
        {
            if (role == null)
                return true;
            //if (!this.IsFilter(role))
            //    return true;
            return false;
        });

        list.Sort((RoleAttribute left, RoleAttribute right) =>
        {
            float leftDistance = Math.Abs(left.transform.position.x - this.destWorldPosition.x);
            float rightDistance = Math.Abs(right.transform.position.x - this.destWorldPosition.x);
            if (leftDistance != rightDistance)
            {
                if (leftDistance < rightDistance)
                    return -1;
                else
                    return 1;
            }
            return 0;
        });
        int maxCount = this.targetFindInfo.maxTarget;

        this.oldTargetList.Capacity = list.Count;
        this.newTargetList.Capacity = list.Count;

        this.targetList.RemoveAll((role) => 
        {
            if (role == null)
                return true;
            //if (!this.IsFilter(role))
            //{
            //    this.oldTargetList.Add(role);
            //    return true;
            //}
            if (!list.Contains(role))
            {
                this.oldTargetList.Add(role);
                return true;
            }
            return false;
        });

        for (int i = 0; i < list.Count;++i )
        {
            if(this.targetList.Count >= maxCount)
            {
                return this.targetList;
            }
            if (targetList.Contains(list[i]))
                continue;
            this.targetList.Add(list[i]);
            this.newTargetList.Add(list[i]);
        }
        return this.targetList;
    }
    private bool TargerModule(RoleAttribute vRole)
    {
        switch(this.targetFindInfo.FindModel)
        {
            case 0: return true;
            case 1: return DeleteLockTarget(vRole);
        }
        return true;
    }
    private bool DeleteLockTarget(RoleAttribute vRole)
    {
        if (vRole != null && this.role != null)
            return !vRole.Equals(this.role);
        return true;
    }
    public void DoTarget()
    {
        this.DoEffect();
        this.DoSpecil();
        this.RemoveEffect(this.oldTargetList);
        this.newTargetList.Clear();
        this.oldTargetList.Clear();
    }
    private void DoSpecil()
    {
        if (this.targetList == null) return;
        foreach (SpecilEffect temp in this.specildEffectList)
        {
            temp.SetTarget(this.specilList,this.RangeScale);
            temp.DoSpecilEffect();
        }
    }
    private void DoEffect()
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            Close();
            return;
        }
        if (this.player != null)
        {
            foreach (EffectManage effect in this.effectList)
            {
                effect.SetTarget(this.player);
                effect.DoEffect();
            }
        }
        else if (this.soldier != null)
        {
            foreach (EffectManage effect in this.effectList)
            {
                effect.SetTarget(this.soldier);
                effect.DoEffect();
            }
        }
        else if(this.effectInfoList != null)
        {
            if (effectInfoList.Count == 0)
                this.specilList = this.applyTargetList;
            else
            {
                this.specilList.Clear();
                this.specilList.AddRange(this.applyTargetList);
            }
            List<RoleAttribute> tempTarget = new List<RoleAttribute>(this.specilList);
            for (int j = 0; j < effectList.Count; ++j)
            {
                EffectManage effect = effectList[j];
                List<RoleAttribute> result = effect.effect.IsCanActive(tempTarget);
                for (int i = specilList.Count - 1; i >= 0; --i)
                {
                    if (!result.Contains(specilList[i]))
                        specilList.RemoveAt(i);
                }
                effect.SetTarget(result);
                effect.DoEffect();
                if (this.skillSpecilInfoList != null && this.skillSpecilInfoList.Count > 0)
                {
                    foreach (SkillSpecilInfo name in this.skillSpecilInfoList)
                    {
                        if (effect.effect == null)
                            continue;
                        if (effect.effect.info.effectType == 1 || effect.effect.info.effectType == 2)
                            effect.effect.AddEffectName(name.effectName);
                        if (effect.effect.info.effectId == 804)
                        {
                            for (int z = 0; z < result.Count; ++z)
                            {
                                RoleAttribute tmp = result[z];
                                if (tmp == null)
                                    continue;
                                if (tmp.shiedEffectName == null)
                                    tmp.shiedEffectName = new List<string>(4);
                                if (name == null)
                                    continue;
                                tmp.shiedEffectName.Add(name.effectName);

                            }
                        }
                    }
                }
            }
        }
    }
    public void RemoveEffect(List<RoleAttribute> list)
    {
        if (list != null)
        {
            foreach (EffectManage effect in this.effectList)
            {
                effect.effect.RevocateEffect(list);
            }
        }
    }
    public void RemoveSpecil(List<RoleAttribute> list)
    {
        if (list != null)
        {
            foreach (SpecilEffect temp in this.specildEffectList)
            {
                temp.RemoveSpecil(list);
            }
        }
    }
    public void Close()
    {
        UpdateTimeTool.Instance.RemoveUpdator(this.TargetFindUpdate);
        RemoveEffectUpDate();
        //this.RemoveEffect(this.targetList);
        if (this.targetList != null)
            this.targetList.Clear();
        if (this.applyTargetList != null)
        {
            this.applyTargetList.Clear();
        }
        if (this.effectInfoList != null)
        {
            this.effectInfoList.Clear();
        }
        if (this.effectList != null)
        {
            this.effectList.Clear();
        }
        if (this.newTargetList != null)
        {
            this.newTargetList.Clear();
        }
        if (this.oldTargetList != null)
        {
            this.oldTargetList.Clear();
        }
        if (this.skillSpecilInfoList != null)
        {
            this.skillSpecilInfoList = null; ;
        }
        if (this.specildEffectList != null)
        {
            this.specildEffectList = null;
        }
        if (this.specilList != null)
        {
            this.specilList.Clear();
        }
        if (this.targetList != null)
        {
            this.targetList.Clear();
        }

        this.player = null;
        this.soldier = null;
        this.role = null;
        this.resource = null;
    }
    private void TargetFindUpdate(float detail)
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            Close();
            return;
        }
        if (this.role != null)
        {
            destWorldPosition = this.role.transform.position;
        }
        else
        {
            Close();
        }
    }
    private void EffectUpDate()
    {
        if (SceneManager.Instance.Get_CurScene == null)
        {
            Close();
            return;
        }
        FindTarget();
        this.DoTarget();
    }
    private void RemoveEffectUpDate()
    {
        UpdateTimeTool.Instance.RemoveTimer(this.EffectUpDate);
    }
    private bool IsFilter(RoleAttribute target)
    {
        if (this.effectList == null) return true;
        if (target.BuffManage == null || target.EffectImmuneManage == null)
            return true;

        foreach (EffectManage effect in this.effectList)
        {
            if(!target.EffectImmuneManage.FilterEffect(effect.effect))
            {
                return false;
            }
        }
        return true;
    }
}
#endregion
class UpdateTimeTool
{
    public List<GameObject> delayDeleteList;
    public bool isHad = false;

    public static readonly UpdateTimeTool Instance = new UpdateTimeTool();

    public delegate void UpdateTimeDelegate(float detail);
    public delegate void OnScheduler();
    private float time;
    private class TimeScheduler
    {
        public uint ID = 0;
        public float StarTime = 0.0f;
        public float Time = 0.0f;
        public bool IsLoop = false;
        public OnScheduler Callback = null;
    }
    private List<TimeScheduler> _timeSchedulers;

    private List<UpdateTimeDelegate> _updateScheduler;

    private uint _curAllotID;

    private bool isPause = false;

    private float timeScale = 1.0f;

    /// <summary>
    /// 构造
    /// </summary>
    private UpdateTimeTool()
    {
        _curAllotID = 0;
        _timeSchedulers = new List<TimeScheduler>();
        _updateScheduler = new List<UpdateTimeDelegate>();
        delayDeleteList = new List<GameObject>();
        time = Time.time;
    }

    ~UpdateTimeTool()
    {
        _timeSchedulers.Clear();
        _timeSchedulers = null;
        _updateScheduler.Clear();
        _updateScheduler = null;
        delayDeleteList.Clear();
        delayDeleteList = null;
    }
    /// <summary>
    /// 更新
    /// </summary>
    public void Update()
    {
        if (isPause)
            return;

        float currTime = Time.time;
        float value = currTime - time;
        this.time = currTime;

        UpdateTimeScheduler();

        UpdateUpdator(value / this.timeScale);
        if (delayDeleteList.Count > 0 && isHad)
        {
            for (int i = delayDeleteList.Count - 1; i >= 0;--i )
            {
                GameObject go = delayDeleteList[i];
                if (go != null && go.activeSelf)
                    GameObject.Destroy(go);
            }
            delayDeleteList.Clear();
            isHad = false;
        }
        if (delayDeleteList.Count > 0)
            isHad = true;
    }
    public void DelayDelGameObject(GameObject go)
    {
        //if (!delayDeleteList.Contains(go))
            this.delayDeleteList.Add(go);
    }
    public void DelayDelGameObject(List<GameObject> go)
    {
        this.delayDeleteList.AddRange(go);
    }
    private void UpdateUpdator(float detail)
    {
        if (_updateScheduler.Count <= 0)
        {
            return;
        }

        for (var i = 0; i < _updateScheduler.Count; ++i)
        {
            _updateScheduler[i](detail);
        }
    }
    private void UpdateTimeScheduler()
    {
        if (_timeSchedulers == null || _timeSchedulers.Count <= 0)
            return;
        for (int i = (_timeSchedulers.Count - 1); i >= 0; --i)
        {
            TimeScheduler obj = _timeSchedulers[i];
            if ((Time.time - obj.StarTime) >= obj.Time * this.timeScale)
            {
                obj.Callback();
                if (obj.IsLoop)
                {
                    obj.StarTime = Time.time;
                }
                else
                {
                    if (i < _timeSchedulers.Count)
                        _timeSchedulers.RemoveAt(i);
                    continue;
                }
            }

        }
    }
    public void AddTimer(float time, bool loop, OnScheduler callback)
    {
        if (IsAddTimer(time, loop, callback))
        {
            return;
        }
        ++_curAllotID;
        var timeScheduler = new TimeScheduler
        {
            ID = _curAllotID,
            Time = time,
            StarTime = Time.time,
            IsLoop = loop,
            Callback = callback
        };
        _timeSchedulers.Add(timeScheduler);
    }


    public bool IsAddTimer(float time, bool loop, OnScheduler callback)
    {
        for (var i = 0; i < _timeSchedulers.Count; ++i)
        {
            var deleData = _timeSchedulers[i];
            if (deleData.Callback == callback && time == deleData.Time && loop == deleData.IsLoop)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsAddTimer(OnScheduler callback)
    {
        for (var i = 0; i < _timeSchedulers.Count; ++i)
        {
            var deleData = _timeSchedulers[i];
            if (deleData.Callback == callback)
            {
                return true;
            }
        }
        return false;
    }
    public void RemoveTimer(OnScheduler callback)
    {
        if (callback == null) return;
        for (var i = 0; i < _timeSchedulers.Count; ++i)
        {
            var deleData = _timeSchedulers[i];
            if (deleData.Callback == callback)
            {
                _timeSchedulers.RemoveAt(i);
                break;
            }
        }
    }
    public void AddUpdator(UpdateTimeDelegate callback)
    {
        if (callback == null)
            return;
        if (_updateScheduler.Contains(callback))
            return;
        _updateScheduler.Add(callback);
    }
    public void RemoveUpdator(UpdateTimeDelegate callback)
    {
        if (callback == null)
            return;
        if (!_updateScheduler.Contains(callback))
            return;
        _updateScheduler.Remove(callback);
    }
    public bool Pause()
    {
        this.isPause = true;

        return true;
    }
    public void Remove()
    {
        _updateScheduler.Clear();
        _timeSchedulers.Clear();
    }
    public bool Resume()
    {
        this.isPause = false;
        this.time = Time.time;
        foreach (TimeScheduler temp in this._timeSchedulers)
        {
            temp.StarTime = this.time;
        }
        return true;
    }
    public void SetTimeScale(float timeScale)
    {
        if (timeScale == 0)
            return;
        this.timeScale = 1.0f / timeScale;
    }
}