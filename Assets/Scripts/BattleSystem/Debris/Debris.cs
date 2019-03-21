using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public enum DebrisCheck
{
    /// <summary>
    /// 未知原因
    /// </summary>
    none,
    /// <summary>
    /// 金钱原因
    /// </summary>
    Money,
    /// <summary>
    /// 材料原因
    /// </summary>
    Goods,
    /// <summary>
    /// 武将超出上限
    /// </summary>
    SoldierFull,
    /// <summary>
    /// 成功
    /// </summary>
    Ok
}
public enum DebrisChange
{
    /// <summary>
    /// 装备碎片变化
    /// </summary>
    Equip,
    /// <summary>
    /// 武将碎片变化
    /// </summary>
    Soldier
}
public class Debris
{
    private ItemInfo att;
    public Texture Icon = null;
    public int Slot = 0;
    public int count = 0;
    public ItemInfo Att { get { return att; } }//获取装备的信息
    public static Debris createByID(uint Id)
    {
        Debris tempDebris = new Debris();
        tempDebris.att = ConfigManager.Instance.mItemData.GetItemInfoByID(Id);

        if (tempDebris.att == null) return null;
        

        return tempDebris;
    }
    public void LoadImage(System.Action<Texture> onload)
    {
        if (this.Icon != null)
        {
            onload(this.Icon);
            return;
        }
        else
        {
            ResourceLoadManager.Instance.LoadAloneImage(this.att.icon, (texture) =>
                {
                    this.Icon = texture;
                    onload(this.Icon);
                });
        }
       
    }
    public bool Serialize()//读档
    {
        return true;
    }
    public Debris()
    {
    }
    public DebrisCheck enabelCompound()
    {
        if(this.count < att.compound_count) return DebrisCheck.Goods;
        if(PlayerData.Instance._SoldierDepot.IsFull(1)) return DebrisCheck.SoldierFull;
        return DebrisCheck.Ok;
    }
}
public class SoldierDebrisDepot
{
    public enum SlotMax { MaxDebris = 100 }
    public List<Debris> _SoldierDebrisList;
    private bool needSort = false;
    private bool needRemoveNull = false;
    /// <summary>
    /// 碎片合成发生了变化
    /// </summary>
    /// <param name="change"></param>
    /// <param name="Slot"></param>
    /// <param name="uID"></param>
    public delegate void SoldierDebrisDelet(DebrisChange change, int Slot = -1, UInt64 uID = 0);
    public event SoldierDebrisDelet SoldierDebrisEvent;

    public void Update()
    {
        _RemoveNull();
        _sort();
    }
    public void Clear()
    {
        this._SoldierDebrisList.Clear();
        this.needRemoveNull = false;
        this.needSort = false;
    }
    //通过slot找技能
    public Debris FindBySlot(int slot)
    {
        return _SoldierDebrisList.Find((Debris p) => { if (p == null)return false; return p.Slot == slot; });
    }
    public Debris FindByid(uint Id)
    {
        return _SoldierDebrisList.Find((Debris p) => { if (p == null)return false; return p.Att.id == Id; });
    }
    /// <summary>
    /// 添加count个ID的碎片
    /// </summary>
    /// <param name="Id"></param>
    /// <param name="count"></param>
    /// <returns></returns>
    public bool oneAdd(uint Id,int count)
    {
        if(count >=0)
            return _Add(Id,count);
        else
            return _Delete(Id,-count);
    }
    public bool RefreshNum(uint Id, int count)
    {
        Debris fd = this.FindByid(Id);
        if (fd == null)
            return false;

        if (count >= 0)
            fd.count = count;
        else
            this._Delete(Id,fd.count);
        return true;
    }
    public bool oneAdd(fogs.proto.msg.ItemInfo item)
    {
        return this.oneAdd(item.id,item.change_num);
    }
    public bool compound(uint Id)
    {
        Debris fd = this.FindByid(Id);
        if (fd == null) return false;
        switch (fd.enabelCompound())
        {
            case DebrisCheck.Goods: UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.NOT_ENOUGH_COUNT); return false;
            case DebrisCheck.SoldierFull: UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, string.Format(ConstString.SOLDIER_FULL, SoldierDepot.MAXCOUNT)); return false;
        }

        BackPackModule.Instance.SendChipCompositeReq((uint)Id);
        return true;
    }
    public void ReceiveChipCompositeResp(ChipCompositeResp data)
    {
        this._Compounds(data.id,data);
    }
    public SoldierDebrisDepot()
    {
        _SoldierDebrisList = new List<Debris>();
        Scheduler.Instance.AddUpdator(Update);
    }
    ~SoldierDebrisDepot()
    {
        Scheduler.Instance.RemoveUpdator(Update);
    }
    public bool Serialize(List<fogs.proto.msg.Item> data)
    {
        this._SoldierDebrisList.Clear();
        foreach(Item it in data)
        {
            Debris de = Debris.createByID(it.id);
            if (de == null) continue;
            if (de.Att.type != 4) continue;
            de.count = it.num;

            this._SoldierDebrisList.Add(de);
            de.Slot = this._SoldierDebrisList.Count - 1;
            this.needSort = true;
        }
        return true;
    }
    private void _Compounds(uint Id, ChipCompositeResp data)
    {
        Debris fd = this.FindByid(Id);

        if (fd == null) return;
        PlayerData.Instance.UpdateItem(data.update_item);
        Soldier sd = PlayerData.Instance._SoldierDepot.oneAdd(data.soldier);
        UISystem.Instance.HintView.ShowMessageBox(MessageBoxType.mb_Hint, ConstString.DEBRIS_COM_OVER);
    }
    private bool _Add(uint Id,int count)
    {
        Debris fd = this.FindByid(Id);
        if(fd == null)
        {
            Debris db = Debris.createByID(Id);
            if (db == null) return false;
            if (db.Att.type != 4)
                return false;
            _SoldierDebrisList.Add(db);
            db.Slot = _SoldierDebrisList.Count - 1;
            db.count = count;
        }
        else
        {
            fd.count += count;
        }
        needSort = true;

        return true;
    }
    private bool _Delete(uint Id,int count)
    {
        Debris fd = this.FindByid(Id);

        if(fd == null) return false;

        if (fd.count <= count)
        {
            fd.count = 0;
            _SoldierDebrisList[fd.Slot] = null;
            fd = null;
            needRemoveNull = true;
        }
        else
        {
            fd.count -= count;
        }
        needSort = true;

        return true;
    }
    /// <summary>
    /// 1、按照碎片是否可以合成排序，可以合成的碎片排序靠前；
    /// 2、按照碎片品质排序，碎片品质越高的排序越靠前；
    /// 3、按照碎片ID排序，ID越小的排序越靠前；
    /// 4、武将ID相同的，无排序需求
    /// </summary>
    private void _sort()
    {
        if (!needSort) return;
        needSort = false;
        this._SoldierDebrisList.Sort(
            (left, right) => 
            {
                if(left.enabelCompound() == DebrisCheck.Ok && right.enabelCompound() != DebrisCheck.Ok)
                {
                    return -1;
                }
                if(left.enabelCompound() != DebrisCheck.Ok && right.enabelCompound() == DebrisCheck.Ok)
                {
                    return 1;
                }
                if(left.Att.quality != right.Att.quality)
                {
                    if (left.Att.quality > right.Att.quality)
                        return -1;
                    else
                        return 1;
                }
                if(left.Att.id != right.Att.id)
                {
                    if (left.Att.id < right.Att.id)
                        return -1;
                    else
                        return 1;
                }
                return 0;
            });
        for (int i = 0; i < this._SoldierDebrisList.Count; ++i)
        {
            this._SoldierDebrisList[i].Slot = i;
        }
        if (SoldierDebrisEvent != null)
        {
            SoldierDebrisEvent(DebrisChange.Soldier);
            SoldierDebrisEvent(DebrisChange.Equip);
        }
    }
    private void _RemoveNull()
    {
        if (!needRemoveNull) return;
        needRemoveNull = false;
        _SoldierDebrisList.RemoveAll((temp) => { return temp == null; });
    }
}
