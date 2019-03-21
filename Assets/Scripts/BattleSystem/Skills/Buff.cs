using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using Assets.Script.Common;
using fogs.proto.msg;
public class Buff
{
    public Effects effect;
    public RoleAttribute role;
    public BuffDepot father;
    public float during;
    public float frequency;
    public float frequencyTime = 0.0f;
    public void Initialize(float during, float frequency,Effects effect,RoleAttribute role)
    {
        this.during = during / 1000.0f;
        this.frequency = frequency / 1000.0f;
        this.effect = effect;
        this.role = role; 
    }
    public void Activate()
    {
        if (this.frequency <= 0)
        {
            this.DoEffect();
        }
        if (this.during > 0)
        {
            UpdateTimeTool.Instance.AddUpdator(this.Update);
        }
    }
    public void OnDelete()
    {
        UpdateTimeTool.Instance.RemoveUpdator(this.Update);
        
        this.RevocateEffect();
        if (this.father != null)
            father.BuffList.Remove(this);
    }
    public void DoEffect()
    {
        if(this.effect != null && this.role != null)
        {
            List<RoleAttribute> list = new List<RoleAttribute>(1);
            list.Add(this.role);
            this.effect.DoEffect(list);
        }
    }
    public void RevocateEffect()
    {
        if (this.effect != null)
        {
            List<RoleAttribute> list = new List<RoleAttribute>(1);
            list.Add(this.role);
            this.effect.RevocateEffect(list);
        }
    }
    public bool ReFreshTime(int time)
    {
        if (this.during <= 0)
        {
            this.OnDelete();
            return false;
        }

        this.during = time / 1000.0f;

        return true;
    }
    private void Update(float time)
    {
        this.frequencyTime += time;
        if(this.frequency != 0 && this.frequencyTime >= this.frequency)
        {
            this.DoEffect();
            this.frequencyTime = 0.0f;
        }
        this.during -= time;
        if (this.during <= 0.01f)
            this.OnDelete();
    }
}
public class BuffDepot
{
    public List<Buff> BuffList = new List<Buff>();
    public List<Filter> FilterList = new List<Filter>();
    public delegate bool Filter(Effects effect);
    public BuffDepot()
    {
    }
    public bool AddBuff(Buff buff)
    {
        if (buff == null)
            return false;
        if (BuffList == null)
            return false;
        Effects effect = buff.effect;
        //if (!this._filter(buff)) return false;
        Buff tempBuff = this.BuffList.Find((Buff bf) => { if (bf == null) return false; return bf.effect.info.effectId == effect.info.effectId && bf.effect.info.effectType == effect.info.effectType; });
        if (tempBuff != null)
        {
            float numTemp = tempBuff.effect.info.num + tempBuff.effect.info.percent * 1000;
            float numBuff = effect.info.num + effect.info.percent * 1000;

            if (numTemp == numBuff)
            {
                if (tempBuff.during <= effect.info.durTime)
                    return false;
            }
        }
        this.BuffList.Add(buff);
        buff.father = this;
        return true;
    }
    public void AddFilter(Filter filter)
    {
        if (this.FilterList.Contains(filter)) return;
        foreach(Buff bf in this.BuffList)
        {
            if(!filter(bf.effect))
            {
                bf.OnDelete();
                this.BuffList.Remove(bf);
            }
        }
        this.FilterList.Add(filter);
    }
    public void DeleteBuff(Filter filter)
    {
        List<Buff> DeleteList = new List<Buff>(this.BuffList.Count);
        if (this.BuffList == null)
            return;

        foreach(Buff temp in this.BuffList)
        {
            if(filter(temp.effect))
                DeleteList.Add(temp);
        }
        foreach (Buff temp in DeleteList)
        {
            temp.OnDelete();
            this.BuffList.Remove(temp);
        }
    }
    public void DeleteUnFilter(Filter filter)
    {
        this.FilterList.Remove(filter);
    }
    public bool TextCanAdd(Buff buff)
    {
        if (buff == null)
            return false;
        if (this.BuffList == null)
            return true;
        Effects effect = buff.effect;

        Buff tempBuff = this.BuffList.Find((Buff bf) => { if (bf == null) return false; return bf.effect.info.effectId == effect.info.effectId && bf.effect.info.effectType == effect.info.effectType; });
        if (tempBuff != null)
        {
            float numTemp = tempBuff.effect.info.num + tempBuff.effect.info.percent * 1000;
            float numBuff = effect.info.num + effect.info.percent * 1000;

            if (numTemp == numBuff)
            {
                if (tempBuff.during <= effect.info.durTime)
                    tempBuff.during = (float)effect.info.durTime / 1000.0f;
                return true;
            }
        }
        return this._filter(buff);
    }
    private bool _filter(Buff buff)
    {
        for (int i = 0; i < this.FilterList.Count;++i )
        {
            Filter fl = this.FilterList[i];
            if (!fl(buff.effect))
            {
                return false;
            }
        }
        return true;
    }
    public bool DepotFilter(Effects effect)
    {
        if (effect == null) return false;
        if (effect.info.effectId == 812) return true;

        Buff tempBuff = this.BuffList.Find((Buff bf) => { if (bf == null) return false; return bf.effect.info.effectId == effect.info.effectId && bf.effect.info.effectType == effect.info.effectType; });
        if (tempBuff != null)
        {
            float numTemp = tempBuff.effect.WorthPrice();
            float numBuff = effect.WorthPrice();
            if (numTemp > numBuff)
            {
                return false;
            }
            else
            {
                if (numTemp == numBuff)
                {
                    if (tempBuff.during <= effect.info.durTime)
                        tempBuff.during = (float)effect.info.durTime / 1000.0f;
                    return true;
                }
                tempBuff.OnDelete();
                this.BuffList.Remove(tempBuff);
                return true;
            }
        }
        return true;
    }
}
public class EffectImmune
{
    public List<Filter> FilterList;
    public delegate bool Filter(Effects effect);
    public EffectImmune()
    {
        this.FilterList = new List<Filter>();
    }
    public void AddFilter(Filter filter)
    {
        if (this.FilterList.Contains(filter)) return;
        
        this.FilterList.Add(filter);
    }
    public void DeleteUnFilter(Filter filter)
    {
        this.FilterList.Remove(filter);
    }
    public bool FilterEffect(Effects effect,bool isTalk = false)
    {
        if (effect == null)
            return false;
        if (this.FilterList == null)
            return true;
        for (int i = 0; i < this.FilterList.Count;++i )
        {
            Filter fl = this.FilterList[i];
            if (fl == null)
                continue;

            if (!fl(effect))
            {
                return false;
            }
        }
        return true;
    }
}