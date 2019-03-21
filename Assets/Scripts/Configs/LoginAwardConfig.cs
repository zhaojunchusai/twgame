using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Collections;

public class LoginAwardData
{
    public LoginAwardConfig.ELoginAwardType Type;
    public uint DayCount;
    public LoginAwardConfig.EAwardType AwardType;
    public uint AwardItemID;
    public uint AwardNum;
}

public class LoginAwardConfig : BaseConfig
{
    public enum EAwardType
    {
        Soldier = 1,
        Equip = 2,
        Prop = 3,
        SpecialItem = 5,
    }

    public enum ELoginAwardType
    {
        Continuous = 0,
        Total = 1,
    }

    public List<LoginAwardData> _continuousDatas;
    public List<LoginAwardData> _totalDatas;

    public LoginAwardConfig()
    {
        _continuousDatas = new List<LoginAwardData>();
        _totalDatas = new List<LoginAwardData>();
        Initialize(GlobalConst.Config.DIR_XML_LOGINAWARD_CON, ParseConfigC);
        Initialize(GlobalConst.Config.DIR_XML_LOGINAWARD_TOTAL, ParseConfigT);
    }
    
    public List<LoginAwardData> GetContinousLoginAwardList()
    {
        return _continuousDatas;
    }

    public List<LoginAwardData> GetTotalLoginAwardList()
    {
        return _totalDatas;
    }
    private void ParseConfigC(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                LoginAwardData data = new LoginAwardData();
                foreach (XmlElement xe in Element)
                {
                    //if (xe.Name == "type")
                    //{
                    //    data.Type = (ELoginAwardType)uint.Parse(xe.InnerText);
                    //}
                    data.Type = ELoginAwardType.Continuous;
                    if (xe.Name == "day_count")
                    {
                        if (xe.InnerText != "")
                            data.DayCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "award_type")
                    {
                        if (xe.InnerText != "")
                            data.AwardType = (EAwardType)int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "id")
                    {
                        if (xe.InnerText != "")
                            data.AwardItemID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "award_num")
                    {
                        if (xe.InnerText != "")
                            data.AwardNum = uint.Parse(xe.InnerText);
                    }
                }
                _continuousDatas.Add(data);
            }
            LoadCompleted();
            //AllotData();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    private void ParseConfigT(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                LoginAwardData data = new LoginAwardData();
                foreach (XmlElement xe in Element)
                {
                    //if (xe.Name == "type")
                    //{
                    //    data.Type = (ELoginAwardType)uint.Parse(xe.InnerText);
                    //}
                    data.Type = ELoginAwardType.Total;
                    if (xe.Name == "day_count")
                    {
                        if (string.IsNullOrEmpty(xe.InnerText))
                            continue;
                        data.DayCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "award_type")
                    {
                        if (xe.InnerText != "")
                            data.AwardType = (EAwardType)uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "id")
                    {
                        data.AwardItemID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "award_num")
                    {
                        data.AwardNum = uint.Parse(xe.InnerText);
                    }
                }
                _totalDatas.Add(data);
            }
            LoadCompleted();
            //AllotData();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }


    public void Uninitialize()
    {
        if (_continuousDatas != null)
        {
            _continuousDatas.Clear();
            _continuousDatas = null;
        }

        if (_totalDatas != null)
        {
            _totalDatas.Clear();
            _totalDatas = null;
        }
    }

}
