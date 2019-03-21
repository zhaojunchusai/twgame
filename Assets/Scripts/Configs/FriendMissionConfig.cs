using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class FriendMissionInfo
{
	/// <summary>
	/// 类型
	/// </summary>
	public int MissionType;
	/// <summary>
	/// 消耗道具ID
	/// </summary>
	public uint Id;
	/// <summary>
    /// 消耗道具数量
	/// </summary>
    public int PlayerLevel;
    /// <summary>
    /// 消耗金钱类型
    /// </summary>
    public int PlayerCount;
    /// <summary>
    /// 消耗金钱数量
    /// </summary>
    public string Descript;
    /// <summary>
    /// 奖励包
    /// </summary>
    public uint RewardBag;
}

public class FriendMissionConfig : BaseConfig 
{
	private List<FriendMissionInfo> _FriendMissionList;

	public FriendMissionConfig()
	{
		_FriendMissionList = new List<FriendMissionInfo>();
        Initialize(GlobalConst.Config.DIR_XML_FRIENDMISSION, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

    public FriendMissionInfo FindById(uint vId)
    {
        return this._FriendMissionList.Find((data) => { if (data == null)return false; return data.Id == vId; });
    }
    public FriendMissionInfo FindByType(int vType)
    {
        return this._FriendMissionList.Find((data) => { if (data == null)return false; return data.MissionType == vType; });
    }
	public void Uninitialize()
	{
		 if (_FriendMissionList  != null)
		{
			_FriendMissionList.Clear();
			_FriendMissionList = null;
		}
	}

	public List<FriendMissionInfo> GetSacrificialSystemList()
	{
		return _FriendMissionList;
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
				FriendMissionInfo data = new FriendMissionInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "MissionType")
					{
						data.MissionType = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "Id")
					{
						data.Id = uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "PlayerCount")
					{
						data.PlayerCount = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "PlayerLevel")
                    {
                        data.PlayerLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Descript")
                    {
                        data.Descript = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "RewardBag")
                    {
                        data.RewardBag = uint.Parse(xe.InnerText);
                    }
                }

				_FriendMissionList.Add(data);
			}

        	base.LoadCompleted();
       		Debug.Log("Load ChapterMonster XML Success!----count:" + _FriendMissionList.Count);
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}