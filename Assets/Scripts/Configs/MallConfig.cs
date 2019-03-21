using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public enum EMallPageType
{
    SkillBook = 1,
    WeaponWhite = 21,
    WeaponGreen = 22,
    WeaponBlue = 23,
    RingWhite = 31,
    RingGreen = 32,
    RingBlue = 33,
    Necklace = 4,
    HorseMat = 5,
    NecklaceWhite = 41,
    NecklaceGreen = 42,
    NecklaceBlue = 43,
    CommonMat = 6,
    EquipBag = 7,
}

public class MallGoodsData
{
    public uint mID;
    public EMallPageType mPageType;
    public uint mPos;
    public uint mCommodityType;
    public uint mGoodsID;
    public ECurrencyType mCurrencyType;
    public uint mPrice;
}

public class MallConfig : BaseConfig
{
    public List<MallGoodsData> _mallGoods;
    public Dictionary<EMallPageType, List<MallGoodsData>> _mallGoodsDic;

    public MallConfig()
    {
        _mallGoods = new List<MallGoodsData>();
        _mallGoodsDic = new Dictionary<EMallPageType, List<MallGoodsData>>();
        Initialize(GlobalConst.Config.DIR_XML_MALL, ParseConfig);
    }

    public List<MallGoodsData> GetMallGoodsByPageType(EMallPageType type)
    {
        if (_mallGoodsDic.ContainsKey(type))
            return _mallGoodsDic[type];
        Debug.LogError("Get Mall Goods Error! " + type.ToString());
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
                MallGoodsData data = new MallGoodsData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "page_type")
                    {
                        if (xe.InnerText != "")
                            data.mPageType = (EMallPageType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "position")
                    {
                        if (xe.InnerText != "")
                            data.mPos = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "commodity_type")
                    {
                        if (xe.InnerText != "")
                            data.mCommodityType = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "item_id")
                    {
                        data.mGoodsID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "money_type")
                    {
                        data.mCurrencyType = (ECurrencyType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "money_num")
                    {
                        data.mPrice = uint.Parse(xe.InnerText);
                    }
                }
                _mallGoods.Add(data);
                if(!_mallGoodsDic.ContainsKey(data.mPageType))
                {
                    List<MallGoodsData> list = new List<MallGoodsData>();
                    _mallGoodsDic.Add(data.mPageType, list);
                }
                _mallGoodsDic[data.mPageType].Add(data);
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
        if (_mallGoods != null)
        {
            _mallGoods.Clear();
            _mallGoods = null;
        }
        if (_mallGoodsDic != null)
        {
            _mallGoodsDic.Clear();
            _mallGoodsDic = null;
        }
    }
}
