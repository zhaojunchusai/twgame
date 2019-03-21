using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class LifeSoulConfigInfo
{
    public uint id;
    public string name;
    public int quality;
    public int type;
    /// <summary>
    /// 装备限制0-通用；1-英雄专用；2-武将专用 
    /// </summary>
    public int godEquip;

    public string icon;
    public int levelLimit;
    /// <summary>
    /// 命魂的初始生命值上限
    /// </summary>
    public int hp_initial;
    /// <summary>
    /// 生命属性每次升级时提升的属性数值
    /// </summary>
    public int hp_up;
    /// <summary>
    /// 命魂的初始攻击值
    /// </summary>
    public int attack_initial;
    /// <summary>
    /// 攻击属性每次升级时提升的属性数值
    /// </summary>
    public int attack_up;
    /// <summary>
    /// 命魂的初始命中值
    /// </summary>
    public int accrate_initial;
    /// <summary>
    /// 命中属性每次升级时提升的属性数值
    /// </summary>
    public int accrate_up;
    /// <summary>
    /// 命魂的初始闪避值
    /// </summary>
    public int ddgrate_initial;
    /// <summary>
    /// 闪避属性每次升级时提升的属性数值
    /// </summary>
    public int ddgrate_up;
    /// <summary>
    /// 命魂的初始暴击值
    /// </summary>
    public int crt_initial;
    /// <summary>
    /// 暴击属性每次升级时提升的属性数值
    /// </summary>
    public int crt_up;

    /// <summary>
    /// 命魂的初始韧性值
    /// </summary>
    public int tenacity_initial;
    /// <summary>
    /// 韧性属性每次升级时提升的属性数值
    /// </summary>
    public int tenacity_up;

    /// <summary>
    /// 附带技能ID  约定技能ID为0  则附带属性
    /// </summary>
    public uint skillID;
    /// <summary>
    /// 材料经验
    /// </summary>
    public int materialExp;
    /// <summary>
    /// 升级消耗
    /// </summary>
    public List<UpgradeCostInfo> upgrade_consume;
    /// <summary>
    /// 出售价格
    /// </summary>
    public MoneyFlowData sellPrice;

    public string desc;
    /// <summary>
    /// 回收价格
    /// </summary>
    public MoneyFlowData recyclePrice;
}

public class LifeSoulConfig : BaseConfig
{
    private List<LifeSoulConfigInfo> mLifeSoulList;

    public LifeSoulConfig()
    {
        mLifeSoulList = new List<LifeSoulConfigInfo>();
        Initialize(GlobalConst.Config.DIR_XML_LIFESOUL, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (mLifeSoulList != null)
        {
            mLifeSoulList.Clear();
            mLifeSoulList = null;
        }
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
            LifeSoulConfigInfo data = new LifeSoulConfigInfo();
            data.name = CommonFunction.GetXmlElementStr(xe, "name");
            data.id = uint.Parse(CommonFunction.GetXmlElementStr(xe, "id"));
            data.quality = int.Parse(CommonFunction.GetXmlElementStr(xe, "quality"));
            data.type = int.Parse(CommonFunction.GetXmlElementStr(xe, "type"));
            data.godEquip = int.Parse(CommonFunction.GetXmlElementStr(xe, "godEquip"));
            data.icon = CommonFunction.GetXmlElementStr(xe, "icon");
            data.levelLimit = int.Parse(CommonFunction.GetXmlElementStr(xe, "levelLimit"));
            data.hp_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "hp_max"));
            data.hp_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_hp"));
            data.attack_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "phy_atk"));
            data.attack_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_attack"));
            data.accrate_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "acc_rate"));
            data.accrate_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_accuracy"));
            data.ddgrate_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "ddg_rate"));
            data.ddgrate_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_dodge"));
            data.crt_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "crt_rate"));
            data.crt_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_crit"));
            data.tenacity_initial = int.Parse(CommonFunction.GetXmlElementStr(xe, "tnc_rate"));
            data.tenacity_up = int.Parse(CommonFunction.GetXmlElementStr(xe, "u_tenacity"));
            data.skillID = uint.Parse(CommonFunction.GetXmlElementStr(xe, "skillID"));
            data.materialExp = int.Parse(CommonFunction.GetXmlElementStr(xe, "materialExp"));

            string up_cost = CommonFunction.GetXmlElementStr(xe, "costbag");
            data.upgrade_consume = new List<UpgradeCostInfo>();
            if (up_cost != "0")
            {
                string[] up_cost_arr = up_cost.Split(';');
                if (up_cost_arr != null)
                {
                    for (int i = 0; i < up_cost_arr.Length; i++)
                    {
                        string tmp_str = up_cost_arr[i];
                        if (tmp_str == null)
                            continue;
                        string[] tmp_arr = tmp_str.Split(',');
                        if (tmp_arr.Length >= 2)
                        {
                            UpgradeCostInfo tmp_data = new UpgradeCostInfo();
                            tmp_data.level = int.Parse(tmp_arr[0]);
                            tmp_data.exp = int.Parse(tmp_arr[1]);
                            data.upgrade_consume.Add(tmp_data);
                        }
                    }
                }
            }

            data.sellPrice = new MoneyFlowData();
            string priceStr = CommonFunction.GetXmlElementStr(xe, "sell_price");
            if (priceStr != "0")
            {
                string[] price_arr = priceStr.Split(':');
                if (price_arr.Length >= 2)
                {
                    data.sellPrice.Type = (ECurrencyType)int.Parse(price_arr[0]);
                    data.sellPrice.Number = int.Parse(price_arr[1]);
                }
            }
            data.desc = CommonFunction.GetXmlElementStr(xe, "desc");
            data.recyclePrice = CommonFunction.ParseMoneyFlowData(CommonFunction.GetXmlElementStr(xe, "recyclePrice"));
            mLifeSoulList.Add(data);

            base.LoadCompleted();
        }
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError("Load LifeSoulConfig XML Error:" + ex.Message);
        }
    }


    public LifeSoulConfigInfo GetLifeDataByID(int id)
    {
        LifeSoulConfigInfo data = null;
        if (mLifeSoulList != null)
        {
            data = mLifeSoulList.Find((tmp) =>
            {
                if (tmp == null)
                    return false;
                return tmp.id == id;
            });
        }
        if (data == null)
            Debug.LogError("can not get LifeSoulInfo by id:" + id.ToString());
        return data;
    }
}
