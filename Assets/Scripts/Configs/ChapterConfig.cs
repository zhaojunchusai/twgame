using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

/* File：ChapterConfig.cs
 * Desc: 章节配置表
 * Date: 2015-05-13 18:47
 * add by taiwei 
 */
public class ChapterInfo
{
    /// <summary>
    /// 章节ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 章节名称
    /// </summary>
    public string name;
    /// <summary>
    /// 上一章节ID
    /// </summary>
    public uint previd;
    /// <summary>
    /// 下一章节ID
    /// </summary>
    public uint nextid;
    /// <summary>
    /// 讨伐之路满星奖励
    /// </summary>
    public List<CommonItemData> crusadeAwards;
    /// <summary>
    /// 精英讨伐满星奖励
    /// </summary>
    public List<CommonItemData> eliteCrusadeAwards;
    /// <summary>
    /// 护送模式满星奖励
    /// </summary>
    public List<CommonItemData> escortAwards;
    /// <summary>
    /// 关卡列表
    /// </summary>
    public List<uint> gates;
    /// <summary>
    /// 章节背景图
    /// </summary>
    public string resicon;
    /// <summary>
    /// 章节类型(0 主线副本 1 活动 2 无尽 3 远征 4 异域探险)
    /// </summary>
    public int chapterType;
    /// <summary>
    /// 开放时间(0 星期天，1 星期一等)
    /// </summary>
    public List<ChapterOpenTime> openTime;
    /// <summary>
    /// 挑战次数
    /// </summary>
    public int Challenge;
    /// <summary>
    /// 等级限制
    /// </summary>
    public int LVLimit;
    /// <summary>
    /// 路线图坐标
    /// </summary>
    public List<Vector2> pointPos;
}

public class ChapterOpenTime
{
    public int week;
    public int startTime;
    public int endTime;

    public ChapterOpenTime(int w, int start, int end)
    {
        this.week = w;
        this.startTime = start;
        this.endTime = end;
    }
}

public class ChapterConfig : BaseConfig
{
    private List<ChapterInfo> _chapterList;

    public ChapterConfig()
    {
        _chapterList = new List<ChapterInfo>();
        Initialize(GlobalConst.Config.DIR_XML_CHAPTER, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_chapterList != null)
        {
            _chapterList.Clear();
            _chapterList = null;
        }
    }

    public List<ChapterInfo> GetChapterList()
    {
        //_chapterList.Sort((ChapterInfo left,ChapterInfo right) => 
        //{
        //    if (left == null || right == null)
        //    {
        //        return 0;
        //    }
        //    return left.id.CompareTo(right.id);
        //});
        return _chapterList;
    }

    public List<ChapterInfo> GetMainBattleChapters()
    {
        List<ChapterInfo> list = new List<ChapterInfo>();
        List<ChapterInfo> all = GetChapterList();
        for (int i = 0; i < all.Count; i++)
        {
            ChapterInfo info = all[i];
            if (info == null)
            {
                continue;
            }
            if (info.chapterType == 0)
            {
                list.Add(info);
            }
        }
        list.Sort((ChapterInfo left, ChapterInfo right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            return left.id.CompareTo(right.id);
        });
        return list;
    }

    public List<ChapterInfo> GetExoticAdvantureChapters() 
    {
        List<ChapterInfo> list = new List<ChapterInfo>();
        List<ChapterInfo> all = GetChapterList();
        for (int i = 0; i < all.Count; i++)
        {
            ChapterInfo info = all[i];
            if (info == null)
            {
                continue;
            }
            if (info.chapterType == 4)
            {
                list.Add(info);
            }
        }
        list.Sort((ChapterInfo left, ChapterInfo right) =>
        {
            if (left == null || right == null)
            {
                return 0;
            }
            return left.id.CompareTo(right.id);
        });
        return list;
    }

    /// <summary>
    /// 主线副本第一章节数据
    /// </summary>
    /// <returns></returns>
    public ChapterInfo GetFirstChapterInfo()
    {
        for (int i = 0; i < _chapterList.Count; i++)
        {
            ChapterInfo chapterInfo = _chapterList[i];
            if (chapterInfo.chapterType == 0) 
            {
                if (chapterInfo.previd == 0)
                {
                    return _chapterList[i];
                }
            }
        }
        Debug.LogError("can not find first chapter!");
        return null;
    }

    /// <summary>
    /// 军团副本第一章节数据
    /// </summary>
    /// <returns></returns>
    public ChapterInfo GetFirstUnionChapterInfo()
    {
        for (int i = 0; i < _chapterList.Count; i++)
        {
            ChapterInfo chapterInfo = _chapterList[i];
            if (chapterInfo.chapterType == 4)
            {
                if (chapterInfo.previd == 0)
                {
                    return _chapterList[i];
                }
            }
        }
        Debug.LogError("can not find first chapter!");
        return null;
    }


    public ChapterInfo GetChapterByID(uint id)
    {
        ChapterInfo info = _chapterList.Find((chapter) =>
        {
            return chapter.id == id;
        });
        if (info == null)
        {
            Debug.LogError("can not get Chapterinfo by ID:" + id);
        }
        return info;
    }

    public ChapterInfo GetChapterByGateID(uint gateID)
    {
        for (int i = 0; i < _chapterList.Count; i++)
        {
            ChapterInfo info = _chapterList[i];
            if (info.gates.Contains(gateID))
            {
                return info;
            }
        }
        Debug.LogError("can not get Chapterinfo by Gate ID:" + gateID);
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
                ChapterInfo data = new ChapterInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "previd")
                    {
                        data.previd = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "nextid")
                    {
                        data.nextid = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "name")
                    {
                        data.name = xe.InnerText;
                    }
                    else if (xe.Name == "resicon")
                    {
                        data.resicon = xe.InnerText;
                    }
                    else if (xe.Name == "gates")
                    {
                        data.gates = new List<uint>();
                        string[] _gates = xe.InnerText.Split(';');
                        for (int i = 0; i < _gates.Length; i++)
                        {
                            uint id = 0;
                            if (uint.TryParse(_gates[i], out id))  //不能保证数据填错的可能
                            {
                                data.gates.Add(id);
                            }
                            else
                            {
                                Debug.LogError("Error Gate ID:" + _gates[i]);
                            }
                        }
                    }
                    else if (xe.Name == "normal")
                    {
                        if (data.crusadeAwards == null)
                        {
                            data.crusadeAwards = new List<CommonItemData>();
                        }
                        string[] arr = xe.InnerText.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string[] array = arr[i].Split(':');
                            if (array.Length == 2)
                            {
                                int star, dropID;
                                if (int.TryParse(array[0], out star) && int.TryParse(array[1], out dropID))
                                {
                                    CommonItemData commonData = new CommonItemData();
                                    commonData.ID = (uint)dropID;
                                    commonData.Num = star;
                                    data.crusadeAwards.Add(commonData);
                                }
                            }
                        }
                    }
                    else if (xe.Name == "eliteAwards")
                    {
                        if (data.eliteCrusadeAwards == null)
                        {
                            data.eliteCrusadeAwards = new List<CommonItemData>();
                        }
                        string[] arr = xe.InnerText.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string[] array = arr[i].Split(':');
                            if (array.Length == 2)
                            {
                                int star, dropID;
                                if (int.TryParse(array[0], out star) && int.TryParse(array[1], out dropID))
                                {
                                    CommonItemData commonData = new CommonItemData();
                                    commonData.ID = (uint)dropID;
                                    commonData.Num = star;
                                    data.eliteCrusadeAwards.Add(commonData);
                                }
                            }
                        }
                    }
                    else if (xe.Name == "escortAwards")
                    {
                        if (data.escortAwards == null)
                        {
                            data.escortAwards = new List<CommonItemData>();
                        }
                        string[] arr = xe.InnerText.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string[] array = arr[i].Split(':');
                            if (array.Length == 2)
                            {
                                int star, dropID;
                                if (int.TryParse(array[0], out star) && int.TryParse(array[1], out dropID))
                                {
                                    CommonItemData commonData = new CommonItemData();
                                    commonData.ID = (uint)dropID;
                                    commonData.Num = star;
                                    data.escortAwards.Add(commonData);
                                }
                            }
                        }
                    }
                    else if (xe.Name == "chapterType")
                    {
                        if (!int.TryParse(xe.InnerText, out  data.chapterType))  //不能保证数据填错的可能
                        {
                            Debug.LogError("Error chapterType :" + xe.InnerText);
                        }
                    }
                    else if (xe.Name == "openTime")
                    {
                        if (data.openTime == null)
                            data.openTime = new List<ChapterOpenTime>();
                        string[] arr = xe.InnerText.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string[] time = arr[i].Split(':');
                            if (time.Length == 3)
                            {
                                int week, start, end;
                                if (int.TryParse(time[0], out week) && int.TryParse(time[1], out start) && int.TryParse(time[2], out end))
                                {
                                    ChapterOpenTime open = new ChapterOpenTime(week, start, end);
                                    data.openTime.Add(open);
                                }
                            }
                        }
                    }
                    else if (xe.Name == "challengTimesLimit")
                    {
                        if (!int.TryParse(xe.InnerText, out data.Challenge))
                        {
                            Debug.LogError("Error Data!!!");
                        }
                    }
                    else if (xe.Name == "heroLevelLimit")
                    {
                        data.LVLimit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "pointPos")
                    {
                        if (data.pointPos == null)
                            data.pointPos = new List<Vector2>();
                        string[] arr = xe.InnerText.Split(';');
                        for (int i = 0; i < arr.Length; i++)
                        {
                            string[] pos = arr[i].Split(':');
                            if (pos.Length == 2)
                            {
                                int x, y;
                                if (int.TryParse(pos[0], out x) && int.TryParse(pos[1], out y))
                                {
                                    Vector2 vector = new Vector2();
                                    vector.Set(x, y);
                                    data.pointPos.Add(vector);
                                }
                            }
                        }
                    }
                }
                _chapterList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError("Load Chapter XML Error:" + ex.Message);
        }
    }
}