using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class StoreGoodsData
{
    public uint ID;
    public uint PropID;
    public GoodsConfigType PropType;
    public ECurrencyType CurrencyType;
    public uint UnitPrice;
}

public class StoreGoodsConfig : BaseConfig
{
    private Dictionary<uint, StoreGoodsData> _storeGoodsDatas;

    public StoreGoodsConfig()
    {
        _storeGoodsDatas = new Dictionary<uint, StoreGoodsData>();
        Initialize(GlobalConst.Config.DIR_XML_COMMODITY, ParseConfig);
    }

    public StoreGoodsData GetStoreGoodsDataByID(uint id)
    {
        if (_storeGoodsDatas.ContainsKey(id))
        {
            return _storeGoodsDatas[id];
        }
        Debug.LogError("can not get StoreGoodsData by ID: " + id.ToString());
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
                StoreGoodsData data = new StoreGoodsData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "item_id")
                    {
                        data.PropID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "config_type")
                    {
                        data.PropType = (GoodsConfigType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "money_type")
                    {
                        data.CurrencyType = (ECurrencyType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "money_value")
                    {
                        data.UnitPrice = uint.Parse(xe.InnerText);
                    }
                }
                _storeGoodsDatas.Add(data.ID, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message + ex.StackTrace);
        }
    }

    public void Uninitialize() 
    {
        if (_storeGoodsDatas != null) 
        {
            _storeGoodsDatas.Clear();
            _storeGoodsDatas = null;
        }
    }

}
