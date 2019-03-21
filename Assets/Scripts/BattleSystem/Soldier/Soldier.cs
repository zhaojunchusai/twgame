using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public enum SoldierCheck
{
    none,
    Level,//等级原因
    Money,//金钱原因
    Goods,//材料原因
    star,//需要升星操作
    Ok//成功
}
public enum SoldierEquip
{
    none,//未知错误
    Type,//装备的格子不正确
    same,//装备就是当前的
    Ok
}
public enum SoldierChange
{
    All,//所有容器发生了变化
    One//特定的Slot的装备发生了变化
}
public enum SoldierControl
{
    /// <summary>
    /// 武将升级
    /// </summary>
    SoldierUpLvResq,
    /// <summary>
    /// 武将甄选
    /// </summary>
    SoldierSelectResp,
    /// <summary>
    /// 武将升星
    /// </summary>
    SoldierUpStarResp,
    /// <summary>
    /// 天将
    /// </summary>
    SoldierUpQualityResp,
    /// <summary>
    /// 武将有可装备，或者武将技能有可升级，或者武将能够升星小红点提示
    /// </summary>
    CanEPISTANUPSTAR,
}
public class ShowInfoSoldiers : ShowInfoBase
{
    /// <summary>
    /// 召唤间隔
    /// </summary>
    public int SummonInterval;

    public ShowInfoSoldiers()
        : base()
    { }

    public override void ClearInfo()
    {
        base.ClearInfo();
    }

    public override void CopyTo(object vInfo)
    {
        base.CopyTo(vInfo);
        if (vInfo == null)
            return;
    }
}
public class Soldier
{
    public static int MAXQUALITY = 6;
    public static int MAXSTAR = 6;
    /// <summary>
    /// 当前经验值
    /// </summary>
    public int _CurrentExp;
    /// <summary>
    /// 到达下一级需要的经验
    /// </summary>
    public int _NextLvExp;
    /// <summary>
    /// 士兵需要的所有战斗数据
    /// </summary>
    public ShowInfoSoldiers showInfoSoldier;

    private SoldierAttributeInfo att;//配置表属性//

    public EquippedDepot _equippedDepot;//武将身上装备//
    public EquipedLifeSoulDepot _lifeSoulDepot; //武将身上的命魂
    public SkillsDepot _skillsDepot;//武将身上的技能
    public Texture Icon = null;//头像//
    public int Level;//等级//
    private int Step = 0;//阶级
    public uint ID;
    public UInt64 uId = 0;//唯一ID//
    public int Slot = -1;//索引//
    public bool GoIntoBattle = false;//是否被派遣上阵//
    public bool IsNew = true;//是否是未获取过的
    public delegate void VoidUpdateEventHandler();
    public event VoidUpdateEventHandler UpdateAttributeEvent;

    public SoldierAttributeInfo Att { get { return att; } }//获取士兵的信息
    public Dictionary<int, int> StrengthExp { get { return att.StrengthExp; } }//获取士兵升级需要的材料
    public int Lv { get { return this.Level; } }//获取装备当前等级
    public int StepNum { get { return this.Step; } set { this.Step = value; } }//武将的当前阶级
    //士兵的创建函数
    public static Soldier createByID(uint Id)
    {
        Soldier tempSoldier = new Soldier();
        tempSoldier.ID = Id;
        tempSoldier.att = ConfigManager.Instance.mSoldierData.FindById(Id);
        if (tempSoldier.att == null) return null;
        tempSoldier._equippedDepot.soldier = tempSoldier;
        tempSoldier._equippedDepot.father = new WeaponsDepot();
        tempSoldier._skillsDepot.soldier = tempSoldier;
        tempSoldier._CurrentExp = 0;
        if (tempSoldier.att.StrengthExp.ContainsKey(1))
            tempSoldier._NextLvExp = tempSoldier.att.StrengthExp[1];
        else
        {
            SoldierAttributeInfo tempAtt = ConfigManager.Instance.mSoldierData.FindById(tempSoldier.att.evolveId);
            if (tempAtt != null && tempAtt.StrengthExp.ContainsKey(1))
            {
                tempSoldier._NextLvExp = tempAtt.StrengthExp[1];
            }
            else
            {
                tempSoldier._CurrentExp = 0;
                tempSoldier._NextLvExp = 0;
            }
        }

        //ResourceLoadManager.Instance.LoadAloneImage(tempSoldier.att.Icon, (texture) =>
        //{
        //    tempSoldier.icon = texture;
        //});

        tempSoldier.Level = tempSoldier.att.baseLevel;
        tempSoldier.showInfoSoldier.KeyData = tempSoldier.att.id;
        tempSoldier.showInfoSoldier.Attack = tempSoldier.att.phy_atk;
        tempSoldier.showInfoSoldier.Accuracy = tempSoldier.att.acc_rate;
        tempSoldier.showInfoSoldier.AttDistance = tempSoldier.att.atk_space;
        tempSoldier.showInfoSoldier.AttRate = CommonFunction.GetSecondTimeByMilliSecond(tempSoldier.att.atk_interval);
        tempSoldier.showInfoSoldier.Crit = tempSoldier.att.crt_rate;
        tempSoldier.showInfoSoldier.Dodge = tempSoldier.att.ddg_rate;
        tempSoldier.showInfoSoldier.HP = tempSoldier.att.hp_max;
        tempSoldier.showInfoSoldier.HPRecovery = 0;
        tempSoldier.showInfoSoldier.KeyData = tempSoldier.att.id;
        tempSoldier.showInfoSoldier.MoveSpeed = tempSoldier.att.speed;
        tempSoldier.showInfoSoldier.Tenacity = tempSoldier.att.tnc_rate;
        tempSoldier.showInfoSoldier.Leadership = (UInt16)tempSoldier.att.leaderShip;
        tempSoldier.Step = 0;
        return tempSoldier;
    }
    public void LoadImage(System.Action<Texture> onload)
    {
        if (this.Icon != null)
        {
            onload(this.Icon);
            return;
        }
        ResourceLoadManager.Instance.LoadAloneImage(this.att.Icon, (texture) =>
        {
            this.Icon = texture;
            onload(this.Icon);
        });
    }
    public int GetCombatPower()
    {
        if (this.showInfoSoldier.CombatPower == 0)
            this.showInfoSoldier.CombatPower = Calculation_Attribute.Instance.Calculation_Attribute_Soldier(this, true).CombatPower;

        return this.showInfoSoldier.CombatPower;
    }
    public bool IsMaxStep()
    {
        return this.Step >= this.att.maxStep || this.att.maxStep == 0;
    }
    public bool Serialize(fogs.proto.msg.Soldier sd)
    {
        this._CurrentExp = sd.exp;
        this.Level = sd.lv;
        this.ID = sd.id;
        this.Step = sd.step;
        if (this.att.StrengthExp.ContainsKey(sd.lv - this.att.baseLevel + 1))
            this._NextLvExp = this.att.StrengthExp[sd.lv - this.att.baseLevel + 1];
        else
        {
            SoldierAttributeInfo tempAtt = ConfigManager.Instance.mSoldierData.FindById(this.att.evolveId);
            if (tempAtt != null && tempAtt.StrengthExp.ContainsKey(1))
            {
                this._NextLvExp = tempAtt.StrengthExp[1];
            }
            else
            {
                this._CurrentExp = 0;
                this._NextLvExp = 0;
            }
        }
        this.uId = sd.uid;
        if (sd.attr != null)
        {
            this.SerializeShowInfo(sd.attr);
        }
        if (sd.skills != null)
        {
            this._skillsDepot.Serialize(sd.skills);
            this._skillsDepot.soldier = this;
        }
        if (sd.equip_on != null)
        {
            this._equippedDepot.Serialize(sd.equip_on);
            this._equippedDepot.soldier = this;
        }
        if (sd.souls != null)
        {
            this._lifeSoulDepot.Serialize(sd.souls, this);
        }
        return true;
    }
    /// <summary>
    /// 获取武将战斗属性
    /// </summary>
    /// <returns></returns>
    public ShowInfoSoldiers getShowInfo()
    {
        ShowInfoSoldiers temp = new ShowInfoSoldiers();
        temp.CopyTo(showInfoSoldier);
        temp.Attack = showInfoSoldier.Attack + (int)(att.u_attack * this.Level);
        temp.Accuracy = showInfoSoldier.Accuracy + (int)(att.u_accuracy * this.Level);
        temp.Crit = showInfoSoldier.Crit + (int)(att.u_crit * this.Level);
        temp.Dodge = showInfoSoldier.Dodge + (int)(att.u_dodge * this.Level);
        temp.HP = showInfoSoldier.HP + (int)(att.u_hp * this.Level);
        temp.Tenacity = showInfoSoldier.Tenacity + (int)(att.u_tenacity * this.Level);

        return temp;
    }
    public void ShowInfoAdd(int Level)
    {
        showInfoSoldier.Attack += (int)(att.u_attack * Level);
        showInfoSoldier.Accuracy += (int)(att.u_accuracy * Level);
        showInfoSoldier.Crit += (int)(att.u_crit * Level);
        showInfoSoldier.Dodge += (int)(att.u_dodge * Level);
        showInfoSoldier.HP += (int)(att.u_hp * Level);
        showInfoSoldier.Tenacity += (int)(att.u_tenacity * Level);
        if (this.UpdateAttributeEvent != null)
            this.UpdateAttributeEvent();

    }
    public ShowInfoSoldiers GetShowInfoAddByStep(int step)
    {
        ShowInfoSoldiers tmpInfo = new ShowInfoSoldiers();
        tmpInfo.CopyTo(this.showInfoSoldier);
        tmpInfo.Attack += (int)(att.stepAttack * step);
        tmpInfo.Accuracy += (int)(att.stepAccuracy * step);
        tmpInfo.Crit += (int)(att.stepCrit * step);
        tmpInfo.Dodge += (int)(att.stepDodge * step);
        tmpInfo.HP += (int)(att.stepHp * step);
        tmpInfo.Tenacity += (int)(att.stepTenacity * step);
        if (this.UpdateAttributeEvent != null)
            this.UpdateAttributeEvent();
        return tmpInfo;
    }
    public void AddStep(int step)
    {
        this.Step += step;
        this.showInfoSoldier = this.GetShowInfoAddByStep(step);
    }
    public void SerializeShowInfo(fogs.proto.msg.Attribute att)
    {
        if (att == null) return;
        this.showInfoSoldier.Attack = att.phy_atk;
        this.showInfoSoldier.Accuracy = att.acc_rate;
        this.showInfoSoldier.Crit = att.crt_rate;
        this.showInfoSoldier.Dodge = att.ddg_rate;
        this.showInfoSoldier.HP = att.hp_max;
        this.showInfoSoldier.Tenacity = att.tnc_rate;
        this.showInfoSoldier.AttRate = CommonFunction.GetSecondTimeByMilliSecond(att.atk_interval);
        this.showInfoSoldier.AttDistance = att.atk_space;
        this.showInfoSoldier.CombatPower = att.combat_power;
        this.showInfoSoldier.Leadership = (UInt16)att.leader;
        
        if (this.UpdateAttributeEvent != null)
            this.UpdateAttributeEvent();

    }
    public void AddShowInfo(fogs.proto.msg.Attribute att)
    {
        if (att == null) return;
        this.showInfoSoldier.Attack += att.phy_atk;
        this.showInfoSoldier.Accuracy += att.acc_rate;
        this.showInfoSoldier.Crit += att.crt_rate;
        this.showInfoSoldier.Dodge += att.ddg_rate;
        this.showInfoSoldier.HP += att.hp_max;
        this.showInfoSoldier.Tenacity += att.tnc_rate;
        this.showInfoSoldier.AttRate += CommonFunction.GetSecondTimeByMilliSecond(att.atk_interval);
        this.showInfoSoldier.AttDistance += att.atk_space;
        if (this.UpdateAttributeEvent != null)
            this.UpdateAttributeEvent();
    }
    public bool IsTheSameTree(uint id)
    {
        if(this.att.id == id)
            return true;
        uint lastId = this.att.lastId;
        uint nextId = this.att.evolveId;
        while (lastId != 0 || nextId != 0)
        {
            if (lastId == id || nextId == id)
                return true;
            if(lastId != 0)
            {
                SoldierAttributeInfo tmp = ConfigManager.Instance.mSoldierData.FindById(lastId);
                if (tmp == null)
                    lastId = 0;
                else
                    lastId = tmp.lastId;
            }
            if (nextId != 0)
            {
                SoldierAttributeInfo tmp = ConfigManager.Instance.mSoldierData.FindById(nextId);
                if (tmp == null)
                    nextId = 0;
                else
                    nextId = tmp.evolveId;
            }
        }
        return false;
    }
    /// <summary>
    /// 武将升级，会返回一个升级过后的武将的临时对象
    /// </summary>
    /// <param name="exp"></param>
    /// <returns></returns>
    public Soldier TextStrong(int exp)
    {
        if (isMaxLevel()) return this;

        Soldier tempSoldier = Soldier.createByID((uint)this.att.id);
        tempSoldier.Level = this.Level;
        tempSoldier.showInfoSoldier.CopyTo(this.showInfoSoldier);
        tempSoldier._CurrentExp = this._CurrentExp;
        tempSoldier._NextLvExp = this._NextLvExp;
        tempSoldier._strong(exp);
        return tempSoldier;
    }
    public int GetMaxLevelEXP()
    {
        int level = this.Level;
        int exp = 0;
        int LvUp = this.Level - att.baseLevel + 1;
        if (!this.isMaxLevel())
        {
            if (att.StrengthExp.ContainsKey(LvUp))
            {
                exp += att.StrengthExp[LvUp] - this._CurrentExp;
            }
        }
        ++level;
        ++LvUp;
        while (level < this.att.evolveLv)
        {
            if (att.StrengthExp.ContainsKey(LvUp))
            {
                exp += att.StrengthExp[LvUp];
            }
            ++level;
            ++LvUp;
        }
        return exp;
    }
    public int GetMaxLevelEXPGold()
    {
        int exp = this.GetMaxLevelEXP();
        return exp * int.Parse(ConfigManager.Instance.mSpecialIDData.GetSingleDataByID(SpecialField.SOLDIERUPSTAR_EXPADD));
    }
    public int GetCurretExp()
    {
        int level = this.Level;
        int exp = this._CurrentExp;
        int LvUp = this.Level - att.baseLevel + 1;
        for (int i = 1; i < LvUp;++i )
        {
            if (att.StrengthExp.ContainsKey(i))
            {
                exp += att.StrengthExp[i];
            }
        }
        return exp;
    }
    
    /// <summary>
    /// 获取这个武将换算的经验价值和金钱价值
    /// </summary>
    /// <returns></returns>
    public Vector2 GetExpAndMoneyWorth()
    {
        Vector2 result = new Vector2();
        result.x += this.GetCurretExp();
        if(this.Step > 0 && !this.att.stepMaterialId.Equals(0))
        {
            Soldier tmpSoldier = Soldier.createByID(this.att.stepMaterialId);
            if (tmpSoldier != null)
            {
                tmpSoldier.Level = this.att.stepMaterialLv;
                tmpSoldier.StepNum = this.att.stepMaterialStep;
                result += tmpSoldier.GetExpAndMoneyWorth() * this.Step;
                result.y += this.att.stepMoneyNum * this.Step;
            }
        }
        
        if(this.att.lastId != 0)
        {
            Soldier tmpSoldier = Soldier.createByID(this.att.lastId);
            if(tmpSoldier != null)
            {
                tmpSoldier.Level = tmpSoldier.att.evolveLv;
                tmpSoldier.StepNum = tmpSoldier.att.maxStep;
                result += tmpSoldier.GetExpAndMoneyWorth();
                result.y += tmpSoldier.att.sellNum;
                Soldier tmpSoldier1 = Soldier.createByID(tmpSoldier.att.evolveMateria.Key);
                if (tmpSoldier1 != null)
                {
                    tmpSoldier1.Level = tmpSoldier.att.evolveMateria.Value;
                    tmpSoldier1.StepNum = tmpSoldier.att.evolveMaterialStep;
                    result += tmpSoldier1.GetExpAndMoneyWorth();
                }
            }
        }
        return result;
    }
    /// <summary>
    /// 获取这个武将换算出的金钱价值
    /// </summary>
    /// <returns></returns>
    public int GetMoneyWorth()
    {
        int result = 0;

        return result;
    }


    /// <summary>
    /// 设置武将上阵或者卸阵
    /// </summary>
    /// <param name="isInto">true代表上阵false代表卸阵</param>
    public void SetIntoBattle(bool isInto)
    {
        this.GoIntoBattle = isInto;
    }
    /// <summary>
    /// 随机触发技能
    /// </summary>
    /// <param name="role">技能发起者</param>
    /// <returns></returns>
    public bool RandomSkill(RoleAttribute role)
    {
        return this._skillsDepot.RandomSkill(role);
    }
    /// <summary>
    /// 激活光环技能
    /// </summary>
    /// <param name="role">技能发起者</param>
    public void ActivateHalo(RoleAttribute role)
    {
        this._skillsDepot.ActivateHalo(role);
    }
    public SoldierCheck enableUpStar()
    {
        if (this.att.evolveId == 0)
            return SoldierCheck.none;
        if (isMaxLevel())
        {
            Soldier tempSoldier = PlayerData.Instance._SoldierDepot._soldierList.Find((sd) =>
            {
                if (sd == null)
                    return false;
                if (this.uId == sd.uId) return false;

                if (this.Att.evolveMateria.Key != sd.Att.id) return false;
                if (this.att.evolveMateria.Value > sd.Level) return false;
                if (!sd.IsMaxStep()) return false;
                return true;
            });
            if (tempSoldier != null)
            {
                SoldierAttributeInfo temp = ConfigManager.Instance.mSoldierData.FindById(tempSoldier.Att.evolveId);
                if (temp == null)
                    return SoldierCheck.none;

                return SoldierCheck.Ok;
            }
        }
        return SoldierCheck.none;
    }
    public bool Strong(int exp)
    {
        return _strong(exp);
    }
    public bool isMaxLevel()
    {
        return this.Level >= att.evolveLv;
    }
    /// <summary>
    /// 获取作为材料提供的经验
    /// </summary>
    /// <returns></returns>
    public int getBeExp()
    {
        float exp = this.att.materiaExp;
        int absLevel = this.Level - att.baseLevel + 1;
        for (int i = 1; i < absLevel; ++i)
        {
            if (this.att.StrengthExp.ContainsKey(i))
                exp += this.att.StrengthExp[i] * 0.7f;
        }
        exp += this._CurrentExp * 0.7f;
        return (int)exp;
    }
    public int StarWorth()
    {
        SoldierStepInfo tmpStarWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(this.att.Star);
        SoldierStepInfo tmpStepWorth = ConfigManager.Instance.mSoldierStepData.FindByStarOrStep(this.Step);
        int num = 0;
        if (tmpStarWorth != null)
            num += tmpStarWorth.starValue;
        if (tmpStepWorth != null)
            num += tmpStepWorth.stepValue;
        return num;
    }
    public bool IsEquipFull()
    {
        if (this._equippedDepot == null || this._equippedDepot._EquiptList == null)
            return false;
        for (int i = 0; i < this._equippedDepot._EquiptList.Count; ++i)
        {
            if (this._equippedDepot._EquiptList[i] == null)
                return false;
        }
        return true;
    }
    public string GetDescript()
    {
        return this.att.Descript;
    }
    public Soldier()
    {
        _skillsDepot = new SkillsDepot();
        _equippedDepot = new EquippedDepot();
        showInfoSoldier = new ShowInfoSoldiers();
        _lifeSoulDepot = new EquipedLifeSoulDepot();
    }

    //强化//
    private bool _strong(int exp)
    {
        bool result = false;
        int level = this.Level;
        int LvUp = this.Level - att.baseLevel + 1;
        while (exp > 0)
        {
            //if (this._NextLvExp - this._CurrentExp < 0)
            //{
            //    ++this.Level;
            //    return false;
            //}
            if (this.isMaxLevel())
            {
                this._CurrentExp = exp;
                ShowInfoAdd(this.Level - level);
                level = this.Level;
                return result;
            }

            if ((this._NextLvExp - this._CurrentExp) > exp)
            {
                this._CurrentExp += exp;
                exp = 0;
                result = true;
            }
            else
            {
                ++this.Level;
                ++LvUp;
                exp -= (this._NextLvExp - this._CurrentExp);
                this._CurrentExp = 0;
                if (!att.StrengthExp.ContainsKey(LvUp))
                {
                    ShowInfoAdd(this.Level - level);
                    level = this.Level;
                    if (this.isMaxLevel())
                    {
                        this._CurrentExp = exp;
                        return result;
                    }
                    return false;
                }
                this._NextLvExp = att.StrengthExp[LvUp];
                result = true;
            }
        }
        ShowInfoAdd(this.Level - level);
        return result;
    }
}

public class SoldierDepot
{
    public List<Soldier> _soldierList;//武将列表
    public bool needSort = false;
    public bool needRemoveNull = false;

    //装备栏发生了变化
    public delegate void SoldierDepotDelet(SoldierChange change, int Slot = -1, UInt64 uID = 0);
    public event SoldierDepotDelet SoldierDepotEvent;

    public delegate void SoldierErrorDelet(SoldierControl control, int errorCode, UInt64 uid = 0);
    public event SoldierErrorDelet SoldierErrorEvent;

    public delegate bool Filter(Soldier sd);
    //加入依附的装备列表对象（不用操作，子已经自动操作）
    public void OnSoldierErrorEvent(SoldierControl control, int errorCode, UInt64 uid = 0)
    {
        if (this.SoldierErrorEvent != null)
            this.SoldierErrorEvent(control, errorCode, uid);
    }
    public static int MAXCOUNT
    {
        get
        {
            return 300;
        }
    }
    public void Update()
    {
        _RemoveNull();
        _sort();
    }
    public void Clear()
    {
        this._soldierList.Clear();
        this.needSort = false;
        this.needRemoveNull = false;
    }
    //通过slot找装备
    public Soldier FindBySlot(int slot)
    {
        return _soldierList.Find((Soldier p) =>
        {
            if (p == null) return false;
            return p.Slot == slot;
        });
    }
    public void CheckSoldierPromt()
    {
        if (this.CheckCanEquip() || this.CheckCanSkillInstance() || this.CheckCanUpStar() || CommonFunction.CheckFightSoldierIsCanUpdate())
            this.OnSoldierErrorEvent(SoldierControl.CanEPISTANUPSTAR, 0);
        else
            this.OnSoldierErrorEvent(SoldierControl.CanEPISTANUPSTAR, 1);

    }

    /// <summary>
    /// 通过ID寻找武将
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public List<Soldier> FindById(uint id)
    {
        return _soldierList.FindAll((Soldier p) => { if (p == null) return false; return p.Att.id == id; });
    }
    public Soldier FindByUid(UInt64 uId)
    {
        return _soldierList.Find((Soldier p) =>
        {
            if (p == null) return false;
            return p.uId == uId;
        });
    }
    //添加一组ID的装备
    //添加失败会返回false，但是会把能添加的装备添加进去
    public bool multipleAdd(List<uint> ID)
    {
        bool success = true;

        foreach (var temp in ID)
        {
            if (_Add(temp) == null) success = false;
        }
        return success;
    }
    //添加一个ID的装备
    public Soldier oneAdd(uint Id)
    {
        return _Add(Id);
    }
    /// <summary>
    /// 根据武将proto创建武将
    /// </summary>
    /// <param name="sd"></param>
    /// <returns></returns>
    public Soldier oneAdd(fogs.proto.msg.Soldier sd, bool isBound = true)
    {
        if (sd == null) return null;
        if (this.FindByUid(sd.uid) != null) return null;
        Soldier wp = Soldier.createByID(sd.id);
        if (wp == null) return null;
        if (isBound)
            wp._equippedDepot.BindFather(PlayerData.Instance._SoldierEquip);
        wp.Serialize(sd);

        _soldierList.Add(wp);
        wp.Slot = _soldierList.Count - 1;
        needSort = true;
        return wp;
    }
    /// <summary>
    /// 批量添加武将
    /// </summary>
    /// <param name="soldierList"></param>
    /// <returns></returns>
    public List<Soldier> multipleAdd(List<fogs.proto.msg.Soldier> soldierList)
    {
        if (soldierList == null)
            return null;
        List<Soldier> resultList = new List<Soldier>(soldierList.Count + 1);
        foreach (fogs.proto.msg.Soldier temp in soldierList)
        {
            resultList.Add(this.oneAdd(temp));
        }
        return resultList;
    }
    /// <summary>
    /// 强化对应唯一ID的武将测试
    /// </summary>
    /// <param name="destSlot">强化的武将对应的slot</param>
    /// <param name="_materials">强化的材料</param>
    /// <returns></returns>
    public Soldier TextStrong(UInt64 destUid, List<UInt64> _materials)
    {
        int exp = 0;
        Soldier destSoldier = this.FindByUid(destUid);
        if (destSoldier == null) return null;
        foreach (UInt64 a in _materials)
        {
            Soldier sd = this.FindByUid(a);
            if (sd == null) continue;

            exp += sd.getBeExp();
        }

        return destSoldier.TextStrong(exp);
    }
    /// <summary>
    /// 强化对应唯一ID的武将
    /// </summary>
    /// <param name="destUid">强化的武将对应的唯一ID</param>
    /// <param name="_materials">强化的材料</param>
    public void Strong(UInt64 destUid, List<UInt64> _materials)
    {
        SoldierModule.Instance.SendSoldierUpLvReq(destUid, _materials);
    }
    public void multipleDelete(List<UInt64> vList)
    {
        foreach (UInt64 tp in vList)
        {
            Soldier de = this.FindByUid(tp);
            if (de != null)
                this._Delete(de.Slot);
        }
        if (PlayerData.Instance._SoldierEquip != null)
        {
            PlayerData.Instance._SoldierEquip.needRemoveNull = true;
            PlayerData.Instance._SoldierEquip.needSort = true;
        }
    }
    public void ReceiveSoldierUpLvReq(SoldierUpLvResp tData)
    {

        foreach (UInt64 tp in tData.uid_list)
        {
            Soldier de = this.FindByUid(tp);
            if (de != null)
                this._Delete(de.Slot);
        }
        foreach (fogs.proto.msg.Equip temp in tData.equip_list)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
            if (wp != null)
            {
                wp.isEquiped = false;
            }
            PlayerData.Instance._SoldierEquip.needSort = true;
            PlayerData.Instance._SoldierEquip.needRemoveNull = true;
        }
        needRemoveNull = true;
        _RemoveNull();
        Soldier soldier = this.FindByUid(tData.soldier_info.uid);
        if (soldier == null) return;
        if (tData.soldier_info != null)
        {
            soldier._CurrentExp = tData.soldier_info.exp;
            soldier.Level = tData.soldier_info.lv;
            if (soldier.Att.StrengthExp.ContainsKey(tData.soldier_info.lv - soldier.Att.baseLevel + 1))
                soldier._NextLvExp = soldier.Att.StrengthExp[tData.soldier_info.lv - soldier.Att.baseLevel + 1];
            else
            {
                SoldierAttributeInfo tempAtt = ConfigManager.Instance.mSoldierData.FindById(soldier.Att.evolveId);
                if (tempAtt != null && tempAtt.StrengthExp.ContainsKey(1))
                {
                    soldier._NextLvExp = tempAtt.StrengthExp[1];
                }
                else
                {
                    soldier._CurrentExp = 0;
                    soldier._NextLvExp = 0;
                }
            }

            soldier.uId = tData.soldier_info.uid;
            if (tData.soldier_info.attr != null)
            {
                soldier.SerializeShowInfo(tData.soldier_info.attr);
            }
        }

        if (this.SoldierDepotEvent != null)
            this.SoldierDepotEvent(SoldierChange.One, soldier.Slot, soldier.uId);
        needSort = true;
        _sort();
    }
    public void ReceiveSoldierSelectResp(SoldierSelectResp tData)
    {
        foreach (UInt64 tp in tData.uid_list)
        {
            Soldier de = this.FindByUid(tp);
            if (de != null)
                this._Delete(de.Slot);
        }
        foreach (fogs.proto.msg.Equip temp in tData.equip_list)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
            if (wp != null)
            {
                wp.isEquiped = false;
            }
        }

        if (tData.soldier_info == null) return;
        Soldier sd = this._Add(tData.soldier_info.id);
        if (sd == null) return;
        if (tData.soldier_info != null)
            sd.Serialize(tData.soldier_info);
    }
    public void ReceiveSoldierUpStarResp(SoldierUpStarResp tData)
    {
        List<UInt64> tmp = new List<ulong>(2);
        tmp.Add(tData.material_uid);
        UpStar(tData.soldier_info.uid, tmp, (int)tData.soldier_info.id);
        Soldier destSoldier = this.FindByUid(tData.soldier_info.uid);
        if (destSoldier != null)
            destSoldier.Serialize(tData.soldier_info);
        if (tData.equip_list != null)
        {
            foreach (fogs.proto.msg.Equip temp in tData.equip_list)
            {
                Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
                if (wp != null)
                {
                    wp.isEquiped = false;
                }
                PlayerData.Instance._SoldierEquip.needSort = true;
                PlayerData.Instance._SoldierEquip.needRemoveNull = true;
            }
        }
    }
    public void ReceiveSoldierUpStepResp(SoldierUpStepResp tData)
    {
        List<UInt64> _material = new List<ulong>(2);
        _material.AddRange(tData.material_uid);
        for (int i = 0; i < _material.Count; ++i)
        {
            Soldier materialSoldier = PlayerData.Instance._SoldierDepot.FindByUid(_material[i]);
            _Delete(materialSoldier.Slot);
        }
        PlayerData.Instance._SoldierEquip.needSort = true;
        PlayerData.Instance._SoldierEquip.needRemoveNull = true;
        Soldier destSoldier = this.FindByUid(tData.soldier_info.uid);
        destSoldier.SerializeShowInfo(tData.soldier_info.attr);
        destSoldier.StepNum = tData.soldier_info.step;
        if (this.SoldierDepotEvent != null)
            this.SoldierDepotEvent(SoldierChange.One, destSoldier.Slot, destSoldier.uId);
    }
    public void ReceiveSoldierUpLevelStarResq(SoldierUpLevelStarResp tData)
    {
        if (tData.soldier_info == null)
            return;

        UpStar(tData.soldier_info.uid, tData.material_uid, (int)tData.soldier_info.id);
        Soldier destSoldier = this.FindByUid(tData.soldier_info.uid);
        if (destSoldier != null)
            destSoldier.Serialize(tData.soldier_info);
        PlayerData.Instance._LifeSoulDepot.AddPackLifeSouls(tData.soul_return_item);
        foreach (fogs.proto.msg.Equip temp in tData.equip_list)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
            if (wp != null)
            {
                wp.isEquiped = false;
            }
            PlayerData.Instance._SoldierEquip.needSort = true;
            PlayerData.Instance._SoldierEquip.needRemoveNull = true;
        }
    }

    //获取所有装备列表
    public List<Soldier> getSoldierList()
    {
        return _soldierList;
    }
    public List<Soldier> getSoldierList(Filter filter)
    {
        List<Soldier> tempList = new List<Soldier>();
        tempList.Capacity = _soldierList.Count;
        foreach (var temp in _soldierList)
        {
            if (filter(temp))
                tempList.Add(temp);
        }
        return tempList;
    }
    public int GetSoldierCount()
    {
        return this._soldierList.Count;
    }
    public bool IsFull(int count)
    {
        return this._soldierList.Count + count > SoldierDepot.MAXCOUNT;
    }
    public SoldierDepot()
    {
        _soldierList = new List<Soldier>();
        Scheduler.Instance.AddUpdator(Update);
    }
    void _SoldierEquip_WeaponDepotEvent(WeaponChange change, int Slot = -1, ulong uID = 0)
    {
        this.CheckSoldierPromt();
    }

    void Instance_UpdatePlayerGoldEvent()
    {
        this.CheckSoldierPromt();
    }
    void Instance_UpdatePlayerItemsEvent()
    {
        this.CheckSoldierPromt();
    }


    ~SoldierDepot()
    {
        Scheduler.Instance.RemoveUpdator(Update);
        PlayerData.Instance.UpdatePlayerGoldEvent -= Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent -= Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent -= _SoldierEquip_WeaponDepotEvent;
    }
    //初始化
    public bool Serialize(List<fogs.proto.msg.Soldier> soldierBag)
    {
        foreach (var temp in soldierBag)
        {
            this.oneAdd(temp);
        }
        PlayerData.Instance.UpdatePlayerGoldEvent += Instance_UpdatePlayerGoldEvent;
        PlayerData.Instance.UpdatePlayerItemsEvent += Instance_UpdatePlayerItemsEvent;
        PlayerData.Instance._SoldierEquip.WeaponDepotEvent += _SoldierEquip_WeaponDepotEvent;
        needSort = true;
        return true;
    }
    private SoldierCheck _enableUpStar(Soldier destSoldier, UInt64 _material)
    {
        if (destSoldier == null) return SoldierCheck.none;
        if (!destSoldier.isMaxLevel()) return SoldierCheck.Level;

        Soldier materialSoldier = PlayerData.Instance._SoldierDepot.FindByUid(_material);
        if (materialSoldier == null) return SoldierCheck.none;

        if (materialSoldier.Level < destSoldier.Att.evolveMateria.Value || materialSoldier.Att.id != destSoldier.Att.evolveMateria.Key)
            return SoldierCheck.Goods;

        return SoldierCheck.Ok;
    }
    private void UpStar(UInt64 dest, List<UInt64> _material, int id)
    {
        Soldier destSoldier = PlayerData.Instance._SoldierDepot.FindByUid(dest);
        if (destSoldier == null) return;

        Soldier sd = Soldier.createByID((uint)id);
        if (sd == null) return;
        sd.uId = destSoldier.uId;
        sd.Slot = destSoldier.Slot;
        _soldierList[destSoldier.Slot] = null;
        _soldierList[sd.Slot] = sd;
        sd._equippedDepot = destSoldier._equippedDepot;
        if (sd._equippedDepot != null)
            sd._equippedDepot.soldier = sd;
        destSoldier = null;
        for (int i = 0; i < _material.Count; ++i)
        {
            Soldier materialSoldier = PlayerData.Instance._SoldierDepot.FindByUid(_material[i]);

            _Delete(materialSoldier.Slot);
        }
        if (this.SoldierDepotEvent != null)
            this.SoldierDepotEvent(SoldierChange.One, sd.Slot, dest);
    }
    private bool CheckCanEquip()
    {
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SoldierEquip))
        {
            return false;
        }
        for (int i = 0; i < this._soldierList.Count; ++i)
        {
            Soldier tmpSoldier = this._soldierList[i];
            if (tmpSoldier == null)
                continue;
            if (!CommonFunction.IsAlreadyBattle(tmpSoldier.uId))
                continue;
            if (
                    PlayerData.Instance._SoldierEquip.getWeaponList
                    (
                               (wp) =>
                               {
                                   if (wp == null)
                                       return false;
                                   if (wp.isEquiped)
                                       return false;
                                   for (int count = 0; count < tmpSoldier._equippedDepot._EquiptList.Count; ++count)
                                   {
                                       if (tmpSoldier._equippedDepot._EquiptList[count] != null)
                                       {
                                           Weapon tmpWp = tmpSoldier._equippedDepot._EquiptList[count];
                                           if (tmpWp == null)
                                               continue;
                                           if (wp.isEquiped)
                                               continue;
                                           if (wp.Att.quality <= tmpWp.Att.quality)
                                               continue;
                                           if (tmpSoldier.Att.PosList[count + 1] == (int)wp.Att.type && tmpSoldier.Level >= (int)wp.Att.levelLimit)
                                               return true;
                                       }
                                       if (tmpSoldier.Att.PosList[count + 1] == (int)wp.Att.type && tmpSoldier.Level >= (int)wp.Att.levelLimit)
                                           return true;
                                   }
                                   return false;
                               }

                    ).Count > 0
               )
            {
                return true;
            }
        }
        return false;
    }
    private bool CheckCanSkillInstance()
    {
        if (!ConfigManager.Instance.mOpenLevelConfig.CheckIsOpen(OpenFunctionType.SoldierSkill))
        {
            return false;
        }
        for (int i = 0; i < this._soldierList.Count; ++i)
        {
            Soldier tmpSoldier = this._soldierList[i];
            if (tmpSoldier == null)
                continue;
            if (!CommonFunction.IsAlreadyBattle(tmpSoldier.uId))
                continue;
            foreach (Skill skill in tmpSoldier._skillsDepot._skillsList)
            {
                if (skill == null)
                    continue;

                if (skill.enableStrong() == SkillCheck.Ok)
                {
                    return true;
                }
            }
        }
        return false;
    }
    private bool CheckCanUpStar()
    {
        for (int i = 0; i < this._soldierList.Count; ++i)
        {
            Soldier tmpSoldier = this._soldierList[i];
            if (tmpSoldier == null)
                continue;
            if (!CommonFunction.IsAlreadyBattle(tmpSoldier.uId))
                continue;
            if (tmpSoldier != null && tmpSoldier.enableUpStar() == SoldierCheck.Ok)
            {
                return true;
            }
        }
        return false;
    }
    private bool _evolve(int slot, int id)
    {
        if (slot >= _soldierList.Count) return false;

        var temp = _soldierList[slot];
        var evoTemp = Soldier.createByID((uint)temp.Att.evolveId);
        if (evoTemp == null) return false;

        evoTemp.Slot = slot;


        _soldierList[slot] = null;
        temp = null;
        _soldierList[slot] = evoTemp;

        needSort = true;
        return true;
    }
    private Soldier _Add(uint Id)
    {
        Soldier wp = Soldier.createByID(Id);
        if (wp == null) return null;

        _soldierList.Add(wp);
        wp.Slot = _soldierList.Count - 1;
        needSort = true;
        return wp;
    }
    private bool _Delete(int Slot)
    {
        if (Slot >= _soldierList.Count) return false;

        Soldier temp = _soldierList[Slot];
        if (temp == null)
            return false;
        foreach (Weapon wp in temp._equippedDepot._EquiptList)
        {
            if (wp == null)
                continue;
            wp.isEquiped = false;
        }
        if (PlayerData.Instance._SoldierEquip != null)
            PlayerData.Instance._SoldierEquip._EquipedList.Remove(temp._equippedDepot);
        temp._equippedDepot._EquiptList.Clear();
        temp._skillsDepot._skillsList.Clear();
        temp = null;
        _soldierList[Slot] = null;

        needSort = true;
        needRemoveNull = true;
        return true;
    }
    public void SortNow()
    {
        this._RemoveNull();
        this._sort();
    }
    /*
    排序：
    1、按照武将等级排序，武将等级越高的排序越靠前；
    2、按照武将星级排序，武将星级越高的排序越靠前；
    3、按照武将阶级排序，武将阶级越高的排序越靠前；
    3、按照武将品质排序，武将品质越高的排序越靠前；
    4、按照武将ID排序，ID越小的排序越靠前；
    5、武将ID相同的，无排序需求
    */
    private void _sort()
    {
        if (!needSort) return;
        _soldierList.Sort
            (
            (left, right) =>
            {
                if (left.Level != right.Level)
                {
                    if (left.Level > right.Level)
                        return -1;
                    else
                        return 1;
                }
                if (left.Att.Star != right.Att.Star)
                {
                    if (left.Att.Star > right.Att.Star)
                        return -1;
                    else
                        return 1;
                }
                if (left.StepNum != right.StepNum)
                {
                    if (left.StepNum > right.StepNum)
                        return -1;
                    else
                        return 1;
                }
                if (left.Att.quality != right.Att.quality)
                {
                    if (left.Att.quality > right.Att.quality)
                        return -1;
                    else
                        return 1;
                }
                if (left.Att.id != right.Att.id)
                {
                    if (left.Att.id < right.Att.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            }
            );
        for (int i = 0; i < _soldierList.Count; ++i)
        {
            _soldierList[i].Slot = i;
        }
        needSort = false;
        this.CheckSoldierPromt(); if (SoldierDepotEvent != null)
        {
            SoldierDepotEvent(SoldierChange.All);
        }
    }
    private void _RemoveNull()
    {
        if (!needRemoveNull) return;
        needRemoveNull = false;
        _soldierList.RemoveAll((temp) => { return temp == null; });
    }
}
public class SoldierMap
{
    List<Soldier> soldierMapList;
    public SoldierMap()
    {
        soldierMapList = new List<Soldier>();
    }
    public int FindIndex(Soldier sd)
    {
        return this.soldierMapList.FindIndex((tmpSolder) =>
        {
            if (tmpSolder == null)
                return false;
            return sd == tmpSolder;
        });
    }
    public List<Soldier> GetSoldierMapList()
    {
        return soldierMapList;
    }
    public void Serialize(List<uint> protoList)
    {
        if (this.soldierMapList == null)
            this.soldierMapList = new List<Soldier>();

        List<SoldierAttributeInfo> InfoList = ConfigManager.Instance.mSoldierData.GetSoldierAttributeList();
        if (InfoList == null)
            return;

        for (int i = 0; i < InfoList.Count; ++i)
        {
            SoldierAttributeInfo soldier = InfoList[i];
            if (soldier == null)
                continue;
            if (soldier.Star > 1)
                continue;
            Soldier temp = Soldier.createByID(soldier.id);

            Skill tmpSkill = Skill.createByID(soldier.initiativeSkill);
            if (tmpSkill != null)
                temp._skillsDepot._skillsList.Add(tmpSkill);

            for (int j = 0; j < soldier.passivitySkill.Count; ++j)
            {
                Skill tmp = Skill.createByID(soldier.passivitySkill[j]);
                if (tmp != null)
                    temp._skillsDepot._skillsList.Add(tmp);
            }

            if (protoList != null)
            {
                if (protoList.Contains(soldier.id))
                    temp.IsNew = false;
            }


            this.soldierMapList.Add(temp);

            this.soldierMapList.Sort((left, right) =>
            {
                if (left == null && right != null)
                    return 1;
                if (left != null && right == null)
                    return -1;

                if (left.Att.quality != right.Att.quality)
                {
                    if (left.Att.quality > right.Att.quality)
                        return -1;
                    else
                        return 1;
                }
                if (left.Att.id != right.Att.id)
                {
                    if (left.Att.id < right.Att.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
        }
    }
    public void BatchDelete(List<uint> protoList)
    {
        for (int i = 0; i < this.soldierMapList.Count; ++i)
        {
            Soldier soldier = this.soldierMapList[i];
            if (soldier == null)
                continue;

            if (protoList.Contains(soldier.Att.id))
                soldier.IsNew = false;
        }
    }
    public void OneDelete(uint protoId)
    {
        for (int i = 0; i < this.soldierMapList.Count; ++i)
        {
            Soldier soldier = this.soldierMapList[i];
            if (soldier == null)
                continue;

            if (protoId == soldier.Att.id)
                soldier.IsNew = false;
        }
    }
    public void Clear()
    {
        if (this.soldierMapList != null)
            this.soldierMapList.Clear();
        this.soldierMapList = null;
    }
}