using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

/*
 * 对话表
 * 序列号【uint】
 * 关卡ID【uint】
 * 对话类型【int：0-无类型 1-新手引导 2-Boss出场】
 * 角色类型【int：0-无类型 1-Boss 2-己方英雄 3-敌方英雄 4-己方小兵 5-敌方小兵 6-己方城堡 7-敌方城堡 8-护送目标 9-城堡射手 10-传送门】
 * 目标位置X【int】
 * 移动类型【int：0-移动 1-瞬移】
 * 对话文本【string】
 * 文本显示类型【int：0-普通 1-常显 2-间隔显示】
 * 文本持续时间/显示、间隔时间【int：毫秒数】
 * 镜头移动速度【像素】
 * */
public class BossTalkInfo 
{
	/// <summary>
	/// 序号
	/// </summary>
	public uint ID;
	/// <summary>
	/// 关卡ID
	/// </summary>
	public uint StageID;
    /// <summary>
    /// 对话类型[1-新手引导 2-BOSS出场]
    /// </summary>
    public int MessageType;
	/// <summary>
    /// 说话角色类别[1-BOSS 2-主角 3-己方小兵 4-对方小兵 5-己方城堡 6-对方城堡 7-护送目标]
	/// </summary>
	public int RoleType;
    /// <summary>
    /// 移动目标位置
    /// </summary>
    public int TargetPosX;
    /// <summary>
    /// 移动类型
    /// </summary>
    public int MoveType;
	/// <summary>
	/// 文本
	/// </summary>
	public string Text;
    /// <summary>
    /// 文本显示类型
    /// </summary>
    public int TextType;
    /// <summary>
    /// 显示间隔时间
    /// </summary>
    public int Time;
    /// <summary>
    /// 镜头移动速度
    /// </summary>
    public int MoveSpeed;
}

public class BossTalkConfig : BaseConfig 
{
	private List<BossTalkInfo> _bossTalkList;

	public BossTalkConfig()
	{
		_bossTalkList = new List<BossTalkInfo>();
        Initialize(GlobalConst.Config.DIR_XML_BOSSTALK, ParseConfig);
	}

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

	public void Uninitialize()
	{
		 if (_bossTalkList  != null)
		{
			_bossTalkList.Clear();
			_bossTalkList = null;
		}
	}

	public List<BossTalkInfo> GetBossTalkList()
	{
		return _bossTalkList;
	}

    public BossTalkInfo FindByID(int vNum)
    {
        if (_bossTalkList == null)
            return null;

        for (int i = 0; i < _bossTalkList.Count; i++)
        {
            if (vNum == _bossTalkList[i].ID)
                return _bossTalkList[i];
        }

        return null;
    }

    public List<BossTalkInfo> FindByStageID(uint vStageID)
    {
        if (_bossTalkList == null)
            return null;
        List<BossTalkInfo> tmpResultList = new List<BossTalkInfo>();

        for (int i = 0; i < _bossTalkList.Count; i++)
        {
            if (vStageID == _bossTalkList[i].StageID)
                tmpResultList.Add(_bossTalkList[i]);
        }

        return tmpResultList;
    }

    /// <summary>
    /// 检测是否拥有数据
    /// </summary>
    /// <param name="vStageID"></param>
    /// <param name="vChatType"></param>
    /// <returns></returns>
    public bool CheckIsHaveData(uint vStageID, EChatType vChatType)
    {
        if (_bossTalkList == null)
            return false;

        for (int i = 0; i < _bossTalkList.Count; i++)
        {
            if (vStageID != _bossTalkList[i].StageID)
                continue;
            if ((int)vChatType != _bossTalkList[i].MessageType)
                continue;
            return true;
        }

        return false;
    }

    //private void ParseConfig(WWW www)
    private void ParseConfig(string xmlstr)
    {
        try
        {
            //string xmlstr = System.Text.Encoding.UTF8.GetString(www.bytes);
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                //Debug.LogWarning("==================================================================");
				BossTalkInfo data = new BossTalkInfo();
	 			foreach (XmlElement xe in Element) 
				{
					if (xe.Name == "id")
					{
                        if (!uint.TryParse(xe.InnerText, out data.ID))
                            data.ID = 0;
					}
					else if (xe.Name == "stageID")
					{
                        if (!uint.TryParse(xe.InnerText, out data.StageID))
                            data.StageID = 0;
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                    }
                    else if (xe.Name == "messageType")
                    {
                        if (!int.TryParse(xe.InnerText, out data.MessageType))
                            data.MessageType = 0;
                    }
					else if (xe.Name == "roleType")
					{
                        if (!int.TryParse(xe.InnerText, out data.RoleType))
                            data.RoleType = 0;
					}
                    else if (xe.Name == "targetPosX")
                    {
                        if (!int.TryParse(xe.InnerText, out data.TargetPosX))
                            data.TargetPosX = 0;
                    }
                    else if (xe.Name == "sceneMoveType")
                    {
                        if (!int.TryParse(xe.InnerText, out data.MoveType))
                            data.MoveType = 0;
                    }
                    else if (xe.Name == "txt")
                    {
                        data.Text = xe.InnerText;
                    }
                    else if (xe.Name == "textType")
                    {
                        if (!int.TryParse(xe.InnerText, out data.TextType))
                            data.TextType = 0;
                    }
                    else if (xe.Name == "time")
                    {
                        if (!int.TryParse(xe.InnerText, out data.Time))
                            data.Time = 0;
                        data.MoveSpeed = 0;
                    }
                    else if (xe.Name == "moveSpeed")
                    {
                        if (!int.TryParse(xe.InnerText, out data.MoveSpeed))
                            data.MoveSpeed = 0;
                    }
				}

				_bossTalkList.Add(data);
			}

        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError(ex.Message + ex.StackTrace);
        }
    }
}