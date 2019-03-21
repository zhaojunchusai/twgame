using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;

public enum WeaponCheck
{
    /// <summary>
    /// 未知原因
    /// </summary>
    none,
    /// <summary>
    /// 等级原因
    /// </summary>
    Level,
    /// <summary>
    /// 达到最大等级
    /// </summary>
    MaxLevel,
    /// <summary>
    /// 英雄等级不足
    /// </summary>
    HeroLevelLower = 327,
    /// <summary>
    /// 士兵等级不足
    /// </summary>
    SoldierLevelLower = 328,
    /// <summary>
    /// 金钱原因
    /// </summary>
    Money,
    /// <summary>
    /// 材料原因
    /// </summary>
    Goods,
    /// <summary>
    /// 需要升星操作
    /// </summary>
    star,
    /// <summary>
    /// 未解锁
    /// </summary>
    Lock,
    /// <summary>
    /// 成功
    /// </summary>
    Ok
}
public enum WeaponEquip
{
    /// <summary>
    /// 未知错误
    /// </summary>
    none,
    /// <summary>
    /// 装备的格子不正确
    /// </summary>
    Type,
    /// <summary>
    /// 装备就是当前的
    /// </summary>
    same,
    /// <summary>
    /// 成功
    /// </summary>
    Ok
}
public enum WeaponChange
{
    /// <summary>
    /// 所有容器发生了变化
    /// </summary>
    All,
    /// <summary>
    /// 装备列表发生了变化
    /// </summary>
    WeaponList,
    /// <summary>
    /// 已装备列表发生了变化
    /// </summary>
    EquiptList,
    /// <summary>
    /// 特定的Slot的装备发生了变化
    /// </summary>
    One
}
public enum EquipControl
{
    /// <summary>
    /// 装备出售
    /// </summary>
    SellEquipResp,
    /// <summary>
    /// 武将装备升星
    /// </summary>
    SoldierEquipStarResp,
    /// <summary>
    /// 穿装备
    /// </summary>
    PutonEquipAndArtifactResp,
    /// <summary>
    /// 卸下装备
    /// </summary>
    GetoffEquipAndArtifactResp,
    /// <summary>
    /// 强化装备
    /// </summary>
    PromoteEquipAndArtifactResp,
    /// <summary>
    /// 一键装备
    /// </summary>
    OneKeyPutOnAllEquipResp,
    /// <summary>
    /// 一键强化所有装备
    /// </summary>
    OneKeyPromoteAllEquipResp,
    /// <summary>
    /// 一键强化单件装备
    /// </summary>
    OneKeyPromoteOneResp,
    /// <summary>
    /// 神兵
    /// </summary>
    EquipUpQualityResp,
    /// <summary>
    /// 一键卸装
    /// </summary>
    OneKeyOffEquipResp
}
public enum EquiptType//武器的类型如配置表的对应
{
    _Weapon = 0,
    _ring = 1,
    _necklace = 2,
    _clothing = 3,
    _mount = 4
}
public class ShowInfoWeapon : ShowInfoBase
{
    public ShowInfoWeapon()
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
public class Weapon
{
    public static int MAXQUALITY = 6;
    public static int MAXSTAR = 6;
    /// <summary>
    /// true为锁住，false为解锁
    /// </summary>
    private bool _isLock = false;
    /// <summary>
    /// 装备配置表数据
    /// </summary>
    private EquipAttributeInfo att;
    /// <summary>
    /// 解锁信息
    /// </summary>
    private GodEquipLockInfo lockInfo;
    /// <summary>
    /// 效果器
    /// </summary>
    public Skill _skill;
    /// <summary>
    /// 图标
    /// </summary>
    public Texture Icon = null;
    /// <summary>
    /// 等级
    /// </summary>
    public int Level;
    /// <summary>
    /// 唯一ID
    /// </summary>
    public UInt64 uId = 0;
    /// <summary>
    /// 在所有装备列表中的索引
    /// </summary>
    public int Slot;
    /// <summary>
    /// 是否已装备
    /// </summary>
    public bool isEquiped = false;
    /// <summary>
    /// 装备加成属性
    /// </summary>
    public ShowInfoWeapon InfoAttribute;
    /// <summary>
    /// 升星经验
    /// </summary>
    public int starExp;
    /// <summary>
    /// 获取装备的配置表数据
    /// </summary>
    public EquipAttributeInfo Att { get { return att; } }
    public GodEquipLockInfo godEquipLockInfo { get { return this.lockInfo; } }
    public Dictionary<int, uint> MaterialBagList { get { return att.MaterialBagList; } }//获取装备升级需要的材料
    public int Lv { get { return this.Level; } }//获取装备当前等级
    public Skill _Skill { get { return this._skill; } }//获取效果器
    //装备的创建函数
    public bool IsLock { get { return this._isLock; } set { this._isLock = value; } }
    public static Weapon createByID(uint Id)
    {
        Weapon tempWeapon = new Weapon();
        tempWeapon.att = ConfigManager.Instance.mEquipData.FindById(Id);
        if (tempWeapon.att == null) return null;

        //ResourceLoadManager.Instance.LoadAloneImage(tempWeapon.att.icon, (texture) =>
        //{
        //    tempWeapon.icon = texture;
        //});
        tempWeapon.Level = tempWeapon.att.baseLevel;

        tempWeapon.InfoAttribute = new ShowInfoWeapon();
        tempWeapon.InfoAttribute.HP = tempWeapon.att.hp_max;
        tempWeapon.InfoAttribute.Attack = tempWeapon.att.phy_atk;
        tempWeapon.InfoAttribute.Accuracy = tempWeapon.att.acc_rate;
        tempWeapon.InfoAttribute.Dodge = tempWeapon.att.ddg_rate;
        tempWeapon.InfoAttribute.Crit = tempWeapon.att.crt_rate;
        tempWeapon.InfoAttribute.Tenacity = tempWeapon.att.tnc_rate;
        tempWeapon.InfoAttribute.MoveSpeed = tempWeapon.att.speed;

        tempWeapon.lockInfo = ConfigManager.Instance.mGodEquipLockData.GetGodEquipLockInfoByID(tempWeapon.GetRootId());

        return tempWeapon;
    }
    public void LoadImage(System.Action<Texture> onload)
    {
        if (this.Icon != null)
        {
            onload(this.Icon);
            return;
        }
        ResourceLoadManager.Instance.LoadAloneImage(this.att.icon, (texture) =>
        {
            this.Icon = texture;
            onload(this.Icon);
        });
    }
    /// <summary>
    /// 获取装备的根节点
    /// </summary>
    /// <returns>返回根节点ID，如果返回为0则代表配置表出错</returns>
    public uint GetRootId()
    {
        List<uint> IdMark = new List<uint>(11);
        IdMark.Add(this.att.id);
        while (true)
        {
            uint tempId = IdMark[IdMark.Count - 1];
            EquipAttributeInfo temp = ConfigManager.Instance.mEquipData.FindById(tempId);
            if (temp == null)
                break;
            if (temp.prepositionId == 0)
            {
                IdMark.Add(temp.id);
                break;
            }
            if (IdMark.Contains(temp.prepositionId))
                break;
            IdMark.Add(temp.prepositionId);
        }
        if (IdMark.Count > 0)
            return IdMark[IdMark.Count - 1];
        return 0;
    }

    /// <summary>
    /// 刷新装备属性加成
    /// </summary>
    public void RefreshInfoAtt(fogs.proto.msg.Attribute att)
    {
        this.InfoAttribute.HP = att.hp_max;
        this.InfoAttribute.Attack = att.phy_atk;
        this.InfoAttribute.Accuracy = att.acc_rate;
        this.InfoAttribute.Dodge = att.ddg_rate;
        this.InfoAttribute.Crit = att.crt_rate;
        this.InfoAttribute.Tenacity = att.tnc_rate;
        this.InfoAttribute.MoveSpeed = att.speed;
    }
    public ShowInfoWeapon GetNextLevelShowInfo()
    {
        ShowInfoWeapon temp = new ShowInfoWeapon();
        temp.CopyTo(this.InfoAttribute);

        temp.HP += this.att.u_hp;
        temp.Attack += this.att.u_attack;
        temp.Accuracy += this.att.u_accuracy;
        temp.Dodge += this.att.u_dodge;
        temp.Crit += this.att.u_crit;
        temp.Tenacity += this.att.u_tenacity;
        temp.MoveSpeed += this.att.u_speed;
        return temp;
    }
    public bool Serialize(Equip eq)
    {
        if (eq.postion > 0 && eq.postion < 9)
        {
            this.isEquiped = true;
        }
        if (this.InfoAttribute != null)
        {
            this.Strong(eq.level - this.Level);
        }
        this.Level = eq.level;
        this.uId = eq.uid;
        this.starExp = eq.start_exp;
        if (eq.skill != null)
        {
            this._skill = null;
            this._skill = Skill.createByID(eq.skill.id);
            if (this._skill == null)
                return false;
            this._skill.Serialize(eq.skill);
        }
        
        return true;
    }
    /// <summary>
    /// 获取装备的Equip属性
    /// </summary>
    public Equip getEquipAtt()
    {
        Equip result = new Equip();
        result.id = this.att.id;
        result.level = this.Level;
        result.uid = this.uId;
        result.postion = 0;
        return result;
    }
    /// <summary>
    /// 获取作为材料提供的经验
    /// </summary>
    /// <returns></returns>
    public int getBeExp()
    {
        uint rootId = this.GetRootId();
        float exp = 0;
        while(rootId != this.att.id)
        {
            EquipAttributeInfo tempInfo = ConfigManager.Instance.mEquipData.FindById(rootId);
            exp += (float)tempInfo.evolveExp;
            rootId = tempInfo.evolveId;
        }
        exp += this.starExp;
        exp *= 0.7f;
        exp += this.att.materialExp;
        return (int)exp;
    }
    public KeyValuePair<string, List<string>> getRegionsAndAtlas()
    {
        return new KeyValuePair<string, List<string>>();
    }
    public WeaponCheck enableStrong(bool needCheckGoods = true)//检查是否能够升级
    {
        return _check(needCheckGoods);
    }

    public WeaponCheck enableUpStar()
    {
        if (att.godEquip != 2) return WeaponCheck.none;
        if (this._isLock) return WeaponCheck.none;
        if (isMaxLevel())
        {
            EquipAttributeInfo temp = ConfigManager.Instance.mEquipData.FindById(this.att.evolveId);
            if (temp == null)
                return WeaponCheck.none;

            return WeaponCheck.Ok;
        }
        return WeaponCheck.none;
    }
    public string Strong()//升级
    {
        _strong();
        return "";
    }
    public bool isMaxLevel()
    {
        return this.Level >= att.evolveLevel;
    }

    public Weapon()
    {
        this.Slot = -1;
    }
    private string _strong()
    {
        ++this.Level;
        if (this.InfoAttribute != null)
        {
            this.InfoAttribute.HP += this.att.u_hp;
            this.InfoAttribute.Attack += this.att.u_attack;
            this.InfoAttribute.Accuracy += this.att.u_accuracy;
            this.InfoAttribute.Dodge += this.att.u_dodge;
            this.InfoAttribute.Crit += this.att.u_crit;
            this.InfoAttribute.Tenacity += this.att.u_tenacity;
            this.InfoAttribute.MoveSpeed += this.att.u_speed;
        }
        return "";
    }
    public void Strong(int Level)
    {
        this.Level += Level;
        if (this.InfoAttribute != null)
        {
            this.InfoAttribute.HP += this.att.u_hp * Level;
            this.InfoAttribute.Attack += this.att.u_attack * Level;
            this.InfoAttribute.Accuracy += this.att.u_accuracy * Level;
            this.InfoAttribute.Dodge += this.att.u_dodge * Level;
            this.InfoAttribute.Crit += this.att.u_crit * Level;
            this.InfoAttribute.Tenacity += this.att.u_tenacity * Level;
            this.InfoAttribute.MoveSpeed += this.att.u_speed * Level;
        }
    }
    private WeaponCheck _check(bool needCheckGoods = true)
    {
        if (this._isLock)
            return WeaponCheck.none;
        if(att.godEquip == 2)
        {
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.SoldierEquip))
                return WeaponCheck.Lock;

            if (isMaxLevel())
            {
                EquipAttributeInfo temp = ConfigManager.Instance.mEquipData.FindById(this.att.evolveId);
                if (temp == null)
                    return WeaponCheck.MaxLevel;
                return WeaponCheck.star;
            }
            else
            {
                ///2016-1-22 tanglan SoldierEquip dont Limit Hero Level
                if (this.Level >= PlayerData.Instance._Level)
                {
                    return WeaponCheck.HeroLevelLower;
                }
                int lv = this.Level - this.Att.baseLevel;
                if (!this.Att.MaterialBagList.ContainsKey(lv + 1))return WeaponCheck.Goods;

                var result = MaterialBag.getResult(this.Att.MaterialBagList[lv + 1]);
                if (result.Key.Key == MaterialBag.MaterialResult.noId)return WeaponCheck.Goods;
                if (result.Key.Key == MaterialBag.MaterialResult.coin || result.Key.Key == MaterialBag.MaterialResult.diamond)
                    return WeaponCheck.Money;
                if (result.Key.Key == MaterialBag.MaterialResult.material)return WeaponCheck.Goods;
            }
        }
        else
        {
            if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
                return WeaponCheck.Lock;

            if (this.Level >= PlayerData.Instance._Level)
            {
                return WeaponCheck.HeroLevelLower;
            }
            int lv = this.Level - this.Att.baseLevel;

            if (this.Att.MaterialBagList == null)
                return WeaponCheck.Lock;

            if (!this.Att.MaterialBagList.ContainsKey(lv + 1) && needCheckGoods)
                return WeaponCheck.Goods;

            var result = MaterialBag.getResult(this.Att.MaterialBagList[lv + 1]);
            if (result.Key.Key == MaterialBag.MaterialResult.noId && needCheckGoods)
                return WeaponCheck.Goods;
            if (result.Key.Key == MaterialBag.MaterialResult.coin || result.Key.Key == MaterialBag.MaterialResult.diamond)
                return WeaponCheck.Money;
            if (result.Key.Key == MaterialBag.MaterialResult.material && needCheckGoods)
                return WeaponCheck.Goods;
            if (isMaxLevel())
            {
                EquipAttributeInfo temp = ConfigManager.Instance.mEquipData.FindById(this.att.evolveId);
                if (temp == null)
                    return WeaponCheck.MaxLevel;
                return WeaponCheck.Level;
            }
        }
        return WeaponCheck.Ok;
    }
}
public class WeaponsDepot
{
    /// <summary>
    /// 装备列表
    /// </summary>
    public List<Weapon> _weaponList;
    ///// <summary>
    ///// 替代装备列表
    ///// </summary>
    //public List<Weapon> tempList;

    public List<EquipedDepotBase> _EquipedList;

    public bool needSort = false;
    public bool needRemoveNull = false;

    /// <summary>
    /// 装备栏发生了变化
    /// </summary>
    /// <param name="change"></param>
    /// <param name="Slot"></param>
    /// <param name="uID"></param>
    public delegate void WeaponDepotDelet(WeaponChange change, int Slot = -1, UInt64 uID = 0);
    public event WeaponDepotDelet WeaponDepotEvent;

    public delegate void ErrotDelete(EquipControl control, int errorCode);
    public event ErrotDelete ErrotDeleteEvent;

    public delegate bool Filter(Weapon wp);
    public void OnWeaponChange(WeaponChange change, int Slot = -1, UInt64 uID = 0)
    {
        if (this.WeaponDepotEvent != null)
        {
            this.WeaponDepotEvent(change, Slot, uID);
        }
    }
    public void OnErrotChange(EquipControl control, int errorCode)
    {
        if (this.ErrotDeleteEvent != null)
        {
            this.ErrotDeleteEvent(control, errorCode);
        }
    }
    public void Clear()
    {
        this._weaponList.Clear();
        this._EquipedList.Clear();
        this.needSort = false;
        this.needRemoveNull = false;
    }
    //加入依附的装备列表对象（不用操作，子已经自动操作）
    public void AddChild(EquipedDepotBase child)
    {
        _EquipedList.Add(child);
    }
    public void RemoveChild(EquipedDepotBase del)
    {
        _EquipedList.Remove(del);
    }
    public void ClearChild()
    {
        _EquipedList.Clear();
    }
    public void Update()
    {
        _RemoveNull();
        _sort();
    }
    /// <summary>
    /// 通过slot找装备
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public Weapon FindBySlot(int slot)
    {
        return _weaponList.Find((Weapon p) => { return p.Slot == slot; });
    }
    /// <summary>
    /// 通过唯一ID找装备
    /// </summary>
    /// <param name="uId"></param>
    /// <returns></returns>
    public Weapon FindByUid(UInt64 uId)
    {
        return _weaponList.Find((Weapon p) => { if (p == null) return false; return p.uId == uId; });
    }
    public List<Weapon> FindById(uint id)
    {
        return _weaponList.FindAll((Weapon p) => { if (p == null)return false; return p.Att.id == id; });
    }
    /// <summary>
    /// 添加一组ID的装备，添加失败会返回false，但是会把能添加的装备添加进去
    /// </summary>
    /// <param name="Slot"></param>
    /// <returns></returns>
    public bool MultipleAdd(List<uint> Slot)
    {
        bool success = true;

        foreach (var temp in Slot)
        {
            if (!_Add(temp)) success = false;
        }
        return success;
    }
    /// <summary>
    /// 添加一个ID的装备
    /// </summary>
    /// <param name="Slot"></param>
    /// <returns></returns>
    public bool OneAdd(uint Slot)
    {
        return _Add(Slot);
    }

    public Weapon OneAdd(Equip eq)
    {
        if (this.FindByUid(eq.uid) != null)
            return null;
        Weapon wp = Weapon.createByID(eq.id);

        if (wp == null) return null;
        wp.Serialize(eq);
        this._weaponList.Add(wp);
        wp.Slot = this._weaponList.Count - 1;
        this.needSort = true;
        return wp;
    }

    /// <summary>
    /// 批量删除
    /// </summary>
    /// <param name="ID"></param>
    /// <returns></returns>
    public bool MultipleDelete(List<UInt64> ID)
    {
        bool success = true;

        foreach (var temp in ID)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp);
            if (wp == null)
                continue;

            if (!_Delete(wp.Slot)) success = false;
        }
        return success;
    }
    /// <summary>
    /// 单个删除
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public bool OneDelete(uint Id)
    {
        return _Delete((int)Id);
    }
    /// <summary>
    /// 测试装备是否能够强化
    /// </summary>
    /// <param name="slot">装备的索引</param>
    /// <returns></returns>
    public WeaponCheck TextStrong(int slot)
    {
        if (slot >= _weaponList.Count) return WeaponCheck.none;
        return _weaponList[slot].enableStrong();
    }
    /// <summary>
    /// 强化装备
    /// </summary>
    /// <param name="slot">装备的索引</param>
    /// <returns></returns>
    public string Strong(int slot)
    {
        if (slot >= _weaponList.Count) return "Equipt";

        Weapon temp = _weaponList[slot];
        switch (temp.enableStrong(false))
        {
            case WeaponCheck.HeroLevelLower: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.HeroLevelLower); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.HeroLevelLower); break;
            case WeaponCheck.SoldierLevelLower: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.SoldierLevelLower); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.SoldierLevelLower); break;
            case WeaponCheck.MaxLevel: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.HadMaxLevel); UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_EQUIPA_ALLLEVEL); break;
            case WeaponCheck.Goods: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.NotEnoughMaterial); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.NotEnoughMaterial); break;
            case WeaponCheck.star: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, 304); ErrorCode.ShowErrorTip(304); break;
            case WeaponCheck.Money: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.NotEnoughGold); ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold); UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW); break;
            case WeaponCheck.Level: EquipModule.Instance.SendPromoteEquipReq(temp.uId, SoliderUidFind(temp), EquipActionType.ADVANCED); break;
            case WeaponCheck.Ok: EquipModule.Instance.SendPromoteEquipReq(temp.uId, SoliderUidFind(temp), EquipActionType.STRENGTH);  break;
        }
        return "Equipt";
    }
    public UInt64 SoliderUidFind(Weapon wp)
    {
        if (wp == null)
            return 0;
        if (!wp.isEquiped)
        {
            if (PlayerData.Instance._WeaponDepot.FindByUid(wp.uId) != null)
                return 0;
            if (PlayerData.Instance._SoldierEquip.FindByUid(wp.uId) != null)
                return 1;
        }
        Soldier temp = PlayerData.Instance._SoldierDepot._soldierList.Find((soldier) =>
        {
            if (soldier == null)
                return false;
            return soldier._equippedDepot._EquiptList.Find((Weapon weapon) => { if (weapon == null) return false; return weapon.uId == wp.uId; }) != null;
        });
        if (temp != null)
            return temp.uId;
        Weapon tempWp = PlayerData.Instance._ArtifactedDepot._EquiptList.Find((weapon) => { if (weapon == null) return false; return weapon.uId == wp.uId; });
        if (tempWp != null)
            return 0;
        return 0;
    }
    /// <summary>
    /// 装备进阶
    /// </summary>
    /// <param name="slot"></param>
    /// <returns></returns>
    public string Evolve(int slot)
    {
        if (slot >= _weaponList.Count) return "Equipt";

        Weapon temp = _weaponList[slot];
        if (temp.isMaxLevel())
            _evolve(slot);
        return "Evolve";
    }
    /// <summary>
    /// 装备升星
    /// </summary>
    /// <param name="slot"></param>
    /// <param name="uIdList"></param>
    /// <returns></returns>
    public string UpStar(int slot, List<UInt64> uIdList)
    {
        if (slot >= _weaponList.Count) return "UpStar";
        Weapon temp = _weaponList[slot];

        if (temp == null)
            return "";

        int exp = 0;
        foreach (UInt64 uid in uIdList)
        {
            Weapon tempWp = PlayerData.Instance._SoldierEquip.FindByUid(uid);
            if (tempWp == null)
                continue;
            exp += tempWp.getBeExp();
        }
        switch (temp.enableUpStar())
        {
            case WeaponCheck.none: break;
            case WeaponCheck.Ok: EquipModule.Instance.SendSoldierEquipStarReq(temp.uId, UpStarEquipUidFind(temp), uIdList); break;
        }
        return "";
    }
    /// <summary>
    /// 装备出售
    /// </summary>
    /// <param name="list">需要出售的装备</param>
    /// <returns></returns>
    public bool Sale(UInt64 uid)
    {
        EquipModule.Instance.SendSellEquipReq(uid);
        return true;
    }
    public void OneKeyStrong(UInt64 uid)
    {
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.OneKeyEnchance, true))
            return;
        Weapon temp = this.FindByUid(uid);
        if (temp == null) return;

        switch (temp.enableStrong())
        {
            case WeaponCheck.HeroLevelLower: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.HeroLevelLower); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.HeroLevelLower); break;
            case WeaponCheck.SoldierLevelLower: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.SoldierLevelLower); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.SoldierLevelLower); break;
            case WeaponCheck.MaxLevel: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.HadMaxLevel); UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.SOLDIER_EQUIPA_ALLLEVEL); break;
            case WeaponCheck.Goods: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.NotEnoughMaterial); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.NotEnoughMaterial); break;
            case WeaponCheck.star: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, 304); ErrorCode.ShowErrorTip(304); break;
            case WeaponCheck.Money: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.NotEnoughGold); ErrorCode.ShowErrorTip((int)ErrorCodeEnum.NotEnoughGold); UISystem.Instance.ShowGameUI(ViewType.DIR_VIEWNAME_BUY_COIN_VIEW); break;
            case WeaponCheck.Level: if (ErrotDeleteEvent != null) ErrotDeleteEvent(EquipControl.PromoteEquipAndArtifactResp, (int)ErrorCodeEnum.HadMaxLevel); ErrorCode.ShowErrorTip((uint)ErrorCodeEnum.HadMaxLevel); break;
            case WeaponCheck.Ok: ulong id = SoliderUidFind(temp); if (id == 1) id = 0; EquipModule.Instance.SendOneKeyPromoteOneReq(uid, id); break;
        }
    }
    public void ReceiveSellEquipResp(SellEquipResp data)
    {
        this.needRemoveNull = true;
        this.needSort = true;
        Weapon de = this.FindByUid(data.uid);
        if (de == null)
            return;
        if (de != null)
        {
            this._weaponList.Remove(de);
        }
        //_RemoveNull();
        //_sort();

        PlayerData.Instance.MoneyChange(data.money_type, data.money_num);
    }
    private UInt64 UpStarEquipUidFind(Weapon wp)
    {
        if (wp == null)
            return 0;
        if (!wp.isEquiped)
            return 0;
        Soldier temp = PlayerData.Instance._SoldierDepot._soldierList.Find((soldier) =>
        {
            if (soldier == null)
                return false;
            return soldier._equippedDepot._EquiptList.Find((Weapon weapon) => { if (weapon == null) return false; return weapon.uId == wp.uId; }) != null;
        });
        if (temp != null)
            return temp.uId;
        return 0;
    }
    public void ReceiveSoldierEquipStarResp(SoldierEquipStarResp tData)
    {
        Weapon wp = this.FindByUid(tData.updata_equip.uid);
        _upStar(wp, tData.uid_list,tData);
    }
    public void ReceiveOneKeyPromoteOneResp(OneKeyPromoteOneResp tData)
    {
        this.OneKeyStrong(tData.equip);
    }
    private void OneKeyStrong(fogs.proto.msg.Equip equip)
    {
        if(equip == null)
        {
            Debug.LogError("equip is null");
            return;
        }
        Weapon temp = PlayerData.Instance._SoldierEquip.FindByUid(equip.uid);
        if (temp == null)
            return;
        temp.Serialize(equip);

        this.OnWeaponChange(WeaponChange.EquiptList, temp.Slot, temp.uId);
        this.needSort = true;
    }
    private void _upStar(Weapon wp, List<UInt64> uIdList, SoldierEquipStarResp tData)
    {
        foreach (UInt64 tp in uIdList)
        {
            Weapon de = this.FindByUid(tp);
            if (de != null)
                this._Delete(de.Slot);
        }
        _RemoveNull();
        _sort();
        if (tData.updata_equip.id == wp.Att.id)
        {
            wp.starExp = tData.updata_equip.start_exp;
            this.OnWeaponChange(WeaponChange.EquiptList, wp.Slot, wp.uId);
            return;
        }
        UInt64 uid = wp.uId;
        var temp = _evolve(wp.Slot);
        if (temp == null)
            return;
        temp.uId = uid;
        temp.starExp = tData.updata_equip.start_exp;
    }
    //获取所有装备列表
    public List<Weapon> getWeaponList()
    {
        return _weaponList;
    }
    /// <summary>
    /// 获取筛选器过滤过后的装备列表
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public List<Weapon> getWeaponList(Filter filter)
    {
        if (this._weaponList == null)
            return new List<Weapon>();
        List<Weapon> tempList = new List<Weapon>(this._weaponList.Count + 1);
        foreach (var temp in _weaponList)
        {
            if (filter(temp))
                tempList.Add(temp);
        }
        return tempList;
    }
    public List<Weapon> GetLockAndUnlockList()
    {
        List<GodEquipLockInfo> lockList = ConfigManager.Instance.mGodEquipLockData.GetGodEquipLockList();
        List<Weapon> unLockList = new List<Weapon>(11);
        List<Weapon> resultList = new List<Weapon>(PlayerData.Instance._WeaponDepot._weaponList.Count + 1);
        resultList.AddRange(this._weaponList);
        if (this._weaponList.Count > 0)
        {
            if (this._weaponList[0].Att.godEquip == 2)
                return resultList;
        }

        for (int i = 0; i < lockList.Count; ++i)
        {
            uint tmpId = lockList[i].id;
            Weapon tmpWeapon = PlayerData.Instance._WeaponDepot._weaponList.Find((wp) => { if (wp == null)return false; return wp.GetRootId() == tmpId; });
            if (tmpWeapon == null)
            {
                tmpWeapon = Weapon.createByID(tmpId);
                if (tmpWeapon == null)
                    continue;
                tmpWeapon.IsLock = true;
                unLockList.Add(tmpWeapon);
                tmpWeapon = null;
            }
        }
        //unLockList.Sort((left, right) =>
        //{
        //    if (left.Lv != right.Lv)
        //    {
        //        if (left.Lv > right.Lv) return -1;
        //        return 1;
        //    }
        //    if (left.Att.star != right.Att.star)
        //    {
        //        if (left.Att.star > right.Att.star) return -1;
        //        return 1;
        //    }
        //    if (left.Att.quality != right.Att.quality)
        //    {
        //        if (left.Att.quality > right.Att.quality) return -1;
        //        return 1;
        //    }

        //    if (left.Att.id > right.Att.id) return 1;
        //    if (left.Att.id < right.Att.id) return -1;
        //    return 0;
        //});

        resultList.AddRange(unLockList);
        return resultList;
    }
    public List<Weapon> GetLockAndUnlockList(Filter filter)
    {
        List<Weapon> result = new List<Weapon>(this.GetLockAndUnlockList());
        List<Weapon> tempList = new List<Weapon>();
        tempList.Capacity = _weaponList.Count;
        foreach (var temp in result)
        {
            if (filter(temp))
                tempList.Add(temp);
        }
        return tempList;
    }

    public WeaponsDepot()
    {
        _weaponList = new List<Weapon>();
        _EquipedList = new List<EquipedDepotBase>();
        Scheduler.Instance.AddUpdator(Update);
    }
    ~WeaponsDepot()
    {
        Scheduler.Instance.RemoveUpdator(Update);
    }
    //初始化
    public bool Serialize(List<fogs.proto.msg.Equip> equipList)
    {
        foreach (var temp in equipList)
        {
            Weapon wp = Weapon.createByID(temp.id);
            if (wp == null)
            {
                continue;
            }
            if (temp.postion > 0)
            {
                continue;
            }
            wp.Serialize(temp);

            _weaponList.Add(wp);
            wp.Slot = _weaponList.Count - 1;
        }
        needSort = true;
        return true;
    }
    public void ReceivePutonEquipResp(PutonEquipResp data)
    {

    }

    public void ReceivePromoteEquipResp(PromoteEquipResp data)
    {
        Weapon wp = this.FindByUid(data.uid);
        if (data.result == 0)
        {
            if (wp == null)
                return;
            foreach (var temp in data.item_list)
            {
                Item tempIt = PlayerData.Instance._MaterialBag.Find((Item it) => { return it.id == temp.id; });
                if (tempIt != null)
                {
                    tempIt.num = temp.num;
                }
            }
            if (wp._skill != null && data.equip_info.skill != null)
            {
                Skill temp = Skill.createByID(data.equip_info.skill.id);
                if (temp != null)
                {
                    wp._skill = null;
                    wp._skill = temp;
                    wp._skill.Serialize(data.equip_info.skill);
                }
            }
            if (data.type == EquipActionType.STRENGTH)
            {
                wp.Strong();
                if (WeaponDepotEvent != null)
                    WeaponDepotEvent(WeaponChange.WeaponList, wp.Slot, wp.uId);
                if (wp.isEquiped)
                {
                    if (WeaponDepotEvent != null)
                        WeaponDepotEvent(WeaponChange.EquiptList, wp.Slot, wp.uId);
                }
            }
            if (data.type == EquipActionType.ADVANCED)
            {
                _evolve(wp.Slot);
                if (WeaponDepotEvent != null)
                    WeaponDepotEvent(WeaponChange.WeaponList, wp.Slot, wp.uId);
                if (wp.isEquiped)
                {
                    if (WeaponDepotEvent != null)
                        WeaponDepotEvent(WeaponChange.EquiptList, wp.Slot, wp.uId);
                }
            }

        }
    }
    private Weapon _evolve(int slot)
    {
        if (slot >= _weaponList.Count) return null;

        var temp = _weaponList[slot];
        var evoTemp = Weapon.createByID(temp.Att.evolveId);
        if (evoTemp == null) return null;

        evoTemp.Slot = slot;
        evoTemp.uId = temp.uId;
        evoTemp.isEquiped = temp.isEquiped;
        evoTemp._skill = temp._skill;
        
        if (evoTemp.isEquiped)
        {
            var find = _EquipedList.FindAll((EquipedDepotBase a) =>
            {
                if (a == null) return false;
                return a._EquiptList.Find((Weapon b) => { if (b == null) return false; return b.Slot == slot; }) != null;
            });
            foreach (var tp in find)
            {
                int index = tp._EquiptList.FindIndex((Weapon a) => { if (a == null) return false; return a.Slot == slot; });
                if (index != -1)
                {
                    tp._EquiptList[index] = evoTemp;
                }
            };
        }

        _weaponList[slot] = null;
        temp = null;
        _weaponList[slot] = evoTemp;
        this.OnWeaponChange(WeaponChange.EquiptList, evoTemp.Slot, evoTemp.uId);
        needSort = true;
        return evoTemp;
    }
    private bool _Add(uint Id)
    {
        Weapon wp = Weapon.createByID(Id);
        if (wp == null) return false;

        _weaponList.Add(wp);
        wp.Slot = _weaponList.Count - 1;
        needSort = true;
        return true;
    }
    private bool _Delete(int Slot)
    {
        if (Slot >= _weaponList.Count) return false;
        Weapon wp = _weaponList[Slot];

        if (wp != null)
        {
            if (wp.isEquiped)
            {
                if (wp.Att.godEquip == 2)
                {
                    Soldier mySoldier = PlayerData.Instance._SoldierDepot._soldierList.Find
                        (
                        (Soldier sd) =>
                        {
                            if (sd == null) return false;
                            return sd._equippedDepot._EquiptList.Contains(wp);
                        }
                        );
                    if (mySoldier != null)
                    {
                        int index = mySoldier._equippedDepot._EquiptList.FindIndex((tmpWp) => { if (tmpWp == null)return false; return tmpWp.uId == wp.uId; });
                        if (index != -1)
                            mySoldier._equippedDepot._EquiptList[index] = null;
                    }
                }
            }

        }

        if (wp._Skill != null)
        {
            PlayerData.Instance._SkillsDepot._skillsList.Remove(wp._Skill);
            wp._skill = null;
        }

        _weaponList[Slot] = null;

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
    1、根据装备的是否穿着状态排序，未装备的装备排序靠前，已装备的排序靠后；
    2、根据装备的强化等级排序，强化等级越高的排序靠前；
    3、根据装备的星级排序，星级越高的排序越靠前；
    4、根据装备的品质排序，品质越高的排序越靠前；
    5、根据装备的ID排序，ID越小的排序越靠前；
    */
    private void _sort()
    {
        if (!needSort) return;
        //if (_weaponList.Count < 2) return;
        _weaponList.Sort
            (
            (left, right) =>
            {
                if (left.isEquiped != right.isEquiped)
                {
                    if (!left.isEquiped) return -1;
                    else return 1;
                }
                if (left.Lv != right.Lv)
                {
                    if (left.Lv > right.Lv) return -1;
                    return 1;
                }
                if (left.Att.star != right.Att.star)
                {
                    if (left.Att.star > right.Att.star) return -1;
                    return 1;
                }
                if (left.Att.quality != right.Att.quality)
                {
                    if (left.Att.quality > right.Att.quality) return -1;
                    return 1;
                }

                if (left.Att.id > right.Att.id) return 1;
                if (left.Att.id < right.Att.id) return -1;
                return 0;
            }
            );
        for (int i = 0; i < _weaponList.Count; ++i)
        {
            _weaponList[i].Slot = i;
        }
        needSort = false;
        if (WeaponDepotEvent != null)
        {
            WeaponDepotEvent(WeaponChange.WeaponList);
        }
    }
    private void _RemoveNull()
    {
        if (!needRemoveNull) return;
        needRemoveNull = false;
        _weaponList.RemoveAll((temp) => { return temp == null; });
    }
}
public class EquipedDepotBase
{
    public List<Weapon> _EquiptList;
    public WeaponsDepot father;
    //装备栏发生了变化
    //绑定依赖装备列表对象
    public virtual void BindFather(WeaponsDepot fa)
    {
        return;
    }
    public void Clear()
    {
        for (int i = 0; i < this._EquiptList.Count; ++i)
        {
            this._EquiptList[i] = null;
        }
    }
    public void ListChange(int index, UInt64 uid = 0)
    {
        if (father == null)
            return;

        father.OnWeaponChange(WeaponChange.EquiptList, index, uid);
    }
    public bool IsHadEquipCanStrong()
    {
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
        {
            return false;
        }
        if (this._EquiptList == null)
            return false;
        foreach (Weapon wp in this._EquiptList)
        {
            if (wp == null)
                continue;
            if (wp.enableStrong() == WeaponCheck.Ok || wp.enableStrong() == WeaponCheck.Level)
                return true;
        }
        return false;
    }
    /// <summary>
    /// 是否穿了有装备
    /// </summary>
    /// <returns></returns>
    public bool IsEquiped()
    {
        foreach (Weapon wp in this._EquiptList)
        {
            if (wp != null)
                return true;
        }
        return false;
    }
    //装备装备
    public virtual bool installEquipt(int sourceSlot, int destIndex, string message)
    {
        return false;
    }
    //卸载装备
    public virtual bool uninstallEquipt(UInt64 weaponUid)
    {
        return false;
    }
    public EquipedDepotBase()
    {

    }
    ~EquipedDepotBase()
    {

    }
    public bool Serialize(List<fogs.proto.msg.Equip> equipList)
    {
        foreach (var temp in equipList)
        {
            if (temp == null)
                continue;
            if (temp.postion > 0 && temp.postion < 9)
            {
                Weapon euqip = null;

                if (father != null)
                {
                    if (father.FindByUid(temp.uid) == null)
                    {
                        euqip = father.OneAdd(temp);
                    }
                    else
                    {
                        euqip = father.FindByUid(temp.uid);
                    }
                }
                else
                {
                    euqip = Weapon.createByID(temp.id);

                    if (euqip == null) continue;
                    euqip.Serialize(temp);
                }
                if (euqip == null) continue;
                euqip.isEquiped = true;
                _EquiptList[temp.postion - 1] = euqip;
            }
        }
        return true;
    }
    public void RefreshList(List<fogs.proto.msg.Equip> equipList)
    {
        foreach (var temp in equipList)
        {
            if (temp == null)
                continue;
            int index = this._EquiptList.FindIndex((wp) => { if (wp == null) return false; return wp.uId == temp.uid; });
            if (index != -1)
            {
                this._EquiptList[index].isEquiped = false;
                this._EquiptList[index] = null;
            }
            if (temp.postion > 0 && temp.postion < 9)
            {
                Weapon euqip = father.FindByUid(temp.uid);
                if (euqip == null)
                    continue;
                Weapon tempWeapon = this._EquiptList[temp.postion - 1];
                if (tempWeapon != null)
                    tempWeapon.isEquiped = false;
                this._EquiptList[temp.postion - 1] = euqip;
                euqip.isEquiped = true;
            }
            if (this.father != null)
                this.father.needSort = true;
        }
    }
    public virtual void ReceivePutonEquipResp(Weapon source, int destIndex, int type, UInt64 other_soldierUid, fogs.proto.msg.Equip other_euqip)
    {
        return;
    }
    public virtual void ReceiveGetoffEquipResp(UInt64 uId)
    {
        return;
    }
    protected virtual WeaponEquip _TextEquipted(int sourceSlot, int destIndex)
    {
        return WeaponEquip.none;
    }
    protected void _Equipt(Weapon source, int destIndex, int type, UInt64 other_soldierUid, fogs.proto.msg.Equip other_euqip)
    {
        if (source == null) return;
        if (father == null)
            return;

        if (destIndex > _EquiptList.Count || destIndex < 0) return;
        var srcTemp = source;
        if (srcTemp == null) return;
        Weapon desTemp = _EquiptList[destIndex];
        if (srcTemp.isEquiped)
        {
            EquipedDepotBase tempDp;
            int tempWp = -1;
            tempDp = father._EquipedList.Find
                ((EquipedDepotBase p) =>
                {
                    tempWp = p._EquiptList.FindIndex(
                        (Weapon wp) =>
                        {
                            return wp == srcTemp;
                        });
                    return tempWp != -1;
                });
            //int slot = _EquiptList.FindIndex((Weapon p) => { return srcTemp == p; });
            if (tempWp == -1) return;

            if (type == 0)
            {
                tempDp._EquiptList[tempWp] = desTemp;
                _EquiptList[destIndex] = srcTemp;
            }
            else
            {
                tempDp._EquiptList[tempWp] = null;
                if (desTemp != null) desTemp.isEquiped = false;
                _EquiptList[destIndex] = srcTemp;
            }
        }
        else
        {
            if (desTemp != null) desTemp.isEquiped = false;
            _EquiptList[destIndex] = srcTemp;
            srcTemp.isEquiped = true;
        }
        father.needSort = true;
        //father.OnWeaponChange(WeaponChange.EquiptList, srcTemp.Slot, srcTemp.uId);
    }
    protected void _unInstall(UInt64 weaponUid)
    {
        Weapon tmpWeapon = PlayerData.Instance._SoldierEquip.FindByUid(weaponUid);
        if (tmpWeapon != null)
            tmpWeapon.isEquiped = false;
        int result = _EquiptList.FindIndex((Weapon p) =>
        {
            if (p == null) return false;
            return p.uId == weaponUid;
        });
        if (result == -1) return;

        _EquiptList[result].isEquiped = false;
        _EquiptList[result] = null;
        father.needSort = true;
        //father.OnWeaponChange(WeaponChange.EquiptList);
    }
}
public class ArtifactedDepot : EquipedDepotBase
{
    public enum SlotMax { MaxEquip = 8, MaxWeapon = 3 }
    public override void BindFather(WeaponsDepot fa)
    {
        this.father = fa;
        this.father.AddChild(this);
    }
    //装备装备
    public override bool installEquipt(int sourceSlot, int destIndex, string message)
    {
        switch (_TextEquipted(sourceSlot, destIndex))
        {
            case WeaponEquip.Ok: UInt64 uid = father.FindBySlot(sourceSlot).uId; EquipModule.Instance.SendPutonEquipReq(uid, 0, destIndex + 1, 0); return true;
            case WeaponEquip.none: break;
            case WeaponEquip.same: break;
            case WeaponEquip.Type: break;
        }
        return false;
    }
    //卸载装备
    public override bool uninstallEquipt(UInt64 weaponUid)
    {
        EquipModule.Instance.SendGetoffEquipReq(weaponUid, 0);
        return true;
    }
    public ArtifactedDepot()
    {
        _EquiptList = new List<Weapon>((int)SlotMax.MaxEquip);
        for (int i = 0; i < (int)SlotMax.MaxEquip; ++i)
        {
            _EquiptList.Add(null);
        }
    }
    ~ArtifactedDepot()
    {
        father.RemoveChild(this);
    }
    public override void ReceivePutonEquipResp(Weapon source, int destIndex, int type, UInt64 other_soldierUid, fogs.proto.msg.Equip other_euqip)
    {
        //换装服务器反馈
        _Equipt(source, destIndex, type, other_soldierUid, other_euqip);
    }
    public override void ReceiveGetoffEquipResp(UInt64 uId)
    {
        _unInstall(uId);
    }
    protected override WeaponEquip _TextEquipted(int sourceSlot, int destIndex)
    {
        if (sourceSlot >= father._weaponList.Count || destIndex >= _EquiptList.Count) return WeaponEquip.none;

        var srcTemp = father._weaponList[sourceSlot];

        if (srcTemp == null) return WeaponEquip.none;

        if (srcTemp.isEquiped) return WeaponEquip.Type;

        if (srcTemp.Att.type != EquptedName.DIR_EQUPTED_NAME[destIndex]) return WeaponEquip.Type;

        if (srcTemp == _EquiptList[destIndex]) return WeaponEquip.same;


        return WeaponEquip.Ok;
    }
    public bool IsHadEquipCanEquip()
    {
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
        {
            return false;
        }
        if (this._EquiptList == null || this.father == null)
            return false;
        if (this.father._weaponList == null)
            return false;

        for (int i = 0; i < this._EquiptList.Count;++i )
        {
            if (this.IsHadEquipCanEquip(i))
                return true;
        }
        return false;
    }
    public bool IsHadEquipCanEquip(int index)
    {
        if (!CommonFunction.CheckIsOpen(OpenFunctionType.ArtifactIntensify))
        {
            return false;
        }
        if (this._EquiptList == null || this.father == null)
            return false;
        if (this.father._weaponList == null)
            return false;

        if (this._EquiptList[index] != null)
            return false;

        List<Weapon> tmpFather = this.father._weaponList;
        for (int j = 0; j < tmpFather.Count; ++j)
        {
            if(tmpFather[j].Att.levelLimit > PlayerData.Instance._Level)
                continue;

            if (this._TextEquipted(tmpFather[j].Slot, index) == WeaponEquip.Ok)
                return true;
        }
        return false;
    }
}
public class EquippedDepot : EquipedDepotBase
{
    public Soldier soldier;
    public enum SlotMax { MaxEquip = 6 }
    public override void BindFather(WeaponsDepot fa)
    {
        this.father = fa;
        this.father.AddChild(this);
    }
    //装备装备
    public override bool installEquipt(int sourceSlot, int destIndex, string message)
    {
        if (father == null)
            return false;

        switch (_TextEquipted(sourceSlot, destIndex))
        {
            case WeaponEquip.Ok:
                Weapon wp = father.FindBySlot(sourceSlot);
                if (wp == null) return false;
                UInt64 other_soldierUid = 0;
                if (wp.isEquiped)
                {
                    other_soldierUid = father.SoliderUidFind(wp);
                }
                UInt64 uid = wp.uId;

                EquipModule.Instance.SendPutonEquipReq(uid, soldier.uId, destIndex + 1, other_soldierUid);
                return true;
            case WeaponEquip.none: break;
            case WeaponEquip.same: break;
            case WeaponEquip.Type: break;
        }
        return false;
    }
    //卸载装备
    public override bool uninstallEquipt(UInt64 weaponUid)
    {
        EquipModule.Instance.SendGetoffEquipReq(weaponUid, soldier.uId);
        return true;
    }
    public EquippedDepot()
    {
        _EquiptList = new List<Weapon>((int)SlotMax.MaxEquip);
        for (int i = 0; i < (int)SlotMax.MaxEquip; ++i)
        {
            _EquiptList.Add(null);
        }
    }
    ~EquippedDepot()
    {
        if (father != null)
            father.RemoveChild(this);
    }
    public override void ReceivePutonEquipResp(Weapon source, int destIndex, int type, UInt64 other_soldierUid, fogs.proto.msg.Equip other_euqip)
    {
        //换装服务器反馈
        _Equipt(source, destIndex, type, other_soldierUid, other_euqip);
    }
    public override void ReceiveGetoffEquipResp(UInt64 uId)
    {
        _unInstall(uId);
    }
    public void ReceiveOneKeyPutOnAllResp(OneKeyPutOnAllResp tData)
    {
        foreach (fogs.proto.msg.Equip temp in tData.equip_list)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
            if (wp == null) continue;
            if (temp.postion > (int)SlotMax.MaxEquip) continue;
            _Equipt(wp, temp.postion - 1, 0, 0, null);
            wp.isEquiped = true;
            UISystem.Instance.SoldierAttView.PlayFastStrengthEffect(temp.postion);

        }
    }
    public void ReceiveOneKeyPromoteAllResp(OneKeyPromoteAllResp tData)
    {
        if (father == null)
            return;

        foreach (fogs.proto.msg.Equip temp in tData.equip_list)
        {
            Weapon wp = PlayerData.Instance._SoldierEquip.FindByUid(temp.uid);
            if (wp == null) continue;
            if (temp.postion > (int)SlotMax.MaxEquip) continue;
            wp.Strong(temp.level - wp.Level);
            //if (tData.equip_list != null && tData.equip_list.Count > 0)
            UISystem.Instance.SoldierAttView.PlayFastIntensifyEffect(temp.postion);
        }
        if (tData.equip_list.Count > 0)
        {
            PlayerData.Instance._WeaponDepot.needSort = true;
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.ONEKEY_STRONG_SUCCESS);
            //father.OnWeaponChange(WeaponChange.EquiptList);
            //
        }
        father.needRemoveNull = true;
        father.needSort = true;
    }
    public List<PutEuipList> FastSelect()
    {
        int[] mark = { 0, 0, 0, 0, 0, 0 };
        List<PutEuipList> temp = new List<PutEuipList>((int)SlotMax.MaxEquip);
        List<Weapon> tmpList = new List<Weapon>(father._weaponList);
        tmpList.Sort((left, right) =>
        {
            if (left.Att.quality != right.Att.quality)
            {
                if (left.Att.quality > right.Att.quality)
                {
                    return -1;
                }
                else return 1;
            }
            if (left.Level != right.Level)
            {
                if (left.Level > right.Level)
                {
                    return -1;
                }
                else return 1;
            }
            if (left.Att.star != right.Att.star)
            {
                if (left.Att.star > right.Att.star)
                {
                    return -1;
                }
                else return 1;
            }
            return 0;
        });
        foreach (Weapon wp in tmpList)
        {
            if (wp.isEquiped) continue;

            for (int i = 0; i < (int)SlotMax.MaxEquip; ++i)
            {
                if (_EquiptList[i] != null)
                {
                    mark[i] = 1;
                    continue;
                }
                if (mark[i] == 1)
                    continue;
                if (!soldier.Att.PosList.ContainsKey(i + 1))
                    continue;
                if (this._TextEquipted(wp.Slot, i) == WeaponEquip.Ok)
                {
                    PutEuipList pl = new PutEuipList();
                    pl.uid = wp.uId;
                    pl.postion = i + 1;
                    temp.Add(pl);
                    mark[i] = 1;
                    break;
                }
            }
        }
        if (temp.Count <= 0)
        {
            UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.HAD_EQUIP_ALL);
            return null;
        }
        EquipModule.Instance.SendOneKeyPutOnAllReq(this.soldier.uId, temp);
        return temp;
    }
    protected override WeaponEquip _TextEquipted(int sourceSlot, int destIndex)
    {
        if (father == null)
            return WeaponEquip.none;

        if (sourceSlot >= father._weaponList.Count || destIndex >= _EquiptList.Count) return WeaponEquip.none;

        Weapon srcTemp = father._weaponList[sourceSlot];

        if (soldier != null)
        {
            if (!soldier.Att.PosList.ContainsKey(destIndex + 1)) return WeaponEquip.Type;
            if (srcTemp.Att.type != soldier.Att.PosList[destIndex + 1]) return WeaponEquip.Type;
            if (soldier.Level < srcTemp.Att.levelLimit) return WeaponEquip.Type;
        }


        if (srcTemp == _EquiptList[destIndex]) return WeaponEquip.same;

        return WeaponEquip.Ok;
    }
}