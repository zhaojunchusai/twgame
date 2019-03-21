using UnityEngine;
using System.Collections.Generic;
using System.Xml;
using System;
using fogs.proto.msg;

public class CaptureCityData 
{
    public uint mID;
    public string mName;
    public string mIcon;
    public Vector3 mPos;
    public bool mIsCapital;
    public NationType mCountry;
    public uint mStageID;
    public uint mAwardDropID;
    public List<uint> mBoxID;
    public List<CommonItemData> mBoxDrops;
    public string mCityAwardDesc;
    public string mBoxAwardTitle;
}

public class CaptureTokenData 
{
    public uint mID;
    public NationType mTokenType;
    public uint mMaxLayerCount;
    /// <summary>
    /// 配置表配置11个数据  从0层开始
    /// </summary>
    public List<float> mDamageIncrease;
    /// <summary>
    /// 配置表配置11个数据  表示升级需要的消耗   eg:0层的时候配置0层升级到1层的消耗
    /// </summary>
    public List<int> mCostDiamond;
    public string mTitleDesc;
    public string mDesc;

    /// <summary>
    /// 获取当前层的伤害提升
    /// </summary>
    /// <param name="level"></param>
    /// <returns></returns>
    public float GetDamageIncreaseByLv(int level)
    {        
        if (level >= 0 && level < mDamageIncrease.Count)
        {
            return mDamageIncrease[level];
        }

        Debug.LogError("ERROR : level out of range, level="+ level);
        return 0;
    }

    /// <summary>
    /// 获取升级到下一层需要的消耗
    /// </summary>
    /// <param name="curlevel"></param>
    /// <returns></returns>
    public int GetCostDiamond(int curlevel)
    {
        if (curlevel >= 0 && curlevel < mCostDiamond.Count)
        {
            return mCostDiamond[curlevel];
        }

        Debug.LogError("ERROR : level out of range, level=" + curlevel);
        return 0;
    }
}

public class CaptureTerritoryConfig :BaseConfig
{
    private Dictionary<uint, CaptureCityData> _campaignCityDatas;
    private Dictionary<NationType, CaptureTokenData> _campaignTokenDatas;
    private List<ArenaAwardInfo> _damageRankAward;

    /// <summary>
    /// 规则 未读表
    /// </summary>
    public string Rule;
    /// <summary>
    /// 立即出战消耗 
    /// </summary>
    public int FightCDCost;
    /// <summary>
    /// 血量转换积分的比例  
    /// </summary>
    public float BloodScoreFactor;
    /// <summary>
    /// 伤害提升需要除以的基数
    /// </summary>
    public int mTokenIncBaseFactor = 1;

    public string RankRefreshTime = "";

    public CaptureTerritoryConfig() 
    {
        _campaignCityDatas = new Dictionary<uint, CaptureCityData>();
        _campaignTokenDatas = new Dictionary<NationType, CaptureTokenData>();
        _damageRankAward = new List<ArenaAwardInfo>();
        Initialize(GlobalConst.Config.DIR_XML_CAMPAIGN, ParseCampaignConfig);
        Initialize(GlobalConst.Config.DIR_XML_CAMPAIGN_CASTLE, ParseCityConfig);
        Initialize(GlobalConst.Config.DIR_XML_CAMPAIGN_TOKEN, ParseTokenConfig);
        Initialize(GlobalConst.Config.DIR_XML_CAMPAIGN_AWARD, ParseAwardConfig);

    }
    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public List<CaptureCityData> GetCityList()
    {
        return new List<CaptureCityData>(_campaignCityDatas.Values);
    }

    public float GetDamageIncreaseByLevel(NationType nation,int level)
    {
        CaptureTokenData data = GetCaptureTokenDataByType(nation);
        return data.GetDamageIncreaseByLv(level);
    }

    public CaptureTokenData GetCaptureTokenDataByType(NationType nation)
    {
        if (_campaignTokenDatas.ContainsKey(nation))
        {
            return _campaignTokenDatas[nation];
        }
        Debug.LogError("ERROR: no such nation");
        return null;
    }

    public List<ArenaAwardInfo> GetArenaAwardList()
    {
        return _damageRankAward;
    }

    public ArenaAwardInfo GetArenaAwardInfoByRank(int rank)
    {
        foreach (ArenaAwardInfo item in _damageRankAward)
        {
            if (rank >= item.min_rank && (rank <= item.max_rank || item.max_rank == -1))//-1视为无穷大， 即N名以后的奖励均一样
            {
                return item;
            }
        }
        Debug.LogError("GetDropIDByPersonDamageRank ERROR :rank =" + rank);
        return new ArenaAwardInfo();
    }

    private List<float> GetDamageIncList(List<int> list)
    {
        List<float> res = new List<float>();

        for (int i = 0; i < list.Count; i++)
        {
            res.Add((float)list[i]/mTokenIncBaseFactor);
        }

        return res;
    }

    private void ParseAwardConfig(string xmlstr)
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
                _damageRankAward.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void ParseCampaignConfig(string xmlstr)
    {
        //try
        //{
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                Rule = CommonFunction.ReplaceEscapeChar(CommonFunction.GetXmlElementStr(Element, "rules"));
                FightCDCost = int.Parse(CommonFunction.GetXmlElementStr(Element, "fight_consume"));
                BloodScoreFactor = GetBloodScoreFactor(CommonFunction.GetXmlElementStr(Element, "score_rate"));
                mTokenIncBaseFactor = int.Parse(CommonFunction.GetXmlElementStr(Element, "damage_increase_base_factor"));
                RankRefreshTime = CommonFunction.GetXmlElementStr(Element, "rank_refresh_time");
            }

            base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError(ex.Message);
        //}
    }

    private void ParseCityConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CaptureCityData data = new CaptureCityData();
                data.mID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.mName = CommonFunction.GetXmlElementStr(Element, "name");
                data.mIcon = CommonFunction.GetXmlElementStr(Element, "icon");
                data.mPos = GetVector3(CommonFunction.GetXmlElementStr(Element, "pos"));
                data.mIsCapital = uint.Parse(CommonFunction.GetXmlElementStr(Element, "is_capital")) == 1;
                data.mCountry = (NationType)uint.Parse(CommonFunction.GetXmlElementStr(Element, "country"));
                data.mStageID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "stage_id"));
                data.mAwardDropID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "award_drop"));
                data.mBoxID = CommonFunction.GetParseStrToUint(CommonFunction.GetXmlElementStr(Element, "box_id"),':');
                data.mBoxDrops = GetItemDataList(data.mBoxID[0]);
                data.mCityAwardDesc = CommonFunction.GetXmlElementStr(Element, "city_award_desc");
                data.mBoxAwardTitle = CommonFunction.GetXmlElementStr(Element, "box_award_title");
                _campaignCityDatas.Add(data.mID, data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private void ParseTokenConfig(string xmlstr)
    {
        //try
        //{
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement Element in nodelist)
            {
                CaptureTokenData data = new CaptureTokenData();
                data.mID = uint.Parse(CommonFunction.GetXmlElementStr(Element, "id"));
                data.mTokenType = (NationType)uint.Parse(CommonFunction.GetXmlElementStr(Element, "country"));
                data.mMaxLayerCount = uint.Parse(CommonFunction.GetXmlElementStr(Element, "max_layer"));
                data.mDamageIncrease = GetDamageIncList(CommonFunction.GetParseStrToInt(CommonFunction.GetXmlElementStr(Element, "damage")));
                data.mCostDiamond = CommonFunction.GetParseStrToInt(CommonFunction.GetXmlElementStr(Element, "diamond"));
                if (data.mDamageIncrease.Count != data.mMaxLayerCount + 1 || data.mCostDiamond.Count != data.mMaxLayerCount + 1)
                {
                    Debug.LogError("ERROR : Capture Token Config Error");
                }
                data.mTitleDesc = CommonFunction.GetXmlElementStr(Element, "title_desc");
                data.mDesc = CommonFunction.GetXmlElementStr(Element, "desc");
                if (!_campaignTokenDatas.ContainsKey(data.mTokenType))
                    _campaignTokenDatas.Add(data.mTokenType, data);
                else
                    Debug.LogError("ERROR :same nation!");
            }

            base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    Debug.LogError(ex.Message);
        //}
    }

    private List<CommonItemData> GetItemDataList(uint boxid)
    {
        List<CommonItemData> list = new List<CommonItemData>();
        ItemInfo item = ConfigManager.Instance.mItemData.GetItemInfoByID(boxid);
        for (int i = 0; i < item.proplist.Count; i++)
        {
            CommonItemData tmpData = new CommonItemData(item.proplist[i].id, item.proplist[i].count, true);
            list.Add(tmpData);
        }        
        return list;
    }

    private List<CommonItemData> GetItemDataList(string str)
    {
        List<CommonItemData> list = new List<CommonItemData>();
        List<string> strlist = GetStringList(str);
        for (int i = 0; i < strlist.Count; i++)
        {
            string[] tmp = strlist[i].Split(':');
            if (tmp.Length >=2)
            {
                uint id = uint.Parse(tmp[0]);
                int num = int.Parse(tmp[1]);
                CommonItemData tmpData = new CommonItemData(id, num, true);
                list.Add(tmpData);
            }
        }
        return list;
    }

    private List<string> GetStringList(string str) 
    {
        return CommonFunction.GetSplitStr(str,';');
    }

    private Vector3 GetVector3(string str)
    {
        List<int> list = CommonFunction.GetParseStrToInt(str, ',');
        if (list != null && list.Count > 1)
        {
            return new Vector3(list[0], list[1], 0);
        }
        else
        {
            return Vector3.zero;
        }
    }

    private float GetBloodScoreFactor(string str)
    {
        List<int> list = CommonFunction.GetParseStrToInt(str,':');
        if (list.Count > 1)
        {
            return (float)list[1] / list[0];
        }
        return 0;
    }
    public void Uninitialize()
    {
        if (_campaignCityDatas != null)
        {
            _campaignCityDatas.Clear();
            _campaignCityDatas = null;
        }
        if (_campaignTokenDatas != null)
        {
            _campaignTokenDatas.Clear();
            _campaignTokenDatas = null;
        }
    }
}
