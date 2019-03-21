
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class ArenaInfo 
{
	/// <summary>
	/// 免费挑战次数
	/// </summary>
	public int free_times;
	/// <summary>
	/// 挑战上限
	/// </summary>
	public int max_times;
	/// <summary>
	/// 单次购买次数
	/// </summary>
	public int every_buy_num;
	/// <summary>
	/// 挑战CD时间
	/// </summary>
	public int challenge_cd;
	/// <summary>
	/// 清除CD花费
	/// </summary>
	public int clear_cd_diamond;
	/// <summary>
	/// 结算时间
	/// </summary>
	public string award_time;
	/// <summary>
	/// 重置时间
	/// </summary>
	public string reset_time;
	/// <summary>
	/// 能量上限
	/// </summary>
	public int max_energy;
	/// <summary>
	/// 奖励标题
	/// </summary>
	public string award_title;
	/// <summary>
	/// 奖励内容
	/// </summary>
	public string award_content;
}

public class ArenaConfig : BaseConfig 
{
	private List<ArenaInfo> _arenaList;

	public ArenaConfig()
	{
		_arenaList = new List<ArenaInfo>();
        Initialize(GlobalConst.Config.DIR_XML_ARENA, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_arenaList  != null)
		{
			_arenaList.Clear();
			_arenaList = null;
		}
	}

	public List<ArenaInfo> GetArenaList()
	{
		return _arenaList;
	}

    /// <summary>
    /// 取得演舞台配置数据  PS：该配置比较特殊 仅有一条数据  因而直接取 add by taiwei
    /// </summary>
    /// <returns></returns>
    public ArenaInfo GetArenaInfo() 
    {
        if (_arenaList != null && _arenaList.Count > 0) 
        {
            return  _arenaList[0];
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
				ArenaInfo data = new ArenaInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "free_times")
					{
						data.free_times = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "max_times")
					{
						data.max_times = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "every_buy_num")
					{
						data.every_buy_num = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "challenge_cd")
					{
						data.challenge_cd = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "clear_cd_diamond")
					{
						data.clear_cd_diamond = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "award_time")
					{
						data.award_time = xe.InnerText;
					}
					else if (xe.Name == "reset_time")
					{
						data.reset_time = xe.InnerText;
					}
					else if (xe.Name == "max_energy")
					{
						data.max_energy = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "award_title")
					{
						data.award_title = xe.InnerText;
					}
					else if (xe.Name == "award_content")
					{
						data.award_content = xe.InnerText;
					}
				}
				_arenaList.Add(data);
			}
        	base.LoadCompleted();
		}
        catch (Exception ex)
        {
            Debug.LogError("Load Arena XML Error:" + ex.Message);
        }
    }
}