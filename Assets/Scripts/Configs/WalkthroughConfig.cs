using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

/// <summary>
/// 攻略类型
/// </summary>
public enum EWalkthroughType
{
    ewtNone = 0,
    /// <summary>
    /// 跳转
    /// </summary>
    ewtJumpTo = 1,
    /// <summary>
    /// 阵容
    /// </summary>
    ewtLineup = 2
}

public class JumpToInfo
{
    /// <summary>
    /// 标题
    /// </summary>
    public string Title;
    /// <summary>
    /// 跳转/显示项列表
    /// </summary>
    public List<int> ListProjects;

    public JumpToInfo(string vTitle, List<int> vListProjects)
    {
        InitInfo(vTitle, vListProjects);
    }
    public JumpToInfo(JumpToInfo vInfo)
    {
        if (vInfo != null)
        {
            InitInfo(vInfo.Title, vInfo.ListProjects);
        }
        else
        {
            InitInfo();
        }
    }

    private void InitInfo()
    {
        Title = "";
        if (ListProjects == null)
        {
            ListProjects = new List<int>();
        }
        else
        {
            ListProjects.Clear();
        }
    }
    private void InitInfo(string vTitle, List<int> vListProjects)
    {
        InitInfo();
        Title = vTitle;
        if (vListProjects != null)
        {
            foreach (int tmpInfo in vListProjects)
            {
                ListProjects.Add(tmpInfo);
            }
        }
    }
}

public class WalkthroughInfo
{
    /// <summary>
    /// 攻略编号
    /// </summary>
    public uint ID;
    /// <summary>
    /// 攻略类型[]
    /// </summary>
    public EWalkthroughType Type;
    /// <summary>
    /// 攻略名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 攻略内容
    /// </summary>
    public string Content;
    /// <summary>
    /// 攻略相关跳转[标题一:ID,ID,ID;标题二:ID,ID]
    /// 下流:11461031,11461032,11461033;中流:11461031,11461032,11461033,11461034
    /// </summary>
    public List<JumpToInfo> ListShowInfo;


    public WalkthroughInfo()
    {
        Init();
    }

    private void Init()
    {
        this.ID = 0;
        this.Type = EWalkthroughType.ewtNone;
        this.Name = "";
        this.Content = "";
        if (this.ListShowInfo == null)
        {
            this.ListShowInfo = new List<JumpToInfo>();
        }
        else
        {
            this.ListShowInfo.Clear();
        }
    }

    public void CopyTo(WalkthroughInfo vInfo)
    {
        Init();
        if (vInfo == null)
            return;
        this.ID = vInfo.ID;
        this.Type = vInfo.Type;
        this.Name = vInfo.Name;
        this.Content = vInfo.Content;
        if (vInfo.ListShowInfo != null)
        {
            foreach (JumpToInfo tmpInfo in vInfo.ListShowInfo)
            {
                this.ListShowInfo.Add(new JumpToInfo(tmpInfo));
            }
        }
    }
}

public class WalkthroughConfig : BaseConfig
{
	private List<WalkthroughInfo> _walkthroughList;

    public WalkthroughConfig()
	{
		_walkthroughList = new List<WalkthroughInfo>();
        Initialize(GlobalConst.Config.DIR_XML_WALKTHROUGH, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_walkthroughList  != null)
		{
			_walkthroughList.Clear();
			_walkthroughList = null;
		}
	}

	public List<WalkthroughInfo> GetWalkthroughList()
	{
		return _walkthroughList;
	}

    /// <summary>
    /// 通过ID获取数据
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public WalkthroughInfo FindByID(uint vID)
    {
        if (_walkthroughList == null)
            return null;
        for (int i = 0; i < _walkthroughList.Count; i++)
        {
            if (vID == _walkthroughList[i].ID)
                return _walkthroughList[i];
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
                //Debug.LogWarning("====================================");
				WalkthroughInfo data = new WalkthroughInfo();
	 			foreach (XmlElement xe in Element) 
				{
                    //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					if (xe.Name == "ID")
					{
                        uint tmpID = 0;
                        uint.TryParse(xe.InnerText, out tmpID);
                        data.ID = tmpID;
                    }
                    else if (xe.Name == "Type")
                    {
                        int tmpType = 0;
                        int.TryParse(xe.InnerText, out tmpType);
                        data.Type = (EWalkthroughType)tmpType;
                    }
                    else if (xe.Name == "Name")
                    {
                        data.Name = xe.InnerText;
                    }
                    else if (xe.Name == "Content")
                    {
                        data.Content = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "JumpTo")
                    {
                        string[] tmpArrFirst = xe.InnerText.Split(';');
                        if ((tmpArrFirst != null) && (tmpArrFirst.Length > 0))
                        {
                            for (int i = 0; i < tmpArrFirst.Length; i++)
                            {
                                string[] tmpArrSecond = tmpArrFirst[i].Split(':');
                                if (tmpArrSecond != null)
                                {
                                    string tmpTitle = "";
                                    List<int> tmpList = new List<int>();
                                    if (tmpArrSecond.Length > 0)
                                    {
                                        tmpTitle = string.Format("{0}：", tmpArrSecond[0]);
                                    }
                                    if (tmpArrSecond.Length > 1)
                                    {
                                        //Debug.LogError(tmpArrSecond[1]);
                                        string[] tmpArrThird = tmpArrSecond[1].Split(',');
                                        if ((tmpArrThird != null) && (tmpArrThird.Length > 0))
                                        {
                                            for (int j = 0; j < tmpArrThird.Length; j++)
                                            {
                                                int tmpIntValue = 0;
                                                int.TryParse(tmpArrThird[j], out tmpIntValue);
                                                tmpList.Add(tmpIntValue);
                                            }
                                        }
                                    }
                                    data.ListShowInfo.Add(new JumpToInfo(tmpTitle, tmpList));
                                }
                            }
                        }
                    }
				}

				_walkthroughList.Add(data);
            }
            //ShowWalkthroughConfig();
        	base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Walkthrough XML Error:" + ex.Message);
        }
    }


    private void ShowWalkthroughConfig()
    {
        if (_walkthroughList != null)
        {
            foreach (WalkthroughInfo tmpSingleInfo in _walkthroughList)
            {
                Debug.LogError("--------------------------------------------------------------------------");
                Debug.LogWarning(string.Format("ID: [{0}]", tmpSingleInfo.ID));
                Debug.LogWarning(string.Format("Name: [{0}]", tmpSingleInfo.Name));
                Debug.LogWarning(string.Format("Content: [{0}]", tmpSingleInfo.Content));
                Debug.LogWarning("JumpTo: ");
                if (tmpSingleInfo.ListShowInfo != null)
                {
                    foreach (JumpToInfo tmpSingle in tmpSingleInfo.ListShowInfo)
                    {
                        Debug.Log(string.Format("Title: [{0}]", tmpSingle.Title));
                        string tmpInt = "";
                        if (tmpSingle.ListProjects != null)
                        {
                            foreach (int tmp in tmpSingle.ListProjects)
                            {
                                tmpInt += string.Format("{0},", tmp);
                            }
                        }
                        Debug.Log(string.Format("Show: [{0}]", tmpInt));
                    }
                }
            }
        }
    }
}
