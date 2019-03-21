using System;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;
using System.Collections;
using fogs.proto.msg;

public class TimesExpendData
{
    /// <summary>
    /// 消耗次数
    /// </summary>
    public uint Times;
    /// <summary>
    /// 购买体力
    /// </summary>
    public MoneyFlowData BuySP;

    public Dictionary<ShopType, MoneyFlowData> ShopRefresh = new Dictionary<ShopType, MoneyFlowData>();

    /// <summary>
    /// 通用商店刷新
    /// </summary>
    public MoneyFlowData NorShopRefresh;
    /// <summary>
    /// 竞技场商店刷新
    /// </summary>
    public MoneyFlowData MedalShopRefresh;
    /// <summary>
    /// 远征商店刷新
    /// </summary>
    public MoneyFlowData HonorShopRefresh;
    /// <summary>
    /// 公会商店刷新
    /// </summary>
    public MoneyFlowData UnionShopRefresh;
    /// <summary>
    /// zaohua商店刷新
    /// </summary>
    public MoneyFlowData RecycleShopRefresh;
    /// <summary>
    /// 兑换金币
    /// </summary>
    public MoneyFlowData ExchangeGold;
    /// <summary>
    /// 装备背包上限购买
    /// </summary>
    public MoneyFlowData EquipBagLimit;
    /// <summary>
    /// 购买竞技场挑战次数
    /// </summary>
    public MoneyFlowData AsynPvpTimes;
    /// <summary>
    /// 购买主线副本挑战次数
    /// </summary>
    public MoneyFlowData AdvanceLevelChallengeTimes;
    /// <summary>
    /// 活动副本购买次数
    /// </summary>
    public MoneyFlowData ActivityDgnChallengeTimes;
    /// <summary>
    /// 排位赛购买次数
    /// </summary>
    public MoneyFlowData QualifyingPurchaseTimes;
    /// <summary>
    /// 排位赛购买消耗
    /// </summary>
    public MoneyFlowData QualifyingPurchaseConsume;
    /// <summary>
    /// 排位赛购买消耗
    /// </summary>
    public MoneyFlowData QualifyingRevengeConsume;

    /// <summary>
    /// 命魂扩充消耗
    /// </summary>
    public MoneyFlowData LifeSoulPackConsume;
    /// <summary>
    /// 无尽战场次数购买消耗
    /// </summary>
    public MoneyFlowData EndlessConsume;
    /// <summary>
    /// 异域探险次数购买消耗
    /// </summary>
    public MoneyFlowData ExoticAdvantureConsume;

    /// <summary>
    /// 跨服战购买消耗
    /// </summary>
    public MoneyFlowData CrossServerConsume;
}

public class TimesExpendConfig : BaseConfig
{
    private Dictionary<uint, TimesExpendData> _TimesExpendDatas;

    public TimesExpendConfig()
    {
        _TimesExpendDatas = new Dictionary<uint, TimesExpendData>();
        Initialize(GlobalConst.Config.DIR_XML_TIMESEXPEND, ParseConfig);
    }

    public TimesExpendData GetTimesExpendData(uint times)
    {
        return !_TimesExpendDatas.ContainsKey(times) ? null : _TimesExpendDatas[times];
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
                TimesExpendData data = new TimesExpendData();
                //Debug.LogWarning("===================================================");
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "num")
                    {
                        data.Times = uint.Parse(xe.InnerText);
                    }
                    if (xe.Name == "ph_power")
                    {
                        data.BuySP = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "normal_shop")
                    {
                        data.NorShopRefresh = ParseMoneyFlowData(xe.InnerText);
                        data.ShopRefresh.Add(ShopType.ST_NomalShop, data.NorShopRefresh);
                    }
                    if (xe.Name == "medal_shop")
                    {
                        data.MedalShopRefresh = ParseMoneyFlowData(xe.InnerText);
                        data.ShopRefresh.Add(ShopType.ST_MedalShop, data.MedalShopRefresh);
                    }
                    if (xe.Name == "honor_shop")
                    {
                        data.HonorShopRefresh = ParseMoneyFlowData(xe.InnerText);
                        data.ShopRefresh.Add(ShopType.ST_HonorShop, data.HonorShopRefresh);
                    }
                    if (xe.Name == "union_shop")
                    {
                        data.UnionShopRefresh = ParseMoneyFlowData(xe.InnerText);
                        data.ShopRefresh.Add(ShopType.ST_UnionShop, data.UnionShopRefresh);
                    }
                    if (xe.Name == "recycle_shop")
                    {
                        data.RecycleShopRefresh = ParseMoneyFlowData(xe.InnerText);
                        data.ShopRefresh.Add(ShopType.ST_RecycleShop, data.RecycleShopRefresh);
                    }
                    if (xe.Name == "exchange_gold")
                    {
                        //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                        data.ExchangeGold = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "equipbag_limit")
                    {
                        data.EquipBagLimit = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "athletics")
                    {
                        data.AsynPvpTimes = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "main_line")
                    {
                        data.AdvanceLevelChallengeTimes = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "acitvity_dgn")
                    {
                        data.ActivityDgnChallengeTimes = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "qualifying_purchase")
                    {
                        data.QualifyingPurchaseConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "qualifying_revenge")
                    {
                        data.QualifyingRevengeConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "buy_soul_grid")
                    {
                        data.LifeSoulPackConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "endless_purchase")
                    {
                        data.EndlessConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "exoticAdvanture_purchase")
                    {
                        data.ExoticAdvantureConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                    if (xe.Name == "crossserver_purchase")
                    {
                        data.CrossServerConsume = ParseMoneyFlowData(xe.InnerText);
                    }
                }
                _TimesExpendDatas.Add(data.Times, data);
            }
            LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError(ex.Message);
        }
    }

    private MoneyFlowData ParseMoneyFlowData(string str)
    {
        return CommonFunction.ParseMoneyFlowData(str);
    }

    /// <summary>
    /// 获取消耗钻石相同的,剩余兑换的数量
    /// </summary>
    public int GetNextExchageGoldCount(int exchageTimes, int needDiamond)
    {
        int count = 0;
        TimesExpendData lastData = GetTimesExpendData((uint)exchageTimes + 1);
        if (lastData == null)
            return count;
        foreach (KeyValuePair<uint, TimesExpendData> data in _TimesExpendDatas)
        {
            //优化说最多就连续兑换10次
            if (data.Key > exchageTimes && data.Value.ExchangeGold.Number == needDiamond && count < 10)
            {
                count += 1;
            }
        }
        return count;
    }

    public void Uninitialize()
    {
        if (_TimesExpendDatas != null)
        {
            _TimesExpendDatas.Clear();
            _TimesExpendDatas = null;
        }
    }
}
