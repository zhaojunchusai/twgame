
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class SoldierStepInfo 
{
	/// <summary>
	/// 对应的阶级或者星级
	/// </summary>
    public int starOrstep;
	/// <summary>
    /// 对应的星级价值几个一星0阶1级初始武将
	/// </summary>
    public int starValue;
	/// <summary>
	/// 对应的阶级价值几个一星0阶1级初始武将
	/// </summary>
    public int stepValue;
}

public class SoldierStepConfig : BaseConfig 
{
	private List<SoldierStepInfo> _soldierSelectList;

	public SoldierStepConfig()
	{
		_soldierSelectList = new List<SoldierStepInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SOLDIERSELECTATTRIBUTE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);

        //NewGuide hh = new NewGuide();
        //hh.init();
	}
    public SoldierStepInfo FindByStarOrStep(int starOrstep)
    {
        return this._soldierSelectList.Find((info) => { if (info == null)return false; return info.starOrstep == starOrstep; });
    }
	public void Uninitialize()
	{
		 if (_soldierSelectList  != null)
		{
			_soldierSelectList.Clear();
			_soldierSelectList = null;
		}
	}

	public List<SoldierStepInfo> GetSoldierSelectList()
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
				SoldierStepInfo data = new SoldierStepInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "starOrstep")
					{
                        data.starOrstep = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "starValue")
					{
                        data.starValue = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "stepValue")
					{
                        data.stepValue = int.Parse(xe.InnerText);
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