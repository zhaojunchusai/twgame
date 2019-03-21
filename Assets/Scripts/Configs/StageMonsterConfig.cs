
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class StageMonsterInfo 
{
	/// <summary>
	/// 序号
	/// </summary>
	public uint ID;
	/// <summary>
	/// 关卡编号
	/// </summary>
	public uint StageID;
	/// <summary>
	/// 时间段
	/// </summary>
	public List<int> TimeSlot;
	/// <summary>
	/// 出怪权重
	/// </summary>
	public Dictionary<uint, byte> MonsterInfo;
	/// <summary>
	/// 间隔时间
	/// </summary>
	public int TimeInterval;
	/// <summary>
	/// 出怪数
	/// </summary>
	public int MonsterNum;
	/// <summary>
	/// 出怪起始位置
	/// </summary>
	public int InitPosX;

    public StageMonsterInfo()
    {
        Init();
    }

    public void CopyTo(StageMonsterInfo vInfo)
    {
        Init();
        if (vInfo == null)
            return;

        this.ID = vInfo.ID;
        this.StageID = vInfo.StageID;
        if (vInfo.TimeSlot != null)
        {
            for (int i = 0; i < vInfo.TimeSlot.Count; i++)
            {
                this.TimeSlot.Add(vInfo.TimeSlot[i]);
            }
        }
        if (vInfo.MonsterInfo != null)
        {
            foreach (KeyValuePair<uint, byte> tmpInfo in vInfo.MonsterInfo)
            {
                this.MonsterInfo.Add(tmpInfo.Key, tmpInfo.Value);
            }
        }
        this.TimeInterval = vInfo.TimeInterval;
        this.MonsterNum = vInfo.MonsterNum;
        this.InitPosX = vInfo.InitPosX;
    }

    private void Init()
    {
        ID = 0;
        StageID = 0;
        if (TimeSlot == null)
            TimeSlot = new List<int>();
        else
            TimeSlot.Clear();
        if (MonsterInfo == null)
            MonsterInfo = new Dictionary<uint, byte>();
        else
            MonsterInfo.Clear();
        TimeInterval = 0;
        MonsterNum = 0;
        InitPosX = 0;
    }
}

public class StageMonsterConfig : BaseConfig 
{
	private List<StageMonsterInfo> _stageMonsterList;

	public StageMonsterConfig()
	{
		_stageMonsterList = new List<StageMonsterInfo>();
        Initialize(GlobalConst.Config.DIR_XML_STAGEMONSTER, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);
	}

	public void Uninitialize()
	{
		 if (_stageMonsterList  != null)
		{
			_stageMonsterList.Clear();
			_stageMonsterList = null;
		}
	}

	public List<StageMonsterInfo> GetStageMonsterList()
	{
		return _stageMonsterList;
	}

    public List<StageMonsterInfo> FindByID(uint vStageID)
    {
        List<StageMonsterInfo> tmpResultList = new List<StageMonsterInfo>();

        for (int i = 0; i < _stageMonsterList.Count; i++)
        {
            if (_stageMonsterList[i].StageID != vStageID)
                continue;
            tmpResultList.Add(_stageMonsterList[i]);
        }

        return tmpResultList;
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
				StageMonsterInfo data = new StageMonsterInfo();
                //Debug.LogWarning("-----------------------------------------------------------");
	 			foreach (XmlElement xe in Element) 
				{
                    //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					if (xe.Name == "ID")
					{
                        data.ID = uint.Parse(xe.InnerText);
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					}
					else if (xe.Name == "StageID")
                    {
                        data.StageID = uint.Parse(xe.InnerText);
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					}
					else if (xe.Name == "TimeSlot")
					{
                        //data.TimeSlot = xe.InnerText;
                        data.TimeSlot = new List<int>();
                        string[] tmpArray = xe.InnerText.Split(',');
                        for (int i = 0; i < tmpArray.Length; i++)
                        {
                            int tmpInfo = 0;
                            if (int.TryParse(tmpArray[i], out tmpInfo))
                                data.TimeSlot.Add(tmpInfo);
                            else
                                Debug.LogError(string.Format("Error TimeSlot Data: {0}", xe.InnerText));
                        }
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					}
					else if (xe.Name == "MonsterInfo")
                    {
                        //data.MonsterInfo = xe.InnerText;
                        data.MonsterInfo = new Dictionary<uint, byte>();
                        string[] tmpArray = xe.InnerText.Split(';');
                        for (int i = 0; i < tmpArray.Length; i++)
                        {
                            if (string.IsNullOrEmpty(tmpArray[i]))
                                continue;
                            string[] tmpArrInfo = tmpArray[i].Split(':');

                            uint tmpID = 0;
                            byte tmpType = 0;
                            if ((uint.TryParse(tmpArrInfo[0], out tmpID)) && (byte.TryParse(tmpArrInfo[1], out tmpType)))
                                data.MonsterInfo.Add(tmpID, tmpType);
                            else
                                Debug.LogError(string.Format("Error MonsterInfo Data: {0}", xe.InnerText));
                        }
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					}
					else if (xe.Name == "TimeInterval")
					{
                        data.TimeInterval = int.Parse(xe.InnerText);
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
					}
					else if (xe.Name == "MonsterNum")
					{
						data.MonsterNum = int.Parse(xe.InnerText);
					}
					else if (xe.Name == "InitPosX")
					{
						data.InitPosX = int.Parse(xe.InnerText);
					}
				}

				_stageMonsterList.Add(data);
			}

        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError("Load stageMonster XML Error:" + ex.Message);
        }
    }
}