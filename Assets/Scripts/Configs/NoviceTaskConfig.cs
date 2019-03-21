using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class NoviceTaskInfo
{
    /// <summary>
    /// 任务ID
    /// </summary>
    public int ID;
    /// <summary>
    /// 任务名称 
    /// </summary>
    public string taskName;
    /// <summary>
    /// 任务描述
    /// </summary>
    public string desc;
    /// <summary>
    /// 子任务
    /// </summary>
    public int[] subTasks;
    /// <summary>
    /// 条件类型
    /// 1-主角等级检测；
    /// 2-英雄战力检测；
    /// 3-主线关卡进度检测；
    /// 4-获得神器数量；
    /// 5-获得武将装备数量；
    /// 6-获得武将数量
    /// </summary>
    public int type;
}


public class NoviceTaskConfig : BaseConfig
{
    private List<NoviceTaskInfo> noviceTaskList;

    public NoviceTaskConfig()
    {
        noviceTaskList = new List<NoviceTaskInfo>();
        Initialize(GlobalConst.Config.DIR_XML_NOVICETASK, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
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
                NoviceTaskInfo data = new NoviceTaskInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.ID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "task_name")
                    {
                        data.taskName = xe.InnerText;
                    }
                    else if (xe.Name == "desc")
                    {
                        data.desc = xe.InnerText;
                    }
                    else if (xe.Name == "sub_task")
                    {
                        List<int> subTasks = new List<int>();
                        if (xe.InnerText != "0")
                        {
                            string[] tmpArray = xe.InnerText.Split(';');
                            if ((tmpArray != null) && (tmpArray.Length > 0))
                            {
                                for (int i = 0; i < tmpArray.Length; i++)
                                {
                                    string tmp = tmpArray[i];
                                    int subID = 0;
                                    if (int.TryParse(tmp, out subID))
                                    {
                                        subTasks.Add(subID);
                                    }
                                    else
                                    {
                                        Debug.LogError("subtask try parse to int error -> line:" + data.ID.ToString() + "; value:" + xe.InnerText);
                                    }
                                }
                            }
                        }
                        data.subTasks = subTasks.ToArray();
                    }
                    else if (xe.Name == "type")
                    {
                        data.type = int.Parse(xe.InnerText);
                    }
                }
                noviceTaskList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load NoviceTask XML Error:" + ex.Message + ex.StackTrace);
        }
    }


    public List<NoviceTaskInfo> GetNoviceTaskInfoList()
    {
        return noviceTaskList;
    }

    public void Uninitialize() 
    {
        noviceTaskList = null;
    }
}
