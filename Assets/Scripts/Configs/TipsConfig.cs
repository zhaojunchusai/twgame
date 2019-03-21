using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
//using fogs.proto.msg;

public class TipsData 
{
    public uint ID;
    public string Contents;
}

public class TipsConfig  
{
    private List<TipsData> _TipsDatas;

    public TipsConfig() 
    {
        Initialize();
    }

    public void Initialize() 
    {
        _TipsDatas = new List<TipsData>();
        ParseConfig();
    }

    private void ParseConfig()
    {
        try 
        {
            TextAsset textAsset = Resources.Load("Config/LoadingTips") as TextAsset;
            string xmlstr = textAsset.text;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                TipsData data = new TipsData();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "contents")
                    {
                        data.Contents = xe.InnerText;
                    }
                }
                _TipsDatas.Add(data);
            }
        }
        catch(Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    public string GetTipsStr() 
    {
        if (_TipsDatas == null || _TipsDatas.Count == 0)
            return "";
        int index = UnityEngine.Random.Range(0, _TipsDatas.Count);
        return _TipsDatas[index].Contents;
    }
}
