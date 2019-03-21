using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class MainCityUnlockData
{
    public int mID;
    public int mCityPos;
    public Vector3 mBuildingPos;
    public uint mBuildingLayer;
}

public class MainCityUnlockConfig : BaseConfig
{
    private Dictionary<int, MainCityUnlockData> _cityUnlockDatas = new Dictionary<int, MainCityUnlockData>();

    public MainCityUnlockConfig()
    {
        _cityUnlockDatas = new Dictionary<int, MainCityUnlockData>();
        Initialize(GlobalConst.Config.DIR_XML_MAIN_CITY_UNLOCK, ParseConfig);
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
                MainCityUnlockData data = new MainCityUnlockData();
                foreach (XmlElement xe in Element)
                {
                    //Debug.Log(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                    if (xe.Name == "id")
                    {
                        data.mID = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "main_pos_x")
                    {
                        data.mCityPos = int.Parse(xe.InnerText);
                        //Debug.LogError(xe.InnerText);
                    }
                    if (xe.Name == "func_pos")
                    {
                        data.mBuildingPos = GetVector3(xe.InnerText);
                    }
                    if (xe.Name == "far_layer")
                    {
                        data.mBuildingLayer = uint.Parse(xe.InnerText);
                    }
                }
                if (!_cityUnlockDatas.ContainsKey(data.mID))
                {
                    _cityUnlockDatas.Add(data.mID, data);
                }
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public MainCityUnlockData GetDataByFuncID(int id)
    {
        if (_cityUnlockDatas.ContainsKey(id))
            return _cityUnlockDatas[id];

        Debug.LogError("MainCityUnlockData ERROR BY ID = " + id);
        return new MainCityUnlockData();
    }

    public bool NeedToShowUnlockEffect(int id)
    {
        if (_cityUnlockDatas.ContainsKey(id))
            return true;
        return false;
    }

    private Vector3 GetVector3(string str)
    {
        List<int> list = CommonFunction.GetParseStrToInt(str, ',');
        if (list != null && list.Count > 1)
        {
            return new Vector3(list[0], list[1], 0);
        }
        else
        {
            return Vector3.zero;
        }
    }

    public void Uninitialize()
    {
        if (_cityUnlockDatas != null)
        {
            _cityUnlockDatas.Clear();
            _cityUnlockDatas = null;
        }
    }
}
