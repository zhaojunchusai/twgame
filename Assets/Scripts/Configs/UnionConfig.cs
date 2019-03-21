using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class UnionIconData
{
    public uint mID;
    public string mIcon;
    public int mUnLock;
    public int mLimitTime;
}

public class UnionDonateRate
{
    public uint Times;
    public uint Cost;
    public uint SupplyNum;
    public uint UnionToken;

    public UnionDonateRate(List<uint> list)
    {
        SetValue(list);
    }

    public void SetValue(List<uint> list)
    {
        if (list.Count < 4)
            return;

        Times = list[0];
        Cost = list[1];
        SupplyNum = list[2];
        UnionToken = list[3];
    }
}

public class UnionLvUpData
{
    public int mLevel;
    public uint mCost;
    public uint mMemberCount;
}

public class CityScore
{
    public uint mID;
    public uint mScore;
}

public class ExoticPassedAward
{
    public uint gateID;
    public uint awardID;
}

public class UnionBaseData
{
    public uint mCreateUnionCost;
    public uint mRenameUnionCost;
    public uint mUpdateUnionIconCost;
    public List<UnionDonateRate> mGoldDonate;
    public List<UnionDonateRate> mDiamondDonate;
    public uint mMaxUnionPVETimes;
    public uint mEveryAdvantureReward;
    public uint mResetDungeonCost;
    public uint mGetHpTimesLimit;
    public uint mVisitTimesLimit;
    public uint mVisitSendHpCount;
    public uint mBasePvpScore;
    public uint mMinPvpScore;
    public uint mMaxPVPScore;
    public uint mPvpSubScore;
    public uint mPvpAddScore;
    public List<CityScore> mCityScores;
    //public uint mLuoYangScore;   //洛阳
    //public uint mXingYangScore;  //荥阳
    //public uint mWanChengScore;  //宛城
    //public uint mHuLaoScore;     //虎牢 
    //public uint mHongNongScore;  //弘农

    public uint mWinScore;
    public uint mPvpWinReward;
    public uint mPvpLoseReward;
    /// <summary>
    /// 异域探险规则
    /// </summary>
    public string mAdvantureRule;
    /// <summary>
    /// 争霸规则
    /// </summary>
    public string mHegemonyRule;
    /// <summary>
    /// 整体通关奖励
    /// </summary>
    public List<ExoticPassedAward> mPassAdvantureRewards;
    public string mUnionRule;
    public int UnionFreeTime;
    public string altar_rule;
    public uint LeaveUnionCD;
    public uint RemoveChairmanCD;//多少秒后不上线罢免军团长
    public uint ChooseChairmanTime;//罢免前多少秒内登陆的人中选
}

public class UnionRankRewardData
{
    public uint mRank;
    public int mAdvantureGoldReward;
    public int mAdvantureTokenReward;
    public int mPvpTokenReward;
    public int mKillTokenReward;
}

public class UnionConfig : BaseConfig
{
    public int MaxUnionLevel = 0;
    private List<UnionIconData> _unionIconDatas;
    private UnionBaseData _unionBaseDatas;
    private Dictionary<int, UnionLvUpData> _unionLvUpDatas;
    private Dictionary<uint, UnionRankRewardData> _unionRankRewardDatas;
    public uint minUnionBadgeID = 0;

    public UnionConfig()
    {
        _unionIconDatas = new List<UnionIconData>();
        _unionBaseDatas = new UnionBaseData();
        _unionLvUpDatas = new Dictionary<int, UnionLvUpData>();
        _unionRankRewardDatas = new Dictionary<uint, UnionRankRewardData>();
        MaxUnionLevel = 0;
        Initialize(GlobalConst.Config.DIR_XML_UNION_ICON, ParseUnionIconConfig);
        Initialize(GlobalConst.Config.DIR_XML_UNION_BASE, ParseUnionBaseConfig);
        Initialize(GlobalConst.Config.DIR_XML_UNION_UPGRADE, ParseUnionLvConfig);
        Initialize(GlobalConst.Config.DIR_XML_UNION_RANKREWARD, ParseUnionRankRewardConfig);
    }

    public string GetUnionIconByID(string id)
    {
        uint mId = 0;
        if (!uint.TryParse(id, out mId))
        {
            return string.Empty;
        }
        return GetUnionBadgeByID(mId);
    }
    public string GetUnionBadgeByID(uint vID)
    {
        UnionIconData tmpData = GetUnionIconByID(vID);
        if (tmpData == null)
        {
            return string.Empty;
        }
        else
        {
            return tmpData.mIcon;
        }
    }
    public UnionIconData GetMinUnionIconData()
    {
        return GetUnionIconByID(minUnionBadgeID);
    }
    public UnionIconData GetUnionIconByID(uint id)
    {
        UnionIconData info = _unionIconDatas.Find((data) =>
        {
            return data.mID == id;
        });
        if (info == null)
        {
            Debug.LogError("can not get UnionIcon by id:" + id);
        }
        return info;
    }
    public List<UnionIconData> GetUnionIconList()
    {
        return _unionIconDatas;
    }
    public UnionBaseData GetUnionBaseData()
    {
        return _unionBaseDatas;
    }

    public Dictionary<uint, UnionRankRewardData> GetUnionRankData()
    {
        return _unionRankRewardDatas;
    }
    public UnionLvUpData GetUnionLvUpData(int level)
    {
        if (_unionLvUpDatas.ContainsKey(level))
            return _unionLvUpDatas[level];

        return null;
    }


    private void ParseUnionLvConfig(string bytes)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(bytes);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                UnionLvUpData data = new UnionLvUpData();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "level")
                    {
                        data.mLevel = int.Parse(xe.InnerText);

                    }
                    else if (xe.Name == "need_res")
                    {
                        data.mCost = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "max_num")
                    {
                        data.mMemberCount = uint.Parse(xe.InnerText);
                    }
                }
                if (data.mLevel > MaxUnionLevel)
                    MaxUnionLevel = data.mLevel;
                _unionLvUpDatas.Add(data.mLevel, data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private void ParseUnionIconConfig(string bytes)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(bytes);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                UnionIconData data = new UnionIconData();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "id")
                    {
                        data.mID = uint.Parse(xe.InnerText);
                        if (minUnionBadgeID == 0)
                        {
                            minUnionBadgeID = data.mID;
                        }
                        else
                        {
                            if (data.mID < minUnionBadgeID)
                            {
                                minUnionBadgeID = data.mID;
                            }
                        }
                    }
                    else if (xe.Name == "icon")
                    {
                        data.mIcon = xe.InnerText;
                    }
                    else if (xe.Name == "unlock")
                    {
                        int tmpValue = 0;
                        if (!int.TryParse(xe.InnerText, out tmpValue))
                        {
                            tmpValue = 0;
                        }
                        data.mUnLock = tmpValue;
                    }
                    else if (xe.Name == "limittime")
                    {
                        int tmpValue = 0;
                        if (!int.TryParse(xe.InnerText, out tmpValue))
                        {
                            tmpValue = 0;
                        }
                        data.mLimitTime = tmpValue;
                    }
                }
                _unionIconDatas.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }
    private void ParseUnionBaseConfig(string bytes)
    {
        //try
        //{
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.LoadXml(bytes);
        XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
        foreach (XmlElement element in nodelist)
        {
            UnionBaseData data = new UnionBaseData();
            foreach (XmlElement xe in element)
            {
                if (xe.Name == "CreateUnionCost")
                {
                    data.mCreateUnionCost = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "RenameUnionCost")
                {
                    data.mRenameUnionCost = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "UpdateUnionIconCost")
                {
                    data.mUpdateUnionIconCost = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "GoldDonateRate")
                {
                    data.mGoldDonate = GetDonateList(xe.InnerText);
                }
                else if (xe.Name == "DiamondDonateRate")
                {
                    data.mDiamondDonate = GetDonateList(xe.InnerText);
                }
                else if (xe.Name == "EveryAdventureReward")
                {
                    data.mEveryAdvantureReward = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "ResetDungeonCost")
                {
                    data.mResetDungeonCost = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "GetHpTimesLimit")
                {
                    data.mGetHpTimesLimit = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "VisitTimesLimit")
                {
                    data.mVisitTimesLimit = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "VisitSendHpCount")
                {
                    data.mVisitSendHpCount = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "BasePvpScore")
                {
                    data.mBasePvpScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "MinPvpScore")
                {
                    data.mMinPvpScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "MaxPVPScore")
                {
                    data.mMaxPVPScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "PvpSubScore")
                {
                    data.mPvpSubScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "PvpAddScore")
                {
                    data.mPvpAddScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "CityScore")
                {
                    data.mCityScores = GetCityScore(xe.InnerText);
                }
                /*else if (xe.Name == "LuoYangScore")
                {
                    data.mLuoYangScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "XingYangScore")
                {
                    data.mXingYangScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "WanChengScore")
                {
                    data.mWanChengScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "HuLaoScore")
                {
                    data.mHuLaoScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "HongNongScore")
                {
                    data.mHongNongScore = uint.Parse(xe.InnerText);
                }*/
                else if (xe.Name == "WinScore")
                {
                    data.mWinScore = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "PvpWinReward")
                {
                    data.mPvpWinReward = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "PvpLoseReward")
                {
                    data.mPvpLoseReward = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "MaxUnionPVETimes")
                {
                    data.mMaxUnionPVETimes = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "HegemonyRule")
                {
                    data.mHegemonyRule = xe.InnerText;
                }
                else if (xe.Name == "free_time")
                {
                    data.UnionFreeTime = int.Parse(xe.InnerText);
                }
                else if (xe.Name == "LeaveUnionCD")
                {
                    data.LeaveUnionCD = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "RecallPeriod")
                {
                    data.RemoveChairmanCD = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "RecallForwardPeriod")
                {
                    data.ChooseChairmanTime = uint.Parse(xe.InnerText);
                }
                else if (xe.Name == "altar_rule")
                {
                    data.altar_rule = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                }
                else if (xe.Name == "AdventureRule")
                {
                    data.mAdvantureRule = xe.InnerText;
                }
                else if (xe.Name == "PassAdventureReward")
                {
                    data.mPassAdvantureRewards = new List<ExoticPassedAward>();
                    if (string.IsNullOrEmpty(xe.InnerText))
                        continue;
                    if (xe.InnerText == "0")
                        continue;
                    string[] strAwards = xe.InnerText.Split(';');
                    for (int i = 0; i < strAwards.Length; i++)
                    {
                        string str = strAwards[i];
                        if (string.IsNullOrEmpty(str))
                            continue;
                        string[] array = str.Split(':');
                        if (array == null || array.Length < 2)
                            continue;
                        ExoticPassedAward award = new ExoticPassedAward();
                        award.gateID = uint.Parse(array[0]);
                        award.awardID = uint.Parse(array[1]);
                        data.mPassAdvantureRewards.Add(award);
                    }
                }
                else if (xe.Name == "UnionRule")
                {
                    data.mUnionRule = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                }
            }
            _unionBaseDatas = data;
        }
        base.LoadCompleted();
        //}
        //catch (Exception ex)
        //{
        //    Debug.Log(ex.Message);
        //}
    }

    public void ParseUnionRankRewardConfig(string bytes)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(bytes);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement element in nodelist)
            {
                UnionRankRewardData data = new UnionRankRewardData();
                foreach (XmlElement xe in element)
                {
                    if (xe.Name == "Rank")
                    {
                        data.mRank = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "AdventureGoldReward")
                    {
                        data.mAdvantureGoldReward = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "AdventureTokenReward")
                    {
                        data.mAdvantureTokenReward = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "PvpTokenReward")
                    {
                        data.mPvpTokenReward = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "KillTokenReward")
                    {
                        data.mKillTokenReward = int.Parse(xe.InnerText);
                    }
                }
                _unionRankRewardDatas.Add(data.mRank, data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.Log(ex.Message);
        }
    }

    private List<CityScore> GetCityScore(string str)
    {
        List<CityScore> list = new List<CityScore>();
        string[] arr = str.Split(';');
        string[] arr1;

        for (int i = 0; i < arr.Length; i++)
        {
            arr1 = arr[i].Split(':');
            if (arr1.Length > 1)
            {
                CityScore cs = new CityScore();
                cs.mID = uint.Parse(arr1[0]);
                cs.mScore = uint.Parse(arr1[1]);
                list.Add(cs);
            }
        }
        return list;
    }

    private List<UnionDonateRate> GetDonateList(string str)
    {
        List<UnionDonateRate> result = new List<UnionDonateRate>();
        List<string> list = CommonFunction.GetSplitStr(str, ';');
        foreach (string item in list)
        {
            UnionDonateRate data = new UnionDonateRate(CommonFunction.GetParseStrToUint(item, ':'));
            result.Add(data);
        }
        return result;
    }

    public void Uninitialize()
    {
        if (_unionIconDatas != null)
        {
            _unionIconDatas.Clear();
            _unionIconDatas = null;
        }
        if (_unionLvUpDatas != null)
        {
            _unionLvUpDatas.Clear();
            _unionLvUpDatas = null;
        }
        if (_unionRankRewardDatas != null)
        {
            _unionRankRewardDatas.Clear();
            _unionRankRewardDatas = null;
        }
        if (_unionBaseDatas != null)
        {
            _unionBaseDatas = null;
        }
    }

}
