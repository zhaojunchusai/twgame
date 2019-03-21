
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class ArenaAwardInfo 
{
	/// <summary>
	/// 奖励编号
	/// </summary>
	public int id;
	/// <summary>
	/// 最高排名
	/// </summary>
	public int min_rank;
	/// <summary>
	/// 最低排名
	/// </summary>
	public int max_rank;
	/// <summary>
	/// 奖励包ID
	/// </summary>
	public int drop_id;

    public override string ToString()
    {
        return string.Format("{0} {1} {2} {3}",id,min_rank,max_rank,drop_id);
    }
}

public class ArenaAwardConfig : BaseConfig
{
	private List<ArenaAwardInfo> _arenaAwardList;

	public ArenaAwardConfig()
	{
        _arenaAwardList = new List<ArenaAwardInfo>();
        Initialize(GlobalConst.Config.DIR_XML_ARENAAWARD, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
        if (_arenaAwardList != null)
		{
			_arenaAwardList.Clear();
			_arenaAwardList = null;
		}
	}

	public List<ArenaAwardInfo> GetArenaAwardList()
	{
		return _arenaAwardList;
	}

    /// <summary>
    /// 通过ID取得奖励数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ArenaAwardInfo GetArenaAwardInfoByID(int id) 
    {
        ArenaAwardInfo info = _arenaAwardList.Find((data) => { return data.id == id; });
        if (info == null) 
        {
            Debug.LogError("can not get ArenaAwardInfo by ID:" + id);
        }
        return info;
    }

    public ArenaAwardInfo GetArenaAwardInfoByRank(int rank) 
    {
        ArenaAwardInfo info = _arenaAwardList.Find((data) => 
        {
            if (data == null) return false;
            if (data.min_rank <= rank && rank <= data.max_rank) 
            {
                return true;
            }
            else 
            {
                return false;
            }
        });
        return info;
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
				ArenaAwardInfo data = new ArenaAwardInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "id")
					{
						data.id = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "min_rank")
					{
						data.min_rank = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "max_rank")
					{
						data.max_rank = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "drop_id")
					{
						data.drop_id = int.Parse(xe.InnerText);
					}
				}

				_arenaAwardList.Add(data);
			}

        	base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load arenaAward XML Error:" + ex.Message);
        }
    }
}