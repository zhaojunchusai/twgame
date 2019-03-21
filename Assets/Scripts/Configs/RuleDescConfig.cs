using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class RuleDescData
{
    /// <summary>
    /// 排位赛规则说明
    /// </summary>
    public string qualifying_rule;
    /// <summary>
    /// 演武台规则说明
    /// </summary>
    public string pvp_rule;

    public string preyLifeSoul_rule;
}

public class RuleDescConfig : BaseConfig
{
    private List<RuleDescData> _ruleList = new List<RuleDescData>();
    public RuleDescConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_RULEDESC, ParseConfig);
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
                RuleDescData data = new RuleDescData();
                data.qualifying_rule = CommonFunction.GetXmlElementStr(Element, "Qualifying_rule");
                data.pvp_rule = CommonFunction.GetXmlElementStr(Element, "Pvp_rule");
                data.preyLifeSoul_rule = CommonFunction.GetXmlElementStr(Element, "preyLifeSoul_rule");
                _ruleList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public RuleDescData GetRuleDescData()
    {
        if (_ruleList == null || _ruleList.Count < 1)
        {
            return null;
        }
        return _ruleList[0];
    }
}
