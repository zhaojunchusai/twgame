using System;
using System.Collections;
using System.IO;
using System.Xml;
using UnityEngine;
using System.Collections.Generic;


public class CombatPetInfo
{
    public uint id;

    public string name;

    public int quality;
    /// <summary>
    /// 初始等级
    /// </summary>
    public int initlevel;
    /// <summary>
    /// 变更等级
    /// </summary>
    public uint changeLevel;
    /// <summary>
    /// 后置宠物ID
    /// </summary>
    public uint suffixPetid;
    public string skill_anim;
    public string effect_name;
    /// <summary>
    /// 界面缩放比例
    /// </summary>
    public float view_scale;
    /// <summary>
    /// 资源名称
    /// </summary>
    public string res_name;
    public string icon;

    public int hp_max;
    public float u_hp;
    public int phy_atk;
    public float u_attack;
    /// <summary>
    /// 命中
    /// </summary>
    public int acc_rate;
    /// <summary>
    /// 命中成长
    /// </summary>
    public float u_accuracy;
    /// <summary>
    /// 闪避
    /// </summary>
    public int ddg_rate;
    /// <summary>
    /// 闪避成长
    /// </summary>
    public float u_dodge;
    /// <summary>
    /// 暴击
    /// </summary>
    public int crt_rate;
    /// <summary>
    /// 暴击成长
    /// </summary>
    public float u_crit;
    /// <summary>
    /// 韧性
    /// </summary>
    public int tnc_rate;
    /// <summary>
    /// 韧性成长
    /// </summary>
    public float u_tenacity;
    /// <summary>
    /// 强化包ID
    /// </summary>
    public List<PetCommonMaterilData> strengthCostBag;
    /// <summary>
    /// 技能ID
    /// </summary>
    public uint skillID;

    public string pet_desc;
    /// <summary>
    /// 解锁描述
    /// </summary>
    public string unlock_desc;

    public uint viewType;
    /// <summary>
    /// 宠物ID
    /// </summary>
    public uint PetID;

}

public class CombatPetsConfig : BaseConfig
{

    private List<CombatPetInfo> mPetList;

    public CombatPetsConfig()
    {
        mPetList = new List<CombatPetInfo>();
        Initialize(GlobalConst.Config.DIR_XML_COMBATPETS, ParseConfig);
    }

    public override void Initialize(string vFileName, Action<string> callback)
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
            foreach (XmlElement Element in nodelist)
            {
                CombatPetInfo data = new CombatPetInfo();
                foreach (XmlElement xe in Element)
                {
                    if (xe.Name.Equals("id"))
                    {
                        data.id = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("Name"))
                    {
                        data.name = xe.InnerText;
                    }
                    else if (xe.Name.Equals("Quality"))
                    {
                        data.quality = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("initLevel"))
                    {
                        data.initlevel = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("changeLevel"))
                    {
                        data.changeLevel = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("suffixPetid"))
                    {
                        data.suffixPetid = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("skill_anim"))
                    {
                        data.skill_anim = xe.InnerText;
                    }
                    else if (xe.Name.Equals("effect_name"))
                    {
                        data.effect_name = xe.InnerText;
                    }
                    else if (xe.Name.Equals("view_scale"))
                    {
                        data.view_scale = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("res_name"))
                    {
                        data.res_name = xe.InnerText;
                    }
                    else if (xe.Name.Equals("icon"))
                    {
                        data.icon = xe.InnerText;
                    }
                    else if (xe.Name.Equals("hp_max"))
                    {
                        data.hp_max = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_hp"))
                    {
                        data.u_hp = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("phy_atk"))
                    {
                        data.phy_atk = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_attack"))
                    {
                        data.u_attack = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("acc_rate"))
                    {
                        data.acc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_accuracy"))
                    {
                        data.u_accuracy = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("ddg_rate"))
                    {
                        data.ddg_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_dodge"))
                    {
                        data.u_dodge = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("crt_rate"))
                    {
                        data.crt_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_crit"))
                    {
                        data.u_crit = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("tnc_rate"))
                    {
                        data.tnc_rate = int.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("u_tenacity"))
                    {
                        data.u_tenacity = float.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("costbag"))
                    {
                        data.strengthCostBag = new List<PetCommonMaterilData>();
                        string[] matArr = xe.InnerText.Split(';');
                        if (matArr == null)
                            continue;
                        for (int i = 0; i < matArr.Length; i++)
                        {
                            string[] mat = matArr[i].Split(':');
                            if (mat == null || mat.Length < 2)
                                continue;
                            PetCommonMaterilData mData = new PetCommonMaterilData();
                            mData.num = int.Parse(mat[0]);
                            mData.materailID = uint.Parse(mat[1]);
                            data.strengthCostBag.Add(mData);
                        }
                    }
                    else if (xe.Name.Equals("skillID"))
                    {
                        data.skillID = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("pet_desc"))
                    {
                        data.pet_desc = xe.InnerText;
                    }
                    else if (xe.Name.Equals("unlock_desc"))
                    {
                        data.unlock_desc = xe.InnerText;
                    }
                    else if (xe.Name.Equals("viewtype"))
                    {
                        data.viewType = uint.Parse(xe.InnerText);
                    }
                    else if (xe.Name.Equals("petID"))
                    {
                        data.PetID = uint.Parse(xe.InnerText);
                    }
                }

                mPetList.Add(data);
            }
            base.LoadCompleted();
        }
        catch (Exception ex)
        {
            base.LoadCompleted();
            Debug.LogError("Load CombatPets XML Error:" + ex.Message);
        }
    }

    public List<CombatPetInfo> GetPetInfos()
    {
        return mPetList;
    }

    public CombatPetInfo GetPetInfoByID(uint id)
    {
        CombatPetInfo info = mPetList.Find((tmp) =>
        {
            if (tmp == null)
                return false;
            return tmp.id == id;
        });
        if (info == null)
            Debug.LogError("can not get CombatPetInfo by ID: " + id.ToString());
        return info;
    }


    public List<CombatPetInfo> GetInitPetInfos()
    {
        List<CombatPetInfo> infos = mPetList.FindAll((tmp) =>
          {
              if (tmp == null)
                  return false;
              return tmp.initlevel == 1;
          });
        if (infos == null)
            Debug.LogError("can not get InitPetInfos ");
        return infos;
    }
}
