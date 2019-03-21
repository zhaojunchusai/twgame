using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class RecruitData 
{
    public int RecruitType;
    public int RecruitWayType;
    public int PropID;
    public int NeedPropCount;
    public ECurrencyType  CurrencyType;
    public int CurrencyCount;
    public int DropID;
    public int LimitVipLV;

    public RecruitData() 
    {
        RecruitType = 0;
        RecruitWayType = 0;
        PropID = 0;
        NeedPropCount = 0;
        CurrencyType = ECurrencyType.None;
        CurrencyCount = 0;
        DropID = 0;
        LimitVipLV = 0;
    }

    public void CopyTo(RecruitData data) 
    {
        RecruitType = data.RecruitType;
        RecruitWayType = data.RecruitWayType;
        PropID = data.PropID;
        NeedPropCount = data.NeedPropCount;
        CurrencyType = data.CurrencyType;
        CurrencyCount = data.CurrencyCount;
        DropID = data.DropID;
        LimitVipLV = data.LimitVipLV;
    }
}


public class RecruitConfig : BaseConfig 
{
    private Dictionary<int,Dictionary<int, RecruitData>>_recruitData = new Dictionary<int,Dictionary<int,RecruitData>>();

    public RecruitConfig() 
    {
        _recruitData.Clear();
        Initialize(GlobalConst.Config.DIR_XIM_RECRUIT, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
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
                RecruitData data = new RecruitData();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "recruit_type")
					{
						data.RecruitType = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "recruit_way")
					{
                        data.RecruitWayType = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "cost_propid")
					{
                        data.PropID = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "prop_count")
					{
                        data.NeedPropCount = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "money_type")
                    {
                        data.CurrencyType =  (ECurrencyType)int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "money_value")
                    {
                        data.CurrencyCount = int.Parse(xe.InnerText);
                    }
                    //else if (xe.Name == "drop_id")
                    //{
                    //    Debug.LogError(xe.InnerText);
                    //    data.DropID = int.Parse(xe.InnerText);
                    //}
                    else if (xe.Name == "vip_limit")
                    {
                        data.LimitVipLV = int.Parse(xe.InnerText);
                    }
				}
                if (_recruitData.ContainsKey(data.RecruitType))
                {
                    _recruitData[data.RecruitType].Add(data.RecruitWayType, data);
                }
                else 
                {
                    _recruitData.Add(data.RecruitType, new Dictionary<int, RecruitData>());
                    _recruitData[data.RecruitType].Add(data.RecruitWayType, data);
                }
			}

            base.LoadCompleted();
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public RecruitData GetRecruitData(int type,int wayType) 
    {
        RecruitData result = new RecruitData();
        if (_recruitData.ContainsKey(type))
        {
            if (_recruitData[type].ContainsKey(wayType))
            {
                result.CopyTo(_recruitData[type][wayType]);
            }
        }
        return result;
    }

    public void Uninitialize() 
    {
        _recruitData.Clear();
    }
}
