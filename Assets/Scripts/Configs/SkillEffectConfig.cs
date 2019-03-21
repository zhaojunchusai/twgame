using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;
using System.Text.RegularExpressions;
public class TargetFindInfo
{
    /// <summary>
    /// 效果范围
    /// </summary>
    public int rang;
    /// <summary>
    /// 数量上限
    /// </summary>
    public int maxTarget;
    /// <summary>
    /// 搜索器是否跟随目标
    /// </summary>
    public int followTarget;
    /// <summary>
    /// 搜索器搜索频率
    /// </summary>
    public int frequencyTime;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int durTime;
    /// <summary>
    /// 目标搜索逻辑
    /// </summary>
    public int FindModel;
}
public class TargetLockonInfo
{
    /// <summary>
    /// 目标距离下限
    /// </summary>
    public int minDistance;
    /// <summary>
    /// 目标距离上限
    /// </summary>
    public int maxDistance;
    /// <summary>
    /// 目标阵营选择
    /// </summary>
    public int targetType;
    /// <summary>
    /// 目标选择逻辑
    /// </summary>
    public int targetMoudle;
}
public class SkillSpecilInfo
{
    /// <summary>
    /// 目标特效层级
    /// </summary>
    public int sortLayer;
    /// <summary>
    /// 时间修正
    /// </summary>
    public int timeDelay;
    /// <summary>
    /// 持续时间
    /// </summary>
    public int during;
    /// <summary>
    /// 目标特效名称
    /// </summary>
    public string effectName;
    /// <summary>
    /// 目标特效位置
    /// </summary>
    public int effectType;
    /// <summary>
    /// 目标特效音效名称
    /// </summary>
    public string effectMusic;
    /// <summary>
    /// 特效是否跟随
    /// </summary>
    public int followSpecil;
    /// <summary>
    /// 没有目标
    /// </summary>
    public int noTarget;
}
public class SkillEffectInfo
{
    /// <summary>
    /// 技能编号
    /// </summary>
    public uint id;
    /// <summary>
    /// 目标锁定
    /// </summary>
    public TargetLockonInfo targetLockonInfo;
    /// <summary>
    /// 目标搜寻
    /// </summary>
    public TargetFindInfo targetFindInfo;
    /// <summary>
    /// 效果器编号 效果器时间修正
    /// </summary>
    public List<KeyValuePair<int, int>> effect;
    /// <summary>
    /// 技能特效1
    /// </summary>
    public SkillSpecilInfo tempSpecilInfo1;
    /// <summary>
    /// 技能特效2
    /// </summary>
    public SkillSpecilInfo tempSpecilInfo2;
    /// <summary>
    /// 技能特效3
    /// </summary>
    public SkillSpecilInfo tempSpecilInfo3;
    /// <summary>
    /// 技能特效容器
    /// </summary>
    public List<SkillSpecilInfo> specilInfoList;
}
public class SkillEffectConfig : BaseConfig
{
    private List<SkillEffectInfo> _skillEffectList;

    public SkillEffectConfig()
    {
        _skillEffectList = new List<SkillEffectInfo>();
        Initialize(GlobalConst.Config.DIR_XML_SKILLEFFECT, ParseConfig);
    }

    public override void Initialize(string vFileName, System.Action<string> callback)
    {
        base.Initialize(vFileName, callback);
    }
    public List<SkillEffectInfo> FindAllByID(uint id)
    {
        return _skillEffectList.FindAll((SkillEffectInfo info) => { if (info == null)return false; return info.id == id; });
    }
    public void Uninitialize()
    {
        if (_skillEffectList != null)
        {
            _skillEffectList.Clear();
            _skillEffectList = null;
        }
    }

    public List<SkillEffectInfo> GetSkillEffectList()
    {
        return _skillEffectList;
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
                SkillEffectInfo data = new SkillEffectInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name == "id")
                    {
                        if (data.targetFindInfo == null)
                            data.targetFindInfo = new TargetFindInfo();
                        if (data.targetLockonInfo == null)
                            data.targetLockonInfo = new TargetLockonInfo();
                        if (data.tempSpecilInfo1 == null)
                        {
                            data.tempSpecilInfo1 = new SkillSpecilInfo();
                            data.tempSpecilInfo2 = new SkillSpecilInfo();
                            data.tempSpecilInfo3 = new SkillSpecilInfo();
                        }
                        if (data.specilInfoList == null)
                            data.specilInfoList = new List<SkillSpecilInfo>(4);
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "minDistance")
                    {

                        data.targetLockonInfo.minDistance = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "maxDistance")
                    {
                        data.targetLockonInfo.maxDistance = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "targetType")
                    {
                        data.targetLockonInfo.targetType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "targetMoudle")
                    {
                        data.targetLockonInfo.targetMoudle = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "rang")
                    {
                        data.targetFindInfo.rang = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "maxTarget")
                    {
                        data.targetFindInfo.maxTarget = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "frequencyTime")
                    {
                        data.targetFindInfo.frequencyTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "followTarget")
                    {
                        data.targetFindInfo.followTarget = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "findTime")
                    {
                        data.targetFindInfo.durTime = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effect")
                    {
                        string temp = xe.InnerText;
                        string[] anyLevel = Regex.Split(temp, ";");

                        data.effect = new List<KeyValuePair<int, int>>(anyLevel.Length + 1);

                        foreach (var tp in anyLevel)
                        {
                            string[] tt = Regex.Split(tp, ":");
                            if (tt.Length < 2) continue;
                            KeyValuePair<int, int> ky = new KeyValuePair<int, int>(int.Parse(tt[1]), int.Parse(tt[0]));
                            if (data.effect.Contains(ky)) continue;
                            data.effect.Add(ky);
                        }
                    }
                    else if (xe.Name == "sortLayer1")
                    {
                        data.tempSpecilInfo1.sortLayer = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "timeDelay1")
                    {
                        data.tempSpecilInfo1.timeDelay = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "during1")
                    {
                        data.tempSpecilInfo1.during = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectName1")
                    {
                        data.tempSpecilInfo1.effectName = xe.InnerText;
                    }
                    else if (xe.Name == "effectType1")
                    {
                        data.tempSpecilInfo1.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectMusic1")
                    {
                        data.tempSpecilInfo1.effectMusic = xe.InnerText;
                    }
                    else if (xe.Name == "followSpecil1")
                    {
                        data.tempSpecilInfo1.followSpecil = int.Parse(xe.InnerText);
                        if (data.tempSpecilInfo1.effectName != "0")
                            data.specilInfoList.Add(data.tempSpecilInfo1);
                    }
                    else if (xe.Name == "noTarget1")
                    {
                        data.tempSpecilInfo1.noTarget = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sortLayer2")
                    {
                        data.tempSpecilInfo2.sortLayer = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "timeDelay2")
                    {
                        data.tempSpecilInfo2.timeDelay = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "during2")
                    {
                        data.tempSpecilInfo2.during = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectName2")
                    {
                        data.tempSpecilInfo2.effectName = xe.InnerText;
                    }
                    else if (xe.Name == "effectType2")
                    {
                        data.tempSpecilInfo2.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectMusic2")
                    {
                        data.tempSpecilInfo2.effectMusic = xe.InnerText;
                    }
                    else if (xe.Name == "followSpecil2")
                    {
                        data.tempSpecilInfo2.followSpecil = int.Parse(xe.InnerText);
                        if (data.tempSpecilInfo2.effectName != "0")
                            data.specilInfoList.Add(data.tempSpecilInfo2);
                    }
                    else if (xe.Name == "noTarget2")
                    {
                        data.tempSpecilInfo2.noTarget = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "sortLayer3")
                    {
                        data.tempSpecilInfo3.sortLayer = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "timeDelay3")
                    {
                        data.tempSpecilInfo3.timeDelay = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "during3")
                    {
                        data.tempSpecilInfo3.during = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectName3")
                    {
                        data.tempSpecilInfo3.effectName = xe.InnerText;
                    }
                    else if (xe.Name == "effectType3")
                    {
                        data.tempSpecilInfo3.effectType = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "effectMusic3")
                    {
                        data.tempSpecilInfo3.effectMusic = xe.InnerText;
                    }
                    else if (xe.Name == "followSpecil3")
                    {
                        data.tempSpecilInfo3.followSpecil = int.Parse(xe.InnerText);
                        if (data.tempSpecilInfo3.effectName != "0")
                            data.specilInfoList.Add(data.tempSpecilInfo3);
                    }
                    else if (xe.Name == "noTarget3")
                    {
                        data.tempSpecilInfo3.noTarget = int.Parse(xe.InnerText);
                    }
                }
                _skillEffectList.Add(data);
            }

            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            Debug.LogError("Load skillEffect XML Error:" + ex.Message);
        }
    }
}