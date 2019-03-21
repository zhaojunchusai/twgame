using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class BuffConsumptionInfo
{
    public int CoinType;
    public int Price;
    public int Num;

    public BuffConsumptionInfo(int vCoinType, int vPrice, int vNum)
    {
        CoinType = vCoinType;
        Price = vPrice;
        Num = vNum;
    }

}

public class ClearCDCost
{
    public int coinType;
    public int price;

    public ClearCDCost() { }
    public ClearCDCost(int _coinType,int _price)
    {
        coinType = _coinType;
        price = _price;
    }
}
/// <summary>
/// 全服霸主信息
/// </summary>
public class ServerHegemonyInfo
{
    /// <summary>
    /// Boss开始时间
    /// </summary>
    //public string boss_time;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int cont_time;
    /// <summary>
    /// 挑战CD时间
    /// </summary>
    public int challenge_cd;
    /// <summary>
    /// 清除CD花费
    /// </summary>
    public ClearCDCost clear_cd_diamond;
    /// <summary>
    /// buff花费
    /// </summary>
    public List<BuffConsumptionInfo> clear_buff_diamond;
    /// <summary>
    /// 加成率
    /// </summary>
    public int buff_effect_rate;
    /// <summary>
    /// 朝拜刷新时间
    /// </summary>
    public string worship_Refresh_time;
    /// <summary>
    /// 挑战规则
    /// </summary>
    public string hegemony_rule_content;
    /// <summary>
    /// 排名奖励
    /// </summary>
    //public List<uint> hegemony_award;
    public Dictionary<int, uint> hegemony_award;
    /// <summary>
    /// 参与奖
    /// </summary>
    public uint min_award;
    /// <summary>
    /// 朝拜奖励
    /// </summary>
    public int worship_award;
    /// <summary>
    /// 贡品奖励
    /// </summary>
    public int tribute_award;
    /// <summary>
    /// 霸主最大血量百分比
    /// </summary>
    public int max_HP;
    /// <summary>
    /// 霸主最小血量百分比
    /// </summary>
    public int min_HP;
    /// <summary>
    /// 等待时间
    /// </summary>
    public int wait_time;
    /// <summary>
    /// 战斗时间
    /// </summary>
    public int fight_time;
    /// <summary>
    /// 初始魔法值
    /// </summary>
    public int init_magic;
    /// <summary>
    /// 战斗背景图
    /// </summary>
    public string fight_backpic;
    /// <summary>
    /// 战斗速度
    /// </summary>
    public int fight_speed;
    /// <summary>
    /// 朝拜奖励描述
    /// </summary>
    public string worship_award_desc;
    /// <summary>
    /// 贡品奖励描述
    /// </summary>
    public string tribute_award_desc;
}

public class ServerHegemonyConfig : BaseConfig
{
    private List<ServerHegemonyInfo> _serverHegemonyList;
    private List<ArenaAwardInfo> _selfRankAwardList;
    private List<ArenaAwardInfo> _unionRankAwardList;

    public ServerHegemonyConfig()
    {
        _serverHegemonyList = new List<ServerHegemonyInfo>();
        _selfRankAwardList = new List<ArenaAwardInfo>();
        _unionRankAwardList = new List<ArenaAwardInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SERVERHEGEMONY, ParseConfig);
        Initialize(GlobalConst.Config.DIR_XML_SERVERHEGEMONYSELFAWARD, ParseSelfAwardConfig);
        Initialize(GlobalConst.Config.DIR_XML_SERVERHEGEMONYUNIONAWARD, ParseUnionAwardConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_serverHegemonyList != null)
        {
            _serverHegemonyList.Clear();
            _serverHegemonyList = null;
        }
        if (_selfRankAwardList != null)
        {
            _selfRankAwardList.Clear();
            _selfRankAwardList = null;
        }
        if (_unionRankAwardList != null)
        {
            _unionRankAwardList.Clear();
            _unionRankAwardList = null;
        }
    }

    public ServerHegemonyInfo GetServerHegemonyInfo()
    {
        if (_serverHegemonyList != null)
        {
            if (_serverHegemonyList.Count > 0)
            {
                return _serverHegemonyList[0];
            }
        }
        return null;
    }

    public List<ArenaAwardInfo> GetSelfRankAwardList()
    {
        return _selfRankAwardList;
    }
    public List<ArenaAwardInfo> GetUnionRankAwardList()
    {
        return _unionRankAwardList;
    }

    public ArenaAwardInfo GetSelfRankAwardInfoByRank(int rank)
    {
        foreach (ArenaAwardInfo item in _selfRankAwardList)
        {
            if (rank >= item.min_rank && (rank <= item.max_rank || item.max_rank == -1))//-1视为无穷大， 即N名以后的奖励均一样
            {
                return item;
            }
        }
        Debug.LogError("GetDropIDBySelfDamageRank ERROR :rank =" + rank);
        return new ArenaAwardInfo();
    }
    public ArenaAwardInfo GetUnionRankAwardInfoByRank(int rank)
    {
        foreach (ArenaAwardInfo item in _unionRankAwardList)
        {
            if (rank >= item.min_rank && (rank <= item.max_rank || item.max_rank == -1))//-1视为无穷大， 即N名以后的奖励均一样
            {
                return item;
            }
        }
        Debug.LogError("GetDropIDByUnionDamageRank ERROR :rank =" + rank);
        return new ArenaAwardInfo();
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
                ServerHegemonyInfo data = new ServerHegemonyInfo();
                foreach (XmlElement xe in Element)
                {
                    //if (xe.Name == "boss_time")
                    //{
                    //    data.boss_time = xe.InnerText;
                    //}
                    if (xe.Name == "cont_time")
                    {
                        data.cont_time = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "challenge_cd")
                    {
                        data.challenge_cd = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "clear_cd_diamond")
                    {
                        string[] costInfo = xe.InnerText.Split(':');
                        if (costInfo.Length == 2)
                        {
                            data.clear_cd_diamond =new ClearCDCost(int.Parse(costInfo[0]), int.Parse(costInfo[1]));
                        }
                       // Debug.LogError(data.clear_cd_diamond.coinType);
                    }
                    if (xe.Name == "clear_buff_diamond")
                    {
                        //Debug.LogError(string.Format("clear_buff_diamond: {0}", xe.InnerText));
                        //data.clear_buff_diamond = int.Parse(xe.InnerText);
                        data.clear_buff_diamond = new List<BuffConsumptionInfo>();
                        string[] tmpArrInit = xe.InnerText.Split(';');
                        if (tmpArrInit.Length > 0)
                        {
                            for (int i = 0; i < tmpArrInit.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpArrInit[i]))
                                {
                                    string[] tmpArr = tmpArrInit[i].Split(':');
                                    if (tmpArr.Length == 3)
                                    {
                                        data.clear_buff_diamond.Add(new BuffConsumptionInfo(int.Parse(tmpArr[0]), int.Parse(tmpArr[1]), int.Parse(tmpArr[2])));
                                    }
                                }
                            }
                        
                        }
                    }
                    if (xe.Name == "buff_effect_rate")
                    {
                        data.buff_effect_rate = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "worship_Refresh_time")
                    {
                        data.worship_Refresh_time = xe.InnerText;
                    }
                    if (xe.Name == "hegemony_rule_content")
                    {
                        data.hegemony_rule_content = xe.InnerText;
                    }
                    if (xe.Name == "hegemony_award")
                    {
                        data.hegemony_award = new Dictionary<int, uint>();
                        string[] tmpArr = xe.InnerText.Split(';');
                        if (tmpArr.Length > 0)
                        {
                            for (int i = 0; i < tmpArr.Length; i++)
                            {
                                if (!string.IsNullOrEmpty(tmpArr[i]))
                                {
                                    string[] tmpArrSingle = tmpArr[i].Split(':');
                                    if (tmpArrSingle.Length >= 2)
                                    {
                                        data.hegemony_award.Add(int.Parse(tmpArrSingle[0]), uint.Parse(tmpArrSingle[1]));
                                    }
                                }
                            }
                        }
                    }
                    if (xe.Name == "min_award")
                    {
                        data.min_award = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "worship_award")
                    {
                        data.worship_award = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "tribute_award")
                    {
                        data.tribute_award = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "max_HP")
                    {
                        data.max_HP = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "min_HP")
                    {
                        data.min_HP = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "wait_time")
                    {
                        data.wait_time = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "fight_time")
                    {
                        data.fight_time = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "init_magic")
                    {
                        data.init_magic = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "fight_backpic")
                    {
                        data.fight_backpic = xe.InnerText;
                    }
                    if (xe.Name == "fight_speed")
                    {
                        data.fight_speed = int.Parse(xe.InnerText);
                    }
                    if(xe.Name== "worship_award_desc")
                    {
                        data.worship_award_desc = xe.InnerText;
                    }
                    if (xe.Name == "tribute_award_desc")
                    {
                        data.tribute_award_desc = xe.InnerText;
                    }
            }
                _serverHegemonyList.Add(data);
            }

            base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError("Load ServerHegemony XML Error:" + ex.Message + ex.StackTrace);
        //}
    }
    private void ParseSelfAwardConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                ArenaAwardInfo data = new ArenaAwardInfo();
                data.id = int.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.min_rank = int.Parse(CommonFunction.GetXmlElementStr(Element, "min_rank"));
                data.max_rank = int.Parse(CommonFunction.GetXmlElementStr(Element, "max_rank"));
                data.drop_id = int.Parse(CommonFunction.GetXmlElementStr(Element, "drop_id"));
                _selfRankAwardList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
    private void ParseUnionAwardConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                ArenaAwardInfo data = new ArenaAwardInfo();
                data.id = int.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.min_rank = int.Parse(CommonFunction.GetXmlElementStr(Element, "min_rank"));
                data.max_rank = int.Parse(CommonFunction.GetXmlElementStr(Element, "max_rank"));
                data.drop_id = int.Parse(CommonFunction.GetXmlElementStr(Element, "drop_id"));
                _unionRankAwardList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }
}