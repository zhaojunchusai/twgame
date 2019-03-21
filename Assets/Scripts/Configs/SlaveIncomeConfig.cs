using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class SlaveIncomeInfo
{
    public int level;
    public int money;
}
public class SlaveIncomeConfig : BaseConfig
{
    private List<SlaveIncomeInfo> _slaveIncomeList;

    public SlaveIncomeConfig()
    {
        _slaveIncomeList = new List<SlaveIncomeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SLAVEINCOME, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }
    public SlaveIncomeInfo FindAllByLv(int lv)
    {
        return _slaveIncomeList.Find((SlaveIncomeInfo info) => { if (info == null)return false; return info.level == lv; });
    }
    public void Uninitialize()
    {
        if (_slaveIncomeList != null)
        {
            _slaveIncomeList.Clear();
            _slaveIncomeList = null;
        }
    }

    public List<SlaveIncomeInfo> GetSkillEffectList()
    {
        return _slaveIncomeList;
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
                SlaveIncomeInfo data = new SlaveIncomeInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "level")
                    {
                        data.level = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "money")
                    {
                        data.money = int.Parse(xe.InnerText);
                    }
                }
                _slaveIncomeList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load skillEffect XML Error:" + ex.Message);
        }
    }
}