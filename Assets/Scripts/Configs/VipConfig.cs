using System;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using fogs.proto.msg;
using System.Linq;

public class VipData
{
    public uint CurLv;
    public uint TotalExp;
    /// <summary>
    /// 精英副本次数
    /// </summary>
    public uint ElteCount;
    /// <summary>
    /// 体力购买次数
    /// </summary>
    public uint EnergyBuyCount;
    /// <summary>
    /// 金币兑换次数
    /// </summary>
    public uint ExchangeGoldCount;
    /// <summary>
    /// 免费聊天次数
    /// </summary>
    public uint ChatCount;
    /// <summary>
    /// 竞技场挑战购买次数
    /// </summary>
    public uint ChallengeBuyCount;
    /// <summary>
    /// 活动挑战次数
    /// </summary>
    public uint ActivityCount;
    /// <summary>
    /// 登陆签到VIP额外奖励掉落包ID
    /// </summary>
    public uint LoginExtraAward;
    /// <summary>
    /// 商店刷新限制
    /// </summary>
    public Dictionary<ShopType, uint> StoreRefreshCount = new Dictionary<ShopType, uint>();
    public string Description;
    /// <summary>
    /// VIP每日奖励掉落包ID
    /// </summary>
    public uint DayGift;
    /// <summary>
    /// VIP升级奖励掉落包ID
    /// </summary>
    public uint LvUpGift;
    /// <summary>
    /// 排位赛购买次数
    /// </summary>
    public uint QualifyingLimit;
    /// <summary>
    /// 命魂购买次数
    /// </summary>
    public uint Soul_Grid_Num;
    /// <summary>
    /// 无尽战场购买次数
    /// </summary>
    public int EndlessLimit;
    /// <summary>
    /// 异域探险购买次数
    /// </summary>
    public int ExoticAdvantureLimit; 
}

public class VipConfig : BaseConfig
{
    private Dictionary<uint, VipData> _vipDatas;
    public static uint VipMaxLevel { get; private set; }
    private MaxTimesLeastLevelTips tips = new MaxTimesLeastLevelTips(); 

    public VipConfig()
    {
        _vipDatas = new Dictionary<uint, VipData>();
        Initialize(GlobalConst.Config.DIR_XML_VIP, ParseConfig);
    }

    public VipData GetVipDataByLv(uint lv)
    {
        return !_vipDatas.ContainsKey(lv) ? null : _vipDatas[lv];
    }

    public List<VipData> GetVipInfoList()
    {
        return new List<VipData>(_vipDatas.Values);
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
                VipData data = new VipData();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "vip_lv")
                    {
                        data.CurLv = uint.Parse(xe.InnerText);
                        if (VipMaxLevel < data.CurLv)
                            VipMaxLevel = data.CurLv;
                    }
                    if (xe.Name == "upgrade_condition")
                    {
                        data.TotalExp = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "elte_count")
                    {
                        data.ElteCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "ph_power_count")
                    {
                        data.EnergyBuyCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "exchange_count")
                    {
                        data.ExchangeGoldCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "chat_count")
                    {
                        data.ChatCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "challenge_count")
                    {
                        data.ChallengeBuyCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "acitvity_count")
                    {
                        data.ActivityCount = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "shop_limit")
                    {
                        data.StoreRefreshCount = ParseStrToShopLimit(xe.InnerText);
                    }
                    if (xe.Name == "login_reward")
                    {
                        data.LoginExtraAward = uint.Parse(xe.InnerText);
                        data.DayGift = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "prop_gift")
                    {
                        data.LvUpGift = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "desc")
                    {
                        data.Description = GetVipDesc(xe.InnerText);
                    }
                    if (xe.Name == "qualifying_limit")
                    {
                        data.QualifyingLimit = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "soul_grid_num")
                    {
                        data.Soul_Grid_Num = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "endless_limit")
                    {
                        data.EndlessLimit = int.Parse(xe.InnerText);
                    }
                    if (xe.Name == "exoticAdvanture_limit")
                    {
                        data.ExoticAdvantureLimit = int.Parse(xe.InnerText);
                    }
                }
                _vipDatas.Add(data.CurLv, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private Dictionary<ShopType, uint> ParseStrToShopLimit(string str)
    {
        Dictionary<ShopType, uint> result = new Dictionary<ShopType, uint>();
        string[] values = str.Split(';');
        string[] value;
        for (int i = 0; i < values.Length; i++)
        {
            value = values[i].Split(':');
            if (value.Length >= 2 && !result.ContainsKey((ShopType)uint.Parse(value[0])))
            {
                result.Add((ShopType)uint.Parse(value[0]), uint.Parse(value[1]));
            }
        }
        return result;
    }

    private string GetVipDesc(string str)
    {
        return str.Replace("/n", "\n");
    }

    public void Uninitialize()
    {
        if (_vipDatas != null)
        {
            _vipDatas.Clear();
            _vipDatas = null;
        }
    }
    /// <summary>
    /// 返回对应类型下，能达到最大购买次数的vip等级.如果返回结果为-1，则说明购买未开启;返回结果为0，则购买次数不与vip等级挂钩;返回结果为-10，属于流程错误或测试流程
    /// </summary>
    /// <param name="type">shopType 当判断的是商店刷新的时候需要传入商店类型</param>
    /// <returns></returns>
    public int LeastVIPLeveForMaxBuyTimesByType(VIPBUYTIMES type,ShopType shopType = ShopType.ST_NomalShop)
    {
        if (_vipDatas == null || _vipDatas.Count <= 0)
        {
            Debug.LogError("Can't read any data in config");
            return -10;
        }
        switch (type)
        {
            case VIPBUYTIMES.None:
                {
                    Debug.Log("No type input. There is reserved for some tests or special cases");
                    return -10;
                }
            case VIPBUYTIMES.Elite:
                {
                    if (tips.max_Elite == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {            
                            if (maxTime < data.Value.ElteCount)
                            {
                                tips.max_Elite = (int)data.Key;
                                maxTime = data.Value.ElteCount;
                            }
                        }
                        if (tips.max_Elite == -10)
                            tips.max_Elite = -1;
                    }
                    return tips.max_Elite;
                }
            case VIPBUYTIMES.Energy:
                {
                    if (tips.max_Energy == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.EnergyBuyCount)
                            {
                                tips.max_Energy = (int)data.Key;
                                maxTime = data.Value.EnergyBuyCount;
                            }
                        }
                        if (tips.max_Energy == -10)
                            tips.max_Energy = -1;
                    }
                    return tips.max_Energy;
                }
            case VIPBUYTIMES.ExchangeGold:
                {
                    if (tips.max_ExchangeGold == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.ExchangeGoldCount)
                            {
                                tips.max_ExchangeGold = (int)data.Key;
                                maxTime = data.Value.ExchangeGoldCount;
                            }
                        }
                        if (tips.max_ExchangeGold == -10)
                            tips.max_ExchangeGold = -1;
                    }
                    return tips.max_ExchangeGold;
                }
            case VIPBUYTIMES.Challenge:
                {
                    if (tips.max_Challenge == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.ChallengeBuyCount)
                            {
                                tips.max_Challenge = (int)data.Key;
                                maxTime = data.Value.ChallengeBuyCount;
                            }
                        }
                        if (tips.max_Challenge == -10)
                            tips.max_Challenge = -1;
                    }
                    return tips.max_Challenge;
                }
            case VIPBUYTIMES.Activity:
                {
                    if (tips.max_Activity == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.ActivityCount)
                            {
                                tips.max_Activity = (int)data.Key;
                                maxTime = data.Value.ActivityCount;
                            }
                        }
                        if (tips.max_Activity == -10)
                            tips.max_Activity = -1;
                    }
                    return tips.max_Activity;
                }
            case VIPBUYTIMES.Qualifying:
                {
                    if (tips.max_Qualifying == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.QualifyingLimit)
                            {
                                tips.max_Qualifying = (int)data.Key;
                                maxTime = data.Value.QualifyingLimit;
                            }
                        }
                        if (tips.max_Qualifying == -10)
                            tips.max_Qualifying = -1;
                    }
                    return tips.max_Qualifying;
                }
            case VIPBUYTIMES.Endless:
                {
                    if (tips.max_Endless == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.EndlessLimit)
                            {
                                tips.max_Endless = (int)data.Key;
                                maxTime = (uint)data.Value.EndlessLimit;
                            }
                        }
                        if (tips.max_Endless == -10)
                            tips.max_Endless = -1;
                    }
                    return tips.max_Endless;
                }
            case VIPBUYTIMES.ExoticAdventure:
                {
                    if (tips.max_ExoticAdventure == -10)
                    {
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.ExoticAdvantureLimit)
                            {
                                tips.max_ExoticAdventure = (int)data.Key;
                                maxTime = (uint)data.Value.ExoticAdvantureLimit;
                            }
                        }
                        if (tips.max_ExoticAdventure == -10)
                            tips.max_ExoticAdventure = -1;
                    }
                    return tips.max_ExoticAdventure;
                }
            case VIPBUYTIMES.StoreRefresh:
                {
                    //if (tips.max_StoreRefresh == -10)
                    //{
                        var orderList = _vipDatas.OrderBy(s => s.Key);

                        uint maxTime = 0;
                        foreach (var data in orderList)
                        {
                            if (maxTime < data.Value.StoreRefreshCount[shopType])
                            {
                                tips.max_StoreRefresh = (int)data.Key;
                                maxTime = data.Value.StoreRefreshCount[shopType];
                            }
                        }
                        if (tips.max_StoreRefresh == -10)
                            tips.max_StoreRefresh = -1;
                    //}
                    return tips.max_StoreRefresh;
                }
            default:
                Debug.LogError("input type error");
                return -10;
        }
        //return viplv;
    }
}
/// <summary>
/// 对应购买挑战次数的战斗类别：
/// 0=无类别
/// 1=精英副本
/// 2=购买体力
/// 3=购买铜钱
/// 4=PVP挑战
/// 5=兵伐不臣
/// 6=排位赛
/// 7=无尽战场
/// 8=异域探险
/// 9=商店刷新
/// </summary>
public enum VIPBUYTIMES
{
    None,
    Elite,
    Energy,
    ExchangeGold,
    Challenge,
    Activity,
    Qualifying,
    Endless,
    ExoticAdventure,
    StoreRefresh
}
/// <summary>
/// 保存对应类型的最大购买次数所需最低vip等级
/// </summary>
public class MaxTimesLeastLevelTips
{
    public int max_Elite = -10;
    public int max_Energy = -10;
    public int max_ExchangeGold = -10;
    public int max_Challenge = -10;
    public int max_Activity = -10;
    public int max_Qualifying = -10;
    public int max_Endless = -10;
    public int max_ExoticAdventure = -10;
    public int max_StoreRefresh = -10;

}
