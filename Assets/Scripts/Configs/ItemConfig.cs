using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

/* File: Item.cs
 * Desc: 物品表
 * Date：2015-05-19 20:41
 * add by taiwei
 */

public class ItemInfo
{
    /// <summary>
    /// 道具ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 道具名称
    /// </summary>
    public string name;
    /// <summary>
    /// 图标资源名
    /// </summary>
    public string icon;
    /// <summary>
    /// 道具品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 道具类型  1:消耗品 2：材料 3:碎片
    /// </summary>
    public int type;
    /// <summary>
    /// 叠加上限
    /// </summary>
    public int overlap_max;
    /// <summary>
    /// 出售货币  0：0-不可出售；1-游戏币；2-钻石
    /// </summary>
    public int sell_type;
    /// <summary>
    /// 出售价格
    /// </summary>
    public int sell_price;
    /// <summary>
    /// 获取判断 true :有获取途径 false无获取途径
    /// </summary>
    public bool ishad_way;
    /// <summary>
    /// 获取途径
    /// </summary>
    public List<uint> getways;
    /// <summary>
    /// 途径界面
    /// </summary>
    public List<uint> view_ways;
    /// <summary>
    /// 途径备注
    /// </summary>
    public string desc_way;
    /// <summary>
    /// 合成数量
    /// </summary>
    public int compound_count;
    /// <summary>
    /// 合成花费类型
    /// </summary>
    public int costType;
    /// <summary>
    /// 合成花费数量
    /// </summary>
    public int cost;
    /// <summary>
    /// 合成产出ID
    /// </summary>
    public uint compound_Id;
    /// <summary>
    /// 使用类型  0-非使用道具；1-药剂；2-道具  3-武将召唤包
    /// </summary>
    public int use_type;
    /// <summary>
    /// 使用效果 当“使用类型”填入0时：  0-无效果占位；
    /// 当“使用类型”填入1时：  0-无效果占位；  1-提升血量恢复速度；  2-提升血量最大值；  3-提升能量恢复速度；  4-提升能量最大值；  5-提升魔法值恢复速度；
    /// 当“使用类型”填入1时：6-提升魔法最大值；  7-获得经验值增加x倍；  8-提升统御力；  9-提升攻击；  10-提升命中；  11-提升闪避；  12-提升暴击；  13-提升韧性；
    /// 当“使用类型”填入2时：  0-无效果占位；  1-获得道具（经验值、体力、游戏币、钻石都算道具）
    /// </summary>
    public int use_effect;
    /// <summary>
    /// 药剂数值 当“使用类型”填入1时：0-无效果占位；1-效果的数值，大于等于0的整数，填入格式“数量”，如“50”
    /// </summary>
    public int pill_value;
    /// <summary>
    /// 道具效果
    /// </summary>
    public List<Prop_value> proplist;
    /// <summary>
    /// 持续类型 当“使用类型”填入0时：0-无效果占位；当“使用类型”填入1时：0-无限制；1-战斗场次；2-时间
    /// </summary>
    public int continu_type;
    /// <summary>
    /// 持续时间 当药剂持续类型为场次时，填入大于等于0的整数，单位为场次，玩家完成该场次的战斗后药剂效果消失；当药剂持续类型为时间时，填入大于等于0的整数，单位为分钟，时间到后药剂效果消失
    /// </summary>
    public int continu_value;
    /// <summary>
    /// 限制类型 0-无限制；1-具体时间限制（起始时间至结束时间）；2-固定时间限制（次数满后，过了指定时间就能继续使用）
    /// </summary>
    public int limit_type;
    /// <summary>
    /// 限制时间 当“限制类型”填入0时：0-无效果占位
    /// 当“限制类型”填入1时：每多少单位的时间内，可使用道具多少次限制的具体时间，单位为小时。
    /// 当“限制类型”填入2时：具体刷新的时间，单位为小时。默认为0，即每日0时刷新
    /// </summary>
    public int limit_value;
    /// <summary>
    /// 单位限制 每个限制时间内可使用的次数，为0时表示无限制
    /// </summary>
    public int limit_unit;
    /// <summary>
    /// 道具描述
    /// </summary>
    public string desc;
    /// <summary>
    /// 材料补全时的价格
    /// </summary>
    public MoneyFlowData price;
    /// <summary>
    /// 回收价格
    /// </summary>
    public MoneyFlowData recyclePrice;
}

public class Prop_value
{
    /// <summary>
    /// 道具具体类型为：1=士兵；2=装备；3=道具；5=数值
    /// </summary>
    public uint type;
    /// <summary>
    /// 物品ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 物品个数
    /// </summary>
    public int count;

}

public class ItemConfig : BaseConfig
{
    private List<ItemInfo> _itemList;

    public ItemConfig()
    {
        _itemList = new List<ItemInfo>();
        Initialize(GlobalConst.Config.DIR_XML_ITEM, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_itemList != null)
        {
            _itemList.Clear();
            _itemList = null;
        }
    }

    public List<ItemInfo> GetItemList()
    {
        return _itemList;
    }

    public ItemInfo GetItemInfoByID(uint id)
    {
        ItemInfo info = null;
        info = _itemList.Find((ItemInfo _item) => { return _item.id == id; });
        if (info == null)
        {
            Debug.LogError("can not get ItemInfo by ID:" + id);
        }
        return info;
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
                ItemInfo data = new ItemInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "name")
                    {
                        data.name = xe.InnerText;
                    }
                    else if (xe.Name == "icon")
                    {
                        data.icon = xe.InnerText;
                    }
                    else if (xe.Name == "character")
                    {
                        data.quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "type")
                    {
                        data.type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "overlap_max")
                    {
                        data.overlap_max = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sell_type")
                    {
                        data.sell_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sell_price")
                    {
                        data.sell_price = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ishad_way")
                    {
                        data.ishad_way = false;
                        uint hadway = 0;
                        if (uint.TryParse(xe.InnerText, out hadway))
                        {
                            if (hadway == 1)
                            {
                                data.ishad_way = true;
                            }
                        }
                    }
                    else if (xe.Name == "getways")
                    {
                        data.getways = new List<uint>();
                        if (xe.InnerText != "0") //约定为0时,无获取途径
                        {
                            string[] datas = xe.InnerText.Split(';');
                            for (int i = 0; i < datas.Length; i++)
                            {
                                uint gate = 0;
                                if (uint.TryParse(datas[i], out gate)) //避免因策划配置错误而导致的程序崩溃
                                {
                                    data.getways.Add(gate);
                                }
                                else
                                {
                                    Debug.LogError("load config error!" + xe.Name);
                                    continue;
                                }
                            }
                        }
                    }
                    else if (xe.Name == "ways_view")
                    {
                        data.view_ways = new List<uint>();
                        if (xe.InnerText != "0") //约定为0时,无途径界面
                        {
                            string[] datas = xe.InnerText.Split(';');
                            for (int i = 0; i < datas.Length; i++)
                            {
                                uint view = 0;
                                if (uint.TryParse(datas[i], out view)) //避免因策划配置错误而导致的程序崩溃
                                {
                                    data.view_ways.Add(view);
                                }
                                else
                                {
                                    Debug.LogError("load config error!" + xe.Name);
                                    continue;
                                }
                            }
                        }
                    }
                    else if (xe.Name == "desc_way")
                    {
                        if (xe.InnerText == "0")     //约定为0时，无显示
                        {
                            data.desc_way = string.Empty;
                        }
                        else
                        {
                            data.desc_way = xe.InnerText;
                        }
                    }
                    else if (xe.Name == "compound_count")
                    {
                        data.compound_count = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "costType")
                    {
                        data.costType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "cost")
                    {
                        data.cost = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "compound_Id")
                    {
                        data.compound_Id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "use_type")
                    {
                        data.use_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "use_effect")
                    {
                        data.use_effect = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "pill_value")
                    {
                        data.pill_value = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "prop_value")
                    {
                        data.proplist = new List<Prop_value>();
                        if (xe.InnerText != "0")  //约定为0时,不做处理
                        {
                            string[] list = xe.InnerText.Split(';');
                            if (list == null || list.Length <= 0)
                                continue;
                            for (int i = 0; i < list.Length; i++)
                            {
                                string[] array = list[i].Split(':');
                                Prop_value prop = new Prop_value();
                                if (!uint.TryParse(array[0], out prop.type))
                                {
                                    Debug.LogError("load config error!" + xe.Name);
                                    continue;
                                }
                                if (!uint.TryParse(array[1], out prop.id))
                                {
                                    Debug.LogError("load config error!" + xe.Name);
                                    continue;
                                }
                                if (!int.TryParse(array[2], out prop.count))
                                {
                                    Debug.LogError("load config error!" + xe.Name);
                                    continue;
                                }
                                data.proplist.Add(prop);
                            }
                        }
                    }
                    else if (xe.Name == "continu_type")
                    {
                        data.continu_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "continu_value")
                    {
                        data.continu_value = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "limit_type")
                    {
                        data.limit_type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "limit_value")
                    {
                        data.limit_value = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "limit_unit")
                    {
                        data.limit_unit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "desc")
                    {
                        data.desc = xe.InnerText;
                    }
                    else if (xe.Name == "price")
                    {
                        data.price = CommonFunction.ParseMoneyFlowData(xe.InnerText);
                    }
                    else if (xe.Name == "recyclePrice")
                    {
                        data.recyclePrice = CommonFunction.ParseMoneyFlowData(xe.InnerText);
                    }
                }

                _itemList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load Item XML Error:" + ex.Message);
        }
    }
}