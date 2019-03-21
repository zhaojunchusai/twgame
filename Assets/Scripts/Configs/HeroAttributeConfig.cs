
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class HeroAttributeInfo
{
    /// <summary>
    /// 英雄等级
    /// </summary>
    public uint Level;
    /// <summary>
    /// 升级经验
    /// </summary>
    public int EXP;
    /// <summary>
    /// 体力上限
    /// </summary>
    public int MaxPhysical;
    /// <summary>
    /// 体力赠送
    /// </summary>
    public int GiftPhysical;
    /// <summary>
    /// 职业ID
    /// </summary>
    public int JobID;
    /// <summary>
    /// 生命
    /// </summary>
    public int HP;
    /// <summary>
    /// 生命恢复
    /// </summary>
    public int HPRecovery;
    /// <summary>
    /// 能量
    /// </summary>
    public int Energy;
    /// <summary>
    /// 能量恢复
    /// </summary>
    public int EnergyRecovery;
    /// <summary>
    /// 魔法
    /// </summary>
    public int MP;
    /// <summary>
    /// 魔法恢复
    /// </summary>
    public int MPRecovery;
    /// <summary>
    /// 统御力
    /// </summary>
    public int Leadership;
    /// <summary>
    /// 攻击
    /// </summary>
    public int Attack;
    /// <summary>
    /// 命中
    /// </summary>
    public int Accuracy;
    /// <summary>
    /// 闪避
    /// </summary>
    public int Dodge;
    /// <summary>
    /// 暴击
    /// </summary>
    public int Crit;
    /// <summary>
    /// 韧性
    /// </summary>
    public int Tenacity;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public int AttDistance;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public int AttRate;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int MoveSpeed;
    /// <summary>
    /// 被动技能ID[技能ID, 技能等级]
    /// </summary>
    public Dictionary<uint, byte> PassiveSkill;
    /// <summary>
    /// 标准移动速度[像素]
    /// </summary>
    public int NormalSpeed;
    /// <summary>
    /// 标准攻击延迟[毫秒]
    /// </summary>
    public int NormalInterval;
}

public class HeroAttributeConfig : BaseConfig
{
    private List<HeroAttributeInfo> _heroAttributeList;

    public HeroAttributeConfig()
    {
        _heroAttributeList = new List<HeroAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_HEROATTRIBUTE, ParseConfig);
    }
    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }
    public HeroAttributeInfo FindByLevel(int level)
    {
        return this._heroAttributeList.Find((HeroAttributeInfo info) =>
        {
            if (info == null)
                return false;
            return info.Level == level;
        });
    }
    public void Uninitialize()
    {
        if (_heroAttributeList != null)
        {
            _heroAttributeList.Clear();
            _heroAttributeList = null;
        }
    }

    /// <summary>
    /// 获取英雄等级数据列表
    /// </summary>
    /// <returns></returns>
    public List<HeroAttributeInfo> GetHeroAttributeList()
    {
        return _heroAttributeList;
    }

    /// <summary>
    /// 通过等级查找英雄数据
    /// </summary>
    /// <param name="vLV"></param>
    /// <returns></returns>
    public HeroAttributeInfo GetHeroAttributeByLV(uint vLV)
    {
        HeroAttributeInfo info = null;
        if (_heroAttributeList == null)
            return null;
        for (int i = 0; i < _heroAttributeList.Count; i++)
        {
            HeroAttributeInfo tmp = _heroAttributeList[i];
            if (tmp.Level == vLV)
            { 
                info = tmp;
                break;
            }
              
        }
        if (info == null)
        {
            Debug.LogError("can not get HeroAttributeInfo by Level:" + vLV.ToString());
        }
        return info;
    }

    /// <summary>
    /// 通过数据表索引查找英雄数据
    /// </summary>
    /// <param name="vIndex"></param>
    /// <returns></returns>
    public HeroAttributeInfo GetHeroAttributeByIndex(int vIndex)
    {
        if (_heroAttributeList == null)
            return null;
        if ((vIndex < 0) || (vIndex >= _heroAttributeList.Count))
            return null;
        return _heroAttributeList[vIndex];
    }

    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                HeroAttributeInfo data = new HeroAttributeInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "level")
                    {
                        data.Level = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "up_exp")
                    {
                        data.EXP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "max_ph_power")
                    {
                        data.MaxPhysical = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "per_ph_power")
                    {
                        data.GiftPhysical = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "job_id")
                    {
                        data.JobID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "hp_max")
                    {
                        data.HP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "hp_revert")
                    {
                        data.HPRecovery = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "energy_max")
                    {
                        data.Energy = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "energy_revert")
                    {
                        data.EnergyRecovery = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "mp_max")
                    {
                        data.MP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "mp_revert")
                    {
                        data.MPRecovery = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "leader")
                    {
                        data.Leadership = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "phy_atk")
                    {
                        data.Attack = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "acc_rate")
                    {
                        data.Accuracy = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ddg_rate")
                    {
                        data.Dodge = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "crt_rate")
                    {
                        data.Crit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "tnc_rate")
                    {
                        data.Tenacity = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "atk_space")
                    {
                        data.AttDistance = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "atk_interval")
                    {
                        data.AttRate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "speed")
                    {
                        data.MoveSpeed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "skill_list")
                    {
                        if (data.PassiveSkill == null)
                            data.PassiveSkill = new Dictionary<uint, byte>();
                        data.PassiveSkill.Clear();
                        string[] tmpArray = xe.InnerText.Split(';');
                        if ((tmpArray != null) && (tmpArray.Length > 0))
                        {
                            for (int i = 0; i < tmpArray.Length; i++)
                            {
                                string[] tmpArrayInfo = tmpArray[i].Split(':');
                                if ((tmpArrayInfo == null) || (tmpArrayInfo.Length <= 1))
                                    continue;
                                data.PassiveSkill.Add(uint.Parse(tmpArrayInfo[0]), byte.Parse(tmpArrayInfo[1]));
                            }
                        }
                    }
                    else if (xe.Name == "normalSpeed")
                    {
                        data.NormalSpeed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "normalInterval")
                    {
                        data.NormalInterval = int.Parse(xe.InnerText);
                    }
                }

                _heroAttributeList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load HeroAttributeInfo XML Error:" + ex.Message);
        }
    }
}