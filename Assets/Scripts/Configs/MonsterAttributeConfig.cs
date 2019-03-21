
using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;

public class MonsterAttributeInfo 
{
	/// <summary>
	/// 怪物ID
	/// </summary>
	public uint ID;
	/// <summary>
	/// 怪物名称
	/// </summary>
	public string Name;
	/// <summary>
	/// 怪物星级
	/// </summary>
	public int Star;
    /// <summary>
    /// 怪物类别[0-小怪 1-BOSS 2-超级兵]
    /// </summary>
    public int IsBoss;
	/// <summary>
	/// 怪物类别
	/// </summary>
    public int Level;
	/// <summary>
	/// 怪物站位
	/// </summary>
    public int Seat;
	/// <summary>
	/// 职业ID
	/// </summary>
    public int JobID;
	/// <summary>
	/// 头像ID
	/// </summary>
	public string HeadID;
	/// <summary>
	/// 动画ID
	/// </summary>
	public string ResourceID;
	/// <summary>
	/// 音效ID
	/// </summary>
	public string SoundID;
	/// <summary>
	/// 生命
	/// </summary>
	public int HP;
	/// <summary>
	/// 攻击
	/// </summary>
    public int Attack;
	/// <summary>
	/// 命中
	/// </summary>
    public int Accuracy;
	/// <summary>
	/// 闪避
	/// </summary>
    public int Dodge;
	/// <summary>
	/// 暴击
	/// </summary>
    public int Crit;
	/// <summary>
	/// 韧性
	/// </summary>
    public int Tenacity;
	/// <summary>
	/// 攻击距离
	/// </summary>
    public int AttDistance;
	/// <summary>
	/// 攻击间隔[毫秒]
	/// </summary>
    public int AttRate;
	/// <summary>
	/// 移动速度
	/// </summary>
    public int Speed;
	/// <summary>
	/// 主动技能ID
	/// </summary>
	public List<uint> Skill;
    /// <summary>
    /// 被动技能
    /// </summary>
    public List<uint> PassivitySkill;
    /// <summary>
    /// 弹道类型
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
    /// 标准移动速度[像素]
    /// </summary>
    public int NormalSpeed;
    /// <summary>
    /// 标准攻击延迟[毫秒]
    /// </summary>
    public int NormalInterval;
    /// <summary>
    /// 怪物描述
    /// </summary>
    public string Desc;

    public void Init()
    {
        ID = 0;
	    Name = string.Empty;
	    Star = 0;
        IsBoss = 0;
	    Level = 0;
	    Seat = 0;
	    JobID = 0;
        HeadID = string.Empty;
        ResourceID = string.Empty;
        SoundID = string.Empty;
	    HP = 0;
	    Attack = 0;
	    Accuracy = 0;
	    Dodge = 0;
	    Crit = 0;
	    Tenacity = 0;
	    AttDistance = 0;
	    AttRate = 0;
	    Speed = 0;
        if (Skill == null)
            Skill = new List<uint>();
        Skill.Clear();
        if (PassivitySkill == null)
            PassivitySkill = new List<uint>();
        PassivitySkill.Clear();
        trajectoryType = 0;
        trajectory = string.Empty;
        trajectoryMusic = string.Empty;
        NormalSpeed = 0;
        NormalInterval = 0;
        Desc = string.Empty;
    }

    public void CopyTo(MonsterAttributeInfo vInfo)
    {
        if (vInfo == null)
            return;
        Init();

        this.ID = vInfo.ID;
        this.Name = vInfo.Name;
        this.Star = vInfo.Star;
        this.IsBoss = vInfo.IsBoss;
        this.Level = vInfo.Level;
        this.Seat = vInfo.Seat;
        this.JobID = vInfo.JobID;
        this.HeadID = vInfo.HeadID;
        this.ResourceID = vInfo.ResourceID;
        this.SoundID = vInfo.SoundID;
        this.HP = vInfo.HP;
        this.Attack = vInfo.Attack;
        this.Accuracy = vInfo.Accuracy;
        this.Dodge = vInfo.Dodge;
        this.Crit = vInfo.Crit;
        this.Tenacity = vInfo.Tenacity;
        this.AttDistance = vInfo.AttDistance;
        this.AttRate = vInfo.AttRate;
        this.Speed = vInfo.Speed;
        this.Skill.AddRange(vInfo.Skill);
        this.PassivitySkill.AddRange(vInfo.PassivitySkill);
        this.trajectoryType = vInfo.trajectoryType;
        this.trajectory = vInfo.trajectory;
        this.trajectoryMusic = vInfo.trajectoryMusic;
        this.NormalSpeed = vInfo.NormalSpeed;
        this.NormalInterval = vInfo.NormalInterval;
        this.Desc = vInfo.Desc;
    }
}

public class MonsterAttributeConfig : BaseConfig 
{
	private List<MonsterAttributeInfo> _monsterAttributeList;

	public MonsterAttributeConfig()
	{
		_monsterAttributeList = new List<MonsterAttributeInfo>();
        Initialize(GlobalConst.Config.DIR_XML_MONSTERATTRIBUTE, ParseConfig);
	}

	public override void Initialize(string vFileName, System.Action<string> callback) 
	{
        base.Initialize(vFileName, callback);	
	}

	public void Uninitialize()
	{
		 if (_monsterAttributeList  != null)
		{
			_monsterAttributeList.Clear();
			_monsterAttributeList = null;
		}
	}

	public List<MonsterAttributeInfo> GetMonsterAttributeList()
	{
		return _monsterAttributeList;
	}

    /// <summary>
    /// 通过ID获取怪物信息
    /// </summary>
    /// <param name="vID"></param>
    /// <returns></returns>
    public MonsterAttributeInfo GetMonsterAttributeByID(uint vID)
    {
        if (_monsterAttributeList == null)
            return null;
        for (int i = 0; i < _monsterAttributeList.Count; i++)
        {
            if (vID == _monsterAttributeList[i].ID)
                return _monsterAttributeList[i];
        }
        return null;
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
                //Debug.LogWarning("----------------------------------------------------------------------------------");
				MonsterAttributeInfo data = new MonsterAttributeInfo();
	 			foreach (XmlElement xe in Element)
                {
                    //Debug.LogWarning(string.Format("Monster: [{0}, {1}]", xe.Name, xe.InnerText));
					if (xe.Name == "ID")
                    {
						data.ID = uint.Parse(xe.InnerText);
					}
					else if (xe.Name == "Name")
					{
						data.Name = xe.InnerText;
					}
					else if (xe.Name == "Star")
					{
                        data.Star = int.Parse(xe.InnerText);
					}
                    else if (xe.Name == "IsBoss")
                    {
                        data.IsBoss = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Level")
                    {
                        data.Level = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Seat")
                    {
                        data.Seat = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "JobID")
                    {
                        data.JobID = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "HeadID")
                    {
                        data.HeadID = xe.InnerText;
                    }
                    else if (xe.Name == "ResourceID")
                    {
                        data.ResourceID = xe.InnerText;
                    }
                    else if (xe.Name == "Sound")
                    {
                        data.SoundID = xe.InnerText;
                    }
                    else if (xe.Name == "HP")
                    {
                        data.HP = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Attack")
                    {
                        data.Attack = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Accuracy")
                    {
                        data.Accuracy = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Dodge")
                    {
                        data.Dodge = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Crit")
                    {
                        data.Crit = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Tenacity")
                    {
                        data.Tenacity = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "AttDistance")
                    {
                        data.AttDistance = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "AttRate")
                    {
                        //Debug.LogWarning(string.Format("Monster: [{0}, {1}]", xe.Name, xe.InnerText));
                        data.AttRate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "Speed")
                    {
                        data.Speed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "initiativeSkill")
                    {
                        if (data.Skill == null)
                            data.Skill = new List<uint>();
                        data.Skill.Clear();

                        string[] tmpArray = xe.InnerText.Split(';');
                        if ((tmpArray != null) && (tmpArray.Length > 0))
                        {
                            for (int i = 0; i < tmpArray.Length; i++)
                            {
                                data.Skill.Add(uint.Parse(tmpArray[i]));
                            }
                        }
                    }
                    else if (xe.Name == "passivitySkill")
                    {
                        if (data.PassivitySkill == null)
                            data.PassivitySkill = new List<uint>();
                        data.PassivitySkill.Clear();

                        string[] tmpArray = xe.InnerText.Split(';');
                        if ((tmpArray != null) && (tmpArray.Length > 0))
                        {
                            for (int i = 0; i < tmpArray.Length; i++)
                            {
                                data.PassivitySkill.Add(uint.Parse(tmpArray[i]));
                            }
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
                    else if (xe.Name == "normalSpeed")
                    {
                        data.NormalSpeed = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "normalInterval")
                    {
                        data.NormalInterval = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name == "desc")
                    {
                        data.Desc = xe.InnerText;
                    }
				}
				_monsterAttributeList.Add(data);
			}

        	base.LoadCompleted();
		}

        catch (Exception ex)
        {
            Debug.LogError("Load monsterAttribute XML Error:" + ex.Message + ex.StackTrace);
        }
    }
}