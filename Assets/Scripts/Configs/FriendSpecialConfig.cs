using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class FriendSpecialInfo
{
	/// <summary>
	/// 消耗道具ID
	/// </summary>
    public FriendSpecialEnum Id;
    /// <summary>
    /// 描述
    /// </summary>
    public string Descript;
}
public enum FriendSpecialEnum
{
    FBSHARETITLE = 1,
    FBSHARELINE = 2,
    FBSHAREDESCRIPT = 3,
    GOOGLEPLAY = 4,
    SDKPLAY = 5,
    LINESHAREDESCRIPT =6,
    LINEINVITEDESCRIPT =7,
    APPDOWNLOADURL = 8
}
public class FriendSpecialConfig : BaseConfig 
{
	private List<FriendSpecialInfo> _FriendMissionList;

	public FriendSpecialConfig()
	{
		_FriendMissionList = new List<FriendSpecialInfo>();
        Initialize(GlobalConst.Config.DIR_XML_FRIENDSPECIAL, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

    public FriendSpecialInfo FindById(FriendSpecialEnum vId)
    {
        return this._FriendMissionList.Find((data) => { if (data == null)return false; return data.Id == vId; });
    }
	public void Uninitialize()
	{
		 if (_FriendMissionList  != null)
		{
			_FriendMissionList.Clear();
			_FriendMissionList = null;
		}
	}

	public List<FriendSpecialInfo> GetSacrificialSystemList()
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
				FriendSpecialInfo data = new FriendSpecialInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "ID")
					{
                        data.Id = (FriendSpecialEnum)uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "Data")
                    {
                        data.Descript = CommonFunction.ReplaceEscapeChar(xe.InnerText);
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