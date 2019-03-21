using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class CrossServerWarSettingData
{
    public uint season_round;
    public string rule;
    public float retain_gain;
    public string exchange_point;
    public string exchange_gold;
    public uint rest_time;
    public string rank_update_time;
}

public class CrossServerWarTimeData
{
    public uint round_id;
    public uint day_of_week;
    public string start_time;
    public uint const_time;
}

public class CrossServerTerritoryData
{
    public uint territory_id;
    public string name;
    public uint column_num;
    public uint row_num;
    public int territory_type;
    public uint goods_num;
    public uint special_production;
    public uint stage_id;
    public string desc;
}

public class CrossServerBattleData
{
    public int goods_floor;
    public int goods_ceil;
    public uint goods_award;
}

public class CrossServerSeasonData
{
    public int rank_floor;
    public int rank_ceil;
    public uint rank_award;
}

public class CrossServerMilitaryRankData
{
    public uint military_rank;
    public int point_low;
    public int point_high;
}

public class CrossServerWarConfig : BaseConfig {

    private CrossServerWarSettingData war_data;
    private Dictionary<uint, CrossServerWarTimeData> time_data;
    private List<CrossServerTerritoryData> territory_data;
    private List<CrossServerBattleData> battle_data;
    private List<CrossServerSeasonData> season_data;
    private List<CrossServerMilitaryRankData> militaryRank_data;

    public CrossServerWarConfig()
    {
        war_data = new CrossServerWarSettingData();
        time_data = new Dictionary<uint, CrossServerWarTimeData>();
        territory_data = new List<CrossServerTerritoryData>();
        battle_data = new List<CrossServerBattleData>();
        season_data = new List<CrossServerSeasonData>();
        militaryRank_data = new List<CrossServerMilitaryRankData>();

        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERWAR, ParseSettingConfig);
        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERTIME, ParseTimeConfig);
        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERTERRITORY, ParseTerritoryConfig);
        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERBATTLE, ParseBattleConfig);
        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERSEASON, ParseSeasonConfig);
        Initialize(GlobalConst.Config.DIR_XML_CROSSSERVERMILITARYRANK, ParseMilitaryRankConfig);

    }

    public override void Initialize(string vFileName, Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void ParseSettingConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                war_data.season_round = uint.Parse(CommonFunction.GetXmlElementStr(Element,"season_round"));
                war_data.rule = CommonFunction.GetXmlElementStr(Element, "rule");
                war_data.retain_gain = float.Parse(CommonFunction.GetXmlElementStr(Element, "retain_gain"));
                war_data.exchange_point = CommonFunction.GetXmlElementStr(Element, "exchange_point");
                war_data.exchange_gold = CommonFunction.GetXmlElementStr(Element, "exchange_gold");
                war_data.rest_time = uint.Parse(CommonFunction.GetXmlElementStr(Element, "rest_time"));
                war_data.rank_update_time = CommonFunction.GetXmlElementStr(Element, "rank_update_time");
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    public CrossServerWarSettingData GetCrossServerWarSettingData()
    {
        if (war_data == null)
            Debug.LogError("war_data is null");
        return war_data;
    }

    public float ExchangePointScale()
    {
        string[] strs = war_data.exchange_point.Split(':');
        if (strs.Length != 2 || int.Parse(strs[0]) == 0)
        {
            Debug.LogError("damage exchange to point scale is wrong");
            return 0;
        }
        return float.Parse(strs[1]) / float.Parse(strs[0]);
    }

    public void ParseTimeConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CrossServerWarTimeData _data = new CrossServerWarTimeData();
                _data.round_id = uint.Parse(CommonFunction.GetXmlElementStr(Element, "round_id"));
                _data.day_of_week = uint.Parse(CommonFunction.GetXmlElementStr(Element, "day_of_week"));
                _data.start_time = CommonFunction.GetXmlElementStr(Element, "start_time");
                _data.const_time = uint.Parse(CommonFunction.GetXmlElementStr(Element, "const_time"));

                time_data.Add(_data.round_id, _data);
            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
        
    }

    public CrossServerWarTimeData GetCrossServerWarTimeData(uint key)
    {
        if (!time_data.ContainsKey(key))
            Debug.LogError("can't find the right data");
        return time_data[key];
    }

    public void ParseTerritoryConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CrossServerTerritoryData _data = new CrossServerTerritoryData();
                _data.territory_id = uint.Parse(CommonFunction.GetXmlElementStr(Element, "territory_id"));
                _data.name = CommonFunction.GetXmlElementStr(Element, "name");
                _data.column_num = uint.Parse(CommonFunction.GetXmlElementStr(Element, "column_num"));
                _data.row_num = uint.Parse(CommonFunction.GetXmlElementStr(Element, "row_num"));
                _data.territory_type = int.Parse(CommonFunction.GetXmlElementStr(Element, "territory_type"));
                _data.goods_num = uint.Parse(CommonFunction.GetXmlElementStr(Element, "goods_num"));
                _data.special_production = uint.Parse(CommonFunction.GetXmlElementStr(Element, "special_production"));
                _data.stage_id = uint.Parse(CommonFunction.GetXmlElementStr(Element, "stage_id"));
                _data.desc = CommonFunction.GetXmlElementStr(Element, "desc");

                territory_data.Add(_data);
            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    public List<CrossServerTerritoryData> GetAllTileInfo()
    {
        if (territory_data == null || territory_data.Count < 1)
            Debug.LogError("there's no data about territory");
        return territory_data;
    }

    public uint GetTileStageID(uint tileID)
    {
        if (territory_data == null || territory_data.Count < 1)
            Debug.LogError("there's no data about territory");
        CrossServerTerritoryData data = territory_data.Find(s => s.territory_id == tileID);
        if (data == null)
            Debug.LogError("can't find the tile with id: " + tileID);
        return data.stage_id;
    }

    public void ParseBattleConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CrossServerBattleData _data = new CrossServerBattleData();
                _data.goods_floor = int.Parse(CommonFunction.GetXmlElementStr(Element, "goods_floor"));
                _data.goods_ceil = int.Parse(CommonFunction.GetXmlElementStr(Element, "goods_ceil"));
                _data.goods_award = uint.Parse(CommonFunction.GetXmlElementStr(Element, "goods_award"));

                battle_data.Add(_data);
            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    public List<CrossServerBattleData> GetCrossServerWarBattleDataList()
    {
        if (battle_data == null || battle_data.Count < 0)
            Debug.LogError("the list is null or empty");
        return battle_data;
    }

    public void ParseSeasonConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CrossServerSeasonData _data = new CrossServerSeasonData();
                _data.rank_floor = int.Parse(CommonFunction.GetXmlElementStr(Element, "rank_floor"));
                _data.rank_ceil = int.Parse(CommonFunction.GetXmlElementStr(Element, "rank_ceil"));
                _data.rank_award = uint.Parse(CommonFunction.GetXmlElementStr(Element, "rank_award"));

                season_data.Add(_data);
            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    public List<CrossServerSeasonData> GetCrossServerWarSeasonDataList()
    {
        if (season_data == null || season_data.Count < 0)
            Debug.LogError("the list is null or empty");
        return season_data;
    }

    public void ParseMilitaryRankConfig(string bytes)
    {
        try
        {
            string xmlstr = bytes;
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CrossServerMilitaryRankData _data = new CrossServerMilitaryRankData();
                _data.military_rank = uint.Parse(CommonFunction.GetXmlElementStr(Element, "military_rank"));
                _data.point_low = int.Parse(CommonFunction.GetXmlElementStr(Element, "point_low"));
                _data.point_high = int.Parse(CommonFunction.GetXmlElementStr(Element, "point_high"));

                militaryRank_data.Add(_data);
            }
            base.LoadCompleted();

        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }

    }

    public uint GetMilitaryRank(uint point)
    {
        if (militaryRank_data == null || militaryRank_data.Count < 1)
        {
            Debug.LogError("military rank config data is null");
        }
        uint rank = 0;
        foreach(var limit in militaryRank_data)
        {
            if (point >= limit.point_low && (point <= limit.point_high || limit.point_high == -1))
            {
                rank = limit.military_rank;
            }
        }
        return rank;
    }
}
