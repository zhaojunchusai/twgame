using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public enum EquipCoordEnum
{
    None = 0,
    HP = 1,
    Attack = 2,
    /// <summary>
    /// 命中
    /// </summary>
    Hit = 3,
    /// <summary>
    /// 闪避
    /// </summary>
    Dodge = 4,
    /// <summary>
    /// 暴击
    /// </summary>
    Crit = 5,
    /// <summary>
    /// 抗暴
    /// </summary>
    Uprising = 6,
}

public class EquipCoordAttribute
{
    public int condition;
    public List<EquipCoordDetailAttribute> list;

    public EquipCoordAttribute()
    {
        condition = 0;
        list = new List<EquipCoordDetailAttribute>();
    }
}

public class EquipCoordDetailAttribute
{
    /// <summary>
    /// 类型：1-生命；2-攻击；3-命中；4-闪避；5-暴击；6抗暴
    /// </summary>
    public EquipCoordEnum type;

    public int att;
}


public class EquipCoordinatesInfo
{
    public uint coordID;

    public string name;

    public List<EquipCoordAttribute> attributes;
}

public class EquipCoordinatesConfig : BaseConfig
{
    private List<EquipCoordinatesInfo> mCoordinatesList;

    public EquipCoordinatesConfig()
    {
        mCoordinatesList = new List<EquipCoordinatesInfo>();
        Initialize(GlobalConst.Config.DIR_XML_EQUIPCOORDINATE, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    private void ParseConfig(string xmlstr)
    {
        //try
        //{
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(xmlstr);
        XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
        foreach (XmlElement Element in nodelist)
        {
            EquipCoordinatesInfo data = new EquipCoordinatesInfo();
            data.attributes = new List<EquipCoordAttribute>();
            EquipCoordAttribute att1 = new EquipCoordAttribute();
            EquipCoordAttribute att2 = new EquipCoordAttribute();
            EquipCoordAttribute att3 = new EquipCoordAttribute();
            EquipCoordAttribute att4 = new EquipCoordAttribute();
            foreach (XmlElement xe in Element)
            {
                if (xe.Name == "CoordID")
                {
                    data.coordID = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "name")
                {
                    data.name = xe.InnerText;
                }
                else
                {
                    if (xe.Name == "condition1")
                    {
                        att1.condition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "attr1")
                    {
                        if (xe.InnerText != "0")
                        {
                            string[] tmp_arr = xe.InnerText.Split(';');
                            if (tmp_arr != null && tmp_arr.Length > 0)
                            {
                                for (int i = 0; i < tmp_arr.Length; i++)
                                {
                                    string str = tmp_arr[i];
                                    if (string.IsNullOrEmpty(str))
                                        continue;
                                    string[] attArray = str.Split(':');
                                    if (attArray != null && attArray.Length >= 2)
                                    {
                                        int type = int.Parse(attArray[0]);
                                        EquipCoordDetailAttribute att = new EquipCoordDetailAttribute();
                                        att.type = (EquipCoordEnum)type;
                                        att.att = int.Parse(attArray[1]);
                                        att1.list.Add(att);
                                    }
                                }
                            }
                        }
                    }
                    else if (xe.Name == "condition2")
                    {
                        att2.condition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "attr2")
                    {
                        if (xe.InnerText != "0")
                        {
                            string[] tmp_arr = xe.InnerText.Split(';');
                            if (tmp_arr != null && tmp_arr.Length > 0)
                            {
                                for (int i = 0; i < tmp_arr.Length; i++)
                                {
                                    string str = tmp_arr[i];
                                    if (string.IsNullOrEmpty(str))
                                        continue;
                                    string[] attArray = str.Split(':');
                                    if (attArray != null && attArray.Length >= 2)
                                    {
                                        int type = int.Parse(attArray[0]);
                                        EquipCoordDetailAttribute att = new EquipCoordDetailAttribute();
                                        att.type = (EquipCoordEnum)type;
                                        att.att = int.Parse(attArray[1]);
                                        att2.list.Add(att);
                                    }
                                }
                            }
                        }
                    }
                    else if (xe.Name == "condition3")
                    {
                        att3.condition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "attr3")
                    {
                        if (xe.InnerText != "0")
                        {
                            string[] tmp_arr = xe.InnerText.Split(';');
                            if (tmp_arr != null && tmp_arr.Length > 0)
                            {
                                for (int i = 0; i < tmp_arr.Length; i++)
                                {
                                    string str = tmp_arr[i];
                                    if (string.IsNullOrEmpty(str))
                                        continue;
                                    string[] attArray = str.Split(':');
                                    if (attArray != null && attArray.Length >= 2)
                                    {
                                        int type = int.Parse(attArray[0]);
                                        EquipCoordDetailAttribute att = new EquipCoordDetailAttribute();
                                        att.type = (EquipCoordEnum)type;
                                        att.att = int.Parse(attArray[1]);
                                        att3.list.Add(att);
                                    }
                                }
                            }
                        }
                    }
                    else if (xe.Name == "condition4")
                    {
                        att4.condition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "attr4")
                    {
                        if (xe.InnerText != "0")
                        {
                            string[] tmp_arr = xe.InnerText.Split(';');
                            if (tmp_arr != null && tmp_arr.Length > 0)
                            {
                                for (int i = 0; i < tmp_arr.Length; i++)
                                {
                                    string str = tmp_arr[i];
                                    if (string.IsNullOrEmpty(str))
                                        continue;
                                    string[] attArray = str.Split(':');
                                    if (attArray != null && attArray.Length >= 2)
                                    {
                                        int type = int.Parse(attArray[0]);
                                        EquipCoordDetailAttribute att = new EquipCoordDetailAttribute();
                                        att.type = (EquipCoordEnum)type;
                                        att.att = int.Parse(attArray[1]);
                                        att4.list.Add(att);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            data.attributes.Add(att1);
            data.attributes.Add(att2);
            data.attributes.Add(att3);
            data.attributes.Add(att4);
            data.attributes.Sort((left, right) =>
            {
                if (left == null || right == null)
                    return 0;
                if (left.condition < right.condition)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            });
            mCoordinatesList.Add(data);
        }
        base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError("Load EquipCoordinatesInfo XML Error:" + ex.Message);
        //    base.LoadCompleted();
        //}
    }

    public EquipCoordinatesInfo GetEquipCoordinatesInfoByID(uint id)
    {
        EquipCoordinatesInfo info = mCoordinatesList.Find((tmp) =>
        {
            if (tmp == null) return false;
            return tmp.coordID.Equals(id);
        });
        if (info == null)
            Debug.LogError("con not get EquipCoordinatesInfo by id:" + id.ToString());
        return info;
    }

}
