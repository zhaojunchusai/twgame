using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class QualifyingRankData
{
    public uint id;

    public uint preID;

    public uint nextID;

    /// <summary>
    /// 段位名
    /// </summary>
    public string divisionName;
    /// <summary>
    /// 最低积分要求
    /// </summary>
    public int point_min;
    /// <summary>
    /// 最低积分要求 (约定填-1时 表示无穷大)
    /// </summary>
    public int point_max;
    /// <summary>
    /// 赛季奖励掉落ID
    /// </summary>
    public uint dropID;
    /// <summary>
    /// 段位ICON
    /// </summary>
    public string icon;
    /// <summary>
    /// 头像边框
    /// </summary>
    public string frame;

    public string divisionup_effect;
}

public class QualifyingRankConfig : BaseConfig
{
    private List<QualifyingRankData> _rankList = new List<QualifyingRankData>();
    public QualifyingRankConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_QUALIFYING_RANK, ParseConfig);
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
                QualifyingRankData data = new QualifyingRankData();
                data.id = uint.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.preID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "preid"));
                data.nextID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "nextid"));
                data.divisionName = CommonFunction.GetXmlElementStr(Element, "name");
                data.dropID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "reward"));
                data.icon = CommonFunction.GetXmlElementStr(Element, "icon");
                data.point_min = int.Parse(CommonFunction.GetXmlElementStr(Element, "point_min"));
                data.point_max = int.Parse(CommonFunction.GetXmlElementStr(Element, "point_max"));
                data.frame = CommonFunction.GetXmlElementStr(Element, "frame");
                data.divisionup_effect = CommonFunction.GetXmlElementStr(Element, "divisionup_effect");
                if (data.divisionup_effect == "0")
                {
                    data.divisionup_effect = string.Empty;
                }
                _rankList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError(ex.Message);
        }
    }

    public List<QualifyingRankData> GetRankDataList()
    {
        return _rankList;
    }

    public QualifyingRankData GetRankDataByPoint(int point)
    {
        QualifyingRankData data = null;
        if (_rankList != null && _rankList.Count > 0)
        {
            for (int i = 0; i < _rankList.Count; i++)
            {
                QualifyingRankData tmp = _rankList[i];
                if (tmp == null)
                    continue;
                if (tmp.point_max == -1)
                {
                    if (point >= tmp.point_min)
                    {
                        data = tmp;
                        break;
                    }
                }
                else
                {
                    if (tmp.point_min <= point && point <= tmp.point_max)
                    {
                        data = tmp;
                        break;
                    }
                }
            }
        }
        if (data == null)
        {
            Debug.LogError("can not get QualifyingRankData by point:" + point.ToString());
        }
        return data;
    }


    public QualifyingRankData GetRankDataByID(uint id)
    {
        QualifyingRankData data = _rankList.Find((tmp) =>
          {
              if (tmp == null)
                  return false;
              return tmp.id == id;
          });
        if (data == null)
            Debug.LogError("Can not get QualifyingRankData by id:" + id.ToString());
        return data;
    }

    public void Uninitialize()
    {
        if (_rankList != null)
        {
            _rankList.Clear();
            _rankList = null;
        }
    }
}
