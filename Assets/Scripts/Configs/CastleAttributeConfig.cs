using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class CastleAttributeInfo 
{
	/// <summary>
	/// 城堡角色ID
	/// </summary>
	public uint id;
	/// <summary>
	/// 角色名称
	/// </summary>
	public string name;
	/// <summary>
	/// 角色类型
	/// </summary>
	public int type;
	/// <summary>
	/// 角色星级
	/// </summary>
	public int star;
	/// <summary>
	/// 角色品质
	/// </summary>
	public int quality;
	/// <summary>
	/// 士兵解锁等级
	/// </summary>
	public int unlock_level;
	/// <summary>
	/// 解锁消耗包ID
	/// </summary>
	public uint unlock_cost_packid;
	/// <summary>
	/// 初始等级
	/// </summary>
	public int level;
	/// <summary>
	/// 职业ID
	/// </summary>
	public uint job_id;
	/// <summary>
	/// 界面资源名称
	/// </summary>
	public string ui_source;
	/// <summary>
	/// 战斗资源名称
	/// </summary>
	public string fight_source;
	/// <summary>
	/// 音效ID
	/// </summary>
	public uint sound_id;
	/// <summary>
	/// 生命
	/// </summary>
	public int hp_max;
	/// <summary>
	/// 生命成长
	/// </summary>
	public int hp_grow;
	/// <summary>
	/// 攻击
	/// </summary>
	public int phy_atk;
	/// <summary>
	/// 攻击成长
	/// </summary>
	public int atk_grow;
	/// <summary>
	/// 攻击距离
	/// </summary>
	public int atk_space;
	/// <summary>
	/// 攻击间隔
	/// </summary>
	public int atk_interval;
	/// <summary>
	/// 界面缩放比例
	/// </summary>
	public string zoom_rate;
	/// <summary>
	/// 解锁等级:主动技能ID
	/// </summary>
    public CalBaseData unlock_positive;
	/// <summary>
	/// 解锁等级:被动技能ID
	/// </summary>
	public string unlock_passive;
	/// <summary>
	/// 等级上限
	/// </summary>
	public int level_limit;
	/// <summary>
	/// 后置角色ID
	/// </summary>
	public uint suffix_id;
	/// <summary>
	/// 相对等级:升级消耗包ID
	/// </summary>
    public Dictionary<int, uint> levelup_cost;
}

public class CastleAttributeConfig : BaseConfig 
{
	private List<CastleAttributeInfo> _castleAttributeList;

	public CastleAttributeConfig()
	{
		_castleAttributeList = new List<CastleAttributeInfo>();
		Initialize(GlobalConst.Config.DIR_XML_CASTLEATTRIBUTE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_castleAttributeList  != null)
		{
			_castleAttributeList.Clear();
			_castleAttributeList = null;
		}
	}

	public List<CastleAttributeInfo> GetCastleAttributeList()
	{
		return _castleAttributeList;
	}

    public CastleAttributeInfo FindByID(uint vID)
    {
        if (_castleAttributeList == null)
            return null;

        for (int i = 0; i < _castleAttributeList.Count; i++)
        {
            if (vID == _castleAttributeList[i].id)
                return _castleAttributeList[i];
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
				CastleAttributeInfo data = new CastleAttributeInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "id")
					{
						data.id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "name")
					{
						data.name = xe.InnerText;
					}
					else if (xe.Name == "type")
					{
						data.type = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "star")
					{
						data.star = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "quality")
					{
						data.quality = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "unlock_level")
					{
						data.unlock_level = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "unlock_cost_packid")
					{
						data.unlock_cost_packid = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "level")
					{
						data.level = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "job_id")
					{
						data.job_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "ui_source")
					{
						data.ui_source = xe.InnerText;
					}
					else if (xe.Name == "fight_source")
					{
						data.fight_source = xe.InnerText;
					}
					else if (xe.Name == "sound_id")
					{
						data.sound_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "hp_max")
					{
						data.hp_max = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "hp_grow")
					{
                        if (!int.TryParse(xe.InnerText, out data.hp_grow))
						    data.hp_grow = 0;
					}
					else if (xe.Name == "phy_atk")
					{
						data.phy_atk = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "atk_grow")
                    {
                        if (!int.TryParse(xe.InnerText, out data.atk_grow))
                            data.atk_grow = 0;
					}
					else if (xe.Name == "atk_space")
					{
						data.atk_space = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "atk_interval")
					{
						data.atk_interval = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "zoom_rate")
					{
						data.zoom_rate = xe.InnerText;
					}
					else if (xe.Name == "unlock_positive")
					{
                        string[] tmpArray = xe.InnerText.Split(':');
                        data.unlock_positive = new CalBaseData(uint.Parse(tmpArray[1]), int.Parse(tmpArray[0]));
					}
					else if (xe.Name == "unlock_passive")
					{
						data.unlock_passive = xe.InnerText;
					}
					else if (xe.Name == "level_limit")
					{
						data.level_limit = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "suffix_id")
					{
						data.suffix_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "levelup_cost")
					{
                        if (data.levelup_cost == null)
                            data.levelup_cost = new Dictionary<int, uint>();
                        else
                            data.levelup_cost.Clear();

                        string[] tmpArray = xe.InnerText.Split(';');
                        for (int i = 0; i < tmpArray.Length; i++)
                        {
                            string[] tmpInfoArray = tmpArray[i].Split(':');
                            data.levelup_cost.Add(i, uint.Parse(tmpInfoArray[1]));
                        }
					}
				}

				_castleAttributeList.Add(data);
			}

        	base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load castleAttribute XML Error:" + ex.Message);
        }
    }
}