using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class AchievementItemData {
    public int id;
    public string name;
    public int type;
    public int final_course;
    public int award_type;
    public int award_id;
    public string desc;
    public string icon;

    public AchievementItemData()
    {
        id = 0;
        name = "";
        type = 0;
        final_course = 0;
        award_type = 0;
        award_id = 0;
        desc = "";
        icon = "";
    }

    public void Copy(AchievementItemData data)
    {
        id = data.id;
        name = data.name;
        type = data.type;
        final_course = data.final_course;
        award_id = data.award_id;
        award_type = data.award_type;
        desc = data.desc;
        icon = data.icon;
    }

}

public class AchievementConfig : BaseConfig{

    private Dictionary<int, AchievementItemData> _achievementItemData = new Dictionary<int, AchievementItemData>();

    public AchievementConfig()
    {
        _achievementItemData.Clear();
        Initialize(GlobalConst.Config.DIR_XML_ACHIEVEMENT, ParseConfig);
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
                AchievementItemData data = new AchievementItemData();
                data.id = int.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.name = CommonFunction.GetXmlElementStr(Element, "name");
                data.type = int.Parse(CommonFunction.GetXmlElementStr(Element, "type"));
                data.final_course = int.Parse(CommonFunction.GetXmlElementStr(Element, "final_course"));
                data.award_type = int.Parse(CommonFunction.GetXmlElementStr(Element, "award_type"));
                data.award_id = int.Parse(CommonFunction.GetXmlElementStr(Element, "award_id"));
                data.desc = CommonFunction.GetXmlElementStr(Element, "desc");
                data.icon = CommonFunction.GetXmlElementStr(Element, "icon");
                _achievementItemData.Add(data.id, data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public AchievementItemData FindDataByID(int id)
    {
        if (_achievementItemData.ContainsKey(id))
        {
            return _achievementItemData[id];
        }
        else return null;
    }

    public string GetNameByID(int id)
    {
        if (_achievementItemData.ContainsKey(id))
        {
            return _achievementItemData[id].name;
        }
        else return null;
    }
}
