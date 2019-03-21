
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class ChapterGradeInfo 
{
	/// <summary>
	/// 章节编号
	/// </summary>
	public uint ChapterID;
	/// <summary>
	/// 积分-小怪
	/// </summary>
	public int MonsterScore;
	/// <summary>
    /// 积分-BOSS
	/// </summary>
	public int BossScore;
	/// <summary>
    /// 积分-敌方城堡
	/// </summary>
	public int EnemyCastleScore;
	/// <summary>
    /// 积分-己方城堡
	/// </summary>
	public int SelfCastleScore;
	/// <summary>
    /// 积分-武将
	/// </summary>
	public int SoldierScore;
	/// <summary>
    /// 积分-英雄
	/// </summary>
	public int HeroScore;
}

/// <summary>
/// 关卡角色积分表
/// </summary>
public class ChapterGradeConfig : BaseConfig 
{
	private List<ChapterGradeInfo> _chapterGradeList;

	public ChapterGradeConfig()
	{
		_chapterGradeList = new List<ChapterGradeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_CHAPTERGRADE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_chapterGradeList  != null)
		{
			_chapterGradeList.Clear();
			_chapterGradeList = null;
		}
	}

    /// <summary>
    /// 返回配置表所有数据
    /// </summary>
    /// <returns></returns>
	public List<ChapterGradeInfo> GetChapterGradeList()
	{
		return _chapterGradeList;
	}

    /// <summary>
    /// 通过ID获取单条数据
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public ChapterGradeInfo GetSingleDataByID(uint vID)
    {
        if (_chapterGradeList == null)
            return null;
        for (int i = 0; i < _chapterGradeList.Count; i++)
        {
            if (_chapterGradeList[i].ChapterID == vID)
                return _chapterGradeList[i];
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
				ChapterGradeInfo data = new ChapterGradeInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "ChapterID")
					{
						data.ChapterID = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "MonsterScore")
					{
						data.MonsterScore = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "BossScore")
					{
						data.BossScore = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "EnemyCastleScore")
					{
						data.EnemyCastleScore = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "SelfCastleScore")
					{
						data.SelfCastleScore = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "SoldierScore")
					{
						data.SoldierScore = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "HeroScore")
					{
						data.HeroScore = int.Parse(xe.InnerText);
					}
				}

				_chapterGradeList.Add(data);
			}

        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError("Load chapterGrade XML Error:" + ex.Message);
        }
    }
}