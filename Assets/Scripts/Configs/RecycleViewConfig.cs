using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using System;
using System.Xml;


public class RecycleViewConfig : BaseConfig
{
    private Dictionary<uint, ECurrencyType> _currencyType = null;
    private Dictionary<uint, RankviewData> _datas = null;
    public List<RankviewData> DataList
    {
        get
        {
            return new List<RankviewData>(_datas.Values);
        }
    }

    public RecycleViewConfig()
    {
        _datas = new Dictionary<uint,RankviewData>();
        _currencyType = new Dictionary<uint, ECurrencyType>();
        Initialize(GlobalConst.Config.DIR_XML_RECYCLEVIEW, ParseConfig);
    }

    public uint GetRankIDByType(ERecycleContentType type)
    {
        List<RankviewData> list = DataList;
        foreach (RankviewData item in list)
        {
            if (item.Type == (uint)type && item.ParentID == 0)
            {
                return item.ID;
            }
        }
        //找不到则回复默认的1 = 等级榜
        return DataList[0].ID;
    }

    public RankviewData GetRankviewDataByID(uint id)
    {
        if (_datas.ContainsKey(id))
        {
            return _datas[id];
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
                RankviewData data = new RankviewData();
                data.ID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.Name = CommonFunction.GetXmlElementStr(Element, "name");
                data.Type = uint.Parse(CommonFunction.GetXmlElementStr(Element, "type"));
                data.Level = uint.Parse(CommonFunction.GetXmlElementStr(Element, "level"));
                data.ChildIDs = GetChildIds(CommonFunction.GetXmlElementStr(Element, "child"));
                data.ParentID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "parent"));
                _datas.Add(data.ID, data);
                if (!_currencyType.ContainsKey(data.Type))
                {
                    _currencyType.Add(data.Type, (ECurrencyType)(uint.Parse(CommonFunction.GetXmlElementStr(Element, "money"))));
                }
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public ECurrencyType GetCurrencyType(uint type)
    {
        if (_currencyType.ContainsKey(type))
            return _currencyType[type];

        return ECurrencyType.RecycleCoin;
    }

    private List<uint> GetChildIds(string str)
    {
        if (CommonFunction.XmlStringIsNull(str))
            return null;

        return CommonFunction.GetParseStrToUint(str, ',');
    }
}
