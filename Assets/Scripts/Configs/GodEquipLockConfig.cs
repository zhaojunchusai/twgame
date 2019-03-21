
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class GodEquipLockInfo
{
    /// <summary>
    /// ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 等级限制
    /// </summary>
    public int level;
    /// <summary>
    /// VIP限制
    /// </summary>
    public int vipLv;
    /// <summary>
    /// 关卡ID限制
    /// </summary>
    public int GateID;

    public int itemType;
}

public class GodEquipLockConfig : BaseConfig
{
    private List<GodEquipLockInfo> _godEquipLockList;

    public GodEquipLockConfig()
    {
        _godEquipLockList = new List<GodEquipLockInfo>();
        Initialize(GlobalConst.Config.DIR_XML_GOGEQUIPLOCK, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_godEquipLockList != null)
        {
            _godEquipLockList.Clear();
            _godEquipLockList = null;
        }
    }

    public List<GodEquipLockInfo> GetGodEquipLockList()
    {
        return _godEquipLockList;
    }

    /// <summary>
    /// 通过ID取得奖励数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public GodEquipLockInfo GetGodEquipLockInfoByID(uint id)
    {
        GodEquipLockInfo info = _godEquipLockList.Find((data) => { return data.id == id; });
        return info;
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
                GodEquipLockInfo data = new GodEquipLockInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "level")
                    {
                        data.level = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "vipLv")
                    {
                        data.vipLv = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "GateID")
                    {
                        data.GateID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ItemType")
                    {
                        data.itemType = int.Parse(xe.InnerText);
                    }
                }
                _godEquipLockList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load arenaAward XML Error:" + ex.Message);
        }
    }
}