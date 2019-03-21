
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class CombatPowerInfo
{
    /// <summary>
    /// 站位[1-进程 2-中程 3-远程 10-英雄]
    /// </summary>
    public int Stance;
    /// <summary>
    /// 职业[0-英雄 1-攻击 2-防御 3-辅助 4-控制]
    /// </summary>
    public int Career;
    /// <summary>
    /// 1点攻击
    /// </summary>
    public float phy_atk;
    /// <summary>
    /// 1点生命
    /// </summary>
    public float hp_max;
    /// <summary>
    /// 1点暴击
    /// </summary>
    public float crt_rate;
    /// <summary>
    /// 1点韧性
    /// </summary>
    public float tnc_rate;
    /// <summary>
    /// 1点命中
    /// </summary>
    public float acc_rate;
    /// <summary>
    /// 1点闪避
    /// </summary>
    public float ddg_rate;
}

public class CombatPowerConfig : BaseConfig
{
    private List<CombatPowerInfo> _combatPowerList;

    public CombatPowerConfig()
    {
        _combatPowerList = new List<CombatPowerInfo>();
        Initialize(GlobalConst.Config.DIR_XML_COMBATPOWER, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_combatPowerList != null)
        {
            _combatPowerList.Clear();
            _combatPowerList = null;
        }
    }

    public List<CombatPowerInfo> GetCombatPowerList()
    {
        return _combatPowerList;
    }

    /// <summary>
    /// 通过站位获取数据
    /// </summary>
    /// <param name="vStance"></param>
    /// <returns></returns>
    public CombatPowerInfo FindByKey(int vStance)
    {
        if (_combatPowerList != null)
        {
            foreach (CombatPowerInfo tmpInfo in _combatPowerList)
            {
                if (tmpInfo == null)
                    continue;
                if (tmpInfo.Stance == vStance)
                    return tmpInfo;
            }
        }
        return null;
    }
    /// <summary>
    /// 通过站位和职业获取数据
    /// </summary>
    /// <param name="vStance"></param>
    /// <param name="vCareer"></param>
    /// <returns></returns>
    public CombatPowerInfo FindSingleInfo(int vStance, int vCareer)
    {
        if (_combatPowerList != null)
        {
            foreach (CombatPowerInfo tmpInfo in _combatPowerList)
            {
                if (tmpInfo == null)
                    continue;
                if ((tmpInfo.Stance == vStance) && (tmpInfo.Career == vCareer))
                    return tmpInfo;
            }
        }
        return null;
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
                CombatPowerInfo data = new CombatPowerInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "Stance")
                    {
                        data.Stance = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "Career")
                    {
                        data.Career = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "phy_atk")
                    {
                        data.phy_atk = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "hp_max")
                    {
                        data.hp_max = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "crt_rate")
                    {
                        data.crt_rate = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "tnc_rate")
                    {
                        data.tnc_rate = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "acc_rate")
                    {
                        data.acc_rate = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ddg_rate")
                    {
                        data.ddg_rate = float.Parse(xe.InnerText);
                    }
                }

                _combatPowerList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load combatPower XML Error:" + ex.Message);
        }
    }
}