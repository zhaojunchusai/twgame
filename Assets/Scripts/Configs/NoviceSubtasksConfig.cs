using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;


public class NoviceSubtasksInfo
{
    public int ID;

    public string desc;
    /// <summary>
    /// 武将、装备、神器的品质判断，没有该判断时填0。当条件类型为4、5、6时，读取该判断
    /// </summary>
    public int quality;
    /// <summary>
    /// 武将、装备、神器的星级判断，没有该判断时填0。当条件类型为4、5、6时，读取该判断
    /// </summary>
    public int star;
    /// <summary>
    /// 武将、装备、神器、玩家的等级判断，没有该判断时填0。当条件类型为1、4、5、6时，读取该判断
    /// </summary>
    public int level;
    /// <summary>
    /// 通常是任务最终的判断条件：
    /// 当条件类型为1时，默认填0占位
    /// 当条件类型为2时，填入目标战力
    /// 当条件类型为3时，填入关卡编号
    /// 当条件类型为4、5、6时，该条表示数量
    /// </summary>
    public int num;
    /// <summary>
    /// 任务奖励 掉落包ID
    /// </summary>
    public int dropID;
}

public class NoviceSubtasksConfig : BaseConfig
{
    private List<NoviceSubtasksInfo> noviceSubtasksList;

    public NoviceSubtasksConfig()
    {
        noviceSubtasksList = new List<NoviceSubtasksInfo>();
        Initialize(GlobalConst.Config.DIR_XML_NOVICESUBTASKS, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public List<NoviceSubtasksInfo> GetAllNoviceSubtasksList()
    {
        return noviceSubtasksList;
    }

    public List<NoviceSubtasksInfo> GetNoviceSubtasksList(int[] idList)
    {
        List<NoviceSubtasksInfo> list = new List<NoviceSubtasksInfo>();
        for (int i = 0; i < idList.Length; i++)
        {
            int id = idList[i];
            NoviceSubtasksInfo info = GetNoviceSubtasksByID(id);
            if (info == null)
                continue;
            list.Add(info);
        }
        return list;
    }


    public NoviceSubtasksInfo GetNoviceSubtasksByID(int id)
    {
        NoviceSubtasksInfo info = noviceSubtasksList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.ID == id;
        });
        if (info == null)
            Debug.LogError("can not find NoviceSubtasksInfo by ID: " + id);
        return info;
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
                NoviceSubtasksInfo data = new NoviceSubtasksInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "desc")
                    {
                        data.desc = xe.InnerText;
                    }
                    else if (xe.Name == "quality")
                    {
                        data.quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "star")
                    {
                        data.star = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "level")
                    {
                        data.level = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "num")
                    {
                        data.num = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "drop_id")
                    {
                        data.dropID = int.Parse(xe.InnerText);
                    }
                }
                noviceSubtasksList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load NoviceSubtasks XML Error:" + ex.Message + ex.StackTrace);
        }
    }

    public void Uninitialize() 
    {
        noviceSubtasksList = null;
    }
}