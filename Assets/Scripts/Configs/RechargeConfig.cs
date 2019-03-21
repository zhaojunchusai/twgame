using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class RechargeShowData
{
    public uint ID;
    public string Icon;
    public bool IsMonthCard;
    public uint Price;
    public uint GetDiamond;
    public uint FirstGift;
    public uint NormalGift;
}

public class RechargeConfig : BaseConfig {

    private Dictionary<uint, RechargeShowData> _rechargeShowDatas;
    public RechargeConfig()
    {
        _rechargeShowDatas = new Dictionary<uint, RechargeShowData>();
        Initialize(GlobalConst.Config.DIR_XML_RECHARGE, ParseConfig);
    }

    public RechargeShowData GetVipDataByLv(uint id)
    {
        return !_rechargeShowDatas.ContainsKey(id) ? null : _rechargeShowDatas[id];
    }

    public List<RechargeShowData> GetRechargeShowList()
    {
        return new List<RechargeShowData>(_rechargeShowDatas.Values);
    }

    public RechargeShowData GetRechargeDataByID(uint id)
    {
        if (_rechargeShowDatas.ContainsKey(id))
            return _rechargeShowDatas[id];

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
                RechargeShowData data = new RechargeShowData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "icon")
                    {
                        data.Icon = xe.InnerText;
                    }
                    if (xe.Name == "month_card")
                    {
                        data.IsMonthCard = uint.Parse(xe.InnerText) == 1;
                    }
                    if (xe.Name == "price")
                    {
                        data.Price = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "get_diamond")
                    {
                        data.GetDiamond = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "first_gift")
                    {
                        data.FirstGift = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "normal_gift")
                    {
                        data.NormalGift = uint.Parse(xe.InnerText);
                    }
                }
                _rechargeShowDatas.Add(data.ID, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public void Uninitialize()
    {
        if (_rechargeShowDatas != null)
        {
            _rechargeShowDatas.Clear();
            _rechargeShowDatas = null;
        }
    }
}
