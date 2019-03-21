using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class ExpeditionChapterInfo 
{
	/// <summary>
	/// 章节ID
	/// </summary>
	public uint chapter_id;
	/// <summary>
	/// 等级下限
	/// </summary>
	public int min_level;
	/// <summary>
	/// 等级上限
	/// </summary>
	public int max_level;
	/// <summary>
	/// 首关ID
	/// </summary>
	public uint first_dungeon_id;
	/// <summary>
	/// 关卡列表
	/// </summary>
	public List<uint> dungeon_id;
    /// <summary>
    /// 背景图列表
    /// </summary>
    public List<string> backdrop_pic;
}

public class ExpeditionChapterConfig : BaseConfig 
{
	private List<ExpeditionChapterInfo> _expeditionChapterList;

	public ExpeditionChapterConfig()
	{
		_expeditionChapterList = new List<ExpeditionChapterInfo>();
        Initialize(GlobalConst.Config.DIR_XML_EXPEDITIONCHAPTER, ParseConfig);
	}
    
	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_expeditionChapterList  != null)
		{
			_expeditionChapterList.Clear();
			_expeditionChapterList = null;
		}
	}

	public List<ExpeditionChapterInfo> GetExpeditionChapterList()
	{
		return _expeditionChapterList;
	}

    public ExpeditionChapterInfo FindByID(uint vID)
    {
        if (_expeditionChapterList == null)
            return null;
        foreach (ExpeditionChapterInfo tmpInfo in _expeditionChapterList)
        {
            if (tmpInfo.chapter_id == vID)
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
				ExpeditionChapterInfo data = new ExpeditionChapterInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "chapter_id")
					{
						data.chapter_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "min_level")
					{
						data.min_level = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "max_level")
					{
						data.max_level = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "first_dungeon_id")
					{
						data.first_dungeon_id = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "dungeon_id")
					{
                        data.dungeon_id = new List<uint>();
                        string[] tmpInfos = xe.InnerText.Split(';');
                        if (tmpInfos.Length != null)
                        {
                            for (int i = 0; i < tmpInfos.Length; i++)
                            {
                                uint tmpID = 0;
                                if (!uint.TryParse(tmpInfos[i], out tmpID))
                                    tmpID = 0;
                                if (!data.dungeon_id.Contains(tmpID))
                                    data.dungeon_id.Add(tmpID);
                            }
                        }
					}
                    else if (xe.Name == "backdrop_pic")
                    {
                        data.backdrop_pic = new List<string>();
                        string[] tmpStrArr = xe.InnerText.Split(',');
                        for (int i = 0; i < tmpStrArr.Length; i++)
                            data.backdrop_pic.Add(tmpStrArr[i]);
                    }
				}

				_expeditionChapterList.Add(data);
			}
        	base.LoadCompleted();
		}
        catch (Exception ex)
        {
            Debug.LogError(ex.Message + ex.StackTrace);
        }
    }
}