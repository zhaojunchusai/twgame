
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class SoldierSelectInfo 
{
	/// <summary>
	/// 甄选产出品质
	/// </summary>
	public int quality;
	/// <summary>
	/// 甄选消耗类型1=游戏币；2=钻石
	/// </summary>
	public int money_type;
	/// <summary>
	/// 甄选消耗数值
	/// </summary>
	public int money_num;
	/// <summary>
	/// 产出随机包ID
	/// </summary>
	public int output_id;
}

public class SoldierSelectConfig : BaseConfig 
{
	private List<SoldierSelectInfo> _soldierSelectList;

	public SoldierSelectConfig()
	{
		_soldierSelectList = new List<SoldierSelectInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SOLDIERSELECTATTRIBUTE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);

        //NewGuide hh = new NewGuide();
        //hh.init();
	}
    public SoldierSelectInfo FindByQuality(int quality)
    {
        return this._soldierSelectList.Find((info) => { if (info == null)return false; return info.quality == quality; });
    }
	public void Uninitialize()
	{
		 if (_soldierSelectList  != null)
		{
			_soldierSelectList.Clear();
			_soldierSelectList = null;
		}
	}

	public List<SoldierSelectInfo> GetSoldierSelectList()
	{
		return _soldierSelectList;
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
				SoldierSelectInfo data = new SoldierSelectInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "quality")
					{
						data.quality = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "money_type")
					{
						data.money_type = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "money_num")
					{
						data.money_num = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "output_id")
					{
						data.output_id = int.Parse(xe.InnerText);
					}
				}

				_soldierSelectList.Add(data);
			}

        	base.LoadCompleted();}

        catch (Exception ex)
        {
            Debug.LogError("Load SoldierSelect XML Error:" + ex.Message);
        }
    }
}