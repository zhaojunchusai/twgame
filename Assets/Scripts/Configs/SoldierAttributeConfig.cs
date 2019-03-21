
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class SoldierAttributeInfo
{
    /// <summary>
    /// 士兵ID
    /// </summary>
    public uint id;
    /// <summary>
    /// 士兵名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 士兵星级
    /// </summary>
    public int Star;
    /// <summary>
    /// 同名类型武将
    /// </summary>
    public int type;
    /// <summary>
    /// 士兵品质
    /// </summary>
    public int quality;
    /// <summary>
    /// 士兵上阵人数限制[主线副本 活动副本 无尽战场]
    /// </summary>
    public int limitNum;
    /// <summary>
    /// 士兵资质
    /// </summary>
    public int talent;
    /// <summary>
    /// 士兵站位
    /// </summary>
    public int Stance;
    /// <summary>
    /// 召唤能量
    /// </summary>
    public int call_energy;
    /// <summary>
    /// 召唤冷却
    /// </summary>
    public int coolLimit;
    /// <summary>
    /// 初始等级
    /// </summary>
    public int baseLevel;
    /// <summary>
    /// 职业ID
    /// </summary>
    public int Career;
    /// <summary>
    /// 头像ID
    /// </summary>
    public string Icon;
    /// <summary>
    /// 动画ID
    /// 命名规则：Soldier_{武将的ID}
    /// </summary>
    public string Animation;
    /// <summary>
    /// 音效ID
    /// </summary>
    public string Music;
    /// <summary>
    /// 统御力
    /// </summary>
    public int leaderShip;
    /// <summary>
    /// 生命
    /// </summary>
    public int hp_max;
    /// <summary>
    /// 生命成长
    /// </summary>
    public float u_hp;
    /// <summary>
    /// 生命升阶成长值
    /// </summary>
    public float stepHp;
    /// <summary>
    /// 攻击
    /// </summary>
    public int phy_atk;
    /// <summary>
    /// 攻击成长
    /// </summary>
    public float u_attack;
    /// <summary>
    /// 攻击升阶成长值
    /// </summary>
    public float stepAttack;
    /// <summary>
    /// 命中
    /// </summary>
    public int acc_rate;
    /// <summary>
    /// 命中成长
    /// </summary>
    public float u_accuracy;
    /// <summary>
    /// 命中升阶成长值
    /// </summary>
    public float stepAccuracy;
    /// <summary>
    /// 闪避
    /// </summary>
    public int ddg_rate;
    /// <summary>
    /// 闪避成长
    /// </summary>
    public float u_dodge;
    /// <summary>
    /// 闪避升阶成长值
    /// </summary>
    public float stepDodge;
    /// <summary>
    /// 暴击
    /// </summary>
    public int crt_rate;
    /// <summary>
    /// 暴击成长
    /// </summary>
    public float u_crit;
    /// <summary>
    /// 暴击升阶成长值
    /// </summary>
    public float stepCrit;
    /// <summary>
    /// 韧性
    /// </summary>
    public int tnc_rate;
    /// <summary>
    /// 韧性成长
    /// </summary>
    public float u_tenacity;
    /// <summary>
    /// 韧性升阶成长值
    /// </summary>
    public float stepTenacity;
    /// <summary>
    /// 攻击距离
    /// </summary>
    public int atk_space;
    /// <summary>
    /// 攻击间隔
    /// </summary>
    public int atk_interval;
    /// <summary>
    /// 移动速度
    /// </summary>
    public int speed;
    /// <summary>
    /// 界面缩放比例
    /// </summary>
    public float Scale;
    /// <summary>
    /// 最大阶级
    /// </summary>
    public int maxStep;
    /// <summary>
    /// 升阶材料ID
    /// </summary>
    public uint stepMaterialId;
    /// <summary>
    /// 升阶材料等级
    /// </summary>
    public int stepMaterialLv;
    /// <summary>
    /// 升阶材料阶级
    /// </summary>
    public int stepMaterialStep;
    /// <summary>
    /// 升阶金钱类型
    /// </summary>
    public int stepMoneyType;
    /// <summary>
    /// 升阶金钱数量
    /// </summary>
    public int stepMoneyNum;
    /// <summary>
    /// 升星等级
    /// </summary>
    public int evolveLv;
    /// <summary>
    /// 升星材料ID:等级
    /// </summary>
    public KeyValuePair<uint, int> evolveMateria;
    /// <summary>
    /// 升星材料所需阶级
    /// </summary>
    public int evolveMaterialStep;
    /// <summary>
    /// 金钱类型
    /// </summary>
    public int sellType;
    /// <summary>
    /// 金钱数值
    /// </summary>
    public int sellNum;
    /// <summary>
    /// 前置ID
    /// </summary>
    public uint lastId;
    /// <summary>
    /// 后置士兵
    /// </summary>
    public uint evolveId;
    /// <summary>
    /// 主动技能ID
    /// </summary>
    public uint initiativeSkill;
    /// <summary>
    /// 被动技能ID
    /// </summary>
    public List<uint> passivitySkill;
    /// <summary>
    /// 弹道类型[0-无弹道 1-抛物线 2-法师直线 3-长柄近战直线]
    /// </summary>
    public int trajectoryType;
    /// <summary>
    /// 弹道特效
    /// </summary>
    public string trajectory;
    /// <summary>
    /// 弹道音效
    /// </summary>
    public string trajectoryMusic;
    /// <summary>
    /// 装备部位<部位，装备类型>
    /// </summary>
    public Dictionary<int, int> PosList;
    /// <summary>
    /// 材料经验
    /// </summary>
    public int materiaExp;
    /// <summary>
    /// 升级级所需经验
    /// </summary>
    public Dictionary<int, int> StrengthExp;
    /// <summary>
    /// 标准移动速度[像素]
    /// </summary>
    public int NormalSpeed;
    /// <summary>
    /// 标准攻击延迟[毫秒]
    /// </summary>
    public int NormalInterval;
    /// <summary>
    /// 描述
    /// </summary>
    public string Descript;
    /// <summary>
    /// 武将定位
    /// </summary>
    public string SoldierPos;
    /// <summary>
    /// 武将传奇
    /// </summary>
    public string SoldierStory;
    /// <summary>
    /// 回收价格
    /// </summary>
    public MoneyFlowData recyclePrice;
    /// <summary>
    /// 属性偏向
    /// </summary>
    public string AttributeBias;
}

public class SoldierAttributeConfig : BaseConfig
{
    private List<SoldierAttributeInfo> _soldierAttributeList;

    public SoldierAttributeConfig()
    {
        _soldierAttributeList = new List<SoldierAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SOLDIERATTRIBUTE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_soldierAttributeList != null)
        {
            _soldierAttributeList.Clear();
            _soldierAttributeList = null;
        }
    }
    public List<SoldierAttributeInfo> GetSoldierAttributeList()
    {
        return _soldierAttributeList;
    }
    public SoldierAttributeInfo FindById(uint id)
    {
        return _soldierAttributeList.Find((SoldierAttributeInfo tp) => { return tp.id == id; });
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
                //Debug.LogWarning("==================================================================");
                SoldierAttributeInfo data = new SoldierAttributeInfo();
                foreach (XmlElement xe in Element)
                {
                    //Debug.LogWarning(string.Format("[{0}, {1}]", xe.Name, xe.InnerText));
                    if (xe.Name == "id")
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Name")
                    {
                        data.Name = xe.InnerText;
                    }
                    else if (xe.Name == "Star")
                    {
                        data.Star = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "quality")
                    {
                        data.quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "limitNum")
                    {
                        if (!int.TryParse(xe.InnerText, out data.limitNum))
                            data.limitNum = 0;
                    }
                    else if (xe.Name == "talent")
                    {
                        data.talent = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Stance")
                    {
                        data.Stance = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "call_energy")
                    {
                        data.call_energy = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "coolLimit")
                    {
                        data.coolLimit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "baseLevel")
                    {
                        data.baseLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Career")
                    {
                        data.Career = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Icon")
                    {
                        data.Icon = xe.InnerText;
                    }
                    else if (xe.Name == "Animation")
                    {
                        data.Animation = xe.InnerText;
                    }
                    else if (xe.Name == "Music")
                    {
                        data.Music = xe.InnerText;
                    }
                    else if (xe.Name == "leaderShip")
                    {
                        data.leaderShip = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "hp_max")
                    {
                        data.hp_max = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_hp")
                    {
                        data.u_hp = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepHp")
                    {
                        data.stepHp = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "phy_atk")
                    {
                        data.phy_atk = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_attack")
                    {
                        data.u_attack = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepAttack")
                    {
                        data.stepAttack = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "acc_rate")
                    {
                        data.acc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_accuracy")
                    {
                        data.u_accuracy = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepAccuracy")
                    {
                        data.stepAccuracy = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "ddg_rate")
                    {
                        data.ddg_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_dodge")
                    {
                        data.u_dodge = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepDodge")
                    {
                        data.stepDodge = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "crt_rate")
                    {
                        data.crt_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_crit")
                    {
                        data.u_crit = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepCrit")
                    {
                        data.stepCrit = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "tnc_rate")
                    {
                        data.tnc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_tenacity")
                    {
                        data.u_tenacity = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepTenacity")
                    {
                        data.stepTenacity = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "atk_space")
                    {
                        data.atk_space = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "atk_interval")
                    {
                        data.atk_interval = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "speed")
                    {
                        data.speed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Scale")
                    {
                        data.Scale = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "maxStep")
                    {
                        data.maxStep = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "stepMaterial")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ":");
                        if (anyLevel.Length == 3)
                        {
                            data.stepMaterialId = uint.Parse(anyLevel[0]);
                            data.stepMaterialLv = int.Parse(anyLevel[1]);
                            data.stepMaterialStep = int.Parse(anyLevel[2]);
                        }
                    }
                    else if (xe.Name == "stepMoney")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ":");
                        if (anyLevel.Length == 2)
                        {
                            data.stepMoneyType = int.Parse(anyLevel[0]);
                            data.stepMoneyNum = int.Parse(anyLevel[1]);
                        }
                    }
                    else if (xe.Name == "evolveLv")
                    {
                        data.evolveLv = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveMateria")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ":");

                        if (anyLevel.Length != 3)
                            data.evolveMateria = new KeyValuePair<uint, int>(0, 0);
                        else
                        {
                            data.evolveMateria = new KeyValuePair<uint, int>(uint.Parse(anyLevel[0]), int.Parse(anyLevel[1]));
                            data.evolveMaterialStep = int.Parse(anyLevel[2]);
                        }
                    }
                    else if (xe.Name == "sellType")
                    {
                        data.sellType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sellNum")
                    {
                        data.sellNum = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "lastId")
                    {
                        data.lastId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveId")
                    {
                        data.evolveId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.CompareTo("initiativeSkill") == 0)
                    {
                        data.initiativeSkill = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "passivitySkill")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.passivitySkill = new List<uint>(anyLevel.Length + 1);
                        foreach (var tp in anyLevel)
                        {
                            if (data.passivitySkill.Contains(uint.Parse(tp))) continue;
                            data.passivitySkill.Add(uint.Parse(tp));
                        }
                    }
                    else if (xe.Name == "trajectoryType")
                    {
                        data.trajectoryType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "trajectory")
                    {
                        data.trajectory = xe.InnerText;
                    }
                    else if (xe.Name == "trajectoryMusic")
                    {
                        data.trajectoryMusic = xe.InnerText;
                    }
                    else if (xe.Name == "equipPos")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.PosList = new Dictionary<int, int>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.PosList.ContainsKey(int.Parse(tt[0]))) continue;

                            data.PosList.Add(int.Parse(tt[0]), int.Parse(tt[1]));
                        }
                    }
                    else if (xe.Name == "Descript")
                    {
                        data.Descript = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "materiaExp")
                    {
                        data.materiaExp = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "exp_max")
                    {

                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.StrengthExp = new Dictionary<int, int>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {

                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.StrengthExp.ContainsKey(int.Parse(tt[0]))) continue;
                            data.StrengthExp.Add(int.Parse(tt[0]), int.Parse(tt[1]));
                        }
                    }
                    else if (xe.Name == "type")
                    {
                        int type = 0;
                        if (int.TryParse(xe.InnerText, out type))
                        {
                            data.type = type;
                        }
                        else
                        {
                            Debug.LogError("Error Type:" + xe.InnerText);
                        }
                    }
                    else if (xe.Name == "normalSpeed")
                    {
                        data.NormalSpeed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "normalInterval")
                    {
                        data.NormalInterval = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "SoldierPos")
                    {
                        data.SoldierPos = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "SoldierStory")
                    {
                        data.SoldierStory = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "recyclePrice")
                    {
                        data.recyclePrice = CommonFunction.ParseMoneyFlowData(xe.InnerText);
                    }
                    else if(xe.Name == "AttributeBias")
                    {
                        data.AttributeBias = xe.InnerText;
                    }
                }
                _soldierAttributeList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load soldierAttribute XML Error:" + ex.Message + ex.StackTrace);
        }
    }
}