using UnityEngine;
using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class EffectInfo : ICloneable
{
    /// <summary>
    /// 效果编号
    /// </summary>
    public int effectId;
    /// <summary>
    /// 效果类型0=无类型；1=buff；2=debuff；3=光环
    /// </summary>
    public int effectType;
    /// <summary>
    /// 效果值N1
    /// </summary>
    public float num;
    /// <summary>
    /// 效果值N2
    /// </summary>
    public float percent;
    /// <summary>
    /// N1成长值
    /// </summary>
    public float u_num;
    /// <summary>
    /// N2成长值
    /// </summary>
    public float u_percent;
    /// <summary>
    /// 效果每跳间隔
    /// </summary>
    public float interval;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int durTime;
    /// <summary>
    /// 持续时间等级成长
    /// </summary>
    public int u_durTime;

    public object Clone()
    {
        EffectInfo info = (EffectInfo)this.MemberwiseClone();
        return info;
    }
    public void LevelAdd(int level)
    {
        this.num += (this.u_num * level);
        this.percent += (this.u_percent * level);
        this.durTime += (this.u_durTime * level);
    }
}
public class SkillAttributeInfo
{
    /// <summary>
    /// 技能ID
    /// </summary>
    public uint nId;
    /// <summary>
    /// 技能名称
    /// </summary>
    public string Name;
    /// <summary>
    /// 技能品质
    /// </summary>
    public int Quality;
    /// <summary>
    /// 初始等级
    /// </summary>
    public int initLevel;
    /// <summary>
    /// 职业ID
    /// </summary>
    public int dependCareer;
    /// <summary>
    /// 图标ID
    /// </summary>
    public string Icon;
    /// <summary>
    /// 特效ID
    /// </summary>
    public string Effects;
    /// <summary>
    /// 音效ID
    /// </summary>
    public string Muisic;
    /// <summary>
    /// 触发类型
    /// </summary>
    public int triggerType;
    /// <summary>
    /// 触发概率
    /// </summary>
    public int triggerProb;
    /// <summary>
    /// 消耗魔法
    /// </summary>
    public int expendMagic;
    /// <summary>
    /// 冷却时间
    /// </summary>
    public int waitTime;
    /// <summary>
    /// 施法是否跟随
    /// </summary>
    public int conjureFollow;
    /// <summary>
    /// 施法特效延迟
    /// </summary>
    public float conjureTimeDelay;
    /// <summary>
    /// 施法特效挂点
    /// </summary>
    public int conjurePosition;
    /// <summary>
    /// 施法特效名称
    /// </summary>
    public string conjure;
    /// <summary>
    /// 施法音效名称
    /// </summary>
    public string conjureMusic;
    /// <summary>
    /// 没有目标
    /// </summary>
    public int noTarget;
    /// <summary>
    /// 弹道延迟时间
    /// </summary>
    public int conjureDelay;
    /// <summary>
    /// 弹道挂点
    /// </summary>
    public int trajectoryPosition;
    /// <summary>
    /// 弹道速度
    /// </summary>
    public float speed;
    /// <summary>
    /// 弹道特效名称
    /// </summary>
    public string trajectory;
    /// <summary>
    /// 弹道类型编号
    /// </summary>
    public int trajectoryType;
    /// <summary>
    /// 弹道音效名称
    /// </summary>
    public string trajectoryMusic;
    /// <summary>
    /// 效果器1
    /// </summary>
    public EffectInfo effect1;
    /// <summary>
    /// 效果器2
    /// </summary>
    public EffectInfo effect2;
    /// <summary>
    /// 效果器3
    /// </summary>
    public EffectInfo effect3;
    /// <summary>
    /// 效果器容器
    /// </summary>
    public List<EffectInfo> effect;
    /// <summary>
    /// 强化等级上限
    /// </summary>
    public int levelLimit;
    /// <summary>
    /// 前置ID
    /// </summary>
    public uint prepositionId;
    /// <summary>
    /// 后置技能
    /// </summary>
    public uint evolveId;
    /// <summary>
    /// 强化类型
    /// </summary>
    public int strengthType;
    /// <summary>
    /// 技能描述
    /// </summary>
    public string Description;
    /// <summary>
    /// 强化消耗材料包
    /// </summary>
    public Dictionary<int, uint> materialBag;
    /// <summary>
    /// 震屏延迟时间
    /// </summary>
    public float ShakeDelayTime;
    /// <summary>
    /// 震屏持续时间
    /// </summary>
    public float ShakeDurationTime;
    /// <summary>
    /// 基础战力
    /// </summary>
    public float CombatPoint;
    /// <summary>
    /// 战力成长
    /// </summary>
    public float CombatGrow;

}

public class SkillAttributeConfig : BaseConfig
{
    private List<SkillAttributeInfo> _skillAttributeList;

    public SkillAttributeConfig()
    {
        _skillAttributeList = new List<SkillAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SKILLATTRIBUTE, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }

    public void Uninitialize()
    {
        if (_skillAttributeList != null)
        {
            _skillAttributeList.Clear();
            _skillAttributeList = null;
        }
    }

    public List<SkillAttributeInfo> GetSkillAttributeList()
    {
        return _skillAttributeList;
    }
    public SkillAttributeInfo FindById(uint id)
    {
        return _skillAttributeList.Find((SkillAttributeInfo temp) => { if (temp == null)return false; return temp.nId == id; });
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
                SkillAttributeInfo data = new SkillAttributeInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "nId")
                    {
                        data.nId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Name")
                    {
                        data.Name = xe.InnerText;
                    }
                    else if (xe.Name == "Quality")
                    {
                        data.Quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "initLevel")
                    {
                        data.initLevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "dependCareer")
                    {
                        data.dependCareer = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Icon")
                    {
                        data.Icon = xe.InnerText;
                    }
                    else if (xe.Name == "Effects")
                    {
                        data.Effects = xe.InnerText;
                    }
                    else if (xe.Name == "Muisic")
                    {
                        data.Muisic = xe.InnerText;
                    }
                    else if (xe.Name == "triggerType")
                    {
                        data.triggerType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "triggerProb")
                    {
                        data.triggerProb = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "expendMagic")
                    {
                        data.expendMagic = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "waitTime")
                    {
                        data.waitTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "conjureFollow")
                    {
                        data.conjureFollow = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "conjureTimeDelay")
                    {
                        int time = int.Parse(xe.InnerText);
                        data.conjureTimeDelay = (float)time / 1000.0f;
                    }
                    else if (xe.Name == "conjurePosition")
                    {
                        data.conjurePosition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "conjure")
                    {
                        data.conjure = xe.InnerText;
                    }
                    else if (xe.Name == "conjureMusic")
                    {
                        data.conjureMusic = xe.InnerText;
                    }
                    else if (xe.Name == "noTarget")
                    {
                        data.noTarget = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "conjureDelay")
                    {
                        data.conjureDelay = int.Parse(xe.InnerText);
                    }
                    else if(xe.Name == "trajectoryPosition")
                    {
                        data.trajectoryPosition = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "trajectory")
                    {
                        data.trajectory = xe.InnerText;
                    }
                    else if (xe.Name == "trajectoryType")
                    {
                        data.trajectoryType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "trajectoryMusic")
                    {
                        data.trajectoryMusic = xe.InnerText;
                    }
                    else if(xe.Name == "speed")
                    {
                        data.speed = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectId1")
                    {
                        data.effect = new List<EffectInfo>(3);
                        
                        data.effect1 = new EffectInfo();
                        data.effect1.effectId = int.Parse(xe.InnerText);
                        data.effect.Add(data.effect1);
                    }
                    else if (xe.Name == "effectType1")
                    {
                        data.effect1.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "num1")
                    {
                        data.effect1.num = float.Parse(xe.InnerText);                 
                    }
                    else if (xe.Name == "percent1")
                    {
                        data.effect1.percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_num1")
                    {
                        data.effect1.u_num = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_percent1")
                    {
                        data.effect1.u_percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "interval1")
                    {
                        data.effect1.interval = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "durTime1")
                    {
                        data.effect1.durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_durTime1")
                    {
                        data.effect1.u_durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectId2")
                    {
                        data.effect2 = new EffectInfo();
                        data.effect2.effectId = int.Parse(xe.InnerText);
                        data.effect.Add(data.effect2);
                    }
                    else if (xe.Name == "effectType2")
                    {
                        data.effect2.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "num2")
                    {
                        data.effect2.num = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "percent2")
                    {
                        data.effect2.percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_num2")
                    {
                        data.effect2.u_num = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_percent2")
                    {
                        data.effect2.u_percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "interval2")
                    {
                        data.effect2.interval = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "durTime2")
                    {
                        data.effect2.durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_durTime2")
                    {
                        data.effect2.u_durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectId3")
                    {
                        data.effect3 = new EffectInfo();
                        data.effect3.effectId = int.Parse(xe.InnerText);
                        data.effect.Add(data.effect3);
                    }
                    else if (xe.Name == "effectType3")
                    {
                        data.effect3.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "num3")
                    {
                        data.effect3.num = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "percent3")
                    {
                        data.effect3.percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_num3")
                    {
                        data.effect3.u_num = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_percent3")
                    {
                        data.effect3.u_percent = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "interval3")
                    {
                        data.effect3.interval = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "durTime3")
                    {
                        data.effect3.durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "u_durTime3")
                    {
                        data.effect3.u_durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "levelLimit")
                    {
                        data.levelLimit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "prepositionId")
                    {
                        data.prepositionId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "evolveId")
                    {
                        data.evolveId = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "strengthType")
                    {
                        data.strengthType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Description")
                    {
                        data.Description = CommonFunction.ReplaceEscapeChar(xe.InnerText);
                    }
                    else if (xe.Name == "materialBags")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.materialBag = new Dictionary<int, uint>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            if (data.materialBag.ContainsKey(int.Parse(tt[0]))) continue;
                            data.materialBag.Add(int.Parse(tt[0]), uint.Parse(tt[1]));
                        }
                    }
                    else if (xe.Name == "ShakeDelayTime")
                    {
                        int tmpData = int.Parse(xe.InnerText);
                        data.ShakeDelayTime = (float)tmpData / 1000.0f;
                    }
                    else if (xe.Name == "ShakeDurationTime")
                    {
                        int tmpData = int.Parse(xe.InnerText);

                        data.ShakeDurationTime = (float)tmpData / 1000.0f;
                    }
                    else if (xe.Name == "Combatpoint")
                    {
                        if (!float.TryParse(xe.InnerText, out data.CombatPoint))
                            data.CombatPoint = 0;
                    }
                    else if (xe.Name == "Combatgrow")
                    {
                        if (!float.TryParse(xe.InnerText, out data.CombatGrow))
                            data.CombatGrow = 0;
                    }
                }

                _skillAttributeList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load skillAttribute XML Error:" + ex.Message);
        }
    }
}