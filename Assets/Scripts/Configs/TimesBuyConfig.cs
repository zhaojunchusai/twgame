using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;

public class TimesBuyData
{
    public int EndlessTimes;
    public int ExoticAdvantureTimes;
    public int CampaignTimes;
}

public class TimesBuyConfig : BaseConfig {
    private TimesBuyData data;

    public TimesBuyConfig()
    {
        data = new TimesBuyData();
        Initialize(GlobalConst.Config.DIR_XML_TIMESBUY, ParseConfig);
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
                data.EndlessTimes = int.Parse(CommonFunction.GetXmlElementStr(Element, "endless_times_get"));
                data.ExoticAdvantureTimes = int.Parse(CommonFunction.GetXmlElementStr(Element, "exoticAdvanture_times_get"));
                data.CampaignTimes = int.Parse(CommonFunction.GetXmlElementStr(Element, "campaign_times_get"));
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public TimesBuyData GetTimesBuyData()
    {
        return data;
    }
}
