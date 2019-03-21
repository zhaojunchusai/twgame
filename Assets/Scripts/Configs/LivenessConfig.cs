using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Collections;

public class LivenessData
{
    public uint ID;
    public string Name;
    public string Icon;
    public string Desc;
    public uint TimesLimit;
    public uint LivenessPoint;
    public string OpenUI;
    public ETaskOpenView OpenUIType;
}


public class LivenessAward
{
    public uint ID;
    public uint LivenessPoint;
    public int Exp;
    public uint DropID;
}


public class LivenessConfig : BaseConfig 
{
    private Dictionary<uint, LivenessData> _livenessDatas;
    private Dictionary<uint, LivenessAward> _livenessAward;
    public uint MaxLivenessPoint = 0;
    public LivenessConfig()
    {
        _livenessDatas = new Dictionary<uint, LivenessData>();
        _livenessAward = new Dictionary<uint, LivenessAward>();
        Initialize(GlobalConst.Config.DIR_XML_LIVENESS_TASK, ParseConfig);
        Initialize(GlobalConst.Config.DIR_XML_LIVENESS_AWARD, ParseAwardConfig);
    }

    public List<LivenessAward> GetLivenessAwardList()
    {
        return new List<LivenessAward>(_livenessAward.Values);
    }

    public List<LivenessData> GetLivenessDataList()
    {
        return new List<LivenessData>(_livenessDatas.Values);
    }

    public LivenessData GetLivenessDataByID(uint id)
    {
        if (_livenessDatas.ContainsKey(id))
            return _livenessDatas[id];

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
                LivenessData data = new LivenessData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "name")
                    {
                        data.Name = xe.InnerText;
                    }
                    if (xe.Name == "icon")
                    {
                        data.Icon = xe.InnerText;
                    }
                    if (xe.Name == "desc")
                    {
                        data.Desc = xe.InnerText;
                    }
                    if (xe.Name == "up_limit")
                    {
                        data.TimesLimit = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "liveness")
                    {
                        data.LivenessPoint = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "open_ui")
                    {
                        data.OpenUI = xe.InnerText;
                        string[] view = xe.InnerText.Split(':');
                        data.OpenUIType = (ETaskOpenView)int.Parse(view[0]);
                    }                    
                }                
                _livenessDatas.Add(data.ID, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void ParseAwardConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                LivenessAward data = new LivenessAward();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "liveness_num")
                    {
                        data.LivenessPoint = uint.Parse(xe.InnerText);
                        MaxLivenessPoint = Math.Max(data.LivenessPoint, MaxLivenessPoint);
                    }
                    if (xe.Name == "exp")
                    {
                        data.Exp = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "drop_id")
                    {
                        data.DropID = uint.Parse(xe.InnerText);
                    }
                }
                _livenessAward.Add(data.ID, data);
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
        if (_livenessDatas != null)
        {
            _livenessDatas.Clear();
            _livenessDatas = null;
        }

        if (_livenessAward != null)
        {
            _livenessAward.Clear();
            _livenessAward = null;
        }
    }
}
