using System;
using System.Xml;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class GameOnlinePackageData
{
    public uint mID;
    public float mSeconds;
    public uint mDropID;
    public uint mNextID;

    public override string ToString()
    {
        return "GameOnlinePackageData id = " + mID + "; seconds = " + mSeconds + "; dropID = " + mDropID + "; nextID = " +mNextID;
    }
}

public class GameOnlinePackageConfig : BaseConfig
{
    private List<GameOnlinePackageData> _list;

    public GameOnlinePackageConfig()
    {
        _list = new List<GameOnlinePackageData>();
        Initialize(GlobalConst.Config.DIR_XML_ONLINEPACKAGE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                GameOnlinePackageData data = new GameOnlinePackageData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "ID")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "condition")
                    {
                        data.mSeconds = float.Parse(xe.InnerText);
                    }
                    if (xe.Name == "reward")
                    {
                        data.mDropID = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "next")
                    {
                        data.mNextID = uint.Parse(xe.InnerText);
                    }
                }
                //Debug.Log(data.ToString());
                _list.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load GameOnlinePackageConfig  XML Error:" + ex.Message);
        }
    }

    public GameOnlinePackageData GetDataByID(uint id)
    {
        for (int i = 0; i < _list.Count; i++)
        {
            GameOnlinePackageData data = _list[i];
            if (data.mID == id)
                return data;
        }
        return null;
    }

    public void Uninitialize()
    {
        _list = null;
    }
}

