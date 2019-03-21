
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class SpecialIDInfo 
{
	/// <summary>
	/// 物品ID
	/// </summary>
	public uint ID;
	/// <summary>
	/// 数据
	/// </summary>
	public string Data;
}

/// <summary>
/// 特殊数值表
/// </summary>
public class SpecialIDConfig : BaseConfig 
{
	private List<SpecialIDInfo> _specialIDList;

	public SpecialIDConfig()
	{
		_specialIDList = new List<SpecialIDInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SPECIALID, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_specialIDList  != null)
		{
			_specialIDList.Clear();
			_specialIDList = null;
		}
	}

	public List<SpecialIDInfo> GetSpecialIDList()
	{
		return _specialIDList;
	}

    /// <summary>
    /// 通过ID获取数据
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public string GetSingleDataByID(uint vID)
    {
        if (_specialIDList == null)
            return string.Empty;
        for (int i = 0; i < _specialIDList.Count; i++)
        {
            if (vID == _specialIDList[i].ID)
                return _specialIDList[i].Data;
        }
        return string.Empty;
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
                //Debug.LogWarning("====================================");
				SpecialIDInfo data = new SpecialIDInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					if (xe.Name == "ID")
					{
						data.ID = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "Data")
					{
                        data.Data = CommonFunction.ReplaceEscapeChar(xe.InnerText);
					}
				}

				_specialIDList.Add(data);
			}
        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError("Load specialID XML Error:" + ex.Message);
        }
    }
}