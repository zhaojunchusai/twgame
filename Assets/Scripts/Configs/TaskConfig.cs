using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class TaskData
{
    public uint ID;
    public byte Type;
    public string Name;
    public string Icon;
    public int LimitVipLv;
    public int LimitLv;
    public int LimitPreMisson;
    public int PreTaskID;
    public int CompleteType;
    public string TimeJugde;
    public int QualityJugde;
    public int StarJugde;
    public int LevelJugde;
    public int ValueJugde;
    public string AwardsDes;
    public string Des;
    public string skip;
    public uint Awards;
    public uint exp;
    public TaskData() 
    {
        ID = 0;
        Type =0;
        Name = "";
        Icon ="";
        LimitVipLv =0;
        LimitLv =0;
        LimitPreMisson = 0;
        PreTaskID =0;
        CompleteType =0;
        TimeJugde ="";
        QualityJugde =0;
        StarJugde =0;
        LevelJugde =0;
        ValueJugde =0;
        AwardsDes ="";
        Des ="";
        skip ="";
        Awards = 0;
        exp = 0;
    }

    public void CopyTo(TaskData data) 
    {
        ID = data.ID;
        Type = data.Type;
        Name = data.Name;
        Icon = data.Icon;
        LimitVipLv = data.LimitVipLv;
        LimitLv = data.LimitLv;
        LimitPreMisson = data.LimitPreMisson;
        PreTaskID = data.PreTaskID;
        CompleteType = data.CompleteType;
        TimeJugde = data.TimeJugde;
        QualityJugde = data.QualityJugde;
        StarJugde = data.StarJugde;
        LevelJugde = data.LevelJugde;
        ValueJugde = data.ValueJugde;
        AwardsDes = data.AwardsDes;
        Des = data.Des;
        skip = data.skip;
        Awards = data.Awards;
        exp = data.exp;
    }
}

public class TaskConfig : BaseConfig 
{
    private Dictionary<uint, TaskData> _taskData = new Dictionary<uint, TaskData>();

    public TaskConfig() 
    {
        _taskData.Clear();
        Initialize(GlobalConst.Config.DIR_XML_TASK, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void ParseConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                TaskData data = new TaskData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "task_name")
                    {
                        data.Name = xe.InnerText;
                    }
                    else if (xe.Name == "task_type")
                    {
                        data.Type = byte.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "icon")
                    {
                        data.Icon = xe.InnerText;
                    }
                    else if (xe.Name == "vip_limit")
                    {
                        data.LimitVipLv = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "level_limit")
                    {
                        data.LimitLv = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "pre_misson_limit")
                    {
                        data.LimitPreMisson = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "pre_task_id")
                    {
                        data.PreTaskID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "desc")
                    {
                        data.Des = xe.InnerText;
                    }
                    else if (xe.Name == "complete_type")
                    {
                        data.CompleteType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "time_judge")
                    {
                        data.TimeJugde = xe.InnerText;
                    }
                    else if (xe.Name == "quality_jugde")
                    {
                        data.QualityJugde = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "star_jugde")
                    {
                        data.StarJugde = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "level_judge")
                    {
                        data.LevelJugde = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "value_judge")
                    {
                        data.ValueJugde = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "awards_desc")
                    {
                        data.AwardsDes = xe.InnerText;
                    }
                    else if (xe.Name == "awards_packid")
                    {
                        data.Awards = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "skip")
                    {
                        data.skip = xe.InnerText;
                    }
                    else if (xe.Name == "exp")
                    {
                        data.exp = uint.Parse(xe.InnerText);
                    }
                }
                _taskData.Add(data.ID, data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public TaskData GetTaskDataByID(uint id) 
    {
        if (_taskData.ContainsKey(id)) 
        {
            return _taskData[id];
        }
        return null;
    }

    public List<fogs.proto.msg.ItemInfo> GetItemInfo(string str ) 
    {
       List< fogs.proto.msg.ItemInfo> result = new List<fogs.proto.msg.ItemInfo>();
        string[] items = str.Split(';');
        for (int i = 0; i < items.Length;i++)
        {
            string[] info = items[i].Split(':');
            if (info.Length < 2){ Debug.LogError("GetItemInfo Error = "+ str );  continue;}
            fogs.proto.msg.ItemInfo data = new fogs.proto.msg.ItemInfo();
            data.id = uint.Parse(info[0]);
            data.num = int.Parse(info[1]);
            result.Add(data);
        }
        return result;

    }

    public void Uninitialize()
    {
        _taskData.Clear();
    }

}
