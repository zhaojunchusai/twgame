
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class ExpeditionData
{
	/// <summary>
	/// 关卡ID
	/// </summary>
	public uint id;
	/// <summary>
	/// 关卡名称
	/// </summary>
	public string name;
	/// <summary>
	/// 上一关卡
	/// </summary>
	public uint prev_id;
	/// <summary>
	/// 下一关卡
	/// </summary>
	public uint next_id;
	/// <summary>
	/// 玩法大类
	/// </summary>
	public int type;
	/// <summary>
	/// 章节编号
	/// </summary>
	public uint chapter_id;
	/// <summary>
	/// 小关编号
	/// </summary>
	public int progress;
	/// <summary>
	/// 时间限制
	/// </summary>
	public int time_limit;
	/// <summary>
	/// 关卡掉落包ID
	/// </summary>
	public uint drop_bagid;
	/// <summary>
	/// 战斗模式
	/// </summary>
	public int model;
	/// <summary>
	/// 关卡描述
	/// </summary>
	public string intrduce;
	/// <summary>
	/// 场景背景资源编号
	/// </summary>
	public string background;
	/// <summary>
	/// 最小匹配系数
	/// </summary>
	public int min_match_factor;
	/// <summary>
	/// 最大匹配系数
	/// </summary>
	public int max_match_factor;
    /// <summary>
    /// 关卡自动战斗初始状态[0-锁定 1-未激活 2-激活]
    /// </summary>
    public int autoStatus;
    /// <summary>
    /// 关卡位置
    /// </summary>
    public Vector3 stagePos;
    /// <summary>
    /// 关卡图标
    /// </summary>
    public string stageIcon;
    /// <summary>
    /// 宝箱位置
    /// </summary>
    public Vector3 awardPos;
    /// <summary>
    /// 宝箱图标
    /// </summary>
    public List<string> awardIcon;
    /// <summary>
    /// 城堡类型
    /// </summary>
    public byte castletype;
}

public class ExpeditionConfig : BaseConfig 
{
    private List<ExpeditionData> _expeditionList;

	public ExpeditionConfig()
	{
        _expeditionList = new List<ExpeditionData>();
        Initialize(GlobalConst.Config.DIR_XML_EXPEDITION, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_expeditionList  != null)
		{
			_expeditionList.Clear();
			_expeditionList = null;
		}
	}

    public List<ExpeditionData> GetExpeditionList()
	{
		return _expeditionList;
	}

    /// <summary>
    /// 通过ID获得远征配置表数据
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public ExpeditionData GetExpeditionDataByID(uint id) 
    {
        ExpeditionData data = _expeditionList.Find((tmp) => { return tmp.id == id; });
        if (data == null) 
        {
            Debug.LogError("can not get ExpeditionData by id:" + id);
        }
        return data;
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
                ExpeditionData data = new ExpeditionData();
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
					else if (xe.Name == "prev_id")
					{
						data.prev_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "next_id")
					{
						data.next_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "type")
					{
						data.type = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "chapter_id")
					{
						data.chapter_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "progress")
					{
						data.progress = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "time_limit")
					{
						data.time_limit = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "drop_bagid")
					{
						data.drop_bagid = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "model")
					{
						data.model = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "intrduce")
					{
						data.intrduce = xe.InnerText;
					}
					else if (xe.Name == "background")
					{
						data.background = xe.InnerText;
					}
					else if (xe.Name == "min_match_factor")
					{
						data.min_match_factor = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "max_match_factor")
					{
						data.max_match_factor = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "autoStatus")
                    {
                        if (!int.TryParse(xe.InnerText, out data.autoStatus))
                            data.autoStatus = 0;
                    }
                    else if (xe.Name == "pos")
                    {
                        data.stagePos = Vector3.zero;
                        string[] tmpStrArr = xe.InnerText.Split(':');
                        if (tmpStrArr.Length == 2)
                        {
                            float tmpPosX = 0;
                            float tmpPosY = 0;
                            if (!float.TryParse(tmpStrArr[0], out tmpPosX))
                                tmpPosX = 0;
                            if (!float.TryParse(tmpStrArr[1], out tmpPosY))
                                tmpPosY = 0;
                            data.stagePos = new Vector3(tmpPosX, tmpPosY, 0);
                        }
                    }
                    else if (xe.Name == "icon")
                    {
                        data.stageIcon = xe.InnerText;
                    }
                    else if (xe.Name == "award_pos")
                    {
                        data.awardPos = Vector3.zero;
                        string[] tmpStrArr = xe.InnerText.Split(':');
                        if (tmpStrArr.Length == 2)
                        {
                            float tmpPosX = 0;
                            float tmpPosY = 0;
                            if (!float.TryParse(tmpStrArr[0], out tmpPosX))
                                tmpPosX = 0;
                            if (!float.TryParse(tmpStrArr[1], out tmpPosY))
                                tmpPosY = 0;
                            data.awardPos = new Vector3(tmpPosX, tmpPosY, 0);
                        }
                    }
                    else if (xe.Name == "award_icon")
                    {
                        data.awardIcon = new List<string>();
                        string[] tmpStrArr = xe.InnerText.Split(',');
                        for (int i = 0; i < tmpStrArr.Length; i++)
                            data.awardIcon.Add(tmpStrArr[i]);
                    }
                    else if (xe.Name == "castletype")
                    {
                        if (!byte.TryParse(xe.InnerText, out data.castletype))
                            data.castletype = 1;
                    }
				}
				_expeditionList.Add(data);
			}

        	base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load expedition XML Error:" + ex.Message + ex.StackTrace);
        }
    }
}