using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Collections;

public class ExchangeGoldData
{
    public uint Level;
    public uint BaseGold;
}

public class ExchangeGoldConfig : BaseConfig {

    private Dictionary<uint, ExchangeGoldData> _exchangeGoldDatas;

    public ExchangeGoldConfig()
    {
        _exchangeGoldDatas = new Dictionary<uint, ExchangeGoldData>();
        Initialize(GlobalConst.Config.DIR_XML_EXCHANGE_GOLD, ParseConfig);
    }

    public ExchangeGoldData GetVipDataByLv(uint lv)
    {
        return !_exchangeGoldDatas.ContainsKey(lv) ? null : _exchangeGoldDatas[lv];
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
                ExchangeGoldData data = new ExchangeGoldData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "level")
                    {
                        data.Level = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "base_num")
                    {
                        data.BaseGold = uint.Parse(xe.InnerText);
                    }
                }
                _exchangeGoldDatas.Add(data.Level, data);
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
        if (_exchangeGoldDatas != null)
        {
            _exchangeGoldDatas.Clear();
            _exchangeGoldDatas = null;
        }
    }
}
