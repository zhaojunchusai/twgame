using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Text.RegularExpressions;
using System.Collections.Generic;
public class EquipAttributeInfo
{
    /// <summary>
    /// 装备ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 装备名称
    /// </summary>
    public string name;
    /// <summary>
    /// 装备品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 装备星级
    /// </summary>
    public int star;
    /// <summary>
    /// 英雄/士兵装备 1=英雄装备（英雄装备无法出售，无获取途径）,2=士兵装备
    /// </summary>
    public int godEquip;
    /// <summary>
    /// 装备部位 0=武器；1=戒指；2=项链；3=衣服；4=坐骑
    /// </summary>
    public int type;
    /// <summary>
    /// 技能动作
    /// </summary>
    public string skillAnimation;
    /// <summary>
    /// 图标ID
    /// </summary>
    public string icon;
    /// <summary>
    /// 贴图路径
    /// </summary>
    public string altlasPath;
    /// <summary>
    /// 贴图ID
    /// </summary>
    public string region;
    /// <summary>
    /// 特效名
    /// </summary>
    public string EffectName;
    /// <summary>
    /// 初始等级
    /// </summary>
    public int baseLevel;
    /// <summary>
    /// 生命
    /// </summary>
    public int hp_max;
    /// <summary>
    /// 生命成长
    /// </summary>
    public int u_hp;
    /// <summary>
    /// 攻击
    /// </summary>
    public int phy_atk;
    /// <summary>
    /// 攻击成长
    /// </summary>
    public int u_attack;
    /// <summary>
    /// 命中
    /// </summary>
    public int acc_rate;
    /// <summary>
    /// 命中成长
    /// </summary>
    public int u_accuracy;
    /// <summary>
    /// 闪避
    /// </summary>
    public int ddg_rate;
    /// <summary>
    /// 闪避成长
    /// </summary>
    public int u_dodge;
    /// <summary>
    /// 暴击
    /// </summary>
    public int crt_rate;
    /// <summary>
    /// 暴击成长
    /// </summary>
    public int u_crit;
    /// <summary>
    /// 韧性
    /// </summary>
    public int tnc_rate;
    /// <summary>
    /// 韧性成长
    /// </summary>
    public int u_tenacity;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int speed;
    /// <summary>
    /// 速度成长
    /// </summary>
    public int u_speed;
    /// <summary>
    /// 技能ID
    /// </summary>
    public uint skillID;
    /// <summary>
    /// 等级限制
    /// </summary>
    public int levelLimit;
    /// <summary>
    /// 进阶/升星等级
    /// </summary>
    public int evolveLevel;
    /// <summary>
    /// 前置ID
    /// </summary>
    public uint prepositionId;
    /// <summary>
    /// 后置装备
    /// </summary>
    public uint evolveId;
    /// <summary>
    /// 材料经验
    /// </summary>
    public int materialExp;
    /// <summary>
    /// 升星所需经验
    /// </summary>
    public int evolveExp;
    /// <summary>
    /// 升星金钱类型
    /// </summary>
    public int evolveCostType;
    /// <summary>
    /// 升星金钱数值
    /// </summary>
    public int evolveCost;
    /// <summary>
    /// 出售金钱类型
    /// </summary>
    public int sellType;
    /// <summary>
    /// 出售价格
    /// </summary>
    public int price;
    /// <summary>
    /// 装备描述
    /// </summary>
    public string descript;
    /// <summary>
    /// 强化消耗
    /// </summary>
    public Dictionary<int, uint> MaterialBagList;
    /// <summary>
    /// 获取途径判断
    /// </summary>
    public bool isHadWay;
    /// <summary>
    /// 界面获取方式
    /// </summary>
    public List<uint> viewWays;
    /// <summary>
    /// 获取途径备注
    /// </summary>
    public string desc_way;
    /// <summary>
    /// 获取途径
    /// </summary>
    public List<uint> getways;
    /// <summary>
    /// 回收价格
    /// </summary>
    public MoneyFlowData recyclePrice;
    /// <summary>
    /// 套装ID
    /// </summary>
    public uint CoordID;
}

public class EquipAttributeConfig : BaseConfig
{
    private List<EquipAttributeInfo> _equipAttributeList;

    public EquipAttributeConfig()
    {
        _equipAttributeList = new List<EquipAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_EQUIPTTRIBUTE, ParseConfig);
    }
    public EquipAttributeInfo FindById(uint ID)
    {
        return _equipAttributeList.Find((EquipAttributeInfo p) =>
        {
            return p.id == ID;
        });
    }
    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_equipAttributeList != null)
        {
            _equipAttributeList.Clear();
            _equipAttributeList = null;
        }
    }

    public List<EquipAttributeInfo> GetEquipAttributeList()
    {
        return _equipAttributeList;
    }

    public List<EquipAttributeInfo> GetAllArtifacts()
    {
        List<EquipAttributeInfo> list = null;
        list = _equipAttributeList.FindAll((EquipAttributeInfo tmp) =>
        {
            if (tmp == null) return false;
            return tmp.godEquip.Equals(1);
        });
        if (list == null)
        {
            Debug.LogError("can not find Artifacts");
        }
        return list;
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
                EquipAttributeInfo data = new EquipAttributeInfo();
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
                    else if (xe.Name == "quality")
                    {
                        data.quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "star")
                    {
                        data.star = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "godEquip")
                    {
                        data.godEquip = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "type")
                    {
                        data.type = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "animation")
                    {
                        data.skillAnimation = xe.InnerText;
                    }
                    else if (xe.Name == "icon")
                    {
                        data.icon = xe.InnerText;
                    }
                    else if (xe.Name == "altlasPath")
                    {
                        data.altlasPath = xe.InnerText;
                    }
                    else if (xe.Name == "region")
                    {
                        data.region = xe.InnerText;
                    }
                    else if (xe.Name == "EffectName")
                    {
                        data.EffectName = xe.InnerText;
                    }
                    else if (xe.Name == "baseLevel")
                    {
                        data.baseLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "hp_max")
                    {
                        data.hp_max = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_hp")
                    {
                        data.u_hp = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "phy_atk")
                    {
                        data.phy_atk = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_attack")
                    {
                        data.u_attack = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "acc_rate")
                    {
                        data.acc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_accuracy")
                    {
                        data.u_accuracy = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ddg_rate")
                    {
                        data.ddg_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_dodge")
                    {
                        data.u_dodge = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "crt_rate")
                    {
                        data.crt_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_crit")
                    {
                        data.u_crit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "tnc_rate")
                    {
                        data.tnc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_tenacity")
                    {
                        data.u_tenacity = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "speed")
                    {
                        data.speed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_speed")
                    {
                        data.u_speed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "skillID")
                    {
                        data.skillID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "levelLimit")
                    {
                        data.levelLimit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveLevel")
                    {
                        data.evolveLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "prepositionId")
                    {
                        data.prepositionId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveId")
                    {
                        data.evolveId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "materialExp")
                    {
                        data.materialExp = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveExp")
                    {
                        data.evolveExp = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveCostType")
                    {
                        data.evolveCostType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveCost")
                    {
                        data.evolveCost = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sellType")
                    {
                        data.sellType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "price")
                    {
                        data.price = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "descript")
                    {
                        data.descript = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "costbag")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.MaterialBagList = new Dictionary<int, uint>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;

                            if (!data.MaterialBagList.ContainsKey(int.Parse(tt[0])))
                            {
                                data.MaterialBagList.Add(int.Parse(tt[0]), uint.Parse(tt[1]));
                            }
                        }
                    }
                    else if (xe.Name == "ishad_way")
                    {
                        data.isHadWay = false;
                        uint hadway = 0;
                        if (uint.TryParse(xe.InnerText, out hadway))
                        {
                            if (hadway == 1)
                            {
                                data.isHadWay = true;
                            }
                        }
                    }
                    else if (xe.Name == "ways_view")
                    {
                        data.viewWays = new List<uint>();
                        if (xe.InnerText != "0") //约定为0时,无途径界面
                        {
                            string[] datas = xe.InnerText.Split(';');
                            for (int i = 0; i < datas.Length; i++)
                            {
                                uint view = 0;
                                if (uint.TryParse(datas[i], out view)) //避免因策划配置错误而导致的程序崩溃
                                {
                                    data.viewWays.Add(view);
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
                    else if (xe.Name == "getways")
                    {
                        if (data.getways == null)
                        {
                            data.getways = new List<uint>();
                        }
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
                    else if (xe.Name == "recyclePrice")
                    {
                        data.recyclePrice = CommonFunction.ParseMoneyFlowData(xe.InnerText);
                    }
                    else if (xe.Name == "CoordID")
                    {
                        data.CoordID = uint.Parse(xe.InnerText);
                    }
                }
                _equipAttributeList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load EquipAttribute XML Error:" + ex.Message);
        }
    }
}