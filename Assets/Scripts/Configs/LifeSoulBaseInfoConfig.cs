using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LifeSoulBaseInfo
{
    public int type;
    /// <summary>
    /// 英雄解锁等级
    /// </summary>
    public int player_unlock_level;
    /// <summary>
    /// 武将解锁等级
    /// </summary>
    public int soldier_unlock_level;
    /// <summary>
    /// 初始概率
    /// </summary>
    public int init_probability;
    /// <summary>
    /// 成长概率
    /// </summary>
    public int up_probability;
    /// <summary>
    /// 指定猎魂价格
    /// </summary>
    public MoneyFlowData specify_price;
    /// <summary>
    /// 指定掉落包
    /// </summary>
    public uint specify_dropid;
    /// <summary>
    ///非指定猎魂价格
    /// </summary>
    public MoneyFlowData unspecify_price;
    /// <summary>
    ///非指定猎魂掉落包
    /// </summary>
    public int unspecify_dropid;
}

public class LifeSoulBaseInfoConfig : BaseConfig
{
    private List<LifeSoulBaseInfo> mLifeSoulBaseList;

    public LifeSoulBaseInfoConfig()
    {
        mLifeSoulBaseList = new List<LifeSoulBaseInfo>();
        Initialize(GlobalConst.Config.DIR_XML_LIFESOULBASE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    private void ParseConfig(string xmlstr)
    {
        try
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml(xmlstr);
            XmlNodeList nodelist = xmlDoc.SelectSingleNode("Data").ChildNodes;
            foreach (XmlElement xe in nodelist)
            {
                LifeSoulBaseInfo data = new LifeSoulBaseInfo();

                data.type = int.Parse(CommonFunction.GetXmlElementStr(xe, "type"));
                data.player_unlock_level = int.Parse(CommonFunction.GetXmlElementStr(xe, "p_unlock_level"));
                data.soldier_unlock_level = int.Parse(CommonFunction.GetXmlElementStr(xe, "s_unlock_level"));
                data.init_probability = int.Parse(CommonFunction.GetXmlElementStr(xe, "init_probability"));
                data.up_probability = int.Parse(CommonFunction.GetXmlElementStr(xe, "up_probability"));
                data.specify_dropid = uint.Parse(CommonFunction.GetXmlElementStr(xe, "specify_id"));
               
                data.unspecify_dropid = int.Parse(CommonFunction.GetXmlElementStr(xe, "unspecify_id"));
                string specify_price = CommonFunction.GetXmlElementStr(xe, "specify_price");
                if (specify_price == "0")
                    data.specify_price = null;
                else
                {
                    string[] specify_priceArr = specify_price.Split(':');
                    if (specify_priceArr != null && specify_priceArr.Length >= 2)
                    {
                        data.specify_price = new MoneyFlowData();
                        data.specify_price.Type = (ECurrencyType)int.Parse(specify_priceArr[0]);
                        data.specify_price.Number = int.Parse(specify_priceArr[1]);
                    }
                }

                string unspecify_price = CommonFunction.GetXmlElementStr(xe, "unspecify_price");
                if (unspecify_price == "0")
                    data.unspecify_price = null;
                else
                {
                    string[] unspecify_priceArr = unspecify_price.Split(':');
                    if (unspecify_priceArr != null && unspecify_priceArr.Length >= 2)
                    {
                        data.unspecify_price = new MoneyFlowData();
                        data.unspecify_price.Type = (ECurrencyType)int.Parse(unspecify_priceArr[0]);
                        data.unspecify_price.Number = int.Parse(unspecify_priceArr[1]);
                    }
                }
                mLifeSoulBaseList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError("Load LifeSoulBaseConfig XML Error:" + ex.Message);
        }
    }


    public LifeSoulBaseInfo GetDataByType(int type)
    {
        LifeSoulBaseInfo info = mLifeSoulBaseList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.type == type;
        });
        if (info == null)
            Debug.LogError("can not get LifeSoulBaseInfo by id :" + type.ToString());
        return info;
    }

    public List<LifeSoulBaseInfo> GetBaseInfoList() 
    {
        return mLifeSoulBaseList;
    }

    public void Uninitialize()
    {
        if (mLifeSoulBaseList != null)
        {
            mLifeSoulBaseList.Clear();
            mLifeSoulBaseList = null;
        }
    }
}
