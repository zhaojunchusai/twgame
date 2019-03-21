using UnityEngine;
using System.Collections;

/// <summary>
/// 传送门
/// </summary>
public class Transfer : RoleAttribute
{
    //private RoleHPBase hpBuild;
    private UISprite Spt_Up;
    private UISprite Spt_Down;

    protected override void InitRoleCoefficient()
    {
        _Coeff_Attack_Hurt = 0;
        _Coeff_Attack_Restraint = 0;
        _Coeff_Attack_Crit = 0;
        _Coeff_Attack_AccuracyRate_1 = 0;
        _Coeff_Attack_AccuracyRate_2 = 0;
        _Coeff_Attack_CritRate_1 = 0;
        _Coeff_Attack_CritRate_2 = 0;
        _Coeff_Skill_Hurt = 0;
        _Coeff_Skill_Restraint = 0;
        _Coeff_Skill_Passive = 0;
    }

    public override void InitRoleAttribute(ShowInfoBase vFightAttribute)
    {
        base.InitRoleAttribute(vFightAttribute);
        if (EffectImmuneManage != null)
            EffectImmuneManage.AddFilter(SkillFilter_Transfer);
        SetRoleCamp(EFightCamp.efcSelf);
        SetRoleType(ERoleType.ertTransfer);

        //hpBuild = new TransferHp();
        //hpBuild.Instance(this, ERoleType.ertTransfer, new Vector3(0, 100, 0));
        //hpBuild.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);
        pRpleHP = new TransferHp();
        pRpleHP.Instance(this, ERoleType.ertTransfer, new Vector3(0, 100, 0));
        pRpleHP.SetCurAndMaxValue(fightAttribute.HP, this.Get_MaxHPValue);

        roleLevel = 0;

        Spt_Up = this.transform.FindChild("Up").GetComponent<UISprite>();
        Spt_Down = this.transform.FindChild("Down").GetComponent<UISprite>();
        CommonFunction.SetObjLayer(this.transform, this.transform.parent.gameObject.layer);
    }

    public override bool ReSetRoleHP(int vValue, HurtType type, bool vIsShowChange = true, bool vIsInit = false)
    {
        //fightAttribute.HP -= 1;
        //hpBuild.RefreshHp(-1,type);
        return true;
    }
    public void ChangeTransferCount()
    {
        fightAttribute.HP -= 1;
        //hpBuild.RefreshHp(-1, HurtType.none);
        pRpleHP.RefreshHp(-1, HurtType.none);
    }

    /// <summary>
    /// 修改传送门火柱样式
    /// </summary>
    /// <param name="vSpriteName"></param>
    public void ReSetFireColumn(string vSpriteName)
    {
        if (vSpriteName.Equals("0") || string.IsNullOrEmpty(vSpriteName))
        {
            CommonFunction.SetSpriteName(Spt_Up, "Eft_Portal_Desert");
            CommonFunction.SetSpriteName(Spt_Down, "Eft_Portal_Desert");
        }
        else
        {
            CommonFunction.SetSpriteName(Spt_Up, vSpriteName);
            CommonFunction.SetSpriteName(Spt_Down, vSpriteName);
        }
    }
}
