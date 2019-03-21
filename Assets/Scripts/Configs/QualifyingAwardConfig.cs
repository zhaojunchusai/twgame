using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class QualifyingAwardData
{
    /// <summary>
    /// 赛季持续时间
    /// </summary>
    public uint ladder_duration;
    /// <summary>
    /// 每日奖励最低战斗场次
    /// </summary>
    public uint day_require_low;
    /// <summary>
    /// 每日奖励中等战斗场次
    /// </summary>
    public uint day_require_mid;
    /// <summary>
    /// 每日奖励最高战斗场次
    /// </summary>
    public uint day_require_high;
    /// <summary>
    /// 每日奖励最低战斗奖励
    /// </summary>
    public uint day_reward_low;
    /// <summary>
    /// 每日奖励中等战斗奖励
    /// </summary>
    public uint day_reward_mid;
    /// <summary>
    /// 每日奖励最高战斗奖励
    /// </summary>
    public uint day_reward_high;
    /// <summary>
    /// 每周奖励要求最低战斗胜场场次
    /// </summary>
    public uint week_require_low;
    /// <summary>
    /// 每周奖励要求中等战斗胜场场次
    /// </summary>
    public uint week_require_mid;
    /// <summary>
    /// 每周奖励要求最高战斗胜场场次
    /// </summary>
    public uint week_require_high;
    /// <summary>
    /// 每周奖励最低战斗胜场奖励
    /// </summary>
    public uint week_reward_low;
    /// <summary>
    /// 每周奖励中等战斗胜场奖励
    /// </summary>
    public uint week_reward_mid;
    /// <summary>
    /// 每周奖励最高战斗胜场奖励
    /// </summary>
    public uint week_reward_high;
    /// <summary>
    /// 战斗时间
    /// </summary>
    public int limittime;
    /// <summary>
    /// 初始魔法
    /// </summary>
    public int initmagic;
    /// <summary>
    /// 背景图
    /// </summary>
    public string background;

    public string ruledesc;

    public int challenge_time_limit;

    public int revenge_time_limit;
    /// <summary>
    /// 消耗总次数上限
    /// </summary>
    public int total_revenge_limit;
    /// <summary>
    /// 消耗总次数货币类型
    /// </summary>
    public int total_revenge_consumetype;
    /// <summary>
    /// 消耗总次数货币数量
    /// </summary>
    public int total_revenge_comsumenum;

    public uint clear_cdtime;
    /// <summary>
    /// 清除CD消耗类型
    /// </summary>
    public int clearcd_consumeType;
    /// <summary>
    /// 清除CD消耗数量
    /// </summary>
    public int clearcd_consumeNum;
}

public class QualifyingAwardConfig : BaseConfig
{
    private List<QualifyingAwardData> _awardList = new List<QualifyingAwardData>();
    public QualifyingAwardConfig()
    {
        Initialize(GlobalConst.Config.DIR_XML_QUALIFYING_AWARD, ParseConfig);
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
                QualifyingAwardData data = new QualifyingAwardData();
                data.ladder_duration = uint.Parse(CommonFunction.GetXmlElementStr(Element, "ladder_duration"));
                data.day_require_low = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_require_low"));
                data.day_require_mid = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_require_mid"));
                data.day_require_high = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_require_high"));
                data.day_reward_low = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_reward_low"));
                data.day_reward_mid = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_reward_mid"));
                data.day_reward_high = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_reward_high"));
                data.week_require_low = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_require_low"));
                data.week_require_mid = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_require_mid"));
                data.week_require_high = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_require_high"));
                data.week_reward_low = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_reward_low"));
                data.week_reward_mid = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_reward_mid"));
                data.week_reward_high = uint.Parse(CommonFunction.GetXmlElementStr(Element, "week_reward_high"));
                data.limittime = int.Parse(CommonFunction.GetXmlElementStr(Element, "limittime"));
                data.initmagic = int.Parse(CommonFunction.GetXmlElementStr(Element, "initmagic"));
                data.background = CommonFunction.GetXmlElementStr(Element, "background");
                data.ruledesc = CommonFunction.GetXmlElementStr(Element, "ruledesc");
                data.challenge_time_limit = int.Parse(CommonFunction.GetXmlElementStr(Element, "challenge_time_limit"));
                data.revenge_time_limit = int.Parse(CommonFunction.GetXmlElementStr(Element, "revenge_time_limit"));
                data.clear_cdtime = uint.Parse(CommonFunction.GetXmlElementStr(Element, "clearcd_time"));
                string total_revenge = CommonFunction.GetXmlElementStr(Element, "revenge_time_total");
                if (!string.IsNullOrEmpty(total_revenge))
                {
                    string[] array = total_revenge.Split(':');
                    if (array.Length >= 3)
                    {
                        data.total_revenge_limit = int.Parse(array[0]);
                        data.total_revenge_consumetype = int.Parse(array[1]);
                        data.total_revenge_comsumenum = int.Parse(array[2]);
                    }
                }
                string clearcd_consume = CommonFunction.GetXmlElementStr(Element, "clearcd_consume");
                if (!string.IsNullOrEmpty(clearcd_consume) && clearcd_consume != "0")
                {
                    string[] array = clearcd_consume.Split(':');
                    if (array.Length >= 2)
                    {
                        data.clearcd_consumeType = int.Parse(array[0]);
                        data.clearcd_consumeNum = int.Parse(array[1]);
                    }
                }
                _awardList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError(ex.Message);
        }
    }

    public QualifyingAwardData GetQualifyingAwardData()
    {
        if (_awardList == null || _awardList.Count < 1)
        {
            Debug.LogError("can not get QualifyingAwardData");
            return null;
        }
        return _awardList[0];
    }

    public void Uninitialize()
    {
        if (_awardList != null)
        {
            _awardList.Clear();
            _awardList = null;
        }
    }
}
