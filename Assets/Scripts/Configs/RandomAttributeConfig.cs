
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class RandomAttributeInfo 
{
	/// <summary>
	/// 属性类别
	/// </summary>
	public int type;
	/// <summary>
	/// 下限系数
	/// </summary>
	public float min;
	/// <summary>
	/// 上限系数
	/// </summary>
    public float max;
}

public class RandomAttributeConfig : BaseConfig 
{
	private List<RandomAttributeInfo> _randomAttributeList;

	public RandomAttributeConfig()
	{
		_randomAttributeList = new List<RandomAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_RANDOMATTRIBUTE, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_randomAttributeList  != null)
		{
			_randomAttributeList.Clear();
			_randomAttributeList = null;
		}
	}

	public List<RandomAttributeInfo> GetRandomAttributeList()
	{
		return _randomAttributeList;
	}

    public RandomAttributeInfo FindById(int vID)
    {
        if (_randomAttributeList == null)
            return null;
        foreach (RandomAttributeInfo tmpInfo in _randomAttributeList)
        {
            if (vID == tmpInfo.type)
                return tmpInfo;
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
                RandomAttributeInfo data = new RandomAttributeInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "type")
                    {
                        data.type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "min")
                    {
                        if (!float.TryParse(xe.InnerText, out data.min))
                            data.min = 0;
                    }
                    else if (xe.Name == "max")
                    {
                        if (!float.TryParse(xe.InnerText, out data.max))
                            data.max = 0;
                    }
                }
                _randomAttributeList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

}