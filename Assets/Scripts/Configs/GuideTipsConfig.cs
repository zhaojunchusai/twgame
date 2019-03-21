using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class GuideTips
{
	/// <summary>
	/// Id
	/// </summary>
    public uint TipsID;
	/// <summary>
	/// 开启关卡
	/// </summary>
    public uint OpenGate;
	/// <summary>
    /// 关闭ID
	/// </summary>
    public uint CloseGate;
    /// <summary>
    /// 描述
    /// </summary>
    public string Descript;
    /// <summary>
    /// 后置ID
    /// </summary>
    public uint NextId;
}

public class _GuideTipsConfig : BaseConfig 
{
	private List<GuideTips> _GuideTipsList;

	public _GuideTipsConfig()
	{
		_GuideTipsList = new List<GuideTips>();
        Initialize(GlobalConst.Config.DIR_XML_GUIDETIPS, ParseConfig);
	}
    public List<GuideTips> GetConfigList()
    {
        return _GuideTipsList;
    }
    public override void Initialize(string vFileName, System.Action<string> callback)
	{
        base.Initialize(vFileName, callback);
	}

    public GuideTips FindById(uint vId)
    {
        return this._GuideTipsList.Find((data) => { if (data == null)return false; return data.TipsID == vId; });
    }
	public void Uninitialize()
	{
		 if (_GuideTipsList  != null)
		{
			_GuideTipsList.Clear();
			_GuideTipsList = null;
		}
	}

	public List<GuideTips> GetSacrificialSystemList()
	{
		return _GuideTipsList;
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
				GuideTips data = new GuideTips();
	 			foreach (XmlElement xe in Element) 
				{
                    if (xe.Name == "TipsID")
					{
                        data.TipsID = uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "OpenGate")
					{
                        data.OpenGate = uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "CloseGate")
					{
                        data.CloseGate = uint.Parse(xe.InnerText);
					}
                    else if (xe.Name == "NextId")
                    {
                        data.NextId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Descript")
                    {
                        data.Descript = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                }
				_GuideTipsList.Add(data);
			}

        	base.LoadCompleted();
       		Debug.Log("Load ChapterMonster XML Success!----count:" + _GuideTipsList.Count);
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}